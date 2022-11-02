'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　休日マスタ管理
'*　ＰＧＭＩＤ：　COMUPDM36
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMUPDM36-000      2016/05/18      加賀　　　新規(SMTHDY001から移行)

Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SQL_DBCLS_LIB


Public Class COMUPDM36
    Inherits System.Web.UI.Page

    '定数
    Const MasterName As String = "休日マスタ"     'マスタ名
    Const PGMID As String = "COMUPDM36"           'マスタID
    'ViewState引数用文字列
    Const VS_CALENDAR_YAER As String = "CALENDAR_YAER"    'カレンダーに表示している年
    Const VS_CALENDAR_MONTH As String = "CALENDAR_MONTH"  'カレンダーに表示している月

    '変数
    Dim mclsDB As New ClsSQLSvrDB
    Dim clsDataConnect As New ClsCMDataConnect
    Dim objStack As StackFrame

    'カレンダーボタンの配列
    Dim btnsCalendar() As Button

    'ボタンカラー
    Dim colorDefault As Drawing.Color = Drawing.Color.White             '平日 
    Dim colorHoliday As Drawing.Color = Drawing.Color.PaleVioletRed     '休日


    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, PGMID)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            'カレンダーの日付ボタンを配列に格納
            btnsCalendar = {Button01, Button02, Button03, Button04, Button05, Button06, Button07 _
                          , Button08, Button09, Button10, Button11, Button12, Button13, Button14 _
                          , Button15, Button16, Button17, Button18, Button19, Button20, Button21 _
                          , Button22, Button23, Button24, Button25, Button26, Button27, Button28 _
                          , Button29, Button30, Button31, Button32, Button33, Button34, Button35 _
                          , Button36, Button37, Button38, Button39, Button40, Button41, Button42}

            'ボタン イベント設定
            AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click  '検索
            AddHandler Master.ppBtnInsert.Click, AddressOf btnUpdate_Click  '登録

            '初回のみ実行　画面設定
            If Not IsPostBack Then

                'プログラムＩＤ、画面名、件数設定
                Master.ppProgramID = PGMID
                Master.ppTitle = MasterName
                Master.ppCount = "0"

                'パンくずリスト設定
                Master.ppBCList = pfGet_BCList("マスタ管理メニュー", MasterName)

                '未使用ボタンを非表示
                Master.ppBtnSrcClear.Visible = False    '検索クリア
                Master.ppBtnUpdate.Visible = False      '更新
                Master.ppBtnDelete.Visible = False      '削除
                Master.ppBtnClear.Visible = False       'クリア

                '登録ボタンのサイズを変更
                Master.ppBtnInsert.Height = 50
                Master.ppBtnInsert.Width = 100

                '該当件数非表示
                Master.ppCount_Visible = False

                '検索ボタン 変更確認スクリプト付与(戻りfalseでポストバック無し)
                Master.ppBtnSearch.OnClientClick = "return AlertDispose()"

                'カレンダーボタン設定
                For Each btnCalendar As Button In btnsCalendar
                    '色設定
                    btnCalendar.BackColor = colorDefault
                    btnCalendar.Height = 65
                    btnCalendar.Width = 65
                    'フラグ変更スクリプト付与
                    btnCalendar.Attributes.Add("onClick", "ChangeFlg()")
                Next

                '現在の年月の取得
                Dim strDateNow() As String = Date.Now.ToString("yyyy/M").Split("/")

                '年月設定
                txtYear.ppText = strDateNow(0)
                ddlMonth.SelectedValue = strDateNow(1)

                'カレンダー取得・設定
                If getCalendar(strDateNow(0), strDateNow(1)) = False Then
                    Exit Sub
                End If

                'フォーカス設定
                txtYear.ppTextBox.Focus()

                'JavaScript用にClass設定
                txtYear.ppTextBox.CssClass = "JS_Year"

                'グリッドボタンイベントをフルポストバックに変更
                'Master.ppScripManeger.RegisterPostBackControl(grvList)
            End If

        Catch ex As Exception
            '初期化失敗
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後) '画面の初期化に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' GRID DataBound
    ''' </summary>
    ''' <remarks>一覧選択ボタンに確認＆入力年月変更スクリプトを付与</remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        objStack = New StackFrame

        Try
            Dim ctrlBtn, ctrlTxt As Control
            Dim strYM() As String

            '行ループ
            For Each GrvRow As GridViewRow In grvList.Rows
                'ﾎﾞﾀﾝとTextBoxを取得
                ctrlBtn = GrvRow.Cells(0).Controls(0)
                ctrlTxt = GrvRow.Cells(1).Controls(0)

                'ボタンに確認＆入力年月変更スクリプト付与
                If TypeOf ctrlBtn Is Button AndAlso TypeOf ctrlTxt Is TextBox Then
                    '年月取得
                    strYM = DirectCast(ctrlTxt, TextBox).Text.Split("/")

                    'UpdatePanel内だとTrue戻りの場合にイベントが発生しない為、onClickとフラグで処理実行可否を制御
                    'DirectCast(ctrl, Button).OnClientClick = "return AlertDispose()" 
                    DirectCast(ctrlBtn, Button).Attributes.Add("onClick", "AlertDispose(" + strYM(0) + "," + strYM(1) + ")")
                End If
            Next
        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧の生成")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 検索ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)
        objStack = New StackFrame

        Try
            Dim intDate As Integer

            '年の形式チェック
            If Integer.TryParse(txtYear.ppText, intDate) Then
                Select Case intDate
                    Case 1900 To 9999
                        'OK
                    Case Else
                        'NG
                        txtYear.psSet_ErrorNo("4001", "年", "1900～9999の数値") '形式エラー{0} は {1} で入力して下さい。
                        Exit Sub
                End Select
            Else
                'NG
                txtYear.psSet_ErrorNo("4001", "年", "1900～9999の数値") '形式エラー{0} は {1} で入力して下さい。
                Exit Sub
            End If

            'カレンダー取得
            getCalendar(txtYear.ppText, ddlMonth.SelectedValue)
        Catch ex As Exception
            'エラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'ログ出力終了
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' カレンダー日付ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As Object, e As EventArgs) Handles Button01.Click, Button02.Click, Button03.Click, Button04.Click, Button05.Click, Button06.Click, Button07.Click _
                                                                         , Button08.Click, Button09.Click, Button10.Click, Button11.Click, Button12.Click, Button13.Click, Button14.Click _
                                                                         , Button15.Click, Button16.Click, Button17.Click, Button18.Click, Button19.Click, Button20.Click, Button21.Click _
                                                                         , Button22.Click, Button23.Click, Button24.Click, Button25.Click, Button26.Click, Button27.Click, Button28.Click _
                                                                         , Button29.Click, Button30.Click, Button31.Click, Button32.Click, Button33.Click, Button34.Click, Button35.Click _
                                                                         , Button36.Click, Button37.Click, Button38.Click, Button39.Click, Button40.Click, Button41.Click, Button42.Click
        '押下されたﾎﾞﾀﾝを取得
        Dim btnSelected As Button = DirectCast(sender, Button)

        'ボタン押下でカラー変更
        If btnSelected.BackColor = colorDefault Then
            btnSelected.BackColor = colorHoliday
        Else
            btnSelected.BackColor = colorDefault
        End If

        'フォーカス設定
        DirectCast(sender, Button).Focus()

        ''変更フラグ設定
        'ViewState(VS_IsChanged) = True

    End Sub

    ''' <summary>
    ''' 一覧選択ボタン　押下
    ''' </summary>
    ''' <remarks>選択した年月をカレンダーに反映</remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As CommandEventArgs) Handles grvList.RowCommand

        'ログ出力開始
        psLogStart(Me)
        objStack = New StackFrame

        Try
            If e.CommandName = "Select" Then

                '隠しラベルからJavaScriptのConfirm選択結果を取得
                If txtOKCancel.Text = "Cancel" Then
                    'Cancelの場合、処理しない
                    txtOKCancel.Text = "OK"
                    Exit Sub
                End If

                Dim ctrlGrv As Control = grvList.Rows(Convert.ToInt32(e.CommandArgument)).Cells(1).Controls(0)
                Dim strDate() As String
                If TypeOf ctrlGrv Is TextBox Then
                    '選択した年月を取得
                    strDate = DirectCast(ctrlGrv, TextBox).Text.Split("/")
                    'カレンダー取得
                    getCalendar(strDate(0), strDate(1))
                Else
                    'エラーメッセージ
                    psMesBox(Me, "30009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "選択", "年月取得に失敗しました。") '{0}ボタンクリック時の処理に失敗しました。\nエラー内容：{1}
                End If
            End If
        Catch ex As Exception
            'エラーメッセージ
            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "選択", "システムエラー") '{0}ボタンクリック時の処理に失敗しました。\nエラー内容：{1}
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'ログ出力終了
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' 更新ボタン　押下
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)

        Try
            'ログ出力開始
            psLogStart(Me)
            objStack = New StackFrame

            'DB接続
            If clsDataConnect.pfOpen_Database(mclsDB.mobjDB) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'カレンダーの日付ボタンを配列に格納
            btnsCalendar = {Button01, Button02, Button03, Button04, Button05, Button06, Button07 _
                          , Button08, Button09, Button10, Button11, Button12, Button13, Button14 _
                          , Button15, Button16, Button17, Button18, Button19, Button20, Button21 _
                          , Button22, Button23, Button24, Button25, Button26, Button27, Button28 _
                          , Button29, Button30, Button31, Button32, Button33, Button34, Button35 _
                          , Button36, Button37, Button38, Button39, Button40, Button41, Button42}

            '休日マスタ取得
            Dim listHoliday As New List(Of String)
            Dim objSqlCmd As SqlCommand

            For Each btn As Button In btnsCalendar
                If btn.Enabled = True AndAlso btn.BackColor = colorHoliday Then
                    '休日格納
                    listHoliday.Add(btn.Text)
                End If
            Next

            'SQLコマンド設定
            objSqlCmd = New SqlCommand("SPCDB.dbo.COMUPDM36_U1", mclsDB.mobjDB)
            objSqlCmd.CommandType = CommandType.StoredProcedure

            'SQLパラメータ設定
            With objSqlCmd
                '年
                .Parameters.Add("prmYear", SqlDbType.NVarChar, 4)
                .Parameters(0).Direction = ParameterDirection.Input
                .Parameters(0).Value = ViewState(VS_CALENDAR_YAER)
                '月
                .Parameters.Add("prmMonth", SqlDbType.NVarChar, 2)
                .Parameters(1).Direction = ParameterDirection.Input
                .Parameters(1).Value = Integer.Parse(ViewState(VS_CALENDAR_MONTH)).ToString("00")
                '日
                .Parameters.Add("prmDay", SqlDbType.NVarChar, 2)
                .Parameters(2).Direction = ParameterDirection.Input
                .Parameters(2).Value = "0"
                'USERID
                .Parameters.Add("prmUserID", SqlDbType.NVarChar, 20)
                .Parameters(3).Direction = ParameterDirection.Input
                .Parameters(3).Value = Session(P_SESSION_USERID)
                '処理区分
                .Parameters.Add("prmProc", SqlDbType.Int, 1)
                .Parameters(4).Direction = ParameterDirection.Input
                .Parameters(4).Value = 0
            End With

            'トランザクション開始
            Using SqlTran As SqlTransaction = mclsDB.mobjDB.BeginTransaction
                Try
                    'トランザクション設定
                    objSqlCmd.Transaction = SqlTran

                    '対象年月データの論理削除
                    objSqlCmd.ExecuteNonQuery()

                    '休日登録
                    For Each strDay As String In listHoliday

                        'SQLパラメータ設定
                        With objSqlCmd
                            .Parameters(2).Value = strDay   '日
                            .Parameters(4).Value = 1        '処理区分
                        End With

                        '実行
                        objSqlCmd.ExecuteNonQuery()
                    Next

                    'コミット
                    SqlTran.Commit()

                Catch ex As SqlException
                    'ロールバック
                    SqlTran.Rollback()

                    'エラーメッセージ
                    If ex.Number = -2 Then
                        'SQLタイムアウト
                        psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】" + MasterName, ex.HResult.ToString) '{0}の登録に失敗しました。\nエラーコード：{1}
                    Else
                        'SQLエラー
                        psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + MasterName, ex.HResult.ToString) '{0}の登録に失敗しました。\nエラーコード：{1}
                    End If

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

                    Exit Sub
                Finally
                    'SQLコマンド破棄
                    objSqlCmd.Dispose()
                End Try
            End Using

            'カレンダー取得・設定
            If getCalendar(ViewState(VS_CALENDAR_YAER), ViewState(VS_CALENDAR_MONTH)) = False Then
                Exit Sub
            End If

            '完了メッセージ
            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName) '{0}の登録が完了しました。

        Catch ex As Exception
            'エラー
            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName, ex.HResult.ToString) '{0}の登録に失敗しました。\nエラーコード：{1}
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB接続クローズ
            Call mclsDB.psDB_Close()
            'ログ出力終了
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' カレンダー・一覧 取得
    ''' </summary>
    ''' <remarks>配置されたボタンをカレンダー形式で表示する</remarks>
    Private Function getCalendar(ByVal strYear As String, ByVal strMonth As String) As Boolean

        Dim dtNow As Date           '月初
        Dim dtEnd As Date           '月末
        Dim dtCNT As Date           'カウントアップ用
        Dim intWeek As Integer      '月初の曜日
        Dim CALENDAR(41) As Date    'ボタンに割り当てる日付の配列
        Dim dstSelect As New DataSet '休日マスタ取得用

        objStack = New StackFrame
        getCalendar = False

        Try
            If Date.TryParse(strYear & "/" & strMonth & "/01", dtNow) Then

                '計算用変数設定
                dtCNT = dtNow

                '月初の曜日 取得
                intWeek = dtNow.DayOfWeek

                '表示対象の日付を取得
                If strYear = "9999" AndAlso strMonth = "12" Then
                    'オーバーフロー対策(9999年12月)
                    dtEnd = Date.Parse("9999/12/31")
                    For i As Integer = 0 To 41
                        Select Case i
                            Case 0 To intWeek - 1
                                '前月
                                CALENDAR(i) = dtCNT.AddDays(i - intWeek)
                            Case intWeek To 30
                                '当月
                                CALENDAR(i) = dtCNT
                                dtCNT = dtCNT.AddDays(1)
                            Case Else
                                '前月
                                CALENDAR(i) = Nothing
                        End Select
                    Next
                Else
                    dtEnd = dtNow.AddMonths(1).AddDays(-1)
                    For i As Integer = 0 To 41
                        If i < intWeek Then
                            '前月
                            CALENDAR(i) = dtCNT.AddDays(i - intWeek)
                        Else
                            '当月
                            CALENDAR(i) = dtCNT
                            dtCNT = dtCNT.AddDays(1)
                        End If
                    Next
                End If

                '全てのボタンに日付を表示
                For i As Integer = 0 To 41
                    If CALENDAR(i) = Nothing Then
                        '日付が無い？
                        btnsCalendar(i).Enabled = False
                        btnsCalendar(i).Text = String.Empty
                        btnsCalendar(i).BackColor = Nothing
                    Else
                        '日付表示
                        btnsCalendar(i).Text = CALENDAR(i).ToString("%d")

                        '当月以外は非活性
                        If CALENDAR(i) < dtNow OrElse CALENDAR(i) > dtEnd Then
                            btnsCalendar(i).Enabled = False
                            'btnsCalendar(i).BackColor = colorDefault
                            btnsCalendar(i).BackColor = Nothing
                            Continue For
                        End If

                        '土日の背景色を設定
                        Select Case CALENDAR(i).DayOfWeek
                            Case 0  '日曜日 赤
                                btnsCalendar(i).Enabled = False
                                btnsCalendar(i).BackColor = colorHoliday
                            Case 6  '土曜日 青
                                btnsCalendar(i).Enabled = False
                                btnsCalendar(i).BackColor = colorHoliday
                            Case Else
                                btnsCalendar(i).Enabled = True
                                btnsCalendar(i).BackColor = colorDefault
                        End Select
                    End If
                Next

                'カレンダー最終行の表示/非表示設定
                If CALENDAR(35) > dtEnd Then
                    trwCalendar.Visible = False
                Else
                    trwCalendar.Visible = True
                End If

                '休日マスタからデータ取得
                Using objSqlCmd As SqlCommand = New SqlCommand("SPCDB.dbo.COMUPDM36_S1")
                    'SQLコマンド設定
                    With objSqlCmd
                        .Parameters.Add("prmYear", SqlDbType.NVarChar, 4)
                        .Parameters(0).Direction = ParameterDirection.Input
                        .Parameters(0).Value = strYear
                        .Parameters.Add("prmMonth", SqlDbType.NVarChar, 2)
                        .Parameters(1).Direction = ParameterDirection.Input
                        .Parameters(1).Value = dtNow.Month.ToString("00")
                        .Parameters.Add("prmDelFlg", SqlDbType.NVarChar, 1)
                        .Parameters(2).Direction = ParameterDirection.Input
                        .Parameters(2).Value = "0"
                    End With

                    'データ取得実行
                    If execStoredProcedure(objSqlCmd, dstSelect, MasterName) = False Then
                        Exit Function
                    End If
                End Using

                '取得データの表示
                If dstSelect IsNot Nothing AndAlso dstSelect.Tables.Count > 1 Then
                    '取得した休日をカレンダーに反映
                    For Each row As DataRow In dstSelect.Tables(0).Rows
                        btnsCalendar(intWeek + Integer.Parse(row.Item(2)) - 1).BackColor = colorHoliday
                    Next

                    '一覧バインド
                    grvList.DataSource = dstSelect.Tables(1)
                    grvList.DataBind()
                End If

                '年月の表示
                lblCarenderMonth.Text = strYear & "年" & strMonth.TrimStart("0") & "月"

                '表示年月の保存
                ViewState(VS_CALENDAR_YAER) = strYear
                ViewState(VS_CALENDAR_MONTH) = strMonth

                '成功
                Return True

            Else
                '日付変換失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "日付変換")    '{0}の検索に失敗しました。'{0}に失敗しました。
            End If

        Catch ex As SqlException

            If ex.Number = -2 Then
                'SQLタイムアウト
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】" + MasterName) '{0}の検索に失敗しました。
            Else
                'SQLエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + MasterName)       '{0}の検索に失敗しました。
            End If

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "カレンダー")    '{0}の出力に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB接続クローズ
            Call mclsDB.psDB_Close()
            'データセットの破棄
            Call mclsDB.psDisposeDataSet(dstSelect)
        End Try

    End Function

    ''' <summary>
    ''' ストアド実行(SELECT)
    ''' </summary>
    ''' <remarks></remarks>
    Private Function execStoredProcedure(ByVal SqlCmd As SqlCommand, ByRef dstSelect As DataSet, ByVal strDataName As String) As Boolean

        execStoredProcedure = False
        objStack = New StackFrame

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(mclsDB.mobjDB) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Function
            End If

            'SQLコマンド設定
            SqlCmd.Connection = mclsDB.mobjDB
            SqlCmd.CommandType = CommandType.StoredProcedure

            'データ取得
            If mclsDB.pfGet_DataSet(SqlCmd, dstSelect) = False Then
                Call mclsDB.psDisposeDataSet(dstSelect)
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strDataName)    '{0}の検索に失敗しました。
                Exit Function
            End If

            'SQLコマンド破棄
            SqlCmd.Dispose()

            Return True

        Catch ex As SqlException
            If ex.Number = -2 Then
                'SQLタイムアウト
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLタイムアウト】" + strDataName) '{0}の検索に失敗しました。
            Else
                'SQLエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "【SQLエラー】" + strDataName)       '{0}の検索に失敗しました。
            End If

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            'エラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strDataName)       '{0}の検索に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB接続クローズ
            Call mclsDB.psDB_Close()
        End Try

    End Function

End Class