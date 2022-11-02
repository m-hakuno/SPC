'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　GridViewテンプレートカラム(ラベル)クラス
'*　ＰＧＭＩＤ：　ClsCMTextColumn
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2017/04/19：伯野
'********************************************************************************************************************************

Public Class ClsCMLabelColumn
'================================================================================================================================
'=　インポート定義
'================================================================================================================================

    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Implements System.Web.UI.ITemplate


    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================

    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================

    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    Dim mstrID As String

    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    Public Property ppDataField As String
    Public Property ppWidth As Unit = 0
    Public Property ppFontSize As FontUnit = 0
    Public Property ppHeight As Unit = 0
    Public Property ppReadOnly As Boolean = False
    Public Property ppHorizontalAlign As String = "left"
    Public Property ppType As Type = Type.GetType("System.String")
    Public Property ppFormat As String = String.Empty
    Public Property ppAutoSize As Boolean = False
    '============================================================================================================================
    '=　プロシージャ定義
    '============================================================================================================================

    Sub New(ByVal ipstrID As String)
        mstrID = ipstrID
    End Sub

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn

        Dim txtTextBox As New Label

        AddHandler txtTextBox.DataBinding, AddressOf OnDataBinding
        txtTextBox.Width = ppWidth

        '高さの変更
        If ppHeight <> 0 Then
'            txtTextBox.TextMode = TextBoxMode.MultiLine
            txtTextBox.Height = ppHeight
'            txtTextBox.Wrap = False
'            txtTextBox.Attributes.Add("style", "OVERFLOW:hidden;WORD-BREAK:BREAK-ALL;")
            txtTextBox.Attributes.Add("style", "WORD-BREAK:BREAK-ALL;")
        End If
        If ppFontSize <> 0 Then
            txtTextBox.Font.Size = ppFontSize
        End If

'        txtTextBox.ReadOnly = ppReadOnly
        txtTextBox.Style.Item(HtmlTextWriterStyle.TextAlign) = ppHorizontalAlign
        txtTextBox.ID = mstrID
        container.Controls.Add(txtTextBox)
    End Sub

    Public Sub OnDataBinding(ByVal sender As Object, ByVal e As EventArgs)
'        Dim txtTextBox As TextBox = CType(sender, TextBox)
        Dim txtTextBox As Label = CType(sender, Label)

        Dim container As GridViewRow = CType(txtTextBox.NamingContainer, GridViewRow)
        If ppType = Type.GetType("System.Decimal") Then
            If container.DataItem(ppDataField).GetType = Type.GetType("System.Decimal") Then
                txtTextBox.Text = Decimal.Parse(container.DataItem(ppDataField)).ToString(ppFormat)
            Else
                txtTextBox.Text = container.DataItem(ppDataField).ToString()
            End If
        Else
            txtTextBox.Text = container.DataItem(ppDataField).ToString()
        End If

        '改行処理
        If ppHeight <> 0 Then
            Dim intMaxLength As Integer = Math.Floor(Decimal.Parse(Me.ppWidth.ToString.Replace("px", "")) / ((Decimal.Parse(txtTextBox.Font.Size.ToString.Replace("pt", "")) / 3) * 2)) - 2
            Dim intTextLength As Integer = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(txtTextBox.Text)
            If intTextLength > intMaxLength Then
                Dim intLength As Integer = 0
                Dim intByte As Integer = 0
                Dim intNByte As Integer = 0
                Dim strText As String = ""

                While 1
                    If txtTextBox.Text.Length <= (intLength) Then
                        txtTextBox.Text = strText
                        Exit While
                    Else
                        strText &= txtTextBox.Text.Substring(intLength, 1)
                        intByte += System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(txtTextBox.Text.Substring(intLength, 1))
                        If strText.Length < txtTextBox.Text.Length - 2 Then
                            If txtTextBox.Text.Substring(intLength, 2) = Environment.NewLine Then
                                intByte = -1
                            End If
                        End If
                        If txtTextBox.Text.Length > (intLength + 1) Then
                            intNByte = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(txtTextBox.Text.Substring(intLength + 1, 1))
                        End If
                        If intByte >= intMaxLength - intNByte + 1 Then
                            If strText.Length <= txtTextBox.Text.Length - 3 Then
                                If txtTextBox.Text.Substring(intLength + 1, 2) <> Environment.NewLine Then
'                                    strText &= Environment.NewLine
                                    strText &= "<br />"
                                End If
                            ElseIf strText.Length > txtTextBox.Text.Length - 3 Then
'                                strText &= Environment.NewLine
                                strText &= "<br />"
                            End If
                            intByte = 0
                        End If
                    End If

                    intLength += 1
                End While
            End If
        End If
    End Sub

End Class
