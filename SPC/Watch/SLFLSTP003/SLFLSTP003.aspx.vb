'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　券売入金機自走　履歴表示
'*　ＰＧＭＩＤ：　SLFLSTP003
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作成：2018/03/29：伯野
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'排他制御用
Imports SPC.ClsCMExclusive

Public Class SLFLSTP003

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
#Region "定数定義"

    Private Const M_DISP_ID As String = P_FUN_SLF & P_SCR_LST & P_PAGE & "003"
    Private Const M_CTRL_NO As String = "照会管理番号"
    Private Const M_NL_CLS As String = "ＮＬ区分"
    Private Const M_ID_IC_CLS As String = "ＩＤ／ＩＣ区分"
    Private Const M_NEW_CTRL_NO As String = "新管理番号"
    Private Const M_UPD_CTRL_NO As String = "管理番号"

#End Region

    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================

    '============================================================================================================================
    '=　イベントプロシージャ定義
    '============================================================================================================================
#Region "イベントプロシージャ"

    ''' <summary>
    ''' 画面処理開始
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(Me.grvList, "SLFLSTP003", "SLFLSTP003_Header", Me.DivOut, "L", 40)

    End Sub

    ''' <summary>
    ''' 画面　ロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objWKDS As New DataSet

        'ボタンアクションの設定
        '        AddHandler Master.ppLeftButton1.Click, AddressOf btnPrint_Click

        If Not IsPostBack Then  '初回表示
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
            '            Master.ppLogout_Mode = Global_asax. ClsComVer.E_ログアウトモード.閉じる
            Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

            'プログラムＩＤ、画面名設定
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'セッションから情報取得
            If Not mfGet_Session() Then
                psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                psClose_Window(Me)
                Return
            End If

            If ViewState("CNT").ToString = "1" Then
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID).Replace("１年履歴", "１ヶ月履歴")
            End If

            '対応結果リスト追加
            msSetddlStatus()

            '一覧バインド
            If mfGet_Data(objWKDS) = True Then
                Me.grvList.DataSource = objWKDS.Tables(0)
                Me.lblDateV.Text = objWKDS.Tables(0).Rows(0).Item("日付範囲").ToString
            Else
                Me.grvList.DataSource = New DataTable
                Me.lblDateV.Text = ""
            End If
            Me.grvList.DataBind()

            msSet_Mode(ViewState(P_SESSION_TERMS))

            msChangeBtn("初期")

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
'                btnWatch.Visible = False
'                btnUpdate.Enabled = False
            Case "NGC"
                '                btnUpdate.Enabled = False
                '                btnWatch.Visible = False
                lblWrncontentN.Visible = False
                lblWrncontentV.Visible = False

        End Select

    End Sub

    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        'ヘッダー、フッターは処理なし
        If e.Row.RowIndex = -1 Then
            Exit Sub
        End If

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
                e.Row.Cells(15).Enabled = False
                e.Row.Cells(16).Enabled = False
        End Select

        If e.Row.Cells(15).Enabled = True Then
            Dim strTellNum As String = String.Empty           '電話番号
            Dim ctrl As Control = e.Row.Cells(21).Controls(0)

            If ctrl.GetType Is GetType(TextBox) Then
                '電話番号を取得する
                strTellNum = DirectCast(ctrl, Label).Text
                If strTellNum = String.Empty Then
                    '電話番号がなければTEL発信ボタン使えません
                    e.Row.Cells(15).Enabled = False
                Else
                    'Javascript埋め込み
                    Dim js_exe As String = String.Empty
                    js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                    e.Row.Cells(15).Attributes.Add("onclick", pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' 券売入金機自走ホール一覧選択処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing
        Dim strKeyList As List(Of String) = Nothing                 '次画面引継ぎ用キー情報

        If e.CommandName <> "btnSelect" And e.CommandName <> "btnTEL" And e.CommandName <> "btnHall" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        intIndex = Convert.ToInt32(e.CommandArgument)   ' ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                ' ボタン押下行

        Select Case e.CommandName
            Case "btnSelect"

                '★項目の初期化
                Me.lblSeqV.Text = String.Empty
                Me.lblEWV.Text = String.Empty
                Me.lblTboxidV.Text = String.Empty
                Me.lblJBNumV.Text = String.Empty
                Me.lblLogunyouDTV.Text = String.Empty
                Me.lblLoghasseiDTV.Text = String.Empty
                Me.lblLoghasseiTimeV.Text = String.Empty
                Me.lblBB1serialNoV.Text = String.Empty
                Me.lblSoftBBClassV.Text = String.Empty
                Me.ddlDealDtl1Cd.SelectedValue = String.Empty
                Me.txtDealDtl2.ppText = String.Empty
                Me.lblWrncontentV.Text = String.Empty
                Me.lblHallnmV.Text = String.Empty

                Me.lblSeqV.Text = CType(rowData.FindControl("連番"), Label).Text
                Me.lblEWV.Text = CType(rowData.FindControl("東西区分"), Label).Text
                Me.lblTboxidV.Text = CType(rowData.FindControl("ＴＢＯＸＩＤ"), Label).Text
                Me.lblJBNumV.Text = CType(rowData.FindControl("ＪＢ番号"), Label).Text
                Me.lblLogunyouDTV.Text = CType(rowData.FindControl("運用日"), Label).Text
                Me.lblLoghasseiDTV.Text = CType(rowData.FindControl("発生日"), Label).Text
                Me.lblLoghasseiTimeV.Text = CType(rowData.FindControl("発生時刻"), Label).Text
                Me.lblBB1serialNoV.Text = CType(rowData.FindControl("拡張ＢＢ１シリアル"), Label).Text
                Me.lblSoftBBClassV.Text = CType(rowData.FindControl("自走開始ＢＢ機種コード"), Label).Text
                Me.ddlDealDtl1Cd.SelectedValue = CType(rowData.FindControl("対応リスト"), Label).Text
                Me.txtDealDtl2.ppText = CType(rowData.FindControl("対応結果フリー入力"), Label).Text
                Me.lblWrncontentV.Text = CType(rowData.FindControl("注意情報"), Label).Text
                Me.lblHallnmV.Text = CType(rowData.FindControl("ホール名"), Label).Text
                '更新用非表示項目
                ViewState(M_NL_CLS) = CType(rowData.FindControl("ＮＬ区分"), Label).Text
                ViewState(M_ID_IC_CLS) = CType(rowData.FindControl("ＩＤ／ＩＣ区分"), Label).Text
                ViewState(M_NEW_CTRL_NO) = CType(rowData.FindControl("新管理番号"), Label).Text
                ViewState(M_UPD_CTRL_NO) = CType(rowData.FindControl("管理番号"), Label).Text
            Case "btnTEL"

            Case "btnHall"
                '次画面引継ぎ用キー情報設定
                strKeyList = New List(Of String)
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text     'パンくずリスト
                strKeyList.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), Label).Text)
                strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), Label).Text)
                strKeyList.Add(CType(rowData.FindControl("ＩＤ／ＩＣ区分"), Label).Text)
                strKeyList.Add(CType(rowData.FindControl("ホールコード"), Label).Text)
                Session(P_KEY) = strKeyList.ToArray

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
                                objStack.GetMethod.Name, "~/" & P_COM & "/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx", strPrm, "TRANS")

                psOpen_Window(Me, "~/" & P_COM & "/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx")
        End Select
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ' ''' <summary>
    ' ''' 印刷ボタン押下処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

    '    'ログ出力開始
    '    psLogStart(Me)

    '    psPrintPDF(Me, New SLFREP001, "EXEC SLFLSTP003_S2 " & "'" & ViewState(M_CTRL_NO).ToString & "'", "IC系券売入金機 自走調査対応報告書")

    '    'ログ出力終了
    '    psLogEnd(Me)

    'End Sub

#End Region

    '============================================================================================================================
    '=　プロシージャ定義
    '============================================================================================================================
#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGet_Data(ByRef cpobjWKDS As DataSet) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim objDumTbl As New DataTable

        objStack = New StackFrame

        mfGet_Data = False

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("SLFLSTP003_S1", conDB)
                cmdDB.Parameters.Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, DirectCast(ViewState("TBOXID"), String))) 'TBOXID
                cmdDB.Parameters.Add(pfSet_Param("JBNO", SqlDbType.NVarChar, DirectCast(ViewState("JBNO"), String)))     'JBNO
                cmdDB.Parameters.Add(pfSet_Param("KIJUN", SqlDbType.NVarChar, DirectCast(ViewState("KIJUN"), String))) '取得日
                cmdDB.Parameters.Add(pfSet_Param("CNT", SqlDbType.NVarChar, DirectCast(ViewState("CNT"), String)))       '件数
                'リストデータ取得
                cpobjWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売入金機自走調査履歴一覧")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                cpobjWKDS = New DataSet
                cpobjWKDS.Tables.Add(New DataTable)
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try

            If cpobjWKDS Is Nothing Then
            Else
                If cpobjWKDS.Tables.Count > 0 Then
                    If cpobjWKDS.Tables(0).Rows.Count > 0 Then
                        mfGet_Data = True
                    End If

                End If
            End If

        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode"></param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件)

        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.更新
                'ボタン設定
'                Master.ppLeftButton1.Text = "印刷"
'                Master.ppLeftButton1.Visible = True
            Case ClsComVer.E_遷移条件.参照
                'ボタン設定
                '                Master.ppLeftButton1.Text = "印刷"
                '                Master.ppLeftButton1.Visible = True

                '                Me.btnUpdate.Enabled = False        '更新
                Me.ddlDealDtl1Cd.Enabled = False    '対応結果
                Me.txtDealDtl2.ppEnabled = False    '対応結果

        End Select
    End Sub

    ''' <summary>
    ''' セッション情報を取得する。
    ''' </summary>
    ''' <returns>OK:True NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Session()

        Dim strKey As String()

        'セッションからキー項目取得
        strKey = DirectCast(Session(P_KEY), String).Split(",")

        If strKey Is Nothing Then
            Return False
        End If

        If strKey(0) Is Nothing Then
            Return False
        End If

        If strKey(1) Is Nothing Then
            Return False
        End If

        If strKey(2) Is Nothing Then
            Return False
        End If

        If strKey(3) Is Nothing Then
            Return False
        End If

        If Session(P_SESSION_TERMS) Is Nothing Then
            Return False
        End If

        '画面設定
        ViewState("TBOXID") = strKey(0)
        ViewState("JBNO") = strKey(1)
        ViewState("KIJUN") = strKey(2)
        ViewState("CNT") = strKey(3)
        ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
        'ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

        '★排他情報用のグループ番号保管
        If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
        End If

        '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
        If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
            Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
        End If

        Return True

    End Function

    ''' <summary>
    ''' ボタン活性非活性制御
    ''' </summary>
    ''' <param name="flag"></param>
    ''' <remarks></remarks>
    Private Sub msChangeBtn(ByVal flag As String)

        Select Case flag
            Case "初期"
                '                Me.btnUpdate.Enabled = False
                '                Me.btnWatch.Enabled = False
                Me.txtDealDtl2.ppEnabled = False
                Me.ddlDealDtl1Cd.Enabled = False
            Case "監視"
'                Me.btnWatch.Enabled = False
            Case "選択"
                '                Me.btnUpdate.Enabled = True
                '                Me.btnWatch.Enabled = True
                Me.txtDealDtl2.ppEnabled = True
                Me.ddlDealDtl1Cd.Enabled = True
        End Select

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus()

        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "56"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlDealDtl1Cd.Items.Clear()
                Me.ddlDealDtl1Cd.DataSource = dstOrders.Tables(0)
                Me.ddlDealDtl1Cd.DataTextField = "進捗ステータス名"
                Me.ddlDealDtl1Cd.DataValueField = "進捗ステータス"
                Me.ddlDealDtl1Cd.DataBind()
                Me.ddlDealDtl1Cd.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

#End Region

End Class
