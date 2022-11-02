'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　業者情報
'*　ＰＧＭＩＤ：　COMSELP002
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.06　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
#End Region

Public Class COMSELP002
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
    Const M_MY_DISP_ID = P_FUN_COM & P_SCR_SEL & P_PAGE & "002"
    '次画面ファイルパス
    Const M_NEXT_DISP_PATH = "~/" & P_COM & "/" &
            P_FUN_COM & P_SCR_SEL & P_PAGE & "005" & "/" &
            P_FUN_COM & P_SCR_SEL & P_PAGE & "005.aspx"
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

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click

        If Not IsPostBack Then  '初回表示のみ

            '検索条件クリアボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '画面クリア
            msClearScreen()

        End If

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
            msGetData()
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    '---------------------------
    '2014/04/14 武 ここから
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
                e.Row.Cells(0).Enabled = False
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '検索条件クリア
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

        If e.CommandName <> "btnDetail" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '次画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(CType(rowData.FindControl("業者連番"), TextBox).Text)

        'セッション情報設定																	
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
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
                        objStack.GetMethod.Name, M_NEXT_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '業者情報詳細画面起動
        psOpen_Window(Me, M_NEXT_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

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

            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            Me.tftTraderCd.ppTextBoxFrom.Focus()

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
    ''' 検索条件クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        Me.tftTraderCd.ppFromText = String.Empty
        Me.tftTraderCd.ppToText = String.Empty
        Me.tftCompanyCd.ppFromText = String.Empty
        Me.tftCompanyCd.ppToText = String.Empty
        Me.txtCompanyNm.ppText = String.Empty
        Me.tftIntgrtCd.ppFromText = String.Empty
        Me.tftIntgrtCd.ppToText = String.Empty
        Me.tftOfficeCd.ppFromText = String.Empty
        Me.tftOfficeCd.ppToText = String.Empty
        Me.txtOfficeNm.ppText = String.Empty
        Me.tftStateCd.ppFromText = String.Empty
        Me.tftStateCd.ppToText = String.Empty
        Me.txtAddress.ppText = String.Empty

        Me.tftTraderCd.ppTextBoxFrom.Focus()

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
                    '業者コードFrom
                    If Me.tftTraderCd.ppToText = String.Empty OrElse
                       Me.tftTraderCd.ppToText = Me.tftTraderCd.ppFromText Then
                        '業者コードToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("trader_cd_f", SqlDbType.NVarChar,
                                         Me.tftTraderCd.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '業者コードToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("trader_cd_f", SqlDbType.NVarChar, Me.tftTraderCd.ppFromText))
                    End If
                    '業者コードTo
                    .Add(pfSet_Param("trader_cd_t", SqlDbType.NVarChar, Me.tftTraderCd.ppToText))
                    '会社コードFrom
                    If Me.tftCompanyCd.ppToText = String.Empty OrElse
                       Me.tftCompanyCd.ppToText = Me.tftCompanyCd.ppFromText Then
                        '会社コードToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("company_cd_f", SqlDbType.NVarChar,
                                         Me.tftCompanyCd.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '会社コードToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("company_cd_f", SqlDbType.NVarChar, Me.tftCompanyCd.ppFromText))
                    End If
                    '会社コードTo
                    .Add(pfSet_Param("company_cd_t", SqlDbType.NVarChar, Me.tftCompanyCd.ppToText))
                    '会社名
                    .Add(pfSet_Param("company_nm", SqlDbType.NVarChar,
                                     Me.txtCompanyNm.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '統括コードFrom
                    If Me.tftIntgrtCd.ppToText = String.Empty OrElse
                       Me.tftIntgrtCd.ppToText = Me.tftIntgrtCd.ppFromText Then
                        '統括コードToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("intgrt_cd_f", SqlDbType.NVarChar,
                                         Me.tftIntgrtCd.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '統括コードToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("intgrt_cd_f", SqlDbType.NVarChar, Me.tftIntgrtCd.ppFromText))
                    End If
                    '統括コードTo
                    .Add(pfSet_Param("intgrt_cd_t", SqlDbType.NVarChar, Me.tftIntgrtCd.ppToText))
                    '営業所コードFrom
                    If Me.tftOfficeCd.ppToText = String.Empty OrElse
                       Me.tftOfficeCd.ppToText = Me.tftOfficeCd.ppFromText Then
                        '営業所コードToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("office_cd_f", SqlDbType.NVarChar,
                                         Me.tftOfficeCd.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '営業所コードToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("office_cd_f", SqlDbType.NVarChar, Me.tftOfficeCd.ppFromText))
                    End If
                    '営業所コードTo
                    .Add(pfSet_Param("office_cd_t", SqlDbType.NVarChar, Me.tftOfficeCd.ppToText))
                    '営業所名
                    .Add(pfSet_Param("office_nm", SqlDbType.NVarChar,
                                     Me.txtOfficeNm.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '県コードFrom
                    If Me.tftStateCd.ppToText = String.Empty OrElse
                       Me.tftStateCd.ppToText = Me.tftStateCd.ppFromText Then
                        '県コードToが空白の場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("state_cd_f", SqlDbType.NVarChar,
                                         Me.tftStateCd.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '県コードToが空白でないの場合は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("state_cd_f", SqlDbType.NVarChar, Me.tftStateCd.ppFromText))
                    End If
                    '県コードTo
                    .Add(pfSet_Param("state_cd_t", SqlDbType.NVarChar, Me.tftStateCd.ppToText))
                    '住所
                    .Add(pfSet_Param("address", SqlDbType.NVarChar,
                                     Me.txtAddress.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '件数を設定
                Master.ppCount = objDs.Tables(0).Rows.Count.ToString

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者")
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
            End Try
        End If

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
