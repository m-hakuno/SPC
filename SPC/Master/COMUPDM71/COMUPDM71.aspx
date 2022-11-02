<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM71.aspx.vb" Inherits="SPC.COMUPDM71" %>
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
            <td style="float: left;">
                <uc:ClsCMDropDownList ID="ddlSDisp" runat="server" ppName="画面区分" ppNameWidth="60" ppWidth="150" ppClassCD="0146" ppNotSelect="true" />
            </td>
            <td style="float: left;margin-left:30px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSSys" runat="server" Text="システム" Width="60px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSSystem" runat="server" Width="120px" ValidationGroup="search"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="float: left;margin-left:30px;">
                <uc:ClsCMTextBoxFromTo ID="tftSCode" runat="server" ppIMEMode="半角_変更不可" ppName="コード" ppNameWidth="60" ppWidth="26" ppMaxLength="3" ppCheckHan="true" ppNum="true" ppValidationGroup="search" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtSName" runat="server" ppIMEMode="全角" ppName="文言" ppNameWidth="60" ppWidth="650" ppMaxLength="50" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 700px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td style="float: left;">
                <uc:ClsCMDropDownList ID="ddlMDisp" runat="server" ppName="画面区分" ppNameWidth="60" ppWidth="150" ppClassCD="0146" ppNotSelect="true" />
            </td>
            <td style="float: left;margin-left:30px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="システム" Width="60px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMSystem" runat="server" Width="120px"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="float: left;margin-left:30px;">
                <uc:ClsCMTextBox ID="txtMCode" runat="server" ppName="コード" ppNameWidth="60" ppWidth="26" ppMaxLength="3" ppIMEMode="半角_変更不可" ppCheckHan="true" ppNum="true" ppValidationGroup="key" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtMName" runat="server" ppName="文言" ppNameWidth="60" ppIMEMode="全角" ppWidth="650" ppMaxLength="50" ppValidationGroup="val" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 1023px; height: 440px;">
        <div class="grid-in" style="width: 1023px; height: 440px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
