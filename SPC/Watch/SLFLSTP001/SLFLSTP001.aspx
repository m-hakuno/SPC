<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="SLFLSTP001.aspx.vb" Inherits="SPC.SLFLSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table class="center">
        <tr>
            <td class="text-center">
                <asp:Label ID="Label1" runat="server" Text="検索条件"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="text-center">
                <uc:ClsCMDateBoxFromTo runat="server" ID="dftExeDT" ppName="調査日付" />
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
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table class="center" style="width:70%;border:solid 2px;" id="TBOX_Serch" runat="server">
        <tr>
            <td class="text-center" colspan="3">
                <asp:Label ID="TBOX_Serch_Title" runat="server" Text="ＴＢＯＸ検索"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="text-center" style ="width:60px;">
                <uc:ClsCMTextBox runat="server" ID="txtTBOXID" ppName="TBOXID" ppNameVisible="True" ppNameWidth="50" ppTextMode="SingleLine" ppMaxLength="8" ppWidth="60"  ppWrap="False" />
            </td>
            <td>
                <asp:Label ID="lblRBOXNM" runat="server" Text="&nbsp;"></asp:Label>
            </td>
            <td style="text-align:right;width:310px;">
                <uc:ClsCMDateBoxFromTo runat="server" ID="dftTBOXFromTo" ppName="調査日付" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td style="text-align:right;">
                <asp:Button ID="btnTBOXClear" runat="server" Text="検索条件クリア" CausesValidation="False" />　　<asp:Button ID="btnTBOXSerach" runat="server" Text="検索" CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
