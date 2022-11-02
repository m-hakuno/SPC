<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BRKUPDP001.aspx.vb" Inherits="SPC.BRKUPDP001" %>
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
                    <uc:ClsCMTextBox ID="txtTboxId"  runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="120px" ppWidth="80px"  ppValidationGroup="1" ppRequiredField ="true" ppCheckHan="True"/>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="登録" ValidationGroup="1" />
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
                <td style="width: 50px;">&nbsp;</td>
                <td style="width: 320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblKanriNoNm" runat="server" Text="管理番号" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblKanriNo" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnVersion" /><asp:HiddenField runat="server" ID="hdnTboxType" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td style="width:320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNlclsNm" runat="server" Text="ＮＬ区分" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNlcls" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>                    
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td style="width:320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblEwclsNm" runat="server" Text="ＥＷ区分" Width="120px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnNlclsCd" />
                            </td>
                            <td>
                                <asp:Label ID="lblEwcls" runat="server"  Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnEwclsCd" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 50px;">&nbsp;</td>
                <td style="width: 320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LblTboxTypeNm" runat="server" Text="ＴＢＯＸタイプ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblTboxType" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="HiddenField1" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td style="width:320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LblTboxVerNm" runat="server" Text="ＶＥＲ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="LblTboxVer" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td style="width:320px;">
                    <table>
                        <tr>
                            <td>

                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:20px;">&nbsp;</td>
                <td style="width:320px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTwinCls" runat="server" Text="双子店" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTwin" runat="server" Width="200px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnTwinCd" />
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
                                <asp:Label ID="lblHall" runat="server" Text="ホール名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHallNm" runat="server" Width="480px"></asp:Label>
                                <asp:HiddenField runat="server" ID="hdnHallCd" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td class ="align-top" colspan="3">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHallAddrNm" runat="server" Text="住所" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHallAddr" runat="server"  Width="480px" ></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTelNm" runat="server" Text="ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTel" runat="server" Width="200px"></asp:Label>
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
                                <asp:Label ID="lblEigyosyo" runat="server" Text="注意事項" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEigyosyoNm" runat="server" Width="200px" ></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnEigyosyoCd" />
                            </td>
                        </tr>
                    </table>
                </td>
<%--                <td>&nbsp;</td>--%>
<%--                <td class ="align-top">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTokatu" runat="server" Text="統括保担名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTokatuNm" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnTokatuCd" />
                            </td>
                        </tr>
                    </table>
                </td>--%>
<%--                <td>&nbsp;</td>--%>
            </tr>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNgcOrgNm" runat="server" Text="担当営業所" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNgcOrg" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSalesEmpNm" runat="server" Text="営業社員" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSalesEmp" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTel2Nm" runat="server" Text="ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTel2" runat="server" Width="200px"></asp:Label>
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
                                <asp:Label ID="lblEstTypeNm" runat="server" Text="ＭＤＮ設置種別" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEstType" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="hdnEstType" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHosyuKeiyakuNm" runat="server" Text="保守契約" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHosyuKeiyaku" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblLineTypeNm" runat="server" Text="回線種類" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblLineType" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
<%--                        <tr>
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
                        </tr>--%>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblInsLineNm" runat="server" Text="ＩＮＳ回線" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblInsLine" runat="server" Width="200px"></asp:Label>
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
                                <asp:Label ID="lblSv1KataNm" runat="server" Text="ＳＶ１型式" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSv1Kata" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSv1verNm" runat="server" Text="ＳＶ１ Ｖｅｒ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSv1ver" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSv2kataNm" runat="server" Text="ＳＶ２型式" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSv2kata" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSv2VerNm" runat="server" Text="ＳＶ２ Ｖｅｒ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSv2Ver" runat="server" Width="200px"></asp:Label>
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
                                <asp:Label ID="lblClkataNm" runat="server" Text="ＣＬ型式" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblClkata" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblClVerNm" runat="server" Text="ＣＬ Ｖｅｒ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblClVer" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblOrderServiceNm" runat="server" Text="ｵｰﾀﾞｰｻｰﾋﾞｽ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblOrderService" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMembershipManagementCompanyNm" runat="server" Text="会員管理会社" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblMembershipManagementCompany" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblAgcNm" runat="server" Text="筆頭代理店名" Width="120px" ></asp:Label>
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
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTel3Nm" runat="server" Text="ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTel3" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblGyomuitakuNm" runat="server" Text="業務委託代理店名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblGyomuitaku" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="HiddenField2" />
                                <asp:HiddenField runat="server" ID ="HiddenField3" />
                                <asp:HiddenField runat="server" ID ="HiddenField4" />
                                <asp:HiddenField runat="server" ID ="HiddenField5" />
                                <asp:HiddenField runat="server" ID ="HiddenField6" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTel4Nm" runat="server" Text="ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTel4" runat="server" Width="200px"></asp:Label>
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
                                <asp:Label ID="lblRepNm" runat="server" Text="代行店名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Width="200px"></asp:Label>
                                <asp:HiddenField runat="server" ID ="HiddenField7" />
                                <asp:HiddenField runat="server" ID ="HiddenField8" />
                                <asp:HiddenField runat="server" ID ="HiddenField9" />
                                <asp:HiddenField runat="server" ID ="HiddenField10" />
                                <asp:HiddenField runat="server" ID ="HiddenField11" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTel5Nm" runat="server" Text="ＴＥＬ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTel5" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHojinNm" runat="server" Text="法人名" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHojin" runat="server" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;</td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblStoreStatusNm" runat="server" Text="店舗ｽﾃｰﾀｽ" Width="120px" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStoreStatus" runat="server" Width="200px"></asp:Label>
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
                            <td style="width: 320px;">
                                <uc:ClsCMDropDownList ID="ddlCallCls" runat="server" ppName="コール区分" ppNameWidth="120px" ppWidth="150px" ppClassCD="0033" ppNotSelect="true"  ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>
                            <td style="width:20px;">&nbsp;</td>
<%--                            <td style="width:320px;">
                                <uc:ClsCMDropDownList ID="ddlBlngCls" runat="server" ppName="所属区分" ppNameWidth="120px" ppWidth="150px" ppClassCD="0034" ppNotSelect="true"  ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>--%>
                            <td style="width:20px;">&nbsp;</td>
<%--                            <td style="width:290px;">
                                <uc:ClsCMDropDownList ID="ddlAppaCls" runat="server" ppName="機種区分" ppNameWidth="120px" ppWidth="150px" ppClassCD="0042" ppNotSelect="true"  ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td style="width: 320px;">
                                <uc:ClsCMDateBox runat="server" ID="dttShinkokuDt" ppName="申告日" ppNameWidth="120px"  ppValidationGroup="Detail"/>
                            </td>
                            <td>&nbsp;</td>
                            <td style="width: 320px;">
                                <uc:ClsCMTimeBox runat="server" ID="tmtShinkokuTm" ppName="申告時間" ppNameWidth="120px"  ppValidationGroup="Detail"/>
                            </td>
                            <td>&nbsp;</td>
                            <td class ="align-top" colspan="3">
                                <uc:ClsCMTextBox ID="txtUketukeNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="受付者" ppNameWidth="120px" ppWidth="150px"  ppValidationGroup="Detail"/>
                            </td>
                        </tr>
                        <tr>
                            <td class ="align-top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRptBase1" runat="server" Text="申告元1" Width="120px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:dropdownList ID="ddlRptBase1" runat="server" style="width:180px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                            <td class ="align-top">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRptBase2" runat="server" Text="申告元2" Width="120px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:dropdownList ID="ddlRptBase2" runat="server" style="width:180px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtShinkokuNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="申告者" ppNameWidth="120px" ppWidth="150px"  ppValidationGroup="Detail"/>
                            </td>
                            <td>&nbsp;</td>
                            <td class ="align-top" colspan="3">
                                <uc:ClsCMTextBox ID="txtShinkokuTel" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="申告者ＴＥＬ" ppNameWidth="120px" ppWidth="120px"  ppValidationGroup="Detail" ppCheckHan="True" ppNum="False"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 320px;">
                                <uc:ClsCMDropDownList ID="ddlModelCls1" runat="server" ppName="機種分類１" ppNameWidth="120px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true"  ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>
                            <td>&nbsp;</td>
                            <td style="width: 320px;">
                                <uc:ClsCMDropDownList ID="ddlModelCls2" runat="server" ppName="機種分類２" ppNameWidth="120px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true"  ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>
                            <td>&nbsp;</td>
                            <td style="width: 320px;">
                                <uc:ClsCMDropDownList ID="ddlModelNm" runat="server" ppName="機種名" ppNameWidth="120px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true"  ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtErrorCode" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="ｴﾗｰｺｰﾄﾞ" ppNameWidth="120px" ppWidth="150px"  ppValidationGroup="Detail"/>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <uc:ClsCMTextBox ID="txtDetailCode" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="詳細ｺｰﾄﾞ" ppNameWidth="120px" ppWidth="150px"  ppValidationGroup="Detail"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMdnWork" runat="server" Text="ＭＤＮ作業" Width="120px" />
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="cklMdnWork" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                                                RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium" Width="565px" >
                                                <asp:ListItem>新規</asp:ListItem>
                                                <asp:ListItem>INS→光</asp:ListItem>
                                                <asp:ListItem>UPS疎通</asp:ListItem>
                                                <asp:ListItem>ｵｰﾀﾞｰ設定(許可)､ｵｰﾀﾞｰ設定(禁止)</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>                                    
                                </table>
                            </td>                            
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMdnMaintenance" runat="server" Text="ＭＤＮ保守" Width="120px" />
                                        </td>
                                        <td colspan="5">
                                            <asp:CheckBoxList ID="cklMdnMaintenance" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                                                RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium" >
                                                <asp:ListItem>ﾘﾓｰﾄ</asp:ListItem>
                                                <asp:ListItem>SV1交換</asp:ListItem>
                                                <asp:ListItem>SV2交換</asp:ListItem>
                                                <asp:ListItem>CL交換</asp:ListItem>
                                                <asp:ListItem>HDD交換</asp:ListItem>
                                                <asp:ListItem>その他</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>                                
                            </td>
                            
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRptCd" runat="server" Text="申告内容" Width="120px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:dropdownList ID="ddlRptCd" runat="server" style="width:800px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <uc:ClsCMTextBox ID="txtShinkokuDtl1" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="申告内容詳細" ppNameWidth="120px" ppWidth="800px"  ppNameVisible="True" ppValidationGroup="Detail"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <uc:ClsCMTextBox ID="txtHikitugiDtl" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="引継内容" ppNameWidth="120px" ppWidth="800px"   ppNameVisible="True" ppValidationGroup="Detail"/>
                            </td>
                        </tr>
                        <tr>
<%--                            <td colspan="5">
                                <uc:ClsCMTextBox ID="txtShinkokuDtl2" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="備考" ppNameWidth="120px" ppWidth="800px"  ppNameVisible="false" ppValidationGroup="Detail"/>
                            </td>--%>
                            <td colspan="5">
                                <uc:ClsCMTextBox ID="txtRemarks" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="備考" ppNameWidth="120px" ppWidth="800px"  ppNameVisible="false" ppValidationGroup="Detail"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:320px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblStatusCd" runat="server" Text="進捗状況" Width="120px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:dropdownList ID="ddlStatusCd" runat="server" style="width:200px;"  AutoPostBack="True"/><br />
                                            <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="ddlStatusCd" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 320px;">
                                <uc:ClsCMDateBox runat="server" ID="dttKanryoDt" ppName="完了日" ppNameWidth="120px"  ppValidationGroup="Detail"/>
                            </td>
                            <td style="width: 320px;">
                                <uc:ClsCMTimeBox runat="server" ID="tmtKanryoTm" ppName="完了時間" ppNameWidth="120px"  ppValidationGroup="Detail"/>
                            </td>
                        </tr>
<%--                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDealDtl" runat="server" Text="処置内容" Width="120px" ></asp:Label>
                                        </td>
                                        <td>
                                            <asp:dropdownList ID="ddlDealDtl" runat="server" style="width:280px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;</td>
                            <td class ="align-top" colspan="3">
                                <uc:ClsCMTextBox ID="txtTroubleNo"  runat="server" ppIMEMode="半角_変更不可" ppMaxLength="11" ppName="トラブル管理番号" ppNameWidth="120px" ppWidth="90px"  ppValidationGroup="Detail" ppCheckHan="True"/>
                            </td>
                        </tr>--%>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
<%--    <asp:panel ID="pnlDevice" runat="server" >
        <table >
              <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <div id="DivOut" runat="server" class="grid-out" style="width: 1140px">
                        <div id="DivIn" runat="server" class="grid-in" style="width: 1140px">
                            <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="GridView1" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr> 
        </table>
    </asp:panel>--%>
    <asp:Panel ID="pnlRegister" runat="server">
         <table border="0" style="height: 107px;">
              <tr>
                <td style="width: 50px">&nbsp;</td>
                <td class="auto-style6">
                     <table border="0" class="float-left" style="border-style: solid; width: 79%; height: 107px;">
                        <tr>
                            <td>
                                <asp:HiddenField ID="hdnMsiKey1" runat="server" />
                                <asp:HiddenField ID="hdnMsiKey2" runat="server" />
                                <uc:ClsCMDateTimeBox runat="server" ID="txtTaiouDt" ppName ="対応日時" ppNameWidth="80px" ppRequiredField="true" ppValidationGroup="Detail2" />
                            </td>
                            <td class="align-top">
                                <uc:ClsCMTextBox ID="txtTaiouNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="対応担当" ppNameWidth="80px" ppRequiredField="true" ppValidationGroup="Detail2" ppWidth="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txaDealDtl" runat="server" Rows="5" Columns="100" MaxLength="122" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                <td class="auto-style6">
                    <table class="auto-style2">
                         <tr>
                            <td class="auto-style4">
                                 <asp:Button ID="btnInsert" runat="server" Text="追加" ValidationGroup="Detail2" />
                            </td>
                            <td class="auto-style4">
                                 <asp:Button ID="btnUpdate" runat="server" Text="更新" ValidationGroup="Detail2" />
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
                                 <asp:Button ID="btnClear" runat="server" Text="クリア" />
                             </td>
                         </tr>
                     </table>
                </td>
            </tr>
             <tr>
                <td  colspan="3">
                    <div class="float-left">
                        <asp:ValidationSummary ID="vasSummaryUpdate" runat="server" CssClass="errortext" ValidationGroup="Detail2" />
                    </div>
                </td>
            </tr>
         </Table>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" style="margin-top: 0px">
        <table>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <div class="grid-out" style="width:1020px;">
                        <div class="grid-in" style="width:1020px;">
                            <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="grvList2" runat="server" ShowHeaderWhenEmpty="True">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" style="margin-top: 0px">
        <table>
            <tr>
                <td style="width: 50px">&nbsp;</td>
                <td>
                    <div class="grid-out" style="width:1020px;">
                        <div class="grid-in" style="width:1020px;">
                            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="grvList" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" style="margin-top: 0px">
        <table>
<%--            <tr>
                <td colspan="5">
                    <uc:ClsCMTextBox ID="txtSyotiDtl" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="処置内容詳細" ppNameWidth="120px" ppWidth="800px" ppNameVisible="True"  ppValidationGroup="Detail"/>
                </td>

            </tr>--%>
            <tr>
                <td colspan="5">
                    <table>
                        <tr>
<%--                            <td style="width:320px;">
                                <uc:ClsCMDateTimeBox runat="server" ID="txtHokokuDt" ppName ="ＮＧＣ報告日時" ppNameWidth="117px" ppValidationGroup="Detail" />
                            </td>
                            <td style="width:20px;">&nbsp;</td>--%>
                            <td style="width:220px;">
                                <uc:ClsCMDateBox runat="server" ID="txtKakuninDt" ppName="確認日" ppDateFormat ="年月日" ppNameWidth="70px"  ppValidationGroup="Detail"/>
                            </td>
                            <td style="width:20px;">&nbsp;</td>
                            <td class ="align-top" style="width:320px;">
                                <uc:ClsCMTextBox ID="txtKakuninNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="確認者" ppNameWidth="70px" ppWidth="150px"  ppValidationGroup="Detail"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="float-left">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errortext" ValidationGroup="Detail" />
    </div>
</asp:Content>
