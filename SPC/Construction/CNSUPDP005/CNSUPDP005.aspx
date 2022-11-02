<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSUPDP005.aspx.vb" Inherits="SPC.CNSUPDP005" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center" style="width: 1050px;">
        <tr>
            <td style="width: 20%">&nbsp;</td>
            <td>
                <table border="0" style="width:100%;">
                    <tr>
                        <td colspan="3">
                            <uc:ClsCMTextBox ID="txtConstructionRequestNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="工事依頼番号" ppNameWidth="80" ppWidth="150" ppCheckHan="True" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px" class="float-left" colspan="3">
                            <table>
                                <tr>
                                    <td>
                                        <uc:ClsCMTextBox ID="txtHoleNm" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="7" ppName="ホール" ppNameWidth="75" ppWidth="90" ppCheckHan="True" ppCheckLength="True" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHoleNm" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="80" ppWidth="100" ppCheckHan="True" ppCheckLength="True" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <uc:ClsCMDateBoxFromTo ID="dftConstructionDt" runat="server" ppName="工事日" ppNameWidth="80"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 38px;">
                            <table border="0" class="float-left">
                                <tr class="align-top">
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="進捗状況" Width="80" ></asp:Label>
                                    </td>
                                    <td>  
                                        <asp:Panel ID="Panel7" runat="server" Width="303px">                               
                                           <asp:DropDownList ID="drpProgressSituation" runat="server" Width ="150"></asp:DropDownList>
                                           <div style="white-space: nowrap">
                                                <asp:Panel ID="Panel8" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="CustomValidator3" runat="server" CssClass="errortext" Display="Dynamic" ValidateEmptyText="True"></asp:CustomValidator>
                                                </asp:Panel>
                                           </div>
                                        </asp:Panel>
                                    </td>
                                 </tr>
                            </table>                          
                        </td>
                        <%--<td style="width: 120px">
                            <uc:ClsCMTextBox ID="txtProgressSituation" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="進捗状況" ppNameWidth="80" ppWidth="40" ppCheckHan="True" ppValidationGroup="1" />
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblProgressSituation" runat="server"></asp:Label>
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="2" class="float-left">
                            <table>
                                <tr>
                                    <td>
                                       <uc:ClsCMTextBoxRef ID="trdBilling" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="請求先連番" ppNameWidth="80" ppWidth="50" ppCheckHan="True" ppValidationGroup="1"  ppURL="~/Common/COMSELP002/COMSELP002.aspx" /> 
                                    </td>
                                    <td>
                                         <asp:Label ID="lblBilling" runat="server"></asp:Label>    
                                    </td>
                                </tr>                   
                            </table>                       
                        </td>
                     <%--   <td style="width: 450px">
                            
                        </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width:1050px; " border="0" class="center">
        <tr>
            <td style="width: 20%">&nbsp;</td>
            <td>
                <table style="width:100%; background-color: #FFFFFF; border-spacing: 0px;" border="1">
                    <tr class="text-center">
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lblNew" runat="server" Text="新規"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblExpansion" runat="server" Text="増設"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblReInstallation" runat="server" Text="再　設置" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblShopRelocation" runat="server" Text="店内移設" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSomeRemoval" runat="server" Text="一部撤去" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAllRemoval" runat="server" Text="全　撤去" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOnceRemoval" runat="server" Text="一時撤去" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblConChange" runat="server" Text="構成変更" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDlvOrgnz" runat="server" Text="構成配信" Width="30px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblVup" runat="server" Text="ＶＵＰ"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOther" runat="server" Text="その他"></asp:Label>
                        </td>
                    </tr>
                    <tr class="text-center">
                        <td>
                            <asp:Label ID="lblHoleConstructionType" runat="server" Text="ホール工事種別" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleNew" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleExpansion" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleReInstallation" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleShopRelocation" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleSomeRemoval" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleAllRemoval" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleOnceRemoval" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleConChange" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleDlvOrgnz" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxVup" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxHoleOther" runat="server" />
                        </td>
                    </tr>
                    <tr class="text-center">
                        <td>
                            <asp:Label ID="lblLanConstructionType" runat="server" Text="ＬＡＮ工事種別" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanNew" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanExpansion" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanReInstallation" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanShopRelocation" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanSomeRemoval" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanAllRemoval" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanOnceRemoval" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanConChange" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxLanDlvOrgnz" runat="server" />
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:CheckBox ID="cbxLanOther" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-center">
                            <asp:Label ID="lblOtherConstructionType" runat="server" Text="その他工事内容" Width="100px"></asp:Label>
                        </td>
                        <td colspan="11">
                            <uc:ClsCMTextBox ID="txtSonotaNaiyo" runat="server" ppNameVisible="False" ppWidth="500" ppMaxLength="50" ppIMEMode="半角_変更可" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 10%">&nbsp;</td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
        <asp:Panel ID="pnlRegister" runat="server" BackColor="#FFCCFF" BorderStyle="Solid" BorderWidth="1px">
        <table border="0" >
            <tr>
                <td style="width: 10%; height: 76px;"></td>
                <td style="width: 302px; height: 76px;">
                    <uc:ClsCMTextBox runat="server" ID="txtKoziirai_IO" ppName="工事依頼番号"  ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppCheckLength="True" ppRequiredField="True" ppValidationGroup="2" ppMaxLength="14"/>
                </td>
                <td style="width: 233px; height: 76px;" >
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel9" runat="server">
                                    <asp:Label ID="lblRenban" runat="server" Text="連番:"></asp:Label>
                                    <asp:Label ID="lblRenban_data" runat="server"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 600px; height: 76px" >
                    <table>
                        <tr>
                            <td>
                                 <%--<uc:ClsCMTextBoxRef ID="txtSeikyusakiCD_IO" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="請求先コード" ppWidth="50" ppCheckHan="True" ppCheckLength="True" ppRequiredField="True" ppValidationGroup="2" />--%>
                                <uc:ClsCMTextBoxRef runat="server" ID="txtSeikyusakiCD" ppName="請求先連番"  ppNameWidth="75" ppWidth="50" ppCheckHan="True" ppCheckLength="False" ppRequiredField="True" ppValidationGroup="2" ppMaxLength="4" ppURL="~/Common/COMSELP002/COMSELP002.aspx" />
                            </td>
                            <td>
                                 <asp:Label ID="lblOffice_name" runat="server"></asp:Label>
                               
                            </td>
                        </tr>
                    </table>
                </td>
               <%--  <td style="width: 233px; height: 76px;" >
                    <table>
                        <tr>
                            <td>
                              
                            </td>
                        </tr>
                    </table>
                </td>--%>
            </tr>
            <tr>
                <td style="width: 10%">&nbsp;</td>
                <td style="width: 302px"  class="align-top">
                    <table border="0"  class="float-left">
                        <tr>
                            <td>
                                <asp:Label ID="lblshiryo" runat="server" Text="請求資料コード" Width="100"></asp:Label>   
                            </td>
                            <td>  
                                <asp:Panel ID="Panel2" runat="server">                      
                                    <asp:DropDownList ID="drpShiryouCD_IO" runat="server" ValidationGroup="2" Width="150"></asp:DropDownList>
                                    <div style="white-space: nowrap" class="align-top">
                                        <asp:Panel ID="Panel1" runat="server" Width="0px">
                                            <asp:CustomValidator ID="vldShiryouCD" runat="server" ValidationGroup="2" CssClass="errortext" Display="Dynamic" ValidateEmptyText="True"></asp:CustomValidator>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                            </td>
                         </tr>
                    </table>
                </td>
                <td style="width: 233px"class="align-top">
                    <table border="0"  class="float-left">
                        <tr>
                            <td>
                                <asp:Label ID="lblSeikyujoukyou" runat="server" Text="進捗状況" ></asp:Label>  
                            </td>
                            <td>  
                                <asp:Panel ID="Panel3" runat="server">                        
                                    <asp:DropDownList ID="drpSeikyuJokyo" runat="server" ValidationGroup="2" Width="150"></asp:DropDownList>
                                        <div style="white-space: nowrap" class="align-top">
                                            <asp:Panel ID="Panel4" runat="server" Width="0px">
                                                <asp:CustomValidator ID="vldSeikyuJokyo" runat="server" ValidationGroup="2" CssClass="errortext" Display="Dynamic" ValidateEmptyText="True"></asp:CustomValidator>
                                            </asp:Panel>
                                        </div>
                                </asp:Panel>
                            </td>
                         </tr>
                    </table>                 
                </td>
            </tr>
            <tr>
                <td style="width: 10%">&nbsp;</td>
                <td colspan="2"   class="align-top">
                    <asp:Panel ID="Panel5" runat="server" CssClass="float-left">
                        <uc:ClsCMDateBox runat="server" ID="txtJyuryoDate_IO" ppName="受領日" ppNameWidth="100" ppDateFormat="年月日" ppValidationGroup="2"/>
                    </asp:Panel>
                    <asp:Panel ID="Panel6" runat="server" CssClass="float-left">
                        <uc:ClsCMTimeBox runat="server" ID="txtJyuryoTime_IO" ppNameVisible="False" ppName="受領日(時分)" ppValidationGroup="2"/>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 10%">&nbsp;</td>
                <td colspan="3"   class="align-top">
                    <table border="0" class="float-left">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_UpdaeName" runat="server" Text="アップロード" Width="100"></asp:Label>
                            </td>
                            <td>  
                                <asp:Panel ID="pnlData" runat="server">                      
                                    <asp:FileUpload ID="fupKozisiryo" runat="server" width ="500" BackColor="White" EnableTheming="True"/><br>
                                    <div style="white-space: nowrap" class="align-top">
                                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                            <asp:CustomValidator ID="valfileUpload" runat="server" ValidationGroup="1" CssClass="errortext" Display="Dynamic" ValidateEmptyText="True"></asp:CustomValidator>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                            </td>
                         </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 10%">&nbsp;</td>
                <td colspan="3">
                    <uc:ClsCMTextBox runat="server" ID="txtBiko_IO" ppName="備考"  ppNameWidth="100" ppWidth="700" ppCheckLength="False" ppValidationGroup="2" ppMaxLength="50" />
                </td>
            </tr>
        </table>
        <table class="float-right">
        <tr>
            <td style="width: 100%">&nbsp;</td>
            <td>
                <asp:Button ID="btnClear" runat="server" Text="クリア" />
            </td>
            <td>
                <asp:Button ID="btnBack" runat="server" Text="元に戻す" />
            </td>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="更新" />
            </td>
            <td>
                <asp:Button ID="btnInsert" runat="server" Text="追加" />
            </td>
        </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="2" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
