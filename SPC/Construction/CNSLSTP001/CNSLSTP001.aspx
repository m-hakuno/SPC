<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="CNSLSTP001.aspx.vb" Inherits="SPC.CNSLSTP001" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table style="width:1050px;" border="0" class="center">
        <tr>
            <td style="width: 59%; height: 30px;">
                <uc:ClsCMTextBoxTwoFromTo ID="tftRequestNo" runat="server" ppIMEMode="半角_変更不可" ppMaxLengthOne="5" ppMaxLengthTwo="8" ppName="依頼番号" ppNameWidth="100" ppWidthOne="70" ppWidthTwo="100" ppCheckHanOne="True" ppCheckHanTwo="True" />
            </td>
            <td style="height: 30px;">
                <uc:ClsCMDateBoxFromTo ID="dftCnstDt" runat="server" ppName="工事開始日" ppNameWidth="100" />
            </td>
            <td>
                <asp:CheckBox ID="cbxFsWrk" runat="server" Text="FS稼働無" />
            </td>
        </tr>
        <tr>
            <td style="width: 59%">
                <uc:ClsCMDateBoxFromTo ID="dftReceptionDt" runat="server" ppName="受信日付" ppNameWidth="100" />
            </td>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftSTestDt" runat="server" ppName="総合テスト" ppNameWidth="100" />
            </td>
        </tr>
        <tr>
            <td style="width: 59%">
                <uc:ClsCMDropDownList ID="ddlContactDvs" runat="server" ppName="連絡区分" ppNameWidth="100" ppWidth="120" ppClassCD="0015" ppRequiredField="False" ppEnabled="True" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftAgency" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="代理店" ppNameWidth="100" ppWidth="70" ppCheckHan="True" ppNum="True" />
            </td>
        </tr>
        <tr>
            <td style="width: 59%">
                <uc:ClsCMTextBoxFromTo ID="tftTboxId" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="ＴＢＯＸＩＤ" ppNameWidth="100" ppWidth="80" ppCheckHan="True" ppNum="true" ppCheckLength="True" />
            </td>
            <td>
                <uc:ClsCMTextBoxFromTo ID="tftAgencyShop" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="5" ppName="代行店" ppNameWidth="100" ppWidth="70" ppCheckHan="True" ppNum="True" />
            </td>
        </tr>
        <tr style="margin-top:8px">
              <td style="height: 33px; width: 59%;">
               <asp:Label ID="lblSystem" runat="server" Text="システム" Width="98px" style="margin-left: 3px"></asp:Label>  
               <asp:DropDownList ID="ddlSystem" runat="server" Width="140" />
              </td>
            <td style="height: 33px">
                <asp:Label ID="Label1" runat="server" Text="出力順" Width="97px" style="margin-left: 3px" Visible="False"></asp:Label>
                <asp:DropDownList ID="ddlOutputOrder" runat="server"  Width="100" Visible="False">
                    <asp:ListItem Value="1">1:受信日付順</asp:ListItem>
                    <asp:ListItem Value="2">2:依頼番号順</asp:ListItem>
                    <asp:ListItem Value="3">3:進捗状況順</asp:ListItem>
                    <asp:ListItem Value="4">4:システム順</asp:ListItem>
                    <asp:ListItem Value="5">5:TBOXID</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 59%">
                <div style="width: 260px; float: left">
                    <uc:ClsCMDropDownList ID="ddlPrgSituatio" runat="server" ppName="進捗状況" ppNameWidth="100" ppWidth="140" ppClassCD="0015" ppRequiredField="False" ppEnabled="True" />
                </div>
                <div style="width: 200px; padding-top: 3px; float: left;">
                    <asp:DropDownList ID="ddlSituUpDwn" runat="server" Width="100">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="0">1:以上</asp:ListItem>
                        <asp:ListItem Value="1">2:以下</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <%--<asp:DropDownList ID="ddlPrgSituatio2" runat="server"  Width="140"></asp:DropDownList>--%>
            </td>
            </tr>
    </table>
    <table style="width:1050px;margin-top:8px" border="0" class="center">
        <tr>
            <td>
                <asp:Label ID="lblHoleConstraction" runat="server" Text="ホール内工事" Width="100px"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="cbxNew" runat="server" Text="新規" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxExpansion" runat="server" Text="増設" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxSomeRemoval" runat="server" EnableTheming="True" Text="一部撤去" Width="100px" />
            </td>
             <td>
                <asp:CheckBox ID="cbxShopRelocation" runat="server" Text="店内移設" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxAllRemoval" runat="server" Text="全撤去" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxOnceRemoval" runat="server" Text="一時撤去" Width="100px" />
            </td>
           
            <td style="width: 20px">&nbsp;</td>
            <td style="width: 30%">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
             <td>
                <asp:CheckBox ID="cbxReInstallation" runat="server" Text="再設置" Width="100px" />
            </td>
             <td>
                <asp:CheckBox ID="cbxConChange" runat="server" Text="構成変更" Width="100px" />
            </td>
            <td>
                <asp:CheckBox ID="cbxConDelivery" runat="server" Text="構成配信" Width="100px" />
            </td>
           <td>
                <asp:CheckBox ID="cbxOther" runat="server" Text="その他" Width="100px" />
            </td>
                      
            <td>
                <asp:CheckBox ID="cbxVup" runat="server" Text="ＶＵＰ" Width="100px" />
            </td>
            
            <td>&nbsp;</td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
<script src="/Scripts/fixed.js" type="text/javascript"></script>
    <!--該当件数表示 & リロードボタン-->
    <div ID="divCount" runat="server" class="float-Left">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCountTitle" runat="server" Text="該当件数：" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 80px">
                    <div class="float-right">
                        <asp:Label ID="lblCount" runat="server" Text="XXXXX" style="font-size:12pt;"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:Label ID="lblCountUnit" runat="server" Text="件" style="font-size:12pt;"></asp:Label>
                </td>
                <td style="width: 15px"></td>
                <td>
                    <asp:Button ID="btnReload" runat="server" CssClass="center" Text="リロード" />
                </td>
        </table>
    </div>
    <!--グリッド-->
    <div id="DivOut" runat="server" class="grid-out" >
        <div id="DivIn" class="grid-in" style="height:300px;  margin-top: 13px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
                <HeaderStyle CssClass="FIXED01" />
            </asp:GridView>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
