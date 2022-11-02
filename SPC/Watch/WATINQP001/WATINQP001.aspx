<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.master" CodeBehind="WATINQP001.aspx.vb" Inherits="SPC.WATINQP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContent" runat="server">

    <style type="text/css">
        #table_edit tr {
            vertical-align: top;
        }
    </style>

    <div class="text-center">
        <asp:Label ID="Label1" runat="server" Text="照会内容" Font-Bold="true"></asp:Label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table id="table_edit" border="0" class="center" style="width: 1150px; margin-bottom: 15px; border: 1px solid black; padding-top: 3px; padding-bottom: 3px;">
                <colgroup>
                    <col style="width: 250px;" />
                    <col style="width: 250px;" />
                    <col style="width: 300px;" />
                    <col style="width: 110px;" />
                    <col style="width: 240px;" />
                </colgroup>
                <tr>
                    <td>
                        <uc:ClsCMTextBox ID="txtTboxId1" runat="server" ppWidth="70" ppNameWidth="70" ppName="TBOXID" ppIMEMode="半角_変更不可"
                            ppCheckHan="true" ppNum="true"
                            ppRequiredField="true" ppMaxLength="8" ppValidationGroup="1" />
                    </td>
                    <td colspan="2" style="padding: 6px 0px 0px 0px;">
                        <asp:Label ID="Label2" runat="server" Text="ホール名" Width="80px"/>
                        <asp:Label ID="lblHallName" runat="server"/>
                    </td>
                    <td colspan="2" style="padding: 6px 0px 0px 0px;">
                        <asp:Label ID="Label3" runat="server" Text="NL区分" Width="100" />
                        <asp:Label ID="lblNlKubun" runat="server" />
                    </td>
                    <%--<td></td>--%>
                </tr>
                <tr>
                    <td rowspan="2">
                        <uc:ClsCMDropDownList runat="server" ID="ddlDataSbt" ppClassCD="0000" ppValidationGroup="1" ppName="データ種別" ppNameWidth="70" ppWidth="130" ppNotSelect="True" />
                    </td>
                    <td rowspan="2" style="padding: 0px 0px 0px 0px;">
                        <uc:ClsCMDateBox runat="server" ID="dtbTboxUnyoDate" ppName="TBOX運用日" ppNameVisible="True" ppDateFormat="年月日" ppValidationGroup="1" ppNameWidth="80" />
                    </td>
                    <td rowspan="2">
                        <uc:ClsCMTextBox runat="server" ID="txtBB1SerialNo" ppName="BB1シリアル番号" ppNameVisible="True" ppCheckHan="true" ppMaxLength="16" ppValidationGroup="1" ppNameWidth="105" ppWidth="130" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbtCID" runat="server" GroupName="1" Text="CID" AutoPostBack="True" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtCID" ppName="CID" ppNameVisible="False" ppCheckHan="true" ppMaxLength="16" ppValidationGroup="1" ppIMEMode="半角_変更不可" ppNameWidth="0" ppWidth="130" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbtNo" runat="server" GroupName="1" Text="入金伝票番号" AutoPostBack="True" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtNyukinDenpyoNo" ppName="入金伝票番号" ppNameVisible="False" ppCheckHan="true" ppMaxLength="28" ppValidationGroup="1" ppIMEMode="半角_変更不可" ppNameWidth="0" ppWidth="190" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td colspan="2" style="letter-spacing: 3px; padding: 10px 10px 0px 0px; text-align: right;">
                        <asp:Button runat="server" ID="btnClear" Text="クリア" />
                        <asp:Button runat="server" ID="btnAdd" Text="追 加" ValidationGroup="1" />
                        <asp:Button runat="server" ID="btnUpdate" Text="更 新" ValidationGroup="1" />
                        <asp:Button runat="server" ID="btnDelete" Text="削 除" />
                    </td>
                </tr>
            </table>

            <%--<div class="grid-out" style="width: 787px; height: auto; max-height: 130px; margin: 0 auto;">--%>
            <div class="grid-out" style="width: 787px; margin: 0 auto;">
                <div class="grid-in" style="height: 97px; ">
                    <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                    <asp:GridView ID="grvInquiryList" runat="server"></asp:GridView>
                </div>
            </div>

            <div style="padding-right: 100px; text-align:right;">
                <asp:Button ID="btnInquiry" runat="server" Text="照会" />
            </div>

            <asp:ValidationSummary ID="vasSummary1" runat="server" CssClass="errortext" ValidationGroup="1" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnInquiry" />
        </Triggers>
    </asp:UpdatePanel>

    <br>
    <hr style="margin-top: 10px;">

    <div class="text-center">
        <asp:Label ID="Label11" runat="server" Text="照会結果" Font-Bold="true"/>
    </div>

    <br>

    <table style="width: 400px;" class="center">
        <tr>
            <td style="width: 200px; padding: 0px 0px 0px 0px;">
                <uc:ClsCMTextBox runat="server" ID="txtTboxId2" ppName="ＴＢＯＸＩＤ"
                    ppIMEMode="半角_変更不可" ppCheckHan="true" ppNum="True" ppMaxLength="8" ppValidationGroup="2" ppWidth="70" />
            </td>
            <td style="width: 200px; padding: 0px 0px 0px 0px;">
                <uc:ClsCMDateBox runat="server" ID="dtbInquiryDate" ppName="照会日付" ppDateFormat="年月日"
                    ppValidationGroup="2" />
            </td>
        </tr>
    </table>

    <br>

    <div class="grid-out">
        <div class="grid-in">
            <input id="Hidden2" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvResultList" runat="server"></asp:GridView>
        </div>
    </div>

    <div style="padding-right:10px; padding-top: 10px; text-align: right;">
        <a href="../../Master/COMUPDMB2/COMUPDMB2.aspx" target="_blank">結果/詳細コード参照</a>
    </div>
    
    <asp:ValidationSummary ID="vasSummary2" runat="server" CssClass="errortext" ValidationGroup="2" />

</asp:Content>
