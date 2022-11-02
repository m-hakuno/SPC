'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'ClsComVer-001     2017.02.06      加賀       CRS管理区分用定数追加


Public Class ClsComVer
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
    'ログアウトのモード
    Public Enum E_ログアウトモード As Short
        ログアウト = 1
        閉じる = 2
        非表示 = 3
    End Enum

    '半角全角指定
    Public Enum E_半角全角 As Short
        半角 = 0
        全角 = 1
    End Enum

    '日付テキスト形式
    Public Enum E_日付形式 As Short
        年月日 = 0
        年月 = 1
    End Enum

    '照会画面マルチビューIndex
    Public Enum E_照会マルチビュー As Short
        非表示 = -1
        一覧表示 = 0
        詳細表示 = 1
    End Enum

    'メッセージタイプ
    Public Enum E_Mタイプ As Short
        情報 = 0
        警告 = 1
        エラー = 2
    End Enum

    'メッセージモード
    Public Enum E_Mモード As Short
        OK = 0
        OKCancel = 1
    End Enum

    'IMEモード
    Public Enum E_IMEモード As Short
        全角 = 0
        半角_変更可 = 1
        半角_変更不可 = 2
    End Enum

    '遷移条件
    Public Enum E_遷移条件 As Short
        参照 = 1
        更新 = 2
        登録 = 3
        仮登録 = 4
    End Enum

    '工事種別の判定桁数
    Public Enum E_工事種別 As Short
        新規 = 1
        増設 = 2
        再配置 = 3
        移設 = 4
        一部撤去 = 5
        全撤去 = 6
        一時撤去 = 7
        機種変更 = 8
        構成配信 = 9
        その他 = 10
        ＶＵＰ = 11
    End Enum

    'ドロップダウンリストの表示切替
    Public Enum E_リスト表示名 As Short
        設無 = -1
        名称 = 1
        略称 = 2
    End Enum

    'Script実行タイミング
    Public Enum E_S実行 As Short
        描画前 = 1
        描画後 = 2
    End Enum

    'バリデーションのメッセージタイプ
    Public Enum E_VMタイプ As Short
        メッセージ = 0
        ショート = 1
        アスタ = 2
    End Enum

    '文字の表示位置
    Public Enum E_文字位置 As Short
        右 = 0
        左 = 1
        中央 = 2
    End Enum

    'ソートフラグ
    Public Enum E_ソートフラグ As Short
        オン = 0
        オフ = Nothing
    End Enum

    'ソートフラグ
    Public Enum E_CRS使用制限 As Short
        使用不可 = 0
        参照 = 1
        更新 = 2
    End Enum

End Class
