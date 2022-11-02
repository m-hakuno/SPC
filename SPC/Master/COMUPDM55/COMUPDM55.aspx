<%@ Page Title="" Language="VB" MasterPageFile="COMUPDM55_Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM55.aspx.vb" Inherits="SPC.COMUPDM55" %>

<%@ MasterType VirtualPath="COMUPDM55_Mst.Master" %>


<%--検索エリア--%>
<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

        //各グリッド選択(≠行選択)によってスクロール位置を下げる
        function scrolldownreset() {
            window.scroll(0, 0);
        }
        function scrolldown1() {
            window.scroll(0, 200);
        }
        function scrolldown2() {
            window.scroll(0, 300);
        }
        function scrolldownmax() {
            window.scroll(0, screen.height);
        }
    </script>

    <table style="width: 1000px; margin-left: auto; margin-right: auto; border: none; text-align: left; margin-top: 40px">
        <tr>
            <td colspan="3" class="auto-style1">
                <asp:Label ID="Label7" runat="server" Text="モード" Width="90px" Style="margin-left: 5px"></asp:Label>
                <asp:DropDownList ID="ddlInsUpd" runat="server" Width="70px">
                    <asp:ListItem Value="0">新規</asp:ListItem>
                    <asp:ListItem Value="1">更新</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Label ID="Label1" runat="server" Text="システム" Width="90px" Style="margin-left: 5px"></asp:Label>
                <asp:DropDownList ID="ddlSystem" runat="server" Width="100"></asp:DropDownList>
            </td>

            <td class="auto-style1">
                <asp:Label ID="Label2" runat="server" Text="持参分類" Width="90px" Style="margin-left: 5px"></asp:Label>
                <asp:DropDownList ID="ddlMachine" runat="server" Width="160"></asp:DropDownList>
            </td>

            <td class="auto-style1">
                <asp:Label ID="Label3" runat="server" Text="バージョン" Width="90px" Style="margin-left: 5px"></asp:Label>
                <asp:DropDownList ID="ddlVer" runat="server" Width="100"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <%-- 初期パラメータ(新規or更新)を保持 --%>
    <asp:Label ID="lblInsOrUpd" runat="server" Text="新規" Width="0px" Visible="false"></asp:Label>
    <hr />
    <table style="width: 1000px; height: 80px; margin-left: auto; margin-right: auto; text-align: left;">
        <tr>
            <td style="vertical-align: top">
                <asp:Label ID="lblpermitVer" runat="server" Text="許容バージョン" Width="115px" />
            </td>
            <td>
                <asp:CheckBoxList ID="cklver" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="12" Font-Size="Medium" />
            </td>
        </tr>
    </table>
    <hr />
</asp:Content>


<%--グリッド--%>
<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div style="border-top-width: 1000px; border-top: 1px solid; margin-bottom: 80px" />
    <table style="width: 100%">
        <tr>
            <td>
                <div style="float: left; margin-left: 120px; margin-top: 30px">
                    <asp:Label ID="Label4" runat="server" Text="ハード付属品関連：" Width="120px" Style="margin-left: 5px"></asp:Label>
                    <asp:Button ID="btnGrid1" runat="server" Width="20px" Text="" CommandName="HARD" />
                    <asp:Label ID="Label5" runat="server" Text="保守ツール関連：" Width="120px" Style="margin-left: 5px"></asp:Label>
                    <asp:Button ID="btnGrid2" runat="server" Width="20" Text="" CommandName="TOOL" />
                    <asp:Label ID="Label6" runat="server" Text="手順書関連：" Width="120px" Style="margin-left: 5px"></asp:Label>
                    <asp:Button ID="btnGrid3" runat="server" Width="20" Text="" CommandName="MANUAL" />
                </div>
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="float: left; margin-top: 20px; margin-left: 120px">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width:20px">
                                <asp:Label ID="lblMark1" runat="server" Text="●" ForeColor="RoyalBlue" Width="16px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblG1" runat="server" Text="ハード付属品関連"></asp:Label>
                                <asp:Label ID="Label9" runat="server" Text="　　　該当件数："></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <div style="float: right;">
                                    <asp:Label ID="lblcount1" runat="server" Text="XXXXX"></asp:Label>
                                </div>
                            </td>
                            <td style="width: 50px;">
                                <asp:Label ID="Label8" runat="server" Text="件"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>

    <table style="width: 840px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td rowspan="2">
                <div class="grid-out" style="width: 980px; height: 180px;">
                    <div class="grid-in" style="width: 980px; height: 180px">
                        <input id="Hidden0" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList1" runat="server"></asp:GridView>
                    </div>
                </div>
            </td>
            <td style="float: left;">
                <asp:Button ID="btnGrv1Up" runat="server" Width="34px" Text="▲" Height="30px" CommandName="Up" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: bottom">
                <asp:Button ID="btnGrv1Dw" runat="server" Width="34px" Text="▼" Height="30px" CommandName="Dw" />
            </td>
        </tr>
    </table>

    <div style="float: left; margin-top: 20px; margin-left: 120px">
        <table style="width: 100%;">
            <tr>
                <td style="width:20px">
                    <asp:Label ID="lblMark2" runat="server" Text="●" ForeColor="RoyalBlue" Width="16px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblG2" runat="server" Text="保守ツール関連"></asp:Label>
                    <asp:Label ID="Label12" runat="server" Text="　　　　該当件数："></asp:Label>
                </td>
                <td style="width: 100px;">
                    <div style="float: right;">
                        <asp:Label ID="lblcount2" runat="server" Text="XXXXX"></asp:Label>
                    </div>
                </td>
                <td style="width: 50px;">
                    <asp:Label ID="Label10" runat="server" Text="件"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <table style="width: 840px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td rowspan="2">
                <div class="grid-out" style="width: 980px; height: 180px;">
                    <div class="grid-in" style="width: 980px; height: 180px">
                        <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList2" runat="server"></asp:GridView>
                    </div>
                </div>
            </td>
            <td style="float: left">
                <asp:Button ID="btnGrv2Up" runat="server" Width="34px" Text="▲" Height="30px" CommandName="Up" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: bottom">
                <asp:Button ID="btnGrv2Dw" runat="server" Width="34px" Text="▼" Height="30px" CommandName="Dw" />
            </td>
        </tr>
    </table>


    <div style="float: left; margin-top: 20px; margin-left: 120px">
        <table style="width: 100%;">
            <tr>
                <td style="width:20px">
                    <asp:Label ID="lblMark3" runat="server" Text="●" ForeColor="RoyalBlue" Width="16px"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblG3" runat="server" Text="手順書関連"></asp:Label>
                    <asp:Label ID="Label14" runat="server" Text="　　　　　　該当件数："></asp:Label>
                </td>
                <td style="width: 100px;">
                    <div style="float: right;">
                        <asp:Label ID="lblcount3" runat="server" Text="XXXXX"></asp:Label>
                    </div>
                </td>
                <td style="width: 50px;">
                    <asp:Label ID="Label13" runat="server" Text="件"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <table style="width: 840px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td rowspan="2">
                <div class="grid-out" style="width: 980px; height: 180px;">
                    <div class="grid-in" style="width: 980px; height: 180px">
                        <input id="Hidden2" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList3" runat="server"></asp:GridView>
                    </div>
                </div>
            </td>
            <td style="float: left">
                <asp:Button ID="btnGrv3Up" runat="server" Width="34px" Text="▲" Height="30px" CommandName="Up" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: bottom">
                <asp:Button ID="btnGrv3Dw" runat="server" Width="34px" Text="▼" Height="30px" CommandName="Dw" />
            </td>
        </tr>
    </table>


    <%-- 選択行管理 (未選択：value=-1  1行目：value=0　n行目：value=n-1) --%>
    <asp:HiddenField ID="hdnSelectLine" runat="server" Value="-1" />
    <%-- イベント管理(行追加、プレビュー等) --%>
    <asp:HiddenField ID="hdnCmd" runat="server" Value="DEF" />
    <%-- 状態管理(HARD、TOOL、MANUAL等コマンド名が入る) --%>
    <asp:HiddenField ID="hdnMode" runat="server" Value="DEF" />
    <%-- 画面遷移時のKEY項目を保持 --%>
    <asp:HiddenField ID="hdnKeySys" runat="server" Value="" />
    <asp:HiddenField ID="hdnKeyMac" runat="server" Value="" />
    <asp:HiddenField ID="hdnKeyVer" runat="server" Value="" />

</asp:Content>




<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
    .auto-style1
    {
        height: 30px;
    }
</style>
</asp:Content>





