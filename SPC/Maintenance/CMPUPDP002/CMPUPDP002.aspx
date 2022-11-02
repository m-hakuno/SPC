<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CMPUPDP002.aspx.vb" Inherits="SPC.CMPUPDP002" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMDateBox runat="server" ID="dttReq_Dt" ppName="請求年月" ppDateFormat="年月" ppNameWidth="110"  />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="tftTboxId" ppName="ＴＢＯＸＩＤ" ppNameWidth="110" ppMaxLength="8" ppCheckHan="True" ppNum="True" ppWidth="70" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 110px">
                            <asp:Label ID="lblTboxClass" runat="server" Text="ＴＢＯＸタイプ"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTboxClass" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td style="width: 60px">
                            <asp:Label ID="lblAppaClass" runat="server" Text="機器名"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAppaClass" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out" style="height:420px;">
        <div id="DivIn" runat="server" class="grid-in" style="height:420px;">
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
