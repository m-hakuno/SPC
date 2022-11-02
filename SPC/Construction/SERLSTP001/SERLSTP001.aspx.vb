'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事業務＞
'*　処理名　　：　シリアル情報一覧
'*　ＰＧＭＩＤ：　SERLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　：　2013.12.10　：　ＮＫＣ
'*  変　更　：　2018.07.19　：　伯野
'********************************************************************************************************************************

'---------------------------------------------------------------------------------------------------------------------------------
'番号　　　　　｜　日付　　　　　｜　名前　　｜　備考
'---------------------------------------------------------------------------------------------------------------------------------
'SERLSTP001_001    2016/01/25        武         FS移転に伴う帳票修正
'SERLSTP001_002    2016/08/30        栗原       シリアル情報の登録仕様変更に伴う修正
'SERLSTP001_003    2018/07/19        伯野       ＮＧＣ要望によりＣＳＶ出力を追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
'Imports SPC.Global_asax
Imports DBFTP.ClsDBFTP_Main
Imports DBFTP.ClsFTPCnfg
Imports DBFTP.ClsSQLSvrDB
#End Region

Public Class SERLSTP001
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
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_SER & P_SCR_LST & P_PAGE & "001"
    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_CNS & "/" &
            P_FUN_SER & P_SCR_UPD & P_PAGE & "001" & "/" &
            P_FUN_SER & P_SCR_UPD & P_PAGE & "001.aspx"
    'ビューステート
    Const M_VS_SEARCH = "SearchCondition"

    'SERLSTP001-002
    '業者コード
    Const TRD_EIGYO = "2"  '営業所
    Const TRD_HOSHU = "3"  '保守拠点
    Const TRD_DAIRI = "4"  '代理店
    Const TRD_MAKER = "5"  'メーカ
    Const TRD_SONOTA = "6" 'その他
    'SERLSTP001-002 END

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
    Dim DBFTP As New DBFTP.ClsDBFTP_Main

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnCsvSpareCnds_Click
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf btnCsvSearchRslt_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnSerialEntry_Click

        'ドロップダウンリストアクションの設定
        AddHandler Me.ddlPrnSelect.ppDropDownList.SelectedIndexChanged, AddressOf ddlPrnSelect_SelectedIndexChanged
        Me.ddlPrnSelect.ppDropDownList.AutoPostBack = True

        'SERUPDP001-002
        AddHandler Me.ddlPlaceclass.ppDropDownList.SelectedIndexChanged, AddressOf ddlPlaceclass_SelectedIndexChanged
        Me.ddlPlaceclass.ppDropDownList.AutoPostBack = True

        'テキストボックスのアクション設定
        AddHandler Me.txtStrageCd.ppTextBox.TextChanged, AddressOf txtStrageCd_TextChanged
        Me.txtStrageCd.ppTextBox.AutoPostBack = True

        'SERUPDP001-002　END

        If Not IsPostBack Then  '初回表示のみ

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '各コマンドボタンの属性設定
            '--検索条件クリア
            Master.ppRigthButton2.CausesValidation = False
            '--予備機配備状況ＣＳＶ
            Master.Master.ppLeftButton1.Text = "予備機配備状況ＣＳＶ"
            Master.Master.ppLeftButton1.Visible = True
            Master.Master.ppLeftButton1.CausesValidation = False
            '--検索結果ＣＳＶ
            Master.Master.ppLeftButton2.Text = "検索結果ＣＳＶ"
            Master.Master.ppLeftButton2.Visible = True
            Master.Master.ppLeftButton2.CausesValidation = False
            '--シリアル登録
            Master.Master.ppRigthButton1.Text = "シリアル登録"
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.CausesValidation = False

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ドロップダウンリスト設定
            msSetddlAppadiv()    '機器分類
            msSetddlSystem()     'システム
            msSetddlMoveReason() '移動理由

            '画面クリア
            msClearScreen()

        End If

    End Sub

    '---------------------------
    '2014/04/15 武 ここから
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
                Master.Master.ppRigthButton1.Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/04/15 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing          'データセット
        Dim intSearchCls As Integer = Nothing   '検索区分（ 1:最新シリアル履歴、2:全シリアル履歴 ）

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckSearchCondition()

        'データ取得
        If (Page.IsValid) Then
            '検索条件を保持
            intSearchCls = msRetentionSearchCondition()

            'データ取得処理
            If intSearchCls = 1 Then
                objDs = mfGetData(M_MY_DISP_ID + "_S1") '最新シリアル履歴
            Else
                objDs = mfGetDataAllHistory()           '全シリアル履歴
            End If
            If Not objDs Is Nothing Then
                '画面表示
                msDispData(objDs)

                'If Session(P_SESSION_AUTH) <> "NGC" And Session(P_SESSION_AUTH) <> "営業所" Then
                If Session(P_SESSION_AUTH) <> "NGC" Then
                    If intSearchCls = 1 Then
                        If mfGetRecordCount() <> 0 Then
                            Me.ddlPrnSelect.ppEnabled = True
                        Else
                            Me.ddlPrnSelect.ppEnabled = False
                        End If
                    Else
                        '全シリアル履歴検索の場合は帳票選択は非活性にする
                        Me.ddlPrnSelect.ppEnabled = False
                    End If
                Else
                    Me.ddlPrnSelect.ppEnabled = False
                End If

                '帳票未選択状態にし、ＣＳＶ・ＰＤＦ・印刷ボタンを非活性にする
                Me.ddlPrnSelect.ppDropDownList.SelectedIndex = 0
                Me.btnCsv.Enabled = False
                Me.btnPdf.Enabled = False
                Me.btnPrint.Enabled = False
                Me.btnCsv.Enabled = False
            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClearSearchCondition()

        Me.txtSerialNo.ppTextBoxFrom.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 予備機配備状況ＣＳＶボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsvSpareCnds_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        Try

            'データ取得処理
            objDs = mfGetDataSpareCnds()
            If Not objDs Is Nothing Then

                'データセットマージ（営業所別データと総合計）
                objDs.Tables(0).Merge(objDs.Tables(1))

                'CSVファイルダウンロード
                If pfDLCsvFile(Master.Master.ppLeftButton1.Text.Replace("ＣＳＶ", "_") +
                               DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                               objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                End If

            End If

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Threading.ThreadAbortException
            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' 検索結果ＣＳＶボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsvSearchRslt_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        Try

            'データ取得処理
            objDs = mfGetDataSearch()

            If Not objDs Is Nothing Then

                'CSVファイルダウンロード
                If pfDLCsvFile(Master.Master.ppLeftButton2.Text.Replace("ＣＳＶ", "_") +
                               DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                               objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                End If

            End If

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Threading.ThreadAbortException
            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' シリアル登録ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSerialEntry_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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
                        objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        'シリアル登録画面起動
        psOpen_Window(Me, M_NEXT_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 機器分類マスタドロップダウンリスト変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlAppaDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAppaDiv.SelectedIndexChanged
        Dim tmp As String = ddlAppaCls.SelectedValue

        'ドロップダウンリスト設定
        msSetddlAppacls()   '機器種別

        If ddlAppaCls.Items.FindByValue(tmp) Is Nothing Then
        Else
            Me.ddlAppaCls.SelectedValue = tmp
        End If

    End Sub

    ''' <summary>
    ''' 帳票選択変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlPrnSelect_SelectedIndexChanged()

        If Master.ppCount > "0" Then
            'ＰＤＦボタン活性・非活性
            Select Case Me.ddlPrnSelect.ppSelectedValue
                Case 2, 3, 6
                    If Session(P_SESSION_AUTH) <> "NGC" And Session(P_SESSION_AUTH) <> "営業所" Then
                        Me.btnPdf.Enabled = True
                    Else
                        Me.btnPdf.Enabled = False
                    End If

                    '確認メッセージ
                    Me.btnPdf.OnClientClick =
                        pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel,
                                         mfGetNamePart(Me.ddlPrnSelect.ppSelectedText))
                Case Else
                    Me.btnPdf.Enabled = False
            End Select

            'ＣＳＶボタン使用不可
            Me.btnCsv.Enabled = False

            'ＣＳＶ・印刷ボタン活性・非活性
            If Me.ddlPrnSelect.ppSelectedValue <> String.Empty Then
                If Session(P_SESSION_AUTH) <> "NGC" Then
                    If Session(P_SESSION_AUTH) = "営業所" Then
                        Me.btnPrint.Enabled = True
                        Me.btnCsv.Enabled = False
                    Else
                        'Me.btnPdf.Enabled = True
                        Me.btnPrint.Enabled = True
                        If Me.ddlPrnSelect.ppSelectedValue = 6 Then
                            Me.btnCsv.Enabled = True
                        End If
                    End If
                End If

                '確認メッセージ
                Me.btnPrint.OnClientClick =
                    pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel,
                                     mfGetNamePart(Me.ddlPrnSelect.ppSelectedText))
                Me.btnCsv.OnClientClick =
                    pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel,
                                     mfGetNamePart(Me.ddlPrnSelect.ppSelectedText))
            Else
                Me.btnPrint.Enabled = False
                Me.btnCsv.Enabled = False
            End If
        End If

    End Sub

    ''' <summary>
    ''' ＰＤＦボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPdf_Click(sender As Object, e As EventArgs) Handles btnPdf.Click

        Dim objDs As DataSet = Nothing          'データセット
        Dim strSpName As String = String.Empty  'ストアドプロシージャ名

        '開始ログ出力
        psLogStart(Me)

        Select Case Me.ddlPrnSelect.ppSelectedValue
            Case 1  '返却品管理表
                strSpName = M_MY_DISP_ID + "_S3"
            Case 2  'SC・CC・RSC返却品一覧表
                strSpName = M_MY_DISP_ID + "_S4"
            Case 3  'SC・CC・RSC返却品集計表
                strSpName = M_MY_DISP_ID + "_S5"
            Case 4  '撤去品管理表兼回収依頼書
                strSpName = M_MY_DISP_ID + "_S6"
            Case 5  '未使用品管理表
                strSpName = M_MY_DISP_ID + "_S7"
            Case 6  '物品返却一覧
                strSpName = M_MY_DISP_ID + "_S8"
            Case Else
                strSpName = String.Empty
        End Select

        'Select Case Me.ddlPrnSelect.ppSelectedValue
        '    Case 3  'SC・CC・RSC返却品集計表
        '        strSpName = M_MY_DISP_ID + "_S5"
        '    Case 6  '物品返却一覧
        '        strSpName = M_MY_DISP_ID + "_S8"
        '    Case Else
        '        strSpName = String.Empty
        'End Select

        'データ取得処理
        If Not ViewState(M_VS_SEARCH) Is Nothing Then
            objDs = mfGetData(strSpName)
            If Not objDs Is Nothing Then
                If objDs.Tables(0).Rows.Count > 0 Then
                    '帳票出力処理
                    msOutputReport(objDs.Tables(0), 1)
                Else
                    psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If
            End If
        Else
            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender">d</param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click

        Dim objDs As DataSet = Nothing          'データセット
        Dim strSpName As String = String.Empty  'ストアドプロシージャ名

        '開始ログ出力
        psLogStart(Me)

        Select Case Me.ddlPrnSelect.ppSelectedValue
            Case 1  '返却品管理表
                strSpName = M_MY_DISP_ID + "_S3"
            Case 2  'SC・CC・RSC返却品一覧表
                strSpName = M_MY_DISP_ID + "_S4"
            Case 3  'SC・CC・RSC返却品集計表
                strSpName = M_MY_DISP_ID + "_S5"
            Case 4  '撤去品管理表兼回収依頼書
                strSpName = M_MY_DISP_ID + "_S6"
            Case 5  '未使用品管理表
                strSpName = M_MY_DISP_ID + "_S7"
            Case 6  '物品返却一覧
                strSpName = M_MY_DISP_ID + "_S8"
            Case Else
                strSpName = String.Empty
        End Select

        'データ取得処理
        If Not ViewState(M_VS_SEARCH) Is Nothing Then
            objDs = mfGetData(strSpName)
            If Not objDs Is Nothing Then
                If objDs.Tables(0).Rows.Count > 0 Then


                    Select Case Me.ddlPrnSelect.ppSelectedValue
                        Case 4  '撤去品管理表兼回収依頼書の時はデータセットを加工する

                            Dim strBikou(5) As String
                            Dim ds As DataSet = objDs.Clone()
                            Dim dr As DataRow
                            dr = ds.Tables(0).NewRow

                            Dim strCntlNo As String = String.Empty
                            Dim strDlvPlanDt As String = String.Empty


                            For zz As Integer = 0 To objDs.Tables(0).Rows.Count - 1

                                If zz = 0 Then
                                    strCntlNo = objDs.Tables(0).Rows(zz).Item("管理番号").ToString
                                    strDlvPlanDt = objDs.Tables(0).Rows(zz).Item("回収希望日").ToString
                                End If

                                If strCntlNo <> objDs.Tables(0).Rows(zz).Item("管理番号").ToString _
                                   Or strDlvPlanDt <> objDs.Tables(0).Rows(zz).Item("回収希望日").ToString Then

                                    dr("備考") = strBikou(0) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(1) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(2) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(3) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(4) & Environment.NewLine & Environment.NewLine

                                    ds.Tables(0).Rows.Add(dr)
                                    dr = ds.Tables(0).NewRow
                                End If

                                strCntlNo = objDs.Tables(0).Rows(zz).Item("管理番号").ToString
                                strDlvPlanDt = objDs.Tables(0).Rows(zz).Item("回収希望日").ToString

                                Select Case objDs.Tables(0).Rows(zz).Item("判定用機器種別コード").ToString
                                    Case "01" '本体
                                        dr("バージョン") = objDs.Tables(0).Rows(zz).Item("バージョン").ToString
                                        dr("機種") = objDs.Tables(0).Rows(zz).Item("機種").ToString
                                        dr("本体シリアル1") = objDs.Tables(0).Rows(zz).Item("本体シリアル1").ToString
                                        dr("本体シリアル2") = objDs.Tables(0).Rows(zz).Item("本体シリアル2").ToString
                                        dr("本体シリアル3") = objDs.Tables(0).Rows(zz).Item("本体シリアル3").ToString
                                        dr("本体シリアル4") = objDs.Tables(0).Rows(zz).Item("本体シリアル4").ToString
                                        dr("本体シリアル5") = objDs.Tables(0).Rows(zz).Item("本体シリアル5").ToString
                                        dr("本体シリアル6") = objDs.Tables(0).Rows(zz).Item("本体シリアル6").ToString
                                        dr("本体シリアル7") = objDs.Tables(0).Rows(zz).Item("本体シリアル7").ToString
                                        dr("本体シリアル8") = objDs.Tables(0).Rows(zz).Item("本体シリアル8").ToString
                                        dr("本体シリアル9") = objDs.Tables(0).Rows(zz).Item("本体シリアル9").ToString
                                        dr("本体シリアル10") = objDs.Tables(0).Rows(zz).Item("本体シリアル10").ToString
                                        dr("本体シリアル11") = objDs.Tables(0).Rows(zz).Item("本体シリアル11").ToString
                                        dr("本体シリアル12") = objDs.Tables(0).Rows(zz).Item("本体シリアル12").ToString
                                        dr("本体シリアル13") = objDs.Tables(0).Rows(zz).Item("本体シリアル13").ToString
                                        dr("本体シリアル14") = objDs.Tables(0).Rows(zz).Item("本体シリアル14").ToString
                                        dr("本体シリアル15") = objDs.Tables(0).Rows(zz).Item("本体シリアル15").ToString
                                        dr("本体シリアル16") = objDs.Tables(0).Rows(zz).Item("本体シリアル16").ToString
                                        strBikou(0) = objDs.Tables(0).Rows(zz).Item("備考").ToString

                                    Case "02" 'LCD
                                        dr("LCDシリアル1") = objDs.Tables(0).Rows(zz).Item("LCDシリアル1").ToString
                                        dr("LCDシリアル2") = objDs.Tables(0).Rows(zz).Item("LCDシリアル2").ToString
                                        dr("LCDシリアル3") = objDs.Tables(0).Rows(zz).Item("LCDシリアル3").ToString
                                        dr("LCDシリアル4") = objDs.Tables(0).Rows(zz).Item("LCDシリアル4").ToString
                                        dr("LCDシリアル5") = objDs.Tables(0).Rows(zz).Item("LCDシリアル5").ToString
                                        dr("LCDシリアル6") = objDs.Tables(0).Rows(zz).Item("LCDシリアル6").ToString
                                        dr("LCDシリアル7") = objDs.Tables(0).Rows(zz).Item("LCDシリアル7").ToString
                                        dr("LCDシリアル8") = objDs.Tables(0).Rows(zz).Item("LCDシリアル8").ToString
                                        dr("LCDシリアル9") = objDs.Tables(0).Rows(zz).Item("LCDシリアル9").ToString
                                        strBikou(1) = objDs.Tables(0).Rows(zz).Item("備考").ToString

                                    Case "04" 'プリンタ
                                        dr("型式") = objDs.Tables(0).Rows(zz).Item("型式").ToString
                                        dr("プリンタシリアル1") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル1").ToString
                                        dr("プリンタシリアル2") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル2").ToString
                                        dr("プリンタシリアル3") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル3").ToString
                                        dr("プリンタシリアル4") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル4").ToString
                                        dr("プリンタシリアル5") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル5").ToString
                                        dr("プリンタシリアル6") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル6").ToString
                                        dr("プリンタシリアル7") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル7").ToString
                                        dr("プリンタシリアル8") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル8").ToString
                                        dr("プリンタシリアル9") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル9").ToString
                                        dr("プリンタシリアル10") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル10").ToString
                                        dr("プリンタシリアル11") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル11").ToString
                                        dr("プリンタシリアル12") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル12").ToString
                                        dr("プリンタシリアル13") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル13").ToString
                                        dr("プリンタシリアル14") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル14").ToString
                                        dr("プリンタシリアル15") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル15").ToString
                                        dr("プリンタシリアル16") = objDs.Tables(0).Rows(zz).Item("プリンタシリアル16").ToString
                                        strBikou(2) = objDs.Tables(0).Rows(zz).Item("備考").ToString

                                    Case "03" 'キーボード
                                        dr("キーボードシリアル1") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル1").ToString
                                        dr("キーボードシリアル2") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル2").ToString
                                        dr("キーボードシリアル3") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル3").ToString
                                        dr("キーボードシリアル4") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル4").ToString
                                        dr("キーボードシリアル5") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル5").ToString
                                        dr("キーボードシリアル6") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル6").ToString
                                        dr("キーボードシリアル7") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル7").ToString
                                        dr("キーボードシリアル8") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル8").ToString
                                        dr("キーボードシリアル9") = objDs.Tables(0).Rows(zz).Item("キーボードシリアル9").ToString
                                        strBikou(3) = objDs.Tables(0).Rows(zz).Item("備考").ToString

                                    Case "06" 'UPS
                                        dr("UPSシリアル1") = objDs.Tables(0).Rows(zz).Item("UPSシリアル1").ToString
                                        dr("UPSシリアル2") = objDs.Tables(0).Rows(zz).Item("UPSシリアル2").ToString
                                        dr("UPSシリアル3") = objDs.Tables(0).Rows(zz).Item("UPSシリアル3").ToString
                                        dr("UPSシリアル4") = objDs.Tables(0).Rows(zz).Item("UPSシリアル4").ToString
                                        dr("UPSシリアル5") = objDs.Tables(0).Rows(zz).Item("UPSシリアル5").ToString
                                        dr("UPSシリアル6") = objDs.Tables(0).Rows(zz).Item("UPSシリアル6").ToString
                                        dr("UPSシリアル7") = objDs.Tables(0).Rows(zz).Item("UPSシリアル7").ToString
                                        dr("UPSシリアル8") = objDs.Tables(0).Rows(zz).Item("UPSシリアル8").ToString
                                        dr("UPSシリアル9") = objDs.Tables(0).Rows(zz).Item("UPSシリアル9").ToString
                                        dr("UPSシリアル10") = objDs.Tables(0).Rows(zz).Item("UPSシリアル10").ToString
                                        strBikou(4) = objDs.Tables(0).Rows(zz).Item("備考").ToString
                                End Select

                                dr("NL区分") = objDs.Tables(0).Rows(zz).Item("NL区分").ToString
                                dr("撤去種別") = objDs.Tables(0).Rows(zz).Item("撤去種別").ToString
                                dr("撤去年月日") = objDs.Tables(0).Rows(zz).Item("撤去年月日")
                                dr("工事会社名") = objDs.Tables(0).Rows(zz).Item("工事会社名").ToString
                                dr("工事部門名") = objDs.Tables(0).Rows(zz).Item("工事部門名").ToString
                                dr("TBOXID1") = objDs.Tables(0).Rows(zz).Item("TBOXID1").ToString
                                dr("TBOXID2") = objDs.Tables(0).Rows(zz).Item("TBOXID2").ToString
                                dr("TBOXID3") = objDs.Tables(0).Rows(zz).Item("TBOXID3").ToString
                                dr("TBOXID4") = objDs.Tables(0).Rows(zz).Item("TBOXID4").ToString
                                dr("TBOXID5") = objDs.Tables(0).Rows(zz).Item("TBOXID5").ToString
                                dr("TBOXID6") = objDs.Tables(0).Rows(zz).Item("TBOXID6").ToString
                                dr("TBOXID7") = objDs.Tables(0).Rows(zz).Item("TBOXID7").ToString
                                dr("TBOXID8") = objDs.Tables(0).Rows(zz).Item("TBOXID8").ToString
                                dr("移動元名") = objDs.Tables(0).Rows(zz).Item("移動元名").ToString
                                dr("回収先会社名") = objDs.Tables(0).Rows(zz).Item("回収先会社名").ToString
                                dr("回収先電話番号") = objDs.Tables(0).Rows(zz).Item("回収先電話番号").ToString
                                dr("回収先FAX番号") = objDs.Tables(0).Rows(zz).Item("回収先FAX番号").ToString
                                dr("回収先住所") = objDs.Tables(0).Rows(zz).Item("回収先住所").ToString
                                dr("回収希望日") = objDs.Tables(0).Rows(zz).Item("回収希望日")
                                dr("帳票用") = objDs.Tables(0).Rows(zz).Item("帳票用").ToString
                                dr("判定用機器種別コード") = String.Empty
                                dr("コメント1") = objDs.Tables(0).Rows(zz).Item("コメント1").ToString

                                If zz = objDs.Tables(0).Rows.Count - 1 Then

                                    dr("備考") = strBikou(0) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(1) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(2) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(3) & Environment.NewLine & Environment.NewLine & _
                                                 strBikou(4) & Environment.NewLine & Environment.NewLine

                                    ds.Tables(0).Rows.Add(dr)
                                End If

                            Next

                            '帳票出力処理
                            msOutputReport(ds.Tables(0), 2)

                        Case Else
                            '帳票出力処理
                            msOutputReport(objDs.Tables(0), 2)

                    End Select


                    ''帳票出力処理
                    'msOutputReport(objDs.Tables(0), 2)
                Else
                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                End If
            End If
        Else
            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ＣＳＶボタン　クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsv_Click(sender As Object, e As EventArgs) Handles btnCsv.Click

        Dim objDs As DataSet = Nothing          'データセット
        Dim strSpName As String = String.Empty  'ストアドプロシージャ名

        '開始ログ出力
        psLogStart(Me)

        Select Case Me.ddlPrnSelect.ppSelectedValue
            Case 6  '物品返却一覧
                strSpName = M_MY_DISP_ID + "_S8"
        End Select

        'データ取得処理
        If Not ViewState(M_VS_SEARCH) Is Nothing Then
            objDs = mfGetData(strSpName)
            If Not objDs Is Nothing Then
                If objDs.Tables(0).Rows.Count <= 0 Then

                    '0件
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "物品返却一覧")
                    Return
                End If

                '不要な列の削除
                objDs.Tables(0).Columns.Remove("会社名")
                objDs.Tables(0).Columns.Remove("部署名")
                objDs.Tables(0).Columns.Remove("電話番号")
                objDs.Tables(0).Columns.Remove("FAX番号")
                objDs.Tables(0).Columns.Remove("タイトル")
                objDs.Tables(0).Columns.Remove("ソート1")
                objDs.Tables(0).Columns.Remove("ソート2")
                objDs.Tables(0).Columns.Remove("送付元会社名")
                objDs.Tables(0).AcceptChanges()

                'CSVファイルダウンロード
                If pfDLCsvFile("物品返却一覧_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                End If

            End If
        Else
            psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    'SERLSTP001-002
    ''' <summary>
    ''' 場所区分変更時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlPlaceclass_SelectedIndexChanged()
        If txtStrageCd.ppText = String.Empty Then
            txtStrageCd.ppTextBox.Focus()
        Else
            txtStrageCd_TextChanged()
            ddlAppaDiv.Focus()
        End If
    End Sub

    ''' <summary>
    ''' 場所コード変更時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtStrageCd_TextChanged()
        Dim strbuff As String = String.Empty    '現設置／保管名称

        If Me.txtStrageCd.ppText <> String.Empty Then
            Select Case Me.ddlPlaceclass.ppSelectedValue
                Case 1  'ホール名取得
                    If mfGetHallInfo(Me.txtStrageCd.ppText, strbuff) Then
                        Me.lblStrageName.Text = strbuff
                    Else
                        Me.lblStrageName.Text = String.Empty
                    End If
                Case 2  '営業所名取得
                    If mfGetOfficeInfo(TRD_EIGYO & "," & TRD_HOSHU, Me.txtStrageCd.ppText, strbuff) Then
                        Me.lblStrageName.Text = strbuff
                    Else
                        Me.lblStrageName.Text = String.Empty
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(TRD_SONOTA, Me.txtStrageCd.ppText, strbuff) Then
                        Me.lblStrageName.Text = strbuff
                    Else
                        Me.lblStrageName.Text = String.Empty
                    End If
                Case 4  '代理店名取得
                    If mfGetOfficeInfo(TRD_DAIRI, Me.txtStrageCd.ppText, strbuff) Then
                        Me.lblStrageName.Text = strbuff
                    Else
                        Me.lblStrageName.Text = String.Empty
                    End If
                Case 5  'メーカ名取得
                    If mfGetOfficeInfo(TRD_MAKER, Me.txtStrageCd.ppText, strbuff) Then
                        Me.lblStrageName.Text = strbuff
                    Else
                        Me.lblStrageName.Text = String.Empty
                    End If
                Case Else
                    Me.lblStrageName.Text = String.Empty
            End Select
        Else
            Me.lblStrageName.Text = String.Empty
        End If
        ddlAppaDiv.Focus()
    End Sub

    ''' <summary>
    ''' データバインド時の編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        '削除シリアルの赤字表示＆選択ボタン非活性処理
        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("削除区分"), TextBox).Text = "1" Then
                For i As Integer = 0 To rowData.Cells.Count - 1
                    If TryCast((rowData.Cells(i).Controls(0)), TextBox).GetType() Is GetType(TextBox) Then
                        CType(rowData.Cells(i).Controls(0), TextBox).ForeColor = Drawing.Color.Red
                    End If
                Next
            End If
        Next
    End Sub

    'SERLSTP001-002 END

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '検索条件クリア
            msClearSearchCondition()

            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Master.ppCount = "0"
            Me.grvList.DataBind()

            '各コマンドボタン、ドロップダウンリスト非活性
            Me.ddlPrnSelect.ppDropDownList.SelectedIndex = 0
            Me.ddlPrnSelect.ppEnabled = False
            Me.btnCsv.Enabled = False
            Me.btnPdf.Enabled = False
            Me.btnPrint.Enabled = False
            Master.Master.ppLeftButton2.Enabled = False

            Me.txtSerialNo.ppTextBoxFrom.Focus()

        Catch ex As Exception
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
        Finally
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        Me.txtSerialNo.ppFromText = String.Empty
        Me.txtSerialNo.ppToText = String.Empty
        Me.ddlPlaceclass.ppDropDownList.SelectedIndex = 0
        Me.txtStrageCd.ppText = String.Empty
        Me.ddlAppaDiv.SelectedIndex = 0
        msSetddlAppacls()
        Me.txtAppaFmt.ppText = String.Empty
        Me.ddlSystem.SelectedIndex = 0
        Me.dtbMoveDt.ppText = String.Empty
        Me.ddlMoveReason.SelectedIndex = 0
        Me.txtCntlNo.ppText = String.Empty
        Me.dtbDlvPlnDt.ppText = String.Empty
        'SERLSTP001_002
        Me.lblStrageName.Text = String.Empty
        Me.ddldel.ppSelectedValue = "0"
        'SERLSTP001_002 END

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="strProc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData(ByVal strProc As String) As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetData = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(strProc, objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'シリアル番号From
                    If ViewState(M_VS_SEARCH)(1).ToString = String.Empty OrElse
                       ViewState(M_VS_SEARCH)(1).ToString = ViewState(M_VS_SEARCH)(0).ToString Then
                        'シリアル番号Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("serialno_f", SqlDbType.NVarChar,
                                         ViewState(M_VS_SEARCH)(0).ToString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        'シリアル番号Toが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("serialno_f", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(0).ToString))
                    End If

                    'シリアル番号To
                    .Add(pfSet_Param("serialno_t", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(1).ToString))

                    '場所区分
                    .Add(pfSet_Param("placeclass", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(2).ToString))

                    '現設置／保管場所
                    .Add(pfSet_Param("stragecd", SqlDbType.NVarChar,
                        ViewState(M_VS_SEARCH)(3).ToString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '機器分類
                    .Add(pfSet_Param("appadivnm", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(4).ToString))

                    '機器種別
                    .Add(pfSet_Param("appaclsnm", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(5).ToString))

                    '型式／機器
                    .Add(pfSet_Param("appanm", SqlDbType.NVarChar,
                        ViewState(M_VS_SEARCH)(6).ToString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    'システムコード
                    .Add(pfSet_Param("systemcd", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(7).ToString))

                    '移動日(yyyy/MM/dd)
                    .Add(pfSet_Param("setdt", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(8).ToString))

                    '移動理由
                    .Add(pfSet_Param("movereason", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(9).ToString))

                    '管理番号
                    .Add(pfSet_Param("serialcntlno", SqlDbType.NVarChar,
                        ViewState(M_VS_SEARCH)(10).ToString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '納入予定日
                    .Add(pfSet_Param("dlvplndt", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(11).ToString))

                    'SERLSTP001-002
                    '削除区分
                    If strProc = "SERLSTP001_S1" OrElse strProc = "SERLSTP001_S10" Then
                        .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, ddldel.ppSelectedValue))
                    End If
                    'SERLSTP001-002 END
                End With

                'データ取得
                mfGetData = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "シリアル履歴")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' データ取得処理（検索条件がシリアル番号のみの場合）
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataAllHistory() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataAllHistory = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'シリアル番号From
                    If ViewState(M_VS_SEARCH)(1).ToString = String.Empty OrElse
                       ViewState(M_VS_SEARCH)(1).ToString = ViewState(M_VS_SEARCH)(0).ToString Then
                        'シリアル番号Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("serialno_f", SqlDbType.NVarChar,
                                         ViewState(M_VS_SEARCH)(0).ToString.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        'シリアル番号Toが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("serialno_f", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(0).ToString))
                    End If

                    'シリアル番号To
                    .Add(pfSet_Param("serialno_t", SqlDbType.NVarChar, ViewState(M_VS_SEARCH)(1).ToString))
                    'SERLSTP001-002
                    '削除区分
                    .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, ddldel.ppSelectedValue))
                    'SERLSTP001-002 END

                End With

                'データ取得
                mfGetDataAllHistory = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "シリアル履歴（シリアル番号検索）")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 予備機配備状況CSVファイル作成用データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataSpareCnds() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataSpareCnds = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S9", objCn)

                'データ取得
                mfGetDataSpareCnds = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "予備機配備状況")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 検索結果CSVファイル作成用データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataSearch() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataSearch = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                'データ取得
                mfGetDataSearch = mfGetData(M_MY_DISP_ID + "_S10")

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "検索結果")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' データ表示処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msDispData(objDs As DataSet)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            '件数を設定
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

                '検索条件ビューステートを初期化
                ViewState(M_VS_SEARCH) = Nothing

                '該当件数初期化
                Master.ppCount = "0"

                '検索結果CSVを非活性
                Master.Master.ppLeftButton2.Enabled = False

            Else
                '閾値を超えた場合はメッセージを表示
                If objDs.Tables(0).Rows(0)("データ件数") > objDs.Tables(0).Rows.Count Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                        objDs.Tables(0).Rows(0)("データ件数").ToString, objDs.Tables(0).Rows.Count.ToString)
                End If
                Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString

                '検索結果CSVを活性
                Master.Master.ppLeftButton2.Enabled = True

            End If

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = objDs.Tables(0)

            '変更を反映
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後,
                     ex.ToString.Replace(Environment.NewLine, "").Replace("'", ""))
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
        End Try

    End Sub

    ''' <summary>
    ''' 帳票出力処理
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="intMode">1:ＰＤＦ出力 2:印刷</param>
    ''' <remarks></remarks>
    Private Sub msOutputReport(ByVal dt As DataTable, ByVal intMode As Integer)

        Dim rpt As Object = Nothing
        Dim strFileClass As String = String.Empty     'ファイル種別
        Dim strReportName As String = String.Empty    '帳票名
        Dim strReportTitle As String = String.Empty   '帳票タイトル
        Dim strServerAddress As String = String.Empty 'サーバアドレス
        Dim strFolderNM As String = String.Empty      '保管フォルダ
        Dim datCreateDate As DateTime = Nothing       '日付
        Dim strFileNm As String = String.Empty        'ファイル名
        Dim intRtn As Integer = 0                     '戻り値
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            strReportName = mfGetNamePart(Me.ddlPrnSelect.ppSelectedText)
            Select Case Me.ddlPrnSelect.ppSelectedValue
                Case 1  '返却品管理表
                    rpt = New CNSREP013
                Case 2  'SC・CC・RSC返却品一覧表
                    rpt = New RETREP002
                    strFileClass = "0333PN"
                    strReportTitle = strReportName
                Case 3  'SC・CC・RSC返却品集計表
                    rpt = New RETREP001
                    strFileClass = "0334PN"
                    strReportTitle = strReportName
                Case 4  '撤去品管理表兼回収依頼書
                    rpt = New SERREP004

                    '用紙サイズをA4縦に設定
                    rpt.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
                    rpt.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait

                    '上下左右の余白を設定
                    rpt.PageSettings.Margins.Top = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    rpt.PageSettings.Margins.Bottom = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    rpt.PageSettings.Margins.Left = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)
                    rpt.PageSettings.Margins.Right = GrapeCity.ActiveReports.SectionReport.CmToInch(0.5F)

                Case 5  '未使用品管理表
                    rpt = New CNSREP008
                Case 6  '物品返却一覧
                    rpt = New CNSREP006
                    strFileClass = "0336PN"
                    strReportName = dt.Rows(0).Item("タイトル")
                    strReportTitle = strReportName
                Case Else
            End Select

            '帳票出力
            If intMode = 1 Then
                'ＰＤＦ作成
                intRtn = pfPDF(strFileClass, strReportName, Nothing, rpt, dt,
                               strServerAddress, strFolderNM, datCreateDate, strFileNm, Session.SessionID)
                If intRtn <> 0 Then
                    psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strReportName)
                    Exit Sub
                End If

                'ダウンロードファイル（T07)にレコードを追加
                pfSetDwnldFile(DateTime.Now.ToString("yyyyMMdd"), strFileClass, strReportName, strFileNm,
                               strReportTitle + datCreateDate.ToString("（yyyy/MM/dd）"),
                               strServerAddress, strFolderNM,
                               datCreateDate, User.Identity.Name)
            Else
                '印刷（ＰＤＦ表示）
                psPrintPDF(Me, rpt, dt, strReportName)
            End If

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strReportName)
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
    ''' ドロップダウンリスト設定（機器分類マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppadiv()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL012", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlAppaDiv.Items.Clear()
                Me.ddlAppaDiv.DataSource = objDs.Tables(0)
                Me.ddlAppaDiv.DataTextField = "名称"
                Me.ddlAppaDiv.DataValueField = "機器分類コード"
                Me.ddlAppaDiv.DataBind()
                Me.ddlAppaDiv.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類マスタ一覧取得")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（システム）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL011", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ＴＢＯＸリスト、ＴＢＯＸ機種コードでDistinctする。（重複データをカット）
                Dim dv As DataView = objDs.Tables(0).DefaultView
                Dim dt As DataTable = dv.ToTable("ＴＢＯＸ", True, "ＴＢＯＸリスト", "ＴＢＯＸ機種コード")

                'ドロップダウンリスト設定
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = dt
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸ機種コード"
                Me.ddlSystem.DataBind()

                '先頭に空白行を追加
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing))

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸマスタ一覧取得")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器種別マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppacls()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                'SERLSTP001-002
                'objCmd = New SqlCommand("ZCMPSEL013", objCn)
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S11", objCn)
                'SERLSTP001-002 END
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, Me.ddlAppaDiv.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlAppaCls.Items.Clear()
                Me.ddlAppaCls.DataSource = objDs.Tables(0)
                Me.ddlAppaCls.DataTextField = "機器種別名"
                Me.ddlAppaCls.DataValueField = "機器種別コード"
                Me.ddlAppaCls.DataBind()
                Me.ddlAppaCls.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（移動理由）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMoveReason()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL030", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索条件
                Me.ddlMoveReason.Items.Clear()
                Me.ddlMoveReason.DataSource = objDs.Tables(0)
                Me.ddlMoveReason.DataTextField = "移動理由"
                Me.ddlMoveReason.DataValueField = "移動理由コード"
                Me.ddlMoveReason.DataBind()
                Me.ddlMoveReason.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "移動理由マスタ一覧取得")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 整合性チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckSearchCondition()

        Dim strErrCode As String = String.Empty 'エラーコード

        '保管コード
        If Me.ddlPlaceclass.ppSelectedValue = "1" Then
            '場所区分が"1:ホール"の場合、数字のみを有効とする
            strErrCode = pfCheck_TxtErr(Me.txtStrageCd.ppText, False, True, True, False, 8, String.Empty, False)
            If strErrCode <> String.Empty Then
                Me.txtStrageCd.psSet_ErrorNo(strErrCode, Me.txtStrageCd.ppName)
                Me.txtStrageCd.ppTextBox.Focus()
            End If
        Else
            '場所区分が"1:ホール"以外の場合、入力桁数は５桁以内を有効とする
            strErrCode = pfCheck_TxtErr(Me.txtStrageCd.ppText, False, False, True, False, 5, String.Empty, False)
            If strErrCode <> String.Empty Then
                Me.txtStrageCd.psSet_ErrorNo(strErrCode, Me.txtStrageCd.ppName, "5")
                Me.txtStrageCd.ppTextBox.Focus()
            End If
        End If

    End Sub

    ''' <summary>
    ''' 検索条件をビューステートに保持し、検索区分を返す
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msRetentionSearchCondition() As Integer

        Dim strList As List(Of String) = Nothing
        Dim intSearchCls As Integer = Nothing   '検索区分（ 1:最新シリアル履歴、2:全シリアル履歴 ）

        strList = New List(Of String)

        strList.Add(Me.txtSerialNo.ppFromText)        ' 0.シリアル番号From
        strList.Add(Me.txtSerialNo.ppToText)          ' 1.シリアル番号To
        strList.Add(Me.ddlPlaceclass.ppSelectedValue) ' 2.場所区分
        strList.Add(Me.txtStrageCd.ppText)            ' 3.現設置／保管場所
        If ddlAppaDiv.SelectedValue = String.Empty Then
            strList.Add(mfGETCodePart(ddlAppaCls.SelectedValue, 0))      ' 5.機器種別
        Else
            strList.Add(Me.ddlAppaDiv.SelectedValue)      ' 4.機器分類
        End If
        strList.Add(mfGETCodePart(ddlAppaCls.SelectedValue, 1))      ' 5.機器種別
        strList.Add(Me.txtAppaFmt.ppText)             ' 6.型式／機器
        strList.Add(Me.ddlSystem.SelectedValue)       ' 7.システム
        strList.Add(Me.dtbMoveDt.ppText)              ' 8.移動日
        strList.Add(Me.ddlMoveReason.SelectedValue)   ' 9.移動理由
        strList.Add(Me.txtCntlNo.ppText)              '10.管理番号
        strList.Add(Me.dtbDlvPlnDt.ppText)            '11.納入予定日

        If (strList(0).ToString.Trim <> String.Empty OrElse strList(1).ToString.Trim <> String.Empty) AndAlso
            strList(2).ToString.Trim = String.Empty AndAlso
            strList(3).ToString.Trim = String.Empty AndAlso
            strList(4).ToString.Trim = String.Empty AndAlso
            strList(5).ToString.Trim = String.Empty AndAlso
            strList(6).ToString.Trim = String.Empty AndAlso
            strList(7).ToString.Trim = String.Empty AndAlso
            strList(8).ToString.Trim = String.Empty AndAlso
            strList(9).ToString.Trim = String.Empty AndAlso
            strList(10).ToString.Trim = String.Empty AndAlso
            strList(11).ToString.Trim = String.Empty Then
            intSearchCls = 2
        Else
            intSearchCls = 1
        End If

        ViewState(M_VS_SEARCH) = strList.ToArray

        Return intSearchCls

    End Function

    ''' <summary>
    ''' ドロップダウンリストの名称部分を取得　※コロン（：）で区切られた文字列の２番目を返す
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNamePart(ByVal strText As String) As String

        Dim strArray As String() = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If strText = String.Empty Then
                Return strText
            End If

            strArray = strText.Split(":")

            Return strArray(1)

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
            Return String.Empty

        End Try

    End Function

    'SERLSTP001-002
    ''' <summary>
    ''' ホール情報取得処理
    ''' </summary>
    ''' <param name="strTboxid"></param>
    ''' <param name="strHallName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHallInfo(ByVal strTboxid As String, ByRef strHallName As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow
        objStack = New StackFrame

        mfGetHallInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL028", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strTboxid))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ホール名設定
                strHallName = dtRow("ホール名").ToString

                mfGetHallInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報取得")
                'ログ出力
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

    ''' <summary>
    ''' 会社・営業所情報取得処理
    ''' </summary>
    ''' <param name="strTraderCd"></param>
    ''' <param name="strOfficeCd"></param>
    ''' <param name="strOfficeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetOfficeInfo(ByVal strTraderCd As String, ByVal strOfficeCd As String, ByRef strOfficeName As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow
        objStack = New StackFrame

        mfGetOfficeInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL029", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '業者コード
                    .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, strTraderCd))
                    '営業所コード
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, strOfficeCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                '会社名または営業所名設定
                If dtRow("業者コード").ToString = TRD_EIGYO OrElse
                   dtRow("業者コード").ToString = TRD_HOSHU OrElse
                   dtRow("業者コード").ToString = TRD_DAIRI Then
                    strOfficeName = dtRow("営業所名").ToString
                Else
                    strOfficeName = dtRow("会社名").ToString
                End If

                mfGetOfficeInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "会社・営業所情報取得")
                'ログ出力
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

    ''' <summary>
    ''' コロン区切りの文字を取得
    ''' </summary>
    ''' <param name="strText">コロン(：)込み文字列</param>
    ''' <param name="idx">区切られた内何番目を返すか指定する</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGETCodePart(ByVal strText As String, ByVal idx As Integer) As String

        Dim strArray As String() = Nothing

        objStack = New StackFrame

        Try
            If strText = String.Empty Then
                Return strText
            End If

            strArray = strText.Split(":")

            Return strArray(idx)

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return String.Empty

        End Try

    End Function

    ''' <summary>
    ''' 未削除データの件数取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetRecordCount() As Integer
        Dim intRtn As Integer = 0

        For Each dr As GridViewRow In Me.grvList.Rows

            If DirectCast(dr.FindControl("削除区分"), TextBox).Text.Trim = "0" Then
                intRtn += 1
            End If
        Next

        Return intRtn
    End Function
    'SERLSTP001-002 END

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
