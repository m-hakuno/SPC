<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="MSTLSTP001.aspx.vb" Inherits="SPC.MSTLSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <td style="width: 30%">&nbsp;</td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="90" ppWidth="100" ppCheckHan="True" ppCheckLength="False" ppRequiredField="True" ppNum="True" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table>
                    <tr  class="float-left">
                        <td style="white-space: nowrap">
                            <asp:Label ID="lblHoleNm_1" runat="server" Text="ホール名　　　："></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblHoleNm_2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table>
                    <tr   class="float-left">
                        <td>
                            <asp:Label ID="lblOperationStatus_1" runat="server" Text="運用状態　　　："></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblOperationStatus_2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table>
                    <tr class="float-left">
                        <td>
                            <asp:Label ID="lblTboxType_1" runat="server" Text="ＴＢＯＸタイプ："></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTboxType_2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <table>
                    <tr class="float-left">
                        <td>
                            <asp:Label ID="lblHaishinResult" runat="server" Text="配信結果　　　："></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblHaishinResult_In" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out" style="height:750px;">
        <div class="grid-in" style="height:750px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
