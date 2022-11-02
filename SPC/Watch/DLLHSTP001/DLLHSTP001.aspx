<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DLLHSTP001.aspx.vb" Inherits="SPC.DLLHSTP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContent" runat="server">


    <style type="text/css">
        .InfoTable {
            width: 880px; 
            margin:10px auto 20px auto;
            padding: 0px 0px 4px 10px; 
            border: 1px solid black; 
            border-spacing: 4px;
        }
    </style>

    <%--基本情報表示--%>
    <table border="0" class="InfoTable">
        <tr>
            <td style="width: 90px;"></td>
            <td style="width: 220px;"></td>
            <td style="width: 70px;"></td>
            <td style="width: 230px;"></td>
            <td style="width: 60px;"></td>
            <td style="width: 170px;">
                <asp:HiddenField ID="hdn_NLCls" runat="server" />
            </td>
        </tr>
        <tr>
            <td>TBOXID
            </td>
            <td>
                <asp:Label ID="lbl_TboxId" runat="server"></asp:Label>
            </td>
            <td>システム</td>
            <td>
                <asp:Label ID="lbl_System" runat="server"></asp:Label>
            </td>
            <td>
                NL区分</td>
            <td>

                <asp:Label ID="lbl_NLCls" runat="server"></asp:Label>

            </td>
        </tr>
        <tr>
            <td>ホール名</td>
            <td colspan="5">
                <asp:Label ID="lbl_HallNm" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>設定依頼内容</td>
            <td colspan="3">
                <asp:Label ID="lbl_DLLWRK" runat="server"></asp:Label>
            </td>
            <%--<td>手入力</td>--%>
            <td>
                <asp:Label ID="lbl_PutCls" runat="server"></asp:Label>
            </td>
        </tr>
        <%--        <tr>
            <td>設定日時</td>
            <td>
                <asp:Label ID="lbl_SetTime" runat="server"></asp:Label>
            </td>
            <td>戻し日時</td>
            <td>
                <asp:Label ID="lbl_RetTime" runat="server"></asp:Label>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>精算機変更</td>
            <td>
                <asp:Label ID="lbl_SisnSet" runat="server"></asp:Label>
            </td>
            <td>精算機戻し</td>
            <td>
                <asp:Label ID="lbl_SisnRet" runat="server"></asp:Label>
            </td>
            <td>ﾘﾄﾗｲ回数</td>
            <td>
                <asp:Label ID="lbl_RetryCnt" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>会員ｻｰﾋﾞｽｺｰﾄﾞ1</td>
            <td>
                <asp:Label ID="lbl_KinSvcCode1" runat="server"></asp:Label>
            </td>
            <td>備考</td>
            <td colspan="3" rowspan="2" style="vertical-align:top;">
                <asp:Label ID="lbl_Note" runat="server" Height="26px" Width="400px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>会員ｻｰﾋﾞｽｺｰﾄﾞ2</td>
            <td>
                <asp:Label ID="lbl_KinSvcCode2" runat="server"></asp:Label>
            </td>
            <td></td>
        </tr>--%>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>
    <%--一覧--%>
    <div style="width:1139px; margin-left:auto; margin-right:auto;">
        <!--該当件数表示 & リロードボタン-->
        <div ID="divCount" runat="server" class="float-Left">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" style="font-size:12pt;"></asp:Label>
                    </td>
                    <td style="width: 60px">
                        <div class="float-right">
                            <asp:Label ID="lblCount" runat="server" Text="XXXXX" style="font-size:12pt;"></asp:Label>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblCountUnit" runat="server" Text="件" style="font-size:12pt;"></asp:Label>
                    </td>
                    <td style="width: 15px"></td>
                    <td>
                        <asp:Button ID="btnReload" runat="server" CssClass="center" Text="リロード" />
                    </td>
                </tr>
            </table>
        </div>

        <!--グリッド-->
        <div class="grid-out">
            <div class="grid-in"  style="height:445px">
                <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                <asp:GridView ID="grvList" runat="server">
                </asp:GridView>
            </div>
        </div>
    </div>
    
</asp:Content>

