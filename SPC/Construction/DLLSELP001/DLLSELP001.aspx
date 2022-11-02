<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="DLLSELP001.aspx.vb" Inherits="SPC.DLLSELP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server"  defaultfocus="txtTboxID">
        <table style="width: 100%;">
            <tr>
                <td>
                    <table  class="center">
                        <tr>
                            <td>
                                <asp:Label ID="lblToroku" runat="server" Text="登録"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="center">
                        <tr>
                            <td  Class="align-top">
                                <asp:Panel ID="pnlTboxID" runat="server">
                                    <uc:ClsCMTextBox runat="server" ID="txtTboxID" ppCheckHan="True" ppCheckLength="True" ppIMEMode="半角_変更可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppRequiredField="True" ppValidationGroup="1" />
                                </asp:Panel>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Panel ID="pnlFileName" runat="server">
                                    <asp:Label ID="lblFileName" runat="server" Text="ファイル名" Width="70"></asp:Label>
                                </asp:Panel>
                            </td>
                            <td Class="align-top">
                                <asp:Panel ID="pnlData" runat="server"  CssClass="align-top">
                                    <asp:DropDownList ID="ddlList" runat="server" Width="200">
                                    </asp:DropDownList>
                                    <div style="white-space: nowrap">
                                        <asp:Panel ID="pnlval" runat="server" Width="0px">
                                            <asp:CustomValidator ID="valddl" runat="server" ControlToValidate="ddlList" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="center">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlBtnHaishin" runat="server" CssClass="center">
                                    <asp:Button ID="btnHaishin" runat="server" Text="配信" />
                                </asp:Panel>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Panel ID="pnlBtnClear" runat="server" CssClass="center">
                                    <asp:Button ID="btnClear" runat="server" Text="クリア" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <div class="float-left">
                            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="errortext" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table  class="center">
                        <tr>
                            <td>
                                <asp:Label ID="lblKosei" runat="server" Text="構成情報"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                     <table  class="center">
                        <tr>
                            <td>
                                <div class="grid-out" style="width:750px;">
                                    <div class="grid-in">
                                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                                        <asp:GridView ID="grdList_Kousei" runat="server"></asp:GridView>
                                    </div>
                                </div>
                            </td>
                        </tr>        
                    </table>
                </td>
            </tr>
        </table>
        <hr />
        <table style="width: 100%;">
            <tr>
                <td>
                    <table  class="center">
                        <tr>
                            <td>
                                <asp:Label ID="lblList" runat="server" Text="一覧"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table  class="center">
                        <tr>
                            <td>
                                <asp:Label ID="lblSerch" runat="server" Text="検索条件"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="center">
                        <tr>
                            <td  class="align-top">
                                <asp:Panel ID="pnlTboxIDSerch" runat="server">
                                    <uc:ClsCMTextBox runat="server" ID="txtTboxIDSerch" ppCheckHan="True" ppCheckLength="True" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppIMEMode="半角_変更不可" />
                                </asp:Panel>
                            </td>
                            <td>&nbsp;</td>
                            <td  class="align-top">
                               <asp:Panel ID="pnlHaishinFromTo" runat="server" CssClass="align-top">
                                   <uc:ClsCMDateBoxFromTo runat="server" ID="txtHaishinFromTo"　ppCheckHan="True" ppMaxLength="8" ppName="配信日"  />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div>
        <table class="center" style="width:950px;">
            <tr>
                <td>
                    <div class="grid-out" style="width:100%;">
                        <div class="grid-in" style="width:100%;">
                            <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="grvList" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
