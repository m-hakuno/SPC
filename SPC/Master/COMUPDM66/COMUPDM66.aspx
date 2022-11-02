<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM66.aspx.vb" Inherits="SPC.COMUPDM66" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
    <style type="text/css">
        .auto-style6
        {
            width: 340px;
        }
    </style>
   </asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:700px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
           
            <td class="auto-style6">
                <uc:ClsCMTextBox ID="txtSModelCd" runat="server" ppValidationGroup="search" ppIMEMode="半角_変更不可" ppCheckHan="true" ppMaxLength="20" ppName="型式番号" ppNameWidth="100" ppWidth="146"  ppExpression="^[a-zA-Z0-9-]+$"/>
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlDel" runat="server" ppName="削除区分" ppNameWidth="100" ppWidth="100" ppNotSelect="true" ppClassCD="0124" />
            </td>
        </tr>
       <tr>
            <td class="auto-style6">
                <uc:ClsCMDropDownList ID="ddlSSystem" runat="server" ppName="システム分類" ppNameWidth="100" ppWidth="150" ppClassCD="0006" ppNotSelect="true" />
            </td>
        
             <td>
                <uc:ClsCMDropDownList ID="ddlSSum"  runat="server" ppName="集計区分" ppNameWidth="100" ppWidth="100" ppClassCD="0127" ppNotSelect="true"/>
            </td>
       </tr>
   </table>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:700px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtModelCd" runat="server" ppIMEMode="半角_変更不可" ppCheckHan="true" ppRequiredField="False" ppMaxLength="20" ppName="型式番号" ppNameWidth="100" ppWidth="146" ppValidationGroup="key"  ppExpression="^[a-zA-Z0-9-]+$"/>
            </td>
        </tr>
        <tr>
            <td class="auto-style6">
                <uc:ClsCMDropDownList ID="ddlSystem" runat="server" ppName="システム分類" ppNameWidth="100" ppWidth="150" ppClassCD="0006" ppValidationGroup="val" ppNotSelect="true" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlSum" runat="server" ppName="集計区分" ppNameWidth="100" ppWidth="100" ppClassCD="0127" ppValidationGroup="val" ppNotSelect="true" ppRequiredField="true"/>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">  
    <div class="grid-out" style="width:520px;height:362px">
        <div class="grid-in" style="width:520px;height:362px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>