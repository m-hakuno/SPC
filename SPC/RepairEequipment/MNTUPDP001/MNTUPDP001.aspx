<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="MNTUPDP001.aspx.vb" Inherits="SPC.MNTUPDP001"  MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <style type="text/css">
    <!--
    .active {ime-mode: active;}
    .disabled {ime-mode: disabled;}
    -->
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="pnlMente" runat="server" >
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td class="align-top" Style= "padding-top :5px; width:150px;" >
                    <asp:Label ID="lblReqcomp_Cd" runat="server" Text="依頼先" Width="150px" Font-Size="Small" ></asp:Label>
                </td>
                <td class="align-top" style= "padding-left :4px;">
                    <asp:Panel ID="Panel5" runat="server">
                        <asp:DropDownList ID="ddlReqcomp_Cd" runat="server" Width="450px" ValidationGroup="1" ></asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:Panel ID="Panel6" runat="server" Width="0px">
                                <asp:CustomValidator ID="cuvReqcomp_Cd" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlReqcomp_Cd" ValidationGroup="1"></asp:CustomValidator>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
            <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td class="align-top" Style= "padding-top :6px; width:150px;">
                    <asp:Label ID="lblMente" runat="server" Text="管理番号" Width="150px" Font-Size="Small"  ></asp:Label>
                </td>
                <td class="align-top" Style= "padding-top :6px">
                    <asp:Label ID="lblMente_No" runat="server" Width="96px" ></asp:Label>
                </td>
                <td>
                    <table border="0" style="width:240px;">
                        <tr>
                            <td style="width:120px;">
                                <uc:ClsCMTextBox runat="server" ID="txtOrder_No" ppName="注文番号" ppCheckAc="False" ppMaxLength="4" ppRequiredField="True" ppCheckLength="False" ppWidth="65" ppValidationGroup="1" />
                            </td>
                            <td style="width:120px;">
                                <uc:ClsCMTextBox runat="server" ID="txtOrder_Seq" ppName="" ppCheckAc="False" ppMaxLength="6" ppRequiredField="True" ppCheckLength="False"  ppWidth="95" ppValidationGroup="1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td>
                    <asp:Label ID="lblTitle11" runat="server" Text="・" Width="15px" ></asp:Label>
                    <asp:Label ID="lblTitle12" runat="server" Text="整備依頼機器種別及び台数" Width="300px" Font-Underline="True" Font-Size="Medium" ></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
               <td Style="width :150px"></td>
               <td style= "padding-left :3px; width: 89px">
                    <asp:Label ID="Label7" runat="server" Text="機器分類" Width="108px" Font-Size="Small"  ></asp:Label>
                </td>
                 <td class="align-top" style= "padding-top :4px;padding-left :4px; width:150px">
                    <asp:Panel ID="Panel14" runat="server">
                        <asp:DropDownList ID="ddlRstAppaDiv" runat="server" Width="150px" ValidationGroup="1" AutoPostBack="True"></asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:Panel ID="Panel15" runat="server" Width="0px">
                                <asp:CustomValidator ID="valRstAppaDiv" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlRstAppaDiv" ValidationGroup="1"></asp:CustomValidator>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </td>
               <td style="width: 123px; height: 24px;"></td>
               <td class="align-top" Style= "padding-top :5px; width: 64px">
                    <asp:Label ID="Label8" runat="server" Text=" 機器種別" Width="64px" Font-Size="Small"  ></asp:Label>
                </td>
               <td class="align-top" style= "padding-top :4px;padding-left :4px; width:300px">
                    <asp:Panel ID="Panel16" runat="server">
                        <asp:DropDownList ID="ddlAppa_Nm" runat="server" Width="200px" ValidationGroup="1" AutoPostBack="True"></asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:Panel ID="Panel17" runat="server" Width="0px">
                                <asp:CustomValidator ID="cuvAppa_Nm" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlAppa_Nm" ValidationGroup="1"></asp:CustomValidator>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
        </table>

        <table border="0" style="width:1050px;">
            <tr>
               <td Style="width :150px"></td>
               <td style= "padding-left :3px; width: 89px">
                    <asp:Label ID="lblAppa_Nm" runat="server" Text="システムコード" Width="108px" Font-Size="Small"  ></asp:Label>
                </td>
                 <td class="align-top" style= "padding-top :4px;padding-left :4px; width:150px">
                    <asp:Panel ID="Panel11" runat="server">
                        <asp:DropDownList ID="ddlSystem" runat="server" Width="150px" ValidationGroup="1" AutoPostBack="True"></asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:Panel ID="Panel12" runat="server" Width="0px">
                                <asp:CustomValidator ID="cuvSystem" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlSystem" ValidationGroup="1"></asp:CustomValidator>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </td>
               <td style="width: 123px; height: 24px;"></td>
               <td class="align-top" Style= "padding-top :5px; width: 64px">
                    <asp:Label ID="Label5" runat="server" Text="型式/機器" Width="64px" Font-Size="Small"  ></asp:Label>
                </td>
               <td class="align-top" style= "padding-top :4px;padding-left :4px; width:300px">
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:DropDownList ID="ddlRstAppaModel" runat="server" Width="200px" ValidationGroup="1"></asp:DropDownList>
                        <div style="white-space: nowrap">
                            <asp:Panel ID="Panel2" runat="server" Width="0px">
                                <asp:CustomValidator ID="valRstAppaModel" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlRstAppaModel" ValidationGroup="1"></asp:CustomValidator>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
           <tr>
               <td Style="width :150px"></td>
               <td style= "width: 89px">
                   <asp:Label ID="Label6" runat="server" Text="台数" Width="108px" Font-Size="Small"  ></asp:Label>
               </td>
               <td>
                     <uc:ClsCMTextBox runat="server" ID="txtAppa_Cnt" ppName="台数" ppMaxLength="4" ppNum="True" ppRequiredField="True" ppValidationGroup="1" ppWidth="50" ppIMEMode="半角_変更不可" ppNameVisible="False" />
               </td>
           </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td>
                    <asp:Label ID="lblTitle21" runat="server" Text="・" Width="15px" ></asp:Label>
                    <asp:Label ID="lblTitle22" runat="server" Text="機器送付先、到着予定日及び台数" Width="300px" Font-Underline="True" Font-Size="Medium" ></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:150px;"></td>
                <td class="align-top"style="width:250px;">
                    <uc:ClsCMDateBox runat="server" ID="dtbArrval_D" ppName="到着日" ppNameWidth="108px" ppRequiredField="True" ppValidationGroup="1" />
                </td>
                <td class="align-top"Style= "padding-top :8px;width:70px;">
                    <asp:Label ID="Label2" runat="server" Text="台数" Width="70px" Font-Size="Small"  ></asp:Label>
                </td>
                <td class="align-top"Style= "padding-top :8px;width:70px;">
                    <asp:Label ID="lblArrval_Cnt" runat="server" Width="70px" ></asp:Label>
                </td>
                <td class="align-top" >
                    <uc:ClsCMTextBox runat="server" ID="txtSend_Nm" ppName="送付先名" ppMaxLength="20" ppRequiredField="True" ppValidationGroup="1"  ppWidth="350" />
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:150px;"></td>
                <td >
                    <uc:ClsCMTextBox runat="server" ID="txtSend_NoteText" ppName="備考" ppNameWidth="108px" ppWidth="700" ppMaxLength="50" ppRequiredField="False" ppValidationGroup="1" />
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td>
                    <asp:Label ID="lblTitle31" runat="server" Text="・" Width="15px" ></asp:Label>
                    <asp:Label ID="lblTitle32" runat="server" Text="作業依頼内容" Width="200px" Font-Underline="True"  Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:150px;"></td>
                <td class="align-top"style= "width:120px">
                    <uc:ClsCMDropDownList runat="server" ID="ddlWrkcls_cd" ppName="作業種別" ppNameWidth="108px" ppWidth="100" ppClassCD="0091" ppRequiredField="True" ppValidationGroup="1" ppNotSelect="True" />
                </td>
                <td class="align-top"style= "padding-top :6px ;width:70px">
                    <asp:Label ID="Label1" runat="server" Text="システム" Width="70px" Font-Size="Small"  ></asp:Label>
                </td>
                <td class="align-top"style= "padding-top :7px ;width:90px">
                    <asp:Label ID="lblSystem" runat="server" Width="87px" ></asp:Label>
                </td>
                 <td class="align-top"style= "width:140px">
                    <uc:ClsCMTextBox runat="server" ID="txtVersion" ppName="ＶＥＲ設定" ppCheckAc="False" ppIMEMode="半角_変更不可" ppMaxLength="5" ppRequiredField="False" ppValidationGroup="1" ppWidth="50" ppCheckHan="True" ppCheckLength="True" />
                </td>
                <td class="align-top"Style= "padding-top :6px;width:70px;">
                    <asp:Label ID="Label3" runat="server" Text="依頼台数" Width="70px" Font-Size="Small" ></asp:Label>
                </td>
                <td class="align-top"Style= "padding-top :6px;">
                    <asp:Label ID="lblReq_Cnt" runat="server" Width="70px" ></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:150px;"></td>
                <td >
                    <uc:ClsCMTextBox runat="server" ID="txtWork_NoteText" ppName="備考" ppNameWidth="108px" ppWidth="700" ppMaxLength="50" ppValidationGroup="1" />
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td>
                    <asp:Label ID="lblTitle41" runat="server" Text="・" Width="15px" ></asp:Label>
                    <asp:Label ID="lblTitle42" runat="server" Text="整備中故障の場合" Width="200px"  Font-Underline="True" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style= "width :150px"></td>
                <td style= "width :108px">
                    <asp:Label ID="lblMemCls" runat="server" Text="対応方法" Width="87px" Font-Size="Small"  ></asp:Label>
                </td>
                <td>
                    <asp:Panel ID="Panel3" runat="server">
                        <asp:RadioButtonList ID="rdlMemCls" runat="server" RepeatDirection="Horizontal" ValidationGroup="1">
                            <asp:ListItem Value="1">修理後整備する</asp:ListItem>
                            <asp:ListItem Value="2">修理せず返却する</asp:ListItem>
                            <asp:ListItem Value="3">発生時、連絡願います</asp:ListItem>
                        </asp:RadioButtonList>
                        <div style="white-space: nowrap">
                            <asp:Panel ID="Panel4" runat="server" Width="0px">
                                <asp:CustomValidator ID="cuvMemCls" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="rdlMemCls" ValidationGroup="1"></asp:CustomValidator>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:108px;"></td>
                <td>
                    <asp:Label ID="lblTitle51" runat="server" Text="・" Width="15px" ></asp:Label>
                    <asp:Label ID="lblTitle52" runat="server" Text="作業完了品希望納期" Width="200px" Font-Underline="True"  Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td Style="width :148px"></td>
                <td class="align-top" style= "width :238px">
                    <uc:ClsCMDateBox runat="server" ID="dtbDeliv_D" ppName="納入希望日" ppNameWidth="108px" ppValidationGroup="1" />
                </td>
               <td >
                    <uc:ClsCMTextBox runat="server" ID="txtDeliv" ppNameVisible="False" ppMaxLength="20" ppValidationGroup="1" ppWidth="350" />
                </td>
            </tr>
        </table>
            <table border="0" style="width:1050px;">
            <tr>
                <td Style="width :150px"></td>
                <td>
                    <asp:Label ID="lblTitle6" runat="server" Text="完了後の納入先" Width="100px" Font-Size="Small"  ></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;" >
           <tr>
               <td Style="width :150px"></td>
               <td style= "width: 89px">
                   <asp:Label ID="Label4" runat="server" Text="納入先" Width="107px" Font-Size="Small"  ></asp:Label>
               </td>
               <td style= "padding-left :4px">
                   <asp:Panel ID="Panel7" runat="server">
                       <asp:DropDownList ID="ddlDeliv_Cd" runat="server" Width ="350" AutoPostBack="True" ValidationGroup="1">
                       </asp:DropDownList>
                           <div style="white-space: nowrap">
                               <asp:Panel ID="Panel8" runat="server" Width="0px">
                                   <asp:CustomValidator ID="cuvDeliv_Cd" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="ddlDeliv_Cd" ValidationGroup="1"></asp:CustomValidator>
                               </asp:Panel>
                           </div>
                   </asp:Panel>
               </td>
               <td></td>
           </tr>
        </table>
        <table border="0" style="width:1050px;">
           <tr>
               <td Style="width :150px"></td>
               <td style= "padding-left :3px; width: 89px">
                   <asp:Label ID="lblZipNoNm" runat="server" Text="郵便番号" Width="108px" Font-Size="Small"  ></asp:Label>
               </td>
               <td>
                   <asp:TextBox ID="txtZipNo" runat="server" MaxLength="8" ValidationGroup="1" CssClass="disabled"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="Panel9" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvZipNo" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="rdlMemCls" ValidationGroup="1"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
               </td>
           </tr>
        </table>
        <table border="0" style="width:1050px;">
           <tr style=" height:40px">
               <td Style="width :150px"></td>
               <td style= "padding-left :3px; width: 89px">
                   <asp:Label ID="lblAddrNm" runat="server" Text="住所" Width="108px" Font-Size="Small" ></asp:Label>
               </td>
               <td>
                   <asp:TextBox ID="txtAddr" runat="server" Width="720px" MaxLength="50" ValidationGroup="1"></asp:TextBox>
               </td>
           </tr>
        </table>
        <table border="0" style="width:1050px;">
           <tr>
               <td Style="width :150px"></td>
               <td style= "padding-left :3px; width: 89px">
                   <asp:Label ID="lblTelNo" runat="server" Text="ＴＥＬ" Width="108px" Font-Size="Small" ></asp:Label>
               </td>
              <td Style="width :100px">
                   <asp:TextBox ID="txtTel" runat="server"  Width="230px" MaxLength="15" ValidationGroup="1" CssClass="disabled"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="Panel10" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvTel" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="rdlMemCls" ValidationGroup="1"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
               </td>
               <td style="width: 73px; height: 44px;"></td>
               <td style= "padding-top :3px; width: 89px">
                   <asp:Label ID="lblFAX" runat="server" Text="ＦＡＸ" Width="50px" ></asp:Label>
               </td>
               <td>
                   <asp:TextBox ID="txtFAX" runat="server"  MaxLength="20" Width="230px" ValidationGroup="1" CssClass="disabled"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="Panel13" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvFAX" runat="server" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" ControlToValidate="rdlMemCls" ValidationGroup="1"></asp:CustomValidator>
                        </asp:Panel>
                    </div>
               </td>
           </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td style="width:150px;"></td>
                <td >
                    <uc:ClsCMTextBox runat="server" ID="txtCeriv_NoteText" ppName="備考" ppNameWidth="108" ppMaxLength="50" ppValidationGroup="1" ppWidth="750" />
                </td>
            </tr>
        </table>
     </asp:Panel>
        <table border="0" style="width:1050px;">
            <tr>
                 <td>
                 </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td>
                    <div class="float-left">
                        <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="1" />
                    </div>
                </td>
            </tr>
        </table>
        <table border="0" style="width:1050px;">
            <tr>
                <td Style="width :108px"></td>
                
                <td>
                    <asp:Label ID="lblTitle7" runat="server" Font-Bold="True">整備機器一覧</asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td Style="width :108px"></td>
                <td class ="align-top">
                           <div class="grid-out">
                               <div class="grid-in">
                                     <input id="hdnData" type="hidden" runat="server" class="grid-data" />
                                     <asp:GridView ID="grvList" runat="server">
                                     </asp:GridView>
                               </div>
                           </div>
                </td>
            </tr>
        </table>
    <%--</asp:Panel>--%>
        <table border="0" style="width:1050px;">
           <tr>
               <td style ="width:80px;">

               </td>
               <td>
                   <div class="float-left">
                       <%--<asp:Button ID="btnPrint" runat="server" Text="印刷" Width="62px" />--%>
                   </div>
               <td>
                   <div class="float-right">
                       <%--                       <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                       <asp:Button ID="Button2" runat="server" Text="Button" Visible="False" />
                       <asp:Button ID="btnClear" runat="server" Text="クリア" />
                       <asp:Button ID="btnDelete" runat="server" Text="削除" />
                       <asp:Button ID="btnUpdate" runat="server" Text="登録" ValidationGroup="1" Height="21px" Width="62px" />--%>
                   </div>
               </td>
           </tr>
        </table>
</asp:Content>
