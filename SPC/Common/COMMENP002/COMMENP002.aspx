<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP002.aspx.vb" Inherits="SPC.COMMENP002" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

    <table style="width: 600px; padding-left: 120px;" border="0" class="center">
        <tr class="align-top">
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle1" runat="server" Text="工事受付" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu1" runat="server">工事受付一覧</asp:LinkButton>
                            &nbsp;&nbsp;<asp:Label ID="lblCnstCnt" runat="server"></asp:Label>
                            <br />
                            <asp:LinkButton ID="lkbMenu2" runat="server">請求資料　状況更新</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu3" runat="server">作業予定一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu16" runat="server">工事連絡票一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu17" runat="server">構成配信/結果参照</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle2" runat="server" Text="物品手配" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu4" runat="server">物品転送依頼一覧</asp:LinkButton>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle3" runat="server" Text="完了登録" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu7" runat="server">工事料金明細書一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu8" runat="server">シリアル登録</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle4" runat="server" Text="シリアル管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu9" runat="server">シリアル情報一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu10" runat="server">ＤＬＬ変更依頼一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle5" runat="server" Text="随時集信一覧" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu11" runat="server">随時集信一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle6" runat="server" Text="マスタ管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu12" runat="server">業者情報</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu13" runat="server" Enabled="False">社員情報</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu14" runat="server">機器参照</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="title">
                            <asp:Label ID="lblTitle7" runat="server" Text="ダウンロード" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbMenu15" runat="server">工事完了報告書</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu18" runat="server">設置環境写真一覧</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>

</asp:Content>
