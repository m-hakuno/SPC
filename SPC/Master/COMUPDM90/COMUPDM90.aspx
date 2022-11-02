<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM90.aspx.vb" Inherits="SPC.COMUPDM90" %>
<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:580px;margin-left:auto;margin-right:auto;border:none;">
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtCode" ppName="コード" ppNameWidth="80"
                    ppIMEMode="半角_変更不可" ppMaxLength="2" ppCheckHan="True" ppNum="true"
                    ppWidth="30" ppValidationGroup="key" />
            </td>
            <td>
                <%-- DBのデータ桁数は20文字だが他画面の表示領域の都合上登録可能数は15文字とする --%>
                <uc:ClsCMTextBox runat="server" ID="txtName" ppName="移動理由" ppIMEMode="全角" ppNameWidth="80"
                    ppMaxLength="15" ppWidth="220" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>  

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:348px;height:520px;">
        <div class="grid-in" style="width:348px;height:520px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
