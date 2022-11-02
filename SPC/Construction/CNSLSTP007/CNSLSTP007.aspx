<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSLSTP007.aspx.vb" Inherits="SPC.CNSLSTP007" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">

    <table style="width: 1050px;" border="0" class="center">
        <tr>
            <td style="width: 250px;"></td>
            <td style="width: 30px"></td>
            <td style="width: 15px"></td>
            <td style="width: 50px"></td>
            <td style="width: 150px"></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr style="padding-bottom:20px;">
            <td colspan="2" style="padding-left:7px;padding-bottom:8px;">
                <asp:Label ID="lblSearch" runat="server" Text="検索対象" Width="100px"></asp:Label>
                <asp:CheckBox ID="cbxMaintenance" runat="server" Text="保守" />
                <asp:CheckBox ID="cbxConstruction" runat="server" Text="工事" />
            </td>
            <td></td>
            <td colspan="2">
                <uc:ClsCMDropDownList ID="ddlNLCls" runat="server" ppName="ＮＬ区分" ppNameWidth="60" ppWidth="100" ppClassCD="0009" ppMode="名称" ppNotSelect="True" />
            </td>
            <td>&nbsp;</td>
            <td>
                &nbsp;</td>

        </tr>
        <tr>
            <td colspan="4"  style="padding-left:4px;">                
                <uc:ClsCMDateBoxFromTo ID="dftStartDt" runat="server" ppName="開始日" ppNameWidth="100" ppDateFormat="年月日" />
            </td>
            <td>
                <%--<asp:CheckBox ID="cbxFswrkCls" runat="server" Checked="false" />--%>
                <asp:CheckBox ID="cbxFsWrkCls" runat="server" Text="FS稼働無" />
            </td>
            <td></td>
        </tr>
        <tr>
            <td colspan="2" style="padding-left:4px;">
                            <uc:ClsCMDateTimeBox runat="server" ID="dtbDepartureFm" ppName="出発日時" ppNameWidth="100" ppValidationGroup="dep" ValidateRequestMode="Disabled" />
            </td>
            <td>～</td>
            <td colspan="2">
                            <uc:ClsCMDateTimeBox runat="server" ID="dtbDepartureTo" ppName="出発日時" ppNameVisible="false" ppValidationGroup="dep" ValidateRequestMode="Disabled" />
            </td>
        </tr>
        <tr>
            <td style="padding-left:110px">
                            <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlState" ppClassCD="0000" ppValidationGroup="1" ppName="ホール住所" ppNameWidth="100" ppWidth="100" ppNotSelect="True" />

                        </td>
                        <td>
                <uc:ClsCMTextBox ID="txtAddress" runat="server" ppMaxLength="100" ppName="" ppNameWidth="0" ppWidth="400" ppIMEMode="全角" />

                        </td>
                    </tr>
                </table>
            </td>
            <%--            <td colspan="1">


            </td>
            <td colspan="5">
            </td>--%>
        </tr>
        <tr>
            <td style="padding-left:4px;">
                <uc:ClsCMTextBox ID="txtOfficeCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="営業所コード" ppNameWidth="100" ppWidth="60" ppCheckHan="True" ppNum="True" />
            </td>
            <td colspan="6">
                <uc:ClsCMTextBox ID="txtPersonInCharge" runat="server" ppMaxLength="20" ppName="担当者" ppNameWidth="50" ppWidth="120" ppIMEMode="全角" />
            </td>
        </tr>
    </table>

    <table style="width:1050px;margin-top:8px" border="0" class="center">
        <tr>
            <td  style="padding-left:6px;">
                <asp:Label ID="lblHoleConstraction" runat="server" Text="ホール内工事" Width="100px"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="cbxNew" runat="server" Text="新規" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxExpansion" runat="server" Text="増設" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxSomeRemoval" runat="server" EnableTheming="True" Text="一部撤去" Width="100px" />
            </td>
             <td>
                <asp:CheckBox ID="cbxShopRelocation" runat="server" Text="店内移設" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxAllRemoval" runat="server" Text="全撤去" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxOnceRemoval" runat="server" Text="一時撤去" Width="100px" />
            </td>
           
            <td style="width: 20px">&nbsp;</td>
            <td style="width: 30%">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
             <td>
                <asp:CheckBox ID="cbxReInstallation" runat="server" Text="再設置" Width="100px" />
            </td>
             <td>
                <asp:CheckBox ID="cbxConChange" runat="server" Text="構成変更" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxConDelivery" runat="server" Text="構成配信" Width="100px" />
            </td>
           <td>
                <asp:CheckBox ID="cbxOther" runat="server" Text="その他" Width="100px" />
            </td>
                      
            <td>
                <asp:CheckBox ID="cbxVup" runat="server" Text="ＶＵＰ" Width="100px" />
            </td>
            
            <td>&nbsp;</td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out" style="padding-top:28px;">
        <div id="DivIn" runat="server" class="grid-in" style="height:272px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
