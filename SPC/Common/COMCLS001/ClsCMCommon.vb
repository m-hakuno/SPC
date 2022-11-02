'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　共通関数クラス
'*　ＰＧＭＩＤ：　ClsCMCommon
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.16　：　土岐
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'ClsCMCommon-001   2015/12/07      栗原　　　GridViewのソートイベント(msGrid_Sorting,msGrid_DataBound,pfGetSortDirection)にMst_Refページ追加
'ClsCMCommon-002   2016/03/30      栗原　　　GridViewのソートイベント(msGrid_Sorting,msGrid_DataBound,pfGetSortDirection)にreference_reverseページ追加
'ClsCMCommon-003   2017/06/09      伯野　　　一覧の構成要素としてテキストボックスとラベルの区分を追加
'ClsCMCommon-004   2017/07/05      伯野　　　一覧のラベル表示するため関数を追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data
''Imports SPC.Global_asax
Imports SPC.ClsCMDataConnect
Imports System.IO
Imports System.Web.Hosting
Imports DBFTP
Imports DBFTP.ClsDBFTP_Main
Imports DBFTP.ClsFTPCnfg
'Imports DBFTP.clsLogwrite
Imports DBFTP.ClsSQLSvrDB
Imports System.Xml
Imports System.Xml.Serialization

#End Region

Public Class ClsCMCommon
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
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    'Private Shared objStackFrame As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Private Shared clsDataConnect As New ClsCMDataConnect

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' GridViewのソートイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Shared Sub msGrid_Sorting(sender As Object, e As GridViewSortEventArgs)
        Dim dttData As DataTable = Nothing
        Dim dtcData As DataColumn = Nothing
        Dim dtrData As DataRow = Nothing
        Dim grvData As GridView = Nothing
        Dim strSort As String = String.Empty
        Dim mstMyMaster As Object = Nothing
        Dim strViewFlgKey As String = Nothing

        objStackFrame = New StackFrame

        'GridView取得
        grvData = TryCast(sender, GridView)

        If grvData Is Nothing Then
            Exit Sub
        End If

        'SiteMasterのページ取得
        Select Case grvData.Page.Master.GetType.Name
            Case P_SITE_T_NM
                mstMyMaster = grvData.Page.Master
            Case P_REFE_T_NM, "reference_reverse_master" 'ClsCMCommon-002
                mstMyMaster = grvData.Page.Master.Master

                'ClsCMCommon-001 
                'Case "master_mst_master", "master_site_master"
            Case "master_mst_master", "master_site_master", "master_mst_ref_master"
                'ClsCMCommon-001
                'マスタ管理画面
                mstMyMaster = grvData.Page.Master
        End Select

        If mstMyMaster Is Nothing Then
            Exit Sub
        End If

        'ソートフラグ設定
        strViewFlgKey = grvData.ID + P_VIEW_SORT_FLG
        mstMyMaster.psSet_ViewState(strViewFlgKey, P_VIEW_SORT_FLG_ON)

        Try
            dttData = New DataTable

            'GridView ⇒ DataTable変換
            dttData = pfParse_DataTable(grvData)

            'ソート順取得
            strSort = pfGetSortDirection(e.SortExpression, grvData.Page, grvData.ID)

            'ソート
            dttData.DefaultView.Sort = e.SortExpression & " " & strSort

            'バインド
            grvData.DataSource = dttData
            grvData.DataBind()
        Catch ex As Exception
            psMesBox(grvData.Page, "30007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
        Finally
            'ソートフラグ削除
            mstMyMaster.psSet_ViewState(strViewFlgKey, Nothing)
        End Try
    End Sub

    ''' <summary>
    ''' GridViewにDataBound時、ViewStateのソート順削除イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Shared Sub msGrid_DataBound(sender As Object, e As EventArgs)
        Dim grvData As GridView = Nothing
        Dim mstMyMaster As Object = Nothing
        Dim strViewFlgKey As String = Nothing
        Dim strViewColKey As String = Nothing
        Dim strViewDirKey As String = Nothing

        grvData = TryCast(sender, GridView)

        If grvData Is Nothing Then
            Exit Sub
        End If

        'SiteMasterのページ取得
        Select Case grvData.Page.Master.GetType.Name
            Case P_SITE_T_NM
                mstMyMaster = grvData.Page.Master
            Case P_REFE_T_NM, "reference_reverse_master" 'ClsCMCommon-002
                mstMyMaster = grvData.Page.Master.Master
                'ClsCMCommon-001 
            Case "master_mst_master", "master_site_master", "master_mst_ref_master"
                'Case "master_site_master"
                'ClsCMCommon-001 END
                'マスタ管理画面
                mstMyMaster = grvData.Page.Master
        End Select

        If mstMyMaster Is Nothing Then
            Exit Sub
        End If

        'ソートの場合は削除しない
        strViewFlgKey = grvData.ID + P_VIEW_SORT_FLG
        If mstMyMaster.pfGet_ViewState(strViewFlgKey) IsNot Nothing _
            AndAlso mstMyMaster.pfGet_ViewState(strViewFlgKey) = P_VIEW_SORT_FLG_ON Then
            Exit Sub
        End If

        strViewColKey = grvData.ID + P_VIEW_SORT_COL
        strViewDirKey = grvData.ID + P_VIEW_SORT_DIR

        'ViewStateのソート列、ソート順削除
        mstMyMaster.psSet_ViewState(strViewColKey, Nothing)
        mstMyMaster.psSet_ViewState(strViewDirKey, Nothing)

    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' ニューメリックチェック。
    ''' 数値以外の文字が含まれていないかを確認する。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <returns>True：OK, False：NG</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Num(ByVal ipstrData As String) As Boolean

        Return System.Text.RegularExpressions.Regex.IsMatch(ipstrData, "^[0-9]+$")

    End Function

    ''' <summary>
    ''' 最小・最大値チェック。
    ''' 範囲内に収まっているかを確認する。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <param name="ipintMax">最大値</param>
    ''' <param name="ipintMin">最小値</param>
    ''' <returns>True：OK, False：NG</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_MinMax(ByVal ipstrData As String,
                                          ByVal ipintMax As Integer,
                                          ByVal ipintMin As Integer) As Boolean
        If (Integer.Parse(ipstrData) < ipintMin) Or (Integer.Parse(ipstrData) > ipintMax) Then
            pfCheck_MinMax = False
        Else
            pfCheck_MinMax = True
        End If
    End Function

    ' ''' <summary>
    ' ''' 半角・全角チェック
    ' ''' チェックモードの文字のみで構成されているかを確認する。
    ' ''' </summary>
    ' ''' <param name="ipstrData">チェックする文字列</param>
    ' ''' <param name="ipshtMode">チェックモード(0:半角 1:全角)</param>
    ' ''' <returns>True：OK, False：NG</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function pfCheck_HanZen(ByVal ipstrData As String, ByVal ipshtMode As  ClsComVer.E_半角全角) As Boolean
    '    Dim shtWordByte As Short    '1文字のByte数
    '    Dim intString As Integer    'チェックする文字列のByte数
    '    Select Case ipshtMode
    '        Case  ClsComVer.E_半角全角.半角
    '            shtWordByte = 1
    '        Case  ClsComVer.E_半角全角.全角
    '            shtWordByte = 2
    '    End Select

    '    'Byte数取得
    '    intString = Encoding.GetEncoding("Shift_JIS").GetByteCount(ipstrData)

    '    If (intString / shtWordByte) > ipstrData.Length Then
    '        pfCheck_HanZen = False
    '    Else
    '        pfCheck_HanZen = True
    '    End If

    'End Function

    ''' <summary>
    ''' Byte数が超過していないか確認する。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <param name="ipshtMax">最大byte数</param>
    ''' <returns>True：OK, False：NG</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Byte(ByVal ipstrData As String, ByVal ipshtMax As Short) As Boolean
        If Encoding.GetEncoding("Shift_JIS").GetByteCount(ipstrData) > ipshtMax Then
            pfCheck_Byte = False
        Else
            pfCheck_Byte = True
        End If
    End Function

    ''' <summary>
    ''' 指定した正規表現に一致するか確認する。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <param name="ipstrPattem">チェックする正規表現</param>
    ''' <returns>True：OK, False：NG</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Pattem(ByVal ipstrData As String, ByVal ipstrPattem As String) As Boolean
        If Not Regex.IsMatch(ipstrData, ipstrPattem, RegexOptions.ECMAScript) Then
            pfCheck_Pattem = False
        Else
            pfCheck_Pattem = True
        End If
    End Function

    ''' <summary>
    ''' 数値以外の文字が含まれていないかを確認する。（記号は可） 
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列。</param>
    ''' <returns>指定した文字列が数値であれば True。それ以外は False。</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Num_Sym(ByVal ipstrData As String) As Boolean
        If Not Regex.IsMatch(ipstrData, "^([0-9]|[-+,*/])+$", RegexOptions.ECMAScript) Then
            pfCheck_Num_Sym = False
        Else
            pfCheck_Num_Sym = True
        End If
    End Function

    ''' <summary>
    ''' 英字チェック。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <returns>True：OK, False：NG</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_AlphabeticChara(ByVal ipstrData As String) As Boolean

        Return System.Text.RegularExpressions.Regex.IsMatch(ipstrData, "^[a-z]+$")

    End Function

    ''' <summary>
    ''' テキストボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrData">チェックするデータ</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <param name="ipblnNumF">数値チェックの要不要</param>
    ''' <param name="ipblnCheckHan">半角チェックの要不要</param>
    ''' <param name="ipblnLengthF">固定桁チェックの要不要</param>
    ''' <param name="ipintLength">桁チェックの桁数</param>
    ''' <param name="ipstrExpression">正規表現チェック(空の場合はチェックを行わない)</param>
    ''' <param name="ipblnAc">英字チェックの要不要</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_TxtErr(ByVal ipstrData As String,
                                          ByVal ipblnReqFieldVal As Boolean,
                                          ByVal ipblnNumF As Boolean,
                                          ByVal ipblnCheckHan As Boolean,
                                          ByVal ipblnLengthF As Boolean,
                                          ByVal ipintLength As Integer,
                                          ByVal ipstrExpression As String,
                                          ByVal ipblnAc As Boolean) As String

        pfCheck_TxtErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrData = String.Empty Then
                pfCheck_TxtErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrData <> String.Empty Then

            '桁数チェック
            If ipblnLengthF Then    '固定桁数チェック
                If ipstrData.Length <> ipintLength Then
                    pfCheck_TxtErr = "3001"
                    Exit Function
                End If
            Else                    '通常桁数チェック
                If ipstrData.Length > ipintLength Then
                    pfCheck_TxtErr = "3002"
                    Exit Function
                End If
            End If

            '数値チェック
            If ipblnNumF Then
                If Not pfCheck_Num(ipstrData) Then
                    pfCheck_TxtErr = "4002"
                    Exit Function
                End If
            End If

            '半角チェック
            If ipblnCheckHan Then
                If Not pfCheck_HanZen(ipstrData, ClsComVer.E_半角全角.半角) Then
                    pfCheck_TxtErr = "4003"
                    Exit Function
                End If
            End If

            'フォーマットチェック
            If ipstrExpression <> String.Empty Then
                If Not pfCheck_Pattem(ipstrData, ipstrExpression) Then
                    pfCheck_TxtErr = "4001"
                    Exit Function
                End If
            End If

            '英字チェック
            If ipblnAc Then
                If Not pfCheck_AlphabeticChara(ipstrData) Then
                    pfCheck_TxtErr = "4008"
                    Exit Function
                End If
            End If
        End If

    End Function

    ''' <summary>
    ''' 複数のテキストボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrFrom">チェックする範囲開始文字列</param>
    ''' <param name="ipstrTo">チェックする範囲終了文字列</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks>個別チェックは行わないため、別途行うこと。</remarks>
    Public Shared Function pfCheck_TxtFTErr(ByVal ipstrFrom As String,
                                            ByVal ipstrTo As String,
                                            ByVal ipblnReqFieldVal As Boolean) As String
        pfCheck_TxtFTErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrFrom = String.Empty Or ipstrTo = String.Empty Then
                pfCheck_TxtFTErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrFrom <> String.Empty And ipstrTo <> String.Empty Then
            '範囲チェック
            If ipstrFrom > ipstrTo Then
                pfCheck_TxtFTErr = "2001"
                Exit Function
            End If
        End If

    End Function

    ''' <summary>
    ''' 個別の日付ボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrData">チェックする文字列</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <param name="ipdafDateFormat">日付の形式</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_DateErr(ByVal ipstrData As String,
                                           ByVal ipblnReqFieldVal As Boolean,
                                           ByVal ipdafDateFormat As Object) As String
'                                           ByVal ipdafDateFormat As  ClsComVer.E_日付形式) As String
        Dim strData As String   'チェックする日付
        Dim dteVal As Date = Nothing
        Dim strMinDt As String = "1753/01/01"
        Dim strMaxDt As String = "9999/12/31"

        pfCheck_DateErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrData = String.Empty Then
                pfCheck_DateErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrData <> String.Empty Then
            '日付チェック
            Select Case ipdafDateFormat
                Case ClsComVer.E_日付形式.年月
                    '年月の場合、日付チェックを行うため"/01"を加えてチェックを行う
                    strData = ipstrData & "/01"
                Case Else
                    strData = ipstrData
            End Select
            If DateTime.TryParse(strData, Nothing) = False Or strData.Length <> 10 Then
                pfCheck_DateErr = "4001"
                Exit Function
            End If

            '境界値チェック
            If Date.TryParse(strData, dteVal) Then
                If DateTime.Parse(strMinDt) > dteVal And _
                    dteVal < DateTime.Parse(strMaxDt) Then
                    pfCheck_DateErr = "6002"
                    Exit Function
                End If
            End If

        End If

    End Function

    ''' <summary>
    ''' 複数の日付ボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrFrom">チェックする範囲開始文字列</param>
    ''' <param name="ipstrTo">チェックする範囲終了文字列</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_DateFTErr(ByVal ipstrFrom As String,
                                             ByVal ipstrTo As String,
                                             ByVal ipblnReqFieldVal As Boolean) As String

        pfCheck_DateFTErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrFrom = String.Empty Or ipstrTo = String.Empty Then
                pfCheck_DateFTErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrFrom <> String.Empty And ipstrTo <> String.Empty Then
            '範囲チェック
            If ipstrFrom > ipstrTo Then
                pfCheck_DateFTErr = "1001"
                Exit Function
            End If
        End If

    End Function

    ''' <summary>
    ''' 時間ボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrHour">チェックする時</param>
    ''' <param name="ipstrMin">チェックする分</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <param name="ipblnOver">24時以降の可否</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_TimeErr(ByVal ipstrHour As String,
                                           ByVal ipstrMin As String,
                                           ByVal ipblnReqFieldVal As Boolean,
                                           ByVal ipblnOver As Boolean) As String

        pfCheck_TimeErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrHour = String.Empty Or ipstrMin = String.Empty Then
                pfCheck_TimeErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrHour <> String.Empty Or ipstrMin <> String.Empty Then
            '時チェック
            If ipblnOver Then   '24時以降可(00～99時)
                If (Not pfCheck_Num(ipstrHour)) Or ipstrHour < "00" Or ipstrHour > "30" Or ipstrHour.Length < 2 Then
                    pfCheck_TimeErr = "4006"
                    Exit Function
                End If
            Else                '24時以降不可(00～23時)
                If (Not pfCheck_Num(ipstrHour)) Or ipstrHour < "00" Or ipstrHour > "23" Or ipstrHour.Length < 2 Then
                    pfCheck_TimeErr = "4005"
                    Exit Function
                End If

            End If
            '分チェック
            If (Not pfCheck_Num(ipstrMin)) Or ipstrMin < "00" Or ipstrMin > "59" Or ipstrMin.Length < 2 Then
                pfCheck_TimeErr = "4007"
                Exit Function
            End If

        End If

    End Function

    ''' <summary>
    ''' 複数の時間ボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrFrom">チェックする範囲開始文字列</param>
    ''' <param name="ipstrTo">チェックする範囲終了文字列</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_TimeFTErr(ByVal ipstrFrom As String,
                                             ByVal ipstrTo As String,
                                             ByVal ipblnReqFieldVal As Boolean) As String

        pfCheck_TimeFTErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrFrom = String.Empty Or ipstrTo = String.Empty Then
                pfCheck_TimeFTErr = "5001"
                Exit Function
            End If
        End If

        '入力がある項目のみチェックする
        If ipstrFrom <> String.Empty And ipstrTo <> String.Empty Then
            '範囲チェック
            If ipstrFrom > ipstrTo Then
                pfCheck_TimeFTErr = "1002"
                Exit Function
            End If
        End If

    End Function

    ''' <summary>
    ''' 日時ボックスの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrDate">チェックする日付</param>
    ''' <param name="ipstrHour">チェックする時</param>
    ''' <param name="ipstrMin">チェックする分</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_Date(ByVal ipstrDate As String,
                                        ByVal ipstrHour As String,
                                        ByVal ipstrMin As String) As String
        pfCheck_Date = String.Empty

        If ipstrDate <> String.Empty Or ipstrHour <> String.Empty Or ipstrMin <> String.Empty Then
            'いずれかの項目に入力有
            If ipstrDate = String.Empty Then
                '日付項目に入力無
                pfCheck_Date = "4012"
                Exit Function
            End If
            If ipstrHour = String.Empty Or ipstrMin = String.Empty Then
                '時刻項目に入力無
                pfCheck_Date = "4013"
                Exit Function
            End If
        End If
    End Function

    ''' <summary>
    ''' ドロップダウンリストの入力値をチェックする。
    ''' </summary>
    ''' <param name="ipstrSelectValue">チェックする選択値</param>
    ''' <param name="ipblnReqFieldVal">必須チェックの要不要</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_ListErr(ByVal ipstrSelectValue As String,
                                           ByVal ipblnReqFieldVal As Boolean) As String
        pfCheck_ListErr = String.Empty

        '未入力チェック
        If ipblnReqFieldVal Then
            If ipstrSelectValue = String.Empty Then
                pfCheck_ListErr = "5003"
                Exit Function
            End If
        End If
    End Function

    ''' <summary>
    ''' グリッドの入力値をチェックする。
    ''' </summary>
    ''' <param name="GrvList">チェックするデータセット</param>
    ''' <param name="Check_Cls">チェックモード(0:数字 1:半角 2:全角)</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_GrvListErr(ByVal GrvList As GridView,
                                           ByVal Check_Cls As String, _
                                           ByVal Start_Cell As Integer, _
                                           ByVal End_Cell As Integer) As String
        Dim errFlg As Integer = 0
        Dim grvHead As String
        pfCheck_GrvListErr = String.Empty

        For grvRow As Integer = 0 To GrvList.Rows.Count - 1
            For grvCell As Integer = Start_Cell To End_Cell
                grvHead = GrvList.Columns(grvCell).HeaderText.ToString()
                If pfCheck_Num(CType(GrvList.Rows(grvRow).FindControl(grvHead), TextBox).Text) = False Then
                    GrvList.Rows(grvRow).Cells(grvCell).BackColor = Drawing.Color.Red
                    errFlg = 1
                End If
            Next
        Next
        If errFlg <> 0 Then
            pfCheck_GrvListErr = "4001"
            Exit Function
        End If

    End Function

'CksCMCommon-004
    ''' <summary>
    ''' グリッドの入力値をチェックする。ラベル用
    ''' </summary>
    ''' <param name="GrvList">チェックするデータセット</param>
    ''' <param name="Check_Cls">チェックモード(0:数字 1:半角 2:全角)</param>
    ''' <returns>検証メッセージNo.(正常の場合は空白)</returns>
    ''' <remarks></remarks>
    Public Shared Function pfCheck_GrvListErrL(ByVal GrvList As GridView,
                                           ByVal Check_Cls As String, _
                                           ByVal Start_Cell As Integer, _
                                           ByVal End_Cell As Integer) As String
        Dim errFlg As Integer = 0
        Dim grvHead As String
        pfCheck_GrvListErrL = String.Empty

        For grvRow As Integer = 0 To GrvList.Rows.Count - 1
            For grvCell As Integer = Start_Cell To End_Cell
                grvHead = GrvList.Columns(grvCell).HeaderText.ToString()
                If pfCheck_Num(CType(GrvList.Rows(grvRow).FindControl(grvHead), Label).Text) = False Then
                    GrvList.Rows(grvRow).Cells(grvCell).BackColor = Drawing.Color.Red
                    errFlg = 1
                End If
            Next
        Next
        If errFlg <> 0 Then
            pfCheck_GrvListErrL = "4001"
            Exit Function
        End If

    End Function

    ''' <summary>
    ''' パンくずリストを編集して返す。
    ''' </summary>
    ''' <param name="ipstrBCList">受け取ったパンくずリスト</param>
    ''' <param name="ipstrDispNm">自画面名</param>
    ''' <returns>パンくずリスト</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_BCList(ByVal ipstrBCList As String, ByVal ipstrDispNm As String) As String
        Dim strRtn As String

        If ipstrBCList = String.Empty Then  '新規パンくずリスト
            strRtn = ipstrDispNm
        Else                                '既存のパンくずリスト
            strRtn = ipstrBCList & " ＞ " & ipstrDispNm
        End If

        pfGet_BCList = strRtn
    End Function

    ' ''' <summary>
    ' ''' メッセージを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">取得するメッセージNo</param>
    ' ''' <param name="ipshtType">取得するメッセージType</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_Mes(ByVal ipstrNo As String,
    '                                           ByVal ipshtType As clscomver.E_Mタイプ)
    '    Dim strRtn As String = String.Empty
    '    pfGet_Mes = mfGet_MesXml(ipstrNo, ipshtType)
    '    'pfGet_Mes = strRtn.Replace(CChar(0x0D) & CChar(10), "\n").Replace(CChar(10), "\n").Replace(CChar(13), "\n")

    'End Function

    ' ''' <summary>
    ' ''' メッセージを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">取得するメッセージNo</param>
    ' ''' <param name="ipshtType">取得するメッセージType</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_Mes(ByVal ipstrNo As String,
    '                                           ByVal ipshtType As clscomver.E_Mタイプ,
    '                                           ByVal ipstrPrm1 As String)
    '    Dim strRtn As String = String.Empty
    '    pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
    '                           ipstrPrm1)
    '    'pfGet_Mes = strRtn.Replace(CChar(13) & CChar(10), "\n").Replace(CChar(10), "\n").Replace(CChar(13), "\n")

    'End Function

    ' ''' <summary>
    ' ''' メッセージを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">取得するメッセージNo</param>
    ' ''' <param name="ipshtType">取得するメッセージType</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_Mes(ByVal ipstrNo As String,
    '                                           ByVal ipshtType As clscomver.E_Mタイプ,
    '                                           ByVal ipstrPrm1 As String,
    '                                           ByVal ipstrPrm2 As String)
    '    Dim strRtn As String = String.Empty
    '    pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
    '                           ipstrPrm1, ipstrPrm2)
    '    'pfGet_Mes = strRtn.Replace(Chr(13) & Chr(10), "\n").Replace(Chr(10), "\n").Replace(Chr(13), "\n")

    'End Function

    ' ''' <summary>
    ' ''' メッセージを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">取得するメッセージNo</param>
    ' ''' <param name="ipshtType">取得するメッセージType</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ' ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ' ''' <returns>取得したメッセージ</returns>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_Mes(ByVal ipstrNo As String,
    '                                           ByVal ipshtType As clscomver.E_Mタイプ,
    '                                           ByVal ipstrPrm1 As String,
    '                                           ByVal ipstrPrm2 As String,
    '                                           ByVal ipstrPrm3 As String)

    '    Dim strRtn As String = String.Empty
    '    pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
    '                           ipstrPrm1, ipstrPrm2, ipstrPrm3)
    '    'pfGet_Mes = strRtn.Replace(Chr(13) & Chr(10), "\n").Replace(Chr(10), "\n").Replace(Chr(13), "\n")

    'End Function

    ' ''' <summary>
    ' ''' メッセージを表示する。
    ' ''' </summary>
    ' ''' <param name="ippagPage">表示するページコントロール</param>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Sub psMesBox(ByVal ippagPage As Page,
    '                                     ByVal ipstrNo As String,
    '                                     ByVal ipshtType As clscomver.E_Mタイプ,
    '                                     ByVal ipshtRun As clscomver.E_S実行)
    '    Dim strMes As String
    '    Dim strMesID As String


    '    'メッセージID設定(メッセージタイプ & メッセージNo.)
    '    strMesID = pfGet_MesType(ipshtType) & ipstrNo

    '    strMes = pfGet_Mes(ipstrNo, ipshtType)
    '    msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    'End Sub

    ' ''' <summary>
    ' ''' メッセージを表示する。
    ' ''' </summary>
    ' ''' <param name="ippagPage">表示するページコントロール</param>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Sub psMesBox(ByVal ippagPage As Page,
    '                                     ByVal ipstrNo As String,
    '                                     ByVal ipshtType As clscomver.E_Mタイプ,
    '                                     ByVal ipshtRun As clscomver.E_S実行,
    '                                     ByVal ipstrPrm1 As String)
    '    Dim strMes As String
    '    Dim strMesID As String


    '    'メッセージID設定(メッセージタイプ & メッセージNo.)
    '    strMesID = pfGet_MesType(ipshtType) & ipstrNo

    '    strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1)
    '    msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    'End Sub

    ' ''' <summary>
    ' ''' メッセージを表示する。
    ' ''' </summary>
    ' ''' <param name="ippagPage">表示するページコントロール</param>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Sub psMesBox(ByVal ippagPage As Page,
    '                                     ByVal ipstrNo As String,
    '                                     ByVal ipshtType As clscomver.E_Mタイプ,
    '                                     ByVal ipshtRun As clscomver.E_S実行,
    '                                     ByVal ipstrPrm1 As String,
    '                                     ByVal ipstrPrm2 As String)
    '    Dim strMes As String
    '    Dim strMesID As String


    '    'メッセージID設定(メッセージタイプ & メッセージNo.)
    '    strMesID = pfGet_MesType(ipshtType) & ipstrNo

    '    strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2)
    '    msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    'End Sub

    ' ''' <summary>
    ' ''' メッセージを表示する。
    ' ''' </summary>
    ' ''' <param name="ippagPage">表示するページコントロール</param>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ' ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Sub psMesBox(ByVal ippagPage As Page,
    '                                     ByVal ipstrNo As String,
    '                                     ByVal ipshtType As clscomver.E_Mタイプ,
    '                                     ByVal ipshtRun As clscomver.E_S実行,
    '                                     ByVal ipstrPrm1 As String,
    '                                     ByVal ipstrPrm2 As String,
    '                                     ByVal ipstrPrm3 As String)
    '    Dim strMes As String
    '    Dim strMesID As String


    '    'メッセージID設定(メッセージタイプ & メッセージNo.)
    '    strMesID = pfGet_MesType(ipshtType) & ipstrNo

    '    strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2, ipstrPrm3)
    '    msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    'End Sub

    ' ''' <summary>
    ' ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtMode">メッセージのモード</param>
    ' ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ' ''' <remarks></remarks>
    'Public Overloads Function pfGet_OCClickMes(ByVal ipstrNo As String,
    '                                                  ByVal ipshtType As clscomver.E_Mタイプ,
    '                                                  ByVal ipshtMode As clscomver.) As String
    '    Dim strMes As String
    '    Dim strTitle As String

    '    'メッセージ取得
    '    strMes = pfGet_Mes(ipstrNo, ipshtType)

    '    strTitle = pfGet_MesType(ipshtType) & ipstrNo
    '    pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    'End Function

    ' ''' <summary>
    ' ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtMode">メッセージのモード</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_OCClickMes(ByVal ipstrNo As String,
    '                                                  ByVal ipshtType As clscomver.E_Mタイプ,
    '                                                  ByVal ipshtMode As clscomver.,
    '                                                  ByVal ipstrPrm1 As String) As String
    '    Dim strMes As String
    '    Dim strTitle As String

    '    'メッセージ取得
    '    strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1)

    '    strTitle = pfGet_MesType(ipshtType) & ipstrNo
    '    pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    'End Function

    ' ''' <summary>
    ' ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtMode">メッセージのモード</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ' ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_OCClickMes(ByVal ipstrNo As String,
    '                                                  ByVal ipshtType As clscomver.E_Mタイプ,
    '                                                  ByVal ipshtMode As clscomver.,
    '                                                  ByVal ipstrPrm1 As String,
    '                                                  ByVal ipstrPrm2 As String) As String
    '    Dim strMes As String
    '    Dim strTitle As String

    '    'メッセージ取得
    '    strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2)

    '    strTitle = pfGet_MesType(ipshtType) & ipstrNo
    '    pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    'End Function

    ' ''' <summary>
    ' ''' コントロールのOnClientClickに設定するメッセージボックスを表示するスクリプトを取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtMode">メッセージのモード</param>
    ' ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ' ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ' ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ' ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ' ''' <remarks></remarks>
    'Public Overloads Shared Function pfGet_OCClickMes(ByVal ipstrNo As String,
    '                                                  ByVal ipshtType As clscomver.E_Mタイプ,
    '                                                  ByVal ipshtMode As clscomver.,
    '                                                  ByVal ipstrPrm1 As String,
    '                                                  ByVal ipstrPrm2 As String,
    '                                                  ByVal ipstrPrm3 As String) As String
    '    Dim strMes As String
    '    Dim strTitle As String

    '    'メッセージ取得
    '    strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2, ipstrPrm3)

    '    strTitle = pfGet_MesType(ipshtType) & ipstrNo
    '    pfGet_OCClickMes = "return " & mfGet_Script(strMes, strTitle, ipshtType, ipshtMode)
    'End Function

    ''' <summary>
    ''' 検証メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <returns>ショートメッセージ、メッセージ</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfGet_ValMes(ByVal ipstrNo As String) As DataRow
        Dim dtrRtn As DataRow

        'Xmlよりデータ取得
        dtrRtn = mfGet_ValMesXml(ipstrNo)

        pfGet_ValMes = dtrRtn
    End Function

    ''' <summary>
    ''' 検証メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <returns>ショートメッセージ、メッセージ</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfGet_ValMes(ByVal ipstrNo As String,
                                                  ByVal ipstrPrm1 As String) As DataRow
        Dim dtrRtn As DataRow
        Dim strMes As String

        'Xmlよりデータ取得
        dtrRtn = mfGet_ValMesXml(ipstrNo)

        'パラメータ埋め込み
        strMes = String.Format(dtrRtn.Item(P_VALMES_MES).ToString, ipstrPrm1)
        dtrRtn.Item(P_VALMES_MES) = strMes
        pfGet_ValMes = dtrRtn
    End Function

    ''' <summary>
    ''' 検証メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <returns>ショートメッセージ、メッセージ</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfGet_ValMes(ByVal ipstrNo As String,
                                                  ByVal ipstrPrm1 As String,
                                                  ByVal ipstrPrm2 As String) As DataRow
        Dim dtrRtn As DataRow
        Dim strMes As String

        'Xmlよりデータ取得
        dtrRtn = mfGet_ValMesXml(ipstrNo)

        'パラメータ埋め込み
        strMes = String.Format(dtrRtn.Item(P_VALMES_MES).ToString, ipstrPrm1, ipstrPrm2)
        dtrRtn.Item(P_VALMES_MES) = strMes
        pfGet_ValMes = dtrRtn
    End Function

    ''' <summary>
    ''' 検証メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <returns>ショートメッセージ、メッセージ</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfGet_ValMes(ByVal ipstrNo As String,
                                                  ByVal ipstrPrm1 As String,
                                                  ByVal ipstrPrm2 As String,
                                                  ByVal ipstrPrm3 As String) As DataRow
        Dim dtrRtn As DataRow
        Dim strMes As String

        'Xmlよりデータ取得
        dtrRtn = mfGet_ValMesXml(ipstrNo)

        'パラメータ埋め込み
        strMes = String.Format(dtrRtn.Item(P_VALMES_MES).ToString, ipstrPrm1, ipstrPrm2, ipstrPrm3)
        dtrRtn.Item(P_VALMES_MES) = strMes
        pfGet_ValMes = dtrRtn
    End Function

    ' ''' <summary>
    ' ''' データベースから画面名を取得する。
    ' ''' </summary>
    ' ''' <param name="ipstrDispCD">画面ＩＤ(プログラムＩＤ)</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function pfGet_DispNm(ByVal ipstrDispCD As String) As String


    '    Dim conDB As SqlClient.SqlConnection
    '    Dim cmdDB As SqlClient.SqlCommand
    '    Dim dstOrders As New DataSet
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStackFrame = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------

    '    '初期化
    '    conDB = Nothing
    '    pfGet_DispNm = ""

    '    '接続
    '    If clsDataConnect.pfOpen_Database(conDB) Then

    '        Try
    '            cmdDB = New SqlClient.SqlCommand("ZCMPSEL001", conDB)
    '            With cmdDB
    '                'コマンドタイプ設定(ストアド)
    '                .CommandType = CommandType.StoredProcedure
    '                'パラメータ設定
    '                .Parameters.Add(pfSet_Param("dispcd", SqlDbType.VarChar, ipstrDispCD))
    '            End With

    '            'ユーザーマスタからデータ取得
    '            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

    '            If dstOrders.Tables(0).Rows.Count > 0 Then
    '                pfGet_DispNm = dstOrders.Tables(0).Rows(0).Item(0).ToString()
    '            End If

    '        Catch ex As Exception
    '            '--------------------------------
    '            '2014/04/14 星野　ここから
    '            '--------------------------------
    '            'ログ出力
    '            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
    '                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
    '            '--------------------------------
    '            '2014/04/14 星野　ここまで
    '            '--------------------------------
    '            pfGet_DispNm = ""
    '        Finally
    '            'DB切断
    '            clsDataConnect.pfClose_Database(conDB)
    '        End Try
    '    Else
    '        pfGet_DispNm = ""
    '    End If

    'End Function

    ''' <summary>
    ''' 保存場所マスタ（M78)からサーバアドレス、フォルダ名取得
    ''' </summary>
    ''' <param name="ipstrFileclassCD">ファイル種別コード</param>
    ''' <param name="opstrServerAddress">取得したサーバアドレス</param>
    ''' <param name="opstrFolderNM">取得したフォルダ名</param>
    ''' <returns>0：正常終了、その他：エラー</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGetPreservePlace(ByVal ipstrFileclassCD As String,
                                              ByRef opstrServerAddress As String,
                                              ByRef opstrFolderNM As String) As Integer
        Dim conDB As SqlClient.SqlConnection = Nothing
        Dim cmdDB As SqlClient.SqlCommand = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlClient.SqlCommand("ZCMPSEL009", conDB)
                With cmdDB
                    'パラメータ設定
                    .Parameters.Add(pfSet_Param("FILECLASS_CD", SqlDbType.VarChar, ipstrFileclassCD))
                    .Parameters.Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.VarChar, 100, ParameterDirection.Output))
                    .Parameters.Add(pfSet_Param("FOLDER_NM", SqlDbType.VarChar, 100, ParameterDirection.Output))
                End With

                'コマンドタイプ設定(ストアド)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.ExecuteNonQuery()

                opstrServerAddress = cmdDB.Parameters("SERVER_ADDRESS").Value.ToString
                opstrFolderNM = cmdDB.Parameters("FOLDER_NM").Value.ToString

                Return 0
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
                Return -1
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            Return -1
        End If
    End Function

    ''' <summary>
    ''' ダウンロードファイル（T07)にレコードを追加
    ''' </summary>
    ''' <param name="ipstrMngNo">管理番号</param>
    ''' <param name="ipstrFileCls">ファイル種別</param>
    ''' <param name="ipstrTitle">タイトル</param>
    ''' <param name="ipstrFileNM">ファイル名</param>
    ''' <param name="ipstrReportNM">帳票のタイトル（日付含む）</param>
    ''' <param name="ipstrServerAddress">サーバアドレス</param>
    ''' <param name="ipstrKeepFold">保管フォルダ</param>
    ''' <param name="ipdatCreateDT">ファイル名に付与した日時</param>
    ''' <param name="ipstrUsrID">ユーザＩＤ</param>
    ''' <returns>0：正常終了、その他：エラー</returns>
    ''' <remarks>エラー時指定先のファイル削除</remarks>
    Public Shared Function pfSetDwnldFile(ByVal ipstrMngNo As String,
                                          ByVal ipstrFileCls As String,
                                          ByVal ipstrTitle As String,
                                          ByVal ipstrFileNM As String,
                                          ByVal ipstrReportNM As String,
                                          ByVal ipstrServerAddress As String,
                                          ByVal ipstrKeepFold As String,
                                          ByVal ipdatCreateDT As DateTime,
                                          ByVal ipstrUsrID As String) As Integer
        Const M_MNG_NO = "0"        '管理番号未設定時の値
        Dim conDB As SqlClient.SqlConnection = Nothing
        Dim cmdDB As SqlClient.SqlCommand = Nothing
        Dim strMngNo As String
        Dim intRtn As Integer
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '管理番号がない場合は置き換え
        If ipstrMngNo = Nothing Then
            strMngNo = M_MNG_NO
        Else
            strMngNo = ipstrMngNo
        End If
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try
                cmdDB = New SqlClient.SqlCommand("ZCMPINS001", conDB)
                With cmdDB
                    'パラメータ設定
                    .Parameters.Add(pfSet_Param("MNG_NO", SqlDbType.VarChar, strMngNo))                     '管理番号
                    .Parameters.Add(pfSet_Param("FILE_CLS", SqlDbType.VarChar, ipstrFileCls))               'ファイル種別
                    .Parameters.Add(pfSet_Param("TITLE", SqlDbType.VarChar, ipstrTitle))                    'タイトル
                    .Parameters.Add(pfSet_Param("FILE_NM", SqlDbType.VarChar, ipstrFileNM))                 'ファイル名
                    .Parameters.Add(pfSet_Param("REPORT_NM", SqlDbType.VarChar, ipstrReportNM))             '帳票名
                    .Parameters.Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.VarChar, ipstrServerAddress))   'サーバアドレス
                    .Parameters.Add(pfSet_Param("KEEP_FOLD", SqlDbType.VarChar, ipstrKeepFold))             '保存先フォルダ
                    .Parameters.Add(pfSet_Param("CREATE_DT", SqlDbType.DateTime, ipdatCreateDT))            '作成日時
                    .Parameters.Add(pfSet_Param("INSERT_USR", SqlDbType.VarChar, ipstrUsrID))               'ユーザＩＤ
                    .Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                            '戻り値
                End With

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn = 0 Then  '正常終了
                        'コミット
                        conTrn.Commit()
                    Else
                        pfDeleteFile(ipstrServerAddress, ipstrKeepFold, ipstrFileNM)
                    End If
                End Using
                Return intRtn
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
                pfDeleteFile(ipstrServerAddress, ipstrKeepFold, ipstrFileNM)
                Return -1
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            pfDeleteFile(ipstrServerAddress, ipstrKeepFold, ipstrFileNM)
            Return -1
        End If
    End Function

    ''' <summary>
    ''' ファイル名を生成してＣＳＶファイルダウンロード
    ''' </summary>
    ''' <param name="ipstrName">帳票名（日本語）</param>
    ''' <param name="ipstrNo">管理番号</param>
    ''' <param name="ipdttDt">データテーブル</param>
    ''' <param name="ipblnheaderOutput">ヘッダー出力有無（True:出力する、Fales:出力しない）</param>
    ''' <param name="ippagPg">出力ページ</param>
    ''' <returns>0：正常終了、その他：エラー</returns>
    ''' <remarks></remarks>
    Public Shared Function pfDLCSV(ByVal ipstrName As String,
                                   ByVal ipstrNo As String,
                                   ByVal ipdttDt As DataTable,
                                   ByVal ipblnheaderOutput As Boolean,
                                   ByVal ippagPg As Page) As Integer
        Dim strFileNm As StringBuilder

        Try
            '出力ファイル名生成
            strFileNm = New StringBuilder
            strFileNm.Append(ipstrName)
            strFileNm.Append("_")
            If ipstrNo <> Nothing Then
                strFileNm.Append(ipstrNo)
                strFileNm.Append("_")
            End If
            strFileNm.Append(DateTime.Now.ToString("yyyyMMddHHmmss"))
            strFileNm.Append(".csv")

            Return pfDLCsvFile(strFileNm.ToString, ipdttDt, ipblnheaderOutput, ippagPg)

        Catch ex As Threading.ThreadAbortException
            Return 0
        Catch ex As Exception
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' 保存場所マスタ（M78）より保存先を取得しＰＤＦを出力（データテーブル）
    ''' </summary>
    ''' <param name="ipstrFileclassCD">ファイル種別コード</param>
    ''' <param name="ipstrName">帳票名（日本語）</param>
    ''' <param name="ipstrNo">管理番号</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipdttData">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="opstrServerAddress">取得したサーバアドレス</param>
    ''' <param name="opstrFolderNM">取得したフォルダ名</param>
    ''' <param name="opdatCreateDate">作成日時</param>
    ''' <param name="opstrFileNm">ファイル名</param>
    ''' <param name="ipstrsessionid">セッションＩＤ</param>
    ''' <param name="useUnderBar">ファイル名の日付部分のアンダーバー使用有無</param>
    ''' <returns>0：正常終了、その他：エラー</returns>
    ''' <remarks>作成後、ダウンロードファイル（T07)にレコードを追加すること</remarks>
    Public Overloads Shared Function pfPDF(ByVal ipstrFileclassCD As String,
                                           ByVal ipstrName As String,
                                           ByVal ipstrNo As String,
                                           ByVal ipobjRpt As Object,
                                           ByVal ipdttData As DataTable,
                                           ByRef opstrServerAddress As String,
                                           ByRef opstrFolderNM As String,
                                           ByRef opdatCreateDate As DateTime,
                                           ByRef opstrFileNm As String,
                                           ByVal ipstrsessionid As String,
                                           Optional ByVal useUnderBar As Boolean = False) As Integer
        Dim strPath As StringBuilder
        Dim strFileNm As StringBuilder
        Dim strFileNmPDF As String
        Dim intRtn As Integer
        Dim strServerAddress As String = Nothing
        Dim strFolderNM As String = Nothing
        Dim datCreateDate As DateTime
        Dim clsFTP_Svr As New ClsFTPCnfg
        Dim reader As XmlSerializer = New XmlSerializer(GetType(ClsFTPCnfg), New XmlRootAttribute("FTP_CONNECT"))
        Dim file As StreamReader = New StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "\CNFG\FTP_Cnfg.xml")
        clsFTP_Svr = CType(reader.Deserialize(file), ClsFTPCnfg)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "変数定義", "Catch")

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "出力先取得", "Catch")
        '出力先取得
        intRtn = pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM)
        If intRtn <> 0 Then
            Return intRtn
        End If
        opstrServerAddress = strServerAddress
        opstrFolderNM = strFolderNM

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "出力先パス生成", "Catch")
        '出力先パス生成
        strPath = New StringBuilder
        strPath.Append("\\")
        strPath.Append(strServerAddress)
        strPath.Append("\")
        strPath.Append(strFolderNM)

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "ファイル名の日付取得", "Catch")
        'ファイル名の日付取得
        datCreateDate = DateTime.Now
        opdatCreateDate = datCreateDate

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "出力ファイル名生成", "Catch")
        '出力ファイル名生成
        strFileNm = New StringBuilder
        strFileNm.Append(ipstrName)
        strFileNm.Append("_")
        If ipstrNo <> Nothing Then
            strFileNm.Append(ipstrNo)
            strFileNm.Append("_")
        End If
        If useUnderBar = True Then
            strFileNm.Append(datCreateDate.ToString("yyyyMMdd_HHmmss"))
        Else
            strFileNm.Append(datCreateDate.ToString("yyyyMMddHHmmss"))
        End If
        strFileNmPDF = strFileNm.ToString
        strFileNm.Append(".pdf")
        opstrFileNm = strFileNm.ToString

        strServerAddress = clsFTP_Svr.FTP_SERVER

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "ファイル出力", "Catch")
        'ファイル出力
        Try
            psCreate_PDF(ipobjRpt, ipdttData, strPath.ToString, strFileNmPDF, strServerAddress, strFolderNM, ipstrsessionid)
            Return 0
        Catch ex As Exception       'ファイル出力失敗
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            'ファイル削除
            pfDeleteFile(strServerAddress, strFolderNM, strFileNm.ToString)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' 保存場所マスタ（M78）より保存先を取得しＰＤＦを出力（データテーブル）
    ''' </summary>
    ''' <param name="ipstrFileclassCD">ファイル種別コード</param>
    ''' <param name="ipstrName">帳票名（日本語）</param>
    ''' <param name="ipstrNo">管理番号</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipdttData">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="opstrServerAddress">取得したサーバアドレス</param>
    ''' <param name="opstrFolderNM">取得したフォルダ名</param>
    ''' <param name="opdatCreateDate">作成日時</param>
    ''' <param name="opstrFileNm">ファイル名</param>
    ''' <returns>0：正常終了、その他：エラー</returns>
    ''' <remarks>作成後、ダウンロードファイル（T07)にレコードを追加すること</remarks>
    Public Overloads Shared Function pfPDF(ByVal ipstrFileclassCD As String, _
                                           ByVal ipstrName As String, _
                                           ByVal ipstrNo As String, _
                                           ByVal ipobjRpt As Object, _
                                           ByVal ipdttData As DataTable, _
                                           ByRef opstrServerAddress As String, _
                                           ByRef opstrFolderNM As String, _
                                           ByRef opdatCreateDate As DateTime, _
                                           ByRef opstrFileNm As String) As Integer
        Dim strPath As StringBuilder
        Dim strFileNm As StringBuilder
        Dim strFileNmPDF As String
        Dim intRtn As Integer
        Dim strServerAddress As String = Nothing
        Dim strFolderNM As String = Nothing
        Dim datCreateDate As DateTime
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '出力先取得
        intRtn = pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM)
        If intRtn <> 0 Then
            Return intRtn
        End If
        opstrServerAddress = strServerAddress
        opstrFolderNM = strFolderNM

        '出力先パス生成
        strPath = New StringBuilder
        strPath.Append("\\")
        strPath.Append(strServerAddress)
        strPath.Append("\")
        strPath.Append(strFolderNM)

        'ファイル名の日付取得
        datCreateDate = DateTime.Now
        opdatCreateDate = datCreateDate

        '出力ファイル名生成
        strFileNm = New StringBuilder
        strFileNm.Append(ipstrName)
        strFileNm.Append("_")
        If ipstrNo <> Nothing Then
            strFileNm.Append(ipstrNo)
            strFileNm.Append("_")
        End If
        strFileNm.Append(datCreateDate.ToString("yyyyMMddHHmmss"))
        strFileNmPDF = strFileNm.ToString
        strFileNm.Append(".pdf")
        opstrFileNm = strFileNm.ToString

        'ファイル出力
        Try
            psCreate_PDF(ipobjRpt, ipdttData, strPath.ToString, strFileNmPDF)
            Return 0
        Catch ex As Exception       'ファイル出力失敗
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            'ファイル削除
            pfDeleteFile(strServerAddress, strFolderNM, strFileNm.ToString)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' 保存場所マスタ（M78）より保存先を取得しＰＤＦを出力（ＳＱＬ）
    ''' </summary>
    ''' <param name="ipstrFileclassCD">ファイル種別コード</param>
    ''' <param name="ipstrName">帳票名（日本語）</param>
    ''' <param name="ipstrNo">管理番号</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipstrSql">ＰＤＦに使用するＳＱＬ</param>
    ''' <param name="opstrServerAddress">取得したサーバアドレス</param>
    ''' <param name="opstrFolderNM">取得したフォルダ名</param>
    ''' <param name="opdatCreateDate">作成日時</param>
    ''' <param name="opstrFileNm">ファイル名</param>
    ''' <returns>0：正常終了、その他：エラー</returns>
    ''' <remarks>作成後、ダウンロードファイル（T07)にレコードを追加すること</remarks>
    Public Overloads Shared Function pfPDF(ByVal ipstrFileclassCD As String,
                                           ByVal ipstrName As String,
                                           ByVal ipstrNo As String,
                                           ByVal ipobjRpt As Object,
                                           ByVal ipstrSql As String,
                                           ByRef opstrServerAddress As String,
                                           ByRef opstrFolderNM As String,
                                           ByRef opdatCreateDate As DateTime,
                                           ByRef opstrFileNm As String) As Integer
        Dim strPath As StringBuilder
        Dim strFileNm As StringBuilder
        Dim strFileNmPDF As String
        Dim intRtn As Integer
        Dim strServerAddress As String = Nothing
        Dim strFolderNM As String = Nothing
        Dim datCreateDate As DateTime
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '出力先取得
        intRtn = pfGetPreservePlace(ipstrFileclassCD, strServerAddress, strFolderNM)
        If intRtn <> 0 Then
            Return intRtn
        End If
        opstrServerAddress = strServerAddress
        opstrFolderNM = strFolderNM

        '出力先パス生成
        strPath = New StringBuilder
        strPath.Append("\\")
        strPath.Append(strServerAddress)
        strPath.Append("\")
        strPath.Append(strFolderNM)

        'ファイル名の日付取得
        datCreateDate = DateTime.Now
        opdatCreateDate = datCreateDate

        '出力ファイル名生成
        strFileNm = New StringBuilder
        strFileNm.Append(ipstrName)
        strFileNm.Append("_")
        If ipstrNo <> Nothing Then
            strFileNm.Append(ipstrNo)
            strFileNm.Append("_")
        End If
        strFileNm.Append(datCreateDate.ToString("yyyyMMddHHmmss"))
        strFileNmPDF = strFileNm.ToString
        strFileNm.Append(".pdf")
        opstrFileNm = strFileNm.ToString

        'ファイル出力
        Try
            psCreate_PDF(ipobjRpt, ipstrSql, strPath.ToString, strFileNmPDF)
            Return 0
        Catch ex As Exception       'ファイル出力失敗
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            'ファイル削除
            pfDeleteFile(strServerAddress, strFolderNM, strFileNm.ToString)
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' ファイルが存在したならば削除
    ''' </summary>
    ''' <param name="ipServerAddress">サードアドレス</param>
    ''' <param name="ipFold">保管フォルダ</param>
    ''' <param name="ipstrFileNm">ファイル名</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks>削除ファイル、保管フォルダが存在しない場合は正常終了</remarks>
    Public Shared Function pfDeleteFile(ByVal ipServerAddress As String,
                                        ByVal ipFold As String,
                                        ByVal ipstrFileNm As String) As Integer
        Dim strFilePath As StringBuilder
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'フルパス作成
            strFilePath = New StringBuilder
            strFilePath.Append("\\")
            strFilePath.Append(ipServerAddress)
            strFilePath.Append("\")
            strFilePath.Append(ipFold)
            strFilePath.Append("\")
            strFilePath.Append(ipstrFileNm)

            '存在チェック
            If File.Exists(strFilePath.ToString) Then
                File.Delete(strFilePath.ToString)
            End If

            Return 0
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
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' フォルダが存在したならば削除
    ''' </summary>
    ''' <param name="ipServerAddress">サードアドレス</param>
    ''' <param name="ipFold">保管フォルダ</param>
    ''' <param name="ipstrFoldNm">フォルダ名</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks>削除フォルダ、保管フォルダが存在しない場合は正常終了</remarks>
    Public Shared Function pfDeleteFold(ByVal ipServerAddress As String,
                                        ByVal ipFold As String,
                                        ByVal ipstrFoldNm As String) As Integer
        Dim strFilePath As StringBuilder
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'フルパス作成
            strFilePath = New StringBuilder
            strFilePath.Append("\\")
            strFilePath.Append(ipServerAddress)
            strFilePath.Append("\")
            strFilePath.Append(ipFold)
            strFilePath.Append("\")
            strFilePath.Append(ipstrFoldNm)

            '存在チェック
            If Directory.Exists(strFilePath.ToString) Then
                Directory.Delete(strFilePath.ToString, True)
            End If

            Return 0
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
            Return -1
        End Try
    End Function

    ''' <summary>
    ''' データセットよりCSVファイルを作成する。
    ''' </summary>
    ''' <param name="fileDir">ファイルパス（ディレクトリのみ）</param>
    ''' <param name="fileNm">ファイル名</param>
    ''' <param name="ds">データセット</param>
    ''' <param name="headerOutput">ヘッダー出力有無（True:出力する、Fales:出力しない）</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfCreateCsvFile(fileDir As String, fileNm As String, ds As DataSet, headerOutput As Boolean) As Integer

        Dim fileFullPath As String = String.Empty   'CSVファイル名（フルパス）
        Dim objSw As StreamWriter = Nothing         'StreamWriterクラス
        Dim objBuff As New StringBuilder            'StringBuilderクラス
        Dim zz As Integer = 0                       'カウンタ
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ファイルパスまたはファイル名が空の場合はエラー
            If fileDir = String.Empty OrElse fileNm = String.Empty Then
                Return -1
            End If

            'フォルダ名作成
            Directory.CreateDirectory(fileDir)

            'パスの最後に"\"がない場合は付加してファイル名を作成する
            If Not fileDir.EndsWith("\") Then
                fileFullPath = fileDir + "\" + fileNm
            Else
                fileFullPath = fileDir + fileNm
            End If

            'ファイルオープン
            objSw = New StreamWriter(fileFullPath, False, System.Text.Encoding.GetEncoding(P_CSV_ENCODING))

            'ヘッダー出力
            If headerOutput = True Then
                '１列目～最終列の１つ前
                For zz = 0 To (ds.Tables(0).Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
                objBuff.AppendLine(P_CSV_QUOTE)
            End If

            'データ出力
            For Each dr As DataRow In ds.Tables(0).Rows
                '１列目～最終列の１つ前
                For zz = 0 To (ds.Tables(0).Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                objBuff.AppendLine(P_CSV_QUOTE)
            Next
            objSw.Write(objBuff.ToString)

            'ファイルクローズ
            objSw.Close()

            Return 0

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
            Return -1

        End Try

    End Function

    ''' <summary>
    ''' データテーブルよりCSVファイルを作成する。
    ''' </summary>
    ''' <param name="fileDir">ファイルパス（ディレクトリのみ）</param>
    ''' <param name="fileNm">ファイル名</param>
    ''' <param name="dt">データテーブル</param>
    ''' <param name="headerOutput">ヘッダー出力有無（True:出力する、Fales:出力しない）</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfCreateCsvFile(fileDir As String, fileNm As String, dt As DataTable, headerOutput As Boolean) As Integer

        Dim fileFullPath As String = String.Empty   'CSVファイル名（フルパス）
        Dim objSw As StreamWriter = Nothing         'StreamWriterクラス
        Dim objBuff As New StringBuilder            'StringBuilderクラス
        Dim zz As Integer = 0                       'カウンタ
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ファイルパスまたはファイル名が空の場合はエラー
            If fileDir = String.Empty OrElse fileNm = String.Empty Then
                Return -1
            End If

            'フォルダ名作成
            Directory.CreateDirectory(fileDir)

            'パスの最後に"\"がない場合は付加してファイル名を作成する
            If Not fileDir.EndsWith("\") Then
                fileFullPath = fileDir + "\" + fileNm
            Else
                fileFullPath = fileDir + fileNm
            End If

            'ファイルオープン
            objSw = New StreamWriter(fileFullPath, False, System.Text.Encoding.GetEncoding(P_CSV_ENCODING))

            'ヘッダー出力
            If headerOutput = True Then
                '１列目～最終列の１つ前
                For zz = 0 To (dt.Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(dt.Columns(zz).ColumnName)
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(dt.Columns(zz).ColumnName)
                objBuff.AppendLine(P_CSV_QUOTE)
            End If

            'データ出力
            For Each dr As DataRow In dt.Rows
                '１列目～最終列の１つ前
                For zz = 0 To (dt.Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                objBuff.AppendLine(P_CSV_QUOTE)
            Next
            objSw.Write(objBuff.ToString)

            'ファイルクローズ
            objSw.Close()

            Return 0

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
            Return -1

        End Try

    End Function

    ''' <summary>
    ''' データセットよりCSVファイルを作成する。サポートセンタ検収書用
    ''' </summary>
    ''' <param name="fileDir">ファイルパス（ディレクトリのみ）</param>
    ''' <param name="fileNm">ファイル名</param>
    ''' <param name="ds">データセット</param>
    ''' <param name="headerOutput">ヘッダー出力有無（True:出力する、Fales:出力しない）</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfCreateCsvFileSC(fileDir As String, fileNm As String, ds As DataSet, headerOutput As Boolean) As Integer

        Dim fileFullPath As String = String.Empty   'CSVファイル名（フルパス）
        Dim objSw As StreamWriter = Nothing         'StreamWriterクラス
        Dim objBuff As New StringBuilder            'StringBuilderクラス
        Dim zz As Integer = 0                       'カウンタ
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ファイルパスまたはファイル名が空の場合はエラー
            If fileDir = String.Empty OrElse fileNm = String.Empty Then
                Return -1
            End If

            'フォルダ名作成
            Directory.CreateDirectory(fileDir)

            'パスの最後に"\"がない場合は付加してファイル名を作成する
            If Not fileDir.EndsWith("\") Then
                fileFullPath = fileDir + "\" + fileNm
            Else
                fileFullPath = fileDir + fileNm
            End If

            'ファイルオープン
            objSw = New StreamWriter(fileFullPath, False, System.Text.Encoding.GetEncoding(P_CSV_ENCODING))

            'ヘッダー出力
            If headerOutput = True Then
                '１列目～最終列の１つ前
                For zz = 0 To (ds.Tables(0).Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
'                    objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
                    If ds.Tables(0).Columns(zz).ColumnName.IndexOf("Column") >= 0 Then
                        objBuff.Append("")
                    Else
                        objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
                    End If
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(ds.Tables(0).Columns(zz).ColumnName)
                objBuff.AppendLine(P_CSV_QUOTE)
            End If

            'データ出力
            For Each dr As DataRow In ds.Tables(0).Rows
                '１列目～最終列の１つ前
                For zz = 0 To (ds.Tables(0).Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                objBuff.AppendLine(P_CSV_QUOTE)
            Next
            objSw.Write(objBuff.ToString)

            'ファイルクローズ
            objSw.Close()

            Return 0

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
            Return -1

        End Try

    End Function

    ''' <summary>
    ''' データテーブルよりCSVファイルを作成しダウンロードする。
    ''' </summary>
    ''' <param name="ipstrfileNm">ファイル名</param>
    ''' <param name="ipdttDt">データテーブル</param>
    ''' <param name="ipblnheaderOutput">ヘッダー出力有無（True:出力する、Fales:出力しない）</param>
    ''' <param name="ippagPg">出力ページ</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks></remarks>
    Public Shared Function pfDLCsvFile(ByVal ipstrfileNm As String,
                                       ByVal ipdttDt As DataTable,
                                       ByVal ipblnheaderOutput As Boolean,
                                       ByVal ippagPg As Page) As Integer

        Dim objBuff As New StringBuilder            'StringBuilderクラス
        Dim zz As Integer = 0                       'カウンタ
        'エンコードの指定
        Dim enc As System.Text.Encoding =
            System.Text.Encoding.GetEncoding(P_CSV_ENCODING)
        Dim encbom() As Byte = enc.GetPreamble
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ファイルパスまたはファイル名が空の場合はエラー
            If ipstrfileNm = String.Empty Then
                Return -1
            End If

            'ヘッダー出力
            If ipblnheaderOutput = True Then
                '１列目～最終列の１つ前
                For zz = 0 To (ipdttDt.Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(ipdttDt.Columns(zz).ColumnName)
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(ipdttDt.Columns(zz).ColumnName)
                objBuff.AppendLine(P_CSV_QUOTE)
            End If

            'データ出力
            For Each dr As DataRow In ipdttDt.Rows
                '１列目～最終列の１つ前
                For zz = 0 To (ipdttDt.Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                objBuff.AppendLine(P_CSV_QUOTE)
            Next

            'Response情報クリア
            ippagPg.Response.ClearContent()

            'ダウンロード用ファイルの名前を設定
            ippagPg.Response.AddHeader("Content-Disposition",
              "attachment;filename=" & HttpUtility.UrlEncode(ipstrfileNm))

            'ダウンロードデータとして設定
            ippagPg.Response.ContentType = "application/octet-stream"
            'ippagPg.Response.ContentType = "text/comma-separated-values"

            'バイナリーで出力
            'ippagPg.Response.BinaryWrite(encbom)    '文字コードがUnicodeの場合はBOMを付加する
            ippagPg.Response.BinaryWrite(enc.GetBytes(objBuff.ToString))

            'フラッシュ
            'ippagPg.Response.Flush()
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
            ippagPg.Response.End()

            Return 0

        Catch ex As Threading.ThreadAbortException
            Return 0

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
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' データテーブルよりCSVファイルを作成しダウンロードする。サポートセンタ検収書用
    ''' </summary>
    ''' <param name="ipstrfileNm">ファイル名</param>
    ''' <param name="ipdttDt">データテーブル</param>
    ''' <param name="ipblnheaderOutput">ヘッダー出力有無（True:出力する、Fales:出力しない）</param>
    ''' <param name="ippagPg">出力ページ</param>
    ''' <returns>0：正常終了、-1:エラー</returns>
    ''' <remarks></remarks>
    Public Shared Function pfDLCsvFileSC(ByVal ipstrfileNm As String,
                                       ByVal ipdttDt As DataTable,
                                       ByVal ipblnheaderOutput As Boolean,
                                       ByVal ippagPg As Page) As Integer

        Dim objBuff As New StringBuilder            'StringBuilderクラス
        Dim zz As Integer = 0                       'カウンタ
        'エンコードの指定
        Dim enc As System.Text.Encoding =
            System.Text.Encoding.GetEncoding(P_CSV_ENCODING)
        Dim encbom() As Byte = enc.GetPreamble
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ファイルパスまたはファイル名が空の場合はエラー
            If ipstrfileNm = String.Empty Then
                Return -1
            End If

            'ヘッダー出力
            If ipblnheaderOutput = True Then
                '１列目～最終列の１つ前
                For zz = 0 To (ipdttDt.Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    If ipdttDt.Columns(zz).ColumnName.IndexOf("Column") >= 0 Then
                        objBuff.Append("")
                    Else
                        objBuff.Append(ipdttDt.Columns(zz).ColumnName)
                    End If
'                    objBuff.Append(ipdttDt.Columns(zz).ColumnName)
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(ipdttDt.Columns(zz).ColumnName)
                objBuff.AppendLine(P_CSV_QUOTE)
            End If

            'データ出力
            For Each dr As DataRow In ipdttDt.Rows
                '１列目～最終列の１つ前
                For zz = 0 To (ipdttDt.Columns.Count) - 2
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                    objBuff.Append(P_CSV_QUOTE)
                    objBuff.Append(P_CSV_DELIMITER)
                Next
                '最終列
                objBuff.Append(P_CSV_QUOTE)
                objBuff.Append(dr(zz).ToString.Replace(Environment.NewLine, "")) '※改行コードはカット
                objBuff.AppendLine(P_CSV_QUOTE)
            Next

            'Response情報クリア
            ippagPg.Response.ClearContent()

            'ダウンロード用ファイルの名前を設定
            ippagPg.Response.AddHeader("Content-Disposition",
              "attachment;filename=" & HttpUtility.UrlEncode(ipstrfileNm))

            'ダウンロードデータとして設定
            ippagPg.Response.ContentType = "application/octet-stream"
            'ippagPg.Response.ContentType = "text/comma-separated-values"

            'バイナリーで出力
            'ippagPg.Response.BinaryWrite(encbom)    '文字コードがUnicodeの場合はBOMを付加する
            ippagPg.Response.BinaryWrite(enc.GetBytes(objBuff.ToString))

            'フラッシュ
            'ippagPg.Response.Flush()
            'HttpContext.Current.ApplicationInstance.CompleteRequest()
            ippagPg.Response.End()

            Return 0

        Catch ex As Threading.ThreadAbortException
            Return 0

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
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' GridViewの設定をXmlファイルから読み込み設定
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
    ''' <returns>OK:True, NG:False</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_GridView(ByVal ipgrvGridView As GridView, _
                                                    ByVal ipstrXmlName As String, _
                                                    Optional ByVal ipintHeight As Integer = 0, _
                                                    Optional ByVal ipintFontSize As Integer = 0) As Boolean

        Dim dtsXml As DataSet = New DataSet()
        Dim dblWidth As Double
        Dim dblTextWidth As Double
        Dim dblSumWidth As Double
        Dim blnReadOnly As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '列を自動生成しない
        ipgrvGridView.AutoGenerateColumns = False

        '0件データはヘッダのみ表示
        ipgrvGridView.ShowHeaderWhenEmpty = True

        'レイアウトの設定
        ipgrvGridView.CssClass = "grid"
        ipgrvGridView.HeaderStyle.CssClass = "grid-head"
        ipgrvGridView.RowStyle.CssClass = "grid-row1"
        ipgrvGridView.AlternatingRowStyle.CssClass = "grid-row2"
        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/" & ipstrXmlName & ".xml"))
            '--------------------------------
            '2015/01/08 加賀　ここから
            '--------------------------------
            'マスタ管理画面のXMLパス指定
            'If ipstrXmlName.Substring(0, 7) = "COMUPDM" Then
            '    dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/Master/XML/" & ipstrXmlName & ".xml"))
            'End If
            '--------------------------------
            '2015/01/08 加賀　ここまで
            '--------------------------------

            Dim dtrColumns As DataRow

            dblSumWidth = 0
            With dtsXml.Tables(0)
                For Each dtrColumns In .Rows
                    Dim objNewColumn As Object

                    '横幅
                    If Not DBNull.Value.Equals(dtrColumns.Item("Width")) Then
                        If Not Double.TryParse(dtrColumns.Item("Width").ToString, dblWidth) Then
                            dblWidth = 100
                        End If
                    Else
                        dblWidth = 100
                    End If

                    If Not DBNull.Value.Equals(dtrColumns.Item("FieldType")) Then
                        Select Case dtrColumns.Item("FieldType").ToString
                            Case "Button"
                                'ボタンフィールド
                                objNewColumn = New ButtonField

                                'ボタンテキスト
                                If Not DBNull.Value.Equals(dtrColumns.Item("text")) Then
                                    objNewColumn.Text = dtrColumns.Item("text").ToString

                                End If
                                '検証グループ名
                                If Not DBNull.Value.Equals(dtrColumns.Item("ValidationGroup")) Then
                                    objNewColumn.ValidationGroup = dtrColumns.Item("ValidationGroup").ToString
                                End If

                                'コマンド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("CommandName")) Then
                                    objNewColumn.CommandName = dtrColumns.Item("CommandName").ToString
                                End If

                                'ボタンタイプ
                                If Not DBNull.Value.Equals(dtrColumns.Item("ButtonType")) Then
                                    Select Case dtrColumns.Item("ButtonType").ToString
                                        Case "Button"
                                            objNewColumn.ButtonType = ButtonType.Button
                                        Case "Image"
                                            objNewColumn.ButtonType = ButtonType.Image
                                        Case "Link"
                                            objNewColumn.ButtonType = ButtonType.Link
                                    End Select
                                End If

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If
                                'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                '        Case "Top"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                '        Case "Middle"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                '        Case "Bottom"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                '    End Select
                                'End If

                                '横幅計
                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                If ipintHeight.ToString <> "0" Then
                                    objNewColumn.ControlStyle.Height = Unit.Parse(ipintHeight.ToString)
                                End If
                            Case "Bound"
                                'バインドフィールド
                                objNewColumn = New TemplateField
                                Dim tmpColItem As ClsCMTextColumn
                                Dim strDataField As String
                                Dim typType As Type = Nothing
                                Dim strFormat As String = String.Empty

                                'バインド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                    strDataField = dtrColumns.Item("DataField")
                                    Select Case strDataField.Substring(0, 1)
                                        Case "N"
                                            strDataField = strDataField.Substring(1)
                                            typType = Type.GetType("System.Decimal")
                                            strFormat = "#0.####"
                                        Case "M"
                                            strDataField = strDataField.Substring(1)
                                            typType = Type.GetType("System.Decimal")
                                            strFormat = "#,##0.####"
                                    End Select
                                    tmpColItem = New ClsCMTextColumn(strDataField)
                                    tmpColItem.ppDataField = strDataField
                                    If typType IsNot Nothing Then
                                        tmpColItem.ppType = typType
                                        tmpColItem.ppFormat = strFormat
                                    End If
                                    objNewColumn.SortExpression = strDataField
                                Else
                                    pfSet_GridView = False
                                    Exit Function
                                End If

                                '幅指定
                                dblTextWidth = dblWidth - 6
                                If dblTextWidth < 0 Then
                                    dblTextWidth = 0
                                End If
                                tmpColItem.ppWidth = Unit.Parse(dblTextWidth.ToString)
                                tmpColItem.ppHeight = Unit.Parse(ipintHeight.ToString)
                                tmpColItem.ppFontSize = FontUnit.Parse(ipintFontSize.ToString)

                                '書込可否
                                If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                    If dtrColumns.Item("ReadOnly") = "1" Then
                                        blnReadOnly = True
                                    Else
                                        blnReadOnly = False
                                    End If
                                    tmpColItem.ppReadOnly = blnReadOnly
                                End If

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    tmpColItem.ppHorizontalAlign = dtrColumns.Item("HorizontalAlign").ToString
                                End If
                                objNewColumn.ItemTemplate = tmpColItem

                                'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                '        Case "Top"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                '        Case "Middle"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                '        Case "Bottom"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                '    End Select
                                'End If

                                '横幅計
                                If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                    If dtrColumns.Item("Visible").ToString = "1" Then
                                        dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                    Else        '列非表示
                                        objNewColumn.Visible = False
                                    End If
                                End If
                            Case "CheckBox"
                                'チェックボックス
                                objNewColumn = New TemplateField
                                Dim tmpColItem As New ClsCMCheckColumn(dtrColumns.Item("DataField"))

                                'バインド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                    tmpColItem.ppDataField = dtrColumns.Item("DataField")
                                    objNewColumn.SortExpression = dtrColumns.Item("DataField")
                                Else
                                    pfSet_GridView = False
                                    Exit Function
                                End If

                                '書込可否
                                If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                    If dtrColumns.Item("ReadOnly") = "1" Then
                                        blnReadOnly = True
                                    Else
                                        blnReadOnly = False
                                    End If
                                    tmpColItem.ppReadOnly = blnReadOnly
                                End If

                                objNewColumn.ItemTemplate = tmpColItem

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If

                                '横幅計
                                If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                    If dtrColumns.Item("Visible").ToString = "1" Then
                                        dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                    Else        '列非表示
                                        objNewColumn.Visible = False
                                    End If
                                End If
                            Case Else
                                pfSet_GridView = False
                                Exit Function
                        End Select

                        '横幅
                        objNewColumn.ItemStyle.Width = Unit.Parse(dblWidth.ToString)
                        objNewColumn.ItemStyle.Height = Unit.Parse(ipintHeight.ToString)
                        objNewColumn.HeaderStyle.Width = Unit.Parse(dblWidth.ToString)

                    Else
                        pfSet_GridView = False
                        Exit Function
                    End If

                    '共通設定
                    'ヘッダ名
                    If Not DBNull.Value.Equals(dtrColumns.Item("HeaderText")) Then
                        objNewColumn.HeaderText = dtrColumns.Item("HeaderText")
                    End If

                    '列を追加
                    ipgrvGridView.Columns.Add(objNewColumn)
                Next
            End With

            '列合計幅をテーブルのサイズに設定
            dblSumWidth = dblSumWidth + 1
            ipgrvGridView.HeaderStyle.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Height = Unit.Parse(ipintHeight.ToString)
            'ソート設定
            ipgrvGridView.AllowSorting = True
            AddHandler ipgrvGridView.Sorting, AddressOf msGrid_Sorting
            AddHandler ipgrvGridView.DataBound, AddressOf msGrid_DataBound

            pfSet_GridView = True
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
            pfSet_GridView = False
        End Try

    End Function

    ''' <summary>
    ''' GridViewの設定をXmlファイルから読み込み設定
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
    ''' <param name="ipstrTLCls">テキストボックス表示：T／ラベル表示：L</param>
    ''' <returns>OK:True, NG:False</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_GridView(ByVal ipgrvGridView As GridView, _
                                                    ByVal ipstrXmlName As String, _
                                                    ByVal ipstrTLCls As String, _
                                                    Optional ByVal ipintHeight As Integer = 0, _
                                                    Optional ByVal ipintFontSize As Integer = 0) As Boolean

        Dim dtsXml As DataSet = New DataSet()
        Dim dblWidth As Double
        Dim dblTextWidth As Double
        Dim dblSumWidth As Double
        Dim blnReadOnly As Boolean
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '列を自動生成しない
        ipgrvGridView.AutoGenerateColumns = False

        '0件データはヘッダのみ表示
        ipgrvGridView.ShowHeaderWhenEmpty = True

        'レイアウトの設定
        ipgrvGridView.CssClass = "grid"
        ipgrvGridView.HeaderStyle.CssClass = "grid-head"
        ipgrvGridView.RowStyle.CssClass = "grid-row1"
        ipgrvGridView.AlternatingRowStyle.CssClass = "grid-row2"
        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/" & ipstrXmlName & ".xml"))
            '--------------------------------
            '2015/01/08 加賀　ここから
            '--------------------------------
            'マスタ管理画面のXMLパス指定
            'If ipstrXmlName.Substring(0, 7) = "COMUPDM" Then
            '    dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/Master/XML/" & ipstrXmlName & ".xml"))
            'End If
            '--------------------------------
            '2015/01/08 加賀　ここまで
            '--------------------------------

            Dim dtrColumns As DataRow

            dblSumWidth = 0
            With dtsXml.Tables(0)
                For Each dtrColumns In .Rows
                    Dim objNewColumn As Object

                    '横幅
                    If Not DBNull.Value.Equals(dtrColumns.Item("Width")) Then
                        If Not Double.TryParse(dtrColumns.Item("Width").ToString, dblWidth) Then
                            dblWidth = 100
                        End If
                    Else
                        dblWidth = 100
                    End If

                    If Not DBNull.Value.Equals(dtrColumns.Item("FieldType")) Then
                        Select Case dtrColumns.Item("FieldType").ToString
                            Case "Button"
                                'ボタンフィールド
                                objNewColumn = New ButtonField

                                'ボタンテキスト
                                If Not DBNull.Value.Equals(dtrColumns.Item("text")) Then
                                    objNewColumn.Text = dtrColumns.Item("text").ToString

                                End If
                                '検証グループ名
                                If Not DBNull.Value.Equals(dtrColumns.Item("ValidationGroup")) Then
                                    objNewColumn.ValidationGroup = dtrColumns.Item("ValidationGroup").ToString
                                End If

                                'コマンド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("CommandName")) Then
                                    objNewColumn.CommandName = dtrColumns.Item("CommandName").ToString
                                End If

                                'ボタンタイプ
                                If Not DBNull.Value.Equals(dtrColumns.Item("ButtonType")) Then
                                    Select Case dtrColumns.Item("ButtonType").ToString
                                        Case "Button"
                                            objNewColumn.ButtonType = ButtonType.Button
                                        Case "Image"
                                            objNewColumn.ButtonType = ButtonType.Image
                                        Case "Link"
                                            objNewColumn.ButtonType = ButtonType.Link
                                    End Select
                                End If

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If
                                'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                '        Case "Top"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                '        Case "Middle"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                '        Case "Bottom"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                '    End Select
                                'End If

                                '横幅計
                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                If ipintHeight.ToString <> "0" Then
                                    objNewColumn.ControlStyle.Height = Unit.Parse(ipintHeight.ToString)
                                End If
                            Case "Bound"
                                'バインドフィールド
                                objNewColumn = New TemplateField
                                Select Case ipstrTLCls
                                    Case "T"
                                        'バインドフィールド
'                                        objNewColumn = New TemplateField
                                        Dim tmpColItem As ClsCMTextColumn
                                        Dim strDataField As String
                                        Dim typType As Type = Nothing
                                        Dim strFormat As String = String.Empty

                                        'バインド名
                                        If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                            strDataField = dtrColumns.Item("DataField")
                                            Select Case strDataField.Substring(0, 1)
                                                Case "N"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#0.####"
                                                Case "M"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#,##0.####"
                                            End Select
                                            tmpColItem = New ClsCMTextColumn(strDataField)
                                            tmpColItem.ppDataField = strDataField
                                            If typType IsNot Nothing Then
                                                tmpColItem.ppType = typType
                                                tmpColItem.ppFormat = strFormat
                                            End If
                                            objNewColumn.SortExpression = strDataField
                                        Else
                                            pfSet_GridView = False
                                            Exit Function
                                        End If

                                        '幅指定
                                        dblTextWidth = dblWidth - 6
                                        If dblTextWidth < 0 Then
                                            dblTextWidth = 0
                                        End If
                                        tmpColItem.ppWidth = Unit.Parse(dblTextWidth.ToString)
                                        tmpColItem.ppHeight = Unit.Parse(ipintHeight.ToString)
                                        tmpColItem.ppFontSize = FontUnit.Parse(ipintFontSize.ToString)

                                        '書込可否
                                        If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                            If dtrColumns.Item("ReadOnly") = "1" Then
                                                blnReadOnly = True
                                            Else
                                                blnReadOnly = False
                                            End If
                                            tmpColItem.ppReadOnly = blnReadOnly
                                        End If

                                        '表示位置
                                        If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                            tmpColItem.ppHorizontalAlign = dtrColumns.Item("HorizontalAlign").ToString
                                        End If
                                        objNewColumn.ItemTemplate = tmpColItem

                                        'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                        '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                        '        Case "Top"
                                        '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                        '        Case "Middle"
                                        '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                        '        Case "Bottom"
                                        '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                        '    End Select
                                        'End If

                                        '横幅計
                                        If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                            If dtrColumns.Item("Visible").ToString = "1" Then
                                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                            Else        '列非表示
                                                objNewColumn.Visible = False
                                            End If
                                        End If

                                    Case "L"
                                        'バインドフィールド
'                                        objNewColumn = New TemplateField
                                        Dim tmpColItem As ClsCMLabelColumn
                                        Dim strDataField As String
                                        Dim typType As Type = Nothing
                                        Dim strFormat As String = String.Empty

                                        'バインド名
                                        If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                            strDataField = dtrColumns.Item("DataField")
                                            Select Case strDataField.Substring(0, 1)
                                                Case "N"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#0.####"
                                                Case "M"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#,##0.####"
                                            End Select
                                            tmpColItem = New ClsCMLabelColumn(strDataField)
                                            tmpColItem.ppDataField = strDataField
                                            If typType IsNot Nothing Then
                                                tmpColItem.ppType = typType
                                                tmpColItem.ppFormat = strFormat
                                            End If
                                            objNewColumn.SortExpression = strDataField
                                        Else
                                            pfSet_GridView = False
                                            Exit Function
                                        End If

                                        '幅指定
                                        dblTextWidth = dblWidth - 6
                                        If dblTextWidth < 0 Then
                                            dblTextWidth = 0
                                        End If
                                        tmpColItem.ppWidth = Unit.Parse(dblTextWidth.ToString)
                                        tmpColItem.ppHeight = Unit.Parse(ipintHeight.ToString)
                                        tmpColItem.ppFontSize = FontUnit.Parse(ipintFontSize.ToString)

                                        '表示位置
                                        If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                            tmpColItem.ppHorizontalAlign = dtrColumns.Item("HorizontalAlign").ToString
                                        End If
                                        objNewColumn.ItemTemplate = tmpColItem

                                        '横幅計
                                        If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                            If dtrColumns.Item("Visible").ToString = "1" Then
                                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                            Else        '列非表示
                                                objNewColumn.Visible = False
                                            End If
                                        End If
                                End Select
                            Case "CheckBox"
                                'チェックボックス
                                objNewColumn = New TemplateField
                                Dim tmpColItem As New ClsCMCheckColumn(dtrColumns.Item("DataField"))

                                'バインド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                    tmpColItem.ppDataField = dtrColumns.Item("DataField")
                                    objNewColumn.SortExpression = dtrColumns.Item("DataField")
                                Else
                                    pfSet_GridView = False
                                    Exit Function
                                End If

                                '書込可否
                                If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                    If dtrColumns.Item("ReadOnly") = "1" Then
                                        blnReadOnly = True
                                    Else
                                        blnReadOnly = False
                                    End If
                                    tmpColItem.ppReadOnly = blnReadOnly
                                End If

                                objNewColumn.ItemTemplate = tmpColItem

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If

                                '横幅計
                                If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                    If dtrColumns.Item("Visible").ToString = "1" Then
                                        dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                    Else        '列非表示
                                        objNewColumn.Visible = False
                                    End If
                                End If
                            Case Else
                                pfSet_GridView = False
                                Exit Function
                        End Select

                        '横幅
                        objNewColumn.ItemStyle.Width = Unit.Parse(dblWidth.ToString)
                        objNewColumn.ItemStyle.Height = Unit.Parse(ipintHeight.ToString)
                        objNewColumn.HeaderStyle.Width = Unit.Parse(dblWidth.ToString)

                    Else
                        pfSet_GridView = False
                        Exit Function
                    End If

                    '共通設定
                    'ヘッダ名
                    If Not DBNull.Value.Equals(dtrColumns.Item("HeaderText")) Then
                        objNewColumn.HeaderText = dtrColumns.Item("HeaderText")
                    End If

                    '列を追加
                    ipgrvGridView.Columns.Add(objNewColumn)
                Next
            End With

            '列合計幅をテーブルのサイズに設定
            dblSumWidth = dblSumWidth + 1
            ipgrvGridView.HeaderStyle.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Height = Unit.Parse(ipintHeight.ToString)
            'ソート設定
            ipgrvGridView.AllowSorting = True
            AddHandler ipgrvGridView.Sorting, AddressOf msGrid_Sorting
            AddHandler ipgrvGridView.DataBound, AddressOf msGrid_DataBound

            pfSet_GridView = True
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
            pfSet_GridView = False
        End Try

    End Function

    ''' <summary>
    ''' GridViewの設定をXmlファイルから読み込み設定(マスタのみ使用)
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
    ''' <returns>OK:True, NG:False</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_GridView(ByVal ipgrvGridView As GridView, _
                                                    ByVal ipstrXmlName As String, _
                                                    ByVal ipintHeader As Integer(), _
                                                    ByVal ipintColumn As Integer(), _
                                                    Optional ByVal ipintHeight As Integer = 0, _
                                                    Optional ByVal ipintFontSize As Integer = 0) As Boolean

        Dim dtsXml As DataSet = New DataSet()
        Dim dblWidth As Double
        Dim dblTextWidth As Double
        Dim dblSumWidth As Double
        Dim blnReadOnly As Boolean
        Dim intHeaderWidth As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '列を自動生成しない
        ipgrvGridView.AutoGenerateColumns = False

        '0件データはヘッダのみ表示
        ipgrvGridView.ShowHeaderWhenEmpty = True

        'レイアウトの設定
        ipgrvGridView.CssClass = "grid"
        ipgrvGridView.HeaderStyle.CssClass = "grid-head"
        ipgrvGridView.RowStyle.CssClass = "grid-row1"
        ipgrvGridView.AlternatingRowStyle.CssClass = "grid-row2"
        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/" & ipstrXmlName & ".xml"))
            '--------------------------------
            '2015/01/08 加賀　ここから
            '--------------------------------
            'マスタ管理画面のXMLパス指定
            'If ipstrXmlName.Substring(0, 7) = "COMUPDM" Then
            '    dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/Master/XML/" & ipstrXmlName & ".xml"))
            'End If
            '--------------------------------
            '2015/01/08 加賀　ここまで
            '--------------------------------

            Dim dtrColumns As DataRow
            Dim intColumncount As Integer = 0
            dblSumWidth = 0
            With dtsXml.Tables(0)
                For Each dtrColumns In .Rows
                    Dim objNewColumn As Object

                    '横幅
                    If Not DBNull.Value.Equals(dtrColumns.Item("Width")) Then
                        If Not Double.TryParse(dtrColumns.Item("Width").ToString, dblWidth) Then
                            dblWidth = 100
                        End If
                    Else
                        dblWidth = 100
                    End If

                    If Not DBNull.Value.Equals(dtrColumns.Item("FieldType")) Then
                        Select Case dtrColumns.Item("FieldType").ToString
                            Case "Button"
                                'ボタンフィールド
                                objNewColumn = New ButtonField

                                'ボタンテキスト
                                If Not DBNull.Value.Equals(dtrColumns.Item("text")) Then
                                    objNewColumn.Text = dtrColumns.Item("text").ToString

                                End If
                                '検証グループ名
                                If Not DBNull.Value.Equals(dtrColumns.Item("ValidationGroup")) Then
                                    objNewColumn.ValidationGroup = dtrColumns.Item("ValidationGroup").ToString
                                End If

                                'コマンド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("CommandName")) Then
                                    objNewColumn.CommandName = dtrColumns.Item("CommandName").ToString
                                End If

                                'ボタンタイプ
                                If Not DBNull.Value.Equals(dtrColumns.Item("ButtonType")) Then
                                    Select Case dtrColumns.Item("ButtonType").ToString
                                        Case "Button"
                                            objNewColumn.ButtonType = ButtonType.Button
                                        Case "Image"
                                            objNewColumn.ButtonType = ButtonType.Image
                                        Case "Link"
                                            objNewColumn.ButtonType = ButtonType.Link
                                    End Select
                                End If

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If
                                'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                '        Case "Top"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                '        Case "Middle"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                '        Case "Bottom"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                '    End Select
                                'End If

                                '横幅計
                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                If ipintHeight.ToString <> "0" Then
                                    objNewColumn.ControlStyle.Height = Unit.Parse(ipintHeight.ToString)
                                End If
                            Case "Bound"
                                'バインドフィールド
                                objNewColumn = New TemplateField
                                Dim tmpColItem As ClsCMTextColumn
                                Dim strDataField As String
                                Dim typType As Type = Nothing
                                Dim strFormat As String = String.Empty

                                'バインド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                    strDataField = dtrColumns.Item("DataField")
                                    Select Case strDataField.Substring(0, 1)
                                        Case "N"
                                            strDataField = strDataField.Substring(1)
                                            typType = Type.GetType("System.Decimal")
                                            strFormat = "#0.####"
                                        Case "M"
                                            strDataField = strDataField.Substring(1)
                                            typType = Type.GetType("System.Decimal")
                                            strFormat = "#,##0.####"
                                    End Select
                                    tmpColItem = New ClsCMTextColumn(strDataField)
                                    tmpColItem.ppDataField = strDataField
                                    If typType IsNot Nothing Then
                                        tmpColItem.ppType = typType
                                        tmpColItem.ppFormat = strFormat
                                    End If
                                    objNewColumn.SortExpression = strDataField
                                Else
                                    pfSet_GridView = False
                                    Exit Function
                                End If

                                '幅指定
                                dblTextWidth = dblWidth - 6
                                If dblTextWidth < 0 Then
                                    dblTextWidth = 0
                                End If
                                tmpColItem.ppWidth = Unit.Parse(dblTextWidth.ToString)
                                tmpColItem.ppHeight = Unit.Parse(ipintHeight.ToString)
                                tmpColItem.ppFontSize = FontUnit.Parse(ipintFontSize.ToString)

                                '書込可否
                                If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                    If dtrColumns.Item("ReadOnly") = "1" Then
                                        blnReadOnly = True
                                    Else
                                        blnReadOnly = False
                                    End If
                                    tmpColItem.ppReadOnly = blnReadOnly
                                End If

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    tmpColItem.ppHorizontalAlign = dtrColumns.Item("HorizontalAlign").ToString
                                End If
                                objNewColumn.ItemTemplate = tmpColItem

                                'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                '        Case "Top"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                '        Case "Middle"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                '        Case "Bottom"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                '    End Select
                                'End If

                                '横幅計
                                If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                    If dtrColumns.Item("Visible").ToString = "1" Then
                                        dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                    Else        '列非表示
                                        objNewColumn.Visible = False
                                    End If
                                End If
                            Case "CheckBox"
                                'チェックボックス
                                objNewColumn = New TemplateField
                                Dim tmpColItem As New ClsCMCheckColumn(dtrColumns.Item("DataField"))

                                'バインド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                    tmpColItem.ppDataField = dtrColumns.Item("DataField")
                                    objNewColumn.SortExpression = dtrColumns.Item("DataField")
                                Else
                                    pfSet_GridView = False
                                    Exit Function
                                End If

                                '書込可否
                                If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                    If dtrColumns.Item("ReadOnly") = "1" Then
                                        blnReadOnly = True
                                    Else
                                        blnReadOnly = False
                                    End If
                                    tmpColItem.ppReadOnly = blnReadOnly
                                End If

                                objNewColumn.ItemTemplate = tmpColItem

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If

                                '横幅計
                                If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                    If dtrColumns.Item("Visible").ToString = "1" Then
                                        dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                    Else        '列非表示
                                        objNewColumn.Visible = False
                                    End If
                                End If
                            Case Else
                                pfSet_GridView = False
                                Exit Function
                        End Select

                        '横幅
                        objNewColumn.ItemStyle.Width = Unit.Parse(dblWidth.ToString)
                        objNewColumn.ItemStyle.Height = Unit.Parse(ipintHeight.ToString)
                        objNewColumn.HeaderStyle.Width = Unit.Parse(dblWidth.ToString)

                    Else
                        pfSet_GridView = False
                        Exit Function
                    End If

                    '共通設定
                    'ヘッダ名
                    If Not DBNull.Value.Equals(dtrColumns.Item("HeaderText")) Then
                        objNewColumn.HeaderText = dtrColumns.Item("HeaderText")
                    End If

                    '列を追加
                    ipgrvGridView.Columns.Add(objNewColumn)

                    intColumncount += 1
                Next
            End With


            '列合計幅をテーブルのサイズに設定
            dblSumWidth = dblSumWidth + 1
            ipgrvGridView.HeaderStyle.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Height = Unit.Parse(ipintHeight.ToString)
            'ソート設定
            ipgrvGridView.AllowSorting = True
            AddHandler ipgrvGridView.Sorting, AddressOf msGrid_Sorting
            AddHandler ipgrvGridView.DataBound, AddressOf msGrid_DataBound

            'ヘッダー整形
            For grvCnt As Integer = 0 To ipgrvGridView.Columns.Count - 1
                intHeaderWidth = 0
                For intcol As Integer = 0 To ipintHeader.Count - 1
                    If grvCnt = ipintHeader(intcol) Then
                        For zz As Integer = 0 To ipintColumn(intcol) - 1
                            intHeaderWidth += Double.Parse(ipgrvGridView.Columns(ipintHeader(intcol) + zz).ItemStyle.Width.ToString.Replace("px", ""))
                        Next
                        intHeaderWidth += 3
                        For xx As Integer = 0 To ipintColumn(intcol) - 1
                            If xx = 0 Then
                                ipgrvGridView.Columns(ipintHeader(intcol)).HeaderStyle.Width = intHeaderWidth
                            Else
                                ipgrvGridView.Columns(ipintHeader(intcol) + xx).HeaderStyle.CssClass = "GridNoDisp"
                            End If
                        Next
                    End If
                Next
            Next

            pfSet_GridView = True
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
            pfSet_GridView = False
        End Try

    End Function

    ''' <summary>
    ''' GridViewの設定をXmlファイルから読み込み設定(マスタのみ使用)
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
    ''' <param name="ipintHeader">設定ファイル名(拡張子除く)</param>
    ''' <param name="ipintColumn">設定ファイル名(拡張子除く)</param>
    ''' <param name="ipstrTLCls">テキストボックス表示：T／ラベル表示：L</param>
    ''' <returns>OK:True, NG:False</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_GridView(ByVal ipgrvGridView As GridView, _
                                                    ByVal ipstrXmlName As String, _
                                                    ByVal ipintHeader As Integer(), _
                                                    ByVal ipintColumn As Integer(), _
                                                    ByVal ipstrTLCls As String, _
                                                    Optional ByVal ipintHeight As Integer = 0, _
                                                    Optional ByVal ipintFontSize As Integer = 0) As Boolean

        Dim dtsXml As DataSet = New DataSet()
        Dim dblWidth As Double
        Dim dblTextWidth As Double
        Dim dblSumWidth As Double
        Dim blnReadOnly As Boolean
        Dim intHeaderWidth As Integer = 0
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '列を自動生成しない
        ipgrvGridView.AutoGenerateColumns = False

        '0件データはヘッダのみ表示
        ipgrvGridView.ShowHeaderWhenEmpty = True

        'レイアウトの設定
        ipgrvGridView.CssClass = "grid"
        ipgrvGridView.HeaderStyle.CssClass = "grid-head"
        ipgrvGridView.RowStyle.CssClass = "grid-row1"
        ipgrvGridView.AlternatingRowStyle.CssClass = "grid-row2"
        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/" & ipstrXmlName & ".xml"))
            '--------------------------------
            '2015/01/08 加賀　ここから
            '--------------------------------
            'マスタ管理画面のXMLパス指定
            'If ipstrXmlName.Substring(0, 7) = "COMUPDM" Then
            '    dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/Master/XML/" & ipstrXmlName & ".xml"))
            'End If
            '--------------------------------
            '2015/01/08 加賀　ここまで
            '--------------------------------

            Dim dtrColumns As DataRow
            Dim intColumncount As Integer = 0
            dblSumWidth = 0
            With dtsXml.Tables(0)
                For Each dtrColumns In .Rows
                    Dim objNewColumn As Object

                    '横幅
                    If Not DBNull.Value.Equals(dtrColumns.Item("Width")) Then
                        If Not Double.TryParse(dtrColumns.Item("Width").ToString, dblWidth) Then
                            dblWidth = 100
                        End If
                    Else
                        dblWidth = 100
                    End If

                    If Not DBNull.Value.Equals(dtrColumns.Item("FieldType")) Then
                        Select Case dtrColumns.Item("FieldType").ToString
                            Case "Button"
                                'ボタンフィールド
                                objNewColumn = New ButtonField

                                'ボタンテキスト
                                If Not DBNull.Value.Equals(dtrColumns.Item("text")) Then
                                    objNewColumn.Text = dtrColumns.Item("text").ToString

                                End If
                                '検証グループ名
                                If Not DBNull.Value.Equals(dtrColumns.Item("ValidationGroup")) Then
                                    objNewColumn.ValidationGroup = dtrColumns.Item("ValidationGroup").ToString
                                End If

                                'コマンド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("CommandName")) Then
                                    objNewColumn.CommandName = dtrColumns.Item("CommandName").ToString
                                End If

                                'ボタンタイプ
                                If Not DBNull.Value.Equals(dtrColumns.Item("ButtonType")) Then
                                    Select Case dtrColumns.Item("ButtonType").ToString
                                        Case "Button"
                                            objNewColumn.ButtonType = ButtonType.Button
                                        Case "Image"
                                            objNewColumn.ButtonType = ButtonType.Image
                                        Case "Link"
                                            objNewColumn.ButtonType = ButtonType.Link
                                    End Select
                                End If

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If
                                'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                '        Case "Top"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                '        Case "Middle"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                '        Case "Bottom"
                                '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                '    End Select
                                'End If

                                '横幅計
                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                If ipintHeight.ToString <> "0" Then
                                    objNewColumn.ControlStyle.Height = Unit.Parse(ipintHeight.ToString)
                                End If
                            Case "Bound"
                                'バインドフィールド
                                objNewColumn = New TemplateField
                                Select Case ipstrTLCls
                                    Case "T"
                                        'バインドフィールド
'                                        objNewColumn = New TemplateField
                                        Dim tmpColItem As ClsCMTextColumn
                                        Dim strDataField As String
                                        Dim typType As Type = Nothing
                                        Dim strFormat As String = String.Empty

                                        'バインド名
                                        If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                            strDataField = dtrColumns.Item("DataField")
                                            Select Case strDataField.Substring(0, 1)
                                                Case "N"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#0.####"
                                                Case "M"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#,##0.####"
                                            End Select
                                            tmpColItem = New ClsCMTextColumn(strDataField)
                                            tmpColItem.ppDataField = strDataField
                                            If typType IsNot Nothing Then
                                                tmpColItem.ppType = typType
                                                tmpColItem.ppFormat = strFormat
                                            End If
                                            objNewColumn.SortExpression = strDataField
                                        Else
                                            pfSet_GridView = False
                                            Exit Function
                                        End If

                                        '幅指定
                                        dblTextWidth = dblWidth - 6
                                        If dblTextWidth < 0 Then
                                            dblTextWidth = 0
                                        End If
                                        tmpColItem.ppWidth = Unit.Parse(dblTextWidth.ToString)
                                        tmpColItem.ppHeight = Unit.Parse(ipintHeight.ToString)
                                        tmpColItem.ppFontSize = FontUnit.Parse(ipintFontSize.ToString)

                                        '書込可否
                                        If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                            If dtrColumns.Item("ReadOnly") = "1" Then
                                                blnReadOnly = True
                                            Else
                                                blnReadOnly = False
                                            End If
                                            tmpColItem.ppReadOnly = blnReadOnly
                                        End If

                                        '表示位置
                                        If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                            tmpColItem.ppHorizontalAlign = dtrColumns.Item("HorizontalAlign").ToString
                                        End If
                                        objNewColumn.ItemTemplate = tmpColItem

                                        'If Not DBNull.Value.Equals(dtrColumns.Item("VerticalAlign")) Then
                                        '    Select Case dtrColumns.Item("VerticalAlign").ToString
                                        '        Case "Top"
                                        '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Top
                                        '        Case "Middle"
                                        '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Middle
                                        '        Case "Bottom"
                                        '            objNewColumn.ItemStyle.VerticalAlign = VerticalAlign.Bottom
                                        '    End Select
                                        'End If

                                        '横幅計
                                        If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                            If dtrColumns.Item("Visible").ToString = "1" Then
                                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                            Else        '列非表示
                                                objNewColumn.Visible = False
                                            End If
                                        End If
                                    Case "L"
                                        'バインドフィールド
                                        Dim tmpColItem As ClsCMLabelColumn
                                        Dim strDataField As String
                                        Dim typType As Type = Nothing
                                        Dim strFormat As String = String.Empty

                                        'バインド名
                                        If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                            strDataField = dtrColumns.Item("DataField")
                                            Select Case strDataField.Substring(0, 1)
                                                Case "N"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#0.####"
                                                Case "M"
                                                    strDataField = strDataField.Substring(1)
                                                    typType = Type.GetType("System.Decimal")
                                                    strFormat = "#,##0.####"
                                            End Select
                                            tmpColItem = New ClsCMLabelColumn(strDataField)
                                            tmpColItem.ppDataField = strDataField
                                            If typType IsNot Nothing Then
                                                tmpColItem.ppType = typType
                                                tmpColItem.ppFormat = strFormat
                                            End If
                                            objNewColumn.SortExpression = strDataField
                                        Else
                                            pfSet_GridView = False
                                            Exit Function
                                        End If

                                        '幅指定
                                        dblTextWidth = dblWidth - 6
                                        If dblTextWidth < 0 Then
                                            dblTextWidth = 0
                                        End If
                                        tmpColItem.ppWidth = Unit.Parse(dblTextWidth.ToString)
                                        tmpColItem.ppHeight = Unit.Parse(ipintHeight.ToString)
                                        tmpColItem.ppFontSize = FontUnit.Parse(ipintFontSize.ToString)

                                        '書込可否
                                        If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                            If dtrColumns.Item("ReadOnly") = "1" Then
                                                blnReadOnly = True
                                            Else
                                                blnReadOnly = False
                                            End If
                                            tmpColItem.ppReadOnly = blnReadOnly
                                        End If

                                        '表示位置
                                        If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                            tmpColItem.ppHorizontalAlign = dtrColumns.Item("HorizontalAlign").ToString
                                        End If
                                        objNewColumn.ItemTemplate = tmpColItem

                                        '横幅計
                                        If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                            If dtrColumns.Item("Visible").ToString = "1" Then
                                                dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                            Else        '列非表示
                                                objNewColumn.Visible = False
                                            End If
                                        End If
                                End Select
                            Case "CheckBox"
                                'チェックボックス
                                objNewColumn = New TemplateField
                                Dim tmpColItem As New ClsCMCheckColumn(dtrColumns.Item("DataField"))

                                'バインド名
                                If Not DBNull.Value.Equals(dtrColumns.Item("DataField")) Then
                                    tmpColItem.ppDataField = dtrColumns.Item("DataField")
                                    objNewColumn.SortExpression = dtrColumns.Item("DataField")
                                Else
                                    pfSet_GridView = False
                                    Exit Function
                                End If

                                '書込可否
                                If Not DBNull.Value.Equals(dtrColumns.Item("ReadOnly")) Then
                                    If dtrColumns.Item("ReadOnly") = "1" Then
                                        blnReadOnly = True
                                    Else
                                        blnReadOnly = False
                                    End If
                                    tmpColItem.ppReadOnly = blnReadOnly
                                End If

                                objNewColumn.ItemTemplate = tmpColItem

                                '表示位置
                                If Not DBNull.Value.Equals(dtrColumns.Item("HorizontalAlign")) Then
                                    Select Case dtrColumns.Item("HorizontalAlign").ToString
                                        Case "Center"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                                        Case "Justify"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Justify
                                        Case "Left"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Left
                                        Case "NotSet"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.NotSet
                                        Case "Right"
                                            objNewColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right
                                    End Select
                                End If

                                '横幅計
                                If Not DBNull.Value.Equals(dtrColumns.Item("Visible")) Then
                                    If dtrColumns.Item("Visible").ToString = "1" Then
                                        dblSumWidth = dblSumWidth + dblWidth + 3    '列幅＋セル間の幅
                                    Else        '列非表示
                                        objNewColumn.Visible = False
                                    End If
                                End If
                            Case Else
                                pfSet_GridView = False
                                Exit Function
                        End Select

                        '横幅
                        objNewColumn.ItemStyle.Width = Unit.Parse(dblWidth.ToString)
                        objNewColumn.ItemStyle.Height = Unit.Parse(ipintHeight.ToString)
                        objNewColumn.HeaderStyle.Width = Unit.Parse(dblWidth.ToString)

                    Else
                        pfSet_GridView = False
                        Exit Function
                    End If

                    '共通設定
                    'ヘッダ名
                    If Not DBNull.Value.Equals(dtrColumns.Item("HeaderText")) Then
                        objNewColumn.HeaderText = dtrColumns.Item("HeaderText")
                    End If

                    '列を追加
                    ipgrvGridView.Columns.Add(objNewColumn)

                    intColumncount += 1
                Next
            End With


            '列合計幅をテーブルのサイズに設定
            dblSumWidth = dblSumWidth + 1
            ipgrvGridView.HeaderStyle.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Width = Unit.Parse(dblSumWidth.ToString)
            ipgrvGridView.Height = Unit.Parse(ipintHeight.ToString)
            'ソート設定
            ipgrvGridView.AllowSorting = True
            AddHandler ipgrvGridView.Sorting, AddressOf msGrid_Sorting
            AddHandler ipgrvGridView.DataBound, AddressOf msGrid_DataBound

            'ヘッダー整形
            For grvCnt As Integer = 0 To ipgrvGridView.Columns.Count - 1
                intHeaderWidth = 0
                For intcol As Integer = 0 To ipintHeader.Count - 1
                    If grvCnt = ipintHeader(intcol) Then
                        For zz As Integer = 0 To ipintColumn(intcol) - 1
                            intHeaderWidth += Double.Parse(ipgrvGridView.Columns(ipintHeader(intcol) + zz).ItemStyle.Width.ToString.Replace("px", ""))
                        Next
                        intHeaderWidth += 3
                        For xx As Integer = 0 To ipintColumn(intcol) - 1
                            If xx = 0 Then
                                ipgrvGridView.Columns(ipintHeader(intcol)).HeaderStyle.Width = intHeaderWidth
                            Else
                                ipgrvGridView.Columns(ipintHeader(intcol) + xx).HeaderStyle.CssClass = "GridNoDisp"
                            End If
                        Next
                    End If
                Next
            Next

            pfSet_GridView = True
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
            pfSet_GridView = False
        End Try

    End Function

    ''' <summary>
    ''' GridViewの設定をXmlファイルから読み込み設定
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
    ''' <param name="ipstrXmlHederName">ヘッダ設定ファイル名(拡張子除く)</param>
    ''' <param name="ippnlOutDiv">GridViewの外側のDivコントロール</param>
    ''' <returns>OK:True, NG:False</returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_GridView(ByVal ipgrvGridView As GridView, _
                                                    ByVal ipstrXmlName As String, _
                                                    ByVal ipstrXmlHederName As String, _
                                                    ByVal ippnlOutDiv As HtmlControls.HtmlGenericControl, _
                                                    Optional ByVal ipintHeight As Integer = 0, _
                                                    Optional ByVal ipintFontSize As Integer = 10) As Boolean
        pfSet_GridView = pfSet_GridView(ipgrvGridView, ipstrXmlName, ipintHeight, ipintFontSize)
        If pfSet_GridView Then
            pfSet_GridView = mfSet_GridView_Header(ipgrvGridView, ipstrXmlHederName, ippnlOutDiv)
        End If
    End Function

     ''' <summary>
     ''' GridViewの設定をXmlファイルから読み込み設定
     ''' </summary>
     ''' <param name="ipgrvGridView">設定するGridView</param>
     ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
     ''' <param name="ipstrXmlHederName">ヘッダ設定ファイル名(拡張子除く)</param>
     ''' <param name="ippnlOutDiv">GridViewの外側のDivコントロール</param>
     ''' <param name="ipstrTLCls">テキストボックス表示：T／ラベル表示：L</param>
     ''' <returns>OK:True, NG:False</returns>
     ''' <remarks></remarks>
    Public Overloads Shared Function pfSet_GridView(ByVal ipgrvGridView As GridView, _
                                                    ByVal ipstrXmlName As String, _
                                                    ByVal ipstrXmlHederName As String, _
                                                    ByVal ippnlOutDiv As HtmlControls.HtmlGenericControl, _
                                                    ByVal ipstrTLCls As String, _
                                                    Optional ByVal ipintHeight As Integer = 0, _
                                                    Optional ByVal ipintFontSize As Integer = 10) As Boolean
        pfSet_GridView = pfSet_GridView(ipgrvGridView, ipstrXmlName, ipstrTLCls, ipintHeight, ipintFontSize)
        If pfSet_GridView Then
            pfSet_GridView = mfSet_GridView_Header(ipgrvGridView, ipstrXmlHederName, ippnlOutDiv)
        End If
    End Function

    ''' <summary>
    ''' GridViewの設定をXmlファイルから読み込み設定(ソート機能ＯＦＦ)
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
    ''' <returns>OK:True, NG:False</returns>
    ''' <remarks></remarks>
    Public Shared Function pfSet_GridView_S_Off(ByVal ipgrvGridView As GridView, _
                                                ByVal ipstrXmlName As String) As Boolean
        pfSet_GridView_S_Off = pfSet_GridView(ipgrvGridView, ipstrXmlName)
        ipgrvGridView.AllowSorting = False
    End Function

     ''' <summary>
     ''' GridViewの設定をXmlファイルから読み込み設定(ソート機能ＯＦＦ)
     ''' </summary>
     ''' <param name="ipgrvGridView">設定するGridView</param>
     ''' <param name="ipstrXmlName">設定ファイル名(拡張子除く)</param>
     ''' <param name="ipstrTLCls">テキストボックス表示：T／ラベル表示：L</param>
     ''' <returns>OK:True, NG:False</returns>
     ''' <remarks></remarks>
    Public Shared Function pfSet_GridView_S_Off(ByVal ipgrvGridView As GridView, _
                                                ByVal ipstrXmlName As String, ByVal ipstrTLCls As String) As Boolean
        pfSet_GridView_S_Off = pfSet_GridView(ipgrvGridView, ipstrXmlName, ipstrTLCls)
        ipgrvGridView.AllowSorting = False
    End Function

    ''' <summary>
    ''' CSVファイルを読込配列に格納する。
    ''' </summary>
    ''' <param name="filePath">ファイルパス（フルパス）</param>
    ''' <returns>データ格納配列</returns>
    ''' <remarks></remarks>
    Public Shared Function pfReadCsvFile(ByVal filePath As String) As List(Of String())

        Dim objFs As FileStream = Nothing   'FileStreamクラス
        Dim objSr As StreamReader = Nothing 'StreamReaderクラス
        Dim lstRet As New List(Of String())
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ファイルパスが空の場合は空文字を返す
            If filePath = String.Empty Then
                Return Nothing
            End If

            'ファイルオープン
            objFs = System.IO.File.OpenRead(filePath)
            objSr = New System.IO.StreamReader(objFs, Encoding.Default)

            Do
                Dim str As String = objSr.ReadLine
                If str Is Nothing Then
                    Exit Do
                End If
                If P_CSV_QUOTE = String.Empty Then
                    lstRet.Add(SplitCSVString(str, P_CSV_DELIMITER))
                Else
                    lstRet.Add(SplitWithQuoter(str, P_CSV_DELIMITER, P_CSV_QUOTE))
                End If
            Loop

            'ファイルクローズ
            objSr.Close()
            objFs.Close()

            Return lstRet

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
            Return Nothing
        Finally
            objSr.Dispose()
            objFs.Dispose()
        End Try

    End Function

    ''' <summary>
    ''' 新規ウィンドウで開く
    ''' </summary>
    ''' <param name="ippagPage">開き元のページコントロール</param>
    ''' <param name="ipstrPath">開き先の仮想パス</param>
    ''' <remarks></remarks>
    Public Shared Sub psOpen_Window(ByVal ippagPage As Page, ByVal ipstrPath As String)
        Dim strKey As String = Nothing
        Dim strScript As String

        '使用していないスクリプトのキー設定
        strKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "subwindow")

        strScript = "window_open(""" & VirtualPathUtility.ToAbsolute(ipstrPath) & """);"
        ippagPage.ClientScript.RegisterStartupScript(ippagPage.GetType, strKey, strScript, True)
    End Sub

    ''' <summary>
    ''' ウィンドウを閉じる
    ''' </summary>
    ''' <param name="ippagPage">対象のページコントロール</param>
    ''' <remarks></remarks>
    Public Shared Sub psClose_Window(ByVal ippagPage As Page)
        Dim strKey As String = Nothing
        Dim strScript As String

        '使用していないスクリプトのキー設定
        strKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "close")

        strScript = "(window.open('','_self').opener=window).close();"
        ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType(), strKey, strScript, True)
    End Sub

    ''' <summary>
    ''' ＰＤＦファイルを表示する（ＳＱＬ）
    ''' </summary>
    ''' <param name="ippagPage">ＰＤＦ表示元のページコントロール</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipstrSql">ＰＤＦに使用するＳＱＬ</param>
    ''' <param name="ipstrFNm">エラー時に表示する名称</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psPrintPDF(ByVal ippagPage As Page,
                                           ByVal ipobjRpt As Object,
                                           ByVal ipstrSql As String,
                                           ByVal ipstrFNm As String)
        Const COMPRVP001 As String = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001.aspx"
        Dim objRpt(0) As Object
        Dim strSql(0) As String
        Dim strFNm(0) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'セッション情報指定
            objRpt(0) = ipobjRpt
            strSql(0) = ipstrSql
            strFNm(0) = ipstrFNm
            ippagPage.Session(P_SESSION_PRV_REPORT) = objRpt
            ippagPage.Session(P_SESSION_PRV_DATA) = strSql
            ippagPage.Session(P_SESSION_PRV_NAME) = strFNm

            '別ウィンドウで出力
            psOpen_Window(ippagPage, COMPRVP001)
        Catch ex As Exception
            ' レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
            psMesBox(ippagPage, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, ipstrFNm)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return
        End Try
    End Sub

    ''' <summary>
    ''' 複数ＰＤＦファイルを表示する（ＳＱＬ）
    ''' </summary>
    ''' <param name="ippagPage">ＰＤＦ表示元のページコントロール</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipstrSql">ＰＤＦに使用するＳＱＬ</param>
    ''' <param name="ipstrFNm">エラー時に表示する名称</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psPrintPDF(ByVal ippagPage As Page,
                                           ByVal ipobjRpt() As Object,
                                           ByVal ipstrSql() As String,
                                           ByVal ipstrFNm() As String)
        Const COMPRVP001 As String = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001.aspx"
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'セッション情報指定
            ippagPage.Session(P_SESSION_PRV_REPORT) = ipobjRpt
            ippagPage.Session(P_SESSION_PRV_DATA) = ipstrSql
            ippagPage.Session(P_SESSION_PRV_NAME) = ipstrFNm

            '別ウィンドウで出力
            For zz As Integer = 0 To ipobjRpt.Length - 1
                psOpen_Window(ippagPage, COMPRVP001 & "?data=" & zz.ToString)
            Next
        Catch ex As Exception
            ' レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
            For Each strFNm As String In ipstrFNm
                psMesBox(ippagPage, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, strFNm)
            Next
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return
        End Try
    End Sub

    ''' <summary>
    ''' ＰＤＦファイルを表示する（データテーブル）
    ''' </summary>
    ''' <param name="ippagPage">ＰＤＦ表示元のページコントロール</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipobjDt">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="ipstrFNm">エラー時に表示する名称</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psPrintPDF(ByVal ippagPage As Page,
                                           ByVal ipobjRpt As Object,
                                           ByVal ipobjDt As DataTable,
                                           ByVal ipstrFNm As String)
        Const COMPRVP001 As String = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001.aspx"
        Dim objRpt(0) As Object
        Dim objDt(0) As DataTable
        Dim strFNm(0) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'セッション情報指定
            objRpt(0) = ipobjRpt
            objDt(0) = ipobjDt
            strFNm(0) = ipstrFNm
            ippagPage.Session(P_SESSION_PRV_REPORT) = objRpt
            ippagPage.Session(P_SESSION_PRV_DATA) = objDt
            ippagPage.Session(P_SESSION_PRV_NAME) = strFNm

            '別ウィンドウで出力
            psOpen_Window(ippagPage, COMPRVP001)
        Catch ex As Exception
            ' レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
            psMesBox(ippagPage, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, ipstrFNm)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return
        Finally
'            ipobjDt.Clear()
'            ipobjDt.Dispose()
'            objDt(0).Clear()
'            objDt(0).Dispose()
        End Try
    End Sub

    ''' <summary>
    ''' ＰＤＦファイルを作成する（データテーブル、サブレポート）
    ''' </summary>
    ''' <param name="ippagPage">ＰＤＦ表示元のページコントロール</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipobjDt">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="ipstrFNm">エラー時に表示する名称</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psPrintPDF(ByVal ippagPage As Page,
                                           ByVal ipobjRpt As Object,
                                           ByVal ipobjDt As DataTable(),
                                           ByVal ipstrFNm As String)
        Const COMPRVP001 As String = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001.aspx"
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'セッション情報指定
            ippagPage.Session(P_SESSION_PRV_REPORT) = ipobjRpt
            ippagPage.Session(P_SESSION_PRV_DATA) = ipobjDt
            ippagPage.Session(P_SESSION_PRV_NAME) = ipstrFNm

            '別ウィンドウで出力
            psOpen_Window(ippagPage, COMPRVP001)
        Catch ex As Exception
            ' レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
            For Each strFNm As String In ipstrFNm
                psMesBox(ippagPage, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, strFNm)
            Next
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return
        'Finally
        '    Dim zz As Integer
        '    For zz = 0 To ipobjDt.Length - 1
        '        ipobjDt(zz).Clear()
        '        ipobjDt(zz).Dispose()
        '    Next
        End Try
    End Sub

    ''' <summary>
    ''' 複数ＰＤＦファイルを表示する（データテーブル）
    ''' </summary>
    ''' <param name="ippagPage">ＰＤＦ表示元のページコントロール</param>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipobjDt">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="ipstrFNm">エラー時に表示する名称</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psPrintPDF(ByVal ippagPage As Page,
                                           ByVal ipobjRpt As Object(),
                                           ByVal ipobjDt As DataTable(),
                                           ByVal ipstrFNm As String())
        Const COMPRVP001 As String = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001/" &
            P_FUN_COM & P_SCR_PRV & P_PAGE & "001.aspx"
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'セッション情報指定
            ippagPage.Session(P_SESSION_PRV_REPORT) = ipobjRpt
            ippagPage.Session(P_SESSION_PRV_DATA) = ipobjDt
            ippagPage.Session(P_SESSION_PRV_NAME) = ipstrFNm

            '別ウィンドウで出力
            For zz As Integer = 0 To ipobjRpt.Length - 1
                psOpen_Window(ippagPage, COMPRVP001 & "?data=" & zz.ToString)
            Next
        Catch ex As Exception
            ' レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
            For Each strFNm As String In ipstrFNm
                psMesBox(ippagPage, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, strFNm)
            Next
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Return
        'Finally
        '    Dim zz As Integer
        '    For zz = 0 To ipobjDt.Length - 1
        '        ipobjDt(zz).Clear()
        '        ipobjDt(zz).Dispose()
        '    Next
        End Try
    End Sub

    ''' <summary>
    ''' ＰＤＦファイルを作成する（ＳＱＬ）
    ''' </summary>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipstrSql">ＰＤＦに使用するＳＱＬ</param>
    ''' <param name="ipstrFPath">ＰＤＦのファイル保存先</param>
    ''' <param name="ipstrFNm">ＰＤＦのファイル名(拡張子を除く)</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psCreate_PDF(ByVal ipobjRpt As Object,
                                             ByVal ipstrSql As String,
                                             ByVal ipstrFPath As String,
                                             ByVal ipstrFNm As String)
        Dim ctsSetting As ConnectionStringSettings
        Dim dS As New GrapeCity.ActiveReports.Data.SqlDBDataSource
        Dim driName As DirectoryInfo
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続文字列の取得
        ctsSetting = ConfigurationManager.ConnectionStrings("SPCDB")
        dS.ConnectionString = ctsSetting.ConnectionString

        'sql設定
        dS.SQL = ipstrSql

        Try
            'ActiveReports実行
            ipobjRpt.DataSource = dS
            ipobjRpt.Run(False)
            Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport

            '保存先のフォルダが存在しない場合作成する
            driName = New DirectoryInfo(ipstrFPath)
            If Not driName.Exists Then
                driName.Create()
            End If

            'ファイルを出力
            pdf.Export(ipobjRpt.Document, ipstrFPath & "/" & ipstrFNm & ".pdf")
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
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' ＰＤＦファイルを作成する（データテーブル）
    ''' </summary>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipobjDt">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="ipstrFPath">ＰＤＦのファイル保存先</param>
    ''' <param name="ipstrFNm">ＰＤＦのファイル名(拡張子を除く)</param>
    ''' <param name="ipstrServerIP">保存先サーバーのＩＰアドレス</param>
    ''' <param name="ipstrSvrPath">保存先サーバーのディレクトリ</param>
    ''' <param name="ipstrsessionID">セッションＩＤ</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psCreate_PDF(ByVal ipobjRpt As Object, ByVal ipobjDt As DataTable, _
                                             ByVal ipstrFPath As String, ByVal ipstrFNm As String, _
                                            ByVal ipstrServerIP As String, _
                                            ByVal ipstrSvrPath As String, ByVal ipstrsessionID As String)
        Dim driName As DirectoryInfo
        Dim dumRet As Boolean = False
        Dim strTempDir As String = System.AppDomain.CurrentDomain.BaseDirectory & "\" & "PDFBuff\" & ipstrsessionID
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Dim clsftp As New DBFTP.ClsDBFTP_Main

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "変数定義", "Catch")

        Try
psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "ActiveReports実行", "Catch")
            'ActiveReports実行
            ipobjRpt.DataSource = ipobjDt
            ipobjRpt.Run(False)
            Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport

psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "保存先のフォルダが存在しない場合作成する", "Catch")
psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "参照先フォルダ：" & ipstrSvrPath, "Catch")
            '保存先のフォルダが存在しない場合作成する
            If clsftp.pfFtpDir_Exists(ipstrSvrPath, dumRet) = False Then
                'ログ出力
                psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name, objStackFrame.GetMethod.Name, "", "保存先フォルダの確認に失敗しました。", "Catch")
                Throw New System.Exception("保存先フォルダの確認に失敗しました。")
            End If
            If dumRet Then
                driName = New DirectoryInfo(strTempDir)
                If Not driName.Exists Then
                    driName.Create()
                End If
                pdf.Export(ipobjRpt.Document, strTempDir & "/" & ipstrFNm & ".pdf")
            Else
            End If

            If clsftp.pfFtpFile_Copy("PUT", ipstrSvrPath, ipstrFNm & ".pdf", dumRet, strTempDir & "\" & ipstrFNm & ".pdf") = False Then
                Throw New System.Exception("ＰＤＦファイルの保存に失敗しました。")
            End If
            'If Not driName.Exists Then
            '    driName.Create()
            'End If

            'ファイルを出力
'            pdf.Export(ipobjRpt.Document, ipstrFPath & "/" & ipstrFNm & ".pdf")
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
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' ＰＤＦファイルを作成する（データテーブル）
    ''' </summary>
    ''' <param name="ipobjRpt">ＰＤＦのレポートクラス</param>
    ''' <param name="ipobjDt">ＰＤＦに使用するデータテーブル</param>
    ''' <param name="ipstrFPath">ＰＤＦのファイル保存先</param>
    ''' <param name="ipstrFNm">ＰＤＦのファイル名(拡張子を除く)</param>
    ''' <remarks></remarks>
    Public Overloads Shared Sub psCreate_PDF(ByVal ipobjRpt As Object,
                                             ByVal ipobjDt As DataTable,
                                             ByVal ipstrFPath As String,
                                             ByVal ipstrFNm As String)
        Dim driName As DirectoryInfo
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ActiveReports実行
            ipobjRpt.DataSource = ipobjDt
            ipobjRpt.Run(False)
            Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport

            '保存先のフォルダが存在しない場合作成する
            driName = New DirectoryInfo(ipstrFPath)
            If Not driName.Exists Then
                driName.Create()
            End If

            'ファイルを出力
            pdf.Export(ipobjRpt.Document, ipstrFPath & "/" & ipstrFNm & ".pdf")
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
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' イベント開始ログ出力
    ''' </summary>
    ''' <param name="ippagPage">対象ページコントロール</param>
    ''' <remarks></remarks>
    Public Shared Sub psLogStart(ippagPage As Page)

        'イベントログ出力判定
        Dim stringOutput As String = ConfigurationManager.AppSettings("LogOutput")
        If String.Compare(stringOutput, "On", True) <> 0 Then
            Return
        End If

        '呼出元情報定義
        Dim objStack As New StackFrame(1)

        'イベントログ出力
        psLogWrite(ippagPage.Session.SessionID, ippagPage.User.Identity.Name,
                   objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name,
                   "START", String.Empty)

    End Sub

    ''' <summary>
    ''' イベント終了ログ出力
    ''' </summary>
    ''' <param name="ippagPage">対象ページコントロール</param>
    ''' <remarks></remarks>
    Public Shared Sub psLogEnd(ippagPage As Page)

        'イベントログ出力判定
        Dim stringOutput As String = ConfigurationManager.AppSettings("LogOutput")
        If String.Compare(stringOutput, "On", True) <> 0 Then
            Return
        End If

        '呼出元情報定義
        Dim objStack As New StackFrame(1)

        'イベントログ出力
        psLogWrite(ippagPage.Session.SessionID, ippagPage.User.Identity.Name,
                   objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name,
                   "END", String.Empty)

    End Sub

    ' ''' <summary>
    ' ''' javascriptにて次ページを開く。
    ' ''' </summary>
    ' ''' <param name="ippagPage">元ページのページコントロール</param>
    ' ''' <param name="ipstrNextPath">次ページの仮想パス</param>
    ' ''' <remarks>メッセージボックス表示後遷移等に使用</remarks>
    'Public Shared Sub psNext_Page(ByVal ippagPage As Page, ByVal ipstrNextPath As String, ByVal ipshtRun As clscomver.E_S実行)
    '    Dim strKey As String = Nothing
    '    Dim strScript As String

    '    '使用していないスクリプトのキー設定
    '    strKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "nextpage")

    '    strScript = "location.replace(""" & VirtualPathUtility.ToAbsolute(ipstrNextPath) & """);"
    '    Select Case ipshtRun
    '        Case clscomver.E_S実行.描画前
    '            ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strKey, strScript, True)
    '        Case clscomver.E_S実行.描画後
    '            ippagPage.ClientScript.RegisterStartupScript(ippagPage.GetType, strKey, strScript, True)
    '    End Select
    'End Sub

    ''' <summary>
    ''' メッセージタイプの記号を取得する。
    ''' </summary>
    ''' <param name="ipshtType">メッセージタイプ</param>
    ''' <returns>メッセージタイプの記号</returns>
    ''' <remarks></remarks>
    Public Shared Function pfGet_MesType(ByVal ipshtType As Object) As String
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
    ''' GridViewのデータをDataTableに変換する。
    ''' </summary>
    ''' <param name="ipgrvData">変換するGridView</param>
    ''' <returns>変換したDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function pfParse_DataTable(ByVal ipgrvData As GridView) As DataTable
        Dim dttData As DataTable = Nothing
        Dim dtcData As DataColumn = Nothing
        Dim dtrData As DataRow = Nothing
        Dim txtData As TextBox = Nothing
        Dim chkData As CheckBox = Nothing
        Dim lblData As Label = Nothing

        dttData = New DataTable

        '列データコピー
        For Each colData In ipgrvData.Columns
            If colData.GetType.Name = "TemplateField" Then
                dtcData = New DataColumn
                dtcData.ColumnName = colData.ItemTemplate.ppDataField
                dtcData.DataType = colData.ItemTemplate.ppType
                dttData.Columns.Add(dtcData)
            End If
        Next

        '行データコピー
        For Each rowData In ipgrvData.Rows
            dtrData = dttData.NewRow
            For Each colData In ipgrvData.Columns
                If colData.GetType.Name = "TemplateField" Then
                    Select Case rowData.FindControl(colData.ItemTemplate.ppDataField()).GetType.Name
                        Case "TextBox"
                            txtData = CType(rowData.FindControl(colData.ItemTemplate.ppDataField()), TextBox)
                            Select Case colData.ItemTemplate.ppType
                                Case Type.GetType("System.Decimal")
                                    If txtData.Text <> String.Empty Then
                                        dtrData.Item(colData.ItemTemplate.ppDataField()) = txtData.Text
                                    End If
                                Case Else
                                    dtrData.Item(colData.ItemTemplate.ppDataField()) = txtData.Text
                            End Select
                        Case "CheckBox"
                            chkData = CType(rowData.FindControl(colData.ItemTemplate.ppDataField()), CheckBox)
                            dtrData.Item(colData.ItemTemplate.ppDataField()) = chkData.Checked
                        Case "Label"
                            lblData = CType(rowData.FindControl(colData.ItemTemplate.ppDataField()), Label)
                            Select Case colData.ItemTemplate.ppType
                                Case Type.GetType("System.Decimal")
                                    If lblData.Text <> String.Empty Then
                                        dtrData.Item(colData.ItemTemplate.ppDataField()) = lblData.Text
                                    End If
                                Case Else
                                    dtrData.Item(colData.ItemTemplate.ppDataField()) = lblData.Text
                            End Select
                    End Select
                End If
            Next
            dttData.Rows.Add(dtrData)
        Next

        Return dttData

    End Function

    ' ''' <summary>
    ' ''' 使用していないスクリプトキーを取得
    ' ''' </summary>
    ' ''' <param name="ippagPage">埋め込むページ</param>
    ' ''' <param name="ipType">スクリプトの型</param>
    ' ''' <param name="ipstrKey">ベースとなるキー</param>
    ' ''' <returns>スクリプトキー</returns>
    ' ''' <remarks>ベースとなるキーに数値をつけて一意となるよう生成</remarks>
    'Public Shared Function pfGet_ScriptKey(ByVal ippagPage As Page,
    '                                       ByVal ipType As Type,
    '                                       ByVal ipstrKey As String) As String
    '    Dim intKeyNo = 0
    '    Dim strKey As String = ipstrKey

    '    '使用していないスクリプトのキー設定
    '    Do Until Not ippagPage.ClientScript.IsStartupScriptRegistered(ipType, strKey)
    '        strKey = ipstrKey & intKeyNo.ToString
    '        intKeyNo = intKeyNo + 1
    '    Loop

    '    Return strKey
    'End Function

    '-----------------------------
    '2014/05/26 土岐　ここから
    '-----------------------------
    ''' <summary>
    ''' ハイフン付テキストボックスあいまい検索用編集
    ''' </summary>
    ''' <param name="ipstrOne">１つめのテキスト</param>
    ''' <param name="ipstrTwo">２つめのテキスト</param>
    ''' <returns>あいまい検索用テキスト</returns>
    ''' <remarks>両方入力があった場合のみハイフン付で返す</remarks>
    Public Shared Function pfConv_TextHyphen(ByVal ipstrOne, ByVal ipstrTwo) As String
        Dim strRtn As String = String.Empty

        If ipstrOne = String.Empty Or ipstrTwo = String.Empty Then  'どちらか入力なし
            strRtn = ipstrOne & ipstrTwo
        Else                                                        '両方入力あり
            strRtn = ipstrOne & "-" & ipstrTwo
        End If

        Return strRtn
    End Function
    '-----------------------------
    '2014/05/26 土岐　ここまで
    '-----------------------------

    Private Shared Function SplitCSVString(ByVal str As String, ByVal delimiter As String) As String()

        Dim lstRet As New List(Of String)
        Dim intFromIndex As Integer = 0
        Dim intToIndex As Integer = 0

        Dim intLastIndex As Integer = str.Length - 1

        Do
            If intLastIndex < intFromIndex Then
                Exit Do
            End If
            If intLastIndex < intToIndex Then
                Exit Do
            End If

            '区切り文字を探す。
            intToIndex = str.IndexOf(delimiter, intFromIndex)

            '区切り文字が見つからない場合
            If intToIndex < 0 Then

                intToIndex = intLastIndex
                lstRet.Add(str.Substring(intFromIndex))
                Exit Do
            End If

            lstRet.Add(str.Substring(intFromIndex, intToIndex - intFromIndex))

            If intToIndex = intLastIndex Then
                lstRet.Add("")
            End If

            intFromIndex = intToIndex + 1

        Loop

        Return lstRet.ToArray

    End Function

    Private Shared Function SplitWithQuoter(ByVal str As String, ByVal delimiter As String, ByVal quote As String) As String()
        Dim lstRet As New List(Of String)

        If str.Trim = String.Empty Then
            Return lstRet.ToArray
        End If

        Dim intFromIndex As Integer = 0
        Dim intToIndex As Integer = 0
        Dim intQuoteFrom As Integer = 0
        Dim intQuoteTo As Integer = 0
        Dim intSeparater As Integer = 0
        Do
            '区切り文字を探す
            intToIndex = str.IndexOf(delimiter, intFromIndex)

            ''見つからない場合は最後まで
            If intToIndex < 0 Then
                intToIndex = str.Length - 1
            Else
                If intToIndex = intFromIndex Then
                    lstRet.Add(String.Empty)
                    If intToIndex = str.Length - 1 Then
                        lstRet.Add(String.Empty)
                        Exit Do
                    End If
                    intFromIndex = intToIndex + 1
                    Continue Do
                ElseIf intToIndex = intFromIndex + 1 Then
                    lstRet.Add(str.Substring(intFromIndex, 1))
                    If intToIndex = str.Length - 1 Then
                        lstRet.Add(String.Empty)
                        Exit Do
                    End If
                    intFromIndex = intToIndex + 1
                    Continue Do
                End If

                'あった場合は区切り文字の一つ前の文字の位置を取得する.
                intToIndex = intToIndex - 1
            End If
            ''囲み文字を探す
            intQuoteFrom = str.IndexOf(quote, intFromIndex, intToIndex + 1 - intFromIndex)
            If intQuoteFrom < 0 Then
                If intToIndex = intFromIndex Then
                    lstRet.Add(str.Substring(intFromIndex, 1))
                Else
                    lstRet.Add(str.Substring(intFromIndex, intToIndex - intFromIndex + 1))
                End If

                If intToIndex = str.Length - 1 Then
                    Exit Do
                End If

                '次の区切り文字を探す
                intSeparater = str.IndexOf(delimiter, intToIndex + 1)

                If intSeparater < 0 Then
                    Exit Do
                End If
                If intSeparater = str.Length - 1 Then
                    lstRet.Add(String.Empty)
                    Exit Do
                End If

                intFromIndex = intSeparater + 1
                Continue Do
            End If

            '囲みの終わりを探す
            If intQuoteFrom = str.Length - 1 Then   'intFromIndexが最後の文字の位置である場合
                intQuoteTo = -1
            Else
                Dim intquote As Integer = intQuoteFrom + 1

                While str.Length - 1 >= intquote
                    intquote = str.IndexOf(quote, intquote)
                    If intquote < 0 Then
                        intquote = -1
                        Exit While
                    End If
                    If intquote = str.Length - 1 Then
                        Exit While
                    End If
                    If str.Chars(intquote + 1) <> quote Then
                        Exit While
                    End If
                    intquote = intquote + 2
                End While
                intQuoteTo = intquote
            End If

            '囲みの終わりがない場合はデータが不正
            If intQuoteTo < 0 Then
                Return Nothing
            End If

            ''空データの
            If intQuoteFrom + 1 = intQuoteTo Then
                lstRet.Add(String.Empty)
            Else
                lstRet.Add(str.Substring(intQuoteFrom + 1, intQuoteTo - (intQuoteFrom + 1)).Replace(quote & quote, quote))
            End If

            '次の区切り文字を探す
            intSeparater = str.IndexOf(delimiter, intQuoteTo)

            If intSeparater < 0 Then
                Exit Do
            End If
            If intSeparater = str.Length - 1 Then
                lstRet.Add(String.Empty)
                Exit Do
            End If
            intFromIndex = intSeparater + 1

        Loop

        Return lstRet.ToArray

    End Function

    ' ''' <summary>
    ' ''' Xmlファイルからメッセージを取得する
    ' ''' </summary>
    ' ''' <param name="ipstrNo">取得するメッセージNo</param>
    ' ''' <param name="ipshtType">取得するメッセージType</param>
    ' ''' <returns>取得したメッセージ</returns>
    ' ''' <remarks></remarks>
    'Private Shared Function mfGet_MesXml(ByVal ipstrNo As String, ByVal ipshtType As clscomver.E_Mタイプ) As String
    '    Dim dtsXml As DataSet = New DataSet()
    '    Dim dtrSelect() As DataRow
    '    Dim strType As String
    '    '--------------------------------
    '    '2014/04/14 星野　ここから
    '    '--------------------------------
    '    objStackFrame = New StackFrame
    '    '--------------------------------
    '    '2014/04/14 星野　ここまで
    '    '--------------------------------

    '    strType = pfGet_MesType(ipshtType)

    '    Try
    '        dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/Message.xml"))

    '        With dtsXml.Tables(0)
    '            dtrSelect = .Select("No = '" & ipstrNo & "' AND Type = '" & strType & "'")
    '            If dtrSelect.Length > 0 Then
    '                mfGet_MesXml = dtrSelect(0).Item("Message").ToString
    '            Else
    '                mfGet_MesXml = String.Empty
    '            End If

    '        End With
    '    Catch ex As Exception
    '        '--------------------------------
    '        '2014/04/14 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
    '                        objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/14 星野　ここまで
    '        '--------------------------------
    '        mfGet_MesXml = String.Empty
    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' メッセージを表示するScriptをHtmlに埋め込む。
    ' ''' </summary>
    ' ''' <param name="ippagPage">表示するページコントロール</param>
    ' ''' <param name="ipstrMes">表示するメッセージ</param>
    ' ''' <param name="ipstrMesID">メッセージID</param>
    ' ''' <param name="ipshtType">メッセージのタイプ</param>
    ' ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ' ''' <remarks></remarks>
    'Private Shared Sub msSet_MesBox(ByVal ippagPage As Page,
    '                                ByVal ipstrMes As String,
    '                                ByVal ipstrMesID As String,
    '                                ByVal ipshtType As clscomver.E_Mタイプ,
    '                                ByVal ipshtRun As clscomver.E_S実行)
    '    Dim strLoad As String
    '    Dim strScript As String
    '    Dim objStack As StackFrame
    '    Dim strVBSKey As String
    '    Dim strSKey As String

    '    If ipshtType = clscomver.E_Mタイプ.エラー Then
    '        'ログファイルへ出力
    '        objStack = New StackFrame(2)   '呼出元情報
    '        psLogWrite(ippagPage.Session.SessionID, ippagPage.User.Identity.Name,
    '                   objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name,
    '                   "ERROR", ipstrMesID & ":" & ipstrMes)
    '    End If

    '    strScript = mfGet_Script(ipstrMes, ipstrMesID, ipshtType, clscomver.E_Mモード.OK)

    '    '使用していないスクリプトのキー設定
    '    strSKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "mes")
    '    Select Case ipshtRun
    '        Case clscomver.E_S実行.描画前
    '            strLoad = mfGet_MesScript(ipshtType, clscomver.E_Mモード.OK)
    '            '使用していないスクリプトのキー設定
    '            strVBSKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "vbmes")
    '            'ページ読込前は外部ファイルが読み込まれないためVBスクリプトを発行する
    '            ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strVBSKey, strLoad, False)
    '            ippagPage.ClientScript.RegisterClientScriptBlock(ippagPage.GetType, strSKey, strScript, True)
    '        Case clscomver.E_S実行.描画後
    '            ippagPage.ClientScript.RegisterStartupScript(ippagPage.GetType, strSKey, strScript, True)
    '    End Select
    'End Sub

    ''' <summary>
    ''' メッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrMes">表示するメッセージ</param>
    ''' <param name="ipstrTitle">表示するタイトル</param>
    ''' <param name="ipshtType">表示するタイプ</param>
    ''' <param name="ipshtMode">表示するモード</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Private Shared Function mfGet_Script(ByVal ipstrMes As String,
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

    ' ''' <summary>
    ' ''' メッセージボックスのVBScriptを取得する。
    ' ''' </summary>
    ' ''' <param name="ipshtType">表示するタイプ</param>
    ' ''' <param name="ipshtMode">表示するモード</param>
    ' ''' <returns>メッセージボックスを表示するVBスクリプト</returns>
    ' ''' <remarks>外部ファイル「popup.vbs」と同内容なため変更時はそちらも変更すること。</remarks>
    'Private Shared Function mfGet_MesScript(ByVal ipshtType As clscomver.E_Mタイプ,
    '                                        ByVal ipshtMode As clscomver.) As String
    '    Dim strScript As New StringBuilder

    '    strScript.AppendLine("<script type=""text/VBScript"">")
    '    Select Case ipshtType
    '        Case clscomver.E_Mタイプ.エラー
    '            Select Case ipshtMode
    '                Case clscomver.E_Mモード.OK
    '                    strScript.AppendLine("Function vb_MsgCri_O(mes, mesno)")
    '                    strScript.AppendLine("    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno)")
    '                    strScript.AppendLine("End Function")

    '                Case clscomver.E_Mモード.OKCancel
    '                    strScript.AppendLine("Function vb_MsgCri_OC(mes, mesno)")
    '                    strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbCritical), mesno)")
    '                    strScript.AppendLine("        Case vbOK")
    '                    strScript.AppendLine("            vb_MsgCri_OC = True")
    '                    strScript.AppendLine("        Case Else")
    '                    strScript.AppendLine("            vb_MsgCri_OC = False")
    '                    strScript.AppendLine("    End Select")
    '                    strScript.AppendLine("End Function")

    '            End Select
    '        Case clscomver.E_Mタイプ.警告
    '            Select Case ipshtMode
    '                Case clscomver.E_Mモード.OK
    '                    strScript.AppendLine("Function vb_MsgExc_O(mes, mesno)")
    '                    strScript.AppendLine("    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbExclamation), mesno)")
    '                    strScript.AppendLine("End Function")

    '                Case clscomver.E_Mモード.OKCancel
    '                    strScript.AppendLine("Function vb_MsgExc_OC(mes, mesno)")
    '                    strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbExclamation), mesno)")
    '                    strScript.AppendLine("        Case vbOK")
    '                    strScript.AppendLine("            vb_MsgExc_OC = True")
    '                    strScript.AppendLine("        Case Else")
    '                    strScript.AppendLine("            vb_MsgExc_OC = False")
    '                    strScript.AppendLine("    End Select")
    '                    strScript.AppendLine("End Function")
    '            End Select
    '        Case clscomver.E_Mタイプ.情報
    '            Select Case ipshtMode
    '                Case clscomver.E_Mモード.OK
    '                    strScript.AppendLine("Function vb_MsgInf_O(mes, mesno)")
    '                    strScript.AppendLine("    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbInformation), mesno)")
    '                    strScript.AppendLine("End Function")
    '                Case clscomver.E_Mモード.OKCancel
    '                    strScript.AppendLine("Function vb_MsgInf_OC(mes, mesno)")
    '                    strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOK + vbInformation), mesno)")
    '                    strScript.AppendLine("        Case vbOK")
    '                    strScript.AppendLine("            vb_MsgInf_OC = True")
    '                    strScript.AppendLine("        Case Else")
    '                    strScript.AppendLine("            vb_MsgInf_OC = False")
    '                    strScript.AppendLine("    End Select")
    '                    strScript.AppendLine("End Function")
    '            End Select
    '    End Select
    '    strScript.AppendLine("</script>")

    '    If strScript.ToString = String.Empty Then
    '        Return String.Empty
    '    End If

    '    mfGet_MesScript = strScript.ToString
    'End Function

    ''' <summary>
    ''' Xmlファイルから検証メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <returns>取得したメッセージ行(ショートメッセージ、メッセージ)</returns>
    ''' <remarks></remarks>
    Private Shared Function mfGet_ValMesXml(ByVal ipstrNo As String) As DataRow
        Dim dtsXml As DataSet = New DataSet()
        Dim dtrSelect() As DataRow
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/ValMessage.xml"))

            With dtsXml.Tables(0)
                dtrSelect = .Select("No = " & ipstrNo)
                If dtrSelect.Length > 0 Then
                    mfGet_ValMesXml = dtrSelect(0)
                Else
                    mfGet_ValMesXml = Nothing
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
            mfGet_ValMesXml = Nothing
        End Try
    End Function

    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    ' ''' <summary>
    ' ''' ログファイル出力
    ' ''' </summary>
    ' ''' <param name="strSessionID">セッションID</param>
    ' ''' <param name="strUserID">ログインID</param>
    ' ''' <param name="strClass">対象クラス</param>
    ' ''' <param name="strMethod">対象メソッド</param>
    ' ''' <param name="strMessageType">メッセージタイプ</param>
    ' ''' <param name="strMessage">メッセージ</param>
    ' ''' <param name="strLogType">ログタイプ</param>
    ' ''' <remarks></remarks>
    'Public Shared Sub psLogWrite(Optional ByVal strSessionID As String = "",
    '                              Optional ByVal strUserID As String = "",
    '                              Optional ByVal strClass As String = "",
    '                              Optional ByVal strMethod As String = "",
    '                              Optional ByVal strMessageType As String = "",
    '                              Optional ByVal strMessage As String = "",
    '                              Optional ByVal strLogType As String = "")

    '    Dim logDir As String = System.AppDomain.CurrentDomain.BaseDirectory & "Log"

    '    If strLogType = "Catch" Then
    '        logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & DateTime.Now.ToString("yyyyMM")
    '        '--------------------------------
    '        '2014/04/16 星野　ここから
    '        '--------------------------------
    '        '■□■□結合試験時のみ使用予定□■□■
    '    ElseIf strLogType = "TRANS" Then
    '        logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log\" & DateTime.Now.ToString("yyyyMM") & "\TRANS"
    '        '■□■□結合試験時のみ使用予定□■□■
    '        '--------------------------------
    '        '2014/04/16 星野　ここまで
    '        '--------------------------------
    '    Else
    '        logDir = System.AppDomain.CurrentDomain.BaseDirectory & "Log"
    '    End If

    '    'ログフォルダ名作成
    '    System.IO.Directory.CreateDirectory(logDir)

    '    'ログの書き込み処理（書き込み失敗時は0.25秒置きに1000回リトライ）
    '    For zz As Integer = 0 To 1000
    '        Try
    '            'ログファイルをロックしてオープン
    '            Using objFs As New FileStream(logDir + "\" + DateTime.Now.ToString("yyyyMMdd") + ".log",
    '                                          FileMode.Append, FileAccess.Write, FileShare.None)

    '                Dim objBuff As New StringBuilder    'StringBuilderクラス

    '                '日時
    '                objBuff.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
    '                objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
    '                'セッションID
    '                If strLogType <> "Catch" Then
    '                    objBuff.Append(strSessionID)
    '                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
    '                End If
    '                'ログインID
    '                objBuff.Append(strUserID)
    '                objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
    '                '対象クラス
    '                objBuff.Append(strClass)
    '                objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
    '                '対象メソッド
    '                objBuff.Append(strMethod)
    '                objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
    '                'メッセージタイプ
    '                If strLogType <> "Catch" Then
    '                    objBuff.Append(strMessageType)
    '                    objBuff.Append(Microsoft.VisualBasic.ControlChars.Tab)
    '                End If
    '                'メッセージ
    '                objBuff.AppendLine(strMessage.Replace("\n", ""))

    '                'ログ出力
    '                objFs.Write(Encoding.GetEncoding("shift-jis").GetBytes(objBuff.ToString), 0,
    '                            Encoding.GetEncoding("shift-jis").GetByteCount(objBuff.ToString))

    '                'ファイルクローズ
    '                objFs.Close()

    '            End Using

    '            Exit Sub

    '        Catch ex As Exception
    '            System.Threading.Thread.Sleep(250)
    '        Finally
    '        End Try
    '    Next

    'End Sub
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------

    ''' <summary>
    ''' GridViewのヘッダ設定をXmlファイルから読み込み設定
    ''' </summary>
    ''' <param name="ipgrvGridView">設定するGridView</param>
    ''' <param name="ipstrXmlHederName">ヘッダ設定ファイル名(拡張子除く)</param>
    ''' <param name="ippnlOutDiv">GridViewの外側のDivコントロール</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function mfSet_GridView_Header(ByVal ipgrvGridView As GridView,
                                                  ByVal ipstrXmlHederName As String,
                                                  ByVal ippnlOutDiv As HtmlControls.HtmlGenericControl) As Boolean

        Dim dtsHeaderXml As DataSet = New DataSet()
        Dim strPaddingTop As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '描画時ヘッダ設定アクション設定
            ipgrvGridView.HeaderStyle.CssClass = Nothing
            Dim ghlGrid As New ClsCMGridHeaderLoad(ipstrXmlHederName)
            AddHandler ipgrvGridView.RowCreated, AddressOf ghlGrid.psGrid_RowCreated

            'データ部の表示位置設定
            dtsHeaderXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/" & ipstrXmlHederName & ".xml"))
            strPaddingTop = Unit.Parse(((dtsHeaderXml.Tables.Count * 16) - 1).ToString).ToString
            ippnlOutDiv.Style.Item(HtmlTextWriterStyle.PaddingTop) = strPaddingTop
            mfSet_GridView_Header = True
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
            mfSet_GridView_Header = False
        End Try
    End Function

    ''' <summary>
    ''' GridViewのソート順取得
    ''' </summary>
    ''' <param name="ipstrColumn">ソート列名</param>
    ''' <param name="ippagPage">GridViewのページ</param>
    ''' <param name="ipstrGridID">GridViewのID</param>
    ''' <returns>ソート順序文字列（昇順：ASC, 降順：DESC）</returns>
    ''' <remarks>マスターページのViewState使用（ソート順序保持）</remarks>
    Private Shared Function pfGetSortDirection(ByVal ipstrColumn As String,
                                               ByVal ippagPage As Page,
                                               ByVal ipstrGridID As String) As String

        Dim strSortDirection As String = "ASC"
        '--------------------------------
        '2015/01/06 加賀　ここから
        '--------------------------------
        'Dim mstMyMaster As SiteMaster = Nothing
        Dim mstMyMaster As Object = Nothing
        '--------------------------------
        '2015/01/06 加賀　ここまで
        '--------------------------------        
        Dim strSortExpression As String = Nothing
        Dim strLastDirection As String = Nothing
        Dim strViewColKey As String = ipstrGridID + P_VIEW_SORT_COL
        Dim strViewDirKey As String = ipstrGridID + P_VIEW_SORT_DIR

        'SiteMasterのページ取得
        Select Case ippagPage.Master.GetType.Name
            Case P_SITE_T_NM
                mstMyMaster = ippagPage.Master
            Case P_REFE_T_NM, "reference_reverse_master" 'ClsCMCommon-002
                mstMyMaster = ippagPage.Master.Master

                'ClsCMCommon-001 
                'Case "master_mst_master", "master_site_master"
            Case "master_mst_master", "master_site_master", "master_mst_ref_master"
                'ClsCMCommon-001
                'マスタ管理画面
                mstMyMaster = ippagPage.Master
        End Select

        If mstMyMaster Is Nothing Then
            Return strSortDirection
        End If

        strSortExpression = TryCast(mstMyMaster.pfGet_ViewState(strViewColKey), String)
        If strSortExpression IsNot Nothing Then         '前回のソート列がない
            If strSortExpression = ipstrColumn Then     'ソート列が前回と同じ
                '前回のソート順取得
                strLastDirection = TryCast(mstMyMaster.pfGet_ViewState(strViewDirKey), String)
                If strLastDirection IsNot Nothing _
                  AndAlso strLastDirection = "ASC" Then '前回のソート順がないまたは昇順
                    '降順を設定
                    strSortDirection = "DESC"

                End If

            End If
        End If

        'ViewStateにソート列、ソート順保存
        mstMyMaster.psSet_ViewState(strViewColKey, ipstrColumn)
        mstMyMaster.psSet_ViewState(strViewDirKey, strSortDirection)

        Return strSortDirection

    End Function


    ''' <summary>
    ''' 逆ポーランド記法整形
    ''' </summary>
    ''' <param name="ipstrNumerical"></param>
    ''' <param name="ipintResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function pfRPN(ByVal ipstrNumerical As String _
                               , ByRef ipintResult As Double) As Boolean

        Dim buff_st As String = String.Empty      'スタック
        Dim st_temp As String = String.Empty      'スタック一時保管場所
        Dim rpn_stack As New Stack                '結果
        Dim wk_stack As New Stack                 'スタック
        Dim buff_temp As String = String.Empty    'トークン
        Dim buff_numerical() As String            '数式(スプリット)
        Dim numerical As String                   '数式
        Dim dble As Double = 0                    '数値型判断用
        Dim int_rpn As Double = 0                 '演算子比較用
        Dim int_st As Double = 0                  '演算子比較用
        Dim roopF As Boolean = False              'ループ用

        pfRPN = False

        numerical = ipstrNumerical

        '計算式が空白の場合は終了
        If numerical = String.Empty Then
            Exit Function
        End If

        '記号を半角文字に変換と整形を行う
        numerical = numerical.Replace("(", ":(:").Replace(")", ":):")
        numerical = numerical.Replace("（", ":(:").Replace("）", ":):")
        numerical = numerical.Replace("+", ":+:").Replace("＋", ":+:")
        numerical = numerical.Replace("-", ":-:").Replace("－", ":-:")
        numerical = numerical.Replace("*", ":*:").Replace("＊", ":*:")
        numerical = numerical.Replace("/", ":/:").Replace("／", ":/:")
        numerical = numerical.Replace(" ", "").Replace("　", "")
        buff_numerical = numerical.Split(":")

        '文字数分ループ
        For i = 0 To buff_numerical.Count - 1

            If buff_numerical(i) <> String.Empty Then

                '一文字取得
                buff_temp = buff_numerical(i)

                '数値であるか
                If Double.TryParse(buff_temp, dble) Then
                    'トークン(数字)を結果にプッシュ
                    rpn_stack.Push(buff_temp)
                Else '数値ではない

                    '括弧であるか判断する
                    If buff_temp = ")" Then
                        ' ( が発生するまで繰り返し
                        Do
                            buff_temp = CType(wk_stack.Pop(), String)
                            If buff_temp = "(" Then
                                buff_temp = String.Empty
                                Exit Do
                            Else
                                If wk_stack.Count <> 0 Then
                                    'トークンを結果に追加
                                    rpn_stack.Push(buff_temp)
                                Else
                                    ' ( がない
                                    pfRPN = False
                                    Exit Function
                                End If
                            End If
                        Loop

                    ElseIf buff_temp = "(" Then

                        'スタックに追加
                        wk_stack.Push(buff_temp)

                    Else

                        'スタックが空か判断
                        If wk_stack.Count = 0 Then
                            'スタックに追加
                            wk_stack.Push(buff_temp)
                        Else

                            roopF = False
                            While roopF = False

                                'スタックの最後の一文字を取得
                                st_temp = CType(wk_stack.Pop(), String)

                                'トークンの演算子の優先度を判定
                                Select Case buff_temp
                                    Case "/"
                                        int_rpn = 4
                                    Case "*"
                                        int_rpn = 3
                                    Case "-"
                                        int_rpn = 2
                                    Case "+"
                                        int_rpn = 1
                                    Case Else
                                        '演算子でも数字でもない
                                        pfRPN = False
                                        Exit Function
                                End Select

                                'スタックの演算子の優先度を判定
                                Select Case st_temp
                                    Case "/"
                                        int_st = 4
                                    Case "*"
                                        int_st = 3
                                    Case "-"
                                        int_st = 2
                                    Case "+"
                                        int_st = 1
                                    Case "("
                                        int_rpn = 5
                                    Case Else
                                        int_st = 0
                                End Select

                                'トークンのほうが優先度が低い
                                If int_rpn < int_st Then
                                    'トークンを結果に追加
                                    rpn_stack.Push(st_temp)

                                    'スタックが空か判断する
                                    If wk_stack.Count = 0 Then
                                        'スタックに追加
                                        wk_stack.Push(buff_temp)
                                        roopF = True
                                    End If
                                Else
                                    'スタックに追加
                                    wk_stack.Push(st_temp)
                                    wk_stack.Push(buff_temp)
                                    roopF = True
                                End If
                            End While
                        End If
                    End If
                End If
            Else
                '空文字は飛ばす
            End If
        Next

        'スタックに残った文字列を連結
        Do Until wk_stack.Count = 0
            'トークンを結果に追加
            rpn_stack.Push(CType(wk_stack.Pop(), String))
        Loop

        '計算開始
        If pfRPN_Result(rpn_stack, ipintResult) Then
            pfRPN = True
        Else
            pfRPN = False
        End If

    End Function

    ''' <summary>
    ''' 逆ポーランド記法計算
    ''' </summary>
    ''' <param name="rpn_stack"></param>
    ''' <param name="ipintResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function pfRPN_Result(ByVal rpn_stack As Stack _
                                       , ByRef ipintResult As Double) As Boolean

        Dim tmp_stack As New Stack                '一時保管スタック
        Dim wk_stack As New Stack                 'スタック
        Dim buff_str As String = String.Empty     '取得文字列
        Dim buff_double As Double = 0             '取得数値
        Dim a As Double = 0                       '計算用
        Dim b As Double = 0                       '計算用

        pfRPN_Result = False

        '逆ポーランド記法整形のスタックを逆転させる
        Do Until rpn_stack.Count = 0
            tmp_stack.Push(CType(rpn_stack.Pop(), String))
        Loop

        '計算開始
        Do Until tmp_stack.Count = 0

            'スタックから値を取得
            buff_str = CType(tmp_stack.Pop(), String)

            Select Case buff_str
                Case "+"
                    a = CType(wk_stack.Pop(), Double)
                    b = CType(wk_stack.Pop(), Double)
                    wk_stack.Push(b + a)
                Case "-"
                    a = CType(wk_stack.Pop(), Double)
                    b = CType(wk_stack.Pop(), Double)
                    wk_stack.Push(b - a)
                Case "*"
                    a = CType(wk_stack.Pop(), Double)
                    b = CType(wk_stack.Pop(), Double)
                    wk_stack.Push(b * a)
                Case "/"
                    a = CType(wk_stack.Pop(), Double)
                    b = CType(wk_stack.Pop(), Double)
                    If a = 0 Then
                        '0割りが発生
                        Exit Function
                    Else
                        wk_stack.Push(b / a)
                    End If

                Case Else
                    '数値として読み込む
                    If Double.TryParse(buff_str, buff_double) Then
                        wk_stack.Push(buff_double)
                    Else
                        '数値ではない場合
                        Exit Function
                    End If
            End Select
        Loop

        '結果を出力
        ipintResult = CType(wk_stack.Pop(), Double)
        pfRPN_Result = True

    End Function

    ''' <summary>
    ''' ファイルダウンロード
    ''' </summary>
    ''' <param name="strFolderNM">ダウンロード先のパス</param>
    ''' <param name="strFileName">ダウンロードするファイル名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function pfFile_Download(ByVal strFolderNM As String _
                                       , ByRef strFileName As String) As String
        Dim DBFTP As New DBFTP.ClsDBFTP_Main
        Dim localdirName As String = "DOWNLOAD"
        Dim localFiledir As String = "C:"
        Dim strLocalPath As String = localFiledir & "/" & localdirName & "/"
        Dim filePath2 As String = Nothing
        Dim strExtension As String = Nothing
        Dim opblnResult As Boolean = False
        Dim localFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString

        pfFile_Download = ""

        'パス生成.
        filePath2 = strFolderNM & "\"

        'ファイルの存在確認
        If DBFTP.pfFtpFile_Exists(filePath2, strFileName, opblnResult) = False Then
            'ファイルが存在しない
            Exit Function
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
        'btnDownload_Start(strDownLoadfile)

        'ダウンロードファイル存在確認(保存先)
        If Not File.Exists(strLocalPath & localFileName) Then
            'ファイルが存在しない
            Exit Function
        End If

        'パスの再設定
        strLocalPath = strLocalPath & localFileName

        pfFile_Download = strLocalPath
    End Function


    ''' <summary>
    ''' ファイルアップロード
    ''' </summary>
    ''' <param name="strFolderNM">アップロード先のパス</param>
    ''' <param name="strFileName">アップロードするファイル名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function pfFile_Upload(ByVal strFolderNM As String _
                                       , ByRef strFileName As String _
                                       , ByRef localFileName As String _
                                       ) As Boolean
        Dim DBFTP As New DBFTP.ClsDBFTP_Main
        Dim localdirName As String = "UPLOAD"
        Dim localFiledir As String = "C:"
        Dim strLocalPath As String = localFiledir & "/" & localdirName & "/"
        Dim strExtension As String = Nothing
        Dim opblnResult As Boolean = False
        Dim dirpath As DirectoryInfo
        Dim strWorkPath As String = String.Empty '出力パス

        pfFile_Upload = False

psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "出力パス生成", "Catch")
        '出力パス生成.
        strWorkPath = strFolderNM & "\"

        dirpath = New DirectoryInfo(strWorkPath)
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "保存先のフォルダの存在有無を確認", "Catch")
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "dirpath:" & dirpath.ToString, "Catch")
        '保存先のフォルダの存在有無を確認
        If DBFTP.pfFtpDir_Exists(dirpath.ToString, opblnResult, "0") = False Then
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "アップロードに失敗", "Catch")
            'アップロードに失敗
            Exit Function
        Else
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "DBFTP.pfFtpFile_Exists", "Catch")
            If DBFTP.pfFtpFile_Exists(dirpath.ToString, strFileName, opblnResult) = True Then
                If DBFTP.pfFtpFile_Delete(dirpath.ToString, strFileName, opblnResult) = False Then
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "アップロードに失敗", "Catch")
                    'アップロードに失敗
                    Exit Function
                End If
            End If
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "DBFTP.pfFtpFile_Copy", "Catch")
            If DBFTP.pfFtpFile_Copy("PUT", dirpath.ToString, strFileName, opblnResult, strLocalPath & localFileName & ".csv") = False Then
psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "アップロードに失敗", "Catch")
                'アップロードに失敗
                Exit Function
            End If
        End If

psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "ローカルに一時的に作成したファイルを削除", "Catch")
        'ローカルに一時的に作成したファイルを削除
        'ファイルの存在を確認
        If System.IO.File.Exists(strLocalPath & localFileName & ".csv") Then
            System.IO.File.Delete(strLocalPath & localFileName & ".csv")
        End If

        pfFile_Upload = True

psLogWrite("", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "pfFile_Upload", "END", "Catch")

    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
