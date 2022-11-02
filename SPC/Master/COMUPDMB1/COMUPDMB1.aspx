<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDMB1.aspx.vb" Inherits="SPC.COMUPDMB1" %>

<%@ MasterType VirtualPath="~/Master/Mst.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content ID="searchcontent" ContentPlaceHolderID="SearchContent" runat="server">
    <table style="width: 700px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td style="width: 30px;">
                <uc:ClsCMTextBox ID="txtSrchTBOXID" runat="server" ppName="ＴＢＯＸＩＤ" ppNameVisible="True" ppRequiredField ="False" 
                    ppNameWidth="80" ppWidth="70" ppMaxLength="8" ppValidationGroup="Search" ppCheckHan="True" ppNum="False" />
            </td>
            <td style="width:40px;">
                <uc:ClsCMDropDownList ID="ddlSrchNLCls" runat="server" ppName="ＮＬ区分" 
                    ppNameWidth="60" ppWidth="80" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Search" ppRequiredField="true" />
            </td>
            <td style="width: 90px;">
                <uc:ClsCMDropDownList ID="ddlSrchTBOX" runat="server" ppName="ＴＢＯＸ種別"
                    ppNameWidth="80" ppWidth="120" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="Search" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 900px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td style="width: 30px;">
                <uc:ClsCMTextBox ID="txtTBOXID" runat="server" ppName="ＴＢＯＸＩＤ" ppNameVisible="True" ppRequiredField ="true" 
                    ppNameWidth="80" ppWidth="70" ppMaxLength="8" ppValidationGroup="val" ppCheckHan="True" ppNum="False" />
            </td>
            <td style="width:40px;">
                <uc:ClsCMDropDownList ID="ddlNLCls" runat="server" ppName="ＮＬ区分" 
                    ppNameWidth="60" ppWidth="80" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td style="width: 90px;">
                <uc:ClsCMDropDownList ID="ddlTBOX" runat="server" ppName="ＴＢＯＸ種別"
                    ppNameWidth="80" ppWidth="120" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td style="width: 30px;">
                <uc:ClsCMTextBox ID="txtHallNM" runat="server" ppName="ホール名" ppNameVisible="True"
                    ppNameWidth="60" ppWidth="300" ppMaxLength="50" ppValidationGroup="val" ppCheckHan="False" ppNum="False" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnSyscls" runat="server" Value="" />
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 870px; height: 458px;">
        <div class="grid-in" style="width: 870px; height: 458px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
