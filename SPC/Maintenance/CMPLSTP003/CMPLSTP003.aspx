<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CMPLSTP003.aspx.vb" Inherits="SPC.CMPLSTP003" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width: 700px; margin-left: auto; margin-right: auto; border: none; text-align: left;" border="0">
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlInsUpd" runat="server" ppNameWidth="67" ppWidth="80" ppRequiredField="true" ppName="モード" ppClassCD="0140" ppNotSelect="true" />
            </td>
            <td>
                <uc:ClsCMDateBox runat="server" ID="dttCntDt" ppName="集計日" ppRequiredField="True" ppNameWidth="80" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtTboxid" runat="server" ppWidth="96" ppNum="true" ppName="TBOXID" ppMaxLength="8" ppCheckLength="true" ppCheckHan="True" ppIMEMode="半角_変更不可" ppNameWidth="80" ppRequiredField="true" />
                <%--<uc:ClsCMTextBoxFromTo ID="txtTboxid" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="20" ppName="TBOXID" ppNameWidth="100" ppWidth="80" ppCheckHan="True" />--%>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <%--<asp:Label ID="lblPreCd" runat="server" Text="都道府県" Width="77px" Style="margin-left: 3px;"></asp:Label>
                <asp:DropDownList ID="ddlPreNm" runat="server" Width="100px" DataSourceID="SqlDataSource1" DataTextField="県名" DataValueField="県コード" ValidationGroup="1">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SPCDB %>" SelectCommand="SELECT 県コード, 県名 FROM (SELECT '' AS 県コード, '' AS 県名 UNION ALL SELECT DISTINCT M11_STARE_CD AS 県コード, M11_ADDR1 AS 県名 FROM M11_ADDRESS) AS AREA ORDER BY 県コード"></asp:SqlDataSource>--%>
                <uc:ClsCMDropDownList ID="ddlPre" runat="server" ppNameWidth="80" ppWidth="110" ppRequiredField="true" ppName="都道府県" ppClassCD="0001" />
            </td>
            <td>
                <%--<asp:Label ID="Label2" runat="server" Text="エリア" Width="77px" Style="margin-left: 3px;"></asp:Label>
                <asp:DropDownList ID="ddlArea" runat="server" Width="100px" DataSourceID="SqlDataSource3" DataTextField="エリア" DataValueField="コード" ValidationGroup="1">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SPCDB %>" SelectCommand="SELECT コード, エリア FROM (SELECT '' AS コード, '' AS エリア UNION ALL SELECT M29_CODE AS コード, M29_NAME AS エリア FROM M29_CLASS WHERE (M29_CLASS_CD = '0080')) AS AREA ORDER BY コード"></asp:SqlDataSource>--%>
                <uc:ClsCMDropDownList ID="ddlArea" runat="server" ppNameWidth="80" ppWidth="100" ppRequiredField="true" ppNotSelect="true" ppName="エリア" ppClassCD="0080" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <%--<asp:Label ID="Label1" runat="server" Text="システム" Width="77px" Style="margin-left: 3px;"></asp:Label>
                <asp:DropDownList ID="ddlSystem" runat="server" Width="100px" DataSourceID="SqlDataSource2" DataTextField="システム" DataValueField="システムコード" ValidationGroup="1">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SPCDB %>" SelectCommand="SELECT システム, システムコード FROM (SELECT '' AS システム, '' AS システムコード UNION ALL SELECT M23_TBOXCLS + ':' + M23_TBOXCLS_NM AS システム, M23_TBOXCLS AS システムコード FROM M23_TBOXCLASS) AS SYSTEM ORDER BY システムコード"></asp:SqlDataSource>--%>
             <uc:ClsCMDropDownList ID="ddlSystem" runat="server" ppNameWidth="80" ppWidth="110" ppRequiredField="true" ppName="システム" ppClassCD="0001" />
            </td>
            <td>
                <%--<asp:Label ID="lblver" runat="server" Text="バージョン" Width="77px" Style="margin-left: 3px;"></asp:Label>
                <asp:DropDownList ID="ddlVer" runat="server" Width="100" ValidationGroup="1"></asp:DropDownList>--%>
             <uc:ClsCMDropDownList ID="ddlVer" runat="server" ppNameWidth="80" ppWidth="100" ppRequiredField="true" ppName="バージョン" ppClassCD="0001" />
            </td>
        </tr>
    </table>
    <table style="width: 1050px;" border="0" class="center">
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <table border="1" style="width: 60%; border-collapse: collapse;">
        <tr>
            <td rowspan="2" style="background-color: #8DB4E2; width: 82px; vertical-align: middle; text-align: center; font-weight: bold;">
                <asp:Label ID="Label3" runat="server" Text="合計"></asp:Label>
            </td>
            <td style="background-color: #8DB4E2; width: 70px; text-align: center; font-weight: bold;">
                <asp:Label ID="Label4" runat="server" Text="件数"></asp:Label>
            </td>
            <td style="background-color: #8DB4E2; width: 70px; text-align: center; font-weight: bold;">
                <asp:Label ID="Label5" runat="server" Text="金額"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="background-color: #FFFFFF; width: 60px;">
                <asp:Label ID="lblCount" runat="server" Text="0" CssClass="float-right"></asp:Label>
            </td>
            <td style="background-color: #FFFFFF; width: 80px;">
                <asp:Label ID="lblAmount" runat="server" Text="0" CssClass="float-right"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out" style="height: 560px;">
        <div id="DivIn" runat="server" class="grid-in" style="height: 560px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
