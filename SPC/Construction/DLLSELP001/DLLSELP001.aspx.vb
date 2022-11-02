'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　構成配信/結果参照
'*　ＰＧＭＩＤ：　DLLSELP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.10　：　高松
'********************************************************************************************************************************

#Region "インポート定義"

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax

#End Region

Public Class DLLSELP001

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
    Const M_DISP_ID = P_FUN_DLL & P_SCR_SEL & P_PAGE & "001"    'プログラムID
    Const M_BBKOUSEI = "0157FC"
    Const M_TENNAIKOUSEI = "店内装置構成票反映日時"

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
    Dim DBFTP As New DBFTP.ClsDBFTP_Main
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得(構成情報明細)の設定
        pfSet_GridView(Me.grdList_Kousei, M_DISP_ID + "_1")

        'グリッドXML定義ファイル取得(検索結果明細)の設定
        pfSet_GridView(Me.grvList, M_DISP_ID + "_2")

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンクリックイベント設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btn_Click       '検索
        AddHandler Master.ppRigthButton2.Click, AddressOf btn_Click       '検索クリア

        'TBOXIDを入力した場合
        AddHandler Me.txtTboxID.ppTextBox.TextChanged, AddressOf ctl_Change
        Me.txtTboxID.ppTextBox.AutoPostBack = True
        'ドロップダウンリストの項目を変更した場合
        AddHandler Me.ddlList.SelectedIndexChanged, AddressOf ctl_Change
        Me.ddlList.AutoPostBack = True

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If Not IsPostBack Then  '初回表示

            Try

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'セッション項目のチェック
                If Session(P_SESSION_BCLIST) Is Nothing _
                    Or Session(P_SESSION_USERID) Is Nothing _
                    Or Session(P_SESSION_IP) Is Nothing Then

                    Throw New Exception

                End If

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面初期化処理
                msClearScreen()

                'フォーカスを当てる
                Me.txtTboxID.ppTextBox.Focus()

                '検索結果明細の表示
                ms_GetSerchRisult(DateTime.Now.ToString("yyyyMMdd") _
                                , DateTime.Now.ToString("yyyyMMdd"))

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                psClose_Window(Me)
                Exit Sub


            End Try

        End If
    End Sub

#End Region

    '---------------------------
    '2014/04/23 武 ここから
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
    '2014/04/23 武 ここまで
    '---------------------------

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click _
                                                                                  , btnHaishin.Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            Select Case sender.text

                Case "クリア"

                    'コントロールの初期化
                    Me.txtTboxID.ppText = Nothing
                    Me.ddlList.Items.Clear()
                    Me.grdList_Kousei.DataSource = New DataTable
                    Me.grdList_Kousei.DataBind()
                    Me.btnHaishin.Enabled = False

                Case "検索条件クリア"

                    Me.txtTboxIDSerch.ppText = Nothing
                    Me.txtHaishinFromTo.ppFromText = Nothing
                    Me.txtHaishinFromTo.ppToText = Nothing
                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()

                Case "検索"

                    '検証チェック結果
                    If (Page.IsValid) Then

                        Try

                            '検索結果明細の表示
                            ms_GetSerchRisult(Me.txtHaishinFromTo.ppFromText.Replace("/", "") _
                                             , Me.txtHaishinFromTo.ppToText.Replace("/", ""))

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
                            '終了
                            Exit Sub

                        End Try

                    End If


                Case "配信"

                    'ドロップダウンリストの選択がない場合
                    If ddlList.SelectedIndex = 0 Then

                        valddl.Text = "未入力エラー"
                        valddl.ErrorMessage = pfGet_Mes("5002", ClsComVer.E_Mタイプ.エラー)
                        valddl.Visible = True
                        valddl.Enabled = True
                        valddl.IsValid = False

                    End If

                    '検証チェック結果
                    If (Page.IsValid) Then

                        Try

                            Dim dt As DataTable = pfParse_DataTable(grdList_Kousei)

                            '2重配信をチェック
                            If Not ms_GetHaishinChack() Then
                                '処理終了
                                Exit Sub
                            End If

                            '構成配信履歴の作成/電文要求データの作成
                            ms_InsHaishin()

                            '検索条件コントロールを初期化
                            Me.txtTboxIDSerch.ppText = Nothing
                            Me.txtHaishinFromTo.ppFromText = Nothing
                            Me.txtHaishinFromTo.ppToText = Nothing

                            '検索結果明細の表示
                            ms_GetSerchRisult(DateTime.Now.ToString("yyyyMMdd"), _
                                              DateTime.Now.ToString("yyyyMMdd"))

                            psMesBox(Me, "20006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "配信依頼が終了しました")

                            '表示をそのままとする
                            grdList_Kousei.DataSource = dt
                            grdList_Kousei.DataBind()

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
                            '終了
                            Exit Sub

                        End Try

                    End If

            End Select

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
    ''' コントロール内容変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ctl_Change(sender As System.Object, e As System.EventArgs)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Select Case sender.ID

            Case "txtTextBox"    'TBOXIDを修正

                Try

                    If Not Me.txtTboxID.ppText = String.Empty Then

                        'TBOXIDからファイル名を取得
                        ms_GetFileName()

                    Else

                        'グリッドビュー・ドロップダウンリストの初期化
                        Me.ddlList.Items.Clear()
                        Me.btnHaishin.Enabled = False

                    End If

                    'グリッドの初期化
                    Me.grdList_Kousei.DataSource = New DataTable
                    Me.grdList_Kousei.DataBind()

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
                    '終了
                    Exit Sub

                End Try

            Case "ddlList"      'ドロップダウンリストを選択

                Dim intSelectIndex As Integer = Me.ddlList.SelectedIndex '選択行
                Dim strSelectValue As String = Nothing                   '保存先格納用
                Dim strFilePath As String = Nothing
                Dim strdirPath As String = Nothing
                Dim splPath As String() = Nothing
                Dim strdirPath2 As String = Nothing
                Dim opblnResult As Boolean = False
                Dim dirPath As DirectoryInfo
                Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString

                Try

                    '先頭行の選択以外
                    If Not intSelectIndex = 0 Then

                        '保管場所からパスを取得
                        strSelectValue = ddlList.SelectedValue
                        strFilePath = ms_GetFilePath()

                        If strFilePath Is Nothing Then

                            '処理終了
                            Throw New Exception

                        End If

                        '対象ディレクトリ内のフォルダ確認用のパス
                        strdirPath = strFilePath.Replace("/", "\")
                        strdirPath = strFilePath + "\"
                        splPath = strFilePath.Split("\")
                        For spl = 3 To splPath.Count - 1
                            strdirPath2 &= splPath(spl) & "\"
                        Next

                        dirPath = New DirectoryInfo(strdirPath2 + Me.txtTboxID.ppText.ToString + "\")
                        '選択されたファイルの存在確認
                        'If Not File.Exists(strSelectValue) Then

                        '    'ファイルが存在しない
                        '    psMesBox(Me, "30003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "選択")
                        '    Throw New Exception

                        'End If

                        'ファイル存在確認
                        If DBFTP.pfFtpDir_Exists(dirPath.ToString, opblnResult, "1") Then
                            If DBFTP.pfFtpFile_Exists(dirPath.ToString, strSelectValue, opblnResult) = False Then
                                'ファイルが存在しない
                                psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "選択")
                                Throw New Exception
                            Else
                                'ローカルにダウンロードを開始する
                                DBFTP.pfFtpFile_Copy("GET", strdirPath2 + Me.txtTboxID.ppText.ToString + "\", strSelectValue, opblnResult, "DOWNLOAD" & "\" & localFileName)

                                'ダウンロードファイル存在確認(保存先)
                                If Not File.Exists("C:\DOWNLOAD\" & localFileName) Then
                                    'ファイルが存在しない
                                    psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strSelectValue)
                                    Exit Sub
                                End If
                            End If
                        Else

                            psMesBox(Me, "50003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "BB構成ファイル")

                        End If

                        'ファイルの展開/内容確認/グリッドの作成・表示
                        'me_getFileData(strSelectValue)
                        me_getFileData("C:\DOWNLOAD\" & localFileName)

                        '配信ボタンの制御
                        Me.btnHaishin.Enabled = True

                    Else

                        'グリッドビューの初期化
                        Me.grdList_Kousei.DataSource = New DataTable
                        Me.grdList_Kousei.DataBind()

                        '配信ボタンの制御
                        Me.btnHaishin.Enabled = False

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
                    '終了
                    Exit Sub

                Finally

                    'ポストバック前の選択に設定する
                    Me.ddlList.SelectedIndex = intSelectIndex
                    'ローカルに一時的に作成したファイルを削除
                    'ファイルの存在を確認
                    If System.IO.File.Exists("C:\DOWNLOAD\" & localFileName) Then
                        System.IO.File.Delete("C:\DOWNLOAD\" & localFileName)
                    End If

                End Try

        End Select

    End Sub

    ''' <summary>
    ''' 画面項目初期化
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msClearScreen()

        Dim strSeskey_out() As String = Session(P_KEY)
        Dim strSesTerms_out As String = Session(P_SESSION_TERMS)
        Dim strViewst_in As String = Nothing

        '入力値の初期化
        txtTboxID.ppText = Nothing
        Me.ddlList.Items.Clear()

        If Not strSesTerms_out Is Nothing Then

            Select Case strSesTerms_out

                Case ClsComVer.E_遷移条件.参照

                    '入力値の初期化
                    Me.btnClear.Enabled = False
                    txtTboxID.ppEnabled = False
                    Me.ddlList.Enabled = False

                Case ClsComVer.E_遷移条件.更新

                    If strSeskey_out(0) Is Nothing Then

                        '入力値の初期化
                        txtTboxID.ppText = Nothing
                        Me.ddlList.Items.Clear()

                    Else

                        '入力値の初期化
                        txtTboxID.ppText = strSeskey_out(0)
                        'TBOXIDからファイル名を取得
                        ms_GetFileName()

                    End If

            End Select

        End If

        '配信項目の初期化
        Me.txtTboxIDSerch.ppText = Nothing
        Me.txtHaishinFromTo.ppFromText = Nothing
        Me.txtHaishinFromTo.ppToText = Nothing

        'マスタページの初期化
        Me.Master.ppCount = "0"

        'グリッドビューの初期化
        Me.grdList_Kousei.DataSource = New DataTable
        Me.grdList_Kousei.DataBind()
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        '検証チェックの設定
        Me.txtTboxID.ppValidationGroup = "1"
        Me.valSummary.ValidationGroup = "1"

        'ボタン項目の設定
        Me.btnHaishin.CausesValidation = True
        Me.btnHaishin.ValidationGroup = "1"
        Master.ppRigthButton1.CausesValidation = True
        Master.ppRigthButton2.CausesValidation = False

        '配信ボタンの制御
        Me.btnHaishin.Enabled = False

        'ボタン押下時のメッセージ
        Me.btnHaishin.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "構成配信データ")

        'フォーカス設定
        Me.txtTboxID.ppTextBox.Focus()

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' ファイル名取得/ドロップダウンリストに設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetFileName()

        Dim ds As New DataSet
        Dim dtsFileName As New DataTable
        Dim dirPath As DirectoryInfo
        Dim strFilePath As String = Nothing
        Dim opblnresult As Boolean = False      '取得結果
        Dim splPath As String() = Nothing
        Dim strdirPath As String = Nothing
        Dim strdirPath2 As String = Nothing
        Dim aryList As New ArrayList
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            dtsFileName.Columns.Add("TextField")
            dtsFileName.Columns.Add("ValueField")

            '保存場所から情報を取得
            strFilePath = ms_GetFilePath()

            If strFilePath Is Nothing Then

                '処理終了
                Throw New Exception

            End If

            '対象ディレクトリ内のフォルダ確認用のパス
            strdirPath = strFilePath.Replace("/", "\")
            strdirPath = strFilePath + "\"
            splPath = strFilePath.Split("\")
            For spl = 3 To splPath.Count - 1
                strdirPath2 &= splPath(spl) & "\"
            Next

            dirPath = New DirectoryInfo(strdirPath2 + Me.txtTboxID.ppText.ToString + "\")

            'If System.IO.Directory.Exists(strFilePath) Then
            If DBFTP.pfFtpDir_Exists(dirPath.ToString, opblnresult, "1") Then

                ' 対象ディレクトリ内のファイルを列挙する
                'For Each stFilePath As String In System.IO.Directory.GetFiles(strFilePath + "\" + Me.txtTboxID.ppText.ToString)
                DBFTP.fFtpGetList(dirPath.ToString, aryList)
                'For intCnt As Integer = 0 To aryList.Count - 1

                Dim Row As DataRow = dtsFileName.NewRow()          'データテーブルの行定義

                'ドロップダウンリストの表示設定
                Row("TextField") = Path.GetFileName(aryList(aryList.Count - 1))
                Row("ValueField") = aryList(aryList.Count - 1)

                dtsFileName.Rows.Add(Row)

                'Next stFilePath
                'Next

            Else

                psMesBox(Me, "50003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "構成配信ファイル")

            End If


            'ファイルが存在するか確認する
            If Not dtsFileName.Rows.Count = 0 Then

                'データセットに登録
                ds.Tables.Add(dtsFileName)

                'ドロップダウンリストに設定
                Me.ddlList.DataSource = ds.Tables(0)
                Me.ddlList.DataTextField = ds.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlList.DataValueField = ds.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlList.DataBind()

                '先頭行の設定と選択
                Me.ddlList.Items.Insert(0, " ")
                Me.ddlList.SelectedIndex = 0

            Else

                '項目の初期化
                Me.ddlList.Items.Clear()

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
            Throw ex

        End Try

    End Sub

    ''' <summary>
    ''' 保存先ファイルから情報を取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetFilePath() As String

        Dim strPath As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
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

                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)

                'BB構成ファイル
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, M_BBKOUSEI))                            'ファイル種別
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString
                Select Case strOKNG
                    Case "0"

                        'パスが取得できない
                        Throw New Exception

                    Case Else

                        strPath = "\\" + dstOrders.Tables(0).Rows(0).Item("パス").ToString

                End Select

                Return strPath

            Catch ex As SqlException

                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保存先パス")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return Nothing

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保存先パス")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return Nothing

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"
            Return Nothing

        End If

    End Function

    ''' <summary>
    ''' 構成ファイルの読み込み/グリッドビューの設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub me_getFileData(ByVal FullPah As String)

        Dim ds As New DataSet                    'データセット
        Dim dt As New DataTable                  'データテーブル
        Dim dtRow As DataRow                     'データ行
        Dim viewSt_Tboxid(20 - 1) As String      'ビューステート(先頭20バイト分保存)
        Dim viewSt_Futago(12 - 1) As String      'ビューステート(双子店情報)

        Dim intCnt As Integer = 0                'FROM - TO ループ用

        'ファイルサイズによるループの変更
        Dim intLoopNum() As Integer = {1500, _
                                       1998, _
                                       2998, _
                                       4500}    'ループ数
        Dim intNum As Integer = 0                '対象ループ数選択用

        'ファイル読み込み処理用変数
        Dim fs As New FileStream( _
                      FullPah, FileMode.Open, FileAccess.Read)  'ファイル読み込み設定
        Dim fileSize As Integer = CInt(fs.Length)               'ファイルのサイズ
        Dim buf_tboxid(20 - 1) As Byte                          'データ格納用配列(先頭20バイト)
        Dim buf_kobetsu(12 - 1) As Byte                         'データ格納用配列(個別情報)
        Dim buf_futago(1 - 1) As Byte                           'データ格納用配列(双子店有無フラグ)
        Dim buf_tenpo(1 - 1) As Byte                            'データ格納用配列(店舗情報有無フラグ)
        Dim buf_tenpoJyoho(137 - 1) As Byte                     'データ格納用配列(双子店情報)
        Dim readSize As Integer                                 'Readメソッドで読み込んだバイト数
        Dim remain As Integer = fileSize                        '読み込むべき残りのバイト数

        Dim strBcd(48 - 1) As String                            'BCD変換用
        Dim bytChr(0) As Byte                                   'CHR変換
        Dim strHex As String                                    'HEX(0.5バイト)用変数
        Dim strBBsum As String = Nothing                        'BB総代数
        Dim intBBsum As Integer = 0                             'BB総代数

        Dim strTemp As String = Nothing                         '電文データ作成用

        'グリッドビュー表示用変数
        Dim strJBNum As String = Nothing                        'JB番号
        Dim strUnyokiban As String = Nothing                    '運用機番
        Dim strKaisenNum As String = Nothing                    '回線番号
        Dim strBBsyubetsu As String = Nothing                   'BB種別
        Dim strBBkisyu As String = Nothing                      'BB機種種別
        Dim strYobi1 As String = Nothing                        '予備1
        Dim strShimaban As String = Nothing                     '島番号
        Dim strTenpo As String = Nothing                        '店舗
        Dim strYobi2 As String = Nothing                        '予備1

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            'ファイルサイズごとの読み込みループ数の判断
            Select Case fileSize
                Case 20480

                    intNum = 0

                Case 40960

                    intNum = 1

                Case 39936

                    intNum = 2

                Case 58368

                    intNum = 3

                Case Else
                    '処理終了
                    psMesBox(Me, "30004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
                    '配信ボタンの非活性
                    Me.btnHaishin.Enabled = False
                    Throw New Exception

            End Select

            dt.Columns.Add("JB番号")
            dt.Columns.Add("運用機番")
            dt.Columns.Add("回線番号")
            dt.Columns.Add("BB種別")
            dt.Columns.Add("BB機種種別")
            dt.Columns.Add("島番号")
            dt.Columns.Add("店舗")
            dt.Columns.Add("予備1")
            dt.Columns.Add("予備2")

            'TBOXID,TBOXハード種別,入力元情報
            '店内装置構成票反映日時,BB総代数
            '回線0接続台数,回線1接続台数の情報を取得(20バイト)
            readSize = fs.Read(buf_tboxid, 0, Math.Min(20, remain))

            'TBOXID(BCD)
            strBcd(0) = Convert.ToString(buf_tboxid(0), 2).ToString.PadLeft(8, "0")
            strBcd(1) = Convert.ToString(buf_tboxid(1), 2).ToString.PadLeft(8, "0")
            strBcd(2) = Convert.ToString(buf_tboxid(2), 2).ToString.PadLeft(8, "0")
            strBcd(3) = Convert.ToString(buf_tboxid(3), 2).ToString.PadLeft(8, "0")
            strBcd(4) = Convert.ToString(buf_tboxid(4), 2).ToString.PadLeft(8, "0")
            'BCDに変換
            For j As Integer = 0 To 5 - 1
                strTemp = strTemp + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                strTemp = strTemp + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
            Next

            If Not strTemp.Substring(2, 8) = Me.txtTboxID.ppText Then

                '配信ボタンの非活性
                Me.btnHaishin.Enabled = False
                Throw New Exception

            End If

            '店内装置構成票反映日時(BCD)
            strTemp = Nothing
            strBcd(0) = Convert.ToString(buf_tboxid(7), 2).ToString.PadLeft(8, "0")
            strBcd(1) = Convert.ToString(buf_tboxid(8), 2).ToString.PadLeft(8, "0")
            strBcd(2) = Convert.ToString(buf_tboxid(9), 2).ToString.PadLeft(8, "0")
            strBcd(3) = Convert.ToString(buf_tboxid(10), 2).ToString.PadLeft(8, "0")
            strBcd(4) = Convert.ToString(buf_tboxid(11), 2).ToString.PadLeft(8, "0")
            strBcd(5) = Convert.ToString(buf_tboxid(12), 2).ToString.PadLeft(8, "0")
            strBcd(6) = Convert.ToString(buf_tboxid(13), 2).ToString.PadLeft(8, "0")
            'BCDに変換
            For j As Integer = 0 To 7 - 1
                strTemp = strTemp + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                strTemp = strTemp + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
                Select Case j
                    Case 1
                        strTemp = strTemp + "/"
                    Case 2
                        strTemp = strTemp + "/"
                    Case 3
                        strTemp = strTemp + " "
                    Case 4
                        strTemp = strTemp + ":"
                    Case 5
                        strTemp = strTemp + ":"
                End Select
            Next

            Me.ViewState(M_TENNAIKOUSEI) = strTemp

            'BB総台数を取得
            strBBsum = Convert.ToString(buf_tboxid(14), 16).PadLeft(2, "0")
            strBBsum = Convert.ToString(buf_tboxid(15), 16).PadLeft(2, "0") + strBBsum
            intBBsum = Convert.ToInt32(strBBsum, 16)

            '読み込み位置の更新
            remain -= readSize

            '個別情報の取得
            For i As Integer = 0 To intLoopNum(intNum) - 1

                'JB番号,回線番号,店舗番号,運用機番,BB種別コード
                'BB機種種別,予備,島番号,予備の情報を取得(12バイトずつ)
                readSize = fs.Read(buf_kobetsu, 0, Math.Min(12, remain))

                'BB総台数分ループ
                Select Case i
                    Case Is <= intBBsum - 1

                        dtRow = dt.NewRow         'データ行

                        'JB番号(BCD)
                        strBcd(0) = Convert.ToString(buf_kobetsu(0), 2).ToString.PadLeft(8, "0")
                        strBcd(1) = Convert.ToString(buf_kobetsu(1), 2).ToString.PadLeft(8, "0")
                        'BCDに変換
                        For j As Integer = 0 To 2 - 1
                            strJBNum = strJBNum + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                            strJBNum = strJBNum + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
                        Next

                        'JB番号が "0000" の場合表示を行わない
                        If strJBNum = "0000" Then
                            '初期化
                            strJBNum = Nothing
                            Exit Select
                        End If

                        '回線番号(CHR)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(2), 16), 16)
                        If bytChr(0) <> 0 Then

                            strKaisenNum = Encoding.UTF8.GetString(bytChr)

                        End If


                        '店舗番号(HEX)
                        strTenpo = Convert.ToString(buf_kobetsu(3), 16).PadLeft(2, "0")

                        '運用機番(BCD)
                        strBcd(0) = Convert.ToString(buf_kobetsu(4), 2).ToString.PadLeft(8, "0")
                        strBcd(1) = Convert.ToString(buf_kobetsu(5), 2).ToString.PadLeft(8, "0")
                        'BCDに変換
                        For j As Integer = 0 To 2 - 1
                            strUnyokiban = strUnyokiban + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                            strUnyokiban = strUnyokiban + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
                        Next

                        'BB種別コード(CHR)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(6), 16), 16)
                        If bytChr(0) <> 0 Then

                            strBBsyubetsu = Encoding.UTF8.GetString(bytChr)

                        End If

                        'HEX(0.5バイト用)
                        strHex = Convert.ToString(buf_kobetsu(3), 16).ToString.PadLeft(2, "0")

                        'BB機器種別(HEX)
                        strBBkisyu = strHex.Substring(0, 1)

                        '予備1(HEX)
                        strYobi1 = strHex.Substring(1, 1)

                        '島番号(BCD)
                        strBcd(0) = Convert.ToString(buf_kobetsu(8), 2).ToString.PadLeft(8, "0")
                        strBcd(1) = Convert.ToString(buf_kobetsu(9), 2).ToString.PadLeft(8, "0")
                        'BCDに変換
                        For j As Integer = 0 To 2 - 1
                            strShimaban = strShimaban + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                            strShimaban = strShimaban + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
                        Next

                        '予備2(CHR)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(10), 16), 16)
                        strYobi2 = Encoding.UTF8.GetString(bytChr)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(11), 16), 16)
                        strYobi2 = strYobi2 + Encoding.UTF8.GetString(bytChr)

                        'データ行に設定
                        dtRow("JB番号") = strJBNum
                        dtRow("運用機番") = strUnyokiban
                        dtRow("回線番号") = strKaisenNum
                        dtRow("BB種別") = strBBsyubetsu
                        dtRow("BB機種種別") = strBBkisyu
                        dtRow("島番号") = strShimaban
                        dtRow("店舗") = strTenpo
                        dtRow("予備1") = strYobi1
                        dtRow("予備2") = strYobi2

                        'データテーブルにセット
                        dt.Rows.Add(dtRow)

                        '変数の初期化k
                        strJBNum = Nothing
                        strUnyokiban = Nothing
                        strKaisenNum = Nothing
                        strBBsyubetsu = Nothing
                        strYobi1 = Nothing
                        strBBkisyu = Nothing
                        strShimaban = Nothing
                        strTenpo = Nothing
                        strYobi2 = Nothing

                End Select

                '読み込み位置の更新
                remain -= readSize

            Next

            'グリッドビューの表示
            Me.grdList_Kousei.DataSource = dt
            Me.grdList_Kousei.DataBind()

            '配信ボタンの活性
            Me.btnHaishin.Enabled = True

        Catch ex As Exception

            '処理終了
            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
        Finally
            'ファイルの開放
            fs.Close()
        End Try


    End Sub

    ''' <summary>
    ''' 検索結果明細表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ms_GetSerchRisult(ByVal dateFrom As String, ByVal dateTo As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing

        Dim tboxid As String
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

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                If dateFrom = String.Empty Then

                    dateFrom = "1"

                End If

                If dateTo = String.Empty Then

                    dateTo = "1"

                End If

                If Me.txtTboxIDSerch.ppText = String.Empty Then

                    tboxid = "1"

                Else

                    tboxid = Me.txtTboxIDSerch.ppText

                End If

                '一覧情報
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_delv_dt_f", SqlDbType.NVarChar, dateFrom))                         'FROM
                    .Add(pfSet_Param("prm_delv_dt_t", SqlDbType.NVarChar, dateTo))                           'TO
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, tboxid))                             'TBOXID
                    .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))              '要求端末
                    .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))          'ユーザＩＤ
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '件数を設定
                        Master.ppCount = "0"

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                    Case Else        'データ有り

                        If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                        End If

                        '件数を設定
                        Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = dstOrders.Tables(0)
                        '変更を反映
                        Me.grvList.DataBind()

                End Select

            Catch ex As SqlException

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "構成配信履歴")
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

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "構成配信履歴")
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

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' 2重配信をチェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function ms_GetHaishinChack() As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
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

                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)

                'BB構成ファイル
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxID.ppText))                 'TBOXID
                    .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))               '要求端末
                    .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))           'ユーザＩＤ
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))        '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                '取得したデータをグリッドに設定
                'Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                'Me.grvList.DataBind()

                Select Case strOKNG
                    Case "0"         'データ無し

                        Return True

                    Case Else        'データ有り

                        '配信中
                        psMesBox(Me, "20001", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        Return False

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "構成配信履歴")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "構成配信履歴")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"
            Return False
        End If

    End Function

    ''' <summary>
    ''' 構成配信データ登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_InsHaishin()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strOKNG As String = Nothing                    '検索結果
        Dim cntrol_num As String = Nothing
        Dim dstOrders As New DataSet
        Dim viewSt_in As String = Me.ViewState(M_TENNAIKOUSEI)
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

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction

                    '管理番号の作成
                    cntrol_num = Date.Now.ToString("yyyyMMdd")

                    '管理番号採番
                    'パラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL022", conDB)
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        'パラメータ設定 
                        .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 1))                                    '管理番号
                        .Add(pfSet_Param("YMD", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))         '年月日
                        .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))         '結果取得用
                    End With


                    'データ取得
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    '管理番号取得失敗
                    If cmdDB.Parameters("SalesYTD").Value Is Nothing Then

                        Throw New Exception

                    End If

                    '管理番号を作成する
                    cntrol_num = cntrol_num + CInt(cmdDB.Parameters("SalesYTD").Value).ToString("0000")

                    cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxID.ppText))                'TBOXID
                        .Add(pfSet_Param("prm_file_name", SqlDbType.NVarChar, Me.ddlList.SelectedItem.ToString)) 'ファイル名
                        .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))          'ユーザＩＤ
                        .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))              '端末情報
                        .Add(pfSet_Param("prm_init_dt", SqlDbType.NVarChar, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")))                          '作成日
                        .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, "401"))                              '処理コード
                        .Add(pfSet_Param("prm_tlgrm_snd", SqlDbType.NVarChar, "0"))                              '電文送信
                        .Add(pfSet_Param("prm_ftp_snd", SqlDbType.NVarChar, "0"))                                'FTP送信
                        .Add(pfSet_Param("prm_ftp_rcv", SqlDbType.NVarChar, "0"))                                'FTP受信
                        .Add(pfSet_Param("prm_kanri_nm", SqlDbType.NVarChar, cntrol_num))                        '管理番号
                        .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用
                    End With

                    'データ取得
                    'dstOrders = pfGet_DataSet(cmdDB)

                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    strOKNG = cmdDB.Parameters("data_exist").Value
                    Select Case strOKNG
                        Case "0"
                            'コミット
                            conTrn.Commit()
                        Case Else
                            Throw New Exception
                    End Select
                End Using

                '更新が正常終了
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "照会要求データ")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
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
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
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
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

#End Region

#Region "終了処理"
#End Region

End Class
