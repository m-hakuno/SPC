'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守料金確認用
'*　ＰＧＭＩＤ：　CMPLSTP003
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.10.01　：　武
'********************************************************************************************************************************

'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'CMPLSTP003-001     2015/12/15      栗原　　　変更機能を追加実装する。それに伴いレイアウトも変更する。

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

#End Region

Public Class CMPLSTP003
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
    Const sCnsErr_0001 As String = "00001"
    Const sCnsErr_0002 As String = "00002"
    Const sCnsErr_0003 As String = "00003"
    Const sCnsErr_0004 As String = "00004"
    Const sCnsErr_0005 As String = "00005"
    Const sCnsErr_30008 As String = "30008"
    'CMPLSTP003-001
    Const M_COMMAND_INS As String = "INSERT"
    Const M_COMMAND_UPD As String = "UPDATE"
    Const M_COMMAND_DEL As String = "DELETE"
    'CMPLSTP003-001 END
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
    Dim clsDataConnect As New ClsCMDataConnect
    Dim blnFirstpostback As Boolean = True
    Dim clsCMDBC As New ClsCMDBCom
    Dim clsExc As New ClsCMExclusive

    'CMPLSTP003-001
    '画面活性制御用変数
    Dim strDispMode As String
    'CMPLSTP003-001 END

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    'CMPLSTP003-001
    Private Property mpEnable() As Boolean
        Get
            Return mpEnable
        End Get
        Set(value As Boolean)
            dttCntDt.ppEnabled = value
            txtTboxid.ppEnabled = value
            ddlPre.ppEnabled = value
            ddlArea.ppEnabled = value
            ddlSystem.ppEnabled = value
            ddlVer.ppEnabled = value
        End Set
    End Property
    'CMPLSTP003-001 END
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
        pfSet_GridView(Me.grvList, "CMPLSTP003", "CMPLSTP003_Header", Me.DivOut)

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

        Try
            AddHandler Master.ppRigthButton1.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton2.Click, AddressOf Button_Click
            AddHandler Master.ppRigthButton3.Click, AddressOf Button_Click
            AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnCsv_Click

            'CMPLSTP003-001　ddlInsUpd(選択リスト｢モード｣)以外は全て、変更モード時のみ処理が走る
            AddHandler Master.ppRigthButton4.Click, AddressOf Button_Click
            AddHandler ddlInsUpd.ppDropDownList.SelectedIndexChanged, AddressOf ddlInsUpd_IndexChanged
            AddHandler ddlSystem.ppDropDownList.SelectedIndexChanged, AddressOf ddlSystem_IndexChanged
            AddHandler txtTboxid.ppTextBox.TextChanged, AddressOf txtTboxid_TextChanged
            AddHandler dttCntDt.ppDateBox.TextChanged, AddressOf dttCntDt_TextChanged
            ddlInsUpd.ppDropDownList.AutoPostBack = True
            'CMPLSTP003-001 END

            If Not IsPostBack Then  '初回表示

                blnFirstpostback = True
                Master.ppCount_Visible = False

                'セッションをビューステートに保存
                ViewState(P_KEY) = Session(P_KEY)

                'プログラムＩＤ、画面名設定
                Master.Master.ppProgramID = "CMPLSTP003"
                Master.Master.ppTitle = clsCMDBC.pfGet_DispNm("CMPLSTP003")

                'マルチビューに検索エリアを表示する
                Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                'グリッドの初期化
                Me.grvList.DataSource = New DataTable

                'CSVボタンを表示
                Master.Master.ppRigthButton1.Visible = True
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton1.Text = "ＣＳＶ"
                Master.Master.ppRigthButton1.OnClientClick = _
                pfGet_OCClickMes("10001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "ＣＳＶ")

                'CMPLSTP003-001　画面モードとドロップダウンリストの設定
                strDispMode = "Default"
                msSetDropDownList()
                'CMPLSTP003-001 END

                'キー値をセット
                Dim strKey() As String = Nothing
                strKey = ViewState(P_KEY)
                If strKey Is Nothing Then
                    strKey = {""}
                End If
                Me.dttCntDt.ppText = strKey(0)

                'ヘッダ表示
                Me.grvList.DataBind()

            Else
                blnFirstpostback = False
            End If

        Catch ex As Exception
            psMesBox(Me, sCnsErr_30008, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        End Try
    End Sub

#End Region

#Region "Page_PreRender"
    ''' <summary>
    ''' Page_PreRender
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'CMPLSTP003-001
        msSetObj()
        'CMPLSTP003-001 END

    End Sub
#End Region

#Region "ボタン押下処理"
    ''' <summary>
    ''' ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Button_Click(sender As System.Object, e As System.EventArgs)
        Try
            'CMPLSTP003-001
            '現在のモードと押下されたボタンを判別し、該当処理に進む。
            Select Case ddlInsUpd.ppSelectedValue
                Case "0"    '照会
                    Select Case sender.ID
                        Case "btnSearchRigth1"        '検索ボタン押下
                            '検索は各項目の空欄を許容
                            txtTboxid.ppRequiredField = False
                            ddlPre.ppRequiredField = False
                            ddlArea.ppRequiredField = False
                            ddlSystem.ppRequiredField = False
                            Page.Validate()
                            If Page.IsValid Then
                                msGet_Data()
                            End If
                        Case "btnSearchRigth2"        '検索クリアボタン押下
                            msClearSearch()
                    End Select

                Case "1" '変更
                    Dim strCommand As String
                    Select Case sender.ID

                        Case "btnSearchRigth1"        '削除ボタン押下
                            strCommand = M_COMMAND_DEL
                            '削除時はキー項目以外の空欄を許容
                            ddlPre.ppRequiredField = False
                            ddlArea.ppRequiredField = False
                            ddlSystem.ppRequiredField = False
                            ddlVer.ppRequiredField = False
                            Page.Validate()
                            If Page.IsValid Then
                                If mfIsErrCheck(strCommand, mfGetIsExistData()) Then
                                    msEditData(strCommand)
                                End If
                            End If

                        Case "btnSearchRigth2"        '更新ボタン押下
                            strCommand = M_COMMAND_UPD
                            If Page.IsValid Then
                                If mfIsErrCheck(strCommand, mfGetIsExistData()) Then
                                    msEditData(strCommand)
                                End If
                            End If

                        Case "btnSearchRigth3"        '登録ボタン押下
                            strCommand = M_COMMAND_INS
                            If Page.IsValid Then
                                '登録時のみ、集計日のチェックを行う
                                If mfIsCntDateCheck() Then
                                    If mfIsErrCheck(strCommand, mfGetIsExistData()) Then
                                        msEditData(strCommand)
                                    End If
                                End If
                            End If

                        Case "btnSearchRigth4"        'クリアボタン押下
                            msClearSearch()

                    End Select

            End Select
            'CMPLSTP003-001
        Catch ex As Exception
            psMesBox(Me, sCnsErr_0003, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        End Try
    End Sub
#End Region

    'CMPLSTP003-001
#Region "モード変更時処理"
    ''' <summary>
    ''' モード変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ddlInsUpd_IndexChanged(sender As Object, e As EventArgs)
        Select Case ddlInsUpd.ppSelectedValue
            Case "0"    '照会
                strDispMode = "Reference"
            Case "1"    '変更
                strDispMode = "Edit"
        End Select
        SetFocus(txtTboxid.ppTextBox.ClientID)
    End Sub
#End Region
#Region "ＴＢＯＸＩＤ変更時処理"
    ''' <summary>
    ''' TBOXID変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub txtTboxid_TextChanged(sender As Object, e As EventArgs)
        Select Case ddlInsUpd.ppSelectedValue
            Case "0"    '照会
                '照会モード時は処理無し
                Exit Sub
            Case "1"    '変更
                If dttCntDt.ppText.Trim <> String.Empty AndAlso txtTboxid.ppText.Trim <> String.Empty Then
                    'strDispModeはmsGetData()内で設定
                    msGet_Data()
                    SetFocus(ddlPre.ppDropDownList.ClientID)
                ElseIf txtTboxid.ppText.Trim = String.Empty Then
                    strDispMode = "Edit"
                    SetFocus(txtTboxid.ppTextBox.ClientID)
                Else
                    strDispMode = "Edit"
                    SetFocus(dttCntDt.ppDateBox.ClientID)
                End If
        End Select
    End Sub
#End Region
#Region "集計日変更時処理"
    ''' <summary>
    ''' 集計日変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub dttCntDt_TextChanged(sender As Object, e As EventArgs)
        Select Case ddlInsUpd.ppSelectedValue
            Case "0"    '照会
                '照会モード時は処理無し
                Exit Sub
            Case "1"    '変更
                If dttCntDt.ppText.Trim <> String.Empty AndAlso txtTboxid.ppText.Trim <> String.Empty Then
                    'strDispModeはmsGetData()内で設定
                    msGet_Data()
                    SetFocus(ddlPre.ppDropDownList.ClientID)
                ElseIf dttCntDt.ppText.Trim = String.Empty Then
                    strDispMode = "Edit"
                    SetFocus(dttCntDt.ppDateBox.ClientID)
                Else
                    strDispMode = "Edit"
                    SetFocus(txtTboxid.ppTextBox.ClientID)
                End If
        End Select
    End Sub
#End Region
#Region "システム変更時処理"
    ''' <summary>
    ''' システム変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ddlSystem_IndexChanged()
        Select Case ddlInsUpd.ppSelectedValue
            Case "0"    '照会
                '照会モードは処理無し
                Exit Sub
            Case "1"    '変更
                'msSetddlVer()内にFocus設定有
                msSetddlVer()
        End Select
    End Sub
#End Region
    'CMPLSTP003-001 END

#End Region

#Region "そのほかのプロシージャ"

#Region "条件検索取得処理"
    ''' <summary>
    ''' 条件検索取得処理
    ''' </summary>
    ''' <remarks>CMPLSTP003-001　変更モード時は明細の存在確認と画面モード設定</remarks>
    Private Sub msGet_Data()

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim intAmount As Integer = 0

        Try
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If

            cmdDB = New SqlCommand("CMPLSTP003_S1", conDB)

            'CMPLSTP003-001
            Select Case ddlInsUpd.ppSelectedValue
                Case "0"        '照会
                    Me.grvList.DataSource = Nothing
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Cntdt", SqlDbType.NVarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                        .Add(pfSet_Param("Tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText.ToString))
                        .Add(pfSet_Param("Systemcd", SqlDbType.NVarChar, Me.ddlSystem.ppSelectedValue.ToString))
                        .Add(pfSet_Param("Precd", SqlDbType.NVarChar, Me.ddlPre.ppSelectedValue.ToString))
                        .Add(pfSet_Param("Areacd", SqlDbType.NVarChar, Me.ddlArea.ppSelectedValue.ToString))
                        'CMPLSTP003-001
                        .Add(pfSet_Param("Del_Flg", SqlDbType.NVarChar, "0"))
                        'CMPLSTP003-001 END
                    End With

                    'データ取得およびデータをリストに設定
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                    Me.grvList.DataSource = dstOrders

                    '件数を設定
                    If dstOrders.Tables(0).Rows.Count > 0 Then
                        Me.lblCount.Text = dstOrders.Tables(0).Rows.Count
                    Else
                        Me.lblCount.Text = 0
                    End If

                    '金額を設定
                    If dstOrders.Tables(0).Rows.Count > 0 Then
                        For rowCnt As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                            intAmount += dstOrders.Tables(0).Rows(rowCnt).Item("集計金額").ToString()
                        Next
                        Me.lblAmount.Text = intAmount
                    Else
                        Me.lblAmount.Text = 0
                    End If

                    'カンマ区切り編集
                    lblCount.Text = CInt(lblCount.Text).ToString("#,##0")
                    lblAmount.Text = CInt(lblAmount.Text).ToString("#,##0")

                    'マルチビューに検索エリアを表示する
                    Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

                    '変更を反映
                    Me.grvList.DataBind()

                    If dstOrders.Tables(0).Rows.Count = 0 Then
                        '0件
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If

                    'CSVボタンを活性
                    Master.Master.ppRigthButton1.Enabled = True

                    'グリッド編集処理
                    msGet_GRIDEdit()

                    'CMPLSTP003-001
                Case "1"        '変更
                    With cmdDB.Parameters
                        .Add(pfSet_Param("Cntdt", SqlDbType.NVarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                        .Add(pfSet_Param("Tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText.ToString))
                        .Add(pfSet_Param("Systemcd", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("Precd", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("Areacd", SqlDbType.NVarChar, ""))
                        .Add(pfSet_Param("Del_Flg", SqlDbType.NVarChar, ""))
                    End With
                    dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                    '件数を確認し、画面モードを設定
                    If dstOrders.Tables(0).Rows.Count > 0 Then
                        strDispMode = "Edit_Upd"
                        ddlPre.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("都道府県コード")
                        ddlArea.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("エリアコード")
                        ddlSystem.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("システムコード")
                        msSetddlVer()
                        ddlVer.ppSelectedValue = dstOrders.Tables(0).Rows(0).Item("バージョン")
                        If dstOrders.Tables(0).Rows(0).Item("削除") = "1" Then
                            Master.ppRigthButton1.Text = "削除取消"
                        End If
                    Else
                        strDispMode = "Edit_Ins"
                        msSetTbox_Info(txtTboxid.ppText.Trim)
                    End If
                    'CMPLSTP003-001 END
            End Select

        Catch ex As DBConcurrencyException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        Catch ex As Exception
            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try

    End Sub

    ''' <summary>
    ''' ＣＳＶ出力処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnCsv_Click(sender As System.Object, e As System.EventArgs)

        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        Try
            Me.grvList.DataSource = Nothing

            'ＤＢ接続
            If clsDataConnect.pfOpen_Database(conDB) Then

                'パラメータ設定
                cmdDB = New SqlCommand("CMPLSTP003_S2", conDB)

                With cmdDB.Parameters
                    .Add(pfSet_Param("Cntdt", SqlDbType.NVarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                    .Add(pfSet_Param("Tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText.ToString))
                    .Add(pfSet_Param("Systemcd", SqlDbType.NVarChar, Me.ddlSystem.ppSelectedValue.ToString))
                    .Add(pfSet_Param("Precd", SqlDbType.NVarChar, Me.ddlPre.ppSelectedValue.ToString))
                    .Add(pfSet_Param("Areacd", SqlDbType.NVarChar, Me.ddlArea.ppSelectedValue.ToString))
                End With

                'データ取得およびデータをリストに設定
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
                'CSVに不要な列を削除
                dstOrders.Tables(0).Columns.Remove("ROW")

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    '0件
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細")
                    Return
                End If

                'CSV出力
                pfDLCSV("保守料金明細", Me.dttCntDt.ppText, dstOrders.Tables(0), False, Me)

            Else
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細")
        Finally
            'DB切断
            If Not clsDataConnect.pfClose_Database(conDB) Then
                psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each rowData As GridViewRow In grvList.Rows
            CType(rowData.FindControl("集計金額"), TextBox).Text = CInt(CType(rowData.FindControl("集計金額"), TextBox).Text).ToString("#,##0")
        Next

    End Sub
#End Region

#Region "グリッド編集処理"
    ''' <summary>
    ''' グリッド編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_GRIDEdit()
        Dim strHeadNm As String() = New String() _
                                    {"TBOXID", "区分", "システム", "バージョン", "ホールコード", "ホール名", _
                                     "都道府県", "保担名", "統括保担名", "集計金額", "運用開始日", _
                                     "運用終了日", "集計金額ＦＳ", "保担コード", "統括保担コード", "エリアコード"}

        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("エリアコード"), TextBox).Text.Trim = "9" _
                And CType(rowData.FindControl("保担名"), TextBox).Text.Trim = "" _
                And CType(rowData.FindControl("統括保担名"), TextBox).Text.Trim = "" _
                And CType(rowData.FindControl("保担コード"), TextBox).Text.Trim = "" _
                And CType(rowData.FindControl("統括保担コード"), TextBox).Text.Trim = "" Then
                For clmCnt As Integer = 0 To grvList.Columns.Count - 1
                    CType(rowData.FindControl(strHeadNm(clmCnt)), TextBox).ForeColor = Drawing.Color.Red
                Next
            End If
        Next
        Return True

    End Function
#End Region

    'CMPLSTP003-001
#Region "TBOX情報関連処理"
    ''' <summary>
    ''' TBOX情報取得処理
    ''' </summary>
    ''' <param name="strTboxId">検索するTBOXID</param>
    ''' <remarks></remarks>
    Protected Function mfGetTbox_Info(ByVal strTboxId As String) As DataSet
        'ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        mfGetTbox_Info = Nothing

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If
            'パラメータ設定
            cmdDB = New SqlCommand("CMPLSTP003_S3", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("Tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText.ToString))
                .Add(pfSet_Param("Cnt_YMD", SqlDbType.NVarChar, Me.dttCntDt.ppText))            '集計金額取得用
                .Add(pfSet_Param("Area", SqlDbType.NVarChar, Me.ddlArea.ppSelectedValue))       '集計金額取得用
                .Add(pfSet_Param("Sys_Cod", SqlDbType.NVarChar, Me.ddlSystem.ppSelectedValue))  '集計金額取得用
            End With

            'データ取得
            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            '件数を確認
            If dstOrders.Tables(0).Rows.Count > 0 Then
                'TBOX情報を戻り値に設定
                mfGetTbox_Info = dstOrders
            Else
                'TBOX情報を空白とし、戻り値に設定
                dstOrders.Tables(0).Rows.Add(dstOrders.Tables(0).NewRow) '空白行を追加
                dstOrders.Tables(0).Rows(0).Item("NL区分") = "N"
                dstOrders.Tables(0).Rows(0).Item("運用開始日") = DBNull.Value
                dstOrders.Tables(0).Rows(0).Item("運用終了日") = DBNull.Value
                mfGetTbox_Info = dstOrders
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        Catch ex As Exception
            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Throw
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try
    End Function
    ''' <summary>
    ''' TBOX情報セット処理
    ''' </summary>
    ''' <param name="strTboxId">検索するTBOXID</param>
    ''' <remarks></remarks>
    Protected Sub msSetTbox_Info(ByVal strTboxId As String)
        'ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If
            'パラメータ設定
            cmdDB = New SqlCommand("CMPLSTP003_S4", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("Tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText.ToString))
            End With

            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            If dstOrders.Tables(0).Rows.Count > 0 Then
                ddlPre.ppSelectedValue = ddlPre.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("都道府県コード")).Value
                ddlArea.ppSelectedValue = ddlArea.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("エリアコード")).Value
                ddlSystem.ppSelectedValue = ddlSystem.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("システムコード")).Value
                msSetddlVer()
                ddlVer.ppSelectedValue = ddlVer.ppDropDownList.Items.FindByValue(dstOrders.Tables(0).Rows(0).Item("バージョン")).Value
            Else
                ddlPre.ppSelectedValue = String.Empty
                ddlArea.ppSelectedValue = String.Empty
                ddlSystem.ppSelectedValue = String.Empty
                msSetddlVer()
                ddlVer.ppSelectedValue = String.Empty
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        Catch ex As Exception
            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
            Throw
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try
    End Sub
    ''' <summary>
    ''' 保守料金明細存在確認
    ''' </summary>
    ''' <returns>
    ''' NotyetCheck：ＤＢ接続失敗等のエラー
    ''' Exist　　　：データ存在
    ''' NoExist　　：データ無し
    ''' Delete　　 ：データ削除済
    ''' </returns>
    ''' <remarks></remarks>
    Protected Function mfGetIsExistData() As String
        mfGetIsExistData = "NotyetCheck"
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim dstOrders As New DataSet
        Dim intAmount As Integer = 0
        Dim strCNST_CLS_Chk As StringBuilder = New StringBuilder

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If
            cmdDB = New SqlCommand("CMPLSTP003_S1", conDB)
            With cmdDB.Parameters
                .Add(pfSet_Param("Cntdt", SqlDbType.NVarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                .Add(pfSet_Param("Tboxid", SqlDbType.NVarChar, Me.txtTboxid.ppText.ToString))
                .Add(pfSet_Param("Systemcd", SqlDbType.NVarChar, ""))
                .Add(pfSet_Param("Precd", SqlDbType.NVarChar, ""))
                .Add(pfSet_Param("Areacd", SqlDbType.NVarChar, ""))
                .Add(pfSet_Param("Del_Flg", SqlDbType.NVarChar, ""))
            End With

            dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)
            '件数を確認し、画面モードを設定
            If dstOrders.Tables(0).Rows.Count > 0 Then
                If dstOrders.Tables(0).Rows(0).Item("削除") = "1" Then
                    mfGetIsExistData = "Delete"
                Else
                    mfGetIsExistData = "Exist"
                End If
            Else
                mfGetIsExistData = "NoExist"
            End If

        Catch ex As DBConcurrencyException
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        Catch ex As Exception
            psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_Mモード.OK, ClsComVer.E_S実行.描画後)
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try
    End Function
#End Region
#Region "登録／更新／削除処理"
    ''' <summary>
    ''' 登録／更新／削除処理
    ''' </summary>
    ''' <param name="strCommand"></param>
    ''' <remarks></remarks>
    Protected Sub msEditData(ByVal strCommand As String)
        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As DataSet = Nothing
        Dim MesCode As String = String.Empty
        Dim dttGrid As New DataTable
        Dim dstTbox As New DataSet              'TBOX情報格納用
        Dim strStored As String = String.Empty
        Dim trans As SqlClient.SqlTransaction   'トランザクション
        Dim intRtn As Integer
        Dim strDelFlg As String = String.Empty  '削除処理時のみ使用

        Select Case strCommand
            Case M_COMMAND_INS
                MesCode = "00003"
                strStored = "CMPLSTP003_I1"
            Case M_COMMAND_UPD
                MesCode = "00001"
                strStored = "CMPLSTP003_U1"
            Case M_COMMAND_DEL
                Select Case Master.ppRigthButton1.Text
                    Case "削除"
                        MesCode = "00002"
                        strStored = "CMPLSTP003_D1"
                        strDelFlg = "1"
                    Case "削除取消"
                        MesCode = "00001"
                        strStored = "CMPLSTP003_D1"
                        strDelFlg = "0"
                End Select
        End Select

        Try
            If clsDataConnect.pfOpen_Database(conDB) Then

                'トランザクションの設定
                trans = conDB.BeginTransaction
                cmdDB = New SqlCommand(strStored, conDB)
                cmdDB.CommandType = CommandType.StoredProcedure
                cmdDB.Transaction = trans
                dstTbox = mfGetTbox_Info(txtTboxid.ppText.Trim)

                Select Case strCommand
                    Case M_COMMAND_INS
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNT_YMD", SqlDbType.VarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                            .Add(pfSet_Param("TBOX_ID", SqlDbType.VarChar, Me.txtTboxid.ppText))
                            .Add(pfSet_Param("NLJ_DVS", SqlDbType.VarChar, dstTbox.Tables(0).Rows(0).Item("NL区分").ToString))
                            .Add(pfSet_Param("SYS_COD", SqlDbType.VarChar, ddlSystem.ppSelectedValue))
                            .Add(pfSet_Param("SYS_NAM", SqlDbType.NVarChar, mfSepColon(ddlSystem.ppSelectedText, 1)))
                            .Add(pfSet_Param("SYS_VER", SqlDbType.VarChar, ddlVer.ppSelectedValue))
                            .Add(pfSet_Param("HOL_COD", SqlDbType.VarChar, dstTbox.Tables(0).Rows(0).Item("ホールコード").ToString))
                            .Add(pfSet_Param("HOL_NAM", SqlDbType.NVarChar, dstTbox.Tables(0).Rows(0).Item("ホール名").ToString))
                            .Add(pfSet_Param("PRE_COD", SqlDbType.VarChar, ddlPre.ppSelectedValue))
                            .Add(pfSet_Param("MNT_COD", SqlDbType.VarChar, dstTbox.Tables(0).Rows(0).Item("保守担当コード").ToString))
                            .Add(pfSet_Param("MNT_NAM", SqlDbType.NVarChar, dstTbox.Tables(0).Rows(0).Item("保守担当名").ToString))
                            .Add(pfSet_Param("UNF_COD", SqlDbType.VarChar, dstTbox.Tables(0).Rows(0).Item("統括保担コード").ToString))
                            .Add(pfSet_Param("UNF_NAM", SqlDbType.NVarChar, dstTbox.Tables(0).Rows(0).Item("統括保担名").ToString))
                            .Add(pfSet_Param("ARE_COD", SqlDbType.VarChar, Me.ddlArea.ppDropDownList.SelectedValue))
                            .Add(pfSet_Param("CNT_AMT", SqlDbType.Money, dstTbox.Tables(1).Rows(0).Item("集計金額").ToString))
                            .Add(pfSet_Param("SPC_AMT", SqlDbType.Money, dstTbox.Tables(1).Rows(0).Item("集計金額FS").ToString))
                            .Add(pfSet_Param("STR_YMD", SqlDbType.DateTime, dstTbox.Tables(0).Rows(0).Item("運用開始日")))
                            .Add(pfSet_Param("END_YMD", SqlDbType.DateTime, dstTbox.Tables(0).Rows(0).Item("運用終了日")))
                            .Add(pfSet_Param("CNT_DEL", SqlDbType.VarChar, mfGetCntDel))
                            .Add(pfSet_Param("CON_DVS", SqlDbType.VarChar, ""))
                            .Add(pfSet_Param("FRE_WRD", SqlDbType.VarChar, ""))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                               '戻り値
                        End With

                    Case M_COMMAND_UPD
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNT_YMD", SqlDbType.VarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                            .Add(pfSet_Param("TBOX_ID", SqlDbType.VarChar, Me.txtTboxid.ppText))
                            .Add(pfSet_Param("NLJ_DVS", SqlDbType.VarChar, dstTbox.Tables(0).Rows(0).Item("NL区分").ToString))
                            .Add(pfSet_Param("SYS_COD", SqlDbType.VarChar, ddlSystem.ppSelectedValue))
                            .Add(pfSet_Param("SYS_NAM", SqlDbType.NVarChar, mfSepColon(ddlSystem.ppSelectedText, 1)))
                            .Add(pfSet_Param("SYS_VER", SqlDbType.VarChar, ddlVer.ppSelectedValue))
                            .Add(pfSet_Param("PRE_COD", SqlDbType.VarChar, ddlPre.ppSelectedValue))
                            .Add(pfSet_Param("ARE_COD", SqlDbType.VarChar, Me.ddlArea.ppDropDownList.SelectedValue))
                            .Add(pfSet_Param("CNT_AMT", SqlDbType.Money, dstTbox.Tables(1).Rows(0).Item("集計金額").ToString))
                            .Add(pfSet_Param("SPC_AMT", SqlDbType.Money, dstTbox.Tables(1).Rows(0).Item("集計金額FS").ToString))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                        End With

                    Case M_COMMAND_DEL
                        With cmdDB.Parameters
                            .Add(pfSet_Param("CNT_YMD", SqlDbType.VarChar, Me.dttCntDt.ppText.Replace("/", "").ToString))
                            .Add(pfSet_Param("TBOX_ID", SqlDbType.VarChar, Me.txtTboxid.ppText))
                            .Add(pfSet_Param("NLJ_DVS", SqlDbType.VarChar, dstTbox.Tables(0).Rows(0).Item("NL区分").ToString))
                            .Add(pfSet_Param("DEL_FLG", SqlDbType.VarChar, strDelFlg))
                            .Add(pfSet_Param("UserId", SqlDbType.NVarChar, Session(P_SESSION_USERID)))
                            .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))                                             '戻り値
                        End With
                End Select
                cmdDB.ExecuteNonQuery()
                'ストアド戻り値チェック
                intRtn = Integer.Parse(cmdDB.Parameters("retvalue").Value.ToString)
                If intRtn <> 0 Then
                    trans.Rollback()
                    'エラーパターン(ストアドの戻り値別)
                    Select Case intRtn
                        Case 1
                            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力した集計日には、集計データが存在しない為、\n登録処理を実施出来ません。\n集計日を確認してください。")
                        Case 2
                            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "保守料金明細への登録処理に失敗しました。")
                        Case 3
                            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "保守料金集計(県別)の更新処理に失敗しました。")
                        Case 4
                            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "保守料金集計(エリア)の更新処理に失敗しました。")
                        Case 5
                            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "CSV出力用テーブルの更新処理に失敗しました。")
                    End Select
                    Exit Sub
                Else
                    trans.Commit()
                    psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
                End If
                If strCommand = M_COMMAND_DEL AndAlso Master.ppRigthButton1.Text = "削除" Then
                    '削除処理　グリッドの初期化
                    Me.grvList.DataSource = New DataTable
                    grvList.DataBind()
                    Me.lblCount.Text = 0
                    Me.lblAmount.Text = 0
                Else
                    '登録/更新/削除取消処理　データを表示する為、一時的に照会モードに変更し、検索する。
                    ddlInsUpd.ppSelectedValue = "0"
                    msGet_Data()
                    ddlInsUpd.ppSelectedValue = "1"
                End If
                msClearSearch()
            Else
                psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
            End If

        Catch ex As Exception
            psMesBox(Me, MesCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try
    End Sub
    ''' <summary>
    ''' msEditData呼び出し前の整合性チェック＆エラー表示
    ''' </summary>
    ''' <param name="strCommand"></param>
    ''' <param name="strExistType"></param>
    ''' <remarks></remarks>
    Protected Function mfIsErrCheck(ByVal strCommand As String, ByVal strExistType As String) As Boolean
        mfIsErrCheck = False
        Select Case strCommand
            Case M_COMMAND_INS
                Select Case strExistType
                    Case "NoExist"
                        mfIsErrCheck = True
                    Case "Delete"
                        psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータは既に存在していて削除されています。\nクリアボタンを押下し、TBOXIDの再入力")
                    Case "Exist"
                        psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータが既に存在しています。\nクリアボタンを押下し、TBOXIDの再入力")
                    Case Else
                        'その他例外エラー
                        psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
                End Select

            Case M_COMMAND_UPD
                Select Case strExistType
                    Case "Exist"
                        mfIsErrCheck = True
                    Case "Delete"
                        psMesBox(Me, "30011", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータ")
                    Case "NoExist"
                        psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータが存在していません。\nクリアボタンを押下し、TBOXIDの再入力")
                    Case Else
                        'その他例外エラー
                        psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
                End Select

            Case M_COMMAND_DEL
                Select Case Master.ppRigthButton1.Text
                    Case "削除"
                        Select Case strExistType
                            Case "Exist"
                                mfIsErrCheck = True
                            Case "Delete"
                                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータは既に削除されています。\nクリアボタンを押下し、TBOXIDの再入力")
                            Case "NoExist"
                                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータが存在していません。\nクリアボタンを押下し、TBOXIDの再入力")
                            Case Else
                                'その他例外エラー
                                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
                        End Select

                    Case "削除取消"
                        Select Case strExistType
                            Case "Delete"
                                mfIsErrCheck = True
                            Case "Exist"
                                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータは既に削除取消処理が行われています。\nクリアボタンを押下し、TBOXIDの再入力")
                            Case "NoExist"
                                psMesBox(Me, "30009", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "該当する集計日、TBOXIDのデータが存在していません。\nクリアボタンを押下し、TBOXIDの再入力")
                            Case Else
                                'その他例外エラー
                                psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.Master.ppTitle)
                        End Select

                End Select
            Case Else
                'その他例外エラー
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金明細の整合性チェック処理に失敗しました。")
        End Select
    End Function
    ''' <summary>
    ''' 集計日の日付チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 最終集計実施時刻以降の日付か判別する。
    ''' ・現在時刻が12時以降：当日の集計が実施されている為、前日までの集計が作成済み。
    ''' 　　1日以上前 = True
    ''' ・現在時刻が12時以前：当日の集計が未実施の為、前々日までの集計が作成済み。
    ''' 　　2日以上前 = True
    ''' </remarks>
    Protected Function mfIsCntDateCheck() As Boolean
        Try
            Dim intDay As Integer
            '現在時刻の午前午後を判別する。
            If TimeSpan.Compare(DateTime.Now.TimeOfDay, New TimeSpan(12, 0, 0)) = -1 Then
                intDay = -2 '午前中：2日前まで集計完了
            Else
                intDay = -1 '午後　：1日前まで集計完了
            End If
            If DateTime.Compare(dttCntDt.ppDate, DateTime.Now.AddDays(intDay)) = -1 Then
                '入力した集計日が、集計未実施の未来日でなければTrueとする。（集計データが存在しない程の過去日はストアド側で弾く）
                mfIsCntDateCheck = True
            Else
                psMesBox(Me, "00000", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "入力した集計日は、保守料金集計がまだ実施されていません。")
                mfIsCntDateCheck = False
            End If
        Catch ex As Exception
            mfIsCntDateCheck = False
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守料金集計データを検索時、時刻チェックに失敗しました。\n集計日を確認してください。")
        End Try

    End Function
    ''' <summary>
    ''' 集計削除に登録する値を返す(月末日の判定)
    ''' </summary>
    ''' <returns>
    ''' 0:末日
    ''' 9:末日以外
    ''' </returns>
    ''' <remarks></remarks>
    Private Function mfGetCntDel() As String
        Dim strInputDay As String = String.Empty
        Dim strLastDayOfMonth As String = String.Empty

        strInputDay = dttCntDt.ppDate.Day
        strLastDayOfMonth = Date.DaysInMonth(dttCntDt.ppDate.Year, dttCntDt.ppDate.Month)

        If strInputDay = strLastDayOfMonth Then
            mfGetCntDel = "0"
        Else
            mfGetCntDel = "9"
        End If

    End Function
#End Region

#Region "ドロップダウンリスト作成"
    ''' <summary>
    ''' ドロップダウンリスト作成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetDropDownList()
        msSetddlPre()
        msSetddlSystem()
        msSetddlVer()
    End Sub
    Private Sub msSetddlVer()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing

        If Not ddlSystem.ppSelectedValue = String.Empty Then
            'DB接続
            If Not clsDataConnect.pfOpen_Database(objCn) Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Else
                Try
                    objCmd = New SqlCommand("ZCMPSEL034", objCn)
                    objCmd.Parameters.Add(pfSet_Param("tbox_cd", SqlDbType.NVarChar, ddlSystem.ppSelectedValue))
                    'データ取得
                    objDs = clsDataConnect.pfGet_DataSet(objCmd)

                    If objDs.Tables(0).Rows.Count > 0 Then
                        'ドロップダウンリスト設定
                        Me.ddlVer.ppDropDownList.Items.Clear()
                        Me.ddlVer.ppDropDownList.DataSource = objDs.Tables(0)
                        Me.ddlVer.ppDropDownList.DataValueField = "正式バージョン"
                        Me.ddlVer.ppDropDownList.DataTextField = "正式バージョン"
                        Me.ddlVer.ppDropDownList.DataBind()
                        Me.ddlVer.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        SetFocus(ddlVer.ppDropDownList.ClientID)
                    Else
                        psMesBox(Me, "10004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "選択されたＴＢＯＸタイプには、バージョン情報")
                        Me.ddlVer.ppDropDownList.Items.Clear()
                        SetFocus(ddlSystem.ppDropDownList.ClientID)
                    End If

                Catch ex As Exception
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "バージョン一覧取得")
                Finally
                    'DB切断
                    If Not clsDataConnect.pfClose_Database(objCn) Then
                        psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    End If
                End Try
            End If
        Else
            Me.ddlVer.ppDropDownList.Items.Clear()
            Me.ddlVer.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
            SetFocus(ddlSystem.ppDropDownList.ClientID)
        End If
    End Sub
    Private Sub msSetddlSystem()
        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZMSTSEL004", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '検索結果
                Me.ddlSystem.ppDropDownList.Items.Clear()
                Me.ddlSystem.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlSystem.ppDropDownList.DataTextField = "ＴＢＯＸリスト"
                Me.ddlSystem.ppDropDownList.DataValueField = "ＴＢＯＸシステムコード"
                Me.ddlSystem.ppDropDownList.DataBind()
                Me.ddlSystem.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Sub
    Private Sub msSetddlPre()

        Dim objCn As SqlConnection = Nothing
        Dim objCmd As SqlCommand = Nothing
        Dim objDs As DataSet = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '都道府県設定
                objCmd = New SqlCommand("ZMSTSEL002", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                Me.ddlPre.ppDropDownList.Items.Clear()
                Me.ddlPre.ppDropDownList.DataSource = objDs.Tables(0)
                Me.ddlPre.ppDropDownList.DataTextField = "項目名"
                Me.ddlPre.ppDropDownList.DataValueField = "都道府県コード"
                Me.ddlPre.ppDropDownList.DataBind()
                Me.ddlPre.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "都道府県一覧取得")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub
    ''' <summary>
    ''' "："を区切り文字としてテキストを分割する(空欄なら空文字を返す)
    ''' </summary>
    ''' <param name="strText">"："入りのテキスト</param>
    ''' <param name="intSeparator">"："区切りの0:前　1:後</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfSepColon(ByVal strText As String, ByVal intSeparator As Integer) As String
        If strText.Trim <> String.Empty Then
            If strText.Contains(":") Then
                mfSepColon = strText.Split(":")(intSeparator)
            ElseIf strText.Contains("：") Then
                mfSepColon = strText.Split("：")(intSeparator)
            Else
                mfSepColon = strText
            End If
        Else
            mfSepColon = ""
        End If
    End Function
#End Region

    ''' <summary>
    ''' オブジェクト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetObj()
        '画面制御
        Select Case strDispMode
            Case "Default"
                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton3.Visible = False
                Master.ppRigthButton4.Visible = False

                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = False

                Master.ppRigthButton1.Text = "検索"
                Master.ppRigthButton2.Text = "検索条件クリア"
                Master.ppRigthButton3.Text = "登録"
                Master.ppRigthButton4.Text = "クリア"

                Master.ppRigthButton1.BackColor = Drawing.Color.Empty

                ddlInsUpd.ppEnabled = True
                mpEnable = False

                ddlVer.Visible = False

                Master.ppRigthButton3.OnClientClick = ""
                Master.ppRigthButton2.OnClientClick = ""
                Master.ppRigthButton1.OnClientClick = ""

            Case "Reference"
                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton2.Enabled = True
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = False

                Master.ppRigthButton1.Text = "検索"
                Master.ppRigthButton2.Text = "検索条件クリア"
                Master.ppRigthButton3.Text = "登録"
                Master.ppRigthButton4.Text = "クリア"

            Case "Edit" '変更モード選択直後
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = True

                Master.ppRigthButton1.Text = "削除"
                Master.ppRigthButton2.Text = "更新"
                Master.ppRigthButton3.Text = "登録"
                Master.ppRigthButton4.Text = "クリア"

                mpEnable = True

            Case "Edit_Ins" '変更モード　登録
                Master.ppRigthButton1.Enabled = False
                Master.ppRigthButton2.Enabled = False
                Master.ppRigthButton3.Enabled = True
                Master.ppRigthButton4.Enabled = True

                mpEnable = True

                Master.ppRigthButton1.Text = "削除"
                Master.ppRigthButton2.Text = "更新"
                Master.ppRigthButton3.Text = "登録"
                Master.ppRigthButton4.Text = "クリア"

                Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle)       '登録
                Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle)       '更新
                Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle)       '削除

            Case "Edit_Upd" '変更モード　更新/削除
                Master.ppRigthButton1.Enabled = True
                Master.ppRigthButton3.Enabled = False
                Master.ppRigthButton4.Enabled = True

                Master.ppRigthButton2.Text = "更新"
                Master.ppRigthButton3.Text = "登録"
                Master.ppRigthButton4.Text = "クリア"

                Master.ppRigthButton3.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle)       '登録
                Master.ppRigthButton2.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle)       '更新

                mpEnable = True

                If Master.ppRigthButton1.Text = "削除取消" Then
                    Master.ppRigthButton2.Enabled = False
                    Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle & "の削除取消")
                    ddlPre.ppEnabled = False
                    ddlArea.ppEnabled = False
                    ddlSystem.ppEnabled = False
                    ddlVer.ppEnabled = False
                Else
                    Master.ppRigthButton2.Enabled = True
                    Master.ppRigthButton1.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, Master.Master.ppTitle)       '削除
                End If
        End Select

        Select Case ddlInsUpd.ppSelectedValue
            Case "0"    '照会
                dttCntDt.ppDateBox.AutoPostBack = False
                txtTboxid.ppTextBox.AutoPostBack = False
                ddlSystem.ppDropDownList.AutoPostBack = False
                Master.ppRigthButton1.CausesValidation = True
                Master.ppRigthButton2.CausesValidation = False
                Master.ppRigthButton3.CausesValidation = False
                Master.ppRigthButton4.CausesValidation = False

                Master.ppRigthButton1.BackColor = Drawing.Color.Empty



                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton3.Visible = False
                Master.ppRigthButton4.Visible = False

                mpEnable = True
                ddlInsUpd.ppEnabled = False

            Case "1"    '変更
                dttCntDt.ppDateBox.AutoPostBack = True
                txtTboxid.ppTextBox.AutoPostBack = True
                ddlSystem.ppDropDownList.AutoPostBack = True
                Master.ppRigthButton1.CausesValidation = True
                Master.ppRigthButton2.CausesValidation = True
                Master.ppRigthButton3.CausesValidation = True
                Master.ppRigthButton4.CausesValidation = False

                Master.ppRigthButton1.BackColor = Drawing.Color.FromArgb(255, 102, 102)

                ddlVer.Visible = True

                Master.ppRigthButton1.Visible = True
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton3.Visible = True
                Master.ppRigthButton4.Visible = True

                ddlInsUpd.ppEnabled = False
        End Select
    End Sub
    'CMPLSTP003-001 END

#Region "検索条件クリア"
    ''' <summary>
    ''' 検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearch()
        'Me.dttCntDt.ppText = String.Empty
        Me.txtTboxid.ppText = String.Empty
        Me.ddlSystem.ppSelectedValue = ""
        Me.ddlPre.ppSelectedValue = ""
        Me.ddlArea.ppSelectedValue = ""
        Master.Master.ppRigthButton1.Enabled = False
        'CMPLSTP003-001
        strDispMode = "Default"
        ddlInsUpd.ppSelectedValue = String.Empty
        msSetddlVer()
        ddlVer.ppSelectedValue = String.Empty
        'CMPLSTP003-001 END
    End Sub
#End Region

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
