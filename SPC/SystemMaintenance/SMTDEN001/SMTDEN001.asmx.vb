'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　.02.18　：　ＸＸＸ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports System.IO
#End Region

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
'<System.Web.Services.WebService(Namespace:="http://localhost:49741/SystemMaintenance/SMTDEN001/")> _
'<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SMTDEN001

#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.Services.WebService
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
    ' ''' <summary>
    ' ''' NGC決済センタLECシステムIP
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const strLECIP As String = "10.100.12.24"

    ' ''' <summary>
    ' ''' NGC決済センタNGCシステムIP
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const strNGCIP As String = "10.100.12.25"

    ''' <summary>
    ''' 出力ファイル名（先頭文字列共通）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strLOGFILE As String = "SNDRCV_"

    ''' <summary>
    ''' 出力ファイル名（拡張子）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strEXT_LOG As String = ".log"

    ''' <summary>
    ''' 出力ファイル名（連結文字列（エラー））
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strERR As String = "_ERR"

    ''' <summary>
    ''' エラーログ出力先（DB接続時、保存場所取得時）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strERRLOGPATH As String = "C:\NGC\DENBUN\LOG"

    ''' <summary>
    ''' 送信済み
    ''' </summary>
    ''' <remarks></remarks>
    Dim strSEND As String = "1"

    ''' <summary>
    ''' 受信済み
    ''' </summary>
    ''' <remarks></remarks>
    Dim strRECV As String = "2"

    ''' <summary>
    ''' BB構成ファイル格納
    ''' </summary>
    ''' <remarks></remarks>
    Dim strFILEBBKOUSEI As String = "7"

    ''' <summary>
    ''' JIS
    ''' </summary>
    ''' <remarks></remarks>
    Dim intENCODE As Integer = 50220

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================

    ' ''' <summary>
    ' ''' 集配信確認ファイル項目
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Enum ESHKakuninFileKmk
    '    処理日時 = 0
    '    照会管理番号
    '    連番
    '    ファイル種別コード
    '    ＴＢＯＸＩＤ
    '    処理区分
    '    処理状態区分
    '    抽出依頼ファイル番号
    '    ユーザーＩＤ
    'End Enum

    ' ''' <summary>
    ' ''' 処理区分
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Enum EShoriKbn
    '    即時集信 = 1
    '    随時照会
    '    配信
    '    リアル通知
    'End Enum

    ' ''' <summary>
    ' ''' 店内装置構成表配信依頼処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctUpTennaiHaisin
    '    Public strShSntkKbn As String              '処理選択区分
    '    Public strShSntk As String                 '処理選択
    '    Public strTboxId As String                 'TBOX-ID
    '    Public strTboxHdSbt As String              'T-BOXハード種別
    '    Public strBBGrandCnt As String             'BB総台数
    '    Public strKaisen0ConCnt As String          '回線0接続台数
    '    Public strKaisen1ConCnt As String          '回線1接続台数
    '    Public strJBNo As String                   'JB番号
    '    Public strUnyoKiban As String              '運用機番
    '    Public strKaisenNo As String               '回線番号
    '    Public strBBSbtCd As String                'BB種別コード
    '    Public strBBKisyuSbt As String             'BB機種種別
    '    Public strSimaNo As String                 '島番号
    '    Public strGeminiNo As String               '双子店番号
    '    Public strGeminiStiYNFlg As String         '双子店設定有無フラグ
    '    Public strTenpoStiYNFlg As String          '店舗設定有無フラグ
    '    Public strSandoSimaNoFm As String          'サンド島番号(From)
    '    Public strSandoSimaNoTo As String          'サンド島番号(To)
    '    Public strKenbaiNyukinKibanFm As String    '券売入金機機番(From)
    '    Public strKenbaiNyukinKibanTo As String    '券売入金機機番(To)
    '    Public strSeisankiKibanFm As String        '精算機機番(From)
    '    Public strSeisankiKibanTo As String        '精算機機番(To)
    '    Public strTorokuUktkKibanFm As String      '登録受付機機番(From)
    '    Public strTorokuUktkKibanTo As String      '登録受付機機番(To)
    'End Structure

    ' ''' <summary>
    ' ''' 店内装置構成表反映指示処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctUpTennaiHanei
    '    Public strShoriSelKbn As String            '処理選択区分
    '    Public strShoriSel As String               '処理選択	
    '    Public strTboxId As String                 'TBOX-ID		
    '    Public strUpdateRenban As String           '更新通番	
    'End Structure

    ' ''' <summary>
    ' ''' T-BOX制御情報更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctUpTboxSeigyoInfoUpdate
    '    Public strShoriSelKbn As String             '処理選択区分		
    '    Public strShoriSel As String                '処理選択		
    '    Public strTboxId As String                  'TBOX-ID		
    '    Public strHosyuModeSw As String             '保守ﾓｰﾄﾞ操作可能SW		
    '    Public strHaisoInfoSw As String             '配送情報突合SW		
    '    Public strTokubetuSw As String              '特別運用ﾓｰﾄﾞSW		
    '    Public strUsingDBSw As String               '使用中DBﾁｪｯｸSW		
    '    Public strChainBBUnyoSw As String           'ﾁｪｰﾝ店BB取込制御SW＜運用ﾓｰﾄﾞ＞		
    '    Public strChainBBHosyuSw As String          'ﾁｪｰﾝ店BB取込制御SW＜保守ﾓｰﾄﾞ＞		
    '    Public strTatenBBUnyoSw As String           '他店BB取込制御SW＜運用ﾓｰﾄﾞ＞		
    '    Public strTatenBBHosyuSw As String          '他店BB取込制御SW＜保守ﾓｰﾄﾞ＞		
    '    Public strDairiBBUnyoSw As String           '代理店BB取込制御SW＜運用ﾓｰﾄﾞ＞		
    '    Public strDairiBBHosyuSw As String          '代理店BB取込制御SW＜保守ﾓｰﾄﾞ＞		
    '    Public strZeroCardSw As String              '0円ｶｰﾄﾞ／ｺｲﾝ許可SW		
    '    Public strTamaTankaSw As String             '玉単価設定許可SW		
    '    Public strUsingDBShokaiSw As String         '使用中DBﾛｰｶﾙ照会制御SW		
    '    Public strOfflineSandoSw As String          'ｵﾌﾗｲﾝﾛｰｶﾙ照会制御SW（ｻﾝﾄﾞ）		
    '    Public strOfflineKenbaiSw As String         'ｵﾌﾗｲﾝﾛｰｶﾙ照会制御SW（券売入金機）		
    '    Public strUpdateNo As String                '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB運用情報更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctUpBBUnyoInfoUpdate
    '    Public strShoriSelKbn As String                       '処理選択区分		
    '    Public strShoriSel As String                          '処理選択		
    '    Public strTBoxId As String                            'TBOX-ID		
    '    Public strCampanyKbn As String                        '会社区分		
    '    Public strCardSbt As String                           'ｶｰﾄﾞ種別		
    '    Public strKenmenKngk As String                        '券面金額		
    '    Public strPremiumKngk As String                       'ﾌﾟﾚﾐｱﾑ金額		
    '    Public strKenbaiFlg As String                         '券売ﾌﾗｸﾞ		
    '    Public strDaikanFlg As String                         '台間ﾌﾗｸﾞ		
    '    Public strShohiFlg As String                          '消費ﾌﾗｸﾞ		
    '    Public strUseTani As String                           '消費単位		
    '    Public strShohiGendoKngk As String                    '消費限度額		
    '    Public strBaitaiRuisekiGendoKngk As String            '媒体累積入金限度額		
    '    Public strCardYukoKikan As String                     'カード有効期間		
    '    Public strOfflineUnyoFm As String                     'ｵﾌﾗｲﾝ運用可能時間帯(From)		
    '    Public strOfflineUnyoTo As String                     'ｵﾌﾗｲﾝ運用可能時間帯(To)		
    '    Public strOfflineUnyoKyoka As String                  'ｵﾌﾗｲﾝ運用許可時間		
    '    Public strOfflineKenbaiHakkenKngk As String           'ｵﾌﾗｲﾝ発券･入金可能金額(券売機)		
    '    Public strOfflineKenbaiHakkenMaisu As String          'ｵﾌﾗｲﾝ発券･入金可能枚数(券売機)		
    '    Public strOfflineSandoNyukinKngk As String            'ｵﾌﾗｲﾝ入金可能金額(ｻﾝﾄﾞ)		
    '    Public strOfflineSandoNyukinMaisu As String           'ｵﾌﾗｲﾝ入金可能枚数(ｻﾝﾄﾞ)		
    '    Public strOfflineShohiKngk As String                  'ｵﾌﾗｲﾝ消費可能金額		
    '    Public strOfflineShohiMaisu As String                 'ｵﾌﾗｲﾝ消費可能枚数		
    '    Public strKaiinSvcCode1 As String                     '会員サービスコード1		
    '    Public strKaiinSvcCode2 As String                     '会員サービスコード2		
    '    Public strKijunTamasuNow As String                    '基準玉/ﾒﾀﾞﾙ数情報（現在）基準玉数		
    '    Public strKijunMedarusuNow As String                  '基準玉/ﾒﾀﾞﾙ数情報（現在）基準ﾒﾀﾞﾙ数		
    '    Public strKijunTamasuFuture As String                 '基準玉/ﾒﾀﾞﾙ数情報（未来）基準玉数		
    '    Public strKijunMedarusuFuture As String               '基準玉/ﾒﾀﾞﾙ数情報（未来）基準ﾒﾀﾞﾙ数		
    '    Public strKijunTamaHaneiYmd As String                 '基準玉/ﾒﾀﾞﾙ数反映年月日	
    '    Public strShohizeiKbnNow As String                    '消費税運用情報（現在）消費税区分
    '    Public strShohizeiUnyoSetteiNow As String             '消費税運用情報（現在）消費税運用設定
    '    Public strShohizeiRituNow As String                   '消費税運用情報（現在）消費税率		
    '    Public strShohizeiKbnFuture As String                 '消費税運用情報（現在）消費税区分
    '    Public strShohizeiUnyoSetteiFuture As String          '消費税運用情報（現在）消費税運用設定
    '    Public strShohizeiRituFuture As String                '消費税運用情報（現在）消費税率		
    '    Public strShohizeiHaneiYmd As String                  '消費税反映年月日		
    '    Public strEveryMoney As String                        'ｴﾌﾞﾘﾏﾈｰ有効期間		
    '    Public strKaiinCardUktk As String                     '会員ｶｰﾄﾞ受付有無（券）		
    '    Public strIppanZeroCard As String                     '一般0円ｶｰﾄﾞ入金（券）		
    '    Public strKaiinZeroCard As String                     '会員0円ｶｰﾄﾞ入金（券）		
    '    Public strTuikaNyukinJoken As String                  '追加入金条件設定（券）		
    '    Public strIppanZeroCardOut As String                  '一般ｶｰﾄﾞ0円時排出方向		
    '    Public strTamakashiTimerVal As String                 '玉貸機(台毎用)ﾀｲﾏ値		
    '    Public strTamakashiShikiiVal As String                '玉貸機(台毎用)しきい値		
    '    Public strCardUnitPtnkTimerVal As String              'ｶｰﾄﾞﾕﾆｯﾄ(ﾊﾟﾁﾝｺ)ﾀｲﾏ値		
    '    Public strCardUnitPtnkShikiiVal As String             'ｶｰﾄﾞﾕﾆｯﾄ(ﾊﾟﾁﾝｺ)しきい値		
    '    Public strCardUnitSlotTimerVal As String              'ｶｰﾄﾞﾕﾆｯﾄ(ﾊﾟﾁｽﾛ)ﾀｲﾏ値		
    '    Public strCardUnitSlotShikiiVal As String             'ｶｰﾄﾞﾕﾆｯﾄ(ﾊﾟﾁｽﾛ)しきい値		
    '    Public strICMedalDaigotoTimerVal As String            'ICﾒﾀﾞﾙ貸機(台毎)ﾀｲﾏ値		
    '    Public strICMedalDaigotoShikiiVal As String           'ICﾒﾀﾞﾙ貸機(台毎)しきい値		
    '    Public strICMedalDaikanTimerVal As String             'ICﾒﾀﾞﾙ貸機(台間)ﾀｲﾏ値		
    '    Public strICMedalDaikanShikiiVal As String            'ICﾒﾀﾞﾙ貸機(台間)しきい値		
    '    Public strTamakashiTankaKino As String                '玉貸単価機能設定		
    '    Public strKaiinCardPassInput As String                '会員ｶｰﾄﾞﾊﾟｽﾜｰﾄﾞ入力有無		
    '    Public strIppanZeroCardKaisyu As String               '一般ｶｰﾄﾞ0円時回収方向		
    '    Public strKaiinCardUktkSando As String                '会員ｶｰﾄﾞ受付有無（ｻ）		
    '    Public strDaikanNyukinKino As String                  '台間入金機能許可/抑止		
    '    Public strUkeireOut500 As String                      '受入禁止券種設定500円玉		
    '    Public strUkeireOut1000 As String                     '受入禁止券種設定1000円札		
    '    Public strUkeireOut2000 As String                     '受入禁止券種設定2000円札		
    '    Public strUkeireOut5000 As String                     '受入禁止券種設定5000円札		
    '    Public strUkeireOut10000 As String                    '受入禁止券種設定10000円札		
    '    Public strIppanZeroCardSando As String                '一般0円ｶｰﾄﾞ入金（ｻ）		
    '    Public strKaiinZeroCardSando As String                '会員0円ｶｰﾄﾞ入金（ｻ）		
    '    Public strTuikaJokenSando As String                   '追加入金条件設定（ｻ）		
    '    Public strAnsyoShogoSbt As String                     '暗証照合種別		
    '    Public strEveryMoneySeisan As String                  'ｴﾌﾞﾘﾏﾈｰ精算設定		
    '    Public strSeisanCardSbt As String                     '精算可能ｶｰﾄﾞ種別設定		
    '    Public strSeisangoKaiinCardOut As String              '精算後0円会員ｶｰﾄﾞ排出方向設定		
    '    Public strPremiumCardSeisan As String                 'ﾌﾟﾚﾐｱﾑ金額有ｶｰﾄﾞ精算設定		
    '    Public strTojituNyukinCardSeisan As String            '当日入金/発券ｶｰﾄﾞ精算設定		
    '    Public strShohiNashiCardSeisan As String              '消費無しｶｰﾄﾞ精算設定		
    '    Public strTorokuKinshiFm As String                    '登録受付禁止時間帯(From)		
    '    Public strTorokuKinshiTo As String                    '登録受付禁止時間帯(To)		
    '    Public strIppanZeroCardKinshi As String               '一般0円ｶｰﾄﾞ受付許可/禁止		
    '    Public strIppanZandakaAriCardKinshi As String         '一般残高ありｶｰﾄﾞ受付許可/禁止		
    '    Public strKaiinCardKinshi As String                   '会員ｶｰﾄﾞ受付許可/禁止		
    '    Public strUpdateNo As String                          '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB制御情報（券売入金機）更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctBBSeigyoKenbaiNyukinki
    '    Public strShoriSelKbn As String                   '処理選択区分		
    '    Public strShoriSel As String                      '処理選択		
    '    Public strTboxId As String                        'TBOX-ID		
    '    Public strOfflineUnyoKyoka As String              'オフライン運用許可/禁止		
    '    Public strEveryMoneyShiyoKyoka As String          'エブリマネー使用許可/停止		
    '    Public strHaisoCardIdChk As String                '配送カードIDチェック有無		
    '    Public strBB1Dosa7 As String                      'BB1動作制御7		
    '    Public strBB1Dosa5_4 As String                    'BB1動作制御5/4		
    '    Public strBB1Dosa3_2 As String                    'BB1動作制御3/2		
    '    Public strBB1Dosa1 As String                      'BB1動作制御1		
    '    Public strICCDosa7_6 As String                    'ICC動作制御7/6		
    '    Public strICCHeisoku1 As String                   'ICC閉塞指示1		
    '    Public strICCHeisoku2 As String                   'ICC閉塞指示2		
    '    Public strICCHeisoku3 As String                   'ICC閉塞指示3		
    '    Public strICCHeisoku4 As String                   'ICC閉塞指示4		
    '    Public strICCHeisoku5 As String                   'ICC閉塞指示5		
    '    Public strHyojiSeigyo As String                   '表示制御		
    '    Public strLamp As String                          'ランプ		
    '    Public strConnectionTimeOut As String             'コネクションタイムアウト時間		
    '    Public strStartShoriUktkTimeOut As String         '開始処理受付タイムアウト		
    '    Public strUnyoStartTimeOut As String              '運用開始タイムアウト		
    '    Public strToiawaseTimeOut As String               '問合せタイムアウト時間		
    '    Public strTusinCloseDelayTime As String           '通信切断時接続ディレイ時間		
    '    Public strConnectionReqRetry As String            'コネクション要求リトライ間隔		
    '    Public strUDPCommandTimeOut As String             'UDPコマンドタイムアウト		
    '    Public strUpdateNo As String                      '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB制御情報（サンド 精算機）更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctBBSeigyoInfoSandoSeisanki
    '    Public strShoriSelKbn As String                '処理選択区分		
    '    Public strShoriSel As String                   '処理選択		
    '    Public strTboxId As String                     'TBOX-ID		
    '    Public strOfflineUnyo As String                'ｵﾌﾗｲﾝ運用許可/禁止		
    '    Public strSeisanKyoka As String                '精算許可/停止		
    '    Public strOfflineSandoNyukinKyoka As String    'ｵﾌﾗｲﾝｻﾝﾄﾞ入金許可/禁止		
    '    Public strYasumiCardKaiin As String            '休ｶｰﾄﾞ(会員)ｵﾌﾗｲﾝ入金許可/禁止		
    '    Public strYasumiCardIppan As String            '休ｶｰﾄﾞ(一般)ｵﾌﾗｲﾝ入金許可/禁止		
    '    Public strBB1Dosa7 As String                   'BB1動作制御7		
    '    Public strBB1Dosa6 As String                   'BB1動作制御6		
    '    Public strBB1Dosa5_4 As String                 'BB1動作制御5/4		
    '    Public strBB1Dosa3_2 As String                 'BB1動作制御3/2		
    '    Public strBB1Dosa1 As String                   'BB1動作制御1		
    '    Public strICCDosa7_6 As String                 'ICC動作制御7/6		
    '    Public strICCDosa3 As String                   'ICC動作制御3		
    '    Public strICCDosa2 As String                   'ICC動作制御2		
    '    Public strICCDosa1 As String                   'ICC動作制御1		
    '    Public strICCHeisoku1 As String                'ICC閉塞指示1		
    '    Public strICCHeisoku2 As String                'ICC閉塞指示2		
    '    Public strICCHeisoku3 As String                'ICC閉塞指示3		
    '    Public strICCHeisoku4 As String                'ICC閉塞指示4		
    '    Public strICCHeisoku5 As String                'ICC閉塞指示5		
    '    Public strHyojiSeigyo As String                '表示制御		
    '    Public strLamp As String                       'ﾗﾝﾌﾟ		
    '    Public strConnectionTimeOut As String          'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間		
    '    Public strStartShoriUktkTimeOut As String      '開始処理受付ﾀｲﾑｱｳﾄ		
    '    Public strUnyoStartTimeOut As String           '運用開始ﾀｲﾑｱｳﾄ		
    '    Public strToiawaseTimeOut As String            '問合せﾀｲﾑｱｳﾄ時間		
    '    Public strTusinCloseDelayTime As String        '通信切断時接続ﾃﾞｨﾚｲ時間		
    '    Public strConnectionReqRetry As String         'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ間隔		
    '    Public strUDPCommandTimeOut As String          'UDPｺﾏﾝﾄﾞﾀｲﾑｱｳﾄ		
    '    Public strUpdateNo As String                   '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB制御情報（登録受付機）更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctBBSeigyoInfoTorokuUktk
    '    Public strShoriSelKbn As String               '処理選択区分		
    '    Public strShoriSel As String                  '処理選択		
    '    Public strTboxId As String                    'TBOX-ID		
    '    Public strTorokuUktkKyoka As String           '登録受付許可/禁止		
    '    Public strBB1Dosa7 As String                  'BB1動作制御7		
    '    Public strBB1Dosa5_4 As String                'BB1動作制御5/4		
    '    Public strBB1Dosa3_2 As String                'BB1動作制御3/2		
    '    Public strBB1Dosa1 As String                  'BB1動作制御1		
    '    Public strICCDosa7_6 As String                'ICC制御動作7/6		
    '    Public strICCHeisokuSzi1 As String            'ICC制御閉塞指示1		
    '    Public strICCHeisokuSzi2 As String            'ICC制御閉塞指示2		
    '    Public strICCHeisokuSzi3 As String            'ICC制御閉塞指示3		
    '    Public strICCHeisokuSzi4 As String            'ICC制御閉塞指示4		
    '    Public strICCHeisokuSzi5 As String            'ICC制御閉塞指示5		
    '    Public strHyojiSeigyo As String               '表示制御		
    '    Public strSeigyoInfo As String                '制御情報		
    '    Public strGamenNo As String                   '画面番号		
    '    Public strConnectionTimeOut As String         'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間		
    '    Public strStartShoriUktkTimeOut As String     '開始処理受付ﾀｲﾑｱｳﾄ		
    '    Public strUnyoStartTimeOut As String          '運用開始ﾀｲﾑｱｳﾄ		
    '    Public strToiawaseTimeOut As String           '問合せﾀｲﾑｱｳﾄ時間		
    '    Public strTusinCloseDelayTime As String       '通信切断時接続ﾃﾞｨﾚｲ時間		
    '    Public strConnectionReqRetry As String        'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ間隔		
    '    Public strUDPCommandTimeOut As String         'UDPｺﾏﾝﾄﾞﾀｲﾑｱｳﾄ		
    '    Public strTorokuToiawaseTimeOut As String     '登録問合せﾀｲﾑｱｳﾄ		
    '    Public strUpdateNo As String                  '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB運用情報（LUT）更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctBBUnyoInfoLUT
    '    Public strShoriSelKbn As String                 '処理選択区分		
    '    Public strShoriSel As String                    '処理選択		
    '    Public strTboxID As String                      'TBOX-ID		
    '    Public strTanmatuId As String                   '端末ID		
    '    Public strUserId As String                      'ユーザID		
    '    Public strShikakuKbn As String                  '資格区分		
    '    Public strKaishaKbn As String                   '会社区分		
    '    Public strCardSbt As String                     'ｶｰﾄﾞ種別		
    '    Public strKenmenKngk As String                  '券面金額		
    '    Public strKenbaiFlg As String                   '券売ﾌﾗｸﾞ		
    '    Public strDaikanFlg As String                   '台間ﾌﾗｸﾞ		
    '    Public strShohiFlg As String                    '消費ﾌﾗｸﾞ		
    '    Public str1DosuKngk As String                   '1度数金額設定情報		
    '    Public strShohizeiKbnNow As String              '消費税区分（現在）		
    '    Public strShohizeiUnyoNow As String             '消費税運用設定（現在）		
    '    Public strShohizeiRituNow As String             '消費税率（現在）		
    '    Public strShohizeiKbnFuture As String           '消費税区分（未来）		
    '    Public strShohizeiUnyoFuture As String          '消費税運用設定（未来）		
    '    Public strShohizeiRituFuture As String          '消費税率（未来）		
    '    Public strShohizeiHaneiYmd As String            '消費税反映年月日		
    '    Public strKaiinShohiYukokigenSu As String       '消費有効期限（会員）数値指定		
    '    Public strKaiinShohiYukokigenYmd As String      '消費有効期限（会員）年月日		
    '    Public strIppanShohiYukokigenSu As String       '消費有効期限（一般）数値指定		
    '    Public strIppanShohiYukokigenYmd As String      '消費有効期限（一般）年月日		
    '    Public strShohiFukaKngk As String               '消費不可金額（一般）		
    '    Public strOfflineUnyoSandoFm As String          'ｵﾌﾗｲﾝ運用可能時間帯（ｻ）（From）		
    '    Public strOfflineUnyoSandoTo As String          'ｵﾌﾗｲﾝ運用可能時間帯（ｻ）（To）		
    '    Public strOfflineUnyoKyokaTimeSando As String   'ｵﾌﾗｲﾝ運用許可時間（ｻ）		
    '    Public strOfflineMisosinTraceCnt As String      'ｵﾌﾗｲﾝ未送信ﾄﾚｰｽ件数		
    '    Public strOfflineUnyoSeiFm As String            'ｵﾌﾗｲﾝ運用可能時間帯（精）（From）		
    '    Public strOfflineUnyoSeiTo As String            'ｵﾌﾗｲﾝ運用可能時間帯（精）（To）		
    '    Public strOfflineUnyoKyokaTimeSei As String     'ｵﾌﾗｲﾝ運用許可時間（精）		
    '    Public strKaiinSvcCode1 As String               '会員ｻｰﾋﾞｽｺｰﾄﾞ1		
    '    Public strKaiinSvcCode2 As String               '会員ｻｰﾋﾞｽｺｰﾄﾞ2		
    '    Public strTuikaNyukinjokenSando As String       '追加入金条件設定（ｻﾝﾄﾞ）		
    '    Public strNyukinJogen As String                 '入金上限額設定		
    '    Public strSeisanCardSbt As String               '精算可能ｶｰﾄﾞ種別設定		
    '    Public strTorokuUktkKinsiFm As String           '登録受付禁止時間帯（From）		
    '    Public strTorokuUktkKinsiTo As String           '登録受付禁止時間帯（To）		
    '    Public strIppanZeroCoinUktk As String           '一般0円ｺｲﾝ受付許可/禁止		
    '    Public strIppanZandakaariCoinUktk As String     '一般残高ありｺｲﾝ受付許可/禁止		
    '    Public strKaiinCardUktk As String               '会員ｶｰﾄﾞ受付許可/禁止		
    '    Public strMode As String                        'モード設定		
    '    Public strRuisekiShohiChkKngk As String         '累積消費チェック金額設定		
    '    Public strDaikanNyukinKino As String            '台間入金機能許可/抑止		
    '    Public strEveryMoneyYukoKikan As String         'ｴﾌﾞﾘﾏﾈｰ有効期間		
    '    Public strUkeireKinsi100 As String              '受入禁止券種設定100円玉		
    '    Public strUkeireKinsi500 As String              '受入禁止券種設定500円玉		
    '    Public strUkeireKinsi1000 As String             '受入禁止券種設定1000円札		
    '    Public strUkeireKinsi2000 As String             '受入禁止券種設定2000円札		
    '    Public strUkeireKinsi5000 As String             '受入禁止券種設定5000円札		
    '    Public strUkeireKinsi10000 As String            '受入禁止券種設定10000円札		
    '    Public strIppanZeroCoinOutSando As String       '一般ｺｲﾝ0円時排出方向（ｻﾝﾄﾞ）		
    '    Public strIppanZeroCoinOutSei As String         '一般ｺｲﾝ0円時排出方向（精）		
    '    Public strSeigyo95_1 As String                  '制御95情報1		
    '    Public strSeigyo95_2 As String                  '制御95情報2		
    '    Public strSeigyo95_3 As String                  '制御95情報3		
    '    Public strSeigyo95_4 As String                  '制御95情報4		
    '    Public strSeigyo95_5 As String                  '制御95情報5		
    '    Public strSeigyo95_6 As String                  '制御95情報6		
    '    Public strSeigyo95_7 As String                  '制御95情報7		
    '    Public strSeigyo95_8 As String                  '制御95情報8		
    '    Public strUpdateNo As String                    '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB制御情報（サンド）（LUT）更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctBBSeigyoInfoSandLUT
    '    Public strShoriSelKbn As String                              '処理選択区分		
    '    Public strShoriSel As String                                 '処理選択		
    '    Public strTboxId As String                                   'TBOX－ID		
    '    Public strOfflineUnyoKyoka As String                         'ｵﾌﾗｲﾝ運用許可/禁止		
    '    Public strEveryMoneyShiyoKyoka As String                     'ｴﾌﾞﾘﾏﾈｰ使用許可/停止		
    '    Public strOfflineEveryMoneyShiyoKyoka As String              'ｵﾌﾗｲﾝｴﾌﾞﾘﾏﾈｰ使用許可/停止		
    '    Public strTorokuUktkKyoka As String                          '登録受付許可/禁止		
    '    Public strCardZanOfflineShohiKyoka As String                 'ｶｰﾄﾞ残高ｵﾌﾗｲﾝ消費許可/禁止		
    '    Public strYasumiCoinIppanNyukinKyoka As String               '休ｺｲﾝ（一般）ｵﾌﾗｲﾝ入金許可/禁止		
    '    Public strYasumiCoinKaiinNyukinKyoka As String               '休ｶｰﾄﾞ（会員）ｵﾌﾗｲﾝ入金許可/禁止		
    '    Public strBB1Dosa7 As String                                 'BB1動作制御7		
    '    Public strBB1Dosa6 As String                                 'BB1動作制御6		
    '    Public strBB1Dosa5_4 As String                               'BB1動作制御5/4		
    '    Public strBB1Dosa3_2 As String                               'BB1動作制御3/2		
    '    Public strBB1Dosa1 As String                                 'BB1動作制御1		
    '    Public strICCDosa7_6 As String                               'ICC動作制御7/6		
    '    Public strICCDosa3 As String                                 'ICC動作制御3		
    '    Public strICCDosa2 As String                                 'ICC動作制御2		
    '    Public strICCDosa1 As String                                 'ICC動作制御1		
    '    Public strICCHeisoku1 As String                              'ICC閉塞指示1		
    '    Public strICCHeisoku2 As String                              'ICC閉塞指示2		
    '    Public strICCHeisoku3 As String                              'ICC閉塞指示3		
    '    Public strICCHeisoku4 As String                              'ICC閉塞指示4		
    '    Public strICCHeisoku5 As String                              'ICC閉塞指示5		
    '    Public strHyojiSeigyo As String                              '表示制御		
    '    Public strLamp As String                                     'ﾗﾝﾌﾟ		
    '    Public strConnectionTimeOut As String                        'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間		
    '    Public strStartShoriUktkTimeOut As String                    '開始処理受付ﾀｲﾑｱｳﾄ		
    '    Public strUnyoStartTimeOut As String                         '運用開始ﾀｲﾑｱｳﾄ		
    '    Public strToiawaseTimeOut As String                          '問合せﾀｲﾑｱｳﾄ時間		
    '    Public strTusinCloseDelayTime As String                      '通信切断時接続ﾃﾞｨﾚｲ時間		
    '    Public strConnectionReqRetry As String                       'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ間隔		
    '    Public strKeepAliveTimeOut As String                         'KeepAliveﾀｲﾑｱｳﾄ		
    '    Public strTorokuToiawaseTimeOut As String                    '登録問い合わせﾀｲﾑｱｳﾄ		
    '    Public strUpdateNo As String                                 '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' BB制御情報（精算機）（LUT）更新処理上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctBBSeigyoInfoSeisankiLUT
    '    Public strShoriSelKbn As String                            '処理選択区分		
    '    Public strShoriSel As String                               '処理選択		
    '    Public strTboxId As String                                 'TBOX-ID		
    '    Public strSeisanKyoka As String                            '精算許可/停止		
    '    Public strTorokuUktkKyoka As String                        '登録受付許可/禁止		
    '    Public strNumShitei As String                              '数値指定		
    '    Public strYmdShitei As String                              '年月日指定		
    '    Public strOfflineSeisanKyoka As String                     'ｵﾌﾗｲﾝ精算許可/禁止		
    '    Public strKyotukaCardTotenNyukinNasiKyoka As String        '共通化ｶｰﾄﾞ当店入金なし精算許可/禁止		
    '    Public strBB1Dosa7 As String                               'BB1動作制御7		
    '    Public strBB1Dosa6 As String                               'BB1動作制御6		
    '    Public strBB1Dosa5_4 As String                             'BB1動作制御5/4		
    '    Public strBB1Dosa3_2 As String                             'BB1動作制御3/2		
    '    Public strBB1Dosa1 As String                               'BB1動作制御1		
    '    Public strICCDosa7_6 As String                             'ICC動作制御7/6		
    '    Public strICCDosa3 As String                               'ICC動作制御3		
    '    Public strICCDosa2 As String                               'ICC動作制御2		
    '    Public strICCDosa1 As String                               'ICC動作制御1		
    '    Public strICCHeisoku1 As String                            'ICC閉塞指示1		
    '    Public strICCHeisoku2 As String                            'ICC閉塞指示2		
    '    Public strICCHeisoku3 As String                            'ICC閉塞指示3		
    '    Public strICCHeisoku4 As String                            'ICC閉塞指示4		
    '    Public strICCHeisoku5 As String                            'ICC閉塞指示5		
    '    Public strHyojiSeigyo As String                            '表示制御		
    '    Public strLamp As String                                   'ﾗﾝﾌﾟ		
    '    Public strConnectionTimeOut As String                      'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間		
    '    Public strStartShoriUktkTimeOut As String                  '開始処理受付ﾀｲﾑｱｳﾄ		
    '    Public strUnyoStartTimeOut As String                       '運用開始ﾀｲﾑｱｳﾄ		
    '    Public strToiawaseTimeOut As String                        '問合せﾀｲﾑｱｳﾄ時間		
    '    Public strTusinCloseDelayTime As String                    '通信切断時接続ﾃﾞｨﾚｲ時間		
    '    Public strConnectionReqRetry As String                     'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ時間		
    '    Public strKeepAliveTimeOut As String                       'KeepAliveﾀｲﾑｱｳﾄ		
    '    Public strTorokuToiawaseTimeOut As String                  '登録問い合わせﾀｲﾑｱｳﾄ		
    '    Public strUpdateNo As String                               '更新通番		
    'End Structure

    ' ''' <summary>
    ' ''' 即時集信依頼上り電文構造体
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Structure StrctSokujiSyushin
    '    Public strShoriSelKbn As String     '処理選択区分
    '    Public strTboxId As String          'TBOX-ID
    'End Structure

#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================

    ''' <summary>
    ''' ソケット
    ''' </summary>
    ''' <remarks></remarks>
    Dim objSck As System.Net.Sockets.TcpClient

    ''' <summary>
    ''' ネットワークストリーム(電文内容)
    ''' </summary>
    ''' <remarks></remarks>
    Dim objStm As System.Net.Sockets.NetworkStream

    ''' <summary>
    ''' エラー分類
    ''' </summary>
    ''' <remarks></remarks>
    Dim strErrBunrui As String = ""

    ''' <summary>
    ''' メソッド名称
    ''' </summary>
    ''' <remarks></remarks>
    Dim strMesod As String = ""

    ''' <summary>
    ''' テーブル名称
    ''' </summary>
    ''' <remarks></remarks>
    Dim strTable As String = ""

    ''' <summary>
    ''' データ（実行したSQL文等）
    ''' </summary>
    ''' <remarks></remarks>
    Dim strData As String = ""

    ''' <summary>
    ''' 内容
    ''' </summary>
    ''' <remarks></remarks>
    Dim strNaiyo As String = ""

    Dim clsDataConnect As New ClsCMDataConnect

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 電文処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function pfSendDenbun() As Boolean
        '    Public Function pfSendDenbun(ByVal strIFSKKanriNo As String) As Boolean
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As New SqlCommand
        Dim strFilePath As String = ""
        Dim strErrKbn As String = "0"
        Dim objDataSet As DataSet = Nothing
        Dim objDtSetConnect As DataSet = Nothing
        'バイト型配列
        Dim btArray As Byte() = New Byte() {}
        'IPアドレス
        Dim strIpAddr As String = ""
        'ポート番号
        Dim strPort As String = ""
        '決済センタからの戻りメッセージ
        Dim strRetrunMessage As String = ""

        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                'トランザクション
                cmdDB.Connection = conDB
            Else
                strFilePath = strERRLOGPATH
                strErrBunrui = "ＤＢ接続"
                strMesod = "pfSendDenbun"
                strTable = ""
                strData = ""
                Throw New Exception("データベースに接続できません")
            End If

            '保存場所取得処理
            If mfGetHozon(cmdDB, conDB, strFilePath) = False Then
                '取得できない場合は異常終了
                Return False
            End If

            '照会要求データ取得処理
            If mfGetReferenceData(cmdDB, conDB, objDataSet, strFilePath, strErrKbn) = False Then
                '取得できない場合
                If strErrKbn = "0" Then
                    '照会要求データ取得処理で該当データがないため正常終了
                    Return True
                Else
                    '照会要求データ取得処理でCatchされたため異常終了
                    Return False
                End If
            End If
            '取得できた内容を設定する
            Dim dtTable As DataTable
            Dim objDataRow As DataRow
            dtTable = objDataSet.Tables(0)
            objDataRow = dtTable.Rows(0)
            Dim dtmYoukyu As DateTime = objDataRow(0) '要求日付
            Dim strTuban As String = objDataRow(1)    '要求通番
            Dim strTboxId As String = objDataRow(2)   'TBOXID
            Dim strProcCd As String = objDataRow(3)   '処理コード
            Dim strReqMngNo As String = objDataRow(4) '依頼管理番号
            Dim strTennaiPath As String = ""
            Dim strTennaiFile() As String = Nothing
            Dim strTennaiFullPathFile As String = Nothing
            '処理コードが401の場合、ファイルから電文を作成するためファイル名（フォルダ）を取得する
            If strProcCd = 401 Then
                If Me.mfGetBbkouseiFolderPath(cmdDB, conDB, strTennaiPath) = False Then
                    '取得できない場合は異常終了
                    Return False
                End If
                'TBOXIDが付加されたものがファイル名となる
                strTennaiPath = strTennaiPath & "\" & strTboxId
                strTennaiFile = System.IO.Directory.GetFiles(strTennaiPath)
                strTennaiFullPathFile = strTennaiFile(0)
            End If
            '電文作成処理
            If mfCreateDenbun(cmdDB, conDB, strProcCd, strReqMngNo, btArray, strTennaiFullPathFile, strTboxId, strFilePath) = False Then
                '電文作成で失敗した場合
                Return False
            End If

            '照会要求データ更新処理
            If mfUpdateReferenceData(cmdDB, conDB, dtmYoukyu, strTuban, strSEND, strFilePath) = False Then
                '更新できない場合
                Return False
            End If

            '接続先情報取得処理
            If mfGetAccessPoint(cmdDB, conDB, strTboxId, objDtSetConnect, strFilePath) = False Then
                '更新できない場合
                Return False
            End If
            '取得できた内容を設定する
            Dim dtTableConnect As DataTable
            Dim objDataRowConnect As DataRow
            dtTableConnect = objDtSetConnect.Tables(0)
            objDataRowConnect = dtTableConnect.Rows(0)
            strIpAddr = objDataRowConnect(0) 'IPアドレス
            strPort = objDataRowConnect(1)   'ポート番号

            'ソケット処理
            If mfNGCConnect(strIpAddr, Integer.Parse(strPort), strFilePath, strProcCd, btArray, strRetrunMessage) = False Then
                Return False
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'ソケット処理の受信が行えない（サーバが存在しないので返信がない）'
            'そのため、とりあえずメッセージを固定で入れている                '
            'ソケット処理が完了したら削除すること！！！！！！！！！！！      '
            'strRetrunMessage = "NT0060"
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '照会要求データ更新処理（電文終了）
            If mfEndUpdateReferenceData(cmdDB, conDB, dtmYoukyu, strTuban, strRECV, strRetrunMessage, strFilePath) = False Then
                '更新できない場合
                Return False
            End If

            ''NGC決済センター接続★ポート番号
            'If mfNGCConnect(0) = False Then
            '    Throw New Exception("ソケットエラー")
            'End If

            ''ＤＢ接続
            'If pfOpen_Database(conDB) Then
            '    'トランザクション
            '    cmdDB.Connection = conDB
            'Else
            '    'DBを開けない場合例外スロー
            '    Throw New Exception("")
            'End If

            ''ファイルの存在チェック(集配信確認ファイル)
            'If Not mfChkFileExists(strFilePath) Then
            '    Throw New Exception("ファイルが存在しない")
            'End If

            ''CSVファイルの内容をリストとして取得
            'Dim objFileData As List(Of String()) = pfReadCsvFile(strFilePath)

            ''内容がない場合例外スロー
            'If objFileData Is Nothing Then
            '    'DBを開けない場合例外スロー
            '    Throw New Exception("ファイル内容なし")
            'End If

            'For intIdx As Integer = 0 To objFileData.Count - 1
            '    Dim strSKKanriNo As String = "" '照会管理番号

            '    '必要な値をファイルより取得
            '    strSKKanriNo = objFileData(intIdx)(ESHKakuninFileKmk.照会管理番号)

            '    If strIFSKKanriNo = strSKKanriNo Then
            '        Using trans As SqlTransaction = conDB.BeginTransaction
            '            Me.msUpdateSHData(trans, cmdDB, conDB, objFileData(intIdx))
            '            trans.Commit()
            '        End Using
            '    End If

            'Next

            'オブジェクトの破棄を保証する
            cmdDB.Dispose()

            Return True

        Catch ex As DBConcurrencyException
            Return False
        Catch ex As Exception
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If

            'ソケットクローズ
            If Not objStm Is Nothing Then
                objStm.Close()
            End If

            If Not objSck Is Nothing Then
                objSck.Close()
            End If

        End Try

    End Function

    ''' <summary>
    ''' 保存場所取得処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHozon(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByRef strFilePath As String) As Boolean
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_S1", conDB)

                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '保存先場所取得
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDs Is Nothing) And objDs.Tables.Count > 0 And strReturn <> "0" Then
                    Dim dtTable As DataTable
                    Dim objDataRow As DataRow
                    dtTable = objDs.Tables(0)
                    objDataRow = dtTable.Rows(0)
                    '保存先場所
                    strFilePath = "\\" & objDataRow(0) & "\" & objDataRow(1)
                Else
                    Throw New Exception("保存場所取得エラー")
                End If
            End Using

            Return True
        Catch ex As Exception
            strFilePath = strERRLOGPATH
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozon"
            strTable = "M78_PRESERVE_PLACE"
            strData = "SMTDEN001_S1"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 照会要求データ取得処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="objDataSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByRef objDataSet As DataSet, ByVal strFilePath As String, ByRef strErrKbn As String) As Boolean
        Try
            'エラー区分を設定（戻った際にCatchされたか、データ無しかの判定に使用）
            strErrKbn = "0"

            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_S2", conDB)

                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objDataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDataSet Is Nothing) And objDataSet.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozon"
            strTable = "D83_PRESERVE_PLACE"
            strData = "SMTDEN001_S2"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            strErrKbn = "1"
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 電文作成処理
    ''' </summary>
    ''' <param name="strTboxId"></param>
    ''' <param name="strProcCd"></param>
    ''' <param name="bytArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCreateDenbun(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                    ByVal strProcCd As String, ByVal strReqMngNo As String, _
                                    ByRef bytArray As Byte(), ByVal strTennaiFile As String, _
                                    ByVal strTboxId As String, ByVal strFilePath As String) As Boolean
        Select Case strProcCd
            Case "401"    '店内装置構成表配信依頼処理上り電文
                bytArray = New Byte(108579) {}
                If mfGetFileData(strTennaiFile, bytArray, strFilePath) = False Then
                    Return False
                End If
            Case "402"    '店内装置構成表反映指示処理上り電文
                bytArray = New Byte(19) {}
                If mfTennaiHaneishiji(cmdDB, conDB, bytArray, strTboxId, strReqMngNo, strFilePath) = False Then
                    Return False
                End If
            Case "901"    '即時集信依頼上り電文
                bytArray = New Byte(11) {}
                If mfSokujiSyushin(bytArray, strTboxId, strFilePath) = False Then
                    Return False
                End If
        End Select
        Return True
    End Function

    ''' <summary>
    ''' 格納フォルダ取得
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetBbkouseiFolderPath(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef strVal As String) As String
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_S4", conDB)

                cmdDB.Parameters.Add(pfSet_Param("prmM78_FILECLASS_CD", SqlDbType.NVarChar, strFILEBBKOUSEI))
                cmdDB.Parameters.Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '保存先場所取得
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDs Is Nothing) And objDs.Tables.Count > 0 And strReturn <> "0" Then
                    Dim dtTable As DataTable
                    Dim objDataRow As DataRow
                    dtTable = objDs.Tables(0)
                    objDataRow = dtTable.Rows(0)
                    '保存先場所
                    strVal = objDataRow(0) & "\" & objDataRow(1)
                Else
                    Throw New Exception("保存場所取得エラー")
                End If
            End Using

            Return True
        Catch ex As Exception
            strVal = strERRLOGPATH
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetHozon"
            strTable = "M78_PRESERVE_PLACE"
            strData = "SMTDEN001_S4"
            msDBErrLog(strVal, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 照会要求データ更新処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="dtmYoukyu"></param>
    ''' <param name="strTuban"></param>
    ''' <param name="strSEND"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByVal dtmYoukyu As DateTime, _
                                           ByVal strTuban As String, ByVal strSEND As String, ByVal strFilePath As String) As Boolean
        Try
            'エラーメッセージ項目設定
            strErrBunrui = "その他処理エラー"
            strTable = ""
            strData = ""
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_U1", conDB)

                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD83_REQ_DT", SqlDbType.DateTime, dtmYoukyu))
                    .Add(pfSet_Param("prmD83_REQ_SEQ", SqlDbType.NVarChar, strTuban))
                    .Add(pfSet_Param("prmD83_TLGRM_SND", SqlDbType.NVarChar, strSEND))
                    .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
                End With

                cmdDB.Transaction = trans1
                '実行
                cmdDB.ExecuteNonQuery()
                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                If intReturn <> 0 Then
                    'エラーメッセージ項目設定
                    trans1.Rollback()
                    strErrBunrui = "ＳＱＬ実行"
                    strTable = "D83_PRESERVE_PLACE"
                    strData = "SMTDEN001_U1"
                    Throw New Exception("ストアドプロシージャ：" & intReturn)
                Else
                    'コミッテッド
                    trans1.Commit()
                End If

            End Using
            Return True
        Catch ex As Exception
            strMesod = "mfUpdateReferenceData"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' 接続先情報取得処理
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strTboxId"></param>
    ''' <param name="objDtSetConnect"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetAccessPoint(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByVal strTboxId As String, _
                                      ByRef objDtSetConnect As DataSet, ByVal strFilePath As String) As Boolean
        'エラー区分を設定（戻った際にCatchされたか、データ無しかの判定に使用）
        Try
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_S3", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, strTboxId))
                    .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '照会要求データ取得処理
                objDtSetConnect = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDtSetConnect Is Nothing) And objDtSetConnect.Tables.Count > 0 And strReturn <> "0" Then
                    '該当データありの場合は既にDataSetに格納されているため何もしない。
                Else
                    '取得データなし
                    strErrBunrui = "その他処理エラー"
                    strMesod = "mfGetAccessPoint"
                    strTable = "M81_ACCESS_POINT"
                    strData = "SMTDEN001_S3"
                    msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                    Return False
                End If
            End Using
            Return True
        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetAccessPoint"
            strTable = "M81_ACCESS_POINT"
            strData = "SMTDEN001_S3"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' NGC決済センター接続
    ''' </summary>
    ''' <param name="strIpAddr"></param>
    ''' <param name="intPortNo"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="bytArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfNGCConnect(ByVal strIpAddr As String, ByVal intPortNo As Integer, ByVal strFilePath As String, _
                                  ByVal strProcCd As String, ByRef bytArray As Byte(), ByRef strRetrunMessage As String) As Boolean
        strErrBunrui = "ソケット接続エラー"
        strTable = ""
        strData = ""
        Try
            'ソケット生成
            objSck = New System.Net.Sockets.TcpClient()
            objSck.Connect(strIpAddr, intPortNo)
            Dim objStm As System.Net.Sockets.NetworkStream = objSck.GetStream()

            ' ソケット送信
            'bytArray = _
            '    System.Text.Encoding.GetEncoding("SHIFT-JIS").GetBytes("abcあいう")
            objStm.Write(bytArray, 0, bytArray.GetLength(0))

            msSeijyouLog(strProcCd, strFilePath, bytArray, "0")

            Dim timSpan As TimeSpan
            Dim dteStart As DateTime = Date.Now
            Dim dat As Byte() = Nothing

            'Dim enc As System.Text.Encoding = System.Text.Encoding.UTF8

            'Dim ms As New System.IO.MemoryStream()
            'Dim resBytes As Byte() = New Byte(255) {}
            'Do
            '    'データの一部を受信する
            '    Dim resSize As Integer = objStm.Read(resBytes, 0, resBytes.Length)
            '    'Readが0を返した時はサーバーが切断したと判断
            '    If resSize = 0 Then
            '        Console.WriteLine("サーバーが切断しました。")
            '        Exit Do
            '    End If
            '    '受信したデータを蓄積する
            '    ms.Write(resBytes, 0, resSize)
            'Loop While objStm.DataAvailable
            ''受信したデータを文字列に変換
            'Dim resMsg As String = enc.GetString(ms.ToArray())
            While 1
                ' ソケット受信
                If objSck.Available > 0 Then
                    dat = New Byte(objSck.Available - 1) {}
                    objStm.Read(dat, 0, dat.GetLength(0))
                    strRetrunMessage = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat)
                    Exit While
                End If
                timSpan = Date.Now - dteStart
                If timSpan.TotalSeconds > 60 Then
                    Throw New Exception("サーバから応答がありません")
                End If
            End While

            msSeijyouLog(strProcCd, strFilePath, dat, "1")

            Return True
        Catch ex As Exception
            strMesod = "mfNGCConnect"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 照会要求データ更新処理（電文終了）
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="dtmYoukyu"></param>
    ''' <param name="strTuban"></param>
    ''' <param name="strRECV"></param>
    ''' <param name="strRetrunMessage"></param>
    ''' <param name="strFilePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfEndUpdateReferenceData(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByVal dtmYoukyu As DateTime, _
                                              ByVal strTuban As String, ByVal strRECV As String, ByVal strRetrunMessage As String, _
                                              ByVal strFilePath As String) As Boolean
        Try
            'エラーメッセージ項目設定
            strErrBunrui = "その他処理エラー"
            strTable = ""
            strData = ""
            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_U2", conDB)

                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD83_REQ_DT", SqlDbType.DateTime, dtmYoukyu))
                    .Add(pfSet_Param("prmD83_REQ_SEQ", SqlDbType.NVarChar, strTuban))
                    .Add(pfSet_Param("prmD83_TLGRM_SND", SqlDbType.NVarChar, strRECV))
                    .Add(pfSet_Param("prmD83_RETURN_CD", SqlDbType.NVarChar, strRetrunMessage))
                    .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
                End With

                cmdDB.Transaction = trans1
                '実行
                cmdDB.ExecuteNonQuery()
                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                If intReturn <> 0 Then
                    'エラーメッセージ項目設定
                    trans1.Rollback()
                    strErrBunrui = "ＳＱＬ実行"
                    strTable = "D83_PRESERVE_PLACE"
                    strData = "SMTDEN001_U2"
                    Throw New Exception("ストアドプロシージャ：" & intReturn)
                Else
                    'コミッテッド
                    trans1.Commit()
                End If

            End Using
            Return True
        Catch ex As Exception
            strMesod = "mfEndUpdateReferenceData"
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' ファイルコンバート
    ''' </summary>
    ''' <param name="FullPah"></param>
    ''' <remarks></remarks>
    Private Function mfGetFileData(ByVal FullPah As String, ByRef bytArray As Byte(), ByVal strFilePath As String) As Boolean

        'Dim ds As New DataSet                    'データセット
        'Dim dt As New DataTable                  'データテーブル
        'Dim dtRow As DataRow                     'データ行
        Dim viewSt_Tboxid(20 - 1) As String      'ビューステート(先頭20バイト分保存)
        Dim viewSt_Futago(12 - 1) As String      'ビューステート(双子店情報)

        Dim intCnt As Integer = 0                'FROM - TO ループ用

        'ファイルサイズによるループの変更
        Dim intLoopNum() As Integer = {1500, _
                                       1998, _
                                       2998, _
                                       4500}    'ループ数
        Dim intNum As Integer = 0                '対象ループ数選択用

        'ファイルサイズによる予備取得バイト数変更
        Dim intYobiNum() As Integer = {2048, _
                                       16552, _
                                       3528, _
                                       3936}    'ファイルサイズ数

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

        '開始位置
        Dim intSyoriSentakuKbn As Integer = 0
        Dim intSyorisentaku As Integer = 2
        Dim intTboxId As Integer = 4
        Dim intTboxHead As Integer = 13
        Dim intBbSoudaisu As Integer = 16
        Dim intKaisen0setuzoku As Integer = 21
        Dim intKaisen1setuzoku As Integer = 26
        Dim intJbBango As Integer = 31
        Dim intUnyouKiban As Integer = 22531
        Dim intKaisenBango As Integer = 45031
        Dim intBbsyubetu As Integer = 54031
        Dim intBbkisyu As Integer = 63031
        Dim intShima As Integer = 72031
        Dim intFutagomise As Integer = 94531
        Dim intFutagoFlg As Integer = 108031
        Dim intTenpoFlg As Integer = 108033
        Dim intSandoFrom As Integer = 108039
        Dim intSandoTo As Integer = 108219
        Dim intKenbaiFrom As Integer = 108399
        Dim intKenbaiTo As Integer = 108429
        Dim intSeisanFrom As Integer = 108459
        Dim intSeisanTo As Integer = 108489
        Dim intTourokuFrom As Integer = 108519
        Dim intTourokuTo As Integer = 108549

        Try

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
                    'psMesBox(Me, "XXXXXX", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後, "ファイルサイズに誤りがあります")
                    Throw New Exception

            End Select

            '★バイト配列に格納（処理選択区分）
            bytArray(intSyoriSentakuKbn) = "2"
            '★バイト配列に格納（処理選択）
            bytArray(intSyorisentaku) = "3"

            Dim buf_yobi(intYobiNum(intNum)) As Byte                     'データ格納用配列(予備)

            'dt.Columns.Add("JB番号")
            'dt.Columns.Add("運用機番")
            'dt.Columns.Add("回線番号")
            'dt.Columns.Add("BB種別")
            'dt.Columns.Add("BB機種種別")
            'dt.Columns.Add("島番号")
            'dt.Columns.Add("店舗")
            'dt.Columns.Add("予備1")
            'dt.Columns.Add("予備2")

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
            'viewSt_Tboxid(0) = strTemp + "0"     '値 + NULL文字
            viewSt_Tboxid(0) = strTemp

            '★バイト配列に格納（TBOXID）
            Dim bytTmp As Byte()
            bytTmp = Nothing
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp.Substring(2, 8))
            Array.Copy(bytTmp, 0, bytArray, intTboxId, bytTmp.Length)

            Dim str As String
            str = System.Text.Encoding.GetEncoding(intENCODE).GetString(bytTmp)

            'If Not strTemp = Me.txtTboxID.ppText Then

            '    '処理終了
            '    psMesBox(Me, "XXXXXX", clscomver.E_Mタイプ.エラー, clscomver.E_Mモード.OK, clscomver.E_S実行.描画後, "ファイル内のTBOXIDに誤りがあります")
            '    Throw New Exception

            'End If

            'T-BOXハード種別(HEX)
            'viewSt_Tboxid(1) = Convert.ToString(buf_tboxid(5), 16).PadLeft(2, "0") + "0"     '値 + NULL文字
            viewSt_Tboxid(1) = Convert.ToString(buf_tboxid(5), 16).PadLeft(2, "0")

            '★バイト配列に格納（T-BOXハード種別）
            bytTmp = Nothing
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(viewSt_Tboxid(1))
            Array.Copy(bytTmp, 0, bytArray, intTboxHead, bytTmp.Length)


            '入力元情報(HEX)
            'viewSt_Tboxid(2) = Convert.ToString(buf_tboxid(6), 16).PadLeft(2, "0") + "0"     '値 + NULL文字
            viewSt_Tboxid(2) = Convert.ToString(buf_tboxid(6), 16).PadLeft(2, "0")

            '店内装置構成法反映日時(BCD)
            strTemp = Nothing
            For j As Integer = 0 To 7 - 1
                strBcd(j) = Convert.ToString(buf_tboxid(j + 7), 2).ToString.PadLeft(8, "0")
            Next
            'BCDに変換
            For j As Integer = 0 To 7 - 1
                strTemp = strTemp + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                strTemp = strTemp + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
            Next
            'viewSt_Tboxid(3) = strTemp + "0"     '値 + NULL文字
            viewSt_Tboxid(3) = strTemp

            'BB総台数を取得
            strBBsum = Convert.ToString(buf_tboxid(14), 16).PadLeft(2, "0")
            strBBsum = strBBsum + Convert.ToString(buf_tboxid(15), 16).PadLeft(2, "0")
            intBBsum = Convert.ToInt32(strBBsum, 16)
            'viewSt_Tboxid(4) = Convert.ToInt32(strBBsum, 16).ToString + "0"     '値 + NULL文字
            viewSt_Tboxid(4) = Convert.ToInt32(strBBsum, 16).ToString

            '★バイト配列に格納（BB総台数）
            bytTmp = Nothing
            'bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(viewSt_Tboxid(4))
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strBBsum)
            Array.Copy(bytTmp, 0, bytArray, intBbSoudaisu, bytTmp.Length)


            '回線0接続数を取得
            strTemp = Nothing
            strTemp = Convert.ToString(buf_tboxid(16), 16).PadLeft(2, "0")
            strTemp = strTemp + Convert.ToString(buf_tboxid(17), 16).PadLeft(2, "0")
            'viewSt_Tboxid(5) = Convert.ToInt32(strTemp, 16).ToString + "0"     '値 + NULL文字
            viewSt_Tboxid(5) = Convert.ToInt32(strTemp, 16).ToString

            '★バイト配列に格納（回線0接続数）
            bytTmp = Nothing
            'bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(viewSt_Tboxid(5))
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
            Array.Copy(bytTmp, 0, bytArray, intKaisen0setuzoku, bytTmp.Length)

            '回線1接続数を取得
            strTemp = Nothing
            strTemp = Convert.ToString(buf_tboxid(18), 16).PadLeft(2, "0")
            strTemp = strTemp + Convert.ToString(buf_tboxid(19), 16).PadLeft(2, "0")
            'viewSt_Tboxid(6) = Convert.ToInt32(strTemp, 16).ToString + "0"     '値 + NULL文字
            viewSt_Tboxid(6) = Convert.ToInt32(strTemp, 16).ToString

            '★バイト配列に格納（回線1接続数）
            bytTmp = Nothing
            'bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(viewSt_Tboxid(6))
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
            Array.Copy(bytTmp, 0, bytArray, intKaisen1setuzoku, bytTmp.Length)

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

                        'dtRow = dt.NewRow         'データ行

                        'JB番号(BCD)
                        strBcd(0) = Convert.ToString(buf_kobetsu(0), 2).ToString.PadLeft(8, "0")
                        strBcd(1) = Convert.ToString(buf_kobetsu(1), 2).ToString.PadLeft(8, "0")
                        'BCDに変換
                        For j As Integer = 0 To 2 - 1
                            strJBNum = strJBNum + Convert.ToInt32(strBcd(j).Substring(0, 4), 2).ToString
                            strJBNum = strJBNum + Convert.ToInt32(strBcd(j).Substring(4, 4), 2).ToString
                        Next

                        '★バイト配列に格納（JB番号）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strJBNum)
                        Array.Copy(bytTmp, 0, bytArray, intJbBango, bytTmp.Length)
                        intJbBango = intJbBango + 5

                        '回線番号(CHR)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(2), 16), 16)
                        strKaisenNum = Encoding.UTF8.GetString(bytChr)

                        '★バイト配列に格納（回線番号）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strKaisenNum)
                        Array.Copy(bytTmp, 0, bytArray, intKaisenBango, bytTmp.Length)
                        intKaisenBango = intKaisenBango + 2

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

                        '★バイト配列に格納（運用機番）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strUnyokiban)
                        Array.Copy(bytTmp, 0, bytArray, intUnyouKiban, bytTmp.Length)
                        intUnyouKiban = intUnyouKiban + 5

                        'BB種別コード(CHR)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(6), 16), 16)
                        strBBsyubetsu = Encoding.UTF8.GetString(bytChr)

                        '★バイト配列に格納（BB種別コード）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strBBsyubetsu)
                        Array.Copy(bytTmp, 0, bytArray, intBbsyubetu, bytTmp.Length)
                        intBbsyubetu = intBbsyubetu + 2

                        'HEX(0.5バイト用)
                        strHex = Convert.ToString(buf_kobetsu(7), 16).ToString.PadLeft(2, "0")

                        'BB機器種別(HEX)
                        strBBkisyu = strHex.Substring(0, 1)

                        '★バイト配列に格納（BB機器種別）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strBBkisyu)
                        Array.Copy(bytTmp, 0, bytArray, intBbkisyu, bytTmp.Length)
                        intBbkisyu = intBbkisyu + 2

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

                        '★バイト配列に格納（島番号）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strShimaban)
                        Array.Copy(bytTmp, 0, bytArray, intShima, bytTmp.Length)
                        intShima = intShima + 5

                        '予備2(CHR)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(10), 16), 16)
                        strYobi2 = Encoding.UTF8.GetString(bytChr)
                        bytChr(0) = Convert.ToInt32(Convert.ToString(buf_kobetsu(11), 16), 16)
                        strYobi2 = strYobi2 + Encoding.UTF8.GetString(bytChr)

                        '★バイト配列に格納（予備2を双子店番号とみなす（バイトサイズから判断））
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strYobi2)
                        Array.Copy(bytTmp, 0, bytArray, intFutagomise, bytTmp.Length)
                        intFutagomise = intFutagomise + 3

                        ''データ行に設定
                        'dtRow("JB番号") = strJBNum
                        'dtRow("運用機番") = strUnyokiban
                        'dtRow("回線番号") = strKaisenNum
                        'dtRow("BB種別") = strBBsyubetsu
                        'dtRow("BB機種種別") = strBBkisyu
                        'dtRow("島番号") = strShimaban
                        'dtRow("店舗") = strTenpo
                        'dtRow("予備1") = strYobi1
                        'dtRow("予備2") = strYobi2

                        ''データテーブルにセット
                        'dt.Rows.Add(dtRow)

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

            'strYobi2 = Nothing

            '双子店設定有無フラグの情報を取得(1バイト)
            readSize = fs.Read(buf_futago, 0, Math.Min(1, remain))
            strHex = Convert.ToString(buf_futago(0), 16).ToString.PadLeft(2, "0")

            '★バイト配列に格納（双子店設定有無フラグ）
            bytTmp = Nothing
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strHex)
            Array.Copy(bytTmp, 0, bytArray, intFutagoFlg, bytTmp.Length)
            intFutagoFlg = intFutagoFlg + 2

            Select Case strHex

                Case "00"

                    viewSt_Futago(0) = strHex + "0"

                Case Else

                    For j As Integer = 0 To 3 - 1

                        '店舗有無フラグ
                        strHex = Nothing
                        readSize = fs.Read(buf_tenpoJyoho, 0, Math.Min(137, remain))
                        strHex = Convert.ToString(buf_tenpoJyoho(0), 16).ToString.PadLeft(2, "0")
                        viewSt_Futago(1) = viewSt_Futago(1) + strHex + "0"

                        '★バイト配列に格納（店舗有無フラグ）
                        bytTmp = Nothing
                        bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strHex)
                        Array.Copy(bytTmp, 0, bytArray, intTenpoFlg, bytTmp.Length)
                        intTenpoFlg = intTenpoFlg + 2

                        '券売入金機機番(FROM - TO)
                        strTemp = Nothing
                        For k As Integer = 0 To 8 - 1
                            strBcd(k) = Convert.ToString(buf_tenpoJyoho(k + 1), 2).ToString.PadLeft(8, "0")
                        Next
                        'BCDに変換
                        intCnt = 0
                        For k As Integer = 0 To 4 - 1
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(4, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(4, 4), 2).ToString
                            'viewステートに格納
                            If intCnt Mod 2 = 0 Then
                                viewSt_Futago(2) = viewSt_Futago(2) + strTemp + "0"

                                '★バイト配列に格納（券売入金機機番FROM）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intKenbaiFrom, bytTmp.Length)
                                intKenbaiFrom = intKenbaiFrom + 5

                            Else
                                viewSt_Futago(3) = viewSt_Futago(3) + strTemp + "0"

                                '★バイト配列に格納（券売入金機機番TO）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intKenbaiTo, bytTmp.Length)
                                intKenbaiTo = intKenbaiTo + 5
                            End If
                            strTemp = Nothing
                            intCnt = intCnt + 1
                        Next

                        '精算機機番(FROM - TO)
                        strTemp = Nothing
                        For k As Integer = 0 To 8 - 1
                            strBcd(k) = Convert.ToString(buf_tenpoJyoho(k + 9), 2).ToString.PadLeft(8, "0")
                        Next
                        'BCDに変換
                        intCnt = 0
                        For k As Integer = 0 To 4 - 1
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(4, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(4, 4), 2).ToString
                            'viewステートに格納
                            If intCnt Mod 2 = 0 Then
                                viewSt_Futago(4) = viewSt_Futago(4) + strTemp + "0"

                                '★バイト配列に格納（精算機機番FROM）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intSeisanFrom, bytTmp.Length)
                                intSeisanFrom = intSeisanFrom + 5

                            Else
                                viewSt_Futago(5) = viewSt_Futago(5) + strTemp + "0"

                                '★バイト配列に格納（精算機機番TO）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intSeisanTo, bytTmp.Length)
                                intSeisanTo = intSeisanTo + 5

                            End If
                            strTemp = Nothing
                            intCnt = intCnt + 1
                        Next

                        '登録受付機機番(FROM - TO)
                        strTemp = Nothing
                        For k As Integer = 0 To 8 - 1
                            strBcd(k) = Convert.ToString(buf_tenpoJyoho(k + 17), 2).ToString.PadLeft(8, "0")
                        Next
                        'BCDに変換
                        intCnt = 0
                        For k As Integer = 0 To 4 - 1
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(4, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(4, 4), 2).ToString
                            'viewステートに格納
                            If intCnt Mod 2 = 0 Then
                                viewSt_Futago(6) = viewSt_Futago(6) + strTemp + "0"

                                '★バイト配列に格納（登録受付機機番FROM）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intTourokuFrom, bytTmp.Length)
                                intTourokuFrom = intTourokuFrom + 5

                            Else
                                viewSt_Futago(7) = viewSt_Futago(7) + strTemp + "0"

                                '★バイト配列に格納（登録受付機機番To）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intTourokuTo, bytTmp.Length)
                                intTourokuTo = intTourokuTo + 5

                            End If
                            strTemp = Nothing
                            intCnt = intCnt + 1
                        Next

                        'サンド島番号(FROM - TO)
                        strTemp = Nothing
                        For k As Integer = 0 To 48 - 1
                            strBcd(k) = Convert.ToString(buf_tenpoJyoho(k + 25), 2).ToString.PadLeft(8, "0")
                        Next
                        'BCDに変換
                        intCnt = 0
                        For k As Integer = 0 To 24 - 1
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt).Substring(4, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(0, 4), 2).ToString
                            strTemp = strTemp + Convert.ToInt32(strBcd(k + intCnt + 1).Substring(4, 4), 2).ToString
                            'viewステートに格納
                            If intCnt Mod 2 = 0 Then
                                viewSt_Futago(8) = viewSt_Futago(8) + strTemp + "0"

                                '★バイト配列に格納（サンド島番号FROM）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intSandoFrom, bytTmp.Length)
                                intSandoFrom = intSandoFrom + 5

                            Else
                                viewSt_Futago(9) = viewSt_Futago(9) + strTemp + "0"

                                '★バイト配列に格納（サンド島番号TO）
                                bytTmp = Nothing
                                bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTemp)
                                Array.Copy(bytTmp, 0, bytArray, intSandoTo, bytTmp.Length)
                                intSandoTo = intSandoTo + 5

                            End If
                            strTemp = Nothing
                            intCnt = intCnt + 1
                        Next

                        '予備3(CHR)
                        For k As Integer = 0 To 64 - 1
                            viewSt_Futago(10) = viewSt_Futago(10) + Convert.ToString(buf_tenpoJyoho(k + 73), 16)
                        Next
                        viewSt_Futago(10) = Convert.ToInt32(viewSt_Futago(10), 16).ToString
                        viewSt_Futago(10) = viewSt_Futago(10) + "0"

                        '読み込み位置の更新
                        remain -= readSize

                    Next

                    readSize = fs.Read(buf_yobi, 0, Math.Min(intYobiNum(intNum), remain))

                    For j As Integer = 0 To readSize - 1

                        viewSt_Futago(11) = viewSt_Futago(11) + Convert.ToString(buf_yobi(j), 16)
                    Next
                    viewSt_Futago(11) = Convert.ToInt32(viewSt_Futago(11), 16).ToString
                    viewSt_Futago(11) = viewSt_Futago(11) + "0"

            End Select

            Return True

        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfGetFileData"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try


    End Function

    ''' <summary>
    ''' 即時集信依頼上り電文作成
    ''' </summary>
    ''' <param name="bytArray"></param>
    ''' <param name="strTboxId"></param>
    ''' <remarks></remarks>
    Private Function mfTennaiHaneishiji(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, _
                                        ByRef bytArray As Byte(), ByVal strTboxId As String, _
                                        ByVal strReqMngNo As String, ByVal strFilePath As String) As String
        Try
            Dim intSyoriSentakuKbn As Integer = 0
            Dim intSyorisentaku As Integer = 2
            Dim intTboxId As Integer = 4
            Dim intKoushinRenban As Integer = 13

            '★バイト配列に格納（処理選択区分）
            bytArray(intSyoriSentakuKbn) = "3"

            '★バイト配列に格納（処理選択）
            bytArray(intSyorisentaku) = "4"

            '★バイト配列に格納（TBOXID）
            Dim bytTmp As Byte()
            bytTmp = Nothing
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTboxId)
            Array.Copy(bytTmp, 0, bytArray, intTboxId, bytTmp.Length)

            Using trans1 As SqlTransaction = conDB.BeginTransaction
                'プロシージャ設定
                cmdDB = New SqlCommand("SMTDEN001_S5", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("prmD103_CTRL_NO", SqlDbType.NVarChar, strReqMngNo))
                    .Add(pfSet_Param("ret", SqlDbType.NVarChar, 1, ParameterDirection.Output))
                End With
                'ストアドプロシージャで実行
                cmdDB.CommandType = CommandType.StoredProcedure

                'トランザクション
                cmdDB.Transaction = trans1

                '配信管理DB取得処理
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
                Dim strReturn As String = cmdDB.Parameters("ret").Value.ToString

                If (Not objDs Is Nothing) And objDs.Tables.Count > 0 And strReturn <> "0" Then
                    '取得できた内容を設定する
                    Dim dtTable As DataTable
                    Dim objDataRow As DataRow
                    dtTable = objDs.Tables(0)
                    objDataRow = dtTable.Rows(0)

                    '★バイト配列に格納（更新通番）
                    bytTmp = Nothing
                    bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(objDataRow(0).ToString.PadLeft(5, "0"))
                    Array.Copy(bytTmp, 0, bytArray, intKoushinRenban, bytTmp.Length)
                Else
                    '取得データなし
                    strErrBunrui = "その他処理エラー"
                    strMesod = "mfTennaiHaneishiji"
                    strTable = "D103_TM_HISN_KNR"
                    strData = "SMTDEN001_S4"
                    msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, "該当レコードなし")
                    Return False
                End If
            End Using
            Return True

        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfTennaiHaneishiji"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function


    ''' <summary>
    ''' 即時集信依頼上り電文作成
    ''' </summary>
    ''' <param name="bytArray"></param>
    ''' <param name="strTboxId"></param>
    ''' <remarks></remarks>
    Private Function mfSokujiSyushin(ByRef bytArray As Byte(), ByVal strTboxId As String, ByVal strFilePath As String) As String
        Try
            Dim intSyoriSentakuKbn As Integer = 0
            Dim intTboxId As Integer = 2

            '★バイト配列に格納（処理選択区分）
            bytArray(intSyoriSentakuKbn) = "9"

            '★バイト配列に格納（TBOXID）
            Dim bytTmp As Byte()
            bytTmp = Nothing
            bytTmp = System.Text.Encoding.GetEncoding(intENCODE).GetBytes(strTboxId)
            Array.Copy(bytTmp, 0, bytArray, intTboxId, bytTmp.Length)

            Return True

        Catch ex As Exception
            strErrBunrui = "その他処理エラー"
            strMesod = "mfSokujiSyushin"
            strTable = ""
            strData = ""
            msDBErrLog(strFilePath, strErrBunrui, strMesod, strTable, strData, ex.ToString)
            Return False
        End Try

    End Function





    ' ''' <summary>
    ' ''' ファイル名確認
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfChkFileExists(ByVal strFileName As String) As Boolean

    '    '空白の場合は比較処理をおこなわない
    '    If strFileName.Trim() = "" Then
    '        Return False
    '    End If

    '    'ファイル存在チェック
    '    If System.IO.File.Exists(strFileName) Then
    '        Return True
    '    Else
    '        Return False
    '    End If

    'End Function


    ' ''' <summary>
    ' ''' 集配信データ更新
    ' ''' </summary>
    ' ''' <param name="trans"></param>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <remarks></remarks>
    'Private Sub msUpdateSHData(ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal strFileData() As String)

    '    Dim strProcedureNm As String = ""

    '    Select Case Convert.ToInt32(strFileData(ESHKakuninFileKmk.処理区分))
    '        Case EShoriKbn.即時集信
    '            strProcedureNm = ""
    '        Case EShoriKbn.随時照会
    '            strProcedureNm = ""
    '        Case EShoriKbn.配信
    '            strProcedureNm = ""
    '        Case EShoriKbn.リアル通知
    '            strProcedureNm = ""
    '        Case Else
    '            Throw New Exception("ファイル異常")
    '    End Select

    '    'プロシージャ設定
    '    cmdDB = New SqlCommand(strProcedureNm, conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    'パラメータ設定
    '    With cmdDB.Parameters
    '        '.Add(pfSet_Param("prmT01_HALL_CD", SqlDbType.NVarChar, objRow("D02_HALL_CD"))) 'ホールコード
    '    End With

    '    '実行
    '    cmdDB.ExecuteNonQuery()

    'End Sub

    ' ''' <summary>
    ' ''' 店内装置構成表配信依頼処理上り電文作成
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpTennaiStiKoseiListHaisinIrai(ByRef objStrct As StrctUpTennaiHaisin) As String
    '    Try
    '        '処理選択区分
    '        With objStrct
    '            '処理選択区分
    '            .strShSntkKbn = "2"
    '            .strShSntkKbn = SetNull(.strShSntkKbn, 2)

    '            '処理選択
    '            .strShSntk = "3"
    '            .strShSntk = SetNull(.strShSntk, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            'T-BOXハード種別
    '            .strTboxHdSbt = SetNull(.strTboxHdSbt, 3)

    '            'BB総台数
    '            .strBBGrandCnt = SetNull(.strBBGrandCnt, 5)

    '            '回線0接続台数
    '            .strKaisen0ConCnt = SetNull(.strKaisen0ConCnt, 5)

    '            '回線1接続台数
    '            .strKaisen1ConCnt = SetNull(.strKaisen1ConCnt, 5)

    '            'JB番号
    '            .strJBNo = SetNull(GetLoopStr(.strJBNo, 4500), 22500)

    '            '運用機番
    '            .strUnyoKiban = SetNull(GetLoopStr(.strUnyoKiban, 4500), 22500)

    '            '回線番号
    '            .strKaisenNo = SetNull(GetLoopStr(.strKaisenNo, 4500), 9000)

    '            'BB種別コード
    '            .strBBSbtCd = SetNull(GetLoopStr(.strBBSbtCd, 4500), 9000)

    '            'BB機種種別
    '            .strBBKisyuSbt = SetNull(GetLoopStr(.strBBKisyuSbt, 4500), 9000)

    '            '島番号
    '            .strSimaNo = SetNull(GetLoopStr(.strSimaNo, 4500), 22500)

    '            '双子店番号
    '            .strGeminiNo = SetNull(GetLoopStr(.strGeminiNo, 4500), 13500)

    '            '双子店設定有無フラグ
    '            .strGeminiStiYNFlg = SetNull(.strGeminiStiYNFlg, 2)

    '            '店舗設定有無フラグ
    '            .strTenpoStiYNFlg = SetNull(GetLoopStr(.strTenpoStiYNFlg, 3), 6)

    '            'サンド島番号(From)
    '            .strSandoSimaNoFm = SetNull(GetLoopStr(.strSandoSimaNoFm, 36), 180)

    '            'サンド島番号(To)
    '            .strSandoSimaNoTo = SetNull(GetLoopStr(.strSandoSimaNoTo, 36), 180)

    '            '券売入金機機番(From)
    '            .strKenbaiNyukinKibanFm = SetNull(GetLoopStr(.strKenbaiNyukinKibanFm, 6), 30)

    '            '券売入金機機番(To)
    '            .strKenbaiNyukinKibanTo = SetNull(GetLoopStr(.strKenbaiNyukinKibanTo, 6), 30)

    '            '精算機機番(From)
    '            .strSeisankiKibanFm = SetNull(GetLoopStr(.strSeisankiKibanFm, 6), 30)

    '            '精算機機番(To)
    '            .strSeisankiKibanTo = SetNull(GetLoopStr(.strSeisankiKibanTo, 6), 30)

    '            '登録受付機機番(From)
    '            .strTorokuUktkKibanFm = SetNull(GetLoopStr(.strTorokuUktkKibanFm, 6), 30)

    '            '登録受付機機番(To)
    '            .strTorokuUktkKibanTo = SetNull(GetLoopStr(.strTorokuUktkKibanTo, 6), 30)

    '            Return .strShSntkKbn & _
    '               .strShSntk & _
    '               .strTboxId & _
    '               .strTboxHdSbt & _
    '               .strBBGrandCnt & _
    '               .strKaisen0ConCnt & _
    '               .strKaisen1ConCnt & _
    '               .strJBNo & _
    '               .strUnyoKiban & _
    '               .strKaisenNo & _
    '               .strBBSbtCd & _
    '               .strBBKisyuSbt & _
    '               .strSimaNo & _
    '               .strGeminiNo & _
    '               .strGeminiStiYNFlg & _
    '               .strTenpoStiYNFlg & _
    '               .strSandoSimaNoFm & _
    '               .strSandoSimaNoTo & _
    '               .strKenbaiNyukinKibanFm & _
    '               .strKenbaiNyukinKibanTo & _
    '               .strSeisankiKibanFm & _
    '               .strSeisankiKibanTo & _
    '               .strTorokuUktkKibanFm & _
    '               .strTorokuUktkKibanTo
    '        End With
    '    Finally
    '        objStrct = Nothing
    '    End Try

    'End Function

    ' ''' <summary>
    ' ''' 店内装置構成表反映指示処理上り電文作成
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunTennaiStiKoseiListHaneiSiji(ByRef objStrct As StrctUpTennaiHanei)
    '    Try
    '        With objStrct
    '            '処理選択区分
    '            .strShoriSelKbn = "3"
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = "4"
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            '更新通番
    '            .strUpdateRenban = SetNull(.strUpdateRenban, 6)

    '            Return .strShoriSelKbn & _
    '                   .strShoriSel & _
    '                   .strTboxId & _
    '                   .strUpdateRenban
    '        End With
    '    Finally
    '        objStrct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' T-BOX制御情報更新処理上り電文作成
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpTBoxSeigyoInfoUpdate(ByVal objStruct As StrctUpTboxSeigyoInfoUpdate) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            '保守モード操作可能SW
    '            .strHosyuModeSw = SetNull(.strHosyuModeSw, 2)

    '            '配送情報突合SW
    '            .strHaisoInfoSw = SetNull(.strHaisoInfoSw, 2)

    '            '特別運用モードSW
    '            .strTokubetuSw = SetNull(.strTokubetuSw, 2)

    '            '使用中DBチェックSW
    '            .strUsingDBSw = SetNull(.strUsingDBSw, 2)

    '            'チェーン店BB取り込み制御SW<運用モード>
    '            .strChainBBUnyoSw = SetNull(.strChainBBUnyoSw, 2)

    '            'チェーン店BB取り込み制御SW<保守モード>
    '            .strChainBBHosyuSw = SetNull(.strChainBBHosyuSw, 2)

    '            '他店BB取り込み制御SW<運用モード>
    '            .strTatenBBUnyoSw = SetNull(.strTatenBBUnyoSw, 2)

    '            '他店BB取り込み制御SW<保守モード>
    '            .strTatenBBHosyuSw = SetNull(.strTatenBBHosyuSw, 2)

    '            '代理店BB取り込み制御SW<運用モード>
    '            .strDairiBBUnyoSw = SetNull(.strDairiBBUnyoSw, 2)

    '            '代理店BB取り込み制御SW<保守モード>
    '            .strDairiBBHosyuSw = SetNull(.strDairiBBHosyuSw, 2)

    '            '0円カード/コイン許可SW
    '            .strZeroCardSw = SetNull(.strZeroCardSw, 2)

    '            '玉単価設定許可SW
    '            .strTamaTankaSw = SetNull(.strTamaTankaSw, 2)

    '            '使用中DBローカル照会制御SW
    '            .strUsingDBShokaiSw = SetNull(.strUsingDBShokaiSw, 2)

    '            'オフラインローカル照会制御SW(サンド)
    '            .strOfflineSandoSw = SetNull(.strOfflineSandoSw, 2)

    '            'オフラインローカル照会制御SW(券売入金機)
    '            .strOfflineKenbaiSw = SetNull(.strOfflineKenbaiSw, 2)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                   .strShoriSel & _
    '                   .strTboxId & _
    '                   .strHosyuModeSw & _
    '                   .strHaisoInfoSw & _
    '                   .strTokubetuSw & _
    '                   .strUsingDBSw & _
    '                   .strChainBBUnyoSw & _
    '                   .strChainBBHosyuSw & _
    '                   .strTatenBBUnyoSw & _
    '                   .strTatenBBHosyuSw & _
    '                   .strDairiBBUnyoSw & _
    '                   .strDairiBBHosyuSw & _
    '                   .strZeroCardSw & _
    '                   .strTamaTankaSw & _
    '                   .strUsingDBShokaiSw & _
    '                   .strOfflineSandoSw & _
    '                   .strOfflineKenbaiSw & _
    '                   .strUpdateNo
    '        End With

    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' BB運用情報更新処理上り電文作成
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpBBUnyoInfoUpdate(ByRef objStruct As StrctUpBBUnyoInfoUpdate) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTBoxId = SetNull(.strTBoxId, 9)

    '            '会社区分
    '            .strCampanyKbn = SetNull(GetLoopStr(.strCampanyKbn, 44), 88)

    '            'カード種別
    '            .strCardSbt = SetNull(GetLoopStr(.strCardSbt, 44), 88)

    '            '券面金額
    '            .strKenmenKngk = SetNull(GetLoopStr(.strKenmenKngk, 44), 264)

    '            'プレミアム金額
    '            .strPremiumKngk = SetNull(GetLoopStr(.strPremiumKngk, 44), 220)

    '            '券売フラグ
    '            .strKenbaiFlg = SetNull(GetLoopStr(.strKenbaiFlg, 44), 88)

    '            '台間フラグ
    '            .strDaikanFlg = SetNull(GetLoopStr(.strDaikanFlg, 44), 88)

    '            '消費フラグ
    '            .strShohiFlg = SetNull(GetLoopStr(.strShohiFlg, 44), 88)

    '            '消費単位
    '            .strUseTani = SetNull(.strUseTani, 5)

    '            '消費限度額
    '            .strShohiGendoKngk = SetNull(.strShohiGendoKngk, 6)

    '            '媒体累積入金限度額
    '            .strBaitaiRuisekiGendoKngk = SetNull(.strBaitaiRuisekiGendoKngk, 4)

    '            'カード有効期間
    '            .strCardYukoKikan = SetNull(.strCardYukoKikan, 4)

    '            'オフライン運用可能時間帯from
    '            .strOfflineUnyoFm = SetNull(.strOfflineUnyoFm, 5)

    '            'オフライン運用可能時間帯to
    '            .strOfflineUnyoTo = SetNull(.strOfflineUnyoTo, 5)

    '            'オフライン運用許可時間
    '            .strOfflineUnyoKyoka = SetNull(.strOfflineUnyoKyoka, 4)

    '            'オフライン発券・入金可能金額（券売機）
    '            .strOfflineKenbaiHakkenKngk = SetNull(.strOfflineKenbaiHakkenKngk, 4)

    '            'オフライン発券・入金可能枚数（券売機）
    '            .strOfflineKenbaiHakkenMaisu = SetNull(.strOfflineKenbaiHakkenMaisu, 4)

    '            'オフライン入金可能金額（サンド）
    '            .strOfflineSandoNyukinKngk = SetNull(.strOfflineSandoNyukinKngk, 3)

    '            'オフライン入金可能枚数（サンド）
    '            .strOfflineSandoNyukinMaisu = SetNull(.strOfflineSandoNyukinMaisu, 3)

    '            'オフライン消費可能金額
    '            .strOfflineShohiKngk = SetNull(.strOfflineShohiKngk, 3)

    '            'オフライン消費可能枚数
    '            .strOfflineShohiMaisu = SetNull(.strOfflineShohiMaisu, 3)

    '            '会員サービスコード１
    '            .strKaiinSvcCode1 = SetNull(.strKaiinSvcCode1, 9)

    '            '会員サービスコード２
    '            .strKaiinSvcCode2 = SetNull(.strKaiinSvcCode2, 9)

    '            '基準玉/メダル数情報(現在)基準玉数
    '            .strKijunTamasuNow = SetNull(.strKijunTamasuNow, 3)

    '            '基準玉/メダル数情報(現在)基準メダル数
    '            .strKijunMedarusuNow = SetNull(.strKijunMedarusuNow, 3)

    '            '基準玉/メダル数情報(未来)基準玉数
    '            .strKijunTamasuFuture = SetNull(.strKijunTamasuFuture, 3)

    '            '基準玉/メダル数情報(未来)基準メダル数
    '            .strKijunMedarusuFuture = SetNull(.strKijunMedarusuFuture, 3)

    '            '基準玉/メダル数反映年月日
    '            .strKijunTamaHaneiYmd = SetNull(.strKijunTamaHaneiYmd, 7)

    '            '消費税運用情報(現在)消費税区分
    '            .strShohizeiKbnNow = SetNull(.strShohizeiKbnNow, 2)

    '            '消費税運用情報(現在)消費税運用設定
    '            .strShohizeiUnyoSetteiNow = SetNull(.strShohizeiUnyoSetteiNow, 2)

    '            '消費税運用情報(現在)消費税率
    '            .strShohizeiRituNow = SetNull(.strShohizeiRituNow, 4)

    '            '消費税運用情報(未来)消費税区分
    '            .strShohizeiKbnFuture = SetNull(.strShohizeiKbnFuture, 2)

    '            '消費税運用情報(未来)消費税運用設定
    '            .strShohizeiUnyoSetteiNow = SetNull(.strShohizeiUnyoSetteiFuture, 2)

    '            '消費税運用情報(未来)消費税率
    '            .strShohizeiRituFuture = SetNull(.strShohizeiRituFuture, 4)

    '            '消費税反映年月日
    '            .strShohizeiHaneiYmd = SetNull(.strShohizeiHaneiYmd, 3)

    '            'エブリマネー有効期間
    '            .strEveryMoney = SetNull(.strEveryMoney, 4)

    '            '会員カード受付有無
    '            .strKaiinCardUktk = SetNull(.strKaiinCardUktk, 2)

    '            '一般0円カード入金
    '            .strIppanZeroCard = SetNull(.strIppanZeroCard, 2)

    '            '会員0円カード入金
    '            .strKaiinZeroCard = SetNull(.strKaiinZeroCard, 2)

    '            '追加入金条件設定
    '            .strTuikaNyukinJoken = SetNull(.strTuikaNyukinJoken, 2)

    '            '一般カード0円時排出方向
    '            .strIppanZeroCardOut = SetNull(.strIppanZeroCardOut, 2)

    '            '玉貸機(台毎用)タイマ値
    '            .strTamakashiTimerVal = SetNull(.strTamakashiTimerVal, 4)

    '            '玉貸機(台毎用)しきい値
    '            .strTamakashiShikiiVal = SetNull(.strTamakashiShikiiVal, 4)

    '            'カードユニット(パチンコ)タイマ値
    '            .strCardUnitPtnkTimerVal = SetNull(.strCardUnitPtnkTimerVal, 4)

    '            'カードユニット(パチンコ)しきい値
    '            .strCardUnitPtnkShikiiVal = SetNull(.strCardUnitPtnkShikiiVal, 4)

    '            'カードユニット(パチスロ)タイマ値
    '            .strCardUnitSlotTimerVal = SetNull(.strCardUnitSlotTimerVal, 4)

    '            'カードユニット(パチスロ)しきい値
    '            .strCardUnitSlotShikiiVal = SetNull(.strCardUnitSlotShikiiVal, 4)

    '            'ICメダル貸機(台毎)タイマ値
    '            .strICMedalDaigotoTimerVal = SetNull(.strICMedalDaigotoTimerVal, 4)

    '            'ICメダル貸機(台毎)しきい値
    '            .strICMedalDaigotoShikiiVal = SetNull(.strICMedalDaigotoShikiiVal, 4)

    '            'ICメダル貸機(台間)タイマ値
    '            .strICMedalDaikanTimerVal = SetNull(.strICMedalDaikanTimerVal, 4)

    '            'ICメダル貸機(台間)しきい値
    '            .strICMedalDaikanShikiiVal = SetNull(.strICMedalDaikanShikiiVal, 4)

    '            '玉貸単価機能設定
    '            .strTamakashiTankaKino = SetNull(.strTamakashiTankaKino, 2)

    '            '会員カードパスワード入力有無
    '            .strKaiinCardPassInput = SetNull(.strKaiinCardPassInput, 2)

    '            '一般カード0円時回収方向
    '            .strIppanZeroCardKaisyu = SetNull(.strIppanZeroCardKaisyu, 2)

    '            '会員カード受付有無(サ)
    '            .strKaiinCardUktkSando = SetNull(.strKaiinCardUktkSando, 2)

    '            '台間入金機能許可/抑止
    '            .strDaikanNyukinKino = SetNull(.strDaikanNyukinKino, 2)

    '            '受入禁止券種設定500円玉
    '            .strUkeireOut500 = SetNull(.strUkeireOut500, 2)

    '            '受入禁止券種設定1000円札
    '            .strUkeireOut1000 = SetNull(.strUkeireOut1000, 2)

    '            '受入禁止券種設定2000円札
    '            .strUkeireOut2000 = SetNull(.strUkeireOut2000, 2)

    '            '受入禁止券種設定5000円札
    '            .strUkeireOut5000 = SetNull(.strUkeireOut5000, 2)

    '            '受入禁止券種設定10000円札
    '            .strUkeireOut10000 = SetNull(.strUkeireOut10000, 2)

    '            '一般0円カード入金（サ）
    '            .strIppanZeroCardSando = SetNull(.strIppanZeroCardSando, 2)

    '            '会員0円カード入金(サ)
    '            .strKaiinZeroCardSando = SetNull(.strKaiinZeroCardSando, 2)

    '            '追加入金条件設定(サ)
    '            .strTuikaJokenSando = SetNull(.strTuikaJokenSando, 2)

    '            '暗証照合種別
    '            .strAnsyoShogoSbt = SetNull(.strAnsyoShogoSbt, 2)

    '            'エブリマネー精算設定
    '            .strEveryMoneySeisan = SetNull(.strEveryMoneySeisan, 2)

    '            '精算可能カード種別設定
    '            .strSeisanCardSbt = SetNull(.strSeisanCardSbt, 2)

    '            '精算後0円会員カード排出方向設定
    '            .strSeisangoKaiinCardOut = SetNull(.strSeisangoKaiinCardOut, 2)

    '            'プレミアム金額有カード精算設定
    '            .strPremiumCardSeisan = SetNull(.strPremiumCardSeisan, 2)

    '            '当日入金/発券カード精算設定
    '            .strTojituNyukinCardSeisan = SetNull(.strTojituNyukinCardSeisan, 2)

    '            '消費無しカード精算設定
    '            .strShohiNashiCardSeisan = SetNull(.strShohiNashiCardSeisan, 2)

    '            '登録受付禁止時間帯(From)
    '            .strTorokuKinshiFm = SetNull(.strTorokuKinshiFm, 5)

    '            '登録受付禁止時間帯(To)
    '            .strTorokuKinshiTo = SetNull(.strTorokuKinshiTo, 5)

    '            '一般0円カード受付許可/禁止
    '            .strIppanZeroCardKinshi = SetNull(.strIppanZeroCardKinshi, 2)

    '            '一般残高ありカード受付許可/禁止
    '            .strIppanZandakaAriCardKinshi = SetNull(.strIppanZandakaAriCardKinshi, 2)

    '            '会員カード受付許可/禁止
    '            .strKaiinCardKinshi = SetNull(.strKaiinCardKinshi, 2)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTBoxId & _
    '                    .strCampanyKbn & _
    '                    .strCardSbt & _
    '                    .strKenmenKngk & _
    '                    .strPremiumKngk & _
    '                    .strKenbaiFlg & _
    '                    .strDaikanFlg & _
    '                    .strShohiFlg & _
    '                    .strUseTani & _
    '                    .strShohiGendoKngk & _
    '                    .strBaitaiRuisekiGendoKngk & _
    '                    .strCardYukoKikan & _
    '                    .strOfflineUnyoFm & _
    '                    .strOfflineUnyoTo & _
    '                    .strOfflineUnyoKyoka & _
    '                    .strOfflineKenbaiHakkenKngk & _
    '                    .strOfflineKenbaiHakkenMaisu & _
    '                    .strOfflineSandoNyukinKngk & _
    '                    .strOfflineSandoNyukinMaisu & _
    '                    .strOfflineShohiKngk & _
    '                    .strOfflineShohiMaisu & _
    '                    .strKaiinSvcCode1 & _
    '                    .strKaiinSvcCode2 & _
    '                    .strKijunTamasuNow & _
    '                    .strKijunMedarusuNow & _
    '                    .strKijunTamasuFuture & _
    '                    .strKijunMedarusuFuture & _
    '                    .strKijunTamaHaneiYmd & _
    '                    .strShohizeiKbnNow & _
    '                    .strShohizeiUnyoSetteiNow & _
    '                    .strShohizeiRituNow & _
    '                    .strShohizeiKbnFuture & _
    '                    .strShohizeiUnyoSetteiNow & _
    '                    .strShohizeiRituFuture & _
    '                    .strShohizeiHaneiYmd & _
    '                    .strEveryMoney & _
    '                    .strKaiinCardUktk & _
    '                    .strIppanZeroCard & _
    '                    .strKaiinZeroCard & _
    '                    .strTuikaNyukinJoken & _
    '                    .strIppanZeroCardOut & _
    '                    .strTamakashiTimerVal & _
    '                    .strTamakashiShikiiVal & _
    '                    .strCardUnitPtnkTimerVal & _
    '                    .strCardUnitPtnkShikiiVal & _
    '                    .strCardUnitSlotTimerVal & _
    '                    .strCardUnitSlotShikiiVal & _
    '                    .strICMedalDaigotoTimerVal & _
    '                    .strICMedalDaigotoShikiiVal & _
    '                    .strICMedalDaikanTimerVal & _
    '                    .strICMedalDaikanShikiiVal & _
    '                    .strTamakashiTankaKino & _
    '                    .strKaiinCardPassInput & _
    '                    .strIppanZeroCardKaisyu & _
    '                    .strKaiinCardUktkSando & _
    '                    .strDaikanNyukinKino & _
    '                    .strUkeireOut500 & _
    '                    .strUkeireOut1000 & _
    '                    .strUkeireOut2000 & _
    '                    .strUkeireOut5000 & _
    '                    .strUkeireOut10000 & _
    '                    .strIppanZeroCardSando & _
    '                    .strKaiinZeroCardSando & _
    '                    .strTuikaJokenSando & _
    '                    .strAnsyoShogoSbt & _
    '                    .strEveryMoneySeisan & _
    '                    .strSeisanCardSbt & _
    '                    .strSeisangoKaiinCardOut & _
    '                    .strPremiumCardSeisan & _
    '                    .strTojituNyukinCardSeisan & _
    '                    .strShohiNashiCardSeisan & _
    '                    .strTorokuKinshiFm & _
    '                    .strTorokuKinshiTo & _
    '                    .strIppanZeroCardKinshi & _
    '                    .strIppanZandakaAriCardKinshi & _
    '                    .strKaiinCardKinshi & _
    '                    .strUpdateNo
    '        End With

    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' BB制御情報（券売入金機）更新処理上り電文作成
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpBBSeigyoInfoKenbaiNyukin(ByRef objStruct As StrctBBSeigyoKenbaiNyukinki) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            'オフライン運用許可/禁止
    '            .strOfflineUnyoKyoka = SetNull(.strOfflineUnyoKyoka, 2)

    '            'エブリマネー使用許可/停止
    '            .strEveryMoneyShiyoKyoka = SetNull(.strEveryMoneyShiyoKyoka, 2)

    '            '配送カードIDチェック有無
    '            .strHaisoCardIdChk = SetNull(.strHaisoCardIdChk, 2)

    '            'BB1動作制御7
    '            .strBB1Dosa7 = SetNull(GetLoopStr(.strBB1Dosa7, 65), 130)

    '            'BB1動作制御5/4
    '            .strBB1Dosa5_4 = SetNull(GetLoopStr(.strBB1Dosa5_4, 65), 195)

    '            'BB1動作制御3/2
    '            .strBB1Dosa3_2 = SetNull(GetLoopStr(.strBB1Dosa3_2, 65), 195)

    '            'BB1動作制御1
    '            .strBB1Dosa1 = SetNull(GetLoopStr(.strBB1Dosa1, 65), 130)

    '            'ICC動作制御7/6
    '            .strICCDosa7_6 = SetNull(GetLoopStr(.strICCDosa7_6, 65), 195)

    '            'ICC閉塞指示1
    '            .strICCHeisoku1 = SetNull(GetLoopStr(.strICCHeisoku1, 65), 130)

    '            'ICC閉塞指示2
    '            .strICCHeisoku2 = SetNull(GetLoopStr(.strICCHeisoku2, 65), 130)

    '            'ICC閉塞指示3
    '            .strICCHeisoku3 = SetNull(GetLoopStr(.strICCHeisoku3, 65), 130)

    '            'ICC閉塞指示4
    '            .strICCHeisoku4 = SetNull(GetLoopStr(.strICCHeisoku4, 65), 130)

    '            'ICC閉塞指示5
    '            .strICCHeisoku5 = SetNull(GetLoopStr(.strICCHeisoku5, 65), 130)

    '            '表示制御
    '            .strHyojiSeigyo = SetNull(GetLoopStr(.strHyojiSeigyo, 65), 260)

    '            'ランプ
    '            .strLamp = SetNull(GetLoopStr(.strLamp, 65), 195)

    '            'コネクションタイムアウト時間
    '            .strConnectionTimeOut = SetNull(.strConnectionTimeOut, 3)

    '            '開始処理受付タイムアウト
    '            .strStartShoriUktkTimeOut = SetNull(.strStartShoriUktkTimeOut, 3)

    '            '運用開始タイムアウト
    '            .strUnyoStartTimeOut = SetNull(.strUnyoStartTimeOut, 3)

    '            '問合せタイムアウト時間
    '            .strToiawaseTimeOut = SetNull(.strToiawaseTimeOut, 3)

    '            '通信切断時接続ディレイ時間
    '            .strTusinCloseDelayTime = SetNull(.strTusinCloseDelayTime, 3)

    '            'コネクション要求リトライ間隔
    '            .strConnectionReqRetry = SetNull(.strConnectionReqRetry, 3)

    '            'UDPコマンドタイムアウト
    '            .strUDPCommandTimeOut = SetNull(.strUDPCommandTimeOut, 3)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTboxId & _
    '                    .strOfflineUnyoKyoka & _
    '                    .strEveryMoneyShiyoKyoka & _
    '                    .strHaisoCardIdChk & _
    '                    .strBB1Dosa7 & _
    '                    .strBB1Dosa5_4 & _
    '                    .strBB1Dosa3_2 & _
    '                    .strBB1Dosa1 & _
    '                    .strICCDosa7_6 & _
    '                    .strICCHeisoku1 & _
    '                    .strICCHeisoku2 & _
    '                    .strICCHeisoku3 & _
    '                    .strICCHeisoku4 & _
    '                    .strICCHeisoku5 & _
    '                    .strHyojiSeigyo & _
    '                    .strLamp & _
    '                    .strConnectionTimeOut & _
    '                    .strStartShoriUktkTimeOut & _
    '                    .strUnyoStartTimeOut & _
    '                    .strToiawaseTimeOut & _
    '                    .strTusinCloseDelayTime & _
    '                    .strConnectionReqRetry & _
    '                    .strUDPCommandTimeOut & _
    '                    .strUpdateNo
    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try

    'End Function

    ' ''' <summary>
    ' ''' BB制御情報（サンド 精算機）更新処理上り電文作成
    ' ''' </summary>
    ' ''' <param name="objStruct"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunBBSeigyoInfoSandoSeisanki(ByRef objStruct As StrctBBSeigyoInfoSandoSeisanki) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            'ｵﾌﾗｲﾝ運用許可/禁止
    '            .strOfflineUnyo = SetNull(.strOfflineUnyo, 2)

    '            '精算許可/停止
    '            .strSeisanKyoka = SetNull(.strSeisanKyoka, 2)

    '            'ｵﾌﾗｲﾝｻﾝﾄﾞ入金許可/禁止
    '            .strOfflineSandoNyukinKyoka = SetNull(.strOfflineSandoNyukinKyoka, 2)

    '            '休ｶｰﾄﾞ(会員)ｵﾌﾗｲﾝ入金許可/禁止
    '            .strYasumiCardKaiin = SetNull(.strYasumiCardKaiin, 2)

    '            '休ｶｰﾄﾞ(一般)ｵﾌﾗｲﾝ入金許可/禁止
    '            .strYasumiCardIppan = SetNull(.strYasumiCardIppan, 2)

    '            'BB1動作制御7
    '            .strBB1Dosa7 = SetNull(GetLoopStr(.strBB1Dosa7, 70), 140)

    '            'BB1動作制御6
    '            .strBB1Dosa6 = SetNull(GetLoopStr(.strBB1Dosa6, 70), 140)

    '            'BB1動作制御5/4
    '            .strBB1Dosa5_4 = SetNull(GetLoopStr(.strBB1Dosa5_4, 70), 210)

    '            'BB1動作制御3/2
    '            .strBB1Dosa3_2 = SetNull(GetLoopStr(.strBB1Dosa3_2, 70), 210)

    '            'BB1動作制御1
    '            .strBB1Dosa1 = SetNull(GetLoopStr(.strBB1Dosa1, 70), 140)

    '            'ICC動作制御7/6
    '            .strICCDosa7_6 = SetNull(GetLoopStr(.strICCDosa7_6, 70), 210)

    '            'ICC動作制御3
    '            .strICCDosa3 = SetNull(GetLoopStr(.strICCDosa3, 70), 140)

    '            'ICC動作制御2
    '            .strICCDosa2 = SetNull(GetLoopStr(.strICCDosa2, 70), 140)

    '            'ICC動作制御1
    '            .strICCDosa1 = SetNull(GetLoopStr(.strICCDosa1, 70), 140)

    '            'ICC閉塞指示1
    '            .strICCHeisoku1 = SetNull(GetLoopStr(.strICCHeisoku1, 70), 140)

    '            'ICC閉塞指示2
    '            .strICCHeisoku2 = SetNull(GetLoopStr(.strICCHeisoku2, 70), 140)

    '            'ICC閉塞指示3
    '            .strICCHeisoku3 = SetNull(GetLoopStr(.strICCHeisoku3, 70), 140)

    '            'ICC閉塞指示4
    '            .strICCHeisoku4 = SetNull(GetLoopStr(.strICCHeisoku4, 70), 140)

    '            'ICC閉塞指示5
    '            .strICCHeisoku5 = SetNull(GetLoopStr(.strICCHeisoku5, 70), 140)

    '            '表示制御
    '            .strHyojiSeigyo = SetNull(GetLoopStr(.strHyojiSeigyo, 70), 280)

    '            'ﾗﾝﾌﾟ
    '            .strLamp = SetNull(GetLoopStr(.strLamp, 70), 210)

    '            'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間
    '            .strConnectionTimeOut = SetNull(.strConnectionTimeOut, 3)

    '            '開始処理受付ﾀｲﾑｱｳﾄ
    '            .strStartShoriUktkTimeOut = SetNull(.strStartShoriUktkTimeOut, 3)

    '            '運用開始ﾀｲﾑｱｳﾄ
    '            .strUnyoStartTimeOut = SetNull(.strUnyoStartTimeOut, 3)

    '            '問合せﾀｲﾑｱｳﾄ時間
    '            .strToiawaseTimeOut = SetNull(.strToiawaseTimeOut, 3)

    '            '通信切断時接続ﾃﾞｨﾚｲ時間
    '            .strTusinCloseDelayTime = SetNull(.strTusinCloseDelayTime, 3)

    '            'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ間隔
    '            .strConnectionReqRetry = SetNull(.strConnectionReqRetry, 3)

    '            'UDPｺﾏﾝﾄﾞﾀｲﾑｱｳﾄ
    '            .strUDPCommandTimeOut = SetNull(.strUDPCommandTimeOut, 3)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTboxId & _
    '                    .strOfflineUnyo & _
    '                    .strSeisanKyoka & _
    '                    .strOfflineSandoNyukinKyoka & _
    '                    .strYasumiCardKaiin & _
    '                    .strYasumiCardIppan & _
    '                    .strBB1Dosa7 & _
    '                    .strBB1Dosa6 & _
    '                    .strBB1Dosa5_4 & _
    '                    .strBB1Dosa3_2 & _
    '                    .strBB1Dosa1 & _
    '                    .strICCDosa7_6 & _
    '                    .strICCDosa3 & _
    '                    .strICCDosa2 & _
    '                    .strICCDosa1 & _
    '                    .strICCHeisoku1 & _
    '                    .strICCHeisoku2 & _
    '                    .strICCHeisoku3 & _
    '                    .strICCHeisoku4 & _
    '                    .strICCHeisoku5 & _
    '                    .strHyojiSeigyo & _
    '                    .strLamp & _
    '                    .strConnectionTimeOut & _
    '                    .strStartShoriUktkTimeOut & _
    '                    .strUnyoStartTimeOut & _
    '                    .strToiawaseTimeOut & _
    '                    .strTusinCloseDelayTime & _
    '                    .strConnectionReqRetry & _
    '                    .strUDPCommandTimeOut & _
    '                    .strUpdateNo
    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' BB制御情報（登録受付機）更新処理上り電文作成
    ' ''' </summary>
    ' ''' <param name="objStruct"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpBBSeigyoInfoTorokuUktk(ByRef objStruct As StrctBBSeigyoInfoTorokuUktk) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            '登録受付許可/禁止
    '            .strTorokuUktkKyoka = SetNull(.strTorokuUktkKyoka, 2)

    '            'BB1動作制御7
    '            .strBB1Dosa7 = SetNull(GetLoopStr(.strBB1Dosa7, 65), 130)

    '            'BB1動作制御5/4
    '            .strBB1Dosa5_4 = SetNull(GetLoopStr(.strBB1Dosa5_4, 65), 195)

    '            'BB1動作制御3/2
    '            .strBB1Dosa3_2 = SetNull(GetLoopStr(.strBB1Dosa3_2, 65), 195)

    '            'BB1動作制御1
    '            .strBB1Dosa1 = SetNull(GetLoopStr(.strBB1Dosa1, 65), 130)

    '            'ICC制御動作7/6
    '            .strICCDosa7_6 = SetNull(GetLoopStr(.strICCDosa7_6, 65), 195)

    '            'ICC制御閉塞指示1
    '            .strICCHeisokuSzi1 = SetNull(GetLoopStr(.strICCHeisokuSzi1, 65), 130)

    '            'ICC制御閉塞指示2
    '            .strICCHeisokuSzi2 = SetNull(GetLoopStr(.strICCHeisokuSzi2, 65), 130)

    '            'ICC制御閉塞指示3
    '            .strICCHeisokuSzi3 = SetNull(GetLoopStr(.strICCHeisokuSzi3, 65), 130)

    '            'ICC制御閉塞指示4
    '            .strICCHeisokuSzi4 = SetNull(GetLoopStr(.strICCHeisokuSzi4, 65), 130)

    '            'ICC制御閉塞指示5
    '            .strICCHeisokuSzi5 = SetNull(GetLoopStr(.strICCHeisokuSzi5, 65), 130)

    '            '表示制御
    '            .strHyojiSeigyo = SetNull(GetLoopStr(.strHyojiSeigyo, 65), 260)

    '            '制御情報
    '            .strSeigyoInfo = SetNull(GetLoopStr(.strSeigyoInfo, 65), 195)

    '            '画面番号
    '            .strGamenNo = SetNull(GetLoopStr(.strGamenNo, 65), 195)

    '            'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間
    '            .strConnectionTimeOut = SetNull(.strConnectionTimeOut, 3)

    '            '開始処理受付ﾀｲﾑｱｳﾄ
    '            .strStartShoriUktkTimeOut = SetNull(.strStartShoriUktkTimeOut, 3)

    '            '運用開始ﾀｲﾑｱｳﾄ
    '            .strUnyoStartTimeOut = SetNull(.strUnyoStartTimeOut, 3)

    '            '問合せﾀｲﾑｱｳﾄ時間
    '            .strToiawaseTimeOut = SetNull(.strToiawaseTimeOut, 3)

    '            '通信切断時接続ﾃﾞｨﾚｲ時間
    '            .strTusinCloseDelayTime = SetNull(.strTusinCloseDelayTime, 3)

    '            'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ間隔
    '            .strConnectionReqRetry = SetNull(.strConnectionReqRetry, 3)

    '            'UDPｺﾏﾝﾄﾞﾀｲﾑｱｳﾄ
    '            .strUDPCommandTimeOut = SetNull(.strUDPCommandTimeOut, 3)

    '            '登録問合せﾀｲﾑｱｳﾄ
    '            .strTorokuToiawaseTimeOut = SetNull(.strTorokuToiawaseTimeOut, 3)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTboxId & _
    '                    .strTorokuUktkKyoka & _
    '                    .strBB1Dosa7 & _
    '                    .strBB1Dosa5_4 & _
    '                    .strBB1Dosa3_2 & _
    '                    .strBB1Dosa1 & _
    '                    .strICCDosa7_6 & _
    '                    .strICCHeisokuSzi1 & _
    '                    .strICCHeisokuSzi2 & _
    '                    .strICCHeisokuSzi3 & _
    '                    .strICCHeisokuSzi4 & _
    '                    .strICCHeisokuSzi5 & _
    '                    .strHyojiSeigyo & _
    '                    .strSeigyoInfo & _
    '                    .strGamenNo & _
    '                    .strConnectionTimeOut & _
    '                    .strStartShoriUktkTimeOut & _
    '                    .strUnyoStartTimeOut & _
    '                    .strToiawaseTimeOut & _
    '                    .strTusinCloseDelayTime & _
    '                    .strConnectionReqRetry & _
    '                    .strUDPCommandTimeOut & _
    '                    .strTorokuToiawaseTimeOut & _
    '                    .strUpdateNo
    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' BB運用情報（LUT）更新処理上り電文作成
    ' ''' </summary>
    ' ''' <param name="objStruct"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpBBUnyoInfoLUT(ByRef objStruct As StrctBBUnyoInfoLUT) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxID = SetNull(.strTboxID, 9)

    '            '端末ID
    '            .strTanmatuId = SetNull(.strTanmatuId, 17)

    '            'ユーザID
    '            .strUserId = SetNull(.strUserId, 17)

    '            '資格区分
    '            .strShikakuKbn = SetNull(.strShikakuKbn, 2)

    '            '会社区分
    '            .strKaishaKbn = SetNull(GetLoopStr(.strKaishaKbn, 44), 88)

    '            'ｶｰﾄﾞ種別
    '            .strCardSbt = SetNull(GetLoopStr(.strCardSbt, 44), 88)

    '            '券面金額
    '            .strKenmenKngk = SetNull(GetLoopStr(.strKenmenKngk, 44), 264)

    '            '券売ﾌﾗｸﾞ
    '            .strKenbaiFlg = SetNull(GetLoopStr(.strKenbaiFlg, 44), 88)

    '            '台間ﾌﾗｸﾞ
    '            .strDaikanFlg = SetNull(GetLoopStr(.strDaikanFlg, 44), 88)

    '            '消費ﾌﾗｸﾞ
    '            .strShohiFlg = SetNull(GetLoopStr(.strShohiFlg, 44), 88)

    '            '1度数金額設定情報
    '            .str1DosuKngk = SetNull(.str1DosuKngk, 5)

    '            '消費税区分（現在）
    '            .strShohizeiKbnNow = SetNull(.strShohizeiKbnNow, 2)

    '            '消費税運用設定（現在）
    '            .strShohizeiUnyoNow = SetNull(.strShohizeiUnyoNow, 2)

    '            '消費税率（現在）
    '            .strShohizeiRituNow = SetNull(.strShohizeiRituNow, 4)

    '            '消費税区分（未来）
    '            .strShohizeiKbnFuture = SetNull(.strShohizeiKbnFuture, 2)

    '            '消費税運用設定（未来）
    '            .strShohizeiUnyoFuture = SetNull(.strShohizeiUnyoFuture, 2)

    '            '消費税率（未来）
    '            .strShohizeiRituFuture = SetNull(.strShohizeiRituFuture, 4)

    '            '消費税反映年月日
    '            .strShohizeiHaneiYmd = SetNull(.strShohizeiHaneiYmd, 7)

    '            '消費有効期限（会員）数値指定
    '            .strKaiinShohiYukokigenSu = SetNull(.strKaiinShohiYukokigenSu, 4)

    '            '消費有効期限（会員）年月日
    '            .strKaiinShohiYukokigenYmd = SetNull(.strKaiinShohiYukokigenYmd, 3)

    '            '消費有効期限（一般）数値指定
    '            .strIppanShohiYukokigenSu = SetNull(.strIppanShohiYukokigenSu, 4)

    '            '消費有効期限（一般）年月日
    '            .strIppanShohiYukokigenYmd = SetNull(.strIppanShohiYukokigenYmd, 3)

    '            '消費不可金額（一般）
    '            .strShohiFukaKngk = SetNull(.strShohiFukaKngk, 3)

    '            'ｵﾌﾗｲﾝ運用可能時間帯（ｻ）（From）
    '            .strOfflineUnyoSandoFm = SetNull(.strOfflineUnyoSandoFm, 5)

    '            'ｵﾌﾗｲﾝ運用可能時間帯（ｻ）（To）
    '            .strOfflineUnyoSandoTo = SetNull(.strOfflineUnyoSandoTo, 5)

    '            'ｵﾌﾗｲﾝ運用許可時間（ｻ）
    '            .strOfflineUnyoKyokaTimeSando = SetNull(.strOfflineUnyoKyokaTimeSando, 4)

    '            'ｵﾌﾗｲﾝ未送信ﾄﾚｰｽ件数
    '            .strOfflineMisosinTraceCnt = SetNull(.strOfflineMisosinTraceCnt, 4)

    '            'ｵﾌﾗｲﾝ運用可能時間帯（精）（From）
    '            .strOfflineUnyoSeiFm = SetNull(.strOfflineUnyoSeiFm, 5)

    '            'ｵﾌﾗｲﾝ運用可能時間帯（精）（To）
    '            .strOfflineUnyoSeiTo = SetNull(.strOfflineUnyoSeiTo, 5)

    '            'ｵﾌﾗｲﾝ運用許可時間（精）
    '            .strOfflineUnyoKyokaTimeSei = SetNull(.strOfflineUnyoKyokaTimeSei, 4)

    '            '会員ｻｰﾋﾞｽｺｰﾄﾞ1
    '            .strKaiinSvcCode1 = SetNull(.strKaiinSvcCode1, 9)

    '            '会員ｻｰﾋﾞｽｺｰﾄﾞ2
    '            .strKaiinSvcCode2 = SetNull(.strKaiinSvcCode2, 9)

    '            '追加入金条件設定（ｻﾝﾄﾞ）
    '            .strTuikaNyukinjokenSando = SetNull(.strTuikaNyukinjokenSando, 3)

    '            '入金上限額設定
    '            .strNyukinJogen = SetNull(.strNyukinJogen, 2)

    '            '精算可能ｶｰﾄﾞ種別設定
    '            .strSeisanCardSbt = SetNull(.strSeisanCardSbt, 2)

    '            '登録受付禁止時間帯（From）
    '            .strTorokuUktkKinsiFm = SetNull(.strTorokuUktkKinsiFm, 5)

    '            '登録受付禁止時間帯（To）
    '            .strTorokuUktkKinsiTo = SetNull(.strTorokuUktkKinsiTo, 5)

    '            '一般0円ｺｲﾝ受付許可/禁止
    '            .strIppanZeroCoinUktk = SetNull(.strIppanZeroCoinUktk, 2)

    '            '一般残高ありｺｲﾝ受付許可/禁止
    '            .strIppanZandakaariCoinUktk = SetNull(.strIppanZandakaariCoinUktk, 2)

    '            '会員ｶｰﾄﾞ受付許可/禁止
    '            .strKaiinCardUktk = SetNull(.strKaiinCardUktk, 2)

    '            'モード設定
    '            .strMode = SetNull(.strMode, 3)

    '            '累積消費チェック金額設定
    '            .strRuisekiShohiChkKngk = SetNull(.strRuisekiShohiChkKngk, 3)

    '            '台間入金機能許可/抑止
    '            .strDaikanNyukinKino = SetNull(.strDaikanNyukinKino, 3)

    '            'ｴﾌﾞﾘﾏﾈｰ有効期間
    '            .strEveryMoneyYukoKikan = SetNull(.strEveryMoneyYukoKikan, 4)

    '            '受入禁止券種設定100円玉
    '            .strUkeireKinsi100 = SetNull(.strUkeireKinsi100, 2)

    '            '受入禁止券種設定500円玉
    '            .strUkeireKinsi500 = SetNull(.strUkeireKinsi500, 2)

    '            '受入禁止券種設定1000円札
    '            .strUkeireKinsi1000 = SetNull(.strUkeireKinsi1000, 2)

    '            '受入禁止券種設定2000円札
    '            .strUkeireKinsi2000 = SetNull(.strUkeireKinsi2000, 2)

    '            '受入禁止券種設定5000円札
    '            .strUkeireKinsi5000 = SetNull(.strUkeireKinsi5000, 2)

    '            '受入禁止券種設定10000円札
    '            .strUkeireKinsi10000 = SetNull(.strUkeireKinsi10000, 2)

    '            '一般ｺｲﾝ0円時排出方向（ｻﾝﾄﾞ）
    '            .strIppanZeroCoinOutSando = SetNull(.strIppanZeroCoinOutSando, 3)

    '            '一般ｺｲﾝ0円時排出方向（精）
    '            .strIppanZeroCoinOutSei = SetNull(.strIppanZeroCoinOutSei, 3)

    '            '制御95情報1
    '            .strSeigyo95_1 = SetNull(.strSeigyo95_1, 37)

    '            '制御95情報2
    '            .strSeigyo95_2 = SetNull(.strSeigyo95_2, 37)

    '            '制御95情報3
    '            .strSeigyo95_3 = SetNull(.strSeigyo95_3, 37)

    '            '制御95情報4
    '            .strSeigyo95_4 = SetNull(.strSeigyo95_4, 37)

    '            '制御95情報5
    '            .strSeigyo95_5 = SetNull(.strSeigyo95_5, 37)

    '            '制御95情報6
    '            .strSeigyo95_6 = SetNull(.strSeigyo95_6, 37)

    '            '制御95情報7
    '            .strSeigyo95_7 = SetNull(.strSeigyo95_7, 37)

    '            '制御95情報8
    '            .strSeigyo95_8 = SetNull(.strSeigyo95_8, 37)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTboxID & _
    '                    .strTanmatuId & _
    '                    .strUserId & _
    '                    .strShikakuKbn & _
    '                    .strKaishaKbn & _
    '                    .strCardSbt & _
    '                    .strKenmenKngk & _
    '                    .strKenbaiFlg & _
    '                    .strDaikanFlg & _
    '                    .strShohiFlg & _
    '                    .str1DosuKngk & _
    '                    .strShohizeiKbnNow & _
    '                    .strShohizeiUnyoNow & _
    '                    .strShohizeiRituNow & _
    '                    .strShohizeiKbnFuture & _
    '                    .strShohizeiUnyoFuture & _
    '                    .strShohizeiRituFuture & _
    '                    .strShohizeiHaneiYmd & _
    '                    .strKaiinShohiYukokigenSu & _
    '                    .strKaiinShohiYukokigenYmd & _
    '                    .strIppanShohiYukokigenSu & _
    '                    .strIppanShohiYukokigenYmd & _
    '                    .strShohiFukaKngk & _
    '                    .strOfflineUnyoSandoFm & _
    '                    .strOfflineUnyoSandoTo & _
    '                    .strOfflineUnyoKyokaTimeSando & _
    '                    .strOfflineMisosinTraceCnt & _
    '                    .strOfflineUnyoSeiFm & _
    '                    .strOfflineUnyoSeiTo & _
    '                    .strOfflineUnyoKyokaTimeSei & _
    '                    .strKaiinSvcCode1 & _
    '                    .strKaiinSvcCode2 & _
    '                    .strTuikaNyukinjokenSando & _
    '                    .strNyukinJogen & _
    '                    .strSeisanCardSbt & _
    '                    .strTorokuUktkKinsiFm & _
    '                    .strTorokuUktkKinsiTo & _
    '                    .strIppanZeroCoinUktk & _
    '                    .strIppanZandakaariCoinUktk & _
    '                    .strKaiinCardUktk & _
    '                    .strMode & _
    '                    .strRuisekiShohiChkKngk & _
    '                    .strDaikanNyukinKino & _
    '                    .strEveryMoneyYukoKikan & _
    '                    .strUkeireKinsi100 & _
    '                    .strUkeireKinsi500 & _
    '                    .strUkeireKinsi1000 & _
    '                    .strUkeireKinsi2000 & _
    '                    .strUkeireKinsi5000 & _
    '                    .strUkeireKinsi10000 & _
    '                    .strIppanZeroCoinOutSando & _
    '                    .strIppanZeroCoinOutSei & _
    '                    .strSeigyo95_1 & _
    '                    .strSeigyo95_2 & _
    '                    .strSeigyo95_3 & _
    '                    .strSeigyo95_4 & _
    '                    .strSeigyo95_5 & _
    '                    .strSeigyo95_6 & _
    '                    .strSeigyo95_7 & _
    '                    .strSeigyo95_8 & _
    '                    .strUpdateNo

    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' BB制御情報（サンド）（LUT）更新処理上り電文作成
    ' ''' </summary>
    ' ''' <param name="objStruct"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpBBSeigyoInfoSandoLUT(ByRef objStruct As StrctBBSeigyoInfoSandLUT) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX－ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            'ｵﾌﾗｲﾝ運用許可/禁止
    '            .strOfflineUnyoKyoka = SetNull(.strOfflineUnyoKyoka, 2)

    '            'ｴﾌﾞﾘﾏﾈｰ使用許可/停止
    '            .strEveryMoneyShiyoKyoka = SetNull(.strEveryMoneyShiyoKyoka, 2)

    '            'ｵﾌﾗｲﾝｴﾌﾞﾘﾏﾈｰ使用許可/停止
    '            .strOfflineEveryMoneyShiyoKyoka = SetNull(.strOfflineEveryMoneyShiyoKyoka, 2)

    '            '登録受付許可/禁止
    '            .strTorokuUktkKyoka = SetNull(.strTorokuUktkKyoka, 2)

    '            'ｶｰﾄﾞ残高ｵﾌﾗｲﾝ消費許可/禁止
    '            .strCardZanOfflineShohiKyoka = SetNull(.strCardZanOfflineShohiKyoka, 2)

    '            '休ｺｲﾝ（一般）ｵﾌﾗｲﾝ入金許可/禁止
    '            .strYasumiCoinIppanNyukinKyoka = SetNull(.strYasumiCoinIppanNyukinKyoka, 2)

    '            '休ｶｰﾄﾞ（会員）ｵﾌﾗｲﾝ入金許可/禁止
    '            .strYasumiCoinKaiinNyukinKyoka = SetNull(.strYasumiCoinKaiinNyukinKyoka, 2)

    '            'BB1動作制御7
    '            .strBB1Dosa7 = SetNull(GetLoopStr(.strBB1Dosa7, 63), 126)

    '            'BB1動作制御6
    '            .strBB1Dosa6 = SetNull(GetLoopStr(.strBB1Dosa6, 63), 126)

    '            'BB1動作制御5/4
    '            .strBB1Dosa5_4 = SetNull(GetLoopStr(.strBB1Dosa5_4, 63), 189)

    '            'BB1動作制御3/2
    '            .strBB1Dosa3_2 = SetNull(GetLoopStr(.strBB1Dosa3_2, 63), 189)

    '            'BB1動作制御1
    '            .strBB1Dosa1 = SetNull(GetLoopStr(.strBB1Dosa1, 63), 126)

    '            'ICC動作制御7/6
    '            .strICCDosa7_6 = SetNull(GetLoopStr(.strICCDosa7_6, 63), 189)

    '            'ICC動作制御3
    '            .strICCDosa3 = SetNull(GetLoopStr(.strICCDosa3, 63), 126)

    '            'ICC動作制御2
    '            .strICCDosa2 = SetNull(GetLoopStr(.strICCDosa2, 63), 126)

    '            'ICC動作制御1
    '            .strICCDosa1 = SetNull(GetLoopStr(.strICCDosa1, 63), 126)

    '            'ICC閉塞指示1
    '            .strICCHeisoku1 = SetNull(GetLoopStr(.strICCHeisoku1, 63), 126)

    '            'ICC閉塞指示2
    '            .strICCHeisoku2 = SetNull(GetLoopStr(.strICCHeisoku2, 63), 126)

    '            'ICC閉塞指示3
    '            .strICCHeisoku3 = SetNull(GetLoopStr(.strICCHeisoku3, 63), 126)

    '            'ICC閉塞指示4
    '            .strICCHeisoku4 = SetNull(GetLoopStr(.strICCHeisoku4, 63), 126)

    '            'ICC閉塞指示5
    '            .strICCHeisoku5 = SetNull(GetLoopStr(.strICCHeisoku5, 63), 126)

    '            '表示制御
    '            .strHyojiSeigyo = SetNull(GetLoopStr(.strHyojiSeigyo, 63), 252)

    '            'ﾗﾝﾌﾟ
    '            .strLamp = SetNull(GetLoopStr(.strLamp, 63), 189)

    '            'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間
    '            .strConnectionTimeOut = SetNull(.strConnectionTimeOut, 3)

    '            '開始処理受付ﾀｲﾑｱｳﾄ
    '            .strStartShoriUktkTimeOut = SetNull(.strStartShoriUktkTimeOut, 3)

    '            '運用開始ﾀｲﾑｱｳﾄ
    '            .strUnyoStartTimeOut = SetNull(.strUnyoStartTimeOut, 3)

    '            '問合せﾀｲﾑｱｳﾄ時間
    '            .strToiawaseTimeOut = SetNull(.strToiawaseTimeOut, 3)

    '            '通信切断時接続ﾃﾞｨﾚｲ時間
    '            .strTusinCloseDelayTime = SetNull(.strTusinCloseDelayTime, 3)

    '            'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ間隔
    '            .strConnectionReqRetry = SetNull(.strConnectionReqRetry, 3)

    '            'KeepAliveﾀｲﾑｱｳﾄ
    '            .strKeepAliveTimeOut = SetNull(.strKeepAliveTimeOut, 3)

    '            '登録問い合わせﾀｲﾑｱｳﾄ
    '            .strTorokuToiawaseTimeOut = SetNull(.strTorokuToiawaseTimeOut, 3)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTboxId & _
    '                    .strOfflineUnyoKyoka & _
    '                    .strEveryMoneyShiyoKyoka & _
    '                    .strOfflineEveryMoneyShiyoKyoka & _
    '                    .strTorokuUktkKyoka & _
    '                    .strCardZanOfflineShohiKyoka & _
    '                    .strYasumiCoinIppanNyukinKyoka & _
    '                    .strYasumiCoinKaiinNyukinKyoka & _
    '                    .strBB1Dosa7 & _
    '                    .strBB1Dosa6 & _
    '                    .strBB1Dosa5_4 & _
    '                    .strBB1Dosa3_2 & _
    '                    .strBB1Dosa1 & _
    '                    .strICCDosa7_6 & _
    '                    .strICCDosa3 & _
    '                    .strICCDosa2 & _
    '                    .strICCDosa1 & _
    '                    .strICCHeisoku1 & _
    '                    .strICCHeisoku2 & _
    '                    .strICCHeisoku3 & _
    '                    .strICCHeisoku4 & _
    '                    .strICCHeisoku5 & _
    '                    .strHyojiSeigyo & _
    '                    .strLamp & _
    '                    .strConnectionTimeOut & _
    '                    .strStartShoriUktkTimeOut & _
    '                    .strUnyoStartTimeOut & _
    '                    .strToiawaseTimeOut & _
    '                    .strTusinCloseDelayTime & _
    '                    .strConnectionReqRetry & _
    '                    .strKeepAliveTimeOut & _
    '                    .strTorokuToiawaseTimeOut & _
    '                    .strUpdateNo
    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' BB制御情報（精算機）（LUT）更新処理上り電文作成
    ' ''' </summary>
    ' ''' <param name="objStruct"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpBBSeigyoInfoSeisankiLUT(ByRef objStruct As StrctBBSeigyoInfoSeisankiLUT) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            '処理選択
    '            .strShoriSel = SetNull(.strShoriSel, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            '精算許可/停止
    '            .strSeisanKyoka = SetNull(.strSeisanKyoka, 2)

    '            '登録受付許可/禁止
    '            .strTorokuUktkKyoka = SetNull(.strTorokuUktkKyoka, 2)

    '            '数値指定
    '            .strNumShitei = SetNull(.strNumShitei, 4)

    '            '年月日指定
    '            .strYmdShitei = SetNull(.strYmdShitei, 3)

    '            'ｵﾌﾗｲﾝ精算許可/禁止
    '            .strOfflineSeisanKyoka = SetNull(.strOfflineSeisanKyoka, 2)

    '            '共通化ｶｰﾄﾞ当店入金なし精算許可/禁止
    '            .strKyotukaCardTotenNyukinNasiKyoka = SetNull(.strKyotukaCardTotenNyukinNasiKyoka, 2)

    '            'BB1動作制御7
    '            .strBB1Dosa7 = SetNull(GetLoopStr(.strBB1Dosa7, 63), 126)

    '            'BB1動作制御6
    '            .strBB1Dosa6 = SetNull(GetLoopStr(.strBB1Dosa6, 63), 126)

    '            'BB1動作制御5/4
    '            .strBB1Dosa5_4 = SetNull(GetLoopStr(.strBB1Dosa5_4, 63), 189)

    '            'BB1動作制御3/2
    '            .strBB1Dosa3_2 = SetNull(GetLoopStr(.strBB1Dosa3_2, 63), 189)

    '            'BB1動作制御1
    '            .strBB1Dosa1 = SetNull(GetLoopStr(.strBB1Dosa1, 63), 126)

    '            'ICC動作制御7/6
    '            .strICCDosa7_6 = SetNull(GetLoopStr(.strICCDosa7_6, 63), 189)

    '            'ICC動作制御3
    '            .strICCDosa3 = SetNull(GetLoopStr(.strICCDosa3, 63), 126)

    '            'ICC動作制御2
    '            .strICCDosa2 = SetNull(GetLoopStr(.strICCDosa2, 63), 126)

    '            'ICC動作制御1
    '            .strICCDosa1 = SetNull(GetLoopStr(.strICCDosa1, 63), 126)

    '            'ICC閉塞指示1
    '            .strICCHeisoku1 = SetNull(GetLoopStr(.strICCHeisoku1, 63), 126)

    '            'ICC閉塞指示2
    '            .strICCHeisoku2 = SetNull(GetLoopStr(.strICCHeisoku2, 63), 126)

    '            'ICC閉塞指示3
    '            .strICCHeisoku3 = SetNull(GetLoopStr(.strICCHeisoku3, 63), 126)

    '            'ICC閉塞指示4
    '            .strICCHeisoku4 = SetNull(GetLoopStr(.strICCHeisoku4, 63), 126)

    '            'ICC閉塞指示5
    '            .strICCHeisoku5 = SetNull(GetLoopStr(.strICCHeisoku5, 63), 126)

    '            '表示制御
    '            .strHyojiSeigyo = SetNull(GetLoopStr(.strHyojiSeigyo, 63), 252)

    '            'ﾗﾝﾌﾟ
    '            .strLamp = SetNull(GetLoopStr(.strLamp, 63), 189)

    '            'ｺﾈｸｼｮﾝﾀｲﾑｱｳﾄ時間
    '            .strConnectionTimeOut = SetNull(.strConnectionTimeOut, 3)

    '            '開始処理受付ﾀｲﾑｱｳﾄ
    '            .strStartShoriUktkTimeOut = SetNull(.strStartShoriUktkTimeOut, 3)

    '            '運用開始ﾀｲﾑｱｳﾄ
    '            .strUnyoStartTimeOut = SetNull(.strUnyoStartTimeOut, 3)

    '            '問合せﾀｲﾑｱｳﾄ時間
    '            .strToiawaseTimeOut = SetNull(.strToiawaseTimeOut, 3)

    '            '通信切断時接続ﾃﾞｨﾚｲ時間
    '            .strTusinCloseDelayTime = SetNull(.strTusinCloseDelayTime, 3)

    '            'ｺﾈｸｼｮﾝ要求ﾘﾄﾗｲ時間
    '            .strConnectionReqRetry = SetNull(.strConnectionReqRetry, 3)

    '            'KeepAliveﾀｲﾑｱｳﾄ
    '            .strKeepAliveTimeOut = SetNull(.strKeepAliveTimeOut, 3)

    '            '登録問い合わせﾀｲﾑｱｳﾄ
    '            .strTorokuToiawaseTimeOut = SetNull(.strTorokuToiawaseTimeOut, 3)

    '            '更新通番
    '            .strUpdateNo = SetNull(.strUpdateNo, 6)

    '            Return .strShoriSelKbn & _
    '                    .strShoriSel & _
    '                    .strTboxId & _
    '                    .strSeisanKyoka & _
    '                    .strTorokuUktkKyoka & _
    '                    .strNumShitei & _
    '                    .strYmdShitei & _
    '                    .strOfflineSeisanKyoka & _
    '                    .strKyotukaCardTotenNyukinNasiKyoka & _
    '                    .strBB1Dosa7 & _
    '                    .strBB1Dosa6 & _
    '                    .strBB1Dosa5_4 & _
    '                    .strBB1Dosa3_2 & _
    '                    .strBB1Dosa1 & _
    '                    .strICCDosa7_6 & _
    '                    .strICCDosa3 & _
    '                    .strICCDosa2 & _
    '                    .strICCDosa1 & _
    '                    .strICCHeisoku1 & _
    '                    .strICCHeisoku2 & _
    '                    .strICCHeisoku3 & _
    '                    .strICCHeisoku4 & _
    '                    .strICCHeisoku5 & _
    '                    .strHyojiSeigyo & _
    '                    .strLamp & _
    '                    .strConnectionTimeOut & _
    '                    .strStartShoriUktkTimeOut & _
    '                    .strUnyoStartTimeOut & _
    '                    .strToiawaseTimeOut & _
    '                    .strTusinCloseDelayTime & _
    '                    .strConnectionReqRetry & _
    '                    .strKeepAliveTimeOut & _
    '                    .strTorokuToiawaseTimeOut & _
    '                    .strUpdateNo
    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' 即時集信依頼上り電文作成
    ' ''' </summary>
    ' ''' <param name="objStruct"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function mfCreateDenbunUpSokujiSyusin(ByRef objStruct As StrctSokujiSyushin) As String
    '    Try
    '        With objStruct
    '            '処理選択区分
    '            .strShoriSelKbn = SetNull(.strShoriSelKbn, 2)

    '            'TBOX-ID
    '            .strTboxId = SetNull(.strTboxId, 9)

    '            Return .strShoriSelKbn & _
    '                    .strTboxId
    '        End With
    '    Finally
    '        objStruct = Nothing
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' Null埋め
    ' ''' </summary>
    ' ''' <param name="strVal"></param>
    ' ''' <param name="intByte"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function SetNull(ByVal strVal As String, ByVal intByte As Integer) As String
    '    Return strVal.PadRight(intByte, Microsoft.VisualBasic.ControlChars.NullChar)
    'End Function

    ' ''' <summary>
    ' ''' 「繰返」する文字の取得
    ' ''' </summary>
    ' ''' <param name="strVal"></param>
    ' ''' <param name="intCnt"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetLoopStr(ByVal strVal As String, ByVal intCnt As Integer) As String
    '    Dim strRetVal As String = ""
    '    For intIdx As Integer = 0 To intCnt - 1
    '        strRetVal = strRetVal & strVal
    '    Next
    '    Return strRetVal
    'End Function

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="strErrBunrui"></param>
    ''' <param name="strMesod"></param>
    ''' <param name="strNaiyo"></param>
    ''' <remarks></remarks>
    Private Sub msDBErrLog(ByVal strPath As String, ByVal strErrBunrui As String, ByVal strMesod As String, _
                           ByVal strTable As String, ByVal strData As String, Optional ByVal strNaiyo As String = Nothing)
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strErrLog As String = ""

        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記
        strErrLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strErrBunrui & " " & strMesod & " " & strTable & " " & strData & " " & strNaiyo

        writer = New System.IO.StreamWriter(strPath & "\" & strLOGFILE & dteSysDate.ToString("yyyyMMdd") & strERR & strEXT_LOG, _
                                            True, System.Text.Encoding.Default)
        writer.WriteLine(strErrLog)        'エラーログ出力

        writer.Close()

    End Sub

    ''' <summary>
    ''' ログ出力
    ''' </summary>
    ''' <param name="strProcCd"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="bytArray"></param>
    ''' <remarks></remarks>
    Private Sub msSeijyouLog(ByVal strProcCd As String, ByVal strFilePath As String, ByVal bytArray As Byte(), ByVal strSouJyushinFlg As String)
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strLog As String = ""
        Dim strDenbunSyoriCode As String = ""
        Dim strDenbunName As String = ""
        Dim strDenbunNaiyou As String = ""

        Select Case strProcCd
            Case "401"
                strDenbunSyoriCode = "401：構成配信"
                strDenbunName = "店内構成表配信依頼処理電文"
                If strSouJyushinFlg = "0" Then
                    strDenbunNaiyou = System.Text.Encoding.GetEncoding(intENCODE).GetString(bytArray)
                Else
                    strDenbunNaiyou = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(bytArray)
                End If
            Case "402"
                strDenbunSyoriCode = "402：構成配信"
                strDenbunName = "店内装置構成表反映指示処理電文"
                If strSouJyushinFlg = "0" Then
                    strDenbunNaiyou = System.Text.Encoding.GetEncoding(intENCODE).GetString(bytArray)
                Else
                    strDenbunNaiyou = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(bytArray)
                End If
            Case "901"
                strDenbunSyoriCode = "901：即時集信"
                strDenbunName = "即時集信依頼電文"
                If strSouJyushinFlg = "0" Then
                    strDenbunNaiyou = System.Text.Encoding.GetEncoding(intENCODE).GetString(bytArray)
                Else
                    strDenbunNaiyou = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(bytArray)
                End If
        End Select
        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記
        strLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strDenbunSyoriCode & " " & strDenbunName & " " & strDenbunNaiyou

        writer = New System.IO.StreamWriter(strFilePath & "\" & strLOGFILE & dteSysDate.ToString("yyyyMMdd") & strEXT_LOG, _
                                            True, System.Text.Encoding.Default)
        writer.WriteLine(strLog)        'ログ出力

        writer.Close()

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
