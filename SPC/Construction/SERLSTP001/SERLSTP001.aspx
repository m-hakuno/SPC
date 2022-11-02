<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="SERLSTP001.aspx.vb" Inherits="SPC.SERLSTP001" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width: 98%;" class="center" border="0">
        <tr>
            <td colspan="2">
                <!--シリアル番号-->
                <uc:ClsCMTextBoxFromTo ID="txtSerialNo" runat="server" ppName="シリアル番号"
                    ppNameWidth="100" ppWidth="215" ppIMEMode="半角_変更不可" ppCheckHan="True"
                    ppMaxLength="30" />
            </td>
            <!--システム-->
            <td style="padding-left: 4px">
                <asp:Label ID="lblSystem" runat="server" Text="システム" Width="98"></asp:Label>
                <asp:DropDownList ID="ddlSystem" runat="server" Width="120"></asp:DropDownList>
            </td>
            <!--管理番号-->
            <td>
                <uc:ClsCMTextBox ID="txtCntlNo" runat="server" ppName="管理番号" ppNameWidth="100"
                    ppWidth="120" ppCheckHan="True" ppIMEMode="半角_変更不可" ppMaxLength="15" />
            </td>
        </tr>
        <tr>
            <!--場所区分-->
            <td>
                <uc:ClsCMDropDownList ID="ddlPlaceclass" runat="server" ppName="場所区分"
                    ppNameWidth="100" ppWidth="100" ppMode="名称" ppClassCD="0048"
                    ppNotSelect="True" />
            </td>
            <!--現設置／保管場所-->
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtStrageCd" runat="server" ppName="現設置／保管場所"
                                ppNameWidth="109" ppWidth="80" ppIMEMode="半角_変更不可" ppCheckHan="True"
                                ppMaxLength="8" />
                        </td>
                        <td>
                            <asp:Label ID="lblStrageName" runat="server" Text="" Width="200px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <!--機器分類-->
            <td style="padding-left: 3px">
                <asp:Label ID="lblAppaDiv" runat="server" Text="機器分類" Width="99"></asp:Label>
                <asp:DropDownList ID="ddlAppaDiv" runat="server" Width="120"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <!--機器種別-->
            <td style="padding-left: 7px">
                <asp:Label ID="lblAppaCls" runat="server" Text="機器種別" Width="107"></asp:Label>
                <asp:DropDownList ID="ddlAppaCls" runat="server" Width="240"></asp:DropDownList>
            </td>
            <!--型式／機器-->
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtAppaFmt" runat="server" ppName="型式／機器"
                    ppNameWidth="100" ppWidth="150" ppIMEMode="半角_変更不可"
                    ppMaxLength="20" ppCheckHan="True" />
            </td>

        </tr>
        <tr>
            <!--移動日-->
            <td>
                <uc:ClsCMDateBox runat="server" ID="dtbMoveDt" ppName="移動日" ppDateFormat="年月日"
                    ppNameWidth="100" />
            </td>
            <!--納入予定日-->
            <td style="padding-left:3px">
                <uc:ClsCMDateBox runat="server" ID="dtbDlvPlnDt" ppName="納入予定日" ppDateFormat="年月日"
                    ppNameWidth="110" />
            </td>
            <!--移動理由-->
            <td colspan="2" style="padding-left: 4px">
                <asp:Label ID="Label1" runat="server" Text="移動理由" Width="98"></asp:Label>
                <asp:DropDownList ID="ddlMoveReason" runat="server" Width="280"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="100" ppWidth="110" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <!--グリッド-->
    <div class="grid-out">
        <div class="grid-in" style="height: 364px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <!--印刷帳票選択-->
    <table class="float-right" border="0">
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlPrnSelect" ppName="帳票選択" ppClassCD="0062"
                    ppNotSelect="true" ppMode="名称" ppWidth="220" />
            </td>
        </tr>
        <tr>
            <td>
                <table class="center" border="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnCsv" runat="server" Text="ＣＳＶ" Width="70"
                                CausesValidation="false" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnPdf" runat="server" Text="ＰＤＦ" Width="70"
                                CausesValidation="false" />
                            &nbsp;
                            <asp:Button ID="btnPrint" runat="server" Text="印刷" Width="70"
                                CausesValidation="false" />
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
