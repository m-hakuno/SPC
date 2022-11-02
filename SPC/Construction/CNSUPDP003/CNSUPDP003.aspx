<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeBehind="CNSUPDP003.aspx.vb" Inherits="SPC.CNSUPDP003" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContent" runat="server">
    <%--<script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/EnableChange.js")%>'></script>--%>
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
        <!--
        .textdata tr td {
            vertical-align: top;
        }

            .textdata tr td span {
                vertical-align: top;
            }
        -->
    </style>

    <!--工事基本情報-->
    <table class="center" style="width: 900px; border-spacing: 6px;" border="0">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTboxId_1" runat="server" Text="ＴＢＯＸＩＤ" Width="90px"></asp:Label>
                <asp:Label ID="lblTboxId_2" runat="server" Width="100px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblConReqNo_1" runat="server" Text="工事依頼番号" Width="90px"></asp:Label>
                <asp:Label ID="lblConReqNo_2" runat="server" Width="100px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblCnstDt_1" runat="server" Text="工事開始日" Width="70px"></asp:Label>
                <asp:Label ID="lblCnstDt_2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTboxSystem_1" runat="server" Text="システム" Width="90px"></asp:Label>
                <asp:Label ID="lblTboxSystem_2" runat="server" Width="100px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblVer_1" runat="server" Text="ＶＥＲ" Width="90px"></asp:Label>
                <asp:Label ID="lblVer_2" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblNjSection_1" runat="server" Text="ＮＬ区分" Width="70px"></asp:Label>
                <asp:Label ID="lblNjSection_2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblHoleNm_1" runat="server" Text="ホール名" Width="90px"></asp:Label>
                <asp:Label ID="lblHoleNm_2" runat="server" Style="white-space: nowrap;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblEwSection_1" runat="server" Text="ＥＷ区分" Width="70px"></asp:Label>
                <asp:Label ID="lblEwSection_2" runat="server" Width="100px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblAddress_1" runat="server" Text="ホール住所" Width="90px"></asp:Label>
                <asp:Label ID="lblAddress_2" runat="server"></asp:Label>
            </td>
            <td>

                <asp:Label ID="lblTel_1" runat="server" Text="ＴＥＬ" Width="70px"></asp:Label>
                <asp:Label ID="lblTel_2" runat="server"></asp:Label>

            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblAgency_1" runat="server" Text="代理店" Width="90px"></asp:Label>
                <asp:Label ID="lblAgency_2" runat="server"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="lblAgencyShop_1" runat="server" Text="代行店" Width="70px"></asp:Label>
                <asp:Label ID="lblAgencyshop_2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblEstCls_1" runat="server" Text="設置区分" Width="90px"></asp:Label>
                <asp:Label ID="lblEstCls_2" runat="server"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="lblPrgSituation_1" runat="server" Text="進捗状況" Width="70px"></asp:Label>
                <asp:Label ID="lblPrgSituation_2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 100px; vertical-align: top;">
                <asp:Label ID="lblConstructionDiv" runat="server" Text="工事区分" Width="70px"></asp:Label>

            </td>
            <td colspan="3" style="width: 800px;">
                <table class="center" style="width: 770px; border-collapse: collapse;" border="1">
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
                            <asp:Label ID="lblNew_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblExpns_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSmRmv_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblShpRelocat_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAllRmv_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOncRmv_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCnChng_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCnDlvry_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblReInst_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblVup_2" runat="server">X</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOther_2" runat="server">X</asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px;">
                        <td class="text-center">
                            <asp:Label ID="lblOtherContent_1" runat="server" Text="その他内容"></asp:Label>
                        </td>
                        <td colspan="10">
                            <asp:Label ID="lblOtherContent_2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr runat="server">
            <td style="width: 100px;"></td>
            <td style="width: 230px;"></td>
            <td style="width: 300px;"></td>
            <td style="width: 270px;"></td>
        </tr>
        <%--        <tr id="trWorker" runat="server">
            <td style="width: 100px;"></td>
            <td style="width: 300px;"></td>
            <td style="width: 230px;"></td>
            <td style="width: 270px;"></td>
        </tr>--%><%--<tr id="trWorker" runat="server">
            <td colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>--%>
    </table>


    <!--作業員情報-->
    <table class="center" style="width: 900px; border-spacing: 6px 0px;" border="0">
        <tr>
            <td style="width: 250px">
                <asp:Label ID="lblICOBranchOffice_1" runat="server" Text="担当支社" Width="90px"></asp:Label>
                <asp:Label ID="lblICOBranchOffice_2" runat="server"></asp:Label>
            </td>
            <td style="width: 300px">
                <asp:Label ID="lblWorkerNm_1" runat="server" Text="作業員名" Width="70px"></asp:Label>
                <asp:Label ID="lblWorkerNm_2" runat="server"></asp:Label>
                <uc:ClsCMTextBox ID="txtWorkerNm" runat="server" ppErrWidth="100" ppMaxLength="20" ppName="作業員名" ppNameVisible="True" ppNameWidth="70" ppRequiredField="False" ppValidationGroup="Worker" ppWidth="150" ppWrap="False" ppIMEMode="全角" />
            </td>
            <td style="width: 250px">
                <asp:Label ID="lblContactInfo_1" runat="server" Text="連絡先" Width="60px"></asp:Label>
                <asp:Label ID="lblContactInfo_2" runat="server"></asp:Label>
                <uc:ClsCMTextBox ID="txtCntactInfo" runat="server" ppNameVisible="True" ppWidth="110" ppName="連絡先" ppMaxLength="15" ppRequiredField="False" ppWrap="False" ppErrWidth="100" ppCheckHan="True" ppIMEMode="半角_変更不可" ppValidationGroup="Worker" ValidateRequestMode="Inherit" ppNameWidth="50" />
            </td>
            <td style="width: 100px">
                <asp:Button ID="btnUpdWrk" runat="server" Text="変更" Width="50px" ValidationGroup="Worker" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:ValidationSummary ID="vasWorker" runat="server" CssClass="errortext" ValidationGroup="Worker" />
            </td>
        </tr>
    </table>

    <hr />

    <!--トラブル関連 ＆ 撤去関連-->
    <table class="center" style="width: 1230px; border-spacing: 6px 3px; border: 1px solid #000; margin-top: 10px;">
        <tr>
            <td>
                <asp:Label ID="lblTrouble" runat="server" Text="トラブル関連" Width="90px"></asp:Label>
            </td>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbxAllCnst" runat="server" Text="発生工事全" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxCnstDelay" runat="server" Text="工事遅延" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxSpareMachineUse" runat="server" Text="予備機使用" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxMisCalcuCnst" runat="server" Text="空振り工事" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxIns" runat="server" Text="ＩＮＳ" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTel" runat="server" Text="ＴＥＬ" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxOther" runat="server" Text="その他" Width="100px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxConstitution" runat="server" Text="構成" Width="100px" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:HiddenField ID="hdnRmv" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 100px;"></td>
            <td style="width: 150px;"></td>
            <td style="width: 400px;"></td>
            <td style="width: 300px;"></td>
            <td style="width: 70px;" rowspan="2">
                <asp:Button ID="btnUpdTrbl" runat="server" Text="変更" Width="50px" ValidationGroup="emp" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRemove" runat="server" Text="撤去関連" Width="90px"></asp:Label>
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlAATConSituation" ppName="未集信" ppMode="名称" ppClassCD="0113" Visible="True" ppValidationGroup="emp" ppNotSelect="False" />
            </td>
            <td>
                <asp:Label ID="lblAATCUReason" runat="server" Text="随時集信未実施理由"></asp:Label>
                <asp:DropDownList ID="ddlAATCUReason" runat="server"></asp:DropDownList>
            </td>
            <td>
                <uc:ClsCMDateBox runat="server" ID="dttAATLstwrkDt" ppName="最終営業日" ppValidationGroup="emp" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:ValidationSummary ID="vasTrblRmv" runat="server" CssClass="errortext" ValidationGroup="emp" />
            </td>
        </tr>
    </table>


    <!--対応明細編集-->
    <table class="center" style="width: 1230px; border-spacing: 3px 3px; border: 1px solid #000; margin-top: 5px; margin-bottom: 10px;">
        <tr>
            <td colspan="2">
                <table style="border-collapse: collapse;" border="1">
                    <tr class="text-center" style="background-color: #8DB4E2;">
                        <asp:Label ID="lblSerialNo_1" runat="server" Text="連番" Visible="False"></asp:Label>
                        <td style="width: 200px;">
                            <asp:Label ID="lblSupportDtAndTm" runat="server" Text="対応日時"></asp:Label>
                        </td>
                        <td style="width: 200px;">
                            <asp:Label ID="lblRespondersNm_1" runat="server" Text="対応者"></asp:Label>
                        </td>
                        <td style="width: 200px;">
                            <asp:Label ID="lblProgressSituation" runat="server" Text="進捗状況" Width="100px"></asp:Label>
                        </td>
                        <td>

                            <asp:Label ID="lblContent" runat="server" Text="内容"></asp:Label>

                        </td>
                    </tr>
                    <tr class="text-center">
                        <asp:Label ID="lblSerialNo_2" runat="server" CssClass="float-right" Visible="False"></asp:Label>
                        <td style="padding-top: 7px; padding-left: 10px;" class="auto-style3">
                            <uc:ClsCMDateTimeBox ID="datlblSupportDt" runat="server" ppNameVisible="False" ppName="対応日時" ppOver="False" ppRequiredField="True" ppValidationGroup="Detail" />
                        </td>
                        <td style="padding-left: 15px;">
                            <uc:ClsCMDropDownList ID="ddlResponser" runat="server" ppName="対応者" ppNameWidth="0" ppWidth="160" ppClassCD="0015" ppRequiredField="True" ppEnabled="True" ppNameVisible="False" ppValidationGroup="Detail" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSituation" runat="server"></asp:DropDownList>
                        </td>
                        <td>

                            <uc:ClsCMTextBox ID="txtContent" runat="server" ppName="内容" ppHeight="40" ppWidth="600" ppTextMode="MultiLine" ppMaxLength="122" ppNameVisible="False" ppWrap="true" ppValidationGroup="Detail" ppIMEMode="全角" />

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

                <asp:Button ID="btnDetailClear" runat="server" Text="クリア" ValidationGroup="Detail" CausesValidation="False" />
                &nbsp;&nbsp;
                <asp:Button ID="btnDetailInsert" runat="server" Text="追加" ValidationGroup="Detail" />
                <asp:Button ID="btnDetailUpdate" runat="server" Text="更新" ValidationGroup="Detail" />
                <asp:Button ID="btnDetailDelete" runat="server" Text="削除" ValidationGroup="Detail" CausesValidation="False" BackColor="#FF6666" />

            </td>
        </tr>
    </table>


    <!--該当件数表示 & リロードボタン-->
    <div id="divCount" runat="server" class="float-Left" style="margin-top: 5px;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" ></asp:Label>
                </td>
                <td style="width: 80px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" ></asp:Label>
                </td>
                <td style="width: 15px"></td>
        </table>
    </div>


    <!--グリッド-->
    <div id="DivOut" runat="server" class="grid-out" style="height: 560px;">
        <div id="DivIn" runat="server" class="grid-in" style="height: 560px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>


</asp:Content>
