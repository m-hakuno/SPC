<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.Master" CodeBehind="WATOUTP001.aspx.vb" Inherits="SPC.WATOUTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">

    <script type="text/javascript">

        //「検索中」メッセージ表示
        function showMsgDiv() {

        var MsgDiv;

        MsgDiv = document.getElementById('MsgDiv');

        if (MsgDiv == null) {

            return false;

        } else {

            MsgDiv.style.display = '';
            return true;

        }
    }

</script>

    <style type="text/css">
        .InfoTable {
            width: 700px; 
            margin:10px auto 20px auto;
            padding: 0px 0px 4px 10px; 
            border: 1px solid black; 
            border-spacing: 4px;
        }
        .SearchTable {
            margin-left:auto;
            margin-right:auto;
            width: 500px;
        }
        .ResultTable {
            margin-left:auto;
            margin-right:auto;
            width: 300px;
            border-collapse:collapse;
        }
    </style>

    <%--基本情報表示--%>
    <table border="0" class="InfoTable">
        <tr>
            <td style="width: 80px;"></td>
            <td style="width: 140px;"></td>
            <td style="width: 70px;"></td>
            <td style="width: 230px;"></td>
            <td style="width: 60px;"></td>
            <td style="width: 120px;"></td>
        </tr>
        <tr>
            <td>
                TBOXID
            </td>
            <td>
                <asp:Label ID="lblTboxId" runat="server"></asp:Label>
            </td>
            <td>
                ホール名
            </td>
            <td colspan="3">
                <asp:Label ID="lblHallNm" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>データ種別</td>
            <td>
                <asp:Label ID="lblDataClsNm" runat="server"></asp:Label>
            </td>
            <td>要求日時</td>
            <td>
                <asp:Label ID="lblReqDate" runat="server" Text="9999/12/31 23:59:59"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="lblVer" runat="server"></asp:Label>
            </td>
        </tr>
    </table>

    <%--検索--%>
    <table border="0" class="SearchTable">
        <tr runat="server" id="tr_time">
            <td>時間帯</td>
            <td>
                <uc:ClsCMTimeBoxFromTo ID="tmftTimeSpan" runat="server" ppName="時間帯" ppNameVisible="False" ppNameWidth="0"/>
                </td>
        </tr>
        <tr runat="server" id="tr_JBNo">
            <td>JB番号</td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftJBNo" runat="server" ppMaxLength="4" ppName="JB番号" ppWidth="40" ppCheckHan="True" ppNum="True" ppCheckLength="True" ppIMEMode="半角_変更不可" ppNameVisible="False" />
                </td>
        </tr>
        <tr runat="server" id="tr_CID">
            <td>CID</td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftCID" runat="server" ppMaxLength="16" ppName="JB番号" ppWidth="130" ppCheckHan="True" ppNum="True" ppCheckLength="True" ppIMEMode="半角_変更不可" ppNameVisible="False" />
                </td>
        </tr>
        <tr runat="server" id="tr_SlctPrnt">
            <td>
                <asp:Label ID="lblSlct" runat="server" Text="データ種別名"></asp:Label>
            </td>
            <td>
                <asp:RadioButtonList ID="rblSlctPrnt" runat="server">
                </asp:RadioButtonList>
            </td>
        </tr>
        <%--        <tr>
            <td>BB異常ログ</td>
            <td></td>
        </tr>
        <tr>
            <td>債権分計情報</td>
            <td></td>
        </tr>
        <tr>
            <td>店舗指定</td>
            <td></td>
        </tr>
        <tr>
            <td>鍵位置</td>
            <td></td>
        </tr>
        <tr>
            <td>店内通信状況</td>
            <td></td>
        </tr>--%>
        <tr style="height: 25px;">
            <td colspan="2">
                <asp:HiddenField ID="hdnDataCls" runat="server" />
            </td>
        </tr>
    </table>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">

    <%--結果表示--%><%--    <table border ="1" class ="ResultTable">
        <tr id="MsgDiv" style="display:none;">
            <td colspan="2">
                <div style="background-color:white; text-align:center; padding:5px 0px 5px 0px;">
                    <asp:Label ID="Label10" runat="server" Text="検索中です" Width="110px" Visible="True" Font-Bold="True" Font-Size="Medium"></asp:Label>
                </div>
            </td>
        </tr>
        <tr style="background-color:white; text-align:center;">
            <td style="width:100px; background-color:#8DB4E2; padding:2px 0px 2px 0px; margin-right:50px; height: 17px;">
                    <asp:Label ID="Label11" runat="server" Text="検索結果" Width="150px"></asp:Label>
            </td>
            <td style="width:200px; height: 17px;">
                    <asp:Label ID="lblRecordCount" runat="server" Text="NNNN" Width="40px"></asp:Label>
                    <asp:Label ID="Label13" runat="server" Text="件" Width="20px" Visible="True"></asp:Label>
            </td>
        </tr>
    </table>--%>


</asp:Content>
