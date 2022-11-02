<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDMA1.aspx.vb" Inherits="SPC.COMUPDMA1" %>

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

    </script>


    <style type="text/css">
        .auto-style4
        {
            width: 260px;
        }

        .auto-style5
        {
            width: 320px;
        }

        .auto-style6
        {
            width: 318px;
        }
    </style>


</asp:Content>

<asp:Content ID="serchcontent" ContentPlaceHolderID="SearchContent" runat="server">
    <table style="width: 1000px; height: 50px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlsTrd" runat="server" ppName="業者区分" ppNameWidth="100" ppWidth="110" ppClassCD="0069" ppNotSelect="true" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlsPair" runat="server" ppName="分類区分" ppNameWidth="100" ppWidth="120" ppClassCD="0135" ppNotSelect="true" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftsCd" runat="server" ppIMEMode="半角_変更可" ppMaxLength="5" ppName="コード" ppNameWidth="100" ppWidth="40" ppCheckHan="True" ppValidationGroup="search" ppExpression="^[0-9a-zA-Z]+$" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="100" ppWidth="110" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 1000px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td class="auto-style6">
                            <uc:ClsCMDropDownList ID="ddlTrd" runat="server" ppName="業者区分" ppNameWidth="100" ppWidth="110" ppClassCD="0069" ppValidationGroup="key" ppNotSelect="true" />
                        </td>
                        <td class="auto-style4">
                            <uc:ClsCMDropDownList ID="ddlPair" runat="server" ppName="分類区分" ppNameWidth="80" ppWidth="120" ppClassCD="0135" ppValidationGroup="key" ppNotSelect="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtCd" runat="server" ppIMEMode="半角_変更可" ppMaxLength="5" ppName="業者コード" ppNameWidth="100" ppWidth="40" ppCheckHan="True" ppValidationGroup="key" ppExpression="^[0-9a-zA-Z]+$" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtName" runat="server" ppIMEMode="全角" ppNameVisible="false" ppMaxLength="45" ppWidth="600" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtZipNo1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="3" ppName="郵便番号" ppNameWidth="100" ppWidth="30" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtZipNo2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="-" ppNameWidth="8" ppWidth="40" ppValidationGroup="val"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblstate" runat="server" Text="都道府県" Width="80" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server" Width="100" ValidationGroup="key" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table style="line-height: 3px">
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtAddr1" runat="server" ppIMEMode="全角" ppMaxLength="25" ppName="住所" ppNameWidth="100" ppWidth="350" ppTextAlign="左" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtAddr2" runat="server" ppIMEMode="全角" ppMaxLength="25" ppName="" ppNameWidth="100" ppWidth="350" ppTextAlign="左" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtAddr3" runat="server" ppIMEMode="全角" ppMaxLength="25" ppName="" ppNameWidth="100" ppWidth="350" ppTextAlign="左" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="auto-style5">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtTel1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppName="代表電話番号" ppNameWidth="100" ppWidth="50" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTel2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtTel3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtEmTel1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppName="連絡電話番号" ppNameWidth="100" ppWidth="50" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtEmTel2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtEmTel3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtFax1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="6" ppName="FAX番号" ppNameWidth="80" ppWidth="50" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtFax2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" />
                        </td>
                        <td class="float-left">
                            <uc:ClsCMTextBox ID="txtFax3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="-" ppNameWidth="8" ppWidth="50" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 1240px; height: 270px;">
        <div class="grid-in" style="width: 1240px; height: 270px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
