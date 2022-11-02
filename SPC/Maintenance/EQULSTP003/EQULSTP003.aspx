<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="EQULSTP003.aspx.vb" Inherits="SPC.EQULSTP003" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br>
    <br>
    <table style="width:1050px; table-layout: fixed;" class="center" border="0">
        <tr>
            <td style="width: 100px; text-align: center;">
                <asp:Label ID="Label1" runat="server" Text="支　社　名"></asp:Label>
            </td>
            <td style="width: 250px">
                <br>
                <asp:DropDownList ID="ddlOfficeFm" runat="server" Width="250"></asp:DropDownList>
                <br>
                <asp:CustomValidator ID="valOffice" runat="server" CssClass="errortext"></asp:CustomValidator>
            </td>
            <td style="width: 50px; text-align: center;">
                <asp:Label ID="Label3" runat="server" Text="～"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlOfficeTo" runat="server" Width="250"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 100px; text-align: center;">
                <asp:Label ID="Label2" runat="server" Text="保　担　名"></asp:Label>
            </td>
            <td style="width: 250px">
                <br>
                <asp:DropDownList ID="ddlMntFm" runat="server" Width="250"></asp:DropDownList>
                <br>
                <asp:CustomValidator ID="valMnt" runat="server" CssClass="errortext"></asp:CustomValidator>
            </td>
            <td style="width: 50px; text-align: center;">
                <asp:Label ID="Label4" runat="server" Text="～"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMntTo" runat="server" Width="250"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <div class="float-right">
        <asp:Button ID="btnClear" runat="server" Text="検索条件クリア" CausesValidation="False" />
        <asp:Button ID="btnCsv" runat="server" Text="ＣＳＶ" />
    </div>
    <br>
    <br>
    <div style="border-width: 1px; border-bottom-style: solid"></div>
    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="errortext" Width="450" />
</asp:Content>
