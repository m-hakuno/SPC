<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDMA8.aspx.vb" Inherits="SPC.COMUPDMA8" %>

<%@ MasterType VirtualPath="~/Master/Mst.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function pageLoad() {
            set_onloadscroll();
        }

        function calconblur(hour, min, day, time, rslt) {
            var wrkcode = document.getElementById("<%= txtWorkCd.ppTextBox.ClientID%>").value

            setzero(day, '0');
            setzero(time, '0');
            if (isNaN(day.value) == false && isNaN(time.value) == false) {
                if (hour.value >= 24 || min.value >= 60) {
                    rslt.innerText = ''
                } else if (wrkcode == '02' || wrkcode == '03') {
                    //深夜・終夜の時は動作時刻、その他は日数+時刻を表示
                    setzero(hour, '00');
                    setzero(min, '00');
                    if (isNaN(hour.value) == false && isNaN(min.value) == false) {
                        calcacttime(hour.value, min.value, day.value, time.value, rslt);
                    }
                } else {
                    calctime(day.value, time.value, rslt);
                }
            } else {
                rslt.innerText = ''
            }
        }

        function setzero(obj, str) {
            var num = obj.value.replace(/(^\s+)|(\s+$)/g, "");
            if (isNaN(num)) {
                if (num == '0') {
                    obj.value = String(str);
                }
            }
            else if (num == '') {
                obj.value = String(str);
            }
            else {
                obj.value = parseInt(obj.value);
                if (isNaN(obj.value)) {//小数入力
                    obj.value = String(str);
                } else if (obj.value == 0) {
                    //ゼロ埋め
                    var slicecnt = (String(str).length) * -1;
                    obj.value = (str + obj.value).slice(slicecnt);
                } else {
                    if (str == '00') {//時刻はゼロ埋め
                        var slicecnt = (String(str).length) * -1;
                        obj.value = (str + obj.value).slice(slicecnt);
                    } else {
                        obj.value = obj.value;
                    }
                }

            }
        }

        function calcacttime(hour, min, day, time, rslt) {
            // 初期値設定
            var msecPerMinute = 1000 * 60;
            var msecPerHour = msecPerMinute * 60;
            var msecPerDay = msecPerHour * 24;
            var minusflg = false

            // 基準日作成(2000/4/1の入力時刻(月は0から数える為、3=4月))
            var dt = new Date(2000, 3, 1, hour, min, 0, 0);
            var dayofm = dt.getDate();
            var minofh = dt.getMinutes();

            dt.setDate(dayofm + parseInt(day));
            dt.setMinutes(minofh + parseInt(time));

            // 日付差分取得の為に時刻のみ同一の基準日を作成
            var maindt = new Date(2000, 3, 1, dt.getHours(), dt.getMinutes(), 0, 0)
            var main = maindt.getTime();
            // 日付部分の差分を取得
            var interval = dt.getTime() - main;

            // マイナス対応
            if (interval < 0) {
                minusflg = true;
                interval *= -1;
            }

            // ゼロ対応
            if (interval == 0) {
                //日付変更無し
                disptime(0, dt.getHours(), dt.getMinutes(), minusflg, rslt)
                return
            }

            // 時刻取得
            var rtnhour = dt.getHours();
            var rtnmin = dt.getMinutes();

            // 日数差取得
            var rtndays = Math.abs(Math.floor(interval / msecPerDay));

            disptime(rtndays, rtnhour, rtnmin, minusflg, rslt)

        }

        function calctime(day, time, rslt) {
            // 初期値設定
            var msecPerMinute = 1000 * 60;
            var msecPerHour = msecPerMinute * 60;
            var msecPerDay = msecPerHour * 24;
            var minusflg = false

            // ミリ秒に変換
            time = day * msecPerDay + time * msecPerMinute;

            // ゼロ対応
            if (time == 0) {
                rslt.innerText = '';
                return
            }

            // マイナス対応
            var interval = time;
            if (time < 0) {
                minusflg = true;
                interval *= -1;
            }

            // ミリ秒→日時分
            var minutes = Math.floor(interval / msecPerMinute);
            var hours = Math.floor(minutes / 60);
            var days = Math.floor(hours / 24);

            // 各単位超過分を再計算(例：30時間→1日+6時間)
            hours = hours - days * 24;
            minutes = minutes - ((days * 24 + hours) * 60);

            disptotal(hours, days, minutes, minusflg, rslt)

        }

        function disptotal(hours, days, minutes, minusflg, rslt) {
            if (days != 0) {
                days = String(days) + "日"
            } else days = ''
            if (hours != 0) {
                hours = String(hours) + "時間"
            } else hours = ''
            if (minutes != 0) {
                minutes = String(minutes) + "分"
            } else minutes = ''

            // 画面表示
            if (minusflg) {
                rslt.innerText = days + hours + minutes + "前";
            } else {
                rslt.innerText = days + hours + minutes + "後";
            }

        }

        function disptime(day, hour, min, minusflg, rslt) {
            if (day != 0) {
                if (minusflg) {
                    day = String(day) + "日前 "
                } else {
                    day = String(day) + "日後 "
                }
            } else day = '当日 '
            hour = ("0" + String(hour)).slice(-2) + ":"
            min = ("0" + String(min)).slice(-2)

            // 画面表示
            rslt.innerText = day + hour + min;

        }

        function deletezero(obj) {
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

        function focusChange(btnDmy, txtBox) {
            btnDmy.style.display = "none";
            txtBox.focus();
        }

    </script>
</asp:Content>

<asp:Content ID="searchcontent" ContentPlaceHolderID="SearchContent" runat="server">
    <table style="width: 700px; margin-left: auto; margin-right: auto; border: none; text-align: left;">
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlSWork" runat="server" ppName="設定依頼内容"
                    ppNameWidth="80" ppWidth="350" ppClassCD="0000" ppNotSelect="true" ppValidationGroup="search" ppRequiredField="true" />
            </td>
            <td>
                <uc:ClsCMDropDownList ID="ddlSSystemCls" runat="server" ppName="分類"
                    ppNameWidth="60" ppWidth="160" ppClassCD="0000" ppValidationGroup="search" ppRequiredField="true" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 900px; margin-right: auto; margin-left: auto; border: none; text-align: left;">
        <tr>
            <td style="width: 90px;">
                <asp:Label ID="lblirainaiyou" runat="server" Text="設定依頼内容" Width="80px"></asp:Label>
            </td>
            <td style="width: 30px;">
                <uc:ClsCMTextBox ID="txtWorkCd" runat="server" ppName="設定依頼内容コード" ppNameVisible="false"
                    ppNameWidth="0" ppIMEMode="半角_変更不可" ppWidth="20" ppMaxLength="2" ppValidationGroup="val" ppCheckHan="true" ppNum="true" ppEnabled="false" />
            </td>
            <td style="width:330px;">
                <uc:ClsCMTextBox ID="txtWork" runat="server" ppName="設定依頼内容" ppNameVisible="false" ppRequiredField="true"
                    ppNameWidth="0" ppIMEMode="全角" ppWidth="300" ppMaxLength="50" ppValidationGroup="val" />
            </td>
            <td style="width:40px;">
                <asp:Label ID="Label1" runat="server" Text="分類" Width="30px"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblSyscls" runat="server" Text="" Width="80px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 40px">
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lbl1_1" runat="server" Text="電文依頼項目１" Width="100px"></asp:Label>
                        </td>
                        <td style="width: 30px;">
                            <uc:ClsCMTextBox ID="txtNameCd1" runat="server" ppName="設定項目名１コード" ppNameVisible="false" ppEnabled="false"
                                ppNameWidth="0" ppIMEMode="半角_変更不可" ppWidth="20" ppCheckLength="true" ppMaxLength="2" ppValidationGroup="val" ppCheckHan="true" ppNum="true" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtName1" runat="server" ppName="電文依頼項目名１" ppNameVisible="false" ppRequiredField="true"
                                ppNameWidth="0" ppIMEMode="全角" ppWidth="600" ppMaxLength="100" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 180px">
                    <tr>
                        <td>
                            <asp:Label ID="lbl1_2" runat="server" Text="変更：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtCngTime1" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngDayRevision1" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngTimeRevision1" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblCng1" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl1_3" runat="server" Text="戻し：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtRtnTime1" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnDayRevision1" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnTimeRevision1" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblRtn1" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 40px">
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lbl2_1" runat="server" Text="電文依頼項目２" Width="100px"></asp:Label>
                        </td>
                        <td style="width: 30px;">
                            <uc:ClsCMTextBox ID="txtNameCd2" runat="server" ppName="設定項目名２コード" ppNameVisible="false" ppEnabled="false"
                                ppNameWidth="0" ppIMEMode="半角_変更不可" ppWidth="20" ppMaxLength="2" ppValidationGroup="val" ppCheckHan="true" ppNum="true" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtName2" runat="server" ppName="電文依頼項目名２" ppNameVisible="false" ppRequiredField="true"
                                ppNameWidth="0" ppIMEMode="全角" ppWidth="600" ppMaxLength="100" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 180px">
                    <tr>
                        <td>
                            <asp:Label ID="lbl2_2" runat="server" Text="変更：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtCngTime2" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngDayRevision2" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngTimeRevision2" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblCng2" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl2_3" runat="server" Text="戻し：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtRtnTime2" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnDayRevision2" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnTimeRevision2" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblRtn2" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 40px">
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lbl3_1" runat="server" Text="電文依頼項目３" Width="100px"></asp:Label>
                        </td>
                        <td style="width: 30px;">
                            <uc:ClsCMTextBox ID="txtNameCd3" runat="server" ppName="設定項目名３コード" ppNameVisible="false" ppEnabled="false"
                                ppNameWidth="0" ppIMEMode="半角_変更不可" ppWidth="20" ppMaxLength="2" ppValidationGroup="val" ppCheckHan="true" ppNum="true" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtName3" runat="server" ppName="電文依頼項目名３" ppNameVisible="false" ppRequiredField="true"
                                ppNameWidth="0" ppIMEMode="全角" ppWidth="600" ppMaxLength="100" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 180px">
                    <tr>
                        <td>
                            <asp:Label ID="lbl3_2" runat="server" Text="変更：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtCngTime3" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngDayRevision3" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngTimeRevision3" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblCng3" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl3_3" runat="server" Text="戻し：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtRtnTime3" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnDayRevision3" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnTimeRevision3" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblRtn3" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 40px">
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lbl4_1" runat="server" Text="電文依頼項目４" Width="100px"></asp:Label>
                        </td>
                        <td style="width: 30px;">
                            <uc:ClsCMTextBox ID="txtNameCd4" runat="server" ppName="設定項目名４コード" ppNameVisible="false" ppEnabled="false"
                                ppNameWidth="0" ppIMEMode="半角_変更不可" ppWidth="20" ppMaxLength="2" ppValidationGroup="val" ppCheckHan="true" ppNum="true" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtName4" runat="server" ppName="電文依頼項目名４" ppNameVisible="false" ppRequiredField="true"
                                ppNameWidth="0" ppIMEMode="全角" ppWidth="600" ppMaxLength="100" ppValidationGroup="val" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td colspan="5">
                <table style="padding-left: 180px">
                    <tr>
                        <td>
                            <asp:Label ID="lbl4_2" runat="server" Text="変更：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtCngTime4" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngDayRevision4" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtCngTimeRevision4" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblCng4" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl4_3" runat="server" Text="戻し：" Width="80px"></asp:Label>
                        </td>
                        <td>
                            <uc:ClsCMTimeBox ID="txtRtnTime4" runat="server" ppName="時間"
                                ppNameWidth="40" ppValidationGroup="val" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnDayRevision4" runat="server" ppName="日数補正(日)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="30" ppMaxLength="3" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td>
                            <uc:ClsCMTextBox ID="txtRtnTimeRevision4" runat="server" ppName="時間補正(分)"
                                ppNameWidth="80" ppIMEMode="半角_変更不可" ppWidth="50" ppMaxLength="5" ppValidationGroup="val" ppCheckHan="true" ppExpression="(^-\d+$)|(^\d+$)" />
                        </td>
                        <td style="text-align: right; width: 100px;">
                            <asp:Label ID="lblRtn4" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
        </tr>
    </table>
    <asp:HiddenField ID="hdnSyscls" runat="server" Value="" />
</asp:Content>

<asp:Content ID="Gridcontent" ContentPlaceHolderID="GridContent" runat="server">
    <%-- このページはグリッド無いです --%>
</asp:Content>
