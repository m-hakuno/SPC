<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM80.aspx.vb" Inherits="SPC.COMUPDM80" %>
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

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

    </script>
    <style type="text/css">
        .auto-style3
        {
            width: 195px;
        }
        .auto-style5
        {
            width: 210px;
        }
        </style>
</asp:Content>

<asp:Content ID="serchcontent" ContentPlaceHolderID="SearchContent" runat="server">
     <table style="width:700px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
         <tr>
             <td class="auto-style5">
                 <table>
                     <tr>
                         <td>
                             <asp:Label runat="server" Text="システム" Width="70"></asp:Label>
                         </td>
                         <td>
                             <asp:dropdownlist id="ddlSSystem" runat="server" width="110"></asp:dropdownlist>
                         </td>
                     </tr>
                 </table>                         
             </td>
             <td class="auto-style3">
                 <uc:ClsCMTextBoxFromTo id="txtScode" runat="server" ppIMEMode="半角_変更不可" ppname="コード" ppNameWidth="60" ppwidth="35" ppMaxLength="4" ppCheckHan="true" ppNum="true"  ppValidationGroup="search" ppTextAlign="左" />
             </td>      
             <td>
                 <uc:ClsCMDropDownList ID="ddlDel" runat="server" ppName="削除区分" ppNameWidth="60" ppWidth="100" ppClassCD="0124" ppNotSelect="true" />
             </td>    
         </tr>
         <tr>
             <td  colspan="3">
                 <uc:ClsCMTextBox id="txtscontent" runat="server" ppIMEMode="全角" ppname="回復内容" ppnamewidth="70" ppwidth="500" ppMaxLength="35" ppTextAlign="左" />
             </td>
         </tr>
     </table>    
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:700px;margin-right:auto;margin-left:auto;border:none;text-align:left;">
        <tr>
            <td class="auto-style5">
                <table>
                     <tr>
                         <td>
                             <asp:Label ID="Label1" runat="server" Text="システム" Width="70"></asp:Label>
                         </td>
                         <td>
                             <asp:dropdownlist id="ddlMSystem" runat="server" width="110" ></asp:dropdownlist>
                         </td>
                     </tr>
                 </table>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtMcode" runat="server" ppName="コード" ppNameWidth="60" ppIMEMode="半角_変更不可" ppwidth="35" ppMaxLength="4" ppValidationGroup="key" ppCheckHan="true" ppNum="true"/>
        </tr>
        <tr>
            <td colspan ="2">
                <uc:clscmtextbox ID="txtMcontent" runat="server" ppIMEMode="全角" ppName="回復内容" ppNameWidth="70" ppwidth="500" ppMaxLength="35" ppWrap="true" ppTextAlign="左" ppRequiredField="true" ppValidationGroup="val"/>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" contentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width:689px;height:450px;">
        <div class="grid-in" style="width:689px;height:450px;">
            <Input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:gridview ID="grvList" runat="server" ></asp:gridview>
        </div>
    </div>
</asp:Content>
