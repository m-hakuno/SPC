'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　スケジュール管理
'*　ＰＧＭＩＤ：　SCLLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.11.28　：　加賀
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'SCLLSTP001-001     2016/11/28      加賀     XXXX
'SCLLSTP001-002     2017/01/20      栗原     グリッド幅の調整、【未実装】ホール名補完(TBOXID入力時)、メール送信有無判定の修正
'SCLLSTP001-003     2017/02/06      加賀     CRSをWKB,SPCで利用可能に変更

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon

Public Class SCLLSTP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    Const MY_DISP_ID = "SCLLSTP001"
    Const SCL_COMMON_S01 = "SCLCOMMON_S01"

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
        'pfSet_GridView(Me.grvList, MY_DISP_ID, {5}, {2})
        pfSet_GridView(Me.grvList, MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンイベント処理設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click        '検索
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click         '検索条件クリア
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnScheMng_Click       'スケジュール管理
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnScheDetail_Click    'スケジュール明細
        AddHandler ddlVstCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlVstCls_SelectedIndexChanged
        'AddHandler txtVstCd.ppTextBox.TextChanged, AddressOf txtVstCd_Textchanged

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

                '検索
                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton1.CausesValidation = False

                '検索条件クリア
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton2.CausesValidation = False

                'スケジュール管理
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton1.Text = "スケジュール管理"
                Master.Master.ppRigthButton1.CausesValidation = False

                'スケジュール明細
                Master.Master.ppRigthButton2.Visible = True
                Master.Master.ppRigthButton2.Text = "スケジュール明細"
                Master.Master.ppRigthButton2.CausesValidation = False

                'ドロップダウンリスト設定
                setDropDownList()

                'AutoPostBack!
                ddlVstCls.ppDropDownList.AutoPostBack = True
                'SCLLSTP001-002
                'txtVstCd.ppTextBox.AutoPostBack = True
                'SCLLSTP001-002 END


                '一覧表示
                grvList.DataSource = New DataTable
                grvList.DataBind()

                '活性制御
                txtCtrlNo.ppEnabled = False
                txtVstCd.ppEnabled = False

                'フォーカス
                ddlVstCls.ppDropDownList.Focus()

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

        'SCLLSTP001-003
        If Not IsPostBack Then  '初回表示のみ

            Select Case Session(P_SESSION_CRS_USE)
                Case ClsComVer.E_CRS使用制限.使用不可
                    'フッター使用不可
                    Master.Master.ppRigthButton1.Enabled = False
                    Master.Master.ppRigthButton2.Enabled = False
                    '一覧使用不可
                    grvList.Enabled = False
                Case ClsComVer.E_CRS使用制限.参照
                    'フッター使用不可
                    Master.Master.ppRigthButton1.Enabled = False
                    Master.Master.ppRigthButton2.Enabled = False
            End Select

        End If

    End Sub

    ''' <summary>
    ''' 検索ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim dstSelect As New DataSet

        Try
            '作業日時入力値を退避
            Dim strWrkDtFrom(3), strWrkDtTo(3) As String

            strWrkDtFrom = {String.Empty, txtWrkDt_From.ppText, txtWrkDt_From.ppHourText, txtWrkDt_From.ppMinText}
            strWrkDtTo = {String.Empty, txtWrkDt_To.ppText, txtWrkDt_To.ppHourText, txtWrkDt_To.ppMinText}

            '作業日時を編集
            If strWrkDtFrom(1) <> String.Empty AndAlso strWrkDtFrom(2) = String.Empty AndAlso strWrkDtFrom(3) = String.Empty Then
                txtWrkDt_From.ppHourText = "00"
                txtWrkDt_From.ppMinText = "00"
            End If

            If strWrkDtTo(1) <> String.Empty AndAlso strWrkDtTo(2) = String.Empty AndAlso strWrkDtTo(3) = String.Empty Then
                txtWrkDt_To.ppHourText = "23"
                txtWrkDt_To.ppMinText = "59"
            End If

            '入力値検証
            Me.Validate()

            '日時のFROM≦TO確認
            If txtWrkDt_To.ppText <> String.Empty AndAlso txtWrkDt_From.ppText <> String.Empty Then
                If txtWrkDt_From.ppDate > txtWrkDt_To.ppDate Then
                    txtWrkDt_To.psSet_ErrorNo("1006", txtWrkDt_From.ppName & "To", txtWrkDt_From.ppName & "From")     '「{0}」は「{1}」以降の日時で入力してください。
                End If
            End If

            '作業日時入力値を復元
            txtWrkDt_From.ppText = strWrkDtFrom(1)
            txtWrkDt_From.ppHourText = strWrkDtFrom(2)
            txtWrkDt_From.ppMinText = strWrkDtFrom(3)
            txtWrkDt_To.ppText = strWrkDtTo(1)
            txtWrkDt_To.ppHourText = strWrkDtTo(2)
            txtWrkDt_To.ppMinText = strWrkDtTo(3)

            '入力値検証結果
            If (Page.IsValid) = False Then
                Exit Try
            End If

            '作業日時をストアド用にﾌｫｰﾏｯﾄ
            'FROM
            strWrkDtFrom(0) = txtWrkDt_From.ppText.Replace("/", "") & txtWrkDt_From.ppHourText & txtWrkDt_From.ppMinText

            'TO 時間未入力許容するのか？
            If txtWrkDt_To.ppText = String.Empty Then
                strWrkDtTo(0) = String.Empty
            ElseIf txtWrkDt_To.ppHourText = String.Empty Then
                strWrkDtTo(0) = txtWrkDt_To.ppText.Replace("/", "") & "235900"
            Else
                strWrkDtTo(0) = txtWrkDt_To.ppText.Replace("/", "") & txtWrkDt_To.ppHourText & txtWrkDt_To.ppMinText
                strWrkDtTo(0) = strWrkDtTo(0).PadRight(14, "0")
            End If


            'SQLコマンド設定
            Using cmdSQL As New SqlCommand(MY_DISP_ID & "_S01")

                'パラメータ設定
                cmdSQL.Parameters.AddRange({New SqlParameter("@ctrl_no", txtCtrlNo.ppText) _
                                          , New SqlParameter("@vst_cls", ddlVstCls.ppSelectedValue) _
                                          , New SqlParameter("@wrk_dt_from", strWrkDtFrom(0)) _
                                          , New SqlParameter("@wrk_dt_to", strWrkDtTo(0)) _
                                          , New SqlParameter("@vst_cd", txtVstCd.ppText) _
                                          , New SqlParameter("@vst_nm", txtVstNm.ppText) _
                                           })

                'データ更新
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
            Else
                Dim intRcdCnt, intLmtCnt As Integer

                intRcdCnt = DirectCast(dstSelect.Tables(0).Rows(0)("該当件数"), Integer)
                intLmtCnt = DirectCast(dstSelect.Tables(0).Rows(0)("取得上限"), Integer)

                '該当件数
                Master.ppCount = intRcdCnt.ToString

                '閾値オーバーk確認
                If intRcdCnt > intLmtCnt Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, intRcdCnt.ToString, intLmtCnt.ToString)
                End If
            End If

            '一覧にバインド
            grvList.DataSource = dstSelect.Tables(0)
            grvList.DataBind()

            'Dispose
            dstSelect.Dispose()

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "データの検索")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Try

            '入力値クリア
            txtCtrlNo.ppText = String.Empty
            ddlVstCls.ppDropDownList.SelectedIndex = -1
            txtWrkDt_From.ppText = String.Empty
            txtWrkDt_From.ppHourText = String.Empty
            txtWrkDt_From.ppMinText = String.Empty
            txtWrkDt_To.ppText = String.Empty
            txtWrkDt_To.ppHourText = String.Empty
            txtWrkDt_To.ppMinText = String.Empty
            txtVstCd.ppText = String.Empty
            txtVstNm.ppText = String.Empty

            '活性制御
            txtCtrlNo.ppEnabled = False
            txtVstCd.ppEnabled = False

            'フォーカス
            ddlVstCls.ppDropDownList.Focus()

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "入力値クリア")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' スケジュール管理ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnScheMng_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim strPath As String   '遷移先パス

        Try

            '遷移先パス設定
            strPath = String.Format("~/Schedule/{0}/{0}.aspx", "SCLINPP001")

            'セッション情報設定
            psOpen_Window(Me, strPath)

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' スケジュール明細ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnScheDetail_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        Dim strPath As String   '遷移先パス

        Try

            '遷移先パス設定
            strPath = String.Format("~/Schedule/{0}/{0}.aspx", "SCLUPDP001")

            'セッション情報設定
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = MY_DISP_ID
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

            '遷移
            psOpen_Window(Me, strPath)

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 訪問種別　変更時処理
    ''' </summary>
    Protected Sub ddlVstCls_SelectedIndexChanged(sender As Object, e As EventArgs)

        Dim strVstCls As String

        '訪問種別取得
        strVstCls = ddlVstCls.ppSelectedValue

        '活性制御
        If strVstCls = "01" OrElse strVstCls = "02" Then

            txtCtrlNo.ppEnabled = True
            txtVstCd.ppEnabled = True

        Else

            txtCtrlNo.ppEnabled = False
            txtVstCd.ppEnabled = False

            txtCtrlNo.ppText = String.Empty
            txtVstCd.ppText = String.Empty
            'SCLLSTP001-002
            'txtVstNm.ppText = String.Empty
            'SCLLSTP001-002
        End If

        'フォーカス
        SetFocus(txtWrkDt_From.ppDateBox)

    End Sub

    'SCLLSTP001-002
    ''' <summary>
    ''' TBOXID　変更時処理(使用してません)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtVstCd_Textchanged(sender As Object, e As EventArgs)
        Dim strHallname As String = String.Empty

        If txtVstCd.ppText.Trim <> String.Empty Then
            Dim dstSelect As New DataSet
            '--ホール名取得
            Using cmdSQL As New SqlCommand("ZCMPSEL028")

                'パラメータ設定
                cmdSQL.Parameters.AddRange({New SqlParameter("@tboxid", txtVstCd.ppText.Trim)})

                'データ更新
                If ClsCMDataConnect.pfExec_StoredProcedure(Me, "ホール名称取得処理", cmdSQL, dstSelect) = False Then
                    Exit Sub
                End If


                If dstSelect IsNot Nothing Then
                    If dstSelect.Tables(0).Rows.Count <> 0 Then
                        strHallname = dstSelect.Tables(0).Rows(0).Item("ホール名").ToString.Trim
                        txtVstNm.ppTextBox.Focus()
                    End If
                End If
            End Using
        Else
            txtVstCd.ppTextBox.Focus()
        End If

        '--ホール名表示
        txtVstNm.ppText = strHallname

    End Sub
    'SCLLSTP001-002 END


    ''' <summary>
    ''' グリッドのボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Dim rowData As GridViewRow = Nothing        'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing '次画面引継ぎ用キー情報
        Dim strPath As String                       '遷移先パス

        Try
            '遷移先パス設定
            strPath = String.Format("~/Schedule/{0}/{0}.aspx", "SCLUPDP001")

            'ボタン押下行の情報を取得
            rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

            '次画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            strKeyList.Add(CType(rowData.FindControl("メール管理番号1"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("メール管理番号2"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("メール管理番号3"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("訪問種別コード"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("作業日付"), TextBox).Text)
            strKeyList.Add(CType(rowData.FindControl("作業時間"), TextBox).Text)

            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = MY_DISP_ID
            Session(P_KEY) = strKeyList.ToArray

            Select Case e.CommandName
                Case "btnReference"

                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                    '遷移
                    psOpen_Window(Me, strPath)

                Case "btnUpdate"

                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                    '★排他情報確認処理(更新処理の実行)
                    Dim clsExc As New ClsCMExclusive
                    Dim strExclusiveDate As String = String.Empty

                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , MY_DISP_ID _
                                     , New ArrayList({"SCL_TRN_DTIL"}) _
                                     , New ArrayList(strKeyList.ToArray)) = 0 Then

                        '★排他情報のグループ番号をセッション変数に設定
                        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                        '★登録年月日時刻
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                        '遷移
                        psOpen_Window(Me, strPath)

                    Else

                        '排他ロック中

                    End If

            End Select

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面遷移")

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
            'SCLLSTP001-003
            Select Case Session(P_SESSION_CRS_USE)
                Case ClsComVer.E_CRS使用制限.参照

                    '更新ボタン使用不可
                    For Each grvrow As GridViewRow In grvList.Rows

                        '更新ボタン非活性
                        grvrow.Cells(1).Enabled = False

                    Next

                Case ClsComVer.E_CRS使用制限.更新

                    '完了分のみ更新ボタン使用不可
                    For Each grvrow As GridViewRow In grvList.Rows

                        'ボタン編集
                        If DirectCast(grvrow.FindControl("完了"), TextBox).Text = "1" Then

                            '更新ボタン非活性
                            grvrow.Cells(1).Enabled = False

                        End If

                    Next

            End Select

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
        ddlVstCls.ppDropDownList.DataSource = dstSelect.Tables(0)
        ddlVstCls.ppDropDownList.DataValueField = "コード"
        ddlVstCls.ppDropDownList.DataTextField = "コード名称"
        ddlVstCls.ppDropDownList.DataBind()
        ddlVstCls.ppDropDownList.Items.Insert(0, String.Empty)

        'Dispose
        dstSelect.Dispose()

    End Sub


#End Region

End Class
