<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM45.aspx.vb" Inherits="SPC.COMUPDM45" %>
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
                <div class="Cell1" >画面／帳票名</div>
                <div class="Cell2" >
                    <uc:ClsCMDropDownList runat="server" ID="ddlSrchScreen_ID" ppWidth="250px" ppClassCD="0000" ppNameVisible ="false" />
                </div>
            </td>
        </tr>
    </table>

        



</asp:Content>

<%--MainContent--%>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <style type="text/css">
        .RowDiv {
            width:1050px;
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

        }
        .RightCell {
            display: inline-block;
            width:50px;
            text-align:left;    
            vertical-align:top ;        
        }
        </style>


    <table style="width:1050px;margin-right:auto;margin-left:auto;text-align:left;">
        <tr>
            <td style ="width:100px; ">
                画面名／帳票名 
            </td>
            <td style="width:100px; ">
                <uc:ClsCMTextBox ID="txtScreen_ID" runat="server" ppIMEMode="半角_変更可" ppMaxLength="13" ppName="" ppWidth="80" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtScreen_NM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="20" ppName="" ppWidth="250" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
        </tr>
    </table>
    <DIV class="RowDiv" >
    <table style="width:1050px;margin-left:auto;margin-right:auto">
        <tr>
            <td style="width:50px;">大分類</td>
            <td>
                <uc:ClsCMTextBox ID="txtL_CLS" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="" ppWidth="40" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
            <td style="width:50px;">中分類</td>
            <td>
                <uc:ClsCMTextBox ID="txtM_CLS" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="" ppWidth="50" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
            <td style="width:50px;">小分類</td>
            <td>
                <uc:ClsCMTextBox ID="txtS_CLS" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="" ppWidth="50" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
            <td style="width:50px;">コード</td>
            <td>
                <uc:ClsCMTextBox ID="txtCODE" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="" ppWidth="50" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
            <td style="width:50px;">連番</td>
            <td>
                <uc:ClsCMTextBox ID="txtDISP_ORDER" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="" ppWidth="50" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
            </td>
        </tr>
    </table>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >表示内容</div>
        <div class="RightCell">
            <uc:ClsCMTextBox ID="txtITEM_NM" runat="server" ppIMEMode="半角_変更可" ppMaxLength="30" ppName="" ppWidth="450" ppNameVisible="False" ppValidationGroup="Edit" ppRequiredField="True" />
        </div>
    </DIV>
    <%--MainContent--%>

</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:950px;">
        <div class="grid-in" style="width:950px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

