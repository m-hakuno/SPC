<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="ICHLSTP001.aspx.vb" Inherits="SPC.ICHLSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 1050px;" class="center">
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="align-top" style="width: 180px">
                    <uc:ClsCMTextBox runat="server" ID="txtTboxID" ppName="ＴＢＯＸＩＤ" ppMaxLength="8" ppCheckHan="true" ppCheckLength="true" ppRequiredField="true" />
                </td>
                <td style="width: 20px">
                <td class="align-top" style="width: 180px">
                    <uc:ClsCMDateBox runat="server" ID="txtDataDT" ppname="データ日付" ppRequiredField="true" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
