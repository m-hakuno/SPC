﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClsCMDateBox.ascx.vb" Inherits="SPC.ClsCMDateBox" %>

<asp:Panel ID="pnlCtrl" runat="server" Wrap="False">
    <table border="0">
        <tr style="vertical-align: top">
            <td style="position: relative; top: 3px">
                <asp:Panel ID="pnlName" runat="server">
                    <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="pnlData" runat="server">
                    <span style="vertical-align: top">
                        <asp:TextBox ID="txtDateBox" runat="server" MaxLength="10" CssClass="ime-disabled" Width="67px"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txtDateBox_CalendarExtender" runat="server" Format="yyyy/MM/dd" PopupButtonID="ibtDate" TargetControlID="txtDateBox" DaysModeTitleFormat="yyyy年 MMMM" TodaysDateFormat="yyyy年 MMMM dd日" StartDate="1753/01/01" EndDate="9999/12/31"></ajaxToolkit:CalendarExtender>
                        <asp:Label ID="lblYobi" runat="server" Visible="False" Width="22px"></asp:Label>
                    </span>
                    <asp:ImageButton ID="ibtDate" runat="server" ImageUrl="~/Images/アイコン（カレンダー）.png" />
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvDateBox" runat="server" ControlToValidate="txtDateBox" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>