<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSLSTP006.aspx.vb" Inherits="SPC.CNSLSTP006" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppIMEMode="半角_変更不可" ppName="ＴＢＯＸＩＤ" ppNameWidth="80" ppMaxLength="8" ppCheckHan="True" />
            </td>
            <td style="width:80px;">
                <uc:ClsCMTextBox runat="server" ID="txtNLCls" ppName="NL区分" ppNameWidth="40" ppMaxLength="1" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="15" ppExpression="[n|N|l|L|j|J]" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtCnsReqestNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="工事依頼番号" ppNameWidth="90" ppWidth="110" ppCheckHan="True" />
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMDateTimeBox runat="server" ID="dtbCnstFm" ppName="工事日時" ppNameWidth="70" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="～"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMDateTimeBox runat="server" ID="dtbCnstTo" ppName="工事日時" ppNameVisible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtHoleNm" runat="server" ppMaxLength="30" ppName="ホール" ppNameWidth="80" ppWidth="430" ppIMEMode="全角" />
            </td>
            <td>
                <table style="float: left;">
                    <tr>
                        <td>
                            <uc:ClsCMTextBox runat="server" ID="txtClosingDt" ppName="締日" ppNameWidth="70" ppIMEMode="半角_変更不可" ppMaxLength="5" ppNum="False" />
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblClosingDt" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">上</asp:ListItem>
                                <asp:ListItem Value="2">下</asp:ListItem>
                                <asp:ListItem Value="3">その他</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width:1050px;margin-top:8px" border="0" class="center">
        <tr>
            <td>
                <asp:Label ID="lblHoleConstraction" runat="server" Text="ホール内工事" Width="100px"></asp:Label>
            </td>
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
           
            <td style="width: 20px">&nbsp;</td>
            <td style="width: 30%">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out" style="height: 430px;">
        <div id="DivIn" runat="server" class="grid-in" style="height: 430px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <br />
    <div style="width: 98%; font-size: 14px; text-align: right; font-weight: bold;">
        <asp:Label ID="lblTotal" runat="server" Text="合計："></asp:Label>
        <asp:Label ID="lblTotalPrice" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
