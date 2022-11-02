'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　券売入金機自走調査
'*　ＰＧＭＩＤ：　SLFLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.12　：　土岐
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用
#End Region

Public Class SLFLSTP001
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
    Private Const M_DISP_ID = P_FUN_SLF & P_SCR_LST & P_PAGE & "001"
    Private Const M_CLEAR As String = "Clear"
    Private Const M_EXE_DT_F As String = "ExeDtF"
    Private Const M_EXE_DT_T As String = "ExeDtT"
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
        pfSet_GridView(Me.grvList, "SLFLSTP001")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnDispUp_Click

        AddHandler Me.txtTBOXID.ppTextBox.TextChanged, AddressOf TextBox_TextChanged
        txtTBOXID.ppTextBox.AutoPostBack = True

        If Not IsPostBack Then  '初回表示

            '検索条件クリアボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'ボタン設定
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.Text = "画面更新"

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '画面クリア
            msClearScreen()
            Call msClearTBOX()

        End If
    End Sub

    '---------------------------
    '2014/04/16 武 ここから
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
                TBOX_Serch_Title.Visible = True
                txtTBOXID.Visible = True
                lblRBOXNM.Visible = True
                dftTBOXFromTo.Visible = True
                btnTBOXClear.Visible = True
                btnTBOXSerach.Visible = True
                TBOX_Serch.Visible = True
            Case "SPC"
                TBOX_Serch_Title.Visible = True
                txtTBOXID.Visible = True
                lblRBOXNM.Visible = True
                dftTBOXFromTo.Visible = True
                btnTBOXClear.Visible = True
                btnTBOXSerach.Visible = True
                TBOX_Serch.Visible = True
            Case "営業所"
                TBOX_Serch_Title.Visible = False
                txtTBOXID.Visible = False
                lblRBOXNM.Visible = False
                dftTBOXFromTo.Visible = False
                btnTBOXClear.Visible = False
                btnTBOXSerach.Visible = False
                TBOX_Serch.Visible = False
            Case "NGC"
                TBOX_Serch_Title.Visible = False
                txtTBOXID.Visible = False
                lblRBOXNM.Visible = False
                dftTBOXFromTo.Visible = False
                btnTBOXClear.Visible = False
                btnTBOXSerach.Visible = False
                TBOX_Serch.Visible = False
        End Select

    End Sub

    '---------------------------
    '2014/04/16 武 ここまで
    '---------------------------
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
                e.Row.Cells(1).Enabled = False
        End Select

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
        If (Page.IsValid) Then
            'データ取得
            msGetData(Me.dftExeDT.ppFromText,
                      Me.dftExeDT.ppToText)

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
        msClearScreen()
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ＴＢＯＸ検索クリア
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnTBOXClear_Click(sender As Object, e As EventArgs) Handles btnTBOXClear.Click

        'ログ出力開始
        psLogStart(Me)
        Call msClearTBOX()
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

        Const SLFLSTP002 As String = "~/" & P_WAT & "/" & P_FUN_SLF & P_SCR_LST & P_PAGE & "002" & "/" & P_FUN_SLF & P_SCR_LST & P_PAGE & "002.aspx"

        Dim intIndex As Integer = Nothing
        Dim rowData As GridViewRow = Nothing
        '--2014/04/23 中川　ここから
        Dim strKey(1) As String
        '--2014/04/23 中川　ここまで

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then Exit Sub

        'ログ出力開始
        psLogStart(Me)

        intIndex = Convert.ToInt32(e.CommandArgument)   ' ボタン押下行のIndex
        rowData = grvList.Rows(intIndex)                ' ボタン押下行

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        strKey(0) = CType(rowData.FindControl("管理番号"), TextBox).Text
        strKey(1) = CType(rowData.FindControl("取得日時"), TextBox).Text
        Session(P_KEY) = strKey
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text                     'パンくずリスト
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号
        Select Case e.CommandName
            Case "btnReference"     ' 参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
            Case "btnUpdate"        ' 更新

                '排他制御用変数
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                'ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D173_KENBAIKIJISOU")

                'ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)

                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他情報確認処理
                'If clsexc.pfSel_Exclusive(strExclusiveDate,
                '                   Me,
                '                   Session(P_SESSION_IP),
                '                   Session(P_SESSION_PLACE),
                '                   Session(P_SESSION_USERID),
                '                   Session(P_SESSION_SESSTION_ID),
                '                   ViewState(P_SESSION_GROUP_NUM),
                '                   "SLFLSTP002",
                '                   arTable_Name,
                '                   arKey) = 0 Then

                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------

                'セッション情報設定
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                'Else
                ''排他ロック中
                'Exit Sub

                'End If

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
                        objStack.GetMethod.Name, SLFLSTP002, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '別ブラウザ起動
        psOpen_Window(Me, SLFLSTP002)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 画面更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDispUp_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        If ViewState(M_CLEAR) Then
            'データクリア
            msClearScreen()
        Else
            'データ取得
            msGetData(ViewState(M_EXE_DT_F).ToString, ViewState(M_EXE_DT_T).ToString)
        End If

        If ViewState("M_TBX_CLEAR") Then
            'データクリア
            Call msClearTBOX()
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ホール情報取得処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        objStack = New StackFrame

        Try
            If Me.txtTBOXID.ppText = String.Empty Then
                'ホール情報クリア
                ViewState("M_TBX_CLEAR") = True
                Exit Try
            End If

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Exit Sub
            End If

            'パラメータ設定
            cmdDB = New SqlCommand("SLFLSTP001_S2", conDB)

            cmdDB.Parameters.Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, txtTBOXID.ppText))
            '            cmdDB.Parameters.Add(pfSet_Param("DTFROM", SqlDbType.NVarChar, dftTBOXFromTo.ppFromText))
            '            cmdDB.Parameters.Add(pfSet_Param("DTTO", SqlDbType.NVarChar, dftTBOXFromTo.ppToText))

            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
                'エラーメッセージ表示
                psMesBox(Me, "10004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力されたＴＢＯＸＩＤ")
                'ホール情報クリア
                Me.txtTBOXID.ppText = String.Empty
                ViewState("M_TBX_CLEAR") = True
                Exit Sub
            End If

            Me.lblRBOXNM.Text = dstOrders.Tables(0).Rows(0)("T01_HALL_NAME").ToString.Trim
            ViewState("M_TBX_CLEAR") = False

        Catch ex As DBConcurrencyException
            psMesBox(Me, "0005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            Me.dftExeDT.ppFromText = String.Empty
            Me.dftExeDT.ppToText = String.Empty
            ViewState(M_EXE_DT_F) = String.Empty
            ViewState(M_EXE_DT_T) = String.Empty
            ViewState(M_CLEAR) = True

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            Me.grvList.DataSource = New DataTable
            Master.ppCount = "0"
            Me.grvList.DataBind()
            Me.dftExeDT.ppDateBoxFrom.Focus()

            '--------------------------------
            '2014/06/09 後藤　ここから
            '--------------------------------
            Master.Master.ppRigthButton1.Enabled = False
            '--------------------------------
            '2014/06/09 後藤　ここまで
            '--------------------------------
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
        Finally
        End Try

    End Sub

    ''' <summary>
    ''' ＴＢＯＸ検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearTBOX()

        objStack = New StackFrame
        Try

            Me.txtTBOXID.ppText = String.Empty
            Me.lblRBOXNM.Text = String.Empty
            Me.dftTBOXFromTo.ppFromText = String.Empty
            Me.dftTBOXFromTo.ppToText = String.Empty
            ViewState("M_TBOX") = String.Empty
            ViewState("M_TBX_DT_F") = String.Empty
            ViewState("M_TBX_DT_T") = String.Empty
            ViewState("M_TBX_CLEAR") = True

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
        End Try

    End Sub

    ''' <summary>
    ''' ＴＢＯＸ表示ボタン　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnTBOXSerach_Click(sender As Object, e As EventArgs) Handles btnTBOXSerach.Click

        Const SLFLSTP004 As String = "~/" & P_WAT & "/" & P_FUN_SLF & P_SCR_LST & P_PAGE & "004" & "/" & P_FUN_SLF & P_SCR_LST & P_PAGE & "004.aspx"

        Dim strKey(2) As String

        'ログ出力開始
        psLogStart(Me)


        'セッション情報設定
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        strKey(0) = Me.txtTBOXID.ppText
        strKey(1) = Me.dftTBOXFromTo.ppFromText
        strKey(2) = Me.dftTBOXFromTo.ppToText
        Session(P_KEY) = strKey
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text                     'パンくずリスト
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)               'グループ番号
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

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, SLFLSTP004, strPrm, "TRANS")
        '別ブラウザ起動
        psOpen_Window(Me, SLFLSTP004)
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetData(ByVal ipstrExeF As String,
                          ByVal ipstrExeT As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
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
                cmdDB = New SqlCommand("SLFLSTP001_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("exedt_f", SqlDbType.NVarChar, ipstrExeF))
                    .Add(pfSet_Param("exedt_t", SqlDbType.NVarChar, ipstrExeT))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '件数を設定
                Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                If dstOrders.Tables(0).Rows.Count <= 0 Then
                    '--------------------------------
                    '2014/06/09 後藤　ここから
                    '--------------------------------
                    'psMesBox(Me, "00007", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                    Master.Master.ppRigthButton1.Enabled = False
                Else
                    Master.Master.ppRigthButton1.Enabled = True
                End If
                '--------------------------------
                '2014/06/09 後藤　ここまで
                '--------------------------------

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                ViewState(M_EXE_DT_F) = ipstrExeF
                ViewState(M_EXE_DT_T) = ipstrExeT
                ViewState(M_CLEAR) = False

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
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
