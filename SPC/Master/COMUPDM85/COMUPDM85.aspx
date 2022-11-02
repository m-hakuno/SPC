<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM85.aspx.vb" Inherits="SPC.COMUPDM85" %>

<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 44px;
        }
        .auto-style2 {
            height: 44px;
            width: 85px;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <table style="width: 600px; margin-left: auto; margin-right: auto; border: none; height: auto;">
        <tr style="float: left;">
            <td class="auto-style2">
               <asp:Label ID="Label3" runat="server" Text="コード" Width="50px" style="margin-left: 4px"></asp:Label>
            <td class="auto-style1">
               <asp:TextBox ID="txtCd" runat="server" Width="30px" CssClass="IMEdisabled" MaxLength="2" AutoPostBack="True"></asp:TextBox>
               <asp:CustomValidator ID="CstAppaCd" runat="server" ControlToValidate="txtCd" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="key" SetFocusOnError="True" ValidateRequestMode="Enabled"></asp:CustomValidator>
            </td>
            <td class="auto-style1">
                <uc:ClsCMTextBox ID="txtReason" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="理由" ppNameWidth="80" ppWidth="700" ppCheckAc="False" ppRequiredField="true" ppValidationGroup="val" />
            </td>
<%--             <td>
                <uc:ClsCMTextBox runat="server" ID="txtReason" ppName="理由" ppIMEMode="全角" ppNameWidth="80"
                    ppMaxLength="50" ppWidth="700" ppValidationGroup="val" ppRequiredField="true" />
            </td>--%>
       </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 850px; height: 240px;">
        <div class="grid-in" style="width: 850px; height: 240px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
