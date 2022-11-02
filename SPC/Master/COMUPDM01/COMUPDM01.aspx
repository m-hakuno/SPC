<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM01.aspx.vb" Inherits="SPC.COMUPDM01" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
</asp:Content>
<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:600px;margin-left:auto;margin-right:auto;border:none;">
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtSUserID" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="10" ppName="ユーザーID" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppValidationGroup="search" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="会社" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSComp" runat="server" Width="105"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="社員" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSEmp" runat="server" Width="240"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:600px;margin-left:auto;margin-right:auto;border:none;">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtUserID" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="10" ppName="ユーザーID" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppValidationGroup="key" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtPassword" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="10" ppName="パスワード" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppRequiredField="true" ppValidationGroup="val" />
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="会社" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlComp" runat="server" Width="105"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="pnlSystemErr" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvComp" runat="server" ValidationGroup="val" ControlToValidate="ddlComp" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="社員" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEmp" runat="server" Width="240"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="Panel1" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvEmp" runat="server" ValidationGroup="val" ControlToValidate="ddlEmp" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">  
    <div class="grid-out" style="width:657px;">
        <div class="grid-in" style="width:657px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>