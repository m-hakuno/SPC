<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM47.aspx.vb" Inherits="SPC.COMUPDM47" %>

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
                <uc:ClsCMTextBox ID="txtProdCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="11" ppName="梱包材コード" ppNameWidth="80" ppWidth="100" ppCheckAc="False" ppRequiredField="True" ppValidationGroup="val" ppCheckLength="True" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtProdNm" runat="server" ppIMEMode="全角" ppMaxLength="30" ppName="梱包材名称" ppNameWidth="80" ppWidth="480" ppCheckAc="False" ppRequiredField="True" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 660px; height: 240px;">
        <div class="grid-in" style="width: 660px; height: 240px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
