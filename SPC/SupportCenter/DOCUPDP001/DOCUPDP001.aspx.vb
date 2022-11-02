'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜サポートセンタ＞
'*　処理名　　：　請求書作成
'*　ＰＧＭＩＤ：　DOCUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.20　：　酒井
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'DOCUPDP001-001     2015/06/03      武       請求書 締め、当月集計、CSVの制御及び締め処理の対象年月を年月度の内容で締めるよう修正    

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

Public Class DOCUPDP001
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
    Const sCnsErr_30008 As String = "30008"

    '請求書作成 画面のパス
    Const sCnsDOCUPDP001 As String = "~/" & P_SPC & "/" &
                                        P_FUN_DOC & P_SCR_UPD & P_PAGE & "001.aspx"

    '一覧ボタン名
    Const sCnsbtnSelect As String = "btnSelect"       '選択

    Const sCnsProgid As String = "DOCUPDP001"
    Const sCnsSqlid_S99 As String = "CNSUPDP001_S4"
    Const sCnsSqlid_I1 As String = "DOCUPDP001_I1"
    Const sCnsSqlid_I2 As String = "DOCUPDP001_I2"
    Const sCnsSqlid_U1 As String = "DOCUPDP001_U1"
    Const sCnsSqlid_U2 As String = "DOCUPDP001_U2"
    Const sCnsSqlid_D1 As String = "DOCUPDP001_D1"
    Const sCnsbtnAdd As String = "追加"
    Const sCnsbtnUpdate As String = "更新"
    Const sCnsbtnDelete As String = "削除"
    Const sCnsCsvButon As String = "ＣＳＶ"
    Const sCnsShimeButon As String = "締め"
    Const sCnsKaijyo As String = "締め解除"
    Const sCnsShukei As String = "当月集計"


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

#Region "ページ初期処理"

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

#Region "Page_Load"

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame
        Try
            If Not IsPostBack Then  '初回表示

                'ボタン活性
                Master.Master.ppRigthButton10.Visible = True    '締め／解除
                Master.Master.ppRigthButton10.Enabled = False
                Master.Master.ppRigthButton9.Visible = True   '当月集計
                Master.Master.ppRigthButton9.Enabled = False
                Master.Master.ppLeftButton10.Visible = True 'ＣＳＶ
                Master.Master.ppLeftButton10.Enabled = False

                '表示設定
                Master.Master.ppRigthButton10.Text = sCnsShimeButon '締め／解除
                Master.Master.ppRigthButton9.Text = sCnsShukei
                Master.Master.ppLeftButton10.Text = sCnsCsvButon          'ＣＳＶ

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = sCnsProgid
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                '初期化処理.
                Call msClearScreen()

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '変更を反映
                Me.grvList.DataBind()

                '活性制御
                If mfCheckClosing(0) = False Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "請求書データが存在しません。画面初期化")

                    psClose_Window(Me)
                    Exit Sub
                End If
            End If

            'ボタンアクションの設定.
            Call msSet_ButtonAction()

        Catch ex As Exception
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
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

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        'メッセージ文言.
        Dim strMessage As String = String.Empty

        objStack = New StackFrame

        Try

            '開始ログ出力.
            psLogStart(Me)

            Select Case sender.ID
                Case "btnUpdate"    '更新
                    '入力項目チェック
                    If mfCheck() = False Then
                        Exit Sub
                    End If

                    '検証チェック.
                    If Not (Page.IsValid) Then
                        Exit Sub
                    End If

                    If mfUpdate() = False Then

                    End If

                Case "btnSearchRigth2"        '検索クリアボタン押下
                    '初期化.
                    Me.dftNendoDt.ppText = String.Empty

                Case "btnSearchRigth1"        '検索ボタン押下
                    strMessage = "検索処理"
                    If (Page.IsValid) Then
                        '条件検索取得
                        msGet_Data()
                    End If

                    'Case "btnRigth9"        '締め／解除ボタン押下
                Case "btnRigth10"        '締め／解除ボタン押下
                    'DOCUPDP001-001
                    If Not Me.dftNendoDt.ppText = String.Empty Then
                        If Me.dftNendoDt.ppText = ViewState("strYM") Then
                            msClose_Click()
                        Else
                            psMesBox(Me, "10006", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画前, "年月度", "検索または当月集計")
                            Exit Sub
                        End If
                    Else
                        psMesBox(Me, "20010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "年月度")
                        Exit Sub
                    End If
                    'DOCUPDP001-001 END
                    'Case "btnSearchRigth4" '当月集計処理
                Case "btnRigth9"        '当月集計処理
                    'DOCUPDP001-001
                    '入力項目チェック
                    If Not Me.dftNendoDt.ppText = String.Empty Then
                        If mfCheck() = False Then
                            Exit Sub
                        End If

                        Call msCreate_Data()
                    Else
                        psMesBox(Me, "20010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "年月度")
                        Exit Sub
                    End If
                    'DOCUPDP001-001 END
                    'Case "btnRigth10"        'ＣＳＶ
                Case "btnLeft10"        'ＣＳＶ
                    'DOCUPDP001-001
                    If Not Me.dftNendoDt.ppText = String.Empty Then
                        If mfCsvDownload("0951CL", "請求書_[" & Me.dftNendoDt.ppText.Replace("/", "") & "]") = False Then
                            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
                        End If
                    Else
                        psMesBox(Me, "20010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "年月度")
                        Exit Sub
                    End If
                    'DOCUPDP001-001 END
            End Select

        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)
    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ButtonAction()

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click   'クリア
        AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click   '検索
        AddHandler Master.Master.ppRigthButton10.Click, AddressOf Button_Click   '締め／解除
        AddHandler Master.Master.ppRigthButton9.Click, AddressOf Button_Click   '当月集計
        AddHandler Master.Master.ppLeftButton10.Click, AddressOf Button_Click    'ＣＳＶ
        AddHandler btnUpdate.Click, AddressOf Button_Click

        '「クリア」ボタン押下時の検証を無効
        Master.ppRigthButton2.CausesValidation = False
        Master.Master.ppRigthButton10.CausesValidation = False
        Master.Master.ppRigthButton9.CausesValidation = False
        Master.Master.ppLeftButton10.CausesValidation = False

        '確認メッセージ設定.
        Master.Master.ppRigthButton10.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton10.Text) '締め確認
        Master.Master.ppRigthButton9.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton9.Text) '当月集計確認

        btnUpdate.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, btnUpdate.Text) '更新確認

    End Sub

    ''' <summary>
    ''' 初期化処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen(Optional ByVal blnClearFlg = False)

        txtSonota1.ppText = String.Empty            'その他１
        txtKingaku1.ppText = String.Empty             '金額１
        txtSonota2.ppText = String.Empty            'その他２
        txtKingaku2.ppText = String.Empty             '金額２
        txtSonota1.ppEnabled = False
        txtKingaku1.ppEnabled = False
        txtSonota2.ppEnabled = False
        txtKingaku2.ppEnabled = False

        '初回のみ初期化.
        If blnClearFlg = False Then
            Me.grvList.DataSource = New Object() {}
            Master.ppCount = "0"
            Me.grvList.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' 入力項目チェック処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCheck() As Boolean
        Dim strErr As String = String.Empty

        '金額2入力チェック
        If Me.txtKingaku2.ppText = String.Empty And Me.txtSonota2.ppText <> String.Empty Then
            Me.txtKingaku2.psSet_ErrorNo("5001", "金額2")
            Me.txtKingaku2.ppTextBox.Focus()
        End If

        strErr = pfCheck_TxtErr(txtKingaku2.ppText, False, True, False, False, 8, "", False)
        If strErr <> "" Then
            Me.txtKingaku2.psSet_ErrorNo(strErr, "金額2", "数字")
            Me.txtKingaku2.ppTextBox.Focus()
        End If

        'その他2入力チェック
        If Me.txtKingaku2.ppText <> String.Empty And Me.txtSonota2.ppText = String.Empty Then
            Me.txtSonota2.psSet_ErrorNo("5001", "その他2")
            Me.txtSonota2.ppTextBox.Focus()
        End If

        '金額1入力チェック
        If Me.txtKingaku1.ppText = String.Empty And Me.txtSonota1.ppText <> String.Empty Then
            Me.txtKingaku1.psSet_ErrorNo("5001", "金額1")
            Me.txtKingaku1.ppTextBox.Focus()
        End If

        strErr = pfCheck_TxtErr(txtKingaku1.ppText, False, True, False, False, 8, "", False)
        If strErr <> "" Then
            Me.txtKingaku1.psSet_ErrorNo(strErr, "金額1", "数字")
            Me.txtKingaku1.ppTextBox.Focus()
        End If

        'その他1入力チェック
        If Me.txtKingaku1.ppText <> String.Empty And Me.txtSonota1.ppText = String.Empty Then
            Me.txtSonota1.psSet_ErrorNo("5001", "その他1")
            Me.txtSonota1.ppTextBox.Focus()
        End If

        Return True
    End Function

#Region "更新処理"
    ''' <summary>
    ''' 更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfUpdate() As Boolean

        'ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim intRtn As Integer

        objStack = New StackFrame

        '初期値.
        mfUpdate = False
        Try

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Function
            End If

            'D37更新・登録.
            Using conTrn = conDB.BeginTransaction
                cmdDB = New SqlCommand("DOCUPDP001_U3", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = conTrn
                With cmdDB.Parameters
                    .Add(pfSet_Param("oth_nm1", SqlDbType.NVarChar, mfGetDBNull(txtSonota1.ppText)))
                    .Add(pfSet_Param("oth_price1", SqlDbType.NVarChar, mfGetDBNull(txtKingaku1.ppText)))
                    .Add(pfSet_Param("oth_nm2", SqlDbType.NVarChar, mfGetDBNull(txtSonota2.ppText)))
                    .Add(pfSet_Param("oth_price2", SqlDbType.NVarChar, mfGetDBNull(txtKingaku2.ppText)))
                    .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                '実行
                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, btnUpdate.Text)
                    'ロールバック
                    conTrn.Rollback()
                    Exit Function
                End If

                'コミット
                conTrn.Commit()
            End Using

            '再検索
            Call msGet_Data()

            '完了メッセージ.
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, btnUpdate.Text)

            'メッセージ.
            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton9.Text)

            '正常
            mfUpdate = True

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try

    End Function
#End Region

#Region "検索処理"
    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As DataSet
        Dim strBill_End_Flg As String = String.Empty
        Dim strBill_Dt As String = String.Empty

        objStack = New StackFrame

        Try

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'パラメータ設定
            cmdDB = New SqlCommand("DOCUPDP001_S1", conDB)
            'DOCUPDP001-001
            cmdDB.Parameters.Add(pfSet_Param("@prmYM", SqlDbType.NVarChar, dftNendoDt.ppText))              '年月度
            'DOCUPDP001-001 END

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            'データがない場合メッセージ.
            strBill_End_Flg = "0"
            If dstOrders.Tables(0).Rows.Count > 0 Then
                '最新の締めデータと年月を比較して画面制御を行う
                If Me.dftNendoDt.ppText.Replace("/", "") > dstOrders.Tables(0).Rows(0).Item("年月度").ToString Then
                    strBill_End_Flg = 0
                    Master.Master.ppRigthButton10.Enabled = True
                Else
                    strBill_End_Flg = 1
                End If
            Else
                Master.Master.ppRigthButton10.Enabled = True
            End If
            strBill_Dt = Me.dftNendoDt.ppText

            'パラメータ設定
            cmdDB = New SqlCommand("DOCUPDP001_S3", conDB)
            cmdDB.Parameters.Add(pfSet_Param("BILL_DT", SqlDbType.NVarChar, dftNendoDt.ppText))              '年月度

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            With dstOrders.Tables(0)
                For zz = 0 To .Rows.Count - 1
                    If .Rows(zz).Item("明細名").ToString.IndexOf("その他") >= 0 Then
                        If .Rows(zz).Item("金額").ToString = String.Empty Then
                            .Rows(zz).Delete()
                        End If
                    End If
                Next
                dstOrders.AcceptChanges()
            End With

            'データがない場合メッセージ.
            If dstOrders.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Else
                '検収書は請求書の締めが行われていないと先にすすまない筈なので締めのチェックはこれでいけるはず...
                For intIndex As Integer = 0 To dstOrders.Tables(0).Rows.Count - 2
                    If dstOrders.Tables(0).Rows(intIndex).Item("締めフラグ").ToString = "0" Then
                        psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "締めが行われていない検収書", "請求書の締め処理を行うこと")
                        Master.Master.ppRigthButton10.Enabled = False
                        Exit For
                    End If
                Next

                '前項目が表示されていない場合は締めボタンを使用不可とする
                If dstOrders.Tables(0).Rows.Count < 7 Then
                    Master.Master.ppRigthButton10.Enabled = False
                End If
                Dim intTotal As Integer = 0
                For intIndex As Integer = 0 To dstOrders.Tables(0).Rows.Count - 2
                    Dim int As Integer
                    If Integer.TryParse(dstOrders.Tables(0).Rows(intIndex).Item("金額").ToString, int) Then
                        intTotal += Integer.Parse(dstOrders.Tables(0).Rows(intIndex).Item("金額").ToString)
                    End If
                Next
                dstOrders.Tables(0).Rows(dstOrders.Tables(0).Rows.Count - 1).Item("金額") = intTotal.ToString("#,0")

                '年月度設定.
                dstOrders.Tables(0).Rows(0).Item("年月度") = dftNendoDt.ppText
            End If

            '件数を設定
            Master.ppCount = dstOrders.Tables(0).Rows.Count

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            Me.grvList.DataSource = dstOrders.Tables(0)

            '変更を反映
            Me.grvList.DataBind()

            '画面・年月度と最新日付比較
            'DOCUPDP001-001
            If strBill_End_Flg = "0" Then
                If dstOrders.Tables(0).Rows.Count > 0 Then
                    If dstOrders.Tables(1).Rows.Count > 0 Then
                        txtSonota1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１名称")         'その他１
                        txtKingaku1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１金額")            '金額１
                        txtSonota2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２名称")         'その他２
                        txtKingaku2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２金額")        '金額２
                        txtSonota1.ppEnabled = True
                        txtKingaku1.ppEnabled = True
                        txtSonota2.ppEnabled = True
                        txtKingaku2.ppEnabled = True
                    Else
                        txtSonota1.ppText = String.Empty
                        txtKingaku1.ppText = String.Empty
                        txtSonota2.ppText = String.Empty
                        txtKingaku2.ppText = String.Empty
                        txtSonota1.ppEnabled = True
                        txtKingaku1.ppEnabled = True
                        txtSonota2.ppEnabled = True
                        txtKingaku2.ppEnabled = True
                    End If
                    Master.Master.ppRigthButton10.Text = "締め"
                    For intCnt As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                        Select Case dstOrders.Tables(0).Rows(intCnt).Item("締めフラグ")
                            Case "0"
                                Master.Master.ppRigthButton10.Enabled = False                               '締め/解除
                                Exit For
                            Case "1"
                                Master.Master.ppRigthButton10.Enabled = True
                        End Select
                    Next
                    Master.Master.ppRigthButton9.Enabled = True                               '当月集計
                    Master.Master.ppLeftButton10.Enabled = True                                'CSV
                Else
                    txtSonota1.ppText = String.Empty
                    txtKingaku1.ppText = String.Empty
                    txtSonota2.ppText = String.Empty
                    txtKingaku2.ppText = String.Empty
                    txtSonota1.ppEnabled = True
                    txtKingaku1.ppEnabled = True
                    txtSonota2.ppEnabled = True
                    txtKingaku2.ppEnabled = True
                    Master.Master.ppRigthButton10.Text = "締め"
                    Master.Master.ppRigthButton10.Enabled = False                               '締め/解除
                    Master.Master.ppRigthButton9.Enabled = True                               '当月集計
                    Master.Master.ppLeftButton10.Enabled = True                                'CSV
                End If
            Else
                txtSonota1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１名称")         'その他１
                txtKingaku1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１金額")            '金額１
                txtSonota2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２名称")         'その他２
                txtKingaku2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２金額")        '金額２
                txtSonota1.ppEnabled = False
                txtKingaku1.ppEnabled = False
                txtSonota2.ppEnabled = False
                txtKingaku2.ppEnabled = False
                Master.Master.ppRigthButton10.Text = "締め解除"
                If dstOrders.Tables(2).Rows.Count > 0 Then
                    For intCnt As Integer = 0 To dstOrders.Tables(2).Rows.Count - 1
                        Select Case dstOrders.Tables(2).Rows(intCnt).Item("締めフラグ")
                            Case "1"
                                Master.Master.ppRigthButton10.Enabled = False                               '締め/解除
                                Exit For
                            Case "0"
                                Master.Master.ppRigthButton10.Enabled = True
                        End Select
                    Next
                Else
                    Master.Master.ppRigthButton10.Enabled = True
                End If

                Master.Master.ppRigthButton9.Enabled = False                               '当月集計
                Master.Master.ppLeftButton10.Enabled = True                                'CSV
            End If

            msSet_ButtonAction()
            ViewState("strYM") = Me.dftNendoDt.ppText
            'DOCUPDP001-001 END

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 再検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSeach_Data()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As DataSet
        Dim strBill_End_Flg As String = String.Empty
        Dim strBill_Dt As String = String.Empty
        Dim intTotal As Integer = 0

        objStack = New StackFrame

        Try

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'パラメータ設定
            cmdDB = New SqlCommand("DOCUPDP001_S1", conDB)
            'DOCUPDP001-001
            cmdDB.Parameters.Add(pfSet_Param("@prmYM", SqlDbType.NVarChar, dftNendoDt.ppText))              '年月度
            'DOCUPDP001-001 END

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            'データがない場合メッセージ.
            strBill_End_Flg = "0"
            If dstOrders.Tables(0).Rows.Count > 0 Then
                '最新の締めデータと年月を比較して画面制御を行う
                If Me.dftNendoDt.ppText.Replace("/", "") > dstOrders.Tables(0).Rows(0).Item("年月度").ToString Then
                    strBill_End_Flg = 0
                    Master.Master.ppRigthButton10.Enabled = True
                Else
                    strBill_End_Flg = 1
                End If
            Else
                Master.Master.ppRigthButton10.Enabled = True
            End If
            strBill_Dt = Me.dftNendoDt.ppText

            'パラメータ設定
            cmdDB = New SqlCommand("DOCUPDP001_S3", conDB)
            cmdDB.Parameters.Add(pfSet_Param("BILL_DT", SqlDbType.NVarChar, strBill_Dt))

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
            Else
                '検収書は全て締めが行われていないと先にすすまない筈なので締めのチェックはこれでいけるはず...
                For intIndex As Integer = 0 To dstOrders.Tables(0).Rows.Count - 2
                    If dstOrders.Tables(0).Rows(intIndex).Item("締めフラグ").ToString = "0" Then
                        Master.Master.ppRigthButton10.Enabled = False
                        Exit For
                    End If
                Next

                intTotal = 0
                For intIndex As Integer = 0 To dstOrders.Tables(0).Rows.Count - 2
                    intTotal += Integer.Parse(dstOrders.Tables(0).Rows(intIndex).Item("金額").ToString)
                Next
                dstOrders.Tables(0).Rows(dstOrders.Tables(0).Rows.Count - 1).Item("金額") = intTotal.ToString("#,0")

                '年月度設定.
                dstOrders.Tables(0).Rows(0).Item("年月度") = strBill_Dt
            End If

            '件数を設定
            Master.ppCount = dstOrders.Tables(0).Rows.Count

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            Me.grvList.DataSource = dstOrders.Tables(0)

            '変更を反映
            Me.grvList.DataBind()
            'DOCUPDP001-001
            If strBill_End_Flg = "0" Then
                If dstOrders.Tables(0).Rows.Count > 0 Then
                    If dstOrders.Tables(1).Rows.Count > 0 Then
                        txtSonota1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１名称")         'その他１
                        txtKingaku1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１金額")            '金額１
                        txtSonota2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２名称")         'その他２
                        txtKingaku2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２金額")        '金額２
                        txtSonota1.ppEnabled = True
                        txtKingaku1.ppEnabled = True
                        txtSonota2.ppEnabled = True
                        txtKingaku2.ppEnabled = True
                    Else
                        txtSonota1.ppText = String.Empty
                        txtKingaku1.ppText = String.Empty
                        txtSonota2.ppText = String.Empty
                        txtKingaku2.ppText = String.Empty
                        txtSonota1.ppEnabled = True
                        txtKingaku1.ppEnabled = True
                        txtSonota2.ppEnabled = True
                        txtKingaku2.ppEnabled = True
                    End If
                    Master.Master.ppRigthButton10.Text = "締め"
                    For intCnt As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                        Select Case dstOrders.Tables(0).Rows(intCnt).Item("締めフラグ")
                            Case "0"
                                Master.Master.ppRigthButton10.Enabled = False                               '締め/解除
                                Exit For
                            Case "1"
                                Master.Master.ppRigthButton10.Enabled = True
                        End Select
                    Next
                    Master.Master.ppRigthButton9.Enabled = True                               '当月集計
                    Master.Master.ppLeftButton10.Enabled = True                                'CSV
                Else
                    txtSonota1.ppText = String.Empty
                    txtKingaku1.ppText = String.Empty
                    txtSonota2.ppText = String.Empty
                    txtKingaku2.ppText = String.Empty
                    txtSonota1.ppEnabled = True
                    txtKingaku1.ppEnabled = True
                    txtSonota2.ppEnabled = True
                    txtKingaku2.ppEnabled = True
                    Master.Master.ppRigthButton10.Text = "締め"
                    Master.Master.ppRigthButton10.Enabled = False                               '締め/解除
                    Master.Master.ppRigthButton9.Enabled = True                               '当月集計
                    Master.Master.ppLeftButton10.Enabled = True                                'CSV
                End If
            Else
                txtSonota1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１名称")         'その他１
                txtKingaku1.ppText = dstOrders.Tables(1).Rows(0).Item("その他１金額")            '金額１
                txtSonota2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２名称")         'その他２
                txtKingaku2.ppText = dstOrders.Tables(1).Rows(0).Item("その他２金額")        '金額２
                txtSonota1.ppEnabled = False
                txtKingaku1.ppEnabled = False
                txtSonota2.ppEnabled = False
                txtKingaku2.ppEnabled = False
                Master.Master.ppRigthButton10.Text = "締め解除"
                If dstOrders.Tables(2).Rows.Count > 0 Then
                    For intCnt As Integer = 0 To dstOrders.Tables(2).Rows.Count - 1
                        Select Case dstOrders.Tables(2).Rows(intCnt).Item("締めフラグ")
                            Case "1"
                                Master.Master.ppRigthButton10.Enabled = False                               '締め/解除
                                Exit For
                            Case "0"
                                Master.Master.ppRigthButton10.Enabled = True
                        End Select
                    Next
                Else
                    Master.Master.ppRigthButton10.Enabled = True
                End If
                Master.Master.ppRigthButton9.Enabled = False                               '当月集計
                Master.Master.ppLeftButton10.Enabled = True                                'CSV
            End If

            msSet_ButtonAction()
            ViewState("strYM") = Me.dftNendoDt.ppText
            'DOCUPDP001-001 END

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try

    End Sub
#End Region

#Region "当月集計処理"
    ''' <summary>
    ''' 当月集計処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCreate_Data()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim dstCsv As New DataSet
        Dim strBill_No As String = String.Empty
        Dim intRtn As Integer
        Dim strBill_No_Y As String = String.Empty

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                'リストデータ取得
                cmdDB = New SqlCommand("DOCUPDP001_S5", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("ym", SqlDbType.NVarChar, Me.dftNendoDt.ppText.Replace("/", "")))
                    .Add(pfSet_Param("oth_nm1", SqlDbType.NVarChar, mfGetDBNull(txtSonota1.ppText)))
                    .Add(pfSet_Param("oth_price1", SqlDbType.NVarChar, mfGetDBNull(txtKingaku1.ppText)))
                    .Add(pfSet_Param("oth_nm2", SqlDbType.NVarChar, mfGetDBNull(txtSonota2.ppText)))
                    .Add(pfSet_Param("oth_price2", SqlDbType.NVarChar, mfGetDBNull(txtKingaku2.ppText)))
                End With
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '0件の場合
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    cmdDB = New SqlCommand("DOCUPDP001_S2", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("ym", SqlDbType.NVarChar, Me.dftNendoDt.ppText.Replace("/", "")))
                        .Add(pfSet_Param("oth_nm1", SqlDbType.NVarChar, mfGetDBNull(txtSonota1.ppText)))
                        .Add(pfSet_Param("oth_price1", SqlDbType.NVarChar, mfGetDBNull(txtKingaku1.ppText)))
                        .Add(pfSet_Param("oth_nm2", SqlDbType.NVarChar, mfGetDBNull(txtSonota2.ppText)))
                        .Add(pfSet_Param("oth_price2", SqlDbType.NVarChar, mfGetDBNull(txtKingaku2.ppText)))
                    End With
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                End If

                If Me.dftNendoDt.ppDate.Month <= 3 Then
                    strBill_No_Y = (Me.dftNendoDt.ppDate.Year - 1).ToString
                Else
                    strBill_No_Y = Me.dftNendoDt.ppDate.Year.ToString
                End If

                '管理番号
                strBill_No = "No." &
                             strBill_No_Y.ToString().Substring(2, 2) &
                             "-" &
                             Me.dftNendoDt.ppDate.Year.ToString.Substring(2, 2) &
                             "-" &
                             "0" & Me.dftNendoDt.ppDate.Month &
                             "01_0"

                'D37更新・登録.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand("DOCUPDP001_U1", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("ym", SqlDbType.NVarChar, Me.dftNendoDt.ppText.Replace("/", "")))
                        .Add(pfSet_Param("bill_no", SqlDbType.NVarChar, strBill_No))
                        .Add(pfSet_Param("comp_nm", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("宛先会社名").ToString))
                        .Add(pfSet_Param("branch_nm", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("宛先部署名").ToString))
                        .Add(pfSet_Param("amount", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("課税対象料金計").ToString))
                        .Add(pfSet_Param("paylimit", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("支払い期限").ToString))
                        .Add(pfSet_Param("bank_nm", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("振込み先銀行名").ToString))
                        .Add(pfSet_Param("b_branch_nm", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("振込み先支店名").ToString))
                        .Add(pfSet_Param("b_name", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("口座名義").ToString))
                        .Add(pfSet_Param("b_no", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("口座番号").ToString))
                        .Add(pfSet_Param("oth_nm1", SqlDbType.NVarChar, mfGetDBNull(txtSonota1.ppText)))
                        .Add(pfSet_Param("oth_price1", SqlDbType.NVarChar, mfGetDBNull(txtKingaku1.ppText)))
                        .Add(pfSet_Param("oth_nm2", SqlDbType.NVarChar, mfGetDBNull(txtSonota2.ppText)))
                        .Add(pfSet_Param("oth_price2", SqlDbType.NVarChar, mfGetDBNull(txtKingaku2.ppText)))
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton9.Text)

                        'ロールバック
                        conTrn.Rollback()
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                'CSV出力用テーブル定義
                dstCsv.Tables.Add("0")
                dstCsv.Tables(0).Columns.Add("請求年月度", GetType(String))
                dstCsv.Tables(0).Columns.Add("管理番号", GetType(String))
                dstCsv.Tables(0).Columns.Add("請求書作成日", GetType(String))
                dstCsv.Tables(0).Columns.Add("宛先会社名", GetType(String))
                dstCsv.Tables(0).Columns.Add("宛先部署名", GetType(String))
                dstCsv.Tables(0).Columns.Add("支払い期限", GetType(String))
                dstCsv.Tables(0).Columns.Add("振込み先銀行名", GetType(String))
                dstCsv.Tables(0).Columns.Add("振込み先支店名", GetType(String))
                dstCsv.Tables(0).Columns.Add("口座番号", GetType(String))
                dstCsv.Tables(0).Columns.Add("口座名義", GetType(String))
                dstCsv.Tables(0).Columns.Add("サポートセンタ運用", GetType(String))
                dstCsv.Tables(0).Columns.Add("情報機器工事", GetType(String))
                dstCsv.Tables(0).Columns.Add("情報機器保守", GetType(String))
                dstCsv.Tables(0).Columns.Add("情報機器整備", GetType(String))
                dstCsv.Tables(0).Columns.Add("その他１(" & dstOrders.Tables(0).Rows(0).Item("その他１名称") & ")", GetType(String))
                dstCsv.Tables(0).Columns.Add("その他２(" & dstOrders.Tables(0).Rows(0).Item("その他２名称") & ")", GetType(String))
                dstCsv.Tables(0).Columns.Add("小計", GetType(String))
                dstCsv.Tables(0).Columns.Add("出精値引き額", GetType(String))
                dstCsv.Tables(0).Columns.Add("課税対象料金計", GetType(String))
                dstCsv.Tables(0).Columns.Add("消費税相当額", GetType(String))
                dstCsv.Tables(0).Columns.Add("請求額", GetType(String))

                dstCsv.Tables(0).Rows.Add(mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("請求年月度").ToString, "1") & "度",
                                          strBill_No,
                                          mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("請求書作成日").ToString, "0"),
                                          dstOrders.Tables(0).Rows(0).Item("宛先会社名").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("宛先部署名").ToString,
                                          mfChange_Jp(dstOrders.Tables(0).Rows(0).Item("支払い期限").ToString, "0"),
                                          dstOrders.Tables(0).Rows(0).Item("振込み先銀行名").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("振込み先支店名").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("口座番号").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("口座名義").ToString,
                                          mfGetDBNull(dstOrders.Tables(0).Rows(0).Item("サポートセンタ運用").ToString),
                                          mfGetDBNull(dstOrders.Tables(0).Rows(0).Item("情報機器工事").ToString),
                                          mfGetDBNull(dstOrders.Tables(0).Rows(0).Item("情報機器保守").ToString),
                                          mfGetDBNull(dstOrders.Tables(0).Rows(0).Item("情報機器整備").ToString),
                                          mfGetDBNull(dstOrders.Tables(0).Rows(0).Item("その他１金額").ToString),
                                          mfGetDBNull(dstOrders.Tables(0).Rows(0).Item("その他２金額").ToString),
                                          dstOrders.Tables(0).Rows(0).Item("小計").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("出精値引き額").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("課税対象料金計").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("消費税相当額").ToString,
                                          dstOrders.Tables(0).Rows(0).Item("請求額").ToString)

                'CSV作成.
                If mfMakeCsv("0951CL", "請求書_[" & dstOrders.Tables(0).Rows(0).Item("請求年月度").ToString().Substring(0, 7).Replace("/", "") & "]", dstCsv) = False Then
                    Exit Sub
                End If
                ViewState("BILL_DT") = dstOrders.Tables(0).Rows(0).Item("請求年月度").ToString().Substring(0, 7).Replace("/", "")

                '画面更新
                mfCheckClosing(0)

                '再検索
                msSeach_Data()

                '完了メッセージ.
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton9.Text)
            End If

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "当月集計")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub
#End Region

#Region "締め処理"
    ''' <summary>
    ''' 締め処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClose_Click()
        Try
            'ログ出力開始
            psLogStart(Me)

            Select Case Master.Master.ppRigthButton10.Text
                Case "締め"

                    If mfCheckClosing(1) = False Then
                        Exit Sub
                    End If

                    '締め処理.
                    If mfUpdateClose() = False Then
                    Else
                        '再検索
                        msSeach_Data()
                        '画面更新
                        Call mfCheckClosing(0)
                        'ボタンアクション再設定
                        Call msSet_ButtonAction()
                    End If

                Case sCnsKaijyo ' "解除"
                    If mfCheckClosing(1) = False Then
                        Exit Sub
                    End If
                    '締め解除処理.
                    If mfUpdateClose() = False Then
                    Else
                        '再検索
                        msSeach_Data()
                        '画面更新
                        Call mfCheckClosing(0)
                        'ボタンアクション再設定
                        Call msSet_ButtonAction()
                    End If
            End Select

            'ログ出力終了
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton10.Text & "処理")
        End Try
    End Sub

    ''' <summary>
    ''' 画面更新/締めチェック処理
    ''' </summary>
    ''' <param name="intWork">0:画面更新 1:締めチェック</param>
    ''' <remarks></remarks>
    Private Function mfCheckClosing(ByVal intWork As Integer) As Boolean

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim strBill_End As String = String.Empty
        Dim strResult_Ins As String = String.Empty
        Dim strResult_Req As String = String.Empty
        Dim strBill_dt As String = String.Empty
        Dim shtMessage As Short = 0

        objStack = New StackFrame
        mfCheckClosing = False
        '初期化
        conDB = Nothing

        '描画設定
        If intWork = 0 Then
            shtMessage = ClsComVer.E_S実行.描画前
        Else
            shtMessage = ClsComVer.E_S実行.描画後
        End If

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, shtMessage)
            Exit Function
        Else
            Try
                cmdDB = New SqlCommand("DOCUPDP001_S4", conDB)
                cmdDB.Parameters.Add(pfSet_Param("bill_end_flg", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d88_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d89_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d90_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d91_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d88_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d89_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d90_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d91_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                'DOCUPDP001-001
                cmdDB.Parameters.Add(pfSet_Param("prmYM", SqlDbType.NVarChar, Me.dftNendoDt.ppText))
                'DOCUPDP001-001 END

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strBill_End = cmdDB.Parameters("bill_end_flg").Value.ToString
                strResult_Ins = cmdDB.Parameters("d88_ins_cls").Value.ToString &
                                cmdDB.Parameters("d89_ins_cls").Value.ToString &
                                cmdDB.Parameters("d90_ins_cls").Value.ToString &
                                cmdDB.Parameters("d91_ins_cls").Value.ToString
                strResult_Req = cmdDB.Parameters("d88_req_cls").Value.ToString &
                                cmdDB.Parameters("d89_req_cls").Value.ToString &
                                cmdDB.Parameters("d90_req_cls").Value.ToString &
                                cmdDB.Parameters("d91_req_cls").Value.ToString

                '最新年月度取得
                cmdDB = New SqlCommand("DOCUPDP001_S1", conDB)
                'DOCUPDP001-001
                cmdDB.Parameters.Add(pfSet_Param("@prmYM", SqlDbType.NVarChar, dftNendoDt.ppText))              '年月度
                'DOCUPDP001-001 END
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                If dstOrders.Tables(0).Rows.Count > 0 Then
                    ViewState("BILL_DT") = dstOrders.Tables(0).Rows(0).Item("年月度").ToString()
                Else
                    ViewState("BILL_DT") = ""
                End If
                If intWork = 0 Then
                    'DOCUPDP001-001
                    '締めボタン文言.
                    'If strBill_End = "0" Then
                    '    Master.Master.ppRigthButton10.Text = sCnsShimeButon '締め
                    '    Master.Master.ppLeftButton10.Enabled = True 'CSV
                    'Else
                    '    Master.Master.ppRigthButton10.Text = sCnsKaijyo '解除
                    '    Master.Master.ppLeftButton10.Enabled = False 'CSV
                    'End If

                    'Select Case Master.Master.ppRigthButton10.Text
                    '    Case sCnsShimeButon
                    '        If strResult_Ins = "1111" And strResult_Req = "0000" Then
                    '            Master.Master.ppRigthButton10.Enabled = True '締め/解除
                    '            Master.Master.ppRigthButton9.Enabled = True '当月集計
                    '        Else
                    '            Master.Master.ppRigthButton10.Enabled = False '締め/解除
                    '            Master.Master.ppRigthButton9.Enabled = True '当月集計
                    '        End If

                    '    Case sCnsKaijyo
                    '        If strResult_Ins = "1111" And strResult_Req = "1111" Then
                    '            Master.Master.ppRigthButton10.Enabled = True '締め/解除
                    '            Master.Master.ppRigthButton9.Enabled = True '当月集計
                    '        Else
                    '            Master.Master.ppRigthButton10.Enabled = False '締め/解除
                    '            Master.Master.ppRigthButton9.Enabled = True '当月集計
                    '        End If
                    'End Select
                    'DOCUPDP001-001 END
                Else
                    Select Case Master.Master.ppRigthButton10.Text
                        Case sCnsShimeButon
                            If strResult_Ins <> "1111" Then
                                psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "未検収の検収書", "締め処理")
                                Exit Function
                            ElseIf strResult_Ins = "1111" And strResult_Req = "1111" Then
                                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "請求書の締め処理")
                                Exit Function
                            End If
                        Case sCnsKaijyo
                            If strResult_Ins = "1111" And strResult_Req = "0000" Then
                                psMesBox(Me, "30011", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "請求書の締め解除")
                                Exit Function
                            ElseIf strResult_Ins <> "1111" And strResult_Req = "0000" Then
                                psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "未検収の検収書", "締め解除")
                                Exit Function
                            ElseIf strResult_Ins = "" And strResult_Req = "" Then
                                psMesBox(Me, "30010", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "未検収の検収書", "締め解除")
                                Exit Function
                            End If
                    End Select
                End If

                '正常終了
                mfCheckClosing = True

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Exit Function
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, shtMessage)
                End If
            End Try
        End If
    End Function

    ''' <summary>
    ''' 締め・締め解除処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfUpdateClose() As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer

        objStack = New StackFrame
        mfUpdateClose = False '初期値

        Try
            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                'トランザクション.
                Using conTrn = conDB.BeginTransaction
                    cmdDB = New SqlCommand("DOCUPDP001_U2", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        'DOCUPDP001-001
                        .Add(pfSet_Param("prmYM", SqlDbType.NVarChar, Me.dftNendoDt.ppText))
                        'DOCUPDP001-001 END
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton10.Text)
                        'ロールバック
                        conTrn.Rollback()
                        Exit Function
                    End If
                    'コミット
                    conTrn.Commit()
                    '成功.
                    mfUpdateClose = True
                    '完了メッセージ.
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton10.Text)

                End Using

            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton10.Text)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try
    End Function
#End Region

#Region "CSV処理"
    ''' <summary>
    ''' CSVファイル取り込み.
    ''' </summary>
    ''' <param name="ipstrFileclassCD"></param>
    ''' <param name="ipstrReportName"></param>
    ''' <param name="ipstrOrderNo"></param>
    ''' <param name="CSVData"></param>
    ''' <param name="Cnt"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetCSVData(ByVal ipstrFileclassCD As String,
                              ByVal ipstrReportName As String,
                              ByVal ipstrOrderNo As String,
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

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
        End If

        strFilePath = pfFile_Download(strFolderNM, ipstrReportName & "_[" & ipstrOrderNo & "]" & ".csv")
        'パス生成.
        'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & "_[" & ipstrOrderNo & "]" & ".csv"

        'ファイル存在チェック
        If Not System.IO.File.Exists(strFilePath) Then
            psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ipstrReportName)
            Exit Sub
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
            'CSVファイル内の整形開始
            '一行づつ読み込む
            Do Until sr.EndOfStream = True
                Dim strMoji As String = Nothing                                                '一文字格納

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
    End Sub

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
        If pfCreateCsvFile(strlocalpath, localFileName & ".csv", ds, True) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        'サーバアドレス取得.
        If pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM) <> 0 Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        '出力パス生成.
        'strWorkPath = "\\" & strServerAddress & "\" & strFolderNM
        If pfFile_Upload(strFolderNM, ipstrReportName & ".csv", localFileName) = False Then
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSV")
            Return False
        End If

        Return True
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
        'strFilePath = "\\" & strServerAddress & "\" & strFolderNM & "\" & ipstrReportName & ".csv"
        strData = ipstrReportName & ".csv"

        strLocalPath = pfFile_Download(strFolderNM, strData)
        If strLocalPath = "" Then
            psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strData)
            Exit Function
        End If

        Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strLocalPath & "&filename=" & HttpUtility.UrlEncode(strData), False)

        'Dim uBytes = Encoding.UTF8.GetBytes(strData)
        'strDownloadFileName = HttpUtility.UrlEncode(strData)

        ''■□■□結合試験時のみ使用予定□■□■
        'Dim objStack As New StackFrame
        'Dim strPrm As String = ""
        'strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
        'Dim tmp As Object() = Session(P_KEY)
        'If Not tmp Is Nothing Then
        '    For zz = 0 To tmp.Length - 1
        '        If zz <> tmp.Length - 1 Then
        '            strPrm &= tmp(zz).ToString & ","
        '        Else
        '            strPrm &= tmp(zz).ToString
        '        End If
        '    Next
        'End If

        'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
        '                objStack.GetMethod.Name, "~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFilePath & "&filename=" & strDownloadFileName, strPrm, "TRANS")

        ''■□■□結合試験時のみ使用予定□■□■
        'Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFilePath & "&filename=" & strDownloadFileName, False)

        Return True

    End Function

    ''' <summary>
    ''' CSV出力可否チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfCheckCsv() As Boolean

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        Dim strBill_End As String = String.Empty
        Dim strResult_Ins As String = String.Empty
        Dim strResult_Req As String = String.Empty
        Dim strBill_dt As String = String.Empty
        Dim shtMessage As Short = 0

        objStack = New StackFrame
        mfCheckCsv = False

        '初期化
        conDB = Nothing

        '接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, shtMessage)
            Exit Function
        Else
            Try
                cmdDB = New SqlCommand("DOCUPDP001_S4", conDB)
                cmdDB.Parameters.Add(pfSet_Param("bill_end_flg", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d88_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d89_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d90_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d91_ins_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d88_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d89_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d90_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                cmdDB.Parameters.Add(pfSet_Param("d91_req_cls", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                'DOCUPDP001-001
                cmdDB.Parameters.Add(pfSet_Param("prmYM", SqlDbType.NVarChar, Me.dftNendoDt.ppText))
                'DOCUPDP001-001 END
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strBill_End = cmdDB.Parameters("bill_end_flg").Value.ToString

                'CSV出力可否判定.
                If strBill_End <> "0" Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "請求書の締め処理", "CSV出力")
                    Exit Function
                End If

                '正常終了
                mfCheckCsv = True

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Exit Function
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, shtMessage)
                End If
            End Try
        End If
    End Function
#End Region

#Region "変換処理"

    ''' <summary>
    ''' 日付変換処理.
    ''' </summary>
    ''' <param name="strReport_D">日付</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChange_Jp(ByVal strReport_D As String, ByVal strType As String) As String

        Dim strYear As String = String.Empty
        Dim strMouth As String = String.Empty
        Dim strDay As String = String.Empty
        Dim dtTarget As DateTime = Nothing
        Dim culture As CultureInfo = New CultureInfo("ja-JP", True)

        culture.DateTimeFormat.Calendar = New JapaneseCalendar()
        objStack = New StackFrame

        Try
            If DateTime.TryParse(strReport_D, dtTarget) = False Then
                strType = "9"
            End If
        Catch ex As Exception
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "日付の変換")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '戻り値
        Select Case strType
            Case "0"
                'Return dtTarget.ToString("ggyy年M月d日", culture)
                Return dtTarget.ToString("yyyy年M月d日")
            Case "1"
                'Return dtTarget.ToString("ggyy年M月", culture)
                Return dtTarget.ToString("yyyy年M月")
            Case Else
                Return String.Empty
        End Select
    End Function

    ''' <summary>
    ''' DBNULL取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If

        Return strVal
    End Function

#End Region

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
