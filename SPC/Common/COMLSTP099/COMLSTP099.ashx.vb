'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　ダウンロード
'*　ＰＧＭＩＤ：　COMLSTP099
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.20　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Imports System.Web
Imports System.Web.Services

#End Region


Public Class COMLSTP0991

    Implements System.Web.IHttpHandler

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ファイルダウンロード処理
    ''' </summary>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim response As HttpResponse = context.Response
        Dim request As HttpRequest = context.Request

        Dim strFilePath As String = request.QueryString("path")
        Dim strDownloadFileName As String = request.QueryString("filename")

        If (request.Browser.Browser.ToUpper().IndexOf("IE") >= 0) Then
            strDownloadFileName = context.Server.UrlEncode(strDownloadFileName)
        End If

        'ダウンロード開始
        response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(strDownloadFileName))
        response.ContentType = "text/plain"
        response.HeaderEncoding = System.Text.Encoding.GetEncoding("shift_jis")
        response.ContentEncoding = System.Text.Encoding.GetEncoding("shift_jis")
        response.TransmitFile(strFilePath)  ' 指定したファイル

    End Sub

    ''' <summary>
    ''' IsReusable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

#End Region

End Class
