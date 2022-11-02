'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　テキストボックス２FromTo
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

Public Class ClsCMTextBoxTwoFromTo
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
    'テキスト1From
    Public Property ppFromTextOne() As String
        Get
            Return txtTextBoxFrom.Text
        End Get
        Set(ByVal value As String)
            txtTextBoxFrom.Text = value
        End Set
    End Property

    'テキスト2From
    Public Property ppFromTextTwo() As String
        Get
            Return txtTextBoxTwoFrom.Text
        End Get
        Set(ByVal value As String)
            txtTextBoxTwoFrom.Text = value
        End Set
    End Property

    'テキストFrom
    Public Property ppFromText() As String
        Get
            Return txtTextBoxFrom.Text & txtTextBoxTwoFrom.Text
        End Get
        Set(ByVal value As String)
            If value.Length >= 0 Then
                If value.Length >= txtTextBoxFrom.MaxLength Then
                    txtTextBoxFrom.Text = value.Substring(0, txtTextBoxFrom.MaxLength)
                Else
                    txtTextBoxFrom.Text = value
                End If
            Else
                txtTextBoxFrom.Text = String.Empty
            End If
            If value.Length >= txtTextBoxFrom.MaxLength Then
                txtTextBoxTwoFrom.Text = value.Substring(txtTextBoxFrom.MaxLength)
            Else
                txtTextBoxTwoFrom.Text = String.Empty
            End If
        End Set
    End Property

    'テキスト1To
    Public Property ppToTextOne() As String
        Get
            Return txtTextBoxTo.Text
        End Get
        Set(ByVal value As String)
            txtTextBoxTo.Text = value
        End Set
    End Property

    'テキスト2To
    Public Property ppToTextTwo() As String
        Get
            Return txtTextBoxTwoTo.Text
        End Get
        Set(ByVal value As String)
            txtTextBoxTwoTo.Text = value
        End Set
    End Property

    'テキストTo
    Public Property ppToText() As String
        Get
            Return txtTextBoxTo.Text & txtTextBoxTwoTo.Text
        End Get
        Set(ByVal value As String)
            If value.Length >= 0 Then
                If value.Length >= txtTextBoxTo.MaxLength Then
                    txtTextBoxTo.Text = value.Substring(0, txtTextBoxTo.MaxLength)
                Else
                    txtTextBoxTo.Text = value
                End If
            Else
                txtTextBoxTo.Text = String.Empty
            End If
            If value.Length >= txtTextBoxTo.MaxLength Then
                txtTextBoxTwoTo.Text = value.Substring(txtTextBoxTo.MaxLength)
            Else
                txtTextBoxTwoTo.Text = String.Empty
            End If
        End Set
    End Property

    'テキストモード1
    Public Property ppTextModeOne() As TextBoxMode
        Get
            Return txtTextBoxFrom.TextMode
        End Get
        Set(ByVal value As TextBoxMode)
            txtTextBoxFrom.TextMode = value
            txtTextBoxTo.TextMode = value
        End Set
    End Property

    'テキストモード2
    Public Property ppTextModeTwo() As TextBoxMode
        Get
            Return txtTextBoxTwoFrom.TextMode
        End Get
        Set(ByVal value As TextBoxMode)
            txtTextBoxTwoFrom.TextMode = value
            txtTextBoxTwoTo.TextMode = value
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

    '数値有無1
    Private Const mblnNumDef = False
    Private mblnNumFOne As Boolean = mblnNumDef
    <DefaultValue(mblnNumDef)> _
    Public Property ppNumOne() As Boolean
        Get
            Return mblnNumFOne
        End Get
        Set(ByVal value As Boolean)
            mblnNumFOne = value
        End Set
    End Property

    '数値有無2
    Private mblnNumFTwo As Boolean = mblnNumDef
    <DefaultValue(mblnNumDef)> _
    Public Property ppNumTwo() As Boolean
        Get
            Return mblnNumFTwo
        End Get
        Set(ByVal value As Boolean)
            mblnNumFTwo = value
        End Set
    End Property

    '最大文字数1
    Public Property ppMaxLengthOne() As Integer
        Get
            Return txtTextBoxFrom.MaxLength
        End Get
        Set(ByVal value As Integer)
            txtTextBoxFrom.MaxLength = value
            txtTextBoxTo.MaxLength = value
        End Set
    End Property

    '最大文字数2
    Public Property ppMaxLengthTwo() As Integer
        Get
            Return txtTextBoxTwoFrom.MaxLength
        End Get
        Set(ByVal value As Integer)
            txtTextBoxTwoFrom.MaxLength = value
            txtTextBoxTwoTo.MaxLength = value
        End Set
    End Property

    '半角チェック
    Private Const mblnCheckHanDef = False
    Private mblnCheckHanOne As Boolean = mblnCheckHanDef
    <DefaultValue(mblnCheckHanDef)> _
    Public Property ppCheckHanOne As Boolean
        Get
            Return mblnCheckHanOne
        End Get
        Set(ByVal value As Boolean)
            mblnCheckHanOne = value
        End Set
    End Property

    '半角チェック
    Private mblnCheckHanTwo As Boolean = mblnCheckHanDef
    <DefaultValue(mblnCheckHanDef)> _
    Public Property ppCheckHanTwo As Boolean
        Get
            Return mblnCheckHanTwo
        End Get
        Set(ByVal value As Boolean)
            mblnCheckHanTwo = value
        End Set
    End Property

    '英字チェック1
    Private Const mblnCheckAcDef = False
    Private mblnCheckAcOne As Boolean = mblnCheckAcDef
    <DefaultValue(mblnCheckAcDef)> _
    Public Property ppCheckAcOne As Boolean
        Get
            Return mblnCheckAcOne
        End Get
        Set(ByVal value As Boolean)
            mblnCheckAcOne = value
        End Set
    End Property

    '英字チェック2
    Private mblnCheckAcTwo As Boolean = mblnCheckAcDef
    <DefaultValue(mblnCheckAcDef)> _
    Public Property ppCheckAcTwo As Boolean
        Get
            Return mblnCheckAcTwo
        End Get
        Set(ByVal value As Boolean)
            mblnCheckAcTwo = value
        End Set
    End Property

    '文字数一致チェック1
    Private Const mblnCheckLengthOneDef = False
    Private mblnCheckLengthOne As Boolean = mblnCheckLengthOneDef
    <DefaultValue(mblnCheckLengthOneDef)> _
    Public Property ppCheckLengthOne() As Boolean
        Get
            Return mblnCheckLengthOne
        End Get
        Set(ByVal value As Boolean)
            mblnCheckLengthOne = value
        End Set
    End Property

    '文字数一致チェック2
    Private Const mblnCheckLengthTwoDef = False
    Private mblnCheckLengthTwo As Boolean = mblnCheckLengthTwoDef
    <DefaultValue(mblnCheckLengthTwoDef)> _
    Public Property ppCheckLengthTwo() As Boolean
        Get
            Return mblnCheckLengthTwo
        End Get
        Set(ByVal value As Boolean)
            mblnCheckLengthTwo = value
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

    '文字幅1
    Public Property ppWidthOne() As Unit
        Get
            Return txtTextBoxFrom.Width
        End Get
        Set(ByVal value As Unit)
            txtTextBoxFrom.Width = value
            txtTextBoxTo.Width = value
        End Set
    End Property

    '文字幅2
    Public Property ppWidthTwo() As Unit
        Get
            Return txtTextBoxTwoFrom.Width
        End Get
        Set(ByVal value As Unit)
            txtTextBoxTwoFrom.Width = value
            txtTextBoxTwoTo.Width = value
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

    '検証する正規表現1
    Private mstrExpressionOne As String = ""
    Public Property ppExpressionOne() As String
        Get
            Return mstrExpressionOne
        End Get
        Set(ByVal value As String)
            mstrExpressionOne = value
        End Set
    End Property

    '検証する正規表現2
    Private mstrExpressionTwo As String = ""
    Public Property ppExpressionTwo() As String
        Get
            Return mstrExpressionTwo
        End Get
        Set(ByVal value As String)
            mstrExpressionTwo = value
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
            Return txtTextBoxFrom.TabIndex
        End Get
        Set(ByVal value As Short)
            txtTextBoxFrom.TabIndex = value
            txtTextBoxTwoFrom.TabIndex = Short.Parse((value + 1).ToString)
            txtTextBoxTo.TabIndex = Short.Parse((value + 2).ToString)
            txtTextBoxTwoTo.TabIndex = Short.Parse((value + 3).ToString)
        End Set
    End Property

    'TextBoxFrom
    Public ReadOnly Property ppTextBoxOneFrom() As TextBox
        Get
            Return txtTextBoxFrom
        End Get
    End Property

    'TextBoxTwoFrom
    Public ReadOnly Property ppTextBoxTwo() As TextBox
        Get
            Return txtTextBoxTwoFrom
        End Get
    End Property

    'TextBoxTo
    Public ReadOnly Property ppTextBoxOneTo() As TextBox
        Get
            Return txtTextBoxTo
        End Get
    End Property

    'TextBoxTwoTo
    Public ReadOnly Property ppTextBoxTwoTo() As TextBox
        Get
            Return txtTextBoxTwoTo
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
                    txtTextBoxFrom.CssClass = P_CSS_ZEN
                    txtTextBoxTo.CssClass = P_CSS_ZEN
                Case ClsComVer.E_IMEモード.半角_変更可
                    txtTextBoxFrom.CssClass = P_CSS_HAN_COK
                    txtTextBoxTo.CssClass = P_CSS_HAN_COK
                Case ClsComVer.E_IMEモード.半角_変更不可
                    txtTextBoxFrom.CssClass = P_CSS_HAN_CNG
                    txtTextBoxTo.CssClass = P_CSS_HAN_CNG
            End Select
        End Set
    End Property

    'IMEモード
    Private mshtIMEModeTwo As ClsComVer.E_IMEモード
    Public Property ppIMEModeTwo As ClsComVer.E_IMEモード
        Get
            Return mshtIMEModeTwo
        End Get
        Set(ByVal value As ClsComVer.E_IMEモード)
            mshtIMEModeTwo = value
            Select Case mshtIMEModeTwo
                Case ClsComVer.E_IMEモード.全角
                    txtTextBoxTwoFrom.CssClass = P_CSS_ZEN
                    txtTextBoxTwoTo.CssClass = P_CSS_ZEN
                Case ClsComVer.E_IMEモード.半角_変更可
                    txtTextBoxTwoFrom.CssClass = P_CSS_HAN_COK
                    txtTextBoxTwoTo.CssClass = P_CSS_HAN_COK
                Case ClsComVer.E_IMEモード.半角_変更不可
                    txtTextBoxTwoFrom.CssClass = P_CSS_HAN_CNG
                    txtTextBoxTwoTo.CssClass = P_CSS_HAN_CNG
            End Select
        End Set
    End Property

    '文字表示位置
    Private mshtTextAlignOne As ClsComVer.E_文字位置 = ClsComVer.E_文字位置.左
    Public Property ppTextAlignOne As ClsComVer.E_文字位置
        Get
            Return mshtTextAlignOne
        End Get
        Set(value As ClsComVer.E_文字位置)
            mshtTextAlignOne = value
            Select Case mshtTextAlignOne
                Case ClsComVer.E_文字位置.右
                    txtTextBoxFrom.Style.Item(HtmlTextWriterStyle.TextAlign) = "Right"
                    txtTextBoxTo.Style.Item(HtmlTextWriterStyle.TextAlign) = "Right"
                Case ClsComVer.E_文字位置.左
                    txtTextBoxFrom.Style.Item(HtmlTextWriterStyle.TextAlign) = "left"
                    txtTextBoxTo.Style.Item(HtmlTextWriterStyle.TextAlign) = "left"
                Case ClsComVer.E_文字位置.中央
                    txtTextBoxFrom.Style.Item(HtmlTextWriterStyle.TextAlign) = "Center"
                    txtTextBoxTo.Style.Item(HtmlTextWriterStyle.TextAlign) = "Center"
            End Select
        End Set
    End Property

    '文字表示位置Two
    Private mshtTextAlignTwo As ClsComVer.E_文字位置 = ClsComVer.E_文字位置.左
    Dim aa As ClsComVer.E_文字位置
    Public Property ppTextAlignTwo As ClsComVer.E_文字位置
        Get
            Return mshtTextAlignTwo
        End Get
        Set(value As ClsComVer.E_文字位置)
            mshtTextAlignTwo = value
            Select Case mshtTextAlignTwo
                Case ClsComVer.E_文字位置.右
                    txtTextBoxTwoFrom.Style.Item(HtmlTextWriterStyle.TextAlign) = "Right"
                    txtTextBoxTwoTo.Style.Item(HtmlTextWriterStyle.TextAlign) = "Right"
                Case ClsComVer.E_文字位置.左
                    txtTextBoxTwoFrom.Style.Item(HtmlTextWriterStyle.TextAlign) = "left"
                    txtTextBoxTwoTo.Style.Item(HtmlTextWriterStyle.TextAlign) = "left"
                Case ClsComVer.E_文字位置.中央
                    txtTextBoxTwoFrom.Style.Item(HtmlTextWriterStyle.TextAlign) = "Center"
                    txtTextBoxTwoTo.Style.Item(HtmlTextWriterStyle.TextAlign) = "Center"
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
            Dim strMaxLength As String

            args.IsValid = True
            strMaxLength = (txtTextBoxFrom.MaxLength + txtTextBoxTwoFrom.MaxLength).ToString

            'Fromチェック
            'TextBox1Fromチェック
            strErrNo = ClsCMCommon.pfCheck_TxtErr(txtTextBoxFrom.Text,
                                                   False,
                                                   mblnNumFOne,
                                                   mblnCheckHanOne,
                                                   mblnCheckLengthOne,
                                                   txtTextBoxFrom.MaxLength,
                                                   mstrExpressionOne,
                                                   mblnCheckAcOne)

            If strErrNo <> String.Empty Then
                'エラー

                If strErrNo = "4001" Then
                    psSet_ErrorNo(strErrNo, lblName.Text, "正しい形式")
                Else
                    psSet_ErrorNo(strErrNo, lblName.Text, strMaxLength)
                End If
                args.IsValid = cuvTextBox.IsValid
                Exit Sub
            End If

            'TextBox2Fromチェック
            strErrNo = ClsCMCommon.pfCheck_TxtErr(txtTextBoxTwoFrom.Text,
                                                   False,
                                                   mblnNumFTwo,
                                                   mblnCheckHanTwo,
                                                   mblnCheckLengthTwo,
                                                   txtTextBoxTwoFrom.MaxLength,
                                                   mstrExpressionTwo,
                                                   mblnCheckAcTwo)

            If strErrNo <> String.Empty Then
                'エラー
                If strErrNo = "4001" Then
                    psSet_ErrorNo(strErrNo, lblName.Text, "正しい形式")
                Else
                    psSet_ErrorNo(strErrNo, lblName.Text, strMaxLength)
                End If
                args.IsValid = cuvTextBox.IsValid
                Exit Sub
            End If

            'Toチェック
            'TextBox1Toチェック
            strErrNo = ClsCMCommon.pfCheck_TxtErr(txtTextBoxTo.Text,
                                                   False,
                                                   mblnNumFOne,
                                                   mblnCheckHanOne,
                                                   mblnCheckLengthOne,
                                                   txtTextBoxTo.MaxLength,
                                                   mstrExpressionOne,
                                                   mblnCheckAcOne)

            If strErrNo <> String.Empty Then
                'エラー
                If strErrNo = "4001" Then
                    psSet_ErrorNo(strErrNo, lblName.Text, "正しい形式")
                Else
                    psSet_ErrorNo(strErrNo, lblName.Text, strMaxLength)
                End If
                args.IsValid = cuvTextBox.IsValid
                Exit Sub
            End If

            'TextBox2Toチェック
            strErrNo = ClsCMCommon.pfCheck_TxtErr(txtTextBoxTwoTo.Text,
                                                   False,
                                                   mblnNumFTwo,
                                                   mblnCheckHanTwo,
                                                   mblnCheckLengthTwo,
                                                   txtTextBoxTwoTo.MaxLength,
                                                   mstrExpressionTwo,
                                                   mblnCheckAcTwo)

            If strErrNo <> String.Empty Then
                'エラー
                If strErrNo = "4001" Then
                    psSet_ErrorNo(strErrNo, lblName.Text, "正しい形式")
                Else
                    psSet_ErrorNo(strErrNo, lblName.Text, strMaxLength)
                End If
                args.IsValid = cuvTextBox.IsValid
                Exit Sub
            End If

            'FromToチェック
            strErrNo = ClsCMCommon.pfCheck_TxtFTErr(txtTextBoxFrom.Text & txtTextBoxTwoFrom.Text,
                                                     txtTextBoxTo.Text & txtTextBoxTwoTo.Text,
                                                     mblnReqFieldVal)

            If strErrNo <> String.Empty Then
                'エラー
                psSet_ErrorNo(strErrNo, lblName.Text, strMaxLength)
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
