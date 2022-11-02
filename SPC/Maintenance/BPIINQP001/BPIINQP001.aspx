<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="BPIINQP001.aspx.vb" Inherits="SPC.BPIINQP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" style="width:100%;">
        <tr>
            <td style="width: 15%">&nbsp;</td>
            <td>
                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="100" ppWidth="80" ppCheckHan="True" ppCheckLength="True" ppNum="True" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblHallNm1" runat="server" Text="ホール名"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblHallNm2" runat="server" Text="ＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸ"></asp:Label>
                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblVER1" runat="server" Text="ＶＥＲ"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblVER2" runat="server" Text="XX.XX"></asp:Label>
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
        <div id="DivIn" runat="server" class="grid-in" style="height:410px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
