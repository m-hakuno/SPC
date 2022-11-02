<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CNSUPDP004.aspx.vb" Inherits="SPC.CNSUPDP004" MaintainScrollPositionOnPostback="true" %>

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
            } else if ((parseInt(size) + 10) < lenW) {
                var limitS = strW.substring(0, (parseInt(size) + 10));
                obj.value = limitS;
            }
        }
    </script>
    <style type="text/css">
        .auto-style2
        {
            float: left;
            width: 943px;
        }

        .auto-style4
        {
            width: 250px;
            height: 49px;
        }

        .auto-style5
        {
            width: 400px;
            height: 49px;
        }

        .auto-style6
        {
            width: 55px;
            height: 49px;
        }

        .auto-style7
        {
            height: 49px;
        }

        .auto-style8
        {
            text-align: center;
            height: 17px;
        }

        .auto-style9
        {
            text-align: center;
            height: 22px;
        }

        .auto-style10
        {
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="pnlSPC" runat="server">
        <%--<table style="width:100%;">
            <tr>
                <td class="auto-style8">
                    <asp:Label ID="lblSpcDescription" runat="server" Text="ＳＰＣ記述"></asp:Label>
                </td>
            </tr>
        </table>--%>
        <table style="width: 1205px; border-spacing: 6px 3px; border: 1px solid #000;" class="center">
            <tr>
                <td>
                    <asp:Label ID="lblCommNo1" runat="server" Text="連絡票管理番号" Width="100px"></asp:Label>
                </td>
                <td style="width: 100px; padding-left: 8px;" class="text-left">
                    <asp:Label ID="lblCommNo2" runat="server" Text="123456789012345"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCharge" runat="server" Text="作成者"></asp:Label>
                </td>
                <td colspan="5" style="padding-left: 6px;">
                    <uc:ClsCMDropDownList ID="ddlCreateUser" runat="server" ppName="作成者" ppNameWidth="0" ppWidth="260" ppClassCD="" ppRequiredField="True" ppEnabled="True" ppNameVisible="False" ppValidationGroup="ValMain" />
                </td>
            </tr>
            <tr>
                <td style="width: 120px; text-align: left">
                    <asp:Label ID="lblTboxId1" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
                </td>
                <td style="text-align: left; padding-left: 8px;">
                    <asp:Label ID="lblTboxId2" runat="server" Text="12345678901234567890"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSystem" runat="server" Text="システム"></asp:Label>
                </td>
                <td style="padding-left: 6px;">
                    <uc:ClsCMDropDownList ID="ddlSystem" runat="server" ppName="システム" ppNameWidth="0" ppWidth="120" ppClassCD="" ppRequiredField="True" ppEnabled="True" ppNameVisible="False" ppValidationGroup="ValMain" />

                </td>
                <td style="padding-left: 6px;">
                    <asp:Label ID="lblTPCtitle" runat="server" Text="登録ｼｽﾃﾑ"></asp:Label>
                </td>
                <td style="padding-left: 6px;">
                    <asp:Label ID="lblTPCName" runat="server" Text="TPC(500)"></asp:Label>
                </td>
                <td colspan="2" style="padding-left: 6px;">
                    <asp:Label ID="lblTPCVer" runat="server" Text="1.001"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;">
                    <asp:Label ID="lblHallNm1" runat="server" Text="ホール名"></asp:Label>
                </td>
                <td colspan="7" style="text-align: left; padding-left: 6px;">
                    <asp:Label ID="lblHallNm2" runat="server" Text="ホール1"></asp:Label>
                </td>
            </tr>
            <tr class="text-left">
                <td>
                    <asp:Label ID="lblCnstNo" runat="server" Text="工事依頼番号"></asp:Label>
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtCnstNo" runat="server" ppMaxLength="14" ppNameVisible="False" ppWidth="150px" ppEnabled="True" ppName="工事依頼番号" ppCheckHan="True" ppNum="False" ppRequiredField="True" ppValidationGroup="ValMain" />
                </td>
                <td>
                    <asp:Label ID="lblSetCls" runat="server" Text="設置区分" Width="80px"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 8px;">
                    <asp:DropDownList ID="ddlSetCls" runat="server" Width="100px"></asp:DropDownList>
                </td>
                <td style="width: 80px;">
                    <asp:Label ID="lblConstD" runat="server" Text="作業実施日"></asp:Label>
                </td>
                <td style="padding-top: 7px;">
                    <uc:ClsCMDateBox ID="dttConstD" runat="server" ppNameVisible="False" ppName="作業実施日" ppRequiredField="True" ppValidationGroup="ValMain" />
                </td>
                <td style="width: 92px">
                    <asp:Label ID="lblCnstT" runat="server" Text="作業実施時間" Width="90px"></asp:Label>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtCnstH" runat="server" ppCheckHan="True" ppIMEMode="半角_変更不可" ppMaxLength="2" ppNameVisible="False" ppWidth="14" ppName="作業実施時間" ppValidationGroup="ValMain" ppCheckLength="False" ppNum="true" />
                            </td>
                            <td>:
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtCnstM" runat="server" ppCheckHan="True" ppIMEMode="半角_変更不可" ppMaxLength="2" ppNameVisible="False" ppWidth="14" ppName="作業実施時間" ppValidationGroup="ValMain" ppCheckLength="False" ppNum="true" />
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>

            <tr>
                <td style="width: 100px; vertical-align: top;">
                    <asp:Label ID="lblConstructionDiv" runat="server" Text="工事区分" Width="70px"></asp:Label>

                </td>
                <td colspan="7" style="width: 800px; padding-left: 7px;">
                    <table class="left" style="width: 770px; border-collapse: collapse;" border="1">
                        <tr class="text-center" style="height: 22px;">
                            <td style="width: 70px">
                                <asp:Label ID="lblNew_1" runat="server" Text="新規"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblExpansion_1" runat="server" Text="増設"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblSomeRemoval_1" runat="server" Text="一部撤去"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblShopRelocation_1" runat="server" Text="店舗移設"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblAllRemoval_1" runat="server" Text="全撤去"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblOnceRemoval_1" runat="server" Text="一時撤去"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblConChange_1" runat="server" Text="構成変更"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblConDelively_1" runat="server" Text="構成配信"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblReInstallation_1" runat="server" Text="再設置"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblVerup_1" runat="server" Text="VUP"></asp:Label>
                            </td>
                            <td style="width: 70px">
                                <asp:Label ID="lblOther_1" runat="server" Text="その他"></asp:Label>
                            </td>

                        </tr>
                        <tr class="text-center" style="height: 22px;">
                            <td>
                                <asp:Label ID="lblNew2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAdd2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrtRemove2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblRelocate2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAllRemove2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTmpRemove2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblChngOrgnz2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDlvOrgnz2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblReset2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVup2" runat="server">X</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblOth2" runat="server">X</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style9">
                                <asp:Label ID="lblOtherContent_1" runat="server" Text="その他内容"></asp:Label>
                            </td>
                            <td colspan="10" class="auto-style10">
                                <asp:Label ID="lblOthDtl" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr class="text-left">
                <td>
                    <asp:Label ID="lblSTestCharge" runat="server" Text="ＦＳ担当者"></asp:Label>
                </td>
                <td>
                    <%--<uc:ClsCMTextBox ID="txtFSCharge" runat="server" ppMaxLength="20" ppNameVisible="False" ppWidth="200" ppName="ＦＳ担当者" ppValidationGroup="ValMain" Visible="False"/>--%>
                    <uc:ClsCMDropDownList ID="ddlFSCharge" runat="server" ppName="ＦＳ担当者" ppNameWidth="0" ppWidth="200" ppClassCD="" ppEnabled="True" ppNameVisible="False" ppValidationGroup="ValMain" />
                </td>
                <td style="width: 100px;">
                    <asp:Label ID="lblHallCharge" runat="server" Text="ホール担当者名" Width="95px"></asp:Label>
                </td>
                <td colspan="2">
                    <uc:ClsCMTextBox ID="txtHallCharge" runat="server" ppMaxLength="30" ppNameVisible="False" ppWidth="200" ppName="ホール担当者名" ppRequiredField="False" ppValidationGroup="ValMain" />
                </td>
                <td style="width: 120px;">
                    <asp:Label ID="lblAgcyCharge" runat="server" Text="代理店担当者"></asp:Label>
                </td>
                <td colspan="2">
                    <uc:ClsCMTextBox ID="txtAgcyCharge" runat="server" ppMaxLength="30" ppNameVisible="False" ppWidth="200" ppName="代理店担当者" ppValidationGroup="ValMain" />
                </td>
            </tr>
        </table>
        <hr />
        <table>
            <tr>
                <td style="padding-left: 30px;">
                    <%--<uc:ClsCMTextBoxPopup ID="tbpCnfrm" runat="server" ppMaxLength="20" ppName="確認者" ppWidth="280" ppRequiredField="False" ppValidationGroup="ValMain" Visible ="false"/>--%>
                    <uc:ClsCMDropDownList ID="ddlCnfrm" runat="server" ppName="確認者" ppNameWidth="100" ppWidth="200" ppClassCD="" ppNameVisible="true" ppValidationGroup="ValMain" />
                </td>
                <%--<td>
                    <uc:ClsCMTextBoxPopup ID="tbpComm" runat="server" ppMaxLength="20" ppName="連絡者" ppWidth="280" ppRequiredField="False" ppValidationGroup="ValMain" visible = "false"/>
                    <uc:ClsCMDropDownList ID="ddlComm" runat="server" ppName="連絡者" ppNameWidth="50" ppWidth="260" ppClassCD="" ppRequiredField="True" ppEnabled="True" ppNameVisible="true" ppValidationGroup="ValMain" />
                </td>--%>
            </tr>
        </table>
        <hr />
        <%--<table class="center" style="width:100%;" border="0">
            <tr>
                <td class="text-center">
                    <asp:Label ID="lblWorkHistory" runat="server" Text="作業履歴"></asp:Label>
                </td>
            </tr>
        </table>--%>
        <table class="center" style="width: 1225px; border-spacing: 3px 3px; border: 1px solid #000; margin-top: 5px; margin-bottom: 10px;">
            <tr>
                <td colspan="2">
                    <table style="border-collapse: collapse;" border="1">
                        <tr class="text-center" style="background-color: #8DB4E2;">
                            <td style="width: 200px;">
                                <asp:Label ID="lblDt" runat="server" Text="日時"></asp:Label>
                            </td>
                            <td style="width: 200px;">
                                <asp:Label ID="lblChargeDtl" runat="server" Text="担当者"></asp:Label>
                            </td>
                            <td>

                                <asp:Label ID="lblContent" runat="server" Text="内容"></asp:Label>

                            </td>
                        </tr>
                        <tr class="text-center">
                            <td style="padding-top: 7px; padding-left: 10px;" class="auto-style3">
                                <uc:ClsCMDateTimeBox ID="dtbDt" runat="server" ppNameVisible="False" ppName="対応日時" ppOver="False" ppRequiredField="True" ppValidationGroup="Detail" />
                            </td>
                            <td style="padding-left: 15px;">
                                <%--<uc:ClsCMTextBox ID="txtChargeDtl" runat="server" ppMaxLength="20" ppNameVisible="False" ppWidth="80" ppName="担当者" ppValidationGroup="detail" Visible="False"/>--%>
                                <uc:ClsCMDropDownList ID="ddlChargeDtl" runat="server" ppName="担当者" ppNameWidth="0" ppWidth="160" ppClassCD="" ppRequiredField="True" ppEnabled="True" ppNameVisible="False" ppValidationGroup="Detail" />
                            </td>
                            <td>
                                <%--<uc:ClsCMTextBox ID="txtContent" runat="server" ppName="内容" ppHeight="40" ppWidth="680" ppTextMode="MultiLine" ppMaxLength="100" ppNameVisible="False" ppWrap="true" ppValidationGroup="Detail" ppIMEMode="全角" ppRequiredField="False"pCheckLength="False" />--%>
                                <uc:ClsCMTextBox ID="txtContent" runat="server" ppNameVisible="False" ppTextMode="MultiLine" ppWidth="680" ppMaxLength="100" ppName="内容" ppRequiredField="False" ppValidationGroup="Details" ppCheckLength="False" ppWrap="True" ppIMEMode="全角" />
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td style="width: 930px;">
                    <asp:ValidationSummary ID="vasDetail" runat="server" CssClass="errortext" ValidationGroup="Detail" />
                </td>
                <td style="width: 300px; text-align: right; vertical-align: bottom; padding-right: 5px;">

                    <asp:Button ID="btnClear" runat="server" Text="クリア" ValidationGroup="Detail" CausesValidation="False" />
                    &nbsp;&nbsp;
                <asp:Button ID="btnAddition" runat="server" Text="追加" ValidationGroup="Detail" />
                    <asp:Button ID="btnUpdateDtl" runat="server" Text="更新" ValidationGroup="Detail" />
                    <asp:Button ID="btnDelete" runat="server" Text="削除" ValidationGroup="Detail" CausesValidation="False" BackColor="#FF6666" />

                </td>
            </tr>
        </table>
        <%--        <div id="DivOut" runat="server" class="grid-out">
            <div id="DivIn" runat="server" class="grid-in">
                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvList" runat="server">
                </asp:GridView>
            </div>
        </div>--%>
        <div id="DivOut" runat="server" class="grid-out" style="width: 1225px; height: 228px; margin-left: auto; margin-right: auto;">
            <div id="DivIn" runat="server" class="grid-in" style="width: 1205px; height: 228px;">
                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvList" runat="server"></asp:GridView>
            </div>
        </div>
    </asp:Panel>
    <hr />
    <asp:Panel ID="pnlNGC" runat="server">
        <table style="width: 100%;" border="0">
            <tr>
                <td class="text-center">
                    <asp:Label ID="Label4" runat="server" Text="回答（承認）内容"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="width: 1205px; border-spacing: 6px 3px; border: 1px solid #000;" border="0" class="center">
            <tr>
                <td class="auto-style4">
                    <uc:ClsCMDateBox ID="dttAnswerD" runat="server" ppName="回答日" ppNameWidth="40" ppValidationGroup="ValNGC" ppRequiredField="False" />
                </td>
                <td class="auto-style5">
                    <uc:ClsCMTextBox ID="txtAnswerCharge" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="回答者" ppWidth="280" ppValidationGroup="ValNGC" ppRequiredField="False" />
                </td>
                <td class="auto-style6">
                    <asp:Label ID="lblNgoStatus" runat="server" Text="進捗状況"></asp:Label>
                </td>
                <td class="auto-style7">
                    <asp:DropDownList ID="ddlNgcStatus" runat="server">
                    </asp:DropDownList>
                    <%--                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvddlNGCStatus" runat="server" ControlToValidate="ddlNGCStatus" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                        </asp:Panel>
                    </div>--%>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <uc:ClsCMTextBox ID="txtAnswer" runat="server" ppTextMode="MultiLine" ppIMEMode="全角" ppHeight="100" ppWidth="950" ppName="回答" ppNameWidth="40" ppCheckLength="False" ppValidationGroup="ValNGC" ppWrap="True" ValidateRequestMode="Inherit" ppMaxLength="180" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table border="0" style="width: 100%">
        <tr class="text-center">
            <td>
                <asp:Button ID="btnUpdateNGC" runat="server" Text="更新" ValidationGroup="ValNGC" Visible="False" />
            </td>
            <td>
                <asp:Button ID="btnDelete2" runat="server" Text="削除" ValidationGroup="ValMain" Visible="False" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="ValMain" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <asp:ValidationSummary ID="vasSummaryDetails" runat="server" CssClass="errortext" ValidationGroup="Details" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <asp:ValidationSummary ID="vasSummaryNGC" runat="server" CssClass="errortext" ValidationGroup="ValNGC" />
            </td>
        </tr>
    </table>
</asp:Content>
