<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM77.aspx.vb" Inherits="SPC.COMUPDM77" %>

<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

    </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 500px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td>
                <asp:Label ID="lblSys" runat="server" Width="67" Text="システム" style="margin-left: 3px;" />
                <asp:DropDownList ID="ddlSys" runat="server" Width="100" ValidationGroup="key" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlDvs" runat="server" ppName="機器分類" ppNameWidth="70" ppWidth="100" ppClassCD="0137" ppNotSelect="true" ppRequiredField="true" ppValidationGroup="key" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtName" runat="server" ppName="文言" ppNameWidth="70" ppWidth="310" ppMaxLength="20" ppIMEMode="全角" ppTextAlign="左" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 626px; height: 500px;">
        <div class="grid-in" style="width: 626px; height: 500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
