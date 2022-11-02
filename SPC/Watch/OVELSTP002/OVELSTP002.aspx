<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="OVELSTP002.aspx.vb" Inherits="SPC.OVELSTP002" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
<script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/ctiexe.js")%>'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br />
    <asp:Panel ID="Panel1" runat="server">
        <table class="center">
            <tr>
                <td>
                    <asp:Panel ID="pnlUpdate" runat="server" Width="1080px">
                        <table class="center">
                            <tr>
                                <td>
                                    <asp:Label ID="lblSyokaiDate" runat="server" Text="照会日時"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSyokaiDate_Input" runat="server" Text="YYYY/MM/DD HH:MM:SS"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnltable" runat="server">

        <div class="center" style="width: 1020px; border: 1px solid black">


            <table class="center" style="width: 1000px; border-spacing: 6px;">
                <tr>
                    <td style="width: 90px;">
                        <asp:Label ID="lblNum" runat="server" Text="連番"></asp:Label>
                    </td>
                    <td style="width: 100px;">
                        <asp:Label ID="lblNum_Input" runat="server" Height="10px" Text="0001"></asp:Label>
                    </td>
                    <td style="width: 80px;"></td>
                    <td style="width: 50px;"></td>
                    <td style="width: 70px;"></td>
                    <td style="width: 80px;"></td>
                    <td style="width: 80px;"></td>
                    <td style="width: 100px;"></td>
                    <td style="width: 100px;"></td>
                    <td style="width: 150px;"></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTboxID" runat="server" Text="TBOXID"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTboxID_Input" runat="server" Text="12345678"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNL" runat="server" Text="ＮＬ区分"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNL_Input" runat="server" Text="N"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSystem" runat="server" Text="システム"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSystem_Input" runat="server" Text="IT130S"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVer" runat="server" Text="ＶＥＲ"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVer_Input" runat="server" Text="99.99"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblKentiDate" runat="server" Text="検知日時"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblKentiDate_Input" runat="server" Text="YYYY/MM/DD HH:MM:SS"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblHallName" runat="server" Text="ホール名"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:Label ID="lblHallName_Input" runat="server" Text="１２３４５６７８９０"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblHallTEL" runat="server" Text="ホールＴＥＬ"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblHallTEL_Input" runat="server" Text="03-1234-5678"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblJimisyoTEL" runat="server" Text="事務所ＴＥＬ"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblJimisyoTEL_Input" runat="server" Text="03-1234-5678"></asp:Label>
                    </td>
                </tr>
                <tr style="vertical-align:top;">
                    <td>
                        <asp:Label ID="lblInfo" runat="server" Text="注意情報"></asp:Label>
                    </td>
                    <td colspan="9">
                        <asp:Label ID="lblInfo_Input" runat="server" Width="800" Text="内容１２３４５６７８９０"></asp:Label>
                    </td>
                </tr>
                <tr style="vertical-align: top; display: none;">
                    <td>
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td colspan="5">
                        &nbsp;</td>
                </tr>
            </table>

            <table class="center" style="width: 1000px; border-spacing: 6px;">
                <tr style="vertical-align:top;">
                    <td style="width: 80px;padding-top:6px;">
                        <asp:Label ID="lblTanto" runat="server" Text="担当者"></asp:Label>
                    </td>
                    <td style="width: 170px;padding-top:3px;">
                        <asp:DropDownList ID="ddlCharge" runat="server" Width="120px">
                        </asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:CustomValidator ID="cuvDropDownList" runat="server" ControlToValidate="ddlCharge" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                    <td style="width: 80px;padding-top:6px;">
                        <asp:Label ID="lblTyousa" runat="server" Text="調査結果"></asp:Label>
                    </td>
                    <td style="width: 130px;padding-top:3px;">
                        <asp:DropDownList ID="ddlResult" runat="server" Width="120px">
                        </asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="ddlResult" CssClass="errortext" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                        </div>
                    </td>
                    <td style="width: 290px;">
                        <uc:ClsCMTextBox ID="txtTyousaDtl" runat="server" ppHeight="60" ppIMEMode="半角_変更可" ppMaxLength="100" ppName="調査結果" ppNameVisible="false" ppTabIndex="2" ppTextMode="MultiLine" ppWidth="300" ppWrap="True" />
                    </td>
                    <td style="width: 30px;"></td>
                    <td style="width: 200px;">
                        <div style="text-align: right;padding-top:20px;">
                            <asp:Button ID="btnKanshi" runat="server" Text="監視報告書兼依頼票" />
                            <br />
                            <br />
                            <div style="width:82px; float:right;">
                            <asp:Button ID="btnUpdata" runat="server" Text="更新" />
                            </div>
                            <div style="width:30%; float:right;">
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" Text="クリア" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="center" style="width: 1000px; border-spacing: 6px;display:none;">
                <tr style="vertical-align: top;">
                    <td style="width:200px">
                        <uc:ClsCMDropDownList ID="ClsCMDropDownList1" runat="server" ppClassCD="0015" ppEnabled="True" ppName="担当者" ppNameVisible="True" ppNameWidth="80" ppRequiredField="True" ppValidationGroup="Detail" ppWidth="100" />
                    </td>
                    <td style="width:200px">
                        <uc:ClsCMDropDownList ID="ClsCMDropDownList2" runat="server" ppClassCD="0015" ppEnabled="True" ppName="調査結果" ppNameVisible="True" ppNameWidth="80" ppRequiredField="True" ppValidationGroup="Detail" ppWidth="100" />
                    </td>
                    <td style="width:350px">
                        <uc:ClsCMTextBox ID="ClsCMTextBox1" runat="server" ppHeight="60" ppIMEMode="半角_変更可" ppMaxLength="100" ppName="調査結果" ppNameVisible="false" ppTabIndex="2" ppTextMode="MultiLine" ppWidth="300" ppWrap="True" />
                    </td>
                    <td style="width:200px">
                        <div style="text-align: right;padding-top:20px;">
                            <asp:Button ID="Button1" runat="server" Text="監視報告書兼依頼票" />
                            <br />
                            <br />
                            <asp:Button ID="Button2" runat="server" Text="更新" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <div id="DivOut" class="grid-out" style="height: 500px;">
        <div class="grid-in" style="height: 500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" />
    </div>
</asp:Content>
