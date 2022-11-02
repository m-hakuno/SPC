<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="SCLINPP001.aspx.vb" Inherits="SPC.SCLINPP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">

    <script type="text/javascript">

        //convertEnterToTab(event)を設定
        function pageLoad() {
            set_onloadenter();
        }

    </script>

    <style>
        .coverTextBox {
            position: absolute;
            top: 5px;
            left: 3px;
        }

        .btnChange {
            text-align: left;
            background-color: #CCCCCC;
            /*border:none;*/
            /*border:1px solid black;*/
        }
    </style>

    <%--<asp:UpdatePanel ID="updpnImport" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>--%>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table class="center" style="width: 700px;" border="0">
                <tr>
                    <td style="width: 103px;"></td>
                    <td style="width: 180px;"></td>
                    <td style="width: 13px;"></td>
                    <td style="width: 100px;"></td>
                    <td style="width: 310px;"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc:ClsCMDropDownList runat="server" ID="ddlImportCls" ppClassCD="0157" ppValidationGroup="1" ppName="取込区分" ppNameWidth="107" ppWidth="140" ppNotSelect="True" />
                    </td>
                </tr>
                <%--        <tr>
            <td colspan="2">
                <uc:ClsCMDropDownList runat="server" ID="ddlDateCls" ppClassCD="0155" ppValidationGroup="1" ppName="日時" ppNameWidth="100" ppWidth="100" ppNotSelect="True" />
            </td>
        </tr>--%>
                <tr>
                    <td>
                        <asp:Button ID="btnTbxArea" runat="server" Text="TBOXID" Width="80px" CssClass="btnChange" TabIndex="99" />
                    </td>
                    <td colspan="4">
                        <uc:ClsCMTextBox ID="txtTBOXID" runat="server" ppName="TBOXID" ppNameWidth="0" ppWidth="70"
                            ppIMEMode="半角_変更不可" ppMaxLength="8" ppCheckHan="True" ppRequiredField="False" ppCheckLength="True" ppNum="True" ppNameVisible="False" />
                        <uc:ClsCMDropDownList runat="server" ID="ddlArea" ppClassCD="0157" ppValidationGroup="1" ppName="取込区分" ppNameWidth="0" ppWidth="140" ppNotSelect="True" ppNameVisible="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc:ClsCMDateTimeBox runat="server" ID="txtDate_From" ppNameVisible="True" ppName="到着日時" ppRequiredField="True" ppNameWidth="107px" />
                    </td>
                    <td>～</td>
                    <td colspan="2">
                        <uc:ClsCMDateTimeBox runat="server" ID="txtDate_To" ppNameVisible="False" ppName="日時To" ppRequiredField="False" ppNameWidth="0px" />
                    </td>
                </tr>
                <tr>
                    <td style="padding: 5px 0px 5px 5px;">CSVファイル
                    </td>
                    <td colspan="4" style="position: relative; padding: 5px 0px 5px 8px;">
                        <asp:FileUpload ID="trfUplordFileSelection" runat="server" Width="500px" BackColor="White" />
                        <asp:TextBox ID="txtCvr" runat="server" CssClass="coverTextBox" ReadOnly="True" TabIndex="99" Width="423px"></asp:TextBox>
                        <br />
                        <asp:CustomValidator ID="valfileUpload" runat="server" CssClass="errortext" Display="Dynamic" ValidateEmptyText="True"></asp:CustomValidator>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlImportCls" />
        </Triggers>
    </asp:UpdatePanel>



    <%--<asp:ValidationSummary ID="vasImport" runat="server" CssClass="errortext" ValidationGroup="Import" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <div class="grid-out" style="width:1254px; margin-left:auto;margin-right:auto;">
        <div class="grid-in"  style="height:430px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
