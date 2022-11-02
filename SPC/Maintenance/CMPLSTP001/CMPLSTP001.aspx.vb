'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守対応依頼書一覧
'*　ＰＧＭＩＤ：　CMPLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.25　：　ＮＫＣ
'*  更　新　　：　2014.07.03　：　間瀬      NL区分でNを指定しているものに対してJも参照するように変更
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPLSTP001-001     2015/10/02      加賀　　　一覧のフォントカラー設定(進捗状況[04:中断]→紫)追加
'CMPLSTP001-002     2016/04/13      栗原　　　検索欄と一覧のレイアウト反転＆検索欄「TBOXタイプ」をマスタ取得に変更

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

Public Class CMPLSTP001
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
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_LST & P_PAGE & "001"     '当画面（保守対応依頼書一覧）
    Const M_MNT_DISP_ID = P_FUN_CMP & P_SCR_SEL & P_PAGE & "001"    '保守対応依頼書

    '次画面ファイルパス（保守対応依頼書）
    Const M_MNT_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "001.aspx"
    '次画面ファイルパス（保守対応依頼書照会）
    Const M_MNTS_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_INQ & P_PAGE & "001" & "/" &
            P_FUN_REQ & P_SCR_INQ & P_PAGE & "001.aspx"

    'ビューステート
    Const M_VIEW_PLACE = "place"        '接続場所
    Const M_VIEW_G_WIDTH = "gridWidth"  'グリッド全体幅

    'CMPLSTP001-002
    ''' <summary>
    ''' ViewStateのリロードフラグ管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IsReloadClick As String = "Reload_Flg_MNT"

    ''' <summary>
    ''' ボタン表示名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strBTN_SEARCH_NM As String = "検索"
    Private Const strBTN_SEARCH_CLEAR_NM As String = "検索条件クリア"
    Private Const strBTN_CSV_NM As String = "ＣＳＶ"
    Private Const strBTN_CSV_DTL_NM As String = "保守対応依頼書ＣＳＶ"

    ''' <summary>
    ''' CSVファイル名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strCSVFileName_Main As String = "保守対応依頼書"
    Private Const strCSVFileName_Dtil As String = "保守対応依頼書_総合"
    'CMPLSTP001-002 END

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
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID, 36, 11)

        'ビューステートにグリッド幅を保持
        ViewState(M_VIEW_G_WIDTH) = Me.grvList.Width.Value

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        'CMPLSTP001-002
        'AddHandler Master.ppRightButton1.Click, AddressOf btnSearch_Click
        'AddHandler Master.ppRightButton2.Click, AddressOf btnClear_Click
        'AddHandler Master.ppRightButton3.Click, AddressOf btnAdd_Click
        msSetBtn()

        AddHandler Me.txtMentBranch.ppTextBox.TextChanged, AddressOf msGetBranchName
        txtMentBranch.ppTextBox.AutoPostBack = True

        DirectCast(Me.cklTboxClass.Controls(0), CheckBox).Width = 90   'チェックボックスの幅調整

        'CMPLSTP001-002

        If Not IsPostBack Then  '初回表示のみ

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '接続場所の取得
            ViewState(M_VIEW_PLACE) = ConfigurationManager.AppSettings("Address").ToString

            '「検索条件クリア」、「登録」ボタン押下時の検証を無効
            'Master.ppRightButton2.CausesValidation = False
            'Master.ppRightButton3.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'NGC以外は「登録」ボタンを活性
            'Master.ppRightButton3.Text = P_BTN_NM_ADD
            If ViewState(M_VIEW_PLACE).ToString <> P_ADD_NGC Then
                'Master.ppRightButton3.Visible = True
                Master.Master.ppRigthButton3.Visible = True
            End If

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '画面クリア
            msClearScreen()

            msSetDropDownList(ddlPrefectureFm, "BRKLSTP001_S11", "M12_STATE_NM", "M12_STATE_CD", "都道府県 From")        '都道府県 From        
            msSetDropDownList(ddlPrefectureTo, "BRKLSTP001_S11", "M12_STATE_NM", "M12_STATE_CD", "都道府県 To")          '都道府県 To        

            ddlPrefectureFm.SelectedIndex = 0
            ddlPrefectureTo.SelectedIndex = 0

            'データ取得
            msGetData(1)

            ViewState(IsReloadClick) = True
            SetFocus(txtTboxid.ppTextBoxFrom.ClientID)
        Else
            hdnScrollTop.Value = "1"
        End If

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

    '---------------------------
    '2014/04/23 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/23 武 ここまで
    '---------------------------
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
            Case "SPC"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            Case "営業所"
                'Master.ppRightButton3.Enabled = False   '登録ボタン
                Master.Master.ppRigthButton3.Enabled = False  '登録ボタン
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
            Case "NGC"
                Master.Master.ppRigthButton3.Visible = False  '登録ボタン（PageLoadでも設定しているが念の為）
                Master.Master.ppLeftButton1.Visible = False
                Master.Master.ppLeftButton2.Visible = False
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

        'データ取得
        If (Page.IsValid) Then
            msGetData(2)
            hdnScrollTop.Value = "0"
            ViewState(IsReloadClick) = False
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

        Me.txtTboxid.ppTextBoxFrom.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 登録ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs)

        Dim strKeyList As List(Of String) = Nothing '次画面引継ぎ用キー情報

        '開始ログ出力
        psLogStart(Me)

        '次画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(String.Empty)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
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
                        objStack.GetMethod.Name, M_MNT_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '保守対応依頼書画面起動
        psOpen_Window(Me, M_MNT_DISP_PATH)

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

        If e.CommandName <> "btnReference" AndAlso e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '次画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(CType(rowData.FindControl("保守管理番号"), TextBox).Text)

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_KEY) = strKeyList.ToArray

        Select Case e.CommandName
            Case "btnReference" '参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                '★排他情報のグループ番号をセッション変数に設定
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                '接続場所がNGCの場合は保守対応依頼書照会(REQINQP001)を
                'NGC以外の場合は保守対応依頼書(CMPSELP001)を起動する
                If ViewState(M_VIEW_PLACE).ToString = P_ADD_NGC Then
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
                                    objStack.GetMethod.Name, M_MNTS_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, M_MNTS_DISP_PATH)
                Else
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
                                    objStack.GetMethod.Name, M_MNT_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, M_MNT_DISP_PATH)
                End If

            Case "btnUpdate"    '更新
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                '★排他制御用の変数
                Dim dtExclusiveDate As String = String.Empty
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D75_DEAL_MAINTAIN")
                arTable_Name.Insert(1, "D76_DEALMAINTAIN_DTIL")

                '★ロックテーブルキー項目の登録(D75_DEAL_MAINTAIN,D76_DEALMAINTAIN_DTIL)
                arKey.Insert(0, CType(rowData.FindControl("保守管理番号"), TextBox).Text) 'D75_MNT_NO
                arKey.Insert(1, CType(rowData.FindControl("保守管理番号"), TextBox).Text) 'D76_MNT_NO

                '★排他情報確認処理(更新処理の実行)
                If clsExc.pfSel_Exclusive(dtExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , M_MNT_DISP_ID _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★排他情報のグループ番号をセッション変数に設定
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

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

                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = dtExclusiveDate

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, M_MNT_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    '保守対応依頼書画面起動
                    psOpen_Window(Me, M_MNT_DISP_PATH)

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

        Dim strPlace As String = ViewState(M_VIEW_PLACE)        '場所（SPC、NGC、作業拠点）
        Dim dblGridWidth As Double = ViewState(M_VIEW_G_WIDTH)  'グリッド幅
        Dim strCancelCls As String = Nothing    'キャンセル区分
        Dim strImpCls As String = Nothing       '故障重要度区分
        Dim strNLCls As String = Nothing        'ＮＬ区分

        '接続場所がNGCの場合、「取消」列を非表示にする
        If strPlace = P_ADD_NGC Then
            grvList.Columns(12).Visible = False
            grvList.Width = dblGridWidth - (grvList.Columns(12).HeaderStyle.Width.Value + 3)
        End If

        Dim dt As DateTime

        For Each rowData As GridViewRow In grvList.Rows

            'グリッド内容取得
            strCancelCls = CType(rowData.FindControl("取消"), TextBox).Text

            If strPlace = P_ADD_NGC Then
                '---------------------------------------接続場所がNGCの場合
                '更新ボタンを非活性にする
                rowData.Cells(1).Enabled = False
            Else
                '---------------------------------------接続場所がNGC以外の場合
                If strCancelCls = "●" Then
                    'キャンセル区分が１：取消の場合、更新ボタンを非活性にする
                    rowData.Cells(1).Enabled = False
                End If
            End If

            'CMPLSTP001-001
            Select Case (CType(rowData.FindControl("作業状況"), TextBox).Text & "XX").Substring(0, 2)
                Case "04" '中断
                    For i As Integer = 2 To rowData.Cells.Count - 1
                        DirectCast(rowData.Cells(i).Controls(0), TextBox).ForeColor = Drawing.Color.BlueViolet '紫色
                    Next
            End Select
            'CMPLSTP001-001 END

            '故障重要度区分が"1"場合、申告内容を赤色にする
            strImpCls = CType(rowData.FindControl("故障重要度区分"), TextBox).Text
            If strImpCls = "1" Then
                CType(rowData.FindControl("申告内容"), TextBox).ForeColor = Drawing.Color.Red
            End If

            'ＮＬ区分が"N"以外の場合、TOXBOXIDを青色にする
            strNLCls = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text
            If strNLCls <> "N" And strNLCls <> "J" Then
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.Blue
            End If

            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("保守管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＮＬ区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("ＴＢＯＸタイプ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("申告内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("回復内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("作業状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("作業予定日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("受付日時／受付者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("更新日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("取消"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("故障重要度区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            Else
                CType(rowData.FindControl("保守管理番号"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＮＬ区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ホール名"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("ＴＢＯＸタイプ"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("申告内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("回復内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("作業状況"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("作業予定日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("受付日時／受付者"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("更新日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("取消"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("故障重要度区分"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
            End If

            If Date.TryParse(CType(rowData.FindControl("作業予定日時"), TextBox).Text, dt) Then
                If (Date.Parse(CType(rowData.FindControl("作業予定日時"), TextBox).Text) - Date.Now) <= New TimeSpan(0, 30, 0) Then

                    If CType(rowData.FindControl("開始日時"), TextBox).Text = String.Empty Then

                        CType(rowData.FindControl("保守管理番号"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("ＮＬ区分"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("ホール名"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("ＴＢＯＸタイプ"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("申告内容"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("回復内容"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("作業状況"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("作業予定日時"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("受付日時／受付者"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("更新日時"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("取消"), TextBox).ForeColor = Drawing.Color.Red
                        CType(rowData.FindControl("故障重要度区分"), TextBox).ForeColor = Drawing.Color.Red
                        'CType(rowData.FindControl("データ件数"), TextBox).ForeColor = Drawing.Color.Red
                        'CType(rowData.FindControl("開始日時"), TextBox).ForeColor = Drawing.Color.Red
                    End If
                End If
            End If

            '文字色変更
            If CType(rowData.FindControl("作業状況"), TextBox).Text <> "" Then
                If "01" = CType(rowData.FindControl("作業状況"), TextBox).Text.Substring(0, 2) Then
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
                Else
                    If CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.IndexOf("引継ぎ有り") > 0 Then
                        CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                    End If
                End If
            Else
                If CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).Text.IndexOf("引継ぎ有り") > 0 Then
                    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.FromArgb(255, 0, 128, 0)
                End If
            End If

        Next

    End Sub

    'CMPLSTP001-002
    ''' <summary>
    ''' [CSV]クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                'パラメータ設定
                msSetPrm(objCmd)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '該当データが存在するか
                If objDs.Tables(0).Rows.Count = 0 Then
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Return
                End If

                'CSVファイルダウンロード
                If pfDLCsvFile(strCSVFileName_Main + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strCSVFileName_Main)
                End If

            Catch ex As Exception
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strCSVFileName_Main)
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
    ''' [保守対応依頼書CSV]クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCSV_Dtl_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)
                'パラメータ設定
                msSetPrm(objCmd)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '該当データが存在するか
                If objDs.Tables(0).Rows.Count = 0 Then
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    Return
                End If

                'CSVファイルダウンロード
                If pfDLCsvFile(strCSVFileName_Dtil + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv", objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strCSVFileName_Dtil)
                End If

            Catch ex As Exception
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, strCSVFileName_Dtil)
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
    ''' ストアドのパラメータ設定（CSV発行処理共通）
    ''' </summary>
    ''' <param name="objCmd">パラメータを設定するSQLCommandクラス</param>
    ''' <remarks></remarks>
    Private Sub msSetPrm(ByRef objCmd As SqlCommand)
        Dim strBuff As String = String.Empty
        Dim strPlace As String = ViewState(M_VIEW_PLACE) '場所（SPC、NGC、作業拠点）
        Dim strReloadFlg As String
        Select Case ViewState(IsReloadClick)
            Case True
                strReloadFlg = "1"
            Case Else
                strReloadFlg = "0"
        End Select


        With objCmd.Parameters
            '--パラメータ設定
            'ＴＢＯＸＩＤFrom
            If Me.txtTboxid.ppToText = String.Empty OrElse
               Me.txtTboxid.ppToText = Me.txtTboxid.ppFromText Then
                'ＴＢＯＸＩＤToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar,
                                 Me.txtTboxid.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                'ＴＢＯＸＩＤToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar,
                                 Me.txtTboxid.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            Else
                'ＴＢＯＸＩＤToが空白でない場合は「範囲検索」なのでエスケープしない
                .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxid.ppFromText))
                'ＴＢＯＸＩＤTo
                .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxid.ppToText))
            End If

            '保守管理番号From
            If Me.txtMentNo.ppToText = String.Empty OrElse
               Me.txtMentNo.ppToText = Me.txtMentNo.ppFromText Then
                '保守管理番号Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                .Add(pfSet_Param("mentno_f", SqlDbType.NVarChar,
                                 Me.txtMentNo.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                '保守管理番号Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                .Add(pfSet_Param("mentno_t", SqlDbType.NVarChar,
                                 Me.txtMentNo.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            Else
                '保守管理番号Toが空白でない場合は「範囲検索」なのでエスケープしない
                .Add(pfSet_Param("mentno_f", SqlDbType.NVarChar, Me.txtMentNo.ppFromText))
                '保守管理番号To
                .Add(pfSet_Param("mentno_t", SqlDbType.NVarChar, Me.txtMentNo.ppToText))
            End If

            '受付日From
            .Add(pfSet_Param("rcptdt_f", SqlDbType.NVarChar, Me.dtbRcptDt.ppFromText))

            '受付日To
            .Add(pfSet_Param("rcptdt_t", SqlDbType.NVarChar, Me.dtbRcptDt.ppToText))

            '対応日From
            .Add(pfSet_Param("rspnsdt_f", SqlDbType.NVarChar, Me.dtbRspnsDt.ppFromText))

            '対応日To
            .Add(pfSet_Param("rspnsdt_t", SqlDbType.NVarChar, Me.dtbRspnsDt.ppToText))

            'ＮＬ区分
            .Add(pfSet_Param("nlcls", SqlDbType.NVarChar, Me.txtNLCls.ppText))

            'ＥＷ区分
            .Add(pfSet_Param("ewcls", SqlDbType.NVarChar, Me.txtEWCls.ppText))

            '保担営業所
            .Add(pfSet_Param("mentbranch", SqlDbType.NVarChar, Me.txtMentBranch.ppText))

            '都道府県From
            If Me.ddlPrefectureTo.SelectedValue = String.Empty OrElse
               Me.ddlPrefectureTo.SelectedValue = Me.ddlPrefectureFm.SelectedValue Then
                '都道府県Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                .Add(pfSet_Param("state_f", SqlDbType.NVarChar,
                                 Me.ddlPrefectureFm.SelectedValue.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                '都道府県Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                .Add(pfSet_Param("state_t", SqlDbType.NVarChar,
                                 Me.ddlPrefectureFm.SelectedValue.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            Else
                '都道府県Toが空白でない場合は「範囲検索」なのでエスケープしない
                .Add(pfSet_Param("state_f", SqlDbType.NVarChar, Me.ddlPrefectureFm.SelectedValue))
                '都道府県To
                .Add(pfSet_Param("state_t", SqlDbType.NVarChar, Me.ddlPrefectureTo.SelectedValue))

            End If

            '引継ぎ区分（営業支障）
            strBuff = String.Empty
            If Me.cbxSalesTrbl.Checked = True Then strBuff = "1"
            .Add(pfSet_Param("salestrbl", SqlDbType.NVarChar, strBuff))

            '引継ぎ区分（二次支障）
            strBuff = String.Empty
            If Me.cbxSecondTrbl.Checked = True Then strBuff = "1"
            .Add(pfSet_Param("secondtrbl", SqlDbType.NVarChar, strBuff))

            'ＴＢＯＸタイプ
            .Add(pfSet_Param("tboxtype", SqlDbType.NVarChar, mfGetTboxType()))

            '回復内容
            .Add(pfSet_Param("rprcd", SqlDbType.NVarChar,
                             Me.txtRpr.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            '回復内容詳細
            .Add(pfSet_Param("rprcntnt", SqlDbType.NVarChar,
                             Me.txtRprCntnt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            'ＶＥＲ
            .Add(pfSet_Param("version", SqlDbType.NVarChar,
                             Me.txtVersion.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            '故障機器1,2,3
            strBuff = String.Empty
            If Me.ddlTrblAppa1.ppSelectedValue <> String.Empty Then
                strBuff &= "," + Me.ddlTrblAppa1.ppSelectedValue
            End If
            If Me.ddlTrblAppa2.ppSelectedValue <> String.Empty Then
                strBuff &= "," + Me.ddlTrblAppa2.ppSelectedValue
            End If
            If Me.ddlTrblAppa3.ppSelectedValue <> String.Empty Then
                strBuff &= "," + Me.ddlTrblAppa3.ppSelectedValue
            End If
            .Add(pfSet_Param("trblappa", SqlDbType.NVarChar, strBuff.TrimStart(",")))

            '申告内容
            .Add(pfSet_Param("rptcd", SqlDbType.NVarChar,
                             Me.txtRpt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            '申告内容詳細
            .Add(pfSet_Param("rptcntnt", SqlDbType.NVarChar,
                             Me.txtRptCntnt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            '対応内容
            .Add(pfSet_Param("rspnscntnt", SqlDbType.NVarChar,
                             Me.txtRspnsCntnt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
            '接続場所区分（ 0:NGC、1:NGC以外 ）
            strBuff = String.Empty
            If strPlace = P_ADD_NGC Then
                strBuff = "0"
            Else
                strBuff = "1"
            End If
            .Add(pfSet_Param("connect_cls", SqlDbType.NVarChar, strBuff))
            'ユーザ権限
            .Add(pfSet_Param("auth_flg", SqlDbType.NVarChar, Session(P_SESSION_AUTH)))

            'リロードフラグ
            .Add(pfSet_Param("IsReload", SqlDbType.NVarChar, strReloadFlg))
        End With
    End Sub

#Region "保担コード変更"
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
                objCmd = New SqlCommand(M_MY_DISP_ID & "_S4", objCn)
                'パラメータ設定
                With objCmd.Parameters
                    .Add(pfSet_Param("MainteCd", SqlDbType.NVarChar, txtMentBranch.ppText.Trim))
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
    'CMPLSTP001-002 END

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

            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
            Me.txtTboxid.ppTextBoxFrom.Focus()

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

        Me.txtTboxid.ppFromText = String.Empty
        Me.txtTboxid.ppToText = String.Empty
        Me.txtMentNo.ppFromText = String.Empty
        Me.txtMentNo.ppToText = String.Empty
        Me.dtbRcptDt.ppFromText = String.Empty
        Me.dtbRcptDt.ppToText = String.Empty
        Me.dtbRspnsDt.ppFromText = String.Empty
        Me.dtbRspnsDt.ppToText = String.Empty
        Me.txtNLCls.ppText = String.Empty
        Me.txtEWCls.ppText = String.Empty
        Me.txtMentBranch.ppText = String.Empty
        Me.cbxSalesTrbl.Checked = False
        Me.cbxSecondTrbl.Checked = False
        'CMPLSTP001-002
        lblBranch.Text = String.Empty
        msSetcklTboxClass()
        'Me.cbxTboxType01.Checked = False
        'Me.cbxTboxType02.Checked = False
        'Me.cbxTboxType03.Checked = False
        'Me.cbxTboxType04.Checked = False
        'Me.cbxTboxType05.Checked = False
        'Me.cbxTboxType06.Checked = False
        'Me.cbxTboxType07.Checked = False
        'Me.cbxTboxType08.Checked = False
        'Me.cbxTboxType09.Checked = False
        'Me.cbxTboxType10.Checked = False
        'Me.cbxTboxType11.Checked = False
        'Me.cbxTboxType12.Checked = False
        'Me.cbxTboxType13.Checked = False
        'Me.cbxTboxType14.Checked = False
        'Me.cbxTboxType15.Checked = False
        'Me.cbxTboxType16.Checked = False
        'CMPLSTP001-002 END
        Me.txtRpr.ppText = String.Empty
        Me.txtRprCntnt.ppText = String.Empty
        Me.txtVersion.ppText = String.Empty
        Me.ddlTrblAppa1.ppDropDownList.SelectedIndex = 0
        Me.ddlTrblAppa2.ppDropDownList.SelectedIndex = 0
        Me.ddlTrblAppa3.ppDropDownList.SelectedIndex = 0
        Me.ddlPrefectureFm.SelectedIndex = 0
        Me.ddlPrefectureTo.SelectedIndex = 0
        Me.txtRpt.ppText = String.Empty
        Me.txtRptCntnt.ppText = String.Empty
        Me.txtRspnsCntnt.ppText = String.Empty

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="strSearchCls"></param>
    ''' <remarks></remarks>
    Private Sub msGetData(ByVal strSearchCls As String, Optional ByVal blnIsReloadFlg As Boolean = False)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strBuff As String = String.Empty
        Dim strPlace As String = ViewState(M_VIEW_PLACE) '場所（SPC、NGC、作業拠点）
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'グリッド及び件数の初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()
        Master.ppCount = "0"

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)

                '--パラメータ設定
                If blnIsReloadFlg = True Then
                    With objCmd.Parameters
                        'ＴＢＯＸＩＤFrom
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, String.Empty))
                        'ＴＢＯＸＩＤTo
                        .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, String.Empty))
                        '保守管理番号From
                        .Add(pfSet_Param("mentno_f", SqlDbType.NVarChar, String.Empty))
                        '保守管理番号To
                        .Add(pfSet_Param("mentno_t", SqlDbType.NVarChar, String.Empty))
                        '受付日From
                        .Add(pfSet_Param("rcptdt_f", SqlDbType.NVarChar, String.Empty))
                        '受付日To
                        .Add(pfSet_Param("rcptdt_t", SqlDbType.NVarChar, String.Empty))
                        '対応日From
                        .Add(pfSet_Param("rspnsdt_f", SqlDbType.NVarChar, String.Empty))
                        '対応日To
                        .Add(pfSet_Param("rspnsdt_t", SqlDbType.NVarChar, String.Empty))
                        'ＮＬ区分
                        .Add(pfSet_Param("nlcls", SqlDbType.NVarChar, String.Empty))
                        'ＥＷ区分
                        .Add(pfSet_Param("ewcls", SqlDbType.NVarChar, String.Empty))
                        '保担営業所
                        .Add(pfSet_Param("mentbranch", SqlDbType.NVarChar, String.Empty))
                        '都道府県From
                        .Add(pfSet_Param("state_f", SqlDbType.NVarChar, String.Empty))
                        '都道府県To
                        .Add(pfSet_Param("state_t", SqlDbType.NVarChar, String.Empty))
                        '引継ぎ区分（営業支障）
                        .Add(pfSet_Param("salestrbl", SqlDbType.NVarChar, String.Empty))
                        '引継ぎ区分（二次支障）
                        .Add(pfSet_Param("secondtrbl", SqlDbType.NVarChar, String.Empty))
                        'ＴＢＯＸタイプ
                        .Add(pfSet_Param("tboxtype", SqlDbType.NVarChar, String.Empty))
                        '回復内容
                        .Add(pfSet_Param("rprcd", SqlDbType.NVarChar, String.Empty))
                        '回復内容詳細
                        .Add(pfSet_Param("rprcntnt", SqlDbType.NVarChar, String.Empty))
                        'ＶＥＲ
                        .Add(pfSet_Param("version", SqlDbType.NVarChar, String.Empty))
                        '故障機器1,2,3
                        .Add(pfSet_Param("trblappa", SqlDbType.NVarChar, String.Empty))
                        '申告内容
                        .Add(pfSet_Param("rptcd", SqlDbType.NVarChar, String.Empty))
                        '申告内容詳細
                        .Add(pfSet_Param("rptcntnt", SqlDbType.NVarChar, String.Empty))
                        '対応内容
                        .Add(pfSet_Param("rspnscntnt", SqlDbType.NVarChar, String.Empty))
                        '接続場所区分（ 0:NGC、1:NGC以外 ）
                        strBuff = String.Empty
                        If strPlace = P_ADD_NGC Then
                            strBuff = "0"
                        Else
                            strBuff = "1"
                        End If
                        .Add(pfSet_Param("connect_cls", SqlDbType.NVarChar, strBuff))
                        '検索区分（ 1:初期表示時、2:検索時 ）
                        .Add(pfSet_Param("search_cls", SqlDbType.NVarChar, strSearchCls))
                        'ユーザ権限
                        .Add(pfSet_Param("auth_flg", SqlDbType.NVarChar, Session(P_SESSION_AUTH)))
                    End With
                Else
                    With objCmd.Parameters
                        '--パラメータ設定
                        'ＴＢＯＸＩＤFrom
                        If Me.txtTboxid.ppToText = String.Empty OrElse
                           Me.txtTboxid.ppToText = Me.txtTboxid.ppFromText Then
                            'ＴＢＯＸＩＤToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                            .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar,
                                             Me.txtTboxid.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                            'ＴＢＯＸＩＤToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                            .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar,
                                             Me.txtTboxid.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        Else
                            'ＴＢＯＸＩＤToが空白でない場合は「範囲検索」なのでエスケープしない
                            .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxid.ppFromText))
                            'ＴＢＯＸＩＤTo
                            .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxid.ppToText))
                        End If

                        '保守管理番号From
                        If Me.txtMentNo.ppToText = String.Empty OrElse
                           Me.txtMentNo.ppToText = Me.txtMentNo.ppFromText Then
                            '保守管理番号Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                            .Add(pfSet_Param("mentno_f", SqlDbType.NVarChar,
                                             Me.txtMentNo.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                            '保守管理番号Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                            .Add(pfSet_Param("mentno_t", SqlDbType.NVarChar,
                                             Me.txtMentNo.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        Else
                            '保守管理番号Toが空白でない場合は「範囲検索」なのでエスケープしない
                            .Add(pfSet_Param("mentno_f", SqlDbType.NVarChar, Me.txtMentNo.ppFromText))
                            '保守管理番号To
                            .Add(pfSet_Param("mentno_t", SqlDbType.NVarChar, Me.txtMentNo.ppToText))
                        End If

                        '受付日From
                        .Add(pfSet_Param("rcptdt_f", SqlDbType.NVarChar, Me.dtbRcptDt.ppFromText))

                        '受付日To
                        .Add(pfSet_Param("rcptdt_t", SqlDbType.NVarChar, Me.dtbRcptDt.ppToText))

                        '対応日From
                        .Add(pfSet_Param("rspnsdt_f", SqlDbType.NVarChar, Me.dtbRspnsDt.ppFromText))

                        '対応日To
                        .Add(pfSet_Param("rspnsdt_t", SqlDbType.NVarChar, Me.dtbRspnsDt.ppToText))

                        'ＮＬ区分
                        .Add(pfSet_Param("nlcls", SqlDbType.NVarChar, Me.txtNLCls.ppText))

                        'ＥＷ区分
                        .Add(pfSet_Param("ewcls", SqlDbType.NVarChar, Me.txtEWCls.ppText))

                        '保担営業所
                        .Add(pfSet_Param("mentbranch", SqlDbType.NVarChar, Me.txtMentBranch.ppText))

                        '都道府県From
                        If Me.ddlPrefectureTo.SelectedValue = String.Empty OrElse
                           Me.ddlPrefectureTo.SelectedValue = Me.ddlPrefectureFm.SelectedValue Then
                            '都道府県Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                            .Add(pfSet_Param("state_f", SqlDbType.NVarChar,
                                             Me.ddlPrefectureFm.SelectedValue.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                            '都道府県Toが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                            .Add(pfSet_Param("state_t", SqlDbType.NVarChar,
                                             Me.ddlPrefectureFm.SelectedValue.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        Else
                            '都道府県Toが空白でない場合は「範囲検索」なのでエスケープしない
                            .Add(pfSet_Param("state_f", SqlDbType.NVarChar, Me.ddlPrefectureFm.SelectedValue))
                            '都道府県To
                            .Add(pfSet_Param("state_t", SqlDbType.NVarChar, Me.ddlPrefectureTo.SelectedValue))

                        End If

                        '引継ぎ区分（営業支障）
                        strBuff = String.Empty
                        If Me.cbxSalesTrbl.Checked = True Then strBuff = "1"
                        .Add(pfSet_Param("salestrbl", SqlDbType.NVarChar, strBuff))

                        '引継ぎ区分（二次支障）
                        strBuff = String.Empty
                        If Me.cbxSecondTrbl.Checked = True Then strBuff = "1"
                        .Add(pfSet_Param("secondtrbl", SqlDbType.NVarChar, strBuff))

                        'CMPLSTP001-002
                        'ＴＢＯＸタイプ
                        'strBuff = String.Empty
                        'If Me.cbxTboxType01.Checked = True Then strBuff &= "," + Me.cbxTboxType01.Text
                        'If Me.cbxTboxType02.Checked = True Then strBuff &= "," + Me.cbxTboxType02.Text
                        'If Me.cbxTboxType03.Checked = True Then strBuff &= "," + Me.cbxTboxType03.Text
                        'If Me.cbxTboxType04.Checked = True Then strBuff &= "," + Me.cbxTboxType04.Text
                        'If Me.cbxTboxType05.Checked = True Then strBuff &= "," + Me.cbxTboxType05.Text
                        'If Me.cbxTboxType06.Checked = True Then strBuff &= "," + Me.cbxTboxType06.Text
                        'If Me.cbxTboxType07.Checked = True Then strBuff &= "," + Me.cbxTboxType07.Text
                        'If Me.cbxTboxType08.Checked = True Then strBuff &= "," + Me.cbxTboxType08.Text
                        'If Me.cbxTboxType09.Checked = True Then strBuff &= "," + Me.cbxTboxType09.Text
                        'If Me.cbxTboxType10.Checked = True Then strBuff &= "," + Me.cbxTboxType10.Text
                        'If Me.cbxTboxType11.Checked = True Then strBuff &= "," + Me.cbxTboxType11.Text
                        'If Me.cbxTboxType12.Checked = True Then strBuff &= "," + Me.cbxTboxType12.Text
                        'If Me.cbxTboxType13.Checked = True Then strBuff &= "," + Me.cbxTboxType13.Text
                        'If Me.cbxTboxType14.Checked = True Then strBuff &= "," + Me.cbxTboxType14.Text
                        'If Me.cbxTboxType15.Checked = True Then strBuff &= "," + Me.cbxTboxType15.Text
                        'If Me.cbxTboxType16.Checked = True Then strBuff &= "," + Me.cbxTboxType16.Text
                        '.Add(pfSet_Param("tboxtype", SqlDbType.NVarChar, strBuff.TrimStart(",")))
                        .Add(pfSet_Param("tboxtype", SqlDbType.NVarChar, mfGetTboxType()))
                        'CMPLSTP001-002 END

                        '回復内容
                        .Add(pfSet_Param("rprcd", SqlDbType.NVarChar,
                                         Me.txtRpr.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '回復内容詳細
                        .Add(pfSet_Param("rprcntnt", SqlDbType.NVarChar,
                                         Me.txtRprCntnt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        'ＶＥＲ
                        .Add(pfSet_Param("version", SqlDbType.NVarChar,
                                         Me.txtVersion.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '故障機器1,2,3
                        strBuff = String.Empty
                        If Me.ddlTrblAppa1.ppSelectedValue <> String.Empty Then
                            strBuff &= "," + Me.ddlTrblAppa1.ppSelectedValue
                        End If
                        If Me.ddlTrblAppa2.ppSelectedValue <> String.Empty Then
                            strBuff &= "," + Me.ddlTrblAppa2.ppSelectedValue
                        End If
                        If Me.ddlTrblAppa3.ppSelectedValue <> String.Empty Then
                            strBuff &= "," + Me.ddlTrblAppa3.ppSelectedValue
                        End If
                        .Add(pfSet_Param("trblappa", SqlDbType.NVarChar, strBuff.TrimStart(",")))

                        '申告内容
                        .Add(pfSet_Param("rptcd", SqlDbType.NVarChar,
                                         Me.txtRpt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '申告内容詳細
                        .Add(pfSet_Param("rptcntnt", SqlDbType.NVarChar,
                                         Me.txtRptCntnt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '対応内容
                        .Add(pfSet_Param("rspnscntnt", SqlDbType.NVarChar,
                                         Me.txtRspnsCntnt.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                        '接続場所区分（ 0:NGC、1:NGC以外 ）
                        strBuff = String.Empty
                        If strPlace = P_ADD_NGC Then
                            strBuff = "0"
                        Else
                            strBuff = "1"
                        End If
                        .Add(pfSet_Param("connect_cls", SqlDbType.NVarChar, strBuff))
                        '検索区分（ 1:初期表示時、2:検索時 ）
                        .Add(pfSet_Param("search_cls", SqlDbType.NVarChar, strSearchCls))
                        'ユーザ権限
                        .Add(pfSet_Param("auth_flg", SqlDbType.NVarChar, Session(P_SESSION_AUTH)))
                    End With
                End If


                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '件数を設定
                Master.ppCount_Visible = False
                If objDs.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    Me.lblCount.Text = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Else
                    '閾値を超えた場合はメッセージを表示
                    If objDs.Tables(0).Rows(0)("データ件数") > objDs.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            objDs.Tables(0).Rows(0)("データ件数").ToString, objDs.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString
                    lblCount.Text = objDs.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書一覧")
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
    ''' リロードボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        'msClearScreen()

        'データ取得
        msGetData(1, True)

        ViewState(IsReloadClick) = True
    End Sub
    'CMPLSTP001-002
    ''' <summary>
    ''' ボタン制御設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetBtn()

        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnSearch_Click   '検索
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnClear_Click   'クリア
        AddHandler Master.Master.ppRigthButton3.Click, AddressOf btnAdd_Click   '登録

        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnCSV_Click  'CSV
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf btnCSV_Dtl_Click  '保守対応依頼書CSV

        '「登録」「検索条件クリア」「リロード」ボタン押下時の検証を無効
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

    End Sub

    ''' <summary>
    ''' ■ リストで選択されたTBOXタイプの取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTboxType() As String
        objStack = New StackFrame
        Try
            '一覧のチェックがついている行について、コードを取得
            Dim strTemp As String = String.Empty
            '許容バージョンを配列に挿入
            For i As Integer = 0 To cklTboxClass.Items.Count - 1
                If cklTboxClass.Items(i).Selected Then
                    strTemp &= "," + cklTboxClass.Items(i).Value
                End If
            Next
            Return strTemp.TrimStart(",")
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' ■　チェックボックスの生成
    ''' </summary>
    ''' <remarks></remarks>
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
    'CMPLSTP001-002 END
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
