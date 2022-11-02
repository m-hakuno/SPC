<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM96.aspx.vb" Inherits="SPC.COMUPDM96" %>

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

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

        function setdeletezero(obj) {
            var number = obj.value.replace(/(^\s+)|(\s+$)/g, "");
            if (number == 0) {
                obj.value = ''
                obj.focus();
                obj.setSelectionRange(0, 0);
            } else {
                obj.focus();
                obj.value = number;
                obj.setSelectionRange(0, 0);
            }
        }

        function settotaljippi(obj1, obj2, objtotal) {
            var money1 = Number(obj1.value.replace(/(^\s+)|(\s+$)/g, ""));
            //var money2 = Number(obj2.value.replace(/(^\s+)|(\s+$)/g, ""));
            if (isNaN(money1)) {
            }
            else if (money1 == '') {
                obj1.value = 0;
            }
            else {
                obj1.value = parseInt(obj1.value);
                if (isNaN(obj1.value)) {//小数入力
                    obj1.value = 0;
                }
            }
            //if (isNaN(money2)) {
            //}
            //else if (money2 == '') {
            //    obj2.value = 0;
            //}
            //else {
            //    obj2.value = parseInt(obj2.value);
            //    if (isNaN(obj2.value)) {//小数入力
            //        obj2.value = 0;
            //    }
            //}
            money1 = Number(obj1.value.replace(/(^\s+)|(\s+$)/g, ""));
            //money2 = Number(obj2.value.replace(/(^\s+)|(\s+$)/g, ""));
            //var totalmoney = money1 + money2;
            //if (isNaN(totalmoney)) {
            //    objtotal.innerText = '\\ 0';
            //}
            //else {
            //    objtotal.innerText = String(totalmoney).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
            //    objtotal.innerText = '\\ ' + objtotal.innerText;
            //}
        }
    </script>
</asp:Content>

<%--<asp:Content ID="serchcontent" ContentPlaceHolderID="SearchContent" runat="server">
     <table style="width:700px;margin-left:auto;margin-right:auto;border:none;text-align:left;">
         <tr>
             <td>
                <uc:ClsCMTextBoxFromTo ID="txtSCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="1" ppName="特別保守フラグ" ppNum="true" ppNameWidth="120" ppWidth="20" ppCheckHan="True" ppValidationGroup="search" />
             </td>
         </tr>
         <tr>
             <td>
                <uc:ClsCMTextBox ID="txtSNm" runat="server" ppIMEMode="全角" ppMaxLength="20" ppName="特別保守名称" ppNameWidth="120" ppWidth="280" />
            </td>
         </tr>
     </table>
</asp:Content>--%>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:800px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMTextBox runat="server" ID="txtSpPriceCd" ppNameWidth="100" ppWidth="20" ppCheckHan="true" ppNum="true" ppName="特別保守フラグ" ppIMEMode="半角_変更不可" ppMaxLength="1" ppValidationGroup="key" ppRequiredField="true" />
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox runat="server" ID="txtSpPriceNm" ppNameWidth="90" ppWidth="280" ppName="特別保守名称" ppIMEMode="全角" ppMaxLength="20" ppRequiredField="true"  ppValidationGroup="val" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblPrice1_b" Width="100px" Text="通常料金" />
                <asp:TextBox ID="txtPrice1_b" runat="server" Width="70px" MaxLength="8" CssClass="IMEdisabledNum"></asp:TextBox>
                <%--<uc:ClsCMTextBox runat="server" ID="txtPrice1" ppName="通常料金" ppNameWidth="150" ppWidth="150" ppMaxLength="8" ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可" ppTextAlign="右" ppExpression="^[0-9]+$" />--%>
                <br />
                <asp:CustomValidator ID="CstmVal_PriceN" runat="server" ControlToValidate="txtPrice1_b" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
            </td>
            <td>
                <uc:ClsCMTimeBox runat="server" ID="tmbStartTm1" ppName="開始時刻" ppNameWidth="90" ppValidationGroup="val" />
            </td>
            <td>
                <uc:ClsCMTimeBox runat="server" ID="tmbEndTm1" ppName="終了時刻" ppNameWidth="80" ppValidationGroup="val" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblPrice2_b" Width="100px" Text="特別料金" />
                <asp:TextBox ID="txtPrice2_b" runat="server" Width="70px" MaxLength="8" CssClass="IMEdisabledNum"></asp:TextBox>
                <%--<uc:ClsCMTextBox runat="server" ID="txtPrice2" ppName="特別料金" ppNameWidth="150" ppWidth="150" ppMaxLength="8" ppCheckHan="True" ppNum="True" ppIMEMode="半角_変更不可" ppTextAlign="右" ppExpression="^[0-9]+$" />--%>
                <br />
                <asp:CustomValidator ID="CstmVal_PriceS" runat="server" ControlToValidate="txtPrice2_b" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
            </td>
            <td>
                <uc:ClsCMTimeBox runat="server" ID="tmbStartTm2" ppName="開始時刻" ppNameWidth="90" ppValidationGroup="val" />
            </td>
            <td>
                <uc:ClsCMTimeBox runat="server" ID="tmbEndTm2" ppName="終了時刻" ppNameWidth="80" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
    <%-- グリッドからの選択待ちの時"1"にする --%>
    <%--<asp:HiddenField ID="hdnDtl_SelectFLG" runat="server" Value="0" />--%>
</asp:Content>


<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 863px; height: 146px;">
        <div class="grid-in" style="width: 863px; height: 146px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
