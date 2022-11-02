<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DSUUPDP001.aspx.vb" Inherits="SPC.DSUUPDP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="pnlDsuRequestForm" runat="server">
        <table border="0">
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="140" ppWidth="80" ppIMEMode="半角_変更不可" ppRequiredField="True" ppCheckHan="True" ppCheckLength="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width:100%; height: 84px;" border="0">
                        <tr>
                            <td style="width: 143px">
                                <asp:Label ID="lblGcReportNo1" runat="server" Text="ＧＣ報告ＮＯ"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblGcReportNo2" runat="server" Text="XXXXXXXXXXXXXXX"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblNlCls1" runat="server" Text="ＮＬ区分"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNlCls2" runat="server" Text="X:XXXXXXXXXX"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblHallNm1" runat="server" Text="ホール名"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHallNm2" runat="server" Text="XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtTorableContent" runat="server" ppMaxLength="100" ppName="障害内容" ppNameWidth="140" ppWidth="670" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width:100%; height: 84px;" border="0">
                        <tr>
                            <td style="width: 143px">
                                <asp:Label ID="lblHZipNo1" runat="server" Text="郵便番号"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHZipNo2" runat="server" Text="999-9999"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblHAddr1" runat="server" Text="住所"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblHAddr2" runat="server" Text="XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTboxTelNo1" runat="server" Text="ＴＢＯＸ呼出番号"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTboxTelNo2" runat="server" Text="XXXXXXXXXXXXXXX"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table border="0">
            <tr>
                <td>
                    <asp:Label ID="lblDetails0" runat="server" Text="《詳細内容》"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtLineCnd" runat="server" ppMaxLength="100" ppName="回線状態" ppNameWidth="140" ppWidth="670" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtCause" runat="server" ppMaxLength="100" ppName="原因" ppNameWidth="140" ppWidth="670" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtRspnsContent" runat="server" ppMaxLength="100" ppName="対応処置内容" ppNameWidth="140" ppWidth="670" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%; height: 28px;" border="0">
                        <tr>
                            <td style="width: 140px">
                                <asp:Label ID="lblStatus" runat="server" Text="進捗状況"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="110px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table border="0">
            <tr>
                <td>
                    <asp:Label ID="lblPurchaseAddr0" runat="server" Text="《ＤＳＵ購入先住所》"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMDateBox ID="dttRspnsD" runat="server" ppName="対応日" ppNameWidth="140" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNttBranch" runat="server" ppMaxLength="50" ppName="対応ＮＴＴ支社" ppNameWidth="140" ppWidth="670" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNZipNo" runat="server" ppMaxLength="8" ppName="ＮＴＴ郵便番号" ppNameWidth="140" ppWidth="80" ppIMEMode="半角_変更不可" ppCheckHan="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNAddr" runat="server" ppMaxLength="100" ppName="ＮＴＴ住所" ppNameWidth="140" ppWidth="670" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNTelNo" runat="server" ppMaxLength="15" ppName="ＮＴＴ電話番号" ppNameWidth="140" ppWidth="120" ppIMEMode="半角_変更不可" ppCheckHan="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNFaxNo" runat="server" ppMaxLength="15" ppName="ＮＴＴＦＡＸ番号" ppNameWidth="140" ppWidth="120" ppIMEMode="半角_変更不可" ppCheckHan="True" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNCharge" runat="server" ppMaxLength="20" ppName="ＮＴＴ担当者" ppNameWidth="140" ppWidth="280" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtNChargeTelNo" runat="server" ppMaxLength="15" ppName="ＮＴＴ担当者ＴＥＬ" ppNameWidth="140" ppWidth="120" ppIMEMode="半角_変更不可" ppCheckHan="True" />
                </td>
            </tr>
            <tr>
                <td>

                    <uc:ClsCMTextBox ID="txtNoteText" runat="server" ppMaxLength="100" ppName="備考" ppNameWidth="140" ppWidth="670" />

                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" />
    </asp:Panel>
</asp:Content>
