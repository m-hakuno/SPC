'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　CTI情報(作業者)
'*　ＰＧＭＩＤ：　CTISELP005
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.01.28　：　高松
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
#End Region

Public Class CTISELP005

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
    Private Const M_DISP_ID = P_FUN_CTI & P_SCR_SEL & P_PAGE & "005"

    Private Const M_NEXT_DISP_PATH_CNS = "~/" & P_CNS & "/" & _
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "001" & "/" & _
                                         P_FUN_CNS & P_SCR_UPD & P_PAGE & "001.aspx"

    Private Const M_NEXT_DISP_PATH_MAI = "~/" & P_MAI & "/" & _
                                         P_FUN_CMP & P_SCR_SEL & P_PAGE & "001" & "/" & _
                                         P_FUN_CMP & P_SCR_SEL & P_PAGE & "001.aspx"

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

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Init
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        '表設定
        pfSet_GridView(Me.grvList, M_DISP_ID)

    End Sub

    ''' <summary>
    ''' Page_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then  '初回表示

            Dim strTel As String = String.Empty        '電話番号保管用
            Dim errMsg As String = String.Empty        'エラーメッセージ用
            Dim sesstion_in() As String = Session(P_KEY)
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            objStack = New StackFrame
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------

            'System.Threading.Thread.Sleep(500)
            Me.grvList.Focus()

            Try

                'プログラムＩＤ、画面名設定
                Master.ppProgramID = M_DISP_ID
                Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)

                'パンくずリスト設定
                Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

                '画面初期化処理
                msClearScreen()

                'URLから情報をゲットする
                'strTel = Page.Request.QueryString.Get("TEL")

                'セッション情報の確認
                If sesstion_in Is Nothing Then
                    Throw New Exception
                End If

                If sesstion_in.Count <> 5 Then
                    Throw New Exception
                End If

                'ビューステートへ設定
                Me.ViewState(P_KEY) = sesstion_in

                '表示情報の取得
                ms_GetCTIdata()

                'Select Case strTel
                '    Case Nothing

                '        Throw New Exception

                '    Case String.Empty

                '        Throw New Exception

                '    Case "非通知"

                '        errMsg = strTel
                '        Throw New Exception

                '    Case "公衆電話"

                '        errMsg = strTel
                '        Throw New Exception

                '    Case "表示圏外"

                '        errMsg = strTel
                '        Throw New Exception

                '    Case "相手先不明"

                '        errMsg = strTel
                '        Throw New Exception

                '    Case Else

                '        '電話番号をビューステートへ設定
                '        ViewState("電話番号") = strTel

                '        '表示情報の取得
                '        ms_GetCTIdata()

                'End Select

            Catch ex As Exception

                'If errMsg Is Nothing Then

                '    psMesBox(Me, "30008", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)

                'Else

                '    psMesBox(Me, "20008", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, errMsg)

                'End If
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------

                psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            End Try

        End If

    End Sub

#End Region

#Region "ボタンクリックイベント"

    ''' <summary>
    ''' グリッドボタン操作
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Select Case e.CommandName

            Case "btnDetail"
            Case Else
                Exit Sub

        End Select

        Dim intIndex As Integer = Convert.ToInt32(e.CommandArgument)    'ボタン押下行のIndex
        Dim rowData As GridViewRow = grvList.Rows(intIndex)             'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing                     '次画面引継ぎ用キー情報

        '画面遷移
        ms_OpnWindow(CType(rowData.FindControl("作業区分"), TextBox).Text _
                     , CType(rowData.FindControl("作業番号"), TextBox).Text)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面項目の初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '表示項目の初期化
        Me.lblTell_input.Text = String.Empty
        Me.lblEmp_input.Text = String.Empty
        Me.lblSisya_input.Text = String.Empty
        Me.lblEigyo_input.Text = String.Empty
        Me.lblTokatu_input.Text = String.Empty

        'グリッドビューの初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        '検証コントロールの非表示
        Me.SystemErr1.Visible = False
        Me.SystemErr2.Visible = False
        Me.SelectErr1.Visible = False
        Me.SelectErr2.Visible = False
        Me.SelectErr3.Visible = False
        Me.SelectErr4.Visible = False
        Me.SelectErr5.Visible = False

    End Sub

    ''' <summary>
    ''' 画面表示情報の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ms_GetCTIdata()

        Dim conDB As SqlConnection = Nothing
        Dim cmdDB As SqlCommand = Nothing
        Dim dstOrders As New DataSet
        Dim strOKNG As String = Nothing
        Dim strHotan_cd As String = Nothing
        Dim strTokatsu_cd As String = Nothing
        Dim strtboxid As String = Nothing
        Dim str_View_st_in() As String = ViewState(P_KEY)
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        If clsDataConnect.pfOpen_Database(conDB) Then

            Try

                cmdDB = New SqlCommand(M_DISP_ID & "_S1", conDB)

                '社員情報、ホール情報の取得
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_telno", SqlDbType.NVarChar, str_View_st_in(0)))                    '電話番号
                    .Add(pfSet_Param("prm_comp_cd", SqlDbType.NVarChar, str_View_st_in(1)))                  '会社コード
                    .Add(pfSet_Param("prm_hall_cd", SqlDbType.NVarChar, str_View_st_in(2)))                  'ホールコード
                    .Add(pfSet_Param("prm_employee_cd", SqlDbType.NVarChar, str_View_st_in(3)))              '社員コード
                    .Add(pfSet_Param("prm_decision_cls", SqlDbType.NVarChar, str_View_st_in(4)))             '判断区分
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        '何も無し
                        psClose_Window(Me)
                        Exit Sub

                    Case Else        'データ有り

                        '画面項目に値を設定する
                        Me.lblTell_input.Text = dstOrders.Tables(0).Rows(0).Item("ＴＥＬ").ToString
                        Me.lblEmp_input.Text = dstOrders.Tables(0).Rows(0).Item("社員名").ToString
                        Me.lblSisya_input.Text = dstOrders.Tables(0).Rows(0).Item("支社名").ToString
                        Me.lblEigyo_input.Text = dstOrders.Tables(0).Rows(0).Item("営業所名").ToString
                        Me.lblTokatu_input.Text = dstOrders.Tables(0).Rows(0).Item("統括営業所名").ToString
                        strHotan_cd = dstOrders.Tables(0).Rows(0).Item("保守担当者").ToString
                        strTokatsu_cd = dstOrders.Tables(0).Rows(0).Item("統括コード").ToString

                End Select

                strOKNG = Nothing

                ''TBOX情報の取得
                'cmdDB = New SqlCommand(M_DISP_ID & "_S3", conDB)
                'With cmdDB.Parameters
                '    'パラメータ設定
                '    .Add(pfSet_Param("prm_hotan_cd", SqlDbType.NVarChar, strHotan_cd))                       '保守担当コード
                '    .Add(pfSet_Param("prm_tokatsu_cd", SqlDbType.NVarChar, strTokatsu_cd))                   '統括コード
                '    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用(0:データなし、1:データあり)
                'End With

                ''データ取得
                'dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                ''結果情報を取得
                'strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                'Select Case strOKNG
                '    Case "0"         'データ無し

                '        '何も無し
                '        Exit Sub

                '    Case Else        'データ有り

                '        '取得したＴＢＯＸＩＤを,(カンマ区切り)で連結する
                '        For i As Integer = 0 To dstOrders.Tables(0).Rows.Count - 1
                '            If i = 0 Then
                '                strtboxid = strtboxid + dstOrders.Tables(0).Rows(i).Item("T03_TBOXID").ToString
                '            Else
                '                strtboxid = strtboxid + "," + dstOrders.Tables(0).Rows(i).Item("T03_TBOXID").ToString
                '            End If
                '        Next

                'End Select

                dstOrders = New DataSet

                '工事、保守情報の取得
                'cmdDB = New SqlCommand(M_DISP_ID & "_S2", conDB)
                'With cmdDB.Parameters
                '    'パラメータ設定
                '    .Add(pfSet_Param("prm_tbox_id", SqlDbType.NVarChar, strtboxid))                          'TBOXID
                '    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))       '結果取得用(0:データなし、1:データあり)
                'End With
                cmdDB = New SqlCommand(M_DISP_ID & "_S4", conDB)
                With cmdDB.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("prm_mnt_cd", SqlDbType.NVarChar, strHotan_cd))                          '保担コード
                    .Add(pfSet_Param("prm_employee_cd", SqlDbType.NVarChar, str_View_st_in(3)))               '社員コード
                    .Add(pfSet_Param("prm_decision_cls", SqlDbType.NVarChar, str_View_st_in(4)))              '判断区分
                    .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))        '結果取得用(0:データなし、1:データあり)
                End With

                'データ取得
                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                '結果情報を取得
                strOKNG = cmdDB.Parameters("data_exist").Value.ToString

                Select Case strOKNG
                    Case "0"         'データ無し

                        'グリッドの初期化
                        Me.grvList.DataSource = New DataTable
                        '変更を反映
                        Me.grvList.DataBind()

                        '--------------------------------
                        '2014/05/12 後藤　ここから
                        '--------------------------------
                        '0件
                        'psMesBox(Me, "00007", clscomver.E_Mタイプ.情報, clscomver.E_S実行.描画後)
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                        '--------------------------------
                        '2014/05/12 後藤　ここまで
                        '--------------------------------

                    Case Else        'データ有り

                        '取得したデータをグリッドに設定
                        Me.grvList.DataSource = dstOrders.Tables(0)

                        '変更を反映
                        Me.grvList.DataBind()

                        'データが一件の場合、画面遷移を行う
                        If dstOrders.Tables(0).Rows.Count = 1 Then

                            ms_OpnWindow(dstOrders.Tables(0).Rows(0).Item("作業区分").ToString _
                                        , dstOrders.Tables(0).Rows(0).Item("作業番号").ToString)

                        End If

                End Select

            Catch ex As SqlException

                '検索失敗
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＴＩ情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Throw ex

            Catch ex As Exception

                'システムエラー
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＴＩ情報")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
                Throw ex

            Finally

                'DB切断
                If Not clsDataConnect.pfClose_Database(conDB) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If

            End Try

        Else

            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

        End If

    End Sub

    ''' <summary>
    ''' 工事進捗 参照更新、保守対応依頼書へ画面遷移
    ''' </summary>
    ''' <param name="flag"></param>
    ''' <param name="strIraiNum"></param>
    ''' <remarks></remarks>
    Private Sub ms_OpnWindow(ByVal flag As String _
                           , ByVal strIraiNum As String)

        Dim strKeyList As New List(Of String)                     '次画面引継ぎ用キー情報

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.ppBcList_Text

        If flag = "工事" Then

            '画面遷移を行う(工事進捗)

            '次画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            strKeyList.Add(strIraiNum)

            '--------------------------------
            '2014/06/03 後藤　ここから
            '--------------------------------
            strKeyList.Add("0") 'SPC
            strKeyList.Add("") 'SPC
            '--------------------------------
            '2014/06/03 後藤　ここまで
            '--------------------------------
            Session(P_KEY) = strKeyList.ToArray
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
                            objStack.GetMethod.Name, M_NEXT_DISP_PATH_CNS, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            '工事進捗起動
            psOpen_Window(Me, M_NEXT_DISP_PATH_CNS)

        Else

            '画面遷移を行う(保守対応依頼書)
            '次画面引継ぎ用キー情報設定
            strKeyList = New List(Of String)
            'セッション情報設定																	
            Session(P_SESSION_BCLIST) = Master.ppBcList_Text
            Session(P_SESSION_OLDDISP) = M_DISP_ID
            strKeyList.Add(strIraiNum)
            Session(P_KEY) = strKeyList.ToArray
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
                            objStack.GetMethod.Name, M_NEXT_DISP_PATH_MAI, strPrm, "TRANS")
            '■□■□結合試験時のみ使用予定□■□■
            '--------------------------------
            '2014/04/16 星野　ここまで
            '--------------------------------
            '工事進捗起動
            psOpen_Window(Me, M_NEXT_DISP_PATH_MAI)

        End If

    End Sub

    ''' <summary>
    ''' グリッドビューフォーカス設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Dim rowNumF As Boolean = True        '行数フラグ

        '詳細ボタンにフォーカスを当てる
        For Each rowData As GridViewRow In grvList.Rows
            If rowNumF Then

                DirectCast(rowData.Cells(0).Controls(0), Button).Focus()

                rowNumF = False

            End If
        Next

    End Sub

#End Region

#Region "終了処理プロシージャ"

#End Region


End Class
