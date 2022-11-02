<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP001.aspx.vb" Inherits="SPC.COMMENP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:MultiView ID="muvList" runat="server">
        <asp:View ID="vieSPC" runat="server">
            <table class="center">
                <tr>
                    <td>
                        <div class="menu-link">
                            <asp:LinkButton ID="lkbMenu1" runat="server">工事管理</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu2" runat="server">保守管理</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu3" runat="server">監視業務</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu4" runat="server">進捗管理</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu5" runat="server">ヘルスチェック</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu6" runat="server">検収/請求</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu7" runat="server">ホール参照</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu8" runat="server">マスタ管理</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu9" runat="server">定時実行　確認</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu10" runat="server">CRS(連絡受付システム)</asp:LinkButton>
                            <br />
                            <asp:LinkButton ID="lkbMenu11" runat="server" Visible ="False">周知事項</asp:LinkButton>
                        </div>                
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="visNGC" runat="server">
            <table style="width: 600px;" class="center">
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" class="title">
                                    <asp:Label ID="lblTitle1" runat="server" Text="物品関連" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="menu-link">
                                <td class="menu-indent">&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lkbNGCMenu1" runat="server">物品転送依頼書</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu2" runat="server">返却品一覧</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="title">
                                    <asp:Label ID="lblTitle2" runat="server" Text="進捗管理" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="menu-link">
                                <td class="menu-indent">&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lkbNGCMenu3" runat="server">工事進捗一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu4" runat="server">保守対応依頼書一覧（保守対応進捗一覧）</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu5" runat="server">工事連絡票一覧</asp:LinkButton>
                                    &nbsp;<asp:Label ID="lblNGCMenu5" runat="server"></asp:Label>
                                    <asp:Label ID="lblNGCMenu5Unit" runat="server" Text="件"></asp:Label>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu6" runat="server">随時集信一覧状況更新</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu20" runat="server">設置環境写真一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu21" runat="server">設置環境アップロード</asp:LinkButton>
                                </td>
                            </tr>
                
                            <tr>
                                <td colspan="2" class="title">
                                    <asp:Label ID="lblTitle3" runat="server" Text="料金関連" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="menu-link">
                                <td class="menu-indent">&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lkbNGCMenu7" runat="server">工事料金明細</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu8" runat="server">工事料金明細一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu9" runat="server">特別保守費用照会（特別保守依頼承認）</asp:LinkButton>
                                    &nbsp;<asp:Label ID="lblNGCMenu9" runat="server"></asp:Label>
                                    <asp:Label ID="lblNGCMenu9Unit" runat="server" Text="件"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="width: 100%;">
                            <tr>
                                <td colspan="2" class="title">
                                    <asp:Label ID="lblTitle4" runat="server" Text="シリアル関連" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="menu-link">
                                <td class="menu-indent">&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lkbNGCMenu10" runat="server">シリアル情報一覧</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="title">
                                    <asp:Label ID="lblTitle5" runat="server" Text="調査・監視業務" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="menu-link">
                                <td class="menu-indent">&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lkbNGCMenu11" runat="server">ＢＢ１調査依頼一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu12" runat="server">監視対象外ホール一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu13" runat="server">品質会議資料</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu19" runat="server">品質会議資料明細</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu14" runat="server">券売入金機自走調査一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu15" runat="server">監視報告書兼依頼票一覧</asp:LinkButton>
                                    &nbsp;<asp:Label ID="lblNGCMenu15" runat="server"></asp:Label>
                                    <asp:Label ID="lblNGCMenu15Unit" runat="server" Text="件"></asp:Label>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu16" runat="server">玉単価設定情報一覧</asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lkbNGCMenu17" runat="server">DSU交換対応依頼書一覧</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="title">
                                    <asp:Label ID="lblTitle6" runat="server" Text="修理・整備業務" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr class="menu-link">
                                <td class="menu-indent">&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lkbNGCMenu18" runat="server">修理整備進捗一覧</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
    <br />
</asp:Content>
