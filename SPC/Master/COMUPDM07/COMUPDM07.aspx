<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM07.aspx.vb" Inherits="SPC.COMUPDM07" %>
<%@ MasterType VirtualPath="~/Master/Mst.Master" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function pageLoad() {

            //PageRequestManagerのインスタンスを生成
            var mng = Sys.WebForms.PageRequestManager.getInstance();

            // 非同期ポストバックの初期化時に呼び出される
            // イベント・ハンドラを定義
            mng.add_initializeRequest(

              // ほかの非同期ポストバックが実行中で、かつ、
              // 現在のイベント発生元要素がクリアボタンでない場合、
              // 現在の非同期ポストバックをキャンセル
              function (sender, args) {
                  if (mng.get_isInAsyncPostBack() &&
                          (args.get_postBackElement().id != 'btnClear')) {
                      args.set_cancel(true);
                  }
               }
            );
        }


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
        function focusChange(btnDmy, txtBox) {
            btnDmy.style.visibility = "hidden";
            txtBox.focus();
        }
    </script>
</asp:Content>

<%--SearchContent--%>
<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <style type="text/css">
        .Cell1 {
            width:120px;
            float:left;
            text-align:left;
            vertical-align:middle;
            padding-top:3px;
        }
        .Cell2 {
            width:220px;
            float:left;
            text-align:left;
        }
        .SearchTable tr {
            height:15px;
            padding-bottom:5px;
            padding-top:5px;

        }
    </style>

   
    <table class="SearchTable" style="border-style: none; border-color: inherit; border-width: 0; width:1050px; margin-left:auto; margin-right:auto;">
        <tr>
            <td>
                    <div class="Cell1" >商品/機器コード</div>
                    <div class="Cell2" >
                <asp:TextBox ID="txtsAPPACDfrom" runat="server" Width="80px" CssClass="ime-disabled" MaxLength="8"></asp:TextBox>
                    ～ <asp:TextBox ID="txtsAPPACDto" runat="server" Width="80px" CssClass="ime-disabled" MaxLength="8"></asp:TextBox>
                        <br />
                <asp:CustomValidator ID="CstmVal_s01" runat="server" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="search" SetFocusOnError="True"></asp:CustomValidator>
                    </div>
            </td>
            
        </tr>
        <tr>
            <td>
                <div class="Cell1" >型式/機器</div>
                <div class="Cell2" style="width:450px;" >
                <asp:TextBox ID="txtsAPPANM" runat="server" MaxLength="50" Width="380px" CssClass="ime-active"></asp:TextBox>
                 </div>
                <div class="Cell1" style="width:90px;" >機器略称</div>
                <div class="Cell2" >
                <asp:TextBox ID="txtsSHORT" runat="server" Width="240px" MaxLength="20" CssClass="ime-active"></asp:TextBox>
                 </div>
            </td>
            
        </tr>
        <tr>
            <td>
                <div class="Cell1" >機器分類</div>
                <div class="Cell2" style="width:220px;" >
                <asp:DropDownList ID="ddlsAPPAGroup" runat="server" AutoPostBack="True" Width="120px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="0">分岐フラグ０</asp:ListItem>
                    <asp:ListItem Value="1">分岐フラグ１</asp:ListItem>
                </asp:DropDownList>
                 </div>
                <div class="Cell1" style="width:90px;margin-left:230px;"  >機器種別</div>
                <div class="Cell2" >
                    <asp:DropDownList ID="ddlsAPPACLS" runat="server" Width="240px">
                    </asp:DropDownList>
                 </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="Cell1" >システム</div>
                <div class="Cell2" >
                <asp:DropDownList ID="ddlsSYS" runat="server" Width="120px"></asp:DropDownList>
                 </div>
                <div class="Cell1"  style="width:82px;margin-left:232px;">削除区分</div>
                <div class="Cell2">
                    <uc:ClsCMDropDownList ID="ddldel" ppNameVisible="false" runat="server" ppName="削除区分" ppNameWidth="0" ppWidth="120" ppClassCD="0124" ppNotSelect="true"/>
                </div>
            </td>
        </tr>
    </table>

        



</asp:Content>

<%--MainContent--%>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <style type="text/css">

        .RowDiv {
            width:1050px;
            margin-left:auto; 
            margin-right:auto;
            padding-top:5px;
            padding-bottom:5px;
            text-align:left;            
        }
        .LeftCell {
            display: inline-block;
            width:115px;
            padding-right:1px;
            text-align:left;
            vertical-align:top ;
            padding-top:3px;

        }
        .RightCell {
            display: inline-block;
            width:215px;
            text-align:left;    
            vertical-align:top ;        
        }
        </style>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            商品/機器コード</div>
        <div class="RightCell">
            <asp:TextBox ID="txtAPPACD" runat="server" MaxLength="8" CssClass="ime-disabled" Width="80px" AutoPostBack="True"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_APPACD" runat="server" ControlToValidate="txtAPPACD" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="key" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" >
        </div>
        <div class="RightCell">
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            型式/機器</div>
        <div class="RightCell">
            <asp:TextBox ID="txtAPPANM" runat="server" MaxLength="50" Width="670px" CssClass="ime-active"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_NM" runat="server" ControlToValidate="txtAPPANM" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            機器略称</div>
        <div class="RightCell">
            <asp:TextBox ID="txtSHORTNM" runat="server" MaxLength="20" Width="280px" CssClass="ime-active"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_Short" runat="server" ControlToValidate="txtSHORTNM" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            機器分類</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlAPPAGroup" runat="server" AutoPostBack="True" style="height: 19px" Width="173px">
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_APPAGroup" runat="server" ControlToValidate="ddlSYS" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" >
        </div>
        <div class="RightCell">
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            機器種別</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlAPPACLS" runat="server" Width="173px" style="height: 19px" AutoPostBack="True">
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_APPACLS" runat="server" ControlToValidate="ddlSYS" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style="width:100px;"  >
            機器バージョン</div>
        <div class="RightCell" style="width:150px;" >
            <asp:TextBox ID="txtVERSION" runat="server" MaxLength="10" CssClass="ime-disabled" Width="96px"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_MachineVer" runat="server" ControlToValidate="txtVERSION" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style="width:60px;" >
            型番</div>
        <div class="RightCell">
            <asp:TextBox ID="txtMODELNo" runat="server" MaxLength="20" Width="148px" CssClass="ime-disabled"></asp:TextBox>
            <br />
            <asp:CustomValidator ID="CstmVal_Model" runat="server" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            HDD No</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlHDDNo" runat="server" Width="173px">
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_HDDNo" runat="server" ControlToValidate="ddlHDDNo" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style="width:100px;"  >
            HDD 種別</div>
        <div class="RightCell" style="width:150px;" >
            <asp:DropDownList ID="ddlHDDCLS" runat="server" Width="100px">
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_HDDCLS" runat="server" ControlToValidate="ddlHDDCLS" CssClass="errortext" ErrorMessage="CustomValidator" ValidateEmptyText="True" Display="Dynamic" ValidationGroup="val" SetFocusOnError="True"></asp:CustomValidator>
        </div>
        <div class="LeftCell" style="width:60px;" runat="server" id="prtclsName">
            印刷区分</div>
        <div class="RightCell">
            <asp:CheckBox ID="chkPrtcls" runat="server"></asp:CheckBox>
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            システム</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlSYS" runat="server" AutoPostBack="True" Width="173px">
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_SYS" runat="server" ControlToValidate="ddlSYS" CssClass="errortext" ErrorMessage="CustomValidator" Display="Dynamic" ValidationGroup="val" ValidateEmptyText="True" SetFocusOnError="True"></asp:CustomValidator>
            <asp:HiddenField ID="hdnSYS" runat="server" />
        </div>
        <div class="LeftCell" style="width:100px;"  >
            バージョン</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlVer" runat="server" Width="100px">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:CustomValidator ID="CstmVal_Ver" runat="server" ControlToValidate="ddlVer" CssClass="errortext" ErrorMessage="CustomValidator" Display="Dynamic" ValidationGroup="val" ValidateEmptyText="True" SetFocusOnError="True"></asp:CustomValidator>
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            工事依頼書用区分</div>
        <div class="RightCell">
            <asp:DropDownList ID="ddlCNSTDET" runat="server" Width="173px">
            </asp:DropDownList>
        </div>
    </DIV>
    <DIV class="RowDiv" >
        <div class="LeftCell" >
            TOMAS名称</div>
        <div class="RightCell" style="width:700px;" >
            <asp:TextBox ID="txtTOMAS" runat="server" MaxLength="50" Width="670px" CssClass="ime-active"></asp:TextBox>
        </div>
    </DIV>
    <%--MainContent--%>

</asp:Content>

<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">
    <div class="grid-out" style="width:1220px;height:200px">
        <div class="grid-in" style="width:1220px;height:200px">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>

