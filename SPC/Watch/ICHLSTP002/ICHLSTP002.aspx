<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="ICHLSTP002.aspx.vb" Inherits="SPC.ICHLSTP002" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
<script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/EnableChange.js")%>'></script>
    <asp:Panel ID="Panel1" runat="server">
        <table style="width: 1050px;" class="center">
            <tr>
                <td style="width: 200px">
                <td colspan="4">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblTboxID" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
                                </td>
                                <td style="width: 19px">
                                <td class="align-top">
                                    <asp:Label ID="lblTboxID_Input" runat="server" Text="NNNNNNNN"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                <td colspan="4">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblHallName" runat="server" Text="ホール名"></asp:Label>
                                </td>
                                <td style="width: 46px">
                                <td class="align-top">
                                    <asp:Label ID="lblHallName_Input" runat="server" Text="ＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮ"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                <td colspan="4">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblTboxType" runat="server" Text="ＶＥＲ"></asp:Label>
                                </td>
                                <td style="width: 60px">
                                <td class="align-top">
                                    <asp:Label ID="lblTboxType_Input" runat="server" Text="99.99"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                <td colspan="4">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblDataDT" runat="server" Text="データ日付"></asp:Label>
                                </td>
                                <td style="width: 35px">
                                <td class="align-top">
                                    <asp:Label ID="lblDataDT_Input" runat="server" Text="9999/99/99"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td><br></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td style="width: 220px" class="align-top">
                    <uc:ClsCMTextBox runat="server" ID="txtCID" ppName="ＣＩＤ" ppMaxlength="16" ppNum="True" ppIMEMode="半角_変更不可" />
                </td>
                <td style="width: 220px" class="align-top">
                    <uc:ClsCMTextBoxFromTo runat="server" ID="txtUnyo" ppName="運用機番" ppWidth="50" ppMaxLength="4" ppNum="True" ppIMEMode="半角_変更不可" />
                </td>
                <td style="width: 220px" class="align-top">
                    <uc:ClsCMTimeBoxFromTo runat="server" ID="txtJikantai" ppName="時間帯" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <uc:ClsCMTextBoxFromTo runat="server" ID="txtJBNum" ppName="ＪＢ番号" ppWidth="50"  ppMaxLength="4" ppNum="True" ppIMEMode="半角_変更不可" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
