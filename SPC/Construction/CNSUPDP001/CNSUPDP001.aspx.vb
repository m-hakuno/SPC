'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事依頼書兼仕様書（参照／更新／確定／仮登録）
'*　ＰＧＭＩＤ：　CNSUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.09　：　酒井
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSUPDP001-001     2015/05/14      加賀      総合テスト・仮設置　日時形式チェック     
'CNSUPDP001_002     2015/06/01      武        DLL削除処理追加
'CNSUPDP001_003     2015/06/10      武        DLL変更なしチェックボックスを常時表示
'CNSUPDP001_004     2015/07/16      武        仮ODでもDLL変更を行えるよう修正
'CNSUPDP001_005     2015/09/02      武        ラベル登録者にユーザーIDではなく、社員名を表示するよう修正
'CNSUPDP001_006     2015/09/18      加賀      進捗状況の判定条件を変更(名称→コード)
'CNSUPDP001_007     2015/10/23      加賀      画面展開時の仮依頼番号発番を廃止　管理番号種別変更[1→5]
'CNSUPDP001_008     2015/10/28      加賀      受付確定時の案件ステータス設定を変更
'CNSUPDP001_009     2015/10/28      加賀      設置日の空文字→NULL変換
'CNSUPDP001_010     2015/10/28      加賀      仮依頼のTBOXID形式チェック追加
'CNSUPDP001_011     2015/11/02      加賀      参照モードでの表示バグ修正
'CNSUPDP001_012     2015/11/02      加賀      受付確定時の営業所コードの整合性チェック追加
'CNSUPDP001_013     2015/11/09      加賀      登録処理のエラー処理を変更
'CNSUPDP001_014     2016/01/12      加賀      緊急依頼表示の追加
'CNSUPDP001_015     2016/01/25      伯野      帳票の打ち込み情報をマスタから取得
'CNSUPDP001_016     2016/04/04      加賀      更新時の随時一覧の更新/削除処理追加
'CNSUPDP001_017     2016/04/18      加賀      休日受けの場合、翌営業日受けとして緊急依頼判定する
'CNSUPDP001_018     2016/04/22      加賀      「元に戻す」ボタン押下時に「作業担当者」を設定する。
'CNSUPDP001_019     2016/07/13      栗原      機器マスタ[印刷区分]追加に伴う修正(ID系帳票のヘッダ文言)
'CNSUPDP001_020     2016/06/16      栗原      「双子店区分」「MDN機器」を更新項目に追加、双子区分を0か1入力に変更
'CNSUPDP001_021     2016/08/05      栗原      「双子店区分」「MDN機器」は本画面から更新しない。
'CNSUPDP001_022     2016/09/08      栗原      FS稼働有無が空欄だった際に、0を登録するように修正
'CNSUPDP001_023     2017/03/21      加賀      帳票の工事機器が表示上限を超えた場合、複数枚出力する(IC/LUTEのみ)
'CNSUPDP001_024     2017/06/13      伯野      工事機器一覧をラベル表示に変更
'CNSUPDP001_025     2018/07/19      伯野      Ｅマネー項目を非表示。ＦＳ稼動有無等の文言を表示


#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.DataSet
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataLink
'Imports SPC.Global_asax
Imports SPC.CNSUPDP001_D39_CNSTREQSPEC
Imports SPC.CNSUPDP001_D47_DLLSEND
Imports SPC.CNSUPDP001_D61_CNSTREQSPEC_HIST

'★排他制御用
Imports SPC.ClsCMExclusive

#End Region

Public Class CNSUPDP001
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
    Const sCnsErr_30008 As String = "30008"
    '--------------------------------
    '2014/04/30 高松　ここから
    '--------------------------------
    Const M_DISP_ID As String = P_FUN_CNS & P_SCR_UPD & P_PAGE & "001"
    Const M_DISPUPD003_ID As String = P_FUN_CNS & P_SCR_UPD & P_PAGE & "003"
    '--------------------------------
    '2014/04/30 高松　ここまで
    '--------------------------------
    '工事依頼書兼仕様書 画面のパス
    Const sCnsCNSUPDP001 As String = "~/" & P_CNS & "/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "001/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "001.aspx"

    '物品転送依頼書 参照/更新画面のパス
    Const sCnsCNSUPDP002 As String = "~/" & P_CNS & "/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "002/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "002.aspx"

    '即時集信一覧画面のパス
    Const sCnsMSTLSTP001 As String = "~/" & P_CNS & "/" &
                                        P_FUN_MST & P_SCR_LST & P_PAGE & "001/" &
                                        P_FUN_MST & P_SCR_LST & P_PAGE & "001.aspx"
    '--------------------------------
    '2014/06/16 武　ここから
    '--------------------------------
    'TBOX随時照会のパス
    Const sCnsWATINQP001 As String = "~/" & P_WAT & "/" &
                                        P_FUN_WAT & P_SCR_INQ & P_PAGE & "001/" &
                                        P_FUN_WAT & P_SCR_INQ & P_PAGE & "001.aspx"
    '--------------------------------
    '2014/06/16 武　ここまで
    '--------------------------------
    'シリアル登録画面のパス
    Const sCnsSERUPDP001 As String = "~/" & P_CNS & "/" &
                                        P_FUN_SER & P_SCR_UPD & P_PAGE & "001/" &
                                        P_FUN_SER & P_SCR_UPD & P_PAGE & "001.aspx"

    '資料請求状況更新画面のパス
    Const sCnsCNSUPDP005 As String = "~/" & P_CNS & "/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "005/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "005.aspx"

    '工事連絡票一覧画面のパス
    Const sCnsCNSLSTP005 As String = "~/" & P_CNS & "/" &
                                         P_FUN_CNS & P_SCR_LST & P_PAGE & "005/" &
                                         P_FUN_CNS & P_SCR_LST & P_PAGE & "005.aspx"

    '工事完了報告書画面のパス
    Const sCnsCNSUPDP006 As String = "~/" & P_CNS & "/" &
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "006/" &
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "006.aspx"


    'アップロード画面のパス
    Const sCnsDLCINPP001 As String = "~/" & P_COM & "/" &
                                         P_FUN_DLC & P_SCR_INP & P_PAGE & "001/" &
                                         P_FUN_DLC & P_SCR_INP & P_PAGE & "001.aspx"

    '工事進捗　参照更新画面のパス
    Const sCnsCNSUPDP003 As String = "~/" & P_CNS & "/" &
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "003/" &
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "003.aspx"

    '一覧ボタン名
    '-----------------------------
    '2014/05/09 土岐　ここから
    '-----------------------------
    'Const sCnsbtnReference As String = "btnReference"       '参照
    'Const sCnsbtnUpdate As String = "btnUpdate"             '更新
    'Const sCnsbtnProgress As String = "btnProgress"         '進捗管理
    '-----------------------------
    '2014/05/09 土岐　ここまで
    '-----------------------------

    Const sCnsProgid As String = "CNSUPDP001"
    Const sCnsSqlid_S1 As String = "CNSUPDP001_S1"
    Const sCnsSqlid_S2 As String = "CNSUPDP001_S2"
    Const sCnsSqlid_S3 As String = "CNSUPDP001_S3"
    Const sCnsSqlid_S4 As String = "CNSUPDP001_S4"
    Const sCnsSqlid_S5 As String = "CNSUPDP001_S5"
    Const sCnsSqlid_S6 As String = "CNSUPDP001_S6"
    Const sCnsSqlid_S7 As String = "CNSUPDP001_S7"
    Const sCnsSqlid_S8 As String = "CNSUPDP001_S8"
    Const sCnsSqlid_S10 As String = "CNSUPDP001_S10"
    Const sCnsSqlid_S11 As String = "CNSUPDP001_S11"
    Const sCnsSqlid_S12 As String = "CNSUPDP001_S12"
    Const sCnsSqlid_S13 As String = "CNSUPDP001_S13"
    Const sCnsSqlid_Z9 As String = "ZCMPSEL009"
    Const sCnsSqlid_I1 As String = "CNSUPDP001_I1"
    Const sCnsSqlid_I2 As String = "CNSUPDP001_I2"
    Const sCnsSqlid_I3 As String = "CNSUPDP001_I3"
    Const sCnsSqlid_I4 As String = "CNSUPDP001_I4"
    Const sCnsSqlid_I5 As String = "CNSUPDP001_I5"
    Const sCnsSqlid_I6 As String = "CNSUPDP001_I6"
    Const sCnsSqlid_U1 As String = "CNSUPDP001_U1"
    Const sCnsSqlid_U2 As String = "CNSUPDP001_U2"
    Const sCnsSqlid_U3 As String = "CNSUPDP001_U3"
    Const sCnsSqlid_D1 As String = "CNSUPDP001_D1"
    Const sCnsSqlid_D2 As String = "CNSUPDP001_D2"
    Const sCnsSqlid_D3 As String = "CNSUPDP001_D3"
    Const sCnsSqlid_S16 As String = "CNSUPDP001_S16"
    Const sCnsSqlid_S27 As String = "CNSUPDP001_S27"

    Const sCnsBupinButon As String = "物品転送依頼"
    Const sCnsTboxButon As String = "ＴＢＯＸ照会"
    Const sCnsShiriaruButon As String = "シリアル登録"
    Const sCnsshiryouButon As String = "資料請求"
    Const sCnsKoujirenButon As String = "工事連絡票一覧"
    Const sCnsKanryouButon As String = "完了報告書"
    Const sCnsShinchokuButon As String = "工事進捗"
    Const sCnsUploodButon As String = "アップロード"
    Const sCnsPrintButon As String = "印刷"
    Const sCnsKakuteiButon As String = "受付確定"
    Const sCnsAddButon As String = "登録"
    Const sCnsUpButon As String = "更新"
    Const sCnsClrButon As String = "クリア"
    Const sCnsBakeButon As String = "元に戻す"

    Const sCnsRequestNo As String = "N0090"        '仮依頼番号固定

    Const M_STE_TMP_DT As String = "本設置仮設置判断"
    '-----------------------------
    '2014/05/27 土岐　ここから
    '-----------------------------
    Const M_TMPSTTS_CD As String = "仮設置進捗ステータス"
    Const M_STATUS_CD As String = "本設置進捗ステータス"
    '-----------------------------
    '2014/05/27 土岐　ここまで
    '-----------------------------
    Private Const M_VIEW_DATECNT = "営業日日数"                         '営業日日数
#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================

    'チェックボックスＯＮ・ＯＦＦ
    Public Enum Enum_ChkBox As Short
        Enum_FALSE = 0
        Enum_TRUE = 1
    End Enum

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================

    'Public WithEvents txtTboxId_TextChanged As TextBox
    'Public Event TextChanged As EventHandler

    Public CNSUPDP001_D39_CNSTREQSPEC As CNSUPDP001_D39_CNSTREQSPEC
    Public CNSUPDP001_D47_DLLSEND As CNSUPDP001_D47_DLLSEND
    Public CNSUPDP001_D61_CNSTREQSPEC_HIST As CNSUPDP001_D61_CNSTREQSPEC_HIST
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim strBtnSts As Integer = 0 '1:受付確定、登録、更新 2:依頼番号紐付け
    Dim grvList2 As New GridView
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive
    Dim strIDIC As String = Nothing

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

#Region "ページ初期処理"
    Dim DataSet_CNSUPDP001 As DataTable

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, sCnsProgid, sCnsProgid + "_Header", Me.DivOut, "L")

        trfAgencyNm.ppURL = "~/Common/COMSELP002/COMSELP002.aspx"
        trfAgencyShop.ppURL = "~/Common/COMSELP002/COMSELP002.aspx"
        trfSendingStation.ppURL = "~/Common/COMSELP002/COMSELP002.aspx"

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
            Dim strKey() As String = Nothing
            Dim KeyCode() As String = Nothing
            Dim strTerms As String = String.Empty
            Dim strMsgNoS As String = String.Empty
            Dim strMsgNoE As String = String.Empty
            Dim strMsg As String = String.Empty
            Dim dstOrders_1 As New DataSet
            Dim dstOrders_2 As New DataSet
            Dim dstOrders_3 As New DataSet
            Dim dstOrders_4 As New DataSet
            Dim dstOrders_5 As New DataSet

            'ポップアップ表示設定
            KeyCode = {"2", Nothing, Nothing}
            trfSfPersonInCharge.ppKeyCode = KeyCode   'NGC

            'ボタンアクションの設定
            AddHandler Master.ppLeftButton1.Click, AddressOf Button_Click   '物品転送依頼
            AddHandler Master.ppLeftButton2.Click, AddressOf Button_Click   'ＴＢＯＸ照会
            AddHandler Master.ppLeftButton3.Click, AddressOf Button_Click   'シリアル登録
            AddHandler Master.ppLeftButton4.Click, AddressOf Button_Click   '資料請求
            AddHandler Master.ppLeftButton5.Click, AddressOf Button_Click   '工事連絡票一覧
            AddHandler Master.ppLeftButton6.Click, AddressOf Button_Click   '完了報告書
            AddHandler Master.ppRigthButton6.Click, AddressOf Button_Click  '工事進捗
            AddHandler Master.ppRigthButton5.Click, AddressOf Button_Click  'アップロード
            AddHandler Master.ppRigthButton4.Click, AddressOf Button_Click  '印刷
            AddHandler Master.ppRigthButton3.Click, AddressOf Button_Click  '受付確定／登録／更新
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click  'クリア
            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click  '元に戻す
            AddHandler Me.btmAttachRequestNo.Click, AddressOf Button_Click  '依頼番号紐付け

            AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf TextBox_TextChanged
            txtTboxId.ppTextBox.AutoPostBack = True
            AddHandler Me.txtOfficCD.ppTextBox.TextChanged, AddressOf txtOfficCD_TextChanged
            AddHandler Me.txtCurrentSys.ppTextBox.TextChanged, AddressOf getSystemGrp
            Me.txtCurrentSys.ppTextBox.AutoPostBack = True
            AddHandler Me.txtSfOperation.ppTextBox.TextChanged, AddressOf FS_WrkCls_TextChanged
            Me.txtSfOperation.ppTextBox.AutoPostBack = True

            '仕様連絡区分　１：新規登録　２：変更通知　３：キャンセル連絡
            AddHandler Me.txtSpecificationConnectingDivision.ppTextBox.TextChanged, AddressOf txtSpecificationConnectingDivision_Lostfocus
            Me.txtSpecificationConnectingDivision.ppTextBox.AutoPostBack = True
            'システム分類　１：磁気　３：ＩＣ　５：ＬＵＴＥＲＮＡ
            AddHandler Me.txtSysClassification.ppTextBox.TextChanged, AddressOf txtSysClassification_Lostfocus
            Me.txtSysClassification.ppTextBox.AutoPostBack = True
            'ホールストック内
            AddHandler Me.txtStkHoleIn.ppTextBox.TextChanged, AddressOf txtStkHoleIn_Lostfocus
            Me.txtStkHoleIn.ppTextBox.AutoPostBack = True
            'ホールストック外
            AddHandler Me.txtStkHoleOut.ppTextBox.TextChanged, AddressOf txtStkHoleOut_Lostfocus
            Me.txtStkHoleOut.ppTextBox.AutoPostBack = True
            'ＦＳ稼動有無　０：有り　１：無し
            'Me.txtSfOperation.ppTextBox.AutoPostBack = True
            'ＴＢＯＸ持帰り　０：なし　１：あり
            AddHandler Me.txtTboxTakeawayDivision.ppTextBox.TextChanged, AddressOf txtTboxTakeawayDivision_Lostfocus
            Me.txtTboxTakeawayDivision.ppTextBox.AutoPostBack = True
            '双子店区分　１：双子店
            AddHandler Me.txtTwinsShop.ppTextBox.TextChanged, AddressOf txtTwinsShop_Lostfocus
            Me.txtTwinsShop.ppTextBox.AutoPostBack = True
            '単独工事区分　０：単独工事　１：同時工事
            AddHandler Me.txtIndependentCns.ppTextBox.TextChanged, AddressOf txtIndependentCns_Lostfocus
            Me.txtIndependentCns.ppTextBox.AutoPostBack = True
            '親子区分　１：親ホール　２：子ホール
            AddHandler Me.txtPAndCDivision.ppTextBox.TextChanged, AddressOf txtPAndCDivision_Lostfocus
            Me.txtPAndCDivision.ppTextBox.AutoPostBack = True
            'ＶＥＲＵＰ日付区分　０：なし　１：同日　２：なし
            AddHandler Me.dttVerupDtDivision.ppTextBox.TextChanged, AddressOf dttVerupDtDivision_Lostfocus
            Me.dttVerupDtDivision.ppTextBox.AutoPostBack = True



            '            Me.txtTboxTakeawayDivision.ppTextBox.Attributes.Add("onBlur", "JPNDisp(cphMainContent_txtTboxTakeawayDivision_txtTextBox,cphMainContent_lblTboxTakeawayDivision);")

            If Not IsPostBack Or strBtnSts = 1 Or strBtnSts = 2 Then  '初回表示または受付確定、更新、登録ボタン押下時

                '業者情報参照ボタンのリンク先設定
                btnTrader.OnClientClick =
                    "return window_open('" & VirtualPathUtility.ToAbsolute("~/Common/COMSELP002/COMSELP002.aspx") & "')"


                '初回表示
                If IsPostBack = False Then
                    'セッション項目取得
                    ViewState(P_KEY) = Session(P_KEY)
                    ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)
                    If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then
                        Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)
                    End If

                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
                        'ホール内工事初期値0
                        Me.txtHoleNew.ppText = "0"                     '新規
                        Me.txtHoleExpansion.ppText = "0"               '増設
                        Me.txtHoleSomeRemoval.ppText = "0"             '一部撤去
                        Me.txtHoleShopRelocation.ppText = "0"          '店舗移設
                        Me.txtHoleAllRemoval.ppText = "0"              '全撤去
                        Me.txtHoleOnceRemoval.ppText = "0"             '一時撤去
                        Me.txtHoleReInstallation.ppText = "0"          '再設置
                        Me.txtHoleConChange.ppText = "0"               '構成変更
                        Me.txtHoleConDelively.ppText = "0"             '構成配信
                        Me.txtHoleOther.ppText = "0"                   'その他
                    End If
                End If

                'ビューステート項目取得
                strKey = ViewState(P_KEY)
                strTerms = ViewState(P_SESSION_TERMS)

                'CNSUPDP001_008
                ''画面連続使用の場合、キー項目を再設定する
                'If strBtnSts = 1 Or strBtnSts = 2 Then
                '    'データ区分
                '    strKey(1) = 0
                '    '案件進捗ステータス　'CNSUPDP001_006
                '    Select Case strKey(2)
                '        'Case "未処理"
                '        Case "01"
                '            'キャンセルの場合は処理完了
                '            'If strKey(3) <> "キャンセル" Then
                '            If strKey(3) <> "3" Then
                '                'FS稼動有無により設定ステータスを変更する
                '                If Me.txtSfOperation.ppText = 0 Then
                '                    '構成配信の工事依頼は現場作業待ちにする
                '                    If Me.txtHoleConDelively.ppText <> "2" Then
                '                        'strKey(2) = "作業依頼中"
                '                        strKey(2) = "03"
                '                    Else
                '                        'strKey(2) = "現場作業待ち"
                '                        strKey(2) = "06"
                '                    End If
                '                Else
                '                    'strKey(2) = "処理完了"
                '                    strKey(2) = "12"
                '                End If
                '            Else
                '                'strKey(2) = "処理完了"
                '                strKey(2) = "12"
                '            End If
                '            'Case "作業依頼中"
                '        Case "03"
                '            If Not dttTestDepartureDt.ppText = String.Empty Or _
                '                Not dttTemporaryInstallationDepartureDt.ppText = String.Empty Then
                '                'strKey(2) = "営業所受託"
                '                strKey(2) = "04"
                '            Else
                '                'strKey(2) = "作業依頼中"
                '                strKey(2) = "03"
                '            End If
                '            'Case "現場作業待ち"
                '        Case "06"
                '            If lblReqState.Text = "終了" Then
                '                'strKey(2) = "現場終了"
                '                strKey(2) = "07"
                '            End If
                '    End Select
                'End If
                ''CNSUPDP001_006　END
                If strBtnSts = 1 Or strBtnSts = 2 Then
                    'データ区分
                    strKey(1) = 0
                End If
                'CNSUPDP001_008　END

                '確認ポップアップの設定
                Me.btmAttachRequestNo.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "依頼番号と紐付")
                Select Case strTerms.ToString.Trim
                    Case ClsComVer.E_遷移条件.仮登録
                        strMsgNoS = "00008"
                        strMsg = "仮工事依頼書兼仕様書"
                    Case ClsComVer.E_遷移条件.登録
                        strMsgNoS = "00008"
                        strMsg = "工事設計依頼書"
                    Case ClsComVer.E_遷移条件.更新
                        strMsgNoS = "00004"
                        strMsg = "工事依頼書兼仕様書"
                End Select
                Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes(strMsgNoS, ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strMsg)

                'ボタン押下時の検証を無効
                Master.ppLeftButton1.CausesValidation = False  '物品転送依頼
                Master.ppLeftButton2.CausesValidation = False  'ＴＢＯＸ照会
                Master.ppLeftButton3.CausesValidation = False  'シリアル登録
                Master.ppLeftButton4.CausesValidation = False  '資料請求
                Master.ppLeftButton5.CausesValidation = False  '工事連絡票一覧
                Master.ppLeftButton6.CausesValidation = False  '完了報告書
                Master.ppRigthButton6.CausesValidation = False '工事進捗
                Master.ppRigthButton5.CausesValidation = False 'アップロード
                Master.ppRigthButton4.CausesValidation = False '印刷
                Master.ppRigthButton2.CausesValidation = False 'クリア
                Master.ppRigthButton1.CausesValidation = False '元に戻す

                'ボタン押下時の検証を有効
                Master.ppRigthButton3.CausesValidation = True '登録・更新

                If Not strKey Is Nothing Then
                    If strTerms.ToString.Trim = ClsComVer.E_遷移条件.登録 Or
                        strTerms.ToString.Trim = ClsComVer.E_遷移条件.参照 Then
                        'If strKey(2).ToString = "未処理" Then 'CNSUPDP001_006
                        If strKey(2).ToString = "01" Then 'CNSUPDP001_006
                            Me.ddlMTRStatus.Items.Insert(0, "01:未処理")
                            Me.ddlMTRStatus.SelectedIndex = 0
                        Else
                            msSetddlStatus(strKey(0))            '案件進捗状況ドロップダウン設定
                        End If
                    Else
                        msSetddlStatus(strKey(0))            '案件進捗状況ドロップダウン設定
                    End If
                ElseIf strTerms.ToString.Trim = ClsComVer.E_遷移条件.仮登録 Then
                    Me.ddlMTRStatus.Items.Insert(0, "00:仮登録")
                    Me.ddlMTRStatus.SelectedIndex = 0
                End If

                '検証グループ設定
                If Not strTerms.ToString.Trim = ClsComVer.E_遷移条件.参照 Then
                    Master.ppRigthButton4.ValidationGroup = "Artcltrns"
                End If

                '確定時のエラーチェック
                'If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                '    Master.ppRigthButton3.ValidationGroup = "2"
                '    vasSummary.ValidationGroup = "2"
                '    trfSfPersonInCharge.ppValidationGroup = "2"
                '    trfSfPersonInCharge.ppRequiredField = True
                '    txtOfficCD.ppValidationGroup = "2"
                '    txtOfficCD.ppRequiredField = True
                'End If

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = sCnsProgid
                Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                '仮本設置のビューステートを 0:なし で設定
                ViewState(M_STE_TMP_DT) = "0"

                '本設置、仮設置進捗ステータスのビューステートをNothingで設定
                ViewState(M_TMPSTTS_CD) = Nothing
                ViewState(M_STATUS_CD) = Nothing

                'グリッドの初期化
                Me.grvList.DataSource = New DataTable

                'データ取得処理
                If Not msGet_Data(dstOrders_1 _
                                  , dstOrders_2 _
                                  , dstOrders_3 _
                                  , dstOrders_4 _
                                  , dstOrders_5) Then
                    Throw New Exception("")
                End If
                grvList2 = Me.grvList

                If strTerms = ClsComVer.E_遷移条件.仮登録 Then
                    '必須項目の設定(True：必須/False：任意)
                    msEdit_RequiredField()
                    '仮依頼番号設定
                    Me.ProvisionalRequestNo.ppTextBoxOne.Text = sCnsRequestNo
                    'Me.ProvisionalRequestNo.ppTextBoxTwo.Text = ViewState("PRECNST_NO").PadLeft(8, "0"c) 'CNSUPDP001_007
                Else
                    '必須項目の設定(True：必須/False：任意)
                    msEdit_RequiredField()

                    Me.ProvisionalRequestNo.ppTextBoxOne.Text = sCnsRequestNo

                    '編集表示処理
                    If Not msGet_Edit(dstOrders_1 _
                                  , dstOrders_3 _
                                  , dstOrders_5) Then
                        Throw New Exception("")
                    End If
                    '一覧差分表示
                    If Not msGet_GRIDEdit(dstOrders_2 _
                                  , dstOrders_4) Then
                        Throw New Exception("")
                    End If
                End If

                '作業担当者ドロップダウン設定
                Dim officecd As String = ""
                If dstOrders_1.Tables.Count <> 0 Then
                    If (Not dstOrders_1 Is Nothing) And (dstOrders_1.Tables(0).Rows.Count <> 0) Then
                        officecd = dstOrders_1.Tables(0).Rows(0).Item("D39_BRANCH_CD").ToString
                        msSetddlEmployee(officecd)
                    End If
                End If

                '仮登録紐付けボタン設定
                If dstOrders_1.Tables.Count <> 0 Then
                    If dstOrders_1.Tables(0).Rows(0).Item("D39_PRECNST_NO").ToString <> String.Empty Then
                        btmAttachRequestNo.Text = "変更または解除"
                    Else
                        btmAttachRequestNo.Text = "依頼番号紐付け"
                    End If
                End If

                '活性化・非活性化処理
                msEnabled(strTerms.ToString.Trim, strKey)

                '画面連続使用の場合、キー項目を再設定する　'CNSUPDP001_008
                If strBtnSts = 1 Or strBtnSts = 2 Then
                    'データ区分
                    strKey(1) = 0

                    Using dst As DataSet = ViewState("D39_DATA_SV")
                        '案件ステータス取得
                        strKey(2) = dst.Tables(0).Rows(0)("D39_MTR_STATUS_CD").ToString
                    End Using

                    'ドロップダウン反映
                    Me.ddlMTRStatus.SelectedValue = strKey(2)
                End If

                '受付確定、登録、更新ボタンフラグ
                strBtnSts = 0
            End If
            'CNSUPDP001_008　END

            Me.txtOfficCD.ppTextBox.AutoPostBack = True

        Catch ex As Exception

            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#Region "ページ描画前"

    '---------------------------
    '2014/04/14 武 ここから
    '---------------------------
    ''' <summary>
    ''' ページ描画前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
                'Master.ppRigthButton6.Enabled = True

            Case "営業所"
                trfAgencyNm.ppEnabled = False
                trfAgencyShop.ppEnabled = False
                trfSendingStation.ppEnabled = False
                Master.ppLeftButton2.Enabled = False
                Master.ppLeftButton5.Enabled = False
                'Master.ppRigthButton3.Enabled = False
                'Master.ppRigthButton6.Enabled = True
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton1.Enabled = False
                txtSpecificationConnectingDivision.ppEnabled = False
                ttwNotificationNo.ppEnabled = False
                txtHoleNm.ppEnabled = False
                txtTboxLine.ppEnabled = False
                txtPersonInCharge_1.ppEnabled = False
                txtPersonInCharge_2.ppEnabled = False
                txtStkHoleIn.ppEnabled = False
                txtSfOperation.ppEnabled = False
                txtEMoneyIntroduction.ppEnabled = False
                txtStkHoleOut.ppEnabled = False
                txtTboxTakeawayDivision.ppEnabled = False
                txtEMoneyIntroductionCns.ppEnabled = False
                dttEMoneyTestDt.ppEnabled = False
                tmtEMoneyTestTm.ppEnabled = False
                txtOtherContents.ppEnabled = False
                txtLanNew.ppEnabled = False
                txtLanExpansion.ppEnabled = False
                txtLanSomeRemoval.ppEnabled = False
                txtLanShopRelocation.ppEnabled = False
                txtLanAllRemoval.ppEnabled = False
                txtLanOnceRemoval.ppEnabled = False
                txtLanReInstallation.ppEnabled = False
                txtLanConChange.ppEnabled = False
                txtLanConDelively.ppEnabled = False
                txtLanOther.ppEnabled = False
                txtTwinsShop.ppEnabled = False
                txtIndependentCns.ppEnabled = False
                txtSameTimeCnsNo.ppEnabled = False
                txtPAndCDivision.ppEnabled = False
                txtParentHoleCd.ppEnabled = False
                txtConstructionExistenceF1.ppEnabled = False
                txtConstructionExistenceF2.ppEnabled = False
                txtConstructionExistenceF3.ppEnabled = False
                txtConstructionExistenceF4.ppEnabled = False
                dttStartOfCon.ppEnabled = False
                tmtStartOfCon.ppEnabled = False
                dttLastOpenDt.ppEnabled = False
                dttLastOpenDtT500.ppEnabled = False
                dttTemporaryInstallationCnsDivision.ppEnabled = False
                dttTemporaryInstallationDtNotInputDivision0.ppEnabled = False
                dttPolice.ppEnabled = False
                tmtPolice.ppEnabled = False
                dttShiftCnsDivision.ppEnabled = False
                dttShiftCnsWorkDivision.ppEnabled = False
                dttOpen.ppEnabled = False
                tmtOpen.ppEnabled = False
                dttLanCns.ppEnabled = False
                tmtLanCns.ppEnabled = False
                dttVerup.ppEnabled = False
                tmtVerup.ppEnabled = False
                dttVerupDtDivision.ppEnabled = False
                txtVerupCnsType_1.ppEnabled = False
                txtVerupCnsType_2.ppEnabled = False
                txtAgencyCd.ppEnabled = False
                trfAgencyNm.ppEnabled = False
                txtAgencyAdd.ppEnabled = False
                txtAgencyResponsible.ppEnabled = False
                trfAgencyShop.ppEnabled = False
                txtAgencyShopAdd.ppEnabled = False
                txtAgencyShopResponsible.ppEnabled = False
                trfSendingStation.ppEnabled = False
                txtSendingStationAdd.ppEnabled = False
                txtAgencyCd.ppEnabled = False
                txtAgencyShop.ppEnabled = False
                txtSendingStation.ppEnabled = False
                txtRemarks.ppEnabled = False
                txtSpecificationsRemarks.ppEnabled = False
                txtImportantPoints1.ppEnabled = False
                txtSpecificationsRemarks.ppEnabled = False
                cbxDllSettingChange.Enabled = False
                trfSfPersonInCharge.ppEnabled = False
                RadioButtonList1.Enabled = False
                txtProcessingResultDetail1.ppEnabled = False
                txtProcessingResultDetail2.ppEnabled = False
                txtProcessingResultDetail3.ppEnabled = False
                txtControlInfoRemarks1.ppEnabled = False
                txtControlInfoRemarks2.ppEnabled = False
                txtControlInfoRemarks3.ppEnabled = False
                txtAgencyTel.ppEnabled = False
                txtAgencyPersonnel.ppEnabled = False
                txtAgencyShopTelNo.ppEnabled = False
                txtAgencyShopPersonnel.ppEnabled = False
                txtSendingStationResponsible.ppEnabled = False
                tdtDeliveryPreferredDt.ppEnabled = False
                ClsCMTimeBox1.ppEnabled = False
                txtSendingStationTelNo.ppEnabled = False
                btmAttachRequestNo.Enabled = False
                txtImportantPoints2.ppEnabled = False
                Master.ppLeftButton1.Enabled = False

            Case "NGC"
        End Select

        'If lblRequestNo_2.Text.IndexOf("N0090") >= 0 Or lblRequestNo_2.Text = String.Empty Then
        '    Me.cbxDllSettingChange.Checked = True
        '    Me.cbxDllSettingChange.Enabled = False
        'End If

        If Not Me.ddlMTRStatus.SelectedValue Is Nothing Then
            '入力項目を非活性
            If CInt(Me.ddlMTRStatus.SelectedValue.Substring(0, 2)) > 6 Then
                Me.cbxDllSettingChange.Enabled = False
            End If
        End If

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim objWKDS As New DataSet

        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '仕様連絡区分　１：新規登録　２：変更通知　３：キャンセル連絡
                If Me.txtSpecificationConnectingDivision.ppText Is DBNull.Value Then
                    Me.lblSpecificationConnectingDivision.Text = ""
                ElseIf Me.txtSpecificationConnectingDivision.ppText = "" Then
                    Me.lblSpecificationConnectingDivision.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0159"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtSpecificationConnectingDivision.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblSpecificationConnectingDivision.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblSpecificationConnectingDivision.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                'システム分類　１：磁気　３：ＩＣ　５：ＬＵＴＥＲＮＡ
                If Me.txtSysClassification.ppText Is DBNull.Value Then
                    Me.lblSysClassification.Text = ""
                ElseIf Me.txtSysClassification.ppText = "" Then
                    Me.lblSysClassification.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0006"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtSysClassification.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblSysClassification.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblSysClassification.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                'ホールストック内
                If Me.txtStkHoleIn.ppText Is DBNull.Value Then
                    Me.lblStkHoleIn.Text = ""
                ElseIf Me.txtStkHoleIn.ppText = "" Then
                    Me.lblStkHoleIn.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0160"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtStkHoleIn.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblStkHoleIn.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblStkHoleIn.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                'ホールストック外
                If Me.txtStkHoleOut.ppText Is DBNull.Value Then
                    Me.lblStkHoleOut.Text = ""
                ElseIf Me.txtStkHoleOut.ppText = "" Then
                    Me.lblStkHoleOut.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0160"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtStkHoleOut.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblStkHoleOut.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblStkHoleOut.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                'ＦＳ稼動有無　０：有り　１：無し
                If Me.txtSfOperation.ppText Is DBNull.Value Then
                    Me.lblSfOperation.Text = ""
                ElseIf Me.txtSfOperation.ppText = "" Then
                    Me.lblSfOperation.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0113"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtSfOperation.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblSfOperation.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblSfOperation.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                'ＴＢＯＸ持帰り　０：なし　１：あり
                If Me.txtTboxTakeawayDivision.ppText Is DBNull.Value Then
                    Me.lblTboxTakeawayDivision.Text = ""
                ElseIf Me.txtTboxTakeawayDivision.ppText = "" Then
                    Me.lblTboxTakeawayDivision.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0001"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtTboxTakeawayDivision.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblTboxTakeawayDivision.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblTboxTakeawayDivision.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                '双子店区分　１：双子店
                If Me.txtTwinsShop.ppText Is DBNull.Value Then
                    Me.lblTwinsShop.Text = ""
                ElseIf Me.txtTwinsShop.ppText = "" Then
                    Me.lblTwinsShop.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0161"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtTwinsShop.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblTwinsShop.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblTwinsShop.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                '単独工事区分　０：単独工事　１：同時工事
                If Me.txtIndependentCns.ppText Is DBNull.Value Then
                    Me.lblIndependentCns.Text = ""
                ElseIf Me.txtIndependentCns.ppText = "" Then
                    Me.lblIndependentCns.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0162"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtIndependentCns.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblIndependentCns.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblIndependentCns.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                '親子区分　１：親ホール　２：子ホール
                If Me.txtPAndCDivision.ppText Is DBNull.Value Then
                    Me.lblPAndCDivision.Text = ""
                ElseIf Me.txtPAndCDivision.ppText = "" Then
                    Me.lblPAndCDivision.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0007"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.txtPAndCDivision.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblPAndCDivision.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblPAndCDivision.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

                'ＶＥＲＵＰ日付区分　０：なし　１：同日　２：なし
                If Me.dttVerupDtDivision.ppText Is DBNull.Value Then
                    Me.lblVerupDtDivision.Text = ""
                ElseIf Me.dttVerupDtDivision.ppText = "" Then
                    Me.lblVerupDtDivision.Text = ""
                Else
                    'ストアド及びパラメータ設定
                    cmdDB = New SqlCommand("ZCMPSEL064", conDB)
                    cmdDB.Parameters.Add(pfSet_Param("@CLASSCD", SqlDbType.NVarChar, "0163"))
                    cmdDB.Parameters.Add(pfSet_Param("@CODE", SqlDbType.NVarChar, Me.dttVerupDtDivision.ppText))
                    'データ取得
                    objWKDS = clsDataConnect.pfGet_DataSet(cmdDB)

                    If objWKDS.Tables.Count > 0 Then
                        If objWKDS.Tables(0).Rows.Count > 0 Then
                            Me.lblVerupDtDivision.Text = objWKDS.Tables(0).Rows(0).Item("M29_NAME").ToString
                        Else
                            Me.lblVerupDtDivision.Text = "不明"
                        End If
                    End If
                End If
                'データセット破棄
                Call psDisposeDataSet(objWKDS)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "区分マスタ情報取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'データセット破棄
                Call psDisposeDataSet(objWKDS)
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If


        ''仕様連絡区分　１：新規登録　２：変更通知　３：キャンセル連絡
        'If Me.txtSpecificationConnectingDivision.ppText Is DBNull.Value Then
        '    Me.lblSpecificationConnectingDivision.Text = ""
        'ElseIf Me.txtSpecificationConnectingDivision.ppText = "" Then
        '    Me.lblSpecificationConnectingDivision.Text = ""
        'ElseIf Me.txtSpecificationConnectingDivision.ppText = "1" Then
        '    Me.lblSpecificationConnectingDivision.Text = "新規登録"
        'ElseIf Me.txtSpecificationConnectingDivision.ppText = "2" Then
        '    Me.lblSpecificationConnectingDivision.Text = "変更通知"
        'ElseIf Me.txtSpecificationConnectingDivision.ppText = "3" Then
        '    Me.lblSpecificationConnectingDivision.Text = "キャンセル連絡"
        'Else
        '    Me.lblSpecificationConnectingDivision.Text = "不明"
        'End If

        ''システム分類　１：磁気　３：ＩＣ　５：ＬＵＴＥＲＮＡ
        'If Me.txtSysClassification.ppText Is DBNull.Value Then
        '    Me.lblSysClassification.Text = ""
        'ElseIf Me.txtSysClassification.ppText = "" Then
        '    Me.lblSysClassification.Text = ""
        'ElseIf Me.txtSysClassification.ppText = "1" Then
        '    Me.lblSysClassification.Text = "磁気"
        'ElseIf Me.txtSysClassification.ppText = "3" Then
        '    Me.lblSysClassification.Text = "ＩＣ"
        'ElseIf Me.txtSysClassification.ppText = "5" Then
        '    Me.lblSysClassification.Text = "ＬＵＴＥＲＮＡ"
        'Else
        '    Me.lblSysClassification.Text = "不明"
        'End If

        ''ホールストック内
        'If Me.txtStkHoleIn.ppText Is DBNull.Value Then
        '    Me.lblStkHoleIn.Text = ""
        'ElseIf Me.txtStkHoleIn.ppText = "" Then
        '    Me.lblStkHoleIn.Text = ""
        'ElseIf Me.txtStkHoleIn.ppText = "0" Then
        '    Me.lblStkHoleIn.Text = ""
        'ElseIf Me.txtStkHoleIn.ppText = "1" Then
        '    Me.lblStkHoleIn.Text = "該当"
        'Else
        '    Me.lblStkHoleIn.Text = "不明"
        'End If

        ''ホールストック外
        'If Me.txtStkHoleOut.ppText Is DBNull.Value Then
        '    Me.lblStkHoleOut.Text = ""
        'ElseIf Me.txtStkHoleOut.ppText = "" Then
        '    Me.lblStkHoleOut.Text = ""
        'ElseIf Me.txtStkHoleOut.ppText = "0" Then
        '    Me.lblStkHoleOut.Text = ""
        'ElseIf Me.txtStkHoleOut.ppText = "1" Then
        '    Me.lblStkHoleOut.Text = "該当"
        'Else
        '    Me.lblStkHoleOut.Text = "不明"
        'End If

        ''ＦＳ稼動有無
        'If Me.txtSfOperation.ppText Is DBNull.Value Then
        '    Me.lblSfOperation.Text = ""
        'ElseIf Me.txtSfOperation.ppText = "" Then
        '    Me.lblSfOperation.Text = ""
        'ElseIf Me.txtSfOperation.ppText = "0" Then
        '    Me.lblSfOperation.Text = "有り"
        'ElseIf Me.txtSfOperation.ppText = "1" Then
        '    Me.lblSfOperation.Text = "無し"
        'Else
        '    Me.lblSfOperation.Text = "不明"
        'End If

        ''ＴＢＯＸ持帰り
        'If Me.txtTboxTakeawayDivision.ppText Is DBNull.Value Then
        '    Me.lblTboxTakeawayDivision.Text = ""
        'ElseIf Me.txtTboxTakeawayDivision.ppText = "" Then
        '    Me.lblTboxTakeawayDivision.Text = ""
        'ElseIf Me.txtTboxTakeawayDivision.ppText = "0" Then
        '    Me.lblTboxTakeawayDivision.Text = "無し"
        'ElseIf Me.txtTboxTakeawayDivision.ppText = "1" Then
        '    Me.lblTboxTakeawayDivision.Text = "有り"
        'Else
        '    Me.lblTboxTakeawayDivision.Text = "不明"
        'End If

        ''双子店区分　１：双子店
        'If Me.txtTwinsShop.ppText Is DBNull.Value Then
        '    Me.lblTwinsShop.Text = ""
        'ElseIf Me.txtTwinsShop.ppText = "" Then
        '    Me.lblTwinsShop.Text = ""
        'ElseIf Me.txtTwinsShop.ppText = "0" Then
        '    Me.lblTwinsShop.Text = "単独"
        'ElseIf Me.txtTwinsShop.ppText = "1" Then
        '    Me.lblTwinsShop.Text = "双子店"
        'Else
        '    Me.lblTwinsShop.Text = "不明"
        'End If

        ''単独工事区分　０：単独工事　１：同時工事
        'If Me.txtIndependentCns.ppText Is DBNull.Value Then
        '    Me.lblIndependentCns.Text = ""
        'ElseIf Me.txtIndependentCns.ppText = "" Then
        '    Me.lblIndependentCns.Text = ""
        'ElseIf Me.txtIndependentCns.ppText = "0" Then
        '    Me.lblIndependentCns.Text = "単独工事"
        'ElseIf Me.txtIndependentCns.ppText = "1" Then
        '    Me.lblIndependentCns.Text = "同時工事"
        'Else
        '    Me.lblIndependentCns.Text = "不明"
        'End If

        ''親子区分　１：親ホール　２：子ホール
        'If Me.txtPAndCDivision.ppText Is DBNull.Value Then
        '    Me.lblPAndCDivision.Text = ""
        'ElseIf Me.txtPAndCDivision.ppText = "" Then
        '    Me.lblPAndCDivision.Text = ""
        'ElseIf Me.txtPAndCDivision.ppText = "1" Then
        '    Me.lblPAndCDivision.Text = "親ホール"
        'ElseIf Me.txtPAndCDivision.ppText = "2" Then
        '    Me.lblPAndCDivision.Text = "子ホール"
        'Else
        '    Me.lblPAndCDivision.Text = "不明"
        'End If

        ''ＶＥＲＵＰ日付区分　０：なし　１：同日　２：なし
        'If Me.dttVerupDtDivision.ppText Is DBNull.Value Then
        '    Me.lblVerupDtDivision.Text = ""
        'ElseIf Me.dttVerupDtDivision.ppText = "" Then
        '    Me.lblVerupDtDivision.Text = ""
        'ElseIf Me.dttVerupDtDivision.ppText = "0" Then
        '    Me.lblVerupDtDivision.Text = "なし"
        'ElseIf Me.dttVerupDtDivision.ppText = "1" Then
        '    Me.lblVerupDtDivision.Text = "同日"
        'ElseIf Me.dttVerupDtDivision.ppText = "2" Then
        '    Me.lblVerupDtDivision.Text = "単独"
        'Else
        '    Me.lblVerupDtDivision.Text = "不明"
        'End If

    End Sub

    '---------------------------
    '2014/04/14 武 ここから
    '---------------------------
#End Region

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)
        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim dstOrders As DataSet = Nothing          'データセット
        Dim err_num As String = String.Empty
        Dim strMes As String = String.Empty
        objStack = New StackFrame

        Try
            Dim strTerms As String = String.Empty
            Dim dstOrders_1 As New DataSet
            Dim dstOrders_2 As New DataSet
            Dim dstOrders_3 As New DataSet
            Dim dstOrders_4 As New DataSet
            Dim dstOrders_5 As New DataSet

            'ビューステート項目取得
            strTerms = ViewState(P_SESSION_TERMS)

            Select Case sender.ID
                Case "btmAttachRequestNo"   '依頼番号紐付け"
                    strBtnSts = 2
                    If btmAttachRequestNo.Text <> "変更または解除" Then
                        Me.msGamen_Check()
                    End If
                    If Page.IsValid Then
                        err_num = "30001"

                        '登録・更新処理
                        If mfSet_Data(0) = True Then
                            '完了メッセージ
                            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "依頼番号と紐付")
                        End If
                    End If
                    Me.grvList = grvList2
                    Me.grvList.DataBind()
                    Page_Load(sender, e)
                Case "btnLeft1"   '物品転送依頼"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = {lblRequestNo_2.Text}   '依頼番号
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                    Session(P_SESSION_OLDDISP) = sCnsProgid
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                    If strTerms = ClsComVer.E_遷移条件.更新 Then

                        '★排他制御処理を実行
                        Dim strExclusiveDate As String = Nothing
                        Dim arTable_Name As New ArrayList
                        Dim arKey As New ArrayList

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D19_ARTCLTRNS")
                        'arTable_Name.Insert(1, "D20_ARTCLTRNS_DTL") ' 2014/05/26 間瀬 親（D19_ARTCLTRNS）がロックされていれば操作できないので不要

                        '★ロックテーブルキー項目の登録(D19_ARTCLTRNS,D20_ARTCLTRNS_DTL)
                        arKey.Insert(0, lblRequestNo_2.Text)
                        arKey.Insert(1, lblRequestNo_2.Text)
                        '★排他情報確認処理(更新画面へ遷移)
                        If clsExc.pfSel_Exclusive(strExclusiveDate _
                                         , Me _
                                         , Session(P_SESSION_IP) _
                                         , Session(P_SESSION_PLACE) _
                                         , Session(P_SESSION_USERID) _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , ViewState(P_SESSION_GROUP_NUM) _
                                         , "CNSUPDP002" _
                                         , arTable_Name _
                                         , arKey) = 0 Then

                            '物品転送依頼存在確認
                            conDB = Nothing
                            If clsDataConnect.pfOpen_Database(conDB) Then
                                Try
                                    cmdDB = New SqlCommand("CNSUPDP001_S19", conDB)
                                    cmdDB.Parameters.Add(pfSet_Param("@prmD39_CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB) 'データ取得
                                Catch ex As Exception
                                    psMesBox(Me, "00003", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "工事依頼書兼仕様書")
                                Finally
                                    'DB切断
                                    clsDataConnect.pfClose_Database(conDB)
                                End Try
                            End If
                            '物品転送依頼データが存在した場合、それを表示
                            If Not dstOrders.Tables(0).Rows.Count = Nothing Then
                                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                                Session(P_KEY) = {dstOrders.Tables(0).Rows(0).Item("物品転送依頼番号").ToString}
                            End If
                            '★登録年月日時刻
                            Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                        Else
                            '排他ロック中
                            Exit Sub
                        End If
                    End If

                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsCNSUPDP002, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsCNSUPDP002)

                Case "btnLeft2"   'ＴＢＯＸ照会"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = {txtTboxId.ppText.ToString.Trim}   'ＴＢＯＸＩＤ
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    Session(P_SESSION_OLDDISP) = sCnsProgid
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsMSTLSTP001, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsWATINQP001)

                Case "btnLeft3"   'シリアル登録"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsSERUPDP001, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsSERUPDP001)

                Case "btnLeft4"   '資料請求"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    '--------------------------------
                    '2015/04/18 加賀　ここから
                    '--------------------------------
                    strTerms = Session(P_SESSION_TERMS)
                    strTerms = ViewState(P_SESSION_TERMS)
                    '--------------------------------
                    '2015/04/18 加賀　ここまで
                    '--------------------------------
                    Select Case strTerms.ToString.Trim
                        Case ClsComVer.E_遷移条件.参照
                            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                        Case Else
                            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                            Using dst As DataSet = ViewState("D39_DATA_SV")
                                Session(P_KEY) = {dst.Tables(0).Rows(0)("D39_CNST_NO").ToString, _
                                                  dst.Tables(0).Rows(0)("D39_TBOXID").ToString, _
                                                  dst.Tables(0).Rows(0)("D39_HALL_CD").ToString}   '依頼番号/TBOXID/ホールコード

                            End Using
                            '★排他制御処理を実行
                            Dim strExclusiveDate As String = Nothing
                            Dim arTable_Name As New ArrayList
                            Dim arKey As New ArrayList

                            '★ロック対象テーブル名の登録
                            arTable_Name.Insert(0, "D57_CNSTREQSPEC_DTL3")
                            arTable_Name.Insert(1, "D39_CNSTREQSPEC")

                            '★ロックテーブルキー項目の登録(D57_CNSTREQSPEC_DTL3,D39_CNSTREQSPEC)
                            arKey.Insert(0, lblRequestNo_2.Text)
                            'arKey.Insert(1, lblRequestNo_2.Text)
                            '★排他情報確認処理(更新画面へ遷移)
                            If clsExc.pfSel_Exclusive(strExclusiveDate _
                                             , Me _
                                             , Session(P_SESSION_IP) _
                                             , Session(P_SESSION_PLACE) _
                                             , Session(P_SESSION_USERID) _
                                             , Session(P_SESSION_SESSTION_ID) _
                                             , ViewState(P_SESSION_GROUP_NUM) _
                                             , "CNSUPDP005" _
                                             , arTable_Name _
                                             , arKey) = 0 Then

                                '★登録年月日時刻
                                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                            Else
                                '排他ロック中
                                Exit Sub
                            End If
                    End Select

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
                                    objStack.GetMethod.Name, sCnsCNSUPDP005, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsCNSUPDP005)

                Case "btnLeft5"   '工事連絡票一覧"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsCNSLSTP005, strPrm, "TRANS")
                    Session(P_KEY) = {lblRequestNo_2.Text, txtTboxId.ppText, txtHoleNm.ppText}
                    psOpen_Window(Me, sCnsCNSLSTP005)

                Case "btnLeft6"   '完了報告書"

                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    Session(P_KEY) = ViewState(P_KEY)
                    Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

                    conDB = Nothing
                    '接続
                    If clsDataConnect.pfOpen_Database(conDB) Then
                        Try
                            cmdDB = New SqlCommand("CNSINQP001_S3", conDB)
                            'パラメータ設定
                            cmdDB.Parameters.Add(pfSet_Param("cntl_no", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                            'データ取得
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                        Catch ex As Exception
                            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
                        Finally
                            'DB切断
                            clsDataConnect.pfClose_Database(conDB)
                        End Try
                    End If

                    If dstOrders.Tables(0).Rows(0).Item("確認者名").ToString <> "" Then
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照              '遷移条件
                    Else
                        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                            'ViewState(P_KEY) = Session(P_KEY)(0)
                            '★ロック対象テーブル名の登録
                            arTable_Name.Insert(0, "D87_CNSTBREAK")

                            '★ロックテーブルキー項目の登録(D87_CNSTBREAK)
                            arKey.Insert(0, Session(P_KEY)(0))       'D87_CONST_NO

                            '★排他情報確認処理(更新画面へ遷移)
                            If clsExc.pfSel_Exclusive(strExclusiveDate _
                                             , Me _
                                             , Session(P_SESSION_IP) _
                                             , Session(P_SESSION_PLACE) _
                                             , Session(P_SESSION_USERID) _
                                             , Session(P_SESSION_SESSTION_ID) _
                                             , ViewState(P_SESSION_GROUP_NUM) _
                                             , P_FUN_CNS & P_SCR_UPD & P_PAGE & "006" _
                                             , arTable_Name _
                                             , arKey) = 0 Then

                                '★登録年月日時刻
                                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                            Else
                                '排他ロック中
                                Exit Sub
                            End If
                        End If
                    End If

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsCNSUPDP006, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsCNSUPDP006)

                Case "btnRigth6"  '工事進捗"
                    Dim strKey() As String = Nothing
                    Dim in_sesstion As String = ViewState(M_STE_TMP_DT)
                    Dim settmp1 As String = String.Empty

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    If Not ViewState(M_TMPSTTS_CD).ToString = Nothing _
                        OrElse Not Not ViewState(M_STATUS_CD).ToString = Nothing Then   '進捗有
                        If Not ViewState(M_TMPSTTS_CD).ToString = Nothing _
                            AndAlso Not ViewState(M_TMPSTTS_CD).ToString = "08" Then    '仮設置有終了前
                            settmp1 = "1"
                        Else                                                            '本設置
                            settmp1 = "2"
                        End If
                    Else                                                                '進捗無
                        Select Case in_sesstion
                            Case "0"      'なし
                            Case "1"      '仮設置
                                settmp1 = "1"
                            Case "2", "3" '本設置,仮本設置一致
                                settmp1 = "2"
                            Case "8"      '仮設置から本設置へ変更
                                settmp1 = "2"
                            Case "9"      '両方
                                settmp1 = "1"
                        End Select
                    End If

                    Dim strDateTime As StringBuilder = New StringBuilder
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttTest.ppText)
                    strKey = {Me.lblRequestNo_2.Text, Me.txtTboxId.ppText, strDateTime.ToString}  '依頼番号,設置区分
                    Session(P_KEY) = {Me.lblRequestNo_2.Text, settmp1}  '依頼番号,設置区分

                    '★排他制御処理を実行
                    Dim strExclusiveDate As String = Nothing
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D24_CNST_SITU_DTL")
                    arTable_Name.Insert(1, "D39_CNSTREQSPEC")
                    arTable_Name.Insert(2, "D84_ANYTIME_LIST")
                    '★ロックテーブルキー項目の登録(D39_CNSTREQSPEC)
                    arKey.Insert(0, strKey(0))
                    arKey.Insert(1, strKey(1))
                    arKey.Insert(2, strKey(2))

                    '★排他情報確認処理(更新画面へ遷移)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_DISPUPD003_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        '★登録年月日時刻
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                    Else
                        '排他ロック中
                        Exit Sub
                    End If

                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsCNSUPDP001, strPrm, "TRANS")

                    psOpen_Window(Me, sCnsCNSUPDP003)

                Case "btnRigth5"  'アップロード"

                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                    Session(P_KEY) = {Me.lblRequestNo_2.Text}  '依頼番号

                    Dim strPrm As String = ""
                    Dim tmp As Object() = Session(P_KEY)
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
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
                                    objStack.GetMethod.Name, sCnsDLCINPP001, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsDLCINPP001)

                Case "btnRigth4"  '印刷"

                    '工事依頼書兼仕様書印刷処理
                    msSet_PrinT()

                Case "btnRigth3"  '受付確定／登録／更新"

                    '入力項目チェック処理
                    Me.msGamen_Check()
                    If Page.IsValid Then
                        Select Case strTerms.ToString.Trim
                            Case ClsComVer.E_遷移条件.仮登録
                                err_num = "00001"
                            Case ClsComVer.E_遷移条件.登録
                                err_num = "00003"
                            Case ClsComVer.E_遷移条件.更新
                                err_num = "00001"
                        End Select
                        If Master.ppRigthButton3.Text = "受付確定" Then
                            '緊急依頼判断処理
                            mfJg_Emergency(Me.lblRequestNo_2.Text)
                        End If

                        'CNSUPDP001_021
                        ''CNSUPDP001_020  
                        ''TBOX、ホール情報の「双子店区分」「MDN機器」を更新
                        'If Master.ppRigthButton3.Text = "更新" Then
                        '    Dim drCnstInfo As DataRow = Nothing
                        '    Dim strPrcCd As String = mfCheckUpdTbox(drCnstInfo)
                        '    Select Case strPrcCd
                        '        Case "0"
                        '            '処理無し

                        '        Case "1" '双子、MDN更新
                        '            '双子店更新
                        '            If mfUpdTwin(drCnstInfo.Item("D39_HALL_CD").ToString.Trim) = False Then
                        '                Exit Try
                        '            End If
                        '            'MDN機器更新
                        '            If mfUpdMDN(drCnstInfo) = False Then
                        '                Exit Try
                        '            End If

                        '        Case "2" 'MDN機器更新
                        '            If mfUpdMDN(drCnstInfo) = False Then
                        '                Exit Try
                        '            End If

                        '        Case "3" 'MDN機器戻し
                        '            If mfUpdMDNRev(drCnstInfo) = False Then
                        '                Exit Try
                        '            End If

                        '        Case "4" '双子のみ更新
                        '            If mfUpdTwin(drCnstInfo.Item("D39_HALL_CD").ToString.Trim) = False Then
                        '                Exit Try
                        '            End If

                        '        Case Else
                        '            'エラー
                        '            psMesBox(Me, "30029", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書データを取得できませんでした。", "データが正しいか")
                        '            Exit Try
                        '    End Select
                        'End If
                        ''CNSUPDP001_020 END
                        'CNSUPDP001_021 END

                        '登録・更新処理
                        If mfSet_Data(1) Then
                            If Master.ppRigthButton3.Text = "受付確定" _
                                And (Me.txtHoleNew.ppText = "1" OrElse Me.txtHoleReInstallation.ppText = "1" OrElse Me.txtHoleOnceRemoval.ppText = "1" OrElse Me.txtHoleAllRemoval.ppText = "1") _
                                And ((Me.txtSfOperation.ppText = "1") _
                                  Or (Me.txtSfOperation.ppText = "0" And Me.txtSpecificationConnectingDivision.ppText = "3") _
                                  Or (Me.txtSfOperation.ppText = "0" And Me.txtSfOperation.ppTextBox.ForeColor = Drawing.Color.Red)) Then
                                'T03_TBOX更新処理
                                If mfUpdate_TBOX(Me.lblRequestNo_2.Text) = False Then
                                    Exit Try
                                End If
                            End If
                        Else
                            psMesBox(Me, err_num, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
                            Exit Try
                        End If

                        If strTerms = ClsComVer.E_遷移条件.仮登録 Then
                            Me.ProvisionalRequestNo.ppTextBoxOne.Text = sCnsRequestNo
                            Me.ProvisionalRequestNo.ppTextBoxTwo.Text = ViewState("PRECNST_NO").PadLeft(8, "0"c)
                            Dim strKey(3) As String

                            strKey(0) = Me.ProvisionalRequestNo.ppTextOne & "-" & Me.ProvisionalRequestNo.ppTextTwo
                            strKey(1) = "0"
                            strKey(2) = "01" 'CNSUPDP001_006
                            strKey(3) = ""
                            ViewState(P_KEY) = strKey
                            ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                            strTerms = ClsComVer.E_遷移条件.仮登録

                            '活性化・非活性化処理
                            msEnabled(strTerms.ToString.Trim, strKey)

                            '確定時のエラーチェック
                            If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                                Master.ppRigthButton3.ValidationGroup = "1"
                                vasSummary.ValidationGroup = "1"
                            End If
                            Master.ppRigthButton1.Enabled = True    '元に戻す
                            Master.ppRigthButton2.Enabled = False   'クリア
                        End If

                        'データ取得
                        If Not msGet_Data(dstOrders_1 _
                                      , dstOrders_2 _
                                      , dstOrders_3 _
                                      , dstOrders_4 _
                                      , dstOrders_5) Then
                            strMes = "データ取得"
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            Exit Try
                        End If
                        '編集表示処理
                        dstOrders_5.Clear()
                        If Not msGet_Edit(dstOrders_1 _
                              , dstOrders_3 _
                              , dstOrders_5) Then
                            strMes = "編集表示処理"
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            Exit Try
                        End If
                        '一覧差分表示
                        If Not msGet_GRIDEdit(dstOrders_2 _
                                      , dstOrders_4) Then
                            strMes = "編集表示処理"
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            Exit Try
                        End If

                        Select Case strTerms.ToString.Trim
                            Case ClsComVer.E_遷移条件.仮登録
                                psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "仮工事依頼書兼仕様書")
                            Case ClsComVer.E_遷移条件.登録
                                Master.ppRigthButton3.Enabled = False                '受付確定
                                Master.ppRigthButton2.Enabled = False                'クリア
                                Me.txtOfficCD.ppEnabled = False                      '担当営業所コード
                                'CNSUPDP001_003
                                'Me.Panel9.Enabled = False                            '処理結果等
                                Me.cbxDllSettingChange.Enabled = True
                                Me.trfSfPersonInCharge.ppEnabled = False
                                Me.RadioButtonList1.Enabled = False
                                Me.txtProcessingResultDetail1.ppEnabled = False
                                Me.txtProcessingResultDetail2.ppEnabled = False
                                Me.txtProcessingResultDetail3.ppEnabled = False
                                Me.txtControlInfoRemarks1.ppEnabled = False
                                Me.txtControlInfoRemarks2.ppEnabled = False
                                Me.txtControlInfoRemarks3.ppEnabled = False
                                'CNSUPDP001_003 END
                                Me.btnTrader.Enabled = False
                                psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事設計依頼書")
                            Case ClsComVer.E_遷移条件.更新
                                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
                        End Select

                        If sender.text = "受付確定" Then
                            ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                        End If
                        '受付確定、登録、更新ボタン押下フラグ
                        strBtnSts = 1
                        Page_Load(sender, e)
                    End If
                    Me.grvList = grvList2
                    Me.grvList.DataBind()
                Case "btnRigth2"  'クリア"

                    err_num = "30008"

                    msClearScreen(ViewState(P_SESSION_TERMS))

                Case "btnRigth1"  '元に戻す"

                    err_num = "30008"

                    '情報取得
                    If Not msGet_Data(dstOrders_1 _
                                  , dstOrders_2 _
                                  , dstOrders_3 _
                                  , dstOrders_4 _
                                  , dstOrders_5) Then
                        Throw New Exception("")
                    End If
                    '編集表示処理
                    If Not msGet_Edit(dstOrders_1 _
                          , dstOrders_3 _
                          , dstOrders_5) Then
                        Throw New Exception("")
                    End If
                    '一覧差分表示
                    If Not msGet_GRIDEdit(dstOrders_2 _
                                  , dstOrders_4) Then
                        Throw New Exception("")
                    End If

                    If strTerms = ClsComVer.E_遷移条件.仮登録 Then
                        Me.ProvisionalRequestNo.ppTextBoxOne.Text = sCnsRequestNo
                        Me.ProvisionalRequestNo.ppTextBoxTwo.Text = ViewState("PRECNST_NO").PadLeft(8, "0"c)
                    Else

                        '情報取得
                        If Not msGet_Data(dstOrders_1 _
                                      , dstOrders_2 _
                                      , dstOrders_3 _
                                      , dstOrders_4 _
                                      , dstOrders_5) Then
                            Throw New Exception("")
                        End If
                        '編集表示処理
                        If Not msGet_Edit(dstOrders_1 _
                              , dstOrders_3 _
                              , dstOrders_5) Then
                            Throw New Exception("")
                        End If
                        '一覧差分表示
                        If Not msGet_GRIDEdit(dstOrders_2 _
                                      , dstOrders_4) Then
                            Throw New Exception("")
                        End If

                    End If

                    '営業所コードの活性制御
                    FS_WrkCls_TextChanged(sender, e)

                    '作業担当者設定 'CNSUPDP001_018
                    Dim strChargeCD() As String = {TryCast(ViewState("D39_STEST_CHARGE_CD"), String), _
                                                   TryCast(ViewState("D39_TMPSET_CHARGE_CD"), String)}
                    '総合テスト 作業担当者
                    If strChargeCD(0) IsNot Nothing Then
                        If ddlPersonal1.Items.FindByValue(strChargeCD(0)) IsNot Nothing Then
                            ddlPersonal1.SelectedValue = strChargeCD(0)
                        End If
                    End If
                    '仮設置 作業担当者
                    If strChargeCD(1) IsNot Nothing Then
                        If ddlPersonal2.Items.FindByValue(strChargeCD(1)) IsNot Nothing Then
                            ddlPersonal2.SelectedValue = strChargeCD(1)
                        End If
                    End If
                    'CNSUPDP001_018 END
            End Select

        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            If strMes <> String.Empty Then
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", strMes)
            End If
        End Try

    End Sub

#End Region

#Region "テキスト変更時"
    ''' <summary>
    ''' ホール情報取得処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub TextBox_TextChanged(ByVal sender As Object _
                                       , ByVal e As System.EventArgs) 'Handles txtTboxId_TextChanged.TextChanged
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        objStack = New StackFrame

        Try
            If Me.txtTboxId.ppText = String.Empty Then
                'ホール情報クリア
                Me.txtCurrentSys.ppText = String.Empty          '現行システム
                Me.txtVer.ppText = String.Empty                 'Ver
                Me.txtSysClassification.ppText = String.Empty   'システム分類
                Me.txtHoleCd.ppText = String.Empty              'ホールコード
                Me.txtHoleNm.ppText = String.Empty              'ホール名
                Me.txtAddress.ppText = String.Empty             '住所
                Me.txtHoleTelNo.ppText = String.Empty           '電話番号
                Me.txtNLDivision.ppText = String.Empty          'NL
                Me.txtPersonInCharge_2.ppText = String.Empty    '担当者氏名
                Me.txtTboxLine.ppText = String.Empty            'ＤＤＸＰ回線番号
                Me.txtOfficCD.ppText = String.Empty             '営業所
                Me.lblSendNmV.Text = String.Empty               '営業所名
                ViewState("TBOXCLC") = String.Empty             'TBOX機種コード
                Exit Try
            End If

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Exit Sub
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(sCnsSqlid_S6, conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, txtTboxId.ppText))               'ＴＢＯＸＩＤ
            End With

            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count = 0 Then
                '--------------------------------
                '2015/04/21 加賀　ここから
                '--------------------------------
                'psMesBox(Me, "30011", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸＩＤ")
                psMesBox(Me, "10004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力されたＴＢＯＸＩＤ")

                'ホール情報クリア
                Me.txtCurrentSys.ppText = String.Empty          '現行システム
                Me.txtVer.ppText = String.Empty                 'Ver
                Me.txtSysClassification.ppText = String.Empty   'システム分類
                Me.txtHoleCd.ppText = String.Empty              'ホールコード
                Me.txtHoleNm.ppText = String.Empty              'ホール名
                Me.txtAddress.ppText = String.Empty             '住所
                Me.txtHoleTelNo.ppText = String.Empty           '電話番号
                Me.txtNLDivision.ppText = String.Empty          'NL
                Me.txtTboxLine.ppText = String.Empty            'ＤＤＸＰ回線番号
                Me.txtPersonInCharge_2.ppText = String.Empty    '担当者氏名
                Me.txtOfficCD.ppText = String.Empty             '営業所
                Me.lblSendNmV.Text = String.Empty               '営業所名
                ViewState("TBOXCLC") = String.Empty             'TBOX機種コード
                '--------------------------------
                '2015/04/21 加賀　ここまで
                '--------------------------------
                Exit Sub
            End If

            Me.txtHoleCd.ppText = dstOrders.Tables(0).Rows(0)("T01_HALL_CD").ToString.Trim           'ホールコード
            Me.txtHoleNm.ppText = dstOrders.Tables(0).Rows(0)("T01_HALL_NAME").ToString.Trim         'ホール名
            Me.txtAddress.ppText = dstOrders.Tables(0).Rows(0)("T01_ADDR").ToString.Trim             '住所
            Me.txtHoleTelNo.ppText = dstOrders.Tables(0).Rows(0)("T01_TELNO").ToString.Trim          '電話番号
            '--------------------------------
            '2015/04/20 加賀　ここから
            '--------------------------------
            Me.txtCurrentSys.ppText = dstOrders.Tables(0).Rows(0)("T03_TBOXCLASS_CD").ToString.Trim '現行システム
            Me.txtVer.ppText = dstOrders.Tables(0).Rows(0)("T03_VERSION").ToString.Trim             'Ver
            Me.txtSysClassification.ppText = dstOrders.Tables(0).Rows(0)("T03_SYSTEM_CLS")          'システム分類
            Me.txtNLDivision.ppText = dstOrders.Tables(0).Rows(0)("T01_NL_CLS").ToString.Trim       'NL
            Me.txtPersonInCharge_2.ppText = dstOrders.Tables(0).Rows(0)("T01_CHARGE").ToString.Trim '担当者氏名
            Me.txtOfficCD.ppText = dstOrders.Tables(0).Rows(0)("M44_OFFICE_CD").ToString.Trim       '営業所
            Me.lblSendNmV.Text = dstOrders.Tables(0).Rows(0)("M44_OFFICE_NM").ToString.Trim         '営業所名
            ViewState("TBOXCLC") = dstOrders.Tables(0).Rows(0)("T03_SYSTEM_CD").ToString.Trim       'TBOX機種コード
            '--------------------------------
            '2015/04/20 加賀　ここまで
            '--------------------------------
            Me.txtTboxLine.ppText = dstOrders.Tables(0).Rows(0)("T03_DDXP_TELNO").ToString.Trim      'ＤＤＸＰ回線番号

        Catch ex As DBConcurrencyException
            psMesBox(Me, "0005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' FS稼働変更時の営業所コード活性制御
    ''' </summary>
    Protected Sub FS_WrkCls_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Try
            Select Case txtSfOperation.ppText
                Case "0"
                    txtOfficCD.ppEnabled = True
                    msSetddlEmployee(txtOfficCD.ppText)
                    lblSfOperation.Text = "有り"
                Case "1"
                    txtOfficCD.ppText = String.Empty
                    txtOfficCD.ppEnabled = False
                    msSetddlEmployee(String.Empty)
                    lblSfOperation.Text = "無し"
                Case Else
            End Select
            Me.txtStkHoleOut.ppTextBox.Focus()
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.txtSfOperation.ppTextBox.Focus()
        End Try
    End Sub
#End Region

#Region "システム分類取得処理"
    ''' <summary>
    ''' システム分類取得処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub getSystemGrp(ByVal sender As Object, ByVal e As System.EventArgs)
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        objStack = New StackFrame

        Try
            '現行システム
            If Me.txtCurrentSys.ppText = String.Empty Then
                '未入力
                Me.txtSysClassification.ppText = String.Empty
            Else
                '現行システム 整合性チェック
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Using conTrn = conDB.BeginTransaction
                        cmdDB = New SqlCommand("CNSUPDP001_S28", conDB)
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("prmTBOXCLASS", SqlDbType.NVarChar, txtCurrentSys.ppText))             '現行システム
                            .Add(pfSet_Param("prmVERSION", SqlDbType.NVarChar, txtVer.ppText))                      'Ver
                            .Add(pfSet_Param("prmFLG", SqlDbType.SmallInt, Integer.Parse(ViewState(P_SESSION_TERMS))))    '判定フラグ
                            .Add(pfSet_Param("outTBOXCLASS", SqlDbType.SmallInt, 10, ParameterDirection.Output))    '戻り値 現行システム件数
                            .Add(pfSet_Param("outVERSION", SqlDbType.SmallInt, 10, ParameterDirection.Output))      '戻り値 Ver件数
                            .Add(pfSet_Param("outSYS_GRP", SqlDbType.NVarChar, 10, ParameterDirection.Output))      '戻り値 システム分類
                        End With

                        'コマンドタイプ設定(ストアド)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        If Integer.Parse(cmdDB.Parameters("outTBOXCLASS").Value.ToString) = 0 Then
                            '現行システム不在
                            'txtCurrentSys.psSet_ErrorNo("2002", txtCurrentSys.ppName & " : " & txtCurrentSys.ppText)
                            'psMesBox(Me, "10004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力されたシステム")

                            'システム分類
                            Me.txtSysClassification.ppText = String.Empty
                        Else
                            'システム分類
                            Me.txtSysClassification.ppText = cmdDB.Parameters("outSYS_GRP").Value.ToString
                        End If

                        'コミット
                        conTrn.Commit()
                    End Using
                End If
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "0005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
            Me.txtVer.ppTextBox.Focus()
        End Try



    End Sub
#End Region

    Private Sub txtSpecificationConnectingDivision_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtNLDivision.ppTextBox.Focus()

    End Sub

    Private Sub txtSysClassification_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtTboxId.ppTextBox.Focus()

    End Sub

    Private Sub txtStkHoleIn_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtSfOperation.ppTextBox.Focus()

    End Sub

    Private Sub txtSfOperation_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtStkHoleOut.ppTextBox.Focus()

    End Sub

    Private Sub txtStkHoleOut_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtTboxTakeawayDivision.ppTextBox.Focus()

    End Sub

    Private Sub txtTboxTakeawayDivision_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtOtherContents.ppTextBox.Focus()

    End Sub

    Private Sub txtTwinsShop_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtIndependentCns.ppTextBox.Focus()

    End Sub

    Private Sub txtIndependentCns_Lostfocus(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.txtSameTimeCnsNo.ppTextBox.Focus()

    End Sub

    Private Sub txtPAndCDivision_Lostfocus()

        Me.txtParentHoleCd.ppTextBox.Focus()

    End Sub

    Private Sub dttVerupDtDivision_Lostfocus()

        Me.txtVerupCnsType_1.ppTextBox.Focus()

    End Sub

#End Region

#Region "そのほかのプロシージャ"

#Region "活性化・非活性化処理"

    ''' <summary>
    ''' 活性化・非活性化処理
    ''' </summary>
    ''' <param name="strTerms">遷移条件</param>
    ''' <param name="strKey">キー情報（参照時のみ設定）</param>
    ''' <remarks></remarks>
    Private Sub msEnabled(ByVal strTerms As String,
                          Optional ByVal strKey() As String = Nothing)

        'ボタン表示
        Master.ppLeftButton1.Visible = True                 '物品転送依頼
        Master.ppLeftButton2.Visible = True                 'ＴＢＯＸ照会
        Master.ppLeftButton3.Visible = True                 'シリアル登録
        Master.ppLeftButton4.Visible = True                 '資料請求
        Master.ppLeftButton5.Visible = True                 '工事連絡票一覧
        Master.ppLeftButton6.Visible = True                 '完了報告書
        '--------------------------------
        '2015/04/03 加賀　ここから
        '--------------------------------
        'Master.ppRigthButton6.Visible = True                '工事進捗
        '--------------------------------
        '2015/04/03 加賀　ここまで
        '--------------------------------
        Master.ppRigthButton5.Visible = True                'アップロード
        Master.ppRigthButton4.Visible = True                '印刷
        Master.ppRigthButton3.Visible = True                '受付確定
        Master.ppRigthButton2.Visible = True                'クリア
        Master.ppRigthButton1.Visible = True                '元に戻す

        'ボタン名称設定
        Master.ppLeftButton1.Text = sCnsBupinButon         '物品転送依頼
        Master.ppLeftButton2.Text = sCnsTboxButon          'ＴＢＯＸ照会
        Master.ppLeftButton3.Text = sCnsShiriaruButon      'シリアル登録
        Master.ppLeftButton4.Text = sCnsshiryouButon       '資料請求
        Master.ppLeftButton5.Text = sCnsKoujirenButon      '工事連絡票一覧
        Master.ppLeftButton6.Text = sCnsKanryouButon       '完了報告書
        Master.ppRigthButton6.Text = sCnsShinchokuButon    '工事進捗
        Master.ppRigthButton5.Text = sCnsUploodButon       'アップロード
        Master.ppRigthButton4.Text = sCnsPrintButon        '印刷

        Select Case strTerms.ToString.Trim
            Case ClsComVer.E_遷移条件.仮登録
                Master.ppRigthButton3.Text = sCnsAddButon       '登録
            Case ClsComVer.E_遷移条件.登録
                Master.ppRigthButton3.Text = sCnsKakuteiButon   '受付確定
            Case ClsComVer.E_遷移条件.更新
                Master.ppRigthButton3.Text = sCnsUpButon        '更新
            Case Else                                           '参照
                If strKey(1).ToString = "0" Then                'ＳＰＣデータ
                    Master.ppRigthButton3.Text = sCnsUpButon        '更新
                Else
                    Master.ppRigthButton3.Text = sCnsKakuteiButon   '受付確定

                End If
        End Select
        Master.ppRigthButton2.Text = sCnsClrButon          'クリア
        Master.ppRigthButton1.Text = sCnsBakeButon         '元に戻す

        'ボタン活性化
        Master.ppLeftButton1.Enabled = True        '物品転送依頼
        Master.ppLeftButton2.Enabled = True        'ＴＢＯＸ照会
        Master.ppLeftButton3.Enabled = True        'シリアル登録
        Master.ppLeftButton4.Enabled = True        '資料請求
        Master.ppLeftButton5.Enabled = True        '工事連絡票一覧
        Master.ppLeftButton6.Enabled = True        '完了報告書
        Master.ppRigthButton6.Enabled = True       '工事進捗
        Master.ppRigthButton5.Enabled = True       'アップロード
        Master.ppRigthButton4.Enabled = True       '印刷
        Master.ppRigthButton3.Enabled = True       '受付確定

        ''項目非活性化
        Me.ProvisionalRequestNo.ppTextBoxOne.Enabled = False    '仮依頼番号
        Me.ProvisionalRequestNo.ppTextBoxTwo.Enabled = False    '仮依頼番号

        '工事依頼番号取得(仮登録データ)
        If ViewState("D39_CNST_NO") IsNot Nothing Then
            If strTerms.ToString.Trim = ClsComVer.E_遷移条件.更新 And ViewState("D39_CNST_NO").Contains(sCnsRequestNo) Then
                If Session(P_SESSION_AUTH) <> "営業所" Then
                    '--------------------------------
                    '2015/04/17 加賀　ここから
                    '--------------------------------
                    'strTerms = ClsComVer.E_遷移条件.仮登録
                    strTerms = 5    '仮依頼更新
                    '--------------------------------
                    '2015/04/17 加賀　ここまで
                    '--------------------------------
                End If
            End If
        End If

        '案件進捗状況引継ぎ内容選択
        Dim cnt As Integer = 0
        Dim status As String = ViewState("MTR_STATUS_CD").ToString
        For Each list As ListItem In Me.ddlMTRStatus.Items
            If list.Text.Contains(status) Then
                '--------------------------------
                '2015/04/18 加賀　ここから
                '--------------------------------
                'Me.ddlMTRStatus.SelectedIndex = cnt
                If strTerms = 5 Then
                    Me.ddlMTRStatus.SelectedValue = ViewState("MTR_STATUS_CD").ToString
                Else
                    Me.ddlMTRStatus.SelectedIndex = cnt

                End If
                '--------------------------------
                '2015/04/18 加賀　ここまで
                '--------------------------------
                Exit For
            End If
            cnt = cnt + 1
        Next

        Dim selecvalue As String
        If strTerms.ToString.Trim = ClsComVer.E_遷移条件.登録 Then  '受付確定(01:未処理)
            Dim mtrstatus As String() = status.Split(":")
            selecvalue = mtrstatus(0)
        Else
            selecvalue = Me.ddlMTRStatus.SelectedValue
        End If
        'ボタン制御
        Dim code As Integer = 0
        If Integer.TryParse(selecvalue, code) Then

            '完了報告 →　現場終了以降に活性化
            If code > 6 Then
                Master.ppLeftButton6.Enabled = True
            Else
                Master.ppLeftButton6.Enabled = False
            End If

            '工事進捗 →　現場作業待ち以降に活性化
            If code > 5 Then
                Master.ppRigthButton6.Enabled = True
            Else
                Master.ppRigthButton6.Enabled = False
            End If

            '物品転送依頼 → 営業所受託以降に活性
            If code > 3 Then
                Master.ppLeftButton1.Enabled = True
            Else
                Master.ppLeftButton1.Enabled = False
            End If

            '受付確定前（未処理） → 受付確定、クリア以外は非活性
            Select Case code
                Case 0
                    Master.ppLeftButton1.Enabled = False    '物品転送依頼
                    Master.ppLeftButton2.Enabled = False    'ＴＢＯＸ照会
                    Master.ppLeftButton3.Enabled = False    'シリアル登録
                    Master.ppLeftButton4.Enabled = False    '資料請求
                    Master.ppLeftButton5.Enabled = False    '工事連絡票一覧
                    Master.ppLeftButton6.Enabled = False    '完了報告書
                    Master.ppRigthButton6.Enabled = False   '工事進捗
                    Master.ppRigthButton5.Enabled = False   'アップロード
                    Master.ppRigthButton4.Enabled = True   '印刷
                    Master.ppRigthButton3.Enabled = False     '受付確定
                    Master.ppRigthButton2.Enabled = False    'クリア
                    Master.ppRigthButton1.Enabled = False   '元に戻す
                Case 1
                    Master.ppLeftButton1.Enabled = False    '物品転送依頼
                    Master.ppLeftButton2.Enabled = False    'ＴＢＯＸ照会
                    Master.ppLeftButton3.Enabled = False    'シリアル登録
                    Master.ppLeftButton4.Enabled = False    '資料請求
                    Master.ppLeftButton5.Enabled = False    '工事連絡票一覧
                    Master.ppLeftButton6.Enabled = False    '完了報告書
                    Master.ppRigthButton6.Enabled = False   '工事進捗
                    Master.ppRigthButton5.Enabled = False   'アップロード
                    Master.ppRigthButton4.Enabled = True   '印刷
                    Master.ppRigthButton3.Enabled = True    '受付確定
                    Master.ppRigthButton2.Enabled = True    'クリア
                    Master.ppRigthButton1.Enabled = False   '元に戻す
            End Select
        End If

        Select Case strTerms.ToString.Trim
            '--------------------------------
            '2015/04/17 加賀　ここから
            '--------------------------------
            'Case ClsComVer.E_遷移条件.仮登録
            Case ClsComVer.E_遷移条件.仮登録, 5 '仮依頼更新
                Select Case strTerms.ToString.Trim
                    Case ClsComVer.E_遷移条件.仮登録
                        'ボタン非活性化
                        Me.btmAttachRequestNo.Enabled = False   '依頼番号紐付け
                        Master.ppLeftButton1.Enabled = False    '物品転送依頼
                        Master.ppLeftButton2.Enabled = False    'ＴＢＯＸ照会
                        Master.ppLeftButton3.Enabled = False    'シリアル登録
                        Master.ppLeftButton4.Enabled = False    '資料請求
                        Master.ppLeftButton5.Enabled = False    '工事連絡票一覧
                        Master.ppLeftButton6.Enabled = False    '完了報告書
                        Master.ppRigthButton6.Enabled = False   '工事進捗
                        Master.ppRigthButton5.Enabled = False   'アップロード
                        Master.ppRigthButton4.Enabled = False   '印刷
                        'ボタン活性化
                        Master.ppRigthButton3.Enabled = True    '登録
                        Master.ppRigthButton2.Enabled = True    'クリア
                        Master.ppRigthButton1.Enabled = False   '元に戻す
                    Case Else
                        '仮依頼更新
                        Me.btmAttachRequestNo.Enabled = False   '依頼番号紐付け
                        Master.ppLeftButton1.Enabled = True        '物品転送依頼
                        Master.ppLeftButton2.Enabled = True        'ＴＢＯＸ照会
                        Master.ppLeftButton3.Enabled = True        'シリアル登録
                        Master.ppLeftButton4.Enabled = True        '資料請求
                        Master.ppLeftButton5.Enabled = True        '工事連絡票一覧
                        'Master.ppLeftButton6.Enabled = True        '完了報告書
                        Master.ppRigthButton6.Enabled = True       '工事進捗
                        Master.ppRigthButton5.Enabled = True       'アップロード
                        Master.ppRigthButton4.Enabled = True       '印刷
                        Master.ppRigthButton3.Enabled = True       '更新
                        Master.ppRigthButton2.Enabled = False   'クリア
                        Master.ppRigthButton1.Enabled = True    '元に戻す

                        Me.txtTboxId.ppEnabled = False
                End Select
                '--------------------------------
                '2015/04/17 加賀　ここまで
                '--------------------------------

                '項目非活性化
                Me.Panel1.Enabled = True
                If Me.ddlMTRStatus.SelectedValue = "00:仮登録" Then
                    Me.ddlMTRStatus.Enabled = False                 '案件進捗状況
                Else
                    Me.ddlMTRStatus.Enabled = True
                End If
                Me.txtSpecificationConnectingDivision.ppEnabled = False     '仕様連絡区分
                Me.ttwNotificationNo.ppEnabled = False          '通知番号
                '--------------------------------
                '2015/04/15 加賀　ここから
                '--------------------------------
                'Me.txtNLDivision.ppEnabled = False              'NL区分
                'Me.txtCurrentSys.ppEnabled = False              '現行システム
                'Me.txtVer.ppEnabled = False                     'ＶＥＲ
                Me.txtNLDivision.ppEnabled = True                'NL区分
                Me.txtCurrentSys.ppEnabled = True                '現行システム
                Me.txtVer.ppEnabled = True                       'ＶＥＲ
                '--------------------------------
                '2015/04/15 加賀　ここまで
                '--------------------------------
                Me.txtSysClassification.ppEnabled = False       'システム分類
                Me.txtHoleNm.ppEnabled = False                  'ホール名
                Me.txtTboxLine.ppEnabled = False                'ＴＢＯＸ回線
                Me.txtHoleCd.ppEnabled = False                  'ホールコード
                Me.txtHoleTelNo.ppEnabled = False               'ＴＥＬ
                Me.txtPersonInCharge_1.ppEnabled = False        '責任者
                Me.txtPersonInCharge_2.ppEnabled = False        '担当者
                Me.txtAddress.ppEnabled = False                 '住所

                Me.Panel2.Enabled = True
                Me.txtLanNew.ppEnabled = False              'LAN工事(新規)
                Me.txtLanExpansion.ppEnabled = False        'LAN工事(増設)
                Me.txtLanSomeRemoval.ppEnabled = False      'LAN工事(一部撤去)
                Me.txtLanShopRelocation.ppEnabled = False   'LAN工事(店舗移設)
                Me.txtLanAllRemoval.ppEnabled = False       'LAN工事(全撤去)
                Me.txtLanOnceRemoval.ppEnabled = False      'LAN工事(一時撤去)
                Me.txtLanReInstallation.ppEnabled = False   'LAN工事(再設置)
                Me.txtLanConChange.ppEnabled = False        'LAN工事(構成変更)
                Me.txtLanConDelively.ppEnabled = False      'LAN工事(構成配信)
                Me.txtLanOther.ppEnabled = False            'LAN工事(その他)

                Me.Panel3.Enabled = False
                Me.Panel4.Enabled = False
                '--------------------------------
                '2015/04/15 加賀　ここから
                '--------------------------------
                Me.txtOtherContents.ppEnabled = True
                '--------------------------------
                '2015/04/15 加賀　ここまで
                '--------------------------------
                Me.Panel5.Enabled = True

                'Me.txtOfficCD.ppEnabled = False             '担当営業所コード
                'Me.btnTrader.Enabled = False                '担当営業所 参照ボタン
                Me.dttStartOfCon.ppEnabled = False          '工事開始日
                Me.tmtStartOfCon.ppEnabled = False          '工事開始時刻
                Me.dttLastOpenDt.ppEnabled = False          '最終営業日
                Me.dttLastOpenDtT500.ppEnabled = False      '最終営業日(T500)
                If Me.ddlMTRStatus.SelectedValue = "00:仮登録" Then
                    Me.ddlPersonal1.Enabled = False             '作業担当者(総合テスト)
                    Me.dttTestDepartureDt.ppEnabled = False     '出発日(総合テスト)
                    Me.tmtTestDepartureTm.ppEnabled = False     '出発時刻(総合テスト)
                    Me.ddlPersonal2.Enabled = False                             '作業担当者(仮設置)
                    Me.dttTemporaryInstallationDepartureDt.ppEnabled = False    '出発日(仮設置)
                    Me.tmtTemporaryInstallationDepartureTm.ppEnabled = False    '出発時刻(仮設置)
                Else
                    Me.ddlPersonal1.Enabled = True             '作業担当者(総合テスト)
                    Me.dttTestDepartureDt.ppEnabled = True     '出発日(総合テスト)
                    Me.tmtTestDepartureTm.ppEnabled = True     '出発時刻(総合テスト)
                    Me.ddlPersonal2.Enabled = True                             '作業担当者(仮設置)
                    Me.dttTemporaryInstallationDepartureDt.ppEnabled = True    '出発日(仮設置)
                    Me.tmtTemporaryInstallationDepartureTm.ppEnabled = True    '出発時刻(仮設置)
                End If
                Me.dttTemporaryInstallationCnsDivision.ppEnabled = False    '仮設置工事区分
                Me.dttTemporaryInstallationDtNotInputDivision0.ppEnabled = False    '仮設置日未入力区分
                Me.dttPolice.ppEnabled = False              '警察(日付)
                Me.tmtPolice.ppEnabled = False              '警察(時刻)
                Me.dttShiftCnsDivision.ppEnabled = False    '移行工事区分
                Me.dttShiftCnsWorkDivision.ppEnabled = False    '移行工事作業区分
                Me.dttOpen.ppEnabled = False                'オープン日
                Me.tmtOpen.ppEnabled = False                'オープン時刻
                Me.dttLanCns.ppEnabled = False              'LAN工事日
                Me.tmtLanCns.ppEnabled = False              'LAN工事時刻
                Me.dttVerup.ppEnabled = False               'VERUP日
                Me.tmtVerup.ppEnabled = False               'VERUP時刻
                Me.dttVerupDtDivision.ppEnabled = False     'VERUP日付区分
                Me.txtVerupCnsType_1.ppEnabled = False      'VERUP工事種類1
                Me.txtVerupCnsType_2.ppEnabled = False      'VERUP工事種類2

                Me.Panel6.Enabled = False
                '--------------------------------
                '2015/04/15 加賀　ここから
                '--------------------------------
                Me.Panel7.Enabled = True
                Me.Panel8.Enabled = False
                'Me.Panel9.Enabled = False
                Select Case strTerms.ToString.Trim
                    Case ClsComVer.E_遷移条件.仮登録
                        Me.Panel9.Enabled = True
                    Case Else
                        '仮依頼更新
                        'CNSUPDP001_003
                        'Me.Panel9.Enabled = False
                        Me.cbxDllSettingChange.Enabled = True
                        Me.trfSfPersonInCharge.ppEnabled = False
                        Me.RadioButtonList1.Enabled = False
                        Me.txtProcessingResultDetail1.ppEnabled = False
                        Me.txtProcessingResultDetail2.ppEnabled = False
                        Me.txtProcessingResultDetail3.ppEnabled = False
                        Me.txtControlInfoRemarks1.ppEnabled = False
                        Me.txtControlInfoRemarks2.ppEnabled = False
                        Me.txtControlInfoRemarks3.ppEnabled = False
                        'CNSUPDP001_003 END
                End Select
                '--------------------------------
                '2015/04/15 加賀　ここまで
                '--------------------------------

                If Integer.TryParse(selecvalue, code) Then
                    '工事進捗 →　現場作業待ち以降に活性化
                    If code > 5 Then
                        Master.ppRigthButton6.Enabled = True
                    Else
                        Master.ppRigthButton6.Enabled = False
                    End If

                    '物品転送依頼 → 営業所受託以降に活性
                    If code > 3 Then
                        Master.ppLeftButton1.Enabled = True
                    Else
                        Master.ppLeftButton1.Enabled = False
                    End If
                End If
            Case ClsComVer.E_遷移条件.登録
                '-----------------------------
                '2014/04/19 高松　ここから
                '-----------------------------
                'ボタン非活性化
                Master.ppRigthButton2.Enabled = True     'クリア
                Master.ppRigthButton1.Enabled = False    '元に戻す
                '-----------------------------
                '2014/04/19 高松　ここまで
                '-----------------------------
                '項目非活性化
                Me.Panel1.Enabled = False
                Me.Panel2.Enabled = False
                Me.Panel3.Enabled = False
                Me.Panel4.Enabled = False
                '--------------------------------
                '2015/04/15 加賀　ここから
                '--------------------------------
                Me.txtOtherContents.ppEnabled = False
                '--------------------------------
                '2015/04/15 加賀　ここまで
                '--------------------------------
                Me.Panel5.Enabled = False
                Me.Panel6.Enabled = False
                Me.Panel7.Enabled = False
                Me.Panel8.Enabled = False
                If Me.txtSfOperation.ppText = "1" Then
                    Me.txtOfficCD.ppEnabled = False
                    Me.btnTrader.Enabled = False
                End If
            Case ClsComVer.E_遷移条件.更新
                '-----------------------------
                '2014/04/19 高松　ここから
                '-----------------------------
                'ボタン非活性化
                Master.ppRigthButton2.Enabled = False    'クリア
                Master.ppRigthButton1.Enabled = True     '元に戻す
                Master.ppRigthButton3.Enabled = True     '更新
                '項目非活性化
                Me.Panel1.Enabled = True
                Me.Panel2.Enabled = True
                Me.Panel3.Enabled = True
                Me.Panel4.Enabled = True
                '--------------------------------
                '2015/04/15 加賀　ここから
                '--------------------------------
                Me.txtOtherContents.ppEnabled = True
                '--------------------------------
                '2015/04/15 加賀　ここまで
                '--------------------------------
                Me.Panel5.Enabled = True
                Me.txtOfficCD.ppEnabled = True
                ''ビューステート項目取得(仕様連絡区分)
                'Dim key As String() = ViewState(P_KEY)
                'If key.GetLength(0) = 4 Then
                '    If key(3).ToString = "キャンセル" Then
                '        'txtOfficCD.ppEnabled = False  '担当営業所コード
                '    End If
                'End If
                If Me.txtSfOperation.ppText = 0 Then
                    Me.txtOfficCD.ppEnabled = True
                    Me.btnTrader.Enabled = True
                Else
                    Me.txtOfficCD.ppEnabled = False
                End If
                Me.dttStartOfCon.ppEnabled = True
                Me.tmtStartOfCon.ppEnabled = True
                Me.dttLastOpenDt.ppEnabled = True
                Me.dttLastOpenDtT500.ppEnabled = True
                Me.dttTest.ppEnabled = True
                Me.tmtTest.ppEnabled = True
                Me.ddlPersonal1.Enabled = True
                Me.dttTestDepartureDt.ppEnabled = True
                Me.tmtTestDepartureTm.ppEnabled = True
                Me.dttTemporaryInstallation.ppEnabled = True
                Me.tmtTemporaryInstallation.ppEnabled = True
                Me.ddlPersonal2.Enabled = True
                Me.dttTemporaryInstallationDepartureDt.ppEnabled = True
                Me.tmtTemporaryInstallationDepartureTm.ppEnabled = True
                Me.dttTemporaryInstallationCnsDivision.ppEnabled = True
                Me.dttTemporaryInstallationDtNotInputDivision0.ppEnabled = True
                Me.dttPolice.ppEnabled = True
                Me.tmtPolice.ppEnabled = True
                Me.dttShiftCnsDivision.ppEnabled = True
                Me.dttShiftCnsWorkDivision.ppEnabled = True
                Me.dttOpen.ppEnabled = True
                Me.tmtOpen.ppEnabled = True
                Me.dttLanCns.ppEnabled = True
                Me.tmtLanCns.ppEnabled = True
                Me.dttVerup.ppEnabled = True
                Me.tmtVerup.ppEnabled = True
                Me.dttVerupDtDivision.ppEnabled = True
                Me.txtVerupCnsType_1.ppEnabled = True
                Me.txtVerupCnsType_2.ppEnabled = True
                Me.Panel6.Enabled = True
                Me.Panel7.Enabled = True
                Me.Panel8.Enabled = True
                '-----------------------------
                '2014/04/19 高松　ここまで
                '-----------------------------
                'CNSUPDP001_003
                'Me.Panel9.Enabled = False
                Me.cbxDllSettingChange.Enabled = True
                Me.trfSfPersonInCharge.ppEnabled = False
                Me.RadioButtonList1.Enabled = False
                Me.txtProcessingResultDetail1.ppEnabled = False
                Me.txtProcessingResultDetail2.ppEnabled = False
                Me.txtProcessingResultDetail3.ppEnabled = False
                Me.txtControlInfoRemarks1.ppEnabled = False
                Me.txtControlInfoRemarks2.ppEnabled = False
                Me.txtControlInfoRemarks3.ppEnabled = False
                Me.ProvisionalRequestNo.ppTextBoxTwo.Enabled = True    '仮依頼番号
                'CNSUPDP001_003 END

                If Session(P_SESSION_AUTH) = "営業所" Then

                    Me.Panel1.Enabled = False
                    Me.Panel2.Enabled = False
                    Me.Panel3.Enabled = False
                    Me.Panel4.Enabled = False
                    Me.Panel6.Enabled = False
                    '--------------------------------
                    '2015/04/15 加賀　ここから
                    '--------------------------------
                    Me.Panel7.Enabled = False
                    Me.Panel8.Enabled = False
                    Me.txtOtherContents.ppEnabled = False
                    '--------------------------------
                    '2015/04/15 加賀　ここまで
                    '--------------------------------
                    'Me.txtOfficCD.ppEnabled = False
                    'Me.btnTrader.Enabled = False
                    Me.dttStartOfCon.ppEnabled = False
                    Me.tmtStartOfCon.ppEnabled = False
                    Me.dttLastOpenDt.ppEnabled = False
                    Me.dttLastOpenDtT500.ppEnabled = False
                    Me.dttTest.ppEnabled = False
                    Me.tmtTest.ppEnabled = False
                    Me.dttTemporaryInstallation.ppEnabled = False
                    Me.tmtTemporaryInstallation.ppEnabled = False
                    Me.dttTemporaryInstallationCnsDivision.ppEnabled = False
                    Me.dttTemporaryInstallationDtNotInputDivision0.ppEnabled = False
                    Me.dttPolice.ppEnabled = False
                    Me.tmtPolice.ppEnabled = False
                    Me.dttShiftCnsDivision.ppEnabled = False
                    Me.dttShiftCnsWorkDivision.ppEnabled = False
                    Me.dttOpen.ppEnabled = False
                    Me.tmtOpen.ppEnabled = False
                    Me.dttLanCns.ppEnabled = False
                    Me.tmtLanCns.ppEnabled = False
                    Me.dttVerup.ppEnabled = False
                    Me.tmtVerup.ppEnabled = False
                    Me.dttVerupDtDivision.ppEnabled = False
                    Me.txtVerupCnsType_1.ppEnabled = False
                    Me.txtVerupCnsType_2.ppEnabled = False
                    If Not Me.ddlMTRStatus.SelectedValue Is Nothing Then
                        '入力項目を非活性
                        If CInt(Me.ddlMTRStatus.SelectedValue) > 7 Then
                            Me.txtOfficCD.ppEnabled = False
                            Me.btnTrader.Enabled = False
                            Me.ddlPersonal1.Enabled = False
                            Me.dttTestDepartureDt.ppEnabled = False
                            Me.tmtTestDepartureTm.ppEnabled = False
                            Me.ddlPersonal2.Enabled = False
                            Me.dttTemporaryInstallationDepartureDt.ppEnabled = False
                            Me.tmtTemporaryInstallationDepartureTm.ppEnabled = False
                            Me.cbxDllSettingChange.Enabled = False
                        End If
                    End If
                End If
            Case ClsComVer.E_遷移条件.参照
                'ボタン非活性化
                Master.ppRigthButton2.Enabled = False  'クリア
                Master.ppRigthButton1.Enabled = False  '元に戻す
                Master.ppRigthButton3.Enabled = False  '更新
                '項目非活性化
                Me.Panel1.Enabled = False
                Me.Panel2.Enabled = False
                Me.Panel3.Enabled = False
                Me.Panel4.Enabled = False
                '--------------------------------
                '2015/04/15 加賀　ここから
                '--------------------------------
                Me.txtOtherContents.ppEnabled = False
                '--------------------------------
                '2015/04/15 加賀　ここまで
                '--------------------------------
                Me.Panel5.Enabled = False
                Me.Panel6.Enabled = False
                Me.Panel7.Enabled = False
                Me.Panel8.Enabled = False
                'CNSUPDP001_003
                'Me.Panel9.Enabled = False
                Me.cbxDllSettingChange.Enabled = True
                Me.trfSfPersonInCharge.ppEnabled = False
                Me.RadioButtonList1.Enabled = False
                Me.txtProcessingResultDetail1.ppEnabled = False
                Me.txtProcessingResultDetail2.ppEnabled = False
                Me.txtProcessingResultDetail3.ppEnabled = False
                Me.txtControlInfoRemarks1.ppEnabled = False
                Me.txtControlInfoRemarks2.ppEnabled = False
                Me.txtControlInfoRemarks3.ppEnabled = False
                'CNSUPDP001_003 END
        End Select
        'End If


        '総合テスト日作業担当者
        If Not ViewState("D39_STEST_CHARGE_CD") Is Nothing Then
            Dim cnt1 As Integer = 0
            Dim name1 As String = ViewState("D39_STEST_CHARGE_CD").ToString

            For Each list As ListItem In Me.ddlPersonal1.Items
                If list.Text.Contains(name1) Then
                    Me.ddlPersonal1.SelectedIndex = cnt1
                    Exit For
                End If
                cnt1 = cnt1 + 1
            Next
        End If

        '仮設置日作業担当者
        If Not ViewState("D39_TMPSET_CHARGE_CD") Is Nothing Then
            Dim cnt2 As Integer = 0
            Dim name2 As String = ViewState("D39_TMPSET_CHARGE_CD").ToString

            For Each list As ListItem In Me.ddlPersonal2.Items
                If list.Text.Contains(name2) Then
                    Me.ddlPersonal2.SelectedIndex = cnt2
                    Exit For
                End If
                cnt2 = cnt2 + 1
            Next
        End If

        If Session(P_SESSION_AUTH) <> "営業所" And strTerms.ToString.Trim <> ClsComVer.E_遷移条件.参照 Then
            '受付確定時は担当営業所を活性
            If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                Me.Panel5.Enabled = True

                Me.txtOfficCD.ppEnabled = True
                If Me.txtSfOperation.ppText = 0 Then
                    Me.txtOfficCD.ppEnabled = True
                    Me.btnTrader.Enabled = True
                Else
                    Me.txtOfficCD.ppEnabled = False
                End If
                Me.dttStartOfCon.ppEnabled = False
                Me.tmtStartOfCon.ppEnabled = False
                Me.dttLastOpenDt.ppEnabled = False
                Me.dttLastOpenDtT500.ppEnabled = False
                Me.dttTest.ppEnabled = False
                Me.tmtTest.ppEnabled = False
                Me.ddlPersonal1.Enabled = False
                Me.dttTestDepartureDt.ppEnabled = False
                Me.tmtTestDepartureTm.ppEnabled = False
                Me.dttTemporaryInstallation.ppEnabled = False
                Me.tmtTemporaryInstallation.ppEnabled = False
                Me.ddlPersonal2.Enabled = False
                Me.dttTemporaryInstallationDepartureDt.ppEnabled = False
                Me.tmtTemporaryInstallationDepartureTm.ppEnabled = False
                Me.dttTemporaryInstallationCnsDivision.ppEnabled = False
                Me.dttTemporaryInstallationDtNotInputDivision0.ppEnabled = False
                Me.dttPolice.ppEnabled = False
                Me.tmtPolice.ppEnabled = False
                Me.dttShiftCnsDivision.ppEnabled = False
                Me.dttShiftCnsWorkDivision.ppEnabled = False
                Me.dttOpen.ppEnabled = False
                Me.tmtOpen.ppEnabled = False
                Me.dttLanCns.ppEnabled = False
                Me.tmtLanCns.ppEnabled = False
                Me.dttVerup.ppEnabled = False
                Me.tmtVerup.ppEnabled = False
                Me.dttVerupDtDivision.ppEnabled = False
                Me.txtVerupCnsType_1.ppEnabled = False
                Me.txtVerupCnsType_2.ppEnabled = False
            End If
        End If

    End Sub

#End Region

#Region "必須項目設定処理"

    ''' <summary>
    ''' 必須項目の設定(True：必須/False：任意)処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEdit_RequiredField()

        Dim strTerms As String = ViewState(P_SESSION_TERMS)

        '工事依頼番号取得(仮登録データ)
        If ViewState("D39_CNST_NO") IsNot Nothing Then
            If ViewState("D39_CNST_NO").Contains(sCnsRequestNo) Then
                If Session(P_SESSION_AUTH) <> "営業所" Then
                    strTerms = ClsComVer.E_遷移条件.仮登録
                End If
            End If
        End If

        Select Case strTerms.ToString.Trim
            Case ClsComVer.E_遷移条件.仮登録
                'Me.txtSpecificationConnectingDivision.ppRequiredField = True    '連絡区分
                'Me.txtNLDivision.ppRequiredField = True                         'ＮＬ区分
                'Me.txtCurrentSys.ppRequiredField = True                         '現行システム
                'Me.txtVer.ppRequiredField = True                                'ＶＥＲ
                'Me.txtSysClassification.ppRequiredField = True                  'システム分類
                Me.txtTboxId.ppRequiredField = True                             'ＴＢＯＸＩＤ
                'Me.txtHoleNm.ppRequiredField = True                             'ホール名
                'Me.txtTboxLine.ppRequiredField = True                           'ＴＢＯＸ回線
                'Me.txtHoleCd.ppRequiredField = True                             'ホールコード
                'Me.txtHoleTelNo.ppRequiredField = True                          'ＴＥＬ
                'Me.txtAddress.ppRequiredField = True                            '住所

                Me.txtHoleNew.ppRequiredField = True                            '新規
                Me.txtHoleExpansion.ppRequiredField = True                      '増設
                Me.txtHoleSomeRemoval.ppRequiredField = True                    '一部撤去
                Me.txtHoleShopRelocation.ppRequiredField = True                 '店舗移設
                Me.txtHoleAllRemoval.ppRequiredField = True                     '全撤去
                Me.txtHoleOnceRemoval.ppRequiredField = True                    '一時撤去
                Me.txtHoleReInstallation.ppRequiredField = True                 '再設置
                Me.txtHoleConChange.ppRequiredField = True                      '構成変更
                Me.txtHoleConDelively.ppRequiredField = True                    '構成配信
                Me.txtHoleOther.ppRequiredField = True                          'その他

                'Me.txtLanNew.ppRequiredField = True                             '新規
                'Me.txtLanExpansion.ppRequiredField = True                       '増設
                'Me.txtLanSomeRemoval.ppRequiredField = True                     '一部撤去
                'Me.txtLanShopRelocation.ppRequiredField = True                  '店舗移設
                'Me.txtLanAllRemoval.ppRequiredField = True                      '全撤去
                'Me.txtLanOnceRemoval.ppRequiredField = True                      '一時撤去
                'Me.txtLanReInstallation.ppRequiredField = True                  '再設置
                'Me.txtLanConChange.ppRequiredField = True                       '構成変更
                'Me.txtLanConDelively.ppRequiredField = True                     '構成配信
                'Me.txtLanOther.ppRequiredField = True                           'その他

                'Me.txtTwinsShop.ppRequiredField = True                          '双子店
                'Me.txtIndependentCns.ppRequiredField = True                     '単独工事
                'Me.txtSameTimeCnsNo.ppRequiredField = True                      '同時工事数
                'Me.txtPAndCDivision.ppRequiredField = True                      '親子区分
                'Me.txtParentHoleCd.ppRequiredField = True                       '親ホールコード

                'Me.txtConstructionExistenceF1.ppRequiredField = True            '工事有無Ｆ１
                'Me.txtConstructionExistenceF2.ppRequiredField = True            '工事有無Ｆ２
                'Me.txtConstructionExistenceF3.ppRequiredField = True            '工事有無Ｆ３
                'Me.txtConstructionExistenceF4.ppRequiredField = True            '工事有無Ｆ４
                'Me.dttStartOfCon.ppRequiredField = True                         '工事開始
                'Me.dttLastOpenDt.ppRequiredField = True                         '最終営業日
                'Me.dttLastOpenDtT500.ppRequiredField = True                     '最終営業日Ｔ５００
                Me.txtOfficCD.ppRequiredField = True                            '担当営業所コード
                Me.dttTest.ppRequiredField = True                               '総合テスト
                Me.dttTemporaryInstallation.ppRequiredField = True              '仮設置

                'Me.ProvisionalRequestNo.ppRequiredField = True    '仮依頼番号

            Case ClsComVer.E_遷移条件.更新

                '作業者、出発時間を必須入力にする
                If Session(P_SESSION_AUTH) = "営業所" Then
                    Me.ddlPersonal1.Enabled = True                                  '作業担当者
                    Me.dttTestDepartureDt.ppRequiredField = True                    '出発日時
                    Me.ddlPersonal2.Enabled = True                                  '作業担当者
                    Me.dttTemporaryInstallationDepartureDt.ppRequiredField = True   '出発日時

                    '総合テスト、仮設置
                    'If Me.dttTest.ppText.Trim = String.Empty Then
                    '    Me.txtTestWorkPersonnel.ppRequiredField = False                   '作業担当者
                    '    Me.dttTestDepartureDt.ppRequiredField = False                     '出発日時
                    'Else
                    '    Me.txtTestWorkPersonnel.ppRequiredField = True                  '作業担当者
                    '    Me.dttTestDepartureDt.ppRequiredField = True                    '出発日時
                    'End If
                    'If Me.dttTemporaryInstallation.ppText.Trim = String.Empty Then
                    '    Me.txtTemporaryInstallationWorkPersonnel.ppRequiredField = False  '作業担当者
                    '    Me.dttTemporaryInstallationDepartureDt.ppRequiredField = False    '出発日時
                    'Else
                    '    Me.txtTemporaryInstallationWorkPersonnel.ppRequiredField = True '作業担当者
                    '    Me.dttTemporaryInstallationDepartureDt.ppRequiredField = True   '出発日時
                    'End If
                End If


            Case ClsComVer.E_遷移条件.登録  '受付確定
                trfSfPersonInCharge.ppRequiredField = True  'エフエス担当者
                txtOfficCD.ppRequiredField = True  '担当営業所コード

                ''ビューステート項目取得(仕様連絡区分)
                'Dim key As String() = ViewState(P_KEY)
                'If key.GetLength(0) = 4 Then
                '    If key(3).ToString = "キャンセル" Then
                '        'txtOfficCD.ppRequiredField = False  '担当営業所コード
                '    End If
                'End If
        End Select
    End Sub

#End Region

#Region "データ取得処理"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="dstOrders_1"></param>
    ''' <param name="dstOrders_2"></param>
    ''' <remarks></remarks>
    Private Function msGet_Data(ByRef dstOrders_1 As DataSet _
                               , ByRef dstOrders_2 As DataSet _
                               , ByRef dstOrders_3 As DataSet _
                               , ByRef dstOrders_4 As DataSet _
                               , ByRef dstOrders_5 As DataSet)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Dim strKey() As String = Nothing
        Dim strTerms As String = String.Empty
        objStack = New StackFrame

        Try
            'ビューステート項目取得
            strKey = ViewState(P_KEY)
            strTerms = ViewState(P_SESSION_TERMS)

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Return False
            End If

            If strTerms.ToString = ClsComVer.E_遷移条件.仮登録 Then
                ViewState("MTR_STATUS_CD") = "03"
            Else
                'SPCデータ重複確認
                If strKey(0) <> Nothing Then
                    cmdDB = New SqlCommand(sCnsSqlid_S16, conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))
                    End With
                    dstOrders_5 = clsDataConnect.pfGet_DataSet(cmdDB)

                    If strKey(1).ToString = "0" Then        'ＳＰＣデータ
                        '***** 工事依頼書兼仕様書データ取得 *****
                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S10, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))              '依頼番号
                        End With
                        dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)
                        'ビューステート案件ステータス設定
                        ViewState("MTR_STATUS_CD") = dstOrders_1.Tables(0).Rows(0).Item("D39_MTR_STATUS_CD").ToString
                        ViewState("D39_CNST_NO") = dstOrders_1.Tables(0).Rows(0).Item("D39_CNST_NO").ToString
                        ViewState("D39_STEST_CHARGE_CD") = dstOrders_1.Tables(0).Rows(0).Item("D39_STEST_CHARGE_CD").ToString
                        ViewState("D39_TMPSET_CHARGE_CD") = dstOrders_1.Tables(0).Rows(0).Item("D39_TMPSET_CHARGE_CD").ToString

                        '***** 工事依頼書兼仕様書　機器明細１取得 *****
                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S2, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))              '依頼番号
                        End With
                        dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                        '***** 工事設計依頼請書取得 *****
                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S7, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))              '依頼番号
                        End With
                        dstOrders_3 = clsDataConnect.pfGet_DataSet(cmdDB)

                    Else                                    'ＴＯＭＡＳデータ
                        '***** 工事依頼書兼仕様書データ取得 *****
                        'SPCデータ重複確認
                        cmdDB = New SqlCommand(sCnsSqlid_S16, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))
                        End With
                        dstOrders_5 = clsDataConnect.pfGet_DataSet(cmdDB)

                        If dstOrders_5.Tables(0).Rows.Count <> 0 Then
                            '重複していた場合

                            '***** 工事設計依頼請書取得 *****
                            'パラメータ設定
                            cmdDB = New SqlCommand(sCnsSqlid_S7, conDB)
                            With cmdDB.Parameters
                                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))
                            End With
                            dstOrders_3 = clsDataConnect.pfGet_DataSet(cmdDB)

                        End If
                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S1, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))              '依頼番号
                        End With
                        dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)
                        'ビューステート案件ステータス設定
                        ViewState("MTR_STATUS_CD") = "01:未処理"
                        ViewState("D39_CNST_NO") = dstOrders_1.Tables(0).Rows(0).Item("D39_CNST_NO").ToString

                        '***** 工事設計依頼明細取得 *****
                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S8, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))              '依頼番号
                        End With
                        dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)

                        '***** 工事依頼書兼仕様書　機器明細１取得 *****
                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S2, conDB)
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strKey(0)))              '依頼番号
                        End With
                        dstOrders_4 = clsDataConnect.pfGet_DataSet(cmdDB)
                    End If
                End If

                '明細データが存在する場合、一覧表示
                If dstOrders_2.Tables(0).Rows.Count > 0 Then
                    Me.grvList.DataSource = dstOrders_2
                Else
                    Me.grvList.DataSource = New DataTable
                End If
            End If

            'ビューステート仮受付番号設定
            ViewState("PRECNST_NO") = String.Empty


            'データを反映
            Me.grvList.DataBind()

        Catch ex As DBConcurrencyException
            psMesBox(Me, "0005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.grvList.DataBind()
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

        Return True

    End Function

#End Region

#Region "編集表示処理"

    ''' <summary>
    ''' 編集表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_Edit(ByVal dstOrders_1 As DataSet _
                               , ByVal dstOrders_3 As DataSet _
                               , ByVal dstorders_5 As DataSet)

        Dim strTerms As String = String.Empty
        Dim strKeys() As String 'CNSUPDP001_011
        Dim intRow As Integer = 0
        Dim intCnt As Integer = 0
        Dim intCnt2 As Integer = intCnt + 1

        'フォントカラー初期化
        msClearFontColor()

        'ビューステート項目取得
        strTerms = ViewState(P_SESSION_TERMS)
        strKeys = ViewState(P_KEY) 'CNSUPDP001_011

        With dstOrders_1.Tables(0)

            'If strTerms = ClsComVer.E_遷移条件.登録 Then 'CNSUPDP001_011
            If strTerms = ClsComVer.E_遷移条件.登録 OrElse (strTerms = ClsComVer.E_遷移条件.参照 AndAlso strKeys(1) = "1") Then 'CNSUPDP001_011
                'For intCnt2 As Integer = intCnt + 1 To dstOrders_1.Tables(0).Rows.Count - 1
                If intCnt2 < dstOrders_1.Tables(0).Rows.Count Then
                    If .Rows(intRow)("D39_TELL_CLS").ToString.Equals("3") Then   '３：キャンセル
                        intCnt = intCnt2
                    End If
                    intRow = intCnt2
                End If
            Else
                If dstOrders_1.Tables(0).Rows.Count > 1 Then
                    intCnt = 1
                    intRow = 0
                Else
                    intCnt = 0
                    intRow = 0
                End If
            End If

            If dstOrders_1.Tables(0).Rows.Count = 0 Then
                Return False
                Exit Function
            End If


            If .Rows(intRow)("D39_PRECNST_NO").ToString.Length > 0 Then
                Dim one As String = .Rows(intRow)("D39_PRECNST_NO").ToString.Substring(0, 5)
                Dim two As String = .Rows(intRow)("D39_PRECNST_NO").ToString.Substring(6)
                Me.ProvisionalRequestNo.ppTextBoxOne.Text = one
                Me.ProvisionalRequestNo.ppTextBoxTwo.Text = two
            Else
                Me.ProvisionalRequestNo.ppTextBoxOne.Text = sCnsRequestNo
                Me.ProvisionalRequestNo.ppTextBoxTwo.Text = ""
            End If

            ''CNSUPDP001_014
            lblEmergency.Text = .Rows(intRow)("EMREQ_CLS").ToString
            ''CNSUPDP001_014 END

            If Not .Rows(intCnt)("D39_TMPSTTS_CD").ToString.Equals(.Rows(intRow)("D39_TMPSTTS_CD").ToString) Then
                Me.lbltMpsttsCd.ForeColor = Drawing.Color.Red
            End If

            Me.lbltMpsttsCd.Text = .Rows(intRow)("TMPSTTS_NM").ToString.Trim                                             '仮設置進捗状況

            If Not .Rows(intCnt)("D39_STATUS_CD").ToString.Equals(.Rows(intRow)("D39_STATUS_CD").ToString) Then
                Me.lblStatusCd.ForeColor = Drawing.Color.Red
            End If

            Me.lblStatusCd.Text = .Rows(intRow)("STATUS_NM").ToString.Trim                                                  '本設置進捗状況

            'Dim aaa As String = dstOrders_1.Tables(0).Rows(intCnt)("D39_REQ_STATE").ToString
            'Dim bbb As String = dstOrders_1.Tables(0).Rows(intRow)("D39_REQ_STATE").ToString
            If Not .Rows(intCnt)("D39_REQ_STATE").ToString.Equals(.Rows(intRow)("D39_REQ_STATE").ToString) Then
                Me.lblReqState.ForeColor = Drawing.Color.Red
            End If
            Me.lblReqState.Text = .Rows(intRow)("D39_REQ_STATE").ToString.Trim                                              '資料請求状況

            If Not .Rows(intCnt)("D39_CNST_NO").ToString.Equals(.Rows(intRow)("D39_CNST_NO").ToString) Then
                Me.lblRequestNo_2.ForeColor = Drawing.Color.Red
            End If
            Me.lblRequestNo_2.Text = .Rows(intRow)("D39_CNST_NO").ToString.Trim                                             '依頼番号

            If Not .Rows(intCnt)("D39_RECEIVE_CNT").ToString.Equals(.Rows(intRow)("D39_RECEIVE_CNT").ToString) Then
                Me.lblNumberOfUpdates.ForeColor = Drawing.Color.Red
            End If
            Me.lblNumberOfUpdates.Text = .Rows(intRow)("D39_RECEIVE_CNT").ToString.Trim                                   '確定回数/受信回数

            If Not .Rows(intCnt)("D39_DATARCV_DT").ToString.Equals(.Rows(intRow)("D39_DATARCV_DT").ToString) Then
                Me.lblReceptionDt_2.ForeColor = Drawing.Color.Red
            End If
            Me.lblReceptionDt_2.Text = .Rows(intRow)("D39_DATARCV_DT").ToString.Trim                                        '受付日付

            If Not .Rows(intCnt)("D39_DATARCV_TM").ToString.Equals(.Rows(intRow)("D39_DATARCV_TM").ToString) Then
                Me.lblReceptionTm_2.ForeColor = Drawing.Color.Red
            End If
            Me.lblReceptionTm_2.Text = .Rows(intRow)("D39_DATARCV_TM").ToString.Trim                                         '受付日付

            If Not .Rows(intCnt)("D39_TELL_CLS").ToString.Equals(.Rows(intRow)("D39_TELL_CLS").ToString) Then
                Me.txtSpecificationConnectingDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSpecificationConnectingDivision.ppText = .Rows(intRow)("D39_TELL_CLS").ToString.Trim                     '連絡区分

            If Not .Rows(intCnt)("D39_NL_CLS").ToString.Equals(.Rows(intRow)("D39_NL_CLS").ToString) Then
                Me.txtNLDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtNLDivision.ppText = .Rows(intRow)("D39_NL_CLS").ToString.Trim                                         'ＮＬ区分

            If Not .Rows(intCnt)("D39_CNST_COM_NO").ToString.Equals(.Rows(intRow)("D39_CNST_COM_NO").ToString) Then
                Me.ttwNotificationNo.ppTextBoxOne.ForeColor = Drawing.Color.Red
                Me.ttwNotificationNo.ppTextBoxTwo.ForeColor = Drawing.Color.Red
            End If
            Me.ttwNotificationNo.ppText = .Rows(intRow)("D39_CNST_COM_NO").ToString.Trim                                  '通知番号

            If Not .Rows(intCnt)("D39_H_TBOXCLASS").ToString.Equals(.Rows(intRow)("D39_H_TBOXCLASS").ToString) Then
                Me.txtCurrentSys.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtCurrentSys.ppText = .Rows(intRow)("D39_H_TBOXCLASS").ToString.Trim                                        '現行システム

            If Not .Rows(intCnt)("D39_H_VERSION").ToString.Equals(.Rows(intRow)("D39_H_VERSION").ToString) Then
                Me.txtVer.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtVer.ppText = .Rows(intRow)("D39_H_VERSION").ToString.Trim                                               'ＶＥＲ

            If Not .Rows(intCnt)("D39_SYSTEM_GRP").ToString.Equals(.Rows(intRow)("D39_SYSTEM_GRP").ToString) Then
                Me.txtSysClassification.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSysClassification.ppText = .Rows(intRow)("D39_SYSTEM_GRP").ToString.Trim                                 'システム分類

            If Not .Rows(intCnt)("D39_TBOXID").ToString.Equals(.Rows(intRow)("D39_TBOXID").ToString) Then
                Me.txtTboxId.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtTboxId.ppText = .Rows(intRow)("D39_TBOXID").ToString.Trim                                              'ＴＢＯＸＩＤ

            If Not .Rows(intCnt)("D39_HALL_NM").ToString.Trim.Equals(.Rows(intRow)("D39_HALL_NM").ToString.Trim) Then
                Me.txtHoleNm.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleNm.ppText = .Rows(intRow)("D39_HALL_NM").ToString.Trim                                               'ホール名

            If Not .Rows(intCnt)("D39_H_TBOX_TELNO").ToString.Equals(.Rows(intRow)("D39_H_TBOX_TELNO").ToString) Then
                Me.txtTboxLine.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtTboxLine.ppText = .Rows(intRow)("D39_H_TBOX_TELNO").ToString.Trim                                       'ＴＢＯＸ回線

            If Not .Rows(intCnt)("D39_HALL_CD").ToString.Equals(.Rows(intRow)("D39_HALL_CD").ToString) Then
                Me.txtHoleCd.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleCd.ppText = .Rows(intRow)("D39_HALL_CD").ToString.Trim                                                'ホールコード

            If Not .Rows(intCnt)("D39_H_TELNO").ToString.Equals(.Rows(intRow)("D39_H_TELNO").ToString) Then
                Me.txtHoleTelNo.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleTelNo.ppText = .Rows(intRow)("D39_H_TELNO").ToString.Trim                                              'ＴＥＬ

            If Not .Rows(intCnt)("D39_H_RSP").ToString.Equals(.Rows(intRow)("D39_H_RSP").ToString) Then
                Me.txtPersonInCharge_1.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtPersonInCharge_1.ppText = .Rows(intRow)("D39_H_RSP").ToString.Trim                                      '責任者

            If Not .Rows(intCnt)("D39_H_CHARGE").ToString.Equals(.Rows(intRow)("D39_H_CHARGE").ToString) Then
                Me.txtPersonInCharge_2.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtPersonInCharge_2.ppText = .Rows(intRow)("D39_H_CHARGE").ToString.Trim                                   '担当者

            If Not .Rows(intCnt)("D39_H_ADDR").ToString.Trim.Equals(.Rows(intRow)("D39_H_ADDR").ToString.Trim) Then
                Me.txtAddress.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAddress.ppText = .Rows(intRow)("D39_H_ADDR").ToString.Trim                                               '住所

            If Not .Rows(intCnt)("D39_H_NEW").ToString.Equals(.Rows(intRow)("D39_H_NEW").ToString) Then
                Me.txtHoleNew.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleNew.ppText = .Rows(intRow)("D39_H_NEW").ToString.Trim                                               '新規

            If Not .Rows(intCnt)("D39_H_ADD").ToString.Equals(.Rows(intRow)("D39_H_ADD").ToString) Then
                Me.txtHoleExpansion.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleExpansion.ppText = .Rows(intRow)("D39_H_ADD").ToString.Trim                                          '増設

            If Not .Rows(intCnt)("D39_H_PRT_REMOVE").ToString.Equals(.Rows(intRow)("D39_H_PRT_REMOVE").ToString) Then
                Me.txtHoleSomeRemoval.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleSomeRemoval.ppText = .Rows(intRow)("D39_H_PRT_REMOVE").ToString.Trim                                 '一部撤去

            If Not .Rows(intCnt)("D39_H_RELOCATE").ToString.Equals(.Rows(intRow)("D39_H_RELOCATE").ToString) Then
                Me.txtHoleShopRelocation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleShopRelocation.ppText = .Rows(intRow)("D39_H_RELOCATE").ToString.Trim                                '店舗移設

            If Not .Rows(intCnt)("D39_H_ALL_REMOVE").ToString.Equals(.Rows(intRow)("D39_H_ALL_REMOVE").ToString) Then
                Me.txtHoleAllRemoval.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleAllRemoval.ppText = .Rows(intRow)("D39_H_ALL_REMOVE").ToString.Trim                                '全撤去

            If Not .Rows(intCnt)("D39_H_TMP_REMOVE").ToString.Equals(.Rows(intRow)("D39_H_TMP_REMOVE").ToString) Then
                Me.txtHoleOnceRemoval.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleOnceRemoval.ppText = .Rows(intRow)("D39_H_TMP_REMOVE").ToString.Trim                                '一時撤去

            If Not .Rows(intCnt)("D39_H_RESET").ToString.Equals(.Rows(intRow)("D39_H_RESET").ToString) Then
                Me.txtHoleReInstallation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleReInstallation.ppText = .Rows(intRow)("D39_H_RESET").ToString.Trim                                     '再設置

            If Not .Rows(intCnt)("D39_H_CHNG_ORGNZ").ToString.Equals(.Rows(intRow)("D39_H_CHNG_ORGNZ").ToString) Then
                Me.txtHoleConChange.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleConChange.ppText = .Rows(intRow)("D39_H_CHNG_ORGNZ").ToString.Trim                                    '構成変更

            If Not .Rows(intCnt)("D39_H_DLV_ORGNZ").ToString.Equals(.Rows(intRow)("D39_H_DLV_ORGNZ").ToString) Then
                Me.txtHoleConDelively.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleConDelively.ppText = .Rows(intRow)("D39_H_DLV_ORGNZ").ToString.Trim                                   '構成配信

            If Not .Rows(intCnt)("D39_H_OTH").ToString.Equals(.Rows(intRow)("D39_H_OTH").ToString) Then
                Me.txtHoleOther.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtHoleOther.ppText = .Rows(intRow)("D39_H_OTH").ToString.Trim                                               'その他

            If Not .Rows(intCnt)("D39_L_NEW").ToString.Equals(.Rows(intRow)("D39_L_NEW").ToString) Then
                Me.txtLanNew.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanNew.ppText = .Rows(intRow)("D39_L_NEW").ToString.Trim                                                 '新規

            If Not .Rows(intCnt)("D39_L_ADD").ToString.Equals(.Rows(intRow)("D39_L_ADD").ToString) Then
                Me.txtLanExpansion.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanExpansion.ppText = .Rows(intRow)("D39_L_ADD").ToString.Trim                                           '増設

            If Not .Rows(intCnt)("D39_L_PRT_REMOVE").ToString.Equals(.Rows(intRow)("D39_L_PRT_REMOVE").ToString) Then
                Me.txtLanSomeRemoval.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanSomeRemoval.ppText = .Rows(intRow)("D39_L_PRT_REMOVE").ToString                                   '一部撤去

            If Not .Rows(intCnt)("D39_L_RELOCATE").ToString.Equals(.Rows(intRow)("D39_L_RELOCATE").ToString) Then
                Me.txtLanShopRelocation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanShopRelocation.ppText = .Rows(intRow)("D39_L_RELOCATE").ToString.Trim                                  '店舗移設

            If Not .Rows(intCnt)("D39_L_ALL_REMOVE").ToString.Equals(.Rows(intRow)("D39_L_ALL_REMOVE").ToString) Then
                Me.txtLanAllRemoval.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanAllRemoval.ppText = .Rows(intRow)("D39_L_ALL_REMOVE").ToString.Trim                                    '全撤去

            If Not .Rows(intCnt)("D39_L_TMP_REMOVE").ToString.Equals(.Rows(intRow)("D39_L_TMP_REMOVE").ToString) Then
                Me.txtLanOnceRemoval.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanOnceRemoval.ppText = .Rows(intRow)("D39_L_TMP_REMOVE").ToString.Trim                                  '一時撤去

            If Not .Rows(intCnt)("D39_L_RESET").ToString.Equals(.Rows(intRow)("D39_L_RESET").ToString) Then
                Me.txtLanReInstallation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanReInstallation.ppText = .Rows(intRow)("D39_L_RESET").ToString.Trim                              '再設置

            If Not .Rows(intCnt)("D39_L_CHNG_ORGNZ").ToString.Equals(.Rows(intRow)("D39_L_CHNG_ORGNZ").ToString) Then
                Me.txtLanConChange.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanConChange.ppText = .Rows(intRow)("D39_L_CHNG_ORGNZ").ToString.Trim                                   '構成変更

            If Not .Rows(intCnt)("D39_L_DLV_ORGNZ").ToString.Equals(.Rows(intRow)("D39_L_DLV_ORGNZ").ToString) Then
                Me.txtLanConDelively.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanConDelively.ppText = .Rows(intRow)("D39_L_DLV_ORGNZ").ToString.Trim                                 '構成配信

            If Not .Rows(intCnt)("D39_L_OTH").ToString.Equals(.Rows(intRow)("D39_L_OTH").ToString) Then
                Me.txtLanOther.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtLanOther.ppText = .Rows(intRow)("D39_L_OTH").ToString.Trim                                                 'その他

            If Not .Rows(intCnt)("D39_SYSSTK_CLS").ToString.Equals(.Rows(intRow)("D39_SYSSTK_CLS").ToString) Then
                Me.txtStkHoleIn.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtStkHoleIn.ppText = .Rows(intRow)("D39_SYSSTK_CLS").ToString.Trim          'ストッカーホール内
            If Me.txtStkHoleIn.ppText Is DBNull.Value Then
                Me.lblStkHoleIn.Text = ""
            ElseIf Me.txtStkHoleIn.ppText = "" Then
                Me.lblStkHoleIn.Text = ""
            ElseIf Me.txtStkHoleIn.ppText = "0" Then
                Me.lblStkHoleIn.Text = ""
            ElseIf Me.txtStkHoleIn.ppText = "1" Then
                Me.lblStkHoleIn.Text = "該当"
            Else
                Me.lblStkHoleIn.Text = "不明"
            End If
            If Not .Rows(intCnt)("D39_SYSSTK_OUT").ToString.Equals(.Rows(intRow)("D39_SYSSTK_OUT").ToString) Then
                Me.txtStkHoleOut.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtStkHoleOut.ppText = .Rows(intRow)("D39_SYSSTK_OUT").ToString.Trim          'ストッカーホール外
            If Me.txtStkHoleOut.ppText Is DBNull.Value Then
                Me.lblStkHoleOut.Text = ""
            ElseIf Me.txtStkHoleOut.ppText = "" Then
                Me.lblStkHoleOut.Text = ""
            ElseIf Me.txtStkHoleOut.ppText = "0" Then
                Me.lblStkHoleOut.Text = ""
            ElseIf Me.txtStkHoleOut.ppText = "1" Then
                Me.lblStkHoleOut.Text = "該当"
            Else
                Me.lblStkHoleOut.Text = "不明"
            End If
            If .Rows(intRow)("D39_FSWRK_CLS").ToString.Trim = "" Then
                Me.txtSfOperation.ppText = "0"
                If intRow <> intCnt Then
                    .Rows(intRow)("D39_FSWRK_CLS") = "0"
                End If
                If Me.txtSfOperation.ppText Is DBNull.Value Then
                    Me.lblSfOperation.Text = "不明"
                ElseIf Me.txtSfOperation.ppText = "" Then
                    Me.lblSfOperation.Text = "不明"
                ElseIf Me.txtSfOperation.ppText = "0" Then
                    Me.lblSfOperation.Text = "有り"
                ElseIf Me.txtSfOperation.ppText = "1" Then
                    Me.lblSfOperation.Text = "無し"
                Else
                    Me.lblSfOperation.Text = "不明"
                End If
            Else
                Me.txtSfOperation.ppText = .Rows(intRow)("D39_FSWRK_CLS").ToString.Trim
                If Me.txtSfOperation.ppText Is DBNull.Value Then
                    Me.lblSfOperation.Text = "不明"
                ElseIf Me.txtSfOperation.ppText = "" Then
                    Me.lblSfOperation.Text = "不明"
                ElseIf Me.txtSfOperation.ppText = "0" Then
                    Me.lblSfOperation.Text = "有り"
                ElseIf Me.txtSfOperation.ppText = "1" Then
                    Me.lblSfOperation.Text = "無し"
                Else
                    Me.lblSfOperation.Text = "不明"
                End If
            End If
            If Not .Rows(intCnt)("D39_FSWRK_CLS").ToString.Equals(.Rows(intRow)("D39_FSWRK_CLS").ToString) Then
                Me.txtSfOperation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            'Me.txtSfOperation.ppText = .Rows(intRow)("D39_FSWRK_CLS").ToString.Trim                                         'エフエス稼動有無

            If Not .Rows(intCnt)("D39_EMNY_CLS").ToString.Equals(.Rows(intRow)("D39_EMNY_CLS").ToString) Then
                Me.txtEMoneyIntroduction.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtEMoneyIntroduction.ppText = .Rows(intRow)("D39_EMNY_CLS").ToString.Trim                                  'Ｅマネー導入


            If Not .Rows(intCnt)("D39_BACK_CLS").ToString.Equals(.Rows(intRow)("D39_BACK_CLS").ToString) Then
                Me.txtTboxTakeawayDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtTboxTakeawayDivision.ppText = .Rows(intRow)("D39_BACK_CLS").ToString.Trim                                'ＴＢＯＸ持帰区分
            If Me.txtTboxTakeawayDivision.ppText Is DBNull.Value Then
                Me.lblTboxTakeawayDivision.Text = ""
            ElseIf Me.txtTboxTakeawayDivision.ppText = "" Then
                Me.lblTboxTakeawayDivision.Text = ""
            ElseIf Me.txtTboxTakeawayDivision.ppText = "0" Then
                Me.lblTboxTakeawayDivision.Text = "無し"
            ElseIf Me.txtTboxTakeawayDivision.ppText = "1" Then
                Me.lblTboxTakeawayDivision.Text = "有り"
            Else
                Me.lblTboxTakeawayDivision.Text = "不明"
            End If

            Me.txtTwinsShop.ppTextBox.AutoPostBack = True '双子店区分
            Me.txtIndependentCns.ppTextBox.AutoPostBack = True '単独工事区分
            Me.txtPAndCDivision.ppTextBox.AutoPostBack = True '親子区分
            Me.dttVerupDtDivision.ppTextBox.AutoPostBack = True 'ＶＥＲＵＰ日付区分

            If Not .Rows(intCnt)("D39_EMNY_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_EMNY_CNST_CLS").ToString) Then
                Me.txtEMoneyIntroductionCns.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtEMoneyIntroductionCns.ppText = .Rows(intRow)("D39_EMNY_CNST_CLS").ToString.Trim                           'Ｅマネー導入工事

            If Not .Rows(intCnt)("D39_EMNY_DT").ToString.Equals(.Rows(intRow)("D39_EMNY_DT").ToString) Then
                Me.dttEMoneyTestDt.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttEMoneyTestDt.ppText = .Rows(intRow)("D39_EMNY_DT").ToString.Trim                                      'Ｅマネーテスト日付/Ｅマネーテスト時間

            If Not .Rows(intCnt)("D39_EMNY_TM").ToString.Equals(.Rows(intRow)("D39_EMNY_TM").ToString) Then
                Me.tmtEMoneyTestTm.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtEMoneyTestTm.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_EMNY_TM").ToString.Length >= 4 Then
                Me.tmtEMoneyTestTm.ppHourText = .Rows(intRow)("D39_EMNY_TM").ToString.Substring(0, 2)                  'Ｅマネーテスト日付/Ｅマネーテスト時間
                Me.tmtEMoneyTestTm.ppMinText = .Rows(intRow)("D39_EMNY_TM").ToString.Substring(2, 2)
            Else
                Me.tmtEMoneyTestTm.ppHourText = String.Empty
                Me.tmtEMoneyTestTm.ppMinText = String.Empty
            End If
            If Not .Rows(intCnt)("D39_OTH_DTL").ToString.Equals(.Rows(intRow)("D39_OTH_DTL").ToString) Then
                Me.txtOtherContents.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtOtherContents.ppText = .Rows(intRow)("D39_OTH_DTL").ToString.Trim                                        'その他内容

            If Not .Rows(intCnt)("D39_TWIN_CD").ToString.Equals(.Rows(intRow)("D39_TWIN_CD").ToString) Then
                Me.txtTwinsShop.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtTwinsShop.ppText = .Rows(intRow)("D39_TWIN_CD").ToString.Trim                                              '双子店

            If Not .Rows(intCnt)("D39_IND_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_IND_CNST_CLS").ToString) Then
                Me.txtIndependentCns.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtIndependentCns.ppText = .Rows(intRow)("D39_IND_CNST_CLS").ToString.Trim                                     '単独工事

            If Not .Rows(intCnt)("D39_SAME_CNST_CNT").ToString.Equals(.Rows(intRow)("D39_SAME_CNST_CNT").ToString) Then
                Me.txtSameTimeCnsNo.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSameTimeCnsNo.ppText = .Rows(intRow)("D39_SAME_CNST_CNT").ToString.Trim                                   '同時工事数

            If Not .Rows(intCnt)("D39_PC_FLG").ToString.Equals(.Rows(intRow)("D39_PC_FLG").ToString) Then
                Me.txtPAndCDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtPAndCDivision.ppText = .Rows(intRow)("D39_PC_FLG").ToString.Trim                                          '親子区分

            If Not .Rows(intCnt)("D39_PR_HALL_CD").ToString.Equals(.Rows(intRow)("D39_PR_HALL_CD").ToString) Then
                Me.txtParentHoleCd.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtParentHoleCd.ppText = .Rows(intRow)("D39_PR_HALL_CD").ToString.Trim                                        '親ホールコード

            If Not .Rows(intCnt)("D39_F1_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_F1_CNST_CLS").ToString) Then
                Me.txtConstructionExistenceF1.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtConstructionExistenceF1.ppText = .Rows(intRow)("D39_F1_CNST_CLS").ToString.Trim                             '工事有無Ｆ１

            If Not .Rows(intCnt)("D39_F2_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_F2_CNST_CLS").ToString) Then
                Me.txtConstructionExistenceF2.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtConstructionExistenceF2.ppText = .Rows(intRow)("D39_F2_CNST_CLS").ToString.Trim                             '工事有無Ｆ２

            If Not .Rows(intCnt)("D39_F3_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_F3_CNST_CLS").ToString) Then
                Me.txtConstructionExistenceF3.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtConstructionExistenceF3.ppText = .Rows(intRow)("D39_F3_CNST_CLS").ToString.Trim                            '工事有無Ｆ３

            If Not .Rows(intCnt)("D39_F4_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_F4_CNST_CLS").ToString) Then
                Me.txtConstructionExistenceF4.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtConstructionExistenceF4.ppText = .Rows(intRow)("D39_F4_CNST_CLS").ToString.Trim                          '工事有無Ｆ４

            If Not .Rows(intCnt)("D39_CNST_DY").ToString.Equals(.Rows(intRow)("D39_CNST_DY").ToString) Then
                Me.dttStartOfCon.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttStartOfCon.ppText = .Rows(intRow)("D39_CNST_DY").ToString.Trim

            If Not .Rows(intCnt)("D39_CNST_TM").ToString.Equals(.Rows(intRow)("D39_CNST_TM").ToString) Then
                Me.tmtStartOfCon.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtStartOfCon.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_CNST_TM").ToString.Length >= 4 Then
                Me.tmtStartOfCon.ppHourText = .Rows(intRow)("D39_CNST_TM").ToString.ToString.Substring(0, 2)
                Me.tmtStartOfCon.ppMinText = .Rows(intRow)("D39_CNST_TM").ToString.ToString.Substring(2, 2)
            Else
                Me.tmtStartOfCon.ppHourText = String.Empty
                Me.tmtStartOfCon.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_LAST_DT").ToString.Equals(.Rows(intRow)("D39_LAST_DT").ToString) Then
                Me.dttLastOpenDt.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttLastOpenDt.ppText = .Rows(intRow)("D39_LAST_DT").ToString.Trim                                                '最終営業日

            If Not .Rows(intCnt)("D39_T500_LAST_DT").ToString.Equals(.Rows(intRow)("D39_T500_LAST_DT").ToString) Then
                Me.dttLastOpenDtT500.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttLastOpenDtT500.ppText = .Rows(intRow)("D39_T500_LAST_DT").ToString.Trim                                 '最終営業日Ｔ５００

            If Not .Rows(intCnt)("D39_STEST_DT").ToString.Equals(.Rows(intRow)("D39_STEST_DT").ToString) Then
                Me.dttTest.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttTest.ppText = .Rows(intRow)("D39_STEST_DT").ToString.Trim                                                  '総合テスト

            If Not .Rows(intCnt)("D39_STEST_TM").ToString.Equals(.Rows(intRow)("D39_STEST_TM").ToString) Then
                Me.tmtTest.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtTest.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_STEST_TM").ToString.Length >= 4 Then
                Me.tmtTest.ppHourText = .Rows(intRow)("D39_STEST_TM").ToString.Substring(0, 2)                            '総合テスト
                Me.tmtTest.ppMinText = .Rows(intRow)("D39_STEST_TM").ToString.Substring(2, 2)
            Else
                Me.tmtTest.ppHourText = String.Empty
                Me.tmtTest.ppMinText = String.Empty
            End If

            'If Not .Rows(intCnt)("D39_STEST_CHARGE").ToString.Equals(.Rows(intRow)("D39_STEST_CHARGE").ToString) Then
            '    Me.txtTestWorkPersonnel.ppTextBox.ForeColor = Drawing.Color.Red
            'End If
            'Me.txtTestWorkPersonnel.ppText = .Rows(intRow)("D39_STEST_CHARGE").ToString.Trim                                   '作業担当者

            If Not .Rows(intCnt)("D39_STESTDEPT_DT").ToString.Equals(.Rows(intRow)("D39_STESTDEPT_DT").ToString) Then
                Me.dttTestDepartureDt.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttTestDepartureDt.ppText = .Rows(intRow)("D39_STESTDEPT_DT").ToString.Trim                                    '出発日時

            If Not .Rows(intCnt)("D39_STESTDEPT_TM").ToString.Equals(.Rows(intRow)("D39_STESTDEPT_TM").ToString) Then
                Me.tmtTestDepartureTm.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtTestDepartureTm.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_STESTDEPT_TM").ToString.Length >= 4 Then
                Me.tmtTestDepartureTm.ppHourText = .Rows(intRow)("D39_STESTDEPT_TM").ToString.Substring(0, 2)
                Me.tmtTestDepartureTm.ppMinText = .Rows(intRow)("D39_STESTDEPT_TM").ToString.Substring(2, 2)
            Else
                Me.tmtTestDepartureTm.ppHourText = String.Empty
                Me.tmtTestDepartureTm.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_TMPSET_DT").ToString.Equals(.Rows(intRow)("D39_TMPSET_DT").ToString) Then
                Me.dttTemporaryInstallation.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttTemporaryInstallation.ppText = .Rows(intRow)("D39_TMPSET_DT").ToString.Trim                                  '仮設置

            If Not .Rows(intCnt)("D39_TMPSET_TM").ToString.Equals(.Rows(intRow)("D39_TMPSET_TM").ToString) Then
                Me.tmtTemporaryInstallation.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtTemporaryInstallation.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_TMPSET_TM").ToString.Length >= 4 Then
                Me.tmtTemporaryInstallation.ppHourText = .Rows(intRow)("D39_TMPSET_TM").ToString.Substring(0, 2)
                Me.tmtTemporaryInstallation.ppMinText = .Rows(intRow)("D39_TMPSET_TM").ToString.Substring(2, 2)
            Else
                Me.tmtTemporaryInstallation.ppHourText = String.Empty
                Me.tmtTemporaryInstallation.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_TMPSET_CHARGE").ToString.Equals(.Rows(intRow)("D39_TMPSET_CHARGE").ToString) Then
                Me.txtTemporaryInstallationWorkPersonnel.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtTemporaryInstallationWorkPersonnel.ppText = .Rows(intRow)("D39_TMPSET_CHARGE").ToString.Trim                 '作業担当者

            If Not .Rows(intCnt)("D39_TMPSETDEPT_DT").ToString.Equals(.Rows(intRow)("D39_TMPSETDEPT_DT").ToString) Then
                Me.dttTemporaryInstallationDepartureDt.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttTemporaryInstallationDepartureDt.ppText = .Rows(intRow)("D39_TMPSETDEPT_DT").ToString.Trim                  '出発日時

            If Not .Rows(intCnt)("D39_TMPSETDEPT_TM").ToString.Equals(.Rows(intRow)("D39_TMPSETDEPT_TM").ToString) Then
                Me.tmtTemporaryInstallationDepartureTm.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtTemporaryInstallationDepartureTm.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_TMPSETDEPT_TM").ToString.Length >= 4 Then
                Me.tmtTemporaryInstallationDepartureTm.ppHourText = .Rows(intRow)("D39_TMPSETDEPT_TM").ToString.Substring(0, 2)
                Me.tmtTemporaryInstallationDepartureTm.ppMinText = .Rows(intRow)("D39_TMPSETDEPT_TM").ToString.Substring(2, 2)
            Else
                Me.tmtTemporaryInstallationDepartureTm.ppHourText = String.Empty
                Me.tmtTemporaryInstallationDepartureTm.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_TMPSET_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_TMPSET_CNST_CLS").ToString) Then
                Me.dttTemporaryInstallationCnsDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttTemporaryInstallationCnsDivision.ppText = .Rows(intRow)("D39_TMPSET_CNST_CLS").ToString.Trim               '仮設置工事区分

            If Not .Rows(intCnt)("D39_TMPSET_UNINP_CLS").ToString.Equals(.Rows(intRow)("D39_TMPSET_UNINP_CLS").ToString) Then
                Me.dttTemporaryInstallationDtNotInputDivision0.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttTemporaryInstallationDtNotInputDivision0.ppText = .Rows(intRow)("D39_TMPSET_UNINP_CLS").ToString.Trim        '仮設置日未入力区分

            If Not .Rows(intCnt)("D39_PTEST_DT").ToString.Equals(.Rows(intRow)("D39_PTEST_DT").ToString) Then
                Me.dttPolice.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttPolice.ppText = .Rows(intRow)("D39_PTEST_DT").ToString.Trim                                                '警察

            If Not .Rows(intCnt)("D39_PTEST_TM").ToString.Equals(.Rows(intRow)("D39_PTEST_TM").ToString) Then
                Me.tmtPolice.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtPolice.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_PTEST_TM").ToString.Length >= 4 Then
                Me.tmtPolice.ppHourText = .Rows(intRow)("D39_PTEST_TM").ToString.Substring(0, 2)                         '警察
                Me.tmtPolice.ppMinText = .Rows(intRow)("D39_PTEST_TM").ToString.Substring(2, 2)
            Else
                Me.tmtPolice.ppHourText = String.Empty
                Me.tmtPolice.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_MOVE_CNST_CLS").ToString.Equals(.Rows(intRow)("D39_MOVE_CNST_CLS").ToString) Then
                Me.dttShiftCnsDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttShiftCnsDivision.ppText = .Rows(intRow)("D39_MOVE_CNST_CLS").ToString.Trim                                 '移行工事区分

            If Not .Rows(intCnt)("D39_MOVE_CNST_WORK_CLS").ToString.Equals(.Rows(intRow)("D39_MOVE_CNST_WORK_CLS").ToString) Then
                Me.dttShiftCnsWorkDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttShiftCnsWorkDivision.ppText = .Rows(intRow)("D39_MOVE_CNST_WORK_CLS").ToString.Trim                          '移行工事作業区分

            If Not .Rows(intCnt)("D39_OPEN_DT").ToString.Equals(.Rows(intRow)("D39_OPEN_DT").ToString) Then
                Me.dttOpen.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttOpen.ppText = .Rows(intRow)("D39_OPEN_DT").ToString.Trim                                                    'オープン

            If Not .Rows(intCnt)("D39_OPEN_TM").ToString.Equals(.Rows(intRow)("D39_OPEN_TM").ToString) Then
                Me.tmtOpen.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtOpen.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_OPEN_TM").ToString.Length >= 4 Then
                Me.tmtOpen.ppHourText = .Rows(intRow)("D39_OPEN_TM").ToString.Substring(0, 2)
                Me.tmtOpen.ppMinText = .Rows(intRow)("D39_OPEN_TM").ToString.Substring(2, 2)
            Else
                Me.tmtOpen.ppHourText = String.Empty
                Me.tmtOpen.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_LAN_CNST_DT").ToString.Equals(.Rows(intRow)("D39_LAN_CNST_DT").ToString) Then
                Me.dttLanCns.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttLanCns.ppText = .Rows(intRow)("D39_LAN_CNST_DT").ToString.Trim                                        'ＬＡＮ工事

            If Not .Rows(intCnt)("D39_LAN_CNST_TM").ToString.Equals(.Rows(intRow)("D39_LAN_CNST_TM").ToString) Then
                Me.tmtLanCns.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtLanCns.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_LAN_CNST_TM").ToString.Length >= 4 Then
                Me.tmtLanCns.ppHourText = .Rows(intRow)("D39_LAN_CNST_TM").ToString.Substring(0, 2)                                                                     'ＬＡＮ工事
                Me.tmtLanCns.ppMinText = .Rows(intRow)("D39_LAN_CNST_TM").ToString.Substring(2, 2)
            Else
                Me.tmtLanCns.ppHourText = String.Empty
                Me.tmtLanCns.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_INSERT_USR").ToString.Equals(.Rows(intCnt)("D39_INSERT_USR").ToString) Then
                Me.lblRegisteredEmployeesNm_2.ForeColor = Drawing.Color.Red
            End If
            'CNSUPDP001_005
            Me.lblRegisteredEmployeesNm_2.Text = dstOrders_1.Tables(1).Rows(intRow)("登録者").ToString.Trim                               '登録社員名
            'CNSUPDP001_005 END

            If Not .Rows(intCnt)("D39_VERUP_CD").ToString.Equals(.Rows(intRow)("D39_VERUP_CD").ToString) Then
                Me.dttVerup.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttVerup.ppText = .Rows(intRow)("D39_VERUP_CD").ToString.Trim                                                  'ＶＥＲＵＰ


            If Not .Rows(intCnt)("D39_VERUP_DT").ToString.Equals(.Rows(intRow)("D39_VERUP_DT").ToString) Then
                Me.dttVerup.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttVerup.ppText = .Rows(intRow)("D39_VERUP_DT").ToString.Trim                                              'ＶＥＲＵＰ日付


            If Not .Rows(intCnt)("D39_VERUP_TM").ToString.Equals(.Rows(intRow)("D39_VERUP_TM").ToString) Then
                Me.tmtVerup.ppHourBox.ForeColor = Drawing.Color.Red
                Me.tmtVerup.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_VERUP_TM").ToString.Length >= 4 Then
                Me.tmtVerup.ppHourText = .Rows(intRow)("D39_VERUP_TM").ToString.Substring(0, 2)                          'ＶＥＲＵＰ日付
                Me.tmtVerup.ppMinText = .Rows(intRow)("D39_VERUP_TM").ToString.Substring(2, 2)
            Else
                Me.tmtVerup.ppHourText = String.Empty
                Me.tmtVerup.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_VERUP_DT_CLS").ToString.Equals(.Rows(intRow)("D39_VERUP_DT_CLS").ToString) Then
                Me.dttVerupDtDivision.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.dttVerupDtDivision.ppText = .Rows(intRow)("D39_VERUP_DT_CLS").ToString                                    'ＶＥＲＵＰ日付区分

            If Not .Rows(intCnt)("D39_KIND_VERUP1").ToString.Equals(.Rows(intRow)("D39_KIND_VERUP1").ToString) Then
                Me.txtVerupCnsType_1.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtVerupCnsType_1.ppText = .Rows(intRow)("D39_KIND_VERUP1").ToString.Trim                                  'バージョンアップ種類１

            If Not .Rows(intCnt)("D39_KIND_VERUP2").ToString.Equals(.Rows(intRow)("D39_KIND_VERUP2").ToString) Then
                Me.txtVerupCnsType_2.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtVerupCnsType_2.ppText = .Rows(intRow)("D39_KIND_VERUP2").ToString.Trim                                      'バージョンアップ種類２

            If Not .Rows(intCnt)("D39_AGENCY_CD").ToString.Equals(.Rows(intRow)("D39_AGENCY_CD").ToString) Then
                Me.txtAgencyCd.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyCd.ppText = .Rows(intRow)("D39_AGENCY_CD").ToString.Trim                                             '代理店

            If Not .Rows(intCnt)("D39_AGENCY_NM").ToString.Equals(.Rows(intRow)("D39_AGENCY_NM").ToString) Then
                Me.trfAgencyNm.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.trfAgencyNm.ppText = .Rows(intRow)("D39_AGENCY_NM").ToString.Trim                                           '代理店名

            If Not .Rows(intCnt)("D39_A_TELNO").ToString.Equals(.Rows(intRow)("D39_A_TELNO").ToString) Then
                Me.txtAgencyTel.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyTel.ppText = .Rows(intRow)("D39_A_TELNO").ToString.Trim                                              'ＴＥＬ

            If Not .Rows(intCnt)("D39_A_CHARGE").ToString.Equals(.Rows(intRow)("D39_A_CHARGE").ToString) Then
                Me.txtAgencyPersonnel.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyPersonnel.ppText = .Rows(intRow)("D39_A_CHARGE").ToString.Trim                                        '担当者

            If Not .Rows(intCnt)("D39_A_ADDR").ToString.Equals(.Rows(intRow)("D39_A_ADDR").ToString) Then
                Me.txtAgencyAdd.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyAdd.ppText = .Rows(intRow)("D39_A_ADDR").ToString.Trim                                          '住所

            If Not .Rows(intCnt)("D39_A_TCHARGE").ToString.Equals(.Rows(intRow)("D39_A_TCHARGE").ToString) Then
                Me.txtAgencyResponsible.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyResponsible.ppText = .Rows(intRow)("D39_A_TCHARGE").ToString.Trim                                    '責任者

            If Not .Rows(intCnt)("D39_REPSHOP_CD").ToString.Equals(.Rows(intRow)("D39_REPSHOP_CD").ToString) Then
                Me.txtAgencyShop.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyShop.ppText = .Rows(intRow)("D39_REPSHOP_CD").ToString.Trim                                          '代行店

            If Not .Rows(intCnt)("D39_REPSHOP_NM").ToString.Equals(.Rows(intRow)("D39_REPSHOP_NM").ToString) Then
                Me.trfAgencyShop.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.trfAgencyShop.ppText = .Rows(intRow)("D39_REPSHOP_NM").ToString.Trim                                             '代行店名

            If Not .Rows(intCnt)("D39_R_TELNO").ToString.Equals(.Rows(intRow)("D39_R_TELNO").ToString) Then
                Me.txtAgencyShopTelNo.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyShopTelNo.ppText = .Rows(intRow)("D39_R_TELNO").ToString.Trim                                            'ＴＥＬ

            If Not .Rows(intCnt)("D39_R_CHARGE").ToString.Equals(.Rows(intRow)("D39_R_CHARGE").ToString) Then
                Me.txtAgencyShopPersonnel.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyShopPersonnel.ppText = .Rows(intRow)("D39_R_CHARGE").ToString.Trim                                         '担当者

            If Not .Rows(intCnt)("D39_R_ADDR").ToString.Equals(.Rows(intRow)("D39_R_ADDR").ToString) Then
                Me.txtAgencyShopAdd.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyShopAdd.ppText = .Rows(intRow)("D39_R_ADDR").ToString.Trim                                              '住所

            If Not .Rows(intCnt)("D39_R_TCHARGE").ToString.Equals(.Rows(intRow)("D39_R_TCHARGE").ToString) Then
                Me.txtAgencyShopResponsible.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtAgencyShopResponsible.ppText = .Rows(intRow)("D39_R_TCHARGE").ToString.Trim                                    '責任者

            If Not .Rows(intCnt)("D39_LAN_SEND_CD").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_CD").ToString) Then
                Me.txtSendingStation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSendingStation.ppText = .Rows(intRow)("D39_LAN_SEND_CD").ToString.Trim                                         'ＬＡＮ送付先コード

            If Not .Rows(intCnt)("D39_LAN_SEND_NM").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_NM").ToString) Then
                Me.trfSendingStation.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.trfSendingStation.ppText = .Rows(intRow)("D39_LAN_SEND_NM").ToString.Trim                                        'ＬＡＮ送付先

            If Not .Rows(intCnt)("D39_LAN_SEND_TCHARGE").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_TCHARGE").ToString) Then
                Me.txtSendingStationResponsible.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSendingStationResponsible.ppText = .Rows(intRow)("D39_LAN_SEND_TCHARGE").ToString.Trim                         'ＬＡＮ送付先責任者

            If Not .Rows(intCnt)("D39_LAN_SEND_DT").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_DT").ToString) Then
                Me.tdtDeliveryPreferredDt.ppDateBox.ForeColor = Drawing.Color.Red
            End If
            Me.tdtDeliveryPreferredDt.ppText = .Rows(intRow)("D39_LAN_SEND_DT").ToString.Trim                                   '納入希望日

            If Not .Rows(intCnt)("D39_LAN_SEND_TM").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_TM").ToString) Then
                Me.ClsCMTimeBox1.ppHourBox.ForeColor = Drawing.Color.Red
                Me.ClsCMTimeBox1.ppMinBox.ForeColor = Drawing.Color.Red
            End If
            If .Rows(intRow)("D39_LAN_SEND_TM").ToString.Length >= 4 Then
                Me.ClsCMTimeBox1.ppHourText = .Rows(intRow)("D39_LAN_SEND_TM").ToString.Substring(0, 2)                       '納入希望日
                Me.ClsCMTimeBox1.ppMinText = .Rows(intRow)("D39_LAN_SEND_TM").ToString.Substring(2, 2)
            Else
                Me.ClsCMTimeBox1.ppHourText = String.Empty
                Me.ClsCMTimeBox1.ppMinText = String.Empty
            End If

            If Not .Rows(intCnt)("D39_LAN_SEND_ADDR").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_ADDR").ToString) Then
                Me.txtSendingStationAdd.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSendingStationAdd.ppText = .Rows(intRow)("D39_LAN_SEND_ADDR").ToString.Trim                                        '住所

            If Not .Rows(intCnt)("D39_LAN_SEND_TELNO").ToString.Equals(.Rows(intRow)("D39_LAN_SEND_TELNO").ToString) Then
                Me.txtSendingStationTelNo.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSendingStationTelNo.ppText = .Rows(intRow)("D39_LAN_SEND_TELNO").ToString.Trim                                  'ＴＥＬ

            If Not .Rows(intCnt)("D39_NOTETEXT").ToString.Equals(.Rows(intRow)("D39_NOTETEXT").ToString) Then
                Me.txtRemarks.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtRemarks.ppText = .Rows(intRow)("D39_NOTETEXT").ToString.Trim                                                      '備考

            If Not .Rows(intCnt)("D39_TELL_MAT").ToString.Equals(.Rows(intRow)("D39_TELL_MAT").ToString) Then
                Me.txtSpecificationsRemarks.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtSpecificationsRemarks.ppText = .Rows(intRow)("D39_TELL_MAT").ToString.Trim                                      '仕様備考

            If Not .Rows(intCnt)("D39_POINT1").ToString.Equals(.Rows(intRow)("D39_POINT1").ToString) Then
                Me.txtImportantPoints1.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtImportantPoints1.ppText = .Rows(intRow)("D39_POINT1").ToString.Trim                                             '注意事項

            If Not .Rows(intCnt)("D39_POINT2").ToString.Equals(.Rows(intRow)("D39_POINT2").ToString) Then
                Me.txtImportantPoints2.ppTextBox.ForeColor = Drawing.Color.Red
            End If
            Me.txtImportantPoints2.ppText = .Rows(intRow)("D39_POINT2").ToString.Trim                                             'ＤＬＬ設定変更なし

            If Not .Rows(intCnt)("D39_DLL_CLS").ToString.Equals(.Rows(intRow)("D39_DLL_CLS").ToString) Then
                Me.cbxDllSettingChange.ForeColor = Drawing.Color.Red
            End If

            If dstorders_5.Tables.Count <> 0 Then
                If dstorders_5.Tables(0).Rows.Count = 0 Then
                    If .Rows(intRow)("D39_DLL_CLS").ToString.Equals(String.Empty) Or .Rows(intRow)("D39_DLL_CLS").ToString = "0" Then
                        Me.cbxDllSettingChange.Checked = False
                    Else
                        Me.cbxDllSettingChange.Checked = True
                    End If
                Else
                    If dstorders_5.Tables(0).Rows(dstorders_5.Tables(0).Rows.Count - 1)("D39_DLL_CLS").ToString.Equals(String.Empty) Or dstorders_5.Tables(0).Rows(dstorders_5.Tables(0).Rows.Count - 1)("D39_DLL_CLS").ToString = 0 Then
                        Me.cbxDllSettingChange.Checked = False
                    Else
                        Me.cbxDllSettingChange.Checked = True
                    End If
                End If
            Else
                If .Rows(intRow)("D39_DLL_CLS").ToString.Equals(String.Empty) Or .Rows(intRow)("D39_DLL_CLS").ToString = "0" Then
                    Me.cbxDllSettingChange.Checked = False
                Else
                    Me.cbxDllSettingChange.Checked = True
                End If
            End If

            If dstorders_5.Tables.Count <> 0 Then
                If dstorders_5.Tables(0).Rows.Count = 0 Then
                    If Not .Rows(intCnt)("D39_BRANCH_CD").ToString.Equals(.Rows(intRow)("D39_BRANCH_CD").ToString) Then
                        Me.txtOfficCD.ppTextBox.ForeColor = Drawing.Color.Red
                    End If
                    Me.txtOfficCD.ppText = .Rows(intRow)("D39_BRANCH_CD").ToString.Trim
                Else
                    Me.txtOfficCD.ppText = dstorders_5.Tables(0).Rows(dstorders_5.Tables(0).Rows.Count - 1)("D39_BRANCH_CD").ToString.Trim

                End If
            Else
                If Not .Rows(intCnt)("D39_BRANCH_CD").ToString.Equals(.Rows(intRow)("D39_BRANCH_CD").ToString) Then
                    Me.txtOfficCD.ppTextBox.ForeColor = Drawing.Color.Red
                End If
                Me.txtOfficCD.ppText = .Rows(intRow)("D39_BRANCH_CD").ToString.Trim
            End If

            If Not .Rows(intCnt)("D39_BRANCH_NM").ToString.Equals(.Rows(intRow)("D39_BRANCH_NM").ToString) Then
                Me.lblSendNmV.ForeColor = Drawing.Color.Red
            End If

            If .Rows(intRow)("D39_BRANCH_NM").ToString.Trim = String.Empty Then
                '業者情報を取得
                msSetTrader()

                '作業担当者ドロップダウン設定
                msSetddlEmployee(Me.txtOfficCD.ppText)
            Else
                Me.lblSendNmV.Text = .Rows(intRow)("D39_BRANCH_NM").ToString.Trim
            End If

            '本設置、仮設置進捗ステータス取得
            ViewState(M_TMPSTTS_CD) = .Rows(intRow)("D39_TMPSTTS_CD").ToString.Trim
            ViewState(M_STATUS_CD) = .Rows(intRow)("D39_STATUS_CD").ToString.Trim

            '本設置、仮設置の判断を行う(0:なし 1:仮設置 2:本設置 9:両方)
            If Not .Rows(intRow)("D39_STEST_DT").ToString = String.Empty Then '総合テスト日が空以外
                If Not .Rows(intRow)("D39_TMPSET_DT").ToString = String.Empty Then '仮設置が空以外
                    If .Rows(intRow)("D39_STEST_DT").ToString + .Rows(intRow)("D39_STEST_TM").ToString _
                        <> .Rows(intRow)("D39_TMPSET_DT").ToString + .Rows(intRow)("D39_TMPSET_TM").ToString Then '総合テスト日と仮設置日が異なっていたら
                        ViewState(M_STE_TMP_DT) = "9"   '本設置/仮設置
                    Else
                        ViewState(M_STE_TMP_DT) = "3"   '仮本設置一致
                    End If
                Else
                    ViewState(M_STE_TMP_DT) = "2"   '本設置
                End If
            ElseIf Not .Rows(intRow)("D39_TMPSET_DT").ToString = String.Empty Then
                ViewState(M_STE_TMP_DT) = "1"   '仮設置
            Else
                ViewState(M_STE_TMP_DT) = "0"   'なし
            End If
            If dstOrders_3.Tables.Count > 0 Then
                If dstOrders_3.Tables(0).Rows.Count > 0 Then

                    Me.trfSfPersonInCharge.ppText = dstOrders_3.Tables(0).Rows(0)("D03_NTTD_CHARGE").ToString.Trim                     'エフエス担当者
                    If dstOrders_3.Tables(0).Rows(0)("D03_PROC_RSLT").ToString = "ＯＫ" Then
                        Me.RadioButtonList1.SelectedIndex = 0
                    Else
                        Me.RadioButtonList1.SelectedIndex = 1
                    End If

                    Dim spl As String()
                    Dim brk As String() = {"##"}
                    spl = dstOrders_3.Tables(0).Rows(0)("D03_PROC_RSLT_DTL").Split(brk, System.StringSplitOptions.None)
                    For i As Integer = 0 To spl.Count - 1
                        Select Case i
                            Case 0
                                Me.txtProcessingResultDetail1.ppText = spl(i).ToString
                            Case 1
                                Me.txtProcessingResultDetail2.ppText = spl(i).ToString
                            Case 2
                                Me.txtProcessingResultDetail3.ppText = spl(i).ToString
                        End Select
                    Next
                    spl = dstOrders_3.Tables(0).Rows(0)("D03_NOTETEXT").ToString.Split(brk, System.StringSplitOptions.None)
                    For i = 0 To spl.Count - 1
                        Select Case i
                            Case 0
                                Me.txtControlInfoRemarks1.ppText = spl(i).ToString
                            Case 1
                                Me.txtControlInfoRemarks2.ppText = spl(i).ToString
                            Case 2
                                Me.txtControlInfoRemarks3.ppText = spl(i).ToString
                        End Select
                    Next
                End If
                'CNSUPDP001_005
            Else
                'ＤＢ接続
                Dim dstUser As New DataSet
                Dim conDB As New SqlConnection
                Dim cmdDB As New SqlCommand
                If clsDataConnect.pfOpen_Database(conDB) Then
                    'トランザクション
                    cmdDB.Connection = conDB
                Else
                    Throw New Exception("")
                End If

                '社員名取得
                cmdDB = New SqlCommand("ZCMPSEL056", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))       '仮依頼番号
                End With

                dstUser = clsDataConnect.pfGet_DataSet(cmdDB)

                Me.trfSfPersonInCharge.ppText = dstUser.Tables(0).Rows(0).Item("社員名").ToString

                'オブジェクトの破棄を保証する
                cmdDB.Dispose()

                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
                'CNSUPDP001_005 END
            End If

            'ボタンの制御を行う
            Select Case ViewState(M_STE_TMP_DT)
                Case "0" 'なし
                    Master.ppRigthButton6.Enabled = False  '工事進捗
                    Master.ppLeftButton6.Enabled = False   '完了報告書
                Case "1" '仮設置
                    Select Case .Rows(intRow)("D39_MTR_STATUS_CD").ToString
                        Case "07", "08", "09", "10", "11", "12"
                            Master.ppRigthButton6.Enabled = True  '工事進捗
                            Master.ppLeftButton6.Enabled = True   '完了報告書
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case "06"
                            Master.ppRigthButton6.Enabled = True  '工事進捗
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case "04", "05"
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case Else
                            Master.ppRigthButton6.Enabled = False  '工事進捗
                            Master.ppLeftButton6.Enabled = False   '完了報告書
                            Master.ppLeftButton1.Enabled = False   '物品転送依頼
                    End Select
                Case "2", "3" '本設置,仮本設置一致
                    Select Case .Rows(intRow)("D39_MTR_STATUS_CD").ToString
                        Case "07", "08", "09", "10", "11", "12"
                            Master.ppRigthButton6.Enabled = True  '工事進捗
                            Master.ppLeftButton6.Enabled = True   '完了報告書
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case "06"
                            Master.ppRigthButton6.Enabled = True  '工事進捗
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case "04", "05"
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case Else
                            Master.ppRigthButton6.Enabled = False  '工事進捗
                            Master.ppLeftButton6.Enabled = False   '完了報告書
                            Master.ppLeftButton1.Enabled = False   '物品転送依頼
                    End Select
                Case "9" '両方
                    Select Case .Rows(intRow)("D39_MTR_STATUS_CD").ToString
                        Case "07", "08", "09", "10", "11", "12"
                            Master.ppRigthButton6.Enabled = True  '工事進捗
                            Master.ppLeftButton6.Enabled = True   '完了報告書
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case "06"
                            Master.ppRigthButton6.Enabled = True  '工事進捗
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case "04", "05"
                            Master.ppLeftButton1.Enabled = True   '物品転送依頼
                        Case Else
                            Master.ppRigthButton6.Enabled = False  '工事進捗
                            Master.ppLeftButton6.Enabled = False   '完了報告書
                            Master.ppLeftButton1.Enabled = False   '物品転送依頼
                    End Select
            End Select

        End With

        ViewState("D39_DATA_SV") = dstOrders_1

        Return True

    End Function

#End Region

#Region "一覧差分表示処理"

    ''' <summary>
    ''' 一覧差分表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_GRIDEdit(ByVal dstOrders_2 As DataSet _
                                    , ByVal dstOrders_4 As DataSet)
        Dim drData As DataRow() = Nothing
        Dim dtCnt As Integer = 0
        Dim rowCnt As Integer = 0
        Dim tes As String
        Dim tes2 As String
        Try

            If dstOrders_4.Tables.Count > 0 Then
                If dstOrders_4.Tables(0).Rows.Count Then
                    'TOMASデータ
                    For Each rowData As GridViewRow In grvList.Rows
                        '変更前の依頼と変更の依頼の相違を調べる
                        drData = dstOrders_4.Tables(0).Select("機器コード = '" & dstOrders_2.Tables(0).Rows(rowCnt).Item("機器コード") & "'")
                        '相違があった場合、文字を赤くする
                        If Not drData.Length = 0 Then
                            For clmCnt As Integer = 3 To dstOrders_2.Tables(0).Columns.Count - 2
                                Dim clmCnt2 As Integer = clmCnt + 1
                                tes = dstOrders_2.Tables(0).Rows(rowCnt).Item(clmCnt2).ToString
                                tes2 = drData(0).ItemArray(clmCnt2).ToString
                                'If Not dstOrders_2.Tables(0).Rows(rowCnt).Item(clmCnt).ToString.Equals(dstOrders_4.Tables(0).Rows(rowCnt).Item(clmCnt).ToString) Then
                                If Not dstOrders_2.Tables(0).Rows(rowCnt).Item(clmCnt2).ToString = drData(0).ItemArray(clmCnt2).ToString Then
                                    CType(rowData.FindControl(grvList.Columns(clmCnt).HeaderText), Label).ForeColor = Drawing.Color.Red
                                End If
                            Next
                            dtCnt += 1
                        Else
                            For clmCnt As Integer = 0 To dstOrders_4.Tables(0).Columns.Count - 2
                                'CType(rowData.FindControl(grvList.Columns(dstOrders_4.Tables(0).Rows.Count - 1).HeaderText), TextBox).ForeColor = Drawing.Color.Red
                                CType(rowData.FindControl(grvList.Columns(clmCnt).HeaderText), Label).ForeColor = Drawing.Color.Red
                            Next
                        End If

                        rowCnt += 1
                    Next
                    'If dstOrders_4.Tables.Count > 0 Then
                    '    '行数だけループする
                    '    For rowCnt As Integer = 0 To dstOrders_2.Tables(0).Rows.Count - 1
                    '        '変更前の依頼と変更の依頼の相違を調べる
                    '        drData = dstOrders_2.Tables(0).Select("機器コード = '" & dstOrders_4.Tables(0).Rows(rowCnt).Item("機器コード") & "'")
                    '        '相違があった場合、文字を赤くする
                    '        If Not drData.Length = 0 Then
                    '            For clmCnt As Integer = 0 To dstOrders_4.Tables(0).Columns.Count - 1
                    '                If Not dstOrders_2.Tables(0).Rows(rowCnt).Item(clmCnt).ToString.Equals(dstOrders_4.Tables(0).Rows(rowCnt).Item(clmCnt).ToString) Then
                    '                    grvList.Rows(rowCnt).Cells(clmCnt).ForeColor = Drawing.Color.Red
                    '                End If
                    '            Next
                    '        Else
                    '            For clmCnt2 As Integer = 0 To dstOrders_4.Tables(0).Columns.Count - 1
                    '                'CType(grvList.Rows(rowCnt).Cells(clmCnt2), TextBox) = Drawing.Color.Red
                    '            Next
                    '        End If
                    '        dtCnt += 1
                    '    Next
                    'End If
                    'Else
                    'SPCデータ
                    'If dstOrders_2.Tables.Count > 0 Then
                    '    With dstOrders_2.Tables(0)
                    '        For intCnt As Integer = 0 To .Rows.Count - 1
                    '            For intCnt2 As Integer = 0 To dstOrders_4.Tables(0).Rows.Count - 1
                    '                For intcle As Integer = 0 To .Columns.Count - 1
                    '                    If Not .Rows(intCnt).Item(intcle).ToString.Equals(dstOrders_4.Tables(0).Rows(intCnt).Item(intcle).ToString) Then
                    '                        grvList.Rows(intCnt).Cells(intcle).ForeColor = Drawing.Color.Red
                    '                    End If
                    '                Next
                    '            Next

                    '        Next
                    '    End With
                    'End If
                End If
            End If
        Catch ex As Exception

            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
        Return True

    End Function

#End Region

#Region "入力項目チェック処理"

    ''' <summary>
    ''' 入力項目チェック処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGamen_Check()

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
            'エフエス担当者
            If Me.trfSfPersonInCharge.ppText = String.Empty Then
                Me.trfSfPersonInCharge.psSet_ErrorNo("5001", trfSfPersonInCharge.ppName)
            End If

            '担当営業所コード
            If Me.txtSfOperation.ppText = "0" Then
                If Me.txtOfficCD.ppText = String.Empty Then
                    Me.txtOfficCD.psSet_ErrorNo("5001", txtOfficCD.ppName)
                Else
                    msOfficeCD_Check() '営業所コード整合性チェック　CNSUPDP001_012
                End If
            End If
            Exit Sub
        End If

        '拠点入力の場合
        If Session(P_SESSION_PLACE) = P_ADD_WKB Then
            '担当営業所コード
            If Me.txtOfficCD.ppText = String.Empty Then
                Me.txtOfficCD.psSet_ErrorNo("5001", txtOfficCD.ppName)
            End If

            Exit Sub
        End If

        '--------------------------------
        '2015/04/21 加賀　ここから
        '--------------------------------
        '営業所コード FS稼働有の場合チェック
        If Me.txtSfOperation.ppText <> "1" Then
            msOfficeCD_Check()
        End If

        '現行システム&VER 整合性チェック
        msSysVer_Check()

        'ＴＢＯＸＩＤ
        If Me.txtTboxId.ppText = String.Empty Then
            Me.txtTboxId.psSet_ErrorNo("5001", txtTboxId.ppName)
        End If
        '--------------------------------
        '2015/04/21 加賀　ここまで
        '--------------------------------

        'CNSUPDP001_004
        '仮ODでDLL変更を行う場合のみ判定
        If (lblRequestNo_2.Text.IndexOf("N0090") >= 0 Or lblRequestNo_2.Text = String.Empty) _
             And Me.cbxDllSettingChange.Checked = False Then

            If Me.txtCurrentSys.ppText = String.Empty Then
                Me.txtCurrentSys.psSet_ErrorNo("5001", txtCurrentSys.ppName)
            End If
            If Me.txtNLDivision.ppText = String.Empty Then
                Me.txtNLDivision.psSet_ErrorNo("5001", txtNLDivision.ppName)
            End If
            If Me.dttTest.ppDateBox.Text = String.Empty Then
                Me.dttTest.psSet_ErrorNo("5001", dttTest.ppName)
            End If
            If Me.tmtTest.ppHourText = String.Empty Then
                Me.dttTest.psSet_ErrorNo("5001", dttTest.ppName)
            End If
            If Me.tmtTest.ppMinText = String.Empty Then
                Me.dttTest.psSet_ErrorNo("5001", dttTest.ppName)
            End If
        End If
        'CNSUPDP001_004 END

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
            '--------------------------------
            '2015/04/18 加賀　ここから
            '--------------------------------
            'ＮＬ区分 N:NGC、L:LEC、J:JACKY
            msNL_Check()

            'ホール内工事
            msHoleCnst_Check()

            '仮設置・総合テスト日時
            msTestDT_Check() 'CNSUPDP001-001

            'ＴＢＯＸＩＤ
            If Me.txtTboxId.ppText.Length <> 8 Then
                Me.txtTboxId.psSet_ErrorNo("3001", txtTboxId.ppName, "8")
            Else
                If Not Me.txtTboxId.ppText = String.Empty Then
                    If pfCheck_Num(txtTboxId.ppText) = False Then
                        txtTboxId.psSet_ErrorNo("4001", txtTboxId.ppName, "数字")
                    End If
                End If
            End If

            ''仮設置日が未入力の場合、総合テスト日をチェック
            'If Me.dttTemporaryInstallation.ppText = String.Empty Then
            '    If Me.dttTest.ppText = String.Empty Then     '総合テスト日
            '        dttTest.psSet_ErrorNo("5001", dttTest.ppName & "(日)")
            '    End If
            '    If Me.tmtTest.ppHourText = String.Empty _
            '        Or Me.tmtTest.ppMinText = String.Empty Then '総合テスト日(時分)
            '        tmtTest.psSet_ErrorNo("5001", dttTest.ppName & "(時刻)")
            '    End If
            'End If

            ''総合テスト日が未入力の場合、仮設置日をチェック
            'If Me.dttTest.ppText = String.Empty Then     '総合テスト日
            '    If Me.dttTemporaryInstallation.ppText = String.Empty Then     '仮設置日
            '        dttTemporaryInstallation.psSet_ErrorNo("5001", dttTemporaryInstallation.ppName & "(日)")
            '    End If
            '    If Me.tmtTemporaryInstallation.ppHourText = String.Empty _
            '        Or Me.tmtTemporaryInstallation.ppMinText = String.Empty Then '仮設置(時分)
            '        tmtTemporaryInstallation.psSet_ErrorNo("5001", dttTemporaryInstallation.ppName & "(時刻)")
            '    End If
            'End If

            'CNSUPDP001-001 END

            '--------------------------------
            '2015/04/18 加賀　ここまで
            '--------------------------------
            Exit Sub
        End If

        '--------------------------------------------
        '　以下、更新ボタン押下時の入力項目チェック
        '--------------------------------------------
        '仮依頼番号
        If strBtnSts = 2 Then
            If Not Me.ProvisionalRequestNo.ppText = String.Empty Then
                If pfCheck_HanZen(ProvisionalRequestNo.ppText, ClsComVer.E_半角全角.半角) = False Then
                    ProvisionalRequestNo.psSet_ErrorNo("4001", ProvisionalRequestNo.ppName, "半角英数")
                End If
            Else
                Me.ProvisionalRequestNo.psSet_ErrorNo("5001", ProvisionalRequestNo.ppName)
            End If
        End If
        '仕様連絡区分 1:新規、2:変更、3:キャンセル
        If Not Me.txtSpecificationConnectingDivision.ppText = String.Empty Then
            If Not Me.txtSpecificationConnectingDivision.ppText = "1" _
                And Not Me.txtSpecificationConnectingDivision.ppText = "2" _
                And Not Me.txtSpecificationConnectingDivision.ppText = "3" Then   '仕様連絡区分
                Me.txtSpecificationConnectingDivision.psSet_ErrorNo("4001", txtSpecificationConnectingDivision.ppName, "1、2、3")
            End If
        End If
        '--------------------------------
        '2015/04/21 加賀　ここから
        '--------------------------------

        If lblRequestNo_2.Text.IndexOf("N0090") >= 0 Or lblRequestNo_2.Text = String.Empty Then   '仮依頼時
            'ＮＬ区分
            msNL_Check()

            '仮設置　総合テスト
            Select Case String.Empty
                Case dttTest.ppText, tmtTest.ppHourText, tmtTest.ppMinText, _
                     dttTemporaryInstallation.ppText, tmtTemporaryInstallation.ppHourText, tmtTemporaryInstallation.ppMinText
                Case Else
                    Dim lngInst As Long
                    Dim lngTest As Long

                    Select Case False
                        Case Long.TryParse(Me.dttTest.ppText.Replace("/", "") & Me.tmtTest.ppHourText & Me.tmtTest.ppMinText, lngTest), _
                             Long.TryParse(Me.dttTemporaryInstallation.ppText.Replace("/", "") & Me.tmtTemporaryInstallation.ppHourText & Me.tmtTemporaryInstallation.ppMinText, lngInst)
                        Case Else
                            If lngInst > lngTest Then
                                dttTest.psSet_ErrorNo("1006", dttTest.ppName, dttTemporaryInstallation.ppName)
                            End If
                    End Select
            End Select
        Else
            'ＮＬ区分
            Select Case Me.txtNLDivision.ppText
                Case "N", "L", "J"
                Case String.Empty
                    txtNLDivision.psSet_ErrorNo("5001", txtNLDivision.ppName)
                Case Else
                    txtNLDivision.psSet_ErrorNo("4001", txtNLDivision.ppName, "N、LまたはJ")
            End Select

            'ホールコード
            msCheck_HallCode(Me.txtHoleCd, True)
        End If
        '--------------------------------
        '2015/04/21 加賀　ここまで
        '--------------------------------


        '通知番号
        If Not Me.ttwNotificationNo.ppText = String.Empty Then
            If pfCheck_Num(ttwNotificationNo.ppText) = False Then
                ttwNotificationNo.psSet_ErrorNo("4001", ttwNotificationNo.ppName, "数字")
            End If
        End If
        '現行システム
        If Not Me.txtCurrentSys.ppText = String.Empty Then
            If pfCheck_HanZen(txtCurrentSys.ppText, 0) = False Then
                txtCurrentSys.psSet_ErrorNo("4001", txtCurrentSys.ppName, "半角英数字")
            End If
        End If
        'VER
        If Not Me.txtVer.ppText = String.Empty Then
            If pfCheck_HanZen(txtVer.ppText, 0) = False Then
                txtVer.psSet_ErrorNo("4001", "バージョン", "半角英数字")
            End If
        End If
        'システム分類 1:磁気、3:IC、5:LUTERNA
        Select Case Me.txtSysClassification.ppText
            Case String.Empty, "1", "3", "5"
            Case Else
                Me.txtSysClassification.psSet_ErrorNo("4001", txtSysClassification.ppName, "1、3または5")
        End Select
        'TBOXID
        If Not Me.txtTboxId.ppText = String.Empty Then
            If pfCheck_Num(txtTboxId.ppText) = False Then
                txtTboxId.psSet_ErrorNo("4001", txtTboxId.ppName, "数字")
            End If
        End If
        'TBOX回線
        If Not Me.txtTboxLine.ppText = String.Empty Then
            If pfCheck_Num_Sym(txtTboxLine.ppText) = False Then
                txtTboxLine.psSet_ErrorNo("4001", txtTboxLine.ppName, "数字")
            End If
        End If

        'TEL
        If Not Me.txtHoleTelNo.ppText = String.Empty Then
            If pfCheck_Num_Sym(txtHoleTelNo.ppText) = False Then
                txtHoleTelNo.psSet_ErrorNo("4001", "ホール電話番号", "数字")
            End If
        End If

        'ホール内工事
        msHoleCnst_Check()

        'LAN工事(新規) 0:No、1:Yes
        msLANCnst_Check()


        '双子店 1:双子店
        'CNSUPDP001_020
        'If (Not Me.txtTwinsShop.ppText = String.Empty) And (Not Me.txtTwinsShop.ppText = "1") Then
        '    Me.txtTwinsShop.psSet_ErrorNo("4001", txtTwinsShop.ppName, "空または1")
        'End If
        If (Not Me.txtTwinsShop.ppText = "0") And (Not Me.txtTwinsShop.ppText = "1") Then
            Me.txtTwinsShop.psSet_ErrorNo("4001", txtTwinsShop.ppName, "0または1")
        End If
        'CNSUPDP001_020 END


        'ストッカーホール外
        If Not Me.txtStkHoleOut.ppText = String.Empty Then
            If Not Me.txtStkHoleOut.ppText = "0" And Not Me.txtStkHoleOut.ppText = "1" Then
                Me.lblStkHoleOut.Text = ""
                Me.txtStkHoleOut.psSet_ErrorNo("4001", txtStkHoleOut.ppName, "0または1")
            End If
        End If

        'ストッカーホール内
        If Not Me.txtStkHoleIn.ppText = String.Empty Then
            If Not Me.txtStkHoleIn.ppText = "0" And Not Me.txtStkHoleIn.ppText = "1" Then
                Me.lblStkHoleIn.Text = ""
                Me.txtStkHoleIn.psSet_ErrorNo("4001", txtStkHoleIn.ppName, "0または1")
            End If
        End If

        'エフエス稼動有無
        If Not Me.txtSfOperation.ppText = String.Empty Then
            If Not Me.txtSfOperation.ppText = "0" And Not Me.txtSfOperation.ppText = "1" Then
                Me.txtSfOperation.psSet_ErrorNo("4001", txtSfOperation.ppName, "0または1")
            End If
        End If

        'ＴＢＯＸ持帰区分 0:無、1:有
        If Not Me.txtTboxTakeawayDivision.ppText = String.Empty Then
            If Not Me.txtTboxTakeawayDivision.ppText = "0" And Not Me.txtTboxTakeawayDivision.ppText = "1" Then
                Me.txtTboxTakeawayDivision.psSet_ErrorNo("4001", txtTboxTakeawayDivision.ppName, "0または1")
            End If
        End If

        'Ｅマネー導入 0:未導入、1:導入済み
        If Not Me.txtEMoneyIntroduction.ppText = String.Empty Then
            If Not Me.txtEMoneyIntroduction.ppText = "0" And Not Me.txtEMoneyIntroduction.ppText = "1" Then
                Me.txtEMoneyIntroduction.psSet_ErrorNo("4001", txtEMoneyIntroduction.ppName, "0または1")
            End If
        End If
        'Ｅマネー導入工事 0:導入工事無、1:導入工事有
        If Not Me.txtEMoneyIntroductionCns.ppText = String.Empty Then
            If Not Me.txtEMoneyIntroductionCns.ppText = "0" And Not Me.txtEMoneyIntroductionCns.ppText = "1" Then
                Me.txtEMoneyIntroductionCns.psSet_ErrorNo("4001", txtEMoneyIntroductionCns.ppName, "0または1")
            End If
        End If

        'Eマネーテスト日時
        msCheck_DateTime(dttEMoneyTestDt, tmtEMoneyTestTm, False)

        '単独工事 0:単独工事、1:同時工事
        If Not Me.txtIndependentCns.ppText = String.Empty _
            And Not Me.txtIndependentCns.ppText = "0" And Not Me.txtIndependentCns.ppText = "1" Then
            txtIndependentCns.psSet_ErrorNo("4001", txtIndependentCns.ppName, "0または1")
        End If
        '同時工事数
        If Not Me.txtSameTimeCnsNo.ppText = String.Empty Then
            If pfCheck_Num(txtSameTimeCnsNo.ppText) = False Then
                txtSameTimeCnsNo.psSet_ErrorNo("4001", txtSameTimeCnsNo.ppName, "0 ～ 99")
            End If
        End If
        '親子区分 1:親ホール、2:子ホール
        If Not Me.txtPAndCDivision.ppText = String.Empty _
            And Not Me.txtPAndCDivision.ppText = "1" _
            And Not Me.txtPAndCDivision.ppText = "2" Then
            Me.txtPAndCDivision.psSet_ErrorNo("4001", txtPAndCDivision.ppName, "1または2")
        End If
        '親ホールコード
        msCheck_HallCode(Me.txtParentHoleCd, False)

        '工事有無
        '-----2014/7/4 武 from
        If Not Me.txtConstructionExistenceF1.ppText = String.Empty _
            And Not Me.txtConstructionExistenceF1.ppText = "0" _
            And Not Me.txtConstructionExistenceF1.ppText = "1" Then     'Ｆ１
            Me.txtConstructionExistenceF1.psSet_ErrorNo("7002", txtConstructionExistenceF1.ppName)
        End If
        If Not Me.txtConstructionExistenceF2.ppText = String.Empty _
            And Not Me.txtConstructionExistenceF2.ppText = "0" _
            And Not Me.txtConstructionExistenceF2.ppText = "1" Then     'Ｆ２
            Me.txtConstructionExistenceF2.psSet_ErrorNo("7002", txtConstructionExistenceF2.ppName)
        End If
        If Not Me.txtConstructionExistenceF3.ppText = String.Empty _
            And Not Me.txtConstructionExistenceF3.ppText = "0" _
            And Not Me.txtConstructionExistenceF3.ppText = "1" Then    'Ｆ3
            Me.txtConstructionExistenceF3.psSet_ErrorNo("7002", txtConstructionExistenceF3.ppName)
        End If
        If Not Me.txtConstructionExistenceF4.ppText = String.Empty _
            And Not Me.txtConstructionExistenceF4.ppText = "0" _
            And Not Me.txtConstructionExistenceF4.ppText = "1" Then    'Ｆ4
            Me.txtConstructionExistenceF4.psSet_ErrorNo("7002", txtConstructionExistenceF4.ppName)
        End If
        '-----2014/7/4 武 to
        ''営業所コード
        'If Not Me.txtOfficCD.ppText = String.Empty Then
        '    If pfCheck_Num(txtOfficCD.ppText) = False Then
        '        txtOfficCD.psSet_ErrorNo("4001", txtOfficCD.ppName, "数字")
        '    End If
        'End If
        '工事開始日
        msCheck_DateTime(Me.dttStartOfCon, tmtStartOfCon, False)

        '最終営業日
        If Not Me.dttLastOpenDt.ppText = String.Empty Then
            If pfCheck_DateErr(dttLastOpenDt.ppText, False, ClsComVer.E_日付形式.年月日) <> "" Then
                dttLastOpenDt.psSet_ErrorNo("4001", dttLastOpenDt.ppName, "YYYY/MM/DD")
            End If
        End If
        '最終営業日T500
        If Not Me.dttLastOpenDtT500.ppText = String.Empty Then
            If pfCheck_DateErr(dttLastOpenDtT500.ppText, False, ClsComVer.E_日付形式.年月日) <> "" Then
                dttLastOpenDtT500.psSet_ErrorNo("4001", dttLastOpenDtT500.ppName, "YYYY/MM/DD")
            End If
        End If

        '総合テスト
        msCheck_DateTime(Me.dttTest, Me.tmtTest, False)

        '出発日時
        msCheck_DateTime(Me.dttTestDepartureDt, Me.tmtTestDepartureTm, False)

        '仮設置
        msCheck_DateTime(Me.dttTemporaryInstallation, Me.tmtTemporaryInstallation, False)

        '出発日時2
        msCheck_DateTime(Me.dttTemporaryInstallationDepartureDt, Me.tmtTemporaryInstallationDepartureTm, False)

        '仮設置工事区分 0:無、1:有
        If Not Me.dttTemporaryInstallationCnsDivision.ppText = String.Empty _
            And Not Me.dttTemporaryInstallationCnsDivision.ppText = "0" _
            And Not Me.dttTemporaryInstallationCnsDivision.ppText = "1" Then
            Me.dttTemporaryInstallationCnsDivision.psSet_ErrorNo("4001", dttTemporaryInstallationCnsDivision.ppName, "0または1")
        End If
        '仮設置日未入力区分 1:未入力(仮設置工事有で日時未入力の場合に1
        If Not Me.dttTemporaryInstallationDtNotInputDivision0.ppText = String.Empty _
            And Not Me.dttTemporaryInstallationDtNotInputDivision0.ppText = "1" Then
            Me.dttTemporaryInstallationDtNotInputDivision0.psSet_ErrorNo("4001", dttTemporaryInstallationDtNotInputDivision0.ppName, "空または1")
        End If
        '警察
        msCheck_DateTime(Me.dttPolice, Me.tmtPolice, False)

        '移行工事区分 0:その他、1:T500→T700移行工事
        If Not Me.dttShiftCnsDivision.ppText = String.Empty _
            And Not Me.dttShiftCnsDivision.ppText = "0" _
            And Not Me.dttShiftCnsDivision.ppText = "1" Then
            Me.dttShiftCnsDivision.psSet_ErrorNo("4001", dttShiftCnsDivision.ppName, "0または1")
        End If
        '移行工事作業区分 1:LAN移行、2:廃止・新設
        If Not Me.dttShiftCnsWorkDivision.ppText = String.Empty _
            And Not Me.dttShiftCnsWorkDivision.ppText = "0" _
            And Not Me.dttShiftCnsWorkDivision.ppText = "1" _
            And Not Me.dttShiftCnsWorkDivision.ppText = "2" Then
            Me.dttShiftCnsWorkDivision.psSet_ErrorNo("4001", dttShiftCnsWorkDivision.ppName, "0、1または2")
        End If

        'オープン
        msCheck_DateTime(Me.dttOpen, Me.tmtOpen, False)

        'LAN工事
        msCheck_DateTime(Me.dttLanCns, Me.tmtLanCns, False)

        'VERUP
        msCheck_DateTime(Me.dttVerup, Me.tmtVerup, False)

        'ＶＥＲＵＰ日付区分 0:なし、1:同日、2:単独
        If Not Me.dttVerupDtDivision.ppText = String.Empty _
            And Not Me.dttVerupDtDivision.ppText = "0" _
            And Not Me.dttVerupDtDivision.ppText = "1" _
            And Not Me.dttVerupDtDivision.ppText = "2" Then
            Me.dttVerupDtDivision.psSet_ErrorNo("4001", txtVerupCnsType_1.ppName, "0、1または2")
        End If
        '代理店コード
        If Not Me.txtAgencyCd.ppText = String.Empty _
            And Not mfGet_Trader(Me.txtAgencyCd.ppText) Then
            Me.txtAgencyCd.psSet_ErrorNo("7002", txtAgencyCd.ppName)
        End If
        '代理店TEL
        If Not Me.txtAgencyTel.ppText = String.Empty Then
            If pfCheck_Num_Sym(txtAgencyTel.ppText) = False Then
                txtAgencyTel.psSet_ErrorNo("4001", "代理店TEL", "数字")
            End If
        End If
        '代行店コード
        If Not Me.txtAgencyShop.ppText = String.Empty _
            And Not mfGet_Trader(Me.txtAgencyShop.ppText) Then
            Me.txtAgencyShop.psSet_ErrorNo("7002", txtAgencyShop.ppName)
        End If
        '代行店TEL
        If Not Me.txtAgencyShopTelNo.ppText = String.Empty Then
            If pfCheck_Num_Sym(txtAgencyShopTelNo.ppText) = False Then
                txtAgencyShopTelNo.psSet_ErrorNo("4001", "代行店TEL", "数字")
            End If
        End If
        'ＬＡＮ送付先
        If Not Me.txtSendingStation.ppText = String.Empty _
            And Not mfGet_Trader(Me.txtSendingStation.ppText) Then
            txtSendingStation.psSet_ErrorNo("7002", txtSendingStation.ppName)
        End If
        '納入希望日
        msCheck_DateTime(Me.tdtDeliveryPreferredDt, Me.ClsCMTimeBox1, False)

        'LAN送付先TEL
        If Not Me.txtSendingStationTelNo.ppText = String.Empty Then
            If pfCheck_Num_Sym(txtSendingStationTelNo.ppText) = False Then
                txtSendingStationTelNo.psSet_ErrorNo("4001", "LAN送付先TEL", "数字")
            End If
        End If
        'グリッドの入力チェック
        'If pfCheck_GrvListErr(Me.grvList, "0", 3, 15) <> "" Then
        '    txtSendingStationTelNo.psSet_ErrorNo("4001", "一覧", "数字")
        'End If
        If pfCheck_GrvListErrL(Me.grvList, "0", 3, 15) <> "" Then
            txtSendingStationTelNo.psSet_ErrorNo("4001", "一覧", "数字")
        End If
    End Sub
    ''' <summary>
    ''' 営業所コードチェック処理
    ''' </summary>
    ''' <remarks>営業所コード　業者マスタ[2:営業所]</remarks>
    Private Sub msOfficeCD_Check()

        Dim conDB As SqlConnection = Nothing

        If pfCheck_Num(txtOfficCD.ppText) = False Then
            txtOfficCD.psSet_ErrorNo("4001", txtOfficCD.ppName, "数字")
        Else
            Select Case txtOfficCD.ppText.Trim
                Case "990"
                Case String.Empty
                    Me.txtOfficCD.psSet_ErrorNo("5001", txtOfficCD.ppName)
                Case Else
                    '整合性チェック
                    If clsDataConnect.pfOpen_Database(conDB) Then
                        Using cmdDB As SqlCommand = New SqlCommand("CNSUPDP001_S29", conDB)
                            'コマンド設定
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Parameters.Add(pfSet_Param("office_cd", SqlDbType.NVarChar, txtOfficCD.ppText))   '営業所コード

                            Using dstOrders As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                                If dstOrders.Tables(0).Rows.Count = 0 Then
                                    '営業所コード不在
                                    txtOfficCD.psSet_ErrorNo("2002", txtOfficCD.ppName & " : " & txtOfficCD.ppText)
                                End If
                            End Using
                        End Using
                    End If
            End Select
        End If
    End Sub
    ''' <summary>
    ''' 現行システム＆VERチェック処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSysVer_Check()

        Dim conDB As SqlConnection = Nothing
        Dim intFlg As Integer = 0

        '現行システム
        If Me.txtCurrentSys.ppText = String.Empty Then
            '未入力
            Me.txtCurrentSys.psSet_ErrorNo("5001", txtCurrentSys.ppName.Trim)
            intFlg = 1
        Else
            '形式
            If pfCheck_HanZen(txtCurrentSys.ppText, 0) = False Then
                txtCurrentSys.psSet_ErrorNo("4001", txtCurrentSys.ppName, "半角英数字")
                intFlg = 1
            End If
        End If

        'VER
        If Me.txtVer.ppText = String.Empty Then
            '未入力
            Me.txtVer.psSet_ErrorNo("5001", txtVer.ppName.Trim)
            intFlg = 2
        Else
            '形式
            If pfCheck_HanZen(txtVer.ppText, 0) = False Then
                txtVer.psSet_ErrorNo("4001", "バージョン", "半角英数字")
                intFlg = 3
            End If
        End If

        If clsDataConnect.pfOpen_Database(conDB) Then
            '現行システム&VER 整合性チェック
            If intFlg <> 1 Then
                Using cmdDB As SqlCommand = New SqlCommand("CNSUPDP001_S28", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    With cmdDB.Parameters
                        'パラメータ設定
                        .Add(pfSet_Param("prmTBOXCLASS", SqlDbType.NVarChar, txtCurrentSys.ppText))             '現行システム
                        .Add(pfSet_Param("prmVERSION", SqlDbType.NVarChar, txtVer.ppText))                      'Ver
                        .Add(pfSet_Param("prmFLG", SqlDbType.SmallInt, Integer.Parse(ViewState(P_SESSION_TERMS))))    '判定フラグ
                        .Add(pfSet_Param("outTBOXCLASS", SqlDbType.SmallInt, 10, ParameterDirection.Output))    '戻り値 現行システム件数
                        .Add(pfSet_Param("outVERSION", SqlDbType.SmallInt, 10, ParameterDirection.Output))      '戻り値 Ver件数
                        .Add(pfSet_Param("outSYS_GRP", SqlDbType.NVarChar, 10, ParameterDirection.Output))      '戻り値 システム分類
                    End With
                    cmdDB.ExecuteNonQuery()

                    If Integer.Parse(cmdDB.Parameters("outTBOXCLASS").Value.ToString) = 0 Then
                        '現行システム不在
                        txtCurrentSys.psSet_ErrorNo("2002", txtCurrentSys.ppName & " : " & txtCurrentSys.ppText)
                    Else
                        Select Case intFlg
                            Case 2, 3
                            Case Else
                                If Integer.Parse(cmdDB.Parameters("outVERSION").Value.ToString) = 0 Then
                                    'Ver不在
                                    txtVer.psSet_ErrorNo("2002", txtCurrentSys.ppText & "に " & txtVer.ppName & " : " & txtVer.ppText)
                                End If
                        End Select
                    End If
                End Using
            End If
        End If


    End Sub
    ''' <summary>
    ''' NL区分チェック処理
    ''' </summary>
    Private Sub msNL_Check()

        Dim conDB As SqlConnection = Nothing

        Select Case Me.txtNLDivision.ppText
            Case "N", "L", "J"
                'TBOXID 存在チェック
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Using cmdDB = New SqlCommand(sCnsSqlid_S6, conDB)
                        'パラメータ設定
                        cmdDB.Parameters.Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, txtTboxId.ppText)) 'ＴＢＯＸＩＤ

                        Using dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                            If dstOrders.Tables(0).Rows.Count <> 0 Then
                                '整合性チェック
                                If Me.txtNLDivision.ppText <> dstOrders.Tables(0).Rows(0)("T01_NL_CLS").ToString Then
                                    txtNLDivision.psSet_ErrorNo("2012", txtNLDivision.ppName & " と TBOXID")
                                End If
                            End If
                        End Using
                    End Using
                End If
            Case String.Empty
                txtNLDivision.psSet_ErrorNo("5001", txtNLDivision.ppName)
            Case Else
                txtNLDivision.psSet_ErrorNo("4001", txtNLDivision.ppName, "N、LまたはJ")
        End Select

    End Sub
    ''' <summary>
    ''' ホール内工事チェック処理
    ''' </summary>
    Private Sub msHoleCnst_Check()

        'ホール内工事(新規) 0:No、1:Yes
        Select Case Me.txtHoleNew.ppText
            Case "0"
                If (Me.txtHoleExpansion.ppText.Trim = "0" AndAlso _
                    Me.txtHoleSomeRemoval.ppText.Trim = "0" AndAlso _
                    Me.txtHoleShopRelocation.ppText.Trim = "0" AndAlso _
                    Me.txtHoleAllRemoval.ppText.Trim = "0" AndAlso _
                    Me.txtHoleOnceRemoval.ppText.Trim = "0" AndAlso _
                    Me.txtHoleReInstallation.ppText.Trim = "0" AndAlso _
                    Me.txtHoleConChange.ppText.Trim = "0" AndAlso _
                    Me.txtHoleConDelively.ppText.Trim = "0" AndAlso _
                    Me.txtHoleOther.ppText.Trim = "0") Then
                    'ホール内工事全項目が0の場合エラー
                    Me.txtHoleNew.psSet_ErrorNo("2019", lblHoleCns.Text)
                End If
            Case "1"
            Case String.Empty
                Me.txtHoleNew.psSet_ErrorNo("5001", lblHoleCns.Text & lblNew.Text)
            Case Else
                Me.txtHoleNew.psSet_ErrorNo("4001", lblHoleCns.Text & lblNew.Text, "0または1")
        End Select
        'ホール内工事(増設) 0:No、1:Yes
        Select Case Me.txtHoleExpansion.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleExpansion.psSet_ErrorNo("5001", lblHoleCns.Text & lblExpansion.Text)
            Case Else
                Me.txtHoleExpansion.psSet_ErrorNo("4001", lblHoleCns.Text & lblExpansion.Text, "0または1")
        End Select
        'ホール内工事(一部撤去) 0:No、1:Yes
        Select Case Me.txtHoleSomeRemoval.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleSomeRemoval.psSet_ErrorNo("5001", lblHoleCns.Text & lblSomeRemoval.Text)
            Case Else
                Me.txtHoleSomeRemoval.psSet_ErrorNo("4001", lblHoleCns.Text & lblSomeRemoval.Text, "0または1")
        End Select
        'ホール内工事(店舗移設) 0:No、1:Yes
        Select Case Me.txtHoleShopRelocation.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleShopRelocation.psSet_ErrorNo("5001", lblHoleCns.Text & lblShopRelocation.Text)
            Case Else
                Me.txtHoleShopRelocation.psSet_ErrorNo("4001", lblHoleCns.Text & lblShopRelocation.Text, "0または1")
        End Select
        'ホール内工事(全撤去) 0:No、1:Yes
        Select Case Me.txtHoleAllRemoval.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleAllRemoval.psSet_ErrorNo("5001", lblHoleCns.Text & lblAllRemoval.Text)
            Case Else
                Me.txtHoleAllRemoval.psSet_ErrorNo("4001", lblHoleCns.Text & lblAllRemoval.Text, "0または1")
        End Select
        'ホール内工事(一時撤去) 0:No、1:Yes
        Select Case Me.txtHoleOnceRemoval.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleOnceRemoval.psSet_ErrorNo("5001", lblHoleCns.Text & lblOnceRemoval.Text)
            Case Else
                Me.txtHoleOnceRemoval.psSet_ErrorNo("4001", lblHoleCns.Text & lblOnceRemoval.Text, "0または1")
        End Select
        'ホール内工事(再設置) 0:No、1:Yes
        Select Case Me.txtHoleReInstallation.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleReInstallation.psSet_ErrorNo("5001", lblHoleCns.Text & lblReInstallation.Text)
            Case Else
                Me.txtHoleReInstallation.psSet_ErrorNo("4001", lblHoleCns.Text & lblReInstallation.Text, "0または1")
        End Select
        'ホール内工事(構成変更) 0:No、1:Yes(現地)
        Select Case Me.txtHoleConChange.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleConChange.psSet_ErrorNo("5001", lblHoleCns.Text & lblConChange.Text)
            Case Else
                Me.txtHoleConChange.psSet_ErrorNo("4001", lblHoleCns.Text & lblConChange.Text, "0または1")
        End Select
        'ホール内工事(構成配信) 0:No、2:Yes(リモート)
        Select Case Me.txtHoleConDelively.ppText
            Case "0", "2"
            Case String.Empty
                Me.txtHoleConDelively.psSet_ErrorNo("5001", lblHoleCns.Text & lblConDelively.Text)
            Case Else
                Me.txtHoleConDelively.psSet_ErrorNo("4001", lblHoleCns.Text & lblConDelively.Text, "0または2")
        End Select
        'ホール内工事(その他) 0:No、1:Yes
        Select Case Me.txtHoleOther.ppText
            Case "0", "1"
            Case String.Empty
                Me.txtHoleOther.psSet_ErrorNo("5001", lblHoleCns.Text & lblOther.Text)
            Case Else
                Me.txtHoleOther.psSet_ErrorNo("4001", lblHoleCns.Text & lblOther.Text, "0または1")
        End Select

        'ホール内工事(構成変更)(構成配信)パターン
        If txtHoleConChange.ppText = "1" AndAlso txtHoleConDelively.ppText = "2" Then
            Me.txtHoleConChange.psSet_ErrorNo("5005", lblConChange.Text & "と" & lblConDelively.Text)
        End If


    End Sub
    ''' <summary>
    ''' LAN工事チェック処理
    ''' </summary>
    Private Sub msLANCnst_Check()

        'LAN工事(新規) 0:No、1:Yes
        Select Case Me.txtLanNew.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanNew.psSet_ErrorNo("5001", lblLanCns.Text & lblNew.Text)
            Case Else
                Me.txtLanNew.psSet_ErrorNo("4001", lblLanCns.Text & lblNew.Text, "0または1")
        End Select
        'LAN工事(増設) 0:No、1:Yes
        Select Case Me.txtLanExpansion.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanExpansion.psSet_ErrorNo("5001", lblLanCns.Text & lblExpansion.Text)
            Case Else
                Me.txtLanExpansion.psSet_ErrorNo("4001", lblLanCns.Text & lblExpansion.Text, "0または1")
        End Select
        'LAN工事(一部撤去) 0:No、1:Yes
        Select Case Me.txtLanSomeRemoval.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanSomeRemoval.psSet_ErrorNo("5001", lblLanCns.Text & lblSomeRemoval.Text)
            Case Else
                Me.txtLanSomeRemoval.psSet_ErrorNo("4001", lblLanCns.Text & lblSomeRemoval.Text, "0または1")
        End Select
        'LAN工事(店舗移設) 0:No、1:Yes
        Select Case Me.txtLanShopRelocation.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanShopRelocation.psSet_ErrorNo("5001", lblLanCns.Text & lblShopRelocation.Text)
            Case Else
                Me.txtLanShopRelocation.psSet_ErrorNo("4001", lblLanCns.Text & lblShopRelocation.Text, "0または1")
        End Select
        'LAN工事(全撤去) 0:No、1:Yes
        Select Case Me.txtLanAllRemoval.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanAllRemoval.psSet_ErrorNo("5001", lblLanCns.Text & lblAllRemoval.Text)
            Case Else
                Me.txtLanAllRemoval.psSet_ErrorNo("4001", lblLanCns.Text & lblAllRemoval.Text, "0または1")
        End Select
        'LAN工事(一時撤去) 0:No、1:Yes
        Select Case Me.txtLanOnceRemoval.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanOnceRemoval.psSet_ErrorNo("5001", lblLanCns.Text & lblOnceRemoval.Text)
            Case Else
                Me.txtLanOnceRemoval.psSet_ErrorNo("4001", lblLanCns.Text & lblOnceRemoval.Text, "0または1")
        End Select
        'LAN工事(再設置) 0:No、1:Yes
        Select Case Me.txtLanReInstallation.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanReInstallation.psSet_ErrorNo("5001", lblLanCns.Text & lblReInstallation.Text)
            Case Else
                Me.txtLanReInstallation.psSet_ErrorNo("4001", lblLanCns.Text & lblReInstallation.Text, "0または1")
        End Select
        'LAN工事(構成変更) 0:No、1:Yes(現地)
        Select Case Me.txtLanConChange.ppText
            Case "0", "1", "2"
            Case String.Empty
                'Me.txtLanConChange.psSet_ErrorNo("5001", lblLanCns.Text & lblConChange.Text)
            Case Else
                Me.txtLanConChange.psSet_ErrorNo("4001", lblLanCns.Text & lblConChange.Text, "0または1")
        End Select
        'LAN工事(構成配信) 0:No、2:Yes(リモート)
        Select Case Me.txtLanConDelively.ppText
            Case "0", "1", "2"
            Case String.Empty
                'Me.txtLanConDelively.psSet_ErrorNo("5001", lblLanCns.Text & lblConDelively.Text)
            Case Else
                Me.txtLanConDelively.psSet_ErrorNo("4001", lblLanCns.Text & lblConDelively.Text, "0または1")
        End Select
        'LAN工事(その他) 0:No、1:Yes
        Select Case Me.txtLanOther.ppText
            Case "0", "1"
            Case String.Empty
                'Me.txtLanOther.psSet_ErrorNo("5001", lblLanCns.Text & lblOther.Text)
            Case Else
                Me.txtLanOther.psSet_ErrorNo("4001", lblLanCns.Text & lblOther.Text, "0または1")
        End Select

    End Sub
    ''' <summary>
    ''' 仮設置・総合テスト日時チェック処理
    ''' </summary>
    Private Sub msTestDT_Check()

        Dim dtmInst As DateTime '仮設置日時
        Dim dtmTest As DateTime '総合テスト日時

        '総合テスト
        If dttTest.ppText & tmtTest.ppHourText & tmtTest.ppMinText = String.Empty Then  '全て未入力
            If dttTemporaryInstallation.ppText = String.Empty Then
                dttTest.psSet_ErrorNo("5001", dttTest.ppName & "(日)")    '日付　未入力
                tmtTest.psSet_ErrorNo("5001", dttTest.ppName & "(時刻)")  '時刻　未入力
            End If
        Else 'いずれか入力済
            '日付
            Select Case String.Empty
                Case dttTest.ppText
                    dttTest.psSet_ErrorNo("5001", dttTest.ppName & "(日)")
                Case pfCheck_DateErr(dttTest.ppText, False, ClsComVer.E_日付形式.年月日)
                Case Else
                    dttTest.psSet_ErrorNo("4001", "総合テスト日付", "正しい日付")
            End Select
            '時刻
            Select Case String.Empty
                Case tmtTest.ppHourText, tmtTest.ppMinText
                    tmtTest.psSet_ErrorNo("5001", dttTest.ppName & "(時刻)")
                Case pfCheck_TimeErr(tmtTest.ppHourText, tmtTest.ppMinText, False, False)
                Case Else
                    tmtTest.psSet_ErrorNo("4001", "総合テスト時間", "正しい時刻")
            End Select
        End If

        '仮設置
        If dttTemporaryInstallation.ppText & tmtTemporaryInstallation.ppHourText & tmtTemporaryInstallation.ppMinText = String.Empty Then  '全て未入力
            If dttTest.ppText = String.Empty Then
                dttTemporaryInstallation.psSet_ErrorNo("5001", dttTemporaryInstallation.ppName & "(日)")    '日付　未入力
                tmtTemporaryInstallation.psSet_ErrorNo("5001", dttTemporaryInstallation.ppName & "(時刻)")  '時刻　未入力
            End If
        Else 'いずれか入力済
            '日付
            Select Case String.Empty
                Case dttTemporaryInstallation.ppText
                    dttTemporaryInstallation.psSet_ErrorNo("5001", dttTemporaryInstallation.ppName & "(日)")
                Case pfCheck_DateErr(dttTemporaryInstallation.ppText, False, ClsComVer.E_日付形式.年月日)
                Case Else
                    dttTemporaryInstallation.psSet_ErrorNo("4001", "仮設置日付", "正しい日付")
            End Select
            '時刻
            Select Case String.Empty
                Case tmtTemporaryInstallation.ppHourText, tmtTemporaryInstallation.ppMinText
                    tmtTemporaryInstallation.psSet_ErrorNo("5001", dttTemporaryInstallation.ppName & "(時刻)")
                Case pfCheck_TimeErr(tmtTemporaryInstallation.ppHourText, tmtTemporaryInstallation.ppMinText, False, False)
                Case Else
                    tmtTemporaryInstallation.psSet_ErrorNo("4001", "仮設置時間", "正しい時刻")
            End Select
        End If

        '仮設置、総合テスト 前後チェック
        Select Case String.Empty
            Case dttTest.ppText, tmtTest.ppHourText, tmtTest.ppMinText, _
                 dttTemporaryInstallation.ppText, tmtTemporaryInstallation.ppHourText, tmtTemporaryInstallation.ppMinText
            Case Else
                Select Case False
                    Case DateTime.TryParse(dttTest.ppText & " " & tmtTest.ppHourText & ":" & tmtTest.ppMinText, dtmTest)
                        '総合テスト 日時形式エラー
                    Case DateTime.TryParse(dttTemporaryInstallation.ppText & " " & tmtTemporaryInstallation.ppHourText & ":" & tmtTemporaryInstallation.ppMinText, dtmInst)
                        '仮設置 日時形式エラー
                    Case Else
                        If dtmInst > dtmTest Then
                            dttTest.psSet_ErrorNo("1006", dttTest.ppName, dttTemporaryInstallation.ppName)
                        End If
                End Select
        End Select

    End Sub
    ''' <summary>
    ''' 日時チェック処理
    ''' </summary>
    Private Sub msCheck_DateTime(ByVal txtDate As SPC.ClsCMDateBox, ByVal txtTime As SPC.ClsCMTimeBox, ByVal IsRequire As Boolean)

        '入力値格納
        Dim strDate As String = txtDate.ppText      '日付(YYYY/MM/DD)
        Dim strHour As String = txtTime.ppHourText  '時間(hh)
        Dim strMin As String = txtTime.ppMinText    '分  (mm)

        Select Case String.Empty
            Case strDate & strHour & strMin '全て未入力
                If IsRequire = True Then
                    '未入力エラー
                    txtDate.psSet_ErrorNo("5001", txtDate.ppName & "(日)")
                End If
            Case strHour & strMin           '日付のみ入力
                '日付形式チェック
                If pfCheck_DateErr(txtDate.ppText, False, ClsComVer.E_日付形式.年月日) <> String.Empty Then
                    txtDate.psSet_ErrorNo("4001", txtDate.ppName, "正しい日付")
                End If
            Case strDate                    '時間のみ入力
                txtDate.psSet_ErrorNo("5001", txtDate.ppName & "(時刻)")
            Case strHour, strMin            '時間半端入力
                txtTime.psSet_ErrorNo("4001", txtDate.ppName & "(時刻)", "正しい時刻")
            Case Else                       '全て入力
                '日付形式チェック
                If pfCheck_DateErr(txtDate.ppText, False, ClsComVer.E_日付形式.年月日) <> String.Empty Then
                    txtDate.psSet_ErrorNo("4001", txtDate.ppName, "正しい日付")
                End If
                '時間形式チェック
                If pfCheck_TimeErr(txtTime.ppHourText, txtTime.ppMinText, False, False) <> String.Empty Then
                    Select Case txtTime.ppName
                        Case String.Empty, "Label"
                            txtTime.psSet_ErrorNo("4001", txtDate.ppName & "(時刻)", "正しい時刻")
                        Case Else
                            txtTime.psSet_ErrorNo("4001", txtTime.ppName, "正しい時刻")
                    End Select
                End If
        End Select
    End Sub
    ''' <summary>
    ''' ホールコードチェック処理
    ''' </summary>
    Private Sub msCheck_HallCode(ByVal txtHallCode As ClsCMTextBox, ByVal IsRequire As Boolean)

        Dim strHallCD As String = txtHallCode.ppText
        Dim conDB As SqlConnection = Nothing

        If txtHallCode.ppText = String.Empty Then
            '未入力
            If IsRequire Then
                txtHallCode.psSet_ErrorNo("5001", txtHallCode.ppName)
            End If
        Else
            If pfCheck_Pattem(strHallCD, "[0-9]{7}") = False Then
                '形式エラー
                txtHoleCd.psSet_ErrorNo("4001", txtHoleCd.ppName, "7桁の数字")
            Else
                'T01ホールコード存在確認
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Using cmdDB As SqlCommand = New SqlCommand("SMTDSY001_S10", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("prmD01_HALL_CD", SqlDbType.NVarChar, strHallCD)) 'ホールコード
                        End With
                        Using dst As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                            If dst.Tables(0).Rows.Count = 0 Then
                                'ホールコード不在
                                txtHallCode.psSet_ErrorNo("2002", txtHallCode.ppName)
                            End If
                        End Using
                    End Using
                End If
            End If
        End If

    End Sub

#End Region

#Region "登録・更新処理"

    ''' <summary>
    ''' 登録・更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function mfSet_Data(ByVal intShori As Integer) As Boolean

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0
        Dim strMes As String = String.Empty
        Dim strTestDepartureDt As String = String.Empty
        Dim strTemInsDepDt As String = String.Empty
        Dim strCNST_CLS_Chk As StringBuilder = New StringBuilder
        mfSet_Data = False
        objStack = New StackFrame

        Try
            If intShori = 0 Then
                '仮登録データ(N0090)と本データ(TOMAS)の紐付け
                Return mfHimotsuke()
            Else
                'ＤＢ接続
                If clsDataConnect.pfOpen_Database(conDB) Then
                    cmdDB.Connection = conDB    'トランザクション
                Else
                    Throw New Exception("")
                End If

                'SPCデータ重複確認
                Dim dstOrders_5 As New DataSet
                cmdDB = New SqlCommand(sCnsSqlid_S16, conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                End With
                dstOrders_5 = clsDataConnect.pfGet_DataSet(cmdDB)

                'トランザクション設定
                trans = conDB.BeginTransaction

                '工事依頼書兼仕様書編集処理
                If Me.msSet_D39_CNSTREQSPEC_Edit() = False Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事依頼書兼仕様書の編集処理")
                    Return False
                End If

                If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then

                    '工事依頼書兼仕様書  登録
                    cmdDB = New SqlCommand(sCnsSqlid_I1, conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    With cmdDB.Parameters
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.ProvisionalRequestNo.ppTextOne & "-" & Me.ProvisionalRequestNo.ppTextTwo)) '工事依頼番号
                        .Add(pfSet_Param("@tvp", SqlDbType.Structured, CNSUPDP001_D39_CNSTREQSPEC.Tables(0))) '工事依頼書兼仕様書 テーブル
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    cmdDB.ExecuteNonQuery() '実行

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "工事依頼書兼仕様書  登録 " & sCnsSqlid_I1
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If

                    'パラメータ設定（工事設計依頼請書  登録）
                    cmdDB = New SqlCommand(sCnsSqlid_I5, conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    With cmdDB.Parameters
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.ProvisionalRequestNo.ppTextOne & "-" & Me.ProvisionalRequestNo.ppTextTwo)) '工事依頼番号
                        .Add(pfSet_Param("@NTTD_CHARGE", SqlDbType.NVarChar, Me.trfSfPersonInCharge.ppText))                'FS担当者
                        If Me.RadioButtonList1.SelectedIndex = 0 Then
                            .Add(pfSet_Param("PROC_RSLT", SqlDbType.NVarChar, "ＯＫ"))                                         '処理結果
                        Else
                            .Add(pfSet_Param("PROC_RSLT", SqlDbType.NVarChar, "ＮＧ"))                                         '処理結果
                        End If
                        .Add(pfSet_Param("PROC_RSLT_DTL", SqlDbType.NVarChar, Me.txtProcessingResultDetail1.ppText.ToString _
                                                                            & "##" & Me.txtProcessingResultDetail2.ppText.ToString _
                                                                            & "##" & Me.txtProcessingResultDetail3.ppText.ToString))    '処理結果詳細
                        .Add(pfSet_Param("NOTETEXT", SqlDbType.NVarChar, Me.txtControlInfoRemarks1.ppText.ToString _
                                                                            & "##" & Me.txtControlInfoRemarks2.ppText.ToString _
                                                                            & "##" & Me.txtControlInfoRemarks3.ppText.ToString))        '備考
                        .Add(pfSet_Param("user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                                   '更新者
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    cmdDB.ExecuteNonQuery() '実行

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "工事設計依頼請書  登録 " & sCnsSqlid_I5
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If

                    If Me.RadioButtonList1.Text <> "2" Then
                        If Not Me.ddlMTRStatus.SelectedValue Is Nothing Then
                            '入力項目を非活性
                            If CInt(Me.ddlMTRStatus.SelectedValue.Substring(0, 2)) < 7 Then

                                '受付確定時にＤＬＬ設定変更依頼のデータを作成する
                                'CNSUPDP001_003
                                'If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                                'CNSUPDP001_003 END

                                Dim ds As New DataSet
                                Dim addDate() As Integer = Nothing

                                'ＤＬＬ設定変更依頼の登録判定
                                If msCheck_DllDt(ds, addDate) Then
                                    ''レコードの重複確認
                                    'If dstOrders_5.Tables(0).Rows.Count = 0 Then
                                    '    cmdDB = New SqlCommand(sCnsSqlid_I6, conDB)
                                    '    cmdDB.CommandType = CommandType.StoredProcedure
                                    '    With cmdDB.Parameters
                                    '        .Add(pfSet_Param("tvp", SqlDbType.Structured, ds.Tables(0)))  '運用モード設定変更テーブル
                                    '        '-----------------------------
                                    '        '2014/08/01 星野　ここから
                                    '        '-----------------------------
                                    '        '.Add(pfSet_Param("add", SqlDbType.Int, addDate))              '追加日付数
                                    '        .Add(pfSet_Param("add1", SqlDbType.Int, addDate(0)))           '追加日付数(設定日時)
                                    '        .Add(pfSet_Param("add2", SqlDbType.Int, addDate(1)))           '追加日付数(戻し日時)
                                    '        '-----------------------------
                                    '        '2014/08/01 星野　ここから
                                    '        '-----------------------------
                                    '        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                    '    End With
                                    'Else
                                    cmdDB = New SqlCommand("CNSUPDP001_U5", conDB)
                                    cmdDB.CommandType = CommandType.StoredProcedure
                                    cmdDB.Transaction = trans
                                    With cmdDB.Parameters
                                        .Add(pfSet_Param("tvp", SqlDbType.Structured, ds.Tables(0)))  '運用モード設定変更テーブル
                                        .Add(pfSet_Param("add1", SqlDbType.Int, addDate(0)))           '追加日付数(設定日時)
                                        .Add(pfSet_Param("add2", SqlDbType.Int, addDate(1)))           '追加日付数(戻し日時)
                                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.ProvisionalRequestNo.ppTextBoxOne.Text + "-" + Me.ProvisionalRequestNo.ppTextBoxTwo.Text))
                                    End With
                                    'End If

                                    cmdDB.ExecuteNonQuery() '実行

                                    'ストアド戻り値チェック
                                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                    If intRtn <> 0 Then
                                        strMes = "ＤＬＬ設定変更依頼の登録 CNSUPDP001_U5"
                                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                                        trans.Rollback()    'ロールバック
                                        Exit Try
                                    End If
                                    'CNSUPDP001_002
                                Else
                                    'チェックボックスにチェックがついている時、DLLデータの存在確認をし、存在していたら削除する
                                    strMes = "ＤＬＬ設定変更依頼の削除"
                                    cmdDB = New SqlCommand("CNSUPDP001_D4", conDB)
                                    cmdDB.CommandType = CommandType.StoredProcedure
                                    cmdDB.Transaction = trans
                                    With cmdDB.Parameters
                                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                                    End With
                                End If

                                cmdDB.ExecuteNonQuery() '実行

                                'ストアド戻り値チェック
                                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                If intRtn <> 0 Then
                                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                                    trans.Rollback()    'ロールバック
                                    Exit Try
                                    'CNSUPDP001_002 END
                                End If
                                'CNSUPDP001_003
                                'End If
                                'CNSUPDP001_003 END
                            End If
                        End If
                    End If
                Else

                    'パラメータ設定（工事依頼書兼仕様書履歴編集処理  登録）
                    cmdDB = New SqlCommand(sCnsSqlid_I2, conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    With cmdDB.Parameters
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text)) '工事依頼番号
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With
                    cmdDB.ExecuteNonQuery() '実行

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "工事依頼書兼仕様書履歴編集処理  登録 " & sCnsSqlid_I2
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If

                    'パラメータ設定（工事依頼書兼仕様書  削除）
                    cmdDB = New SqlCommand(sCnsSqlid_D1, conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    With cmdDB.Parameters
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text)) '工事依頼番号
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With
                    cmdDB.ExecuteNonQuery() '実行

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "工事依頼書兼仕様書  削除 " & sCnsSqlid_D1
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If

                    'パラメータ設定（工事依頼書兼仕様書  登録）
                    cmdDB = New SqlCommand(sCnsSqlid_I1, conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    With cmdDB.Parameters
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))                '工事依頼番号
                        .Add(pfSet_Param("@tvp", SqlDbType.Structured, CNSUPDP001_D39_CNSTREQSPEC.Tables(0)))   '工事依頼書兼仕様書
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With
                    cmdDB.ExecuteNonQuery() '実行

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "工事依頼書兼仕様書  登録 " & sCnsSqlid_I1
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If

                    If ViewState(M_STE_TMP_DT) = "8" Then '仮設置から本設置になった場合
                        'パラメータ設定（工事依頼書兼仕様書  登録）
                        cmdDB = New SqlCommand(sCnsSqlid_U3, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            .Add(pfSet_Param("@tvp", SqlDbType.Structured, CNSUPDP001_D39_CNSTREQSPEC.Tables(0))) '工事依頼書兼仕様書
                        End With
                        cmdDB.ExecuteNonQuery() '実行

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            strMes = "工事依頼書兼仕様書  登録 " & sCnsSqlid_U3
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            trans.Rollback()    'ロールバック
                            Exit Try
                        End If
                    End If

                    'CNSUPDP001_016
                    'パラメータ設定（工事依頼書兼仕様書  削除）
                    cmdDB = New SqlCommand("CNSUPDP001_U10", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.Transaction = trans
                    With cmdDB.Parameters
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))    '工事依頼番号
                        If dttTest.ppText = String.Empty Then
                            .Add(pfSet_Param("cnst_dt", SqlDbType.DateTime, DBNull.Value))          '総合テスト日
                        Else
                            .Add(pfSet_Param("cnst_dt", SqlDbType.DateTime, dttTest.ppDate))        '総合テスト日
                        End If
                        .Add(pfSet_Param("FsWrk_Cls", SqlDbType.NVarChar, txtSfOperation.ppText))   'FS稼働
                        .Add(pfSet_Param("userid", SqlDbType.NVarChar, Session(P_SESSION_USERID)))  'ユーザーID
                        If txtHoleAllRemoval.ppText = "1" OrElse txtHoleOnceRemoval.ppText = "1" Then
                            .Add(pfSet_Param("Rmv_cls", SqlDbType.Int, 1))
                        Else
                            .Add(pfSet_Param("Rmv_cls", SqlDbType.Int, 0))
                        End If
                    End With
                    cmdDB.ExecuteNonQuery() '実行

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "随時一覧  更新 " & "CNSUPDP001_U10"
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If
                    'CNSUPDP001_016 END

                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                        'パラメータ設定（工事依頼書兼仕様書　機器明細１ 　機器明細２  登録）
                        cmdDB = New SqlCommand(sCnsSqlid_I4, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text)) '工事依頼番号
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                        cmdDB.ExecuteNonQuery() '実行

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            strMes = "工事依頼書兼仕様書(機器明細１ 　機器明細２) 登録 " & sCnsSqlid_I4
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            trans.Rollback()    'ロールバック
                            Exit Try
                        End If

                        'パラメータ設定（工事設計依頼請書  登録）
                        Dim dstOrders_6 As New DataSet
                        cmdDB = New SqlCommand(sCnsSqlid_S27, conDB)
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                        End With

                        dstOrders_6 = clsDataConnect.pfGet_DataSet(cmdDB)

                        If dstOrders_6.Tables(0).Rows.Count = 0 Then
                            cmdDB = New SqlCommand(sCnsSqlid_I5, conDB)
                        Else
                            cmdDB = New SqlCommand("CNSUPDP001_U4", conDB)
                        End If
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))                    '工事依頼番号
                            .Add(pfSet_Param("@NTTD_CHARGE", SqlDbType.NVarChar, Me.trfSfPersonInCharge.ppText))        'FS担当者

                            If Me.RadioButtonList1.SelectedIndex = 0 Then
                                .Add(pfSet_Param("PROC_RSLT", SqlDbType.NVarChar, "ＯＫ"))                              '処理結果
                            Else
                                .Add(pfSet_Param("PROC_RSLT", SqlDbType.NVarChar, "ＮＧ"))                              '処理結果
                            End If
                            .Add(pfSet_Param("PROC_RSLT_DTL", SqlDbType.NVarChar, Me.txtProcessingResultDetail1.ppText.ToString _
                                                                                & "##" & Me.txtProcessingResultDetail2.ppText.ToString _
                                                                                & "##" & Me.txtProcessingResultDetail3.ppText.ToString))    '処理結果詳細
                            .Add(pfSet_Param("NOTETEXT", SqlDbType.NVarChar, Me.txtControlInfoRemarks1.ppText.ToString _
                                                                                & "##" & Me.txtControlInfoRemarks2.ppText.ToString _
                                                                                & "##" & Me.txtControlInfoRemarks3.ppText.ToString))        '備考
                            .Add(pfSet_Param("user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))                                   '更新者
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                        cmdDB.ExecuteNonQuery() '実行

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            strMes = "工事設計依頼請書  登録"
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            trans.Rollback()    'ロールバック
                            Exit Try
                        End If

                        'パラメータ設定（工事設計依頼書　工事設計依頼明細  削除）
                        cmdDB = New SqlCommand(sCnsSqlid_D3, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))                        '工事依頼番号
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                        cmdDB.ExecuteNonQuery() '実行

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            strMes = "工事設計依頼書　工事設計依頼明細  削除 " & sCnsSqlid_D3
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                            trans.Rollback()    'ロールバック
                            Exit Try
                        End If
                    End If

                    'FS稼動有無によりDLL設定変更依頼のデータを作成する。
                    'If Me.txtSfOperation.ppText = 0 And _
                    If Me.RadioButtonList1.Text <> "2" Then
                        If Not Me.ddlMTRStatus.SelectedValue Is Nothing Then
                            '入力項目を非活性
                            If CInt(Me.ddlMTRStatus.SelectedValue.Substring(0, 2)) < 7 Then

                                '受付確定時にＤＬＬ設定変更依頼のデータを作成する
                                'CNSUPDP001_003
                                'If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                                'CNSUPDP001_003 END

                                Dim ds As New DataSet
                                Dim addDate() As Integer = Nothing

                                'ＤＬＬ設定変更依頼の登録判定
                                If msCheck_DllDt(ds, addDate) Then
                                    ''レコードの重複確認
                                    'If dstOrders_5.Tables(0).Rows.Count = 0 Then
                                    '    cmdDB = New SqlCommand(sCnsSqlid_I6, conDB)
                                    '    cmdDB.CommandType = CommandType.StoredProcedure
                                    '    With cmdDB.Parameters
                                    '        .Add(pfSet_Param("tvp", SqlDbType.Structured, ds.Tables(0)))  '運用モード設定変更テーブル
                                    '        '-----------------------------
                                    '        '2014/08/01 星野　ここから
                                    '        '-----------------------------
                                    '        '.Add(pfSet_Param("add", SqlDbType.Int, addDate))              '追加日付数
                                    '        .Add(pfSet_Param("add1", SqlDbType.Int, addDate(0)))           '追加日付数(設定日時)
                                    '        .Add(pfSet_Param("add2", SqlDbType.Int, addDate(1)))           '追加日付数(戻し日時)
                                    '        '-----------------------------
                                    '        '2014/08/01 星野　ここから
                                    '        '-----------------------------
                                    '        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                    '    End With
                                    'Else
                                    cmdDB = New SqlCommand("CNSUPDP001_U5", conDB)
                                    cmdDB.CommandType = CommandType.StoredProcedure
                                    cmdDB.Transaction = trans
                                    With cmdDB.Parameters
                                        .Add(pfSet_Param("tvp", SqlDbType.Structured, ds.Tables(0)))  '運用モード設定変更テーブル
                                        .Add(pfSet_Param("add1", SqlDbType.Int, addDate(0)))           '追加日付数(設定日時)
                                        .Add(pfSet_Param("add2", SqlDbType.Int, addDate(1)))           '追加日付数(戻し日時)
                                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                                    End With
                                    'End If

                                    cmdDB.ExecuteNonQuery() '実行

                                    'ストアド戻り値チェック
                                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                    If intRtn <> 0 Then
                                        strMes = "ＤＬＬ設定変更依頼の登録 CNSUPDP001_U5"
                                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                                        trans.Rollback()    'ロールバック
                                        Exit Try
                                    End If
                                    'CNSUPDP001_002
                                Else
                                    'チェックボックスにチェックがついている時、DLLデータの存在確認をし、存在していたら削除する
                                    cmdDB = New SqlCommand("CNSUPDP001_D4", conDB)
                                    cmdDB.CommandType = CommandType.StoredProcedure
                                    cmdDB.Transaction = trans
                                    With cmdDB.Parameters
                                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                                    End With
                                    cmdDB.ExecuteNonQuery() '実行

                                    'ストアド戻り値チェック
                                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                                    If intRtn <> 0 Then
                                        strMes = "ＤＬＬ設定変更依頼の削除 CNSUPDP001_D4"
                                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                                        trans.Rollback()    'ロールバック
                                        Exit Try
                                        'CNSUPDP001_002 END
                                    End If
                                    'CNSUPDP001_003
                                    'End If
                                    'CNSUPDP001_003 END
                                End If
                            End If
                        End If
                    End If
                    If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                        'キャンセルの場合は処理完了にする
                        If CNSUPDP001_D39_CNSTREQSPEC.Tables(0).Rows(0).Item("D39_TELL_CLS").ToString = "3" Then
                            cmdDB = New SqlCommand("CNSUPDP001_D4", conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text)) '工事依頼番号
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With
                            cmdDB.ExecuteNonQuery() '実行

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                strMes = "キャンセルの場合は処理完了にする CNSUPDP001_D4"
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                                trans.Rollback()    'ロールバック
                                Exit Try
                            End If
                        End If
                    End If
                End If
            End If

            'コミッテッド
            trans.Commit()

            'オブジェクトの破棄を保証する
            cmdDB.Dispose()

            mfSet_Data = True

        Catch ex As DBConcurrencyException
            'ロールバック
            trans.Rollback()
            mfSet_Data = False
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            mfSet_Data = False

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'CNSUPDP001_013
            'Me.grvList.DataBind()
            'Throw
            'CNSUPDP001_013 END
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            'ログ出力
            If strMes <> String.Empty Then
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", strMes, )
            End If
        End Try

    End Function

    ''' <summary>
    ''' 仮依頼と本依頼の紐付け
    ''' </summary>
    ''' <param name="strMes">エラー内容</param>
    ''' <remarks></remarks>
    Private Function mfHimotsuke(Optional ByRef strMes As String = Nothing) As Boolean

        mfHimotsuke = False

        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim trans As SqlTransaction = Nothing
        Dim dstOrders As New DataSet
        Dim strCnsCls As String = Nothing
        Dim intRtn As Integer = 0
        objStack = New StackFrame

        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                cmdDB.Connection = conDB    'トランザクション
            Else
                strMes = "ＤＢ接続失敗"
                Exit Try
            End If

            If btmAttachRequestNo.Text = "変更または解除" AndAlso ProvisionalRequestNo.ppTextTwo = String.Empty Then
                '紐付け解除のときは存在判定を行わない
            Else
                '画面入力した仮工事依頼番号が工事依頼書兼仕様書(D39)に存在するか
                cmdDB = New SqlCommand("CNSUPDP001_S5", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, _
                                     ProvisionalRequestNo.ppTextOne.ToString & "-" & _
                                     ProvisionalRequestNo.ppTextTwo.ToString))       '仮依頼番号
                End With
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '存在判定
                If dstOrders.Tables(0).Rows.Count < 1 Then
                    psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                    Exit Try
                End If

                '紐付けフラグ判定(1:紐付け済)
                If dstOrders.Tables(0).Rows(0).Item("D39_CNCT_FLG") = "1" Then
                    psMesBox(Me, "40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "記入された仮依頼番号は", "紐付け済みです。")
                    Exit Try
                End If

                '本データの案件ステータス判定(01:未処理)
                If ddlMTRStatus.SelectedValue = "01" Then
                    psMesBox(Me, "40002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Me.lblRequestNo_1.Text, "未処理", "受付確定")
                    Exit Try
                End If

                'TBOXID判定
                If txtTboxId.ppText <> dstOrders.Tables(0).Rows(0).Item("D39_TBOXID") Then
                    psMesBox(Me, "50002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, txtTboxId.ppText, dstOrders.Tables(0).Rows(0).Item("D39_TBOXID"))
                    Exit Try
                End If

                '工事種別判定
                strCnsCls = txtHoleNew.ppText & txtHoleExpansion.ppText & txtHoleSomeRemoval.ppText & txtHoleShopRelocation.ppText & _
                    txtHoleAllRemoval.ppText & txtHoleOnceRemoval.ppText & txtHoleReInstallation.ppText & _
                    txtHoleConChange.ppText & txtHoleConDelively.ppText & txtHoleOther.ppText
                If Not strCnsCls.ToString = dstOrders.Tables(0).Rows(0).Item("CNST_CLS") Then
                    psMesBox(Me, "30011", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "仮依頼の工事種別")
                    Exit Try
                End If
            End If

            '本データ、仮データを更新する
            '本データの紐付け確認
            cmdDB = New SqlCommand("CNSUPDP001_S18", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
            End With

            Using dstOrders_2 As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                trans = conDB.BeginTransaction
                '既に紐付けてある場合、解除する
                If dstOrders_2.Tables(0).Rows(0).Item("D39_CNCT_FLG") = "1" Then
                    cmdDB = New SqlCommand("CNSUPDP001_U6", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    With cmdDB.Parameters
                        '工事依頼番号
                        .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                        '仮登録工事依頼番号
                        .Add(pfSet_Param("PRECNCT_NO", SqlDbType.NVarChar, dstOrders_2.Tables(0).Rows(0).Item("D39_PRECNST_NO").ToString))
                        '更新者
                        .Add(pfSet_Param("UPDATE_USER", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        '戻り値
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    cmdDB.Transaction = trans
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        strMes = "工事依頼書兼仕様書紐付け更新 CNSUPDP001_U6"
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                        trans.Rollback()    'ロールバック
                        Exit Try
                    End If
                End If
            End Using

            '紐付け解除のとき、紐付け処理を行わない
            If Not ProvisionalRequestNo.ppTextTwo = String.Empty Then

                cmdDB = New SqlCommand("CNSUPDP001_U2", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    '工事依頼番号
                    .Add(pfSet_Param("const_no", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                    '仮登録工事依頼番号
                    .Add(pfSet_Param("precnst_no", SqlDbType.NVarChar, Me.ProvisionalRequestNo.ppTextBoxOne.Text & "-" & Me.ProvisionalRequestNo.ppTextBoxTwo.Text))
                    '総合テスト作業担当者
                    .Add(pfSet_Param("stest_charge", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_STEST_CHARGE").ToString))
                    '総合テスト作業担当者コード
                    .Add(pfSet_Param("stest_charge_cd", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_STEST_CHARGE_CD").ToString))
                    '総合テスト出発日時
                    .Add(pfSet_Param("stestdept_dt", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_STESTDEPT_DT").ToString))
                    '仮設置作業担当
                    .Add(pfSet_Param("tmpset_charge", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_TMPSET_CHARGE").ToString))
                    '仮設置作業担当コード
                    .Add(pfSet_Param("tmpset_charge_cd", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_TMPSET_CHARGE_CD").ToString))
                    '仮設置出発日時
                    .Add(pfSet_Param("tmpsetdept_dt", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_TMPSETDEPT_DT").ToString))
                    '担当営業所コード
                    .Add(pfSet_Param("branch_cd", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_BRANCH_CD").ToString))
                    '担当営業所名
                    .Add(pfSet_Param("branch_nm", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_BRANCH_NM").ToString))
                    '更新者
                    .Add(pfSet_Param("update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    '案件進捗ステータス
                    .Add(pfSet_Param("status_cd", SqlDbType.NVarChar, dstOrders.Tables(0).Rows(0).Item("D39_MTR_STATUS_CD").ToString))
                    '戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                cmdDB.Transaction = trans
                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    strMes = "工事依頼書兼仕様書紐付け更新 CNSUPDP001_U2"
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMes)
                    trans.Rollback()    'ロールバック
                    Exit Try
                End If
            End If

            '破棄
            dstOrders.Dispose()

            'コミッテッド
            trans.Commit()

            '成功
            mfHimotsuke = True

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'コマンドの破棄
            cmdDB.Dispose()

            'ログ出力
            If Not strMes Is Nothing Then
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "ERROR", strMes)
            End If
        End Try

    End Function

#End Region

#Region "工事依頼書兼仕様書処理"

    ''' <summary>
    ''' 工事依頼書兼仕様書処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_D39_CNSTREQSPEC_Edit() As Boolean

        msSet_D39_CNSTREQSPEC_Edit = False

        Dim dataset As New CNSUPDP001_D39_CNSTREQSPEC
        Dim conDB As SqlConnection = Nothing
        Dim dtTable As New D39_CNSTREQSPECDataTable
        Dim dtRow As DataRow
        Dim strDateTime As StringBuilder = New StringBuilder
        Dim strMes As String = Nothing

        Try
            'データセットのインスタンスを作成
            dtRow = dataset.Tables(0).NewRow

            Dim dtD39_DATA_SV As DataSet
            dtD39_DATA_SV = ViewState("D39_DATA_SV")

            '本設置、仮設置の判断を行う(0:なし 1:仮設置 2:本設置 9:両方)
            If Not Me.dttTest.ppText = String.Empty Then '総合テスト日が空以外
                If Not Me.dttTemporaryInstallationDepartureDt.ppText = String.Empty Then '仮設置が空以外
                    If Me.dttTest.ppText _
                       + Me.tmtTestDepartureTm.ppHourText _
                       + Me.tmtTestDepartureTm.ppMinText _
                      <> _
                       Me.dttTemporaryInstallationDepartureDt.ppText _
                       + Me.tmtTemporaryInstallationDepartureTm.ppHourText _
                       + Me.tmtTemporaryInstallationDepartureTm.ppMinText Then '総合テスト日と仮設置日が異なっていたら

                        If ViewState(M_STE_TMP_DT) = "1" Then '仮設置から本設置になった場合
                            ViewState(M_STE_TMP_DT) = "8"   '仮設置から本設置
                        Else
                            ViewState(M_STE_TMP_DT) = "9"   '本設置/仮設置
                        End If

                    Else
                        If ViewState(M_STE_TMP_DT) = "1" Then '仮設置から本設置になった場合
                            ViewState(M_STE_TMP_DT) = "8"   '仮設置から本設置
                        Else
                            ViewState(M_STE_TMP_DT) = "3"   '本設置(仮本一致)
                        End If

                    End If
                Else
                    ViewState(M_STE_TMP_DT) = "2"   '本設置
                End If
            ElseIf Not Me.dttTemporaryInstallationDepartureDt.ppText = String.Empty Then
                ViewState(M_STE_TMP_DT) = "1"   '仮設置
            Else
                ViewState(M_STE_TMP_DT) = "0"   'なし
            End If

            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
                '仮依頼番号発番
                Dim intFlg As Integer = 0
                If clsDataConnect.pfOpen_Database(conDB) Then
                    Using cmdDB As SqlCommand = New SqlCommand("CNSUPDP001_U9", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        With cmdDB.Parameters
                            'パラメータ設定 
                            .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 5))                                    '管理番号 CNSUPDP001_007 1→5
                            .Add(pfSet_Param("YMD", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))         '年月日
                            '.Add(pfSet_Param("YMD", SqlDbType.NVarChar, "1"))                                  '年月日
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))                 'ユーザーＩＤ
                            .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))         '結果取得用
                        End With
                        cmdDB.ExecuteNonQuery()

                        If cmdDB.Parameters("SalesYTD").Value Is Nothing Then
                            strMes = "仮依頼番号発番 CNSUPDP001_U9"
                            Exit Try
                        Else
                            Me.ProvisionalRequestNo.ppTextTwo = cmdDB.Parameters("SalesYTD").Value.ToString.PadLeft(8, "0")
                            dtRow("D39_CNST_NO") = Me.ProvisionalRequestNo.ppTextOne & "-" & cmdDB.Parameters("SalesYTD").Value.ToString.PadLeft(8, "0")
                            ViewState("PRECNST_NO") = cmdDB.Parameters("SalesYTD").Value.ToString
                        End If
                    End Using
                Else
                    strMes = "仮依頼番号発番 DB接続"
                    Exit Try
                End If
            Else
                dtRow("D39_CNST_NO") = Me.lblRequestNo_2.Text                                               '工事依頼番号
            End If
            dtRow("D39_CNST_COM_NO") = Me.ttwNotificationNo.ppText                                      '工事通知番号

            dtRow("D39_TBOXID") = Me.txtTboxId.ppText                                                   'ＴＢＯＸＩＤ
            dtRow("D39_HALL_CD") = Me.txtHoleCd.ppText                                                  'ホールコード
            dtRow("D39_HALL_NM") = Me.txtHoleNm.ppText                                                  'ホール名
            dtRow("D39_H_RSP") = Me.txtPersonInCharge_1.ppText                                          'ホール責任者
            dtRow("D39_H_ADDR") = Me.txtAddress.ppText                                                  'ホール住所
            dtRow("D39_H_TELNO") = Me.txtHoleTelNo.ppText                                               'ホール電話番号
            dtRow("D39_H_TBOXCLASS") = Me.txtCurrentSys.ppText                                          '現行システム
            dtRow("D39_H_VERSION") = Me.txtVer.ppText                                                   '現行バージョン
            dtRow("D39_H_TBOX_TELNO") = Me.txtTboxLine.ppText                                           'ＴＢＯＸ回線番号
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then
                dtRow("D39_MTR_STATUS_CD") = "01"                                                        '案件ステータス(01:未処理)
            ElseIf ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
                dtRow("D39_MTR_STATUS_CD") = "03"                                                        '案件ステータス(03:作業依頼中)
            Else
                dtRow("D39_MTR_STATUS_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_MTR_STATUS_CD")        '案件ステータス
            End If

            '更新時は選択内容を案件ステータスを設定する
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                dtRow("D39_MTR_STATUS_CD") = Me.ddlMTRStatus.SelectedValue.ToString
            End If

            '受付確定時、各フラグによって案件ステータスを変更
            If Master.ppRigthButton3.Text = sCnsKakuteiButon Then
                'CNSUPDP001_008
                If Me.RadioButtonList1.Text = "1" Then
                    '処理結果:OK 
                    '[連絡区分(新規/変更/キャンセル)]で判定
                    If Me.txtSpecificationConnectingDivision.ppText = "3" Then
                        'キャンセル
                        dtRow("D39_MTR_STATUS_CD") = "12"   '処理完了
                    Else
                        '[FS稼働(有無)]で判定
                        If Me.txtSfOperation.ppText = "0" Then
                            'FS稼働有
                            dtRow("D39_MTR_STATUS_CD") = "03"   '作業依頼中
                        Else
                            'FS稼働無
                            dtRow("D39_MTR_STATUS_CD") = "12"   '処理完了
                        End If
                    End If
                Else
                    '処理結果:NG
                    dtRow("D39_MTR_STATUS_CD") = "12"   '処理完了
                End If
                ''キャンセルの場合は処理完了にする
                'If dtD39_DATA_SV.Tables(0).Rows(0).Item("D39_TELL_CLS").ToString <> "3" Then
                '    'FS稼動有無が有り 0　又はNG
                '    If Me.txtSfOperation.ppText = 0 And _
                '        Me.RadioButtonList1.Text <> "2" Then
                '        '----07/01 武
                '        '構成配信の工事依頼は現場作業待ちにする
                '        If Me.txtHoleConDelively.ppText = "0" Then
                '            '-------------
                '            dtRow("D39_MTR_STATUS_CD") = "03"   '作業依頼中
                '            '----07/01 武
                '        Else
                '            dtRow("D39_MTR_STATUS_CD") = "06"   '現場作業待ち
                '        End If
                '        '---------------
                '    Else
                '        dtRow("D39_MTR_STATUS_CD") = "12"   '処理完了
                '    End If
                'Else
                '    dtRow("D39_MTR_STATUS_CD") = "12"   '処理完了
                'CNSUPDP001_008 END
            ElseIf Master.ppRigthButton3.Text = "登録" Then
                '構成配信の工事依頼は現場作業待ちにする
                If Me.txtHoleConDelively.ppText = "0" Then
                    '-------------
                    dtRow("D39_MTR_STATUS_CD") = "03"   '作業依頼中
                    '----07/01 武
                Else
                    dtRow("D39_MTR_STATUS_CD") = "06"   '現場作業待ち
                End If
                '---------------
            End If

            '営業所で担当者、出発予定登録時は案件ステータスを「営業所受託」に変更する
            'If Session(P_SESSION_AUTH) = "営業所" Then
            If Me.ddlMTRStatus.SelectedValue = "03" And _
              Me.ddlPersonal1.Text <> String.Empty And _
              Me.dttTestDepartureDt.ppText <> String.Empty Then
                dtRow("D39_MTR_STATUS_CD") = "04"   '営業所受託
            End If
            'End If

            '進捗ステータスの設定方法
            Dim splitStatus() As String = Nothing
            Dim splittMpstts() As String = Nothing
            If Not lblStatusCd.Text = String.Empty Then
                splitStatus = Me.lblStatusCd.Text.Split(":")
            Else
                splitStatus = {"01"}
            End If
            If Not lbltMpsttsCd.Text = String.Empty Then
                splittMpstts = Me.lbltMpsttsCd.Text.Split(":")
            Else
                splittMpstts = {"01"}
            End If
            dtRow("D39_OTH_DTL") = Me.txtOtherContents.ppText                                           'その他工事内容

            'CNSUPDP001_022
            If Me.txtSfOperation.ppText.Trim = String.Empty Then
                Me.txtSfOperation.ppText = "0"
                Me.lblSfOperation.Text = "有り"
            End If
            dtRow("D39_FSWRK_CLS") = Me.txtSfOperation.ppText                                           'ＦＳ稼働有無
            'CNSUPDP001_022 END


            If Me.dttStartOfCon.ppText.Equals(String.Empty) Then
                dtRow("D39_CNST_DY") = DBNull.Value
            Else
                If Me.tmtStartOfCon.ppHourText.Equals(String.Empty) _
                    Or Me.tmtStartOfCon.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_CNST_DY") = dttStartOfCon.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttStartOfCon.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtStartOfCon.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtStartOfCon.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_CNST_DY") = strDateTime.ToString                                          '工事開始日時
                End If
            End If

            If Me.dttOpen.ppText.Equals(String.Empty) Then
                dtRow("D39_OPEN_DT") = DBNull.Value
            Else
                If Me.tmtOpen.ppHourText.Equals(String.Empty) _
                    Or Me.tmtOpen.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_OPEN_DT") = dttOpen.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttOpen.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtOpen.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtOpen.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_OPEN_DT") = strDateTime.ToString                                         'オープン日時
                End If
            End If
            If Me.dttPolice.ppText.Equals(String.Empty) Then
                dtRow("D39_PTEST_DT") = DBNull.Value
            Else
                If Me.tmtPolice.ppHourText.Equals(String.Empty) _
                    Or Me.tmtPolice.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_PTEST_DT") = dttPolice.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttPolice.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtPolice.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtPolice.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_PTEST_DT") = strDateTime.ToString                                        '警察検査日時
                End If
            End If
            If Me.dttTest.ppText.Equals(String.Empty) Then
                dtRow("D39_STEST_DT") = DBNull.Value
            Else
                If Me.tmtTest.ppHourText.Equals(String.Empty) _
                    Or Me.tmtTest.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_STEST_DT") = dttTest.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttTest.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtTest.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtTest.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_STEST_DT") = strDateTime.ToString                                        '総合テスト日時
                End If
            End If
            If Me.dttTemporaryInstallation.ppText.Equals(String.Empty) Then
                dtRow("D39_TMPSET_DT") = DBNull.Value
            Else
                If Me.tmtTemporaryInstallation.ppHourText.Equals(String.Empty) _
                    Or Me.tmtTemporaryInstallation.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_TMPSET_DT") = dttTemporaryInstallation.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttTemporaryInstallation.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtTemporaryInstallation.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtTemporaryInstallation.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_TMPSET_DT") = strDateTime.ToString                                       '仮設置日時
                End If
            End If

            dtRow("D39_AGENCY_CD") = Me.txtAgencyCd.ppText                                              '代理店コード
            dtRow("D39_AGENCY_NM") = Me.trfAgencyNm.ppText                                              '代理店名
            dtRow("D39_A_ADDR") = Me.txtAgencyAdd.ppText                                                '代理店住所
            dtRow("D39_A_TCHARGE") = Me.txtAgencyResponsible.ppText                                     '代理店責任者
            dtRow("D39_A_CHARGE") = Me.txtAgencyPersonnel.ppText                                        '代理店担当者
            dtRow("D39_A_TELNO") = Me.txtAgencyTel.ppText                                               '代理店電話番号

            dtRow("D39_REPSHOP_CD") = Me.txtAgencyShop.ppText                                           '代行店コード
            dtRow("D39_REPSHOP_NM") = Me.trfAgencyShop.ppText                                           '代行店名
            dtRow("D39_R_ADDR") = Me.txtAgencyShopAdd.ppText                                            '代行店住所
            dtRow("D39_R_CHARGE") = Me.txtAgencyShopPersonnel.ppText                                    '代行店担当者
            dtRow("D39_R_TELNO") = Me.txtAgencyShopTelNo.ppText                                         '代行店電話番号

            dtRow("D39_NOTETEXT") = Me.txtRemarks.ppText                                                '備考

            If lblReceptionDt_2.Text.ToString = String.Empty Then
                dtRow("D39_DATARCV_DT") = DBNull.Value                                                  'データ受信日時
            Else
                dtRow("D39_DATARCV_DT") = lblReceptionDt_2.Text & " " & lblReceptionTm_2.Text
            End If
            dtRow("D39_DELETE_FLG") = "0"                                                               '削除フラグ
            dtRow("D39_CHECKB_FLG") = "0"                                                               '検収書ＵＰＬＯＡＤ

            dtRow("D39_TWIN_CD") = Me.txtTwinsShop.ppText                                               '双子店フラグ
            dtRow("D39_PC_FLG") = Me.txtPAndCDivision.ppText                                            '親子フラグ
            dtRow("D39_NL_CLS") = Me.txtNLDivision.ppText                                               'ＮＬ区分
            dtRow("D39_SYSTEM_GRP") = Me.txtSysClassification.ppText                                    'システム分類
            dtRow("D39_TELL_CLS") = Me.txtSpecificationConnectingDivision.ppText                        '仕様連絡区分

            dtRow("D39_H_CHARGE") = Me.txtPersonInCharge_2.ppText                                       'ホール担当者
            '案件終了フラグの設定
            If Me.txtSfOperation.ppText.ToString = "0" Or _
                Me.txtSfOperation.ppText.ToString = "" Then 'FS稼動有無が有り 0
                Select Case ViewState(M_STE_TMP_DT)
                    Case "0" 'なし
                        dtRow("D39_CNST_END_FLG") = "0"                                                     '案件終了フラグ
                    Case "1" '仮設置
                        dtRow("D39_CNST_END_FLG") = "0"                                                     '案件終了フラグ
                    Case "2", "3", "8", "9" '本設置,仮本設置一致,仮設置から本設置へ変更,両方
                        If splitStatus(0) = "12" Then
                            dtRow("D39_CNST_END_FLG") = "1"                                                 '案件終了フラグ
                        Else
                            dtRow("D39_CNST_END_FLG") = "0"                                                 '案件終了フラグ
                        End If
                End Select
            Else ' 0以外
                dtRow("D39_CNST_END_FLG") = "0"
            End If
            dtRow("D39_BACK_CLS") = Me.txtTboxTakeawayDivision.ppText                                   'TBOX持帰区分
            dtRow("D39_EMNY_CLS") = Me.txtEMoneyIntroduction.ppText                                     'Eマネー導入区分
            dtRow("D39_EMNY_CNST_CLS") = Me.txtEMoneyIntroductionCns.ppText                             'Eマネー導入工事区分
            If Me.dttEMoneyTestDt.ppText.Equals(String.Empty) Then
                dtRow("D39_EMNY_DT") = DBNull.Value
            ElseIf Me.tmtEMoneyTestTm.ppHourText = String.Empty _
            OrElse Me.tmtEMoneyTestTm.ppMinText = String.Empty Then
                dtRow("D39_EMNY_DT") = Me.dttEMoneyTestDt.ppText
            Else
                strDateTime = New StringBuilder
                strDateTime.Append(Me.dttEMoneyTestDt.ppText)
                strDateTime.Append(" ")
                strDateTime.Append(Me.tmtEMoneyTestTm.ppHourText)
                strDateTime.Append(":")
                strDateTime.Append(Me.tmtEMoneyTestTm.ppMinText)
                strDateTime.Append(":00")
                dtRow("D39_EMNY_DT") = strDateTime.ToString                                         'Eマネーテスト日時
            End If
            dtRow("D39_PR_HALL_CD") = Me.txtParentHoleCd.ppText                                         '親ホールコード
            dtRow("D39_IND_CNST_CLS") = Me.txtIndependentCns.ppText                                     '単独工事区分
            dtRow("D39_SAME_CNST_CNT") = Me.txtSameTimeCnsNo.ppText                                     '同時工事数
            '--------------------------------
            '2014/06/20　武　ここから
            '--------------------------------
            If Me.lblReqState.Text = "" Then
                dtRow("D39_REQ_STATE") = Me.lblReqState.Text
            Else
                dtRow("D39_REQ_STATE") = Me.lblReqState.Text.Substring(0, 1)                                               '資料請求状況
            End If
            '--------------------------------
            '2014/06/20　武　ここまで
            '--------------------------------
            If Me.dttLastOpenDt.ppText.Equals(String.Empty) Then
                dtRow("D39_LAST_DT") = DBNull.Value
            Else
                dtRow("D39_LAST_DT") = Me.dttLastOpenDt.ppText                                          '最終営業日
            End If

            If Me.dttLastOpenDtT500.ppText.Equals(String.Empty) Then
                dtRow("D39_T500_LAST_DT") = DBNull.Value
            Else
                dtRow("D39_T500_LAST_DT") = Me.dttLastOpenDtT500.ppText                                  '最終営業日_T500
            End If
            dtRow("D39_TMPSET_CNST_CLS") = Me.dttTemporaryInstallationCnsDivision.ppText                '仮設置工事区分
            dtRow("D39_TMPSET_UNINP_CLS") = Me.dttTemporaryInstallationDtNotInputDivision0.ppText       '仮設置日未入力区分
            dtRow("D39_MOVE_CNST_CLS") = Me.dttShiftCnsDivision.ppText                                  '移行工事区分
            dtRow("D39_MOVE_CNST_WORK_CLS") = Me.dttShiftCnsWorkDivision.ppText                         '移行工事作業区分
            If Me.dttLanCns.ppText.Equals(String.Empty) Then
                dtRow("D39_LAN_CNST_DT") = DBNull.Value
            Else
                If Me.tmtLanCns.ppHourText.Equals(String.Empty) _
                    Or Me.tmtLanCns.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_LAN_CNST_DT") = dttLanCns.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttLanCns.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtLanCns.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtLanCns.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_LAN_CNST_DT") = strDateTime.ToString                                      'LAN工事日時
                End If
            End If
            If Me.dttVerup.ppText.Equals(String.Empty) Then
                dtRow("D39_VERUP_DT") = DBNull.Value
            Else
                If Me.tmtVerup.ppHourText.Equals(String.Empty) _
                    Or Me.tmtVerup.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_VERUP_DT") = dttVerup.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttVerup.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtVerup.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtVerup.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_VERUP_DT") = strDateTime.ToString                                        'バージョンアップ日時
                End If
            End If
            dtRow("D39_VERUP_DT_CLS") = Me.dttVerupDtDivision.ppText                                    'バージョンアップ日付区分
            dtRow("D39_KIND_VERUP1") = Me.txtVerupCnsType_1.ppText                                      'バージョンアップ種類１
            dtRow("D39_KIND_VERUP2") = Me.txtVerupCnsType_2.ppText                                      'バージョンアップ種類２
            dtRow("D39_R_TCHARGE") = Me.txtAgencyShopResponsible.ppText                                 '代行店責任者
            dtRow("D39_LAN_SEND_NM") = Me.trfSendingStation.ppText                                      'LAN送付先名
            dtRow("D39_LAN_SEND_ADDR") = Me.txtSendingStationAdd.ppText                                 'LAN送付先住所
            dtRow("D39_LAN_SEND_TELNO") = Me.txtSendingStationTelNo.ppText                              'LAN送付先電話番号
            If Me.tdtDeliveryPreferredDt.ppText.Equals(String.Empty) Then
                dtRow("D39_LAN_SEND_DT") = DBNull.Value
            Else
                If Me.ClsCMTimeBox1.ppHourText.Equals(String.Empty) _
                   Or Me.ClsCMTimeBox1.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_LAN_SEND_DT") = tdtDeliveryPreferredDt.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.tdtDeliveryPreferredDt.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.ClsCMTimeBox1.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.ClsCMTimeBox1.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_LAN_SEND_DT") = strDateTime.ToString                                     'LAN納入希望日
                End If
            End If
            dtRow("D39_TELL_MAT") = Me.txtSpecificationsRemarks.ppText                                  '仕様備考

            dtRow("D39_RECEIVE_CNT") = Me.lblNumberOfUpdates.Text                                       '受信回数
            dtRow("D39_H_NEW") = Me.txtHoleNew.ppText                                                   'ホール内工事新規
            dtRow("D39_H_ADD") = Me.txtHoleExpansion.ppText                                             'ホール内工事増設
            dtRow("D39_H_PRT_REMOVE") = Me.txtHoleSomeRemoval.ppText                                    'ホール内工事一部撤去
            dtRow("D39_H_RELOCATE") = Me.txtHoleShopRelocation.ppText                                   'ホール内工事店内移設
            dtRow("D39_H_ALL_REMOVE") = Me.txtHoleAllRemoval.ppText                                     'ホール内工事全撤去
            dtRow("D39_H_TMP_REMOVE") = Me.txtHoleOnceRemoval.ppText                                    'ホール内工事一時撤去
            dtRow("D39_H_RESET") = Me.txtHoleReInstallation.ppText                                      'ホール内工事再設置
            dtRow("D39_H_CHNG_ORGNZ") = Me.txtHoleConChange.ppText                                      'ホール内工事構成変更
            dtRow("D39_H_DLV_ORGNZ") = Me.txtHoleConDelively.ppText                                     'ホール内工事構成配信
            dtRow("D39_H_OTH") = Me.txtHoleOther.ppText                                                 'ホール内工事その他
            dtRow("D39_L_NEW") = Me.txtLanNew.ppText                                                    'ＬＡＮ工事新規
            dtRow("D39_L_ADD") = Me.txtLanExpansion.ppText                                              'ＬＡＮ工事増設
            dtRow("D39_L_PRT_REMOVE") = Me.txtLanSomeRemoval.ppText                                     'ＬＡＮ工事一部撤去
            dtRow("D39_L_RELOCATE") = Me.txtLanShopRelocation.ppText                                    'ＬＡＮ工事店内移設
            dtRow("D39_L_ALL_REMOVE") = Me.txtLanAllRemoval.ppText                                      'ＬＡＮ工事全撤去
            dtRow("D39_L_TMP_REMOVE") = Me.txtLanOnceRemoval.ppText                                     'ＬＡＮ工事一時撤去
            dtRow("D39_L_RESET") = Me.txtLanReInstallation.ppText                                       'ＬＡＮ工事再設置
            dtRow("D39_L_CHNG_ORGNZ") = Me.txtLanConChange.ppText                                       'ＬＡＮ工事構成変更
            dtRow("D39_L_DLV_ORGNZ") = Me.txtLanConDelively.ppText                                      'ＬＡＮ工事構成配信
            dtRow("D39_L_OTH") = Me.txtLanOther.ppText                                                  'ＬＡＮ工事その他

            dtRow("D39_STEST_CHARGE_CD") = Me.ddlPersonal1.SelectedValue.ToString                       '総合テスト作業担当者の社員コード
            If dtRow("D39_STEST_CHARGE_CD") = "" Then
                dtRow("D39_STEST_CHARGE") = ""                                                          '総合テスト作業担当者
            Else
                Dim selectItem1 As String = Me.ddlPersonal1.SelectedItem.ToString
                Dim name1 As String() = selectItem1.Split(":")
                dtRow("D39_STEST_CHARGE") = name1(1)                                                    '総合テスト作業担当者
            End If

            If Me.dttTestDepartureDt.ppText.Equals(String.Empty) Then
                dtRow("D39_STESTDEPT_DT") = DBNull.Value
            Else
                If Me.tmtTestDepartureTm.ppHourText.Equals(String.Empty) _
                  Or Me.tmtTestDepartureTm.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_STESTDEPT_DT") = dttTestDepartureDt.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttTestDepartureDt.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtTestDepartureTm.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtTestDepartureTm.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_STESTDEPT_DT") = strDateTime.ToString                                    '総合テスト出発日時
                End If
            End If

            dtRow("D39_TMPSET_CHARGE_CD") = Me.ddlPersonal2.SelectedValue.ToString                    '仮設置作業担当者の社員コード
            If dtRow("D39_TMPSET_CHARGE_CD") = "" Then
                dtRow("D39_TMPSET_CHARGE") = ""                                                       '仮設置作業担当者
            Else
                Dim selectItem2 As String = Me.ddlPersonal2.SelectedItem.ToString
                Dim name2 As String() = selectItem2.Split(":")
                dtRow("D39_TMPSET_CHARGE") = name2(1)                                                 '仮設置作業担当者
            End If

            If Me.dttTemporaryInstallationDepartureDt.ppText.Equals(String.Empty) Then
                dtRow("D39_TMPSETDEPT_DT") = DBNull.Value
            Else
                If Me.tmtTemporaryInstallationDepartureTm.ppHourText.Equals(String.Empty) _
                  Or Me.tmtTemporaryInstallationDepartureTm.ppMinText.Equals(String.Empty) Then
                    dtRow("D39_TMPSETDEPT_DT") = dttTemporaryInstallationDepartureDt.ppText
                Else
                    strDateTime = New StringBuilder
                    strDateTime.Append(Me.dttTemporaryInstallationDepartureDt.ppText)
                    strDateTime.Append(" ")
                    strDateTime.Append(Me.tmtTemporaryInstallationDepartureTm.ppHourText)
                    strDateTime.Append(":")
                    strDateTime.Append(Me.tmtTemporaryInstallationDepartureTm.ppMinText)
                    strDateTime.Append(":00")
                    dtRow("D39_TMPSETDEPT_DT") = strDateTime.ToString                                  '仮設置出発日時
                End If
            End If

            dtRow("D39_F1_CNST_CLS") = Me.txtConstructionExistenceF1.ppText                             'Ｆ１工事有無
            dtRow("D39_F2_CNST_CLS") = Me.txtConstructionExistenceF2.ppText                             'Ｆ２工事有無
            dtRow("D39_F3_CNST_CLS") = Me.txtConstructionExistenceF3.ppText                             'Ｆ３工事有無
            dtRow("D39_F4_CNST_CLS") = Me.txtConstructionExistenceF4.ppText                             'Ｆ４工事有無
            dtRow("D39_LAN_SEND_CD") = txtSendingStation.ppText                                         'ＬＡＮ送付先コード
            dtRow("D39_LAN_SEND_TCHARGE") = Me.txtSendingStationResponsible.ppText                      'ＬＡＮ送付先責任者

            dtRow("D39_POINT1") = Me.txtImportantPoints1.ppText                                         '注意事項１
            dtRow("D39_POINT2") = Me.txtImportantPoints2.ppText                                         '連絡事項
            'dtRow("D39_POINT2") = Me.txtImportantPoints2.ppText                                         '注意事項２
            If ViewState(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.更新 Then
                dtRow("D39_CNCT_FLG") = "0"                                                                 '紐付けフラグ
            Else
                dtRow("D39_CNCT_FLG") = dtD39_DATA_SV.Tables(0).Rows(0).Item("D39_CNCT_FLG").ToString
            End If
            If cbxDllSettingChange.Checked.Equals(True) Then
                dtRow("D39_DLL_CLS") = "1"                                                              'ＤＬＬ設定変更なし
            Else
                dtRow("D39_DLL_CLS") = "0"                                                              'ＤＬＬ設定変更あり
            End If
            dtRow("D39_NL_CLS") = txtNLDivision.ppText                                                  'ＮＬ区分
            dtRow("D39_H_TBOXCLASS") = txtCurrentSys.ppText                                             '現行システム
            dtRow("D39_BRANCH_CD") = Me.txtOfficCD.ppText                                                '担当営業所コード
            dtRow("D39_BRANCH_NM") = Me.lblSendNmV.Text                                                  '担当営業所コード
            dtRow("D39_SYSSTK_CLS") = Me.txtStkHoleIn.ppText                                             'システムストッカ設置場所内
            If Me.txtStkHoleIn.ppText Is DBNull.Value Then
                Me.lblStkHoleIn.Text = ""
            ElseIf Me.txtStkHoleIn.ppText = "" Then
                Me.lblStkHoleIn.Text = ""
            ElseIf Me.txtStkHoleIn.ppText = "0" Then
                Me.lblStkHoleIn.Text = ""
            ElseIf Me.txtStkHoleIn.ppText = "1" Then
                Me.lblStkHoleIn.Text = "該当"
            End If
            dtRow("D39_SYSSTK_OUT") = Me.txtStkHoleOut.ppText                                            'システムストッカ設置場所外
            If Me.txtStkHoleOut.ppText Is DBNull.Value Then
                Me.lblStkHoleOut.Text = ""
            ElseIf Me.txtStkHoleOut.ppText = "" Then
                Me.lblStkHoleOut.Text = ""
            ElseIf Me.txtStkHoleOut.ppText = "0" Then
                Me.lblStkHoleOut.Text = ""
            ElseIf Me.txtStkHoleOut.ppText = "1" Then
                Me.lblStkHoleOut.Text = "該当"
            End If
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
                If ViewState("TBOXCLC") Is Nothing OrElse ViewState("TBOXCLC") = String.Empty Then
                    dtRow("D39_TBXCLS_CD") = DBNull.Value
                Else
                    dtRow("D39_TBXCLS_CD") = ViewState("TBOXCLC")
                End If
            Else
                dtRow("D39_TBXCLS_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("T03_SYSTEM_CD")
            End If


            'If ViewState(P_SESSION_TERMS) =  ClsComVer.E_遷移条件.登録 _
            '    Or ViewState(P_SESSION_TERMS) =  ClsComVer.E_遷移条件.仮登録 Then
            If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
                dtRow("D39_H_ZIPNO") = DBNull.Value                                                         'ホール郵便番号
                dtRow("D39_SYSSTK_CLS") = DBNull.Value                                                      'システムストッカ設置場所内
                dtRow("D39_SYSSTK_OUT") = DBNull.Value                                                      'システムストッカ設置場所外
                dtRow("D39_A_ZIPNO") = DBNull.Value                                                         '代理店郵便番号
                dtRow("D39_R_ZIPNO") = DBNull.Value                                                         '代行店郵便番号
                dtRow("D39_IND_DDXP") = DBNull.Value                                                        'ＩＮＳ　ＤＤＸＰ
                dtRow("D39_INS_LCGN") = DBNull.Value                                                        'ＩＮＳ　ＬＣＧＮ
                dtRow("D39_INS_LCN") = DBNull.Value                                                         'ＩＮＳ　ＬＣＮ
                dtRow("D39_CARDCNVY_CLS") = DBNull.Value                                                    'カード搬送装置有無
                dtRow("D39_ALLOPN_CLS") = DBNull.Value                                                      '一斉解除装置有無
                dtRow("D39_MONYCNVY_CLS") = DBNull.Value                                                    '紙幣搬出装置有無
                dtRow("D39_MONYCNVYMK_CD") = DBNull.Value                                                   '紙幣搬出装置メーカコード
                dtRow("D39_MONYCNVYMK_NM") = DBNull.Value                                                   '紙幣搬出装置メーカ名
                dtRow("D39_CNSTCHARGE_TELNO") = DBNull.Value                                                '工事担当者電話番号
                dtRow("D39_CNSTCHARGE_CD") = DBNull.Value                                                   '責任者コード
                dtRow("D39_FSCHARGE_CD") = DBNull.Value                                                     '担当者コード
                dtRow("D39_FSRCPT_CD") = DBNull.Value                                                       '受付者コード
                dtRow("D39_CNSTCHARGE") = DBNull.Value                                                      '工事担当者
                dtRow("D39_PRECNST_NO") = DBNull.Value                                                      '仮受付番号
                dtRow("D39_CALL_FLG") = DBNull.Value                                                        '立会フラグ
                dtRow("D39_VERUP_CD") = DBNull.Value                                                        'バージョンアップコード
                dtRow("D39_TBOXCLS_NM") = DBNull.Value                                                      'ＴＢＯＸ機種
                dtRow("D39_TELL_SRC_CLS") = DBNull.Value                                                    '工事連絡元区分
                dtRow("D39_H_STATE_CD") = DBNull.Value                                                      'ホール県コード
                dtRow("D39_SYSTEM_CLS") = DBNull.Value                                                      'システム区分
                dtRow("D39_CSTSRV_TCHARGE") = DBNull.Value                                                  'カスタマーサービス部責任者
                dtRow("D39_CSTSRV_CHARGE") = DBNull.Value                                                   'カスタマーサービス部担当者
                dtRow("D39_CNST_ALL") = DBNull.Value                                                        '発生工事全
                dtRow("D39_CNST_DELAY") = DBNull.Value                                                      '工事遅延
                dtRow("D39_USE_SPARE") = DBNull.Value                                                       '予備機使用
                dtRow("D39_VAIN") = DBNull.Value                                                            '空振り
                dtRow("D39_INS") = DBNull.Value                                                             'ＩＮＳ
                dtRow("D39_TEL") = DBNull.Value                                                             'ＴＥＬ
                dtRow("D39_OTH") = DBNull.Value                                                             'その他
                dtRow("D39_ORGNZ") = DBNull.Value                                                           '構成
                dtRow("D39_LAN_SEND_ZIPNO") = DBNull.Value                                                  'ＬＡＮ送付先郵便番号
                dtRow("D39_TR_SEQ") = DBNull.Value                                                          '業者連番
                dtRow("D39_SET_DT") = DBNull.Value                                                          '本設置日時

                dtRow("D39_H_TBOXCLASS_CD") = DBNull.Value                                                  '現行システムコード
                dtRow("D39_TMPSTTS_CD") = DBNull.Value                                                      '仮設置進捗ステータス
                dtRow("D39_STATUS_CD") = DBNull.Value                                                       '本設置進捗ステータス
                dtRow("D39_NJ_CLS") = DBNull.Value                                                          'ＮＪ区分
                dtRow("D39_REQ_CLS") = DBNull.Value                                                         '資料請求区分

                dtRow("D39_INSERT_DT") = Date.Now                                                           '登録日時
                dtRow("D39_INSERT_USR") = Session(P_SESSION_USERID).ToString                                '登録者
                dtRow("D39_UPDATE_DT") = DBNull.Value                                                       '更新日時
                dtRow("D39_UPDATE_USR") = DBNull.Value                                                      '更新者
            ElseIf ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                dtRow("D39_H_ZIPNO") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_H_ZIPNO")                                                        'ホール郵便番号
                'CNSUPDP001_003
                'dtRow("D39_DLL_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_DLL_CLS")                                                        'ＤＬＬ設定変更なし
                'CNSUPDP001_003 END
                dtRow("D39_A_ZIPNO") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_A_ZIPNO")                                                        '代理店郵便番号
                dtRow("D39_R_ZIPNO") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_R_ZIPNO")                                                        '代行店郵便番号
                dtRow("D39_IND_DDXP") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_IND_DDXP")                                                       'ＩＮＳ　ＤＤＸＰ
                dtRow("D39_INS_LCGN") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_INS_LCGN")                                                       'ＩＮＳ　ＬＣＧＮ
                dtRow("D39_INS_LCN") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_INS_LCN")                                                        'ＩＮＳ　ＬＣＮ
                dtRow("D39_CARDCNVY_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CARDCNVY_CLS")                                                   'カード搬送装置有無
                dtRow("D39_ALLOPN_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_ALLOPN_CLS")                                                     '一斉解除装置有無
                dtRow("D39_MONYCNVY_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_MONYCNVY_CLS")                                                   '紙幣搬出装置有無
                dtRow("D39_MONYCNVYMK_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_MONYCNVYMK_CD")                                                  '紙幣搬出装置メーカコード
                dtRow("D39_MONYCNVYMK_NM") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_MONYCNVYMK_NM")                                                  '紙幣搬出装置メーカ名
                dtRow("D39_CNSTCHARGE_TELNO") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CNSTCHARGE_TELNO")                                               '工事担当者電話番号
                dtRow("D39_CNSTCHARGE_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CNSTCHARGE_CD")                                                  '責任者コード
                dtRow("D39_FSCHARGE_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_FSCHARGE_CD")                                                    '担当者コード
                dtRow("D39_FSRCPT_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_FSRCPT_CD")                                                      '受付者コード
                dtRow("D39_CNSTCHARGE") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CNSTCHARGE")                                                     '工事担当者
                dtRow("D39_PRECNST_NO") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_PRECNST_NO")                                                     '仮受付番号
                dtRow("D39_CALL_FLG") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CALL_FLG")                                                       '立会フラグ
                dtRow("D39_VERUP_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_VERUP_CD")                                                       'バージョンアップコード
                dtRow("D39_TBOXCLS_NM") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_TBOXCLS_NM")                                                     'ＴＢＯＸ機種
                dtRow("D39_TELL_SRC_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_TELL_SRC_CLS")                                                   '工事連絡元区分
                dtRow("D39_H_STATE_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_H_STATE_CD")                                                     'ホール県コード
                dtRow("D39_SYSTEM_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_CLS")                                                     'システム区分
                dtRow("D39_CSTSRV_TCHARGE") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CSTSRV_TCHARGE")                                                 'カスタマーサービス部責任者
                dtRow("D39_CSTSRV_CHARGE") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CSTSRV_CHARGE")                                                  'カスタマーサービス部担当者
                dtRow("D39_CNST_ALL") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CNST_ALL")                                                     '発生工事全
                dtRow("D39_CNST_DELAY") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_CNST_DELAY")                                                     '工事遅延
                dtRow("D39_USE_SPARE") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_USE_SPARE")                                                      '予備機使用
                dtRow("D39_VAIN") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_VAIN")                                                           '空振り
                dtRow("D39_INS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_INS")                                                            'ＩＮＳ
                dtRow("D39_TEL") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_TEL")                                                          'ＴＥＬ
                dtRow("D39_OTH") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_OTH")                                                            'その他
                dtRow("D39_ORGNZ") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_ORGNZ")                                                          '構成
                dtRow("D39_LAN_SEND_ZIPNO") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_LAN_SEND_ZIPNO")                                                   'ＬＡＮ送付先郵便番号
                dtRow("D39_TR_SEQ") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_TR_SEQ")                                                         '業者連番
                If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SET_DT").ToString = String.Empty Then 'CNSUPDP001_009
                    dtRow("D39_SET_DT") = DBNull.Value
                Else
                    dtRow("D39_SET_DT") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_SET_DT")                                                          '本設置日時
                End If
                dtRow("D39_H_TBOXCLASS_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_H_TBOXCLASS_CD")     '現行システムコード
                dtRow("D39_TMPSTTS_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_TMPSTTS_CD")             '仮設置進捗ステータス
                dtRow("D39_STATUS_CD") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_STATUS_CD")               '本設置進捗ステータス
                dtRow("D39_NJ_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_NJ_CLS")                     'ＮＪ区分
                dtRow("D39_REQ_CLS") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_REQ_CLS")                   '資料請求区分

                dtRow("D39_INSERT_DT") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_INSERT_DT")                '登録日時
                dtRow("D39_INSERT_USR") = dtD39_DATA_SV.Tables(0).Rows(0)("D39_INSERT_USR")              '登録者
                dtRow("D39_UPDATE_DT") = Date.Now                                                        '更新日時
                dtRow("D39_UPDATE_USR") = Session(P_SESSION_USERID).ToString                             '更新者
            Else
                dtRow("D39_INSERT_DT") = Date.Now                                                        '登録日時
                dtRow("D39_INSERT_USR") = Session(P_SESSION_USERID).ToString                             '登録者
            End If
            dtRow("D39_VERUPCNST_CD") = DBNull.Value                                                    'ＶｅｒＵｐ工事種類コード 

            dataset.Tables(0).Rows.Add(dtRow)
            CNSUPDP001_D39_CNSTREQSPEC = dataset

            '成功
            msSet_D39_CNSTREQSPEC_Edit = True

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            'ログ出力
            If Not strMes Is Nothing Then
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", strMes & " 失敗", )
            End If
        End Try

    End Function

#End Region

#Region "工事依頼書兼仕様書印刷処理"

    ''' <summary>
    ''' 工事依頼書兼仕様書印刷処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_PrinT()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Dim dtD39_DATA_SV As New DataSet
        Dim dstOrders_1 As New DataSet
        Dim dstOrders_2 As New DataSet
        Dim dstOrders_3 As New DataSet
        Dim dstOrders_4 As New DataSet

        Dim rpt() As Object = Nothing
        Dim strSql() As String = Nothing
        Dim strFileName() As String = Nothing
        objStack = New StackFrame

        Try

            dtD39_DATA_SV = ViewState("D39_DATA_SV")

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Return False
            End If
            If Me.ddlMTRStatus.SelectedItem.Text.ToString <> "01:未処理" Then
                If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then      '１：ＩＤ
                    strIDIC = "1"
                    'パラメータ設定
                    cmdDB = New SqlCommand(sCnsSqlid_S12, conDB)
                Else
                    strIDIC = "3"
                    'パラメータ設定
                    cmdDB = New SqlCommand(sCnsSqlid_S11, conDB)
                End If
            Else
                If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then
                    strIDIC = "1"
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSUPDP001_S21", conDB)
                Else
                    strIDIC = "3"
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSUPDP001_S20", conDB)
                End If
            End If
            With cmdDB.Parameters
                .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
            End With

            dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)

            If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then
                If Me.ddlMTRStatus.SelectedItem.Text.ToString <> "01:未処理" Then
                    If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then      '１：ＩＤ
                        'パラメータ設定
                        cmdDB = New SqlCommand("CNSUPDP001_S23", conDB)
                    End If
                Else
                    If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then
                        'パラメータ設定
                        cmdDB = New SqlCommand("CNSUPDP001_S22", conDB)
                    End If
                End If

                With cmdDB.Parameters
                    .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, Me.lblRequestNo_2.Text))
                End With

                dstOrders_4 = clsDataConnect.pfGet_DataSet(cmdDB)
            End If
            '工事依頼書兼仕様書印刷編集処理
            msSet_Print_Edit(dstOrders_1)
            'CNSUPDP001_015
            Call sAddSPCName(dstOrders_1, conDB, cmdDB)
            'CNSUPDP001_015

            If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then
                msSet_Print_Edit2(dstOrders_4)
                'CNSUPDP001_015
                Call sAddSPCName(dstOrders_4, conDB, cmdDB)
                'CNSUPDP001_015
            End If
            strFileName = {"工事依頼書兼仕様書", "工事依頼書兼仕様書"}

            If dtD39_DATA_SV.Tables(0).Rows(0)("D39_SYSTEM_GRP").ToString.Equals("1") Then      '１：ＩＤ

                'CNSUPDP001_019
                '[印刷区分]フラグ有無の確認
                Session("印刷区分") = "0"
                For Each gr As GridViewRow In Me.grvList.Rows
                    If CType(gr.FindControl("印刷区分"), Label).Text.Trim = "1" Then
                        Session("印刷区分") = "1"
                    End If
                Next
                'CNSUPDP001_019 END

                rpt = {New CNSREP019, New CNSREP020}

                '帳票を出力する
                psPrintPDF(Me, rpt, {dstOrders_1.Tables(0), dstOrders_4.Tables(0)}, strFileName)
                'End If
            Else
                rpt = {New CNSREP001}

                '帳票を出力する
                psPrintPDF(Me, rpt(0), dstOrders_1.Tables(0), strFileName(0))
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "0005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.grvList.DataBind()
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

        Return True

    End Function

#End Region

#Region "工事依頼書兼仕様書印刷編集処理"

    ''' <summary>
    ''' 工事依頼書兼仕様書印刷編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Print_Edit(ByRef dstOrders_1 As DataSet)

        Dim dstOrders_Edit As New DataSet

        Dim intSys As String = 0
        Dim strNo() As String = {"１", "２", "３", "４", "５", "６", "７", "８", "９", "１０"}
        Dim strName As StringBuilder = New StringBuilder
        Dim dtRptSource As DataTable = dstOrders_1.Tables(1).Copy
        Dim dctRowCnt As New Dictionary(Of String, Integer())   '工事機器種別 表示件数 カウンタ
        Dim dctAPPA As New Dictionary(Of String, String)
        Dim strItem(13, 1) As String

        dstOrders_Edit = dstOrders_1.Copy

        dctAPPA.Add("01", "カードユニット")
        dctAPPA.Add("02", "玉貸機")
        dctAPPA.Add("03", "メダル貸機")
        dctAPPA.Add("04", "券売機")
        dctAPPA.Add("05", "券売機")
        dctAPPA.Add("06", "システムストッカー")
        dctAPPA.Add("07", "ディスプレイ")
        dctAPPA.Add("08", "電源")
        dctAPPA.Add("09", "接続ﾎﾞｯｸｽ")
        dctAPPA.Add("10", "紙幣搬送装置")
        dctAPPA.Add("12", "カード受付機")
        dctAPPA.Add("13", "TBOXラック")
        dctAPPA.Add("14", "OAタップ")
        dctAPPA.Add("15", "ＬＡＮケーブル")
        dctAPPA.Add(String.Empty, "その他")

        If strIDIC = "1" Then
            dctRowCnt.Add("01", {7, 0})
            dctRowCnt.Add("02", {4, 0})
            dctRowCnt.Add("03", {1, 0})
            dctRowCnt.Add("04", {6, 0})
            dctRowCnt.Add("05", {6, 0})
            dctRowCnt.Add("06", {2, 0})
            dctRowCnt.Add("07", {1, 0})
            dctRowCnt.Add("08", {Nothing, 0})
            dctRowCnt.Add("09", {Nothing, 0})
            dctRowCnt.Add("10", {Nothing, 0})
            dctRowCnt.Add("12", {Nothing, 0})
            dctRowCnt.Add("13", {Nothing, 0})
            dctRowCnt.Add("14", {Nothing, 0})
            dctRowCnt.Add("15", {Nothing, 0})
            dctRowCnt.Add(String.Empty, {4, 0})
        Else
            dctRowCnt.Add("01", {8, 0})
            dctRowCnt.Add("02", {2, 0})
            dctRowCnt.Add("03", {10, 0})
            dctRowCnt.Add("04", {3, 0})
            dctRowCnt.Add("05", {3, 0})
            dctRowCnt.Add("06", {2, 0})
            dctRowCnt.Add("07", {1, 0})
            dctRowCnt.Add("08", {1, 0})
            dctRowCnt.Add("09", {1, 0})
            dctRowCnt.Add("10", {2, 0})
            dctRowCnt.Add("12", {2, 0})
            dctRowCnt.Add("13", {2, 0})
            dctRowCnt.Add("14", {1, 0})
            dctRowCnt.Add("15", {6, 0})
            dctRowCnt.Add(String.Empty, {10, 0})
        End If

        strItem(0, 0) = "機器名"
        strItem(1, 0) = "工事前稼動"
        strItem(2, 0) = "工事前予備"
        strItem(3, 0) = "工事前総合"
        strItem(4, 0) = "店内移設"
        strItem(5, 0) = "撤去対象"
        strItem(6, 0) = "新品"
        strItem(7, 0) = "店内在庫自社"
        strItem(8, 0) = "店内在庫他社"
        strItem(9, 0) = "譲与品"
        strItem(10, 0) = "代理在庫"
        strItem(11, 0) = "工事後稼動"
        strItem(12, 0) = "工事後予備"
        strItem(13, 0) = "工事後総合"
        strItem(0, 1) = "D40_APPA"
        strItem(1, 1) = "D40_CNSTB_WRKCNT"
        strItem(2, 1) = "D40_CNSTB_STCCNT"
        strItem(3, 1) = "D40_CNSTB_ALLCNT"
        strItem(4, 1) = "D40_CNSTN_MOVCNT"
        strItem(5, 1) = "D40_CNSTN_REVCNT"
        strItem(6, 1) = "D40_CNSTN_NEWCNT"
        strItem(7, 1) = "D40_CNSRN_STCCNT"
        strItem(8, 1) = "D40_CNSTN_OSTCCNT"
        strItem(9, 1) = "D40_CNSTN_TRNCNT"
        strItem(10, 1) = "D40_CNSTN_AGCCNT"
        strItem(11, 1) = "D40_CNSTA_WRKCNT"
        strItem(12, 1) = "D40_CNSTA_STCCNT"
        strItem(13, 1) = "D40_CNSTA_ALLCNT"

        Dim strAPPACLS As String
        Dim drwRpt As DataRow

        dstOrders_Edit.Tables(0).Rows.Clear()

        Try
            'CNSUPDP001_023
            Do Until dtRptSource.Rows.Count = 0

                '行コピー
                drwRpt = dstOrders_1.Tables(0).Copy.Rows(0)

                For i_rpt As Integer = 0 To dtRptSource.Rows.Count - 1

                    Select Case dtRptSource.Rows(i_rpt)("D40_FREQUENCY").ToString
                        Case "2"
                            drwRpt.Item("周波数") = "Ｆ２"
                        Case "3"
                            drwRpt.Item("周波数") = "Ｆ３"
                        Case "4"
                            drwRpt.Item("周波数") = "Ｆ４"
                    End Select

                    '機器種別取得
                    strAPPACLS = dtRptSource.Rows(i_rpt)("M07_CNSTDET_CLS").ToString

                    If dctRowCnt.Keys.Contains(strAPPACLS) = False Then
                        strAPPACLS = String.Empty
                    End If

                    '表示上限確認
                    If dctRowCnt(strAPPACLS)(0) = Nothing OrElse dctRowCnt(strAPPACLS)(0) > dctRowCnt(strAPPACLS)(1) Then
                        '表示条件以内
                        'カウンターインクリメント
                        dctRowCnt(strAPPACLS)(1) += 1
                    Else
                        '表示上限オーバー
                        'スキップ
                        Continue For
                    End If

                    '全項目分に台数を設定
                    For i_item As Integer = 0 To 13

                        'i_item = 1~10 and 0台の場合は印字無し
                        If i_item = 0 OrElse i_item > 10 OrElse dtRptSource.Rows(i_rpt)(strItem(i_item, 1)).ToString <> "0" Then

                            strName.Clear()
                            strName.Append(dctAPPA(strAPPACLS))
                            strName.Append(strNo(dctRowCnt(strAPPACLS)(1) - 1))
                            strName.Append(strItem(i_item, 0))

                            drwRpt.Item(strName.ToString) = dtRptSource.Rows(i_rpt)(strItem(i_item, 1)).ToString

                        End If

                    Next

                    '処理済の行を削除
                    dtRptSource.Rows(i_rpt).Delete()

                Next

                '変更を反映
                dtRptSource.AcceptChanges()

                '作成した行を反映
                dstOrders_Edit.Tables(0).ImportRow(drwRpt)

                'IDの場合は2枚目以降を出力しない
                If strIDIC = "1" Then
                    Exit Do
                End If

                'カウンターリセット
                For Each c As Integer() In dctRowCnt.Values
                    c(1) = 0
                Next

            Loop

            '変更を反映
            dstOrders_Edit.AcceptChanges()



            'For i As Integer = 0 To dstOrders_1.Tables(1).Rows.Count - 1
            '    With dstOrders_Edit.Tables(0)
            '        Select Case dstOrders_1.Tables(1).Rows(i)("D40_FREQUENCY").ToString
            '            Case "2"
            '                .Rows(0)("周波数") = "Ｆ２"
            '            Case "3"
            '                .Rows(0)("周波数") = "Ｆ３"
            '            Case "4"
            '                .Rows(0)("周波数") = "Ｆ４"
            '        End Select
            '    End With
            'Next

            For i As Integer = 0 To dstOrders_1.Tables(2).Rows.Count - 1
                With dstOrders_Edit.Tables(0)

                    strName = New StringBuilder
                    strName.Append("SHUB機器名")
                    strName.Append(strNo(i))
                    .Rows(0)(strName.ToString) = dstOrders_1.Tables(2).Rows(i)("D41_APPA").ToString

                    strName = New StringBuilder
                    strName.Append("SHUB工事前数")
                    strName.Append(strNo(i))
                    .Rows(0)(strName.ToString) = dstOrders_1.Tables(2).Rows(i)("D41_CNSTB_WRKCNT").ToString

                    strName = New StringBuilder
                    strName.Append("SHUB店内移設")
                    strName.Append(strNo(i))
                    .Rows(0)(strName.ToString) = dstOrders_1.Tables(2).Rows(i)("D41_CNSTN_MOVCNT").ToString

                    strName = New StringBuilder
                    strName.Append("SHUB撤去対象")
                    strName.Append(strNo(i))
                    .Rows(0)(strName.ToString) = dstOrders_1.Tables(2).Rows(i)("D41_CNSTN_REMCNT").ToString

                    strName = New StringBuilder
                    strName.Append("SHUB今回取付")
                    strName.Append(strNo(i))
                    .Rows(0)(strName.ToString) = dstOrders_1.Tables(2).Rows(i)("D41_CNSTN_NEWCNT").ToString

                    strName = New StringBuilder
                    strName.Append("SHUB工事係数")
                    strName.Append(strNo(i))
                    .Rows(0)(strName.ToString) = dstOrders_1.Tables(2).Rows(i)("D41_CNSTA_WRKCNT").ToString


                    If i > 2 Then
                        Exit For
                    End If
                End With
            Next

            dstOrders_1 = dstOrders_Edit

        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.grvList.DataBind()
        End Try
    End Sub

    ''' <summary>
    ''' 工事依頼書兼仕様書印刷編集処理(ID2枚目用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Print_Edit2(ByRef repDs As DataSet)
        Dim dstOrders As New DataSet
        Dim int As Integer = 0
        Dim int2 As Integer = 0
        Dim idx01 As Integer
        Dim idx02 As Integer
        Dim idx03 As Integer
        Dim idx04 As Integer
        Dim idx05 As Integer
        Dim idx06 As Integer
        Dim idx07 As Integer
        Dim idx08 As Integer
        Dim idx09 As Integer
        Dim idx10 As Integer
        Dim idx11 As Integer
        Dim idx12 As Integer
        Dim idx13 As Integer
        Dim idx14 As Integer
        Dim idx15 As Integer
        Dim idx16 As Integer
        Dim idx17 As Integer
        Dim idx99 As Integer
        Dim idx As Integer

        Dim strNo() As String = {"１", "２", "３", "４", "５", "６"}
        Dim strName As StringBuilder = New StringBuilder
        dstOrders = repDs

        Try
            For repCnt As Integer = 0 To repDs.Tables(0).Rows.Count - 1
                '初期化
                idx01 = 0
                idx02 = 0
                idx03 = 0
                idx04 = 0
                idx05 = 0
                idx06 = 0
                idx07 = 0
                idx08 = 0
                idx09 = 0
                idx10 = 0
                idx11 = 0
                idx12 = 0
                idx13 = 0
                idx14 = 0
                idx15 = 0
                idx16 = 0
                idx17 = 0
                idx99 = 0
                idx = 0
                int2 = int
                For i As Integer = int To repDs.Tables(1).Rows.Count - 1
                    If i > int2 Then
                        If repDs.Tables(1).Rows(i)("D40_FREQUENCY").ToString <> repDs.Tables(1).Rows(i - 1)("D40_FREQUENCY").ToString Then
                            Exit For
                        End If
                    End If
                    int += 1
                    With dstOrders.Tables(0)
                        Select Case repDs.Tables(1).Rows(i)("D40_FREQUENCY").ToString
                            Case "2"
                                .Rows(repCnt)("周波数") = "Ｆ２"
                            Case "3"
                                .Rows(repCnt)("周波数") = "Ｆ３"
                            Case "4"
                                .Rows(repCnt)("周波数") = "Ｆ４"
                        End Select
                        Select Case repDs.Tables(1).Rows(i)("M07_CNSTDET_CLS").ToString
                            Case "01"

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("機器名")
                                .Rows(repCnt).Item(strName.ToString) = repDs.Tables(1).Rows(i)("D40_APPA").ToString

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("工事前稼動")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString <> "0" Then
                                    .Rows(repCnt).Item(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("工事前予備")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("工事前総合")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("店内移設")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("撤去対象")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("新品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("店内在庫自社")
                                If repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("店内在庫他社")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("譲与品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("代理在庫")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("工事後稼動")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_WRKCNT").ToString

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("工事後予備")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_STCCNT").ToString

                                strName = New StringBuilder
                                strName.Append("カードユニット")
                                strName.Append(strNo(idx01))
                                strName.Append("工事後総合")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_ALLCNT").ToString

                                idx01 += 1
                            Case "02"
                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("機器名")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_APPA").ToString

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("工事前稼動")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("工事前予備")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("工事前総合")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("店内移設")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("撤去対象")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("新品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("店内在庫自社")
                                If repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("店内在庫他社")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("譲与品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("代理在庫")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("工事後稼動")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_WRKCNT").ToString

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("工事後予備")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_STCCNT").ToString

                                strName = New StringBuilder
                                strName.Append("玉貸機")
                                strName.Append(strNo(idx02))
                                strName.Append("工事後総合")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_ALLCNT").ToString

                                idx02 += 1
                            Case "03"

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("機器名")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_APPA").ToString

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("工事前稼動")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("工事前予備")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("工事前総合")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("店内移設")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("撤去対象")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("新品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("店内在庫自社")
                                If repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("店内在庫他社")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("譲与品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("代理在庫")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("工事後稼動")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_WRKCNT").ToString

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("工事後予備")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_STCCNT").ToString

                                strName = New StringBuilder
                                strName.Append("メダル貸機")
                                strName.Append(strNo(idx03))
                                strName.Append("工事後総合")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_ALLCNT").ToString

                                idx03 += 1
                            Case "06"
                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("機器名")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_APPA").ToString

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("工事前稼動")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("工事前予備")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("工事前総合")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("店内移設")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("撤去対象")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("新品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("店内在庫自社")
                                If repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("店内在庫他社")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("譲与品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("代理在庫")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("工事後稼動")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_WRKCNT").ToString

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("工事後予備")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_STCCNT").ToString

                                strName = New StringBuilder
                                strName.Append("システムストッカー")
                                strName.Append(strNo(idx06))
                                strName.Append("工事後総合")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_ALLCNT").ToString

                                idx06 += 1

                            Case "07"

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("工事前稼動")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_WRKCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("工事前予備")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("工事前総合")
                                If repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTB_ALLCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("店内移設")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_MOVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("撤去対象")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_REVCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("新品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_NEWCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("店内在庫自社")
                                If repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSRN_STCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("店内在庫他社")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_OSTCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("譲与品")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_TRNCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("代理在庫")
                                If repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString <> "0" Then
                                    .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTN_AGCCNT").ToString
                                End If

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("工事後稼動")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_WRKCNT").ToString

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("工事後予備")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_STCCNT").ToString

                                strName = New StringBuilder
                                strName.Append("ディスプレイ")
                                strName.Append(strNo(idx07))
                                strName.Append("工事後総合")
                                .Rows(repCnt)(strName.ToString) = repDs.Tables(1).Rows(i)("D40_CNSTA_ALLCNT").ToString

                                idx07 += 1
                                If idx07 >= 1 Then
                                    idx07 = 0
                                End If

                        End Select
                    End With
                Next
            Next
        Catch ex As Exception
            psMesBox(Me, "0003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.grvList.DataBind()
        End Try

    End Sub

#End Region

#Region "ＤＬＬ設定変更依頼確認"

    '-----------------------------
    '2014/04/11 高松　ここから
    '-----------------------------

    ''' <summary>
    ''' ＤＬＬ設定変更依頼確認
    ''' </summary>
    ''' <param name="CNSUPDP001_D47_DLLSEND"></param>
    ''' <param name="AddDayF"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function msCheck_DllDt(ByRef CNSUPDP001_D47_DLLSEND As DataSet, ByRef AddDayF() As Integer) As Boolean

        Dim fsworkF As Boolean = False                  'ＦＳ稼動フラグ(なし:false あり:true)
        Dim cnsDate As DateTime = Nothing               '工事日時
        Dim spCnsDate As DateTime = Nothing             '特別変更時間(設定日時)
        Dim noCnsDate As DateTime = Nothing             '通常変更時間(戻し日時)
        Dim tmpCnsDate As String = String.Empty         '工事日(一時保管)
        Dim tmpCnstime As String = String.Empty         '工事時(一時保管)
        Dim dataset As New CNSUPDP001_D47_DLLSEND       'D47_DLLSENDのデータセット
        Dim dtRow As DataRow = dataset.Tables(0).NewRow 'D47_DLLSENDの行定義
        Dim cnsKbn As New System.Text.StringBuilder     '工事区分連結用

        '--------------------------------
        '2014/08/01 星野　ここから
        '--------------------------------
        'AddDayF = 0                                     '追加日時数
        AddDayF = {0, 0}                                 '追加日時数
        '--------------------------------
        '2014/08/01 星野　ここまで
        '--------------------------------
        msCheck_DllDt = False                           '戻り値
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

            '工事開始日の設定
            tmpCnsDate = Me.dttStartOfCon.ppText + " "

            tmpCnstime = Me.tmtStartOfCon.ppHourText + Me.tmtStartOfCon.ppMinText

            'ＤＬＬ設定変更なしにチェック有り
            If cbxDllSettingChange.Checked = True Then

                'ＤＬＬ変更依頼対象外
                Exit Function

            End If

            '備考に「特運無し」の文字列が含まれている
            If Me.txtRemarks.ppText.ToString.IndexOf("特運無し") > 0 _
            Or Me.txtSpecificationsRemarks.ppText.ToString.IndexOf("特運無し") > 0 Then
                'ＤＬＬ変更依頼対象外
                Exit Function

            End If

            '備考に「ＦＳ稼動無し」の文字列が含まれていない
            'ＦＳ稼働がない案件の場合
            '判定基準は、備考の文字列ではなく総合テスト日時が未設定で工事日時が設定されている場合はＦＳ稼働なし
            '上記以外はＦＳ稼働有で判定する
            'If Me.txtRemarks.ppText.ToString.IndexOf("ＦＳ稼動無し") <= 0 Then
            If (String.IsNullOrEmpty(dttTest.ppText) Or String.IsNullOrEmpty(String.IsNullOrEmpty(dttTest.ppText.Trim))) _
            And String.IsNullOrEmpty(dttStartOfCon.ppText) = False Then
            Else
                '総合テスト日時の設定
                tmpCnsDate = Me.dttTest.ppText + " "

                tmpCnstime = Me.tmtTest.ppHourText + Me.tmtTest.ppMinText

                fsworkF = True     'ＦＳ稼動あり

                'かつ、ホール内工事全撤去 または ホール内工事一時撤去が "1" の場合
                If txtHoleAllRemoval.ppText = "1" _
                    Or txtHoleOnceRemoval.ppText = "1" Then

                    'ＤＬＬ変更依頼対象外
                    Exit Function

                End If
            End If


            '時間が空の場合、"0000"とする
            If tmpCnstime = String.Empty Then
                tmpCnstime = "0000"
            End If

            '工事日時の設定を行う
            If 900 <= CInt(tmpCnstime) And CInt(tmpCnstime) <= 1759 Then

                spCnsDate = tmpCnsDate + "9:00"
                noCnsDate = tmpCnsDate + "18:00"

                '--------------------------------
                '2014/08/01 星野　ここから
                '--------------------------------
                'Else

                '    spCnsDate = tmpCnsDate + "18:00"
                '    noCnsDate = tmpCnsDate + "09:00"

                '    AddDayF = 1
            ElseIf 1800 <= CInt(tmpCnstime) And CInt(tmpCnstime) <= 2359 Then
                spCnsDate = tmpCnsDate + "18:00"
                noCnsDate = tmpCnsDate + "09:00"

                AddDayF = {0, 1}

            Else
                spCnsDate = tmpCnsDate + "18:00"
                noCnsDate = tmpCnsDate + "09:00"

                AddDayF = {-1, 0}

                '--------------------------------
                '2014/08/01 星野　ここまで
                '--------------------------------

            End If

            '工事区分連結
            ms_setCnsKbn(cnsKbn, Me.txtHoleNew.ppText)              'ホール内工事新規
            ms_setCnsKbn(cnsKbn, Me.txtHoleExpansion.ppText)        'ホール内工事増設
            ms_setCnsKbn(cnsKbn, Me.txtHoleSomeRemoval.ppText)      'ホール内工事一部撤去
            ms_setCnsKbn(cnsKbn, Me.txtHoleShopRelocation.ppText)   'ホール内工事店内移設
            ms_setCnsKbn(cnsKbn, Me.txtHoleAllRemoval.ppText)       'ホール内工事全撤去
            ms_setCnsKbn(cnsKbn, Me.txtHoleOnceRemoval.ppText)      'ホール内工事一時撤去
            ms_setCnsKbn(cnsKbn, Me.txtHoleReInstallation.ppText)   'ホール内工事再設置
            ms_setCnsKbn(cnsKbn, Me.txtHoleConChange.ppText)        'ホール内工事構成変更
            ms_setCnsKbn(cnsKbn, Me.txtHoleConDelively.ppText)      'ホール内工事構成配信
            ms_setCnsKbn(cnsKbn, Me.txtHoleOther.ppText)            'ホール内工事その他

            'D47_DLLSENDの設定
            If Not lblRequestNo_2.Text = String.Empty Then
                dtRow("D47_DLLSEND_NO") = Me.lblRequestNo_2.Text        '工事依頼番号
            Else
                dtRow("D47_DLLSEND_NO") = Me.ProvisionalRequestNo.ppTextBoxOne.Text + "-" + Me.ProvisionalRequestNo.ppTextBoxTwo.Text
            End If
            dtRow("D47_SEQNO") = "1"                                '連番(1:固定)
            dtRow("D47_NL_FLG") = Me.txtNLDivision.ppText           'ＮＬ区分
            dtRow("D47_REQ") = "工事"                               '依頼方法(工事:固定)
            dtRow("D47_SET_CD") = "99"                              '作業内容コード(99:固定)
            dtRow("D47_SET_NM") = "特別運用モードＳＷ"              '作業内容(特別運用モードＳＷ:固定)
            dtRow("D47_TBOXID") = Me.txtTboxId.ppText               'ＴＢＯＸＩＤ
            dtRow("D47_HALL_CD") = Me.txtHoleCd.ppText              'ホールコード
            dtRow("D47_HALL_NM") = Me.txtHoleNm.ppText              'ホール名
            dtRow("D47_TBOXCLASS_CD") = DBNull.Value                'ＴＢＯＸ機種コード(画面に無し登録時に取得)
            dtRow("D47_TBOXCLASS_NM") = DBNull.Value                'ＴＢＯＸ機種(画面に無し登録時に取得)
            If fsworkF = True And Me.txtHoleNew.ppText = "1" Then   '特別運用変更内容
                dtRow("D47_SPMODEST_DT") = DBNull.Value             'ＦＳ稼動ありかつ新規の場合 Null
            Else
                dtRow("D47_SPMODEST_DT") = spCnsDate                '特別変更時間(設定日時)を設定
            End If
            dtRow("D47_SPMODEST_CD") = DBNull.Value                 '特別運用変更内容コード(NULL:固定)
            dtRow("D47_NMMODEST_DT") = noCnsDate                    '通常運用モード戻し日時
            dtRow("D47_NMMODEST_CD") = DBNull.Value                 '通常運用変更内容コード(NULL:固定)
            If fsworkF Then                                         '工事有無
                dtRow("D47_CNST_FLG") = "1"                         'ＦＳ稼動有り
            Else
                dtRow("D47_CNST_FLG") = "0"                         'ＦＳ稼動無し
            End If
            dtRow("D47_DNIGHT_FLG") = DBNull.Value                  '深夜営業有無(NULL:固定)
            dtRow("D47_MODE_CD") = DBNull.Value                     '現状モード(NULL:固定)
            dtRow("D47_CNST_NM") = DBNull.Value                     '工事区分(NULL:固定)
            dtRow("D47_SETREQ_NM") = DBNull.Value                   '設定依頼者(NULL:固定)
            dtRow("D47_CNST_CLS") = cnsKbn                          '工事区分(ホール内工事新規～その他まで連結)
            dtRow("D47_SET_MSG") = DBNull.Value                     '設定依頼メッセージ(NULL:固定)
            dtRow("D47_SET_INFO") = DBNull.Value                    '設定情報(NULL:固定)
            dtRow("D47_RET_INFO") = DBNull.Value                    '戻し情報(NULL:固定)
            dtRow("D47_ADJMACHINE_CHG") = "0"                       '精算機変更(0:固定)
            dtRow("D47_ADJMACHINE_RTN") = "0"                       '精算機戻し(0:固定)
            dtRow("D47_NOTETEXT") = DBNull.Value                    '備考(NULL:固定)
            dtRow("D47_SPMODESND_FLG") = "0"                        '特別運用送信フラグ(0:固定)
            dtRow("D47_NMMODESND_FLG") = "0"                        '通常運用送信フラグ(0:固定)
            dtRow("D47_SPMODERCV_FLG") = DBNull.Value               '特別運用結果フラグ(NULL:固定)
            dtRow("D47_NMMODERCV_FLG") = DBNull.Value               '通常運用結果フラグ(NULL:固定)
            dtRow("D47_SENDEND_FLG") = DBNull.Value                 '案件終了フラグ(NULL:固定)
            dtRow("D47_DELETE_FLG") = "0"                           '削除フラグ(0:固定)
            dtRow("D47_INSERT_DT") = DBNull.Value                   '登録日時(登録時に取得)
            dtRow("D47_INSERT_USR") = Session(P_SESSION_USERID)     '登録者(NULL:固定)
            dtRow("D47_UPDATE_DT") = DBNull.Value                   '更新日時(NULL:固定)
            dtRow("D47_UPDATE_USR") = DBNull.Value                  '更新者(NULL:固定)
            dtRow("D47_STATUS_CD") = "1"                            '進捗状況(進捗ステータス1:受付済み:固定)
            dtRow("D47_SETRET_FLG") = DBNull.Value                  '入り戻し依頼フラグ(NULL:固定)
            dtRow("D47_PROCTIME_CLS") = "0"                         '処理時間判断区分(0:固定)

            dataset.Tables(0).Rows.Add(dtRow)
            CNSUPDP001_D47_DLLSEND = dataset

            msCheck_DllDt = True

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

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' 工事区分連結処理
    ''' </summary>
    ''' <param name="Kbn"></param>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Private Sub ms_setCnsKbn(ByRef Kbn As StringBuilder, data As String)
        If data = String.Empty Or data = Nothing Then
            Kbn.Append("0")
        Else
            Kbn.Append(data)
        End If
    End Sub

    '-----------------------------
    '2014/04/11 高松　ここまで
    '-----------------------------

#End Region

#End Region

#Region "案件進捗状況ドロップダウンリスト設定"
    ''' <summary>
    ''' 案件進捗状況設定（M27 進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlStatus(ByVal ipstrCnst_No As String)

        Dim conDB As SqlConnection = Nothing        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing           'SqlCommandクラス
        Dim dstOrders As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(conDB) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                cmdDB = New SqlCommand("CNSUPDP001_S17", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prmD39_CNST_NO", SqlDbType.NVarChar, ipstrCnst_No))
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'ドロップダウンリスト設定
                Me.ddlMTRStatus.Items.Clear()
                '--------------------------------
                '2015/04/20 加賀　ここから
                '--------------------------------
                ''仮登録のときは、現場作業待ち以降を表示しない
                'If Me.lblRequestNo_2.Text = String.Empty Then
                '    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録 Then
                '        For int As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                '            Select Case dstOrders.Tables(0).Rows(int).Item("ステータス").ToString
                '                Case "07", "08", "09", "10", "11"
                '                    dstOrders.Tables(0).Rows.RemoveAt(int)
                '                    int -= 1
                '            End Select
                '            If int = dstOrders.Tables(0).Rows.Count - 1 Then
                '                Exit For
                '            End If
                '        Next
                '    End If
                'Else
                '    If Me.lblRequestNo_2.Text.Substring(0, 5) = "N0090" Then
                '        For int As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                '            Select Case dstOrders.Tables(0).Rows(int).Item("ステータス").ToString
                '                Case "07", "08", "09", "10", "11"
                '                    dstOrders.Tables(0).Rows.RemoveAt(int)
                '                    int -= 1
                '            End Select
                '            If int = dstOrders.Tables(0).Rows.Count - 1 Then
                '                Exit For
                '            End If
                '        Next
                '    End If
                'End If
                '--------------------------------
                '2015/04/20 加賀　ここまで
                '--------------------------------

                Me.ddlMTRStatus.DataSource = dstOrders.Tables(0)

                Me.ddlMTRStatus.DataTextField = "ステータス名"
                Me.ddlMTRStatus.DataValueField = "ステータス"
                Me.ddlMTRStatus.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ステータスマスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

#End Region

#Region "営業所コード変更時処理"
    ''' <summary>
    ''' 営業所コード変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtOfficCD_TextChanged(sender As Object, e As EventArgs)

        '業者情報を取得
        msSetTrader()

        '作業担当者ドロップダウン設定
        msSetddlEmployee(Me.txtOfficCD.ppText)

    End Sub

#End Region

#Region "業者名設定処理"
    ''' <summary>
    ''' 会社コード、営業所コードに対応する業者を設定する。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetTrader()
        Dim dtsTRaderData As DataSet = Nothing
        '業者情報取得
        If mfGet_Trader(Me.txtOfficCD.ppText,
                        dtsTRaderData) Then
            '取得したデータを設定
            With dtsTRaderData.Tables(0).Rows(0)
                Me.lblSendNmV.Text = .Item("営業所名").ToString
            End With
        Else
            Me.lblSendNmV.Text = String.Empty
        End If

    End Sub

#End Region

#Region "業者名取得処理"
    ''' <summary>
    ''' 業者情報取得
    ''' </summary>
    ''' <param name="ipstrOfficeCD">営業所コード</param>
    ''' <param name="opdsTrader">業者情報</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Trader(ByVal ipstrOfficeCD As String,
                                  Optional ByRef opdsTrader As DataSet = Nothing)

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP001_S13", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ipstrOfficeCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdsTrader = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '整合性OK
                        mfGet_Trader = True
                    Case Else
                        '整合性エラー
                        mfGet_Trader = False
                End Select

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGet_Trader = False
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGet_Trader = False
        End If
    End Function

#End Region

#Region "緊急依頼判断処理"

    ''' <summary>
    ''' 緊急依頼判断処理
    ''' </summary>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <returns>成功：True, 失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfJg_Emergency(ByVal ipstrCnstNo As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0
        Dim dstOrders As New DataSet
        Dim dstJgmnt As New DataSet
        Dim dstDtCnt As New DataSet
        Dim CnstDate As Date
        Dim DataRcvDt As Date
        Dim stestDt As String = String.Empty
        Dim tmpSetDt As String = String.Empty
        Dim cnstDt As String = String.Empty
        Dim rcvDt As String = String.Empty
        Dim dayCnt As Integer = 0

        objStack = New StackFrame
        ViewState(M_VIEW_DATECNT) = 0

        mfJg_Emergency = False

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'FS稼動無の場合は処理しない
                If Me.txtSfOperation.ppText = 0 Then
                    If Not lblRequestNo_2.Text.IndexOf("N0090") >= 0 Or Not lblRequestNo_2.Text = String.Empty Then

                        '緊急依頼テーブル検索処理
                        cmdDB = New SqlCommand("CNSUPDP001_S24", conDB)
                        With cmdDB.Parameters
                            'パラメータ設定
                            .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))
                        End With

                        'データ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        '緊急依頼テーブルに該当レコードがない場合、もしくは緊急依頼区分が0の場合、緊急依頼判断処理を行う
                        If dstOrders.Tables(0).Rows.Count = 0 OrElse dstOrders.Tables(0).Rows(0).Item("D94_EMREQ_CLS").ToString = "0" Then

                            stestDt = Me.dttTest.ppText + " " + Me.tmtTest.ppHourText + ":" + Me.tmtTest.ppMinText
                            tmpSetDt = Me.dttTemporaryInstallation.ppText + " " + Me.tmtTemporaryInstallation.ppHourText + ":" + Me.tmtTemporaryInstallation.ppMinText
                            cnstDt = Me.dttStartOfCon.ppText + " " + Me.tmtStartOfCon.ppHourText + ":" + Me.tmtStartOfCon.ppMinText
                            rcvDt = Me.lblReceptionDt_2.Text + " " + Me.lblReceptionTm_2.Text

                            '工事日と受信日から土日を除いた日数を取得
                            If stestDt.Replace(":", "").Trim <> "" And tmpSetDt.Replace(":", "").Trim <> "" Then
                                If CType(stestDt, DateTime).Subtract(CType(rcvDt, DateTime)) > CType(tmpSetDt, DateTime).Subtract(CType(rcvDt, DateTime)) Then
                                    CnstDate = CType(tmpSetDt, DateTime).ToString("yyyy/MM/dd HH:mm")
                                Else
                                    CnstDate = CType(stestDt, DateTime).ToString("yyyy/MM/dd HH:mm")
                                End If
                            ElseIf stestDt.Replace(":", "").Trim <> "" Then
                                CnstDate = CType(stestDt, DateTime).ToString("yyyy/MM/dd HH:mm")
                            ElseIf tmpSetDt.Replace(":", "").Trim <> "" Then
                                CnstDate = CType(tmpSetDt, DateTime).ToString("yyyy/MM/dd HH:mm")
                            Else
                                CnstDate = CType(cnstDt, DateTime).ToString("yyyy/MM/dd HH:mm")
                            End If

                            If CnstDate <> Nothing And rcvDt.Replace(":", "").Trim <> "" Then
                                DataRcvDt = CType(rcvDt, DateTime).ToString("yyyy/MM/dd HH:mm")
                                For zz = 1 To CType(CnstDate.ToString("yyyy/MM/dd"), DateTime).Subtract(CType(DataRcvDt.ToString("yyyy/MM/dd"), DateTime)).Days
                                    If DataRcvDt.AddDays(zz).DayOfWeek <> DayOfWeek.Saturday And DataRcvDt.AddDays(zz).DayOfWeek <> DayOfWeek.Sunday Then
                                        ViewState(M_VIEW_DATECNT) += 1
                                    Else
                                        If DataRcvDt.AddDays(zz).ToString("yyyyMMdd") = CnstDate.ToString("yyyyMMdd") Then
                                            ViewState(M_VIEW_DATECNT) += 1
                                        End If
                                    End If
                                Next

                                '休日受けの場合、翌営業日受け扱いの為 日数を-1 'CNSUPDP001_017
                                Select Case DataRcvDt.DayOfWeek
                                    Case DayOfWeek.Saturday, DayOfWeek.Sunday
                                        ViewState(M_VIEW_DATECNT) -= 1
                                End Select
                                'CNSUPDP001_017 END
                            End If

                            '緊急依頼か判断
                            If stestDt.Replace(":", "").Trim = "" Then
                                stestDt = String.Empty
                            End If
                            If tmpSetDt.Replace(":", "").Trim = "" Then
                                tmpSetDt = String.Empty
                            End If
                            cmdDB = New SqlCommand("CNSUPDP001_S25", conDB)
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))
                                .Add(pfSet_Param("rcv_dt", SqlDbType.DateTime, DataRcvDt))
                                .Add(pfSet_Param("cnst_dt", SqlDbType.DateTime, CnstDate))
                                .Add(pfSet_Param("stest_dt", SqlDbType.NVarChar, stestDt))
                                .Add(pfSet_Param("tmpset_dt", SqlDbType.NVarChar, tmpSetDt))
                                .Add(pfSet_Param("date_cnt", SqlDbType.Int, ViewState(M_VIEW_DATECNT)))
                            End With

                            'データ取得
                            dstJgmnt = clsDataConnect.pfGet_DataSet(cmdDB)


                            '日調を取得
                            cmdDB = New SqlCommand("CNSUPDP001_S26", conDB)
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))
                                .Add(pfSet_Param("rcv_dt", SqlDbType.DateTime, DataRcvDt))
                                .Add(pfSet_Param("cnst_dt", SqlDbType.DateTime, CnstDate))
                                .Add(pfSet_Param("date_cnt", SqlDbType.Int, ViewState(M_VIEW_DATECNT)))
                            End With

                            'データ取得
                            dstDtCnt = clsDataConnect.pfGet_DataSet(cmdDB)

                            dayCnt = dstDtCnt.Tables(0).Rows(0).Item("日調")


                            '緊急依頼テーブル更新処理
                            cmdDB = New SqlCommand("CNSUPDP001_U7", conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            With cmdDB.Parameters
                                .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))              '工事依頼番号
                                .Add(pfSet_Param("emreq_cls", SqlDbType.NVarChar, dstJgmnt.Tables(0).Rows(0).Item("M29_CODE").ToString))            '緊急依頼区分'CNSUPDP001_014
                                '.Add(pfSet_Param("emreq_cls", SqlDbType.NVarChar, dstJgmnt.Tables(1).Rows(0).Item("M29_CODE").ToString))            '緊急依頼区分
                                .Add(pfSet_Param("rcv_dt", SqlDbType.DateTime, DataRcvDt))                 '受信日時
                                .Add(pfSet_Param("day_cnt", SqlDbType.Int, dayCnt))                        '日調
                                .Add(pfSet_Param("update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With
                            cmdDB.Transaction = trans

                            '実行
                            cmdDB.ExecuteNonQuery()

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "緊急依頼登録処理")
                                'ロールバック
                                trans.Rollback()
                                Return False
                            End If

                        End If
                    End If
                End If

                mfJg_Emergency = True
            Catch ex As Exception
                mfJg_Emergency = False
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Me.grvList.DataBind()
                Throw
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If
    End Function

#End Region

#Region "T03_TBOX更新処理"

    ''' <summary>
    ''' T03_TBOX更新処理
    ''' </summary>
    ''' <param name="ipstrCnstNo">工事依頼番号</param>
    ''' <returns>成功：True, 失敗：False</returns>
    ''' <remarks></remarks>
    Private Function mfUpdate_TBOX(ByVal ipstrCnstNo As String) As Boolean

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0

        objStack = New StackFrame

        mfUpdate_TBOX = False

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'T03_TBOX更新処理
                cmdDB = New SqlCommand("CNSUPDP001_U8", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, ipstrCnstNo))              '工事依頼番号
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtTboxId.ppText))       'TBOXID
                    .Add(pfSet_Param("update_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      '更新者
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With
                cmdDB.Transaction = trans

                '実行
                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "T03_TBOX更新処理")
                    'ロールバック
                    trans.Rollback()
                    Return False
                End If

                mfUpdate_TBOX = True
            Catch ex As Exception
                mfUpdate_TBOX = False
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

                Me.grvList.DataBind()
                Throw
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If
    End Function

#End Region

#Region "クリア処理"
    '-----------------------------
    '2014/05/29 土岐　ここから
    '-----------------------------
    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <param name="ipshtMode">遷移条件</param>
    ''' <remarks></remarks>
    Private Sub msClearScreen(ByVal ipshtMode As ClsComVer.E_遷移条件)
        '-----------------------------
        '2014/05/29 土岐　ここまで
        '-----------------------------
        Select Case ipshtMode
            Case ClsComVer.E_遷移条件.仮登録
                Me.txtTboxId.ppText = String.Empty                      'ＴＢＯＸＩＤ
                Me.txtHoleCd.ppText = String.Empty                      'ホールコード
                Me.txtHoleNm.ppText = String.Empty                      'ホール名
                Me.txtAddress.ppText = String.Empty                     '住所
                Me.txtHoleTelNo.ppText = String.Empty                   '電話番号
                Me.txtPersonInCharge_2.ppText = String.Empty            '担当者氏名
                Me.txtTboxLine.ppText = String.Empty                    'ＤＤＸＰ回線番号


                Me.dttTest.ppText = String.Empty                        '総合テスト日
                Me.tmtTest.ppHourText = String.Empty
                Me.tmtTest.ppMinText = String.Empty

                Me.dttTemporaryInstallation.ppText = String.Empty       '仮設置日
                Me.tmtTemporaryInstallation.ppHourText = String.Empty
                Me.tmtTemporaryInstallation.ppMinText = String.Empty

                'ホール内工事
                'Me.txtHoleNew.ppText = String.Empty                     '新規
                'Me.txtHoleExpansion.ppText = String.Empty               '増設
                'Me.txtHoleSomeRemoval.ppText = String.Empty             '一部撤去
                'Me.txtHoleShopRelocation.ppText = String.Empty          '店舗移設
                'Me.txtHoleAllRemoval.ppText = String.Empty              '全撤去
                'Me.txtHoleOnceRemoval.ppText = String.Empty             '一時撤去
                'Me.txtHoleReInstallation.ppText = String.Empty          '再設置
                'Me.txtHoleConChange.ppText = String.Empty               '構成変更
                'Me.txtHoleConDelively.ppText = String.Empty             '構成配信
                'Me.txtHoleOther.ppText = String.Empty                   'その他
                Me.txtHoleNew.ppText = "0"                     '新規
                Me.txtHoleExpansion.ppText = "0"               '増設
                Me.txtHoleSomeRemoval.ppText = "0"             '一部撤去
                Me.txtHoleShopRelocation.ppText = "0"          '店舗移設
                Me.txtHoleAllRemoval.ppText = "0"              '全撤去
                Me.txtHoleOnceRemoval.ppText = "0"             '一時撤去
                Me.txtHoleReInstallation.ppText = "0"          '再設置
                Me.txtHoleConChange.ppText = "0"               '構成変更
                Me.txtHoleConDelively.ppText = "0"             '構成配信
                Me.txtHoleOther.ppText = "0"                   'その他

                Me.txtOfficCD.ppText = String.Empty                     '営業所コード
                '-----------------------------
                '2014/06/14 武　ここから
                '-----------------------------
                'Me.lblSendNmN.Text = String.Empty                       '営業所名
                '-----------------------------
                '2014/06/14 武　ここまで
                '-----------------------------
                '--------------------------------
                '2015/04/17 加賀　ここから
                '--------------------------------
                Me.txtSysClassification.ppText = String.Empty           'システム分類
                Me.txtNLDivision.ppText = String.Empty                  'NL区分
                Me.txtCurrentSys.ppText = String.Empty                  '現行システム
                Me.txtVer.ppText = String.Empty                         'VER
                Me.txtOtherContents.ppText = String.Empty               'その他内容
                lblSendNmV.Text = String.Empty                          '営業所名
                Me.txtRemarks.ppText = String.Empty                     '備考
                Me.txtSpecificationsRemarks.ppText = String.Empty       '使用備考
                Me.txtImportantPoints1.ppText = String.Empty            '注意事項
                Me.txtImportantPoints2.ppText = String.Empty            '連絡事項

                '制御情報
                Me.cbxDllSettingChange.Checked = False                  'DLL設定変更
                Me.trfSfPersonInCharge.ppText = String.Empty            'FS担当者
                Me.txtProcessingResultDetail1.ppText = String.Empty     '処理結果詳細1
                Me.txtProcessingResultDetail2.ppText = String.Empty     '処理結果詳細2
                Me.txtProcessingResultDetail3.ppText = String.Empty     '処理結果詳細3
                Me.txtControlInfoRemarks1.ppText = String.Empty         '備考
                Me.txtControlInfoRemarks2.ppText = String.Empty         '備考
                Me.txtControlInfoRemarks3.ppText = String.Empty         '備考
                '--------------------------------
                '2015/04/17 加賀　ここまで
                '--------------------------------
            Case ClsComVer.E_遷移条件.登録
                Me.txtOfficCD.ppText = String.Empty                 '営業所コード
                '-----------------------------
                '2014/06/14 武　ここから
                '-----------------------------
                'Me.lblSendNmN.Text = String.Empty                   '営業所名
                '-----------------------------
                '2014/06/14 武　ここまで
                '-----------------------------
                Me.cbxDllSettingChange.Checked = False              'ＤＬＬ設定変更なし
                Me.trfSfPersonInCharge.ppText = String.Empty        'エフエス担当者
                Me.RadioButtonList1.SelectedIndex = 0               '処理結果
                Me.txtProcessingResultDetail1.ppText = String.Empty '処理結果詳細1
                Me.txtProcessingResultDetail2.ppText = String.Empty '処理結果詳細2
                Me.txtProcessingResultDetail3.ppText = String.Empty '処理結果詳細3
                Me.txtControlInfoRemarks1.ppText = String.Empty     '備考1
                Me.txtControlInfoRemarks2.ppText = String.Empty     '備考2
                Me.txtControlInfoRemarks3.ppText = String.Empty     '備考3
        End Select
    End Sub
#End Region

    ''' <summary>
    ''' フォントカラー初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearFontColor()
        Me.lbltMpsttsCd.ForeColor = Drawing.Color.Empty                                 '仮設置進捗状況
        Me.lblStatusCd.ForeColor = Drawing.Color.Empty                                  '本設置進捗状況
        Me.lblReqState.ForeColor = Drawing.Color.Empty                                  '資料請求状況
        Me.lblRequestNo_2.ForeColor = Drawing.Color.Empty                               '依頼番号
        Me.lblNumberOfUpdates.ForeColor = Drawing.Color.Empty                           '更新回数
        Me.lblReceptionDt_2.ForeColor = Drawing.Color.Empty                             '受付日付(日付)
        Me.lblReceptionTm_2.ForeColor = Drawing.Color.Empty                             '受付日付(時刻)
        Me.txtSpecificationConnectingDivision.ppTextBox.ForeColor = Drawing.Color.Empty '仕様連絡区分
        Me.txtNLDivision.ppTextBox.ForeColor = Drawing.Color.Empty                      'ＮＬ区分
        Me.ttwNotificationNo.ppTextBoxOne.ForeColor = Drawing.Color.Empty               '通知番号(前半)
        Me.ttwNotificationNo.ppTextBoxTwo.ForeColor = Drawing.Color.Empty               '通知番号(後半)
        Me.txtCurrentSys.ppTextBox.ForeColor = Drawing.Color.Empty                      '現行システム
        Me.txtVer.ppTextBox.ForeColor = Drawing.Color.Empty                             'ＶＥＲ
        Me.txtSysClassification.ppTextBox.ForeColor = Drawing.Color.Empty               'システム分類
        Me.txtTboxId.ppTextBox.ForeColor = Drawing.Color.Empty                          'ＴＢＯＸＩＤ
        Me.txtHoleNm.ppTextBox.ForeColor = Drawing.Color.Empty                          'ホール名
        Me.txtTboxLine.ppTextBox.ForeColor = Drawing.Color.Empty                        'ＴＢＯＸ回線
        Me.txtHoleCd.ppTextBox.ForeColor = Drawing.Color.Empty                          'ホールコード
        Me.txtHoleTelNo.ppTextBox.ForeColor = Drawing.Color.Empty                       'ホールＴＥＬ
        Me.txtPersonInCharge_1.ppTextBox.ForeColor = Drawing.Color.Empty                '責任者
        Me.txtPersonInCharge_2.ppTextBox.ForeColor = Drawing.Color.Empty                '担当者
        Me.txtAddress.ppTextBox.ForeColor = Drawing.Color.Empty                         '住所
        Me.txtHoleNew.ppTextBox.ForeColor = Drawing.Color.Empty                         'ホール内工事新規
        Me.txtHoleExpansion.ppTextBox.ForeColor = Drawing.Color.Empty                   'ホール内工事増設
        Me.txtHoleSomeRemoval.ppTextBox.ForeColor = Drawing.Color.Empty                 'ホール内工事一部撤去
        Me.txtHoleShopRelocation.ppTextBox.ForeColor = Drawing.Color.Empty              'ホール内工事店舗移設
        Me.txtHoleAllRemoval.ppTextBox.ForeColor = Drawing.Color.Empty                  'ホール内工事全撤去
        Me.txtHoleOnceRemoval.ppTextBox.ForeColor = Drawing.Color.Empty                 'ホール内工事一時撤去
        Me.txtHoleReInstallation.ppTextBox.ForeColor = Drawing.Color.Empty              'ホール内工事再設置
        Me.txtHoleConChange.ppTextBox.ForeColor = Drawing.Color.Empty                   'ホール内工事構成変更
        Me.txtHoleConDelively.ppTextBox.ForeColor = Drawing.Color.Empty                 'ホール内工事構成配信
        Me.txtHoleOther.ppTextBox.ForeColor = Drawing.Color.Empty                       'ホール内工事その他
        Me.txtLanNew.ppTextBox.ForeColor = Drawing.Color.Empty                          'ＬＡＮ工事新規
        Me.txtLanExpansion.ppTextBox.ForeColor = Drawing.Color.Empty                    'ＬＡＮ工事増設
        Me.txtLanSomeRemoval.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＬＡＮ工事一部撤去
        Me.txtLanShopRelocation.ppTextBox.ForeColor = Drawing.Color.Empty               'ＬＡＮ工事店舗移設
        Me.txtLanAllRemoval.ppTextBox.ForeColor = Drawing.Color.Empty                   'ＬＡＮ工事全撤去
        Me.txtLanOnceRemoval.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＬＡＮ工事一時撤去
        Me.txtLanReInstallation.ppTextBox.ForeColor = Drawing.Color.Empty               'ＬＡＮ工事再設置
        Me.txtLanConChange.ppTextBox.ForeColor = Drawing.Color.Empty                    'ＬＡＮ工事構成変更
        Me.txtLanConDelively.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＬＡＮ工事構成配信
        Me.txtLanOther.ppTextBox.ForeColor = Drawing.Color.Empty                        'ＬＡＮ工事その他
        Me.txtStkHoleIn.ppTextBox.ForeColor = Drawing.Color.Empty                       'ストッカーホール内
        Me.txtStkHoleOut.ppTextBox.ForeColor = Drawing.Color.Empty                      'ストッカーホール外
        Me.txtSfOperation.ppTextBox.ForeColor = Drawing.Color.Empty                     'エフエス稼動有無
        Me.txtEMoneyIntroduction.ppTextBox.ForeColor = Drawing.Color.Empty              'Ｅマネー導入
        Me.txtTboxTakeawayDivision.ppTextBox.ForeColor = Drawing.Color.Empty            'ＴＢＯＸ持帰区分
        Me.txtEMoneyIntroductionCns.ppTextBox.ForeColor = Drawing.Color.Empty           'Ｅマネー導入工事
        Me.dttEMoneyTestDt.ppDateBox.ForeColor = Drawing.Color.Empty                    'Ｅマネーテスト日付
        Me.tmtEMoneyTestTm.ppHourBox.ForeColor = Drawing.Color.Empty                    'Ｅマネーテスト時間(時)
        Me.tmtEMoneyTestTm.ppMinBox.ForeColor = Drawing.Color.Empty                     'Ｅマネーテスト時間(分)
        Me.txtOtherContents.ppTextBox.ForeColor = Drawing.Color.Empty                   'その他内容
        Me.txtTwinsShop.ppTextBox.ForeColor = Drawing.Color.Empty                       '双子店
        Me.txtIndependentCns.ppTextBox.ForeColor = Drawing.Color.Empty                  '単独工事
        Me.txtSameTimeCnsNo.ppTextBox.ForeColor = Drawing.Color.Empty                   '同時工事数
        Me.txtPAndCDivision.ppTextBox.ForeColor = Drawing.Color.Empty                   '親子区分
        Me.txtParentHoleCd.ppTextBox.ForeColor = Drawing.Color.Empty                    '親ホールコード
        Me.txtConstructionExistenceF1.ppTextBox.ForeColor = Drawing.Color.Empty         '工事有無Ｆ1
        Me.txtConstructionExistenceF2.ppTextBox.ForeColor = Drawing.Color.Empty         '工事有無Ｆ2
        Me.txtConstructionExistenceF3.ppTextBox.ForeColor = Drawing.Color.Empty         '工事有無Ｆ3
        Me.txtConstructionExistenceF4.ppTextBox.ForeColor = Drawing.Color.Empty         '工事有無Ｆ4
        Me.dttStartOfCon.ppDateBox.ForeColor = Drawing.Color.Empty                      '工事開始(日付)
        Me.tmtStartOfCon.ppHourBox.ForeColor = Drawing.Color.Empty                      '工事開始(時)
        Me.tmtStartOfCon.ppMinBox.ForeColor = Drawing.Color.Empty                       '工事開始(分)
        Me.dttLastOpenDt.ppDateBox.ForeColor = Drawing.Color.Empty                      '最終営業日
        Me.dttLastOpenDtT500.ppDateBox.ForeColor = Drawing.Color.Empty                  '最終営業日Ｔ５００
        Me.dttTest.ppDateBox.ForeColor = Drawing.Color.Empty                            '総合テスト(日付)
        Me.tmtTest.ppHourBox.ForeColor = Drawing.Color.Empty                            '総合テスト(時)
        Me.tmtTest.ppMinBox.ForeColor = Drawing.Color.Empty                             '総合テスト(分)
        Me.txtTestWorkPersonnel.ppTextBox.ForeColor = Drawing.Color.Empty               '総合テスト作業担当者
        Me.dttTestDepartureDt.ppDateBox.ForeColor = Drawing.Color.Empty                 '総合テスト出発日時(日付)
        Me.tmtTestDepartureTm.ppHourBox.ForeColor = Drawing.Color.Empty                 '総合テスト出発日時(時)
        Me.tmtTestDepartureTm.ppMinBox.ForeColor = Drawing.Color.Empty                  '総合テスト出発日時(分)
        Me.dttTemporaryInstallation.ppDateBox.ForeColor = Drawing.Color.Empty           '仮設置(日付)
        Me.tmtTemporaryInstallation.ppHourBox.ForeColor = Drawing.Color.Empty           '仮設置(時)
        Me.tmtTemporaryInstallation.ppMinBox.ForeColor = Drawing.Color.Empty            '仮設置(分)
        Me.txtTemporaryInstallationWorkPersonnel.ppTextBox.ForeColor = Drawing.Color.Empty      '仮設置作業担当者
        Me.dttTemporaryInstallationDepartureDt.ppDateBox.ForeColor = Drawing.Color.Empty        '仮設置出発日時(日付)
        Me.tmtTemporaryInstallationDepartureTm.ppHourBox.ForeColor = Drawing.Color.Empty        '仮設置出発日時(時)
        Me.tmtTemporaryInstallationDepartureTm.ppMinBox.ForeColor = Drawing.Color.Empty         '仮設置出発日時(分)
        Me.dttTemporaryInstallationCnsDivision.ppTextBox.ForeColor = Drawing.Color.Empty        '仮設置工事区分
        Me.dttTemporaryInstallationDtNotInputDivision0.ppTextBox.ForeColor = Drawing.Color.Empty '仮設置日未入力区分
        Me.dttPolice.ppDateBox.ForeColor = Drawing.Color.Empty                          '警察(日付)
        Me.tmtPolice.ppHourBox.ForeColor = Drawing.Color.Empty                          '警察(時)
        Me.tmtPolice.ppMinBox.ForeColor = Drawing.Color.Empty                           '警察(分)
        Me.dttShiftCnsDivision.ppTextBox.ForeColor = Drawing.Color.Empty                '移行工事区分
        Me.dttShiftCnsWorkDivision.ppTextBox.ForeColor = Drawing.Color.Empty            '移行工事作業区分
        Me.dttOpen.ppDateBox.ForeColor = Drawing.Color.Empty                            'オープン(日付)
        Me.tmtOpen.ppHourBox.ForeColor = Drawing.Color.Empty                            'オープン(時)
        Me.tmtOpen.ppMinBox.ForeColor = Drawing.Color.Empty                             'オープン(分)
        Me.dttLanCns.ppDateBox.ForeColor = Drawing.Color.Empty                          'ＬＡＮ工事(日付)
        Me.tmtLanCns.ppHourBox.ForeColor = Drawing.Color.Empty                          'ＬＡＮ工事(時)
        Me.tmtLanCns.ppMinBox.ForeColor = Drawing.Color.Empty                           'ＬＡＮ工事(分)
        Me.lblRegisteredEmployeesNm_2.ForeColor = Drawing.Color.Empty                   '登録社員名
        Me.dttVerup.ppDateBox.ForeColor = Drawing.Color.Empty                           'ＶＥＲＵＰ(日付)
        Me.tmtVerup.ppHourBox.ForeColor = Drawing.Color.Empty                           'ＶＥＲＵＰ(時)
        Me.tmtVerup.ppMinBox.ForeColor = Drawing.Color.Empty                            'ＶＥＲＵＰ(分)
        Me.dttVerupDtDivision.ppTextBox.ForeColor = Drawing.Color.Empty                 'ＶＥＲＵＰ日付区分
        Me.txtVerupCnsType_1.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＶＥＲＵＰ工事種類１
        Me.txtVerupCnsType_2.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＶＥＲＵＰ工事種類２
        Me.txtAgencyCd.ppTextBox.ForeColor = Drawing.Color.Empty                        '代理店コード
        Me.trfAgencyNm.ppTextBox.ForeColor = Drawing.Color.Empty                        '代理店名
        Me.txtAgencyTel.ppTextBox.ForeColor = Drawing.Color.Empty                       '代理店ＴＥＬ
        Me.txtAgencyPersonnel.ppTextBox.ForeColor = Drawing.Color.Empty                 '代理店担当者
        Me.txtAgencyAdd.ppTextBox.ForeColor = Drawing.Color.Empty                       '代理店住所
        Me.txtAgencyResponsible.ppTextBox.ForeColor = Drawing.Color.Empty               '代理店責任者
        Me.txtAgencyShop.ppTextBox.ForeColor = Drawing.Color.Empty                      '代行店コード
        Me.trfAgencyShop.ppTextBox.ForeColor = Drawing.Color.Empty                      '代行店名
        Me.txtAgencyShopTelNo.ppTextBox.ForeColor = Drawing.Color.Empty                 '代行店ＴＥＬ
        Me.txtAgencyShopPersonnel.ppTextBox.ForeColor = Drawing.Color.Empty             '代行店担当者
        Me.txtAgencyShopAdd.ppTextBox.ForeColor = Drawing.Color.Empty                   '代行店住所
        Me.txtAgencyShopResponsible.ppTextBox.ForeColor = Drawing.Color.Empty           '代行店責任者
        Me.txtSendingStation.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＬＡＮ送付先コード
        Me.trfSendingStation.ppTextBox.ForeColor = Drawing.Color.Empty                  'ＬＡＮ送付先名
        Me.txtSendingStationResponsible.ppTextBox.ForeColor = Drawing.Color.Empty       'ＬＡＮ送付先責任者
        Me.tdtDeliveryPreferredDt.ppDateBox.ForeColor = Drawing.Color.Empty             'ＬＡＮ送付先納入希望日(日付)
        Me.ClsCMTimeBox1.ppHourBox.ForeColor = Drawing.Color.Empty                      'ＬＡＮ送付先納入希望日(時)
        Me.ClsCMTimeBox1.ppMinBox.ForeColor = Drawing.Color.Empty                       'ＬＡＮ送付先納入希望日(分)
        Me.txtSendingStationAdd.ppTextBox.ForeColor = Drawing.Color.Empty               'ＬＡＮ送付先住所
        Me.txtSendingStationTelNo.ppTextBox.ForeColor = Drawing.Color.Empty             'ＬＡＮ送付先ＴＥＬ
        Me.txtRemarks.ppTextBox.ForeColor = Drawing.Color.Empty                         '備考
        Me.txtSpecificationsRemarks.ppTextBox.ForeColor = Drawing.Color.Empty           '仕様備考
        Me.txtImportantPoints1.ppTextBox.ForeColor = Drawing.Color.Empty                '注意事項１
        Me.txtImportantPoints2.ppTextBox.ForeColor = Drawing.Color.Empty                '連絡事項
        Me.cbxDllSettingChange.ForeColor = Drawing.Color.Empty                          'ＤＬＬ設定変更なし
        Me.txtOfficCD.ppTextBox.ForeColor = Drawing.Color.Empty                         '営業所コード
        Me.lblSendNmV.ForeColor = Drawing.Color.Empty                                   '営業所名
    End Sub

#Region "作業担当者ドロップダウン設定"
    ''' <summary>
    ''' 作業担当者ドロップダウン設定
    ''' </summary>
    ''' <param name="ipstrOfficeCD">営業所コード</param>
    ''' <remarks></remarks>
    Private Sub msSetddlEmployee(ByVal ipstrOfficeCD As String)

        Dim dsEmployee As DataSet = Nothing

        If mfGet_Employee(ipstrOfficeCD, dsEmployee) Then

            '総合テスト日(本設置)の作業担当者
            Me.ddlPersonal1.Items.Clear()
            Me.ddlPersonal1.DataSource = dsEmployee.Tables(0)
            Me.ddlPersonal1.DataTextField = "社員名"
            Me.ddlPersonal1.DataValueField = "社員コード"
            Me.ddlPersonal1.DataBind()
            Me.ddlPersonal1.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            '仮設置日の作業担当者
            Me.ddlPersonal2.Items.Clear()
            Me.ddlPersonal2.DataSource = dsEmployee.Tables(0)
            Me.ddlPersonal2.DataTextField = "社員名"
            Me.ddlPersonal2.DataValueField = "社員コード"
            Me.ddlPersonal2.DataBind()
            Me.ddlPersonal2.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

        Else

            Me.ddlPersonal1.Items.Clear()
            Me.ddlPersonal2.Items.Clear()

        End If

    End Sub

#End Region

#Region "作業担当者取得"
    ''' <summary>
    ''' 作業担当者取得
    ''' </summary>
    ''' <param name="ipstrOfficeCD">営業所コード</param>
    ''' <param name="opdsEmployee">作業担当者</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Employee(ByVal ipstrOfficeCD As String,
                                  Optional ByRef opdsEmployee As DataSet = Nothing)

        Dim conDB As SqlConnection
        Dim cmdDB As SqlCommand
        Dim strOKNG As String

        objStack = New StackFrame

        '初期化
        conDB = Nothing
        strOKNG = String.Empty
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("CNSUPDP001_S14", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, ipstrOfficeCD))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                opdsEmployee = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '該当データあり
                        mfGet_Employee = True
                    Case Else
                        '該当データなし
                        mfGet_Employee = False
                End Select

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGet_Employee = False
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGet_Employee = False
        End If
    End Function

#End Region

    'CNSUPDP001_015
    ''' <summary>
    ''' ＦＳ、ＮＧＣをマスターから取得
    ''' </summary>
    ''' <param name="cpobjDS"></param>
    ''' <param name="cpobjDBCon"></param>
    ''' <param name="cpobjDBCmd"></param>
    ''' <remarks></remarks>
    Private Sub sAddSPCName(ByRef cpobjDS As DataSet, cpobjDBCon As SqlConnection, cpobjDBCmd As SqlCommand)

        Dim objWKDS As New DataSet
        Dim objWKDS_CMP As New DataSet
        Dim strEW_FLG As String = ""

        cpobjDS.Tables(0).Columns.Add("ＦＳＣＭＰ", Type.GetType("System.String"))
        cpobjDS.Tables(0).Columns.Add("ＮＧＣＣＭＰ", Type.GetType("System.String"))
        cpobjDS.Tables(0).Columns.Add("ＮＧＣＣＳＨ", Type.GetType("System.String"))
        cpobjDS.Tables(0).Columns.Add("ＦＳＳＰＣ", Type.GetType("System.String"))
        cpobjDS.Tables(0).AcceptChanges()

        'ＤＢ接続
        If cpobjDBCon Is Nothing Then
            If Not clsDataConnect.pfOpen_Database(cpobjDBCon) Then
                Exit Sub
            End If
        End If
        'パラメータ設定
        cpobjDBCmd = New SqlCommand("CNSCOMP001_S2", cpobjDBCon)
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_HALLCD", SqlDbType.NVarChar, txtHoleCd.ppText))
        objWKDS = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        strEW_FLG = "E"
        If objWKDS.Tables.Count > 0 Then
            If objWKDS.Tables(0).Rows.Count > 0 Then
                If objWKDS.Tables(0).Rows(0).Item("T01_FAXNO") Is DBNull.Value Then
                ElseIf objWKDS.Tables(0).Rows(0).Item("T01_FAXNO").ToString = "" Then
                Else
                    strEW_FLG = objWKDS.Tables(0).Rows(0).Item("T01_FAXNO").ToString
                End If
            End If
        End If

        cpobjDBCmd = New SqlCommand("CNSCOMP001_S1", cpobjDBCon)
        '    @prm_SEQNO AS NUMERIC(4,0)
        '   ,@prm_TRADE_CD AS NVARCHAR(2)
        '   ,@prm_COMP_CD AS NVARCHAR(5)
        '   ,@prm_OFFICE_CD AS NVARCHAR(5)
        If strEW_FLG = "E" Then
            cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "1"))
        Else
            cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "2"))
        End If
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_TRADE_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_COMP_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_OFFICE_CD", SqlDbType.NVarChar, ""))
        objWKDS_CMP = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        If objWKDS_CMP.Tables.Count > 0 Then
            If objWKDS_CMP.Tables(0).Rows.Count > 0 Then
                For zz = 0 To cpobjDS.Tables(0).Rows.Count - 1
                    cpobjDS.Tables(0).Rows(zz).Item("ＮＧＣＣＭＰ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_COMP_NM")
                    cpobjDS.Tables(0).Rows(zz).Item("ＮＧＣＣＳＨ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_OFFICE_NM")
                Next
            End If
        End If

        cpobjDBCmd = New SqlCommand("CNSCOMP001_S1", cpobjDBCon)
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "207"))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_TRADE_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_COMP_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_OFFICE_CD", SqlDbType.NVarChar, ""))
        objWKDS_CMP = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        If objWKDS_CMP.Tables.Count > 0 Then
            If objWKDS_CMP.Tables(0).Rows.Count > 0 Then
                For zz = 0 To cpobjDS.Tables(0).Rows.Count - 1
                    cpobjDS.Tables(0).Rows(zz).Item("ＦＳＳＰＣ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_COMP_NM") & " " & objWKDS_CMP.Tables(0).Rows(0).Item("M44_OFFICE_NM")
                Next
            End If
        End If

        cpobjDBCmd = New SqlCommand("CNSCOMP001_S1", cpobjDBCon)
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_SEQNO", SqlDbType.NVarChar, "208"))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_TRADE_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_COMP_CD", SqlDbType.NVarChar, ""))
        cpobjDBCmd.Parameters.Add(pfSet_Param("prm_OFFICE_CD", SqlDbType.NVarChar, ""))
        objWKDS_CMP = clsDataConnect.pfGet_DataSet(cpobjDBCmd)

        If objWKDS_CMP.Tables.Count > 0 Then
            If objWKDS_CMP.Tables(0).Rows.Count > 0 Then
                For zz = 0 To cpobjDS.Tables(0).Rows.Count - 1
                    cpobjDS.Tables(0).Rows(zz).Item("ＦＳＣＭＰ") = objWKDS_CMP.Tables(0).Rows(0).Item("M44_COMP_NM") & " " & objWKDS_CMP.Tables(0).Rows(0).Item("M44_OFFICE_NM")
                Next
            End If
        End If

        cpobjDS.Tables(0).AcceptChanges()

    End Sub
    'CNSUPDP001_015



    'CNSUPDP001_020
    ''' <summary>
    ''' 双子店区分、MDN機器の更新有無をチェックする。
    ''' </summary>
    ''' <param name="drCnstInfo">工事依頼書の情報（更新前）を上書きして返す</param>
    ''' <returns>0:更新不要　1:MDN、双子店更新　2:MDNのみ更新　3:MDN戻し
    ''' </returns>
    ''' <remarks></remarks>
    Private Function mfCheckUpdTbox(ByRef drCnstInfo As DataRow) As String
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0
        Dim dsCnstInfo As New DataSet
        Dim blnTwinUpd As Boolean = False
        Dim blnFSWork As Boolean = False

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '工事データ取得
                cmdDB = New SqlCommand("CNSUPDP001_S1", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, lblRequestNo_2.Text))
                End With
                trans = conDB.BeginTransaction
                cmdDB.Transaction = trans
                dsCnstInfo = clsDataConnect.pfGet_DataSet(cmdDB)
                trans.Commit()
                'データセット取得チェック
                If dsCnstInfo Is Nothing OrElse dsCnstInfo.Tables(0).Rows().Count = 0 Then
                    Return "9" 'エラー
                    Exit Function
                End If

                drCnstInfo = dsCnstInfo.Tables(0).Rows(0)

                'FS稼働無チェック(FS稼働無のみ更新処理対象) 
                If drCnstInfo.Item("D39_FSWRK_CLS") = "1" OrElse (drCnstInfo.Item("D39_TMPSET_DT") = "" And drCnstInfo.Item("D39_STEST_DT") = "") Then
                    '未来の工事かチェック（日付部分を比較）マシンタイムと工事開始日
                    If DateTime.Now.Date.CompareTo(DateTime.Parse(drCnstInfo("D39_CNST_DY").ToString)) < 0 Then
                        '未来なので処理無し
                        Return "0" '0:処理無し
                        Exit Function
                    ElseIf DateTime.Now.Date.CompareTo(DateTime.Parse(drCnstInfo("D39_CNST_DY").ToString)) = 0 Then
                        '本日の工事なので今の時間をチェック
                        '時刻部分だけ比較（2000/01/01 11:10:00として比較）マシンタイムと11:10(タスク起動時刻)
                        If DateTime.Now.TimeOfDay.CompareTo(New DateTime("2000", "01", "01", "11", "10", "00").TimeOfDay) >= 0 Then
                            '11:10以降は以降の処理で分岐
                        Else
                            '11:10以前なので処理無し（タスク処理に任せる）
                            Return "0" '0:処理無し
                            Exit Function
                        End If
                    Else
                        '過去工事は以降の処理で分岐
                    End If
                Else
                    blnFSWork = True
                End If

                '連絡区分チェック
                If txtSpecificationConnectingDivision.ppText.Trim = "3" OrElse drCnstInfo.Item("D39_TELL_CLS") = "3" Then '3:キャンセル
                    'キャンセルなので処理無し
                    Return "0" '0:処理無し
                    Exit Function
                End If

                '進捗状況チェック「07:現場終了」以上の場合のみ更新する。
                Dim strTwinBefore As String = drCnstInfo.Item("D39_TWIN_CD").ToString '変更前双子区分
                Dim strTwinAfter As String = txtTwinsShop.ppText.Trim '変更後双子区分
                Dim intStsInput As Integer = ddlMTRStatus.SelectedValue '案件進捗状況

                '[双子区分]未入力は0として扱います
                If strTwinBefore = String.Empty Then
                    strTwinBefore = "0"
                End If
                If strTwinAfter = String.Empty Then
                    strTwinAfter = "0"
                End If

                '双子区分に変更が無い場合、双子区分の変更処理無し。
                If strTwinBefore = strTwinAfter Then
                    blnTwinUpd = False
                Else
                    blnTwinUpd = True
                End If

                'FS稼働有は「工事進捗　参照／更新」での更新処理を実施する為、双子店のみ更新
                If blnFSWork = True Then
                    If blnTwinUpd = True Then
                        Return "4"
                    End If
                Else
                    If intStsInput < 7 Then
                        '入力値：現場未終了
                        '3:MDN戻し処理
                        Return "3"
                    Else
                        '入力値：現場終了以上
                        If blnTwinUpd = True Then
                            '1:双子、MDN更新
                            Return "1"
                        Else
                            '2:MDNのみ更新
                            Return "2"
                        End If
                    End If
                End If

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Throw
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If
        Return "0"
    End Function

    ''' <summary>
    ''' 双子店区分の更新処理
    ''' </summary>
    ''' <param name="strHallcd">工事依頼書のホールコード</param>
    ''' <returns>True:成功　False:失敗</returns>
    ''' <remarks></remarks>
    Private Function mfUpdTwin(ByVal strHallcd As String) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0
        Dim dsHALLInfo As New DataSet
        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                'ホール情報取得
                cmdDB = New SqlCommand("CNSUPDP001_S30", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, strHallcd))
                End With
                trans = conDB.BeginTransaction()
                cmdDB.Transaction = trans
                dsHALLInfo = clsDataConnect.pfGet_DataSet(cmdDB)

                'データセット取得チェック
                If dsHALLInfo Is Nothing OrElse dsHALLInfo.Tables(0).Rows().Count = 0 Then
                    mfUpdTwin = False 'エラー
                    psMesBox(Me, "30029", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホールが存在しません。", "ホール情報")
                    Throw New Exception
                    Exit Function
                End If
                Dim strTwinData As String = dsHALLInfo.Tables(0).Rows(0).Item("T01_TWIN_CD").ToString.Trim
                Dim strTwinInput As String = txtTwinsShop.ppText.Trim

                '双子区分空欄(又はNULL)は0として扱う
                If strTwinInput = "" Then
                    strTwinInput = "0"
                End If
                If strTwinData = "" Then
                    strTwinData = "0"
                End If

                '双子店区分に差異があれば更新する。
                If strTwinData <> strTwinInput Then
                    cmdDB = New SqlCommand("CNSUPDP001_U11", conDB)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    With cmdDB.Parameters
                        .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, strHallcd))
                        .Add(pfSet_Param("TWIN_CD", SqlDbType.NVarChar, strTwinInput))
                        .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With
                    cmdDB.Transaction = trans
                    cmdDB.ExecuteNonQuery()
                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "双子店区分更新処理")
                        'ロールバック
                        trans.Rollback()
                        Return False
                    End If
                    mfUpdTwin = True
                Else
                    mfUpdTwin = True
                End If
                trans.Commit()
            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "双子店区分更新処理")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                trans.Rollback()
                Return False
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If
    End Function

    ''' <summary>
    ''' MDN機器の更新処理
    ''' </summary>
    ''' <param name="drCnstInfo">工事依頼の情報</param>
    ''' <returns>True:成功　False:失敗</returns>
    ''' <remarks></remarks>
    Private Function mfUpdMDN(ByVal drCnstInfo As DataRow) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0
        Dim dsMDN As New DataSet
        Dim intMDNCnt As Integer = 0
        Dim strMDNCd As String = ""

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '明細（D40）のMDN明細のみ取得
                cmdDB = New SqlCommand("CNSUPDP001_S31", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, drCnstInfo.Item("D39_CNST_NO").ToString.Trim))
                End With
                trans = conDB.BeginTransaction
                cmdDB.Transaction = trans
                dsMDN = clsDataConnect.pfGet_DataSet(cmdDB)

                'データセット取得チェック
                If dsMDN.Tables(0).Rows().Count = 0 Then
                    'MDN機器無しなので0台で登録
                Else
                    intMDNCnt = dsMDN.Tables(0).Rows(0).Item("MDN台数")
                    strMDNCd = dsMDN.Tables(0).Rows(0).Item("MDN機種コード")
                End If

                cmdDB = New SqlCommand("CNSUPDP001_U12", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("TBOX_ID", SqlDbType.NVarChar, drCnstInfo.Item("D39_TBOXID").ToString.Trim))
                    .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, drCnstInfo.Item("D39_HALL_CD").ToString.Trim))
                    .Add(pfSet_Param("MDN_CD", SqlDbType.NVarChar, strMDNCd))
                    .Add(pfSet_Param("MDN_CNT", SqlDbType.Int, intMDNCnt))
                    .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With
                cmdDB.Transaction = trans
                cmdDB.ExecuteNonQuery()
                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
                    'ロールバック
                    trans.Rollback()
                    Return False
                End If
                trans.Commit()
                mfUpdMDN = True
            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                trans.Rollback()
                Return False
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If
    End Function

    ''' <summary>
    ''' MDN機器の戻し処理
    ''' </summary>
    ''' <param name="drCnstInfo">工事依頼の情報</param>
    ''' <returns>True:成功　False:失敗</returns>
    ''' <remarks>前回工事終了時の状態にMDN情報を戻します。
    ''' 処理無し（前回工事データ無し）でもTrueを返します。</remarks>
    Private Function mfUpdMDNRev(ByVal drCnstInfo As DataRow) As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlTransaction = Nothing
        Dim intRtn As Integer = 0
        Dim dsTmp As New DataSet
        Dim strCnstNoBefore As String
        Dim dsMDN As New DataSet
        Dim intMDNCnt As Integer = 0
        Dim strMDNCd As String = ""

        objStack = New StackFrame

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '前回の工事依頼番号を取得する。
                cmdDB = New SqlCommand("CNSUPDP001_S32", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, drCnstInfo.Item("D39_CNST_NO").ToString.Trim))
                    .Add(pfSet_Param("TBOX_ID", SqlDbType.NVarChar, drCnstInfo.Item("D39_TBOXID").ToString.Trim))
                    .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, drCnstInfo.Item("D39_HALL_CD").ToString.Trim))
                End With
                trans = conDB.BeginTransaction
                cmdDB.Transaction = trans
                dsTmp = clsDataConnect.pfGet_DataSet(cmdDB)
                If dsTmp Is Nothing OrElse dsTmp.Tables(0).Rows().Count = 0 Then
                    '前回工事が存在しない為、処理終了
                    Return True
                End If
                strCnstNoBefore = dsTmp.Tables(0).Rows(0).Item("前回工事依頼番号")

                '前回工事の明細（D40）のMDN明細のみ取得
                cmdDB = New SqlCommand("CNSUPDP001_S31", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("CNST_NO", SqlDbType.NVarChar, strCnstNoBefore))
                End With
                cmdDB.Transaction = trans
                dsMDN = clsDataConnect.pfGet_DataSet(cmdDB)

                'データセット取得チェック
                If dsMDN.Tables(0).Rows().Count = 0 Then
                    'MDN機器無しなので0台で登録
                Else
                    intMDNCnt = dsMDN.Tables(0).Rows(0).Item("MDN台数")
                    strMDNCd = dsMDN.Tables(0).Rows(0).Item("MDN機種コード")
                End If

                cmdDB = New SqlCommand("CNSUPDP001_U12", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                With cmdDB.Parameters
                    .Add(pfSet_Param("TBOX_ID", SqlDbType.NVarChar, drCnstInfo.Item("D39_TBOXID").ToString.Trim))
                    .Add(pfSet_Param("HALL_CD", SqlDbType.NVarChar, drCnstInfo.Item("D39_HALL_CD").ToString.Trim))
                    .Add(pfSet_Param("MDN_CD", SqlDbType.NVarChar, strMDNCd))
                    .Add(pfSet_Param("MDN_CNT", SqlDbType.Int, intMDNCnt))
                    .Add(pfSet_Param("USER_ID", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With
                cmdDB.Transaction = trans
                cmdDB.ExecuteNonQuery()
                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)

                If intRtn <> 0 Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
                    'ロールバック
                    trans.Rollback()
                    Return False
                End If
                trans.Commit()
                mfUpdMDNRev = True

            Catch ex As Exception
                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "MDN機種情報更新処理")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                trans.Rollback()
                Return False
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    clsDataConnect.pfClose_Database(conDB)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If
    End Function
    'CNSUPDP001_020 END


#Region "終了処理プロシージャ"
#End Region

End Class

