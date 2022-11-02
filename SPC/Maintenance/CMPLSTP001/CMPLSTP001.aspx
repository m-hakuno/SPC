<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference_Reverse.master" CodeBehind="CMPLSTP001.aspx.vb" Inherits="SPC.CMPLSTP001" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/Reference_Reverse.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <script type="text/javascript">
        //オンロードイベントでスクロール位置を最上部に設定する
        onload = pageonload

        function pageonload() {
            //ヒドゥンの取得
            //スクロール位置調整フラグ
            var element = document.getElementById("cphMainContent_cphSearchContent_hdnScrollTop");
            if (element.value == 0) {
                window.scroll(0, 0);
            }
        }

    </script>
    <asp:HiddenField ID="hdnScrollTop" runat="server" Value="0" />
    <table style="width: 1050px;" class="center" border="0">
        <tr>
            <td>
                <table border="0" style="width: 100%">
                    <tr>
                        <!--ＴＢＯＸＩＤ-->
                        <td>
                            <uc:ClsCMTextBoxFromTo runat="server" ID="txtTboxid" ppName="TBOXID"
                                ppNameWidth="100" ppMaxLength="8" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="80" ppFontSize="12pt" ppNum="False" />
                        </td>
                        <!--保守管理番号-->
                        <td>
                            <uc:ClsCMTextBoxFromTo runat="server" ID="txtMentNo" ppName="保守管理番号"
                                ppNameWidth="100" ppMaxLength="12" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="130" ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <!--受付日-->
                        <td>
                            <uc:ClsCMDateBoxFromTo runat="server" ID="dtbRcptDt" ppName="受付日"
                                ppNameWidth="100" ppDateFormat="年月日" ppFontSize="12pt" />
                        </td>
                        <!--対応日-->
                        <td>
                            <uc:ClsCMDateBoxFromTo runat="server" ID="dtbRspnsDt" ppName="作業予定日"
                                ppNameWidth="100" ppDateFormat="年月日" ppFontSize="12pt" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" style="width: 100%">
                    <tr>
                        <!--ＮＬ区分-->
                        <td style="width: 250px;">
                            <uc:ClsCMTextBox runat="server" ID="txtNLCls" ppName="NL区分"
                                ppNameWidth="100" ppMaxLength="1" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="15" ppFontSize="12pt" ppExpression="[n|N|l|L|j|J]" />
                        </td>
                        <!--ＥＷ区分-->
                        <td style="width: 243px;">
                            <uc:ClsCMTextBox runat="server" ID="txtEWCls" ppName="EW区分"
                                ppNameWidth="55" ppMaxLength="1" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="15" ppFontSize="12pt" ppExpression="[e|E|w|W]" />
                        </td>
                        <!--保担営業所-->
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <uc:ClsCMTextBox runat="server" ID="txtMentBranch" ppName="保担営業所"
                                            ppNameWidth="100" ppMaxLength="4" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="35" ppFontSize="12pt" ppNum="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBranch" runat="server" Text="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 2px;">
                <table border="0" style="width: 100%">
                    <tr>
                        <!--都道府県-->
                        <td style="width: 602px">
                            <span style="position: relative;">
                                <asp:Label ID="LabelState" Text="都道府県　" runat="server" Width="98px" Style="font-size: 12pt;"></asp:Label><span style="position: relative; left: 8px;"><asp:DropDownList runat="server" ID="ddlPrefectureFm" Width="120px" Style="font-size: 12pt;" /><label> ～ </label>
                                    <asp:DropDownList runat="server" ID="ddlPrefectureTo" Width="120px" Style="font-size: 12pt;" /></span></span><br />
                            <span style="position: relative; top: 2px;">
                                <asp:CustomValidator runat="server" ID="cuvPrefecture" ControlToValidate="ddlPrefectureFm" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" /></span>
                        </td>

                        <!--引継ぎ区分-->
                        <td>
                            <asp:CheckBox ID="cbxSalesTrbl" runat="server" Text="営業支障" Style="font-size: 12pt;" />
                            <asp:CheckBox ID="cbxSecondTrbl" runat="server" Text="二次支障" Style="font-size: 12pt;" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table class="center" style="width: 100%; text-align: left">
                    <tr>
                        <td style="width: 94px; vertical-align: top; padding-top: 12px;">
                            <asp:Label ID="lblTboxClass" Style="font-size: 12pt; margin-left: 2px" runat="server" Text="TBOXﾀｲﾌﾟ" />
                        </td>
                        <td>
                            <div style="width: 100%">
                                <asp:CheckBoxList ID="cklTboxClass" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table"
                                    RepeatDirection="Horizontal" RepeatColumns="8" Font-Size="Medium" />
                            </div>
                        </td>
                    </tr>
                </table>
                <%--<table border="0" style="width: 100%">
                    <!--ＴＢＯＸタイプ-->
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="TBOXﾀｲﾌﾟ" Style="font-size: 12pt;"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType01" runat="server" Text="T500" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType02" runat="server" Text="T555" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType03" runat="server" Text="T700" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType04" runat="server" Text="T70M" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType05" runat="server" Text="T70R" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType06" runat="server" Text="T750" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType07" runat="server" Text="T75R" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType08" runat="server" Text="T780" Style="font-size: 12pt;" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType09" runat="server" Text="IT100" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType10" runat="server" Text="IT130" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType11" runat="server" Text="IT130S" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType12" runat="server" Text="IT135S" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType13" runat="server" Text="NVC100" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType14" runat="server" Text="NVC100S" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType15" runat="server" Text="NVC130" Style="font-size: 12pt;" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxTboxType16" runat="server" Text="NVC300" Style="font-size: 12pt;" />
                        </td>
                    </tr>
                </table>--%>
            </td>
        </tr>
        <tr>
            <!--ＶＥＲ-->
            <td style="padding-left: 4px;">
                <uc:ClsCMTextBox runat="server" ID="txtVersion" ppName="VER"
                    ppNameWidth="99" ppMaxLength="5" ppIMEMode="半角_変更不可" ppCheckHan="True" ppWidth="40" ppFontSize="12pt" />
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" style="width: 100%">
                    <tr>
                        <!--回復内容-->
                        <td style="width: 498px;">
                            <uc:ClsCMTextBox runat="server" ID="txtRpr" ppName="回復内容"
                                ppNameWidth="100" ppMaxLength="50" ppWidth="296" ppIMEMode="全角" ppFontSize="12pt" />
                        </td>
                        <!--回復内容詳細-->
                        <td>
                            <uc:ClsCMTextBox runat="server" ID="txtRprCntnt" ppName="回復内容詳細"
                                ppNameWidth="100" ppMaxLength="50" ppWidth="296" ppIMEMode="全角" ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <!--故障機器1-->
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlTrblAppa1" ppClassCD="0094" ppName="故障機器1"
                                ppNameWidth="100" ppNotSelect="true" ppWidth="300" ppFontSize="12pt" />
                        </td>
                        <!--故障機器2-->
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlTrblAppa2" ppClassCD="0094" ppName="故障機器2"
                                ppNameWidth="100" ppNotSelect="true" ppWidth="300" ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <!--故障機器3-->
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlTrblAppa3" ppClassCD="0094" ppName="故障機器3"
                                ppNameWidth="100" ppNotSelect="true" ppWidth="300" ppFontSize="12pt" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td>
                <table border="0" style="width: 100%">
                    <tr>
                        <!--申告内容-->
                        <td style="width: 498px;">
                            <uc:ClsCMTextBox runat="server" ID="txtRpt" ppName="申告内容"
                                ppNameWidth="100" ppMaxLength="100" ppWidth="296" ppIMEMode="全角" ppFontSize="12pt" />
                        </td>
                        <!--申告内容詳細-->
                        <td>
                            <uc:ClsCMTextBox runat="server" ID="txtRptCntnt" ppName="申告内容詳細"
                                ppNameWidth="100" ppMaxLength="100" ppWidth="296" ppIMEMode="全角" ppFontSize="12pt" />
                        </td>
                    </tr>
                    <tr>
                        <!--対応内容-->
                        <td style="margin-left: 80px" colspan="2">
                            <uc:ClsCMTextBox runat="server" ID="txtRspnsCntnt" ppName="対応内容"
                                ppNameWidth="100" ppMaxLength="200" ppWidth="296" ppIMEMode="全角" ppFontSize="12pt" />
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
    <div id="divCount" runat="server" class="float-Left">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" Style="font-size: 12pt;"></asp:Label>
                </td>
                <td style="width: 80px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX" Style="font-size: 12pt;"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" Style="font-size: 12pt;"></asp:Label>
                </td>
                <td style="width: 15px"></td>
                <td>
                    <asp:Button ID="btnReload" runat="server" CssClass="center" Text="リロード" />
                </td>
        </table>
    </div>
    <!--グリッド-->
    <div class="grid-out" style="height: 649px">
        <div class="grid-in" style="height: 649px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
