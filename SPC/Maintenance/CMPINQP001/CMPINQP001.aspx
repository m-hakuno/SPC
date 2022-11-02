<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CMPINQP001.aspx.vb" Inherits="SPC.CMPINQP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1036px;" class="center" border="0">
        <tr>
            <td>
                <!--ＴＢＯＸＩＤ-->
                <uc:ClsCMTextBoxFromTo ID="txtTboxId" runat="server" ppMaxLength="8" ppName="ＴＢＯＸＩＤ"
                    ppNameWidth="90" ppWidth="100" ppIMEMode="半角_変更不可" ppCheckHan="true" />
            </td>
        </tr>
        <tr>
            <td>
                <!--対応日-->
                <uc:ClsCMDateBoxFromTo ID="dtbSupportDt" runat="server" ppName="対応日" ppNameWidth="90" />
            </td>
        </tr>
        <tr>
            <td>
                <!--開始-->
                <uc:ClsCMTimeBoxFromTo ID="tmbStartTm" runat="server" ppName="開始" ppNameWidth="90" />
            </td>
        </tr>
        <tr>
            <td>
                <!--依頼（元は検収）-->
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblInsApp" runat="server" Text="依頼" Width="90"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbtInsApp" runat="server" RepeatColumns="2" Width="150">
                                <asp:ListItem>承認</asp:ListItem>
                                <asp:ListItem Selected="True">未承認</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <!--検収（元は請求）-->
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblReqApp" runat="server" Text="検収" Width="90"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbtReqApp" runat="server" RepeatColumns="2" Width="150">
                                <asp:ListItem>承認</asp:ListItem>
                                <asp:ListItem Selected="True">未承認</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <!--取消-->
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblCancel" runat="server" Text="取消含む" Width="90"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxCancel" runat="server" />
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
    <!--グリッド-->
    <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data"/>
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <table style="width:100%;">
        <tr>
            <td>
                <!--担当者名-->
                <uc:ClsCMTextBox ID="txtChargeNm" runat="server" ppName="担当者名" ppWidth="320" ppMaxLength="30"
                    ppRequiredField="true" ppValidationGroup="Approval" ppIMEMode="全角" />
            </td>
            <td class="float-right" style="padding-right: 10px">
                <!--合計金額-->
                <asp:Label ID="lblTotalAmount" runat="server" Text="合計金額"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="valSumApproval" runat="server" CssClass="errortext" ValidationGroup="Approval" />
    <asp:HiddenField ID="param" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
