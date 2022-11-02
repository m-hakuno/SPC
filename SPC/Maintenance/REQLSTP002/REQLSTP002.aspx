<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="REQLSTP002.aspx.vb" Inherits="SPC.REQLSTP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <Script src='<%= Me.ResolveClientUrl("~/Scripts/setCheck.js")%>'></Script>
    <table class="center">
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtTboxID" ppName="ＴＢＯＸＩＤ" ppNum="True" ppCheckHan="True" ppMaxLength="8" ppCheckLength="True" ppRequiredField="True" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid">
        <table class="center">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHallNameN" runat="server" Text="ホール名" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHallNameV" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTboxClassCDN" runat="server" Text="システム種別" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxClassCDV" runat="server" Text="" Width="400"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVersionN" runat="server" Text="ＶＥＲ" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblVersionV" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMntN" runat="server" Text="保守拠点" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntCDV" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntNmV" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUnfN" runat="server" Text="統括拠点" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblUnfCDV" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblUnfNmV" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr class="align-top">
                            <td>
                                <uc:ClsCMTextBoxRef runat="server" ID="tsrStrageCD" ppNameWidth="100" ppName="別拠点" ppIMEMode="半角_変更不可" ppMaxLength="3" ppURL="~/Common/COMSELP002/COMSELP002.aspx" ppValidationGroup="Change" ppCheckHan="True" ppRequiredField="False" />
                            </td>
                            <td style="padding-top: 7px">
                                <asp:Label ID="lblStrageNmV" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="float-left">
                        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="Change" />
                    </div>
                    <div class="float-right">
                        <asp:Button ID="btnChange" runat="server" Text="変更" ValidationGroup="Change" />
                    </div>
                </td>
            </tr>
        </table>

        <br />

    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div style="display:none">
        <asp:GridView ID="grvSetList" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="拠点コード" HeaderText="拠点コード" />
                <asp:BoundField DataField="機器種別コード" HeaderText="機器種別コード" />
                <asp:BoundField DataField="ＨＤＤＮｏ" HeaderText="ＨＤＤＮｏ" />
                <asp:BoundField DataField="ＨＤＤ種別" HeaderText="ＨＤＤ種別" />
                <asp:BoundField DataField="名称" HeaderText="名称" />
                <asp:BoundField DataField="在庫数" HeaderText="在庫数" />
                <asp:BoundField DataField="持参数" HeaderText="持参数" />
                <asp:BoundField DataField="プリンタ区分" HeaderText="プリンタ区分" />
            </Columns>
        </asp:GridView>
    </div>
    <div id="DivOut" runat="server" class="grid-out">
        <div id="DivIn" runat="server" class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <table class="float-right">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="拠点"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlStrage" runat="server"  Width="70px"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="機器型式"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlModel" runat="server" Width="270px"></asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnSelect" runat="server" Text="選択" Enabled="False" CausesValidation="False" />
            </td>
            <td>
                <asp:Button ID="btnPrint" runat="server" Text="印刷" Enabled="False" CausesValidation="False" />
            </td>
        </tr>
    </table>
    </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
