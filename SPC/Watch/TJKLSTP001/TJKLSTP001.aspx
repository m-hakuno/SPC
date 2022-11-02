<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeBehind="TJKLSTP001.aspx.vb" Inherits="SPC.TJKLSTP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="margin-left :auto;margin-right :auto;width:1150px;">
        <tr>
            <td style="width:250px;">
                <uc:ClsCMDropDownList runat="server" ID="ddlSElProc" ppName ="実行処理選択" ppNameWidth ="90px" ppNameVisible ="true" ppClassCD="0000" />
            </td>
            <td style="width:100px;">
                <asp:Button ID="btnProcSel" runat="server" Text="選択" />
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSelProc" runat="server" Text="再実行判定"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblJudgeRes" runat="server" Text="○" Font-Size="XX-Large" BackColor="White"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblErrDetail" runat="server" Text="処理状態、発生エラー等" BackColor="Yellow"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td></td>
        </tr>
    </table> 
    <table border="1" style="margin-left :auto;margin-right :auto;width:1150px;">
        <tr>
            <td style="background-color:#ccffff;text-align:center;vertical-align:middle;">
                決済センタ<br />ＴＢＯＸマスタ<br />取得
            </td>
            <td style="background-color:#ffffcc;text-align:center;vertical-align:middle;">
                <asp:Button ID="btnKSMST" runat="server" Text="任意実行" height="60"/>
            </td>
            <td>
                <div id="Div1" runat="server" class="grid-out" style="height:276px;">
                    <div class="grid-in" style="height:276px;">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList1" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
            <td style="background-color:#ccffff;text-align:center;vertical-align:middle;">
                集信エラー
            </td>
            <td style="background-color:#ffffcc;text-align:center;vertical-align:middle;">
                <asp:Button ID="btnERLST" runat="server" Text="任意実行" height="60"/>
            </td>
            <td>
                <div id="Div2" runat="server" class="grid-out" style="height:276px;">
                    <div class="grid-in" style="height:276px;">
                        <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList2" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="background-color:#ccffff;text-align:center;vertical-align:middle;">
                券売機　自走
            </td>
            <td style="background-color:#ffffcc;text-align:center;vertical-align:middle;">
                <asp:Button ID="btnAUTRN" runat="server" Text="任意実行" height="60"/>
            </td>
            <td>
                <div id="Div3" runat="server" class="grid-out" style="height:276px;">
                    <div class="grid-in" style="height:276px;">
                        <input id="Hidden2" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList3" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
            <td style="background-color:#ccffff;text-align:center;vertical-align:middle;">
                時間外消費
            </td>
            <td style="background-color:#ffffcc;text-align:center;vertical-align:middle;">
                <asp:Button ID="btnOVELT" runat="server" Text="任意実行" height="60" />
            </td>
            <td>
                <div id="Div4" runat="server" class="grid-out" style="height:276px;">
                    <div class="grid-in" style="height:276px;">
                        <input id="Hidden3" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList4" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <table style="margin-left :auto;margin-right :auto;width:1150px;">
        <tr style="height:60px;">
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td style="text-align:right;vertical-align:bottom;border-right-style:none;">
            </td>
            <td style="text-align:right;vertical-align:bottom;border-right-style:none;">
                <asp:Button ID="btnRELOD" runat="server" Text="リロード" height="40px" />
            </td>
        </tr>
    </table>
</asp:Content>
