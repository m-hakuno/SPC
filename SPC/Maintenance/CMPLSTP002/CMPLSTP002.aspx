<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CMPLSTP002.aspx.vb" Inherits="SPC.CMPLSTP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:100%;" border="0">
        <tr>
            <td>
                <table style="width: 100%;" border="0">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="lblMaker" runat="server" Text="メーカ区分"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMaker" runat="server" Width="300px">
                            </asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="pnlddlMakerErr" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvddlMaker" runat="server" ControlToValidate="ddlMaker" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftReqDt" runat="server" ppName="請求年月" ppNameWidth="100" ppDateFormat="年月" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out" style="height:460px;">
        <div id="DivIn" runat="server" class="grid-in" style="height:460px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <table style="width:100%;">
        <tr>
            <td class="float-right" style="padding-right: 10px">
                <!--合計件数-->
                <asp:Label ID="lblTotalSum" runat="server" Text="合計件数"></asp:Label>
                <br />
                <!--合計金額-->
                <asp:Label ID="lblTotalAmount" runat="server" Text="合計金額"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
