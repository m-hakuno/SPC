<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMDropDownList_Mastr.ascx.vb" Inherits="SPC.ClsCMDropDownList_Mastr" Explicit="true" %>
<asp:Panel ID="pnlCtrl" runat="server" Wrap="False">
    <table border="0">
        <tr style="vertical-align: top">
            <td style="position: relative; top: 3px">
                <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                <asp:Panel ID="pnlData" runat="server">
                    <asp:DropDownList ID="ddlList" runat="server" DataSourceID="SqlDataSource">
                    </asp:DropDownList>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvDropDownList" runat="server" ControlToValidate="ddlList" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>