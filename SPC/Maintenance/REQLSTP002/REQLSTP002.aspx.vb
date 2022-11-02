'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　持参物品一覧
'*　ＰＧＭＩＤ：　REQLSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.25　：　土岐
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'                   2014/06/17      間瀬      レイアウト変更
'                   2014/06/19      稲葉      データ検索時のメッセージ変更
'                   2014/09/01      稲葉      メッセージボックスのタイプをエラー⇒警告に変更
'REQLSTP002-001     2015/11/11      稲葉      
'REQLSTP002-002     2015/11/13      武        制御部のバージョン、プリンタのシステムの表示内容修正
'REQLSTP002-003     2016/01/22      栗原      帳票作成時の項目追加（サポートセンタTEL）
'REQLSTP002-004     2016/07/07      稲葉      帳票作成時の処理変更
'REQLSTP002-005     2016/09/16      稲葉      シリアル重複対応(シリアル管理テーブル定義変更)

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
#End Region

Public Class REQLSTP002
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
    Private Const M_DISP_ID = P_FUN_REQ & P_SCR_LST & P_PAGE & "002"
    Private Const M_DEAL_DISP_ID = P_FUN_CMP & P_SCR_SEL & P_PAGE & "001"   '保守対応依頼書
    Private Const M_VIEW_TBOXID = "ＴＢＯＸＩＤ"
    Private Const M_VIEW_SYSTEM_CD = "システムコード"
    Private Const M_VIEW_SERIALCNTL_CLS = "管理番号種類"
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
    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        'pfSet_GridView(Me.grvList, "REQLSTP002")

        Dim intHeadCol As Integer() = New Integer() {1, 3, 5}
        Dim intColSpan As Integer() = New Integer() {2, 2, 2}
        pfSet_GridView(Me.grvList, "REQLSTP002", intHeadCol, intColSpan)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Const WARNING_NO = "30006"
        Dim strMes As String
        Dim strMesID As String

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click

        AddHandler Me.tsrStrageCD.ppTextBox.TextChanged, AddressOf tsrStrageCD_TextChanged
        If Not IsPostBack Then  '初回表示

            '「クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
            Me.tsrStrageCD.ppTextBox.AutoPostBack = True

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'セッション情報取得
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            If Session(P_KEY) Is Nothing Then
                ViewState(P_KEY) = Nothing
            Else
                ViewState(P_KEY) = Session(P_KEY)(0)
                ViewState(P_SESSION_OLDDISP) = Session(P_SESSION_OLDDISP)
            End If

            '(1：保守/2：工事/その他:なし)
            If ViewState(P_SESSION_OLDDISP) IsNot Nothing Then
                Select Case ViewState(P_SESSION_OLDDISP)
                    Case M_DEAL_DISP_ID
                        ViewState(M_VIEW_SERIALCNTL_CLS) = "1"
                        'Case   '現在工事業務からの遷移なし
                    Case Else
                        ViewState(M_VIEW_SERIALCNTL_CLS) = Nothing
                End Select
            End If
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'メッセージ取得
            strMes = pfGet_Mes(WARNING_NO, ClsComVer.E_Mタイプ.警告)
            strMesID = pfGet_MesType(ClsComVer.E_Mタイプ.警告) & WARNING_NO

            'スクリプト設定
            Me.btnSelect.OnClientClick =
                String.Format("return setCheck('{0}', '{1}', '{2}', '{3}', '{4}');",
                              Me.grvSetList.ClientID,
                              Me.ddlStrage.ClientID,
                              Me.ddlModel.ClientID,
                              strMes,
                              strMesID)

            Me.btnPrint.OnClientClick =
                pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "持参物品一覧")

            '画面初期化処理
            msClearScreen()

            If Session(P_KEY) IsNot Nothing Then
                Me.txtTboxID.ppText = Session(P_KEY)(1)
                '検索処理
                msGet_Data(Me.txtTboxID.ppText)
            End If
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
                tsrStrageCD.ppEnabled = False
            Case "NGC"
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
            msGet_Data(Me.txtTboxID.ppText)
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
    ''' 別拠点コード変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub tsrStrageCD_TextChanged(sender As Object, e As EventArgs)
        Dim dtsData As DataSet = Nothing
        If mfGet_OfficeNm(Me.tsrStrageCD.ppText, dtsData) Then
            Me.lblStrageNmV.Text = dtsData.Tables(0).Rows(0).Item("営業所名")
        End If
    End Sub

    ''' <summary>
    ''' 別拠点変更ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        'ログ出力開始
        psLogStart(Me)
        '別拠点未入力（処理なし）
        If Me.tsrStrageCD.ppText = String.Empty Then
            Exit Sub
        End If

        '未検索
        If ViewState(M_VIEW_TBOXID) = Nothing Then
            psMesBox(Me, "30012", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "別拠点の変更")
            Me.txtTboxID.ppTextBox.Focus()
            Exit Sub
        End If

        '整合性エラー
        If Me.tsrStrageCD.ppText = "900" Then
            Me.tsrStrageCD.psSet_ErrorNo("7002", Me.tsrStrageCD.ppName)
            Me.tsrStrageCD.ppTextBox.Focus()
            Me.lblStrageNmV.Text = String.Empty
        ElseIf Not mfGet_OfficeNm(Me.tsrStrageCD.ppText, Nothing) Then
            Me.tsrStrageCD.psSet_ErrorNo("2002", Me.tsrStrageCD.ppName)
            Me.tsrStrageCD.ppTextBox.Focus()
            Me.lblStrageNmV.Text = String.Empty
        End If

        If (Page.IsValid) Then
            msGet_StrageData(ViewState(M_VIEW_TBOXID),
                             ViewState(M_VIEW_SYSTEM_CD),
                             Me.lblVersionV.Text,
                             Me.tsrStrageCD.ppText)
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        'ログ出力開始
        psLogStart(Me)
        msGet_Select(Me.ddlStrage.SelectedValue, Me.ddlModel.SelectedValue)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim conDB As SqlConnection = Nothing
        Dim intRtn As Integer
        Dim blnCommit As Boolean = False

        objStack = New StackFrame

        'ログ出力開始
        psLogStart(Me)
        '接続
        If clsDataConnect.pfOpen_Database(conDB) = False Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            msClearScreen()
        End If

        'データ追加／更新
        Using conTrn = conDB.BeginTransaction(IsolationLevel.Serializable)
            Try
                For Each grdRow As GridViewRow In Me.grvList.Rows

                    '持参物品以外は更新しない
                    If CType(grdRow.FindControl("持参"), TextBox).Text <> "●" Then
                        Continue For
                    End If

                    'REQLSTP002-005 引数追加
                    'チェック処理
                    intRtn = mf_CheckSerial(conDB,
                                            conTrn,
                                            CType(grdRow.FindControl("シリアル番号"), TextBox).Text,
                                            CType(grdRow.FindControl("システムコード"), TextBox).Text,
                                            CType(grdRow.FindControl("機器分類"), TextBox).Text,
                                            CType(grdRow.FindControl("機器種別"), TextBox).Text,
                                            CType(grdRow.FindControl("型式/機器"), TextBox).Text,
                                            CType(grdRow.FindControl("現設置/保管コード"), TextBox).Text,
                                            CType(grdRow.FindControl("連番"), TextBox).Text)

                    If intRtn <> 0 Then '変更有
                        psMesBox(Me, "10003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If

                    'REQLSTP002-005 引数追加
                    'シリアル管理更新
                    intRtn = mf_UpdateSerial(conDB,
                                             conTrn,
                                             CType(grdRow.FindControl("シリアル番号"), TextBox).Text,
                                             CType(grdRow.FindControl("システムコード"), TextBox).Text,
                                             CType(grdRow.FindControl("機器分類"), TextBox).Text,
                                             CType(grdRow.FindControl("機器種別"), TextBox).Text,
                                             CType(grdRow.FindControl("型式/機器"), TextBox).Text,
                                             ViewState(P_KEY),
                                             ViewState(M_VIEW_SERIALCNTL_CLS),
                                             CType(grdRow.FindControl("連番"), TextBox).Text)
                    'エラーチェック
                    If intRtn <> 0 Then
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "シリアル管理", intRtn.ToString)
                        Exit Sub
                    End If

                    'REQLSTP002-005 引数追加
                    'シリアル履歴追加
                    intRtn = mf_AddSerialHst(conDB,
                                             conTrn,
                                             CType(grdRow.FindControl("シリアル番号"), TextBox).Text,
                                             CType(grdRow.FindControl("システムコード"), TextBox).Text,
                                             CType(grdRow.FindControl("機器分類"), TextBox).Text,
                                             CType(grdRow.FindControl("機器種別"), TextBox).Text,
                                             CType(grdRow.FindControl("型式/機器"), TextBox).Text,
                                             CType(grdRow.FindControl("連番"), TextBox).Text)
                    'エラーチェック
                    If intRtn <> 0 Then
                        psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "シリアル履歴", intRtn.ToString)
                        Exit Sub
                    End If
                Next

                '印刷処理
                msPrint(conDB,
                        conTrn,
                        ViewState(M_VIEW_TBOXID),
                        Me.ddlStrage.SelectedValue,
                        Me.ddlModel.SelectedValue,
                        pfParse_DataTable(Me.grvList))

                'コミット
                'conTrn.Rollback()
                conTrn.Commit()

                blnCommit = True
            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "シリアル管理")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                msClearScreen()
            Finally
                If blnCommit = False Then
                    conTrn.Rollback()
                End If
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End Using

        'ログ出力終了
        psLogEnd(Me)
    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()
        Me.txtTboxID.ppText = String.Empty
        Me.lblHallNameV.Text = String.Empty
        Me.lblTboxClassCDV.Text = String.Empty
        Me.lblVersionV.Text = String.Empty
        Me.lblMntCDV.Text = String.Empty
        Me.lblMntNmV.Text = String.Empty
        Me.lblUnfCDV.Text = String.Empty
        Me.lblUnfNmV.Text = String.Empty
        Me.tsrStrageCD.ppText = String.Empty
        Me.lblStrageNmV.Text = String.Empty
        Me.ddlStrage.Items.Clear()                  '拠点リストクリア
        Me.ddlModel.Items.Clear()                   '機器型式リストクリア

        Master.ppCount = 0
        Me.grvList.DataSource = New Object() {}
        Me.grvList.DataBind()
        ViewState(M_VIEW_TBOXID) = Nothing
        ViewState(M_VIEW_SYSTEM_CD) = Nothing
        Me.txtTboxID.ppTextBox.Focus()
    End Sub

    ''' <summary>
    ''' 検索項目クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()
        Me.txtTboxID.ppText = String.Empty
    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <param name="ipstrTboxid">ＴＢＯＸＩＤ</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrTboxid As String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstData As DataSet = Nothing
        Dim dstList As DataSet = Nothing
        Dim dstDataS6 As DataSet = Nothing
        Dim strMnt_cd As String = String.Empty
        Dim strUnf_cd As String = String.Empty
        Dim strTboxClassCD As String = String.Empty
        Dim strVersion As String = String.Empty
        Dim dttData As DataTable = Nothing
        Dim dttNGData As DataTable = Nothing
        Dim dtrData As DataRow = Nothing

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("REQLSTP002_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxid))
                End With

                'データ取得
                dstData = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstData.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    '件数を設定
                    Master.ppCount = dstData.Tables(0).Rows.Count.ToString
                    Return
                End If


                cmdDB = New SqlCommand("REQLSTP002_S6", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxid))
                End With

                'データ取得
                dstDataS6 = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstDataS6.Tables(0).Rows.Count = 0 Then
                    'メッセージをエラーから警告へ変更
                    'psMesBox(Me, "30016", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "持参物品マスタ")
                    psMesBox(Me, "10004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "持参物品マスタ")
                    Return
                End If

                '取得したデータを画面に設定
                msSet_MainData(dstData)

                With dstData.Tables(0).Rows(0)
                    strTboxClassCD = .Item("システムコード").ToString
                    strVersion = .Item("バージョン").ToString
                    strMnt_cd = .Item("保守担当コード").ToString
                    strUnf_cd = .Item("統括コード").ToString
                End With

                'リストデータ
                cmdDB = New SqlCommand("REQLSTP002_S2", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxid))
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, strTboxClassCD))
                    .Add(pfSet_Param("version", SqlDbType.NVarChar, strVersion))
                    .Add(pfSet_Param("strage_cd", SqlDbType.NVarChar, strMnt_cd))
                    .Add(pfSet_Param("cond_flg", SqlDbType.NVarChar, "0"))
                End With

                'データ取得
                dstList = clsDataConnect.pfGet_DataSet(cmdDB)
                dttData = dstList.Tables(0)
                dttNGData = dstList.Tables(1)

                '保守と統括拠点が同じ場合は検索しない
                If strMnt_cd <> strUnf_cd Then
                    '統括
                    cmdDB = New SqlCommand("REQLSTP002_S2", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxid))
                        .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, strTboxClassCD))
                        .Add(pfSet_Param("version", SqlDbType.NVarChar, strVersion))
                        .Add(pfSet_Param("strage_cd", SqlDbType.NVarChar, strUnf_cd))
                        .Add(pfSet_Param("cond_flg", SqlDbType.NVarChar, "0"))
                    End With

                    'データ取得
                    dstList = clsDataConnect.pfGet_DataSet(cmdDB)
                    'データ結合
                    dttData.Merge(dstList.Tables(0))
                    dttNGData.Merge(dstList.Tables(1))
                End If

                If dttData.Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                '件数を設定
                Master.ppCount = dttData.Rows.Count.ToString

                '完全に一致する項目は在庫数を分散させる
                For intIndex As Integer = 0 To dttNGData.Rows.Count - 2
                    For intChack As Integer = intIndex + 1 To dttNGData.Rows.Count - 1
                        With dttNGData
                            If dttNGData.Rows(intIndex).Item("拠点コード").ToString = dttNGData.Rows(intChack).Item("拠点コード").ToString _
                                    And .Rows(intIndex).Item("機器種別コード").ToString = dttNGData.Rows(intChack).Item("機器種別コード").ToString _
                                    And .Rows(intIndex).Item("ＨＤＤＮｏ").ToString = dttNGData.Rows(intChack).Item("ＨＤＤＮｏ").ToString _
                                    And .Rows(intIndex).Item("ＨＤＤ種別").ToString = dttNGData.Rows(intChack).Item("ＨＤＤ種別").ToString Then
                                If dttNGData.Rows(intIndex).Item("持参数").ToString < dttNGData.Rows(intIndex).Item("在庫数").ToString Then
                                    'dttNGData.Rows(intChack).Item("在庫数") = (Integer.Parse(dttNGData.Rows(intIndex).Item("在庫数").ToString) - Integer.Parse(dttNGData.Rows(intIndex).Item("持参数").ToString)).ToString
                                    dttNGData.Rows(intChack).Item("在庫数") = dttNGData.Rows(intChack).Item("在庫数") + (Integer.Parse(dttNGData.Rows(intIndex).Item("在庫数").ToString) - Integer.Parse(dttNGData.Rows(intIndex).Item("持参数").ToString)).ToString

                                    dttNGData.Rows(intIndex).Item("在庫数") = dttNGData.Rows(intIndex).Item("持参数").ToString
                                End If
                            End If
                        End With
                    Next
                Next

                Me.grvList.DataSource = dttData
                Me.grvSetList.DataSource = dttNGData

                '別拠点クリア
                Me.tsrStrageCD.ppText = String.Empty
                Me.lblStrageNmV.Text = String.Empty

                '変更を反映
                Me.grvList.DataBind()
                Me.grvSetList.DataBind()

                '機器型式リスト
                msSetddlModel(strTboxClassCD)

                '拠点リスト設定
                msSetStrageList()

                ViewState(M_VIEW_TBOXID) = ipstrTboxid
                ViewState(M_VIEW_SYSTEM_CD) = strTboxClassCD
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "持参物品一覧")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                msClearScreen()
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            msClearScreen()
        End If
    End Sub

    ''' <summary>
    ''' 拠点検索処理
    ''' </summary>
    ''' <param name="ipstrTboxid">ＴＢＯＸＩＤ</param>
    ''' <param name="ipstrTboxClassCD">システムコード</param>
    ''' <param name="ipstrVersion">バージョン</param>
    ''' <param name="ipstrStrage">拠点コード</param>
    ''' <remarks></remarks>
    Private Sub msGet_StrageData(ByVal ipstrTboxid As String,
                                 ByVal ipstrTboxClassCD As String,
                                 ByVal ipstrVersion As String,
                                 ByVal ipstrStrage As String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstData As DataSet = Nothing
        Dim dstList As DataSet = Nothing
        Dim dttData As DataTable = Nothing
        Dim dttNGData As DataTable = Nothing

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'リストデータ
                cmdDB = New SqlCommand("REQLSTP002_S2", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxid))
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ipstrTboxClassCD))
                    .Add(pfSet_Param("version", SqlDbType.NVarChar, ipstrVersion))
                    .Add(pfSet_Param("strage_cd", SqlDbType.NVarChar, ipstrStrage))
                    .Add(pfSet_Param("cond_flg", SqlDbType.NVarChar, "0"))
                End With

                'データ取得
                dstList = clsDataConnect.pfGet_DataSet(cmdDB)
                dttData = dstList.Tables(0)
                dttNGData = dstList.Tables(1)

                If dttData.Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Exit Sub
                End If

                '件数を設定
                Master.ppCount = dttData.Rows.Count.ToString

                Me.grvList.DataSource = dttData
                Me.grvSetList.DataSource = dttNGData

                '変更を反映
                Me.grvList.DataBind()
                Me.grvSetList.DataBind()

                '機器型式リスト
                msSetddlModel(ipstrTboxClassCD)

                '拠点リスト設定
                msSetStrageList()
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "持参物品一覧")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                msClearScreen()
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            msClearScreen()
        End If
    End Sub

    ''' <summary>
    ''' 選択処理
    ''' </summary>
    ''' <param name="ipstrStrageCd"></param>
    ''' <param name="ipstrModelCd"></param>
    ''' <remarks></remarks>
    Private Sub msGet_Select(ByVal ipstrStrageCd As String,
                             ByVal ipstrModelCd As String)
        Dim strAppaknd As String
        Dim dttData As DataTable
        Dim strSearch As StringBuilder
        Dim dtrData() As DataRow
        Dim intCount As Integer
        Dim intAllCount As Integer
        Dim strHDDNo As String
        Dim strHDDCls As String
        Dim strSystemCd As String
        Dim strAppaCdBef As String = String.Empty
        Dim strHddNoBef As String = String.Empty
        Dim strHddClsBef As String = String.Empty

        '選択リストを一旦保存
        Dim strSelectPrint As String = ipstrModelCd

        'GridViewからデータ取得（一覧表示）
        dttData = pfParse_DataTable(Me.grvList)

        '更新用テーブルに行数追加
        dttData.Columns.Add("行数")

        '更新用テーブルの持参クリア
        dttData.Columns.Remove("持参")
        dttData.Columns.Add("持参")

        Select Case ipstrModelCd
            Case "1"
                ipstrModelCd = "01"
            Case "3", "4"
                ipstrModelCd = "04"
        End Select

        For zz As Integer = 0 To dttData.Rows.Count - 1
            dttData.Rows(zz).Item("行数") = zz
        Next

        strSystemCd = dttData.Rows(0).Item("システムコード").ToString

        intAllCount = 0

        '選択された拠点、機器型式のセット機器取得
        For Each gvrRow As GridViewRow In Me.grvSetList.Rows
            '拠点チェック
            If gvrRow.Cells(0).Text <> ipstrStrageCd Then
                Continue For
            End If

            ''機器型式チェック
            If ipstrModelCd = "04" Then '出力機器がプリンタ
                If gvrRow.Cells(1).Text <> "04" And gvrRow.Cells(1).Text <> "05" Then
                    Continue For
                End If
                If gvrRow.Cells(7).Text <> strSelectPrint Then
                    Continue For
                End If
                strAppaknd = "([機器種別コード] = '04' OR [機器種別コード] = '05')"
            Else                        'プリンタ以外
                If gvrRow.Cells(1).Text = "04" Or gvrRow.Cells(1).Text = "05" Then
                    Continue For
                End If
                strAppaknd = "[機器種別コード] <> '04' AND [機器種別コード] <> '05' "
            End If

            'セット機器から対象機器を選定し持参に「●」をつける
            strSearch = New StringBuilder
            strSearch.Append(String.Format("     [現設置/保管コード] = '{0}'", ipstrStrageCd))
            strSearch.Append(String.Format(" AND [機器種別] = '{0}'", gvrRow.Cells(1).Text))

            If ipstrModelCd = "04" AndAlso Me.ddlModel.SelectedValue = "3" Then
                If strSystemCd = "30" Then
                    strSearch.Append(" AND [備考] LIKE 'IC現行'")
                End If
                If strSystemCd = "32" Or strSystemCd = "33" Or strSystemCd = "50" Or strSystemCd = "51" _
                   Or strSystemCd = "52" Or strSystemCd = "53" Then
                    strSearch.Append(" AND [備考] LIKE 'ITS%'")
                End If
            End If
            If ipstrModelCd = "04" AndAlso Me.ddlModel.SelectedValue = "4" Then
                If strSystemCd = "30" Then
                    strSearch.Append(" AND [備考] LIKE 'IC新型'")
                End If
                If strSystemCd = "50" Or strSystemCd = "51" Or strSystemCd = "52" Or strSystemCd = "53" Then
                    strSearch.Append(" AND [備考] LIKE 'NVC%'")
                End If
            End If

            '検索機器がＨＤＤの場合ＨＤＤＮｏ、ＨＤＤ種別も条件とする
            If gvrRow.Cells(1).Text = "09" Then
                If gvrRow.Cells(2).Text = "&nbsp;" Then
                    strHDDNo = String.Empty
                Else
                    strHDDNo = gvrRow.Cells(2).Text
                End If
                If gvrRow.Cells(3).Text = "&nbsp;" Then
                    strHDDCls = String.Empty
                Else
                    strHDDCls = gvrRow.Cells(3).Text
                End If
                strSearch.Append(String.Format(" AND [HDD番号] = '{0}'", strHDDNo))
                strSearch.Append(String.Format(" AND [HDD種別] = '{0}'", strHDDCls))
            End If

            'REQLSTP002-005
            '移動日が古いものから取得し、持参最大数まで持参物品とする
            dtrData = dttData.Select(strSearch.ToString, "[移動日] ASC, [優先度] ASC, [シリアル番号] ASC, [連番] ASC")
            'dtrData = dttData.Select(strSearch.ToString, "[移動日] ASC, [優先度] ASC, [シリアル番号] ASC")
            'REQLSTP002-005 END

            If Not gvrRow.Cells(1).Text = strAppaCdBef Then
                intCount = 0
            Else
                If gvrRow.Cells(1).Text = "09" _
                    AndAlso (gvrRow.Cells(2).Text <> strHddNoBef _
                             Or (gvrRow.Cells(2).Text = strHddNoBef AndAlso gvrRow.Cells(3).Text <> strHddClsBef)) Then
                    intCount = 0
                End If
            End If
            intCount = 0
            Dim blnEndFlg As Boolean = False
            For Each dtrRow As DataRow In dtrData
                If intCount >= gvrRow.Cells(6).Text OrElse
                    intCount >= gvrRow.Cells(5).Text Then
                    '持参最大数または在庫全て移動完了
                    blnEndFlg = True
                    Exit For
                End If

                If dttData.Rows(dtrRow.Item("行数")).Item("持参").ToString = Nothing Then
                    '持参物品
                    dttData.Rows(dtrRow.Item("行数")).Item("持参") = "●"
                    intAllCount = intAllCount + 1
                    intCount = intCount + 1
                End If
            Next
            strAppaCdBef = gvrRow.Cells(1).Text
            strHddNoBef = gvrRow.Cells(2).Text
            strHddClsBef = gvrRow.Cells(3).Text

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dstData As DataSet = Nothing
            Dim strTboxId As String = ViewState(M_VIEW_TBOXID).ToString
            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                cmdDB = New SqlCommand("REQLSTP002_S1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strTboxId))
                End With

                'データ取得
                dstData = clsDataConnect.pfGet_DataSet(cmdDB)
                If blnEndFlg = True AndAlso ipstrModelCd = "04" AndAlso dstData.Tables(0).Rows(0).Item("システム分類").ToString <> "1" Then
                    Exit For
                End If
            End If
        Next

        '持参数チェック
        If intAllCount > 0 Then
            '一覧更新
            Me.grvList.DataSource = dttData
            Me.grvList.DataBind()

            '選択、拠点、機器型式非活性、印刷活性
            Me.btnSelect.Enabled = False
            Me.ddlStrage.Enabled = False
            Me.ddlModel.Enabled = False
            Me.btnPrint.Enabled = True
        Else
            '該当機器なし
            psMesBox(Me, "30017", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（拠点リスト）設定
    ''' </summary>
    ''' <remarks>一覧表示から拠点リスト作成</remarks>
    Private Sub msSetStrageList()
        Dim dttStrage As DataTable
        Dim dtrNewRow As DataRow
        Dim strStrage As String
        dttStrage = New DataTable
        dttStrage.Columns.Add("拠点コード")

        For Each dtrRow As GridViewRow In Me.grvList.Rows
            strStrage = CType(dtrRow.FindControl("現設置/保管コード"), TextBox).Text
            '重複していなければ拠点リストに追加
            If dttStrage.Select(String.Format("拠点コード = {0}", strStrage)).Count = 0 Then
                '重複なし
                dtrNewRow = dttStrage.NewRow()
                dtrNewRow("拠点コード") = CType(dtrRow.FindControl("現設置/保管コード"), TextBox).Text
                dttStrage.Rows.Add(dtrNewRow)
            End If
        Next

        '拠点有かつ機器型式有の場合のみ選択ボタンを活性化
        '選択前となるので拠点、機器型式活性、印刷非活性
        Me.ddlStrage.Enabled = True
        Me.ddlModel.Enabled = True
        Me.btnPrint.Enabled = False
        If dttStrage.Rows.Count > 0 And Me.ddlModel.SelectedValue <> String.Empty Then
            '該当拠点あり
            Me.btnSelect.Enabled = True     '選択ボタン活性
        Else
            '該当拠点なし
            Me.btnSelect.Enabled = False    '選択ボタン非活性
        End If

        Me.ddlStrage.DataTextField = "拠点コード"
        Me.ddlStrage.DataValueField = "拠点コード"

        Me.ddlStrage.DataSource = dttStrage
        Me.ddlStrage.DataBind()

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（システム別印刷マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlModel(ByVal ipstrSystemCD)

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット

        objStack = New StackFrame

        '機器種別がない場合リストを空にする
        If ipstrSystemCD = String.Empty Then
            Me.ddlModel.Items.Clear()
            Exit Sub
        End If

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL036", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ipstrSystemCD))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlModel.Items.Clear()
                Me.ddlModel.DataSource = dstOrders.Tables(0)
                Me.ddlModel.DataTextField = "システム別印刷"
                Me.ddlModel.DataValueField = "コード"
                Me.ddlModel.DataBind()
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "システム別印刷マスタ一覧取得")
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

    ''' <summary>
    ''' データ設定処理
    ''' </summary>
    ''' <param name="ipdstData">設定データ</param>
    ''' <remarks></remarks>
    Private Sub msSet_MainData(ByVal ipdstData As DataSet)
        objStack = New StackFrame

        Try
            '取得したデータを設定
            With ipdstData.Tables(0).Rows(0)
                '取得したデータを画面に設定
                lblTboxClassCDV.Text = .Item("システム").ToString
                lblVersionV.Text = .Item("バージョン").ToString
                lblMntCDV.Text = .Item("保守担当コード").ToString
                lblMntNmV.Text = .Item("保守担当").ToString
                lblUnfCDV.Text = .Item("統括コード").ToString
                lblUnfNmV.Text = .Item("統括名").ToString
                lblHallNameV.Text = .Item("ホール名").ToString
            End With
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' シリアル管理テーブル更新チェック
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ接続</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrSerialNo">シリアル番号</param>
    ''' <param name="ipstrSystemCd">システムコード</param>
    ''' <param name="ipstrAppabiyNm">機器分類</param>
    ''' <param name="ipstrAppaClassNm">機器種別</param>
    ''' <param name="ipstrAppaNm">型式/機器</param>
    ''' <param name="ipstrStrageCd"></param>
    ''' <returns>登録ＯＫ：0 移動済：-1</returns>
    ''' <remarks>REQLSTP002-005 引数(ipstrSeq)追加</remarks>
    Private Function mf_CheckSerial(ByVal ipconDB As SqlConnection,
                                    ByVal iptrnDB As SqlTransaction,
                                    ByVal ipstrSerialNo As String,
                                    ByVal ipstrSystemCd As String,
                                    ByVal ipstrAppabiyNm As String,
                                    ByVal ipstrAppaClassNm As String,
                                    ByVal ipstrAppaNm As String,
                                    ByVal ipstrStrageCd As String,
                                    ByVal ipstrSeq As String) As Integer
        Dim cmdDB As SqlCommand
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            cmdDB = New SqlCommand("REQLSTP002_S3", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("serial_no", SqlDbType.NVarChar, ipstrSerialNo))           'シリアル番号
                .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ipstrSystemCd))           'システムコード
                .Add(pfSet_Param("appadiv_nm", SqlDbType.NVarChar, ipstrAppabiyNm))         '機器分類
                .Add(pfSet_Param("appaclass_nm", SqlDbType.NVarChar, ipstrAppaClassNm))     '機器種別
                .Add(pfSet_Param("appa_nm", SqlDbType.NVarChar, ipstrAppaNm))               '型式/機器
                .Add(pfSet_Param("strage_cd", SqlDbType.NVarChar, ipstrStrageCd))           '現設置/保管コード
                'REQLSTP002-005 追加
                .Add(pfSet_Param("seq", SqlDbType.NVarChar, ipstrSeq))                      '連番
                'REQLSTP002-005 END
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mf_CheckSerial = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' シリアル管理テーブル更新
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ接続</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrSerialNo">シリアル番号</param>
    ''' <param name="ipstrSystemCd">システムコード</param>
    ''' <param name="ipstrAppabiyNm">機器分類</param>
    ''' <param name="ipstrAppaClassNm">機器種別</param>
    ''' <param name="ipstrAppaNm">型式/機器</param>
    ''' <param name="ipSerialcntlNo">管理番号（保守：保守管理番号/工事：工事依頼番号/その他：なし）</param>
    ''' <param name="ipSerialcntlCls">管理番号の区分(1：保守/2：工事/その他:なし)</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks>REQLSTP002-005 引数(ipstrSeq)追加</remarks>
    Private Function mf_UpdateSerial(ByVal ipconDB As SqlConnection,
                                     ByVal iptrnDB As SqlTransaction,
                                     ByVal ipstrSerialNo As String,
                                     ByVal ipstrSystemCd As String,
                                     ByVal ipstrAppabiyNm As String,
                                     ByVal ipstrAppaClassNm As String,
                                     ByVal ipstrAppaNm As String,
                                     ByVal ipSerialcntlNo As String,
                                     ByVal ipSerialcntlCls As String,
                                     ByVal ipstrSeq As String) As Integer
        Dim cmdDB As SqlCommand

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("REQLSTP002_U1", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("serial_no", SqlDbType.NVarChar, ipstrSerialNo))           'シリアル番号
                .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ipstrSystemCd))           'システムコード
                .Add(pfSet_Param("appadiv_nm", SqlDbType.NVarChar, ipstrAppabiyNm))         '機器分類
                .Add(pfSet_Param("appaclass_nm", SqlDbType.NVarChar, ipstrAppaClassNm))     '機器種別
                .Add(pfSet_Param("appa_nm", SqlDbType.NVarChar, ipstrAppaNm))               '型式/機器
                .Add(pfSet_Param("serialcntl_no", SqlDbType.NVarChar, ipSerialcntlNo))      '管理番号
                .Add(pfSet_Param("serialcntl_cls", SqlDbType.NVarChar, ipSerialcntlCls))    '管理番号の区分
                'REQLSTP002-005 追加
                .Add(pfSet_Param("seq", SqlDbType.NVarChar, ipstrSeq))                      '連番
                'REQLSTP002-005 END
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mf_UpdateSerial = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' シリアル履歴テーブル登録
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ接続</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrSerialNo">シリアル番号</param>
    ''' <param name="ipstrSystemCd">システムコード</param>
    ''' <param name="ipstrAppabiyNm">機器分類</param>
    ''' <param name="ipstrAppaClassNm">機器種別</param>
    ''' <param name="ipstrAppaNm">型式/機器</param>
    ''' <returns>ＳＱＬ実行結果</returns>
    ''' <remarks>REQLSTP002-005 引数(ipstrSeq)追加</remarks>
    Private Function mf_AddSerialHst(ByVal ipconDB As SqlConnection,
                                     ByVal iptrnDB As SqlTransaction,
                                     ByVal ipstrSerialNo As String,
                                     ByVal ipstrSystemCd As String,
                                     ByVal ipstrAppabiyNm As String,
                                     ByVal ipstrAppaClassNm As String,
                                     ByVal ipstrAppaNm As String,
                                     ByVal ipstrSeq As String) As Integer
        Dim cmdDB As SqlCommand

        objStack = New StackFrame

        Try
            cmdDB = New SqlCommand("REQLSTP002_U2", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("serial_no", SqlDbType.NVarChar, ipstrSerialNo))           'シリアル番号
                .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ipstrSystemCd))           'システムコード
                .Add(pfSet_Param("appadiv_nm", SqlDbType.NVarChar, ipstrAppabiyNm))         '機器分類
                .Add(pfSet_Param("appaclass_nm", SqlDbType.NVarChar, ipstrAppaClassNm))     '機器種別
                .Add(pfSet_Param("appa_nm", SqlDbType.NVarChar, ipstrAppaNm))               '型式/機器
                'REQLSTP002-005 追加
                .Add(pfSet_Param("seq", SqlDbType.NVarChar, ipstrSeq))                      '連番
                'REQLSTP002-005 END
                .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))         'ユーザーＩＤ
                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                           '戻り値
            End With

            'コマンドタイプ設定(ストアド)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.ExecuteNonQuery()

            'ストアド戻り値チェック
            mf_AddSerialHst = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' 別拠点名取得
    ''' </summary>
    ''' <param name="ipstrCode">営業所コード</param>
    ''' <param name="opdstData">営業所情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_OfficeNm(ByVal ipstrCode As String,
                                   ByRef opdstData As DataSet) As Boolean
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing
        strOKNG = Nothing
        '-----------------------------
        '2014/04/24 土岐　ここから
        '-----------------------------
        '移動中は除く
        If ipstrCode = "900" Then
            Return False
        End If
        '-----------------------------
        '2014/04/24 土岐　ここまで
        '-----------------------------
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL029", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    '-----------------------------
                    '2014/04/21 土岐　ここから
                    '-----------------------------
                    .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, "2,3"))
                    '-----------------------------
                    '2014/04/21 土岐　ここまで
                    '-----------------------------
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ipstrCode))
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                If opdstData.Tables(0).Rows.Count > 0 Then
                    '整合性OK
                    mfGet_OfficeNm = True
                Else
                    '整合性エラー
                    mfGet_OfficeNm = False
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
                mfGet_OfficeNm = False
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGet_OfficeNm = False
        End If
    End Function

    ''' <summary>
    ''' 印刷処理
    ''' </summary>
    ''' <param name="ipconDB">ＤＢ接続</param>
    ''' <param name="iptrnDB">トランザクション</param>
    ''' <param name="ipstrTboxID">ＴＢＯＸＩＤ</param>
    ''' <param name="ipstrStrageVal">拠点</param>
    ''' <param name="ipstrModelVal">選択機器型式</param>
    ''' <param name="ipdttData">印刷データ</param>
    ''' <remarks></remarks>
    Private Sub msPrint(ByVal ipconDB As SqlConnection,
                        ByVal iptrnDB As SqlTransaction,
                        ByVal ipstrTboxID As String,
                        ByVal ipstrStrageVal As String,
                        ByVal ipstrModelVal As String,
                        ByVal ipdttData As DataTable)
        Const TITLE = "タイトル名称"
        Const NL = "NL区分"
        Const SUPPORT = "サポートセンタ名"
        Const TBOXHALL = "TBOXIDホール名"
        Const TYPE = "種類"
        Const SEQ = "連番"
        Const NOTETEXT = "物品名"
        Const SYS = "システム"
        Const VER = "バージョン"
        Const SERIAL = "シリアル"
        'REQLSTP002-003
        Const SPCTEL = "サポートセンタTEL"
        'REQLSTP002-003 END

        Dim cmdDB As SqlCommand
        Dim dttData1 As DataTable
        Dim dttData2 As DataTable
        Dim dttPrint As DataTable
        Dim dtrNew As DataRow
        Dim dtrSerial() As DataRow
        Dim objRep As Object
        Dim strFileNm As String
        Dim intCount As Integer
        Dim dtrOldRow As DataRow = Nothing

        Dim strWhere As StringBuilder

        objStack = New StackFrame '2014/04/14 星野

        '2014/05/29 武
        Select Case ipstrModelVal
            Case "1"
                ipstrModelVal = "01"
            Case "3", "4"   '20140901稲葉要各員
                ipstrModelVal = "03"
        End Select

        Try

            cmdDB = New SqlCommand("REQLSTP002_S4", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxID))        'ＴＢＯＸＩＤ
                .Add(pfSet_Param("strage", SqlDbType.NVarChar, ipstrStrageVal))     '拠点
                .Add(pfSet_Param("model", SqlDbType.NVarChar, ipstrModelVal))       '機器型式
            End With

            'データ取得
            dttData1 = clsDataConnect.pfGet_DataSet(cmdDB).Tables(0)


            cmdDB = New SqlCommand("REQLSTP002_S5", ipconDB)
            cmdDB.Transaction = iptrnDB
            With cmdDB.Parameters
                .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxID))        'ＴＢＯＸＩＤ
                .Add(pfSet_Param("strage", SqlDbType.NVarChar, ipstrStrageVal))     '拠点
                .Add(pfSet_Param("model", SqlDbType.NVarChar, ipstrModelVal))       '機器型式
            End With

            'データ取得
            dttData2 = clsDataConnect.pfGet_DataSet(cmdDB).Tables(0)

            '印刷用テーブル設定
            dttPrint = New DataTable
            dttPrint.Columns.Add(TITLE)
            dttPrint.Columns.Add(NL)
            dttPrint.Columns.Add(SUPPORT)
            dttPrint.Columns.Add(TBOXHALL)
            dttPrint.Columns.Add(TYPE)
            dttPrint.Columns.Add(SEQ)
            dttPrint.Columns.Add(NOTETEXT)
            dttPrint.Columns.Add(SYS)
            dttPrint.Columns.Add(VER)
            dttPrint.Columns.Add(SERIAL)
            dttPrint.Columns.Add(SPCTEL) 'REQLSTP002-003
            '持参物品の行数分レコード追加、シリアル番号紐付け
            For Each dtrRow As DataRow In dttData1.Rows
                '-----------------------------
                '2014/05/02 土岐　ここから
                '-----------------------------
                strWhere = New StringBuilder
                strWhere.Append(String.Format("[持参]='●' AND [機器種別]='{0}'", dtrRow.Item("詳細機器種別コード")))
                '-----------------------------
                '2014/05/29 武　ここから
                '-----------------------------
                If dtrRow.Item("詳細機器種別コード").ToString = "09" Then 'HDD
                    'If dtrRow.Item("詳細機器種別コード") = "09" Then 'HDD
                    '-----------------------------
                    '2014/05/29 武　ここまで
                    '-----------------------------
                    strWhere.Append(String.Format(" AND [HDD番号] = '{0}'", dtrRow.Item("ＨＤＤＮｏ")))
                    strWhere.Append(String.Format(" AND [HDD種別] = '{0}'", dtrRow.Item("ＨＤＤ種別")))
                End If
                '持参物品取得
                dtrSerial = ipdttData.Select(strWhere.ToString, "[移動日] ASC, [シリアル番号] ASC")
                '-----------------------------
                '2014/05/02 土岐　ここまで
                '-----------------------------

                'REQLSTP002-004
                intCount = 0
                'If dtrOldRow IsNot Nothing Then
                '    If dtrOldRow.Item("詳細機器種別コード").ToString = dtrRow.Item("詳細機器種別コード").ToString _
                '            And dtrOldRow.Item("ＨＤＤＮｏ").ToString = dtrRow.Item("ＨＤＤＮｏ").ToString _
                '            And dtrOldRow.Item("HDD種別").ToString = dtrRow.Item("HDD種別").ToString Then
                '        intCount = dtrOldRow.Item("行数")
                '    End If
                'End If
                'REQLSTP002-004 END

                dtrOldRow = dtrRow

                '-----------------------------
                '2014/04/30 土岐　ここから
                '-----------------------------
                For zz As Integer = 0 To (dtrRow.Item("行数") - 1)
                    '-----------------------------
                    '2014/04/30 土岐　ここまで
                    '-----------------------------
                    dtrNew = dttPrint.NewRow
                    dtrNew.Item(TITLE) = dtrRow.Item(TITLE)
                    dtrNew.Item(NL) = dtrRow.Item(NL)
                    dtrNew.Item(SUPPORT) = dtrRow.Item(SUPPORT)
                    dtrNew.Item(TBOXHALL) = dtrRow.Item(TBOXHALL)
                    dtrNew.Item(TYPE) = dtrRow.Item(TYPE)
                    dtrNew.Item(SEQ) = dtrRow.Item(SEQ)
                    dtrNew.Item(NOTETEXT) = dtrRow.Item(NOTETEXT)
                    dtrNew.Item(SYS) = dtrRow.Item(SYS)
                    dtrNew.Item(VER) = dtrRow.Item(VER)
                    dtrNew.Item(SPCTEL) = Microsoft.VisualBasic.StrConv(dtrRow.Item(SPCTEL), Microsoft.VisualBasic.VbStrConv.Wide) 'REQLSTP002-003 電話番号は全角
                    'シリアル番号設定
                    If zz + intCount < dtrSerial.Count Then
                        dtrNew.Item(SERIAL) = dtrSerial(zz + intCount).Item("シリアル番号")
                        If dtrRow.Item("詳細機器種別コード").ToString = "01" Or dtrRow.Item("詳細機器種別コード").ToString = "09" Then
                            If dtrRow.Item("ＨＤＤＮｏ").ToString <> "F" Then
                                dtrNew.Item(VER) = dtrSerial(zz + intCount).Item("バージョン")
                            End If
                        End If
                        If dtrRow.Item("詳細機器種別コード").ToString = "04" Then
                            dtrNew.Item(SYS) = dtrSerial(zz + intCount).Item("システム")
                        End If

                    Else
                        dtrNew.Item(SERIAL) = String.Empty
                        'REQLSTP002-002
                        If dtrRow.Item("詳細機器種別コード").ToString = "01" Or dtrRow.Item("詳細機器種別コード").ToString = "09" Then
                            If dtrRow.Item("ＨＤＤＮｏ").ToString <> "F" Then
                                dtrNew.Item(VER) = String.Empty
                            End If
                        End If
                        If dtrRow.Item("詳細機器種別コード").ToString = "04" Then
                            dtrNew.Item(SYS) = String.Empty
                        End If
                        'REQLSTP002-002 END
                    End If
                    dttPrint.Rows.Add(dtrNew)

                    'REQLSTP002-004
                    '追加したシリアルは消していきましょう
                    If zz + intCount < dtrSerial.Count Then
                        For Each dr As DataRow In ipdttData.Rows
                            If dr.Item("シリアル番号").ToString = dtrSerial(zz + intCount).Item("シリアル番号") Then
                                dr.Delete()
                                ipdttData.AcceptChanges()
                                Exit For
                            End If
                        Next
                    End If
                    'REQLSTP002-004 END
                Next

                ''持参物品マスタ(M54).行数 < 持参物品セットマスタ(M95).持参最大数の場合発生(通常なし)
                'For zz As Integer = dtrRow.Item("行数") To dtrSerial.Count - 1
                '    dtrNew = dttPrint.NewRow
                '    dtrNew.Item(TITLE) = dtrRow.Item(TITLE)
                '    dtrNew.Item(NL) = dtrRow.Item(NL)
                '    dtrNew.Item(SUPPORT) = dtrRow.Item(SUPPORT)
                '    dtrNew.Item(TBOXHALL) = dtrRow.Item(TBOXHALL)
                '    dtrNew.Item(TYPE) = dtrRow.Item(TYPE)
                '    dtrNew.Item(SEQ) = dtrRow.Item(SEQ)
                '    dtrNew.Item(NOTETEXT) = dtrRow.Item(NOTETEXT)
                '    dtrNew.Item(SYS) = dtrRow.Item(SYS)
                '    dtrNew.Item(VER) = dtrRow.Item(VER)
                '    dtrNew.Item(SERIAL) = dtrSerial(zz).Item("シリアル番号")
                '    dttPrint.Rows.Add(dtrNew)
                'Next
                '-----------------------------
                '2014/05/02 土岐　ここから
                '-----------------------------
                strWhere.Clear()
                '-----------------------------
                '2014/05/02 土岐　ここまで
                '-----------------------------
            Next

            '保守ツール、手順書の行数分レコード追加
            If dttData2.Rows.Count > 0 Then
                For Each dtrRow As DataRow In dttData2.Rows
                    '-----------------------------
                    '2014/04/30 土岐　ここから
                    '-----------------------------
                    For zz As Integer = 0 To (dtrRow.Item("行数") - 1)
                        '-----------------------------
                        '2014/04/30 土岐　ここまで
                        '-----------------------------
                        dtrNew = dttPrint.NewRow
                        dtrNew.Item(TITLE) = dtrRow.Item(TITLE)
                        dtrNew.Item(NL) = dtrRow.Item(NL)
                        dtrNew.Item(SUPPORT) = dtrRow.Item(SUPPORT)
                        dtrNew.Item(TBOXHALL) = dtrRow.Item(TBOXHALL)
                        dtrNew.Item(TYPE) = dtrRow.Item(TYPE)
                        dtrNew.Item(SEQ) = dtrRow.Item(SEQ)
                        dtrNew.Item(NOTETEXT) = dtrRow.Item(NOTETEXT)
                        dtrNew.Item(SYS) = dtrRow.Item(SYS)
                        dtrNew.Item(VER) = dtrRow.Item(VER)
                        dtrNew.Item(SERIAL) = dtrRow.Item(SERIAL)
                        dtrNew.Item(SPCTEL) = Microsoft.VisualBasic.StrConv(dtrRow.Item(SPCTEL), Microsoft.VisualBasic.VbStrConv.Wide) 'REQLSTP002-003　電話番号は全角
                        dttPrint.Rows.Add(dtrNew)
                    Next
                Next
            End If

            '出力帳票設定
            Select Case ipstrModelVal
                '-----------------------------
                '2014/04/30 土岐　ここから
                '-----------------------------
                Case "03"   'プリンタ
                    '-----------------------------
                    '2014/04/30 土岐　ここまで
                    '-----------------------------
                    objRep = New ETCREP018
                    strFileNm = "ﾌﾟﾘﾝﾀ故障対応持参物品一覧"
                Case Else   'プリンタ以外
                    objRep = New ETCREP017
                    strFileNm = "制御部・HDD・ﾃﾞｨｽﾌﾟﾚｲ・操作盤故障対応持参物品一覧"

            End Select

            '印刷
            psPrintPDF(Me, objRep, dttPrint, strFileNm)

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
            Throw ex
        End Try
    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
