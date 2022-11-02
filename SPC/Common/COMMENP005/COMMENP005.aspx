<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP005.aspx.vb" Inherits="SPC.COMMENP005" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width: 100%;">
        <tr class="align-top">
            <td style="width: 43%">&nbsp;</td>
            <td>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblProgressManagement" runat="server" Text="進捗管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu1" runat="server">工事進捗一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu2" runat="server">保守対応依頼書一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu3" runat="server">トラブル処理票一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu4" runat="server">ミニ処理票一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblReport" runat="server" Text="報告書" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu5" runat="server">作業予定一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTboxAAndTInquiry" runat="server" Text="ＴＢＯＸ随時照会" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu6" runat="server">ＴＢＯＸ随時照会</asp:LinkButton>
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblNoLendBallsSettingInf" runat="server" Text="貸玉数設定情報" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu7" runat="server">玉単価設定情報一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu8" runat="server">設定情報差異確認</asp:LinkButton>
                        </td>
                    </tr>
               </table>
            </td>
        </tr>
    </table>
</asp:Content>
