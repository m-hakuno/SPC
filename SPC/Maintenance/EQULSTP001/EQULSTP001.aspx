<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="EQULSTP001.aspx.vb" Inherits="SPC.EQULSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <style type="text/css">
     <!--
        .new-row input[type="text"] {
            color: #0000FF;
        }
        #cphMainContent_cphUpdateContent_ddlDelivery_pnlName
        {
            width: 0px !important;
        }
        #cphMainContent_cphUpdateContent_ddlPoint_pnlName
        {
            width: 0px !important;
        }
    --> 
     </style>
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
    <uc:ClsCMDateBoxFromTo runat="server" ID="dftMntDT" ppName="発送日" ppNameWidth="100" />
    <uc:ClsCMDateBoxFromTo runat="server" ID="dftArvDT" ppName="到着日" ppNameWidth="100" />
    <uc:ClsCMTextBoxFromTo runat="server" ID="tftMntNo" ppName="管理番号" ppNameWidth="100" ppMaxLength="14" ppIMEMode="半角_変更不可" />
    <table border="0">
        <tr style="vertical-align: top">
            <td style="position: relative; top: 3px">
                <asp:Panel ID="pnlSituationNm" runat="server" Width="100">
                    <asp:Label ID="lblSituation" runat="server" Text="進捗状況"></asp:Label>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlSituation" runat="server">
                    <asp:DropDownList ID="ddlSituation" runat="server">
                    </asp:DropDownList>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlSituationErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvSituation" runat="server" ControlToValidate="ddlSituation" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table border="0">
        <tr>
            <td style="position: relative; top: 3px">
                <asp:Panel ID="pnlPoint" runat="server" Width="93">
                    <asp:Label ID="lblSPoint" runat="server" Text="配送先拠点名"></asp:Label>
                </asp:Panel>
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlSPoint" ppWidth="100" ppMode="名称" ppClassCD="0115" ppNotSelect="True" ppName="配送先拠点名" ppNameVisible="false" />
            </td>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtSPoint" ppName="配送先拠点名" ppNameVisible="False" ppIMEMode="半角_変更不可" ppMaxLength="10" ppCheckHan="True" ppWidth="85" />
            </td>
            <td>
                <asp:Label ID="lblPointNm" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <div style="border-style: solid; border-width: 1px; display: table; width: 100%;">
        <table class="center">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table border="1" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="機器分類"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="機器種別"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="ＴＢＯＸ種別"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="ＶＥＲ"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="型式/機器"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="ＨＤＤＮＯ．"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="ＨＤＤ種別"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label8" runat="server" Text="シリアルＮＯ．"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label10" runat="server" Text="配送元"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="発送日"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label9" runat="server" Text="配送先拠点名"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="到着予定日"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="到着日"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label14" runat="server" Text="管理番号"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="進捗状況"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="Label16" runat="server" Text="備考"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--機器分類--%>
                                        <asp:Panel ID="pnlAppaClass" runat="server">
                                            <asp:DropDownList ID="ddlAppaClass" runat="server" AutoPostBack="True" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlAppaClassErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvAppaClass" runat="server" ControlToValidate="ddlAppaClass" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <%--機器種別--%>
                                        <asp:Panel ID="pnlAppaClassCD" runat="server">
                                            <asp:DropDownList ID="ddlAppaClassCD" runat="server" AutoPostBack="True" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlAppaClassCDErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvAppaClassCD" runat="server" ControlToValidate="ddlAppaClassCD" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <%--ＴＢＯＸタイプ--%>
                                        <asp:Panel ID="pnlTboxType" runat="server">
                                            <asp:DropDownList ID="ddlTboxType" runat="server" AutoPostBack="True" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlTboxTypeErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvTboxType" runat="server" ControlToValidate="ddlTboxType" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <%--ＶＥＲ--%>
                                        <asp:Panel ID="pnlTboxVer" runat="server">
                                            <uc:ClsCMTextBox runat="server" ID="TxtTboxVer" ppName="ＶＥＲ" ppNameVisible="False" ppValidationGroup="Dtl" ppMaxLength="5" ppWidth="150" ppCheckHan="True" ppIMEMode="半角_変更不可" />
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <%--型式／機器--%>
                                        <asp:Panel ID="pnlAppaNM" runat="server">
                                            <asp:DropDownList ID="ddlAppaNM" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlAppaNMErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvAppaNM" runat="server" ControlToValidate="ddlAppaNM" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <%--ＨＤＤＮＯ--%>
                                        <asp:Panel ID="pnlHDDNo" runat="server">
                                            <asp:DropDownList ID="ddlHDDNo" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlHDDNoErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvHDDNo" runat="server" ControlToValidate="ddlHDDNo" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--ＨＤＤ種別--%>
                                        <asp:Panel ID="pnlHDDCls" runat="server">
                                            <asp:DropDownList ID="ddlHDDCls" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlHDDClsErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvHDDCls" runat="server" ControlToValidate="ddlHDDCls" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td colspan="2">
                                        <%--シリアルＮＯ--%>
                                        <uc:ClsCMTextBox runat="server" ID="txtSerialNo" ppName="シリアルＮＯ" ppNameVisible="False" ppValidationGroup="Dtl" ppMaxLength="30" ppWidth="300" ppIMEMode="半角_変更不可" ppCheckHan="true"/>
                                    </td>
                                    <td colspan="2">
                                        <%--配送元--%>
                                        <table>
                                            <tr>
                                                <td style="width:105px;">
                                                    <uc:ClsCMDropDownList runat="server" ID="ddlDelivery" ppWidth="100" ppMode="名称" ppClassCD="0115" ppNotSelect="True" ppNameWidth="20" ppName="配送元" ppNameVisible="false" ppValidationGroup="Dtl" />
                                                </td>
                                                <td>
                                                    <uc:ClsCMTextBox runat="server" ID="txtDelivery" ppName="配送元" ppNameVisible="False" ppIMEMode="半角_変更不可" ppMaxLength="10" ppCheckHan="True" ppWidth="85" ppValidationGroup="Dtl" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblDelivery" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <%--発送日--%>
                                        <uc:ClsCMDateBox runat="server" ID="dtbDeliveryDT" ppName="発送日" ppNameVisible="False" ppValidationGroup="Dtl" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%--配送先拠点名--%>
                                        <table>
                                            <tr>
                                                <td style="width:105px;">
                                                    <uc:ClsCMDropDownList runat="server" ID="ddlPoint" ppWidth="100" ppMode="名称" ppClassCD="0115" ppNotSelect="True" ppNameWidth="20" ppName="配送先拠点名" ppNameVisible="false" ppValidationGroup="Dtl" />
                                                </td>
                                                <td>
                                                    <uc:ClsCMTextBox runat="server" ID="txtPoint" ppName="配送先拠点名" ppNameVisible="False" ppIMEMode="半角_変更不可" ppMaxLength="10" ppCheckHan="True" ppWidth="85" ppValidationGroup="Dtl" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblPoint" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <%--致着予定日--%>
                                        <uc:ClsCMDateBox runat="server" ID="dtbArrivePlanDT" ppName="致着予定日" ppNameVisible="False" ppValidationGroup="Dtl" />
                                    </td>
                                    <td>
                                        <%--致着日--%>
                                        <uc:ClsCMDateBox runat="server" ID="dtbArriveDT" ppName="致着日" ppNameVisible="False" ppValidationGroup="Dtl" />
                                    </td>
                                    <td colspan="2">
                                        <%--管理番号--%>
                                        <uc:ClsCMTextBox runat="server" ID="txtMntNo" ppName="管理番号" ppNameVisible="False" ppValidationGroup="Dtl" ppMaxLength="15" ppWidth="300" ppIMEMode="半角_変更不可" ppCheckHan="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--進捗状況--%>
                                        <asp:Panel ID="pnlStatus" runat="server">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlStatusErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Dtl"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td colspan="5">
                                        <%--備考--%>
                                        <uc:ClsCMTextBox ID="txtNotes" runat="server" ppName="備考" ppHeight="40" ppWidth="800" ppTextMode="MultiLine" ppMaxLength="200"  ppValidationGroup="Dtl" ppNameVisible="False" ppWrap="true" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <table>
                        <tr>
                            <td><asp:Label ID="lblPbrnNo" runat="server" Text="Label" Visible="false"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                         <tr>
                            <td style="white-space: nowrap">
                                <asp:Button ID="btnAdd" runat="server" Text="登録" ValidationGroup="Dtl" />
                                <asp:Button ID="btnUpdate" runat="server" Text="更新" ValidationGroup="Dtl" />
                                <asp:Button ID="btnDel" runat="server" Text="削除" CausesValidation="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" CssClass="float-right">
                                    <asp:Button ID="btnClear" runat="server" Text="クリア" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Dtl" CssClass="errortext" />
        </ContentTemplate>
    </asp:UpdatePanel>

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
