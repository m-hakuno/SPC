'********************************************************************************************************************************
'*　システム　：　サポートセンタシステム
'*　処理名　　：　テキストボックス参照
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.29　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.ComponentModel
'Imports SPC.Global_asax

Public Class ClsCMTextBoxRef
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
            Return txtTextBox.Text
        End Get
        Set(ByVal value As String)
            txtTextBox.Text = value
        End Set
    End Property

    'テキストモード
    Public Property ppTextMode() As TextBoxMode
        Get
            Return txtTextBox.TextMode
        End Get
        Set(ByVal value As TextBoxMode)
            txtTextBox.TextMode = value
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

    '数値有無
    Private Const mblnNumDef = False
    Private mblnNumF As Boolean = mblnNumDef
    <DefaultValue(mblnNumDef)> _
    Public Property ppNum() As Boolean
        Get
            Return mblnNumF
        End Get
        Set(ByVal value As Boolean)
            mblnNumF = value
        End Set
    End Property

    '最大文字数
    Public Property ppMaxLength() As Integer
        Get
            Return txtTextBox.MaxLength
        End Get
        Set(ByVal value As Integer)
            txtTextBox.MaxLength = value
        End Set
    End Property

    '半角チェック
    Private Const mblnCheckHanDef = False
    Private mblnCheckHan As Boolean = mblnCheckHanDef
    <DefaultValue(mblnCheckHanDef)> _
    Public Property ppCheckHan As Boolean
        Get
            Return mblnCheckHan
        End Get
        Set(ByVal value As Boolean)
            mblnCheckHan = value
        End Set
    End Property

    '英字チェック
    Private Const mblnCheckAcDef = False
    Private mblnCheckAc As Boolean = mblnCheckAcDef
    <DefaultValue(mblnCheckAcDef)> _
    Public Property ppCheckAc As Boolean
        Get
            Return mblnCheckAc
        End Get
        Set(ByVal value As Boolean)
            mblnCheckAc = value
        End Set
    End Property

    '文字数一致チェック
    Private Const mblnCheckLengthDef = False
    Private mblnCheckLength As Boolean = mblnCheckLengthDef
    <DefaultValue(mblnCheckLengthDef)> _
    Public Property ppCheckLength() As Boolean
        Get
            Return mblnCheckLength
        End Get
        Set(ByVal value As Boolean)
            mblnCheckLength = value
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

    '文字幅
    Public Property ppWidth() As Unit
        Get
            Return txtTextBox.Width
        End Get
        Set(ByVal value As Unit)
            txtTextBox.Width = value
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

    '検証する正規表現
    Private mstrExpression As String = ""
    Public Property ppExpression() As String
        Get
            Return mstrExpression
        End Get
        Set(ByVal value As String)
            mstrExpression = value
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
            Return cuvTextBox.ValidationGroup
        End Get
        Set(ByVal value As String)
            cuvTextBox.ValidationGroup = value
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
            Return txtTextBox.TabIndex
        End Get
        Set(ByVal value As Short)
            txtTextBox.TabIndex = value
        End Set
    End Property

    'TextBox
    Public ReadOnly Property ppTextBox() As TextBox
        Get
            Return txtTextBox
        End Get
    End Property

    'IMEモード
    Private mshtIMEMode As ClsComVer.E_IMEモード
    Public Property ppIMEMode As ClsComVer.E_IMEモード
        Get
            Return mshtIMEMode
        End Get
        Set(ByVal value As ClsComVer.E_IMEモード)
            mshtIMEMode = value
            Select Case mshtIMEMode
                Case ClsComVer.E_IMEモード.全角
                    txtTextBox.CssClass = P_CSS_ZEN
                Case ClsComVer.E_IMEモード.半角_変更可
                    txtTextBox.CssClass = P_CSS_HAN_COK
                Case ClsComVer.E_IMEモード.半角_変更不可
                    txtTextBox.CssClass = P_CSS_HAN_CNG
            End Select
        End Set
    End Property

    '参照先
    Private mstrURL As String
    Public Property ppURL As String
        Get
            Return mstrURL
        End Get
        Set(ByVal value As String)
            mstrURL = value
            btnRef.OnClientClick = "return window_open('" & VirtualPathUtility.ToAbsolute(mstrURL) & "')"
        End Set
    End Property

    '参照ボタン
    Public ReadOnly Property ppButton As Button
        Get
            Return btnRef
        End Get
    End Property

    '文字表示位置
    Private mshtTextAlign As ClsComVer.E_文字位置 = ClsComVer.E_文字位置.左
    Public Property ppTextAlign As ClsComVer.E_文字位置
        Get
            Return mshtTextAlign
        End Get
        Set(value As ClsComVer.E_文字位置)
            mshtTextAlign = value
            Select Case mshtTextAlign
                Case ClsComVer.E_文字位置.右
                    txtTextBox.Style.Item(HtmlTextWriterStyle.TextAlign) = "Right"
                Case ClsComVer.E_文字位置.左
                    txtTextBox.Style.Item(HtmlTextWriterStyle.TextAlign) = "left"
                Case ClsComVer.E_文字位置.中央
                    txtTextBox.Style.Item(HtmlTextWriterStyle.TextAlign) = "Center"
            End Select
        End Set
    End Property
#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub cuvTextBox_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cuvTextBox.ServerValidate
        If pnlCtrl.Enabled = True Then
            Dim strErrNo As String

            args.IsValid = True

            strErrNo = ClsCMCommon.pfCheck_TxtErr(args.Value,
                                                   mblnReqFieldVal,
                                                   mblnNumF,
                                                   mblnCheckHan,
                                                   mblnCheckLength,
                                                   txtTextBox.MaxLength,
                                                   mstrExpression,
                                                   mblnCheckAc)

            If strErrNo <> String.Empty Then
                'エラー
                If strErrNo = "4001" Then
                    psSet_ErrorNo(strErrNo, lblName.Text, "正しい形式")
                Else
                    psSet_ErrorNo(strErrNo, lblName.Text, txtTextBox.MaxLength.ToString)
                End If
                args.IsValid = cuvTextBox.IsValid
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
        If cuvTextBox.IsValid Then      '正常時のみ設定
            Select Case mshtMesType
                Case ClsComVer.E_VMタイプ.アスタ
                    cuvTextBox.Text = P_VALMES_AST
                Case ClsComVer.E_VMタイプ.ショート
                    cuvTextBox.Text = ipdtrMes.Item(P_VALMES_SMES)
                Case ClsComVer.E_VMタイプ.メッセージ
                    cuvTextBox.Text = ipdtrMes.Item(P_VALMES_MES)
                Case Else
                    cuvTextBox.Text = String.Empty
            End Select
            cuvTextBox.ErrorMessage = ipdtrMes.Item(P_VALMES_MES)
            cuvTextBox.IsValid = False
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
