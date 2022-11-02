<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="REPLSTP001.aspx.vb" Inherits="SPC.REPLSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="padding-left:4px; width: 99px;">
                <asp:Label ID="lblMakerKubun" runat="server" text="メーカ区分" Width="97px" Height="16px"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMakerKubun" runat="server" Width="250px" AutoPostBack="True"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr class="align-top">
            <td style="width: 510px">
                <uc:ClsCMTextBox ID="txtKanriNo" runat="server" ppName="管理番号" ppNameWidth="99" ppMaxLength="12" ppCheckHan="True" ppNum="False" ppIMEMode="半角_変更不可" />
            </td>
            <td style="width: 100px">
                <asp:CheckBox ID="chkRepairState" runat="server" Text="修理進捗" TextAlign="Left" AutoPostBack="True" />
            </td>
            <td>
                <asp:CheckBox ID="chkMenteState" runat="server" Text="整備進捗" TextAlign="Left" AutoPostBack="True" />
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr class="align-top">
            <td>
                <uc:ClsCMTextBox ID="txtTboxId" runat="server" ppName="ＴＢＯＸＩＤ" ppNameWidth="99" ppWidth="100" ppMaxLength="8" ppCheckHan="True" ppIMEMode="半角_変更不可" />
            </td>
            <td style="padding-top: 7px; width: 80px;">
                <asp:Label ID="lblHallName1" runat="server" text="ホール名：" Width="80px"></asp:Label>
            </td>
            <td style="padding-top: 7px">
                <asp:Label ID="lblHallName2" runat="server" Text="Label" Width="305px"></asp:Label>
            </td>
          </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="width: 510px">
                <uc:ClsCMDateBoxFromTo ID="dftAppaSendDt" runat="server" ppName="機器発送日" ppNameWidth="99" />
            </td>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftAppaArvDt" runat="server" ppName="機器到着日" ppNameWidth="100" />
            <td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">

        <tr>
            <td style="width: 510px">
                <uc:ClsCMDateBoxFromTo ID="dftTroubleDt" runat="server" ppName="故障発生日" ppNameWidth="99" />
            </td>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftArrivalDt" runat="server" ppName="対応日" ppNameWidth="100" />
            </td>
        </tr>

        <tr>
            <td style="width: 510px">
                <uc:ClsCMDateBoxFromTo ID="dftCmpSndDt" runat="server" ppName="完了発送日" ppNameWidth="99" />
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="width: 510px">
                <uc:ClsCMTextBox ID="txtOldSerial" runat="server" ppName="旧シリアル" ppNameWidth="99" ppMaxLength="20" ppNum="False" ppIMEMode="半角_変更不可" ppWidth="150" ppCheckHan="True"/>
            </td>
            <td>
                <uc:ClsCMTextBox ID="txtNewSerial" runat="server" ppName="新シリアル" ppNameWidth="100" ppMaxLength="20" ppNum="False" ppIMEMode="半角_変更不可" ppWidth="150" ppCheckHan="True"/>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr class="align-top">
            <td style="width: 405px">
                <uc:ClsCMTextBox ID="txtContent" runat="server" ppName="故障内容" ppNameWidth="99" ppWidth="420" ppMaxLength="30" />
            </td>
            <td style="padding-top: 7px">
                <asp:Label ID="lblAimaiSearch1" runat="server" text="(あいまい検索)" Width="100px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr class="align-top">
            <td style="width: 405px">
                <uc:ClsCMTextBox ID="txtRptDtl" runat="server" ppName="処置内容" ppNameWidth="99" ppWidth="420" ppMaxLength="30"/>
            </td>
            <td style="padding-top: 7px">
                <asp:Label ID="lblAimaiSearch2" runat="server" text="(あいまい検索)" Width="100px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr class="align-top">
            <td style="width: 405px">
                <uc:ClsCMTextBox ID="txtRepairContent" runat="server" ppName="修理結果" ppNameWidth="99" ppWidth="420" ppMaxLength="30"/>
            </td>
            <td style="padding-top: 7px">
                <asp:Label ID="lblAimaiSearch3" runat="server" text="(あいまい検索)" Width="100px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="padding-left:4px; width: 99px;">
                <asp:Label ID="lblWorkNo1" runat="server" text="作業項番１" Width="99px" Height="16px"></asp:Label>
            </td>     
            <td style="width: 231px">
                <asp:DropDownList ID="ddlWorkNo1" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
            <td style="width: 81px">
                <asp:Label ID="lblWorkNo2" runat="server" text="作業項番２" Width="80px"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlWorkNo2" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="padding-left:4px; width: 99px;">
                <asp:Label ID="lblPrtsNo1" runat="server" text="部品項番１" Width="99px"></asp:Label>
            </td>
            <td>      
                <asp:DropDownList ID="ddlPrtsNo1" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="padding-left:4px; width: 99px;">
                <asp:Label ID="lblPrtsNo2" runat="server" text="部品項番２" Width="99px"></asp:Label>
            </td>
            <td style="width: 232px">
                <asp:DropDownList ID="ddlPrtsNo2" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
            <td style="width: 227px">      
                <asp:DropDownList ID="ddlPrtsNo3" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlPrtsNo4" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="width: 102px"></td>
            <td style="width: 232px">      
                <asp:DropDownList ID="ddlPrtsNo5" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
            <td style="width: 249px">      
                <asp:DropDownList ID="ddlPrtsNo6" runat="server" Width="180px" Enabled="False"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" text="※メーカ区分と項番はＡＮＤ条件" Width="220px"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="width: 99px">
                <uc:ClsCMDropDownList runat="server" ID="ddlTmpResult" ppName="一時診断結果" ppNameWidth="99" ppWidth="100" ppClassCD="0021" ppNotSelect="True"/>
            </td>
            <td style="width: 41px"></td>
            <td style="width: 80px">
                <asp:Label ID="lblStatusCd" runat="server" text="ステータス" Width="80px"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlStatusCd" runat="server" Width="180px" ValidationGroup="2"></asp:DropDownList>
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo runat="server" ID="tftReqDt" ppName="請求年月" ppWidth="48" ppMaxLength="6" ppIMEMode="半角_変更不可" ppNum="False"/>
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
        <div class="grid-out">
        <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
