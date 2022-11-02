<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM68.aspx.vb" Inherits="SPC.COMUPDM68" %>

<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width: 500px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftSCode" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="コード" ppNameWidth="60" ppWidth="20" ppCheckHan="True" ppNum="true" ppValidationGroup="search" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlDel" runat="server" ppName="削除区分" ppNameWidth="80" ppWidth="96" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtSName" runat="server" ppIMEMode="全角" ppMaxLength="25" ppName="名称" ppNameWidth="60" ppWidth="330" />
            </td>

        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width: 500px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtCode" ppName="コード" ppNameWidth="60"
                    ppIMEMode="半角_変更不可" ppMaxLength="2" ppCheckHan="True"
                    ppWidth="20" ppNum="true" ppValidationGroup="key" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtName" ppName="名称" ppIMEMode="全角" ppNameWidth="60"
                    ppMaxLength="25" ppWidth="330" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 507px; height: 440px;">
        <div class="grid-in" style="width: 507px; height: 440px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
