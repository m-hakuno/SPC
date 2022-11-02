<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CTISELP005.aspx.vb" Inherits="SPC.CTISELP005" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br />
    <asp:Panel ID="pnlHallInf" runat="server">
        <table class="center">
            <tr>
                <td style="width:50px;" ></td>
                <td colspan="2">
                    <table style="width:200px;" >
                        <tr class="float-left">
                            <td>
                                <asp:Label ID="lblTell" runat="server" Text="Ｔ　Ｅ　Ｌ" Width="80"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTell_input" runat="server" Text="123-1234-1234"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
               <td style="width:50px;"></td>
               <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblEmp" runat="server" Text="社　員　名"  Width="80"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEmp_input" runat="server" Text="XXX : ああああああああああああああああ"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSisya" runat="server" Text="支　社　名"  Width="90"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSisya_input" runat="server" Text="XXX : ああああああああああああああああ"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width:50px;" ></td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblEigyo" runat="server" Text="営　業　所"  Width="80"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEigyo_input" runat="server" Text="XXX : ああああああああああああああああ"></asp:Label>
                            </td>                           
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTokatu" runat="server" Text="統括営業所名"  Width="90"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTokatu_input" runat="server" Text="XXX : ああああああああああああああああ"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
    <br /><br />
        <div class="grid-out center" style="width: 98%;">
            <div class="grid-in">
                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvList" runat="server">
                </asp:GridView>
            </div>
        </div>
    <div class="float-left">
        <asp:CustomValidator ID="SystemErr1" runat="server" ErrorMessage="画面項目の表示に失敗しました" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
        <asp:CustomValidator ID="SelectErr1" runat="server" ErrorMessage="社員情報の取得に失敗しました" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
        <asp:CustomValidator ID="SelectErr2" runat="server" ErrorMessage="代理店情報の取得に失敗しました" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
        <asp:CustomValidator ID="SelectErr3" runat="server" ErrorMessage="保守担当情報の取得に失敗しました" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
        <asp:CustomValidator ID="SelectErr4" runat="server" ErrorMessage="工事設計依頼書情報の取得に失敗しました" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
        <asp:CustomValidator ID="SelectErr5" runat="server" ErrorMessage="保守設計対応依頼書情報の取得に失敗しました" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
        <asp:CustomValidator ID="SystemErr2" runat="server" ErrorMessage="システムエラー" CssClass="errortext" ValidationGroup="1"></asp:CustomValidator>
    </div>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
    </div>
</asp:Content>
