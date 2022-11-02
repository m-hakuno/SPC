<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="SCLLSTP001.aspx.vb" Inherits="SPC.SCLLSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">

    <table class="center" style="width: 1000px;" border="0">
        <tr>
            <td style="width: 250px;"></td>
            <td style="width: 250px;"></td>
            <td style="width: 15px;"></td>
            <td style="width: 250px;"></td>
            <td style="width: 235px;"></td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlVstCls" ppClassCD="0000" ppValidationGroup="1" ppName="訪問種別" ppNameWidth="70" ppWidth="130" ppNotSelect="True" />
            </td>
            <td style="padding-top:5px;">
                <uc:ClsCMDateTimeBox runat="server" ID="txtWrkDt_From" ppNameVisible="True" ppName="到着日時" ppRequiredField="False" ppNameWidth="70px" />
            </td>
            <td>～</td>
            <td style="padding-top:5px;">
                <uc:ClsCMDateTimeBox runat="server" ID="txtWrkDt_To" ppNameVisible="False" ppName="到着日時To" ppRequiredField="False" ppNameWidth="0px" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtCtrlNo" runat="server" ppName="依頼番号" ppNameWidth="70" ppWidth="110" ppIMEMode="半角_変更不可" ppMaxLength="14" ppCheckHan="True" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtVstCd" runat="server" ppName="TBOXID" ppNameWidth="70" ppWidth="70"
                    ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppNum="True" />
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtVstNm" runat="server" ppName="訪問先名" ppNameWidth="70" ppWidth="630"
                    ppIMEMode="全角" ppMaxLength="50" ppCheckHan="False" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out" style="width:1161px; margin-left:auto;margin-right:auto;">
        <div class="grid-in"  style="height:484px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
