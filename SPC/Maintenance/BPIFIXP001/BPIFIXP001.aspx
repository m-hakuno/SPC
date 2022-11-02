<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="BPIFIXP001.aspx.vb" Inherits="SPC.BPIFIXP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width: 100%; height: 80px;" border="0">
        <tr>
            <td style="width: 100px">
                <asp:Label ID="lblTboxId1" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTboxId2" runat="server" Text="XXXXXXXX"></asp:Label>
            </td>
            <td>
                <div class="float-right">
                    <asp:Label ID="lblDifferenceY" runat="server" Text="差異有　："></asp:Label>
                </div>
            </td>
            <td style="width: 80px">
                <div class="float-right">
                    <asp:Label ID="lblCountY" runat="server" Text="XXXXX"></asp:Label>
                </div>
            </td>
            <td style="width: 13px">
                <asp:Label ID="lblCountUnitY" runat="server" Text="件"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblHallNm1" runat="server" Text="ホール名"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblHallNm2" runat="server" Text="ＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸ"></asp:Label>
            </td>
            <td>
                <div class="float-right">
                    <asp:Label ID="lblDifferenceN" runat="server" Text="差異無　："></asp:Label>
                </div>
            <td>
                <div class="float-right">
                    <asp:Label ID="lblCountN" runat="server" Text="XXXXX"></asp:Label>
                </div>
            </td>
            <td>
                <asp:Label ID="lblCountUnitN" runat="server" Text="件"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblVer1" runat="server" Text="ＶＥＲ"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblVer2" runat="server" Text="XX.XX"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in" style="height:415px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
