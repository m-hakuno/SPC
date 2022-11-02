<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MNTSELP001.aspx.vb" Inherits="SPC.MNTSELP001"  MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 250px;
            height: 44px;
        }
        .auto-style3 {
            vertical-align: top;
            width: 90px;
            height: 44px;
        }
        .auto-style4 {
            width: 238px;
            height: 44px;
        }
        .auto-style5 {
            height: 44px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="pnlMente" runat="server" >
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblReqComp" runat="server" Text="依頼先　　："></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblReqComp_Nm" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblMante" runat="server" Text="管理番号　："></asp:Label>
            </td>
            <td Style="width :300px">
                <asp:Label ID="lblMente_No" runat="server"></asp:Label>
            </td>
            <td Style="width :89px">
                <asp:Label ID="lblOrder" runat="server" Text="注文番号　："></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblOrder_No" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblWrkCls" runat="server" Text="作業種別　："></asp:Label>
            </td>
            <td Style="width :300px">
                <asp:Label ID="lblWrkCls_Cd" runat="server"></asp:Label>
            </td>
            <td Style="width :89px">
                <asp:Label ID="lblTboxCls" runat="server" Text="システム　："></asp:Label>
            </td>
            <td >
                <asp:Label ID="lblTboxCls_Cd" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblVersion" runat="server" Text="VER設定 　："></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblVersion_Nm" runat="server"></asp:Label>
            </td>

        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblCeriv" runat="server" Text="納入先　　："></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblCeriv_Nm" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                </td>
            <td>
                <asp:Label ID="lblZip" runat="server">〒</asp:Label>
                <asp:Label ID="lblZip_No" runat="server"></asp:Label>
                
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                </td>
            <td>
                <asp:Label ID="lblAddr" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                </td>
            <td Style="width :110px">
                <asp:Label ID="lblTel" runat="server">ＴＥＬ 　　：</asp:Label>
            </td>
            <td Style="width :190px">
                <asp:Label ID="lblTel_No" runat="server"></asp:Label>
            </td>
            <td Style="width :90px">
                <asp:Label ID="lblFax" runat="server">ＦＡＸ　　：</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblFax_No" runat="server"></asp:Label>
            </td>
        </tr>
    </table><br />
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblTitle1" runat="server" Text="整備機器進捗" Font-Bold="True"></asp:Label>
            </td>
            <td>
                </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :252px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblAppa" runat="server" Text="機器名　　　　"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblAppa_Nm" runat="server" ></asp:Label>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :250px"></td>
            <td class="align-top" Style=" padding-top:2px; width :200px">
                <uc:ClsCMDropDownList runat="server" ID="ddlTmptst_Cd" ppName="一時診断結果" ppNameWidth="90" ppWidth="130" ppClassCD="0021" ppNotSelect="True" ppValidationGroup="1"/>
            </td>
            <td Style="width :100px"></td>
            <td class="align-top" style= "width:315px">
                <uc:ClsCMDateBox ID="dtbRsltSend_Dt" runat="server" ppName="診断結果送付日" ppNameWidth="101" ppValidationGroup="1"/>
            </td>
            <td>
                </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :253px"></td>
            <td class="align-top" Style="padding-top:7px; width :88px">
                <asp:Label ID="lblPrgStatus" runat="server" Text="ステータス"></asp:Label>
            </td>
            <td class="align-top" Style="padding-top:5px; width :238px">
                <asp:DropDownList ID="ddlPrgStatus_Cd" runat="server" Width="130px">
                </asp:DropDownList>
            </td>
            <td class="align-top" >
                <uc:ClsCMDateBox ID="dtbSend_Dt" runat="server" ppName="完了発送日" ppNameWidth="102" ppValidationGroup="1"/>
            </td>
            <td>
                </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :250px"></td>
            <td class="align-top" Style="width :238px">
                <uc:ClsCMDropDownList runat="server" ID="ddlWork" ppName="作業内容" ppNameWidth="88" ppWidth="100" ppClassCD="0071" ppNotSelect="True" ppValidationGroup="1"/>
            </td>
            <td Style="width :90px"></td>
            <td class="align-top" >
                <uc:ClsCMTextBox ID="txtSerial_No" runat="server" ppName="シリアルＮｏ" ppNameWidth="103" ppValidationGroup="1" ppMaxLength="20" ppCheckHan="True" ppIMEMode="半角_変更不可"  ppWidth="160"/>
            </td>
            <td>
                </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :250px"></td>
            <td class="align-top" style= "width:200px">
                <uc:ClsCMDateBox ID="dtbReceipt_Dt" runat="server" ppName="受領日" ppNameWidth="88" ppValidationGroup="1" />
            </td>
            <td Style="width :130px"></td>
            <td class="align-top" >
                <uc:ClsCMDropDownList runat="server" ID="ddlClass" ppName="テスト区分" ppNameWidth="101" ppWidth="100" ppClassCD="0073" ppNotSelect="True" ppValidationGroup="1"/>
            </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
            <td class="auto-style2"></td>
            <td class="auto-style3">
                <uc:ClsCMTextBox runat="server" ID="txtReq_Dt" ppName="請求年月" ppNameWidth="88" ppValidationGroup="1" ppIMEMode="半角_変更不可" ppMaxLength="6" />
            </td>
            <td class="auto-style4">
            </td>
            <td style= "padding-top :16px;" class="auto-style5">
            </td>
            <td class="auto-style5">
                </td>
        </tr>
    </table><br />
    <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblTitle2" runat="server" Text="交換部品" Font-Bold="True"></asp:Label>
            </td>
            <td>
                
            </td>
        </tr>
    </table>

<asp:Panel ID="pnlRegister" runat="server"  BorderStyle="Solid" BorderWidth="1px" Width="840" CssClass="center">

    <table border="0" style="width:800px;">
      <tr>
          <td Style="width :47px"></td>
          <td class="align-top" style= "padding-top :8px; width: 85px">
               <uc:ClsCMDropDownList runat="server" ID="ddlWrkCls" ppName="作業分類" ppNameWidth="88" ppWidth="130" ppClassCD="0092" ppNotSelect="True" ppValidationGroup="2" ppRequiredField="True"/>
          </td>
          <td>

          </td>
          </tr>
    </table>
    <table border="0" style="width:800px;">
        <tr>
            <td Style="width :50px"></td>
            <td class="align-top" style= "padding-top :8px;  width:85px">
                 <asp:Label ID="lblPartsNm" runat="server" Text="部品名" Width="80px" ></asp:Label>
            </td>
            <td class="align-top" style= "padding-top :5px; padding-left :4px; width: 200px">
                 <asp:Panel ID="Panel5" runat="server">
                    <asp:DropDownList ID="ddlPartsNm" runat="server" Width="350px" ValidationGroup="2"></asp:DropDownList>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="Panel6" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvPartsNm" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ValidationGroup="2" ControlToValidate="ddlPartsNm"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
                 </asp:Panel>
            <td></td>
            <td class="align-top" >
                <uc:ClsCMTextBox ID="txtQuantity" runat="server" ppName="個数" ppWidth="50" ppValidationGroup="2" ppMaxLength="4" ppNum="True" ppRequiredField="True" ppIMEMode="半角_変更不可" />
            </td>
    </table>

    <table style="width: 800px">
        <tr>
             <td>
                 <div class="float-right">
                     <asp:Button ID="btnClear" runat="server" Text="クリア" Width="58px" />
                     <asp:Button ID="btnInsert" runat="server" Text="追加" ValidationGroup="2" Height="21px" Width="62px" />
                     <asp:Button ID="btnUpdate" runat="server" Text="更新" ValidationGroup="2" Width="62px" />
                     <asp:Button ID="btnDelete" runat="server" Text="削除" ValidationGroup="2" Width="62px" />
                 </div>
             </td>
        </tr>
    </table>
    <table border="0" style="width:1050px;">
        <tr>
             <td>
             </td>
        </tr>
    </table>
</asp:Panel>
    <table border="0" style="width:1050px;">
        <tr>
            <td>
                <div class="float-left">
                    <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
                    <asp:ValidationSummary ID="vasSummary2" runat="server" CssClass="errortext" ValidationGroup="2" />
                </div>
            </td>
        </tr>
    </table><br />
      <table border="0" style="width:1050px;">
        <tr>
            <td Style="width :200px"></td>
            <td Style="width :90px">
                <asp:Label ID="lblTitle3" runat="server" Text="交換部品一覧" Font-Bold="True" Width="100px"></asp:Label>
            </td>
            <td>
                
            </td>
        </tr>
    </table>

    <table class="center">
        <tr>
            <td style="width:840px;">
                <div class="float-top">
                    <div class="grid-out">
                        <div style="overflow:auto;width:843px;height:200px;">
                            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                            <asp:GridView ID="grvList" runat="server">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
<%--    <table style="width: 1050px">
        <tr>
             <td>
                 <div class="float-right">
                     <%--<asp:Button ID="btnReset" runat="server" Text="元に戻す" Height="21px" Width="68px" />--%>
                     <%--<asp:Button ID="btnAllDelete" runat="server" Text="削除"  Width="62px" />--%>
                     <%--<asp:Button ID="btnAllUpdate" runat="server" Text="更新" ValidationGroup="1" Width="62px" />--%>
<%--                 </div>
             </td>
        </tr>
    </table>--%>
</asp:Panel>

</asp:Content>

