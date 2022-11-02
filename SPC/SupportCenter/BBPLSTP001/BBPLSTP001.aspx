<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="BBPLSTP001.aspx.vb" Inherits="SPC.BBPLSTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:800px;" border="0">
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBoxFromTo ID="txtSrcTboxIdFrTo" runat="server" ppName="ＴＢＯＸＩＤ" ppWidth="60px" ppNameWidth="100px" ppTabIndex="1" ppMaxLength="8" ppNum="true" ppValidationGroup="Detail" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxTwoFromTo ID="txtSrcBBChohoNo_2FrTo" runat="server" ppName="ＢＢ調報Ｎｏ．" ppWidthOne="35px" ppWidthTwo="35px" ppNameWidth="100px" ppMaxLengthOne ="3" ppMaxLengthTwo ="4" ppValidationGroup="Detail" />
            </td>
            <td colspan="2">
                <uc:ClsCMTextBoxFromTo ID="txtSrcSyuriIraiNoFrTo" runat="server" ppName="修理依頼Ｎｏ．" ppNameWidth="100px" ppWidth="60px" ppTabIndex="6" ppMaxLength="8" ppValidationGroup="Detail" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlSrcNlKbn" runat="server" ppClassCD="0009" ppMode="名称" ppNotSelect="true" ppName="ＮＬ区分" ppNameWidth="100px" ppWidth="90px" ppTabIndex="7" />
            </td>
            <td>
                    <uc:ClsCMDropDownList runat="server" ID="ddlSystem" ppClassCD="0109" ppValidationGroup="1" ppName="ＴＢＯＸ種別" ppNameWidth="100" ppWidth="110" ppNotSelect="True" ppTabIndex="8" />
            </td>
            <td>
                <span style="width:70px;padding-right:30px;"><label>進捗状況</label></span><asp:DropDownList ID="ddlSrcShinchokuJokyo" runat="server" Width="100px" TabIndex="9"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="txtSrcJuryobiFrTo" runat="server" ppName="受領日" ppNameWidth="100px" ppTabIndex="10" ppValidationGroup="Detail" />
            </td>
            <td colspan="2">
                <uc:ClsCMTextBoxFromTo ID="txtSrcKensyuTukiFrTo" runat="server" ppName="検収月" ppNameWidth="100px" ppWidth="35px" ppTabIndex="11" ppMaxLength="4" ppNum="true" ppValidationGroup="Detail" ppExpression="[0-9][0-9]([0][1-9]|[1][0-2])"/>
            </td>
        </tr>
    </table>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummarySearch" runat="server" CssClass="errortext" ValidationGroup="Detail" TabIndex="12" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
 <asp:Panel ID="pnlRegister" runat="server"  BorderStyle="Solid" BorderWidth="1px">
    <table style="width:1000px;" border="0">
        <tr>
            <td colspan="2">
                <%--<label style="width:100px;padding-left:2px;" >ＢＢ調報Ｎｏ．</label><span style="padding-left:14px;"><asp:Label ID="lblBBChohoNoFr" runat="server" Width="40px"/></span><label style="padding-left:20px;">－</label><span style="padding-left:20px;width:40px;"><asp:Label ID="lblBBChohoNoTo" runat="server" Width="40px"/></span>--%>
                <uc:ClsCMTextBox ID="txtSyuriIraiNo" runat="server" ppName="修理依頼Ｎｏ．" ppWidth="60px" ppNameWidth="100px" ppTabIndex="13" ppMaxLength="8" ppRequiredField="true" ppValidationGroup="Detail2" ppCheckHan="True" ppCheckLength="True" />
            </td>
            <td colspan="4">
                
            </td>
        </tr>
        <tr>
            <td style="width: 245px">
                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppName="ＴＢＯＸＩＤ" ppWidth="60px" ppNameWidth="100px" ppRequiredField="true" ppTabIndex="14" ppMaxLength="8" ppNum="true" ppValidationGroup="Detail2" />
            </td>
            <td style="width: 200px">
                <label style="width:100px;padding-left:2px;" >ＮＬ区分</label><span style="padding-left:20px;"><asp:Label ID="lblNlKbn" runat="server" /></span>
            </td>
            <td>
                <label style="width:100px;padding-left:2px;" >ＥＷ区分</label><span style="padding-left:20px;"><asp:Label ID="lblEwKbn" runat="server" /></span> 
            </td>
            <td colspan="3">
                <label style="width:100px;padding-left:2px;" >ＴＢＯＸ種別</label><span style="padding-left:20px;"><asp:Label ID="lblTboxSbt" runat="server" />
                <asp:HiddenField ID="hdnTboxSbtCd" runat="server" />
                </span> 
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label style="width:100px;padding-left:2px;" >ホール名</label><span style="padding-left:58px;"><asp:Label ID="lblHallNm" runat="server" Width="320px"/></span>
            </td>
            <td colspan="4">
                <span style="width:70px;padding-left:2px;padding-right:16px;"><label style="width:100px;">型式番号</label></span><asp:DropDownList ID="ddlKosyoKatasikiNo" runat="server" Width="130px" TabIndex="15" ></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 245px">
                <uc:ClsCMTextBox ID="txtSerialNo" runat="server" ppRequiredField="true" ppName="シリアルＮｏ．" ppWidth="115px" ppNameWidth="100px" ppTabIndex="16" ppMaxLength="16" ppValidationGroup="Detail2" />
            </td>
            <td colspan="5">
                <span style="width:70px;padding-left:2px;padding-right:20px;"><label style="width:60px;">進捗状況</label></span><asp:DropDownList ID="ddlShinchokuJokyo" runat="server" Width="100px" TabIndex="17" ></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 245px">
                <uc:ClsCMDateBox ID="txtJuryobi" runat="server" ppName="受領日" ppNameWidth="100px" ppTabIndex="18" ppValidationGroup="Detail2" />
            </td>
            <td style="width: 200px">
                <uc:ClsCMDateBox ID="txtSagyobi" runat="server" ppName="作業日" ppNameWidth="67px" ppTabIndex="19" ppValidationGroup="Detail2"  />
            </td>
            <td colspan="4">
                <uc:ClsCMDateBox ID="txtHokokubi" runat="server" ppName="報告日" ppNameWidth="65px" ppTabIndex="20" ppValidationGroup="Detail2"  />
            </td>
        </tr>
        <tr>
            <td style="width: 245px">
                <uc:ClsCMDateBox ID="txtBB1Sofubi" runat="server" ppName="ＢＢ１送付日" ppNameWidth="100px" ppTabIndex="21" ppValidationGroup="Detail2"  />
            </td>
            <td colspan="5">
                <uc:ClsCMTextBox ID="txtKensyuTuki" runat="server" ppName="検収月" ppWidth="35px" ppNameWidth="67px" ppTabIndex="22" ppMaxLength="4" ppNum="true" ppValidationGroup="Detail2" ppExpression="[0-9][0-9]([0][1-9]|[1][0-2])" />
            </td>
        </tr>
    </table>
<%--    <div class="float-left">
        <asp:ValidationSummary ID="vasSummaryUpdate" runat="server" CssClass="errortext" ValidationGroup="Detail2" TabIndex="23" />
    </div>
    <div class="float-right">
        <asp:Button ID="btnDetailClear" runat="server" Text="クリア" ValidationGroup="Detail2" TabIndex="24" />
        <asp:Button ID="btnDetailUpdate" runat="server" Text="登録" ValidationGroup="Detail2" tabindex="25" />
    </div>--%>


        <table style="width: 100%;" class="align-top" border="0">
            <tr>
                <td class="float-left">
                    <asp:ValidationSummary ID="vasSummaryUpdate" runat="server" CssClass="errortext" ValidationGroup="Detail2" TabIndex="23" />
                </td>
                <td class="float-right">
                    <table border="0" class="float-right">
                        <tr>
                            <td>
                                <asp:Button ID="btnDetailClear" runat="server" Text="クリア" ValidationGroup="Detail2" TabIndex="24" />
                            </td>
                            <td>
                               <asp:Button ID="btnDetailUpdate" runat="server" Text="登録" ValidationGroup="Detail2" tabindex="25" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server" TabIndex="26" >
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
