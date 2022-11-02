'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　マスタ管理メニュー
'*　ＰＧＭＩＤ：　COMUPDM38
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.25　：　加賀
'*  更　新　　：　2015.04.23　：　杉山　
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM38-001     2015/06/03      加賀      現在適用期間より未来のデータを青文字表示に変更
'COMUPDM38-002     2015/06/08      稲葉      期間重複時のエラーメッセージ変更
'COMUPDM38-003     2015/06/10      栗原　　　更新日時の表示、JACKY非表示修正、編集エリア項目追加（登録、更新日時）

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SQL_DBCLS_LIB
Imports System.Data.SqlClient
Imports SPC.ClsCMExclusive



#End Region

Public Class COMUPDM38

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
    Const DispCode As String = "COMUPDM38"                  '画面ID

    'マスタ情報
    Const MasterName As String = "特殊店舗マスタ"           'マスタ名
    Const TableName As String = "M38_SPECIAL_SHOP"          'テーブル名
    Const KeyName1 As String = "M38_TBOXID"
    Const KeyName2 As String = "M38_NL_CLS"
    Const KeyName3 As String = "M38_TBOXID_TRUN"
    'Const KeyName4 As String = ""
    'Const KeyName5 As String = ""

    'ソート情報
    Const OrderByKey As String = " ORDER BY " & KeyName1 & ", 順番号 , M38_FROM_DT "

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
    Dim RcdCase As String = ""
    Dim strWhere As String = ""
    Dim stbValMes As New StringBuilder
    'COMUPDM38-003
    Dim clickflg As Boolean = False
    Dim txtchangeflg As Boolean = False
    'COMUPDM38-003 END
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

        '削除データ含むのチェックボックスは未使用となった
        '削除データは、検索条件に合致するデータを常に赤文字で表示する仕様とする
        Me.Master.ppchksDel.Checked = True
        Me.Master.ppchksDel.Visible = False


        '初回のみ実行　画面設定
        If Not IsPostBack Then
            SRCH = "first"
            DispMode = "DEF"
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)
            'Master.ppchksDel.Visible = True
            dtbStartDt.ppDateBox.Width = 69
            dtbENDDt.ppDateBox.Width = 69
            'COMUPDM38-003 バリデーション表示の調整
            Master.ppBtnClear.ValidationGroup = Nothing
            Master.ppBtnSrcClear.ValidationGroup = Nothing
            Dim val As CustomValidator = DirectCast(dtbStart.FindControl("cuvDateBox"), CustomValidator)
            val.EnableClientScript = True
            val = DirectCast(dtbStartDt.FindControl("cuvDateBox"), CustomValidator)
            val.EnableClientScript = True
            val = DirectCast(dtbFin.FindControl("cuvDateBox"), CustomValidator)
            val.EnableClientScript = True
            val = DirectCast(dtbENDDt.FindControl("cuvDateBox"), CustomValidator)
            val.EnableClientScript = True
            'COMUPDM38-003 END


            txtTBOX.Enabled = True
            lblSEQ.Enabled = False
            ddlSYS.Enabled = False
            txtVer.Enabled = False
            dtbENDDt.ppEnabled = False
            dtbStartDt.ppEnabled = False
            ddlPERAT.Enabled = False

            '削除データ検索フラグ初期化
            Me.ViewState.Add("SrchDelFlg", "0")

            'TBOX機種コード
            stb.Clear()
            stb.Append("SELECT M23_TBOXCLS, (M23_TBOXCLS + ' : ' + M23_TBOXCLS_NM) AS CLSNM FROM M23_TBOXCLASS WHERE  M23_DELETE_FLG = 0  ")
            ClsSQL.psDropDownDataBind(ddlSYS, stb.ToString, "CLSNM", "M23_TBOXCLS")
            ClsSQL.psDropDownDataBind(ddlSystem, stb.ToString, "CLSNM", "M23_TBOXCLS")
        End If

        'ボタンイベント設定
        AddHandler Master.ppBtnSrcClear.Click, AddressOf BtnSearchClear_Click
        AddHandler Master.ppBtnSearch.Click, AddressOf BtnSearch_Click
        AddHandler Master.ppBtnClear.Click, AddressOf BtnClear_Click
        AddHandler Master.ppBtnInsert.Click, AddressOf BtnInsert_Click
        AddHandler Master.ppBtnUpdate.Click, AddressOf BtnUpdate_Click
        AddHandler Master.ppBtnDelete.Click, AddressOf BtnDelete_Click

    End Sub
    ''' <summary>
    ''' PAGE PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        '閉じるボタンでEXIT
        If Master.ppCount = "close" OrElse DispMode = "EXIT" Then
            Exit Sub
        End If

        'COMUPDM38-003 
        If clickflg = True AndAlso txtTBOX.Enabled = False Then
            CstVal_Enable("select")
        End If

        'COMUPDM38-003 END


        'ボタンの使用制限
        If DispMode = String.Empty And Not ViewState("DispMode") Is Nothing Then
            DispMode = DirectCast(ViewState("DispMode"), String)
        ElseIf DispMode <> String.Empty Then
            Me.ViewState.Add("DispMode", DispMode)
        End If

        Select Case DispMode
            Case "DEF", "First"
                '初期　追加
                Master.ppBtnInsert.Enabled = False
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False

            Case "ADD"
                '初期　追加
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Enabled = False
            Case "UPD"
                '選択後　更新　削除
                Master.ppBtnInsert.Enabled = False
                If Master.ppBtnDelete.Text = "削除" Then
                    Master.ppBtnUpdate.Enabled = True
                Else
                    Master.ppBtnUpdate.Enabled = False
                End If
                Master.ppBtnDelete.Enabled = True

                txtTBOX.Enabled = False
                lblSEQ.Enabled = False
                dtbENDDt.ppEnabled = True
                dtbStartDt.ppEnabled = True
                ddlPERAT.Enabled = True
            Case Else
                '使用不可
                'BtnClear.Enabled = False
                'BtnAdd.Enabled = False
                'BtnUpd.Enabled = False
                'ckbDEL.Enabled = False
        End Select

        '更　新　　：　2015.04.22　：　杉山 　：　[M38_DELETE_FLG] = '0', '', '●'→[M38_DELETE_FLG] = '0', '', '1'へ変更
        'GRIDの検索結果の保持
        Dim SearchFlg As Boolean = False    '検索判定用フラグ
        Dim DispMesBox As Boolean = True    'メッセージ表示の有無

        stb.Clear()
        stb.Append("        [M38_TBOXID] AS TBOXID ")
        'COMUPDM38-003 区分JをN表示に変更、選択時J表示出来るように隠れ項目も追加
        stb.Append("       ,REPLACE([M38_NL_CLS],'J','N') AS 区分")
        stb.Append("       ,[M38_NL_CLS]                　 AS 検索用区分")
        'COMUPDM38-003 END
        stb.Append(" 	   ,CAST([M38_TBOXID_TRUN] AS smallINT) AS 順番号")
        'stb.Append(" 	   ,[M38_TBOXID_TRUN] AS 順番号") 
        stb.Append("       ,CONVERT(NVARCHAR, [M38_FROM_DT], 111) AS 開始日付 ")
        stb.Append("       ,CONVERT(NVARCHAR, [M38_END_DT], 111) AS 終了日付 ")
        stb.Append("       ,[M38_TBOX_TYPE] AS TYPE ")
        stb.Append(" 	   ,[M38_TBOX_TYPE] + ' : ' + [M23_TBOXCLS_NM] AS システム名称 ")
        'stb.Append(" 	   ,IIF([M38_TBOX_TYPE] = '**', '**', [M38_TBOX_TYPE] + ' : ' + [M23_TBOXCLS_NM]) AS システム名称 ")
        'stb.Append(" 	   ,IIF([M38_TBOX_TYPE] = '**', '共通', [M23_TBOXCLS_NM]) AS システム名称 ")
        'stb.Append(" 	   ,[M38_TBOX_TYPE]                AS システム名称 ")
        stb.Append("       ,[M38_VERSION] AS バージョン ")
        stb.Append("       ,[M38_HALL_CD] AS HALL_CD")
        stb.Append("  	   ,[M38_HALL_CD] + ' : ' + [T01_HALL_NAME] AS ホール名称 ")
        stb.Append("       ,IIF([M38_PERAT_CLS] = '0', '-', '+') AS 計算区分 ")
        'stb.Append("       ,IIF([M38_DELETE_FLG] = '0', '', '●') AS 削除 ")
        stb.Append("       ,IIF([M38_DELETE_FLG] = '0', '', '1') AS 削除 ")
        stb.Append("       ,FORMAT([M38_INSERT_DT], 'yyyy/MM/dd HH:mm:ss') AS 登録日時 ")
        'COMUPDM38-003
        stb.Append("       ,FORMAT([M38_UPDATE_DT], 'yyyy/MM/dd HH:mm:ss') AS 更新日時 ")
        'COMUPDM38-003　END
        stb.Append("   FROM [SPCDB].[dbo].[M38_SPECIAL_SHOP] AS M38 ")
        stb.Append("   LEFT JOIN [SPCDB].[dbo].[M23_TBOXCLASS] AS M23 ")
        stb.Append("   ON   M38.M38_TBOX_TYPE = M23.M23_TBOXCLS  ")
        stb.Append("    ")
        stb.Append("   LEFT JOIN [SPCDB].[dbo].[T01_HALL] AS T01 ")
        stb.Append("   ON   M38.M38_HALL_CD  = T01.T01_HALL_CD ")

        Select Case SRCH
            Case ""         '非検索時
                Exit Sub
            Case "Edit"
                'SRCH = DirectCast(ViewState("SRCH"), String)
                SRCH = stb.ToString + strWhere
                DispMesBox = False
            Case "Del"      '削除時
                'Select Case DirectCast(ViewState("SrchDelFlg"), String)
                '    Case "0"
                '        SRCH = DirectCast(ViewState("SRCH"), String) + " AND " & TableName.Substring(0, 4) & "DELETE_FLG = '0' "
                '    Case "1"
                '        SRCH = DirectCast(ViewState("SRCH"), String)
                'End Select
                'データバインド
                Master.ppCount = "0"
                grvList.DataSource = New DataTable
                grvList.DataBind()
                Exit Sub
            Case "Dep"
                SRCH = DirectCast(ViewState("SRCH"), String)
            Case "first"    '初回表示
                stb.Append(" WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 ")
                stb.Append(" AND M38_FROM_DT <= GETDATE() AND M38_END_DT >= GETDATE() ")
                SRCH = stb.ToString
            Case "clear"    '検索条件無し
                'stb.Append(" WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 ")
                SRCH = stb.ToString
                SearchFlg = True
            Case Else       '検索時
                SRCH = stb.ToString + SRCH
                SearchFlg = True
        End Select
        Me.ViewState.Add("SRCH", SRCH)

        '該当件数表示 & 表示件数上限設定
        Dim RecordCount As Integer = ClsSQL.GetRecordCount("SELECT" + SRCH) '該当件数
        Dim DispLimit As Integer = ClsSQL.GetDispLimit(DispCode, SearchFlg) '表示上限
        Master.ppCount = RecordCount                                     '該当件数表示

        Select Case ClsSQL.CheckLimitOver(DispCode, RecordCount, SearchFlg)
            Case -1         'エラー
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                Exit Sub
            Case 0          '0件
                If DispMesBox = True Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If
                SRCH = "SELECT" + SRCH
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

        'ソート指定
        SRCH = SRCH + OrderByKey

        Try
            dt = ClsSQL.getDataSetTable(SRCH, "view")
        Catch ex As Exception
            'データの取得に失敗しました
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Exit Sub
        End Try

        'データバインド
        grvList.DataSource = dt
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
        clickflg = True
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
        lblFROM.Text = ""
        lblEND.Text = ""
        txtTBOX.Text = ""
        lblNL.Text = ""
        'COMUPDM38-003
        lblInsDay.Text = ""
        lblUpdDay.Text = ""
        dtbStart.ValidateRequestMode = UI.ValidateRequestMode.Disabled
        'COMUPDM38-003 END


        txtVer.Text = ""
        lblHALL.Text = ""
        lblSEQ.Text = ""
        dtbStartDt.ppText = ""
        dtbENDDt.ppText = ""
        ddlSYS.SelectedIndex = -1

        ddlPERAT.SelectedIndex = 0

        txtTBOX.Enabled = True
        lblSEQ.Enabled = False
        ddlSYS.Enabled = False
        txtVer.Enabled = False
        dtbENDDt.ppEnabled = False
        dtbStartDt.ppEnabled = False
        ddlPERAT.Enabled = False
        Master.ppBtnDelete.Text = "削除"
        Master.ppMainEnabled = True
        Master.ppBtnUpdate.Enabled = True


        DispMode = "DEF"

        txtTBOX.Focus()

        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    ''' 追加ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BtnInsert_Click()

        'ログ出力開始
        psLogStart(Me)

        '検証
        If Me.IsValid = False OrElse stbValMes.ToString <> "" Then
            '整合性チェック
            If stbValMes.ToString <> "" Then
                stb.Clear()
                'stb.Append("vb_MsgCri_O('\n" & stbValMes.ToString & "','整合性エラー');")
                stb.Append("vb_MsgExc_O('\n" & stbValMes.ToString & "','整合性エラー');")
                'Me.ClientScript.RegisterStartupScript(Me.GetType, "W30010", stb.ToString, True)
                Me.ClientScript.RegisterStartupScript(Me.GetType, "E20005", stb.ToString, True)
            End If

            Exit Sub
        End If

        Try
            'DB接続
            If mclsDB.pfDB_Connect(ClsSQL.CnStr) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                'ログ出力終了
                psLogEnd(Me)
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
            stb.Append(" INSERT INTO " & TableName & " ( ")
            stb.Append("       [M38_TBOXID] ")
            stb.Append("       ,[M38_NL_CLS] ")
            stb.Append("       ,[M38_TBOXID_TRUN] ")
            stb.Append("       ,[M38_FROM_DT] ")
            stb.Append("       ,[M38_END_DT] ")
            stb.Append("       ,[M38_TBOX_TYPE] ")
            stb.Append("       ,[M38_VERSION] ")
            stb.Append("       ,[M38_HALL_CD] ")
            stb.Append("       ,[M38_PERAT_CLS] ")
            stb.Append(" ,	" & TableName.Substring(0, 4) & "DELETE_FLG ")
            stb.Append(" ,	" & TableName.Substring(0, 4) & "INSERT_DT ")
            stb.Append(" ,	" & TableName.Substring(0, 4) & "INSERT_USR ")
            stb.Append(" ) SELECT ")
            stb.Append("    '" & txtTBOX.Text & "' ")
            If lblNL.Text = "" Then
                stb.Append(" ,  '' ")
            Else
                stb.Append(" ,  '" & lblKey2.Text & "' ")
            End If
            stb.Append(" ,  '" & lblSEQ.Text & "' ")
            stb.Append(" ,  '" & dtbStartDt.ppText & "' ")
            stb.Append(" ,  '" & dtbENDDt.ppText & "' ")

            If ddlSYS.SelectedIndex = 0 Then
                stb.Append(" ,  '' ")
            Else
                stb.Append(" ,  '" & ddlSYS.SelectedValue.ToString & "' ")
            End If

            stb.Append(" ,  '" & txtVer.Text & "' ")

            If lblHALL.Text = "" Then
                stb.Append(" ,  '' ")
            Else
                stb.Append(" ,  '" & lblHALL.Text.Substring(0, 7) & "' ")
            End If

            stb.Append(" ,  '" & ddlPERAT.SelectedValue.ToString & "' ")
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
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)
            stb.Clear()
            stb.Append("WHERE  ")
            stb.Append("" & KeyName1 & " = '" & txtTBOX.Text & "' AND ")
            stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
            stb.Append("" & KeyName3 & " = '" & lblSEQ.Text & "'  ")
            strWhere = stb.ToString
            BtnClear_Click()
            DispMode = "DEF"
            SRCH = "Edit"

            'フォーカス設定
            txtsTfrom.Focus()
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

        'ログ出力開始
        psLogStart(Me)

        '検証
        If Me.IsValid = False OrElse stbValMes.ToString <> "" Then
            '整合性チェック
            If stbValMes.ToString <> "" Then
                stb.Clear()
                stb.Append("vb_MsgExc_O('" & stbValMes.ToString & "','W30010');")
                Me.ClientScript.RegisterStartupScript(Me.GetType, "W30010", stb.ToString, True)
            End If

            Exit Sub
        End If

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
            stb.Append("        [M38_TBOXID_TRUN] = '" & lblSEQ.Text & "'")
            stb.Append("       ,[M38_FROM_DT] = '" & dtbStartDt.ppText & "'")
            stb.Append("       ,[M38_END_DT] = '" & dtbENDDt.ppText & "'")
            stb.Append("       ,[M38_PERAT_CLS] = '" & ddlPERAT.SelectedValue.ToString & "'")
            If ddlSYS.Enabled = True AndAlso txtVer.Enabled = True Then
                stb.Append("       ,[M38_TBOX_TYPE]  = '" & ddlSYS.SelectedValue.ToString & "'")
                stb.Append("       ,[M38_VERSION] = '" & txtVer.Text & "'")
            End If
            'stb.Append("," & TableName.Substring(0, 4) & "DELETE_FLG = '" & ClsSQL.GetDltCheckNum(ckbDEL) & "'")
            stb.Append("," & TableName.Substring(0, 4) & "UPDATE_DT = '" & DateTime.Now & "'")
            stb.Append("," & TableName.Substring(0, 4) & "UPDATE_USR = '" & User.Identity.Name & "'")
            stb.Append("WHERE  ")
            stb.Append("" & KeyName1 & " = '" & lblKey1.Text & "' AND ")
            stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
            stb.Append("" & KeyName3 & " = '" & lblKey3.Text & "'  ")

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
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)
            stb.Clear()
            stb.Append("WHERE  ")
            stb.Append("" & KeyName1 & " = '" & txtTBOX.Text & "' AND ")
            stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
            stb.Append("" & KeyName3 & " = '" & lblSEQ.Text & "'  ")
            strWhere = stb.ToString
            BtnClear_Click()
            DispMode = "DEF"
            SRCH = "Edit"

            'フォーカス設定
            txtsTfrom.Focus()
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

        Dim MesNo As String
        Dim strDelFlg As String
        Select Case Master.ppBtnDelete.Text
            Case "削除"
                MesNo = "00002"
                strDelFlg = "1"
                SRCH = "Del"
            Case Else
                '削除解除

                '重複チェック
                stb.Clear()
                stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
                stb.Append("WHERE  ")
                stb.Append("" & KeyName1 & " =  '" & txtTBOX.Text & "' AND ")
                stb.Append("" & KeyName2 & " =  '" & lblNL.Text.Substring(0, 1) & "' AND ")
                stb.Append("" & KeyName3 & " <> '" & lblKey3.Text & "' AND ")
                stb.Append(" M38_DELETE_FLG  =   '0' AND ")
                stb.Append(" (( ")
                stb.Append(" ( M38_FROM_DT <= '" & dtbStartDt.ppText & "'  AND '" & dtbStartDt.ppText & "' <=  M38_END_DT )")
                stb.Append(" OR ")
                stb.Append(" ( M38_FROM_DT <= '" & dtbENDDt.ppText & "'    AND '" & dtbENDDt.ppText & "' <=  M38_END_DT )")
                stb.Append(" ) OR ( ")
                stb.Append("  '" & dtbStartDt.ppText & "' <= [M38_FROM_DT] AND [M38_END_DT] < '" & dtbENDDt.ppText & "'  ")
                stb.Append(" )) ")

                If ClsSQL.GetRecord(stb.ToString) > 0 Then
                    'COMUPDM38-002
                    'stbValMes.Append("・TBOXID:" & txtTBOX.Text & " NL区分:" & lblNL.Text & " に重複する期間が存在しています。\n")
                    stbValMes.Append("重複する期間が登録されています。\n登録データの確認をしてください。\n")
                    'COMUPDM38-002 END
                    stb.Clear()
                    stb.Append("vb_MsgExc_O('\n" & stbValMes.ToString & "','E20005');")
                    Me.ClientScript.RegisterStartupScript(Me.GetType, "E20005", stb.ToString, True)
                    Exit Sub
                End If
                stb.Clear()
                stb.Append("WHERE  ")
                stb.Append("" & KeyName1 & " = '" & txtTBOX.Text & "' AND ")
                stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
                stb.Append("" & KeyName3 & " = '" & lblSEQ.Text & "'  ")
                strWhere = stb.ToString

                MesNo = "00001"
                strDelFlg = "0"
                SRCH = "Edit"
        End Select

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
                psMesBox(Me, MesNo, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            stb.Clear()
            stb.Append("UPDATE " & TableName & " SET ")
            stb.Append("" & TableName.Substring(0, 4) & "DELETE_FLG = '" & strDelFlg & "', ")
            stb.Append("" & TableName.Substring(0, 4) & "UPDATE_DT = '" & DateTime.Now & "', ")
            stb.Append("" & TableName.Substring(0, 4) & "UPDATE_USR = '" & User.Identity.Name & "' ")
            stb.Append("WHERE  ")
            stb.Append("" & KeyName1 & " = '" & lblKey1.Text & "' AND ")
            stb.Append("" & KeyName2 & " = '" & lblKey2.Text & "' AND ")
            stb.Append("" & KeyName3 & " = '" & lblKey3.Text & "'  ")


            'SQL実行
            If mclsDB.pfDB_ExecuteSQL(stb.ToString) > 0 Then
            Else
                '失敗メッセージ表示
                psMesBox(Me, MesNo, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            'コミット
            If mclsDB.pfDB_CommitTrans() = False Then
                '失敗メッセージ表示
                psMesBox(Me, MesNo, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Sub
            End If

            '完了メッセージ表示
            psMesBox(Me, MesNo, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)
            BtnClear_Click()
            DispMode = "DEF"

            Master.ppBtnDelete.Text = "削除"

            'フォーカス設定
            txtsTfrom.Focus()
        Catch ex As Exception
            '失敗メッセージ表示
            psMesBox(Me, MesNo, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
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
    Private Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand
        clickflg = True
        Try
            If e.CommandName = "Sort" Then

                '４桁以上の数字文字列でカンマ表示しない時使用
                'grvList_DataBoundのコメントアウトも解除

                ''順番号の頭に０付加
                'Dim intTRUN As Int16
                'For Each rowData As GridViewRow In grvList.Rows
                '    intTRUN = Int16.Parse(CType(rowData.FindControl("順番号"), TextBox).Text)

                '    CType(rowData.FindControl("順番号"), TextBox).Text = intTRUN.ToString("000")
                'Next

                Exit Sub

            ElseIf e.CommandName = "Select" Then

                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

                stb.Clear()
                stb.Append("SELECT *　FROM " & TableName & " ")
                stb.Append(" WHERE ")
                'stb.Append("" & KeyName1 & " = '" & CType(rowData.FindControl("TBOXID"), TextBox).Text & "' AND ")
                'stb.Append("" & KeyName1 & " = '" & CType(rowData.FindControl("TBOXID"), TextBox).Text & "' AND ")
                stb.Append("" & KeyName1 & " = '" & CType(rowData.FindControl("TBOXID"), TextBox).Text & "' AND ")
                'COMUPDM38-003　検索用区分を使用するように変更
                stb.Append("" & KeyName2 & " = '" & CType(rowData.FindControl("検索用区分"), TextBox).Text & "' AND ")
                'COMUPDM38-003
                stb.Append("" & KeyName3 & " = '" & CType(rowData.FindControl("順番号"), TextBox).Text & "'  ")
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


                '編集エリアに値を反映
                lblKey1.Text = dt(0)(KeyName1).ToString
                lblKey2.Text = dt(0)(KeyName2).ToString
                lblKey3.Text = dt(0)(KeyName3).ToString
                lblFROM.Text = dt(0)("M38_FROM_DT").ToString.Substring(0, 10)
                lblEND.Text = dt(0)("M38_END_DT").ToString.Substring(0, 10)
                'COMUPDM38-003
                lblInsDay.Text = dt(0)("M38_INSERT_DT").ToString
                lblUpdDay.Text = dt(0)("M38_UPDATE_DT").ToString
                If dt(0)("M38_NL_CLS").ToString <> "J" Then
                    lblNL.Text = dt(0)("M38_NL_CLS").ToString
                Else
                    lblNL.Text = "N"
                End If
                'COMUPDM38-003 END
                txtTBOX.Text = dt(0)("M38_TBOXID").ToString
                If dt(0)("M38_TBOX_TYPE").ToString.Trim = "" Then
                    ddlSYS.SelectedIndex = 0
                ElseIf dt(0)("M38_TBOX_TYPE").ToString = "**" Then
                    ddlSYS.SelectedIndex = 0
                Else
                    ddlSYS.SelectedValue = dt(0)("M38_TBOX_TYPE").ToString
                End If
                txtVer.Text = dt(0)("M38_VERSION").ToString
                If dt(0)("M38_HALL_CD").ToString.Trim = "" Then
                    lblHALL.Text = ""
                Else
                    'lblHALL.Text = dt(0)("M38_HALL_CD").ToString + ":" + CType(rowData.FindControl("ホール名称"), TextBox).Text
                    lblHALL.Text = CType(rowData.FindControl("ホール名称"), TextBox).Text
                End If

                lblSEQ.Text = dt(0)("M38_TBOXID_TRUN").ToString
                dtbStartDt.ppText = dt(0)("M38_FROM_DT").ToString.Substring(0, 10)
                dtbENDDt.ppText = dt(0)("M38_END_DT").ToString.ToString.Substring(0, 10)
                ddlPERAT.SelectedValue = dt(0)("M38_PERAT_CLS").ToString

                'TBOXID:21419011　黒鳥１２３の場合のみ活性
                If dt(0)("M38_TBOXID").ToString = "21419011" Then
                    ddlSYS.Enabled = True
                    txtVer.Enabled = True
                    'フォーカス設定
                    ddlSYS.Focus()
                Else
                    ddlSYS.Enabled = False
                    txtVer.Enabled = False
                    'フォーカス設定
                    dtbStartDt.ppDateBox.Focus()
                End If

                Select Case dt(0)("" & TableName.Substring(0, 4) & "DELETE_FLG").ToString
                    Case "0"
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                        Master.ppMainEnabled = True
                        Master.ppBtnUpdate.Enabled = True
                    Case "1"
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                        Master.ppMainEnabled = False
                        Master.ppBtnUpdate.Enabled = False

                        'フォーカス設定
                        Master.ppBtnClear.Focus()
                End Select

            End If

            DispMode = "UPD"
            txtTBOX.Enabled = False
            'txtSEQ.Enabled = False

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
            If CType(rowData.FindControl("削除"), TextBox).Text = "1" Then
                'COMUPDM38-001
                CType(rowData.FindControl("TBOXID"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("順番号"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ホール名称"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("区分"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("システム名称"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("バージョン"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("開始日付"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("終了日付"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("計算区分"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("登録日時"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("更新日時"), TextBox).ForeColor = Drawing.Color.Red
                'COMUPDM38-001 END
            Else

                dteFROM = Date.Parse(CType(rowData.FindControl("開始日付"), TextBox).Text)
                dteTO = Date.Parse(CType(rowData.FindControl("終了日付"), TextBox).Text)

                If dteFROM <= dteNow And dteNow <= dteTO Then
                    '期間内
                    '初回検索時は適応期間外データ非表示
                ElseIf dteNow < dteFROM Then
                    '後
                    'COMUPDM38-001
                    CType(rowData.FindControl("開始日付"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("終了日付"), TextBox).ForeColor = Drawing.Color.Blue
                    'COMUPDM38-001 END
                Else

                    '更　新　　：　2015.04.22　：　杉山 　：　Drawing.Color.RedをDrawing.Color.Blueへ更新
                    '終了期間
                    'CType(rowData.FindControl("開始日付"), TextBox).ForeColor = Drawing.Color.Red
                    'CType(rowData.FindControl("終了日付"), TextBox).ForeColor = Drawing.Color.Red

                    CType(rowData.FindControl("開始日付"), TextBox).ForeColor = Drawing.Color.Blue
                    CType(rowData.FindControl("終了日付"), TextBox).ForeColor = Drawing.Color.Blue

                End If

            End If

        Next

        ''更　新　　：　2015.04.22　：　杉山 　：　削除データを赤字にする 'COMUPDM38-001 上のループ内に移動
        'For Each rowData As GridViewRow In grvList.Rows
        '    If CType(rowData.FindControl("削除"), TextBox).Text = "1" Then
        '        CType(rowData.FindControl("TBOXID"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("順番号"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("ホール名称"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("区分"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("システム名称"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("バージョン"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("開始日付"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("終了日付"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("計算区分"), TextBox).ForeColor = Drawing.Color.Red
        '        CType(rowData.FindControl("登録日時"), TextBox).ForeColor = Drawing.Color.Red
        '    End If
        'Next 'COMUPDM38-001 END

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

    ''' <summary>
    '''TBOX情報表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub txtTBOX_TextChanged(sender As Object, e As EventArgs) Handles txtTBOX.TextChanged
        ''キー入力値検証
        CstVal_Enable("txt")
        txtchangeflg = True
        Page.Validate("key")
        If Me.IsValid = False Then
            Exit Sub
        End If

        If DirectCast(sender, TextBox).Text = String.Empty Then
            BtnClear_Click()
            Exit Sub
        End If

        '活性制御
        txtTBOX.Enabled = False
        'txtSEQ.Enabled = True
        dtbENDDt.ppEnabled = True
        dtbStartDt.ppEnabled = True
        ddlPERAT.Enabled = True

        'フォーカス設定
        dtbStartDt.ppDateBox.Focus()

        stb.Clear()
        stb.Append(" SELECT [T03_NL_CLS]                            AS NL  ")
        stb.Append("       ,[T03_SYSTEM_CD]                         AS TYPE")
        stb.Append("       ,[T03_VERSION]                           AS VER ")
        stb.Append("       ,[T03_HALL_CD] + ' : ' + [T01_HALL_NAME] AS HALL")

        stb.Append("   FROM (SELECT [T03_NL_CLS]")
        stb.Append("               ,[T03_SYSTEM_CD]")
        stb.Append("               ,[T03_TBOXCLASS_CD]")
        stb.Append("               ,[T03_VERSION]")
        stb.Append("               ,[T03_HALL_CD]")

        stb.Append("   FROM [SPCDB].[dbo].[T03_TBOX]")
        stb.Append("  WHERE [T03_TBOXID] = '" & txtTBOX.Text & "' ) AS T03 ")
        stb.Append("   ")
        stb.Append("  LEFT JOIN [SPCDB].[dbo].[T01_HALL] AS T01 ")
        stb.Append("   ON   T03.T03_HALL_CD  = T01.T01_HALL_CD ")
        stb.Append("  ")
        stb.Append(" UNION ALL ")
        stb.Append("  ")
        stb.Append(" SELECT 'EMPTY', '', '', '' ")

        dt.Clear()
        dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")



        Select Case dt(0)("NL").ToString
            'COMUPDM38-003 lblKey2に登録用のNL区分を保持
            Case "N"
                lblNL.Text = "N"
                lblKey2.Text = "N"
            Case "L"
                lblNL.Text = "L"
                lblKey2.Text = "L"

            Case "J"
                lblNL.Text = "N"
                lblKey2.Text = "J"
                'COMUPDM38-003 END
            Case "EMPTY"
                'TBOXID存在しない
                lblNL.Text = ""
                lblKey2.Text = ""
                ddlSYS.SelectedIndex = 0
                txtVer.Text = ""
                lblHALL.Text = ""
                lblSEQ.Text = ""
                'DispMode = "EXIT"
                DispMode = "ADD"

                Exit Sub
        End Select



        If dt(0)("TYPE").ToString.Trim = "" Then
            ddlSYS.SelectedIndex = 0
        ElseIf dt(0)("TYPE").ToString = "**" Then
            ddlSYS.SelectedIndex = 0
        Else
            ddlSYS.SelectedValue = dt(0)("TYPE").ToString
        End If
        'ddlSYS.SelectedValue = dt(0)("TYPE").ToString
        txtVer.Text = dt(0)("VER").ToString
        lblHALL.Text = dt(0)("HALL").ToString


        'TBOXID:21419011　黒鳥１２３の場合のみドロップダウン活性
        If txtTBOX.Text = "21419011" Then
            ddlSYS.Enabled = True
            txtVer.Enabled = True
            ddlSYS.Focus()
        Else
            ddlSYS.Enabled = False
            txtVer.Enabled = False
        End If


        '順番号の発番
        stb.Clear()
        stb.Append(" DECLARE @SEQ as numeric; ")
        stb.Append(" SET @SEQ = ISNULL( ( SELECT ISNULL(MAX(CONVERT(NUMERIC, [M38_TBOXID_TRUN])) + 1, 1) as TRUN       ")
        stb.Append("                        FROM [SPCDB].[dbo].[M38_SPECIAL_SHOP] ")
        stb.Append("                       WHERE [M38_TBOXID]  = '" & txtTBOX.Text & "' ), 1) ; ")
        stb.Append(" SELECT CASE  ")
        stb.Append("          WHEN @SEQ =   1 THEN 1 ")
        stb.Append("          WHEN @SEQ > 999 THEN (SELECT IIF (先頭連番 > 1, 1, 欠番 + 1)       ")
        stb.Append("                                  FROM (    SELECT MIN(CONVERT(Numeric, T1.M38_TBOXID_TRUN))   AS 欠番,  ")
        stb.Append("                                           (SELECT MIN(CONVERT(Numeric, M38_TBOXID_TRUN)) FROM [SPCDB].[dbo].[M38_SPECIAL_SHOP]")
        stb.Append("                                            WHERE M38_TBOXID = '" & txtTBOX.Text & "') AS 先頭連番 ")
        stb.Append("                                              FROM (SELECT * FROM  [SPCDB].[dbo].[M38_SPECIAL_SHOP] WHERE M38_TBOXID = '" & txtTBOX.Text & "') AS T1  	                       	  ")
        stb.Append("                                         LEFT JOIN (SELECT * FROM  [SPCDB].[dbo].[M38_SPECIAL_SHOP] WHERE M38_TBOXID = '" & txtTBOX.Text & "') AS T2  ")
        stb.Append("                                                ON T1.[M38_TBOXID_TRUN] + 1 = T2.[M38_TBOXID_TRUN] ")
        stb.Append("                                             WHERE T2.[M38_TBOXID_TRUN] Is NULL )   AS T0) ")
        stb.Append("          ELSE @SEQ ")
        stb.Append("        END ")
        'stb.Append("    AND  [M38_DELETE_FLG] = '0' ")
        'dt.Clear()
        'dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

        'txtSEQ.Text = dt(0)("TRUN").ToString
        lblSEQ.Text = ClsSQL.GetRecord(stb.ToString)

        DispMode = "ADD"

    End Sub
    '============================================================================================================================
    '==   Validation
    '============================================================================================================================

#Region "                             --- Validation ---                                "
    Protected Sub CstDayVal_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstDayVal.ServerValidate

        '値が日付になるか
        Dim result As Date
        If Not dtbStart.ppFromText = "" Then
            If Date.TryParse(dtbStart.ppFromText, result) = False Then
                'ユーザーコントロールで検証するのでEXIT
                Exit Sub
            End If
        End If
        If Not dtbStart.ppToText = "" Then
            If Date.TryParse(dtbStart.ppToText, result) = False Then
                'ユーザーコントロールで検証するのでEXIT
                Exit Sub
            End If
        End If
        If Not dtbFin.ppFromText = "" Then
            If Date.TryParse(dtbFin.ppFromText, result) = False Then
                'ユーザーコントロールで検証するのでEXIT
                Exit Sub
            End If
        End If
        If Not dtbFin.ppToText = "" Then
            If Date.TryParse(dtbFin.ppToText, result) = False Then
                'ユーザーコントロールで検証するのでEXIT
                Exit Sub
            End If
        End If

        If (dtbStart.ppToText <> "" OrElse dtbStart.ppFromText <> "") AndAlso (dtbFin.ppToText <> "" OrElse dtbFin.ppFromText <> "") Then
            source.ErrorMessage = "開始日付、終了日付はどちらかのみ入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub
    ''' <summary>
    '''TBOXID
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_sID_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_sID.ServerValidate

        Dim tb As TextBox = txtsTfrom
        Const Name As String = "TBOXID"
        source.ControlToValidate = tb.ID

        If Regex.IsMatch(tb.Text, "^[0-9]{0,8}$") = False OrElse Regex.IsMatch(txtsTto.Text, "^[0-9]{0,8}$") = False Then
            source.ErrorMessage = Name & "は半角数字で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub
    ''' <summary>
    '''TBOXID
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_DUTY_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_TBOX.ServerValidate

        Const Name As String = "TBOXID"
        Dim tb As TextBox = CType(Master.FindControl("MainContent").FindControl(source.ControlToValidate), TextBox)

        If Regex.IsMatch(tb.Text, "^[0-9a-zA-Z]{8}$") Then
        Else
            source.ErrorMessage = "" & Name & "は半角英数字 8 桁で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

        ''存在チェック
        'stb.Clear()
        'stb.Append(" SELECT T03_TBOXID   FROM T03_TBOX  ")
        'stb.Append(" WHERE T03_TBOXID = '" & txtTBOX.Text & "' ")
        'If ClsSQL.GetRecordCount(stb.ToString) <= 0 Then
        '    'psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "" & Name & ":" & txtTBOX.Text & "")

        '    source.ErrorMessage = "" & Name & ":" & txtTBOX.Text & "は存在しません。"
        '    source.Text = "整合性エラー"
        '    args.IsValid = False
        '    stbValMes.Append("" & Name & ":" & txtTBOX.Text & "は存在しません。\n")
        '    Exit Sub

        'End If

        '更　新　　：　2015.04.23　：　杉山　整合性チェックの処理変更
        '存在チェック
        stb.Clear()
        stb.Append(" SELECT T01_TBOXID   FROM T01_HALL  ")
        stb.Append(" WHERE T01_TBOXID = '" & txtTBOX.Text & "' ")

        Select Case ClsSQL.GetRecordCount(stb.ToString)


            Case "-1"

                'エラー

            Case "0"

                stb.Clear()
                stb.Append(" SELECT C01_TBOX_ID   FROM C01_SPS_JGT  ")
                stb.Append(" WHERE C01_TBOX_ID = '" & txtTBOX.Text & "' ")

                If ClsSQL.GetRecordCount(stb.ToString) > 0 Then

                    args.IsValid = True

                Else

                    source.ErrorMessage = "" & Name & ":" & txtTBOX.Text & "は存在しません。"
                    source.Text = "整合性エラー"
                    args.IsValid = False
                    'stbValMes.Append("" & Name & ":" & txtTBOX.Text & "は存在しません。\n")
                    Exit Sub

                End If

        End Select



        ''ホール存在チェック
        'Select Case tb.Text
        '    '例外設定 以下のTBOXIDはホール存在チェックしない
        '    Case "00000000", _
        '        "00000061", _
        '        "80000011", _
        '        "80000021", _
        '        "99900011", _
        '        "99900021", _
        '        "99900201", _
        '        "99990021", _
        '        "99999989", _
        '        "99999999", _
        '        "80000011", _
        '        "80000011"

        '        Exit Sub
        'End Select
        'stb.Clear()
        'stb.Append(" SELECT ISNULL([T01_HALL_CD], 'NULL') ")
        'stb.Append("       ,[T03_HALL_CD] ")
        'stb.Append(" 	   ,[T03_TBOXID] ")
        'stb.Append("   FROM [SPCDB].[dbo].[T03_TBOX] ")
        'stb.Append("   LEFT JOIN [SPCDB].[dbo].[T01_HALL] ")
        'stb.Append("   ON [T03_HALL_CD] = [T01_HALL_CD] ")
        'stb.Append("   WHERE [T03_TBOXID] = '" & txtTBOX.Text & "' ")
        'If ClsSQL.GetRecord(stb.ToString).ToString = "NULL" Then
        '    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "" & Name & ":" & txtTBOX.Text & "のホール")
        '    'source.ErrorMessage = ""
        '    'source.Text = ""
        '    'stbValMes.Append("" & Name & ":" & txtTBOX.Text & "のホールは存在しません。\n")
        '    source.ErrorMessage = "" & Name & ":" & txtTBOX.Text & "のホールは存在しません。"
        '    source.Text = "整合性エラー"
        '    args.IsValid = False
        '    Exit Sub
        'End If



    End Sub
    ''' <summary>
    '''バージョン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_4_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_4.ServerValidate

        Dim tb As TextBox = txtVer
        Const Name As String = "バージョン"
        source.ControlToValidate = tb.ID

        If tb.Enabled = False Then
            Exit Sub
        End If

        If tb.Text = String.Empty Then
            source.ErrorMessage = "" & Name & "に値が設定されていません。"
            source.Text = "未入力エラー"
            args.IsValid = False
            Exit Sub
        End If

        If Regex.IsMatch(tb.Text, "[a-zA-Z0-9 -/:-@\[-\`\{-\~]+") = False Then
            source.ErrorMessage = "" & Name & "は半角文字で入力して下さい。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

        If tb.Text.ToUpper = "NULL" Then
            source.ErrorMessage = "" & Name & "は不正な値です。"
            source.Text = "形式エラー"
            args.IsValid = False
            Exit Sub
        End If

    End Sub
    ''' <summary>
    '''開始日付
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_Date_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_Date.ServerValidate

        '値が日付になるか
        Dim result As Date
        If Date.TryParse(dtbStartDt.ppText, result) = False Then
            'ユーザーコントロールで検証するのでEXIT
            Exit Sub
        End If
        If Date.TryParse(dtbENDDt.ppText, result) = False Then
            'ユーザーコントロールで検証するのでEXIT
            Exit Sub
        End If

        If lblNL.Text = "" Then
            Exit Sub
        End If

        If Master.ppBtnDelete.Text <> "削除" Then
            Exit Sub
        End If

        '重複チェック
        stb.Clear()
        stb.Append("SELECT COUNT(*) FROM " & TableName & " ")
        stb.Append("WHERE  ")
        stb.Append("" & KeyName1 & " =  '" & txtTBOX.Text & "' AND ")
        stb.Append("" & KeyName2 & " =  '" & lblNL.Text.Substring(0, 1) & "' AND ")
        stb.Append("" & KeyName3 & " <> '" & lblKey3.Text & "' AND ")
        stb.Append(" M38_DELETE_FLG  =   '0' AND ")
        stb.Append(" (( ")
        stb.Append(" ( M38_FROM_DT <= '" & dtbStartDt.ppText & "'  AND '" & dtbStartDt.ppText & "' <=  M38_END_DT )")
        stb.Append(" OR ")
        stb.Append(" ( M38_FROM_DT <= '" & dtbENDDt.ppText & "'    AND '" & dtbENDDt.ppText & "' <=  M38_END_DT )")
        stb.Append(" ) OR ( ")
        stb.Append("  '" & dtbStartDt.ppText & "' <= [M38_FROM_DT] AND [M38_END_DT] < '" & dtbENDDt.ppText & "'  ")
        stb.Append(" )) ")

        If ClsSQL.GetRecord(stb.ToString) > 0 Then
            stb.Clear()

            stb.Append("重複する期間が登録されています。")
            'COMUPDM38-002
            'stbValMes.Append("・TBOXID:" & txtTBOX.Text & " NL区分:" & lblNL.Text & " に重複する期間が存在しています。\n")
            stbValMes.Append("重複する期間が登録されています。\n登録データの確認をしてください。\n")
            'COMUPDM38-002 END

            'stb.Append("vb_MsgExc_O('TBOXID:00000000 NL区分:L に重複する期間が存在しています。\nデータ変更ができません。','W30010');")
            'psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "TBOXID:" & txtTBOX.Text & " NL区分:" & lblNL.Text & " に重複する期間", "データ変更")
            'Me.ClientScript.RegisterStartupScript(Me.GetType, "W30010", stb.ToString, True)
            'vb_MsgExc_O('TBOXID:00000061 NL区分:N に重複する期間が存在しています。\nデータ変更ができません。','W30010');
            'source.ErrorMessage = ""
            'source.Text = ""
            'source.ErrorMessage = stb.ToString
            'source.Text = "整合性エラー"
            'args.IsValid = False
            RcdCase = "重複"
        End If

    End Sub
    ''' <summary>
    '''終了日付
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CstmVal_End_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CstmVal_End.ServerValidate

        '値が日付になるか
        Dim result As Date
        If Date.TryParse(dtbENDDt.ppText, result) = False Then
            'ユーザーコントロールで検証するのでEXIT
            Exit Sub
        End If

        If Date.TryParse(dtbStartDt.ppText, result) = False Then
            '下の比較でエラーになるのでEXIT
            Exit Sub
        End If

        '開始日との整合性
        If Date.Parse(dtbStartDt.ppText) > Date.Parse(dtbENDDt.ppText) Then
            source.ErrorMessage = "終了日付は開始日付以降の日付で入力してください。"
            source.Text = "日付エラー"
            args.IsValid = False
            Exit Sub
        End If

        Select Case RcdCase
            Case "重複"
                'stb.Clear()
                'stb.Append("TBOXID:" & txtTBOX.Text & " ")
                'stb.Append("NL区分:" & lblNL.Text & " ")
                'stb.Append("に重複する期間が登録されています。")
                'source.ErrorMessage = stb.ToString
                ''source.ErrorMessage = ""
                'source.Text = "整合性エラー"
                'args.IsValid = False
                ''RcdCase = "重複"
                Exit Sub
        End Select
    End Sub

#End Region
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' キー入力時制御
    ''' </summary>
    Private Sub inputSelectData(ByVal sender As Control, ByVal e As EventArgs)

        'フォーカス設定
        dtbStartDt.ppDateBox.Focus()

        'キー項目入力制御
        If txtTBOX.Text = "" Then
            lblSEQ.Text = ""
            lblSEQ.Enabled = False
        ElseIf lblSEQ.Text = "" Then
            'TBOXIDのみの場合
            Me.Validate("key1")
            If Me.IsValid = False Then
                Exit Sub
            End If

            'txtSEQ.Enabled = True
            'DispMode = "ELS"
        Else
            '両キー項目入力済

            'キー入力値検証
            Me.Validate("key2")
            If Me.IsValid = False Then
                Exit Sub
            End If

            '項目活性制御
            'txtTBOX.Enabled = False
            'txtSEQ.Enabled = True
            dtbENDDt.ppEnabled = True
            dtbStartDt.ppEnabled = True
            ddlPERAT.Enabled = True

            '入力したキーに対応するデータ取得＆項目にセット
            stb.Clear()
            stb.Append(" SELECT [M38_TBOXID] ")
            stb.Append("       ,[M38_NL_CLS] ")
            stb.Append("       ,[M38_TBOXID_TRUN] ")
            stb.Append("       ,[M38_FROM_DT] ")
            stb.Append("       ,[M38_END_DT] ")
            stb.Append("       ,[M38_TBOX_TYPE] ")
            stb.Append("       ,[M38_VERSION] ")
            stb.Append("       ,[M38_HALL_CD] + ' : ' + [T01_HALL_NAME] AS HALL ")
            stb.Append("       ,[M38_PERAT_CLS] ")
            stb.Append("       ,[M38_DELETE_FLG] ")
            stb.Append("   FROM [SPCDB].[dbo].[M38_SPECIAL_SHOP] ")
            stb.Append("   LEFT JOIN [SPCDB].[dbo].[T01_HALL] AS T01 ")
            stb.Append("   ON   M38.M38_HALL_CD  = T01.T01_HALL_CD ")
            stb.Append("  WHERE [M38_TBOXID]      = '" & txtTBOX.Text & "'  ")
            stb.Append("    AND [M38_TBOXID_TRUN] = '" & lblSEQ.Text & "' ")
            dt = ClsSQL.getDataSetTable(stb.ToString, "SelectedRow")

            If dt.Rows.Count = 0 Then

                'Select Case sender.ID
                '    Case ddlAPPAGroup.ID
                '        ddlAPPAGroup.Focus()
                '    Case txtAPPACLS.ID
                '        FocusChange(sender, txtAPPACLSNM)
                '    Case Else

                'End Select

                'txtAPPACLSNM.Text = ""
                'txtSHORTNM.Text = ""
                'ddlSERCNT.SelectedValue = ""

                DispMode = "ADD"
                Exit Sub
            Else

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


                '編集エリアに値を反映
                lblKey1.Text = dt(0)(KeyName1).ToString
                lblKey2.Text = dt(0)(KeyName2).ToString
                lblKey3.Text = dt(0)(KeyName3).ToString
                lblFROM.Text = dt(0)("M38_FROM_DT").ToString.Substring(0, 10)
                lblEND.Text = dt(0)("M38_END_DT").ToString.Substring(0, 10)

                txtTBOX.Text = dt(0)("M38_TBOXID").ToString
                lblNL.Text = dt(0)(KeyName2).ToString
                If dt(0)("M38_TBOX_TYPE").ToString.Trim = "" Then
                    ddlSYS.SelectedIndex = 0
                ElseIf dt(0)("M38_TBOX_TYPE").ToString = "**" Then
                    ddlSYS.SelectedIndex = 0
                Else
                    ddlSYS.SelectedValue = dt(0)("M38_TBOX_TYPE").ToString
                End If
                txtVer.Text = dt(0)("M38_VERSION").ToString
                If dt(0)("M38_HALL_CD").ToString.Trim = "" Then
                    lblHALL.Text = ""
                Else
                    'lblHALL.Text = dt(0)("M38_HALL_CD").ToString + ":" + CType(rowData.FindControl("ホール名称"), TextBox).Text
                    lblHALL.Text = dt(0)("HALL").ToString
                End If

                lblSEQ.Text = dt(0)("M38_TBOXID_TRUN").ToString
                dtbStartDt.ppText = dt(0)("M38_FROM_DT").ToString.Substring(0, 10)
                dtbENDDt.ppText = dt(0)("M38_END_DT").ToString.ToString.Substring(0, 10)
                ddlPERAT.SelectedValue = dt(0)("M38_PERAT_CLS").ToString

                'TBOXID:21419011　黒鳥１２３の場合のみ活性
                If dt(0)("M38_TBOXID").ToString = "21419011" Then
                    ddlSYS.Enabled = True
                    txtVer.Enabled = True
                    'フォーカス設定
                    ddlSYS.Focus()
                Else
                    ddlSYS.Enabled = False
                    txtVer.Enabled = False
                    'フォーカス設定
                    lblSEQ.Focus()
                End If

                Select Case dt(0)("" & TableName.Substring(0, 4) & "DELETE_FLG").ToString
                    Case "0"
                        Master.ppBtnDelete.Text = "削除"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName)       '削除
                        Master.ppMainEnabled = True
                        Master.ppBtnUpdate.Enabled = True
                    Case "1"
                        Master.ppBtnDelete.Text = "削除取消"
                        Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                        Master.ppMainEnabled = False
                        Master.ppBtnUpdate.Enabled = False
                End Select

            End If

            'ボタンの使用制限
            Select Case Master.ppBtnDelete.Text
                Case "削除"
                    DispMode = "UPD"
                Case Else
                    DispMode = "DEL"
                    Master.ppBtnClear.Focus()
            End Select


        End If



    End Sub
    ''' <summary>
    '''検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Sub BtnSearchClear_Click()
        'ログ出力開始
        psLogStart(Me)



        'ddlday.ppDropDownList.SelectedIndex = -1
        ddldel.ppDropDownList.SelectedIndex = -1
        txtsTfrom.Text = ""
        txtsTto.Text = ""
        dtbStart.ppFromText = ""
        dtbStart.ppToText = ""
        dtbFin.ppFromText = ""
        dtbFin.ppToText = ""
        Master.ppchksDel.Checked = False
        ddlSNL.ppDropDownList.SelectedIndex = -1
        ddlSystem.SelectedIndex = -1
        txtsTfrom.Focus()

        'ログ出力終了
        psLogEnd(Me)
    End Sub
    ''' <summary>
    '''検索ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Sub BtnSearch_Click()
        'COMUPDM38-003
        clickflg = True

        If Page.IsValid = False Then
            If txtchangeflg = True Then
                CstVal_Enable("select")
            End If
            Exit Sub
        End If

        Page.Validate("Edit")
        If Page.IsValid = False Then
            If txtchangeflg = True Then
                CstVal_Enable("select")
            End If
            Exit Sub
        End If



        'COMUPDM38-003END
        stb.Clear()
        stb.Append(txtsTfrom.Text)
        stb.Append(txtsTto.Text)
        stb.Append(dtbStart.ppFromText)
        stb.Append(dtbStart.ppToText)
        stb.Append(dtbFin.ppFromText)
        stb.Append(dtbFin.ppToText)
        stb.Append(ddlSystem.SelectedValue.ToString)
        stb.Append(ddlSNL.ppSelectedValue.ToString)

        If stb.ToString = "" And ddldel.ppDropDownList.SelectedIndex = 0 Then
            SRCH = "clear"
            Exit Sub
        End If

        'ログ出力開始
        psLogStart(Me)

        stb.Clear()
        stb.Append(" WHERE")

        If ddldel.ppDropDownList.SelectedIndex = 1 Then '活動中
            stb.Append(" AND M38_DELETE_FLG = '0' ")
        ElseIf ddldel.ppDropDownList.SelectedIndex = 2 Then '削除
            stb.Append(" AND M38_DELETE_FLG = '1' ")
        End If
        'If ddlday.ppDropDownList.SelectedIndex = 1 Then '含む
        '    stb.Append(" AND M38_FROM_DT <= GETDATE() AND M38_END_DT >= GETDATE() ")
        'ElseIf ddlday.ppDropDownList.SelectedIndex = 2 Then '含まない
        '    stb.Append(" AND (M38_FROM_DT > GETDATE() OR M38_END_DT < GETDATE()) ")
        'End If

        If ddlSystem.SelectedValue <> "" Then 'システムコード検索
            stb.Append(" AND M38_TBOX_TYPE = '" & ddlSystem.SelectedValue.ToString & "'")
        End If

        If ddlSNL.ppSelectedValue <> "" Then 'NL区分検索
            'COMUPDM38-003 NL区分N選択時にJACKYも検索対象になるように変更
            If ddlSNL.ppSelectedValue.ToString = "N" Then
                stb.Append(" AND　( M38_NL_CLS = '" & ddlSNL.ppSelectedValue.ToString & "'")
                stb.Append(" OR M38_NL_CLS = 'J') ")
            Else
                stb.Append(" AND M38_NL_CLS = '" & ddlSNL.ppSelectedValue.ToString & "'")
            End If
            'COMUPDM38-003
        End If



        '期間検索
        '開始終了日付ともに入力時はエラー(バリデーションで弾く)

        Dim result As Date
        '開始日付検索
        If dtbStart.ppFromText = "" Then
            If dtbStart.ppToText = "" Then
            Else '-To
                If Date.TryParse(dtbStart.ppToText, result) = True Then
                    stb.Append(" AND M38_FROM_DT <= '" & dtbStart.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            End If
        Else
            If dtbStart.ppToText = "" Then
                ' From-
                If Date.TryParse(dtbStart.ppFromText, result) = True Then
                    stb.Append(" AND '" & dtbStart.ppFromText & "' <= M38_FROM_DT")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            Else 'From TO
                If Date.TryParse(dtbStart.ppFromText, result) = True And Date.TryParse(dtbStart.ppToText, result) = True Then
                    stb.Append(" AND '" & dtbStart.ppFromText & "' <= M38_FROM_DT AND  M38_FROM_DT <= '" & dtbStart.ppToText & "'")
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
                    stb.Append(" AND M38_END_DT <= '" & dtbFin.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            End If
        Else
            If dtbFin.ppToText = "" Then
                ' From-
                If Date.TryParse(dtbFin.ppFromText, result) = True Then
                    stb.Append(" AND '" & dtbFin.ppFromText & "' <= M38_END_DT")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            Else 'From TO
                If Date.TryParse(dtbFin.ppFromText, result) = True And Date.TryParse(dtbFin.ppToText, result) = True Then
                    stb.Append(" AND '" & dtbFin.ppFromText & "' <= M38_END_DT AND  M38_END_DT <= '" & dtbFin.ppToText & "'")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If
            End If
        End If




        'TBOXID
        Dim intFROM As Integer
        Dim intTO As Integer

        If txtsTfrom.Text = "" Then
            If txtsTto.Text = "" Then
            Else '-To
                If Integer.TryParse(txtsTto.Text, intTO) = True Then
                    stb.Append(" AND M38_TBOXID <= " & intTO & "")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            End If
        Else
            If txtsTto.Text = "" Then
                ' From-
                If Integer.TryParse(txtsTfrom.Text, intFROM) = True Then
                    stb.Append(" AND M38_TBOXID = " & intFROM & " ")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If

            Else 'From TO
                If Integer.TryParse(txtsTfrom.Text, intFROM) = True And Integer.TryParse(txtsTto.Text, intTO) = True Then
                    stb.Append(" AND " & intFROM & " <= M38_TBOXID AND  M38_TBOXID <= " & intTO & "")
                Else
                    stb.Append(" AND 1 = 2 ")
                End If
            End If
        End If


        stb.Replace("WHERE AND", "WHERE")
        SRCH = stb.ToString

        Me.ViewState.Add("SrchDelFlg", ClsSQL.GetDltCheckNum(Master.ppchksDel))

        txtsTfrom.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub
    ''' <summary>
    ''' ドロップダウンリスト作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        Dim clsDataConnect As New ClsCMDataConnect
        Dim clsMst As New ClsMSTCommon

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
                Me.ddlSystem.DataSource = objDs.Tables(1)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
                Me.ddlSystem.Items.Insert(0, "**:ダミー") 'ダミーを追加
                Me.ddlSystem.Items(0).Value = "**"
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
    'COMUPDM38-003
    Private Sub CstVal_Enable(ByVal strVal As String)
        Dim panelkey As ValidationSummary
        ' Dim panelsearch As ValidationSummary
        panelkey = DirectCast(Master.FindControl("ValidSumKey"), ValidationSummary)
        ' panelsearch = DirectCast(Master.FindControl("ValidSumSearch"), ValidationSummary)
        Select Case strVal.ToString
            Case "txt"
                CstmVal_TBOX.Visible = True
                panelkey.Visible = True
                clickflg = False
                txtchangeflg = False
                'Case "key"
                '    CstmVal_TBOX.Visible = False
                '    panelkey.Visible = False
                '    clickflg = False
                '    txtchangeflg = False
            Case "select"
                CstmVal_TBOX.Visible = False
                panelkey.Visible = False
                clickflg = False
            Case Else
                CstmVal_TBOX.Visible = True
                panelkey.Visible = True
        End Select
    End Sub
    'COMUPDM38-003END

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
