<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMpnlPopup.ascx.vb" Inherits="SPC.ClsCMpnlPopup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>  

<asp:Panel ID="pnlPopup" runat="server" BackColor="#C0C0FF" style="display : none">  
    <asp:UpdatePanel ID="updPopup" runat="server" UpdateMode="Conditional">  
        <ContentTemplate>  
            <asp:Button ID="btnDummy" runat="server" Text="" Style="display: none" />
            <asp:Button ID="btnDummy2" runat="server" Text="" Style="display: none" />
            <asp:ModalPopupExtender ID="modalPopupExtender" runat="server"  
                TargetControlID="btnDummy"  
                BackgroundCssClass="modalBackground"  
                DropShadow="true"  
                PopupControlID="pnlPopup"  
                PopupDragHandleControlID="pnlTitle"  
                CancelControlID="btnDummy2" />  
            <asp:Panel ID="pnlTitle" runat="server"> 
                <table style="width: 100%;" >
                    <tr>
                        <td>
                             <table class="center">
                                 <tr>
                                    <td>
                                        <asp:Label ID="lblTitle" runat="server" Text="社員名一覧" Font-Size="Large"></asp:Label>
                                    </td>  
                                </tr>
                            </table>
                        </td>  
                    </tr>
                 </table>
            </asp:Panel>  
            <div>  
                <asp:GridView ID="grvList" runat="server" AllowPaging="True" PageSize="5">  
                    <Columns>  
                        <asp:TemplateField HeaderText="選択">  
                            <ItemTemplate>  
                                <asp:LinkButton ID="lnkbtnSelect" runat="server" CommandName="select">選択</asp:LinkButton>  
                            </ItemTemplate>  
                        </asp:TemplateField>  
                    </Columns>  
                </asp:GridView>
                <asp:Button ID="btnCancel" runat="server" Text="閉じる"/>  
            </div>  
         </ContentTemplate>  
    </asp:UpdatePanel>  
</asp:Panel>  
