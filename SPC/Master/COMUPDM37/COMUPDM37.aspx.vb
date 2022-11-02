'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　保守料金マスタ管理
'*　ＰＧＭＩＤ：　COMUPDM37
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.25　：　加賀
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM37-001     2015/05/28      稲葉       バグ修正（適用期間外のデータの日付部分を青くする処理を修正）
'COMUPDM37-002     2015/05/28      稲葉       バグ修正（登録／更新時の期間重複メッセージを修正）

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB

#End Region

Public Class COMUPDM37

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
    '画面情報
    Const DispCode As String = "COMUPDM37"                   '画面ID

    'マスタ情報
    Const MasterName As String = "保守料金マスタ"            'マスタ名
    Const TableName As String = "M37_MAINTE_RATE"            'テーブル名
    Const KeyName1 As String = "M37_WRK_CD"
    Const KeyName2 As String = "M37_TBOX_CLS"
    Const KeyName3 As String = "M37_FROM_DT"
    Const KeyName4 As String = "M37_END_DT"
    Const KeyName5 As String = "M37_MAINTE_FLG"

    'ソート情報
    'Const OrderByKey As String = " ORDER BY " & KeyName1 & "," & KeyName3 & ", M38_FROM_DT "

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim mclsDB As New ClsSQLSvrDB
    Dim ClsSQL As New ClsSQL
    Dim clsExc As New ClsCMExclusive
    Dim objStack As StackFrame
    Dim stb As New StringBuilder
    Dim dv As New DataView
    Dim dt As New DataTable
    Dim DispMode As String
    Dim SRCH As String = ""
    '適用期間用
    Dim RcdCase As String = ""
    Dim before As String = ""
    Dim after As String = ""
    Dim DateVal As String = ""
    Dim strwhere As String = ""
    Dim stbValMes As New StringBuilder

#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' PAGE Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, DispCode)

    End Sub
    ''' <summary>
    ''' PAGE Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        '初回実行
        If Not IsPostBack Then
            SRCH = "first"
            DispMode = "ADD"
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)
            DropDownList_Bind()

            'AutoPostBack
            ddlSYS.AutoPostBack = True
            ddlLAN_CLS.AutoPostBack = True

            '活性制御
            ddlLAN_CLS.Enabled = False
            txtLAN.Enabled = False
        End If

        'フォーカス設定
        ddlsSYS.Focus()

        'ボタンイベント設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf BtnSearchClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf BtnSearch_Click
        AddHandler Master.ppBtnClear.Click, AddressOf BtnClear_Click
        AddHandler Master.ppBtnInsert.Click, AddressOf BtnInsert_Click
        AddHandler Master.ppBtnUpdate.Click, AddressOf BtnUpdate_Click
        AddHandler Master.ppBtnDelete.Click, AddressOf BtnDelete_Click

        '編集ボタン　メッセージボックス
        'BtnUpd.OnClientClick = "return vb_MsgInf_OC('保守料金マスタを更新します。\n更新によって前後の期間も変更されます。\nよろしいですか？','I00006');"
        'BtnAdd.OnClientClick = "return vb_MsgInf_OC('保守料金マスタを追加します。\n追加によって前後の期間も変更されます。\nよろしいですか？','I00006');"
        'BtnDel.OnClientClick = "return vb_MsgInf_OC('保守料金マスタを削除します。\n削除によって料金設定の無い期間が発生します。\nよろしいですか？','I00006');"

    End Sub
    ''' <summary>
    ''' PAGE PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '閉じるボタンでEXIT
        If Master.ppCount = "close" Then
            Exit Sub
        End If

        'ボタンの使用制限
        If DispMode = String.Empty And Not ViewState("DispMode") Is Nothing Then
            DispMode = DirectCast(ViewState("DispMode"), String)
        End If
        Me.ViewState.Add("DispMode", DispMode)

        Select Case DispMode
            Case "ADD"
                '初期　追加のみ
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
                'ckbDEL.Enabled = False
            Case "UPD"
                '選択後　更新　削除
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = True
                Master.ppBtnDelete.Enabled = True
            Case Else
                '使用不可
                Master.ppBtnClear.Enabled = False
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
        End Select


        'GRIDの検索結果の保持
        Dim SearchFlg As Boolean = False    '検索判定用フラグ
        Dim DispMesBox As Boolean = True    'メッセージ表示の有無

        stb.Clear()
        stb.Append("        [M37_TBOX_CLS] AS TBOXCLS  ")
        stb.Append("       ,[M37_TBOX_CLS] + ' : ' + [M23_TBOXCLS_NM] AS TBOX種別 ")
        stb.Append("       ,CONVERT(NVARCHAR, [M37_FROM_DT], 111) AS 適用開始日 ")
        stb.Append("       ,CONVERT(NVARCHAR, [M37_END_DT], 111) AS 適用終了日 ")
        stb.Append("       ,[M37_MAINTE_FLG] AS fMAINTE ")
        stb.Append("       ,[M37_MAINTE_FLG] + ' : ' + T2.[M29_NAME]  AS 料金区分 ")
        stb.Append("       ,[M37_PRICE] AS 単価 ")
        'stb.Append(" 	   ,[M37_LAN_PRICE_CLS] + ':' + [M37_LAN_PRICE_NM] AS LAN ")
        stb.Append(" 	   ,[M37_LAN_PRICE_CLS] + ' : ' + [M37_LAN_PRICE_NM] AS LAN単価種別   ")
        stb.Append("       ,IIF([M37_LAN_PRICE_CLS] IS NULL, NULL, [M37_LAN_PRICE])  AS LAN単価  ")
        stb.Append("       ,IIF([M37_DELETE_FLG] = '0', '', '●') AS 削除 ")
        stb.Append("       ,FORMAT([M37_INSERT_DT], 'yyyy/MM/dd HH:mm:ss') AS 登録日時 ")
        stb.Append(" 	   ,[M23_SYSTEM_CD] AS SYS_CLS")
        stb.Append("   FROM [SPCDB].[dbo].[M37_MAINTE_RATE]   ")
        stb.Append("   LEFT JOIN [SPCDB].[dbo].[M23_TBOXCLASS]   ")
        stb.Append("   ON   [M37_TBOX_CLS] = [M23_TBOXCLS]   ")
        stb.Append("   LEFT JOIN  ")
        stb.Append("    (SELECT * FROM [SPCDB].[dbo].[M29_CLASS] WHERE [M29_CLASS_CD] = '0080') AS T2 ")
        stb.Append("   ON   [M37_MAINTE_FLG] = T2.[M29_CODE] ")


        Select Case SRCH
            Case ""         '非検索時
                Exit Sub
            Case "Edit"
                'SRCH = DirectCast(ViewState("SRCH"), String)
                SRCH = stb.ToString + strwhere
                DispMesBox = False
            Case "Del"      '削除時
                'SRCH = DirectCast(ViewState("SRCH"), String)
                '空データバインド
                Master.ppCount = "0"
                grvList.DataSource = New DataTable
                grvList.DataBind()
                Exit Sub
            Case "first"    '初回表示
                'grvList.DataSource = New DataTable
                'grvList.DataBind()
                'Exit Sub
                stb.Append(" WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 ")
                stb.Append(" AND M37_FROM_DT <= GETDATE() AND M37_END_DT >= GETDATE() ")
                SRCH = stb.ToString
            Case "clear"    '検索条件無し
                stb.Append(" WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 ")
                SRCH = stb.ToString
                SearchFlg = True
            Case Else       '検索時
                SRCH = stb.ToString + SRCH
                SearchFlg = True
        End Select

        'Me.ViewState.Add("SRCH", SRCH)


        '該当件数表示 & 表示件数上限設定
        Dim RecordCount As Integer = ClsSQL.GetRecordCount("SELECT" + SRCH) '該当件数
        Dim DispLimit As Integer = ClsSQL.GetDispLimit(DispCode, SearchFlg) '表示上限
        Master.ppCount = RecordCount                                        '該当件数表示

        Select Case ClsSQL.CheckLimitOver(DispCode, RecordCount, SearchFlg)
            Case -1         'エラー
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                Exit Sub
            Case 0          '0件
                SRCH = "SELECT" + SRCH
                If DispMesBox = True Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If
            Case 1          '上限オーバー
                SRCH = "SELECT TOP(" & DispLimit & ")" + SRCH
                If DispMesBox = True Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        RecordCount.ToString, DispLimit.ToString)
                End If
            Case 2          '上限以内
                SRCH = "SELECT" + SRCH
            Case 3          '上限0件
                SRCH = ""
                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        RecordCount.ToString, "0")
                grvList.DataSource = New DataTable
                grvList.DataBind()
                Exit Sub
        End Select

        SRCH = SRCH + " ORDER BY " & KeyName2 & ", " & KeyName5 & ", " & KeyName3 & " DESC"

        Try
            dt = ClsSQL.getDataSetTable(SRCH, "view")
        Catch ex As Exception
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'データの取得に失敗しました
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            Exit Sub
        End Try

        'GridViewにテーブルを表示
        grvList.DataSource = dv
        dv.Table = dt
        grvList.DataBind()

    End Sub

    '============================================================================================================================
    '==   データ編集
    '============================================================================================================================
    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnClear_Click()

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

        lblKey1.Text = ""
        lblKey2.Text = ""
        lblKey3.Text = ""
        lblKey4.Text = ""
        lblKey5.Text = ""
        lblSYS_CLS.Text = ""

        dtbStartDt.ppText = ""
        txtPrice.Text = ""
        txtLAN.Text = ""
        dtbENDDt.ppText = ""

        ddlSYS.Enabled = True
        ddlMAINTE.Enabled = True
        ddlMAINTE.SelectedIndex = 0
        ddlSYS.SelectedIndex = 0
        ddlLAN_CLS.SelectedIndex = 0

        ddlLAN_CLS.Enabled = False
        txtLAN.Enabled = False
        'ckbDEL.Checked = False

        DispMode = "ADD"

        'フォーカス設定
        ddlSYS.Focus()

        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 追加ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnInsert_Click()

        '検証
        If Me.IsValid = False OrElse stbValMes.ToString <> "" Then
            '整合性チェック
            If stbValMes.ToString <> "" Then
                stb.Clear()
                stb.Append("vb_MsgExc_O('" & stbValMes.ToString & "','整合性エラー');")
                Me.ClientScript.RegisterStartupScript(Me.GetType, "W30010", stb.ToString, True)
            End If

            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        Try
            'DB接続
            If mclsDB.pfDB_Connect(ClsSQL.CnStr) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'TRAN開始
            If mclsDB.pfDB_BeginTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力終了
                psLogEnd(Me)
                Exit Sub
            End If

            '追加コマンド
            stb.Clear()
            stb.Append(" INSERT INTO " & TableName & " ")
            stb.Append(" (  M37_WRK_CD ")
            stb.Append(" ,	M37_TBOX_CLS ")
            stb.Append(" ,	M37_FROM_DT ")
            stb.Append(" ,	M37_END_DT ")
            stb.Append(" ,	M37_MAINTE_FLG ")
            stb.Append(" ,	M37_PRICE ")
            stb.Append(" ,	M37_LAN_PRICE_CLS ")
            stb.Append(" ,	M37_LAN_PRICE_NM ")
            stb.Append(" ,	M37_LAN_PRICE ")
            stb.Append(" ,	" & TableName.Substring(0, 4) & "DELETE_FLG ")
            stb.Append(" ,	" & TableName.Substring(0, 4) & "INSERT_DT ")
            stb.Append(" ,	" & TableName.Substring(0, 4) & "INSERT_USR)              ")
            stb.Append(" SELECT ")
            stb.Append("  	'76' ")
            stb.Append(" ,	'" & ddlSYS.SelectedValue.ToString & "' ")
            stb.Append(" ,	'" & dtbStartDt.ppText & "' ")
            stb.Append(" ,	'" & dtbENDDt.ppText & "' ")
            stb.Append(" ,  '" & ddlMAINTE.SelectedValue.ToString & "' ")
            stb.Append(" ,  '" & txtPrice.Text.Replace(",", "") & "' ")
            'Select Case ddlSYS.SelectedValue.ToString
            Select If(ddlSYS.SelectedValue Is Nothing, "  ", ddlSYS.SelectedValue).Substring(0, 1)
                '   [T500][T700][T70M][T70R]
                'Case "01", "03", "04", "05"
                Case "0", "1"
                    If ddlLAN_CLS.SelectedIndex = 0 Then
                        stb.Append(" ,  NULL ")
                        stb.Append(" ,  NULL ")
                        stb.Append(" ,  0 ")
                    Else
                        stb.Append(" ,  '" & ddlLAN_CLS.SelectedValue.ToString & "' ")
                        stb.Append(" ,  '" & (ddlLAN_CLS.SelectedItem.Text + "    ").Substring(4).Trim & "' ")
                        stb.Append(" ,  '" & txtLAN.Text.Replace(",", "") & "' ")
                    End If
                Case Else
                    stb.Append(" ,  NULL ")
                    stb.Append(" ,  NULL ")
                    stb.Append(" ,  0 ")
            End Select

            stb.Append(" ,	'0' ")
            stb.Append(" ,	'" & DateTime.Now & "' ")
            stb.Append(" ,	'" & User.Identity.Name & "'  ")

            'SQL実行
            If mclsDB.pfDB_ExecuteSQL(stb.ToString) > 0 Then
            Else
                '失敗メッセージ表示
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            'コミット
            If mclsDB.pfDB_CommitTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            '完了メッセージ表示
            Select Case RcdCase
                Case "空期間"
                    stb.Clear()
                    stb.Append("vb_MsgExc_O('保守料金マスタの更新が完了しました。\n")
                    'stb.Append("システム：" & ddlSYS.SelectedItem.Text.Substring(4) & "、料金区分：" & ddlMAINTE.SelectedItem.Text.Substring(4) & "に")
                    'stb.Append("料金設定のない期間が存在します。データを見直してください。','I00001');")
                    stb.Append("料金設定のない期間が存在します。データを見直してください。\n")
                    stb.Append("対象　システム:" & ddlSYS.SelectedItem.Text.Substring(4) & "　料金区分:" & ddlMAINTE.SelectedItem.Text.Substring(4) & "','I00001');")
                    Me.ClientScript.RegisterStartupScript(Me.GetType, "mes", stb.ToString, True)
                Case Else
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)
            End Select

            stb.Clear()
            stb.Append("WHERE  ")
            stb.Append("      " & KeyName1 & " = '76' ")
            stb.Append(" AND  " & KeyName2 & " = '" & ddlSYS.SelectedValue.ToString & "' ")
            stb.Append(" AND  " & KeyName3 & " = '" & dtbStartDt.ppText & "' ")
            stb.Append(" AND  " & KeyName4 & " = '" & dtbENDDt.ppText & "' ")
            stb.Append(" AND  " & KeyName5 & " = '" & ddlMAINTE.SelectedValue.ToString & "' ")
            strwhere = stb.ToString

            BtnClear_Click()
            DispMode = "ADD"
            SRCH = "Edit"

            'フォーカス設定
            ddlsSYS.Focus()

            'ckbDEL.Enabled = False
        Catch ex As Exception
            '失敗メッセージ表示
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            mclsDB.psDB_Close()
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    ''' 更新ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnUpdate_Click()

        '検証
        If Me.IsValid = False OrElse stbValMes.ToString <> "" Then
            '整合性チェック
            If stbValMes.ToString <> "" Then
                stb.Clear()
                stb.Append("vb_MsgExc_O('" & stbValMes.ToString & "','整合性エラー');")
                Me.ClientScript.RegisterStartupScript(Me.GetType, "W30010", stb.ToString, True)
            End If

            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        '実行
        Try
            'DB接続
            If mclsDB.pfDB_Connect(ClsSQL.CnStr) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'TRAN開始
            If mclsDB.pfDB_BeginTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            stb.Clear()
            stb.Append("UPDATE " & TableName & " ")
            stb.Append("SET ")
            stb.Append(" M37_TBOX_CLS = '" & ddlSYS.SelectedValue.ToString & "' ")
            stb.Append(",M37_FROM_DT = '" & dtbStartDt.ppText & "'")
            stb.Append(",M37_END_DT = '" & dtbENDDt.ppText & "'")
            stb.Append(",M37_MAINTE_FLG = '" & ddlMAINTE.SelectedValue.ToString & "'")
            stb.Append(",M37_PRICE = '" & txtPrice.Text & "'")
            'Select Case ddlSYS.SelectedValue.ToString
            Select If(ddlSYS.SelectedValue Is Nothing, "  ", ddlSYS.SelectedValue).Substring(0, 1)
                '   [T500][T700][T70M][T70R]
                'Case "01", "03", "04", "05"
                Case "0", "1"
                    If ddlLAN_CLS.SelectedIndex = 0 Then
                        stb.Append(",M37_LAN_PRICE_CLS = NULL ")
                        stb.Append(",M37_LAN_PRICE_NM = NULL ")
                        stb.Append(",M37_LAN_PRICE = 0 ")
                    Else
                        stb.Append(",M37_LAN_PRICE_CLS = '" & ddlLAN_CLS.SelectedValue.ToString & "'")
                        stb.Append(",M37_LAN_PRICE_NM = '" & (ddlLAN_CLS.SelectedItem.Text + "    ").Substring(4).Trim & "'")
                        stb.Append(",M37_LAN_PRICE = '" & txtLAN.Text & "'")
                    End If
                Case Else
                    stb.Append(",M37_LAN_PRICE_CLS = NULL ")
                    stb.Append(",M37_LAN_PRICE_NM = NULL ")
                    stb.Append(",M37_LAN_PRICE = 0 ")
            End Select

            'stb.Append("," & TableName.Substring(0, 4) & "DELETE_FLG = '" & ClsSQL.GetDltCheckNum(ckbDEL) & "'")
            stb.Append("," & TableName.Substring(0, 4) & "UPDATE_DT = '" & DateTime.Now & "'")
            stb.Append("," & TableName.Substring(0, 4) & "UPDATE_USR = '" & User.Identity.Name & "'")
            stb.Append("WHERE  ")
            stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
            stb.Append("" & KeyName3 & " = '" & lblKey3.Text & "' AND ")
            stb.Append("" & KeyName4 & " = '" & lblKey4.Text & "' AND ")
            stb.Append("" & KeyName5 & " = '" & lblKey5.Text & "'  ")

            'SQL実行
            If mclsDB.pfDB_ExecuteSQL(stb.ToString) > 0 Then
            Else
                '失敗メッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            'コミット
            If mclsDB.pfDB_CommitTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            '完了メッセージ表示
            Select Case RcdCase
                Case "空期間"
                    stb.Clear()
                    stb.Append("vb_MsgExc_O('保守料金マスタの更新が完了しました。\n")
                    'stb.Append("システム：" & ddlSYS.SelectedItem.Text.Substring(4) & "、料金区分：" & ddlMAINTE.SelectedItem.Text.Substring(4) & "に")
                    'stb.Append("料金設定のない期間が存在します。データを見直してください。','I00001');")
                    stb.Append("料金設定のない期間が存在します。データを見直してください。\n")
                    stb.Append("対象　システム:" & ddlSYS.SelectedItem.Text.Substring(4) & "　料金区分:" & ddlMAINTE.SelectedItem.Text.Substring(4) & "','I00001');")
                    Me.ClientScript.RegisterStartupScript(Me.GetType, "mes", stb.ToString, True)
                Case Else
                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)
            End Select
            stb.Clear()
            stb.Append("WHERE  ")
            stb.Append("      " & KeyName1 & " = '76' ")
            stb.Append(" AND  " & KeyName2 & " = '" & ddlSYS.SelectedValue.ToString & "' ")
            stb.Append(" AND  " & KeyName3 & " = '" & dtbStartDt.ppText & "' ")
            stb.Append(" AND  " & KeyName4 & " = '" & dtbENDDt.ppText & "' ")
            stb.Append(" AND  " & KeyName5 & " = '" & ddlMAINTE.SelectedValue.ToString & "' ")
            strwhere = stb.ToString

            BtnClear_Click()
            DispMode = "ADD"
            SRCH = "Edit"

            'フォーカス設定
            ddlsSYS.Focus()

            'ckbDEL.Enabled = False
        Catch ex As Exception
            '失敗メッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            mclsDB.psDB_Close()
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    ''' 削除ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnDelete_Click()

        'ログ出力開始
        psLogStart(Me)

        '実行
        Try
            'DB接続
            If mclsDB.pfDB_Connect(ClsSQL.CnStr) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'TRAN開始
            If mclsDB.pfDB_BeginTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            stb.Append("DELETE FROM " & TableName & " ")
            stb.Append(" WHERE ")
            stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
            stb.Append("" & KeyName3 & " = '" & lblKey3.Text & "' AND ")
            stb.Append("" & KeyName4 & " = '" & lblKey4.Text & "' AND ")
            stb.Append("" & KeyName5 & " = '" & lblKey5.Text & "'  ")

            'SQL実行
            If mclsDB.pfDB_ExecuteSQL(stb.ToString) > 0 Then
            Else
                '失敗メッセージ表示
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            'コミット
            If mclsDB.pfDB_CommitTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            '前後期間取得
            stb.Clear()
            stb.Append(" DECLARE @OLD_fromDATE CHAR(10);  ")
            stb.Append(" DECLARE @TBOXCLS      CHAR(2);  ")
            stb.Append(" DECLARE @MAINTE       CHAR(1);  ")
            stb.Append("     SET @OLD_fromDATE = '" & lblKey3.Text & "'; ")
            stb.Append("     SET @TBOXCLS      = '" & lblKey2.Text & "'; ")
            stb.Append("     SET @MAINTE       = '" & lblKey5.Text & "'; ")
            stb.Append(" SELECT * FROM   (SELECT TOP 1  ")
            stb.Append(" 				        '1'			  AS ROW  ")
            stb.Append(" 				       ,[M37_FROM_DT] AS FROM_DT ")
            stb.Append(" 				       ,[M37_END_DT]  AS  END_DT ")
            stb.Append(" 				   FROM [SPCDB].[dbo].[M37_MAINTE_RATE] ")
            stb.Append(" 				   				 ")
            stb.Append(" 				  WHERE [M37_TBOX_CLS]   =  @TBOXCLS ")
            stb.Append(" 				    AND [M37_MAINTE_FLG] =  @MAINTE  ")
            stb.Append(" 				    AND [M37_FROM_DT]    <  @OLD_fromDATE ")
            stb.Append(" 				  ORDER BY [M37_FROM_DT] DESC ")
            stb.Append(" 				  )AS T3 ")
            stb.Append(" UNION ALL ")
            stb.Append(" SELECT * FROM   (SELECT TOP 1  ")
            stb.Append(" 				        '2'			  AS ROW  ")
            stb.Append(" 				       ,[M37_FROM_DT] AS FROM_DT ")
            stb.Append(" 				       ,[M37_END_DT]  AS  END_DT ")
            stb.Append(" 				   FROM [SPCDB].[dbo].[M37_MAINTE_RATE] ")
            stb.Append(" 				   				 ")
            stb.Append(" 				  WHERE [M37_TBOX_CLS]   =  @TBOXCLS ")
            stb.Append(" 				    AND [M37_MAINTE_FLG] =  @MAINTE  ")
            stb.Append(" 				    AND [M37_FROM_DT]    >  @OLD_fromDATE ")
            stb.Append(" 				  ORDER BY [M37_FROM_DT] ASC ")
            stb.Append(" 				  )AS T3 ")
            stb.Append(" UNION ALL ")
            stb.Append(" SELECT * FROM(SELECT 'EE' AS ROW,'9999/11/11' AS FROM_DT,'9999/11/11' AS END_DT) AS T_Empty1 ")
            stb.Append(" UNION ALL ")
            stb.Append(" SELECT * FROM(SELECT 'EE' AS ROW,'9999/11/11' AS FROM_DT,'9999/11/11' AS END_DT) AS T_Empty2 ")
            dt.Clear()
            dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

            Select Case dt(0)("ROW").ToString
                Case "1"
                    If dt(1)("ROW").ToString = "2" Then
                        RcdCase = "前後"
                    Else
                        RcdCase = "前"
                    End If
                Case "2"
                    RcdCase = "後"
                Case "EE"
                    RcdCase = "なし"
            End Select

            '完了メッセージ表示
            If RcdCase = "前後" Then
                stb.Clear()
                stb.Append("vb_MsgExc_O('保守料金マスタの更新が完了しました。\n")
                stb.Append("料金設定のない期間が存在します。データを見直してください。\n")
                stb.Append("対象　システム:" & ddlSYS.SelectedItem.Text.Substring(4) & "　料金区分:" & ddlMAINTE.SelectedItem.Text.Substring(4) & "','I00001');")
                Me.ClientScript.RegisterStartupScript(Me.GetType, "mes", stb.ToString, True)
            Else
                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)
            End If

            BtnClear_Click()
            DispMode = "ADD"
            SRCH = "Del"

            'フォーカス設定
            ddlsSYS.Focus()

            'ckbDEL.Enabled = False
        Catch ex As Exception
            '失敗メッセージ表示
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            mclsDB.psDB_Close()
        End Try

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
    Private Sub GridView1_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        Try
            If e.CommandName = "Sort" Then
                'SRCH = "sort"
                Exit Sub
            ElseIf e.CommandName = "Select" Then

                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))
                stb.Clear()
                stb.Append("SELECT M37_MAINTE_RATE.*")
                stb.Append("      ,M23_SYSTEM_CD")
                stb.Append("  FROM " & TableName & " ")
                stb.Append("  LEFT JOIN SPCDB.dbo.M23_TBOXCLASS")
                stb.Append("    ON M37_TBOX_CLS = M23_TBOXCLS")
                stb.Append(" WHERE ")
                stb.Append("" & KeyName1 & " = '76' AND ")
                stb.Append("" & KeyName2 & " = '" & CType(rowData.FindControl("TBOXCLS"), TextBox).Text & "' AND ")
                stb.Append("" & KeyName3 & " = '" & CType(rowData.FindControl("適用開始日"), TextBox).Text & "' AND ")
                stb.Append("" & KeyName4 & " = '" & CType(rowData.FindControl("適用終了日"), TextBox).Text & "' AND ")
                stb.Append("" & KeyName5 & " = '" & CType(rowData.FindControl("fMAINTE"), TextBox).Text & "'  ")
                dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

                'ログ出力開始
                psLogStart(Me)


                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                'Select Case ViewState(P_SESSION_TERMS)
                '    Case  ClsComVer.E_遷移条件.更新

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
                arKey.Insert(0, dt(0)(KeyName1).ToString)
                arKey.Insert(1, dt(0)(KeyName2).ToString)
                arKey.Insert(2, dt(0)(KeyName3).ToString)
                arKey.Insert(3, dt(0)(KeyName4).ToString)
                arKey.Insert(4, dt(0)(KeyName5).ToString)

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

                lblKey2.Text = dt(0)(KeyName2).ToString
                lblKey3.Text = dt(0)(KeyName3).ToString.Substring(0, 10)
                lblKey4.Text = dt(0)(KeyName4).ToString.Substring(0, 10)
                lblKey5.Text = dt(0)(KeyName5).ToString
                lblSYS_CLS.Text = dt(0)("M23_SYSTEM_CD").ToString

                ddlSYS.SelectedValue = dt(0)("M37_TBOX_CLS").ToString
                dtbStartDt.ppText = dt(0)("M37_FROM_DT").ToString.Substring(0, 10)
                dtbENDDt.ppText = dt(0)("M37_END_DT").ToString.Substring(0, 10)

                ddlMAINTE.SelectedValue = dt(0)("M37_MAINTE_FLG").ToString
                'txtPrice.Text = CInt(dt(0)("M37_PRICE")).ToString("#,0")
                txtPrice.Text = CInt(dt(0)("M37_PRICE")).ToString("#0")
                ddlLAN_CLS.SelectedValue = dt(0)("M37_LAN_PRICE_CLS").ToString
                txtLAN.Text = CInt(dt(0)("M37_LAN_PRICE")).ToString("#0")

                'Select Case dt(0)("M37_DELETE_FLG").ToString
                '    Case "0"
                '        ckbDEL.Enabled = False
                '        ckbDEL.Checked = False
                '        BtnDel.Enabled = True
                '    Case "1"
                '        ckbDEL.Enabled = True
                '        ckbDEL.Checked = True
                '        BtnDel.Enabled = False
                'End Select

                '活性制御
                'Select Case dt(0)("M37_TBOX_CLS").ToString
                Select Case dt(0)("M23_SYSTEM_CD").ToString
                    '   [T500][T700][T70M][T70R]
                    'Case "01", "03", "04", "05"
                    'ＩＤは選べる
                    Case "1"
                        ddlLAN_CLS.Enabled = True
                        'txtLAN.Enabled = False
                        'ＩＤ以外は選べない
                    Case Else
                        ddlLAN_CLS.Enabled = False
                        txtLAN.Enabled = False
                        ddlLAN_CLS.SelectedIndex = 0
                        txtLAN.Text = String.Empty
                End Select
                Select Case dt(0)("M37_LAN_PRICE_CLS").ToString
                    Case String.Empty
                        txtLAN.Enabled = False
                        txtLAN.Text = String.Empty
                    Case Else
                        txtLAN.Enabled = True
                End Select

                ddlSYS.Enabled = False
                ddlMAINTE.Enabled = False
            End If

            DispMode = "UPD"

            'フォーカス設定
            txtPrice.Focus()

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' GRID DataBound
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        '終了した期間を青字表示にする
        Dim dteFROM As Date = Nothing
        Dim dteTO As Date = Nothing
        Dim dteNow As Date = DateTime.Now.Date

        For Each rowData As GridViewRow In grvList.Rows
            dteFROM = Date.Parse(CType(rowData.FindControl("適用開始日"), TextBox).Text)
            dteTO = Date.Parse(CType(rowData.FindControl("適用終了日"), TextBox).Text)

            ''''COMUPDM37-001
            'If dteFROM <= dteNow And dteNow <= dteTO Then
            '    '期間内
            'ElseIf dteNow < dteFROM Then
            '    '後
            'Else
            '    '終了期間
            '    CType(rowData.FindControl("適用開始日"), TextBox).ForeColor = Drawing.Color.Blue
            '    CType(rowData.FindControl("適用終了日"), TextBox).ForeColor = Drawing.Color.Blue
            'End If
            If dteFROM <= dteNow And dteNow <= dteTO Then
                '期間内
            Else
                '期間外
                CType(rowData.FindControl("適用開始日"), TextBox).ForeColor = Drawing.Color.Blue
                CType(rowData.FindControl("適用終了日"), TextBox).ForeColor = Drawing.Color.Blue
            End If
            ''''COMUPDM37-001 END
        Next

        'LAN単価種別表示
        For Each rowData As GridViewRow In grvList.Rows
            Select Case CType(rowData.FindControl("LAN単価種別"), TextBox).Text.Trim
                Case ":"
                    CType(rowData.FindControl("LAN単価種別"), TextBox).Text = ""
            End Select
        Next


        '４桁以上の数字文字列でカンマ表示しない時使用
        'grvList_RowCommandのコメントアウトも解除

        ''頭の０外し
        'Dim intTRUN As Int16
        'For Each rowData As GridViewRow In grvList.Rows
        '    intTRUN = Int16.Parse(CType(rowData.FindControl("順番号"), TextBox).Text)

        '    CType(rowData.FindControl("順番号"), TextBox).Text = intTRUN.ToString
        'Next

    End Sub


    '============================================================================================================================
    '==   他
    '============================================================================================================================



    '============================================================================================================================
    '==   Validation
    '============================================================================================================================
#Region "                             --- Validation ---                                "

    ''' <summary>
    '''開始日付、終了日付検索用 
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub CstDayVal_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstDayVal.ServerValidate
        If (dtbStart.ppToText <> "" OrElse dtbStart.ppFromText <> "") AndAlso (dtbFin.ppToText <> "" OrElse dtbFin.ppFromText <> "") Then
            source.ErrorMessage = "開始日付、終了日付はどちらかのみ入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub

    ''' <summary>
    '''適用開始日
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Date_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Date.ServerValidate

        '値が日付になるか
        Dim result As Date
        If Date.TryParse(dtbStartDt.ppText, result) = False OrElse Date.TryParse(dtbENDDt.ppText, result) = False Then
            'ユーザーコントロールで検証するのでEXIT
            Exit Sub
        End If

        If ddlSYS.SelectedIndex = 0 OrElse ddlMAINTE.SelectedIndex = 0 Then
            Exit Sub
        End If

        '重複
        stb.Clear()
        stb.Append(" SELECT  COUNT([M37_TBOX_CLS]) ")
        stb.Append("   FROM  [SPCDB].[dbo].[M37_MAINTE_RATE] ")
        stb.Append("WHERE  ")
        stb.Append(" " & KeyName2 & " =  '" & ddlSYS.SelectedValue & "' AND ")
        stb.Append(" " & KeyName5 & " =  '" & ddlMAINTE.SelectedValue & "'  ")

        stb.Append("   AND  (     [M37_FROM_DT] BETWEEN '" & dtbStartDt.ppText & "' AND '" & dtbENDDt.ppText & "'  ")
        'stb.Append(" 		 OR   [M37_END_DT]  BETWEEN '" & dtbStartDt.ppText & "' AND '" & dtbENDDt.ppText & "' ) ")
        stb.Append(" 		 OR   [M37_END_DT]  BETWEEN '" & dtbStartDt.ppText & "' AND '" & dtbENDDt.ppText & "'  ")
        stb.Append(" 		 OR ( [M37_FROM_DT] <= '" & dtbStartDt.ppText & "' AND '" & dtbENDDt.ppText & "' <= [M37_END_DT] ) ) ")
        stb.Append("   AND  NOT (    [M37_TBOX_CLS]   = '" & lblKey2.Text & "' ")
        stb.Append(" 	     	 AND [M37_FROM_DT]    = '" & lblKey3.Text & "' ")
        stb.Append(" 		 	 AND [M37_END_DT]     = '" & lblKey4.Text & "'	  ")
        stb.Append(" 	     	 AND [M37_MAINTE_FLG] = '" & lblKey5.Text & "' )  ")
        If ClsSQL.GetRecord(stb.ToString) > 0 Then

            ''''COMUPDM37-002
            '重複エラー
            'stb.Clear()
            'stb.Append("システム " & ddlSYS.SelectedItem.Text & " ")
            'stb.Append("料金区分 " & ddlMAINTE.SelectedItem.Text & " ")
            'stb.Append("に重複する期間が登録されています。")
            stb.Clear()
            stb.Append(ddlSYS.SelectedItem.Text.Replace(ddlSYS.SelectedValue, String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty) & " の")
            stb.Append(ddlMAINTE.SelectedItem.Text.Replace(ddlMAINTE.SelectedValue, String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty) & " ")
            stb.Append("に期間が重複するデータが登録されています。")
            ''''COMUPDM37-002 END

            'source.ErrorMessage = stb.ToString
            'source.Text = "整合性エラー"
            'args.IsValid = False
            'RcdCase = "重複"
            stbValMes.Append("・" & stb.ToString & "\n")
            Exit Sub
        End If

        '変更先の前後のレコード取得
        stb.Clear()
        stb.Append(" DECLARE @OLD_DATE DATE;  ")
        stb.Append(" DECLARE @DATE DATE;  ")
        stb.Append(" DECLARE @TBOXCLS CHAR(2);  ")
        stb.Append(" DECLARE @MAINTE CHAR(1);   ")
        stb.Append("  ")
        stb.Append(" SET @OLD_DATE = '" & lblKey3.Text & "' ")
        stb.Append(" SET @DATE = '" & dtbStartDt.ppText & "'; ")
        stb.Append(" SET @TBOXCLS  = '" & ddlSYS.SelectedValue.ToString & "'; ")
        stb.Append(" SET @MAINTE  = '" & ddlMAINTE.SelectedValue.ToString & "'; ")
        stb.Append("  ")
        stb.Append(" SELECT * FROM ( ")
        stb.Append("   SELECT TOP 1  ")
        stb.Append("        '1' AS ROW  ")
        stb.Append("       ,[M37_FROM_DT] AS FROM_DT ")
        stb.Append("       ,[M37_END_DT]  AS  END_DT ")
        stb.Append("   FROM [SPCDB].[dbo].[M37_MAINTE_RATE] ")
        stb.Append("  ")
        stb.Append("   WHERE [M37_TBOX_CLS] = @TBOXCLS ")
        stb.Append("     AND [M37_MAINTE_FLG] = @MAINTE  ")
        stb.Append("     AND [M37_FROM_DT] < @DATE ")
        stb.Append(" 	 AND [M37_FROM_DT] <> @OLD_DATE ")
        stb.Append("   ORDER BY [M37_FROM_DT] DESC ) AS T1 ")
        stb.Append("    ")
        stb.Append(" UNION ALL ")
        stb.Append("    ")
        stb.Append(" SELECT * FROM ( ")
        stb.Append("   SELECT TOP 1  ")
        stb.Append("        '2' AS ROW ")
        stb.Append("       ,[M37_FROM_DT] AS FROM_DT ")
        stb.Append("       ,[M37_END_DT]  AS  END_DT ")
        stb.Append("   FROM [SPCDB].[dbo].[M37_MAINTE_RATE] ")
        stb.Append("  ")
        stb.Append("   WHERE [M37_TBOX_CLS] = @TBOXCLS ")
        stb.Append("     AND [M37_MAINTE_FLG] = @MAINTE  ")
        stb.Append("     AND [M37_FROM_DT] > @DATE  ")
        stb.Append(" 	 AND [M37_FROM_DT] <> @OLD_DATE ")
        stb.Append("   ORDER BY [M37_FROM_DT] ASC ) AS T2 ")
        stb.Append("  ")
        stb.Append(" UNION ALL ")
        stb.Append("  ")
        stb.Append("   SELECT * FROM(SELECT 'EE' AS ROW,'9999/11/11' AS FROM_DT,'9999/11/11' AS END_DT) AS T_Empty ")

        dt.Clear()
        dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

        Select Case dt(0)("ROW").ToString
            Case "1"
                If dt(1)("ROW").ToString = "2" Then
                    '変更先に前後の期間が存在する
                    '前期間の終了日付の変更と自身の終了日付の変更
                    RcdCase = "前後"
                    before = dt(0)("FROM_DT").ToString.Substring(0, 10) + dt(0)("END_DT").ToString.Substring(0, 10)
                    after = dt(1)("FROM_DT").ToString.Substring(0, 10) + dt(1)("END_DT").ToString.Substring(0, 10)
                Else
                    '変更先に前の期間のみ存在する
                    RcdCase = "前"
                    before = dt(0)("FROM_DT").ToString.Substring(0, 10) + dt(0)("END_DT").ToString.Substring(0, 10)
                End If
            Case "2"
                '変更先に後の期間のみ存在する
                RcdCase = "後"
                after = dt(0)("FROM_DT").ToString.Substring(0, 10) + dt(0)("END_DT").ToString.Substring(0, 10)
            Case "EE"
                '変更先に前後の期間が存在しない
                RcdCase = "なし"
        End Select

        '空白期間有無チェック
        Select Case RcdCase
            Case "前後"
                If CType(before.Substring(10), Date) = CType(dtbStartDt.ppText, Date).AddDays(-1) _
                   AndAlso CType(dtbENDDt.ppText, Date).AddDays(1) = CType(after.Substring(0, 10), Date) Then
                    'OK
                Else
                    RcdCase = "空期間"
                    Exit Sub
                End If
            Case "前"
                If CType(before.Substring(10), Date) = CType(dtbStartDt.ppText, Date).AddDays(-1) Then
                    'OK
                Else
                    RcdCase = "空期間"
                    Exit Sub
                End If
            Case "後"
                If CType(dtbENDDt.ppText, Date).AddDays(1) = CType(after.Substring(0, 10), Date) Then
                    'OK
                Else
                    RcdCase = "空期間"
                    Exit Sub
                End If
        End Select

        '更新の時
        If lblKey3.Text = "" Then
        Else
            '更新前の前後期間取得
            stb.Clear()
            stb.Append(" DECLARE @OLD_fromDATE DATE;  ")
            stb.Append(" DECLARE @TBOXCLS      CHAR(2);  ")
            stb.Append(" DECLARE @MAINTE       CHAR(1);  ")
            stb.Append("     SET @OLD_fromDATE = '" & lblKey3.Text & "'; ")
            stb.Append("     SET @TBOXCLS      = '" & lblKey2.Text & "'; ")
            stb.Append("     SET @MAINTE       = '" & lblKey5.Text & "'; ")
            stb.Append(" SELECT * FROM   (SELECT TOP 1  ")
            stb.Append(" 				        '1'			  AS ROW  ")
            stb.Append(" 				       ,[M37_FROM_DT] AS FROM_DT ")
            stb.Append(" 				       ,[M37_END_DT]  AS  END_DT ")
            stb.Append(" 				   FROM [SPCDB].[dbo].[M37_MAINTE_RATE] ")
            stb.Append(" 				   				 ")
            stb.Append(" 				  WHERE [M37_TBOX_CLS]   =  @TBOXCLS ")
            stb.Append(" 				    AND [M37_MAINTE_FLG] =  @MAINTE  ")
            stb.Append(" 				    AND [M37_FROM_DT]    <  @OLD_fromDATE ")
            stb.Append(" 				  ORDER BY [M37_FROM_DT] DESC ")
            stb.Append(" 				  )AS T3 ")
            stb.Append(" UNION ALL ")
            stb.Append(" SELECT * FROM   (SELECT TOP 1  ")
            stb.Append(" 				        '2'			  AS ROW  ")
            stb.Append(" 				       ,[M37_FROM_DT] AS FROM_DT ")
            stb.Append(" 				       ,[M37_END_DT]  AS  END_DT ")
            stb.Append(" 				   FROM [SPCDB].[dbo].[M37_MAINTE_RATE] ")
            stb.Append(" 				   				 ")
            stb.Append(" 				  WHERE [M37_TBOX_CLS]   =  @TBOXCLS ")
            stb.Append(" 				    AND [M37_MAINTE_FLG] =  @MAINTE  ")
            stb.Append(" 				    AND [M37_FROM_DT]    >  @OLD_fromDATE ")
            stb.Append(" 				  ORDER BY [M37_FROM_DT] ASC ")
            stb.Append(" 				  )AS T3 ")
            stb.Append(" UNION ALL ")
            stb.Append(" SELECT * FROM(SELECT 'EE' AS ROW,'9999/11/11' AS FROM_DT,'9999/11/11' AS END_DT) AS T_Empty1 ")
            stb.Append(" UNION ALL ")
            stb.Append(" SELECT * FROM(SELECT 'EE' AS ROW,'9999/11/11' AS FROM_DT,'9999/11/11' AS END_DT) AS T_Empty2 ")
            dt.Clear()
            dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

            Select Case RcdCase
                Case "前後"
                    If CType(before.Substring(0, 10), Date) = CType((dt(0)("FROM_DT").ToString + "0123456789").Substring(0, 10), Date) _
                    AndAlso CType(after.Substring(0, 10), Date) = CType((dt(1)("FROM_DT").ToString + "0123456789").Substring(0, 10), Date) Then
                        'OK
                    Else
                        RcdCase = "空期間"
                        Exit Sub
                    End If
                Case "前"
                    If CType(before.Substring(0, 10), Date) = CType((dt(0)("FROM_DT").ToString + "0123456789").Substring(0, 10), Date) Then
                        'OK
                    Else
                        RcdCase = "空期間"
                        Exit Sub
                    End If
                Case "後"
                    If CType(after.Substring(0, 10), Date) = CType((dt(0)("FROM_DT").ToString + "0123456789").Substring(0, 10), Date) Then
                        'OK
                    Else
                        RcdCase = "空期間"
                        Exit Sub
                    End If
            End Select


        End If

    End Sub

    ''' <summary>
    '''適用終了日
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_End_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_End.ServerValidate

        '値が日付になるか
        Dim result As Date
        If Date.TryParse(dtbStartDt.ppText, result) = False OrElse Date.TryParse(dtbENDDt.ppText, result) = False Then
            'ユーザーコントロールで検証するのでEXIT
            Exit Sub
        End If
        If Date.TryParse(dtbStartDt.ppText, result) = False Then
            '下の比較でエラーになるのでEXIT
            Exit Sub
        End If

        Select Case RcdCase
            Case "重複"
                '重複エラー
                'stb.Clear()
                'stb.Append("システム:" & ddlSYS.SelectedItem.Text & " ")
                'stb.Append("料金区分:" & ddlMAINTE.SelectedItem.Text & " ")
                'stb.Append("に重複する期間が登録されています。")
                source.ErrorMessage = ""
                source.Text = "整合性エラー"
                args.IsValid = False
                RcdCase = "重複"
                Exit Sub
        End Select


        ''変更先が既存の1レコード期間内に含まれる場合
        'If DateVal = "間" Then
        '    source.ErrorMessage = "適用期間が既存の期間に含まれています。"
        '    source.Text = "日付エラー"
        '    args.IsValid = False
        '    Exit Sub
        'End If

        ''期間内に含まれるレコードが存在しないか
        'If DateVal = "中" Then
        '    source.ErrorMessage = "期間内に別のレコードが存在します。"
        '    source.Text = "整合性エラー"
        '    args.IsValid = False
        '    Exit Sub
        'End If

        '開始日との整合性
        If Date.Parse(dtbStartDt.ppText) > Date.Parse(dtbENDDt.ppText) Then
            source.ErrorMessage = "適用終了日は適用開始日以降の日付で入力してください。"
            source.Text = "日付エラー"
            args.IsValid = False
            Exit Sub
        End If



        ''重複チェック
        'stb.Clear()
        'stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
        'stb.Append("WHERE  ")
        'stb.Append("" & KeyName2 & " = '" & ddlSYS.SelectedValue.ToString & "' AND ")
        'stb.Append("" & KeyName5 & " = '" & ddlMAINTE.SelectedValue.ToString & "' AND ")
        'stb.Append("" & KeyName4 & " = '" & dtbENDDt.ppText & "' AND ")
        'stb.Append("" & KeyName3 & " = '" & dtbStartDt.ppText & "' ")

        ''変更・追加する場合
        'If ClsSQL.GetRecord(stb.ToString) = -1 Then
        '    'エラー
        'ElseIf (lblKey2.Text <> ddlSYS.SelectedValue.ToString Or lblKey3.Text <> dtbStartDt.ppText _
        '        Or lblKey4.Text <> dtbENDDt.ppText Or lblKey5.Text <> ddlMAINTE.SelectedValue.ToString) _
        '    And ClsSQL.GetRecord(stb.ToString) <> 0 Then

        '    stb.Clear()
        '    stb.Append("TBOX種別コード:" & ddlSYS.SelectedValue.ToString & " ")
        '    stb.Append("料金区分:" & ddlMAINTE.SelectedItem.Text & " ")
        '    stb.Append("に　同一期間： " & dtbStartDt.ppText & "　～　" & dtbENDDt.ppText & " が既に登録されています。")

        '    source.ErrorMessage = stb.ToString
        '    source.Text = "整合性エラー"
        '    args.IsValid = False
        '    Exit Sub
        'End If


    End Sub

    ''' <summary>
    ''' 料金区分
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Mainte_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Mainte.ServerValidate

        Const Name As String = "料金区分"

        If ddlMAINTE.SelectedIndex = 0 Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

        ''重複チェック
        'stb.Clear()
        'stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
        'stb.Append("WHERE  ")
        'stb.Append("" & KeyName2 & " = '" & ddlSYS.SelectedValue.ToString & "' AND ")
        'stb.Append("" & KeyName3 & " = '" & dtbStartDt.ppText & "' AND ")
        'stb.Append("" & KeyName4 & " = '" & dtbENDDt.ppText & "' AND ")
        'stb.Append("" & KeyName5 & " = '" & ddlMAINTE.SelectedValue.ToString & "'  ")

        ''変更・追加する場合
        'If ClsSQL.GetRecord(stb.ToString) = -1 Then
        '    'エラー
        'ElseIf (lblKey2.Text <> ddlSYS.SelectedValue.ToString Or _
        '        lblKey3.Text <> dtbStartDt.ppText Or lblKey4.Text <> dtbENDDt.ppText _
        '    Or lblKey5.Text <> ddlMAINTE.SelectedValue.ToString) And ClsSQL.GetRecord(stb.ToString) <> 0 Then

        '    stb.Clear()
        '    stb.Append("TBOX種別コード:" & ddlSYS.SelectedValue.ToString & " ")
        '    stb.Append("適用開始日:" & dtbStartDt.ppText & " ")
        '    stb.Append("適用終了日:" & dtbENDDt.ppText & " ")
        '    stb.Append("に　料金区分： " & ddlLAN_CLS.SelectedItem.Text & " は既に登録されています。")

        '    source.ErrorMessage = stb.ToString
        '    source.Text = "整合性エラー"
        '    args.IsValid = False
        'End If

    End Sub

    ''' <summary>
    ''' 単価
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Price_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Price.ServerValidate

        Dim tb As TextBox = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        Const Name As String = "単価"

        If tb.Enabled = True Then
            If tb.Text = String.Empty Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If
        Else
            Exit Sub
        End If

        If Regex.IsMatch(tb.Text, "^[0-9]{1}$|^[1-9]{1}[0-9]{0,7}$") Then
        Else
            source.ErrorMessage = "" & Name & "は 8 桁以内の数値で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub
    ''' <summary>
    ''' LAN単価
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_LAN_Price_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_LAN_Price.ServerValidate

        Dim tb As TextBox = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)
        Const Name As String = "LAN単価"

        'LAN単価種別選択時のみ検証
        If ddlLAN_CLS.SelectedIndex <> 0 Then
            If tb.Text = String.Empty Then
                source.ErrorMessage = "" & Name & "に値が設定されていません。"
                source.Text = "未入力エラー"
                args.IsValid = False
                Exit Sub
            End If

            If Regex.IsMatch(tb.Text, "^[0-9]{1}$|^[1-9]{1}[0-9]{0,7}$") Then
            Else
                source.ErrorMessage = "" & Name & "は 8 桁以内の数値で入力して下さい。"
                source.Text = "形式エラー"
                args.IsValid = False
                Exit Sub
            End If
        End If

    End Sub
#End Region


#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Sub BtnSearchClear_Click()
        'ログ出力開始


        psLogEnd(Me)

        ddlsSYS.SelectedIndex = 0
        dtbStart.ppFromText = ""
        dtbStart.ppToText = ""
        dtbFin.ppFromText = ""
        dtbFin.ppToText = ""

        ddlsMAINTE.SelectedIndex = 0
        ddlsLAN_CLS.SelectedIndex = 0
        'ckbsDEL.Checked = False

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Sub BtnSearch_Click()

        Page.Validate("search")
        If Me.IsValid = False Then
            Exit Sub
        End If

        If ddlsSYS.SelectedIndex = 0 And ddlsMAINTE.SelectedIndex = 0 And ddlsLAN_CLS.SelectedIndex = 0 And _
        dtbStart.ppFromText + dtbStart.ppToText = "" And dtbFin.ppFromText + dtbFin.ppToText = "" Then
            SRCH = "clear"
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)


        stb.Clear()
        stb.Append("  WHERE")

        'If ddlday.ppDropDownList.SelectedIndex = 1 Then '含む
        '    stb.Append(" AND M37_FROM_DT <= GETDATE() AND M37_END_DT >= GETDATE() ")
        'ElseIf ddlday.ppDropDownList.SelectedIndex = 2 Then '含まない
        '    stb.Append(" AND (M37_END_DT < GETDATE() OR M37_FROM_DT > GETDATE()) ")
        'End If

        If ddlsSYS.SelectedIndex <> 0 Then
            stb.Append(" AND M37_TBOX_CLS = '" & ddlsSYS.SelectedValue.ToString & "' ")
        End If
        If ddlsLAN_CLS.SelectedIndex <> 0 Then
            stb.Append(" AND M37_LAN_PRICE_CLS = '" & ddlsLAN_CLS.SelectedValue.ToString & "' ")
        End If
        If ddlsMAINTE.SelectedIndex <> 0 Then
            stb.Append(" AND M37_MAINTE_FLG = '" & ddlsMAINTE.SelectedValue.ToString & "' ")
        End If

        '期間検索
        '開始終了日付ともに入力時はエラー(バリデーションで弾く)

        Dim result As Date
        '開始日付検索
        If dtbStart.ppFromText = "" Then
            If dtbStart.ppToText = "" Then
            Else '-To
                If Date.TryParse(dtbStart.ppToText, result) = True Then
                    stb.Append(" AND M37_FROM_DT <= '" & dtbStart.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            End If
        Else
            If dtbStart.ppToText = "" Then
                ' From-
                If Date.TryParse(dtbStart.ppFromText, result) = True Then
                    stb.Append(" AND '" & dtbStart.ppFromText & "' <= M37_FROM_DT")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            Else 'From TO
                If Date.TryParse(dtbStart.ppFromText, result) = True And Date.TryParse(dtbStart.ppToText, result) = True Then
                    stb.Append(" AND '" & dtbStart.ppFromText & "' <= M37_FROM_DT AND  M37_FROM_DT <= '" & dtbStart.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If
            End If
        End If

        '終了日付検索
        If dtbFin.ppFromText = "" Then
            If dtbFin.ppToText = "" Then
            Else '-To
                If Date.TryParse(dtbFin.ppToText, result) = True Then
                    stb.Append(" AND M37_END_DT <= '" & dtbFin.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            End If
        Else
            If dtbFin.ppToText = "" Then
                ' From-
                If Date.TryParse(dtbFin.ppFromText, result) = True Then
                    stb.Append(" AND '" & dtbFin.ppFromText & "' <= M37_END_DT")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            Else 'From TO
                If Date.TryParse(dtbFin.ppFromText, result) = True And Date.TryParse(dtbFin.ppToText, result) = True Then
                    stb.Append(" AND '" & dtbFin.ppFromText & "' <= M37_END_DT AND  M37_END_DT <= '" & dtbFin.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If
            End If
        End If


        stb.Replace("WHERE AND", "WHERE")
        SRCH = stb.ToString

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    '''ドロップダウンのデータ取得＆バインド
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DropDownList_Bind()

        'TBOX機種コード
        stb.Clear()
        stb.Append("SELECT M23_TBOXCLS")
        stb.Append("      ,(M23_TBOXCLS + ' : ' + M23_TBOXCLS_NM) AS CLSNM")
        stb.Append("  FROM M23_TBOXCLASS")
        stb.Append(" WHERE M23_DELETE_FLG = 0")
        Dim SYSCD As New DataTable
        SYSCD = ClsSQL.getDataSetTable(stb.ToString, "SYSCD")

        ddlsSYS.DataSource = SYSCD
        ddlsSYS.DataTextField = "CLSNM"
        ddlsSYS.DataValueField = "M23_TBOXCLS"
        ddlsSYS.DataBind()
        ddlsSYS.Items.Insert(0, "")

        ddlSYS.DataSource = SYSCD
        ddlSYS.DataTextField = "CLSNM"
        ddlSYS.DataValueField = "M23_TBOXCLS"
        ddlSYS.DataBind()
        ddlSYS.Items.Insert(0, "")

        SYSCD.Clear()

        '料金区分
        stb.Clear()
        stb.Append(" SELECT M29_CODE AS VALUE, (M29_CODE + ' : ' + M29_NAME) AS TEXT FROM M29_CLASS  ")
        stb.Append(" WHERE M29_CLASS_CD = '0080' ")
        Dim MAINTE As New DataTable
        MAINTE = ClsSQL.getDataSetTable(stb.ToString, "MAINTE")

        ddlsMAINTE.DataSource = MAINTE
        ddlsMAINTE.DataTextField = "TEXT"
        ddlsMAINTE.DataValueField = "VALUE"
        ddlsMAINTE.DataBind()
        ddlsMAINTE.Items.Insert(0, "")

        ddlMAINTE.DataSource = MAINTE
        ddlMAINTE.DataTextField = "TEXT"
        ddlMAINTE.DataValueField = "VALUE"
        ddlMAINTE.DataBind()
        ddlMAINTE.Items.Insert(0, "")
        MAINTE.Clear()

        'LAN単価種別
        stb.Clear()
        stb.Append(" SELECT M29_CODE AS VALUE, (M29_CODE + ' : ' + M29_NAME) AS NM FROM M29_CLASS  ")
        stb.Append(" WHERE  M29_CLASS_CD = '0116' ")
        ClsSQL.psDropDownDataBind(ddlsLAN_CLS, stb.ToString, "NM", "VALUE")
        ClsSQL.psDropDownDataBind(ddlLAN_CLS, stb.ToString, "NM", "VALUE")

    End Sub
    ''' <summary>
    ''' システム別LAN単価入力制御
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSYS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSYS.SelectedIndexChanged

        'フォーカス設定
        ddlSYS.Focus()

        'システム別活性制御　LAN単価
        'Select Case ddlSYS.SelectedValue.ToString
        Select Case If(ddlSYS.SelectedValue Is Nothing, "  ", ddlSYS.SelectedValue).Substring(0, 1)
            '   [T500][T700][T70M][T70R]
            'Case "01", "03", "04", "05"
            Case "0", "1"
                ddlLAN_CLS.Enabled = True
                'txtLAN.Enabled = False
            Case Else
                ddlLAN_CLS.Enabled = False
                txtLAN.Enabled = False
                ddlLAN_CLS.SelectedIndex = 0
                txtLAN.Text = String.Empty
        End Select
    End Sub
    ''' <summary>
    ''' LAN単価種別選択
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlLAN_CLS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlLAN_CLS.SelectedIndexChanged

        'フォーカス設定
        ddlLAN_CLS.Focus()

        Select Case ddlLAN_CLS.SelectedIndex
            Case 0
                txtLAN.Enabled = False
                txtLAN.Text = String.Empty
            Case Else
                txtLAN.Enabled = True
        End Select

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region


End Class
