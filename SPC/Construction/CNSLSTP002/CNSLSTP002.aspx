<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSLSTP002.aspx.vb" Inherits="SPC.CNSLSTP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" border="0" class="center">
        <tr class="align-top">
            <td style="width: 50%">
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblRepuest" runat="server" Text="依頼区分" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="ckbArtcl" runat="server" Text="物品転送" />
                        </td>
                        <td>
                            <asp:CheckBox ID="ckbPack" runat="server" Text="梱包箱出荷" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%" id="100">
                <uc:clscmdateboxfromto runat="server" id="dftDelivDt" ppNameWidth="100" ppName="納期" />
            </td>
        </tr>
        <tr class="align-top">
            <td>
                <uc:ClsCMDateBoxFromTo runat="server" ID="dftArtcltransD" ppName="依頼日" ppNameWidth="100" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="tftArtclNo" ppNameWidth="100" ppName="指示Ｎｏ" ppIMEMode="半角_変更不可" ppMaxLength="10"
                     ppWidth="67" ppCheckHan="True" />
            </td>
        </tr>
        <tr class="align-top">
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="tftTboxId" ppNameWidth="100" ppName="ＴＢＯＸＩＤ" ppMaxLength="8" ppIMEMode="半角_変更不可" ppWidth="54" ppCheckHan="True" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="tftRequestNo" ppNameWidth="100" ppName="依頼番号" ppIMEMode="半角_変更不可" ppMaxLength="14" ppWidth="120" ppCheckHan="True" />
            </td>
        </tr>
        <tr class="align-top">
            <td>
                <uc:ClsCMDateBoxFromTo runat="server" ID="dftConstDt" ppNameWidth="100" ppName="工事日" />
            </td>
            <td>
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="出力順" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSort" runat="server">
                                <asp:ListItem Value="1">1:ＴＢＯＸＩＤ順</asp:ListItem>
                                <asp:ListItem Value="2">2:納期順</asp:ListItem>
                                <asp:ListItem Value="3">3:依頼日順</asp:ListItem>
                                <asp:ListItem Value="4">4:依頼番号順</asp:ListItem>
                                <asp:ListItem Value="5">5:指示Ｎｏ順</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in" style="height:400px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>