'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　ポップアップ表示
'*　ＰＧＭＩＤ：　ClsCMpnlPopup
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.24　：　酒井
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.ComponentModel
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMDataLink
Imports System.Web.UI

Public Class ClsCMpnlPopup
    Inherits System.Web.UI.UserControl

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

#End Region

#Region "イベントプロシージャ"
    ''' <summary>   
    ''' ダイアログを閉じたイベントのイベント引数   
    ''' </summary>   
    ''' <remarks></remarks>   
    Public Class ClosedEventArgs
        Public Property strSearch As String
    End Class

    ''' <summary>   
    ''' ダイアログを閉じた際のイベント   
    ''' </summary>   
    ''' <param name="arg"></param>   
    ''' <remarks></remarks>   
    Public Event DialogClosed(arg As ClosedEventArgs)


    ''' <summary>   
    ''' ポップアップウインドウを表示   
    ''' </summary>   
    ''' <remarks></remarks>   
    Public Sub Show(ByVal sArg() As String, ByVal showKbn As String)

        'Dim grvList As GridView

        grvList.Visible = True

        Me.ViewState("表示区分") = showKbn
        SetGridData(sArg, showKbn)

        updPopup.Update()
        modalPopupExtender.Show()

    End Sub

    ' ''' <summary>   
    ' ''' 表示ボタンClickイベント   
    ' ''' </summary>   
    ' ''' <param name="sender"></param>   
    ' ''' <param name="e"></param>   
    ' ''' <remarks></remarks>   
    'Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

    '    Me.grvList.Visible = True
    '    'グリッドデータ表示
    '    SetGridData()

    '    Me.updPopup.Update()
    '    Me.modalPopupExtender.Show()

    'End Sub

    ''' <summary>   
    ''' グリッドのRowDataBoundイベント   
    ''' </summary>   
    ''' <param name="sender"></param>   
    ''' <param name="e"></param>   
    ''' <remarks></remarks>   
    Private Sub grvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grvList.RowDataBound

        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        Dim drv As DataRowView = DirectCast(e.Row.DataItem, DataRowView)

        Dim showKbn As String = Me.ViewState("表示区分")
        Dim args() As String = Nothing

        '選択
        '表示区分毎にデータテーブルの形を変更
        Select Case showKbn
            Case "0", "2"

                args = {drv.Item("社員コード").ToString(), drv.Item("社員名").ToString()}


            Case "1"

                args = {drv.Item("連番").ToString(), drv.Item("会社名").ToString(), drv.Item("営業所").ToString()}

        End Select

        Dim lnkbtnSelect As LinkButton = DirectCast(e.Row.FindControl("lnkbtnSelect"), LinkButton)
        lnkbtnSelect.CommandArgument = String.Join(",", args)

    End Sub

    ''' <summary>   
    ''' グリッドのRowCommandイベント   
    ''' </summary>   
    ''' <param name="sender"></param>   
    ''' <param name="e"></param>   
    ''' <remarks></remarks>   
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        grvList.Visible = False
        modalPopupExtender.Hide()

        Dim args() As String = e.CommandArgument.ToString.Split(",")
        Dim eventArgs As New ClosedEventArgs

        Dim showKbn As String = Me.ViewState("表示区分")

        Select Case e.CommandName
            Case "select"        '選択ボタン押下
                Select Case showKbn
                    Case "0", "2"    '社員名

                        eventArgs.strSearch = args(1)

                    Case "1"     '連番

                        eventArgs.strSearch = args(0)

                End Select

                RaiseEvent DialogClosed(eventArgs)
        End Select

    End Sub

    ''' <summary>   
    ''' グリッドのPageIndexChangingイベント   
    ''' </summary>   
    ''' <param name="sender"></param>   
    ''' <param name="e"></param>   
    ''' <remarks></remarks>   
    Private Sub grvList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvList.PageIndexChanging

        grvList.Visible = True

        '第二引数から、選択されたページindexを取得。
        'GridViewのページIndexに設定。　　　　
        grvList.PageIndex = e.NewPageIndex

        'GridViewに再度データをバインドさせる。
        '先ほどのデータセットを格納。
        grvList.DataSource = ViewState("data")
        grvList.DataBind()

        modalPopupExtender.Show()

    End Sub

    ''' <summary>   
    ''' グリッドデータ表示   
    ''' </summary>   
    ''' <remarks></remarks>   
    Private Sub SetGridData(ByVal sArg() As String, ByVal showKbn As String)
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

        Try

            '画面ページ表示初期化
            grvList.DataSource = Nothing

            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Exit Try
            End If

            '***** 一覧データ取得 *****

            'パラメータ設定
            Select Case showKbn

                Case "0"  '社員名

                    cmdDB = New SqlCommand("ZCMPSEL031", conDB)

                    'データ取得
                    With cmdDB.Parameters
                        .Add(pfSet_Param("TRADER_CD", SqlDbType.NVarChar, sArg(0)))       '業者コード
                        If sArg(1) = Nothing Then
                            .Add(pfSet_Param("AUTH_CLS_FROM", SqlDbType.NVarChar, "0"))   '権限区分
                        Else
                            .Add(pfSet_Param("AUTH_CLS_FROM", SqlDbType.NVarChar, sArg(1)))   '権限区分
                        End If
                        If sArg(2) = Nothing Then
                            .Add(pfSet_Param("AUTH_CLS_TO", SqlDbType.NVarChar, "9"))     '権限区分
                        Else
                            .Add(pfSet_Param("AUTH_CLS_TO", SqlDbType.NVarChar, sArg(2)))     '権限区分
                        End If

                    End With

                Case "1" '業者名、営業所、電話番号

                    'タイトル張替え
                    Me.lblTitle.Text = "業者情報一覧"

                    cmdDB = New SqlCommand("ZCMPSEL042", conDB)

                Case "2"    '社員名（SPCのみ）

                    cmdDB = New SqlCommand("ZCMPSEL046", conDB)

                    'データ取得
                    With cmdDB.Parameters
                        .Add(pfSet_Param("comp_cd", SqlDbType.NVarChar, "701"))       '会社コード
                        .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, "701"))       '会社コード
                        .Add(pfSet_Param("data_exist", SqlDbType.NVarChar, 20, ParameterDirection.Output))
                    End With

            End Select


            Dim ds As New DataSet
            ds = clsDataConnect.pfGet_DataSet(cmdDB)

            If showKbn = "2" Then
                ds.Tables(0).Columns.Remove("表示名")
                ds.AcceptChanges()
            End If

            grvList.DataSource = ds.Tables(0)

            grvList.DataBind()

            ViewState("data") = ds.Tables(0)

        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            grvList.DataBind()
        Finally
            'DB切断
            If Not conDB Is Nothing Then
                clsDataConnect.pfClose_Database(conDB)
            End If
        End Try

    End Sub

    ''' <summary>
    ''' 閉じるボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click, btnDummy.Click

        Dim eventArgs As New ClosedEventArgs
        eventArgs.strSearch = ""
        RaiseEvent DialogClosed(eventArgs)

    End Sub

#End Region
End Class
