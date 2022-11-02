<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSOUTP001.aspx.vb" Inherits="SPC.CNSOUTP001" %>
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
                setTimeout("changeVisible()", 1000);
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
        }
    </script>
<table class="center">
    <tr>
        <td>
            <uc:ClsCMDateBox runat="server" ID="txtNendo" ppName="年月度" ppDateFormat="年月" ppYobiVisible="False" ppRequiredField="True" />
        </td>
    </tr>
</table>       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
      <table class="center">
            <tr>
                <td>
                    <div id="waitMsg" style="display:none;font-size:18px;font-weight:bold;">
                        <asp:Label ID="lblWait" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
        <tr>
            <td>
                <div class="grid-out" style="height: 146px; width: 720px;">
                    <div class="grid-in" style="height: 146px; width: 720px;">
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
