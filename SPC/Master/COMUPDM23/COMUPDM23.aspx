<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM23.aspx.vb" Inherits="SPC.COMUPDM23" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
    <style type="text/css">
        .auto-style2
        {
        }
        .auto-style4
        {
            width: 120px;
        }
        .auto-style6
        {
            width: 204px;
        }
        .auto-style7
        {
            width: 100px;
        }
        .auto-style8
        {
            width: 275px;
        }
        .auto-style9
        {
            width: 210px;
        }
        .auto-style10
        {
            width: 265px;
        }
        </style>
</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:600px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBoxFromTo ID="tftSSystemCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="システム" ppNameWidth="100" ppWidth="30" ppCheckHan="True" ppValidationGroup="search" ppNum="true"/>
            </td>
            <td class="auto-style10">
                <uc:ClsCMTextBox ID="txtSSystemNm" runat="server" ppIMEMode="半角_変更可" ppName="システム名" ppNameWidth="100" ppMaxLength="10" ppWidth="150" />
            </td>
             
                    </tr>
        <tr>
            <td style="text-align:left" class="auto-style7">
                <asp:Label ID="Label2" runat="server" Text="システム分類" Width="90" style="margin-left: 2px"></asp:Label>
            </td>
            <td style="text-align:left" class="auto-style9">
                <asp:DropDownList ID="ddlSSystemCls" runat="server" Width="150" style="margin-left: 0px"></asp:DropDownList>
            </td>
            <td class="auto-style10">
                <uc:ClsCMDropDownList ID="ddlSSumCls" runat="server" ppName="集計用区分" ppNameWidth="100" ppWidth="100" ppClassCD="0095" ppNotSelect="true" />
            </td>
        </tr>
    </table>
             <uc:ClsCMDropDownList ID="ddlDel" runat="server" ppName="削除区分" ppNameWidth="0" ppWidth="0" ppClassCD="0124" ppNotSelect="true" Visible="false" />
    </asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width:890px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
        <tr>
            <td class="auto-style4">
                            <asp:Label ID="Label3" runat="server" Text="システム" Width="100px" style="margin-left: 4px"></asp:Label>
              </td>
            <td class="auto-style6">
           <asp:TextBox ID="txtSystemCd" runat="server" Width="30px" CssClass="IMEdisabled" MaxLength="2" AutoPostBack="True"></asp:TextBox>
                <asp:CustomValidator ID="CstSystemCd" runat="server" ControlToValidate="txtSystemCd" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="key" SetFocusOnError="True"></asp:CustomValidator>

                        </td>
            <td class="auto-style8">
                <uc:ClsCMTextBox ID="txtSystemNm" runat="server" ppIMEMode="半角_変更可" ppName="システム名" ppNameWidth="100" ppMaxLength="10" ppWidth="150" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtShortNm" runat="server" ppIMEMode="半角_変更可" ppName="システム略称" ppNameWidth="100" ppMaxLength="10" ppWidth="150" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2" colspan="2">
                <table>
                    <tr>
                        <td class="auto-style7">
                            <asp:Label ID="Label4" runat="server" Text="システム分類" Width="115px" style="margin-left: 0px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSystemCls" runat="server" Width="150" style="margin-left: 3px"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="pnlSystemErr" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvSystemCls" runat="server" ValidationGroup="val" ControlToValidate="ddlSystemCls" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="auto-style8">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="集計用区分" Width="100"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSumCls" runat="server" Width="100"></asp:DropDownList>
                            <div style="white-space: nowrap">
                                <asp:Panel ID="Panel1" runat="server" Width="0px">
                                    <asp:CustomValidator ID="cuvSumCls" runat="server" ValidationGroup="val" ControlToValidate="ddlSumCls" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
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
    <div class="grid-out" style="width:648px;height:380px">
        <div class="grid-in" style="width:648px;height:380px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>