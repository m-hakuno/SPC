<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMLSTP099.aspx.vb" Inherits="SPC.COMLSTP099" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px;" class="center">
        <tr>
            <td class="text-center">
                <asp:Label ID="lblSummary" runat="server" Text="一覧" Visible="False"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="border: 1px solid #0000FF; position: relative; padding-top: 15px; overflow-x: hidden; width: 965px;" class="center">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
