<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="DLCLSTP001.aspx.vb" Inherits="SPC.DLCLSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <%--DLCLSTP001-001--%>
    <script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/EnableChange.js")%>'></script>
        <script type="text/javascript">
            function lenCheck(obj, size) {
                var strW = obj.value;
                var lenW = strW.length;
                var num

                num = obj.value.match(/\n|\r\n/g);
                if (num != null) {
                    gyosuu = num.length;
                } else {
                    gyosuu = 0;
                }

                if ((parseInt(size) + parseInt(gyosuu)) < lenW) {
                    var limitS = strW.substring(0, (parseInt(size) + parseInt(gyosuu)));
                    obj.value = limitS;
                }
            }
    </script>
    <%--DLCLSTP001-001 END--%>
    <asp:Panel ID="pnlSearch" runat="server" Height="135px" >
        <table style="width: 1050px;padding:0px;" class="center">
            <tr style="height:5px;">
                <td style="width: 100px">
                <td class="align-top" style="width: 200px">
                    <uc:ClsCMTextBoxFromTo runat="server" ID="txtTboxID" ppName="ＴＢＯＸＩＤ" ppMaxLength="8" ppCheckHan="true" ppCheckLength="False" ppRequiredField="False" ppWidth="80" ppIMEMode="半角_変更不可" />
                </td>
                <td style="width: 20px">
                <td class="align-top" style="width: 200px" colspan="2">
                    <div style="text-align:left">
                        <table style="padding:0px;">
                            <tr>
                                <td class="center">
                                    <asp:Label ID="lblCenterCls_search" runat="server" Text="NL区分"></asp:Label>
                                </td>
                                <td style="width: 5px">
                                <td class="align-top">
                                    <asp:DropDownList ID="ddlCenterCls" runat="server" Width="115px"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="width: 20px">
                <td class="align-top" style="width: 250px">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="center">
                                    <asp:Label ID="lblSystem_search" runat="server" Text="システム"></asp:Label>
                                </td>
                                <td style="width: 5px">
                                <td class="align-top">
                                    <asp:DropDownList ID="ddlSystem" runat="server" Width="150px"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr style="height:5px;">
                <td>&nbsp;</td>
                <td colspan="3">
                    <uc:ClsCMTextBox runat="server" ID="txtHallName" ppName="ホール名"  ppWidth="300px" ppNameWidth="78px" ppMaxLength="50" ppIMEMode="全角" />
                </td>
                <td>
                </td>
                <td>
                </td>
                <td colspan="2">
                    <table style="height:20px;">
                        <tr>
                            <td class="align-top">
                                <uc:ClsCMDateBox runat="server" ID="txtInsDay_From" ppNameVisible="true" ppName="登録日時" ppRequiredField="False" ppNameWidth="60px"/>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMTimeBox runat="server" ID="txtInsTim_From" ppNameVisible="false" ppName="登録日時" ppRequiredField="False" />
                            </td>
                            <td class="center">
                                <asp:Label ID="Label3" runat="server" Text="～"></asp:Label>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMDateBox runat="server" ID="txtInsDay_To" ppNameVisible="false" ppName="登録日時" ppRequiredField="False"/>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMTimeBox runat="server" ID="txtInsTim_To" ppNameVisible="false" ppName="登録日時" ppRequiredField="False" />
                            </td>
                        </tr>                          
                    </table>                                        
                </td>
            </tr>
            <tr style="height:5px;">
<%--                <td style="height: 30px"></td>--%>
                <td></td>
<%--                <td colspan="3" style="height: 30px">--%>
                <td colspan="4">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="center">
                                    <asp:Label ID="lblIrainaiyo" runat="server" Text="設定依頼内容"></asp:Label>
                                </td>
                                <td class="align-top">
                                    <asp:DropDownList ID="ddlSetteiirai" runat="server" Width="400px"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td>
                </td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td class="align-top">
                                <uc:ClsCMDateBox runat="server" ID="txtUpdDay_From" ppNameVisible="true" ppName="更新日時" ppRequiredField="False" ppNameWidth="60px"/>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMTimeBox runat="server" ID="txtUpdTim_From" ppNameVisible="false" ppName="更新日時" ppRequiredField="False" />
                            </td>
                            <td class="center">
                                <asp:Label ID="Label4" runat="server" Text="～"></asp:Label>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMDateBox runat="server" ID="txtUpdDay_To" ppNameVisible="false" ppName="更新日時" ppRequiredField="False"/>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMTimeBox runat="server" ID="txtUpdTim_To" ppNameVisible="false" ppName="更新日時" ppRequiredField="False" />
                            </td>
                        </tr>                          
                    </table>                                        
                </td>
            </tr>
            <tr style="height:5px;">
                <td>&nbsp;</td>
                <td colspan="5">
                    <div style="text-align:left">
                        <table class="align-top" style="padding:0px;">
                            <tr>
                               <%-- <td class="align-top"　 style="height: 49px">--%>
                                <td class="align-top">
                                    <uc:ClsCMDateBox runat="server" ID="txtSetteiDayFrom" ppNameVisible="true" ppName="設定日時" ppRequiredField="False" ppNameWidth="76px"/>
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMTimeBox runat="server" ID="txtSetteiTimeFrom" ppNameVisible="false" ppName="設定日時" ppRequiredField="False" />
                                </td>
                                <td class="center">
                                    <asp:Label ID="lblbar" runat="server" Text="～"></asp:Label>
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMDateBox runat="server" ID="txtSetteiDayTo" ppNameVisible="false" ppName="設定日時" ppRequiredField="False"/>
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMTimeBox runat="server" ID="txtSetteiTimeTo" ppNameVisible="false" ppName="設定日時" ppRequiredField="False" />
                                </td>
                            </tr>                          
                        </table>                                        
                    </div>
                </td>
                <td>
                    進捗状況　<asp:DropDownList ID="ddlDutyCD" runat="server" Width="85px" ></asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
            <tr style="height:5px;">
                <td>&nbsp;</td>
                <td colspan="5">
                    <div style="text-align:left">
                        <table class="align-top">
                            <tr>
                                <td class="align-top">
                                    <uc:ClsCMDateBox runat="server" ID="txtReturnDayFrom" ppNameVisible="true" ppName="戻し日時" ppRequiredField="False" ppNameWidth="76px"/>
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMTimeBox runat="server" ID="txtReturnTimeFrom" ppNameVisible="false" ppName="戻し日時" ppRequiredField="False"/>
                                </td>
                                <td class="center">
                                    <asp:Label ID="Label1" runat="server" Text="～"></asp:Label>
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMDateBox runat="server" ID="txtReturnDayTO" ppNameVisible="false" ppName="戻し日時" ppRequiredField="False" />
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMTimeBox runat="server" ID="txtReturnTimeTO" ppNameVisible="false" ppName="戻し日時" ppRequiredField="False"/>
                                </td>
                            </tr>                          
                        </table>                                        
                    </div>
                </td>
                <td>
                    リトライ　<asp:DropDownList ID="ddlRetrySel" runat="server" Width="150px" ></asp:DropDownList>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
    <asp:Panel ID="pnlUpdate" runat="server"  BackColor="#FFCCFF" BorderStyle="Solid" BorderWidth="1px">
        <table style="width: 1050px;padding:0px;" class="center">
            <tr>
                <td style="width: 100px"></td> 
                <td class="align-top" style="width: 150px">
                    <uc:ClsCMTextBox runat="server" ID="txtTboxID_Input" ppName="ＴＢＯＸＩＤ" ppMaxLength="8" ppCheckHan="true" ppCheckLength="true" ppRequiredField="true" ppValidationGroup="2" ppIMEMode="半角_変更不可" />
                </td>
                <td style="width: 20px">
                <td style="width: 150px">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblCenterCls" runat="server" Text="NL区分"></asp:Label>
                                </td>
                                <td style="width: 10px">
                                <td class="align-top">
                                    <asp:Label ID="lblCenterCls_Input" runat="server" Text="ＮＮＮ"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="width:150px; ">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblSystem" runat="server" Text="システム"></asp:Label>
                                </td>
                                <td style="width: 10px">
                                <td class="align-top">
                                    <asp:Label ID="lblSystem_Input" runat="server" Text="ＮＮＮＮＮＮＮ"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td colspan="3">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblHallName" runat="server" Text="ホール名"></asp:Label>
                                </td>
                                <td style="width: 10px">
                                <td class="align-top">
                                    <asp:Label ID="lblHallName_Input" runat="server" Text="ＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮ"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
<%--            <tr>
                <td>&nbsp;</td>
                <td colspan="4">
                    <div style="text-align:left">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblHallName" runat="server" Text="ホール名"></asp:Label>
                                </td>
                                <td style="width: 20px">
                                <td class="align-top">
                                    <asp:Label ID="lblHallName_Input" runat="server" Text="ＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮＮ"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>--%>
            <tr>
                <td>&nbsp;</td>
                <td colspan="4" >
                    <div style="text-align:left">
                        <table style="padding:0px">
                            <tr>
                                <td class="center">
                                    <asp:Label ID="lblSettei_input" runat="server" Text="設定依頼内容"></asp:Label>
                                </td>
                                <td class="align-top">
                                    <asp:DropDownList ID="ddlSettei_input" runat="server" Width="400px"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td colspan="3">
                     <div style="text-align:left">
                        <table style="padding:0px">
                            <tr>
                                <td class="align-top">
                                    <uc:ClsCMTextBox runat="server" ID="txtIraisya" ppWidth="200" ppName="設定依頼者" ppMaxLength="10" ppValidationGroup="2" ppIMEMode="全角" />
                                </td>
<%--                                <td class="center">
                                    <asp:Label ID="lblSeqNo" runat="server" Text="XX" Visible="False"></asp:Label>
                                </td>--%>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="4" >
                    <div style="text-align:left">
                        <table style="padding:0px">
                            <tr>
                                <td class="align-top">
                                    <uc:ClsCMDateBox runat="server" ID="txtStteiDay_input" ppNameVisible="true" ppName="設定日時" ppNameWidth="76px"  ppValidationGroup="2" />
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMTimeBox runat="server" ID="txtStteiTime_input" ppNameVisible="false" ppName="設定日時" ppValidationGroup="2" />
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMDateBox runat="server" ID="txtReturnDay_Input" ppNameVisible="true" ppName="戻し日時" ppNameWidth="76px" ppValidationGroup="2" />
                                </td>
                                <td class="align-top">
                                    <uc:ClsCMTimeBox runat="server" ID="txtReturnTime_Input" ppNameVisible="false" ppName="戻し日時" ppValidationGroup="2" />
                                </td>
                            </tr>               
                        </table>
                    </div>
                </td>
                <td colspan="3">
                    <%--DLCLSTP001-001--%>
                    <uc:ClsCMTextBox ID="txtNotes" runat="server" ppName="備考" ppHeight="30" ppNameWidth="68" ppWidth="410" ppTextMode="MultiLine" ppMaxLength="50"  ppValidationGroup="2" ppNameVisible="true" ppWrap="true" ppIMEMode="全角" />
                    <%--DLCLSTP001-001 END--%>
                </td>
            </tr>
            <tr>
                <td style="height: 44px"></td>
                <td style="height: 44px">
                    <asp:CheckBoxList ID="ChkSeisanki" runat="server" RepeatDirection="Horizontal" TextAlign="Left" ValidationGroup="2">
                        <asp:ListItem Value="1">精算機変更</asp:ListItem>
                        <asp:ListItem Value="2">精算機戻し</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <td colspan="5" style="width:200px; height: 44px;">
                    <table style="width:200px">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label2" runat="server" Text="会員サービスコード"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 44px">
                                <uc:ClsCMTextBox runat="server" ID="txtSrvCd1" ppWidth="100" ppName="1:" ppMaxLength="8" ppValidationGroup="2" ppIMEMode="半角_変更不可" />
                            </td>
                            <td style="height: 44px">
                                <uc:ClsCMTextBox runat="server" ID="txtSrvCd2" ppWidth="100" ppName="2:" ppMaxLength="8" ppValidationGroup="2" ppIMEMode="半角_変更不可" />
                            </td>
                        </tr>
<%--                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label2" runat="server" Text="会員サービスコード"></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtSrvCd1" ppWidth="100" ppName="1:" ppMaxLength="8" ppValidationGroup="2" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtSrvCd2" ppWidth="100" ppName="2:" ppMaxLength="8" ppValidationGroup="2" />
                            </td>
                        </tr>--%>
                    </table>
                </td>
                <td style="height: 44px">
                    <table class="float-right">
                        <tr>
                            <td>
                                <asp:Button ID="btnClear" runat="server" Text="クリア" />
                            </td>
                            <td>
                                <asp:Button ID="btnInsert" runat="server" Text="登録" />
                            </td>
                            <td>
                                <asp:Button ID="btnUpdate" runat="server" Text="変更" ValidationGroup="2" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="削除" />
                            </td>
                        </tr>
                    </table>
                </td> 
            </tr>
<%--            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td colspan="4">
                    <table class="float-right">
                        <tr>
                            <td>
                                <asp:Button ID="btnClear" runat="server" Text="クリア" />
                            </td>
                            <td>
                                <asp:Button ID="btnInsert" runat="server" Text="登録" />
                            </td>
                            <td>
                                <asp:Button ID="btnUpdate" runat="server" Text="変更" ValidationGroup="2" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="削除" />
                            </td>
                        </tr>
                    </table>
                <td colspan="2">
                </td>
            </tr>--%>
        </table>
    </asp:Panel>
    <table>
            <tr>
                <td>
                    <div class="float-left">
                        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="2" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">  
    <!--該当件数表示 & リロードボタン-->
    <div ID="divCount" runat="server" class="float-Left">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 40px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX" style="font-size:12pt;"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 15px"></td>
                            <td>
                                <asp:Button ID="btnReload" runat="server" Text="リロード" />
                            </td>
                <td style="width: 15px"></td>
                <td>
                    <asp:Label ID="lblAutoRetry" runat="server" Text="自動リトライ：" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 60px">
                    <div class="float-right">
                        <asp:Label ID="lblAutoRetrySts" runat="server" Text="XXXXX" style="font-size:12pt;" Font-Bold="True"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblUpdate" runat="server" Text="(変更日時：" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 160px">
                    <div class="float-left">
                        <asp:Label ID="lblUpdateDt" runat="server" Text="XXXXX" style="font-size:12pt;" Font-Bold="false"></asp:Label>
                    </div>
                </td>
        </table>
    </div>
    <div id="DivOut" runat="server" class="grid-out" style="height:360px;">
        <div class="grid-in" style="height:360px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
