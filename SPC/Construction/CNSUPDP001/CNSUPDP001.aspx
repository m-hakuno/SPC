<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CNSUPDP001.aspx.vb" Inherits="SPC.CNSUPDP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript">
        function JPNDisp(txtobj,lblobj) {
            var txtcntrl = txtobj.name;
            var txtvalue = txtobj.value;
            var lblcntrl = lblobj.name;
            var lblvalue = lblobj.value;

            if (txtcntrl == 'cphMainContent_txtStkHoleIn_txtTextBox') {
                if(txtvalue == '') {
                    lblobj.value = '';
                } else if(txtvalue == '0') {
                    lblobj.value = '';
                } else if(txtvalue == '1') {
                    lblobj.value = '該当';
                } else {
                    lblobj.value = '不明';
                }
            } else if (txtcntrl == 'cphMainContent_txtStkHoleOut_txtTextBox') {
                if(txtvalue == '') {
                    lblobj.value = '';
                } else if(txtvalue == '0') {
                    lblobj.value = '';
                } else if(txtvalue == '1') {
                    lblobj.value = '該当';
                } else {
                    lblobj.value = '不明';
                }
            } else if (txtcntrl == 'cphMainContent_txtSfOperation_txtTextBox') {
                if(txtvalue == '') {
                    lblobj.value = '';
                } else if(txtvalue == '0') {
                    lblobj.value = '有り';
                } else if(txtvalue == '1') {
                    lblobj.value = '無し';
                } else {
                    lblobj.value = '不明';
                }
            } else if (txtcntrl == 'cphMainContent_txtTboxTakeawayDivision_txtTextBox') {
                if(txtvalue == '') {
                    lblobj.value = '';
                } else if(txtvalue == '0') {
                    lblobj.value = '無し';
                } else if(txtvalue == '1') {
                    lblobj.value = '有り';
                } else {
                    lblobj.value = '不明';
                }
            }
        }
    </script>
    <style type="text/css">
        .auto-style4 {
            height: 49px;
        }
        .auto-style5 {
            width: 150px;
            height: 49px;
        }
        .auto-style7 {
            width: 85px;
        }
        .auto-style11 {
            width: 255px;
        }
        .auto-style12 {
            width: 256px;
        }
        .auto-style13 {
            width: 150px;
        }
        .auto-style14 {
            width: 179px;
        }
        .auto-style15 {
            width: 181px;
        }
        .auto-style19 {
            width: 188px;
        }
        .auto-style23 {
            width: 303px;
        }
        .auto-style24 {
            width: 200px;
        }
        .auto-style25 {
            width: 521px;
        }
        .auto-style26 {
            width: 298px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table border="0" class="center" style="width: 100%;">
        <tr>
            <td colspan="2">
                <asp:Panel ID="Panel1" runat="server">
                    <table border="0" style="width:1050px;">
                        <tr>
                            <td style="width: 220px">
                                <uc:ClsCMTextBoxTwo ID="ProvisionalRequestNo" runat="server" ppIMEModeTwo="半角_変更不可" ppMaxLengthTwo="8" ppName="仮依頼番号" ppNameWidth="80" ppWidthOne="50" ppWidthTwo="80" ppMaxLengthOne="5" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 180px">
                                <asp:Button ID="btmAttachRequestNo" runat="server" Text="依頼番号紐付け" Width="120px" OnClientClick="ProvisionalRequestNo.text = lblRequestNo_2.text " />
                            </td>
                            <td class="auto-style23">
                                <asp:Label ID="Label3" runat="server" Text="案件進捗状況" Width="85px"></asp:Label>
                                 <asp:DropDownList ID="ddlMTRStatus" runat="server"></asp:DropDownList>
                            </td>
                            <td style="text-align:right;">
                                <asp:Label ID="lblEmergency" runat="server" ForeColor="#FF33CC" Font-Bold="True"></asp:Label>
                            </td>
                          <%--  <td>
                                <asp:Label ID="lbltMpsttsCd_1" runat="server" Text="仮設置進捗状況" Width="100px"></asp:Label>
                                <asp:Label ID="lbltMpsttsCd" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblStatusCd_1" runat="server" Text="本設置進捗状況" Width="100px"></asp:Label>
                                <asp:Label ID="lblStatusCd" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblReqState_1" runat="server" Text="資料請求状況" Width="100px"></asp:Label>
                                <asp:Label ID="lblReqState" runat="server" Text=""></asp:Label>
                            </td>--%>
                        </tr>
                    </table>
                    <table border="0" style="width:1050px;">
                        <tr>
                            <td style="width: 300px"></td>
                            <td style="width: 250px">
                                <asp:Label ID="lbltMpsttsCd_1" runat="server" Text="仮設置進捗状況" Width="100px"></asp:Label>
                                <asp:Label ID="lbltMpsttsCd" runat="server" Text=""></asp:Label>
                            </td>
                                <td style="width: 250px">
                                <asp:Label ID="lblStatusCd_1" runat="server" Text="本設置進捗状況" Width="100px"></asp:Label>
                                <asp:Label ID="lblStatusCd" runat="server" Text=""></asp:Label>
                            </td>
                                  <td style="width: 250px">
                                <asp:Label ID="lblReqState_1" runat="server" Text="資料請求状況" Width="100px"></asp:Label>
                                <asp:Label ID="lblReqState" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width:1050px;">
                        <tr>
                            <td style="width: 200px;padding-left:4px;">
                                <asp:Label ID="lblRequestNo_1" runat="server" Text="依頼番号" Width="85px"></asp:Label>
                                <asp:Label ID="lblRequestNo_2" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="("></asp:Label>
                                <asp:Label ID="lblNumberOfUpdates" runat="server" Text=""></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text=")"></asp:Label>&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblReceptionDt_1" runat="server" Text="受付日付" Width="60px"></asp:Label>
                                <asp:Label ID="lblReceptionDt_2" runat="server" Text="" Width="70px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblReceptionTm_1" runat="server" Text="受付時間" Width="60px"></asp:Label>
                                <asp:Label ID="lblReceptionTm_2" runat="server" Text="" Width="70px"></asp:Label>
                            </td>
                            <td style="width:190px;">
                                <table>
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtSpecificationConnectingDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="仕様連絡区分" ppWidth="15" ppNum="True"  ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSpecificationConnectingDivision" runat="server" Text="キャンセル登録" Width="110px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtNLDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ＮＬ区分" ppWidth="15" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width:1180px;">
                        <tr>
                            <td style="width: 280px;">
                                <uc:ClsCMTextBoxTwo ID="ttwNotificationNo" runat="server" ppIMEModeOne="半角_変更不可" ppIMEModeTwo="半角_変更不可" ppMaxLengthOne="5" ppMaxLengthTwo="8" ppName="通知番号" ppNameWidth="80" ppWidthOne="50" ppWidthTwo="80" ppCheckHanOne="True" ppCheckHanTwo="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 430px;">
                                <uc:ClsCMTextBox ID="txtCurrentSys" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="40" ppName="現行システム" ppWidth="300" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width:172px;">
                                <uc:ClsCMTextBox ID="txtVer" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="ＶＥＲ" ppWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtSysClassification" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="システム分類" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSysClassification" runat="server" Text="ＬＵＴＥＲＮＡ" Width="110px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width:1050px;">
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="80" ppWidth="80" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleNm" runat="server" ppMaxLength="30" ppName="ホール名" ppWidth="410" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtTboxLine" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="ＴＢＯＸ回線" ppWidth="160" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width:1050px;">
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="7" ppName="ホールコード" ppNameWidth="80" ppWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="ＴＥＬ" ppWidth="110" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtPersonInCharge_1" runat="server" ppMaxLength="30" ppName="ホール責任者" ppWidth="280" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtPersonInCharge_2" runat="server" ppMaxLength="30" ppName="NGC営業担当者" ppWidth="250" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="width:100%;">
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtAddress" runat="server" ppMaxLength="100" ppName="住所" ppNameWidth="80" ppWidth="500" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="align-top" style="width:450px;">
                <asp:Panel ID="Panel2" runat="server">
                    <table style="width:100%;" border="0">
                        <tr class="text-center">
                            <td>&nbsp;</td>
                            <td style="width: 30px">
                                <asp:Label ID="lblNew" runat="server" Text="新規" Width="20px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblExpansion" runat="server" Text="増設" Width="20px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblSomeRemoval" runat="server" Text="一部撤去" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblShopRelocation" runat="server" Text="店舗移設" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblAllRemoval" runat="server" Text="全　撤去" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblOnceRemoval" runat="server" Text="一時撤去" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblReInstallation" runat="server" Text="再　設置" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblConChange" runat="server" Text="構成変更" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblConDelively" runat="server" Text="構成配信" Width="30px"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                <asp:Label ID="lblOther" runat="server" Text="その他" Width="30px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblHoleCns" runat="server" Text="ホール内工事"></asp:Label>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleNew" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleExpansion" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleSomeRemoval" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleShopRelocation" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleAllRemoval" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleOnceRemoval" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleReInstallation" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleConChange" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleConDelively" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtHoleOther" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblLanCns" runat="server" Text="ＬＡＮ工事"></asp:Label>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanNew" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanExpansion" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanSomeRemoval" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanShopRelocation" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanAllRemoval" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanOnceRemoval" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanReInstallation" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanConChange" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanConDelively" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtLanOther" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppNameVisible="False" ppWidth="15" ppNum="True"   ppCheckHan="True" ppCheckLength="True"  ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="Panel3" runat="server">
                    <table style="width:620px;" border="0">
                        <tr>
                            <td style="width: 260px;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:130px;">
                                            <uc:ClsCMTextBox ID="txtStkHoleIn" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ストッカーホール内" ppNameWidth="120" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStkHoleIn" runat="server" Text="該当" Width="40"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:130px;">
                                            <uc:ClsCMTextBox ID="txtSfOperation" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="エフエス稼動有無" ppNameWidth="110" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSfOperation" runat="server" Text="あり" Width="40"></asp:Label>
                                        </td>
                                    </tr>
                                </table> 
                            </td>
                            <td style="width: 50px">
                                <uc:ClsCMTextBox ID="txtEMoneyIntroduction" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="Ｅマネー導入" ppNameWidth="110" ppWidth="15" ppCheckHan="True" ppNum="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" Visible="false" />
                            </td>
                            <td style="width: 50px">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtStkHoleOut" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ストッカーホール外" ppNameWidth="120" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStkHoleOut" runat="server" Text="該当" Width="40"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtTboxTakeawayDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ＴＢＯＸ持帰区分" ppNameWidth="110" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxTakeawayDivision" runat="server" Text="あり" Width="40"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtEMoneyIntroductionCns" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="Ｅマネー導入工事" ppNameWidth="110" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" Visible="false" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <uc:ClsCMDateBox ID="dttEMoneyTestDt" runat="server" ppName="Ｅマネーテスト日付" ppNameWidth="120" ppMesType="アスタ" ppValidationGroup="1" visible="false" />
                            </td>
                            <td colspan="2" style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtEMoneyTestTm" runat="server" ppName="Ｅマネーテスト時間" ppNameWidth="120" ppMesType="アスタ" ppValidationGroup="1" Visible ="false" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <%--<uc:ClsCMTextBox ID="txtOtherContents" runat="server" ppMaxLength="40" ppName="その他内容" ppNameWidth="70" ppWidth="540" ppMesType="アスタ" ppValidationGroup="1" />--%>
                            </td>
                        </tr>
                    </table>

                </asp:Panel>

            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table border="0" style="width: 1050px;">
                    <tr>
                        <td style="width: 400px"></td>
                        <td style="width: 600px">
                            <uc:ClsCMTextBox ID="txtOtherContents" runat="server" ppMaxLength="40" ppName="その他内容" ppNameWidth="70" ppWidth="540" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel4" runat="server">
                    <table style="width: 1050px;" border="0">
                        <tr>
                            <td>
                                <table style="width:100px;">
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtTwinsShop" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="双子店" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTwinsShop" runat="server" Text="双子店" Width="40px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table style="width:100px;">
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtIndependentCns" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="工事区分" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIndependentCns" runat="server" Text="単独工事" Width="60"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtSameTimeCnsNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="同時工事数" ppWidth="20" ppNum="True" ppCheckHan="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <table style="width:135px;">
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="txtPAndCDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="親子区分" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPAndCDivision" runat="server" Text="子ホール" Width="60"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtParentHoleCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="7" ppName="親ホールコード" ppWidth="70" ppCheckHan="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <asp:Label ID="lblConstructionExistence" runat="server" Text="工事有無"></asp:Label>
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtConstructionExistenceF1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="Ｆ1" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtConstructionExistenceF2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="Ｆ2" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtConstructionExistenceF3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="Ｆ3" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtConstructionExistenceF4" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="Ｆ4" ppWidth="15" ppNum="True" ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="Panel5" runat="server">
                    <table style="width:1050px;" border="0">

                        <tr>
                            <td class="auto-style19">
                                <uc:ClsCMTextBox runat="server" ID="txtOfficCD" ppName="営業所コード" ppNameWidth="100" ppWidth="50" ppMaxLength="5" ppIMEMode="半角_変更不可" ppRequiredField="True" ppShowCode="0" ppMesType="アスタ" ppValidationGroup="1" ppCheckHan="True" />
                            </td>
                            <td>
                                <asp:Button ID="btnTrader" runat="server" Text="参照" CausesValidation="False" />
                            </td>
                            <td>
                                <asp:Label ID="lblSendNmN" runat="server" Text="営業所名" Width="80"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSendNmV" runat="server" Text="" Width="200"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width:1050px;" border="0">

                        <tr>
                            <td class="auto-style19" >
                                <uc:ClsCMDateBox ID="dttStartOfCon" runat="server" ppName="工事開始" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style7"style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtStartOfCon" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style13" >
                                <uc:ClsCMDateBox ID="dttLastOpenDt" runat="server" ppName="最終営業日" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>
                                <uc:ClsCMDateBox ID="dttLastOpenDtT500" runat="server" ppName="最終営業日Ｔ５００" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width:1050px;" border="0">

                        <tr>
                            <td class="auto-style19" >
                                <uc:ClsCMDateBox ID="dttTest" runat="server" ppName="総合テスト" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style7" style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtTest" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <!--<td class="auto-style12" style="padding-bottom: 5px">
                                <uc:ClsCMTextBox ID="txtTestWorkPersonnel" runat="server" ppMaxLength="10" ppName="作業担当者" ppNameWidth="70" ppWidth="150" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>-->
                            <td class="auto-style23">
                                <asp:Label ID="lblPersonal1" runat="server" Text="作業担当者" Width="70px"></asp:Label>
                                 <asp:DropDownList ID="ddlPersonal1" runat="server" Width="200px"></asp:DropDownList>
                            </td>
                            <td class="auto-style14">
                                <uc:ClsCMDateBox ID="dttTestDepartureDt" runat="server" ppName="出発日時" ppNameWidth="60" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtTestDepartureTm" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width:1050px;" border="0">

                        <tr>
                            <td class="auto-style19" >
                                <uc:ClsCMDateBox ID="dttTemporaryInstallation" runat="server" ppName="仮設置" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style7" style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtTemporaryInstallation" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <!--<td class="auto-style11"style="padding-bottom: 5px" >
                                <uc:ClsCMTextBox ID="txtTemporaryInstallationWorkPersonnel" runat="server" ppMaxLength="10" ppName="作業担当者" ppNameWidth="70" ppWidth="150" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>-->
                            <td class="auto-style23">
                                <asp:Label ID="lblPersonal2" runat="server" Text="作業担当者" Width="70px"></asp:Label>
                                 <asp:DropDownList ID="ddlPersonal2" runat="server"  Width="200px"></asp:DropDownList>
                            </td>
                            <td class="auto-style15">
                                <uc:ClsCMDateBox ID="dttTemporaryInstallationDepartureDt" runat="server" ppName="出発日時" ppNameWidth="60" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtTemporaryInstallationDepartureTm" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width:1050px;" border="0">
                        <tr>
                            <td style="width: 150px">
                                <uc:ClsCMTextBox ID="dttTemporaryInstallationCnsDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="仮設置工事区分" ppNameWidth="95" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" />
                            </td>
                            <td colspan="7">
                                <uc:ClsCMTextBox ID="dttTemporaryInstallationDtNotInputDivision0" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="仮設置日未入力区分" ppNameWidth="120" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width:1050px;" border="0">

                        <tr>
                            <td class="auto-style19" >
                                <uc:ClsCMDateBox ID="dttPolice" runat="server" ppName="警察" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style7" style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtPolice" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 130px ;padding-bottom: 5px">
                                <uc:ClsCMTextBox ID="dttShiftCnsDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="移行工事区分" ppNameWidth="80" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="padding-bottom: 5px">
                                <uc:ClsCMTextBox ID="dttShiftCnsWorkDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="移行工事作業区分" ppNameWidth="100" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width:1050px;" border="0">

                        <tr>
                            <td class="auto-style19" >
                                <uc:ClsCMDateBox ID="dttOpen" runat="server" ppName="オープン" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style7" style="padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtOpen" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style13" >
                                <uc:ClsCMDateBox ID="dttLanCns" runat="server" ppName="ＬＡＮ工事" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 100px ; padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtLanCns" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblRegisteredEmployeesNm_1" runat="server" Text="登録社員名" Width="80px"></asp:Label>
                                <asp:Label ID="lblRegisteredEmployeesNm_2" runat="server" Text=""></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width:1230px;" border="0">

                        <tr>
                             <td style="width: 188px">
                                <uc:ClsCMDateBox ID="dttVerup" runat="server" ppName="ＶＥＲＵＰ" ppNameWidth="70" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                             <td style="width: 85px ; padding-bottom: 5px">
                                <uc:ClsCMTimeBox ID="tmtVerup" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="padding-bottom:5px;width:145px;">
                                <table>
                                    <tr>
                                        <td>
                                            <uc:ClsCMTextBox ID="dttVerupDtDivision" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ＶＥＲＵＰ日付区分" ppNameWidth="120" ppWidth="15"  ppNum="True"   ppCheckHan="True" ppCheckLength="True" ppMesType="アスタ" ppValidationGroup="1" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVerupDtDivision" runat="server" Text="単独" Width="40px" ></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="padding-bottom: 5px;width:530px;">
                                <uc:ClsCMTextBox ID="txtVerupCnsType_1" runat="server" ppMaxLength="30" ppName="ＶＥＲＵＰ工事種類" ppNameWidth="120" ppWidth="410" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="padding-bottom: 5px">
                                <uc:ClsCMTextBox ID="txtVerupCnsType_2" runat="server" ppMaxLength="10" ppNameVisible="False" ppWidth="150" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="Panel6" runat="server">
                    <table style="width: 1050px;" border="0">
                        <tr>
                            <td colspan="2" class="auto-style24">
                                <uc:ClsCMTextBox ID="txtAgencyCd" runat="server" ppMaxLength="5" ppIMEMode="半角_変更不可" ppName="代理店" ppNameWidth="80" ppWidth="50" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td colspan="2">
                                <uc:ClsCMTextBoxRef ID="trfAgencyNm" runat="server" ppMaxLength="40" ppNameVisible="False" ppWidth="540" ppURL="/~COMSELP002.aspx&quot;" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style25"></td>
                            <td colspan="2"></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <table style="width: 1050px;" border="0">
                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td style="width: 140px">
                                <uc:ClsCMTextBox ID="txtAgencyTel" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppMesType="アスタ" ppName="ＴＥＬ" ppValidationGroup="1" ppWidth="110" ppNameWidth="65" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtAgencyPersonnel" runat="server" ppMaxLength="30" ppMesType="アスタ" ppName="担当者" ppValidationGroup="1" ppWidth="410" ppNameWidth="65" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td colspan="8">
                                <uc:ClsCMTextBox ID="txtAgencyAdd" runat="server" ppMaxLength="100" ppName="住所" ppNameWidth="65" ppWidth="500" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtAgencyResponsible" runat="server" ppMaxLength="30" ppName="責任者" ppWidth="410" ppNameWidth="65" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtAgencyShop" runat="server" ppMaxLength="5" ppIMEMode="半角_変更不可" ppName="代行店" ppNameWidth="80" ppWidth="50" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 298px">
                                <uc:ClsCMTextBoxRef ID="trfAgencyShop" runat="server" ppMaxLength="40" ppNameVisible="False" ppWidth="540" ppURL="/~COMSELP002.aspx&quot;" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 527px"></td>
                            <%--                            <td>
                                <uc:ClsCMTextBox ID="txtAgencyShopTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="ＴＥＬ" ppWidth="110" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td colspan="2">
                                <uc:ClsCMTextBox ID="txtAgencyShopPersonnel" runat="server" ppMaxLength="30" ppName="担当者" ppWidth="410" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>--%>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">
                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td style="width: 140px">
                                <uc:ClsCMTextBox ID="txtAgencyShopTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="ＴＥＬ" ppWidth="110" ppMesType="アスタ" ppValidationGroup="1" ppNameWidth="65" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtAgencyShopPersonnel" runat="server" ppMaxLength="30" ppName="担当者" ppWidth="410" ppMesType="アスタ" ppValidationGroup="1" ppNameWidth="65" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td colspan="8">
                                <uc:ClsCMTextBox ID="txtAgencyShopAdd" runat="server" ppMaxLength="100" ppName="住所" ppNameWidth="65" ppWidth="500" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtAgencyShopResponsible" runat="server" ppMaxLength="30" ppName="責任者" ppWidth="410" ppNameWidth="65" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtSendingStation" runat="server" ppMaxLength="5" ppIMEMode="半角_変更不可" ppName="ＬＡＮ送付先" ppNameWidth="80" ppWidth="50" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 298px">
                                <uc:ClsCMTextBoxRef ID="trfSendingStation" runat="server" ppMaxLength="40" ppNameVisible="False" ppWidth="540" ppURL="/~COMSELP002.aspx&quot;" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td style="width: 527px"></td>
                            <%--                            <td colspan="2" class="auto-style4">
                                <uc:ClsCMTextBox ID="txtSendingStationResponsible" runat="server" ppMaxLength="30" ppName="責任者" ppWidth="410" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style5">
                                <uc:ClsCMDateBox ID="tdtDeliveryPreferredDt" runat="server" ppName="納入希望日" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td colspan="2" class="auto-style4">
                                <uc:ClsCMTimeBox ID="ClsCMTimeBox1" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td class="auto-style4"></td>--%>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">
                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td>
                                <uc:ClsCMTextBox ID="txtSendingStationResponsible" runat="server" ppMaxLength="20" ppName="責任者" ppWidth="410" ppMesType="アスタ" ppValidationGroup="1" ppNameWidth="65" ppIMEMode="全角" />
                            </td>
                            <td style="padding-top: 8px">
                                <uc:ClsCMDateBox ID="tdtDeliveryPreferredDt" runat="server" ppName="納入希望日" ppValidationGroup="1" ppNameWidth="65" ppMesType="アスタ" />
                            </td>
                            <td>
                                <uc:ClsCMTimeBox ID="ClsCMTimeBox1" runat="server" ppNameVisible="False" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                            <td style="width: 200px">&nbsp;</td>
                        </tr>
                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td style="width: 88px">&nbsp;</td>
                            <td style="width: 550px">
                                <uc:ClsCMTextBox ID="txtSendingStationAdd" runat="server" ppMaxLength="100" ppName="住所" ppNameWidth="65" ppWidth="500" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtSendingStationTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="16" ppName="ＴＥＬ" ppWidth="110" ppMesType="アスタ" ppValidationGroup="1" />
                            </td>
                        </tr>

                    </table>
                </asp:Panel>

                <asp:Panel ID="Panel7" runat="server">
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td colspan="9">
                                <uc:ClsCMTextBox ID="txtRemarks" runat="server" ppMaxLength="100" ppName="備考" ppNameWidth="80" ppWidth="1040" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td colspan="9">
                                <uc:ClsCMTextBox ID="txtSpecificationsRemarks" runat="server" ppMaxLength="100" ppName="仕様備考" ppNameWidth="80" ppWidth="1040" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>

                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtImportantPoints1" runat="server" ppMaxLength="100" ppName="注意事項" ppNameWidth="80" ppWidth="1040" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <table style="width: 1050px;" border="0">

                        <tr>
                            <td colspan="9">
                                <uc:ClsCMTextBox ID="txtImportantPoints2" runat="server" ppMaxLength="100" ppName="連絡事項" ppNameWidth="80" ppWidth="1040" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel ID="Panel8" runat="server">
                    <%--<div class="grid-out">
                        <div id="DivOut" runat="server" class="grid-out">
                            <div id="DivIn" runat="server" class="grid-in">
                                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                                <asp:GridView ID="grvList" runat="server">
                                </asp:GridView>
                            </div>
                        </div>
                    </div>--%>
                    <div id="DivOut" runat="server" class="grid-out" style="width: 1140px">
                        <div id="DivIn" runat="server" class="grid-in" style="width: 1140px">
                            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="grvList" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="Panel9" runat="server">
                    <table style="width: 100%;" border="0">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblControlInfo" runat="server" Text="制御情報"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="width: 500px">
                                <asp:CheckBox ID="cbxDllSettingChange" runat="server" Text="ＤＬＬ設定変更なし" TextAlign="Left" />
                            </td>
                            <td style="width: 60px">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <uc:ClsCMTextBoxPopup ID="trfSfPersonInCharge" runat="server" ppMaxLength="20" ppName="エフエス担当者" ppNameWidth="100" ppWidth="280" ppShowCode="2" ppMesType="アスタ" ppValidationGroup="1" ppIMEMode="全角"/>
                                <%--<uc:ClsCMTextBoxRef ID="trfSfPersonInCharge" runat="server" ppMaxLength="20" ppName="エフエス担当者" ppNameWidth="100" ppWidth="280" />--%>
                            </td>
                            <td>
                                <asp:Label ID="lblProcessingResult" runat="server" Text="処理結果"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Selected="True">ＯＫ</asp:ListItem>
                                    <asp:ListItem Value="2" Selected="False">ＮＧ</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtProcessingResultDetail1" runat="server" ppMaxLength="100" ppName="処理結果詳細" ppNameWidth="100" ppWidth="1010" ppMesType="アスタ" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtProcessingResultDetail2" runat="server" ppMaxLength="100" ppNameWidth="100" ppWidth="1010" ppName="　" ppMesType="アスタ" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtProcessingResultDetail3" runat="server" ppMaxLength="50" ppNameWidth="100" ppWidth="1010" ppName="　" ppMesType="アスタ" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtControlInfoRemarks1" runat="server" ppMaxLength="100" ppName="備考" ppNameWidth="100" ppWidth="1010" ppMesType="アスタ" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtControlInfoRemarks2" runat="server" ppMaxLength="100" ppNameWidth="100" ppWidth="1010" ppName="　" ppMesType="アスタ" ppIMEMode="全角" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                <uc:ClsCMTextBox ID="txtControlInfoRemarks3" runat="server" ppMaxLength="50" ppNameWidth="100" ppWidth="1010" ppName="　" ppMesType="アスタ" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
                <td>
                    <div class="float-left">
                        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
                    </div>
                </td>
            </tr>
    </table>
</asp:Content>
