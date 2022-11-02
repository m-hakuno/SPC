'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　即時集信一覧
'*　ＰＧＭＩＤ：　MSTLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.07　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax

#End Region

Public Class MSTLSTP001

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
    Private Const M_DISP_ID = P_FUN_MST & P_SCR_LST & P_PAGE & "001"


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
        pfSet_GridView(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton3.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton4.Click, AddressOf btn_Click
        AddHandler Master.ppRigthButton5.Click, AddressOf btn_Click

        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf ms_GetHall
        Me.txtTboxId.ppTextBox.AutoPostBack = True

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
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                If Session(P_SESSION_BCLIST) Is Nothing Then

                    '不正アクセス
                    Throw New Exception

                End If

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面初期化処理
                msClearScreen()

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

            End Try

        End If

    End Sub

#End Region

#Region "ユーザー権限"
    '---------------------------
    '2014/04/15 武 ここから
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
    '2014/04/15 武 ここまで
    '---------------------------
#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btn_Click(sender As System.Object, e As System.EventArgs)

        Dim result As Boolean = False     '処理結果
        Dim strView(0) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Select Case sender.text

            Case "検索条件クリア"

                '入力値の初期化
                Me.txtTboxId.ppText = Nothing
                Me.lblHoleNm_2.Text = Nothing
                Me.lblOperationStatus_2.Text = Nothing
                Me.lblTboxType_2.Text = Nothing
                Me.lblHaishinResult_In.Text = String.Empty
                'ボタンの切り替え
                ms_ChangeBtn("クリア")

                '表示グリッドビューの初期化
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()

                '件数のクリア
                Master.ppCount = "0"

            Case "検索"

                'ＴＢＯＸ情報が存在するかチェック
                If Me.txtTboxId.ppText <> String.Empty And _
                    Me.lblHoleNm_2.Text = Nothing Then

                    Me.txtTboxId.psSet_ErrorNo("2002", Me.txtTboxId.ppName)

                End If

                '検索処理開始
                If (Page.IsValid) Then

                    '入力TBOXIDの整合性チェック
                    Try
                        'TBOX情報を取得
                        If ms_GetHall() = False Then
                            Master.ppRigthButton1.Visible = False
                            Master.ppRigthButton2.Visible = False
                            '処理終了
                            Exit Sub
                        End If

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

                    Try

                        '照会要求親データのチェックを行う
                        If ms_GetYokyuDB() Then

                            '集信管理DBを表示する
                            If ms_GetSyushinMngDB() Then    '集信完了
                                'ボタンの切り替え
                                ms_ChangeBtn("集信完了")
                            Else                            '集信中
                                'ボタンの切り替え
                                ms_ChangeBtn("集信中")
                            End If

                        Else
                            'ボタン項目を切り替える
                            ms_ChangeBtn("集信中")
                        End If


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
                        '処理を終了する
                        Exit Sub

                    End Try

                End If

            Case "集信"

                'ＴＢＯＸ情報が存在するかチェック
                If Me.txtTboxId.ppText <> String.Empty And _
                    Me.lblHoleNm_2.Text = Nothing Then

                    Me.txtTboxId.psSet_ErrorNo("2002", Me.txtTboxId.ppName)

                End If

                If (Page.IsValid) Then

                    Try

                        '照会要求親データに集信データを追加
                        ms_InsYokyuDB("902")

                        '照会要求親データのチェックを行う
                        If ms_GetYokyuDB() Then

                            '集信管理DBを表示する
                            If ms_GetSyushinMngDB() Then    '集信完了
                                'ボタンの切り替え
                                ms_ChangeBtn("集信完了")
                            Else                            '集信中
                                'ボタンの切り替え
                                ms_ChangeBtn("集信中")
                            End If

                        Else
                            'ボタン項目を切り替える
                            ms_ChangeBtn("集信中")
                        End If

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
                        '処理を終了する
                        Exit Sub

                    End Try

                End If

            Case "即時集信"

                'ＴＢＯＸ情報が存在するかチェック
                If Me.txtTboxId.ppText <> String.Empty And _
                    Me.lblHoleNm_2.Text = Nothing Then

                    Me.txtTboxId.psSet_ErrorNo("2002", Me.txtTboxId.ppName)

                End If

                If (Page.IsValid) Then

                    Try

                        '照会要求親データに即時集信データを追加
                        ms_InsYokyuDB("901")

                        '照会要求親データのチェックを行う
                        If ms_GetYokyuDB() Then

                            '集信管理DBを表示する
                            If ms_GetSyushinMngDB() Then    '集信完了
                                'ボタンの切り替え
                                ms_ChangeBtn("集信完了")
                            Else                            '集信中
                                'ボタンの切り替え
                                ms_ChangeBtn("集信中")
                            End If

                        Else
                            'ボタン項目を切り替える
                            ms_ChangeBtn("集信中")
                        End If

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
                        '処理を終了する
                        Exit Sub

                    End Try

                End If

            Case "更新"

                Try

                    '照会要求親データのチェックを行う
                    If ms_GetYokyuDB() Then

                        '集信管理DBを表示する
                        If ms_GetSyushinMngDB() Then    '集信完了
                            'ボタンの切り替え
                            ms_ChangeBtn("集信完了")
                            'メッセージ表示
                            psMesBox(Me, "20010", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
                        Else                            '集信中
                            'ボタンの切り替え
                            ms_ChangeBtn("集信中")
                        End If

                    Else
                        'ボタン項目を切り替える
                        ms_ChangeBtn("集信中")
                    End If

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
                    '処理を終了する
                    Exit Sub

                End Try


        End Select

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msClearScreen()

        Dim strSession_in() As String = Session(P_KEY)
        Dim strSesTerms_out As String = Session(P_SESSION_TERMS)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '権限で設定を変更する
            'If User = "" Then

            'End If
            '場所で設定を変更する
            'If Prace = "" Then

            'End If

            'If Not strSesTerms_out Is Nothing Then

            '    Select Case strSesTerms_out
            '        Case  ClsComVer.E_遷移条件.参照

            '            '入力値の初期化
            '            txtTboxId.ppText = Nothing

            '        Case  ClsComVer.E_遷移条件.更新

            If strSession_in Is Nothing Then

                '入力値の初期化
                txtTboxId.ppText = Nothing

            Else

                '入力値の初期化
                txtTboxId.ppText = strSession_in(0)

                'ホール情報の取得
                ms_GetHall()

            End If
            'End Select

            'End If

            '名称の表示
            Master.ppRigthButton1.Text = "即時集信"
            Master.ppRigthButton2.Text = "集信"
            Master.ppRigthButton3.Text = P_BTN_NM_UPD
            Master.ppRigthButton4.Text = "検索"
            Master.ppRigthButton5.Text = "検索条件クリア"

            'ボタンの表示設定
            Master.ppRigthButton1.Visible = False
            Master.ppRigthButton2.Visible = False
            Master.ppRigthButton3.Visible = False
            Master.ppRigthButton4.Visible = True
            Master.ppRigthButton5.Visible = True

            '表示グリッドビューの初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            '件数初期化
            Master.ppCount = "0"

            '検証エラー設定
            Master.ppRigthButton1.CausesValidation = True
            Master.ppRigthButton2.CausesValidation = True
            Master.ppRigthButton3.CausesValidation = False
            Master.ppRigthButton4.CausesValidation = True
            Master.ppRigthButton5.CausesValidation = False

            'ボタン押下時のメッセージ
            Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("20003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel)
            Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("20005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel)

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
            'システムエラー
            Throw ex

        Finally
        End Try

    End Sub

    ''' <summary>
    ''' ボタン制御
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ms_ChangeBtn(ByVal strFlag As String)

        '権限で設定を変更する
        'If User = "" Then

        'End If

        '場所で設定を変更する
        'If Prace = "" Then

        'End If

        Select Case strFlag
            Case "クリア"

                'ボタンの表示設定
                Master.ppRigthButton1.Visible = False
                Master.ppRigthButton2.Visible = False
                Master.ppRigthButton3.Visible = False

            Case "集信中"

                Master.ppRigthButton1.Visible = False
                Master.ppRigthButton2.Visible = False
                Master.ppRigthButton3.Visible = True

            Case "集信完了"

                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton3.Visible = False

        End Select

    End Sub

    ''' <summary>
    ''' 照会要求親データの結果確認
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ms_GetYokyuDB() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strOKNG As String = Nothing        '検索結果
        Dim strView() As String = Me.ViewState("TBOXID")
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(objCn) Then

            Try

                objCmd = New SqlCommand(M_DISP_ID + "_S2", objCn)
                With objCmd.Parameters

                    '--パラメータ設定
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, strView(0).ToString))            'TBOXID
                    .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP).ToString)) '端末情報
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用

                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '結果情報を取得
                strOKNG = objCmd.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"         '集信中
                        'メッセージ表示
                        psMesBox(Me, "20004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = objDs.Tables(1)
                        '変更を反映
                        Me.grvList.DataBind()

                        '件数を設定
                        Master.ppCount = objDs.Tables(1).Rows.Count.ToString

                        '配信結果を設定
                        Me.lblHaishinResult_In.Text = String.Empty

                        Return False
                    Case "2"         '即時集信中
                        'メッセージ表示
                        psMesBox(Me, "20004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "即時")

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = objDs.Tables(2)
                        '変更を反映
                        Me.grvList.DataBind()

                        '件数を設定
                        Master.ppCount = objDs.Tables(2).Rows.Count.ToString

                        '配信結果を設定
                        Me.lblHaishinResult_In.Text = String.Empty

                        Return False
                    Case Else        '集信完了
                        Return True
                End Select

            Catch ex As SqlException

                'SQLエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
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
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
            Return False

        End If

    End Function

    ''' <summary>
    ''' 集信管理DBを検索
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function ms_GetSyushinMngDB() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strOKNG As String = Nothing        '検索結果
        Dim strView() As String = Me.ViewState("TBOXID")
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(objCn) Then

            Try

                objCmd = New SqlCommand(M_DISP_ID + "_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, strView(0)))                     'TBOXID
                    .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))          '端末情報
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '結果情報を取得
                strOKNG = objCmd.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"        '集信完了

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = objDs.Tables(0)

                        '変更を反映
                        Me.grvList.DataBind()

                        '件数を設定
                        Master.ppCount = objDs.Tables(0).Rows.Count.ToString

                        '配信結果を設定
                        If Not objDs.Tables(1).Rows.Count = 0 Then
                            Me.lblHaishinResult_In.Text = objDs.Tables(1).Rows(0)("内容")
                        Else
                            Me.lblHaishinResult_In.Text = ""
                        End If

                        Return True
                    Case Else       'データ無し

                        '表示グリッドビューの初期化
                        Me.grvList.DataSource = New Object() {}
                        Me.grvList.DataBind()

                        '件数を設定
                        Master.ppCount = "0"

                        '配信結果を設定
                        Me.lblHaishinResult_In.Text = String.Empty

                        'メッセージ表示
                        '--------------------------------
                        '2014/05/12 後藤　ここから
                        '--------------------------------
                        'psMesBox(Me, "00007", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        '--------------------------------
                        '2014/05/12 後藤　ここまで
                        '--------------------------------
                        Return True
                End Select

            Catch ex As SqlException

                'SQLエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信管理DB")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "集信管理DB")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
            Return False
        End If

    End Function

    ''' <summary>
    ''' 照会要求親データの作成
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ms_InsYokyuDB(ByVal strSyoriCD As String)

        Dim objCn As SqlConnection = Nothing               'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing                 'SqlCommandクラス
        Dim objDs As DataSet = Nothing                     'データセット
        Dim strOKNG As String = Nothing                    '検索結果
        Dim strView() As String = Me.ViewState("TBOXID")   'ビューステートから取得
        Dim strReqmng_no As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try

                '管理番号の作成
                strReqmng_no = Date.Now.ToString("yyyyMMdd")

                '管理番号採番
                'パラメータ設定
                objCmd = New SqlCommand("ZCMPSEL022", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 1))                                    '管理番号
                    .Add(pfSet_Param("YMD", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))         '年月日
                    .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))         '結果取得用
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '戻り値確認
                If objCmd.Parameters("SalesYTD").Value Is Nothing Then

                    '処理終了
                    Throw New Exception

                End If

                '管理番号を作成する
                strReqmng_no = strReqmng_no _
                             + String.Format("{0:D4}", objCmd.Parameters("SalesYTD").Value)

                objCmd = New SqlCommand(M_DISP_ID & "_U1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    '照会要求親データ
                    .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))        '端末情報
                    .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))    'ユーザID
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, strView(0)))                   'TBOXID
                    .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, strSyoriCD))                   '処理コード
                    .Add(pfSet_Param("prm_reqmng_no", SqlDbType.NVarChar, strReqmng_no))               '依頼番号
                End With

                'データ追加／更新
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn
                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure
                    objCmd.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                End Using

                '更新が正常終了
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "照会要求データ")

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
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
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "照会要求データ")
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
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    Protected Function ms_GetHall() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strOKNG As String = Nothing         '検索結果
        Dim strView(1 - 1) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '入力値チェック
        If Me.txtTboxId.ppText = String.Empty Then

            Me.lblHoleNm_2.Text = String.Empty
            Me.lblOperationStatus_2.Text = String.Empty
            Me.lblTboxType_2.Text = String.Empty
            Me.ViewState("TBOXID") = Nothing
            Me.lblHaishinResult_In.Text = String.Empty
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

        Else

            strView(0) = Me.txtTboxId.ppText
            Me.ViewState("TBOXID") = strView

        End If

        '接続
        If clsDataConnect.pfOpen_Database(objCn) Then

            Try

                objCmd = New SqlCommand(M_DISP_ID + "_S1", objCn)
                With objCmd.Parameters

                    '--パラメータ設定
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, Me.txtTboxId.ppText))            'TBOXID
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))   '結果取得用

                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '結果情報を取得
                strOKNG = objCmd.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "1"        'データ有り

                        Me.lblHoleNm_2.Text = objDs.Tables(0).Rows(0).Item("ホール名").ToString
                        Me.lblOperationStatus_2.Text = objDs.Tables(0).Rows(0).Item("運用状態").ToString
                        Me.lblTboxType_2.Text = objDs.Tables(0).Rows(0).Item("バージョン").ToString
                        Me.lblHaishinResult_In.Text = String.Empty
                        Return True

                    Case "2"        'データ有り（IC以外）

                        Me.lblHoleNm_2.Text = objDs.Tables(0).Rows(0).Item("ホール名").ToString
                        Me.lblOperationStatus_2.Text = objDs.Tables(0).Rows(0).Item("運用状態").ToString
                        Me.lblTboxType_2.Text = objDs.Tables(0).Rows(0).Item("バージョン").ToString
                        Me.lblHaishinResult_In.Text = String.Empty

                        'TBOXID対象外
                        psMesBox(Me, "30013", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, objDs.Tables(0).Rows(0).Item("TBOX種別").ToString)
                        Return False

                    Case Else       'データ無し

                        Me.lblHoleNm_2.Text = String.Empty
                        Me.lblOperationStatus_2.Text = String.Empty
                        Me.lblTboxType_2.Text = String.Empty
                        Me.ViewState("TBOXID") = Nothing
                        Me.lblHaishinResult_In.Text = String.Empty
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()
                        Return False

                End Select

                'ボタン項目を切り替える
                ms_ChangeBtn("クリア")

            Catch ex As SqlException

                'SQLエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Return False

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False

        End If

    End Function
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
