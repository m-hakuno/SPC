'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　集信エラーホール一覧
'*　ＰＧＭＩＤ：　ERRSELP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.17　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

'排他制御用
Imports SPC.ClsCMExclusive

#End Region

Public Class ERRSELP001
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
    Private Const M_DISP_ID = P_FUN_ERR & P_SCR_SEL & P_PAGE & "001"

    Private Const M_TROUBLE_ID = P_FUN_REQ & P_SCR_SEL & P_PAGE & "001"

    '次画面ファイルパス(ホールマスタ管理)
    Const M_NEXT_DISP_HALL = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006" & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx"

    '次画面ファイルパス(トラブル処理票)
    Const M_NEXT_DISP_TROUBLE = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"

    Private Const M_FST_KYE = "初期キー"

    'REQSELP001

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
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

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
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(Me.grvList, M_DISP_ID, "L")

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strKanriNum As String = Nothing
        Dim strTyousaDate As String = Nothing

        'ボタンアクションの設定
        AddHandler Me.btnTrouble.Click, AddressOf btn_Click
        AddHandler Me.btnUpdata.Click, AddressOf btn_Click
        AddHandler Master.ppLeftButton1.Click, AddressOf btn_Click
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If Not IsPostBack Then  '初回表示

            Try

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                '画面初期化処理
                msClearScreen()

                'セッション変数の取得/明細情報検索
                If ms_GetSession(strKanriNum, strTyousaDate) Then

                    Try

                        ms_GetNoResponse(strKanriNum, strTyousaDate)

                    Catch ex As Exception
                        '--------------------------------
                        '2014/06/12 後藤　ここから
                        '--------------------------------
                        ''システムエラー
                        'psMesBox(Me, "30008", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
                        '--------------------------------
                        '2014/06/12 後藤　ここまで
                        '--------------------------------
                        '--------------------------------
                        '2014/04/14 星野　ここから
                        '--------------------------------
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '--------------------------------
                        '2014/04/14 星野　ここまで
                        '--------------------------------
                        '--------------------------------
                        '2014/06/11 後藤　ここから
                        '--------------------------------
                        '排他削除
                        clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                        '--------------------------------
                        '2014/06/11 後藤　ここまで
                        '--------------------------------
                        psClose_Window(Me)
                        Exit Sub

                    End Try

                Else

                    'システムエラー
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    '--------------------------------
                    '2014/06/11 後藤　ここから
                    '--------------------------------
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                    '--------------------------------
                    '2014/06/11 後藤　ここまで
                    '--------------------------------
                    psClose_Window(Me)
                    Exit Sub

                End If

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '--------------------------------
                '2014/06/11 後藤　ここから
                '--------------------------------
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)
                '--------------------------------
                '2014/06/11 後藤　ここまで
                '--------------------------------
                psClose_Window(Me)
                Exit Sub

            End Try

        End If

    End Sub

    '------------------------------------
    '2014/5/9 稲葉 ここから
    '------------------------------------
    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        'ヘッダー、フッターは処理なし
        If e.Row.RowIndex = -1 Then
            Exit Sub
        End If

        Dim strTellNum As String = String.Empty           '電話番号
        Dim ctrl As Control = e.Row.Cells(27).Controls(0)

        If ctrl.GetType Is GetType(Label) Then
            '電話番号を取得する
            strTellNum = DirectCast(ctrl, Label).Text
            If strTellNum = String.Empty Then
                '電話番号がなければTEL発信ボタン使えません
                e.Row.Cells(15).Enabled = False
            Else
                'Javascript埋め込み
                Dim js_exe As String = String.Empty
                js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + ");"
                '                e.Row.Cells(15).Attributes.Add("onclick", pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe)
                e.Row.Cells(1).Attributes.Add("onclick", pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTellNum).Replace(";", "") + "&&" + js_exe)
            End If
        End If
    End Sub

    '------------------------------------
    '2014/5/9 稲葉 ここまで
    '------------------------------------

#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            Select Case sender.text

                Case "トラブル処理票"
                    '----20140731   武   from
                    'ms_chackValid()

                    'If (Page.IsValid) Then
                    '----20140731   武   to
                    Dim strView_st() As String = Me.ViewState(P_KEY)      'キー項目の設定

                    '次画面引継ぎ用キー情報設定
                    strKeyList = New List(Of String)

                    '新管理番号の有無で表示画面の設定を変更する(0:新管理番号無し)
                    Select Case strView_st(6)
                        Case "0"

                            'strKeyList.Add(M_DISP_ID)                                '画面ID
                            'strKeyList.Add("0")                                      'トラブル処理票管理番号有無フラグ(なし)
                            strKeyList.Add("0")                                       'トラブル処理票管理番号
                            strKeyList.Add(strView_st(0))                             '管理番号
                            strKeyList.Add(strView_st(1))                             '連番
                            strKeyList.Add(strView_st(2))                             'ＮＬ区分
                            strKeyList.Add(strView_st(3))                             'ID_IC区分
                            strKeyList.Add(strView_st(4))                             'データ受信日
                            strKeyList.Add(strView_st(5))                             '受信連番
                            strKeyList.Add(strView_st(7))                             'ＴＢＯＸＩＤ
                            'strKeyList.Add(Me.txtTyousaDay.ppText _
                            '             + " " _
                            '             + Me.txtTyousaTime.ppHourText _
                            '             + ":" _
                            '             + Me.txtTyousaTime.ppMinText _
                            '             + ":00")                                     '調査日時
                            'strKeyList.Add(Me.ddlList.SelectedValue)                  '担当者
                            'strKeyList.Add(Me.txtTousaResult.ppText)                  '調査結果

                            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

                        Case Else

                            Dim strExclusiveDate As String = Nothing
                            Dim arTable_Name As New ArrayList
                            Dim arKey As New ArrayList

                            '★ロック対象テーブル名の登録
                            arTable_Name.Insert(0, "D73_TROUBLE")

                            '★ロックテーブルキー項目の登録(D73_TROUBLE)
                            arKey.Insert(0, strView_st(6))

                            '★排他情報確認処理(更新画面へ遷移)
                            If clsExc.pfSel_Exclusive(strExclusiveDate _
                                             , Me _
                                             , Session(P_SESSION_IP) _
                                             , Session(P_SESSION_PLACE) _
                                             , Session(P_SESSION_USERID) _
                                             , Session(P_SESSION_SESSTION_ID) _
                                             , ViewState(P_SESSION_GROUP_NUM) _
                                             , M_TROUBLE_ID _
                                             , arTable_Name _
                                             , arKey) = 0 Then


                                'strKeyList.Add(M_DISP_ID)                                '画面ID
                                'strKeyList.Add("1")                                      'トラブル処理票管理番号有無フラグ
                                strKeyList.Add(strView_st(6))                             'トラブル処理票管理番号
                                strKeyList.Add(strView_st(0))                             '管理番号
                                strKeyList.Add(strView_st(1))                             '連番
                                strKeyList.Add(strView_st(2))                             'ＮＬ区分
                                strKeyList.Add(strView_st(3))                             'ID_IC区分
                                strKeyList.Add(strView_st(4))                             'データ受信日
                                strKeyList.Add(strView_st(5))                             '受信連番
                                strKeyList.Add(strView_st(7))                             'ＴＢＯＸＩＤ
                                'strKeyList.Add(Me.txtTyousaDay.ppText _
                                '             + " " _
                                '             + Me.txtTyousaTime.ppHourText _
                                '             + ":" _
                                '             + Me.txtTyousaTime.ppMinText _
                                '             + ":00")                                     '調査日時
                                'strKeyList.Add(Me.ddlList.SelectedValue)                  '担当者
                                'strKeyList.Add(Me.txtTousaResult.ppText)                  '調査結果

                                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                                '★登録年月日時刻
                                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                            Else

                                '排他ロック中
                                Exit Sub

                            End If

                    End Select

                    'セッション情報設定
                    '--------------------------------
                    '2014/04/17 高松　ここから
                    '--------------------------------
                    Session(P_SESSION_OLDDISP) = M_DISP_ID
                    '--------------------------------
                    '2014/04/17 高松　ここまで
                    '--------------------------------
                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = strKeyList.ToArray

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
                                    objStack.GetMethod.Name, M_NEXT_DISP_TROUBLE, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    'トラブル処理票
                    psOpen_Window(Me, M_NEXT_DISP_TROUBLE)

                    'トラブル処理票ボタンを非活性化
                    '----20140731   武   from
                    'Me.btnTrouble.Enabled = False
                    'End If
                    '----20140731   武   to
                Case "更新"

                    ms_chackValid()

                    If (Page.IsValid) Then

                        Dim intRowNum As Integer = Me.ViewState("行番号")   '選択行の設定
                        Dim strView_st() As String = Me.ViewState(P_KEY)    'キー項目の設定
                        Dim intIndex As Integer = Me.ddlList.SelectedIndex

                        '項目の更新
                        Try

                            ms_UpdNoResponse()

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
                            '処理終了
                            Exit Sub

                        End Try

                        '項目の初期化
                        'msClearScreen()

                        '再検索
                        Try

                            Dim strkey As String()

                            strkey = ViewState(M_FST_KYE)

                            ms_GetNoResponse(strkey(0), strkey(1)) '照会管理番号,データ受信日

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
                            '処理終了
                            Exit Sub

                        End Try

                        'グリッドビューの選択行を設定
                        Me.grvList.SelectedIndex = intRowNum
                        'ドロップダウンリストの再選択
                        Me.ddlList.SelectedIndex = intIndex

                    End If

                Case "ＣＳＶ"

                    Dim dsCSV As DataSet = New DataSet("dtCSV")
                    Dim dtCSV As DataTable = pfParse_DataTable(Me.grvList)     'CSV用データテーブル
                    Dim dirPath As String = ms_GetDirpath()
                    Dim strView_st() As String = Me.ViewState(P_KEY)           'キー項目の設定 

                    If Not dirPath Is Nothing Then

                        Dim fileName As String = Nothing
                        Dim CrateDate As String = DateTime.Now.ToString("yyyyMMddHHmmss")

                        'ファイル名作成
                        fileName = "集信エラーホール一覧_" _
                                 + strView_st(0) _
                                 + "_" _
                                 + CrateDate _
                                 + ".csv"

                        'ダウンロードファイルテーブルの更新
                        Try

                            ms_InsDownload(strView_st(0) _
                                         , "58" _
                                         , fileName _
                                         , dirPath _
                                         , CrateDate _
                                         , "集信エラーホール一覧_")

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
                            '処理終了
                            Exit Sub

                        End Try

                        'CSV出力
                        pfCreateCsvFile(dirPath, fileName, dsCSV, False)
                    Else

                        '処理終了
                        Exit Sub

                    End If

            End Select

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
            '処理終了
            Exit Sub

        End Try

    End Sub

    ''' <summary>
    ''' グリッドボタン操作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Select Case e.CommandName

            Case "btnSelect"
            Case "btnCallTEL"
            Case "btnMastMng"
            Case Else
                Exit Sub

        End Select

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        Select Case e.CommandName

            Case "btnSelect"     '選択

                '★排他制御用の変数
                Dim strExclusiveDate As String = String.Empty
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★項目の初期化
                Me.lblRecvDay_Input.Text = Nothing
                'Me.lblRecvTime_Input.Text = Nothing
                Me.lblTboxID_Input.Text = Nothing
                Me.lblHallName_Input.Text = Nothing
                Me.lblVer_Input.Text = Nothing
                Me.lblAddress_Input.Text = Nothing
                Me.txtTyousaDay.ppText = Nothing
                Me.txtTyousaTime.ppHourText = Nothing
                Me.txtTyousaTime.ppMinText = Nothing
                Me.ddlList.SelectedValue = Nothing
                Me.txtTousaResult.ppText = Nothing
                Me.lblInfo_Input.Text = Nothing

                Dim strUpdKey(8 - 1) As String
                Dim strBtnFlag As String = Me.ViewState(P_SESSION_TERMS)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                objStack = New StackFrame
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

                Select Case strBtnFlag

                    Case ClsComVer.E_遷移条件.参照

                        Try

                            Me.ddlList.SelectedValue = CType(rowData.FindControl("担当者コード"), TextBox).Text

                        Catch ex As Exception

                            'マスタ情報が存在しない
                            psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                            '--------------------------------
                            '2014/04/14 星野　ここから
                            '--------------------------------
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            '--------------------------------
                            '2014/04/14 星野　ここまで
                            '--------------------------------
                            Exit Sub

                        End Try

                        'ビューステートの設定
                        Me.ViewState("行番号") = intIndex

                        '更新項目に表示
                        '検知日時の有無で処理を切り分ける
                        'If CType(rowData.FindControl("検知日時"), TextBox).Text.Length >= 19 Then

                        '    Me.lblRecvDay_Input.Text = CType(rowData.FindControl("検知日時"), TextBox).Text.Substring(0, 10)
                        '    Me.lblRecvTime_Input.Text = CType(rowData.FindControl("検知日時"), TextBox).Text.Substring(11, 8)

                        'End If
                        Me.lblRecvDay_Input.Text = CType(rowData.FindControl("受信日"), TextBox).Text
                        If Me.lblRecvDay_Input.Text <> String.Empty Then
                            Me.lblRecvDay_Input.Text &= " " & CType(rowData.FindControl("受信時間"), TextBox).Text
                        Else
                            Me.lblRecvDay_Input.Text = CType(rowData.FindControl("受信時間"), TextBox).Text
                        End If
                        'Me.lblRecvDay_Input.Text = CType(rowData.FindControl("受信日"), TextBox).Text
                        'Me.lblRecvTime_Input.Text = CType(rowData.FindControl("受信時間"), TextBox).Text

                        Me.lblTboxID_Input.Text = CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text
                        Me.lblHallName_Input.Text = CType(rowData.FindControl("ホール名"), TextBox).Text
                        Me.lblSystem_Input.Text = CType(rowData.FindControl("システム"), TextBox).Text
                        Me.lblVer_Input.Text = CType(rowData.FindControl("バージョン"), TextBox).Text
                        Me.lblAddress_Input.Text = CType(rowData.FindControl("住所"), TextBox).Text
                        Me.lblINSNum_Input.Text = CType(rowData.FindControl("ＩＮＳ回線"), TextBox).Text

                        '調査日時の有無で処理を切り分ける
                        If CType(rowData.FindControl("調査日時"), TextBox).Text.Length >= 19 Then

                            Me.txtTyousaDay.ppText = CType(rowData.FindControl("調査日時"), TextBox).Text.Substring(0, 10)
                            Me.txtTyousaTime.ppHourText = CType(rowData.FindControl("調査日時"), TextBox).Text.Substring(11, 2)
                            Me.txtTyousaTime.ppMinText = CType(rowData.FindControl("調査日時"), TextBox).Text.Substring(14, 2)

                        End If

                        Me.txtTousaResult.ppText = CType(rowData.FindControl("調査結果"), TextBox).Text
                        Me.lblInfo_Input.Text = CType(rowData.FindControl("注意情報"), TextBox).Text


                    Case ClsComVer.E_遷移条件.更新

                        '★排他情報削除
                        If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

                            clsExc.pfDel_Exclusive(Me _
                                          , Session(P_SESSION_SESSTION_ID) _
                                          , Me.Master.ppExclusiveDateDtl)

                            Me.Master.ppExclusiveDateDtl = String.Empty

                        End If

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D183_HEALTH_NOREPLY")

                        '★ロックテーブルキー項目の登録(D168_JIKANGAI)
                        arKey.Insert(0, CType(rowData.FindControl("照会管理番号"), Label).Text)    'D183_CTRL_NO
                        arKey.Insert(1, CType(rowData.FindControl("連番"), Label).Text)            'D183_SEQ
                        arKey.Insert(2, CType(rowData.FindControl("ＮＬ区分"), Label).Text)        'D183_NL_CLS
                        arKey.Insert(3, CType(rowData.FindControl("ID_IC区分"), Label).Text)       'D183_ID_IC_CLS
                        arKey.Insert(4, CType(rowData.FindControl("データ受信日"), Label).Text)    'D183_RECVDATE
                        arKey.Insert(5, CType(rowData.FindControl("受信連番"), Label).Text)        'D183_RECVSEQ

                        '★排他情報確認処理(更新処理の実行)
                        If clsExc.pfSel_Exclusive(strExclusiveDate _
                                         , Me _
                                         , Session(P_SESSION_IP) _
                                         , Session(P_SESSION_PLACE) _
                                         , Session(P_SESSION_USERID) _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , ViewState(P_SESSION_GROUP_NUM) _
                                         , M_DISP_ID _
                                         , arTable_Name _
                                         , arKey) = 0 Then


                            Try

                                Me.ddlList.SelectedValue = CType(rowData.FindControl("担当者コード"), Label).Text

                            Catch ex As Exception

                                'マスタ情報が存在しない
                                psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                                '--------------------------------
                                '2014/04/14 星野　ここから
                                '--------------------------------
                                'ログ出力
                                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                                '--------------------------------
                                '2014/04/14 星野　ここまで
                                '--------------------------------

                                '★排他情報削除
                                clsExc.pfDel_Exclusive(Me _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , Me.Master.ppExclusiveDateDtl)

                                Me.Master.ppExclusiveDateDtl = String.Empty

                                Exit Sub

                            End Try

                            'ビューステートの設定
                            Me.ViewState("行番号") = intIndex

                            '更新項目に表示
                            '検知日時の有無で処理を切り分ける
                            'If CType(rowData.FindControl("検知日時"), TextBox).Text.Length >= 19 Then

                            '    Me.lblRecvDay_Input.Text = CType(rowData.FindControl("検知日時"), TextBox).Text.Substring(0, 10)
                            '    Me.lblRecvTime_Input.Text = CType(rowData.FindControl("検知日時"), TextBox).Text.Substring(11, 8)

                            'End If
                            Me.lblRecvDay_Input.Text = CType(rowData.FindControl("受信日"), Label).Text
                            If Me.lblRecvDay_Input.Text <> String.Empty Then
                                Me.lblRecvDay_Input.Text &= " " & CType(rowData.FindControl("受信時間"), Label).Text
                            Else
                                Me.lblRecvDay_Input.Text = CType(rowData.FindControl("受信時間"), Label).Text
                            End If
                            'Me.lblRecvDay_Input.Text = CType(rowData.FindControl("受信日"), TextBox).Text
                            'Me.lblRecvTime_Input.Text = CType(rowData.FindControl("受信時間"), TextBox).Text

                            Me.lblTboxID_Input.Text = CType(rowData.FindControl("ＴＢＯＸＩＤ"), Label).Text
                            Me.lblHallName_Input.Text = CType(rowData.FindControl("ホール名"), Label).Text
                            Me.lblSystem_Input.Text = CType(rowData.FindControl("システム"), Label).Text
                            Me.lblVer_Input.Text = CType(rowData.FindControl("バージョン"), Label).Text
                            Me.lblAddress_Input.Text = CType(rowData.FindControl("住所"), Label).Text
                            Me.lblINSNum_Input.Text = CType(rowData.FindControl("ＩＮＳ回線"), Label).Text

                            '調査日時の有無で処理を切り分ける
                            If CType(rowData.FindControl("調査日時"), Label).Text.Length >= 19 Then
                                Me.txtTyousaDay.ppText = CType(rowData.FindControl("調査日時"), Label).Text.Substring(0, 10)
                                Me.txtTyousaTime.ppHourText = CType(rowData.FindControl("調査日時"), Label).Text.Substring(11, 2)
                                Me.txtTyousaTime.ppMinText = CType(rowData.FindControl("調査日時"), Label).Text.Substring(14, 2)
                            End If

                            Me.txtTousaResult.ppText = CType(rowData.FindControl("調査結果"), Label).Text
                            Me.lblInfo_Input.Text = CType(rowData.FindControl("注意情報"), Label).Text

                            '★登録年月日時刻(明細)に登録
                            Me.Master.ppExclusiveDateDtl = strExclusiveDate

                        Else

                            '排他ロック中
                            Exit Sub

                        End If

                        'ボタンの活性化
                        Me.btnTrouble.Enabled = True
                        Me.btnUpdata.Enabled = True
                        Me.ddlList.Enabled = True
                        Me.txtTousaResult.ppEnabled = True
                        Me.txtTyousaDay.ppEnabled = True
                        Me.txtTyousaTime.ppEnabled = True

                End Select

                '更新キー情報の設定
                strUpdKey(0) = CType(rowData.FindControl("照会管理番号"), Label).Text
                strUpdKey(1) = CType(rowData.FindControl("連番"), Label).Text
                strUpdKey(2) = CType(rowData.FindControl("ＮＬ区分"), Label).Text
                strUpdKey(3) = CType(rowData.FindControl("ID_IC区分"), Label).Text
                strUpdKey(4) = CType(rowData.FindControl("データ受信日"), Label).Text
                strUpdKey(5) = CType(rowData.FindControl("受信連番"), Label).Text
                strUpdKey(6) = CType(rowData.FindControl("トラブル処理票管理番号"), Label).Text
                strUpdKey(7) = CType(rowData.FindControl("ＴＢＯＸＩＤ"), Label).Text

                'フォーカスを当てる
                Me.txtTyousaDay.ppDateBox.Focus()
                'ビューステートの設定
                Me.ViewState.Add(P_KEY, strUpdKey)
                'フォーカス設定
                Me.txtTyousaDay.ppDateBox.Focus()

                'Case "btnCallTEL"    'TEL発信

                '    Dim strCallTell As String = "callto:"
                '    strCallTell = strCallTell + CType(rowData.FindControl("ホール名"), TextBox).Text

                '    '--------------------------------
                '    '2014/04/16 星野　ここから
                '    '--------------------------------
                '    '■□■□結合試験時のみ使用予定□■□■
                '    Dim objStack As New StackFrame
                '    Dim strPrm As String = ""
                '    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                '    Dim tmp As Object() = Session(P_KEY)
                '    If Not tmp Is Nothing Then
                '        For zz = 0 To tmp.Length - 1
                '            If zz <> tmp.Length - 1 Then
                '                strPrm &= tmp(zz).ToString & ","
                '            Else
                '                strPrm &= tmp(zz).ToString
                '            End If
                '        Next
                '    End If

                '    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                '                    objStack.GetMethod.Name, strCallTell, strPrm, "TRANS")
                '    '■□■□結合試験時のみ使用予定□■□■
                '    '--------------------------------
                '    '2014/04/16 星野　ここまで
                '    '--------------------------------
                '    Response.Redirect(strCallTell)

            Case "btnMastMng"    'ホールマスタ管理

                '次画面引継ぎ用キー情報設定
                strKeyList = New List(Of String)
                'セッション情報設定																	
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                strKeyList.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), Label).Text)
                strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), Label).Text)
                strKeyList.Add(CType(rowData.FindControl("ID_IC区分"), Label).Text)
                strKeyList.Add(CType(rowData.FindControl("ホールコード"), Label).Text)

                '遷移条件
                Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

                'セッション情報設定																	
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                Session(P_KEY) = strKeyList.ToArray

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
                                objStack.GetMethod.Name, M_NEXT_DISP_HALL, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                'ホールマスタ管理
                psOpen_Window(Me, M_NEXT_DISP_HALL)

        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' セッション情報取得
    ''' </summary>
    ''' <param name="KanriNum"></param>
    ''' <param name="TyousaDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetSession(ByRef KanriNum As String, ByRef TyousaDate As String) As Boolean

        Dim strKey() As String = Session(P_KEY)
        Dim strBtnFlag As String = Session(P_SESSION_TERMS)

        If strKey Is Nothing Then

            Return False

        End If

        If Not strKey.Count = 3 Then

            Return False

        End If

        If strBtnFlag Is Nothing Then

            Return False

        End If

        '★排他情報用のグループ番号保管
        If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

        End If

        '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
        If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

            Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

        End If

        KanriNum = strKey(0)                                  '照会管理番号
        TyousaDate = strKey(1)                                'データ受信日時
        ViewState(M_FST_KYE) = strKey
        Me.lblSyokaiDate_Input.Text = strKey(2).ToString      '照会日時

        Me.ViewState.Add(P_KEY, Session(P_KEY)) 'ビューステートに保管
        Me.ViewState.Add(P_SESSION_TERMS, Session(P_SESSION_TERMS)) 'ビューステートに保管

        Return True

    End Function

    ''' <summary>
    ''' 画面項目初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '表示項目の初期化
        Me.lblSyokaiDate_Input.Text = Nothing
        Me.lblRecvDay_Input.Text = Nothing
        'Me.lblRecvTime_Input.Text = Nothing
        Me.lblTboxID_Input.Text = Nothing
        Me.lblHallName_Input.Text = Nothing
        Me.lblSystem_Input.Text = Nothing
        Me.lblVer_Input.Text = Nothing
        Me.lblAddress_Input.Text = Nothing
        Me.lblINSNum_Input.Text = Nothing
        Me.lblInfo_Input.Text = Nothing
        Me.txtTyousaDay.ppText = Nothing
        Me.txtTyousaTime.ppHourText = Nothing
        Me.txtTyousaTime.ppMinText = Nothing
        Me.txtTousaResult.ppText = Nothing
        Me.ddlList.Items.Clear()
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        'ボタンの設定
        'Master.ppLeftButton1.Visible = True
        Master.ppLeftButton1.Text = "ＣＳＶ"
        Me.btnTrouble.CausesValidation = True
        Me.btnUpdata.CausesValidation = True
        Master.ppLeftButton1.CausesValidation = False
        Me.btnTrouble.ValidationGroup = "1"
        Me.btnUpdata.ValidationGroup = "1"
        Master.ppLeftButton1.ValidationGroup = "1"
        Me.btnUpdata.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "集信エラー情報")
        Me.btnTrouble.OnClientClick = pfGet_OCClickMes("30004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票")
        Master.ppLeftButton1.OnClientClick = pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "CSV[集信エラー情報一覧]")

        'コントロールの非活性
        Me.btnTrouble.Enabled = False
        Me.btnUpdata.Enabled = False
        Me.txtTyousaDay.ppEnabled = False
        Me.txtTyousaTime.ppEnabled = False
        Me.ddlList.Enabled = False
        Me.txtTousaResult.ppEnabled = False

        'リストボックスの初期設定
        ms_GetEmployee()

    End Sub

    ''' <summary>
    ''' 社員名取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetEmployee()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得データをバインド
                Me.ddlList.DataSource = dstOrders.Tables(0)

                Me.ddlList.DataValueField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlList.DataTextField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlList.DataBind()

                '一行目を設定
                Me.ddlList.Items.Insert(0, "")
                Me.ddlList.SelectedIndex = 0

                '非活性化
                Me.ddlList.Enabled = False

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "社員情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)

        End If

    End Sub

    ''' <summary>
    ''' 明細情報の検索
    ''' </summary>
    ''' <param name="strKanriNum"></param>
    ''' <param name="strTyousaDate"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetNoResponse(strKanriNum As String _
                               , strTyousaDate As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                'ヘルスチェック無応答
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_noreply_no", SqlDbType.NVarChar, strKanriNum))                 '管理番号
                    .Add(pfSet_Param("prm_recv_date", SqlDbType.NVarChar, strTyousaDate))                '受信日
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                        Throw New Exception

                    Case Else        'データ有り

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = dstOrders.Tables(0)
                        '変更を反映
                        Me.grvList.DataBind()

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラーホール情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Catch ex As Exception

                '--------------------------------
                '2014/06/09 後藤　ここから
                '--------------------------------
                ''システムエラー
                'psMesBox(Me, "00004", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "時間外消費")
                If strOKNG = "0" Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Else
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
                End If
                '--------------------------------
                '2014/06/09 後藤　ここまで
                '--------------------------------
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                '結果を返す
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)

        End If

    End Sub

    ''' <summary>
    ''' 無応答テーブル更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_UpdNoResponse()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strView_st() As String = Me.ViewState(P_KEY)   'キー項目の設定
        Dim strOKNG As String = Nothing                    '検索結果
        Dim datInvst_Dt As String = String.Empty
        Dim dt As DateTime
        If DateTime.TryParse(Me.txtTyousaDay.ppText + " " + Me.txtTyousaTime.ppHourText + ":" + Me.txtTyousaTime.ppMinText + ":00", dt) Then
            datInvst_Dt = DateTime.ParseExact(Me.txtTyousaDay.ppText _
                                            + " " _
                                            + Me.txtTyousaTime.ppHourText _
                                            + ":" _
                                            + Me.txtTyousaTime.ppMinText _
                                            + ":00", "yyyy/MM/dd HH:mm:ss", Nothing).ToString '日時に変換する
        End If
        Dim RecvDate As String = strView_st(4)
        Dim intRtn As Integer
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try



                cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_ctrl_no", SqlDbType.NVarChar, strView_st(0)))                       '管理番号
                    .Add(pfSet_Param("prm_seq", SqlDbType.NVarChar, strView_st(1)))                           '連番
                    .Add(pfSet_Param("prm_nl_cls", SqlDbType.NVarChar, strView_st(2)))                        'ＮＬ区分
                    .Add(pfSet_Param("prm_id_ic_cls", SqlDbType.NVarChar, strView_st(3)))                     'ID_IC区分
                    .Add(pfSet_Param("prm_recvdate", SqlDbType.NVarChar, RecvDate.Substring(0, 10).Replace("/", "")))      'データ受信日
                    .Add(pfSet_Param("prm_recvseq", SqlDbType.NVarChar, strView_st(5)))                       '受信連番
                    .Add(pfSet_Param("prm_invest_dt", SqlDbType.NVarChar, datInvst_Dt))                       '調査日時 YYYY/MM/DD HH:MM:00
                    .Add(pfSet_Param("prm_charge_cd", SqlDbType.NVarChar, Me.ddlList.SelectedValue))          '対応者コード
                    .Add(pfSet_Param("prm_invest", SqlDbType.NVarChar, Me.txtTousaResult.ppText))             '調査結果
                    .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))           'ログインユーザ
                    .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, strView_st(7)))                        'TBOXID
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                         '戻り値
                End With

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー情報")
                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()
                End Using

                '更新が正常終了
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "集信エラー情報")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー情報")
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
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信エラー情報")
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
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリストの選択行参照
    ''' </summary>
    ''' <param name="p1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetSelectIndex(p1 As String) As Integer

        Dim selIndex As Integer = 0

        For Each lstItem As ListItem In Me.ddlList.Items

            ' value が 用途コードと一致する
            If (lstItem.Value = p1) Then
                Return selIndex
                Exit Function
            End If

            selIndex += 1

        Next

        Return 0

    End Function

    ''' <summary>
    ''' CSV出力先取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetDirpath() As String

        Dim strPath As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand("ZCMPSEL009", conDB)

                'CSV出力先
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("FILECLASS_CD", SqlDbType.NVarChar, "10"))                              'ファイル種別
                    .Add(pfSet_Param("PLACE_CLS", SqlDbType.NVarChar, "2"))                                  '場所(SPC)
                    .Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.NVarChar, 20, ParameterDirection.Output))   'アドレス
                    .Add(pfSet_Param("FOLDER_NM", SqlDbType.NVarChar, 20, ParameterDirection.Output))        'フォルダ名
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                strPath = "\\" + cmdDB.Parameters("SERVER_ADDRESS").Value.ToString + cmdDB.Parameters("FOLDER_NM").Value.ToString

                Return strPath

            Catch ex As SqlException

                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保存先パス")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return Nothing

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保存先パス")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return Nothing

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Return Nothing

        End If


    End Function

    ''' <summary>
    ''' ダウンロードファイルテーブルの登録
    ''' </summary>
    ''' <param name="KanriNum"></param>
    ''' <param name="DispNum"></param>
    ''' <remarks></remarks>
    Private Sub ms_InsDownload(ByVal KanriNum As String _
                             , ByVal DispNum As String _
                             , ByVal fileName As String _
                             , ByVal dirPath As String _
                             , ByVal CrateDate As String _
                             , ByVal ReportName As String)

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strOKNG As String = Nothing                    '検索結果
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                cmdDB = New SqlCommand("ZCMPINS001", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    '照会要求親データ
                    .Add(pfSet_Param("MNG_NO", SqlDbType.NVarChar, KanriNum))                               '管理番号
                    .Add(pfSet_Param("FILE_CLS", SqlDbType.NVarChar, DispNum))                              'ファイル種別
                    .Add(pfSet_Param("TITLE", SqlDbType.NVarChar, Master.ppTitle))                          '画面タイトル
                    .Add(pfSet_Param("FILE_NM", SqlDbType.NVarChar, fileName))                              'ファイル名
                    .Add(pfSet_Param("REPORT_NM", SqlDbType.NVarChar, ReportName + CrateDate))              '帳票名(名称_yyyyMMddHHmmss)
                    .Add(pfSet_Param("KEEP_FOLD", SqlDbType.NVarChar, dirPath))                             '保存先フォルダ
                    .Add(pfSet_Param("CREATE_DT", SqlDbType.DateTime, Date.Parse(CrateDate)))                           '作成日
                    .Add(pfSet_Param("INSERT_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID).ToString)) 'ユーザＩＤ
                End With

                'データ追加／更新
                Using conTrn = conDB.BeginTransaction
                    cmdDB.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

                '更新が正常終了
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
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
            Catch ex As Exception
                'システムエラー
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ダウンロードファイル")
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
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If


    End Sub

#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' 検証チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_chackValid()


        'If Me.ddlList.SelectedIndex = 0 Then

        '    vldDdlList.Text = "未入力エラー"
        '    vldDdlList.ErrorMessage = "担当者 が選択されていません。"
        '    vldDdlList.Enabled = True
        '    vldDdlList.IsValid = False
        '    vldDdlList.Focus()

        'End If

        If Me.txtTyousaDay.ppText = String.Empty And (Me.txtTyousaTime.ppHourText <> String.Empty And Me.txtTyousaTime.ppMinText <> String.Empty) Then
            Me.txtTyousaDay.psSet_ErrorNo("4012", "調査日時")
        End If

        If Me.txtTyousaDay.ppText <> String.Empty And (Me.txtTyousaTime.ppHourText = String.Empty And Me.txtTyousaTime.ppMinText = String.Empty) Then
            Me.txtTyousaTime.psSet_ErrorNo("4013", "調査日時")
        End If

        Dim strlen As String = Me.txtTousaResult.ppText
        If strlen.Length > 255 Then
            Me.txtTousaResult.psSet_ErrorNo("3002", Me.txtTousaResult.ppName, "調査結果", Me.txtTousaResult.ppMaxLength)
        End If

    End Sub

#End Region

End Class
