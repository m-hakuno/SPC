<%@ Page Title="" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="SCLUPDP001.aspx.vb" Inherits="SPC.SCLUPDP001" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">

    <script type="text/javascript">

        //convertEnterToTab(event)を設定
        function pageLoad() {
            set_onloadenter();
        }


    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

    <div style="border: 1px solid; margin: 0px 0.5% 10px 0.5%; padding: 10px 3px 5px 3px;">
        <div style="width: 99%; padding-bottom: 5px;">

            <asp:UpdatePanel ID="updpnDetail" runat="server">
                <ContentTemplate>

                    <div style="position: relative;">

                        <table class="center" style="width: 920px;" border="0">
                            <tr>
                                <td style="width: 25px;"></td>
                                <td style="width: 300px;"></td>
                                <td style="width: 300px;"></td>
                                <td style="width: 300px;"></td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-left: 3px; padding-bottom: 8px;">

                                    <asp:Label ID="Label3" runat="server" Text="スケジュール情報"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="3" style="padding-left: 3px;">

                                    <asp:Label ID="Label2" runat="server" Text="メール管理番号" Width="100px"></asp:Label>
                                    <asp:Label ID="lblMailNo" runat="server" Text="" Width="100px"></asp:Label>

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <uc:ClsCMDropDownList runat="server" ID="ddlVstCls" ppClassCD="0000" ppValidationGroup="1" ppName="訪問種別" ppNameWidth="100" ppWidth="130" ppNotSelect="True" />
                                </td>
                                <td colspan="2" style="padding: 8px 0px 8px 4px;">
                                    <asp:Label ID="Label1" runat="server" Text="依頼番号" Width="70px"></asp:Label>
                                    <asp:Label ID="lblCtrlNo" runat="server"></asp:Label>
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </td>
                                <%-- <td colspan="2">
                                    <uc:ClsCMTextBox ID="txtCtrlNo" runat="server" ppName="依頼番号" ppNameWidth="70" ppWidth="110"
                                        ppIMEMode="半角_変更不可" ppMaxLength="14" ppCheckHan="True" ppRequiredField="True" ppValidationGroup="Main" />
                                </td>--%>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <uc:ClsCMDateTimeBox runat="server" ID="txtWrkDt" ppNameVisible="True" ppName="到着日時" ppRequiredField="True" ppNameWidth="100px" ppValidationGroup="Main" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="padding-left: 3px;">
                                    <%--<uc:ClsCMTextBox ID="txtVstCd" runat="server" ppName="訪問先コード" ppNameWidth="100" ppWidth="70"
                                        ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppRequiredField="True" ppValidationGroup="Main" />--%>
                                    <asp:Label ID="Label5" runat="server" Text="TBOXID" Width="100px"></asp:Label>
                                    <asp:Label ID="lblVstCd" runat="server" Width="100px" Visible="false"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <uc:ClsCMTextBox ID="txtVstNm" runat="server" ppName="訪問先名" ppNameWidth="70" ppWidth="350"
                                        ppIMEMode="全角" ppMaxLength="50" ppCheckHan="False" ppRequiredField="True" ppValidationGroup="Main" />
                                </td>
                            </tr>
                        </table>

                        <table style="position: absolute; bottom: 2px; right: 5px;">
                            <tr>
                                <td>
                                    <asp:Button ID="btnClear" runat="server" Text="クリア" CausesValidation="False" />
                                </td>
                                <td>
                                    <asp:Button ID="btnInsert" runat="server" Text="登録" ValidationGroup="Main" />
                                </td>
                            </tr>
                        </table>

                    </div>

                    <asp:ValidationSummary ID="vasMain" runat="server" CssClass="errortext" ValidationGroup="Main" />

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnInsert" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>

    <div style="border: 1px solid; margin: 0px 0.5% 10px 0.5%; padding: 10px 3px 5px 3px;">
        <div style="width: 99%; position: relative; padding-bottom: 5px;">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div style="position: relative;">

                        <table class="center" style="width: 925px;" border="0">
                            <tr>
                                <td style="width: 25px;"></td>
                                <td style="width: 200px;">
                                    <asp:HiddenField ID="hdnSEQ" runat="server" />
                                </td>
                                <td style="width: 150px;"></td>
                                <td style="width: 300px;"></td>
                                <td style="width: 250px;"></td>
                            </tr>
                            <tr>
                                <td colspan="4" style="padding-left: 3px; padding-bottom: 8px;">
                                    <asp:Label ID="Label4" runat="server" Text="作業者スケジュール情報"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <uc:ClsCMTextBox ID="txtWkmCd" runat="server" ppName="作業者コード" ppNameWidth="100" ppWidth="70"
                                        ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppValidationGroup="Detail" ppRequiredField="True" />
                                </td>
                                <td colspan="3" style="padding-left: 4px;">
                                    <asp:Label ID="lblWkmLNm" runat="server"></asp:Label>
                                    <asp:Label ID="lblWkmFNm" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr style="padding-left: 50px;">
                                <td></td>
                                <td colspan="2" style="padding: 8px 0px 8px 4px;">
                                    <asp:Label ID="lbl01" runat="server" Text="営業所" Width="100px"></asp:Label>
                                    <asp:Label ID="lblMntNm" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnMntCd" runat="server" />
                                </td>
                                <td colspan="2" style="padding: 8px 0px 8px 4px;">
                                    <asp:Label ID="lbl02" runat="server" Text="エリア" Width="80px"></asp:Label>
                                    <asp:Label ID="lblArea" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnAreaCd" runat="server" />
                                </td>
                            </tr>
                            <tr style="padding-left: 50px;">
                                <td></td>
                                <td colspan="2" style="padding: 8px 0px 8px 4px;">
                                    <asp:Label ID="lbl03" runat="server" Text="責任者1" Width="100px"></asp:Label>
                                    <asp:Label ID="lblAdm1" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnAdm1Cd" runat="server" />
                                    <asp:HiddenField ID="hdnAdm1LNm" runat="server" />
                                    <asp:HiddenField ID="hdnAdm1FNm" runat="server" />
                                </td>
                                <td colspan="2" style="padding: 8px 0px 8px 4px;">
                                    <asp:Label ID="lbl04" runat="server" Text="責任者2" Width="80px"></asp:Label>
                                    <asp:Label ID="lblAdm2" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnAdm2Cd" runat="server" />
                                    <asp:HiddenField ID="hdnAdm2LNm" runat="server" />
                                    <asp:HiddenField ID="hdnAdm2FNm" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <uc:ClsCMDateTimeBox runat="server" ID="txtDptDt" ppNameVisible="True" ppName="出発日時" ppRequiredField="True" ppNameWidth="100px" ppValidationGroup="Detail" />
                                </td>
                                <td colspan="2" style="padding: 0px 0px 8px 4px;">
                                    <%--<uc:ClsCMDateTimeBox runat="server" ID="txtArrDt" ppNameVisible="True" ppName="到着日時" ppRequiredField="True" ppNameWidth="80px" ppValidationGroup="Detail" />--%>
                                    <asp:Label ID="lblArrDtNm" runat="server" Text="到着日時" Width="80px"></asp:Label>
                                    <asp:Label ID="lblArrDt" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table style="position: absolute; bottom: 2px; right: 2px;">
                            <tr>
                                <td>
                                    <asp:Button ID="btnClearDtl" runat="server" Text="クリア" />
                                </td>
                                <td>
                                    <asp:Button ID="btnInsertDtl" runat="server" Text="登録" ValidationGroup="Detail" />
                                </td>
                                <td>
                                    <asp:Button ID="btnUpdateDtl" runat="server" Text="更新" ValidationGroup="Detail" />
                                </td>
                                <td>
                                    <asp:Button ID="btnDeleteDtl" runat="server" Text="明細削除" />
                                </td>
                            </tr>
                        </table>

                    </div>

                    <asp:ValidationSummary ID="vasDtl" runat="server" CssClass="errortext" ValidationGroup="Detail" />

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnInsertDtl" />
                    <asp:PostBackTrigger ControlID="btnUpdateDtl" />
                    <asp:PostBackTrigger ControlID="btnDeleteDtl" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

    </div>
    <div id="divCount" runat="server" class="float-right" style="padding-right: 8px;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="明細件数："></asp:Label>
                </td>
                <td style="width: 20px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件"></asp:Label>
                </td>
        </table>
    </div>
    <div id="DivOut" runat="server" class="grid-out" style="width: 1250px; margin-left: auto; margin-right: auto;">
        <div class="grid-in" style="height: 270px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>


</asp:Content>
