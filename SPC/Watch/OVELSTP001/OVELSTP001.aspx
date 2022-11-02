<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="OVELSTP001.aspx.vb" Inherits="SPC.OVELSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table class="center">
        <tr>
            <td class="text-center">
                <asp:Label ID="lblSerchTitle" runat="server" Text="検索条件"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="text-center">
                <uc:ClsCMDateBoxFromTo runat="server" ID="txtTyousaFromTo" ppDateFormat="年月日" ppName="調査日付" />
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out" style="width: 474px; margin-right: auto; margin-left: auto;">
        <div id="DivIn" runat="server" class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
