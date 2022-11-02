<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="DOCUPDP001.aspx.vb" Inherits="SPC.DOCUPDP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
     <asp:Panel ID="Panel2" runat="server">
         <table class="center" border="0">
            <tr>
                <td>
                    <uc:ClsCMDateBox runat="server" ID="dftNendoDt" ppName="年月度" ppDateFormat="年月" ppRequiredField="True" />
                </td>
             </tr>
          </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" >
    <table class="center" border="0" >
         <tr>
             <td >
                 <div class="float-left">
              <table class="center" border="0" >
                <tr>
                    <td class="align-top" style="padding-top: 8px">
                        <asp:Label ID="Label2" runat="server" Text="年月度："></asp:Label>
                        <asp:Label ID="lblNendoYM" runat="server" Text=""></asp:Label>
                    </td>
                     <td class="align-top">
                         <uc:ClsCMTextBox runat="server" ID="txtSonota1" ppName="その他１" ppMaxLength="20" ppValidationGroup="2" ppWidth="300" ppTextAlign="左" />
                    </td>
                     <td class="align-top">
                         <uc:ClsCMTextBox runat="server" ID="txtKingaku1" ppName="金額１" ppMaxLength="8" ppIMEMode="半角_変更不可" ppTextMode="Number" ppValidationGroup="2" ppTextAlign="右" />
                    </td>
                 </tr>
                 <tr>
                    <td>
                    </td>
                     <td class="align-top">
                         <uc:ClsCMTextBox runat="server" ID="txtSonota2" ppName="その他２" ppMaxLength="20" ppValidationGroup="2" ppWidth="300" ppTextAlign="左" />
                    </td>
                     <td class="align-top">
                         <uc:ClsCMTextBox runat="server" ID="txtKingaku2" ppName="金額２" ppMaxLength="8" ppIMEMode="半角_変更不可" ppTextMode="Number" ppValidationGroup="2" ppTextAlign="右" />
                    </td>
                 </tr>
                  <tr>
                    <td colspan="2">
                        <div class="float-left">
                            <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="2" />
                        </div>
                    </td>
                     <td colspan="2">
                        <div class="float-right">
                           <%--<asp:Button ID="btnAdd" runat="server" Text="追加" />--%>
                           &nbsp;
                           <asp:Button ID="btnUpdate" runat="server" Text="更新" ValidationGroup="2" Visible="False" />
                            &nbsp;
                           <%--<asp:Button ID="btnDelete" runat="server" Text="削除" />--%>
                        </div>
                    </td>
                </tr>
               </table>
                </div>
            </td>
         </tr>
    </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
      <table class="center">
        <tr>
            <td>
                <div class="grid-out" style="height: 156px; width: 613px;">
                    <div class="grid-in" style="height: 156px; width: 613px;">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
      </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
