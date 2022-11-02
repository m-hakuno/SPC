<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP009.aspx.vb" Inherits="SPC.COMMENP009" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
<asp:Panel ID="Panel1" runat="server">
        <table style="width: 600px; margin-left: auto; margin-right: auto; border: none;">
            <tr>
                <td colspan="4" class="title">
                    <asp:Label ID="lblTitle1" runat="server" Text="マスタ" Font-Bold="True"></asp:Label>
                    <br />
                </td>
            </tr>

            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu1" runat="server">ユーザーマスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu2" runat="server">社員マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu3" runat="server">他社マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu35" runat="server">業者基本マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu36" runat="server">業者マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu13" runat="server">ＴＢＯＸタイプマスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu14" runat="server">ＴＢＯＸバージョンマスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu21" runat="server">機器分類マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu29" runat="server">機器種別マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu47" runat="server">電話番号マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu16" runat="server">電話番号別文言マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu19" runat="server">出精値引マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu45" runat="server">休日マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="title">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="工事" Font-Bold="True"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu26" runat="server">工事区分マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu27" runat="server">工事名マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu38" runat="server">工事機器マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu15" runat="server">型式マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu23" runat="server">MDN機種マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu24" runat="server">梱包材マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu22" runat="server">移動理由マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu46" runat="server">シリアル特殊条件マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="title">
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="監視" Font-Bold="True"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu20" runat="server">ヘルスチェックエラーマスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu28" runat="server">表示項目マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu39" runat="server">未集信マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenuA7" runat="server">DLL設定変更依頼内容マスタ</asp:LinkButton>
                    <br />
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="4" class="title">
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="保守" Font-Bold="True"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu6" runat="server">保守料金マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu37" runat="server">保守担当者マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu7" runat="server">特殊店舗マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                
                <td>
                    <asp:LinkButton ID="lkbMenu25" runat="server">持参物品セットマスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu17" runat="server">持参物品マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu18" runat="server">持参物品選択マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                
                <td>
                    <asp:LinkButton ID="lkbMenu12" runat="server">倉庫マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu31" runat="server">回復内容マスタ</asp:LinkButton>
                    <br /> 
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu33" runat="server">作業内容マスタ</asp:LinkButton>
                    <br />
                    
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                
                <td>
                    <asp:LinkButton ID="lkbMenu34" runat="server">事象マスタ</asp:LinkButton>
                    <br />
                    
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu40" runat="server">申告元マスタ</asp:LinkButton>
                    <br />
                    
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu41" runat="server">申告内容マスタ</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu42" runat="server">処置内容マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenu43" runat="server">特別保守料金マスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <br />
                </td>
            </tr>
            <tr class="menu-link">
                <td class="menu-indent">&nbsp;</td>
                <td>
                    <asp:LinkButton ID="lkbMenu44" runat="server">修理・有償部品費用マスタ</asp:LinkButton>
                     <br />
                </td>
                <td>
                    <asp:LinkButton ID="lkbMenuB1" runat="server" visible ="true">保守機ＴＢＯＸマスタ</asp:LinkButton>
                    <br />
                </td>
                <td>
                    <br />
                </td>
            </tr>
        </table>
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="lkbMenu30" runat="server" Visible="false">工事区分別使用機器マスタ</asp:LinkButton>
                    <br />
                    <br />
            </td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>
