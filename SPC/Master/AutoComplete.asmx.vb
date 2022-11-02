Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
Imports System.Web.Script.Services

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
<ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
Public Class AutoComplete
    Inherits System.Web.Services.WebService
    Dim clsDataConnect As New ClsCMDataConnect


    <WebMethod()> _
    Public Function GetCompletionDomainList(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim list As New List(Of String)
        ' ＤＢ接続変数
        Dim conDB As New SqlConnection
        Dim cmdDB As New SqlCommand
        Dim query As String = String.Format("SELECT TOP {0} [MSA_DMN_NAM] FROM [SPCDB].[dbo].[SCL_MSA_DMN] WHERE [MSA_DMN_NAM] LIKE @item AND [MSA_DEL_DVS] = '0'", count)

        Try
            'ＤＢ接続
            If Not clsDataConnect.pfOpen_Database(conDB) Then
                Throw New Exception("")
            End If
            Dim cmd As New SqlCommand(query, conDB)
            Dim reader As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            cmd.Parameters.Add("item", SqlDbType.NVarChar)
            cmd.Parameters.Item("item").Value = prefixText & "%"
            reader.Fill(ds)
            ds.AcceptChanges()

            For Each dr As DataRow In ds.Tables(0).Rows
                list.Add(dr.Item(0).ToString)
            Next

            Return list.ToArray()
        Catch ex As Exception
            Return {String.Empty}
        Finally
            'DB切断
            clsDataConnect.pfClose_Database(conDB)
        End Try


    End Function

End Class