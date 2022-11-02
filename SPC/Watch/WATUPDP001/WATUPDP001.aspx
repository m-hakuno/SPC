<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WATUPDP001.aspx.vb" Inherits="SPC.WATUPDP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br />
    <asp:Panel ID="Panel1" runat="server" CssClass="text-center">
        <asp:Label ID="Label1" runat="server" Text="サポートセンタ　記述項目"></asp:Label>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlSPC" runat="server">
        <table border="1" class="center">
            <tr>
                <td style="width: 85px">
                    <asp:Label ID="lblMntrReportNo1" runat="server" Text="管理番号"></asp:Label>
                </td>
                <td style="text-indent: 9px">
                    <asp:Label ID="lblMntrReportNo2" runat="server" Text="123456789012345"></asp:Label>
                </td>
                <td style="width: 65px">
                    <asp:Label ID="lblOBreakD" runat="server" Text="発生日"></asp:Label>
                </td>
                <td>
                    <uc:ClsCMDateBox ID="dttOBreakD" runat="server" ppNameVisible="False" ppName="発生日" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTboxId" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppNameVisible="False" ppWidth="210" ppIMEMode="半角_変更不可" ppCheckHan="True" ppMaxLength="20" ppName="ＴＢＯＸＩＤ" ppNum="False" ppRequiredField="True" ppCheckLength="False" />
                </td>
                <td>
                    <asp:Label ID="lblHallNm1" runat="server" Text="ホール名"></asp:Label>
                </td>
                <td style="text-indent: 5px">
                    <asp:Label ID="lblHallNm2" runat="server" Text="Label" Width="540px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSpcCharge" runat="server" Text="報告者"></asp:Label>
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtSpcCharge" runat="server" ppNameVisible="False" ppWidth="210" ppName="報告者" ppMaxLength="20" ppIMEMode="全角" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="cbxDeleteFlg" runat="server" Text="削　除" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="進捗状況"></asp:Label>
                </td>
                <td colspan="3" style="text-indent: 7px">
                    <asp:DropDownList ID="ddlStatus" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblHpnStts" runat="server" Text="発生状況"></asp:Label>
                </td>
                <td colspan="3">
                    <uc:ClsCMDropDownList ID="ddlHpnStts" runat="server" ppNameVisible="False" ppClassCD="0017" ppName="発生状況" ppNotSelect="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblContent" runat="server" Text="報告内容"></asp:Label>
                </td>
                <td colspan="3">
                    <uc:ClsCMTextBox ID="txtContent" runat="server" ppNameVisible="False" ppTextMode="MultiLine" ppWidth="800" ppName="報告内容" ppHeight="65" ppMaxLength="300" ppIMEMode="全角" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <hr />
    <br />
    <asp:Panel ID="Panel2" runat="server" CssClass="text-center">
        <asp:Label ID="Label2" runat="server" Text="ＮＧＣ　記述項目"></asp:Label>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlNGC" runat="server">
        <table border="1" class="center">
            <tr>
                <td style="width: 60px">
                    <asp:Label ID="lblNgcDeal" runat="server" Text="対応者"></asp:Label>
                </td>
                <td style="width: 290px">
                    <uc:ClsCMTextBox ID="txtNgcDeal" runat="server" ppNameVisible="False" ppWidth="280" ppName="対応者" ppMaxLength="20" ppIMEMode="全角" />
                </td>
                <td style="width: 60px">
                    <asp:Label ID="lblDealStatus" runat="server" Text="対応状況"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDealStatus" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAnswer" runat="server" Text="対応内容"></asp:Label>
                </td>
                <td colspan="3">
                    <uc:ClsCMTextBox ID="txtAnswer" runat="server" ppNameVisible="False" ppTextMode="MultiLine" ppWidth="800" ppMaxLength="300" ppName="対応内容" ppHeight="130" ppIMEMode="全角" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <hr />
    <br />
    <asp:Panel ID="Panel3" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <table border="0" class="center">
                        <tr>
                            <td>
                                <asp:Button ID="btnUpdate" runat="server" Text="更新" />
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Text="登録" />
                            </td>
                            <td>
                                <asp:Button ID="btnPrint" runat="server" Text="印刷" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="float-left">
                        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" />
                        <asp:ValidationSummary ID="vasSummaryTboxId" runat="server" CssClass="errortext" ValidationGroup="TboxId" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
