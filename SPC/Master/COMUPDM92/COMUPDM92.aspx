<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM92.aspx.vb" Inherits="SPC.COMUPDM92" %>
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
            width:250px;
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
                <uc:ClsCMDropDownList runat="server" ID="ddlSrchCNST_CLS" ppNameVisible="true" ppName="工事区分" ppWidth="250px" ppClassCD="0000" />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlSrchTBOXCLS_CD" ppNameVisible="true" ppName="ＴＢＯＸタイプ" ppWidth="250px" ppClassCD="0000" />
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
                <td style="width:100px">
                    <uc:ClsCMTextBox ID="txtCnst_Cls" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppNameVisible="true" ppName="工事区分" ppWidth="30" ppCheckHan="true" ppValidationGroup="Edit" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtCnstCls_NM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="20" ppNameVisible="false" ppName="" ppWidth="250" ppCheckHan="False" ppValidationGroup="Edit" />
                </td>
                <td>
                    <uc:ClsCMDropDownList runat="server" ID="ddlTBOXCLS_CD" ppNameVisible="true" ppName="ＴＢＯＸタイプ" ppWidth="250px" ppClassCD="0000" ppRequiredField="True" ppValidationGroup ="Edit" />
                </td>
            </tr>
        </table>
        <br />
        <table style="width:1050px;margin-left:auto;">
            <tr>
                <td>連番</td>
                <td>工事名</td>
                <td>緊急依頼時付加</td>
                <td>通常料金</td>
                <td>休日料金</td>
                <td>深夜料金</td>
                <td></td>
            </tr>
            <tr>
                <td style="width:30px;">
                    <uc:ClsCMTextBox ID="txtSEQNO" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppNameVisible="false" ppName="" ppWidth="30" ppCheckHan="true" ppValidationGroup="Edit" />
                </td>
                <td style="width:250px;">
                    <uc:ClsCMDropDownList runat="server" ID="ddlWork_Cls" ppNameVisible="false" ppName="" ppWidth="250px" ppClassCD="0000" ppRequiredField="True" ppValidationGroup ="Edit" />
                </td>
                <td style="width:110px;text-align:center;">
                    <asp:CheckBox ID="ChkEMGNCY_FLG" runat="server" />
                </td>
                <td style="width:100px;text-align:right;">
                    <asp:Label ID="lblNml_Amount" runat="server" Text="" style="text-align:right;"></asp:Label>
                </td>
                <td style="width:100px;text-align:right;">
                    <asp:Label ID="lblHld_Amount" runat="server" Text="" style="text-align:right;"></asp:Label>
                </td>
                <td style="width:100px;text-align:right;">
                    <asp:Label ID="lblMdn_Amount" runat="server" Text="" style="text-align:right;"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblAmount_CLS" runat="server" Text="" style="text-align:right;"></asp:Label>
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

