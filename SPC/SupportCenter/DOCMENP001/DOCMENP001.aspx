<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="DOCMENP001.aspx.vb" Inherits="SPC.DOCMENP001" %>

<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <script>
        function changeVisible() {
            document.getElementById('waitMsg').style.display = 'none';
            document.getElementById("<%= lblWait.ClientID%>").innerText = '';
            document.body.style.cursor = 'default';
        }

        function dispWait(flg) {
            var idWaitMsg = document.getElementById('waitMsg');
            var idlblWait = document.getElementById("<%= lblWait.ClientID%>");
            document.body.style.cursor = 'wait';
            if (flg == 'search') {
                idlblWait.innerText = '検索中です';
                setTimeout("changeVisible()", 1000);
            } else if (flg == 'count') {
                idlblWait.innerText = '当月集計中です';
            } else if (flg == 'close') {
                idlblWait.innerText = '締め処理中です';
            } else if (flg == 'unclose') {
                idlblWait.innerText = '締め解除処理中です';
            } else {
                idlblWait.innerText = '';
            }
            if (idWaitMsg.style.display == 'none') {
                idWaitMsg.style.display = 'block';
            } else {
                idWaitMsg.style.display = 'none';
            }
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

        function settotaljinkenhi(objmoney, objcnt, objtotal) {
            var money = objmoney.value.replace(/(^\s+)|(\s+$)/g, "");
            var cnt = objcnt.value.replace(/(^\s+)|(\s+$)/g, "");
            if (isNaN(money)) {
            }
            else if (money == '') {
                objmoney.value = 0;
            }
            else {
                objmoney.value = parseInt(objmoney.value);
                if (isNaN(objmoney.value)) {//小数入力
                    objmoney.value = 0;
                }
            }
            if (isNaN(cnt)) {
            }
            else if (cnt == '') {
                objcnt.value = 0;
            }
            else {
                objcnt.value = parseInt(objcnt.value);
                if (isNaN(objcnt.value)) {//小数入力
                    objcnt.value = 0;
                }
            }

            money = objmoney.value.replace(/(^\s+)|(\s+$)/g, "");
            cnt = objcnt.value.replace(/(^\s+)|(\s+$)/g, "");
            var totalmoney = money * cnt;

            if (isNaN(totalmoney)) {
                objtotal.innerText = '\\ 0';
            }
            else {
                objtotal.innerText = String(totalmoney).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                objtotal.innerText = '\\ ' + objtotal.innerText;
            }
        }

        function settotaljippi_two(obj1, obj2, objtotal) {
            var money1 = Number(obj1.value.replace(/(^\s+)|(\s+$)/g, ""));
            var money2 = Number(obj2.value.replace(/(^\s+)|(\s+$)/g, ""));
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
            if (isNaN(money2)) {
            }
            else if (money2 == '') {
                obj2.value = 0;
            }
            else {
                obj2.value = parseInt(obj2.value);
                if (isNaN(obj2.value)) {//小数入力
                    obj2.value = 0;
                }
            }
            money1 = Number(obj1.value.replace(/(^\s+)|(\s+$)/g, ""));
            money2 = Number(obj2.value.replace(/(^\s+)|(\s+$)/g, ""));
            var totalmoney = money1 + money2;
            if (isNaN(totalmoney)) {
                objtotal.innerText = '\\ 0';
            }
            else {
                objtotal.innerText = String(totalmoney).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                objtotal.innerText = '\\ ' + objtotal.innerText;
            }
        }

        function settotaljippi(obj1, obj2, obj3, obj4, objtotal) {
            var money1 = Number(obj1.value.replace(/(^\s+)|(\s+$)/g, ""));
            var money2 = Number(obj2.value.replace(/(^\s+)|(\s+$)/g, ""));
            var money3 = Number(obj3.value.replace(/(^\s+)|(\s+$)/g, ""));
            var money4 = Number(obj4.value.replace(/(^\s+)|(\s+$)/g, ""));
            
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
            if (isNaN(money2)) {
            }
            else if (money2 == '') {
                obj2.value = 0;
            }
            else {
                obj2.value = parseInt(obj2.value);
                if (isNaN(obj2.value)) {//小数入力
                    obj2.value = 0;
                }
            }
            if (isNaN(money3)) {
            }
            else if (money3 == '') {
                obj3.value = 0;
            }
            else {
                obj3.value = parseInt(obj3.value);
                if (isNaN(obj3.value)) {//小数入力
                    obj3.value = 0;
                }
            }
            if (isNaN(money4)) {
            }
            else if (money4 == '') {
                obj4.value = 0;
            }
            else {
                obj4.value = parseInt(obj4.value);
                if (isNaN(obj4.value)) {//小数入力
                    obj4.value = 0;
                }
            }

            money1 = Number(obj1.value.replace(/(^\s+)|(\s+$)/g, ""));
            money2 = Number(obj2.value.replace(/(^\s+)|(\s+$)/g, ""));
            money3 = Number(obj3.value.replace(/(^\s+)|(\s+$)/g, ""));
            money4 = Number(obj4.value.replace(/(^\s+)|(\s+$)/g, ""));
            var totalmoney = money1 + money2 + money3 + money4;
            if (isNaN(totalmoney)) {
                objtotal.innerText = '\\ 0';
            }
            else {
                objtotal.innerText = String(totalmoney).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1,');
                objtotal.innerText = '\\ ' + objtotal.innerText;
            }
        }

    </script>
    <table class="center" border="0" style="width: 40%;">
        <tr>
            <td style="padding-left: 80px;">
                <uc:ClsCMDateBox runat="server" ID="txtKensyuYm" ppName="検収年月" ppDateFormat="年月" ppNameWidth="70px" ppValidationGroup="Kensyu" ppRequiredField="true" />
            </td>
        </tr>
        <tr>
            <td>
                <div class="float-left" style="padding-bottom: 10px; padding-left: 80px;">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errortext" ValidationGroup="Kensyu" />
                </div>
            </td>
        </tr>

    </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">

    <%----------------------新マスタ登録欄--------------------------%>
    <asp:Panel ID="pnlJinkenhi201610" runat="server" CssClass="center" BorderStyle="Solid" BorderWidth="1px" Width="95%" Visible="true">
        <table class="center" border="1" style="width: 90%">
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label1" runat="server" Text="管理者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label2" Text="金額" />
                </td>
                <td>
                    <uc:ClsCMTextBox runat="server" ID="txtAdminAmt" ppName="管理者金額" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                </td>
                <td>
                    <asp:Label runat="server" ID="Label33" Text="人数" />
                </td>
                <td>
                    <uc:ClsCMTextBox runat="server" ID="txtAdminCnt" ppName="管理者人数" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                </td>
                <td style="text-align: right;width:200px;">
                    <asp:Label runat="server" ID="lblNewAdminTotal" Text="\ 0" />
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label37" runat="server" Text="業務担当者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label39" Text="金額" />
                </td>
                <td>
                    <uc:ClsCMTextBox runat="server" ID="txtStaffAmt" ppName="業務担当者金額" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                </td>
                <td>
                    <asp:Label runat="server" ID="Label41" Text="人数" />
                </td>
                <td>
                    <uc:ClsCMTextBox runat="server" ID="txtStaffCnt" ppName="業務担当者人数" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                </td>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblNewStaffTotal" Text="\ 0" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <div class="float-right">
                        <asp:Button ID="BtnJinkenhiIns" runat="server" Text="マスタ変更" />
                    </div>
                    <div class="float-left" style="padding-bottom: 30px;">
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" CssClass="errortext" ValidationGroup="Jinkenhi" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%----------------------マスタ登録欄（通常はVisible=false）--------------------------%>
    <asp:Panel ID="pnlJinkenhiNew" runat="server" CssClass="center" BorderStyle="Solid" BorderWidth="1px" Width="95%" Visible="false">
        <table class="center" border="0" style="width: 90%">
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label19" runat="server" Text="管理者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label20" Text="金額" />
                </td>
                <td style="text-align: right; padding-right: 20px;width :100px;">
                    <asp:Label runat="server" ID="lblAdminAmt" Text="金額" />
<%--                    <uc:ClsCMTextBox runat="server" ID="txtAdminAmt" ppName="管理者金額" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />--%>
                </td>
                <td style="width:100px;"></td>
                <td>
                    <asp:Label runat="server" ID="Label21" Text="人数" />
                </td>
                <td style="text-align: right; padding-right: 20px;width:100px;">
                    <asp:Label runat="server" ID="lblAdminCnt" Text="金額" />
<%--                    <uc:ClsCMTextBox runat="server" ID="txtAdminCnt" ppName="管理者人数" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />--%>
                </td>
                <td style="width:80px;"></td>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblAdminTotal" Text="\ 0" />
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label22" runat="server" Text="業務担当者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label23" Text="金額" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblStaffAmt" Text="金額" />
<%--                    <uc:ClsCMTextBox runat="server" ID="txtStaffAmt" ppName="業務担当者金額" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />--%>
                </td>
                <td style="width:100px;"></td>
                <td>
                    <asp:Label runat="server" ID="Label24" Text="人数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblStaffCnt" Text="金額" />
<%--                    <uc:ClsCMTextBox runat="server" ID="txtStaffCnt" ppName="業務担当者人数" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />--%>
                </td>
                <td style="width:80px;"></td>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblStaffTotal" Text="\ 0" />
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label25" runat="server" Text="システム運用管理者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label26" Text="金額" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblSystemAdminAmt" Text="金額" />
<%--                    <uc:ClsCMTextBox runat="server" ID="txtSystemAdminAmt" ppName="システム運用管理者金額" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />--%>
                </td>
                <td style="width:100px;"></td>
                <td>
                    <asp:Label runat="server" ID="Label27" Text="人数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblSystemAdminCnt" Text="人数" />
<%--                    <uc:ClsCMTextBox runat="server" ID="txtSystemAdminCnt" ppName="システム運用管理者人数" ppIMEMode="半角_変更不可" ppMaxLength="8"
                        ppNum="true" ppValidationGroup="Jinkenhi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />--%>
                </td>
                <td style="width:80px;"></td>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblSystemAdminTotal" Text="\ 0" />
                </td>
            </tr>
<%--            <tr>
                <td colspan="8">
                    <div class="float-right">
                        <asp:Button ID="BtnJinkenhiIns" runat="server" Text="マスタ変更" />
                    </div>
                    <div class="float-left" style="padding-bottom: 30px;">
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errortext" ValidationGroup="Jinkenhi" />
                    </div>
                </td>
            </tr>--%>
        </table>
    </asp:Panel>

    <%----------------------過去検収書の人件費表示用（通常はVisible=false）--------------------------%>
    <asp:Panel ID="pnlJinkenhiOld" runat="server" CssClass="center" BorderStyle="Solid" BorderWidth="1px" Width="95%" Visible="false">
        <table class="center" border="0" style="width: 90%">
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label28" runat="server" Text="管理者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label29" Text="金額" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldAdminAmt" Text="\ 0" />
                </td>
                <td>
                    <asp:Label runat="server" ID="Label30" Text="時間" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldAdminTime" Text="0時間" />
                <td>
                <td>
                    <asp:Label runat="server" ID="Label38" Text="日数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldAdminDay" Text="0日" />
                <td>
                <td>
                    <asp:Label runat="server" ID="Label40" Text="人数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldAdminCnt" Text="0人" />
                <td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldAdminTotal" Text="\ 0" />
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label31" runat="server" Text="業務担当者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label32" Text="金額" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldBusinessStaffAmt" Text="\ 0" />
                </td>
                <td>
                    <asp:Label runat="server" ID="Label34" Text="時間" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldBusinessStaffTime" Text="0時間" />
                <td>
                <td>
                    <asp:Label runat="server" ID="Label36" Text="日数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldBusinessStaffDay" Text="0日" />
                <td>
                <td>
                    <asp:Label runat="server" ID="Label43" Text="人数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldBusinessStaffCnt" Text="0人" />
                <td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldBusinessStaffTotal" Text="\ 0" />
                </td>
            </tr>
            <tr>
                <td style="height: 26px">
                    <asp:Label ID="Label45" runat="server" Text="故障受付担当者"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label46" Text="金額" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldTroubleStaffAmt" Text="\ 0" />
                </td>
                <td>
                    <asp:Label runat="server" ID="Label48" Text="時間" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldTroubleStaffTime" Text="0時間" />
                <td>
                <td>
                    <asp:Label runat="server" ID="Label50" Text="日数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldTroubleStaffDay" Text="0日" />
                <td>
                <td>
                    <asp:Label runat="server" ID="Label52" Text="人数" />
                </td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldTroubleStaffCnt" Text="0人" />
                <td>
                <td style="text-align: right; padding-right: 20px">
                    <asp:Label runat="server" ID="lblOldTroubleStaffTotal" Text="\ 0" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%----------------------実費登録欄--------------------------%>
    <div style="margin-top: 40px">
        <asp:Panel ID="pnlJippi" runat="server" CssClass="center" BorderStyle="Solid" BorderWidth="1px" Width="95%">
            <table class="center" border="0" style="width: 90%">
                <tr>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="停電時給電作業費（年１度）"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label17" Text="（１）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtKyuden1" ppName="給電作業費１" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label18" Text="（２）" />
                    </td>
                    <td colspan="5">
                        <uc:ClsCMTextBox runat="server" ID="txtKyuden2" ppName="給電作業費２" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label runat="server" ID="lblKyudenTotal" Text="\ 0" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="消耗品"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label11" Text="（１）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtSyomohin1" ppName="消耗品１" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label12" Text="（２）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtSyomohin2" ppName="消耗品２" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label13" Text="（３）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtSyomohin3" ppName="消耗品３" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label14" Text="（４）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtSyomohin4" ppName="消耗品４" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label runat="server" ID="lblSyomohinTotal" Text="\ 0" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="水道光熱費"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label6" Text="（１）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtKonetuhi1" ppName="水道光熱費１" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label15" Text="（２）" />
                    </td>
                    <td colspan="5">
                        <uc:ClsCMTextBox runat="server" ID="txtKonetuhi2" ppName="水道光熱費２" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label runat="server" ID="lblKonetuhiTotal" Text="\ 0" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="運送費"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label7" Text="（１）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtUnsohi1" ppName="運送費１" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label8" Text="（２）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtUnsohi2" ppName="運送費２" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label9" Text="（３）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtUnsohi3" ppName="運送費３" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label10" Text="（４）" />
                    </td>
                    <td>
                        <uc:ClsCMTextBox runat="server" ID="txtUnsohi4" ppName="運送費４" ppIMEMode="半角_変更不可" ppMaxLength="8" ppNum="true" ppValidationGroup="Jippi" ppCheckHan="true" ppNameVisible="false" ppTextAlign="右" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label runat="server" ID="lblUnsohiTotal" Text="\ 0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <div class="float-right">
                            <asp:Button ID="btnJippiIns" runat="server" Text="登録" />
                            &nbsp;
                        <asp:Button ID="btnJippiUpd" runat="server" Text="更新" />
                        </div>
                        <div class="float-left" style="padding-bottom: 30px;">
                            <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="Jippi" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <table class="center">
        <tr>
            <td>
                <div id="waitMsg" style="display: none; font-size: 18px; font-weight: bold;">
                    <asp:Label ID="lblWait" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="grid-out" style="height: 146px; width: 636px;">
                    <div class="grid-in" style="height: 146px; width: 636px;">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
