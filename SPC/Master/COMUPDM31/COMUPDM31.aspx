<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM31.aspx.vb" Inherits="SPC.COMUPDM31" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {

            //PageRequestManagerのインスタンスを生成
            var mng = Sys.WebForms.PageRequestManager.getInstance();

            // 非同期ポストバックの初期化時に呼び出される
            // イベント・ハンドラを定義
            mng.add_initializeRequest(

              // ほかの非同期ポストバックが実行中で、かつ、
              // 現在のイベント発生元要素がbtnGetSeq以外である場合、
              // 現在の非同期ポストバックをキャンセル
              function (sender, args) {
                  if (mng.get_isInAsyncPostBack() &&
                          args.get_postBackElement().id != "btnGetSeq") {
                      args.set_cancel(true);
                  }
              }
            );
        }

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
        }

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

    </script>
</asp:Content>

<asp:Content ID="SearchContent" ContentPlaceHolderID="SearchContent" runat="server">
    <table style="width:540px; margin-left:auto; margin-right:auto; border:none; text-align:left;">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtScode" runat="server" ppName="コード" ppNameWidth="70" ppWidth="30" ppMaxLength="2" ppIMEMode="半角_変更不可" ppTextAlign="左" ppCheckHan="true" ppNum="true" ppValidationGroup="search" />
            </td>
            <td style="float:left;margin-top:6px">
                <asp:Label ID="lblScontent" runat="server" Width="350" Text="" />
                <%--<uc:ClsCMTextBox ID="txtScontent" runat="server" ppName="用途名称" ppNameWidth="70" ppWidth="350" ppMaxLength="20" ppIMEMode="全角" ppTextAlign="左" />--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                 <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="70" ppWidth="100" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:540px; margin-right:auto; margin-left:auto; border:none; text-align:left;">
        <tr>
            <td>
                 <asp:Label ID="Label1" runat="server" Width="70" Text="コード" />
            </td>
            <td>
                <asp:Button ID="btnGetSeq" runat="server" Text="採番" />
            </td>
        
            <td>
                <uc:ClsCMTextBox ID="txtMcode" runat="server" ppName="コード" ppNameVisible="false" ppNameWidth="0" ppWidth="30" ppMaxLength="2" ppIMEmode="半角_変更不可" ppTextAlign="左" ppValidationGroup="key" ppCheckHan="true" ppNum="true" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtMcontent" runat="server" ppName="" ppNameWidth="0" ppWidth="300" ppMaxLength="20" ppIMEMode="全角" ppTextAlign="左" ppRequiredField="True" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" contentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 428px;height:460px;">
        <div class="grid-in" style="width: 428px;height:460px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
