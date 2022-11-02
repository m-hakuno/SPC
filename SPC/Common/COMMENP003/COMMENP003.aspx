<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP003.aspx.vb" Inherits="SPC.COMMENP003" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table border="0" class="center">
        <tr class="align-top">
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblCompleteRegistration" runat="server" Text="完了登録" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu1" runat="server">保守対応依頼書一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu20" runat="server">特別保守費用作成</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu2" runat="server">アップロード</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblSerialManagement" runat="server" Text="シリアル管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu3" runat="server">シリアル登録</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu4" runat="server">シリアル情報一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu5" runat="server">保守予備機棚卸し表</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu6" runat="server">持参物品一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblThingArrangement" runat="server" Text="物品手配" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu7" runat="server">配送機器一覧表</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu8" runat="server">物品転送依頼一覧</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 100px">&nbsp;</td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblQualityConference" runat="server" Text="品質会議" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu9" runat="server">品質会議資料</asp:LinkButton>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu19" runat="server">品質会議資料明細</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblRepairUpgrading" runat="server" Text="修理/整備" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu10" runat="server">進捗一覧（修理・整備）</asp:LinkButton>

                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblWork" runat="server" Text="実作業" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu11" runat="server">ＢＢ1調査依頼一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu12" runat="server">使用中カードＤＢ吸上一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblMasterManagement" runat="server" Text="マスタ管理" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu13" runat="server">業者情報</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu14" runat="server">社員情報</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu15" runat="server">機器参照</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu16" runat="server">申告・回答内容</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu17" runat="server">物品マスタ</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblDownLoad" runat="server" Text="ダウンロード" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu18" runat="server">保守完了報告書</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbsubMenu21" runat="server">設置環境写真一覧</asp:LinkButton>
                        </td>
                                            </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
