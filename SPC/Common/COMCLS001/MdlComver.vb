'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'MdlComver-001     2017/02/06      加賀       CRS管理区分用定数追加
'MdlComver-002     2017/03/28      加賀       ユーザーの管理者判定用定数追加
'MdlComver-003     2017/04/11      加賀       正常ログの出力先を修正



'Imports clscomver
Imports System.IO

Module MdlComver

    '業務フォルダ
    Public Const P_COM As String = "Common"             '共通
    Public Const P_CNS As String = "Construction"       '工事
    Public Const P_MAI As String = "Maintenance"        '保守
    Public Const P_WAT As String = "Watch"              '監視
    Public Const P_FLR As String = "FaultReception"     '故障受付
    Public Const P_RPE As String = "RepairEequipment"   '修理・設備
    Public Const P_SPC As String = "SupportCenter"      'サポートセンタ
    Public Const P_MST As String = "Master"             'マスタ
    Public Const P_SCL As String = "Schedule"           'スケジュール管理

    '機能ID
    Public Const P_FUN_COM As String = "COM"    '共通
    Public Const P_FUN_CNS As String = "CNS"    '工事
    Public Const P_FUN_DLL As String = "DLL"    '構成配信・DLL変更
    Public Const P_FUN_MST As String = "MST"    'マスタ依頼書管理
    Public Const P_FUN_SER As String = "SER"    'シリアル管理
    Public Const P_FUN_REQ As String = "REQ"    '保守受付
    Public Const P_FUN_PRO As String = "PRO"    '保守作業進捗
    Public Const P_FUN_CMP As String = "CMP"    '保守完了
    Public Const P_FUN_EQU As String = "EQU"    '保守機器管理
    Public Const P_FUN_QUA As String = "QUA"    '品質会議資料
    Public Const P_FUN_BPI As String = "BPI"    '玉単価設定情報
    Public Const P_FUN_CTI As String = "CTI"    'CTI電話受付
    Public Const P_FUN_WAT As String = "WAT"    '監視共通
    Public Const P_FUN_HEA As String = "HEA"    'ヘルスチェック
    Public Const P_FUN_OVE As String = "OVE"    '時間外調査
    Public Const P_FUN_SLF As String = "SLF"    '券売機自走調査
    Public Const P_FUN_ERR As String = "ERR"    'ノーマル集信エラー
    Public Const P_FUN_ICH As String = "ICH"    'ＩＣカード履歴
    Public Const P_FUN_DLC As String = "DLC"    'ＤＬＬ変更依頼
    Public Const P_FUN_DSU As String = "DSU"    'ＤＳＵ交換対応依頼書
    Public Const P_FUN_BRK As String = "BRK"    '故障受付
    Public Const P_FUN_PHN As String = "PHN"    '電話集計表
    Public Const P_FUN_REP As String = "REP"    '修理
    Public Const P_FUN_MNT As String = "MNT"    '整備
    Public Const P_FUN_BBP As String = "BBP"    'BB1吸い上げ
    Public Const P_FUN_CDP As String = "CDP"    '使用中カードDB吸上
    Public Const P_FUN_TBP As String = "TBP"    'TBOX吸上
    Public Const P_FUN_DOC As String = "DOC"    '検収書、請求書
    Public Const P_FUN_HIL As String = "HIL"    'ホール情報一覧
    Public Const P_FUN_SMT As String = "SMT"    'システムメンテナンス
    Public Const P_FUN_SCL As String = "SCL"    'スケジュール

    '画面機能ID
    Public Const P_SCR_LST As String = "LST"    '一覧
    Public Const P_SCR_FIX As String = "FIX"    '確定
    Public Const P_SCR_PRO As String = "PRO"    '仮登録
    Public Const P_SCR_MNG As String = "MNG"    'マスタ管理
    Public Const P_SCR_UPD As String = "UPD"    '更新(編集)
    Public Const P_SCR_SEL As String = "SEL"    '参照(詳細)
    Public Const P_SCR_INQ As String = "INQ"    '照会
    Public Const P_SCR_OUT As String = "OUT"    '帳票出力
    Public Const P_SCR_INP As String = "INP"    '入力
    Public Const P_SCR_MEN As String = "MEN"    'メニュー
    Public Const P_SCR_PRV As String = "PRV"    'プレビュー
    Public Const P_SCR_LGI As String = "LGI"    'ログイン

    Public Const P_PAGE As String = "P"         'PAGE
    Public Const P_PAGE_M As String = "M"       'マスタページ

    '場所
    Public Const P_ADD_SPC As String = "SPC"    'サポートセンタ
    Public Const P_ADD_NGC As String = "NGC"    'ＮＧＣ
    Public Const P_ADD_WKB As String = "WKB"    '作業拠点

    'IMEモードのCssClass
    Public Const P_CSS_ZEN As String = "ime-active"           '日本語入力
    Public Const P_CSS_HAN_COK As String = "ime-inactive"     '半角英数(変更可能)
    Public Const P_CSS_HAN_CNG As String = "ime-disabled"     '半角英数(変更不可)

    '汎用ボタン名
    Public Const P_BTN_NM_DEL As String = "削除"
    Public Const P_BTN_NM_CLE As String = "クリア"
    Public Const P_BTN_NM_ADD As String = "登録"
    Public Const P_BTN_NM_UPD As String = "更新"
    Public Const P_BTN_NM_PDF As String = "ＰＤＦ"
    Public Const P_BTN_NM_PRI As String = "印刷"
    Public Const P_BTN_NM_TRA As String = "送信"

    'ＰＤＦ、ＣＳＶ等の保存フォルダ名(~/P_WORK_NM/セッションＩＤ)
    Public Const P_WORK_NM = "WORK"

    'マスターページタイプ名
    Public Const P_SITE_T_NM As String = "site_master"
    Public Const P_REFE_T_NM As String = "reference_master"

    'セッション変数名
    Public Const P_SESSION_USERID As String = "ユーザＩＤ"
    Public Const P_SESSION_TERMS As String = "遷移条件"
    Public Const P_SESSION_BCLIST As String = "パンくず"
    Public Const P_SESSION_NGC_MEN As String = "ＮＧＣメニュー"    '0:メニュー名　1:値
    Public Const P_SESSION_OLDDISP As String = "遷移元ＩＤ"
    Public Const P_SESSION_PRV_REPORT As String = "レポート"
    Public Const P_SESSION_PRV_DATA As String = "データ"
    Public Const P_SESSION_PRV_NAME As String = "レポート名"
    Public Const P_SESSION_CRS_USE As String = "CRS使用制限"    'MdlComver-001
    Public Const P_SESSION_ADMIN As String = "管理者区分"    'MdlComver-002

    'セッション変数(排他制御処理)
    Public Const P_SESSION_EXCLUSIV_DATE As String = "登録年月日時刻"
    Public Const P_SESSION_IP As String = "端末情報"
    Public Const P_SESSION_PLACE As String = "場所"
    Public Const P_SESSION_LOGIN_DATE As String = "ログイン年月日"
    Public Const P_SESSION_SESSTION_ID As String = "セッションＩＤ"
    Public Const P_SESSION_GROUP_NUM As String = "グループ番号"

    Public Const P_KEY As String = "ＫＥＹ"    '一覧詳細のキー項目

    'ViewState名
    Public Const P_VIEW_SORT_COL As String = "ソート列"         'ViewStateのソート列キー
    Public Const P_VIEW_SORT_DIR As String = "ソート順"         'ViewStateのソート順キー
    Public Const P_VIEW_SORT_FLG As String = "ソートフラグ"     'ViewStateのソートフラグキー

    'ViewState値
    Public Const P_VIEW_SORT_FLG_ON As Short = 1                'ViewStateのソートフラグON

    '検証メッセージ
    Public Const P_VALMES_SMES As String = "ShortMessage"
    Public Const P_VALMES_MES As String = "Message"
    Public Const P_VALMES_AST As String = "***"

    'ＣＳＶ
    Public Const P_CSV_ENCODING As String = "SHIFT-JIS"         'エンコード
    Public Const P_CSV_DELIMITER As String = ","                '区切りカンマ
    Public Const P_CSV_QUOTE As String = """"                   '引用符（ダブルクォーテーション）

    '権限判別
    Public P_SESSION_AUTH As String = "権限区分"

    Public objStackFrame As StackFrame
    Public USER_NAME As String

    'ログ種別
    Public Enum E_LogCls As Short
        エラー = -1
        正常 = 0
    End Enum



    ''' <summary>
    ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtMode">メッセージのモード</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Public Function pfGet_OCClickMes(ByVal ipstrNo As String,
                                                      ByVal ipshtType As ClsComVer.E_Mタイプ,
                                                      ByVal ipshtMode As ClsComVer.E_Mモード) As String
        Dim strMes As String
        Dim strTitle As String

        'メッセージ取得
        strMes = pfGet_Mes(ipstrNo, ipshtType)

        strTitle = pfGet_MesType(ipshtType) & ipstrNo
        pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    End Function
    ''' <summary>
    ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtMode">メッセージのモード</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Public Function pfGet_OCClickMes(ByVal ipstrNo As String,
                                                      ByVal ipshtType As ClsComVer.E_Mタイプ,
                                                      ByVal ipshtMode As ClsComVer.E_Mモード,
                                                      ByVal ipstrPrm1 As String) As String
        Dim strMes As String
        Dim strTitle As String

        'メッセージ取得
        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1)

        strTitle = pfGet_MesType(ipshtType) & ipstrNo
        pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    End Function

    ''' <summary>
    ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtMode">メッセージのモード</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Public Function pfGet_OCClickMes(ByVal ipstrNo As String,
                                                      ByVal ipshtType As ClsComVer.E_Mタイプ,
                                                      ByVal ipshtMode As ClsComVer.E_Mモード,
                                                      ByVal ipstrPrm1 As String,
                                                      ByVal ipstrPrm2 As String) As String
        Dim strMes As String
        Dim strTitle As String

        'メッセージ取得
        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2)

        strTitle = pfGet_MesType(ipshtType) & ipstrNo
        pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    End Function

    ''' <summary>
    ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtMode">メッセージのモード</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Public Function pfGet_OCClickMes(ByVal ipstrNo As String,
                                                      ByVal ipshtType As ClsComVer.E_Mタイプ,
                                                      ByVal ipshtMode As ClsComVer.E_Mモード,
                                                      ByVal ipstrPrm1 As String,
                                                      ByVal ipstrPrm2 As String,
                                                      ByVal ipstrPrm3 As String) As String
        Dim strMes As String
        Dim strTitle As String

        'メッセージ取得
        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2, ipstrPrm3)

        strTitle = pfGet_MesType(ipshtType) & ipstrNo
        pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    End Function

    ''' <summary>
    ''' メッセージタイプの記号を取得する。
    ''' </summary>
    ''' <param name="ipshtType">メッセージタイプ</param>
    ''' <returns>メッセージタイプの記号</returns>
    ''' <remarks></remarks>
    Public Function pfGet_MesType(ByVal ipshtType As Object) As String
'    Public Shared Function pfGet_MesType(ByVal ipshtType As clscomver.E_Mタイプ) As String

        Select Case ipshtType
            Case ClsComVer.E_Mタイプ.エラー
                pfGet_MesType = "E"
            Case ClsComVer.E_Mタイプ.警告
                pfGet_MesType = "W"
            Case ClsComVer.E_Mタイプ.情報
                pfGet_MesType = "I"
            Case Else
                pfGet_MesType = String.Empty
        End Select
    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ)
        Dim strRtn As String = String.Empty
        pfGet_Mes = mfGet_MesXml(ipstrNo, ipshtType)
        'pfGet_Mes = strRtn.Replace(CChar(0x0D) & CChar(10), "\n").Replace(CChar(10), "\n").Replace(CChar(13), "\n")

    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ,
                                               ByVal ipstrPrm1 As String)
        Dim strRtn As String = String.Empty
        pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
                               ipstrPrm1)
        'pfGet_Mes = strRtn.Replace(CChar(13) & CChar(10), "\n").Replace(CChar(10), "\n").Replace(CChar(13), "\n")

    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ,
                                               ByVal ipstrPrm1 As String,
                                               ByVal ipstrPrm2 As String)
        Dim strRtn As String = String.Empty
        pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
                               ipstrPrm1, ipstrPrm2)
        'pfGet_Mes = strRtn.Replace(Chr(13) & Chr(10), "\n").Replace(Chr(10), "\n").Replace(Chr(13), "\n")

    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <returns>取得したメッセージ</returns>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ,
                                               ByVal ipstrPrm1 As String,
                                               ByVal ipstrPrm2 As String,
                                               ByVal ipstrPrm3 As String)

        Dim strRtn As String = String.Empty
        pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
                               ipstrPrm1, ipstrPrm2, ipstrPrm3)
        'pfGet_Mes = strRtn.Replace(Chr(13) & Chr(10), "\n").Replace(Chr(10), "\n").Replace(Chr(13), "\n")

    End Function

    ''' <summary>
    ''' Xmlファイルからメッセージを取得する
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <returns>取得したメッセージ</returns>
    ''' <remarks></remarks>
    Private Function mfGet_MesXml(ByVal ipstrNo As String, ByVal ipshtType As ClsComVer.E_Mタイプ) As String
        Dim dtsXml As DataSet = New DataSet()
        Dim dtrSelect() As DataRow
        Dim strType As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        strType = pfGet_MesType(ipshtType)

        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/Message.xml"))

            With dtsXml.Tables(0)
                dtrSelect = .Select("No = '" & ipstrNo & "' AND Type = '" & strType & "'")
                If dtrSelect.Length > 0 Then
                    mfGet_MesXml = dtrSelect(0).Item("Message").ToString
                Else
                    mfGet_MesXml = String.Empty
                End If

            End With
        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            mfGet_MesXml = String.Empty
        End Try

    End Function

    ''' <summary>
    ''' メッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrMes">表示するメッセージ</param>
    ''' <param name="ipstrTitle">表示するタイトル</param>
    ''' <param name="ipshtType">表示するタイプ</param>
    ''' <param name="ipshtMode">表示するモード</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Script(ByVal ipstrMes As String,
                                         ByVal ipstrTitle As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtMode As ClsComVer.E_Mモード) As String
        Dim strScript As String

        strScript = String.Empty
        Select Case ipshtType
            Case ClsComVer.E_Mタイプ.エラー
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        strScript = "vb_MsgCri_O"
                    Case ClsComVer.E_Mモード.OKCancel
                        strScript = "vb_MsgCri_OC"
                End Select
            Case ClsComVer.E_Mタイプ.警告
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        strScript = "vb_MsgExc_O"
                    Case ClsComVer.E_Mモード.OKCancel
                        strScript = "vb_MsgExc_OC"
                End Select
            Case ClsComVer.E_Mタイプ.情報
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        strScript = "vb_MsgInf_O"
                    Case ClsComVer.E_Mモード.OKCancel
                        strScript = "vb_MsgInf_OC"
                End Select
        End Select

        If strScript = String.Empty Then
            Return String.Empty
        End If

        mfGet_Script = strScript & "('" & ipstrMes & "','" & ipstrTitle & "');"
    End Function

    ''' <summary>
    ''' ログファイル出力
    ''' </summary>
    ''' <param name="strSessionID">セッションID</param>
    ''' <param name="strUserID">ログインID</param>
    ''' <param name="strClass">対象クラス</param>
    ''' <param name="strMethod">対象メソッド</param>
    ''' <param name="strMessageType">メッセージタイプ</param>
    ''' <param name="strMessage">メッセージ</param>
    ''' <param name="strLogType">ログタイプ</param>
    ''' <remarks></remarks>
    Public Sub psLogWrite(ByVal strSessionID As String,
                                  ByVal strUserID As String,
                                  Optional ByVal strClass As String = "",
                                  Optional ByVal strMethod As String = "",
                                  Optional ByVal strMessageType As String = "",
                                  Optional ByVal strMessage As String = "",
                                  Optional ByVal strLogType As String = "")

        Dim logDir As String = System.AppDomain.CurrentDomain.BaseDirectory & "Log"

        If strLogType = "Catch" Then
            logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & DateTime.Now.ToString("yyyyMM")
        Else
            logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & DateTime.Now.ToString("yyyyMM") & "\TRANS" 'MdlComver-003 
        End If

        'ログフォルダ名作成
        System.IO.Directory.CreateDirectory(logDir)

        'ログの書き込み処理（書き込み失敗時は0.25秒置きに1000回リトライ）
        For zz As Integer = 0 To 1000
            Try
                'ログファイルをロックしてオープン
                Using objFs As New FileStream(logDir + "\" + DateTime.Now.ToString("yyyyMMdd") + ".log",
                                              FileMode.Append, FileAccess.Write, FileShare.None)

                    Dim objBuff As New StringBuilder    'StringBuilderクラス

                    '日時
                    objBuff.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    'セッションID
                    If strLogType <> "Catch" Then
                        objBuff.Append(strSessionID)
                        objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    End If
                    'ログインID
                    objBuff.Append(strUserID)
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    '対象クラス
                    objBuff.Append(strClass)
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    '対象メソッド
                    objBuff.Append(strMethod)
                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    'メッセージタイプ
                    If strLogType <> "Catch" Then
                        objBuff.Append(strMessageType)
                        objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
                    End If
                    'メッセージ
                    objBuff.AppendLine(strMessage.Replace("\n", ""))

                    'ログ出力
                    objFs.Write(Encoding.GetEncoding("shift-jis").GetBytes(objBuff.ToString), 0,
                                Encoding.GetEncoding("shift-jis").GetByteCount(objBuff.ToString))

                    'ファイルクローズ
                    objFs.Close()

                End Using

                Exit Sub

            Catch ex As Exception
                System.Threading.Thread.Sleep(250)
            Finally
            End Try
        Next

    End Sub

    ''' <summary>
    ''' ログ出力
    ''' </summary>
    ''' <param name="LogCls">ログ種別</param>
    ''' <param name="strMessage">ログに出力するメッセージ</param>
    ''' <remarks></remarks>
    Public Sub psLogWrite(ByVal pgCall As Page, ByVal LogCls As E_LogCls, ByVal strMessage As String)

        Dim ExprtDir As String
        Dim ExprtFileNm As String
        Dim strLogHeader As String
        Dim strStackCall As String = String.Empty
        Dim dtmNow As DateTime
        Dim encLog As Encoding = Encoding.GetEncoding("shift-jis")

        '出力内容生成
        Try

            '現在日時を取得
            dtmNow = DateTime.Now

            'ログ出力先ディレクトリ設定
            ExprtDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & dtmNow.ToString("yyyyMM")

            'ログ出力ファイル名設定
            Select Case LogCls
                Case E_LogCls.エラー
                    ExprtFileNm = dtmNow.ToString("yyyyMMdd") + "_ERR.log"
                Case E_LogCls.正常
                    ExprtFileNm = dtmNow.ToString("yyyyMM") & "\" + dtmNow.ToString("yyyyMMdd") + ".log"
                Case Else
                    ExprtFileNm = dtmNow.ToString("yyyyMM") & "\" + dtmNow.ToString("yyyyMMdd") + ".log"
            End Select

            'ログフォルダ名作成
            System.IO.Directory.CreateDirectory(ExprtDir)

            '出力ヘッダ生成
            strLogHeader = Environment.NewLine

            '日時
            strLogHeader &= dtmNow.ToString("yyyy/MM/dd HH:mm:ss")
            strLogHeader &= Microsoft.VisualBasic.ControlChars.Tab

            'ログインID
            strLogHeader &= pgCall.User.Identity.Name
            strLogHeader &= Microsoft.VisualBasic.ControlChars.Tab

            '呼び出し元取得(4階層分)
            For i As Integer = 4 To 1 Step -1
                strStackCall &= " > "
                strStackCall &= New StackFrame(i).GetType.Name    'クラス
                strStackCall &= "."
                strStackCall &= New StackFrame(i).GetMethod.Name  '処理
            Next

            'セッションID
            strLogHeader &= pgCall.Session.SessionID
            strLogHeader &= Microsoft.VisualBasic.ControlChars.Tab

            'ヘッダ&メッセージ 結合
            strMessage = strLogHeader & Environment.NewLine & strMessage & Environment.NewLine

        Catch ex As Exception

            Exit Sub

        End Try

        '出力(書き込み失敗時は0.25秒置きに100回リトライ)
        For i As Integer = 0 To 100
            Try

                Using fsLog As New FileStream(ExprtDir + "\" + ExprtFileNm, FileMode.Append, FileAccess.Write, FileShare.None)

                    'ログ出力
                    fsLog.Write(encLog.GetBytes(strMessage), 0, encLog.GetByteCount(strMessage))

                    'ファイルクローズ
                    fsLog.Close()

                End Using

            Catch e As IO.IOException

                '0.25sec sleep
                System.Threading.Thread.Sleep(250)

            Catch e As ObjectDisposedException

                '0.25sec sleep
                System.Threading.Thread.Sleep(250)

            Catch e As NotSupportedException

                Exit For

            Catch ex As Exception

                Exit For

            End Try
        Next

    End Sub

    ''' <summary>
    ''' 半角・全角チェック
    ''' チェックモードの文字のみで構成されているかを確認する。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <param name="ipshtMode">チェックモード(0:半角 1:全角)</param>
    ''' <returns>True：OK, False：NG</returns>
    ''' <remarks></remarks>
    Public Function pfCheck_HanZen(ByVal ipstrData As String, ByVal ipshtMode As ClsComVer.E_半角全角) As Boolean
        Dim shtWordByte As Short    '1文字のByte数
        Dim intString As Integer    'チェックする文字列のByte数
        Select Case ipshtMode
            Case ClsComVer.E_半角全角.半角
                shtWordByte = 1
            Case ClsComVer.E_半角全角.全角
                shtWordByte = 2
        End Select

        'Byte数取得
        intString = Encoding.GetEncoding("Shift_JIS").GetByteCount(ipstrData)

        If (intString / shtWordByte) > ipstrData.Length Then
            pfCheck_HanZen = False
        Else
            pfCheck_HanZen = True
        End If

    End Function

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行,
                                         ByVal ipstrPrm1 As String)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行,
                                         ByVal ipstrPrm1 As String,
                                         ByVal ipstrPrm2 As String)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行,
                                         ByVal ipstrPrm1 As String,
                                         ByVal ipstrPrm2 As String,
                                         ByVal ipstrPrm3 As String)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2, ipstrPrm3)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示するScriptをHtmlに埋め込む。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrMes">表示するメッセージ</param>
    ''' <param name="ipstrMesID">メッセージID</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <remarks></remarks>
    Private Sub msSet_MesBox(ByVal ippagPage As Page,
                                    ByVal ipstrMes As String,
                                    ByVal ipstrMesID As String,
                                    ByVal ipshtType As ClsComVer.E_Mタイプ,
                                    ByVal ipshtRun As ClsComVer.E_S実行)
        Dim strLoad As String
        Dim strScript As String
        Dim objStack As StackFrame
        Dim strVBSKey As String
        Dim strSKey As String

        If ipshtType = ClsComVer.E_Mタイプ.エラー Then
            'ログファイルへ出力
            objStack = New StackFrame(2)   '呼出元情報
            psLogWrite(ippagPage.Session.SessionID, ippagPage.User.Identity.Name,
                       objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name,
                       "ERROR", ipstrMesID & ":" & ipstrMes)
        End If

        strScript = mfGet_Script(ipstrMes, ipstrMesID, ipshtType, ClsComVer.E_Mモード.OK)

        '使用していないスクリプトのキー設定
        strSKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "mes")
        Select Case ipshtRun
            Case ClsComVer.E_S実行.描画前
                strLoad = mfGet_MesScript(ipshtType, ClsComVer.E_Mモード.OK)
                '使用していないスクリプトのキー設定
                strVBSKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "vbmes")
                'ページ読込前は外部ファイルが読み込まれないためVBスクリプトを発行する
                ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strVBSKey, strLoad, False)
                ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strSKey, strScript, True)
            Case ClsComVer.E_S実行.描画後
                ippagPage.ClientScript.RegisterStartupScript(ippagPage.GetType, strSKey, strScript, True)
        End Select
    End Sub

    ''' <summary>
    ''' 使用していないスクリプトキーを取得
    ''' </summary>
    ''' <param name="ippagPage">埋め込むページ</param>
    ''' <param name="ipType">スクリプトの型</param>
    ''' <param name="ipstrKey">ベースとなるキー</param>
    ''' <returns>スクリプトキー</returns>
    ''' <remarks>ベースとなるキーに数値をつけて一意となるよう生成</remarks>
    Public Function pfGet_ScriptKey(ByVal ippagPage As Page,
                                           ByVal ipType As Type,
                                           ByVal ipstrKey As String) As String
        Dim intKeyNo = 0
        Dim strKey As String = ipstrKey

        '使用していないスクリプトのキー設定
        Do Until Not ippagPage.ClientScript.IsStartupScriptRegistered(ipType, strKey)
            strKey = ipstrKey & intKeyNo.ToString
            intKeyNo = intKeyNo + 1
        Loop

        Return strKey
    End Function

    ''' <summary>
    ''' メッセージボックスのVBScriptを取得する。
    ''' </summary>
    ''' <param name="ipshtType">表示するタイプ</param>
    ''' <param name="ipshtMode">表示するモード</param>
    ''' <returns>メッセージボックスを表示するVBスクリプト</returns>
    ''' <remarks>外部ファイル「popup.vbs」と同内容なため変更時はそちらも変更すること。</remarks>
    Private Function mfGet_MesScript(ByVal ipshtType As ClsComVer.E_Mタイプ,
                                            ByVal ipshtMode As ClsComVer.E_Mモード) As String
        Dim strScript As New StringBuilder

        'strScript.AppendLine("<script type=""text/VBScript"">")
        strScript.AppendLine("<script type=""text/javascript"">")
        Select Case ipshtType
            Case ClsComVer.E_Mタイプ.エラー
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        'strScript.AppendLine("Function vb_MsgCri_O(mes, mesno)")
                        'strScript.AppendLine("    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno)")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//エラーメッセージ_OK")
                        strScript.AppendLine("function vb_MsgCri_O(mes,mesno) {")
                        strScript.AppendLine("    alert(mesno + ""\n"" + mes);")
                        strScript.AppendLine("    //    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno);")
                        strScript.AppendLine("}")

                    Case ClsComVer.E_Mモード.OKCancel
                        'strScript.AppendLine("Function vb_MsgCri_OC(mes, mesno)")
                        'strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbCritical), mesno)")
                        'strScript.AppendLine("        Case vbOK")
                        'strScript.AppendLine("            vb_MsgCri_OC = True")
                        'strScript.AppendLine("        Case Else")
                        'strScript.AppendLine("            vb_MsgCri_OC = False")
                        'strScript.AppendLine("    End Select")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//エラーメッセージ_OKCancel")
                        strScript.AppendLine("function vb_MsgCri_OC(mes, mesno) {")
                        strScript.AppendLine("    Return confirm(mesno + ""\n"" + mes)")
                        strScript.AppendLine("}")
                        strScript.AppendLine("//End Function")

                End Select
            Case ClsComVer.E_Mタイプ.警告
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        'strScript.AppendLine("Function vb_MsgExc_O(mes, mesno)")
                        'strScript.AppendLine("    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbExclamation), mesno)")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//警告メッセージ_OK")
                        strScript.AppendLine("function vb_MsgExc_O(mes,mesno) {")
                        strScript.AppendLine("    alert(mesno + ""\n"" + mes);")
                        strScript.AppendLine("}")

                    Case ClsComVer.E_Mモード.OKCancel
                        'strScript.AppendLine("Function vb_MsgExc_OC(mes, mesno)")
                        'strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbExclamation), mesno)")
                        'strScript.AppendLine("        Case vbOK")
                        'strScript.AppendLine("            vb_MsgExc_OC = True")
                        'strScript.AppendLine("        Case Else")
                        'strScript.AppendLine("            vb_MsgExc_OC = False")
                        'strScript.AppendLine("    End Select")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//警告メッセージ_OKCancel")
                        strScript.AppendLine("function vb_MsgExc_OC(mes, mesno) {")
                        strScript.AppendLine("    Return confirm(mesno + ""\n"" + mes)")
                        strScript.AppendLine("}")
                        strScript.AppendLine("//End Function")
                End Select
            Case ClsComVer.E_Mタイプ.情報
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        'strScript.AppendLine("Function vb_MsgInf_O(mes, mesno)")
                        'strScript.AppendLine("    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbInformation), mesno)")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//情報メッセージ_OK")
                        strScript.AppendLine("function vb_MsgInf_O(mes,mesno) {")
                        strScript.AppendLine("    alert(mesno + ""\n"" + mes);")
                        strScript.AppendLine("}")
                    Case ClsComVer.E_Mモード.OKCancel
                        'strScript.AppendLine("Function vb_MsgInf_OC(mes, mesno)")
                        'strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOK + vbInformation), mesno)")
                        'strScript.AppendLine("        Case vbOK")
                        'strScript.AppendLine("            vb_MsgInf_OC = True")
                        'strScript.AppendLine("        Case Else")
                        'strScript.AppendLine("            vb_MsgInf_OC = False")
                        'strScript.AppendLine("    End Select")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//情報メッセージ_OKCancel")
                        strScript.AppendLine("function vb_MsgInf_OC(mes, mesno) {")
                        strScript.AppendLine("    Return confirm(mesno + ""\n"" + mes)")
                        strScript.AppendLine("}")
                        strScript.AppendLine("//End Function")
                End Select
        End Select
        strScript.AppendLine("</script>")

        If strScript.ToString = String.Empty Then
            Return String.Empty
        End If

        mfGet_MesScript = strScript.ToString
    End Function

    ''' <summary>
    ''' javascriptにて次ページを開く。
    ''' </summary>
    ''' <param name="ippagPage">元ページのページコントロール</param>
    ''' <param name="ipstrNextPath">次ページの仮想パス</param>
    ''' <remarks>メッセージボックス表示後遷移等に使用</remarks>
    Public Sub psNext_Page(ByVal ippagPage As Page, ByVal ipstrNextPath As String, ByVal ipshtRun As ClsComVer.E_S実行)
        Dim strKey As String = Nothing
        Dim strScript As String

        '使用していないスクリプトのキー設定
        strKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "nextpage")

        strScript = "location.replace(""" & VirtualPathUtility.ToAbsolute(ipstrNextPath) & """);"
        Select Case ipshtRun
            Case ClsComVer.E_S実行.描画前
                ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strKey, strScript, True)
            Case ClsComVer.E_S実行.描画後
                ippagPage.ClientScript.RegisterStartupScript(ippagPage.GetType, strKey, strScript, True)
        End Select
    End Sub

End Module
