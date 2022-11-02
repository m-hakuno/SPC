'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　時間外消費ホール詳細
'*　ＰＧＭＩＤ：　OVELSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.14　：　ＮＫＣ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'OVELSTP002-0001    2016.04.15      伯野　     削除された担当者の情報を保持する
'OVELSTP002-002     2017.03.28      加賀       一部の画面制御・レイアウト変更 クリアボタン追加


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


Public Class OVELSTP002
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
    Private Const M_DISP_ID = P_FUN_OVE & P_SCR_LST & P_PAGE & "002"
    Private Const M_DISP_HALL_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "006"
    Private Const M_DISP_KANSHI_ID = P_FUN_WAT & P_SCR_UPD & P_PAGE & "001"

    '次画面ファイルパス(ホールマスタ管理)
    Const M_NEXT_DISP_HALL = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006" & "/" &
            M_DISP_HALL_ID & ".aspx"

    '次画面ファイルパス(監視報告書兼依頼書)
    Const M_NEXT_DISP_KANSHI = "~/" & P_WAT & "/" &
            P_FUN_WAT & P_SCR_UPD & P_PAGE & "001" & "/" &
            M_DISP_KANSHI_ID & ".aspx"

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
    ''' <remarks></remarks>
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strKanriNum As String = Nothing
        Dim strTyousaDate As String = Nothing

        'ボタンアクションの設定
        AddHandler Me.btnKanshi.Click, AddressOf btn_Click
        AddHandler Me.btnUpdata.Click, AddressOf btn_Click
        AddHandler Me.btnClear.Click, AddressOf btn_Click   'OVELSTP002-002

        objStack = New StackFrame

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

                        ms_GetJikangaiHall(strKanriNum, strTyousaDate)

                    Catch ex As Exception

                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        '排他削除
                        clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)

                        psClose_Window(Me)
                        Exit Sub

                    End Try

                Else

                    'システムエラー
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    '排他削除
                    clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)

                    psClose_Window(Me)
                    Exit Sub

                End If

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '排他削除
                clsExc.psExclusive_Del_Chk(Me, Me.Master.ppExclusiveDate, Me.Master.ppExclusiveDateDtl)

                psClose_Window(Me)
                Exit Sub

            End Try

        End If

    End Sub

    ''' <summary>
    ''' データバインド時処理 'OVELSTP002-002
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strTelNo As String = Nothing 'ホールTEL

        '削除フラグがある行の更新を非活性にする
        For Each rowData As GridViewRow In grvList.Rows

            strTelNo = CType(rowData.FindControl("ホールTEL"), TextBox).Text

            If strTelNo = String.Empty Then
                '電話番号がなければTEL発信ボタン使えません
                rowData.Cells(2).Enabled = False
            Else
                'CTI起動用Javascript埋め込み
                Dim js_exe As String = String.Empty
                js_exe = "exe_Cti(" + """" + strTelNo.Replace("-", "") + """" + ");"
                rowData.Cells(2).Attributes.Add("onclick", pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strTelNo).Replace(";", "") + "&&" + js_exe)
            End If
        Next


    End Sub


#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing                       '次画面引継ぎ用キー情報
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing

        objStack = New StackFrame

        Try

            Select Case sender.text

                Case "監視報告書兼依頼票"

                    Dim strView_st() As String = Me.ViewState(P_KEY)      'キー項目の設定

                    If clsDataConnect.pfOpen_Database(conDB) Then
                        Try
                            cmdDB = New SqlCommand("OVELSTP002_S4", conDB)
                            With cmdDB.Parameters
                                'パラメータ設定
                                .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strView_st(0)))
                                .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strView_st(7)))
                                .Add(pfSet_Param("seq", SqlDbType.NVarChar, strView_st(1)))
                            End With

                            'リストデータ取得
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        Catch ex As Exception
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費ホール")
                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            Exit Sub
                        Finally
                            'DB切断
                            If Not clsDataConnect.pfClose_Database(conDB) Then
                                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                            End If
                        End Try
                    Else
                        psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        Exit Sub
                    End If

                    '次画面引継ぎ用キー情報設定
                    strKeyList = New List(Of String)

                    '監視報告書の有無で表示画面の設定を変更する
                    If dstOrders Is Nothing OrElse dstOrders.Tables(0).Rows.Count = 0 Then

                        strKeyList.Add(M_DISP_ID)                                 '画面ID
                        strKeyList.Add("1")                                       '新管理番号有無フラグ
                        strKeyList.Add(strView_st(0))                             '管理番号
                        strKeyList.Add(strView_st(1))                             '連番
                        strKeyList.Add(strView_st(2))                             'ID_IC区分
                        strKeyList.Add(strView_st(3))                             'ＮＬ区分
                        strKeyList.Add(strView_st(4))                             '検知日時
                        strKeyList.Add(strView_st(5))                             '受信連番
                        strKeyList.Add("0")                                       '新管理番号
                        strKeyList.Add(strView_st(7))                             'TBOXID
                        strKeyList.Add(Me.ddlCharge.SelectedItem.ToString)          '対応者
                        strKeyList.Add(Me.txtTyousaDtl.ppText)                       '対応内容

                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

                    Else

                        Dim strExclusiveDate As String = Nothing
                        Dim arTable_Name As New ArrayList
                        Dim arKey As New ArrayList

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, "D36_MNTRREPORT")

                        '★ロックテーブルキー項目の登録(D36_MNTRREPORT_NO)
                        arKey.Insert(0, strView_st(6))

                        '★排他情報確認処理(更新画面へ遷移)
                        If clsExc.pfSel_Exclusive(strExclusiveDate _
                                         , Me _
                                         , Session(P_SESSION_IP) _
                                         , Session(P_SESSION_PLACE) _
                                         , Session(P_SESSION_USERID) _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , ViewState(P_SESSION_GROUP_NUM) _
                                         , M_DISP_KANSHI_ID _
                                         , arTable_Name _
                                         , arKey) = 0 Then

                            '遷移先へ受け渡す変数の設定
                            strKeyList.Add(M_DISP_ID)                                 '画面ID
                            strKeyList.Add("2")                                       '新管理番号有無フラグ
                            strKeyList.Add("0")                                       '管理番号
                            strKeyList.Add("0")                                       'キー連番
                            strKeyList.Add("0")                                       'ID_IC区分
                            strKeyList.Add("0")                                       'ＮＬ区分
                            strKeyList.Add("0")                                       'キー検知日時 YYYYMMDD
                            strKeyList.Add("0")                                       '受信連番
                            strKeyList.Add(dstOrders.Tables(0).Rows(0).Item("D36_MNTRREPORT_NO").ToString)                             '新管理番号
                            strKeyList.Add(strView_st(7))                             'TBOXID
                            strKeyList.Add(Me.ddlCharge.SelectedItem.ToString)          '対応者
                            strKeyList.Add(Me.txtTyousaDtl.ppText)                       '対応内容

                            '★登録年月日時刻
                            Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate

                        Else

                            '排他ロック中
                            Exit Sub

                        End If

                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                    End If

                    '画面遷移が参照である場合
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then

                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                    End If

                    'セッション情報設定																	
                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                    Session(P_KEY) = strKeyList.ToArray
                    Session(P_SESSION_OLDDISP) = M_DISP_ID   '遷移元ID

                    '★排他情報のグループ番号をセッション変数に設定
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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
                                    objStack.GetMethod.Name, M_NEXT_DISP_KANSHI, strPrm, "TRANS")

                    '監視報告書兼依頼票
                    psOpen_Window(Me, M_NEXT_DISP_KANSHI)

                    '監視報告書兼依頼票ボタンを非活性化
                    'Me.btnKanshi.Enabled = False

                Case "更新"

                    'Dim intRowNum As Integer = Me.ViewState("行番号")   '選択行の設定
                    Dim strView_st() As String = Me.ViewState(P_KEY)    'キー項目の設定
                    'Dim intIndex As Integer = Me.ddlList.SelectedIndex

                    If (Page.IsValid) Then

                        '項目の更新
                        Try

                            ms_UpdJikangai()

                        Catch ex As Exception

                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
                            '処理終了
                            Exit Sub

                        End Try

                        '項目の初期化
                        'msClearScreen()
                        ClearEditArea()

                        '再検索
                        Try

                            ms_GetJikangaiHall(strView_st(0), strView_st(4))

                        Catch ex As Exception

                            'ログ出力
                            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

                            '処理終了
                            Exit Sub

                        End Try

                        ''グリッドビューの選択行を設定
                        'Me.grvList.SelectedIndex = intRowNum
                        ''ドロップダウンリストの再選択
                        'Me.ddlList.SelectedIndex = intIndex

                    End If

                Case btnClear.Text  'OVELSTP002-002

                    ClearEditArea()

            End Select

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

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
                'Case "btnCallTEL"
            Case "btnMastMng"
            Case Else
                Exit Sub
        End Select

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報

        objStack = New StackFrame

        '開始ログ出力
        psLogStart(Me)

        Select Case e.CommandName

            Case "btnSelect"     '選択

                Dim strUpdKey(8 - 1) As String
                Dim strBtnFlag As ClsComVer.E_遷移条件 = Me.ViewState(P_SESSION_TERMS)

                '★排他制御用の変数
                Dim strExclusiveDate As String = String.Empty
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★項目の初期化
                Me.lblNum_Input.Text = String.Empty
                Me.lblKentiDate_Input.Text = String.Empty
                Me.lblNL_Input.Text = String.Empty
                Me.lblTboxID_Input.Text = String.Empty
                Me.lblHallName_Input.Text = String.Empty
                Me.lblHallTEL_Input.Text = String.Empty
                Me.lblJimisyoTEL_Input.Text = String.Empty
                Me.lblSystem_Input.Text = String.Empty
                Me.lblVer_Input.Text = String.Empty
                Me.ddlCharge.SelectedIndex = 0
                Me.txtTyousaDtl.ppText = String.Empty
                Me.lblInfo_Input.Text = String.Empty
                Me.btnKanshi.Enabled = True

                If strBtnFlag = ClsComVer.E_遷移条件.更新 Then

                    '★排他情報削除
                    If Not Me.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me _
                                      , Session(P_SESSION_SESSTION_ID) _
                                      , Me.Master.ppExclusiveDateDtl)

                        Me.Master.ppExclusiveDateDtl = String.Empty

                    End If

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D168_JIKANGAI")

                    '★ロックテーブルキー項目の登録(D168_JIKANGAI)
                    arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text)        'D168_CTRL_NO
                    arKey.Insert(1, CType(rowData.FindControl("キー連番"), TextBox).Text)        'D168_SEQ
                    arKey.Insert(2, CType(rowData.FindControl("ＮＬ区分"), TextBox).Text)        'D168_NL_CLS
                    arKey.Insert(3, CType(rowData.FindControl("ID_IC区分"), TextBox).Text)       'D168_ID_IC_CLS
                    arKey.Insert(4, CType(rowData.FindControl("キー検知日"), TextBox).Text)      'D168_RECVDATE
                    arKey.Insert(5, CType(rowData.FindControl("連番"), TextBox).Text)            'D168_RECVSEQ

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

                        '登録年月日時刻(明細)
                        Me.Master.ppExclusiveDate = strExclusiveDate

                    Else
                        '排他ロック中
                        Exit Sub
                    End If

                    'ビューステートの設定
                    Me.ViewState("行番号") = intIndex

                    '★登録年月日時刻(明細)に登録
                    Me.Master.ppExclusiveDateDtl = strExclusiveDate

                    'ボタンの活性化
                    Me.btnUpdata.Enabled = True
                    Me.ddlCharge.Enabled = True
                    Me.ddlResult.Enabled = True
                    Me.txtTyousaDtl.ppEnabled = True

                Else

                    Me.btnKanshi.Enabled = False

                End If

                Try

                    'OVELSTP002-0001 削除社員対策
                    If ddlCharge.Items.FindByValue(CType(rowData.FindControl("担当コード"), TextBox).Text) Is Nothing Then
                        ddlCharge.Items.Item(0).Value = CType(rowData.FindControl("担当コード"), TextBox).Text
                        ddlCharge.Items.Item(0).Text = CType(rowData.FindControl("担当者"), TextBox).Text
                        ddlCharge.SelectedIndex = 0
                    Else
                        Me.ddlCharge.SelectedValue = CType(rowData.FindControl("担当コード"), TextBox).Text
                    End If
                    'OVELSTP002-0001　END

                    Me.ddlResult.SelectedValue = CType(rowData.FindControl("定型文内容コード"), TextBox).Text

                Catch ex As Exception

                    'マスタ情報が存在しない
                    psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")

                    If strBtnFlag = ClsComVer.E_遷移条件.更新 Then
                        '排他情報解除
                        clsExc.pfDel_Exclusive(Me _
                                  , Session(P_SESSION_SESSTION_ID) _
                                  , Me.Master.ppExclusiveDateDtl)

                        Me.Master.ppExclusiveDateDtl = String.Empty
                    End If

                    Exit Sub

                End Try

                '更新項目に表示
                Me.lblNum_Input.Text = CType(rowData.FindControl("連番"), TextBox).Text
                Me.lblKentiDate_Input.Text = CType(rowData.FindControl("検知日時"), TextBox).Text
                Me.lblNL_Input.Text = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text
                Me.lblTboxID_Input.Text = CType(rowData.FindControl("TBOXID"), TextBox).Text
                'Me.lblHallName_Input.Text = CType(rowData.FindControl("ホール名"), TextBox).Text
                Me.lblHallName_Input.Text = CType(rowData.FindControl("ホール名_全文"), TextBox).Text
                Me.lblHallTEL_Input.Text = CType(rowData.FindControl("ホールTEL"), TextBox).Text
                Me.lblJimisyoTEL_Input.Text = CType(rowData.FindControl("事務所TEL"), TextBox).Text
                Me.lblSystem_Input.Text = CType(rowData.FindControl("システム"), TextBox).Text
                Me.lblVer_Input.Text = CType(rowData.FindControl("VER"), TextBox).Text
                Me.txtTyousaDtl.ppText = CType(rowData.FindControl("調査内容"), TextBox).Text
                'Me.lblInfo_Input.Text = CType(rowData.FindControl("注意事項"), TextBox).Text
                Me.lblInfo_Input.Text = CType(rowData.FindControl("注意事項_全文"), TextBox).Text

                '参照キー情報の設定
                strUpdKey(0) = CType(rowData.FindControl("管理番号"), TextBox).Text
                strUpdKey(1) = CType(rowData.FindControl("キー連番"), TextBox).Text
                strUpdKey(2) = CType(rowData.FindControl("ID_IC区分"), TextBox).Text
                strUpdKey(3) = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text
                strUpdKey(4) = CType(rowData.FindControl("キー検知日"), TextBox).Text
                strUpdKey(5) = CType(rowData.FindControl("連番"), TextBox).Text
                strUpdKey(6) = CType(rowData.FindControl("新管理番号"), TextBox).Text
                strUpdKey(7) = CType(rowData.FindControl("TBOXID"), TextBox).Text

                'ビューステートの設定
                Me.ViewState.Add(P_KEY, strUpdKey)

            Case "btnMastMng"    'ホールマスタ管理

                '次画面引継ぎ用キー情報設定
                strKeyList = New List(Of String)
                'セッション情報設定
                If Me.ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then

                    '★排他制御用の変数
                    Dim dtExclusiveDate As String = String.Empty
                    Dim arTable_Name As New ArrayList
                    Dim arKey As New ArrayList

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "T04_HALL_COMM")
                    arTable_Name.Insert(1, "T01_HALL")

                    '★ロックテーブルキー項目の登録(T04_HALL_COMM,T01_HALL)
                    arKey.Insert(0, CType(rowData.FindControl("ホールコード"), TextBox).Text)    'T04_HALL_CD
                    arKey.Insert(1, CType(rowData.FindControl("ホールコード"), TextBox).Text)    'T01_HALL_CD

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(dtExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_DISP_HALL_ID _
                                     , arTable_Name _
                                     , arKey) = 0 Then

                        '★登録年月日時刻
                        Session(P_SESSION_EXCLUSIV_DATE) = dtExclusiveDate

                    Else

                        '排他ロック中
                        Exit Sub

                    End If

                End If
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                strKeyList.Add(CType(rowData.FindControl("TBOXID"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ID_IC区分"), TextBox).Text)
                strKeyList.Add(CType(rowData.FindControl("ホールコード"), TextBox).Text)
                Session(P_SESSION_TERMS) = Me.ViewState(P_SESSION_TERMS)

                'セッション情報設定																	
                Session(P_SESSION_BCLIST) = Master.ppBcList_Text
                Session(P_KEY) = strKeyList.ToArray
                '★排他情報のグループ番号をセッション変数に設定
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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

                'ホールマスタ管理
                psOpen_Window(Me, M_NEXT_DISP_HALL)

        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '照会日時
        Me.lblSyokaiDate_Input.Text = String.Empty

        '更新項目の初期化
        ClearEditArea()

        '注意事項の折り返し設定
        lblInfo_Input.Style.Add("word-wrap", "break-word")

        'グリッドの初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        'ボタン押下時のメッセージ
        Me.btnUpdata.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "運用時間外使用情報")
        Me.btnKanshi.OnClientClick = pfGet_OCClickMes("30004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "監視報告書兼依頼票参照／更新")

        '★排他情報用コントロールの初期化
        Me.Master.ppExclusiveDate = String.Empty
        Me.Master.ppExclusiveDateDtl = String.Empty

        'リストボックスの初期設定
        ms_GetEmployee()
        'リストボックス(定型文)の初期設定
        ms_GetStatus()

    End Sub

    ''' <summary>
    ''' 編集エリア初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearEditArea()

        '更新項目の初期化
        Me.lblNum_Input.Text = String.Empty
        Me.lblKentiDate_Input.Text = String.Empty
        Me.lblNL_Input.Text = String.Empty
        Me.lblTboxID_Input.Text = String.Empty
        Me.lblHallName_Input.Text = String.Empty
        Me.lblHallTEL_Input.Text = String.Empty
        Me.lblJimisyoTEL_Input.Text = String.Empty
        Me.lblSystem_Input.Text = String.Empty
        Me.lblVer_Input.Text = String.Empty
        Me.txtTyousaDtl.ppText = String.Empty
        Me.lblInfo_Input.Text = String.Empty
        ddlCharge.SelectedIndex = -1
        ddlResult.SelectedIndex = -1

        '調査内容の非活性
        ddlCharge.Enabled = False
        ddlResult.Enabled = False
        Me.txtTyousaDtl.ppEnabled = False

        'ボタンの非活性
        Me.btnKanshi.Enabled = False
        Me.btnUpdata.Enabled = False

        '★排他情報削除
        If Not Me.Master.ppExclusiveDate = String.Empty Then
            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.ppExclusiveDate) = 0 Then
                Me.Master.ppExclusiveDate = String.Empty
            Else
                'ログ出力終了
                psLogEnd(Me)
                Exit Sub
            End If
        End If


    End Sub

    ''' <summary>
    ''' セッション情報を取得
    ''' </summary>
    ''' <param name="KanriNum"></param>
    ''' <param name="TyousaDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetSession(ByRef KanriNum As String _
                                 , ByRef TyousaDate As String) As Boolean

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

        TyousaDate = strKey(0)                                      '調査日時
        KanriNum = strKey(1)                                        '管理番号
        Me.lblSyokaiDate_Input.Text = strKey(2).ToString            '照会日時


        Me.ViewState.Add(P_KEY, Session(P_KEY))                     'ビューステートに保管
        Me.ViewState.Add(P_SESSION_TERMS, Session(P_SESSION_TERMS)) 'ビューステートに保管

        Return True

    End Function

    ''' <summary>
    ''' 明細情報の検索
    ''' </summary>
    ''' <param name="strKanriNum"></param>
    ''' <param name="strTyousaDate"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetJikangaiHall(ByVal strKanriNum As String _
                                 , ByVal strTyousaDate As String)

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

                '時間外情報
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_ctrl_num", SqlDbType.NVarChar, strKanriNum))                   '管理番号
                    .Add(pfSet_Param("prm_recv_date", SqlDbType.NVarChar, strTyousaDate))                '受信日(YYYYMMDD)
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
                End With

                'タイムアウト設定
                cmdDB.CommandTimeout = 180

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
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
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
    ''' 時間外消費ホール詳細の更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_UpdJikangai()

        Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strView_st() As String = Me.ViewState(P_KEY)   'キー項目の設定
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

                cmdDB = New SqlCommand(M_DISP_ID & "_U1", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    '照会要求親データ
                    .Add(pfSet_Param("prm_ctrl_no", SqlDbType.NVarChar, strView_st(0)))                '管理番号
                    .Add(pfSet_Param("prm_seq", SqlDbType.NVarChar, strView_st(1)))                    'キー連番
                    .Add(pfSet_Param("prm_id_ic_cls", SqlDbType.NVarChar, strView_st(2)))              'ID_IC区分
                    .Add(pfSet_Param("prm_nl_cls", SqlDbType.NVarChar, strView_st(3)))                 'ＮＬ区分
                    .Add(pfSet_Param("prm_recv_date", SqlDbType.NVarChar, strView_st(4)))              'キー検知日時 YYYYMMDD
                    .Add(pfSet_Param("prm_recv_seq", SqlDbType.NVarChar, strView_st(5)))               '受信連番
                    .Add(pfSet_Param("prm_charge_cd", SqlDbType.NVarChar, Me.ddlCharge.SelectedValue))   '対応者コード
                    .Add(pfSet_Param("prm_deal_cd", SqlDbType.NVarChar, Me.ddlResult.SelectedValue))   '定型文内容コード
                    .Add(pfSet_Param("prm_deal_dtl", SqlDbType.NVarChar, Me.txtTyousaDtl.ppText))         '対応内容
                    .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))    'ログインユーザ
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
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "運用時間外使用情報")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "運用時間外使用情報")
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
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
    ''' 社員名の取得
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
                Me.ddlCharge.DataSource = dstOrders.Tables(0)

                Me.ddlCharge.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlCharge.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlCharge.DataBind()

                '一行目を設定
                Me.ddlCharge.Items.Insert(0, "")
                Me.ddlCharge.SelectedIndex = 0

                '非活性化
                Me.ddlCharge.Enabled = False

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
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
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
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
    ''' 定型文の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetStatus()
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得データをバインド
                Me.ddlResult.DataSource = dstOrders.Tables(0)

                Me.ddlResult.DataTextField = dstOrders.Tables(0).Columns(0).ColumnName.ToString
                Me.ddlResult.DataValueField = dstOrders.Tables(0).Columns(1).ColumnName.ToString
                Me.ddlResult.DataBind()

                '一行目を設定
                Me.ddlResult.Items.Insert(0, "")
                Me.ddlResult.SelectedIndex = 0

                '非活性化
                Me.ddlResult.Enabled = False

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
                '結果を返す
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "時間外消費")
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

    '--------------------------------
    '2014/05/09 稲葉　ここから
    '--------------------------------
    ' ''' <summary>
    ' ''' データバインド時処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

    '    Dim strTellNum As String = String.Empty        '電話番号
    '    Dim strVs() As String = ViewState(P_KEY)
    '    Dim js_exe As String = String.Empty

    '    'CallToプロトコルの設定
    '    For Each rowData As GridViewRow In grvList.Rows
    '        strTellNum = CType(rowData.FindControl("ホールTEL"), TextBox).Text
    '        If strTellNum Is Nothing Or strTellNum = String.Empty Then

    '            rowData.Cells(20).Enabled = False

    '        Else

    '            js_exe = "exe_Cti(" + """" + strTellNum.Replace("-", "") + """" + "); return false;"
    '            rowData.Cells(20).Attributes.Add("onclick", js_exe)

    '        End If

    '    Next

    'End Sub
    '--------------------------------
    '2014/05/09 稲葉　ここまで
    '--------------------------------

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
