<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSLSTP005.aspx.vb" Inherits="SPC.CNSLSTP005" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppIMEMode="半角_変更不可" ppName="ＴＢＯＸＩＤ" ppNameWidth="100" ppWidth="64" ppMaxLength="8" ppCheckHan="True" ppNum="True" />
            </td>
            <td style="padding-left: 10px;">
                <uc:ClsCMDateBoxFromTo ID="dftConstD" runat="server" ppName="工事日" ppNameWidth="100" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtCnstNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="工事依頼番号" ppNameWidth="100" ppWidth="100" ppCheckHan="True" />
            </td>
            <td style="padding-left: 10px;">
                <uc:ClsCMTextBox ID="txtCommNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="連絡票管理番号" ppNameWidth="100" ppWidth="80" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtHallNm" runat="server" ppMaxLength="50" ppName="ホール名" ppNameWidth="100" ppWidth="410" ppIMEMode="全角" />
            </td>
            <td style="padding-left: 10px;">
                <%--<uc:ClsCMTextBox ID="txtCommNo_" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="工事連絡票番号" ppNameWidth="100" ppWidth="120" ppCheckHan="True" ppNum="True" ppCheckLength="False" ppCheckAc="False" />--%>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblNGCStatus" runat="server" Text="進捗状況"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlNGCStatus" runat="server">
                            </asp:DropDownList>
                            <%--                            <div style="white-space: nowrap">
                                <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvddlNGCStatus" runat="server" ControlToValidate="ddlNGCStatus" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>--%>
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
        <div id="DivIn" runat="server" class="grid-in" style="height: 427px; margin-top: 13px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
