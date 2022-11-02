<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM36.aspx.vb" Inherits="SPC.COMUPDM36" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        //変更フラグ
        var intChanged = 0;

        //保存した一覧のscrollTopを設定
        function pageLoad() {            
            var divGrv = document.getElementsByClassName('grid-in')[0];
            var txtSavePosition = document.getElementsByClassName('JS_GrvPosition')[0];
            divGrv.scrollTop = txtSavePosition.value;
        }

        //ボタン押下で変更フラグ＝１
        function ChangeFlg(objbtn) {
            intChanged = 1;

            //一覧のScrollPosition保存
            var divGrv = document.getElementsByClassName('grid-in')[0];
            var txtSavePosition = document.getElementsByClassName('JS_GrvPosition')[0];
            txtSavePosition.value = divGrv.scrollTop;
        }

        //編集エリア内のポストバック時の処理
        function AlertDispose(strYear, strMonth) {

            //一覧のScrollPosition保存
            var divGrv = document.getElementsByClassName('grid-in')[0];
            var txtSavePosition = document.getElementsByClassName('JS_GrvPosition')[0];
            txtSavePosition.value = divGrv.scrollTop;

            //変更破棄確認＆入力年月変更
            if (intChanged == 1) {

                //隠しラベル取得(confirm結果のJS→サーバー渡し用)
                var txtOKCancel;
                txtOKCancel = document.getElementsByClassName('JS_OKCancel');

                if (confirm("入力内容が破棄されます。よろしいですか？") == true) {
                    //OK 結果保存 フラグ初期化
                    intChanged = 0;
                    txtOKCancel[0].value = 'OK';

                    if (typeof strYear != 'undefined' && typeof strMonth != 'undefined') {
                        //選択年月を検索エリアに反映
                        var txtYaer = document.getElementsByClassName('JS_Year');
                        var ddlMonth = document.getElementsByClassName('JS_Month');
                        txtYaer[0].value = strYear;
                        ddlMonth[0].options[strMonth - 1].selected = true;
                    }

                    return true;
                } else {
                    //Cancel 結果保存
                    txtOKCancel[0].value = 'Cancel';

                    return false;   //falseを返してpostbackを起こさない(onClientClickの場合)
                }
            } else {

                if (arguments.length == 2) {
                    //選択年月を検索エリアに反映
                    var txtYaer = document.getElementsByClassName('JS_Year');
                    var ddlMonth = document.getElementsByClassName('JS_Month');
                    txtYaer[0].value = strYear;
                    ddlMonth[0].options[strMonth - 1].selected = true;
                }

                return true;
            }
        }

    </script>

    <style type="text/css">
        table.border {
            border: solid 1px #000000;
            border-collapse: collapse;
        }

        td.cell {
            border: solid 1px #000000;
            padding: 3px 1px 3px 1px;
        }
    </style>

</asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width: 150px; margin-left: auto; margin-right: auto; border: none;">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtYear" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="4" ppName="年" ppNameWidth="0" ppWidth="35" ppCheckHan="True" ppValidationGroup="search" ppNameVisible="False" />
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="年" Width="30px"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" Width="40px" CssClass="JS_Month">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="月" Width="30px"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">

    <div id="hidden" style="display: none;">
        <asp:TextBox ID="txtOKCancel" runat="server" CssClass="JS_OKCancel">OK</asp:TextBox>
        <asp:TextBox ID="txtGrvPosition" runat="server" CssClass="JS_GrvPosition">0</asp:TextBox>
    </div>
    <div class="center" style="width: 1000px; height: 450px;">
        <div style="float: left;">
            <div style="padding-bottom: 2px;text-align:left;">
                <asp:Label ID="lblCarenderMonth" runat="server" Text="YYYY年MM月" Font-Size="Large"></asp:Label>
            </div>
            <table style="width: 600px; border: solid 1px #000000; border-collapse: collapse;">
                <tr style="height: 33px">
                    <td class="cell">
                        <asp:Label ID="Label3" runat="server" Font-Size="11pt" Text="日" ForeColor="Red"></asp:Label>
                    </td>
                    <td class="cell">
                        <asp:Label ID="Label4" runat="server" Font-Size="11pt" Text="月"></asp:Label>
                    </td>
                    <td class="cell">
                        <asp:Label ID="Label5" runat="server" Font-Size="11pt" Text="火"></asp:Label>
                    </td>
                    <td class="cell">
                        <asp:Label ID="Label6" runat="server" Font-Size="11pt" Text="水"></asp:Label>
                    </td>
                    <td class="cell">
                        <asp:Label ID="Label7" runat="server" Font-Size="11pt" Text="木"></asp:Label>
                    </td>
                    <td class="cell">
                        <asp:Label ID="Label8" runat="server" Font-Size="11pt" Text="金"></asp:Label>
                    </td>
                    <td class="cell">
                        <asp:Label ID="Label9" runat="server" Font-Size="11pt" Text="土" ForeColor="#3333FF"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="cell">
                        <asp:Button ID="Button01" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button02" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button03" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button04" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button05" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button06" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button07" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                </tr>
                <tr>
                    <td class="cell">
                        <asp:Button ID="Button08" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button09" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button10" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button11" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button12" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button13" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button14" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                </tr>
                <tr>
                    <td class="cell">
                        <asp:Button ID="Button15" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button16" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button17" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button18" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button19" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button20" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button21" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                </tr>
                <tr>
                    <td class="cell">
                        <asp:Button ID="Button22" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button23" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button24" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button25" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button26" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button27" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button28" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                </tr>
                <tr>
                    <td class="cell">
                        <asp:Button ID="Button29" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button30" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button31" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button32" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button33" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button34" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button35" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                </tr>
                <tr id="trwCalendar" runat="server">
                    <td class="cell">
                        <asp:Button ID="Button36" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button37" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button38" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button39" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button40" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button41" runat="server" Height="100px" Text="Button" Width="100px" />
                    </td>
                    <td class="cell">
                        <asp:Button ID="Button42" runat="server" Height="100px" Text="Button" Width="100px" BorderStyle="None" />
                    </td>
                </tr>
            </table>
        </div>

        <div style="float: right;">
            <div style="padding-bottom: 2px;">
                <asp:Label ID="lblListTitle" runat="server" Text="登録内容一覧"></asp:Label>
            </div>
            <div id="DivOut" runat="server" class="grid-out" style="width: 165px; height: 370px;">
                <div id="DivIn" runat="server" class="grid-in" style="width: 165px; height: 370px;">
                    <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                    <asp:GridView ID="grvList" runat="server"></asp:GridView>
                </div>
            </div>
        </div>

    </div>

</asp:Content>

