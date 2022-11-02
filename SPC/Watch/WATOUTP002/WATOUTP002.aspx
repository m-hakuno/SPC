<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WATOUTP002.aspx.vb" Inherits="SPC.WATOUTP002" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

        <asp:RadioButtonList ID="rblSlctPrnt" runat="server" CssClass="center">
            <asp:ListItem Value="1">    決済照会情報</asp:ListItem>
            <asp:ListItem Value="2">    機番別合算</asp:ListItem>
            <asp:ListItem Value="3">    島別合算</asp:ListItem>
        </asp:RadioButtonList>
    
</asp:Content>
