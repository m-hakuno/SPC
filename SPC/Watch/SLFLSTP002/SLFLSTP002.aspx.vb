'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　券売入金機自走ホール一覧
'*　ＰＧＭＩＤ：　SLFLSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.13　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

'--------------------------------
'2014/04/15 高松　ここから
'--------------------------------
'排他制御用
Imports SPC.ClsCMExclusive
'--------------------------------
'2014/04/15 高松　ここまで
'--------------------------------

#End Region

Public Class SLFLSTP002

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================

    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
#Region "構造体・列挙体定義"
    Private Const M_DISP_ID As String = P_FUN_SLF & P_SCR_LST & P_PAGE & "002"
    Private Const M_CTRL_NO As String = "照会管理番号"
    Private Const M_NL_CLS As String = "ＮＬ区分"
    Private Const M_ID_IC_CLS As String = "ＩＤ／ＩＣ区分"
    '--------------------------------
    '2014/04/15 高松　ここから
    '--------------------------------
    Private Const M_NEW_CTRL_NO As String = "新管理番号"
    Private Const M_UPD_CTRL_NO As String = "管理番号"
    '--------------------------------
    '2014/04/15 高松　ここまで
    '--------------------------------
#End Region

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"
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

#End Region

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#Region "プロパティ定義"
#End Region

    '============================================================================================================================
    '=　イベントプロシージャ定義
    '============================================================================================================================
#Region "イベントプロシージャ"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(Me.grvList, "SLFLSTP002", "SLFLSTP002_Header", Me.DivOut)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim objwkDS As New DataSet

        'ボタンアクションの設定
        AddHandler Master.ppLeftButton1.Click, AddressOf btnPrint_Click
        Me.txtDealDtl2.ppTextBox.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txtDealDtl2.ppMaxLength & """);")
        Me.txtDealDtl2.ppTextBox.Attributes.Add("onChange", "lenCheck(this,""" & Me.txtDealDtl2.ppMaxLength & """);")

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

            Me.btnWatch.OnClientClick = pfGet_OCClickMes("30004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視報告書兼依頼書")
            Me.btnUpdate.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "券売入金機　自走ホール")
            Master.ppLeftButton1.OnClientClick =
            pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "IC系券売入金機 自走調査対応報告書")

            'セッションから情報取得
            If Not mfGet_Session() Then
                psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------
                psClose_Window(Me)
                Return
            End If

            '対応結果リスト追加
            msSetddlStatus()

            '一覧検索
            If mfGet_Data(ViewState(M_CTRL_NO).ToString, objwkDS) Then
                Me.grvList.DataSource = objwkDS.Tables(0)
            Else
            End If
            Me.grvList.DataBind()

            msSet_Mode(ViewState(P_SESSION_TERMS))

            '--------------------------------
            '2014/04/15 高松　ここから
            '--------------------------------
            msChangeBtn("初期")
            '--------------------------------
            '2014/04/15 高松　ここまで
            '--------------------------------

        End If

    End Sub

    '---------------------------
    '2014/04/25 武 ここから
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
                btnWatch.Visible = False
                btnUpdate.Enabled = False
                '                btnPrtHist.Enabled = False
                '                btnHist.Enabled = False
                ddlDealDtl1Cd.Enabled = False
                txtDealDtl2.ppTextBox.Enabled = False
                chkExclusion.Enabled = False
            Case "NGC"
                btnUpdate.Enabled = False
                btnWatch.Visible = False
                '                btnPrtHist.Enabled = True
                '                btnHist.Enabled = True
                lblWrncontentN.Text = ""
                lblWrncontentV.Text = ""
                ddlDealDtl1Cd.Enabled = False
                txtDealDtl2.ppTextBox.Enabled = False
                chkExclusion.Enabled = False
        End Select

    End Sub

    ''' <summary>
    ''' 明細設置時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
                e.Row.Cells(1).Enabled = False
                e.Row.Cells(2).Enabled = False
        End Select

        If e.Row.Cells(2).Enabled = True Then
            Dim strTellNum As String = String.Empty           '電話番号
            Dim ctrl As Control = e.Row.Cells(22).Controls(0)

            If ctrl.GetType Is GetType(TextBox) Then
                '電話番号を取得する
                strTellNum = DirectCast(ctrl, TextBox).Text
                If strTellNum = String.Empty Then
                    '電話番号がなければTEL発信ボタン使えません
                    e.Row.Cells(2).Enabled = False
                Else
                    'Javascript埋め込み
                    Dim js_exe As String = String.Empty
                    js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                    e.Row.Cells(2).Attributes.Add("onclick", pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe)
                End If
            End If
        End If
    End Sub
    '---------------------------
    '2014/04/25 武 ここまで
    '---------------------------

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

                '--------------------------------
                '2014/04/15 高松　ここから
                '--------------------------------
                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

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

                Select Case ViewState(P_SESSION_TERMS)
                    Case ClsComVer.E_遷移条件.更新

                        '★排他情報削除
                        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

                            clsExc.pfDel_Exclusive(Me _
                                          , Session(P_SESSION_SESSTION_ID) _
                                          , Me.Master.ppExclusiveDateDtl)

                            Me.Master.ppExclusiveDateDtl = String.Empty

                        End If

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D173_KENBAIKIJISOU")

                        '★ロックテーブルキー項目の登録(D173_KENBAIKIJISOU)
                        arKey.Insert(0, (CType(rowData.FindControl("管理番号"), TextBox).Text))
                        arKey.Insert(1, (CType(rowData.FindControl("連番"), TextBox).Text))
                        arKey.Insert(2, (CType(rowData.FindControl("ＮＬ区分"), TextBox).Text))
                        arKey.Insert(3, (CType(rowData.FindControl("ＩＤ／ＩＣ区分"), TextBox).Text))
                        arKey.Insert(4, (CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text))
                        arKey.Insert(5, (CType(rowData.FindControl("ＪＢ番号"), TextBox).Text))

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

                            ''★登録年月日時刻(明細)
                            'Master.ppExclusiveDateDtl = strExclusiveDate
                            '登録年月日時刻(明細)に登録.
                            Me.Master.ppExclusiveDateDtl = strExclusiveDate
                            msChangeBtn("選択")

                        Else

                            '排他ロック中
                            Exit Sub

                        End If
                    Case ClsComVer.E_遷移条件.参照
                        msChangeBtn("参照")
                End Select

                Me.lblSeqV.Text = CType(rowData.FindControl("連番"), TextBox).Text
                Me.lblEWV.Text = CType(rowData.FindControl("東西区分"), TextBox).Text
                Me.lblTboxidV.Text = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                Me.lblJBNumV.Text = CType(rowData.FindControl("ＪＢ番号"), TextBox).Text
                Me.lblLogunyouDTV.Text = CType(rowData.FindControl("運用日"), TextBox).Text
                Me.lblLoghasseiDTV.Text = CType(rowData.FindControl("発生日"), TextBox).Text
                Me.lblLoghasseiTimeV.Text = CType(rowData.FindControl("発生時刻"), TextBox).Text
                Me.lblBB1serialNoV.Text = CType(rowData.FindControl("拡張ＢＢ１シリアル"), TextBox).Text
                Me.lblSoftBBClassV.Text = CType(rowData.FindControl("自走開始ＢＢ機種コード"), TextBox).Text
                Me.ddlDealDtl1Cd.SelectedValue = CType(rowData.FindControl("対応リスト"), TextBox).Text
                Me.txtDealDtl2.ppText = CType(rowData.FindControl("対応結果フリー入力"), TextBox).Text
                Me.lblWrncontentV.Text = CType(rowData.FindControl("注意情報"), TextBox).Text
                Me.lblHallnmV.Text = CType(rowData.FindControl("ホール名"), TextBox).Text
                If CType(rowData.FindControl("引継ぎフラグ"), TextBox).Text = "1" Then
                    Me.chkExclusion.Checked = True
                Else
                    Me.chkExclusion.Checked = False
                End If


                '更新用非表示項目
                ViewState(M_NL_CLS) = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text
                ViewState(M_ID_IC_CLS) = CType(rowData.FindControl("ＩＤ／ＩＣ区分"), TextBox).Text
                ViewState(M_NEW_CTRL_NO) = CType(rowData.FindControl("新管理番号"), TextBox).Text
                ViewState(M_UPD_CTRL_NO) = CType(rowData.FindControl("管理番号"), TextBox).Text
                '--------------------------------
                '2014/04/15 高松　ここまで
                '--------------------------------

            Case "btnTEL"

            Case "btnHall"
                '次画面引継ぎ用キー情報設定
                strKeyList = New List(Of String)
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text     'パンくずリスト
                strKeyList.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ＩＤ／ＩＣ区分"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ホールコード"), TextBox).Text)
                Session(P_KEY) = strKeyList.ToArray
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
                                objStack.GetMethod.Name, "~/" & P_COM & "/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx", strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                psOpen_Window(Me, "~/" & P_COM & "/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006/" &
                              P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx")
        End Select
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        psPrintPDF(Me, New SLFREP001, "EXEC SLFLSTP002_S2 " & "'" & ViewState(M_CTRL_NO).ToString & "'", "IC系券売入金機 自走調査対応報告書")

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer
        Dim strExclusion As String = ""
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        If (Page.IsValid) Then
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    If chkExclusion.Checked = True Then
                        strExclusion = "1"
                    Else
                        strExclusion = "0"
                    End If
                    cmdDB = New SqlCommand("SLFLSTP002_U1", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, ViewState(M_UPD_CTRL_NO)))          '照会管理番号
                        .Add(pfSet_Param("seq", SqlDbType.Int, Me.lblSeqV.Text))                        '連番
                        .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, ViewState(M_NL_CLS)))            'ＮＬ区分
                        .Add(pfSet_Param("id_ic_cls", SqlDbType.NVarChar, ViewState(M_ID_IC_CLS)))      'ＩＤ／ＩＣ区分
                        .Add(pfSet_Param("tboxid", SqlDbType.NChar, Me.lblTboxidV.Text))                'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("jbnumeric", SqlDbType.NChar, Me.lblJBNumV.Text))              'ＪＢ番号
                        .Add(pfSet_Param("deal_dtl1_cd", SqlDbType.NVarChar, Me.ddlDealDtl1Cd.SelectedValue)) '対応内容１コード
                        .Add(pfSet_Param("deal_dtl12", SqlDbType.NVarChar, Me.txtDealDtl2.ppText))      '対応内容２
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))             'ユーザーＩＤ
                        .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, strExclusion))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                               '戻り値

                    End With

                    'データ追加／更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        'コマンドタイプ設定(ストアド)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自走中情報", intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "自走中情報")
                Catch ex As Exception
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自走中情報")
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

            '一覧検索
            If mfGet_Data(ViewState(M_CTRL_NO).ToString, dstOrders) Then
                Me.grvList.DataSource = dstOrders.Tables(0)
            Else
                Me.grvList.DataSource = New DataTable
            End If
            Me.grvList.DataBind()

        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 監視報告兼依頼票
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnWatch_Click(sender As Object, e As EventArgs) Handles btnWatch.Click

        'ログ出力開始
        psLogStart(Me)
        '--------------------------------
        '2014/04/15 高松　ここから
        '--------------------------------
        Dim strKeyList = New List(Of String)
        '--------------------------------
        '2014/04/15 高松　ここまで
        '--------------------------------
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号
        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)                       '遷移条件
        Session(P_SESSION_OLDDISP) = M_DISP_ID                                      '遷移元ID
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text                            'パンくずリスト


        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("SLFLSTP002_S3", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, ViewState(M_UPD_CTRL_NO)))
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.lblTboxidV.Text))
                    .Add(pfSet_Param("jbno", SqlDbType.NVarChar, Me.lblJBNumV.Text))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売入金機自走ホール")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Exit Sub
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Exit Sub
        End If


        '--------------------------------
        '2014/04/15 高松　ここから
        '--------------------------------
        'Session(P_KEY)
        If dstOrders Is Nothing OrElse dstOrders.Tables(0).Rows.Count = 0 Then
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

            strKeyList.Add(M_DISP_ID)                                 '画面ID
            strKeyList.Add("1")                                       '新管理番号有無フラグ
            strKeyList.Add(ViewState(M_UPD_CTRL_NO))                  '管理番号
            strKeyList.Add(Me.lblSeqV.Text)                           '連番
            strKeyList.Add(ViewState(M_ID_IC_CLS))                    'ID_IC区分
            strKeyList.Add(ViewState(M_NL_CLS))                       'ＮＬ区分
            strKeyList.Add(Me.lblJBNumV.Text)                         'ＪＢ番号
            strKeyList.Add("0")                                       '新管理番号
            strKeyList.Add(Me.lblTboxidV.Text)                        'TBOXID
            strKeyList.Add(Me.txtDealDtl2.ppText)                     '対応内容

        Else
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

            '★排他制御処理
            Dim strExclusiveDate As String = Nothing
            Dim arTable_Name As New ArrayList
            Dim arKey As New ArrayList

            '★ロック対象テーブル名の登録
            arTable_Name.Insert(0, "D36_MNTRREPORT")

            '★ロックテーブルキー項目の登録(D36_MNTRREPORT_NO)
            arKey.Insert(0, (ViewState(M_NEW_CTRL_NO)))

            '★排他情報確認処理(更新画面へ遷移)
            If clsExc.pfSel_Exclusive(strExclusiveDate _
                             , Me _
                             , Session(P_SESSION_IP) _
                             , Session(P_SESSION_PLACE) _
                             , Session(P_SESSION_USERID) _
                             , Session(P_SESSION_SESSTION_ID) _
                             , ViewState(P_SESSION_GROUP_NUM) _
                             , P_FUN_WAT & P_SCR_UPD & P_PAGE & "001" _
                             , arTable_Name _
                             , arKey) = 0 Then

                'キー情報の設定
                strKeyList.Add(M_DISP_ID)                                 '画面ID
                strKeyList.Add("2")                                       '新管理番号有無フラグ
                strKeyList.Add(ViewState(M_UPD_CTRL_NO))                  '管理番号
                strKeyList.Add(Me.lblSeqV.Text)                           '連番
                strKeyList.Add(ViewState(M_ID_IC_CLS))                    'ID_IC区分
                strKeyList.Add(ViewState(M_NL_CLS))                       'ＮＬ区分
                strKeyList.Add(Me.lblJBNumV.Text)                         'ＪＢ番号
                strKeyList.Add(dstOrders.Tables(0).Rows(0).Item("D36_MNTRREPORT_NO").ToString)                  '新管理番号
                strKeyList.Add(Me.lblTboxidV.Text)                        'TBOXID
                strKeyList.Add(Me.txtDealDtl2.ppText)                     '対応内容

                '★登録年月日時刻
                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
            Else

                '排他ロック中
                Exit Sub

            End If
        End If
        Session(P_KEY) = strKeyList.ToArray
        '--------------------------------
        '2014/04/15 高松　ここまで
        '--------------------------------
        '--------------------------------
        '2014/04/16 星野　ここから
        '--------------------------------
        '■□■□結合試験時のみ使用予定□■□■
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
                        objStack.GetMethod.Name, "~/" & P_WAT & "/" &
                      P_FUN_WAT & P_SCR_UPD & P_PAGE & "001/" &
                      P_FUN_WAT & P_SCR_UPD & P_PAGE & "001.aspx", strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        psOpen_Window(Me, "~/" & P_WAT & "/" &
                      P_FUN_WAT & P_SCR_UPD & P_PAGE & "001/" &
                      P_FUN_WAT & P_SCR_UPD & P_PAGE & "001.aspx")

        '--------------------------------
        '2014/04/15 高松　ここから
        '--------------------------------
        Select Case ViewState(P_SESSION_TERMS)
            Case ClsComVer.E_遷移条件.更新
                msChangeBtn("監視")
        End Select
        '--------------------------------
        '2014/04/15 高松　ここまで
        '--------------------------------

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 履歴表示ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnHist_Click(sender As Object, e As EventArgs) Handles btnHist.Click

        'ログ出力開始
        psLogStart(Me)

        Dim strKeyList = New List(Of String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号
        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)                       '遷移条件
        Session(P_SESSION_OLDDISP) = M_DISP_ID                                      '遷移元ID
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text                            'パンくずリスト

        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
        Session(P_KEY) = Me.lblTboxidV.Text & "," & Me.lblJBNumV.Text & "," & lblDateV.Text & ",0"

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                        objStack.GetMethod.Name, "~/" & P_WAT & "/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003.aspx", Me.lblTboxidV.Text & "," & Me.lblJBNumV.Text, "TRANS")
        psOpen_Window(Me, "~/" & P_WAT & "/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003.aspx")

        'ログ出力終了
        psLogEnd(Me)

    End Sub


    ''' <summary>
    ''' 直近履歴ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrtHist_Click(sender As Object, e As EventArgs) Handles btnPrtHist.Click

        'ログ出力開始
        psLogStart(Me)

        Dim strKeyList = New List(Of String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        objStack = New StackFrame

        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号
        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)                       '遷移条件
        Session(P_SESSION_OLDDISP) = M_DISP_ID                                      '遷移元ID
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text                            'パンくずリスト

        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
        Session(P_KEY) = Me.lblTboxidV.Text & "," & Me.lblJBNumV.Text & "," & lblDateV.Text & ",1"

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                        objStack.GetMethod.Name, "~/" & P_WAT & "/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003.aspx", Me.lblTboxidV.Text & "," & Me.lblJBNumV.Text, "TRANS")
        psOpen_Window(Me, "~/" & P_WAT & "/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003/" &
                      P_FUN_SLF & P_SCR_LST & P_PAGE & "003.aspx")

        'ログ出力終了
        psLogEnd(Me)

    End Sub

#End Region

    '============================================================================================================================
    '=　プロシージャ定義
    '============================================================================================================================
#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 一覧情報を検索する。
    ''' </summary>
    ''' <param name="ipstrCtrlNo">照会管理番号</param>
    ''' <returns>一覧情報</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Data(ByVal ipstrCtrlNo As String, ByRef dstorders As DataSet) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim blnRet As Boolean = False
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
                cmdDB = New SqlCommand("SLFLSTP002_S1", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, ipstrCtrlNo))           '照会管理番号
                End With

                'リストデータ取得
                dstorders = clsDataConnect.pfGet_DataSet(cmdDB)

                blnRet = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売入金機自走調査一覧")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                dstorders = New DataSet
                dstorders.Tables.Add(New DataTable)
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            dstorders = New DataSet
            dstorders.Tables.Add(New DataTable)
        End If

        Return blnRet

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
                Master.ppLeftButton1.Text = "印刷"
                Master.ppLeftButton1.Visible = True
            Case ClsComVer.E_遷移条件.参照
                'ボタン設定
                Master.ppLeftButton1.Text = "印刷"
                Master.ppLeftButton1.Visible = True

                Me.btnUpdate.Enabled = False        '更新
                Me.btnHist.Enabled = False
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
        '--2014/04/23 中川　ここから
        Dim strKey(1) As String
        '--2014/04/23 中川　ここまで

        'セッションからキー項目取得
        strKey = Session(P_KEY)
        If strKey Is Nothing Then
            Return False
        End If

        If strKey(0) Is Nothing Then
            Return False
        End If

        If strKey(1) Is Nothing Then
            Return False
        End If

        If Session(P_SESSION_TERMS) Is Nothing Then
            Return False
        End If

        '画面設定
        ViewState(M_CTRL_NO) = strKey(0)
        Me.lblDateV.Text = strKey(1)
        ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
        '---------------------------
        '2014/06/10 後藤 ここから
        '---------------------------
        'ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

        '★排他情報用のグループ番号保管
        If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

        End If

        '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
        If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

            Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

        End If
        '---------------------------
        '2014/06/10 後藤 ここまで
        '---------------------------
        Return True

    End Function

    '--------------------------------
    '2014/04/15 高松　ここから
    '--------------------------------
    ''' <summary>
    ''' ボタン活性非活性制御
    ''' </summary>
    ''' <param name="flag"></param>
    ''' <remarks></remarks>
    Private Sub msChangeBtn(ByVal flag As String)

        Select Case flag
            Case "初期"
                Me.btnUpdate.Enabled = False
                Me.btnHist.Enabled = False
                Me.btnPrtHist.Enabled = False
                Me.btnWatch.Enabled = False
                Me.txtDealDtl2.ppEnabled = False
                Me.ddlDealDtl1Cd.Enabled = False
                Me.chkExclusion.Enabled = False
            Case "監視"
                Me.btnWatch.Enabled = False
            Case "選択"
                Me.btnUpdate.Enabled = True
                Me.btnHist.Enabled = True
                Me.btnPrtHist.Enabled = True
                Me.btnWatch.Enabled = True
                Me.txtDealDtl2.ppEnabled = True
                Me.ddlDealDtl1Cd.Enabled = True
                Me.chkExclusion.Enabled = True
            Case "参照"
                Me.btnUpdate.Enabled = False
                Me.btnHist.Enabled = True
                Me.btnPrtHist.Enabled = True
                Me.btnWatch.Enabled = False
                Me.txtDealDtl2.ppEnabled = False
                Me.ddlDealDtl1Cd.Enabled = False
                Me.chkExclusion.Enabled = False
        End Select

    End Sub
    '--------------------------------
    '2014/04/15 高松　ここまで
    '--------------------------------

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus()

        Dim conDB As SqlConnection = Nothing    'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing      'SqlCommandクラス
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
        End If

    End Sub

    '--------------------------------
    '2014/05/09 稲葉　ここから
    '--------------------------------
    ''--------------------------------
    ''2014/04/15 高松　ここから
    ''--------------------------------
    ' ''' <summary>
    ' ''' データバインド時処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

    '    Dim strTellNum As String = String.Empty        '電話番号
    '    Dim strVs() As String = ViewState(P_KEY)
    '    Dim js_exe As String = String.Empty

    '    'CallToプロトコルの設定
    '    For Each rowData As GridViewRow In grvList.Rows
    '        strTellNum = CType(rowData.FindControl("ホールTEL"), TextBox).Text
    '        If strTellNum Is Nothing Or strTellNum = String.Empty Then

    '            rowData.Cells(20).Enabled = False

    '        Else

    '            js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + "); return false;"
    '            rowData.Cells(20).Attributes.Add("onclick", js_exe)

    '        End If

    '    Next

    'End Sub
    ''--------------------------------
    ''2014/04/15 高松　ここまで
    ''--------------------------------
    '--------------------------------
    '2014/05/09 稲葉　ここまで
    '--------------------------------

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
