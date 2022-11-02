'********************************************************************************************************************************
'*　システム　：　サポートセンタシステム　＜共通＞
'*　処理名　　：　持参物品マスタプレビュー
'*　ＰＧＭＩＤ：　COMUPDM55
'*                                                                                                  CopyRight SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.04.30  ：　栗原
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports Microsoft.VisualBasic
#End Region

Public Class COMUPDM55

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
    Const DispCode As String = "COMUPDM55"                       '画面ID
    Const MasterName As String = "持参物品マスタ登録"            '画面名
    Const USEMST_NM As String = "持参物品マスタ"
    Const TableName As String = "M54_BRING_ARTICLE"              'テーブル名

    'グリッド項目
    Const C_TYPE As Integer = 1
    Const C_DDL As Integer = 2
    Const C_NM As Integer = 3
    Const C_SYS As Integer = 4
    Const C_VER As Integer = 5
    Const C_LINE As Integer = 7
    Const C_DT As Integer = 8
    Const C_USER As Integer = 9
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim clsMst As New ClsMSTCommon
    Dim objStack As StackFrame
    Dim mclsDB As New ClsSQLSvrDB

    'エラーチェック用変数
    Dim strErrGrvNm_Inp As String = String.Empty '未入力エラーのグリッド名保管用
    Dim strErrGrvNm_Num As String = String.Empty '不正入力のグリッド名保管用
    Dim blnIsInput As Boolean = True '物品名か行数が空ならFalse
    Dim blnIsNum As Boolean = True '行数が1以上の数値以外ならFalse

#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim intHeadCol As Integer() = New Integer() {2} 'グリッドXML定義ファイル取得　ヘッダ修正パラメータ
        Dim intColSpan As Integer() = New Integer() {2}
        'グリッドの設定をXMLから読み込む(ドロップダウンリストと入力検証の設定はRowDataCreated)
        pfSet_GridView(Me.grvList1, DispCode, intHeadCol, intColSpan)
        pfSet_GridView(Me.grvList2, DispCode, intHeadCol, intColSpan)
        pfSet_GridView(Me.grvList3, DispCode, intHeadCol, intColSpan)
        '並べ替えは上下ボタンのみで行う為、ソート機能を無効化
        grvList1.AllowSorting = False
        grvList2.AllowSorting = False
        grvList3.AllowSorting = False
    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'スクロール調整
        MaintainScrollPositionOnPostBack = True

        'コントロールの初期化
        obj_Init()

        If Not IsPostBack Then

            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.FindControl("lblCountTitle").Visible = False
            Master.FindControl("lblcount").Visible = False
            Master.FindControl("lblCountFt").Visible = False

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'ドロップダウンリスト設定
            msSetddlSystem()
            msSetddlMachine(ddlMachine)

            'グリッドの初期化
            CleanGrid()

            'セッション情報からデータ取得
            '新規以外
            If Session("prm_SysCod") <> String.Empty AndAlso _
                Session("prm_AppCod") <> String.Empty AndAlso _
                Session("prm_TbxVer") <> String.Empty Then
                hdnKeySys.Value = Session("prm_SysCod")
                hdnKeyMac.Value = Session("prm_AppCod")
                hdnKeyVer.Value = Session("prm_TbxVer")
                lblInsOrUpd.Text = "更新"
                '排他情報セット
                excExclusive("NEW")
            Else  '新規
                lblInsOrUpd.Text = "新規"
            End If

            '画面初期化
            msScreenClear(True)

            'フォーカス設定
            SetFocus(ddlSystem.ClientID)

        End If
    End Sub

    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '活性制御
        msEnableChange(hdnMode.Value)

        'ボタン押下時のスクロール調整
        msSetScrollEvent()

    End Sub

    ''' <summary>
    ''' 登録ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnIns_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        '登録処理※入力検証はmsEditData内で行う
        msEditData()

        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 更新ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpd_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        '更新処理※入力検証はmsEditData内で行う
        msEditData()

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 行数追加削除ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnLine_Click(sender As Object, e As CommandEventArgs)
        Dim grvSelectGrid As GridView
        Dim lblcount As Label
        Dim strTitle As String
        Dim dtt As New DataTable  '編集用データテーブル

        'グリッドと表示件数の設定
        hdnCmd.Value = e.CommandName
        Select Case hdnMode.Value
            Case "HARD"
                grvSelectGrid = grvList1
                lblcount = lblcount1
                strTitle = "ハード付属品関連"
            Case "TOOL"
                grvSelectGrid = grvList2
                lblcount = lblcount2
                strTitle = "保守ツール関連"
            Case "MANUAL"
                grvSelectGrid = grvList3
                lblcount = lblcount3
                strTitle = "手順書関連"
            Case Else
                Exit Sub  'エディタの警告回避用
        End Select

        If e.CommandName = "Add" Then
            If mfGridValidate(grvSelectGrid) AndAlso mfParse_DataTable(dtt, grvSelectGrid) Then

                '空白行を一番下に追加し、選択行を追加行に設定する
                Dim dtr As DataRow = dtt.NewRow
                dtt.Rows.Add(dtr)
                '表示件数更新
                lblcount.Text += 1
                hdnSelectLine.Value = lblcount.Text - 1

            Else
                'psMesBox(Me, "30029", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, "帳票レイアウトが作成できない為", "行追加", "物品名と行数を入力してください。\n" & strTitle)
                'hdnCmd.Value = "DEF"
                msErrMes()
                Exit Sub
            End If

        ElseIf e.CommandName = "Delete" Then

            If grvSelectGrid.Rows.Count = 0 OrElse hdnSelectLine.Value = -1 Then
                '削除行が選択されていない無い(グリッドに1行も無い)時
                psMesBox(Me, "10005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "行削除", "削除対象行の選択")
                Exit Sub
            End If

            mfParse_DataTable(dtt, grvSelectGrid)
            dtt.Rows(hdnSelectLine.Value).Delete()  '選択行を削除
            lblcount.Text -= 1
            hdnSelectLine.Value = 0     '行削除後は選択行を先頭行に戻す

        End If

        grvSelectGrid.DataSource = dtt
        grvSelectGrid.DataBind()

        'グリッド内のドロップダウンリストはデータバインドではバインドされないので、手動で行う
        If grvSelectGrid.Equals(grvList1) Then
            msGridDDLBind(grvSelectGrid, dtt, e.CommandName)
        End If

    End Sub

    ''' <summary>
    '''システム項目変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSystem_SelectedIndexChanged()
        'ログ出力開始
        psLogStart(Me)

        If ddlSystem.SelectedValue <> "" Then
            msSetcklVer()
            msGetVer()
        Else 'システム未選択時はバージョン/許容バージョンを初期化
            ddlVer.SelectedIndex = -1
            ddlVer.Enabled = False
            cklver.DataSource = New DataTable
            cklver.DataBind()
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    '''機器項目変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlMachine_SelectedIndexChanged()

        'ログ出力開始
        psLogStart(Me)

        'KEY項目が全て決定したらデータ検索
        If ddlSystem.SelectedValue <> "" AndAlso ddlMachine.SelectedValue <> "" AndAlso ddlVer.SelectedValue <> "" Then
            msGet_ExistData("Machine")
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    '''バージョン変更
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlVer_SelectedIndexChanged()

        'ログ出力開始
        psLogStart(Me)
        If ddlSystem.SelectedValue <> "" AndAlso ddlMachine.SelectedValue <> "" AndAlso ddlVer.SelectedValue <> "" Then
            msGet_ExistData("Ver")

            'バージョンを未選択に戻した時
        ElseIf ddlSystem.SelectedValue <> "" AndAlso ddlMachine.SelectedValue <> "" Then
            hdnMode.Value = "DEF"
            '許容バージョン初期化
            cklver.ClearSelection()
            'グリッド初期化
            CleanGrid()
        End If
        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 新規更新切替
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlInsUpd_SelectedIndexChanged()

        'グリッド初期化
        CleanGrid()
        '許容バージョン初期化
        cklver.ClearSelection()
        'バージョン空欄
        ddlVer.SelectedIndex = -1

    End Sub

    ''' <summary>
    ''' クリアボタン押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()

        '画面を遷移時に初期化
        msScreenClear(True)

    End Sub

    ''' <summary>
    ''' ３つの選択ボタン制御
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGridTop(sender As Object, e As CommandEventArgs) Handles btnGrid1.Command, btnGrid2.Command, btnGrid3.Command
        Dim grv As GridView
        Dim lblMark As Label
        Dim lblGridName As Label
        Select Case hdnMode.Value '押下前の選択グリッド(入力検証用)
            Case "HARD"
                grv = grvList1
            Case "TOOL"
                grv = grvList2
            Case "MANUAL"
                grv = grvList3
            Case Else
                grv = Nothing
        End Select

        Select Case e.CommandName '押下先の情報(文字色変更用)
            Case "HARD"
                lblMark = lblMark1
                lblGridName = lblG1
            Case "TOOL"
                lblMark = lblMark2
                lblGridName = lblG2
            Case "MANUAL"
                lblMark = lblMark3
                lblGridName = lblG3
            Case Else
                lblMark = Nothing
                lblGridName = Nothing
        End Select

        '検証しない
        hdnMode.Value = e.CommandName
        hdnSelectLine.Value = 0
        setbtnColor(sender, lblMark, lblGridName)

    End Sub
    Private Sub setbtnColor(ByRef btn As Button, ByRef mrk As Label, ByRef lblGridName As Label)

        btnGrid1.BackColor = Drawing.Color.Empty
        btnGrid2.BackColor = Drawing.Color.Empty
        btnGrid3.BackColor = Drawing.Color.Empty
        lblG1.ForeColor = Drawing.Color.Empty
        lblG2.ForeColor = Drawing.Color.Empty
        lblG3.ForeColor = Drawing.Color.Empty
        lblMark1.Visible = False
        lblMark2.Visible = False
        lblMark3.Visible = False

        If Not IsNothing(btn) Then
            btn.BackColor = System.Drawing.Color.RoyalBlue
            lblGridName.ForeColor = System.Drawing.Color.RoyalBlue
            mrk.Visible = True
        End If

    End Sub

    ''' <summary>
    ''' 行の上下移動
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LineUpDw(sender As Object, e As CommandEventArgs) Handles btnGrv1Up.Command, btnGrv2Up.Command, btnGrv3Up.Command, _
                                                                          btnGrv1Dw.Command, btnGrv2Dw.Command, btnGrv3Dw.Command
        Dim grvList As GridView
        Dim dtt As New DataTable    '編集用データテーブル(一時置き場) 
        Dim intUpDw As Integer      '挿入位置と選択行制御用・UPなら-1、Downなら+1

        hdnCmd.Value = "DEF"

        '選択グリッドの設定
        Select Case sender.ClientID
            Case btnGrv1Up.ClientID, btnGrv1Dw.ClientID
                grvList = grvList1
            Case btnGrv2Up.ClientID, btnGrv2Dw.ClientID
                grvList = grvList2
            Case btnGrv3Up.ClientID, btnGrv3Dw.ClientID
                grvList = grvList3
            Case Else
                'エラー
                Exit Sub
        End Select

        Select Case e.CommandName
            Case "Up"
                intUpDw = -1
            Case "Dw"
                intUpDw = 1
        End Select

        mfParse_DataTable(dtt, grvList)

        Dim dtr As DataRow = dtt.Rows(hdnSelectLine.Value)
        Dim tmp As DataRow = dtt.NewRow

        'コピー行を作成
        tmp.ItemArray = dtr.ItemArray
        dtr.Delete()

        'コピー行を挿入
        dtt.Rows.InsertAt(tmp, hdnSelectLine.Value + intUpDw)
        hdnSelectLine.Value += intUpDw

        grvList.DataSource = dtt
        grvList.DataBind()

        'グリッド内のドロップダウンリストはデータバインドではバインドされないので手動で行う
        If grvList.Equals(grvList1) Then
            msGridDDLBind(grvList, dtt, "LineUpDw")
        End If

    End Sub

#End Region

    '============================================================================================================================
    '==   GRID関係
    '============================================================================================================================
    ''' <summary>
    ''' Grid_Rowコマンド
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList1.RowCommand, grvList2.RowCommand, grvList3.RowCommand

        'ソートイベントも拾ってくるため、コマンドネーム確認後に処理を開始する
        If e.CommandName = "Select" Then
            Dim rowData As GridViewRow = sender.Rows(Convert.ToInt32(e.CommandArgument))
            '選択行を有効化
            rowData.Enabled = True
            hdnSelectLine.Value = Convert.ToInt32(e.CommandArgument)
            sender.BackColor = Drawing.Color.RoyalBlue
        End If

    End Sub
    ''' <summary>
    ''' Grid_RowCreated
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList1_RowCreated(sender As Object, e As EventArgs) Handles grvList1.RowCreated

        'ヘッダテキスト設定
        Dim strHeader As String() = New String() {"選択", "種類", "物品名", "物品名", "システム", "バージョン", "シリアル", "行数"}

        Try
            If Not IsPostBack Then
                For clm As Integer = 1 To 7
                    grvList1.Columns(clm).HeaderText = strHeader(clm)
                Next
            End If

            For Each rowData As GridViewRow In grvList1.Rows
                Dim ddlHard As New DropDownList '埋め込み用ドロップダウンリスト

                If rowData.RowIndex = 0 Then
                    rowData.Cells(C_TYPE).Text = "ハード付属品関連"
                Else
                    rowData.Cells(C_TYPE).Text = ""
                End If

                'ドロップダウンリストの設定とグリッドへの追加
                msSetddlGridMachine(ddlHard)
                ddlHard.Width = 120
                rowData.Cells(C_DDL).Controls.Clear()
                rowData.Cells(C_DDL).Controls.Add(ddlHard)

                'グリッド項目の入力制限設定
                CType(rowData.Cells(C_NM).Controls(0), TextBox).MaxLength = 70 '物品名
                CType(rowData.Cells(C_NM).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_SYS).Controls(0), TextBox).MaxLength = 15 'システム
                CType(rowData.Cells(C_SYS).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_VER).Controls(0), TextBox).MaxLength = 10 'バージョン
                CType(rowData.Cells(C_VER).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_LINE).Controls(0), TextBox).MaxLength = 1 '行数
                CType(rowData.Cells(C_LINE).Controls(0), TextBox).CssClass = P_CSS_HAN_CNG

            Next

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub
    Private Sub grvList2_RowCreated(sender As Object, e As EventArgs) Handles grvList2.RowCreated

        Try
            For Each rowData As GridViewRow In grvList2.Rows

                If rowData.RowIndex = 0 Then
                    rowData.Cells(C_TYPE).Text = "保守ツール関連"
                Else
                    rowData.Cells(C_TYPE).Text = ""
                End If

                'グリッド項目の入力制限設定
                rowData.Cells(C_DDL).Text = "" 'ドロップダウンリストは設定しない
                CType(rowData.Cells(C_NM).Controls(0), TextBox).MaxLength = 100 '物品名
                CType(rowData.Cells(C_NM).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_SYS).Controls(0), TextBox).MaxLength = 15 'システム
                CType(rowData.Cells(C_SYS).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_VER).Controls(0), TextBox).MaxLength = 10 'バージョン
                CType(rowData.Cells(C_VER).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_LINE).Controls(0), TextBox).MaxLength = 1 '行数
                CType(rowData.Cells(C_LINE).Controls(0), TextBox).CssClass = P_CSS_HAN_CNG

            Next

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub
    Private Sub grvList3_RowCreated(sender As Object, e As EventArgs) Handles grvList3.RowCreated
        Try

            For Each rowData As GridViewRow In grvList3.Rows
                If rowData.RowIndex = 0 Then
                    rowData.Cells(C_TYPE).Text = "手順書関連"
                Else
                    rowData.Cells(C_TYPE).Text = ""
                End If

                'グリッド項目の入力制限設定
                rowData.Cells(C_DDL).Text = "" 'ドロップダウンリストは設定しない
                CType(rowData.Cells(C_NM).Controls(0), TextBox).MaxLength = 100 '物品名
                CType(rowData.Cells(C_NM).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_SYS).Controls(0), TextBox).MaxLength = 15 'システム
                CType(rowData.Cells(C_SYS).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_VER).Controls(0), TextBox).MaxLength = 10 'バージョン
                CType(rowData.Cells(C_VER).Controls(0), TextBox).CssClass = P_CSS_ZEN
                CType(rowData.Cells(C_LINE).Controls(0), TextBox).MaxLength = 1 '行数
                CType(rowData.Cells(C_LINE).Controls(0), TextBox).CssClass = P_CSS_HAN_CNG
            Next

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try


    End Sub
    ''' <summary>
    ''' グリッド内活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub msGridEnable(ByVal strMode As String)
        Dim grvSelectGrid As GridView
        Dim grvNoSel1 As GridView
        Dim grvNoSel2 As GridView

        '選択グリッドの設定
        Select Case strMode
            Case "HARD"
                grvSelectGrid = grvList1
                grvNoSel1 = grvList2
                grvNoSel2 = grvList3
            Case "TOOL"
                grvSelectGrid = grvList2
                grvNoSel1 = grvList1
                grvNoSel2 = grvList3
            Case "MANUAL"
                grvSelectGrid = grvList3
                grvNoSel1 = grvList1
                grvNoSel2 = grvList2
            Case Else
                Exit Sub 'エラー
        End Select

        '選択グリッドを有効化
        grvSelectGrid.Enabled = True

        For Each rowData As GridViewRow In grvSelectGrid.Rows
            '行未選択時は全行で選択ボタンのみ有効化

            If hdnSelectLine.Value = -1 Then
                For i As Integer = 0 To rowData.Cells.Count - 1
                    If i = 0 Then
                        rowData.Cells(i).Enabled = True
                    Else
                        rowData.Cells(i).Enabled = False
                    End If
                Next
            Else            '選択ボタンのみ全行有効化、選択行は全項目有効化
                For i As Integer = 0 To rowData.Cells.Count - 1
                    '選択行
                    If rowData.RowIndex = hdnSelectLine.Value Then
                        rowData.Cells(i).Enabled = True
                        DirectCast(rowData.Cells(0).Controls(0), Button).BackColor = Drawing.Color.RoyalBlue

                    Else '選択行以外
                        If i = 0 Then
                            rowData.Cells(i).Enabled = True
                            DirectCast(rowData.Cells(0).Controls(0), Button).BackColor = Drawing.Color.Empty
                        Else
                            rowData.Cells(i).Enabled = False
                        End If
                    End If
                Next
            End If
        Next

        '選択されていないグリッドのボタンカラーを戻す
        For Each rowData As GridViewRow In grvNoSel1.Rows
            DirectCast(rowData.Cells(0).Controls(0), Button).BackColor = Drawing.Color.Empty
        Next

        For Each rowData As GridViewRow In grvNoSel2.Rows
            DirectCast(rowData.Cells(0).Controls(0), Button).BackColor = Drawing.Color.Empty
        Next


    End Sub

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msScreenClear(Optional ByVal blnInit As Boolean = False)

        'Trueなら画面遷移時に初期化
        '画面遷移時更新
        If blnInit = True AndAlso lblInsOrUpd.Text = "更新" Then
            ddlInsUpd.SelectedValue = 1
            hdnMode.Value = "HARD"
            ddlSystem.SelectedValue = hdnKeySys.Value
            ddlMachine.SelectedValue = hdnKeyMac.Value
            msGetVer()
            ddlVer.SelectedValue = hdnKeyVer.Value
            msSetcklVer()
            msGet_Data()

        Else '初期化
            If blnInit = True Then
                ddlInsUpd.SelectedValue = 0
            End If
            hdnMode.Value = "DEF"
            ddlMachine.Enabled = True
            ddlSystem.Enabled = True
            ddlVer.Enabled = False
            ddlMachine.SelectedIndex = -1
            ddlSystem.SelectedIndex = -1
            ddlVer.SelectedIndex = -1
            cklver.DataSource = New DataTable
            cklver.DataBind()
            CleanGrid()
        End If

    End Sub

    ''' <summary>
    ''' 活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub msEnableChange(ByVal strMode As String)
        'ボタン設定
        Dim btnFtPrev As Button = DirectCast(Master.FindControl("btnLeft1"), Button)
        Dim btnFtClear As Button = Master.ppBtnSrcClear
        Dim btnFtLineAdd As Button = DirectCast(Master.FindControl("btnRigth5"), Button)
        Dim btnFtLineDel As Button = DirectCast(Master.FindControl("btnRigth4"), Button)
        Dim btnFtIns As Button = DirectCast(Master.FindControl("btnRigth3"), Button)
        Dim btnFtUpdate As Button = DirectCast(Master.FindControl("btnRigth2"), Button)
        Dim btnFtDelete As Button = DirectCast(Master.FindControl("btnRigth1"), Button)

        Select Case strMode
            Case "DEF"
                setbtnColor(Nothing, Nothing, Nothing)
                ddlMachine.Enabled = True
                ddlSystem.Enabled = True
                btnFtLineAdd.Enabled = False
                btnFtLineDel.Enabled = False
                btnFtClear.Enabled = True
                grvList1.Enabled = False
                grvList2.Enabled = False
                grvList3.Enabled = False
                btnGrid1.Enabled = False
                btnGrid2.Enabled = False
                btnGrid3.Enabled = False
                cklver.Enabled = False

            Case "HARD"
                setbtnColor(btnGrid1, lblMark1, lblG1)
                ddlMachine.Enabled = False
                ddlSystem.Enabled = False
                btnFtLineAdd.Enabled = True
                btnFtLineDel.Enabled = True
                btnFtClear.Enabled = True
                grvList2.Enabled = False
                grvList3.Enabled = False
                msGridEnable(strMode)
                btnGrid1.Enabled = True
                btnGrid2.Enabled = True
                btnGrid3.Enabled = True
                cklver.Enabled = True

            Case "TOOL"
                setbtnColor(btnGrid2, lblMark2, lblG2)
                ddlMachine.Enabled = False
                ddlSystem.Enabled = False
                btnFtLineAdd.Enabled = True
                btnFtLineDel.Enabled = True
                btnFtClear.Enabled = True
                grvList1.Enabled = False
                grvList3.Enabled = False
                msGridEnable(strMode)
                btnGrid1.Enabled = True
                btnGrid2.Enabled = True
                btnGrid3.Enabled = True
                cklver.Enabled = True

            Case "MANUAL"
                setbtnColor(btnGrid3, lblMark3, lblG3)
                ddlMachine.Enabled = False
                ddlSystem.Enabled = False
                btnFtLineAdd.Enabled = True
                btnFtLineDel.Enabled = True
                btnFtClear.Enabled = True
                grvList1.Enabled = False
                grvList2.Enabled = False
                msGridEnable(strMode)
                btnGrid1.Enabled = True
                btnGrid2.Enabled = True
                btnGrid3.Enabled = True
                cklver.Enabled = True

        End Select

        '行上下ボタンの活性制御
        btnUpDownEnable(strMode)

        'プレビューボタンの活性制御
        msPrvEnable()

        'モードに則した登録更新ボタン制御
        If ddlSystem.Enabled = False AndAlso ddlMachine.Enabled = False Then

            '新規
            Select Case ddlInsUpd.SelectedValue
                Case 0
                    btnFtIns.Enabled = True
                    btnFtUpdate.Enabled = False
                    btnFtDelete.Enabled = False
                Case 1
                    btnFtIns.Enabled = False
                    btnFtUpdate.Enabled = True
                    btnFtDelete.Enabled = False
            End Select

        Else

            btnFtIns.Enabled = False
            btnFtUpdate.Enabled = False
            btnFtDelete.Enabled = False

        End If

    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        objStack = New StackFrame

        If Me.IsPostBack Then 'ポストバックかどうか判定
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                    .Add(pfSet_Param("machine", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                    .Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                If dstOrders.Tables(0).Rows.Count + dstOrders.Tables(1).Rows.Count + dstOrders.Tables(2).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    lblcount1.Text = "0"
                    lblcount2.Text = "0"
                    lblcount3.Text = "0"

                ElseIf dstOrders.Tables(0).Rows.Count = 0 OrElse dstOrders.Tables(1).Rows.Count = 0 OrElse dstOrders.Tables(2).Rows.Count = 0 Then
                    Master.ppCount = dstOrders.Tables(0).Rows.Count + dstOrders.Tables(1).Rows.Count + dstOrders.Tables(2).Rows.Count
                    lblcount1.Text = dstOrders.Tables(0).Rows.Count
                    lblcount2.Text = dstOrders.Tables(1).Rows.Count
                    lblcount3.Text = dstOrders.Tables(2).Rows.Count

                    '総件数とデータセット内の件数(閾値制限)の比較
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) + CType(dstOrders.Tables(1).Rows(0).Item("総件数").ToString, Integer) + CType(dstOrders.Tables(2).Rows(0).Item("総件数").ToString, Integer) _
                    > dstOrders.Tables(0).Rows.Count + dstOrders.Tables(1).Rows.Count + dstOrders.Tables(2).Rows.Count Then
                    '閾値オーバー
                    Master.ppCount = CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) + CType(dstOrders.Tables(1).Rows(0).Item("総件数").ToString, Integer) + CType(dstOrders.Tables(2).Rows(0).Item("総件数").ToString, Integer)
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        Master.ppCount, dstOrders.Tables(0).Rows.Count + dstOrders.Tables(1).Rows.Count + dstOrders.Tables(2).Rows.Count)

                Else
                    Master.ppCount = CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) + CType(dstOrders.Tables(1).Rows(0).Item("総件数").ToString, Integer) + CType(dstOrders.Tables(2).Rows(0).Item("総件数").ToString, Integer)
                    lblcount1.Text = dstOrders.Tables(0).Rows.Count
                    lblcount2.Text = dstOrders.Tables(1).Rows.Count
                    lblcount3.Text = dstOrders.Tables(2).Rows.Count
                End If

                If Master.ppCount <> 0 AndAlso ddlInsUpd.SelectedValue = 0 Then          '既にデータがある為新規登録出来ない
                    psMesBox(Me, "30026", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "既存データ", "更新モードで編集")
                    CleanGrid()
                    'msScreenClear()
                    Exit Sub
                ElseIf Master.ppCount = 0 AndAlso ddlInsUpd.SelectedValue = 1 Then       'データが無い為、更新出来ない
                    psMesBox(Me, "30027", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "データ", "新規登録を")
                    CleanGrid()
                    'msScreenClear()
                    Exit Sub
                End If

                '更新モードならグリッド更新
                If ddlInsUpd.SelectedValue = 1 Then
                    Me.grvList1.DataSource = dstOrders.Tables(0)
                    Me.grvList1.DataBind()
                    msGridDDLBind(grvList1, dstOrders.Tables(0), "Search")

                    Me.grvList2.DataSource = dstOrders.Tables(1)
                    Me.grvList2.DataBind()

                    Me.grvList3.DataSource = dstOrders.Tables(2)
                    Me.grvList3.DataBind()

                    '排他処理
                    excExclusive("SET", ddlSystem.SelectedValue, ddlVer.SelectedValue, ddlMachine.SelectedValue)
                    hdnMode.Value = "HARD"
                    hdnSelectLine.Value = 0

                    '新規モード
                Else
                    '新規検索時
                    If hdnMode.Value = "DEF" Then
                        hdnMode.Value = "HARD"
                        hdnSelectLine.Value = 0
                    End If
                End If


            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, USEMST_NM)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                CleanGrid()
                Master.ppCount = "0"

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try

        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'グリッドの初期化
            CleanGrid()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <remarks></remarks>
    Private Sub msGet_ExistData(ByVal strKey As String)

        'データ取得
        msGet_Data()

        '更新時は、チェックボックス再取得
        If ddlInsUpd.SelectedValue = 1 Then
            msSetcklVer()
        End If

    End Sub

    ''' <summary>
    ''' 登録/更新ボタン押下時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditData()
        Dim objDt1 As New DataTable
        Dim objDt2 As New DataTable
        Dim objDt3 As New DataTable
        Dim objTbl As New DataTable
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim MesCode As String = String.Empty
        Dim strMes As String = String.Empty
        Dim dttGrid As New DataTable
        Dim strStored As String = String.Empty
        Dim trans As SqlClient.SqlTransaction             'トランザクション
        objStack = New StackFrame
        Dim mver(cklver.Items.Count - 1) As String
        Dim dttNow As Date = DateTime.Now

        Select Case ddlInsUpd.SelectedValue
            Case 0  '新規
                MesCode = "00003"
                strMes = "登録"
                hdnCmd.Value = "Ins"
            Case 1  '更新
                MesCode = "00001"
                strMes = "更新"
                hdnCmd.Value = "Upd"
        End Select

        'データ取得
        If grvList1.Rows.Count + grvList2.Rows.Count + grvList3.Rows.Count = 0 Then
            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画前, "０行での" & strMes & "は出来ません。\n" & strMes & "するには明細行の追加")
            Exit Sub
        End If

        'グリッドの内容をデータテーブルに変換
        '行数欄の入力チェック
        If mfGridValidate(grvList1) = False Or _
           mfGridValidate(grvList2) = False Or _
           mfGridValidate(grvList3) = False Then
            msErrMes()
            Exit Sub
        End If
        If mfParse_DataTable(objDt1, grvList1) = False OrElse _
           mfParse_DataTable(objDt2, grvList2) = False OrElse _
           mfParse_DataTable(objDt3, grvList3) = False Then
            Exit Sub
        End If

        '許容バージョンを配列に挿入
        Dim mver_cnt As Integer = 0
        For i As Integer = 0 To cklver.Items.Count - 1
            If cklver.Items(i).Selected Then
                mver(mver_cnt) = cklver.Items(i).Text
                mver_cnt += 1
            End If
        Next
        If mver_cnt = 0 Then
            psMesBox(Me, "10009", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, "許容バージョン", "は１つ以上の選択を")
            Exit Sub
        End If

        Try
            If clsDataConnect.pfOpen_Database(conDB) Then
                'トランザクションの設定
                trans = conDB.BeginTransaction
                strStored = DispCode & "_U1"
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                If ddlInsUpd.SelectedValue = 1 Then
                    '更新なら登録済みの更新対象を削除
                    For cnt As Integer = 0 To (mver_cnt - 1)
                        With cmdDB.Parameters
                            .Clear()
                            'パラメータ設定
                            .Add(pfSet_Param("table_cd", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                            .Add(pfSet_Param("machine_cd", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                            .Add(pfSet_Param("mver", SqlDbType.NVarChar, mver(cnt).Trim))
                            .Add(pfSet_Param("appa_knd_cd", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                            .Add(pfSet_Param("notetext", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("tboxcls_nm", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("dispver", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("linecnt", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("insdt", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("insuser", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("seq", SqlDbType.NVarChar, ""))
                            .Add(pfSet_Param("now", SqlDbType.DateTime, dttNow))
                        End With
                        cmdDB.ExecuteNonQuery()
                    Next
                End If

                '登録開始
                'ハード付属品関連
                For Each dtr As DataRow In objDt1.Rows
                    For cnt As Integer = 0 To (mver_cnt - 1)
                        With cmdDB.Parameters
                            .Clear()
                            'パラメータ設定
                            .Add(pfSet_Param("table_cd", SqlDbType.NVarChar, "1"))
                            .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                            .Add(pfSet_Param("machine_cd", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                            .Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                            .Add(pfSet_Param("mver", SqlDbType.NVarChar, mver(cnt).Trim))
                            .Add(pfSet_Param("appa_knd_cd", SqlDbType.NVarChar, dtr.Item("物品名DDL").ToString))
                            .Add(pfSet_Param("notetext", SqlDbType.NVarChar, dtr.Item("物品名").ToString.Trim))
                            .Add(pfSet_Param("tboxcls_nm", SqlDbType.NVarChar, dtr.Item("システム").ToString.Trim))
                            .Add(pfSet_Param("dispver", SqlDbType.NVarChar, dtr.Item("バージョン").ToString.Trim))
                            .Add(pfSet_Param("linecnt", SqlDbType.NVarChar, dtr.Item("行数").ToString.Trim))
                            .Add(pfSet_Param("insdt", SqlDbType.NVarChar, dtr.Item("登録日時").ToString.Trim))
                            .Add(pfSet_Param("insuser", SqlDbType.NVarChar, dtr.Item("登録者").ToString.Trim))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("seq", SqlDbType.NVarChar, objDt1.Rows.IndexOf(dtr) + 1))
                            .Add(pfSet_Param("now", SqlDbType.DateTime, dttNow))
                        End With
                        cmdDB.ExecuteNonQuery()
                    Next
                Next

                '保守ツール関連
                For Each dtr As DataRow In objDt2.Rows
                    With cmdDB.Parameters
                        .Clear()
                        'パラメータ設定
                        .Add(pfSet_Param("table_cd", SqlDbType.NVarChar, "2"))
                        .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                        .Add(pfSet_Param("machine_cd", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                        .Add(pfSet_Param("mver", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("appa_knd_cd", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, dtr.Item("物品名").ToString.Trim))
                        .Add(pfSet_Param("tboxcls_nm", SqlDbType.NVarChar, dtr.Item("システム").ToString.Trim))
                        .Add(pfSet_Param("dispver", SqlDbType.NVarChar, dtr.Item("バージョン").ToString.Trim))
                        .Add(pfSet_Param("linecnt", SqlDbType.NVarChar, dtr.Item("行数").ToString.Trim))
                        .Add(pfSet_Param("insdt", SqlDbType.NVarChar, dtr.Item("登録日時").ToString.Trim))
                        .Add(pfSet_Param("insuser", SqlDbType.NVarChar, dtr.Item("登録者").ToString.Trim))
                        .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("seq", SqlDbType.NVarChar, objDt2.Rows.IndexOf(dtr) + 1))
                        .Add(pfSet_Param("now", SqlDbType.DateTime, dttNow))
                    End With
                    cmdDB.ExecuteNonQuery()

                Next

                '手順書関連
                For Each dtr As DataRow In objDt3.Rows
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Clear()
                        .Add(pfSet_Param("table_cd", SqlDbType.NVarChar, "3"))
                        .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                        .Add(pfSet_Param("machine_cd", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                        .Add(pfSet_Param("mver", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("appa_knd_cd", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, dtr.Item("物品名").ToString.Trim))
                        .Add(pfSet_Param("tboxcls_nm", SqlDbType.NVarChar, dtr.Item("システム").ToString.Trim))
                        .Add(pfSet_Param("dispver", SqlDbType.NVarChar, dtr.Item("バージョン").ToString.Trim))
                        .Add(pfSet_Param("linecnt", SqlDbType.NVarChar, dtr.Item("行数").ToString.Trim))
                        .Add(pfSet_Param("insdt", SqlDbType.NVarChar, dtr.Item("登録日時").ToString.Trim))
                        .Add(pfSet_Param("insuser", SqlDbType.NVarChar, dtr.Item("登録者").ToString.Trim))
                        .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("seq", SqlDbType.NVarChar, objDt3.Rows.IndexOf(dtr) + 1))
                        .Add(pfSet_Param("now", SqlDbType.DateTime, dttNow))
                    End With
                    cmdDB.ExecuteNonQuery()
                Next

                trans.Commit()
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, USEMST_NM)
            Else
                'ロールバック
                mclsDB.psDB_Rollback()
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, USEMST_NM)
            End If

        Catch ex As Exception
            'ロールバック
            mclsDB.psDB_Rollback()
            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, USEMST_NM)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            mclsDB.psDB_Close()
        End Try
        Select Case ddlInsUpd.SelectedValue
            Case 0  '新規
                'psMesBox(Me, "30013", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "登録したデータを編集する場合は、更新モードに変更して\nシステム、持参分類、バージョン")
                '更新モードに変更する
                ddlInsUpd.SelectedValue = 1

            Case 1  '更新

        End Select
        'データ再取得(登録日時を保持する為)
        msGet_ExistData("Retry")
        '選択行の設定
        msSelectLine_Reset()
        'スクロール位置を一番上に設定
        msScrollReset()

    End Sub

    ''' <summary>
    ''' プレビューボタン押下時
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Protected Sub btnPrev_Click()
        Dim objdt As New DataTable
        Dim objDt1 As New DataTable
        Dim objDt2 As New DataTable
        Dim objDt3 As New DataTable
        Dim objTbl As New DataTable     'プリント用テーブル
        Dim rpt As Object = Nothing     'ActiveReports用
        Dim strTitle As String

        '開始ログ出力
        psLogStart(Me)
        objStack = New StackFrame
        hdnCmd.Value = "Prev"

        Try
            '行数に不正入力が無いかをチェック
            'グリッドのデータをデータテーブルに変換する
            If mfGridValidate(grvList1) = False Or _
               mfGridValidate(grvList2) = False Or _
               mfGridValidate(grvList3) = False Then
                msErrMes()
                Exit Sub
            End If
            If mfParse_DataTable(objDt1, grvList1) = False _
                OrElse mfParse_DataTable(objDt2, grvList2) = False _
                OrElse mfParse_DataTable(objDt3, grvList3) = False Then
                Exit Sub
            End If

            'プリント用テーブルのカラム設定
            objTbl.Columns.Add("タイトル名称")
            objTbl.Columns.Add("種類")
            objTbl.Columns.Add("システム")
            objTbl.Columns.Add("バージョン")
            objTbl.Columns.Add("物品名")
            objTbl.Columns.Add("行数", Type.GetType("System.Decimal"))
            objTbl.Columns.Add("シリアル")
            objTbl.Columns.Add("連番")
            objTbl.Columns.Add("システムコード")

            'プリント用のテーブルにデータテーブルを挿入
            '行数が2以上なら行数文のコピーレコード(≠空レコード)を追加
            Dim intRowCount As Integer = 0
            For tblcnt As Integer = 0 To 2
                Select Case tblcnt
                    Case 0
                        objdt = objDt1
                        strTitle = "ハード付属品関連"
                    Case 1
                        objdt = objDt2
                        strTitle = "保守ツール関連"
                    Case 2
                        objdt = objDt3
                        strTitle = "手順書関連"
                    Case Else
                        'エディタの注意回避用
                        strTitle = ""
                End Select

                For Each dtrRow As DataRow In objdt.Rows
                    If dtrRow.Item("行数") <> 1 Then
                        For i As Integer = 1 To (dtrRow.Item("行数") - 1)
                            Dim dtrNew As DataRow
                            dtrNew = objTbl.NewRow
                            dtrNew.Item("システム") = dtrRow.Item("システム")
                            dtrNew.Item("バージョン") = dtrRow.Item("バージョン")
                            dtrNew.Item("物品名") = dtrRow.Item("物品名")
                            dtrNew.Item("シリアル") = dtrRow.Item("シリアル")
                            dtrNew.Item("行数") = dtrRow.Item("行数")
                            dtrNew.Item("種類") = strTitle
                            dtrNew.Item("システムコード") = ddlSystem.SelectedValue
                            intRowCount += 1
                            objTbl.Rows.InsertAt(dtrNew, intRowCount)
                        Next
                    End If
                    Dim dtrNew1 As DataRow
                    dtrNew1 = objTbl.NewRow
                    dtrNew1.Item("システム") = dtrRow.Item("システム")
                    dtrNew1.Item("バージョン") = dtrRow.Item("バージョン")
                    dtrNew1.Item("物品名") = dtrRow.Item("物品名")
                    dtrNew1.Item("シリアル") = dtrRow.Item("シリアル")
                    dtrNew1.Item("行数") = dtrRow.Item("行数")
                    dtrNew1.Item("種類") = strTitle
                    dtrNew1.Item("システムコード") = ddlSystem.SelectedValue
                    intRowCount += 1
                    objTbl.Rows.Add(dtrNew1)
                Next
            Next

            '帳票のタイトル設定
            For Each dtrRow As DataRow In objTbl.Rows
                dtrRow.Item(0) = ddlSystem.SelectedItem.ToString.Split(":")(1) & "(" & ddlVer.Text & ")"
            Next

            '印刷
            If ddlMachine.SelectedValue = "01" Then
                rpt = New MSTREP055 ''制御部系の場合
            Else
                rpt = New MSTREP056 ''プリンタの場合
            End If

            '帳票出力
            psPrintPDF(Me, rpt, objTbl, Master.ppTitle)

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ドロップダウンリストシステム取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = objDs.Tables(0)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト持参分類取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMachine(ByRef ddlMac As DropDownList)

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM54_S3", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                ddlMac.Items.Clear()
                ddlMac.DataSource = objDs.Tables(0)
                ddlMac.DataTextField = "機器"
                ddlMac.DataValueField = "機器コード"
                ddlMac.DataBind()
                ddlMac.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト機器取得処理(グリッド用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlGridMachine(ByRef ddlGrvMac As DropDownList)

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM55_S4", objCn)
                objCmd.Parameters.Add(pfSet_Param("machine", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                ddlGrvMac.Items.Clear()
                ddlGrvMac.DataSource = objDs.Tables(0)
                ddlGrvMac.DataTextField = "機器種別"
                ddlGrvMac.DataValueField = "機器種別コード"
                ddlGrvMac.DataBind()
                ddlGrvMac.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' システム変更時のバージョン設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetVer()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        If Not ddlSystem.SelectedIndex = 0 Then
            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                Try
                    objCmd = New SqlCommand("COMUPDM54_S2", objCn)
                    objCmd.Parameters.Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    If objDs.Tables(0).Rows.Count > 0 Then
                        'ドロップダウンリスト設定
                        Me.ddlVer.Items.Clear()
                        Me.ddlVer.DataSource = objDs.Tables(0)
                        Me.ddlVer.DataTextField = "バージョン"
                        Me.ddlVer.DataBind()
                        Me.ddlVer.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        'ドロップダウンリスト活性化
                        Me.ddlVer.Enabled = True
                    Else
                        clsMst.psMesBox(Me, "10004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "選択されたＴＢＯＸタイプには、バージョン情報")
                        Me.ddlVer.Items.Clear()
                        Me.ddlVer.Enabled = False
                    End If

                Catch ex As Exception
                    clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "バージョン一覧取得")
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If
        End If

    End Sub

    ''' <summary>
    ''' 許容バージョンの取得とチェックボックスリストの作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetcklVer()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim intCkl As Integer = 0
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("COMUPDM55_S2", objCn)
                objCmd.Parameters.Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                Me.cklver.Items.Clear()
                Me.cklver.DataSource = objDs.Tables(0)
                Me.cklver.DataTextField = "許容バージョン"
                Me.cklver.DataBind()
                objCmd.Dispose()
                objDs.Dispose()

                If ddlInsUpd.SelectedIndex = 1 AndAlso ddlVer.SelectedValue <> "" Then         '更新モードの時だけ自動でチェックを付ける
                    objCmd = New SqlCommand("COMUPDM55_S3", objCn)
                    objCmd.Parameters.Add(pfSet_Param("system", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                    objCmd.Parameters.Add(pfSet_Param("ver", SqlDbType.NVarChar, ddlVer.SelectedValue))
                    objCmd.Parameters.Add(pfSet_Param("machine", SqlDbType.NVarChar, ddlMachine.SelectedValue))
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    '登録済み許容バージョンにチェックを付ける
                    If Not objDs.Tables(0).Rows.Count = 0 Then
                        Dim strVer(objDs.Tables(0).Rows.Count - 1) As String
                        For i As Integer = 0 To objDs.Tables(0).Rows.Count - 1
                            '登録済み許容バージョンを文字型配列へ変換
                            strVer(i) = objDs.Tables(0).Rows(i).Item(0).ToString
                        Next

                        intCkl = cklver.Items.Count
                        For i As Integer = 0 To intCkl - 1
                            For j As Integer = 0 To strVer.Count - 1
                                If cklver.Items(i).Text = strVer(j) Then
                                    'バージョンが一致すればチェックする
                                    cklver.Items(i).Selected = True
                                    Exit For
                                End If
                            Next
                        Next
                    End If

                End If

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "許容バージョン一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' GridViewのデータをDataTableに変換する。
    ''' </summary>
    ''' <param name="ipgrvData">変換するGridView</param>
    ''' <returns>変換したDataTable</returns>
    ''' <remarks></remarks>
    Private Function mfParse_DataTable(ByRef dtt As DataTable, ByVal ipgrvData As GridView) As Boolean
        Dim dtcData As DataColumn = Nothing
        Dim dtrData As DataRow = Nothing
        Dim strTitle As String

        '選択グリッドの設定
        Select Case ipgrvData.ClientID
            Case grvList1.ClientID
                strTitle = "ハード付属品関連"
            Case grvList2.ClientID
                strTitle = "保守ツール関連"
            Case grvList3.ClientID
                strTitle = "手順書関連"
            Case Else
                'エディタの注意回避用
                strTitle = ""
        End Select

        'カラム設定
        dtcData = New DataColumn
        dtt.Columns.Add("選択")
        dtt.Columns.Add("種類")
        dtt.Columns.Add("物品名DDL")
        dtt.Columns.Add("物品名")
        dtt.Columns.Add("システム")
        dtt.Columns.Add("バージョン")
        dtt.Columns.Add("シリアル")
        dtt.Columns.Add("行数")
        dtt.Columns.Add("登録日時")
        dtt.Columns.Add("登録者")

        '行データコピー
        For Each rowData As GridViewRow In ipgrvData.Rows
            dtrData = dtt.NewRow
            If ipgrvData.Equals(grvList1) Then
                dtrData.Item("物品名DDL") = CType(rowData.Cells(C_DDL).Controls(0), DropDownList).SelectedValue
            Else
                dtrData.Item("物品名DDL") = ""
            End If
            dtrData.Item("物品名") = CType(rowData.Cells(C_NM).Controls(0), TextBox).Text
            dtrData.Item("システム") = CType(rowData.Cells(C_SYS).Controls(0), TextBox).Text
            dtrData.Item("バージョン") = CType(rowData.Cells(C_VER).Controls(0), TextBox).Text
            dtrData.Item("シリアル") = ""
            dtrData.Item("行数") = CType(rowData.Cells(C_LINE).Controls(0), TextBox).Text
            dtrData.Item("登録日時") = CType(rowData.Cells(C_DT).Controls(0), TextBox).Text
            dtrData.Item("登録者") = CType(rowData.Cells(C_USER).Controls(0), TextBox).Text

            'コピー行をデータテーブルに追加
            dtt.Rows.Add(dtrData)
        Next

        Return True
    End Function

    ''' <summary>
    ''' DataTableからGridViewのドロップダウンリストに値を設定する。
    ''' </summary>
    ''' <param name="grv"></param>
    ''' <param name="dttData"></param>
    ''' <param name="Mode"></param>
    ''' <remarks></remarks>
    Private Sub msGridDDLBind(ByRef grv As GridView, ByVal dttData As DataTable, ByVal Mode As String)

        Select Case Mode
            '追加行に対しては選択する値を設定できない為、行追加処理とそれ以外で処理を分ける。
            Case "Add"
                For Each rowData As GridViewRow In grv.Rows

                    If rowData.RowIndex = grv.Rows.Count - 1 Then   '追加行の値は設定しない(空欄のままとする)
                        Exit Sub
                    End If

                    Dim inttmp As Integer
                    If Not Integer.TryParse(dttData.Rows(rowData.RowIndex).Item("物品名DDL"), inttmp) Then
                        CType(rowData.Cells(C_DDL).Controls(0), DropDownList).SelectedIndex = 0
                    Else  'ここに変数inttmpは使えない(SelectedIndexが使えない)
                        CType(rowData.Cells(C_DDL).Controls(0), DropDownList).SelectedValue = dttData.Rows(rowData.RowIndex).Item("物品名DDL")
                    End If

                Next

            Case "Delete", "Search", "LineUpDw"
                For Each rowData As GridViewRow In grv.Rows
                    Dim inttmp As Integer
                    If Not Integer.TryParse(dttData.Rows(rowData.RowIndex).Item("物品名DDL"), inttmp) Then
                        CType(rowData.Cells(C_DDL).Controls(0), DropDownList).SelectedIndex = 0
                    Else   'ここに変数inttmpは使えない(SelectedIndexが使えない)
                        CType(rowData.Cells(C_DDL).Controls(0), DropDownList).SelectedValue = dttData.Rows(rowData.RowIndex).Item("物品名DDL")
                    End If
                Next
        End Select

    End Sub

    ''' <summary>
    ''' GridView内の行数入力チェック
    ''' </summary>
    ''' <param name="grv"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGridValidate(ByVal grv As GridView) As Boolean
        Dim strTitle As String
        Dim intI As Integer

        Select Case grv.ClientID
            Case grvList1.ClientID
                strTitle = "ハード付属品関連"
            Case grvList2.ClientID
                strTitle = "保守ツール関連"
            Case grvList3.ClientID
                strTitle = "手順書関連"
            Case Else
                'エディタの警告回避用
                strTitle = ""
        End Select

        For Each rowData As GridViewRow In grv.Rows
            '未入力エラー
            If CType(rowData.Cells(C_LINE).Controls(0), TextBox).Text.Trim = String.Empty Then
                blnIsInput = False
                strErrGrvNm_Inp += "　" & strTitle
                Return False
            End If
            If CType(rowData.Cells(C_NM).Controls(0), TextBox).Text.Trim = String.Empty Then
                blnIsInput = False
                strErrGrvNm_Inp += "　" & strTitle
                Return False
            End If

            '不正入力エラー
            If Integer.TryParse(CType(rowData.Cells(C_LINE).Controls(0), TextBox).Text, intI) Then
                If intI = 0 Then
                    blnIsNum = False
                    strErrGrvNm_Num += "　" & strTitle
                    Return False
                End If
            Else
                blnIsNum = False
                strErrGrvNm_Num += "　" & strTitle
                Return False
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' 行上下ボタンの活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub btnUpDownEnable(strMode As String)

        '行上下ボタンは選択中の一覧のみ使用可能とする
        Select Case strMode
            Case "DEF"
                btnGrv1Up.Enabled = False
                btnGrv1Dw.Enabled = False
                btnGrv2Up.Enabled = False
                btnGrv2Dw.Enabled = False
                btnGrv3Up.Enabled = False
                btnGrv3Dw.Enabled = False

            Case "HARD"
                If hdnSelectLine.Value = 0 Then
                    btnGrv1Up.Enabled = False
                Else
                    btnGrv1Up.Enabled = True
                End If
                If hdnSelectLine.Value >= grvList1.Rows.Count - 1 Then
                    btnGrv1Dw.Enabled = False
                Else
                    btnGrv1Dw.Enabled = True
                End If
                btnGrv2Up.Enabled = False
                btnGrv2Dw.Enabled = False
                btnGrv3Up.Enabled = False
                btnGrv3Dw.Enabled = False

            Case "TOOL"
                If hdnSelectLine.Value = 0 Then
                    btnGrv2Up.Enabled = False
                Else
                    btnGrv2Up.Enabled = True
                End If
                If hdnSelectLine.Value >= grvList2.Rows.Count - 1 Then
                    btnGrv2Dw.Enabled = False
                Else
                    btnGrv2Dw.Enabled = True
                End If
                btnGrv1Up.Enabled = False
                btnGrv1Dw.Enabled = False
                btnGrv3Up.Enabled = False
                btnGrv3Dw.Enabled = False

            Case "MANUAL"
                If hdnSelectLine.Value = 0 Then
                    btnGrv3Up.Enabled = False
                Else
                    btnGrv3Up.Enabled = True
                End If
                If hdnSelectLine.Value >= grvList3.Rows.Count - 1 Then
                    btnGrv3Dw.Enabled = False
                Else
                    btnGrv3Dw.Enabled = True
                End If
                btnGrv1Up.Enabled = False
                btnGrv1Dw.Enabled = False
                btnGrv2Up.Enabled = False
                btnGrv2Dw.Enabled = False
        End Select
    End Sub

    ''' <summary>
    ''' グリッドの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CleanGrid()
        Me.grvList1.DataSource = New DataTable
        Me.grvList1.DataBind()
        Me.grvList2.DataSource = New DataTable
        Me.grvList2.DataBind()
        Me.grvList3.DataSource = New DataTable
        Me.grvList3.DataBind()
        lblcount1.Text = "0"
        lblcount2.Text = "0"
        lblcount3.Text = "0"
        'データが0行なので選択行の設定も初期化する。
        msSelectLine_Reset()
    End Sub

    ''' <summary>
    ''' グリッドビューの総件数を返す
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGrvRowCount() As Integer
        mfGrvRowCount = grvList1.Rows.Count + grvList2.Rows.Count + grvList3.Rows.Count
    End Function

    ''' <summary>
    ''' プレビューボタンの活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPrvEnable()
        Dim btnFtPrev As Button = DirectCast(Master.FindControl("btnLeft1"), Button)
        '一覧にデータが1行でもあれば有効化
        If mfGrvRowCount() = 0 Then
            btnFtPrev.Enabled = False
        Else
            btnFtPrev.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' 排他処理
    ''' </summary>
    ''' <param name="strCmd"></param>
    ''' <param name="keysys"></param>
    ''' <param name="keyver"></param>
    ''' <param name="keymac"></param>
    ''' <remarks></remarks>
    Private Sub excExclusive(strCmd As String, Optional ByVal keysys As String = Nothing, _
                             Optional ByVal keyver As String = Nothing, Optional ByVal keymac As String = Nothing)
        Select Case strCmd
            Case "NEW"
                Me.Master.ppExclusiveDate = Session("ExclusiveDate")
                Session("ExclusiveDate") = Nothing

            Case "SET"
                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★排他情報削除
                If Not Me.Master.ppExclusiveDate = String.Empty Then
                    If clsExc.pfDel_Exclusive(Me _
                                       , Session(P_SESSION_SESSTION_ID) _
                                       , Me.Master.ppExclusiveDate) = 0 Then
                        Me.Master.ppExclusiveDate = String.Empty
                    Else
                        'ログ出力終了
                        psLogEnd(Me)
                        Exit Sub
                    End If
                End If

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, TableName)

                '★ロックテーブルキー項目の登録
                arKey.Insert(0, keysys)
                arKey.Insert(0, keyver)
                arKey.Insert(0, keymac)

                '★排他情報確認処理
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , DispCode _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻(明細)
                    Me.Master.ppExclusiveDate = strExclusiveDate

                Else
                    '排他ロック中
                    'ログ出力終了
                    psLogEnd(Me)
                    Exit Sub
                End If

            Case "DEL"
                '★排他情報削除
                If Not Me.Master.ppExclusiveDate = String.Empty Then
                    If clsExc.pfDel_Exclusive(Me _
                                       , Session(P_SESSION_SESSTION_ID) _
                                       , Me.Master.ppExclusiveDate) = 0 Then
                        Me.Master.ppExclusiveDate = String.Empty
                    Else
                        'ログ出力終了
                        psLogEnd(Me)
                        Exit Sub
                    End If
                End If
        End Select
    End Sub

    ''' <summary>
    ''' オブジェクトの初期化とイベント設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub obj_Init()
        'ボタン設定
        Dim btnFtPrev As Button = DirectCast(Master.FindControl("btnLeft1"), Button)
        Dim btnFtClear As Button = Master.ppBtnSrcClear
        Dim btnFtLineAdd As Button = DirectCast(Master.FindControl("btnRigth5"), Button)
        Dim btnFtLineDel As Button = DirectCast(Master.FindControl("btnRigth4"), Button)
        Dim btnFtIns As Button = DirectCast(Master.FindControl("btnRigth3"), Button)
        Dim btnFtUpdate As Button = DirectCast(Master.FindControl("btnRigth2"), Button)
        Dim btnFtDelete As Button = DirectCast(Master.FindControl("btnRigth1"), Button)

        'ボタンスクリプト設定
        btnFtIns.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, USEMST_NM)       '登録
        btnFtUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, USEMST_NM)       '更新
        btnFtDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, USEMST_NM)       '削除

        'ボタンイベント設定
        AddHandler btnFtIns.Click, AddressOf btnIns_Click                        '登録ボタン
        AddHandler btnFtUpdate.Click, AddressOf btnUpd_Click                        '更新ボタン
        AddHandler btnFtPrev.Click, AddressOf btnPrev_Click                       'プレビュ-
        AddHandler btnFtClear.Click, AddressOf btnClear_Click
        AddHandler btnFtLineAdd.Command, AddressOf btnLine_Click
        AddHandler btnFtLineDel.Command, AddressOf btnLine_Click

        'スクロール位置調整
        btnFtClear.Attributes.Add("onClick", "scrolldownreset();")
        Me.btnGrid1.Attributes.Add("onClick", "scrolldown1();")
        Me.btnGrid2.Attributes.Add("onClick", "scrolldown2();")
        Me.btnGrid3.Attributes.Add("onClick", "scrolldownmax();")

        btnFtLineAdd.Visible = True
        btnFtLineAdd.Text = "行追加"
        btnFtLineAdd.CommandName = "Add"

        btnFtLineDel.Visible = True
        btnFtLineDel.Text = "行削除"
        btnFtLineDel.CommandName = "Delete"
        btnFtLineDel.BackColor = Drawing.Color.FromArgb(255, 102, 102)

        btnFtPrev.Visible = True
        btnFtPrev.Text = "プレビュー"

        btnFtClear.Text = "クリア"

        btnFtIns.Visible = True
        btnFtIns.Text = "登録"

        btnFtUpdate.Visible = True
        btnFtUpdate.Text = "更新"

        'スクリプトマネージャーに登録
        '登録しないとポストバックでグリッドが更新されない
        Dim scm As ScriptManager
        scm = Master.FindControl("tsmManager")
        scm.RegisterPostBackControl(ddlInsUpd)
        scm.RegisterPostBackControl(ddlMachine)
        scm.RegisterPostBackControl(ddlVer)
        scm.RegisterPostBackControl(btnFtClear)

        'ドロップダウンリストのイベント設定
        AddHandler ddlSystem.SelectedIndexChanged, AddressOf ddlSystem_SelectedIndexChanged
        ddlSystem.AutoPostBack = True
        AddHandler ddlMachine.SelectedIndexChanged, AddressOf ddlMachine_SelectedIndexChanged
        ddlMachine.AutoPostBack = True
        AddHandler ddlVer.SelectedIndexChanged, AddressOf ddlVer_SelectedIndexChanged
        ddlVer.AutoPostBack = True
        AddHandler ddlInsUpd.SelectedIndexChanged, AddressOf ddlInsUpd_SelectedIndexChanged
        ddlInsUpd.AutoPostBack = True

        Master.ppBtnSearch.Visible = False

    End Sub

    ''' <summary>
    ''' ボタン押下時のスクロール設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetScrollEvent()
        'ボタン設定
        Dim btnFtLineAdd As Button = DirectCast(Master.FindControl("btnRigth5"), Button)
        Dim btnFtLineDel As Button = DirectCast(Master.FindControl("btnRigth4"), Button)
        Dim strCommand As String = String.Empty

        'スクロール値を設定(選択一覧によって変更)
        Select Case hdnMode.Value
            Case "HARD"
                strCommand = "scrolldown1();"
            Case "TOOL"
                strCommand = "scrolldown2();"
            Case "MANUAL"
                strCommand = "scrolldownmax();"
        End Select

        'ボタン押下時のスクロール調整
        btnFtLineAdd.Attributes.Add("onClick", strCommand)
        btnFtLineDel.Attributes.Add("onClick", strCommand)
    End Sub

    ''' <summary>
    ''' スクロール位置をリセット(一番上)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msScrollReset()

        'スクロール位置を縦横0とするスクリプトをページに埋め込む(実行は描画後)
        'window.scrolltopではMaintainScrollPositionOnPostBackが優先される
        Dim strScript As New System.Text.StringBuilder
        strScript.Append("document.getElementById(""__SCROLLPOSITIONX"").value=0;")
        strScript.Append("document.getElementById(""__SCROLLPOSITIONY"").value=0;")
        ClientScript.RegisterStartupScript(Page.GetType(), "", strScript.ToString(), True)

    End Sub

    ''' <summary>
    ''' エラーメッセージ表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msErrMes()
        Dim strCommand As String
        Dim blnDouble_ErrFlg As Boolean = False

        'メッセージ用に今回の処理を取得
        Select Case hdnCmd.Value
            Case "Add"
                strCommand = "行追加"
            Case "Ins"
                strCommand = "登録処理"
            Case "Upd"
                strCommand = "更新処理"
            Case "Prev"
                strCommand = "プレビュー"
            Case Else
                strCommand = "処理"
        End Select

        '先頭の空白を削除
        strErrGrvNm_Inp = strErrGrvNm_Inp.TrimStart
        strErrGrvNm_Num = strErrGrvNm_Num.TrimStart

        '2種類のエラーが発生している時
        If strErrGrvNm_Inp <> String.Empty AndAlso strErrGrvNm_Num <> String.Empty Then
            blnDouble_ErrFlg = True
        End If

        If blnDouble_ErrFlg = True Then '種類毎にエラー内容が異なった場合
            psMesBox(Me, "30029", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, "追加した内容が不適切です。", "追加した内容")
        ElseIf blnIsInput = False Then '未入力エラー
            psMesBox(Me, "30029", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strErrGrvNm_Inp & "\n明細の入力内容が不十分です。", "追加した内容")
        ElseIf blnIsNum = False Then '不正入力エラー
            psMesBox(Me, "30030", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strErrGrvNm_Num & "\n行数は1以上の数値")
        End If

        '初期化
        blnDouble_ErrFlg = False
        blnIsInput = True
        blnIsNum = True
        strErrGrvNm_Inp = String.Empty
        strErrGrvNm_Num = String.Empty

    End Sub

    ''' <summary>
    ''' 選択行をリセット
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSelectLine_Reset()
        'データが存在する一番上の一覧の一番上の行に選択行を設定する
        '１行も存在しない場合は、選択一覧と選択行を初期化
        If grvList1.Rows.Count > 0 Then
            hdnMode.Value = "HARD"
            hdnSelectLine.Value = 0
        ElseIf grvList2.Rows.Count > 0 Then
            hdnMode.Value = "TOOL"
            hdnSelectLine.Value = 0
        ElseIf grvList3.Rows.Count > 0 Then
            hdnMode.Value = "MANUAL"
            hdnSelectLine.Value = 0
        Else
            hdnMode.Value = "DEF"
            hdnSelectLine.Value = -1
        End If
    End Sub

#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' Dispose時の排他情報削除処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Dispose(sender As Object, e As EventArgs) Handles Me.Disposed

        '排他情報削除
        excExclusive("DEL")

    End Sub

#End Region

End Class
