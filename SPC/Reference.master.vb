'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　照会マスターページ
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.29　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================

Public Class reference
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.MasterPage
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
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
#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    '件数
    Public Property ppCount() As String
        Get
            Return lblCount.Text
        End Get
        Set(value As String)
            lblCount.Text = value
        End Set
    End Property
    '-----------------------------
    '2014/04/25 土岐　ここから
    '-----------------------------
    '件数　表示／非表示
    Public Property ppCount_Visible() As Boolean
        Get
            Return divCount.Visible
        End Get
        Set(value As Boolean)
            divCount.Visible = value
        End Set
    End Property
    '-----------------------------
    '2014/04/25 土岐　ここまで
    '-----------------------------

    'マルチビュー
    Public ReadOnly Property ppMultiView() As MultiView
        Get
            Return muvList
        End Get
    End Property

    '右ボタン1
    Public ReadOnly Property ppRigthButton1() As Button
        Get
            Return btnSearchRigth1
        End Get
    End Property

    '右ボタン2
    Public ReadOnly Property ppRigthButton2() As Button
        Get
            Return btnSearchRigth2
        End Get
    End Property

    '右ボタン3
    Public ReadOnly Property ppRigthButton3() As Button
        Get
            Return btnSearchRigth3
        End Get
    End Property

    '右ボタン4
    Public ReadOnly Property ppRigthButton4() As Button
        Get
            Return btnSearchRigth4
        End Get
    End Property

    '右ボタン5
    Public ReadOnly Property ppRigthButton5() As Button
        Get
            Return btnSearchRigth5
        End Get
    End Property
#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Master.ppBcList_Visible = True
'        Master.ppLogout_Mode = Global_asax. ClsComVer.E_ログアウトモード.閉じる
        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる
    End Sub
#End Region

#Region "そのほかのプロシージャ"
#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
