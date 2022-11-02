<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="QUAUPDP001.aspx.vb" Inherits="SPC.QUAUPDP001" MaintainScrollPositionOnPostBack="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" class="center" border="0">
        <tr>
            <!--報告日-->
            <td colspan="2">
                <uc:ClsCMDateBoxFromTo ID="dtbReportDt" runat="server" ppName="報告日" ppNameWidth="70" />
            </td>
        </tr>
        <tr>
            <!--種別（検索条件）-->
            <td style="width: 250px">
                <uc:ClsCMDropDownList runat="server" ID="ddlTboxType" ppName="種　別" ppClassCD="0105"
                    ppNameWidth="70" ppWidth="130" ppNotSelect="true" ppRequiredField="true" />
            </td>
            <!--提出区分-->
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlCdnSubmsnCls" ppName="提出区分" ppClassCD="0106"
                    ppNameWidth="70" ppWidth="100" ppNotSelect="true" />
            </td>
        </tr>

    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <script type="text/javascript">
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

    </script>
   <%-- <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
        .auto-style2 {
            height: 44px;
        }
    </style>--%>
    <hr/>
    <table style="width:1000px;" border="0" >
        <!--種別（検索結果）-->
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="padding-left: 5px">
                            <asp:Label ID="Label1" runat="server" Text="種　別"></asp:Label>
                        </td>
                        <td style="padding-left: 30px">
                            <asp:Label ID="lblClass" runat="server" Text="ＮＮＮＮＮＮＮＮＮＮＮＮＮ"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderWidth="1" >
                    <br/>
                    <table>
                        <tr style="vertical-align: top;">
                            <td>
                                <table border="1" style="width:100%; border-collapse: collapse;" >
                                    <!--ヘッダー（１行目）-->
                                    <tr>
                                        <!--項番-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label2" runat="server" Text="　項番" Width="110"></asp:Label>
                                        </td>
                                        <!--管理番号-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label3" runat="server" Text="　管理番号" Width="110"></asp:Label>
                                        </td>
                                        <!--ＴＢＯＸＩＤ-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label4" runat="server" Text="　ＴＢＯＸＩＤ" Width="100"></asp:Label>
                                        </td>
                                        <!--ホール名-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label6" runat="server" Text="　ホール名" Width="100"></asp:Label>
                                        </td>
                                        <!--ＮＬ区分（名称）-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label8" runat="server" Text="　ＮＬ区分" Width="80"></asp:Label>
                                        </td>
                                        <!--マスタ開始日-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label9" runat="server" Text="　マスタ開始日" ></asp:Label>
                                        </td>
                                        <!--ＶＥＲ-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label5" runat="server" Text="　ＶＥＲ" Width="100"></asp:Label>
                                        </td>         
                                    </tr>
                                    <!--ヘッダー（２行目）-->
                                    <tr>
                                        <!--受付日時-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label7" runat="server" Text="　受付日時" ></asp:Label>
                                        </td>
                                        <!--回復日時-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label10" runat="server" Text="　回復日時" ></asp:Label>
                                        </td>
                                        <!--サンド入金停止時間-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label11" runat="server" Text="　サンド入金停止時間" ></asp:Label>
                                        </td>
                                        <!--申告元-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label12" runat="server" Text="　申告元" ></asp:Label>
                                        </td>
                                        <!--調査完了日-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label18" runat="server" Text="　調査完了日" ></asp:Label>
                                        </td>
                                        <!--ステータス-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label20" runat="server" Text="　ステータス" ></asp:Label>
                                        </td>
                                        <!--提出-->
                                        <td style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label15" runat="server" Text="　提出区分" ></asp:Label>
                                        </td>
                                    </tr>
                                    <!--ヘッダー（３行目）-->
                                    <tr>
                                         <!--申告内容-->
                                        <td colspan="7" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label13" runat="server" Text="　申告内容" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <!--事象-->
                                        <td colspan="5" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label14" runat="server" Text="　事象" ></asp:Label>
                                        </td>
                                        <!--故障部位-->
                                        <td colspan="2" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label17" runat="server" Text="　故障部位" ></asp:Label>
                                        </td>
                                    </tr>
                                    <!--明細（１行目）-->
                                    <tr>
                                        <!--項番-->
                                        <td>
                                            <asp:Label ID="lblNo" runat="server" Text="　項番" ></asp:Label>
                                        </td>
                                        <!--管理番号、枝番-->
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <uc:ClsCMTextBox runat="server" ID="txtKanriNo" ppName="管理番号"
                                                            ppNameVisible="false" ppCheckHan="true" ppMaxLength="12" ppWidth="100"
                                                            ppRequiredField="true" ppValidationGroup="Detail"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <!--ＴＢＯＸＩＤ-->
                                        <td>
                                            <asp:Label ID="lblTboxId" runat="server" ></asp:Label>
                                        </td>
                                        <!--ホール名-->
                                        <td>
                                            <asp:Label ID="lblHallNm" runat="server" ></asp:Label>
                                        </td>
                                        <!--ＮＬ区分（名称）-->
                                        <td>
                                            <asp:Label ID="lblNLClsNm" runat="server" ></asp:Label>
                                        </td>
                                        <!--マスタ開始日-->
                                        <td>
                                            <asp:Label ID="lblStartDt" runat="server" Width="105"></asp:Label>
                                        </td>
                                        <!--ＶＥＲ-->
                                        <td>
                                            <asp:Label ID="lblVer" runat="server" ></asp:Label>
                                        </td>
                                    </tr>
                                    <!--明細（２行目）-->
                                    <tr>
                                        <!--受付日時-->
                                        <td>
                                            <%--<asp:Label ID="lblUketukeDt" runat="server" ></asp:Label>--%>
                                            <uc:ClsCMDateTimeBox runat="server" ID="dtbUketukeDt" ppName="受付日時" ppNameVisible="false" ppValidationGroup="Detail" />
                                        </td>
                                        <!--回復日時-->
                                        <td>
                                            <%--<asp:Label ID="lblKaifukuDt" runat="server" ></asp:Label>--%>
                                            <uc:ClsCMDateTimeBox runat="server" ID="dtbKaifukuDt" ppName="回復日時" ppNameVisible="false" ppValidationGroup="Detail" />
                                        </td>
                                        <!--サンド入金停止時間-->
                                        <td>
                                            <uc:ClsCMTextBox runat="server" ID="txtStopTime" ppName="サンド入金停止時間"
                                                ppNameVisible="false" ppMaxLength="10" ppWidth="140"
                                                ppValidationGroup="Detail" ppIMEMode="全角" />
                                        </td>
                                        <!--申告元-->
                                        <td style="padding-left: 4px">
                                            <asp:DropDownList ID="ddlReport" runat="server" Width="200"
                                                ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <!--調査完了日-->
                                        <td>
                                           <uc:ClsCMDateBox runat="server" ID="dtbChousaEnd" ppName="調査完了日"
                                               ppNameVisible="False" ppValidationGroup="Detail"/>
                                        </td>
                                        <!--ステータス-->
                                        <td style="padding-left: 4px">
                                            <asp:DropDownList ID="ddlStatusCd" runat="server" Width="90"
                                                ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <!--提出-->
                                        <td>
                                            <uc:ClsCMDropDownList runat="server" ID="ddlRstSubmsnCls" ppName="提出区分"
                                                ppClassCD="0106" ppNameVisible="false" ppWidth="90" ppNotSelect="True"
                                                ppValidationGroup="Detail"/>
                                        </td>
                                    </tr>
                                    <!--明細（３行目）-->
                                    <tr>
                                        <!--申告内容-->
                                        <td colspan="7">
                                            <uc:ClsCMTextBox runat="server" ID="txtReportCon" ppName="申告内容"
                                                ppNameVisible="false" ppMaxLength="100" ppWidth="1142"
                                                ppValidationGroup="Detail" ppIMEMode="全角" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <!--事象-->
                                        <td colspan="5" style="padding-left: 4px">
                                            <asp:DropDownList ID="ddlEvent" runat="server" Width="500"
                                                ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <!--故障部位-->
                                        <td colspan="2">
                                            <uc:ClsCMTextBox runat="server" ID="txtBuiKoshou" ppName="故障部位"
                                                ppNameVisible="false" ppMaxLength="10" ppWidth="140"
                                                ppValidationGroup="Detail" ppIMEMode="全角" />
                                        </td>
                                    </tr>
                                    <!--ヘッダー（４行目）-->
                                    <tr>
                                        <!--対応内容-->
                                        <td colspan="7" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label21" runat="server" Text="　対応内容" ></asp:Label>
                                        </td>
                                    </tr>
                                    <!--明細（４行目）-->
                                    <tr>
                                        <!--対応内容-->
                                        <%--<td colspan="7">
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect" ppName="対応内容"
                                                ppNameVisible="false" ppMaxLength="100" ppWidth="1142"
                                                ppValidationGroup="Detail" />
                                        </td>--%>
                                        <td colspan="3">
                                            <asp:Label ID="Label23" runat="server" Text="１行目～３行目" ></asp:Label>
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect" ppName="対応内容"
                                                ppNameVisible="false" ppMaxLength="30" ppWidth="400"
                                                ppValidationGroup="Detail" ppIMEMode="全角" />
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect2" ppName="対応内容"
                                                ppNameVisible="false" ppMaxLength="30" ppWidth="400"
                                                ppValidationGroup="Detail" ppIMEMode="全角" />
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect3" ppName="対応内容"
                                                ppNameVisible="false" ppMaxLength="30" ppWidth="400"
                                                ppValidationGroup="Detail" ppIMEMode="全角" />
                                        </td>
                                        <td colspan="4">
                                            <asp:Label ID="Label26" runat="server" Text="４行目～６行目" ></asp:Label>
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect4" ppName="対応内容"
                                                    ppNameVisible="false" ppMaxLength="30" ppWidth="400"
                                                    ppValidationGroup="Detail" ppIMEMode="全角" />
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect5" ppName="対応内容"
                                                    ppNameVisible="false" ppMaxLength="30" ppWidth="400"
                                                    ppValidationGroup="Detail" ppIMEMode="全角" />
                                            <uc:ClsCMTextBox runat="server" ID="txtRepect6" ppName="対応内容"
                                                    ppNameVisible="false" ppMaxLength="30" ppWidth="400"
                                                    ppValidationGroup="Detail" ppIMEMode="全角" />    
                                        </td>
                                    </tr>
                                    <!--ヘッダー-->
                                    <tr>
                                        <!--製品名-->
                                        <td colspan="1" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label16" runat="server" Text="　製品名" ></asp:Label>
                                        </td>
                                        <!--一時診断結果-->
                                        <td colspan="1" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label19" runat="server" Text="　一時診断結果" ></asp:Label>
                                        </td>
                                        <!--調査及び調査状況-->
                                        <td colspan="5" style="background-color: #8DB4E2; ">
                                            <asp:Label ID="Label22" runat="server" Text="　調査及び調査状況" ></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <asp:HiddenField ID="hdnSeq1" runat="server" />
                                            <asp:HiddenField ID="hdnEdaban1" runat="server" />
                                            <asp:Radiobutton ID="rdbEdaban1" runat="server" GroupName="Detail" ></asp:Radiobutton>
                                            <asp:DropDownList ID="ddlKosyoBui1" runat="server" Width="120" ValidationGroup="Detail"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlKosyoBui1Err1" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui1" runat="server" ControlToValidate="ddlKosyoBui1" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlKosyoBui1Err11" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui11" runat="server" ControlToValidate="ddlKosyoBui1" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="_Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <!--一時診断結果-->
                                        <td colspan="1">
                                            <asp:DropDownList ID="ddlIchijiShindan1" runat="server" Width="90" ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <!--調査及び調査状況-->
                                        <td colspan="5">
                                            <asp:TextBox ID="txtChousaJyokyou1" runat="server" Rows="3" Columns="100" MaxLength="200" TextMode="MultiLine" Width="810px" CssClass="ime-active"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <asp:HiddenField ID="hdnSeq2" runat="server" />
                                            <asp:HiddenField ID="hdnEdaban2" runat="server" />
                                            <asp:Radiobutton ID="rdbEdaban2" runat="server" GroupName="Detail" ></asp:Radiobutton>
                                            <asp:DropDownList ID="ddlKosyoBui2" runat="server" Width="120" ValidationGroup="Detail"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlKosyoBui1Err2" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui2" runat="server" ControlToValidate="ddlKosyoBui2" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlKosyoBui1Err22" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui22" runat="server" ControlToValidate="ddlKosyoBui2" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="_Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <!--一時診断結果-->
                                        <td colspan="1">
                                            <asp:DropDownList ID="ddlIchijiShindan2" runat="server" Width="90" ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtChousaJyokyou2" runat="server" Rows="3" Columns="100" MaxLength="200" TextMode="MultiLine" Width="810px" CssClass="ime-active"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <asp:HiddenField ID="hdnSeq3" runat="server" />
                                            <asp:HiddenField ID="hdnEdaban3" runat="server" />
                                            <asp:Radiobutton ID="rdbEdaban3" runat="server" GroupName="Detail" ></asp:Radiobutton>
                                            <asp:DropDownList ID="ddlKosyoBui3" runat="server" Width="120" ValidationGroup="Detail"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlKosyoBui1Err3" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui3" runat="server" ControlToValidate="ddlKosyoBui3" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlKosyoBui1Err33" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui33" runat="server" ControlToValidate="ddlKosyoBui3" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="_Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <!--一時診断結果-->
                                        <td colspan="1">
                                            <asp:DropDownList ID="ddlIchijiShindan3" runat="server" Width="90" ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtChousaJyokyou3" runat="server" Rows="3" Columns="100" MaxLength="200" TextMode="MultiLine" Width="810px" CssClass="ime-active"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <asp:HiddenField ID="hdnSeq4" runat="server" />
                                            <asp:HiddenField ID="hdnEdaban4" runat="server" />
                                            <asp:Radiobutton ID="rdbEdaban4" runat="server" GroupName="Detail" ></asp:Radiobutton>
                                            <asp:DropDownList ID="ddlKosyoBui4" runat="server" Width="120" ValidationGroup="Detail"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlKosyoBui1Err4" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui4" runat="server" ControlToValidate="ddlKosyoBui4" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlKosyoBui1Err44" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui44" runat="server" ControlToValidate="ddlKosyoBui4" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="_Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <!--一時診断結果-->
                                        <td colspan="1">
                                            <asp:DropDownList ID="ddlIchijiShindan4" runat="server" Width="90" ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtChousaJyokyou4" runat="server" Rows="3" Columns="100" MaxLength="200" TextMode="MultiLine" Width="810px" CssClass="ime-active"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <asp:HiddenField ID="hdnSeq5" runat="server" />
                                            <asp:HiddenField ID="hdnEdaban5" runat="server" />
                                            <asp:Radiobutton ID="rdbEdaban5" runat="server" GroupName="Detail" ></asp:Radiobutton>
                                            <asp:DropDownList ID="ddlKosyoBui5" runat="server" Width="120" ValidationGroup="Detail"></asp:DropDownList>
                                            <div style="white-space: nowrap">
                                                <asp:Panel ID="pnlKosyoBui1Err5" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui5" runat="server" ControlToValidate="ddlKosyoBui5" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlKosyoBui1Err55" runat="server" Width="0px">
                                                    <asp:CustomValidator ID="cuvKosyoBui55" runat="server" ControlToValidate="ddlKosyoBui5" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="_Detail"></asp:CustomValidator>
                                                </asp:Panel>
                                            </div>
                                        </td>
                                        <!--一時診断結果-->
                                        <td colspan="1">
                                            <asp:DropDownList ID="ddlIchijiShindan5" runat="server" Width="90" ValidationGroup="Detail"></asp:DropDownList>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txtChousaJyokyou5" runat="server" Rows="3" Columns="100" MaxLength="200" TextMode="MultiLine" Width="810px" CssClass="ime-active"></asp:TextBox>
                                        </td>
                                    </tr> 
                                </table><br />
                                <div class="float-right" style="padding-right: 10px">
                                    <asp:Button ID="btnGetRep" runat="server" Text="修理データ取得" ValidationGroup="Detail" CausesValidation="false" />
                                    <asp:Button ID="btnRowAdd" runat="server" Text="行追加" ValidationGroup="_Detail" CausesValidation="true" />
                                    <asp:Button ID="btnRowDel" runat="server" Text="行削除" ValidationGroup="Detail" CausesValidation="false" />
                                    <asp:Button ID="btnRowUp" runat="server" Text="▲" ValidationGroup="Detail" CausesValidation="false" />
                                    <asp:Button ID="btnRowDn" runat="server" Text="▼" ValidationGroup="Detail" CausesValidation="false" />
                                </div>
                            </td>
                            <td>
                                <!--コマンドボタン-->
                                <table style="width: 65px;">
                                    <tr>
                                        <td class="float-right">
                                            <asp:Button ID="btnInsert" runat="server" Text="追加" ValidationGroup="Detail" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="float-right">
                                            <asp:Button ID="btnUpdate" runat="server" Text="更新" ValidationGroup="Detail" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="float-right">
                                            <asp:Button ID="btnDelete" runat="server" Text="削除" ValidationGroup="Detail" CausesValidation="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br/><br/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="float-right">
                                            <asp:Button ID="btnClear" runat="server" Text="クリア" ValidationGroup="Detail" CausesValidation="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <!--非表示項目-->
                                        <td>
                                            <!--子画面が開いているか閉じているか（0:閉じている、1:開いている）-->
                                            <asp:HiddenField ID ="hdnChildWindow" runat="server" />
                                            <!--子画面を開いたときに保持しておく管理番号-->
                                            <asp:HiddenField ID ="hdnKanriNo" runat="server" />
                                            <!--子画面を開いたときに保持しておくチェックボックスがついている行数-->
                                            <asp:HiddenField ID ="hdnCheckRow" runat="server" />

                                            <!--明細の追加とかのモードモード-->
                                            <asp:Label ID="lblMode" runat="server" Text="0" Visible="false"></asp:Label>
                                            <!--修理データ存在-->
                                            <asp:Label ID="lblExistsRp" runat="server" Text="0" Visible="false"></asp:Label>
                                            <!--種別コード-->
                                            <asp:Label ID="lblClassCd" runat="server" Text="XXXXXXXX" Visible="false"></asp:Label>
                                            <!--種別名称-->
                                            <asp:Label ID="lblClassNm" runat="server" Text="XXXXXXXX" Visible="false"></asp:Label>
                                            <!--ＮＬ区分（コード）-->
                                            <asp:Label ID="lblNLClsCd" runat="server" Text="XXXXXXXX" Visible="false"></asp:Label>
                                            <!--ホールコード-->
                                            <asp:Label ID="lblHallCd" runat="server" Text="XXXXXXXX" Visible="false"></asp:Label>
                                            <!--ホール住所-->
                                            <asp:Label ID="lblHallAdr" runat="server" Text="XXXXXXXX" Visible="false"></asp:Label>
                                            <!--開始日時（対応日）-->
                                            <asp:Label ID="lblDealDt" runat="server" Text="yyyy/MM/dd HH:mm" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errortext" ValidationGroup="Detail" />
    <asp:ValidationSummary ID="ValidationSummary11" runat="server" CssClass="errortext" ValidationGroup="_Detail" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <!--グリッド-->
    <div class="grid-out" style="height: 489px">
        <div class="grid-in" style="height: 489px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <br>
    <div class="float-right" style="padding-right: 10px">
        <asp:Button ID="btnListClear" runat="server" Text="明細クリア" CausesValidation="false" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
