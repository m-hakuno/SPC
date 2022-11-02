<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="COMSELP004.aspx.vb" Inherits="SPC.COMSELP004" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width: 70%;" class="center" border="0">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftAppaCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8"
                    ppName="機器コード" ppNameWidth="100" ppWidth="56" ppCheckHan="True" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtVersion" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="10"
                    ppName="バージョン" ppNameWidth="100" ppWidth="70" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftModelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="20"
                    ppName="型番" ppNameWidth="100" ppWidth="140" ppCheckHan="True" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtAppaNm" runat="server" ppMaxLength="20" ppName="機器名" ppIMEMode="全角"
                    ppNameWidth="100" ppWidth="280" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div style="padding-left: 40px;">
        <div id="DivOut" runat="server" class="grid-out" style="height: 480px; width: 1150px;">
            <div id="DivIn" runat="server" class="grid-in" style="height: 480px; width: 1150px;">
                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvList" runat="server">
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
