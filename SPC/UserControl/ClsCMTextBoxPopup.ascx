<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMTextBoxPopup.ascx.vb" Inherits="SPC.ClsCMTextBoxPopup" %>
<%@ Register Src="~/UserControl/ClsCMpnlPopup.ascx" TagPrefix="uc" TagName="ClsCMpnlPopup" %>
<asp:Panel ID="pnlCtrl" runat="server" Wrap="False">
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">  
        <ContentTemplate>  
            <table border="0">
                <tr style="vertical-align: top">
                    <td style="position: relative; top: 5px">
                        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        <asp:Panel ID="pnlData" runat="server">
                            <asp:TextBox ID="txtTextBox" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:Button ID="btnRef" runat="server" Text="参照" CausesValidation="False"/>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvTextBox" runat="server" ControlToValidate="txtTextBox" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>  
    </asp:UpdatePanel>  
    <uc:ClsCMpnlPopup runat="server" id="ClsCMpnlPopup" />  
</asp:Panel>

