<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="COMMENP006.aspx.vb" Inherits="SPC.COMMENP006" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/ctiexe.js")%>'></script>
    <script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/EnableChange.js")%>'></script>
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
    <style type="text/css">
        .auto-style1 {
        }
        .auto-style2 {
            width: 45px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="PnlEnabled1" runat="server">
    <table style="width: 1060px;" border="0" class="center">
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td style="width: 105px">
                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppName="ＴＢＯＸＩＤ" ppNameWidth="100" ppWidth="60" />
            </td>
            <td class="auto-style2">
                <asp:TextBox ID="tbxNLSec" runat="server" Width="20px"></asp:TextBox>
            </td>
            <td style="padding-left:30px;">
                <uc:ClsCMTextBox ID="txtSystem" runat="server" ppName="システム" ppNameWidth="70"  ppWidth="80" />
                <asp:HiddenField ID="hdnSystem" runat="server" />
            </td>
            <td class="auto-style1">
                <uc:ClsCMTextBox ID="txtVER" runat="server" ppName="ＶＥＲ" ppWidth="60" ppRequiredField="False" ppMaxLength="5" ppValidationGroup="1" ppCheckHan="True" />
            </td>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtTboxTel" runat="server" ppName="ＴＢＯＸＴＥＬ" ppNameWidth="100" ppWidth="80" />
            </td>
            <td colspan="2" style="padding-left:47px;">
                <uc:ClsCMTextBox ID="txtOptSttDate" runat="server" ppWidth="70" ppNameWidth="70" ppName="運用開始日" />
            </td>
        </tr>
        <tr runat="server" id="trHdn" >
            <td colspan="2">
                <uc:ClsCMDropDownList ID="ddlPerCls" runat="server" ppClassCD="0109" ppName="運用状況" ppNameWidth="100" ppNotSelect="False" ppValidationGroup="1" ppWidth="100" ppRequiredField="False" />
                <asp:HiddenField ID="hdnPerCls" runat="server" />
            </td>
            <td colspan="2" style="padding-left:30px;">

                <uc:ClsCMTextBox ID="txtWrkEndDate" runat="server" ppCheckHan="True" ppIMEMode="半角_変更不可" ppMaxLength="10" ppName="運用終了日" ppNameWidth="70" ppRequiredField="False" ppValidationGroup="1" ppWidth="70" ppCheckLength="True" />

                <asp:HiddenField ID="hdnWrkEndDate" runat="server" />

            </td>
            <td colspan="3">

                <asp:HiddenField ID="hdnSystemCls" runat="server" />

            </td>
            <td colspan="2" style="padding-left:47px;">

                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="4" style="width: 650px">
                <uc:ClsCMTextBox ID="txtHoleNm" runat="server" ppName="ホール名" ppNameWidth="100" ppWidth="520" ppIMEMode="全角" ppMaxLength="40" ppRequiredField="True" ppValidationGroup="1" />
            </td>
            <td style="width: 105px">
                <uc:ClsCMTextBox ID="txtHoleCd" runat="server" ppName="ホールコード" ppNameWidth="100" ppWidth="60" ppCheckHan="True" ppCheckLength="True" ppIMEMode="半角_変更不可" ppMaxLength="7" ppNum="True" ppRequiredField="True" ppValidationGroup="1" />
                <asp:HiddenField ID="hdnHallCd" runat="server" />
            </td>
            <td colspan="1">
                <asp:CheckBox ID="cbxHallCd" runat="server" Checked="True" />
            </td>
            <td></td>
            <td colspan="1" style="padding-left:47px;">
                <uc:ClsCMTextBox ID="txtCcrtDate" runat="server" ppName="集信日" ppNameWidth="70" ppWidth="70" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtZip" runat="server" ppName="郵便番号" ppNameWidth="100" ppWidth="60" ppIMEMode="半角_変更不可" ppMaxLength="8" ppRequiredField="False" ppValidationGroup="1" />
            </td>
            <td colspan="7">
                <uc:ClsCMTextBox ID="txtHoleAdd" runat="server" ppName="住所" ppNameWidth="40" ppWidth="770" ppRequiredField="True" ppValidationGroup="1" ClientIDMode="Inherit" ppCheckLength="False" ppMaxLength="100" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    <table style="width:1060px;" border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtHoleTel1" runat="server" ppName="ホールＴＥＬ１" ppNameWidth="100" ppWidth="100" />
            </td>
            <td>
                <span>
                    <asp:Button ID="btnTell1" runat="server" Text="発信" Width="50px" CausesValidation="false" />
                </span>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtFaxNo" runat="server" ppName="ＦＡＸ番号" ppNameWidth="70" ppWidth="100" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtEWSec" runat="server" ppName="ＥＷ区分" ppWidth="20" />
            </td>
            <td style="width:495px;"></td>
        </tr>
    </table>
    <table style="width:1060px;" border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtHoleTel2" runat="server" ppName="ホールＴＥＬ２" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppCheckLength="False" ppIMEMode="半角_変更不可" ppMaxLength="15" ppValidationGroup="1" ppNum="False" />
            </td>
            <td>
                <asp:DropDownList ID="DDLUseCd1" runat="server" Width="350px">
                </asp:DropDownList>
            </td>
            <td style="width: 8px;">&nbsp;</td>
            <td>
                <asp:Button ID="btnTell2" runat="server" Text="発信" Width="50px" CausesValidation="false" />
            </td>
            <td style="width: 130px;">&nbsp;</td>
            <td colspan="1" style="width: 110px;">
                <uc:ClsCMTextBox ID="txtMDNInstallation" runat="server" ppName="ＭＤＮ設置" ppWidth="20" ppNameWidth="80" />
            </td>
            <td style="width: 120px;">
                <uc:ClsCMTextBox ID="txtNumberOfMDN" runat="server" ppName="　　ＭＤＮ台数" ppWidth="40" ppNameWidth="101" ppTextAlign="右" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtHoleTel3" runat="server" ppName="ホールＴＥＬ３" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppCheckLength="False" ppIMEMode="半角_変更不可" ppMaxLength="15" ppValidationGroup="1" />
            </td>
            <td>
                <asp:DropDownList ID="DDLUseCd2" runat="server" Width="350px">
                </asp:DropDownList>
            </td>
            <td style="width: 8px;">&nbsp;</td>
            <td>
                <asp:Button ID="btnTell3" runat="server" Text="発信" Width="50px" CausesValidation="false" />
            </td>
            <td>&nbsp;</td>
            <td colspan="1">
                <uc:ClsCMTextBox ID="txtPrtNumberOfMDN" runat="server" ppName="親ＭＤＮ台数" ppWidth="40" ppNameWidth="80" ppTextAlign="右" />
            </td>
            <td colspan="1">
                <uc:ClsCMTextBox ID="txtChd1NumberOfMDN" runat="server" ppName="子１ＭＤＮ台数" ppWidth="40" ppNameWidth="101" ppTextAlign="右" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtHoleTel4" runat="server" ppName="ホールＴＥＬ４" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppCheckLength="False" ppIMEMode="半角_変更不可" ppMaxLength="15" ppValidationGroup="1" />
            </td>
            <td>
                <asp:DropDownList ID="DDLUseCd3" runat="server" Width="350px">
                </asp:DropDownList>
            </td>
            <td style="width: 8px;">&nbsp;</td>
            <td>
                <asp:Button ID="btnTell4" runat="server" Text="発信" Width="50px" CausesValidation="false" />
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <uc:ClsCMTextBox ID="txtChd2NumberOfMDN" runat="server" ppName="子２ＭＤＮ台数" ppWidth="40" ppNameWidth="101" ppTextAlign="右" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtHoleTel5" runat="server" ppName="ホールＴＥＬ５" ppNameWidth="100" ppWidth="100" ppCheckHan="True" ppCheckLength="False" ppIMEMode="半角_変更不可" ppMaxLength="15" ppValidationGroup="1" />
            </td>
            <td>
                <asp:DropDownList ID="DDLUseCd4" runat="server" Width="350px" >
                </asp:DropDownList>
            </td>
            <td style="width:8px;">&nbsp;</td>
            <td>
                <asp:Button ID="btnTell5" runat="server" Text="発信" Width="50px" CausesValidation="false" />
            </td>           
            <td>&nbsp;</td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtMDN" runat="server" ppName="ＭＤＮ機器名" ppWidth="200" ppNameWidth="80" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="PnlEnabled3" runat="server" >
    <table style="width:1060px;" border="0"  class="center" >
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtNotes1" runat="server" ppName="注意事項" ppNameWidth="80" ppWidth="280" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtNotes2" runat="server" ppNameVisible="False" ppWidth="280" />
            </td>
            <td>
                <asp:TextBox ID="tbxStoreType" runat="server" Width="50px"></asp:TextBox>
            </td>
            <td style="width:310px;">&nbsp;</td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMTextBox ID="txtNotes3" runat="server" ppName="　" ppNameWidth="80" ppWidth="280" ppNameVisible="True" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtNotes4" runat="server" ppNameVisible="False" ppWidth="280" />
            </td>
            <td colspan="2">&nbsp;</td>
        </tr>
        </table>
        </asp:Panel>
    <table style="width:1060px;" border="0" class="center">      
        <tr>
            <td rowspan="3" class="align-top">
                <asp:Panel ID="PnlEnabled4" runat="server">
                <uc:ClsCMTextBox ID="txtNotes" runat="server" ppName="注意事項" ppHeight="92" ppNameWidth="80" ppWidth="955" ppTextMode="MultiLine" ppMaxLength="500"  ppValidationGroup="1" ppNameVisible="False" ppWrap="true" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:Panel ID="PnlEnabled6" runat="server" BorderColor="#CCFFFF" BorderStyle="Solid" BorderWidth="1px" Width="1038" CssClass="center">
        <table style="width:1020px;" border="0">
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtTboxSerial" runat="server" ppName="ＴＢＯＸシリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtOppSerial" runat="server" ppName="操作盤シリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td colspan="5">
                    <uc:ClsCMTextBox ID="txtUPSSerial" runat="server" ppName="ＵＰＳシリアル" ppWidth="220" ppNameWidth="107" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtPrtSerial" runat="server" ppName="プリンタシリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtCRTSerial" runat="server" ppName="ＣＲＴシリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td style="width: 64px">&nbsp;</td>
                <td>&nbsp;</td>
                <td style="width: 100px">
                    <uc:ClsCMTextBox ID="txtSC" runat="server" ppName="ＳＣ" ppWidth="50" ppNameWidth="35" ppTextAlign="右" />
                </td>
                <td style="width: 55px">&nbsp;</td>
                <td style="width: 110px">
                    <uc:ClsCMTextBox ID="txtTVMachine" runat="server" ppName="券売機" ppNameWidth="46" ppWidth="50" ppTextAlign="右" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtHDD1Serial" runat="server" ppName="ＨＤＤ１シリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtHDD2Serial" runat="server" ppName="ＨＤＤ２シリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <uc:ClsCMTextBox ID="txtCC" runat="server" ppName="ＣＣ" ppWidth="50" ppNameWidth="35" ppTextAlign="右" />
                </td>
                <td>&nbsp;</td>
                <td>
                    <uc:ClsCMTextBox ID="txtAmMachine" runat="server" ppName="精算機" ppNameWidth="46" ppWidth="50" ppTextAlign="右" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc:ClsCMTextBox ID="txtHDD3Serial" runat="server" ppName="ＨＤＤ３シリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td>
                    <uc:ClsCMTextBox ID="txtHDD4Serial" runat="server" ppName="ＨＤＤ４シリアル" ppWidth="220" ppNameWidth="107" />
                </td>
                <td>&nbsp;</td>
                <td colspan="2">
                    <uc:ClsCMTextBox ID="txtSand" runat="server" ppName="サンド" ppWidth="50" ppNameWidth="46" ppTextAlign="右" />
                </td>
                <td>&nbsp;</td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        </asp:Panel>
    <asp:Panel ID="PnlEnabled7" runat="server">
     <table style="width:1060px;" class="center" border="0">
        <tr>
            <td>&nbsp;</td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtConSalDep" runat="server" ppName="担当営業部" ppNameWidth="80" ppWidth="397" ppMaxLength="50" ppValidationGroup="1" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSdTel" runat="server" ppName="ＴＥＬ" ppNameWidth="50" ppMaxLength="15" ppValidationGroup="1" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlSecChaSC" runat="server" ppClassCD="0115" ppMode="名称" ppName="保担ＳＣ" ppNameVisible="True" ppNotSelect="False" ppWidth="250" ppNameWidth="80" ppValidationGroup="1" ppRequiredField="False" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtLANResp" runat="server" ppName="ＬＡＮ担当" ppNameWidth="80" ppWidth="30" />
            </td>
            <td>
                <asp:TextBox ID="txtLANName" runat="server" Width="353px"></asp:TextBox>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtSecChaTel" runat="server" ppName="ＴＥＬ" ppNameWidth="50" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtScAdd" runat="server" ppName="住所" ppNameWidth="80" ppWidth="745" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtScFax" runat="server" ppName="ＦＡＸ" ppNameWidth="50" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlReviewSC" runat="server" ppClassCD="0115" ppMode="名称" ppName="総括ＳＣ" ppNameVisible="True" ppNameWidth="80" ppNotSelect="False" ppWidth="250" ppValidationGroup="1" ppRequiredField="False" />
            </td>
            <td colspan="2">
                <uc:ClsCMTextBox ID="txtBranchNm" runat="server" ppName="支店名" ppNameWidth="80" ppWidth="397" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtReviewTel" runat="server" ppName="ＴＥＬ" ppNameWidth="50" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtReviewAdd" runat="server" ppName="住所" ppNameWidth="80" ppWidth="745" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtReviewFax" runat="server" ppName="ＦＡＸ" ppNameWidth="50" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList ID="ddlAgency" runat="server" ppClassCD="0115" ppMode="名称" ppName="代理店" ppNameVisible="True" ppNameWidth="80" ppNotSelect="False" ppWidth="250" ppValidationGroup="1" />
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <uc:ClsCMTextBox ID="txtAgencyTel" runat="server" ppName="ＴＥＬ" ppNameWidth="50" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc:ClsCMTextBox ID="txtAgencyAdd" runat="server" ppName="住所" ppNameWidth="80" ppWidth="745" />
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtAgencyFax" runat="server" ppName="ＦＡＸ" ppNameWidth="50" />
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
  </asp:Content>

