<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="WATLSTP001.aspx.vb" Inherits="SPC.WATLSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="110" ppWidth="80" ppIMEMode="半角_変更不可" ppCheckHan="True" ppNum="True" ppCheckLength="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftMntrReportNo" runat="server" ppMaxLength="15" ppName="管理番号" ppWidth="120" ppNameWidth="110" ppIMEMode="半角_変更不可" ppCheckHan="True" ppCheckAc="False" ppCheckLength="False" ppExpression="KS\d{4}-\d{2}-\d{3}" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftOBreakD" runat="server" ppName="発生日" ppNameWidth="110" />
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 110px">
                            <asp:Label ID="lblMntrEnd1" runat="server" Text="案件終了"></asp:Label>
                        </td>
                        <td style="width: 30px">
                            <asp:CheckBox ID="cbxMntrEnd" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblMntrEnd2" runat="server" Text="※チェックの場合、案件終了を含む"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlHpnStts" runat="server" ppName="発生状況" ppNameWidth="110" ppWidth="150" ppClassCD="0017" ppMode="名称" ppNotSelect="True" ppRequiredField="False" />
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 110px">
                            <asp:Label ID="lblStatus" runat="server" Text="進捗状況"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="110px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 110px">
                            <asp:Label ID="lblDealStatus" runat="server" Text="ＮＧＣ対応状況"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDealStatus" runat="server" Width="130px">
                            </asp:DropDownList>
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
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
