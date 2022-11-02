#Region "プログラムヘッダ"
'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　物品転送依頼一覧
'*　ＰＧＭＩＤ：　CNSLSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.28　：　土岐
'********************************************************************************************************************************
#End Region

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

#Region "クラス定義"
Public Class CNSLSTP002
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
    '物品転送依頼書 参照／更新 画面のパス
    Private Const M_CNSUPDP002 As String = "~/" & P_CNS & "/" &
        P_FUN_CNS & P_SCR_UPD & P_PAGE & "002" & "/" &
        P_FUN_CNS & P_SCR_UPD & P_PAGE & "002.aspx"
    Private Const M_DISP_ID = P_FUN_CNS & P_SCR_LST & P_PAGE & "002"
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

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        'pfSet_GridView(grvList, "CNSLSTP002")
        pfSet_GridView(Me.grvList, "CNSLSTP002", "L")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnDay_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnAdd_Click

        If Not IsPostBack Then  '初回表示

            '「クリア」「当日」「登録」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False
            Master.ppRigthButton3.CausesValidation = False
            Master.Master.ppRigthButton1.CausesValidation = False

            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '「当日」、「登録」のボタン活性
            Master.ppRigthButton3.Text = "当日"
            Master.ppRigthButton3.Visible = True
            Master.Master.ppRigthButton1.Text = P_BTN_NM_ADD
            Master.Master.ppRigthButton1.Visible = True

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '画面初期化処理
            msClearScreen()

        End If

    End Sub

    '---------------------------
    '2014/04/14 武 ここから
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
                Master.Master.ppRigthButton1.Enabled = False    '登録ボタン
            Case "NGC"
                '---------------------------
                '2014/06/18 武 ここから
                '---------------------------
                Master.Master.ppRigthButton1.Enabled = False    '登録ボタン
                '---------------------------
                '2014/06/18 武 ここまで
                '---------------------------
        End Select
    End Sub
    '---------------------------
    '2014/06/18 武 ここから
    '---------------------------
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                e.Row.Cells(1).Enabled = False
            Case "NGC"
                e.Row.Cells(1).Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/06/18 武 ここまで
    '---------------------------
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        If (Page.IsValid) Then
            msGet_Data(Me.ckbArtcl.Checked,
                       Me.ckbPack.Checked,
                       Me.dftDelivDt.ppFromText,
                       Me.dftDelivDt.ppToText,
                       Me.dftArtcltransD.ppFromText,
                       Me.dftArtcltransD.ppToText,
                       Me.tftArtclNo.ppFromText,
                       Me.tftArtclNo.ppToText,
                       Me.tftTboxId.ppFromText,
                       Me.tftTboxId.ppToText,
                       Me.tftRequestNo.ppFromText,
                       Me.tftRequestNo.ppToText,
                       Me.dftConstDt.ppFromText,
                       Me.dftConstDt.ppToText,
                       Me.ddlSort.SelectedValue)
        End If
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 検索クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        msClearSearch()
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 当日ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDay_Click(sender As Object, e As EventArgs)
        Dim strNow As String = DateTime.Now.ToString("yyyy/MM/dd")
        'ログ出力開始
        psLogStart(Me)
        '全ての入力項目をクリア
        msClearSearch()
        '依頼日 FROMにシステム日付を設定
        dftArtcltransD.ppFromText = strNow
        '現在日付で検索
        msGet_Data(Me.ckbArtcl.Checked,
                   Me.ckbPack.Checked,
                   Me.dftDelivDt.ppFromText,
                   Me.dftDelivDt.ppToText,
                   Me.dftArtcltransD.ppFromText,
                   Me.dftArtcltransD.ppToText,
                   Me.tftArtclNo.ppFromText,
                   Me.tftArtclNo.ppToText,
                   Me.tftTboxId.ppFromText,
                   Me.tftTboxId.ppToText,
                   Me.tftRequestNo.ppFromText,
                   Me.tftRequestNo.ppToText,
                   Me.dftConstDt.ppFromText,
                   Me.dftConstDt.ppToText,
                   Me.ddlSort.SelectedValue)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text     'パンくずリスト
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録                  '画面遷移条件
        Session(P_SESSION_OLDDISP) = M_DISP_ID                      '遷移元の画面ＩＤ
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号
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
                        objStack.GetMethod.Name, M_CNSUPDP002, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        psOpen_Window(Me, M_CNSUPDP002)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 一覧の更新／参照ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing
        Dim strArtclNo As String                '受渡し用指示Ｎｏ(X-XXXX-XX-XXX⇒XXXXXXXXXX)

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        intIndex = Convert.ToInt32(e.CommandArgument)   'ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                'ボタン押下行

        '受渡し用指示Ｎｏ(X-XXXX-XX-XXX⇒XXXXXXXXXX)取得
        'strArtclNo = CType(rowData.FindControl("指示Ｎｏ"), TextBox).Text
        strArtclNo = CType(rowData.FindControl("指示Ｎｏ"), Label).Text
        strArtclNo = strArtclNo.Replace("-", "")

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text 'パンくずリスト
        Session(P_KEY) = {strArtclNo}                           '詳細キー項目(指示Ｎｏ．)
        Session(P_SESSION_OLDDISP) = M_DISP_ID                  '遷移元の画面ＩＤ
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)   'グループ番号

        Select Case e.CommandName
            Case "btnReference"     ' 参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"        ' 更新
                '-----------------------------
                '2014/04/21 土岐　ここから
                '-----------------------------
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                '-----------------------------
                '2014/05/26 土岐　ここから
                '-----------------------------
                arTable_Name.Insert(0, "D19_ARTCLTRNS")
                '-----------------------------
                '2014/05/26 土岐　ここまで
                '-----------------------------

                '★ロックテーブルキー項目の登録(D39_CNSTREQSPEC)
                arKey.Insert(0, strArtclNo)

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , P_FUN_CNS & P_SCR_UPD & P_PAGE & "002" _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                Else

                    '排他ロック中
                    Exit Sub

                End If
                '-----------------------------
                '2014/04/21 土岐　ここまで
                '-----------------------------
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
        End Select

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
                        objStack.GetMethod.Name, M_CNSUPDP002, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '別ブラウザ起動
        psOpen_Window(Me, M_CNSUPDP002)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' データバインド時、削除フラグがある行の更新を非活性にする。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        Dim strDelF As String
        Dim strSendDT As String
        For Each rowData As GridViewRow In grvList.Rows
            'strDelF = CType(rowData.FindControl("削除"), TextBox).Text
            strDelF = CType(rowData.FindControl("削除"), Label).Text
            'strSendDT = CType(rowData.FindControl("送信日時"), TextBox).Text
            strSendDT = CType(rowData.FindControl("送信日時"), Label).Text
            If strDelF = "●" Then   '削除フラグあり  ※送信ボタン削除により更新遷移不可
                rowData.Cells(1).Enabled = False
            End If

        Next
    End Sub
#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <param name="ipblnArtcl">依頼区分（物品転送）</param>
    ''' <param name="ipblnPack">依頼区分（梱包箱出荷）</param>
    ''' <param name="ipstrDeliv_dt_f">納期 From</param>
    ''' <param name="ipstrDeliv_dt_t">納期 To</param>
    ''' <param name="ipstrArtcltrans_d_f">依頼日 From</param>
    ''' <param name="ipstrArtcltrans_d_t">依頼日 To</param>
    ''' <param name="ipstrArtcl_no_f">指示Ｎｏ From</param>
    ''' <param name="ipstrArtcl_no_t">指示Ｎｏ To</param>
    ''' <param name="ipstrTboxid_f">ＴＢＯＸＩＤ From</param>
    ''' <param name="ipstrTboxid_t">ＴＢＯＸＩＤ To</param>
    ''' <param name="ipstrRequest_no_f">依頼番号 From</param>
    ''' <param name="ipstrRequest_no_t">依頼番号 To</param>
    ''' <param name="ipstrConst_d_f">工事日 From</param>
    ''' <param name="ipstrConst_d_t">工事日 To</param>
    ''' <param name="ipstrSort">ソートコード</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipblnArtcl As Boolean,
                           ByVal ipblnPack As Boolean,
                           ByVal ipstrDeliv_dt_f As String,
                           ByVal ipstrDeliv_dt_t As String,
                           ByVal ipstrArtcltrans_d_f As String,
                           ByVal ipstrArtcltrans_d_t As String,
                           ByVal ipstrArtcl_no_f As String,
                           ByVal ipstrArtcl_no_t As String,
                           ByVal ipstrTboxid_f As String,
                           ByVal ipstrTboxid_t As String,
                           ByVal ipstrRequest_no_f As String,
                           ByVal ipstrRequest_no_t As String,
                           ByVal ipstrConst_d_f As String,
                           ByVal ipstrConst_d_t As String,
                           ByVal ipstrSort As String)
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strArtcl As String
        Dim strPack As String
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing

        '値取得
        Select Case ipblnArtcl
            Case True
                strArtcl = "1"
            Case Else
                strArtcl = String.Empty
        End Select

        Select Case ipblnPack
            Case True
                strPack = "1"
            Case Else
                strPack = String.Empty
        End Select

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSLSTP002_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("artcl", SqlDbType.NVarChar, strArtcl))
                    .Add(pfSet_Param("pack", SqlDbType.NVarChar, strPack))
                    .Add(pfSet_Param("deliv_dt_f", SqlDbType.NVarChar, ipstrDeliv_dt_f))
                    .Add(pfSet_Param("deliv_dt_t", SqlDbType.NVarChar, ipstrDeliv_dt_t))
                    .Add(pfSet_Param("artcltrans_d_f", SqlDbType.NVarChar, ipstrArtcltrans_d_f))
                    .Add(pfSet_Param("artcltrans_d_t", SqlDbType.NVarChar, ipstrArtcltrans_d_t))
                    .Add(pfSet_Param("artcl_no_f", SqlDbType.NVarChar, ipstrArtcl_no_f))
                    .Add(pfSet_Param("artcl_no_t", SqlDbType.NVarChar, ipstrArtcl_no_t))
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, ipstrTboxid_f))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, ipstrTboxid_t))
                    .Add(pfSet_Param("request_no_f", SqlDbType.NVarChar, ipstrRequest_no_f))
                    .Add(pfSet_Param("request_no_t", SqlDbType.NVarChar, ipstrRequest_no_t))
                    .Add(pfSet_Param("const_d_f", SqlDbType.NVarChar, ipstrConst_d_f))
                    .Add(pfSet_Param("const_d_t", SqlDbType.NVarChar, ipstrConst_d_t))
                    .Add(pfSet_Param("sort", SqlDbType.NVarChar, ipstrSort))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)
                    '-----------------------------
                    '2014/05/14 土岐　ここから
                    '-----------------------------
                    '件数を設定
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    '件数を設定
                    Master.ppCount = dstOrders.Tables(0).Rows(0)("データ件数").ToString
                End If
                '-----------------------------
                '2014/05/14 土岐　ここまで
                '-----------------------------

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "物品転送依頼一覧")
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
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
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
        'クリア
        msClearSearch()
        Me.grvList.DataSource = New Object() {}
        Master.ppCount = "0"

        '変更を反映
        Me.grvList.DataBind()
    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()
        'クリア
        Me.ckbArtcl.Checked = False
        Me.ckbPack.Checked = False
        Me.dftDelivDt.ppFromText = String.Empty
        Me.dftDelivDt.ppToText = String.Empty
        Me.dftArtcltransD.ppFromText = String.Empty
        Me.dftArtcltransD.ppToText = String.Empty
        Me.tftArtclNo.ppFromText = String.Empty
        Me.tftArtclNo.ppToText = String.Empty
        Me.tftTboxId.ppFromText = String.Empty
        Me.tftTboxId.ppToText = String.Empty
        '-----------------------------
        '2014/05/26 土岐　ここから
        '-----------------------------
        Me.tftRequestNo.ppFromText = String.Empty
        Me.tftRequestNo.ppToText = String.Empty
        '-----------------------------
        '2014/05/26 土岐　ここまで
        '-----------------------------
        Me.dftConstDt.ppFromText = String.Empty
        Me.dftConstDt.ppToText = String.Empty
        Me.ddlSort.SelectedValue = "1"
    End Sub
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
#End Region