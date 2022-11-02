<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMTextBoxTwoFromTo.ascx.vb" Inherits="SPC.ClsCMTextBoxTwoFromTo" %>
<asp:Panel ID="pnlCtrl" runat="server" Wrap="False">
    <table border="0">
        <tr style="vertical-align: top">
            <td style="position: relative; top: 3px">
                <asp:Panel ID="pnlName" runat="server">
                    <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlData" runat="server">
                    <asp:TextBox ID="txtTextBoxFrom" runat="server"></asp:TextBox>
                    <asp:Label ID="lblHyphenFrom" runat="server" Text="-"></asp:Label>
                    <asp:TextBox ID="txtTextBoxTwoFrom" runat="server"></asp:TextBox>
                    <span style="position: relative; top: 3px; vertical-align: top;">
                        <asp:Label ID="lblFromTo" runat="server" Text="～"></asp:Label>
                    </span>
                    <asp:TextBox ID="txtTextBoxTo" runat="server"></asp:TextBox>
                    <asp:Label ID="lblHyphenTo" runat="server" Text="-"></asp:Label>
                    <asp:TextBox ID="txtTextBoxTwoTo" runat="server"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvTextBox" runat="server" ControlToValidate="txtTextBoxFrom" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>