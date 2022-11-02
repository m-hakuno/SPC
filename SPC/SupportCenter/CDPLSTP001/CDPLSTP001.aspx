<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CDPLSTP001.aspx.vb" Inherits="SPC.CDPLSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:700px;" border="0">
        <tr>
            <td colspan ="2"><uc:ClsCMTextBoxFromTo ID="txtSrcTBoxFrTo" runat ="server" ppIMEMode="半角_変更不可" ppName="ＴＢＯＸＩＤ" ppNameWidth ="100px" ppWidth ="70px" ppTextAlign="左" ppTabIndex="1" ppMaxLength="8" ppNum="true" ppValidationGroup="Detail" /></td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="txtSrcKanriNoFrTo" runat ="server" ppIMEMode="半角_変更不可" ppName="管理番号" ppNameWidth ="100px" ppWidth ="75px" ppTextAlign="左" ppTabIndex="2" ppMaxLength="9" ppNum="true" ppValidationGroup="Detail" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="txtSrcNoFrTo" runat ="server" ppIMEMode="半角_変更不可" ppName="Ｎｏ．" ppNameWidth ="60px" ppWidth ="45px" ppTextAlign="左" ppTabIndex="3" ppMaxLength="5" ppNum="true" ppValidationGroup="Detail" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="txtSrcJuryoFrTo" runat="server" ppName ="受領" ppDateFormat ="年月日" ppNameWidth ="100px" ppTextAlign="左" ppTabIndex="4" ppNum="true" ppValidationGroup="Detail" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="txtSrcKensyuMonthFrTo" runat ="server" ppIMEMode="半角_変更不可" ppName="検収月" ppNameWidth ="60px" ppWidth ="40px" ppTextAlign="左" ppTabIndex="5" ppMaxLength="4" ppNum="true" ppExpression="[0-9][0-9]([0][1-9]|[1][0-2])" ppValidationGroup="Detail" />
            </td>
        </tr>
    </table>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummarySearch" runat="server" CssClass="errortext" ValidationGroup="Detail" TabIndex="6" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
  <asp:Panel ID="pnlRegister" runat="server"  BorderStyle="Solid" BorderWidth="1px">
    <table style="width:1050px;" border="0">
        <tr>
            <td>
                <label style="width:100px;padding-left:2px;" >Ｎｏ．</label><asp:Label ID="lblNo" runat="server" TabIndex="7" />
            </td>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtKanriNo" runat="server" ppName="管理番号" ppNameWidth="75px" ppWidth="75px" ppTextAlign="左" ppTabIndex="8" ppMaxLength="9" ppNum="true" ppIMEMode="半角_変更不可" ppRequiredField="true" ppValidationGroup="Detail2" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtTBoxId" runat="server" ppRequiredField="true" ppName="ＴＢＯＸＩＤ" ppNameWidth="100px" ppWidth="70px" ppTextAlign="左" ppTabIndex="9" ppMaxLength="8" ppNum="true" ppIMEMode="半角_変更不可" ppValidationGroup="Detail2" />
            </td>
            <td>
                <label style="padding-left:2px;">ＮＬ区分</label><span style="padding-left:27px;"><asp:Label ID="lblNLKbn" runat="server" TabIndex="10" /><asp:HiddenField runat="server" ID="hdnNLKbn" /></span>
            </td>
            <td>
                <label style="padding-left:2px;">種別</label><span style="padding-left:27px;"><asp:Label ID="lblSyubetu" runat="server" TabIndex="11" /></span>
            </td>
            <td>
                <label style="padding-left:2px;">ＶＥＲ</label><span style="padding-left:27px;"><asp:Label ID="lblVersion" runat="server" TabIndex="12" /></span>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <label style="width:100px;padding-left:2px;">ホール名</label><span style="padding-left:55px;"><asp:Label ID="lblHallNm" runat="server" TabIndex="13" /></span>
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtSerialNo" runat="server" ppRequiredField="true" ppName="シリアルＮｏ．" ppNameWidth="100px"  ppTextAlign="左" ppWidth="120px" ppTabIndex="14" ppMaxLength="16" ppIMEMode="半角_変更不可" ppValidationGroup="Detail2" />
            </td>
            <td colspan="3">
                <uc:ClsCMDropDownList ID="ddlSagyoKekka" runat="server" ppName="作業結果" ppNameWidth="75px" ppWidth="90px" ppTabIndex="15" ppClassCD="0072" ppMode ="名称" ppNotSelect="true" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBox ID="txtJuryo" runat="server" ppName="受領" ppNameWidth="100px" ppTextAlign="左" ppTabIndex="16" ppDateFormat="年月日" ppValidationGroup="Detail2" />
            </td>
            <td>
                <uc:ClsCMDateBox ID="txtSagyoJissi" runat="server" ppName="作業実施" ppTextAlign="左" ppNameWidth="75px" ppTabIndex="17" ppDateFormat="年月日" ppValidationGroup="Detail2" />
            </td>
            <td>
                <uc:ClsCMDateBox ID="txtBaitaiSofu" runat="server" ppName="媒体送付" ppTextAlign="左" ppNameWidth="75px" ppTabIndex="18" ppDateFormat="年月日" ppValidationGroup="Detail2" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBox ID="txtHenkyakuHasso" runat="server" ppName="返却(発送)" ppNameWidth="100px" ppTextAlign="左" ppTabIndex="19" ppDateFormat="年月日" ppValidationGroup="Detail2" />
            </td>
            <td>
                <uc:ClsCMDateBox ID="txtHenkyakuNonyu" runat="server" ppName="返却(納入)" ppTextAlign="左" ppNameWidth="75px" ppTabIndex="20" ppDateFormat="年月日" ppValidationGroup="Detail2" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtKensyuMonth" runat="server" ppName="検収月" ppTextAlign="左" ppWidth="40px" ppNameWidth="75px" ppTabIndex="21" ppMaxLength="4" ppNum="true" ppIMEMode="半角_変更不可" ppExpression ="[0-9][0-9]([0][1-9]|[1][0-2])" ppValidationGroup="Detail2" />
            </td>
            <td>
                <asp:CheckBox ID="chkRequest" runat="server" text="請求対象"/>
            </td>
        </tr>
    </table>
    <table style="width: 100%;" class="align-top" border="0">
        <tr>
            <td class="float-left">
                <asp:ValidationSummary ID="vasSummaryUpdate" runat="server" CssClass="errortext" ValidationGroup="Detail2" TabIndex="22" />
            </td>
            <td class="float-right">
                <table border="0" class="float-right">
                    <tr>
                        <td>
                            <asp:Button ID="btnDetailInsert" runat="server" Text="登録" ValidationGroup="Detail2" TabIndex="23" />
                        </td>
                        <td>
                           <asp:Button ID="btnDetailUpdate" runat="server" Text="更新" ValidationGroup="Detail2" TabIndex="24" />
                        </td>
                        <td>
                           <asp:Button ID="btnDetailDelete" runat="server" Text="削除" ValidationGroup="Detail2" TabIndex="25" />
                        </td>
                        <td>
                           <asp:Button ID="btnDetailClear" runat="server" Text="クリア" ValidationGroup="Detail2" TabIndex="26" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
<%--    <div class="float-left">
        <asp:ValidationSummary ID="vasSummaryUpdate" runat="server" CssClass="errortext" ValidationGroup="Detail2" TabIndex="22" />
    </div>
    <div class="float-right">
        <asp:Button ID="btnDetailInsert" runat="server" Text="登録" ValidationGroup="Detail2" TabIndex="23" />
        <asp:Button ID="btnDetailUpdate" runat="server" Text="更新" ValidationGroup="Detail2" TabIndex="24" />
        <asp:Button ID="btnDetailDelete" runat="server" Text="削除" ValidationGroup="Detail2" TabIndex="25" />
    </div>--%>
 </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server" >
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
