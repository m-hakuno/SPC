<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CMPOUTP001.aspx.vb" Inherits="SPC.CMPOUTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <td style="width: 25%">&nbsp;</td>
            <td>
                <uc:ClsCMDateBox ID="dtbSupport" runat="server" ppDateFormat="年月" ppName="対応年月" ppNameWidth="120"
                    ppRequiredField="False" />
            </td>
        </tr>
        <tr>
            <td style="width: 25%">&nbsp;</td>
            <td style="width: 270px;">
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Width="113">対応開始日時</asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMDateTimeBox runat="server" ID="dttStDtFrom" ppName="対応開始日時From" ppNameVisible="false" ppTimeBlank="true" />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Width="15">～</asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMDateTimeBox runat="server" ID="dttStDtTo" ppName="対応開始日時To" ppNameVisible="false"  ppTimeBlank="true"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 25%">&nbsp;</td>
            <td>
                <!--提出日-->
                <uc:ClsCMDateBoxFromTo ID="dtbSubmitDt" runat="server" ppName="提出日" ppNameWidth="120" />
            </td>

        </tr>
    </table>
    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="errortext" Width="450" />
    <br>
    <div class="float-right">
        <asp:Button ID="btnClear" runat="server" Text="印刷条件クリア" CausesValidation="False" />
        <asp:Button ID="btnCsv" runat="server" Text="特別保守費用一覧ＣＳＶ" />
        <asp:Button ID="btnTCsv" runat="server" Text="特別保守作業集計ＣＳＶ" />
        <asp:Button ID="btnPrint" runat="server" Text="印刷" />
    </div>
</asp:Content>
