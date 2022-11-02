'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　ヘルスチェック一覧
'*　ＰＧＭＩＤ：　HEALSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　14.02.03　：　(NKC)浜本
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient
Imports SPC.ClsCMExclusive
#End Region

Public Class HEALSTP001
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

    ''' <summary>
    ''' プログラムＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_DISP_ID As String = P_FUN_HEA & P_SCR_LST & P_PAGE & "001"

    ''' <summary>
    ''' 次画面ＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_MY_DISP_ID_UPDP As String = P_FUN_HEA & P_SCR_UPD & P_PAGE & "001"

    ''' <summary>
    ''' リダイレクトパス(ヘルスチェック詳細/更新画面)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strREDIRECTPATH_HLTCHK As String = "~/" & P_WAT & "/" & P_FUN_HEA & P_SCR_UPD & P_PAGE & "001/" & P_FUN_HEA & P_SCR_UPD & P_PAGE & "001.aspx"

    ''' <summary>
    ''' リダイレクトパス(トラブル処理票画面)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strREDIRECTPATH_TRBL As String = "~/" & P_MAI & "/" & P_FUN_REQ & P_SCR_SEL & P_PAGE & "001/" & P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"

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
        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_DISP_ID, 40, 10)
    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'イベントハンドラセット
        Me.msSetHandler()

        'ポストバック？
        If Me.IsPostBack = False Then
            'ログ出力開始
            psLogStart(Me)

            'プログラムＩＤ、画面名設定
            Master.Master.ppProgramID = M_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

            'ViewStateに「キー情報」を保存
            ViewState(P_KEY) = Session(P_KEY)
            ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'ボタン名変更
            Master.ppRigthButton1.Text = "最新情報"
            Master.ppRigthButton2.Text = "表示条件クリア"

            'ValidateGroupの設定
            Master.ppRigthButton1.ValidationGroup = "Detail"

            '「クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'システムコンボボックス設定
            Me.msSetddlSystem()

            'ヘルスチェック結果コンボボックス設定
            Me.msSetddlHealthChkRslt()

            'ページ初期化
            Me.msPageClear()

            '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
            If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                Me.Master.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

            End If

            '読み込み
            Me.msDataRead()

            'ログ出力終了
            psLogEnd(Me)

        End If

    End Sub

    '---------------------------
    '2014/04/21 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/21 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 最新情報ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Object)

        '--2014/04/23 中川　ここから
        '発生日時チェック
        Me.msCheck_Error()
        '--2014/04/23 中川　ここまで

        '項目チェック
        If (Page.IsValid) = False Then
            Return
        End If

        '読み込み
        Me.msDataRead()

    End Sub

    ''' <summary>
    ''' 表示条件クリアボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Object)
        'ログ出力開始
        psLogStart(Me)

        '検索条件のクリア
        msPageClearSearchPart()

        'ログ出力終了
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' 一覧のボタン項目のクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            Dim strExclusiveDate As String = Nothing
            Dim arTable_Name As New ArrayList
            Dim arKye As New ArrayList

            'ボタンでない場合はなにもしない
            If e.CommandName.Trim() <> "btnUpdate" AndAlso e.CommandName.Trim() <> "btnPrtTrblShorihyo" Then
                Return
            End If

            '開始ログ出力
            psLogStart(Me)

            Select Case e.CommandName.Trim()
                Case "btnUpdate"
                    Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
                    Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行

                    '★排他情報削除
                    If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me _
                                      , Session(P_SESSION_SESSTION_ID) _
                                      , Me.Master.Master.ppExclusiveDateDtl)

                        Me.Master.Master.ppExclusiveDateDtl = String.Empty

                    End If

                    ''★ロック対象テーブル名の登録
                    'arTable_Name.Insert(0, "D180_HC_INVEST_HISTRY")

                    ''★ロックテーブルキー項目の登録(D180_HC_INVEST_HISTRY)
                    'arKye.Insert(0, CType(rowData.FindControl("照会管理番号"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("連番"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("KMKNL区分"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("IDIC区分"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("データ受信日"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("受信連番"), TextBox).Text)

                    ''★排他情報確認処理
                    'If clsExc.pfSel_Exclusive(strExclusiveDate _
                    '                 , Me _
                    '                 , Session(P_SESSION_IP) _
                    '                 , Session(P_SESSION_PLACE) _
                    '                 , Session(P_SESSION_USERID) _
                    '                 , Session(P_SESSION_SESSTION_ID) _
                    '                 , ViewState(P_SESSION_GROUP_NUM) _
                    '                 , M_MY_DISP_ID_UPDP _
                    '                 , arTable_Name _
                    '                 , arKye) = 0 Then

                    Dim strCtrl_No As String = CType(rowData.FindControl("照会管理番号"), TextBox).Text
                    Dim strRenban As String = CType(rowData.FindControl("連番"), TextBox).Text
                    Dim strNLKbn As String = CType(rowData.FindControl("KMKNL区分"), TextBox).Text
                    Dim strIDICKbn As String = CType(rowData.FindControl("IDIC区分"), TextBox).Text
                    Dim strDataJsnDate As String = CType(rowData.FindControl("データ受信日"), TextBox).Text
                    Dim strJsnRenban As String = CType(rowData.FindControl("受信連番"), TextBox).Text
                    Dim strTboxId As String = CType(rowData.FindControl("TBOXID"), TextBox).Text
                    Dim strKekkaCd As String = CType(rowData.FindControl("ヘルスチェック結果"), TextBox).Text
                    Dim strNewCtrl_No As String = CType(rowData.FindControl("新管理番号"), TextBox).Text

                    Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                    Session(P_SESSION_USERID) = ViewState(P_SESSION_USERID)         'ユーザＩＤ
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新      '処理区分
                    Session(P_SESSION_OLDDISP) = M_DISP_ID      '画面ID
                    Session(P_KEY) = {strCtrl_No, strRenban, strNLKbn, strIDICKbn, strDataJsnDate, strJsnRenban, strTboxId, strKekkaCd, strNewCtrl_No}
                    '★排他情報のグループ番号をセッション変数に設定
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    ''★登録年月日時刻
                    'Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    ''★登録年月日時刻(明細)に登録
                    'Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

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
                                    objStack.GetMethod.Name, strREDIRECTPATH_HLTCHK, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    '画面遷移
                    psOpen_Window(Me, strREDIRECTPATH_HLTCHK)
                    'Else
                    'Return
                    'End If

                Case "btnPrtTrblShorihyo"
                    Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
                    Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行

                    '★排他情報削除
                    If Not Me.Master.Master.ppExclusiveDateDtl = String.Empty Then

                        clsExc.pfDel_Exclusive(Me _
                                      , Session(P_SESSION_SESSTION_ID) _
                                      , Me.Master.Master.ppExclusiveDateDtl)

                        Me.Master.Master.ppExclusiveDateDtl = String.Empty

                    End If

                    ''★ロック対象テーブル名の登録
                    'arTable_Name.Insert(0, "D174_HEALTH")

                    ''★ロックテーブルキー項目の登録(D174_HEALTH)
                    'arKye.Insert(0, CType(rowData.FindControl("照会管理番号"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("連番"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("KMKNL区分"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("IDIC区分"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("データ受信日"), TextBox).Text)
                    'arKye.Insert(0, CType(rowData.FindControl("受信連番"), TextBox).Text)


                    ''★排他情報確認処理
                    'If clsExc.pfSel_Exclusive(strExclusiveDate _
                    '                 , Me _
                    '                 , Session(P_SESSION_IP) _
                    '                 , Session(P_SESSION_PLACE) _
                    '                 , Session(P_SESSION_USERID) _
                    '                 , Session(P_SESSION_SESSTION_ID) _
                    '                 , ViewState(P_SESSION_GROUP_NUM) _
                    '                 , M_MY_DISP_ID_UPDP _
                    '                 , arTable_Name _
                    '                 , arKye) = 0 Then
                    '    '--------------------------------
                    '    '2014/04/17 高松　ここから
                    '    '--------------------------------
                    Dim strKey(7) As String

                    If CType(rowData.FindControl("新管理番号"), TextBox).Text Is Nothing _
                        Or CType(rowData.FindControl("新管理番号"), TextBox).Text = String.Empty _
                        Or CType(rowData.FindControl("新管理番号"), TextBox).Text = "1" Then

                        strKey(0) = ""
                        strKey(1) = CType(rowData.FindControl("照会管理番号"), TextBox).Text
                        strKey(2) = CType(rowData.FindControl("連番"), TextBox).Text
                        strKey(3) = CType(rowData.FindControl("KMKNL区分"), TextBox).Text
                        strKey(4) = CType(rowData.FindControl("IDIC区分"), TextBox).Text
                        strKey(5) = CType(rowData.FindControl("データ受信日"), TextBox).Text
                        strKey(6) = CType(rowData.FindControl("受信連番"), TextBox).Text
                        strKey(7) = CType(rowData.FindControl("TBOXID"), TextBox).Text
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録      '処理区分
                    Else
                        strKey(0) = CType(rowData.FindControl("新管理番号"), TextBox).Text
                        strKey(1) = CType(rowData.FindControl("照会管理番号"), TextBox).Text
                        strKey(2) = CType(rowData.FindControl("連番"), TextBox).Text
                        strKey(3) = CType(rowData.FindControl("KMKNL区分"), TextBox).Text
                        strKey(4) = CType(rowData.FindControl("IDIC区分"), TextBox).Text
                        strKey(5) = CType(rowData.FindControl("データ受信日"), TextBox).Text
                        strKey(6) = CType(rowData.FindControl("受信連番"), TextBox).Text
                        strKey(7) = CType(rowData.FindControl("TBOXID"), TextBox).Text
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新      '処理区分
                    End If
                    '--------------------------------
                    '2014/04/17 高松　ここまで
                    '--------------------------------
                    'ホールコード
                    Dim strHVal As String = CType(rowData.FindControl("ホールコード"), TextBox).Text

                    'TBOXID
                    Dim strTboxId As String = CType(rowData.FindControl("TBOXID"), TextBox).Text

                    Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
                    Session(P_SESSION_USERID) = ViewState(P_SESSION_USERID)         'ユーザＩＤ
                    '--------------------------------
                    '2014/04/17 高松　ここから
                    '--------------------------------
                    'Session(P_SESSION_TERMS) =  ClsComVer.E_遷移条件.更新      '処理区分
                    Session(P_SESSION_OLDDISP) = M_DISP_ID      '画面ID
                    'Session(P_KEY) = {strHVal, strTboxId}
                    Session(P_KEY) = strKey
                    '--------------------------------
                    '2014/04/17 高松　ここまで
                    '--------------------------------
                    '★排他情報のグループ番号をセッション変数に設定
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    ''★登録年月日時刻
                    'Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                    ''★登録年月日時刻(明細)に登録
                    'Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

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
                                    objStack.GetMethod.Name, strREDIRECTPATH_TRBL, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    '画面遷移
                    psOpen_Window(Me, strREDIRECTPATH_TRBL)
                    'Else
                    'Return
                    'End If

                Case Else

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
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub
#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' イベントハンドラ設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetHandler()
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
    End Sub

    ''' <summary>
    ''' システム_ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCon) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'ストアドプロシージャ設定
            objCmd = New SqlCommand("HEALSTP001_S1")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'ＳＱＬ実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得異常
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception
            End If

            ddlSystem.Items.Clear()
            ddlSystem.DataSource = objDs.Tables(0)
            ddlSystem.DataTextField = "T03_TBOXCLASS_CD"
            ddlSystem.DataValueField = "T03_SYSTEM_CD"
            ddlSystem.DataBind()
            ddlSystem.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "システムの一覧")
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
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' ヘルスチェック結果_ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlHealthChkRslt()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCon) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'ストアドプロシージャ設定(ヘルスチェック結果マスタ全レコードの貼り付け)
            objCmd = New SqlCommand("HEALSTP001_S2")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'ＳＱＬ実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得異常
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception
            End If

            ddlHealthChkRslt.Items.Clear()
            ddlHealthChkRslt.DataSource = objDs.Tables(0)
            ddlHealthChkRslt.DataTextField = "M93_CNSTCLS_NM"
            ddlHealthChkRslt.DataValueField = "M93_CNST_CLS"
            ddlHealthChkRslt.DataBind()
            ddlHealthChkRslt.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ヘルスチェック結果一覧")
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
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' ページクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear()
        '★排他情報用コントロールの初期化
        Me.Master.Master.ppExclusiveDate = String.Empty
        Me.Master.Master.ppExclusiveDateDtl = String.Empty

        '検索エリアクリア
        Me.msPageClearSearchPart()

        '一覧クリア
        Me.msPageClearListPart()

    End Sub

    ''' <summary>
    ''' 検索エリアクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClearSearchPart()
        txtHasseiDateFr.ppText = ""     '発生日時(日)From
        txtHasseiTimeFr.ppHourText = "" '発生日時(時)From
        txtHasseiTimeFr.ppMinText = ""  '発生日時(分)From
        txtHasseiDateTo.ppText = ""     '発生日時(日)To
        txtHasseiTimeTo.ppHourText = "" '発生日時(時)To
        txtHasseiTimeTo.ppMinText = ""  '発生日時(分)To
        txtTBoxIdFrTo.ppFromText = ""   'TBOXID From
        txtTBoxIdFrTo.ppToText = ""     'TBOXID To
        txtHasseiDateFr.ppDateBox.Focus()
        ddlHealthChkRslt.SelectedIndex = 0
        ddlSystem.SelectedIndex = 0
    End Sub

    ''' <summary>
    ''' 一覧クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClearListPart()
        grvList.DataSource = New DataTable() '一覧
        Master.ppCount = "0"                            '件数
        grvList.DataBind()
    End Sub

    ''' <summary>
    ''' 空白の値をDBNullに
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If
        Return strVal
    End Function

    ''' <summary>
    ''' 検索データ読み込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDataRead()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ログ出力開始
            psLogStart(Me)

            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCon) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'ストアド設定
            objCmd = New SqlCommand("HEALSTP001_S3")
            objCmd.Connection = objCon
            objCmd.CommandTimeout = 120
            objCmd.CommandType = CommandType.StoredProcedure

            With objCmd.Parameters
                .Add(pfSet_Param("prmD174_HPN_DFr", SqlDbType.NVarChar, txtHasseiDateFr.ppText.Replace("/", "")))
                .Add(pfSet_Param("prmD174_HPN_DTo", SqlDbType.NVarChar, txtHasseiDateTo.ppText.Replace("/", "")))
                .Add(pfSet_Param("prmD174_HPN_TFr", SqlDbType.NVarChar, txtHasseiTimeFr.ppHourText & txtHasseiTimeFr.ppMinText))
                .Add(pfSet_Param("prmD174_HPN_TTo", SqlDbType.NVarChar, txtHasseiTimeTo.ppHourText & txtHasseiTimeTo.ppMinText))
                .Add(pfSet_Param("prmD174_TBOXIDFr", SqlDbType.NVarChar, txtTBoxIdFrTo.ppFromText))
                .Add(pfSet_Param("prmD174_TBOXIDTo", SqlDbType.NVarChar, txtTBoxIdFrTo.ppToText))
                .Add(pfSet_Param("prmT03_TBOXCLASS_CD", SqlDbType.NVarChar, ddlSystem.SelectedValue))
                .Add(pfSet_Param("prmD174_DETECT_CD", SqlDbType.NVarChar, ddlHealthChkRslt.SelectedValue))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            '一覧にセット
            Master.ppCount = objDs.Tables(0).Rows.Count

            '閾値制御
            Dim objTbl As DataTable = mfSetShikiichi(objDs.Tables(0))

            grvList.DataSource = objTbl
            grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ヘルスチェック一覧")
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
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            'ログ出力終了
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 閾値制御
    ''' </summary>
    ''' <param name="objTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetShikiichi(ByVal objTbl As DataTable) As DataTable
        Dim objWkTbl As DataTable = objTbl.Clone()

        'テーブルに行はあるか？
        If Not objTbl Is Nothing AndAlso objTbl.Rows.Count > 0 Then

            'テーブルの行が最大件数を超えているか
            If objTbl.Rows.Count > objTbl.Rows(0)("最大件数") Then

                '行を最大件数に絞込み
                For i As Integer = 0 To objTbl.Rows(0)("最大件数") - 1
                    objWkTbl.ImportRow(objTbl.Rows(i))
                Next

                '閾値超過をメッセで知らせる
                psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, objTbl.Rows.Count, objTbl.Rows(0)("最大件数"))

            Else
                objWkTbl = objTbl
            End If
        Else
            objWkTbl = objTbl
        End If

        objWkTbl.Columns.Remove("最大件数")

        Return objWkTbl

    End Function

    '--2014/04/22 中川　ここから
    ''' <summary>
    ''' 発生日時チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheck_Error()

        Dim strErr As String = Nothing              'エラーメッセージ
        Dim strHasseiDtFr As String = Nothing
        Dim strHasseiDtTo As String = Nothing

        '発生日時
        'From 日時入力チェック
        If txtHasseiTimeFr.ppHourText <> "" And txtHasseiDateFr.ppText = "" Then
            'エラー設定
            txtHasseiDateFr.psSet_ErrorNo("4012", "発生日時")

        End If

        'To 日時入力チェック
        If txtHasseiTimeTo.ppHourText <> "" And txtHasseiDateTo.ppText = "" Then
            'エラー設定
            txtHasseiDateTo.psSet_ErrorNo("4012", "発生日時")

        End If

        'FromTo 範囲チェック
        If txtHasseiDateFr.ppText <> "" And txtHasseiDateTo.ppText <> "" Then

            If txtHasseiTimeFr.ppHourText <> "" Then

                strHasseiDtFr = txtHasseiDateFr.ppText & txtHasseiTimeFr.ppHourText & txtHasseiTimeFr.ppMinText

            Else
                strHasseiDtFr = txtHasseiDateFr.ppText & "0000"

            End If

            If txtHasseiTimeTo.ppHourText <> "" Then

                strHasseiDtTo = txtHasseiDateTo.ppText & txtHasseiTimeTo.ppHourText & txtHasseiTimeTo.ppMinText

            Else
                strHasseiDtTo = txtHasseiDateTo.ppText & "2359"

            End If

            If strHasseiDtFr > strHasseiDtTo Then
                'エラー設定
                txtHasseiDateFr.psSet_ErrorNo("1005", "発生日時")

            End If

        End If

    End Sub
    '--2014/04/22 中川　ここまで

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each rowData As GridViewRow In grvList.Rows
            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("発生日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("受信日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("TBOXID"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("システム"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("VER"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ヘルスチェック結果"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("調査日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("調査者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("調査結果"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            Else
                CType(rowData.FindControl("発生日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("受信日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("TBOXID"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("システム"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("VER"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ヘルスチェック結果"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("調査日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("調査者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("調査結果"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
            End If
        Next

    End Sub
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
