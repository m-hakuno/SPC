<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM87.aspx.vb" Inherits="SPC.COMUPDM87" %>

<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <table style="width: 510px; margin-left: auto; margin-right: auto; border: none; height: auto;">
        <tr style="float: left;">
            <td>
                <asp:Label ID="Label3" runat="server" Text="機種コード" Width="80px" style="margin-left: 4px"></asp:Label>
                <asp:TextBox ID="txtAppaCd" runat="server" Width="30px" CssClass="IMEdisabled" MaxLength="2" AutoPostBack="True"></asp:TextBox>
                <asp:CustomValidator ID="CstAppaCd" runat="server" ControlToValidate="txtAppaCd" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="key" SetFocusOnError="True" ValidateRequestMode="Enabled"></asp:CustomValidator>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtAppaNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="機器名称" ppNameWidth="80" ppWidth="270" ppCheckAc="False" ppRequiredField="True" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 435px;">
        <div class="grid-in" style="width: 435px; height: 400px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
