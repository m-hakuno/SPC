'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　工事検収書
'*　ＰＧＭＩＤ：　CNSOUTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.03.06　：　ＸＸＸ
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CNSOUTP001-001     2015/08/06      栗原　　　帳票フッター用項目を追加


'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports System.Data.SqlClient

Public Class CNSOUTP001
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
    Private Const M_DISP_ID = P_FUN_CNS & P_SCR_OUT & P_PAGE & "001"
    Private Const KENSYU_YM As String = "KensyuYM" 'ViewStateでの年月保管用

    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
    'ＤＢ処理　制御
    Private Enum menmProcFlg
        GetList = 1 '出力帳票取得
        GetData = 2 '一覧データ取得
        GetPrim = 3 '画面状態取得
        GetChkB = 4 '検収書用データ取得
        GetChkS = 5 '検収書用集計データ取得
        GetPrnt = 12 '検収書印刷用データ取得
        GetCSVF = 13 'ＣＳＶファイル排出用
        PutClse = 6 '締め更新
        PutUCls = 7 '締め解除更新
        PutChkB1 = 8 '検収書挿入(D55)
        PutChkB2 = 9 '検収書挿入
        DelChkB1 = 10 '検収書削除(D55)
        DelChkB2 = 11 '検収書削除
    End Enum

    '画面表示　制御
    Private Enum menmDispCntl
        ALLDis = 0 '全未使用
        NoCulc = 1 '未計算(前月請求締め済)
        NoClse = 2 '未締め
        RClose = 3 '締め済
        InpErr = 4 '入力エラー発生
        ZenNoClse = 5 '前月請求未締め
        First = 9
    End Enum

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim mcdbDB As New SqlConnection     'ＤＢ接続
    Dim dtbDumm As New DataTable        '空データグリッド表示用
    Dim dtbPrntList As New DataTable    '対象帳票作成用データテーブル
    Dim dtbSrchList As New DataTable    '検索結果表示用データテーブル
    Dim dtbDispList As New DataTable    '画面制御等用データテーブル
    Dim dtbCulcList As New DataTable    '当月算出結果テーブル
    Dim mobjDispCntl As menmDispCntl    '締め状態の保管

    Dim mstrDefYM As String = ""        '当月の保管
    Dim mblnErr As Boolean = False      '入力エラー有無
    Dim mstrSelYM As String = ""
    Dim mstrSelDoc As String = ""
    Dim marySelDoc As String()
    Dim strClsFlg As String = "0"

    '--------------------------------
    '2014/04/15 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/15 星野　ここまで
    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================

    '============================================================================================================================
    '=　イベントプロシージャ
    '============================================================================================================================
    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(grvList, M_DISP_ID)

        '件数を非表設定
        '-----------------------------
        '2014/04/22 Hamamoto ここから
        '-----------------------------
        Master.ppCount = "0"
        '-----------------------------
        '2014/04/22 Hamamoto ここまで
        '-----------------------------
    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　ページロード　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ページ読み取り時の処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        'ログ出力開始
        Call psLogStart(Me)

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(mcdbDB) = False Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

        Try
            '画面のボタンを使用不可
            mobjDispCntl = menmDispCntl.ALLDis

            If Not IsPostBack Then  '初回表示
                ''検索ボタンのイベント関連付け
                'AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
                ''検索クリアボタンのイベント関連付け
                'AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
                'AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnCloseM_Click
                'AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnCulc_Click

                ''--------------------------------
                ''2015/01/27 加賀　ここから
                ''--------------------------------
                ''処理中メッセージの設定
                'Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め確認
                ''btnUnCloseM.OnClientClick = "dispWait('unclose');"
                'Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '当月集計確認
                'Master.ppRigthButton1.Attributes("onClick") = "dispWait('search');"

                '--------------------------------
                '2015/01/27 加賀　ここまで
                '--------------------------------

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = M_DISP_ID
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                Master.ppCount = "0"

                '「締め／解除」「当月集計」のボタン表示設定
                Master.Master.ppRigthButton1.Visible = True '当月集計ボタン
                Master.Master.ppRigthButton2.Visible = True '締め/締め解除ボタン
                Master.Master.ppRigthButton1.Enabled = False '当月集計ボタン
                Master.Master.ppRigthButton2.Enabled = False '締め/締め解除ボタン

                '「検索」「検索条件クリア」「締め／解除」「当月集計」のボタン活性
                Master.Master.ppRigthButton1.Text = "当月集計"
                Master.Master.ppRigthButton2.Text = "締め"


                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'グリッドの初期化
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()

                '検索クリアボタン押下時の検証を無効
                Master.ppRigthButton2.CausesValidation = False

                '初期表示
                mobjDispCntl = menmDispCntl.First

            Else

                Dim zz As Integer = 0
                Dim yy As Integer = 0

                If ViewState("PrintList") Is Nothing Then
                Else
                    dtbPrntList = DirectCast(ViewState("PrintList"), DataTable).Copy
                End If
                If ViewState("SrchList") Is Nothing Then
                Else
                    dtbSrchList = DirectCast(ViewState("SrchList"), DataTable).Copy
                End If

                '締め状態の取得
                If txtNendo.ppText <> String.Empty Then
                    Dim mdateDef As DateTime
                    If Date.TryParse(txtNendo.ppText, mdateDef) = True And txtNendo.ppText.Length = 7 Then
                        Call sSetDefYM()
                    Else
                        mstrDefYM = String.Empty
                    End If
                Else
                    'グリッドの初期化
                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()

                    '検索クリアボタン押下時の検証を無効
                    Master.ppRigthButton2.CausesValidation = False

                    '初期表示
                    mobjDispCntl = menmDispCntl.First
                End If

            End If


            'マルチビューの表示設定
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ボタンアクションの設定.
            Call msSet_ButtonAction()

        Catch ex As Exception
            '画面初期化に失敗
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        Call psLogEnd(Me)

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　ページ描画前処理　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ページ描画前
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CNSOUTP001_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------
        Try

            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            If dtbSrchList Is Nothing Then
                grvList.DataSource = dtbDumm
                Master.ppCount = dtbDumm.Rows.Count
            Else
                grvList.DataSource = dtbSrchList
                Master.ppCount = dtbSrchList.Rows.Count
            End If
            grvList.DataBind()
            grvList.ShowHeaderWhenEmpty = True

            Call sDispCntl(mobjDispCntl)

            'VIEWSTATEにデータの保管
            ViewState("PrintList") = dtbPrntList
            ViewState("SrchList") = dtbSrchList

            'DB切断
            If clsDataConnect.pfClose_Database(mcdbDB) = False Then
                '切断に失敗したからといってどうもしないが、切断失敗は表示する
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '締め処理を行った場合、印刷処理を行う
            pfPrint()

        Catch ex As Exception
            'データの表示に失敗
            psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, ex.Message)
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　検索ボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click()

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        Try
            Call sCheck_Error()

            '入力エラーなしのとき
            If Page.IsValid Then
                If fGetData(menmProcFlg.GetData, dtbSrchList) = True Then
                    If dtbSrchList Is Nothing Then
                        '検索失敗の表示
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収情報")
                        mobjDispCntl = menmDispCntl.ALLDis
                    Else
                        If dtbSrchList.Rows.Count > 0 Then
                            grvList.DataSource = dtbSrchList
                            grvList.DataBind()
                        Else
                            'データなしの表示
                            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        End If
                    End If
                Else
                    '検索失敗の表示
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収情報")
                    mobjDispCntl = menmDispCntl.ALLDis
                End If
                '入力エラーありのとき
            Else
                '入力エラー発生
                mobjDispCntl = menmDispCntl.First
            End If
        Catch ex As Exception
            '検索処理に失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書情報")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　検索クリアボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 検索クリアボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        '入力値クリア
        Me.txtNendo.ppText = ""

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　締めボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 締めボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCloseM_Click(sender As Object, e As EventArgs)

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim intRtn As Integer

        'ログ出力開始
        psLogStart(Me)

        Try
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                'トランザクション.
                Using conTrn = conDB.BeginTransaction

                    Select Case Master.Master.ppRigthButton2.Text
                        '接続
                        Case "締め"
                            cmdDB = New SqlCommand("CNSOUTP001_U1", conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = conTrn
                            With cmdDB.Parameters
                                .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            mobjDispCntl = menmDispCntl.RClose
                            Master.Master.ppRigthButton2.Text = "締め解除"
                            Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('unclose');" '締め/締め解除確認
                            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事検収書　締め")
                            'psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　締め", "00010")

                            strClsFlg = "1"

                        Case "締め解除"
                            cmdDB = New SqlCommand("CNSOUTP001_U2", conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = conTrn
                            With cmdDB.Parameters
                                .Add(pfSet_Param("update_user", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            mobjDispCntl = menmDispCntl.NoClse
                            psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事検収書　締め解除")
                            Master.Master.ppRigthButton2.Text = "締め"
                            Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め/締め解除確認
                            'psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　締め解除", "00010")

                    End Select
                    '実行
                    cmdDB.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

                        'ロールバック
                        conTrn.Rollback()

                        Exit Sub
                    End If

                    'コミット
                    conTrn.Commit()

                    '完了メッセージ.
                    'psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppRigthButton2.Text)

                    '再検索
                    btnSearch_Click()

                    '締め状態の取得
                    Call sSetDefYM()

                End Using

            End If
        Catch ex As Exception
            '締め解除処理に失敗
            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　締め", "00010")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''--------------------------------------------------------------------------------------------------------
    ''-　締め解除ボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    ''-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    ''--------------------------------------------------------------------------------------------------------
    ' ''' <summary>
    ' ''' 締め解除ボタン押下処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub btnUnCloseM_Click(sender As Object, e As EventArgs) Handles btnUnCloseM.Click

    '    '--------------------------------
    '    '2014/04/15 星野　ここから
    '    '--------------------------------
    '    objStack = New StackFrame
    '    '--------------------------------
    '    '2014/04/15 星野　ここまで
    '    '--------------------------------

    '    'ログ出力開始
    '    psLogStart(Me)

    '    Try
    '        If (Page.IsValid) Then
    '            If fPutData(menmProcFlg.PutUCls) = True Then
    '                mobjDispCntl = menmDispCntl.NoClse
    '                psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事検収書　締め解除")
    '            Else
    '                psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　締め解除", "00010")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        '締め解除処理に失敗
    '        psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　締め解除", "00010")
    '        '--------------------------------
    '        '2014/04/15 星野　ここから
    '        '--------------------------------
    '        'ログ出力
    '        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
    '                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
    '        '--------------------------------
    '        '2014/04/15 星野　ここまで
    '        '--------------------------------
    '    End Try

    '    'ログ出力終了
    '    psLogEnd(Me)

    'End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　当月算出ボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 当月算出ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCulc_Click_(sender As Object, e As EventArgs)

        Dim cmdDB As New SqlCommand
        Dim cmdDel1 As New SqlCommand
        Dim cmdDel2 As New SqlCommand
        Dim cmdIns1 As New SqlCommand
        Dim cmdIns2 As New SqlCommand
        Dim conDB As New SqlConnection
        Dim dstBuff As New DataSet
        Dim stbRetSQL1 As New StringBuilder
        Dim stbRetSQL2 As New StringBuilder
        Dim dstOrders1 As New DataSet '当月情報
        Dim dtbBuff As New DataTable
        'Dim yy As Integer
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            'トランザクション開始
            Using conTrn = conDB.BeginTransaction

                Try
                    'ＳｑｌＣｏｍｍａｎｄにＣｏｎｎｅｃｔｉｏｎとＴｒａｎｓａｃｔｉｏｎをリンク
                    cmdDB.Connection = conDB
                    cmdDB.Transaction = conTrn
                    Call sRemoveCmdParm(cmdDB)
                    '当月情報取得

                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S3", conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))          '適用日
                    End With

                    'データ取得およびデータをリストに設定
                    dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)

                    '当月算出結果削除1
                    'If DelChk(menmProcFlg.DelChkB1, conDb, cmdDB, conTrn) = False Then
                    '    conTrn.Rollback()
                    '    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部情報削除")
                    '    Exit Sub
                    'End If

                    cmdDel1 = New SqlCommand("CNSOUTP001_D1", conDB)
                    cmdDel1.CommandType = CommandType.StoredProcedure
                    cmdDel1.Transaction = conTrn
                    With cmdDB.Parameters
                        .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                        .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                    End With

                    '実行
                    cmdDel1.ExecuteNonQuery()

                    'D55へのインサート
                    If PutChk(menmProcFlg.PutChkB1, conDB, cmdDB, conTrn) = False Then
                        conTrn.Rollback()
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　当月情報登録")
                        Exit Sub
                    End If

                    'D89への値取得
                    stbRetSQL1.Clear()
                    If fGetData(menmProcFlg.GetChkS, dtbCulcList, conDB, cmdDB, conTrn) = False Then
                        conTrn.Rollback()
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　当月情報")
                        Exit Sub
                    End If

                    Dim decTotAmount As Decimal = 0 '小計金額
                    Dim decRate As Decimal = 0      '値引き率
                    Dim decTaxRate As Decimal = 0   '消費税率
                    Dim decRdctAmnt As Decimal = 0  '値引き額
                    Dim decTaxAmnt As Decimal = 0   '消費税額
                    Dim decTotal As Decimal = 0     '合計

                    decRate = dtbCulcList.Rows(0)("RATE")
                    decTaxRate = dtbCulcList.Rows(0)("TAXRATE")
                    If dtbCulcList.Rows(0)("NMLCNST") IsNot DBNull.Value Then
                        decTotAmount += dtbCulcList.Rows(0)("NMLCNST")
                    End If
                    If dtbCulcList.Rows(0)("NEWCNST") IsNot DBNull.Value Then
                        decTotAmount += dtbCulcList.Rows(0)("NEWCNST")
                    End If

                    decRdctAmnt = Math.Floor(decTotAmount * decRate)
                    decTaxAmnt = Math.Floor((decTotAmount - decRdctAmnt) * decTaxRate)
                    decTotal = decTotAmount - decRdctAmnt + decTaxAmnt

                    'D89の削除
                    '当月算出結果削除2
                    If DelChk(menmProcFlg.DelChkB2, conDB, cmdDB, conTrn) = False Then
                        conTrn.Rollback()
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部情報削除")
                        Exit Sub
                    End If

                    'D89へのインサート
                    If PutChk(menmProcFlg.PutChkB2, conDB, cmdDB, conTrn) = False Then
                        conTrn.Rollback()
                        psMesBox(Me, "00008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　当月情報登録")
                        Exit Sub
                    End If

                    '当月算出　成功
                    '                    trnDB.Commit()
                    cmdDB.Transaction.Commit()
                    'ボタン制御
                    'mobjDispCntl = menmDispCntl.NoClse
                    '締め状態確認
                    sSetDefYM()
                    '処理成功メッセージ
                    psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事検収書　当月情報")
                Catch ex As Exception
                    If cmdDB IsNot Nothing Then
                        '                        trnDB.Rollback()
                        cmdDB.Transaction.Rollback()
                    End If
                    psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　当月情報", "00010")
                    '--------------------------------
                    '2014/04/15 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/15 星野　ここまで
                    '--------------------------------
                    Exit Sub
                Finally
                    '後始末
                    cmdDB.Dispose()
                    conTrn.Dispose()
                End Try

            End Using

        Catch ex As Exception
            '締め処理に失敗
            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　当月情報", "00010")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        Finally
            '最後にしつこく後始末
            If cmdDB IsNot Nothing Then cmdDB.Dispose()
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　当月算出ボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 当月算出ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCulc_Click(sender As Object, e As EventArgs)

        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim cmdDel1 As New SqlCommand
        Dim cmdDel2 As New SqlCommand
        Dim cmdIns1 As New SqlCommand
        Dim cmdIns2 As New SqlCommand
        Dim dstOrders1 As New DataSet       '当月情報
        Dim dstOrders2 As New DataSet       'D89更新用
        Dim intRtn As Integer
        'Dim brPC_flg As Boolean = True
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        Try

            '接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                Try

                    '当月情報取得
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S3", conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))          '適用日
                    End With

                    'データ取得およびデータをリストに設定
                    dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)


                    'D89更新用データ取得
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S4", conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))          '適用日
                    End With

                    'データ取得およびデータをリストに設定
                    dstOrders2 = clsDataConnect.pfGet_DataSet(cmdDB)
                    dtbCulcList = dstOrders2.Tables(0)

                    'データ登録/更新
                    Using conTrn = conDB.BeginTransaction


                        '当月算出結果削除1
                        cmdDel1 = New SqlCommand("CNSOUTP001_D1", conDB)
                        With cmdDel1.Parameters
                            .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        cmdDel1.Transaction = conTrn
                        cmdDel1.CommandType = CommandType.StoredProcedure
                        cmdDel1.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDel1.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理")
                            'ロールバック
                            conTrn.Rollback()
                            Exit Sub
                        End If


                        'ＴＢＯＸ種別毎にレコード追加
                        For zz = 0 To dstOrders1.Tables(0).Rows.Count - 1
                            Dim decNmlAmnt As Integer = 0
                            Dim decNewAmnt As Integer = 0
                            cmdIns1 = New SqlCommand("CNSOUTP001_I1", conDB)
                            With cmdIns1.Parameters
                                .Add(pfSet_Param("D55_CBOOK_NO", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("D55_DEGREE", SqlDbType.NVarChar, dstOrders1.Tables(0).Rows(zz)("D89_DEGREE").ToString))
                                .Add(pfSet_Param("D55_SEQ", SqlDbType.NVarChar, dstOrders1.Tables(0).Rows(zz)("M23_DISP_SEQ")))
                                .Add(pfSet_Param("D55_TBOXCLS_CD", SqlDbType.NVarChar, dstOrders1.Tables(0).Rows(zz)("M23_TBOXCLS").ToString))
                                If dstOrders1.Tables(0).Rows(zz)("NMLCNST") Is DBNull.Value Then
                                    .Add(pfSet_Param("D55_NML_AMOUNT", SqlDbType.NVarChar, 0))
                                Else
                                    .Add(pfSet_Param("D55_NML_AMOUNT", SqlDbType.NVarChar, dstOrders1.Tables(0).Rows(zz)("NMLCNST")))
                                    decNmlAmnt = dstOrders1.Tables(0).Rows(zz)("NMLCNST")
                                End If
                                If dstOrders1.Tables(0).Rows(zz)("NEWCNST") Is DBNull.Value Then
                                    .Add(pfSet_Param("D55_NEW_AMOUNT", SqlDbType.NVarChar, 0))
                                Else
                                    .Add(pfSet_Param("D55_NEW_AMOUNT", SqlDbType.NVarChar, dstOrders1.Tables(0).Rows(zz)("NEWCNST")))
                                    decNewAmnt = dstOrders1.Tables(0).Rows(zz)("NEWCNST")
                                End If
                                .Add(pfSet_Param("D55_TOTAL", SqlDbType.NVarChar, decNmlAmnt + decNewAmnt))
                                .Add(pfSet_Param("D55_CHARGE", SqlDbType.NVarChar, String.Empty))
                                .Add(pfSet_Param("D55_INSERT_USR", SqlDbType.NVarChar, DirectCast(Session(P_SESSION_USERID), String)))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            'ＳＱＬ実行
                            cmdIns1.Transaction = conTrn
                            cmdIns1.CommandType = CommandType.StoredProcedure
                            cmdIns1.ExecuteNonQuery()

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdIns1.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ更新処理")
                                'ロールバック
                                conTrn.Rollback()
                                Exit Sub
                            End If

                        Next


                        Dim decTotAmount As Decimal = 0 '小計金額
                        Dim decRate As Decimal = 0      '値引き率
                        Dim decTaxRate As Decimal = 0   '消費税率
                        Dim decRdctAmnt As Decimal = 0  '値引き額
                        Dim decTaxAmnt As Decimal = 0   '消費税額
                        Dim decTotal As Decimal = 0     '合計

                        decRate = dtbCulcList.Rows(0)("RATE")
                        decTaxRate = dtbCulcList.Rows(0)("TAXRATE")
                        If dtbCulcList.Rows(0)("NMLCNST") IsNot DBNull.Value Then
                            decTotAmount += dtbCulcList.Rows(0)("NMLCNST")
                        End If
                        If dtbCulcList.Rows(0)("NEWCNST") IsNot DBNull.Value Then
                            decTotAmount += dtbCulcList.Rows(0)("NEWCNST")
                        End If

                        decRdctAmnt = Math.Floor(decTotAmount * decRate)
                        decTaxAmnt = Math.Floor((decTotAmount - decRdctAmnt) * decTaxRate)
                        decTotal = decTotAmount - decRdctAmnt + decTaxAmnt


                        '当月算出結果削除2
                        cmdDel2 = New SqlCommand("CNSOUTP001_D2", conDB)
                        With cmdDel2.Parameters
                            .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        cmdDel2.Transaction = conTrn
                        cmdDel2.CommandType = CommandType.StoredProcedure
                        cmdDel2.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDel2.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理")
                            'ロールバック
                            conTrn.Rollback()
                            Exit Sub
                        End If

                        cmdIns2 = New SqlCommand("CNSOUTP001_I2", conDB)
                        With cmdIns2.Parameters
                            .Add(pfSet_Param("D89_DEGREE", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("D89_INS_CLS", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("D89_REQ_CLS", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("D89_CNST_PRICE", SqlDbType.NVarChar, decTotAmount))
                            .Add(pfSet_Param("D89_MNG_NO", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("D89_DELETE_FLG", SqlDbType.NVarChar, "0"))
                            .Add(pfSet_Param("D89_INSERT_USR", SqlDbType.NVarChar, DirectCast(Session(P_SESSION_USERID), String)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        'ＳＱＬ実行
                        cmdIns2.Transaction = conTrn
                        cmdIns2.CommandType = CommandType.StoredProcedure
                        cmdIns2.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdIns2.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理")
                            'ロールバック
                            conTrn.Rollback()
                            Exit Sub
                        End If

                        conTrn.Commit()
                    End Using

                    btnSearch_Click()

                    'ボタン制御
                    'mobjDispCntl = menmDispCntl.NoClse
                    '締め状態確認
                    sSetDefYM()
                    '処理成功メッセージ
                    psMesBox(Me, "00009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "工事検収書　当月情報")

                Catch ex As Exception
                    'メッセージ表示
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            Else
                'メッセージ表示
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If


        Catch ex As Exception
            '締め処理に失敗
            psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　当月情報", "00010")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        Finally
            '最後にしつこく後始末
            If cmdDB IsNot Nothing Then cmdDB.Dispose()
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　明細　印刷ボタン／ＣＳＶボタン　クリック　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 明細　印刷ボタン／ＣＳＶボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        If e.CommandName <> "btnPrint" And e.CommandName <> "btnCsv" Then
            Exit Sub
        End If
        ' ＤＢ接続変数
        Dim dstOrders As New DataSet
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstBuff As New DataSet
        Dim dtbBuff As New DataTable
        Dim strRepCD As String = ""
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
            Select Case e.CommandName
                '印刷ボタン
                Case "btnPrint"
                    strRepCD = DirectCast(grvList.Rows(e.CommandArgument).FindControl("DOCCD"), TextBox).Text   '文書種別
                    mstrSelYM = DirectCast(grvList.Rows(e.CommandArgument).FindControl("DEGREE"), TextBox).Text   '年月保管
                    Select Case strRepCD
                        Case "01" '工事検収書
                            Dim rpt As DOCREP003
                            Dim strFNm As String
                            'パラメータ設定
                            cmdDB = New SqlCommand("CNSOUTP001_S6", conDB)
                            strFNm = DirectCast(grvList.Rows(e.CommandArgument).FindControl("DEGREE"), TextBox).Text   '"工事検収書"
                            With cmdDB.Parameters
                                .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, strFNm))          '適用日
                            End With
                            rpt = New DOCREP003
                            'データ取得およびデータをリストに設定
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            psPrintPDF(Me, rpt, dstOrders.Tables(0), "工事検収書")

                        Case "02" '工事料金明細一覧
                            'ここに”工事料金明細一覧　集計”の出力を記述
                            'ここに”工事料金明細一覧　明細”の出力を記述
                        Case Else
                    End Select
                    'ＣＳＶボタン
                Case "btnCsv"
                    strRepCD = DirectCast(grvList.Rows(e.CommandArgument).FindControl("DOCCD"), TextBox).Text   '文書種別
                    mstrSelYM = DirectCast(grvList.Rows(e.CommandArgument).FindControl("DEGREE"), TextBox).Text   '年月保管
                    Select Case strRepCD
                        Case "01" '工事検収書
                            'パラメータ設定
                            cmdDB = New SqlCommand("CNSOUTP001_S7", conDB)
                            With cmdDB.Parameters
                                .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrSelYM))          '適用日
                            End With

                            'データ取得およびデータをリストに設定
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                            'CSVファイルダウンロード
                            If pfDLCsvFile("工事検収書" & mstrSelYM & "_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".csv", dstOrders.Tables(0), True, Me) <> 0 Then
                                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                            End If
                        Case "02" '工事料金明細一覧
                            'ここに”工事料金明細一覧　集計”の出力を記述
                            'ここに”工事料金明細一覧　明細”の出力を記述
                        Case Else
                    End Select
                    '他にはないが一応
                Case Else

            End Select

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception
            '締め処理に失敗
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)


    End Sub

    '============================================================================================================================
    '=　プロシージャ
    '============================================================================================================================

    '--------------------------------------------------------------------------------------------------------
    '-　画面コントロール制御　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 画面コントロール制御
    ''' </summary>
    ''' <param name="ipobjDispCntl"></param>
    ''' <remarks></remarks>
    Private Sub sDispCntl(ByVal ipobjDispCntl As menmDispCntl)

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        'ログ出力開始
        psLogStart(Me)

        Try
            Me.Master.Master.ppRigthButton1.Enabled = True
            Me.Master.Master.ppRigthButton2.Enabled = True
            Master.ppRigthButton1.Enabled = True
            Master.ppRigthButton2.Enabled = True
            txtNendo.ppEnabled = True

            Select Case ipobjDispCntl
                Case menmDispCntl.NoCulc
                    Me.Master.Master.ppRigthButton2.Text = "締め"
                    Me.Master.Master.ppRigthButton2.Enabled = False
                Case menmDispCntl.NoClse
                    Me.Master.Master.ppRigthButton2.Text = "締め"
                Case menmDispCntl.RClose
                    Me.Master.Master.ppRigthButton1.Enabled = False
                    Me.Master.Master.ppRigthButton2.Text = "締め解除"
                Case menmDispCntl.InpErr
                    Me.Master.Master.ppRigthButton2.Text = "締め"
                    Me.Master.Master.ppRigthButton1.Enabled = False
                    Me.Master.Master.ppRigthButton2.Enabled = False
                    Master.ppRigthButton1.Enabled = False
                    Master.ppRigthButton2.Enabled = False
                    txtNendo.ppEnabled = False
                Case menmDispCntl.First
                    Me.Master.Master.ppRigthButton2.Text = "締め"
                    Me.Master.Master.ppRigthButton1.Enabled = False
                    Me.Master.Master.ppRigthButton2.Enabled = False
                Case Else
                    Me.Master.Master.ppRigthButton2.Text = "締め"
                    Me.Master.Master.ppRigthButton1.Enabled = False
                    Me.Master.Master.ppRigthButton2.Enabled = False
                    Master.ppRigthButton1.Enabled = False
                    Master.ppRigthButton2.Enabled = False
                    txtNendo.ppEnabled = False
            End Select

        Catch ex As Exception
            '画面初期化に失敗
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　締め状態取得　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' 締め状態取得
    ''' </summary>
    ''' <remarks></remarks>
    Sub sSetDefYM()

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------
        Dim strTougetu As String = txtNendo.ppText.Substring(0, 4) & txtNendo.ppText.Substring(5, 2)
        Dim strZengetu As String = Date.Parse(txtNendo.ppText + "/01").AddMonths(-1).ToString("yyyyMM")
        Try
            If fGetData(menmProcFlg.GetPrim, dtbDispList) = True Then
                If dtbDispList Is Nothing Then
                    mobjDispCntl = menmDispCntl.ALLDis
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Else
                    If dtbDispList.Rows.Count > 0 Then
                        '当月未計算＋未締め＋請求未締め（全月請求締め）
                        If dtbDispList.Rows(0)("NOCLOSE_YM").ToString() = strZengetu _
                           And dtbDispList.Rows(0)("CLOSE_YM").ToString() <> strTougetu _
                           And dtbDispList.Rows(0)("REQ_YM").ToString() <> strTougetu Then
                            mobjDispCntl = menmDispCntl.NoCulc
                            If dtbDispList.Rows(0)("REQ_YM").ToString = String.Empty Then
                                mstrDefYM = strTougetu
                            Else
                                'mstrDefYM = DateTime.Parse(Integer.Parse(dtbDispList.Rows(0)("REQ_YM")).ToString("####/##") & "/01").AddMonths(1).ToString("yyyyMM")
                                mstrDefYM = strTougetu
                            End If
                            'psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "前月の請求締めが行われていません")
                            '当月計算済＋検収締め＋請求未締め
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM").ToString() = strTougetu _
                        And dtbDispList.Rows(0)("CLOSE_YM").ToString() = strTougetu _
                        And dtbDispList.Rows(0)("REQ_YM").ToString() <> strTougetu _
                        And dtbDispList.Rows(0)("REQ_YM").ToString() = strZengetu Then
                            mobjDispCntl = menmDispCntl.RClose
                            'mstrDefYM = dtbDispList.Rows(0)("CLOSE_YM").ToString()
                            mstrDefYM = strTougetu
                            'psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "請求の締めが行われていません")
                            '当月計算済＋未締め＋請求未締め
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM").ToString() = strTougetu _
                        And dtbDispList.Rows(0)("CLOSE_YM").ToString() <> strTougetu _
                        And dtbDispList.Rows(0)("REQ_YM").ToString() <> strTougetu _
                        And dtbDispList.Rows(0)("REQ_YM").ToString() = strZengetu Then
                            mobjDispCntl = menmDispCntl.NoClse
                            'mstrDefYM = dtbDispList.Rows(0)("NOCLOSE_YM").ToString()
                            mstrDefYM = strTougetu
                            'psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "当月の締めが行われていません")
                            '全て締めていて翌月分も締めている
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM").ToString() = strTougetu _
                        And dtbDispList.Rows(0)("CLOSE_YM").ToString() = strTougetu _
                        And dtbDispList.Rows(0)("REQ_YM").ToString() = strTougetu _
                        And dtbDispList.Rows(0)("SAISIN_YM").ToString() <> strTougetu Then
                            mobjDispCntl = menmDispCntl.First
                            'mstrDefYM = dtbDispList.Rows(0)("CLOSE_YM").ToString()
                            mstrDefYM = strTougetu
                            'psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "前月の締めが行われていません")
                            '前月を締めていない
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM").ToString() <> strZengetu Then
                            mobjDispCntl = menmDispCntl.First
                            'mstrDefYM = dtbDispList.Rows(0)("NOCLOSE_YM").ToString()
                            mstrDefYM = strTougetu
                            '全件未締め＋請求未締め（この状態は起こらないはずだが念のため）
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM") IsNot DBNull.Value _
                        And dtbDispList.Rows(0)("CLOSE_YM") Is DBNull.Value _
                        And dtbDispList.Rows(0)("REQ_YM") Is DBNull.Value Then
                            mobjDispCntl = menmDispCntl.NoClse
                            'mstrDefYM = dtbDispList.Rows(0)("NOCLOSE_YM").ToString()
                            mstrDefYM = strTougetu
                            'データ破損状態？（この状態は起こらないはずだが念のため）
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM") IsNot DBNull.Value _
                        And dtbDispList.Rows(0)("CLOSE_YM") IsNot DBNull.Value _
                        And dtbDispList.Rows(0)("REQ_YM") IsNot DBNull.Value Then
                            '未締めが一番古い場合
                            If dtbDispList.Rows(0)("NOCLOSE_YM") <= dtbDispList.Rows(0)("CLOSE_YM") _
                            And dtbDispList.Rows(0)("NOCLOSE_YM") <= dtbDispList.Rows(0)("REQ_YM") Then
                                mobjDispCntl = menmDispCntl.NoClse
                                mstrDefYM = dtbDispList.Rows(0)("NOCLOSE_YM").ToString()
                                '請求未締めが一番古い場合
                            ElseIf dtbDispList.Rows(0)("CLOSE_YM") <= dtbDispList.Rows(0)("NOCLOSE_YM") _
                            And dtbDispList.Rows(0)("CLOSE_YM") <= dtbDispList.Rows(0)("REQ_YM") Then
                                mobjDispCntl = menmDispCntl.RClose
                                mstrDefYM = dtbDispList.Rows(0)("CLOSE_YM").ToString()
                                '前月請求締めが一番古い場合
                            ElseIf dtbDispList.Rows(0)("REQ_YM") <= dtbDispList.Rows(0)("NOCLOSE_YM") _
                            And dtbDispList.Rows(0)("REQ_YM") <= dtbDispList.Rows(0)("CLOSE_YM") Then
                                mobjDispCntl = menmDispCntl.NoClse
                                mstrDefYM = DateTime.Parse(Integer.Parse(dtbDispList.Rows(0)("REQ_YM")).ToString("####/##") & "/01").AddMonths(1).ToString("yyyyMM")
                            End If
                            '--------------------------------
                            '2014/06/16 武 ここから
                            '--------------------------------
                            'データがないとき。
                        ElseIf dtbDispList.Rows(0)("NOCLOSE_YM") Is DBNull.Value _
                        And dtbDispList.Rows(0)("CLOSE_YM") Is DBNull.Value _
                        And dtbDispList.Rows(0)("REQ_YM") Is DBNull.Value Then
                            mobjDispCntl = menmDispCntl.NoClse
                            '--------------------------------
                            '2014/06/16 武 ここまで
                            '--------------------------------
                            '想定外の状態
                        Else
                            'データなし
                            mobjDispCntl = menmDispCntl.NoCulc
                        End If
                    Else
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End If
            Else
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        Catch ex As Exception
            '締め情報の取得失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "締め情報")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　データ取得　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データ取得
    ''' </summary>
    ''' <param name="ipenmProcFlg">処理フラグ　列挙体から選択</param>
    ''' <param name="opdtbBuff">データテーブル</param>
    ''' <remarks></remarks>
    ''' <returns>True：OK, False：NG</returns>
    Private Function fGetData(ByVal ipenmProcFlg As menmProcFlg, ByRef opdtbBuff As DataTable) As Boolean

        Dim stbRetSQL As New StringBuilder
        ' ＤＢ接続変数
        Dim dstOrders As New DataSet
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        fGetData = False

        Try
            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            'リストデータ取得
            Select Case ipenmProcFlg
                Case menmProcFlg.GetData
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S1", conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, txtNendo.ppText.Substring(0, 4) & txtNendo.ppText.Substring(5, 2)))          '適用日
                    End With
                Case menmProcFlg.GetPrim
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S2", conDB)
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, txtNendo.ppText.Substring(0, 4) & txtNendo.ppText.Substring(5, 2)))          '適用日
                        .Add(pfSet_Param("Zengetu_YM", SqlDbType.NVarChar, Date.Parse(txtNendo.ppText + "/01").AddMonths(-1).ToString("yyyyMM")))          '適用日
                        .Add(pfSet_Param("Yokugetu_YM", SqlDbType.NVarChar, Date.Parse(txtNendo.ppText + "/01").AddMonths(+1).ToString("yyyyMM")))          '適用日
                    End With
            End Select

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables.Count = 1 Then
                '結果が存在したらコピーして保管
                opdtbBuff = dstOrders.Tables(0).Copy
            Else
                opdtbBuff = Nothing
            End If

            'データセット廃棄
            Call sDisposeDS(dstOrders)

            fGetData = True

        Catch ex As Exception
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Function

    ''' <summary>
    ''' データ取得
    ''' </summary>
    ''' <param name="ipenmProcFlg">処理フラグ　列挙体から選択</param>
    ''' <param name="opdtbBuff">データテーブル</param>
    ''' <remarks></remarks>
    ''' <returns>True：OK, False：NG</returns>
    Private Function fGetData(ByVal ipenmProcFlg As menmProcFlg, ByRef opdtbBuff As DataTable, _
                             ByVal conDB As SqlConnection, ByVal cmdDB As SqlCommand, ByVal conTrn As SqlTransaction) As Boolean

        Dim dstBuff As New DataSet
        Dim stbRetSQL As New StringBuilder
        ' ＤＢ接続変数
        Dim dstOrders As New DataSet
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        fGetData = False

        Try
            cmdDB.Connection = conDB
            cmdDB.Transaction = conTrn

            'リストデータ取得
            Select Case ipenmProcFlg
                Case menmProcFlg.GetChkB
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S3", conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))          '適用日
                    End With

                Case menmProcFlg.GetChkS
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S4", conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, mstrDefYM))          '適用日
                    End With

            End Select

            'データ取得およびデータをリストに設定
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables.Count = 1 Then
                '結果が存在したらコピーして保管
                opdtbBuff = dstOrders.Tables(0).Copy
            Else
                opdtbBuff = Nothing
            End If

            'データセット廃棄
            Call sDisposeDS(dstBuff)
            fGetData = True

        Catch ex As Exception
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＥＸＥＣＵＴＥＳＱＬ　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＥＸＥＣＵＴＥＳＱＬ
    ''' </summary>
    ''' <param name="ipenmProcFlg"></param>
    ''' <remarks></remarks>
    ''' <returns>True：OK, False：NG</returns>
    Private Function fPutData(ByVal ipenmProcFlg As menmProcFlg) As Boolean

        Dim stbRetSQL As New StringBuilder
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        fPutData = False

        If fCreateSQL(ipenmProcFlg, stbRetSQL) = False Then
            Exit Function
        End If

        Try

            'リストデータ
            If fDB_ExecuteSQL(mcdbDB, stbRetSQL.ToString) >= 1 Then
            Else
            End If

            fPutData = True

        Catch ex As Exception
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Function

    ''' <summary>
    ''' 入力エラーチェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub sCheck_Error()

        Dim strErr As String
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        Try
            '検収桁数チェック.
            strErr = pfCheck_TxtErr(Me.txtNendo.ppText, False, False, False, True, 7, String.Empty, False)
            If strErr <> String.Empty Then
                Me.txtNendo.psSet_ErrorNo(strErr, "検収年月", "７")
            End If

            '検収年月英字チェック.
            strErr = pfCheck_TxtErr(Me.txtNendo.ppText, False, False, True, False, 7, String.Empty, False)
            If strErr <> String.Empty Then
                Me.txtNendo.psSet_ErrorNo(strErr, "検収年月", "")
            End If

            ''検収年月全角チェック.
            'strErr = pfCheck_TxtErr(Me.txtNendo.ppText, False, False, True, False, 6, String.Empty, False)
            'If strErr <> String.Empty Then
            '    Me.txtNendo.psSet_ErrorNo(strErr, "検収年月", "")
            'End If

            '検収年月月範囲チェック.
            If txtNendo.ppText.Length = 6 Then
                If txtNendo.ppText.Substring(4, 2) < "01" Or txtNendo.ppText.Substring(4, 2) > "12" Then
                    Me.txtNendo.psSet_ErrorNo(4001, "月の範囲", "０１～１２")
                End If
            End If
        Catch ex As Exception
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Sub


    '--------------------------------------------------------------------------------------------------------
    '-　ＳＱＬ生成　　(未使用)　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＳＱＬ生成
    ''' </summary>
    ''' <param name="ipenmProcFlg"></param>
    ''' <param name="opstbSQL"></param>
    ''' <remarks></remarks>
    ''' <returns>True：OK, False：NG</returns>
    Private Function fCreateSQL(ByVal ipenmProcFlg As menmProcFlg, ByRef opstbSQL As StringBuilder) As Boolean

        fCreateSQL = False
        opstbSQL.Clear()
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        Try
            Select Case ipenmProcFlg
                Case menmProcFlg.GetList 'チェックボックスリスト（対象ドキュメント一覧）作成
                    opstbSQL.Append("SELECT [M45_CODE]                                                                            ")
                    opstbSQL.Append("      ,[M45_ITEM_NM]                                                                       ")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[M45_DISPLAY_ITEM]                                                    ")
                    opstbSQL.Append(" WHERE [M45_SCREEN_ID] = '" & M_DISP_ID & "'                                                           ")
                    opstbSQL.Append("   AND [M45_L_CLS] = '01'                                                                      ")
                    opstbSQL.Append("   AND [M45_M_CLS]='99'                                                                            ")
                    opstbSQL.Append("   AND [M45_S_CLS]='999'                                                                                   ")
                    opstbSQL.Append(" ORDER BY [M45_DISP_ORDER]                                                                                     ")
                Case menmProcFlg.GetData '画面一覧
                    opstbSQL.Append("SELECT '情報機器工事の報告書兼検収書' AS DOCTTL                                                                      ")
                    opstbSQL.Append("      ,'01' AS DOCCD                                                                                           ")
                    opstbSQL.Append("      ,[D55_CBOOK_NO]                                                                              ")
                    opstbSQL.Append("      ,MIN([D55_DEGREE]) AS DEGREE                                                                 ")
                    opstbSQL.Append("      ,REPLACE(CONVERT(varchar,CONVERT(MONEY,SUM([D55_TOTAL])),1),'.00','') AS SUBTOTAL                    ")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[D55_CNSTCHECKBOOK]                                                               ")
                    If String.IsNullOrEmpty(Me.txtNendo.ppText) = False Then
                        opstbSQL.Append(" WHERE [D55_DEGREE] LIKE '" & Me.txtNendo.ppText & "%'                                                                             ")
                    End If
                    opstbSQL.Append("       GROUP BY [D55_CBOOK_NO]                                                                       ")
                    opstbSQL.Append("       ORDER BY DEGREE DESC,DOCCD                                                                    ")
                Case menmProcFlg.GetPrim '画面コントロール初期設定用判断材料取得
                    opstbSQL.Append("SELECT                                                                                                     ")
                    opstbSQL.Append("(SELECT TOP 1                                                                                          ")
                    opstbSQL.Append("       [D89_DEGREE]                                                                                        ")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[D89_CNST_INSPECTION]                                                                 ")
                    opstbSQL.Append(" WHERE [D89_INS_CLS] = '0'                                                                             ")
                    opstbSQL.Append("   AND [D89_REQ_CLS] = '0'                                                         ")
                    opstbSQL.Append("   AND [D89_DELETE_FLG] = '0'                                              ")
                    opstbSQL.Append(" ORDER BY [D89_DEGREE] DESC) AS NOCLOSE_YM                                                                 ")
                    opstbSQL.Append(",                                                                                          ")
                    opstbSQL.Append("(SELECT TOP 1                                                                  ")
                    opstbSQL.Append("       [D89_DEGREE]                                                        ")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[D89_CNST_INSPECTION]                                                     ")
                    opstbSQL.Append(" WHERE [D89_INS_CLS] = '1'                                                     ")
                    opstbSQL.Append("   AND [D89_REQ_CLS] = '0'                                                 ")
                    opstbSQL.Append("   AND [D89_DELETE_FLG] = '0'                                                      ")
                    opstbSQL.Append(" ORDER BY [D89_DEGREE] DESC) AS CLOSE_YM                                                                   ")
                    opstbSQL.Append(",")
                    opstbSQL.Append("(SELECT TOP 1 [D89_DEGREE]                                                     ")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[D89_CNST_INSPECTION]                                                             ")
                    opstbSQL.Append(" WHERE [D89_INS_CLS] = '1'                                                         ")
                    opstbSQL.Append("   AND [D89_REQ_CLS] = '1'                                                                 ")
                    opstbSQL.Append("   AND [D89_DELETE_FLG] = '0'                                                  ")
                    opstbSQL.Append(" ORDER BY [D89_DEGREE] DESC) AS REQ_YM                                                 ")
                Case menmProcFlg.GetChkB '当月算出
                    '                    opstbSQL.Append("SELECT [D89_DEGREE]")
                    opstbSQL.Append("SELECT '" & mstrDefYM & "' AS [D89_DEGREE]                                                                                 ")
                    opstbSQL.Append("      ,[M23_DISP_SEQ]                                                                              ")
                    opstbSQL.Append("      ,[M23_TBOXCLS]                                                                           ")
                    opstbSQL.Append("      ,[M23_TBOXCLS_NM]                                                                        ")
                    opstbSQL.Append("      ,[M23_GROUP_CD]                                                                                      ")
                    opstbSQL.Append("      ,(SELECT NMLAMNT                                                                             ")
                    opstbSQL.Append("	      FROM (SELECT LEFT([D27_CLOSE_DT],4) AS NMLCLOSEYM                                                                                 ")
                    opstbSQL.Append("                     ,[D39_H_TBOXCLASS_CD] AS NMLTBOX_CD                                                                                   ")
                    opstbSQL.Append("                     ,SUM([D27_TOTAL]) AS NMLAMNT                                                                          ")
                    opstbSQL.Append("                  FROM [SPCDB].[dbo].[D39_CNSTREQSPEC]                                                                         ")
                    opstbSQL.Append("                  LEFT JOIN [dbo].[D27_CNST_AMOUNT]                                                                    ")
                    opstbSQL.Append("                    ON [D39_CNST_NO] = [D27_CNTL_NO]                                                   ")
                    opstbSQL.Append("                 WHERE ([D39_H_NEW] = '0' or [D39_H_NEW] IS NULL)                                                                      ")
                    opstbSQL.Append("                 GROUP BY LEFT([D27_CLOSE_DT],4),[D39_H_TBOXCLASS_CD]) AS NMLC                                                                     ")
                    opstbSQL.Append("         WHERE NMLCLOSEYM = '" & mstrDefYM.Remove(0, 2) & "'                                                           ")
                    opstbSQL.Append("           AND NMLTBOX_CD = [M23_TBOXCLS]) AS NMLCNST                                                                                      ")
                    opstbSQL.Append("      ,(SELECT NEWAMNT                                                                                             ")
                    opstbSQL.Append("	      FROM (SELECT LEFT([D27_CLOSE_DT],4) AS NEWCLOSEYM                                                                         ")
                    opstbSQL.Append("		              ,[D39_H_TBOXCLASS_CD] AS NEWTBOX_CD                                                   ")
                    opstbSQL.Append("                     ,SUM([D27_TOTAL]) AS NEWAMNT                                                              ")
                    opstbSQL.Append("                  FROM [SPCDB].[dbo].[D39_CNSTREQSPEC]                                                                     ")
                    opstbSQL.Append("                  LEFT JOIN [dbo].[D27_CNST_AMOUNT]                                                                                ")
                    opstbSQL.Append("                    ON [D39_CNST_NO] = [D27_CNTL_NO]                                                           ")
                    opstbSQL.Append("                 WHERE [D39_H_NEW] = '1'                                                                                       ")
                    opstbSQL.Append("                 GROUP BY LEFT([D27_CLOSE_DT],4),[D39_H_TBOXCLASS_CD]) AS NEWC                                                                             ")
                    opstbSQL.Append("         WHERE NEWCLOSEYM = '" & mstrDefYM.Remove(0, 2) & "'                                                                                   ")
                    opstbSQL.Append("           AND NEWTBOX_CD = [M23_TBOXCLS]) AS NEWCNST                                                                              ")
                    opstbSQL.Append("       ,(SELECT TOP 1 [M22_RATE]                                                                                               ")
                    opstbSQL.Append("           FROM [dbo].[M22_TAXRATE]                                                                                ")
                    opstbSQL.Append("          WHERE [M22_START_D] <= Convert(VARCHAR, GETDATE(), 112)                                                                          ")
                    opstbSQL.Append("          ORDER BY [M22_START_D] DESC) AS TAXRATE                                                                  ")
                    opstbSQL.Append("       ,(SELECT TOP 1 [M75_RATE]                                                                                       ")
                    opstbSQL.Append("           FROM [dbo].[M75_REDUCTION]                                                                                  ")
                    opstbSQL.Append("          WHERE [M75_START_DT] <= GETDATE()                                                                        ")
                    opstbSQL.Append("          ORDER BY [M75_START_DT] DESC) AS RATE                                                                    ")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[M23_TBOXCLASS]                                                                                   ")
                    opstbSQL.Append(" WHERE ([M23_DELETE_FLG] = '0' or [M23_DELETE_FLG] IS NULL)                                                                                    ")
                    opstbSQL.Append(" ORDER BY [D89_DEGREE],[M23_GROUP_CD],[M23_DISP_SEQ]                                                                       ")
                Case menmProcFlg.GetChkS '検収書作成
                    opstbSQL.Append("SELECT D89_DEGREE                                                                                                  ")
                    opstbSQL.Append("      ,SUM(NMLCNST) AS NMLCNST                                                                                         ")
                    opstbSQL.Append("      ,SUM(NEWCNST) AS NEWCNST                                                                                                 ")
                    opstbSQL.Append("      ,SUM(NMLCNST)+SUM(NEWCNST) AS TOT                                                                                    ")
                    opstbSQL.Append("      ,MAX(TAXRATE) AS TAXRATE                                                                                                 ")
                    opstbSQL.Append("      ,MAX(RATE) AS RATE                                                                               ")
                    opstbSQL.Append("  FROM (")
                    opstbSQL.Append("        SELECT [D89_DEGREE]")
                    opstbSQL.Append("              ,[M23_DISP_SEQ]")
                    opstbSQL.Append("              ,[M23_TBOXCLS]")
                    opstbSQL.Append("              ,[M23_TBOXCLS_NM]")
                    opstbSQL.Append("              ,[M23_GROUP_CD]")
                    opstbSQL.Append("              ,(SELECT NMLAMNT")
                    opstbSQL.Append("	              FROM (SELECT LEFT([D27_CLOSE_DT],4) AS NMLCLOSEYM")
                    opstbSQL.Append("                              ,[D39_H_TBOXCLASS_CD] AS NMLTBOX_CD")
                    opstbSQL.Append("                              ,SUM([D27_TOTAL]) AS NMLAMNT")
                    opstbSQL.Append("                          FROM [SPCDB].[dbo].[D39_CNSTREQSPEC]")
                    opstbSQL.Append("                          LEFT JOIN [dbo].[D27_CNST_AMOUNT]")
                    opstbSQL.Append("                            ON [D39_CNST_NO] = [D27_CNTL_NO]")
                    opstbSQL.Append("                         WHERE ([D39_H_NEW] = '0' or [D39_H_NEW] IS NULL)")
                    opstbSQL.Append("                         GROUP BY LEFT([D27_CLOSE_DT],4),[D39_H_TBOXCLASS_CD]) AS NMLC")
                    opstbSQL.Append("                 WHERE NMLCLOSEYM = '" & mstrDefYM.Remove(0, 2) & "'")
                    opstbSQL.Append("                   AND NMLTBOX_CD = [M23_TBOXCLS]) AS NMLCNST")
                    opstbSQL.Append("              ,(SELECT NEWAMNT")
                    opstbSQL.Append("	              FROM (SELECT LEFT([D27_CLOSE_DT],4) AS NEWCLOSEYM")
                    opstbSQL.Append("                              ,[D39_H_TBOXCLASS_CD] AS NEWTBOX_CD")
                    opstbSQL.Append("                              ,SUM([D27_TOTAL]) AS NEWAMNT")
                    opstbSQL.Append("                          FROM [SPCDB].[dbo].[D39_CNSTREQSPEC]")
                    opstbSQL.Append("                          LEFT JOIN [dbo].[D27_CNST_AMOUNT]")
                    opstbSQL.Append("                            ON [D39_CNST_NO] = [D27_CNTL_NO]")
                    opstbSQL.Append("                         WHERE [D39_H_NEW] = '1'")
                    opstbSQL.Append("                         GROUP BY LEFT([D27_CLOSE_DT],4),[D39_H_TBOXCLASS_CD]) AS NEWC")
                    opstbSQL.Append("                 WHERE NEWCLOSEYM = '" & mstrDefYM.Remove(0, 2) & "'")
                    opstbSQL.Append("                   AND NEWTBOX_CD = [M23_TBOXCLS]) AS NEWCNST")
                    opstbSQL.Append("              ,(SELECT TOP 1 [M22_RATE]")
                    opstbSQL.Append("                  FROM [dbo].[M22_TAXRATE]")
                    opstbSQL.Append("                 WHERE [M22_START_D]<=CONVERT(VARCHAR,GETDATE(),112)")
                    opstbSQL.Append("                 ORDER BY [M22_START_D] DESC) AS TAXRATE")
                    opstbSQL.Append("              ,(SELECT TOP 1 [M75_RATE]")
                    opstbSQL.Append("                  FROM [dbo].[M75_REDUCTION]")
                    opstbSQL.Append("                 WHERE [M75_START_DT] <= GETDATE()")
                    opstbSQL.Append("                 ORDER BY [M75_START_DT] DESC) AS RATE")
                    opstbSQL.Append("          FROM [SPCDB].[dbo].[D89_CNST_INSPECTION],[SPCDB].[dbo].[M23_TBOXCLASS]")
                    opstbSQL.Append("         WHERE ([M23_DELETE_FLG] = '0' or [M23_DELETE_FLG] IS NULL)) AS BASE")
                    opstbSQL.Append(" GROUP BY D89_DEGREE")
                Case menmProcFlg.GetPrnt '検収書印刷
                    opstbSQL.Append("SELECT [D55_CBOOK_NO]")
                    opstbSQL.Append("      ,[D55_DEGREE]")
                    opstbSQL.Append("      ,SUBSTRING([D55_DEGREE],5,2) AS DMONTH")
                    opstbSQL.Append("      ,CONVERT(DATE,LEFT(D55_DEGREE,4) + '/' + RIGHT(D55_DEGREE,2) + '/01') AS YM")
                    opstbSQL.Append("      ,[D55_SEQ]")
                    opstbSQL.Append("      ,[D55_TBOXCLS_CD]")
                    opstbSQL.Append("      ,M23_TBOXCLS_NM")
                    opstbSQL.Append("      ,M23_GROUP_CD")
                    opstbSQL.Append("      ,[D55_NML_AMOUNT]")
                    opstbSQL.Append("      ,[D55_NEW_AMOUNT]")
                    opstbSQL.Append("      ,[D55_TOTAL]")
                    opstbSQL.Append("      ,[D55_CHARGE]")
                    opstbSQL.Append("      ,(SELECT TOP 1 [M22_RATE]")
                    opstbSQL.Append("          FROM [dbo].[M22_TAXRATE]")
                    opstbSQL.Append("         WHERE [M22_START_D] <= CONVERT(VARCHAR, '" & mstrSelYM & "01', 112)")
                    opstbSQL.Append("         ORDER BY [M22_START_D] DESC) AS TAXRATE")
                    opstbSQL.Append("      ,(SELECT TOP 1 [M75_RATE]")
                    opstbSQL.Append("          FROM [dbo].[M75_REDUCTION]")
                    opstbSQL.Append("         WHERE [M75_START_DT] <= CONVERT(VARCHAR, '" & mstrSelYM & "01', 112)")
                    opstbSQL.Append("         ORDER BY [M75_START_DT] DESC) AS RATE")
                    opstbSQL.Append("      ,DEST.M44_COMP_NM AS DEST_COMP")
                    opstbSQL.Append("      ,DEST.M44_OFFICE_NM AS DEST_OFFICE")
                    opstbSQL.Append("      ,CORP.M79_ADDR AS CORP_ADDR")
                    opstbSQL.Append("	   ,CORP.M44_COMP_NM AS CORP_CONM")
                    opstbSQL.Append("	   ,CORP.M44_OFFICE_NM + '　' + CORP.M79_DESTINATION + '　宛' AS CORP_OFNM")
                    opstbSQL.Append("	   ,CORP.M79_DESTINATION AS CORP_DEST")
                    opstbSQL.Append("      ,CORP.M79_TELNO AS CORP_TEL")
                    opstbSQL.Append("	   ,CORP.M79_FAXNO AS CORP_FAX ")
                    'CNSOUTP001
                    opstbSQL.Append("      ,DEST_FT.M44_OFFICE_NM AS DEST_OFFICE_FT")
                    'CNSOUTP001
                    opstbSQL.Append("  FROM ([dbo].[D55_CNSTCHECKBOOK]")
                    opstbSQL.Append("  LEFT JOIN dbo.M23_TBOXCLASS ON D55_TBOXCLS_CD = M23_TBOXCLS)")
                    opstbSQL.Append("      ,(SELECT [M79_ZIPNO]")
                    opstbSQL.Append("              ,[M79_ADDR]")
                    opstbSQL.Append("              ,[M79_COMP_CD]")
                    opstbSQL.Append("	          ,M44_COMP_NM ")
                    opstbSQL.Append("              ,[M79_BRANCH_CD]")
                    opstbSQL.Append("	          ,M44_OFFICE_NM ")
                    opstbSQL.Append("              ,[M79_DESTINATION]")
                    opstbSQL.Append("              ,[M79_TELNO]")
                    opstbSQL.Append("              ,[M79_FAXNO]")
                    opstbSQL.Append("          FROM [dbo].[M79_DESTINATION]")
                    opstbSQL.Append("          LEFT JOIN dbo .M44_TRADER ON M79_COMP_CD = M44_COMP_CD AND M79_BRANCH_CD = M44_OFFICE_CD")
                    opstbSQL.Append("         WHERE [M79_REPORT_ID] = 'DOCREP003'")
                    opstbSQL.Append("           AND [M79_SEQNO] = 1")
                    opstbSQL.Append("           AND [M44_TRADER_CD] = '6') AS DEST")
                    opstbSQL.Append("      ,(SELECT [M79_ZIPNO]")
                    opstbSQL.Append("              ,[M79_ADDR]")
                    opstbSQL.Append("              ,[M79_COMP_CD]")
                    opstbSQL.Append("              ,M44_COMP_NM")
                    opstbSQL.Append("              ,[M79_BRANCH_CD]")
                    opstbSQL.Append("              ,M44_OFFICE_NM")
                    opstbSQL.Append("              ,[M79_DESTINATION]")
                    opstbSQL.Append("              ,[M79_TELNO]")
                    opstbSQL.Append("              ,[M79_FAXNO]")
                    opstbSQL.Append("          FROM [dbo].[M79_DESTINATION]")
                    opstbSQL.Append("          LEFT JOIN dbo .M44_TRADER ON M79_COMP_CD = M44_COMP_CD AND M79_BRANCH_CD = M44_OFFICE_CD")
                    opstbSQL.Append("         WHERE [M79_REPORT_ID] = 'DOCREP003'")
                    opstbSQL.Append("           AND [M79_SEQNO] = 2")
                    opstbSQL.Append("           AND [M44_TRADER_CD] = '8') AS CORP")
                    'CNSOUTP001
                    opstbSQL.Append("      ,(SELECT [M44_OFFICE_NM]")
                    opstbSQL.Append("          FROM [dbo].[M79_DESTINATION]")
                    opstbSQL.Append("          LEFT JOIN dbo .M44_TRADER ON M79_COMP_CD = M44_COMP_CD AND M79_BRANCH_CD = M44_OFFICE_CD")
                    opstbSQL.Append("         WHERE [M79_REPORT_ID] = 'DOCREP003'")
                    opstbSQL.Append("           AND [M79_SEQNO] = 3")
                    opstbSQL.Append("           ) AS DEST_FT")
                    'CNSOUTP001 END
                    opstbSQL.Append(" WHERE D55_DEGREE = '" & mstrSelYM & "'")
                    opstbSQL.Append(" ORDER BY M23_GROUP_CD,M23_DISP_SEQ")
                Case menmProcFlg.GetCSVF 'ＣＳＶ出力
                    opstbSQL.Append("SELECT [D55_CBOOK_NO] AS ""管理番号""")
                    opstbSQL.Append("      ,[D55_DEGREE] AS ""年月度""")
                    opstbSQL.Append("      ,DEST.M44_COMP_NM AS ""宛先　会社名""")
                    opstbSQL.Append("      ,DEST.M44_OFFICE_NM AS ""宛先　部署名""")
                    opstbSQL.Append("      ,CORP.M79_ADDR AS ""自社住所""")
                    opstbSQL.Append("      ,SUBSTRING([D55_DEGREE],5,2) AS ""月度""")
                    opstbSQL.Append("      ,[D55_SEQ] AS ""表示順""")
                    opstbSQL.Append("      ,M23_TBOXCLS_NM AS ""ＴＢＯＸシステム名""")
                    opstbSQL.Append("      ,[D55_NML_AMOUNT] AS ""通常工事額""")
                    opstbSQL.Append("      ,[D55_NEW_AMOUNT] AS ""新規工事額""")
                    opstbSQL.Append("      ,[D55_TOTAL] AS ""合計額""")
                    opstbSQL.Append("      ,(SELECT TOP 1 [M22_RATE]")
                    opstbSQL.Append("          FROM [dbo].[M22_TAXRATE]")
                    opstbSQL.Append("         WHERE [M22_START_D] <= CONVERT(VARCHAR, '" & mstrSelYM & "01', 112)")
                    opstbSQL.Append("         ORDER BY [M22_START_D] DESC) AS ""消費税額""")
                    opstbSQL.Append("      ,(SELECT TOP 1 [M75_RATE]")
                    opstbSQL.Append("          FROM [dbo].[M75_REDUCTION]")
                    opstbSQL.Append("         WHERE [M75_START_DT] <= CONVERT(VARCHAR, '" & mstrSelYM & "01', 112)")
                    opstbSQL.Append("         ORDER BY [M75_START_DT] DESC) AS ""値引率""")
                    opstbSQL.Append("	   ,CORP.M44_COMP_NM AS ""自社名""")
                    opstbSQL.Append("	   ,CORP.M44_OFFICE_NM + '　' + CORP.M79_DESTINATION AS ""自社部署　担当者""")
                    opstbSQL.Append("	   ,CORP.M79_DESTINATION AS ""自社部門名""")
                    opstbSQL.Append("      ,CORP.M79_TELNO AS ""自社部門　電話番号""")
                    opstbSQL.Append("	   ,CORP.M79_FAXNO AS ""自社部門　ＦＡＸ番号""")
                    opstbSQL.Append("  FROM ([dbo].[D55_CNSTCHECKBOOK]")
                    opstbSQL.Append("  LEFT JOIN dbo.M23_TBOXCLASS ON D55_TBOXCLS_CD = M23_TBOXCLS)")
                    opstbSQL.Append("      ,(SELECT [M79_ZIPNO]")
                    opstbSQL.Append("              ,[M79_ADDR]")
                    opstbSQL.Append("              ,[M79_COMP_CD]")
                    opstbSQL.Append("	          ,M44_COMP_NM ")
                    opstbSQL.Append("              ,[M79_BRANCH_CD]")
                    opstbSQL.Append("	          ,M44_OFFICE_NM ")
                    opstbSQL.Append("              ,[M79_DESTINATION]")
                    opstbSQL.Append("              ,[M79_TELNO]")
                    opstbSQL.Append("              ,[M79_FAXNO]")
                    opstbSQL.Append("          FROM [dbo].[M79_DESTINATION]")
                    opstbSQL.Append("          LEFT JOIN dbo .M44_TRADER ON M79_COMP_CD = M44_COMP_CD AND M79_BRANCH_CD = M44_OFFICE_CD")
                    opstbSQL.Append("         WHERE [M79_REPORT_ID] = 'DOCREP003'")
                    opstbSQL.Append("           AND [M79_SEQNO] = 1) AS DEST")
                    opstbSQL.Append("      ,(SELECT [M79_ZIPNO]")
                    opstbSQL.Append("              ,[M79_ADDR]")
                    opstbSQL.Append("              ,[M79_COMP_CD]")
                    opstbSQL.Append("              ,M44_COMP_NM")
                    opstbSQL.Append("              ,[M79_BRANCH_CD]")
                    opstbSQL.Append("              ,M44_OFFICE_NM")
                    opstbSQL.Append("              ,[M79_DESTINATION]")
                    opstbSQL.Append("              ,[M79_TELNO]")
                    opstbSQL.Append("              ,[M79_FAXNO]")
                    opstbSQL.Append("          FROM [dbo].[M79_DESTINATION]")
                    opstbSQL.Append("          LEFT JOIN dbo .M44_TRADER ON M79_COMP_CD = M44_COMP_CD AND M79_BRANCH_CD = M44_OFFICE_CD AND M44_TRADER_CD ='2'")
                    opstbSQL.Append("         WHERE [M79_REPORT_ID] = 'DOCREP003'")
                    opstbSQL.Append("           AND [M79_SEQNO] = 2) AS CORP")
                    opstbSQL.Append(" WHERE D55_DEGREE = '" & mstrSelYM & "'")
                    opstbSQL.Append(" ORDER BY M23_GROUP_CD,M23_DISP_SEQ")
                Case menmProcFlg.PutClse '締め
                    opstbSQL.Append("UPDATE [SPCDB].[dbo].[D89_CNST_INSPECTION]")
                    opstbSQL.Append("   SET [D89_INS_CLS] = '1'")
                    opstbSQL.Append("      ,[D89_UPDATE_DT] = GETDATE()")
                    opstbSQL.Append("      ,[D89_UPDATE_USR] = '" & Session(P_SESSION_USERID) & "'")
                    opstbSQL.Append(" WHERE [D89_DEGREE] = '" & mstrDefYM & "'")
                    opstbSQL.Append("   AND [D89_INS_CLS] = '0' ")
                    opstbSQL.Append("UPDATE [SPCDB].[dbo].[D39_CNSTREQSPEC]")
                    opstbSQL.Append("   SET [D39_MTR_STATUS_CD] = '12' ")
                    opstbSQL.Append("      ,[D39_UPDATE_DT] = GETDATE() ")
                    opstbSQL.Append("      ,[D39_UPDATE_USR] = '" & Session(P_SESSION_USERID) & "'")
                    opstbSQL.Append(" FROM [SPCDB].[dbo].[D39_CNSTREQSPEC] ")
                    opstbSQL.Append(" LEFT JOIN [SPCDB].[dbo].[D27_CNST_AMOUNT] ")
                    opstbSQL.Append(" ON [D27_CNST_AMOUNT].[D27_CNTL_NO] = [D39_CNSTREQSPEC].[D39_CNST_NO] ")
                    opstbSQL.Append(" WHERE LEFT([D27_CLOSE_DT],4) = '" & mstrDefYM.Remove(0, 2) & "'")
                Case menmProcFlg.PutUCls '締め解除
                    opstbSQL.Append("UPDATE [SPCDB].[dbo].[D89_CNST_INSPECTION]")
                    opstbSQL.Append("   SET [D89_INS_CLS] = '0'")
                    opstbSQL.Append("      ,[D89_UPDATE_DT] = GETDATE()")
                    opstbSQL.Append("      ,[D89_UPDATE_USR] = '" & Session(P_SESSION_USERID) & "'")
                    opstbSQL.Append(" WHERE [D89_DEGREE] = '" & mstrDefYM & "'")
                    opstbSQL.Append("   AND [D89_INS_CLS] = '1'")
                    opstbSQL.Append("UPDATE [SPCDB].[dbo].[D39_CNSTREQSPEC]")
                    opstbSQL.Append("   SET [D39_MTR_STATUS_CD] = '11' ")
                    opstbSQL.Append("      ,[D39_UPDATE_DT] = GETDATE() ")
                    opstbSQL.Append("      ,[D39_UPDATE_USR] = '" & Session(P_SESSION_USERID) & "'")
                    opstbSQL.Append(" FROM [SPCDB].[dbo].[D39_CNSTREQSPEC] ")
                    opstbSQL.Append(" LEFT JOIN [SPCDB].[dbo].[D27_CNST_AMOUNT] ")
                    opstbSQL.Append(" ON [D27_CNST_AMOUNT].[D27_CNTL_NO] = [D39_CNSTREQSPEC].[D39_CNST_NO] ")
                    opstbSQL.Append(" WHERE LEFT([D27_CLOSE_DT],4) = '" & mstrDefYM.Remove(0, 2) & "'")
                Case menmProcFlg.PutChkB1 '当月算出　追加１
                    opstbSQL.Append("INSERT INTO [SPCDB].[dbo].[D55_CNSTCHECKBOOK]")
                    opstbSQL.Append("           ([D55_CBOOK_NO]")
                    opstbSQL.Append("           ,[D55_DEGREE]")
                    opstbSQL.Append("           ,[D55_SEQ]")
                    opstbSQL.Append("           ,[D55_TBOXCLS_CD]")
                    opstbSQL.Append("           ,[D55_NML_AMOUNT]")
                    opstbSQL.Append("           ,[D55_NEW_AMOUNT]")
                    opstbSQL.Append("           ,[D55_TOTAL]")
                    opstbSQL.Append("           ,[D55_CHARGE]")
                    opstbSQL.Append("           ,[D55_INSERT_DT]")
                    opstbSQL.Append("           ,[D55_INSERT_USR])")
                    opstbSQL.Append("     VALUES")
                    opstbSQL.Append("           (@D55_CBOOK_NO")
                    opstbSQL.Append("           ,@D55_DEGREE")
                    opstbSQL.Append("           ,@D55_SEQ")
                    opstbSQL.Append("           ,@D55_TBOXCLS_CD")
                    opstbSQL.Append("           ,@D55_NML_AMOUNT")
                    opstbSQL.Append("           ,@D55_NEW_AMOUNT")
                    opstbSQL.Append("           ,@D55_TOTAL")
                    opstbSQL.Append("           ,@D55_CHARGE")
                    opstbSQL.Append("           ,@D55_INSERT_DT")
                    opstbSQL.Append("           ,@D55_INSERT_USR)")
                Case menmProcFlg.PutChkB2 '当月算出追加２
                    opstbSQL.Append("INSERT INTO [SPCDB].[dbo].[D89_CNST_INSPECTION]")
                    opstbSQL.Append("           ([D89_DEGREE]")
                    opstbSQL.Append("           ,[D89_INS_CLS]")
                    opstbSQL.Append("           ,[D89_REQ_CLS]")
                    opstbSQL.Append("           ,[D89_CNST_PRICE]")
                    opstbSQL.Append("           ,[D89_MNG_NO]")
                    opstbSQL.Append("           ,[D89_DELETE_FLG]")
                    opstbSQL.Append("           ,[D89_INSERT_DT]")
                    opstbSQL.Append("           ,[D89_INSERT_USR])")
                    opstbSQL.Append("     VALUES")
                    opstbSQL.Append("           (@D89_DEGREE")
                    opstbSQL.Append("           ,@D89_INS_CLS")
                    opstbSQL.Append("           ,@D89_REQ_CLS")
                    opstbSQL.Append("           ,@D89_CNST_PRICE")
                    opstbSQL.Append("           ,@D89_MNG_NO")
                    opstbSQL.Append("           ,@D89_DELETE_FLG")
                    opstbSQL.Append("           ,@D89_INSERT_DT")
                    opstbSQL.Append("           ,@D89_INSERT_USR)")
                Case menmProcFlg.DelChkB1 '当月算出結果　削除１
                    opstbSQL.Append("DELETE")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[D55_CNSTCHECKBOOK]")
                    opstbSQL.Append(" WHERE [D55_CBOOK_NO] = '" & mstrDefYM & "'")
                    opstbSQL.Append("   AND [D55_DEGREE] = '" & mstrDefYM & "'")
                Case menmProcFlg.DelChkB2 '当月算出結果　削除２
                    opstbSQL.Append("DELETE")
                    opstbSQL.Append("  FROM [SPCDB].[dbo].[D89_CNST_INSPECTION]")
                    opstbSQL.Append(" WHERE [D89_DEGREE] = '" & mstrDefYM & "'")
            End Select

            fCreateSQL = True

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴ作成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：保管データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　２：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　３：テーブル名　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセットの作成　コネクションが事前に作成されていることが条件になります
    ''' </summary>
    ''' <param name="cpobjDB">ＤＢ接続</param>
    ''' <param name="cpobjDS">結果を格納するデータセット　事前にインスタンスを作成しておくこと</param>
    ''' <param name="ipstrSQL">ＳＱＬ文</param>
    ''' <param name="ipstrDSName">データテーブルに名前をつける場合は指定して下さい</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fDB_CreateDataSetNoCon(ByRef cpobjDB As SqlConnection, ByRef cpobjDS As DataSet, _
                                    ByVal ipstrSQL As String, Optional ByVal ipstrDSName As String = "") _
                                                                                            As Boolean

        Dim objDBDA As SqlDataAdapter = Nothing
        Dim objDBCM As SqlCommand = Nothing
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        '戻り値　設定（失敗）
        fDB_CreateDataSetNoCon = False

        Try
            'ＤＢコネクトが壊れていなくてかつ閉じていないとき
            If cpobjDB.State <> ConnectionState.Broken And cpobjDB.State <> ConnectionState.Closed Then
                Try
                    'データセットの作成
                    objDBDA = New SqlDataAdapter(ipstrSQL, cpobjDB)
                    If cpobjDB Is Nothing Then
                    Else
                        If objDBCM Is Nothing Then
                        Else
                            objDBDA.SelectCommand.Transaction = objDBCM.Transaction
                        End If
                    End If
                    If ipstrDSName <> "" Then
                        objDBDA.Fill(cpobjDS, ipstrDSName)
                    Else
                        objDBDA.Fill(cpobjDS)
                    End If
                    '戻り値　設定（成功）
                    fDB_CreateDataSetNoCon = True
                Catch ex As Exception
                    'エラー時の処理
                    'Call mclsLog.wrtLog(ex.Message & ".データセット作成失敗：" & ipstrSQL)
                    '--------------------------------
                    '2014/04/15 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/15 星野　ここまで
                    '--------------------------------
                Finally
                    'データアダプターの廃棄
                    objDBDA.Dispose()
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        Catch ex As Exception
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＤＡＴＡＳＥＴ作成　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：保管データセットオブジェクト　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　２：ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　３：テーブル名　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセットの作成　コネクションが事前に作成されていることが条件になります
    ''' </summary>
    ''' <param name="cpobjDB">ＤＢ接続</param>
    ''' <param name="cpobjDS">結果を格納するデータセット　事前にインスタンスを作成しておくこと</param>
    ''' <param name="cpobjCMD">トランザクション付きのＳＥＬＥＣＴを実行する時に使用　事前にインスタンスを作成しておくこと</param>
    ''' <param name="ipstrSQL">ＳＱＬ文</param>
    ''' <param name="ipstrDSName">データテーブルに名前をつける場合は指定して下さい</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function fDB_CreateDataSetNoCon(ByRef cpobjDB As SqlConnection, ByRef cpobjDS As DataSet, _
                                        ByRef cpobjCMD As SqlCommand, ByVal ipstrSQL As String, _
                                        Optional ByVal ipstrDSName As String = "") As Boolean

        Dim objDBDA As SqlDataAdapter = Nothing
        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        '戻り値　設定（失敗）
        fDB_CreateDataSetNoCon = False

        Try
            'ＤＢコネクトが壊れていなくてかつ閉じていないとき
            If cpobjDB.State <> ConnectionState.Broken And cpobjDB.State <> ConnectionState.Closed Then
                Try
                    'データセットの作成
                    objDBDA = New SqlDataAdapter(ipstrSQL, cpobjDB)
                    If cpobjDB Is Nothing Then
                    Else
                        If cpobjCMD Is Nothing Then
                        Else
                            objDBDA.SelectCommand.Transaction = cpobjCMD.Transaction
                        End If
                    End If
                    If ipstrDSName <> "" Then
                        objDBDA.Fill(cpobjDS, ipstrDSName)
                    Else
                        objDBDA.Fill(cpobjDS)
                    End If
                    '戻り値　設定（成功）
                    fDB_CreateDataSetNoCon = True
                Catch ex As Exception
                    'エラー時の処理
                    'Call mclsLog.wrtLog(ex.Message & ".データセット作成失敗：" & ipstrSQL)
                    '--------------------------------
                    '2014/04/15 星野　ここから
                    '--------------------------------
                    'ログ出力
                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    '--------------------------------
                    '2014/04/15 星野　ここまで
                    '--------------------------------
                Finally
                    'データアダプターの廃棄
                    objDBDA.Dispose()
                End Try
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        Catch ex As Exception
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　ＳＱＬ文　実行　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：実行ＳＱＬ文　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' ＳＱＬ文を実行します　事前にコネクション・トランザクションの確認をしてください
    ''' </summary>
    ''' <param name="cpobjDB">ＤＢ接続</param>
    ''' <param name="ipstrSQL">実行するＳＱＬ文</param>
    ''' <returns>処理件数が帰る　失敗した場合は処理件数は-1</returns>
    ''' <remarks></remarks>
    Private Function fDB_ExecuteSQL(ByRef cpobjDB As SqlConnection, ByVal ipstrSQL As String) As Integer

        '変数定義
        Dim objDBDA As SqlDataAdapter = Nothing
        Dim objDBCM As SqlCommand = Nothing
        Dim intRetCnt As Integer '処理件数

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------

        '戻り値　設定
        fDB_ExecuteSQL = -1
        'エラートラップ　開始
        Try
            'ＳＱＬコマンド発行
            'Dim i As Integer = Integer.Parse("dddd")
            objDBCM = New SqlCommand(ipstrSQL, cpobjDB)
            objDBCM.CommandText = ipstrSQL
            'ＳＱＬコマンド　実行（処理件数　保管）
            intRetCnt = objDBCM.ExecuteNonQuery()
            fDB_ExecuteSQL = intRetCnt
        Catch ex As SqlException
            'エラー発生時の処理
            '            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
            Return -1
        Catch ex As Exception
            'エラー発生時の処理
            '            psMesBox(Me, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
            Return -1
        End Try

    End Function

    '--------------------------------------------------------------------------------------------------------
    '-　データセット　廃棄　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：処理対象データセット　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データセット　廃棄
    ''' </summary>
    ''' <param name="cpobjDST"></param>
    ''' <remarks></remarks>
    Private Sub sDisposeDS(ByRef cpobjDST As DataSet)

        Dim zz As Integer = 0

        For zz = 0 To cpobjDST.Tables.Count - 1
            cpobjDST.Tables(zz).Clear()
            cpobjDST.Tables(zz).AcceptChanges()
            cpobjDST.Tables.RemoveAt(zz)
        Next
        cpobjDST.Dispose()

    End Sub

    '--------------------------------------------------------------------------------------------------------
    '-　ＳＱＬＣＯＭＭＡＮＤのパラメータ　廃棄　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　パラメータ１：処理対象ＳＱＬＣＯＭＭＡＮＤ　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Ｓｑｌコマンドのパラメータ　廃棄
    ''' </summary>
    ''' <param name="cpobjSqlCmd"></param>
    ''' <remarks></remarks>
    Private Sub sRemoveCmdParm(ByRef cpobjSqlCmd As SqlCommand)

        Dim zz As Integer = 0

        If cpobjSqlCmd.Parameters.Count >= 0 Then
            For zz = 0 To cpobjSqlCmd.Parameters.Count - 1
                cpobjSqlCmd.Parameters.RemoveAt(0)
            Next
        End If

    End Sub

    ''' <summary>
    ''' ボタンアクションの設定.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_ButtonAction()

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click   '検索
        AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click   'クリア
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf Button_Click '当月集計
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf Button_Click '締め／解除

        Master.ppRigthButton1.Attributes("onClick") = "dispWait('search');"

        '「クリア」ボタン押下時の検証を無効
        Master.ppRigthButton2.CausesValidation = False
        'Master.Master.ppRigthButton1.CausesValidation = False
        'Master.Master.ppRigthButton2.CausesValidation = False

        '確認メッセージ設定.
        Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton1.Text).Replace(";", "") + "&&dispWait('count');" '当月集計確認
        Master.Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppRigthButton2.Text).Replace(";", "") + "&&dispWait('close');" '締め確認

    End Sub

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        Dim strMessage As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            Select Case sender.id
                Case "btnSearchRigth2"        '検索クリアボタン押下

                    strMessage = "検索条件クリア処理"

                    '入力項目初期化.
                    Me.txtNendo.ppText = String.Empty

                    mobjDispCntl = menmDispCntl.First

                Case "btnSearchRigth1"        '検索ボタン押下

                    strMessage = "検索処理"

                    If (Page.IsValid) Then
                        '検収月をViewStateに保存
                        ViewState(KENSYU_YM) = txtNendo.ppText
                        '条件検索取得
                        '入力エラーなしのとき
                        If Page.IsValid Then

                            btnSearch_Click()
                        Else
                            '検索失敗の表示
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収情報")
                            mobjDispCntl = menmDispCntl.ALLDis
                        End If
                        '入力エラーありのとき
                    Else
                        '入力エラー発生
                        mobjDispCntl = menmDispCntl.First
                    End If


                Case "btnRigth2"        '締め／解除ボタン押下

                    strMessage = "締め処理"

                    '締め処理.
                    If (Page.IsValid) Then
                        btnCloseM_Click(sender, e)
                    Else
                        '入力エラー発生
                        mobjDispCntl = menmDispCntl.First
                    End If

                Case "btnRigth1"        '当月集計ボタン押下

                    strMessage = "当月集計処理"

                    '当月集計処理
                    If (Page.IsValid) Then
                        btnCulc_Click(sender, e)
                    Else
                        '入力エラー発生
                        mobjDispCntl = menmDispCntl.First
                    End If

            End Select
            'SetFocus(Me.txtNendo.ppDateBox)

        Catch ex As Exception

            psMesBox(Me, "30015", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, strMessage)
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

    End Sub

#End Region

    '--------------------------------------------------------------------------------------------------------
    '-　データベース更新　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '-　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　-
    '--------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' データベース更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Function DelChk(ByVal ipenmProcFlg As menmProcFlg, conDB As SqlConnection, cmdDB As SqlCommand, conTrn As SqlTransaction) As Boolean

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------
        Dim intRtn As Integer

        DelChk = False

        'ログ出力開始
        psLogStart(Me)

        Try
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                Select Case ipenmProcFlg
                    '接続
                    Case menmProcFlg.DelChkB1       '結果削除1
                        cmdDB = New SqlCommand("CNSOUTP001_D1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                    Case menmProcFlg.DelChkB2       '結果削除2
                        cmdDB = New SqlCommand("CNSOUTP001_D2", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With
                End Select
                '実行
                cmdDB.ExecuteNonQuery()

                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理")

                    'ロールバック
                    conTrn.Rollback()

                    Exit Function
                End If

                DelChk = True

            End If
        Catch ex As Exception
            '処理に失敗
            'psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理", "00010")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Function

    ''' <summary>
    ''' データベース更新2
    ''' </summary>
    ''' <remarks></remarks>
    Private Function PutChk(ByVal ipenmProcFlg As menmProcFlg, conDB As SqlConnection, cmdDB As SqlCommand, conTrn As SqlTransaction) As Boolean

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------
        Dim intRtn As Integer

        PutChk = False

        'ログ出力開始
        psLogStart(Me)

        Try
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else

                Select Case ipenmProcFlg
                    Case menmProcFlg.PutChkB1
                        cmdDB = New SqlCommand("CNSOUTP001_I1", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        Dim decNmlAmnt As Integer = 0
                        Dim decNewAmnt As Integer = 0

                        'ＴＢＯＸ種別毎にレコード追加
                        For zz = 0 To dtbCulcList.Rows.Count - 1
                            With cmdDB.Parameters
                                .Add(pfSet_Param("D55_CBOOK_NO", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("D55_DEGREE", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("D55_SEQ", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("D55_TBOXCLS_CD", SqlDbType.NVarChar, mstrDefYM))
                                If dtbCulcList.Rows(zz)("NMLCNST") Is DBNull.Value Then
                                    .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                Else
                                    .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                End If
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                                .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                            End With

                            'ＳＱＬ実行
                            cmdDB.ExecuteNonQuery()

                            'ストアド戻り値チェック
                            intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                            If intRtn <> 0 Then
                                psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理")

                                'ロールバック
                                conTrn.Rollback()

                                Exit Function
                            End If

                        Next

                    Case menmProcFlg.PutChkB2
                        cmdDB = New SqlCommand("CNSOUTP001_I2", conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = conTrn
                        With cmdDB.Parameters
                            .Add(pfSet_Param("mstrDefYM", SqlDbType.NVarChar, mstrDefYM))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理")

                            'ロールバック
                            conTrn.Rollback()

                            Exit Function
                        End If


                End Select

                PutChk = True

            End If
        Catch ex As Exception
            '処理に失敗
            'ロールバック
            'psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理", "00010")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Function

    ''' <summary>
    ''' 印刷処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function pfPrint() As Boolean

        '--------------------------------
        '2014/04/15 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/15 星野　ここまで
        '--------------------------------
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing

        pfPrint = False

        'ログ出力開始
        psLogStart(Me)

        Try
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                If strClsFlg = "1" Then
                    '印刷処理
                    Dim dstOrders As New DataSet
                    Dim strRepCD As String = ""

                    strRepCD = DirectCast(grvList.Rows(0).FindControl("DOCCD"), TextBox).Text   '文書種別
                    mstrSelYM = DirectCast(grvList.Rows(0).FindControl("DEGREE"), TextBox).Text   '年月保管
                    Dim rpt As DOCREP003
                    Dim strFNm As String
                    'パラメータ設定
                    cmdDB = New SqlCommand("CNSOUTP001_S6", conDB)
                    strFNm = DirectCast(grvList.Rows(0).FindControl("DEGREE"), TextBox).Text   '"工事検収書"
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Tekiyou_YM", SqlDbType.NVarChar, strFNm))          '適用日
                    End With
                    rpt = New DOCREP003
                    'データ取得およびデータをリストに設定
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    psPrintPDF(Me, rpt, dstOrders.Tables(0), "工事検収書")

                    strClsFlg = "0"

                End If

                pfPrint = True

            End If
        Catch ex As Exception
            '処理に失敗
            'ロールバック
            'psMesBox(Me, "00010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "工事検収書　内部データ削除処理", "00010")
            '--------------------------------
            '2014/04/15 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/15 星野　ここまで
            '--------------------------------
        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Function

End Class
