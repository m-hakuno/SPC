<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="COMLSTP001.aspx.vb" Inherits="SPC.COMLSTP001" MaintainScrollPositionOnPostback="true" %>
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
                <uc:ClsCMDateBoxFromTo ID="dftGetDt" runat="server" ppName="登録日" ppNameWidth="70" />
            </td>
        </tr>
        <tr>
        <td colspan="3">
                <uc:ClsCMTextBox ID="txtHallNm" runat="server" ppName="ホール名" ppNameWidth="70" ppWidth="370"
                    ppMaxLength="40" ppIMEMode="全角" />
            </td>
       </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out center" style="width:970px;height:385px; margin-top: 13px;">
        <div id="DivIn" runat="server" class="grid-in" style="height:385px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
                <HeaderStyle BorderStyle="None" />
            </asp:GridView>
            </div>
           </div>
        <div style="text-align: right; vertical-align: bottom;">
             <br /><br />
            <asp:Button ID="BtnDelete" runat="server" Text="削除" Font-Size="Medium" />
        </div>
</asp:Content>
