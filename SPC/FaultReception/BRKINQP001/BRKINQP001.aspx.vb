'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜故障受付＞
'*　処理名　　：　対応履歴照会
'*　ＰＧＭＩＤ：　BRKINQP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.31　：　中川
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'BRKINQP001-001     2016/01/06      栗原　　　ホール情報取得失敗時も、対応履歴は取得／表示するよう変更。

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

'排他制御用
Imports SPC.ClsCMExclusive

#End Region

Public Class BRKINQP001

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
    Const M_DISP_ID As String = P_FUN_BRK & P_SCR_INQ & P_PAGE & "001"          '自画面ID
    Const M_CNSUPDP003_ID As String = P_FUN_CNS & P_SCR_UPD & P_PAGE & "003"    '工事進捗画面ID
    Const M_CMPSELP001_ID As String = P_FUN_CMP & P_SCR_SEL & P_PAGE & "001"    '保守対応依頼書画面ID
    Const M_BRKUPDP001_ID As String = P_FUN_BRK & P_SCR_UPD & P_PAGE & "001"    'ミニ処理票画面ID
    Const M_REQSELP001_ID As String = P_FUN_REQ & P_SCR_SEL & P_PAGE & "001"    'トラブル処理票画面ID
    Const M_CNSUPDP003_PATH As String =
            "~/" & P_CNS & "/" & M_CNSUPDP003_ID & "/" & M_CNSUPDP003_ID & ".aspx"
    Const M_CMPSELP001_PATH As String =
            "~/" & P_MAI & "/" & M_CMPSELP001_ID & "/" & M_CMPSELP001_ID & ".aspx"
    Const M_BRKUPDP001_PATH As String =
            "~/" & P_FLR & "/" & M_BRKUPDP001_ID & "/" & M_BRKUPDP001_ID & ".aspx"
    Const M_REQSELP001_PATH As String =
            "~/" & P_MAI & "/" & M_REQSELP001_ID & "/" & M_REQSELP001_ID & ".aspx"
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

    ''' <summary>
    ''' Page_Initイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'グリッドXML定義ファイル取得
            pfSet_GridView(Me.grvInqList, M_DISP_ID)

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
        End Try

    End Sub

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dstOrders As DataSet = Nothing

            If Not IsPostBack Then  '初回表示

                Dim strKey() As String = Nothing

                'プログラムＩＤと画面名の設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'セッションから情報を取得
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)               '遷移条件
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then                 'グループ番号
                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                End If
                strKey = Session(P_KEY)                                             'キー項目

                '--------------------------------
                '2014/05/16 後藤　ここから
                '--------------------------------
                Me.lblCountNum.Text = "0" & " 件" '件数を初期設定
                '--------------------------------
                '2014/05/16 後藤　ここまで
                '--------------------------------

                Try
                    '画面情報取得
                    If clsDataConnect.pfOpen_Database(conDB) Then  'DB接続
                        cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, strKey(0))) 'TBOXID
                            .Add(pfSet_Param("nl_kbn", SqlDbType.NVarChar, strKey(1)))  'NL区分
                            .Add(pfSet_Param("sys_cls", SqlDbType.NVarChar, strKey(2))) 'システム分類
                        End With

                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        'BRKINQP001-001
                        Me.lblTboxId.Text = strKey(0)
                        Me.lblNlKbn.Text = strKey(1)
                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            Me.lblHallName.Text = String.Empty
                            Me.lblUnyoKbn.Text = String.Empty

                            '    '該当が0件
                            '    psMesBox(Me, "00004", clscomver.E_Mタイプ.警告, clscomver.E_S実行.描画後)

                            '    'グリッド初期化
                            '    msCrean_Grid()
                            '    Exit Sub
                        Else

                            Me.lblHallName.Text = dstOrders.Tables(0).Rows(0).Item("ホール名")
                            Me.lblUnyoKbn.Text = dstOrders.Tables(0).Rows(0).Item("運用区分")

                        End If
                        'BRKINQP001-001

                    Else
                        'DB接続に失敗
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        'グリッド初期化
                        msCrean_Grid()
                    End If
                    'BRKINQP001-001 CATCHの外へ
                    '履歴一覧取得処理
                    'Call msGet_Data()
                    'BRKINQP001-001
                Catch ex As Exception

                    '検索に失敗
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸＩＤ")
                    'グリッド初期化
                    msCrean_Grid()

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
                'BRKINQP001-001
                '履歴一覧取得処理
                Call msGet_Data()
                'BRKINQP001-001
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
        End Try

    End Sub

    '---------------------------
    '2014/04/22 武 ここから
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
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/22 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 履歴一覧データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvInqList_DataBound(sender As Object, e As GridViewRowEventArgs) Handles grvInqList.RowDataBound

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '遷移条件によってボタンの活性／非活性を変える
            For Each rowData As GridViewRow In grvInqList.Rows

                Select Case ViewState(P_SESSION_TERMS)
                    Case ClsComVer.E_遷移条件.参照
                        rowData.Cells(0).Enabled = True
                        rowData.Cells(1).Enabled = False
                    Case ClsComVer.E_遷移条件.更新
                        rowData.Cells(0).Enabled = True
                        rowData.Cells(1).Enabled = True

                End Select
            Next

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
        End Try

    End Sub

    ''' <summary>
    ''' 履歴一覧のボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvInqList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvInqList.RowCommand

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
                Exit Sub
            End If

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvInqList.Rows(intIndex)          'ボタン押下行
            Dim strWork As String = CType(rowData.FindControl("作業"), TextBox).Text
            Dim strKeyList As New List(Of String)
            Dim strPath As String = Nothing
            Dim strDispId As String = Nothing

            '排他制御用
            Dim strExclusiveDate As String = String.Empty
            Dim arTable_Name As New ArrayList
            Dim arKey As New ArrayList

            '遷移先パスとキー項目の設定
            Select Case strWork
                Case "工事"
                    strPath = M_CNSUPDP003_PATH
                    strDispId = M_CNSUPDP003_ID
                    strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)      '工事依頼番号
                    strKeyList.Add(CType(rowData.FindControl("予備1"), TextBox).Text)
                Case "保守"
                    strPath = M_CMPSELP001_PATH
                    strDispId = M_CMPSELP001_ID
                    strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)      '保守管理番号
                    'strKeyList.Add(CType(rowData.FindControl("予備1"), TextBox).Text)
                Case "ミニ"
                    strPath = M_BRKUPDP001_PATH
                    strDispId = M_BRKUPDP001_ID
                    strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)      'ミニ処理管理番号
                Case "トラブル"
                    strPath = M_REQSELP001_PATH
                    strDispId = M_REQSELP001_ID
                    strKeyList.Add(CType(rowData.FindControl("管理番号"), TextBox).Text)      'トラブル管理番号
            End Select

            Select Case e.CommandName
                Case "btnReference"
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                Case "btnUpdate"

                    'ロック対象の登録
                    Select Case strWork
                        Case "工事"
                            arTable_Name.Insert(0, "D24_CNST_SITU_DTL")
                            arTable_Name.Insert(1, "D39_CNSTREQSPEC")
                            arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)     '工事依頼番号
                            arKey.Insert(1, CType(rowData.FindControl("予備1"), TextBox).Text)        '設置区分
                            arKey.Insert(2, CType(rowData.FindControl("予備2"), TextBox).Text)        '連番
                            arKey.Insert(3, CType(rowData.FindControl("管理番号"), TextBox).Text)     '工事依頼番号

                        Case "保守"
                            arTable_Name.Insert(0, "D75_DEAL_MAINTAIN")
                            arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)     '保守管理番号

                        Case "ミニ"
                            arTable_Name.Insert(0, "D77_MINI_MANAGE")
                            arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)     'ミニ処理管理番号

                        Case "トラブル"
                            arTable_Name.Insert(0, "D73_TROUBLE")
                            arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)     'トラブル管理番号
                    End Select

                    '排他情報確認処理
                    If clsExc.pfSel_Exclusive(strExclusiveDate,
                                       Me,
                                       Session(P_SESSION_IP),
                                       Session(P_SESSION_PLACE),
                                       Session(P_SESSION_USERID),
                                       Session(P_SESSION_SESSTION_ID),
                                       ViewState(P_SESSION_GROUP_NUM),
                                       strDispId,
                                       arTable_Name,
                                       arKey) = 0 Then

                    Else
                        '排他ロック中
                        Exit Sub

                    End If

                    'セッション設定
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                    Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

            End Select

            'セッション設定
            '画面遷移フラグ未定
            Session(P_KEY) = strKeyList.ToArray
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
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
                            objStack.GetMethod.Name, strPath, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            '画面を開く
            psOpen_Window(Me, strPath)

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
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 履歴一覧取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim strCount As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                cmdDB.Parameters.Add(pfSet_Param("tbox_id", SqlDbType.NVarChar, Me.lblTboxId.Text))

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '該当件数を設定
                strCount = dstOrders.Tables(0).Rows.Count
                Me.lblCountNum.Text = strCount & " 件"
                If strCount = 0 Then
                    '該当が0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    'グリッド初期化
                    msCrean_Grid()
                    Exit Sub

                ElseIf dstOrders.Tables(0).Rows(0).Item("総件数") > strCount Then
                    '最大件数を超えた場合
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                             dstOrders.Tables(0).Rows(0).Item("総件数"), strCount)
                End If

                '取得データをグリッドに設定
                Me.grvInqList.DataSource = dstOrders.Tables(0)
                Me.grvInqList.DataBind()

            Else
                'DB接続に失敗
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                'グリッド初期化
                msCrean_Grid()

            End If

        Catch ex As Exception

            '検索に失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "対応履歴")
            'グリッド初期化
            msCrean_Grid()

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

    End Sub

    ''' <summary>
    ''' グリッド初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCrean_Grid()

        Me.grvInqList.DataSource = New DataTable
        Me.grvInqList.DataBind()

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
