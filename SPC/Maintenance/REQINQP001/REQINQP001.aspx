<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="REQINQP001.aspx.vb" Inherits="SPC.REQINQP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <br>
                    <table border="0" style="width:100%" >
                        <tr>
                            <!--管理番号-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="管理番号" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--特別保守-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="特別保守" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSpecialMnt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＮＬ区分-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="ＮＬ区分" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNlCls" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--ＴＢＯＸＩＤ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="ＴＢＯＸＩＤ" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxid" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＴＢＯＸタイプ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="ＴＢＯＸタイプ" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxType" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＶＥＲ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="ＶＥＲ" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxVer" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--ホール名-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="ホール名" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHallNm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＥＷ区分-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="ＥＷ区分" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEwCls" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--住所（上段）-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="　　　住所" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAddr1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--住所（下段）-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAddr2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--保担名-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="保担名" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntNm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--ＴＥＬ-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label22" runat="server" Text="　　　ＴＥＬ" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTelno" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--総括保担名-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label24" runat="server" Text="総括保担名" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblUnfNm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--申告日-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label26" runat="server" Text="申告日" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRptDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--申告元-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label28" runat="server" Text="申告元" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRptBase" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--受付日時-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label30" runat="server" Text="受付日時" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRcptDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--依頼日時-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label32" runat="server" Text="依頼日時" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReqDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--申告内容（コードの内容）-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label34" runat="server" Text="申告内容" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRptcdDtl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--申告内容１-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label36" runat="server" Text="" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRptDtl1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--申告内容２-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label38" runat="server" Text="" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRptDtl2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--空行-->
                            <td>
                                <br>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--作業予定日時-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label40" runat="server" Text="作業予定日時" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblWrkDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--開始日時-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label42" runat="server" Text="開始日時" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStartDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--終了日時-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label44" runat="server" Text="終了日時" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEndDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--作業状況-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label46" runat="server" Text="作業状況" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="作業日時" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStNotext" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                            <!--故障機器-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label48" runat="server" Text="故障機器" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAppaCd" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--回復内容（コードの内容）-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label50" runat="server" Text="回復内容" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRepairCdCntnt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--回復内容-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label52" runat="server" Text="" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRepairCntnt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--備考・連絡１-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label54" runat="server" Text="備考・連絡" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNotetext1" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--備考・連絡２-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label56" runat="server" Text="" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNotetext2" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <br>
                </asp:Panel>

                <!--グリッド-->
                <br>
                <div id="DivOut" runat="server" class="grid-out" style="height: 420px">
                    <div id="DivIn" runat="server" class="grid-in" style="height: 420px">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
                <br>

                <asp:Panel ID="Panel2" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <br>
                    <table border="0" style="width:100%" >
                        <tr>
                            <!--特別保守フラグ-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label58" runat="server" Text="特別保守フラグ" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntFlg" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--依頼承認-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label60" runat="server" Text="依頼承認" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInsApp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--検収承認-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label62" runat="server" Text="検収承認" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReqApp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--出発時間-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label64" runat="server" Text="出発時間" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDeptTm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--開始時間-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label66" runat="server" Text="開始時間" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblStartTm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--終了時間-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label68" runat="server" Text="終了時間" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEndTm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--作業内容-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label70" runat="server" Text="作業内容" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblWrkCntnt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--特別保守作業時間-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label72" runat="server" Text="特別保守作業時間" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntTm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--特別保守往復時間-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label74" runat="server" Text="特別保守往復時間" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGbTm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--作業人数-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label76" runat="server" Text="作業人数" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPsnNum" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--提出日-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label78" runat="server" Text="提出日" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSubmitDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--メーカ修理-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label80" runat="server" Text="メーカ修理" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMakeRepair" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--備考-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label82" runat="server" Text="備考" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNotetext" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--処置内容-->
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label84" runat="server" Text="処置内容" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDealDtl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br>
                </asp:Panel>
                <br>

                <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <br>
                    <table border="0" style="width:100%" >
                        <tr>
                            <!--輸送日-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label86" runat="server" Text="輸送日" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransDt" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <!--輸送元-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label88" runat="server" Text="輸送元" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransSource" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--輸送先-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label90" runat="server" Text="輸送先" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransDest" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--輸送物品-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label92" runat="server" Text="輸送物品" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransItem" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--輸送理由-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label94" runat="server" Text="輸送理由" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransReason" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--輸送会社-->
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label96" runat="server" Text="輸送会社" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransComp" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--輸送区分-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label98" runat="server" Text="輸送区分" Width="110"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTransCls" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <br>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
