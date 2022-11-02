<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="REPUPDP002.aspx.vb" Inherits="SPC.REPUPDP002"  MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <style type="text/css">
    <!--
    .active {ime-mode: active;}
    .disabled {ime-mode: disabled;}
        .auto-style1 {
            width: 10px;
        }
    -->
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
        <asp:Panel ID="pnlRepair" runat="server" >
    　　　　　　<table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :35px"></td>
                         <td style="width: 256px">
                             <asp:Label ID="lblRepairRequest" runat="server" Text="修理依頼書" Font-Bold="True"></asp:Label>
                         </td>
                         <td>
                         </td>
                     </tr>
                </table>
                <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td class="align-top" Style="width :72px"></td>
                         <td class="align-top" Style= "padding-top :8px; width :110px">
                            <asp:Label ID="lblRepair" runat="server" Width="70px" Text="管理番号"></asp:Label>
                             
                         </td>
                         <td class="align-top" Style= "padding-top :8px; width :100px">
                             <asp:Label ID="lblRepair_No" runat="server" Width="99px" ></asp:Label>
                         </td>
                         <td class="align-top" Style="width :226px">
                             <uc:ClsCMTextBox ID="txtBranch" runat="server" ppNum="True" ppValidationGroup="1" ppWidth="20" ppNameVisible="False" ppMaxLength="2" ppIMEMode="半角_変更不可" ppRequiredField="True" ppName="枝番" ppCheckHan="True" />
                         </td>
                        <td class="align-top" style= "width: 215px">
                            <uc:ClsCMDateBox ID="dtbTroubleDt" runat="server" ppName="故障発生日" ppNameWidth="102" ppRequiredField="False" ppValidationGroup="1" />
                        </td>
                         <td></td>
                    </tr>
                 </table>
    　　　　　　 <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :70px"></td>
                         <td class="align-top" style="width: 448px">
                             <uc:ClsCMTextBox ID="txtTboxid" runat="server" ppName="ＴＢＯＸＩＤ" ppNameWidth="108" ppWidth="100" ppRequiredField="True" ppMaxLength="8" ppIMEMode="半角_変更不可" ppCheckLength="True" ppValidationGroup="1" ppCheckHan="True" />
                         </td>
                         <td class="align-top" style="padding-top :5px; width: 107px">
                             <asp:Label ID="Label6" runat="server" Width="70px" Text="システム" ></asp:Label>
                         </td>
                         <td class="align-top" style="padding-top :5px; width: 96px">
                              <asp:Label ID="lblSystem" runat="server" Width="70px"></asp:Label>
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :74px"></td>
                         <td class="align-top" style= "padding-top :3px; width: 103px">
                             <asp:Label ID="Label5" runat="server" Text="製品名" Width="100px" Height="16px"  ></asp:Label>
                         </td>
                         <td class="align-top" style= "padding-left :4px">
                             <asp:Panel ID="Panel11" runat="server">
                                <asp:DropDownList ID="ddlProductNm" runat="server" Width="150px" ValidationGroup="1">
                             </asp:DropDownList>
                                <div style="white-space: nowrap">
                                   <asp:Panel ID="Panel12" runat="server" Width="0px">
                                       <asp:CustomValidator ID="cuvProductNm" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlProductNm" ValidationGroup="1"></asp:CustomValidator>
                                   </asp:Panel>
                                </div>
                             </asp:Panel>
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :70px"></td>
                         <td class="align-top" style="width: 256px">
                             <uc:ClsCMTextBox ID="txtHallNm" runat="server" ppName="ホール名" ppNameWidth="108" ppWidth="800"  ppRequiredField="True" ppMaxLength="50" ppValidationGroup="1" ppIMEMode="全角" />
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;"class="center">
                     <tr>
                         <td style="width :70px"></td>
                         <td class="align-top" Style="width :190px">
                             <uc:ClsCMTextBox ID="txtOldVer" runat="server" ppName="ＶＥＲ" ppNameWidth="108" ppWidth="50" ppMaxLength="5"  ppIMEMode="半角_変更不可" ppCheckLength="True" ppValidationGroup="1" ppCheckHan="True" />
                         </td>
                         <td class="align-top" Style="padding-top :7px; padding-left: 5px;width :30px">
                             <asp:Label ID="Label3" runat="server" Text="⇒" Width="16px" Height="16px"></asp:Label>
                         </td>
                         <td class="align-top" style="width :215px">
                             <uc:ClsCMTextBox ID="txtNewVer" runat="server" ppName="VER新" ppNameWidth="1" ppWidth="50" ppRequiredField="False" ppMaxLength="5" ppIMEMode="半角_変更不可" ppNameVisible="False" ppValidationGroup="1" ppCheckLength="True" ppCheckHan="True" />
                         </td>
                        <td class="align-top" style="width: 246px">
                             <uc:ClsCMTextBox ID="txtSerial" runat="server" ppName="故障機シリアル" ppNameWidth="101" ppWidth="130" ppRequiredField="True" ppMaxLength="16" ppIMEMode="半角_変更不可" ppValidationGroup="1" ppCheckHan="True" />
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :73px"></td>
                         <td class="align-top" style= "padding-top :3px; width: 80px">
                             <asp:Label ID="Label1" runat="server" Text="障害内容" Width="105px"  ></asp:Label>
                         </td>
                         <td class="align-top" style= "padding-left :4px">
                             <asp:Panel ID="Panel7" runat="server">
                                 <asp:DropDownList ID="ddlTrouble" runat="server" Width="400" ValidationGroup="1">
                                 </asp:DropDownList>
                                     <div style="white-space: nowrap">
                                         <asp:Panel ID="Panel8" runat="server" Width="0px">
                                             <asp:CustomValidator ID="cuvTrouble" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"  ControlToValidate="ddlTrouble" ValidationGroup="1"></asp:CustomValidator>
                                         </asp:Panel>
                                     </div>
                             </asp:Panel>
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :73px"></td>
                         <td style= "padding-top :3px;width :105px">
                         </td>
                         <td style= "padding-left :4px">
                             <asp:Panel ID="Panel1" runat="server">
                                 <asp:TextBox ID="txtTrouble" runat="server" Height="26px" TextMode="MultiLine" Width="800px" CssClass="active" ></asp:TextBox>
                                     <div style="white-space: nowrap">
                                         <asp:Panel ID="Panel2" runat="server" Width="0px">
                                             <asp:CustomValidator ID="cuvTrouble_t" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"  ControlToValidate="txtTrouble" ValidationGroup="1"></asp:CustomValidator>
                                         </asp:Panel>
                                     </div>
                             </asp:Panel>
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :73px"></td>
                         <td style= "padding-top :3px; width: 80px">
                             <asp:Label ID="Label4" runat="server" Text="完了品の送付先" Width="105px" ></asp:Label>
                         </td>
                         <td style= "padding-left :4px">
                             <asp:Panel ID="Panel3" runat="server">
                                 <asp:DropDownList ID="ddlTraderCd" runat="server" Width ="350" AutoPostBack="True" ValidationGroup="1">
                                 </asp:DropDownList>
                                     <div style="white-space: nowrap">
                                         <asp:Panel ID="Panel4" runat="server" Width="0px">
                                             <asp:CustomValidator ID="cuvTraderCd" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlTraderCd" ValidationGroup="1"></asp:CustomValidator>
                                         </asp:Panel>
                                     </div>
                             </asp:Panel>
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :72px"></td>
                         <td style= "padding-left :3px; width: 89px">
                             <asp:Label ID="lblZipNoNm" runat="server" Text="郵便番号" Width="108px" ></asp:Label>
                         </td>
                         <td>
                             <asp:TextBox ID="txtZipNo" runat="server" MaxLength="8" ValidationGroup="1" CssClass="disabled"></asp:TextBox>
                         </td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :72px"></td>
                         <td style= "padding-left :3px; width: 89px">
                             <asp:Label ID="lblAddrNm" runat="server" Text="住所" Width="108px" ></asp:Label>
                         </td>
                         <td>
                             <asp:TextBox ID="txtAddr" runat="server" Width="720px" MaxLength="50" ValidationGroup="1" CssClass="active"></asp:TextBox>
                         </td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :72px"></td>
                         <td style= "padding-left :3px; width: 89px">
                             <asp:Label ID="lblTelNo" runat="server" Text="ＴＥＬ" Width="108px" ></asp:Label>
                         </td>
                        <td Style="width :100px">
                             <asp:TextBox ID="txtTel" runat="server"  Width="230px" MaxLength="15" ValidationGroup="1" CssClass="disabled"></asp:TextBox>
                         </td>
                         <td style="width: 73px; height: 44px;"></td>
                         <td style= "padding-top :3px; width: 89px">
                             <asp:Label ID="lblChargeNm" runat="server" Text="担当者" Width="109px" ></asp:Label>
                         </td>
                         <td>
                             <asp:TextBox ID="txtCharge" runat="server"  MaxLength="20" Width="280px" ValidationGroup="1" CssClass="active"></asp:TextBox>
                         </td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :35px"></td>
                         <td style="width: 256px">
                             <asp:Label ID="lblNotice" runat="server" Text="通知完了登録" Font-Bold="True"></asp:Label>
                         </td>
                         <td></td>
                     </tr>
                  </table>
        </asp:Panel>
        <asp:Panel ID="pnlComplete" runat="server">
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :73px"></td>
                         <td style= "padding-top :3px; width: 80px">
                             <asp:Label ID="Label2" runat="server" Text="会社名" Width="105px" ></asp:Label>
                         </td>
                         <td style= "padding-left :4px">
                             <asp:Panel ID="Panel9" runat="server">
                                 <asp:DropDownList ID="ddlCompNm" runat="server" Width="300" AutoPostBack="True" ValidationGroup="1" >
                                 </asp:DropDownList>
                                     <div style="white-space: nowrap">
                                         <asp:Panel ID="Panel10" runat="server" Width="0px">
                                             <asp:CustomValidator ID="cuvCompNm" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlCompNm" ValidationGroup="1"></asp:CustomValidator>
                                         </asp:Panel>
                                     </div>
                             </asp:Panel>
                         </td>
                         <td></td>
                     </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :72px"></td>
                         <td Style="width :70px">
                             <asp:Label ID="lblRepairContent" runat="server" Text="故障原因及び修理内容" Width="88px" ></asp:Label>
                         </td>
                         <td Style="width :14px"></td>
                         <td style= "padding-left:4px; ">
                             <asp:Panel ID="Panel13" runat="server">
                                 <asp:TextBox ID="txtRepairContent" runat="server" Height="26px" TextMode="MultiLine" Width="800px" CssClass="active"></asp:TextBox>
                                     <div style="white-space: nowrap">
                                         <asp:Panel ID="Panel14" runat="server" Width="0px">
                                             <asp:CustomValidator ID="cuvRepairContent" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtRepairContent"></asp:CustomValidator>
                                         </asp:Panel>
                                     </div>
                             </asp:Panel>
                         </td>
                      </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr style=" height:40px">
                         <td Style="width :70px"></td>
                         <td>
                             <uc:ClsCMTextBox ID="txtRepairCharge" runat="server" ppName="修理責任者" ppNameWidth="109" ppWidth="300" ppMaxLength="20" ppIMEMode="全角" />
                         </tr>
                  </table>
                  <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :70px"></td>
                         <td class="align-top" style="width: 256px">
                             <uc:ClsCMDateBox ID="dtbAppasendDt" runat="server" ppName="機器発送日" ppNameWidth="109" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtVer1" runat="server" ppName="ＶＥＲ" ppNameWidth="50" ppWidth="50" ppMaxLength="5" ppIMEMode="半角_変更不可" ppCheckLength="True" ppCheckHan="True" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtHddNo1" runat="server" ppName="ＨＤＤＮo" ppNameWidth="65" ppWidth="20" ppMaxLength="2" ppIMEMode="半角_変更不可" ppCheckLength="False" ppCheckHan="True"/>
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtHddCls1" runat="server" ppName="ＨＤＤ種別" ppNameWidth="70" ppWidth="50" ppMaxLength="4" ppCheckHan="False" ppIMEMode="全角"/>
                         </td>
<%--                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="ClsCMTextBox1" runat="server" ppName="ＨＤＤＮo" ppNameWidth="65" ppWidth="20" ppMaxLength="1" ppIMEMode="半角_変更不可" ppCheckLength="True" ppNum="True" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="ClsCMTextBox2" runat="server" ppName="ＨＤＤ種別" ppNameWidth="70" ppWidth="50" ppMaxLength="4" ppIMEMode="半角_変更不可" />
                         </td>--%>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :70px"></td>
                         <td style="width: 256px">
                             <uc:ClsCMDateBox ID="dtbAppaarvDt" runat="server" ppName="機器到着日" ppNameWidth="109" />
                         </td>
                         <td></td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td style= "width: 63px">
                         <td style= "width: 70px">
                             <asp:Label ID="lblWrkNo11Nm" runat="server" Text="作業項番１" Width="70px"></asp:Label>
                         </td>
                         <td style="width: 15px">
                             <asp:Label ID="lblWrkNo11" runat="server" Text="①" Width="20px" ></asp:Label>
                         </td>
                         <td style="width: 256px">
                             <asp:DropDownList ID="ddlWrkNo11" runat="server" Width ="350" ></asp:DropDownList>
                         </td>
                         <td style= "padding-left :10px; width: 20px">
                             <asp:Label ID="lblWrkNo12" runat="server" Text="②" Width="20px" ></asp:Label>
                         </td>
                         <td style="width: 256px">
                             <asp:DropDownList ID="ddlWrkNo12" runat="server" Width ="350" ></asp:DropDownList>
                         </td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td style= "width: 63px">
                         <td style= "width: 70px">
                             <asp:Label ID="Label18" runat="server" Text="" Width="70px" ></asp:Label>
                         </td>
                         <td style="width: 15px">
                             <asp:Label ID="lblWrkNo13" runat="server" Text="③" Width="20px" ></asp:Label>
                         </td>
                         <td style="width: 256px">
                             <asp:DropDownList ID="ddlWrkNo13" runat="server" Width ="350" ></asp:DropDownList>
                         </td>
                         <td style= "padding-left :10px; width: 20px">
                             <asp:Label ID="lblWrkNo14" runat="server" Text="④" Width="20px" ></asp:Label>
                         </td>
                         <td style="width: 256px">
                             <asp:DropDownList ID="ddlWrkNo14" runat="server" Width ="350" ></asp:DropDownList>
                         </td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :73px"></td>
                         <td style= "width: 75px">
                             <asp:Label ID="lblWrkNo21" runat="server" Text="作業項番２" Width="109px" ></asp:Label>
                         </td>
                         <td style= "width: 854px">
                             <asp:DropDownList ID="ddlWrkNo21" runat="server" Width ="350" ></asp:DropDownList>
                         </td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :73px"></td>
                         <td style= "width: 75px">
                             <asp:Label ID="lblPartsNo1" runat="server" Text="部品項番１" Width="109px" ></asp:Label>
                         </td>
                         <td style= "width: 854px">
                             <asp:DropDownList ID="ddlPartsNo1" runat="server" Width ="350" ></asp:DropDownList>
                         </td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td style= "width: 200px"></td>
                         <td class="align-top" style= "width: 308px">
                            <uc:ClsCMDropDownList runat="server" ID="ddlTmpResult" ppName="一時診断結果" ppNameWidth="110" ppWidth="100" ppClassCD="0021" ppNotSelect="True" />
                         </td>
                         <td style="width: 250px"></td>
                         <td class="align-top" style="width: 599px">
                             <uc:ClsCMDateBox ID="dtbRsltSndDt" runat="server" ppName="診断結果送付日" />
                         </td>
                         <td style="width: 599px"></td>
                         <td style= "width: 200px"></td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :73px"></td>
                         <td style= "width: 75px">
                             <asp:Label ID="lblCmprtnCd" runat="server" Text="完了返却先" Width="109px" ></asp:Label>
                         </td>
                         <td style= "width: 854px">
                             <asp:DropDownList ID="ddlCmprtnCd" runat="server" Width ="350"></asp:DropDownList>
                         </td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :70px"></td>
                         <td class="align-top" style="width: 256px">
                             <uc:ClsCMDateBox ID="dtbCmpSndDt" runat="server" ppName="完了発送日" ppNameWidth="109" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtVer2" runat="server" ppName="ＶＥＲ" ppNameWidth="50" ppWidth="50" ppMaxLength="5" ppIMEMode="半角_変更不可" ppCheckLength="True" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtHddNo2" runat="server" ppName="ＨＤＤＮo" ppNameWidth="65" ppWidth="20" ppMaxLength="2" ppIMEMode="半角_変更不可" ppCheckLength="False" ppCheckHan="True" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtHddCls2" runat="server" ppName="ＨＤＤ種別" ppNameWidth="70" ppWidth="50" ppMaxLength="4" ppIMEMode="全角" />
                         </td>
<%--                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="ClsCMTextBox1" runat="server" ppName="ＨＤＤＮo" ppNameWidth="65" ppWidth="20" ppMaxLength="1" ppIMEMode="半角_変更不可" ppCheckLength="True" ppNum="True" />
                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="ClsCMTextBox2" runat="server" ppName="ＨＤＤ種別" ppNameWidth="70" ppWidth="50" ppMaxLength="4" ppIMEMode="半角_変更不可" />
                         </td>--%>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :70px"></td>
                         <td class="align-top" style= "width: 320px">
                                  <uc:ClsCMDateBox ID="dtbArrivalDt" runat="server" ppName="受領日" ppNameWidth="109" />
                        </td>
                         <td class="auto-style1"></td>
                         <td class="align-top" style="width: 300px" >
                                  <uc:ClsCMTextBox ID="txtNewSerial" runat="server" ppName="新シリアル" ppNameWidth="87" ppWidth="150" ppMaxLength="16" ppIMEMode="半角_変更不可" />
                         </td>
                         <td class="align-top" style="width: 60px">
                            <asp:Label ID="Label7" runat="server" Text="運用区分" Width="60px"  ></asp:Label>
                         </td>
                         <td class="align-top" style="width: 300px">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="Panel15" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                         </td>                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td style= "width: 68px"></td>
                         <td class="align-top" style= "width: 300px">
                            <uc:ClsCMDropDownList runat="server" ID="ddlInsResult" ppName="検品結果" ppNameWidth="109" ppWidth="100" ppClassCD="0022" ppNotSelect="True" />
                         </td>
                         <td style="width: 10px"></td>
                         <td class="align-top" style="width: 300px">
                                  <uc:ClsCMTextBox ID="txtSubNo" runat="server" ppName="代替製造番号" ppNameWidth="89" ppWidth="150" ppMaxLength="20" ppIMEMode="半角_変更不可" ppCheckHan="True" />
                         </td>
                         <td class="align-top" style="width: 300px">
                             <uc:ClsCMDropDownList ID="ddlTokCls" runat="server" ppClassCD="0073"  ppName="テスト区分" ppWidth="100" ppNotSelect="True" />
                         </td>
                         <td style= "width: 10px"></td>
                     </tr>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td style="width: 72px"></td>
                         <td class="align-top" style="padding-top :5px; width: 110px">
                             <asp:Label ID="lblStatusCd" runat="server" Text="ステータス" Width="70px"  ></asp:Label>
                         </td>
                         <td class="align-top" Style= "padding-top :3px; width: 194px">
                             <asp:DropDownList ID="ddlStatusCd" runat="server" Width ="100" ></asp:DropDownList>
                         </td>
                         <td style="width: 12px"></td>
                         <td class="align-top" style="width: 397px">
                             <uc:ClsCMTextBox  ID="txtReqDt" runat="server" ppName="請求年月" ppNameWidth ="88" ppIMEMode="半角_変更不可"  ppWidth="55" ppMaxLength="6" ppCheckHan="True"/>
                         </td>
                         <td></td>
                     </tr>
                   </table>
 
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td Style="width :35px"></td>
                         <td style="width: 256px">
                             <asp:Label ID="lblParts" runat="server" Text="交換部品" Font-Bold="True"></asp:Label>
                         </td>
                         <td>
                         </td>
                     </tr>
                  </table>

            <asp:Panel ID="pnlRegister" runat="server"  BorderStyle="Solid" BorderWidth="1px" Width="1050px" CssClass="center">

                   <table border="0" style="width:1050px; height: 60px;" class="center">
                     <tr>
                         <td Style="width :72px"></td>
                         <td class="align-top" style= "padding-top :8px; width: 95px">
                             <asp:Label ID="lblPartsNm" runat="server" Text="部品名" Width="107px"  ></asp:Label>
                         </td>
                         <td class="align-top" style= "padding-left :4px ;padding-top :4px;width: 200px">
                             <asp:Panel ID="Panel5" runat="server">
                                 <asp:DropDownList ID="ddlPartsNm" runat="server" Width="350px"  ValidationGroup="2"></asp:DropDownList>
                                     <div style="white-space: nowrap">
                                         <asp:Panel ID="Panel6" runat="server" Width="0px">
                                             <asp:CustomValidator ID="cuvPartsNm" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2" ControlToValidate="ddlPartsNm"></asp:CustomValidator>
                                         </asp:Panel>
                                     </div>
                             </asp:Panel>
                         </td>
                         <td>

                         </td>
                         <td class="align-top" >
                             <uc:ClsCMTextBox ID="txtQuantity" runat="server" ppName="個数" ppWidth="50"  ppValidationGroup="2" ppMaxLength="3" ppIMEMode="半角_変更不可" ppRequiredField="True" ppNum="True" ppCheckHan="True" />
                         </td>
                   </table>
                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td>
                             <div class="float-right">
                          <%--       <asp:Button ID="btnSearchRigth5" runat="server" Text="Button" Visible="False" />
                                 <asp:Button ID="btnSearchRigth4" runat="server" Text="Button" Visible="False" />
                                 <asp:Button ID="btnSearchRigth3" runat="server" Text="Button" Visible="False" />--%>
                                 <asp:Button ID="btnClear" runat="server" Text="クリア"  />
                                 <asp:Button ID="btnInsert" runat="server" Text="追加" ValidationGroup="2" Height="21px" Width="62px"  />
                                 <asp:Button ID="btnChange" runat="server" Text="変更"  ValidationGroup="2" Width="62px" Enabled="False" />
                                 <asp:Button ID="btnDelete" runat="server" Text="削除"  ValidationGroup="2" Width="62px" Enabled="False" />
                             </div>
                         </td>
                     </tr>
                   </table>

            </asp:Panel>

                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td style="width: 1050px">
                             <div id="DivOut" runat="server" class="grid-out">
                                 <div id="DivIn" runat="server" style="overflow: auto;height: 260px;">
                                     <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                                     <asp:GridView ID="grvList" runat="server" >
                                     </asp:GridView>
                                 </div>
                             </div>
                         </td>
                         <td></td>
                     </tr>
                     <tr>
                         <td class ="align-top" style="width:500px">
                             <div class="align-top">
                                 <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
                                 <asp:ValidationSummary ID="vasPartsSummary" runat="server" CssClass="errortext" ValidationGroup="2" />
                             </div>
                         </td>
                     </tr>

                   </table>
        </asp:Panel>
<%--                   <table border="0" style="width:1050px;" class="center">
                     <tr>
                         <td>
                             <div class="float-left">
                                 <%--<asp:Button ID="btnPrint" runat="server" Text="印刷" Width="62px"  />--%>
<%--                             </div>
                         </td>
                         <td>
                             <div class="float-right">
                                 <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                                 <asp:Button ID="Button2" runat="server" Text="Button" Visible="False" />
                                 <asp:Button ID="Button3" runat="server" Text="Button" Visible="False" />--%>
                                 <%--<asp:Button ID="btnAllClear" runat="server" Text="クリア"  />--%>
                                 <%--<asp:Button ID="btnUpdate" runat="server" Text="登録"  ValidationGroup="1" Height="21px" Width="62px" />--%>
 <%--                            </div>
                         </td>
                     </tr>
                  </table>--%>
</asp:Content>
