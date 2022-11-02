<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference_Reverse.master" CodeBehind="BRKLSTP001.aspx.vb" Inherits="SPC.BRKLSTP001" %>

<%@ MasterType VirtualPath="~/Reference_Reverse.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <script type="text/javascript">

        onload = pageonload

        function pageonload() {
            //ヒドゥンの取得
            var element = document.getElementById("cphMainContent_cphSearchContent_hdnScrollTop");
            if (element.value == 0) {
                window.scroll(0, 0);
            }
        }

    </script>
    <asp:HiddenField ID="hdnScrollTop" runat="server" Value="0" />
    <table class="center" style="width: 95%; text-align: left">
        <tr style="height: 10px;">
            <td>
                <uc:ClsCMTextBoxFromTo ID="txtTboxID" runat="server" ppName="TBOXID" ppNum="True" ppCheckHan="True" ppMaxLength="8" ppWidth="70px" ppNameWidth="75px" ppIMEMode="半角_変更不可" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDateBoxFromTo ID="txtUketukeDtFmTo" runat="server" ppName="受付日" ppNameWidth="110px" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td colspan="2">
                <span style="position: relative; left: 86px">
                    <uc:ClsCMTextBoxFromTo ID="txtKanriNo" runat="server" ppName="管理番号" ppCheckHan="True" ppMaxLength="15" ppWidth="150px" ppNameWidth="95px" ppIMEMode="半角_変更不可" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
        </tr>
        <tr style="height: 10px;">
            <td>
                <uc:ClsCMTextBox ID="txtHallName" runat="server" ppMaxLength="50" ppName="ﾎｰﾙ名" ppWidth="450px" ppNameWidth="75px" ppIMEMode="全角" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
            <td>
<%--                <uc:ClsCMTextBox ID="txtNlKbn" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="NL区分" ppWidth="15" ppNameWidth="90px" ppValidationGroup="Detail1" ppCheckHan="True" ppFontSize="12pt" />--%>
                <uc:ClsCMDropDownList ID="ddlNlCls" runat="server" ppName="NL区分" ppNameWidth="110px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Datail1" ppFontSize="12pt" />
           </td>
<%--            <td>
                <span style="position: relative;">
                    <uc:ClsCMTextBox ID="txtEwKbn" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="EW区分" ppWidth="15px" ppNameWidth="70px" ppValidationGroup="Detail1" ppCheckHan="True" ppFontSize="12pt" />
                </span>
            </td>--%>
            <td colspan="2">
                <span style="position: relative; left: 86px">
                    <label style="font-size: 12pt; width: 70px">都道府県</label><span style="position: relative; left: 35px;"><asp:DropDownList runat="server" ID="ddlPrefectureFm" Width="120px" ValidationGroup="Detail1" Style="font-size: 12pt;" /><label> ～ </label>
                        <asp:DropDownList runat="server" ID="ddlPrefectureTo" Width="120px" ValidationGroup="Detail1" Style="font-size: 12pt;" /></span></span><br />
                <span style="position: relative; top: 2px; left: 170px;">
                    <asp:CustomValidator runat="server" ID="cuvPrefecture" ControlToValidate="ddlPrefectureFm" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail1" /></span>
            </td>
        </tr>
        <tr style="height: 10px;">
<%--            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlAppaCls" runat="server" ppName="機種区分" ppNameWidth="90px" ppWidth="150px" ppClassCD="0042" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlBlngCls" runat="server" ppName="所属区分" ppNameWidth="70px" ppWidth="150px" ppClassCD="0034" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>--%>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlCallCls" runat="server" ppName="ｺｰﾙ区分" ppNameWidth="75px" ppWidth="150px" ppClassCD="0033" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlSalesOfficeInCharge" runat="server" ppName="担当営業所" ppNameWidth="110px" ppWidth="150px" ppClassCD="0042" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative; left: 86px">
                    <uc:ClsCMDropDownList ID="ddlMembershipManagementCompany" runat="server" ppName="会員管理会社" ppNameWidth="95px" ppWidth="150px" ppClassCD="0034" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
        </tr>
        <tr style="height: 10px;">
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlReporter1" runat="server" ppName="申告元1" ppNameWidth="75px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlReporter2" runat="server" ppName="申告元2" ppNameWidth="110px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative; left: 86px">
                    <uc:ClsCMTextBox ID="txtUketukeNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="受付者" ppWidth="148px" ppNameWidth="95px" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>                
            </td>
<%--            <td colspan="2">
                <span style="position: relative;">
                    <label style="font-size: 12pt; margin-left: 2px">作業状況</label><span style="position: relative; left: 12px;"><asp:DropDownList runat="server" ID="ddlSagyoJokyoFm" Width="170px" ValidationGroup="Detail1" Style="font-size: 12pt;" /><label> ～ </label>
                        <asp:DropDownList runat="server" ID="ddlSagyoJokyoTo" Width="170px" ValidationGroup="Detail1" Style="font-size: 12pt;" /></span></span><br />
                <span style="position: relative; top: 2px; left:79px">
                    <asp:CustomValidator runat="server" ID="cuvddlSagyoJokyo" ControlToValidate="ddlSagyoJokyoFm" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail1" /></span>
            </td>--%>
        </tr>
        <tr style="height: 10px;">
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlEquipmentCls1" runat="server" ppName="機器分類1" ppNameWidth="75px" ppWidth="150px" ppClassCD="0042" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlEquipmentCls2" runat="server" ppName="機器分類2" ppNameWidth="110px" ppWidth="150px" ppClassCD="0042" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative; left: 86px">
                    <uc:ClsCMDropDownList ID="ddlEquipmentName" runat="server" ppName="機器名" ppNameWidth="95px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
        </tr>
        <tr style="height: 10px;">
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlErrorCode" runat="server" ppName="ｴﾗｰｺｰﾄﾞ" ppNameWidth="75px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtDetailCode" runat="server" ppMaxLength="50" ppName="詳細ｺｰﾄﾞ" ppWidth="450px" ppNameWidth="110px" ppIMEMode="全角" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
            <td>
                <span style="position: relative; left: 86px">
                    <uc:ClsCMTextBox ID="txtVersion" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="20" ppName="VER" ppWidth="150px" ppNameWidth="95px" ppValidationGroup="Detail1" ppCheckHan="True" ppFontSize="12pt" />
                </span>
            </td>
        </tr>
        <tr style="height: 10px;">
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlHitoAgency" runat="server" ppName="筆頭代理店" ppNameWidth="75px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlGyomuitakuAgency" runat="server" ppName="業務委託代理店" ppNameWidth="110px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative; left: 86px">
                    <uc:ClsCMDropDownList ID="ddlAgency" runat="server" ppName="代理店" ppNameWidth="95px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
        </tr>
        <tr style="height: 10px;">
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlFinalReception" runat="server" ppName="最終受付" ppNameWidth="75px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlProgress" runat="server" ppName="進捗状況" ppNameWidth="110px" ppWidth="150px" ppClassCD="0033" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <span style="position: relative; left: 86px">
                    <uc:ClsCMDateBoxFromTo ID="txtKakuninDtFmTo" runat="server" ppName="確認日" ppNameWidth="95px" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
        </tr>
    </table>
    <div style="height: 2px;"></div>
    <table class="center" style="width: 95%; text-align: left">
        <tr>
            <td style="width: 84px; vertical-align: top; padding-top: 12px;">
                <asp:Label ID="lblTboxClass" Style="font-size: 12pt; margin-left: 2px" runat="server" Text="TBOXﾀｲﾌﾟ" />
            </td>
            <td>
                <asp:CheckBoxList ID="cklTboxClass" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                    RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium">
                    <asp:ListItem>T-PC</asp:ListItem>
                    <asp:ListItem>IT130S</asp:ListItem>
                    <asp:ListItem>IT135S</asp:ListItem>
                    <asp:ListItem>NVC100</asp:ListItem>
                    <asp:ListItem>NVC100S</asp:ListItem>
                    <asp:ListItem>NVC300</asp:ListItem>
                    <asp:ListItem>HTB(B)</asp:ListItem>
                    <asp:ListItem>HTB(L)</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td style="width: 84px; vertical-align: top; padding-top: 12px;">
                <asp:Label ID="lblMdnWork" Style="font-size: 12pt; margin-left: 2px" runat="server" Text="MDN作業" />
            </td>
            <td>
                <asp:CheckBoxList ID="cklMdnWork" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                    RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium">
                    <asp:ListItem>新規</asp:ListItem>
                    <asp:ListItem>INS→光</asp:ListItem>
                    <asp:ListItem>UPS疎通</asp:ListItem>
                    <asp:ListItem>ｵｰﾀﾞｰ設定(許可)､ｵｰﾀﾞｰ設定(禁止)</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td style="width: 84px; vertical-align: top; padding-top: 12px;">
                <asp:Label ID="lblMdnMaintenance" Style="font-size: 12pt; margin-left: 2px" runat="server" Text="MDN保守" />
            </td>
            <td>
                <asp:CheckBoxList ID="cklMdnMaintenance" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                    RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium">
                    <asp:ListItem>ﾘﾓｰﾄ</asp:ListItem>
                    <asp:ListItem>SV1交換</asp:ListItem>
                    <asp:ListItem>SV2交換</asp:ListItem>
                    <asp:ListItem>CL交換</asp:ListItem>
                    <asp:ListItem>HDD交換</asp:ListItem>
                    <asp:ListItem>その他</asp:ListItem>
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <%--<span style="position: relative; left: 4px;">
            <label style="font-size: 12pt;">TBOXﾀｲﾌﾟ</label></span>
        <div class="grid-out" style="position: relative; bottom: 15px; left: 104px; width: 268px; height: 90px;">
            <div class="grid-in" style="height: 400px;">
                <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvListTboxcls" runat="server" Width="300px"></asp:GridView>
            </div>
        </div>--%>
    <table class="center" style="width: 95%; text-align: left;">
        <tr>
            <td>
                <span style="position: relative;">
                    <uc:ClsCMDropDownList ID="ddlMdnType" runat="server" ppName="MDN種別" ppNameWidth="75px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
                </span>
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlHosyukeiyaku" runat="server" ppName="保守契約" ppNameWidth="75px" ppWidth="150px" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
        </tr>
        <tr style="height: 10px;">
            <td style="width: 600px;">
                <uc:ClsCMTextBox ID="txtShinsei" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="申告内容" ppWidth="450px" ppNameWidth="90px" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
            <td style="width: 600px;">
                <uc:ClsCMTextBox ID="txtShinseiDtl" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="申告内容詳細" ppWidth="450px" ppNameWidth="125px" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
        </tr>
        <tr style="height: 10px;">
            <td>
                <uc:ClsCMTextBox ID="txtSyochi" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="引継内容" ppWidth="450px" ppNameWidth="90px" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtTaioShinchokuJk" runat="server" ppMaxLength="50" ppName="対応内容" ppWidth="450px" ppNameWidth="90px" ppIMEMode="全角" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSyochiDtl" runat="server" ppMaxLength="50" ppName="備考" ppWidth="450px" ppNameWidth="125px" ppIMEMode="全角" ppValidationGroup="Detail1" ppFontSize="12pt" />
            </td>
        </tr>
        </table>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummaryUpdate" runat="server" CssClass="errortext" ValidationGroup="Detail1" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="divCount" runat="server" class="float-Left">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" Style="font-size: 12pt;"></asp:Label>
                </td>
                <td style="width: 80px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX" Style="font-size: 12pt;"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" Style="font-size: 12pt;"></asp:Label>
                </td>
                <td style="width: 15px"></td>
                <td>
                    <asp:Button ID="btnReload" runat="server" CssClass="center" Text="リロード" />
                </td>
            </tr>
        </table>
    </div>
    <div class="grid-out" style="height: 649px">
        <div class="grid-in" style="height: 649px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server" CssClass ="GridView_Font">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
