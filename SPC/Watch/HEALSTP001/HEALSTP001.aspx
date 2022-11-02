<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="HEALSTP001.aspx.vb" Inherits="SPC.HEALSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" >
        <tr>
            <td>
                <uc:ClsCMDateBox runat="server" ID="txtHasseiDateFr" ppDateFormat="年月日" ppName="発生日時" ppNameWidth="100px" ppTabIndex="1" ppValidationGroup="Detail" />
            </td>
            <td>
                <uc:ClsCMTimeBox runat="server" ID="txtHasseiTimeFr" ppName="発生日時" ppNameVisible="false" ppTabIndex="2" ppValidationGroup="Detail" />
            </td>
            <td style="padding-left:15px;padding-right:15px;">
                <label runat="server">&nbsp～&nbsp</label>
            </td>
            <td>
                <uc:ClsCMDateBox runat="server" ID="txtHasseiDateTo" ppDateFormat="年月日" ppName="発生日時" ppNameVisible="false" ppTabIndex="3" ppValidationGroup="Detail" />
            </td>
            <td>
                <uc:ClsCMTimeBox runat="server" ID="txtHasseiTimeTo" ppName="発生日時" ppNameVisible="false" ppTabIndex="4" ppValidationGroup="Detail" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBoxFromTo runat="server" ID="txtTBoxIdFrTo" ppName="ＴＢＯＸＩＤ" ppWidth="60px" ppTextAlign="中央" ppNameWidth="100px" ppTabIndex="5" ppMaxLength="8" ppIMEMode="半角_変更不可" ppNum="true" ppValidationGroup="Detail" />
            </td>
            <td colspan="3" style="padding-left:30px;">
                <span><label style="padding-right:30px;" >システム</label><asp:DropDownList runat="server" ID="ddlSystem" TabIndex="6" /></span>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="padding-top:10px;">
                <span><label style="padding-right:30px;" >ヘルスチェック結果</label><asp:DropDownList runat="server" ID="ddlHealthChkRslt" TabIndex="7" style="width:130px;"/></span>
            </td>
        </tr>
    </table>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummarySearch" runat="server" CssClass="errortext" ValidationGroup="Detail" TabIndex="8" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server" Visible="false" >
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out" style="height: 560px;">
        <div class="grid-in" style="height: 560px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server" TabIndex="26" >
                <EditRowStyle Height="40px" />
                <RowStyle Height="40px" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
