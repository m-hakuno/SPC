<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMLGIP001.aspx.vb" Inherits="SPC.COMLGIP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">

    <div class="center" style="width: 315px">
        <uc:ClsCMTextBox ID="txtUserNm" runat="server" ppName="ユーザ名" ppNameWidth="100" ppRequiredField="True" ppMaxLength="10" ppIMEMode="半角_変更不可" ppWidth="132px" ppMesType="メッセージ" ppCheckHan="True" />
        <uc:ClsCMTextBox ID="txtPass" runat="server" ppName="パスワード" ppNameWidth="100" ppTextMode="Password" ppRequiredField="True" ppMaxLength="10" ppIMEMode="半角_変更不可" ppWidth="132" ppMesType="メッセージ" ppCheckHan="True" />
        <br />
        <div class="float-right">
            <asp:Button ID="btnLogin" runat="server" Text="ログイン" />
            &nbsp;<asp:Button ID="btnClear" runat="server" Text="クリア" CausesValidation="False" />
        </div>
    </div>
</asp:Content>
