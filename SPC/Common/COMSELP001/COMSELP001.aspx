<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="COMSELP001.aspx.vb" Inherits="SPC.COMSELP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">

    <table style="width:100%;" border="0">
        <tr>
            <td style="width:200px;"></td>
            <td style="width:70px;"></td>
            <td style="width:100px;"></td>
            <td style="width:100px;"></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2">
                    <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppName="ＴＢＯＸＩＤ" ppNameWidth="100" ppWidth="65"
                        ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppNum="True" />

            </td>

            <td>
                    <uc:ClsCMDropDownList runat="server" ID="ddlSystem" ppClassCD="0109" ppValidationGroup="1" ppName="システム" ppNameWidth="60" ppWidth="110" ppNotSelect="True" />
            </td>

            <td>
                <uc:ClsCMTextBox ID="txtVersion" runat="server" ppName="ＶＥＲ" ppNameWidth="50" ppWidth="110"
                    ppIMEMode="半角_変更不可" ppMaxLength="10" ppCheckHan="True" />
            </td>

            <td>

                <uc:ClsCMDropDownList runat="server" ID="ddlPerCls" ppClassCD="0109" ppValidationGroup="1" ppName="運用状況" ppNameWidth="60" ppWidth="100" ppNotSelect="True" />

            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtHallNm" runat="server" ppName="ホール名" ppNameWidth="100" ppWidth="370"
                    ppMaxLength="40" ppIMEMode="全角" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtTelNo" runat="server" ppName="ＴＥＬ" ppNameWidth="50" ppWidth="110"
                    ppIMEMode="半角_変更不可" ppMaxLength="15" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td colspan="1">

                <uc:ClsCMDropDownList runat="server" ID="ddlState" ppClassCD="0000" ppValidationGroup="1" ppName="ホール住所" ppNameWidth="100" ppWidth="100" ppNotSelect="True" />

            </td>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtHallAd" runat="server" ppName="" ppNameWidth="0" ppWidth="500"
                    ppMaxLength="100" ppIMEMode="全角" />
            </td>

        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlTwinCls" ppClassCD="0129" ppValidationGroup="1" ppName="双子区分" ppNameWidth="100" ppWidth="100" ppNotSelect="True" />
            </td>
            <td></td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlMDN" ppClassCD="0109" ppValidationGroup="1" ppName="MDN機器" ppNameWidth="60" ppWidth="110" ppNotSelect="True" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out">
        <div class="grid-in"  style="height:350px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
