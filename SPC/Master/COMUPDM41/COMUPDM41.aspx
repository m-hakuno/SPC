<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM41.aspx.vb" Inherits="SPC.COMUPDM41" %>

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
              // 現在の非同期ポストバックをキャンセル(キー項目入力とクリアボタン押下の競合時用)
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
            btnDmy.style.visibility = "hidden";
            txtBox.focus();
        }
    </script>

    <style type="text/css">
        .auto-style1
        {
            width: 140px;
        }

        .auto-style2
        {
            width: 220px;
        }
    </style>

</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width: 700px; height: 50px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td class="auto-style1">
                <uc:ClsCMTextBox ID="txtBranchCd1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="支店" ppNameWidth="80" ppWidth="40" ppValidationGroup="search" ppTextAlign="左" ppNum="true" ppCheckHan="True" />
            </td>
            <td style="text-align: left; margin-top: 5px;" class="auto-style2">
                <asp:Label ID="lblBranchNm" runat="server"></asp:Label>
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftMainteCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="保守担当" ppNameWidth="80" ppWidth="40" ppCheckHan="True" ppValidationGroup="search" ppNum="true" ppTextAlign="左" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBoxFromTo ID="tftGeneralCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="統括保守担当" ppNameWidth="80" ppWidth="40" ppCheckHan="True" ppValidationGroup="search" ppNum="true" ppTextAlign="左" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="80" ppWidth="100" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <table style="width: 1000px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtMainteCd" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="保守担当" ppNameWidth="100" ppWidth="40" ppCheckHan="True" ppNum="True" ppValidationGroup="key" />
                        </td>
                        <td>
                            <asp:Label ID="lblMainteNm" runat="server" Text="" Width="600" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="郵便番号" Width="100" Style="margin-left: 3px;" />
                        </td>
                        <td>
                            <asp:Label ID="lblZipNo" runat="server" Text="" Width="60" Style="margin-left: 2px;"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="住所" Width="100" Style="margin-left: 3px;" />
                        </td>
                        <td>
                            <asp:Label ID="lblAddr" runat="server" Text="" Width="800" Style="margin-left: 2px;"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="電話番号" Width="100" Style="margin-left: 3px;" />
                        </td>
                        <td>
                            <asp:Label ID="lblTelNo" runat="server" Text="" Width="110" Style="margin-left: 2px;"/>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="FAX番号" Width="100" Style="margin-left: 10px;"/>
                        </td>
                        <td>
                            <asp:Label ID="lblFaxNo" runat="server" Text="" Width="110" />
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="連絡電話番号" Width="100" />
                        </td>
                        <td>
                            <asp:Label ID="lblTelNoS" runat="server" Text="" Width="110" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="支店" Width="100" />
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="" Width="600" Style="margin-left: 1px;"/>
                        </td>
                    </tr>
                </table>
            </td>
            <%--<td style="padding-left: 5px;">
                <uc:ClsCMDropDownList ID="ddlBranchCd" runat="server" ppName="支店" ppNameWidth="100" ppWidth="600" ppRequiredField="true" ppValidationGroup="val" ppClassCD="0015" />
            </td>--%>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <uc:ClsCMDropDownList ID="ddlGeneralCd" runat="server" ppName="統括保守担当" ppNameWidth="100" ppWidth="600" ppValidationGroup="val" ppClassCD="0015" />
            </td>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <uc:ClsCMDropDownList ID="ddlMaterialCd" runat="server" ppName="部材配備拠点" ppNameWidth="100" ppWidth="600" ppValidationGroup="val" ppClassCD="0015" />
            </td>
        </tr>
        <tr>
            <td style="padding-left: 2px;">
                <table>
                    <tr>
                        <td>
                            <uc:ClsCMTextBox ID="txtEmTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="緊急電話番号" ppNameWidth="100" ppWidth="110" ppCheckHan="True" ppValidationGroup="val" ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtSpTelNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="センタ番号" ppNameWidth="100" ppWidth="110" ppCheckHan="True" ppValidationGroup="val" ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 5px;">
                <uc:ClsCMTextBox ID="txtRemark1" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="備考" ppNameWidth="100" ppWidth="680" ppTextAlign="左" ppValidationGroup="val" />
            </td>
        </tr>

    </table>

</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div id="DivOut" runat="server" class="grid-out" style="width: 1240px; height: 228px;">
        <div id="DivIn" runat="server" class="grid-in" style="width: 1240px; height: 228px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

