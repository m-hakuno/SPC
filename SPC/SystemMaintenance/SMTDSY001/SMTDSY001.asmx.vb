'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事データ集信
'*　ＰＧＭＩＤ：　SMTDSY001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.09　：　浜本
'********************************************************************************************************************************

#Region "インポート定義"
Imports System.IO
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
#End Region


' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://localhost:49741/SystemMaintenance/SMTDSY001/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SMTDSY001
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
    ''' <summary>
    ''' 工事設計依頼ファイル名の頭
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strHD_KOJI_SEKKEI_IRAI As String = "FT_SpecOrder_"

    ''' <summary>
    ''' 工事設計依頼明細ファイル名の頭
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strHD_KOJI_SEKKEI_IRAI_MSI As String = "FT_SpecDetail_"

    ''' <summary>
    ''' INS連絡ファイル名の頭
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strHD_INS_RENRAKU As String = "FT_INSNotics_"

    ''' <summary>
    ''' OPEN連絡ファイル名の頭
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strHD_OPEN_RENRAKU As String = "FT_OpenNotice_"

    ''' <summary>
    ''' ヴァージョンアップ連絡ファイル名の頭
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strHD_VERUP_RENRAKU As String = "FT_VerUpNotice_"

    ''' <summary>
    ''' ファイル拡張子(CSV)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strEXT_CSV As String = ".csv"

    ''' <summary>
    ''' ファイル拡張子(LOG)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strEXT_LOG As String = ".log"

    ''' <summary>
    ''' ファイル提供場所ファイル種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strFILESYUBETU_TEIKYO As String = "960"

    ''' <summary>
    ''' ファイル格納場所ファイル種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strFILESYUBETU_KAKUNO As String = "961"

    ''' <summary>
    ''' ファイルバックアップ先ファイル種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strFILESYUBETU_BACKUP As String = "962"

    ''' <summary>
    ''' INS連絡フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strINS_RENRAKU As String = "01_INS_RENRAKU"

    ''' <summary>
    ''' 工事設計依頼書フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strKOJI_SEKKEI As String = "02_KOJI_SEKKEI"

    ''' <summary>
    ''' 工事設計依頼書明細フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strKOJI_SEKKEI_MEISAI As String = "03_KOJI_SEKKEI_MEISAI"

    ''' <summary>
    ''' オープン日連絡フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strOPEN_RENRAKU As String = "05_OPEN_RENRAKU"

    ''' <summary>
    ''' TBOXVerUp単独工事フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strVERUP_RENRAKU As String = "06_VERUP_RENRAKU"

    ''' <summary>
    ''' ＤＢ接続エラー格納ファイル（フルパス）（通信サーバ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strDB_ERR_LOG As String = "C:\TOMAS\RECEIPT\ERROR\DataBaseConnection"

#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
    ''' <summary>
    ''' 工事設計依頼データインデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum EKojiDataIndex
        工事依頼番号 = 0
        送信日付
        送信時間
        仕様連絡区分
        工事通知番号
        ＴＢＯＸ＿ＩＤ
        ホール名
        ホール責任者名
        ホール所在地
        ホールＴＥＬ
        ホールコード
        ＴＢＯＸ回線番号
        現行システム
        現行システムバージョン
        工事種別＿新規
        工事種別＿増設
        工事種別＿再設置
        工事種別＿店内移設
        工事種別＿一部撤去
        工事種別＿全撤去
        工事種別＿一時撤去
        工事種別＿構成変更
        工事種別＿その他
        工事種別＿その他工事内容
        双子店区分
        単独工事区分
        同時工事店舗数
        親子区分
        親ホールコード
        Ｆ１工事有無
        Ｆ２工事有無
        Ｆ３工事有無
        Ｆ４工事有無
        ストッカホール内
        ストッカホール外
        工事開始日付
        工事開始時間
        オープン日付
        オープン時間
        警察検査日付
        警察検査時間
        総合テスト日付
        総合テスト時間
        最終営業日
        代理店コード
        代理店名
        代理店所在地
        代理店ＴＥＬ
        代理店責任者
        代理店担当者
        代行店コード
        代行店名
        代行店所在地
        代行店ＴＥＬ
        代行店担当者
        ＬＡＮ送付先
        ＬＡＮ送付先住所
        ＬＡＮ送付先責任者
        ＬＡＮ送付先ＴＥＬ
        ＬＡＮ送付先納入希望日
        ＬＡＮ送付先午前午後区分
        ＬＡＮ送付先納入希望時間
        備考
        仕様備考
        担当営業部
        登録社員コード
        登録社員名
        移行工事区分
        移行工事作業区分
        最終営業日＿Ｔ５００
        仮設置工事区分
        仮設置工事日未入力区分
        仮設置工事日付
        仮設置工事時間
        ＴＢＯＸ持帰区分
        システム分類
        ＮＪ区分
        ＶＥＲＵＰ日付
        ＶＥＲＵＰ時間
        ＶＥＲＵＰ工事種類１
        ＶＥＲＵＰ工事種類２
        ＶＥＲＵＰ日付区分
        ホール担当者名
        ＮＴＴＤ稼動有無
        ＬＡＮ工事日付
        ＬＡＮ工事時間
        ＬＡＮ工事＿新規
        ＬＡＮ工事＿増設
        ＬＡＮ工事＿一部撤去
        ＬＡＮ工事＿店内移設
        ＬＡＮ工事＿全撤去
        ＬＡＮ工事＿一時撤去
        ＬＡＮ工事＿再設置
        ＬＡＮ工事＿構成変更
        ＬＡＮ工事＿その他
        ｼｽﾃﾑ区分
        Ｅマネー導入ホールフラグ
        Ｅマネー導入工事フラグ
        Ｅマネーテスト日付
        Ｅマネーテスト時間
    End Enum

    ''' <summary>
    ''' 工事設計依頼明細インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum EKojiMsiDataIndex
        工事依頼番号 = 0
        送信日付
        送信時間
        代理店コード
        工事通知番号
        周波数
        機器名
        工事前稼働数
        工事前予備数
        工事前総台数
        店内移設台数
        撤去対象台数
        新品取付台数
        自店在庫取付台数
        他店在庫取付台数
        譲渡品取付台数
        代理店在庫取付台数
        工事後稼働数
        工事後予備数
        工事後総台数
        登録社員コード
        登録社員名
    End Enum

    ''' <summary>
    ''' Ins連絡書インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum EKojiInsDataIndex
        '        工事依頼番号 = 0
        工事依頼番号
        送信日付
        送信時間
        ＩＮＳ連絡区分
        工事通知番号
        ホール名
        ホールコード
        ＮＪ区分
        ＴＢＯＸＩＤ
        ＤＤＸＰ回線番号
        ＬＣＧＮ
        ＬＣＮ
        ＩＮＳ申込要否
        ＩＮＳ申込日
        ＩＮＳ開通日付
        ＩＮＳ開通時間
        ＩＮＳ回線番号
        ＩＮＳ回線現状
        ＮＴＴ申込支店名
        ＮＴＴ申込支店ＴＥＬ
        ＮＴＴ申込支店担当者名
        特記事項
        登録社員コード
        登録社員名
        システム分類
        ＩＮＳ回線名義
        センタ登録日
        センタ登録備考
        回線管理番号
        ｼｽﾃﾑ区分
    End Enum

    ''' <summary>
    ''' オープン日連絡インデックス
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum EKojiOpenDataIndex
        工事依頼番号 = 0
        送信日付
        送信時間
        仕様連絡区分
        工事通知番号
        ＴＢＯＸＩＤ
        ホール名
        ホール責任者名
        ホール所在地
        ホールＴＥＬ
        ホールコード
        ＴＢＯＸ回線番号
        現行システム
        現行システムバージョン
        工事種別＿新規
        工事種別＿増設
        工事種別＿再設置
        工事種別＿店内移設
        工事種別＿一部撤去
        工事種別＿全撤去
        工事種別＿一時撤去
        工事種別＿構成変更
        工事種別＿その他
        工事種別＿その他工事内容
        双子店区分
        単独工事区分
        同時工事店舗数
        親子区分
        親ホールコード
        Ｆ１工事有無
        Ｆ２工事有無
        Ｆ３工事有無
        Ｆ４工事有無
        ストッカホール内
        ストッカホール外
        工事開始日付
        工事開始時間
        オープン日付
        オープン時間
        警察検査日付
        警察検査時間
        総合テスト日付
        総合テスト時間
        最終営業日
        代理店コード
        代理店名
        代理店所在地
        代理店ＴＥＬ
        代理店責任者
        代理店担当者
        代行店コード
        代行店名
        代行店所在地
        代行店ＴＥＬ
        代行店担当者
        ＬＡＮ送付先
        ＬＡＮ送付先住所
        ＬＡＮ送付先責任者
        ＬＡＮ送付先ＴＥＬ
        ＬＡＮ送付先納入希望日
        ＬＡＮ送付先午前午後区分
        ＬＡＮ送付先納入希望時間
        備考
        仕様備考
        担当営業部
        登録社員コード
        登録社員名
        移行工事区分
        移行工事作業区分
        最終営業日＿Ｔ５００
        仮設置工事区分
        仮設置工事日未入力区分
        仮設置工事日付
        仮設置工事時間
        ＴＢＯＸ持帰区分
        システム分類
        ＮＪ区分
        ＶＥＲＵＰ日付
        ＶＥＲＵＰ時間
        ＶＥＲＵＰ工事種類１
        ＶＥＲＵＰ工事種類２
        ＶＥＲＵＰ日付区分
        ホール担当者名
        ＮＴＴＤ稼動有無
        ＬＡＮ工事日付
        ＬＡＮ工事時間
        ＬＡＮ工事＿新規
        ＬＡＮ工事＿増設
        ＬＡＮ工事＿一部撤去
        ＬＡＮ工事＿店内移設
        ＬＡＮ工事＿全撤去
        ＬＡＮ工事＿一時撤去
        ＬＡＮ工事＿再設置
        ＬＡＮ工事＿構成変更
        ＬＡＮ工事＿その他
        ｼｽﾃﾑ区分
    End Enum

    ''' <summary>
    ''' TBOXVerUp単独工事
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum EKojiVerUp
        工事依頼番号 = 0
        送信日付
        送信時間
        仕様連絡区分
        工事通知番号
        ＴＢＯＸＩＤ
        ホール名
        ホール責任者名
        ホール所在地
        ホールＴＥＬ
        ホールコード
        ＴＢＯＸ回線番号
        現行システム
        現行システムバージョン
        工事種別＿新規
        工事種別＿増設
        工事種別＿再設置
        工事種別＿店内移設
        工事種別＿一部撤去
        工事種別＿全撤去
        工事種別＿一時撤去
        工事種別＿構成変更
        工事種別＿その他
        工事種別＿その他工事内容
        双子店区分
        単独工事区分
        同時工事店舗数
        親子区分
        親ホールコード
        Ｆ１工事有無
        Ｆ２工事有無
        Ｆ３工事有無
        Ｆ４工事有無
        ストッカホール内
        ストッカホール外
        工事開始日付
        工事開始時間
        オープン日付
        オープン時間
        警察検査日付
        警察検査時間
        総合テスト日付
        総合テスト時間
        最終営業日
        代理店コード
        代理店名
        代理店所在地
        代理店ＴＥＬ
        代理店責任者
        代理店担当者
        代行店コード
        代行店名
        代行店所在地
        代行店ＴＥＬ
        代行店担当者
        ＬＡＮ送付先
        ＬＡＮ送付先住所
        ＬＡＮ送付先責任者
        ＬＡＮ送付先ＴＥＬ
        ＬＡＮ送付先納入希望日
        ＬＡＮ送付先午前午後区分
        ＬＡＮ送付先納入希望時間
        備考
        仕様備考
        担当営業部
        登録社員コード
        登録社員名
        移行工事区分
        移行工事作業区分
        最終営業日＿Ｔ５００
        仮設置工事区分
        仮設置工事日未入力区分
        仮設置工事日付
        仮設置工事時間
        ＴＢＯＸ持帰区分
        システム分類
        ＮＪ区分
        ＶＥＲＵＰ日付
        ＶＥＲＵＰ時間
        ＶＥＲＵＰ工事種類１
        ＶＥＲＵＰ工事種類２
        ＶＥＲＵＰ日付区分
        ホール担当者名
        ＮＴＴＤ稼動有無
        ＬＡＮ工事日付
        ＬＡＮ工事時間
        ＬＡＮ工事＿新規
        ＬＡＮ工事＿増設
        ＬＡＮ工事＿一部撤去
        ＬＡＮ工事＿店内移設
        ＬＡＮ工事＿全撤去
        ＬＡＮ工事＿一時撤去
        ＬＡＮ工事＿再設置
        ＬＡＮ工事＿構成変更
        ＬＡＮ工事＿その他
        ｼｽﾃﾑ区分
        Ｅマネー導入ホールフラグ
        Ｅマネー導入工事フラグ
        Ｅマネーテスト日付
        Ｅマネーテスト時間
    End Enum
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim strErrBunrui As String = ""
    Dim strTableName As String = ""
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
    ''' テーブル登録処理
    ''' </summary>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function pfKojiDataShushinDataWrite() As Boolean
        ' ＤＢ接続変数
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As New SqlCommand
        Dim strGetFolderPath As String = ""
        Dim strGetkakunouFolderPath As String = ""
        Dim strGetBackUpFolderPath As String = ""
        Dim strFolderPath As String = ""
        Dim strMsiFolderPath As String = ""
        Dim strInsFolderPath As String = ""
        Dim strOpenFolderPath As String = ""
        Dim strVerUpFolderPath As String = ""
        Dim strFileName As String = ""
        Dim blnDBsetuzokuflg As Boolean = False
        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                'トランザクション
                cmdDB.Connection = conDB
            Else
                blnDBsetuzokuflg = True
                strErrBunrui = "ＤＢ接続"
                Throw New Exception("データベースに接続できません")
            End If

            '更新日時取得
            Dim dteUpdSysDate As DateTime = DateTime.Now()

            '各ファイルの格納フォルダパスの取得
            strGetFolderPath = Me.mfGetFolderPath(cmdDB, conDB, strFILESYUBETU_TEIKYO)           '提供フォルダ
            strGetkakunouFolderPath = Me.mfGetFolderPath(cmdDB, conDB, strFILESYUBETU_KAKUNO)    '格納フォルダ
            strGetBackUpFolderPath = Me.mfGetFolderPath(cmdDB, conDB, strFILESYUBETU_BACKUP)     'バックアップフォルダ
            strFolderPath = strGetFolderPath & strKOJI_SEKKEI                                    '工事設計依頼書フォルダ
            strMsiFolderPath = strGetFolderPath & strKOJI_SEKKEI_MEISAI                          '工事設計依頼明細フォルダ
            strInsFolderPath = strGetFolderPath & strINS_RENRAKU                                 'INS連絡フォルダ
            strOpenFolderPath = strGetFolderPath & strOPEN_RENRAKU                               'オープン日連絡フォルダ
            strVerUpFolderPath = strGetFolderPath & strVERUP_RENRAKU                             'TBOXVerUp単独工事フォルダ

            'フォルダより、ファイル名を取得
            Dim strKojiFullPathFileNms() As String = System.IO.Directory.GetFiles(strFolderPath)            '工事設計依頼書
            Dim strKojiMeisaiFullPathFileNms() As String = System.IO.Directory.GetFiles(strMsiFolderPath)   '工事設計依頼明細
            Dim strInsFullPathFileNms() As String = System.IO.Directory.GetFiles(strInsFolderPath)          'INS連絡
            Dim strOpenFullPathFileNms() As String = System.IO.Directory.GetFiles(strOpenFolderPath)        'オープン日連絡
            Dim strVerFullPathFileNms() As String = System.IO.Directory.GetFiles(strVerUpFolderPath)        'TBOXVerUp単独工事

            'ファイルがどれかひとつでも取得できていない場合は処理終了
            If strKojiFullPathFileNms.Length = 0 And _
                strKojiMeisaiFullPathFileNms.Length = 0 And _
                strInsFullPathFileNms.Length = 0 And _
                strOpenFullPathFileNms.Length = 0 And _
                strVerFullPathFileNms.Length = 0 Then
                Return True
            End If

            'INS連絡ファイルが存在するか確認
            If strInsFullPathFileNms.Length <> 0 Then
                'INS連絡ファイルが存在する場合
                For i As Integer = 0 To strInsFullPathFileNms.Length - 1
                    'ファイル名取得
                    strFileName = System.IO.Path.GetFileName(strInsFullPathFileNms(i))
                    'エラー分類設定
                    strErrBunrui = "その他処理エラー"
                    '提供場所から格納場所にファイル移動
                    System.IO.File.Move(strInsFullPathFileNms(i), _
                                        strGetkakunouFolderPath & strINS_RENRAKU & "\\" & strFileName)
                Next
                '格納場所のファイル名（フルパス）を取得
                Dim strInsKakunoFullPathFileNms() As String = System.IO.Directory.GetFiles(strGetkakunouFolderPath & strINS_RENRAKU)
                '■TMSDB ＩＮＳ連絡書　D01登録
                Me.msUpdateInsData(strGetkakunouFolderPath & strINS_RENRAKU, strInsKakunoFullPathFileNms, cmdDB, conDB, dteUpdSysDate)

                For i As Integer = 0 To strInsKakunoFullPathFileNms.Length - 1
                    'ファイル名取得
                    strFileName = System.IO.Path.GetFileName(strInsKakunoFullPathFileNms(i))
                    'ファイル名から、日付 & 連番部分 & 拡張子を取得
                    Dim strCompVal As String = strFileName.Replace(strHD_INS_RENRAKU, "")
                    strFileName = strHD_INS_RENRAKU & DateTime.Now.ToString("yyyyMM") & strCompVal
                    'エラー分類設定
                    strErrBunrui = "その他処理エラー"
                    '格納場所からバックアップにファイル移動
                    System.IO.File.Move(strInsKakunoFullPathFileNms(i), _
                                        strGetBackUpFolderPath & strINS_RENRAKU & "\\" & strFileName)
                Next

            End If

            '工事設計依頼書ファイルが存在するか確認
            If strKojiFullPathFileNms.Length <> 0 Then
                '工事設計依頼書ファイルが存在する場合
                '工事依頼設計依頼明細ファイルが存在するか確認
                If strKojiMeisaiFullPathFileNms.Length <> 0 Then
                    '工事依頼設計依頼明細ファイルが存在する場合
                    For i As Integer = 0 To strKojiFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strKojiFullPathFileNms(i))
                        strErrBunrui = "その他処理エラー"
                        '工事依頼設計書を提供場所から格納場所にファイル移動
                        System.IO.File.Move(strKojiFullPathFileNms(i), _
                                            strGetkakunouFolderPath & strKOJI_SEKKEI & "\\" & strFileName)
                    Next
                    For i As Integer = 0 To strKojiMeisaiFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strKojiMeisaiFullPathFileNms(i))
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '工事依頼設計明細を提供場所から格納場所にファイル移動
                        System.IO.File.Move(strKojiMeisaiFullPathFileNms(i), _
                                            strGetkakunouFolderPath & strKOJI_SEKKEI_MEISAI & "\\" & strFileName)
                    Next
                    '工事設計依頼書ファイル格納場所のファイル名（フルパス）を取得
                    Dim strKojiKakunoFullPathFileNms() As String = System.IO.Directory.GetFiles(strGetkakunouFolderPath & strKOJI_SEKKEI)
                    '■TMSDB 工事設計依頼書 D02登録
                    Me.msUpdateKojiData(strGetkakunouFolderPath & strKOJI_SEKKEI, strKojiFullPathFileNms, cmdDB, conDB, dteUpdSysDate, "1")

                    For i As Integer = 0 To strKojiKakunoFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strKojiKakunoFullPathFileNms(i))
                        'ファイル名から、日付 & 連番部分 & 拡張子を取得
                        Dim strCompVal As String = strFileName.Replace(strHD_KOJI_SEKKEI_IRAI, "")
                        strFileName = strHD_KOJI_SEKKEI_IRAI & DateTime.Now.ToString("yyyyMM") & strCompVal
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '格納場所からバックアップにファイル移動
                        System.IO.File.Move(strKojiKakunoFullPathFileNms(i), _
                                            strGetBackUpFolderPath & strKOJI_SEKKEI & "\\" & strFileName)
                    Next

                    '工事設計依頼明細ファイル格納場所のファイル名（フルパス）を取得
                    Dim strKojiMeisaiKakunoFullPathFileNms() As String = System.IO.Directory.GetFiles(strGetkakunouFolderPath & strKOJI_SEKKEI_MEISAI)
                    '■TMSDB 工事設計依頼明細 D022登録
                    Me.msUpdateKojiMsiData(strGetkakunouFolderPath & strKOJI_SEKKEI_MEISAI, strKojiMeisaiKakunoFullPathFileNms, cmdDB, conDB, dteUpdSysDate)

                    For i As Integer = 0 To strKojiMeisaiKakunoFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strKojiMeisaiKakunoFullPathFileNms(i))
                        'ファイル名から、日付 & 連番部分 & 拡張子を取得
                        Dim strCompVal As String = strFileName.Replace(strHD_KOJI_SEKKEI_IRAI_MSI, "")
                        strFileName = strHD_KOJI_SEKKEI_IRAI_MSI & DateTime.Now.ToString("yyyyMM") & strCompVal
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '格納場所からバックアップにファイル移動
                        System.IO.File.Move(strKojiMeisaiKakunoFullPathFileNms(i), _
                                            strGetBackUpFolderPath & strKOJI_SEKKEI_MEISAI & "\\" & strFileName)
                    Next
                End If

            End If

            'TBOXVerUp単独工事ファイルが存在するか確認
            If strVerFullPathFileNms.Length <> 0 Then
                'TBOXVerUp単独工事ファイルが存在する場合
                '工事依頼設計依頼明細ファイルが存在するか確認
                If strKojiMeisaiFullPathFileNms.Length <> 0 Then
                    '工事依頼設計依頼明細ファイルが存在する場合
                    For i As Integer = 0 To strVerFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strVerFullPathFileNms(i))
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '工事依頼設計書を提供場所から格納場所にファイル移動
                        System.IO.File.Move(strVerFullPathFileNms(i), _
                                            strGetkakunouFolderPath & strVERUP_RENRAKU & "\\" & strFileName)
                    Next
                    For i As Integer = 0 To strKojiMeisaiFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strKojiMeisaiFullPathFileNms(i))
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '工事依頼設計明細を提供場所から格納場所にファイル移動
                        System.IO.File.Move(strKojiMeisaiFullPathFileNms(i), _
                                            strGetkakunouFolderPath & strKOJI_SEKKEI_MEISAI & "\\" & strFileName)
                    Next
                    'TBOXVerUp単独工事ファイル格納場所のファイル名（フルパス）を取得
                    Dim strVerKakunoFullPathFileNms() As String = System.IO.Directory.GetFiles(strGetkakunouFolderPath & strVERUP_RENRAKU)
                    '■TMSDB 工事設計依頼書 D02登録（工事設計依頼と同じ構造のため、同じメソッドを使用）
                    Me.msUpdateKojiData(strGetkakunouFolderPath & strVERUP_RENRAKU, strVerKakunoFullPathFileNms, cmdDB, conDB, dteUpdSysDate, "2")

                    For i As Integer = 0 To strVerKakunoFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strVerKakunoFullPathFileNms(i))
                        'ファイル名から、日付 & 連番部分 & 拡張子を取得
                        Dim strCompVal As String = strFileName.Replace(strHD_VERUP_RENRAKU, "")
                        strFileName = strHD_VERUP_RENRAKU & DateTime.Now.ToString("yyyyMM") & strCompVal
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '格納場所からバックアップにファイル移動
                        System.IO.File.Move(strVerKakunoFullPathFileNms(i), _
                                            strGetBackUpFolderPath & strVERUP_RENRAKU & "\\" & strFileName)
                    Next

                    '工事設計依頼明細ファイル格納場所のファイル名（フルパス）を取得
                    Dim strKojiMeisaiKakunoFullPathFileNms() As String = System.IO.Directory.GetFiles(strGetkakunouFolderPath & strKOJI_SEKKEI_MEISAI)
                    '■TMSDB 工事設計依頼明細 D022登録（TBOXVerUp単独工事でも同じ明細を使用するため同じメソッド、ファイルを使用）
                    Me.msUpdateKojiMsiData(strGetkakunouFolderPath & strKOJI_SEKKEI_MEISAI, strKojiMeisaiKakunoFullPathFileNms, cmdDB, conDB, dteUpdSysDate)

                    For i As Integer = 0 To strKojiMeisaiKakunoFullPathFileNms.Length - 1
                        'ファイル名取得
                        strFileName = System.IO.Path.GetFileName(strKojiMeisaiKakunoFullPathFileNms(i))
                        'ファイル名から、日付 & 連番部分 & 拡張子を取得
                        Dim strCompVal As String = strFileName.Replace(strHD_KOJI_SEKKEI_IRAI_MSI, "")
                        strFileName = strHD_KOJI_SEKKEI_IRAI_MSI & DateTime.Now.ToString("yyyyMM") & strCompVal
                        'エラー分類設定
                        strErrBunrui = "その他処理エラー"
                        '格納場所からバックアップにファイル移動
                        System.IO.File.Move(strKojiMeisaiKakunoFullPathFileNms(i), _
                                            strGetBackUpFolderPath & strKOJI_SEKKEI_MEISAI & "\\" & strFileName)
                    Next
                End If

            End If

            'オープン日連絡ファイルが存在するか確認
            If strOpenFullPathFileNms.Length <> 0 Then
                'オープン日連絡ファイルが存在する場合
                For i As Integer = 0 To strOpenFullPathFileNms.Length - 1
                    'ファイル名取得
                    strFileName = System.IO.Path.GetFileName(strOpenFullPathFileNms(i))
                    'エラー分類設定
                    strErrBunrui = "その他処理エラー"
                    '提供場所から格納場所にファイル移動
                    System.IO.File.Move(strOpenFullPathFileNms(i), _
                                        strGetkakunouFolderPath & strOPEN_RENRAKU & "\\" & strFileName)
                Next
                '格納場所のファイル名（フルパス）を取得
                Dim strOpenKakunoFullPathFileNms() As String = System.IO.Directory.GetFiles(strGetkakunouFolderPath & strOPEN_RENRAKU)
                '■TMSDB  オープン日連絡　D07登録
                Me.msUpdateOpenData(strGetkakunouFolderPath & strOPEN_RENRAKU, strOpenKakunoFullPathFileNms, cmdDB, conDB, dteUpdSysDate)

                For i As Integer = 0 To strOpenKakunoFullPathFileNms.Length - 1
                    'ファイル名取得
                    strFileName = System.IO.Path.GetFileName(strOpenKakunoFullPathFileNms(i))
                    'ファイル名から、日付 & 連番部分 & 拡張子を取得
                    Dim strCompVal As String = strFileName.Replace(strHD_OPEN_RENRAKU, "")
                    strFileName = strHD_OPEN_RENRAKU & DateTime.Now.ToString("yyyyMM") & strCompVal
                    'エラー分類設定
                    strErrBunrui = "その他処理エラー"
                    '格納場所からバックアップにファイル移動
                    System.IO.File.Move(strOpenKakunoFullPathFileNms(i), _
                                        strGetBackUpFolderPath & strOPEN_RENRAKU & "\\" & strFileName)
                Next

            End If


            'オブジェクトの破棄を保証する
            cmdDB.Dispose()

            ''フォルダより、ファイル名を取得
            'Dim strFileNms() As String = System.IO.Directory.GetFiles(strFolderPath)
            'Using trans As SqlTransaction = conDB.BeginTransaction
            '    '■TMSDB 工事設計依頼書 D02登録
            '    'Me.msUpdateKojiData(trans, cmdDB, conDB, strFileNms, dteUpdSysDate)

            '    '■TMSDB 工事設計依頼明細　D022登録(★データ型変更待ち)
            '    'Me.msUpdateKojiMsiData(strMsiFolderPath, trans, cmdDB, conDB, dteUpdSysDate)

            '    '■TMSDB ＩＮＳ連絡書　D01登録
            '    'Me.msUpdateInsData(strInsFolderPath, strFileNms, trans, cmdDB, conDB, dteUpdSysDate)

            '    '■TMSDB オープン日連絡　D07登録
            '    Me.msUpdateOpenData(strOpenFolderPath, trans, cmdDB, conDB, dteUpdSysDate)

            '    '■TMSDB バージョンアップ連絡　D02登録
            '    'Me.msUpdateVerUpData(strVerUpFolderPath, trans, cmdDB, conDB, dteUpdSysDate)

            '    '■SPCDB ホール情報更新
            '    '1)更新処理(工事設計依頼書データ、ホール情報データ両方にホールコードが存在するものについて(ホール名、更新日時、更新者)を更新)
            '    Me.msDataUpdateHallInfo(cmdDB, conDB, trans, dteUpdSysDate)

            '    '2)新規登録処理(工事設計依頼書データにホールコードがあり、ホール情報データにホールコードが存在しないもの)
            '    Me.msDataInsertHallInfo(cmdDB, conDB, trans, dteUpdSysDate)

            '    '3)論理削除処理(ホール情報データにホールコードが存在し、工事設計依頼書データにホールコードが存在しないもの)
            '    Me.msDataDeleteHallInfo(cmdDB, conDB, trans, dteUpdSysDate)

            '    ''■SPCDB TBOX情報更新★更新項目は？NL区分に結びつく工事設計依頼書データの項目は？
            '    ''1)TBOX情報更新(更新処理(工事設計依頼書データ、TBOX情報データ両方にT-BOXIDが存在するものについて)
            '    'Me.msDataUpdateTBOXInfo(cmdDB, conDB, trans,dteUpdSysDate)

            '    ''2)T-BOX情報新規登録処理(工事設計依頼書データにT-BOXIDがあり、T-BOX情報データにT-BOXIDが存在しないもの)
            '    'Me.msDataInsertTBOXInfo(cmdDB, conDB, trans,dteUpdSysDate)

            '    ''3)論理削除処理(T-BOX情報データにT-BOXIDが存在し、工事設計依頼書データにT-BOXIDが存在しないもの)
            '    'Me.msDataDeleteTBOXInfo(cmdDB, conDB, trans,dteUpdSysDate)

            '    'コミッテッド
            '    trans.Commit()

            '    'オブジェクトの破棄を保証する
            '    cmdDB.Dispose()
            'End Using

            Return True

        Catch ex As DBConcurrencyException
            Return False
        Catch ex As Exception
            If blnDBsetuzokuflg = False Then
                msErrLog(cmdDB, conDB, strErrBunrui, "pfKojiDataShushinDataWrite", "2", System.IO.Path.GetFileNameWithoutExtension(strFileName), _
                         Nothing, 0, strTableName, ex.ToString)
            Else
                'データベース接続時のエラー
                msDBErrLog(strErrBunrui, "pfKojiDataShushinDataWrite", ex.ToString)
            End If
            Return False
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

    End Function

    ''' <summary>
    ''' 格納フォルダ取得
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetFolderPath(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal strVal As String) As String
        'プロシージャ設定
        cmdDB = New SqlCommand("SMTDSY001_S8", conDB)

        With cmdDB.Parameters
            .Add(pfSet_Param("prmM78_FILECLASS_CD", SqlDbType.NVarChar, strVal))
        End With

        'データ検索
        Dim objDtSet As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)
        Dim strRetVal As String = ""

        '検索データのフォルダ名を取得
        If Not objDtSet Is Nothing AndAlso objDtSet.Tables.Count > 0 Then
            'データは存在するか？
            If objDtSet.Tables(0).Rows.Count > 0 Then
                Dim objRow As DataRow = objDtSet.Tables(0).Rows(0)

                If objRow("M78_FOLDER_NM") Is DBNull.Value Then
                    '名前が登録されていない場合は空白
                    strRetVal = ""
                Else
                    'フォルダ名設定
                    strRetVal = Convert.ToString(objRow("M78_FOLDER_NM"))
                End If
            Else
                '存在しないので空
                strVal = ""
            End If

        End If

        Return strRetVal

    End Function

    ''' <summary>
    ''' ファイル名に同一の値が含まれるかチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfInsContainsVal(ByVal strFileNms() As String, ByVal strCompVal As String) As Boolean
        For i As Integer = 0 To strFileNms.Length - 1
            'ファイル名を取得
            Dim strVal As String = System.IO.Path.GetFileNameWithoutExtension(strFileNms(i))
            'INSファイル名の数字部分にマッチするファイル名が存在するか
            If strVal.Contains(strCompVal) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' ファイル名チェック
    ''' </summary>
    ''' <param name="strFileNm"></param>
    ''' <param name="strFileNmHead"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkFileNm(ByVal strFileNm As String, ByVal strFileNmHead As String) As Boolean
        '拡張子をのぞいたファイル名前の文字部分を切り取って数字のみがのこらなければ、異なるファイル
        If pfCheck_Num(strFileNm.Replace(strFileNmHead, "")) Then
            Return True
        End If
        Return False
    End Function

    ' ''' <summary>
    ' ''' 工事設計依頼書 D02登録
    ' ''' </summary>
    ' ''' <param name="trans"></param>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>    
    ' ''' <remarks></remarks>
    'Private Sub msUpdateKojiData(ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal strFileNms() As String, ByVal dteSysDate As DateTime)

    '    For i As Integer = 0 To strFileNms.Length - 1

    '        '拡張子を取り除いたファイル名
    '        Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strFileNms(i))
    '        '正しいファイルかどうかチェック
    '        If Not Me.mfChkFileNm(strWithoutEx, strHD_KOJI_SEKKEI_IRAI) Then
    '            '違う名前のファイルを読み込んだので次のファイルを読み込み
    '            Continue For
    '        End If

    '        'CSVファイルの内容をリストとして取得
    '        Dim objFileData As List(Of String()) = pfReadCsvFile(strFileNms(i))

    '        'リストのデータ数分、工事設計依頼書データを更新する
    '        For j As Integer = 0 To objFileData.Count - 1

    '            'プロシージャ設定
    '            cmdDB = New SqlCommand("SMTDSY001_S4", conDB)

    '            'ストアドプロシージャで実行
    '            cmdDB.CommandType = CommandType.StoredProcedure

    '            cmdDB.Transaction = trans

    '            With cmdDB.Parameters
    '                .Add(pfSet_Param("prmD02_CONSTRACT", SqlDbType.NVarChar, objFileData(j)(EKojiDataIndex.工事依頼番号)))
    '            End With

    '            'データ検索
    '            Dim objDtSet As DataSet = pfGet_DataSet(cmdDB)

    '            'CSVレコードの工事依頼番号にマッチする工事設計依頼書データが存在するかチェック
    '            If (Not objDtSet Is Nothing) AndAlso objDtSet.Tables.Count > 0 Then
    '                If objDtSet.Tables(0).Rows.Count > 0 Then
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U2", conDB)
    '                Else
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U1", conDB)
    '                End If

    '                '更新パラメータの設定
    '                Me.msSetParam(cmdDB, objFileData(j), strFileNms(i), dteSysDate)

    '                'ストアドプロシージャで実行
    '                cmdDB.CommandType = CommandType.StoredProcedure

    '                cmdDB.Transaction = trans

    '                '実行
    '                cmdDB.ExecuteNonQuery()

    '            Else
    '                Throw New Exception()
    '            End If

    '        Next
    '    Next
    'End Sub

    ' ''' <summary>
    ' ''' 工事設計依頼明細　D022登録
    ' ''' </summary>
    ' ''' <param name="strMsiFolderPath"></param>
    ' ''' <param name="trans"></param>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <remarks></remarks>
    'Private Sub msUpdateKojiMsiData(ByVal strMsiFolderPath As String, ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
    '    Dim strFileMsiNms() As String = System.IO.Directory.GetFiles(strMsiFolderPath)

    '    For i As Integer = 0 To strFileMsiNms.Length - 1

    '        '拡張子を取り除いたファイル名
    '        Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strFileMsiNms(i))
    '        '正しいファイルかどうかチェック
    '        If Not Me.mfChkFileNm(strWithoutEx, strHD_KOJI_SEKKEI_IRAI_MSI) Then
    '            '違う名前のファイルを読み込んだので次のファイルを読み込み
    '            Continue For
    '        End If

    '        'CSVファイルの内容をリストとして取得
    '        Dim objMsiFileData As List(Of String()) = pfReadCsvFile(strFileMsiNms(i))
    '        Dim strIraiNo As String = ""
    '        Dim intSeqNo As Integer = 1
    '        Dim objDataTbl As New DataTable()

    '        objDataTbl = Me.mfChgKojiMsiDataTbl(objMsiFileData)

    '        '並べ替え
    '        Dim objDataView As DataView = objDataTbl.DefaultView
    '        objDataView.Sort = "D022_CONST_NO"

    '        'リストのデータ数分、工事設計依頼書データを更新する
    '        For j As Integer = 0 To objDataView.Count - 1
    '            If strIraiNo <> objDataView.Item(j)("D022_CONST_NO") Then
    '                strIraiNo = objDataView.Item(j)("D022_CONST_NO")
    '                intSeqNo = 1
    '            Else
    '                intSeqNo += 1
    '            End If

    '            'プロシージャ設定
    '            cmdDB = New SqlCommand("SMTDSY001_S5", conDB)

    '            'ストアドプロシージャで実行
    '            cmdDB.CommandType = CommandType.StoredProcedure

    '            'パラメータ設定
    '            With cmdDB.Parameters
    '                .Add(pfSet_Param("prmD022_CONST_NO", SqlDbType.NVarChar, strIraiNo))
    '                .Add(pfSet_Param("prmD022_SEQNO", SqlDbType.Int, intSeqNo))
    '            End With

    '            cmdDB.Transaction = trans

    '            'データ検索
    '            Dim objDs As DataSet = pfGet_DataSet(cmdDB)

    '            'CSVレコードの工事依頼番号,連番にマッチする工事設計依頼書明細データが存在するかチェック
    '            If (Not objDs Is Nothing) OrElse objDs.Tables.Count > 0 Then
    '                If objDs.Tables(0).Rows.Count > 0 Then
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U4", conDB)
    '                Else
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U3", conDB)
    '                End If

    '                'パラメータの設定
    '                Me.msSetMsiParam(cmdDB, objDataView.Item(j), intSeqNo, dteSysDate)

    '                'ストアドプロシージャで実行
    '                cmdDB.CommandType = CommandType.StoredProcedure

    '                cmdDB.Transaction = trans

    '                '実行
    '                cmdDB.ExecuteNonQuery()

    '            Else
    '                Throw New Exception()
    '            End If

    '        Next
    '    Next
    'End Sub

    ''' <summary>
    ''' ＩＮＳ連絡書　D01登録
    ''' </summary>
    ''' <param name="strInsFolderPath"></param>
    ''' <param name="strFileNms"></param>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <remarks></remarks>
    Private Sub msUpdateInsData(ByVal strInsFolderPath As String, ByVal strFileNms() As String, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
        '    Private Sub msUpdateInsData(ByVal strInsFolderPath As String, ByVal strFileNms() As String, ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
        Dim strInsFileNms() As String = System.IO.Directory.GetFiles(strInsFolderPath)

        'INSCOMテーブル更新登録
        For i As Integer = 0 To strInsFileNms.Length - 1

            '拡張子を取り除いたファイル名
            Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strInsFileNms(i))
            '正しいファイルかどうかチェック
            If Not Me.mfChkFileNm(strWithoutEx, strHD_INS_RENRAKU) Then
                '違う名前のファイルを読み込んだので次のファイルを読み込み
                Continue For
            End If

            'ファイル名から、日付 & 連番部分を取得
            Dim strCompVal As String = strWithoutEx.Replace(strHD_INS_RENRAKU, "")

            'ファイル名に同一の文字列(日付 & 連番部分)が含まれるかチェック
            If Me.mfInsContainsVal(strFileNms, strCompVal) Then
                'CSVファイルの内容をリストとして取得()
                Dim objInsFileData As List(Of String()) = pfReadCsvFile(strInsFileNms(i))

                For j As Integer = 0 To objInsFileData.Count - 1

                    '---INS連絡表反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S6", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD01_CONST_NO", SqlDbType.NVarChar, objInsFileData(j)(EKojiInsDataIndex.工事依頼番号)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータの工事依頼番号にマッチするINS連絡所データが存在するかチェック
                        If (Not objDs Is Nothing) AndAlso objDs.Tables.Count > 0 Then
                            If objDs.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U6", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "D01_INSCOM"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U5", conDB)
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "D01_INSCOM"
                            End If

                            'パラメータの設定
                            Me.msSetInsParam(cmdDB, objInsFileData(j), strInsFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateInsData", "1", strWithoutEx, objInsFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If

                            Catch Ex As Exception
                                trans1.Rollback()
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateInsData", "1", strWithoutEx, objInsFileData, j, strTableName, Ex.ToString)
                                'ログ出力
                            End Try
                        Else
                            Throw New Exception()
                        End If

                    End Using

                    '---TBOX情報反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S9", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD01_TBOXID", SqlDbType.NVarChar, objInsFileData(j)(EKojiInsDataIndex.ＴＢＯＸＩＤ)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDsTbox As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータのＴＢＯＸにマッチするＴＢＯＸ情報が存在するかチェック
                        If (Not objDsTbox Is Nothing) AndAlso objDsTbox.Tables.Count > 0 Then
                            If objDsTbox.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U9", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "T03_TBOX"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_I1", conDB) '
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "T03_TBOX"
                            End If

                            'パラメータの設定
                            Me.msSetInsParam(cmdDB, objInsFileData(j), strInsFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateInsData", "1", strWithoutEx, objInsFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateInsData", "1", strWithoutEx, objInsFileData, j, strTableName, Ex.ToString)
                                'ログ出力
                            End Try
                        Else
                            Throw New Exception()
                        End If
                    End Using

                    '---ホール情報反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S10", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD01_HALL_CD", SqlDbType.NVarChar, objInsFileData(j)(EKojiInsDataIndex.ホールコード)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDsTbox As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータのホールコードにマッチするホール情報が存在するかチェック
                        If (Not objDsTbox Is Nothing) AndAlso objDsTbox.Tables.Count > 0 Then
                            If objDsTbox.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U10", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "T01_HALL"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_I2", conDB) '
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "T01_HALL"
                            End If

                            'パラメータの設定
                            Me.msSetInsParam(cmdDB, objInsFileData(j), strInsFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateInsData", "1", strWithoutEx, objInsFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateInsData", "1", strWithoutEx, objInsFileData, j, strTableName, Ex.ToString)
                                'ログ出力
                            End Try
                        Else
                            Throw New Exception()
                        End If
                    End Using

                Next

            End If
        Next
    End Sub

    ''' <summary>
    ''' 工事設計依頼書 D02登録
    ''' </summary>
    ''' <param name="strKojiFolderPath"></param>
    ''' <param name="strFileNms"></param>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <remarks></remarks>
    Private Sub msUpdateKojiData(ByVal strKojiFolderPath As String, ByVal strFileNms() As String, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime, ByVal strFileKbn As String)
        Dim strKojiFileNms() As String = System.IO.Directory.GetFiles(strKojiFolderPath)

        '工事設計依頼テーブル更新登録
        For i As Integer = 0 To strKojiFileNms.Length - 1

            '拡張子を取り除いたファイル名
            Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strKojiFileNms(i))
            Dim strCompVal As String
            '正しいファイルかどうかチェック
            If strFileKbn = "1" Then

                If Not Me.mfChkFileNm(strWithoutEx, strHD_KOJI_SEKKEI_IRAI) Then
                    '違う名前のファイルを読み込んだので次のファイルを読み込み
                    Continue For
                End If

                'ファイル名から、日付 & 連番部分を取得
                strCompVal = strWithoutEx.Replace(strHD_KOJI_SEKKEI_IRAI, "")
            Else
                If Not Me.mfChkFileNm(strWithoutEx, strHD_VERUP_RENRAKU) Then
                    '違う名前のファイルを読み込んだので次のファイルを読み込み
                    Continue For
                End If

                'ファイル名から、日付 & 連番部分を取得
                strCompVal = strWithoutEx.Replace(strHD_VERUP_RENRAKU, "")

            End If


            'ファイル名に同一の文字列(日付 & 連番部分)が含まれるかチェック
            If Me.mfInsContainsVal(strFileNms, strCompVal) Then
                'CSVファイルの内容をリストとして取得()
                Dim objKojiFileData As List(Of String()) = pfReadCsvFile(strKojiFileNms(i))

                For j As Integer = 0 To objKojiFileData.Count - 1

                    '---工事設計依頼反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S4", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD02_CONSTRACT", SqlDbType.NVarChar, objKojiFileData(j)(EKojiDataIndex.工事依頼番号)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータの工事依頼番号にマッチする工事設計依頼書データが存在するかチェック
                        If (Not objDs Is Nothing) AndAlso objDs.Tables.Count > 0 Then
                            If objDs.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U2", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "D02_CONSTRACT"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U1", conDB)
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "D02_CONSTRACT"
                            End If

                            'パラメータの設定
                            Me.msSetParam(cmdDB, objKojiFileData(j), strKojiFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", "1", strWithoutEx, objKojiFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", "1", strWithoutEx, objKojiFileData, j, strTableName, Ex.ToString)
                                'ログ出力
                            End Try
                        Else
                            Throw New Exception()
                        End If

                    End Using

                    '---TBOX情報反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S9", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD01_TBOXID", SqlDbType.NVarChar, objKojiFileData(j)(EKojiDataIndex.ＴＢＯＸ＿ＩＤ)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDsTbox As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータのＴＢＯＸにマッチするＴＢＯＸ情報が存在するかチェック
                        If (Not objDsTbox Is Nothing) AndAlso objDsTbox.Tables.Count > 0 Then
                            If objDsTbox.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U14", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "T03_TBOX"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_I3", conDB)
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "T03_TBOX"
                            End If

                            'パラメータの設定
                            Me.msSetParam(cmdDB, objKojiFileData(j), strKojiFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", "1", strWithoutEx, objKojiFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                'ログ出力
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", "1", strWithoutEx, objKojiFileData, j, strTableName, Ex.ToString)
                            End Try
                        Else
                            Throw New Exception()
                        End If
                    End Using

                    '---ホール情報反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S10", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD01_HALL_CD", SqlDbType.NVarChar, objKojiFileData(j)(EKojiDataIndex.ホールコード)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDsTbox As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータのホールコードにマッチするホール情報が存在するかチェック
                        If (Not objDsTbox Is Nothing) AndAlso objDsTbox.Tables.Count > 0 Then
                            If objDsTbox.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U15", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "T01_HALL"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_I4", conDB) '
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "T01_HALL"
                            End If

                            'パラメータの設定
                            Me.msSetParam(cmdDB, objKojiFileData(j), strKojiFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", "1", strWithoutEx, objKojiFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", "1", strWithoutEx, objKojiFileData, j, strTableName, Ex.ToString)
                                'ログ出力
                            End Try
                        Else
                            Throw New Exception()
                        End If
                    End Using

                Next

            End If
        Next
    End Sub


    ''' <summary>
    ''' 工事設計依頼書 D02登録
    ''' </summary>
    ''' <param name="strKojiMEisaiFolderPath"></param>
    ''' <param name="strFileNms"></param>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <remarks></remarks>
    Private Sub msUpdateKojiMsiData(ByVal strKojiMeisaiFolderPath As String, ByVal strFileNms() As String, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
        Dim strKojiMeisaiFileNms() As String = System.IO.Directory.GetFiles(strKojiMeisaiFolderPath)

        '工事設計明細テーブル更新登録
        For i As Integer = 0 To strKojiMeisaiFileNms.Length - 1

            '拡張子を取り除いたファイル名
            Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strKojiMeisaiFileNms(i))
            '正しいファイルかどうかチェック
            If Not Me.mfChkFileNm(strWithoutEx, strHD_KOJI_SEKKEI_IRAI_MSI) Then
                '違う名前のファイルを読み込んだので次のファイルを読み込み
                Continue For
            End If

            'ファイル名から、日付 & 連番部分を取得
            Dim strCompVal As String = strWithoutEx.Replace(strHD_KOJI_SEKKEI_IRAI_MSI, "")

            'ファイル名に同一の文字列(日付 & 連番部分)が含まれるかチェック
            If Me.mfInsContainsVal(strFileNms, strCompVal) Then
                'CSVファイルの内容をリストとして取得()
                Dim objMsiFileData As List(Of String()) = pfReadCsvFile(strKojiMeisaiFileNms(i))
                Dim strIraiNo As String = ""
                Dim intSeqNo As Integer = 1
                Dim objDataTbl As New DataTable()
                objDataTbl = Me.mfChgKojiMsiDataTbl(objMsiFileData)

                '並べ替え
                Dim objDataView As DataView = objDataTbl.DefaultView
                objDataView.Sort = "D022_CONST_NO"

                For j As Integer = 0 To objDataView.Count - 1

                    '---工事設計依頼反映---
                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        If strIraiNo <> objDataView.Item(j)("D022_CONST_NO") Then
                            strIraiNo = objDataView.Item(j)("D022_CONST_NO")
                            intSeqNo = 1
                        Else
                            intSeqNo += 1
                        End If

                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S5", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD022_CONST_NO", SqlDbType.NVarChar, strIraiNo))
                            .Add(pfSet_Param("prmD022_SEQNO", SqlDbType.Int, intSeqNo))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータの工事依頼番号にマッチする工事設計依頼書データが存在するかチェック
                        If (Not objDs Is Nothing) AndAlso objDs.Tables.Count > 0 Then
                            If objDs.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U4", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "D022_CONSTRACT"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U3", conDB)
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "D022_CONSTRACT"
                            End If

                            'パラメータの設定
                            Me.msSetMsiParam(cmdDB, objDataView.Item(j), intSeqNo, strKojiMeisaiFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", strWithoutEx, objDataView.Item(j), strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                'ログ出力
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateKojiData", strWithoutEx, objDataView.Item(j), strTableName, Ex.ToString)
                            End Try
                        Else
                            Throw New Exception()
                        End If

                    End Using

                Next

            End If
        Next
    End Sub

    ''' <summary>
    ''' オープン日連絡　D07登録
    ''' </summary>
    ''' <param name="strOpenFolderPath"></param>
    ''' <param name="strFileNms"></param>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <remarks></remarks>
    Private Sub msUpdateOpenData(ByVal strOpenFolderPath As String, ByVal strFileNms() As String, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
        '    Private Sub msUpdateInsData(ByVal strInsFolderPath As String, ByVal strFileNms() As String, ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
        Dim strOpenFileNms() As String = System.IO.Directory.GetFiles(strOpenFolderPath)

        'OPENDATEテーブル更新登録
        For i As Integer = 0 To strOpenFileNms.Length - 1

            '拡張子を取り除いたファイル名
            Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strOpenFileNms(i))
            '正しいファイルかどうかチェック
            If Not Me.mfChkFileNm(strWithoutEx, strHD_OPEN_RENRAKU) Then
                '違う名前のファイルを読み込んだので次のファイルを読み込み
                Continue For
            End If

            'ファイル名から、日付 & 連番部分を取得
            Dim strCompVal As String = strWithoutEx.Replace(strHD_OPEN_RENRAKU, "")

            'ファイル名に同一の文字列(日付 & 連番部分)が含まれるかチェック
            If Me.mfInsContainsVal(strFileNms, strCompVal) Then
                'CSVファイルの内容をリストとして取得()
                Dim objOpenFileData As List(Of String()) = pfReadCsvFile(strOpenFileNms(i))

                For j As Integer = 0 To objOpenFileData.Count - 1

                    Using trans1 As SqlTransaction = conDB.BeginTransaction
                        'プロシージャ設定
                        cmdDB = New SqlCommand("SMTDSY001_S7", conDB)

                        'ストアドプロシージャで実行
                        cmdDB.CommandType = CommandType.StoredProcedure

                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("prmD07_CONST_NO", SqlDbType.NVarChar, objOpenFileData(j)(EKojiOpenDataIndex.工事依頼番号)))
                        End With

                        cmdDB.Transaction = trans1

                        'データ検索
                        Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                        'CSVデータの工事依頼番号にマッチするINS連絡所データが存在するかチェック
                        If (Not objDs Is Nothing) AndAlso objDs.Tables.Count > 0 Then
                            If objDs.Tables(0).Rows.Count > 0 Then
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U8", conDB)
                                strErrBunrui = "ＳＱＬ実行（更新）"
                                strTableName = "D07_OPENDATE"
                            Else
                                'プロシージャ設定
                                cmdDB = New SqlCommand("SMTDSY001_U7", conDB)
                                strErrBunrui = "ＳＱＬ実行（登録）"
                                strTableName = "D07_OPENDATE"
                            End If

                            'パラメータの設定
                            Me.msSetOpenParam(cmdDB, objOpenFileData(j), strOpenFileNms(i), dteSysDate)
                            Try
                                'ストアドプロシージャで実行
                                cmdDB.CommandType = CommandType.StoredProcedure

                                cmdDB.Transaction = trans1

                                '実行
                                cmdDB.ExecuteNonQuery()
                                Dim intReturn As Integer = Integer.Parse(cmdDB.Parameters("returnCode").Value.ToString)
                                If intReturn <> 0 Then
                                    trans1.Rollback()
                                    msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateOpenData", "1", strWithoutEx, objOpenFileData, j, strTableName, "ストアドプロシージャ：" & intReturn)
                                Else
                                    'コミッテッド
                                    trans1.Commit()
                                End If
                            Catch Ex As Exception
                                trans1.Rollback()
                                msErrLog(cmdDB, conDB, strErrBunrui, "msUpdateOpenData", "1", strWithoutEx, objOpenFileData, j, strTableName, Ex.ToString)
                                'ログ出力
                            End Try
                        Else
                            Throw New Exception()
                        End If

                    End Using

                Next

            End If
        Next
    End Sub

    ' ''' <summary>
    ' ''' オープン日連絡　D07登録
    ' ''' </summary>
    ' ''' <param name="strOpenFolderPath"></param>
    ' ''' <param name="trans"></param>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <remarks></remarks>
    'Private Sub msUpdateOpenData(ByVal strOpenFolderPath As String, ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
    '    'フォルダより、オープン日連絡ファイル名を取得
    '    Dim strOpenFileNms() As String = System.IO.Directory.GetFiles(strOpenFolderPath)

    '    For i As Integer = 0 To strOpenFileNms.Length - 1

    '        '拡張子を取り除いたファイル名
    '        Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strOpenFileNms(i))
    '        '正しいファイルかどうかチェック
    '        If Not Me.mfChkFileNm(strWithoutEx, strHD_OPEN_RENRAKU) Then
    '            '違う名前のファイルを読み込んだので次のファイルを読み込み
    '            Continue For
    '        End If

    '        'CSVファイルの内容をリストとして取得
    '        Dim objOpenFileData As List(Of String()) = pfReadCsvFile(strOpenFileNms(i))

    '        For j As Integer = 0 To objOpenFileData.Count - 1
    '            'プロシージャ設定
    '            cmdDB = New SqlCommand("SMTDSY001_S7", conDB)

    '            'ストアドプロシージャで実行
    '            cmdDB.CommandType = CommandType.StoredProcedure

    '            cmdDB.Transaction = trans

    '            With cmdDB.Parameters
    '                .Add(pfSet_Param("prmD07_CONST_NO", SqlDbType.NVarChar, objOpenFileData(j)(EKojiOpenDataIndex.工事依頼番号)))
    '                .Add(pfSet_Param("prmD07_SEND_T", SqlDbType.NVarChar, objOpenFileData(j)(EKojiOpenDataIndex.送信時間)))
    '                .Add(pfSet_Param("prmD07_SEND_D", SqlDbType.NVarChar, objOpenFileData(j)(EKojiOpenDataIndex.送信日付)))
    '            End With

    '            'データ検索
    '            Dim objDs As DataSet = pfGet_DataSet(cmdDB)

    '            'パラメータ設定
    '            If (Not objDs Is Nothing) AndAlso objDs.Tables.Count > 0 Then
    '                If objDs.Tables(0).Rows.Count > 0 Then
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U8", conDB)
    '                Else
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U7", conDB)
    '                End If

    '                'パラメータ設定
    '                Me.msSetOpenParam(cmdDB, objOpenFileData(j), strOpenFileNms(i), dteSysDate)

    '                'ストアドプロシージャで実行
    '                cmdDB.CommandType = CommandType.StoredProcedure

    '                cmdDB.Transaction = trans

    '                '実行
    '                cmdDB.ExecuteNonQuery()

    '            Else
    '                Throw New Exception()
    '            End If

    '        Next
    '    Next
    'End Sub

    ' ''' <summary>
    ' ''' バージョンアップ連絡　D02更新
    ' ''' </summary>
    ' ''' <param name="strVerUpFolderPath"></param>
    ' ''' <param name="trans"></param>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>    
    ' ''' <remarks></remarks>
    'Private Sub msUpdateVerUpData(ByVal strVerUpFolderPath As String, ByRef trans As SqlTransaction, ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByVal dteSysDate As DateTime)
    '    'フォルダより、verup連絡ファイル名を取得(※ファイルの形は工事設計依頼書のものと同一。バージョンアップのため、かならず更新処理をおこなう)
    '    Dim strVerUPFileNms() As String = System.IO.Directory.GetFiles(strVerUpFolderPath)

    '    For i As Integer = 0 To strVerUPFileNms.Length - 1

    '        '拡張子を取り除いたファイル名
    '        Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strVerUPFileNms(i))
    '        '正しいファイルかどうかチェック
    '        If Not Me.mfChkFileNm(strWithoutEx, strHD_VERUP_RENRAKU) Then
    '            '違う名前のファイルを読み込んだので次のファイルを読み込み
    '            Continue For
    '        End If

    '        'CSVファイルの内容をリストとして取得
    '        Dim objVerUpFileData As List(Of String()) = pfReadCsvFile(strVerUPFileNms(i))

    '        'プロシージャ設定
    '        cmdDB = New SqlCommand("SMTDSY001_U2", conDB)

    '        'ストアドプロシージャで実行
    '        cmdDB.CommandType = CommandType.StoredProcedure

    '        cmdDB.Transaction = trans

    '        'パラメータ設定(※型は工事設計依頼と同一)
    '        Me.msSetParam(cmdDB, objVerUpFileData(i), strVerUPFileNms(i), dteSysDate)

    '        '実行
    '        cmdDB.ExecuteNonQuery()
    '    Next
    'End Sub

    ' ''' <summary>
    ' ''' ホール情報更新(更新処理(工事設計依頼書データ、ホール情報データ両方にホールコードが存在するものについて)
    ' ''' </summary>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <param name="trans"></param>
    ' ''' <remarks></remarks>
    'Private Sub msDataUpdateHallInfo(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef trans As SqlTransaction, ByVal dteSysDate As DateTime)
    '    Dim objExDs As New DataSet()
    '    'プロシージャ設定
    '    cmdDB = New SqlCommand("SMTDSY001_S1", conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    objExDs = pfGet_DataSet(cmdDB)

    '    If (Not objExDs Is Nothing) AndAlso objExDs.Tables.Count > 0 Then
    '        If objExDs.Tables(0).Rows.Count > 0 Then
    '            Dim objExDataTbl As DataTable = objExDs.Tables(0)

    '            If objExDataTbl Is Nothing Then
    '                Throw New Exception()
    '            End If

    '            If objExDataTbl.Rows.Count > 0 Then
    '                For i As Integer = 0 To objExDataTbl.Rows.Count - 1
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U11", conDB)

    '                    'ストアドプロシージャで実行
    '                    cmdDB.CommandType = CommandType.StoredProcedure

    '                    cmdDB.Transaction = trans

    '                    'パラメータ設定
    '                    With cmdDB.Parameters
    '                        Dim objRow As DataRow = objExDataTbl.Rows(i)
    '                        .Add(pfSet_Param("prmT01_HALL_NAME", SqlDbType.NVarChar, objRow("D02_HALL_NM"))) 'ホール名
    '                        .Add(pfSet_Param("prmT01_HALL_CD", SqlDbType.NVarChar, objRow("D02_HALL_CD"))) 'ホールコード
    '                        .Add(pfSet_Param("prmT01_DT", SqlDbType.DateTime, dteSysDate))
    '                    End With

    '                    '実行
    '                    cmdDB.ExecuteNonQuery()

    '                Next
    '            End If
    '        End If
    '    Else
    '        Throw New Exception()
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' ホール情報新規登録処理(工事設計依頼書データにホールコードがあり、ホール情報データにホールコードが存在しないもの)
    ' ''' </summary>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <param name="trans"></param>
    ' ''' <remarks></remarks>
    'Private Sub msDataInsertHallInfo(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef trans As SqlTransaction, ByVal dteSysDate As DateTime)
    '    Dim objNExDs As New DataSet()

    '    'プロシージャ設定
    '    cmdDB = New SqlCommand("SMTDSY001_S2", conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    objNExDs = pfGet_DataSet(cmdDB)

    '    If (Not objNExDs Is Nothing) AndAlso objNExDs.Tables.Count > 0 Then
    '        If objNExDs.Tables(0).Rows.Count > 0 Then
    '            Dim objNExDataTbl As DataTable = objNExDs.Tables(0)

    '            If objNExDataTbl Is Nothing Then
    '                Throw New Exception
    '            End If

    '            If objNExDataTbl.Rows.Count > 0 Then
    '                For i As Integer = 0 To objNExDataTbl.Rows.Count - 1
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U12", conDB)

    '                    'ストアドプロシージャで実行
    '                    cmdDB.CommandType = CommandType.StoredProcedure

    '                    cmdDB.Transaction = trans

    '                    Dim objRow As DataRow = objNExDataTbl.Rows(i)

    '                    'パラメータ設定
    '                    With cmdDB.Parameters
    '                        .Add(pfSet_Param("prmT01_HALL_CD", SqlDbType.NVarChar, objRow("D02_HALL_CD"))) 'ホールコード
    '                        .Add(pfSet_Param("prmT01_HALL_NAME", SqlDbType.NVarChar, objRow("D02_HALL_NM"))) 'ホール名
    '                        .Add(pfSet_Param("prmT01_ADDR", SqlDbType.NVarChar, objRow("D02_HALL_ADDR"))) 'ホール住所
    '                        .Add(pfSet_Param("prmT01_TELNO", SqlDbType.NVarChar, objRow("D02_HALL_TEL"))) 'ホール電話番号
    '                        .Add(pfSet_Param("prmT01_DT", SqlDbType.DateTime, dteSysDate))
    '                    End With

    '                    '実行
    '                    cmdDB.ExecuteNonQuery()
    '                Next
    '            End If
    '        End If
    '    Else
    '        Throw New Exception()
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' 論理削除処理(ホール情報データにホールコードが存在し、工事設計依頼書データにホールコードが存在しないもの)
    ' ''' </summary>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <param name="trans"></param>
    ' ''' <remarks></remarks>
    'Private Sub msDataDeleteHallInfo(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef trans As SqlTransaction, ByVal dteSysDate As DateTime)
    '    Dim objHNExDs As New DataSet()

    '    'プロシージャ設定
    '    cmdDB = New SqlCommand("SMTDSY001_S3", conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    objHNExDs = pfGet_DataSet(cmdDB)

    '    If (Not objHNExDs Is Nothing) AndAlso objHNExDs.Tables.Count > 0 Then
    '        If objHNExDs.Tables(0).Rows.Count > 0 Then
    '            Dim objHNExDataTbl As DataTable = objHNExDs.Tables(0)

    '            If objHNExDataTbl Is Nothing Then
    '                Throw New Exception
    '            End If

    '            If objHNExDataTbl.Rows.Count > 0 Then
    '                For i As Integer = 0 To objHNExDataTbl.Rows.Count - 1
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("SMTDSY001_U13", conDB)

    '                    'ストアドプロシージャで実行
    '                    cmdDB.CommandType = CommandType.StoredProcedure

    '                    cmdDB.Transaction = trans

    '                    Dim objRow As DataRow = objHNExDataTbl.Rows(i)

    '                    'パラメータ設定
    '                    With cmdDB.Parameters
    '                        .Add(pfSet_Param("prmT01_HALL_CD", SqlDbType.NVarChar, objRow("T01_HALL_CD"))) 'ホールコード
    '                        .Add(pfSet_Param("prmT01_DT", SqlDbType.DateTime, dteSysDate))
    '                    End With

    '                    '実行
    '                    cmdDB.ExecuteNonQuery()
    '                Next
    '            End If
    '        End If
    '    Else
    '        Throw New Exception()
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' TBOX情報更新(更新処理(工事設計依頼書データ、TBOX情報データ両方にT-BOXIDが存在するものについて)
    ' ''' </summary>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <param name="trans"></param>
    ' ''' <remarks></remarks>
    'Private Sub msDataUpdateTBOXInfo(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef trans As SqlTransaction, ByVal dteSysDate As DateTime)
    '    Dim objExDs As New DataSet()
    '    'プロシージャ設定
    '    cmdDB = New SqlCommand("★プロシージャ名", conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    objExDs = pfGet_DataSet(cmdDB)

    '    If (Not objExDs Is Nothing) AndAlso objExDs.Tables.Count > 0 Then
    '        If objExDs.Tables(0).Rows.Count > 0 Then
    '            Dim objExDataTbl As DataTable = objExDs.Tables(0)

    '            If objExDataTbl Is Nothing Then
    '                Throw New Exception()
    '            End If

    '            If objExDataTbl.Rows.Count > 0 Then
    '                For i As Integer = 0 To objExDataTbl.Rows.Count - 1
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("★プロシージャ名", conDB)

    '                    'ストアドプロシージャで実行
    '                    cmdDB.CommandType = CommandType.StoredProcedure

    '                    cmdDB.Transaction = trans

    '                    'パラメータ設定
    '                    With cmdDB.Parameters
    '                        Dim objRow As DataRow = objExDataTbl.Rows(i)
    '                        '★必要なパラメータは？QA中。
    '                        .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, objRow("D02_HALL_NM"))) 'T-BOXID
    '                        .Add(pfSet_Param("prmT03_NL_CLS", SqlDbType.NVarChar, objRow("D02_HALL_CD"))) 'NL区分
    '                    End With

    '                    '実行
    '                    cmdDB.ExecuteNonQuery()

    '                Next
    '            End If
    '        End If
    '    Else
    '        Throw New Exception()
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' T-BOX情報新規登録処理(工事設計依頼書データにT-BOXIDがあり、T-BOX情報データにT-BOXIDが存在しないもの)
    ' ''' </summary>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <param name="trans"></param>
    ' ''' <remarks></remarks>
    'Private Sub msDataInsertTBOXInfo(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef trans As SqlTransaction)
    '    Dim objNExDs As New DataSet()

    '    'プロシージャ設定
    '    cmdDB = New SqlCommand("★プロシージャ名", conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    objNExDs = pfGet_DataSet(cmdDB)

    '    If (Not objNExDs Is Nothing) AndAlso objNExDs.Tables.Count > 0 Then
    '        If objNExDs.Tables(0).Rows.Count > 0 Then
    '            Dim objNExDataTbl As DataTable = objNExDs.Tables(0)

    '            If objNExDataTbl Is Nothing Then
    '                Throw New Exception
    '            End If

    '            If objNExDataTbl.Rows.Count > 0 Then
    '                For i As Integer = 0 To objNExDataTbl.Rows.Count - 1
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("★プロシージャ名", conDB)

    '                    'ストアドプロシージャで実行
    '                    cmdDB.CommandType = CommandType.StoredProcedure

    '                    cmdDB.Transaction = trans

    '                    Dim objRow As DataRow = objNExDataTbl.Rows(i)

    '                    'パラメータ設定
    '                    With cmdDB.Parameters
    '                        '★必要なパラメータは？QA中。
    '                        .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, objRow("D02_HALL_NM"))) 'T-BOXID
    '                        .Add(pfSet_Param("prmT03_NL_CLS", SqlDbType.NVarChar, objRow("D02_HALL_CD"))) 'NL区分
    '                    End With

    '                    '実行
    '                    cmdDB.ExecuteNonQuery()
    '                Next
    '            End If

    '        End If
    '    Else
    '        Throw New Exception()
    '    End If
    'End Sub

    ' ''' <summary>
    ' ''' 論理削除処理(T-BOX情報データにT-BOXIDが存在し、工事設計依頼書データにT-BOXIDが存在しないもの)
    ' ''' </summary>
    ' ''' <param name="cmdDB"></param>
    ' ''' <param name="conDB"></param>
    ' ''' <param name="trans"></param>
    ' ''' <remarks></remarks>
    'Private Sub msDataDeleteTBOXInfo(ByRef cmdDB As SqlCommand, ByRef conDB As SqlConnection, ByRef trans As SqlTransaction)
    '    Dim objHNExDs As New DataSet()

    '    'プロシージャ設定
    '    cmdDB = New SqlCommand("★プロシージャ名", conDB)

    '    'ストアドプロシージャで実行
    '    cmdDB.CommandType = CommandType.StoredProcedure

    '    cmdDB.Transaction = trans

    '    objHNExDs = pfGet_DataSet(cmdDB)

    '    If (Not objHNExDs Is Nothing) AndAlso objHNExDs.Tables.Count > 0 Then
    '        If objHNExDs.Tables(0).Rows.Count > 0 Then
    '            Dim objHNExDataTbl As DataTable = objHNExDs.Tables(0)

    '            If objHNExDataTbl Is Nothing Then
    '                Throw New Exception
    '            End If

    '            If objHNExDataTbl.Rows.Count > 0 Then
    '                For i As Integer = 0 To objHNExDataTbl.Rows.Count - 1
    '                    'プロシージャ設定
    '                    cmdDB = New SqlCommand("★プロシージャ名", conDB)

    '                    'ストアドプロシージャで実行
    '                    cmdDB.CommandType = CommandType.StoredProcedure

    '                    cmdDB.Transaction = trans

    '                    Dim objRow As DataRow = objHNExDataTbl.Rows(i)

    '                    'パラメータ設定
    '                    With cmdDB.Parameters
    '                        '★必要なパラメータは？QA中。
    '                        .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, objRow("D02_HALL_NM"))) 'T-BOXID
    '                        .Add(pfSet_Param("prmT03_NL_CLS", SqlDbType.NVarChar, objRow("D02_HALL_CD"))) 'NL区分
    '                    End With

    '                    '実行
    '                    cmdDB.ExecuteNonQuery()
    '                Next
    '            End If
    '        End If
    '    Else
    '        Throw New Exception()
    '    End If
    'End Sub

    ''' <summary>
    ''' パラメータ設定
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="strVal"></param>
    ''' <remarks></remarks>
    Private Sub msSetParam(ByRef cmdDB As SqlCommand, ByVal strVal() As String, ByVal strFileNms As String, ByVal dteSysDate As DateTime)
        With cmdDB.Parameters
            .Add(pfSet_Param("prmD02_CONST_NO", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事依頼番号)))
            .Add(pfSet_Param("prmD02_SEND_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.送信日付)))
            .Add(pfSet_Param("prmD02_SEND_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.送信時間)))
            .Add(pfSet_Param("prmD02_SPEC_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.仕様連絡区分)))
            .Add(pfSet_Param("prmD02_NOTICE_NO", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事通知番号)))
            .Add(pfSet_Param("prmD02_TBOXID", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＴＢＯＸ＿ＩＤ)))
            .Add(pfSet_Param("prmD02_HALL_NM", SqlDbType.NVarChar, strVal(EKojiDataIndex.ホール名)))
            .Add(pfSet_Param("prmD02_HALL_RSP", SqlDbType.NVarChar, strVal(EKojiDataIndex.ホール責任者名)))
            .Add(pfSet_Param("prmD02_HALL_ADDR", SqlDbType.NVarChar, strVal(EKojiDataIndex.ホール所在地)))
            .Add(pfSet_Param("prmD02_HALL_TEL", SqlDbType.NVarChar, strVal(EKojiDataIndex.ホールＴＥＬ)))
            .Add(pfSet_Param("prmD02_HALL_CD", SqlDbType.NVarChar, strVal(EKojiDataIndex.ホールコード)))
            .Add(pfSet_Param("prmD02_TBOX_TELNO", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＴＢＯＸ回線番号)))
            .Add(pfSet_Param("prmD02_SYSTEM", SqlDbType.NVarChar, strVal(EKojiDataIndex.現行システム)))
            .Add(pfSet_Param("prmD02_SYSTEM_VER", SqlDbType.NVarChar, strVal(EKojiDataIndex.現行システムバージョン)))
            .Add(pfSet_Param("prmD02_CONSTCLS_NEW", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿新規)))
            .Add(pfSet_Param("prmD02_CONSTCLS_ADD", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿増設)))
            .Add(pfSet_Param("prmD02_CONSTCLS_REI", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿再設置)))
            .Add(pfSet_Param("prmD02_CONSTCLS_MOV", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿店内移設)))
            .Add(pfSet_Param("prmD02_CONSTCLS_SRE", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿一部撤去)))
            .Add(pfSet_Param("prmD02_CONSTCLS_REM", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿全撤去)))
            .Add(pfSet_Param("prmD02_CONSTCLS_TRE", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿一時撤去)))
            .Add(pfSet_Param("prmD02_CONSTCLS_CNS", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿構成変更)))
            .Add(pfSet_Param("prmD02_CONSTCLS_OTH", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿その他)))
            .Add(pfSet_Param("prmD02_CONSTCLS_OTH_DTL", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事種別＿その他工事内容)))
            .Add(pfSet_Param("prmD02_TWIN_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.双子店区分)))
            .Add(pfSet_Param("prmD02_SINGLE_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.単独工事区分)))
            .Add(pfSet_Param("prmD02_CONST_CNT", SqlDbType.NVarChar, strVal(EKojiDataIndex.同時工事店舗数)))
            .Add(pfSet_Param("prmD02_PC_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.親子区分)))
            .Add(pfSet_Param("prmD02_PARENT_HALL_CD", SqlDbType.NVarChar, strVal(EKojiDataIndex.親ホールコード)))
            .Add(pfSet_Param("prmD02_F1CNST_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｆ１工事有無)))
            .Add(pfSet_Param("prmD02_F2CNST_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｆ２工事有無)))
            .Add(pfSet_Param("prmD02_F3CNST_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｆ３工事有無)))
            .Add(pfSet_Param("prmD02_F4CNST_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｆ４工事有無)))
            .Add(pfSet_Param("prmD02_STOCKIN_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ストッカホール内)))
            .Add(pfSet_Param("prmD02_STOCKOUT_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ストッカホール外)))
            .Add(pfSet_Param("prmD02_CNST_SD", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事開始日付)))
            .Add(pfSet_Param("prmD02_CNST_ST", SqlDbType.NVarChar, strVal(EKojiDataIndex.工事開始時間)))
            .Add(pfSet_Param("prmD02_OPEN_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.オープン日付)))
            .Add(pfSet_Param("prmD02_OPEN_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.オープン時間)))
            .Add(pfSet_Param("prmD02_PTEST_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.警察検査日付)))
            .Add(pfSet_Param("prmD02_PTEST_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.警察検査時間)))
            .Add(pfSet_Param("prmD02_ATEST_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.総合テスト日付)))
            .Add(pfSet_Param("prmD02_ATEST_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.総合テスト時間)))
            .Add(pfSet_Param("prmD02_LASTWORK_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.最終営業日)))
            .Add(pfSet_Param("prmD02_AGENCY_CD", SqlDbType.NVarChar, strVal(EKojiDataIndex.代理店コード)))
            .Add(pfSet_Param("prmD02_AGENCY_NM", SqlDbType.NVarChar, strVal(EKojiDataIndex.代理店名)))
            .Add(pfSet_Param("prmD02_AGENCY_ADDR", SqlDbType.NVarChar, strVal(EKojiDataIndex.代理店所在地)))
            .Add(pfSet_Param("prmD02_AGENCY_TEL", SqlDbType.NVarChar, strVal(EKojiDataIndex.代理店ＴＥＬ)))
            .Add(pfSet_Param("prmD02_AGC_RSP_NM", SqlDbType.NVarChar, strVal(EKojiDataIndex.代理店責任者)))
            .Add(pfSet_Param("prmD02_AGC_CHARGE", SqlDbType.NVarChar, strVal(EKojiDataIndex.代理店担当者)))
            .Add(pfSet_Param("prmD02_ACTING_CD", SqlDbType.NVarChar, strVal(EKojiDataIndex.代行店コード)))
            .Add(pfSet_Param("prmD02_ACTING_NM", SqlDbType.NVarChar, strVal(EKojiDataIndex.代行店名)))
            .Add(pfSet_Param("prmD02_ACTING_ADDR", SqlDbType.NVarChar, strVal(EKojiDataIndex.代行店所在地)))
            .Add(pfSet_Param("prmD02_ACTING_TEL", SqlDbType.NVarChar, strVal(EKojiDataIndex.代行店ＴＥＬ)))
            .Add(pfSet_Param("prmD02_ACTING_CHARGE", SqlDbType.NVarChar, strVal(EKojiDataIndex.代行店担当者)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_NM", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_ADDR", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先住所)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_RESP", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先責任者)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_TEL", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先ＴＥＬ)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先納入希望日)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_AP", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先午前午後区分)))
            .Add(pfSet_Param("prmD02_LAN_DELIV_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ送付先納入希望時間)))
            .Add(pfSet_Param("prmD02_NOTETEXT", SqlDbType.NVarChar, strVal(EKojiDataIndex.備考)))
            .Add(pfSet_Param("prmD02_SPEC_NOTE", SqlDbType.NVarChar, strVal(EKojiDataIndex.仕様備考)))
            .Add(pfSet_Param("prmD02_CHARGEORG", SqlDbType.NVarChar, strVal(EKojiDataIndex.担当営業部)))
            .Add(pfSet_Param("prmD02_CHARGE_CD", SqlDbType.NVarChar, strVal(EKojiDataIndex.登録社員コード)))
            .Add(pfSet_Param("prmD02_CHARGE_NM", SqlDbType.NVarChar, strVal(EKojiDataIndex.登録社員名)))
            .Add(pfSet_Param("prmD02_SHIFT_CNST", SqlDbType.NVarChar, strVal(EKojiDataIndex.移行工事区分)))
            .Add(pfSet_Param("prmD02_SHIFT_CNST_WORK", SqlDbType.NVarChar, strVal(EKojiDataIndex.移行工事作業区分)))
            .Add(pfSet_Param("prmD02_LASTWORK_T500", SqlDbType.NVarChar, strVal(EKojiDataIndex.最終営業日＿Ｔ５００)))
            .Add(pfSet_Param("prmD02_PRE_CNST", SqlDbType.NVarChar, strVal(EKojiDataIndex.仮設置工事区分)))
            .Add(pfSet_Param("prmD02_PRE_CNST_DCLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.仮設置工事日未入力区分)))
            .Add(pfSet_Param("prmD02_PRE_CNST_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.仮設置工事日付)))
            .Add(pfSet_Param("prmD02_PRE_CNST_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.仮設置工事時間)))
            .Add(pfSet_Param("prmD02_TBOX_TO", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＴＢＯＸ持帰区分)))
            .Add(pfSet_Param("prmD02_TBOX_SCLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.システム分類)))
            .Add(pfSet_Param("prmD02_NJ_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＮＪ区分)))
            .Add(pfSet_Param("prmD02_VERUP_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＶＥＲＵＰ日付)))
            .Add(pfSet_Param("prmD02_VERUP_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＶＥＲＵＰ時間)))
            .Add(pfSet_Param("prmD02_VERUP_KND1", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＶＥＲＵＰ工事種類１)))
            .Add(pfSet_Param("prmD02_VERUP_KND2", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＶＥＲＵＰ工事種類２)))
            .Add(pfSet_Param("prmD02_VERUP_DCLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＶＥＲＵＰ日付区分)))
            .Add(pfSet_Param("prmD02_HALL_CHARGE", SqlDbType.NVarChar, strVal(EKojiDataIndex.ホール担当者名)))
            .Add(pfSet_Param("prmD02_NTTD_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＮＴＴＤ稼動有無)))
            .Add(pfSet_Param("prmD02_LAN_CNST_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事日付)))
            .Add(pfSet_Param("prmD02_LAN_CNST_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事時間)))
            .Add(pfSet_Param("prmD02_LAN_CNST_NEW", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿新規)))
            .Add(pfSet_Param("prmD02_LAN_CNST_ADD", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿増設)))
            .Add(pfSet_Param("prmD02_LAN_CNST_SRE", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿一部撤去)))
            .Add(pfSet_Param("prmD02_LAN_CNST_MOV", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿店内移設)))
            .Add(pfSet_Param("prmD02_LAN_CNST_REM", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿全撤去)))
            .Add(pfSet_Param("prmD02_LAN_CNST_TRE", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿一時撤去)))
            .Add(pfSet_Param("prmD02_LAN_CNST_REI", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿再設置)))
            .Add(pfSet_Param("prmD02_LAN_CNST_CNS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿構成変更)))
            .Add(pfSet_Param("prmD02_LAN_CNST_OTH", SqlDbType.NVarChar, strVal(EKojiDataIndex.ＬＡＮ工事＿その他)))
            .Add(pfSet_Param("prmD02_SYSTEM_CLS", SqlDbType.NVarChar, strVal(EKojiDataIndex.ｼｽﾃﾑ区分)))
            '.Add(pfSet_Param("prmD02_EMON_FLG", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネー導入ホールフラグ)))
            '.Add(pfSet_Param("prmD02_EMON_CNST_FLG", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネー導入工事フラグ)))
            '.Add(pfSet_Param("prmD02_EMON_TEST_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネーテスト日付)))
            '.Add(pfSet_Param("prmD02_EMON_TEST_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネーテスト時間)))
            '.Add(pfSet_Param("prmD02_EMON_FLG", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネー導入ホールフラグ)))
            .Add(pfSet_Param("prmD02_EMON_FLG", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネー導入ホールフラグ)))
            .Add(pfSet_Param("prmD02_EMON_CNST_FLG", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネー導入工事フラグ)))
            .Add(pfSet_Param("prmD02_EMON_TEST_D", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネーテスト日付)))
            .Add(pfSet_Param("prmD02_EMON_TEST_T", SqlDbType.NVarChar, strVal(EKojiDataIndex.Ｅマネーテスト時間)))
            .Add(pfSet_Param("prmD02_FILE_NM", SqlDbType.NVarChar, strFileNms)) 'ファイル名
            .Add(pfSet_Param("prmD02_DT", SqlDbType.DateTime, dteSysDate))
            .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
        End With
    End Sub

    ''' <summary>
    ''' 明細パラメータ設定
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="objDRowView"></param>
    ''' <remarks></remarks>
    Private Sub msSetMsiParam(ByRef cmdDB As SqlCommand, ByVal objDRowView As DataRowView, ByVal intSeqNo As Integer, ByVal strFileNms As String, ByVal dteSysDate As DateTime)
        With cmdDB.Parameters
            .Add(pfSet_Param("prmD022_CONST_NO", SqlDbType.NVarChar, objDRowView("D022_CONST_NO")))
            .Add(pfSet_Param("prmD022_SEQNO", SqlDbType.Int, intSeqNo))
            .Add(pfSet_Param("prmD022_SEND_D", SqlDbType.NVarChar, objDRowView("D022_SEND_D")))
            .Add(pfSet_Param("prmD022_SEND_T", SqlDbType.NVarChar, objDRowView("D022_SEND_T")))
            .Add(pfSet_Param("prmD022_AGENCY_CD", SqlDbType.NVarChar, objDRowView("D022_AGENCY_CD")))
            .Add(pfSet_Param("prmD022_NOTICE_NO", SqlDbType.NVarChar, objDRowView("D022_NOTICE_NO")))
            .Add(pfSet_Param("prmD022_FREQENCY", SqlDbType.NVarChar, objDRowView("D022_FREQENCY")))
            .Add(pfSet_Param("prmD022_APPA_NM", SqlDbType.NVarChar, objDRowView("D022_APPA_NM")))
            .Add(pfSet_Param("prmD022_CNSTB_WRKCNT", SqlDbType.Int, objDRowView("D022_CNSTB_WRKCNT")))
            .Add(pfSet_Param("prmD022_CNSTB_STCCNT", SqlDbType.Int, objDRowView("D022_CNSTB_STCCNT")))
            .Add(pfSet_Param("prmD022_CNSTB_ALLCNT", SqlDbType.Int, objDRowView("D022_CNSTB_ALLCNT")))
            .Add(pfSet_Param("prmD022_CNSTN_MOVCNT", SqlDbType.Int, objDRowView("D022_CNSTN_MOVCNT")))
            .Add(pfSet_Param("prmD022_CNSTN_REVCNT", SqlDbType.Int, objDRowView("D022_CNSTN_REVCNT")))
            .Add(pfSet_Param("prmD022_CNSTN_NEWCNT", SqlDbType.Int, objDRowView("D022_CNSTN_NEWCNT")))
            .Add(pfSet_Param("prmD022_CNSRN_STCCNT", SqlDbType.Int, objDRowView("D022_CNSRN_STCCNT")))
            .Add(pfSet_Param("prmD022_CNSTN_OSTCCNT", SqlDbType.Int, objDRowView("D022_CNSTN_OSTCCNT")))
            .Add(pfSet_Param("prmD022_CNSTN_TRNCNT", SqlDbType.Int, objDRowView("D022_CNSTN_TRNCNT")))
            .Add(pfSet_Param("prmD022_CNSTN_AGCCNT", SqlDbType.Int, objDRowView("D022_CNSTN_AGCCNT")))
            .Add(pfSet_Param("prmD022_CNSTA_WRKCNT", SqlDbType.Int, objDRowView("D022_CNSTA_WRKCNT")))
            .Add(pfSet_Param("prmD022_CNSTA_STCCNT", SqlDbType.Int, objDRowView("D022_CNSTA_STCCNT")))
            .Add(pfSet_Param("prmD022_CNSTA_ALLCNT", SqlDbType.Int, objDRowView("D022_CNSTA_ALLCNT")))
            .Add(pfSet_Param("prmD022_RGSTEMP_CD", SqlDbType.NVarChar, objDRowView("D022_RGSTEMP_CD")))
            .Add(pfSet_Param("prmD022_RGSTEMP_NM", SqlDbType.NVarChar, objDRowView("D022_RGSTEMP_NM")))
            .Add(pfSet_Param("prmD022_FILE_NM", SqlDbType.NVarChar, strFileNms))
            .Add(pfSet_Param("prmD022_DT", SqlDbType.DateTime, dteSysDate))
            .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
        End With
    End Sub

    ''' <summary>
    ''' INSリストパラメータ設定
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="strVal"></param>
    ''' <param name="strFileNms"></param>
    ''' <remarks></remarks>
    Private Sub msSetInsParam(ByRef cmdDB As SqlCommand, ByVal strVal() As String, ByVal strFileNms As String, ByVal dteSysDate As DateTime)
        With cmdDB.Parameters
            .Add(pfSet_Param("prmD01_CONST_NO", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.工事依頼番号)))
            .Add(pfSet_Param("prmD01_SEND_D", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.送信日付)))
            .Add(pfSet_Param("prmD01_SEND_T", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.送信時間)))
            .Add(pfSet_Param("prmD01_INS_CLS", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ連絡区分)))
            .Add(pfSet_Param("prmD01_NOTICE_NO", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.工事通知番号)))
            .Add(pfSet_Param("prmD01_HALL_NM", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ホール名)))
            .Add(pfSet_Param("prmD01_HALL_CD", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ホールコード)))
            .Add(pfSet_Param("prmD01_NJ_CLS", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＮＪ区分)))
            .Add(pfSet_Param("prmD01_TBOXID", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＴＢＯＸＩＤ)))
            .Add(pfSet_Param("prmD01_DDXP", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＤＤＸＰ回線番号)))
            .Add(pfSet_Param("prmD01_LCGN", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＬＣＧＮ)))
            .Add(pfSet_Param("prmD01_LCN", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＬＣＮ)))
            .Add(pfSet_Param("prmD01_INSAPP_CLS", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ申込要否)))
            .Add(pfSet_Param("prmD01_INSAPP_D", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ申込日)))
            .Add(pfSet_Param("prmD01_INSOPN_D", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ開通日付)))
            .Add(pfSet_Param("prmD01_INSOPN_T", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ開通時間)))
            .Add(pfSet_Param("prmD01_INS_TELNO", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ回線番号)))
            .Add(pfSet_Param("prmD01_INS_CNDT", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ回線現状)))
            .Add(pfSet_Param("prmD01_NTT_BRANCH", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＮＴＴ申込支店名)))
            .Add(pfSet_Param("prmD01_NTT_BRANCH_TEL", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＮＴＴ申込支店ＴＥＬ)))
            .Add(pfSet_Param("prmD01_NTT_CHARGE", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＮＴＴ申込支店担当者名)))
            .Add(pfSet_Param("prmD01_NOTETEXT", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.特記事項)))
            .Add(pfSet_Param("prmD01_EMPLOYEE_CD", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.登録社員コード)))
            .Add(pfSet_Param("prmD01_EMPLOYEE_NM", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.登録社員名)))
            .Add(pfSet_Param("prmD01_SYSTEM_KND", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.システム分類)))
            .Add(pfSet_Param("prmD01_INSNAME", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ＩＮＳ回線名義)))
            .Add(pfSet_Param("prmD01_CENTER_D", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.センタ登録日)))
            .Add(pfSet_Param("prmD01_CNTR_NOTETEXT", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.センタ登録備考)))
            .Add(pfSet_Param("prmD01_INS_CNTLNO", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.回線管理番号)))
            .Add(pfSet_Param("prmD01_SYSTEM_CLS", SqlDbType.NVarChar, strVal(EKojiInsDataIndex.ｼｽﾃﾑ区分)))
            .Add(pfSet_Param("prmD01_FILE_NM", SqlDbType.NVarChar, strFileNms)) 'ファイル名
            .Add(pfSet_Param("prmD01_DT", SqlDbType.DateTime, dteSysDate))
            .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
        End With
    End Sub

    ''' <summary>
    ''' オープン日連絡 パラメータ設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetOpenParam(ByRef cmdDB As SqlCommand, ByVal strVal() As String, ByVal strFileNms As String, ByVal dteSysDate As DateTime)
        With cmdDB.Parameters

            '同時工事店舗数を計算
            Dim intConstCnt As Integer = 0
            If strVal(EKojiOpenDataIndex.同時工事店舗数).Trim() = "" Then
                intConstCnt = 0
            Else
                intConstCnt = Convert.ToInt16(strVal(EKojiOpenDataIndex.同時工事店舗数).Trim())
            End If

            .Add(pfSet_Param("prmD07_CONST_NO", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事依頼番号)))
            .Add(pfSet_Param("prmD07_SEND_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.送信日付)))
            .Add(pfSet_Param("prmD07_SEND_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.送信時間)))
            .Add(pfSet_Param("prmD07_SPECCOM_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.仕様連絡区分)))
            .Add(pfSet_Param("prmD07_CNSTCOM_NO", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事通知番号)))
            .Add(pfSet_Param("prmD07_TBOXID", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＴＢＯＸＩＤ)))
            .Add(pfSet_Param("prmD07_HALL_NM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ホール名)))
            .Add(pfSet_Param("prmD07_HALL_RSP", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ホール責任者名)))
            .Add(pfSet_Param("prmD07_HALL_ADDR", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ホール所在地)))
            .Add(pfSet_Param("prmD07_HALL_TEL", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ホールＴＥＬ)))
            .Add(pfSet_Param("prmD07_HALL_CD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ホールコード)))
            .Add(pfSet_Param("prmD07_TBOX_TEL", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＴＢＯＸ回線番号)))
            .Add(pfSet_Param("prmD07_SYSTEM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.現行システム)))
            .Add(pfSet_Param("prmD07_SYSTEM_VER", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.現行システムバージョン)))
            .Add(pfSet_Param("prmD07_CNSTCLS_NEW", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿新規)))
            .Add(pfSet_Param("prmD07_CNSTCLS_ADD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿増設)))
            .Add(pfSet_Param("prmD07_CNSTCLS_REI", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿再設置)))
            .Add(pfSet_Param("prmD07_CNSTCLS_MOV", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿店内移設)))
            .Add(pfSet_Param("prmD07_CNSTCLS_SRE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿一部撤去)))
            .Add(pfSet_Param("prmD07_CNSTCLS_REM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿全撤去)))
            .Add(pfSet_Param("prmD07_CNSTCLS_TRE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿一時撤去)))
            .Add(pfSet_Param("prmD07_CNSTCLS_CNS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿構成変更)))
            .Add(pfSet_Param("prmD07_CNSTCLS_OTH", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿その他)))
            .Add(pfSet_Param("prmD07_CNSTCLS_OTHDTL", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事種別＿その他工事内容)))
            .Add(pfSet_Param("prmD07_TWIN_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.双子店区分)))
            .Add(pfSet_Param("prmD07_SINGLE_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.単独工事区分)))
            .Add(pfSet_Param("prmD07_CONST_CNT", SqlDbType.SmallInt, intConstCnt))          'e同時工事店舗数
            .Add(pfSet_Param("prmD07_PC_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.親子区分)))
            .Add(pfSet_Param("prmD07_PARENT_HALL_CD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.親ホールコード)))
            .Add(pfSet_Param("prmD07_F1CNST_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.Ｆ１工事有無)))
            .Add(pfSet_Param("prmD07_F2CNST_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.Ｆ２工事有無)))
            .Add(pfSet_Param("prmD07_F3CNST_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.Ｆ３工事有無)))
            .Add(pfSet_Param("prmD07_F4CNST_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.Ｆ４工事有無)))
            .Add(pfSet_Param("prmD07_STOCKIN_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ストッカホール内)))
            .Add(pfSet_Param("prmD07_STOCKOUT_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ストッカホール外)))
            .Add(pfSet_Param("prmD07_CNST_SD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事開始日付)))
            .Add(pfSet_Param("prmD07_CNST_ST", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.工事開始時間)))
            .Add(pfSet_Param("prmD07_OPEN_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.オープン日付)))
            .Add(pfSet_Param("prmD07_OPEN_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.オープン時間)))
            .Add(pfSet_Param("prmD07_PTEST_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.警察検査日付)))
            .Add(pfSet_Param("prmD07_PTEST_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.警察検査時間)))
            .Add(pfSet_Param("prmD07_ATEST_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.総合テスト日付)))
            .Add(pfSet_Param("prmD07_ATEST_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.総合テスト時間)))
            .Add(pfSet_Param("prmD07_LASTWORK_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.最終営業日)))
            .Add(pfSet_Param("prmD07_AGENCY_CD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代理店コード)))
            .Add(pfSet_Param("prmD07_AGENCY_NM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代理店名)))
            .Add(pfSet_Param("prmD07_AGENCY_ADDR", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代理店所在地)))
            .Add(pfSet_Param("prmD07_AGENCY_TEL", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代理店ＴＥＬ)))
            .Add(pfSet_Param("prmD07_AGC_RSP_NM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代理店責任者)))
            .Add(pfSet_Param("prmD07_AGC_CHARGE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代理店担当者)))
            .Add(pfSet_Param("prmD07_ACTING_CD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代行店コード)))
            .Add(pfSet_Param("prmD07_ACTING_NM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代行店名)))
            .Add(pfSet_Param("prmD07_ACTING_ADDR", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代行店所在地)))
            .Add(pfSet_Param("prmD07_ACTING_TEL", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代行店ＴＥＬ)))
            .Add(pfSet_Param("prmD07_ACTING_CHARGE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.代行店担当者)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_NM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_ADDR", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先住所)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_RESP", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先責任者)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_TEL", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先ＴＥＬ)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先納入希望日)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_AP", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先午前午後区分)))
            .Add(pfSet_Param("prmD07_LAN_DELIV_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ送付先納入希望時間)))
            .Add(pfSet_Param("prmD07_NOTETEXT", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.備考)))
            .Add(pfSet_Param("prmD07_SPEC_NOTE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.仕様備考)))
            .Add(pfSet_Param("prmD07_CHARGEORG", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.担当営業部)))
            .Add(pfSet_Param("prmD07_CHARGE_CD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.登録社員コード)))
            .Add(pfSet_Param("prmD07_CHARGE_NM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.登録社員名)))
            .Add(pfSet_Param("prmD07_SHIFT_CNST", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.移行工事区分)))
            .Add(pfSet_Param("prmD07_SHIFT_CNST_WORK", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.移行工事作業区分)))
            .Add(pfSet_Param("prmD07_LASTWORK_T500", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.最終営業日＿Ｔ５００)))
            .Add(pfSet_Param("prmD07_PRE_CNST", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.仮設置工事区分)))
            .Add(pfSet_Param("prmD07_PRE_CNST_DCLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.仮設置工事日未入力区分)))
            .Add(pfSet_Param("prmD07_PRE_CNST_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.仮設置工事日付)))
            .Add(pfSet_Param("prmD07_PRE_CNST_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.仮設置工事時間)))
            .Add(pfSet_Param("prmD07_TBOX_TO", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＴＢＯＸ持帰区分)))
            .Add(pfSet_Param("prmD07_TBOX_SCLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.システム分類)))
            .Add(pfSet_Param("prmD07_NJ_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＮＪ区分)))
            .Add(pfSet_Param("prmD07_VERUP_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＶＥＲＵＰ日付)))
            .Add(pfSet_Param("prmD07_VERUP_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＶＥＲＵＰ時間)))
            .Add(pfSet_Param("prmD07_VERUP_KND1", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＶＥＲＵＰ工事種類１)))
            .Add(pfSet_Param("prmD07_VERUP_KND2", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＶＥＲＵＰ工事種類２)))
            .Add(pfSet_Param("prmD07_VERUP_DCLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＶＥＲＵＰ日付区分)))
            .Add(pfSet_Param("prmD07_HALL_CHARGE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ホール担当者名)))
            .Add(pfSet_Param("prmD07_NTTD_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＮＴＴＤ稼動有無)))
            .Add(pfSet_Param("prmD07_LAN_CNST_D", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事日付)))
            .Add(pfSet_Param("prmD07_LAN_CNST_T", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事時間)))
            .Add(pfSet_Param("prmD07_LAN_CNST_NEW", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿新規)))
            .Add(pfSet_Param("prmD07_LAN_CNST_ADD", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿増設)))
            .Add(pfSet_Param("prmD07_LAN_CNST_SRE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿一部撤去)))
            .Add(pfSet_Param("prmD07_LAN_CNST_MOV", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿店内移設)))
            .Add(pfSet_Param("prmD07_LAN_CNST_REM", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿全撤去)))
            .Add(pfSet_Param("prmD07_LAN_CNST_TRE", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿一時撤去)))
            .Add(pfSet_Param("prmD07_LAN_CNST_REI", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿再設置)))
            .Add(pfSet_Param("prmD07_LAN_CNST_CNS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿構成変更)))
            .Add(pfSet_Param("prmD07_LAN_CNST_OTH", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ＬＡＮ工事＿その他)))
            .Add(pfSet_Param("prmD07_SYSTEM_CLS", SqlDbType.NVarChar, strVal(EKojiOpenDataIndex.ｼｽﾃﾑ区分)))
            .Add(pfSet_Param("prmD07_FILE_NM", SqlDbType.NVarChar, strFileNms)) 'ファイル名
            .Add(pfSet_Param("prmD07_DT", SqlDbType.DateTime, dteSysDate))
            .Add(pfSet_Param("returnCode", SqlDbType.NVarChar))
        End With
    End Sub

    ''' <summary>
    ''' 明細リストのテーブル変換
    ''' </summary>
    ''' <param name="objMsiFileData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChgKojiMsiDataTbl(ByVal objMsiFileData As List(Of String())) As DataTable

        '明細データを格納するテーブルを作成
        Dim objDataTbl As DataTable = New DataTable()
        objDataTbl.Columns.Add("D022_CONST_NO", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_SEND_D", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_SEND_T", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_AGENCY_CD", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_NOTICE_NO", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_FREQENCY", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_APPA_NM", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTB_WRKCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTB_STCCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTB_ALLCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTN_MOVCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTN_REVCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTN_NEWCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSRN_STCCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTN_OSTCCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTN_TRNCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTN_AGCCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTA_WRKCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTA_STCCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_CNSTA_ALLCNT", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_RGSTEMP_CD", Type.GetType("System.String"))
        objDataTbl.Columns.Add("D022_RGSTEMP_NM", Type.GetType("System.String"))

        '各行にデータを設定
        For iidx As Integer = 0 To objMsiFileData.Count - 1
            Dim objRow As DataRow = objDataTbl.NewRow
            objRow("D022_CONST_NO") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事依頼番号)
            objRow("D022_SEND_D") = objMsiFileData(iidx)(EKojiMsiDataIndex.送信日付)
            objRow("D022_SEND_T") = objMsiFileData(iidx)(EKojiMsiDataIndex.送信時間)
            objRow("D022_AGENCY_CD") = objMsiFileData(iidx)(EKojiMsiDataIndex.代理店コード)
            objRow("D022_NOTICE_NO") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事通知番号)
            objRow("D022_FREQENCY") = objMsiFileData(iidx)(EKojiMsiDataIndex.周波数)
            objRow("D022_APPA_NM") = objMsiFileData(iidx)(EKojiMsiDataIndex.機器名)
            objRow("D022_CNSTB_WRKCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事前稼働数)
            objRow("D022_CNSTB_STCCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事前予備数)
            objRow("D022_CNSTB_ALLCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事前総台数)
            objRow("D022_CNSTN_MOVCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.店内移設台数)
            objRow("D022_CNSTN_REVCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.撤去対象台数)
            objRow("D022_CNSTN_NEWCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.新品取付台数)
            objRow("D022_CNSRN_STCCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.自店在庫取付台数)
            objRow("D022_CNSTN_OSTCCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.他店在庫取付台数)
            objRow("D022_CNSTN_TRNCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.譲渡品取付台数)
            objRow("D022_CNSTN_AGCCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.代理店在庫取付台数)
            objRow("D022_CNSTA_WRKCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事後稼働数)
            objRow("D022_CNSTA_STCCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事後予備数)
            objRow("D022_CNSTA_ALLCNT") = objMsiFileData(iidx)(EKojiMsiDataIndex.工事後総台数)
            objRow("D022_RGSTEMP_CD") = objMsiFileData(iidx)(EKojiMsiDataIndex.登録社員コード)
            objRow("D022_RGSTEMP_NM") = objMsiFileData(iidx)(EKojiMsiDataIndex.登録社員名)

            objDataTbl.Rows.Add(objRow)

        Next

        Return objDataTbl

    End Function

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strErrBunrui"></param>
    ''' <param name="strMesod"></param>
    ''' <param name="strKbn"></param>
    ''' <param name="strFileName"></param>
    ''' <param name="objInsFileData"></param>
    ''' <param name="intCnt"></param>
    ''' <param name="strTable"></param>
    ''' <param name="strNaiyo"></param>
    ''' <remarks></remarks>
    Private Sub msErrLog(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByVal strErrBunrui As String, ByVal strMesod As String, ByVal strKbn As String, ByVal strFileName As String, ByVal objInsFileData As List(Of String()), ByVal intCnt As Integer, Optional ByVal strTable As String = Nothing, Optional ByVal strNaiyo As String = Nothing)
        Dim strErrLogFollder As String = "ERRLOG"
        Dim strBackupPath As String = ""
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strSysDate As String = ""
        Dim strErrLog As String = ""
        Dim strCsv As String = ""
        strBackupPath = Me.mfGetFolderPath(cmdDB, conDB, strFILESYUBETU_BACKUP)           'バックアップフォルダ

        strFileName = strBackupPath & strErrLogFollder & "\\" & strFileName & strEXT_LOG

        'パラメータに設定されたＣＳＶの内容を出力する
        If strKbn = "1" Then
            For i = 0 To objInsFileData(intCnt).Length - 1
                strCsv = strCsv & objInsFileData(intCnt)(i)
                If i < objInsFileData(intCnt).Length - 1 Then
                    strCsv = strCsv & ","
                End If
            Next
        End If
        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記
        strErrLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strErrBunrui & " " & strMesod & " " & strTable & " " & strCsv & " " & strNaiyo

        writer = New System.IO.StreamWriter(strFileName, True, System.Text.Encoding.Default)
        writer.WriteLine(strErrLog)        'エラーログ出力

        writer.Close()

    End Sub

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="cmdDB"></param>
    ''' <param name="conDB"></param>
    ''' <param name="strErrBunrui"></param>
    ''' <param name="strMesod"></param>
    ''' <param name="strFileName"></param>
    ''' <param name="objInsFileData"></param>
    ''' <param name="strTable"></param>
    ''' <param name="strNaiyo"></param>
    ''' <remarks></remarks>
    Private Sub msErrLog(ByVal cmdDB As SqlCommand, ByVal conDB As SqlConnection, ByVal strErrBunrui As String, ByVal strMesod As String, ByVal strFileName As String, ByVal objInsFileData As DataRowView, Optional ByVal strTable As String = Nothing, Optional ByVal strNaiyo As String = Nothing)
        Dim strErrLogFollder As String = "ERRLOG"
        Dim strBackupPath As String = ""
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strSysDate As String = ""
        Dim strErrLog As String = ""
        Dim strCsv As String = ""
        strBackupPath = Me.mfGetFolderPath(cmdDB, conDB, strFILESYUBETU_BACKUP)           'バックアップフォルダ
        strFileName = strBackupPath & strErrLogFollder & "\\" & strFileName & strEXT_LOG
        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記

        'データテーブルに設定されたＣＳＶの内容を出力する
        strCsv = objInsFileData("D022_CONST_NO") & "," & objInsFileData("D022_SEND_D") & "," & _
                 objInsFileData("D022_SEND_T") & "," & objInsFileData("D022_AGENCY_CD") & "," & _
                 objInsFileData("D022_NOTICE_NO") & "," & objInsFileData("D022_FREQENCY") & "," & _
                 objInsFileData("D022_APPA_NM") & "," & objInsFileData("D022_CNSTB_WRKCNT") & "," & _
                 objInsFileData("D022_CNSTB_STCCNT") & "," & objInsFileData("D022_CNSTB_ALLCNT") & "," & _
                 objInsFileData("D022_CNSTN_MOVCNT") & "," & objInsFileData("D022_CNSTN_REVCNT") & "," & _
                 objInsFileData("D022_CNSTN_NEWCNT") & "," & objInsFileData("D022_CNSRN_STCCNT") & "," & _
                 objInsFileData("D022_CNSTN_OSTCCNT") & "," & objInsFileData("D022_CNSTN_TRNCNT") & "," & _
                 objInsFileData("D022_CNSTN_AGCCNT") & "," & objInsFileData("D022_CNSTA_WRKCNT") & "," & _
                 objInsFileData("D022_CNSTA_STCCNT") & "," & objInsFileData("D022_CNSTA_ALLCNT") & "," & _
                 objInsFileData("D022_RGSTEMP_CD") & "," & objInsFileData("D022_RGSTEMP_NM")

        strErrLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strErrBunrui & " " & strMesod & " " & strTable & " " & strCsv & " " & strNaiyo

        writer = New System.IO.StreamWriter(strFileName, True, System.Text.Encoding.Default)
        writer.WriteLine(strErrLog)        'エラーログ出力

        writer.Close()

    End Sub

    ''' <summary>
    ''' エラーログ出力
    ''' </summary>
    ''' <param name="strErrBunrui"></param>
    ''' <param name="strMesod"></param>
    ''' <param name="strNaiyo"></param>
    ''' <remarks></remarks>
    Private Sub msDBErrLog(ByVal strErrBunrui As String, ByVal strMesod As String, Optional ByVal strNaiyo As String = Nothing)
        Dim writer As System.IO.StreamWriter
        Dim dteSysDate As DateTime = DateTime.Now()
        Dim strErrLog As String = ""

        'Shift-JISのテキストファイルを作成します。
        '第２パラメータは既存ファイルが存在する場合の振る舞いを示します。
        'false：上書き、true：追記
        strErrLog = dteSysDate.ToString("yyyy/MM/dd HH:mm:ss") & " " & _
             " " & strErrBunrui & " " & strMesod & " " & "" & " " & "" & " " & strNaiyo

        writer = New System.IO.StreamWriter(strDB_ERR_LOG & dteSysDate.ToString("yyyyMMdd") & strEXT_LOG, _
                                            True, System.Text.Encoding.Default)
        writer.WriteLine(strErrLog)        'エラーログ出力

        writer.Close()

    End Sub

    Private Sub msBackUpFile(ByVal strInsFolderPath As String, ByVal strFileNms() As String)
        Dim strAllFileNms() As String = System.IO.Directory.GetFiles(strInsFolderPath)
        For i As Integer = 0 To strAllFileNms.Length - 1

            '拡張子を取り除いたファイル名
            Dim strWithoutEx As String = System.IO.Path.GetFileNameWithoutExtension(strAllFileNms(i))
            '正しいファイルかどうかチェック
            If Not Me.mfChkFileNm(strWithoutEx, strHD_INS_RENRAKU) Then
                '違う名前のファイルを読み込んだので次のファイルを読み込み
                Continue For
            End If

            'ファイル名から、日付 & 連番部分を取得
            Dim strCompVal As String = strWithoutEx.Replace(strHD_INS_RENRAKU, "")
        Next
    End Sub


#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
