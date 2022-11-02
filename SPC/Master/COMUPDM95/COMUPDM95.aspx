<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM95.aspx.vb" Inherits="SPC.COMUPDM95" %>

<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:800px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td>
                <asp:Label ID="lblInsUpd" runat="server" Width="64" Text="モード" style="margin-left:3px;" />
                <asp:DropDownList ID="ddlInsUpd" runat="server" Width="70px">
                    <%--<asp:ListItem Value=""></asp:ListItem>--%>
                    <asp:ListItem Value="0">新規</asp:ListItem>
                    <asp:ListItem Value="1">更新</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <%-- <asp:Label ID="lblSys" runat="server" Width="67" Text="システム" />--%>
                <uc:ClsCMDropDownList ID="ddlSys" runat="server" ppNameWidth="67" ppWidth="120" ppValidationGroup="key" ppRequiredField="true" ppName="システム" ppClassCD="0001" />
            </td>
            <td>
                <%--<asp:Label ID="Label5" runat="server" Width="67" Text="機器分類" />--%>
                <uc:ClsCMDropDownList ID="ddlDvs" runat="server" ppNameWidth="67" ppWidth="120" ppValidationGroup="key" ppRequiredField="true" ppName="機器分類" ppClassCD="0001" />
            </td>
            <td colspan="2">
                <%--<asp:Label ID="Label1" runat="server" Width="67" Text="機器種別" />--%>
                <uc:ClsCMDropDownList ID="ddlCls" runat="server" ppNameWidth="67" ppWidth="220" ppValidationGroup="val" ppRequiredField="true" ppName="機器種別" ppClassCD="0001" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtSetNo" runat="server" ppNameWidth="67" ppWidth="40" ppCheckHan="true" ppNum="true" ppName="セットNo" ppIMEMode="半角_変更不可" ppMaxLength="2" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtMaxCnt" runat="server" ppNameWidth="67" ppWidth="40" ppCheckHan="true" ppNum="true" ppName="台数" ppIMEMode="半角_変更不可" ppMaxLength="1" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td>
                <%--<asp:Label ID="Label3" runat="server" Width="67" Text="HDD No" />--%>
                <uc:ClsCMDropDownList ID="ddlHDDNo" runat="server" ppNameWidth="67" ppWidth="45" ppValidationGroup="val" ppName="HDDNo" ppClassCD="0001" />
            </td>
            <td colspan="3">
                <%--<asp:Label ID="Label4" runat="server" Width="67" Text="HDD 種別" />--%>
                <uc:ClsCMDropDownList ID="ddlHDDCls" runat="server" ppNameWidth="67" ppWidth="45" ppValidationGroup="val" ppName="HDD種別" ppClassCD="0001" />
            </td>
        </tr>
    </table>
    <%-- グリッドからの選択待ちの時"1"にする --%>
    <asp:HiddenField ID="hdnDtl_SelectFLG" runat="server" Value="0" />
</asp:Content>


<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <asp:UpdatePanel runat="server" ID="updPanelGrv" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="grid-out" style="width: 822px; height: 490px;">
                <div class="grid-in" style="width: 822px; height: 490px;">
                    <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                    <asp:GridView ID="grvList" runat="server"></asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
