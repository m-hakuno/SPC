'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　貸玉数設定変換マスタ
'*　ＰＧＭＩＤ：　COMUPDMA0
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.12　：　星野
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports SQL_DBCLS_LIB

#End Region

Public Class COMUPDMA0

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
    Const DispCode As String = "COMUPDMA0"                  '画面ID
    Const MasterName As String = "貸玉数設定変換マスタ"     '画面名
    Const TableName As String = "MA0_LENDBALL"              'テーブル名

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive
    Dim objStack As StackFrame
    Dim strMode As String = Nothing

#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'ボタンアクション設定
        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf btn_Click              '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf btn_Click              '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf btn_Click              '削除
        AddHandler Master.ppBtnClear.Click, AddressOf btnClear_Click            'クリア

        If Not IsPostBack Then
            'プログラムＩＤ、画面名、件数設定
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"

            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            'グリッドの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'データ取得
            'msGet_Data()

            strMode = "Default"

        End If

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        Select Case strMode
            Case "Default"
                txtTboxCls.ppEnabled = True
                tftBbCls.ppEnabled = True
                tftLendCd.ppEnabled = True
                Master.ppBtnInsert.Enabled = True      '追加
                Master.ppBtnUpdate.Enabled = False     '更新
                Master.ppBtnDelete.Enabled = False     '削除
                Master.ppBtnClear.Enabled = True       'クリア
            Case "Select"
                txtTboxCls.ppEnabled = False
                tftBbCls.ppEnabled = False
                tftLendCd.ppEnabled = False
                Master.ppBtnInsert.Enabled = False     '追加
                Master.ppBtnUpdate.Enabled = True      '更新
                Master.ppBtnDelete.Enabled = True      '削除
                Master.ppBtnClear.Enabled = True       'クリア
        End Select

    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        '入力チェック
        msCheck("Search")

        'データ取得
        If (Page.IsValid) Then
            msGet_Data()
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        txtSTboxCls.ppText = String.Empty
        txtBbCls.ppText = String.Empty
        txtLendCd.ppText = String.Empty
        tftSPrice.ppFromText = String.Empty
        tftSPrice.ppToText = String.Empty

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 追加/更新/削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        '入力チェック
        If e.CommandName <> "DELETE" Then
            msCheck(e.CommandName)
        End If

        If (Page.IsValid) Then
            msEditData(e.CommandName)
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click()

        'ログ出力開始
        psLogStart(Me)

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

        txtTboxCls.ppText = String.Empty
        tftBbCls.ppText = String.Empty
        tftLendCd.ppText = String.Empty
        tftPrice.ppText = String.Empty

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    '============================================================================================================================
    '==   GRID関係
    '============================================================================================================================

    ''' <summary>
    ''' GRID RowCommand
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        Try
            If e.CommandName = "Select" Then

                'ログ出力開始
                psLogStart(Me)

                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

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
                arKey.Insert(0, CType(rowData.FindControl("ＴＢＯＸ種別"), TextBox).Text)
                arKey.Insert(1, CType(rowData.FindControl("ＢＢ種別コード"), TextBox).Text)
                arKey.Insert(2, CType(rowData.FindControl("貸玉数設定情報"), TextBox).Text)

                '★排他情報確認処理(更新画面へ遷移)
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

                '編集エリアに値を設定
                txtTboxCls.ppText = CType(rowData.FindControl("ＴＢＯＸ種別"), TextBox).Text
                tftBbCls.ppText = CType(rowData.FindControl("ＢＢ種別コード"), TextBox).Text
                tftLendCd.ppText = CType(rowData.FindControl("貸玉数設定情報"), TextBox).Text
                tftPrice.ppText = CType(rowData.FindControl("単価"), TextBox).Text

                strMode = "Select"
            End If

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        'ログ出力終了
        psLogEnd(Me)
    End Sub

#End Region

#Region "そのほかのプロシージャ"

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

        If Me.IsPostBack Then
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
                    .Add(pfSet_Param("tbox_cls", SqlDbType.NVarChar, txtSTboxCls.ppText.Trim))
                    .Add(pfSet_Param("bb_cls", SqlDbType.NVarChar, txtBbCls.ppText.Trim))
                    .Add(pfSet_Param("lend_cd", SqlDbType.NVarChar, txtLendCd.ppText.Trim))
                    .Add(pfSet_Param("price_from", SqlDbType.NVarChar, tftSPrice.ppFromText.Trim))
                    .Add(pfSet_Param("price_to", SqlDbType.NVarChar, tftSPrice.ppToText.Trim))
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    '上限オーバー
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0).Item("総件数").ToString, dstOrders.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                End If

                '取得したデータをリストに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
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

    ''' <summary>
    ''' 追加/更新/削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEditData(ByVal ipstrMode As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer
        Dim MesCode As String = String.Empty
        Dim procCls As String = String.Empty
        Dim strStored As String = String.Empty
        Dim dttGrid As New DataTable
        Dim drData As DataRow
        objStack = New StackFrame

        Select Case ipstrMode
            Case "INSERT"
                MesCode = "00003"
                procCls = "0"
                strStored = DispCode & "_U1"
            Case "UPDATE"
                MesCode = "00001"
                procCls = "1"
                strStored = DispCode & "_U1"
            Case "DELETE"
                MesCode = "00002"
                strStored = DispCode & "_D1"
        End Select

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '重複確認
                If ipstrMode = "INSERT" Then

                    cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("tbox_cls", SqlDbType.NVarChar, txtTboxCls.ppText.Trim))
                        .Add(pfSet_Param("bb_cls", SqlDbType.NVarChar, tftBbCls.ppText.Trim))
                        .Add(pfSet_Param("lend_cd", SqlDbType.NVarChar, tftLendCd.ppText.Trim))
                    End With

                    'リストデータ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    '既に存在している場合は処理終了
                    If dstOrders.Tables(0).Rows.Count > 0 Then
                        psMesBox(Me, "30019", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If
                End If

                cmdDB = New SqlCommand(strStored, conDB)
                Select Case ipstrMode
                    Case "INSERT", "UPDATE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("tbox_cls", SqlDbType.NVarChar, txtTboxCls.ppText.Trim))       'ＴＢＯＸ種別
                            .Add(pfSet_Param("bb_cls", SqlDbType.NVarChar, tftBbCls.ppText.Trim))           'ＢＢ種別コード
                            .Add(pfSet_Param("lend_cd", SqlDbType.NVarChar, tftLendCd.ppText.Trim))         '貸玉数設定情報
                            .Add(pfSet_Param("price", SqlDbType.NVarChar, tftPrice.ppText.Trim))            '単価
                            .Add(pfSet_Param("proc_cls", SqlDbType.NVarChar, procCls))                      '処理区分
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))             'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                               '戻り値
                        End With
                    Case "DELETE"
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("tbox_cls", SqlDbType.NVarChar, txtTboxCls.ppText.Trim))       'ＴＢＯＸ種別
                            .Add(pfSet_Param("bb_cls", SqlDbType.NVarChar, tftBbCls.ppText.Trim))           'ＢＢ種別コード
                            .Add(pfSet_Param("lend_cd", SqlDbType.NVarChar, tftLendCd.ppText.Trim))         '貸玉数設定情報
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                               '戻り値
                        End With
                End Select

                'データ登録/更新/削除
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn

                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        conTrn.Rollback()
                        psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                '追加、更新の場合は対象のレコードのみグリッドに表示
                If ipstrMode = "INSERT" OrElse ipstrMode = "UPDATE" Then
                    dttGrid.Columns.Add("ＴＢＯＸ種別")
                    dttGrid.Columns.Add("ＢＢ種別コード")
                    dttGrid.Columns.Add("貸玉数設定情報")
                    dttGrid.Columns.Add("単価")
                    drData = dttGrid.NewRow()
                    drData.Item("ＴＢＯＸ種別") = txtTboxCls.ppText.Trim
                    drData.Item("ＢＢ種別コード") = tftBbCls.ppText.Trim
                    drData.Item("貸玉数設定情報") = tftLendCd.ppText.Trim
                    drData.Item("単価") = tftPrice.ppText.Trim
                    dttGrid.Rows.Add(drData)
                Else
                    '削除の場合はグリッドから対象のレコードを削除
                    dttGrid = pfParse_DataTable(Me.grvList)
                    With dttGrid
                        For zz = 0 To .Rows.Count - 1
                            If txtTboxCls.ppText.Trim = .Rows(zz).Item("ＴＢＯＸ種別") AndAlso tftBbCls.ppText.Trim = .Rows(zz).Item("ＢＢ種別コード") AndAlso tftLendCd.ppText.Trim = .Rows(zz).Item("貸玉数設定情報") Then
                                .Rows(zz).Delete()
                                Exit For
                            End If
                        Next
                    End With
                End If

                'データをセット
                dttGrid.AcceptChanges()
                grvList.DataSource = dttGrid
                grvList.DataBind()

                Master.ppCount = dttGrid.Rows.Count

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)
                btnClear_Click()

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' 入力チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck(ByVal ipstrMode As String)

        Dim dec As Decimal
        Dim bln As Boolean = True

        Select Case ipstrMode
            Case "INSERT", "UPDATE"
                'ＴＢＯＸ種別
                If txtTboxCls.ppText.Trim = String.Empty Then
                    txtTboxCls.psSet_ErrorNo("5001", "ＴＢＯＸ種別")
                End If

                'ＢＢ種別コード
                If tftBbCls.ppText.Trim = String.Empty Then
                    tftBbCls.psSet_ErrorNo("5001", "ＢＢ種別コード")
                End If

                '貸玉数設定情報
                If tftLendCd.ppText.Trim = String.Empty Then
                    tftLendCd.psSet_ErrorNo("5001", "貸玉数設定情報")
                End If

                '単価
                If tftPrice.ppText.Trim <> String.Empty Then
                    If Decimal.TryParse(tftPrice.ppText.Trim, dec) = False Then
                        tftPrice.psSet_ErrorNo("4002", "単価")
                    End If
                Else
                    tftPrice.psSet_ErrorNo("5001", "単価")
                End If

            Case "Search"
                '単価
                If tftSPrice.ppFromText.Trim <> String.Empty Then
                    If Decimal.TryParse(tftSPrice.ppFromText.Trim, dec) = False Then
                        tftSPrice.psSet_ErrorNo("4002", "単価")
                        bln = False
                    End If
                End If

                If tftSPrice.ppToText.Trim <> String.Empty Then
                    If Decimal.TryParse(tftSPrice.ppToText.Trim, dec) = False Then
                        tftSPrice.psSet_ErrorNo("4002", "単価")
                        bln = False
                    End If
                End If

                If bln = True AndAlso tftSPrice.ppFromText.Trim <> String.Empty AndAlso tftSPrice.ppToText.Trim <> String.Empty Then
                    If CType(tftSPrice.ppFromText.Trim, Decimal) > CType(tftSPrice.ppToText.Trim, Decimal) Then
                        tftSPrice.psSet_ErrorNo("2001", "単価")
                    End If
                End If
        End Select

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
