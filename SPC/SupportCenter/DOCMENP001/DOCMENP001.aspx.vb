'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜サポートセンタ＞
'*　処理名　　：　サポートセンタ検収書
'*　ＰＧＭＩＤ：　DOCMENP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.20　：　酒井
'*  変　更　　：　2017.01.25　：　伯野
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DOCMENP001-001     2015/07/06      稲葉       締め処理済みの場合の「印刷」「ＣＳＶ」ボタン非活性処理を除外
'DOCMENP001-002     2015/07/06      稲葉       ＰＤＦ、ＣＳＶファイルが無かった場合のメッセージ処理変更
'DOCMENP001-003     2015/08/04      稲葉       当月集計時、処理したデータが存在しない場合はＣＳＶを作成しない
'DOCMENP001-004     2015/08/06      栗原       CSVの項目に検収先営業所フッターを追加　DOCMENP001_S14　DOCREP002
'DOCMENP001-005     2016/01/25      栗原       印刷時、業者情報をマスタ参照するように変更
'DOCMENP001-005     2016/04/20      栗原       帳票レイアウト変更依頼に伴い、プログラム全面改修。
'DOCMENP001-006     2017/01/25      伯野       帳票レイアウト変更依頼に伴い、プログラム改修。
'                                              blnNewFlgをBooleanからStringに変更
'                                                True→"2" False→"2" 2016/10以降→"3"
'                                              システム運用管理者の項目を無効化

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataLink
'Imports SPC.Global_asax
Imports System.Globalization
Imports System
Imports System.IO
Imports System.String
Imports SPC.ClsCMExclusive

#End Region

Public Class DOCMENP001
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

    'エラーコード
    Const sCnsErr_0001 As String = "0001"
    Const sCnsErr_0002 As String = "0002"
    Const sCnsErr_0003 As String = "0003"
    Const sCnsErr_0004 As String = "0004"
    Const sCnsErr_0005 As String = "0005"

    'サポートセンタ検収書 画面のパス
    Const sCnsDOCMENP001 As String = "~/" & P_SPC & "/" &
                                        P_FUN_DOC & P_SCR_MEN & P_PAGE & "001.aspx"

    Const sCnsProgid As String = "DOCMENP001"
    Const sCnsSqlid_S1 As String = "DOCMENP001_S1"
    Const sCnsSqlid_S2 As String = "DOCMENP001_S2"
    Const sCnsSqlid_S3 As String = "DOCMENP001_S3"
    Const sCnsSqlid_S4 As String = "DOCMENP001_S4"
    Const sCnsSqlid_S5 As String = "DOCMENP001_S5"
    Const sCnsSqlid_S6 As String = "DOCMENP001_S6"
    Const sCnsSqlid_S7 As String = "DOCMENP001_S7"
    Const sCnsSqlid_S8 As String = "DOCMENP001_S8"
    Const sCnsSqlid_I1 As String = "DOCMENP001_I1"
    Const sCnsSqlid_U1 As String = "DOCMENP001_U1"
    Const sCnsSqlid_U2 As String = "DOCMENP001_U2"

    Const sCnsUpButon As String = "更新"

    Const sCnsbtnUpdate As String = "btnUpdate"             '更新
    Const sCnsShimeButon As String = "締め"
    Const sCnsKaijyo As String = "解除"
    Const sCnsSumButon As String = "当月集計"

    '一覧ボタン名
    Const sCnsbtnCvs As String = "btnCsv"                'ＣＳＶ
    Const sCnsbtnPrint As String = "btnPrint"            '印刷

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
    ''' <summary>
    ''' 一覧列定義
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum eColGrv
        印刷 = 0
        ＣＳＶ
        検収書
        検収年月
        総計
        検収締め
        請求締め
    End Enum

    Private Enum 人件費表示状態
        ラベル１
        ラベル２
        テキストボックス
    End Enum

    Private Enum 帳票
        サポセン検収書
        '券・サ・BB1
        券サBB1
        券売機明細
        サンド明細
        カードDB
        TBOX吸上げ
    End Enum

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
    Private Property mpPanelVisible As 人件費表示状態
        Get
            '            If pnlJinkenhiNew.Visible = True AndAlso pnlJinkenhiOld.Visible = False Then
            If pnlJinkenhi201610.Visible = True Then
                Return 人件費表示状態.テキストボックス
            ElseIf pnlJinkenhiNew.Visible = True Then
                Return 人件費表示状態.ラベル２

            Else
                Return 人件費表示状態.ラベル１
            End If
        End Get
        Set(表示状態 As 人件費表示状態)
            If 表示状態 = 人件費表示状態.テキストボックス Then
                pnlJinkenhi201610.Visible = True
                pnlJinkenhiNew.Visible = False
                pnlJinkenhiOld.Visible = False
            ElseIf 表示状態 = 人件費表示状態.ラベル２ Then
                pnlJinkenhi201610.Visible = False
                pnlJinkenhiNew.Visible = True
                pnlJinkenhiOld.Visible = False
            ElseIf 表示状態 = 人件費表示状態.ラベル１ Then
                pnlJinkenhi201610.Visible = False
                pnlJinkenhiNew.Visible = False
                pnlJinkenhiOld.Visible = True
            End If
        End Set
    End Property


#End Region

#Region "イベントプロシージャ"

#Region "■ Init"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, sCnsProgid)

    End Sub

#End Region

#Region "■ Page_Load"

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try

            'イベントハンドラ設定
            msAddHandler()
            msAddMoneyColc()

            Master.ppCount_Visible = False
            If Not IsPostBack Then  '初回表示
                '初期表示
                Me.mpPanelVisible = 人件費表示状態.テキストボックス

                '開始ログ出力
                psLogStart(Me)

                'ユーザを記憶
                ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)

                'ページ初期化
                msPageClear()

                SetFocus(txtKensyuYm.ppDateBox)

                msSetEnable("0")

            Else

                msDispSetJinkenhi()
            End If

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '読み込み失敗は画面を閉じる
            psClose_Window(Me)
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

#End Region

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

#Region "■ ボタン"

    ''' <summary>
    ''' 当月集計ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSyukei_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            If Page.IsValid = False Then
                Return
            End If

            ''状態チェック
            'If mfChkJotai(3) = False Then
            '    Return
            'End If

            '当月集計処理
            msSyukei()

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                           objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try


    End Sub

    ''' <summary>
    ''' 締め/締め解除ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnShime_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim strKensyuYm As String = Me.txtKensyuYm.ppText.Replace("/", "")
        Dim strShime As String = ""
        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            If Page.IsValid = False Then
                Return
            End If

            If Master.Master.ppRigthButton2.Text = "締め" Then
                strShime = "1"
            Else
                strShime = "0"
            End If

            '締め/締め解除処理
            If mfShime(strShime) = False Then
                Throw New Exception()
            End If

            If strShime = "1" Then
                AllPrint()
            End If

            '再検索
            msDataRead(strKensyuYm)

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' 検索ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        objStack = New StackFrame
        Dim strKensyuYM As String = txtKensyuYm.ppText.Replace("/", "").Trim

        Try
            '開始ログ出力
            psLogStart(Me)

            '入力検証
            Page.Validate("Kensyu")

            If IsValid() Then
                If strKensyuYM <= 201407 Then
                    '過去データの為作成不可
                    psMesBox(Me, "30029", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検収対象データが存在しません。", "年月")
                Else
                    Dim drBeforeShime As DataRow = mfGetBeforeShimeJotai()
                    If drBeforeShime Is Nothing Then
                        If strKensyuYM = "201408" Then  '特例として2014/08のみ表示する
                            '読み込み
                            msDataRead(txtKensyuYm.ppText.Replace("/", "").Trim)
                        Else
                            '作成不可能
                            psMesBox(Me, "30029", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "前月の検収データが存在しません。", "検収年月")
                        End If
                    ElseIf drBeforeShime("請求区分") = "1" Then
                        '前月の請求が完了している為作成可能
                        '読み込み
                        msDataRead(txtKensyuYm.ppText.Replace("/", "").Trim)
                    ElseIf drBeforeShime("請求書締め") = "0" OrElse drBeforeShime("請求区分") = "0" OrElse drBeforeShime("検収区分") = "0" Then
                        '作成不可能
                        psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "検収書及び請求書の締め状況を確認して下さい。")
                    Else
                        '作成不可能
                        psMesBox(Me, "30029", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "前月の検収データが存在しません。", "検収年月")
                    End If
                End If
            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' 検索条件クリアボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            '初期表示
            Me.mpPanelVisible = 人件費表示状態.テキストボックス

            'ページ初期化
            msPageClear()

            '活性制御
            msSetEnable("0")

            SetFocus(txtKensyuYm.ppDateBox)

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' 人件費登録ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnJinkenhiIns_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            Page.Validate("Jinkenhi")
            If Page.IsValid Then
                msMasterUpdate()
            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                           objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

    Private Sub btnJippi_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            Page.Validate("Jippi")
            If Page.IsValid Then
                msWrkUpdate(sender.text)
            End If

            msSetEnable(mfSettingEnableMode())
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                           objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try


    End Sub

#End Region

#Region "■ 一覧の選択ボタン押下処理"

    ''' <summary>
    ''' 一覧の選択ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        'ボタン名が印刷とCSVでない場合は処理を抜ける
        If e.CommandName <> "btnPrint" And e.CommandName <> "btnCsv" Then
            Exit Sub
        End If

        objStack = New StackFrame

        Try
            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行
            Dim CsvData As New ArrayList
            Dim intCnt As Integer = 0
            Dim strArrayList As String() = Nothing
            Dim drShime As DataRow = mfGetShimeJotai()

            'ログ出力開始
            psLogStart(Me)

            '検収月を設定
            Dim strKensyuYm As String = Me.txtKensyuYm.ppText.Replace("/", "")
            '            Dim blnNewFlg As Boolean
            Dim blnNewFlg As String = "3"
            If strKensyuYm >= 201610 Then
                blnNewFlg = "3"
            ElseIf strKensyuYm >= 201604 Then
                blnNewFlg = "2"
            Else
                blnNewFlg = "1"
            End If
            If drShime("切替区分") = 1 Then
                '                blnNewFlg = "3"
                If strKensyuYm >= "201610" Then
                    blnNewFlg = "3"
                Else
                    blnNewFlg = "2"
                End If
            End If

            Select Case e.CommandName
                Case sCnsbtnPrint     '印刷
                    Dim dsWrk As New DataSet
                    Select Case CType(rowData.FindControl("検収書"), TextBox).Text
                        Case "サポートセンタ運用の報告書兼検収書"
                            '検収書のみ2016/03から許容する
                            If strKensyuYm >= "201610" Then
                                blnNewFlg = "3"
                            ElseIf strKensyuYm >= "201603" Then
                                blnNewFlg = "2"
                            Else
                                blnNewFlg = "1"
                            End If
                            If drShime("切替区分") = 1 Then
                                If strKensyuYm >= "201610" Then
                                    blnNewFlg = "3"
                                Else
                                    blnNewFlg = "2"
                                End If
                            End If
                            If blnNewFlg = "3" Then
                                dsWrk = mfGetReportDataset(帳票.サポセン検収書)
                                If dsWrk.Tables(0).Rows.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("サポートセンタ運用の報告書兼検収書", New DOCREP002_2, dsWrk)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            ElseIf blnNewFlg = "2" Then
                                ms_GetCSVData("0941CL", "サポートセンタ運用の報告書兼検収書_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("サポートセンタ運用の報告書兼検収書", CsvData, intCnt, New DOCREP002_1, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            Else
                                ms_GetCSVData("0941CL", "サポートセンタ運用の報告書兼検収書_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("サポートセンタ運用の報告書兼検収書", CsvData, intCnt, New DOCREP002, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            End If

                        Case "券売機・サンドBB1読み出し処理件数"

                            Dim strKensyuYYYY As String = String.Empty
                            Dim strKensyuMM As String = String.Empty

                            Select Case strKensyuYm.Substring(4, 2)
                                Case "01", "02", "03"
                                    strKensyuYYYY = (Integer.Parse(strKensyuYm.Substring(0, 4)) - 1).ToString
                                Case Else
                                    strKensyuYYYY = strKensyuYm.Substring(0, 4)
                            End Select

                            '4月から翌年の3月までのCSVを順番に取り込む
                            Dim CsvDataWrk As New ArrayList
                            Dim strArrayListWrk As String() = Nothing
                            Dim blnIsExistsData As Boolean = False
                            Dim strTmpLine As String = String.Empty
                            Dim TimeStamp As String = DateTime.Now.ToString() & "," & strKensyuYYYY & "," & strKensyuMM
                            For zz As Integer = 1 To 12
                                If zz < 10 Then
                                    strKensyuMM = (zz + 3).ToString("00")
                                Else
                                    If zz = 10 Then
                                        strKensyuYYYY = (Integer.Parse(strKensyuYYYY) + 1).ToString
                                    End If
                                    strKensyuMM = (zz - 9).ToString("00")
                                End If

                                '初期化
                                CsvDataWrk.Clear()

                                'CSVファイル取り込み.
                                strTmpLine = String.Empty

                                '日付によって処理変更
                                If blnNewFlg = "3" Then
                                    'Dim dsTmp As DataSet = mfGetReportDataset(帳票.券・サ・BB1, strKensyuYYYY + strKensyuMM)
                                    Dim dsTmp As DataSet = mfGetReportDataset(帳票.券サBB1, strKensyuYYYY + strKensyuMM)
                                    If dsTmp Is Nothing OrElse dsTmp.Tables(0).Rows.Count = 0 Then
                                        strTmpLine = DateTime.Now.ToString("yyyy/MM/dd") & "," & strKensyuYYYY & "," & strKensyuMM & ",券売機,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,サンド,,,,,計,,,,"
                                    Else
                                        Dim dsCSVStyle As DataSet = mfConvertCSVStyle(dsTmp)
                                        Dim strTmp As Object() = dsCSVStyle.Tables(0).Rows(0).ItemArray
                                        ReDim strArrayListWrk(dsCSVStyle.Tables(0).Columns.Count - 1)

                                        Dim intI As Integer = 0
                                        For Each clm As DataColumn In dsCSVStyle.Tables(0).Columns
                                            strArrayListWrk(intI) = clm.ColumnName.ToString
                                            intI += 1
                                        Next

                                        strTmpLine = String.Join(",", strTmp)
                                        blnIsExistsData = True
                                    End If
                                Else
                                    If ms_GetCSVData("0942CL", "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYYYY & strKensyuMM & "]", CsvDataWrk, intCnt, strArrayListWrk) = False Then
                                        strTmpLine = DateTime.Now.ToString("yyyy/MM/dd") & "," & strKensyuYYYY & "," & strKensyuMM & ",券売機,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,サンド,,,,,計,,,,"
                                    Else
                                        strTmpLine = CsvDataWrk(0).ToString
                                        blnIsExistsData = True
                                    End If
                                End If

                                CsvData.Add(strTmpLine)

                                '■繰り返し処理の中で一番長いstrArrayListWrkをstrArrayListに昇格
                                If strArrayListWrk IsNot Nothing Then
                                    If strArrayList Is Nothing Then
                                        strArrayList = strArrayListWrk
                                    Else
                                        If strArrayList.Length < strArrayListWrk.Length Then
                                            strArrayList = strArrayListWrk
                                        End If
                                    End If
                                End If

                            Next

                            'データが一個もない
                            If blnIsExistsData = False Then
                                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                Exit Sub
                            End If

                            If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                'CSVのままのデータ形態では印刷ができないので加工する
                                Dim PrData As New ArrayList
                                Dim strCsv As String
                                Dim strPrTitle() As String
                                Dim strPrData() As String
                                Dim dt As New DataTable
                                Dim intMAX As Integer
                                Dim intRows As Integer

                                'データテーブルカラム作成(ヘッダ)
                                For intIndex = 0 To strArrayList.Length - 1
                                    dt.Columns.Add(strArrayList(intIndex))
                                Next

                                '行数分ループ
                                'まず最大列数を取得する
                                Dim intColCnt As Integer = strArrayList.Length
                                Dim intColCntWrk As Integer
                                For intIndex = 0 To CsvData.Count - 1

                                    strPrData = Nothing

                                    'カンマ毎に分割
                                    strPrData = CsvData(intIndex).ToString.Split(",")

                                    '■CSVのデータを成形してからデータテーブルに保管する
                                    'まず処理する行の列数を取得する
                                    If strPrData.Count <> intColCnt Then
                                        'ワーク用の配列に退避
                                        Dim strPrDataWrk() As String
                                        strPrDataWrk = strPrData
                                        intColCntWrk = strPrDataWrk.Length - 10
                                        '配列を最大列数で初期化
                                        ReDim strPrData(intColCnt - 1)
                                        '配列の中身を埋めていく
                                        For zz As Integer = 0 To strPrData.Length - 1
                                            '元データの最後の10列まではそのまま入れる
                                            If zz < strPrDataWrk.Length - 10 Then
                                                strPrData(zz) = strPrDataWrk(zz)
                                            Else
                                                '最大列数より10列分前までは空文字を入れる
                                                If zz < intColCnt - 10 Then
                                                    strPrData(zz) = String.Empty
                                                Else
                                                    '元データの残り10列を入れる
                                                    strPrData(zz) = strPrDataWrk(intColCntWrk)
                                                    intColCntWrk = intColCntWrk + 1
                                                End If
                                            End If
                                        Next
                                    End If

                                    dt.Rows.Add(strPrData)
                                Next

                                intMAX = (dt.Columns.Count - 3) / 5

                                ReDim strPrTitle(60)
                                strPrTitle(0) = "検収年"
                                For intIndex = 1 To 12
                                    strPrTitle(1 + (0 + ((intIndex - 1) * 5))) = "区分名" & intIndex.ToString
                                    strPrTitle(1 + (1 + ((intIndex - 1) * 5))) = "券売機日付" & intIndex.ToString
                                    strPrTitle(1 + (2 + ((intIndex - 1) * 5))) = "西日本券件数" & intIndex.ToString
                                    strPrTitle(1 + (3 + ((intIndex - 1) * 5))) = "東日本券件数" & intIndex.ToString
                                    strPrTitle(1 + (4 + ((intIndex - 1) * 5))) = "券件数" & intIndex.ToString & "計"
                                Next

                                For intIndex = 0 To intMAX - 1
                                    strCsv = ""
                                    For intRows = 0 To dt.Rows.Count - 1
                                        If intRows <> 0 Then
                                            strCsv &= ","
                                        Else
                                            strCsv = dt.Rows(intRows).Item(1).ToString()
                                            strCsv &= ","
                                        End If
                                        strCsv &= dt.Rows(intRows).Item(3 + (intIndex * 5)).ToString()
                                        strCsv &= ","
                                        strCsv &= dt.Rows(intRows).Item(4 + (intIndex * 5)).ToString()
                                        strCsv &= ","

                                        strCsv &= dt.Rows(intRows).Item(6 + (intIndex * 5)).ToString()
                                        strCsv &= ","
                                        strCsv &= dt.Rows(intRows).Item(5 + (intIndex * 5)).ToString()
                                        strCsv &= ","
                                        strCsv &= dt.Rows(intRows).Item(7 + (intIndex * 5)).ToString()
                                    Next

                                    PrData.Add(strCsv)
                                Next

                                'PDFファイル作成.
                                ms_MakePdf("券売機・サンドＢＢ１読み出し処理件数", PrData, intCnt, New BBPREP003, strPrTitle)
                            Else
                                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                            End If


                        Case "券売機BB1読み出し処理件数明細"
                            If blnNewFlg = "3" Then
                                dsWrk = mfGetReportDataset(帳票.券売機明細)
                                If dsWrk.Tables(0).Rows.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("券売機ＢＢ１読み出し処理件数明細", New BBPREP001, dsWrk)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            ElseIf blnNewFlg = "2" Then
                                ms_GetCSVData("0943CL", "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    ms_MakePdf("券売機ＢＢ１読み出し処理件数明細", CsvData, intCnt, New BBPREP001, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            Else
                                ms_GetCSVData("0943CL", "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    ms_MakePdf("券売機ＢＢ１読み出し処理件数明細", CsvData, intCnt, New BBPREP001, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            End If


                        Case "サンドBB1読み出し処理件数明細"
                            If blnNewFlg = "3" Then
                                dsWrk = mfGetReportDataset(帳票.サンド明細)
                                If dsWrk.Tables(0).Rows.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("サンドＢＢ１読み出し処理件数明細", New BBPREP002, dsWrk)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            ElseIf blnNewFlg = "2" Then
                                ms_GetCSVData("0944CL", "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("サンドＢＢ１読み出し処理件数明細", CsvData, intCnt, New BBPREP002, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            Else
                                ms_GetCSVData("0944CL", "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("サンドＢＢ１読み出し処理件数明細", CsvData, intCnt, New BBPREP002, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            End If


                        Case "使用中カードDB吸上げ作業明細"
                            If blnNewFlg = "3" Then
                                dsWrk = mfGetReportDataset(帳票.カードDB)
                                If dsWrk.Tables(0).Rows.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("使用中カードＤＢ吸上げ作業明細", New CDPREP001_1, dsWrk)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            ElseIf blnNewFlg = "2" Then
                                ms_GetCSVData("0945CL", "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("使用中カードＤＢ吸上げ作業明細", CsvData, intCnt, New CDPREP001, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            Else
                                ms_GetCSVData("0945CL", "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)
                                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                                    'PDFファイル作成.
                                    ms_MakePdf("使用中カードＤＢ吸上げ作業明細", CsvData, intCnt, New CDPREP001, strArrayList)
                                Else
                                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                                End If
                            End If
                    End Select

                Case sCnsbtnCvs        'ＣＳＶボタン押下
                    Dim ReportClass As New 帳票
                    Try
                        If CType(rowData.FindControl("検収書"), TextBox).Text = "サポートセンタ運用の報告書兼検収書" Then
                            '検収書のみ2016/03から許容する
                            If strKensyuYm >= "201610" Then
                                blnNewFlg = "3"
                            ElseIf strKensyuYm >= "201603" Then
                                blnNewFlg = "2"
                            Else
                                blnNewFlg = "1"
                            End If
                            If drShime("切替区分") = 1 Then
                                '                                blnNewFlg = "3"
                                If strKensyuYm >= "201610" Then
                                    blnNewFlg = "3"
                                ElseIf strKensyuYm >= "201603" Then
                                    blnNewFlg = "2"
                                End If
                            End If
                        End If

                        If blnNewFlg = "3" Then
                            Select Case CType(rowData.FindControl("検収書"), TextBox).Text
                                Case "サポートセンタ運用の報告書兼検収書"
                                    ReportClass = 帳票.サポセン検収書
                                    'CSV発行
                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "券売機・サンドBB1読み出し処理件数"
                                    'ReportClass = 帳票.券・サ・BB1
                                    ReportClass = 帳票.券サBB1
                                    'CSV発行
                                    msPutCSVFile(ReportClass, mfConvertCSVStyle(mfGetReportDataset(ReportClass, strKensyuYm)))
                                Case "券売機BB1読み出し処理件数明細"
                                    ReportClass = 帳票.券売機明細
                                    'CSV発行
                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "サンドBB1読み出し処理件数明細"
                                    ReportClass = 帳票.サンド明細
                                    'CSV発行
                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "使用中カードDB吸上げ作業明細"
                                    ReportClass = 帳票.カードDB
                                    'CSV発行
                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "TBOX吸上げ作業処理件数"
                                    ReportClass = 帳票.TBOX吸上げ
                                    'CSV発行
                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                            End Select
                        ElseIf blnNewFlg = "2" Then
                            Select Case CType(rowData.FindControl("検収書"), TextBox).Text
                                Case "サポートセンタ運用の報告書兼検収書"
                                    If mfCsvDownload("0941CL", "サポートセンタ運用の報告書兼検収書_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
'                                    ReportClass = 帳票.サポセン検収書
'                                    'CSV発行
'                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "券売機・サンドBB1読み出し処理件数"
                                    If mfCsvDownload("0942CL", "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
'                                    ReportClass = 帳票.券・サ・BB1
'                                    'CSV発行
'                                    msPutCSVFile(ReportClass, mfConvertCSVStyle(mfGetReportDataset(ReportClass, strKensyuYm)))
                                Case "券売機BB1読み出し処理件数明細"
                                    If mfCsvDownload("0943CL", "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
'                                    ReportClass = 帳票.券売機明細
'                                    'CSV発行
'                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "サンドBB1読み出し処理件数明細"
                                    If mfCsvDownload("0944CL", "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
'                                    ReportClass = 帳票.サンド明細
'                                    'CSV発行
'                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "使用中カードDB吸上げ作業明細"
                                    If mfCsvDownload("0945CL", "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
'                                    ReportClass = 帳票.カードDB
'                                    'CSV発行
'                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                                Case "TBOX吸上げ作業処理件数"
                                    If mfCsvDownload("0946CL", "ＴＢＯＸ吸上げ作業処理件数_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                                    '                                    ReportClass = 帳票.TBOX吸上げ
                                    '                                    'CSV発行
                                    '                                    msPutCSVFile(ReportClass, mfGetReportDataset(ReportClass))
                            End Select

                        Else
                            Select Case CType(rowData.FindControl("検収書"), TextBox).Text
                                Case "サポートセンタ運用の報告書兼検収書"
                                    If mfCsvDownload("0941CL", "サポートセンタ運用の報告書兼検収書_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                                Case "券売機・サンドBB1読み出し処理件数"
                                    If mfCsvDownload("0942CL", "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                                Case "券売機BB1読み出し処理件数明細"
                                    If mfCsvDownload("0943CL", "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                                Case "サンドBB1読み出し処理件数明細"
                                    If mfCsvDownload("0944CL", "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                                Case "使用中カードDB吸上げ作業明細"
                                    If mfCsvDownload("0945CL", "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                                Case "TBOX吸上げ作業処理件数"
                                    If mfCsvDownload("0946CL", "ＴＢＯＸ吸上げ作業処理件数_[" & strKensyuYm & "]") = False Then
                                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                                    End If
                            End Select
                        End If


                    Catch ex As Exception
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                    End Try
            End Select

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "■ サポートセンタ検収書データの件数チェック"

    ''' <summary>
    ''' サポートセンタ検収書データ件数のチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkDataCnt() As Boolean
        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        mfChkDataCnt = False

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S12")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmErr", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                'エラーor0件
                If objCmd.Parameters("prmErr").Value = "1" Then
                    mfChkDataCnt = False
                    Exit Function
                End If

                mfChkDataCnt = True
            Catch ex As Exception
                mfChkDataCnt = False
            Finally
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    mfChkDataCnt = False
                End If
            End Try
        Else
            mfChkDataCnt = False
        End If

    End Function

#End Region

#Region "■ 初期化"

    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAddHandler()

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click   '検索
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click   'クリア
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnSyukei_Click
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnShime_Click
        AddHandler BtnJinkenhiIns.Click, AddressOf btnJinkenhiIns_Click
        AddHandler btnJippiIns.Click, AddressOf btnJippi_Click
        AddHandler btnJippiUpd.Click, AddressOf btnJippi_Click

        'ボタン名称セット(初期表示はまず「締め」で表示。後にデータチェックから締め解除となるように変更)
        Master.Master.ppRigthButton1.Visible = True
        Master.Master.ppRigthButton2.Visible = True
        Master.Master.ppRigthButton1.Text = "当月集計"

        '検索ボタンに対し、ValidationGroupの設定
        Master.ppRigthButton1.ValidationGroup = "Detail1"

        Master.ppRigthButton1.Attributes("onClick") = "dispWait('search');"

        Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '当月集計確認
        Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め確認

        'ボタン押下時の検証を無効
        Master.ppRigthButton2.CausesValidation = False '検索条件クリア
        Master.Master.ppRigthButton1.CausesValidation = True '当月集計
        Master.Master.ppRigthButton2.CausesValidation = True '締め/締め解除
        Master.Master.ppRigthButton1.ValidationGroup = "Detail2"
        Master.Master.ppRigthButton2.ValidationGroup = "Detail2"

    End Sub

    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear()

        'プログラムＩＤ、画面名設定
        Master.Master.ppProgramID = sCnsProgid
        Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

        'パンくずリスト設定
        Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

        'マルチビューに検索エリアを表示する
        Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

        '検収年月初期化
        txtKensyuYm.ppText = ""

        '更新エリアテキスト項目クリア
        msJinkenAreaClear()
        msJippiAreaClear()

        'グリッドの初期化
        Me.grvList.DataSource = New DataTable

        '件数を初期設定
        Master.ppCount = "0"

        '装置詳細を反映
        Me.grvList.DataBind()

    End Sub

    ''' <summary>
    ''' 人件費エリアクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msJinkenAreaClear()
        txtAdminAmt.ppText = ""
        txtAdminCnt.ppText = ""
        lblNewAdminTotal.Text = "\ 0"
        txtStaffAmt.ppText = ""
        txtStaffCnt.ppText = ""
        lblNewStaffTotal.Text = "\ 0"
        'DOCMENP001-006
        lblAdminAmt.Text = "\ 0"
        lblAdminCnt.Text = "0 人"
        lblNewAdminTotal.Text = "\ 0"
        lblStaffAmt.Text = "\ 0"
        lblStaffCnt.Text = "0 人"
        lblNewStaffTotal.Text = "\ 0"
        '        txtSystemAdminAmt.ppText = ""
        '        txtSystemAdminCnt.ppText = ""
        '        lblNewSystemAdminTotal.Text = "\ 0"
        lblSystemAdminAmt.Text = "\ 0"
        lblSystemAdminCnt.Text = "0 人"
        lblSystemAdminTotal.Text = "\ 0"
        'DOCMENP001-006

        lblOldAdminAmt.Text = "\ 0"
        lblOldAdminTime.Text = "0 時間"
        lblOldAdminDay.Text = "0 日"
        lblOldAdminCnt.Text = "0 人"
        lblOldAdminTotal.Text = "\ 0"
        lblOldBusinessStaffAmt.Text = "\ 0"
        lblOldBusinessStaffTime.Text = "0 時間"
        lblOldBusinessStaffDay.Text = "0 日"
        lblOldBusinessStaffCnt.Text = "0 人"
        lblOldBusinessStaffTotal.Text = "\ 0"
        lblOldTroubleStaffAmt.Text = "\ 0"
        lblOldTroubleStaffTime.Text = "0 時間"
        lblOldTroubleStaffDay.Text = "0 日"
        lblOldTroubleStaffCnt.Text = "0 人"
        lblOldTroubleStaffTotal.Text = "\ 0"

    End Sub

    ''' <summary>
    ''' 実費項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msJippiAreaClear()
        'テキスト項目に設定
        txtKyuden1.ppText = ""
        txtKyuden2.ppText = ""
        lblKyudenTotal.Text = "\ 0"
        txtSyomohin1.ppText = ""
        txtSyomohin2.ppText = ""
        txtSyomohin3.ppText = ""
        txtSyomohin4.ppText = ""
        lblSyomohinTotal.Text = "\ 0"
        txtKonetuhi1.ppText = ""
        txtKonetuhi2.ppText = ""
        lblKonetuhiTotal.Text = "\ 0"
        txtUnsohi1.ppText = ""
        txtUnsohi2.ppText = ""
        txtUnsohi3.ppText = ""
        txtUnsohi4.ppText = ""
        lblUnsohiTotal.Text = "\ 0"
    End Sub

    ''' <summary>
    ''' 合計額計算スクリプトの埋め込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAddMoneyColc()
        '-----合計額計算
        '----------マスタ登録
        txtAdminAmt.ppTextBox.Attributes.Add("onblur", "settotaljinkenhi(" & txtAdminAmt.ppTextBox.ClientID & "," & txtAdminCnt.ppTextBox.ClientID & " ," & lblNewAdminTotal.ClientID & ");")
        txtAdminCnt.ppTextBox.Attributes.Add("onblur", "settotaljinkenhi(" & txtAdminAmt.ppTextBox.ClientID & "," & txtAdminCnt.ppTextBox.ClientID & " ," & lblNewAdminTotal.ClientID & ");")
        txtStaffAmt.ppTextBox.Attributes.Add("onblur", "settotaljinkenhi(" & txtStaffAmt.ppTextBox.ClientID & "," & txtStaffCnt.ppTextBox.ClientID & " ," & lblNewStaffTotal.ClientID & ");")
        txtStaffCnt.ppTextBox.Attributes.Add("onblur", "settotaljinkenhi(" & txtStaffAmt.ppTextBox.ClientID & "," & txtStaffCnt.ppTextBox.ClientID & " ," & lblNewStaffTotal.ClientID & ");")
        'DOCMENP001-006
        '        txtSystemAdminAmt.ppTextBox.Attributes.Add("onblur", "settotaljinkenhi(" & txtSystemAdminAmt.ppTextBox.ClientID & "," & txtSystemAdminCnt.ppTextBox.ClientID & " ," & lblNewSystemAdminTotal.ClientID & ");")
        '        txtSystemAdminCnt.ppTextBox.Attributes.Add("onblur", "settotaljinkenhi(" & txtSystemAdminAmt.ppTextBox.ClientID & "," & txtSystemAdminCnt.ppTextBox.ClientID & " ," & lblNewSystemAdminTotal.ClientID & ");")
        'DOCMENP001-006

        '----------実費請求
        txtKyuden1.ppTextBox.Attributes.Add("onblur", "settotaljippi_two(" & txtKyuden1.ppTextBox.ClientID & "," & txtKyuden2.ppTextBox.ClientID & "," & lblKyudenTotal.ClientID & ");")
        txtKyuden2.ppTextBox.Attributes.Add("onblur", "settotaljippi_two(" & txtKyuden1.ppTextBox.ClientID & "," & txtKyuden2.ppTextBox.ClientID & "," & lblKyudenTotal.ClientID & ");")
        txtSyomohin1.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtSyomohin1.ppTextBox.ClientID & "," & txtSyomohin2.ppTextBox.ClientID & "," & txtSyomohin3.ppTextBox.ClientID & "," & txtSyomohin4.ppTextBox.ClientID & "," & lblSyomohinTotal.ClientID & ");")
        txtSyomohin2.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtSyomohin1.ppTextBox.ClientID & "," & txtSyomohin2.ppTextBox.ClientID & "," & txtSyomohin3.ppTextBox.ClientID & "," & txtSyomohin4.ppTextBox.ClientID & "," & lblSyomohinTotal.ClientID & ");")
        txtSyomohin3.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtSyomohin1.ppTextBox.ClientID & "," & txtSyomohin2.ppTextBox.ClientID & "," & txtSyomohin3.ppTextBox.ClientID & "," & txtSyomohin4.ppTextBox.ClientID & "," & lblSyomohinTotal.ClientID & ");")
        txtSyomohin4.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtSyomohin1.ppTextBox.ClientID & "," & txtSyomohin2.ppTextBox.ClientID & "," & txtSyomohin3.ppTextBox.ClientID & "," & txtSyomohin4.ppTextBox.ClientID & "," & lblSyomohinTotal.ClientID & ");")
        txtKonetuhi1.ppTextBox.Attributes.Add("onblur", "settotaljippi_two(" & txtKonetuhi1.ppTextBox.ClientID & "," & txtKonetuhi2.ppTextBox.ClientID & "," & lblKonetuhiTotal.ClientID & ");")
        txtKonetuhi2.ppTextBox.Attributes.Add("onblur", "settotaljippi_two(" & txtKonetuhi1.ppTextBox.ClientID & "," & txtKonetuhi2.ppTextBox.ClientID & "," & lblKonetuhiTotal.ClientID & ");")
        txtUnsohi1.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtUnsohi1.ppTextBox.ClientID & "," & txtUnsohi2.ppTextBox.ClientID & "," & txtUnsohi3.ppTextBox.ClientID & "," & txtUnsohi4.ppTextBox.ClientID & "," & lblUnsohiTotal.ClientID & ");")
        txtUnsohi2.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtUnsohi1.ppTextBox.ClientID & "," & txtUnsohi2.ppTextBox.ClientID & "," & txtUnsohi3.ppTextBox.ClientID & "," & txtUnsohi4.ppTextBox.ClientID & "," & lblUnsohiTotal.ClientID & ");")
        txtUnsohi3.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtUnsohi1.ppTextBox.ClientID & "," & txtUnsohi2.ppTextBox.ClientID & "," & txtUnsohi3.ppTextBox.ClientID & "," & txtUnsohi4.ppTextBox.ClientID & "," & lblUnsohiTotal.ClientID & ");")
        txtUnsohi4.ppTextBox.Attributes.Add("onblur", "settotaljippi(" & txtUnsohi1.ppTextBox.ClientID & "," & txtUnsohi2.ppTextBox.ClientID & "," & txtUnsohi3.ppTextBox.ClientID & "," & txtUnsohi4.ppTextBox.ClientID & "," & lblUnsohiTotal.ClientID & ");")

        '-----合計額計算
        '----------マスタ登録
        txtAdminAmt.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtAdminAmt.ppTextBox.ClientID & ");")
        txtAdminCnt.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtAdminCnt.ppTextBox.ClientID & ");")
        txtStaffAmt.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtStaffAmt.ppTextBox.ClientID & ");")
        txtStaffCnt.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtStaffCnt.ppTextBox.ClientID & ");")
        'DOCMENP001-006
        '        txtSystemAdminAmt.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtSystemAdminAmt.ppTextBox.ClientID & ");")
        '        txtSystemAdminCnt.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtSystemAdminCnt.ppTextBox.ClientID & ");")
        'DOCMENP001-006

        '----------実費請求
        txtKyuden1.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtKyuden1.ppTextBox.ClientID & ");")
        txtKyuden2.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtKyuden2.ppTextBox.ClientID & ");")
        txtSyomohin1.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtSyomohin1.ppTextBox.ClientID & ");")
        txtSyomohin2.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtSyomohin2.ppTextBox.ClientID & ");")
        txtSyomohin3.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtSyomohin3.ppTextBox.ClientID & ");")
        txtSyomohin4.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtSyomohin4.ppTextBox.ClientID & ");")
        txtKonetuhi1.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtKonetuhi1.ppTextBox.ClientID & ");")
        txtKonetuhi2.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtKonetuhi2.ppTextBox.ClientID & ");")
        txtUnsohi1.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtUnsohi1.ppTextBox.ClientID & ");")
        txtUnsohi2.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtUnsohi2.ppTextBox.ClientID & ");")
        txtUnsohi3.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtUnsohi3.ppTextBox.ClientID & ");")
        txtUnsohi4.ppTextBox.Attributes.Add("onfocus", "setdeletezero(" & txtUnsohi4.ppTextBox.ClientID & ");")

    End Sub

#End Region

#Region "■ 検索データ検索・設定"

    ''' <summary>
    ''' 読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDataRead(ByVal strDegreeYm As String)
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S10")
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Connection = objCon

                'パラメータ設定
                With objCmd.Parameters
                    '検収年月
                    .Add(pfSet_Param("prmD91_DEGREE", SqlDbType.NVarChar, strDegreeYm))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                'エラー？
                If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                    Throw New Exception()
                End If

                '各項目にデータを整形して設定
                msSetData(objDs.Tables(0))

                'データなしの場合、更新エリアを非活性、検収年月を空にする
                If objDs.Tables(0).Rows.Count <= 0 Then

                    '確認メッセージ再設定.
                    Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '締め確認

                End If

                msSetEnable(mfSettingEnableMode())

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "読み込み処理")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        End If
    End Sub

    ''' <summary>
    ''' データ設定
    ''' </summary>
    ''' <param name="objTbl"></param>
    ''' <remarks></remarks>
    Private Sub msSetData(ByVal objTbl As DataTable)
        Dim drWork As DataRow = mfGetWorkData()
        'データ無し（新規）
        If objTbl.Rows.Count <= 0 Then
            Dim drMaster As DataRow = mfGetMasterData()

            'テキスト項目に設定
            msJippiAreaClear()

            If drMaster Is Nothing Then
                'テキスト項目に設定
                txtAdminAmt.ppText = "0"
                txtAdminCnt.ppText = "0"
                lblNewAdminTotal.Text = "\ 0"
                txtStaffAmt.ppText = "0"
                txtStaffCnt.ppText = "0"
                lblNewStaffTotal.Text = "\ 0"
                'DOCMENP001-006
                lblAdminAmt.Text = "\ 0"
                lblAdminCnt.Text = "0 人"
                lblNewAdminTotal.Text = "\ 0"
                lblStaffAmt.Text = "\ 0"
                lblStaffCnt.Text = "0 人"
                lblNewStaffTotal.Text = "\ 0"
                'txtSystemAdminAmt.ppText = "0"
                'txtSystemAdminCnt.ppText = "0"
                'lblNewSystemAdminTotal.Text = "\ 0"
                lblSystemAdminAmt.Text = "\ 0"
                lblSystemAdminCnt.Text = "0 人"
                lblSystemAdminTotal.Text = "\ 0"
                'DOCMENP001-006
            Else
                txtAdminAmt.ppText = Math.Truncate(drMaster("管理者金額"))
                txtAdminCnt.ppText = drMaster("管理者人数")
                lblNewAdminTotal.Text = "\ " & Integer.Parse(txtAdminAmt.ppText * txtAdminCnt.ppText).ToString("N0")
                txtStaffAmt.ppText = Math.Truncate(drMaster("業務担当者金額"))
                txtStaffCnt.ppText = drMaster("業務担当者人数")
                lblNewStaffTotal.Text = "\ " & Integer.Parse(txtStaffAmt.ppText * txtStaffCnt.ppText).ToString("N0")
                'DOCMENP001-006
                lblAdminAmt.Text = "\ " & Decimal.Parse(drMaster("管理者金額")).ToString("N0")
                lblAdminCnt.Text = drMaster("管理者人数") & " 人"
                lblAdminTotal.Text = "\ " & Decimal.Parse(drMaster("管理者金額") * drMaster("管理者人数")).ToString("N0")
                lblStaffAmt.Text = "\ " & Decimal.Parse(drMaster("管理者金額")).ToString("N0")
                lblStaffCnt.Text = drMaster("業務担当者人数") & " 人"
                lblStaffTotal.Text = "\ " & Decimal.Parse(drMaster("管理者金額") * drMaster("業務担当者人数")).ToString("N0")
                '                txtSystemAdminAmt.ppText = Math.Truncate(drMaster("システム運用管理者金額"))
                '                txtSystemAdminCnt.ppText = drMaster("システム運用管理者人数")
                '                lblNewSystemAdminTotal.Text = "\ " & Integer.Parse(txtSystemAdminAmt.ppText * txtSystemAdminCnt.ppText).ToString("N0")
                lblSystemAdminAmt.Text = "\ " & Decimal.Parse(drMaster("システム運用管理者金額")).ToString("N0")
                lblSystemAdminCnt.Text = drMaster("システム運用管理者人数") & " 人"
                lblSystemAdminTotal.Text = "\ " & Decimal.Parse(drMaster("システム運用管理者金額") * drMaster("システム運用管理者人数")).ToString("N0")
                'DOCMENP001-006
            End If

            If drWork Is Nothing Then
                txtKyuden1.ppText = "0"
                txtKyuden2.ppText = "0"
                lblKyudenTotal.Text = "\ 0"
                txtSyomohin1.ppText = "0"
                txtSyomohin2.ppText = "0"
                txtSyomohin3.ppText = "0"
                txtSyomohin4.ppText = "0"
                lblSyomohinTotal.Text = "\ 0"
                txtKonetuhi1.ppText = "0"
                txtKonetuhi2.ppText = "0"
                lblKonetuhiTotal.Text = "\ 0"
                txtUnsohi1.ppText = "0"
                txtUnsohi2.ppText = "0"
                txtUnsohi3.ppText = "0"
                txtUnsohi4.ppText = "0"
                lblUnsohiTotal.Text = "\ 0"

            Else
                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画前, "実費請求内容を確認し、当月集計処理")

                txtKyuden1.ppText = Math.Truncate(drWork("給電作業費1"))
                txtKyuden2.ppText = Math.Truncate(drWork("給電作業費2"))
                lblKyudenTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("給電作業費合計"))).ToString("N0")
                txtSyomohin1.ppText = Math.Truncate(drWork("消耗品1"))
                txtSyomohin2.ppText = Math.Truncate(drWork("消耗品2"))
                txtSyomohin3.ppText = Math.Truncate(drWork("消耗品3"))
                txtSyomohin4.ppText = Math.Truncate(drWork("消耗品4"))
                lblSyomohinTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("消耗品合計"))).ToString("N0")
                txtKonetuhi1.ppText = Math.Truncate(drWork("光熱費1"))
                txtKonetuhi2.ppText = Math.Truncate(drWork("光熱費2"))
                lblKonetuhiTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("光熱費合計"))).ToString("N0")
                txtUnsohi1.ppText = Math.Truncate(drWork("運送費1"))
                txtUnsohi2.ppText = Math.Truncate(drWork("運送費2"))
                txtUnsohi3.ppText = Math.Truncate(drWork("運送費3"))
                txtUnsohi4.ppText = Math.Truncate(drWork("運送費4"))
                lblUnsohiTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("運送費合計"))).ToString("N0")
            End If

            'リストに設定
            grvList.DataSource = New DataTable()
            grvList.DataBind()

            '件数を設定
            Master.ppCount = "0"
            Return
        End If

        Dim objRow As DataRow = objTbl.Rows(0)

        If objRow("切替区分") = "1" Then
            If Me.txtKensyuYm.ppText.Replace("/", "") >= "201610" Then
                'テキスト項目に設定
                txtAdminAmt.ppText = objRow("管理者金額")
                txtAdminCnt.ppText = objRow("管理者人数")
                lblNewAdminTotal.Text = "\ " & Integer.Parse(txtAdminAmt.ppText * txtAdminCnt.ppText).ToString("N0")
                txtStaffAmt.ppText = objRow("業務担当者金額")
                txtStaffCnt.ppText = objRow("業務担当者人数")
                lblNewStaffTotal.Text = "\ " & Integer.Parse(txtStaffAmt.ppText * txtStaffCnt.ppText).ToString("N0")
                'DOCMENP001-006
                lblAdminAmt.Text = "\ " & Integer.Parse(objRow("管理者金額")).ToString("N0")
                lblAdminCnt.Text = objRow("管理者人数") & " 人"
                lblAdminTotal.Text = "\ " & Decimal.Parse(objRow("管理者金額") * objRow("管理者人数")).ToString("N0")
                lblStaffAmt.Text = "\ " & Integer.Parse(objRow("業務担当者金額")).ToString("N0")
                lblStaffCnt.Text = objRow("業務担当者人数") & " 人"
                lblStaffTotal.Text = "\ " & Integer.Parse(objRow("業務担当者金額") * objRow("業務担当者人数")).ToString("N0")
                '            txtSystemAdminAmt.ppText = objRow("システム運用管理者金額")
                '            txtSystemAdminCnt.ppText = objRow("システム運用管理者人数")
                '            lblNewSystemAdminTotal.Text = "\ " & Integer.Parse(txtSystemAdminAmt.ppText * txtSystemAdminCnt.ppText).ToString("N0")
                lblSystemAdminAmt.Text = "\ " & Integer.Parse(objRow("システム運用管理者金額")).ToString("N0")
                lblSystemAdminCnt.Text = objRow("システム運用管理者人数") & " 人"
                lblSystemAdminTotal.Text = "\ " & Integer.Parse(objRow("システム運用管理者金額") * objRow("システム運用管理者人数")).ToString("N0")
                'DOCMENP001-006

                txtKyuden1.ppText = objRow("給電作業費１")
                txtKyuden2.ppText = objRow("給電作業費２")
                lblKyudenTotal.Text = "\ " & Integer.Parse(objRow("給電作業費合計")).ToString("N0")
                txtSyomohin1.ppText = objRow("消耗品１")
                txtSyomohin2.ppText = objRow("消耗品２")
                txtSyomohin3.ppText = objRow("消耗品３")
                txtSyomohin4.ppText = objRow("消耗品４")
                lblSyomohinTotal.Text = "\ " & Integer.Parse(objRow("消耗品合計")).ToString("N0")
                txtKonetuhi1.ppText = objRow("光熱費１")
                txtKonetuhi2.ppText = objRow("光熱費２")
                lblKonetuhiTotal.Text = "\ " & Integer.Parse(objRow("光熱費合計")).ToString("N0")
                txtUnsohi1.ppText = objRow("運送費１")
                txtUnsohi2.ppText = objRow("運送費２")
                txtUnsohi3.ppText = objRow("運送費３")
                txtUnsohi4.ppText = objRow("運送費４")
                lblUnsohiTotal.Text = "\ " & Integer.Parse(objRow("運送費合計")).ToString("N0")

                '一覧に必要ない列を削除
                objTbl.Columns.Remove("管理者金額")
                objTbl.Columns.Remove("管理者人数")
                objTbl.Columns.Remove("業務担当者金額")
                objTbl.Columns.Remove("業務担当者人数")
                objTbl.Columns.Remove("システム運用管理者金額")
                objTbl.Columns.Remove("システム運用管理者人数")

                objTbl.Columns.Remove("給電作業費１")
                objTbl.Columns.Remove("給電作業費２")
                objTbl.Columns.Remove("給電作業費合計")
                objTbl.Columns.Remove("消耗品１")
                objTbl.Columns.Remove("消耗品２")
                objTbl.Columns.Remove("消耗品３")
                objTbl.Columns.Remove("消耗品４")
                objTbl.Columns.Remove("消耗品合計")
                objTbl.Columns.Remove("光熱費１")
                objTbl.Columns.Remove("光熱費２")
                objTbl.Columns.Remove("光熱費合計")
                objTbl.Columns.Remove("運送費１")
                objTbl.Columns.Remove("運送費２")
                objTbl.Columns.Remove("運送費３")
                objTbl.Columns.Remove("運送費４")
                objTbl.Columns.Remove("運送費合計")
            Else
                'テキスト項目に設定
                txtAdminAmt.ppText = objRow("管理者金額")
                txtAdminCnt.ppText = objRow("管理者人数")
                lblNewAdminTotal.Text = "\ " & Integer.Parse(txtAdminAmt.ppText * txtAdminCnt.ppText).ToString("N0")
                txtStaffAmt.ppText = objRow("業務担当者金額")
                txtStaffCnt.ppText = objRow("業務担当者人数")
                lblNewStaffTotal.Text = "\ " & Integer.Parse(txtStaffAmt.ppText * txtStaffCnt.ppText).ToString("N0")
                'DOCMENP001-006
                lblAdminAmt.Text = "\ " & Integer.Parse(objRow("管理者金額")).ToString("N0")
                lblAdminCnt.Text = objRow("管理者人数") & " 人"
                lblAdminTotal.Text = "\ " & Integer.Parse(objRow("管理者金額") * objRow("管理者人数")).ToString("N0")
                lblStaffAmt.Text = "\ " & Integer.Parse(objRow("業務担当者金額")).ToString("N0")
                lblStaffCnt.Text = objRow("業務担当者人数") & " 人"
                lblStaffTotal.Text = "\ " & Integer.Parse(objRow("業務担当者金額") * objRow("業務担当者人数")).ToString("N0")
                '            txtSystemAdminAmt.ppText = objRow("システム運用管理者金額")
                '            txtSystemAdminCnt.ppText = objRow("システム運用管理者人数")
                '            lblNewSystemAdminTotal.Text = "\ " & Integer.Parse(txtSystemAdminAmt.ppText * txtSystemAdminCnt.ppText).ToString("N0")
                lblSystemAdminAmt.Text = "\ " & Integer.Parse(objRow("システム運用管理者金額")).ToString("N0")
                lblSystemAdminCnt.Text = objRow("システム運用管理者人数") & " 人"
                lblSystemAdminTotal.Text = "\ " & Integer.Parse(objRow("システム運用管理者金額") * objRow("システム運用管理者人数")).ToString("N0")
                'DOCMENP001-006

                txtKyuden1.ppText = objRow("給電作業費１")
                txtKyuden2.ppText = objRow("給電作業費２")
                lblKyudenTotal.Text = "\ " & Integer.Parse(objRow("給電作業費合計")).ToString("N0")
                txtSyomohin1.ppText = objRow("消耗品１")
                txtSyomohin2.ppText = objRow("消耗品２")
                txtSyomohin3.ppText = objRow("消耗品３")
                txtSyomohin4.ppText = objRow("消耗品４")
                lblSyomohinTotal.Text = "\ " & Integer.Parse(objRow("消耗品合計")).ToString("N0")
                txtKonetuhi1.ppText = objRow("光熱費１")
                txtKonetuhi2.ppText = objRow("光熱費２")
                lblKonetuhiTotal.Text = "\ " & Integer.Parse(objRow("光熱費合計")).ToString("N0")
                txtUnsohi1.ppText = objRow("運送費１")
                txtUnsohi2.ppText = objRow("運送費２")
                txtUnsohi3.ppText = objRow("運送費３")
                txtUnsohi4.ppText = objRow("運送費４")
                lblUnsohiTotal.Text = "\ " & Integer.Parse(objRow("運送費合計")).ToString("N0")

                '一覧に必要ない列を削除
                objTbl.Columns.Remove("管理者金額")
                objTbl.Columns.Remove("管理者人数")
                objTbl.Columns.Remove("業務担当者金額")
                objTbl.Columns.Remove("業務担当者人数")
                objTbl.Columns.Remove("システム運用管理者金額")
                objTbl.Columns.Remove("システム運用管理者人数")

                objTbl.Columns.Remove("給電作業費１")
                objTbl.Columns.Remove("給電作業費２")
                objTbl.Columns.Remove("給電作業費合計")
                objTbl.Columns.Remove("消耗品１")
                objTbl.Columns.Remove("消耗品２")
                objTbl.Columns.Remove("消耗品３")
                objTbl.Columns.Remove("消耗品４")
                objTbl.Columns.Remove("消耗品合計")
                objTbl.Columns.Remove("光熱費１")
                objTbl.Columns.Remove("光熱費２")
                objTbl.Columns.Remove("光熱費合計")
                objTbl.Columns.Remove("運送費１")
                objTbl.Columns.Remove("運送費２")
                objTbl.Columns.Remove("運送費３")
                objTbl.Columns.Remove("運送費４")
                objTbl.Columns.Remove("運送費合計")
            End If
        Else
            'テキスト項目に設定
            lblOldAdminAmt.Text = "\ " & Integer.Parse(objRow("管理者金額")).ToString("N0")
            lblOldAdminTime.Text = objRow("管理者時間") & " 時間"
            lblOldAdminDay.Text = objRow("管理者日数") & " 日"
            lblOldAdminCnt.Text = objRow("管理者人数") & " 人"
            lblOldAdminTotal.Text = "\ " & Integer.Parse(objRow("管理者金額") * objRow("管理者時間") * objRow("管理者日数") * objRow("管理者人数")).ToString("N0")
            lblOldBusinessStaffAmt.Text = "\ " & Integer.Parse(objRow("業務担当者金額")).ToString("N0")
            lblOldBusinessStaffTime.Text = objRow("業務担当者時間") & " 時間"
            lblOldBusinessStaffDay.Text = objRow("業務担当者日数") & " 日"
            lblOldBusinessStaffCnt.Text = objRow("業務担当者人数") & " 人"
            lblOldBusinessStaffTotal.Text = "\ " & Integer.Parse(objRow("業務担当者金額") * objRow("業務担当者時間") * objRow("業務担当者日数") * objRow("業務担当者人数")).ToString("N0")
            lblOldTroubleStaffAmt.Text = "\ " & Integer.Parse(objRow("故障受付担当者金額")).ToString("N0")
            lblOldTroubleStaffTime.Text = objRow("故障受付担当者時間") & " 時間"
            lblOldTroubleStaffDay.Text = objRow("故障受付担当者日数") & " 日"
            lblOldTroubleStaffCnt.Text = objRow("故障受付担当者人数") & " 人"
            lblOldTroubleStaffTotal.Text = "\ " & Integer.Parse(objRow("故障受付担当者金額") * objRow("故障受付担当者時間") * objRow("故障受付担当者日数") * objRow("故障受付担当者人数")).ToString("N0")

            txtKyuden1.ppText = objRow("給電作業費１")
            txtKyuden2.ppText = objRow("給電作業費２")
            lblKyudenTotal.Text = "\ " & Integer.Parse(objRow("給電作業費合計")).ToString("N0")
            txtSyomohin1.ppText = objRow("消耗品１")
            txtSyomohin2.ppText = objRow("消耗品２")
            txtSyomohin3.ppText = objRow("消耗品３")
            txtSyomohin4.ppText = objRow("消耗品４")
            lblSyomohinTotal.Text = "\ " & Integer.Parse(objRow("消耗品合計")).ToString("N0")
            txtKonetuhi1.ppText = objRow("光熱費１")
            txtKonetuhi2.ppText = objRow("光熱費２")
            lblKonetuhiTotal.Text = "\ " & Integer.Parse(objRow("光熱費合計")).ToString("N0")
            txtUnsohi1.ppText = objRow("運送費１")
            txtUnsohi2.ppText = objRow("運送費２")
            txtUnsohi3.ppText = objRow("運送費３")
            txtUnsohi4.ppText = objRow("運送費４")
            lblUnsohiTotal.Text = "\ " & Integer.Parse(objRow("運送費合計")).ToString("N0")

            '一覧に必要ない列を削除
            objTbl.Columns.Remove("管理者金額")
            objTbl.Columns.Remove("管理者時間")
            objTbl.Columns.Remove("管理者日数")
            objTbl.Columns.Remove("管理者人数")
            objTbl.Columns.Remove("業務担当者金額")
            objTbl.Columns.Remove("業務担当者時間")
            objTbl.Columns.Remove("業務担当者日数")
            objTbl.Columns.Remove("業務担当者人数")
            objTbl.Columns.Remove("故障受付担当者金額")
            objTbl.Columns.Remove("故障受付担当者時間")
            objTbl.Columns.Remove("故障受付担当者日数")
            objTbl.Columns.Remove("故障受付担当者人数")

            objTbl.Columns.Remove("給電作業費１")
            objTbl.Columns.Remove("給電作業費２")
            objTbl.Columns.Remove("給電作業費合計")
            objTbl.Columns.Remove("消耗品１")
            objTbl.Columns.Remove("消耗品２")
            objTbl.Columns.Remove("消耗品３")
            objTbl.Columns.Remove("消耗品４")
            objTbl.Columns.Remove("消耗品合計")
            objTbl.Columns.Remove("光熱費１")
            objTbl.Columns.Remove("光熱費２")
            objTbl.Columns.Remove("光熱費合計")
            objTbl.Columns.Remove("運送費１")
            objTbl.Columns.Remove("運送費２")
            objTbl.Columns.Remove("運送費３")
            objTbl.Columns.Remove("運送費４")
            objTbl.Columns.Remove("運送費合計")

        End If

        If Not drWork Is Nothing Then
            txtKyuden1.ppText = Math.Truncate(drWork("給電作業費1"))
            txtKyuden2.ppText = Math.Truncate(drWork("給電作業費2"))
            lblKyudenTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("給電作業費合計"))).ToString("N0")
            txtSyomohin1.ppText = Math.Truncate(drWork("消耗品1"))
            txtSyomohin2.ppText = Math.Truncate(drWork("消耗品2"))
            txtSyomohin3.ppText = Math.Truncate(drWork("消耗品3"))
            txtSyomohin4.ppText = Math.Truncate(drWork("消耗品4"))
            lblSyomohinTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("消耗品合計"))).ToString("N0")
            txtKonetuhi1.ppText = Math.Truncate(drWork("光熱費1"))
            txtKonetuhi2.ppText = Math.Truncate(drWork("光熱費2"))
            lblKonetuhiTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("光熱費合計"))).ToString("N0")
            txtUnsohi1.ppText = Math.Truncate(drWork("運送費1"))
            txtUnsohi2.ppText = Math.Truncate(drWork("運送費2"))
            txtUnsohi3.ppText = Math.Truncate(drWork("運送費3"))
            txtUnsohi4.ppText = Math.Truncate(drWork("運送費4"))
            lblUnsohiTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("運送費合計"))).ToString("N0")
        End If

        'リストに設定
        grvList.DataSource = objTbl
        grvList.DataBind()

        '件数を設定
        Master.ppCount = objTbl.Rows.Count

        '一覧の2行目以降の総計を空にする
        Dim intCnt As Integer = 0
        For Each objgrvRow As GridViewRow In grvList.Rows
            intCnt += 1
            If intCnt = 1 Then
                Continue For
            End If

            '総計列を空白に設定
            objgrvRow.Cells(eColGrv.総計).Text = ""
        Next

    End Sub

#End Region

#Region "■ 活性制御"

    ''' <summary>
    ''' 活性制御状態取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfGetShimeJotai() As DataRow
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        mfGetShimeJotai = Nothing
        If txtKensyuYm.ppText.Replace("/", "").Trim() = String.Empty Then
            Exit Function
        End If
        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S19")
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Connection = objCon

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmD91_DEGREE", SqlDbType.NVarChar, txtKensyuYm.ppText.Replace("/", "").Trim()))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count <> 0 Then
                    Return objDs.Tables(0).Rows(0)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                mfGetShimeJotai = Nothing
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    mfGetShimeJotai = Nothing
                End If
            End Try
        Else
            mfGetShimeJotai = Nothing
        End If
    End Function

    Private Function mfGetBeforeShimeJotai() As DataRow
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        mfGetBeforeShimeJotai = Nothing
        If txtKensyuYm.ppText.Replace("/", "").Trim() = String.Empty Then
            Exit Function
        End If
        Dim strKensyuYM As String = txtKensyuYm.ppText.Replace("/", "").Trim()
        If strKensyuYM.Substring(4, 2) = "01" Then
            strKensyuYM = strKensyuYM - 100 '1年前に
            strKensyuYM = strKensyuYM + 11  '12月に
        Else
            strKensyuYM = strKensyuYM - 1  '前月に
        End If

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S19")
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Connection = objCon

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmD91_DEGREE", SqlDbType.NVarChar, strKensyuYM))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count <> 0 Then
                    Return objDs.Tables(0).Rows(0)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                mfGetBeforeShimeJotai = Nothing
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    mfGetBeforeShimeJotai = Nothing
                End If
            End Try
        Else
            mfGetBeforeShimeJotai = Nothing
        End If
    End Function

    Private Function mfGetMasterData() As DataRow
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim strDegreeYm As String = txtKensyuYm.ppText.Replace("/", "")
        mfGetMasterData = Nothing
        If strDegreeYm.Trim = String.Empty Then
            Exit Function
        End If
        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S6")
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Connection = objCon

                'パラメータ設定
                With objCmd.Parameters
                    '検収年月
                    .Add(pfSet_Param("prmKensyuYM", SqlDbType.NVarChar, strDegreeYm))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count <> 0 Then
                    Return objDs.Tables(0).Rows(0)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "読み込み処理")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        End If
    End Function

    Private Function mfGetWorkData() As DataRow
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim strDegreeYm As String = txtKensyuYm.ppText.Replace("/", "")
        mfGetWorkData = Nothing
        If strDegreeYm = String.Empty Then
            Exit Function
        End If
        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S7")
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Connection = objCon

                'パラメータ設定
                With objCmd.Parameters
                    '検収年月
                    .Add(pfSet_Param("prmKensyuYM", SqlDbType.NVarChar, strDegreeYm))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count <> 0 Then
                    Return objDs.Tables(0).Rows(0)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "読み込み処理")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        End If
    End Function

    Private Function mfSettingEnableMode() As String
        Dim drShime As DataRow = mfGetShimeJotai()
        Dim drMaster As DataRow = mfGetMasterData()
        Dim drWork As DataRow = mfGetWorkData()

        If drShime Is Nothing Then
            If drMaster Is Nothing Then
                mfSettingEnableMode = "1"
            Else
                If drWork Is Nothing Then
                    mfSettingEnableMode = "1"
                Else
                    mfSettingEnableMode = "2"
                End If
            End If
        Else
            Dim strSeikyu As String = drShime.Item("請求区分")
            Dim strKensyu As String = drShime.Item("検収区分")
            If strSeikyu = "1" Then
                mfSettingEnableMode = "3"
            Else
                If strKensyu = "1" Then
                    mfSettingEnableMode = "4"
                Else
                    mfSettingEnableMode = "5"
                End If

            End If
        End If

        '検収データがあったら×登録○更新
        If mfSettingEnableMode = "3" OrElse _
           mfSettingEnableMode = "4" OrElse _
           mfSettingEnableMode = "5" Then
            btnJippiIns.Enabled = False
            btnJippiUpd.Enabled = True
        Else
            If drWork Is Nothing Then
                btnJippiIns.Enabled = True
                btnJippiUpd.Enabled = False
            Else
                btnJippiIns.Enabled = False
                btnJippiUpd.Enabled = True
            End If
        End If

    End Function

    ''' <summary>
    ''' 活性制御
    ''' </summary>
    ''' <param name="strMode"></param>
    ''' <remarks></remarks>
    Private Sub msSetEnable(ByVal strMode As String)
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        Select Case strMode
            Case "0"
                txtKensyuYm.ppEnabled = True
                txtAdminAmt.ppEnabled = False
                txtAdminCnt.ppEnabled = False
                txtStaffAmt.ppEnabled = False
                txtStaffCnt.ppEnabled = False
                'DOCMENP001-0006
                '                txtSystemAdminAmt.ppEnabled = False
                '                txtSystemAdminCnt.ppEnabled = False
                lblSystemAdminAmt.Enabled = False
                lblSystemAdminCnt.Enabled = False
                'DOCMENP001-0006
                txtKyuden1.ppEnabled = False
                txtKyuden2.ppEnabled = False
                txtSyomohin1.ppEnabled = False
                txtSyomohin2.ppEnabled = False
                txtSyomohin3.ppEnabled = False
                txtSyomohin4.ppEnabled = False
                txtKonetuhi1.ppEnabled = False
                txtKonetuhi2.ppEnabled = False
                txtUnsohi1.ppEnabled = False
                txtUnsohi2.ppEnabled = False
                txtUnsohi3.ppEnabled = False
                txtUnsohi4.ppEnabled = False
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Master.Master.ppRigthButton2.Text = "締め"

                BtnJinkenhiIns.Enabled = False
                btnJippiIns.Enabled = False
                btnJippiUpd.Enabled = False

            Case "1"
                txtKensyuYm.ppEnabled = False
                txtAdminAmt.ppEnabled = True
                txtAdminCnt.ppEnabled = True
                txtStaffAmt.ppEnabled = True
                txtStaffCnt.ppEnabled = True
                'DOCMENP001-0006
                '                txtSystemAdminAmt.ppEnabled = True
                '                txtSystemAdminCnt.ppEnabled = True
                lblSystemAdminAmt.Enabled = True
                lblSystemAdminCnt.Enabled = True
                'DOCMENP001-0006
                txtKyuden1.ppEnabled = True
                txtKyuden2.ppEnabled = True
                txtSyomohin1.ppEnabled = True
                txtSyomohin2.ppEnabled = True
                txtSyomohin3.ppEnabled = True
                txtSyomohin4.ppEnabled = True
                txtKonetuhi1.ppEnabled = True
                txtKonetuhi2.ppEnabled = True
                txtUnsohi1.ppEnabled = True
                txtUnsohi2.ppEnabled = True
                txtUnsohi3.ppEnabled = True
                txtUnsohi4.ppEnabled = True
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Master.Master.ppRigthButton2.Text = "締め"

                BtnJinkenhiIns.Enabled = True
                'btnJippiIns.Enabled = True
                'btnJippiUpd.Enabled = True

            Case "2"
                txtKensyuYm.ppEnabled = False
                txtAdminAmt.ppEnabled = True
                txtAdminCnt.ppEnabled = True
                txtStaffAmt.ppEnabled = True
                txtStaffCnt.ppEnabled = True
                'DOCMENP001-0006
                '                txtSystemAdminAmt.ppEnabled = True
                '                txtSystemAdminCnt.ppEnabled = True
                lblSystemAdminAmt.Enabled = True
                lblSystemAdminCnt.Enabled = True
                'DOCMENP001-0006
                txtKyuden1.ppEnabled = True
                txtKyuden2.ppEnabled = True
                txtSyomohin1.ppEnabled = True
                txtSyomohin2.ppEnabled = True
                txtSyomohin3.ppEnabled = True
                txtSyomohin4.ppEnabled = True
                txtKonetuhi1.ppEnabled = True
                txtKonetuhi2.ppEnabled = True
                txtUnsohi1.ppEnabled = True
                txtUnsohi2.ppEnabled = True
                txtUnsohi3.ppEnabled = True
                txtUnsohi4.ppEnabled = True
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = False
                Master.Master.ppRigthButton2.Text = "締め"

                BtnJinkenhiIns.Enabled = True
                'btnJippiIns.Enabled = True
                'btnJippiUpd.Enabled = True

            Case "3"
                txtKensyuYm.ppEnabled = False
                txtAdminAmt.ppEnabled = False
                txtAdminCnt.ppEnabled = False
                txtStaffAmt.ppEnabled = False
                txtStaffCnt.ppEnabled = False
                'DOCMENP001-0006
                '                txtSystemAdminAmt.ppEnabled = False
                '                txtSystemAdminCnt.ppEnabled = False
                lblSystemAdminAmt.Enabled = False
                lblSystemAdminCnt.Enabled = False
                'DOCMENP001-0006
                txtKyuden1.ppEnabled = False
                txtKyuden2.ppEnabled = False
                txtSyomohin1.ppEnabled = False
                txtSyomohin2.ppEnabled = False
                txtSyomohin3.ppEnabled = False
                txtSyomohin4.ppEnabled = False
                txtKonetuhi1.ppEnabled = False
                txtKonetuhi2.ppEnabled = False
                txtUnsohi1.ppEnabled = False
                txtUnsohi2.ppEnabled = False
                txtUnsohi3.ppEnabled = False
                txtUnsohi4.ppEnabled = False
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Master.Master.ppRigthButton2.Text = "締め解除"

                BtnJinkenhiIns.Enabled = False
                btnJippiIns.Enabled = False
                btnJippiUpd.Enabled = False

            Case "4"
                txtKensyuYm.ppEnabled = False
                txtAdminAmt.ppEnabled = False
                txtAdminCnt.ppEnabled = False
                txtStaffAmt.ppEnabled = False
                txtStaffCnt.ppEnabled = False
                'DOCMENP001-0006
                '                txtSystemAdminAmt.ppEnabled = False
                '                txtSystemAdminCnt.ppEnabled = False
                lblSystemAdminAmt.Enabled = False
                lblSystemAdminCnt.Enabled = False
                'DOCMENP001-0006
                txtKyuden1.ppEnabled = False
                txtKyuden2.ppEnabled = False
                txtSyomohin1.ppEnabled = False
                txtSyomohin2.ppEnabled = False
                txtSyomohin3.ppEnabled = False
                txtSyomohin4.ppEnabled = False
                txtKonetuhi1.ppEnabled = False
                txtKonetuhi2.ppEnabled = False
                txtUnsohi1.ppEnabled = False
                txtUnsohi2.ppEnabled = False
                txtUnsohi3.ppEnabled = False
                txtUnsohi4.ppEnabled = False
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = True
                Master.Master.ppRigthButton2.Text = "締め解除"

                BtnJinkenhiIns.Enabled = False
                btnJippiIns.Enabled = False
                btnJippiUpd.Enabled = False

            Case "5"
                txtKensyuYm.ppEnabled = False
                txtAdminAmt.ppEnabled = True
                txtAdminCnt.ppEnabled = True
                txtStaffAmt.ppEnabled = True
                txtStaffCnt.ppEnabled = True
                'DOCMENP001-0006
                '                txtSystemAdminAmt.ppEnabled = True
                '                txtSystemAdminCnt.ppEnabled = True
                lblSystemAdminAmt.Enabled = True
                lblSystemAdminCnt.Enabled = True
                'DOCMENP001-0006
                txtKyuden1.ppEnabled = True
                txtKyuden2.ppEnabled = True
                txtSyomohin1.ppEnabled = True
                txtSyomohin2.ppEnabled = True
                txtSyomohin3.ppEnabled = True
                txtSyomohin4.ppEnabled = True
                txtKonetuhi1.ppEnabled = True
                txtKonetuhi2.ppEnabled = True
                txtUnsohi1.ppEnabled = True
                txtUnsohi2.ppEnabled = True
                txtUnsohi3.ppEnabled = True
                txtUnsohi4.ppEnabled = True
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True
                Master.Master.ppRigthButton2.Text = "締め"

                BtnJinkenhiIns.Enabled = True
                'btnJippiIns.Enabled = True
                'btnJippiUpd.Enabled = True
        End Select

        'ボタン押下時のメッセージ表示再設定
        Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '当月集計確認
        Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め確認

        '一覧
        For Each objRow As GridViewRow In grvList.Rows
            'ＴＢＯＸ吸上げ作業処理件数の帳票は存在しないため、常に非活性を設定
            If CType(objRow.Cells(eColGrv.検収書).Controls(0), TextBox).Text = "TBOX吸上げ作業処理件数" Then
                objRow.Cells(eColGrv.印刷).Enabled = False
            End If
        Next
    End Sub

#End Region

#Region "■ 当月集計処理"

    ''' <summary>
    ''' 当月集計処理(帳票DBの登録/更新)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSyukei()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Using objTrans As SqlTransaction = objCon.BeginTransaction()
                Try
                    'モード、検収日付取得
                    Dim intMode As String
                    If mfGetShimeJotai() Is Nothing Then
                        intMode = "0"   '登録
                    Else
                        intMode = "1"   '更新
                    End If
                    msDBUpdate(objCon, objTrans, intMode, "DOCMENP001_U13")
                    msDBUpdate(objCon, objTrans, intMode, "DOCMENP001_U14")
                    msDBUpdate(objCon, objTrans, intMode, "DOCMENP001_U15")
                    msDBUpdate(objCon, objTrans, intMode, "DOCMENP001_U16")

                    'トランザクション開始
                    'ストアドの設定
                    objCmd = New SqlCommand("DOCMENP001_I1")
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Transaction = objTrans

                    With objCmd.Parameters
                        '更新
                        .Add(pfSet_Param("InsUpd", SqlDbType.NVarChar, intMode))
                        .Add(pfSet_Param("DEGREE", SqlDbType.NVarChar, txtKensyuYm.ppText.Replace("/", "").Trim))
                        .Add(pfSet_Param("ADM_AMT", SqlDbType.Money, mfGetZero(txtAdminAmt.ppText.Trim)))
                        .Add(pfSet_Param("ADM_CNT", SqlDbType.Int, mfGetZero(txtAdminCnt.ppText.Trim)))
                        .Add(pfSet_Param("STF_AMT", SqlDbType.Money, mfGetZero(txtStaffAmt.ppText.Trim)))
                        .Add(pfSet_Param("STF_CNT", SqlDbType.Int, mfGetZero(txtStaffCnt.ppText.Trim)))
                        'DOCMENP001-0006
                        '                        .Add(pfSet_Param("SYS_AMT", SqlDbType.Money, mfGetZero(txtSystemAdminAmt.ppText.Trim)))
                        '                        .Add(pfSet_Param("SYS_CNT", SqlDbType.Int, mfGetZero(txtSystemAdminCnt.ppText.Trim)))
                        .Add(pfSet_Param("SYS_AMT", SqlDbType.Money, 0))
                        .Add(pfSet_Param("SYS_CNT", SqlDbType.Int, 0))
                        'DOCMENP001-0006
                        .Add(pfSet_Param("PFD_AMT1", SqlDbType.Money, mfGetZero(txtKyuden1.ppText.Trim)))
                        .Add(pfSet_Param("PFD_AMT2", SqlDbType.Money, mfGetZero(txtKyuden2.ppText.Trim)))
                        .Add(pfSet_Param("SPLY_COST1", SqlDbType.Money, mfGetZero(txtSyomohin1.ppText)))
                        .Add(pfSet_Param("SPLY_COST2", SqlDbType.Money, mfGetZero(txtSyomohin2.ppText)))
                        .Add(pfSet_Param("SPLY_COST3", SqlDbType.Money, mfGetZero(txtSyomohin3.ppText)))
                        .Add(pfSet_Param("SPLY_COST4", SqlDbType.Money, mfGetZero(txtSyomohin4.ppText)))
                        .Add(pfSet_Param("UTILITY_COST", SqlDbType.Money, mfGetZero(txtKonetuhi1.ppText.Trim)))
                        .Add(pfSet_Param("UTL_AMT2", SqlDbType.Money, mfGetZero(txtKonetuhi2.ppText.Trim)))
                        .Add(pfSet_Param("TRANS_COST1", SqlDbType.Money, mfGetZero(txtUnsohi1.ppText)))
                        .Add(pfSet_Param("TRANS_COST2", SqlDbType.Money, mfGetZero(txtUnsohi2.ppText)))
                        .Add(pfSet_Param("TRANS_COST3", SqlDbType.Money, mfGetZero(txtUnsohi3.ppText)))
                        .Add(pfSet_Param("TRANS_COST4", SqlDbType.Money, mfGetZero(txtUnsohi4.ppText)))
                        .Add(pfSet_Param("INSERT_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                    End With

                    'SQL実行
                    objCmd.ExecuteNonQuery()

                    'コミット
                    objTrans.Commit()

                    'CSV作成を行い、合計を取得
                    If mfCreateCSV(objCmd, objCon) = False Then
                        '完了メッセージ
                        psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "データベースの更新後、CSVファイルの保存に失敗しました。\n再度、当月集計")
                    Else
                        '完了メッセージ
                        psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "当月集計処理")
                    End If

                    '再検索
                    msDataRead(txtKensyuYm.ppText.Replace("/", "").Trim)

                Catch ex As Exception
                    objTrans.Rollback()
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "当月集計処理")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCon) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    End If
                End Try
            End Using
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        End If
    End Sub

#End Region

#Region "■ 検収月取得"

    ''' <summary>
    ''' 検収月の取得
    ''' </summary>
    ''' <returns>負数：エラー 0：新規　 1：更新</returns>
    ''' <remarks></remarks>
    Private Function mfGetKensyuYmTogetu(ByRef strKensyuYm As String) As Integer
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        strKensyuYm = ""
        mfGetKensyuYmTogetu = 0

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                '検収月の算出
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S13")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmDATE", SqlDbType.NVarChar, Me.txtKensyuYm.ppText.Replace("/", "")))
                    .Add(pfSet_Param("prmINS", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                    .Add(pfSet_Param("prmREQ", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                    .Add(pfSet_Param("prmErr", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                'エラー？
                If objCmd.Parameters("prmErr").Value <> 0 Then
                    Throw New Exception()
                End If

                '検収月
                strKensyuYm = objDs.Tables(0).Rows(0)("検収年月")

                '更新か新規か
                If objCmd.Parameters("prmINS").Value = "" Or (objCmd.Parameters("prmINS").Value = "1" AndAlso objCmd.Parameters("prmREQ").Value = "1") Then
                    '新規データ登録
                    mfGetKensyuYmTogetu = 0
                Else
                    'データ更新
                    mfGetKensyuYmTogetu = 1
                End If

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "検収月の取得")
                strKensyuYm = ""
                mfGetKensyuYmTogetu = -1
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    strKensyuYm = ""
                    mfGetKensyuYmTogetu = -1
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            strKensyuYm = ""
            mfGetKensyuYmTogetu = -1
        End If
    End Function

#End Region

#Region "■ 締め/締め解除処理"

    ''' <summary>
    ''' 締め/締め解除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfShime(ByVal strShimeOrKaijyo As String) As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim strBtnNm As String = ""

        mfShime = False
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'トランザクション開始
                Using objTran As SqlTransaction = objCon.BeginTransaction()
                    '検収月のチェック

                    'ストアド設定
                    objCmd = New SqlCommand("DOCMENP001_U10")
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Transaction = objTran

                    strBtnNm = Master.Master.ppRigthButton2.Text

                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmD91_INS_CLS", SqlDbType.NVarChar, strShimeOrKaijyo))
                        .Add(pfSet_Param("prmD91_UPDATE_DT", SqlDbType.DateTime, DateTime.Now))
                        .Add(pfSet_Param("prmD91_UPDATE_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                        .Add(pfSet_Param("prmD91_DEGREE", SqlDbType.NVarChar, txtKensyuYm.ppText.Replace("/", "").Trim))
                    End With

                    '更新(締め/締め解除)
                    objCmd.ExecuteNonQuery()

                    'コミット
                    objTran.Commit()

                    '完了メッセ
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, strBtnNm & "処理")

                    mfShime = True
                End Using

            Catch ex As Exception
                mfShime = False
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strBtnNm & "処理")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    mfShime = False
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            mfShime = False
        End If

        msSetEnable(mfSettingEnableMode())
    End Function

#End Region

#Region "■ 締め状態チェック"

    ''' <summary>
    ''' 状態チェック
    ''' </summary>
    ''' <param name="intMode">1:締め　2:締め解除　3:当月集計　4:更新</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkJotai(ByVal intMode As Integer) As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        mfChkJotai = False

        If Me.txtKensyuYm.ppText = "" Then
            psMesBox(Me, "30016", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "検収年月")
            Exit Function
        End If

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("DOCMENP001_S11")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmDATE", SqlDbType.NVarChar, Me.txtKensyuYm.ppText.Replace("/", "")))
                    .Add(pfSet_Param("prmINS", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                    .Add(pfSet_Param("prmREQ", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                    .Add(pfSet_Param("prmErr", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With

                'SQL実行
                clsDataConnect.pfGet_DataSet(objCmd)

                'エラー
                If objCmd.Parameters("prmErr").Value <> "0" Then
                    Throw New Exception()
                End If

                '締めている書類を取得
                Dim strMesPrm As String = "検収書"
                If objCmd.Parameters("prmREQ").Value = "1" Then
                    strMesPrm = "請求書"
                End If

                Select Case intMode
                    Case 1 '締め
                        If Not (objCmd.Parameters("prmINS").Value = "0" AndAlso objCmd.Parameters("prmREQ").Value = "0") Then
                            psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理")
                            mfChkJotai = False
                            Exit Function
                        End If

                    Case 2 '締め解除
                        If Not (objCmd.Parameters("prmINS").Value = "1" AndAlso objCmd.Parameters("prmREQ").Value = "0") Then
                            If objCmd.Parameters("prmREQ").Value = "1" Then
                                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理", "締め解除")
                            Else
                                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め解除")
                            End If

                            mfChkJotai = False
                            Exit Function
                        End If

                    Case 3 '当月集計
                        If Not (objCmd.Parameters("prmINS").Value = "0" AndAlso objCmd.Parameters("prmREQ").Value = "0" _
                                OrElse objCmd.Parameters("prmINS").Value = "1" AndAlso objCmd.Parameters("prmREQ").Value = "1" _
                                OrElse objCmd.Parameters("prmINS").Value = "" AndAlso objCmd.Parameters("prmREQ").Value = "") Then
                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理", "当月集計が")
                            mfChkJotai = False
                            Exit Function
                        End If

                    Case 4 '更新
                        If Not (objCmd.Parameters("prmINS").Value = "0" AndAlso objCmd.Parameters("prmREQ").Value = "0") Then
                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, strMesPrm & "の締め処理", "更新")
                            mfChkJotai = False
                            Exit Function
                        End If
                End Select

                mfChkJotai = True
                Exit Function

            Catch ex As Exception
                mfChkJotai = False

                'システムエラー
                Select Case intMode
                    Case 1 '締め
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "締め処理")
                    Case 2 '締め解除
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "締め解除処理")
                    Case 3 '当月集計
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "当月集計処理")
                    Case 4 '更新
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "更新処理")
                End Select

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    mfChkJotai = False
                End If
            End Try
        Else
            mfChkJotai = False
        End If

    End Function

#End Region

#Region "■ 更新処理"

    ''' <summary>
    ''' マスタ更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msMasterUpdate()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'トランザクション開始
                Using objTrans As SqlTransaction = objCon.BeginTransaction()

                    'ストアド設定
                    objCmd = New SqlCommand("DOCMENP001_U18")
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Transaction = objTrans
                    '検収月を取得
                    Dim strKensyuYm As String = Me.txtKensyuYm.ppText.Replace("/", "")
                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmAdminAMT", SqlDbType.Money, mfGetZero(txtAdminAmt.ppText.Trim)))
                        .Add(pfSet_Param("prmAdminCNT", SqlDbType.Int, mfGetZero(txtAdminCnt.ppText.Trim)))
                        .Add(pfSet_Param("prmStaffAMT", SqlDbType.Money, mfGetZero(txtStaffAmt.ppText.Trim)))
                        .Add(pfSet_Param("prmStaffCNT", SqlDbType.Int, mfGetZero(txtStaffCnt.ppText.Trim)))
                        'DOCMENP001-0006
                        '                        .Add(pfSet_Param("prmSysAdAMT", SqlDbType.Money, mfGetZero(txtSystemAdminAmt.ppText.Trim)))
                        '                        .Add(pfSet_Param("prmSysAdCNT", SqlDbType.Int, mfGetZero(txtSystemAdminCnt.ppText.Trim)))
                        .Add(pfSet_Param("prmSysAdAMT", SqlDbType.Money, 0))
                        .Add(pfSet_Param("prmSysAdCNT", SqlDbType.Int, 0))
                        'DOCMENP001-0006
                        .Add(pfSet_Param("prmDATE_DT", SqlDbType.DateTime, DateTime.Now))
                        .Add(pfSet_Param("prmUSR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                        .Add(pfSet_Param("prmKensyuYM", SqlDbType.NVarChar, strKensyuYm))
                    End With

                    'SQL実行
                    objCmd.ExecuteNonQuery()

                    'コミット
                    objTrans.Commit()

                    '再描画
                    msMasterRewrite()

                    '完了メッセ
                    psMesBox(Me, "10006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "金額を変更した為、当月集計")

                End Using

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "更新処理")
            Finally
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        End If
    End Sub

    Private Sub msMasterRewrite()
        Dim drMaster As DataRow = mfGetMasterData()

        'テキスト項目に設定
        msJinkenAreaClear()

        If drMaster Is Nothing Then
            'テキスト項目に設定
            txtAdminAmt.ppText = "0"
            txtAdminCnt.ppText = "0"
            lblNewAdminTotal.Text = "\ 0"
            txtStaffAmt.ppText = "0"
            txtStaffCnt.ppText = "0"
            lblNewStaffTotal.Text = "\ 0"
            'DOCMENP001-0006
            lblAdminAmt.Text = "0"
            lblAdminCnt.Text = "0"
            lblAdminTotal.Text = "\ 0"
            lblStaffAmt.Text = "0"
            lblStaffCnt.Text = "0"
            lblStaffTotal.Text = "\ 0"
            '            txtSystemAdminAmt.ppText = "0"
            '            txtSystemAdminCnt.ppText = "0"
            '            lblNewSystemAdminTotal.Text = "\ 0"
            lblSystemAdminAmt.Text = "0"
            lblSystemAdminCnt.Text = "0"
            lblSystemAdminTotal.Text = "\ 0"
            'DOCMENP001-0006
        Else
            txtAdminAmt.ppText = Math.Truncate(drMaster("管理者金額"))
            txtAdminCnt.ppText = drMaster("管理者人数")
            lblNewAdminTotal.Text = "\ " & Integer.Parse(txtAdminAmt.ppText * txtAdminCnt.ppText).ToString("N0")
            txtStaffAmt.ppText = Math.Truncate(drMaster("業務担当者金額"))
            txtStaffCnt.ppText = drMaster("業務担当者人数")
            lblNewStaffTotal.Text = "\ " & Integer.Parse(txtStaffAmt.ppText * txtStaffCnt.ppText).ToString("N0")
            'DOCMENP001-0006
            lblAdminAmt.Text = "\ " & Decimal.Parse(drMaster("管理者金額")).ToString("N0")
            lblAdminCnt.Text = drMaster("管理者人数") & " 人"
            lblAdminTotal.Text = "\ " & Decimal.Parse(drMaster("管理者金額") * drMaster("管理者人数")).ToString("N0")
            lblStaffAmt.Text = "\ " & Decimal.Parse(drMaster("業務担当者金額")).ToString("N0")
            lblStaffCnt.Text = drMaster("業務担当者人数") & " 人"
            lblStaffTotal.Text = "\ " & Decimal.Parse(drMaster("業務担当者金額") * drMaster("業務担当者人数")).ToString("N0")
            '            txtSystemAdminAmt.ppText = Math.Truncate(drMaster("システム運用管理者金額"))
            '            txtSystemAdminCnt.ppText = drMaster("システム運用管理者人数")
            '            lblNewSystemAdminTotal.Text = "\ " & Integer.Parse(txtSystemAdminAmt.ppText * txtSystemAdminCnt.ppText).ToString("N0")
            lblSystemAdminAmt.Text = "\ " & Decimal.Parse(drMaster("システム運用管理者金額")).ToString("N0")
            lblSystemAdminCnt.Text = drMaster("システム運用管理者人数") & " 人"
            lblSystemAdminTotal.Text = "\ " & Decimal.Parse(drMaster("システム運用管理者金額") * drMaster("システム運用管理者人数")).ToString("N0")
            'DOCMENP001-0006
        End If

    End Sub
    ''' <summary>
    ''' ワークテーブル登録/更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msWrkUpdate(ByVal btnName As String)
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim strInsUpdMode As String = ""

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'トランザクション開始
                Using objTrans As SqlTransaction = objCon.BeginTransaction()

                    'ストアド設定
                    objCmd = New SqlCommand("DOCMENP001_U19")
                    objCmd.Connection = objCon
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.Transaction = objTrans

                    '検収月を取得
                    Dim strKensyuYm As String = Me.txtKensyuYm.ppText.Replace("/", "")

                    'パラメータ設定
                    With objCmd.Parameters
                        .Add(pfSet_Param("prmKensyuYM", SqlDbType.VarChar, strKensyuYm))
                        .Add(pfSet_Param("prmKyuden_AMT1", SqlDbType.Money, mfGetZero(txtKyuden1.ppText.Trim)))
                        .Add(pfSet_Param("prmKyuden_AMT2", SqlDbType.Money, mfGetZero(txtKyuden2.ppText.Trim)))
                        .Add(pfSet_Param("prmSyomo_AMT1", SqlDbType.Money, mfGetZero(txtSyomohin1.ppText)))
                        .Add(pfSet_Param("prmSyomo_AMT2", SqlDbType.Money, mfGetZero(txtSyomohin2.ppText)))
                        .Add(pfSet_Param("prmSyomo_AMT3", SqlDbType.Money, mfGetZero(txtSyomohin3.ppText)))
                        .Add(pfSet_Param("prmSyomo_AMT4", SqlDbType.Money, mfGetZero(txtSyomohin4.ppText)))
                        .Add(pfSet_Param("prmKonetuhi_AMT1", SqlDbType.Money, mfGetZero(txtKonetuhi1.ppText.Trim)))
                        .Add(pfSet_Param("prmKonetuhi_AMT2", SqlDbType.Money, mfGetZero(txtKonetuhi2.ppText.Trim)))
                        .Add(pfSet_Param("prmUnsohi1", SqlDbType.Money, mfGetZero(txtUnsohi1.ppText)))
                        .Add(pfSet_Param("prmUnsohi2", SqlDbType.Money, mfGetZero(txtUnsohi2.ppText)))
                        .Add(pfSet_Param("prmUnsohi3", SqlDbType.Money, mfGetZero(txtUnsohi3.ppText)))
                        .Add(pfSet_Param("prmUnsohi4", SqlDbType.Money, mfGetZero(txtUnsohi4.ppText)))
                        .Add(pfSet_Param("prmDATE_DT", SqlDbType.DateTime, DateTime.Now))
                        .Add(pfSet_Param("prmUSR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                    End With

                    'SQL実行
                    objCmd.ExecuteNonQuery()

                    'コミット
                    objTrans.Commit()

                    '再描画
                    msWrkRewrite()

                    '完了メッセ
                    psMesBox(Me, "10006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "金額を変更した為、当月集計")

                End Using

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "更新処理")
            Finally
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        End If
    End Sub
    Private Sub msWrkRewrite()
        Dim drWork As DataRow = mfGetWorkData()

        'テキスト項目に設定
        msJippiAreaClear()

        If drWork Is Nothing Then
            txtKyuden1.ppText = "0"
            txtKyuden2.ppText = "0"
            lblKyudenTotal.Text = "\ 0"
            txtSyomohin1.ppText = "0"
            txtSyomohin2.ppText = "0"
            txtSyomohin3.ppText = "0"
            txtSyomohin4.ppText = "0"
            lblSyomohinTotal.Text = "\ 0"
            txtKonetuhi1.ppText = "0"
            txtKonetuhi2.ppText = "0"
            lblKonetuhiTotal.Text = "\ 0"
            txtUnsohi1.ppText = "0"
            txtUnsohi2.ppText = "0"
            txtUnsohi3.ppText = "0"
            txtUnsohi4.ppText = "0"
            lblUnsohiTotal.Text = "\ 0"
        Else
            txtKyuden1.ppText = Math.Truncate(drWork("給電作業費1"))
            txtKyuden2.ppText = Math.Truncate(drWork("給電作業費2"))
            lblKyudenTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("給電作業費合計"))).ToString("N0")
            txtSyomohin1.ppText = Math.Truncate(drWork("消耗品1"))
            txtSyomohin2.ppText = Math.Truncate(drWork("消耗品2"))
            txtSyomohin3.ppText = Math.Truncate(drWork("消耗品3"))
            txtSyomohin4.ppText = Math.Truncate(drWork("消耗品4"))
            lblSyomohinTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("消耗品合計"))).ToString("N0")
            txtKonetuhi1.ppText = Math.Truncate(drWork("光熱費1"))
            txtKonetuhi2.ppText = Math.Truncate(drWork("光熱費2"))
            lblKonetuhiTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("光熱費合計"))).ToString("N0")
            txtUnsohi1.ppText = Math.Truncate(drWork("運送費1"))
            txtUnsohi2.ppText = Math.Truncate(drWork("運送費2"))
            txtUnsohi3.ppText = Math.Truncate(drWork("運送費3"))
            txtUnsohi4.ppText = Math.Truncate(drWork("運送費4"))
            lblUnsohiTotal.Text = "\ " & Decimal.Parse(Math.Truncate(drWork("運送費合計"))).ToString("N0")
        End If
    End Sub

    Private Sub msDBUpdate(ByVal objCon As SqlConnection, ByVal objTrans As SqlTransaction, ByVal strInsUpd As String, ByVal strSqlCommandName As String)
        Dim objCmd As SqlCommand = Nothing

        Try
            'ストアド設定
            objCmd = New SqlCommand(strSqlCommandName)
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Transaction = objTrans
            '検収月を取得
            Dim strKensyuYm As String = Me.txtKensyuYm.ppText.Replace("/", "")
            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmCNTYM", SqlDbType.NVarChar, strKensyuYm))
                .Add(pfSet_Param("prmSWFLG", SqlDbType.NVarChar, strInsUpd))
                .Add(pfSet_Param("prmUSRID", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
            End With

            'SQL実行
            objCmd.ExecuteNonQuery()

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "更新処理")
        Finally
        End Try
    End Sub

#End Region

#Region "■ その他"

    ''' <summary>
    ''' DBNullの取得
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDbNull(ByVal strVal As String) As Object
        If strVal Is Nothing OrElse strVal.Trim() = "" Then
            Return DBNull.Value
        Else
            Return strVal
        End If
    End Function
    ''' <summary>
    ''' 空文字をゼロにする
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetZero(ByVal strVal As String) As Object
        If strVal Is Nothing OrElse strVal.Trim() = "" Then
            Return 0
        Else
            Return strVal
        End If
    End Function
    ''' <summary>
    ''' 全帳票印刷
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AllPrint()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim CsvData As New ArrayList
        Dim strArrayList As String() = Nothing
        Dim intCnt As Integer
        Dim strCsv As String
        Dim strAryCsv() As String
        Dim dtDOCREP002, dtBBPREP003, dtBBPREP001, dtBBPREP002, dtCDPREP001 As New DataTable
        Dim strNmDOCREP002, strNmBBPREP003, strNmBBPREP001, strNmBBPREP002, strNmCDPREP001 As String
        Dim strKensyuYM As String = txtKensyuYm.ppText.Replace("/", "").Trim


        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                Dim strKensyuYmSave As String = Me.txtKensyuYm.ppText
                Dim blnNewFlg As Boolean
                strNmDOCREP002 = ""
                strNmBBPREP003 = ""
                strNmBBPREP001 = ""
                strNmBBPREP002 = ""
                strNmCDPREP001 = ""
                If strKensyuYM >= 201603 Then
                    blnNewFlg = True
                Else
                    blnNewFlg = False
                End If

                If blnNewFlg = True Then
                    dtDOCREP002 = mfGetReportDataset(帳票.サポセン検収書).Tables(0)
                Else
                    'CSVファイル取り込み.
                    ms_GetCSVData("0941CL", "サポートセンタ運用の報告書兼検収書_[" & strKensyuYM & "]", CsvData, intCnt, strArrayList)

                    If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                        'データテーブルカラム作成(ヘッダ)
                        strNmDOCREP002 = "サポートセンタ運用の報告書兼検収書_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
                        'データテーブルカラム作成(ヘッダ)
                        For i As Integer = 0 To strArrayList.Length - 1
                            dtDOCREP002.Columns.Add(strArrayList(i))
                        Next

                        '行数分ループ
                        For i As Integer = 0 To CsvData.Count - 1
                            strAryCsv = Nothing
                            'カンマ毎に分割
                            strAryCsv = CsvData(i).Split(",")
                            dtDOCREP002.Rows.Add(strAryCsv)
                        Next
                    Else
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                End If

                CsvData.Clear()

                Dim strKensyuYYYY As String = String.Empty
                Dim strKensyuMM As String = String.Empty

                Select Case strKensyuYM.Substring(4, 2)
                    Case "01", "02", "03"
                        strKensyuYYYY = (Integer.Parse(strKensyuYM.Substring(0, 4)) - 1).ToString
                    Case Else
                        strKensyuYYYY = strKensyuYM.Substring(0, 4)
                End Select

                '4月から翌年の3月までのCSVを順番に取り込む
                Dim CsvDataWrk As New ArrayList
                Dim strArrayListWrk As String() = Nothing
                Dim blnIsExistsData As Boolean = False
                Dim strTmpLine As String = String.Empty
                For zz As Integer = 1 To 12
                    If zz < 10 Then
                        strKensyuMM = (zz + 3).ToString("00")
                    Else
                        If zz = 10 Then
                            strKensyuYYYY = (Integer.Parse(strKensyuYYYY) + 1).ToString
                        End If
                        strKensyuMM = (zz - 9).ToString("00")
                    End If

                    '初期化
                    CsvDataWrk.Clear()

                    'CSVファイル取り込み.
                    strTmpLine = String.Empty
                    '取りこんだデータを順々に追加していく
                    If blnNewFlg = True Then
                        'Dim dsTmp As DataSet = mfGetReportDataset(帳票.券・サ・BB1, strKensyuYYYY + strKensyuMM)
                        Dim dsTmp As DataSet = mfGetReportDataset(帳票.券サBB1, strKensyuYYYY + strKensyuMM)
                        If dsTmp Is Nothing OrElse dsTmp.Tables(0).Rows.Count = 0 Then
                            strTmpLine = DateTime.Now.ToString("yyyy/MM/dd") & "," & strKensyuYYYY & "," & strKensyuMM & ",券売機,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,サンド,,,,,計,,,,"
                        Else
                            Dim dsCSVStyle As DataSet = mfConvertCSVStyle(dsTmp)
                            Dim strTmp As Object() = dsCSVStyle.Tables(0).Rows(0).ItemArray
                            ReDim strArrayListWrk(dsCSVStyle.Tables(0).Columns.Count - 1)

                            Dim intI As Integer = 0
                            For Each clm As DataColumn In dsCSVStyle.Tables(0).Columns
                                strArrayListWrk(intI) = clm.ColumnName.ToString
                                intI += 1
                            Next

                            strTmpLine = String.Join(",", strTmp)
                            blnIsExistsData = True
                        End If
                    Else
                        If ms_GetCSVData("0942CL", "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYYYY & strKensyuMM & "]", CsvDataWrk, intCnt, strArrayListWrk) = False Then
                            strTmpLine = DateTime.Now.ToString("yyyy/MM/dd") & "," & strKensyuYYYY & "," & strKensyuMM & ",券売機,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,サンド,,,,,計,,,,"
                        Else
                            strTmpLine = CsvDataWrk(0).ToString
                        End If
                    End If
                    CsvData.Add(strTmpLine)
                    blnIsExistsData = True

                    '■繰り返し処理の中で一番長いstrArrayListWrkをstrArrayListに昇格
                    If strArrayListWrk IsNot Nothing Then
                        If strArrayList Is Nothing Then
                            strArrayList = strArrayListWrk
                        Else
                            If strArrayList.Length < strArrayListWrk.Length Then
                                strArrayList = strArrayListWrk
                            End If
                        End If
                    End If

                Next

                'データが一個もない
                If blnIsExistsData = False Then
                    psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "券売機・サンドＢＢ１読み出し処理件数")
                End If

                'CSVファイル取り込み.
                'ms_GetCSVData("0942CL", "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYm & "]", CsvData, intCnt, strArrayList)

                If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                    'CSVのままのデータ形態では印刷ができないので加工する
                    Dim PrData As New ArrayList
                    Dim strPrTitle() As String
                    Dim strPrData() As String
                    Dim dt As New DataTable
                    Dim intMAX As Integer
                    Dim intRows As Integer

                    'データテーブルカラム作成(ヘッダ)
                    For intIndex = 0 To strArrayList.Length - 1
                        dt.Columns.Add(strArrayList(intIndex))
                    Next

                    '行数分ループ
                    'まず最大列数を取得する
                    Dim intColCnt As Integer = strArrayList.Length
                    Dim intColCntWrk As Integer

                    For intIndex = 0 To CsvData.Count - 1
                        strPrData = Nothing
                        'カンマ毎に分割
                        strPrData = CsvData(intIndex).ToString.Split(",")

                        '■CSVのデータを成形してからデータテーブルに保管する
                        'まず処理する行の列数を取得する
                        If strPrData.Count <> intColCnt Then
                            'ワーク用の配列に退避
                            Dim strPrDataWrk() As String
                            strPrDataWrk = strPrData
                            intColCntWrk = strPrDataWrk.Length - 10
                            '配列を最大列数で初期化
                            ReDim strPrData(intColCnt - 1)
                            '配列の中身を埋めていく
                            For zz As Integer = 0 To strPrData.Length - 1
                                '元データの最後の10列まではそのまま入れる
                                If zz < strPrDataWrk.Length - 10 Then
                                    strPrData(zz) = strPrDataWrk(zz)
                                Else
                                    '最大列数より10列分前までは空文字を入れる
                                    If zz < intColCnt - 10 Then
                                        strPrData(zz) = String.Empty
                                    Else
                                        '元データの残り10列を入れる
                                        strPrData(zz) = strPrDataWrk(intColCntWrk)
                                        intColCntWrk = intColCntWrk + 1
                                    End If
                                End If
                            Next
                        End If

                        dt.Rows.Add(strPrData)
                    Next

                    intMAX = (dt.Columns.Count - 3) / 5

                    ReDim strPrTitle(60)
                    strPrTitle(0) = "検収年"
                    For intIndex = 1 To 12
                        strPrTitle(1 + (0 + ((intIndex - 1) * 5))) = "区分名" & intIndex.ToString
                        strPrTitle(1 + (1 + ((intIndex - 1) * 5))) = "券売機日付" & intIndex.ToString
                        strPrTitle(1 + (2 + ((intIndex - 1) * 5))) = "西日本券件数" & intIndex.ToString
                        strPrTitle(1 + (3 + ((intIndex - 1) * 5))) = "東日本券件数" & intIndex.ToString
                        strPrTitle(1 + (4 + ((intIndex - 1) * 5))) = "券件数" & intIndex.ToString & "計"
                    Next

                    For intIndex = 0 To intMAX - 1
                        strCsv = ""
                        For intRows = 0 To dt.Rows.Count - 1
                            If intRows <> 0 Then
                                strCsv &= ","
                            Else
                                strCsv = dt.Rows(intRows).Item(1).ToString()
                                strCsv &= ","
                            End If
                            strCsv &= dt.Rows(intRows).Item(3 + (intIndex * 5)).ToString()
                            strCsv &= ","
                            strCsv &= dt.Rows(intRows).Item(4 + (intIndex * 5)).ToString()
                            strCsv &= ","
                            strCsv &= dt.Rows(intRows).Item(6 + (intIndex * 5)).ToString()
                            strCsv &= ","
                            strCsv &= dt.Rows(intRows).Item(5 + (intIndex * 5)).ToString()
                            strCsv &= ","
                            strCsv &= dt.Rows(intRows).Item(7 + (intIndex * 5)).ToString()
                        Next

                        PrData.Add(strCsv)
                    Next

                    'データテーブルカラム作成(ヘッダ)
                    strNmBBPREP003 = "券売機・サンドＢＢ１読み出し処理件数_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
                    'データテーブルカラム作成(ヘッダ)
                    For i As Integer = 0 To strPrTitle.Length - 1
                        dtBBPREP003.Columns.Add(strPrTitle(i))
                    Next

                    '行数分ループ
                    For i As Integer = 0 To PrData.Count - 1
                        strAryCsv = Nothing
                        'カンマ毎に分割
                        strAryCsv = PrData(i).Split(",")
                        dtBBPREP003.Rows.Add(strAryCsv)
                    Next
                Else
                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If

                CsvData.Clear()

                If blnNewFlg = True Then
                    dtBBPREP001 = mfGetReportDataset(帳票.券売機明細).Tables(0)
                Else
                    'CSVファイル取り込み.
                    ms_GetCSVData("0943CL", "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]", CsvData, intCnt, strArrayList)

                    If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                        'データテーブルカラム作成(ヘッダ)
                        strNmBBPREP001 = "券売機ＢＢ１読み出し処理件数明細_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
                        'データテーブルカラム作成(ヘッダ)
                        For i As Integer = 0 To strArrayList.Length - 1
                            dtBBPREP001.Columns.Add(strArrayList(i))
                        Next

                        '行数分ループ
                        For i As Integer = 0 To CsvData.Count - 1
                            strAryCsv = Nothing
                            'カンマ毎に分割
                            strAryCsv = CsvData(i).Split(",")
                            dtBBPREP001.Rows.Add(strAryCsv)
                        Next
                    Else
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                End If


                CsvData.Clear()
                If blnNewFlg = True Then
                    dtBBPREP002 = mfGetReportDataset(帳票.サンド明細).Tables(0)
                Else
                    'CSVファイル取り込み.
                    ms_GetCSVData("0944CL", "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]", CsvData, intCnt, strArrayList)

                    If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                        'データテーブルカラム作成(ヘッダ)
                        strNmBBPREP002 = "サンドＢＢ１読み出し処理件数明細_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
                        'データテーブルカラム作成(ヘッダ)
                        For i As Integer = 0 To strArrayList.Length - 1
                            dtBBPREP002.Columns.Add(strArrayList(i))
                        Next

                        '行数分ループ
                        For i As Integer = 0 To CsvData.Count - 1
                            strAryCsv = Nothing
                            'カンマ毎に分割
                            strAryCsv = CsvData(i).Split(",")
                            dtBBPREP002.Rows.Add(strAryCsv)
                        Next
                    Else
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                End If

                CsvData.Clear()
                'CSVファイル取り込み.
                If blnNewFlg = True Then
                    dtCDPREP001 = mfGetReportDataset(帳票.カードDB).Tables(0)
                Else
                    ms_GetCSVData("0945CL", "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYM & "]", CsvData, intCnt, strArrayList)

                    If Not CsvData Is Nothing AndAlso CsvData.Count > 0 Then
                        'PDFファイル作成.
                        strNmCDPREP001 = "使用中カードＤＢ吸上げ作業明細_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
                        'データテーブルカラム作成(ヘッダ)
                        For i As Integer = 0 To strArrayList.Length - 1
                            dtCDPREP001.Columns.Add(strArrayList(i))
                        Next

                        '行数分ループ
                        For i As Integer = 0 To CsvData.Count - 1
                            strAryCsv = Nothing
                            'カンマ毎に分割
                            strAryCsv = CsvData(i).Split(",")
                            dtCDPREP001.Rows.Add(strAryCsv)
                        Next
                    Else
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                End If
                If strKensyuYM >= 201610 Then
                    'PDF出力処理.
                    psPrintPDF(Me, {New DOCREP002_2, New BBPREP003, New BBPREP001, New BBPREP002, New CDPREP001_1}, _
                               {dtDOCREP002, dtBBPREP003, dtBBPREP001, dtBBPREP002, dtCDPREP001}, _
                               {strNmDOCREP002, strNmBBPREP003, strNmBBPREP001, strNmBBPREP002, strNmCDPREP001})
                ElseIf strKensyuYM >= 201603 Then
                    'PDF出力処理.
                    psPrintPDF(Me, {New DOCREP002_1, New BBPREP003, New BBPREP001, New BBPREP002, New CDPREP001}, _
                               {dtDOCREP002, dtBBPREP003, dtBBPREP001, dtBBPREP002, dtCDPREP001}, _
                               {strNmDOCREP002, strNmBBPREP003, strNmBBPREP001, strNmBBPREP002, strNmCDPREP001})
                Else
                    'PDF出力処理.
                    psPrintPDF(Me, {New DOCREP002, New BBPREP003, New BBPREP001, New BBPREP002, New CDPREP001}, _
                               {dtDOCREP002, dtBBPREP003, dtBBPREP001, dtBBPREP002, dtCDPREP001}, _
                               {strNmDOCREP002, strNmBBPREP003, strNmBBPREP001, strNmBBPREP002, strNmCDPREP001})
                End If



            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "帳票出力処理")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

#End Region

#Region "■ CSV処理"

    ''' <summary>
    ''' CSV作成
    ''' </summary>
    ''' <param name="objCmd"></param>
    ''' <param name="objCon"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCreateCSV(ByVal objCmd As SqlCommand, ByVal objCon As SqlConnection) As Boolean
        Dim objDs As DataSet = New DataSet
        Dim objDr As DataRow = Nothing
        Dim strKensyuYM As String = txtKensyuYm.ppText.Replace("/", "").Trim
        Dim intIndex As Integer
        Dim intMax As Integer
        Dim objItem As DataSet
        Dim objDt As DataTable
        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                For intShori As Integer = 0 To 5
                    Dim strFileclassCD As String = ""
                    Dim strReportName As String = ""

                    Select Case intShori
                        Case 0  'サポートセンタ検収書作成処理
                            'ストアド設定
                            objCmd = New SqlCommand("DOCMENP001_S1")
                            objCmd.Connection = objCon
                            objCmd.CommandType = CommandType.StoredProcedure
                            'パラメータ設定
                            objCmd.Parameters.Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                            'SQL実行
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)

                            strReportName = "サポートセンタ運用の報告書兼検収書_[" & strKensyuYM & "]"
                            strFileclassCD = "0941CL"

                        Case 1  '券売機サンドＢＢ１読出し処理件数
                            objCmd = New SqlCommand("DOCMENP001_S2")

                            With objCmd.Parameters
                                .Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                                '.Add(pfSet_Param("prm_year", SqlDbType.NVarChar, strKensyuYM.Substring(0, 4)))
                                '.Add(pfSet_Param("prm_month", SqlDbType.NVarChar, strKensyuYM.Substring(4, 2)))
                            End With

                            strFileclassCD = "0942CL"
                            strReportName = "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYM & "]"

                            objCmd.Connection = objCon
                            objCmd.CommandType = CommandType.StoredProcedure

                            'SQL実行
                            objItem = clsDataConnect.pfGet_DataSet(objCmd)

                            objDt = New DataTable
                            objDt.Columns.Add("帳票出力日")
                            objDt.Columns.Add("検収年")
                            objDt.Columns.Add("集計月")

                            intMax = 0
                            For intIndex = 1 To 12
                                If intMax < objItem.Tables(0).Select("集計月 = '" & intIndex.ToString("00") & "' AND 区分 = '券売機'").Count Then
                                    intMax = objItem.Tables(0).Select("集計月 = '" & intIndex.ToString("00") & "' AND 区分 = '券売機'").Count
                                End If
                            Next

                            If (intMax Mod 8) > 6 Then
                                intMax = intMax + (6 + (8 - (intMax Mod 8)))
                            ElseIf (intMax Mod 8) < 6 Then
                                intMax = intMax + (6 - (intMax Mod 8))
                            End If

                            For intIndex = 1 To intMax + 2
                                objDt.Columns.Add("区分" & intIndex.ToString("00"))
                                objDt.Columns.Add("日付" & intIndex.ToString("00"))
                                objDt.Columns.Add("東日本件数" & intIndex.ToString("00"))
                                objDt.Columns.Add("西日本件数" & intIndex.ToString("00"))
                                objDt.Columns.Add("件数計" & intIndex.ToString("00"))
                            Next

                            Dim intCnt As Integer = 0
                            Dim intBrank As Integer = 0
                            Dim strDate As String = ""

                            For intIndex = 0 To objItem.Tables(0).Rows.Count - 1
                                If strDate <> objItem.Tables(0).Rows(intIndex).Item("集計月").ToString Then
                                    If intIndex > 0 Then
                                        objDt.Rows.Add(objDr)
                                    End If

                                    objDr = objDt.NewRow
                                    objDr.Item("帳票出力日") = objItem.Tables(0).Rows(intIndex).Item("帳票出力日").ToString
                                    objDr.Item("検収年") = objItem.Tables(0).Rows(intIndex).Item("検収年").ToString
                                    objDr.Item("集計月") = objItem.Tables(0).Rows(intIndex).Item("集計月").ToString
                                    strDate = objItem.Tables(0).Rows(intIndex).Item("集計月").ToString
                                    intCnt = 1
                                End If

                                If objItem.Tables(0).Rows(intIndex).Item("区分").ToString = "サンド" Then
                                    For intCnt = intCnt To intMax
                                        objDr.Item("区分" & intCnt.ToString("00")) = ""
                                        objDr.Item("日付" & intCnt.ToString("00")) = ""
                                        objDr.Item("東日本件数" & intCnt.ToString("00")) = ""
                                        objDr.Item("西日本件数" & intCnt.ToString("00")) = ""
                                        objDr.Item("件数計" & intCnt.ToString("00")) = ""
                                    Next
                                End If

                                objDr.Item("区分" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("区分").ToString
                                If objItem.Tables(0).Rows(intIndex).Item("日付").ToString <> "" Then
                                    objDr.Item("日付" & intCnt.ToString("00")) = Date.Parse(objItem.Tables(0).Rows(intIndex).Item("日付").ToString).ToString("MM/dd")
                                Else
                                    objDr.Item("日付" & intCnt.ToString("00")) = ""
                                End If

                                If objItem.Tables(0).Rows(intIndex).Item("件数計").ToString = "0" Then
                                    objDr.Item("東日本件数" & intCnt.ToString("00")) = ""
                                    objDr.Item("西日本件数" & intCnt.ToString("00")) = ""
                                    objDr.Item("件数計" & intCnt.ToString("00")) = ""
                                Else
                                    objDr.Item("東日本件数" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("東日本件数").ToString
                                    objDr.Item("西日本件数" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("西日本件数").ToString
                                    objDr.Item("件数計" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("件数計").ToString
                                End If

                                intCnt += 1

                            Next
                            objDt.Rows.Add(objDr)

                            objDs.Dispose()
                            objDs = New DataSet
                            objDs.Tables.Add(objDt)

                        Case 2  '券売機ＢＢ１読出し処理件数明細
                            objCmd = New SqlCommand("DOCMENP001_S3")
                            objCmd.Connection = objCon
                            objCmd.CommandType = CommandType.StoredProcedure
                            'パラメータ設定
                            With objCmd.Parameters
                                .Add(pfSet_Param("MAC_DVS", SqlDbType.NVarChar, "0")) '0:券売機
                                .Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                            End With
                            'SQL実行
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)

                            strFileclassCD = "0943CL"
                            strReportName = "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]"

                        Case 3  'サンドＢＢ１読出し処理件数明細
                            objCmd = New SqlCommand("DOCMENP001_S3")
                            objCmd.Connection = objCon
                            objCmd.CommandType = CommandType.StoredProcedure
                            'パラメータ設定
                            With objCmd.Parameters
                                .Add(pfSet_Param("MAC_DVS", SqlDbType.NVarChar, "1")) '1:サンド
                                .Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                            End With
                            'SQL実行
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)

                            strFileclassCD = "0944CL"
                            strReportName = "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]"

                        Case 4  '使用中カードＤＢ吸上げ作業明細
                            objCmd = New SqlCommand("DOCMENP001_S4")
                            objCmd.Connection = objCon
                            objCmd.CommandType = CommandType.StoredProcedure
                            'パラメータ設定
                            objCmd.Parameters.Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                            'SQL実行
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)

                            strFileclassCD = "0945CL"
                            strReportName = "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYM & "]"

                        Case 5  'ＴＢＯＸ吸上げ作業処理件数
                            objCmd = New SqlCommand("DOCMENP001_S5")
                            objCmd.Connection = objCon
                            objCmd.CommandType = CommandType.StoredProcedure
                            'パラメータ設定
                            objCmd.Parameters.Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                            'SQL実行
                            objDs = clsDataConnect.pfGet_DataSet(objCmd)

                            strFileclassCD = "0946CL"
                            strReportName = "ＴＢＯＸ吸上げ作業処理件数_[" & strKensyuYM & "]"
                    End Select


                    'エラー
                    If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                        Throw New Exception()
                    End If

                    If mfMakeCsv(strFileclassCD, strReportName, objDs) = False Then
                        Throw New Exception
                    End If
                Next

                mfCreateCSV = True

            Catch ex As Exception
                mfCreateCSV = False
            Finally
            End Try
        Else
            mfCreateCSV = False
        End If

    End Function

    ''' <summary>
    ''' ＣＳＶ作成処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfMakeCsv(ByVal ipstrFileclassCD As String,
                               ByVal ipstrReportName As String,
                               ByVal ds As DataSet) As Boolean

        Dim strServerAddress As String = String.Empty 'サーバアドレス
        Dim strFolderNM As String = String.Empty 'フォルダ名
        Dim strWorkPath As String = String.Empty '出力パス
        Dim strlocalpath As String = "C:\UPLOAD\"
        Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
        Dim opblnResult As Boolean = False

        'CSV出力.
        'If pfCreateCsvFile(strlocalpath, localFileName & ".csv", ds, True) <> 0 Then
        '    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
        '    Return False
        'End If
        Select Case ipstrFileclassCD
            Case "0941CL"
                If pfCreateCsvFileSC(strlocalpath, localFileName & ".csv", ds, True) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                    Return False
                End If
            Case Else
                If pfCreateCsvFile(strlocalpath, localFileName & ".csv", ds, True) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                    Return False
                End If
        End Select

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        ''出力パス生成.
        'strWorkPath = "\\" & strServerAddress & "\" & strFolderNM

        If pfFile_Upload(strFolderNM, ipstrReportName & ".csv", localFileName) = False Then
            Return False
        End If


        Return True

    End Function

    ''' <summary>
    ''' CSVファイル取り込み.
    ''' </summary>
    ''' <param name="ipstrFileclassCD"></param>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="CSVData"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Function ms_GetCSVData(ByVal ipstrFileclassCD As String,
                              ByVal ipstrReportName As String,
                              ByRef CSVData As ArrayList,
                              ByRef Cnt As Integer,
                              ByRef strArrayList As String())

        Dim strServerAddress As String = String.Empty    'サーバアドレス
        Dim strFolderNM As String = String.Empty         'フォルダ名
        Dim strFilePath As String = String.Empty         'ファイルパス
        Dim strDownloadFileName As String = String.Empty '出力パス
        Dim strData As String = String.Empty
        Dim utf As Encoding = Encoding.UTF8
        Dim sjis As Encoding = Encoding.GetEncoding("utf-8")

        ms_GetCSVData = False

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
        End If

        strFilePath = pfFile_Download(strFolderNM, ipstrReportName & ".csv")
        ''パス生成.
        'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & ".csv"

        'ファイル存在チェック
        If Not System.IO.File.Exists(strFilePath) Then
            'psMesBox(Me, "30003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, ipstrReportName)
            Exit Function
        End If
        Dim sr As StreamReader = New StreamReader(strFilePath, System.Text.Encoding.Default)  'ファイルの読み込み
        Dim errMsg As String = "ＣＳＶファイル"                                            'エラーメッセージ

        objStack = New StackFrame

        Try
            Dim strHead As String = sr.ReadLine                                                'ヘッダ行の読み込み

            Dim strHeader As String = strHead.Replace("""", String.Empty)

            strArrayList = strHeader.ToString.Split(",")

            Dim strRep As String = strHead.Replace(",", String.Empty)                          'カンマを削除
            Dim strLin As String = Nothing                                                     'CSVデータ(一行)
            Dim tmpLin As String = Nothing                                                     'CSVデータ(一時保管)
            Dim loopFlg As Boolean = False                                                     'ループ間のフラグ
            Dim loopCnt As Integer = 0                                                         'ループ回数
            Dim fstqt As Boolean = False                                                       '囲み開始フラグ

            Cnt = strHead.Length - strRep.Length                                               'カンマの数を算出

            Dim num As Integer = 0                                                             'カンマ数
            Dim strMoji As String = Nothing                                                '一文字格納

            'CSVファイル内の整形開始
            '一行づつ読み込む
            Do Until sr.EndOfStream = True
                strLin = sr.ReadLine '一行読み込み
                'CSVファイル読み込みカウントアップ
                loopCnt = loopCnt + 1

                'カンマの数を調べる
                '文字数分カウント
                For i As Integer = 0 To strLin.Length - 1
                    '一文字ずつ抽出
                    strMoji = strLin.Substring(i, 1)

                    If strMoji = """" And fstqt = False Then '先頭の囲み
                        fstqt = True
                    ElseIf strMoji = "," And fstqt = True Then 'カンマが""で囲まれている
                        'カンマを別文字に置き換える
                        tmpLin = tmpLin + "‥"
                    ElseIf fstqt = False Then 'カンマが""で囲まれていない
                        '文字を連結
                        tmpLin = tmpLin + strMoji
                        'カンマ数をカウント
                        num = num + 1
                    ElseIf strMoji <> """" And fstqt = True Then '""で囲まれている文字
                        '文字を連結
                        tmpLin = tmpLin + strMoji
                    ElseIf strMoji = """" And fstqt = True Then '囲み終了
                        fstqt = False
                    End If
                Next

                If fstqt = False Then
                    'カンマの数が間違っている場合
                    If num <> Cnt Then
                        Throw New Exception
                    End If

                    CSVData.Add(tmpLin)         '保存
                    tmpLin = Nothing
                    num = 0
                End If

            Loop
            ms_GetCSVData = True
        Catch ex As Exception
            'システムエラー
            psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Throw ex
        Finally
            sr.Close()                 'ファイルクローズ
        End Try
    End Function

    ''' <summary>
    ''' CSVダウンロード.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCsvDownload(ByVal ipstrFileclassCD As String,
                                   ByVal ipstrReportName As String) As Boolean

        Dim strServerAddress As String = String.Empty    'サーバアドレス
        Dim strFolderNM As String = String.Empty         'フォルダ名
        Dim strFilePath As String = String.Empty         'ファイルパス
        Dim strDownloadFileName As String = String.Empty '出力パス
        Dim strData As String = String.Empty
        Dim utf As Encoding = Encoding.UTF8
        Dim sjis As Encoding = Encoding.GetEncoding("utf-8")
        Dim strLocalPath As String

        mfCsvDownload = False

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        'パス生成.
        strData = ipstrReportName & ".csv"

        strLocalPath = pfFile_Download(strFolderNM, strData)
        If strLocalPath = "" Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return True
        End If

        Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strLocalPath & "&filename=" & HttpUtility.UrlEncode(strData), False)

        Return True
    End Function

    ''' <summary>
    ''' DBを参照して各帳票のデータセットを作成
    ''' </summary>
    ''' <param name="prmReport"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetReportDataset(ByVal prmReport As 帳票, Optional ByVal strKensyuYM As String = "") As DataSet
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim strFileName As String = ""
        Dim blnRepFlg As Boolean = False

        If strKensyuYM = "" Then '通常検索
            strKensyuYM = Me.txtKensyuYm.ppText.Replace("/", "")
        Else '券売機・サンドＢＢ１読み出し処理件数帳票出力
            blnRepFlg = True
        End If

        Select Case prmReport
            Case 帳票.サポセン検収書
                objCmd = New SqlCommand("DOCMENP001_S1")
            'Case 帳票.券・サ・BB1
            '    objCmd = New SqlCommand("DOCMENP001_S2")
            '    strFileName = "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYM & "]"
            Case 帳票.券サBB1
                objCmd = New SqlCommand("DOCMENP001_S2")
                strFileName = "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYM & "]"
            Case 帳票.券売機明細
                objCmd = New SqlCommand("DOCMENP001_S3")
                objCmd.Parameters.Add(pfSet_Param("MAC_DVS", SqlDbType.NVarChar, "0")) '0:券売機
                strFileName = "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]"
            Case 帳票.サンド明細
                objCmd = New SqlCommand("DOCMENP001_S3")
                objCmd.Parameters.Add(pfSet_Param("MAC_DVS", SqlDbType.NVarChar, "1")) '1:サンド
                strFileName = "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]"
            Case 帳票.カードDB
                objCmd = New SqlCommand("DOCMENP001_S4")
                strFileName = "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYM & "]"
            Case 帳票.TBOX吸上げ
                objCmd = New SqlCommand("DOCMENP001_S5")
                strFileName = "ＴＢＯＸ吸上げ作業処理件数_[" & strKensyuYM & "]"
        End Select

        'DB接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("KensyuYM", SqlDbType.NVarChar, strKensyuYM))
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                mfGetReportDataset = objDs

            Catch ex As Exception
                Throw ex
                mfGetReportDataset = Nothing
            Finally
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        Else
            mfGetReportDataset = Nothing
        End If

    End Function

    ''' <summary>
    ''' データセットをCSV形式で発行
    ''' </summary>
    ''' <param name="prmReport"></param>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msPutCSVFile(ByVal prmReport As 帳票, ByVal objDs As DataSet)
        Dim strKensyuYM As String = Me.txtKensyuYm.ppText.Replace("/", "")
        Dim strFileName As String = ""
        'If prmReport = 帳票.券・サ・BB1 Then
        '    'データ加工
        '    objDs = mfDataEditToCSV(objDs)
        'End If

        Select Case prmReport
            Case 帳票.サポセン検収書
                strFileName = "サポートセンタ運用の報告書兼検収書_[" & strKensyuYM & "]"
            'Case 帳票.券・サ・BB1
            '    strFileName = "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYM & "]"
            Case 帳票.券サBB1
                strFileName = "券売機・サンドＢＢ１読み出し処理件数_[" & strKensyuYM & "]"
            Case 帳票.券売機明細
                strFileName = "券売機ＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]"
            Case 帳票.サンド明細
                strFileName = "サンドＢＢ１読み出し処理件数明細_[" & strKensyuYM & "]"
            Case 帳票.カードDB
                strFileName = "使用中カードＤＢ吸上げ作業明細_[" & strKensyuYM & "]"
            Case 帳票.TBOX吸上げ
                strFileName = "ＴＢＯＸ吸上げ作業処理件数_[" & strKensyuYM & "]"
        End Select
        If objDs.Tables(0).Rows.Count = 0 Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return
        End If

        '        objDs.Tables(0).Columns(16).ColumnName = " "
        '        objDs.Tables(0).Columns(17).ColumnName = " "
        '        objDs.Tables(0).Columns(18).ColumnName = " "
        '        objDs.Tables(0).Columns(26).ColumnName = " "
        '        objDs.Tables(0).Columns(27).ColumnName = " "
        '        objDs.Tables(0).AcceptChanges()

        'CSVファイルダウンロード
        If pfDLCsvFileSC(strFileName + ".csv", objDs.Tables(0), True, Me) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strFileName)
        End If
    End Sub

    ''' <summary>
    ''' PDF　券・サ・BB1作成
    ''' </summary>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="CsvDate"></param>
    ''' <param name="intCnt"></param>
    ''' <param name="rpt"></param>
    ''' <param name="strArrayList"></param>
    ''' <remarks></remarks>
    Private Sub ms_MakePdf(ByVal ipstrReportName As String, CsvDate As ArrayList, ByVal intCnt As Integer, ByVal rpt As Object, ByVal strArrayList As String())

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strCsv() As String
        Dim dt As New DataTable
        Dim strFNm As String = ipstrReportName & "_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称

        'データテーブルカラム作成(ヘッダ)
        For i As Integer = 0 To strArrayList.Length - 1
            dt.Columns.Add(strArrayList(i))
        Next

        '行数分ループ
        For i As Integer = 0 To CsvDate.Count - 1
            strCsv = Nothing

            'カンマ毎に分割
            strCsv = CsvDate(i).ToString.Split(",")

            dt.Rows.Add(strCsv)
        Next

        Select Case ipstrReportName
            Case "券売機・サンドBB1読み出し処理件数"
                Dim dtrSend As DataRow = mfGetTraderInfo("5")
                Dim dtrRcv As DataRow = mfGetTraderInfo("8")
                dt.Columns.Add("自社名") 'CSVに項目が無い為追加
                dt.Columns.Add("自部署名") 'CSVに項目が無い為追加
                For Each dtr As DataRow In dt.Rows
                    dtr.Item("送付先名") = dtrRcv.Item("M40_SEND_NM1")
                    dtr.Item("送付先営業所") = dtrRcv.Item("M40_SEND_NM1_2")
                    dtr.Item("自社名") = dtrSend.Item("M40_SEND_NM1")
                    dtr.Item("自部署名") = dtrSend.Item("M40_SEND_NM1_2")
                Next
        End Select

        'PDF出力処理.
        psPrintPDF(Me, rpt, dt, strFNm)
    End Sub

    ''' <summary>
    ''' 券売機・サンド・BB1読出し処理件数のCSV用加工
    ''' </summary>
    ''' <param name="objItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfDataEditToCSV(ByRef objItem As DataSet) As DataSet
        Dim intMax As Integer
        Dim objDs As DataSet = New DataSet
        Dim objDt As DataTable = New DataTable
        Dim objDr As DataRow = Nothing
        objDt.Columns.Add("帳票出力日")
        objDt.Columns.Add("検収年")
        objDt.Columns.Add("集計月")

        intMax = 0
        For intIndex = 1 To 12
            If intMax < objItem.Tables(0).Select("集計月 = '" & intIndex.ToString("00") & "' AND 区分 = '券売機'").Count Then
                intMax = objItem.Tables(0).Select("集計月 = '" & intIndex.ToString("00") & "' AND 区分 = '券売機'").Count
            End If
        Next

        If (intMax Mod 8) > 6 Then
            intMax = intMax + (6 + (8 - (intMax Mod 8)))
        ElseIf (intMax Mod 8) < 6 Then
            intMax = intMax + (6 - (intMax Mod 8))
        End If

        For intIndex = 1 To intMax + 2
            objDt.Columns.Add("区分" & intIndex.ToString("00"))
            objDt.Columns.Add("日付" & intIndex.ToString("00"))
            objDt.Columns.Add("東日本件数" & intIndex.ToString("00"))
            objDt.Columns.Add("西日本件数" & intIndex.ToString("00"))
            objDt.Columns.Add("件数計" & intIndex.ToString("00"))
        Next

        Dim intCnt As Integer = 0
        Dim intBrank As Integer = 0
        Dim strDate As String = ""
        For intIndex = 0 To objItem.Tables(0).Rows.Count - 1
            If strDate <> objItem.Tables(0).Rows(intIndex).Item("集計月").ToString Then
                If intIndex > 0 Then
                    objDt.Rows.Add(objDr)
                End If

                objDr = objDt.NewRow
                objDr.Item("帳票出力日") = objItem.Tables(0).Rows(intIndex).Item("帳票出力日").ToString
                objDr.Item("検収年") = objItem.Tables(0).Rows(intIndex).Item("検収年").ToString
                objDr.Item("集計月") = objItem.Tables(0).Rows(intIndex).Item("集計月").ToString
                strDate = objItem.Tables(0).Rows(intIndex).Item("集計月").ToString
                intCnt = 1
            End If

            If objItem.Tables(0).Rows(intIndex).Item("区分").ToString = "サンド" Then
                For intCnt = intCnt To intMax
                    objDr.Item("区分" & intCnt.ToString("00")) = ""
                    objDr.Item("日付" & intCnt.ToString("00")) = ""
                    objDr.Item("東日本件数" & intCnt.ToString("00")) = ""
                    objDr.Item("西日本件数" & intCnt.ToString("00")) = ""
                    objDr.Item("件数計" & intCnt.ToString("00")) = ""
                Next
            End If

            objDr.Item("区分" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("区分").ToString
            If objItem.Tables(0).Rows(intIndex).Item("日付").ToString <> "" Then
                objDr.Item("日付" & intCnt.ToString("00")) = Date.Parse(objItem.Tables(0).Rows(intIndex).Item("日付").ToString).ToString("MM/dd")
            Else
                objDr.Item("日付" & intCnt.ToString("00")) = ""
            End If

            If objItem.Tables(0).Rows(intIndex).Item("件数計").ToString = "0" Then
                objDr.Item("東日本件数" & intCnt.ToString("00")) = ""
                objDr.Item("西日本件数" & intCnt.ToString("00")) = ""
                objDr.Item("件数計" & intCnt.ToString("00")) = ""
            Else
                objDr.Item("東日本件数" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("東日本件数").ToString
                objDr.Item("西日本件数" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("西日本件数").ToString
                objDr.Item("件数計" & intCnt.ToString("00")) = objItem.Tables(0).Rows(intIndex).Item("件数計").ToString
            End If
            intCnt += 1
        Next
        objDt.Rows.Add(objDr)
        objDs.Tables.Add(objDt)
        Return objDs
    End Function

    ''' <summary>
    ''' 券売機・サンド・BB1読出し処理件数
    ''' 取得したデータセットからCSV形式のデータセットに編集
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfConvertCSVStyle(ByVal ds As DataSet) As DataSet
        Dim objDs As DataSet = New DataSet
        Dim objDr As DataRow = Nothing
        Dim strKensyuYM As String = txtKensyuYm.ppText.Replace("/", "").Trim
        Dim intIndex As Integer
        Dim intMax As Integer
        Dim objDt As DataTable
        objDt = New DataTable
        objDt.Columns.Add("帳票出力日")
        objDt.Columns.Add("検収年")
        objDt.Columns.Add("集計月")

        intMax = 0
        For intIndex = 1 To 12
            If intMax < ds.Tables(0).Select("集計月 = '" & intIndex.ToString("00") & "' AND 区分 = '券売機'").Count Then
                intMax = ds.Tables(0).Select("集計月 = '" & intIndex.ToString("00") & "' AND 区分 = '券売機'").Count
            End If
        Next

        If (intMax Mod 8) > 6 Then
            intMax = intMax + (6 + (8 - (intMax Mod 8)))
        ElseIf (intMax Mod 8) < 6 Then
            intMax = intMax + (6 - (intMax Mod 8))
        End If

        For intIndex = 1 To intMax + 2
            objDt.Columns.Add("区分" & intIndex.ToString("00"))
            objDt.Columns.Add("日付" & intIndex.ToString("00"))
            objDt.Columns.Add("東日本件数" & intIndex.ToString("00"))
            objDt.Columns.Add("西日本件数" & intIndex.ToString("00"))
            objDt.Columns.Add("件数計" & intIndex.ToString("00"))
        Next

        Dim intCnt As Integer = 0
        Dim intBrank As Integer = 0
        Dim strDate As String = ""

        For intIndex = 0 To ds.Tables(0).Rows.Count - 1
            If strDate <> ds.Tables(0).Rows(intIndex).Item("集計月").ToString Then
                If intIndex > 0 Then
                    objDt.Rows.Add(objDr)
                End If

                objDr = objDt.NewRow
                objDr.Item("帳票出力日") = ds.Tables(0).Rows(intIndex).Item("帳票出力日").ToString
                objDr.Item("検収年") = ds.Tables(0).Rows(intIndex).Item("検収年").ToString
                objDr.Item("集計月") = ds.Tables(0).Rows(intIndex).Item("集計月").ToString
                strDate = ds.Tables(0).Rows(intIndex).Item("集計月").ToString
                intCnt = 1
            End If

            If ds.Tables(0).Rows(intIndex).Item("区分").ToString = "サンド" Then
                For intCnt = intCnt To intMax
                    objDr.Item("区分" & intCnt.ToString("00")) = ""
                    objDr.Item("日付" & intCnt.ToString("00")) = ""
                    objDr.Item("東日本件数" & intCnt.ToString("00")) = ""
                    objDr.Item("西日本件数" & intCnt.ToString("00")) = ""
                    objDr.Item("件数計" & intCnt.ToString("00")) = ""
                Next
            End If

            objDr.Item("区分" & intCnt.ToString("00")) = ds.Tables(0).Rows(intIndex).Item("区分").ToString
            If ds.Tables(0).Rows(intIndex).Item("日付").ToString <> "" Then
                objDr.Item("日付" & intCnt.ToString("00")) = Date.Parse(ds.Tables(0).Rows(intIndex).Item("日付").ToString).ToString("MM/dd")
            Else
                objDr.Item("日付" & intCnt.ToString("00")) = ""
            End If

            If ds.Tables(0).Rows(intIndex).Item("件数計").ToString = "0" Then
                objDr.Item("東日本件数" & intCnt.ToString("00")) = ""
                objDr.Item("西日本件数" & intCnt.ToString("00")) = ""
                objDr.Item("件数計" & intCnt.ToString("00")) = ""
            Else
                objDr.Item("東日本件数" & intCnt.ToString("00")) = ds.Tables(0).Rows(intIndex).Item("東日本件数").ToString
                objDr.Item("西日本件数" & intCnt.ToString("00")) = ds.Tables(0).Rows(intIndex).Item("西日本件数").ToString
                objDr.Item("件数計" & intCnt.ToString("00")) = ds.Tables(0).Rows(intIndex).Item("件数計").ToString
            End If
            intCnt += 1
        Next

        objDt.Rows.Add(objDr)

        objDs.Dispose()
        objDs = New DataSet
        objDs.Tables.Add(objDt)
        Return objDs

    End Function

    Private Function mfEditDatasetToCSV(ByVal ds As DataSet, ByRef strRtnHeaderList As String()) As ArrayList

        Dim rtnList As ArrayList = New ArrayList

        Dim fileFullPath As String = String.Empty   'CSVファイル名（フルパス）
        Dim objSw As StreamWriter = Nothing         'StreamWriterクラス
        Dim objBuff As New StringBuilder            'StringBuilderクラス
        Dim zz As Integer = 0                       'カウンタ
        objStackFrame = New StackFrame
        ReDim strRtnHeaderList(ds.Tables(0).Columns.Count - 1)
        Try
            'ヘッダー出力
            '１列目～最終列の１つ前
            For zz = 0 To (ds.Tables(0).Columns.Count) - 2
                objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
                objBuff.Append(P_CSV_DELIMITER)
                strRtnHeaderList(zz) = objBuff.ToString
                objBuff.Clear()
            Next
            '最終列
            objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
            strRtnHeaderList(zz) = objBuff.ToString
            objBuff.Clear()
            'データ出力
            For Each dr As DataRow In ds.Tables(0).Rows
                '１列目～最終列の１つ前
                For zz = 0 To (ds.Tables(0).Columns.Count) - 2
                    objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                rtnList.Add(objBuff.ToString)
                objBuff.Clear()
            Next

            Return rtnList

        Catch ex As Exception
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            Return Nothing
        End Try
    End Function

    Private Function mfChangeDSToCSV(ByVal strArrayList As String(), CSVDate As ArrayList) As DataSet
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strCsv() As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        'データテーブルカラム作成(ヘッダ)
        For i As Integer = 0 To strArrayList.Length - 1
            dt.Columns.Add(strArrayList(i))
        Next

        '行数分ループ
        For i As Integer = 0 To CSVDate.Count - 1
            strCsv = Nothing

            'カンマ毎に分割
            strCsv = CSVDate(i).ToString.Split(",")

            dt.Rows.Add(strCsv)
        Next
        ds.Tables.Add(dt)
        Return ds

    End Function


#End Region

#Region "■ PDF作成"

    ''' <summary>
    ''' PDF作成
    ''' </summary>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="rpt"></param>
    ''' <param name="dset"></param>
    ''' <remarks></remarks>
    Private Sub ms_MakePdf(ByVal ipstrReportName As String, ByVal rpt As Object, ByVal dset As DataSet)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim dt As New DataTable
        Dim strFNm As String = ipstrReportName & "_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称

        'データテーブルカラム
        dt = dset.Tables(0).Copy
        If ipstrReportName = "使用中カードＤＢ吸上げ作業明細" Then
            Dim dtrTrader As DataRow = mfGetTraderInfo("5")
            dt.Columns.Add("自社名")
            dt.Columns.Add("自部署名")
            'dt.Columns("自社名").DefaultValue = dtrTrader.Item("M40_SEND_NM1")
            'dt.Columns("自部署名").DefaultValue = dtrTrader.Item("M40_SEND_NM1_2")
            For Each dtr As DataRow In dt.Rows
                dtr.Item("自社名") = dtrTrader.Item("M40_SEND_NM1")
                dtr.Item("自部署名") = dtrTrader.Item("M40_SEND_NM1_2")
            Next
        End If

        Select Case ipstrReportName
            Case "サポートセンタ運用の報告書兼検収書"
                dt.Columns.Add("発行元営業所名") 'CSVに項目が無い為追加
                Dim dtrSend As DataRow = mfGetTraderInfo("9")
                Dim dtrRcv As DataRow = mfGetTraderInfo("8")
                For Each dtr As DataRow In dt.Rows
                    dtr.Item("宛先会社名") = dtrRcv.Item("M40_SEND_NM1")
                    dtr.Item("宛先営業所名") = dtrRcv.Item("M40_SEND_NM1_2")
                    dtr.Item("発行元名") = dtrSend.Item("M40_SEND_NM1")
                    dtr.Item("発行元営業所名") = dtrSend.Item("M40_SEND_NM1_2")
                    dtr.Item("発行元住所") = dtrSend.Item("M40_ADDR1")
                    dtr.Item("発行元担当者") = dtrSend.Item("M40_SEND_NM1_3")
                    dtr.Item("発行元TEL") = dtrSend.Item("M40_TELNO1")
                    dtr.Item("発行元FAX") = dtrSend.Item("M40_FAXNO1")
                    dtr.Item("宛先営業所名フッター") = dtrRcv.Item("M40_SEND_NM1_3")
                Next

            Case "券売機ＢＢ１読み出し処理件数明細", "サンドＢＢ１読み出し処理件数明細", "券売機・サンドBB1読み出し処理件数"
                Dim dtrSend As DataRow = mfGetTraderInfo("5")
                Dim dtrRcv As DataRow = mfGetTraderInfo("8")
                dt.Columns.Add("自社名") 'CSVに項目が無い為追加
                dt.Columns.Add("自部署名") 'CSVに項目が無い為追加
                For Each dtr As DataRow In dt.Rows
                    dtr.Item("送付先名") = dtrRcv.Item("M40_SEND_NM1")
                    dtr.Item("送付先営業所") = dtrRcv.Item("M40_SEND_NM1_2")
                    dtr.Item("自社名") = dtrSend.Item("M40_SEND_NM1")
                    dtr.Item("自部署名") = dtrSend.Item("M40_SEND_NM1_2")
                Next
        End Select

        'PDF出力処理.
        psPrintPDF(Me, rpt, dt, strFNm)
    End Sub

    'DOCMENP001-005
    ''' <summary>
    ''' 自社名取得
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTraderInfo(ByVal strKey As String) As DataRow
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            mfGetTraderInfo = Nothing
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL059", objCn)
                objCmd.Parameters.Add(pfSet_Param("PRC_DVS", SqlDbType.NVarChar, strKey))
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count > 0 Then
                    mfGetTraderInfo = objDs.Tables(0).Rows(0)
                Else
                    mfGetTraderInfo = Nothing
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "自社名の取得")
                mfGetTraderInfo = Nothing
                'ログ出力"
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function
    'DOCMENP001-005
#End Region

    ''' <summary>
    ''' 人件費の表示状態チェンジ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDispSetJinkenhi()
        Dim dr As DataRow = mfGetShimeJotai()
        If dr Is Nothing Then
            mpPanelVisible = 人件費表示状態.テキストボックス
        Else
            If dr.Item("切替区分") = 1 Then
                If txtKensyuYm.ppText.Replace("/", "") >= "201610" Or txtKensyuYm.ppText.Replace("/", "") = "" Then
                    mpPanelVisible = 人件費表示状態.テキストボックス
                Else
                    mpPanelVisible = 人件費表示状態.ラベル２
                End If
                '                mpPanelVisible = 人件費表示状態.テキストボックス
            Else
                mpPanelVisible = 人件費表示状態.ラベル１
            End If
        End If
    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
