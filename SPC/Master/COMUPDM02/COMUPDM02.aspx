<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM02.aspx.vb" Inherits="SPC.COMUPDM02" %>

<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
    <style type="text/css">
        <!--
        .AutoExtender
        {
            margin: 0px !important;
            padding: 0px !important;
            text-align: left;
            color: WindowText;
            overflow: auto;
            border-color: ButtonShadow;
            border-width: 1px;
            border-style: solid;
            list-style-type: none;
            background-color: inherit;
        }

        li
        {
            /*margin-top: 5px;*/
            font-size: 13px;
            font-family: Arial;
        }
        -->
    </style>
</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width: 950px; margin-left: auto; margin-right: auto; border: none; text-align: left">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftSEmpCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="10" ppName="社員コード" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppValidationGroup="search" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSName" runat="server" ppIMEMode="全角" ppMaxLength="40" ppName="社員姓名" ppNameWidth="100" ppWidth="100" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSNameKana" runat="server" ppIMEMode="全角" ppMaxLength="40" ppName="社員姓名カナ" ppNameWidth="100" ppWidth="100" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="会社" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSComp" runat="server" Width="240" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="営業所" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSOffice" runat="server" Width="240"></asp:DropDownList>
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
                            <asp:Label ID="Label6" runat="server" Text="権限区分" Width="96"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chkListAuth" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                        </td>

                    </tr>
                </table>
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlsMailDvs" runat="server" ppName="メール送信区分" ppNameWidth="100" ppWidth="100" ppClassCD="0149" ppNotSelect="true"></uc:ClsCMDropDownList>
            </td>
            <td colspan="3">
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="100" ppWidth="100" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <table style="width: 960px; margin-left: auto; margin-right: auto; border: none; text-align: left">
        <tr>
            <td colspan="3" style="font-size: 15px; color: red; font-weight: bold;">
                <asp:Label ID="lblEmp" runat="server" Text="この社員コードは社員マスタで既に登録されています。" Visible="false"></asp:Label>
                <asp:Label ID="lblACmp" runat="server" Text="この社員コードは他社マスタで既に登録されています。" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtEmpCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="10" ppName="社員コード" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppValidationGroup="key" />
            </td>
        </tr>
        <tr>
            <td style="width: 260px;">
                <uc:ClsCMTextBox ID="txtLName" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="社員姓" ppNameWidth="100" ppWidth="160" ppRequiredField="true" ppValidationGroup="val" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtFName" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="社員名" ppNameWidth="100" ppWidth="160" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSrtName" runat="server" ppIMEMode="全角" ppMaxLength="10" ppName="社員略称" ppNameWidth="100" ppWidth="100" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtLNameK" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="社員姓カナ" ppNameWidth="100" ppWidth="160" />
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtFNameK" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="社員名カナ" ppNameWidth="100" ppWidth="160" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="TEL" Width="94"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtTel1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppNameVisible="false" ppName="TEL番号" ppNameWidth="0" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTel2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTel3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="FAX" Width="94"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtFax1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppNameVisible="false" ppName="FAX番号" ppNameWidth="0" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtFax2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtFax3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
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
                            <asp:Label ID="Label3" runat="server" Text="会社" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlComp" runat="server" Width="240" AutoPostBack="true"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="pnlSystemErr" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvComp" runat="server" ValidationGroup="val" ControlToValidate="ddlComp" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="営業所" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOffice" runat="server" Width="240"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="Panel1" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvOffice" runat="server" ValidationGroup="val" ControlToValidate="ddlOffice" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="権限区分" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAuth" runat="server" Width="105"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="Panel2" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvAuth" runat="server" ValidationGroup="val" ControlToValidate="ddlAuth" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
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
                            <asp:Label ID="Label7" runat="server" Text="携帯電話番号1" Width="94"></asp:Label>
                            <asp:HiddenField ID="hdnTelSp1" runat="server" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtTelSp1_1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppNameVisible="false" ppName="携帯電話番号１" ppNameWidth="0" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTelSp1_2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTelSp1_3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="携帯電話番号2" Width="94"></asp:Label>
                            <asp:HiddenField ID="hdnTelSp2" runat="server" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtTelSp2_1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppNameVisible="false" ppName="携帯電話番号２" ppNameWidth="0" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTelSp2_2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTelSp2_3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" ppNum="true" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding-right:18px;text-align:right">
                <asp:Label ID ="lblMailAdmin" runat ="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="CRS管理" Width="94"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMDropDownList ID="ddlCrsDvs" runat="server" ppName="CRS管理" ppNameVisible="false" ppWidth="90" ppClassCD="0158" ppNotSelect="true" ></uc:ClsCMDropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="範囲管理区分" Width="94"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMDropDownList ID="ddlVscDvs" runat="server" ppName="範囲管理区分" ppNameVisible="false" ppWidth="105" ppClassCD="0150" ppNotSelect="true"></uc:ClsCMDropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="メール送信区分" Width="94"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMDropDownList ID="ddlMailDvs" runat="server" ppName="メール送信区分" ppNameVisible="false" ppWidth="105" ppClassCD="0149" ppNotSelect="true" ppValidationGroup="val"></uc:ClsCMDropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="メールアドレス" Width="94"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtMail" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="100" ppNameVisible="false" ppName="メールアドレス" ppNameWidth="0" ppWidth="500" ppValidationGroup="val" ppCheckAc="False" ppCheckHan="True" />
                        </td>
                        <td>
                            <asp:Label ID="lbl" runat="server" Text="＠"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDomain" CssClass="ime-disabled" runat="server" MaxLength="50" Width="300" ValidationGroup="val" />
                            <ajaxToolkit:AutoCompleteExtender ID="comp" runat="server" CompletionInterval="50"
                                MinimumPrefixLength="1" ServiceMethod="GetCompletionDomainList"
                                ServicePath="~/Master/AutoComplete.asmx" TargetControlID="txtDomain"
                                CompletionSetCount="10" CompletionListCssClass="AutoExtender">
                            </ajaxToolkit:AutoCompleteExtender>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 1270px; height: 200px;">
        <div class="grid-in" style="width: 1270px; height: 200px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
