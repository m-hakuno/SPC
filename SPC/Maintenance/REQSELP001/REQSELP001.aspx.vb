'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　トラブル処理票
'*　ＰＧＭＩＤ：　REQSELP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.08　：　酒井
'*  更　新　　：　2014.06.17　：　間瀬　レイアウト変更
'*  更　新　　：　2014.07.03　：　間瀬      NL区分でNを指定しているものに対してJも参照するように変更
'*  更　新　　：　2015.02.10　：　加賀  レイアウト変更＆発生区分[2:発見]選択時に申告日項目をクリア
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'REQSELP001-001     2015/08/19      加賀      ホールマスタ管理からの画面遷移に対応              
'REQSELP001-002     2015/10/23      加賀      画面遷移[ホールマスタ管理][対応履歴照会]追加              
'REQSELP001-003     2016/02/15      栗原      代行店の表示/非表示をマスタ取得に変更
'REQSELP001-004     2017/01/27      栗原      データ更新時の[確認日]が[承認日]と同じ値に上書きされるバグの修正

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System
Imports System.Globalization
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataLink
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

#End Region

Public Class REQSELP001
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
    Const sCnsErr_0001 As String = "0001"
    Const sCnsErr_0002 As String = "0002"
    Const sCnsErr_0003 As String = "0003"
    Const sCnsErr_0004 As String = "0004"
    Const sCnsErr_0005 As String = "0005"
    Const sCnsErr_30008 As String = "30008"

    'トラブル処理票一覧 画面のパス
    Const sCnsREQLSTP001 As String = "~/" & P_MAI & "/" & P_FUN_REQ & P_SCR_LST & P_PAGE & "001/" &
                                        P_FUN_REQ & P_SCR_LST & P_PAGE & "001.aspx"

    'トラブル処理票画面のパス
    Const sCnsREQSELP001 As String = "~/" & P_MAI & "/" & P_FUN_REQ & P_SCR_SEL & P_PAGE & "001/" &
                                       P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"

    '保守対応依頼書画面のパス
    Const sCnsCMPSELP001 As String = "~/" & P_MAI & "/" & P_FUN_CMP & P_SCR_SEL & P_PAGE & "001/" &
                                       P_FUN_CMP & P_SCR_SEL & P_PAGE & "001.aspx"

    'ホールマスタ管理画面のパス
    Const M_HMST_DISP_PATH = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006" & "/" &
            P_FUN_COM & P_SCR_MEN & P_PAGE & "006.aspx"

    '対応履歴照会画面のパス
    Const M_CHST_DISP_PATH As String = "~/" & P_FLR & "/" &
            P_FUN_BRK & P_SCR_INQ & P_PAGE & "001" & "/" &
            P_FUN_BRK & P_SCR_INQ & P_PAGE & "001.aspx"

    '一覧ボタン名
    Const sCnsbtnSntak As String = "btnSntaku"       '選択

    Const sCnsProgid As String = "REQSELP001"
    Const sCnsSqlid_S1 As String = "REQSELP001_S1"
    Const sCnsSqlid_S2 As String = "REQSELP001_S2"
    Const sCnsSqlid_S3 As String = "REQSELP001_S3"
    Const sCnsSqlid_S4 As String = "CNSUPDP001_S4"
    Const sCnsSqlid_S5 As String = "REQSELP001_S5"
    Const sCnsSqlid_S6 As String = "REQSELP001_S6"
    Const sCnsSqlid_S98 As String = "CNSUPDP001_S6"
    Const sCnsSqlid_S99 As String = "CNSUPDP001_S4"
    Const sCnsSqlid_Z9 As String = "ZCMPSEL009"
    Const sCnsSqlid_I1 As String = "REQSELP001_I1"
    Const sCnsSqlid_I2 As String = "REQSELP001_I2"
    Const sCnsSqlid_I3 As String = "REQSELP001_I3"
    Const sCnsSqlid_I4 As String = "REQSELP001_I4"
    Const sCnsSqlid_U1 As String = "REQSELP001_U1"
    Const sCnsSqlid_U2 As String = "REQSELP001_U2"
    Const sCnsSqlid_U3 As String = "REQSELP001_U3"
    Const sCnsSqlid_U4 As String = "REQSELP001_U4"
    Const sCnsSqlid_U5 As String = "REQSELP001_U5"
    Const sCnsSqlid_U6 As String = "REQSELP001_U6"
    Const sCnsSqlid_U7 As String = "REQSELP001_U7"
    Const sReqSqlid_S4 As String = "REQSELP001_S4"
    Const sCnsSqlid_D1 As String = "REQSELP001_D1"
    Const sCnsSqlid_D2 As String = "REQSELP001_D2"

    Const sCnsPrintButon As String = "印刷"
    Const sCnsAddButon As String = "登録"
    Const sCnsUpButon As String = "更新"
    Const sCnsDelButon As String = "削除"
    Const sCnsClrButon As String = "クリア"
    Const sCnsCMPSELP001Buton As String = "保守対応依頼書"

    '作業状況のステータスコード"08"
    Const M_WORK_STSCD = "08"

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

    'ボタンアクションの設定
    Public WithEvents btnAdd As Button          '登録
    Public WithEvents btnInsert As Button       '追加
    Public WithEvents btnUpdate As Button       '更新
    Public WithEvents btnDelete As Button       '削除
    Public WithEvents btnClere As Button        'クリア
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
    Dim DataSet_CNSUPDP001 As DataTable
    Dim REQSELP001_D73_TROUBLE As DataSet
    Dim REQSELP001_D74_TROUBLE_DTIL As DataSet

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        '表設定
        pfSet_GridView(grvList, sCnsProgid, 60, 9)

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

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            'Dim strKeyList As List(Of String)
            Dim strKeyList() As String
            Dim strTerms As String = String.Empty
            '--------------------------------
            '2014/04/ 高松　ここから
            '--------------------------------
            Dim strolddisp As String = String.Empty
            '--------------------------------
            '2014/04/17 高松　ここまで
            '--------------------------------
            Dim dstOrders_1 As New DataSet
            Dim dstOrders_2 As New DataSet
            Dim dstOrders_3 As New DataSet

            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton3.Click, AddressOf Button_Click   '印刷
            AddHandler Master.ppRigthButton6.Click, AddressOf Button_Click  '保守対応依頼書
            AddHandler Master.ppRigthButton5.Click, AddressOf Button_Click  '保守対応依頼書2
            'REQSELP001-002
            AddHandler Master.ppRigthButton9.Click, AddressOf btnHallMstMng_Click   '対応履歴照会
            AddHandler Master.ppRigthButton10.Click, AddressOf btnHallMstMng_Click    'ホールマスタ管理
            'REQSELP001-002 END

            AddHandler btnAdd.Click, AddressOf Button_Click                  '登録
            AddHandler btnInsert.Click, AddressOf Button_Click               '追加
            AddHandler btnUpdate.Click, AddressOf Button_Click               '更新
            AddHandler btnDelete.Click, AddressOf Button_Click               '削除
            AddHandler btnClere.Click, AddressOf Button_Click                'クリア

            AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf txtTboxId_TextChanged
            Me.txtTboxId.ppTextBox.AutoPostBack = True

            AddHandler Me.ddlEqCls.SelectedIndexChanged, AddressOf ddlEqCls_SelectedIndexChanged
            Me.ddlEqCls.AutoPostBack = True

            '--------------------------------
            '2015/02/10 加賀　ここから
            '--------------------------------
            AddHandler Me.ddlHpnCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlHpnCls_SelectedIndexChanged
            Me.ddlHpnCls.ppDropDownList.AutoPostBack = True
            '--------------------------------
            '2015/02/10 加賀　ここまで
            '--------------------------------

            Me.txaDealDtl.Attributes.Add("onKeyUp", "lenCheck(this,""" & Me.txaDealDtl.MaxLength & """);")
            Me.txaDealDtl.Attributes.Add("onChange", "lenCheck(this,""" & Me.txaDealDtl.MaxLength & """);")

            tetInhCntnt1.ppTextBox.ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)
            tetInhCntnt2.ppTextBox.ForeColor = Drawing.Color.FromArgb(255, 255, 0, 0)

            If Not IsPostBack Then  '初回表示
                'セッション項目取得
                ViewState(P_KEY) = Session(P_KEY)
                ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)
                'ビューステート項目取得
                strKeyList = ViewState(P_KEY)
                '--------------------------------
                '2014/04/17 高松　ここから
                '--------------------------------
                strolddisp = Session(P_SESSION_OLDDISP)
                '--------------------------------
                '2014/04/17 高松　ここまで
                '--------------------------------

                '排他情報用のグループ番号保管
                If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                    ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

                End If

                If strKeyList Is Nothing Then

                    Me.ViewState("遷移元") = "一覧"

                Else
                    '--------------------------------
                    '2014/04/17 高松　ここから
                    '--------------------------------
                    ''集信エラーホール一覧から遷移
                    'Select Case strKeyList.Count
                    '    Case 1 'トラブル管理表一覧
                    '        Me.ViewState("遷移元") = "一覧"
                    '    Case 2 '保守対応一覧
                    '        Me.ViewState("遷移元") = "保守"
                    '        strTerms =  ClsComVer.E_遷移条件.更新
                    '    Case 8 '集信エラーホール一覧
                    '        Me.ViewState("遷移元") = "集信"
                    'End Select
                    '集信エラーホール一覧から遷移
                    Select Case strolddisp
                        Case "REQLSTP001"                       'トラブル管理表一覧
                            Me.ViewState("遷移元") = "一覧"
                        Case "CMPSELP001"                       '保守対応依頼書
                            Me.ViewState("遷移元") = "保守"
                            '--------------------------------
                            '2014/05/29 松川　ここから
                            '--------------------------------
                            'strTerms =  ClsComVer.E_遷移条件.更新
                            '--------------------------------
                            '2014/05/29 松川　ここまで
                            '--------------------------------
                        Case "ERRSELP001"                       '集信エラーホール一覧
                            Me.ViewState("遷移元") = "集信"
                        Case "HEALSTP001"                       'ヘルスチェック一覧
                            Me.ViewState("遷移元") = "ヘルス"
                        Case "COMMENP006"                       'REQSELP001-001
                            Me.ViewState("遷移元") = "ホールマスタ管理"
                    End Select
                    '--------------------------------
                    '2014/04/17 高松　ここまで
                    '--------------------------------
                End If

                'ボタン押下時の検証を無効
                Master.ppRigthButton1.CausesValidation = True      '更新
                Master.ppRigthButton2.CausesValidation = True      '削除
                Master.ppRigthButton3.CausesValidation = False     '印刷
                Master.ppRigthButton6.CausesValidation = False    '保守対応依頼書
                Master.ppRigthButton5.CausesValidation = False    '保守対応依頼書
                'REQSELP001-002
                Master.ppRigthButton9.CausesValidation = False  '対応履歴照会
                Master.ppRigthButton10.CausesValidation = False   'ホールマスタ管理
                'REQSELP001-002 END

                '押下時のメッセージ設定
                Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票")          '更新
                Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票")          '削除
                Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票")          '印刷
                Master.ppRigthButton6.OnClientClick = pfGet_OCClickMes("30004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "保守対応依頼書")         '保守対応依頼書
                Master.ppRigthButton5.OnClientClick = pfGet_OCClickMes("30004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "保守対応依頼書")         '保守対応依頼書

                Me.btnAdd.OnClientClick = pfGet_OCClickMes("00008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票")                      '登録
                Me.btnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票(明細)")             '更新(明細)
                Me.btnClere.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "入力項目のクリア")                  'クリア(明細)
                Me.btnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票(明細)")             '削除(明細)
                Me.btnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "トラブル処理票(明細)")             '追加(明細)

                'ボタン名称設定
                Master.ppRigthButton1.Text = sCnsUpButon           '更新
                Master.ppRigthButton2.Text = sCnsDelButon          '削除
                Master.ppRigthButton3.Text = sCnsPrintButon        '印刷
                Master.ppRigthButton6.Text = sCnsCMPSELP001Buton  '保守対応依頼書
                Master.ppRigthButton5.Text = sCnsCMPSELP001Buton + "2"  '保守対応依頼書
                'REQSELP001-002
                Master.ppRigthButton10.Text = "ホールマスタ管理"    'ホールマスタ管理
                Master.ppRigthButton9.Text = "対応履歴照会"       '対応履歴照会
                'REQSELP001-002 END

                'ボタン表示切替
                Master.ppRigthButton1.Visible = True               '更新
                Master.ppRigthButton2.Visible = True               '削除
                Master.ppRigthButton3.Visible = True               '印刷
                Master.ppRigthButton6.Visible = True              '保守対応依頼書
                Master.ppRigthButton5.Visible = False
                'REQSELP001-002
                Master.ppRigthButton10.Visible = True                 'ホールマスタ管理
                Master.ppRigthButton9.Visible = True                '対応履歴照会
                'REQSELP001-002 END

                '検証グループ設定
                Master.ppRigthButton1.ValidationGroup = "2"        '更新
                Master.ppRigthButton2.ValidationGroup = "2"        '削除

                Me.btnAdd.ValidationGroup = "1"                    '登録
                Me.btnUpdate.ValidationGroup = "3"                 '更新(明細)
                Me.btnDelete.ValidationGroup = "3"                 '削除(明細)
                Me.btnInsert.ValidationGroup = "3"                 '追加(明細)

                Master.ppRigthButton2.BackColor = Drawing.Color.FromArgb(255, 102, 102)          '削除
                btnDelete.BackColor = Drawing.Color.FromArgb(255, 102, 102)

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = sCnsProgid
                Master.ppTitle = clsCMDBC.pfGet_DispNm(sCnsProgid)
                '登録用に設定
                ViewState(P_SESSION_BCLIST) = Session(P_SESSION_BCLIST)
                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                If Session("P_CLER") = "1" Then
                    Exit Sub
                End If


                'データ取得処理
                If Not msGet_Data(dstOrders_1, dstOrders_2, dstOrders_3) Then
                    Exit Sub
                End If

                '編集表示処理
                If Not msGet_Edit(dstOrders_1, dstOrders_2, dstOrders_3) Then
                    Exit Sub
                End If

                '★排他情報削除用の登録年月日時刻をマスタページのコントロールに登録
                If Not Session(P_SESSION_EXCLUSIV_DATE) Is Nothing Then

                    Me.Master.ppExclusiveDate = Session(P_SESSION_EXCLUSIV_DATE)

                End If
                strTerms = ViewState(P_SESSION_TERMS)

                Select Case strTerms.ToString.Trim

                    Case ClsComVer.E_遷移条件.参照

                        '非活性化処理
                        msEdit_Enabled("1")

                        'ボタン制御
                        msChange_btn("参照")

                    Case ClsComVer.E_遷移条件.更新

                        '非活性化処理
                        msEdit_Enabled("2")

                        '必須項目設定処理
                        msEdit_RequiredField()

                        '明細項目初期化
                        msDtil_Clere()

                        'ボタン制御
                        msChange_btn("更新")
                        msChange_btn("明細_更新")

                        Me.ddlHpnCls.ppDropDownList.Focus()

                    Case ClsComVer.E_遷移条件.登録

                        '非活性化処理
                        msEdit_Enabled("3")

                        '必須項目設定処理
                        msEdit_RequiredField()

                        '初期フォーカス
                        Me.txtTboxId.ppTextBox.Focus()

                        '画面ページ表示初期化
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()

                        'ボタン制御
                        msChange_btn("登録")
                        msChange_btn("明細_登録")

                        Select Case Me.ViewState("遷移元")
                            Case "集信"
                                Dim strView_in() As String = Me.ViewState(P_KEY)
                                Me.txtTboxId.ppText = strView_in(7)
                            Case "ヘルス"
                                Dim strView_in() As String = Me.ViewState(P_KEY)
                                Me.txtTboxId.ppText = strView_in(7)
                            Case "ホールマスタ管理" 'REQSELP001-001
                                Me.txtTboxId.ppText = strKeyList(0)
                                txtTboxId_TextChanged(Nothing, Nothing)
                        End Select

                End Select
                ''--------------------------------
                ''2014/05/29 松川　ここから
                ''--------------------------------
                'If Me.ViewState("遷移元") = "保守" Then
                '    Master.ppRigthButton6.Enabled = False          '保守対応依頼書
                'End If
                ''--------------------------------
                ''2014/05/29 松川　ここまで
                ''--------------------------------
            End If

        Catch ex As Exception

            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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

#Region "ユーザー権限"
    '---------------------------
    '2014/04/16 武 ここから
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

        If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
            Dim dt As DateTime = DateTime.Now
            '--------------------------------
            '2015/02/10 加賀　ここから
            '--------------------------------
            'If dttRptDt.ppText = String.Empty Then
            If dttRptDt.ppText = String.Empty AndAlso ddlHpnCls.ppSelectedValue <> "2" Then
                dttRptDt.ppText = dt.ToString("yyyy/MM/dd")
            End If
            '--------------------------------
            '2015/02/10 加賀　ここまで
            '--------------------------------
            If dttRcptDt.ppText = String.Empty AndAlso dttRcptDt.ppHourText = String.Empty AndAlso dttRcptDt.ppMinText = String.Empty Then
                dttRcptDt.ppText = dt.ToString("yyyy/MM/dd")
                dttRcptDt.ppHourText = dt.ToString("HH")
                dttRcptDt.ppMinText = dt.ToString("mm")
            End If
            If txtDealDt.ppText = String.Empty AndAlso txtDealDt.ppHourText = String.Empty AndAlso txtDealDt.ppMinText = String.Empty Then
                txtDealDt.ppText = dt.ToString("yyyy/MM/dd")
                txtDealDt.ppHourText = dt.ToString("HH")
                txtDealDt.ppMinText = dt.ToString("mm")
            End If
        End If

    End Sub
    '---------------------------
    '2014/04/16 武 ここまで
    '---------------------------
#End Region

#Region "ボタン押下処理"

    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        'Dim trans As SqlTransaction = conDB.BeginTransaction
        Dim strKey() As String = Nothing
        Dim responsKey(0) As String

        Dim dstOrders_1 As New DataSet
        Dim dstOrders_2 As New DataSet
        Dim dstOrders_3 As New DataSet

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            Select Case sender.id
                Case "btnAdd"       'トラブル 登録

                    msIsvalid_chk("btnAdd")

                    If (Page.IsValid) Then

                        'トラブル 登録・更新・削除処理
                        If Not Me.msSet_Data(1) Then
                            Exit Sub
                        End If

                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                        ViewState(P_KEY) = {ViewState("TRBL_NO").ToString}

                        'データ取得処理
                        If Not msGet_Data(dstOrders_1, dstOrders_2, dstOrders_3) Then
                            Exit Sub
                        End If

                        '編集表示処理
                        If Not msGet_Edit(dstOrders_1, dstOrders_2, dstOrders_3) Then
                            Exit Sub
                        End If

                        '非活性化処理
                        msEdit_Enabled("2")

                        '必須項目設定処理
                        msEdit_RequiredField()

                        '明細項目初期化
                        msDtil_Clere()

                        'ボタン制御
                        msChange_btn("更新")
                        msChange_btn("明細_更新")

                        Me.ddlHpnCls.ppDropDownList.Focus()

                    End If

                Case "btnInsert"    'トラブル明細 追加

                    msIsvalid_chk("btnInsert")

                    If (Page.IsValid) Then

                        'トラブル明細 登録・更新・削除処理
                        Me.msSet_Data(4)

                        '明細編集項目初期表示処理
                        Me.msDtil_Clere()

                        'ボタン制御
                        msChange_btn("追加")

                        Page.SetFocus(chkAdptCls)

                    End If

                Case "btnUpdate"    'トラブル明細 更新

                    msIsvalid_chk("btnUpdate")

                    If (Page.IsValid) Then

                        'トラブル明細 登録・更新・削除処理
                        Me.msSet_Data(5)
                        Page.SetFocus(chkAdptCls)
                    End If

                Case "btnDelete"    'トラブル明細 削除

                    'トラブル明細 登録・更新・削除処理
                    Me.msSet_Data(6)

                    '明細編集項目初期表示処理
                    Me.msDtil_Clere()

                    'ボタン制御
                    msChange_btn("削除")
                    Page.SetFocus(chkAdptCls)

                Case "btnClere"     'クリア


                    '明細編集項目初期表示処理
                    Me.msDtil_Clere()

                    'ボタン制御
                    msChange_btn("クリア")
                    '-----------------------------
                    '2014/06/24 武　ここから
                    '-----------------------------
                    '参照時は選択したときのみ使用できる
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                        Me.btnClere.Enabled = False
                    End If
                    '-----------------------------
                    '2014/06/24 武　ここまで
                    '-----------------------------
                    Page.SetFocus(chkAdptCls)

                Case "btnRigth1"  'トラブル 更新

                    msIsvalid_chk("btnRigth1")

                    If (Page.IsValid) Then

                        'トラブル 登録・更新・削除処理
                        Me.msSet_Data(2)

                        'データ取得処理
                        If Not msGet_Data(dstOrders_1, dstOrders_2, dstOrders_3) Then
                            Exit Sub
                        End If

                        '編集表示処理
                        If Not msGet_Edit(dstOrders_1, dstOrders_2, dstOrders_3) Then
                            Exit Sub
                        End If

                    End If

                Case "btnRigth2"  'トラブル 削除

                    'トラブル 登録・更新・削除処理
                    Me.msSet_Data(3)

                    Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                    ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録

                    'TBOX情報初期化
                    msInitTBoxData()

                    ViewState("TRBL_NO") = String.Empty
                    Me.txtTboxId.ppText = String.Empty                        'ＴＢＯＸＩＤ
                    Me.ddlTboxType.SelectedValue = String.Empty               'ＴＢＯＸタイプ
                    Me.ddlHpnCls.ppSelectedValue = String.Empty               '発生区分
                    Me.ddlMngCls.ppSelectedValue = String.Empty               '管理区分
                    Me.lblTrblno.Text = String.Empty                          '管理番号
                    Me.lblIraino.Text = String.Empty                          '依頼書番号（保守管理番号）
                    Me.lblIraino2.Text = String.Empty                         '依頼書番号（保守管理番号）
                    Me.dttRptDt.ppText = String.Empty
                    Me.dttRcptDt.ppText = String.Empty
                    Me.dttRcptDt.ppHourText = String.Empty
                    Me.dttRcptDt.ppMinText = String.Empty
                    Me.tetRpt.ppText = String.Empty                         '申告者
                    Me.ddlRptBase.SelectedValue = String.Empty              '申告元
                    Me.tetRptTel.ppText = String.Empty                      '申告者ＴＥＬ
                    Me.tetRptCharg.ppText = String.Empty                    '受付者
                    Me.ddlRptCd.SelectedValue = String.Empty                '申告内容
                    Me.tetRptDtl1.ppText = String.Empty                     '申告内容
                    Me.tetRptDtl2.ppText = String.Empty                     '申告内容
                    Me.chkImpCls.Checked = False
                    Me.tetInhCntnt1.ppText = String.Empty                   '引継内容
                    Me.tetInhCntnt2.ppText = String.Empty
                    Me.CheckBoxList1.Items(0).Selected = False
                    Me.CheckBoxList1.Items(1).Selected = False
                    Me.ddlDealreqCd1.ppSelectedValue = String.Empty         '対応依頼１
                    Me.ddlDealreqCd2.ppSelectedValue = String.Empty         '対応依頼２
                    Me.ddlDealreqCd3.ppSelectedValue = String.Empty         '対応依頼３
                    Me.ddlEqCls.SelectedValue = String.Empty              '装置区分
                    Me.ddlEqCd.SelectedValue = String.Empty               '装置詳細
                    Me.ddlStatusCd.SelectedValue = String.Empty             '作業状況
                    Me.txtSttsNotetext.ppText = String.Empty
                    Me.txtSttsNotetext.ppEnabled = False
                    Me.ddlRepairCd.SelectedValue = String.Empty             '回復内容
                    Me.tetRepairCcntnt.ppText = String.Empty                '回復内容
                    Me.lblConfDt.Text = String.Empty
                    Me.txtConfUsr.ppText = String.Empty
                    'Me.ddlConfUsr.SelectedValue = String.Empty              '確認者
                    Me.lblAppDt.Text = String.Empty
                    Me.ddlAddUsr.SelectedValue = String.Empty               '承認者
                    Master.ppRigthButton6.Enabled = False                  '保守対応依頼書
                    Master.ppRigthButton5.Enabled = False                  '保守対応依頼書
                    chkAdptCls.Checked = False                '印刷区分
                    txtDealCd.ppText = ""              '対応コード
                    txtDealDt.ppDateBox.Text = String.Empty  '対応日時
                    txtDealDt.ppHourText = String.Empty      '対応日時
                    txtDealDt.ppMinText = String.Empty       '対応日時
                    txtDealUer.ppText = Nothing           '対応担当者
                    txaDealDtl.Text = Nothing              '対応内容

                    '非活性化処理
                    msEdit_Enabled("3")

                    '必須項目設定処理
                    msEdit_RequiredField()

                    '初期フォーカス
                    Me.txtTboxId.ppTextBox.Focus()

                    '画面ページ表示初期化
                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()

                    'ボタン制御
                    msChange_btn("登録")
                    msChange_btn("明細_登録")

                    If Me.ViewState("遷移元") = "集信" Then
                        Dim strView_in() As String = Me.ViewState(P_KEY)
                        Me.txtTboxId.ppText = strView_in(7)
                    ElseIf Me.ViewState("遷移元") = "ヘルス" Then
                        Dim strView_in() As String = Me.ViewState(P_KEY)
                        Me.txtTboxId.ppText = strView_in(7)
                    End If

                    'psClose_Window(Me)

                Case "btnRigth3"  '印刷"
                    '印刷処理
                    Me.msSet_Print()

                Case "btnRigth6"  '保守対応依頼書
                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text

                    '2014/06/05 間瀬 再読み込みを行いと保守対応依頼書の管理番号を更新する
                    'データ取得処理
                    If Not msGet_Data(dstOrders_1, dstOrders_2, dstOrders_3) Then
                        Exit Sub
                    End If

                    If dstOrders_2 Is Nothing Then
                        Me.grvList.DataSource = Nothing
                    Else
                        Me.grvList.DataSource = dstOrders_2.Tables(0)
                    End If
                    Me.grvList.DataBind()

                    Me.lblIraino.Text = dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO").ToString                             '依頼書番号（保守管理番号）

                    '--------------------------------
                    '2014/05/29 松川　ここから
                    '--------------------------------
                    '保守管理番号・トラブル管理番号・ＴＢＯＸＩＤ・申告日・申告者・申告元コード・申告元ＴＥＬ・受付日時・受付者
                    Dim strArry(12 - 1) As String

                    strArry(0) = lblIraino.Text
                    strArry(1) = lblTrblno.Text
                    strArry(2) = txtTboxId.ppText

                    If Me.dttRptDt.ppDateBox.Text = String.Empty Or Me.dttRptDt.ppDateBox.Text Is Nothing Then
                        strArry(3) = String.Empty
                    Else
                        strArry(3) = Me.dttRptDt.ppDate.ToString("yyyy/MM/dd")
                    End If
                    strArry(4) = Me.tetRpt.ppText
                    If Me.ddlRptBase.SelectedIndex = 0 Then
                        strArry(5) = String.Empty
                    Else
                        strArry(5) = Me.ddlRptBase.SelectedValue
                    End If
                    strArry(6) = Me.tetRptTel.ppText
                    If Me.dttRcptDt.ppDateBox.Text = String.Empty Or Me.dttRcptDt.ppDateBox.Text Is Nothing Then
                        strArry(7) = String.Empty
                    Else
                        strArry(7) = Me.dttRcptDt.ppDateBox.Text _
                                     + " " _
                                     + CInt(Me.dttRcptDt.ppHourText).ToString("00") _
                                     + ":" _
                                     + CInt(Me.dttRcptDt.ppMinText).ToString("00")
                    End If
                    strArry(8) = Me.tetRptCharg.ppText

                    strArry(9) = Me.ddlRptCd.SelectedValue
                    strArry(10) = Me.tetRptDtl1.ppText
                    strArry(11) = Me.tetRptDtl2.ppText


                    'strKey = strArry
                    Session(P_KEY) = strArry
                    Session(P_SESSION_OLDDISP) = sCnsProgid
                    'Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)
                    If Me.lblIraino.Text = String.Empty AndAlso
                       ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                    Else
                        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)
                    End If
                    '--------------------------------
                    '2014/05/29 松川　ここまで
                    '--------------------------------

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
                                    objStack.GetMethod.Name, sCnsCMPSELP001, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, sCnsCMPSELP001)

                    '--------------------------------
                    '2014/05/29 松川　ここから
                    '--------------------------------
                    'Master.ppRigthButton6.Enabled = False          '保守対応依頼書
                    '--------------------------------
                    '2014/05/29 松川　ここまで
                    '--------------------------------


                Case "btnRigth5"  '保守対応依頼書
                    Session(P_SESSION_BCLIST) = Master.ppBcList_Text

                    'データ取得処理
                    If Not msGet_Data(dstOrders_1, dstOrders_2, dstOrders_3) Then
                        Exit Sub
                    End If

                    If dstOrders_2 Is Nothing Then
                        Me.grvList.DataSource = Nothing
                    Else
                        Me.grvList.DataSource = dstOrders_2.Tables(0)
                    End If
                    Me.grvList.DataBind()

                    Me.lblIraino2.Text = dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO2").ToString                             '依頼書番号（保守管理番号）

                    '保守管理番号・トラブル管理番号・ＴＢＯＸＩＤ・申告日・申告者・申告元コード・申告元ＴＥＬ・受付日時・受付者
                    Dim strArry(12 - 1) As String

                    strArry(0) = lblIraino2.Text
                    strArry(1) = lblTrblno.Text
                    strArry(2) = txtTboxId.ppText

                    If Me.dttRptDt.ppDateBox.Text = String.Empty Or Me.dttRptDt.ppDateBox.Text Is Nothing Then
                        strArry(3) = String.Empty
                    Else
                        strArry(3) = Me.dttRptDt.ppDate.ToString("yyyy/MM/dd")
                    End If
                    strArry(4) = Me.tetRpt.ppText
                    If Me.ddlRptBase.SelectedIndex = 0 Then
                        strArry(5) = String.Empty
                    Else
                        strArry(5) = Me.ddlRptBase.SelectedValue
                    End If
                    strArry(6) = Me.tetRptTel.ppText
                    If Me.dttRcptDt.ppDateBox.Text = String.Empty Or Me.dttRcptDt.ppDateBox.Text Is Nothing Then
                        strArry(7) = String.Empty
                    Else
                        strArry(7) = Me.dttRcptDt.ppDateBox.Text _
                                     + " " _
                                     + CInt(Me.dttRcptDt.ppHourText).ToString("00") _
                                     + ":" _
                                     + CInt(Me.dttRcptDt.ppMinText).ToString("00")
                    End If
                    strArry(8) = Me.tetRptCharg.ppText

                    strArry(9) = Me.ddlRptCd.SelectedValue
                    strArry(10) = Me.tetRptDtl1.ppText
                    strArry(11) = Me.tetRptDtl2.ppText

                    Session(P_KEY) = strArry
                    Session(P_SESSION_OLDDISP) = sCnsProgid
                    If Me.lblIraino2.Text = String.Empty AndAlso
                       ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新 Then
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録
                    Else
                        Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)
                    End If

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
                                    objStack.GetMethod.Name, sCnsCMPSELP001, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    psOpen_Window(Me, sCnsCMPSELP001)


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
            'メッセージは各処理で出力
        End Try

    End Sub

    ''' <summary>
    ''' ホールマスタ管理/対応履歴照会ボタンクリック時処理 CMPSELP001-002
    ''' </summary>
    Private Sub btnHallMstMng_Click(sender As Object, e As EventArgs)
        Try
            Dim strKeyList As List(Of String) = Nothing '画面引継ぎ用キー情報
            Dim strDispPath As String = String.Empty
            objStack = New StackFrame

            '開始ログ出力
            psLogStart(Me)

            '画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            strKeyList.Add(Me.txtTboxId.ppText)                 'ＴＢＯＸＩＤ
            strKeyList.Add(hdnNlclsCd.Value)                    'NL
            strKeyList.Add(ViewState("SYSTEM_CLS").ToString)    'システム分類
            strKeyList.Add(hdnHallCd.Value)                     'ホールコード

            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = sCnsProgid
            Session(P_KEY) = strKeyList.ToArray
            Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)
            Session(P_SESSION_TERMS) = ViewState(P_SESSION_TERMS)

            '遷移先設定
            Select Case DirectCast(sender, Button).ID
                Case Master.ppRigthButton10.ID
                    'ホールマスタ管理
                    strDispPath = M_HMST_DISP_PATH
                Case Master.ppRigthButton9.ID
                    '対応履歴照会
                    strDispPath = M_CHST_DISP_PATH
            End Select

            '--------------------------------
            '2014/04/16 星野　ここから
            '--------------------------------
            '■□■□結合試験時のみ使用予定□■□■
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
                            objStack.GetMethod.Name, strDispPath, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------

            '画面起動
            psOpen_Window(Me, strDispPath)

            '終了ログ出力
            psLogEnd(Me)

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
    End Sub

#End Region

#Region "テキスト編集処理"

    ''' <summary>
    ''' TBOXID変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxId_TextChanged(sender As System.Object, e As System.EventArgs)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            '入力がない場合
            If txtTboxId.ppText.Trim() = "" Then
                'TBOX情報の初期化
                msInitKmkMainIns()
            Else
                'TBOXデータの設定
                If mfSetTboxData(txtTboxId.ppText.Trim()) = False Then
                    Throw New Exception
                End If
            End If

        Catch ex As Exception
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 装置区分変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlEqCls_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Dim dsEqCd As DataSet

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If
            '***** ドロップダウンリスト表示データ取得 *****
            cmdDB = New SqlCommand("ZCMPSEL049", conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("prc_dvs", SqlDbType.NVarChar, "1"))                           '処理区分
                .Add(pfSet_Param("eq_cls", SqlDbType.NVarChar, Me.ddlEqCls.SelectedValue))    '装置区分
            End With

            dsEqCd = clsDataConnect.pfGet_DataSet(cmdDB)

            '装置区分リスト
            Me.ddlEqCd.DataSource = dsEqCd.Tables(0)
            Me.ddlEqCd.DataValueField = dsEqCd.Tables(0).Columns(1).ColumnName.ToString
            Me.ddlEqCd.DataTextField = dsEqCd.Tables(0).Columns(0).ColumnName.ToString
            Me.ddlEqCd.DataBind()
            '一行目を設定
            Me.ddlEqCd.Items.Insert(0, "")
            Me.ddlEqCd.SelectedIndex = 0
        Catch ex As Exception
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 発生区分変更時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlHpnCls_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
        '--------------------------------
        '2015/02/10 加賀　ここから
        '--------------------------------
        Try
            '[2:発見]選択時、申告日項目のクリア
            Select Case DirectCast(sender, DropDownList).SelectedValue
                Case "2" '発見
                    dttRptDt.ppText = ""
            End Select
        Catch ex As Exception
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try
        '--------------------------------
        '2015/02/10 加賀　ここまで
        '--------------------------------
    End Sub
#End Region

#Region "データバインド時処理"

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strTEKIYOF As String = Nothing '適応区分
        Dim strHANDANF As String = Nothing '判断区分

        '適応区を判断する
        For Each rowData As GridViewRow In grvList.Rows
            strTEKIYOF = CType(rowData.FindControl("適応区分"), TextBox).Text
            strHANDANF = CType(rowData.FindControl("判断区分"), TextBox).Text
            rowData.Cells(1).Enabled = False
            If strTEKIYOF = "0" Then   '適応区分 0:適用外
                CType(rowData.FindControl("ボタン対応コード"), CheckBox).Checked = False
            Else
                CType(rowData.FindControl("ボタン対応コード"), CheckBox).Checked = True
            End If
            'If strHANDANF = "02" Then
            '    rowData.Cells(0).Enabled = False
            'End If
            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("対応コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応担当"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            End If

        Next

    End Sub

#End Region

#Region "一覧の選択ボタン押下処理"
    ''' <summary>
    ''' 一覧の選択ボタン押下処理
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

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    ' ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             ' ボタン押下行

            If e.CommandName = sCnsbtnSntak Then  '選択

                txtDealUer.ppText = CType(rowData.FindControl("対応担当"), TextBox).Text       '対応担当者

                If CType(rowData.FindControl("適応区分"), TextBox).Text = "0" Then

                    chkAdptCls.Checked = False                                                        '適応区分
                Else
                    chkAdptCls.Checked = True                                                         '適応区分

                End If
                txtDealCd.ppText = CType(rowData.FindControl("対応コード"), TextBox).Text.Replace("◆", "") '対応コード
                If CType(rowData.FindControl("対応日時"), TextBox).Text Is Nothing _
                    Or CType(rowData.FindControl("対応日時"), TextBox).Text = String.Empty Then
                    txtDealDt.ppDateBox.Text = String.Empty                                           '対応日時
                    txtDealDt.ppHourText = String.Empty
                    txtDealDt.ppMinText = String.Empty
                Else
                    txtDealDt.ppDateBox.Text = CType(rowData.FindControl("対応日時"), TextBox).Text.Substring(0, 10) '対応日時

                    '--------------------------------
                    '2014/05/22 後藤　ここから
                    '--------------------------------
                    'txtDealDt.ppHourText = CType(rowData.FindControl("対応日時"), TextBox).Text.Substring(11, 2)
                    'txtDealDt.ppMinText = CType(rowData.FindControl("対応日時"), TextBox).Text.Substring(14, 2)

                    Dim dtVal As DateTime = Nothing
                    If DateTime.TryParse(CType(rowData.FindControl("対応日時"), TextBox).Text, dtVal) Then
                        txtDealDt.ppHourText = dtVal.ToString("HH")
                        txtDealDt.ppMinText = dtVal.ToString("mm")
                    End If
                    '--------------------------------
                    '2014/05/22 後藤　ここまで
                    '--------------------------------
                End If
                txaDealDtl.Text = CType(rowData.FindControl("対応内容"), TextBox).Text              '対応内容
                ViewState("TRBL_SEQ") = CType(rowData.FindControl("管理連番"), TextBox).Text          '管理連番
                ViewState("HANDANF") = CType(rowData.FindControl("判断区分"), TextBox).Text          '判断区分
                txtDealCd.ppTextBox.Focus()
                msChange_btn("選択")

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
        End Try

    End Sub

#End Region

#End Region

#Region "そのほかのプロシージャ"

#Region "初期化"

    ''' <summary>
    ''' 登録時にクリアする項目のクリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitKmkMainIns()
        lblTrblno.Text = "" '管理番号
        lblIraino.Text = ""
        lblIraino2.Text = ""

        'TBOX情報
        msInitTBoxData()

        Me.txtTboxId.ppText = ""
        Me.ddlTboxType.SelectedIndex = -1

    End Sub

    ''' <summary>
    ''' TBOX情報の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msInitTBoxData()
        '--------------------------------
        '2015/02/05 加賀　ここから
        '--------------------------------
        lblTboxType.Text = "" 'TBOXタイプ
        '--------------------------------
        '2015/02/05 加賀　ここまで
        '--------------------------------
        lblVer.Text = "" 'Ｖｅｒ
        hdnSysCd.Value = "" 'システムコード
        lblNlcls.Text = "" 'ＮＬ区分
        hdnNlclsCd.Value = "" 'ＮＬ区分
        lblEwcls.Text = "" 'ＥＷ区分
        hdnEwclsCd.Value = "" 'ＥＷ区分
        hdnHallCd.Value = "" 'ホールコード
        lblHallNm.Text = "" 'ホール名
        lblAddr1.Text = "" 'ホール住所
        lblTel.Text = "" 'ホールTEL
        lblWrkCharge.Text = "" '保担名
        hdnEigyoCd.Value = "" '営業所コード
        lblTokatuCharge.Text = "" '統括名
        hdnTokatuCd.Value = "" '統括コード
        lblTwin.Text = "" '双子店区分
        hdnTwinCd.Value = "" '双子店区分
        lblAgc.Text = "" '代理店名
        hdnAgcCd.Value = "" '代理店コード
        hdnAgcZip.Value = "" '代理店郵便番号
        hdnAgcAddr.Value = "" '代理店住所
        hdnAgcTel.Value = "" '代理店ＴＥＬ
        hdnAgcFax.Value = "" '代理店ＦＡＸ
        lblRep.Text = "" '代行店名
        hdnRepCd.Value = "" '代行店コード
        hdnRepZip.Value = "" '代行店郵便番号
        hdnRepAddr.Value = "" '代行店住所
        hdnRepTel.Value = "" '代行店ＴＥＬ
        hdnRepChg.Value = "" '代行店担当者
        lblNgcOrg.Text = "" 'ＮＧＣ担当営業部
        lblOrgTel.Text = "" '担当営業部ＴＥＬ
        lblEst.Text = "" 'ＭＤＮ設置有無
        hdnEstCls.Value = "" 'ＭＤＮ設置有無
        lblMdn.Text = "" 'ＭＤＮ機器名
        hdnMdnCnt.Value = "" 'ＭＤＮ台数
        hdnMdnCd1.Value = "" 'ＭＤＮ機器コード１
        hdnMdnCd2.Value = "" 'ＭＤＮ機器コード２
        hdnMdnCd3.Value = "" 'ＭＤＮ機器コード３
    End Sub

#End Region

#Region "項目活性化・非活性化処理"

    ''' <summary>
    ''' 項目活性化・非活性化処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEdit_Enabled(ByVal Terms As String)

        'ボタン非活性化
        Master.ppRigthButton1.Enabled = False
        Master.ppRigthButton2.Enabled = False
        Master.ppRigthButton3.Enabled = False

        Select Case Terms
            Case "1"

                Me.Panel1.Enabled = False
                Me.Panel2.Enabled = False
                Me.Panel4.Enabled = True
                Me.Panel5.Enabled = True

                'コントロールの非活性化
                Me.chkAdptCls.Enabled = False
                Me.txtDealUer.ppEnabled = False
                Me.txtDealDt.ppEnabled = False
                Me.txaDealDtl.Enabled = False
                Me.txtDealCd.ppEnabled = False

            Case "2"

                Me.Panel1.Enabled = False
                Me.Panel2.Enabled = True
                Me.Panel4.Enabled = True
                Me.Panel5.Enabled = True

            Case "3"

                Me.Panel1.Enabled = True
                Me.Panel2.Enabled = False
                Me.Panel4.Enabled = False
                Me.Panel5.Enabled = False

        End Select

    End Sub

#End Region

#Region "必須項目設定処理"

    ''' <summary>
    ''' 必須項目の設定(True：必須/False：任意)処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msEdit_RequiredField()

        Me.txtTboxId.ppRequiredField = True                             'ＴＢＯＸＩＤ
        'Me.txtDealCd.ValidateRequestMode = True
        'Me.txtDealDt.ValidateRequestMode = True
        'Me.txtDealDtl.ValidateRequestMode = True

        Me.txtDealCd.ValidateRequestMode = UI.ValidateRequestMode.Enabled
        Me.txtDealDt.ValidateRequestMode = UI.ValidateRequestMode.Enabled
        Me.txaDealDtl.ValidateRequestMode = UI.ValidateRequestMode.Enabled

    End Sub

#End Region

#Region "ボタン制御処理"

    ''' <summary>
    ''' ボタン制御処理
    ''' </summary>
    ''' <param name="flag"></param>
    ''' <remarks></remarks>
    Private Sub msChange_btn(flag As String)

        Dim strTerms As String = ViewState(P_SESSION_TERMS)

        Select Case flag

            Case "参照"

                Me.btnAdd.Enabled = False
                Me.btnClere.Enabled = False
                Me.btnDelete.Enabled = False
                Me.btnInsert.Enabled = False
                Me.btnUpdate.Enabled = False
                Master.ppRigthButton3.Enabled = True            '印刷
                'REQSELP001-002
                Master.ppRigthButton9.Enabled = True    '対応履歴照会
                Master.ppRigthButton10.Enabled = True   'ホールマスタ管理
                'REQSELP001-002 END

            Case "更新"

                Me.btnAdd.Enabled = False
                Me.btnClere.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnInsert.Enabled = True
                Me.btnUpdate.Enabled = False
                'REQSELP001-002
                Master.ppRigthButton9.Enabled = True    '対応履歴照会
                Master.ppRigthButton10.Enabled = True   'ホールマスタ管理
                'REQSELP001-002 END

            Case "登録"

                Me.btnAdd.Enabled = True
                Me.btnClere.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnInsert.Enabled = True
                Me.btnUpdate.Enabled = False
                Master.ppRigthButton6.Enabled = False
                Master.ppRigthButton5.Enabled = False
                'REQSELP001-002
                Master.ppRigthButton9.Enabled = False   '対応履歴照会
                Master.ppRigthButton10.Enabled = False    'ホールマスタ管理
                'REQSELP001-002 END

            Case "追加"

                Me.btnAdd.Enabled = False
                Me.btnClere.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnInsert.Enabled = True
                Me.btnUpdate.Enabled = False

            Case "選択"
                If strTerms = ClsComVer.E_遷移条件.参照 Then
                    Me.btnAdd.Enabled = False
                    Me.btnClere.Enabled = True
                    Me.btnDelete.Enabled = False
                    Me.btnInsert.Enabled = False
                    Me.btnUpdate.Enabled = False
                Else
                    Me.btnAdd.Enabled = False
                    Me.btnClere.Enabled = True
                    Me.btnDelete.Enabled = True
                    Me.btnInsert.Enabled = False
                    Me.btnUpdate.Enabled = True
                End If
            Case "更新"

                Me.btnAdd.Enabled = False
                Me.btnClere.Enabled = True
                Me.btnDelete.Enabled = True
                Me.btnInsert.Enabled = False
                Me.btnUpdate.Enabled = True

            Case "削除"

                Me.btnAdd.Enabled = False
                Me.btnClere.Enabled = True
                Me.btnDelete.Enabled = False
                Me.btnInsert.Enabled = True
                Me.btnUpdate.Enabled = False

            Case "クリア"
                If strTerms = ClsComVer.E_遷移条件.参照 Then
                    Me.btnAdd.Enabled = False
                    Me.btnClere.Enabled = True
                    Me.btnDelete.Enabled = False
                    Me.btnInsert.Enabled = False
                    Me.btnUpdate.Enabled = False
                Else
                    Me.btnAdd.Enabled = False
                    Me.btnClere.Enabled = True
                    Me.btnDelete.Enabled = False
                    Me.btnInsert.Enabled = True
                    Me.btnUpdate.Enabled = False
                End If

            Case "明細_登録"

                'ボタン名称設定
                Master.ppRigthButton1.Enabled = False           '更新
                Master.ppRigthButton2.Enabled = False           '削除
                Master.ppRigthButton3.Enabled = False           '印刷


            Case "明細_更新"

                'ボタン名称設定
                Master.ppRigthButton1.Enabled = True            '更新
                If Me.ddlStatusCd.SelectedValue = "13" Then
                    Master.ppRigthButton2.Enabled = True           '削除
                Else
                    Master.ppRigthButton2.Enabled = False          '削除
                End If
                Master.ppRigthButton3.Enabled = True            '印刷

        End Select



    End Sub

    ''' <summary>
    ''' 作業状況ドロップダウンリスト変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlStatusCd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStatusCd.SelectedIndexChanged
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット


        '選択ステータスコード＝"08"の場合は入力可能にする
        If ddlStatusCd.SelectedValue = M_WORK_STSCD Then
            Me.txtSttsNotetext.ppEnabled = True
            Page.SetFocus(Me.txtSttsNotetext.FindControl("txtTextBox"))
        Else
            Me.txtSttsNotetext.ppText = String.Empty
            Me.txtSttsNotetext.ppEnabled = False
            Page.SetFocus(Me.ddlRepairCd)
        End If
        If Me.ddlStatusCd.SelectedValue = "13" Then
            Master.ppRigthButton2.Enabled = True           '削除
        Else
            Master.ppRigthButton2.Enabled = False          '削除
        End If

        If Session(P_SESSION_AUTH) = "管理者" Then
            If ddlStatusCd.SelectedValue = "12" Then
                'DB接続
                If clsDataConnect.pfOpen_Database(objCn) Then
                    Try
                        objCmd = New SqlCommand("ZCMPSEL051", objCn)

                        With objCmd.Parameters
                            .Add(pfSet_Param("userid", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                        End With


                        'データ取得
                        objDs = clsDataConnect.pfGet_DataSet(objCmd)
                        Me.ddlAddUsr.SelectedValue = objDs.Tables(0).Rows(0).Item("社員番号").ToString
                    Catch ex As Exception
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                    Finally
                        'DB切断
                        If Not clsDataConnect.pfClose_Database(objCn) Then
                        End If
                    End Try
                Else
                    psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
                Me.lblAppDt.Text = System.DateTime.Now.ToString("yyyy/MM/dd")
            Else
                Me.ddlAddUsr.SelectedIndex = 0
                Me.lblAppDt.Text = ""
            End If
        End If

    End Sub

#End Region

#Region "明細編集項目初期表示処理"

    ''' <summary>
    ''' 明細編集項目初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDtil_Clere()
        chkAdptCls.Checked = False                '印刷区分
        'If Not Me.lblTrblno.Text = String.Empty Then
        '    txtDealCd.ppText = "TR"&  Me.lblTrblno.Text.Substring(Me.lblTrblno.Text.Length - 4, 3)      '対応コード
        'Else
        txtDealCd.ppText = "TR"              '対応コード
        'End If
        txtDealDt.ppDateBox.Text = String.Empty  '対応日時
        txtDealDt.ppHourText = String.Empty      '対応日時
        txtDealDt.ppMinText = String.Empty       '対応日時
        txtDealUer.ppText = Nothing             '対応担当者
        txaDealDtl.Text = Nothing              '対応内容

    End Sub

#End Region

#Region "データ取得処理"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="dstOrders_2"></param>
    ''' <remarks></remarks>
    Private Function msGet_Data(ByRef dstOrders_1 As DataSet _
                               , ByRef dstOrders_2 As DataSet _
                               , ByRef dstOrders_3 As DataSet)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Dim strKey() As String = Nothing
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try

            '画面ページ表示初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return False
            End If

            If Not ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then

                'ビューステート項目取得
                strKey = ViewState(P_KEY)

                '***** トラブルデータ取得 *****

                'パラメータ設定
                cmdDB = New SqlCommand(sCnsSqlid_S1, conDB)

                With cmdDB.Parameters
                    If Me.ViewState("遷移元") = "保守" Then

                        .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, strKey(0)))         'トラブル管理番号
                        .Add(pfSet_Param("MNT_NO", SqlDbType.NVarChar, DBNull.Value))       '保守管理番号

                    Else
                        .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, strKey(0)))         'トラブル管理番号
                        .Add(pfSet_Param("MNT_NO", SqlDbType.NVarChar, DBNull.Value))       '保守管理番号

                    End If
                End With

                dstOrders_1 = clsDataConnect.pfGet_DataSet(cmdDB)

                '***** トラブル明細データ取得 *****

                'パラメータ設定
                cmdDB = New SqlCommand(sCnsSqlid_S2, conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, dstOrders_1.Tables(0).Rows(0)("D73_TRBL_NO").ToString))              'トラブル管理番号
                    .Add(pfSet_Param("MNT_NO", SqlDbType.NVarChar, dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO").ToString)) '保守管理番号
                    .Add(pfSet_Param("MNT_NO2", SqlDbType.NVarChar, dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO2").ToString)) '保守管理番号
                End With

                dstOrders_2 = clsDataConnect.pfGet_DataSet(cmdDB)


                '***** ドロップダウンリスト表示データ取得 *****
                cmdDB = New SqlCommand(sCnsSqlid_S5, conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("prm_errmsg", SqlDbType.NVarChar, 4000, ParameterDirection.Output))       '
                    .Add(pfSet_Param("sys_code", SqlDbType.NVarChar, dstOrders_1.Tables(0).Rows(0).Item("D73_TBOX_TYPE").ToString))       '

                End With

                dstOrders_3 = clsDataConnect.pfGet_DataSet(cmdDB)

                Select Case dstOrders_3.Tables.Count
                    Case 9

                        '正常に取得

                    Case Else

                        '異常取得
                        psMesBox(Me, "30001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                        Me.grvList.DataBind()
                        Return False
                        Exit Function

                End Select
            Else

                '***** ドロップダウンリスト表示データ取得 *****
                cmdDB = New SqlCommand(sCnsSqlid_S5, conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("prm_errmsg", SqlDbType.NVarChar, 4000, ParameterDirection.Output))       '
                    .Add(pfSet_Param("sys_code", SqlDbType.NVarChar, ""))       '

                End With

                dstOrders_3 = clsDataConnect.pfGet_DataSet(cmdDB)
            End If

        Catch ex As DBConcurrencyException
            '検索失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
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
            '検索失敗
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
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
            If Not conDB Is Nothing Then
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        End Try

        Return True

    End Function

    ''' <summary>
    ''' TBOX情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSetTboxData(ByVal strTboxId As String) As Boolean
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'DB接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                '値が空白の場合はエラー
                If strTboxId = "" Then
                    Throw New Exception()
                End If

                'ストアド設定
                cmdDB = New SqlCommand("REQSELP001_S7")
                cmdDB.Connection = conDB
                cmdDB.CommandType = CommandType.StoredProcedure

                'パラメータ設定
                With cmdDB.Parameters
                    .Add(pfSet_Param("@prmT03_TBOXID", SqlDbType.NVarChar, strTboxId))              'TBOXID
                End With

                'SQL実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(cmdDB)

                '取得エラー？
                If objDs Is Nothing OrElse objDs.Tables.Count <= 0 Then
                    Throw New Exception()
                End If

                'データが存在しない
                If objDs.Tables(0).Rows.Count <= 0 Then
                    Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                    Me.txtTboxId.ppTextBox.Focus()

                    'TBOX情報の初期化
                    msInitTBoxData()
                    Me.ddlTboxType.SelectedIndex = -1
                    Return True
                End If

                'TBOX情報設定
                msSetTboxInfo(objDs.Tables(0).Rows(0))

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
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Return False
        End If

    End Function

#End Region

#Region "印刷処理"

    ''' <summary>
    ''' 印刷処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_Print()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand

        Dim strKey() As String = Nothing

        Dim dtPrint As New DataTable                     '帳票用データセット
        Dim dtRow As DataRow = dtPrint.NewRow()          'データテーブルの行定義
        Dim dstOrders1 As New DataSet                    '表示情報のデータセット
        Dim strView() As String = Nothing                'ビューステート登録用の情報
        Dim rpt As New REQREP002
        Dim strFNm As String = "トラブル処理票_" & Date.Today.ToString("yyyyMMddHHmmss")  '帳票名称
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        Try
            'ビューステート項目取得
            strKey = ViewState(P_KEY)

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return False
            End If

            '***** トトラブル処理票印刷データ取得 *****

            'パラメータ設定
            cmdDB = New SqlCommand(sCnsSqlid_S3, conDB)

            With cmdDB.Parameters
                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, Me.lblTrblno.Text))              'トラブル管理番号
            End With

            dstOrders1 = clsDataConnect.pfGet_DataSet(cmdDB)

            'Active Reports(帳票)の起動

            psPrintPDF(Me, rpt, dstOrders1.Tables(0), strFNm)

        Catch ex As SqlException

            '帳票情報の取得に失敗
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
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

            'システムエラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "トラブル処理票")
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
            If Not conDB Is Nothing Then
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End If
        End Try

        Return True

    End Function

#End Region

#Region "編集表示処理"

    ''' <summary>
    ''' 編集表示処理
    ''' </summary>
    ''' <param name="dstOrders_2"></param>
    ''' <remarks></remarks>
    Private Function msGet_Edit(ByRef dstOrders_1 As DataSet _
                               , ByRef dstOrders_2 As DataSet _
                               , ByRef dstOrders_3 As DataSet)
        Dim strKey() As String = Nothing

        'ドロップダウンリストの設定
        'ＴＢＯＸタイプ
        Me.ddlTboxType.DataSource = dstOrders_3.Tables(7)
        Me.ddlTboxType.DataValueField = dstOrders_3.Tables(7).Columns(1).ColumnName.ToString
        Me.ddlTboxType.DataTextField = dstOrders_3.Tables(7).Columns(0).ColumnName.ToString
        Me.ddlTboxType.DataBind()
        '一行目を設定
        Me.ddlTboxType.Items.Insert(0, "")
        Me.ddlTboxType.SelectedIndex = 0

        '申告元
        Me.ddlRptBase.DataSource = dstOrders_3.Tables(0)
        Me.ddlRptBase.DataValueField = dstOrders_3.Tables(0).Columns(1).ColumnName.ToString
        Me.ddlRptBase.DataTextField = dstOrders_3.Tables(0).Columns(0).ColumnName.ToString
        Me.ddlRptBase.DataBind()
        '一行目を設定
        Me.ddlRptBase.Items.Insert(0, "")
        Me.ddlRptBase.SelectedIndex = 0

        '申告内容
        Me.ddlRptCd.DataSource = dstOrders_3.Tables(1)
        Me.ddlRptCd.DataValueField = dstOrders_3.Tables(1).Columns(1).ColumnName.ToString
        Me.ddlRptCd.DataTextField = dstOrders_3.Tables(1).Columns(0).ColumnName.ToString
        Me.ddlRptCd.DataBind()
        '一行目を設定
        Me.ddlRptCd.Items.Insert(0, "")
        Me.ddlRptCd.SelectedIndex = 0

        '作業状況リスト
        Me.ddlStatusCd.DataSource = dstOrders_3.Tables(2)
        Me.ddlStatusCd.DataValueField = dstOrders_3.Tables(2).Columns(1).ColumnName.ToString
        Me.ddlStatusCd.DataTextField = dstOrders_3.Tables(2).Columns(0).ColumnName.ToString
        Me.ddlStatusCd.DataBind()
        '一行目を設定
        Me.ddlStatusCd.Items.Insert(0, "")
        Me.ddlStatusCd.SelectedIndex = 0

        '回復内容
        Me.ddlRepairCd.DataSource = dstOrders_3.Tables(3)
        Me.ddlRepairCd.DataValueField = dstOrders_3.Tables(3).Columns(1).ColumnName.ToString
        Me.ddlRepairCd.DataTextField = dstOrders_3.Tables(3).Columns(0).ColumnName.ToString
        Me.ddlRepairCd.DataBind()
        '一行目を設定
        Me.ddlRepairCd.Items.Insert(0, "")
        Me.ddlRepairCd.SelectedIndex = 0

        ''確認者リスト
        'Me.ddlConfUsr.DataSource = dstOrders_3.Tables(4)
        'Me.ddlConfUsr.DataValueField = dstOrders_3.Tables(4).Columns(1).ColumnName.ToString
        'Me.ddlConfUsr.DataTextField = dstOrders_3.Tables(4).Columns(0).ColumnName.ToString
        'Me.ddlConfUsr.DataBind()
        ''一行目を設定
        'Me.ddlConfUsr.Items.Insert(0, "")
        'Me.ddlConfUsr.SelectedIndex = 0

        '承認者リスト
        Me.ddlAddUsr.DataSource = dstOrders_3.Tables(5)
        Me.ddlAddUsr.DataValueField = dstOrders_3.Tables(5).Columns(1).ColumnName.ToString
        Me.ddlAddUsr.DataTextField = dstOrders_3.Tables(5).Columns(0).ColumnName.ToString
        Me.ddlAddUsr.DataBind()
        '一行目を設定
        Me.ddlAddUsr.Items.Insert(0, "")
        Me.ddlAddUsr.SelectedIndex = 0

        '装置区分リスト
        Me.ddlEqCls.DataSource = dstOrders_3.Tables(8)
        Me.ddlEqCls.DataValueField = dstOrders_3.Tables(8).Columns(1).ColumnName.ToString
        Me.ddlEqCls.DataTextField = dstOrders_3.Tables(8).Columns(0).ColumnName.ToString
        Me.ddlEqCls.DataBind()
        '一行目を設定
        Me.ddlEqCls.Items.Insert(0, "")
        Me.ddlEqCls.SelectedIndex = 0

        Call ddlEqCls_SelectedIndexChanged(Nothing, Nothing)

        If Not ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.登録 Then

            'データ取得およびデータをリストに設定
            If dstOrders_2 Is Nothing Then
                Me.grvList.DataSource = Nothing
            Else
                Me.grvList.DataSource = dstOrders_2.Tables(0)
            End If

            'トラブル項目
            If dstOrders_1 Is Nothing Then
                'ビューステート項目取得
                strKey = ViewState(P_KEY)
                If strKey IsNot String.Empty Then
                    Me.lblIraino.Text = strKey(0)       '保守管理番号
                    Me.txtTboxId.ppText = strKey(1)     'ＴＢＯＸＩＤ
                End If
                Return True
            Else
                Me.txtTboxId.ppText = dstOrders_1.Tables(0).Rows(0)("D73_TBOXID").ToString                           'ＴＢＯＸＩＤ
                Me.ddlTboxType.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_TBOX_TYPE").ToString               'ＴＢＯＸタイプ
                ViewState("SYSTEM_CLS") = dstOrders_1.Tables(0).Rows(0)("T03_SYSTEM_CLS").ToString   'REQSELP001-002 システムクラス
                '--------------------------------
                '2015/02/05 加賀　ここから
                '--------------------------------
                Me.lblTboxType.Text = dstOrders_1.Tables(0).Rows(0)("M23_TBOXCLS_NM").ToString                       'システム
                '--------------------------------
                '2015/02/05 加賀　ここまで
                '--------------------------------
                Me.ddlHpnCls.ppSelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_HPN_CLS").ToString                 '発生区分
                Me.ddlMngCls.ppSelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_MNG_CLS").ToString                 '管理区分
                Me.lblTrblno.Text = dstOrders_1.Tables(0).Rows(0)("D73_TRBL_NO").ToString                            '管理番号
                Me.lblIraino.Text = dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO").ToString                             '依頼書番号（保守管理番号）
                Me.lblIraino2.Text = dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO2").ToString                           '依頼書番号（保守管理番号）
                Me.lblVer.Text = dstOrders_1.Tables(0).Rows(0)("D73_TBOX_VER").ToString                              'ＶＥＲ
                Me.hdnSysCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_SYSTEM_CD").ToString                          'システムコード
                Me.lblNlcls.Text = dstOrders_1.Tables(0).Rows(0)("NLNAME").ToString                                  'ＮＬ区分
                Me.hdnNlclsCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_NL_CLS").ToString                           'ＮＬ区分
                Me.lblEwcls.Text = dstOrders_1.Tables(0).Rows(0)("EWNAME").ToString                                  'ＥＷ区分
                Me.hdnEwclsCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_EW_CLS").ToString                           'ＥＷ区分
                Me.lblHallNm.Text = dstOrders_1.Tables(0).Rows(0)("D73_HALL_NM").ToString                            'ホール名
                Me.hdnHallCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_HALL_CD").ToString                           'ホールコード
                Me.lblAddr1.Text = dstOrders_1.Tables(0).Rows(0)("D73_ADDR").ToString                                '住所
                'Me.lblAddr2.Text = dstOrders_1.Tables(0).Rows(0)("D73_ADDR").ToString                               '住所
                Me.lblTel.Text = dstOrders_1.Tables(0).Rows(0)("D73_TELNO").ToString                                 'ＴＥＬ
                Me.lblWrkCharge.Text = dstOrders_1.Tables(0).Rows(0)("T03_MNT_NM").ToString                          '保担名
                Me.hdnEigyoCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_BRANCH_CD").ToString                        '営業所コード
                Me.lblTokatuCharge.Text = dstOrders_1.Tables(0).Rows(0)("T03_UNF_NM").ToString                       '総括保担名
                Me.hdnTokatuCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_UNF_CD").ToString                          '総括コード
                If dstOrders_1.Tables(0).Rows(0)("D73_MDN_SET_DVS").ToString = "0" Then                              '双子店区分
                    lblTwin.Text = "単独店"
                    hdnTwinCd.Value = "0"
                ElseIf dstOrders_1.Tables(0).Rows(0)("D73_MDN_SET_DVS").ToString = "1" Then
                    lblTwin.Text = "双子店"
                    hdnTwinCd.Value = "1"
                Else
                    lblTwin.Text = ""
                    hdnTwinCd.Value = ""
                End If
                Me.lblAgc.Text = dstOrders_1.Tables(0).Rows(0)("D73_AGC_NAM").ToString                               '代理店名
                Me.hdnAgcCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_AGC_COD").ToString                            '代理店コード
                Me.hdnAgcZip.Value = dstOrders_1.Tables(0).Rows(0)("D73_AGC_ZIP").ToString                           '代理店郵便番号
                Me.hdnAgcAddr.Value = dstOrders_1.Tables(0).Rows(0)("D73_AGC_ADS").ToString                          '代理店住所
                Me.hdnAgcTel.Value = dstOrders_1.Tables(0).Rows(0)("D73_AGC_TEL").ToString                           '代理店ＴＥＬ
                Me.hdnAgcFax.Value = dstOrders_1.Tables(0).Rows(0)("D73_AGC_FAX").ToString                           '代理店ＦＡＸ

                'REQSELP001-003
                Me.lblRep.Text = dstOrders_1.Tables(0).Rows(0)("D73_PER_NAM").ToString                               '代行店名
                Me.hdnRepCd.Value = dstOrders_1.Tables(0).Rows(0)("D73_PER_COD").ToString                            '代行店コード
                Me.hdnRepZip.Value = dstOrders_1.Tables(0).Rows(0)("D73_PER_ZIP").ToString                           '代行店郵便番号
                Me.hdnRepAddr.Value = dstOrders_1.Tables(0).Rows(0)("D73_PER_ADS").ToString                          '代行店住所
                Me.hdnRepTel.Value = dstOrders_1.Tables(0).Rows(0)("D73_PER_TEL").ToString                           '代行店ＴＥＬ
                Me.hdnRepChg.Value = dstOrders_1.Tables(0).Rows(0)("D73_PER_STF_COD").ToString                       '代行店担当者
                If dstOrders_1.Tables(0).Rows(0)("代行店表示フラグ") = "1" Then
                    lblRep.Visible = False
                Else
                    lblRep.Visible = True
                End If
                'REQSELP001-003 END

                Me.lblNgcOrg.Text = dstOrders_1.Tables(0).Rows(0)("D73_NGC_BNS").ToString                            'ＮＧＣ担当営業部
                Me.lblOrgTel.Text = dstOrders_1.Tables(0).Rows(0)("D73_NGC_TEL").ToString                            '担当営業部ＴＥＬ
                Me.lblEst.Text = dstOrders_1.Tables(0).Rows(0)("EST_CLS").ToString                                   'ＭＤＮ設置有無
                Me.hdnEstCls.Value = dstOrders_1.Tables(0).Rows(0)("D73_MDN_DVS").ToString                           'ＭＤＮ設置有無
                Me.lblMdn.Text = dstOrders_1.Tables(0).Rows(0)("MDN").ToString                                       'ＭＤＮ機器名
                Me.hdnMdnCnt.Value = dstOrders_1.Tables(0).Rows(0)("D73_MDN_SUU").ToString                           'ＭＤＮ台数
                Me.hdnMdnCd1.Value = dstOrders_1.Tables(0).Rows(0)("D73_MDN_COD1").ToString                          'ＭＤＮ機器コード１
                Me.hdnMdnCd2.Value = dstOrders_1.Tables(0).Rows(0)("D73_MDN_COD2").ToString                          'ＭＤＮ機器コード２
                Me.hdnMdnCd3.Value = dstOrders_1.Tables(0).Rows(0)("D73_MDN_COD3").ToString                          'ＭＤＮ機器コード３
                If dstOrders_1.Tables(0).Rows(0)("D73_RPT_DT").ToString.Length >= 18 Then
                    Me.dttRptDt.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RPT_DT").ToString.Substring(0, 10)       '申告日
                Else
                    Me.dttRptDt.ppText = String.Empty
                End If

                If dstOrders_1.Tables(0).Rows(0)("D73_RCPT_DT").ToString.Length >= 18 Then
                    '--------------------------------
                    '2014/05/26 武　ここから
                    '--------------------------------
                    Me.dttRcptDt.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RCPT_DT").ToString.Substring(0, 10)      '受付日時
                    Dim dtVal As DateTime = Nothing
                    If DateTime.TryParse(dstOrders_1.Tables(0).Rows(0)("D73_RCPT_DT"), dtVal) Then
                        Me.dttRcptDt.ppHourText = dtVal.ToString("HH")                                                '受付日時
                        Me.dttRcptDt.ppMinText = dtVal.ToString("mm")                                                 '受付日時
                    End If
                    'Me.dttRcptDt.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RCPT_DT").ToString.Substring(0, 10)     '受付日時
                    'Me.dttRcptDt.ppHourText = dstOrders_1.Tables(0).Rows(0)("D73_RCPT_DT").ToString.Substring(11, 2) '受付日時
                    'Me.dttRcptDt.ppMinText = dstOrders_1.Tables(0).Rows(0)("D73_RCPT_DT").ToString.Substring(14, 2)  '受付日時
                Else
                    '--------------------------------
                    '2014/05/26 武　ここまで
                    '--------------------------------
                    Me.dttRcptDt.ppText = String.Empty
                    Me.dttRcptDt.ppText = String.Empty
                End If
                Me.tetRpt.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RPT_CHARGE").ToString                          '申告者
                Me.ddlRptBase.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_RPT_BASE").ToString                 '申告元
                Me.tetRptTel.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RPT_TEL").ToString                          '申告者ＴＥＬ

                Me.tetRptCharg.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RCPT_CHARGE").ToString                    '受付者
                Me.ddlRptCd.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_RPT_CD").ToString                     '申告内容
                Me.tetRptDtl1.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RPT_DTL1").ToString                        '申告内容
                Me.tetRptDtl2.ppText = dstOrders_1.Tables(0).Rows(0)("D73_RPT_DTL2").ToString                        '申告内容
                If dstOrders_1.Tables(0).Rows(0)("D73_IMP_CLS") = "1" Then                                           '故障重要度
                    Me.chkImpCls.Checked = True
                Else
                    Me.chkImpCls.Checked = False
                End If

                Me.tetInhCntnt1.ppText = dstOrders_1.Tables(0).Rows(0)("D73_INH_CNTNT1").ToString                    '引継内容
                Me.tetInhCntnt2.ppText = dstOrders_1.Tables(0).Rows(0)("D73_INH_CNTNT2").ToString
                If dstOrders_1.Tables(0).Rows(0)("D73_BSNSDIST_CLS").ToString = "1" Then                                      '引継区分 営業支障区分
                    Me.CheckBoxList1.Items(0).Selected = True
                Else
                    Me.CheckBoxList1.Items(0).Selected = False
                End If
                If dstOrders_1.Tables(0).Rows(0)("D73_SCNDDIST_CLS").ToString = "1" Then                                      '引継区分 ２次支障区分
                    Me.CheckBoxList1.Items(1).Selected = True
                Else
                    Me.CheckBoxList1.Items(1).Selected = False
                End If
                Me.ddlDealreqCd1.ppSelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_DEALREQ_CD1").ToString         '対応依頼１
                Me.ddlDealreqCd2.ppSelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_DEALREQ_CD2").ToString         '対応依頼２
                Me.ddlDealreqCd3.ppSelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_DEALREQ_CD3").ToString         '対応依頼３
                Me.ddlEqCls.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_EQ_CLS").ToString                   '装置区分
                Call ddlEqCls_SelectedIndexChanged(Nothing, Nothing)
                Me.ddlEqCd.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_EQ_CD").ToString                     '装置詳細
                Me.ddlStatusCd.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_STATUS_CD").ToString               '作業状況
                If Session(P_SESSION_AUTH) <> "管理者" Then
                    If Me.ddlStatusCd.SelectedValue = "12" Or Me.ddlStatusCd.SelectedValue = "13" Or Me.ddlStatusCd.SelectedValue = "22" Then
                        ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                        Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照
                        '非活性化処理
                        msEdit_Enabled("1")
                        'ボタン制御
                        msChange_btn("参照")
                    Else
                        For intState As Integer = 0 To ddlStatusCd.Items.Count - 1
                            If Me.ddlStatusCd.Items(intState).Value = "12" Then
                                Me.ddlStatusCd.Items.RemoveAt(intState)
                                Exit For
                            End If
                        Next
                        For intState As Integer = 0 To ddlStatusCd.Items.Count - 1
                            If Me.ddlStatusCd.Items(intState).Value = "13" Then
                                Me.ddlStatusCd.Items.RemoveAt(intState)
                                Exit For
                            End If
                        Next
                        For intState As Integer = 0 To ddlStatusCd.Items.Count - 1
                            If Me.ddlStatusCd.Items(intState).Value = "22" Then
                                Me.ddlStatusCd.Items.RemoveAt(intState)
                                Exit For
                            End If
                        Next
                    End If
                End If

                If dstOrders_1.Tables(0).Rows(0)("D73_STATUS_CD").ToString = M_WORK_STSCD Then
                    Me.txtSttsNotetext.ppText = dstOrders_1.Tables(0).Rows(0)("D73_STTS_NOTETEXT").ToString
                    Me.txtSttsNotetext.ppEnabled = True
                Else
                    Me.txtSttsNotetext.ppText = String.Empty
                    Me.txtSttsNotetext.ppEnabled = False
                End If
                Me.ddlRepairCd.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_REPAIR_CD").ToString               '回復内容
                Me.tetRepairCcntnt.ppText = dstOrders_1.Tables(0).Rows(0)("D73_REPAIR_CNTNT").ToString               '回復内容
                If dstOrders_1.Tables(0).Rows(0)("D73_CONF_DT").ToString.Length >= 18 Then
                    Me.lblConfDt.Text = dstOrders_1.Tables(0).Rows(0)("D73_CONF_DT").ToString.Substring(0, 10)       '確認日
                Else
                    Me.lblConfDt.Text = String.Empty
                End If
                Me.txtConfUsr.ppText = dstOrders_1.Tables(0).Rows(0)("D73_CONF_USR").ToString
                'Me.ddlConfUsr.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_CONFUSR_CD").ToString               '確認者
                If dstOrders_1.Tables(0).Rows(0)("D73_APP_DT").ToString.Length >= 18 Then
                    Me.lblAppDt.Text = dstOrders_1.Tables(0).Rows(0)("D73_APP_DT").ToString.Substring(0, 10)         '承認日
                Else
                    Me.lblAppDt.Text = String.Empty
                End If
                Me.ddlAddUsr.SelectedValue = dstOrders_1.Tables(0).Rows(0)("D73_APPUSR_CD").ToString                 '承認者
                ViewState("dstOrders_1") = dstOrders_1
                '--------------------------------
                '2014/05/29 松川　ここから
                '--------------------------------
                'If dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO").ToString = String.Empty _
                '    And ViewState(P_SESSION_TERMS) =  ClsComVer.E_遷移条件.更新 Then
                '    Master.ppRigthButton6.Enabled = True           '保守対応依頼書
                'Else
                '    Master.ppRigthButton6.Enabled = False          '保守対応依頼書
                'End If
                If dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO").ToString = String.Empty Then
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                        Master.ppRigthButton6.Enabled = False      '保守対応依頼書
                    Else
                        Master.ppRigthButton6.Enabled = True       '保守対応依頼書
                    End If
                Else
                    Master.ppRigthButton6.Enabled = True           '保守対応依頼書
                End If

                If dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO2").ToString = String.Empty Then
                    If ViewState(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照 Then
                        Master.ppRigthButton5.Enabled = False      '保守対応依頼書
                        Master.ppRigthButton5.Visible = False
                    Else
                        If dstOrders_1.Tables(0).Rows(0)("D73_MNT_NO").ToString = String.Empty Then
                            Master.ppRigthButton5.Enabled = False      '保守対応依頼書
                            Master.ppRigthButton5.Visible = False
                        Else
                            Master.ppRigthButton5.Enabled = True       '保守対応依頼書
                            Master.ppRigthButton5.Visible = True
                        End If
                    End If
                Else
                    Master.ppRigthButton5.Enabled = True           '保守対応依頼書
                    Master.ppRigthButton5.Visible = True
                End If
                'If Me.ViewState("遷移元") = "保守" Then
                '    Master.ppRigthButton6.Enabled = False      '保守対応依頼書
                'End If
                '--------------------------------
                '2014/05/29 松川　ここまで
                '--------------------------------
            End If
        Else
            Me.grvList.DataSource = Nothing
        End If

        '変更を反映
        Me.grvList.DataBind()

        Return True

    End Function

#End Region

#Region "TBOX情報設定"

    ''' <summary>
    ''' TBOX情報設定
    ''' </summary>
    ''' <param name="objRow"></param>
    ''' <remarks></remarks>
    Private Sub msSetTboxInfo(ByVal objRow As DataRow)
        txtTboxId.ppText = objRow("ＴＢＯＸＩＤ")
        If Me.txtTboxId.ppText = "99999999" Or Me.txtTboxId.ppText = "99999989" Then
            Me.ddlTboxType.SelectedIndex = -1
            Me.lblNlcls.Text = ""
        Else
            Me.ddlTboxType.SelectedValue = objRow("ＴＢＯＸタイプ")
            Me.lblNlcls.Text = objRow("ＮＬ区分")
        End If

        '--------------------------------
        '2015/02/05 加賀　ここから
        '--------------------------------
        lblTboxType.Text = objRow("ＴＢＯＸタイプ名")
        '--------------------------------
        '2015/02/05 加賀　ここまで
        '--------------------------------
        ViewState("SYSTEM_CLS") = objRow("システムクラス") 'REQSELP001-002
        lblVer.Text = objRow("Ｖｅｒ")
        hdnSysCd.Value = objRow("ＴＢＯＸタイプ")
        hdnNlclsCd.Value = objRow("ＮＬ区分ＣＤ")
        lblEwcls.Text = objRow("ＥＷ区分")
        hdnEwclsCd.Value = objRow("ＥＷ区分ＣＤ")
        hdnHallCd.Value = objRow("ホールＣＤ")
        lblHallNm.Text = objRow("ホール名")
        lblAddr1.Text = objRow("ホール住所")
        lblTel.Text = objRow("ホールＴＥＬ")
        lblWrkCharge.Text = objRow("保担名")
        hdnEigyoCd.Value = objRow("営業所ＣＤ")
        lblTokatuCharge.Text = objRow("統括名")
        hdnTokatuCd.Value = objRow("統括名ＣＤ")
        If objRow("双子店区分") = "0" Then
            lblTwin.Text = "単独店"
            hdnTwinCd.Value = "0"
        ElseIf objRow("双子店区分") = "1" Then
            lblTwin.Text = "双子店"
            hdnTwinCd.Value = "1"
        Else
            lblTwin.Text = ""
            hdnTwinCd.Value = ""
        End If
        lblAgc.Text = objRow("代理店名")
        hdnAgcCd.Value = objRow("代理店ＣＤ")
        hdnAgcZip.Value = objRow("代理店郵便番号")
        hdnAgcAddr.Value = objRow("代理店住所")
        hdnAgcTel.Value = objRow("代理店ＴＥＬ")
        hdnAgcFax.Value = objRow("代理店ＦＡＸ")
        'REQSELP001-003
        If objRow("代行店表示フラグ") = "1" Then
            lblRep.Visible = False
        Else
            lblRep.Visible = True
        End If
        'REQSELP001-003 END
        lblRep.Text = objRow("代行店名")
        hdnRepCd.Value = objRow("代行店ＣＤ")
        hdnRepZip.Value = objRow("代行店郵便番号")
        hdnRepAddr.Value = objRow("代行店住所")
        hdnRepTel.Value = objRow("代行店ＴＥＬ")
        hdnRepChg.Value = objRow("代行店担当者")
        lblNgcOrg.Text = objRow("ＮＧＣ担当営業部")
        lblOrgTel.Text = objRow("担当営業部ＴＥＬ")
        lblEst.Text = objRow("ＭＤＮ設置有無")
        hdnEstCls.Value = objRow("ＭＤＮ設置有無ＣＤ")
        lblMdn.Text = objRow("ＭＤＮ機器名")
        hdnMdnCnt.Value = objRow("ＭＤＮ台数")
        hdnMdnCd1.Value = objRow("ＭＤＮＣＤ１")
        hdnMdnCd2.Value = objRow("ＭＤＮＣＤ２")
        hdnMdnCd3.Value = objRow("ＭＤＮＣＤ３")

    End Sub

#End Region

#Region " 登録・更新・削除処理"

    ''' <summary>
    ''' 登録・更新・削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_Data(ByVal strShoriKbn As Integer)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim trans As SqlClient.SqlTransaction             'トランザクション
        Dim inParam(0) As SqlParameter
        Dim strKey() As String = Nothing

        Dim dstT01_HALL As New DataSet
        Dim dstOrders As New DataSet

        Dim strCNST_CLS_Chk As StringBuilder = New StringBuilder
        Dim strTRBL_NO As StringBuilder = New StringBuilder
        Dim strNo As String = String.Empty

        Dim strNL As String = "N"
        Dim strSysCls As String = "1"

        Dim msgNo As String = Nothing
        Dim msg As String = Nothing

        Dim strExclusiveDate As String = Nothing
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If (Page.IsValid) Then

            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) Then
                'トランザクション
                cmdDB.Connection = conDB
            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return False
                Exit Function
            End If

            'トランザクションの設定
            trans = conDB.BeginTransaction

            Try

                'ビューステート項目取得
                strKey = ViewState(P_KEY)

                Select Case strShoriKbn
                    Case 1  'トラブル 登録
                        msgNo = "00003"
                        msg = "トラブル処理票"



                        ''パラメータ設定
                        'cmdDB = New SqlCommand("ZCMPSEL022", conDB)
                        'cmdDB.Transaction = trans
                        'With cmdDB.Parameters
                        '    'パラメータ設定
                        '    .Add(pfSet_Param("MNGNO_CLS", SqlDbType.Int, 1))                                    '管理番号
                        '    .Add(pfSet_Param("YMD", SqlDbType.NVarChar, Date.Now.ToString("yyyyMMdd")))         '年月日
                        '    .Add(pfSet_Param("SalesYTD", SqlDbType.Int, 20, ParameterDirection.Output))         '結果取得用
                        'End With

                        ''リストデータ取得
                        'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        ''管理番号取得失敗
                        'If cmdDB.Parameters("SalesYTD").Value Is Nothing Then

                        '    Throw New Exception

                        'End If

                        'strNo = cmdDB.Parameters("SalesYTD").Value



                        'パラメータ設定
                        cmdDB = New SqlCommand(sCnsSqlid_S98, conDB)
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters                                                               'ホール情報取得
                            .Add(pfSet_Param("TBOXID", SqlDbType.NVarChar, txtTboxId.ppText))               'ＴＢＯＸＩＤ
                        End With

                        dstT01_HALL = clsDataConnect.pfGet_DataSet(cmdDB)

                        If dstT01_HALL.Tables(0).Rows.Count = 0 Then
                            Me.txtTboxId.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                            Me.txtTboxId.ppTextBox.Focus()
                            Return False
                        End If

                        If dstT01_HALL.Tables(0).Rows(0).Item("T03_WRKEND_DT").ToString.Trim <> Nothing Then
                            If Date.Parse(dstT01_HALL.Tables(0).Rows(0).Item("T03_WRKEND_DT").ToString.Trim) < DateTime.Now Then
                                Me.txtTboxId.psSet_ErrorNo("2016", "ホール", "登録")
                                Me.txtTboxId.ppTextBox.Focus()
                                Return False
                            End If
                        End If

                        ''ホール情報.ＮＬ区分
                        'If dstT01_HALL.Tables(0).Rows(0).Item("T01_NL_CLS").ToString.Trim = strNL Or dstT01_HALL.Tables(0).Rows(0).Item("T01_NL_CLS").ToString.Trim = "J" Then
                        '    strTRBL_NO.Append("G")
                        'Else
                        '    strTRBL_NO.Append("L")
                        'End If
                        ''ホール情報.システム分類
                        'If dstT01_HALL.Tables(0).Rows(0).Item("T01_SYSTEM_CLS").ToString.Trim = strSysCls Then
                        '    strTRBL_NO.Append("C")
                        'Else
                        '    strTRBL_NO.Append("I")
                        'End If

                        '管理番号取得
                        Dim wareki As String
                        Dim ci As New System.Globalization.CultureInfo("ja-JP", False)
                        ci.DateTimeFormat.Calendar = New System.Globalization.JapaneseCalendar()
                        If DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") > "2019/05/01 00:00:00" Then
                            wareki = (Integer.Parse(Date.Now.ToString("yyyy").Substring(2, 2)) - 18).ToString("00")
                            wareki = wareki.Substring(wareki.Length - 2)
                        Else
                            wareki = Date.Now.ToString("ggyy", ci).Substring(2, 2)
                            wareki = wareki.Substring(wareki.Length - 2)
                        End If


                        cmdDB = New SqlCommand("ZCMPSEL050", conDB)
                        cmdDB.Transaction = trans
                        'パラメータ設定
                        With cmdDB.Parameters
                            .Add(pfSet_Param("nlcls", SqlDbType.NVarChar, dstT01_HALL.Tables(0).Rows(0).Item("T01_NL_CLS").ToString.Trim))      'ＮＬ区分
                            .Add(pfSet_Param("syscls", SqlDbType.NVarChar, dstT01_HALL.Tables(0).Rows(0).Item("T01_SYSTEM_CLS").ToString.Trim)) 'システム分類
                            .Add(pfSet_Param("YYwareki", SqlDbType.NVarChar, wareki))                       '和暦YY
                            .Add(pfSet_Param("MM", SqlDbType.NVarChar, Date.Now.ToString("MM")))            '月
                            .Add(pfSet_Param("usernm", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      '社員名
                            .Add(pfSet_Param("mntno", SqlDbType.NVarChar, 20, ParameterDirection.Output))   'トラブル管理番号
                        End With
                        'リストデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        '管理番号取得失敗
                        If cmdDB.Parameters("mntno").Value Is Nothing Then
                            Throw New Exception
                        End If
                        ViewState("TRBL_NO") = cmdDB.Parameters("mntno").Value

                        ''西暦を和暦にする
                        'Dim ci As CultureInfo = New CultureInfo("ja-JP", True)
                        'ci.DateTimeFormat.Calendar = New JapaneseCalendar
                        'strTRBL_NO.Append(DateTime.Now.ToString("yyMM", ci))
                        'strTRBL_NO.Append("-")
                        'strTRBL_NO.Append(CInt(strNo).ToString("0000"))
                        'ViewState("TRBL_NO") = strTRBL_NO.ToString

                        'トラブル編集処理
                        Me.msSet_D73_TROUBLE_Edit(REQSELP001_D73_TROUBLE, dstT01_HALL)

                        'パラメータ設定（トラブル登録）
                        cmdDB = New SqlCommand(sCnsSqlid_I1, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        With cmdDB.Parameters
                            .Add(pfSet_Param("tvp", SqlDbType.Structured, REQSELP001_D73_TROUBLE.Tables(0)))    'トラブル処理票
                        End With
                        cmdDB.Transaction = trans

                        '実行
                        cmdDB.ExecuteNonQuery()

                        If Me.ViewState("遷移元") = "集信" Then

                            'ヘルスチェック(無応答)を更新する
                            cmdDB = New SqlCommand(sCnsSqlid_U6, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            With cmdDB.Parameters
                                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, ViewState("TRBL_NO")))          'トラブル管理番号
                                .Add(pfSet_Param("CTRL_NO", SqlDbType.NVarChar, strKey(1)))                     '管理番号
                                .Add(pfSet_Param("SEQ", SqlDbType.NVarChar, strKey(2)))                         '連番
                                .Add(pfSet_Param("NL_CLS", SqlDbType.NVarChar, strKey(3)))                      'ＮＬ区分
                                .Add(pfSet_Param("ID_IC_CLS", SqlDbType.NVarChar, strKey(4)))                   'ID_IC区分
                                .Add(pfSet_Param("RECVDATE", SqlDbType.NVarChar, strKey(5)))                    'データ受信日
                                .Add(pfSet_Param("RECVSEQ", SqlDbType.NVarChar, strKey(6)))                     '受信連番
                                .Add(pfSet_Param("UPDATE_DT", SqlDbType.DateTime, DateTime.Now))                '更新日時
                                .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))  '更新者
                            End With
                            cmdDB.Transaction = trans

                            '実行
                            cmdDB.ExecuteNonQuery()

                        End If

                        If Me.ViewState("遷移元") = "ヘルス" Then

                            'ヘルスチェックを更新する
                            cmdDB = New SqlCommand(sCnsSqlid_U7, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            With cmdDB.Parameters
                                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, ViewState("TRBL_NO")))          'トラブル管理番号
                                .Add(pfSet_Param("CTRL_NO", SqlDbType.NVarChar, strKey(1)))                     '照会管理番号
                                .Add(pfSet_Param("SEQ", SqlDbType.NVarChar, strKey(2)))                         '連番
                                .Add(pfSet_Param("NL_CLS", SqlDbType.NVarChar, strKey(3)))                      'ＮＬ区分
                                .Add(pfSet_Param("ID_IC_CLS", SqlDbType.NVarChar, strKey(4)))                   'ID_IC区分
                                .Add(pfSet_Param("RECVDATE", SqlDbType.NVarChar, strKey(5)))                    'データ受信日
                                .Add(pfSet_Param("RECVSEQ", SqlDbType.NVarChar, strKey(6)))                     '受信連番
                                .Add(pfSet_Param("UPDATE_DT", SqlDbType.DateTime, DateTime.Now))                '更新日時
                                .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))  '更新者
                            End With
                            cmdDB.Transaction = trans

                            '実行
                            cmdDB.ExecuteNonQuery()

                        End If


                        arKey.Insert(0, ViewState("TRBL_NO"))        'D73_TRBL_NO

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
                                         , sCnsProgid _
                                         , arTable_Name _
                                         , arKey) = 0 Then

                            '★登録年月日時刻
                            Me.Master.ppExclusiveDate = strExclusiveDate
                        End If

                    Case 2  'トラブル 更新

                        msgNo = "00001"
                        msg = "トラブル処理票"

                        'トラブル編集処理
                        Me.msSet_D73_TROUBLE_Edit(REQSELP001_D73_TROUBLE, dstT01_HALL)

                        'パラメータ設定（トラブル更新）
                        cmdDB = New SqlCommand(sCnsSqlid_U1, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                         'トラブル管理番号
                            .Add(pfSet_Param("UPDATE_DT", SqlDbType.DateTime, DateTime.Now))                    '更新日時
                            .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      '更新者
                            .Add(pfSet_Param("tvp", SqlDbType.Structured, REQSELP001_D73_TROUBLE.Tables(0)))    'トラブル処理票
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                    Case 3  'トラブル 削除

                        msgNo = "00002"
                        msg = "トラブル処理票"

                        'パラメータ設定（トラブル削除）
                        cmdDB = New SqlCommand(sCnsSqlid_U2, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                         'トラブル管理番号
                            .Add(pfSet_Param("UPDATE_DT", SqlDbType.DateTime, DateTime.Now))                    '更新日時
                            .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      '更新者
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                        'パラメータ設定（トラブル明細削除）
                        cmdDB = New SqlCommand(sCnsSqlid_U4, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                         'トラブル管理番号
                            .Add(pfSet_Param("UPDATE_DT", SqlDbType.DateTime, DateTime.Now))                    '更新日時
                            .Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      '更新者
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()
                    Case 4  'トラブル明細 追加

                        msgNo = "00003"
                        msg = "トラブル処理票(明細)"

                        If lblTrblno.Text.Trim Is Nothing Then
                            Throw New Exception
                        End If

                        'パラメータ設定
                        cmdDB = New SqlCommand(sReqSqlid_S4, conDB)
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                         'トラブル管理番号
                            .Add(pfSet_Param("TRBL_SEQ", SqlDbType.NVarChar, 20, ParameterDirection.Output))    '明細連番の最大値
                        End With

                        'リストデータ取得
                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                        ViewState("TRBL_SEQ") = cmdDB.Parameters("TRBL_SEQ").Value.ToString

                        'トラブル編集処理
                        Me.msSet_D74_TROUBLE_DTIL_Edit(REQSELP001_D74_TROUBLE_DTIL, "登録")

                        'パラメータ設定（トラブル登録）
                        cmdDB = New SqlCommand(sCnsSqlid_I2, conDB)
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.Transaction = trans
                        With cmdDB.Parameters
                            .Add(pfSet_Param("tvp", SqlDbType.Structured, REQSELP001_D74_TROUBLE_DTIL.Tables(0)))    'トラブル処理票
                        End With

                        '実行
                        cmdDB.ExecuteNonQuery()

                    Case 5  'トラブル明細 更新

                        msgNo = "00001"
                        msg = "トラブル処理票(明細)"

                        'トラブル編集処理
                        Me.msSet_D74_TROUBLE_DTIL_Edit(REQSELP001_D74_TROUBLE_DTIL, "更新")

                        '保守対応依頼明細の場合
                        If ViewState("HANDANF") = "02" Then
                            '選択データの削除
                            cmdDB = New SqlCommand(sCnsSqlid_D2, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, lblIraino.Text.Trim))                            'トラブル管理番号
                                .Add(pfSet_Param("mnt_seq", SqlDbType.NVarChar, ViewState("TRBL_SEQ")))               '更新者
                            End With
                            '実行
                            cmdDB.ExecuteNonQuery()

                            '新しい連番の取得
                            cmdDB = New SqlCommand(sReqSqlid_S4, conDB)
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                         'トラブル管理番号
                                .Add(pfSet_Param("TRBL_SEQ", SqlDbType.NVarChar, 20, ParameterDirection.Output))    '明細連番の最大値
                            End With

                            'リストデータ取得
                            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                            ViewState("TRBL_SEQ") = cmdDB.Parameters("TRBL_SEQ").Value.ToString

                            'トラブル編集処理
                            Me.msSet_D74_TROUBLE_DTIL_Edit(REQSELP001_D74_TROUBLE_DTIL, "登録")

                            'パラメータ設定（トラブル登録）
                            cmdDB = New SqlCommand(sCnsSqlid_I2, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("tvp", SqlDbType.Structured, REQSELP001_D74_TROUBLE_DTIL.Tables(0)))    'トラブル処理票
                            End With
                        Else
                            'パラメータ設定（トラブル更新）
                            cmdDB = New SqlCommand(sCnsSqlid_U3, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                            'トラブル管理番号
                                .Add(pfSet_Param("TRBL_SEQ", SqlDbType.NVarChar, ViewState("TRBL_SEQ")))               '更新者
                                .Add(pfSet_Param("tvp", SqlDbType.Structured, REQSELP001_D74_TROUBLE_DTIL.Tables(0)))  'トラブル処理票
                            End With
                        End If


                        '実行
                        cmdDB.ExecuteNonQuery()

                        '選択フォーカス
                        txtDealCd.ppTextBox.Focus()

                    Case 6  'トラブル明細 削除

                        '--------------------------------
                        '2014/05/19 後藤　ここから
                        '--------------------------------
                        'msgNo = "00004"
                        msgNo = "00002"
                        msg = "トラブル処理票(明細)"
                        '--------------------------------
                        '2014/05/19 後藤　ここまで
                        '--------------------------------

                        '保守対応依頼明細の場合
                        If ViewState("HANDANF") = "02" Then
                            '選択データの削除
                            cmdDB = New SqlCommand(sCnsSqlid_D2, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, lblIraino.Text.Trim))                            'トラブル管理番号
                                .Add(pfSet_Param("mnt_seq", SqlDbType.NVarChar, ViewState("TRBL_SEQ")))               '更新者
                            End With
                            '実行
                            cmdDB.ExecuteNonQuery()
                        Else
                            'パラメータ設定（トラブル明細削除）
                            cmdDB = New SqlCommand(sCnsSqlid_D1, conDB)
                            cmdDB.CommandType = CommandType.StoredProcedure
                            cmdDB.Transaction = trans
                            With cmdDB.Parameters
                                .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))                         'トラブル管理番号
                                .Add(pfSet_Param("TRBL_SEQ", SqlDbType.NVarChar, ViewState("TRBL_SEQ")))            'トラブル管理順番号
                                '.Add(pfSet_Param("UPDATE_DT", SqlDbType.DateTime, DateTime.Now))                    '更新日時
                                '.Add(pfSet_Param("UPDATE_USR", SqlDbType.NVarChar, Session(P_SESSION_USERID)))      '更新者
                            End With

                            '実行
                            cmdDB.ExecuteNonQuery()
                        End If

                End Select

                'コミッテッド
                trans.Commit()

                '***** トラブル明細データ取得 *****

                'パラメータ設定
                If strShoriKbn <> 1 Then

                    cmdDB = New SqlCommand(sCnsSqlid_S2, conDB)

                    With cmdDB.Parameters
                        .Add(pfSet_Param("TRBL_NO", SqlDbType.NVarChar, lblTrblno.Text.Trim))            'トラブル管理番号
                        If Me.lblIraino.Text = String.Empty Then
                            .Add(pfSet_Param("MNT_NO", SqlDbType.NVarChar, DBNull.Value))      '保守管理番号
                        Else
                            .Add(pfSet_Param("MNT_NO", SqlDbType.NVarChar, Me.lblIraino.Text)) '保守管理番号
                        End If
                        If Me.lblIraino2.Text = String.Empty Then
                            .Add(pfSet_Param("MNT_NO2", SqlDbType.NVarChar, DBNull.Value))      '保守管理番号2
                        Else
                            .Add(pfSet_Param("MNT_NO2", SqlDbType.NVarChar, Me.lblIraino2.Text)) '保守管理番号2
                        End If
                    End With

                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                    If dstOrders.Tables(0).Rows.Count > 0 Then
                        Me.grvList.DataSource = dstOrders
                    Else
                        Me.grvList.DataSource = New DataTable
                    End If

                Else

                    Me.grvList.DataSource = New DataTable

                End If

                Me.grvList.DataBind()

                psMesBox(Me, msgNo, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画前, msg)

                Return True

            Catch ex As DBConcurrencyException
                'ロールバック
                trans.Rollback()
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                psMesBox(Me, msgNo, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, msg)
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
                psMesBox(Me, msgNo, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, msg)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                trans.Rollback()
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Throw
            Finally
                'DB切断
                If Not conDB Is Nothing Then
                    If Not clsDataConnect.pfClose_Database(conDB) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End If
            End Try

        End If

        Return True

    End Function

#End Region

#Region "トラブル編集処理"

    ''' <summary>
    ''' トラブル編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_D73_TROUBLE_Edit(ByRef REQSELP001_D73_TROUBLE As DataSet _
                                            , ByVal dstT01_HALL As DataSet)

        Dim dataset As New REQSELP001_D73_TROUBLE
        Dim dtRow As DataRow
        Dim strview_Terms_in As String = ViewState(P_SESSION_TERMS)
        dtRow = dataset.Tables(0).NewRow
        Dim strDateTime As StringBuilder = New StringBuilder
        Dim strSplit() As String = Nothing

        Try
            Select Case strview_Terms_in
                Case ClsComVer.E_遷移条件.登録

                    dtRow("D73_TRBL_NO") = ViewState("TRBL_NO")                                        'トラブル管理番号
                    dtRow("D73_MNG_CLS") = ""                                                          'トラブル管理区分
                    dtRow("D73_HPN_CLS") = ""                                                          '発生区分
                    dtRow("D73_HPN_DT") = DateTime.Now                                                 '発生日時
                    dtRow("D73_TBOXID") = Me.txtTboxId.ppText                                          'ＴＢＯＸＩＤ
                    dtRow("D73_TBOX_TYPE") = Me.ddlTboxType.SelectedValue.ToString                     'ＴＢＯＸタイプ
                    dtRow("D73_TBOX_VER") = Me.lblVer.Text                                             'ＴＢＯＸバージョン
                    dtRow("D73_HALL_CD") = Me.hdnHallCd.Value                                          'ホールコード
                    dtRow("D73_HALL_NM") = Me.lblHallNm.Text                                           'ホール名称
                    dtRow("D73_ADDR") = Me.lblAddr1.Text                                               'ホール住所
                    dtRow("D73_TELNO") = Me.lblTel.Text                                                'ホールＴＥＬ
                    dtRow("D73_EW_CLS") = Me.hdnEwclsCd.Value                                          'ＥＷ区分
                    dtRow("D73_NL_CLS") = Me.hdnNlclsCd.Value                                          'ＮＬ区分
                    dtRow("D73_UNF_CD") = Me.hdnTokatuCd.Value                                         '統括コード
                    dtRow("D73_BRANCH_CD") = Me.hdnEigyoCd.Value                                       '営業所コード
                    dtRow("D73_RPT_DT") = DBNull.Value                                                 '申告日
                    dtRow("D73_RPT_CHARGE") = DBNull.Value                                             '申告者
                    dtRow("D73_RPT_BASE") = DBNull.Value                                               '申告元コード
                    dtRow("D73_RPT_TEL") = DBNull.Value                                                '申告元ＴＥＬ
                    dtRow("D73_RPT_CD") = DBNull.Value                                                 '申告内容コード
                    dtRow("D73_RPT_DTL1") = DBNull.Value                                               '申告内容１
                    dtRow("D73_RPT_DTL2") = DBNull.Value                                               '申告内容２
                    dtRow("D73_RCPT_DT") = DBNull.Value                                                '受付日時
                    dtRow("D73_RCPT_CHARGE") = DBNull.Value                                            '受付者
                    dtRow("D73_INH_CNTNT1") = DBNull.Value                                             '引継内容１
                    dtRow("D73_INH_CNTNT2") = DBNull.Value                                             '引継内容２
                    dtRow("D73_DEALREQ_CD1") = DBNull.Value                                            '対応依頼コード１
                    dtRow("D73_DEALREQ_CD2") = DBNull.Value                                            '対応依頼コード２
                    dtRow("D73_DEALREQ_CD3") = DBNull.Value                                            '対応依頼コード３
                    dtRow("D73_EQ_CLS") = DBNull.Value                                                 '装置区分
                    dtRow("D73_EQ_CD") = DBNull.Value                                                  '装置詳細コード
                    dtRow("D73_MNT_NO") = DBNull.Value                                                 '保守管理番号
                    dtRow("D73_STATUS_CD") = DBNull.Value                                              '作業状況コード
                    dtRow("D73_STTS_NOTETEXT") = DBNull.Value                                          '作業状況備考
                    dtRow("D73_REPAIR_CD") = DBNull.Value                                              '回復内容コード
                    dtRow("D73_REPAIR_CNTNT") = DBNull.Value                                           '回復内容
                    dtRow("D73_IMP_CLS") = "0"                                                         '故障重要度区分
                    dtRow("D73_BSNSDIST_CLS") = "0"                                                    '営業支障区分
                    dtRow("D73_SCNDDIST_CLS") = "0"                                                    '２次支障区分
                    dtRow("D73_CONF_DT") = DBNull.Value                                                '確認日
                    dtRow("D73_CONFUSR_CD") = DBNull.Value                                             '確認者コード
                    dtRow("D73_CONF_USR") = DBNull.Value                                               '確認者名称
                    dtRow("D73_APP_DT") = DBNull.Value                                                 '承認日
                    dtRow("D73_APPUSR_CD") = DBNull.Value                                              '承認者コード
                    dtRow("D73_APP_USR") = DBNull.Value                                                '承認者名称
                    dtRow("D73_CANCEL_CLS") = DBNull.Value                                             'キャンセル区分
                    dtRow("D73_DELETE_FLG") = "0"                                                      '削除フラグ
                    dtRow("D73_TR_SEQ") = DBNull.Value                                                 '業者連番
                    dtRow("D73_SYSTEM_CD") = Me.hdnSysCd.Value                                         'システムコード
                    dtRow("D73_INSERT_DT") = DateTime.Now                                              '登録日時
                    dtRow("D73_INSERT_USR") = Session(P_SESSION_USERID)                                '登録者
                    dtRow("D73_UPDATE_DT") = DBNull.Value                                              '更新日時
                    dtRow("D73_UPDATE_USR") = DBNull.Value                                             '更新者
                    dtRow("D73_MNT_NO2") = DBNull.Value                                                '保守管理番号
                    dtRow("D73_AGC_COD") = Me.hdnAgcCd.Value                                           '代理店コード
                    dtRow("D73_AGC_NAM") = Me.lblAgc.Text                                              '代理店名
                    dtRow("D73_AGC_ZIP") = Me.hdnAgcZip.Value                                          '代理店郵便番号
                    dtRow("D73_AGC_ADS") = Me.hdnAgcAddr.Value                                         '代理店住所
                    dtRow("D73_AGC_TEL") = Me.hdnAgcTel.Value                                          '代理店ＴＥＬ
                    dtRow("D73_AGC_FAX") = Me.hdnAgcFax.Value                                          '代理店ＦＡＸ
                    dtRow("D73_PER_COD") = Me.hdnRepCd.Value                                           '代行店コード
                    dtRow("D73_PER_NAM") = Me.lblRep.Text                                              '代行店名
                    dtRow("D73_PER_ZIP") = Me.hdnRepZip.Value                                          '代行店郵便番号
                    dtRow("D73_PER_ADS") = Me.hdnRepAddr.Value                                         '代行店住所
                    dtRow("D73_PER_TEL") = Me.hdnRepTel.Value                                          '代行店ＴＥＬ
                    dtRow("D73_PER_STF_COD") = Me.hdnRepChg.Value                                      '代行店担当
                    dtRow("D73_NGC_BNS") = Me.lblNgcOrg.Text                                           'ＮＧＣ担当営業部
                    dtRow("D73_NGC_TEL") = Me.lblOrgTel.Text                                           '担当営業部ＴＥＬ
                    dtRow("D73_MDN_SET_DVS") = Me.hdnTwinCd.Value                                      '双子店区分
                    dtRow("D73_MDN_DVS") = Me.hdnEstCls.Value                                          'ＭＤＮ設置有無
                    dtRow("D73_MDN_SUU") = Me.hdnMdnCnt.Value                                          'ＭＤＮ台数
                    dtRow("D73_MDN_COD1") = Me.hdnMdnCd1.Value                                         'ＭＤＮ機器名１
                    dtRow("D73_MDN_COD2") = Me.hdnMdnCd2.Value                                         'ＭＤＮ機器名２
                    dtRow("D73_MDN_COD3") = Me.hdnMdnCd3.Value                                         'ＭＤＮ機器名３

                Case Else
                    Dim dstOrders As New DataSet
                    dstOrders = ViewState("dstOrders_1")

                    dtRow("D73_TRBL_NO") = dstOrders.Tables(0).Rows(0)("D73_TRBL_NO").ToString         'トラブル管理番号
                    dtRow("D73_MNG_CLS") = Me.ddlMngCls.ppSelectedValue                                'トラブル管理区分
                    dtRow("D73_HPN_CLS") = Me.ddlHpnCls.ppSelectedValue                                '発生区分
                    dtRow("D73_HPN_DT") = dstOrders.Tables(0).Rows(0)("D73_HPN_DT").ToString           '発生日時
                    dtRow("D73_TBOXID") = Me.txtTboxId.ppText                                          'ＴＢＯＸＩＤ
                    dtRow("D73_TBOX_TYPE") = Me.ddlTboxType.SelectedValue                              'ＴＢＯＸタイプ
                    dtRow("D73_TBOX_VER") = Me.lblVer.Text                                             'ＴＢＯＸバージョン
                    dtRow("D73_HALL_CD") = Me.hdnHallCd.Value                                          'ホールコード
                    dtRow("D73_HALL_NM") = Me.lblHallNm.Text                                           'ホール名称
                    dtRow("D73_ADDR") = Me.lblAddr1.Text                                               'ホール住所
                    dtRow("D73_TELNO") = Me.lblTel.Text                                                'ホールＴＥＬ
                    dtRow("D73_EW_CLS") = Me.hdnEwclsCd.Value                                          'ＥＷ区分
                    dtRow("D73_NL_CLS") = Me.hdnNlclsCd.Value                                          'ＮＬ区分
                    dtRow("D73_UNF_CD") = Me.hdnTokatuCd.Value                                         '統括コード
                    dtRow("D73_BRANCH_CD") = Me.hdnEigyoCd.Value                                       '営業所コード
                    If Me.dttRptDt.ppText Is Nothing Or Me.dttRptDt.ppText = String.Empty Then
                        dtRow("D73_RPT_DT") = DBNull.Value                                             '申告日
                    Else
                        dtRow("D73_RPT_DT") = Me.dttRptDt.ppText                                       '申告日
                    End If
                    dtRow("D73_RPT_CHARGE") = Me.tetRpt.ppText                                         '申告者
                    dtRow("D73_RPT_BASE") = Me.ddlRptBase.SelectedValue                                '申告元コード
                    dtRow("D73_RPT_TEL") = Me.tetRptTel.ppText                                         '申告元ＴＥＬ
                    dtRow("D73_RPT_CD") = Me.ddlRptCd.SelectedValue                                    '申告内容コード
                    dtRow("D73_RPT_DTL1") = Me.tetRptDtl1.ppText                                       '申告内容１
                    dtRow("D73_RPT_DTL2") = Me.tetRptDtl2.ppText                                       '申告内容２
                    If Me.dttRcptDt.ppDateBox.Text Is Nothing _
                        Or Me.dttRcptDt.ppDateBox.Text = String.Empty Then
                        dtRow("D73_RCPT_DT") = DBNull.Value
                    Else
                        dtRow("D73_RCPT_DT") = Me.dttRcptDt.ppDateBox.Text _
                                               + " " _
                                               + CInt(Me.dttRcptDt.ppHourText).ToString("00") _
                                               + ":" _
                                               + CInt(Me.dttRcptDt.ppMinText).ToString("00") _
                                               + ":00"                                                 '受付日時
                    End If
                    dtRow("D73_RCPT_CHARGE") = Me.tetRptCharg.ppText                                   '受付者
                    dtRow("D73_INH_CNTNT1") = Me.tetInhCntnt1.ppText                                   '引継内容１
                    dtRow("D73_INH_CNTNT2") = Me.tetInhCntnt2.ppText                                   '引継内容２
                    dtRow("D73_DEALREQ_CD1") = Me.ddlDealreqCd1.ppDropDownList.SelectedValue           '対応依頼コード１
                    dtRow("D73_DEALREQ_CD2") = Me.ddlDealreqCd2.ppDropDownList.SelectedValue           '対応依頼コード２
                    dtRow("D73_DEALREQ_CD3") = Me.ddlDealreqCd3.ppDropDownList.SelectedValue           '対応依頼コード３
                    dtRow("D73_EQ_CLS") = Me.ddlEqCls.SelectedValue                     '装置区分
                    dtRow("D73_EQ_CD") = Me.ddlEqCd.SelectedValue                       '装置詳細コード
                    If Me.lblIraino.Text Is Nothing Or Me.lblIraino.Text = String.Empty Then
                        dtRow("D73_MNT_NO") = DBNull.Value                                             '保守管理番号
                    Else
                        dtRow("D73_MNT_NO") = Me.lblIraino.Text                                        '保守管理番号
                    End If
                    dtRow("D73_STATUS_CD") = Me.ddlStatusCd.SelectedValue                              '作業状況コード
                    dtRow("D73_STTS_NOTETEXT") = Me.txtSttsNotetext.ppText                             '作業状況備考
                    dtRow("D73_REPAIR_CD") = Me.ddlRepairCd.SelectedValue                              '回復内容コード
                    dtRow("D73_REPAIR_CNTNT") = Me.tetRepairCcntnt.ppText                              '回復内容
                    If Me.chkImpCls.Checked = True Then
                        dtRow("D73_IMP_CLS") = "1"                                                     '故障重要度区分
                    Else
                        dtRow("D73_IMP_CLS") = "0"                                                     '故障重要度区分
                    End If

                    If CheckBoxList1.Items(0).Selected Then                                            '営業支障区分
                        dtRow("D73_BSNSDIST_CLS") = "1"
                    Else
                        dtRow("D73_BSNSDIST_CLS") = "0"
                    End If
                    If CheckBoxList1.Items(1).Selected Then                                            '２次支障区分
                        dtRow("D73_SCNDDIST_CLS") = "1"
                    Else
                        dtRow("D73_SCNDDIST_CLS") = "0"
                    End If
                    If Me.txtConfUsr.ppText.Trim.Length = 0 Then
                        dtRow("D73_CONF_DT") = DBNull.Value                                            '確認日
                        dtRow("D73_CONFUSR_CD") = DBNull.Value                                         '確認者コード
                        dtRow("D73_CONF_USR") = DBNull.Value                                           '確認者名称
                    Else
                        If Me.lblConfDt.Text = String.Empty _
                            Or Me.lblConfDt.Text Is Nothing Then
                            dtRow("D73_CONF_DT") = DateTime.Now                                        '確認日
                        Else
                            'REQSELP001-004
                            '更新無しパターン
                            'If dstOrders.Tables(0).Rows(0)("D73_CONFUSR_CD") = Me.ddlAddUsr.SelectedValue Then
                            If dstOrders.Tables(0).Rows(0)("D73_CONF_USR") = Me.txtConfUsr.ppText.Trim Then
                                'dtRow("D73_CONF_DT") = Me.lblConfDt.Text                               '確認日
                                If dstOrders.Tables(0).Rows(0)("D73_CONF_DT") Is DBNull.Value Then
                                    dtRow("D73_CONF_DT") = DBNull.Value
                                Else
                                    dtRow("D73_CONF_DT") = dstOrders.Tables(0).Rows(0)("D73_CONF_DT")
                                End If
                                'REQSELP001-004 END
                            Else
                                '更新有パターン
                                dtRow("D73_CONF_DT") = DateTime.Now                                    '確認日
                            End If
                        End If
                        dtRow("D73_CONFUSR_CD") = ""                                                    '確認者コード
                        'dtRow("D73_CONFUSR_CD") = Me.ddlConfUsr.SelectedValue                          '確認者コード
                        'strSplit = Me.ddlConfUsr.SelectedItem.Text.Split(":")
                        dtRow("D73_CONF_USR") = Me.txtConfUsr.ppText.Trim                               '確認者名称
                    End If
                    If Me.ddlAddUsr.SelectedIndex = 0 Then
                        dtRow("D73_APP_DT") = DBNull.Value                                             '承認日
                        dtRow("D73_APPUSR_CD") = DBNull.Value                                          '承認者コード
                        dtRow("D73_APP_USR") = DBNull.Value                                            '承認者名称
                    Else
                        If (Me.lblAppDt.Text = String.Empty _
                            Or Me.lblAppDt.Text Is Nothing) Then
                            dtRow("D73_APP_DT") = DateTime.Now                                         '承認日
                        Else
                            'REQSELP001-004
                            If dstOrders.Tables(0).Rows(0)("D73_APPUSR_CD").ToString <> String.Empty Then
                                If dstOrders.Tables(0).Rows(0)("D73_APPUSR_CD") = Me.ddlAddUsr.SelectedValue Then
                                    dtRow("D73_APP_DT") = dstOrders.Tables(0).Rows(0)("D73_APP_DT")
                                    'dtRow("D73_APP_DT") = Me.lblAppDt.Text                                 '承認日
                                Else
                                    dtRow("D73_APP_DT") = DateTime.Now                                     '承認日
                                End If
                            Else
                                dtRow("D73_APP_DT") = DateTime.Now
                                'dtRow("D73_APP_DT") = Me.lblAppDt.Text                                 '承認日
                            End If
                            'REQSELP001-004 END
                        End If
                        dtRow("D73_APPUSR_CD") = Me.ddlAddUsr.SelectedValue                            '承認者コード
                        strSplit = Me.ddlAddUsr.SelectedItem.Text.Split(":")
                        dtRow("D73_APP_USR") = strSplit(1)                                             '承認者名称
                    End If
                    dtRow("D73_CANCEL_CLS") = DBNull.Value                                             'キャンセル区分
                    dtRow("D73_DELETE_FLG") = "0"                                                      '削除フラグ
                    dtRow("D73_TR_SEQ") = DBNull.Value                                                 '業者連番
                    dtRow("D73_SYSTEM_CD") = dstOrders.Tables(0).Rows(0)("D73_SYSTEM_CD")              'システムコード
                    dtRow("D73_INSERT_DT") = dstOrders.Tables(0).Rows(0)("D73_INSERT_DT")              '登録日時
                    dtRow("D73_INSERT_USR") = dstOrders.Tables(0).Rows(0)("D73_INSERT_USR")            '登録者
                    dtRow("D73_UPDATE_DT") = DateTime.Now                                              '更新日時
                    dtRow("D73_UPDATE_USR") = Session(P_SESSION_USERID)                                '更新者
                    If Me.lblIraino2.Text Is Nothing Or Me.lblIraino2.Text = String.Empty Then
                        dtRow("D73_MNT_NO2") = DBNull.Value                                             '保守管理番号
                    Else
                        dtRow("D73_MNT_NO2") = Me.lblIraino2.Text                                        '保守管理番号
                    End If
            End Select

            dataset.Tables(0).Rows.Add(dtRow)
            REQSELP001_D73_TROUBLE = dataset

            Return True

        Catch ex As Exception
            Return False
        End Try


    End Function

#End Region

#Region "トラブル明細編集処理"

    ''' <summary>
    ''' トラブル明細編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msSet_D74_TROUBLE_DTIL_Edit(ByRef REQSELP001_D74_TROUBLE_DTIL As DataSet _
                                                , ByVal Terms As String)

        Dim dataset As New REQSELP001_D74_TROUBLE_DTIL
        Dim dtRow As DataRow
        Dim strKey() As String = Nothing

        dtRow = dataset.Tables(0).NewRow

        'ビューステート項目取得
        strKey = ViewState(P_KEY)

        If Terms = "登録" Then
            dtRow("D74_TRBL_NO") = lblTrblno.Text.Trim                                       'トラブル管理番号
            dtRow("D74_TRBL_SEQ") = CInt(ViewState("TRBL_SEQ")) + 1                'トラブル管理連番
            dtRow("D74_DEAL_CD") = txtDealCd.ppText                                '対応コード
            If txtDealDt.ppDateBox.Text = String.Empty Then
                dtRow("D74_DEAL_DT") = DBNull.Value
            Else
                dtRow("D74_DEAL_DT") = txtDealDt.ppDateBox.Text _
                                   + " " _
                                   + CInt(txtDealDt.ppHourText).ToString("00") _
                                   + ":" _
                                   + CInt(txtDealDt.ppMinText).ToString("00") _
                                   + ":00"                                         '対応日時
            End If
            dtRow("D74_DEAL_USR") = txtDealUer.ppText                       '対応担当者
            dtRow("D74_DEAL_DTL") = txaDealDtl.Text                              '対応内容
            If chkAdptCls.Checked = False Then
                dtRow("D74_ADPT_CLS") = "0"     '0:非適応                            '適応区分
            Else
                dtRow("D74_ADPT_CLS") = "1"     '1:適応                          '適応区分
            End If
            dtRow("D74_DELETE_FLG") = "0"                                          '削除フラグ
            dtRow("D74_INSERT_DT") = DateTime.Now                                  '登録日時
            dtRow("D74_INSERT_USR") = Session(P_SESSION_USERID)                    '登録者
            dtRow("D74_UPDATE_DT") = DBNull.Value                                  '更新日時
            dtRow("D74_UPDATE_USR") = DBNull.Value                                 '更新者

        Else
            dtRow("D74_TRBL_NO") = lblTrblno.Text.Trim                                       'トラブル管理番号
            dtRow("D74_TRBL_SEQ") = CInt(ViewState("TRBL_SEQ"))                    'トラブル管理連番
            dtRow("D74_DEAL_CD") = txtDealCd.ppText                                '対応コード
            If txtDealDt.ppDateBox.Text = String.Empty Then
                dtRow("D74_DEAL_DT") = DBNull.Value
            Else
                dtRow("D74_DEAL_DT") = txtDealDt.ppDateBox.Text _
                                   + " " _
                                   + CInt(txtDealDt.ppHourText).ToString("00") _
                                   + ":" _
                                   + CInt(txtDealDt.ppMinText).ToString("00") _
                                   + ":00"                                         '対応日時
            End If
            dtRow("D74_DEAL_USR") = txtDealUer.ppText                       '対応担当者
            dtRow("D74_DEAL_DTL") = txaDealDtl.Text                              '対応内容
            If chkAdptCls.Checked = False Then
                dtRow("D74_ADPT_CLS") = "0"     '0:非適応                            '適応区分
            Else
                dtRow("D74_ADPT_CLS") = "1"     '1:適応                          '適応区分
            End If
            dtRow("D74_DELETE_FLG") = "0"                                          '削除フラグ
            dtRow("D74_INSERT_DT") = DBNull.Value                                  '登録日時
            dtRow("D74_INSERT_USR") = DBNull.Value                                 '登録者
            dtRow("D74_UPDATE_DT") = DateTime.Now                                  '更新日時
            dtRow("D74_UPDATE_USR") = Session(P_SESSION_USERID)                    '更新者

        End If

        dataset.Tables(0).Rows.Add(dtRow)
        REQSELP001_D74_TROUBLE_DTIL = dataset

        Return True

    End Function

#End Region

#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' 検証チェック
    ''' </summary>
    ''' <param name="btn_nm"></param>
    ''' <remarks></remarks>
    Private Sub msIsvalid_chk(ByVal btn_nm As String)

        Dim dtrErrMes As DataRow

        Select Case btn_nm
            Case "btnAdd"    'トラブル 登録
                If ddlTboxType.SelectedIndex = 0 Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "ＴＢＯＸタイプ")
                    valddl.Text = "未入力エラー"
                    valddl.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    valddl.Enabled = True
                    valddl.IsValid = False
                    valddl.SetFocusOnError = True
                End If

            Case "btnRigth1"    'トラブル 更新
                If ddlStatusCd.SelectedIndex = 0 Then
                    dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "作業状況")
                    CustomValidator3.Text = "未入力エラー"
                    CustomValidator3.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
                    CustomValidator3.Enabled = True
                    CustomValidator3.IsValid = False
                    CustomValidator3.SetFocusOnError = True
                End If

            Case "btnInsert"    'トラブル明細 追加
                'If Not Me.txtDealDtl.ppText = String.Empty Or Not Me.txtDealDtl.ppText Is Nothing Then
                '    If Me.txtDealDtl.ppText.Length > 200 Then
                '        Me.txtDealDtl.psSet_ErrorNo("3002", Me.txtDealDtl.ppName, Me.txtDealDtl.ppMaxLength)
                '    End If
                'End If

                If Me.txtDealCd.ppText <> "TR" Then
                    Me.txtDealCd.psSet_ErrorNo("2011", Me.txtDealCd.ppName, """TR""")
                End If

            Case "btnUpdate"    'トラブル明細 更新
                'If Not Me.txtDealDtl.ppText = String.Empty Or Not Me.txtDealDtl.ppText Is Nothing Then
                '    If Me.txtDealDtl.ppText.Length > 200 Then
                '        Me.txtDealDtl.psSet_ErrorNo("3002", Me.txtDealDtl.ppName, Me.txtDealDtl.ppMaxLength)
                '    End If
                'End If

                If Me.txtDealCd.ppText <> "TR" Then
                    Me.txtDealCd.psSet_ErrorNo("2011", Me.txtDealCd.ppName, """TR""")
                End If

        End Select


    End Sub

#End Region

End Class
