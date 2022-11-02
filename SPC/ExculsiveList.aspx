<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.Master" CodeBehind="ExculsiveList.aspx.vb" Inherits="SPC.ExculsiveList" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td style="width:99px;padding-left:4px;">
                <asp:Label ID="Label2" runat="server" Text="場所区分"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlPlace" runat="server">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="1">1:ＳＰＣ</asp:ListItem>
                    <asp:ListItem Value="2">2:営業所</asp:ListItem>
                    <asp:ListItem Value="3">3:ＮＧＣ</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtUserID" runat="server" ppMaxLength="50" ppName="ユーザーID" ppNameWidth="100" ppWidth="196" ppIMEMode="半角_変更不可" />
            </td>
        </tr>
        <tr>
            <td style="width:99px;padding-left:4px;">
                <asp:Label ID="Label3" runat="server" Text="IPアドレス"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlIp" runat="server" Width="200"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMDateBoxFromTo ID="dftSetDt" runat="server" ppName="排他日付" ppNameWidth="100"  />
            </td>
        </tr>
        <tr>
            <td style="width:99px;padding-left:5px;">
                <asp:Label ID="Label1" runat="server" Text="画面名"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlDisp" runat="server" Width="300"></asp:DropDownList>
            </td>
        </tr>
    </table>
   
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
