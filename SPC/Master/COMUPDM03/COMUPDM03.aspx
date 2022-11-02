<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM03.aspx.vb" Inherits="SPC.COMUPDM03" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
    
    <style type="text/css">
        .auto-style1
        {
            width: 440px;
        }
        .auto-style2
        {
            width: 270px;
        }
        .auto-style3
        {
            width: 220px;
        }
    </style>
    
</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:900px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td class="auto-style1">
                <table>
                    <tr>
                        <td class="auto-style3">
                            &nbsp
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="システム" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSSystem" runat="server" Width="110"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlSSystemCls" runat="server" ppName="システム分類" ppNameWidth="100" ppWidth="130" ppClassCD="0006" ppNotSelect="true" />
            </td>
            
        </tr>
    </table>
                     <uc:ClsCMDropDownList ID="ddldel" Visible="false" runat="server" ppName="削除区分" ppNameWidth="110" ppWidth="120" ppClassCD="0124" ppNotSelect="true"/>
            
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:900px;margin-left:auto;margin-right:auto;border:none;text-align:left">
        <tr>
            <td style="width:270px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="システム" Width="125"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSystem" runat="server" Width="110" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="auto-style2">
                <uc:ClsCMTextBox ID="txtVer" runat="server" ppIMEMode="半角_変更不可" ppName="正式バージョン" ppNameWidth="130" ppMaxLength="5" ppWidth="50" ppCheckHan="true" ppValidationGroup="key" ppExpression="^[a-zA-Z0-9.]+$"/>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtShortVer" runat="server" ppIMEMode="半角_変更不可" ppName="略式バージョン" ppNameWidth="110" ppMaxLength="5" ppWidth="50" ppCheckHan="true" ppValidationGroup="val" ppExpression="^[a-zA-Z0-9.]+$"/>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="システム分類" Width="125"></asp:Label>
                        </td>
                        <td style="text-align:left;">
                            <asp:Label ID="lblSystemCls" runat="server" Width="120"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="auto-style2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="表示用システム分類" Width="130"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDispSysCls" runat="server" Width="55">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="有線無線区分" Width="110"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLineCls" runat="server" Width="70">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtTboxVer" runat="server" ppIMEMode="半角_変更不可" ppName="TBOXタイプ(Ver込み)" ppNameWidth="125" ppMaxLength="1" ppWidth="20" ppCheckHan="true" ppValidationGroup="val" ppExpression="^[a-zA-Z0-9.]+$"/>
            </td>
            <td class="auto-style2">
<%--                <uc:ClsCMTextBox ID="txtType" runat="server" ppIMEMode="半角_変更不可" ppName="現行システム" ppNameWidth="130" ppMaxLength="12" ppWidth="100" ppCheckHan="true" ppValidationGroup="val" ppExpression="^[a-zA-Z0-9.-]+$+\(+\)"/>--%>
                <uc:ClsCMTextBox ID="txtType" runat="server" ppIMEMode="半角_変更不可" ppName="現行システム" ppNameWidth="130" ppMaxLength="12" ppWidth="100" ppValidationGroup="val" ppExpression="^[a-zA-Z0-9\.\-\)\(]+$"/>
            </td>


                    </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">  
    <div class="grid-out" style="width:1053px;height:400px;">
        <div class="grid-in" style="width:1053px;height:400px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>