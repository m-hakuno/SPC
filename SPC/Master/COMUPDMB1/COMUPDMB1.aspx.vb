'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　保守用ＴＢＯＸマスタ
'*　ＰＧＭＩＤ：　COMUPDMB1
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2017.06.27　：　伯野
'********************************************************************************************************************************

'============================================================================================================================
'=　インポート定義
'============================================================================================================================
#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
#End Region

Public Class COMUPDMB1

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
#Region "定数定義"

    Const DispCode As String = "COMUPDMB1"
    Const MasterName As String = "保守機ＴＢＯＸマスタ"
    Const TableName_MB1 As String = "MB1_REP_TBOXID"     'テーブル名

#End Region

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect
    Dim objStack As StackFrame
    Dim clsExc As New ClsCMExclusive
    Dim strMode As String = Nothing
    Dim clsMst As New ClsMSTCommon

#End Region

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#Region "プロパティ定義"
#End Region

    '============================================================================================================================
    '=　イベントプロシージャ定義
    '============================================================================================================================
#Region "イベントプロージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        pfSet_GridView(Me.grvList, DispCode, "L")
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        AddHandler Master.ppBtnSearch.Click, AddressOf btnSearch_Click          '検索
        AddHandler Master.ppBtnSrcClear.Click, AddressOf btnSearchClear_Click   '検索条件クリア
        AddHandler Master.ppBtnInsert.Command, AddressOf sEditAreaBtn_Click     '登録
        AddHandler Master.ppBtnUpdate.Command, AddressOf sEditAreaBtn_Click     '更新
        AddHandler Master.ppBtnDelete.Command, AddressOf sEditAreaBtn_Click     '削除
        AddHandler Master.ppBtnClear.Click, AddressOf sbtnClear_Click           'クリア
        AddHandler txtTBOXID.ppTextBox.TextChanged, AddressOf stxtTBOXID_TextChanged
        txtTBOXID.ppTextBox.AutoPostBack = True

        If Not IsPostBack Then
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()

            Call msGet_Data()
            Call sGetTBOXCls()
            Call sGetNLCls()

            Master.ppBtnClear.Enabled = True
            Master.ppBtnInsert.Enabled = True
            Master.ppBtnUpdate.Enabled = False
            Master.ppBtnDelete.Enabled = False

        End If

    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

    End Sub

    ''' <summary>
    ''' 検索ボタン　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearch_Click(sender As Object, e As EventArgs)

        'ログ出力開始
        psLogStart(Me)

        'データ取得
        If (Page.IsValid) Then
            msGet_Data(Me.txtSrchTBOXID.ppText, Me.ddlSrchNLCls.ppSelectedValue, Me.ddlSrchTBOX.ppSelectedValue)
            'フォーカス設定
            Me.txtSrchTBOXID.ppTextBox.Focus()
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリア　押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSearchClear_Click(sender As Object, e As EventArgs)
        'ログ出力開始
        psLogStart(Me)

        Me.txtSrchTBOXID.ppText = ""
        Me.ddlSrchNLCls.ppSelectedValue = ""
        Me.ddlSrchTBOX.ppSelectedValue = ""

        'フォーカス設定
        Me.txtSrchTBOXID.ppTextBox.Focus()

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッド　行選択イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Try

            If e.CommandName = "Select" Then

                'ログ出力開始
                psLogStart(Me)

                '★排他制御処理
                Dim strExclusiveDate As String = Nothing
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList
                Dim rowData As GridViewRow = grvList.Rows(Convert.ToInt32(e.CommandArgument))

                '★排他情報削除
                If Not Me.Master.ppExclusiveDate = String.Empty Then
                    If clsExc.pfDel_Exclusive(Me _
                                       , Session(P_SESSION_SESSTION_ID) _
                                       , Me.Master.ppExclusiveDate) = 0 Then
                        Me.Master.ppExclusiveDate = String.Empty
                    Else
                        'ログ出力終了
                        psLogEnd(Me)
                        Exit Sub
                    End If
                End If

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, TableName_MB1)

                '★ロックテーブルキー項目の登録
                arKey.Insert(0, CType(rowData.FindControl("TBOXID"), Label).Text)

                '★排他情報確認処理(更新画面へ遷移)
                If clsExc.pfSel_Exclusive(strExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , DispCode _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★登録年月日時刻(明細)
                    Me.Master.ppExclusiveDate = strExclusiveDate

                Else
                    '排他ロック中
                    'ログ出力終了
                    psLogEnd(Me)
                    Exit Sub
                End If

                '編集エリアに値を設定
                Me.txtTBOXID.ppTextBox.Text = CType(rowData.FindControl("TBOXID"), Label).Text
                Me.ddlNLCls.ppDropDownList.SelectedValue = CType(rowData.FindControl("CNTR_CLS"), Label).Text
                Me.ddlTBOX.ppDropDownList.SelectedValue = CType(rowData.FindControl("TBOX_CLS"), Label).Text
                Me.txtHallNM.ppTextBox.Text = CType(rowData.FindControl("HALL_NM"), Label).Text

                '削除フラグによってボタンの文言を変更
                If CType(rowData.FindControl("DELETE_FLG"), Label).Text.Trim = "1" Then
                    Me.ddlNLCls.ppEnabled = False
                    Me.ddlTBOX.ppEnabled = False
                    Me.txtHallNM.ppEnabled = False
                    Master.ppBtnDelete.Text = "削除取消"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                    Master.ppBtnInsert.Enabled = False
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppBtnDelete.Enabled = True
                Else
                    Me.ddlNLCls.ppEnabled = True
                    Me.ddlTBOX.ppEnabled = True
                    Me.txtHallNM.ppEnabled = True
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName))       '削除
                    Master.ppBtnInsert.Enabled = False
                    Master.ppBtnUpdate.Enabled = True
                    Master.ppBtnDelete.Enabled = True
                End If

                'フォーカス設定
                If Master.ppBtnDelete.Text = "削除" Then
                    Me.ddlNLCls.ppDropDownList.Focus()
                Else
                    SetFocus(Master.ppBtnClear.ClientID)
                End If

                strMode = "Select"
                Me.txtTBOXID.ppEnabled = False

            Else
                Me.txtTBOXID.ppEnabled = True
            End If

        Catch ex As Exception
            'エラー
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            objStack = New StackFrame
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Me.txtTBOXID.ppEnabled = True
            Me.txtTBOXID.ppText = ""
            Me.ddlNLCls.ppSelectedValue = ""
            Me.ddlTBOX.ppSelectedValue = ""
            Me.txtHallNM.ppText = ""

        End Try

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 編集エリア　ボタンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub sEditAreaBtn_Click(sender As Object, e As CommandEventArgs)

        'ログ出力開始
        psLogStart(Me)

        If e.CommandName = "DELETE" Then
        Else
            Page.Validate("val")

            If Me.ddlNLCls.ppSelectedValue = "" Then
                ddlNLCls.psSet_ErrorNo("2011", "携帯電話番号2の登録", "携帯電話番号1")
            End If

            If Me.ddlTBOX.ppSelectedValue = "" Then
                ddlTBOX.psSet_ErrorNo("2011", "携帯電話番号2の登録", "携帯電話番号1")
            End If
        End If

        If (Page.IsValid) Then
            If sDataProc(e.CommandName) Then

                '値のクリア
                Me.txtTBOXID.ppText = ""
                Me.ddlNLCls.ppSelectedValue = ""
                Me.ddlTBOX.ppSelectedValue = ""
                Me.txtHallNM.ppText = ""
                'ボタンの使用可否
                Master.ppBtnInsert.Enabled = True
                Master.ppBtnUpdate.Enabled = False
                Master.ppBtnDelete.Text = "削除"
                Master.ppBtnDelete.Enabled = False

                'フォーカス設定
                Me.txtTBOXID.ppTextBox.Focus()

            End If
        End If

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' クリアボタン
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub sbtnClear_Click()

        'ログ出力開始
        psLogStart(Me)

        '★排他情報削除
        If Not Me.Master.ppExclusiveDate = String.Empty Then
            If clsExc.pfDel_Exclusive(Me _
                               , Session(P_SESSION_SESSTION_ID) _
                               , Me.Master.ppExclusiveDate) = 0 Then
                Me.Master.ppExclusiveDate = String.Empty
            Else
                'ログ出力終了
                psLogEnd(Me)
                Exit Sub
            End If
        End If

        '値のクリア
        Me.txtTBOXID.ppText = ""
        Me.ddlNLCls.ppSelectedValue = ""
        Me.ddlTBOX.ppSelectedValue = ""
        Me.txtHallNM.ppText = ""
        '使用可に変更
        Me.txtTBOXID.ppEnabled = True
        Me.ddlNLCls.ppEnabled = True
        Me.ddlTBOX.ppEnabled = True
        Me.txtHallNM.ppEnabled = True
        'ボタンの使用可否
        Master.ppBtnInsert.Enabled = True
        Master.ppBtnUpdate.Enabled = False
        Master.ppBtnDelete.Text = "削除"
        Master.ppBtnDelete.Enabled = False

        'フォーカス設定
        Me.txtTBOXID.ppTextBox.Focus()

        strMode = "Default"

        'ログ出力終了
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' ＴＢＯＸＩＤ　変更時
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub stxtTBOXID_TextChanged()

        If Me.txtTBOXID.ppTextBox.Text.Length = 8 Then
            Dim blnResult As Boolean = False
            Call sGetMB1Info(blnResult)
            If blnResult = False Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            End If
        End If

    End Sub

#End Region

    '============================================================================================================================
    '=　プロシージャ定義
    '============================================================================================================================
#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGet_Data()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        Dim delflg As String = String.Empty
        objStack = New StackFrame

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, MasterName)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                End If
                Me.grvList.DataSource = dstOrders.Tables(0)
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="ipstrtTBOXID">検索条件　ＴＢＯＸＩＤ</param>
    ''' <param name="ipstrNL_CLS">検索条件　ＮＬ区分</param>
    ''' <param name="ipstrTBOX_CLS">検索条件　ＴＢＯＸ種別</param>
    ''' <remarks></remarks>
    Private Sub msGet_Data(ByVal ipstrtTBOXID As String, ByVal ipstrNL_CLS As String, ByVal ipstrTBOX_CLS As String)

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim dispFlg As String = String.Empty
        Dim delflg As String = String.Empty
        objStack = New StackFrame

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S5", conDB)
                cmdDB.Parameters.Add("prmTBOXID", SqlDbType.NVarChar)
                cmdDB.Parameters.Add("prmNLCLS", SqlDbType.NVarChar)
                cmdDB.Parameters.Add("prmTBOXCLS", SqlDbType.NVarChar)
                cmdDB.Parameters("prmTBOXID").Value = txtSrchTBOXID.ppText
                cmdDB.Parameters("prmNLCLS").Value = ddlSrchNLCls.ppSelectedValue
                cmdDB.Parameters("prmTBOXCLS").Value = ddlSrchTBOX.ppSelectedValue

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, MasterName)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                End If
                Me.grvList.DataSource = dstOrders.Tables(0)
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.grvList.DataSource = New DataTable
                Me.grvList.DataBind()
                Master.ppCount = "0"
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If

    End Sub



    ''' <summary>
    ''' ＴＢＯＸ種別一覧　取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub sGetTBOXCls()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim objwkDST As DataSet = Nothing
        objStack = New StackFrame

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S2", conDB)

                objwkDST = clsDataConnect.pfGet_DataSet(cmdDB)

                If objwkDST.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, MasterName)
                Else
                    '検索条件
                    Me.ddlSrchTBOX.ppDropDownList.DataSource = objwkDST.Tables(0)
                    Me.ddlSrchTBOX.ppDropDownList.DataTextField = "DISP_VAL"
                    Me.ddlSrchTBOX.ppDropDownList.DataValueField = "TBOXCLS_CD"
                    Me.ddlSrchTBOX.DataBind()
                    Me.ddlSrchTBOX.ppDropDownList.Items.Insert(0, New ListItem("", ""))
                    '編集エリア
                    Me.ddlTBOX.ppDropDownList.DataSource = objwkDST.Tables(0)
                    Me.ddlTBOX.ppDropDownList.DataTextField = "DISP_VAL"
                    Me.ddlTBOX.ppDropDownList.DataValueField = "TBOXCLS_CD"
                    Me.ddlTBOX.DataBind()
                    Me.ddlTBOX.ppDropDownList.Items.Insert(0, New ListItem("", ""))
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.ddlTBOX.ppDropDownList.DataSource = New DataTable
                Me.ddlTBOX.ppDropDownList.DataBind()
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            Me.ddlTBOX.ppDropDownList.DataSource = New DataTable
            Me.ddlTBOX.ppDropDownList.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' ＮＬ区分一覧　取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub sGetNLCls()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim objwkDSC As DataSet = Nothing
        objStack = New StackFrame

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand("ZCMPSEL002", conDB)
                cmdDB.Parameters.Add("classcd", SqlDbType.NVarChar)
                cmdDB.Parameters("classcd").Value = "0128"
                objwkDSC = clsDataConnect.pfGet_DataSet(cmdDB)

                If objwkDSC.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, MasterName)
                Else
                    '検索エリア
                    Me.ddlSrchNLCls.ppDropDownList.DataSource = objwkDSC.Tables(0)
                    Me.ddlSrchNLCls.ppDropDownList.DataTextField = "M29_NAME"
                    Me.ddlSrchNLCls.ppDropDownList.DataValueField = "M29_CODE"
                    Me.ddlSrchNLCls.DataBind()
                    Me.ddlSrchNLCls.ppDropDownList.Items.Insert(0, New ListItem("", ""))
                    '編集エリア
                    Me.ddlNLCls.ppDropDownList.DataSource = objwkDSC.Tables(0)
                    Me.ddlNLCls.ppDropDownList.DataTextField = "M29_NAME"
                    Me.ddlNLCls.ppDropDownList.DataValueField = "M29_CODE"
                    Me.ddlNLCls.DataBind()
                    Me.ddlNLCls.ppDropDownList.Items.Insert(0, New ListItem("", ""))
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                Me.ddlNLCls.ppDropDownList.DataSource = New DataTable
                Me.ddlNLCls.ppDropDownList.DataBind()
            Finally
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            Me.ddlNLCls.ppDropDownList.DataSource = New DataTable
            Me.ddlNLCls.ppDropDownList.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' 明細データ　追加／更新
    ''' </summary>
    ''' <param name="ipstrCmdName">押下処理</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function sDataProc(ByVal ipstrCmdName As String) As Boolean

        sDataProc = False

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim intRtn As Integer = 0
        Dim MesCode As String = String.Empty
        Dim strProcCls As String = String.Empty
        Dim strStored As String = String.Empty
        Dim dttGrid As New DataTable
        'Dim drData As DataRow
        objStack = New StackFrame

        Select Case ipstrCmdName
            Case "INSERT"
                MesCode = "00003"
                strProcCls = "0"
                strStored = DispCode & "_I1"
            Case "UPDATE"
                MesCode = "00001"
                strProcCls = "0"
                strStored = DispCode & "_U1"
            Case "DELETE"
                Select Case Master.ppBtnDelete.Text
                    Case "削除"
                        MesCode = "00002"
                        strProcCls = "1"
                        strStored = DispCode & "_U2"
                    Case "削除取消"
                        MesCode = "00001"
                        strProcCls = "0"
                        strStored = DispCode & "_U2"
                End Select
        End Select

        '接続
        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(strStored, conDB)
                Select Case ipstrCmdName
                    Case "INSERT"
                        'パラメータ設定
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOXID", SqlDbType.NVarChar, Me.txtTBOXID.ppText.Trim))            'TBOXID
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_NL_CLS", SqlDbType.NVarChar, Me.ddlNLCls.ppSelectedValue.Trim))    'NL区分
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_SYSTEM_CLS", SqlDbType.NVarChar, Me.ddlTBOX.ppSelectedValue.Trim)) 'システム分類
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOX_CLS", SqlDbType.NVarChar, Me.ddlTBOX.ppSelectedValue.Trim))   'TBOX種別
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_HALL_NM", SqlDbType.NVarChar, Me.txtHallNM.ppText.Trim))           'ホール名
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_INSERT_USR", SqlDbType.NVarChar, User.Identity.Name.Trim))         'ユーザーＩＤ
                        cmdDB.Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                    Case "UPDATE"
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOXID", SqlDbType.NVarChar, Me.txtTBOXID.ppText.Trim))            'TBOXID
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_NL_CLS", SqlDbType.NVarChar, Me.ddlNLCls.ppSelectedValue.Trim))    'NL区分
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_SYSTEM_CLS", SqlDbType.NVarChar, Me.ddlTBOX.ppSelectedValue.Trim)) 'システム分類
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOX_CLS", SqlDbType.NVarChar, Me.ddlTBOX.ppSelectedValue.Trim))   'TBOX種別
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_HALL_NM", SqlDbType.NVarChar, Me.txtHallNM.ppText.Trim))           'ホール名
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_UPDATE_USR", SqlDbType.NVarChar, User.Identity.Name.Trim))         'ユーザーＩＤ
                        cmdDB.Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                    Case "DELETE"
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOXID", SqlDbType.NVarChar, Me.txtTBOXID.ppText.Trim))            'TBOXID
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_NL_CLS", SqlDbType.NVarChar, Me.ddlNLCls.ppSelectedValue.Trim))    'NL区分
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_SYSTEM_CLS", SqlDbType.NVarChar, Me.ddlTBOX.ppSelectedValue.Trim)) 'システム分類
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOX_CLS", SqlDbType.NVarChar, Me.ddlTBOX.ppSelectedValue.Trim))   'TBOX種別
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_HALL_NM", SqlDbType.NVarChar, Me.txtHallNM.ppText.Trim))           'ホール名
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_DELETE_FLG", SqlDbType.NVarChar, strProcCls))                      '削除フラグ
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_UPDATE_USR", SqlDbType.NVarChar, User.Identity.Name.Trim))         'ユーザーＩＤ
                        cmdDB.Parameters.Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                           '戻り値
                End Select

                'データ登録/更新/削除
                'トランザクション開始
                Using conTrn = conDB.BeginTransaction
                    Try
                        'トランザクション継承
                        cmdDB.Transaction = conTrn
                        'コマンドタイプ指定及び実行
                        cmdDB.CommandType = CommandType.StoredProcedure
                        cmdDB.ExecuteNonQuery()

                        'ストアド戻り値チェック
                        intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                        If intRtn <> 0 Then
                            conTrn.Rollback()
                            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                            Exit Function
                        End If
                        'コミット
                        conTrn.Commit()
                    Catch ex As Exception
                        'ロールバック
                        conTrn.Rollback()
                    End Try
                    '最悪でもここでロールバック
                End Using

                '追加、更新の場合は対象のレコードのみグリッドに表示
                If ipstrCmdName = "INSERT" OrElse ipstrCmdName = "UPDATE" OrElse ipstrCmdName = "DELETE" Then
                    Try
                        cmdDB = New SqlCommand(DispCode & "_S3", conDB)
                        cmdDB.Parameters.Add(pfSet_Param("prmMB1_TBOXID", SqlDbType.NVarChar, Me.txtTBOXID.ppText.Trim))            'TBOXID

                        dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                        If dstOrders.Tables(0).Rows.Count = 0 Then
                            Master.ppCount = "0"
                            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, MasterName)
                        Else
                            Master.ppCount = dstOrders.Tables(0).Rows.Count.ToString
                        End If
                        Me.grvList.DataSource = dstOrders.Tables(0)
                        Me.grvList.DataBind()

                    Catch ex As Exception
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        'ログ出力
                        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                        objStack.GetMethod.Name, "", ex.ToString, "Catch")
                        Me.grvList.DataSource = New DataTable
                        Me.grvList.DataBind()
                        Master.ppCount = "0"
                    Finally
                        If Not clsDataConnect.pfClose_Database(conDB) Then
                            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                        End If
                    End Try
                    Me.txtTBOXID.ppEnabled = True
                    Me.ddlNLCls.ppEnabled = True
                    Me.ddlTBOX.ppEnabled = True
                    Me.txtHallNM.ppEnabled = True
                    Master.ppBtnDelete.Text = "削除"
                    Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName))       '削除
                    Master.ppBtnInsert.Enabled = True
                    Master.ppBtnUpdate.Enabled = False
                    Master.ppBtnDelete.Enabled = True

                Else
                    Me.grvList.DataSource = New DataTable
                    Me.grvList.DataBind()
                    Master.ppCount = "0"
                End If

                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName)

                sDataProc = True

            Catch ex As Exception
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
        End If

    End Function

    ''' <summary>
    ''' 保守機ＴＢＯＸマスタ　該当ＴＢＯＸＩＤ設定
    ''' </summary>
    ''' <param name="opblnResult">TBOXID</param>
    ''' <remarks></remarks>
    Private Sub sGetMB1Info(ByRef opblnResult As Boolean)

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

        opblnResult = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
        Else
            Try
                objCmd = New SqlCommand("COMUPDMB1_S4", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("prmTBOXID", SqlDbType.NVarChar, Me.txtTBOXID.ppText))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables.Count > 0 Then
                    If objDs.Tables(0).Rows.Count > 0 Then

                        'ログ出力開始
                        psLogStart(Me)

                        '★排他制御処理
                        Dim strExclusiveDate As String = Nothing
                        Dim arTable_Name As New ArrayList
                        Dim arKey As New ArrayList

                        '★排他情報削除
                        If Not Me.Master.ppExclusiveDate = String.Empty Then
                            If clsExc.pfDel_Exclusive(Me, Session(P_SESSION_SESSTION_ID), Me.Master.ppExclusiveDate) = 0 Then
                                Me.Master.ppExclusiveDate = String.Empty
                            Else
                                'ログ出力終了
                                psLogEnd(Me)
                                Exit Sub
                            End If
                        End If

                        '★ロック対象テーブル名の登録
                        arTable_Name.Insert(0, TableName_MB1)

                        '★ロックテーブルキー項目の登録
                        arKey.Insert(0, objDs.Tables(0).Rows(0).Item("MB1_TBOXID"))

                        '★排他情報確認処理(更新画面へ遷移)
                        If clsExc.pfSel_Exclusive(strExclusiveDate _
                                         , Me _
                                         , Session(P_SESSION_IP) _
                                         , Session(P_SESSION_PLACE) _
                                         , Session(P_SESSION_USERID) _
                                         , Session(P_SESSION_SESSTION_ID) _
                                         , ViewState(P_SESSION_GROUP_NUM) _
                                         , DispCode _
                                         , arTable_Name _
                                         , arKey) = 0 Then

                            '★登録年月日時刻(明細)
                            Me.Master.ppExclusiveDate = strExclusiveDate
                        Else
                            '排他ロック中
                            'ログ出力終了
                            psLogEnd(Me)
                            Exit Sub
                        End If

                        '編集エリアの値設定
                        txtTBOXID.ppText = objDs.Tables(0).Rows(0).Item("MB1_TBOXID")
                        If objDs.Tables(0).Rows(0).Item("MB1_NL_CLS") Is DBNull.Value Then
                        Else
                            Me.ddlNLCls.ppSelectedValue = objDs.Tables(0).Rows(0).Item("MB1_NL_CLS")
                        End If
                        If objDs.Tables(0).Rows(0).Item("MB1_TBOX_CLS") Is DBNull.Value Then
                        Else
                            Me.ddlTBOX.ppSelectedValue = objDs.Tables(0).Rows(0).Item("MB1_TBOX_CLS")
                        End If
                        If objDs.Tables(0).Rows(0).Item("MB1_HALL_NM") Is DBNull.Value Then
                        Else
                            Me.txtHallNM.ppText = objDs.Tables(0).Rows(0).Item("MB1_HALL_NM")
                        End If
                        '削除フラグによってボタンの文言を変更
                        If objDs.Tables(0).Rows(0).Item("MB1_DELETE_FLG") = "1" Then
                            Me.txtTBOXID.ppEnabled = False
                            Me.ddlNLCls.ppEnabled = False
                            Me.ddlTBOX.ppEnabled = False
                            Me.txtHallNM.ppEnabled = False
                            Master.ppBtnDelete.Text = "削除取消"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, MasterName & "の削除取消")       '削除取消
                            Master.ppBtnInsert.Enabled = False
                            Master.ppBtnUpdate.Enabled = False
                            Master.ppBtnDelete.Enabled = True
                        Else
                            Me.ddlNLCls.ppEnabled = True
                            Me.ddlTBOX.ppEnabled = True
                            Me.txtHallNM.ppEnabled = True
                            Master.ppBtnDelete.Text = "削除"
                            Master.ppBtnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, ViewState(MasterName))       '削除
                            Master.ppBtnInsert.Enabled = False
                            Master.ppBtnUpdate.Enabled = True
                            Master.ppBtnDelete.Enabled = True
                        End If

                        '正常戻り値　設定
                        opblnResult = True
                    End If
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, "保守機ＴＢＯＸマスタ取得")
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
                'データセット　破棄
                Call psDisposeDataSet(objDs)
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                End If
            End Try
        End If

    End Sub


#End Region

End Class
