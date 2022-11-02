<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM44.aspx.vb" Inherits="SPC.COMUPDM44" %>

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
            btnDmy.style.visibility = "hidden";
            txtBox.focus();

        }

    </script>
   
</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width: 800px; height: 50px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlTrd" runat="server" ppName="業者区分" ppNameWidth="80" ppWidth="110" ppClassCD="0015" ppRequiredField="False" ppEnabled="True" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftCompCd" runat="server" ppIMEMode="半角_変更可" ppMaxLength="5" ppName="会社" ppNameWidth="80" ppWidth="40" ppCheckHan="True" ppValidationGroup="search" ppExpression="^[0-9a-zA-Z]+$" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftOfficeCd" runat="server" ppIMEMode="半角_変更可" ppMaxLength="5" ppName="営業所" ppNameWidth="80" ppWidth="40" ppCheckHan="True" ppValidationGroup="search" ppExpression="^[0-9a-zA-Z]+$" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftSeq" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="連番" ppNameWidth="80" ppWidth="35" ppCheckHan="True" ppValidationGroup="search" ppNum="True" />
            </td>
            <td>
                <span style="position: relative;">
                    <asp:Label ID="LabelState" Text="都道府県" runat="server" Width="85px" Style="margin-top: 3px; margin-left: 3px;"></asp:Label></span><span style="position: relative;"><asp:DropDownList runat="server" ID="ddlPrefectureFm" Width="100px" /><label> ～ </label>
                        <asp:DropDownList runat="server" ID="ddlPrefectureTo" Width="100px" /></span><br />
                <div style="margin-left: 88px">
                    <asp:CustomValidator ID="cuvState" runat="server" Display="Dynamic" CssClass="errortext" ValidationGroup="search" ErrorMessage="CustomValidator" ValidateEmptyText="false" SetFocusOnError="True" />
                </div>
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="80" ppWidth="120" ppClassCD="0124" ppNotSelect="true" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <table style="width: 720px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td class="float-left">
                <uc:ClsCMTextBox ID="txtSeqNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="連番" ppNameWidth="100" ppWidth="40" ppCheckHan="True" ppNum="true" ppValidationGroup="key" EnableViewState="True" ViewStateMode="Inherit" />
            </td>
            <td class="float-left" style="margin-top: 2px;" colspan="4">
                <asp:Button ID="btnGetSeq" runat="server" Text="採番" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <uc:ClsCMDropDownList ID="ddlTrd1" runat="server" ppName="業者区分" ppNameWidth="100" ppWidth="100" ppClassCD="0015" ppRequiredField="True" ppEnabled="True" ppValidationGroup="val" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <uc:ClsCMDropDownList ID="ddlCompCd" runat="server" ppName="会社" ppNameWidth="100" ppWidth="600" ppRequiredField="true" ppValidationGroup="val" ppClassCD="0015" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <uc:ClsCMDropDownList ID="ddlOfficeCd" runat="server" ppName="営業所" ppNameWidth="100" ppWidth="600" ppValidationGroup="val" ppClassCD="0015" />
            </td>

        </tr>
        <tr>
            <td colspan="5">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="統括保守担当" Width="100" />
                        </td>
                        <td>
                            <asp:Label ID="lblIntgrtCd" runat="server" Text="" Width="400" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl" runat="server" Text="郵便番号" Width="100" />
                        </td>
                        <td>
                            <asp:Label ID="lblZipNo" runat="server" Text="" Width="80" />
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="3">
                <table>
                    <tr>
                        <td>
                            <%--<uc:ClsCMDropDownList ID="ddlState" runat="server" ppName="都道府県" ppNameWidth="100" ppWidth="100" ppClassCD="0015" ppRequiredField="False" ppEnabled="True" />--%>
                            <asp:Label ID="Label5" runat="server" Text="都道府県" Width="100" />
                        </td>
                        <td>
                            <asp:Label ID="lblState" runat="server" Text="" Width="80" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td colspan="5">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblad" runat="server" Text="住所" Width="100" />
                        </td>
                        <td>
                            <asp:Label ID="lblAddr" runat="server" Text="" Width="500" />
                        </td>
                    </tr>
                </table>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="代表電話番号" Width="100px" />
                        </td>
                        <td>
                            <asp:Label ID="lblTelNo" runat="server" Width="140" Text="" />
                        </td>
                    </tr>
                </table>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="FAX番号" Width="100" />

                            </td>
                            <td>
                                <asp:Label ID="lblFaxNo" runat="server" Width="140" Text="" />

                            </td>
                        </tr>
                    </table>
                </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="連絡電話番号" Width="100px" />
                        </td>
                        <td>
                            <asp:Label ID="lblEmTelNo" runat="server" Width="140" Text="" />

                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlArea" runat="server" ppName="料金エリア" ppNameWidth="100" ppWidth="140" ppClassCD="0015" ppEnabled="True" ppRequiredField="false" ppValidationGroup="val" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div id="DivOut" runat="server" class="grid-out" style="width: 1240px; height: 250px;">
        <div id="DivIn" runat="server" class="grid-in" style="width: 1240px; height: 250px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
