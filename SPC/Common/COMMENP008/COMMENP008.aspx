<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP008.aspx.vb" Inherits="SPC.COMMENP008" MaintainScrollPositionOnPostback="true" %>
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
                            <asp:Label ID="lblConstruction" runat="server" Text="工事" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu1" runat="server">情報機器工事検収書</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu14" runat="server">工事料金明細書一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblMaintenance" runat="server" Text="保守" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu2" runat="server">情報機器保守検収書</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu15" runat="server">特別保守費用作成</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu3" runat="server">修理・有償部品費用</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu4" runat="server">対メーカ検収書確認リスト</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu5" runat="server">保守料金明細作成</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblSupportCenter" runat="server" Text="サポートセンタ" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu6" runat="server">サポートセンタ運用検収書</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblEnquipment" runat="server" Text="整備" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu7" runat="server">情報機器整備検収書</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblBill" runat="server" Text="請求書" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu8" runat="server">請求書</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="visibility:hidden">
        <tr>
            <td style="width: 100px">&nbsp;</td>
            <td style="visibility:hidden">
                <table style="width: 100%;" border="0">
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblMasterManagement" runat="server" Text="マスタ管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu9" runat="server">修理・有償部品費用マスタ</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu10" runat="server">工事費　登録</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu11" runat="server">保守費　登録</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu12" runat="server">サポートセンタ費　登録</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu13" runat="server">部品マスタ</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
