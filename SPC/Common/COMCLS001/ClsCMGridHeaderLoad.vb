'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　GridViewヘッダ読込クラス
'*　ＰＧＭＩＤ：　ClsCMGridHeaderLoad
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.28　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
#Region "インポート定義"
#End Region

Public Class ClsCMGridHeaderLoad

'============================================================================================================================
'=　継承定義
'============================================================================================================================
#Region "継承定義"
#End Region

'============================================================================================================================
'=　定数定義
'============================================================================================================================
#Region "定数定義"
#End Region

'============================================================================================================================
'=　構造体・列挙体定義
'============================================================================================================================
#Region "構造体・列挙体定義"
#End Region

'============================================================================================================================
'=　変数定義
'============================================================================================================================
#Region "変数定義"
    Private mstrXmlNm As String
#End Region

'============================================================================================================================
'=　プロパティ定義
'============================================================================================================================
#Region "プロパティ定義"
#End Region

'============================================================================================================================
'=　イベントプロシージャ定義
'============================================================================================================================
#Region "イベントプロシージャ"
    Sub New(ByVal ipstrXMLNm As String)
        mstrXmlNm = ipstrXMLNm
    End Sub

    Public Sub psGrid_RowCreated(sender As Object, e As GridViewRowEventArgs)
        Dim dtsXml As DataSet
        Dim intCol As Integer
        Dim dblWidth As Double

        If e.Row.RowType = DataControlRowType.Header Then           'ヘッダ
            '既存ヘッダ非表示
            e.Row.Visible = False

            dtsXml = New DataSet()
            e.Row.TableSection = System.Web.UI.WebControls.TableRowSection.TableHeader

            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/" & mstrXmlNm & ".xml"))

            For Each dttTable As DataTable In dtsXml.Tables
                Dim rowHeader As New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)
                intCol = 0

                '追加するヘッダ行作成
                For Each dtrColumns As DataRow In dttTable.Rows
                    If dtrColumns.Item("null").ToString = "0" Then

                        Dim celHeader As New TableHeaderCell()
                        celHeader.RowSpan = dtrColumns.Item("RowSpan").ToString
                        celHeader.ColumnSpan = Integer.Parse(dtrColumns.Item("ColSpan").ToString)
                        celHeader.Text = dtrColumns.Item("Text").ToString
                        dblWidth = 0
                        '既存列から列幅取得
                        For zz As Integer = 0 To celHeader.ColumnSpan - 1
                            dblWidth += Double.Parse((sender.Columns(intCol + zz).ItemStyle.Width.ToString).Replace("px", ""))
                            If zz > 0 Then
                                dblWidth += 3
                            End If
                        Next

                        '作成したヘッダ列追加
                        celHeader.Width = Unit.Parse(dblWidth.ToString)
                        rowHeader.Cells.Add(celHeader)

                    End If
                    intCol += 1
                Next

                '作成したヘッダ行追加
                sender.Controls(0).Controls.AddAt(-1, rowHeader)
                rowHeader.TableSection = System.Web.UI.WebControls.TableRowSection.TableHeader
            Next
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then    'データ行
            e.Row.TableSection = System.Web.UI.WebControls.TableRowSection.TableBody
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then     'フッター
            e.Row.TableSection = System.Web.UI.WebControls.TableRowSection.TableFooter
        End If

    End Sub
#End Region

'============================================================================================================================
'=　プロシージャ定義
'============================================================================================================================
#Region "そのほかのプロシージャ"
#End Region

End Class
