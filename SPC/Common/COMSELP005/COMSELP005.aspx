<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMSELP005.aspx.vb" Inherits="SPC.COMSELP005" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <td style="width: 80px">
                <asp:TextBox ID="txtTraderCd" runat="server" Width="50px" Enabled="False"></asp:TextBox>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtTraderNm" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtCompanyCd" runat="server" Width="50px" Enabled="False"></asp:TextBox>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtCompanyNm" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCpyPostalCd" runat="server" Text="郵便番号" Width="70px"></asp:Label>
            </td>
            <td style="width: 150px">
                <uc:ClsCMTextBox ID="txtCmpZipNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="100" ppEnabled="False" />
            </td>
            <td style="width: 80px">
                <asp:Label ID="lblCpyPrefectureCd" runat="server" Text="県コード" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtCmpStateCd" runat="server" ppName="：" ppNameWidth="20" ppWidth="50" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCpyAddress" runat="server" Text="住所" Width="70px"></asp:Label>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtCmpAddress" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCpyTelNo" runat="server" Text="電話番号" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtCmpTelNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="150" ppEnabled="False" />
            </td>
            <td style="width: 80px">
                <asp:Label ID="lblCpyFaxNo" runat="server" Text="ＦＡＸ番号" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtCmpFaxNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="150" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtIgtCd" runat="server" Width="50px" Enabled="False"></asp:TextBox>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtIgtNm" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSvoPostalCd" runat="server" Text="郵便番号" Width="70px"></asp:Label>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtIgtZipNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="100" ppEnabled="False" />
            </td>
            <td>
                <asp:Label ID="lblSvoPrefectureCd" runat="server" Text="県コード" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtIgtStateCd" runat="server" ppName="：" ppNameWidth="20" ppWidth="50" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSvoAddress" runat="server" Text="住所" Width="70px"></asp:Label>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtIgtAddress" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSvoTelNo" runat="server" Text="電話番号" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtIgtTelNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="150" ppEnabled="False" />
            </td>
            <td>
                <asp:Label ID="lblSvoFaxNo" runat="server" Text="ＦＡＸ番号" Width="70px"></asp:Label>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtIgtFaxNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="150" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtOfficeCd" runat="server" Width="50px" Enabled="False"></asp:TextBox>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtOfficeNm" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOfcPostalCd" runat="server" Text="郵便番号" Width="70px"></asp:Label>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtOfcZipNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="100" ppEnabled="False" />
            </td>
            <td>
                <asp:Label ID="lblOfcPrefectureCd" runat="server" Text="県コード" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtOfcStateCd" runat="server" ppName="：" ppNameWidth="20" ppWidth="50" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOfcAddress" runat="server" Text="住所" Width="70px"></asp:Label>
            </td>
            <td colspan="4">
                <uc:ClsCMTextBox ID="txtOfcAddress" runat="server" ppName="：" ppNameWidth="20" ppWidth="930" ppEnabled="False" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblOfcTelNo" runat="server" Text="電話番号" Width="70px"></asp:Label>
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtOfcTelNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="150" ppEnabled="False" />
            </td>
            <td>
                <asp:Label ID="lblOfcFaxNo" runat="server" Text="ＦＡＸ番号" Width="70px"></asp:Label>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtOfcFaxNo" runat="server" ppName="：" ppNameWidth="20" ppWidth="150" ppEnabled="False" />
            </td>
        </tr>
    </table>
</asp:Content>
