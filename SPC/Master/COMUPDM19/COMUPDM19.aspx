<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM19.aspx.vb" Inherits="SPC.COMUPDM19" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        //convertEnterToTab(event)を設定
        function pageLoad() {
            set_onloadenter();
        }

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "hidden";
            txtBox.focus();
        }
//        function KeepData() {
//            var 
        function exec_postback() {
            Sys.WebForms.PageRequestManager.getInstance()._doPostBack('UpdatePanelSample', '');
        }
        function textChange() {
            var updbtn = document.getElementById("btnUpdate");
//            if (updbtn.disabled == false) {
//                updbtn.disabled = true;
//            }
//            var insbtn = document.getElementById("btnInsert");
//            if (insbtn.disabled == false) {
//                insbtn.disabled = true;
//            }
        }
    </script>
    <style type="text/css">
        .auto-style2
        {
        }
        .auto-style4
        {
            width: 120px;
        }
        .auto-style6
        {
            width: 204px;
        }
        .auto-style7
        {
            width: 100px;
        }
        .auto-style8
        {
            width: 275px;
        }
        .auto-style9
        {
            width: 210px;
        }
        .auto-style10
        {
            width: 265px;
        }
        </style>
</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:600px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtSrch_TelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="電話番号" ppNameWidth="100" ppWidth="120" ppCheckHan="False" ppValidationGroup="search"  />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlSrch_JudgeCls" ppName="区分" ppNameWidth="100" ppWidth="100px" ppClassCD="0000" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="txtSrch_tboxidFromTo" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="TBOXID/社員ｺｰﾄﾞ" ppNameWidth="100" ppWidth="100" ppCheckHan="False" ppValidationGroup="search" />
            </td>
            <td style="text-align:left">
                <uc:ClsCMTextBox ID="txtSrch_Hall" runat="server" ppIMEMode="全角" ppMaxLength="30" ppName="ホール名/社員名" ppNameWidth="100" ppWidth="250" ppCheckHan="False" ppValidationGroup="search" />
            </td>
            <td>
                    <uc:ClsCMDropDownList ID="ddlSrchOperate" runat="server" ppName="運用状況" ppNameWidth="60" ppWidth="100" ppClassCD="0000" ppNotSelect="true" />
            </td>
        </tr>
    </table>
    <uc:ClsCMDropDownList ID="ddlDel" runat="server" ppName="削除区分" ppNameWidth="0" ppWidth="0" ppClassCD="0124" ppNotSelect="true" Visible="false" />
    </asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:890px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td rowspan="2" style="width:230px;">
                <uc:ClsCMTextBox ID="txtTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="電話番号" ppNameWidth="65px"  ppWidth="120" ppNameVisible="true" ppValidationGroup="key" ppCheckHan="True" ppCheckLength="false" ppRequiredField="True" />
                <asp:TextBox ID="txtOldTelNo" runat="server" Visible ="false" ></asp:TextBox>
            </td>
            <td rowspan="2" style="width:200px;">
                <uc:ClsCMDropDownList runat="server" ID="ddlJudge_Cls" ppName="区分" ppWidth="100px" ppClassCD="0000" ppValidationGroup="key" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtName" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="TBOXID" ppNameWidth="50px"  ppWidth="80" ppNameVisible="true" ppValidationGroup="key" ppCheckHan="True" ppCheckLength="True" ppRequiredField="false" />
                <asp:TextBox ID="txtOldHallCD" runat="server" Visible ="false" ></asp:TextBox>
            </td>
            <td style="text-align:left;">
                <div style="text-align:left;width:350px;">
                    <asp:Label ID="lblHall" runat="server" Text="Label" ></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMDropDownList runat="server" ID="ddlCompCD" ppName="会社" ppNameWidth="50px" ppWidth="200px" ppClassCD="0000" visible="false" />
                <uc:ClsCMDropDownList runat="server" ID="ddlEmployee" ppName="社員" ppNameWidth="50px"  ppWidth="180px" ppClassCD="0000" ppValidationGroup="key" />
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">  
    <div class="grid-out" style="width:900px;height:450px;">
        <div class="grid-in" style="width:900px;height:450px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>