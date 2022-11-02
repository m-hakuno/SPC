<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM37.aspx.vb" Inherits="SPC.COMUPDM37" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>
<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <div class="center " style ="width:100%;">
        <table style="border-style: none; border-color: inherit; border-width: 0; width:950px; margin-left:auto; margin-right:auto; text-align:left">
            <tr>
                <td class="auto-style6">システム</td>
                <td class="auto-style4"><asp:DropDownList ID="ddlsSYS" runat="server" Width="120px" style="margin-left: 6px"></asp:DropDownList></td>
                <td class="auto-style8" style="margin-left:0px">料金区分</td>
                <td class="auto-style5" style="margin-left:0px"><asp:DropDownList ID="ddlsMAINTE" runat="server" Width="120px" style="margin-left: 6px"></asp:DropDownList></td>
                <td class="auto-style3">LAN単価種別<asp:DropDownList ID="ddlsLAN_CLS" runat="server" style="margin-left:10px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td class="auto-style7">開始日付</td>
                <td class="auto-style4">
                    <uc:ClsCMDateBoxFromTo runat="server" ID="dtbStart" ppName="適用期間"  ppNameVisible="false" ppDateFormat="年月日" ppMesType="ショート" ValidateRequestMode="Enabled" ClientIDMode="Inherit" ppValidationGroup="search" />
                </td>
                <td class="auto-style7">終了日付</td>
                <td class="auto-style4">
                    <uc:ClsCMDateBoxFromTo runat="server" ID="dtbFin" ppName="適用期間"  ppNameVisible="false" ppDateFormat="年月日" ppMesType="ショート" ValidateRequestMode="Enabled" ClientIDMode="Inherit" ppValidationGroup="search" />
                    <asp:CustomValidator ID="CstDayVal" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="search" SetFocusOnError="True"></asp:CustomValidator>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <style type="text/css">
        .RowDiv {
            width:950px;
            margin-left:auto; 
            margin-right:auto;
            padding-top:5px;
            padding-bottom:5px;
            text-align:left;            
        }
        .LeftCell {
            display: inline-block;
            width:75px;
            padding-right:7px;
            text-align:left;
            vertical-align:top ;
            padding-top:3px;
        }
        .RightCell {
            display: inline-block;
            width:150px;
            text-align:left;   
            vertical-align:top  ;         
        }
        .auto-style3
        {
            height: 32px;
            width: 211px;
        }
        .auto-style4
        {
            margin-left: 10px;
            width: 380px;
        }
        .auto-style5
        {
            height: 32px;
            width: 250px;
        }
        .auto-style6
        {
            margin-left: 10px;
            width: 90px;
        }
        .auto-style7
        {
            width: 90px;
        }
        .auto-style8
        {
            height: 32px;
            width: 90px;
        }
    </style>

    <%--グリッド表示エリア--%>
    <div class="RowDiv">
        <div class="LeftCell" >システム</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlSYS" runat="server" Width="120px"></asp:DropDownList>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSYS" CssClass="errortext" Display="Dynamic" EnableTheming="True" ErrorMessage="システムに値が設定されていません。" ValidationGroup="val" EnableClientScript="False">未入力エラー</asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="RowDiv"  >
        <div class="LeftCell" >料金区分</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlMAINTE" runat="server" Width="120px"></asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_Mainte" runat="server" ControlToValidate="ddlMAINTE" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
        </div>
        <div class="LeftCell"  >単価</div>
        <div class="RightCell" style="width:120px;">
            <asp:TextBox ID="txtPrice" runat="server" Width="69px" MaxLength="8" CssClass="IMEdisabledNum"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_Price" runat="server" ControlToValidate="txtPrice" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
        </div>
        <div class="LeftCell" >LAN単価種別</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlLAN_CLS" runat="server" Width="120px" ></asp:DropDownList>
        </div>
        <div class="LeftCell" style="width:55px;"  >LAN単価</div>
        <div class="RightCell">
            <asp:TextBox ID="txtLAN" runat="server" Width="67px" MaxLength="8" CssClass="IMEdisabledNum"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_LAN_Price" runat="server" ControlToValidate="txtLAN" CssClass="errortext" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
        </div>
    </div>
    <div class="RowDiv"  >
        <div class="LeftCell" style="padding-top:7px;padding-right:0px" >適用開始日</div>
        <div class="RightCell">
            <uc:ClsCMDateBox runat="server" ID="dtbStartDt" ppName="適用開始日" ppNameWidth="0" ppNameVisible="False" ppRequiredField="True" ppValidationGroup="val" />
            <asp:CustomValidator ID="CstmVal_Date" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style="padding-top:7px;padding-right:0px; padding-left:7px;" >適用終了日</div>
        <div class="RightCell">
            <uc:ClsCMDateBox runat="server" ID="dtbENDDt" ppName="適用終了日" ppNameWidth="0" ppNameVisible="False" ppRequiredField="True" ppValidationGroup="val" />
            <asp:CustomValidator ID="CstmVal_End" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val"></asp:CustomValidator>
        </div>
        <div class="LeftCell" ></div>                
        <div class="RightCell">
            <asp:Label ID="lblKey1" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblKey2" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblKey3" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblKey4" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblKey5" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblSYS_CLS" runat="server" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>

<%--グリッド表示エリア--%>
<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:972px;height:362px;">
        <div class="grid-in" style="width:972px;height:362px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">       
            </asp:GridView>
        </div>
    </div>
</asp:Content>
