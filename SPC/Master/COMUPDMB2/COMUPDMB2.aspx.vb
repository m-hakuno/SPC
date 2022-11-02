'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　処理結果コード参照
'*　ＰＧＭＩＤ：　COMUPDMB2
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2017.07.13　：　加賀
'********************************************************************************************************************************


Imports System.Data.SqlClient


Public Class COMUPDMB2

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    Const DispCode As String = "COMUPDMB2"
    'Const MasterName As String = "DLL設定依頼内容マスタ"

#End Region

#Region "変数定義"

    Dim clsDataConnect As New ClsCMDataConnect

#End Region

#Region "プロパティ定義"

#End Region

#Region "イベントプロージャ"

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '画面初期化処理
        If Not IsPostBack Then
            getData()
        End If

    End Sub

#End Region

#Region "そのほかのプロージャ"

    ''' <summary>
    ''' データ取得
    ''' </summary>
    Private Sub getData()

        Dim dstSelect As New DataSet

        'SQLコマンド設定
        Using cmdSQL As New SqlCommand("COMUPDMB2_S01")

            ''パラメータ設定
            'cmdSQL.Parameters.AddRange({New SqlParameter("@mail_no1", strPrmAry(0)) _
            '                          , New SqlParameter("@mail_no2", strPrmAry(1)) _
            '                          , New SqlParameter("@mail_no3", strPrmAry(2)) _
            '                          , New SqlParameter("@vst_cls", strPrmAry(3)) _
            '                          , New SqlParameter("@wrk_day", strPrmAry(4)) _
            '                          , New SqlParameter("@wrk_tim", strPrmAry(5)) _
            '                           })

            'データ更新
            If ClsCMDataConnect.pfExec_StoredProcedure(Me, "明細", cmdSQL, dstSelect) = False Then

                Exit Sub

            End If

        End Using

        '該当件数表示
        If dstSelect.Tables(0).Rows.Count = 0 Then

            '該当データ無し
            'psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)

        Else
            '画面反映
            grvList1.DataSource = dstSelect.Tables(0)
            grvList1.DataBind()
            grvList2.DataSource = dstSelect.Tables(1)
            grvList2.DataBind()
        End If

        'Dispose
        dstSelect.Dispose()

    End Sub

#End Region

End Class
