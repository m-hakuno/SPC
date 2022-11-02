<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDMA0.aspx.vb" Inherits="SPC.COMUPDMA0" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:500px;margin-left:auto;margin-right:auto;border:none;">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtSTboxCls" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ＴＢＯＸ種別" ppNameWidth="100" ppWidth="20" ppCheckHan="True" ppValidationGroup="search" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtBbCls" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="ＢＢ種別コード" ppNameWidth="106" ppWidth="20" ppCheckHan="True" ppValidationGroup="search" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtLendCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="貸玉数設定情報" ppNameWidth="108" ppWidth="30" ppCheckHan="True" ppValidationGroup="search" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBoxFromTo ID="tftSPrice" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="単価" ppNameWidth="100" ppWidth="50" ppCheckHan="True" ppValidationGroup="search" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:500px;margin-left:auto;margin-right:auto;border:none;">
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtTboxCls" ppName="ＴＢＯＸ種別" ppNameWidth="100"
                    ppIMEMode="半角_変更不可" ppMaxLength="1" ppCheckHan="True"
                    ppWidth="20" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMTextBox runat="server" ID="tftBbCls" ppName="ＢＢ種別コード" ppNameWidth="106"
                    ppIMEMode="半角_変更不可" ppMaxLength="1" ppCheckHan="True"
                    ppWidth="20" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMTextBox runat="server" ID="tftLendCd" ppName="貸玉数設定情報" ppNameWidth="108"
                    ppIMEMode="半角_変更不可" ppMaxLength="2" ppCheckHan="True"
                    ppWidth="30" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox runat="server" ID="tftPrice" ppName="単価" ppNameWidth="100"
                    ppIMEMode="半角_変更不可" ppMaxLength="5" ppCheckHan="True"
                    ppWidth="50" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>
 
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:440px;">
        <div class="grid-in" style="width:440px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>