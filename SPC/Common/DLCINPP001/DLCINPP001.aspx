<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DLCINPP001.aspx.vb" Inherits="SPC.DLCINPP001" MaintainScrollPositionOnPostback="true" %>
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
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbAcceptanceCertificate" runat="server" Text="工事完了報告書兼検収書" Checked="True" GroupName="FileType" />
                        </td>                     
                        <td>
                            <uc:ClsCMTextBoxTwo ID="txtWorkRequestNo" runat="server" ppIMEModeOne="半角_変更不可" ppMaxLengthOne="5" ppMaxLengthTwo="8" ppName="工事依頼番号" ppNameWidth="80" ppWidthOne="70" ppWidthTwo="100" ppRequiredField="False" ppCheckHanOne="False" ppCheckHanTwo="False" ppCheckLengthOne="True" ppCheckLengthTwo="True" ppValidationGroup="1" ppIMEModeTwo="半角_変更不可" ppTabIndex="1" ppNumOne="False" ppNumTwo="True" />
                        </td>
                    </tr>
                    <tr class="align-top">
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbDllDistributionDt" runat="server" Text="ＤＬＬ配信データ" GroupName="FileType" />
                        </td>
                    </tr>
                    <tr class="align-top">
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbTomasHoleInfo" runat="server" Text="ＴＯＭＡＳホール情報" GroupName="FileType" />
                        </td>
                     </tr>
                    <tr class="align-top">
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbWorkReport" runat="server" Text="保守完了報告書" GroupName="FileType" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRequestNo" runat="server" ppMaxLength="12" ppName="依頼番号" ppNameWidth="80" ppWidth="200" ppRequiredField="False" ppValidationGroup="1" ppCheckHan="False" ppCheckLength="False" ppIMEMode="半角_変更不可" ppTabIndex="2" />
                        </td>
                    </tr>
                    <tr class="align-top">
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbANOBallsSettingInfo" runat="server" Text="貸玉数設定情報" GroupName="FileType"  />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="80" ppWidth="200" ppRequiredField="False" ppValidationGroup="1" ppCheckHan="False" ppCheckLength="False" ppIMEMode="半角_変更不可" ppTabIndex="3" />
                        </td>
                    </tr>
                    <tr class="align-top">
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbBbInvestigativeReport" runat="server" Text="ＢＢ調査報告書" GroupName="FileType" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtControlNo" runat="server" ppMaxLength="8" ppName="管理番号" ppNameWidth="80" ppWidth="200" ppRequiredField="False" ppValidationGroup="1" ppCheckHan="False" ppCheckLength="False" ppIMEMode="半角_変更不可" ppTabIndex="4" />
                        </td>
                    </tr>
                    <tr class="align-top">
                        <td style="width: 5%">&nbsp;</td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbConstitutionFile" runat="server" Text="ＢＢ構成ファイル" GroupName="FileType" />
                        </td>
                         <td>
                            <uc:ClsCMTextBox ID="txtBBTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="80" ppWidth="200" ppRequiredField="False" ppValidationGroup="1" ppCheckHan="False" ppCheckLength="False" ppIMEMode="半角_変更不可" ppTabIndex="5" />
                        </td>
                    </tr>
                    <tr class="align-top">
                        <td class="auto-style10"></td>
                        <td class="auto-style11">
                            <asp:RadioButton ID="rdbGenbaPic" runat="server" Text="設置環境写真" GroupName="FileType" />
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
