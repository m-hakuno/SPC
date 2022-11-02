<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMTimeBoxFromTo.ascx.vb" Inherits="SPC.ClsCMTimeBoxFromTo" %>
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
                    <asp:TextBox ID="txtHourFrom" runat="server" Width="20px" CssClass="ime-disabled" MaxLength="2"></asp:TextBox>
                    <asp:Label ID="lblColonFrom" runat="server" Text=":"></asp:Label>
                    <asp:TextBox ID="txtMinFrom" runat="server" Width="20px" CssClass="ime-disabled" MaxLength="2"></asp:TextBox>
                    <span style="position: relative; top: 3px; vertical-align: top;">
                        <asp:Label ID="lblFromTo" runat="server" Text="～"></asp:Label>
                    </span>
                    <asp:TextBox ID="txtHourTo" runat="server" Width="20px" CssClass="ime-disabled" MaxLength="2"></asp:TextBox>
                    <asp:Label ID="lblColonTo" runat="server" Text=":"></asp:Label>
                    <asp:TextBox ID="txtMinTo" runat="server" Width="20px" CssClass="ime-disabled" MaxLength="2"></asp:TextBox>
                    <div style="white-space: nowrap">                    
                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvTimeBox" runat="server" ErrorMessage="CustomValidator" SetFocusOnError="True" CssClass="errortext" Display="Dynamic" ControlToValidate="txtHourFrom" ValidateEmptyText="True"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
