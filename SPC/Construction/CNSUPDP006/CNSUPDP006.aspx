<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CNSUPDP006.aspx.vb" Inherits="SPC.CNSUPDP006" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <Script src='<%= Me.ResolveClientUrl("~/Scripts/tableCalculation.js")%>'></Script>
    <Script type="text/javascript">
        function pageLoad() {
            setTableSum('<%= Me.grvList1.ClientID%>', 1);
            setTableSum('<%= Me.grvList2.ClientID%>', 2);
            setTableSum('<%= Me.grvList3.ClientID%>', 3);
        }
    </Script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="width: 1050px; white-space: nowrap;" border="1" class="text-center">
        <tr>
            <td style="width: 150px">
                <asp:Label ID="lblbConstNo1" runat="server" Text="工事依頼番号"></asp:Label>
            </td>
            <td style="width: 200px">
                <asp:Label ID="lblbConstNo2" runat="server"></asp:Label>
            </td>
            <td style="width: 150px">
                <asp:Label ID="lblConstcomNo1" runat="server" Text="工事通知番号"></asp:Label>
            </td>
            <td style="width: 200px">
                <asp:Label ID="lblConstcomNo2" runat="server"></asp:Label>
            </td>
            <td style="width: 150px">
                <asp:Label ID="lblTotalBef1" runat="server" Text="データ締日"></asp:Label>
            </td>
            <td style="width: 200px">
                <asp:Label ID="lblTotalBef2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label11" runat="server" Text="工事種別"></asp:Label>
            </td>
            <td colspan="5">
                <table border="1" style="white-space: nowrap">
                    <tr class="text-center">
                        <td style="width: 55px">
                            <asp:Label ID="lblNew1" runat="server" Text="新規"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblAdd1" runat="server" Text="増設"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblReset1" runat="server" Text="再設置"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblRelocate1" runat="server" Text="店内移設"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblPrtRemove1" runat="server" Text="一部撤去"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblAllRemove1" runat="server" Text="全撤去"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblTmpRemove1" runat="server" Text="一時撤去"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblChngOrgnz1" runat="server" Text="構成変更"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblDlvOrgnz1" runat="server" Text="構成配信"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblVup1" runat="server" Text="ＶＵＰ"></asp:Label>
                        </td>
                        <td style="width: 55px">
                            <asp:Label ID="lblOth1" runat="server" Text="その他"></asp:Label>
                        </td>
                        <td style="width: 70px">
                            <asp:Label ID="lblOthDtl1" runat="server" Text="その他内容"></asp:Label>
                        </td>
                    </tr>
                    <tr class="text-center">
                        <td>
                            <asp:Label ID="lblNew2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblAdd2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblReset2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblRelocate2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblPrtRemove2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblAllRemove2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblTmpRemove2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblChngOrgnz2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblDlvOrgnz2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblVup2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblOth2" runat="server"></asp:Label><br />
                        </td>
                        <td>
                            <asp:Label ID="lblOthDtl2" runat="server"></asp:Label><br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    <div class="text-center">
        <asp:Label ID="Label2" runat="server" Text="設置工事" Font-Bold="True"></asp:Label>
    </div>

    <asp:GridView ID="grvList1" runat="server" AutoGenerateColumns="False" CssClass="center">
        <Columns>
            <asp:BoundField HeaderText="機器／部材" DataField="機器部材" >
            <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="工事前台数">
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="old_row" runat="server" ppName='<%# Eval("機器部材") & "の工事台数"%>' ppNameVisible="False" ppText='<%# Eval("工事前台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移設台数">
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="mov_row" runat="server" ppName='<%# Eval("機器部材") & "の移設台数"%>' ppNameVisible="False" ppText='<%# Eval("移設台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="撤去台数">                
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="rem_row" runat="server" ppName='<%# Eval("機器部材") & "の撤去台数"%>' ppNameVisible="False" ppText='<%# Eval("撤去台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="設置台数">                
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="newtot_row" runat="server" ppName='<%# Eval("機器部材") & "の設置台数"%>' ppNameVisible="False" ppText='<%# Eval("設置台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="既存機器流用台数">                
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="divertot_row" runat="server" ppName='<%# Eval("機器部材") & "の既存機器流用台数"%>' ppNameVisible="False" ppText='<%# Eval("既存機器流用台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工事後台数">                
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("機器部材") & "の合計"%>' Visible="False"></asp:Label>
                    <asp:TextBox ID="afttot" runat="server" Text='<%# Eval("工事後台数")%>' style="text-align:right" Width="100"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlAfttotErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvAfttot" runat="server" ControlToValidate="afttot" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" OnServerValidate="sum_ServerValidate"></asp:CustomValidator>                                
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <hr />
    <div class="text-center">
        <asp:Label ID="Label1" runat="server" Text="店内設置工事" Font-Bold="True"></asp:Label>

    </div>
    <asp:GridView ID="grvList2" runat="server" AutoGenerateColumns="False" CssClass="center">
        <Columns>
            <asp:BoundField HeaderText="機器／部材" DataField="機器部材" >
            <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="工事前台数">
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="store_old_row" runat="server" ppName='<%# Eval("機器部材") & "の工事前台数"%>' ppNameVisible="False" ppText='<%# Eval("工事前台数") %>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移設台数">
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="store_mov_row" runat="server" ppName='<%# Eval("機器部材") & "の移設台数"%>' ppNameVisible="False" ppText='<%# Eval("移設台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="撤去台数">                
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="store_rem_row" runat="server" ppName='<%# Eval("機器部材") & "の撤去台数"%>' ppNameVisible="False" ppText='<%# Eval("撤去台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="設置台数">                
                <ItemTemplate>
                    <uc:ClsCMTextBox ID="store_newtot_row" runat="server" ppName='<%# Eval("機器部材") & "の設置台数"%>' ppNameVisible="False" ppText='<%# Eval("設置台数")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="100" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工事後台数">                
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("機器部材") & "の合計"%>' Visible="False"></asp:Label>
                    <asp:TextBox ID="store_afttot" runat="server" Text='<%# Eval("工事後台数")%>' style="text-align:right" Width="100"></asp:TextBox>
                    <div style="white-space: nowrap">
                        <asp:Panel ID="pnlStore_afttotErr" runat="server" Width="0px">
                            <asp:CustomValidator ID="cuvStore_afttot" runat="server" ControlToValidate="store_afttot" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" OnServerValidate="sum_ServerValidate"></asp:CustomValidator>                                
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <table>
        <tr>
            <td>
                <asp:Label ID="lblFrequenxy" runat="server" Text="周波数"></asp:Label>
            </td>
            <td>
                <table border="1">
                    <tr class="text-center">
                        <td>
                            <asp:Label ID="lblFrequenxy1" runat="server" Text="１"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFrequenxy2" runat="server" Text="２"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFrequenxy3" runat="server" Text="３"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFrequenxy4" runat="server" Text="４"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFrequenxy5" runat="server" Text="５"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFrequenxy6" runat="server" Text="６"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlFrequenxy1" ppClassCD="0082" ppNameVisible="False" ppName="周波数１" ppNotSelect="True" />
                        </td>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlFrequenxy2" ppClassCD="0082" ppNameVisible="False" ppName="周波数２" ppNotSelect="True" />
                        </td>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlFrequenxy3" ppClassCD="0083" ppNameVisible="False" ppName="周波数３" ppNotSelect="True" />
                        </td>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlFrequenxy4" ppClassCD="0082" ppNameVisible="False" ppName="周波数４" ppNotSelect="True" />
                        </td>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlFrequenxy5" ppClassCD="0082" ppNameVisible="False" ppName="周波数５" ppNotSelect="True" />
                        </td>
                        <td>
                            <uc:ClsCMDropDownList runat="server" ID="ddlFrequenxy6" ppClassCD="0082" ppNameVisible="False" ppName="周波数６" ppNotSelect="True" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <table>
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlFsbltyStdy" ppClassCD="0086" ppName="事前調査" ppNotSelect="True" />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlCstmluckUSE" ppClassCD="0086" ppName="改造ラック" ppNotSelect="True" />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlUpsincorp" ppClassCD="0086" ppName="ＵＰＳ内蔵" ppNotSelect="True" />
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlTBLuck" ppClassCD="0084" ppName="ＴＢラック" ppNotSelect="True" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <uc:ClsCMDropDownList runat="server" ID="ddlTboxToCls" ppClassCD="0085" ppName="ＴＢＯＸ持帰" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <uc:ClsCMTextBox runat="server" ID="txtImptntNotice" ppTextMode="MultiLine" ppName="特記事項" ppMaxLength="100" />
            </td>
        </tr>
    </table>

    <table class="center">
        <tr class="text-center">
            <td>
                <asp:Label ID="Label5" runat="server" Width="200px"></asp:Label>
            </td>
            <td>
                <table style="table-layout: fixed; width: 100%;">
                    <tr class="text-center">
                        <td>
                            <asp:Label ID="lblNew" runat="server" Text="新設（今回取り付け）"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDiv" runat="server" Text="撤去設（内撤去品利用）"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAFT" runat="server" Text="工事後"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="grvList3" runat="server" AutoGenerateColumns="False" CssClass="center">
                    <Columns>
                        <%-- 新設 --%>
                        <asp:TemplateField HeaderText="機器／部材">
                            <ItemTemplate>
                                <asp:Panel ID="pnlName" runat="server" Width="200">
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("機器部材") %>'></asp:Label>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ１">
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="new_f1_row" runat="server" ppName='<%# "新設　" & Eval("機器部材") & "のＦ１"%>' ppNameVisible="False" ppText='<%# Eval("新設Ｆ１")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ２">
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="new_f2_row" runat="server" ppName='<%# "新設　" & Eval("機器部材") & "のＦ２"%>' ppNameVisible="False" ppText='<%# Eval("新設Ｆ２")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ３">                
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="new_f3_row" runat="server" ppName='<%# "新設　" & Eval("機器部材") & "のＦ３"%>' ppNameVisible="False" ppText='<%# Eval("新設Ｆ３")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ４">                
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="new_f4_row" runat="server" ppName='<%# "新設　" & Eval("機器部材") & "のＦ４"%>' ppNameVisible="False" ppText='<%# Eval("新設Ｆ４")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="合計">                
                            <ItemTemplate>
                                <asp:Label ID="lblNewName" runat="server" Text='<%# "新設　" & Eval("機器部材") & "の合計"%>' Visible="False"></asp:Label>
                                <asp:TextBox ID="new_f" runat="server" Text='<%# Eval("新設合計")%>' style="text-align:right" Width="70"></asp:TextBox>
                                <div style="white-space: nowrap">
                                    <asp:Panel ID="pnlNew_fErr" runat="server" Width="0px">
                                        <asp:CustomValidator ID="cuvNew_f" runat="server" ControlToValidate="new_f" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" OnServerValidate="cuvNew_f_ServerValidate"></asp:CustomValidator>                                
                                    </asp:Panel>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- 撤去設 --%>
                        <asp:TemplateField HeaderText="Ｆ１">
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="div_f1_row" runat="server" ppName='<%# "撤去設　" & Eval("機器部材") & "のＦ１"%>' ppNameVisible="False" ppText='<%# Eval("撤去設Ｆ１")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ２">
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="div_f2_row" runat="server" ppName='<%# "撤去設　" & Eval("機器部材") & "のＦ２"%>' ppNameVisible="False" ppText='<%# Eval("撤去設Ｆ２")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ３">                
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="div_f3_row" runat="server" ppName='<%# "撤去設　" & Eval("機器部材") & "のＦ３"%>' ppNameVisible="False" ppText='<%# Eval("撤去設Ｆ３")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ４">                
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="div_f4_row" runat="server" ppName='<%# "撤去設　" & Eval("機器部材") & "のＦ４"%>' ppNameVisible="False" ppText='<%# Eval("撤去設Ｆ４")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="合計">                
                            <ItemTemplate>
                                <asp:Label ID="lblDivName" runat="server" Text='<%# "撤去設　" & Eval("機器部材") & "の合計"%>' Visible="False"></asp:Label>
                                <asp:TextBox ID="div_f" runat="server" Text='<%# Eval("撤去設合計")%>' style="text-align:right" Width="70"></asp:TextBox>
                                <div style="white-space: nowrap">
                                    <asp:Panel ID="pnlDiv_fErr" runat="server" Width="0px">
                                        <asp:CustomValidator ID="cuvDiv_f" runat="server" ControlToValidate="div_f" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" OnServerValidate="cuvDiv_f_ServerValidate"></asp:CustomValidator>                                
                                    </asp:Panel>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- 工事後 --%>
                        <asp:TemplateField HeaderText="Ｆ１">
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="aft_f1_row" runat="server" ppName='<%# "工事後　" & Eval("機器部材") & "のＦ１"%>' ppNameVisible="False" ppText='<%# Eval("工事後Ｆ１")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ２">
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="aft_f2_row" runat="server" ppName='<%# "工事後　" & Eval("機器部材") & "のＦ２"%>' ppNameVisible="False" ppText='<%# Eval("工事後Ｆ２")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ３">                
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="aft_f3_row" runat="server" ppName='<%# "工事後　" & Eval("機器部材") & "のＦ３"%>' ppNameVisible="False" ppText='<%# Eval("工事後Ｆ３")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ｆ４">                
                            <ItemTemplate>
                                <uc:ClsCMTextBox ID="aft_f4_row" runat="server" ppName='<%# "工事後　" & Eval("機器部材") & "のＦ４"%>' ppNameVisible="False" ppText='<%# Eval("工事後Ｆ４")%>' ppTextAlign="右" ppIMEMode="半角_変更不可" ppMaxLength="4" ppNum="True" ppCheckHan="True" ppWidth="70" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="合計">                
                            <ItemTemplate>
                                <asp:Label ID="lblAftName" runat="server" Text='<%# "工事後　" & Eval("機器部材") & "の合計"%>' Visible="False"></asp:Label>
                                <asp:TextBox ID="aft_f" runat="server" Text='<%# Eval("工事後合計")%>' style="text-align:right" Width="70"></asp:TextBox>
                                <div style="white-space: nowrap">
                                    <asp:Panel ID="pnlAft_fErr" runat="server" Width="0px">
                                        <asp:CustomValidator ID="cuvAft_f" runat="server" ControlToValidate="aft_f" CssClass="errortext" ErrorMessage="CustomValidator" EnableClientScript="False" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic" OnServerValidate="cuvAft_f_ServerValidate"></asp:CustomValidator>                                
                                    </asp:Panel>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errortext" />
</asp:Content>
