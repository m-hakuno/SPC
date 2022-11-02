'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　トラブル処理票一覧
'*　ＰＧＭＩＤ：　REQLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.24　：　酒井
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'REQLSTP001-001     2016/02/17      栗原　　　検索項目のレイアウト変更
'REQLSTP001-002     2016/04/05      栗原　　　検索欄と一覧のレイアウト上下反転、csv発行機能追加
'REQLSTP001-003     2016/07/13      栗原　　　文言の修正(.aspxのみ)

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMExclusive

#End Region

Public Class REQLSTP001
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
    Const sCnsErr_00004 As String = "00004"
    Const sCnsErr_00005 As String = "00005"
    Const sCnsErr_00006 As String = "00006"
    Const sCnsErr_20004 As String = "20004"
    Const sCnsErr_30008 As String = "30008"

    'トラブル処理票画面のパス
    Const sCnsREQSELP001 As String = "~/" & P_MAI & "/" & P_FUN_REQ & P_SCR_SEL & P_PAGE & "001/" &
                                        P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"
    'トラブル処理票画面のパス
    Const M_UPD_DISP_ID = P_FUN_REQ & P_SCR_SEL & P_PAGE & "001"

    Const sCnsProgid As String = "REQLSTP001"
    Const sCnsSqlid_S1 As String = "REQLSTP001_S1"
    Const sCnsSqlid_S2 As String = "REQLSTP001_S2"
    Const sCnsSqlid_S3 As String = "REQLSTP001_S3"
    Const sCnsSqlid_S4 As String = "REQLSTP001_S4"
    Const sCnsSqlid_S5 As String = "REQLSTP001_S5"
    Const sCnsSqlid_S6 As String = "REQLSTP001_S6"
    Const sCnsSqlid_S7 As String = "REQLSTP001_S7"
    Const sCnsSqlid_S8 As String = "REQLSTP001_S8"
    Const sCnsSqlid_S9 As String = "REQLSTP001_S9"
    Const sCnsSqlid_S10 As String = "REQLSTP001_S10"
    '一覧ボタン名
    Const sCnsbtnReference As String = "btnReference"       '参照
    Const sCnsbtnUpdate As String = "btnUpdate"             '更新

    'REQLSTP001-002
    ''' <summary>
    ''' 登録ボタン表示名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_SEARCH_NM As String = "検索"
    Private Const strBTN_SEARCH_CLEAR_NM As String = "検索条件クリア"
    Private Const strBTN_CSV_NM As String = "ＣＳＶ"
    Private Const strBTN_CSV_DTL_NM As String = "トラブル処理票ＣＳＶ"

    ''' <summary>
    ''' CSVファイル名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strCSVFileName_Main As String = "トラブル処理票"
    Private Const strCSVFileName_Dtil As String = "トラブル処理票_総合"

    ''' <summary>
    ''' ViewStateのリロードフラグ管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IsReloadClick As String = "Reload_Flg_TR"
    'REQLSTP001-002 END

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

#Region "ページ初期処理"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'グリッドXML定義ファイル取得
        pfSet_GridView(grvList, sCnsProgid, 36, 11)
    End Sub

#End Region

#Region "Page_Load"

    ''' <summary>
    ''' Page_Load処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim dstTboxcls As New DataSet

        'REQLSTP001-002
        'スクロールポジションの設定
        MaintainScrollPositionOnPostBack = True

        ''ボタンアクションの設定
        'AddHandler Master.ppRightButton1.Click, AddressOf Button_Click
        'AddHandler Master.ppRightButton2.Click, AddressOf Button_Click
        'AddHandler Master.ppRightButton3.Click, AddressOf Button_Click

        AddHandler Master.Master.ppRigthButton1.Click, AddressOf Button_Click   '検索
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf Button_Click   'クリア
        AddHandler Master.Master.ppRigthButton3.Click, AddressOf Button_Click   '登録

        AddHandler Master.Master.ppLeftButton1.Click, AddressOf Button_Click    'CSV
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf Button_Click    'トラブル処理票CSV

        '「登録」「検索条件クリア」ボタン押下時の検証を無効
        Master.Master.ppRigthButton2.CausesValidation = False
        Master.Master.ppRigthButton3.CausesValidation = False
        btnReload.CausesValidation = False

        'ボタンの表示設定
        Master.Master.ppRigthButton1.Visible = True
        Master.Master.ppRigthButton2.Visible = True
        Master.Master.ppRigthButton3.Visible = True

        Master.Master.ppLeftButton1.Visible = True
        Master.Master.ppLeftButton2.Visible = True

        'ボタンの名称設定
        Master.Master.ppRigthButton1.Text = strBTN_SEARCH_NM
        Master.Master.ppRigthButton2.Text = strBTN_SEARCH_CLEAR_NM
        Master.Master.ppRigthButton3.Text = P_BTN_NM_ADD

        Master.Master.ppLeftButton1.Text = strBTN_CSV_NM
        Master.Master.ppLeftButton2.Text = strBTN_CSV_DTL_NM
        'REQLSTP001-002 END

        'REQLSTP001-001
        AddHandler Me.ddlEQDvs1.SelectedIndexChanged, AddressOf msGetEQDetail
        AddHandler Me.ddlEQDvs2.SelectedIndexChanged, AddressOf msGetEQDetail
        AddHandler Me.ddlEQDvs3.SelectedIndexChanged, AddressOf msGetEQDetail
        AddHandler Me.txtBranch_Cd.ppTextBox.TextChanged, AddressOf msGetBranchName
        ddlEQDvs1.AutoPostBack = True
        ddlEQDvs2.AutoPostBack = True
        ddlEQDvs3.AutoPostBack = True
        txtBranch_Cd.ppTextBox.AutoPostBack = True
        DirectCast(Me.txtTboxID.FindControl("lblFromTo"), Label).Font.Size = 12
        DirectCast(Me.txtTrbl_No.FindControl("lblFromTo"), Label).Font.Size = 12
        'REQLSTP001-001 END

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            If Not IsPostBack Then  '初回表示

                '「検索条件クリア」ボタン押下時の検証を無効
                Master.ppRightButton2.CausesValidation = False

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = sCnsProgid
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)

                'パンくずリスト設定
                Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

                'セッション変数「グループナンバー」が存在しない場合、画面を閉じる
                If Session(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, sCnsErr_20004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If

                'ViewStateに「グループナンバー」を保存
                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                '画面クリア
                Me.msClear_SearchArea()

                'REQLSTP001-002
                ''ボタン表示
                'Master.ppRightButton3.Text = "登録"
                'Master.ppRightButton3.Visible = True
                'Master.ppRightButton3.CausesValidation = False
                'REQLSTP001-002 END

                'ＴＢＯＸ機種・装置詳細設定
                'REQLSTP001-001
                'Me.msGet_grvData()
                Me.msSetEQDropDownList()
                Me.msSetCheckList()
                'REQLSTP001-001 END

                msSetDropDownList(ddlHPN_Cls, "ZCMPSEL002", "M29_NAME", "M29_CODE", "発生状況")        '発生状況
                msSetDropDownList(ddlPrefectureFm, "BRKLSTP001_S11", "M12_STATE_NM", "M12_STATE_CD", "都道府県 From")        '都道府県 From        
                msSetDropDownList(ddlPrefectureTo, "BRKLSTP001_S11", "M12_STATE_NM", "M12_STATE_CD", "都道府県 To")          '都道府県 To        
                msSetDropDownList(ddlSagyoJokyoFm, "ZCMPSEL015", "進捗ステータス名", "進捗ステータス", "作業状況 From") '作業状況 From
                msSetDropDownList(ddlSagyoJokyoTo, "ZCMPSEL015", "進捗ステータス名", "進捗ステータス", "作業状況 To")   '作業状況 To

                'グリッド及び件数の初期化
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"

                'データ取得
                Me.msGet_Data("1")

                ViewState(IsReloadClick) = True

                'Master.Master.ppLeftButton1.Enabled = False
                'Master.Master.ppLeftButton2.Enabled = False

            Else

                'スクロール位置調整用変数
                hdnScrollTop.Value = "1"

                'ViewState「グループナンバー」が存在しない場合、画面を閉じる
                If ViewState(P_SESSION_GROUP_NUM) Is Nothing Then
                    psMesBox(Me, sCnsErr_20004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
                End If
            End If

        Catch ex As Exception
            '画面初期化エラー
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
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

    ''' <summary>
    ''' ドロップダウンリスト設定
    ''' </summary>
    ''' <param name="ddlCtrl">ドロップダウンリスト</param>
    ''' <param name="strStoredNm">ストアド名称</param>
    ''' <param name="strDataTxtKmkNm">DB表示項目名</param>
    ''' <param name="strValKmkNm">DB値項目名</param>
    ''' <param name="strMstNm">マスタ名称(エラーメッセージ用)</param>
    ''' <remarks></remarks>
    Private Sub msSetDropDownList(ByRef ddlCtrl As DropDownList, ByVal strStoredNm As String, ByVal strDataTxtKmkNm As String, ByVal strValKmkNm As String, ByVal strMstNm As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(strStoredNm, objCn)
                If "ZCMPSEL015" = strStoredNm Then
                    objCmd.Parameters.Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "67"))
                End If
                If "ZCMPSEL002" = strStoredNm Then
                    objCmd.Parameters.Add(pfSet_Param("classcd", SqlDbType.NVarChar, "0036"))
                End If

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                ddlCtrl.Items.Clear()
                ddlCtrl.DataSource = objDs.Tables(0)
                ddlCtrl.DataTextField = strDataTxtKmkNm
                ddlCtrl.DataValueField = strValKmkNm
                ddlCtrl.DataBind()
                ddlCtrl.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                ddlCtrl.SelectedIndex = 0

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strMstNm & "マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound
        Dim strCancelCls As String = Nothing    'キャンセル区分

        For Each rowData As GridViewRow In grvList.Rows
            'グリッド内容取得
            strCancelCls = CType(rowData.FindControl("取消"), TextBox).Text

            If strCancelCls = "●" Then
                'キャンセル区分が１：取消の場合、更新ボタンを非活性にする
                rowData.Cells(1).Enabled = False
            End If

            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("受付日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("申請事象"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("回復内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("進捗状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("データ件数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("取消"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            Else
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("受付日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("申請事象"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("回復内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("進捗状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("データ件数"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("取消"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
            End If

            '文字色変更
            If CType(rowData.FindControl("進捗状況"), TextBox).Text <> "" Then
                If "01" = CType(rowData.FindControl("進捗状況"), TextBox).Text.Substring(0, 2) Then
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                    If CType(rowData.FindControl("故障区分"), TextBox).Text > "0" Then
                        CType(rowData.FindControl("申請事象"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                        'CType(rowData.FindControl("申請事象"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                    End If
                Else
                    If CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.IndexOf("引継ぎ有り") > 0 Then
                        CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                    End If
                    If CType(rowData.FindControl("故障区分"), TextBox).Text > "0" Then
                        CType(rowData.FindControl("申請事象"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                    End If
                End If
            Else
                If CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.IndexOf("引継ぎ有り") > 0 Then
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                End If
                If CType(rowData.FindControl("故障区分"), TextBox).Text > "0" Then
                    CType(rowData.FindControl("申請事象"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                End If
            End If
        Next
    End Sub

#End Region

#Region "ユーザー権限"
    '---------------------------
    '2014/04/18 武 ここから
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
                If grvList.Rows.Count > 0 Then
                    Master.Master.ppLeftButton1.Enabled = True
                    Master.Master.ppLeftButton2.Enabled = True
                Else
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                End If
                'REQLSTP001-002
            Case "SPC"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            Case "営業所"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            Case "NGC"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
                'REQLSTP001-002 END
        End Select

    End Sub
    '---------------------------
    '2014/04/18 武 ここまで
    '---------------------------
#End Region

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>リロードボタン押下は別処理</remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)
        '開始ログ出力
        psLogStart(Me)

        Select Case sender.ID
            'REQLSTP001-002
            'Case "btnSearchRigth1"        '検索ボタン押下
            Case "btnRigth1"
                'REQLSTP001-002 END
                Page.Validate()
                If (Page.IsValid) Then
                    '条件検索取得
                    msGet_Data("0")

                    ViewState(IsReloadClick) = False

                    '検索後はスクロール位置を最上部に設定
                    hdnScrollTop.Value = "0"
                End If
                If grvList.Rows.Count > 0 Then
                    Master.Master.ppLeftButton1.Enabled = True
                    Master.Master.ppLeftButton2.Enabled = True
                Else
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                End If
                'REQLSTP001-002
                'Case "btnSearchRigth2"        '検索クリアボタン押下
            Case "btnRigth2"
                'REQLSTP001-002 END

                '画面クリア
                Me.msClear_SearchArea()

                'ＴＢＯＸ機種・装置詳細再設定
                'REQLSTP001-001
                'Me.msGet_grvData()
                Me.msSetCheckList()
                'REQLSTP001-001 END

                'REQLSTP001-002
                'Case "btnSearchRigth3"
            Case "btnRigth3"
                'REQLSTP001-002

                'トラブル処理票 新規作成
                '--------------------------------
                '2014/05/14 高松　ここから
                '--------------------------------
                Session(P_SESSION_OLDDISP) = "REQLSTP001"
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
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
                                objStack.GetMethod.Name, sCnsREQSELP001, strPrm, "TRANS")
                '■□■□結合試験時のみ使用予定□■□■
                psOpen_Window(Me, sCnsREQSELP001)           'トラブル処理票 画面遷移
                '--------------------------------
                '2014/05/14 高松　ここまで
                '--------------------------------

                'REQLSTP001-002
            Case "btnLeft1"
                Page.Validate()
                If (Page.IsValid) Then
                    msGet_CSV()
                End If
            Case "btnLeft2"
                Page.Validate()
                If (Page.IsValid) Then
                    msGet_CSV_Dtl()
                End If
                'REQLSTP001-002 END
        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "一覧の更新／参照／進捗画面ボタン押下処理"

    ''' <summary>
    ''' 一覧の更新／参照／進捗画面ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As DataSet

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If e.CommandName <> sCnsbtnReference And e.CommandName <> sCnsbtnUpdate Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        Try
            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

            'セッション情報設定
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM).ToString

            Session(P_KEY) = {CType(rowData.FindControl("管理番号"), TextBox).Text.ToString.Trim}


            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'パラメータ設定
            cmdDB = New SqlCommand("REQSELP001_S1", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, CType(rowData.FindControl("管理番号"), TextBox).Text.ToString.Trim))      'トラブル管理番号
                .Add(pfSet_Param("MNT_NO", SqlDbType.NVarChar, DBNull.Value))          '保守管理番号
            End With

            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

            If dstOrders.Tables(0).Rows.Count <= 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")
                Exit Sub
            End If

            'DB切断
            If Not conDB Is Nothing Then
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If

            Select Case e.CommandName
                Case sCnsbtnReference     '参照
                    '--------------------------------
                    '2014/04/17 高松　ここから
                    '--------------------------------
                    Session(P_SESSION_OLDDISP) = "REQLSTP001"
                    '--------------------------------
                    '2014/04/17 高松　ここまで
                    '--------------------------------
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
                                    objStack.GetMethod.Name, sCnsREQSELP001, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, sCnsREQSELP001)           'トラブル処理票 画面遷移
                Case sCnsbtnUpdate        '更新
                    If dstOrders.Tables(0).Rows(0).Item("D73_CANCEL_CLS").ToString = "1" Then
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "")
                        Exit Sub
                    End If

                    arKey.Insert(0, CType(rowData.FindControl("管理番号"), TextBox).Text.ToString.Trim)        'D73_TRBL_NO

                    '★ロック対象テーブル名の登録
                    arTable_Name.Insert(0, "D73_TROUBLE")

                    '★排他情報確認処理(更新処理の実行)
                    If clsExc.pfSel_Exclusive(strExclusiveDate _
                                     , Me _
                                     , Session(P_SESSION_IP) _
                                     , Session(P_SESSION_PLACE) _
                                     , Session(P_SESSION_USERID) _
                                     , Session(P_SESSION_SESSTION_ID) _
                                     , ViewState(P_SESSION_GROUP_NUM) _
                                     , M_UPD_DISP_ID _
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
                    '--------------------------------
                    '2014/04/17 高松　ここから
                    '--------------------------------
                    Session(P_SESSION_OLDDISP) = "REQLSTP001"
                    '--------------------------------
                    '2014/04/17 高松　ここまで
                    '--------------------------------
                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新
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
                                    objStack.GetMethod.Name, sCnsREQSELP001, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, sCnsREQSELP001)           'トラブル処理票 画面遷移
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
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "検索条件クリア処理"

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_SearchArea()

        Me.txtTboxID.ppFromText = String.Empty
        Me.txtTboxID.ppToText = String.Empty
        Me.txtTrbl_No.ppFromText = String.Empty
        Me.txtTrbl_No.ppToText = String.Empty
        Me.dftRcpt_Dt.ppFromText = String.Empty
        Me.dftRcpt_Dt.ppToText = String.Empty
        Me.dftApp_Dt.ppFromText = String.Empty
        Me.dftApp_Dt.ppToText = String.Empty
        Me.txtNL_Cls.ppText = String.Empty
        Me.txtEW_Cls.ppText = String.Empty
        Me.txtBranch_Cd.ppText = String.Empty
        Me.txtTbox_Ver.ppText = String.Empty
        Me.txtARRSM.ppText = String.Empty
        Me.txtARRS.ppText = String.Empty
        Me.txtASPS.ppText = String.Empty

        Me.ddlHPN_Cls.SelectedIndex = 0
        Me.ddlPrefectureFm.SelectedIndex = 0
        Me.ddlPrefectureTo.SelectedIndex = 0
        Me.ddlSagyoJokyoFm.SelectedIndex = 0
        Me.ddlSagyoJokyoTo.SelectedIndex = 0

        Me.txtTboxID.ppTextBoxFrom.Focus()

        'REQLSTP001-001
        lblBranch.Text = String.Empty
        Me.cklTboxClass.ClearSelection()
        msSetEQDropDownList()
        Me.cklEQ.DataSource = New DataTable
        Me.cklEQ.DataBind()
        'REQLSTP001-001 END

    End Sub

#End Region

#Region "条件検索取得処理"

    ''' <summary>
    ''' 条件検索取得処理
    ''' </summary>
    ''' <param name="ipstrDefault">初期表示フラグ（1：初期　1以外：初期以外）</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrDefault As String, Optional ByVal blnIsReloadFlg As Boolean = False)
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        Dim strTboxclss_Chk As String = String.Empty
        Dim strEQ_Cls As String = String.Empty
        Dim strEQ_Chk As String = String.Empty
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid_S2, conDB)
                'パラメータ設定
                If blnIsReloadFlg = True Then
                    With cmdDB.Parameters
                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, String.Empty))
                        '管理番号
                        .Add(pfSet_Param("trbl_no_f", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("trbl_no_t", SqlDbType.NVarChar, String.Empty))
                        '受付日From
                        .Add(pfSet_Param("rcpt_dt_f", SqlDbType.NVarChar, String.Empty))
                        '受付日To
                        .Add(pfSet_Param("rcpt_dt_t", SqlDbType.NVarChar, String.Empty))
                        '承認日From
                        .Add(pfSet_Param("app_dt_f", SqlDbType.NVarChar, String.Empty))
                        '承認日To
                        .Add(pfSet_Param("app_dt_t", SqlDbType.NVarChar, String.Empty))
                        'ＮＬ区分
                        .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, String.Empty))
                        'ＥＷ区分
                        .Add(pfSet_Param("ew_cls", SqlDbType.NVarChar, String.Empty))
                        '発生状況
                        .Add(pfSet_Param("hpn_cls", SqlDbType.NVarChar, String.Empty))
                        '担当営業所
                        .Add(pfSet_Param("branch_cd", SqlDbType.NVarChar, String.Empty))
                        '都道府県From
                        .Add(pfSet_Param("state_cd_f", SqlDbType.NVarChar, String.Empty))
                        '都道府県To
                        .Add(pfSet_Param("state_cd_t", SqlDbType.NVarChar, String.Empty))
                        '作業状況From
                        .Add(pfSet_Param("status_cd_f", SqlDbType.NVarChar, String.Empty))
                        '作業状況To
                        .Add(pfSet_Param("status_cd_t", SqlDbType.NVarChar, String.Empty))
                        'ＴＢＯＸ機種
                        .Add(pfSet_Param("tboxclass", SqlDbType.NVarChar, String.Empty))
                        'ＶＥＲ
                        .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, String.Empty))
                        '装置詳細
                        .Add(pfSet_Param("eq_cls", SqlDbType.NVarChar, String.Empty))
                        .Add(pfSet_Param("eq_cd", SqlDbType.NVarChar, String.Empty))
                        '申告内容
                        .Add(pfSet_Param("rpt_cd", SqlDbType.NVarChar, String.Empty))
                        '故障修理依頼状況
                        .Add(pfSet_Param("rpt_dtl", SqlDbType.NVarChar, String.Empty))
                        '故障対応進捗状況
                        .Add(pfSet_Param("deal_dtl", SqlDbType.NVarChar, String.Empty))
                        '画面ＩＤ
                        .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, sCnsProgid))
                        '初期表示フラグ
                        .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))
                    End With
                Else
                    With cmdDB.Parameters
                        'ＴＢＯＸＩＤ
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxID.ppFromText))
                        .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxID.ppToText))
                        '管理番号
                        .Add(pfSet_Param("trbl_no_f", SqlDbType.NVarChar, Me.txtTrbl_No.ppFromText))
                        .Add(pfSet_Param("trbl_no_t", SqlDbType.NVarChar, Me.txtTrbl_No.ppToText))
                        '受付日From
                        .Add(pfSet_Param("rcpt_dt_f", SqlDbType.NVarChar, Me.dftRcpt_Dt.ppFromText))
                        '受付日To
                        .Add(pfSet_Param("rcpt_dt_t", SqlDbType.NVarChar, Me.dftRcpt_Dt.ppToText))
                        '承認日From
                        .Add(pfSet_Param("app_dt_f", SqlDbType.NVarChar, Me.dftApp_Dt.ppFromText))
                        '承認日To
                        .Add(pfSet_Param("app_dt_t", SqlDbType.NVarChar, Me.dftApp_Dt.ppToText))
                        'ＮＬ区分
                        .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, Me.txtNL_Cls.ppText))
                        'ＥＷ区分
                        .Add(pfSet_Param("ew_cls", SqlDbType.NVarChar, Me.txtEW_Cls.ppText))
                        '発生状況
                        .Add(pfSet_Param("hpn_cls", SqlDbType.NVarChar, Me.ddlHPN_Cls.SelectedValue.ToString))
                        '担当営業所
                        .Add(pfSet_Param("branch_cd", SqlDbType.NVarChar, Me.txtBranch_Cd.ppText))
                        '都道府県From
                        .Add(pfSet_Param("state_cd_f", SqlDbType.NVarChar, Me.ddlPrefectureFm.SelectedValue.ToString))
                        '都道府県To
                        .Add(pfSet_Param("state_cd_t", SqlDbType.NVarChar, Me.ddlPrefectureTo.SelectedValue.ToString))
                        '作業状況From
                        .Add(pfSet_Param("status_cd_f", SqlDbType.NVarChar, Me.ddlSagyoJokyoFm.SelectedValue.ToString))
                        '作業状況To
                        .Add(pfSet_Param("status_cd_t", SqlDbType.NVarChar, Me.ddlSagyoJokyoTo.SelectedValue.ToString))

                        'ＴＢＯＸ機種
                        'REQLSTP001-001
                        strTboxclss_Chk = mfGetSelectedCheckList(cklTboxClass)
                        .Add(pfSet_Param("tboxclass", SqlDbType.NVarChar, strTboxclss_Chk))
                        'REQLSTP001-001 END

                        'ＶＥＲ
                        .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, Me.txtTbox_Ver.ppText))

                        '装置詳細
                        'REQLSTP001-001
                        strEQ_Cls = mfGetSelectedEQList()
                        strEQ_Chk = mfGetSelectedCheckList(cklEQ)
                        .Add(pfSet_Param("eq_cls", SqlDbType.NVarChar, strEQ_Cls))
                        .Add(pfSet_Param("eq_cd", SqlDbType.NVarChar, strEQ_Chk))
                        'REQLSTP001-001 END

                        '申告内容(「あいまい検索」なのでエスケープする)
                        .Add(pfSet_Param("rpt_cd", SqlDbType.NVarChar _
                                         , Me.txtARRSM.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '故障修理依頼状況(「あいまい検索」なのでエスケープする)
                        .Add(pfSet_Param("rpt_dtl", SqlDbType.NVarChar _
                                         , Me.txtARRS.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '対応内容(「あいまい検索」なのでエスケープする)
                        .Add(pfSet_Param("deal_dtl", SqlDbType.NVarChar _
                                         , Me.txtASPS.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '画面ＩＤ
                        .Add(pfSet_Param("disp_id", SqlDbType.NVarChar, sCnsProgid))
                        '初期表示フラグ
                        .Add(pfSet_Param("default", SqlDbType.NVarChar, ipstrDefault))
                    End With
                End If


                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                'データ有無確認
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    Me.lblCount.Text = "0"
                    psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                Else
                    '閾値を超えた場合はメッセージを表示
                    If dstOrders.Tables(0).Rows(0)("データ件数") > dstOrders.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            dstOrders.Tables(0).Rows(0)("データ件数").ToString, dstOrders.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = dstOrders.Tables(0).Rows.Count
                    Master.ppCount_Visible = False
                    lblCount.Text = dstOrders.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = dstOrders.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' CSV発行処理（依頼書本体）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_CSV()
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        Dim strTboxclss_Chk As String = String.Empty
        Dim strEQ_Cls As String = String.Empty
        Dim strEQ_Chk As String = String.Empty
        objStack = New StackFrame


        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid_S9, conDB)

                Dim strReloadFlg As String
                Select Case ViewState(IsReloadClick)
                    Case True
                        strReloadFlg = "1"
                    Case Else
                        strReloadFlg = "0"
                End Select
                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxID.ppFromText))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxID.ppToText))
                    '管理番号
                    .Add(pfSet_Param("trbl_no_f", SqlDbType.NVarChar, Me.txtTrbl_No.ppFromText))
                    .Add(pfSet_Param("trbl_no_t", SqlDbType.NVarChar, Me.txtTrbl_No.ppToText))
                    '受付日From
                    .Add(pfSet_Param("rcpt_dt_f", SqlDbType.NVarChar, Me.dftRcpt_Dt.ppFromText))
                    '受付日To
                    .Add(pfSet_Param("rcpt_dt_t", SqlDbType.NVarChar, Me.dftRcpt_Dt.ppToText))
                    '承認日From
                    .Add(pfSet_Param("app_dt_f", SqlDbType.NVarChar, Me.dftApp_Dt.ppFromText))
                    '承認日To
                    .Add(pfSet_Param("app_dt_t", SqlDbType.NVarChar, Me.dftApp_Dt.ppToText))
                    'ＮＬ区分
                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, Me.txtNL_Cls.ppText))
                    'ＥＷ区分
                    .Add(pfSet_Param("ew_cls", SqlDbType.NVarChar, Me.txtEW_Cls.ppText))
                    '発生状況
                    .Add(pfSet_Param("hpn_cls", SqlDbType.NVarChar, Me.ddlHPN_Cls.SelectedValue.ToString))
                    '担当営業所
                    .Add(pfSet_Param("branch_cd", SqlDbType.NVarChar, Me.txtBranch_Cd.ppText))
                    '都道府県From
                    .Add(pfSet_Param("state_cd_f", SqlDbType.NVarChar, Me.ddlPrefectureFm.SelectedValue.ToString))
                    '都道府県To
                    .Add(pfSet_Param("state_cd_t", SqlDbType.NVarChar, Me.ddlPrefectureTo.SelectedValue.ToString))
                    '作業状況From
                    .Add(pfSet_Param("status_cd_f", SqlDbType.NVarChar, Me.ddlSagyoJokyoFm.SelectedValue.ToString))
                    '作業状況To
                    .Add(pfSet_Param("status_cd_t", SqlDbType.NVarChar, Me.ddlSagyoJokyoTo.SelectedValue.ToString))
                    'ＴＢＯＸ機種
                    strTboxclss_Chk = mfGetSelectedCheckList(cklTboxClass)
                    .Add(pfSet_Param("tboxclass", SqlDbType.NVarChar, strTboxclss_Chk))
                    'ＶＥＲ
                    .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, Me.txtTbox_Ver.ppText))
                    '装置詳細
                    strEQ_Cls = mfGetSelectedEQList()
                    strEQ_Chk = mfGetSelectedCheckList(cklEQ)
                    .Add(pfSet_Param("eq_cls", SqlDbType.NVarChar, strEQ_Cls))
                    .Add(pfSet_Param("eq_cd", SqlDbType.NVarChar, strEQ_Chk))
                    '申告内容(「あいまい検索」なのでエスケープする)
                    .Add(pfSet_Param("rpt_cd", SqlDbType.NVarChar _
                                     , Me.txtARRSM.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '故障修理依頼状況(「あいまい検索」なのでエスケープする)
                    .Add(pfSet_Param("rpt_dtl", SqlDbType.NVarChar _
                                     , Me.txtARRS.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '対応内容(「あいまい検索」なのでエスケープする)
                    .Add(pfSet_Param("deal_dtl", SqlDbType.NVarChar _
                                     , Me.txtASPS.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))

                    .Add(pfSet_Param("IsReload", SqlDbType.NVarChar, strReloadFlg)) 'リロードフラグ
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                '該当データが存在するか
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Return
                End If

                'CSVファイルダウンロード
                If pfDLCsvFile(strCSVFileName_Main + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", dstOrders.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strCSVFileName_Main)
                End If

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

    ''' <summary>
    ''' CSV発行処理（総合）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_CSV_Dtl()
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        Dim strTboxclss_Chk As String = String.Empty
        Dim strEQ_Cls As String = String.Empty
        Dim strEQ_Chk As String = String.Empty
        objStack = New StackFrame

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(sCnsSqlid_S10, conDB)

                Dim strReloadFlg As String
                Select Case ViewState(IsReloadClick)
                    Case True
                        strReloadFlg = "1"
                    Case Else
                        strReloadFlg = "0"
                End Select

                'パラメータ設定
                With cmdDB.Parameters
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxID.ppFromText))
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxID.ppToText))
                    '管理番号
                    .Add(pfSet_Param("trbl_no_f", SqlDbType.NVarChar, Me.txtTrbl_No.ppFromText))
                    .Add(pfSet_Param("trbl_no_t", SqlDbType.NVarChar, Me.txtTrbl_No.ppToText))
                    '受付日From
                    .Add(pfSet_Param("rcpt_dt_f", SqlDbType.NVarChar, Me.dftRcpt_Dt.ppFromText))
                    '受付日To
                    .Add(pfSet_Param("rcpt_dt_t", SqlDbType.NVarChar, Me.dftRcpt_Dt.ppToText))
                    '承認日From
                    .Add(pfSet_Param("app_dt_f", SqlDbType.NVarChar, Me.dftApp_Dt.ppFromText))
                    '承認日To
                    .Add(pfSet_Param("app_dt_t", SqlDbType.NVarChar, Me.dftApp_Dt.ppToText))
                    'ＮＬ区分
                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, Me.txtNL_Cls.ppText))
                    'ＥＷ区分
                    .Add(pfSet_Param("ew_cls", SqlDbType.NVarChar, Me.txtEW_Cls.ppText))
                    '発生状況
                    .Add(pfSet_Param("hpn_cls", SqlDbType.NVarChar, Me.ddlHPN_Cls.SelectedValue.ToString))
                    '担当営業所
                    .Add(pfSet_Param("branch_cd", SqlDbType.NVarChar, Me.txtBranch_Cd.ppText))
                    '都道府県From
                    .Add(pfSet_Param("state_cd_f", SqlDbType.NVarChar, Me.ddlPrefectureFm.SelectedValue.ToString))
                    '都道府県To
                    .Add(pfSet_Param("state_cd_t", SqlDbType.NVarChar, Me.ddlPrefectureTo.SelectedValue.ToString))
                    '作業状況From
                    .Add(pfSet_Param("status_cd_f", SqlDbType.NVarChar, Me.ddlSagyoJokyoFm.SelectedValue.ToString))
                    '作業状況To
                    .Add(pfSet_Param("status_cd_t", SqlDbType.NVarChar, Me.ddlSagyoJokyoTo.SelectedValue.ToString))
                    'ＴＢＯＸ機種
                    strTboxclss_Chk = mfGetSelectedCheckList(cklTboxClass)
                    .Add(pfSet_Param("tboxclass", SqlDbType.NVarChar, strTboxclss_Chk))
                    'ＶＥＲ
                    .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, Me.txtTbox_Ver.ppText))
                    '装置詳細
                    strEQ_Cls = mfGetSelectedEQList()
                    strEQ_Chk = mfGetSelectedCheckList(cklEQ)
                    .Add(pfSet_Param("eq_cls", SqlDbType.NVarChar, strEQ_Cls))
                    .Add(pfSet_Param("eq_cd", SqlDbType.NVarChar, strEQ_Chk))
                    '申告内容(「あいまい検索」なのでエスケープする)
                    .Add(pfSet_Param("rpt_cd", SqlDbType.NVarChar _
                                     , Me.txtARRSM.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '故障修理依頼状況(「あいまい検索」なのでエスケープする)
                    .Add(pfSet_Param("rpt_dtl", SqlDbType.NVarChar _
                                     , Me.txtARRS.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '対応内容(「あいまい検索」なのでエスケープする)
                    .Add(pfSet_Param("deal_dtl", SqlDbType.NVarChar _
                                     , Me.txtASPS.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))

                    .Add(pfSet_Param("IsReload", SqlDbType.NVarChar, strReloadFlg)) 'リロードフラグ
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                '該当データが存在するか
                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Return
                End If

                'CSVファイルダウンロード
                If pfDLCsvFile(strCSVFileName_Dtil + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", dstOrders.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strCSVFileName_Dtil)
                End If

            Catch ex As Exception
                'データ取得エラー
                psMesBox(Me, sCnsErr_00004, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, sCnsErr_00006, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, sCnsErr_00005, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Sub

#End Region

    'REQLSTP001-001

#Region "検索項目チェックボックス設定"

    ''' <summary>
    ''' チェックボックスリストの作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetCheckList()
        msSetcklTboxClass()
        msSetcklEQ()
    End Sub
    Private Sub msSetcklTboxClass()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim intCkl As Integer = 0
        Dim strErrMsg As String = "TBOXﾀｲﾌﾟ"
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                strErrMsg = "TBOXﾀｲﾌﾟ"
                objCmd = New SqlCommand("ZCMPSEL038", objCn)
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                Me.cklTboxClass.Items.Clear()
                Me.cklTboxClass.DataSource = objDs.Tables(0)
                Me.cklTboxClass.DataTextField = "ＴＢＯＸシステム"
                Me.cklTboxClass.DataValueField = "ＴＢＯＸシステムコード"
                Me.cklTboxClass.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strErrMsg & "一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub
    Private Sub msSetcklEQ()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing
        Dim intCkl As Integer = 0
        Dim strEQDvs As String = mfGetSelectedEQList()
        Dim strErrMsg As String = "装置詳細"
        objStack = New StackFrame

        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                strErrMsg = "装置詳細"
                objCmd = New SqlCommand(sCnsSqlid_S6, objCn)
                With objCmd.Parameters
                    .Add(pfSet_Param("EQ_Dvs", SqlDbType.NVarChar, strEQDvs))
                End With
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                Me.cklEQ.Items.Clear()
                Me.cklEQ.DataSource = objDs.Tables(0)
                Me.cklEQ.DataTextField = "名称"
                Me.cklEQ.DataValueField = "コード"
                Me.cklEQ.DataBind()
                objCmd.Dispose()
                objDs.Dispose()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strErrMsg & "一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub

#End Region
#Region "装置詳細設定"

    ''' <summary>
    ''' 装置詳細ドロップダウンリスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetEQDropDownList()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try

                '都道府県設定
                objCmd = New SqlCommand(sCnsSqlid_S7, objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlEQDvs1.Items.Clear()
                Me.ddlEQDvs1.DataSource = objDs.Tables(0)
                Me.ddlEQDvs1.DataTextField = "装置区分名"
                Me.ddlEQDvs1.DataValueField = "装置区分コード"
                Me.ddlEQDvs1.DataBind()
                Me.ddlEQDvs1.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlEQDvs2.Items.Clear()
                Me.ddlEQDvs2.DataSource = objDs.Tables(0)
                Me.ddlEQDvs2.DataTextField = "装置区分名"
                Me.ddlEQDvs2.DataValueField = "装置区分コード"
                Me.ddlEQDvs2.DataBind()
                Me.ddlEQDvs2.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

                Me.ddlEQDvs3.Items.Clear()
                Me.ddlEQDvs3.DataSource = objDs.Tables(0)
                Me.ddlEQDvs3.DataTextField = "装置区分名"
                Me.ddlEQDvs3.DataValueField = "装置区分コード"
                Me.ddlEQDvs3.DataBind()
                Me.ddlEQDvs3.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "装置区分の取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub
    ''' <summary>
    ''' 装置区分変更時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetEQDetail()
        'チェック状態のセーブ
        Dim strCheckedList() As String = mfParseCheckValueToArray()
        'チェックリスト更新
        msSetcklEQ()
        'チェック状態のロード
        msCheckedLoad(strCheckedList)
    End Sub
    ''' <summary>
    ''' チェックリストをカンマ区切りで返す
    ''' </summary>
    ''' <param name="ckl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetSelectedCheckList(ByVal ckl As CheckBoxList) As String
        Dim strTemp As String = String.Empty
        '許容バージョンを配列に挿入
        For i As Integer = 0 To ckl.Items.Count - 1
            If ckl.Items(i).Selected Then
                strTemp &= "," + ckl.Items(i).Value
            End If
        Next
        mfGetSelectedCheckList = strTemp.TrimStart(",")
    End Function
    ''' <summary>
    ''' 装置区分をカンマ区切りで返す
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetSelectedEQList() As String
        Dim strTemp(2) As String
        Dim cnt As Integer = 0          '配列インデックス管理用
        If ddlEQDvs1.SelectedValue <> String.Empty Then
            strTemp(cnt) = ddlEQDvs1.SelectedValue
            cnt += 1
        End If
        If ddlEQDvs2.SelectedValue <> String.Empty Then
            strTemp(cnt) = ddlEQDvs2.SelectedValue
            cnt += 1
        End If
        If ddlEQDvs3.SelectedValue <> String.Empty Then
            strTemp(cnt) = ddlEQDvs3.SelectedValue
        End If
        mfGetSelectedEQList = String.Join(",", strTemp).TrimStart(",").TrimEnd(",")
    End Function
    ''' <summary>
    ''' チェックリストのチェック状態を配列で返す
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfParseCheckValueToArray() As String()
        Dim strTemp(cklEQ.Items.Count - 1) As String
        Dim intArrayCnt As Integer = 0
        For i As Integer = 0 To cklEQ.Items.Count - 1
            If cklEQ.Items(i).Selected = True Then
                '選択された項目のValue値を配列に保存
                strTemp(intArrayCnt) = cklEQ.Items(i).Value.ToString & cklEQ.Items(i).Text.ToString
                intArrayCnt += 1
            End If
        Next
        mfParseCheckValueToArray = strTemp
    End Function
    ''' <summary>
    ''' チェックリストの指定項目を選択状態にする
    ''' </summary>
    ''' <param name="strCheckedArray"></param>
    ''' <remarks>配列で受け取ったValue値が一致するチェックボックスを選択状態にする。</remarks>
    Private Sub msCheckedLoad(ByVal strCheckedArray() As String)
        For i = 0 To cklEQ.Items.Count - 1
            For j As Integer = 0 To strCheckedArray.Count - 1
                '配列に保存されたデータ(ポストバック前のチェック済み項目のValue値)を比較
                If cklEQ.Items(i).Value.ToString & cklEQ.Items(i).Text.ToString = strCheckedArray(j) Then
                    cklEQ.Items(i).Selected = True
                End If
            Next
        Next
    End Sub

#End Region
#Region "保担名取得"
    ''' <summary>
    ''' 保守担当者名の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetBranchName()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(sCnsSqlid_S8, objCn)
                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, txtBranch_Cd.ppText.Trim))
                End With
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                If objDs.Tables(0).Rows.Count > 0 Then
                    '保担が存在していたら名称を返す（最大20文字まで）
                    lblBranch.Font.Size = 11
                    If objDs.Tables(0).Rows(0).Item("保担名").ToString.Length > 20 Then
                        lblBranch.Text = objDs.Tables(0).Rows(0).Item("保担名").ToString.Substring(0, 20)
                    Else
                        lblBranch.Text = objDs.Tables(0).Rows(0).Item("保担名").ToString
                    End If
                Else
                    lblBranch.Text = String.Empty
                End If
                SetFocus(ddlPrefectureFm.ClientID)
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保担営業所の取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub
#End Region
    'REQLSTP001-001 END

#Region "個別エラーチェック"

    ''' <summary>
    ''' 県Validator作動
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvPrefecture_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvPrefecture.ServerValidate

        '都道府県FROMTO
        If ddlPrefectureFm.SelectedIndex <> 0 AndAlso ddlPrefectureTo.SelectedIndex <> 0 Then
            If ddlPrefectureFm.SelectedValue > ddlPrefectureTo.SelectedValue Then
                cuvPrefecture.Text = pfGet_ValMes("2001", "都道府県").Item(P_VALMES_SMES)
                cuvPrefecture.ErrorMessage = pfGet_ValMes("2001", "都道府県").Item(P_VALMES_MES)
                cuvPrefecture.Visible = True
                args.IsValid = False

            Else
                cuvPrefecture.Text = ""
                cuvPrefecture.ErrorMessage = ""
                cuvPrefecture.Visible = False
                args.IsValid = True
            End If
        End If

    End Sub

    ''' <summary>
    ''' 作業状況Validator作動
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub cuvddlSagyoJokyo_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvddlSagyoJokyo.ServerValidate

        '作業状況FROMTO
        If ddlSagyoJokyoFm.SelectedIndex <> 0 AndAlso ddlSagyoJokyoTo.SelectedIndex <> 0 Then

            If ddlSagyoJokyoFm.SelectedValue > ddlSagyoJokyoTo.SelectedValue Then
                cuvddlSagyoJokyo.Text = pfGet_ValMes("2001", "作業状況").Item(P_VALMES_SMES)
                cuvddlSagyoJokyo.ErrorMessage = pfGet_ValMes("2001", "作業状況").Item(P_VALMES_MES)
                cuvddlSagyoJokyo.Visible = True
                args.IsValid = False
            Else
                cuvddlSagyoJokyo.Text = ""
                cuvddlSagyoJokyo.ErrorMessage = ""
                cuvddlSagyoJokyo.Visible = False
                args.IsValid = True
            End If
        End If

    End Sub

#End Region

    ''' <summary>
    ''' リロードボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        msGet_Data("1", True)
        ViewState(IsReloadClick) = True
        'Master.Master.ppLeftButton1.Enabled = False
        'Master.Master.ppLeftButton2.Enabled = False
        'リロードボタン押下時は、スクロール位置がほぼ最上部にあるはずなので
        'スクロール位置のリセットはしない。
    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
