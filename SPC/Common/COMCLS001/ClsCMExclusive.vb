'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　排他制御処理関連クラス
'*　ＰＧＭＩＤ：　ClsCMExclusive
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.02.19　：　高松
'*  更　新　　：　2014.05.08　：　朝比奈
'*
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
Imports System.Data.SqlClient
Imports System.Data

#End Region


Public Class ClsCMExclusive

#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================

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
    'Private Shared clsDataConnect As New ClsCMDataConnect
    Dim clsDataConnect As New ClsCMDataConnect

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    'Public Overloads Shared Function clsExc.pfSel_Exclusive(ByRef stDate_time As String, _
    '                                       ByRef ipage As Page, _
    '                                       ByVal Ipaddress As String, _
    '                                       ByVal Place As String, _
    '                                       ByVal UserID As String, _
    '                                       ByVal SesstionID As String, _
    '                                       ByVal Group_Num As Integer, _
    '                                       ByVal DispID As String, _
    '                                       ByVal Table_Name As ArrayList, _
    '                                       ByVal Kye As ArrayList
    '                                       ) As Integer
    '    Dim Date_Time As DateTime
    '    Dim strDateMilli As String
    '    '日付の取得
    '    If stDate_time = Nothing Then
    '        Date_Time = DateTime.Now
    '        strDatemilli = Date_Time.ToString("yyyyMMddhhmmss") & Date_Time.Millisecond
    '    Else
    '        Date_Time = Date.Parse(stDate_time)
    '    End If

    '    Return clsExc.pfSel_Exclusive(Date_Time, ipage, Ipaddress, Place, UserID, SesstionID, Group_Num, DispID, Table_Name, Kye)

    'End Function

    '-----------------------------------------------------------------
    ' 排他制御確認・追加処理
    '-----------------------------------------------------------------
    ''' <summary>
    ''' 排他情報確認
    ''' </summary>
    ''' <param name="Date_time"></param>
    ''' <param name="ipage"></param>
    ''' <param name="Ipaddress"></param>
    ''' <param name="Place"></param>
    ''' <param name="UserID"></param>
    ''' <param name="SesstionID"></param>
    ''' <param name="Group_Num"></param>
    ''' <param name="DispID"></param>
    ''' <param name="Table_Name"></param>
    ''' <param name="Kye"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function pfSel_Exclusive(ByRef Date_time As String, _
                                           ByRef ipage As Page, _
                                           ByVal Ipaddress As String, _
                                           ByVal Place As String, _
                                           ByVal UserID As String, _
                                           ByVal SesstionID As String, _
                                           ByVal Group_Num As Integer, _
                                           ByVal DispID As String, _
                                           ByVal Table_Name As ArrayList, _
                                           ByVal Kye As ArrayList
                                           ) As Integer

        '-----------------------------------------------------------------
        '  変数定義　
        '-----------------------------------------------------------------
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim order As DataSet = Nothing
        Dim strKye(15 - 1) As String
        Dim intInfo As Integer = 0
        Dim strOKNG As Integer = 0
        Dim arrySize As Integer = 0
        Dim cntNow As Integer = 1
        Dim dt As DateTime = DateTime.Now
        Dim trans As SqlClient.SqlTransaction  '-- トランザクション
        Dim cmtFlg As Integer = 0 '-- コミットフラグ

        'clsExc.pfSel_Exclusive = 0

        '日付の取得
        Date_time = dt.ToString("yyyyMMddHHmmss") & dt.Millisecond

        ''''Dim Datetime As Date = Date.Now
        ''''Date_time = Datetime.ToString("yyyy-MM-dd HH:mm:ss") + "." + Datetime.Millisecond.ToString
        ''''

        'ループ回数を取得
        arrySize = Table_Name.Count

        Try
            pfSel_Exclusive = 0

            If clsDataConnect.pfOpen_Database(conDB) Then
                trans = conDB.BeginTransaction

                Try

                    '-- 排他情報確認処理を開始 --
                    For Each strTable_Name As String In Table_Name

                        '-- キー項目の初期化 --
                        For i As Integer = 0 To strKye.Count - 1
                            strKye(i) = ""
                        Next

                        '-----------------------------------------
                        ' 画面、テーブル毎に設定
                        '-----------------------------------------
                        Select Case DispID

                            Case "BRKUPDP001" '-- ミニ処理表(故障･受付業務)

                                Select Case strTable_Name.ToString

                                    Case "D77_MINI_MANAGE"
                                        '>ミニ処理票　
                                        strKye(0) = Kye(0) 'D77_MNG_NO
                                        'strKye(1) = Kye(1) 'D77_MNG_ORDER

                                    Case "D78_MINIMNG_DTIL"
                                        '>ミニ処理票明細　
                                        strKye(0) = Kye(0) 'D78_MNG_NO
                                        strKye(1) = Kye(1) 'D78_MNG_ORDER
                                        strKye(2) = Kye(2) 'D78_MNG_SEQ

                                End Select


                            Case "CDPLSTP001" '-- 使用中カードDB吸上一覧(SC業務)

                                '>使用中DB吸上げ（D22_USINGDB）
                                strKye(0) = Kye(0) 'D22_REGIST_SEQ


                            Case "CMPINQP001" '-- 特別保守費用照会(保守業務)
                                '>D75_DEAL_MAINTAIN(保守対応)
                                strKye(0) = Kye(0) 'D75_MNT_NO


                            Case "CMPSELP001" '-- 保守対応依頼書(保守業務)　

                                Select Case strTable_Name.ToString

                                    Case "D75_DEAL_MAINTAIN"
                                        '>保守対応
                                        strKye(0) = Kye(0) 'D75_MNT_NO

                                    Case "D76_DEALMAINTAIN_DTIL"
                                        '>保守対応依頼明細
                                        strKye(0) = Kye(0) 'D76_MNT_NO
                                        'strKye(1) = Kye(1) 'D76_MNT_SEQ

                                    Case "D73_TROUBLE"
                                        '>トラブル
                                        strKye(0) = Kye(0) 'D73_TRBL_NO

                                End Select


                            Case "CNSINQP001" '-- 工事料金明細書(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D27_CNST_AMOUNT"
                                        '>工事料金
                                        strKye(0) = Kye(0) 'D27_CNTL_NO

                                    Case "D28_CNST_AMOUNT_DTL"
                                        '>工事料金明細
                                        strKye(0) = Kye(0) 'D87_CONST_NO

                                    Case "D05_CNSTBREAK_DTL"
                                        '>工事完了報告明細(TMSDB)
                                        strKye(0) = Kye(0) 'D05_CONST_NO

                                    Case "D06_CNSTAMNT_DTL"
                                        '>工事料金明細(TMSDB)
                                        strKye(0) = Kye(0) 'D06_CONST_NO

                                End Select


                            Case "CNSUPDP001" '-- 工事依頼書兼仕様書(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D39_CNSTREQSPEC"
                                        '>工事依頼書兼仕様書

                                        strKye(0) = Kye(0) 'D39_CNST_NO　

                                    Case "D19_ARTCLTRNS"
                                        '>物品転送依頼　
                                        strKye(0) = Kye(0) 'D19_ARTCL_NO 

                                    Case "D20_ARTCLTRNS_DTL"
                                        '<物品転送依頼明細 
                                        strKye(0) = Kye(0) 'D20_ARTCL_NO
                                        strKye(1) = Kye(1) 'D20_SEQNO

                                        '--------------------------------
                                        '2015/04/03 加賀　ここから
                                        '--------------------------------
                                    Case "D24_CNST_SITU_DTL"
                                        strKye(0) = Kye(0) 'D24_CONST_NO
                                        '--------------------------------
                                        '2015/04/03 加賀　ここまで
                                        '--------------------------------

                                End Select


                            Case "CNSUPDP002"   '-- 物品転送依頼書 参照/更新(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D19_ARTCLTRNS"
                                        '>物品転送依頼
                                        strKye(0) = Kye(0) 'D19_ARTCL_NO

                                    Case "D20_ARTCLTRNS_DTL"
                                        '>物品転送依頼明細
                                        strKye(0) = Kye(0) 'D20_ARTCL_NO
                                        strKye(1) = Kye(1) 'D20_SEQNO

                                    Case "D39_CNSTREQSPEC"
                                        strKye(0) = Kye(0)
                                End Select


                            Case "CNSUPDP003" '-- 工事進捗 参照/更新(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D24_CNST_SITU_DTL"
                                        '>工事状況詳細
                                        strKye(0) = Kye(0) 'D24_CONST_NO

                                        '-----------------------------------------
                                        ' 5/29 後藤　ここから
                                        '-----------------------------------------
                                        'Select Case Kye.Count
                                        '    Case 1
                                        '        strKye(0) = Kye(0) 'D24_CONST_NO
                                        '    Case 2
                                        '        strKye(0) = Kye(0) 'D24_CONST_NO
                                        '        strKye(1) = Kye(1) 'D24_EST_CLS or D24_SEQNO
                                        '    Case 3
                                        '        strKye(0) = Kye(0) 'D24_CONST_NO
                                        '        strKye(1) = Kye(1) 'D24_EST_CLS
                                        '        strKye(2) = Kye(2) 'D24_SEQNO
                                        'End Select

                                        'strKye(1) = Kye(1) 'D24_EST_CLS
                                        'strKye(2) = Kye(2) 'D24_SEQNO
                                        '-----------------------------------------
                                        ' 5/29 後藤　ここまで
                                        '-----------------------------------------

                                    Case "D39_CNSTREQSPEC"
                                        '>工事依頼書兼仕様書
                                        strKye(0) = Kye(0) 'D39_CNST_NO

                                    Case "D84_ANYTIME_LIST"
                                        '>随時一覧　
                                        strKye(0) = Kye(1) 'D84_TBOXID
                                        strKye(1) = Kye(2) 'D84_CNST_DT

                                End Select


                            Case "CNSUPDP004" '-- 工事連絡票 参照/更新(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D26_CNST_COMM_DTL"
                                        '>工事連絡票明細
                                        strKye(0) = Kye(0) 'D26_CNST_NO
                                        strKye(1) = Kye(1) 'D26_DTL_CLS
                                        strKye(2) = Kye(2) 'D26_SEQNO
                                        'strKye(3) = Kye(3) 'D26_DT
                                        'strKye(4) = Kye(4) 'D26_CHARGE
                                        'strKye(5) = Kye(5) 'D26_CONTENT

                                    Case "D25_CNST_COMM"
                                        '>工事連絡票
                                        strKye(0) = Kye(0) 'D25_COMM_NO

                                End Select


                            Case "CNSUPDP005" '-- 請求資料 状況更新(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D57_CNSTREQSPEC_DTL3"
                                        '>工事依頼書兼仕様書　請求資料明細

                                        '-----------------------------------------
                                        ' 5/29 後藤　ここから
                                        '-----------------------------------------

                                        'strKye(0) = Kye(0) 'D57_CNST_NO
                                        ''strKye(1) = Kye(1) 'D57_SEQ

                                        Select Case Kye.Count
                                            Case 1
                                                strKye(0) = Kye(0) 'D24_CONST_NO
                                            Case 2
                                                strKye(0) = Kye(0) 'D57_CNST_NO
                                                strKye(1) = Kye(1) 'D57_SEQ
                                        End Select
                                        '-----------------------------------------
                                        ' 5/29 後藤　ここまで
                                        '-----------------------------------------

                                    Case "D39_CNSTREQSPEC"
                                        '>工事依頼書兼仕様書
                                        strKye(0) = Kye(0) 'D39_CNST_NO

                                End Select

                            Case "CNSUPDP006" '-- 工事完了報告書(工事業務)
                                '>工事完了報告明細
                                strKye(0) = Kye(0) 'D05_CONST_NO

                            Case "DLCINPP001" '-- アップロード(共通)
                                '>工事依頼書兼仕様書(D39_CNSTREQSPEC)
                                strKye(0) = Kye(0) 'D39_CNST_NO

                            Case "DLCLSTP001" '-- DLL設定変更依頼一覧(監視業務)
                                '>運用モード設定変更(D47_DLLSEND)
                                strKye(0) = Kye(0) 'D47_DLLSEND_NO
                                strKye(1) = Kye(1) 'D47_SEQNO

                            Case "DOCMENP001" '-- 検収書作成(SC業務)
                                '>サポートセンタ検収書(D91_SPC_INSPECTION)
                                strKye(0) = Kye(0) 'D91_DEGREE

                            Case "DOCUPDP001" '-- 請求書作成(SC業務)

                                Select Case strTable_Name.ToString

                                    Case "D88_MNT_INSPECTION"
                                        '>保守検収
                                        strKye(0) = Kye(0) 'D88_DEGREE

                                    Case "D37_BILL"
                                        '>請求書
                                        strKye(0) = Kye(0) 'D37_BILL_DTD
                                        strKye(1) = Kye(1) 'D37_BILL_NO

                                End Select

                            Case "DSUUPDP001" '-- DSU交換対応依頼書 参照/更新(監視業務)
                                '>DSU交換報告(D35_DSUREPLC)
                                strKye(0) = Kye(0) 'D35_GC_REPORT_NO

                            Case "EQULSTP001" '-- 配送予備一覧(保守業務)
                                '>配送機器一覧(D79_DELIVERY_EQUIP)　
                                strKye(0) = Kye(0) 'D79_PBRN_NO

                            Case "ERRSELP001" '-- 集信エラーホール一覧(監視業務)
                                '>無応答(D51_NOREPLY)
                                strKye(0) = Kye(0) 'D51_NOREPLY_NO

                            Case "MNTSELP001" '-- 修理整備・整備進捗明細(修理･整備業務)

                                Select Case strTable_Name.ToString

                                    Case "D232_MENTE_REQUEST_DTL"
                                        '>整備作業依頼明細
                                        strKye(0) = Kye(0) 'D232_MENTE_NO
                                        strKye(1) = Kye(1) 'D232_BRANCH

                                    Case "D82_MENTEPARTS_DTIL"
                                        '>整備作業依頼書部品明細
                                        strKye(0) = Kye(0) 'D82_REPAIR_NO
                                        strKye(1) = Kye(1) 'D82_BRANCH

                                        '-- 場合によってSeqがないときがある --
                                        If Kye.Count = 4 Then
                                            strKye(2) = Kye(2) 'D82_SEQ
                                        End If

                                End Select

                            Case "MNTUPDP001" '-- 修理整備・整備依頼書(修理･整備業務)

                                Select Case strTable_Name.ToString

                                    Case "D23_MENTE_REQUEST"
                                        '>整備作業依頼
                                        strKye(0) = Kye(0) 'D23_MENTE_NO

                                    Case "D232_MENTE_REQUEST_DTL"
                                        '>整備作業依頼明細
                                        strKye(0) = Kye(0) 'D232_MENTE_NO
                                        strKye(1) = Kye(1) 'D232_BRANCH

                                    Case "D82_MENTEPARTS_DTIL"
                                        '>整備作業依頼部品明細
                                        strKye(0) = Kye(0) 'D82_REPAIR_NO
                                        strKye(1) = Kye(1) 'D82_BRANCH
                                        'strKye(2) = Kye(2) 'D82_SEQ

                                End Select


                            Case "OVELSTP002" '-- 時間外消費ホール詳細(監視業務)
                                '>運用時間外使用情報(D168_JIKANGAI(CNT_DB))
                                '-----------------------------------------
                                ' 6/08 後藤　ここから
                                '-----------------------------------------
                                Select Case Kye.Count
                                    Case 1
                                        strKye(0) = Kye(0) 'D168_CTRL_NO
                                    Case Else
                                        strKye(0) = Kye(0) 'D168_CTRL_NO
                                        strKye(1) = Kye(1) 'D168_SEQ
                                        strKye(2) = Kye(2) 'D168_NL_CLS
                                        strKye(3) = Kye(3) 'D168_ID_IC_CLS
                                        strKye(4) = Kye(4) 'D168_RECVDATE
                                        strKye(5) = Kye(5) 'D168_RECVSEQ
                                End Select
                                '-----------------------------------------
                                ' 6/08 後藤　ここまで
                                '-----------------------------------------

                            Case "REPUPDP001"  '-- 修理整備･修理有償部品費用(修理･整備依頼書)
                                '>部品マスタ(M59_PARTS)
                                strKye(0) = Kye(0) 'M59_MAKER
                                strKye(1) = Kye(1) 'M59_WRK_CLS
                                strKye(2) = Kye(2) 'M59_PARTS_CD


                            Case "REPUPDP002" '-- 修理依頼書(修理･整備依頼書)

                                Select Case strTable_Name.ToString

                                    Case "D80_REPAIR_DTIL"
                                        '>修理依頼明細(D80_REPAIR_DTIL)
                                        strKye(0) = Kye(0) 'D80_REPAIR_NO
                                        strKye(1) = Kye(1) 'D80_BRANCH
                                        'strKye(2) = Kye(2) 'D80_SEQNO

                                    Case "D29_REPAIR"
                                        '>修理依頼(D29_REPAIR)
                                        strKye(0) = Kye(0) 'D29_REPAIR_NO
                                        strKye(1) = Kye(1) 'D29_BRANCH

                                End Select


                            Case "REQSELP001" '-- トラブル処理(保守業務)

                                Select Case strTable_Name.ToString

                                    Case "D73_TROUBLE"
                                        '>トラブル(D73_TROUBLE)
                                        strKye(0) = Kye(0) 'D73_TRBL_NO

                                    Case "D74_TROUBLE_DTIL"
                                        '>トラブル明細(D74_TROUBLE_DTIL)
                                        strKye(0) = Kye(0) 'D74_TRBL_NO
                                        strKye(1) = Kye(1) 'D74_TRBL_SEQ

                                End Select


                            Case "SERUPDP001" '-- シリアル登録(工事業務)

                                Select Case strTable_Name.ToString

                                    Case "D60_SERIALMNG"
                                        '>シリアル管理(D60_SERIALMNG)
                                        strKye(0) = Kye(0) 'D60_SERIAL_NO
                                        strKye(1) = Kye(1) 'D60_SYSTEM_CD
                                        strKye(2) = Kye(2) 'D60_APPADIV_NM
                                        strKye(3) = Kye(3) 'D60_APPACLASS_NM
                                        strKye(4) = Kye(4) 'D60_APPA_NM

                                    Case "D42_SERIALHST"
                                        '>シリアル履歴(D42_SERIALHST)
                                        strKye(0) = Kye(0) 'D42_SERIAL_NO
                                        strKye(1) = Kye(1) 'D42_SYSTEM_CD
                                        strKye(2) = Kye(2) 'D42_APPADIV_NM
                                        strKye(3) = Kye(3) 'D42_APPACLASS_NM
                                        strKye(4) = Kye(4) 'D42_APPA_NM
                                        strKye(5) = Kye(5) 'D42_SET_DT

                                End Select

                            Case "SLFLSTP002" '-- 券売入金自走ホール一覧(監視業務)
                                '>自走中情報(D173_KENBAIKIJISOU(CNT_DB))
                                '-----------------------------------------
                                ' 6/11 後藤　ここから
                                '-----------------------------------------
                                Select Case Kye.Count
                                    Case 1
                                        strKye(0) = Kye(0) 'D173_CTRL_NO
                                    Case Else
                                        strKye(0) = Kye(0) 'D173_CTRL_NO
                                        strKye(1) = Kye(1) 'D173_SEQ
                                        strKye(2) = Kye(2) 'D173_NL_CLS
                                        strKye(3) = Kye(3) 'D173_ID_IC_CLS
                                        strKye(4) = Kye(4) 'D173_TBOXID
                                        strKye(5) = Kye(5) 'D173_JBNUMERIC
                                End Select
                                '-----------------------------------------
                                ' 6/11 後藤　ここまで
                                '-----------------------------------------

                            Case "TBPINPP001" '-- 随時集信一覧状況入力(SC業務)　
                                '随時一覧(D84_ANYTIME_LIST)
                                strKye(0) = Kye(0) 'D84_TBOXID
                                strKye(1) = Kye(1) 'D84_CNST_DT


                            Case "WATLSTP002" '-- 監視対象外ホール一覧(監視業務)
                                '>監視対象外ホール情報(T02_HALL_EXCLD)
                                strKye(0) = Kye(0)  'T02_TBOXID
                                'strKye(1) = Kye(1)  'T02_NL_CLS
                                'strKye(2) = Kye(2)  'T02_SYSTEM_CLS


                            Case "WATUPDP001"    '-- 監視報告書兼依頼票 参照/更新(監視業務)
                                '監視報告書兼依頼書(D36_MNTRREPORT)
                                strKye(0) = Kye(0) 'D36_MNTRREPORT_NO


                            Case "QUAUPDP001"    '-- 品質会議資料明細 
                                '>品質会議資料(D86_MEETING)
                                strKye(0) = Kye(0) 'D86_NO
                                strKye(1) = Kye(1) 'D86_MEETING_NO

                            Case "BBPUPDP001"    '--ブラックボックス調査報告書(保守業務)
                                'ブラックボックス調査報告書(D50_BBINVESTREPORT)
                                strKye(0) = Kye(0) 'D50_BBREP_NO

                            Case "D174_HEALTH"    '--ヘルスチェック
                                'ヘルスチェック(D174_HEALTH)
                                strKye(0) = Kye(0)
                                strKye(1) = Kye(1)
                                strKye(2) = Kye(2)
                                strKye(3) = Kye(3)
                                strKye(4) = Kye(4)
                                strKye(5) = Kye(5)

                            Case "D180_HC_INVEST_HISTRY"    '--ヘルスチェック調査履歴
                                'ヘルスチェック調査履歴(D180_HC_INVEST_HISTRY)
                                strKye(0) = Kye(0)
                                strKye(1) = Kye(1)
                                strKye(2) = Kye(2)
                                strKye(3) = Kye(3)
                                strKye(4) = Kye(4)
                                strKye(5) = Kye(5)


                            Case "COMMENP006" '-- シリアル登録(工事業務)
                                Select Case strTable_Name.ToString

                                    Case "T01_HALL"    '--ホール情報
                                        'ホール情報(T01_HALL)
                                        strKye(0) = Kye(0) 'T01_HALL_CD

                                    Case "T04_HALL_COMM"    '-- ホール連絡先情報
                                        '>ホール連絡先情報(T04_HALL_COMM)
                                        strKye(0) = Kye(0) 'T04_HALL_CD

                                End Select

                                '---------------------------
                                '2015/12/22 加賀 ここから
                                '---------------------------
                            Case Else '-- マスタ管理画面

                                For i As Integer = 0 To Kye.Count - 1
                                    strKye(i) = Kye(i)
                                Next


                                '---------------------------
                                '2015/12/22 加賀 ここまで
                                '---------------------------
                        End Select

                        cmdDB = New SqlCommand("ClsCMExclusive_S1", conDB)
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters

                            '-------------------------------------------
                            ' パラメータ設定
                            '-------------------------------------------
                            .Add(pfSet_Param("prm_Table_Name", SqlDbType.NVarChar, strTable_Name))                  '-- テーブル名
                            .Add(pfSet_Param("prm_key_0", SqlDbType.NVarChar, strKye(0)))                           '-- キー情報_1
                            .Add(pfSet_Param("prm_key_1", SqlDbType.NVarChar, strKye(1)))                           '-- キー情報_2
                            .Add(pfSet_Param("prm_key_2", SqlDbType.NVarChar, strKye(2)))                           '-- キー情報_3
                            .Add(pfSet_Param("prm_key_3", SqlDbType.NVarChar, strKye(3)))                           '-- キー情報_4
                            .Add(pfSet_Param("prm_key_4", SqlDbType.NVarChar, strKye(4)))                           '-- キー情報_5
                            .Add(pfSet_Param("prm_key_5", SqlDbType.NVarChar, strKye(5)))                           '-- キー情報_6
                            .Add(pfSet_Param("prm_key_6", SqlDbType.NVarChar, strKye(6)))                           '-- キー情報_7
                            .Add(pfSet_Param("prm_key_7", SqlDbType.NVarChar, strKye(7)))                           '-- キー情報_8
                            .Add(pfSet_Param("prm_key_8", SqlDbType.NVarChar, strKye(8)))                           '-- キー情報_9
                            .Add(pfSet_Param("prm_key_9", SqlDbType.NVarChar, strKye(9)))                           '-- キー情報_10
                            .Add(pfSet_Param("prm_key_10", SqlDbType.NVarChar, strKye(10)))                         '-- キー情報_11
                            .Add(pfSet_Param("prm_key_11", SqlDbType.NVarChar, strKye(11)))                         '-- キー情報_12
                            .Add(pfSet_Param("prm_key_12", SqlDbType.NVarChar, strKye(12)))                         '-- キー情報_13
                            .Add(pfSet_Param("prm_key_13", SqlDbType.NVarChar, strKye(13)))                         '-- キー情報_14
                            .Add(pfSet_Param("prm_key_14", SqlDbType.NVarChar, strKye(14)))                         '-- キー情報_15
                            .Add(pfSet_Param("prm_Data_Exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))  '-- 結果

                        End With

                        '-------------------------------------------
                        '  結果を取得
                        '-------------------------------------------
                        order = clsDataConnect.pfGet_DataSet(cmdDB)
                        strOKNG = cmdDB.Parameters("prm_Data_Exist").Value

                        Select Case strOKNG

                            Case 0 '該当件数なし
                                '-------------------------------
                                '  排他情報を登録
                                '-------------------------------
                                If fIns_Exclusive(Ipaddress, _
                                                   Place, _
                                                   UserID, _
                                                   SesstionID, _
                                                   Group_Num, _
                                                   DispID, _
                                                   strTable_Name, _
                                                   strKye, _
                                                   Date_time, _
                                                   cmdDB, _
                                                   conDB, _
                                                   arrySize, _
                                                   cntNow, _
                                                   trans) = 9 Then
                                    Throw New Exception
                                End If

                                pfSel_Exclusive = 0
                                cntNow += 1
                                cmtFlg += 1

                            Case 1 '該当件数あり
                                '-------------------------------
                                '  ＩＤ確認処理
                                '-------------------------------
                                intInfo = fCheck_UserInfo(UserID, _
                                                           SesstionID, _
                                                           Group_Num, _
                                                           DispID, _
                                                           strTable_Name, _
                                                           strKye, _
                                                           cmdDB, _
                                                           conDB, _
                                                           trans)
                                Select Case intInfo

                                    Case 0 '該当件数なし
                                        '--------------------------------
                                        '2015/04/03 加賀　ここから
                                        '--------------------------------
                                        '工事依頼書兼仕様書・工事進捗　参照更新　の場合ユーザー情報を確認しない
                                        Select Case DispID
                                            Case "CNSUPDP001", "CNSUPDP003"
                                                pfSel_Exclusive = 1
                                                cntNow += 1
                                                psMesBox(ipage, "30012", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)

                                                Exit For
                                        End Select
                                        '--------------------------------
                                        '2015/04/03 加賀　ここまで
                                        '--------------------------------

                                        '-------------------------------
                                        '  排他情報を登録
                                        '-------------------------------
                                        If fIns_Exclusive(Ipaddress, _
                                                           Place, _
                                                           UserID, _
                                                           SesstionID, _
                                                           Group_Num, _
                                                           DispID, _
                                                           strTable_Name, _
                                                           strKye, _
                                                           Date_time, _
                                                           cmdDB, _
                                                           conDB, _
                                                           arrySize, _
                                                           cntNow, _
                                                           trans) = 9 Then
                                            Throw New Exception
                                        End If

                                        pfSel_Exclusive = 0
                                        cntNow += 1
                                        cmtFlg += 1

                                    Case 1 '該当件数あり
                                        pfSel_Exclusive = 1
                                        cntNow += 1
                                        psMesBox(ipage, "30010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)

                                        Exit For

                                    Case 9 'エラー
                                        pfSel_Exclusive = 9
                                        cntNow += 1


                                        Exit For

                                    Case Else '想定外のエラー
                                        pfSel_Exclusive = 9
                                        cntNow += 1

                                        Exit For

                                End Select
                                '-- メッセージ --
                                'Exit For

                            Case 9
                                '-- 異常終了
                                pfSel_Exclusive = 9

                                Exit For

                            Case Else
                                '-- 例外
                                pfSel_Exclusive = 9

                                Exit For

                        End Select

                    Next

                    If cmtFlg = arrySize Then '正常
                        trans.Commit()
                    Else               '例外
                        trans.Rollback()
                    End If

                Catch ex As Exception

                    pfSel_Exclusive = 9
                    '-- ロールバック --
                    trans.Rollback()

                    '-- エラーメッセージ --
                    psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
                    psClose_Window(ipage)

                Finally

                    '-- DB切断 --
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        '-- エラーメッセージ --
                        psMesBox(ipage, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    End If

                End Try

            Else
                pfSel_Exclusive = 9
                '---------------------------
                '2014/06/06 武 ここから
                '---------------------------
                ''-- ロールバック --
                'trans.Rollback()
                '---------------------------
                '2014/06/06 武 ここまで
                '---------------------------
                '-- エラーメッセージの出力 --
                psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
                Throw New Exception

            End If

        Catch ex As Exception
            pfSel_Exclusive = 9
            '---------------------------
            '2014/06/06 武 ここから
            '---------------------------
            ''-- ロールバック --
            'trans.Rollback()
            '---------------------------
            '2014/06/06 武 ここまで
            '---------------------------
            '-- エラーメッセージの出力 --
            psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
            psClose_Window(ipage)

        End Try

    End Function

    '-------------------------------------------------------------------
    '  ＩＤ確認処理
    '-------------------------------------------------------------------
    ''' <summary>
    ''' ID確認処理
    ''' </summary>
    ''' <param name="UserID"></param>
    ''' <param name="SesstionID"></param>
    ''' <param name="Group_Num"></param>
    ''' <param name="DispID"></param>
    ''' <param name="Table_Name"></param>
    ''' <param name="strKye"></param>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fCheck_UserInfo(ByVal UserID As String, _
                                            ByVal SesstionID As String, _
                                            ByVal Group_Num As Integer, _
                                            ByVal DispID As String, _
                                            ByVal Table_Name As String, _
                                            ByVal strKye() As String, _
                                            ByVal cmdDB As SqlCommand, _
                                            ByVal conDB As SqlConnection, _
                                            ByRef trans As SqlClient.SqlTransaction) As Integer

        '-- 変数定義 --
        Dim order As DataSet = Nothing
        Dim strOKNG As Integer = 0

        fCheck_UserInfo = 0

        Try
            cmdDB = New SqlCommand("ClsCMExclusive_S2", conDB)
            cmdDB.Transaction = trans

            With cmdDB.Parameters
                '-------------------------------------------
                ' パラメータ設定
                '-------------------------------------------
                .Add(pfSet_Param("prm_User_ID", SqlDbType.NVarChar, UserID))                            '-- ユーザID
                .Add(pfSet_Param("prm_Sesstion_ID", SqlDbType.NVarChar, SesstionID))                    '-- セッションID
                .Add(pfSet_Param("prm_Group_Num", SqlDbType.Int, Group_Num))                      '-- グループ番号
                .Add(pfSet_Param("prm_Disp_ID", SqlDbType.NVarChar, DispID))                            '-- DispID
                .Add(pfSet_Param("prm_Table_Name", SqlDbType.NVarChar, Table_Name))                     '-- テーブル名
                .Add(pfSet_Param("prm_key_0", SqlDbType.NVarChar, strKye(0)))                           '-- キー情報_1
                .Add(pfSet_Param("prm_key_1", SqlDbType.NVarChar, strKye(1)))                           '-- キー情報_2
                .Add(pfSet_Param("prm_key_2", SqlDbType.NVarChar, strKye(2)))                           '-- キー情報_3
                .Add(pfSet_Param("prm_key_3", SqlDbType.NVarChar, strKye(3)))                           '-- キー情報_4
                .Add(pfSet_Param("prm_key_4", SqlDbType.NVarChar, strKye(4)))                           '-- キー情報_5
                .Add(pfSet_Param("prm_key_5", SqlDbType.NVarChar, strKye(5)))                           '-- キー情報_6
                .Add(pfSet_Param("prm_key_6", SqlDbType.NVarChar, strKye(6)))                           '-- キー情報_7
                .Add(pfSet_Param("prm_key_7", SqlDbType.NVarChar, strKye(7)))                           '-- キー情報_8
                .Add(pfSet_Param("prm_key_8", SqlDbType.NVarChar, strKye(8)))                           '-- キー情報_9
                .Add(pfSet_Param("prm_key_9", SqlDbType.NVarChar, strKye(9)))                           '-- キー情報_10
                .Add(pfSet_Param("prm_key_10", SqlDbType.NVarChar, strKye(10)))                         '-- キー情報_11
                .Add(pfSet_Param("prm_key_11", SqlDbType.NVarChar, strKye(11)))                         '-- キー情報_12
                .Add(pfSet_Param("prm_key_12", SqlDbType.NVarChar, strKye(12)))                         '-- キー情報_13
                .Add(pfSet_Param("prm_key_13", SqlDbType.NVarChar, strKye(13)))                         '-- キー情報_14
                .Add(pfSet_Param("prm_key_14", SqlDbType.NVarChar, strKye(14)))                         '-- キー情報_15
                .Add(pfSet_Param("prm_Data_Exist", SqlDbType.Int, 20, ParameterDirection.Output))  '-- 結果
            End With

            order = clsDataConnect.pfGet_DataSet(cmdDB)
            strOKNG = cmdDB.Parameters("prm_Data_Exist").Value

            Select Case strOKNG

                Case 0
                    '-- 正常 --
                    fCheck_UserInfo = 0

                Case 1
                    '-- 排他ロック中 --
                    fCheck_UserInfo = 1

                Case 9
                    '-- 異常 --
                    fCheck_UserInfo = 9

                Case Else
                    '-- 例外エラー --
                    fCheck_UserInfo = 9

            End Select

        Catch ex As Exception

            '-- 異常 --
            fCheck_UserInfo = 9

        End Try

    End Function

    '-------------------------------------------------------------------
    '  排他情報登録
    '-------------------------------------------------------------------
    ''' <summary>
    ''' 排他情報登録
    ''' </summary>
    ''' <param name="Ipaddress"></param>
    ''' <param name="Place"></param>
    ''' <param name="UserID"></param>
    ''' <param name="SesstionID"></param>
    ''' <param name="Group_Num"></param>
    ''' <param name="DispID"></param>
    ''' <param name="Table_Name"></param>
    ''' <param name="strKye"></param>
    ''' <param name="Date_Time"></param>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function fIns_Exclusive(ByVal Ipaddress As String, _
                                           ByVal Place As String, _
                                           ByVal UserID As String, _
                                           ByVal SesstionID As String, _
                                           ByVal Group_Num As Integer, _
                                           ByVal DispID As String, _
                                           ByVal Table_Name As String, _
                                           ByVal strKye() As String, _
                                           ByVal Date_Time As String, _
                                           ByVal cmdDB As SqlCommand, _
                                           ByVal conDB As SqlConnection, _
                                           ByVal arrySize As Integer, _
                                           ByVal cntNow As Integer, _
                                           ByRef trans As SqlClient.SqlTransaction) As Integer

        '-- 変数定義 --
        'Dim trans As SqlClient.SqlTransaction  '-- トランザクション
        Dim order As DataSet = Nothing
        Dim strOKNG As Integer = 0

        fIns_Exclusive = 0
        'If cntNow = 1 Then
        '    trans = conDB.BeginTransaction
        'Else
        'End If

        Try

            cmdDB = New SqlCommand("ClsCMExclusive_I1", conDB)
            cmdDB.CommandType = CommandType.StoredProcedure
            cmdDB.Transaction = trans

            With cmdDB.Parameters

                '-- パラメータ設定 --
                .Add(pfSet_Param("prm_IpAddress", SqlDbType.NVarChar, Ipaddress))      '-- IPアドレス
                .Add(pfSet_Param("prm_Place", SqlDbType.NVarChar, Place))              '-- 
                .Add(pfSet_Param("prm_User_ID", SqlDbType.NVarChar, UserID))           '-- テーブル名
                .Add(pfSet_Param("prm_Sesstion_ID", SqlDbType.NVarChar, SesstionID))   '-- セッションID
                .Add(pfSet_Param("prm_Group_SEQ", SqlDbType.Int, Group_Num))       '-- グループ番号
                .Add(pfSet_Param("prm_Disp_ID", SqlDbType.NVarChar, DispID))           '-- 画面ID
                .Add(pfSet_Param("prm_Table_Name", SqlDbType.NVarChar, Table_Name))    '-- テーブル名
                .Add(pfSet_Param("prm_key_0", SqlDbType.NVarChar, strKye(0)))          '-- キー情報_1
                .Add(pfSet_Param("prm_key_1", SqlDbType.NVarChar, strKye(1)))          '-- キー情報_2
                .Add(pfSet_Param("prm_key_2", SqlDbType.NVarChar, strKye(2)))          '-- キー情報_3
                .Add(pfSet_Param("prm_key_3", SqlDbType.NVarChar, strKye(3)))          '-- キー情報_4
                .Add(pfSet_Param("prm_key_4", SqlDbType.NVarChar, strKye(4)))          '-- キー情報_5
                .Add(pfSet_Param("prm_key_5", SqlDbType.NVarChar, strKye(5)))          '-- キー情報_6
                .Add(pfSet_Param("prm_key_6", SqlDbType.NVarChar, strKye(6)))          '-- キー情報_7
                .Add(pfSet_Param("prm_key_7", SqlDbType.NVarChar, strKye(7)))          '-- キー情報_8
                .Add(pfSet_Param("prm_key_8", SqlDbType.NVarChar, strKye(8)))          '-- キー情報_9
                .Add(pfSet_Param("prm_key_9", SqlDbType.NVarChar, strKye(9)))          '-- キー情報_10
                .Add(pfSet_Param("prm_key_10", SqlDbType.NVarChar, strKye(10)))        '-- キー情報_11
                .Add(pfSet_Param("prm_key_11", SqlDbType.NVarChar, strKye(11)))        '-- キー情報_12
                .Add(pfSet_Param("prm_key_12", SqlDbType.NVarChar, strKye(12)))        '-- キー情報_13
                .Add(pfSet_Param("prm_key_13", SqlDbType.NVarChar, strKye(13)))        '-- キー情報_14
                .Add(pfSet_Param("prm_key_14", SqlDbType.NVarChar, strKye(14)))        '-- キー情報_15
                .Add(pfSet_Param("prm_Date_Time", SqlDbType.NVarChar, Date_Time))     '-- 登録用日付
                .Add(pfSet_Param("prm_Data_Exist", SqlDbType.Int, 20, ParameterDirection.Output))  '-- 結果

            End With

            '-- コマンドタイプ設定(ストアド) --
            cmdDB.ExecuteNonQuery()
            strOKNG = cmdDB.Parameters("prm_Data_Exist").Value

            '-- ストアドの結果判断 --
            Select Case strOKNG
                Case 0
                    '-- 正常 --
                    fIns_Exclusive = 0

                    ''-- コミット --
                    'If arrySize = cntNow Then
                    '    'trans.Commit()
                    'End If

                Case 9
                    '-- 異常 --
                    Throw New Exception

                Case Else
                    '-- 例外エラー -- 
                    Throw New Exception

            End Select

        Catch ex As Exception

            '-- ロールバック --
            'trans.Rollback()
            fIns_Exclusive = 9

        End Try

    End Function

    '-------------------------------------------------------------------
    '  ログイン情報の確認
    '-------------------------------------------------------------------
    ''' <summary>
    ''' ログイン確認
    ''' </summary>
    ''' <param name="ipage"></param>
    ''' <param name="SesstionID"></param>
    ''' <param name="LoginDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fCheck_Login(ByRef ipage As Page, _
                                         ByVal SesstionID As String, _
                                         ByVal LoginDate As String)


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim order As DataSet = Nothing
        Dim strOKNG As String = Nothing

        fCheck_Login = 0

        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand("ClsCMExclusive_S3", conDB)

                With cmdDB.Parameters
                    '-- パラメータ設定 --
                    .Add(pfSet_Param("prm_Sesstion_ID", SqlDbType.NVarChar, SesstionID))                   '-- セッションID
                    .Add(pfSet_Param("prm_User_ID", SqlDbType.NVarChar, LoginDate))                        '-- 登録日時
                    .Add(pfSet_Param("prm_Data_Exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '-- 結果

                End With

                order = clsDataConnect.pfGet_DataSet(cmdDB)
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                '-- 結果を取得 --
                Select Case strOKNG

                    Case "0"
                        '-- 正常 --
                        fCheck_Login = 0

                    Case "9"
                        '-- 異常 --
                        Throw New Exception

                    Case Else
                        '-- 例外エラー --
                        Throw New Exception

                End Select

            Catch ex As Exception
                '-- 異常 --
                fCheck_Login = 9
                psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
            End Try

        Else
            '-- エラーメッセージの出力 --
            psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
            fCheck_Login = 9
            psClose_Window(ipage)
        End If

    End Function
    '---------------------------
    '2014/06/10 後藤 ここから
    '---------------------------
    '-------------------------------------------------------------------
    '  排他情報削除
    '-------------------------------------------------------------------
    ''' <summary>
    ''' 排他情報削除(画面用)
    ''' </summary>
    ''' <param name="ipage"></param>
    ''' <param name="SesstionID"></param>
    ''' <param name="Date_Time"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDel_Exclusive(ByVal ipage As Page, _
                                                     ByVal SesstionID As String, _
                                                     ByVal Date_Time As String) As Integer


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim order As DataSet = Nothing
        Dim strOKNG As String = Nothing
        Dim trans As SqlClient.SqlTransaction  '-- トランザクション

        pfDel_Exclusive = 0

        If clsDataConnect.pfOpen_Database(conDB) Then

            '-- トランザクションの設定 --
            trans = conDB.BeginTransaction

            Try

                cmdDB = New SqlCommand("ClsCMExclusive_D1", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                With cmdDB.Parameters
                    '-- パラメータ設定 --
                    '.Add(pfSet_Param("prm_Sesstion_ID", SqlDbType.NVarChar, SesstionID))                   '-- セッションID
                    .Add(pfSet_Param("prm_Date_Time", SqlDbType.NVarChar, Date_Time))                      '-- 登録日時
                    .Add(pfSet_Param("prm_Data_Exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '-- 結果
                End With

                '-- コマンドタイプ設定(ストアド) --
                cmdDB.ExecuteNonQuery()
                strOKNG = cmdDB.Parameters("prm_data_exist").Value.ToString

                '-- ストアドの結果判断 --
                Select Case strOKNG
                    Case "0"
                        '-- 正常 --
                        pfDel_Exclusive = 0

                        '-- コミット --
                        trans.Commit()

                    Case "9"
                        '-- 異常 --
                        Throw New Exception

                    Case Else
                        '-- 例外エラー --
                        Throw New Exception

                End Select

            Catch ex As Exception

                '-- ロールバック --
                trans.Rollback()
                psMesBox(ipage, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                pfDel_Exclusive = 9
                psClose_Window(ipage)

            Finally

                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(ipage, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If

            End Try

        Else
            '-- エラーメッセージの出力 --
            psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
            pfDel_Exclusive = 9
            psClose_Window(ipage)

        End If

    End Function

    ''' <summary>
    ''' 排他情報削除(ログアウト・セッションタイムアウト用)
    ''' </summary>
    ''' <param name="SesstionID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDel_Exclusive(ByVal SesstionID As String) As Integer


        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim order As DataSet = Nothing
        Dim strOKNG As String = Nothing
        Dim trans As SqlClient.SqlTransaction  '-- トランザクション

        pfDel_Exclusive = 0

        If clsDataConnect.pfOpen_Database(conDB) Then

            '-- トランザクションの設定 --
            trans = conDB.BeginTransaction

            Try

                cmdDB = New SqlCommand("ClsCMExclusive_D2", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans

                With cmdDB.Parameters
                    '-- パラメータ設定 --
                    .Add(pfSet_Param("prm_Sesstion_ID", SqlDbType.NVarChar, SesstionID))                      '-- 登録日時
                    .Add(pfSet_Param("prm_Data_Exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '-- 結果
                End With

                '-- コマンドタイプ設定(ストアド) --
                cmdDB.ExecuteNonQuery()
                strOKNG = cmdDB.Parameters("prm_data_exist").Value.ToString

                '-- ストアドの結果判断 --
                Select Case strOKNG
                    Case "0"
                        '-- 正常 --
                        pfDel_Exclusive = 0

                        '-- コミット --
                        trans.Commit()

                    Case "9"
                        '-- 異常 --
                        Throw New Exception

                    Case Else
                        '-- 例外エラー --
                        Throw New Exception

                End Select

            Catch ex As Exception

                '-- ロールバック --
                trans.Rollback()
                'psMesBox(ipage, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                pfDel_Exclusive = 9
                'psClose_Window(ipage)

            Finally

                If Not clsDataConnect.pfClose_Database(conDB) Then
                    'psMesBox(ipage, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                End If

            End Try

        Else
            '-- エラーメッセージの出力 --
            'psMesBox(ipage, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画前)
            pfDel_Exclusive = 9
            'psClose_Window(ipage)

        End If

    End Function
    '---------------------------
    '2014/06/10 後藤 ここまで
    '---------------------------

    '-------------------------------------------------------------------
    '  グループ番号
    '-------------------------------------------------------------------
    ''' <summary>
    ''' グループ番号
    ''' </summary>
    ''' <param name="GroupNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfGet_GroupNum(ByRef GroupNum As Integer, _
                                          ByVal ipage As Page) As Integer

        pfGet_GroupNum = 0
        GroupNum = 1

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim order As DataSet = Nothing
        Dim strOKNG As Integer = 0

        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand("ClsCMExclusive_S4", conDB)

                With cmdDB.Parameters

                    '-- パラメータ設定 --
                    .Add(pfSet_Param("prm_Group_Num_SEQ", SqlDbType.Int, 16, ParameterDirection.Output))    '-- グループ番号
                    .Add(pfSet_Param("prm_Data_Exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))  '-- 結果

                End With

                '-- 結果を取得 --
                order = clsDataConnect.pfGet_DataSet(cmdDB)
                strOKNG = cmdDB.Parameters("prm_Data_Exist").Value

                '-- ストアドの結果判断 --
                Select Case strOKNG

                    Case 0
                        '-- 取得成功 --
                        GroupNum = cmdDB.Parameters("prm_Group_Num_SEQ").Value

                    Case 9
                        '-- 取得失敗 --
                        Throw New Exception

                    Case Else
                        '-- 例外エラー --
                        Throw New Exception

                End Select

                pfGet_GroupNum = 0

            Catch ex As Exception

                psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
                pfGet_GroupNum = 9
                'psClose_Window(ipage)

            Finally

                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(ipage, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If

            End Try

        Else
            psMesBox(ipage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画前)
            pfGet_GroupNum = 9
            psClose_Window(ipage)

        End If

    End Function
    '---------------------------
    '2014/06/10 後藤 ここから
    '---------------------------
    ''' <summary>
    ''' 排他情報削除(画面終了時)
    ''' </summary>
    ''' <param name="SesstionID"></param>
    ''' <param name="Date_Time"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfDel_Exclusive_master(ByVal SesstionID As String _
                                                , ByVal Date_Time As String) As Integer

        'pfDel_Exclusive_master = 0

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim order As DataSet = Nothing
        Dim strOKNG As String = Nothing
        Dim trans As SqlClient.SqlTransaction             'トランザクション


        If clsDataConnect.pfOpen_Database(conDB) Then

            'トランザクションの設定
            trans = conDB.BeginTransaction

            Try

                cmdDB = New SqlCommand("ClsCMExclusive_D1", conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans


                With cmdDB.Parameters
                    'パラメータ設定
                    '.Add(pfSet_Param("prm_Sesstion_ID", SqlDbType.NVarChar, SesstionID))                 'セッションID
                    .Add(pfSet_Param("prm_Date_Time", SqlDbType.NVarChar, Date_Time))                   'グループ番号
                    .Add(pfSet_Param("prm_data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))  '結果
                End With

                '-- コマンドタイプ設定(ストアド) --
                cmdDB.ExecuteNonQuery()
                strOKNG = cmdDB.Parameters("prm_data_exist").Value.ToString

                '-- ストアドの結果判断 --
                Select Case strOKNG
                    Case "0"
                        '-- 正常 --
                        pfDel_Exclusive_master = 0

                        '-- コミット --
                        trans.Commit()

                    Case "9"
                        '-- 異常 --
                        Throw New Exception

                    Case Else
                        '-- 例外エラー --
                        Throw New Exception

                End Select

            Catch ex As Exception

                'ロールバック
                trans.Rollback()
                'psMesBox(ipage, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                pfDel_Exclusive_master = 9
                'psClose_Window(ipage)

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    'psMesBox(ipage, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画前)
                End If

            End Try

        Else

            'エラーメッセージの出力
            'psMesBox(ipage, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画前)
            pfDel_Exclusive_master = 9
            'psClose_Window(ipage)

        End If


    End Function
    '---------------------------
    '2014/06/10 後藤 ここまで
    '---------------------------
    '---------------------------
    '2014/06/10 後藤 ここから
    '---------------------------
    ''' <summary>
    ''' 排他情報削除処理(psClose_Window対応)
    ''' </summary>
    ''' <param name="ipage"></param>
    ''' <param name="ExclusiveDate"></param>
    ''' <param name="ExclusiveDate_dtl"></param>
    ''' <remarks></remarks>
    Public Sub psExclusive_Del_Chk(ByVal ipage As Page, _
                                          ByVal ExclusiveDate As String, _
                                          ByVal ExclusiveDate_dtl As String)

        '★排他情報削除
        If Not ExclusiveDate = String.Empty Then

            pfDel_Exclusive(ipage, "", ExclusiveDate)

        End If

        '★排他情報削除(明細)
        If Not ExclusiveDate_dtl = String.Empty Then

            pfDel_Exclusive(ipage, "", ExclusiveDate_dtl)

        End If

    End Sub
    '---------------------------
    '2014/06/10 後藤 ここまで
    '---------------------------
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
