'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　ＢＢ１調査依頼一覧
'*　ＰＧＭＩＤ：　BBPLSTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　14.02.01　：　(NKC)浜本
'********************************************************************************************************************************
'---------------------------------------------------------------------------------------------------------------------------------
'番号　　　　　｜　日付　　　　　｜　名前　　｜　備考
'---------------------------------------------------------------------------------------------------------------------------------
'BBPLSTP001-001　　2015/07/17　　　　栗原　　　TBOXID変更時、型式番号のドロップダウンリストを再設定するよう変更　
'BBPLSTP001-002    2016/01/26        武         FS移転に伴う帳票修正
'BBPLSTP001-003    2016/02/15        加賀       システムの入力をテキスト→ﾄﾞﾛｯﾌﾟﾀﾞｳﾝに変更
'BBPLSTP001-004    2017/04/10        加賀       修理依頼Noの入力形式を変更(先頭数字を許容)

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMDataConnect
Imports System.Data.SqlClient
Imports SPC.ClsCMExclusive

#End Region

Public Class BBPLSTP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"

    ''' <summary>
    ''' 帳票名(ブラックボックス調査チェックリスト)(日本語箇所)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strCHOHYO_NM_PART_JP As String = "ブラックボックス調査チェックリスト"

    ''' <summary>
    ''' リダイレクトパス(次画面相対パス)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const strREDIRECTPATH As String = "~/" & P_SPC & "/" & P_FUN_BBP & P_SCR_UPD & P_PAGE & "001/" & P_FUN_BBP & P_SCR_UPD & P_PAGE & "001.aspx"

    ''' <summary>
    ''' プログラムＩＤ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const M_MY_DISP_ID As String = P_FUN_BBP & P_SCR_LST & P_PAGE & "001"
    Private Const M_UPD_DISP_ID = P_FUN_BBP & P_SCR_UPD & P_PAGE & "001"

#End Region

#Region "構造体・列挙体定義"
    Private Enum E_CHKKBN
        検索 = 1
        登録
    End Enum
#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

#End Region

#Region "イベントプロシージャ"
    ''' <summary>
    ''' 初期化イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'イベントハンドラ付加
        msSetEventHandler()

        'ポストバック
        If Not Me.IsPostBack Then

            '開始ログ出力
            psLogStart(Me)

            '排他情報用のグループ番号保管
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            End If

            ViewState(P_SESSION_USERID) = Session(P_SESSION_USERID)
            ViewState(P_SESSION_TERMS) = Session(P_SESSION_TERMS)

            'ValidationGroupの設定
            Master.ppRigthButton1.ValidationGroup = "Detail"

            '「検索条件クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            '「クリア」ボタン押下時の検証を無効
            btnDetailClear.CausesValidation = False

            '「BB1調査結果CSV」ボタン押下時の検証を無効
            Master.Master.ppRigthButton1.CausesValidation = False

            '「チェックリスト印刷」ボタン押下時の検証を無効
            Master.Master.ppRigthButton2.CausesValidation = False

            '「PDF」ボタン押下時の検証を無効
            Master.Master.ppRigthButton3.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '「印刷」、「CSV」ボタン活性
            Master.Master.ppRigthButton1.Text = "ＢＢ１調査結果ＣＳＶ"
            Master.Master.ppRigthButton1.Visible = True
            Master.Master.ppRigthButton2.Text = "チェックリスト印刷"
            Master.Master.ppRigthButton2.Visible = True
            Master.Master.ppRigthButton3.Text = "印刷"
            Master.Master.ppRigthButton3.Visible = True

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ドロップダウンリスト設定　'BBPLSTP001-003
            msSetddl()

            'ページの初期化
            msPageClear()

            'モードによる、項目のEnable制御をおこなう
            Me.msPageEnableSet(ViewState(P_SESSION_TERMS))

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

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
                btnDetailClear.Enabled = False
                btnDetailUpdate.Enabled = False
            Case "NGC"
        End Select

    End Sub

    ''' <summary>
    ''' TBOXID入力時の動き
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ctl_Change(ByVal sender As Object, ByVal e As System.Object)
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        objStack = New StackFrame

        '空白の場合は処理しない.
        If txtTboxId.ppText = String.Empty Then
            'BBPLSTP001-001
            msSetddlKosyo(ddlKosyoKatasikiNo)
            'BBPLSTP001-001 END
            Exit Sub
        End If

        Try

            '開始ログ出力
            psLogStart(Me)

            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'プロシージャ設定(TBOXデータ検索)
            objCmd = New SqlCommand("BBPLSTP001_S3")
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Connection = objCon

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, txtTboxId.ppText))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            Dim objTbl As DataTable = objDs.Tables(0)
            If objTbl.Rows.Count > 0 Then
                lblNlKbn.Text = CType(objTbl.Rows(0)("T03_NL_CLS"), String)
                lblEwKbn.Text = CType(objTbl.Rows(0)("T01_EW_FLG"), String)
                lblTboxSbt.Text = CType(objTbl.Rows(0)("T03_TBOXCLASS_CD"), String)
                lblHallNm.Text = CType(objTbl.Rows(0)("T01_HALL_NAME"), String)
                hdnTboxSbtCd.Value = CType(objTbl.Rows(0)("M23_TBOXCLS"), String)
                'BBPLSTP001-001
                msSetddlKosyo(ddlKosyoKatasikiNo)
                'BBPLSTP001-001 END
            Else
                lblNlKbn.Text = ""
                lblEwKbn.Text = ""
                lblTboxSbt.Text = ""
                lblHallNm.Text = ""
                hdnTboxSbtCd.Value = ""
                'BBPLSTP001-001
                msSetddlKosyo(ddlKosyoKatasikiNo)
                'BBPLSTP001-001 END
                txtTboxId.psSet_ErrorNo("2002", "入力されたTBOXID")
                txtTboxId.ppTextBox.Focus()
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return
        Finally

            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.Object)
        '開始ログ出力
        psLogStart(Me)

        '検索条件をクリアする
        msClearSearchPart()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.Object)
        '開始ログ出力
        psLogStart(Me)

        '検索
        msDataRead()

        '終了ログ出力
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' クリア(登録エリア)ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailClear_Click(sender As Object, e As EventArgs) Handles btnDetailClear.Click
        '開始ログ出力
        psLogStart(Me)

        '更新項目クリア
        msClearUpdatePart()
        'BBPLSTP001-001
        msSetddlKosyo(ddlKosyoKatasikiNo)
        'BBPLSTP001-001 END

        '終了ログ出力
        psLogEnd(Me)
    End Sub

    ''' <summary>
    ''' BB調報Noチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSelfChk() As Integer
        Dim intErrFlg As Integer = 0
        Dim intErrJti As Integer = 0

        Return mfChkInputSrcBBChohoNo_2(txtSrcBBChohoNo_2FrTo)

    End Function

    ''' <summary>
    ''' 入力確認チェック
    ''' </summary>13    
    ''' <returns>負値：エラー</returns>
    ''' <remarks></remarks>
    Private Function mfChkInputSrcBBChohoNo_2(ByRef objCtrl As ClsCMTextBoxTwoFromTo) As Integer

        Dim strVal1 As String = objCtrl.ppFromTextOne.Trim()
        Dim strVal2 As String = objCtrl.ppFromTextTwo.Trim()
        Dim strVal3 As String = objCtrl.ppToTextOne.Trim()
        Dim strVal4 As String = objCtrl.ppToTextTwo.Trim()

        '入力なし
        If strVal1 = "" AndAlso strVal2 = "" AndAlso strVal3 = "" AndAlso strVal4 = "" Then
            Return 0
            '①のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 = "" AndAlso strVal3 = "" AndAlso strVal4 = "" Then
            Return 1
            '②のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 <> "" AndAlso strVal3 = "" AndAlso strVal4 = "" Then
            Return 2
            '③のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 = "" AndAlso strVal3 <> "" AndAlso strVal4 = "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '④のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 = "" AndAlso strVal3 = "" AndAlso strVal4 <> "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '①、②のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 <> "" AndAlso strVal3 = "" AndAlso strVal4 = "" Then
            Return 12
            '①、③のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 = "" AndAlso strVal3 <> "" AndAlso strVal4 = "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '①、④のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 = "" AndAlso strVal3 = "" AndAlso strVal4 <> "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '②、③のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 <> "" AndAlso strVal3 <> "" AndAlso strVal4 = "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '②、④のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 <> "" AndAlso strVal3 = "" AndAlso strVal4 <> "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '③、④のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 = "" AndAlso strVal3 <> "" AndAlso strVal4 <> "" Then
            Return 34
            '①、②、③のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 <> "" AndAlso strVal3 <> "" AndAlso strVal4 = "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '①、②、④のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 <> "" AndAlso strVal3 = "" AndAlso strVal4 <> "" Then
            objCtrl.ppToTextOne = strVal1
            Return 124
            '①、③、④のみ入力
        ElseIf strVal1 <> "" AndAlso strVal2 = "" AndAlso strVal3 <> "" AndAlso strVal4 <> "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '②、③、④のみ入力
        ElseIf strVal1 = "" AndAlso strVal2 <> "" AndAlso strVal3 <> "" AndAlso strVal4 <> "" Then
            objCtrl.psSet_ErrorNo("5005", objCtrl.ppName, "BB調報No")
            objCtrl.ppTextBoxOneFrom.Focus()
            Return -1
            '①、②、③、④入力
        Else
            Return 1234
        End If
    End Function

    ''' <summary>
    ''' 登録ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailUpdate_Click(sender As Object, e As EventArgs) Handles btnDetailUpdate.Click
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing
        Dim objTran As SqlTransaction = Nothing

        objStack = New StackFrame

        Dim blnResult As Boolean = False
        'TBOXID存在チェック
        msChk_Tboxid(txtTboxId.ppText, blnResult)

        '入力チェック
        msGamen_Check(blnResult)

        '登録エリア項目チェック
        If Page.IsValid() = False Then
            Return
        End If

        Try
            '開始ログ出力
            psLogStart(Me)

            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            objTran = objCon.BeginTransaction()

            'ストアド設定
            objCmd = New SqlCommand("BBPLSTP001_U1")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            Dim strVal As String = ""
            Dim dteToday As DateTime = DateTime.Now

            'ＢＢ調報Ｎｏの頭3文字を取得
            strVal = mfGetBB1NoInitial(lblNlKbn.Text, dteToday)

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmInitial", SqlDbType.NVarChar, strVal))
                .Add(pfSet_Param("prmD50_REPREQ_NO", SqlDbType.NVarChar, txtSyuriIraiNo.ppText))
                .Add(pfSet_Param("prmD50_TBOXID", SqlDbType.NVarChar, txtTboxId.ppText))
                .Add(pfSet_Param("prmD50_NL_FLG", SqlDbType.NVarChar, lblNlKbn.Text))
                .Add(pfSet_Param("prmD50_EW_FLG", SqlDbType.NVarChar, lblEwKbn.Text))
                .Add(pfSet_Param("prmD50_HALL_NM", SqlDbType.NVarChar, lblHallNm.Text))
                .Add(pfSet_Param("prmD50_TBOX_CLS", SqlDbType.NVarChar, mfGetDBNull(hdnTboxSbtCd.Value)))
                .Add(pfSet_Param("prmD50_MODEL_NO", SqlDbType.NVarChar, ddlKosyoKatasikiNo.SelectedValue))
                .Add(pfSet_Param("prmD50_SERIAL", SqlDbType.NVarChar, txtSerialNo.ppText))
                .Add(pfSet_Param("prmD50_STATUS_CD", SqlDbType.NVarChar, ddlShinchokuJokyo.SelectedValue))
                .Add(pfSet_Param("prmD50_RCV_DT", SqlDbType.DateTime, mfGetDBNull(txtJuryobi.ppText)))
                .Add(pfSet_Param("prmD50_WRK_DT", SqlDbType.DateTime, mfGetDBNull(txtSagyobi.ppText)))
                .Add(pfSet_Param("prmD50_REP_DT", SqlDbType.DateTime, mfGetDBNull(txtHokokubi.ppText)))
                .Add(pfSet_Param("prmD50_BB1SEND_DT", SqlDbType.DateTime, mfGetDBNull(txtBB1Sofubi.ppText)))
                .Add(pfSet_Param("prmD50_INS_MT", SqlDbType.NVarChar, txtKensyuTuki.ppText))
                .Add(pfSet_Param("prmD50_INSERT_DT", SqlDbType.DateTime, dteToday))
                .Add(pfSet_Param("prmD50_INSERT_USR", SqlDbType.NVarChar, ViewState(P_SESSION_USERID)))
                .Add(pfSet_Param("prmD50_UPDATE_DT", SqlDbType.DateTime, DBNull.Value))
                .Add(pfSet_Param("prmD50_UPDATE_USR", SqlDbType.NVarChar, DBNull.Value))
            End With

            objCmd.Transaction = objTran

            'SQL実行
            Dim intRet As Integer = objCmd.ExecuteNonQuery()

            '登録エラー
            If intRet <= 0 Then
                Throw New Exception()
            End If

            'コミット
            objTran.Commit()

            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "画面内容")

        Catch ex As Exception
            If Not objTran Is Nothing Then
                'ロールバック
                objTran.Rollback()
            End If

            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "画面内容")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' BB1調査結果CSVボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsv_Click(sender As Object, e As EventArgs)

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            Dim objTbl As DataTable = mfGetNlTable()

            'データテーブルとして一覧を取得
            For i As Integer = 0 To grvList.Rows.Count - 1
                Dim objRow As DataRow = objTbl.NewRow()
                objRow("BBCHOHONO") = CType(grvList.Rows(i).FindControl("BBCHOHONO"), TextBox).Text
                objRow("SYURIIRAINO") = CType(grvList.Rows(i).FindControl("SYURIIRAINO"), TextBox).Text
                objRow("TBOXID") = CType(grvList.Rows(i).FindControl("TBOXID"), TextBox).Text
                objRow("KMKNLKBN") = CType(grvList.Rows(i).FindControl("KMKNLKBN"), TextBox).Text
                objRow("EWKBN") = CType(grvList.Rows(i).FindControl("EWKBN"), TextBox).Text
                objRow("HALLNM") = CType(grvList.Rows(i).FindControl("HALLNM"), TextBox).Text
                objRow("TBOXSBT") = CType(grvList.Rows(i).FindControl("TBOXSBT"), TextBox).Text
                objRow("KOSYOKIKATASIKI") = CType(grvList.Rows(i).FindControl("KOSYOKIKATASIKI"), TextBox).Text
                objRow("SERIALNO") = CType(grvList.Rows(i).FindControl("SERIALNO"), TextBox).Text
                objRow("SHINCHOKUJOKYO") = CType(grvList.Rows(i).FindControl("SHINCHOKUJOKYO"), TextBox).Text
                objRow("JURYOBI") = CType(grvList.Rows(i).FindControl("JURYOBI"), TextBox).Text
                objRow("SAGYOBI") = CType(grvList.Rows(i).FindControl("SAGYOBI"), TextBox).Text
                objRow("HOKOKUBI") = CType(grvList.Rows(i).FindControl("HOKOKUBI"), TextBox).Text
                objRow("BB1SOFUBI") = CType(grvList.Rows(i).FindControl("BB1SOFUBI"), TextBox).Text
                objRow("KENSYUTUKI") = CType(grvList.Rows(i).FindControl("KENSYUTUKI"), TextBox).Text
                objTbl.Rows.Add(objRow)
            Next

            If objTbl.Rows.Count = 0 Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Return
            End If

            'データセットに変換
            Dim objDs As New DataSet()
            objDs.Tables.Add(objTbl)

            Dim dteDate As DateTime = DateTime.Now()
            Dim strHiduke As String = ""
            Dim strFilePath As String = ""
            strHiduke = String.Format("{0:yyyyMMddHHmmss}", dteDate)
            strFilePath = Server.MapPath("~/" & P_WORK_NM & "/" & Session.SessionID)

            'CSVファイルを作成
            If pfDLCsvFile(strCHOHYO_NM_PART_JP & "_" & strHiduke & ".csv", objDs.Tables(0), True, Me) Then
                Throw New Exception()
            End If

            '完了メッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "CSVファイルの作成")

        Catch ex As Threading.ThreadAbortException
            '完了メッセージ
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "CSVファイルの作成")

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "CSVファイル")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

    ''' <summary>
    ''' チェックリスト印刷ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            Dim strHiduke As String = ""
            Dim dteNow As DateTime = Nothing

            '印刷データ作成
            Dim objPrtDtTbl As DataTable = mfGetPrintData(dteNow)

            strHiduke = String.Format("{0:yyyyMMddHHmmss}", dteNow)

            'データがない場合は印刷しない
            If objPrtDtTbl.Rows.Count = 0 Then
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Return
            End If

            'ファイル名作成
            Dim strFileNm As String = ""
            strFileNm = strCHOHYO_NM_PART_JP & "_" & strHiduke

            'PDF作成
            Dim rpt As New WATREP001()
            psPrintPDF(Me, rpt, objPrtDtTbl, strFileNm)

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "チェックリスト")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            '終了ログ出力
            psLogEnd(Me)

        End Try

    End Sub

    ''' <summary>
    ''' PDFボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPdf_Click(sender As Object, e As EventArgs)

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            Dim dteNow As DateTime = Nothing

            '印刷データ作成
            Dim objPrtDtTbl As DataTable = mfGetPrintData(dteNow)

            'データがない場合は印刷しない
            If objPrtDtTbl.Rows.Count = 0 Then
                psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Return
            End If

            'ブラックボックス調査報告書の印刷処理
            mfGetHokokushoPrint()

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ブラックボックス調査報告書")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub

    ''' <summary>
    ''' 更新ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        Dim arTable_Name As New ArrayList
        Dim arKey As New ArrayList
        Dim strExclusiveDate As String = String.Empty

        objStack = New StackFrame

        Try
            '開始ログ出力
            psLogStart(Me)

            '選択ボタンでなければ、なにもしない
            If e.CommandName.Trim() <> "btnUpdate" Then
                Return
            End If

            Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
            Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行

            'BB調報NO
            Dim strVal(0) As String
            strVal(0) = CType(rowData.FindControl("BBCHOHONO"), TextBox).Text

            'セッション変数に受け渡す値を記憶
            Session(P_SESSION_USERID) = ViewState(P_SESSION_USERID)         'ユーザＩＤ
            Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新      '処理区分
            Session(P_KEY) = strVal                     'ＢＢ調報Ｎｏ
            Session(P_SESSION_OLDDISP) = M_MY_DISP_ID '画面ID
            Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text

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


            'ロック対象テーブル名の登録.
            arTable_Name.Insert(0, "D50_BBINVESTREPORT")

            'ロックテーブルキー項目の登録.
            arKey.Insert(0, CType(rowData.FindControl("BBCHOHONO"), TextBox).Text)

            '排他情報確認処理(更新処理の実行).
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

                '登録年月日時刻.
                Session(P_SESSION_EXCLUSIV_DATE) = strExclusiveDate
                '登録年月日時刻(明細)に登録.
                Me.Master.Master.ppExclusiveDateDtl = strExclusiveDate

            Else

                '排他ロック中.
                Exit Sub

            End If

            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, strREDIRECTPATH, strPrm, "TRANS")

            '画面遷移
            psOpen_Window(Me, strREDIRECTPATH)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' マスターページ項目にイベントハンドラを付加
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetEventHandler()
        '検索条件クリアボタン
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        '検索ボタン
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        'BB1調査結果CSVボタン
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnCsv_Click
        'チェックリスト印刷ボタン
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnPrint_Click
        'PDFボタン
        AddHandler Master.Master.ppRigthButton3.Click, AddressOf btnPdf_Click
        'TBOXIDを入力した場合
        AddHandler Me.txtTboxId.ppTextBox.TextChanged, AddressOf ctl_Change
        Me.txtTboxId.ppTextBox.AutoPostBack = True

        '登録ボタンの確認メッセージ設定
        Me.btnDetailUpdate.OnClientClick =
            pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "画面の入力内容")

        'BB1調査結果CSVボタンの確認メッセージ設定
        Master.Master.ppRigthButton1.OnClientClick =
            pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "一覧の内容でCSV")

        'チェックリスト印刷ボタンの確認メッセージ設定
        Master.Master.ppRigthButton2.OnClientClick =
            pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "チェックリスト")

        'PDFボタンの確認メッセージ設定
        Master.Master.ppRigthButton3.OnClientClick =
            pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ブラックボックス調査報告書")

    End Sub

    ''' <summary>
    ''' ページ初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msPageClear()
        '検索項目クリア
        msClearSearchPart()

        '更新項目クリア
        msClearUpdatePart()

        '一覧項目クリア
        msClearListPart()

        'BB調報NO項目のタブインデックス設定
        msSetTabIndex()

        '(検索)進捗状況ドロップダウンリスト設定
        msSetddlShinchoku(ddlSrcShinchokuJokyo)

        '(追加)進捗状況ドロップダウンリスト設定
        msSetddlShinchoku(ddlShinchokuJokyo)

        '故障型式番号ドロップダウンリスト設定
        msSetddlKosyo(ddlKosyoKatasikiNo)

        'TBOXID項目にフォーカスをセット
        txtSrcTboxIdFrTo.ppTextBoxFrom.Focus()

    End Sub

    ''' <summary>
    ''' 検索項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchPart()
        txtSrcTboxIdFrTo.ppFromText = ""                'TBOXIDFROM
        txtSrcTboxIdFrTo.ppToText = ""                  'TBOXIDTO
        txtSrcBBChohoNo_2FrTo.ppFromTextOne = ""        'BB調報NoFrom1
        txtSrcBBChohoNo_2FrTo.ppToTextOne = ""          'BB調報NoTo1
        txtSrcBBChohoNo_2FrTo.ppFromTextTwo = ""        'BB調報NoFrom2
        txtSrcBBChohoNo_2FrTo.ppToTextTwo = ""          'BB調報NoTo2
        txtSrcSyuriIraiNoFrTo.ppFromText = ""           '修理依頼NoFrom
        txtSrcSyuriIraiNoFrTo.ppToText = ""             '修理依頼NoTo
        ddlSrcNlKbn.ppDropDownList.SelectedIndex = 0    'NL区分
        ddlSystem.ppSelectedValue = ""                  'TBOX種別 'BBPLSTP001-003
        ddlSrcShinchokuJokyo.SelectedIndex = 0          '進捗状況
        txtSrcJuryobiFrTo.ppFromText = ""               '受領日From
        txtSrcJuryobiFrTo.ppToText = ""                 '受領日To
        txtSrcKensyuTukiFrTo.ppFromText = ""            '検収月From
        txtSrcKensyuTukiFrTo.ppToText = ""              '検収月To
    End Sub

    ''' <summary>
    ''' 更新項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearUpdatePart()
        'lblBBChohoNoFr.Text = ""                'BB調報NoFrom
        'lblBBChohoNoTo.Text = ""                'BB調報NoFrom
        txtSyuriIraiNo.ppText = ""              '修理依頼No
        txtTboxId.ppText = ""                   'TBOXID
        lblNlKbn.Text = ""                      'NL区分
        lblEwKbn.Text = ""                      'EW区分
        lblTboxSbt.Text = ""                    'TBOX種別
        lblHallNm.Text = ""                     'ホール名
        ddlKosyoKatasikiNo.SelectedIndex = 0    '故障機型式番号
        txtSerialNo.ppText = ""                 'シリアルNo
        ddlShinchokuJokyo.SelectedIndex = 0     '進捗状況
        txtJuryobi.ppText = ""                  '受領日
        txtSagyobi.ppText = ""                  '作業日
        txtHokokubi.ppText = ""                 '報告日
        txtBB1Sofubi.ppText = ""                'BB1送付日
        txtKensyuTuki.ppText = ""               '検収月
        hdnTboxSbtCd.Value = ""                 'TBOX種別コード
    End Sub

    ''' <summary>
    ''' 一覧項目クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearListPart()
        Me.grvList.DataSource = New DataTable()
        Master.ppCount = "0"
        Me.grvList.DataBind()
    End Sub

    ''' <summary>
    ''' 検索処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDataRead()
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        objStack = New StackFrame

        '--2014/04/17 中川　ここから
        Dim strFromTboxId As String = Nothing
        Dim strToTboxId As String = Nothing

        'ＴＢＯＸＩＤチェック
        msCheck_TboxId(strFromTboxId, strToTboxId)
        '--2014/04/17 中川　ここまで

        '検索項目チェック
        If Page.IsValid() = False Then
            Return
        End If

        '自前チェック(ＢＢ調報NOの入力チェック)
        Dim intBBSelFlg As Integer = mfSelfChk()
        If intBBSelFlg < 0 Then
            Return
        End If

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return
            End If

            'プロシージャセット
            objCmd = New SqlCommand("BBPLSTP001_S1")
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Connection = objCon

            Dim strBBChohoFrom As String = ""
            Dim strBBChohoTo As String = ""
            If txtSrcBBChohoNo_2FrTo.ppFromTextOne = "" AndAlso txtSrcBBChohoNo_2FrTo.ppFromTextTwo = "" Then
                strBBChohoFrom = ""
            End If

            If txtSrcBBChohoNo_2FrTo.ppToTextOne = "" AndAlso txtSrcBBChohoNo_2FrTo.ppToTextTwo = "" Then
                strBBChohoTo = ""
            End If

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmBBSelFlg", SqlDbType.Int, intBBSelFlg))
                '.Add(pfSet_Param("prmD50_TBOXIDFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcTboxIdFrTo.ppFromText))) 'TBOXIDFrom
                '.Add(pfSet_Param("prmD50_TBOXIDTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcTboxIdFrTo.ppToText))) 'TBOXIDTo
                .Add(pfSet_Param("prmD50_TBOXIDFr", SqlDbType.NVarChar, mfGetDBNull(strFromTboxId))) 'TBOXIDFrom
                .Add(pfSet_Param("prmD50_TBOXIDTo", SqlDbType.NVarChar, mfGetDBNull(strToTboxId))) 'TBOXIDTo
                .Add(pfSet_Param("prmD50_BBREP_NOFr1", SqlDbType.NVarChar, mfGetDBNull(txtSrcBBChohoNo_2FrTo.ppFromTextOne))) 'BB調報NoFrom
                .Add(pfSet_Param("prmD50_BBREP_NOTo1", SqlDbType.NVarChar, mfGetDBNull(txtSrcBBChohoNo_2FrTo.ppToTextOne))) 'BB調報NoTo
                .Add(pfSet_Param("prmD50_BBREP_NOFr2", SqlDbType.NVarChar, mfGetDBNull(txtSrcBBChohoNo_2FrTo.ppFromTextTwo))) 'BB調報NoFrom
                .Add(pfSet_Param("prmD50_BBREP_NOTo2", SqlDbType.NVarChar, mfGetDBNull(txtSrcBBChohoNo_2FrTo.ppToTextTwo))) 'BB調報NoTo
                .Add(pfSet_Param("prmD50_REPREQ_NOFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcSyuriIraiNoFrTo.ppFromText))) '修理依頼NoFrom
                .Add(pfSet_Param("prmD50_REPREQ_NOTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcSyuriIraiNoFrTo.ppToText))) '修理依頼NoTo
                .Add(pfSet_Param("prmD50_NL_FLG", SqlDbType.NVarChar, mfGetDBNull(ddlSrcNlKbn.ppSelectedValue))) 'NL区分
                .Add(pfSet_Param("prmD50_TBOX_CLS", SqlDbType.NVarChar, mfGetDBNull(ddlSystem.ppSelectedValue))) 'TBOX種別 'BBPLSTP001-003
                .Add(pfSet_Param("prmD50_STATUS_CD", SqlDbType.NVarChar, mfGetDBNull(ddlSrcShinchokuJokyo.SelectedValue))) '進捗状況
                .Add(pfSet_Param("prmD50_RCV_DTFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcJuryobiFrTo.ppFromText))) '受領日From
                .Add(pfSet_Param("prmD50_RCV_DTTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcJuryobiFrTo.ppToText))) '受領日To
                .Add(pfSet_Param("prmD50_INS_MTFr", SqlDbType.NVarChar, mfGetDBNull(txtSrcKensyuTukiFrTo.ppFromText))) '検収月From
                .Add(pfSet_Param("prmD50_INS_MTTo", SqlDbType.NVarChar, mfGetDBNull(txtSrcKensyuTukiFrTo.ppToText))) '検収月To
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)


            '検索結果が0件の場合.
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            End If

            '取得異常
            If objDs Is Nothing Then
                Throw New Exception()
            End If

            '件数を設定
            Master.ppCount = objDs.Tables(0).Rows.Count.ToString

            '閾値設定
            Dim objTbl As DataTable = mfSetShikiichi(objDs.Tables(0))

            '取得したデータをグリッドに設定
            Me.grvList.DataSource = objTbl

            '変更を反映
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＢＢ１調査依頼一覧")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Me.grvList.DataSource = New DataTable()
            Me.grvList.DataBind()
            Master.ppCount = "0"
        Finally
            'DB切断
            If clsDataConnect.pfClose_Database(objCon) = False Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' 検索一覧の型(空テーブル)取得(CSV用)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNlTable() As DataTable
        Dim objTbl As New DataTable()
        objTbl.Columns.Add("BBCHOHONO")
        objTbl.Columns.Add("SYURIIRAINO")
        objTbl.Columns.Add("TBOXID")
        objTbl.Columns.Add("KMKNLKBN")
        objTbl.Columns.Add("EWKBN")
        objTbl.Columns.Add("HALLNM")
        objTbl.Columns.Add("TBOXSBT")
        objTbl.Columns.Add("KOSYOKIKATASIKI")
        objTbl.Columns.Add("SERIALNO")
        objTbl.Columns.Add("SHINCHOKUJOKYO")
        objTbl.Columns.Add("JURYOBI")
        objTbl.Columns.Add("SAGYOBI")
        objTbl.Columns.Add("HOKOKUBI")
        objTbl.Columns.Add("BB1SOFUBI")
        objTbl.Columns.Add("KENSYUTUKI")

        Return objTbl
    End Function

    ''' <summary>
    ''' ドロップダウンリスト設定 'BBPLSTP001-003
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
            End Try
        End If
    End Sub

    ''' <summary>
    ''' BB調報NO項目のTabIndexセット
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetTabIndex()
        txtSrcBBChohoNo_2FrTo.ppTextBoxOneFrom.TabIndex = 2
        txtSrcBBChohoNo_2FrTo.ppTextBoxTwo.TabIndex = 3
        txtSrcBBChohoNo_2FrTo.ppTextBoxOneTo.TabIndex = 4
        txtSrcBBChohoNo_2FrTo.ppTextBoxTwoTo.TabIndex = 5
    End Sub

    ''' <summary>
    ''' モード別Enable制御
    ''' </summary>
    ''' <param name="shrMode"></param>
    ''' <remarks></remarks>
    Private Sub msPageEnableSet(ByVal shrMode As Short)
        txtSrcTboxIdFrTo.ppTextBoxFrom.Enabled = True
        txtSrcTboxIdFrTo.ppTextBoxTo.Enabled = True
        txtSrcBBChohoNo_2FrTo.ppTextBoxOneFrom.Enabled = True
        txtSrcBBChohoNo_2FrTo.ppTextBoxTwo.Enabled = True
        txtSrcSyuriIraiNoFrTo.ppTextBoxFrom.Enabled = True
        txtSrcSyuriIraiNoFrTo.ppTextBoxTo.Enabled = True
        ddlSrcNlKbn.ppEnabled = True
        ddlSystem.ppEnabled = True 'BBPLSTP001-003
        ddlSrcShinchokuJokyo.Enabled = True
        txtSrcJuryobiFrTo.ppDateBoxFrom.Enabled = True
        txtSrcJuryobiFrTo.ppDateBoxTo.Enabled = True
        txtSrcKensyuTukiFrTo.ppTextBoxFrom.Enabled = True
        txtSrcKensyuTukiFrTo.ppTextBoxTo.Enabled = True
        Master.ppRigthButton1.Enabled = True
        Master.ppRigthButton2.Enabled = True
        Master.Master.ppRigthButton1.Enabled = True
        Master.Master.ppRigthButton2.Enabled = True
        Master.Master.ppRigthButton3.Enabled = True

        If shrMode = ClsComVer.E_遷移条件.参照 Then
            txtSyuriIraiNo.ppEnabled = False
            txtTboxId.ppEnabled = False
            ddlKosyoKatasikiNo.Enabled = False
            txtSerialNo.ppEnabled = False
            ddlShinchokuJokyo.Enabled = False
            txtJuryobi.ppEnabled = False
            txtSagyobi.ppEnabled = False
            txtHokokubi.ppEnabled = False
            txtBB1Sofubi.ppEnabled = False
            txtKensyuTuki.ppEnabled = False
            btnDetailClear.Enabled = False
            btnDetailUpdate.Enabled = False
        ElseIf shrMode = ClsComVer.E_遷移条件.更新 Then
            txtSyuriIraiNo.ppEnabled = True
            txtTboxId.ppEnabled = True
            ddlKosyoKatasikiNo.Enabled = True
            txtSerialNo.ppEnabled = True
            ddlShinchokuJokyo.Enabled = True
            txtJuryobi.ppEnabled = True
            txtSagyobi.ppEnabled = True
            txtHokokubi.ppEnabled = True
            txtBB1Sofubi.ppEnabled = True
            txtKensyuTuki.ppEnabled = True
            btnDetailClear.Enabled = True
            btnDetailUpdate.Enabled = True
        ElseIf shrMode = ClsComVer.E_遷移条件.登録 Then
            txtSyuriIraiNo.ppEnabled = True
            txtTboxId.ppEnabled = True
            ddlKosyoKatasikiNo.Enabled = True
            txtSerialNo.ppEnabled = True
            ddlShinchokuJokyo.Enabled = True
            txtJuryobi.ppEnabled = True
            txtSagyobi.ppEnabled = True
            txtHokokubi.ppEnabled = True
            txtBB1Sofubi.ppEnabled = True
            txtKensyuTuki.ppEnabled = True
            btnDetailClear.Enabled = True
            btnDetailUpdate.Enabled = True
        Else

        End If
    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（進捗ステータスマスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlShinchoku(ByRef ddlCtrl As DropDownList)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL015", objCn)

                With objCmd.Parameters
                    '業務分類→監視系
                    .Add(pfSet_Param("duty_cd", SqlDbType.NVarChar, "90"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                ddlCtrl.Items.Clear()
                ddlCtrl.DataSource = objDs.Tables(0)
                ddlCtrl.DataTextField = "進捗ステータス名"
                ddlCtrl.DataValueField = "進捗ステータス"
                ddlCtrl.DataBind()
                ddlCtrl.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                ddlCtrl.SelectedIndex = 0

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "進捗ステータスマスタ")

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
    ''' ドロップダウンリスト設定（型式マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlKosyo(ByRef ddlCtrl As DropDownList)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("BBPLSTP001_S2", objCn)
                'BBPLSTP001-001
                objCmd.Parameters.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, txtTboxId.ppText))
                'BBPLSTP001-001 END
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                ddlCtrl.Items.Clear()
                ddlCtrl.DataSource = objDs.Tables(0)
                ddlCtrl.DataTextField = "M66_MODEL_NO"
                ddlCtrl.DataValueField = "M66_MODEL_NO"
                ddlCtrl.DataBind()
                ddlCtrl.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                ddlCtrl.SelectedIndex = 0

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "型式マスタ")

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
    ''' チェックリスト印刷データの取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintData(ByRef dteNow As DateTime) As DataTable
        Dim objCon As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing

        objStack = New StackFrame

        mfGetPrintData = mfGetPrintTblKata()

        'ＤＢ接続
        If clsDataConnect.pfOpen_Database(objCon) Then
            Try
                'ストアド設定
                objCmd = New SqlCommand("BBPLSTP001_S7")
                objCmd.Connection = objCon
                objCmd.CommandType = CommandType.StoredProcedure

                'ＳＱＬ実行
                Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

                '取得エラー
                If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                    Throw New Exception()
                End If

                'データ整形
                Dim objTbl As DataTable = objDs.Tables(0)
                Dim objWkTbl As DataTable = objTbl.Clone()

                '表示中のBB調報NOを取得
                Dim strBBNo As String = ""
                For Each objRow As GridViewRow In grvList.Rows
                    strBBNo &= "'" & CType(objRow.Cells(1).Controls(0), TextBox).Text & "',"
                Next

                '取得したBB1調報Noで絞る
                If strBBNo.Trim() <> "" Then
                    Dim objDv As DataView = objTbl.DefaultView
                    strBBNo = strBBNo.Substring(0, strBBNo.Length - 1)
                    objDv.RowFilter = " BB1調報No IN (" & strBBNo & ")"
                    objDv.Sort = "TBOX種別CD"
                    objWkTbl = objDv.ToTable()
                End If

                objWkTbl.Columns.Add("No")

                For i As Integer = 1 To objWkTbl.Rows.Count
                    objWkTbl.Rows(i - 1)("No") = i
                Next

                mfGetPrintData = objWkTbl

            Catch ex As Exception

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                mfGetPrintData = mfGetPrintTblKata()
            Finally
                'ＤＢ切断
                If Not clsDataConnect.pfClose_Database(objCon) Then
                    mfGetPrintData = mfGetPrintTblKata()
                End If
            End Try
        Else
            Return mfGetPrintTblKata()
        End If

    End Function

    ''' <summary>
    ''' チェックリスト印刷のテーブル型の取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintTblKata() As DataTable
        Dim objTbl As New DataTable()

        objTbl.Columns.Add("No")
        objTbl.Columns.Add("BB1調報No")
        objTbl.Columns.Add("修理依頼No")
        objTbl.Columns.Add("TBOXID")
        objTbl.Columns.Add("型式番号")
        objTbl.Columns.Add("ＴＢＯＸ種別")
        'objTbl.Columns.Add("現在日時")
        Return objTbl

    End Function

    ''' <summary>
    ''' ＢＢ調報Ｎｏ(XYY-9999)のXYY部の取得
    ''' </summary>
    ''' <param name="strNlKbn"></param>
    ''' <returns></returns>
    ''' <remarks>ＮＬ区分が「Ｎ」ならば、DYY
    '''          ＮＬ区分が「Ｌ」ならば、LYY
    '''          ＮＬ区分が「Ｊ」ならば、DYY
    '''            ※YY：年の2桁
    ''' </remarks>
    Private Function mfGetBB1NoInitial(ByVal strNlKbn As String, ByVal dteNow As DateTime) As String
        Dim strVal As String = ""
        If Microsoft.VisualBasic.StrConv(strNlKbn, Microsoft.VisualBasic.VbStrConv.Narrow) = "N" Then
            strVal &= "D"
        ElseIf Microsoft.VisualBasic.StrConv(strNlKbn, Microsoft.VisualBasic.VbStrConv.Narrow) = "L" Then
            strVal &= "L"
        ElseIf Microsoft.VisualBasic.StrConv(strNlKbn, Microsoft.VisualBasic.VbStrConv.Narrow) = "J" Then
            strVal &= "D"
        End If

        strVal &= String.Format("{0:yy}", dteNow)

        Return strVal
    End Function

    ''' <summary>
    ''' ブラックリスト調査報告書PDF出力データ作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mfGetHokokushoPrint()
        Dim strPrmIcBBNo As String = ""
        Dim strPrmIdBBNo As String = ""
        Dim strNow As String = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)

        For i As Integer = 0 To grvList.Rows.Count - 1
            Dim strIdIcKbn As String = ""
            Dim strTBoxId As String = CType(grvList.Rows(i).FindControl("TBOXID"), TextBox).Text
            Dim strNLKBn As String = CType(grvList.Rows(i).FindControl("KMKNLKBN"), TextBox).Text

            'ＩＤ系か、ＩＣ系かをチェック
            strIdIcKbn = mfChkICOrIDData(strTBoxId, strNLKBn)

            Select Case strIdIcKbn
                Case "ＩＣ"
                    strPrmIcBBNo &= " '" & CType(grvList.Rows(i).FindControl("BBCHOHONO"), TextBox).Text & "' "
                    strPrmIcBBNo &= ", "
                Case "ＩＤ"
                    strPrmIdBBNo &= " '" & CType(grvList.Rows(i).FindControl("BBCHOHONO"), TextBox).Text & "' "
                    strPrmIdBBNo &= ", "
            End Select

        Next

        Dim objTblIc As DataTable = Nothing
        Dim objTblId As DataTable = Nothing

        If strPrmIcBBNo <> "" Then
            strPrmIcBBNo = strPrmIcBBNo.Substring(0, strPrmIcBBNo.Length - 2)
            'IC系伝票の印刷
            objTblIc = mfGetPrintIcData(strPrmIcBBNo)

        End If

        If strPrmIdBBNo <> "" Then
            strPrmIdBBNo = strPrmIdBBNo.Substring(0, strPrmIdBBNo.Length - 2)
            'ID系伝票の印刷
            objTblId = mfGetPrintIdData(strPrmIdBBNo)

        End If

        Dim rptIc As WATREP002 = Nothing
        Dim rptId As WATREP004 = Nothing

        If objTblIc Is Nothing AndAlso Not objTblId Is Nothing Then
            rptId = New WATREP004()
            psPrintPDF(Me, rptId, objTblId, "IC系 ブラックボックス調査管理報告書")
        ElseIf Not objTblIc Is Nothing AndAlso objTblId Is Nothing Then
            rptIc = New WATREP002()
            psPrintPDF(Me, rptIc, objTblIc, "IC系 ブラックボックス調査管理報告書")
        ElseIf Not objTblIc Is Nothing AndAlso Not objTblId Is Nothing Then
            rptIc = New WATREP002()
            rptId = New WATREP004()
            psPrintPDF(Me, {rptIc, rptId}, {objTblIc, objTblId}, {"IC系 ブラックボックス調査管理報告書", "ID系 ブラックボックス調査管理報告書"})
        Else
            psMesBox(Me, "00008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Return
        End If

        'For i As Integer = 0 To grvList.Rows.Count - 1
        '    Dim strNow As String = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now)
        '    Dim strIdIcKbn As String = ""
        '    Dim strTBoxId As String = CType(grvList.Rows(i).FindControl("TBOXID"), TextBox).Text
        '    Dim strNLKBn As String = CType(grvList.Rows(i).FindControl("KMKNLKBN"), TextBox).Text
        '    Dim strBBChohoNo As String = CType(grvList.Rows(i).FindControl("BBCHOHONO"), TextBox).Text

        '    'ＩＤ系か、ＩＣ系かをチェック
        '    strIdIcKbn = mfChkICOrIDData(strTBoxId, strNLKBn)

        '    Select Case strIdIcKbn
        '        Case "ＩＣ"
        '            'IC系伝票の印刷
        '            msPrintIc(strBBChohoNo, strNow & "_")
        '        Case "ＩＤ"
        '            'ID系伝票の印刷
        '            msPrintId(strBBChohoNo, strNow & "_")
        '    End Select
        'Next

    End Sub

    ''' <summary>
    ''' IC系伝票の印刷
    ''' </summary>
    ''' <param name="strBBChohoNo"></param>
    ''' <remarks></remarks>
    Private Function mfGetPrintIcData(ByVal strBBChohoNo As String) As DataTable
        Dim objDataTbl As DataTable = Nothing

        'データ取得
        objDataTbl = mfGetPrintData_IC(strBBChohoNo)

        If objDataTbl Is Nothing OrElse objDataTbl.Rows.Count <= 0 Then
            Return Nothing
        End If

        '和暦に編集
        For rowCnt As Integer = 0 To objDataTbl.Rows.Count - 1
            Dim objRow As DataRow = objDataTbl.Rows(rowCnt)
            Dim dteWareki As DateTime = Convert.ToDateTime(objRow("和暦日付型"))
            objRow("和暦") = mfGetWareki(dteWareki)
        Next

        '必要ない列を除去
        objDataTbl.Columns.Remove("和暦日付型")

        Return objDataTbl

    End Function

    ''' <summary>
    ''' ID系伝票の印刷
    ''' </summary>
    ''' <param name="strBBChohoNo"></param>
    ''' <remarks></remarks>
    Private Function mfGetPrintIdData(ByVal strBBChohoNo As String) As DataTable
        Dim objDataTbl As DataTable = Nothing

        'データ取得
        objDataTbl = mfGetPrintData_ID(strBBChohoNo)

        If objDataTbl Is Nothing OrElse objDataTbl.Rows.Count <= 0 Then
            Return Nothing
        End If

        '和暦に編集
        For rowCnt As Integer = 0 To objDataTbl.Rows.Count - 1
            Dim objRow As DataRow = objDataTbl.Rows(rowCnt)
            Dim dteWareki As DateTime = Convert.ToDateTime(objRow("和暦日付型"))

            objRow("和暦") = mfGetWareki(dteWareki)
        Next

        '必要ない列を除去
        objDataTbl.Columns.Remove("和暦日付型")

        Return objDataTbl

    End Function

    ''' <summary>
    ''' 和暦取得
    ''' </summary>
    ''' <param name="dteWareki"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetWareki(ByVal dteWareki As DateTime) As String
        Dim objCal As New System.Globalization.JapaneseCalendar()
        Dim intEra As Integer = objCal.GetEra(dteWareki)
        Dim strEra As String = ""
        Select Case intEra
            Case 4
                strEra = "平成"
            Case 3
                strEra = "昭和"
            Case 2
                strEra = "大正"
            Case 1
                strEra = "明治"
            Case Else
                strEra = ""
        End Select

        'Return strEra & objCal.GetYear(dteWareki) & "年" & objCal.GetMonth(dteWareki) & "月" & objCal.GetDayOfMonth(dteWareki) & "日"
        Return dteWareki.ToString("yyyy") & "年" & objCal.GetMonth(dteWareki) & "月" & objCal.GetDayOfMonth(dteWareki) & "日"

    End Function

    ''' <summary>
    ''' ＩＣ帳票データ取得
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintData_IC(ByVal strVal As String) As DataTable
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        objStack = New StackFrame

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return Nothing
            End If

            'ストアドプロシージャ設定(ＩＣ帳票データ取得SQL)
            objCmd = New SqlCommand("BBPLSTP001_S5")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD50_BBREP_NO", SqlDbType.NVarChar, strVal))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            Return objDs.Tables(0)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return Nothing
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Function

    ''' <summary>
    ''' ＩＤ帳票データ取得
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetPrintData_ID(ByVal strVal As String) As DataTable
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        objStack = New StackFrame

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return Nothing
            End If

            'ストアドプロシージャ設定(ＩＤ帳票データ取得SQL)
            objCmd = New SqlCommand("BBPLSTP001_S6")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmD50_BBREP_NO", SqlDbType.NVarChar, strVal))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            Return objDs.Tables(0)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return Nothing
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Function

    ''' <summary>
    ''' ＩＣ/ＩＤデータチェック
    ''' </summary>
    ''' <param name="strTboxId"></param>
    ''' <param name="strNlKbn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfChkICOrIDData(ByVal strTboxId As String, ByVal strNlKbn As String) As String
        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        objStack = New StackFrame

        Try
            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Return ""
            End If

            'ストアドプロシージャ設定(IC/IDチェック)
            objCmd = New SqlCommand("BBPLSTP001_S4")
            objCmd.Connection = objCon
            objCmd.CommandType = CommandType.StoredProcedure

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, strTboxId))
                .Add(pfSet_Param("prmT03_NL_CLS", SqlDbType.NVarChar, strNlKbn))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            Dim strRetVal As String = ""
            If objDs.Tables(0).Rows.Count > 0 Then
                Return Convert.ToString(objDs.Tables(0).Rows(0)("ＩＤ_ＩＣ区分"))
            Else
                Return ""
            End If

        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return ""
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(objCon) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Function

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

        objTbl.Columns.Remove("最大件数")

        Return objWkTbl

    End Function

    ''' <summary>
    ''' オブジェクト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDBNull(ByVal strVal As String) As Object
        If strVal.Trim() = "" Then
            Return DBNull.Value
        End If
        Return strVal

    End Function

    Private Sub msCheck_TboxId(ByRef strFromTboxId As String,
                               ByRef strToTboxId As String)

        'エラーメッセージ
        Dim strError As String = Nothing

        'From To検索時のチェック
        If txtSrcTboxIdFrTo.ppFromText <> String.Empty And txtSrcTboxIdFrTo.ppToText <> String.Empty Then

            strFromTboxId = ("00000000" + txtSrcTboxIdFrTo.ppFromText).Substring(txtSrcTboxIdFrTo.ppFromText.Length)
            strToTboxId = ("00000000" + txtSrcTboxIdFrTo.ppToText).Substring(txtSrcTboxIdFrTo.ppToText.Length)

            strError = pfCheck_TxtFTErr(strFromTboxId, strToTboxId, False)
            If strError <> "" Then
                Me.txtSrcTboxIdFrTo.psSet_ErrorNo(strError, "ＴＢＯＸＩＤ")
            End If

            'To検索の場合
        ElseIf txtSrcTboxIdFrTo.ppFromText = String.Empty And txtSrcTboxIdFrTo.ppToText <> String.Empty Then

            strFromTboxId = txtSrcTboxIdFrTo.ppFromText
            strToTboxId = ("00000000" + txtSrcTboxIdFrTo.ppToText).Substring(txtSrcTboxIdFrTo.ppToText.Length)

        Else
            strFromTboxId = txtSrcTboxIdFrTo.ppFromText
            strToTboxId = txtSrcTboxIdFrTo.ppToText

        End If

    End Sub

    ''' <summary>
    ''' TBOXID存在チェック
    ''' </summary>
    ''' <param name="strTboxId"></param>
    ''' <remarks></remarks>
    Private Sub msChk_Tboxid(ByVal strTboxId As String, ByRef blnResult As Boolean)

        Dim objCmd As SqlCommand = Nothing
        Dim objCon As SqlConnection = Nothing

        objStack = New StackFrame

        blnResult = False

        If strTboxId = String.Empty Then
            blnResult = True
            Exit Sub
        End If

        Try

            '開始ログ出力
            psLogStart(Me)

            'DB接続
            If clsDataConnect.pfOpen_Database(objCon) = False Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Sub
            End If

            'プロシージャ設定(TBOXデータ検索)
            objCmd = New SqlCommand("BBPLSTP001_S3")
            objCmd.CommandType = CommandType.StoredProcedure
            objCmd.Connection = objCon

            'パラメータ設定
            With objCmd.Parameters
                .Add(pfSet_Param("prmT03_TBOXID", SqlDbType.NVarChar, strTboxId))
            End With

            'SQL実行
            Dim objDs As DataSet = clsDataConnect.pfGet_DataSet(objCmd)

            '取得エラー
            If objDs Is Nothing OrElse objDs.Tables.Count = 0 Then
                Throw New Exception()
            End If

            '存在しない場合エラー
            Dim objTbl As DataTable = objDs.Tables(0)
            If objTbl.Rows.Count > 0 Then
                blnResult = True
            Else
                'txtTboxId.psSet_ErrorNo("2002", "入力されたTBOXID")
                'txtTboxId.ppTextBox.Focus()
            End If

        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOX情報")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally

            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Sub
    ''' <summary>
    ''' 入力チェック
    ''' </summary>
    ''' <param name="blnResult"></param>
    ''' <remarks></remarks>
    Private Sub msGamen_Check(ByVal blnResult As Boolean)

        Dim strErr As String = String.Empty

        'TBOXID
        If blnResult = False Then
            txtTboxId.psSet_ErrorNo("2002", "入力されたTBOXID")
            txtTboxId.ppTextBox.Focus()
        End If

        strErr = pfCheck_TxtErr(txtTboxId.ppText, True, False, False, False, 8, "", False)
        If strErr <> String.Empty Then

            txtTboxId.psSet_ErrorNo(strErr, txtTboxId.ppName)
            txtTboxId.ppTextBox.Focus()

        End If


        ''修理依頼No.
        ''strErr = pfCheck_TxtErr(txtSyuriIraiNo.ppText, True, False, False, False, 8, "[a-zA-Z][0-9][0-9][0-9][0-9][0-9][0-9][0-9]", False)  'BBPLSTP001-004
        'strErr = pfCheck_TxtErr(txtSyuriIraiNo.ppText, True, False, False, False, 8, "[0-9a-zA-Z][0-9][0-9][0-9][0-9][0-9][0-9][0-9]", False)  'BBPLSTP001-004
        'If strErr <> String.Empty Then

        '    Select Case strErr
        '        Case "5001"
        '            txtSyuriIraiNo.psSet_ErrorNo(strErr, txtSyuriIraiNo.ppName)
        '            txtSyuriIraiNo.ppTextBox.Focus()
        '        Case "4001"
        '            txtSyuriIraiNo.psSet_ErrorNo(strErr, "先頭1桁", "英字")
        '            txtSyuriIraiNo.ppTextBox.Focus()
        '    End Select

        'End If

    End Sub

#End Region

End Class
