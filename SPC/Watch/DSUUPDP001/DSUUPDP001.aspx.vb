'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ＤＳＵ交換対応依頼書　参照更新
'*　ＰＧＭＩＤ：　DSUUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　.02.18　：　ＸＸＸ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive      '排他制御用

#End Region

Public Class DSUUPDP001
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
    Const M_MY_DISP_ID = P_FUN_DSU & P_SCR_UPD & P_PAGE & "001"

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
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''ボタンアクションの設定
        AddHandler Master.ppRigthButton2.Click, AddressOf btnDelete_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btnUpdate_Click
        AddHandler Master.ppRigthButton4.Click, AddressOf btnClear_Click
        AddHandler Master.ppLeftButton1.Click, AddressOf btnPrint_Click
        AddHandler Master.ppLeftButton2.Click, AddressOf btnPDF_Click
        AddHandler Master.ppLeftButton3.Click, AddressOf btnCSV_Click
        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf txtTBoxId_TextChanged
        txtTboxId.ppTextBox.AutoPostBack = True
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示のみ

                'ボタンを活性化
                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton1.Text = "戻る"
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton2.Text = P_BTN_NM_DEL
                Master.ppRigthButton3.Visible = True
                Master.ppRigthButton3.Text = P_BTN_NM_UPD
                Master.ppRigthButton4.Visible = True
                Master.ppRigthButton4.Text = "入力内容クリア"
                Master.ppLeftButton1.Visible = True
                Master.ppLeftButton1.Text = P_BTN_NM_PRI
                'Master.ppLeftButton2.Visible = True
                'Master.ppLeftButton2.Text = P_BTN_NM_PDF
                Master.ppLeftButton2.Visible = False
                Master.ppLeftButton2.Text = P_BTN_NM_PDF
                Master.ppLeftButton3.Visible = True
                Master.ppLeftButton3.Text = "ＣＳＶ"

                '「戻る」ボタン設定
                Master.ppRigthButton1.OnClientClick = "return window_close('" & pfGet_Mes("00001", ClsComVer.E_Mタイプ.警告) & "'" &
                        ",'" & pfGet_MesType(ClsComVer.E_Mタイプ.警告) & "00001')"

                '入力内容クリアボタンと戻るボタン押下時の検証を無効
                Master.ppRigthButton1.CausesValidation = False
                Master.ppRigthButton4.CausesValidation = False

                '画面設定
                Master.ppProgramID = M_MY_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)
                Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                'セッション変数「グループナンバー」「遷移条件」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Or Session(P_SESSION_TERMS) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「遷移条件」「キー情報」「グループナンバー」を保存
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                ViewState(P_KEY) = Session(P_KEY)
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                '--2014/01/15 中川　ここから
                '排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                End If
                '--2014/01/15 中川　ここまで

                'ドロップダウンリスト設定
                msSet_ddlStatus()

                '画面クリア
                msAllClear_Screen()

                If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.登録 Then
                    'データ取得
                    msGet_Data()
                End If

                '活性／非活性設定
                msSet_Mode(ViewState(P_SESSION_TERMS))

            Else
                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Return
                End If

            End If

        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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
    ''' 印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)
        Dim objRpt As DSUREP001                     'レポートクラス
        Dim dtsDSUData As DataSet = Nothing         'データセット
        Dim dttData As DataTable = Nothing          'データテーブル

        'ログ出力開始
        psLogStart(Me)

        If mfGet_DSUInfo(Me.lblGcReportNo2.Text, dtsDSUData) Then
            'ＰＤＦデータ取得処理
            dttData = dtsDSUData.Tables(0)
            objRpt = New DSUREP001

            'ファイル出力
            psPrintPDF(Me, objRpt, dttData, "ＤＳＵ交換報告書")
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ＰＤＦボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPDF_Click(sender As Object, e As EventArgs)
        Dim strServerAddress As String = Nothing        'サーバアドレス
        Dim strFolderNM As String = Nothing             '保管フォルダ
        Dim datCreateDate As DateTime = Nothing         '日付
        Dim strFileNm As String = Nothing               'ファイル名
        Dim intRtn As Integer                           '戻り値
        Dim dtsDSUData As DataSet = Nothing             'データセット
        Dim dttData As DataTable = Nothing              'データテーブル
        Dim objRpt As DSUREP001                         'レポートクラス

        'ログ出力開始
        psLogStart(Me)

        If mfGet_DSUInfo(Me.lblGcReportNo2.Text, dtsDSUData) Then
            'データをテーブルに設定
            dttData = dtsDSUData.Tables(0)
            objRpt = New DSUREP001

            'ファイル出力
            intRtn = pfPDF("0621AT" _
                         , "ＤＳＵ交換報告書" _
                         , Me.lblGcReportNo2.Text _
                         , objRpt _
                         , dttData _
                         , strServerAddress _
                         , strFolderNM _
                         , datCreateDate _
                         , strFileNm)

            'ファイル出力エラー
            If intRtn <> 0 Then
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告書")
                Exit Sub
            End If

            'ダウンロードファイル（T07)にレコードを追加
            intRtn = pfSetDwnldFile(Me.lblGcReportNo2.Text _
                                  , "0621AT" _
                                  , "ＤＳＵ交換対応依頼書　参照更新" _
                                  , strFileNm _
                                  , "ＤＳＵ交換報告書" _
                                  , strServerAddress _
                                  , strFolderNM _
                                  , datCreateDate _
                                  , User.Identity.Name)

            'ダウンロードファイル（T07)にレコードを追加エラー
            If intRtn <> 0 Then
                psMesBox(Me, "10002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告書")
                Exit Sub
            End If
        End If

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' ＣＳＶボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCSV_Click(sender As Object, e As EventArgs)
        Dim intRtn As Integer                       '戻り値
        Dim dtsDSUData As DataSet = Nothing         'データセット
        Dim dttData As DataTable = Nothing          'データテーブル

        'ログ出力開始
        psLogStart(Me)

        Try

            If mfGet_DSUInfo(Me.lblGcReportNo2.Text, dtsDSUData) Then
                'ＣＳＶデータ取得処理
                dttData = dtsDSUData.Tables(0)

                'ファイル出力
                intRtn = pfDLCSV("ＤＳＵ交換報告書", Me.lblGcReportNo2.Text, dttData, True, Me)

                'ファイル出力エラー
                If intRtn <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "ＣＳＶファイル")
                    Exit Sub
                End If
            End If

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 入力内容クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        Select Case ViewState(P_SESSION_TERMS)
            Case ClsComVer.E_遷移条件.登録
                msAllClear_Screen()
            Case ClsComVer.E_遷移条件.更新
                msClear_Screen()
        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 更新ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        '個別整合性チェック
        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
            msCheck_Error()
        End If

        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim dstOrders As DataSet = Nothing          'データセット
            Dim intRtn As Integer
            Dim strMes1 As String = Nothing
            Dim strMes2 As String = Nothing
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            ''個別整合性チェック
            'If ViewState(P_SESSION_TERMS) =  ClsComVer.E_遷移条件.登録 Then
            '    msCheck_Error()
            'End If

            Select Case ViewState(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.登録
                    strMes1 = "00003"
                    strMes2 = "00008"
                Case ClsComVer.E_遷移条件.更新
                    strMes1 = "00001"
                    strMes2 = "00007"
            End Select

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                        cmdDB = New SqlCommand(M_MY_DISP_ID + "_U1", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                        'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("gc_report_no", SqlDbType.NVarChar, Me.lblGcReportNo2.Text))               'ＧＣ報告ＮＯ
                            .Add(pfSet_Param("torable_content", SqlDbType.NVarChar, Me.txtTorableContent.ppText))       '障害内容
                            .Add(pfSet_Param("line_cnd", SqlDbType.NVarChar, Me.txtLineCnd.ppText))                     '回線状態
                            .Add(pfSet_Param("cause", SqlDbType.NVarChar, Me.txtCause.ppText))                          '原因
                            .Add(pfSet_Param("rspns_content", SqlDbType.NVarChar, Me.txtRspnsContent.ppText))           '対応処置内容
                            .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))              '進捗状況
                            .Add(pfSet_Param("rspns_d", SqlDbType.NVarChar, Me.dttRspnsD.ppText))                       '対応日
                            .Add(pfSet_Param("ntt_branch", SqlDbType.NVarChar, Me.txtNttBranch.ppText))                 '対応ＮＴＴ支社
                            .Add(pfSet_Param("n_zipno", SqlDbType.NVarChar, Me.txtNZipNo.ppText))                       'ＮＴＴ郵便番号
                            .Add(pfSet_Param("n_addr", SqlDbType.NVarChar, Me.txtNAddr.ppText))                         'ＮＴＴ住所
                            .Add(pfSet_Param("n_telno", SqlDbType.NVarChar, Me.txtNTelNo.ppText))                       'ＮＴＴＴＥＬ
                            .Add(pfSet_Param("n_faxno", SqlDbType.NVarChar, Me.txtNFaxNo.ppText))                       'ＮＴＴＦＡＸ
                            .Add(pfSet_Param("n_charge", SqlDbType.NVarChar, Me.txtNCharge.ppText))                     'ＮＴＴ担当者
                            .Add(pfSet_Param("n_charge_telno", SqlDbType.NVarChar, Me.txtNChargeTelNo.ppText))          'ＮＴＴ担当者電話番号
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                            .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNoteText.ppText))                    '備考
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                        End With
                    Else
                        cmdDB = New SqlCommand(M_MY_DISP_ID + "_I1", conDB)
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                        'ＴＢＯＸＩＤ
                            .Add(pfSet_Param("torable_content", SqlDbType.NVarChar, Me.txtTorableContent.ppText))       '障害内容
                            .Add(pfSet_Param("line_cnd", SqlDbType.NVarChar, Me.txtLineCnd.ppText))                     '回線状態
                            .Add(pfSet_Param("cause", SqlDbType.NVarChar, Me.txtCause.ppText))                          '原因
                            .Add(pfSet_Param("rspns_content", SqlDbType.NVarChar, Me.txtRspnsContent.ppText))           '対応処置内容
                            .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))              '進捗状況
                            .Add(pfSet_Param("rspns_d", SqlDbType.NVarChar, Me.dttRspnsD.ppText))                       '対応日
                            .Add(pfSet_Param("ntt_branch", SqlDbType.NVarChar, Me.txtNttBranch.ppText))                 '対応ＮＴＴ支社
                            .Add(pfSet_Param("n_zipno", SqlDbType.NVarChar, Me.txtNZipNo.ppText))                       'ＮＴＴ郵便番号
                            .Add(pfSet_Param("n_addr", SqlDbType.NVarChar, Me.txtNAddr.ppText))                         'ＮＴＴ住所
                            .Add(pfSet_Param("n_telno", SqlDbType.NVarChar, Me.txtNTelNo.ppText))                       'ＮＴＴＴＥＬ
                            .Add(pfSet_Param("n_faxno", SqlDbType.NVarChar, Me.txtNFaxNo.ppText))                       'ＮＴＴＦＡＸ
                            .Add(pfSet_Param("n_charge", SqlDbType.NVarChar, Me.txtNCharge.ppText))                     'ＮＴＴ担当者
                            .Add(pfSet_Param("n_charge_telno", SqlDbType.NVarChar, Me.txtNChargeTelNo.ppText))          'ＮＴＴ担当者電話番号
                            .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtNoteText.ppText))                    '備考
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                        End With
                    End If

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        Select Case ViewState(P_SESSION_TERMS)
                            Case ClsComVer.E_遷移条件.登録
                                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                            Case ClsComVer.E_遷移条件.更新
                                'コマンドタイプ設定(ストアド)
                                cmdDB.CommandType = CommandType.StoredProcedure
                                cmdDB.ExecuteNonQuery()
                        End Select

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, strMes2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using

                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then

                        'ＧＣ報告ＮＯを表示
                        Me.lblGcReportNo2.Text = dstOrders.Tables(0).Rows(0).Item("ＧＣ報告ＮＯ").ToString


                        '--2014/04/15 中川　ここから
                        '排他制御用変数
                        Dim strExclusiveDate As String = Nothing
                        Dim arTable_Name As New ArrayList
                        Dim arKey As New ArrayList

                        'ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D35_DSUREPLC")

                        'ロックテーブルキー項目の登録
                        arKey.Insert(0, dstOrders.Tables(0).Rows(0).Item("ＧＣ報告ＮＯ").ToString)

                        '排他情報確認処理
                        If clsExc.pfSel_Exclusive(strExclusiveDate,
                                           Me,
                                           Session(P_SESSION_IP),
                                           Session(P_SESSION_PLACE),
                                           Session(P_SESSION_USERID),
                                           Session(P_SESSION_SESSTION_ID),
                                           ViewState(P_SESSION_GROUP_NUM),
                                           M_MY_DISP_ID,
                                           arTable_Name,
                                           arKey) = 0 Then

                            'セッション情報を設定
                            Me.Master.ppExclusiveDate = strExclusiveDate

                        End If
                        '--2014/04/15 中川　ここまで

                        '更新画面に変更
                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                        Master.ppRigthButton3.Text = P_BTN_NM_UPD
                        Master.ppRigthButton2.Enabled = True
                        Master.ppLeftButton1.Enabled = True
                        Master.ppLeftButton2.Enabled = True
                        Master.ppLeftButton3.Enabled = True

                        '活性／非活性設定
                        msSet_Mode(ViewState(P_SESSION_TERMS))

                    End If

                    '完了メッセージ
                    psMesBox(Me, strMes1, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告")

                Catch ex As Exception
                    'データ登録／更新処理エラー
                    psMesBox(Me, strMes1, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告")
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
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        '開始ログ出力
        psLogStart(Me)

        If (Page.IsValid) Then
            Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
            Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
            Dim intRtn As Integer
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
                    cmdDB = New SqlCommand(M_MY_DISP_ID + "_D1", conDB)
                    'パラメータ設定
                    With cmdDB.Parameters
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("gc_report_no", SqlDbType.NVarChar, Me.lblGcReportNo2.Text))               'ＧＣ報告ＮＯ
                        .Add(pfSet_Param("torable_content", SqlDbType.NVarChar, Me.txtTorableContent.ppText))       '障害内容
                        .Add(pfSet_Param("line_cnd", SqlDbType.NVarChar, Me.txtLineCnd.ppText))                     '回線状態
                        .Add(pfSet_Param("cause", SqlDbType.NVarChar, Me.txtCause.ppText))                          '原因
                        .Add(pfSet_Param("rspns_content", SqlDbType.NVarChar, Me.txtRspnsContent.ppText))           '対応処置内容
                        .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, Me.ddlStatus.SelectedValue))              '進捗状況
                        .Add(pfSet_Param("rspns_d", SqlDbType.NVarChar, Me.dttRspnsD.ppText))                       '対応日
                        .Add(pfSet_Param("ntt_branch", SqlDbType.NVarChar, Me.txtNttBranch.ppText))                 '対応ＮＴＴ支社
                        .Add(pfSet_Param("n_zipno", SqlDbType.NVarChar, Me.txtNZipNo.ppText))                       'ＮＴＴ郵便番号
                        .Add(pfSet_Param("n_addr", SqlDbType.NVarChar, Me.txtNAddr.ppText))                         'ＮＴＴ住所
                        .Add(pfSet_Param("n_telno", SqlDbType.NVarChar, Me.txtNTelNo.ppText))                       'ＮＴＴＴＥＬ
                        .Add(pfSet_Param("n_faxno", SqlDbType.NVarChar, Me.txtNFaxNo.ppText))                       'ＮＴＴＦＡＸ
                        .Add(pfSet_Param("n_charge", SqlDbType.NVarChar, Me.txtNCharge.ppText))                     'ＮＴＴ担当者
                        .Add(pfSet_Param("n_charge_telno", SqlDbType.NVarChar, Me.txtNChargeTelNo.ppText))          'ＮＴＴ担当者ＴＥＬ
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                         'ユーザーＩＤ
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                    End With

                    'データ更新
                    Using conTrn = conDB.BeginTransaction
                        cmdDB.Transaction = conTrn

                        'コマンドタイプ設定(ストアド)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, intRtn.ToString)
                            Exit Sub
                        End If

                        'コミット
                        conTrn.Commit()

                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                        '活性／非活性設定
                        msSet_Mode(ViewState(P_SESSION_TERMS))

                    End Using

                    '完了メッセージ
                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告")

                Catch ex As Exception
                    'データ削除処理エラー
                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告")
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
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理(全部)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAllClear_Screen()

        Me.txtTboxId.ppText = String.Empty              'ＴＢＯＸＩＤ
        Me.lblGcReportNo2.Text = String.Empty           'ＧＣ報告ＮＯ
        Me.lblNlCls2.Text = String.Empty                'ＮＬ区分
        Me.lblHallNm2.Text = String.Empty               'ホール名
        Me.txtTorableContent.ppText = String.Empty      '障害内容
        Me.lblHZipNo2.Text = String.Empty               '郵便番号
        Me.lblHAddr2.Text = String.Empty                '住所
        Me.lblTboxTelNo2.Text = String.Empty            'ＴＢＯＸ呼出番号
        Me.txtLineCnd.ppText = String.Empty             '回線状態
        Me.txtCause.ppText = String.Empty               '原因
        Me.txtRspnsContent.ppText = String.Empty        '対応処置内容
        Me.ddlStatus.SelectedValue = String.Empty       '進捗状況
        Me.dttRspnsD.ppText = String.Empty              '対応日
        Me.txtNttBranch.ppText = String.Empty           '対応ＮＴＴ支社
        Me.txtNZipNo.ppText = String.Empty              'ＮＴＴ郵便番号
        Me.txtNAddr.ppText = String.Empty               'ＮＴＴ住所
        Me.txtNTelNo.ppText = String.Empty              'ＮＴＴＴＥＬ
        Me.txtNFaxNo.ppText = String.Empty              'ＮＴＴＦＡＸ
        Me.txtNCharge.ppText = String.Empty             'ＮＴＴ担当者
        Me.txtNChargeTelNo.ppText = String.Empty        'ＮＴＴ担当者ＴＥＬ
        Me.txtNoteText.ppText = String.Empty            '備考
        Me.txtTboxId.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' 画面クリア処理(更新時入力可項目)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Screen()

        Me.txtTorableContent.ppText = String.Empty      '障害内容
        Me.txtLineCnd.ppText = String.Empty             '回線状態
        Me.txtCause.ppText = String.Empty               '原因
        Me.txtRspnsContent.ppText = String.Empty        '対応処置内容
        Me.ddlStatus.SelectedValue = String.Empty       '進捗状況
        Me.dttRspnsD.ppText = String.Empty              '対応日
        Me.txtNttBranch.ppText = String.Empty           '対応ＮＴＴ支社
        Me.txtNZipNo.ppText = String.Empty              'ＮＴＴ郵便番号
        Me.txtNAddr.ppText = String.Empty               'ＮＴＴ住所
        Me.txtNTelNo.ppText = String.Empty              'ＮＴＴＴＥＬ
        Me.txtNFaxNo.ppText = String.Empty              'ＮＴＴＦＡＸ
        Me.txtNCharge.ppText = String.Empty             'ＮＴＴ担当者
        Me.txtNChargeTelNo.ppText = String.Empty        'ＮＴＴ担当者ＴＥＬ
        Me.txtTorableContent.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing              'DataRow
        Dim strKey() As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'キー項目取得
        strKey = ViewState(P_KEY)

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S1", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("gc_report_no", SqlDbType.NVarChar, strKey(0).ToString))       'ＧＣ報告ＮＯ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strKey(1).ToString))             'ＴＢＯＸＩＤ
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ表示
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告")
                Else
                    dtRow = dstOrders.Tables(0).Rows(0)

                    Me.txtTboxId.ppText = strKey(1).ToString
                    Me.lblGcReportNo2.Text = strKey(0).ToString
                    Me.lblNlCls2.Text = dtRow("ＮＬ区分").ToString
                    Me.lblHallNm2.Text = dtRow("ホール名").ToString
                    Me.txtTorableContent.ppText = dtRow("障害内容").ToString
                    Me.lblHZipNo2.Text = dtRow("郵便番号").ToString
                    Me.lblHAddr2.Text = dtRow("住所").ToString
                    Me.lblTboxTelNo2.Text = dtRow("ＴＢＯＸ呼出番号").ToString
                    Me.txtLineCnd.ppText = dtRow("回線状態").ToString
                    Me.txtCause.ppText = dtRow("原因").ToString
                    Me.txtRspnsContent.ppText = dtRow("対応処置内容").ToString
                    Me.ddlStatus.SelectedValue = dtRow("進捗状況").ToString
                    Me.dttRspnsD.ppText = dtRow("対応日").ToString
                    Me.txtNttBranch.ppText = dtRow("対応ＮＴＴ支社").ToString
                    Me.txtNZipNo.ppText = dtRow("ＮＴＴ郵便番号").ToString
                    Me.txtNAddr.ppText = dtRow("ＮＴＴ住所").ToString
                    Me.txtNTelNo.ppText = dtRow("ＮＴＴＴＥＬ").ToString
                    Me.txtNFaxNo.ppText = dtRow("ＮＴＴＦＡＸ").ToString
                    Me.txtNCharge.ppText = dtRow("ＮＴＴ担当者").ToString
                    Me.txtNChargeTelNo.ppText = dtRow("ＮＴＴ担当者ＴＥＬ").ToString
                    Me.txtNoteText.ppText = dtRow("備考").ToString

                End If

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告書")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Sub

    ''' <summary>
    ''' 遷移条件によって活性／非活性を設定する。
    ''' </summary>
    ''' <param name="ipshtMode"></param>
    ''' <remarks></remarks>
    Private Sub msSet_Mode(ByVal ipshtMode As ClsComVer.E_遷移条件)
        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.参照
                '非活性設定
                pnlDsuRequestForm.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = False
                '確認処理付与
                Master.ppLeftButton1.OnClientClick =
                    pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")
                Master.ppLeftButton2.OnClientClick =
                    pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")
                Master.ppLeftButton3.OnClientClick =
                    pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")

            Case ClsComVer.E_遷移条件.更新
                Me.txtTboxId.ppEnabled = False
                'Me.txtNoteText.ppEnabled = False
                Me.txtTorableContent.ppTextBox.Focus()
                '確認処理付与
                Master.ppLeftButton1.OnClientClick =
                    pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")
                Master.ppLeftButton2.OnClientClick =
                    pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")
                Master.ppLeftButton3.OnClientClick =
                    pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")
                Master.ppRigthButton3.OnClientClick =
                    pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")
                Master.ppRigthButton2.OnClientClick =
                    pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")

            Case ClsComVer.E_遷移条件.登録
                Master.ppRigthButton3.Text = P_BTN_NM_ADD
                Me.txtTboxId.ppTextBox.Focus()
                Master.ppRigthButton2.Enabled = False
                Master.ppLeftButton1.Enabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppLeftButton3.Enabled = False
                '確認処理付与
                Master.ppRigthButton3.OnClientClick =
                    pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＤＳＵ交換報告書")

        End Select
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗状況）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ddlStatus()
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("ZCMPSEL015", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "62"))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlStatus.Items.Clear()
                Me.ddlStatus.DataSource = dstOrders.Tables(0)
                Me.ddlStatus.DataTextField = "進捗ステータス名"
                Me.ddlStatus.DataValueField = "進捗ステータス"
                Me.ddlStatus.DataBind()
                Me.ddlStatus.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ一覧取得")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

    ''' <summary>
    ''' ＤＳＵ交換報告書情報取得処理
    ''' </summary>
    ''' <param name="ipstrGcReportNo">ＧＣ報告ＮＯ番号</param>
    ''' <param name="opdstData">ＧＣ報告ＮＯ項目</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_DSUInfo(ByVal ipstrGcReportNo As String, ByRef opdstData As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_DSUInfo = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S2", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＧＣ報告ＮＯ
                    .Add(pfSet_Param("gc_report_no", SqlDbType.NVarChar, ipstrGcReportNo))
                End With

                'データ取得
                opdstData = clsDataConnect.pfGet_DataSet(cmdDB)

                If opdstData.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告")
                    Exit Function
                End If

                mfGet_DSUInfo = True

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＤＳＵ交換報告書情報")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

    ''' <summary>
    ''' ＴＢＯＸＩＤ、整合性チェック
    ''' </summary>
    ''' <param name="ipstrTboxId">ＴＢＯＸＩＤ</param>
    ''' <param name="opdstTboxId">ＴＢＯＸＩＤ情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_TboxId(ByVal ipstrTboxId As String, ByRef opdstTboxId As DataSet) As Boolean
        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGet_TboxId = False

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_MY_DISP_ID + "_S3", conDB)
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, ipstrTboxId))
                End With

                'データ取得
                opdstTboxId = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If opdstTboxId.Tables(0).Rows.Count = 0 Then
                    Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                    Exit Function
                End If

                mfGet_TboxId = True

            Catch ex As Exception
                'データ取得処理エラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種コード")
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
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If
    End Function

    ''' <summary>
    ''' 入力項目のエラーチェックを行う。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()
        Dim dtsTboxId As DataSet = Nothing

        'ＴＢＯＸＩＤ情報取得
        If mfGet_TboxId(Me.txtTboxId.ppText, dtsTboxId) Then
            With dtsTboxId.Tables(0).Rows(0)
                Me.lblNlCls2.Text = .Item("ＮＬ区分").ToString
                Me.lblHallNm2.Text = .Item("ホール名").ToString
                Me.lblHZipNo2.Text = .Item("郵便番号").ToString
                Me.lblHAddr2.Text = .Item("住所").ToString
                Me.lblTboxTelNo2.Text = .Item("ＴＢＯＸ呼出番号").ToString
            End With
        Else
            Me.txtTboxId.ppText = String.Empty
            Me.txtTboxId.ppTextBox.Focus()

        End If
    End Sub
    '----20140731   武   from
    ''' <summary>
    ''' TBOXIDロストフォーカス時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTBoxId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            psLogStart(Me)

            Dim conDB As SqlConnection = Nothing
            Dim cmdDB As SqlCommand = Nothing
            Dim dtsTboxId As DataSet = Nothing

            'TBOXID数値チェック
            If pfCheck_Num(txtTboxId.ppText) = False Then
                txtTboxId.psSet_ErrorNo("4002", "ＴＢＯＸＩＤ")
            End If

            If mfGet_TboxId(Me.txtTboxId.ppText, dtsTboxId) Then
                With dtsTboxId.Tables(0).Rows(0)
                    Me.lblNlCls2.Text = .Item("ＮＬ区分").ToString
                    Me.lblHallNm2.Text = .Item("ホール名").ToString
                    Me.lblHZipNo2.Text = .Item("郵便番号").ToString
                    Me.lblHAddr2.Text = .Item("住所").ToString
                    Me.lblTboxTelNo2.Text = .Item("ＴＢＯＸ呼出番号").ToString
                End With
            Else
                Me.txtTboxId.ppTextBox.Focus()
            End If

        Catch ex As Exception
            'データ取得処理エラー
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種コード")
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
    '----20140731   武   to
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
