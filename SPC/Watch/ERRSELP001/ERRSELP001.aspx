<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ERRSELP001.aspx.vb" Inherits="SPC.ERRSELP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
   <%-- ----------------------------------------
    2014/5/9 稲葉　ここから
    ------------------------------------------%>
    <Script src='<%= Me.ResolveClientUrl("~/Scripts/ctiexe.js")%>'></Script>
      <%-- ----------------------------------------
    2014/5/9 稲葉　ここまで
    ------------------------------------------%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <br />
    <asp:Panel ID="Panel1" runat="server">
        <table class ="center">
            <tr>
                <td>
                    <asp:Panel ID="pnlUpdate" runat="server" Width="1080px">
                        <table class ="center">
                            <tr>
                                <td>
                                    <asp:Label ID="lblSyokaiDate" runat="server" Text="照会日時"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSyokaiDate_Input" runat="server" Text="YYYY/MM/DD HH:MM:SS"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="Panel3" runat="server">
        <table class ="center">
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" Width="1080px">
                        <table style="width: 100%;" border="1" class ="center">
                            <tr>
<%--                                <td style="width:270px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblRecvDay" runat="server" Text="データ受信日"></asp:Label>
                                    </div>
                                </td>
                                <td style="width:150px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblRecvTime" runat="server" Text="データ受信時間"></asp:Label>
                                    </div>
                                </td>--%>
                                <td style="width:100px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblRecvDay" runat="server" Text="データ受信日時"></asp:Label>
                                    </div>
                                </td>
                                <td style="width:200px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblTboxID" runat="server" Text="ＴＢＯＸＩＤ"></asp:Label>
                                    </div>
                                </td>
                                <td style="width:250px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblHallName" runat="server" Text="ホール名"></asp:Label>
                                    </div>
                                </td>
<%--                                <td style="width:80px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblSystem" runat="server" Text="システム"></asp:Label>
                                    </div>
                                </td>
                                <td style="width:60px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblVer" runat="server" Text="ＶＥＲ"></asp:Label>
                                    </div>
                                    
                                </td>--%>
                                <td style="width:100px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblSystem" runat="server" Text="ＴＢＯＸタイプ"></asp:Label>
                                    </div>
                                </td>
                                <td style="width:250px">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblAddress" runat="server" Text="住所"></asp:Label>
                                    </div>
                                </td>
                                <td rowspan="4" style="width:125px">
                                    <div style="text-align:center">
                                        <asp:Button ID="btnTrouble" runat="server" Text="トラブル処理票" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btnUpdata" runat="server" Text="更新" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="center">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblRecvDay_Input" runat="server" Text="9999/99/99" Height="10px"></asp:Label>
                                    </div>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblTboxID_Input" runat="server" Text="12345678"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblHallName_Input" runat="server" Text="無応答ホール1"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <table style="text-align:center;">
                                        <tr>
                                            <td>
                                                <div style="text-align:center">
                                                    <asp:Label ID="lblSystem_Input" runat="server" Text="NVC100"></asp:Label>
                                                </div>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <div style="text-align:center">
                                                    <asp:Label ID="lblVer_Input" runat="server" Text="99.99"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblAddress_Input" runat="server" Text="ＡＡＡ県ＢＢＢＢＢＢ市ＣＣＣＣ区 ９－９－９"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblINSNum" runat="server" Text="INS回線番号"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblTyousaDT" runat="server" Text="調査日時"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblTanto" runat="server" Text="担当者"></asp:Label>
                                    </div>
                                </td>
                                <td colspan="2">
                                    <div style="text-align:center">
                                        <asp:Label ID="lblTyousaResult" runat="server" Text="調査結果"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblINSNum_Input" runat="server" Text="03-1234-5678"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align:left">
                                        <table style="width: 100%;" class="center">
                                            <tr>
                                                <td class="align-top">
                                                    <uc:ClsCMDateBox runat="server" ID="txtTyousaDay" ppNameVisible="false" ppName="調査日" ppRequiredField="false" ppValidationGroup="1" />
                                                </td>
                                                <td class="align-top">
                                                    <uc:ClsCMTimeBox runat="server" ID="txtTyousaTime" ppNameVisible="false" ppName="調査時間" ppRequiredField="false" ppValidationGroup="1" />
                                                </td>
                                            </tr>                          
                                        </table>                                        
                                    </div>
                                </td>
                                <td>
                                    <table border="0">
                                        <tr style="vertical-align: top">
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="align-top">
                                                            <asp:Panel ID="pnlData" runat="server" Width="100" CssClass="align-top">
                                                                <asp:DropDownList ID="ddlList" runat="server"  Width="120px">
                                                                </asp:DropDownList>
                                                                <div style="white-space: nowrap">
                                                                    <asp:Panel ID="pnlErr" runat="server" Width="0px">
                                                                        <asp:CustomValidator ID="vldDdlList" runat="server" ValidationGroup="1" ControlToValidate="ddlList" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                                                    </asp:Panel>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2">
                                     <table class="center">
                                        <tr>
                                            <td>
                                                <uc:ClsCMTextBox runat="server" ID="txtTousaResult" ppHeight="30" ppTextMode="MultiLine" ppWidth="330" ppNameVisible="False" ppMaxLength="50" ppName="調査結果" ppRequiredField="false" ppValidationGroup="1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="text-align:center">
                                        <asp:Label ID="lblInfo" runat="server" Text="注意情報"></asp:Label>
                                    </div>
                                </td>
                                <td colspan="5">
                                    <asp:Label ID="lblInfo_Input" runat="server" Text="内容１２３４５６７８９０"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <br />

    <div id="DivOut" class="grid-out" style="height:500px;">
        <div class="grid-in" style="height:500px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <div class="float-left">
        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
    </div>
</asp:Content>
