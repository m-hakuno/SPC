<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="WATLSTP002.aspx.vb" Inherits="SPC.WATLSTP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr class="align-top">
            <td style="width: 380px">
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="90" ppWidth="70" ppIMEMode="半角_変更不可" ppNum="True" ppRequiredField="False" ppCheckHan="True" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlNLCls" runat="server" ppName="ＮＬ区分" ppNameWidth="100" ppWidth="100" ppClassCD="0009" ppMode="名称" ppNotSelect="True" />
            </td>
        </tr>
        <tr class="align-top">
            <td style="width: 380px">
                <table border="0">
                    <tr>
                        <td style="width: 90px">
                            <asp:Label ID="lblSystem" runat="server" Text="システム"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSystem" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtReason1" runat="server" ppName="監視対象外理由" ppNameWidth="100" ppWidth="150" ppMaxLength="10" />
            </td>
        </tr>
        <tr>
            <td style="width: 380px">
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 90px">
                            <asp:Label ID="lblDeleteFlg" runat="server" Text="削除フラグ"></asp:Label>
                        </td>
                        <td style="width: 60px">
                            <asp:CheckBox ID="cbxDeleteFlgY" runat="server" Text="有効" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxDeleteFlgN" runat="server" Text="削除" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <asp:Panel ID="pnlRegister" runat="server" BackColor="#FFCCFF" BorderStyle="Solid" BorderWidth="1px">
        <table border="0" class="center">
            <tr>
                <td style="width: 947px">
                    <table border="0">
                        <tr>
                            <td class="align-top" style="width:250px">
                                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppName="ＴＢＯＸＩＤ" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="True" ppRequiredField="True" ppWidth="85" ppValidationGroup="RegistrationArea" ValidateRequestMode="Inherit" ppCheckHan="True" ppCheckLength="True" />
                            </td>
                            <td class="align-top" style="width:160px">
                                <table border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNLCls1" runat="server" Text="ＮＬ区分" Width="55px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNLCls2" runat="server" Width="65px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="align-top" style="width:170px">
                                <table border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSystem1" runat="server" Text="システム" Width="55px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSystem2" runat="server" Width="80px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="align-top" style="width:295px">
                                <table border="0">
                                    <tr>
                                        <td class="align-top">
                                            <asp:Label ID="lblHallNm1" runat="server" Text="ホール名" Width="55px"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:Label ID="lblHallNm2" runat="server" Width="200px" ></asp:Label>--%>
                                             <asp:Label ID="lblHallNm2" runat="server" Width="400px" ></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 947px">
                    <table border="0" style="width: 879px">
                        <tr>
                            <td class="align-top" style="width: 305px">
                                <uc:ClsCMTextBox ID="txtReason2" runat="server" ppName="監視対象外理由" ppWidth="150" ppMaxLength="10" ppRequiredField="True" ppValidationGroup="RegistrationArea" />
                            </td>
                            <td class="align-top" style="width: 240px">
                                <uc:ClsCMTextBox ID="txtCnfm" runat="server" ppMaxLength="10" ppName="確認先" ppWidth="150" ppRequiredField="True" ppValidationGroup="RegistrationArea" />
                            </td>
                            <td class="align-top" style="width: 240px">
                                <uc:ClsCMTextBox ID="txtMntrReportNo" runat="server" ppMaxLength="15" ppName="管理番号" ppWidth="120" ppRequiredField="True" ppValidationGroup="RegistrationArea" ppCheckHan="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 947px">
                    <table class="align-top" style="width:101%;" border="0">
                        <tr>
                            <td class="align-top" style="width: 335px">
                                <table border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblInpUsr" runat="server" Text="入力者"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInpUsr" runat="server" Width="220px">
                                            </asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlErr" runat="server" Width="220px">
                                                    <asp:CustomValidator ID="cuvDropDownList" runat="server" ControlToValidate="ddlInpUsr" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="RegistrationArea"></asp:CustomValidator>
                                                </asp:Panel>
<%--                                                <asp:Panel ID="Panel1" runat="server" Width="0px">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="ddlInpUsr" CssClass="errortext" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="RegistrationArea"></asp:RequiredFieldValidator>
                                                </asp:Panel>--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="align-top">
                                <uc:ClsCMTextBox ID="txtNoteText" runat="server" ppName="備考" ppWidth="280" ppMaxLength="20" ppValidationGroup="RegistrationArea" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 100%;" class="align-top" border="0">
            <tr>
                <td class="float-left">
                    <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="RegistrationArea" />
                </td>
                <td class="float-right">
                    <table border="0" class="float-right">
                        <tr>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Text="登録" ValidationGroup="RegistrationArea" />
                            </td>
                            <td>
                                <asp:Button ID="btnUpdate" runat="server" Text="更新" ValidationGroup="RegistrationArea" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" Text="削除" ValidationGroup="RegistrationArea" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <table border="0" class="float-right">
        <tr>
            <td>
                <asp:Button ID="btnCSV" runat="server" Text="ＣＳＶ"/>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
