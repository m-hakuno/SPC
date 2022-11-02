<%@ Page Title="" Language="VB" MasterPageFile="COMUPDM54_Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM54.aspx.vb" Inherits="SPC.COMUPDM54"%>
<%@ MasterType VirtualPath="COMUPDM54_Mst.Master"%>
<%--検索エリア--%>
<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
                btnDmy.style.display = "none";
                txtBox.focus();
        }
    </script>
    <table style="width:1020px;margin-left:auto;margin-right:auto;border:none;text-align:left;margin-bottom:18px;">
        <tr>
            <td class="auto-style3">
                <asp:Label ID="Label1" runat="server" Text="システム" Width="115px"></asp:Label>
                <asp:DropDownList ID="ddlSystem" runat="server" Width="120"></asp:DropDownList>
            </td>
            <td class="auto-style3">
                <asp:Label ID="Label2" runat="server" Text="持参分類" Width="115px" style="margin-left: 5px"></asp:Label>
                <asp:DropDownList ID="ddlMachine" runat="server" Width="180"></asp:DropDownList>
            </td>
            <td class="auto-style3">
                <asp:Label ID="Label3" runat="server" Text="バージョン" Width="115px"></asp:Label>
                <asp:DropDownList ID="ddlVer" runat="server" Width="120"></asp:DropDownList>
            </td>
        </tr>
    </table>
</asp:Content>
<%--グリッド--%>
<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div style="border-top-width:1000px;border-top:1px solid;margin-bottom:80px" />
          
    <div class="grid-out" style="width:1100px;height:400px;margin-top:60px">
        <div class="grid-in" style="width:1100px;height:400px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
</asp:Content>

