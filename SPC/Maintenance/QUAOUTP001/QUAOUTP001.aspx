<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Reference.master" CodeBehind="QUAOUTP001.aspx.vb" Inherits="SPC.QUAOUTP001" %>
<%@ MasterType VirtualPath="~/Reference.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSearchContent" runat="server">
    <table border="0" class="center">
        <tr>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftStartDt" runat="server" ppName="対応年月" ppNameWidth="70" ppDateFormat="年月" ppRequiredField="False" />
            </td>
        </tr>
        <tr>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlClass" ppName="種　　別" ppClassCD="0105" ppNameWidth="70" ppNotSelect="True" ppRequiredField="True" />
             </td>
        </tr>
    </table>
<%--    <table style="width:1036px;" class="center" border="0">
        <tr>
            <td style="width: 300px">
            </td>
            <td>
                <uc:ClsCMDateBoxFromTo ID="dftSupportDt" runat="server" ppName="対応年月" ppNameWidth="80" ppDateFormat="年月" />
            </td>
        </tr>
        <tr>
            <td style="width: 300px">
            </td>
            <td>
                <uc:ClsCMDropDownList runat="server" ID="ddlTboxType" ppName="種別" ppClassCD="0006" />
            </td>
        </tr>
    </table>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphUpdateContent" runat="server">
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphListContent" runat="server">
    <%--    <div style="border:1px solid; margin:0px; padding:10px; border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;">--%>
    <table border="0" class="center">
        <tr>
            <td>
                <table border="0">
                    <tr>
                        <td style="width: 70px">
                             <asp:Label ID="lblClass1" runat="server" Text="種　　別"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblClass2" runat="server" Text="XXXXXXXXXX"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlGrv" runat="server" style="border:1px solid; margin:0px; padding:10px; border-radius: 10px;" Width="1010px">
                    <table border="0" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblAppaclsNm1" runat="server" Text="制御部"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="DivOutControl" runat="server" class="grid-out">
                                    <div id="DivInControl" runat="server" class="grid-in">
                                        <input id="hdnDataControl" type="hidden" runat="server" class="grid-data" />
                                        <asp:GridView ID="grvListControl" runat="server" CssClass="grid" ShowHeaderWhenEmpty="True" AlternatingRowStyle-CssClass="grid-row2" HeaderStyle-CssClass="grid-head" RowStyle-CssClass="grid-row1" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="grid-row2" />
                                            <Columns>
                                                <asp:BoundField DataField="部位" HeaderText="部位">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="目標値" HeaderText="目標値">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量名称" HeaderText="数量">
                                                <HeaderStyle Width="125px" />
                                                <ItemStyle Width="125px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量1" DataFormatString="{0:#,###}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量2" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量3" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量4" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量5" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量6" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量7" HeaderText="合計" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <%--                                    <asp:BoundField DataField="故障率" HeaderText="故障率" Visible="False" />
                                                <asp:BoundField DataField="コード" HeaderText="コード" Visible="False" />
                                                <asp:BoundField DataField="番号" HeaderText="番号" Visible="False" />--%>
                                            </Columns>
                                            <HeaderStyle CssClass="grid-head" />
                                            <RowStyle CssClass="grid-row1" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAppaclsNm2" runat="server" Text="プリンタ/ディスプレイ/操作盤"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="DivOutParts" runat="server" class="grid-out">
                                    <div id="DivInParts" runat="server" class="grid-in">
                                        <input id="hdnDataParts" type="hidden" runat="server" class="grid-data" />
                                        <asp:GridView ID="grvListParts" runat="server" CssClass="grid" ShowHeaderWhenEmpty="True" AlternatingRowStyle-CssClass="grid-row2" HeaderStyle-CssClass="grid-head" RowStyle-CssClass="grid-row1" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="grid-row2" />
                                            <Columns>
                                                <asp:BoundField DataField="部位" HeaderText="部位">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="目標値" HeaderText="目標値">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量名称" HeaderText="数量">
                                                <HeaderStyle Width="125px" />
                                                <ItemStyle Width="125px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量1" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量2" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量3" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量4" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量5" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量6" DataFormatString="{0:N0}">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="数量7" DataFormatString="{0:N0}" HeaderText="合計">
                                                <HeaderStyle Width="90px" />
                                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                </asp:BoundField>
                                                <%--                                    <asp:BoundField DataField="故障率" HeaderText="故障率" Visible="False" />
                                                <asp:BoundField DataField="コード" HeaderText="コード" Visible="False" />
                                                <asp:BoundField DataField="番号" HeaderText="番号" Visible="False" />--%>
                                            </Columns>
                                            <HeaderStyle CssClass="grid-head" />
                                            <RowStyle CssClass="grid-row1" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--    </div>--%>
<%--     <div class="grid-out">
         <asp:Label ID="Label2" runat="server" Text="制御部"></asp:Label>
         <div class="grid-in">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server">
            </asp:GridView>
        </div>
    </div>
    <div class="grid-out">
        <asp:Label ID="Label3" runat="server" Text="プリンタ・ディスプレイ"></asp:Label>
        <div class="grid-in">
            <input id="Hidden1" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList1" runat="server">
            </asp:GridView>
        </div>
    </div>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphDetailContent" runat="server">
</asp:Content>
