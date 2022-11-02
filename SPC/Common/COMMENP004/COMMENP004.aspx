<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP004.aspx.vb" Inherits="SPC.COMMENP004" MaintainScrollPositionOnPostback="true" %>
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
                            <asp:Label ID="lblTboxAAndTInquiry" runat="server" Text="ＴＢＯＸ随時照会" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu1" runat="server">ＴＢＯＸ随時照会</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblScheduleSurvey" runat="server" Text="スケジュール調査" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu2" runat="server">時間外ホール</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu3" runat="server">券売機自走ホール</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu4" runat="server">集信エラーホール</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblReport" runat="server" Text="報告書" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu5" runat="server">トラブル処理票一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu6" runat="server">ミニ処理一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu7" runat="server">監視報告書兼依頼票一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu8" runat="server">ＤＳＵ交換報告書一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblIcCardHistorySurvey" runat="server" Text="ＩＣカード履歴調査" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu9" runat="server">ＩＣカード履歴調査一覧</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 100px">&nbsp;</td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblDll" runat="server" Text="ＤＬＬ" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu10" runat="server">ＤＬＬ設定変更一覧</asp:LinkButton>
                        </td>
                    </tr>
                     <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="lblNoLendBallsSettingInf" runat="server" Text="貸玉数設定情報" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu11" runat="server">玉単価設定情報一覧</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbSubMenu12" runat="server">設定情報差異確認</asp:LinkButton>
                            <br />
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
                            <asp:LinkButton ID="lkbSubMenu13" runat="server">監視対象外ホール一覧</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" colspan="2">
                            <asp:Label ID="Label1" runat="server" Text="アップロード" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr class="menu-link">
                        <td class="menu-indent">&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="lkbSubMenu14" runat="server">アップロード</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>