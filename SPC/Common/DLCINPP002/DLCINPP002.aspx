<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DLCINPP002.aspx.vb" Inherits="SPC.DLCINPP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/EnableChange.js")%>'></script>
    <style type="text/css">
        .auto-style1 {
            height: 17px;
        }
        .auto-style2 {
            height: 44px;
        }
        .auto-style10 {
            width: 5%;
            height: 44px;
        }
        .auto-style11 {
            width: 255px;
        }
        .auto-style12 {
            height: 44px;
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="width: 20%">&nbsp;</td>
            <td>
                <table style="width:100%;" border="0">
                    <tr>
                        <td colspan="2" class="auto-style1">
                            <asp:Label ID="lblFileTypeSelection" runat="server" Text="ファイル種別選択"></asp:Label>
                        </td>
                        <td class="auto-style1"></td>
                    </tr>
                    <tr class="align-top">
                        <td class="auto-style10"></td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbMDNPic" runat="server" Text="ＭＤＮ－Ｈ設置写真" GroupName="FileType" />
                            <asp:Label ID="Label1" runat="server" Font-Size="Small" Text="   (最大3MB)"></asp:Label>
                        </td>
                         <td class="auto-style2">
                            <uc:ClsCMTextBox ID="txtPicTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="80" ppWidth="200" ppRequiredField="False" ppValidationGroup="1" ppCheckHan="False" ppCheckLength="False" ppIMEMode="半角_変更不可" ppTabIndex="6" />
                        </td>
                    </tr>
                    <tr>
                       <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="auto-style1">
                            <asp:Label ID="lblUploadFileSelection" runat="server" Text="アップロードファイル選択"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class ="align-top" border="0">
                    <tr>
                        <td style="width: 5%">&nbsp;</td>
                        <td>
                            <div style="position: relative;">
                                <asp:Panel ID="pnlData" runat="server">
                                    <asp:FileUpload ID="trfUplordFileSelection" runat="server" Width="500" BackColor="White" EnableTheming="True" /><br>
                                    <div style="white-space: nowrap" class="align-top">
                                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                            <asp:CustomValidator ID="valfileUpload" runat="server" ValidationGroup="1" CssClass="errortext" Display="Dynamic" ValidateEmptyText="True"></asp:CustomValidator>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                                <%--<div style="position: absolute; top: 0px; left: 0px; background-color:#CCCCCC; padding-top:3px;">
                                    <asp:Label ID="Label1" runat="server" Text="C:\Users\SanDvltPc53\Desktop\UPLOADテスト用ふぁいる\掃除当番表(150401).pdf" Width="400" Height="20"></asp:Label>
                                </div>--%>
                                
                            </div>

                        </td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>         
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
    </div>
</asp:Content>
