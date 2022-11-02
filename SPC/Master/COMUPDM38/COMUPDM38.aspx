<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM38.aspx.vb" Inherits="SPC.COMUPDM38" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    
    <div class="center " style ="width:100%;">

        <%--グリッド表示エリア--%>
        <table style="border-style: none; border-color: inherit; border-width: 0; width:900px; margin-left:auto; margin-right:auto;text-align:left">
            <tr>
                 <td class="auto-style37" style="padding-right :7px;">TBOXID</td>
                <td class="auto-style35">
                <asp:TextBox ID="txtsTfrom" runat="server" Width="69px" CssClass="IMEdisabled" MaxLength="8" ValidationGroup="search"></asp:TextBox>
                    ～ <asp:TextBox ID="txtsTto" runat="server" Width="69px" CssClass="IMEdisabled" MaxLength="8" ValidationGroup="search"></asp:TextBox>
                    <br />
                    <asp:CustomValidator ID="CstmVal_sID" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="search" SetFocusOnError="True"></asp:CustomValidator>
                 </td>
               <td class="auto-style35" style="padding-top:6px;">
        <uc:ClsCMDateBoxFromTo runat="server" ID="dtbStart" ppName="開始日付" ppNameWidth="70"
                                ppDateFormat="年月日" ppMesType="ショート"  ppValidationGroup="search"/>
                   <Div style="margin-left:75px;">

                   <asp:CustomValidator  ID="CstDayVal" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="search" SetFocusOnError="True"></asp:CustomValidator>
                </Div>
                       </td>
                <td class="auto-style26" colspan="2" style="padding-top:6px;">
        <uc:ClsCMDateBoxFromTo runat="server" ID="dtbFin" ppName="終了日付" ppNameWidth="75"
                                ppDateFormat="年月日" ppMesType="ショート" ppValidationGroup="search"/>
                
                </td> 
            </tr>
            <tr>
                <td class="auto-style38">
                     <asp:Label ID="Label2" runat="server" Text="システム" Width="70px" style="margin-bottom: 0px"></asp:Label>
                 </td>
            <td class="auto-style36">
                 <asp:DropDownList ID="ddlSystem" runat="server" Width="110" ></asp:DropDownList>
            </td>
                <td class="auto-style36">
                <uc:ClsCMDropDownList ID="ddlSNL" runat="server" ppName="NL区分" ppNameWidth="70" ppWidth="140" ppClassCD="0128" ppNotSelect="true"/>
            </td>


                 <td class="auto-style32">
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="75" ppWidth="120" ppClassCD="0124" ppNotSelect="true"/>
            </td>
                </tr>
            </table>
          <div style="float:left; text-align:left;">
                        <asp:ValidationSummary ID="ValidSumEdit" runat="server" CssClass="errortext" ValidationGroup="Edit" />
                    </div>

    </div>
    
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <style type="text/css">


        .RowDiv {
            width:900px;
            margin-left:auto; 
            margin-right:auto;
            padding-top:5px;
            padding-bottom:5px;
            text-align:left;               
        }
        .LeftCell {
            display: inline-block;
            width:70px;
            padding-right:7px;
            text-align:left;
            vertical-align:top ;
            padding-top:3px;
        }
        .RightCell {
            display: inline-block;
            width:125px;
            text-align:left;   
            vertical-align:top  ;         
        }
        .auto-style32
        {
            width: 282px;
        }
        .auto-style35
        {
            text-align: left;
            width: 280px;
            height: 50px;
        }
        .auto-style36
        {
            width: 280px;
        }
        .auto-style37
        {
            text-align: left;
            width: 72px;
            height: 50px;
        }
        .auto-style38
        {
            width: 72px;
        }
    </style>
    <div class="RowDiv"  >
        <div class="LeftCell" >
            TBOXID</div>
        <div class="RightCell">
            <asp:TextBox ID="txtTBOX" runat="server" Width="69px" CssClass="IMEdisabled" MaxLength="8" AutoPostBack="True"  ValidationGroup="key" CausesValidation="true" ></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_TBOX" runat="server" ControlToValidate="txtTBOX" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="False" Display="Dynamic" ValidationGroup="key"  SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" >
            連番</div>
        <div class="RightCell" style ="padding-top:3px;">
            <asp:Label ID="lblSEQ" runat="server"></asp:Label>
        </div>
        <div class="LeftCell" >
            ホール</div>
        <div class="RightCell"  style="width:350px;padding-top:3px;">
            <asp:Label ID="lblHALL" runat="server"></asp:Label>
        </div>
    </div>
    <div class="RowDiv"  style="margin-top:1px" >
        <div class="LeftCell" >
            NL区分</div>
        <div class="RightCell" style ="padding-top:3px;">
            <asp:Label ID="lblNL" runat="server"></asp:Label>
        </div>
        <div class="LeftCell" >
            システム</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlSYS" runat="server">
            </asp:DropDownList>
        </div>
        <div class="LeftCell" >
            バージョン</div>
        <div class="RightCell">
            <asp:TextBox ID="txtVer" runat="server" Width="96px" CssClass="IMEdisabled" MaxLength="5"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_4" runat="server" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div> 
        <div class="LeftCell" style="margin-left:25px">
            <asp:Label ID="Label3" Text="登録日時" runat="server"></asp:Label></div>
        <div class="RightCell" style="padding-top:3px">
            <asp:Label ID="lblInsDay" runat="server"></asp:Label>
        </div>

        
    </div>
    <div class="RowDiv" style="margin-top:1px">
        <div class="LeftCell" style="padding-right:0px; padding-top:5px; " >
            開始日付</div>
        <div class="RightCell" style="padding-right:6px;">
            <uc:ClsCMDateBox runat="server" ID="dtbStartDt" ppName="開始日付" ppNameVisible="False" ppRequiredField="True" ppValidationGroup="val" />
            <asp:CustomValidator ID="CstmVal_Date" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style=" padding-top:8px; padding-right:1px;" >
            終了日付</div>
        <div class="RightCell"style=" padding-top:1px;">
            <uc:ClsCMDateBox runat="server" ID="dtbENDDt" ppName="終了日付" ppNameVisible="False" ppRequiredField="True" ppValidationGroup="val" ppNameWidth="0" />
            <asp:CustomValidator ID="CstmVal_End" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style="padding-top:8px; padding-left:7px; " >
            計算区分</div>
        <div class="RightCell" style="padding-left:0px; padding-top:4px; vertical-align:top  ;" >
            <asp:DropDownList ID="ddlPERAT" runat="server" Width="100px">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem Value="0">-</asp:ListItem>
                <asp:ListItem Value="1">+</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPERAT" CssClass="errortext" Display="Dynamic" EnableTheming="True" ErrorMessage="計算区分に値が設定されていません。" ValidationGroup="val" EnableClientScript="True" SetFocusOnError="True">未入力エラー</asp:RequiredFieldValidator>
        </div>
     <div class="LeftCell" style="padding-top:9px;margin-left:25px">
            <asp:Label ID="Label1" Text="更新日時" runat="server"></asp:Label></div>
        <div class="RightCell" style="padding-top:9px">
            <asp:Label ID="lblUpdDay" runat="server"></asp:Label>
        </div>
    
    </div>
   <div class="RightCell">
            <asp:Label ID="lblKey1" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblKey2" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblKey3" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblFROM" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblEND" runat="server" Visible="False"></asp:Label>
        </div>
</asp:Content>

<%--グリッド表示エリア--%>
<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:1208px;height:313px;">
        <div class="grid-in" style="width:1208px;height:313px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />

            <asp:GridView ID="grvList" runat="server">       

            </asp:GridView>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style26 {
            text-align: left;
            height: 50px;
        }
        </style>
    </asp:Content>

