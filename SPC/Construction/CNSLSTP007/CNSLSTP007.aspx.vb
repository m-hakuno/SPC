'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　作業予定一覧
'*　ＰＧＭＩＤ：　CNSLSTP007
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.24　：　酒井
'********************************************************************************************************************************
'CNSLSTP007_001     2016.01.25      伯野　    ＦＳの会社名を直打ちからマスタ取得に変更
'CNSLSTP007_002     2016.02.01      加賀      レイアウト変更


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

#End Region

Public Class CNSLSTP007

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
    '作業予定一覧 画面のパス
    Const sCnsCNSLSTP001 As String = "~/" & P_CNS & "/" &
                                        P_FUN_CNS & P_SCR_LST & P_PAGE & "007.aspx"

    Const sCnsProgid As String = "CNSLSTP007"
    'Const sCnsSqlid As String = "CNSLSTP007_S1"
    'Const sCnsSqlid_2 As String = "CNSLSTP007_S2"
    'Const sCnsSqlid_3 As String = "CNSLSTP007_S3"
    '--------------------------------
    '2014/07/11 星野　ここから
    '--------------------------------
    Const sCnsSqlid_4 As String = "CNSLSTP007_S4"
    Const sCnsSqlid_5 As String = "CNSLSTP007_S5"
    Const sCnsSqlid_6 As String = "CNSLSTP007_S6"
    '--------------------------------
    '2014/07/11 星野　ここまで
    '--------------------------------

    '一覧ボタン名
    Const sCnsbtnEigyou_Pr As String = "営業所印刷"
    Const sCnsbtnKouji_Pr As String = "工事印刷"

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

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        'pfSet_GridView(grvList, sCnsProgid)
        pfSet_GridView(Me.grvList, sCnsProgid, 36, 11)

    End Sub

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            'ボタンアクションの設定
            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click   '検索
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click   'クリア
            AddHandler Master.Master.ppRigthButton1.Click, AddressOf ButtonPr_Click  '営業所印刷
            AddHandler Master.Master.ppRigthButton2.Click, AddressOf ButtonPr_Click  '工事印刷

            If Not IsPostBack Then  '初回表示

                'ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False          'クリア
                Master.Master.ppRigthButton1.CausesValidation = False   '営業所印刷
                Master.Master.ppRigthButton2.CausesValidation = False   '工事印刷

                '「検索」「検索条件クリア」のボタン活性
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton2.Visible = True

                'ボタン表示設定
                Master.Master.ppRigthButton1.Text = sCnsbtnEigyou_Pr  '営業所印刷
                Master.Master.ppRigthButton2.Text = sCnsbtnKouji_Pr   '工事印刷

                'ボタン非活性設定
                Master.Master.ppRigthButton1.Enabled = False '営業所印刷
                Master.Master.ppRigthButton2.Enabled = False '工事印刷

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = sCnsProgid
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'グリッドの初期化
                Me.grvList.DataSource = New DataTable

                '件数を初期設定
                Master.ppCount = "0"

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'ヘッダ表示
                Me.grvList.DataBind()

                '保守チェックボックスへフォーカス
                cbxMaintenance.Focus()

                'ドロップダウンリスト設定
                msSetddl()

            End If

        Catch ex As Exception

            'メッセージ出力
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ' ''' <summary>
    ' ''' ユーザー権限
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    '    Select Case Session(P_SESSION_AUTH)
    '        Case "管理者"
    '        Case "SPC"
    '        Case "営業所"
    '        Case "NGC"
    '    End Select
    'End Sub

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        objStack = New StackFrame

        Try

            'ログ出力開始
            psLogStart(Me)

            Select Case sender.ID
                Case "btnSearchRigth1"        '検索ボタン押下
                    '個別エラーチェック.
                    'Call msCheck_Error()

                    If (Page.IsValid) Then

                        '画面ページ表示初期化
                        Master.ppCount = "0"
                        Me.grvList.DataSource = New DataTable

                        '条件検索取得
                        Me.msGet_Data(0)
                    End If
                Case "btnSearchRigth2"        '検索クリアボタン押下
                    '画面コントロール初期化
                    cbxMaintenance.Checked = False
                    cbxConstruction.Checked = False
                    'CNSLSTP007_002
                    'dftStartDt.ppFromText = String.Empty
                    dftStartDt.ppFromText = String.Empty
                    'dftStartDt.ppToText = String.Empty
                    dftStartDt.ppToText = String.Empty
                    'txtStateCd.ppText = String.Empty
                    ddlState.ppDropDownList.SelectedIndex = -1
                    'CNSLSTP007_002 END

                    '--------------------------------
                    '2014/07/09 星野　ここから
                    '--------------------------------
                    'cbxFswrkCls.Checked = True
                    cbxFsWrkCls.Checked = False
                    '--------------------------------
                    '2014/07/09 星野　ここまで
                    '--------------------------------
                    ddlNLCls.ppSelectedValue = String.Empty
                    txtPersonInCharge.ppText = String.Empty
                    txtOfficeCd.ppText = String.Empty
                    dtbDepartureFm.ppText = String.Empty
                    dtbDepartureFm.ppHourText = String.Empty
                    dtbDepartureFm.ppMinText = String.Empty
                    dtbDepartureTo.ppText = String.Empty
                    dtbDepartureTo.ppHourText = String.Empty
                    dtbDepartureTo.ppMinText = String.Empty
                    txtAddress.ppText = String.Empty
                    cbxNew.Checked = False
                    cbxExpansion.Checked = False
                    cbxSomeRemoval.Checked = False
                    cbxShopRelocation.Checked = False
                    cbxAllRemoval.Checked = False
                    cbxOnceRemoval.Checked = False
                    cbxConChange.Checked = False
                    cbxConDelivery.Checked = False
                    cbxVup.Checked = False
                    cbxReInstallation.Checked = False
                    cbxOther.Checked = False
                    cbxMaintenance.Focus()

            End Select

        Catch ex As Exception

            'メッセージ出力
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業予定一覧")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ButtonPr_Click(sender As System.Object, e As System.EventArgs)

        objStack = New StackFrame

        Try
            Select Case sender.ID
                Case "btnRigth1"        '営業所印刷押下
                    '営業所印刷未作成
                    Me.msGet_Data(1)
                Case "btnRigth2"        '工事印刷押下
                    '工事印刷未作成
                    Me.msGet_Data(2)
            End Select

        Catch ex As Exception
            'メッセージ出力
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim txtCells() As TextBox

        For Each grvrow As GridViewRow In grvList.Rows
            '改行を削除するカラムを設定
            txtCells = {DirectCast(grvrow.FindControl("ホール名"), TextBox) _
                      , DirectCast(grvrow.FindControl("住所"), TextBox) _
                      , DirectCast(grvrow.FindControl("その他内容"), TextBox) _
                      }

            '改行削除(CR+LF) ※任意で改行する場合は[CR][LF]いずれかを使用する
            For Each txtColumn As TextBox In txtCells
                txtColumn.Text = txtColumn.Text.Replace(Environment.NewLine, "")
            Next

        Next

        'For Each rowData As GridViewRow In grvList.Rows
        '    If Date.TryParse(CType(rowData.FindControl("開始日時"), TextBox).Text, dt) Then
        '        If (Date.Parse(CType(rowData.FindControl("開始日時"), TextBox).Text) - Date.Now) <= New TimeSpan(0, 30, 0) Then
        '            rowData.Cells(1).Enabled = False
        '            CType(rowData.FindControl("依頼番号"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("開始日時"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("TBOXID"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("ホール名"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("担当者"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("出発日時"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("住所"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("種別"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("機種"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("対象"), TextBox).ForeColor = Drawing.Color.Red
        '            CType(rowData.FindControl("データ件数"), TextBox).ForeColor = Drawing.Color.Red
        '        End If
        '    End If
        'Next
    End Sub



#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 条件検索取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal intPrintKbn As Integer)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim rpt As Object = Nothing
        Dim strFNm As String = String.Empty
        Dim strTime As New StringBuilder
        objStack = New StackFrame

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            Select Case intPrintKbn
                Case 0
                    'cmdDB = New SqlCommand(sCnsSqlid_1, conDB)
                    cmdDB = New SqlCommand(sCnsSqlid_4, conDB)

                Case 1  '作業予定表
                    'cmdDB = New SqlCommand(sCnsSqlid_2, conDB)
                    cmdDB = New SqlCommand(sCnsSqlid_6, conDB)

                    '帳票名称設定
                    strFNm = "作業予定表"
                    rpt = New CNSREP004

                Case 2  '工事予定一覧表
                    'cmdDB = New SqlCommand(sCnsSqlid_3, conDB)
                    cmdDB = New SqlCommand(sCnsSqlid_5, conDB)

                    '帳票名称設定
                    strFNm = "工事予定一覧表"
                    rpt = New CNSREP015
            End Select

            'パラメータ設定
            With cmdDB.Parameters

                '開始日FROM
                If dftStartDt.ppFromDate = Nothing Then
                    .Add(pfSet_Param("StartDt_From", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("StartDt_From", SqlDbType.DateTime, dftStartDt.ppFromDate))
                End If

                '開始日TO
                If dftStartDt.ppToDate = Nothing Then
                    .Add(pfSet_Param("StartDt_To", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("StartDt_To", SqlDbType.DateTime, dftStartDt.ppToDate))
                End If

                '出発日時FROM
                If dtbDepartureFm.ppDate = Nothing Then
                    .Add(pfSet_Param("DepartureDt_From", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("DepartureDt_From", SqlDbType.DateTime, dtbDepartureFm.ppDate))
                End If

                '出発日時TO
                If dtbDepartureTo.ppDate = Nothing Then
                    .Add(pfSet_Param("DepartureDt_To", SqlDbType.NVarChar, DBNull.Value))
                Else
                    .Add(pfSet_Param("DepartureDt_TO", SqlDbType.DateTime, dtbDepartureTo.ppDate))
                End If

                'ＦＳ稼動有無
                If Me.cbxFsWrkCls.Checked.Equals(True) Then
                    .Add(pfSet_Param("FswrkCls", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("FswrkCls", SqlDbType.NVarChar, "0"))
                End If

                'NL区分
                .Add(pfSet_Param("NLCls", SqlDbType.NVarChar, Me.ddlNLCls.ppSelectedValue))

                '県コード
                '.Add(pfSet_Param("StateCd", SqlDbType.NVarChar, Me.txtStateCd.ppText.Trim))
                .Add(pfSet_Param("StateCd", SqlDbType.NVarChar, Me.ddlState.ppSelectedValue.Trim))

                'ホール住所
                .Add(pfSet_Param("Address", SqlDbType.NVarChar, Me.txtAddress.ppText.Trim))

                '担当者
                .Add(pfSet_Param("PersonInCharge", SqlDbType.NVarChar, Me.txtPersonInCharge.ppText.Trim))
                '営業所コード
                .Add(pfSet_Param("OfficeCd", SqlDbType.NVarChar, Me.txtOfficeCd.ppText.Trim))
                '新規
                If Me.cbxNew.Checked.Equals(True) Then
                    .Add(pfSet_Param("New", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("New", SqlDbType.NVarChar, String.Empty))
                End If
                '増設
                If Me.cbxExpansion.Checked.Equals(True) Then
                    .Add(pfSet_Param("Expansion", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("Expansion", SqlDbType.NVarChar, String.Empty))
                End If
                '一部撤去
                If Me.cbxSomeRemoval.Checked.Equals(True) Then
                    .Add(pfSet_Param("SomeRemoval", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("SomeRemoval", SqlDbType.NVarChar, String.Empty))
                End If
                '店内移設
                If Me.cbxShopRelocation.Checked.Equals(True) Then
                    .Add(pfSet_Param("ShopRelocation", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("ShopRelocation", SqlDbType.NVarChar, String.Empty))
                End If
                '全撤去
                If Me.cbxAllRemoval.Checked.Equals(True) Then
                    .Add(pfSet_Param("AllRemoval", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("AllRemoval", SqlDbType.NVarChar, String.Empty))
                End If
                '一時撤去
                If Me.cbxOnceRemoval.Checked.Equals(True) Then
                    .Add(pfSet_Param("OnceRemoval", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("OnceRemoval", SqlDbType.NVarChar, String.Empty))
                End If
                '構成変更
                If Me.cbxConChange.Checked.Equals(True) Then
                    .Add(pfSet_Param("ConChange", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("ConChange", SqlDbType.NVarChar, String.Empty))
                End If
                '構成配信
                If Me.cbxConDelivery.Checked.Equals(True) Then
                    .Add(pfSet_Param("ConDelively", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("ConDelively", SqlDbType.NVarChar, String.Empty))
                End If
                'バージョンアップ
                If Me.cbxVup.Checked.Equals(True) Then
                    .Add(pfSet_Param("Vup", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("Vup", SqlDbType.NVarChar, String.Empty))
                End If
                '再設置
                If Me.cbxReInstallation.Checked.Equals(True) Then
                    .Add(pfSet_Param("ReInstallation", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("ReInstallation", SqlDbType.NVarChar, String.Empty))
                End If
                'その他
                If Me.cbxOther.Checked.Equals(True) Then
                    .Add(pfSet_Param("Other", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("Other", SqlDbType.NVarChar, String.Empty))
                End If
                '検索対象（工事）
                If cbxConstruction.Checked Then
                    .Add(pfSet_Param("KSearch", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("KSearch", SqlDbType.NVarChar, "0"))
                End If
                '検索対象（保守）
                If cbxMaintenance.Checked Then
                    .Add(pfSet_Param("HSearch", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("HSearch", SqlDbType.NVarChar, "0"))
                End If

                '画面ＩＤ
                .Add(pfSet_Param("Progid", SqlDbType.NVarChar, sCnsProgid))
            End With

            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            'CNSLSTP007_001
            Call sAddSPCName(dstOrders, conDB, cmdDB)
            'CNSLSTP007_001
            If intPrintKbn = 0 Then
                'データ取得およびデータをリストに設定
                Me.grvList.DataSource = dstOrders

                '変更を反映
                Me.grvList.DataBind()

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                    'ボタン非活性設定
                    Master.Master.ppRigthButton1.Enabled = False '営業所印刷
                    Master.Master.ppRigthButton2.Enabled = False '工事印刷
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString

                    'ボタン活性設定
                    Master.Master.ppRigthButton1.Enabled = True '営業所印刷
                    Master.Master.ppRigthButton2.Enabled = True '工事印刷
                End If
            Else
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '該当
                    If intPrintKbn = 1 Then
                        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "営業所担当の工事データがありません。")
                    Else
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                Else
                    'Active Reports(帳票)の起動
                    psPrintPDF(Me, rpt, dstOrders.Tables(0), strFNm)
                End If
            End If

        Catch ex As Exception
            'システムエラー
            If intPrintKbn = 0 Then
                'メッセージ出力
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "作業予定一覧")
            Else
                '帳票情報の取得に失敗
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strFNm)
            End If
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定　COMSELP001-002
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddl()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try

                '都道府県設定
                objCmd = New SqlCommand("ZMSTSEL002", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'ドロップダウンリスト設定
                Me.ddlState.ppDropDownList.Items.Clear()
                Me.ddlState.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlState.ppDropDownList.DataTextField = "項目名"
                Me.ddlState.ppDropDownList.DataValueField = "都道府県コード"
                Me.ddlState.ppDropDownList.DataBind()
                Me.ddlState.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                'メッセージ出力
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ドロップダウン一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
                'Dispose
                objCmd.Dispose()
                objDs.Dispose()
            End Try
        End If
    End Sub

    ' ''' <summary>
    ' ''' 個別エラーチェック
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub msCheck_Error()

    '    ''出発日時
    '    'If Me.dtbDepartureFm.ppDate = Nothing OrElse Me.dtbDepartureTo.ppDate = Nothing Then
    '    '    '日付形式エラー
    '    '    Me.dtbDepartureFm.psSet_ErrorNo("4001", Me.dtbDepartureFm.ppName, "正しい日時")
    '    'Else
    '    '    '出発日時FROM～TO
    '    '    If Me.dtbDepartureFm.ppText <> String.Empty AndAlso Me.dtbDepartureTo.ppText <> String.Empty Then
    '    '        If Me.dtbDepartureFm.ppText + Me.dtbDepartureFm.ppHourText + Me.dtbDepartureFm.ppMinText >
    '    '           Me.dtbDepartureTo.ppText + Me.dtbDepartureTo.ppHourText + Me.dtbDepartureTo.ppMinText Then
    '    '            Me.dtbDepartureFm.psSet_ErrorNo("1005", "出発日時")
    '    '            Me.dtbDepartureFm.ppDateBox.Focus()
    '    '        End If
    '    '    End If
    '    'End If

    'End Sub

    'CNSUPDP001_015
    ''' <summary>
    ''' ＦＳ、ＮＧＣをマスターから取得
    ''' </summary>
    ''' <param name="cpobjDS"></param>
    ''' <param name="cpobjDBCon"></param>
    ''' <param name="cpobjDBCmd"></param>
    ''' <remarks></remarks>
    Private Sub sAddSPCName(ByRef cpobjDS As DataSet, cpobjDBCon As SqlConnection, cpobjDBCmd As SqlCommand)

        Dim objWKDS As New DataSet
        Dim objWKDS_CMP As New DataSet
        Dim strEW_FLG As String = ""

        cpobjDS.Tables(0).Columns.Add("ＦＳＣＭＰ", Type.GetType("System.String"))
        cpobjDS.Tables(0).Columns.Add("ＮＧＣＣＭＰ", Type.GetType("System.String"))
        cpobjDS.Tables(0).Columns.Add("ＮＧＣＣＳＨ", Type.GetType("System.String"))
        cpobjDS.Tables(0).Columns.Add("ＦＳＳＰＣ", Type.GetType("System.String"))
        cpobjDS.Tables(0).AcceptChanges()

        'ＤＢ接続
        If cpobjDBCon Is Nothing Then
            If Not clsDataConnect.pfOpen_Database(cpobjDBCon) Then
                Exit Sub
            End If
        End If
        ''パラメータ設定
        'cpobjDBCmd = New SqlCommand("CNSCOMP001_S2", cpobjDBCon)
        'cpobjDBCmd.Parameters.Add(pfSet_Param("prm_HALLCD", SqlDbType.NVarChar, txtHoleCd.ppText))
        'objWKDS = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        'strEW_FLG = "E"
        'If objWKDS.Tables.Count > 0 Then
        '    If objWKDS.Tables(0).Rows.Count > 0 Then
        '        If objWKDS.Tables(0).Rows(0).Item("T01_FAXNO") Is DBNull.Value Then
        '        ElseIf objWKDS.Tables(0).Rows(0).Item("T01_FAXNO").ToString = "" Then
        '        Else
        '            strEW_FLG = objWKDS.Tables(0).Rows(0).Item("T01_FAXNO").ToString
        '        End If
        '    End If
        'End If

        cpobjDBCmd = New SqlCommand("CNSCOMP001_S1", cpobjDBCon)
        '    @prm_SEQNO AS NUMERIC(4,0)
        '   ,@prm_TRADE_CD AS NVARCHAR(2)
        '   ,@prm_COMP_CD AS NVARCHAR(5)
        '   ,@prm_OFFICE_CD AS NVARCHAR(5)
        If strEW_FLG = "E" Then
            cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "1"))
        Else
            cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "2"))
        End If
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_TRADE_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_COMP_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_OFFICE_CD", SqlDbType.NVarChar, ""))
        objWKDS_CMP = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        If objWKDS_CMP.Tables.Count > 0 Then
            If objWKDS_CMP.Tables(0).Rows.Count > 0 Then
                For zz = 0 To cpobjDS.Tables(0).Rows.Count - 1
                    cpobjDS.Tables(0).Rows(zz).Item("ＮＧＣＣＭＰ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_COMP_NM")
                    cpobjDS.Tables(0).Rows(zz).Item("ＮＧＣＣＳＨ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_OFFICE_NM")
                Next
            End If
        End If

        cpobjDBCmd = New SqlCommand("CNSCOMP001_S1", cpobjDBCon)
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "207"))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_TRADE_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_COMP_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_OFFICE_CD", SqlDbType.NVarChar, ""))
        objWKDS_CMP = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        If objWKDS_CMP.Tables.Count > 0 Then
            If objWKDS_CMP.Tables(0).Rows.Count > 0 Then
                For zz = 0 To cpobjDS.Tables(0).Rows.Count - 1
                    cpobjDS.Tables(0).Rows(zz).Item("ＦＳＳＰＣ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_COMP_NM") & " " & objWKDS_CMP.Tables(0).Rows(0).Item("M44_OFFICE_NM")
                Next
            End If
        End If

        cpobjDBCmd = New SqlCommand("CNSCOMP001_S1", cpobjDBCon)
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "208"))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_TRADE_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_COMP_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_OFFICE_CD", SqlDbType.NVarChar, ""))
        objWKDS_CMP = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        If objWKDS_CMP.Tables.Count > 0 Then
            If objWKDS_CMP.Tables(0).Rows.Count > 0 Then
                For zz = 0 To cpobjDS.Tables(0).Rows.Count - 1
                    cpobjDS.Tables(0).Rows(zz).Item("ＦＳＣＭＰ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_COMP_NM") & " " & objWKDS_CMP.Tables(0).Rows(0).Item("M44_OFFICE_NM")
                Next
            End If
        End If

        cpobjDS.Tables(0).AcceptChanges()

    End Sub
    'CNSUPDP001_015

#End Region


#Region "終了処理プロシージャ"
#End Region

    ''' <summary>
    ''' バリデータ　チェック
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CustomValidator1_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CustomValidator1.ServerValidate

        '出発日時
        If Me.dtbDepartureFm.ppText.Trim = "" _
        AndAlso Me.dtbDepartureFm.ppMinText.Trim = "" _
        AndAlso Me.dtbDepartureFm.ppHourText.Trim = "" Then
            '日時分全て未入力 チェックしない
        Else
            If Me.dtbDepartureFm.ppDate = Nothing _
            OrElse Me.dtbDepartureFm.ppText.Trim = "" _
            OrElse Me.dtbDepartureFm.ppHourText.Trim = "" _
            OrElse Me.dtbDepartureFm.ppMinText.Trim = "" Then
                '日付形式エラー
                source.ErrorMessage = "出発日時は正しい日付 で入力してください。"
                source.Text = "日付エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If

        If Me.dtbDepartureTo.ppText.Trim = "" _
        AndAlso Me.dtbDepartureTo.ppMinText.Trim = "" _
        AndAlso Me.dtbDepartureTo.ppHourText.Trim = "" Then
            '日時分全て未入力 チェックしない
        Else
            If Me.dtbDepartureTo.ppDate = Nothing _
            OrElse Me.dtbDepartureTo.ppText.Trim = "" _
            OrElse Me.dtbDepartureTo.ppHourText.Trim = "" _
            OrElse Me.dtbDepartureTo.ppMinText.Trim = "" Then
                '日付形式エラー
                source.ErrorMessage = "出発日時は正しい日付 で入力してください。"
                source.Text = "日付エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If

        'If Me.dtbDepartureFm.ppDate = Nothing OrElse Me.dtbDepartureTo.ppDate = Nothing Then
        '    '日付形式エラー
        '    source.ErrorMessage = "出発日時は正しい日付で入力してください。"
        '    source.Text = "日付エラー"
        '    args.IsValid = False
        '    Exit Sub
        'Else
        '出発日時FROM～TO
        If Me.dtbDepartureFm.ppText <> String.Empty AndAlso Me.dtbDepartureTo.ppText <> String.Empty Then
            If Me.dtbDepartureFm.ppText + Me.dtbDepartureFm.ppHourText + Me.dtbDepartureFm.ppMinText >
               Me.dtbDepartureTo.ppText + Me.dtbDepartureTo.ppHourText + Me.dtbDepartureTo.ppMinText Then
                source.ErrorMessage = "出発日時は開始が終了以前の日時となるよう入力してください。"
                source.Text = "日付エラー"
                args.IsValid = False
                Me.dtbDepartureFm.ppDateBox.Focus()
                Exit Sub
            End If
        End If
        'End If

    End Sub

End Class
