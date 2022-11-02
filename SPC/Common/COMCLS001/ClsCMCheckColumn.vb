'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　GridViewテンプレートカラム(チェック)クラス
'*　ＰＧＭＩＤ：　ClsCMCheckColumn
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.27　：　土岐
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
#End Region

Public Class ClsCMCheckColumn
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Implements System.Web.UI.ITemplate

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
    Dim mstrID As String
#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    Public Property ppDataField As String
    Public Property ppReadOnly As Boolean = False
    Public Property ppType As Type = Type.GetType("System.Boolean")
#End Region

#Region "イベントプロシージャ"
#End Region

#Region "そのほかのプロシージャ"
    Sub New(ByVal ipstrID As String)
        mstrID = ipstrID
    End Sub

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) _
      Implements System.Web.UI.ITemplate.InstantiateIn

        Dim chkCheckBox As New CheckBox

        AddHandler chkCheckBox.DataBinding, AddressOf OnDataBinding
        If ppReadOnly Then
            chkCheckBox.Attributes.Add("OnClick", "return false")
        End If
        chkCheckBox.ID = mstrID
        container.Controls.Add(chkCheckBox)
    End Sub

    Public Sub OnDataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkCheckBox As CheckBox = CType(sender, CheckBox)

        Dim container As GridViewRow = CType(chkCheckBox.NamingContainer, GridViewRow)

        If container.DataItem(ppDataField).ToString = "1" Then
            chkCheckBox.Checked = True
        Else
            chkCheckBox.Checked = False
        End If

    End Sub
#End Region

End Class
