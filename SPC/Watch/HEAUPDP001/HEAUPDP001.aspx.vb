'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　ヘルスチェック詳細／更新
'*　ＰＧＭＩＤ：　HEAUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　14.02.05　：　(NKC)浜本
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient
#End Region

Public Class HEAUPDP001
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

    ''' <summary>
    ''' プログラムＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_DISP_ID As String = P_FUN_HEA & P_SCR_UPD & P_PAGE & "001"

    ''' <summary>
    ''' TBOXID無応答
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TBOXID_MUOUTOU As String = "ＴＢＯＸＩＤ無応答"

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================

    ''' <summary>
    ''' P-KEY格納項目
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum E_P_KEY項目
        照会管理番号 = 0
        連番
        NL区分
        IDIC区分
        データ受信日
        受信連番
        TBOXID
        ヘルスチェック結果
        新管理番号
    End Enum

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================

    ''' <summary>
    ''' TBOX随時照会画面相対パス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strREDIRECTPATH_TBOXZUIJI As String = "~/" & P_WAT & "/" & P_FUN_WAT & P_SCR_INQ & P_PAGE & "001/" & P_FUN_WAT & P_SCR_INQ & P_PAGE & "001.aspx"
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
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
    Private Sub HEAUPDP001_Init(sender As Object, e As EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_DISP_ID)
    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strTitle As String
        Master.ppProgramID = M_DISP_ID

        'タイトル設定
        strTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
        Master.ppTitle = strTitle

        'パンくずリスト
        Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

        'ボタンのメッセージダイアログ設定
        btnUpdate.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ヘルスチェック調査履歴")

        'ポストバック？
        If Not Me.IsPostBack() Then

            'ログ出力開始
            psLogStart(Me)

            'キー項目取得
            ViewState(P_KEY) = Session(P_KEY)
            ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)

            If ViewState(P_KEY) Is Nothing Then
                Return
            End If

            ''★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            'If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

            '    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            'End If
            'ページクリア
            Me.msPageClear()

            '調査者ドロップダウンリスト読み込み
            msSetDdlChosasha()

            '項目内容読み込み
            If Me.mfDataRead() = False Then
                Return
            End If

            '調査結果履歴情報一覧読み込み
            If Me.mfDataReadLst(ViewState(P_KEY)(E_P_KEY項目.TBOXID)) = False Then
                Return
            End If

            'ログ出力終了
            psLogEnd(Me)

        End If
    End Sub

    '---------------------------
    '2014/04/21 武 ここから
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
    '2014/04/21 武 ここまで
    '---------------------------

    ''' <summary>
    ''' TBOXリアル照会ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTboxRealShokai_Click(sender As Object, e As EventArgs) Handles btnTboxRealShokai.Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'ログ出力開始
            psLogStart(Me)

            'TBOXIDがない場合は処理を抜ける
            If lblTBoxId.Text.Trim = "" Then
                Return
            End If

            Dim strVal(0) As String
            strVal(0) = ViewState(P_KEY)(E_P_KEY項目.TBOXID)                  'TBOX_ID

            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            Session(P_SESSION_USERID) = ViewState(P_SESSION_USERID).ToString 'ユーザＩＤ
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照                       '処理区分
            Session(P_SESSION_OLDDISP) = M_DISP_ID                           '画面ID
            Session(P_KEY) = strVal

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
                            objStack.GetMethod.Name, strREDIRECTPATH_TBOXZUIJI, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            '画面遷移
            psOpen_Window(Me, strREDIRECTPATH_TBOXZUIJI)

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
            'ログ出力終了
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 調査登録ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        '全角変換
        txtChosaKekka.ppText = mfToWide(txtChosaKekka.ppText)

        'チェック
        If (Page.IsValid() = False) Then
            Return
        End If

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ログ出力開始
            psLogStart(Me)

            'ヘルスチェック調査報告書データの登録
            If mfDataWrite() = False Then
                Throw New Exception()
            End If

            '完了メッセ
            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "ヘルスチェック調査履歴")

            '再読み込み
            '項目内容読み込み
            Me.msPageClear()
            Me.mfDataRead()

            '調査結果履歴情報一覧読み込み
            Me.mfDataReadLst(ViewState(P_KEY)(E_P_KEY項目.TBOXID))

        Catch ex As Exception
            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ヘルスチェック調査履歴", ex.ToString)
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
            'ログ出力終了
            psLogEnd(Me)
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    '''  <summary>半角文字を全角文字に変換する。</summary>
    '''  <param name="strVal">対象の文字列</param>
    '''  <returns>変換後の文字列</returns>
    '''  <remarks>「\」、「"」、「'」も処理することができます。</remarks>
    Public Function mfToWide(ByVal strVal As String) As String
        Dim strRetVal As String

        strRetVal = strVal.Replace("\", "￥")
        strRetVal = strRetVal.Replace("""", Microsoft.VisualBasic.ChrW(8221))
        strRetVal = strRetVal.Replace("'", "’")
        strRetVal = Microsoft.VisualBasic.StrConv(strRetVal, Microsoft.VisualBasic.VbStrConv.Wide)

        Return strRetVal

    End Function

    ''' <summary>
    ''' ページクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear()

        'ラベル項目クリア
        mslblKmkClear()

        '一覧クリア
        msListClear()

        '項目のENABLE制御
        msSetKmkEnabled(ViewState(P_SESSION_TERMS))

    End Sub

    ''' <summary>
    ''' ラベル項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mslblKmkClear()
        lblKensu.Text = ""
        lblRealTutiCd.Text = ""                         ' リアル通知コード
        lblTBoxId.Text = ""                             ' TBOX_ID
        lblJotai1.Text = ""                             ' ヘルスチェック結果
        lblHallNm.Text = ""                             ' ホール名
        lblHotan.Text = ""                              ' 保担
        lblDataJusinDate.Text = ""                      ' データ受信日
        lblSosinRenban.Text = ""                        ' 送信通番
        lblKosinKeisiki.Text = ""                       ' 更新形式
        lblDataJusinTime.Text = ""                      ' データ受信時刻
        lblUnyoDate.Text = ""                           ' 運用日付
        lblHoseiKbn.Text = ""                           ' 補正区分
        lblJusinRenban.Text = ""                        ' 受信連番
        lblHasseiDate.Text = ""                         ' 発生日付
        lblHoseiJokyo.Text = ""                         ' 補正状況
        lblMotoJusinRenban.Text = ""                    ' 元の受信連番
        lblHasseiTime.Text = ""                         ' 発生時刻
        lblHantei.Text = ""                             ' 判定
        lblTBoxSbt.Text = ""                            ' TBOX種別
        lblTBoxMode.Text = ""                           ' T_BOXモード
        lblHoseiGenin.Text = ""                         ' 補正原因
        lblTBoxVer.Text = ""                            ' ＴＢＯＸバージョン
        lblTboxJotai.Text = ""                          ' ＴＢＯＸ状態
        lblLatestUpdDate.Text = ""                      ' 最新更新日
        lblTBoxUnyoDate.Text = ""                       ' 運用日付
        lblHcKenchiCd.Text = ""                         ' HC検知コード
        lblLatestUpdTime.Text = ""                      ' 最新更新時刻
        lblDataSbt.Text = ""                            ' データ種別
        lblSyosaiNaiyo.Text = ""                        ' 詳細内容
        lblAPHalt.Text = ""                             ' AP_HALT
        lblLanCardErr.Text = ""                         ' LANカード異常
        lblHosyu1.Text = ""                             ' 保守1操作有無
        lblOndoErr1.Text = ""                           ' 温度異常1
        lblHddErrFlg1.Text = ""                         ' HDD故障フラグ1
        lblHosyu2.Text = ""                             ' 保守2操作有無
        lblUpsChangeReq.Text = ""                       ' UPSバッテリー交換要求
        lblHddErrFlg2.Text = ""                         ' HDD故障フラグ2
        lblZenmenKagiKaiho.Text = ""                    ' 前面鍵扉開放有無
        lblDengenChange.Text = ""                       ' 電源ユニット異常
        lblHddErrFlg3.Text = ""                         ' HDD故障フラグ3
        lblKoumenKagiKaiho.Text = ""                    ' 後面鍵扉開放有無
        lblDengenFanAlerm.Text = ""                     ' 電源ファンアラーム
        lblHddErrFlg4.Text = ""                         ' HDD故障フラグ4
        lblYosinGendogaku.Text = ""                     ' 与信限度額超過有無
        lblCpuFanAlerm.Text = ""                        ' CPUファンアラーム
        lblHddNukeFlg1.Text = ""                        ' HDD抜けフラグ1
        lblIsagakuChosa.Text = ""                       ' 違差額超過有無
        lblLitiumLow.Text = ""                          ' リチウムLow
        lblHddNukeFlg2.Text = ""                        ' HDD抜けフラグ2
        lblSeisangakuChoka.Text = ""                    ' 精算額超過有無
        lblNikkadoLow.Text = ""                         ' ニッカドLow
        lblHddNukeFlg3.Text = ""                        ' HDD抜けフラグ3
        lblDaikibotenKoseiTusin.Text = ""               ' 大規模店構成通信異常
        lblKeyBordNuke.Text = ""                        ' キーボード抜け
        lblHddNukeFlg4.Text = ""                        ' HDD抜けフラグ4
        lblTboxKaisiWasure.Text = ""                    ' T_BOX開始忘れ
        lblBbmErr.Text = ""                             ' BBM故障
        lblTboxKaizanFound.Text = ""                    ' T_BOX改竄検出
        lblMcErr.Text = ""                              ' MC異常
        lblCidDbFlg.Text = ""                           ' C_ID_DBニアエンド検出フラグ
        lblNkDnpDbAllFlg.Text = ""                      ' 入金伝票番号DBニアエンド検出フラグ_全体
        lblNkDnpDbOffFlg.Text = ""                      ' 入金伝票番号DBニアエンド検出フラグ_オフ
        lblSeijoUntenStrtStp.Text = ""                  ' 正常運転中_運転停止中
        lblAcDengen.Text = ""                           ' AC電源有無
        lblTennaiSotiKoseiFlg.Text = ""                 ' 店内装置構成表変更フラグ
        lblKakuchoTboxSerialNoFlg.Text = ""             ' 拡張T_BOXシリアルNo変更フラグ
        lblMihakkenShohiFlg.Text = ""                   ' 未発券消費発生フラグ
        lblTBoxSoftVerFlg.Text = ""                     ' T_BOXソフトバージョン変更フラグ
        lblMiuketukekiSiyoFlg.Text = ""                 ' 未受付使用発生フラグ
        lblKojoSyukka.Text = ""                         ' 工場出荷取込
        lblJBHenko.Text = ""                            ' JB変更有無
        lblTatenSoti.Text = ""                          ' 他店装置取込
        lblBB2SerialNoHenko.Text = ""                   ' BB2シリアルNo変更有無
        lblChainTenSoti.Text = ""                       ' チェーン店装置取込
        lblKarijimeHaniGai.Text = ""                    ' 仮締め範囲外
        lblDairitenSoti.Text = ""                       ' 代理店装置取込
        lblTatenUketukeLog.Text = ""                    ' 他店受付ログ取込
        lblBBSerialKanrigai.Text = ""                   ' BBシリアル管理外取込
        lblOfflineSandKensyutu.Text = ""                ' オフラインサンド検出有無
        lblKagiKanrigai.Text = ""                       ' 鍵管理外取込
        lblNinsyoErrKensyutu.Text = ""                  ' 認証エラー検出
        lblDairitenSotiKensyutu.Text = ""               ' 代理店装置検出
        lblDenbunErrKensyutu.Text = ""                  ' 電文長エラー検出
        lblFukugoErrKensyutu.Text = ""                  ' 複合エラー検出
        lblBBSeriaruKanrigaiKensyutu.Text = ""          ' BBシリアル管理外検出
        lblResponseCdErrKensyutu.Text = ""              ' レスポンスコードエラー検出
        lblBLtaisyoErrKensyutu.Text = ""                ' BL対象エラー検出
        lblKoseihyoJBNoKensyutu.Text = ""               ' 構成表登録外JB番号検出
        lblKagikanrigaiKensyutu.Text = ""               ' 鍵管理外検出
        lblBBChofukuKensyutu.Text = ""                  ' BB重複接続エラー検出
        lblSetuzokufukaSyubetu.Text = ""                ' 接続不可機種種別検出
        lblSequenceErrKensyutu.Text = ""                ' シーケンス異常検出
        lblIpErrKensyutu.Text = ""                      ' IPアドレスエラー検出
        lblSetuzokufukaCd.Text = ""                     ' 接続不可機種コード検出
        lblBBSerialNoErrKensyutu.Text = ""              ' BB1シリアルNoエラー検出
        lblTatenSotiKensyutu.Text = ""                  ' 他店装置検出
        lblVersionErrKensyutu.Text = ""                 ' バージョンエラー検出
        lblCardkaisyaErrKensyutu.Text = ""              ' カード会社区分エラー検出
        lblChainSotiKensyutu.Text = ""                  ' チェーン店装置検出
        lblTaCardkaisyaSotiKensyutu.Text = ""           ' 他カード会社装置検出
        lblDenbunSequenceErrKensyutu.Text = ""          ' 電文シーケンス番号異常検出

        txtChosaKekka.ppText = ""                       ' 調査結果
        ddlChousasya.SelectedIndex = 0                  ' 調査者

    End Sub

    ''' <summary>
    ''' 一覧クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msListClear()
        '空の一覧を設定
        grvList.DataSource = New DataTable()
        lblKensu.Text = "0"
        grvList.DataBind()

        '調査者にフォーカス
        ddlChousasya.Focus()
    End Sub

    ''' <summary>
    ''' 項目ENABLE制御
    ''' </summary>
    ''' <param name="shrMode"></param>
    ''' <remarks></remarks>
    Private Sub msSetKmkEnabled(ByVal shrMode As Short)
        Select Case shrMode
            Case ClsComVer.E_遷移条件.参照
                ddlChousasya.Enabled = False
                txtChosaKekka.ppEnabled = False
                btnUpdate.Enabled = False
            Case ClsComVer.E_遷移条件.更新
                ddlChousasya.Enabled = True
                txtChosaKekka.ppEnabled = True
                btnUpdate.Enabled = True
            Case ClsComVer.E_遷移条件.登録
                ddlChousasya.Enabled = True
                txtChosaKekka.ppEnabled = True
                btnUpdate.Enabled = True
            Case Else
        End Select
    End Sub

    ''' <summary>
    ''' 調査者ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetDdlChosasha()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'プロシージャ設定
            objCmd = New SqlCommand("HEAUPDP001_S3")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            '空データを先頭に設定
            objDs.Tables(0).Rows.InsertAt(objDs.Tables(0).NewRow, 0)

            'ドロップダウンリストにバインド
            ddlChousasya.DataSource = objDs.Tables(0)
            ddlChousasya.DataTextField = "M02_SHORT_NM"
            ddlChousasya.DataValueField = "M02_EMPLOYEE_CD"
            ddlChousasya.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "調査者一覧")
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
            If clsDataConnect.pfClose_Database(objCon) = False Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        End Try
    End Sub

    ''' <summary>
    ''' 項目内容読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfDataRead() As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return False
            End If

            'プロシージャ設定
            objCmd = New SqlCommand("HEAUPDP001_S2")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD174_CTRL_NO", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.照会管理番号)))
                .Add(pfSet_Param("prmD174_SEQ", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.連番)))
                .Add(pfSet_Param("prmD174_NL_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.NL区分)))
                .Add(pfSet_Param("prmD174_ID_IC_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.IDIC区分)))
                .Add(pfSet_Param("prmD174_RECVDATE", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.データ受信日)))
                .Add(pfSet_Param("prmD174_RECVSEQ", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.受信連番)))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            '項目設定
            msSetKmkDisp(objDs.Tables(0))

            Return True

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ヘルスチェック詳細データ")
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
            If clsDataConnect.pfClose_Database(objCon) = False Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Function

    ''' <summary>
    ''' 調査履歴一覧読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfDataReadLst(ByVal strTboxId As String) As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return False
            End If

            'TBOXIDがない場合は処理しない
            If strTboxId Is Nothing OrElse strTboxId = "" Then
                grvList.DataSource = New DataTable()
                grvList.DataBind()
            End If

            'プロシージャ設定
            objCmd = New SqlCommand("HEAUPDP001_S1")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD174_TBOXID", SqlDbType.NVarChar, strTboxId)) 'TBOXID
                .Add(pfSet_Param("prmDetectCd", SqlDbType.NVarChar, lblHcKenchiCd.Text))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            '一覧にバインド、データ件数を設定
            grvList.DataSource = objDs.Tables(0)
            lblKensu.Text = objDs.Tables(0).Rows.Count
            grvList.DataBind()

            Return True

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "調査履歴一覧")
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
            If clsDataConnect.pfClose_Database(objCon) = False Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Function

    ''' <summary>
    ''' 表示データ設定
    ''' </summary>
    ''' <param name="objTbl"></param>
    ''' <remarks></remarks>
    Private Sub msSetKmkDisp(ByVal objTbl As DataTable)
        For Each objRow As DataRow In objTbl.Rows
            lblRealTutiCd.Text = objRow("リアル通知コード")                                         ' リアル通知コード
            lblTBoxId.Text = objRow("TBOX_ID")                                                      ' TBOX_ID
            lblJotai1.Text = objRow("状態１")                                                       ' ヘルスチェック結果
            lblHallNm.Text = objRow("ホール名")                                                     ' ホール名
            lblHotan.Text = objRow("保担")                                                          ' 保担
            lblDataJusinDate.Text = objRow("データ受信日")                                          ' データ受信日
            lblSosinRenban.Text = objRow("送信通番")                                                ' 送信通番
            lblKosinKeisiki.Text = objRow("更新形式")                                               ' 更新形式
            lblDataJusinTime.Text = objRow("データ受信時刻")                                        ' データ受信時刻
            lblUnyoDate.Text = objRow("運用日付")                                                   ' 運用日付
            lblHoseiKbn.Text = objRow("補正区分")                                                   ' 補正区分
            lblJusinRenban.Text = objRow("受信連番")                                                ' 受信連番
            lblHasseiDate.Text = objRow("発生日付")                                                 ' 発生日付
            lblHoseiJokyo.Text = objRow("補正状況")                                                 ' 補正状況
            lblMotoJusinRenban.Text = objRow("元の受信連番")                                        ' 元の受信連番
            lblHasseiTime.Text = objRow("発生時刻")                                                 ' 発生時刻
            lblHantei.Text = objRow("判定")                                                         ' 判定
            lblTBoxSbt.Text = objRow("TBOX種別")                                                    ' TBOX種別
            lblTBoxMode.Text = objRow("T_BOXモード")                                                ' T_BOXモード
            lblHoseiGenin.Text = objRow("補正原因")                                                 ' 補正原因
            lblTBoxVer.Text = objRow("ＴＢＯＸバージョン")                                          ' ＴＢＯＸバージョン
            lblTboxJotai.Text = objRow("ＴＢＯＸ状態")                                              ' ＴＢＯＸ状態
            lblLatestUpdDate.Text = objRow("最新更新日")                                            ' 最新更新日
            lblTBoxUnyoDate.Text = objRow("ＴＢＯＸ運用日付")                                       ' TBOX運用日付
            lblHcKenchiCd.Text = objRow("HC検知コード")                                             ' HC検知コード
            lblLatestUpdTime.Text = objRow("最新更新時刻")                                          ' 最新更新時刻
            lblDataSbt.Text = objRow("データ種別")                                                  ' データ種別
            lblSyosaiNaiyo.Text = objRow("詳細内容")                                                ' 詳細内容
            lblAPHalt.Text = objRow("AP_HALT")                                                      ' AP_HALT
            lblLanCardErr.Text = objRow("LANカード異常")                                            ' LANカード異常
            lblHosyu1.Text = objRow("保守1操作有無")                                                ' 保守1操作有無
            lblOndoErr1.Text = objRow("温度異常1")                                                  ' 温度異常1
            lblHddErrFlg1.Text = objRow("HDD故障フラグ1")                                           ' HDD故障フラグ1
            lblHosyu2.Text = objRow("保守2操作有無")                                                ' 保守2操作有無
            lblUpsChangeReq.Text = objRow("UPSバッテリー交換要求")                                  ' UPSバッテリー交換要求
            lblHddErrFlg2.Text = objRow("HDD故障フラグ2")                                           ' HDD故障フラグ2
            lblZenmenKagiKaiho.Text = objRow("前面鍵扉開放有無")                                    ' 前面鍵扉開放有無
            lblDengenChange.Text = objRow("電源ユニット異常")                                       ' 電源ユニット異常
            lblHddErrFlg3.Text = objRow("HDD故障フラグ3")                                           ' HDD故障フラグ3
            lblKoumenKagiKaiho.Text = objRow("後面鍵扉開放有無")                                    ' 後面鍵扉開放有無
            lblDengenFanAlerm.Text = objRow("電源ファンアラーム")                                   ' 電源ファンアラーム
            lblHddErrFlg4.Text = objRow("HDD故障フラグ4")                                           ' HDD故障フラグ4
            lblYosinGendogaku.Text = objRow("与信限度額超過有無")                                   ' 与信限度額超過有無
            lblCpuFanAlerm.Text = objRow("CPUファンアラーム")                                       ' CPUファンアラーム
            lblHddNukeFlg1.Text = objRow("HDD抜けフラグ1")                                          ' HDD抜けフラグ1
            lblIsagakuChosa.Text = objRow("違差額超過有無")                                         ' 違差額超過有無
            lblLitiumLow.Text = objRow("リチウムLow")                                               ' リチウムLow
            lblHddNukeFlg2.Text = objRow("HDD抜けフラグ2")                                          ' HDD抜けフラグ2
            lblSeisangakuChoka.Text = objRow("精算額超過有無")                                      ' 精算額超過有無
            lblNikkadoLow.Text = objRow("ニッカドLow")                                              ' ニッカドLow
            lblHddNukeFlg3.Text = objRow("HDD抜けフラグ3")                                          ' HDD抜けフラグ3
            lblDaikibotenKoseiTusin.Text = objRow("大規模店構成通信異常")                           ' 大規模店構成通信異常
            lblKeyBordNuke.Text = objRow("キーボード抜け")                                          ' キーボード抜け
            lblHddNukeFlg4.Text = objRow("HDD抜けフラグ4")                                          ' HDD抜けフラグ4
            lblTboxKaisiWasure.Text = objRow("T_BOX開始忘れ")                                       ' T_BOX開始忘れ
            lblBbmErr.Text = objRow("BBM故障")                                                      ' BBM故障
            lblTboxKaizanFound.Text = objRow("T_BOX改竄検出")                                       ' T_BOX改竄検出
            lblMcErr.Text = objRow("MC異常")                                                        ' MC異常
            lblCidDbFlg.Text = objRow("C_ID_DBニアエンド検出フラグ")                                ' C_ID_DBニアエンド検出フラグ
            lblNkDnpDbAllFlg.Text = objRow("入金伝票番号DBニアエンド検出フラグ_全体")               ' 入金伝票番号DBニアエンド検出フラグ_全体
            lblNkDnpDbOffFlg.Text = objRow("入金伝票番号DBニアエンド検出フラグ_オフライン")         ' 入金伝票番号DBニアエンド検出フラグ_オフ
            lblSeijoUntenStrtStp.Text = objRow("正常運転中_運転停止中")                             ' 正常運転中_運転停止中
            lblAcDengen.Text = objRow("AC電源有無")                                                 ' AC電源有無
            lblTennaiSotiKoseiFlg.Text = objRow("店内装置構成表変更フラグ")                         ' 店内装置構成表変更フラグ
            lblKakuchoTboxSerialNoFlg.Text = objRow("拡張T_BOXシリアルNo変更フラグ")                ' 拡張T_BOXシリアルNo変更フラグ
            lblMihakkenShohiFlg.Text = objRow("未発券消費発生フラグ")                               ' 未発券消費発生フラグ
            lblTBoxSoftVerFlg.Text = objRow("T_BOXソフトバージョン変更フラグ")                      ' T_BOXソフトバージョン変更フラグ
            lblMiuketukekiSiyoFlg.Text = objRow("未受付使用発生フラグ")                             ' 未受付使用発生フラグ
            lblKojoSyukka.Text = objRow("工場出荷取込")                                             ' 工場出荷取込
            lblJBHenko.Text = objRow("JB変更有無")                                                  ' JB変更有無
            lblTatenSoti.Text = objRow("他店装置取込")                                              ' 他店装置取込
            lblBB2SerialNoHenko.Text = objRow("BB2シリアルNo変更有無")                              ' BB2シリアルNo変更有無
            lblChainTenSoti.Text = objRow("チェーン店装置取込")                                     ' チェーン店装置取込
            lblKarijimeHaniGai.Text = objRow("仮締め範囲外")                                        ' 仮締め範囲外
            lblDairitenSoti.Text = objRow("代理店装置取込")                                         ' 代理店装置取込
            lblTatenUketukeLog.Text = objRow("他店受付ログ取込")                                    ' 他店受付ログ取込
            lblBBSerialKanrigai.Text = objRow("BBシリアル管理外取込")                               ' BBシリアル管理外取込
            lblOfflineSandKensyutu.Text = objRow("オフラインサンド検出有無")                        ' オフラインサンド検出有無
            lblKagiKanrigai.Text = objRow("鍵管理外取込")                                           ' 鍵管理外取込
            lblNinsyoErrKensyutu.Text = objRow("認証エラー検出")                                    ' 認証エラー検出
            lblDairitenSotiKensyutu.Text = objRow("代理店装置検出")                                 ' 代理店装置検出
            lblDenbunErrKensyutu.Text = objRow("電文長エラー検出")                                  ' 電文長エラー検出
            lblFukugoErrKensyutu.Text = objRow("複合エラー検出")                                    ' 複合エラー検出
            lblBBSeriaruKanrigaiKensyutu.Text = objRow("BBシリアル管理外検出")                      ' BBシリアル管理外検出
            lblResponseCdErrKensyutu.Text = objRow("レスポンスコードエラー検出")                    ' レスポンスコードエラー検出
            lblBLtaisyoErrKensyutu.Text = objRow("BL対象エラー検出")                                ' BL対象エラー検出
            lblKoseihyoJBNoKensyutu.Text = objRow("構成表登録外JB番号検出")                         ' 構成表登録外JB番号検出
            lblKagikanrigaiKensyutu.Text = objRow("鍵管理外検出")                                   ' 鍵管理外検出
            lblBBChofukuKensyutu.Text = objRow("BB重複接続エラー検出")                              ' BB重複接続エラー検出
            lblSetuzokufukaSyubetu.Text = objRow("接続不可機種種別検出")                            ' 接続不可機種種別検出
            lblSequenceErrKensyutu.Text = objRow("シーケンス異常検出")                              ' シーケンス異常検出
            lblIpErrKensyutu.Text = objRow("IPアドレスエラー検出")                                  ' IPアドレスエラー検出
            lblSetuzokufukaCd.Text = objRow("接続不可機種コード検出")                               ' 接続不可機種コード検出
            lblBBSerialNoErrKensyutu.Text = objRow("BB1シリアルNoエラー検出")                       ' BB1シリアルNoエラー検出
            lblTatenSotiKensyutu.Text = objRow("他店装置検出")                                      ' 他店装置検出
            lblVersionErrKensyutu.Text = objRow("バージョンエラー検出")                             ' バージョンエラー検出
            lblCardkaisyaErrKensyutu.Text = objRow("カード会社区分エラー検出")                      ' カード会社区分エラー検出
            lblChainSotiKensyutu.Text = objRow("チェーン店装置検出")                                ' チェーン店装置検出
            lblTaCardkaisyaSotiKensyutu.Text = objRow("他カード会社装置検出")                       ' 他カード会社装置検出
            lblDenbunSequenceErrKensyutu.Text = objRow("電文シーケンス番号異常検出")                ' 電文シーケンス番号異常検出

            '定型文セット
            msSetChosaKekkaMoji(objRow)
        Next
    End Sub

    ''' <summary>
    ''' 定型文設定
    ''' </summary>
    ''' <param name="objRow"></param>
    ''' <remarks></remarks>
    Private Sub msSetChosaKekkaMoji(ByVal objRow As DataRow)
        Dim strRetVal As String = ""

        '無応答の場合、「ＴＢＯＸ無応答」を定型文に追加
        If ViewState(P_KEY)(E_P_KEY項目.ヘルスチェック結果) = TBOXID_MUOUTOU Then
            strRetVal &= TBOXID_MUOUTOU & Microsoft.VisualBasic.vbCrLf
        End If
        Dim strRow As String()

        strRow = {"AP_HALT", _
                "LANカード異常", _
                "保守1操作有無", _
                "温度異常1", _
                "HDD故障フラグ1", _
                "保守2操作有無", _
                "UPSバッテリー交換要求", _
                "HDD故障フラグ2", _
                "前面鍵扉開放有無", _
                "電源ユニット異常", _
                "HDD故障フラグ3", _
                "後面鍵扉開放有無", _
                "電源ファンアラーム", _
                "HDD故障フラグ4", _
                "与信限度額超過有無", _
                "CPUファンアラーム", _
                "HDD抜けフラグ1", _
                "違差額超過有無", _
                "リチウムLow", _
                "HDD抜けフラグ2", _
                "精算額超過有無", _
                "ニッカドLow", _
                "HDD抜けフラグ3", _
                "大規模店構成通信異常", _
                "キーボード抜け", _
                "HDD抜けフラグ4", _
                "T_BOX開始忘れ", _
                "BBM故障", _
                "T_BOX改竄検出", _
                "MC異常", _
                "C_ID_DBニアエンド検出フラグ", _
                "入金伝票番号DBニアエンド検出フラグ_全体", _
                "入金伝票番号DBニアエンド検出フラグ_オフライン", _
                "正常運転中_運転停止中", _
                "AC電源有無", _
                "店内装置構成表変更フラグ", _
                "拡張T_BOXシリアルNo変更フラグ", _
                "未発券消費発生フラグ", _
                "T_BOXソフトバージョン変更フラグ", _
                "未受付使用発生フラグ", _
                "工場出荷取込", _
                "JB変更有無", _
                "他店装置取込", _
                "BB2シリアルNo変更有無", _
                "チェーン店装置取込", _
                "仮締め範囲外", _
                "代理店装置取込", _
                "他店受付ログ取込", _
                "BBシリアル管理外取込", _
                "オフラインサンド検出有無", _
                "鍵管理外取込", _
                "認証エラー検出", _
                "代理店装置検出", _
                "電文長エラー検出", _
                "複合エラー検出", _
                "BBシリアル管理外検出", _
                "レスポンスコードエラー検出", _
                "BL対象エラー検出", _
                "構成表登録外JB番号検出", _
                "鍵管理外検出", _
                "BB重複接続エラー検出", _
                "接続不可機種種別検出", _
                "シーケンス異常検出", _
                "IPアドレスエラー検出", _
                "接続不可機種コード検出", _
                "BB1シリアルNoエラー検出", _
                "他店装置検出", _
                "バージョンエラー検出", _
                "カード会社区分エラー検出", _
                "チェーン店装置検出", _
                "他カード会社装置検出", _
                "電文シーケンス番号異常検出"}

        For Each strVal As String In strRow
            If objRow(strVal) = "1" Then
                If (objRow("D174_ID_IC_CLS").ToString = "3" And objRow("ＴＢＯＸバージョン").ToString.Substring(0, 1) >= "F") _
                Or (objRow("D174_ID_IC_CLS").ToString = "5" And objRow("ＴＢＯＸバージョン").ToString.Substring(0, 1) >= "W") Then
                    Select Case strVal
                        Case "LANカード異常"
                            strRetVal &= "ＬＡＮカード異常検知(ﾌﾟﾘﾝﾀ･UPS側)" & Microsoft.VisualBasic.vbCrLf
                        Case "HDD故障フラグ1"
                            strRetVal &= "ＳＳＤ１故障フラグ" & Microsoft.VisualBasic.vbCrLf
                        Case "HDD故障フラグ3"
                            strRetVal &= "ＳＳＤ２故障フラグ" & Microsoft.VisualBasic.vbCrLf
                        Case "保守2操作有無"
                            strRetVal &= "ＯＳライセンス認証実施有無" & Microsoft.VisualBasic.vbCrLf
                        Case "後面鍵扉開放有無"
                            strRetVal &= "Ｃｆａｓｔ抜け検知" & Microsoft.VisualBasic.vbCrLf
                        Case "電源ファンアラーム"
                            strRetVal &= "筐体ファン２アラーム" & Microsoft.VisualBasic.vbCrLf
                        Case "CPUファンアラーム"
                            strRetVal &= "筐体ファン１アラーム" & Microsoft.VisualBasic.vbCrLf
                        Case "HDD抜けフラグ1"
                            strRetVal &= "ＳＳＤ１抜けフラグ" & Microsoft.VisualBasic.vbCrLf
                        Case "HDD抜けフラグ3"
                            strRetVal &= "ＳＳＤ２抜けフラグ" & Microsoft.VisualBasic.vbCrLf
                        Case "ニッカドLow"
                            strRetVal &= "ＬＡＮカード異常検知(店内装置側)" & Microsoft.VisualBasic.vbCrLf
                        Case "MC異常"
                            strRetVal &= "Ｃｆａｓｔ異常" & Microsoft.VisualBasic.vbCrLf
                        Case Else
                            strRetVal &= strVal & Microsoft.VisualBasic.vbCrLf
                    End Select
                Else
                    strRetVal &= strVal & Microsoft.VisualBasic.vbCrLf
                End If
            End If
        Next

        txtChosaKekka.ppText = strRetVal

    End Sub

    ''' <summary>
    ''' ヘルスチェック調査報告書の登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfDataWrite() As Boolean
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim objTran As SqlTransaction = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return False
            End If

            'トランザクション開始
            objTran = objCon.BeginTransaction()

            'ストアド設定
            objCmd = New SqlCommand("HEAUPDP001_U1")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Transaction = objTran

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD179_CTRL_NO", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.照会管理番号)))
                .Add(pfSet_Param("prmD179_SEQ", SqlDbType.Int, CType(ViewState(P_KEY), String())(E_P_KEY項目.連番)))
                .Add(pfSet_Param("prmD179_NL_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.NL区分)))
                .Add(pfSet_Param("prmD179_ID_IC_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.IDIC区分)))
                .Add(pfSet_Param("prmD179_RECVDATE", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.データ受信日)))
                .Add(pfSet_Param("prmD179_RECVSEQ", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.受信連番)))
                .Add(pfSet_Param("prmInvestUser", SqlDbType.NVarChar, ddlChousasya.SelectedItem.Text))
                .Add(pfSet_Param("prmInvestResult", SqlDbType.NVarChar, txtChosaKekka.ppText))
                .Add(pfSet_Param("prmInsertUser", SqlDbType.NVarChar, "SYSTEM"))
                .Add(pfSet_Param("prmTboxId", SqlDbType.NVarChar, lblTBoxId.Text))
                .Add(pfSet_Param("prmEmpCd", SqlDbType.NVarChar, ddlChousasya.SelectedValue.ToString))
            End With

            '登録
            Dim intRet As Integer = objCmd.ExecuteNonQuery()

            '登録失敗？
            If intRet <= 0 Then
                Throw New Exception()
            End If

            '未対応のデータを対象とする
            If CType(ViewState(P_KEY), String())(E_P_KEY項目.新管理番号) Is Nothing _
                Or CType(ViewState(P_KEY), String())(E_P_KEY項目.新管理番号) = String.Empty Then

                'ストアド設定
                objCmd = New SqlCommand("HEAUPDP001_U2")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure
                objCmd.Transaction = objTran

                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("prmD174_CTRL_NO", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.照会管理番号)))
                    .Add(pfSet_Param("prmD174_SEQ", SqlDbType.Int, CType(ViewState(P_KEY), String())(E_P_KEY項目.連番)))
                    .Add(pfSet_Param("prmD174_NL_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.NL区分)))
                    .Add(pfSet_Param("prmD174_ID_IC_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.IDIC区分)))
                    .Add(pfSet_Param("prmD174_RECVDATE", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.データ受信日)))
                    .Add(pfSet_Param("prmD174_RECVSEQ", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.受信連番)))
                    .Add(pfSet_Param("prmD174_NEW_CTRL_NO", SqlDbType.Char, "1"))
                    .Add(pfSet_Param("prmInsertUser", SqlDbType.NVarChar, "SYSTEM"))
                End With

                '登録
                intRet = objCmd.ExecuteNonQuery()

                '登録失敗？
                If intRet <= 0 Then
                    Throw New Exception()
                End If

            End If


            'ストアド設定
            objCmd = New SqlCommand("HEAUPDP001_U3")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Transaction = objTran

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD174_TBOXID", SqlDbType.NVarChar, lblTBoxId.Text))
                .Add(pfSet_Param("prmD174_CTRL_NO", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.照会管理番号)))
                .Add(pfSet_Param("prmD174_SEQ", SqlDbType.Int, CType(ViewState(P_KEY), String())(E_P_KEY項目.連番)))
                .Add(pfSet_Param("prmD174_NL_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.NL区分)))
                .Add(pfSet_Param("prmD174_ID_IC_CLS", SqlDbType.NVarChar, CType(ViewState(P_KEY), String())(E_P_KEY項目.IDIC区分)))
                .Add(pfSet_Param("prmD174_RECVDATE", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.データ受信日)))
                .Add(pfSet_Param("prmD174_RECVSEQ", SqlDbType.Char, CType(ViewState(P_KEY), String())(E_P_KEY項目.受信連番)))
            End With

            '登録
            intRet = objCmd.ExecuteNonQuery()

            '登録失敗？
            If intRet <= 0 Then
                Throw New Exception()
            End If


            'コミット
            objTran.Commit()

            Return True
        Catch ex As Exception
            'ロールバック
            If Not objTran Is Nothing Then
                objTran.Rollback()
            End If

            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "調査履歴登録", ex.ToString)
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
            If clsDataConnect.pfClose_Database(objCon) = False Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
