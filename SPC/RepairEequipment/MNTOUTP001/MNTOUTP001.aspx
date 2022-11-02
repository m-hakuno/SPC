<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="MNTOUTP001.aspx.vb" Inherits="SPC.MNTOUTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <script>
        function changeVisible() {
            document.getElementById('waitMsg').style.display = 'none';
            document.getElementById("<%= lblWait.ClientID%>").innerText = '';
            document.body.style.cursor = 'default';
        }

        function dispWait(flg) {
            var idWaitMsg = document.getElementById('waitMsg');
            var idlblWait = document.getElementById("<%= lblWait.ClientID%>");
            document.body.style.cursor = 'wait';
            if (flg == 'search') {
                idlblWait.innerText = '検索中です';
            } else if (flg == 'count') {
                idlblWait.innerText = '当月集計中です';
            } else if (flg == 'close') {
                idlblWait.innerText = '締め処理中です';
            } else if (flg == 'unclose') {
                idlblWait.innerText = '締め解除処理中です';
            } else {
                idlblWait.innerText = '';
            }
            if (idWaitMsg.style.display == 'none') {
                idWaitMsg.style.display = 'block';
            } else {
                idWaitMsg.style.display = 'none';
            }
            setTimeout("changeVisible()", 1000);
        }
    </script>
        <table border="0" style="width:1050px;">
            <tr>
                <td style= "padding-bottom :12px; width :225px"></td>
                <td>
                    <asp:CheckBoxList ID="chkDocument" runat="server" RepeatDirection="Vertical">
                        <asp:ListItem Value="0">情報機器整備の報告書兼検収書</asp:ListItem>
                        <asp:ListItem Value="1">ＴＢＯＸ通常整備料金について</asp:ListItem>
                        <asp:ListItem Value="2">整備完了品一覧表</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width: 235px"></td>
                <td style="width: 429px">
                    <uc:ClsCMDateBox runat="server" ID="txtNendo" ppName="年月度" ppDateFormat="年月" ppYobiVisible="False" ppRequiredField="False" ppNameWidth="69" />
                    <%--<uc:ClsCMTextBoxFromTo ID="tftReq" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppName="検収年月" ppNameWidth="69" ppWidth="50" ppCheckHan="True" ppNum="True"/>--%>
                </td>
                <td>
                    <uc:ClsCMTextBoxFromTo ID="tftOrder_No" runat="server" ppIMEMode="全角" ppMaxLength="11" ppName="注文番号" ppNameWidth="100" ppWidth="75" ppCheckHan="False" />
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width: 235px"></td>
                <td style="width: 431px">
                    <uc:ClsCMDateBoxFromTo ID="dftDeliv_D" runat="server" ppName="納入日" ppNameWidth="69" />
                </td>
                <td>
                    <uc:ClsCMTextBoxFromTo ID="tftMente_No" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="9" ppName="管理番号" ppNameWidth="99" ppWidth="65" ppCheckHan="False" />
                </td>
            </tr>
        </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
        <table class="center" style="width:933px;">
            <tr>
                <td>
                    <div id="waitMsg" style="display:none;font-size:18px;font-weight:bold;">
                        <asp:Label ID="lblWait" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="grid-out">
                        <div class="grid-in">
                              <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                              <asp:GridView ID="grvList" runat="server">
                              </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
