'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ICカード履歴調査一覧(詳細)
'*　ＰＧＭＩＤ：　ICHLSTP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.20　：　ＮＫＣ
'*  更　新　　：　2014.07.03　：　間瀬      NL区分でNを指定しているものに対してJも参照するように変更
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


Public Class ICHLSTP002

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
    Private Const M_DISP_ID = P_FUN_ICH & P_SCR_LST & P_PAGE & "002"

    Private Const M_HYOUJI = "表示条件"
    Private Const M_UNYO = "運用機番"
    Private Const M_JB = "ＪＢ番号"
    Private Const M_CID = "ＣＩＤ番号"
    Private Const M_TIME = "時間帯"
    Private Const M_SPACE = " "
    Private Const M_AND = "AND"
    Private Const M_EQUAL = "="
    Private Const M_PLUS = "+"
    Private Const M_SYONARI_EQ = "<="
    Private Const M_DAINARI_EQ = ">="
    Private Const M_START = "【"
    Private Const M_END = "】"
    Private Const M_SINGLE = "'"

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
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If Not IsPostBack Then  '初回表示

            Dim enableScript As String = Nothing                '用途内容制御用

            Try

                '開始ログ出力
                psLogStart(Me)

                '活性非活性のJavascriptの設定(運用機番,ＪＢ番号)
                enableScript = "txtChange('" + "1" + "','" + txtUnyo.ppTextBoxFrom.ClientID + "','" + txtUnyo.ppTextBoxTo.ClientID + "','" + _
                                              txtJBNum.ppTextBoxFrom.ClientID + "','" + txtJBNum.ppTextBoxTo.ClientID + "')"
                txtUnyo.ppTextBoxFrom.Attributes.Add("onChange", enableScript)
                txtUnyo.ppTextBoxTo.Attributes.Add("onChange", enableScript)

                enableScript = "txtChange('" + "2" + "','" + txtUnyo.ppTextBoxFrom.ClientID + "','" + txtUnyo.ppTextBoxTo.ClientID + "','" + _
                                              txtJBNum.ppTextBoxFrom.ClientID + "','" + txtJBNum.ppTextBoxTo.ClientID + "')"
                txtJBNum.ppTextBoxFrom.Attributes.Add("onChange", enableScript)
                txtJBNum.ppTextBoxTo.Attributes.Add("onChange", enableScript)

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面初期化処理
                msClearScreen()

                'セッション変数の取得/明細情報検索
                If Not ms_GetSession() Then

                    'システムエラー
                    psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    '処理終了(画面終了)
                    psClose_Window(Me)
                    Exit Sub

                End If

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
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

        End If

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

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            Select Case sender.text

                Case "検索条件クリア"

                    '入力項目の初期化
                    Me.txtCID.ppText = Nothing
                    Me.txtUnyo.ppFromText = Nothing
                    Me.txtUnyo.ppToText = Nothing
                    Me.txtJBNum.ppFromText = Nothing
                    Me.txtJBNum.ppToText = Nothing
                    Me.txtJikantai.ppHourTextFrom = Nothing
                    Me.txtJikantai.ppMinTextFrom = Nothing
                    Me.txtJikantai.ppHourTextTo = Nothing
                    Me.txtJikantai.ppMinTextTo = Nothing

                Case "検索"

                    Try

                        If (Page.IsValid) Then

                            '表示条件文字列の設定
                            Dim sb As New System.Text.StringBuilder

                            sb.Append(M_HYOUJI)
                            sb.Append(M_START)

                            For i As Integer = 0 To 4 - 1

                                Select Case i
                                    Case 0

                                        If Not Me.txtCID.ppText Is String.Empty Then

                                            sb.Append(M_CID)
                                            sb.Append(M_EQUAL)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtCID.ppText)
                                            sb.Append(M_SINGLE)

                                        End If

                                    Case 1

                                        If Not Me.txtUnyo.ppFromText Is String.Empty _
                                            And Me.txtUnyo.ppFromText Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_UNYO)
                                            sb.Append(M_EQUAL)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtUnyo.ppFromText)
                                            sb.Append(M_SINGLE)

                                        ElseIf Me.txtUnyo.ppFromText Is String.Empty _
                                            And Not Me.txtUnyo.ppFromText Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_UNYO)
                                            sb.Append(M_SYONARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtUnyo.ppFromText)
                                            sb.Append(M_SINGLE)


                                        ElseIf Not Me.txtUnyo.ppFromText Is String.Empty _
                                            And Not Me.txtUnyo.ppFromText Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_UNYO)
                                            sb.Append(M_DAINARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtUnyo.ppFromText)
                                            sb.Append(M_SINGLE)
                                            sb.Append(M_SPACE)
                                            sb.Append(M_AND)
                                            sb.Append(M_SYONARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtUnyo.ppToText)
                                            sb.Append(M_SINGLE)

                                        End If

                                    Case 2

                                        If Not Me.txtJBNum.ppFromText Is String.Empty _
                                            And Me.txtJBNum.ppFromText Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_JB)
                                            sb.Append(M_EQUAL)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJBNum.ppFromText)
                                            sb.Append(M_SINGLE)

                                        ElseIf Me.txtJBNum.ppFromText Is String.Empty _
                                            And Not Me.txtJBNum.ppFromText Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_JB)
                                            sb.Append(M_SYONARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJBNum.ppFromText)
                                            sb.Append(M_SINGLE)


                                        ElseIf Not Me.txtJBNum.ppFromText Is String.Empty _
                                            And Not Me.txtJBNum.ppFromText Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_JB)
                                            sb.Append(M_DAINARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJBNum.ppFromText)
                                            sb.Append(M_SINGLE)
                                            sb.Append(M_SPACE)
                                            sb.Append(M_AND)
                                            sb.Append(M_SYONARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJBNum.ppToText)
                                            sb.Append(M_SINGLE)

                                        End If

                                    Case 3

                                        If Not Me.txtJikantai.ppHourTextFrom Is String.Empty _
                                            And Me.txtJikantai.ppHourTextTo Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_TIME)
                                            sb.Append(M_EQUAL)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJikantai.ppHourTextFrom)
                                            sb.Append(":")
                                            sb.Append(Me.txtJikantai.ppMinTextFrom)
                                            sb.Append(M_SINGLE)

                                        ElseIf Me.txtJikantai.ppHourTextFrom Is String.Empty _
                                            And Not Me.txtJikantai.ppHourTextTo Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_TIME)
                                            sb.Append(M_EQUAL)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJikantai.ppHourTextTo)
                                            sb.Append(":")
                                            sb.Append(Me.txtJikantai.ppMinTextTo)
                                            sb.Append(M_SINGLE)


                                        ElseIf Not Me.txtJikantai.ppHourTextFrom Is String.Empty _
                                            And Not Me.txtJikantai.ppHourTextTo Is String.Empty Then

                                            sb.Append(M_SPACE)
                                            sb.Append(M_TIME)
                                            sb.Append(M_DAINARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJikantai.ppHourTextFrom)
                                            sb.Append(":")
                                            sb.Append(Me.txtJikantai.ppMinTextFrom)
                                            sb.Append(M_SINGLE)
                                            sb.Append(M_SPACE)
                                            sb.Append(M_AND)
                                            sb.Append(M_SYONARI_EQ)
                                            sb.Append(M_SINGLE)
                                            sb.Append(Me.txtJikantai.ppHourTextTo)
                                            sb.Append(":")
                                            sb.Append(Me.txtJikantai.ppMinTextTo)
                                            sb.Append(M_SINGLE)

                                        End If

                                End Select

                            Next

                            sb.Append(M_END)

                            Me.ViewState("検索条件") = sb.ToString

                            'ICカード履歴調査一覧の取得
                            ms_GetICRireki()

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
                        'グリッドビューの初期化
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()

                        '処理終了
                        Exit Sub

                    End Try

                Case "印刷"

                    Dim dsPDF As DataSet = New DataSet("dtPDF")
                    Dim dtPDF As DataTable = New DataTable                     'PDF用データテーブル
                    Dim dirPath As String = ms_GetDirpath()
                    Dim strView_st() As String = Me.ViewState(P_KEY)           'キー項目の設定 
                    Dim rpt As New ICHREP001

                    If Not dirPath Is Nothing Then

                        Dim fileName As String = Nothing
                        Dim CrateDate As String = Date.Now.ToString("yyyyMMddHHmmss")

                        'ファイル名作成
                        fileName = "ICカード履歴調査一覧_" _
                                 + strView_st(11) _
                                 + "_" _
                                 + CrateDate _
                                 + ".pdf"

                        '■■■■　ダウンロード対象外の為、不要　■■■■■■■■■■■■■■■■■■■■2014/04/01
                        ''ダウンロードファイルテーブルの更新
                        'Try

                        '    ms_InsDownload(strView_st(11) _
                        '                 , "59" _
                        '                 , fileName _
                        '                 , dirPath _
                        '                 , CrateDate _
                        '                 , "ICカード履歴調査一覧_")

                        'Catch ex As Exception

                        ''--------------------------------
                        ''2014/04/14 星野　ここから
                        ''--------------------------------
                        ''ログ出力
                        'psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                        '                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        ''--------------------------------
                        ''2014/04/14 星野　ここまで
                        ''--------------------------------
                        '    '処理終了
                        '    Exit Sub

                        'End Try
                        '■■■■　ダウンロード対象外の為、不要　■■■■■■■■■■■■■■■■■■■■2014/04/01

                        '帳票の出力
                        ms_GetPdfDatatable(dtPDF)

                        'Active Reports(帳票 ホール管理データ)の起動
                        Try

                            'rpt = New MNTREP003
                            psPrintPDF(Me, rpt, dtPDF, fileName)

                        Catch ex As Exception

                            '帳票の出力に失敗
                            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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

                    Else

                        '処理終了
                        Exit Sub

                    End If

            End Select

            'ＪＢ番号と運用機番の非活性を判定し設定する
            msEnable_JbUnyo()

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

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' グリッドボタン操作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim result As Boolean = False
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Select Case e.CommandName

            Case "btnSyoukai"     '使用中カードＤＢ

                '開始ログ出力
                psLogStart(Me)

                Try

                    '使用中カードDBの照会状態チェック
                    ms_GetUseCD(Me.lblTboxID_Input.Text _
                              , CType(rowData.FindControl("ＣＩＤ"), TextBox).Text _
                              , result)

                    If result Then

                        '使用中カードDBの照会開始
                        ms_InsUseCD(Me.lblTboxID_Input.Text _
                                    , CType(rowData.FindControl("ＣＩＤ"), TextBox).Text)
                    Else

                        '処理を終了する
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

                Finally

                    '終了ログ出力
                    psLogEnd(Me)

                End Try

        End Select

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面項目の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        'コントロールの初期化
        Me.txtCID.ppText = Nothing
        Me.txtUnyo.ppFromText = Nothing
        Me.txtUnyo.ppToText = Nothing
        Me.txtJBNum.ppFromText = Nothing
        Me.txtJBNum.ppToText = Nothing
        Me.txtJikantai.ppHourTextFrom = Nothing
        Me.txtJikantai.ppMinTextFrom = Nothing
        Me.txtJikantai.ppHourTextTo = Nothing
        Me.txtJikantai.ppMinTextTo = Nothing

        'グリッドビューの初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        '初期表示の件数
        Master.ppCount = "0"

        'ボタン項目の設定
        Master.ppRigthButton1.CausesValidation = True
        Master.ppRigthButton2.CausesValidation = False
        Master.ppRigthButton3.CausesValidation = False

        Master.ppRigthButton3.Enabled = False
        Master.ppRigthButton3.Visible = True
        Master.ppRigthButton3.Text = "印刷"
        Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ICカード履歴調査一覧")

        'フォーカスの設定
        Me.txtCID.ppTextBox.Focus()

    End Sub

    ''' <summary>
    ''' セッション変数の設定
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetSession() As Boolean

        Dim strSession() As String = Me.Session(P_KEY)
        Dim strViewState(13 - 1) As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '開始ログ出力
            psLogStart(Me)

            If strSession Is Nothing Then

                Throw New Exception

            End If

            'セッション情報の有無の確認
            For i As Integer = 0 To strSession.Count - 1

                Select Case i
                    Case 2   'TBOXID

                        Me.lblTboxID_Input.Text = strSession(2)

                    Case 4   'ホール名

                        Me.lblHallName_Input.Text = strSession(4)

                    Case 6   'VER

                        Me.lblTboxType_Input.Text = strSession(6)

                    Case 10  'データ日付

                        Me.lblDataDT_Input.Text = strSession(10)

                End Select

                strViewState(i) = strSession(i)

            Next

            'ビューステートに格納
            Me.ViewState.Add(P_KEY, strViewState)

            Return True

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
            Return False

        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Function

    ''' <summary>
    ''' ＪＢ番号・運用機番の活性/非活性判断
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEnable_JbUnyo()

        'ＪＢ番号が活性の場合、運用機番は非活性
        If txtJBNum.ppFromText <> String.Empty _
            Or txtJBNum.ppToText <> String.Empty Then

            txtJBNum.ppEnabled = True
            txtUnyo.ppEnabled = False

        ElseIf txtUnyo.ppFromText <> String.Empty _
            Or txtUnyo.ppToText <> String.Empty Then

            txtJBNum.ppEnabled = False
            txtUnyo.ppEnabled = True

        Else

            txtJBNum.ppEnabled = True
            txtUnyo.ppEnabled = True

        End If

    End Sub

    ''' <summary>
    ''' ICカード履歴調査一覧(詳細)の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetICRireki()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strTime_f As String = Nothing
        Dim strTime_t As String = Nothing
        Dim vsKey() As String = ViewState(P_KEY)
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

                '開始ログ出力
                psLogStart(Me)

                strTime_f = Me.txtJikantai.ppHourTextFrom + Me.txtJikantai.ppMinTextFrom
                strTime_t = Me.txtJikantai.ppHourTextTo + Me.txtJikantai.ppMinTextTo

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                'ICカード履歴調査
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_shokai_seq", SqlDbType.NVarChar, vsKey(0).Substring(1, 5)))  '照会通番
                    .Add(pfSet_Param("prm_server_dt", SqlDbType.NVarChar, vsKey(1)))                   '照会日時
                    .Add(pfSet_Param("prm_ctrl_no", SqlDbType.NVarChar, vsKey(11)))                    '管理番号
                    .Add(pfSet_Param("prm_nl_cls", SqlDbType.NVarChar, vsKey(3)))                      'NL区分
                    .Add(pfSet_Param("prm_seq", SqlDbType.Int, 1))                                     '連番
                    .Add(pfSet_Param("prm_ic_id", SqlDbType.NVarChar, "3"))                            'IC/ID区分
                    .Add(pfSet_Param("prm_eda_seq_tennai", SqlDbType.NVarChar, "01"))                  '枝番(店内)
                    .Add(pfSet_Param("prm_eda_seq_saiken", SqlDbType.NVarChar, "02"))                  '枝番(債権)
                    .Add(pfSet_Param("prm_eda_seq_saimu", SqlDbType.NVarChar, "03"))                   '枝番(債務)
                    .Add(pfSet_Param("prm_eda_seq_seisan", SqlDbType.NVarChar, "04"))                  '枝番(精算)
                    .Add(pfSet_Param("prm_cid", SqlDbType.NVarChar, Me.txtCID.ppText))                 'CID
                    .Add(pfSet_Param("prm_unyo_f", SqlDbType.NVarChar, Me.txtUnyo.ppFromText))         '運用機番FROM
                    .Add(pfSet_Param("prm_unyo_t", SqlDbType.NVarChar, Me.txtUnyo.ppToText))           '運用機番TO
                    .Add(pfSet_Param("prm_jb_f", SqlDbType.NVarChar, Me.txtJBNum.ppFromText))          'JB番号FROM
                    .Add(pfSet_Param("prm_jb_t", SqlDbType.NVarChar, Me.txtJBNum.ppToText))            'JB番号TO
                    .Add(pfSet_Param("prm_time_f", SqlDbType.NVarChar, strTime_f))                     '時間帯FROM
                    .Add(pfSet_Param("prm_time_t", SqlDbType.NVarChar, strTime_t))                     '時間帯TO
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                Select Case strOKNG
                    Case "0"         'データ無し

                        '件数を設定
                        Master.ppCount = "0"

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                        '0件
                        '--------------------------------
                        '2014/05/12 後藤　ここから
                        '--------------------------------
                        'psMesBox(Me, "00007", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        '--------------------------------
                        '2014/05/12 後藤　ここまで
                        '--------------------------------

                        '印刷ボタンを非活性化
                        Master.ppRigthButton3.Enabled = False

                    Case Else        'データ有り

                        '件数を設定
                        'Master.ppCount = dstOrders.Tables(0).Rows(0).Item("該当件数").ToString
                        Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString

                        '検索時、該当件数が表示件数より大きい場合
                        'If CInt(Master.ppCount) _
                        '   > CInt(dstOrders.Tables(0).Rows.Count) Then
                        If Not CInt(Master.ppCount) _
                           >= CInt(dstOrders.Tables(0).Rows(0).Item("該当件数").ToString) Then

                            psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, dstOrders.Tables(0).Rows(0).Item("該当件数").ToString, Master.ppCount)

                        End If

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = dstOrders.Tables(0)

                        '変更を反映
                        Me.grvList.DataBind()

                        '印刷ボタンを活性化
                        Master.ppRigthButton3.Enabled = True

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
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
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
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

                '終了ログ出力
                psLogEnd(Me)

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' 使用中カードＤＢの照会
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_InsUseCD(ByVal TboxID As String _
                          , ByVal CID As String)

        Dim conDB As SqlConnection = Nothing                        'SqlConnectionクラス
        Dim cmdDB As SqlCommand = Nothing                           'SqlCommandクラス
        Dim trans As SqlClient.SqlTransaction                       'トランザクション
        Dim objDs As DataSet = Nothing                              'データセット
        'Dim strIrai As String = DateTime.Parse("yyyyMMdd").ToString '要求通番
        Dim strView_st() As String = Me.ViewState(P_KEY)            'キー項目の設定
        Dim strOKNG As String = Nothing                             '検索結果
        Dim strResult As String = Nothing                           '検索結果
        Dim dstOrders As New DataSet
        Dim maxSeq As String = Nothing
        Dim errMsg As String = "集信エラー情報"                     'エラーメッセージ用
        Dim strKKTboxId As String = Nothing
        Dim strUseCardNum As String = "25"                          '種別の切り替え
        Dim strDateClass As String = "302"                          'ICカード履歴の処理コード切り替え
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then

            'トランザクションの設定
            trans = conDB.BeginTransaction

            Try

                '開始ログ出力
                psLogStart(Me)

                '管理番号の作成
                Dim cntrol_num As String = Date.Now.ToString("yyyyMMdd")

                '管理番号採番
                'パラメータ設定
                cmdDB = New SqlCommand("ZCMPSEL022", conDB)
                cmdDB.Transaction = trans
                With cmdDB.Parameters
                    'パラメータ設定 
                    .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 1))                                      '管理番号
                    .Add(pfSet_Param("YMD", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))           '年月日
                    .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))           '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '戻り値確認
                If cmdDB.Parameters("SalesYTD").Value.ToString Is Nothing Then

                    '処理終了
                    Throw New Exception

                End If

                'トランザクションの設定
                'cmdDB.Transaction = trans
                'cmdDB.CommandType = CommandType.StoredProcedure
                'cmdDB.ExecuteNonQuery()

                '管理番号を作成する
                'cntrol_num = cntrol_num.Substring(2, 2) _
                '             + "_" _
                '             + cntrol_num.Substring(2, 2) _
                '             + "_" _
                '             + cmdDB.Parameters("SalesYTD").Value.ToString("0000")

                cntrol_num = Date.Now.ToString("yyyyMMdd") _
                             + CInt(cmdDB.Parameters("SalesYTD").Value).ToString("0000")

                '拡張TBOXIDの設定
                If strView_st(3) = "N" Or strView_st(3) = "J" Then

                    'cntrol_num = "GI" + "_" + cntrol_num
                    strKKTboxId = "20"
                Else

                    'cntrol_num = "LI" + "_" + cntrol_num
                    strKKTboxId = "10"

                End If

                '照会対象のデータ種別切り替え
                Select Case strView_st(8).ToString
                    Case "P", "R", "S", "X"
                        strUseCardNum = "f1"
                        strDateClass = "303"
                End Select

                '照会要求データ連番最大値取得
                'パラメータ設定
                cmdDB = New SqlCommand("ICHLSTP001_S4", conDB)
                cmdDB.Transaction = trans
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("ymd", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))            '要求日付
                    .Add(pfSet_Param("maxseq", SqlDbType.Int, 20, ParameterDirection.Output))              '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                maxSeq = cmdDB.Parameters("maxseq").Value.ToString("000000")

                Try

                    '照会要求データ
                    cmdDB = New SqlCommand("ICHLSTP001_I1", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prm_req_dt", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))          '要求日付
                        .Add(pfSet_Param("prm_req_seq", SqlDbType.NVarChar, CInt(maxSeq).ToString("000000")))       '要求通番
                        .Add(pfSet_Param("prm_cond_flg", SqlDbType.NVarChar, "0"))                                  '状態フラグ
                        .Add(pfSet_Param("prm_term_nm", SqlDbType.NVarChar, Session(P_SESSION_IP)))                 '端末情報
                        .Add(pfSet_Param("prm_user_id", SqlDbType.NVarChar, Session(P_SESSION_USERID)))             'ユーザID
                        .Add(pfSet_Param("prm_init_dt", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMddHHmmss")))   '作成日
                        .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, TboxID))                                 'TBOXID
                        .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, "302"))                                 '処理コード(使用中カードDB)
                        .Add(pfSet_Param("prm_ftp_snd", SqlDbType.NVarChar, "0"))                                   'FTP送信
                        .Add(pfSet_Param("prm_ftp_rcv", SqlDbType.NVarChar, "0"))                                   'FTP受信
                        .Add(pfSet_Param("prm_reqmng_no", SqlDbType.NVarChar, cntrol_num))                          '新管理番号
                    End With
                    'コマンドタイプ設定(ストアド)
                    errMsg = "照会要求データ"
                    'トランザクションの設定
                    cmdDB.Transaction = trans
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    '照会依頼データ
                    cmdDB = New SqlCommand(M_DISP_ID + "_I1", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prm_ctrl_no", SqlDbType.NVarChar, cntrol_num))
                        .Add(pfSet_Param("prm_seq", SqlDbType.Int, 1))
                        .Add(pfSet_Param("prm_nl_cls", SqlDbType.NVarChar, strView_st(3)))
                        .Add(pfSet_Param("prm_id_ic_cls", SqlDbType.NVarChar, strView_st(9)))
                        .Add(pfSet_Param("prm_reqseq_spc", SqlDbType.NChar, "000001"))
                        .Add(pfSet_Param("prm_reqdate_spc", SqlDbType.NChar, Date.Now.ToString("yyyyMMddHHmmss")))
                        .Add(pfSet_Param("prm_kkchutbxid", SqlDbType.NChar, strKKTboxId + TboxID))
                        .Add(pfSet_Param("prm_dataclass_1", SqlDbType.NChar, strUseCardNum))
                        .Add(pfSet_Param("prm_datadate_1", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_bbserialno_1", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_cid_1", SqlDbType.NChar, CID))
                        .Add(pfSet_Param("prm_denpyono_1", SqlDbType.NChar, ""))
                        .Add(pfSet_Param("prm_insert_usr", SqlDbType.NChar, Session(P_SESSION_USERID)))
                    End With
                    'コマンドタイプ設定(ストアド)
                    errMsg = "照会依頼データ"
                    'トランザクションの設定
                    cmdDB.Transaction = trans
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'ICカード履歴調査一覧
                    cmdDB = New SqlCommand("ICHLSTP001_I3", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("prm_response_no", SqlDbType.NVarChar, cntrol_num))                 '管理番号
                        .Add(pfSet_Param("prm_seqno", SqlDbType.Int, 1))                                     '連番
                        .Add(pfSet_Param("prm_tboxid", SqlDbType.NVarChar, TboxID))                          'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, strView_st(5)))                  'ホールコード
                        .Add(pfSet_Param("prm_hall_nm", SqlDbType.NVarChar, Me.lblHallName_Input.Text))      'ホール名
                        .Add(pfSet_Param("prm_system_cd", SqlDbType.NVarChar, strView_st(7)))                'システムコード
                        .Add(pfSet_Param("prm_system_nm", SqlDbType.NVarChar, strView_st(8)))                'システム
                        .Add(pfSet_Param("prm_version", SqlDbType.NVarChar, Me.lblTboxType_Input.Text))      'バージョン
                        .Add(pfSet_Param("prm_proc_cd", SqlDbType.NVarChar, strDateClass))                   '処理コード
                        .Add(pfSet_Param("prm_cardid", SqlDbType.NVarChar, CID))                             'カードＩＤ
                        .Add(pfSet_Param("prm_data_dt", SqlDbType.DateTime, DBNull.Value))                   'データ日付
                        .Add(pfSet_Param("prm_insert_usr", SqlDbType.NVarChar, Session(P_SESSION_USERID)))   '登録者
                    End With
                    'コマンドタイプ設定(ストアド)
                    errMsg = "ICカード履歴調査一覧"
                    'トランザクションの設定
                    cmdDB.Transaction = trans
                    cmdDB.CommandType = CommandType.StoredProcedure
                    cmdDB.ExecuteNonQuery()

                    'コミット
                    trans.Commit()

                Catch ex As Exception

                    'システムエラー
                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
                    '--------------------------------
                    '2014/04/14 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/14 星野　ここまで
                    '--------------------------------

                    'ロールバック
                    trans.Rollback()

                    Throw ex

                End Try

                '更新が正常終了
                psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, errMsg)

            Catch ex As SqlException
                '更新に失敗
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
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
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, errMsg)
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

                '終了ログ出力
                psLogEnd(Me)

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Throw New Exception
        End If

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strDelF As String = Nothing '削除フラグ
        Dim strVs() As String = ViewState(P_KEY)
        Dim strVs_Kensaku As String = ViewState("検索条件")

        'ＴＢＯＸ種別がP,R,S,Xの場合、ボタンを非活性とする
        For Each rowData As GridViewRow In grvList.Rows
            strDelF = strVs(8)

            'テーブルの列に検索条件を追加
            Select Case rowData.RowIndex

                Case 1

                    rowData.Cells(15).Text = strVs_Kensaku

            End Select

        Next

    End Sub

    ''' <summary>
    ''' 使用中カードＤＢの照会状態チェック
    ''' </summary>
    ''' <param name="TboxID"></param>
    ''' <param name="CID"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetUseCD(ByVal TboxID As String _
                          , ByVal CID As String _
                          , ByRef result As Boolean)

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

                '開始ログ出力
                psLogStart(Me)

                cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)

                '使用中カードDBの照会状況確認
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_ip", SqlDbType.NVarChar, Session(P_SESSION_IP)))             '端末情報
                    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, TboxID))                       'TBOXID
                    .Add(pfSet_Param("prm_cid", SqlDbType.NVarChar, CID))                              '使用中カードDB
                    .Add(pfSet_Param("prm_syori_cd", SqlDbType.NVarChar, "201"))                       '使用中カードDB
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output)) '結果取得用
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        result = True

                    Case Else        'データ有り


                        '検索失敗
                        psMesBox(Me, "20002", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        result = False

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                result = False
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＩＣカード履歴")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                result = False
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

                '終了ログ出力
                psLogEnd(Me)

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Master.ppCount = "0"

        End If

    End Sub

    ''' <summary>
    ''' PDF出力先取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ms_GetDirpath() As String

        Dim strPath As String = Nothing
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strAddress As String = Nothing
        Dim strFileclassCd As String = Nothing
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

                '開始ログ出力
                psLogStart(Me)

                'ファイル種別コード
                strFileclassCd = "0594AT"     'ICカード履歴調査

                cmdDB = New SqlCommand("ZCMPSEL009", conDB)

                'PDF出力先
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("FILECLASS_CD", SqlDbType.NVarChar, strFileclassCd))                    'ファイル種別
                    .Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.NVarChar, 20, ParameterDirection.Output))   'アドレス
                    .Add(pfSet_Param("FOLDER_NM", SqlDbType.NVarChar, 20, ParameterDirection.Output))        'フォルダ名
                End With

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                strPath = "\\" + cmdDB.Parameters("SERVER_ADDRESS").Value.ToString + "," + cmdDB.Parameters("FOLDER_NM").Value.ToString

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

                '終了ログ出力
                psLogEnd(Me)

            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Return Nothing

        End If

    End Function

    ' ''' <summary>
    ' ''' ダウンロードファイルテーブルの登録
    ' ''' </summary>
    ' ''' <param name="KanriNum"></param>
    ' ''' <param name="DispNum"></param>
    ' ''' <remarks></remarks>
    'Private Sub ms_InsDownload(ByVal KanriNum As String _
    '                         , ByVal DispNum As String _
    '                         , ByVal fileName As String _
    '                         , ByVal dirPath As String _
    '                         , ByVal CrateDate As String _
    '                         , ByVal ReportName As String)

    '    Dim conDB As SqlConnection = Nothing               'SqlConnectionクラス
    '    Dim cmdDB As SqlCommand = Nothing                  'SqlCommandクラス
    '    Dim objDs As DataSet = Nothing                     'データセット
    '    Dim strOKNG As String = Nothing                    '検索結果
    '    Dim strSprit() As String = Nothing                 'サーバアドレス、フォルダ名
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    '    objStack = New StackFrame
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------

    '    'サーバアドレス、フォルダ名の分割
    '    strSprit = dirPath.Split(",")

    '    '接続
    '    If pfOpen_Database(conDB) Then
    '        Try

    '            cmdDB = New SqlCommand("ZCMPINS001", conDB)
    '            With cmdDB.Parameters
    '                'パラメータ設定
    '                '照会要求親データ
    '                .Add(pfSet_Param("MNG_NO", SqlDbType.NVarChar, KanriNum))                               '管理番号
    '                .Add(pfSet_Param("FILE_CLS", SqlDbType.NVarChar, DispNum))                              'ファイル種別
    '                .Add(pfSet_Param("TITLE", SqlDbType.NVarChar, Master.Master.ppTitle))                   '画面タイトル
    '                .Add(pfSet_Param("FILE_NM", SqlDbType.NVarChar, fileName))                              'ファイル名
    '                .Add(pfSet_Param("REPORT_NM", SqlDbType.NVarChar, ReportName + CrateDate))              '帳票名(名称_yyyyMMddHHmmss)
    '                .Add(pfSet_Param("SERVER_ADDRESS", SqlDbType.NVarChar, strSprit(0)))                    'サーバアドレス
    '                .Add(pfSet_Param("KEEP_FOLD", SqlDbType.NVarChar, strSprit(1)))                         '保存先フォルダ
    '                '暫定              .Add(pfSet_Param("CREATE_DT", SqlDbType.DateTime, Date.Parse(CrateDate)))               '作成日
    '                .Add(pfSet_Param("CREATE_DT", SqlDbType.DateTime, Date.Now.ToString("yyyy/MM/dd HH:mm:ss")))     '作成日
    '                .Add(pfSet_Param("INSERT_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID).ToString)) 'ユーザＩＤ
    '            End With

    '            'データ追加／更新
    '            Using conTrn = conDB.BeginTransaction
    '                cmdDB.Transaction = conTrn
    '                'コマンドタイプ設定(ストアド)
    '                cmdDB.CommandType = CommandType.StoredProcedure
    '                cmdDB.ExecuteNonQuery()
    '                'コミット
    '                conTrn.Commit()
    '            End Using

    '            '更新が正常終了
    '            psMesBox(Me, "00003", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後, "ダウンロードファイル")

    '        Catch ex As SqlException
    '            '更新に失敗
    '            psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ダウンロードファイル")
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    ''ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------
    '            Throw ex
    '        Catch ex As Exception
    '            'システムエラー
    '            psMesBox(Me, "00001", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, "ダウンロードファイル")
    ''--------------------------------
    ''2014/04/14 星野　ここから
    ''--------------------------------
    ''ログ出力
    '            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
    ''--------------------------------
    ''2014/04/14 星野　ここまで
    ''--------------------------------
    '            Throw ex
    '        Finally
    '            'DB切断
    '            If Not pfClose_Database(conDB) Then
    '                psMesBox(Me, "00006", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
    '            End If

    '        End Try
    '    Else
    '        psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
    '        Throw New Exception
    '    End If

    'End Sub

    ''' <summary>
    ''' PDF出力用のデータテーブル作成
    ''' </summary>
    ''' <param name="dtPDF"></param>
    ''' <remarks></remarks>
    Private Sub ms_GetPdfDatatable(ByRef dtPDF As DataTable)

        Dim dtRow As DataRow
        Dim Search As String = Me.ViewState("検索条件")
        Dim dtTable As DataTable = pfParse_DataTable(Me.grvList)
        Dim strDate As String = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")

        '項目作成

        dtPDF.Columns.Add("TBOXID")
        dtPDF.Columns.Add("ホール名")
        dtPDF.Columns.Add("TBOXバージョン")
        dtPDF.Columns.Add("データ日付")
        dtPDF.Columns.Add("ログ検知日時")
        dtPDF.Columns.Add("ＣＩＤ")
        dtPDF.Columns.Add("運用機番")
        dtPDF.Columns.Add("ＪＢ番号")
        dtPDF.Columns.Add("ＢＢ種別")
        dtPDF.Columns.Add("正副種別")
        dtPDF.Columns.Add("入金種別")
        dtPDF.Columns.Add("入金金額")
        dtPDF.Columns.Add("前残高")
        dtPDF.Columns.Add("後残高")
        dtPDF.Columns.Add("カード種別")
        dtPDF.Columns.Add("店番")
        dtPDF.Columns.Add("表示条件")
        dtPDF.Columns.Add("出力日時")

        'For i As Integer = 0 To grvList.Rows.Count - 1
        For i As Integer = 0 To dtTable.Rows.Count - 1
            dtRow = dtPDF.NewRow()          'データテーブルの行定義

            dtRow("TBOXID") = Me.lblTboxID_Input.Text
            dtRow("ホール名") = Me.lblHallName_Input.Text
            dtRow("TBOXバージョン") = Me.lblTboxType_Input.Text
            dtRow("データ日付") = Me.lblDataDT_Input.Text
            dtRow("ログ検知日時") = dtTable.Rows(i).Item(0)
            dtRow("ＣＩＤ") = dtTable.Rows(i).Item(1)
            dtRow("運用機番") = dtTable.Rows(i).Item(2)
            dtRow("ＪＢ番号") = dtTable.Rows(i).Item(3)
            dtRow("ＢＢ種別") = dtTable.Rows(i).Item(4)
            dtRow("正副種別") = dtTable.Rows(i).Item(6)
            dtRow("入金種別") = dtTable.Rows(i).Item(7)
            dtRow("入金金額") = dtTable.Rows(i).Item(8)
            dtRow("前残高") = dtTable.Rows(i).Item(9)
            dtRow("後残高") = dtTable.Rows(i).Item(10)
            dtRow("カード種別") = dtTable.Rows(i).Item(11)
            dtRow("店番") = dtTable.Rows(i).Item(12)
            'dtRow("ログ検知日時") = Me.grvList.Rows(i).Cells(0)
            'dtRow("ＣＩＤ") = Me.grvList.Rows(i).Cells(1)
            'dtRow("運用機番") = Me.grvList.Rows(i).Cells(3)
            'dtRow("ＪＢ番号") = Me.grvList.Rows(i).Cells(4)
            'dtRow("ＢＢ種別") = Me.grvList.Rows(i).Cells(5)
            'dtRow("正/副") = Me.grvList.Rows(i).Cells(7)
            'dtRow("入金種別") = Me.grvList.Rows(i).Cells(8)
            'dtRow("入金金額") = Me.grvList.Rows(i).Cells(9)
            'dtRow("更新前残") = Me.grvList.Rows(i).Cells(10)
            'dtRow("更新後残") = Me.grvList.Rows(i).Cells(11)
            'dtRow("カード種別") = Me.grvList.Rows(i).Cells(12)
            'dtRow("店番") = Me.grvList.Rows(i).Cells(13)
            dtRow("表示条件") = Search
            dtRow("出力日時") = strDate

            dtPDF.Rows.Add(dtRow)

        Next

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
