<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM53.aspx.vb" Inherits="SPC.COMUPDM53" %>

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

    <table style="width: 380px; margin-left: auto; margin-right: auto; border: none; height: auto;">
        <tr style="float: left;">
            <td>
                <asp:Label ID="lblErrRslt" runat="server" Text="エラー種別" Width="80"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlErrRslt" runat="server" Width="220" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtErrNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="エラー名称" ppNameWidth="80" ppWidth="300" ppCheckAc="False" ppRequiredField="True" ppValidationGroup="val" ppCheckLength="False" />
            </td>
<%--            <td>--%>
                <%--<asp:Label ID="lblLmpFlg" runat="server" Text="ランプフラグ" Width="100"></asp:Label>--%>
            <%--</td>--%>
            <td>
<%--                <asp:DropDownList ID="ddlLmpFlg" runat="server" Width="100" AutoPostBack="true" ValidationGroup="val">
                </asp:DropDownList>--%>
                <uc:ClsCMDropDownList ID="ddlLmpFlg" runat="server" ppName="ランプフラグ" ppNameWidth="100" ppWidth="100" ppClassCD="0095" ppNotSelect="true" ppRequiredField="True" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 490px; height: 240px;">
        <div class="grid-in" style="width: 490px; height: 240px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
