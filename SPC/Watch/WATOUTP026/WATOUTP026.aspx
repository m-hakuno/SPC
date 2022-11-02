<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WATOUTP026.aspx.vb" Inherits="SPC.WATOUTP026" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br>
    <div class="center" style="width:100px; line-height:22px;">
        <asp:Label ID="Label1" runat="server" Text="通信状態"></asp:Label><br>           
                <asp:RadioButtonList ID="rblSlctPrnt" runat="server" CssClass="center">
                <asp:ListItem Value="1">    全て</asp:ListItem>
                <asp:ListItem Value="2">    接続分</asp:ListItem>
                <asp:ListItem Value="3">    未接続分</asp:ListItem>
            </asp:RadioButtonList>
    </div>
</asp:Content>
