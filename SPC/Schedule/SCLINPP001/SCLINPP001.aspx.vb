'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　スケジュール管理
'*　ＰＧＭＩＤ：　SCLINPP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.11.28　：　加賀
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'SCLINPP001-001     2016/11/28      加賀     作成
'SCLINPP001-002     2016/01/20      武　     日時設定処理
'SCLINPP001-003     2016/01/20      武　     検索条件を到着のみにする(SCLINPP001_S01)
'SCLINPP001-004     2016/01/20      武　     過去日時エラーとなる登録不可データは除外する(SCLINPP001_S01)
'SCLINPP001-005     2016/01/20      武　     登録処理後、再描画する
'SCLINPP001-006     2016/01/23      稲葉     訪問種別変更後、TBOXIDの値を初期化
'SCLINPP001-007     2016/01/23      稲葉     登録処理後、フッター登録ボタンを非活性化 
'SCLINPP001-008     2016/01/24      加賀     TBOXID⇔エリアの切り替え
'SCLINPP001-009     2016/02/06      加賀     CRSをWKB,SPCで利用可能に変更

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon

Public Class SCLINPP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    Const MY_DISP_ID As String = "SCLINPP001"
    Const SCL_COMMON_S01 As String = "SCLCOMMON_S01"
    Const VS_TERMS As String = "VS_TERMS"
    Const VS_PRMS As String = "VS_PRMS"
    Const VS_SCHEDULE As String = "VS_SCHEDULE"
    Const VS_IMPORT_CLS As String = "VS_IMPORT_CLS"
    Const VS_SQLCMD_PRM As String = "VS_SQLCMD_PRM"
    ''SCLINPP001-005
    'Const VS_TBOXID As String = "VS_TBOXID"
    'Const VS_AREA As String = "VS_AREA"     'SCLINPP001-008
    'Const VS_SDT As String = "VS_SDT"
    'Const VS_SH As String = "VS_SH"
    'Const VS_SM As String = "VS_SM"
    'Const VS_EDT As String = "VS_EDT"
    'Const VS_EH As String = "VS_EH"
    'Const VS_EM As String = "VS_EM"
    ''SCLINPP001-005 END

    ''' <summary>
    ''' 取込みエリア制御用
    ''' </summary>
    Private Enum EditMode As Short
        未選択 = 0
        検索 = 1
        取込 = 2
    End Enum

    ''' <summary>
    ''' CSV項目定義
    ''' </summary>
    Private Enum CSVField As Integer
        訪問種別コード
        作業日付
        作業時間
        訪問先
        社員コード
        出発日付
        出発時間
        登録区分
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
        'Dim intHeadCol As Integer() = New Integer() {3}
        'Dim intColSpan As Integer() = New Integer() {2}
        'ClsCMCommon.pfSet_GridView(Me.grvList, M_MY_DISP_ID, intHeadCol, intColSpan)

        ClsCMCommon.pfSet_GridView(Me.grvList, MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンイベント処理設定
        AddHandler Master.ppRigthButton3.Click, AddressOf btnImport_Click         '抽出
        AddHandler Master.ppRigthButton4.Click, AddressOf btnClear_Click         'クリア
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnRegist_Click '登録
        AddHandler ddlImportCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlDateCls_SelectedIndexChanged  '取込区分

        Try

            If Not IsPostBack Then  '初回表示のみ

                Dim clsCMDBC As New ClsCMDBCom  '画面名取得用

                'プログラムID、画面名設定
                Master.Master.ppProgramID = MY_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(MY_DISP_ID)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = ClsCMCommon.pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '件数非表示
                Master.ppCount_Visible = False

                'ボタン設定
                '検索,検索条件クリア
                Master.ppRigthButton1.Visible = False           '検索
                Master.ppRigthButton2.Visible = False           '検索条件クリア

                '取込み
                Master.ppRigthButton3.Visible = True
                Master.ppRigthButton3.Text = "抽出"
                Master.ppRigthButton3.CausesValidation = False

                'クリア
                Master.ppRigthButton4.Visible = True
                Master.ppRigthButton4.Text = "クリア"
                Master.ppRigthButton4.CausesValidation = False

                '登録
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton1.Text = "登録"
                Master.Master.ppRigthButton1.CausesValidation = False
                Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "エラーが無いデータのみ")    '{0}を登録します。\nよろしいですか？

                'TBOXID/エリア
                btnTbxArea.CausesValidation = False

                ''クリア
                'Master.Master.ppRigthButton2.Visible = True
                'Master.Master.ppRigthButton2.Text = "クリア"
                'Master.Master.ppRigthButton2.CausesValidation = False

                'ファイルパス表示ダミー用
                'trfUplordFileSelection.Attributes.Add("onchange", String.Format("{0}.style.display='inline-block'; {0}.value = this.value;", txtCvr.ClientID))

                'ドロップダウンリスト設定
                setDropDownList()

                'AutoPostBack!
                ddlImportCls.ppDropDownList.AutoPostBack = True

                '活性制御
                setEnable(EditMode.未選択)

                'フォーカス
                ddlImportCls.ppDropDownList.Focus()

                'ファイルパスダミー
                txtCvr.Visible = False

                '一覧表示
                grvList.DataSource = New DataTable
                grvList.DataBind()

            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面の初期化")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'SCLINPP001-009
        If Not IsPostBack Then  '初回表示のみ

            Select Case Session(P_SESSION_CRS_USE)
                Case ClsComVer.E_CRS使用制限.使用不可
                    '使用不可
                    setEnable(EditMode.未選択)
                    ddlImportCls.ppEnabled = False
                Case ClsComVer.E_CRS使用制限.参照
                    '使用不可
                    setEnable(EditMode.未選択)
                    ddlImportCls.ppEnabled = False
            End Select

        End If

    End Sub


    ''' <summary>
    ''' 取込区分　変更時処理
    ''' </summary>
    Protected Sub ddlDateCls_SelectedIndexChanged(sender As Object, e As EventArgs)

        '取込区分
        Select Case ddlImportCls.ppSelectedValue
            Case String.Empty

                '活性制御
                setEnable(EditMode.未選択)

                'クリア
                txtDate_From.ppText = String.Empty
                txtDate_From.ppHourText = String.Empty
                txtDate_From.ppMinText = String.Empty
                txtDate_To.ppText = String.Empty
                txtDate_To.ppHourText = String.Empty
                txtDate_To.ppMinText = String.Empty

                'SCLINPP001-006
                txtTBOXID.ppText = String.Empty
                'SCLINPP001-006 END

                'フォーカス
                SetFocus(ddlImportCls.ppDropDownList)

            Case "1"

                'SCLINPP001-002
                Dim dsMailMst As DataSet = Nothing
                Dim dtSDt As DateTime = Nothing
                Dim dtEDt As DateTime = Nothing
                Dim strSDt As String = String.Empty
                Dim strSH As String = String.Empty
                Dim strSM As String = String.Empty
                Dim strEDt As String = String.Empty
                Dim strEH As String = String.Empty
                Dim strEM As String = String.Empty

                '活性制御
                setEnable(EditMode.検索)

                '初期値
                txtDate_From.ppText = Date.Now.ToString("yyyy/MM/dd")
                txtDate_From.ppHourText = "18"
                txtDate_From.ppMinText = "00"
                txtDate_To.ppText = Date.Now.AddDays(1).ToString("yyyy/MM/dd")
                txtDate_To.ppHourText = "09"
                txtDate_To.ppMinText = "00"

                'SCLINPP001-006
                txtTBOXID.ppText = String.Empty
                'SCLINPP001-006 END

                'フォーカス
                SetFocus(txtTBOXID.ppTextBox)

            Case Else

                '活性制御
                setEnable(EditMode.取込)

                'クリア
                txtDate_From.ppText = String.Empty
                txtDate_From.ppHourText = String.Empty
                txtDate_From.ppMinText = String.Empty
                txtDate_To.ppText = String.Empty
                txtDate_To.ppHourText = String.Empty
                txtDate_To.ppMinText = String.Empty

                'SCLINPP001-006
                txtTBOXID.ppText = String.Empty
                'SCLINPP001-006 END

                'フォーカス
                SetFocus(trfUplordFileSelection)

        End Select

    End Sub

    ''' <summary>
    ''' クリアボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Try

            '明細クリア
            ClearImport()

            'クリア
            ViewState(VS_SCHEDULE) = Nothing

            '一覧初期化
            grvList.DataSource = New DataTable
            grvList.DataBind()

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "抽出条件のクリア")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 抽出ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnImport_Click()

        '開始ログ出力
        psLogStart(Me)

        Try

            If ddlImportCls.ppSelectedValue = "1" Then

                '日時入力値を退避
                Dim strDateFrom(3), strDateTo(3) As String

                strDateFrom = {String.Empty, txtDate_From.ppText, txtDate_From.ppHourText, txtDate_From.ppMinText}
                strDateTo = {String.Empty, txtDate_To.ppText, txtDate_To.ppHourText, txtDate_To.ppMinText}

                '日時を編集
                If strDateFrom(1) <> String.Empty AndAlso strDateFrom(2) = String.Empty AndAlso strDateFrom(3) = String.Empty Then
                    txtDate_From.ppHourText = "00"
                    txtDate_From.ppMinText = "00"
                End If

                If strDateTo(1) <> String.Empty AndAlso strDateTo(2) = String.Empty AndAlso strDateTo(3) = String.Empty Then
                    txtDate_To.ppHourText = "23"
                    txtDate_To.ppMinText = "59"
                End If

                '入力値検証
                Me.Validate()

                '作業日時検証
                'If strDateFrom(1) <> String.Empty Then
                '    If strDateFrom(2) = String.Empty AndAlso strDateFrom(3) = String.Empty Then
                '        If txtDate_From.ppText < DateTime.Now.ToShortDateString Then

                '            '過去日登録
                '            txtDate_From.psSet_ErrorNo("1006", txtDate_From.ppName, "現在日時")    '「{0}」は「{1}」以降の日時で入力してください。

                '        End If
                '    Else
                '        If txtDate_From.ppDate < DateTime.Now Then

                '            '過去日登録
                '            txtDate_From.psSet_ErrorNo("1006", txtDate_From.ppName, "現在日時")    '「{0}」は「{1}」以降の日時で入力してください。

                '        End If
                '    End If
                'End If

                'If txtDate_To.ppText <> String.Empty AndAlso txtDate_To.ppDate < DateTime.Now Then

                '    '過去日登録
                '    txtDate_To.psSet_ErrorNo("1006", txtDate_To.ppName, "現在日時")    '「{0}」は「{1}」以降の日時で入力してください。

                'End If

                '日時のFROM≦TO確認
                If txtDate_To.ppText <> String.Empty AndAlso txtDate_From.ppText <> String.Empty Then
                    If txtDate_From.ppDate > txtDate_To.ppDate Then
                        txtDate_To.psSet_ErrorNo("1006", txtDate_From.ppName & "To", txtDate_From.ppName & "From")     '「{0}」は「{1}」以降の日時で入力してください。
                    End If
                End If

                '日時入力値を復元
                txtDate_From.ppText = strDateFrom(1)
                txtDate_From.ppHourText = strDateFrom(2)
                txtDate_From.ppMinText = strDateFrom(3)
                txtDate_To.ppText = strDateTo(1)
                txtDate_To.ppHourText = strDateTo(2)
                txtDate_To.ppMinText = strDateTo(3)

                '入力値検証結果
                If (Page.IsValid) = False Then
                    Exit Sub
                End If

                '取込み区分保存
                ViewState(VS_IMPORT_CLS) = ddlImportCls.ppSelectedValue

                '工事・保守データ取得
                getData()

            Else

                Me.Validate()

                '入力値検証
                If Me.IsValid() = False Then
                    Exit Sub
                End If

                '取込み区分保存
                ViewState(VS_IMPORT_CLS) = ddlImportCls.ppSelectedValue

                'CSV取込
                getCSVData()

            End If

            ''抽出内容を確認してメッセージを表示する
            'Dim dtGrvlist As New DataTable
            'Dim intTFFlg As Integer = 0

            'dtGrvlist = Me.grvList.DataSource

            'If dtGrvlist.Rows.Count > 0 Then
            '    For intRowCnt As Integer = 0 To dtGrvlist.Rows.Count - 1
            '        If dtGrvlist.Rows(intRowCnt).Item("エラー内容") = String.Empty _
            '            And dtGrvlist.Rows(intRowCnt).Item("状況") = String.Empty Then
            '            intTFFlg = 1
            '            Exit For
            '        End If
            '    Next
            'Else
            '    intTFFlg = 2
            'End If

            'Select Case intTFFlg
            '    Case 0      'エラーのみ
            '        '登録なしメッセージ
            '        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "抽出内容を確認してください。")
            '    Case 1      '取り込みデータあり
            '        '正常終了メッセージ
            '        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "抽出内容を確認して登録ボタンを押下してください。")
            '    Case 2      '抽出件数なし
            '        '抽出内容なし
            'End Select

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "データの抽出")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try
        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnRegist_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim intReturn As Integer

        Try

            If ViewState(VS_IMPORT_CLS) = "1" Then

                '工事・保守

                'SQLコマンド設定
                Using cmdSQL As New SqlCommand(MY_DISP_ID & "_I01")

                    'パラメータ設定
                    cmdSQL.Parameters.Add(New SqlParameter("@usr_id", Session(P_SESSION_USERID).ToString))
                    cmdSQL.Parameters.Add(New SqlParameter("@SCHEDULE", DirectCast(ViewState(VS_SCHEDULE), DataTable)))
                    cmdSQL.Parameters("@SCHEDULE").SqlDbType = SqlDbType.Structured

                    '登録・更新実行
                    If ClsCMDataConnect.pfExec_StoredProcedure(Me, {cmdSQL}, intReturn) = False Then

                        'メッセージ表示
                        Select Case intReturn
                            Case -1
                                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                            Case 0
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "スケジュール登録")       '{0}に失敗しました。
                        End Select

                        Exit Sub

                    End If

                End Using

                'SCLINPP001-005
                'データ再取得
                getData(DirectCast(ViewState(VS_SQLCMD_PRM), String()))
                'SCLINPP001-005 END

                'クリア
                'ViewState(VS_SCHEDULE) = Nothing

            Else

                'CSV

                'SQLコマンド設定
                Using cmdSQL As New SqlCommand(MY_DISP_ID & "_I02")

                    'パラメータ設定
                    cmdSQL.Parameters.Add(New SqlParameter("@usr_id", Session(P_SESSION_USERID).ToString))
                    cmdSQL.Parameters.Add(New SqlParameter("@CSV", DirectCast(ViewState(VS_SCHEDULE), DataTable)))
                    cmdSQL.Parameters("@CSV").SqlDbType = SqlDbType.Structured

                    '登録・更新実行
                    If ClsCMDataConnect.pfExec_StoredProcedure(Me, {cmdSQL}, intReturn) = False Then

                        'メッセージ表示
                        Select Case intReturn
                            Case -1
                                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                            Case 0
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + "スケジュール登録")       '{0}に失敗しました。
                        End Select

                        Exit Sub

                    End If

                End Using

                'SCLINPP001-005
                'クリア
                'ViewState(VS_SCHEDULE) = Nothing
                'SCLINPP001-005 END

            End If

            ''一覧初期化
            'grvList.DataSource = New DataTable
            'grvList.DataBind()

            ''取込み条件クリア
            'ClearImport()

            ''取込み区分初期化
            'ViewState(VS_IMPORT_CLS) = String.Empty

            'SCLINPP001-007
            '処理完了後、登録ボタンを非活性とする
            Master.Master.ppRigthButton1.Enabled = False   'フッター登録
            'SCLINPP001-007 END

            '正常終了メッセージ
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "スケジュール")


        Catch ex As Exception

            'メッセージ出力
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "スケジュール登録に失敗しました。")

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
    ''' <remarks>赤字表示</remarks>
    Private Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Try

            '行ループ
            For Each grvrow As GridViewRow In grvList.Rows

                'エラー有レコード
                If DirectCast(grvrow.FindControl("エラー内容"), TextBox).Text <> String.Empty Then

                    For Each cell As TableCell In grvrow.Cells

                        '赤字
                        If TypeOf cell.Controls(0) Is TextBox Then

                            DirectCast(cell.Controls(0), TextBox).ForeColor = Drawing.Color.Red

                        End If

                    Next

                Else

                    '保守の場合のみ差異を青表示
                    If DirectCast(grvrow.FindControl("訪問種別コード"), TextBox).Text = "02" _
                        AndAlso DirectCast(grvrow.FindControl("訪問種別コード"), TextBox).Text = "済" Then

                        If DirectCast(grvrow.FindControl("旧作業人数"), TextBox).Text = "1" Then

                            If DirectCast(grvrow.FindControl("訪問先変更"), TextBox).Text = "1" Then

                                DirectCast(grvrow.FindControl("訪問先"), TextBox).ForeColor = Drawing.Color.Blue

                            End If

                            If DirectCast(grvrow.FindControl("作業担当者変更"), TextBox).Text = "1" Then

                                DirectCast(grvrow.FindControl("作業者"), TextBox).ForeColor = Drawing.Color.Blue

                            End If

                            If DirectCast(grvrow.FindControl("出発日時変更"), TextBox).Text = "1" Then

                                DirectCast(grvrow.FindControl("出発日時"), TextBox).ForeColor = Drawing.Color.Blue

                            End If

                            If DirectCast(grvrow.FindControl("到着日時変更"), TextBox).Text = "1" Then

                                DirectCast(grvrow.FindControl("到着日時"), TextBox).ForeColor = Drawing.Color.Blue

                            End If

                        Else

                            DirectCast(grvrow.FindControl("訪問先"), TextBox).ForeColor = Drawing.Color.Blue

                            DirectCast(grvrow.FindControl("作業者"), TextBox).ForeColor = Drawing.Color.Blue

                            DirectCast(grvrow.FindControl("出発日時"), TextBox).ForeColor = Drawing.Color.Blue

                            DirectCast(grvrow.FindControl("到着日時"), TextBox).ForeColor = Drawing.Color.Blue

                            DirectCast(grvrow.FindControl("作業人数"), TextBox).ForeColor = Drawing.Color.Blue

                        End If

                    End If

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

    ''' <summary>
    ''' File選択のエラーチェック
    ''' </summary>
    Protected Sub valfileUpload_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles valfileUpload.ServerValidate

        If ddlImportCls.ppSelectedValue = "2" Then

            If trfUplordFileSelection.HasFile = False Then

                source.ErrorMessage = "読込むファイルを指定して下さい。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub

            End If

            'ファイル拡張子確認
            If IO.Path.GetExtension(trfUplordFileSelection.FileName) <> ".csv" Then

                source.ErrorMessage = "ファイルの形式に相違があります。CSVファイルを指定して下さい。"
                source.Text = "形式エラー"
                args.IsValid = False
                Exit Sub

            End If

        End If

    End Sub

    ''' <summary>
    ''' TBOXID/エリア 変更ボタン押下時処理 'SCLINPP001-008
    ''' </summary>
    Protected Sub btnTbxArea_Click(sender As Object, e As EventArgs) Handles btnTbxArea.Click

        '開始ログ出力
        psLogStart(Me)

        Try

            'TBOXID/エリア制御
            Select Case btnTbxArea.Text
                Case "TBOXID"
                    setTbxArea(1)
                    'フォーカス
                    SetFocus(ddlArea.ppDropDownList)
                Case "エリア"
                    setTbxArea(0)
                    'フォーカス
                    SetFocus(txtTBOXID.ppTextBox)
            End Select


        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "抽出条件の変更")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region


#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得
    ''' </summary>
    Private Sub getData(Optional ByVal strRegetPrm() As String = Nothing)

        Dim dstSelect As New DataSet
        Dim dtImport As New DataTable
        Dim strSqlPrm(3) As String

        Try
            '作業日時入力値を退避
            Dim strDateFrom(3), strDateTo(3) As String

            strDateFrom = {String.Empty, txtDate_From.ppText, txtDate_From.ppHourText, txtDate_From.ppMinText}
            strDateTo = {String.Empty, txtDate_To.ppText, txtDate_To.ppHourText, txtDate_To.ppMinText}

            '作業日時をストアド用にﾌｫｰﾏｯﾄ
            'FROM
            strDateFrom(0) = strDateFrom(1).Replace("/", "") & strDateFrom(2) & strDateFrom(3)

            'TO
            If txtDate_To.ppText = String.Empty Then
                strDateTo(0) = String.Empty
            ElseIf txtDate_To.ppHourText = String.Empty Then
                strDateTo(0) = txtDate_To.ppText.Replace("/", "") & "2359"
            Else
                strDateTo(0) = txtDate_To.ppText.Replace("/", "") & txtDate_To.ppHourText & txtDate_To.ppMinText
            End If

            'パラメータ準備
            If strRegetPrm Is Nothing Then
                strSqlPrm = {strDateFrom(0), strDateTo(0), txtTBOXID.ppText, ddlArea.ppSelectedValue}
            Else
                strSqlPrm = strRegetPrm 'SCLINPP001-005 
            End If


            'SQLコマンド設定
            Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S01")

                'パラメータ設定
                cmdSQL.Parameters.AddRange({New SqlParameter("@wrk_dt_from", strSqlPrm(0)) _
                                          , New SqlParameter("@wrk_dt_to", strSqlPrm(1)) _
                                          , New SqlParameter("@tboxid", strSqlPrm(2)) _
                                          , New SqlParameter("@area", strSqlPrm(3)) _
                                           })


                'コマンド保存 'SCLINPP001-005 
                ViewState(VS_SQLCMD_PRM) = strSqlPrm

                'データ取得
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "スケジュール", cmdSQL, dstSelect) = False Then

                    Exit Sub

                End If

            End Using

            '該当件数表示
            If dstSelect.Tables(0).Rows.Count = 0 Then

                '該当件数
                Master.ppCount = "0"

                '該当データ無し
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                'フッター
                Master.Master.ppRigthButton1.Enabled = False  '登録

            Else

                'フッター
                Master.Master.ppRigthButton1.Enabled = True   '登録

            End If

            '一覧にバインド
            grvList.DataSource = dstSelect.Tables(0).Copy()
            grvList.DataBind()

            '一覧ソート可能
            grvList.AllowSorting = True


            '登録用テーブル作成
            If dstSelect.Tables(0).Select("エラー内容 = ''").Count = 0 Then

                '登録ボタン
                Master.Master.ppRigthButton1.Enabled = False

                '登録なしメッセージ
                If grvList.Rows.Count > 0 Then
                    psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "抽出内容を確認してください。")
                End If

            Else

                '登録ボタン
                Master.Master.ppRigthButton1.Enabled = True

                '正常終了メッセージ
                If strRegetPrm Is Nothing Then
                    psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "抽出内容を確認して登録ボタンを押下してください。")
                End If

                '登録するレコードのみ保存
                dtImport = dstSelect.Tables(0).Select("エラー内容 = ''").CopyToDataTable

                '不要項目を削除
                With dtImport.Columns
                    .Remove("SEQ")
                    .Remove("工事区分名称")
                    .Remove("訪問種別")
                    .Remove("訪問先")
                    .Remove("エリア")
                    .Remove("作業者")
                    .Remove("作業人数")
                    .Remove("エラー内容")
                    .Remove("訪問先変更")
                    .Remove("作業担当者変更")
                    .Remove("出発日時変更")
                    .Remove("到着日時変更")
                    .Remove("旧作業人数")
                End With

                '変更を反映
                dtImport.AcceptChanges()

            End If

            '保管
            ViewState(VS_SCHEDULE) = dtImport

        Catch ex As Exception

            '一覧にバインド
            grvList.DataSource = New DataTable
            grvList.DataBind()

            '登録
            Master.Master.ppRigthButton1.Enabled = False

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "データの検索")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            'Dispose
            If dstSelect IsNot Nothing Then
                dstSelect.Dispose()
            End If

        End Try

    End Sub

    ''' <summary>
    ''' CSVデータ取得
    ''' </summary>
    ''' <remarks>取込んだCSVを整形し、DBと結合して再取得します</remarks>
    Private Sub getCSVData()

        'CSV読み込み用
        Dim srmCSV As IO.Stream
        Dim bytBuff() As Byte
        Dim dtCSV As New DataTable
        Dim drwCSV As DataRow
        Dim strCSV As String
        Dim strCSVItem() As String
        Dim strReturn As String
        Dim dstSelect As New DataSet

        Try

            'カラム設定
            dtCSV.Columns.Add("訪問種別コード")
            dtCSV.Columns.Add("作業日付")
            dtCSV.Columns.Add("作業時間")
            dtCSV.Columns.Add("訪問先")
            dtCSV.Columns.Add("社員コード")
            dtCSV.Columns.Add("出発日付")
            dtCSV.Columns.Add("出発時間")
            dtCSV.Columns.Add("登録区分")
            dtCSV.Columns.Add("エラー内容")

            '☆CSVファイル読み込み処理
            '以下の形式を前提として処理します
            '①フィールドはカンマ(,)区切り
            '②レコードは改行(CrLf)区切り
            '③フィールドにはカンマを含まない

            'ファイル取得
            srmCSV = trfUplordFileSelection.PostedFile.InputStream

            '一覧にバインド
            grvList.DataSource = New DataTable
            grvList.DataBind()

            '適切なCSVファイルか判定する
            If Not Me.trfUplordFileSelection.FileName.Substring(0, 4) = "CRS_" Then
                '形式エラー ファイル名違い
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CRSの形式のファイル名ではありません。\nファイル名を確認して下さい。")
                Exit Sub
            End If

            'バイト配列用意
            bytBuff = New Byte(srmCSV.Length - 1) {}

            'Read
            srmCSV.Read(bytBuff, 0, srmCSV.Length)

            '文字列に変換
            strCSV = Encoding.GetEncoding("SHIFT-JIS").GetString(bytBuff)

            'レコードに分解 ループ
            For Each strCSVRow As String In strCSV.Split(Microsoft.VisualBasic.vbCrLf.ToCharArray, StringSplitOptions.RemoveEmptyEntries)

                'フィールドに分解
                strCSVItem = strCSVRow.Split(",")

                'フィールド数確認
                If strCSVItem.Length <> 8 Then

                    '形式エラー フィールド数違い
                    psMesBox(Me, "10006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイルの形式")    '{0}に相違があります。ファイルを確認して下さい。

                    Exit Sub

                End If

                'フィールドの値から前後のスペースを除去
                'strCSVItem(0) = strCSVItem(0).Replace("""", String.Empty).Trim
                strCSVItem(0) = strCSVItem(0).Trim
                strCSVItem(1) = strCSVItem(1).Trim
                strCSVItem(2) = strCSVItem(2).Trim
                strCSVItem(3) = strCSVItem(3).Trim
                strCSVItem(4) = strCSVItem(4).Trim
                strCSVItem(5) = strCSVItem(5).Trim
                strCSVItem(6) = strCSVItem(6).Trim
                strCSVItem(7) = strCSVItem(7).Trim

                '結果初期化
                strReturn = String.Empty

                'CSV項目チェック・整形
                CheckCSVData(strCSVItem, strReturn)

                'レコード作成
                drwCSV = dtCSV.NewRow

                'フィールドの値を格納
                drwCSV(0) = strCSVItem(0)   '訪問種別コード
                drwCSV(1) = strCSVItem(3)   '訪問先
                drwCSV(2) = strCSVItem(5)   '出発日付
                drwCSV(3) = strCSVItem(6)   '出発時間
                drwCSV(4) = strCSVItem(1)   '到着日付
                drwCSV(5) = strCSVItem(2)   '到着時間
                drwCSV(6) = strCSVItem(4)   '作業者コード
                drwCSV(7) = strCSVItem(7)   '状況
                drwCSV(8) = strReturn       'エラー内容

                'レコード追加
                dtCSV.Rows.Add(drwCSV)

            Next

            '変更を反映
            dtCSV.AcceptChanges()


            'SQLコマンド設定
            Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S02")

                'パラメータ設定
                cmdSQL.Parameters.Add(New SqlParameter("@CSV", dtCSV))
                cmdSQL.Parameters("@CSV").SqlDbType = SqlDbType.Structured

                'データ取得
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "CSVデータ", cmdSQL, dstSelect) = False Then

                    Exit Sub

                End If

            End Using

            '該当件数表示
            If dstSelect.Tables(0).Rows.Count = 0 Then
                '該当件数
                Master.ppCount = "0"
                '該当データ無し
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                'フッター
                Master.Master.ppRigthButton1.Enabled = False  '登録
            Else
                'フッター
                Master.Master.ppRigthButton1.Enabled = True   '登録
            End If

            'Table編集 
            Dim intCnt As Integer = 1
            Dim strErrDtl As String = String.Empty
            For Each drwSelect As DataRow In dstSelect.Tables(0).Rows

                '作業者重複
                If drwSelect("エラー内容").ToString = String.Empty _
                AndAlso dstSelect.Tables(0).Select("SEQ = " & drwSelect("SEQ").ToString & " AND 作業者コード = '" & drwSelect("作業者コード").ToString & "'").Count > 1 Then
                    drwSelect("エラー内容") = "作業者重複"
                End If

                '先頭項目以外一部の表示項目消す
                If drwSelect("SEQ") = intCnt Then

                    '明細にエラーが存在するか確認
                    strErrDtl = dstSelect.Tables(0).Compute("MAX(エラー内容)", "SEQ = " & intCnt.ToString)

                    '次のSEQまでインクリメント
                    intCnt += 1

                Else

                    '表示項目消す
                    drwSelect("SEQ") = DBNull.Value
                    'drwSelect("状況") = String.Empty
                    drwSelect("訪問種別") = String.Empty
                    drwSelect("訪問先") = String.Empty
                    drwSelect("作業人数") = String.Empty

                    '明細にエラーが存在する場合、エラーとして表示
                    If strErrDtl <> String.Empty AndAlso drwSelect("エラー内容").ToString = String.Empty Then
                        drwSelect("エラー内容") = Environment.NewLine
                    End If

                End If

            Next

            '登録用テーブル作成
            If dstSelect.Tables(0).Select("状況 = '' AND エラー内容 = ''").Count = 0 Then

                '登録ボタン
                Master.Master.ppRigthButton1.Enabled = False
                '登録なしメッセージ
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "抽出内容を確認してください。")

            Else

                '登録ボタン
                Master.Master.ppRigthButton1.Enabled = True
                '正常終了メッセージ
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "抽出内容を確認して登録ボタンを押下してください。")

                '登録するレコードのみ保存
                dtCSV = dstSelect.Tables(0).Select("状況 = '' AND エラー内容 = ''").CopyToDataTable

                '不要項目を削除
                With dtCSV.Columns
                    .Remove("SEQ")
                    .Remove("訪問種別")
                    .Remove("出発日時")
                    .Remove("到着日時")
                    .Remove("訪問先")
                    .Remove("作業者")
                    .Remove("作業人数")
                    .Remove("エリア")
                End With
            End If

            '一覧バインドに必要な項目を追加
            dstSelect.Tables(0).Columns.Add("依頼番号")
            dstSelect.Tables(0).Columns.Add("工事区分名称")
            dstSelect.Tables(0).Columns.Add("訪問先変更")
            dstSelect.Tables(0).Columns.Add("作業担当者変更")
            dstSelect.Tables(0).Columns.Add("出発日時変更")
            dstSelect.Tables(0).Columns.Add("到着日時変更")
            dstSelect.Tables(0).Columns.Add("旧作業人数")

            '一覧にバインド
            grvList.DataSource = dstSelect.Tables(0).Copy
            grvList.DataBind()

            '一覧ソート不可 (ぐちゃる為)
            grvList.AllowSorting = False

            '保管
            ViewState(VS_SCHEDULE) = dtCSV.Copy

        Catch ex As Exception

            '一覧にバインド
            grvList.DataSource = New DataTable
            grvList.DataBind()

            '登録
            Master.Master.ppRigthButton1.Enabled = False

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイルの読込み")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Finally

            'Dispose
            If dtCSV IsNot Nothing Then
                dtCSV.Dispose()
            End If

            If dstSelect IsNot Nothing Then
                dstSelect.Dispose()
            End If

        End Try

    End Sub

    ''' <summary>
    ''' CSVデータ項目チェック・整形
    ''' </summary>
    Private Sub CheckCSVData(ByRef strCSVItem() As String, ByRef strReturn As String)

        Dim intItemLen(6)() As Integer

        'CSV項目の桁数定義
        intItemLen(0) = {2, 2}      '訪問種別コード
        intItemLen(1) = {8, 8}      '作業日付
        intItemLen(2) = {4, 4}      '作業時間
        intItemLen(3) = {1, 50}     '訪問先
        intItemLen(4) = {1, 8}      '社員コード
        intItemLen(5) = {8, 8}      '出発日付
        intItemLen(6) = {4, 4}      '出発時間

        '登録区分整形
        If strCSVItem(7) <> "済" Then

            strCSVItem(7) = String.Empty

        End If

        'フィールド数確認
        If strCSVItem.Length <> 8 Then

            '項目数エラー
            strReturn = "項目数エラー"

            For i As Integer = 0 To 6

                If strCSVItem(i).Length > intItemLen(i)(1) Then

                    '余分を切り捨て
                    strCSVItem(i) = strCSVItem(i).Substring(0, intItemLen(i)(1))

                End If

            Next

            Exit Sub

        End If

        'フィールドの値からダブルクォーテーションを除去
        strCSVItem(0) = strCSVItem(0).Replace("""", String.Empty).Trim
        strCSVItem(1) = strCSVItem(1).Replace("""", String.Empty).Trim
        strCSVItem(2) = strCSVItem(2).Replace("""", String.Empty).Trim
        strCSVItem(3) = strCSVItem(3).Replace("""", String.Empty).Trim
        strCSVItem(4) = strCSVItem(4).Replace("""", String.Empty).Trim
        strCSVItem(5) = strCSVItem(5).Replace("""", String.Empty).Trim
        strCSVItem(6) = strCSVItem(6).Replace("""", String.Empty).Trim
        strCSVItem(7) = strCSVItem(7).Replace("""", String.Empty).Trim

        '桁数チェック
        For i As Integer = 0 To 6

            '定義がある場合のみチェック
            If intItemLen(i) IsNot Nothing Then

                If strCSVItem(i).Length < intItemLen(i)(0) Then

                    '桁数エラー
                    strReturn = "桁数エラー"

                ElseIf strCSVItem(i).Length > intItemLen(i)(1) Then

                    '余分を切り捨て
                    strCSVItem(i) = strCSVItem(i).Substring(0, intItemLen(i)(1))

                    '桁数エラー
                    strReturn = "桁数エラー"

                Else

                    '桁数が正しい場合、整形
                    Select Case i
                        Case CSVField.作業日付
                            strCSVItem(CSVField.作業日付) = strCSVItem(CSVField.作業日付).Insert(6, "/").Insert(4, "/")
                        Case CSVField.作業時間
                            strCSVItem(CSVField.作業時間) = strCSVItem(CSVField.作業時間).Insert(2, ":")
                        Case CSVField.出発日付
                            strCSVItem(CSVField.出発日付) = strCSVItem(CSVField.出発日付).Insert(6, "/").Insert(4, "/")
                        Case CSVField.出発時間
                            strCSVItem(CSVField.出発時間) = strCSVItem(CSVField.出発時間).Insert(2, ":")
                    End Select

                End If


            End If

        Next

        If strReturn <> String.Empty Then

            Exit Sub

        End If

        '作業日時の形式チェック 妥当性
        If Date.TryParse(strCSVItem(CSVField.作業日付) + " " + strCSVItem(CSVField.作業時間), New Date) = False Then

            '日付形式エラー
            strReturn = "日付形式エラー"

            Exit Sub

        End If

        '出発日時の形式チェック 妥当性
        If Date.TryParse(strCSVItem(CSVField.出発日付) + " " + strCSVItem(CSVField.出発時間), New Date) = False Then

            '日付形式エラー
            strReturn = "日付形式エラー"

            Exit Sub

        End If

        '作業日時の形式チェック 妥当性
        If Date.Parse(strCSVItem(CSVField.出発日付) + " " + strCSVItem(CSVField.出発時間)) > Date.Parse(strCSVItem(CSVField.作業日付) + " " + strCSVItem(CSVField.作業時間)) Then

            '日付形式エラー
            strReturn = "日時前後エラー"

            Exit Sub

        End If

    End Sub

    ''' <summary>
    ''' 活性制御
    ''' </summary>
    Private Sub setEnable(ByVal intEditMode As EditMode)

        Select Case intEditMode

            Case EditMode.未選択

                'ddlInput               .ppEnabled = True
                'ddlDateCls             .ppEnabled = False
                'txtDate_From           .ppEnabled = False
                'txtDate_To             .ppEnabled = False
                'txtCvr                 .Enabled = False
                'trfUplordFileSelection .Enabled = False

                '入力
                ddlImportCls.ppEnabled = True
                txtTBOXID.ppEnabled = False
                ddlArea.ppEnabled = False
                txtDate_From.ppEnabled = False
                txtDate_To.ppEnabled = False
                txtCvr.Enabled = False
                trfUplordFileSelection.Enabled = False

                setTbxArea(0)   'SCLINPP001-008

                'ボタン
                btnTbxArea.Enabled = False
                Master.ppRigthButton3.Enabled = False   '抽出

                'フッター
                Master.Master.ppRigthButton1.Enabled = False  '登録

            Case EditMode.検索

                ddlImportCls.ppEnabled = True
                txtTBOXID.ppEnabled = True
                ddlArea.ppEnabled = True
                txtDate_From.ppEnabled = True
                txtDate_To.ppEnabled = True
                txtCvr.Enabled = False
                trfUplordFileSelection.Enabled = False

                setTbxArea(0)   'SCLINPP001-008

                'ボタン
                btnTbxArea.Enabled = True
                Master.ppRigthButton3.Enabled = True   '抽出

                'フッター
                Master.Master.ppRigthButton1.Enabled = False  '登録

            Case EditMode.取込


                ddlImportCls.ppEnabled = True
                txtTBOXID.ppEnabled = False
                ddlArea.ppEnabled = False
                txtDate_From.ppEnabled = False
                txtDate_To.ppEnabled = False
                txtCvr.Enabled = True
                trfUplordFileSelection.Enabled = True

                setTbxArea(0)   'SCLINPP001-008

                'ボタン
                btnTbxArea.Enabled = False
                Master.ppRigthButton3.Enabled = True   '抽出

                'フッター
                Master.Master.ppRigthButton1.Enabled = False  '登録

        End Select



    End Sub

    ''' <summary>
    ''' 取込み条件クリア
    ''' </summary>
    Private Sub ClearImport()

        ddlImportCls.ppDropDownList.SelectedIndex = -1
        txtTBOXID.ppText = String.Empty
        txtDate_From.ppText = String.Empty
        txtDate_From.ppHourText = String.Empty
        txtDate_From.ppMinText = String.Empty
        txtDate_To.ppText = String.Empty
        txtDate_To.ppHourText = String.Empty
        txtDate_To.ppMinText = String.Empty

        txtCvr.Text = String.Empty
        'trfUplordFileSelection.File

        setTbxArea(0)

        '活性制御
        setEnable(EditMode.未選択)

        'フォーカス
        ddlImportCls.ppDropDownList.Focus()

    End Sub

    ''' <summary>
    ''' TBOXID/エリア 変更制御   'SCLINPP001-008
    ''' </summary>
    Private Sub setTbxArea(ByVal intItem As Integer)

        Dim aryItem(1) As String

        aryItem = {"TBOXID", "エリア"}

        '入力値クリア
        txtTBOXID.ppText = String.Empty
        ddlArea.ppDropDownList.SelectedIndex = -1

        'TBOXID/エリア制御
        Select Case intItem
            Case 0  'TBOXID
                btnTbxArea.Text = aryItem(intItem)
                txtTBOXID.Visible = True
                ddlArea.Visible = False
            Case 1  'エリア
                btnTbxArea.Text = aryItem(intItem)
                txtTBOXID.Visible = False
                ddlArea.Visible = True
            Case Else
                txtTBOXID.Visible = False
                ddlArea.Visible = False
        End Select

    End Sub

    ''' <summary>
    ''' DropDownList設定 'SCLINPP001-008
    ''' </summary>
    Private Sub setDropDownList()

        Dim dstSelect As New DataSet

        'SQLコマンド設定
        Using cmdSQL As New SqlCommand(SCL_COMMON_S01)

            'パラメータ設定
            cmdSQL.Parameters.Add(New SqlParameter("@data_cls", "SCL_MST_RNG"))

            'データ取得
            If ClsCMDataConnect.pfExec_StoredProcedure(Me, "訪問種別マスタ", cmdSQL, dstSelect) = False Then

                Exit Sub

            End If

        End Using

        '訪問先種別
        ddlArea.ppDropDownList.DataSource = dstSelect.Tables(0)
        ddlArea.ppDropDownList.DataValueField = "コード"
        ddlArea.ppDropDownList.DataTextField = "コード名称"
        ddlArea.ppDropDownList.DataBind()
        ddlArea.ppDropDownList.Items.Insert(0, String.Empty)

        'Dispose
        dstSelect.Dispose()

    End Sub

#End Region

End Class
