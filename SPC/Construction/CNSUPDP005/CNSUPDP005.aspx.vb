'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　請求資料　状況更新
'*　ＰＧＭＩＤ：　CNSUPDP005
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.25　：　高松
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
Imports DBFTP.ClsDBFTP_Main
Imports DBFTP.ClsFTPCnfg
'Imports DBFTP.clsLogwrite
Imports DBFTP.ClsSQLSvrDB
Imports System.Security.AccessControl

#End Region

Public Class CNSUPDP005
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
    Const M_DISP_ID = P_FUN_CNS & P_SCR_UPD & P_PAGE & "005"    'プログラムID
    Const M_FILE_TYPE9 = "9"                                    '工事資料

    Const M_VIEW_SEARCH = "検索"                                '検索項目保管用ビューステート
    Const M_VIEW_UPDATE = "更新"                                '登録更新保管用ビューステート

    Const M_VIEW_INDEX = "選択行"                               '選択行のINDEX

    Const M_GYOSYA_PATH = "~/Common/COMSELP002/COMSELP002.aspx"

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
    Dim clsExc As New ClsCMExclusive

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

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            'ボタンクリックイベ ント設定
            AddHandler Master.ppRigthButton1.Click, AddressOf btn_Click       '検索
            AddHandler Master.ppRigthButton2.Click, AddressOf btn_Click       '検索クリア

            '工事依頼NOが変更された場合
            'AddHandler Me.txtConstructionRequestNo.ppTextBox.TextChanged, AddressOf txtCnst_noChanged
            'Me.txtConstructionRequestNo.ppTextBox.AutoPostBack = True

            '工事依頼番号が変更された場合
            AddHandler Me.txtKoziirai_IO.ppTextBox.TextChanged, AddressOf ms_serch_KojiNum
            AddHandler Me.txtKoziirai_IO.ppTextBox.TextChanged, AddressOf txtCnst_noChanged
            Me.txtKoziirai_IO.ppTextBox.AutoPostBack = True

            'ホール名を設定した場合
            AddHandler Me.txtHoleNm.ppTextBox.TextChanged, AddressOf txtHoleNm_TextChanged
            Me.txtHoleNm.ppTextBox.AutoPostBack = True

            '請求先を設定した場合
            AddHandler Me.trdBilling.ppTextBox.TextChanged, AddressOf trdBilling_TextChanged
            AddHandler Me.txtSeikyusakiCD.ppTextBox.TextChanged, AddressOf txtSeikyusakiCD_TextChanged
            Me.trdBilling.ppTextBox.AutoPostBack = True
            Me.txtSeikyusakiCD.ppTextBox.AutoPostBack = True
            '--------------------------------
            '2014/05/30 後藤　ここから
            '--------------------------------
            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
            '--------------------------------
            '2014/05/30 後藤　ここまで
            '--------------------------------

            If Not IsPostBack Then  '初回表示

                Dim strUser(2) As String        '0:ユーザ権限     1: 起動場所
                Dim strKeys() As String         '依頼番号/TBOXID/ホールコード

                '開始ログ出力
                psLogStart(Me)

                'セッションの存在確認
                If Session(P_SESSION_BCLIST) = Nothing Then

                    Throw New Exception

                End If

                'ユーザの権限、場所を取得
                strUser(0) = Session(P_SESSION_BCLIST)
                strUser(1) = Session(P_SESSION_BCLIST)

                'キー情報取得
                strKeys = Session(P_KEY)



                '権限、場所をビューステートに格納
                'Me.ViewState.Add(P_KENGEN, strUser)

                '画面名を表示
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                    Me.Master.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
                End If

                'コントロールの初期化
                msClearScreen()

                'キー情報取得を設定
                If Not strKeys Is Nothing AndAlso strKeys.Length >= 3 Then
                    txtConstructionRequestNo.ppText = strKeys(0)
                    txtTboxId.ppText = strKeys(1)
                    txtHoleNm.ppText = strKeys(2)
                    txtHoleNm_TextChanged(sender, e)
                End If

            End If

        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

#End Region

#Region "ユーザー権限"
    '---------------------------
    '2014/04/14 武 ここから
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
                'btnUpdate.Enabled = False
                'btnInsert.Enabled = False
                '---------------------------
                '2014/06/19 星野 ここから
                '---------------------------
                pnlRegister.Enabled = False
                fupKozisiryo.Enabled = False
                '---------------------------
                '2014/06/19 星野 ここまで
                '---------------------------
                ''---------------------------
                ''2014/06/20 武 ここから
                ''---------------------------
                txtKoziirai_IO.ppEnabled = False
                txtSeikyusakiCD.ppEnabled = False
                drpShiryouCD_IO.Enabled = False
                drpSeikyuJokyo.Enabled = False
                txtJyuryoDate_IO.ppEnabled = False
                txtJyuryoTime_IO.ppEnabled = False
                fupKozisiryo.Enabled = False
                txtBiko_IO.ppEnabled = False
                btnBack.Enabled = False
                ''---------------------------
                ''2014/06/20 武 ここまで
                ''---------------------------
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/06/02 後藤 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザ権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound
        '---------------------------
        '2014/06/20 武 ここから
        '---------------------------
        'Select Case Session(P_SESSION_AUTH)
        '    Case "管理者"
        '    Case "SPC"
        '    Case "営業所"
        '        e.Row.Cells(0).Enabled = False
        '    Case "NGC"
        'End Select

        ''---------------------------
        ''2014/06/09 後藤 ここから
        ''---------------------------
        'Select Case Session(P_SESSION_TERMS)
        '    Case  ClsComVer.E_遷移条件.参照
        '        e.Row.Cells(0).Enabled = False
        '    Case Else
        'End Select
        ''---------------------------
        ''2014/06/09 後藤 ここまで
        ''---------------------------
        '---------------------------
        '2014/06/20 武 ここまで
        '---------------------------
    End Sub
    '---------------------------
    '2014/06/02 後藤 ここまで
    '---------------------------
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------
#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click _
                                                                                  , btnBack.Click _
                                                                                  , btnUpdate.Click _
                                                                                  , btnInsert.Click _
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try

            Dim Fullpath As HttpPostedFile = Nothing
            Dim strExtension As String = Nothing
            Dim strUpladfile As String = Nothing
            Dim strfileName As String = Nothing
            Dim strErrmsg As String = Nothing
            Dim offName As String = Nothing

            Select Case sender.ID

                Case "btnSearchRigth1"         '検索

                    Try

                        '検証チェック(検索項目)
                        ms_check_ISvalid("1")

                        '検索処理開始
                        If (Page.IsValid) Then

                            'ビューステートの初期化
                            Me.ViewState(M_VIEW_SEARCH) = Nothing

                            '検索項目の再設定
                            msSet_View()

                            '更新ボタンが活性化している場合のみ排他制御を解除する
                            If Me.btnUpdate.Visible = True Then
                                '排他制御終了
                            End If

                            'ラベルに業者名を表示する
                            If Not trdBilling.ppText = String.Empty Then
                                If Not msGetBilling(Me.trdBilling.ppText, offName) = Nothing Then
                                    lblBilling.Text = offName
                                Else
                                    lblBilling.Text = ""
                                End If
                            Else
                                lblBilling.Text = ""
                            End If

                            '検索開始
                            If Not msGet_GridData() Then
                                '取得エラーのため処理終了
                                Exit Sub
                            End If

                            '登録項目の初期化
                            ms_clearUpd()

                            '--------------------------------
                            '2014/06/09 後藤　ここから
                            '--------------------------------
                            'ボタン/コントロールの非活性化
                            Select Case Session(P_SESSION_TERMS)
                                Case ClsComVer.E_遷移条件.参照
                                    ms_changeCtl("2")
                                Case Else
                                    ms_changeCtl("0")
                            End Select
                            '--------------------------------
                            '2014/06/09 後藤　ここまで
                            '--------------------------------

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
                        '処理の終了
                        Exit Sub

                    End Try

                Case "btnSearchRigth2"         '検索条件クリア

                    Me.txtConstructionRequestNo.ppText = Nothing     '工事依頼番号
                    Me.txtHoleNm.ppText = Nothing                    'ホール名
                    Me.lblHoleNm.Text = Nothing                      'ホール名ラベル
                    Me.txtTboxId.ppText = Nothing                    'TBOXID
                    Me.dftConstructionDt.ppFromText = Nothing        '工事日FROM
                    Me.dftConstructionDt.ppToText = Nothing          '工事日TO
                    Me.drpProgressSituation.SelectedIndex = 0        '進捗状況
                    Me.trdBilling.ppText = Nothing                   '請求先
                    Me.lblBilling.Text = Nothing                     '請求先ラベル
                    'ホール工事種別
                    Me.cbxHoleNew.Checked = False                    '新規
                    Me.cbxHoleExpansion.Checked = False              '増設
                    Me.cbxHoleReInstallation.Checked = False         '再設置
                    Me.cbxHoleShopRelocation.Checked = False         '店内移転
                    Me.cbxHoleSomeRemoval.Checked = False            '一部撤去
                    Me.cbxHoleAllRemoval.Checked = False             '全撤去
                    Me.cbxHoleOnceRemoval.Checked = False            '一時撤去
                    Me.cbxHoleConChange.Checked = False              '構成変更
                    Me.cbxHoleDlvOrgnz.Checked = False               '構成配信
                    Me.cbxVup.Checked = False                        'バージョンアップ
                    Me.cbxHoleOther.Checked = False                  'その他
                    'LAN工事種別
                    Me.cbxLanNew.Checked = False                     '新規
                    Me.cbxLanExpansion.Checked = False               '増設
                    Me.cbxLanReInstallation.Checked = False          '再設置
                    Me.cbxLanShopRelocation.Checked = False          '店内移転
                    Me.cbxLanSomeRemoval.Checked = False             '一部撤去
                    Me.cbxLanAllRemoval.Checked = False              '全撤去
                    Me.cbxLanOnceRemoval.Checked = False             '一時撤去
                    Me.cbxLanConChange.Checked = False               '構成変更
                    Me.cbxLanDlvOrgnz.Checked = False                '構成配信
                    Me.cbxLanOther.Checked = False                   'その他
                    'その他工事内容
                    Me.txtSonotaNaiyo.ppText = Nothing

                Case "btnClear"               'クリア

                    '登録項目の初期化
                    ms_clearUpd()

                    '更新ボタンが活性化している場合のみ排他制御を解除する
                    If Me.btnUpdate.Visible = True Then
                        '排他制御終了
                    End If

                    'ボタン/コントロールの非活性化
                    ms_changeCtl("0")

                Case "btnBack"                '元に戻す

                    Dim strView() As String = Nothing

                    'ビューステートから戻す
                    strView = Me.ViewState(M_VIEW_UPDATE)

                    'ビューステートに設定する
                    Me.txtKoziirai_IO.ppText = strView(0)
                    'Me.txtSeikyusakiCD_IO.ppText = strView(1)
                    Me.txtSeikyusakiCD.ppText = strView(1)
                    Me.drpShiryouCD_IO.SelectedIndex = CInt(strView(2))
                    Me.drpSeikyuJokyo.SelectedIndex = CInt(strView(3))
                    Me.txtJyuryoDate_IO.ppText = strView(4)
                    Me.txtJyuryoTime_IO.ppHourText = strView(5)
                    Me.txtJyuryoTime_IO.ppMinText = strView(6)
                    Me.txtBiko_IO.ppText = strView(7)

                Case "btnUpdate"              '更新

                    Try

                        '検証チェック(追加更新項目)
                        ms_check_ISvalid("2")

                        Dim strView() As String = Nothing

                        'アップロードファイルの有無
                        If Me.fupKozisiryo.HasFile Then
                            'ファイル名を取得
                            Fullpath = fupKozisiryo.PostedFile
                            'ファイルの拡張子を取得
                            strExtension = Path.GetExtension(Fullpath.FileName)
                            '拡張子の確認
                            msCheck_Integrity(strExtension, Fullpath)
                        End If

                        '検証チェック
                        If (Page.IsValid) Then
                            'ファイルアップロード
                            If Not Fullpath Is Nothing Then
                                'アップロード開始
                                If Not ms_file_upd(Fullpath, strExtension, strUpladfile, strfileName) Then
                                    'ファイルのアップロード失敗
                                    Exit Sub
                                End If
                            End If

                            '更新開始
                            If Not ms_mrg_ShiryoMeisai(strfileName) Then
                                'アップロードファイルの削除
                                If File.Exists(strUpladfile) Then
                                    'ファイルを削除
                                    System.IO.File.Delete(strUpladfile)
                                End If
                                '処理終了
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "資料請求データ")
                                '更新に失敗した
                                Exit Sub
                            End If

                            'ボタン/コントロールの非活性化
                            ms_changeCtl("1")

                            '画面の再描画
                            If Not msGet_GridData() Then
                                '取得エラーのため処理終了
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "資料請求データ")
                                Exit Sub
                            End If

                            '正常終了
                            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "資料請求データ")

                        End If

                    Catch ex As Exception

                        'システムエラー
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "資料請求データ")
                        '--------------------------------
                        '2014/04/14 星野　ここから
                        '--------------------------------
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '--------------------------------
                        '2014/04/14 星野　ここまで
                        '--------------------------------

                        '排他制御終了

                        '処理の終了
                        Exit Sub

                    End Try

                Case "btnInsert"              '追加

                    Try

                        '検証チェック(追加更新項目)
                        ms_check_ISvalid("2")
                        'ビューステートの初期化
                        ViewState("ファイル名") = String.Empty
                        'アップロードファイルの有無
                        If Me.fupKozisiryo.HasFile Then
                            'ファイル名を取得
                            Fullpath = fupKozisiryo.PostedFile
                            'ファイルの拡張子を取得
                            strExtension = Path.GetExtension(Fullpath.FileName)
                            '拡張子の確認
                            msCheck_Integrity(strExtension, Fullpath)
                        End If

                        '工事依頼番号からデータがあるかを確認する
                        If Not ms_serch_KojiNum() Then
                            '工事依頼番号が存在しない
                            Exit Sub
                        End If

                        '検証チェック
                        If (Page.IsValid) Then
                            'ファイルアップロード
                            If Not Fullpath Is Nothing Then
                                'アップロード開始
                                If Not ms_file_upd(Fullpath, strExtension, strUpladfile, strfileName) Then
                                    'ファイルのアップロード失敗
                                    Exit Sub
                                End If
                            End If

                            '追加開始
                            If Not ms_mrg_ShiryoMeisai(strfileName) Then
                                'アップロードファイルの削除
                                If File.Exists(strUpladfile) Then
                                    'ファイルを削除
                                    System.IO.File.Delete(strUpladfile)
                                End If
                                '処理終了
                                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "資料請求データ")
                                Exit Sub
                            End If

                            '正常終了

                            'ボタン/コントロールの非活性化
                            ms_changeCtl("0")

                            'ビューステイトの設定
                            msSet_View()

                            '画面の再描画
                            'If Not msGet_GridData() Then
                            '    '取得エラーのため処理終了
                            '    Exit Sub
                            'End If

                            '登録項目の初期化
                            ms_clearUpd()

                            'ボタン/コントロールの非活性化
                            ms_changeCtl("0")

                            '正常終了
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "資料請求データ")

                        End If

                    Catch ex As Exception

                        'システムエラー
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "資料請求データ")
                        '--------------------------------
                        '2014/04/14 星野　ここから
                        '--------------------------------
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '--------------------------------
                        '2014/04/14 星野　ここまで
                        '--------------------------------

                        '排他制御終了

                        '処理の終了
                        Exit Sub

                    End Try

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
    ''' グリッドボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim intIndex As Integer = Nothing                               'ボタン押下行のIndex
        Dim rowData As GridViewRow = Nothing                            'ボタン押下行
        Dim strErrmsg As String = Nothing                               '置き換えエラーメッセージ
        Dim strDownLoadfile As String = Nothing                         'ダウンロード対象ファイル名
        Dim strDownLoadPath As String = Nothing                         '保管場所
        Dim strFullPath As String = Nothing                             'フルパス
        Dim ev As EventArgs = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '選択／印刷ボタン以外は終了
            If e.CommandName <> "btnSelect" And e.CommandName <> "btnPrint" Then
                Exit Sub
            End If
            intIndex = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            rowData = grvList.Rows(intIndex)                 'ボタン押下行

            Select Case e.CommandName

                Case "btnSelect"          '選択ボタン

                    Dim strView(8 - 1) As String

                    '--------------------------------
                    '2014/05/30 後藤　ここから
                    '--------------------------------
                    '★排他制御用の変数
                    Dim strExclusiveDate As String = String.Empty
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList
                    '--------------------------------
                    '2014/05/30 後藤　ここまで
                    '--------------------------------
                    '業者名用の変数
                    Dim offName As String = ""

                    '工事依頼番号と連番が存在する場合、排他制御の解除を行う
                    If Me.txtKoziirai_IO.ppText <> Nothing And Me.lblRenban_data.Text <> Nothing Then

                        '排他解除

                        '--------------------------------
                        '2014/05/30 後藤　ここから
                        '--------------------------------

                        '★排他情報削除
                        If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                            clsExc.pfDel_Exclusive(Me _
                                                         , Session(P_SESSION_SESSTION_ID) _
                                                         , Me.Master.Master.ppExclusiveDateDtl)

                            Me.Master.Master.ppExclusiveDateDtl = String.Empty

                        End If
                        '--------------------------------
                        '2014/05/30 後藤　ここまで
                        '--------------------------------


                        '登録項目の初期化
                        ms_clearUpd()

                    End If

                    '排他制御確認
                    '排他制御開始

                    '--------------------------------
                    '2014/05/30 後藤　ここから
                    '--------------------------------
                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D57_CNSTREQSPEC_DTL3")
                    'arTable_Name.Insert(1, "D39_CNSTREQSPEC")

                    '★ロックテーブルキー項目の登録(D57_CNSTREQSPEC_DTL3,D39_CNSTREQSPEC)
                    arKey.Insert(0, CType(rowData.FindControl("工事依頼番号"), TextBox).Text)
                    arKey.Insert(1, CType(rowData.FindControl("連番"), TextBox).Text)

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_DISP_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

                        ms_GetDDLSiryou(CType(rowData.FindControl("工事依頼番号"), TextBox).Text)

                        'マスタのデータ有無判断
                        Try

                            Me.drpShiryouCD_IO.SelectedValue = CType(rowData.FindControl("資料コード"), TextBox).Text
                            Me.drpSeikyuJokyo.SelectedValue = CType(rowData.FindControl("進捗データ"), TextBox).Text

                        Catch ex As Exception

                            'マスタに情報がありません
                            psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                            '--------------------------------
                            '2014/04/14 星野　ここから
                            '--------------------------------
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            '--------------------------------
                            '2014/04/14 星野　ここまで
                            '--------------------------------
                            Exit Sub

                        End Try

                        '選択行の情報を更新コントロールに渡す
                        Me.txtKoziirai_IO.ppText = CType(rowData.FindControl("工事依頼番号"), TextBox).Text
                        Me.txtSeikyusakiCD.ppText = CType(rowData.FindControl("請求先コード"), TextBox).Text
                        If CType(rowData.FindControl("受領日時"), TextBox).Text.Length >= 19 Then

                            Me.txtJyuryoDate_IO.ppText = CType(rowData.FindControl("受領日時"), TextBox).Text.Substring(0, 10)
                            Me.txtJyuryoTime_IO.ppHourText = CType(rowData.FindControl("受領日時"), TextBox).Text.Substring(11, 2)
                            Me.txtJyuryoTime_IO.ppMinText = CType(rowData.FindControl("受領日時"), TextBox).Text.Substring(14, 2)

                        End If

                        Me.txtBiko_IO.ppText = CType(rowData.FindControl("備考"), TextBox).Text
                        Me.lblRenban_data.Text = CType(rowData.FindControl("連番"), TextBox).Text

                        'ビューステートに設定する
                        strView(0) = Me.txtKoziirai_IO.ppText
                        strView(1) = Me.txtSeikyusakiCD.ppText
                        strView(2) = Me.drpShiryouCD_IO.SelectedIndex.ToString
                        strView(3) = Me.drpSeikyuJokyo.SelectedIndex.ToString
                        strView(4) = Me.txtJyuryoDate_IO.ppText
                        strView(5) = Me.txtJyuryoTime_IO.ppHourText
                        strView(6) = Me.txtJyuryoTime_IO.ppMinText
                        strView(7) = Me.txtBiko_IO.ppText

                        'ラベルに業者名を表示する
                        If Not txtSeikyusakiCD.ppText = String.Empty Then
                            If Not msGetBilling(Me.txtSeikyusakiCD.ppText, offName) = Nothing Then
                                lblOffice_name.Text = offName
                            Else
                                lblOffice_name.Text = ""
                            End If
                        Else
                            lblOffice_name.Text = ""
                        End If

                        '元に戻すボタン押下時の情報を設定
                        Me.ViewState.Add(M_VIEW_UPDATE, strView)

                        'ファイル名が存在した場合に保管
                        If Not CType(rowData.FindControl("ファイル名"), TextBox).Text = String.Empty Then

                            Me.ViewState("ファイル名") = CType(rowData.FindControl("ファイル名"), TextBox).Text

                        End If

                        'ボタン/コントロールの非活性化
                        ms_changeCtl("1")

                    Else

                        '排他ロック中

                    End If
                    '--------------------------------
                    '2014/05/30 後藤　ここまで
                    '--------------------------------




                Case "btnPrint"           '印刷ボタン

                    Dim strFileName As String = Nothing
                    Dim strFilePath As String = Nothing
                    Dim slctItem As String = CType(grvList.Rows(e.CommandArgument).FindControl("工事依頼番号"), TextBox).Text
                    Dim splPath As String()
                    Dim filePath2 As String = Nothing
                    Dim opblnResult As Boolean
                    Dim strExtension As String = Nothing
                    Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
                    Dim localFiledir As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    Dim localdirName As String = "DOWNLOAD"
                    Dim strLocalPath As String = "C:" & "/" & localdirName & "/"

                    'ファイル名の設定
                    'strFileName = CType(rowData.FindControl("ファイル名"), TextBox).Text

                    '保存場所の取得
                    strFilePath = msGet_SavePath(CType(rowData.FindControl("資料コード"), TextBox).Text, slctItem, "GET")
                    If strFilePath Is Nothing Then
                        psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strFileName)
                        Exit Sub
                    End If



                    strFilePath = strFilePath.Replace("/", "\")
                    splPath = strFilePath.Split("\")
                    For spl = 1 To splPath.Count - 2
                        filePath2 &= splPath(spl) & "\"
                    Next
                    strFileName = splPath(splPath.Count - 1)

                    'フルパスの設定
                    'strFullPath = "\\" + strFilePath + "\" + strFileName
                    strFullPath = "\\" + strFilePath
                    'アップロードファイル存在確認(保存先)
                    'If Not File.Exists(strFullPath) Then
                    '    'ファイルが存在しない
                    '    psMesBox(Me, "30003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, strFileName)
                    '    Exit Sub
                    'End If
                    'ファイルの存在確認
                    If DBFTP.pfFtpFile_Exists(filePath2, strFileName, opblnResult) = False Then

                        'ファイルが存在しない
                        psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strFileName)
                        Exit Sub

                    End If

                    '拡張子の取得
                    strExtension = Path.GetExtension(strFileName)
                    localFileName = localFileName & strExtension
                    'ローカルにフォルダがなかった場合、作成する
                    If Directory.Exists(strLocalPath) = False Then
                        System.IO.Directory.CreateDirectory(strLocalPath)
                    End If

                    'ローカルにダウンロードを開始する
                    DBFTP.pfFtpFile_Copy("GET", filePath2, strFileName, opblnResult, localdirName & "/" & localFileName)

                    'ダウンロードファイル存在確認(保存先)
                    If Not File.Exists("C:/" & "DOWNLOAD" & "/" & localFileName) Then
                        'ファイルが存在しない
                        psMesBox(Me, "30003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strFileName)
                        Exit Sub
                    End If

                    'パスの再設定
                    strLocalPath = "C:/" & "DOWNLOAD" & "/" & localFileName

                    'ダウンロード
                    Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strLocalPath & "&filename=" & HttpUtility.UrlEncode(strFileName), False)

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
                                    objStack.GetMethod.Name, "~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFullPath & "&filename=" & HttpUtility.UrlEncode(strFileName), strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    ''ダウンロード
                    'Response.Redirect("~/Common/COMLSTP099/COMLSTP099.ashx?path=" & strFullPath & "&filename=" & HttpUtility.UrlEncode(strFileName), False)

            End Select


        Catch ex As Exception

            'システムエラー
            psMesBox(Me, "30002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strDelF As String = Nothing '削除フラグ

        'ファイル名有り無し判断
        For Each rowData As GridViewRow In grvList.Rows
            strDelF = CType(rowData.FindControl("ファイル有無判断"), TextBox).Text
            If strDelF = "0" Then   '資料無し

                rowData.Cells(1).Enabled = False
                CType(rowData.FindControl("資料有無"), CheckBox).Checked = False

            Else

                rowData.Cells(1).Enabled = True
                CType(rowData.FindControl("資料有無"), CheckBox).Checked = True

            End If

            rowData.Cells(4).Enabled = False

        Next

    End Sub

    ''' <summary>
    ''' 業者情報取得(検索項目)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub trdBilling_TextChanged(sender As Object, e As EventArgs)
        Dim dtsData As DataSet = Nothing
        Dim offName As String = Nothing
        Dim numChk As Integer = 0

        '入力チェック
        If Not Me.trdBilling.ppText = String.Empty Then
            If pfCheck_Num_Sym(trdBilling.ppText) = False Then
                trdBilling.psSet_ErrorNo("4001", "請求先(検索項目)", "数字")
            Else
                numChk = 1
            End If
        Else
            numChk = 1
        End If

        If numChk = 1 Then
            'ラベルに業者名を表示する
            If Not Me.trdBilling.ppText = String.Empty Then
                If Not msGetBilling(Me.trdBilling.ppText, offName) = Nothing Then
                    lblBilling.Text = offName
                Else
                    lblBilling.Text = ""
                End If
            Else
                lblBilling.Text = ""
            End If
        Else
            Exit Sub
        End If


    End Sub

    ''' <summary>
    ''' 業者情報取得(明細項目)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtSeikyusakiCD_TextChanged(sender As Object, e As EventArgs)
        Dim dtsData As DataSet = Nothing
        Dim offName As String = Nothing
        Dim numchk As Integer = 0

        '入力チェック
        If Not Me.txtSeikyusakiCD.ppText = String.Empty Then
            If pfCheck_Num_Sym(txtSeikyusakiCD.ppText) = False Then
                txtSeikyusakiCD.psSet_ErrorNo("4001", "請求先(登録項目)", "数字")
            Else
                numchk = 1
            End If
        Else
            numchk = 1
        End If

        If numchk = 1 Then
            'ラベルに業者名を表示する
            If Not Me.txtSeikyusakiCD.ppText = String.Empty Then
                If Not msGetBilling(Me.txtSeikyusakiCD.ppText, offName) = Nothing Then
                    lblOffice_name.Text = offName
                Else
                    lblOffice_name.Text = ""
                End If
            Else
                lblOffice_name.Text = ""
            End If
        Else
            Exit Sub
        End If


    End Sub

    ''' <summary>
    ''' コントロールの初期化
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '--------------------------------
            '2014/06/09 後藤　ここから
            '--------------------------------
            Dim strKeyCode(0) As String
            strKeyCode(0) = "0"

            'Master.Master.ppRigthButton1.Visible = False
            'Master.Master.ppRigthButton2.Visible = False
            Master.Master.ppRigthButton1.Enabled = False
            Master.Master.ppRigthButton2.Enabled = False

            Me.txtConstructionRequestNo.ppText = Nothing     '工事依頼番号
            Me.txtHoleNm.ppText = Nothing                    'ホール名
            Me.lblHoleNm.Text = Nothing                      'ホール名ラベル
            Me.txtTboxId.ppText = Nothing                    'TBOXID
            Me.dftConstructionDt.ppFromText = Nothing        '工事日FROM
            Me.dftConstructionDt.ppToText = Nothing          '工事日TO
            Me.drpProgressSituation.SelectedIndex = 0        '進捗状況
            Me.trdBilling.ppText = Nothing                   '請求先
            Me.lblBilling.Text = Nothing                     '請求先ラベル
            'Me.trfBilling.ppURL = M_GYOSYA_PATH              '業者情報へ遷移するためのパス
            '--07/09    武
            'Me.trdBilling.ppKeyCode = strKeyCode             '業者情報ポップアップ用のキー項目
            'ホール工事種別
            Me.cbxHoleNew.Checked = False                    '新規
            Me.cbxHoleExpansion.Checked = False              '増設
            Me.cbxHoleReInstallation.Checked = False         '再設置
            Me.cbxHoleShopRelocation.Checked = False         '店内移転
            Me.cbxHoleSomeRemoval.Checked = False            '一部撤去
            Me.cbxHoleAllRemoval.Checked = False             '全撤去
            Me.cbxHoleOnceRemoval.Checked = False            '一時撤去
            Me.cbxHoleConChange.Checked = False              '構成変更
            Me.cbxHoleDlvOrgnz.Checked = False               '構成配信
            Me.cbxVup.Checked = False                        'バージョンアップ
            Me.cbxHoleOther.Checked = False                  'その他
            'LAN工事種別
            Me.cbxLanNew.Checked = False                     '新規
            Me.cbxLanExpansion.Checked = False               '増設
            Me.cbxLanReInstallation.Checked = False          '再設置
            Me.cbxLanShopRelocation.Checked = False          '店内移転
            Me.cbxLanSomeRemoval.Checked = False             '一部撤去
            Me.cbxLanAllRemoval.Checked = False              '全撤去
            Me.cbxLanOnceRemoval.Checked = False             '一時撤去
            Me.cbxLanConChange.Checked = False               '構成変更
            Me.cbxLanDlvOrgnz.Checked = False                '構成配信
            Me.cbxLanOther.Checked = False                   'その他
            'その他工事内容
            Me.txtSonotaNaiyo.ppText = Nothing

            '登録項目の初期化
            ms_clearUpd()

            'ドロップダウンリスト(進捗ステータス)の設定
            msGet_DROPData_Sel()
            Me.drpSeikyuJokyo.SelectedIndex = 0
            Me.drpProgressSituation.SelectedIndex = 0

            '表示グリッドビューの初期化
            Me.grvList.DataSource = New Object() {}
            Me.grvList.DataBind()
            Master.ppCount = 0

            'グリッドビューの表示
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ボタン押下のメッセージ
            Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "請求資料 状況")
            Me.btnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "請求資料 状況")
            Me.btnBack.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "更新項目の初期化")
            Me.btnClear.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "更新項目のクリア")

            'エラーグループ設定
            'Master.ppRigthButton1.ValidationGroup = "1"
            Me.btnUpdate.ValidationGroup = "2"
            Me.btnInsert.ValidationGroup = "2"

            '権限の制御
            Select Case Session(P_SESSION_TERMS)
                Case ClsComVer.E_遷移条件.参照
                    'Me.btnBack.Visible = False
                    'Me.btnClear.Visible = False
                    'Me.btnInsert.Visible = False
                    'Me.btnUpdate.Visible = False
                    Me.btnBack.Enabled = False
                    Me.btnClear.Enabled = False
                    Me.btnInsert.Enabled = False
                    Me.btnUpdate.Enabled = False
                    '登録項目の非活性
                    txtKoziirai_IO.ppEnabled = False
                    txtSeikyusakiCD.ppEnabled = False
                    drpShiryouCD_IO.Enabled = False
                    drpSeikyuJokyo.Enabled = False
                    txtJyuryoDate_IO.ppEnabled = False
                    txtJyuryoTime_IO.ppEnabled = False
                    fupKozisiryo.Enabled = False
                    txtBiko_IO.ppEnabled = False
                Case ClsComVer.E_遷移条件.更新
                    'Me.btnBack.Visible = False
                    'Me.btnClear.Visible = True
                    'Me.btnInsert.Visible = True
                    'Me.btnUpdate.Visible = False
                    Me.btnBack.Enabled = False
                    Me.btnClear.Enabled = True
                    Me.btnInsert.Enabled = True
                    Me.btnUpdate.Enabled = False
            End Select
            '--------------------------------
            '2014/06/09 後藤　ここまで
            '--------------------------------

        Catch ex As Exception

            '画面を終了
            psMesBox(Me, "0007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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

        End Try

    End Sub

    ''' <summary>
    ''' 更新、登録処理のコントロール切り替え
    ''' </summary>
    ''' <param name="strCase"></param>
    ''' <remarks></remarks>
    Protected Sub ms_changeCtl(ByVal strCase As String)
        '--------------------------------
        '2014/06/09 後藤　ここから
        '--------------------------------
        'コントロールの活性費活性を制御
        Select Case strCase

            Case "0"       '初期、新規追加時

                'Me.btnBack.Visible = False
                'Me.btnClear.Visible = True
                'Me.btnInsert.Visible = True
                'Me.btnUpdate.Visible = False
                Me.btnBack.Enabled = False
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = True
                Me.btnUpdate.Enabled = False
                Me.grvList.Enabled = True
                Me.txtKoziirai_IO.ppEnabled = True

            Case "1"        '更新時

                'Me.btnBack.Visible = True
                'Me.btnClear.Visible = True
                'Me.btnInsert.Visible = False
                'Me.btnUpdate.Visible = True
                Me.btnBack.Enabled = True
                Me.btnClear.Enabled = True
                Me.btnInsert.Enabled = False
                Me.btnUpdate.Enabled = True
                Me.grvList.Enabled = True
                Me.txtKoziirai_IO.ppEnabled = False

            Case "2"         '参照時

                Me.btnBack.Enabled = False
                Me.btnClear.Enabled = False
                Me.btnInsert.Enabled = False
                Me.btnUpdate.Enabled = False
                Me.grvList.Enabled = True
                Me.txtKoziirai_IO.ppEnabled = False

        End Select

        'Master.ppRigthButton1.Visible = True
        'Master.ppRigthButton2.Visible = True
        Master.ppRigthButton1.Enabled = True
        Master.ppRigthButton2.Enabled = True
        '--------------------------------
        '2014/06/09 後藤　ここまで
        '--------------------------------
    End Sub

    ''' <summary>
    ''' ドロップダウンリストのINDEX指定
    ''' </summary>
    ''' <param name="strSelectValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ms_getDrpIndex(ByVal strSelectValue As String, DDLitem As Object) As Integer
        Dim idx As Integer = 0
        Dim flag As Boolean = False

        For Each item As ListItem In DDLitem.item
            ' value が 用途コードと一致する
            If (item.Value = strSelectValue) Then
                flag = True
                Return idx
                Exit Function
            End If
            idx += 1
        Next

        Return 0

    End Function

    ''' <summary>
    ''' 登録項目の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ms_clearUpd()

        Dim strKeyCode(0) As String
        strKeyCode(0) = "0"

        Me.txtKoziirai_IO.ppText = Nothing
        Me.txtSeikyusakiCD.ppText = Nothing
        'Me.txtSeikyusakiCD.ppKeyCode = strKeyCode
        'Me.txtSeikyusakiCD_IO.ppURL = M_GYOSYA_PATH
        Me.drpShiryouCD_IO.Items.Clear()
        Me.drpSeikyuJokyo.SelectedIndex = 0
        Me.txtBiko_IO.ppText = Nothing
        Me.lblRenban_data.Text = Nothing
        '--------------------------------
        '2014/06/09 後藤　ここから
        '--------------------------------
        'Me.btnUpdate.Visible = False
        'Me.btnBack.Visible = False
        Me.btnUpdate.Enabled = False
        Me.btnBack.Enabled = False
        '--------------------------------
        '2014/06/09 後藤　ここまで
        '--------------------------------
        Me.txtJyuryoDate_IO.ppText = Nothing
        Me.txtJyuryoTime_IO.ppHourText = Nothing
        Me.txtJyuryoTime_IO.ppMinText = Nothing
        Me.lblOffice_name.Text = Nothing

    End Sub

    ''' <summary>
    ''' 保管場所取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_SavePath(ByVal strSeikyuCD As String, ByVal slctItem As String, ByVal cls As String) As String

        Dim strPath As String = Nothing
        Dim result As String = "0"
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim dstOrders_2 As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S6", conDB)

            With cmdDB.Parameters
                'パラメータ設定
                '.Add(pfSet_Param("prm_file", SqlDbType.NVarChar, strSeikyuCD))      'ファイル種別
                .Add(pfSet_Param("prm_file", SqlDbType.NVarChar, "0281PT"))      'ファイル種別
            End With

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If cls = "GET" Then

                cmdDB = New SqlCommand(M_DISP_ID & "_S8", conDB)

                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_MngNo", SqlDbType.NVarChar, slctItem))      'ファイル種別
                    .Add(pfSet_Param("prm_fileCls", SqlDbType.NVarChar, "0281PT"))      'ファイル種別
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))  '結果取得用(0:データなし、1:データあり)
                End With

                dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                strPath = dstOrders.Tables(0).Rows(0).Item("パス").ToString + "/" + dstOrders_2.Tables(0).Rows(0).Item("ファイル名").ToString
            Else
                strPath = dstOrders.Tables(0).Rows(0).Item("パス").ToString
            End If

            '終了ログ出力
            psLogEnd(Me)

            Return strPath


        Catch ex As SqlException

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

        End Try
    End Function

    ''' <summary>
    ''' アップロード処理
    ''' </summary>
    ''' <param name="path"></param>
    ''' <param name="Extension"></param>
    ''' <param name="SavePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_file_upd(ByVal path As HttpPostedFile _
                                 , ByVal Extension As String _
                                 , ByRef SavePath As String _
                                 , ByRef fileName As String) As Boolean

        Dim strSavePath As String = Nothing
        Dim strFileName As String = Nothing
        Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString
        Dim localDirName As String = "UPLOAD"
        Dim strLocalDir As String = "C:"
        Dim strLocalPath As String = strLocalDir & "/" & localDirName & "/"
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)
            'ローカルにファイルを一時的に保存
            If Not path.FileName = "" Then

                'フォルダがなかった場合、作成する
                If Directory.Exists(strLocalPath) = False Then
                    System.IO.Directory.CreateDirectory(strLocalPath)
                End If

                strLocalPath = strLocalPath & localFileName & Extension
                'strLocalPath.Replace(":\", "://")

                'ローカルに保存
                path.SaveAs(strLocalPath)
            End If


            '保管場所情報を取得
            strSavePath = msGet_SavePath(drpShiryouCD_IO.SelectedValue, txtKoziirai_IO.ppText, "PUT")
            If strSavePath = Nothing Then

                '保管場所の取得に失敗した
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保管場所")
                Throw New Exception

            End If

            'アップロード処理
            strFileName = msGet_fileName(Extension _
                                         , strSavePath _
                                         , Me.txtKoziirai_IO.ppText _
                                         , Me.drpShiryouCD_IO.SelectedItem.ToString _
                                         , fileName _
                                         , strLocalPath)
            'アップロードに失敗した
            If strFileName = Nothing Then

                psMesBox(Me, "20003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Me.drpShiryouCD_IO.SelectedItem.ToString)
                Throw New Exception

            End If
            ''アップロード
            'SavePath = strFileName
            'path.SaveAs(SavePath)

            'アップロードファイル存在確認(保存先)
            'If Not File.Exists(SavePath) Then
            '    'ファイルが存在しない
            '    psMesBox(Me, "20003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, Me.drpShiryouCD_IO.SelectedItem.ToString)
            '    Throw New Exception
            'Else

            'End If

            Return True

        Catch ex As Exception
            'ローカルに一時的に作成したファイルを削除
            'ファイルの存在を確認
            If System.IO.File.Exists(strLocalPath) Then
                System.IO.File.Delete(strLocalPath)
            End If
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

            'ローカルに一時的に作成したファイルを削除
            'ファイルの存在を確認
            If System.IO.File.Exists(strLocalPath) Then
                System.IO.File.Delete(strLocalPath)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' 一覧表示データの取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_GridData() As Boolean

        Dim strPath As String = Nothing
        Dim result As String = "0"
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strView() As String = Nothing
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Throw New Exception
            End If

            'ビューステートから戻す
            strView = Me.ViewState(M_VIEW_SEARCH)

            'グリッド及び件数の初期化
            Me.grvList.DataSource = Nothing
            Me.grvList.DataBind()

            cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_kojinum", SqlDbType.NVarChar, strView(0)))                     '工事依頼番号
                .Add(pfSet_Param("prm_hallname", SqlDbType.NVarChar, strView(1)))                    'ホール名
                .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, strView(2)))                      'TBOXID
                .Add(pfSet_Param("prm_kojidateFrom", SqlDbType.NVarChar, strView(3)))                '工事日FROM
                .Add(pfSet_Param("prm_kojidateTo", SqlDbType.NVarChar, strView(4)))                  '工事日TO
                .Add(pfSet_Param("prm_shintyoku", SqlDbType.NVarChar, strView(5)))                   '進捗状況
                .Add(pfSet_Param("prm_Seikyusaki", SqlDbType.NVarChar, strView(6)))                  '請求先
                .Add(pfSet_Param("prm_hallNew", SqlDbType.NVarChar, strView(7)))                     'ホール工事種別(新規)
                .Add(pfSet_Param("prm_hallZousetsu", SqlDbType.NVarChar, strView(8)))                'ホール工事種別(増設)
                .Add(pfSet_Param("prm_hallSaisechi", SqlDbType.NVarChar, strView(9)))                'ホール工事種別(再設置)
                .Add(pfSet_Param("prm_hallTennai", SqlDbType.NVarChar, strView(10)))                 'ホール工事種別(店内移設)
                .Add(pfSet_Param("prm_hallItibu", SqlDbType.NVarChar, strView(11)))                  'ホール工事種別(一部撤去)
                .Add(pfSet_Param("prm_hallZen", SqlDbType.NVarChar, strView(12)))                    'ホール工事種別(全撤去)
                .Add(pfSet_Param("prm_hallItiji", SqlDbType.NVarChar, strView(13)))                  'ホール工事種別(一時撤去)
                .Add(pfSet_Param("prm_hallKousei", SqlDbType.NVarChar, strView(14)))                 'ホール工事種別(構成変更)
                .Add(pfSet_Param("prm_hallHaishin", SqlDbType.NVarChar, strView(15)))                'ホール工事種別(構成配信)
                .Add(pfSet_Param("prm_vup", SqlDbType.NVarChar, strView(28)))                        'バージョンアップ
                .Add(pfSet_Param("prm_hallSonota", SqlDbType.NVarChar, strView(16)))                 'LAN工事種別(その他)
                .Add(pfSet_Param("prm_lanNew", SqlDbType.NVarChar, strView(17)))                     'LAN工事種別(新規)
                .Add(pfSet_Param("prm_lanZousetsu", SqlDbType.NVarChar, strView(18)))                'LAN工事種別(増設)
                .Add(pfSet_Param("prm_lanSaisechi", SqlDbType.NVarChar, strView(19)))                'LAN工事種別(再設置)
                .Add(pfSet_Param("prm_lanTennai", SqlDbType.NVarChar, strView(20)))                  'LAN工事種別(店内移設)
                .Add(pfSet_Param("prm_lanItibu", SqlDbType.NVarChar, strView(21)))                   'LAN工事種別(一部撤去)
                .Add(pfSet_Param("prm_lanZen", SqlDbType.NVarChar, strView(22)))                     'LAN工事種別(全撤去)
                .Add(pfSet_Param("prm_lanItiji", SqlDbType.NVarChar, strView(23)))                   'LAN工事種別(一時撤去)
                .Add(pfSet_Param("prm_lanKousei", SqlDbType.NVarChar, strView(24)))                  'LAN工事種別(構成変更)
                .Add(pfSet_Param("prm_lanHaishin", SqlDbType.NVarChar, strView(25)))                 'LAN工事種別(構成配信)
                .Add(pfSet_Param("prm_lanSonota", SqlDbType.NVarChar, strView(26)))                  'LAN工事種別(その他)
                .Add(pfSet_Param("prm_naiyo", SqlDbType.NVarChar, strView(27)))                      'その他工事内容
                .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)


            '結果情報を取得
            strOKNG = cmdDB.Parameters("data_exist").Value.ToString

            Select Case strOKNG
                Case "0"         'データ無し

                    '件数を設定
                    Master.ppCount = "0"
                    '--------------------------------
                    '2014/05/12 後藤　ここから
                    '--------------------------------
                    '0件
                    'psMesBox(Me, "00007", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    '--------------------------------
                    '2014/05/12 後藤　ここまで
                    '--------------------------------
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

            '終了ログ出力
            psLogEnd(Me)

            Return True

        Catch ex As SqlException

            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求資料　状況更新")
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

            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求資料　状況更新")
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
    End Function

    ''' <summary>
    ''' ファイル名取得/ディレクトリの作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_fileName(ByVal Extension As String _
                                  , ByVal filePath As String _
                                  , ByVal Kojinum As String _
                                  , ByVal Filenum As String _
                                  , ByRef fileName As String _
                                  , ByVal strLocalPath As String) As String

        Dim dirpath As DirectoryInfo
        Dim splPath As String() = Nothing
        Dim filePath2 As String = Nothing
        Dim folderName As String = DateTime.Now.ToString("yyyyMMdd")
        Dim opblnresult As Boolean = False
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            filePath = filePath.Replace("/", "\")
            filePath = "\\" + filePath + "\" + folderName
            splPath = filePath.Split("\")
            'splPath(splPath.Count - 2) = folderName

            For spl = 3 To splPath.Count - 1
                filePath2 &= splPath(spl) & "\"
            Next

            fileName = Filenum + "_" + Kojinum + "_" + DateTime.Now.ToString("yyyyMMdd") + Extension

            '保存先のフォルダが存在しない場合作成し、アップロード
            'dirpath = New DirectoryInfo(filePath)
            dirpath = New DirectoryInfo(filePath2)
            'If Not dirpath.Exists Then
            '    dirpath.Create()
            'End If
            If DBFTP.pfFtpDir_Exists(dirpath.ToString, opblnresult, "1") = False Then
                '存在しなかった場合、フォルダを作成しアップロード
                If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, folderName, fileName, opblnresult, strLocalPath) = False Then
                    'アップロードに失敗
                    Throw New Exception
                End If
                'フォルダが存在した場合
            Else
                'アップロード
                If DBFTP.pfFtpFile_Exists(dirpath.ToString, fileName, opblnresult) = True Then
                    If DBFTP.pfFtpFile_Delete(dirpath.ToString, fileName, opblnresult) = False Then
                        'アップロードに失敗
                        Throw New Exception
                    End If
                    'Else
                    '    splPath(splPath.Count - 1) = splPath(splPath.Count - 1) + "/" + fileName
                    '    'splPath(splPath.Count - 1) = fileName
                    '    ms_InsDownload(splPath)
                End If
                If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, fileName, opblnresult, strLocalPath) = False Then
                    'アップロードに失敗
                    Throw New Exception
                Else
                    'splPath(splPath.Count - 1) = splPath(splPath.Count - 1) + "/" + fileName
                    splPath(splPath.Count - 1) = splPath(splPath.Count - 1) + "/" + fileName
                    ms_InsDownload(splPath)
                End If

            End If

            Return filePath + "\" + fileName

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
            'システムエラー
            Return Nothing

        Finally
        End Try

    End Function

    ''' <summary>
    ''' ドロップダウンリスト(進捗ステータスコード)の情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_DROPData_Sel()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                msGet_DROPData_Sel = False
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB) 'すべての表示情報を取得

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '項目にデータ設定
            '取得したデータをリストに設定
            Me.drpSeikyuJokyo.DataSource = dstOrders.Tables(0)
            Me.drpProgressSituation.DataSource = dstOrders.Tables(0)

            '進捗状況ステータスの設定(ドロップダウンリストに表示する)
            Me.drpSeikyuJokyo.DataValueField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
            Me.drpProgressSituation.DataValueField = dstOrders.Tables(0).Columns(0).ColumnName.ToString

            '進捗状況ステータス名の設定(テキストボックスに表示する)
            Me.drpSeikyuJokyo.DataTextField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
            Me.drpProgressSituation.DataTextField = dstOrders.Tables(0).Columns(1).ColumnName.ToString

            'ドロップダウンリストにデータをバインド
            Me.drpSeikyuJokyo.DataBind()
            Me.drpProgressSituation.DataBind()

            '空白を設定
            Me.drpSeikyuJokyo.Items.Insert(0, " ")
            Me.drpProgressSituation.Items.Insert(0, " ")

            '正常終了
            msGet_DROPData_Sel = True

        Catch ex As SqlException

            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            msGet_DROPData_Sel = False

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
            msGet_DROPData_Sel = False

        Finally

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' ドロップダウンリスト(資料請求コード)の情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetDDLSiryou(ByVal cntrol_num As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S5", conDB) 'すべての表示情報を取得
            With cmdDB.Parameters
                .Add(pfSet_Param("prm_cnst_no", SqlDbType.NVarChar, cntrol_num))
            End With

            'リストデータ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '項目にデータ設定
            '取得したデータをリストに設定
            Me.drpShiryouCD_IO.DataSource = dstOrders.Tables(0)

            '資料名コードの設定
            Me.drpShiryouCD_IO.DataValueField = dstOrders.Tables(0).Columns(0).ColumnName.ToString

            '資料名の設定
            Me.drpShiryouCD_IO.DataTextField = dstOrders.Tables(0).Columns(1).ColumnName.ToString

            'ドロップダウンリストにデータをバインド
            Me.drpShiryouCD_IO.DataBind()

            '空白を設定
            Me.drpShiryouCD_IO.Items.Insert(0, " ")

            '正常終了

        Catch ex As SqlException

            '更新に失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求資料名")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

        Catch ex As Exception

            '更新に失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "請求資料名")
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
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try
    End Sub

    ''' <summary>
    ''' ビューステートの保管
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_View()

        Dim strView(29 - 1) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '工事依頼番号
            If Me.txtConstructionRequestNo.ppText = String.Empty Then

                strView(0) = String.Empty

            Else

                strView(0) = Me.txtConstructionRequestNo.ppText     '工事依頼番号

            End If

            'ホール名
            If Me.txtHoleNm.ppText = String.Empty Then

                strView(1) = String.Empty

            Else

                strView(1) = Me.txtHoleNm.ppText

            End If

            'TBOXID
            If Me.txtTboxId.ppText = String.Empty Then

                strView(2) = String.Empty

            Else

                strView(2) = Me.txtTboxId.ppText

            End If

            '工事日FROM
            If Me.dftConstructionDt.ppFromText = String.Empty Then

                strView(3) = String.Empty

            Else

                strView(3) = Me.dftConstructionDt.ppFromText.Replace("/", "")

            End If

            '工事日TO
            If Me.dftConstructionDt.ppToText = String.Empty Then

                strView(4) = String.Empty

            Else

                strView(4) = Me.dftConstructionDt.ppToText.Replace("/", "")

            End If

            '進捗状況
            If Me.drpProgressSituation.SelectedIndex = 0 Then

                strView(5) = String.Empty

            Else

                strView(5) = Me.drpProgressSituation.SelectedValue

            End If

            '請求先
            If Me.trdBilling.ppText = String.Empty Then

                strView(6) = String.Empty

            Else

                strView(6) = Me.trdBilling.ppText

            End If

            'ホール工事種別
            If Me.cbxHoleNew.Checked Then                       '新規
                strView(7) = "1" 'trueの場合
            Else
                strView(7) = "0" 'falseの場合
            End If
            If Me.cbxHoleExpansion.Checked Then                 '増設
                strView(8) = "1" 'trueの場合
            Else
                strView(8) = "0" 'falseの場合
            End If
            If Me.cbxHoleReInstallation.Checked Then            '再設置
                strView(9) = "1" 'trueの場合
            Else
                strView(9) = "0" 'falseの場合
            End If
            If Me.cbxHoleShopRelocation.Checked Then            '店内移転
                strView(10) = "1" 'trueの場合
            Else
                strView(10) = "0" 'falseの場合
            End If
            If Me.cbxHoleSomeRemoval.Checked Then               '一部撤去
                strView(11) = "1" 'trueの場合
            Else
                strView(11) = "0" 'falseの場合
            End If
            If Me.cbxHoleAllRemoval.Checked Then                '全撤去
                strView(12) = "1" 'trueの場合
            Else
                strView(12) = "0" 'falseの場合
            End If
            If Me.cbxHoleOnceRemoval.Checked Then               '一時撤去
                strView(13) = "1" 'trueの場合
            Else
                strView(13) = "0" 'falseの場合
            End If
            If Me.cbxHoleConChange.Checked Then                 '構成変更
                strView(14) = "1" 'trueの場合
            Else
                strView(14) = "0" 'falseの場合
            End If
            If Me.cbxHoleDlvOrgnz.Checked Then                  '構成配信
                strView(15) = "1" 'trueの場合
            Else
                strView(15) = "0" 'falseの場合
            End If
            If Me.cbxVup.Checked Then                           'バージョンアップ
                strView(28) = "1" 'trueの場合
            Else
                strView(28) = "0" 'falseの場合
            End If
            If Me.cbxHoleOther.Checked Then                     'その他
                strView(16) = "1" 'trueの場合
            Else
                strView(16) = "0" 'falseの場合
            End If
            'LAN工事種別
            If Me.cbxLanNew.Checked Then                        '新規
                strView(17) = "1" 'trueの場合
            Else
                strView(17) = "0" 'falseの場合
            End If
            If Me.cbxLanExpansion.Checked Then                  '増設
                strView(18) = "1" 'trueの場合
            Else
                strView(18) = "0" 'falseの場合
            End If
            If Me.cbxLanReInstallation.Checked Then             '再設置
                strView(19) = "1" 'trueの場合
            Else
                strView(19) = "0" 'falseの場合
            End If
            If Me.cbxLanShopRelocation.Checked Then             '店内移転
                strView(20) = "1" 'trueの場合
            Else
                strView(20) = "0" 'falseの場合
            End If
            If Me.cbxLanSomeRemoval.Checked Then                '一部撤去
                strView(21) = "1" 'trueの場合
            Else
                strView(21) = "0" 'falseの場合
            End If
            If Me.cbxLanAllRemoval.Checked Then                 '全撤去
                strView(22) = "1" 'trueの場合
            Else
                strView(22) = "0" 'falseの場合
            End If
            If Me.cbxLanOnceRemoval.Checked Then                '一時撤去
                strView(23) = "1" 'trueの場合
            Else
                strView(23) = "0" 'falseの場合
            End If
            If Me.cbxLanConChange.Checked Then                  '構成変更
                strView(24) = "1" 'trueの場合
            Else
                strView(24) = "0" 'falseの場合
            End If
            If Me.cbxLanDlvOrgnz.Checked Then                   '構成配信
                strView(25) = "1" 'trueの場合
            Else
                strView(25) = "0" 'falseの場合
            End If
            If Me.cbxLanOther.Checked Then                      'その他
                strView(26) = "1" 'trueの場合
            Else
                strView(26) = "0" 'falseの場合
            End If
            strView(27) = Me.txtSonotaNaiyo.ppText              'その他工事内容

            'ビューステートに設定
            Me.ViewState(M_VIEW_SEARCH) = strView

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
    ''' 工事依頼書兼仕様書 請求資料の追加/更新
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ms_mrg_ShiryoMeisai(ByVal Upladfile As String) As Boolean

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '初期化
        conDB = Nothing

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    '工事依頼書兼仕様書 請求資料明細
                    .Add(pfSet_Param("prm_kojiNum", SqlDbType.NVarChar, Me.txtKoziirai_IO.ppText))

                    '連番の新規設定時の処理
                    If Me.lblRenban_data.Text = String.Empty Then

                        .Add(pfSet_Param("prm_seq", SqlDbType.Int, 0))

                    Else

                        .Add(pfSet_Param("prm_seq", SqlDbType.Int, CInt(Me.lblRenban_data.Text)))

                    End If

                    .Add(pfSet_Param("prm_seikyu", SqlDbType.NVarChar, Me.txtSeikyusakiCD.ppText))
                    .Add(pfSet_Param("prm_shiryo", SqlDbType.NVarChar, Me.drpShiryouCD_IO.SelectedValue))
                    .Add(pfSet_Param("prm_shintyoku", SqlDbType.NVarChar, Me.drpSeikyuJokyo.SelectedValue))
                    If Me.txtJyuryoDate_IO.ppText = String.Empty Then
                        .Add(pfSet_Param("prm_juryobi", SqlDbType.NVarChar, String.Empty))
                    Else
                        .Add(pfSet_Param("prm_juryobi", SqlDbType.NVarChar, Me.txtJyuryoDate_IO.ppText + " " _
                                                                       + Me.txtJyuryoTime_IO.ppHourText + ":" _
                                                                       + Me.txtJyuryoTime_IO.ppMinText))
                    End If
                    If Me.fupKozisiryo.HasFile Then
                        .Add(pfSet_Param("prm_fileumu", SqlDbType.NVarChar, "1"))
                        .Add(pfSet_Param("prm_filemei", SqlDbType.NVarChar, Upladfile))
                    Else
                        .Add(pfSet_Param("prm_fileumu", SqlDbType.NVarChar, "0"))

                        If Me.ViewState("ファイル名") = String.Empty Then

                            .Add(pfSet_Param("prm_filemei", SqlDbType.NVarChar, ""))

                        Else

                            .Add(pfSet_Param("prm_filemei", SqlDbType.NVarChar, Me.ViewState("ファイル名")))

                        End If


                    End If
                    .Add(pfSet_Param("prm_biko", SqlDbType.NVarChar, Me.txtBiko_IO.ppText))
                    .Add(pfSet_Param("prm_userID", SqlDbType.NVarChar, Session(P_SESSION_USERID).ToString))

                End With

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

                Return True

            Catch ex As SqlException
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '更新に失敗
                Return False
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
                'システムエラー
                Return False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If

    End Function

    ''' <summary>
    ''' ホール情報からホール名を取得
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtHoleNm_TextChanged(sender As Object, e As EventArgs)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            'ホールコードを空にした場合
            If Me.txtHoleNm.ppText = String.Empty Then

                '表示をクリア
                Me.lblHoleNm.Text = Nothing

                '処理を終了
                Exit Sub

            End If

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Exit Sub
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB) 'すべての表示情報を取得
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_hallCd", SqlDbType.NVarChar, Me.txtHoleNm.ppText))             'ホールコード
                .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
            End With

            'データセット
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '結果情報を取得
            strOKNG = cmdDB.Parameters("data_exist").Value.ToString

            Select Case strOKNG
                Case "1"
                    '整合性OK
                    Me.lblHoleNm.Text = dstOrders.Tables(0).Rows(0).Item("ホール名").ToString()
                Case Else
                    '整合性エラー
                    Exit Sub
            End Select

        Catch ex As SqlException
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
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
        Finally

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub



    ''' <summary>
    ''' 工事別資料請求マスタのデータ取得
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtCnst_noChanged(sender As Object, e As EventArgs)

        'ドロップダウンリストの初期データを取得
        If Me.txtKoziirai_IO.ppText = String.Empty Then

            Me.drpShiryouCD_IO.Items.Clear()

        Else

            ms_GetDDLSiryou(Me.txtKoziirai_IO.ppText)

        End If

    End Sub

    ''' <summary>
    ''' 工事依頼書の存在チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function ms_serch_KojiNum()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet        '表示情報のデータセット
        Dim maxVal As Integer = 0
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                ms_serch_KojiNum = False
                Exit Function
            End If

            cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB) 'すべての表示情報を取得
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_kojiNum", SqlDbType.NVarChar, Me.txtKoziirai_IO.ppText))        '工事依頼番号
                .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
            End With

            'データセット
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            '結果情報を取得
            strOKNG = cmdDB.Parameters("data_exist").Value.ToString

            Select Case strOKNG
                Case "1"
                    '整合性OK
                    ms_serch_KojiNum = True
                Case Else
                    '整合性エラー
                    ms_serch_KojiNum = False

                    'エラーメッセージ
                    Me.txtKoziirai_IO.psSet_ErrorNo("2002", Me.txtKoziirai_IO.ppName)
                    Exit Function
            End Select

        Catch ex As SqlException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            ms_serch_KojiNum = False
        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            ms_serch_KojiNum = False
        Finally

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    '--2014/7/10 武 from
    ''' <summary>
    ''' 業者連番から業者名を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGetBilling(ByVal SEQ As String, ByRef offName As String)
        msGetBilling = ""

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            '接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Function
            End If

            cmdDB = New SqlCommand("CNSUPDP005_S7", conDB)
            With cmdDB.Parameters
                'パラメータ設定
                .Add(pfSet_Param("prm_SEQ", SqlDbType.Int, CType(SEQ, Integer)))        '業者連番
            End With

            'データセット
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count <> 0 Then
                offName = dstOrders.Tables(0).Rows(0).Item("業者名").ToString
            End If

            msGetBilling = "1"

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者連番")
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            msGetBilling = ""
        Finally

            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' ダウンロード情報追加
    ''' </summary>
    ''' <param name="strFileInf"></param>
    ''' <remarks></remarks>
    Private Sub ms_InsDownload(ByVal strFileInf() As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim strKanriNum As String = Nothing                                         '管理番号
        Dim strFileType As String = Nothing                                         'ファイル種別
        Dim strFileName As String = Nothing                                         'ファイル名
        Dim strReportName As String = Nothing                                       '帳票名
        Dim strServerName As String = Nothing                                       'サーバアドレス
        Dim strFilePath As String = Nothing                                         '保存先フォルダ
        Dim datCreateDate As DateTime = DateTime.Now                                '作成日
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        strFileName = strFileInf.Last
        strServerName = strFileInf.GetValue(2)

        '保存先フォルダ
        For i As Integer = 3 To strFileInf.Count - 2

            strFilePath += strFileInf(i)

            If i <> strFileInf.Count - 2 Then

                strFilePath += "\"

            End If

        Next



        strKanriNum = txtKoziirai_IO.ppText
        strFileType = "0281PT"
        strReportName = drpShiryouCD_IO.SelectedItem.Text


        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                '開始ログ出力
                psLogStart(Me)

                cmdDB = New SqlCommand("ZCMPINS001", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("MNG_NO", SqlDbType.NVarChar, strKanriNum))                            '管理番号
                    .Add(pfSet_Param("FILE_CLS", SqlDbType.NVarChar, strFileType))                          'ファイル種別
                    .Add(pfSet_Param("TITLE", SqlDbType.NVarChar, Master.Master.ppTitle))                          '画面タイトル
                    .Add(pfSet_Param("FILE_NM", SqlDbType.NVarChar, strFileName))                           'ファイル名
                    .Add(pfSet_Param("REPORT_NM", SqlDbType.NVarChar, strReportName))                       '帳票名
                    .Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.NVarChar, strServerName))                  'サーバアドレス
                    .Add(pfSet_Param("KEEP_FOLD", SqlDbType.NVarChar, strFilePath))                         '保存先フォルダ
                    .Add(pfSet_Param("CREATE_DT", SqlDbType.DateTime, datCreateDate))                       '作成日
                    .Add(pfSet_Param("INSERT_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID).ToString)) 'ユーザＩＤ
                End With

                'データ追加
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

            Catch ex As SqlException
                '追加に失敗
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
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
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
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

                '終了ログ出力
                psLogEnd(Me)

            End Try

        End If

    End Sub
    '--2014/7/10 武 to

#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' 検証チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_check_ISvalid(ByVal flag As String)

        Dim dtrErrMes As DataRow

        Select Case flag
            Case "1"

                'ホール名
                If Not Me.txtHoleNm.ppText Is Nothing And Me.lblHoleNm Is Nothing Then
                    Me.txtHoleNm.psSet_ErrorNo("2002", Me.txtHoleNm.ppNum)
                End If

            Case "2"

                '請求資料コード
                If Me.drpShiryouCD_IO.SelectedIndex = 0 Then

                    dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", Me.lblshiryo.Text)

                    vldShiryouCD.Text = "未入力エラー"
                    vldShiryouCD.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    vldShiryouCD.Visible = True
                    vldShiryouCD.Enabled = True
                    vldShiryouCD.IsValid = False

                End If

                '進捗状況
                If Me.drpSeikyuJokyo.SelectedIndex = 0 Then

                    dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", Me.lblSeikyujoukyou.Text)

                    vldSeikyuJokyo.Text = "未入力エラー"
                    vldSeikyuJokyo.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    vldSeikyuJokyo.Visible = True
                    vldSeikyuJokyo.Enabled = True
                    vldSeikyuJokyo.IsValid = False

                End If

                '進捗状況が受領済のときに受領日は必須入力
                If Me.drpSeikyuJokyo.SelectedItem.ToString = "受領済" Then
                    If Me.txtJyuryoDate_IO.ppText = "" Then

                        Me.txtJyuryoDate_IO.psSet_ErrorNo("5001", Me.txtJyuryoDate_IO.ppName)
                    End If

                    If Me.txtJyuryoTime_IO.ppHourText = "" Or
                       Me.txtJyuryoTime_IO.ppMinText = "" Then

                        Me.txtJyuryoTime_IO.psSet_ErrorNo("5001", Me.txtJyuryoTime_IO.ppName)
                    End If

                End If

        End Select

    End Sub

    ''' <summary>
    ''' ファイルアップロードコントロールの検証チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msCheck_Integrity(ByVal strExtension As String, ByVal Fullpath As HttpPostedFile)

        Dim dtrErrMes As DataRow

        'ファイルの拡張子エラー
        If Not strExtension = ".pdf" Then
            dtrErrMes = ClsCMCommon.pfGet_ValMes("4004", Me.drpShiryouCD_IO.SelectedItem.ToString + "の拡張子")

            valfileUpload.Text = "形式エラー"
            valfileUpload.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            valfileUpload.Enabled = True
            valfileUpload.IsValid = False
        End If

    End Sub

#End Region

End Class
