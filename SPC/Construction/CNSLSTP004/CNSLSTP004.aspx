<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSLSTP004.aspx.vb" Inherits="SPC.CNSLSTP004" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <td style="width: 30%">&nbsp;</td>
            <td style="width: 70%">
                <table>
                    <tr>
                        <td style="width: 100px; padding-left:5px;">

                            <asp:Label ID="Label4" runat="server" Text="設置区分 " Width="100px"></asp:Label>

                        </td>
                        <td>
                            <asp:CheckBox ID="cbxEstTmp" runat="server" Text="仮設置" Width="90px" />
                            <asp:CheckBox ID="cbxEstSet" runat="server" Text="本設置" Width="90px" />
                        </td>
                        <td style="width: 100px;">

                            <asp:Label ID="lblOutputOrderN" runat="server" Text="出力順" Width="100"></asp:Label>

                        </td>
                        <td>

                            <asp:DropDownList ID="ddlOutputOrder" runat="server">
                                <asp:ListItem Value="1">1:実施日順</asp:ListItem>
                                <asp:ListItem Value="2">2:ＴＢＯＸＩＤ順</asp:ListItem>
                                <asp:ListItem Value="3">3:工事依頼番号順</asp:ListItem>
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left:2px;">
                            <uc:ClsCMDateBoxFromTo ID="dftImplementationDt" runat="server" ppName="実施日" ppNameWidth="120" />
                        </td>
                        <td>

                            <asp:Label ID="lblSituationN" runat="server" Text="状況" Width="120"></asp:Label>

                        </td>
                        <td>

                            <asp:DropDownList ID="ddlSituation" runat="server"></asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="120" ppWidth="100" ppCheckHan="True" ppNum="True" />
                        </td>
                        <td>
                            <asp:Label ID="lblSituationN0" runat="server" Text="システム" Width="120px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSystem" runat="server" Width="140" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc:ClsCMTextBoxFromTo ID="tftConsReqNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="工事依頼番号" ppNameWidth="120" ppWidth="100" ppCheckHan="True" />
                        </td>
                        <td>

                            <asp:Label ID="Label2" runat="server" Text="都道府県" Width="120"></asp:Label>

                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPreFrom" runat="server"></asp:DropDownList>
                            <asp:Label ID="Label3" runat="server" Text="～" Width="25px" Class="text-center"></asp:Label>
                            <asp:DropDownList ID="ddlPreTo" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:2px;">

                            <asp:Label ID="lblHoleConstraction" runat="server" Text="工事種別" Width="100px"></asp:Label>

                        </td>
                        <td colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbxNew" runat="server" Text="新規" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxExpansion" runat="server" Text="増設" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxSomeRemoval" runat="server" EnableTheming="True" Text="一部撤去" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxShopRelocation" runat="server" Text="店内移設" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxAllRemoval" runat="server" Text="全撤去" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxOnceRemoval" runat="server" Text="一時撤去" Width="100px" />
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbxReInstallation" runat="server" Text="再設置" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxConChange" runat="server" Text="構成変更" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxConDelivery" runat="server" Text="構成配信" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxOther" runat="server" Text="その他" Width="100px" />
                                    </td>

                                    <td>
                                        <asp:CheckBox ID="cbxVup" runat="server" Text="ＶＵＰ" Width="100px" />
                                    </td>

                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="トラブル関連" Width="120"></asp:Label>
                        </td>
                        <td colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbxAll" runat="server" Text="発生工事全" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxDelay" runat="server" Text="工事遅延" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxSpare" runat="server" Text="予備機使用" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxVain" runat="server" Text="空振り工事" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxIns" runat="server" Text="ＩＮＳ" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxTel" runat="server" Text="ＴＥＬ" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxOth" runat="server" Text="その他" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbxOrgnz" runat="server" Text="構成" Width="100px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <!--該当件数表示 & リロードボタン-->
    <div ID="divCount" runat="server" class="float-Left">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 80px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX" style="font-size:12pt;"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 15px"></td>
                <td>
                    <asp:Button ID="btnReload" runat="server" CssClass="center" Text="リロード" />
                </td>
        </table>
    </div>
    <div id="DivOut" runat="server" class="grid-out" style="height: 509px;">
        <div id="DivIn" runat="server" class="grid-in" style="height: 509px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
                <EditRowStyle Height="40px" />
                <RowStyle Height="40px" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
