'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　DLL変更依頼内容マスタ一覧
'*　ＰＧＭＩＤ：　COMUPDMA7
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2016.10.XX　：　栗原
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
#End Region

Public Class COMUPDMA7

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
    Const DispCode As String = "COMUPDMA7"                   '画面ID
    Const MasterName As String = "DLL設定依頼内容マスタ一覧" '画面名
    Const TableName_M62 As String = "M62_DLL_WORK"            'テーブル名
    Const TableName_MA7 As String = "MA7_DLL_TIME"
    Const TableName_MA8 As String = "MA8_DEN_NAME"
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim clsDataConnect As New ClsCMDataConnect
    Dim objStack As StackFrame
    Dim clsExc As New ClsCMExclusive
    Dim strMode As String = Nothing
    Dim clsMst As New ClsMSTCommon
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page Init
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        pfSet_GridView(Me.grvList, DispCode, DispCode + "_Header", Me.DivOut, 28, 9)
    End Sub

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        '不要なので登録エリアを消します
        DirectCast(Me.Master.FindControl("UpdPanelMain"), UpdatePanel).Visible = False

        'フッタ部に更新ボタンを表示・設定します。
        Dim btnFtUpdate As Button = DirectCast(Me.Master.FindControl("btnRight1"), Button)
        btnFtUpdate.Visible = True
        btnFtUpdate.Text = "更新"
        AddHandler btnFtUpdate.Click, AddressOf msMovePage

        'リロードボタンを設定します
        AddHandler btnReload.Click, AddressOf msGet_Data

        If Not IsPostBack Then
            Master.ppProgramID = DispCode
            Master.ppTitle = MasterName
            Master.ppCount = "0"
            Master.ppBCList = pfGet_BCList("マスタ管理メニュー", Master.ppTitle)

            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            msGet_Data()

            SetFocus(btnReload.ClientID)
        End If
    End Sub

    ''' <summary>
    ''' Page PreRender
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender


    End Sub

#End Region

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

        If Me.IsPostBack Then
            dispFlg = "0"
        Else
            dispFlg = "1"
        End If

        If clsDataConnect.pfOpen_Database(conDB) Then
            Try
                cmdDB = New SqlCommand(DispCode & "_S1", conDB)
                With cmdDB.Parameters
                    .Add(pfSet_Param("prog_id", SqlDbType.NVarChar, DispCode))
                    .Add(pfSet_Param("disp_flg", SqlDbType.NVarChar, dispFlg))
                End With

                dstOrders = clsDataConnect.pfGet_DataSet(cmdDB)

                If dstOrders.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                ElseIf CType(dstOrders.Tables(0).Rows(0).Item("総件数").ToString, Integer) > dstOrders.Tables(0).Rows.Count Then
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
                    psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                        dstOrders.Tables(0).Rows(0).Item("総件数").ToString, dstOrders.Tables(0).Rows.Count)
                Else
                    Master.ppCount = dstOrders.Tables(0).Rows(0).Item("総件数").ToString
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
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"
        End If
    End Sub

    ''' <summary>
    ''' 登録画面へ遷移
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub msMovePage()
        'ユーザーマスタ
        Dim strDispID As String = P_FUN_COM & P_SCR_UPD & P_PAGE_M & "A8"
        Dim strPath As String = "~/" & P_MST & "/" & strDispID & "/" & strDispID & ".aspx"
        'ログ出力開始
        psLogStart(Me)

        psOpen_Window(Me, strPath)
        'ログ出力終了
        psLogEnd(Me)
    End Sub

    Private Sub msGridBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each dr As GridViewRow In grvList.Rows
            If DirectCast(dr.FindControl("設定依頼内容コード"), TextBox).Text = "02" OrElse DirectCast(dr.FindControl("設定依頼内容コード"), TextBox).Text = "03" Then

                Dim cnghour As String = DirectCast(dr.FindControl("変更設定時間"), TextBox).Text.Trim.Split(":")(0)
                Dim cngmin As String = DirectCast(dr.FindControl("変更設定時間"), TextBox).Text.Trim.Split(":")(1)
                Dim cngday As String = DirectCast(dr.FindControl("変更日付補正"), TextBox).Text.Replace(Microsoft.VisualBasic.vbCrLf, "").Trim
                Dim cngtime As String = DirectCast(dr.FindControl("変更時間補正"), TextBox).Text.Replace(Microsoft.VisualBasic.vbCrLf, "").Trim
                Dim rtnhour As String = DirectCast(dr.FindControl("戻し設定時間"), TextBox).Text.Trim.Split(":")(0)
                Dim rtnmin As String = DirectCast(dr.FindControl("戻し設定時間"), TextBox).Text.Trim.Split(":")(1)
                Dim rtnday As String = DirectCast(dr.FindControl("戻し日付補正"), TextBox).Text.Replace(Microsoft.VisualBasic.vbCrLf, "").Trim
                Dim rtntime As String = DirectCast(dr.FindControl("戻し時間補正"), TextBox).Text.Replace(Microsoft.VisualBasic.vbCrLf, "").Trim

                DirectCast(dr.FindControl("変更動作時刻"), TextBox).Text = mfCalcTime(cnghour, cngmin, cngday, cngtime)
                DirectCast(dr.FindControl("戻し動作時刻"), TextBox).Text = mfCalcTime(rtnhour, rtnmin, rtnday, rtntime)

            Else
                DirectCast(dr.FindControl("変更動作時刻"), TextBox).Text = "-"
                DirectCast(dr.FindControl("戻し動作時刻"), TextBox).Text = "-"
                DirectCast(dr.FindControl("変更設定時間"), TextBox).Text = "-"
                DirectCast(dr.FindControl("戻し設定時間"), TextBox).Text = "-"

            End If
        Next

    End Sub

    Private Function mfCalcTime(ByVal _hour As String, ByVal _min As String, ByVal _day As String, ByVal _time As String) As String
        Dim msecPerMinute As Decimal = 1000 * 60
        Dim msecPerHour As Decimal = msecPerMinute * 60
        Dim msecPerDay As Decimal = msecPerHour * 24
        Dim minusflg As Boolean = False
        Dim minutes As Decimal = 0
        Dim hours As Decimal = 0
        Dim days As Decimal = 0
        Dim time As Decimal = 0
        Dim rtn As String = String.Empty
        Try
            '補正値(日付・時間)を成形(1日前⇒-1、2分後⇒2)
            If _day.Contains("日後") Then
                _day = _day.Replace("日後", "")
            ElseIf _day.Contains("日前") Then
                _day = "-" & _day.Replace("日前", "")
            Else
                _day = "0"
            End If

            If _time.Contains("分後") Then
                _time = _time.Replace("分後", "")
            ElseIf _time.Contains("分前") Then
                _time = "-" & _time.Replace("分前", "")
            Else
                _time = "0"
            End If

            '型変換
            If Decimal.TryParse(_hour, hours) Or Decimal.TryParse(_min, minutes) Or Decimal.TryParse(_day, days) Or Decimal.TryParse(_time, time) Then
            Else
                Return String.Empty
            End If

            '基準日(2000/4/1)を作成し、基準日への加算により依頼の作動時刻を取得、元との差分から日数補正値を取得します。
            '基準日　main：変更を加えない元の基準値保持用　dt：計算用
            Dim main As New DateTime(2000, 4, 1, hours, minutes, 0)
            Dim dt As DateTime = main
            dt = dt.AddDays(days)
            dt = dt.AddMinutes(time)

            '日数差分の取得
            Dim rtnTime As TimeSpan = dt.Date - main.Date
            days = rtnTime.Days.ToString

            '作動時刻の取得
            hours = dt.Hour
            minutes = dt.Minute

            If days <> 0 Then
                If days < 0 Then
                    rtn = Math.Abs(days).ToString + "日前 "
                Else
                    rtn = Math.Abs(days).ToString + "日後 "
                End If
            Else
                rtn = "当日 "
            End If
            rtn = rtn + hours.ToString("00") + ":" + minutes.ToString("00")

            Return rtn


        Catch ex As Exception
            Return String.Empty
        End Try

    End Function

#End Region

#Region "終了処理プロシージャ"

#End Region

End Class
