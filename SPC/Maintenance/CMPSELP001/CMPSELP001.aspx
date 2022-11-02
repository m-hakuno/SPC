<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CMPSELP001.aspx.vb" Inherits="SPC.CMPSELP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/UserControl/ClsCMDateTimeBox.ascx" TagPrefix="uc" TagName="ClsCMDateTimeBox" %>

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
            height: 23px;
        }
        .auto-style2 {
            height: 44px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px; table-layout: fixed;" class="center" border="0">
        <tr>
            <td>
                <div style="border-width: 1px; border-bottom-style: solid">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--発生区分-->
                            <td style="width: 320px">
                                <table border="0">
                                   <tr>
                                        <td style="vertical-align: top; padding-top: 4px;" >
                                            <asp:Label ID="lblOccurCls" runat="server" Text="発生区分" Width="60"></asp:Label>
                                        </td>
                                        <td style="margin-left: 40px">
                                            <asp:DropDownList ID="ddlOccurCls" runat="server" Width="230"
                                                ValidationGroup="Entry" ></asp:DropDownList>
                                            <br />
                                            <asp:CustomValidator ID="valOccurCls" runat="server" CssClass="errortext" 
                                                ValidationGroup="Entry"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＴＢＯＸＩＤ-->
                            <td style="vertical-align: top">
                                <uc:ClsCMTextBox runat="server" ID="txtTBoxid" ppName="ＴＢＯＸＩＤ" ppNameWidth="90"
                                    ppWidth="65" ppMaxLength="8" ppCheckHan="True" ppNum="False" ppRequiredField="True"
                                    ppValidationGroup="Entry" ppIMEMode="半角_変更不可" />
                            </td>
                            <!--登録ボタン-->
                            <td style="vertical-align: bottom; text-align: right;">
                                <asp:Button ID="btnEntry" runat="server" Text="登録" ValidationGroup="Entry" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="valSumEntry" runat="server" CssClass="errortext" ValidationGroup="Entry" />
                </div>
                <br />

                <asp:Panel ID="pnlMnt1" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--管理番号-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="管理番号" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntNo" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtMntNo" ppName="管理番号" ppNameWidth="100"
                                    ppWidth="150" />--%>
                            </td>
                            <!--特別保守-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="特別保守" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSpecialMnt" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtSpecialMnt" ppName="特別保守" ppNameWidth="100"
                                    ppWidth="200" />--%>
                            </td>
                            <!--ＮＬ区分-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="ＮＬ区分" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNLCls" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtNLCls" ppName="ＮＬ区分" ppNameWidth="110"
                                    ppWidth="20" />--%>
                            </td>
                        </tr>
                        <tr>
                            <!--ＴＢＯＸタイプ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="ＴＢＯＸタイプ" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxType" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtTboxType" ppName="ＴＢＯＸタイプ" ppNameWidth="100"
                                    ppWidth="200" />--%>
                            </td>
                            <!--ＶＥＲ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="ＶＥＲ" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxVer" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtTboxVer" ppName="ＶＥＲ" ppNameWidth="110"
                                    ppWidth="50" />--%>
                            </td>
                            <!--ＥＷ区分-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="ＥＷ区分" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEWCls" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtEWCls" ppName="ＥＷ区分" ppNameWidth="110"
                                    ppWidth="20" />--%>
                            </td>
                        </tr>
                        <tr>
                            <!--ホール名-->
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" Text="ホール名" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHallNm" runat="server" Width="800px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtHallNm" ppName="ホール名" ppNameWidth="100"
                                    ppWidth="500" />--%>
                            </td>
                        </tr>
                        <tr>
                            <!--住所（上段）-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label21" runat="server" Text="住所" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAddr1" runat="server" Width="500px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtAddr1" ppName="　　　住所" ppNameWidth="100"
                                    ppWidth="500" />--%>
                            </td>
                            <!--ＴＥＬ-->
                            <td class="auto-style1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label24" runat="server" Text="ＴＥＬ" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTelno" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtTelno" ppName="　　　ＴＥＬ" ppNameWidth="100"
                                    ppWidth="150" />--%>
                            </td>
                        </tr>
                        <tr>
                            <!--保担名-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" Text="保担名" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranchNm" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtBranchNm" ppName="保担名" ppNameWidth="110"
                                    ppWidth="200" />--%>
                            </td>
                            <!--統括保担名-->
                            <td class="auto-style1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label25" runat="server" Text="統括保担名" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblUnfNm" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtUnfNm" ppName="統括保担名" ppNameWidth="110"
                                    ppWidth="200" />--%>
                            </td>
                            <!--住所（下段）-->
<%--                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label22" runat="server" Text="" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAddr2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNgcOrgNm" runat="server" Text="担当営業部" Width="100px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNgcOrg" runat="server" Width="200px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAgcNm" runat="server" Text="代理店名" Width="100px" ></asp:Label>
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
                            <td class ="align-top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRepNm" runat="server" Text="代行店名" Width="100px" ></asp:Label>
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
                            <td class ="align-top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOrgTelNo" runat="server" Text="営業部ＴＥＬ" Width="100px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOrgTel" runat="server" Width="200px" ></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class ="align-top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTwinCls" runat="server" Text="双子店区分" Width="100px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTwin" runat="server" Width="200px" ></asp:Label>
                                            <asp:HiddenField runat="server" ID ="hdnTwinCd" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEstCls" runat="server" Text="ＭＤＮ設置有無" Width="100px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEst" runat="server" Width="200px"></asp:Label>
                                            <asp:HiddenField runat="server" ID ="hdnEstCls" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class ="align-top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMdnNm" runat="server" Text="ＭＤＮ機器名" Width="100px" ></asp:Label>
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
                            <!--申告日-->
                            <td>
                                <uc:ClsCMDateBox ID="dtbRptDt" runat="server" ppName="申告日" ppNameWidth="100" />
                            </td>
                            <!--申告者-->
                            <td>
                                <uc:ClsCMTextBox ID="txtRptCharge" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="申告者" ppNameWidth="100" ppWidth="150" />
                            </td>
                            <!--申告元（申告元マスタ）-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="申告元　　　　　"></asp:Label>
                                            <asp:DropDownList ID="ddlRptBase" runat="server" Width="150">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--申告日-->
                            <td>
                                <uc:ClsCMDateTimeBox ID="dtbRcptDt" runat="server" ppName="受付日時" ppNameWidth="100" />
                            </td>
                            <!--申告者-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtRcptCharge" ppName="受付者" ppNameWidth="100"
                                    ppWidth="150" ppMaxLength="20" ppIMEMode="全角" />
                            </td>
                            <!--申告元（申告元マスタ）-->
                            <td>
                                <asp:Label ID="lblRptTel" runat="server" Text="申告元ＴＥＬ" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <!--受付日時-->
                            <td>
                                <uc:ClsCMDateTimeBox runat="server" ID="dtbReqDt" ppName="依頼日時" ppNameWidth="100" />
                            </td>
                            <!--受付者-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtReqUsr" ppName="依頼者" ppNameWidth="100"
                                    ppWidth="150" ppMaxLength="20" ppIMEMode="全角" />
                            </td>
                            <!--申告元ＴＥＬ-->
                            <td>
                                <table border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label26" runat="server" Text="トラブル管理番号" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTrbNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <%--                                <uc:ClsCMTextBox runat="server" ID="txtTrbNo" ppName="トラブル管理番号" ppNameWidth="110"
                                    ppWidth="100" />--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <asp:Panel ID="pnlMnt2" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--ホール担当者-->
                            <td style="width: 350px">
                                <uc:ClsCMTextBox runat="server" ID="txtHallCharge" ppName="ホール担当者" ppNameWidth="100"
                                    ppWidth="200" ppMaxLength="20" ppIMEMode="全角" />
                            </td>
                            <!--最終集信日-->
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dtbLastDt" ppName="最終集信日" ppNameWidth="100" />
                            </td>
                        </tr>
                        <tr>
                            <!--引継内容１、２-->
                            <td colspan="2">
                                <uc:ClsCMTextBox runat="server" ID="txtInhCntnt1" ppName="引継内容" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppIMEMode="全角" />
                                <uc:ClsCMTextBox runat="server" ID="txtInhCntnt2" ppName="引継内容" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="false" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <asp:Panel ID="pnlMnt3" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--申告内容（申告内容マスタ）-->
                            <td colspan="2">
                                <asp:Label ID="Label2" runat="server" Text="申告内容" Width="100"></asp:Label>
                                <asp:DropDownList ID="ddlRpt" runat="server"  Width="650"></asp:DropDownList>
                            </td>
                            <!--故障重要度-->
                            <td>
                                <asp:CheckBox ID="cbxImpCls" runat="server" Text="故障重要度" />
                            </td>
                        </tr>
                        <tr>
                            <!--申告内容１、２-->
                            <td colspan="2">
                                <uc:ClsCMTextBox runat="server" ID="txtRptDtl1" ppName="申告内容詳細" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="True" ppIMEMode="全角" />
                                <uc:ClsCMTextBox runat="server" ID="txtRptDtl2" ppName="申告内容" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="false" ppIMEMode="全角" />
                            </td>
                            <!--引継区分-->
                            <td class="align-top"  style="width: 400px">
                                <asp:CheckBox ID="cbxBsnsdistCls" runat="server" Text="営業支障" />
                                <asp:CheckBox ID="cbxScnddistCls" runat="server" Text="２次支障" />
                            </td>
                        </tr>
                        <tr>
                            <!--連絡事項-->
                            <td colspan="2">
                                <uc:ClsCMTextBox runat="server" ID="txtInfo1" ppName="連絡事項" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppIMEMode="全角" />
                                <uc:ClsCMTextBox runat="server" ID="txtInfo2" ppName="連絡事項" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="false" ppIMEMode="全角" />
                            </td>
                            <td class="align-top">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <!--作業予定日時-->
                                <uc:ClsCMDateTimeBox runat="server" ID="dtbWrkDt" ppName="作業予定日時" ppNameWidth="100" />
                            </td>
                            <td>
                                <!--出発日時-->
                                <uc:ClsCMDateTimeBox runat="server" ID="dtbDeptDt" ppName="出発予定日時" ppNameWidth="90" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <!--会社-->
                                <uc:ClsCMDropDownList runat="server" ID="ddlCmp" ppName="会社名" ppNameWidth="100"
                                    ppNotSelect="True" ppWidth="200" ppClassCD="0111" />
                            </td>
                            <td>
                                <!--作業者コード-->
                                <uc:ClsCMTextBox runat="server" ID="txtWrkCode" ppName="作業者コード" ppNameWidth="90"
                                    ppWidth="150" ppMaxLength="8" ppIMEMode="半角_変更不可" />
                            </td>
                            <td>
                                <!--作業者-->
                                <uc:ClsCMTextBox runat="server" ID="txtWrkUser" ppName="作業者" ppNameWidth="100"
                                    ppWidth="150" ppMaxLength="20" ppIMEMode="全角" />
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <!--開始日時-->
                                <uc:ClsCMDateTimeBox runat="server" ID="dtbStartDt" ppName="開始日時" ppNameWidth="100" />
                            </td>
                            <td>
                                <!--終了日時-->
                                <uc:ClsCMDateTimeBox runat="server" ID="dtbEndDt" ppName="終了日時" ppNameWidth="90" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label29" runat="server" Text="作業者TEL" Width="93"></asp:Label>
                                        </td>
                                        <td class="auto-style3">
                                            <%--<asp:Label ID="LblWrkTel" runat="server"></asp:Label>--%>
                                            <uc:ClsCMTextBox ID="txtWrkTel" runat="server" ppName="作業者TEL" ppWidth="150" ppNameVisible="false" ppCheckHan="True" ppCheckLength="False" ppIMEMode="半角_変更不可" ppMaxLength="15" ppNum="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--作業状況（ステータス）-->
                            <td colspan="2">
                                <div class="float-left" style="margin-top: 3px">
                                    <asp:Label ID="Label4" runat="server" Text="作業状況" Width="100"></asp:Label>
                                    <asp:DropDownList ID="ddlStatus" runat="server"  Width="280px" AutoPostBack="true"></asp:DropDownList>
                                    <div style="white-space: nowrap">
                                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                            <asp:Label ID="Label5" runat="server" Text="　　　　" Width="100"></asp:Label>
                                            <asp:CustomValidator ID="cuvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </td>
                            <!--故障機器-->
                            <td>
                                <uc:ClsCMDropDownList runat="server" ID="ddlAppa" ppName="故障機器" ppNameWidth="100"
                                    ppNotSelect="True" ppWidth="150" ppClassCD="0094" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="auto-style2">
                                <div >
                                    <uc:ClsCMTextBox runat="server" ID="txtSttsNotetext" ppName="作業日時"
                                        ppNameVisible="True" ppWidth="280" ppMaxLength="20" ppIMEMode="全角" ppNameWidth="100" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <!--回復内容-->
                            <td colspan="2">
                                <!--回復内容マスタ-->
                                <asp:Label ID="Label6" runat="server" Text="回復内容" Width="100"></asp:Label>
                                <asp:DropDownList ID="ddlRepair" runat="server"  Width="650"></asp:DropDownList>
                                <!--入力-->
                                <uc:ClsCMTextBox runat="server" ID="txtRepair" ppName="回復内容詳細" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="True" ppIMEMode="全角" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--備考・連絡-->
                            <td colspan="2">
                                <uc:ClsCMTextBox runat="server" ID="txtNotetext1" ppName="備考・連絡" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppIMEMode="全角" />
                                <uc:ClsCMTextBox runat="server" ID="txtNotetext2" ppName="備考・連絡" ppNameWidth="100"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="false" ppIMEMode="全角" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br/>

                <asp:Panel ID="pnlMnt4" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <br/>
                    <table>
                        <tr>
                            <td>
                                <table border="1" style="width:100%; border-collapse: collapse;" >
                                    <tr>
                                        <td colspan="2" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label10" runat="server" Text="　対応コード" Width="100"></asp:Label>
                                        </td>
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label11" runat="server" Text="　対応日時"></asp:Label>
                                        </td>
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label12" runat="server" Text="　対　応　担　当　者"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label13" runat="server" Text="　対　応　内　容"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <!--適応区分-->
                                            <asp:CheckBox ID="cbxDealAdptCls" runat="server" />
                                        </td>
                                        <td>
                                            <!--対応コード-->
                                            <uc:ClsCMTextBox runat="server" ID="txtDealCd" ppName="対応コード" ppNameVisible="false"
                                                ppWidth="60" ppMaxLength="5" ppCheckHan="True" ppIMEMode="半角_変更不可"
                                                ppRequiredField="true" ppValidationGroup="Detail"/>
                                        </td>
                                        <td>
                                            <!--対応日時-->
                                            <uc:ClsCMDateTimeBox runat="server" ID="dtbDealDt" ppName="対応日時" ppNameVisible="False"
                                                ppRequiredField="true" ppValidationGroup="Detail" />
                                        </td>
                                        <td>
                                            <!--対応担当者-->
                                            <uc:ClsCMTextBox runat="server" ID="txtDealUsr" ppName="対応担当者" ppNameVisible="false"
                                                ppWidth="575" ppMaxLength="20" ppRequiredField="true" ppValidationGroup="Detail" ppIMEMode="全角" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <!--対応内容-->
<%--                                           <uc:ClsCMTextBox runat="server" ID="txtDealDtl" ppName="対応内容" ppNameVisible="false"
                                               ppWidth="850" ppMaxLength="200" ppRequiredField="true" ppValidationGroup="Detail" ppIMEMode="全角" />--%>
                                            <asp:TextBox ID="txaDealDtl" runat="server" Rows="5" Columns="100" MaxLength="122" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <!--保守管理連番-->
                                            <asp:Label ID="lblDealMntSeq" runat="server" Text="保守管理連番" Visible="false"></asp:Label>
                                            <asp:Label ID="lblRecKbn" runat="server" Text="対応区分" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnDetailInsert" runat="server" Text="追加" ValidationGroup="Detail" />
                                            <asp:Button ID="btnDetailUpdate" runat="server" Text="更新" ValidationGroup="Detail" />
                                            <asp:Button ID="btnDetailDelete" runat="server" Text="削除" ValidationGroup="Detail"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr>
                                        <td class="float-right">
                                            <asp:Button ID="btnDetailClear" runat="server" Text="クリア" ValidationGroup="Detail"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="valSumDetail" runat="server" CssClass="errortext" ValidationGroup="Detail" />
                <br/>
                </asp:Panel>
                <br/>

                <!--【対応明細（グリッド）】-->
                <div id="DivOut" runat="server" class="grid-out" style="width: 1040px">
                    <div id="DivIn" runat="server" class="grid-in" style="height: 360px">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
                <br/>

                <asp:Panel ID="pnlMnt5" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--特別保守フラグ-->
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="特別保守フラグ" Width="106"></asp:Label>
                                <asp:DropDownList ID="ddlMntFlg" runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList>
                            </td>
                            <!--依頼承認-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label27" runat="server" Text="依頼承認" Width="105"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInsApp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtInsApp" ppName="検収承認" ppNameWidth="105"
                                    ppWidth="200" />--%>
                            </td>
                            <!--検収承認-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label28" runat="server" Text="検収承認" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReqApp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
<%--                                <uc:ClsCMTextBox runat="server" ID="txtReqApp" ppName="請求承認" ppNameWidth="100"
                                    ppWidth="200" />--%>
                            </td>
                        </tr>
                        <tr>
                            <!--出発時間-->
                            <td>
                                <uc:ClsCMTimeBox runat="server" ID="tmbDeptTm" ppName="出発時間" ppNameWidth="105" />
                            </td>
                            <!--開始時間-->
                            <td>
                                <uc:ClsCMTimeBox runat="server" ID="tmbStartTm" ppName="開始時間" ppNameWidth="105"/>
                            </td>
                            <!--終了時間-->
                            <td>
                                <uc:ClsCMTimeBox runat="server" ID="tmbEndTm" ppName="終了時間" ppNameWidth="100" />
                            </td>
                        </tr>
                        <tr>
                            <!--作業内容（作業内容マスタ）-->
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="作業内容" Width="106"></asp:Label>
                                <asp:DropDownList ID="ddlWrk" runat="server" Width="150" AutoPostBack="True"></asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--特別保守作業時間-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtMntTm" ppName="特別保守作業時間" ppNameWidth="105"
                                    ppMaxLength="3" ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可" />
                            </td>
                            <!--特別保守往復時間-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtGbTm" ppName="特別保守往復時間" ppNameWidth="105"
                                    ppMaxLength="3" ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--作業人数-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtPsnNum" ppName="作業人数" ppNameWidth="105"
                                    ppWidth="50" ppMaxLength="2" ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可" />
                            </td>
                            <!--請求金額-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtReqPrice" ppName="請求金額" ppNameWidth="105"
                                    ppWidth="100" ppMaxLength="8" ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可"
                                    ppTextAlign="右" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--提出日-->
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dtbSubmitDt" ppName="提出日" ppNameWidth="105" />
                            </td>
                            <!--メーカ修理-->
                            <td>
                                <uc:ClsCMDropDownList runat="server" ID="ddlRepairCls" ppName="メーカ修理" ppNameWidth="105"
                                    ppNotSelect="True" ppWidth="150" ppClassCD="0108" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--備考-->
                            <td colspan="3">
                                <uc:ClsCMTextBox runat="server" ID="txtNotetext" ppName="備考" ppNameWidth="105"
                                    ppWidth="650" ppMaxLength="50" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <!--処置内容-->
                            <td colspan="3">
                                <!--処置内容マスタ-->
                                <asp:Label ID="Label8" runat="server" Text="処置内容" Width="106"></asp:Label>
                                <asp:DropDownList ID="ddlDeal" runat="server" Width="650"></asp:DropDownList>
                                <!--入力-->
                                <uc:ClsCMTextBox runat="server" ID="txtDeal" ppName="処置内容" ppNameWidth="105"
                                    ppWidth="650" ppMaxLength="50" ppNameVisible="false" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br/>

                <asp:Panel ID="pnlMnt6" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--輸送日-->
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dtbTransDt" ppName="輸送日" ppNameWidth="105" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--輸送元-->
                            <td>
                                <asp:Panel ID="Panel17" runat="server" CssClass="float-left">
                                    <uc:ClsCMDropDownList runat="server" ID="ddlTransbfCls" ppName="輸送元" ppNameWidth="105"
                                        ppNotSelect="True" ppWidth="100" ppClassCD="0051" />
                                </asp:Panel>
                                <asp:Panel ID="Panel18" runat="server" CssClass="float-left">
                                    <uc:ClsCMTextBox runat="server" ID="txtTransbfCd" ppName="輸送元コード" ppNameVisible="false"
                                        ppWidth="80" ppMaxLength="8" ppCheckHan="True" ppIMEMode="半角_変更不可" />
                                </asp:Panel>
                                <asp:Panel ID="Panel19" runat="server" CssClass="float-left" Style= "padding-top:7px">
                                    <asp:Label ID="lblTransbfNm" runat="server"></asp:Label>
<%--                                    <uc:ClsCMTextBox runat="server" ID="txtTransbfNm" ppNameVisible="false" ppWidth="200" />--%>
                                </asp:Panel>
                            </td>
                            <!--輸送先-->
                            <td>
                                <asp:Panel ID="Panel20" runat="server" CssClass="float-left">
                                    <uc:ClsCMDropDownList runat="server" ID="ddlTransafCls" ppName="輸送先" ppNameWidth="105"
                                        ppNotSelect="True" ppWidth="100" ppClassCD="0051" />
                                </asp:Panel>
                                <asp:Panel ID="Panel21" runat="server" CssClass="float-left">
                                    <uc:ClsCMTextBox runat="server" ID="txtTransafCd" ppName="輸送先コード" ppNameVisible="false"
                                        ppWidth="80" ppMaxLength="8" ppCheckHan="True" ppIMEMode="半角_変更不可" />
                                </asp:Panel>
                                <asp:Panel ID="Panel22" runat="server" CssClass="float-left" Style= "padding-top:7px">
                                    <asp:Label ID="lblTransafNm" runat="server"></asp:Label>
<%--                                    <uc:ClsCMTextBox runat="server" ID="txtTransafNm" ppNameVisible="false" ppWidth="200" />--%>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <!--輸送物品-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtTransItem" ppName="輸送物品" ppNameWidth="105"
                                    ppWidth="300" ppMaxLength="50" ppIMEMode="全角" />
                            </td>
                            <!--輸送理由-->
                            <td>
                                <uc:ClsCMDropDownList runat="server" ID="ddlTransRsn" ppName="輸送理由" ppNameWidth="105"
                                    ppNotSelect="True" ppWidth="300" ppClassCD="0055" />
                            </td>
                        </tr>
                        <tr>
                            <!--輸送会社-->
                            <td colspan="2">
                                <uc:ClsCMTextBox runat="server" ID="txtTransComp" ppName="輸送会社" ppNameWidth="105"
                                    ppWidth="400" ppMaxLength="50" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <!--輸送区分-->
                            <td>
                                <uc:ClsCMDropDownList runat="server" ID="ddlTransCls" ppName="輸送区分" ppNameWidth="105"
                                    ppNotSelect="True" ppWidth="80" ppClassCD="0108" />
                            </td>
                            <!--輸送金額-->
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtTransPrice" ppName="輸送金額" ppNameWidth="105"
                                    ppWidth="100" ppMaxLength="8"  ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可"
                                    ppTextAlign="右" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="valSumDeal" runat="server" CssClass="errortext"/>
</asp:Content>
