<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="SERUPDP001.aspx.vb" Inherits="SPC.SERUPDP001" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Reference.Master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">

    <script type="text/javascript">
        function ddlReload(ddl) {
            var count = ddl.length
            var options = ddl.options
            while (true) {
                if (options[0].text != '') {
                    if (count == 1) {
                        options[0].text = ''
                    } else {
                        ddl.removeChild(options[0]);
                    }
                }
                else {
                    break;
                }
            }
        }
    </script>

    <!--【検索条件】-->
    <%--<div class="text-center">
        <asp:Label ID="lblSearchCondition" runat="server" Text="検索条件" CssClass="title"></asp:Label>
    </div>
    <br>--%>
    <table style="width: 95%;" class="center" border="0">
        <tr>
            <!--シリアル番号-->
            <td>
                <uc:ClsCMTextBox ID="txtCndSerialNo" runat="server" ppIMEMode="半角_変更不可"
                    ppMaxLength="20" ppName="シリアル番号" ppNameWidth="85" ppWidth="150" ppCheckHan="True" />
            </td>
            <!--現設置／保管場所-->
            <td colspan="3">
                <asp:Panel ID="Panel1" runat="server" CssClass="float-left">
                    <uc:ClsCMDropDownList runat="server" ID="ddlCndPlaceCls" ppName="現設置／保管場所" ppNameWidth="110"
                        ppWidth="110" ppMode="名称" ppClassCD="0048" ppNotSelect="True" />
                </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" CssClass="float-left">
                    <uc:ClsCMTextBox ID="txtCndStrageCd" runat="server" ppWidth="90" ppName="現設置／保管場所"
                        ppNameVisible="false" ppMaxLength="8" ppIMEMode="半角_変更不可" ppCheckHan="true" />
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" CssClass="float-left" Style="margin-left: 12px; margin-top: 6px">
                    <asp:Label ID="lblPlaceNm" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <!--機器分類-->
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblAppaDiv" runat="server" Text="機器分類" Width="85"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCndAppaDiv" runat="server" Width="110" AutoPostBack="True"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <!--機器種別-->
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblAppaCls" runat="server" Text="機器種別" Width="110"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCndAppaCls" runat="server" Width="215"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <!--システム-->
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSystem" runat="server" Text="システム" Width="85"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCndSystem" runat="server" Width="110"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <!--管理番号-->
            <td>
                <uc:ClsCMTextBox ID="txtCndCntlNo" runat="server" ppIMEMode="半角_変更不可"
                    ppMaxLength="15" ppName="管理番号" ppNameWidth="110" ppWidth="120" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <!--移動日-->
            <td>
                <uc:ClsCMDateBox runat="server" ID="dtbCndMoveDt" ppDateFormat="年月日" ppName="移動日" ppNameWidth="85" />
            </td>
            <!--移動理由-->
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblMoveReason" runat="server" Text="移動理由" Width="110"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCndMoveReason" runat="server" Width="215"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <!--納入予定日-->
            <td>
                <uc:ClsCMDateBox runat="server" ID="dtbCndDlvPlndt" ppDateFormat="年月日" ppName="納入予定日" ppNameWidth="85" />
            </td>
            <!--納入日-->
            <td>
                <uc:ClsCMDateBox runat="server" ID="dtbCndDlvDt" ppDateFormat="年月日" ppName="納入日" ppNameWidth="110" />
            </td>
        </tr>
        <tr>
            <td>
                 <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="85" ppWidth="110" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <!--【検索結果（明細）】-->
    <asp:Panel ID="pnlResultDetail" runat="server">
        <hr />
        <%--<br style="border-top-style: none">
        <div class="text-center">
            <asp:Label ID="lblSearchResult" runat="server" Text="検索結果" CssClass="title"></asp:Label>
        </div>
        <br>
        <div class="text-center">
            <asp:Label ID="lblDetail" runat="server" Text="明細"></asp:Label>
        </div>--%>
        <!--更新日時-->
        <asp:Label ID="lblUpdateDt" runat="server" Text="9999-99-99 99:99:99.999"
            Visible="false"></asp:Label>
        <br>
        <table style="width: 85%;" class="center" border="1">
            <tr>
                <td style="width: 200px; text-align: center;">機器分類</td>
                <td style="text-align: center">機器種別</td>
                <td style="text-align: center" colspan="2">シリアル番号／連番</td>
                <td style="text-align: center">システム</td>
                <td style="text-align: center">ＶＥＲ</td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <br>
                    <asp:DropDownList ID="ddlRstAppaDiv" runat="server" Width="140"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valRstAppaDiv" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>
                </td>
                <td style="text-align: center">
                    <br>
                    <asp:DropDownList ID="ddlRstAppaCls" runat="server" Width="180"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valRstAppaCls" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>
                </td>
                <td colspan="2" style="padding-left: 10px; text-align: center">
                    <table>
                        <tr>
                            <td>
                                <uc:ClsCMTextBox runat="server" ID="txtRstSerialNo" ppName="シリアル番号" ppNameVisible="False"
                                    ppIMEMode="半角_変更不可" ppMaxLength="20" ppCheckHan="True" ppRequiredField="true"
                                    ppWidth="150" ppValidationGroup="Detail" />
                            </td>
                            <td>
                                <asp:Label ID="lblSeq" runat="server" Text=""></asp:Label>

                            </td>
                        </tr>
                    </table>

                </td>
                <td style="text-align: center">
                    <br>
                    <asp:DropDownList ID="ddlRstSystem" runat="server" Width="140"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valRstSystem" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>

                </td>
                <td style="text-align: center">
                    <uc:ClsCMDropDownList runat="server" ID="ddlRstVersion" ppName="ＶＥＲ" ppNameVisible="false" ppWidth="85" ppValidationGroup="Detail" ppClassCD="0015" />
                    <%--<uc:ClsCMTextBox runat="server" ID="txtRstVersion" ppName="ＶＥＲ" ppNameVisible="False"
                        ppIMEMode="半角_変更不可" ppMaxLength="10" ppCheckHan="True"
                        ppWidth="85" ppValidationGroup="Detail" />--%>
                </td>

                <%--<uc:ClsCMTextBox runat="server" ID="txtCondNm" ppName="持参物品条件" ppNameVisible="False"
                        ppIMEMode="半角_変更可" ppMaxLength="20" ppCheckHan="False"
                        ppWidth="150" ppValidationGroup="Detail" />--%>
            </tr>
            <tr>
                <td style="text-align: center">型式／機器</td>
                <td style="width: 200px; text-align: center">機器備考</td>
                <td style="text-align: center">HDD No.　HDD種別</td>
                <td style="text-align: center" colspan="2">現設置／保管場所</td>
                <td style="text-align: center">移動日</td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <br>
                    <asp:DropDownList ID="ddlRstAppaModel" runat="server" Width="180"></asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valRstAppaModel" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>
                    <br>
                </td>
                <td style="text-align: center">
                    <br>
                    <asp:DropDownList ID="ddlCondNm" runat="server" Width="180"></asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valCondNm" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>
                </td>
                <td style="text-align: center">
                    <br>
                    <asp:DropDownList ID="ddlRstHddNo" runat="server" Width="60"></asp:DropDownList>
                    <asp:DropDownList ID="ddlRstHddCls" runat="server" Width="60"></asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valRstHddNo" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>
                </td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td style="text-align: center">
                                <uc:ClsCMDropDownList runat="server" ID="ddlRstPlaceCls" ppWidth="100" ppMode="名称"
                                    ppClassCD="0048" ppNotSelect="True" ppNameWidth="20" ppName="現設置／保管場所区分"
                                    ppNameVisible="false" ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>
                            <td style="text-align: center">
                                <uc:ClsCMTextBox runat="server" ID="txtRstStrageCd" ppName="現設置／保管場所" ppNameVisible="False"
                                    ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppRequiredField="true"
                                    ppWidth="85" ppValidationGroup="Detail" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 24px">
                                <asp:Label ID="lblRstStrageNm" runat="server" Text=""></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </td>
                <td style="text-align: center">
                    <uc:ClsCMDateBox runat="server" ID="dtbRstMoveDt" ppName="移動日" ppNameVisible="False"
                        ppRequiredField="true" ppValidationGroup="Detail" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">納入予定日</td>
                <td style="text-align: center" colspan="2">移動理由</td>
                <td style="text-align: center" colspan="2">移動先</td>
                <td style="text-align: center">納入日</td>
            </tr>
            <tr>
                <td style="padding-left: 24px; text-align: center">
                    <uc:ClsCMDateBox runat="server" ID="dtbRstDlvPlndt" ppName="納入予定日" ppNameVisible="False"
                        ppValidationGroup="Detail" />
                </td>
                <td style="text-align: center" colspan="2">
                    <br>
                    <asp:DropDownList ID="ddlRstMoveReason" runat="server" Width="250"></asp:DropDownList>
                    <br>
                    <asp:CustomValidator ID="valRstMoveReason" runat="server" CssClass="errortext"
                        ValidationGroup="Detail"></asp:CustomValidator>
                </td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td style="text-align: center">
                                <uc:ClsCMDropDownList runat="server" ID="ddlRstMoveCls" ppWidth="100" ppMode="名称"
                                    ppClassCD="0048" ppNotSelect="True" ppNameWidth="20" ppName="移動先区分"
                                    ppNameVisible="false" ppRequiredField="true" ppValidationGroup="Detail" />
                            </td>
                            <td style="text-align: center">
                                <uc:ClsCMTextBox runat="server" ID="txtRstMoveCd" ppName="移動先" ppNameVisible="False"
                                    ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppRequiredField="true"
                                    ppWidth="85" ppValidationGroup="Detail" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; padding-left: 24px">
                                <asp:Label ID="lblRstMoveNm" runat="server" Text="" Style="text-align: left"></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </td>
                <td style="text-align: center">
                    <uc:ClsCMDateBox runat="server" ID="dtbRstDlvDt" ppName="納入日" ppNameVisible="False"
                        ppValidationGroup="Detail" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">管理番号</td>
                <td style="text-align: center">物品転送Ｎｏ．</td>
                <td style="text-align: center" colspan="4">備考</td>
            </tr>
            <tr>
                <td style="padding-left: 20px; text-align: center">
                    <uc:ClsCMTextBox runat="server" ID="txtRstCntlNo" ppName="管理番号" ppNameVisible="False"
                        ppIMEMode="半角_変更不可" ppMaxLength="15" ppCheckHan="True" ppWidth="120"
                        ppValidationGroup="Detail" />
                </td>

                <td style="padding-left: 20px; text-align: center">
                    <uc:ClsCMTextBox runat="server" ID="txtRstArclNo" ppName="物品転送Ｎｏ．" ppNameVisible="False"
                        ppIMEMode="半角_変更不可" ppMaxLength="15" ppCheckHan="True" ppWidth="120"
                        ppValidationGroup="Detail" />
                </td>

                <td colspan="4" style="text-align: center">
                    <uc:ClsCMTextBox runat="server" ID="txtRstNotetext" ppName="備考" ppNameVisible="False"
                        ppMaxLength="50" ppWidth="640"
                        ppValidationGroup="Detail" ppIMEMode="全角" />
                </td>
            </tr>
        </table>
        <table style="width: 85%; text-align: right;" class="center">
            <tr>
                <td style="border-top-style: none">
                    <asp:Button ID="btnDetailInsert" runat="server" Text="追加" ValidationGroup="Detail" />
                    <asp:Button ID="btnDetailUpdate" runat="server" Text="更新" ValidationGroup="Detail" />
                    <asp:Button ID="btnDetailDelete" runat="server" Text="削除" />
                    <asp:Button ID="btnDetailClear" runat="server" Text="クリア" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="valSumDetail" runat="server" CssClass="errortext" ValidationGroup="Detail" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <!--【検索結果（グリッド）】-->
    <div class="grid-out">
        <div class="grid-in" style="height: 162px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
