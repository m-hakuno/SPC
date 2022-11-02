<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="REPUPDP001.aspx.vb" Inherits="SPC.REPUPDP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px; height: 23px;"></td>
                         <td style= "padding-left :4px; width: 100px; height: 23px;">
                             <asp:Label ID="lblMakerKubun" runat="server" Text="メーカ名" Width="100px" ></asp:Label>
                         </td>
                         <td style="width: 290px; height: 23px;">
                             <asp:DropDownList ID="ddlMakerKubun" runat="server" Width="300px" ValidationGroup="1">
                             </asp:DropDownList>
                         </td>
                         <td style="height: 23px"></td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px">&nbsp;</td>
                         <td colspan="3">
                             <uc:ClsCMDropDownList runat="server" ID="ddlWrkCls" ppClassCD="0092" ppValidationGroup="1" ppName="作業分類" ppNameWidth="100" ppNotSelect="True" />
                         </td>
                      </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px">&nbsp;</td>
                         <td>
                             <uc:ClsCMTextBoxFromTo ID="tftPartsCd" runat="server" ppNameWidth="100" ppName="部品コード" ppWidth="50" ppMaxLength="5" ppNum="True" ppValidationGroup="" ppEnabled="True" ppIMEMode="半角_変更不可" />
                         </td>
                     </tr>
                 </table>

                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px">&nbsp;</td>
                         <td colspan="3">
                             <uc:ClsCMTextBox ID="txtPartsNm" runat="server" ppName="部品名" ppNameWidth="100" ppWidth="300" ppMaxLength="20" ppValidationGroup="1" ppIMEMode="全角" />
                         </td>
                      </tr>
                 </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">

                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style= "padding-bottom :5px;width: 131px">
                             <asp:Label ID="Label6" runat="server" Text="部品マスタ詳細" Font-Bold="True"></asp:Label>
                         </td>
                     </tr>
                 </table>
          <asp:Panel ID="pnlRegister" runat="server"  BorderStyle="Solid" BorderWidth="1px">
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 74px; height: 44px;"></td>
                         <td style= "padding-top :3px; width: 89px">
                             <asp:Label ID="lblMakerKubun_S" runat="server" Text="メーカ名" Width="100px" ></asp:Label>
                         </td>
                         <td style= "padding-left :4px">
                             <asp:Panel ID="pnlData" runat="server">
                                 <asp:DropDownList ID="ddlMakerKubun_S" runat="server" Width="300px">
                                 </asp:DropDownList>
                                 <div style="white-space: nowrap">
                                     <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                         <asp:CustomValidator ID="cuvDropDownList" runat="server" ControlToValidate="ddlMakerKubun_S" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2"></asp:CustomValidator>
                                     </asp:Panel>
                                 </div>
                             </asp:Panel>
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 18px; height: 44px;"></td>
                         <td style="width: 256px">
                             <uc:ClsCMDropDownList runat="server" ID="ddlWrkCls_S" ppClassCD="0092" ppValidationGroup="2" ppName="作業分類" ppNameWidth="100" ppNotSelect="True" ppRequiredField="True" />  
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px; height: 44px;"></td>
                         <td style="width: 256px">
                              <%--<uc:ClsCMTextBox ID="txtPartsCd_S" runat="server" ppName="部品コード" ppNameWidth="100" ppWidth="50" ppMaxLength="4" ppValidationGroup="2" ppCheckHan="False" ppNum="True" ppRequiredField="True" ppIMEMode="半角_変更不可" ppCheckLength="True" />--%>
                             <uc:ClsCMTextBox ID="txtPartsCd_S" runat="server" ppName="部品コード" ppNameWidth="100" ppWidth="50" ppMaxLength="5" ppValidationGroup="2" ppCheckHan="False" ppNum="True" ppRequiredField="True" ppIMEMode="半角_変更不可" ppCheckLength="True" />
                         </td>
                         <td>
                              <uc:ClsCMTextBox ID="txtPartsNm_S" runat="server" ppName="部品名" ppNameWidth="100" ppWidth="300" ppMaxLength="20" ppValidationGroup="2" ppCheckHan="False" ppRequiredField="True" ppIMEMode="全角" />
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px">&nbsp;</td>
                         <td colspan="3">
                             <uc:ClsCMTextBox ID="txtMakerWrkNm_S" runat="server" ppName="メーカ作業名称" ppNameWidth="100" ppWidth="420" ppMaxLength="30" ppValidationGroup="2" ppIMEMode="全角" />
                         </td>
                     </tr>
                     <tr>
                         <td style="width: 73px">&nbsp;</td>
                         <td colspan="3">
                               <uc:ClsCMTextBox ID="txtNgcWrkNm_S" runat="server" ppName="ＮＧＣ作業名称" ppNameWidth="100" ppWidth="420" ppMaxLength="30" ppValidationGroup="2" ppIMEMode="全角" />
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style= "padding-bottom :5px;width: 73px">
                             <asp:Label ID="lblAdjust1" runat="server" Text="適応①"></asp:Label>
                         </td>
                         <td>
                                  <uc:ClsCMDateBox runat="server" ID="dtbStartDt1_S" ppName="適応開始日" ppNameWidth="100" ppValidationGroup="2" ppRequiredField="True" />
                         </td>
                         <td>
                                  <uc:ClsCMTextBox ID="txtPrice11_S" runat="server" ppName="作業単価１" ppNameWidth="100" ppWidth="200" ppMaxLength="8" ppValidationGroup="2" ppNum="True" ppRequiredField="True" ppIMEMode="半角_変更不可" />
                         </td>
                         <td>
                                  <uc:ClsCMTextBox ID="txtPrice12_S" runat="server" ppName="作業単価２" ppNameWidth="100" ppWidth="200" ppMaxLength="8" ppValidationGroup="2" ppNum="True" ppRequiredField="True" ppIMEMode="半角_変更不可" />
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style= "padding-bottom :5px;width: 73px">
                             <asp:Label ID="lblAdjust2" runat="server" Text="適応②"></asp:Label>
                         </td>
                         <td>
                                  <uc:ClsCMDateBox runat="server" ID="dtbStartDt2_S" ppName="適応開始日" ppNameWidth="100" ppValidationGroup="2" />
                         </td>
                         <td>
                                  <uc:ClsCMTextBox ID="txtPrice21_S" runat="server" ppName="作業単価１" ppNameWidth="100" ppWidth="200" ppMaxLength="8" ppValidationGroup="2" ppNum="True" ppIMEMode="半角_変更不可" />
                         </td>
                         <td>
                                  <uc:ClsCMTextBox ID="txtPrice22_S" runat="server" ppName="作業単価２" ppNameWidth="100" ppWidth="200" ppMaxLength="8" ppValidationGroup="2" ppNum="True" ppIMEMode="半角_変更不可" />
                         </td>
                     </tr>
                 </table>
                  <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style= "padding-bottom :5px;width: 73px">
                             <asp:Label ID="lblAdjust3" runat="server" Text="適応③"></asp:Label>
                         </td>
                         <td>
                                  <uc:ClsCMDateBox runat="server" ID="dtbStartDt3_S" ppName="適応開始日" ppNameWidth="100" ppValidationGroup="2" />
                         </td>
                         <td>
                                  <uc:ClsCMTextBox ID="txtPrice31_S" runat="server" ppName="作業単価１" ppNameWidth="100" ppWidth="200" ppMaxLength="8" ppValidationGroup="2" ppNum="True" ppIMEMode="半角_変更不可" />
                         </td>
                         <td>
                                  <uc:ClsCMTextBox ID="txtPrice32_S" runat="server" ppName="作業単価２" ppNameWidth="100" ppWidth="200" ppMaxLength="8" ppValidationGroup="2" ppNum="True" ppIMEMode="半角_変更不可" />
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 75px; height: 44px;"></td>
                         <td style= "padding-top :3px; width: 89px">
                             <asp:Label ID="Label1" runat="server" Text="システム名称" Width="100px" ></asp:Label>
                         </td>
                         <td style= "padding-left :4px">
                             <asp:Panel ID="Panel1" runat="server">
                                 <asp:DropDownList ID="ddlSystem_S" runat="server" Width="200">
                                 </asp:DropDownList>
                                 <div style="white-space: nowrap">
                                     <asp:Panel ID="Panel2" runat="server" Width="0px">
                                         <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ddlMakerKubun_S" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2"></asp:CustomValidator>
                                     </asp:Panel>
                                 </div>
                             </asp:Panel>
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 75px; height: 44px;"></td>
                         <td style= "padding-top :3px; width: 89px">
                             <asp:Label ID="Label2" runat="server" Text="機器名称" Width="100px" ></asp:Label>
                         </td>
                         <td style= "padding-left :4px">
                             <asp:Panel ID="Panel3" runat="server">
                                 <asp:DropDownList ID="ddlAppaNm_S" runat="server" Width="200">
                                 </asp:DropDownList>
                                 <div style="white-space: nowrap">
                                     <asp:Panel ID="Panel4" runat="server" Width="0px">
                                         <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="ddlMakerKubun_S" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2"></asp:CustomValidator>
                                     </asp:Panel>
                                 </div>
                             </asp:Panel>
                         </td>
                     </tr>
                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px">&nbsp;</td>
                         <td style= "padding-left :4px; height: 23px;" colspan="3">
                             <asp:Label ID="lblInsertDt" runat="server" Text="登録日　　：　　"></asp:Label>
                             <asp:Label ID="lblInsertDt_S" runat="server" Text="9999/99/99"></asp:Label>
                             <asp:Label ID="lblUpdateDt" runat="server" Text="　　　更新日　　：　　"></asp:Label>
                             <asp:Label ID="lblUpdateDt_S" runat="server" Text="9999/99/99"></asp:Label>
                         </td>
                     </tr>

                 </table>
                 <table style="width:1050px;" border="0" class="left">
                     <tr>
                         <td style="width: 73px">
                             
                             <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errortext" ValidationGroup="2" />

                         </td>
                         <td style="width: 73px">
                             

                         </td>
                         <td style="width: 73px">
                             

                             &nbsp;</td>
                     </tr>

                 </table>
                 <table style="width:1050px;" border="0" class="center">
                     <tr>
                         <td style="width: 73px">
                             
                    <div class="float-right">
                        <asp:Button ID="btnSearchRigth5" runat="server" Text="Button" Visible="False" />
                        &nbsp;<asp:Button ID="btnSearchRigth4" runat="server" Text="Button" Visible="False" />
                        &nbsp;<asp:Button ID="btnSearchRigth3" runat="server" Text="Button" Visible="False" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" Text="クリア" />
                        &nbsp;<asp:Button ID="btnInsert" runat="server" Text="追加" ValidationGroup="2" />
                        &nbsp;<asp:Button ID="btnUpdate" runat="server" Text="更新" Enabled="False" ValidationGroup="2" />
                        &nbsp;<asp:Button ID="btnDelete" runat="server" Text="削除" Enabled="False" ValidationGroup="2" />
                    </div>
                         </td>
                     </tr>
                 </table>

                  </asp:Panel>   
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
