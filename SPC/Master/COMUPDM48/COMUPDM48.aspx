<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM48.aspx.vb" Inherits="SPC.COMUPDM48" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">
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
            btnDmy.style.visibility = "hidden";
            txtBox.focus();
        }
    </script>
</asp:Content>

<%--SearchContent--%>
<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <style type="text/css">
        .Cell1 {
            width:120px;
            float:left;
            text-align:left;
            vertical-align:middle;
            padding-top:3px;
        }
        .Cell2 {
            width:120px;
            float:left;
            text-align:left;
        }
        .Cell3 {
            width:700px;
            float:left;
            text-align:left;
        }
        .SearchTable tr {
            height:15px;
            padding-bottom:5px;
            padding-top:5px;

        }
    </style>

   
    <table class="SearchTable" style="border-style: none; border-color: inherit; border-width: 0; width:1050px; margin-left:auto; margin-right:auto;">
        <tr>
            <td>
                <div class="Cell1" >工事コード
                </div>
                <div class="Cell2" >
                    <uc:ClsCMTextBox ID="txtSrchCODE" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppNameVisible ="false" ppWidth="40" ppCheckHan="true" ppValidationGroup="Serch" />
                </div>
            </td>
            <td>
                <div class="Cell1" >工事名
                </div>
                <div class="Cell3" >
                    <uc:ClsCMTextBox ID="txtSrchCNST_NM" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="30" ppNameVisible ="false" ppWidth="420" ppCheckHan="False" ppValidationGroup="Serch" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

<%--MainContent--%>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <style type="text/css">
        .RowDiv {
            width:1100px;
            margin-left:auto; 
            margin-right:auto;
            padding-top:5px;
            padding-bottom:5px;
            text-align:left;            
        }
        .RowDiv_Half {
            width:525px;
            margin-left:auto; 
            margin-right:auto;
            padding-top:5px;
            padding-bottom:5px;
            text-align:left;            
        }
        .LeftCell {
            display: inline-block;
            width:100px;
            padding-right:1px;
            text-align:left;
            vertical-align:top ;
            padding-top:3px;
            text-align:right;            
        }
        .RightCell {
            display: inline-block;
            width:400px;
            text-align:left;    
            vertical-align:top ;        
        }
        </style>

    <DIV class="RowDiv" >
        <table style="width:100%;">
            <tr>
                <td style="width:520px">
                    <table style="padding-left:0;padding-right:0;">
                        <tr>
                            <td>
                                <uc:ClsCMTextBox ID="txtCODE" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="工事" ppWidth="35" ppCheckHan="true" ppValidationGroup="Edit" />
                            </td>
                            <td>
                                <uc:ClsCMTextBox ID="txtCNST_NM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="30" ppNameVisible ="false" ppWidth="300" ppCheckHan="False" ppValidationGroup="Edit" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:150px">
                    <uc:ClsCMDateBox runat="server" ID="cdbSUMSTART_D" ppName ="開始日" />
    <%--                            <br />
                    <asp:CustomValidator ID="CstmVal_cdbSUMSTART_D" runat="server" ControlToValidate="cdbSUMSTART_D" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>--%>
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtNML_PRICE" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="7" ppName="通常料金" ppWidth="40" ppCheckHan="true" ppValidationGroup="Edit" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtHLDY_PRICE" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="7" ppName="休日料金" ppWidth="40" ppCheckHan="true" ppValidationGroup="Edit" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtNGHT_PRICE" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="7" ppName="夜間料金" ppWidth="40" ppCheckHan="true" ppValidationGroup="Edit" />
                </td>
                <td>
                    <uc:ClsCMDropDownList runat="server" ID="ddlL_CLS" ppName="料金分類" ppWidth="150px" ppClassCD="0000" ppRequiredField="True" ppValidationGroup ="Edit" />
                    <uc:ClsCMDropDownList runat="server" ID="ddlM_CLS" ppName="　　　　" ppNamewidth="52px" ppNameVisible="true" ppWidth="150px" ppClassCD="0000" ppRequiredField="True" ppValidationGroup ="Edit" />
                </td>
            </tr>
        </table>
    </DIV>
    <%--MainContent--%>

</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:1100px;height:500px;">
        <div class="grid-in" style="width:1100px;height:500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

