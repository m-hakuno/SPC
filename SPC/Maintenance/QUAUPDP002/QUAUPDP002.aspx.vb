'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　修理機器一覧
'*　ＰＧＭＩＤ：　QUAUPDP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2015.02.27　：　武
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

Public Class QUAUPDP002
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
    Const M_MY_DISP_ID = P_FUN_QUA & P_SCR_UPD & P_PAGE & "002"

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
    'Dim strPkey() As String
    Dim strBranch As String = Nothing
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

        '戻るボタン設定
        Master.ppRigthButton1.Attributes.Add("onclick", "window.close();")
        '更新ボタン設定
        'AddHandler Master.ppRigthButton2.Click, AddressOf btnUpdate_Click
        AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

        If Not IsPostBack Then  '初回表示のみ
            '    'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = "修理機器一覧"

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'ボタン活性
            Master.ppRigthButton1.Visible = True
            'Master.ppRigthButton2.Visible = True
            Master.ppRigthButton1.Text = "戻る"
            'Master.ppRigthButton2.Text = "更新"

            '画面間パラメータ
            ViewState(P_KEY) = Session(P_KEY)
            Dim strPkey() As String = ViewState(P_KEY)
            Dim strBranch As String = strPkey(6)

            'パラメータセット
            If Not strPkey Is Nothing Then

                lblMntNo.Text = strPkey(0)

                'グリッドの初期化
                Me.grvList.DataSource = New DataTable
                grvList.DataBind()

                SetData()

                'グリッド編集
                msGet_GRIDEdit()

            End If
        End If
    End Sub

    ''' <summary>
    ''' グリッドの選択ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        psLogStart(Me)

        Dim rowData As GridViewRow = Nothing        'ボタン押下行

        If e.CommandName <> "btnSelect" Then
            Dim strMntNo As String = ViewState(P_KEY)(0)
            Dim strSecID As String = ViewState(P_KEY)(1)
            Dim strBranchID As String = ViewState(P_KEY)(2)
            Dim strKosyoBuiID As String = ViewState(P_KEY)(3)
            Dim strIchijiShindan As String = ViewState(P_KEY)(4)
            Dim strContentID As String = ViewState(P_KEY)(5)

            rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))
            Master.ppRigthButton1.Attributes.Add("onFocus", "closeWindow(""" & strSecID & _
                                                 """,""" & CType(rowData.FindControl("枝番"), TextBox).Text.Split(":"c)(0) & """,""" & strBranchID & _
                                                 """,""" & CType(rowData.FindControl("機器種別コード"), TextBox).Text.Split(":"c)(0) & """,""" & strKosyoBuiID & _
                                                 """,""" & CType(rowData.FindControl("診断結果"), TextBox).Text.Split(":"c)(0) & """,""" & strIchijiShindan & _
                                                 """,""" & CType(rowData.FindControl("故障原因及び修理内容"), TextBox).Text & """,""" & strContentID & "" & _
                                                 """,""" & strMntNo & """);")
            Master.ppRigthButton1.Focus()

            Exit Sub
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    'データ取得
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

                '--修理基本データの取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, lblMntNo.Text))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                lblTboxID.Text = objDs.Tables(0).Rows(0).Item("D29_TBOXID").ToString
                lblHallNm.Text = objDs.Tables(0).Rows(0).Item("D29_HALL_NM").ToString

                '--修理明細データの取得
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, lblMntNo.Text))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count > 0 Then
                    '取得したデータをグリッドに設定
                    Me.grvList.DataSource = objDs.Tables(0)
                    '変更を反映
                    Me.grvList.DataBind()
                Else
                    Me.grvList.DataBind()
                    '修理がありません
                    psMesBox(Me, "30022", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "品質会議資料明細")

                End If

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

    ''' <summary>
    ''' グリッド編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function msGet_GRIDEdit()
        'ヘッダーの値を設定
        Dim strHeadNm As String() = New String() _
                                    {"選択", "枝番", "機器種別コード", "診断結果", "故障原因及び修理内容"}

        Try

            For Each rowData As GridViewRow In grvList.Rows
                If CType(rowData.FindControl(strHeadNm(1)), TextBox).Text.Trim = ViewState(P_KEY)(6) Then
                    For clmCnt As Integer = 0 To rowData.Cells.Count - 1
                        If clmCnt = 0 Then
                            CType(rowData.Cells(0).Controls.Item(0), Button).ForeColor = Drawing.Color.Red
                        Else
                            CType(rowData.FindControl(strHeadNm(clmCnt)), TextBox).ForeColor = Drawing.Color.Red
                        End If
                    Next
                End If
            Next

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

        End Try
        Return True

    End Function

    ''' <summary>
    ''' 更新ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs)
        'グリッドの初期化
        Me.grvList.DataSource = New DataTable
        grvList.DataBind()

        SetData()

        'グリッド編集
        msGet_GRIDEdit()
    End Sub

#End Region

End Class
