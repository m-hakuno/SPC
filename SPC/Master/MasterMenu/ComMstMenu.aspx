<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ComMstMenu.aspx.vb" Inherits="SPC.WebForm1" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <div style ="text-align :center ">
        <asp:BulletedList ID="bltMenu" runat="server" DisplayMode="LinkButton" DataTextField="M17_DISP_NM" DataValueField="M17_DISP_CD" Width="100%">
        </asp:BulletedList>
    </div>
</asp:Content>
