<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="REQSELP001.aspx.vb" Inherits="SPC.REQSELP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">    
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

    <style type="text/css">
        .auto-style1 {
            width: 202px;
        }
        .auto-style2 {
            width: 50px;
            height: 86px;
        }
        .auto-style4 {
            width: 42px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <table>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td style="width: 350px">
                    <uc:ClsCMTextBox ID="txtTboxId"  runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="120" ppWidth="120" ppNum="True" ppValidationGroup="1" ppRequiredField="True" />
                </td>
                <td style="width: 500px">
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlFileName" runat="server">
                                    <asp:Label ID="lblFileName" runat="server" Text="ＴＢＯＸタイプ" Width="120px"></asp:Label>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlData" runat="server"  CssClass="align-top">
                                    <asp:DropDownList ID="ddlTboxType" runat="server" Width="200" ValidationGroup="1">
                                    </asp:DropDownList>
                                    <div style="white-space: nowrap">
                                        <asp:Panel ID="pnlval" runat="server" Width="0px">
                                            <asp:CustomValidator ID="valddl" runat="server" ControlToValidate="ddlTboxType" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="1"></asp:CustomValidator>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="登録" />
                </td>
            </tr>
            <tr>
                <td  colspan="3">
                    <div class="float-left">
                        <asp:ValidationSummary ID="valsum1" runat="server" CssClass="errortext" ValidationGroup="1"/>
                    </div>
                </td>
            </tr>
        </table>
        <hr>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="100%">
        <table>
            
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTrblnoNm" runat="server" Text="管理番号" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTrblno" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblIrainoNm" runat="server" Text="保守管理番号" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIraino" runat="server" Width="120px"></asp:Label>
                                <asp:Label ID="lblIraino2" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td  class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNlclsNm" runat="server" Text="ＮＬ区分" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNlcls" runat="server" Width="100px"></asp:Label>
                                <asp:HiddenField ID="hdnNlclsCd" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTboxTypeNm" runat="server" Text="ＴＢＯＸタイプ" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTboxType" runat="server" Width="120px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblVerNm" runat="server" Text="ＶＥＲ" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVer" runat="server"></asp:Label>
                                <asp:HiddenField ID="hdnSysCd" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td  class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblEwclsNm" runat="server" Text="ＥＷ区分" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEwcls" runat="server"  Width="100px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnEwclsCd" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td colspan="5">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHall" runat="server" Text="ホール名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHallNm" runat="server" ></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnHallCd" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td colspan="3">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAddrNm" runat="server" Text="住所" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAddr1" runat="server" Width="480px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td style="width:320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTelNm" runat="server" Text="ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTel" runat="server" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<tr>
                <td style="width: 50px">&nbsp;</td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td style="width: 120px">&nbsp;</td>
                            <td>
                                <asp:Label ID="lblAddr2" runat="server" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td style="width: 120px">&nbsp;</td>
                            <td>
                                <asp:Label ID="lblAddr3" runat="server" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
            </tr>--%>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="保担名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblWrkCharge" runat="server"  Width="200px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnEigyoCd" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTokatuChargeNm" runat="server" Text="総括保担名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTokatuCharge" runat="server"  Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnTokatuCd" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNgcOrgNm" runat="server" Text="担当営業部" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNgcOrg" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAgcNm" runat="server" Text="代理店名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAgc" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnAgcCd" />
                                <asp:HiddenField runat="server" ID ="hdnAgcZip" />
                                <asp:HiddenField runat="server" ID ="hdnAgcAddr" />
                                <asp:HiddenField runat="server" ID ="hdnAgcTel" />
                                <asp:HiddenField runat="server" ID ="hdnAgcFax" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblRepNm" runat="server" Text="代行店名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblRep" runat="server" Width="200px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnRepCd" />
                                <asp:HiddenField runat="server" ID ="hdnRepZip" />
                                <asp:HiddenField runat="server" ID ="hdnRepAddr" />
                                <asp:HiddenField runat="server" ID ="hdnRepTel" />
                                <asp:HiddenField runat="server" ID ="hdnRepChg" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrgTelNo" runat="server" Text="営業部ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblOrgTel" runat="server" Width="200px" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTwinCls" runat="server" Text="双子店区分" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTwin" runat="server" Width="200px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnTwinCd" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblEstCls" runat="server" Text="ＭＤＮ設置有無" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEst" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnEstCls" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMdnNm" runat="server" Text="ＭＤＮ機器名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblMdn" runat="server" Width="200px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnMdnCnt" />
                                <asp:HiddenField runat="server" ID ="hdnMdnCd1" />
                                <asp:HiddenField runat="server" ID ="hdnMdnCd2" />
                                <asp:HiddenField runat="server" ID ="hdnMdnCd3" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 50px;">&nbsp;</td>
                <td>
                    <table style="border-style:solid;">
                        <tr>
                            <td style="width:320px;">
                                <uc:ClsCMDropDownList ID="ddlHpnCls" runat="server" ppName="発生区分" ppNameWidth="120px" ppWidth="125" ppClassCD="0036" ppNotSelect="True" ppValidationGroup="2" ppRequiredField="True" />
                            </td>
                            <td style="width:20px;">&nbsp;</td>
                            <td style="width:320px;" colspan="3">
                                <uc:ClsCMDropDownList ID="ddlMngCls" runat="server" ppName="管理区分" ppNameWidth="120px" ppWidth="200" ppClassCD="0037" ppNotSelect="True" ppValidationGroup="2" ppRequiredField="True"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dttRptDt" ppName="申告日" ppNameWidth="120" ppValidationGroup="2" />
                            </td>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="tetRpt" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="申告者" ppNameWidth="120" ppWidth="240" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel10" runat="server">
                                                <asp:Label ID="Label5" runat="server" Text="申告元" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel11" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlRptBase" runat="server" Width="200">
                                                </asp:DropDownList>
                                                <div style="white-space: nowrap">
                                                    <asp:Panel ID="Panel26" runat="server" Width="0px">
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ddlRptBase" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                                    </asp:Panel>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="tetRptTel" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="申告者ＴＥＬ" ppNameWidth="120" ppWidth="240" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMDateTimeBox runat="server" ID="dttRcptDt" ppName="受付日時" ppNameWidth="120" ppValidationGroup="2"  />
                            </td>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="tetRptCharg" runat="server" ppMaxLength="20" ppName="受付者" ppNameWidth="120" ppWidth="240" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel12" runat="server">
                                                <asp:Label ID="Label6" runat="server" Text="申告内容" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 300px;">
                                            <asp:Panel ID="Panel13" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlRptCd" runat="server" Width="750px">
                                                </asp:DropDownList>
                                                <div style="white-space: nowrap">
                                                    <asp:Panel ID="Panel27" runat="server" Width="0px">
                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="ddlRptCd" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                                    </asp:Panel>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width:20px;">&nbsp;</td>
                            <td>
                                <asp:CheckBox ID="chkImpCls" runat="server" Text="故障重要度" />
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="3">
                                <uc:ClsCMTextBox ID="tetRptDtl1" runat="server" ppMaxLength="50" ppName="申告内容詳細" ppNameWidth="120" ppWidth="750px" ppValidationGroup="2" ppNameVisible="True" />
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem>営業支障</asp:ListItem>
                                    <asp:ListItem>２次支障</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="5">
                                <uc:ClsCMTextBox ID="tetRptDtl2" runat="server" ppMaxLength="50" ppName="" ppNameWidth="120" ppWidth="750px" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="5">
                                <uc:ClsCMTextBox ID="tetInhCntnt1" runat="server" ppMaxLength="50" ppName="引継内容" ppNameWidth="120" ppWidth="750px" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td  colspan="5">
                                <uc:ClsCMTextBox ID="tetInhCntnt2" runat="server" ppMaxLength="50" ppName="" ppNameWidth="120" ppWidth="750px" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMDropDownList ID="ddlDealreqCd1" runat="server" ppName="対応依頼１" ppNameWidth="120" ppWidth="130" ppClassCD="0087" ppNotSelect="True" ppValidationGroup="2" />
                            </td>
                            <td>&nbsp;</td>
                            <td class="auto-style1">
                                <uc:ClsCMDropDownList ID="ddlDealreqCd2" runat="server" ppName="対応依頼２" ppNameWidth="120" ppWidth="130" ppClassCD="0087" ppNotSelect="True" ppValidationGroup="2" />
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <uc:ClsCMDropDownList ID="ddlDealreqCd3" runat="server" ppName="対応依頼３" ppNameWidth="70" ppWidth="130" ppClassCD="0087" ppNotSelect="True" ppValidationGroup="2" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel3" runat="server">
                                                <asp:Label ID="Label9" runat="server" Text="装置区分" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel6" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlEqCls" runat="server" Width="130" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <div style="white-space: nowrap">
                                                    <asp:Panel ID="Panel7" runat="server" Width="0px">
                                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ControlToValidate="ddlEqCls" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2"></asp:CustomValidator>
                                                    </asp:Panel>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <%--<uc:ClsCMDropDownList ID="ClsCMDropDownList1" runat="server" ppName="装置区分" ppNameWidth="120" ppWidth="130" ppNotSelect="True" ppValidationGroup="2" />--%>
                            </td>
                            <td>&nbsp;</td>
                            <td class="auto-style1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel8" runat="server">
                                                <asp:Label ID="Label11" runat="server" Text="装置詳細" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel9" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlEqCd" runat="server" Width="130" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <div style="white-space: nowrap">
                                                    <asp:Panel ID="Panel18" runat="server" Width="0px">
                                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ControlToValidate="ddlEqCd" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2"></asp:CustomValidator>
                                                    </asp:Panel>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
            
                                <%--<uc:ClsCMDropDownList ID="ddlEqCd" runat="server" ppName="装置詳細" ppNameWidth="120" ppWidth="130" ppNotSelect="True" ppValidationGroup="2" />--%>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel14" runat="server">
                                                <asp:Label ID="Label7" runat="server" Text="作業状況" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel15" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlStatusCd" runat="server" Width="200" AutoPostBack="True"></asp:DropDownList><br />
                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="ddlStatusCd" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2"></asp:CustomValidator>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtSttsNotetext" ppName="作業日時"
                                        ppNameVisible="True" ppWidth="200" ppMaxLength="20" ppNameWidth="120" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel16" runat="server">
                                                <asp:Label ID="Label8" runat="server" Text="回復内容" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel17" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlRepairCd" runat="server" Width="750px">
                                                </asp:DropDownList>
                                                <div style="white-space: nowrap">
                                                    <asp:Panel ID="Panel29" runat="server" Width="0px">
                                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ControlToValidate="ddlRepairCd" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                                    </asp:Panel>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <uc:ClsCMTextBox runat="server" ID="tetRepairCcntnt" ppName="回復内容詳細" ppNameVisible="True" ppWidth="750px" ppMaxLength="50" ppNameWidth="120" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblConfDtNm" runat="server" Text="確認日" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblConfDt" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <uc:ClsCMTextBox ID="txtConfUsr" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="確認者" ppNameWidth="120" ppWidth="200" />
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAppDtNm" runat="server" Text="承認日" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAppDt" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel20" runat="server">
                                                <asp:Label ID="Label10" runat="server" Text="承認者" Width="120"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel21" runat="server"  CssClass="align-top">
                                                <asp:DropDownList ID="ddlAddUsr" runat="server" Width="200"></asp:DropDownList><br />
                                                <asp:CustomValidator ID="CustomValidator6" runat="server" ControlToValidate="ddlAddUsr" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <div class="float-left">
                                    <asp:ValidationSummary ID="valsum2" runat="server" CssClass="errortext"  ValidationGroup="2"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
         <table border="0" style="height: 107px;">
              <tr>
                <td style="width: 50px">&nbsp;</td>
                <td class="auto-style6">
                     <table border="0" class="float-left" style="border-style: solid; width: 79%; height: 107px;">
                         <tr>
                            <td style="background-color: #8DB4E2" colspan="2">
                               <asp:Label ID="Label1" runat="server" Text="対応コード" Width="100px" CssClass="center"></asp:Label>
                            </td>
                            <td style="background-color: #8DB4E2;width:100px">
                               <asp:Label ID="Label2" runat="server" Text="対応日時" Width="100px" CssClass="center"></asp:Label>
                            </td>
                            <td style="background-color: #8DB4E2;width:300px">
                               <asp:Label ID="Label4" runat="server" Text="対応担当者" Width="100px" CssClass="center"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="background-color: #8DB4E2">
                               <asp:Label ID="Label3" runat="server" Text="対応内容" Width="100px" CssClass="center"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:CheckBox ID="chkAdptCls" runat="server" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtDealCd" ppName = "対応コード" ppNameVisible="false" ppIMEMode="半角_変更不可" ppMaxLength="5" ppValidationGroup="3" ppRequiredField="True" />
                            </td>
                             <td>
                                 <table>
                                     <tr class="float-left">
                                         <td>
                                            <uc:ClsCMDateTimeBox runat="server" ID="txtDealDt" ppName ="対応日時" ppNameVisible="false" ppValidationGroup="3" ppRequiredField="True" />
                                         </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtDealUer" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="対応担当" ppNameVisible="false" ppWidth="300px"  ppValidationGroup="Detail2" ppRequiredField ="true"/>
                            </td>

                        </tr>
                        <tr>
                           <%-- <td style="text-align: left">
                                <asp:TextBox ID="txtDealUer" runat="server" CssClass="float-left"></asp:TextBox>
                                <uc:ClsCMDropDownList_Mastr runat="server" ID="txtDealUer"  ppName="対応担当者" ppNameVisible="false" ppDataTextField="M02_LASTNAME" ppDataValueField="M02_EMPLOYEE_CD" ppDataSourceID="SqlDataSource11" ppNameWidth="100" ppWidth="120" />
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:SPCDB %>" SelectCommand="SELECT [M02_EMPLOYEE_CD], [M02_LASTNAME] FROM [M02_EMPLOYEE] ORDER BY [M02_EMPLOYEE_CD]" >
                                </asp:SqlDataSource>
                            </td>--%>
                        </tr>
                        <tr>
                            <%--<td>
                            </td>--%>
                            <%--<td>
                               <div style="white-space: nowrap">
                                    <asp:Panel ID="Panel7" runat="server" Width="0px">
                                        <asp:RequiredFieldValidator  ID="cuvtxtDealCd" runat="server" ControlToValidate="txtDealCd" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False"  SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </asp:Panel>
                                </div>
                            </td>
                             <td>
                                <div style="white-space: nowrap">
                                    <asp:Panel ID="Panel8" runat="server" Width="0px">
                                        <asp:RequiredFieldValidator  ID="cuvtxtDealDt" runat="server" ControlToValidate="txtDealDt" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False"  SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </asp:Panel>
                                </div>
                            </td>
                            <td>
                               <div style="white-space: nowrap">
                                    <asp:Panel ID="Panel9" runat="server" Width="0px">
                                        <asp:RequiredFieldValidator  ID="cuvtxtDealUer" runat="server" ControlToValidate="txtDealUer" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </asp:Panel>
                                </div>
                            </td>--%>
                        </tr>
                         <tr>
                            <td colspan="4">
                                <table>
                                    <tr>
                                        <td>
                                            <%--<asp:TextBox ID="txtDealDtl" runat="server" CssClass="float-left" Columns="100" MaxLength="200" TextMode="MultiLine"></asp:TextBox>--%>
                                            <%--<uc:ClsCMTextBox runat="server" ID="txtDealDtl" ppRows="6" ppColumns="70" ppMaxLength="200" ppTextMode="MultiLine" ppNameVisible="False" ppWrap="true" ppName="対応内容" ppValidationGroup="3" ppRequiredField="True"/>--%>
                                            <asp:TextBox ID="txaDealDtl" runat="server" Rows="5" Columns="100" MaxLength="122" TextMode="MultiLine"></asp:TextBox>

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="4">
                                 <div style="white-space: nowrap">
                                    <asp:Panel ID="Panel6" runat="server" Width="0px">
                                        <asp:CustomValidator ID="cuvtxtDealDtl" runat="server" ControlToValidate="txtDealDtl" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>--%>
                        </table>
                     <%--<span><i>FindControl</i></span></td>--%>
                <td class="auto-style6">
                    <table class="auto-style2">
                         <tr>
                            <td class="auto-style4">
                                 <asp:Button ID="btnInsert" runat="server" Text="追加" />
                            </td>
                            <td class="auto-style4">
                                 <asp:Button ID="btnUpdate" runat="server" Text="更新" />
                            </td>
                            <td class="auto-style4">
                                 <asp:Button ID="btnDelete" runat="server" Text="削除" />
                             </td>
                         </tr>
                         <tr>
                             <td class="center" colspan="3">
                             </td>
                         </tr>
                         <tr>
                             <td class="float-right" colspan="3">
                                 <asp:Button ID="btnClere" runat="server" CssClass="center" Text="クリア" />
                             </td>
                         </tr>
                     </table>
                </td>
            </tr>
             <tr>
                <td  colspan="3">
                    <div class="float-left">
                        <asp:ValidationSummary ID="valsum3" runat="server" CssClass="errortext" ValidationGroup="3" />
                    </div>
                </td>
            </tr>
         </Table>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" style="margin-top: 0px" Width="1050px">
        <table>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td style="width:1000px">
                    <div class="grid-out">
                        <div style="height:294px;" class="grid-in">
                            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="grvList" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
