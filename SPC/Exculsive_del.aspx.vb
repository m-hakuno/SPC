'********************************************************************************************************************************
'*　システム　：　サポートセンタステム <排他>
'*　処理名　　：　×ボタン押下時の排他制御削除処理実行画面
'*　ＰＧＭＩＤ：　Exculsive_del
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.04.12　：　高松
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.IO
Imports System.String
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports SPC.ClsCMExclusive

Public Class Exculsive_del
    Inherits System.Web.UI.Page

    Dim clsExc As New ClsCMExclusive

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim flag As String = 0                           'フラグ(0:排他情報 1:排他情報と明細情報あり)
        Dim ExculsiveDate As String = String.Empty       '排他情報
        Dim ExculsiveDateDtl As String = String.Empty    '排他情報(明細)

        'URL変数からフラグを取得
        flag = Request.QueryString.Get("flag").ToString

        '★排他情報削除
        If flag = "0" Or flag = "1" Then
            ExculsiveDate = Request.QueryString.Get("Excul_date").ToString
            clsExc.pfDel_Exclusive(Me _
                            , Session(P_SESSION_SESSTION_ID) _
                            , ExculsiveDate)
        End If

        '★排他情報削除(明細)
        If flag = "1" Then
            '--------------------------------
            '2014/06/10 後藤　ここから
            '--------------------------------
            'ExculsiveDateDtl = Request.QueryString.Get("Excul_date").ToString
            ExculsiveDateDtl = Request.QueryString.Get("Excul_date_dtl").ToString
            '--------------------------------
            '2014/06/10 後藤　ここまで
            '--------------------------------
            clsExc.pfDel_Exclusive(Me _
                            , Session(P_SESSION_SESSTION_ID) _
                            , ExculsiveDateDtl)
        End If

        '画面終了
        psClose_Window(Me)

    End Sub

End Class