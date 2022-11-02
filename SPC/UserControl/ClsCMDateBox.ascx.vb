'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　日付ボックス
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.16　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.ComponentModel
'Imports SPC.Global_asax

Public Class ClsCMDateBox
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
    'テキスト
    Public Property ppText() As String
        Get
            Return txtDateBox.Text
        End Get
        Set(ByVal value As String)
            txtDateBox.Text = value
        End Set
    End Property

    '日付
    Public Property ppDate() As Date
        Get
            If DateTime.TryParse(txtDateBox.Text, Nothing) Then
                Return Date.Parse(txtDateBox.Text)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As Date)
            Select Case mdafDataFormat
                Case ClsComVer.E_日付形式.年月日
                    txtDateBox.Text = value.ToString("yyyy/MM/dd")
                Case ClsComVer.E_日付形式.年月
                    txtDateBox.Text = value.ToString("yyyy/MM")
            End Select
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

    '日付の形式
    Private mdafDataFormat As ClsComVer.E_日付形式 = ClsComVer.E_日付形式.年月日
    Public Property ppDateFormat() As ClsComVer.E_日付形式
        Get
            Return mdafDataFormat
        End Get
        Set(ByVal value As ClsComVer.E_日付形式)
            mdafDataFormat = value
            Select Case mdafDataFormat
                Case ClsComVer.E_日付形式.年月
                    txtDateBox_CalendarExtender.Format = "yyyy/MM"
                    txtDateBox.MaxLength = 7
                    txtDateBox.Width = 48
                Case ClsComVer.E_日付形式.年月日
                    txtDateBox_CalendarExtender.Format = "yyyy/MM/dd"
                    txtDateBox.MaxLength = 10
                    txtDateBox.Width = 67
            End Select
        End Set
    End Property

    '曜日の表示
    <DefaultValue(False)> _
    Public Property ppYobiVisible() As Boolean
        Get
            Return lblYobi.Visible
        End Get
        Set(ByVal value As Boolean)
            lblYobi.Visible = value
        End Set
    End Property

    '曜日の表示
    Public Property ppYobiText() As String
        Get
            Return lblYobi.Text
        End Get
        Set(ByVal value As String)
            lblYobi.Text = value
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
            Return cuvDateBox.ValidationGroup
        End Get
        Set(ByVal value As String)
            cuvDateBox.ValidationGroup = value
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

    'TabIndex
    Public Property ppTabIndex() As Short
        Get
            Return txtDateBox.TabIndex
        End Get
        Set(ByVal value As Short)
            txtDateBox.TabIndex = value
        End Set
    End Property

    'DateBox
    Public ReadOnly Property ppDateBox() As TextBox
        Get
            Return txtDateBox
        End Get
    End Property
#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Select Case mdafDataFormat
            Case ClsComVer.E_日付形式.年月
                txtDateBox.Attributes.Add("onBlur", "setDateM(this)")
            Case ClsComVer.E_日付形式.年月日
                txtDateBox.Attributes.Add("onBlur", "setDate(this, document.getElementById(""" + Me.lblYobi.ClientID + """))")
                ibtDate.Attributes.Add("onFocus", "setDate(document.getElementById(""" +
                                       Me.txtDateBox.ClientID +
                                       """), document.getElementById(""" +
                                       Me.lblYobi.ClientID +
                                       """))")
        End Select
    End Sub

    Protected Sub cuvDateBox_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvDateBox.ServerValidate
        '活性時のみチェック
        If pnlCtrl.Enabled = True Then
            Dim strErrNo As String
            args.IsValid = True
            '-----------------------------
            '2014/04/23 土岐　ここから
            '-----------------------------
            Dim dttDate As DateTime
            If DateTime.TryParse(txtDateBox.Text, dttDate) Then
                Select Case mdafDataFormat
                    Case ClsComVer.E_日付形式.年月
                        txtDateBox.Text = dttDate.ToString("yyyy/MM")
                    Case ClsComVer.E_日付形式.年月日
                        txtDateBox.Text = dttDate.ToString("yyyy/MM/dd")
                End Select
            End If
            'エラーチェック
            strErrNo = ClsCMCommon.pfCheck_DateErr(txtDateBox.Text, mblnReqFieldVal, mdafDataFormat)
            '-----------------------------
            '2014/04/23 土岐　ここまで
            '-----------------------------
            If strErrNo <> String.Empty Then

                Select Case strErrNo
                    Case "6002"
                        'エラー
                        psSet_ErrorNo(strErrNo, lblName.Text, "1753/01/01 ～ 9999/12/31")
                        args.IsValid = cuvDateBox.IsValid
                        Exit Sub
                    Case Else
                        'エラー
                        psSet_ErrorNo(strErrNo, lblName.Text, "正しい日付")
                        args.IsValid = cuvDateBox.IsValid
                        Exit Sub
                End Select

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
        If cuvDateBox.IsValid Then  '正常時のみ設定
            Select Case mshtMesType
                Case ClsComVer.E_VMタイプ.アスタ
                    cuvDateBox.Text = P_VALMES_AST
                Case ClsComVer.E_VMタイプ.ショート
                    cuvDateBox.Text = ipdtrMes.Item(P_VALMES_SMES)
                Case ClsComVer.E_VMタイプ.メッセージ
                    cuvDateBox.Text = ipdtrMes.Item(P_VALMES_MES)
                Case Else
                    cuvDateBox.Text = String.Empty
            End Select
            cuvDateBox.ErrorMessage = ipdtrMes.Item(P_VALMES_MES)
            cuvDateBox.IsValid = False
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
