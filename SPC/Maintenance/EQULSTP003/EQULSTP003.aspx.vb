'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守予備機棚卸表
'*　ＰＧＭＩＤ：　EQULSTP003
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.29　：　ＮＫＣ
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

Public Class EQULSTP003
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
    Const M_MY_DISP_ID = P_FUN_EQU & P_SCR_LST & P_PAGE & "003"

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
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then  '初回表示のみ

            'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            'ドロップダウンリスト設定
            msSetddlOffice()    '支社
            msSetddlMnt()       '保担

            '画面初期化
            msClearScreen()

        End If

    End Sub

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

    End Sub
    '---------------------------
    '2014/04/16 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索条件クリアボタンクリック時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        '開始ログ出力
        psLogStart(Me)

        '画面クリア
        msClearScreen()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' CSVボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnCsv_Click(sender As Object, e As EventArgs) Handles btnCsv.Click

        Dim objDs As DataSet = Nothing  'データセット

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        msCheckSearchCondition()

        '入力内容検証
        If Page.IsValid Then
            'データ取得処理
            objDs = mfGetData()

            If Not objDs Is Nothing Then
                'CSVファイル出力
                msCsvFileOutput(objDs)
            End If
        End If

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
            Me.ddlOfficeFm.SelectedIndex = 0
            Me.ddlOfficeTo.SelectedIndex = 0
            Me.ddlMntFm.SelectedIndex = 0
            Me.ddlMntTo.SelectedIndex = 0
            Me.ddlOfficeFm.Focus()

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
    ''' 整合性チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckSearchCondition()

        Dim drMsg As DataRow = Nothing  '検証メッセージ取得

        '支社名FromToの両方が選択されていて、保担名FromToの
        '何れかが選択されている場合はエラー
        If Me.ddlOfficeFm.SelectedValue <> String.Empty AndAlso
           Me.ddlOfficeTo.SelectedValue <> String.Empty AndAlso
           (Me.ddlMntFm.SelectedValue <> String.Empty OrElse
            Me.ddlMntTo.SelectedValue <> String.Empty) Then
            drMsg = pfGet_ValMes("4011")
            Me.valOffice.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valOffice.Text = drMsg.Item(P_VALMES_SMES)
            Me.valOffice.IsValid = False
        End If

        '支社名Fromのみ選択されていて、保担名FromToの
        '何れかが選択されている場合はエラー
        If Me.ddlOfficeFm.SelectedValue <> String.Empty AndAlso
           Me.ddlOfficeTo.SelectedValue = String.Empty AndAlso
           (Me.ddlMntFm.SelectedValue <> String.Empty OrElse
            Me.ddlMntTo.SelectedValue <> String.Empty) Then
            drMsg = pfGet_ValMes("4011")
            Me.valOffice.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valOffice.Text = drMsg.Item(P_VALMES_SMES)
            Me.valOffice.IsValid = False
        End If

        '支社名Toのみ選択されている場合はエラー
        If Me.ddlOfficeFm.SelectedValue = String.Empty AndAlso
           Me.ddlOfficeTo.SelectedValue <> String.Empty Then
            drMsg = pfGet_ValMes("5003", "開始支社名")
            Me.valOffice.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valOffice.Text = drMsg.Item(P_VALMES_SMES)
            Me.valOffice.IsValid = False
        End If

        '支社名FromToの両方未選択で、保担名Toのみが
        '選択されている場合はエラー
        If Me.ddlOfficeFm.SelectedValue = String.Empty AndAlso
           Me.ddlOfficeTo.SelectedValue = String.Empty AndAlso
           Me.ddlMntFm.SelectedValue = String.Empty AndAlso
           Me.ddlMntTo.SelectedValue <> String.Empty Then
            drMsg = pfGet_ValMes("5003", "開始保担名")
            Me.valMnt.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valMnt.Text = drMsg.Item(P_VALMES_SMES)
            Me.valMnt.IsValid = False
        End If

        '支社名FromTo及び保担名FromToのすべてが未選択の場合はエラー
        If Me.ddlOfficeFm.SelectedValue = String.Empty AndAlso
           Me.ddlOfficeTo.SelectedValue = String.Empty AndAlso
           Me.ddlMntFm.SelectedValue = String.Empty AndAlso
           Me.ddlMntTo.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "支社名または保担名")
            Me.valOffice.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valOffice.Text = drMsg.Item(P_VALMES_SMES)
            Me.valOffice.IsValid = False
        End If

        '支社名FromTo大小チェック
        If Me.ddlOfficeTo.SelectedValue <> String.Empty AndAlso
           Me.ddlOfficeFm.SelectedValue > Me.ddlOfficeTo.SelectedValue Then
            drMsg = pfGet_ValMes("2001", "支社名")
            Me.valOffice.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valOffice.Text = drMsg.Item(P_VALMES_SMES)
            Me.valOffice.IsValid = False
        End If

        '保担名FromTo大小チェック
        If Me.ddlMntTo.SelectedValue <> String.Empty AndAlso
           Me.ddlMntFm.SelectedValue > Me.ddlMntTo.SelectedValue Then
            drMsg = pfGet_ValMes("2001", "保担名")
            Me.valMnt.ErrorMessage = drMsg.Item(P_VALMES_MES)
            Me.valMnt.Text = drMsg.Item(P_VALMES_SMES)
            Me.valMnt.IsValid = False
        End If

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim strBuff As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetData = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '支社名From
                    .Add(pfSet_Param("office_f", SqlDbType.NVarChar, Me.ddlOfficeFm.SelectedValue))

                    '支社名To
                    .Add(pfSet_Param("office_t", SqlDbType.NVarChar, Me.ddlOfficeTo.SelectedValue))

                    '保担名From
                    .Add(pfSet_Param("mnt_f", SqlDbType.NVarChar, Me.ddlMntFm.SelectedValue))

                    '保担名To
                    .Add(pfSet_Param("mnt_t", SqlDbType.NVarChar, Me.ddlMntTo.SelectedValue))
                End With

                'データ取得
                mfGetData = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守予備機棚卸表")
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

    End Function

    ''' <summary>
    ''' CSVファイル出力処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msCsvFileOutput(objDs As DataSet)

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
            Else
                'CSVファイルダウンロード
                If pfDLCsvFile(Master.ppTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv",
                               objDs.Tables(0), True, Me) <> 0 Then
                    psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
                End If
            End If

        Catch ex As Threading.ThreadAbortException

        Catch ex As Exception
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＣＳＶファイル")
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
    ''' ドロップダウンリスト設定（支社名）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlOffice()

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
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '対応年月(yyyy/MM)
                    .Add(pfSet_Param("infocls", SqlDbType.NVarChar, "1"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '--支社名From
                Me.ddlOfficeFm.Items.Clear()
                Me.ddlOfficeFm.DataSource = objDs.Tables(0)
                Me.ddlOfficeFm.DataTextField = "支社名"
                Me.ddlOfficeFm.DataValueField = "支社コード"
                Me.ddlOfficeFm.DataBind()
                Me.ddlOfficeFm.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加
                '--支社名To
                Me.ddlOfficeTo.Items.Clear()
                Me.ddlOfficeTo.DataSource = objDs.Tables(0)
                Me.ddlOfficeTo.DataTextField = "支社名"
                Me.ddlOfficeTo.DataValueField = "支社コード"
                Me.ddlOfficeTo.DataBind()
                Me.ddlOfficeTo.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "支社名一覧取得")
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
    ''' ドロップダウンリスト設定（保担名）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMnt()

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
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '対応年月(yyyy/MM)
                    .Add(pfSet_Param("infocls", SqlDbType.NVarChar, "2"))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '--保担名From
                Me.ddlMntFm.Items.Clear()
                Me.ddlMntFm.DataSource = objDs.Tables(0)
                Me.ddlMntFm.DataTextField = "保担名"
                Me.ddlMntFm.DataValueField = "保担コード"
                Me.ddlMntFm.DataBind()
                Me.ddlMntFm.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加
                '--保担名To
                Me.ddlMntTo.Items.Clear()
                Me.ddlMntTo.DataSource = objDs.Tables(0)
                Me.ddlMntTo.DataTextField = "保担名"
                Me.ddlMntTo.DataValueField = "保担コード"
                Me.ddlMntTo.DataBind()
                Me.ddlMntTo.Items.Insert(0, New ListItem(Nothing, Nothing))  '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保担名一覧取得")
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

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
