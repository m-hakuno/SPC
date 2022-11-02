<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CNSUPDP002.aspx.vb" Inherits="SPC.CNSUPDP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 7px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

    <table style="width: 1050px;" class="center">
        <tr class="align-top">
            <td>
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtArtclNo1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="指示Ｎｏ" ppValidationGroup="Artcltrns" ppRequiredField="True" ppWidth="10" ppNameWidth="110" ppCheckHan="True" />
                        </td>
                        <td class="align-top">
                            <asp:Panel ID="pnlArtclNo2" runat="server" Enabled="False">
                                <table border="0">
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblArtclNoH1" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtArtclNoT2" runat="server" Columns="4" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblArtclNoH2" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtArtclNoT3" runat="server" Columns="2" Enabled="False" Height="19px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblArtclNoH3" runat="server" Text="-"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtArtclNoT4" runat="server" Columns="4" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Panel ID="pnlChange" runat="server">
                    <table style="padding-top: 6px">
                        <tr>
                            <td>
                                <asp:Label ID="lblChangeN" runat="server" Text="変更回数"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblChangeV" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>

            <td>
                <asp:Panel ID="pnlProcCls" runat="server">
                    <table style="padding-top: 6px">
                        <tr>
                            <td>
                                <asp:Label ID="lblProcClsN" runat="server" Text="処理区分"></asp:Label>

                            </td>
                            <td>
                                <asp:Label ID="lblProcClsV" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr class="align-top">
            <td>
                <table style="white-space: nowrap;">
                    <tr class="align-top">
                        <td style="padding-top: 9px">
                            <asp:Label ID="lblRepuestClsN" runat="server" Text="依頼区分" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:Panel ID="pnlRepuestCls" runat="server">
                                <asp:RadioButtonList ID="rblrdoRepuestClsV" runat="server" RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem Value="1">物品転送依頼</asp:ListItem>
                                    <asp:ListItem Value="2">梱包箱出荷依頼</asp:ListItem>
                                </asp:RadioButtonList>
                                <div style="white-space: nowrap">
                                    <asp:Panel ID="pnlRepuestClsErr" runat="server" Width="0px">
                                        <asp:CustomValidator ID="cuvRepuestCls" runat="server" ControlToValidate="rblrdoRepuestClsV" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" ValidationGroup="Artcltrns" Display="Dynamic"></asp:CustomValidator>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Panel ID="pnlSendDT" runat="server">
                    <table style="padding-top: 8px">
                        <tr>
                            <td>
                                <asp:Label ID="lblSendDTN" runat="server" Text="送信日時"></asp:Label>

                            </td>
                            <td>
                                <asp:Label ID="lblSendDTV" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlSendCNT" runat="server">
                    <table style="padding-top: 8px">
                        <tr>
                            <td>
                                <asp:Label ID="lblSendCNTN" runat="server" Text="送信回数"></asp:Label>

                            </td>
                            <td>
                                <asp:Label ID="lblSendCNTV" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <asp:Label ID="lblCnst" runat="server" Text="工事内容"></asp:Label>
    <asp:Panel ID="pnlCnst" runat="server" BorderStyle="Outset">

        <table style="width: 1050px;" class="center">
            <tr class="align-top">
                <td colspan="3">
                    <uc:ClsCMDropDownList runat="server" ID="ddlBasedutyCD" ppNameWidth="110" ppName="出荷種別" ppClassCD="0046" ppNotSelect="True" ppRequiredField="True" ppValidationGroup="Artcltrns" ppMode="名称" />
                </td>

            </tr>
            <tr class="align-top">
                <td colspan="3">
                    <uc:ClsCMTextBox runat="server" ID="txtRequestNo" ppIMEModeOne="半角_変更不可" ppIMEMode="半角_変更不可" ppMaxLength="14" ppName="依頼番号" ppNameWidth="110" ppValidationGroup="Artcltrns" ppWidth="110" ppCheckHan="True" />
                </td>
            </tr>
            <tr class="align-top">
                <td colspan="3">
                    <uc:ClsCMTextBoxTwo runat="server" ID="ttxCommNo" ppIMEModeOne="半角_変更不可" ppIMEModeTwo="半角_変更不可" ppMaxLengthOne="5" ppMaxLengthTwo="8" ppName="通知番号" ppNameWidth="110" ppCheckLengthOne="False" ppCheckLengthTwo="False" ppValidationGroup="Artcltrns" ppRequiredField="False" ppWidthOne="35" ppWidthTwo="54" ppCheckHanTwo="True" ppCheckHanOne="True" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="pnlCnstCls" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCnstClsN" runat="server" Text="工事種別" Width="110"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCnstClsV1" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN1" runat="server" Text="新規"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV2" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN2" runat="server" Text="増設"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV3" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN3" runat="server" Text="再配置"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV4" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN4" runat="server" Text="移設"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV5" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN5" runat="server" Text="一部撤去"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV6" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN6" runat="server" Text="全撤去"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV7" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN7" runat="server" Text="一時撤去"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV8" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN8" runat="server" Text="構成変更"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV9" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN9" runat="server" Text="構成配信"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV11" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN11" runat="server" Text="ＶＵＰ"></asp:Label>&nbsp;&nbsp;
                                    <asp:Label ID="lblCnstClsV10" runat="server" Text="□"></asp:Label>
                                    <asp:Label ID="lblCnstClsN10" runat="server" Text="その他"></asp:Label>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCnstNotetextN" runat="server" Text="その他"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCnstNotetextV" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr class="align-top">
                <td style="width: 200px; margin-left: 200px;">
                    <uc:ClsCMTextBox runat="server" ID="txtTboxID" ppName="ＴＢＯＸＩＤ" ppNameWidth="110" ppMaxLength="8" ppCheckHan="True" ppValidationGroup="Artcltrns" ppCheckLength="True" ppRequiredField="True" />
                </td>
                <td style="width: 600px">
                    <asp:Panel ID="pnlHallNm" runat="server">
                        <table style="padding-top: 3px">
                            <tr>
                                <td style="margin-left: 40px">
                                    <asp:Label ID="lblHallNMN" runat="server" Text="ホール名" Width="100"></asp:Label>
                                </td>
                                <td style="margin-left: 40px">
                                    <asp:Label ID="lblHallNmV" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlNlCls" runat="server">
                        <table style="padding-top: 3px">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNlClsN" runat="server" Text="ＮＬ区分" Width="100"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblNlClsV" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMDateBox runat="server" ID="dtbArtcltransD" ppNameWidth="110" ppName="依頼日" ppYobiVisible="True" ppValidationGroup="Artcltrns" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="margin-left: 80px">
                    <uc:ClsCMDateBox runat="server" ID="dtbDelivDT" ppName="納期" ppNameWidth="110" ppYobiVisible="True" ppValidationGroup="Artcltrns" ppRequiredField="True" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="pnlTestDt" runat="server">
                        <table>
                            <tr>
                                <td class="align-top">
                                    <asp:Label ID="lblTestDtN" runat="server" Text="総合試験日" Width="110"></asp:Label>
                                </td>
                                <td class="align-top">
                                    <asp:Label ID="lblTestDtV" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <br />

    <asp:Label ID="lblSend" runat="server" Text="送付先情報"></asp:Label>
    <asp:Panel ID="pnlSend" runat="server" BorderStyle="Outset">
        <table style="width: 1050px;" class="center">
            <tr>
                <td colspan="2">
                    <uc:ClsCMTextBox runat="server" ID="txtCompCD" ppName="会社コード" ppNameWidth="110" ppIMEMode="半角_変更不可" ppMaxLength="3" ppRequiredField="True" ppValidationGroup="Artcltrns" ppCheckHan="True" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <uc:ClsCMTextBox runat="server" ID="txtOfficCD" ppName="営業所コード" ppNameWidth="110" ppMaxLength="5" ppIMEMode="半角_変更不可" ppRequiredField="True" ppValidationGroup="Artcltrns" ppCheckHan="True" />
                </td>
            </tr>
            <tr class="align-top">
                <td rowspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTrader" runat="server" Text="業者情報" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnTrader" runat="server" Text="参照" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="padding-top: 4px">
                        <tr>
                            <td>
                                <asp:Label ID="lblSendNmN" runat="server" Text="送付先名" Width="80"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblSendNmV" runat="server" Text=""></asp:Label>
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
                                <asp:Label ID="lblBranchN" runat="server" Text="営業所名" Width="80"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblBranchV" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblChargeN" runat="server" Text="担当者名" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlChargeV" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>

                                <div style="white-space: nowrap">
                                    <asp:Panel ID="Panel1" runat="server" Width="0px">
                                        <asp:CustomValidator ID="cuvCharge" runat="server" ControlToValidate="rblrdoRepuestClsV" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" ValidationGroup="Artcltrns" Display="Dynamic"></asp:CustomValidator>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>


            <tr>
                <td colspan="2">
                    <table class="align-top">
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblZipnoN" runat="server" Text="郵便番号" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblZipnoV" runat="server" Text="" Width="300"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrefN" runat="server" Text="県コード" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrefV" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAddrN" runat="server" Text="住所" Width="110"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblAddrV" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTelN" runat="server" Text="ＴＥＬ" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTelV" runat="server" Text="" Width="300"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFaxN" runat="server" Text="ＦＡＸ" Width="110"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFaxV" runat="server" Text="" Width="300"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <uc:ClsCMTextBox runat="server" ID="txtSPNotetext" ppName="特記事項" ppNameWidth="110" ppMaxLength="50" ppWidth="800" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <br />

    <asp:Label ID="lblArtcltrnsDTL" runat="server" Text="部品/部材"></asp:Label>
    <asp:Panel ID="pnlArtcltrnsDTL" runat="server" BorderStyle="Outset">
        <table style="width: 1050px;" class="center">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 150px">
                                <uc:ClsCMTextBox runat="server" ID="txtAppaCD" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="物品／部材コード" ppNameWidth="110" ppValidationGroup="ArtcltrnsDTL" ppWidth="100" ppCheckHan="True" ppRequiredField="True" />
                            </td>
                            <td style="width: 500px; padding-top: 7px;" class="align-top">
                                <asp:Label ID="lblAppaNm" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 250px">
                                <uc:ClsCMTextBox runat="server" ID="txtQuantity" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="数量" ppValidationGroup="ArtcltrnsDTL" ppWidth="28" ppCheckHan="True" ppNum="True" ppRequiredField="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 1050px;" class="center">
                        <tr>
                            <td style="width: 300px">
                                <uc:ClsCMDropDownList runat="server" ID="ddlAppaCnds" ppName="物品状況" ppNameWidth="110" ppClassCD="0047" ppValidationGroup="ArtcltrnsDTL" ppNotSelect="True" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtNotetext" ppMaxLength="50" ppName="備考" ppValidationGroup="ArtcltrnsDTL" ppWidth="652" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlArtcltrnsDTLBtn" runat="server" CssClass="float-right">
                        <asp:Button ID="btnDtlClear" runat="server" Text="クリア" CausesValidation="False" />
                        <asp:Button ID="btnAdd" runat="server" Text="追加" ValidationGroup="ArtcltrnsDTL" />
                        <asp:Button ID="btnUpdate" runat="server" Text="変更" ValidationGroup="ArtcltrnsDTL" />
                        <asp:Button ID="btnDelete" runat="server" Text="削除" CausesValidation="False" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <br />

    <asp:Panel ID="pnlGrvArtcltrnsDTL" runat="server">
        <div class="grid-out">
            <div class="grid-in">
                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvArtcltrnsDTL" runat="server">
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
    <div style="float: left">
        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="Artcltrns" />
        <asp:ValidationSummary ID="vasSummaryDTL" runat="server" ValidationGroup="ArtcltrnsDTL" CssClass="errortext" />
    </div>


    <hr />
</asp:Content>

