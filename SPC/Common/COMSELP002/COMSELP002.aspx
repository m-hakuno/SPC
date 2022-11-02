<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="COMSELP002.aspx.vb" Inherits="SPC.COMSELP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftTraderCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1"
                    ppName="業者コード" ppNameWidth="100" ppWidth="50" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftCompanyCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="3"
                    ppName="会社コード" ppNameWidth="100" ppWidth="50" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtCompanyNm" runat="server" ppMaxLength="50" ppName="会社名" ppNameWidth="100"
                    ppWidth="670" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftIntgrtCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="3"
                    ppName="統括コード" ppNameWidth="100" ppWidth="50" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftOfficeCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5"
                    ppName="営業所コード" ppNameWidth="100" ppWidth="50" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtOfficeNm" runat="server" ppMaxLength="20" ppName="営業所名" ppNameWidth="100"
                    ppWidth="670" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftStateCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2"
                    ppName="県コード" ppNameWidth="100" ppWidth="50" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtAddress" runat="server" ppMaxLength="100" ppName="住所" ppNameWidth="100"
                    ppWidth="670" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
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
