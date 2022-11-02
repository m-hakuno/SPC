<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMTimeBox.ascx.vb" Inherits="SPC.ClsCMTimeBox" %>
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
                    <asp:TextBox ID="txtHour" runat="server" Width="15px" CssClass="ime-disabled" MaxLength="2"></asp:TextBox>
                    <asp:Label ID="lblColon" runat="server" Text=":"></asp:Label>
                    <asp:TextBox ID="txtMin" runat="server" Width="15px" CssClass="ime-disabled" MaxLength="2"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvTimeBox" runat="server" ErrorMessage="CustomValidator" SetFocusOnError="True" CssClass="errortext" Display="Dynamic" ControlToValidate="txtHour" ValidateEmptyText="True"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
