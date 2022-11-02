'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　ドロップダウンリストマスタ用
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.09　：　酒井
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
Imports System.Web.UI.WebControls.DropDownList
Imports System.Web.UI.WebControls.SqlDataSource
'Imports clscomver

Public Class ClsCMDropDownList_Mastr

#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.UserControl

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    '取得する条件の"M29_CLASS_CD"
    Private mstrClassCD As String
    Public Property ppClassCD() As String
        Get
            Return mstrClassCD
        End Get
        Set(ByVal value As String)
            mstrClassCD = value
        End Set
    End Property

    '名称表示、略称表示の切り替え
    Public Property ppMode() As ClsComVer.E_リスト表示名
        Get
            Return ddlList.DataTextField
        End Get
        Set(ByVal value As ClsComVer.E_リスト表示名)
            ddlList.DataTextField = value
        End Set
    End Property

    'リストの選択項目の値
    Public Property ppSelectedValue() As String
        Get
            Return ddlList.SelectedValue
        End Get
        Set(ByVal value As String)
            ddlList.SelectedValue = value

        End Set
    End Property

    'リストの選択項目の値
    Public Property ppDataValueField() As String
        Get
            Return ddlList.DataValueField
        End Get
        Set(ByVal value As String)
            ddlList.DataValueField = value

        End Set
    End Property

    'DataTextFieldの値
    Public Property ppDataTextField() As String
        Get
            Return ddlList.DataTextField
        End Get
        Set(ByVal value As String)
            ddlList.DataTextField = value

        End Set
    End Property

    'DataSourceIDの値
    Public Property ppDataSourceID() As String
        Get
            Return ddlList.DataSourceID
        End Get
        Set(ByVal value As String)
            ddlList.DataSourceID = value

        End Set
    End Property


    '必須有無
    Private Const M_REQFIELD = False
    Private mblnReqFieldVal As Boolean = M_REQFIELD
    <DefaultValue(M_REQFIELD)> _
    Public Property ppRequiredField() As Boolean
        Get
            Return mblnReqFieldVal
        End Get
        Set(ByVal value As Boolean)
            mblnReqFieldVal = value
        End Set
    End Property

    '項目名
    Public Property ppName() As String
        Get
            Return lblName.Text
        End Get
        Set(ByVal value As String)
            lblName.Text = value
        End Set
    End Property

    '文字幅
    Public Property ppWidth() As Unit
        Get
            Return ddlList.Width
        End Get
        Set(ByVal value As Unit)
            ddlList.Width = value
        End Set
    End Property

    '項目名の表示
    Public Property ppNameVisible() As Boolean
        Get
            Return lblName.Visible
        End Get
        Set(ByVal value As Boolean)
            lblName.Visible = value
        End Set
    End Property

    '項目名幅
    Public Property ppNameWidth() As Unit
        Get
            Return lblName.Width
        End Get
        Set(ByVal value As Unit)
            lblName.Width = value
        End Set
    End Property

    '未選択の有無
    Private Const M_NOTSELECT = False
    Private mblnNotSelectVal As Boolean = M_NOTSELECT
    <DefaultValue(M_NOTSELECT)> _
    Public Property ppNotSelect As Boolean
        Get
            Return mblnNotSelectVal
        End Get
        Set(ByVal value As Boolean)
            mblnNotSelectVal = value
        End Set
    End Property

    '活性/非活性
    <DefaultValue(True)> _
    Public Property ppEnabled() As Boolean
        Get
            Return pnlCtrl.Enabled
        End Get
        Set(ByVal value As Boolean)
            pnlCtrl.Enabled = value
        End Set
    End Property

    '検証グループ
    Public Property ppValidationGroup() As String
        Get
            Return cuvDropDownList.ValidationGroup
        End Get
        Set(ByVal value As String)
            cuvDropDownList.ValidationGroup = value
        End Set
    End Property

    '検証メッセージタイプ
    Private mshtMesType As clscomver.E_VMタイプ = ClsComVer.E_VMタイプ.ショート
    Public Property ppMesType() As clscomver.E_VMタイプ
        Get
            Return mshtMesType
        End Get
        Set(value As clscomver.E_VMタイプ)
            mshtMesType = value
        End Set
    End Property

    'TabIndex
    Public Property ppTabIndex() As Short
        Get
            Return ddlList.TabIndex
        End Get
        Set(ByVal value As Short)
            ddlList.TabIndex = value

        End Set
    End Property

    'DropDownList
    Public ReadOnly Property ppDropDownList() As DropDownList
        Get
            Return ddlList
        End Get
    End Property

#End Region


#Region "イベントプロシージャ"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then

            '未選択追加
            If mblnNotSelectVal Then
                ddlList.Items.Insert(0, New ListItem(Nothing, Nothing))
            End If

        Else
            psMesBox(Me.Page, "00005", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後)
        End If

    End Sub

    Protected Sub cuvTextBox_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvDropDownList.ServerValidate
        '活性時のみチェック
        If pnlCtrl.Enabled = True Then
            Dim strErrNo As String

            args.IsValid = True

            'エラーチェック
            strErrNo = ClsCMCommon.pfCheck_ListErr(args.Value, mblnReqFieldVal)

            If strErrNo <> String.Empty Then
                'エラー
                psSet_ErrorNo(strErrNo, lblName.Text)
                args.IsValid = cuvDropDownList.IsValid
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' エラーメッセージを設定する。
    ''' </summary>
    ''' <param name="ipdtrMes">エラーメッセージ</param>
    ''' <remarks></remarks>
    Private Sub msSet_ErrorMes(ByVal ipdtrMes As DataRow)
        If cuvDropDownList.IsValid Then     '正常時のみ設定
            Select Case mshtMesType
                Case ClsComVer.E_VMタイプ.アスタ
                    cuvDropDownList.Text = P_VALMES_AST
                Case ClsComVer.E_VMタイプ.ショート
                    cuvDropDownList.Text = ipdtrMes.Item(P_VALMES_SMES)
                Case ClsComVer.E_VMタイプ.メッセージ
                    cuvDropDownList.Text = ipdtrMes.Item(P_VALMES_MES)
                Case Else
                    cuvDropDownList.Text = String.Empty
            End Select
            cuvDropDownList.ErrorMessage = ipdtrMes.Item(P_VALMES_MES)
            cuvDropDownList.IsValid = False
        End If
    End Sub

    ''' <summary>
    ''' エラーを設定する。
    ''' </summary>
    ''' <param name="ipstrNo">エラーNo.</param>
    ''' <remarks></remarks>
    Public Overloads Sub psSet_ErrorNo(ByVal ipstrNo As String)
        Dim dtrErrMes As DataRow

        dtrErrMes = ClsCMCommon.pfGet_ValMes(ipstrNo)
        msSet_ErrorMes(dtrErrMes)

    End Sub
    ''' <summary>
    ''' エラーを設定する。
    ''' </summary>
    ''' <param name="ipstrNo">エラーNo.</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <remarks></remarks>
    Public Overloads Sub psSet_ErrorNo(ByVal ipstrNo As String,
                                       ByVal ipstrPrm1 As String)
        Dim dtrErrMes As DataRow

        dtrErrMes = ClsCMCommon.pfGet_ValMes(ipstrNo, ipstrPrm1)
        msSet_ErrorMes(dtrErrMes)

    End Sub
    ''' <summary>
    ''' エラーを設定する。
    ''' </summary>
    ''' <param name="ipstrNo">エラーNo.</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <remarks></remarks>
    Public Overloads Sub psSet_ErrorNo(ByVal ipstrNo As String,
                                       ByVal ipstrPrm1 As String,
                                       ByVal ipstrPrm2 As String)
        Dim dtrErrMes As DataRow

        dtrErrMes = ClsCMCommon.pfGet_ValMes(ipstrNo, ipstrPrm1, ipstrPrm2)
        msSet_ErrorMes(dtrErrMes)

    End Sub
    ''' <summary>
    ''' エラーを設定する。
    ''' </summary>
    ''' <param name="ipstrNo">エラーNo.</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <remarks></remarks>
    Public Overloads Sub psSet_ErrorNo(ByVal ipstrNo As String,
                                       ByVal ipstrPrm1 As String,
                                       ByVal ipstrPrm2 As String,
                                       ByVal ipstrPrm3 As String)
        Dim dtrErrMes As DataRow

        dtrErrMes = ClsCMCommon.pfGet_ValMes(ipstrNo, ipstrPrm1, ipstrPrm2, ipstrPrm3)
        msSet_ErrorMes(dtrErrMes)

    End Sub
#End Region

#Region "終了処理プロシージャ"
#End Region


End Class