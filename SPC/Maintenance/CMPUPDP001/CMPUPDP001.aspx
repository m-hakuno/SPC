<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CMPUPDP001.aspx.vb" Inherits="SPC.CMPUPDP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <uc:ClsCMDateBox runat="server" ID="dtbAggDt" ppDateFormat="年月日" ppName="集　計　日" ppNameWidth="80"
            ppRequiredField="True" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <!--エリア合計-->
    <table border="1" style="width:60%; border-collapse: collapse;">
        <tr>
            <td rowspan="2" style="background-color: #8DB4E2; width: 82px;
                                    vertical-align:middle; text-align: center; font-weight: bold;">
                <asp:Label ID="Label1" runat="server" Text="エリア合計" ></asp:Label>
            </td>
            <td colspan="2" style="background-color: #8DB4E2; width: 140px;
                                    text-align: center; font-weight: bold;" >
                <asp:Label ID="Label2" runat="server" Text="ＦＳエリア"></asp:Label>
            </td>
            <td colspan="2" style="background-color: #8DB4E2; width: 140px;
                                    text-align: center; font-weight: bold;">
                <asp:Label ID="Label3" runat="server" Text="ＣＳエリア"></asp:Label>
            </td>
            <td colspan="2" style="background-color: #8DB4E2; width: 140px;
                                    text-align: center; font-weight: bold;">
                <asp:Label ID="Label4" runat="server" Text="その他エリア"></asp:Label>
            </td>
            <td colspan="2" style="background-color: #8DB4E2; width: 140px;
                                    text-align: center; font-weight: bold;">
                <asp:Label ID="Label5" runat="server" Text="総合計"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="background-color: #FFFFFF; width: 60px;">
                <asp:Label ID="lblFsAreaCount" runat="server" Text="9,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 80px;">
                <asp:Label ID="lblFsAreaAmount" runat="server" Text="9,999,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 60px;">
                <asp:Label ID="lblCsAreaCount" runat="server" Text="9,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 80px;">
                <asp:Label ID="lblCsAreaAmount" runat="server" Text="9,999,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 60px;">
                <asp:Label ID="lblOtAreaCount" runat="server" Text="9,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 80px;">
                <asp:Label ID="lblOtAreaAmount" runat="server" Text="9,999,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 60px;">
                <asp:Label ID="lblTotalAreaCount" runat="server" Text="9,999" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 80px;">
                <asp:Label ID="lblTotalAreaAmount" runat="server" Text="9,999,999" CssClass="float-right"></asp:Label>
            </td>
        </tr>
    </table>
    <br>
    <!--グリッド（都道府県別）-->
    <div id="DivOutState" runat="server" class="grid-out">
        <div id="DivInState" runat="server" class="grid-in" style="height: 260px">
            <input id="hdnDataState" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvListState" runat="server" CssClass="grid" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="grid-row2" />
                <HeaderStyle CssClass="grid-head" />
                <RowStyle CssClass="grid-row1" />
            </asp:GridView>
        </div>
    </div>
    <br>
    <!--グリッド（総合計）-->
    <div id="DivOutSummary" runat="server" class="grid-out">
        <div id="DivInSummary" runat="server" class="grid-in" style="height: 85px">
            <input id="hdnDataSummary" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvListSummary" runat="server" CssClass="grid" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="grid-row2" />
                <HeaderStyle CssClass="grid-head" />
                <RowStyle CssClass="grid-row1" />
            </asp:GridView>
        </div>
    </div>
    <br>
    <!--グリッド（LAN）-->
    <div id="DivOutLan" runat="server" class="grid-out">
        <div id="DivInLan" runat="server" class="grid-in" style="height: 70px">
            <input id="HiddenDataLan" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvListLan" runat="server" CssClass="grid" ShowHeaderWhenEmpty="true">
                <AlternatingRowStyle CssClass="grid-row2" />
                <HeaderStyle CssClass="grid-head" />
                <RowStyle CssClass="grid-row1" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
