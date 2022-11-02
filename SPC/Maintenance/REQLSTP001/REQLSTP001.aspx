<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference_Reverse.master" CodeBehind="REQLSTP001.aspx.vb" Inherits="SPC.REQLSTP001" %>

<%@ MasterType VirtualPath="~/Reference_Reverse.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <script type="text/javascript">

        onload = pageonload

        function pageonload() {
            //ヒドゥンの取得
            var element = document.getElementById("cphMainContent_cphSearchContent_hdnScrollTop");
            if (element.value == 0) {
                window.scroll(0, 0);
            }
        }

    </script>
    <asp:HiddenField ID="hdnScrollTop" runat="server" Value="0" />
    <table border="0" class="center">
        <tr>
            <td>
                <table style="width: 100%; text-align: left; font-size: 12px;" border="0">
                    <tr>
                        <td colspan="2">
                            <uc:ClsCMTextBoxFromTo runat="server" ID="txtTboxID" ppName="TBOXID"
                                ppNameWidth="110" ppMaxLength="8" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="112" ppNum="true" ppFontSize="12pt" />
                        </td>
                        <td colspan="2">
                            <uc:ClsCMTextBoxFromTo runat="server" ID="txtTrbl_No" ppName="管理番号"
                                ppNameWidth="110" ppMaxLength="11" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="112" ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc:ClsCMDateBoxFromTo ID="dftRcpt_Dt" runat="server" ppName="受付日" ppNameWidth="110" ppFontSize="12pt" />
                        </td>
                        <td colspan="2">
                            <uc:ClsCMDateBoxFromTo ID="dftApp_Dt" runat="server" ppName="承認日" ppNameWidth="110" ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <td style="margin-left: 1px; width: 291px;">
                            <uc:ClsCMTextBox ID="txtNL_Cls" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="NL区分" ppWidth="30"
                                ppNameWidth="110" ppCheckHan="True" ppCheckLength="True" ppFontSize="12pt" ppExpression="[n|N|l|L|j|J]" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtEW_Cls" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="EW区分" ppWidth="30"
                                ppNameWidth="110" ppCheckHan="True" ppCheckLength="True" ppFontSize="12pt" ppExpression="[e|E|w|W]" />
                        </td>
                        <td style="width: 170px">
                            <uc:ClsCMTextBox ID="txtBranch_Cd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="保担営業所" ppWidth="50"
                                ppNameWidth="110" ppCheckHan="True" ppNum="True" ppFontSize="12pt" />
                        </td>
                        <td style="width: 296px">
                            <asp:Label ID="lblBranch" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <span style="position: relative;">
                                <label style="font-size: 12pt; margin-left: 2px">都道府県</label>
                                <span style="position: relative; left: 45px;">
                                    <asp:DropDownList runat="server" ID="ddlPrefectureFm" Width="150px" Style="font-size: 12pt;" />
                                    <label style="font-size: 16px;">～ </label>
                                    <asp:DropDownList runat="server" ID="ddlPrefectureTo" Width="150px" Style="font-size: 12pt;" />
                                </span>
                            </span>
                            <br />
                            <span style="position: relative;">
                                <label></label>
                                <span style="position: relative; left: 117px;">
                                    <asp:CustomValidator runat="server" ID="cuvPrefecture" ControlToValidate="ddlPrefectureFm" CssClass="errortext"
                                        ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" />
                                </span>
                            </span>
                        </td>
                        <td colspan="2">
                            <span style="position: relative; margin-left: 4px;">
                                <label style="font-size: 12pt;">作業状況</label>
                                <span style="position: relative; left: 43px;">
                                    <asp:DropDownList runat="server" ID="ddlSagyoJokyoFm" Width="150px" Style="font-size: 12pt;" />
                                    <label style="font-size: 16px;">～ </label>
                                    <asp:DropDownList runat="server" ID="ddlSagyoJokyoTo" Width="150px" Style="font-size: 12pt;" />
                                </span>
                            </span>
                            <br />
                            <span style="position: relative;">
                                <span style="position: relative; left: 117px;">
                                    <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                        <asp:CustomValidator runat="server" ID="cuvddlSagyoJokyo" ControlToValidate="ddlSagyoJokyoFm" CssClass="errortext"
                                            ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" />
                                    </asp:Panel>
                                </span></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <%--<span style="position: relative;">--%>
                            <asp:Label ID="lblsagyo" runat="server" Style="font-size: 12pt; margin-left: 2px" Text="発生区分" />
                            <span style="position: relative; left: 45px;">
                                <asp:DropDownList runat="server" ID="ddlHPN_Cls" Width="150px" Style="font-size: 12pt;" />
                            </span>
                            <%--</span>--%>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; text-align: left">
                    <tr>
                        <td style="width: 104px; vertical-align: top; padding-top: 12px;">
                            <asp:Label ID="lblTboxClass" Style="font-size: 12pt; margin-left: 2px" runat="server" Text="TBOXﾀｲﾌﾟ" />
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cklTboxClass" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                                RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; text-align: left;" border="0">
                    <tr>
                        <td style="width: 110px">
                            <uc:ClsCMTextBox ID="txtTbox_Ver" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="VER" ppWidth="60"
                                ppNameWidth="110" ppCheckHan="True" ppCheckLength="True" ppNum="False" ppFontSize="12pt" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table style="width: 100%; text-align: left;">
                        <tr>
                            <td style="width: 112px">
                                <asp:Label ID="Label1" runat="server" Text="装置詳細" Style="font-size: 12pt;" />
                            </td>
                            <td style="float: left;">
                                <asp:DropDownList runat="server" ID="ddlEQDvs1" Width="150px" Style="font-size: 12pt;" />
                            </td>
                            <td style="float: left; margin-left: 20px;">
                                <asp:DropDownList runat="server" ID="ddlEQDvs2" Width="150px" Style="font-size: 12pt;" />
                            </td>
                            <td style="float: left; margin-left: 20px;">
                                <asp:DropDownList runat="server" ID="ddlEQDvs3" Width="150px" Style="font-size: 12pt;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="padding-left: 112px;">
                                <asp:CheckBoxList ID="cklEQ" runat="server" CellSpacing="4" TextAlign="Right" RepeatLayout="Table" RepeatDirection="Horizontal"
                                    RepeatColumns="8" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 100%; text-align: left;" border="0">
                    <tr>
                        <td style="width: 508px">
                            <uc:ClsCMTextBox ID="txtARRSM" runat="server" ppMaxLength="30" ppName="申告内容" ppWidth="330" ppNameWidth="110" ppIMEMode="全角"
                                ppFontSize="12pt" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtARRS" runat="server" ppMaxLength="30" ppName="申告内容詳細" ppWidth="330" ppNameWidth="110" ppIMEMode="全角"
                                ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <uc:ClsCMTextBox ID="txtASPS" runat="server" ppMaxLength="30" ppName="対応内容" ppWidth="330" ppNameWidth="110" ppIMEMode="全角"
                                ppFontSize="12pt" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div id="divCount" runat="server" class="float-Left">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" Style="font-size: 12pt;"></asp:Label>
                </td>
                <td style="width: 80px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX" Style="font-size: 12pt;"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" Style="font-size: 12pt;"></asp:Label>
                </td>
                <td style="width: 15px"></td>
                <td>
                    <asp:Button ID="btnReload" runat="server" CssClass="center" Text="リロード" />
                </td>
            </tr>
        </table>
    </div>

    <div id="DivOut" runat="server" class="grid-out" style="height: 649px">
        <div id="DivIn" runat="server" class="grid-in" style="height: 649px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
