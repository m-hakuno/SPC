<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM75.aspx.vb" Inherits="SPC.COMUPDM75" %>

<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <table style="width: 380px; margin-left: auto; margin-right: auto; border: none; height: auto;">
        <tr style="float: left;">
            <td>
                <div style="margin-top:8px">
                    <uc:ClsCMDateBox runat="server" ID="dtbStartDt" ppName="開始年月" ppDateFormat="年月" ppNameWidth="80" ppNameVisible="True" ppRequiredField="True" ppValidationGroup="key" />
                </div>
            </td>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtPer" ppName="値引率" ppNameWidth="80" ppMaxLength="5" ppCheckHan="True" ppNameVisible="True" ppTextAlign="右" ppRequiredField="True" ppValidationGroup="val" ppWidth="80" ppIMEMode="半角_変更不可" ppExpression="\d[.]\d\d\d" />
            </td>
            <%--<td>
                <asp:Label ID="lblPer" runat="server" Text="%" />
            </td>--%>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 240px; height: 500px;">
        <div class="grid-in" style="width: 240px; height: 500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
