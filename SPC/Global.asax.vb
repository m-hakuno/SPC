Imports System.IO
Imports GrapeCity.ActiveReports.Web
'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　共通処理
'*　ＰＧＭＩＤ：　Global_asax
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.22　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'排他制御用

Public Class Global_asax
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits HttpApplication
#End Region

    '#Region "定数定義"
    '    '============================================================================================================================
    '    '=　定数定義
    '    '===========================================================================================================================
    '    '業務フォルダ
    '    Public Const P_COM As String = "Common"             '共通
    '    Public Const P_CNS As String = "Construction"       '工事
    '    Public Const P_MAI As String = "Maintenance"        '保守
    '    Public Const P_WAT As String = "Watch"              '監視
    '    Public Const P_FLR As String = "FaultReception"     '故障受付
    '    Public Const P_RPE As String = "RepairEequipment"   '修理・設備
    '    Public Const P_SPC As String = "SupportCenter"      'サポートセンタ

    '    '機能ID
    '    Public Const P_FUN_COM As String = "COM"    '共通
    '    Public Const P_FUN_CNS As String = "CNS"    '工事
    '    Public Const P_FUN_DLL As String = "DLL"    '構成配信・DLL変更
    '    Public Const P_FUN_MST As String = "MST"    'マスタ依頼書管理
    '    Public Const P_FUN_SER As String = "SER"    'シリアル管理
    '    Public Const P_FUN_REQ As String = "REQ"    '保守受付
    '    Public Const P_FUN_PRO As String = "PRO"    '保守作業進捗
    '    Public Const P_FUN_CMP As String = "CMP"    '保守完了
    '    Public Const P_FUN_EQU As String = "EQU"    '保守機器管理
    '    Public Const P_FUN_QUA As String = "QUA"    '品質会議資料
    '    Public Const P_FUN_BPI As String = "BPI"    '玉単価設定情報
    '    Public Const P_FUN_CTI As String = "CTI"    'CTI電話受付
    '    Public Const P_FUN_WAT As String = "WAT"    '監視共通
    '    Public Const P_FUN_HEA As String = "HEA"    'ヘルスチェック
    '    Public Const P_FUN_OVE As String = "OVE"    '時間外調査
    '    Public Const P_FUN_SLF As String = "SLF"    '券売機自走調査
    '    Public Const P_FUN_ERR As String = "ERR"    'ノーマル集信エラー
    '    Public Const P_FUN_ICH As String = "ICH"    'ＩＣカード履歴
    '    Public Const P_FUN_DLC As String = "DLC"    'ＤＬＬ変更依頼
    '    Public Const P_FUN_DSU As String = "DSU"    'ＤＳＵ交換対応依頼書
    '    Public Const P_FUN_BRK As String = "BRK"    '故障受付
    '    Public Const P_FUN_PHN As String = "PHN"    '電話集計表
    '    Public Const P_FUN_REP As String = "REP"    '修理
    '    Public Const P_FUN_MNT As String = "MNT"    '整備
    '    Public Const P_FUN_BBP As String = "BBP"    'BB1吸い上げ
    '    Public Const P_FUN_CDP As String = "CDP"    '使用中カードDB吸上
    '    Public Const P_FUN_TBP As String = "TBP"    'TBOX吸上
    '    Public Const P_FUN_DOC As String = "DOC"    '検収書、請求書
    '    Public Const P_FUN_HIL As String = "HIL"    'ホール情報一覧
    '    Public Const P_FUN_SMT As String = "SMT"    'システムメンテナンス

    '    '画面機能ID
    '    Public Const P_SCR_LST As String = "LST"    '一覧
    '    Public Const P_SCR_FIX As String = "FIX"    '確定
    '    Public Const P_SCR_PRO As String = "PRO"    '仮登録
    '    Public Const P_SCR_MNG As String = "MNG"    'マスタ管理
    '    Public Const P_SCR_UPD As String = "UPD"    '更新(編集)
    '    Public Const P_SCR_SEL As String = "SEL"    '参照(詳細)
    '    Public Const P_SCR_INQ As String = "INQ"    '照会
    '    Public Const P_SCR_OUT As String = "OUT"    '帳票出力
    '    Public Const P_SCR_INP As String = "INP"    '入力
    '    Public Const P_SCR_MEN As String = "MEN"    'メニュー
    '    Public Const P_SCR_PRV As String = "PRV"    'プレビュー
    '    Public Const P_SCR_LGI As String = "LGI"    'ログイン

    '    Public Const P_PAGE As String = "P"         'PAGE

    '    '場所
    '    Public Const P_ADD_SPC As String = "SPC"    'サポートセンタ
    '    Public Const P_ADD_NGC As String = "NGC"    'ＮＧＣ
    '    Public Const P_ADD_WKB As String = "WKB"    '作業拠点

    '    'IMEモードのCssClass
    '    Public Const P_CSS_ZEN As String = "ime-active"           '日本語入力
    '    Public Const P_CSS_HAN_COK As String = "ime-inactive"     '半角英数(変更可能)
    '    Public Const P_CSS_HAN_CNG As String = "ime-disabled"     '半角英数(変更不可)

    '    '汎用ボタン名
    '    Public Const P_BTN_NM_DEL As String = "削除"
    '    Public Const P_BTN_NM_CLE As String = "クリア"
    '    Public Const P_BTN_NM_ADD As String = "登録"
    '    Public Const P_BTN_NM_UPD As String = "更新"
    '    Public Const P_BTN_NM_PDF As String = "ＰＤＦ"
    '    Public Const P_BTN_NM_PRI As String = "印刷"
    '    Public Const P_BTN_NM_TRA As String = "送信"

    '    'ＰＤＦ、ＣＳＶ等の保存フォルダ名(~/P_WORK_NM/セッションＩＤ)
    '    Public Const P_WORK_NM = "WORK"

    '    'マスターページタイプ名
    '    Public Const P_SITE_T_NM As String = "site_master"
    '    Public Const P_REFE_T_NM As String = "reference_master"

    '    'セッション変数名
    '    Public Const P_SESSION_USERID As String = "ユーザＩＤ"
    '    Public Const P_SESSION_TERMS As String = "遷移条件"
    '    Public Const P_SESSION_BCLIST As String = "パンくず"
    '    Public Const P_SESSION_NGC_MEN As String = "ＮＧＣメニュー"    '0:メニュー名　1:値
    '    Public Const P_SESSION_OLDDISP As String = "遷移元ＩＤ"
    '    Public Const P_SESSION_PRV_REPORT As String = "レポート"
    '    Public Const P_SESSION_PRV_DATA As String = "データ"
    '    Public Const P_SESSION_PRV_NAME As String = "レポート名"

    '    'セッション変数(排他制御処理)
    '    Public Const P_SESSION_EXCLUSIV_DATE As String = "登録年月日時刻"
    '    Public Const P_SESSION_IP As String = "端末情報"
    '    Public Const P_SESSION_PLACE As String = "場所"
    '    Public Const P_SESSION_LOGIN_DATE As String = "ログイン年月日"
    '    Public Const P_SESSION_SESSTION_ID As String = "セッションＩＤ"
    '    Public Const P_SESSION_GROUP_NUM As String = "グループ番号"

    '    Public Const P_KEY As String = "ＫＥＹ"    '一覧詳細のキー項目

    '    'ViewState名
    '    Public Const P_VIEW_SORT_COL As String = "ソート列"         'ViewStateのソート列キー
    '    Public Const P_VIEW_SORT_DIR As String = "ソート順"         'ViewStateのソート順キー
    '    Public Const P_VIEW_SORT_FLG As String = "ソートフラグ"     'ViewStateのソートフラグキー

    '    'ViewState値
    '    Public Const P_VIEW_SORT_FLG_ON As Short = 1                'ViewStateのソートフラグON

    '    '検証メッセージ
    '    Public Const P_VALMES_SMES As String = "ShortMessage"
    '    Public Const P_VALMES_MES As String = "Message"
    '    Public Const P_VALMES_AST As String = "***"

    '    'ＣＳＶ
    '    Public Const P_CSV_ENCODING As String = "SHIFT-JIS"         'エンコード
    '    Public Const P_CSV_DELIMITER As String = ","                '区切りカンマ
    '    Public Const P_CSV_QUOTE As String = """"                   '引用符（ダブルクォーテーション）

    '    '---------------------------
    '    '2014/04/11 武 ここから
    '    '---------------------------
    '    '権限判別
    '    Public Shared P_SESSION_AUTH As String = "権限区分"
    '    '---------------------------
    '    '2014/04/11 武 ここまで
    '    '---------------------------
    '#End Region

    '#Region "構造体・列挙体定義"
    '    '============================================================================================================================
    '    '=　構造体・列挙体定義
    '    '============================================================================================================================
    '    'ログアウトのモード
    '    Public Enum E_ログアウトモード As Short
    '        ログアウト = 1
    '        閉じる = 2
    '        非表示 = 3
    '    End Enum

    '    '半角全角指定
    '    Public Enum  ClsComVer.E_半角全角 As Short
    '        半角 = 0
    '        全角 = 1
    '    End Enum

    '    '日付テキスト形式
    '    Public Enum  ClsComVer.E_日付形式 As Short
    '        年月日 = 0
    '        年月 = 1
    '    End Enum

    '    '照会画面マルチビューIndex
    '    Public Enum E_照会マルチビュー As Short
    '        非表示 = -1
    '        一覧表示 = 0
    '        詳細表示 = 1
    '    End Enum

    '    'メッセージタイプ
    '    Public Enum clscomver.E_Mタイプ As Short
    '        情報 = 0
    '        警告 = 1
    '        エラー = 2
    '    End Enum

    '    'メッセージモード
    '    Public Enum clscomver. As Short
    '        OK = 0
    '        OKCancel = 1
    '    End Enum

    '    'IMEモード
    '    Public Enum  ClsComVer.E_IMEモード As Short
    '        全角 = 0
    '        半角_変更可 = 1
    '        半角_変更不可 = 2
    '    End Enum

    '    '遷移条件
    '    Public Enum E_遷移条件 As Short
    '        参照 = 1
    '        更新 = 2
    '        登録 = 3
    '        仮登録 = 4
    '    End Enum

    '    '工事種別の判定桁数
    '    Public Enum  ClsComVer.E_工事種別 As Short
    '        新規 = 1
    '        増設 = 2
    '        再配置 = 3
    '        移設 = 4
    '        一部撤去 = 5
    '        全撤去 = 6
    '        一時撤去 = 7
    '        機種変更 = 8
    '        構成配信 = 9
    '        その他 = 10
    '    End Enum

    '    'ドロップダウンリストの表示切替
    '    Public Enum E_リスト表示名 As Short
    '        設無 = -1
    '        名称 = 1
    '        略称 = 2
    '    End Enum

    '    'Script実行タイミング
    '    Public Enum clscomver.E_S実行 As Short
    '        描画前 = 1
    '        描画後 = 2
    '    End Enum

    '    'バリデーションのメッセージタイプ
    '    Public Enum clscomver.E_VMタイプ As Short
    '        メッセージ = 0
    '        ショート = 1
    '        アスタ = 2
    '    End Enum

    '    '文字の表示位置
    '    Public Enum  ClsComVer.E_文字位置 As Short
    '        右 = 0
    '        左 = 1
    '        中央 = 2
    '    End Enum

    '    'ソートフラグ
    '    Public Enum E_ソートフラグ As Short
    '        オン = 0
    '        オフ = Nothing
    '    End Enum
    '#End Region

    '#Region "変数定義"
    '    '============================================================================================================================
    '    '=　変数定義
    '    '============================================================================================================================
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    Public Shared USER_NAME As String
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsExc As New ClsCMExclusive

    '#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Me.UseReporting(Sub(settings)
                            settings.UseFileStore(New DirectoryInfo("レポートを含むディレクトリへのパスを指定してください"))
                            settings.UseCompression = True
                        End Sub)
        ' アプリケーションの起動時に呼び出されます
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 各要求の開始時に呼び出されます
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 使用の認証時に呼び出されます
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' エラーの発生時に呼び出されます
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' アプリケーションの終了時に呼び出されます
    End Sub

    ''' <summary>
    ''' セッション終了時
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Session_OnEnd()
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand
        Dim strAddress As String = Nothing
        Dim strName As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        Dim objStack As New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'セッションＩＤのフォルダが存在する場合削除する
        '作業領域取得
        If pfGetPreservePlace("1031FT", strAddress, strName) <> 0 Then
            '取得失敗
            Exit Sub
        End If

        'セッションＩＤのフォルダ削除
        If pfDeleteFold(strAddress, strName, Session.SessionID) <> 0 Then
            '削除失敗
            Exit Sub
        End If

        'セッションＩＤの接続情報を削除する
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPDEL001", conDB)
                With cmdDB
                    'コマンドタイプ設定(ストアド)
                    .CommandType = CommandType.StoredProcedure
                    'パラメータ設定
                    .Parameters.Add(pfSet_Param("sessionid", SqlDbType.VarChar, Session.SessionID))
                End With

                'データ削除
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    cmdDB.ExecuteNonQuery()

                    'コミット
                    conTrn.Commit()
                End Using

                '--------------------------------
                '2014/06/10 後藤　ここから
                '--------------------------------
                If Not Session(P_SESSION_SESSTION_ID) = Nothing Then

                    '排他削除
                    If clsExc.pfDel_Exclusive(Session(P_SESSION_SESSTION_ID)) <> 0 Then

                    End If

                End If
                '--------------------------------
                '2014/06/10 後藤　ここまで
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
                Return
            Finally
                'DB切断
                clsDataConnect.pfOpen_Database(conDB)
            End Try
        End If

    End Sub

#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
