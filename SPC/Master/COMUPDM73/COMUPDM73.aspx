<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst_Ref.Master" AutoEventWireup="false" CodeBehind="COMUPDM73.aspx.vb" Inherits="SPC.COMUPDM73" %>
<%@ MasterType VirtualPath="~/Master/Mst_Ref.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {

            //PageRequestManagerのインスタンスを生成
            var mng = Sys.WebForms.PageRequestManager.getInstance();

            // 非同期ポストバックの初期化時に呼び出される
            // イベント・ハンドラを定義
            mng.add_initializeRequest(

              // ほかの非同期ポストバックが実行中で、かつ、
              // 現在のイベント発生元要素がbtnClear以外である場合、
              // 現在の非同期ポストバックをキャンセル(キー項目入力とクリアボタン押下の競合時用)
              function (sender, args) {
                  if (mng.get_isInAsyncPostBack() &&
                          args.get_postBackElement().id != "btnClear") {
                      args.set_cancel(true);
                  }
              }
            );
        }
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }
    </script>
   
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <table style="width:320px; margin-left: auto; margin-right: auto; border: none; height: auto;text-align:left;">
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtAppaCd" ppName="機器分類コード" ppNameWidth="100" ppMaxLength="2" ppCheckHan="True" ppTextAlign="左" ppValidationGroup="key" ppWidth="20" ppIMEMode="半角_変更不可" ppNum="true" />
            </td>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtName" ppName="機器分類名" ppNameWidth="100" ppMaxLength="10" ppTextAlign="左" ppRequiredField="True" ppValidationGroup="val" ppWidth="170" ppIMEMode="全角" />
            </td>
        </tr>
    </table>
    <%-- 区分は必要ないのでVisible=Falseに変更 --%>
                <uc:ClsCMTextBox runat="server" ID="txtDvsCd" Visible="false" ppName="区分" ppNameWidth="100" ppMaxLength="1" ppCheckHan="True" ppTextAlign="左" ppRequiredField="True" ppValidationGroup="val" ppWidth="20" ppIMEMode="半角_変更不可" ppNum="true" />
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width: 295px; height: 400px;">
        <div class="grid-in" style="width: 295px; height: 400px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

