'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事一覧
'*　ＰＧＭＩＤ：　CNSLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.27　：　酒井
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSLSTP001-001     2015/07/06      栗原　　　検索欄と検索条件にシステムを追加、グリッド項目「システム分類」を文言、内容共に「システム」に変更
'CNSLSTP001-002     2015/08/31      加賀　　　検索項目「ホールコード」削除
'CNSLSTP001-003     2015/08/31      加賀　　　検索項目「工事開始日」追加
'CNSLSTP001-004     2015/08/31      加賀　　　検索項目「進捗状況」の検索方法変更
'CNSLSTP001-005     2015/09/01      加賀　　　検索項目「FS稼働無」の検索項目追加
'CNSLSTP001-006     2015/09/01      加賀　　　一覧の初期表示、リロードの追加
'CNSLSTP001-007     2015/09/01      加賀　　　一覧のボタン活性制御をRowDataBoundイベントに統一
'CNSLSTP001-008     2015/09/02      加賀　　　SPC一般ユーザーの一覧進捗ボタンの活性制御変更
'CNSLSTP001-009     2015/09/18      加賀　　　進捗状況の判定条件変更(文言→コード)
'CNSLSTP001-010     2016/02/23      加賀　　　grvList_RowCommandでソート時にExitするよう修正
'CNSLSTP001-011     2016/06/22      栗原　　　明細表示項目に[都道府県コード:名称]を追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

#End Region

Public Class CNSLSTP001
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
    Const sCnsErr_0001 As String = "00001"
    Const sCnsErr_0002 As String = "00002"
    Const sCnsErr_0003 As String = "00003"
    Const sCnsErr_0004 As String = "00004"
    Const sCnsErr_0005 As String = "00005"
    Const sCnsErr_30008 As String = "30008"

    '工事一覧 画面のパス
    Const sCnsCNSLSTP001 As String = "~/" & P_CNS & "/" &
                                        P_FUN_CNS & P_SCR_LST & P_PAGE & "001.aspx"

    '工事依頼書兼仕様書 画面のパス
    Const sCnsCNSUPDP001 As String = "~/" & P_CNS & "/" & "CNSUPDP001/" &
                                        P_FUN_CNS & P_SCR_UPD & P_PAGE & "001.aspx"
    '工事進捗　参照更新画面のパス
    Const sCnsCNSUPDP003 As String = "~/" & P_CNS & "/" & "CNSUPDP003/" &
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "003.aspx"

    '一覧ボタン名
    Const sCnsbtnReference As String = "btnReference"       '参照
    Const sCnsbtnUpdate As String = "btnUpdate"             '更新
    Const sCnsbtnProgress As String = "btnProgress"         '進捗管理

    Const sCnsProgid As String = "CNSLSTP001"
    '-----------------------------
    '2014/05/27 土岐　ここから
    '-----------------------------
    Const sCnsUpdateid As String = "CNSUPDP001"             '工事依頼書兼仕様書
    Const sCnsUpdateid003 As String = "CNSUPDP003"             '工事進捗
    '-----------------------------
    '2014/05/27 土岐　ここまで
    '-----------------------------
    Const sCnsSqlid As String = "CNSLSTP001_S1"
    Const sCnsAddButon As String = "仮登録"
    Const sCnsShinchokuButon As String = "進捗管理"

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
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim blnFirstpostback As Boolean = True
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "Page_Init-Load-PreRender"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, sCnsProgid, 36, 11)
        'pfSet_GridView(Me.grvList, sCnsProgid, "L", 36, 11)

    End Sub

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            'フッダーボタンのイベント設定
            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton3.Click, AddressOf Button_Click
            AddHandler btnReload.Click, AddressOf msReLoad  'CNSLSTP001-007 リロード

            If Not IsPostBack Then  '初回表示

                blnFirstpostback = True

                '画面右上該当件数非表示
                Master.ppCount_Visible = False

                '「クリア」「当日」「リロード」ボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False
                Master.ppRigthButton3.CausesValidation = False
                btnReload.CausesValidation = False

                '「仮登録」のボタン活性
                Master.ppRigthButton3.Visible = True

                '「仮登録」のボタン表示設定
                Master.ppRigthButton3.Text = sCnsAddButon

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = sCnsProgid
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'CNSLSTP001-007
                ''グリッドの初期化
                'Me.grvList.DataSource = New DataTable

                ''件数を初期設定
                ''-----------------------------
                ''2014/05/30 土岐　ここから
                ''-----------------------------
                'Me.lblCount.Text = "0"
                ''-----------------------------
                ''2014/05/30 土岐　ここまで
                ''-----------------------------
                ''ヘッダ表示
                'Me.grvList.DataBind()

                msReLoad()
                'CNSLSTP001-007 END

                'ドロップダウンリスト生成.
                Me.msGet_DropListData_Sel()

                'CNSLSTP001-001
                Me.msSetddlSystem()
                'CNSLSTP001-001 END

            Else
                blnFirstpostback = False
            End If

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        If blnFirstpostback Then
            '--------------------------------
            '2014/07/24 星野　ここまで
            '--------------------------------
            'ddlOutputOrder.SelectedIndex = 1
            ddlOutputOrder.SelectedIndex = 0
            '--------------------------------
            '2014/07/24 星野　ここまで
            '--------------------------------
        End If

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
                Master.ppRigthButton3.Enabled = False       '仮登録ボタン
            Case "NGC"
        End Select
    End Sub

#End Region


#Region "一覧"

    ''' <summary>
    ''' GridViewデータバインド
    ''' </summary>
    ''' <remarks>ClsCMTextColumnで挿入された改行を削除</remarks>
    Private Sub msGrvList_DataBound(sender As Object, e As EventArgs) Handles grvList.PreRender

        Try
            Dim txtCells() As TextBox
            'Dim txtCells() As Label
            Dim strAuth As String = Session(P_SESSION_AUTH)
            Dim strUserID As String = Session(P_SESSION_USERID)

            '行ループ
            For Each grvrow As GridViewRow In grvList.Rows

                'トーマス　D02_CONSTRACTのデータの場合、進捗管理ボタンを非活性
                If CType(grvrow.FindControl("データ区分"), TextBox).Text.ToString.Trim.Equals("1") Then
                    'If CType(grvrow.FindControl("データ区分"), Label).Text.ToString.Trim.Equals("1") Then
                    grvrow.Cells(2).Enabled = False
                End If

                '現場到着待ち以前のデータの場合、進捗管理ボタンを非活性
                If CType(grvrow.FindControl("進捗状況区分"), TextBox).Text.ToString.Equals("1") Then
                    'If CType(grvrow.FindControl("進捗状況区分"), Label).Text.ToString.Equals("1") Then
                    grvrow.Cells(2).Enabled = False
                End If

                '権限設定
                Select Case strAuth
                    Case "SPC"
                        'CNSLSTP001-008 ユーザーID[SPCNGC]の場合、進捗ボタン制御
                        If String.Compare(strUserID, "SPCNGC", True) = 0 Then
                            If CType(grvrow.FindControl("進捗状況CD"), TextBox).Text.Replace(Environment.NewLine, "").Trim.Equals("06") Then
                                'If CType(grvrow.FindControl("進捗状況CD"), Label).Text.Replace(Environment.NewLine, "").Trim.Equals("06") Then
                                grvrow.Cells(2).Enabled = True     '進捗管理
                            Else
                                grvrow.Cells(2).Enabled = False    '進捗管理
                            End If
                        End If
                    Case "営業所"
                        grvrow.Cells(1).Enabled = False    '更新ボタン
                        grvrow.Cells(2).Enabled = False    '進捗管理ボタン
                        If CType(grvrow.FindControl("連絡区分"), TextBox).Text.Replace(Environment.NewLine, "").Trim.Equals("キャンセル") = False Then
                            'If CType(grvrow.FindControl("連絡区分"), Label).Text.Replace(Environment.NewLine, "").Trim.Equals("キャンセル") = False Then
                            Select CType(grvrow.FindControl("進捗状況CD"), TextBox).Text.Replace(Environment.NewLine, "").Trim
                            'Select Case CType(grvrow.FindControl("進捗状況CD"), Label).Text.Replace(Environment.NewLine, "").Trim
                                Case "03", "04", "05", "06", "07"
                                    grvrow.Cells(1).Enabled = True    '更新ボタン
                            End Select
                        End If
                End Select

                '改行を削除するカラムを設定
                txtCells = {DirectCast(grvrow.FindControl("ホール名"), TextBox) _
                          , DirectCast(grvrow.FindControl("代行店名"), TextBox) _
                          , DirectCast(grvrow.FindControl("代理店名"), TextBox) _
                          , DirectCast(grvrow.FindControl("その他内容"), TextBox) _
                          }
                'txtCells = {DirectCast(grvrow.FindControl("ホール名"), Label) _
                '          , DirectCast(grvrow.FindControl("代行店名"), Label) _
                '          , DirectCast(grvrow.FindControl("代理店名"), Label) _
                '          , DirectCast(grvrow.FindControl("その他内容"), Label) _
                '          }

                '改行削除(CR+LF) ※任意で改行する場合は[CR][LF]いずれかを使用する
                For Each txtColumn As TextBox In txtCells
                    'For Each txtColumn As Label In txtCells
                    txtColumn.Text = txtColumn.Text.Replace(Environment.NewLine, "")
                Next
            Next

        Catch ex As Exception
            'メッセージ出力
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧の表示編集処理に失敗しました。")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

    ''' <summary>
    ''' 一覧の更新／参照／進捗画面ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList

        objStack = New StackFrame

        Try
            'CNSLSTP001-010 追加
            If e.CommandName = "Sort" Then
                Exit Sub
            End If

            'ログ出力開始
            psLogStart(Me)

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

            'セッション情報設定
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text

            '排他情報用のグループ番号保管
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            End If

            '工事依頼書兼仕様書(D39)存在確認（受付確定済）
            Dim dsGetData As DataSet = Nothing
            Dim const_no As String = CType(rowData.FindControl("依頼番号"), TextBox).Text.Substring(0, 14)
            'Dim const_no As String = CType(rowData.FindControl("依頼番号"), Label).Text.Substring(0, 14)
            Dim data_kbn As String
            'Dim renrakukbn As String = CType(rowData.FindControl("連絡区分"), TextBox).Text.Replace(Environment.NewLine, "").ToString 'CNSLSTP001-009
            'Dim status As String = CType(rowData.FindControl("進捗状況"), TextBox).Text.Replace(Environment.NewLine, "").ToString 'CNSLSTP001-009
            Dim renrakukbn As String = CType(rowData.FindControl("連絡区分CD"), TextBox).Text.Replace(Environment.NewLine, "").ToString
            'Dim renrakukbn As String = CType(rowData.FindControl("連絡区分CD"), Label).Text.Replace(Environment.NewLine, "").ToString
            Dim status As String = CType(rowData.FindControl("進捗状況CD"), TextBox).Text.Replace(Environment.NewLine, "").ToString
            'Dim status As String = CType(rowData.FindControl("進捗状況CD"), Label).Text.Replace(Environment.NewLine, "").ToString
            If mfGetCnstreqspec(const_no, dsGetData) Then
                '未処理の場合、登録モードで画面を開く
                'If status = "未処理" Then
                If status = "01" Then
                    If mfGetCnstreqspcTMS(const_no) Then
                        data_kbn = "1"
                    Else
                        data_kbn = "0"
                        renrakukbn = dsGetData.Tables(0).Rows(0).Item("D39_TELL_CLS").ToString()
                        'status = dsGetData.Tables(0).Rows(0).Item("M30_STATUS_NM").ToString() CNSLSTP001-009
                        status = dsGetData.Tables(0).Rows(0).Item("D39_MTR_STATUS_CD").ToString()
                    End If
                Else
                    data_kbn = "0"
                    renrakukbn = dsGetData.Tables(0).Rows(0).Item("D39_TELL_CLS").ToString()
                    'status = dsGetData.Tables(0).Rows(0).Item("M30_STATUS_NM").ToString() CNSLSTP001-009
                    status = dsGetData.Tables(0).Rows(0).Item("D39_MTR_STATUS_CD").ToString()
                End If

            Else
                data_kbn = "1"
            End If


            Select Case e.CommandName
                Case sCnsbtnReference     '参照
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                    '受渡し用依頼番号
                    Session(P_KEY) = {const_no, data_kbn, status}

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
                                    objStack.GetMethod.Name, sCnsCNSUPDP001, strPrm, "TRANS")
                    '工事依頼書兼仕様書 画面遷移
                    psOpen_Window(Me, sCnsCNSUPDP001)

                Case sCnsbtnUpdate        '更新

                    If data_kbn.Equals("1") Then
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                    Else
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                    End If

                    '受渡し用依頼番号
                    Session(P_KEY) = {const_no, _
                                      data_kbn, _
                                      status, _
                                      renrakukbn}

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
                                    objStack.GetMethod.Name, sCnsCNSUPDP001, strPrm, "TRANS")

                    arKey.Insert(0, const_no)

                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D39_CNSTREQSPEC")

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , sCnsUpdateid _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        'ビューステートの設定
                        Me.ViewState("行番号") = intIndex

                        '★登録年月日時刻
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Else
                        '排他ロック中
                        Exit Sub

                    End If

                    psOpen_Window(Me, sCnsCNSUPDP001)           '工事依頼書兼仕様書 画面遷移

                Case sCnsbtnProgress   '進捗管理
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
                    '受渡し用依頼番号
                    Session(P_KEY) = {const_no, CType(rowData.FindControl("設置区分"), TextBox).Text.ToString.Trim}
                    'Session(P_KEY) = {const_no, CType(rowData.FindControl("設置区分"), Label).Text.ToString.Trim}
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
                                    objStack.GetMethod.Name, sCnsCNSUPDP003, strPrm, "TRANS")
                    '-----------------------------
                    '2014/05/27 間瀬　ここから
                    '-----------------------------
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D24_CNST_SITU_DTL")
                    arTable_Name.Insert(1, "D39_CNSTREQSPEC")
                    arTable_Name.Insert(2, "D84_ANYTIME_LIST")

                    arKey.Insert(0, const_no)
                    arKey.Insert(1, CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.ToString.Trim)
                    'arKey.Insert(1, CType(rowData.FindControl("ＴＢＯＸＩＤ"), Label).Text.ToString.Trim)
                    arKey.Insert(2, CType(rowData.FindControl("総合テスト日"), TextBox).Text.Replace(Environment.NewLine, "").Trim)
                    'arKey.Insert(2, CType(rowData.FindControl("総合テスト日"), Label).Text.Replace(Environment.NewLine, "").Trim)

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , sCnsUpdateid003 _
                                     , arTable_Name _
                                     , arKey) = 0 Then
                        'ビューステートの設定
                        Me.ViewState("行番号") = intIndex

                        '★登録年月日時刻
                        Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    Else
                        '排他ロック中
                        Exit Sub

                    End If
                    '-----------------------------
                    '2014/05/27 間瀬　ここまで
                    '-----------------------------

                    '別ブラウザ起動
                    psOpen_Window(Me, sCnsCNSUPDP003)           '工事進捗　参照更新画面遷移
            End Select

            'ログ出力終了
            psLogEnd(Me)

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region


#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        objStack = New StackFrame

        Try

            'ログ出力開始
            psLogStart(Me)

            Select Case sender.ID
                Case "btnSearchRigth1"        '検索ボタン押下
                    '個別エラーチェック.
                    Call msCheck_Error()

                    If (Page.IsValid) Then
                        '条件検索取得
                        msGet_Data()
                    End If
                Case "btnSearchRigth2"        '検索クリアボタン押下

                    '検索条件クリア
                    msClearSearch()

                Case "btnSearchRigth3"        '仮登録ボタン押下
                    Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.仮登録
                    Session(P_KEY) = Nothing

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
                                    objStack.GetMethod.Name, sCnsCNSUPDP001, strPrm, "TRANS")
                    psOpen_Window(Me, sCnsCNSUPDP001)
            End Select

            'ログ出力終了
            psLogEnd(Me)

        Catch ex As Exception
            'メッセージ表示
            psMesBox(Me, sCnsErr_0003, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

#Region "条件検索取得処理"

    ''' <summary>
    ''' 条件検索取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        '-----------------------------
        '2014/05/26 土岐　ここから
        '-----------------------------
        Dim strNoFrom As String
        Dim strNoTo As String
        '-----------------------------
        '2014/05/26 土岐　ここまで
        '-----------------------------

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            '画面ページ表示初期化
            Me.lblCount.Text = "0"
            Me.grvList.DataSource = Nothing

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(sCnsSqlid, conDB)

            With cmdDB.Parameters
                '-----------------------------
                '2014/05/26 土岐　ここから
                '-----------------------------
                'ハイフンがあるため変換
                strNoFrom = pfConv_TextHyphen(Me.tftRequestNo.ppFromTextOne.Trim,
                                              Me.tftRequestNo.ppFromTextTwo.Trim)
                .Add(pfSet_Param("RequestNo_From", SqlDbType.NVarChar, strNoFrom))                              '依頼番号
                strNoTo = pfConv_TextHyphen(Me.tftRequestNo.ppToTextOne.Trim,
                                            Me.tftRequestNo.ppToTextTwo.Trim)
                .Add(pfSet_Param("RequestNo_To", SqlDbType.NVarChar, strNoTo))
                '-----------------------------
                '2014/05/26 土岐　ここまで
                '-----------------------------
                .Add(pfSet_Param("CnstDt_From", SqlDbType.NVarChar, Me.dftCnstDt.ppFromText))                   '工事開始日
                .Add(pfSet_Param("CnstDt_To", SqlDbType.NVarChar, Me.dftCnstDt.ppToText))
                .Add(pfSet_Param("STestDt_From", SqlDbType.NVarChar, Me.dftSTestDt.ppFromText))                 '総合テスト日
                .Add(pfSet_Param("STestDt_To", SqlDbType.NVarChar, Me.dftSTestDt.ppToText))
                .Add(pfSet_Param("ReceptionDt_From", SqlDbType.NVarChar, Me.dftReceptionDt.ppFromText))         '受信日付
                .Add(pfSet_Param("ReceptionDt_To", SqlDbType.NVarChar, Me.dftReceptionDt.ppToText))
                'CNSLSTP001-002
                '.Add(pfSet_Param("HoleCd_From", SqlDbType.NVarChar, Me.tftHoleCd.ppFromText.Trim))             'ホールコード
                '.Add(pfSet_Param("HoleCd_To", SqlDbType.NVarChar, Me.tftHoleCd.ppToText.Trim))
                'CNSLSTP001-002
                .Add(pfSet_Param("ContactDvs", SqlDbType.NVarChar, Me.ddlContactDvs.ppSelectedValue.Trim))      '連絡区分
                .Add(pfSet_Param("Agency_From", SqlDbType.NVarChar, Me.tftAgency.ppFromText))                   '代理店
                .Add(pfSet_Param("Agency_To", SqlDbType.NVarChar, Me.tftAgency.ppToText))
                .Add(pfSet_Param("TboxId_From", SqlDbType.NVarChar, Me.tftTboxId.ppFromText.Trim))              'ＴＢＯＸＩＤ
                .Add(pfSet_Param("TboxId_To", SqlDbType.NVarChar, Me.tftTboxId.ppToText.Trim))
                .Add(pfSet_Param("AgencyShop_From", SqlDbType.NVarChar, Me.tftAgencyShop.ppFromText))           '代行店
                .Add(pfSet_Param("AgencyShop_To", SqlDbType.NVarChar, Me.tftAgencyShop.ppToText))
                .Add(pfSet_Param("PrgSituation", SqlDbType.NVarChar, Me.ddlPrgSituatio.ppSelectedValue))          '進捗状況
                .Add(pfSet_Param("PrgSituRange", SqlDbType.NVarChar, Me.ddlSituUpDwn.SelectedValue))            '進捗状況範囲　CNSLSTP001-004
                .Add(pfSet_Param("OutputOrder", SqlDbType.NVarChar, Me.ddlOutputOrder.SelectedValue))           '出力順

                'ホール内工事
                If Me.cbxNew.Checked.Equals(True) Then  '新規
                    .Add(pfSet_Param("H_NEW", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_NEW", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxSomeRemoval.Checked.Equals(True) Then  '一部撤去
                    .Add(pfSet_Param("H_PRT_REMOVE", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_PRT_REMOVE", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxAllRemoval.Checked.Equals(True) Then  '全撤去
                    .Add(pfSet_Param("H_ALL_REMOVE", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_ALL_REMOVE", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxReInstallation.Checked.Equals(True) Then  '再設置
                    .Add(pfSet_Param("H_RESET", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_RESET", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxConDelivery.Checked.Equals(True) Then  '構成配信
                    .Add(pfSet_Param("H_DLV_ORGNZ", SqlDbType.NVarChar, "2"))
                Else
                    .Add(pfSet_Param("H_DLV_ORGNZ", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxExpansion.Checked.Equals(True) Then  '増設
                    .Add(pfSet_Param("H_ADD", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_ADD", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxShopRelocation.Checked.Equals(True) Then  '店舗移設
                    .Add(pfSet_Param("H_RELOCATE", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_RELOCATE", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxOnceRemoval.Checked.Equals(True) Then  '一時撤去
                    .Add(pfSet_Param("H_TMP_REMOVE", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_TMP_REMOVE", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxConChange.Checked.Equals(True) Then  '構成変更
                    .Add(pfSet_Param("H_CHNG_ORGNZ", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_CHNG_ORGNZ", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxVup.Checked.Equals(True) Then  'その他
                    .Add(pfSet_Param("VUP", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("VUP", SqlDbType.NVarChar, String.Empty))
                End If
                If Me.cbxOther.Checked.Equals(True) Then  'その他
                    .Add(pfSet_Param("H_OTH", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("H_OTH", SqlDbType.NVarChar, String.Empty))
                End If

                .Add(pfSet_Param("Progid", SqlDbType.NVarChar, sCnsProgid))        '画面ＩＤ

                'CNSLSTP001-001
                .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                'CNSLSTP001-001　END

                If Me.cbxFsWrk.Checked.Equals(True) Then  'FS稼働　CNSLSTP001-005
                    .Add(pfSet_Param("Fs_Wrk", SqlDbType.NVarChar, "1"))
                Else
                    .Add(pfSet_Param("Fs_Wrk", SqlDbType.NVarChar, String.Empty))
                End If


            End With

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.grvList.DataSource = dstOrders
            '件数を設定
            Me.lblCount.Text = dstOrders.Tables(0).Rows.Count

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '変更を反映
            Me.grvList.DataBind()

            ''D02_CONSTRACTのデータ抽出　'CNSLSTP001-007　RoeDataDound移動
            'For i As Integer = 0 To grvList.Rows.Count - 1
            '    Dim rowData As GridViewRow = grvList.Rows(i)
            '    If CType(rowData.FindControl("データ区分"), TextBox).Text.ToString.Trim.Equals("1") Then
            '        'トーマス　D02_CONSTRACTのデータの場合、進捗管理ボタンを非活性
            '        grvList.Rows(i).Cells(2).Enabled = False
            '    End If
            '    '6/12 高松 追加
            '    If CType(rowData.FindControl("進捗状況区分"), TextBox).Text.ToString.Equals("1") Then
            '        '現場到着待ち以前のデータの場合、進捗管理ボタンを非活性
            '        grvList.Rows(i).Cells(2).Enabled = False
            '    End If
            '    '6/12 高松 ここまで
            '    If Session(P_SESSION_AUTH) = "SPC" Then

            '        If CType(rowData.FindControl("進捗状況"), TextBox).Text.ToString.Trim.Equals("現場作業待ち") Then
            '            grvList.Rows(i).Cells(2).Enabled = True     '進捗管理
            '        Else
            '            grvList.Rows(i).Cells(2).Enabled = False    '進捗管理
            '        End If

            '    ElseIf Session(P_SESSION_AUTH) = "営業所" Then
            '        Select Case CType(rowData.FindControl("進捗状況"), TextBox).Text.ToString.Trim
            '            Case "作業依頼中", "営業所受託", "物品転送依頼済", "現場作業待ち", "現場終了"
            '                grvList.Rows(i).Cells(1).Enabled = True    '更新ボタン
            '        End Select
            '        'If CType(rowData.FindControl("進捗状況"), TextBox).Text.ToString.Trim.Equals("作業依頼中") Then
            '        '    grvList.Rows(i).Cells(1).Enabled = True    '更新ボタン
            '        'End If
            '        If CType(rowData.FindControl("進捗状況"), TextBox).Text.ToString.Trim.Equals("現場作業待ち") Then
            '            grvList.Rows(i).Cells(2).Enabled = False    '進捗管理ボタン
            '        End If
            '        If CType(rowData.FindControl("連絡区分"), TextBox).Text.ToString.Trim.Equals("キャンセル") Then
            '            grvList.Rows(i).Cells(1).Enabled = False    '更新ボタン
            '            grvList.Rows(i).Cells(2).Enabled = False    '進捗管理ボタン
            '        End If
            '        '変更連絡で日付が作業担当、出発日時がクリアされて・・・

            '    End If

            'Next

            If dstOrders.Tables(0).Rows.Count = 0 Then
                '0件
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Me.lblCount.Text = dstOrders.Tables(0).Rows.Count.ToString
            Else
                '閾値を超えた場合はメッセージを表示
                If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                        dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                End If
                Me.lblCount.Text = dstOrders.Tables(0).Rows(0)("データ件数").ToString
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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
            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
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
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub

    ''' <summary>
    ''' リロード/初期表示　CNSLSTP001-006
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msReLoad()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        objStack = New StackFrame

        Try
            '画面ページ表示初期化
            Me.lblCount.Text = "0"
            Me.grvList.DataSource = Nothing

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            'パラメータ設定
            cmdDB = New SqlCommand(sCnsSqlid, conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("RequestNo_From", SqlDbType.NVarChar, String.Empty))               '依頼番号
                .Add(pfSet_Param("RequestNo_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("CnstDt_From", SqlDbType.NVarChar, String.Empty))                  '工事開始日
                .Add(pfSet_Param("CnstDt_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("STestDt_From", SqlDbType.NVarChar, String.Empty))                 '総合テスト日
                .Add(pfSet_Param("STestDt_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("ReceptionDt_From", SqlDbType.NVarChar, _
                                 Date.Now.Date.ToShortDateString))                                  '受信日付
                .Add(pfSet_Param("ReceptionDt_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("ContactDvs", SqlDbType.NVarChar, String.Empty))                   '連絡区分
                .Add(pfSet_Param("Agency_From", SqlDbType.NVarChar, String.Empty))                  '代理店
                .Add(pfSet_Param("Agency_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("TboxId_From", SqlDbType.NVarChar, String.Empty))                  'ＴＢＯＸＩＤ
                .Add(pfSet_Param("TboxId_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("AgencyShop_From", SqlDbType.NVarChar, String.Empty))              '代行店
                .Add(pfSet_Param("AgencyShop_To", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("PrgSituation", SqlDbType.NVarChar, String.Empty))                 '進捗状況
                .Add(pfSet_Param("PrgSituRange", SqlDbType.NVarChar, String.Empty))                 '進捗状況範囲
                .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, String.Empty))                    'システム
                .Add(pfSet_Param("OutputOrder", SqlDbType.NVarChar, "9"))                           '出力順 進捗状況,受信日時
                .Add(pfSet_Param("H_NEW", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_PRT_REMOVE", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_ALL_REMOVE", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_RESET", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_DLV_ORGNZ", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_ADD", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_RELOCATE", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_TMP_REMOVE", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_CHNG_ORGNZ", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("VUP", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("H_OTH", SqlDbType.NVarChar, String.Empty))
                .Add(pfSet_Param("Progid", SqlDbType.NVarChar, sCnsProgid))                         '画面ＩＤ
                .Add(pfSet_Param("Fs_Wrk", SqlDbType.NVarChar, String.Empty))                       'FS稼働
            End With

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            Me.grvList.DataSource = dstOrders
            '件数を設定
            Me.lblCount.Text = dstOrders.Tables(0).Rows.Count

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '変更を反映
            Me.grvList.DataBind()

            If dstOrders.Tables(0).Rows.Count = 0 Then
                '0件
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Me.lblCount.Text = dstOrders.Tables(0).Rows.Count.ToString
            Else
                '閾値を超えた場合はメッセージを表示
                If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                        dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                End If
                Me.lblCount.Text = dstOrders.Tables(0).Rows(0)("データ件数").ToString
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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
            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            Me.grvList.DataBind()
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub

#End Region

#Region "検索条件クリア"
    ''' <summary>
    ''' 検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()
        Me.tftRequestNo.ppFromText = String.Empty           '依頼番号From
        Me.tftRequestNo.ppToText = String.Empty             '依頼番号To
        Me.dftCnstDt.ppFromText = String.Empty              '工事開始日From
        Me.dftCnstDt.ppToText = String.Empty                '工事開始日To
        Me.dftSTestDt.ppFromText = String.Empty             '総合テスト日From
        Me.dftSTestDt.ppToText = String.Empty               '総合テスト日To
        Me.dftReceptionDt.ppFromText = String.Empty         '受信日付From
        Me.dftReceptionDt.ppToText = String.Empty           '受信日付To
        'CNSLSTP001-002　除外
        'Me.tftHoleCd.ppFromText = String.Empty              'ホールコードFrom　
        'Me.tftHoleCd.ppToText = String.Empty                'ホールコードTo
        'CNSLSTP001-002　END
        Me.ddlContactDvs.ppDropDownList.SelectedIndex = 0   '連絡区分
        Me.tftAgency.ppFromText = String.Empty              '代理店From
        Me.tftAgency.ppToText = String.Empty                '代理店To
        Me.tftTboxId.ppFromText = String.Empty              'ＴＢＯＸＩＤFrom
        Me.tftTboxId.ppToText = String.Empty                'ＴＢＯＸＩＤTo
        Me.tftAgencyShop.ppFromText = String.Empty          '代行店From
        Me.tftAgencyShop.ppToText = String.Empty            '代行店To
        Me.ddlPrgSituatio.ppDropDownList.SelectedIndex = 0  '進捗状況
        Me.ddlSituUpDwn.SelectedIndex = 0                   '進捗状況範囲　'CNSLSTP001-004
        Me.ddlOutputOrder.SelectedIndex = 0                 '出力順
        Me.cbxNew.Checked = False                           'ホール内工事新規
        Me.cbxSomeRemoval.Checked = False                   'ホール内工事一部撤去
        Me.cbxAllRemoval.Checked = False                    'ホール内工事全撤去
        Me.cbxReInstallation.Checked = False                'ホール内工事再設置
        Me.cbxConDelivery.Checked = False                   'ホール内工事構成配信
        Me.cbxExpansion.Checked = False                     'ホール内工事増設
        Me.cbxShopRelocation.Checked = False                'ホール内工事店舗移設
        Me.cbxOnceRemoval.Checked = False                   'ホール内工事一時撤去
        Me.cbxConChange.Checked = False                     'ホール内工事構成変更
        Me.cbxVup.Checked = False                           'ＶＵＰ
        Me.cbxOther.Checked = False                         'ホール内工事その他
        Me.ddlSystem.SelectedIndex = -1                     'システム   CNSLSTP001-001
        Me.cbxFsWrk.Checked = False                         'FS稼働     CNSLSTP001-005
    End Sub
#End Region

#Region "工事依頼書兼仕様書取得処理"
    ''' <summary>
    ''' 工事依頼書兼仕様書取得処理
    ''' </summary>
    ''' <param name="const_no">工事依頼番号</param>
    ''' <param name="dsCnstrecspec">作業担当者</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGetCnstreqspec(ByVal const_no As String, _
                             Optional ByRef dsCnstrecspec As DataSet = Nothing)

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
                cmdDB = New SqlCommand("CNSLSTP001_S2", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, const_no))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                dsCnstrecspec = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '該当データあり
                        mfGetCnstreqspec = True
                    Case Else
                        '該当データなし
                        mfGetCnstreqspec = False
                End Select

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGetCnstreqspec = False
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGetCnstreqspec = False
        End If
    End Function

    ''' <summary>
    ''' 工事依頼書兼仕様書取得処理(TOMAS)
    ''' </summary>
    ''' <param name="const_no">工事依頼番号</param>
    ''' <param name="dsCnstrecspec">作業担当者</param>
    ''' <returns>整合性OK：True, NG:False</returns>
    ''' <remarks></remarks>
    Private Function mfGetCnstreqspcTMS(ByVal const_no As String, _
                             Optional ByRef dsCnstrecspec As DataSet = Nothing)

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
                cmdDB = New SqlCommand("CNSLSTP001_S3", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("cnst_no", SqlDbType.NVarChar, const_no))
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                End With

                'データ取得
                dsCnstrecspec = clsDataConnect.pfGet_DataSet(cmdDB)

                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"
                        '該当データあり
                        mfGetCnstreqspcTMS = True
                    Case Else
                        '該当データなし
                        mfGetCnstreqspcTMS = False
                End Select

            Catch ex As Exception
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGetCnstreqspcTMS = False
            Finally
                'DB切断
                clsDataConnect.pfClose_Database(conDB)
            End Try
        Else
            mfGetCnstreqspcTMS = False
        End If
    End Function

#End Region

#Region "個別エラーチェック"
    ''' <summary>
    ''' 個別エラーチェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        Dim strErr As String

        '工事依頼番号FROM～TO
        If Not Me.tftRequestNo.ppFromText.ToString.Equals(String.Empty) _
            And Not Me.tftRequestNo.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_TxtFTErr(Me.tftRequestNo.ppFromText.Trim, Me.tftRequestNo.ppToText.Trim, False)
            If Not strErr.Equals(String.Empty) Then
                Me.tftRequestNo.psSet_ErrorNo(strErr, "工事依頼番号", "番号")
            End If
        End If

        '工事開始日FROM～TO
        If Not Me.dftCnstDt.ppFromText.ToString.Equals(String.Empty) _
           And Not Me.dftCnstDt.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_DateFTErr(Me.dftCnstDt.ppFromText, Me.dftCnstDt.ppToText, False)
            If strErr <> "" Then
                Me.dftCnstDt.psSet_ErrorNo(strErr, "工事開始日", "日付")
            End If
        End If

        '総合テスト日FROM～TO
        If Not Me.dftSTestDt.ppFromText.ToString.Equals(String.Empty) _
           And Not Me.dftSTestDt.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_DateFTErr(Me.dftSTestDt.ppFromText, Me.dftSTestDt.ppToText, False)
            If strErr <> "" Then
                Me.dftSTestDt.psSet_ErrorNo(strErr, "総合テスト日", "日付")
            End If
        End If

        '受信日付FROM～TO
        If Not Me.dftReceptionDt.ppFromText.ToString.Equals(String.Empty) _
           And Not Me.dftReceptionDt.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_DateFTErr(Me.dftReceptionDt.ppFromText, Me.dftReceptionDt.ppToText, False)
            If Not strErr.Equals(String.Empty) Then
                Me.dftReceptionDt.psSet_ErrorNo(strErr, "受信日付", "日付")
            End If
        End If

        'ＴＢＯＸＩＤFROM～TO
        If Not Me.tftTboxId.ppFromText.ToString.Equals(String.Empty) _
           And Not Me.tftTboxId.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_TxtFTErr(Me.tftTboxId.ppFromText, Me.tftTboxId.ppToText, False)
            If Not strErr.Equals(String.Empty) Then
                Me.tftTboxId.psSet_ErrorNo(strErr, "ＴＢＯＸＩＤ", "ＴＢＯＸＩＤ")
            End If
        End If

        'ホールコードFROM～TO　'CNSLSTP001-002　除外
        'If Not Me.tftHoleCd.ppFromText.ToString.Equals(String.Empty) _
        '  And Not Me.tftHoleCd.ppToText.ToString.Equals(String.Empty) Then
        '    strErr = pfCheck_TxtFTErr(Me.tftHoleCd.ppFromText, Me.tftHoleCd.ppToText, False)
        '    If Not strErr.Equals(String.Empty) Then
        '        Me.tftHoleCd.psSet_ErrorNo(strErr, "ホールコード", "コード")
        '    End If
        'End If

        '代理店コードFROM～TO
        If Not Me.tftAgency.ppFromText.ToString.Equals(String.Empty) _
          And Not Me.tftAgency.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_TxtFTErr(Me.tftAgency.ppFromText, Me.tftAgency.ppToText, False)
            If Not strErr.Equals(String.Empty) Then
                Me.tftAgency.psSet_ErrorNo(strErr, "代理店", "コード")
            End If
        End If

        '代行店コードFROM～TO
        If Not Me.tftAgencyShop.ppFromText.ToString.Equals(String.Empty) _
          And Not Me.tftAgencyShop.ppToText.ToString.Equals(String.Empty) Then
            strErr = pfCheck_TxtFTErr(Me.tftAgencyShop.ppFromText, Me.tftAgencyShop.ppToText, False)
            If Not strErr.Equals(String.Empty) Then
                Me.tftAgencyShop.psSet_ErrorNo(strErr, "代行店", "コード")
            End If
        End If

        '進捗状況
        If ddlSituUpDwn.SelectedIndex > 0 AndAlso ddlPrgSituatio.ppSelectedValue = String.Empty Then
            ddlPrgSituatio.psSet_ErrorNo("5003", ddlPrgSituatio.ppName)
        End If

    End Sub

#End Region

#Region "ドロップダウンリスト設定"
    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_DropListData_Sel()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '-----------------------------
                '2014/05/28 土岐　ここから
                '-----------------------------
                objCmd = New SqlCommand("ZCMPSEL041", objCn)
                '-----------------------------
                '2014/05/28 土岐　ここまで
                '-----------------------------

                objCmd.Parameters.Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "17"))

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定（進捗ステータスマスタ）
                Me.ddlPrgSituatio.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlPrgSituatio.ppDropDownList.DataTextField = objDs.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlPrgSituatio.ppDropDownList.DataValueField = objDs.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlPrgSituatio.DataBind()
                Me.ddlPrgSituatio.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

                'ドロップダウンリスト設定（連絡区分）
                Me.ddlContactDvs.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                'ドロップダウンリスト設定（出力順）
                '--------------------------------
                '2014/07/24 星野　ここから
                '--------------------------------
                'Me.ddlOutputOrder.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加
                'Me.ddlOutputOrder.Items.Insert(1, "1:受信日付順")
                'Me.ddlOutputOrder.Items.Insert(2, "2:依頼番号順")
                '--------------------------------
                '2014/07/24 星野　ここまで
                '--------------------------------

            Catch ex As Exception
                '-----------------------------
                '2014/05/28 土岐　ここから
                '-----------------------------
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ステータスマスタ一覧取得")
                '-----------------------------
                '2014/05/28 土岐　ここまで
                '-----------------------------
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト(システム)作成 'CNSLSTP001-001
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        Dim clsDataConnect As New ClsCMDataConnect
        Dim clsMst As New ClsMSTCommon

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            clsMst.psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                objDs.Tables(0).DefaultView.Sort = "ＴＢＯＸシステムコード"

                'ドロップダウンリスト設定(TBOX機種マスタ)(削除済みデータも取得)
                Me.ddlSystem.Items.Clear()
                Me.ddlSystem.DataSource = objDs.Tables(0)
                Me.ddlSystem.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.DataBind()
                Me.ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                clsMst.psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    clsMst.psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

#End Region

    Private Sub grdList_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowCreated
        'GridViewはタイトル行をthaed要素にする
        Select Case e.Row.RowType
            Case DataControlRowType.Header
                e.Row.TableSection = TableRowSection.TableHeader
            Case DataControlRowType.DataRow
                e.Row.TableSection = TableRowSection.TableBody
            Case DataControlRowType.Footer
                e.Row.TableSection = TableRowSection.TableFooter
        End Select
    End Sub


#End Region


End Class
