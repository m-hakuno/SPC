<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="DSULSTP001.aspx.vb" Inherits="SPC.DSULSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftGcReportNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="ＧＣ報告ＮＯ" ppNameWidth="90" ppWidth="120" ppCheckHan="True" ppCheckLength="False" ppExpression="\d{2}-\d{2}-\d{2}" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="90" ppWidth="70" ppCheckHan="True" ppNum="True" ppCheckLength="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftRspnsD" runat="server" ppName="対応日" ppNameWidth="90" />
            </td>
        </tr>
        <tr>
            <td>
                <table border="0">
                    <tr>
                        <td style="width: 90px">
                            <asp:Label ID="lblStatus" runat="server" Text="進捗状況"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
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
