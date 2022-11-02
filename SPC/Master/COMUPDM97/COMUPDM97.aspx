<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM97.aspx.vb" Inherits="SPC.COMUPDM97" %>

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

</asp:Content>

<asp:Content ID="serchcontent" ContentPlaceHolderID="SearchContent" runat="server">
    <table style="width: 700px; height: 50px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td style="width: 340px;">
                <uc:ClsCMDropDownList ID="ddlsAppaDvs" runat="server" ppName="機器分類" ppNameWidth="100" ppWidth="120" ppClassCD="0000" ppNotSelect="true" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlsAppaCls" runat="server" ppName="機器種別" ppNameWidth="100" ppWidth="220" ppClassCD="0000" ppNotSelect="true" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="100" ppWidth="120" ppClassCD="0124" ppNotSelect="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 800px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td style="width: 340px;">
                <uc:ClsCMDropDownList ID="ddlAppaDvs" runat="server" ppNameWidth="100" ppWidth="120" ppValidationGroup="key" ppRequiredField="true" ppName="機器分類" ppClassCD="0000" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlAppaCls" runat="server" ppNameWidth="85" ppWidth="220" ppValidationGroup="key" ppRequiredField="true" ppName="機器種別" ppClassCD="0000" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSeq" runat="server" ppNameWidth="50" ppWidth="30" ppValidationGroup="key" ppRequiredField="true" ppName="連番" ppIMEMode="半角_変更不可" ppMaxLength="3" ppNum="true" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlName" runat="server">
        <table style="width: 800px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
            <tr>
                <td style="width: 340px;">
                    <uc:ClsCMTextBox ID="txtAppaNm" runat="server" ppNameWidth="100" ppWidth="210" ppValidationGroup="val" ppRequiredField="true" ppName="機器備考名称" ppIMEMode="全角" ppMaxLength="15" />
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblbring" Text="持参物品管理" Width="82px"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxFlg" runat="server" Text="" TextAlign="Left" Width="20px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlOther" runat="server">
        <table style="width: 800px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
            <tr>
                <td style="width: 180px">
                    <uc:ClsCMDropDownList ID="ddlHddNo" runat="server" ppNameWidth="100" ppWidth="45" ppValidationGroup="val" ppRequiredField="true" ppName="HDDNo" ppClassCD="0000" />
                </td>
                <td style="padding-left: 2px">
                    <uc:ClsCMDropDownList ID="ddlHddCls" runat="server" ppNameWidth="85" ppWidth="45" ppValidationGroup="val" ppName="HDD種別" ppClassCD="0000" />
                </td>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlSeigyo" runat="server">
        <table style="width: 800px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
            <tr>
                <td style="width: 340px;">
                    <uc:ClsCMDropDownList ID="ddlKijun" runat="server" ppNameWidth="100" ppWidth="100" ppValidationGroup="val" ppRequiredField="true" ppName="判定基準" ppClassCD="0000" />
                </td>
                <td>
                    <uc:ClsCMDropDownList ID="ddlHante" runat="server" ppNameWidth="85" ppWidth="45" ppValidationGroup="val" ppRequiredField="true" ppName="判定値" ppClassCD="0000" />
                </td>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtSerStt" runat="server" ppNameWidth="100" ppWidth="30" ppValidationGroup="val" ppRequiredField="true" ppName="開始位置" ppIMEMode="半角_変更不可" ppMaxLength="2" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtSerStr" runat="server" ppNameWidth="85" ppWidth="30" ppValidationGroup="val" ppRequiredField="true" ppName="シリアル文字" ppIMEMode="半角_変更不可" ppMaxLength="1" />
                </td>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table style="width: 800px; height: 80px; margin-left: auto; margin-right: auto; text-align: left;">
        <tr>
            <td style="height: 100px; vertical-align: top; padding-top: 20px; padding-left: 2px;">
                <asp:Label ID="lblTboxType" runat="server" Text="システム" Width="72px" />
            </td>
            <td>
                <asp:CheckBoxList ID="cklTboxType" runat="server" CellSpacing="8" TextAlign="Right" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="7" Font-Size="Medium" />
                <br />
                <asp:CustomValidator ID="cus_TboxType" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <div class="grid-out" style="width: 1253px; height: 270px;">
        <div class="grid-in" style="width: 1253px; height: 270px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
