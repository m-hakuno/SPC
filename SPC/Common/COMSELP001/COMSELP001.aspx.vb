'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ホール参照
'*　ＰＧＭＩＤ：　COMSELP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.04　：　ＮＫＣ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'COMSELP001-001     2015/05/27      加賀     [検索]該当件数無しメッセージ追加             
'COMSELP001-002     2015/05/27      加賀     [検索]項目追加             
'COMSELP001-003     2016/08/10      栗原     CTI情報からの遷移対応
'COMSELP001-004     2016/11/21      加賀     電話番号マスタ(M19)同期機能追加
'COMSELP001-005     2016/12/20      栗原     CRS対応（同期処理共通化）
'COMSELP001-005     2017/01/11      栗原     同期ボタンの活性制御(管理者のみ)追加

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
'Imports SPC.Global_asax
#End Region

Public Class COMSELP001
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
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_COM & P_SCR_SEL & P_PAGE & "001"
    Const M_HALLMNT_DISP_ID = P_FUN_COM & P_SCR_MEN & P_PAGE & "006"

    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006" & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx"

    'COMSELP001-003
    Const M_CTI_DISP_ID As String = P_FUN_CTI & P_SCR_SEL & P_PAGE & "019"           'CTI画面ID
    'COMSELP001-003 END
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
    Dim clsSqlDbSvr As New ClsSQLSvrDB
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
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        'ヘッダ修正パラメータ
        Dim intHeadCol As Integer() = New Integer() {6, 10, 15, 17, 19}
        Dim intColSpan As Integer() = New Integer() {2, 2, 2, 2, 2}
        pfSet_GridView(Me.grvList, M_MY_DISP_ID, intHeadCol, intColSpan)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnSync_Click  'COMSELP001-004 

        If Not IsPostBack Then  '初回表示のみ

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '検索条件クリアボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'フッターボタン設定  'COMSELP001-004 
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton1.CausesValidation = False
            Master.Master.ppRigthButton1.Text = "同期"
            Master.Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "電話番号マスタ") '同期

            '画面クリア
            msClearScreen()

            'ドロップダウンリスト設定　COMSELP001-002
            msSetddl()

            '運用区分初期値設定 COMSELP001-002
            ddlPerCls.ppSelectedValue = "01"

            'COMSELP001-003 CTIからの遷移
            Try
                If Not Session("遷移元") Is Nothing AndAlso Session("遷移元") = M_CTI_DISP_ID Then
                    Me.txtTelNo.ppText = Session(P_KEY)(0)
                    ddlPerCls.ppSelectedValue = String.Empty
                    msGetData_CTI(Me.txtTelNo.ppText)
                End If
            Catch ex As Exception
                Me.txtTelNo.ppText = String.Empty
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "SPC"
                'COMSELP001-005
                Master.Master.ppRigthButton1.Enabled = False
                'COMSELP001-005 END
            Case "管理者"
            Case "NGC"
                'COMSELP001-005
                Master.Master.ppRigthButton1.Enabled = False
                'COMSELP001-005 END
            Case "営業所"
                'COMSELP001-005
                Master.Master.ppRigthButton1.Enabled = False
                'COMSELP001-005 END
        End Select

    End Sub

    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
                'e.Row.Cells(1).Enabled = False
            Case "営業所"
                e.Row.Cells(1).Enabled = False
            Case "NGC"
                e.Row.Cells(1).Enabled = False
        End Select

    End Sub

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        If Not Me.txtTelNo.ppText = String.Empty And _
                Not Long.TryParse(Me.txtTelNo.ppText.Replace("-", ""), New Long) Then
            Me.txtTelNo.psSet_ErrorNo("4001", Me.txtTelNo.ppName, "ハイフン(-)又は数字")
        End If

        'データ取得
        If (Page.IsValid) Then
            msGetData()
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClearSearchCondition()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッドのボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim rowData As GridViewRow = Nothing        'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing '次画面引継ぎ用キー情報

        If e.CommandName <> "btnReference" And e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '次画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text)
        strKeyList.Add(CType(rowData.FindControl("ＮＬ区分"), TextBox).Text)
        strKeyList.Add(CType(rowData.FindControl("システム分類"), TextBox).Text)
        strKeyList.Add(CType(rowData.FindControl("ホールコード"), TextBox).Text)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_KEY) = strKeyList.ToArray
        Select Case e.CommandName
            Case "btnReference" '参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

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
                                objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                '--------------------------------
                '2014/04/16 星野　ここまで
                '--------------------------------
                'ホールマスタ管理画面起動
                psOpen_Window(Me, M_NEXT_DISP_PATH)

            Case "btnUpdate"    '更新
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                '★排他制御用の変数
                Dim dtExclusiveDate As String = String.Empty
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "T04_HALL_COMM")
                arTable_Name.Insert(1, "T01_HALL")

                '★ロックテーブルキー項目の登録(T04_HALL_COMM,T01_HALL)
                arKey.Insert(0, CType(rowData.FindControl("ホールコード"), TextBox).Text)   'T01_HALL_CD

                '★排他情報確認処理(更新処理の実行)
                If clsExc.pfSel_Exclusive(dtExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , M_HALLMNT_DISP_ID _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★排他情報のグループ番号をセッション変数に設定
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = dtExclusiveDate

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
                                    objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    'ホールマスタ管理画面起動
                    psOpen_Window(Me, M_NEXT_DISP_PATH)

                Else

                    '排他ロック中

                End If

        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strDelF As String = Nothing '削除フラグ

        '削除フラグがある行の更新を非活性にする
        For Each rowData As GridViewRow In grvList.Rows
            strDelF = CType(rowData.FindControl("削除フラグ"), TextBox).Text
            If strDelF = "●" Then   '削除フラグあり
                rowData.Cells(1).Enabled = False
                CType(rowData.FindControl("運用状況"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＮＬ区分"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ホール名"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("システムコード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("システム"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＶＥＲ"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("双子店フラグ"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＭＤＮコード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＭＤＮ"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ホール所在地"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("ＴＥＬ"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("担当営業部"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("保担コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("保担"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("総括コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("総括"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("代理店コード"), TextBox).ForeColor = Drawing.Color.Red
                CType(rowData.FindControl("代理店"), TextBox).ForeColor = Drawing.Color.Red
            End If
        Next

    End Sub

    ''' <summary>
    ''' 同期ボタン押下処理 'COMSELP001-004
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnSync_Click()
        If mfSyncAllTelNoMaster() = True Then
            If mfIsTelNoIntegration() = False Then
                '不整合データ有
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "複数の電話番号を持つホールが登録されています。\n電話番号マスタを確認して下さい。")
            End If
        End If
    End Sub


#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '検索条件クリア
            msClearSearchCondition()

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            Me.tftTboxId.ppTextBoxFrom.Focus()

        Catch ex As Exception
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
        Finally
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        Me.tftTboxId.ppFromText = String.Empty
        Me.tftTboxId.ppToText = String.Empty
        Me.txtVersion.ppText = String.Empty
        Me.txtTelNo.ppText = String.Empty
        Me.txtHallNm.ppText = String.Empty
        Me.txtHallAd.ppText = String.Empty
        'COMSELP001-002
        Me.ddlSystem.ppDropDownList.SelectedIndex = -1
        Me.ddlPerCls.ppDropDownList.SelectedValue = "01"
        Me.ddlState.ppDropDownList.SelectedIndex = -1
        Me.ddlTwinCls.ppDropDownList.SelectedIndex = -1
        Me.ddlMDN.ppDropDownList.SelectedIndex = -1
        'COMSELP001-002 END

        Me.tftTboxId.ppTextBoxFrom.Focus()

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetData()

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
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤFrom
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.tftTboxId.ppFromText))

                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.tftTboxId.ppToText))

                    'システム
                    .Add(pfSet_Param("system", SqlDbType.NVarChar, Me.ddlSystem.ppSelectedValue))

                    'バージョン
                    .Add(pfSet_Param("version", SqlDbType.NVarChar,
                                     Me.txtVersion.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '運用状況
                    .Add(pfSet_Param("perat_cls", SqlDbType.NVarChar, Me.ddlPerCls.ppSelectedValue))

                    'ＴＥＬ（電話番号）
                    .Add(pfSet_Param("telno", SqlDbType.NVarChar,
                                     Me.txtTelNo.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    'ホール名
                    .Add(pfSet_Param("hallnm", SqlDbType.NVarChar,
                                     Me.txtHallNm.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    'ホール県コード
                    .Add(pfSet_Param("hallstate", SqlDbType.NVarChar, Me.ddlState.ppSelectedValue))

                    'ホール住所
                    If ddlState.ppSelectedValue = String.Empty Then
                        .Add(pfSet_Param("hallad", SqlDbType.NVarChar,
                                         Me.txtHallAd.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        .Add(pfSet_Param("hallad", SqlDbType.NVarChar,
                                         Me.txtHallAd.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]") _
                                         .Replace(ddlState.ppSelectedText.Substring(3), "")))
                    End If

                    '双子区分
                    .Add(pfSet_Param("twincls", SqlDbType.NVarChar, Me.ddlTwinCls.ppSelectedValue))

                    'MDN機器
                    .Add(pfSet_Param("mdncd", SqlDbType.NVarChar, Me.ddlMDN.ppSelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '件数を設定
                If objDs.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後) 'COMSELP001-001
                Else
                    '閾値を超えた場合はメッセージを表示
                    If objDs.Tables(0).Rows(0)("データ件数") > objDs.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            objDs.Tables(0).Rows(0)("データ件数").ToString, objDs.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
                'Dispose
                objCmd.Dispose()
                clsSqlDbSvr.psDisposeDataSet(objDs)
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定　COMSELP001-002
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddl()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                'システム設定
                objCmd = New SqlCommand("ZMSTSEL004", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'ドロップダウンリスト設定
                Me.ddlSystem.ppDropDownList.Items.Clear()
                Me.ddlSystem.ppDropDownList.DataSource = objDs.Tables(1)    'Tables(0)で削除データ含む
                Me.ddlSystem.ppDropDownList.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.ppDropDownList.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.ppDropDownList.DataBind()
                Me.ddlSystem.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                '都道府県設定
                objCmd = New SqlCommand("ZMSTSEL002", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'ドロップダウンリスト設定
                Me.ddlState.ppDropDownList.Items.Clear()
                Me.ddlState.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlState.ppDropDownList.DataTextField = "項目名"
                Me.ddlState.ppDropDownList.DataValueField = "都道府県コード"
                Me.ddlState.ppDropDownList.DataBind()
                Me.ddlState.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                'MDN設定
                objCmd = New SqlCommand("ZCMPSEL055", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'ドロップダウンリスト設定
                Me.ddlMDN.ppDropDownList.Items.Clear()
                Me.ddlMDN.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlMDN.ppDropDownList.DataTextField = "MDN"
                Me.ddlMDN.ppDropDownList.DataValueField = "MDNコード"
                Me.ddlMDN.ppDropDownList.DataBind()
                Me.ddlMDN.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ドロップダウン一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
                'Dispose
                objCmd.Dispose()
                clsSqlDbSvr.psDisposeDataSet(objDs)
            End Try
        End If
    End Sub


    'COMSELP001-003
    ''' <summary>
    ''' データ取得処理(CTIからの遷移時専用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetData_CTI(ByVal _telno As String)

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
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＥＬ（電話番号）
                    .Add(pfSet_Param("telno", SqlDbType.NVarChar, _telno))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '件数を設定
                If objDs.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Else
                    Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
                'Dispose
                objCmd.Dispose()
                clsSqlDbSvr.psDisposeDataSet(objDs)
            End Try
        End If

    End Sub
    'COMSELP001-003 END

    'COMSELP001-005
    Private Function mfSyncAllTelNoMaster() As Boolean
        Const strStored As String = "ZCMPINS003"
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim trans As SqlClient.SqlTransaction
        objStack = New StackFrame

        mfSyncAllTelNoMaster = False

        'トランザクションの設定
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                With cmdDB.Parameters
                    .Clear()
                    .Add(pfSet_Param("tblid", SqlDbType.NVarChar, "T01_HALL"))
                    .Add(pfSet_Param("usrid", SqlDbType.NVarChar, User.Identity.Name))
                End With
                Try
                    cmdDB.ExecuteNonQuery()
                Catch ex As Exception
                    trans.Rollback()
                    Throw ex
                End Try
                trans.Commit()
                mfSyncAllTelNoMaster = True
                '正常終了メッセージ
                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "電話番号の同期")
            Catch ex As Exception
                mfSyncAllTelNoMaster = False
                'エラーメッセージ表示
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "電話番号の同期に失敗しました。\nシステム管理者に問い合わせてください。")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                  objStack.GetMethod.Name, "", ex.ToString, "Catch")
            End Try
        End If
    End Function
    Private Function mfIsTelNoIntegration() As Boolean
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        mfIsTelNoIntegration = False
        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try

                cmdDB = New SqlCommand("ZMSTSEL007", conDB)
                cmdDB.Parameters.Add(pfSet_Param("dvs", SqlDbType.NVarChar, "0"))

                'リストデータ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    mfIsTelNoIntegration = True
                Else
                    mfIsTelNoIntegration = False
                End If

            Catch ex As Exception
                mfIsTelNoIntegration = False
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function
    'COMSELP001-005 END

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
