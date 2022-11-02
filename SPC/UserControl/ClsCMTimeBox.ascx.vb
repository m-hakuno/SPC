'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　時間ボックス
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.17　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.ComponentModel
'Imports SPC.Global_asax
'Imports clscomver

Public Class ClsCMTimeBox
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.UserControl
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
    'バリデーションのメッセージタイプ
    'Public Enum E_VMタイプ As Short
    '    メッセージ = 0
    '    ショート = 1
    '    アスタ = 2
    'End Enum
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
    'テキスト時
    Public Property ppHourText() As String
        Get
            Return txtHour.Text
        End Get
        Set(ByVal value As String)
            txtHour.Text = value
        End Set
    End Property

    'テキスト分
    Public Property ppMinText() As String
        Get
            Return txtMin.Text
        End Get
        Set(ByVal value As String)
            txtMin.Text = value
        End Set
    End Property

    '必須有無
    Private Const mblnReqFieldDef = False
    Private mblnReqFieldVal As Boolean = mblnReqFieldDef
    <DefaultValue(mblnReqFieldDef)> _
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
            Return pnlName.Width
        End Get
        Set(ByVal value As Unit)
            pnlName.Width = value
        End Set
    End Property

    'TabIndex
    Public Property ppTabIndex() As Short
        Get
            Return txtHour.TabIndex
        End Get
        Set(ByVal value As Short)
            txtHour.TabIndex = value
            txtMin.TabIndex = Short.Parse((value + 1).ToString)
        End Set
    End Property

    'テキストボックス時
    Public ReadOnly Property ppHourBox() As TextBox
        Get
            Return txtHour
        End Get
    End Property

    'テキストボックス分
    Public ReadOnly Property ppMinBox() As TextBox
        Get
            Return txtMin
        End Get
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

    '24時以降の可否
    Private Const mblnOverDef = False
    Private mblnOverVal As Boolean = mblnOverDef
    <DefaultValue(mblnOverDef)> _
    Public Property ppOver() As Boolean
        Get
            Return mblnOverVal
        End Get
        Set(ByVal value As Boolean)
            mblnOverVal = value
        End Set
    End Property

    '検証グループ
    Public Property ppValidationGroup() As String
        Get
            Return cuvTimeBox.ValidationGroup
        End Get
        Set(ByVal value As String)
            cuvTimeBox.ValidationGroup = value
        End Set
    End Property

    '検証メッセージタイプ
    Private mshtMesType As ClsComVer.E_VMタイプ = ClsComVer.E_VMタイプ.ショート
    Public Property ppMesType() As ClsComVer.E_VMタイプ
        Get
            Return mshtMesType
        End Get
        Set(value As ClsComVer.E_VMタイプ)
            mshtMesType = value
        End Set
    End Property

#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '-----------------------------
        '2014/04/23 土岐　ここから
        '-----------------------------
        txtHour.Attributes.Add("onBlur", "setTime(this)")
        txtMin.Attributes.Add("onBlur", "setTime(this)")
        '-----------------------------
        '2014/04/23 土岐　ここまで
        '-----------------------------
    End Sub

    Protected Sub cuvTimeBox_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvTimeBox.ServerValidate
        If pnlCtrl.Enabled = True Then
            Dim strErrNo As String

            args.IsValid = True

            '-----------------------------
            '2014/04/23 土岐　ここから
            '-----------------------------
            If Regex.IsMatch(
                txtHour.Text,
                "^^\d{1}$") Then
                txtHour.Text = "0" & txtHour.Text
            End If

            If Regex.IsMatch(
                txtMin.Text,
                "^^\d{1}$") Then
                txtMin.Text = "0" & txtMin.Text
            End If
            '-----------------------------
            '2014/04/23 土岐　ここまで
            '-----------------------------
            strErrNo = ClsCMCommon.pfCheck_TimeErr(txtHour.Text, txtMin.Text, mblnReqFieldVal, mblnOverVal)

            If strErrNo <> String.Empty Then
                'エラー
                psSet_ErrorNo(strErrNo, lblName.Text)
                args.IsValid = cuvTimeBox.IsValid
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
        If cuvTimeBox.IsValid Then      '正常時のみ設定
            Select Case mshtMesType
                Case ClsComVer.E_VMタイプ.アスタ
                    cuvTimeBox.Text = P_VALMES_AST
                Case ClsComVer.E_VMタイプ.ショート
                    cuvTimeBox.Text = ipdtrMes.Item(P_VALMES_SMES)
                Case ClsComVer.E_VMタイプ.メッセージ
                    cuvTimeBox.Text = ipdtrMes.Item(P_VALMES_MES)
                Case Else
                    cuvTimeBox.Text = String.Empty
            End Select
            cuvTimeBox.ErrorMessage = ipdtrMes.Item(P_VALMES_MES)
            cuvTimeBox.IsValid = False
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
