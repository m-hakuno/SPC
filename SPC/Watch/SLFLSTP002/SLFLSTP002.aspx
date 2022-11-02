<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SLFLSTP002.aspx.vb" Inherits="SPC.SLFLSTP002" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/ctiexe.js")%>'></script>
    <script type="text/javascript">
        function lenCheck(obj, size) {
            var strW = obj.value;
            var lenW = strW.length;
            var num

            num = obj.value.match(/\n|\r\n/g);
            if (num != null) {
                gyosuu = num.length;
            } else {
                gyosuu = 0;
            }

            if ((parseInt(size) + parseInt(gyosuu)) < lenW) {
                var limitS = strW.substring(0, (parseInt(size) + parseInt(gyosuu)));
                obj.value = limitS;
            }
            if (strW.match(/\d{2}\/\d{2}\/\d{2}\s報告済み\s/) != null) {
            } else {
                if (lenW > 86) {
                    alert("86文字以内で入力してください");
                }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br />
    <asp:Panel ID="Panel1" runat="server">      
         <table class ="center">
            <tr>
                <td>
                    <asp:Panel ID="pnlUpdate" runat="server" Width="1080px">
                        <table class ="center">
                            <tr>
                                <td>
                                    <asp:Label ID="lblDateN" runat="server" Text="照会日時"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDateV" runat="server" Text="YYYY/MM/DD HH:MM:SS"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:panel>
    <br />
    <asp:Panel ID="pnltable" runat="server">      
         <table class ="center">
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" Width="1080px">
                        <table border ="1" style="width:100%;empty-cells:hide;border-collapse:collapse;border-spacing:3px;" class="text-center">
                            <tr>
                                <td style="width: 95px">
                                    <asp:Label ID="lblSeqN" runat="server" Text="連番"></asp:Label>
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="lblEWN" runat="server" Text="東西区分"></asp:Label>
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="lblTboxidN" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
                                </td>
                                <td style="width: 150px">
                                    <asp:Label ID="lblJBNumN" runat="server" Text="ＪＢ番号"></asp:Label>
                                </td>
                                <td style="width: 450px" colspan="2" >
                                    <asp:Label ID="lblHallnmN" runat="server" Text="ホール名"></asp:Label>
                                </td>
                                <td rowspan="5" style="width: 155px">
                                    <asp:Button ID="btnWatch" runat="server" Text=" 監視報告兼依頼票 " />
                                    <br /><br />
                                    <asp:Button ID="btnUpdate" runat="server" Text="　　　更　新　　　" />
                                    <br /><br />
                                    <asp:Button ID="btnHist" runat="server" Text=" 過去１年履歴表示 " />
                                    <br /><br />
                                    <asp:Button ID="btnPrtHist" runat="server" Text="過去１ヶ月履歴表示" />
                                </td>
                            </tr>
                            <tr style="height:40px;">
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblSeqV" runat="server" Text="&nbsp;"></asp:Label> 
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblEWV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblTboxidV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblJBNumV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="width: 450px;background-color:#F0F0F0;" colspan="2" >
                                    <asp:Label ID="lblHallnmV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label5" runat="server" Text="自走開始"></asp:Label>
                                </td>
                                <td rowspan="2">
                                    <asp:Label ID="lblBB1serialNoN" runat="server" Text="拡張ＢＢ１シリアル"></asp:Label>
                                </td>
                                <td rowspan="2">
                                    <asp:Label ID="Label7" runat="server" Text="自走開始"></asp:Label>
                                    <br />
                                    <asp:Label ID="Label8" runat="server" Text="ＢＢ機種コード"></asp:Label>
                                </td>
                                <td rowspan="2">
                                    <asp:Label ID="Label9" runat="server" Text="対応結果"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLogunyouDTN" runat="server" Text="運用日"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLoghasseiDTN" runat="server" Text="発生日"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLoghasseiTimeN" runat="server" Text="発生時刻"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblLogunyouDTV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblLoghasseiDTV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblLoghasseiTimeV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblBB1serialNoV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;">
                                    <asp:Label ID="lblSoftBBClassV" runat="server" Text="&nbsp;"></asp:Label>
                                </td>
                                <td style="background-color:#F0F0F0;text-align:left;">
                                    <div>&nbsp;<asp:DropDownList ID="ddlDealDtl1Cd" runat="server"></asp:DropDownList></div>
                                    <uc:ClsCMTextBox runat="server" ID="txtDealDtl2" ppName="対応結果" ppNameVisible="False" ppNameWidth="3" ppTextMode="MultiLine" ppMaxLength="100" ppWidth="350"  ppWrap="True" onchange="countlength" />
                                    <asp:CheckBox ID="chkExclusion" runat="server" Text="対応結果を引継ぎ" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblWrncontentN" runat="server" Text="注意情報"></asp:Label>
                                </td>
                                <td colspan="6" style="background-color:#F0F0F0;text-align:left;">
                                    <div class="float-left">
                                        <asp:Label ID="lblWrncontentV" runat="server" Text="&nbsp;" ></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:panel>

    <br />

    <div id="DivOut" runat="server" class="grid-out" style="height:500px;">
        <div class="grid-in" style="height:500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>

</asp:Content>
