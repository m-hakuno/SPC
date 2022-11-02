<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="BPILSTP001.aspx.vb" Inherits="SPC.BPILSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="TBOXID" ppNameWidth="70" ppWidth="70" ppCheckHan="True" ppNum="True" ppCheckLength="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftGetDt" runat="server" ppName="取得日" ppNameWidth="70" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out center" style="width:905px;height:435px;">
        <div id="DivIn" runat="server" class="grid-in" style="height:435px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
