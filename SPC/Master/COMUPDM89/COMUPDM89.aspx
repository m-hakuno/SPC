<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM89.aspx.vb" Inherits="SPC.COMUPDM89" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

    </script>
</asp:Content>

<asp:Content ID="serchcontent" ContentPlaceHolderID="SearchContent" runat="server">
     <table style="width:600px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
         <tr>
             <td>
                 <table>
                     <tr>
                         <td>
                             <asp:Label ID="Label1" runat="server" Text="システム" Width="70"></asp:Label>
                         </td>
                         <td>
                             <asp:dropdownlist id="ddlSSystem" runat="server" width="110"></asp:dropdownlist>
                         </td>
                     </tr>
                 </table>                         
             </td>
             <td>
                 <uc:ClsCMTextBoxFromTo id="txtScode" runat="server" ppIMEMode="半角_変更不可" ppname="コード" ppNameWidth="70" ppwidth="25" ppMaxLength="4" ppCheckHan="true" ppNum="true"  ppValidationGroup="search" ppTextAlign="左" />
             </td>      
             <td>
                 <uc:ClsCMDropDownList ID="ddlDel" runat="server" ppName="削除区分" ppNameWidth="70" ppWidth="100" ppClassCD="0124" ppNotSelect="true" />
             </td>    
         </tr>
         <tr>
             <td  colspan="3">
                 <uc:ClsCMTextBox id="txtscontent" runat="server" ppIMEMode="全角" ppname="作業内容" ppnamewidth="70" ppwidth="292" ppMaxLength="20" ppTextAlign="左" />
             </td>
         </tr>
     </table>    
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:500px;margin-right:auto;margin-left:auto;border:none;text-align:left;">
        <tr class="float-left">
            <td>
                <table>
                     <tr>
                         <td>
                             <asp:Label ID="Label2" runat="server" Text="システム" Width="70"></asp:Label>
                         </td>
                         <td>
                             <asp:dropdownlist id="ddlSystem" runat="server" width="110" ></asp:dropdownlist>
                         </td>
                     </tr>
                 </table>
            </td>
            <td style="padding-left:60px;">
                <uc:ClsCMTextBox ID="txtCode" runat="server" ppName="コード" ppNameWidth="70" ppIMEMode="半角_変更不可" ppwidth="25" ppMaxLength="2" ppValidationGroup="key" ppCheckHan="true" ppNum="true"/>
        </tr>
        <tr class="float-left">
            <td colspan ="3">
                <uc:clscmtextbox ID="txtContent" runat="server" ppIMEMode="全角" ppName="作業内容" ppNameWidth="70" ppwidth="280" ppMaxLength="20" ppWrap="true" ppTextAlign="左" ppRequiredField="true" ppValidationGroup="val"/>
            </td>
        </tr>
        <tr class="float-left">
            <td>
                <uc:ClsCMTextBox ID="txtWorkTime" runat="server" ppName="作業時間" ppNameWidth="70" ppIMEMode="半角_変更不可" ppwidth="25" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppNum="true" ppRequiredField="true" />
            </td>
            <td style="padding-left:15px;">
                <uc:ClsCMTextBox ID="txtGoRtnTime" runat="server" ppName="往復時間" ppNameWidth="70" ppIMEMode="半角_変更不可" ppwidth="25" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppNum="true" ppRequiredField="true" />
            </td>
            <td style="padding-left:15px;">
                <uc:ClsCMTextBox ID="txtPeopleCnt" runat="server" ppName="人数" ppNameWidth="70" ppIMEMode="半角_変更不可" ppwidth="25" ppMaxLength="2" ppValidationGroup="val" ppCheckHan="true" ppNum="true" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" contentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width:671px;height:400px;">
        <div class="grid-in" style="width:671px;height:400px;">
            <Input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:gridview ID="grvList" runat="server" ></asp:gridview>
        </div>
    </div>
</asp:Content>
