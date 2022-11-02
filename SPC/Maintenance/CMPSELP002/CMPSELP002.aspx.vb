'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守対応依頼書ＭＣ
'*　ＰＧＭＩＤ：　CMPSELP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.10.08　：　ＳＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive
#End Region

Public Class CMPSELP002
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
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_SEL & P_PAGE & "002"

    '画面ID
    Const M_MNT_DISP_ID = P_FUN_CMP & P_SCR_LST & P_PAGE & "001"    '保守対応依頼書一覧
    Const M_REPR_DISP_ID = P_FUN_REP & P_SCR_UPD & P_PAGE & "002"   '修理依頼書
    Const M_TRBL_DISP_ID = P_FUN_REQ & P_SCR_SEL & P_PAGE & "001"   'トラブル処理票
    Const M_BRNG_DISP_ID = P_FUN_REQ & P_SCR_LST & P_PAGE & "002"   '持参物品一覧
    Const M_SMNT_DISP_ID = P_FUN_CMP & P_SCR_INQ & P_PAGE & "001"   '特別保守費用照会
    Const M_CTI_DISP_ID = P_FUN_CTI & P_SCR_SEL & P_PAGE & "005"    'CTI情報(作業者)
    Const M_CHST_DISP_ID = P_FUN_BRK & P_SCR_INQ & P_PAGE & "001"   '対応履歴照会

    '修理依頼書ファイルパス
    Const M_REPR_DISP_PATH = "~/" & P_RPE & "/" &
            P_FUN_REP & P_SCR_UPD & P_PAGE & "002" & "/" &
            P_FUN_REP & P_SCR_UPD & P_PAGE & "002.aspx"
    'トラブル処理票ファイルパス
    Const M_TRBL_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_REQ & P_SCR_SEL & P_PAGE & "001.aspx"
    '持参物品一覧ファイルパス
    Const M_BRNG_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_LST & P_PAGE & "002" & "/" &
            P_FUN_REQ & P_SCR_LST & P_PAGE & "002.aspx"

    '作業状況のステータスコード"08"
    Const M_WORK_STSCD = "08"

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
    Dim objStack As StackFrame
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
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtStatus As New DataTable

        If Not IsPostBack Then  '初回表示のみ
            'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = "保守対応故障品登録"

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            msSetddlRstAppaDiv()

            Me.ddlCndAppaDiv.Enabled = False
            Me.ddlRstAppaModel.Enabled = False

            ddlStatus.Items.Add("")
            ddlStatus.Items.Add("1:故障")
            ddlStatus.Items.Add("2:調査中")
            ddlStatus.Items.Add("3:TOK")

            '画面間パラメータ
            Dim strPkey() As String = Session(P_KEY)

            If Not strPkey Is Nothing Then
                lblMntNo.Text = strPkey(0)

                mfGetTboxInfo(strPkey(0))

                SetData()
                msSetActivecontrol()
            End If
        End If
    End Sub

    Private Sub msSetddlRstAppaDiv()
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("CMPSELP002_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appa_cd", SqlDbType.NVarChar, "01"))
                End With
                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                'ドロップダウンリスト設定
                Me.ddlRstAppaDiv.Items.Clear()
                Me.ddlRstAppaDiv.DataSource = objDs.Tables(0)
                Me.ddlRstAppaDiv.DataTextField = "名称"
                Me.ddlRstAppaDiv.DataValueField = "機器分類コード"
                Me.ddlRstAppaDiv.DataBind()
                Me.ddlRstAppaDiv.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類マスタ一覧取得")
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
    ''' 各コントロールの活性・非活性
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetActivecontrol()
        Select Case Session(P_SESSION_TERMS)
            Case ClsComVer.E_遷移条件.参照
                Me.ddlRstAppaDiv.Enabled = False
                Me.ddlCndAppaDiv.Enabled = False
                Me.ddlRstAppaModel.Enabled = False
                Me.ddlStatus.Enabled = False
                Me.btnDetailInsert.Enabled = False
                Me.btnDetailUpdate.Enabled = False
                Me.btnDetailDelete.Enabled = False

            Case Else
                Me.ddlRstAppaDiv.Enabled = True
                Me.ddlStatus.Enabled = True
                Me.btnDetailInsert.Enabled = True
                Me.btnDetailUpdate.Enabled = False
                Me.btnDetailDelete.Enabled = False
        End Select

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器種別マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppacls(ByRef pddlLst As DropDownList, ByVal pAppaDiv As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("CMPSELP002_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, pAppaDiv))
                    .Add(pfSet_Param("systemcd", SqlDbType.NVarChar, lblTboxType.Text.Substring(0, 2)))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                pddlLst.Items.Clear()
                pddlLst.DataSource = objDs.Tables(0)
                pddlLst.DataTextField = "機器種別名"
                pddlLst.DataValueField = "機器種別コード"
                pddlLst.DataBind()
                pddlLst.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別マスタ一覧取得")
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
    ''' ドロップダウンリスト設定（機器マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppamodel()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("CMPSELP002_S4", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    '機器種別コード
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, Me.ddlCndAppaDiv.SelectedValue))
                    'システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.lblTboxType.Text.Split(":")(0)))
                    '保守管理番号
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, Me.lblMntNo.Text))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Me.ddlRstAppaModel.Items.Clear()    'データが無い場合は初期化
                    Me.ddlRstAppaModel.Enabled = False  'データが無い場合は非活性
                Else
                    'ドロップダウンリスト設定
                    Me.ddlRstAppaModel.Items.Clear()
                    Me.ddlRstAppaModel.DataSource = objDs.Tables(0)
                    Me.ddlRstAppaModel.DataTextField = "型式機器"
                    Me.ddlRstAppaModel.DataValueField = "型式機器コード"
                    Me.ddlRstAppaModel.DataBind()
                    Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                    Me.ddlRstAppaModel.Enabled = True
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器マスタ一覧取得")
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
    ''' 機器分類ドロップダウンリスト（明細）変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlRstAppaDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRstAppaDiv.SelectedIndexChanged
        'ドロップダウンリスト設定
        msSetddlAppacls(Me.ddlCndAppaDiv, Me.ddlRstAppaDiv.SelectedValue)   '機器種別

        If ddlCndAppaDiv.Items.Count > 0 Then
            Me.ddlCndAppaDiv.Enabled = True
        Else
            Me.ddlCndAppaDiv.Enabled = False
        End If

    End Sub

    ''' <summary>
    ''' 機器分類マスタドロップダウンリスト（検索条件）変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlCndAppaDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCndAppaDiv.SelectedIndexChanged
        'ドロップダウンリスト設定
        msSetddlAppamodel()   '機器型式
        If ddlRstAppaModel.Items.Count > 0 Then
            Me.ddlRstAppaModel.Enabled = True
        Else
            Me.ddlRstAppaModel.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' ＴＢＯＸ情報取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTboxInfo(strMntNo As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfGetTboxInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                '--保守対応データの取得
                objCmd = New SqlCommand("CMPSELP001_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)
                '取得したデータを表示（テキストボックスに設定）
                If objDs Is Nothing Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守対応故障品")
                    Exit Function
                Else
                    dtRow = objDs.Tables(0).Rows(0)
                    Me.lblHallNm.Text = dtRow("ホール名").ToString
                    Me.lblTboxID.Text = dtRow("ＴＢＯＸＩＤ").ToString
                    Me.lblTboxType.Text = dtRow("システムコード").ToString + ":" + dtRow("ＴＢＯＸタイプ").ToString
                    Me.lblTboxVer.Text = dtRow("ＶＥＲ").ToString
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "ＴＢＯＸ情報取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    mfGetTboxInfo = False
                End If
            End Try
        End If
    End Function


    ''' <summary>
    ''' グリッドの選択ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim rowData As GridViewRow = Nothing        'ボタン押下行

        If e.CommandName <> "btnSelect" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '保守対応明細データを編集エリアに表示
        '--保守管理連番
        Me.ddlRstAppaDiv.SelectedValue = CType(rowData.FindControl("機器分類"), TextBox).Text.Split(":"c)(0)
        msSetddlAppacls(ddlCndAppaDiv, CType(rowData.FindControl("機器分類"), TextBox).Text.Split(":"c)(0))
        Me.ddlCndAppaDiv.SelectedValue = CType(rowData.FindControl("機器種別"), TextBox).Text.Split(":"c)(0)
        msSetddlAppamodel()
        Me.ddlRstAppaModel.SelectedValue = CType(rowData.FindControl("機器"), TextBox).Text
        Me.ddlStatus.SelectedIndex = Integer.Parse(CType(rowData.FindControl("状態区分"), TextBox).Text.Split(":"c)(0))
        lblSno.Text = CType(rowData.FindControl("連番"), TextBox).Text


        'ボタンを活性、非活性
        If Session(P_SESSION_TERMS) <> ClsComVer.E_遷移条件.参照 Then
            Me.ddlRstAppaDiv.Enabled = True
            Me.ddlCndAppaDiv.Enabled = True
            Me.ddlRstAppaModel.Enabled = True
            Me.ddlStatus.Enabled = True

            Me.btnDetailInsert.Enabled = False
            Me.btnDetailUpdate.Enabled = True
            Me.btnDetailDelete.Enabled = True
        Else
            Me.ddlRstAppaDiv.Enabled = False
            Me.ddlCndAppaDiv.Enabled = False
            Me.ddlRstAppaModel.Enabled = False
            Me.ddlStatus.Enabled = False

            Me.btnDetailClear.Enabled = True
            Me.btnDetailInsert.Enabled = False
            Me.btnDetailUpdate.Enabled = False
            Me.btnDetailDelete.Enabled = False
        End If
        '対応コードにフォーカス
        Me.ddlRstAppaDiv.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub


    Protected Sub btnDetailInsert_Click(sender As Object, e As EventArgs) Handles btnDetailInsert.Click
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strBuff As String = String.Empty

        If fnCheckInput() = False Then
            Exit Sub
        End If

        objStack = New StackFrame

        '保守対応データ更新
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_I1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, Me.lblMntNo.Text))
                    .Add(pfSet_Param("sys_cod", SqlDbType.NVarChar, Me.lblTboxType.Text.Split(":"c)(0)))
                    .Add(pfSet_Param("sys_nam", SqlDbType.NVarChar, Me.lblTboxType.Text.Split(":"c)(1)))
                    .Add(pfSet_Param("tbx_ver", SqlDbType.NVarChar, Me.lblTboxVer.Text))
                    .Add(pfSet_Param("apc_cod", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    .Add(pfSet_Param("app_cod", SqlDbType.NVarChar, Me.ddlCndAppaDiv.SelectedValue))
                    .Add(pfSet_Param("mdl_cod", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedValue))
                    .Add(pfSet_Param("mdl_nam", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedItem.Text))
                    .Add(pfSet_Param("sts_dvs", SqlDbType.NVarChar, Me.ddlStatus.Text.Split(":"c)(0)))
                    .Add(pfSet_Param("user", SqlDbType.NVarChar, User.Identity.Name))
                End With

                'データ更新
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                    sbClearItem()
                    SetData()

                    'ボタンを活性、非活性
                    Me.btnDetailClear.Enabled = True
                    Me.btnDetailInsert.Enabled = True
                    Me.btnDetailUpdate.Enabled = False
                    Me.btnDetailDelete.Enabled = False

                    psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応故障品")
                End Using
            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応故障品")
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

    Protected Sub btnDetailUpdate_Click(sender As Object, e As EventArgs) Handles btnDetailUpdate.Click
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strBuff As String = String.Empty

        If fnCheckInput() = False Then
            Exit Sub
        End If

        objStack = New StackFrame

        '保守対応データ更新
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_U1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, Me.lblMntNo.Text))
                    .Add(pfSet_Param("mnt_sno", SqlDbType.NVarChar, lblSno.Text))
                    .Add(pfSet_Param("apc_cod", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    .Add(pfSet_Param("app_cod", SqlDbType.NVarChar, Me.ddlCndAppaDiv.SelectedValue))
                    .Add(pfSet_Param("mdl_cod", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedValue))
                    .Add(pfSet_Param("mdl_nam", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedItem.Text))
                    .Add(pfSet_Param("sts_dvs", SqlDbType.NVarChar, Me.ddlStatus.Text.Split(":"c)(0)))
                    .Add(pfSet_Param("user", SqlDbType.NVarChar, User.Identity.Name))
                End With

                'データ更新
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                    sbClearItem()
                    SetData()

                    'ボタンを活性、非活性
                    Me.btnDetailClear.Enabled = True
                    Me.btnDetailInsert.Enabled = True
                    Me.btnDetailUpdate.Enabled = False
                    Me.btnDetailDelete.Enabled = False

                    psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応故障品")
                End Using
            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応故障品")
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

    Protected Sub btnDetailDelete_Click(sender As Object, e As EventArgs) Handles btnDetailDelete.Click
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strBuff As String = String.Empty

        objStack = New StackFrame

        '保守対応データ更新
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_D1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("mnt_no", SqlDbType.NVarChar, Me.lblMntNo.Text))
                    .Add(pfSet_Param("mnt_sno", SqlDbType.NVarChar, lblSno.Text))
                    .Add(pfSet_Param("user", SqlDbType.NVarChar, User.Identity.Name))
                End With

                'データ更新
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()
                    'コミット
                    conTrn.Commit()
                    sbClearItem()
                    SetData()

                    'ボタンを活性、非活性
                    Me.btnDetailClear.Enabled = True
                    Me.btnDetailInsert.Enabled = True
                    Me.btnDetailUpdate.Enabled = False
                    Me.btnDetailDelete.Enabled = False

                    psMesBox(Me, "00002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "保守対応故障品")
                End Using
            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応故障品")
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

    Protected Sub btnDetailClear_Click(sender As Object, e As EventArgs) Handles btnDetailClear.Click
        sbClearItem()

        'ボタンを活性、非活性
        Me.btnDetailClear.Enabled = True
        Me.btnDetailInsert.Enabled = True
        Me.btnDetailUpdate.Enabled = False
        Me.btnDetailDelete.Enabled = False
    End Sub

    Private Sub sbClearItem()
        '保守対応明細データを編集エリアに表示
        '--保守管理連番
        Me.ddlRstAppaDiv.SelectedIndex = 0
        If ddlCndAppaDiv.Items.Count > 0 Then
            Me.ddlCndAppaDiv.SelectedIndex = 0
        End If
        If ddlRstAppaModel.Items.Count > 0 Then
            Me.ddlRstAppaModel.SelectedIndex = 0
        End If
        Me.ddlStatus.SelectedIndex = 0
        lblSno.Text = 0

        Me.ddlRstAppaDiv.Enabled = True
        Me.ddlCndAppaDiv.Enabled = False
        Me.ddlRstAppaModel.Enabled = False
        Me.ddlStatus.Enabled = True
    End Sub

    Private Function fnCheckInput() As Boolean
        Dim dtrErrMes As DataRow = Nothing

        fnCheckInput = False

        If Me.ddlRstAppaDiv.SelectedIndex <= 0 Then
            dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "機器分類")
            cuvRstAppaDiv.Text = "未入力エラー"
            cuvRstAppaDiv.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cuvRstAppaDiv.Enabled = True
            cuvRstAppaDiv.IsValid = False
            cuvRstAppaDiv.SetFocusOnError = True
        End If
        If Me.ddlCndAppaDiv.SelectedIndex <= 0 Then
            dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "機器分類")
            cuvCndAppaDiv.Text = "未入力エラー"
            cuvCndAppaDiv.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cuvCndAppaDiv.Enabled = True
            cuvCndAppaDiv.IsValid = False
            cuvCndAppaDiv.SetFocusOnError = True
        End If
        If Me.ddlRstAppaModel.SelectedIndex <= 0 Then
            dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "機器分類")
            cuvRstAppaModel.Text = "未入力エラー"
            cuvRstAppaModel.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cuvRstAppaModel.Enabled = True
            cuvRstAppaModel.IsValid = False
            cuvRstAppaModel.SetFocusOnError = True
        End If
        If Me.ddlStatus.SelectedIndex <= 0 Then
            dtrErrMes = ClsCMCommon.pfGet_ValMes("5003", "機器分類")
            cuvStatus.Text = "未入力エラー"
            cuvStatus.ErrorMessage = dtrErrMes.Item(P_VALMES_MES)
            cuvStatus.Enabled = True
            cuvStatus.IsValid = False
            cuvStatus.SetFocusOnError = True
        End If
        If dtrErrMes Is Nothing Then
            fnCheckInput = True
        End If
    End Function

    Private Function SetData() As Boolean
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        SetData = False
        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try

                '--保守対応明細データの取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, lblMntNo.Text))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                SetData = True
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守対応故障品")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If
    End Function
#End Region

End Class
