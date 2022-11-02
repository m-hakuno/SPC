<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDMA7.aspx.vb" Inherits="SPC.COMUPDMA7" %>

<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            set_onloadscroll();
        }

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <%-- 検索エリア／登録エリアは有りません --%>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div style="margin-left:6px;margin-bottom: 6px;text-align:left">
        <asp:Button ID="btnReload" runat="server" Text="リロード" />
    </div>
    <div id="DivOut" runat="server" class="grid-out" style="width: 1266px; height: 500px;">
        <div class="grid-in" style="width: 1266px; height: 500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
