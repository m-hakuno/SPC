<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="TBPINPP001.aspx.vb" Inherits="SPC.TBPINPP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <!--【検索条件】-->
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <!--工事日-->
            <td>
                <uc:ClsCMDateBoxFromTo runat="server" ID="dtbCnstDt" ppName="工事日" ppNameWidth="80"
                    ppDateFormat="年月日" />
            </td>
            <!--ＴＢＯＸＩＤ-->
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="txtTboxid" ppName="ＴＢＯＸＩＤ" ppNameWidth="100"
                    ppCheckHan="True" ppIMEMode="半角_変更不可" ppMaxLength="8" ppWidth="70" />
            </td>
        </tr>
        <tr>
            <!--システム-->
            <td>
                <br>
                <asp:Label ID="Label7" runat="server" Text="システム" Width="80"></asp:Label>
                <asp:DropDownList ID="ddlSystemFm" runat="server" Width="160"></asp:DropDownList>
                <asp:Label ID="Label8" runat="server" Text="～" Width="15"></asp:Label>
                <asp:DropDownList ID="ddlSystemTo" runat="server" Width="160"></asp:DropDownList>
                <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:CustomValidator ID="valSystem" runat="server" CssClass="errortext"></asp:CustomValidator>
            </td>
            <!--吸上げ処理日-->
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMDateBoxFromTo runat="server" ID="dtbSuckDt" ppName="吸上げ処理日"
                                ppNameWidth="97" ppDateFormat="年月日" />
                        </td>
                        <td>
                            &nbsp;
                            <asp:CheckBox ID="cbxSuckDt" runat="server" Text="吸上げ処理日空欄検索" />
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
                            <!--未集信-->
                            <asp:Label ID="lblMishushin" runat="server" Text="未集信" Width="80"></asp:Label>
                            <asp:CheckBox ID="cbxMishushinAri" runat="server" Text="あり" />
                            &nbsp;&nbsp;
                            <asp:CheckBox ID="cbxMishushinNashi" runat="server" Text="なし" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
    <asp:Panel ID="pnlResultDetail" runat="server" BackColor="#FFD6D6" BorderStyle="Solid" BorderWidth="1">
        <!--【検索結果（明細）】-->
        <table style="width:1050px;" class="center" border="0">
            <!--１行目-->
            <tr>
                <td>
                    <table  style="width:100%" border="0">
                        <tr>
                            <td>
                                <table  style="width:100%" border="0">
                                    <tr>
                                        <!--工事日-->
                                        <td style="width: 150px">
                                            <asp:Label ID="Label1" runat="server" Text="工事日"></asp:Label>
                                            <asp:Label ID="lblCnstDtDtl" runat="server" Text="9999/99/99"></asp:Label>
                                        </td>
                                        <!--ＴＢＯＸＩＤ-->
                                        <td style="width: 200px">
                                            <uc:ClsCMTextBox runat="server" ID="txtTboxidDtl" ppName="ＴＢＯＸＩＤ" ppNameWidth="80"
                                                ppMaxLength="8" ppIMEMode="半角_変更不可" ppRequiredField="true" ppCheckHan="True"
                                                ppWidth="70" ppValidationGroup="Detail" />
                                        </td>
                                        <!--ホール名-->
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="ホール名"></asp:Label>
                                            <asp:Label ID="lblHallNmDtl" runat="server" Text="ＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸＸ"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--２行目-->
            <tr>
                <td>
                    <table style="width:100%" border="0">
                        <tr>
                            <td>
                                <table  style="width:100%" border="0">
                                    <tr>
                                        <!--システム-->
                                        <td style="width: 200px">
                                            <asp:Label ID="Label3" runat="server" Text="システム"></asp:Label>
                                            <asp:Label ID="lblSystemDtl" runat="server" Text="XXXXXXXXXXXXXXXXXXXX"></asp:Label>
                                        </td>
                                        <!--工事依頼番号-->
                                        <td style="width: 220px">
                                            <asp:Label ID="Label4" runat="server" Text="工事依頼番号"></asp:Label>
                                            <asp:Label ID="lblCnstNoDtl" runat="server" Text="XXXXXXXXXXXXXXX"></asp:Label>
                                        </td>
                                        <!--最終営業日-->
                                        <td style="width: 200px">
                                            <uc:ClsCMDateBox runat="server" ID="dtbLastBDtDtl" ppName="最終営業日"
                                                ppDateFormat="年月日" ppNameWidth="70" ppValidationGroup="Detail" />
                                        </td>
                                        <td style="width: 200px">
                                        <!--最終集信日-->
                                            <uc:ClsCMDateBox runat="server" ID="dtbLastSDtDtl" ppName="最終集信日"
                                                ppDateFormat="年月日" ppNameWidth="70" ppValidationGroup="Detail" />
                                        </td>
                                        <!--撤去区分-->
                                        <td>
                                            <uc:ClsCMDropDownList runat="server" ID="ddlRemoveClsDtl" ppName="撤去区分"
                                                ppNameWidth="50" ppWidth="100" ppClassCD="0052" ppNotSelect="true"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--３行目-->
            <tr>
                <td>
                    <table  style="width:100%" border="0">
                        <tr>
                            <td>
                                <table  style="width:100%" border="0">
                                    <tr>
                                        <!--未集信-->
                                        <td style="width: 150px">
                                            <asp:Label ID="Label5" runat="server" Text="未集信"></asp:Label>
                                            <asp:Label ID="lblMishushinDtl" runat="server" Text="ＸＸ"></asp:Label>
                                        </td>
                                        <!--未集信理由-->
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="未集信理由"></asp:Label>
                                            <asp:DropDownList ID="ddlUnclctReasonDtl" runat="server" Width="500"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--４、５行目-->
            <tr>
                <td>
                    <table style="width:100%" border="0">
                        <tr>
                            <!--物品到着日-->
                            <td style="width :300px">
                                <uc:ClsCMDateBox runat="server" ID="dtbArtclArvDtDtl" ppName="物品到着日" ppDateFormat="年月日"
                                    ppNameWidth="130" ppValidationGroup="Detail" />
                            </td>
                            <!--吸上げ処理日-->
                            <td style="width :330px">
                                <uc:ClsCMDateBox runat="server" ID="dtbSuckDtDtl" ppName="吸上げ処理日" ppDateFormat="年月日"
                                    ppNameWidth="140" ppValidationGroup="Detail" />
                            </td>
                            <!--物品返却日-->
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dtbArtclRetDtDtl" ppName="物品返却日" ppDateFormat="年月日"
                                    ppNameWidth="70" ppValidationGroup="Detail" />
                            </td>
                        </tr>
                        <tr>
                            <!--マスタ廃止日（ＦＳ）-->
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dtbMstRepFSDtDtl" ppName="マスタ廃止日（ＦＳ）"
                                    ppDateFormat="年月日" ppNameWidth="130" ppValidationGroup="Detail" />
                            </td>
                            <!--マスタ廃止日（ＮＧＣ）-->
                            <td>
                                <uc:ClsCMDateBox runat="server" ID="dtbMstRepNGCDtDtl" ppName="マスタ廃止日（ＮＧＣ）"
                                    ppDateFormat="年月日" ppNameWidth="140" ppValidationGroup="Detail" />
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--６行目-->
            <tr>
                <td>
                    <table style="width:100%" border="0" >
                        <tr>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtNoteTextDtl" ppName="備考" ppNameWidth="130"
                                    ppWidth="450" ppHeight="44" ppTextMode="MultiLine" ppMaxLength="100"
                                    ppValidationGroup="Detail" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="float-left">
            <asp:ValidationSummary ID="vasSummaryDetail" runat="server" CssClass="errortext" ValidationGroup="Detail" />
        </div>
        <div class="float-right">
            <asp:Button ID="btnDetailInsert" runat="server" Text="追加" Width="80" ValidationGroup="Detail" />&nbsp;
            <asp:Button ID="btnDetailUpdate" runat="server" Text="変更" Width="80" ValidationGroup="Detail" />&nbsp;
            <asp:Button ID="btnDetailDelete" runat="server" Text="削除" Width="80" />&nbsp;
            <asp:Button ID="btnDetailCncnt" runat="server" Text="即時集信" Width="80" />&nbsp;
            <asp:Button ID="btnDetailPrint" runat="server" Text="マスタ廃止" Width="80" />&nbsp;
        </div>
        <br>
        <br>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <!--【検索結果（グリッド）】-->
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
