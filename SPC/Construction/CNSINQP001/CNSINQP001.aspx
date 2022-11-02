<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CNSINQP001.aspx.vb" Inherits="SPC.CNSINQP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    

    <uc:ClsCMTextBoxTwo ID="txtCntlNo" runat="server" ppName="工事管理番号" ppMaxLengthOne="5" ppMaxLengthTwo="8" ppWidthOne="40" ppWidthTwo="56" ppIMEModeOne="半角_変更不可" ppIMEModeTwo="半角_変更不可" ppValidationGroup="Search" ppCheckHanOne="True" ppCheckHanTwo="True" ppRequiredField="True" />

    <table style="width: 100%">
        <tr>
            <td>
                <div class="float-left">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errortext" ValidationGroup="Search" />
                </div>
                <div class="float-right">
                    <asp:Button ID="btnClear" runat="server" Text="検索条件クリア" />
                    &nbsp;<asp:Button ID="btnSearch" runat="server" Text="検索" ValidationGroup="Search" />
                </div>
            </td>
        </tr>
    </table>

    <hr />

    <table class="center" style="white-space: nowrap">
        <tr>
            <td colspan="7">
                <table>
                    <tr>
                        <td><asp:Label ID="lblSendSttsN" runat="server" Width="60px" Text="送信状況："></asp:Label></td>
                        <td><asp:Label ID="lblSendSttsV" runat="server" Width="200px"></asp:Label></td>
                    </tr>
                </table>
            </td>
            <td colspan="2"><asp:Label ID="lblPDFSttsN" runat="server" Width="60px" Text="PDF状態："></asp:Label><asp:Label ID="lblPDFSttsV" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="lblTboxIDN" runat="server" Text="ＴＢＯＸＩＤ："></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="lblTboxIDV" runat="server" Text=""></asp:Label>
            </td>
            <td style="width: 80px">
                <asp:Label ID="lblHallNmN" runat="server" Text="ホール名："></asp:Label>
            </td>
            <td style="width: 200px">
                <asp:Label ID="lblHallNmV" runat="server" Text=""></asp:Label>
            </td>
            <td style="width: 80px">
                <asp:Label ID="lblCnstDtN" runat="server" Text="工事日時："></asp:Label>
            </td>
            <td style="width: 120px">
                <asp:Label ID="lblCnstDtV" runat="server" Text=""></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="lblDatarcvDtN" runat="server" Text="依頼受信日時："></asp:Label>
            </td>
            <td style="width: 120px">
                <asp:Label ID="lblDatarcvDtV" runat="server" Text=""></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="lblEmreqCls" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTboxclsNmN" runat="server" Text="システム："></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTboxclsNmV" runat="server" Text=""></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTPCTITLE" runat="server" Text="登録システム："></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblTPCNAME" runat="server" Text=""></asp:Label>　<asp:Label ID="lblTPCVER" runat="server" Text=""></asp:Label>
            </td>
            <td colspan="5">
                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <asp:Label ID="lblCnst" runat="server" Text="工事種別："></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="9">
                <table class="center">
                    <tr class="text-center">
                        <td><br /></td>
                        <td style="width: 7%">
                            <asp:Label ID="lblNew_1" runat="server" Text="新規"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblExpansion_1" runat="server" Text="増設"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblSomeRemoval_1" runat="server" Text="一部撤去"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblShopRelocation_1" runat="server" Text="店内移設"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblAllRemoval_1" runat="server" Text="全撤去"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblOnceRemoval_1" runat="server" Text="一時撤去"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblReInstallation_1" runat="server" Text="再設置"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblConChange_1" runat="server" Text="構成変更"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblConDelively_1" runat="server" Text="構成配信"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblVup_1" runat="server" Text="ＶＵＰ"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lblOther_1" runat="server" Text="その他"></asp:Label>
                        </td>
                        <td style="width: 8%">
                            <asp:Label ID="lblOtherDTL_1" runat="server" Text="その他内容"></asp:Label>
                        </td>
                        <td><br /></td>
                    </tr>
                    <tr class="text-center">
                        <td><br /></td>
                        <td>
                            <asp:Label ID="lblNew_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblExpansion_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSomeRemoval_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblShopRelocation_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAllRemoval_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOnceRemoval_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblReInstallation_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblConChange_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblConDelivery_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblVup_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOther_2" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOtherDTL_2" runat="server"></asp:Label>
                        </td>
                        <td><br /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTmpsetDtN" runat="server" Text="仮設置日時："></asp:Label>
            </td>
            <td colspan="8">
                <asp:Label ID="lblTmpsetDtV" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblStestDtN" runat="server" Text="総合テスト日："></asp:Label>
            </td>
            <td colspan="4">
                <asp:Label ID="lblStestDtV" runat="server" Text=""></asp:Label>
            </td>
            <td colspan="4">
                <asp:Panel ID="pnlCnfrm" runat="server" Visible="False">
                    <table>
                        <tr class="align-top">
                            <td>
                                <uc:ClsCMTextBoxPopup ID="txtCnfrm" runat="server" ppName="確認者" ppValidationGroup="Update" />
                            </td>
                            <td style="padding-top: 4px;">
                                <asp:Button ID="btnCnfrm" runat="server" Text="確認" ValidationGroup="Update" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="9">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="9">                
                <table>
                    <tr class="align-top">
                        <td style="width: 70px">&nbsp;</td>
                        <td>
                            <uc:ClsCMTextBox runat="server" ID="txtCloseDtym" ppName="締日" ppNameWidth="100" ppMaxLength="4" ppWidth="28" ppIMEMode="半角_変更不可" ppValidationGroup="Update" ppRequiredField="True" ppCheckHan="True" ppCheckLength="True" />
                        </td>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlCloseDt" ppName="締日" ppClassCD="0019" ppMode="名称" ppNameVisible="False" ppValidationGroup="Update" ppRequiredField="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
<%--                        <td colspan="2">
                            <uc:ClsCMTextBox runat="server" ID="txtCnstCls" ppName="工事区分" ppNameWidth="100" ppMaxLength="2" ppWidth="15" ppIMEMode="半角_変更不可" ppValidationGroup="Update" />
                        </td>--%>
                        <td colspan="2">
                            <%--<uc:ClsCMDropDownList runat="server" ID="ddlCnstCls" ppName="工事区分" ppNameWidth="100" ppClassCD="0112" ppMode="名称" ppValidationGroup="Update" ppNotSelect="true" />--%>
                            <uc:ClsCMDropDownList runat="server" ID="ddlCnstCls" ppName="工事区分" ppNameWidth="100" ppClassCD="0000" ppValidationGroup="Update" ppNotSelect="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <table border="1" class="center">
        <tr class="text-center">
            <td style="width: 100px">
                <asp:Label ID="Label20" runat="server" Text="コード"></asp:Label>
            </td>
            <td style="width: 300px">
                <asp:Label ID="Label21" runat="server" Text="工事名"></asp:Label>
            </td>
            <td style="width: 150px">
                <asp:Label ID="Label22" runat="server" Text="単価"></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="Label23" runat="server" Text="数量"></asp:Label>
            </td>
            <td style="width: 100px">
                <asp:Label ID="Label24" runat="server" Text="割増区分"></asp:Label>
            </td>
        </tr>
        <tr class="text-center">
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtCnsrCD" ppNameVisible="False" ppName="コード" ppMaxLength="5" ppWidth="90" ppIMEMode="半角_変更不可" ppValidationGroup="Amount" />
            </td>
            <td>
                <asp:Label ID="lblCnstNM" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblUnitPrice" runat="server"></asp:Label>
            </td>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtQuantity" ppNameVisible="False" ppName="数量" ppMaxLength="4" ppWidth="90" ppIMEMode="半角_変更不可" ppNum="True" ppValidationGroup="Amount" />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlExtraCls" ppName="割増区分" ppNameVisible="False" ppClassCD="0053" ppMode="名称" ppValidationGroup="Amount" />
            </td>
            <td style="white-space:nowrap">
                <asp:Button ID="btnAmoClear" runat="server" Text="クリア" />
                &nbsp;<asp:Button ID="btnAmoAdd" runat="server" Text="追加" ValidationGroup="Amount" />
                &nbsp;<asp:Button ID="btnAmoUpdate" runat="server" Text="更新" ValidationGroup="Amount" />
                &nbsp;<asp:Button ID="btnAmoDel" runat="server" Text="削除" />
            </td>
        </tr>
    </table>
    
    
    <table class="center">
        <tr>
            <td>
                <div id="DivOut1" runat="server" class="grid-out" style="width: 970px;">
                    <div id="DivIn1" runat="server" class="grid-in">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <hr />

    <table border="1" class="center">
        <colgroup style="width: 200px"></colgroup>
        <colgroup style="width: 150px"></colgroup>
        <colgroup style="width: 200px"></colgroup>
        <colgroup style="width: 150px"></colgroup>

        <tr>
            <td>
                <asp:Label ID="lblLName1" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice1" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit1" runat="server" Text="円"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLName6" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice6" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit6" runat="server" Text="円"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLName2" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice2" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit2" runat="server" Text="円"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLName7" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice7" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit7" runat="server" Text="円"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLName3" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice3" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit3" runat="server" Text="円"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLName3to7" runat="server" Text="　　（３～７小計）"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice3to7" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit3to7" runat="server" Text="円"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLName4" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice4" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit4" runat="server" Text="円"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLName8" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice8" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit8" runat="server" Text="円"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblLName5" runat="server"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPrice5" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnit5" runat="server" Text="円"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblLNameSum" runat="server" Text="合計"></asp:Label>
            </td>
            <td class="float-right">
                <asp:Label ID="lblPriceSum" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblUnitSum" runat="server" Text="円"></asp:Label>
            </td>
        </tr>
    </table>

    <asp:Panel ID="Panel2" runat="server" CssClass="text-center">
        <asp:Button ID="btnRecalculation" runat="server" Text="再計算" />
    </asp:Panel>

    <hr />

    <table border="1" class="center">
        <tr>
            <td style="width: 150px">
                <asp:Label ID="lblListLName3" runat="server"></asp:Label>
            </td>
            <td class="align-top">
                <div id="DivOut3" runat="server" class="grid-out" style="width: 100%">
                    <div id="DivIn3" runat="server" class="grid-in" style="height: auto;">
                        <input id="hdnData3" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvData3" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
            <td style="width: 150px">
                <asp:Label ID="lblListLName4" runat="server"></asp:Label>
            </td>
            <td class="align-top">
                <div id="DivOut4" runat="server" class="grid-out" style="width: 100%">
                    <div id="DivIn4" runat="server" class="grid-in" style="height: auto;">
                        <input id="hdnData4" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvData4" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblListLName5" runat="server"></asp:Label>
            </td>
            <td class="align-top">
                <div id="DivOut5" runat="server" class="grid-out" style="width: 100%">
                    <div id="DivIn5" runat="server" class="grid-in" style="height: auto;">
                        <input id="hdnData5" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvData5" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
            <td>
                <asp:Label ID="lblListLName6" runat="server"></asp:Label>
            </td>
            <td class="align-top">
                <div id="DivOut6" runat="server" class="grid-out" style="width: 100%">
                    <div id="DivIn6" runat="server" class="grid-in" style="height: auto;">
                        <input id="hdnData6" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvData6" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblListLName7" runat="server"></asp:Label>
            </td>
            <td class="align-top">
                <div id="DivOut7" runat="server" class="grid-out" style="width: 100%">
                    <div id="DivIn7" runat="server" class="grid-in" style="height: auto;">
                        <input id="hdnData7" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvData7" runat="server">
                        </asp:GridView>
                    </div>
                </div>
                <asp:CustomValidator ID="cuvGrid7" runat="server" ErrorMessage="CustomValidator" CssClass="errortext" ValidationGroup="Update"></asp:CustomValidator>
            </td>
            <td>
                <asp:Label ID="lblListLName8" runat="server"></asp:Label>
            </td>
            <td class="align-top">
                <div id="DivOut8" runat="server" class="grid-out" style="width: 100%">
                    <div id="DivIn8" runat="server" class="grid-in" style="height: auto;">
                        <input id="hdnData8" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvData8" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td class="float-right">
                <asp:Panel ID="Panel1" runat="server">
                    <asp:Label ID="Label53" runat="server" Text="備考"></asp:Label>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="float-right">
                <uc:ClsCMTextBox runat="server" ID="txtNotetext" ppName="備考" ppIMEMode="全角" ppNameVisible="False" ppMaxLength="250" ppTextMode="MultiLine" ppHeight="100" ppWidth="400" ppValidationGroup="Update" />
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errortext" ValidationGroup="Update" />
    <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="errortext" ValidationGroup="Amount" />
    </asp:Content>
