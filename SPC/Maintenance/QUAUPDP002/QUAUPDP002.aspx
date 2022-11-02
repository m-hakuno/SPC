<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="QUAUPDP002.aspx.vb" Inherits="SPC.QUAUPDP002" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/UserControl/ClsCMDateTimeBox.ascx" TagPrefix="uc" TagName="ClsCMDateTimeBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript">
        function lenCheck(obj, size) {
            var strW = obj.value;
            var lenW = strW.length;
            if (size < lenW) {
                var limitS = strW.substring(0, size);
                obj.value = limitS;
            }
        }
        function closeWindow(seqid,branch,branchid,appacd,appacdid,result,resultid,content,contentid,mntno) {
            // 親ウィンドウの存在チェック
            if (!window.opener.closed) {

                var parent = window.opener;
                var seqtxt;
                var branchtxt;
                var appacdtxt;
                var resulttxt;
                var contenttxt;
                var flg = "0";
                if (parent.document.getElementById("cphMainContent_cphUpdateContent_rdbEdaban1").checked == true) {
                    seqid = "";

                    seqtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnSeq1");
                    branchtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnEdaban1");
                    appacdid = "cphMainContent_cphUpdateContent_ddlKosyoBui1";
                    resultid = "cphMainContent_cphUpdateContent_ddlIchijiShindan1";
                    contenttxt = parent.document.getElementById("cphMainContent_cphUpdateContent_txtChousaJyokyou1");
                    flg = "1";
                } else if (parent.document.getElementById("cphMainContent_cphUpdateContent_rdbEdaban2").checked == true) {
                    seqtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnSeq2");
                    branchtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnEdaban2");
                    appacdid = "cphMainContent_cphUpdateContent_ddlKosyoBui2";
                    resultid = "cphMainContent_cphUpdateContent_ddlIchijiShindan2";
                    contenttxt = parent.document.getElementById("cphMainContent_cphUpdateContent_txtChousaJyokyou2");
                    flg = "1";
                } else if (parent.document.getElementById("cphMainContent_cphUpdateContent_rdbEdaban3").checked == true) {
                    seqtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnSeq3");
                    branchtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnEdaban3");
                    appacdid = "cphMainContent_cphUpdateContent_ddlKosyoBui3";
                    resultid = "cphMainContent_cphUpdateContent_ddlIchijiShindan3";
                    contenttxt = parent.document.getElementById("cphMainContent_cphUpdateContent_txtChousaJyokyou3");
                    flg = "1";
                } else if (parent.document.getElementById("cphMainContent_cphUpdateContent_rdbEdaban4").checked == true) {
                    seqtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnSeq4");
                    branchtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnEdaban4");
                    appacdid = "cphMainContent_cphUpdateContent_ddlKosyoBui4";
                    resultid = "cphMainContent_cphUpdateContent_ddlIchijiShindan4";
                    contenttxt = parent.document.getElementById("cphMainContent_cphUpdateContent_txtChousaJyokyou4");
                    flg = "1";
                } else if (parent.document.getElementById("cphMainContent_cphUpdateContent_rdbEdaban5").checked == true) {
                    seqtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnSeq5");
                    branchtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnEdaban5");
                    appacdid = "cphMainContent_cphUpdateContent_ddlKosyoBui5";
                    resultid = "cphMainContent_cphUpdateContent_ddlIchijiShindan5";
                    contenttxt = parent.document.getElementById("cphMainContent_cphUpdateContent_txtChousaJyokyou5");
                    flg = "1";
                }
                //var seqtxt = parent.document.getElementById(seqid);
                //var branchtxt = parent.document.getElementById(branchid);
                //var appacdtxt = parent.document.getElementById(appacdid);
                //var resulttxt = parent.document.getElementById(resultid);
                //var contenttxt = parent.document.getElementById(contentid);
                if (mntno != parent.document.getElementById("cphMainContent_cphUpdateContent_txtKanriNo_txtTextBox").value) {
                    alert("管理番号が変更されている為、画面を終了します。");
                } else {
                    if (flg == "0") {
                        alert("行が選択されていない為、画面を終了します。");
                    } else {
                        seqtxt.value = "";
                        branchtxt.value = branch;

                        dropdownselect(appacdid, appacd)
                        dropdownselect(resultid, result)

                        contenttxt.innerText = content;
                    }
                }
            }
            else {
                alert("親ウィンドウが閉じられた為、画面を終了します。");
            }

            window.close();
        }
        // ドロップダウンリスト選択関数
        function dropdownselect(src, dst) {

            var parent = window.opener;
            var src_element = parent.document.getElementById(src);

            // 同期元の値を取得する
            var value = dst;
            // 同期先の同一値の項目を選択状態にする
            var flag = false;
            for (var i = 0; i < src_element.options.length; i++) {
                var option = src_element.options[i];

                // value が一致していれば選択状態にする
                if (option.value == value) {
                    // 同一値が2つ以上存在する場合は、最初に見つかった項目のみを
                    // 選択状態にするための措置
                    // 同一値が存在しない場合が明確な場合は、対応不要
                    if (flag) {
                        option.selected = false;
                    } else {
                        option.selected = true; 
                        flag = true;
                    }
                } else { 
                    option.selected = false; 
                }
            }
        }
        window.onunload = function () {
            unload()
        }
        
        function unload() {

            var parent = window.opener;
            if (!parent.closed) {
                var flgtxt = parent.document.getElementById("cphMainContent_cphUpdateContent_hdnChildWindow");
                if (flgtxt != null) {
                    flgtxt.value = "0";
                }
            }
        }
    </script>
    <style type="text/css">
        .auto-style3 {
            height: 16px;
        }
        .auto-style4 {
            height: 21px;
        }
        .auto-style5 {
            height: 23px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width:1050px; table-layout: fixed;" class="center" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlMnt1" runat="server" BorderStyle="Solid" BorderWidth="1">
                    <table border="0" style="width:100%">
                        <tr>
                            <!--管理番号-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="管理番号" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMntNo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ＴＢＯＸＩＤ-->
                            <td class="auto-style5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="TBOXID" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTboxID" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!--ホール名-->
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="ホール名" Width="100"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHallNm" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />

                <!--【対応明細（グリッド）】-->
                <div id="DivOut" runat="server" class="grid-out" style="width: 1040px;height: 300px">
                    <div id="DivIn" runat="server" class="grid-in" style="height: 300px">
                        <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                        <asp:GridView ID="grvList" runat="server">
                        </asp:GridView>
                    </div>
                </div>
                <asp:Button ID="btnUpdate" runat="server" Text="リロード" CssClass="float-right" />
                <br/>
            </td>
        </tr>
    </table>
</asp:Content>
