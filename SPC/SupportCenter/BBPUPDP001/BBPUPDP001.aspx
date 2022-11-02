<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="BBPUPDP001.aspx.vb" Inherits="SPC.BBPUPDP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table Style="height :50px">    </table>
    <table style ="width: 95%;border-collapse: collapse;height: 68px;">
        <tr>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;" >
                <asp:Label ID="lblBbinvst" runat="server" Text="ＢＢ調報Ｎｏ．"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblRepair_No" runat="server" Text="修理依頼Ｎｏ．"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblTbox_Id" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblNl" runat="server" Text="Ｎ／Ｌ"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblEw" runat="server" Text="東／西"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblHall" runat="server" Text="ホール名"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblTboxClass" runat="server" Text="ＴＢＯＸ種別"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;height: 13px;">
                <asp:Label ID="lblMente" runat="server" Text="型式番号"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="align-top" style="border: 1px solid #000000;padding-top: 3px;" >
                <asp:Label ID="lblBbinvst_No" runat="server" ></asp:Label>
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMTextBox ID="txtRepair_No" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="修理依頼Ｎｏ．" ppRequiredField="True" ppValidationGroup="1" ppCheckHan="True" ppCheckLength="True" />
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMTextBox ID="txtTbox_Id" runat="server" ppNameVisible="False" ppEnabled="True" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppRequiredField="True" ppValidationGroup="1" ppNum="True" />
            </td>
            <td class="align-top" style="border: 1px solid #000000;padding-top: 3px;" >
                <div class="align-top">
                <asp:Label ID="lblNl_Cls" runat="server" ></asp:Label>
                </div>
            </td>
            <td class="align-top" style="border: 1px solid #000000;padding-top: 3px;" >
                <asp:Label ID="lblEw_Flg" runat="server" ></asp:Label>
            </td>
            <td class="align-top" style="border: 1px solid #000000;padding-top: 3px;" >
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtHall_Nm" runat="server" ppNameVisible="False" ppEnabled="True" ppMaxLength="50" ppValidationGroup="1" ppName="ホール名" />
                <%--<asp:Label ID="lblHall_Nm" runat="server" ></asp:Label>--%>
                </div>
            </td>
            <td class="align-top" style="border: 1px solid #000000;padding-top: 3px;" >
                <asp:Label ID="lblTboxClass_Cd" runat="server" ></asp:Label>
            </td>
            <td class="align-top" style="border: 1px solid #000000" >
                <asp:DropDownList ID="ddlMente_No" runat="server" Width="130px" Enabled="true">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table Style="height :25px">  </table>
    <table  style ="width: 90%;border-collapse: collapse;height: 68px;border: 1px solid #000000;">
        <tr>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblSerial" runat="server" Text="シリアルＮｏ．"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblStatus" runat="server" Text="進捗状況"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblReceipt_D" runat="server" Text="受領日"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblWrk_D" runat="server" Text="作業日"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblReport_D" runat="server" Text="報告日"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblBb1_Send_D" runat="server" Text="ＢＢ１送付日"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #8EB5E3;">
                <asp:Label ID="lblInspect_M" runat="server" Text="検収月"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMTextBox ID="txtSerial" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppRequiredField="True" ppValidationGroup="1" ppName="シリアルNo" ppMaxLength="16" />
            </td>
            <td class="align-top" style="border: 1px solid #000000;padding-top: 2px;" >
                <asp:DropDownList ID="ddlStatus" runat="server" Width="100px" Enabled="False">
                </asp:DropDownList>
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMDateBox ID="dtbReceipt_D" runat="server" ppNameVisible="False" ppEnabled="False" ppValidationGroup="1" ppName="受領日" />
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMDateBox ID="dtbWrk_D" runat="server" ppNameVisible="False" ppEnabled="False" ppValidationGroup="1" ppName="作業日" />
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMDateBox ID="dtbReport_D" runat="server" ppNameVisible="False" ppEnabled="False" ppValidationGroup="1" ppName="報告日" />
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMDateBox ID="dtbBb1_send_D" runat="server" ppNameVisible="False" ppEnabled="False" ppValidationGroup="1" ppName="BB1送付日" />
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMTextBox ID="txtInspect_M" runat="server" ppNameVisible="False" ppEnabled="False" ppCheckLength="True" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="検収月" />
            </td>
        </tr>
    </table>
    <table Style="height :25px"></table>
    <table style ="border-collapse: collapse;">
        <tr>
            <td style="border: 1px solid #000000; background-color: #FFFF99;">
                <asp:Label ID="lblAccDt" runat="server" Text="事故発生日"></asp:Label>
            </td>
            <td style="border: 1px solid #000000; background-color: #FFFF99;">
                <asp:Label ID="lblBb_Cls" runat="server" Text="ＢＢ種別名"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMDateBox ID="dtbAccDt" runat="server" ppNameVisible="False" ppEnabled="False" ppValidationGroup="1" ppName="事故発生日" />
            </td>
            <td class="align-top" style="border: 1px solid #000000">
                <uc:ClsCMTextBox ID="txtBb_Cls" runat="server" ppNameVisible="False" ppEnabled="False" ppMaxLength="10" ppValidationGroup="1" ppName="ＢＢ種別名" />
            </td>
        </tr>
    </table>
    <table Style="height :25px">  </table>
    <table style="border-collapse: collapse;">
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;"rowspan="3">
                <div class="text-center">
                <asp:Label ID="Label25" runat="server" Text="読出ＪＢ番号"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                <asp:Label ID="lblJb_No1" runat="server" Text="１回目"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtJb_No1" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppValidationGroup="1" ppName="読出JB番号1回目" />
                </div>
            </td>
            <td style="width: 30px;"></td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">
                <div class="text-center">
                <asp:Label ID="lblChecker" runat="server" Text="ＣＨＥＣＫＥＲ"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                <uc:ClsCMTextBox runat="server" ID="txtChecker" ppNameVisible="False" ppWidth="96" ppMaxLength="5" ppIMEMode="半角_変更不可" ppEnabled="False" ppValidationGroup="1" />
                </div>
            </td>
            <td></td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" >
                <div class="text-center">
                <asp:Label ID="lblResult" runat="server" Text="店内集信結果" Enabled="False"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" >
                <div class="text-center">
                <asp:DropDownList ID="ddlResult" runat="server" Width="100px" Enabled="False">
                </asp:DropDownList>
                </div>
            </td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                <asp:Label ID="lblJb_No2" runat="server" Text="２回目"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtJb_No2" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppValidationGroup="1" ppName="読出JB番号2回目" />
                </div>
            </td>
            <td></td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">
                <div class="text-center">
                <asp:Label ID="lblLed_Flg" runat="server" Text="ＬＥＤ点灯"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                <uc:ClsCMDropDownList runat="server" ID="ddlLed_Flg" ppWidth="100" ppEnabled="False" ppNameVisible="False" ppClassCD="0029" ppNotSelect="True" ppValidationGroup="1" />
                </div>
            </td>
            <td style="width:30px"></td>
            <td style ="border: 1px solid #000000;"colspan="2">
                <div class="text-center">
                <%--<asp:Label ID="Label42" runat="server" Text="他店舗／正常終了"></asp:Label>--%>
                </div>
            </td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                <asp:Label ID="lblJb_No3" runat="server" Text="３回目"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtJb_No3" ppNameVisible="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppEnabled="False" ppValidationGroup="1" ppName="読出JB番号3回目" />
                </div>
            </td>
            <td style="width:30px"></td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">
                <div class="text-center">
                <asp:Label ID="lblDst_Flg" runat="server" Text="データ破棄完了"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                <uc:ClsCMDropDownList runat="server" ID="ddlDst_Flg" ppWidth="100" ppEnabled="False" ppNameVisible="False" ppClassCD="0029" ppNotSelect="True" ppValidationGroup="1" />
                </div>
            </td>
            <td style="width:30px"></td>
        </tr>
     </table>
     <table Style="height :25px">  </table>
     <table  style="width: 80%; border-collapse: collapse;">
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOlddt_Flg" runat="server" Text="日付古い"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;"rowspan="2">
                <div class="text-center" >
                <asp:CheckBox ID="chkOlddt_Flg" runat="server" Enabled="False" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblCrpt_Flg" runat="server" Text="データ化け"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;width:20px;"rowspan="2">
                <div class="text-center" >
                <asp:CheckBox ID="chkCrpt_Flg" runat="server" Enabled="False" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="4">
                <div class="text-center">
                <asp:Label ID="lblOth" runat="server" Text="その他"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;"rowspan="4">
                <asp:Panel ID="Panel3" runat="server">
                <div style="padding-left:3px">
                <asp:DropDownList ID="ddlOth_Cd" runat="server" Width="400px" Enabled="False">
                </asp:DropDownList>
                </div>
                </asp:Panel>
                <uc:ClsCMTextBox ID="txtOth_Free" runat="server" ppNameVisible="False" ppWidth="400" ppEnabled="False" ppMaxLength="40" ppValidationGroup="1" ppName="その他" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblBb1brk_Flg" runat="server" Text="ＢＢ１故障"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;"rowspan="2">
                <div class="text-center" >
                <asp:CheckBox ID="chkBb1brk_Flg" runat="server" Enabled="False" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblBrkCntnt" runat="server" Text="故障内容"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;"rowspan="2">
                <uc:ClsCMTextBox runat="server" ID="txtBrkCntnt" ppNameVisible="False" ppWidth="100" ppMaxLength="7" ppEnabled="False" ppValidationGroup="1" ppName="故障内容" />
                </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
     </table>
     <table Style="height :25px">  </table>
     <table  style="width: 80%;border-collapse: collapse;">
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">
                <div class="text-center">
                <asp:Label ID="lblBf_Crct" runat="server" Text="訂正前"></asp:Label>
                </div></td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtBf_Crct" ppNameVisible="False" ppEnabled="False" ppMaxLength="40" ppValidationGroup="1" ppName="訂正前" />
                </div>
            </td>
            <td style =" width:30px"></td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">
                <div class="text-center">
                <asp:Label ID="lblAf_Crct" runat="server" Text="訂正後"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtAf_Crct" ppNameVisible="False" ppEnabled="False" ppMaxLength="40" ppValidationGroup="1" ppName="訂正後" />
                </div>
            </td>
            <td style =" width:30px"></td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">
                <div class="text-center">
                <asp:Label ID="lblRep_Req" runat="server" Text="修理依頼票"></asp:Label>
                </div></td>
            <td style ="border: 1px solid #000000;">
                <uc:ClsCMDropDownList runat="server" ID="ddlRep_Req" ppWidth="100" ppEnabled="False" ppNameVisible="False" ppClassCD="0079" ppNotSelect="True" ppMode="名称" ppName="修理依頼票" ppValidationGroup="1" />
            </td>
        </tr>
     </table>
     <table border="0" style="width:80%;">
        <tr>
            <td>
                <div class="float-left">
                <asp:Label ID="Label49" runat="server" Text="ＩＣ系" Width="110px" Font-Bold="True" ForeColor="Red"></asp:Label>
                </div>
            </td>
        </tr>
     </table>
     <table  style="width: 45%; border-collapse: collapse;">
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">                
                <div class="text-center">
                    <asp:Label ID="lblFsi" runat="server" Text="ＢＢ負債額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">                
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtFsi" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="6" ppValidationGroup="1" ppName="BB負債額" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">                
                <div class="text-center">
                    <asp:Label ID="lblSim" runat="server" Text="ＢＢ債務額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">                
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtSim" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="6" ppValidationGroup="1" ppName="BB債務額" />
                </div>
            </td>
        </tr>
     </table>
     <table border="0" style="width:45%;">
        <tr>
            <td>
                <div class="float-left">
                <asp:Label ID="Label52" runat="server" Text="ＩＤ系" Width="110px" Font-Bold="True" ForeColor="Red"></asp:Label>
                </div>
            </td>
        </tr>
    </table>
     <table  style="width: 40%; border-collapse: collapse;">
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">                
                <div class="text-center">
                    <asp:Label ID="lblMng_Dt" runat="server" Text="管理日付"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">                
                <div class="text-center">
                <uc:ClsCMDateBox runat="server" ID="dtbMng_Dt" ppNameVisible="False" ppEnabled="False" ppValidationGroup="1" ppName="管理日付" ppText="管理日付" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;">                
                <div class="text-center">
                    <asp:Label ID="lblBb_No" runat="server" Text="ＢＢＮo."></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;">                
                <div class="text-center">
                    <uc:ClsCMTextBox runat="server" ID="txtBb_No" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppValidationGroup="1" ppName="BBNo" />
                </div>
            </td>
        </tr>
     </table>
     <table Style="height :25px">  </table>
     <table style="width: 95%;border-collapse: collapse;">
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" colspan ="4">
                <div class="text-center">
                <asp:Label ID="Label67" runat="server" Text="ＪＢアドレス"></asp:Label>
                </div>
            </td>
            <td style =" width:30px"></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td style =" width:30px"></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
             <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgN_No" runat="server" Text="正機番"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2" >
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtOrgn_No" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="正機番" />
                </div>
            </td>
             <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_No" runat="server" Text="副機番"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtDpl_No" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="副機番" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblTotal" runat="server" Text="ＢＢ累計金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblBbTotal" runat="server" ></asp:Label>
                </div>
            </td>
            <td>
                <div class="text-center">
                <asp:Label ID="Label95" runat="server" Text="-"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblTbox_Bb" runat="server" Text="ＴＢＯＸ集信分"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtTbox_Bb" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="8" ppValidationGroup="1" />
                </div>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgn_Lno" runat="server" Text="左件数"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtOrgn_Lno" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="正機番左件数"  />
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_Lno" runat="server" Text="左件数"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtDpl_Lno" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="副機番左件数" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <div class="text-center">
                <asp:Label ID="Label96" runat="server" Text="="></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label63" runat="server" Text="補償金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblComMoney" runat="server" Text="9999999"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgn_Rno" runat="server" Text="右件数"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtOrgn_Rno" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="正機番右件数" />
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_Rno" runat="server" Text="右件数"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtDpl_Rno" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppValidationGroup="1" ppName="副機番右件数" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label79" runat="server" Text="左金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgn_Lm" runat="server" ></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label80" runat="server" Text="左金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_Lm" runat="server" ></asp:Label>
                </div>
            </td>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label82" runat="server" Text="ミニカード入金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblCardPay" runat="server" ></asp:Label>
                </div>
            </td>
            <td>
                <div class="text-center">
                <asp:Label ID="Label97" runat="server" Text="-"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblTbox_Receipt" runat="server" Text="ＴＢＯＸ集信分"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtTbox_Receipt" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="8" ppValidationGroup="1" />
                </div>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label72" runat="server" Text="右金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgn_Rm" runat="server"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label78" runat="server" Text="右金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_Rm" runat="server" ></asp:Label>
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <div class="text-center">
                <asp:Label ID="Label98" runat="server" Text="="></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label68" runat="server" Text="未集信額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblNot_Col1" runat="server" ></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" colspan="4">
                <div class="text-center">
                <asp:Label ID="Label88" runat="server" Text="予備金額"></asp:Label>
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgn_Pay" runat="server" Text="ミニカード入金金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtOrgn_Pay" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="7" ppNum="True" ppValidationGroup="1" ppName="正機番ミニカード入金金額" />
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_Pay" runat="server" Text="ミニカード入金金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtDpl_Pay" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="7" ppNum="True" ppValidationGroup="1" ppName="副機番ミニカード入金金額" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label76" runat="server" Text="ミニカード消費額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblCardComsumer" runat="server"></asp:Label>
                </div>
            </td>
            <td>
                <div class="text-center">
                <asp:Label ID="Label99" runat="server" Text="-"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblTbox_Cnsmp" runat="server" Text="ＴＢＯＸ集信分"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtTbox_Cnsmp" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="8" ppValidationGroup="1" />
                </div>
            </td>
        </tr>
        <tr>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblOrgn_Consumer" runat="server" Text="ミニカード消費金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtOrgn_Consumer" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="7" ppNum="True" ppValidationGroup="1" ppName="正機番ミニカード消費金額" />
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblDpl_Consumer" runat="server" Text="ミニカード消費金額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <uc:ClsCMTextBox ID="txtDpl_Consumer" runat="server" ppNameVisible="False" ppEnabled="False" ppIMEMode="半角_変更不可" ppMaxLength="7" ppNum="True" ppValidationGroup="1" ppName="副機番ミニカード消費金額" />
                </div>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <div class="text-center">
                <asp:Label ID="Label100" runat="server" Text="="></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000; background-color: #FFFF99;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="Label101" runat="server" Text="未集信額"></asp:Label>
                </div>
            </td>
            <td style ="border: 1px solid #000000;" rowspan="2">
                <div class="text-center">
                <asp:Label ID="lblNot_Col2" runat="server" ></asp:Label>
                </div>
            </td>
        </tr>

        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
     </table>
     <table Style="height :25px">  </table>
     <table border="0" style="width:1050px;">
        <tr>
            <td>
                <div class="float-left">
                <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
                </div>
            </td>
            <td>
                <div class="float-right">
<%--                    <asp:Button ID="btnSearchRigth5" runat="server" Text="Button" Visible="False" />
                    &nbsp;<asp:Button ID="btnSearchRigth4" runat="server" Text="Button" Visible="False" />
                    &nbsp;<asp:Button ID="btnSearchRigth3" runat="server" Text="Button" Visible="False" />
                    &nbsp;<asp:Button ID="btnReset" runat="server" Text="元に戻す" Enabled="False" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="印刷" Enabled="False" />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" Text="削除" Enabled="False" />
                    &nbsp;<asp:Button ID="btnUpdate" runat="server" Text="更新" Enabled="False" ValidationGroup="1" />--%>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
