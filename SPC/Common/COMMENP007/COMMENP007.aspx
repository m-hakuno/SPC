<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP007.aspx.vb" Inherits="SPC.COMMENP007" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td class="menu-link">
                <asp:LinkButton ID="lkbSubMenu1" runat="server">ヘルスチェック一覧</asp:LinkButton>
                <br />
                <asp:LinkButton ID="lkbSubMenu2" runat="server">監視対象外ホール一覧</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
