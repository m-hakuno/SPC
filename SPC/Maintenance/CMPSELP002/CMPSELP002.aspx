<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CMPSELP002.aspx.vb" Inherits="SPC.CMPSELP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/UserControl/ClsCMDateTimeBox.ascx" TagPrefix="uc" TagName="ClsCMDateTimeBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript">
        function lenCheck(obj, size) {
            var strW = obj.value;
            var lenW = strW.length;
            if (size < lenW) {
                var limitS = strW.substring(0, size);
                obj.value = limitS;
            }
        }
    </script>
    <style type="text/css">
        .auto-style3 {
            height: 16px;
        }
        .auto-style4 {
            height: 21px;
        }
        .auto-style5 {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px; table-layout: fixed;" class="center" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMnt1" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--管理番号-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="管理番号" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＴＢＯＸタイプ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="ホール名" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHallNm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＶＥＲ-->
                            <td>
                            </td>
                        </tr>
                      <tr>
                            <!--ＴＢＯＸＩＤ-->
                            <td class="auto-style5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="TBOXID" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxID" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＴＢＯＸタイプ-->
                            <td class="auto-style5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="TBOXタイプ" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxType" runat="server"></asp:Label>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                            <!--ＶＥＲ-->
                            <td class="auto-style5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="VER" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxVer" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <asp:Panel ID="pnlMnt4" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table>
                        <tr>
                            <td>
                                <table border="1" style="width:100%; border-collapse: collapse;" >
                                    <tr>
                                        <td style="background-color: #8DB4E2; " class="auto-style3">
                                            <asp:Label ID="Label12" runat="server" Text=" 　機器分類"></asp:Label>
                                        </td>
                                        <td style="background-color: #8DB4E2; " class="auto-style3">
                                            <asp:Label ID="Label1" runat="server" Text=" 　機器種別"></asp:Label>
                                        </td>
                                        <td style="background-color: #8DB4E2; " class="auto-style3">
                                            <asp:Label ID="Label4" runat="server" Text=" 　型式/機器"></asp:Label>
                                        </td>
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label5" runat="server" Text=" 　状態区分"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlRstAppaDiv" runat="server" Width="200" AutoPostBack="True"></asp:DropDownList> 
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvRstAppaDiv" runat="server" ControlToValidate="ddlRstAppaDiv" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlCndAppaDiv" runat="server" Width="200" AutoPostBack="True"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="Panel1" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvCndAppaDiv" runat="server" ControlToValidate="ddlCndAppaDiv" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlRstAppaModel" runat="server" Width="350"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="Panel2" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvRstAppaModel" runat="server" ControlToValidate="ddlRstAppaModel" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <td class="auto-style4">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="Panel3" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvStatus" runat="server" ControlToValidate="ddlStatus" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
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
                                            <!--保守管理連番-->
                                            <asp:Label ID="lblDealMntSeq" runat="server" Text="保守管理連番" Visible="false"></asp:Label>
                                            <asp:Label ID="lblRecKbn" runat="server" Text="対応区分" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr><td>
                                        <asp:Label ID="lblSno" runat="server" Visible="False"></asp:Label>
                                        </td></tr>
                                    <tr><td></td></tr>
                                    <tr><td>&nbsp;</td></tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnDetailInsert" runat="server" Text="追加" ValidationGroup="Detail" />
                                            <asp:Button ID="btnDetailUpdate" runat="server" Text="更新" ValidationGroup="Detail" />
                                            <asp:Button ID="btnDetailDelete" runat="server" Text="削除" ValidationGroup="Detail"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr><td></td></tr>
                                    <tr>
                                        <td class="float-right">
                                            <asp:Button ID="btnDetailClear" runat="server" Text="クリア" ValidationGroup="Detail"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="valSumDetail" runat="server" CssClass="errortext" ValidationGroup="Detail" />
                <br/>
                </asp:Panel>
                <br/>

                <!--【対応明細（グリッド）】-->
                <div id="DivOut" runat="server" class="grid-out" style="width: 1040px">
                    <div id="DivIn" runat="server" class="grid-in" style="height: 120px">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
                <br/>
            </td>
        </tr>
    </table>
</asp:Content>
