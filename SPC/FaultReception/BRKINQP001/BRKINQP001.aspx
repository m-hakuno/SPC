<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BRKINQP001.aspx.vb" Inherits="SPC.BRKINQP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br>
    <table style="width:90%; height:100px" class="center">
        <tr>
            <td colspan="2" style="text-align:right">
                <asp:Label ID="Label1" runat="server" Text="該当件数："></asp:Label>
            </td>
            <td style="width:90px; text-align:right;">
                <asp:Label ID="lblCountNum" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:90px;">
                <asp:Label ID="Label2" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTboxId" runat="server" Text="" Width="70px"></asp:Label>
                <asp:Label ID="lblNlKbn" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:90px;">
                <asp:Label ID="Label3" runat="server" Text="ホール名"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblHallName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:90px;">
                <asp:Label ID="Label4" runat="server" Text="運用区分"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblUnyoKbn" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <br>
    <hr>
    <br>

    <div class="grid-out">
        <div class="grid-in" style="height:auto; max-height:550px;">
            <input id="Hidden2" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvInqList" runat="server"></asp:GridView>
        </div>
    </div>

</asp:Content>

