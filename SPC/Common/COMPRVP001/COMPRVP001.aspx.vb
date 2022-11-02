'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　帳票プレビュー
'*　ＰＧＭＩＤ：　COMPRVP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.02.07　：　ＮＫＣ
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
#Region "インポート定義"
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
#End Region

Public Class COMPRVP001

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
#End Region

    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
#Region "構造体・列挙体定義"
#End Region

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
#Region "変数定義"
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
#End Region

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#Region "プロパティ定義"
#End Region

    '============================================================================================================================
    '=　イベントプロシージャ定義
    '============================================================================================================================
#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objReport As Object()
        Dim objData As Object()
        Dim strName As String()
        Dim intData As Integer
        Dim mstStream As New System.IO.MemoryStream()
        Dim ctsSetting As ConnectionStringSettings
        Dim dS As New GrapeCity.ActiveReports.Data.SqlDBDataSource
        Dim pdfExport As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'セッション変数「レポート」「データ」「レポート名」が存在しない場合、画面を閉じる
        If Session(P_SESSION_PRV_REPORT) Is Nothing OrElse _
            Session(P_SESSION_PRV_DATA) Is Nothing OrElse _
            Session(P_SESSION_PRV_NAME) Is Nothing Then
            psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            psClose_Window(Me)
            Return
        End If

        'セッション情報取得
        objReport = Session(P_SESSION_PRV_REPORT)
        objData = Session(P_SESSION_PRV_DATA)
        strName = Session(P_SESSION_PRV_NAME)

        Integer.TryParse(Request.QueryString("data"), intData)

        'ActiveReports実行
        Try
            Select Case objData(intData).GetType.ToString
                Case "System.String"            'SQL指定
                    'DB接続文字列の取得
                    ctsSetting = ConfigurationManager.ConnectionStrings("SPCDB")
                    dS.ConnectionString = ctsSetting.ConnectionString
                    'sql設定
                    dS.SQL = objData(intData)
                    objReport(intData).DataSource = dS
                Case "System.Data.DataTable"    'DataTable指定
                    objReport(intData).DataSource = objData(intData)
                Case Else
                    psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    psClose_Window(Me)
                    Return
            End Select
            objReport(intData).Run(False)
        Catch ex As Exception
            If strName.Length <= intData _
                OrElse strName(intData) Is Nothing Then
                psMesBox(Me, "20004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            Else
                psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前, strName(intData))
            End If
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            psClose_Window(Me)
            Return
        End Try

        'ファイルを出力
        pdfExport.Export(objReport(intData).Document, mstStream)

        '表示指定
        mstStream.Position = 0
        Response.ContentType = "application/pdf"
        Response.AddHeader("title", strName(intData))
        Response.AddHeader("content-disposition", "inline;filename=""" & strName(intData) & ".pdf""")
        Response.BinaryWrite(mstStream.ToArray())
        Response.Flush()

    End Sub
#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
