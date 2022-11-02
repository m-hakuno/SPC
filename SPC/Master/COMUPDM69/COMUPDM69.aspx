<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM69.aspx.vb" Inherits="SPC.COMUPDM69" %>

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
              // 現在のイベント発生元要素がbtnClear以外である場合、
              // 現在の非同期ポストバックをキャンセル(クリアボタンを優先する) 
              function (sender, args) {
                  if (mng.get_isInAsyncPostBack() &&
                          args.get_postBackElement().id != "btnClear") {
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

<asp:Content ID="serchcontent" ContentPlaceHolderID="SearchContent" runat="server">
    <table style="width: 700px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftSCode" runat="server" ppIMEMode="半角_変更不可" ppName="コード" ppNameWidth="60" ppWidth="26" ppMaxLength="3" ppCheckHan="true" ppNum="true" ppValidationGroup="search" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSName" runat="server" ppIMEMode="全角" ppName="名称" ppNameWidth="50" ppWidth="400" ppMaxLength="30" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlSPrint" runat="server" ppName="品質会議印刷名" ppNameWidth="100" ppWidth="90" ppClassCD="0145" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 700px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtMCode" runat="server" ppName="コード" ppNameWidth="60" ppWidth="26" ppMaxLength="3" ppIMEMode="半角_変更不可" ppCheckHan="true" ppNum="true" ppValidationGroup="key" />
            </td>
            <td style="padding-left:60px;">
                <uc:ClsCMTextBox ID="txtMName" runat="server" ppName="名称" ppNameWidth="50" ppIMEMode="全角" ppWidth="400" ppMaxLength="30" ppValidationGroup="val" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlMPrint" runat="server" ppName="品質会議印刷名" ppNameWidth="100" ppWidth="90" ppClassCD="0145" ppNotSelect="True" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 642px; height: 500px;">
        <div class="grid-in" style="width: 642px; height: 500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
