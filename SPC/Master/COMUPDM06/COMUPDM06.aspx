<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM06.aspx.vb" Inherits="SPC.COMUPDM06" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function lenCheck(obj, size) {
            var strW = obj.value;
            var lenW = strW.length;
            var num

            num = obj.value.match(/\n|\r\n/g);
            if (num != null) {
                gyosuu = num.length;
            } else {
                gyosuu = 0;
            }

            if ((parseInt(size) + parseInt(gyosuu)) < lenW) {
                var limitS = strW.substring(0, (parseInt(size) + parseInt(gyosuu)));
                obj.value = limitS;
            }
        }
        function focusChange(btnDmy,txtBox) {            
            btnDmy.style.visibility = "hidden";
            txtBox.focus();
        }
    </script>
    </asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:500px;height:50px;margin-left:auto;margin-right:auto;border:none;text-align:left;" >
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlSrchAPPACLASS_CD" ppName="機器分類" ppWidth="130px" ppClassCD="0000" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSrchAPPACLS_NM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="20" ppName="機器種別名" ppNameWidth="80" ppWidth="200" ppCheckHan="False" ppValidationGroup="Serch" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:1000px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlAPPACLASS_CD" ppName="機器分類" ppWidth="130px" ppClassCD="0000"  />
            </td>
            <Td>
                <uc:ClsCMTextBox ID="txtAPPA_CLS" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="機器種別コード" ppNameWidth="100px"  ppWidth="30" ppNameVisible="true" ppValidationGroup="Edit" ppCheckHan="True" ppCheckLength="True" ppRequiredField="True" />
            </td>
            <Td>
                <uc:ClsCMTextBox ID="txtAPPACLS_NM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="20" ppName="機器種別名" ppNameWidth="70" ppWidth="300" ppNameVisible="true" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
            <Td>
                <uc:ClsCMTextBox ID="txtAPPACLS_SNM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="5" ppName="機器種別略称" ppNameWidth="90" ppWidth="80" ppNameVisible="true" ppValidationGroup="Edit" ppRequiredField="True" />
                <asp:HiddenField ID="htxtDELETE" runat="server" />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlSEARIAL_CLS" ppName="管理機器区分" ppWidth="120px" ppClassCD="0000" ppRequiredField="True"  />
            </td>
        </tr>
        <tr>
            <td>
                <div class="float-left">
                    <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="Edit" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">  
    <div id="DivOut" runat="server" class="grid-out" style="width:920px;height:505px;">
        <div id="DivIn" runat="server" class="grid-in" style="width:920px;height:505px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

