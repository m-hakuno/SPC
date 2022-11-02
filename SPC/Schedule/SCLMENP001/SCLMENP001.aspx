<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SCLMENP001.aspx.vb" Inherits="SPC.SCLMENP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <style type="text/css">
        .auto-style1
        {
            width: 230px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table border="0" class="center">
        <tr class="align-top">
            <td>
                <table border="0" style="width:100%;">
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblConstruction" runat="server" Text="スケジュール管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu1" runat="server">スケジュール一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu2" runat="server">スケジュール明細</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu3" runat="server">スケジュール管理</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
