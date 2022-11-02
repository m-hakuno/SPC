<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Site.Master" AutoEventWireup="false" CodeBehind="COMUPDM32.aspx.vb" Inherits="SPC.COMUPDM32" %>
<%@ MasterType VirtualPath="~/Master/Site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SearchContent" runat="server">

        <!--テーブル検索配置-->

         <table class="center" style="width:300px;">
             <tr>
                 <td> 
                     <asp:Label ID="Label38" runat="server" Text="画面ID"></asp:Label>
                     <br />
                     <asp:DropDownList ID="ddlSrcDispCd" runat="server" Width="150px">
                     </asp:DropDownList>
                     <br />
            <td>

                <asp:Label ID="Label1" runat="server" Text="画面名"></asp:Label>
                <br />
                <asp:TextBox ID="txtSrcName" runat="server" MaxLength="20" Width="250px"></asp:TextBox>
            </td>
            </tr>
             <tr>
                 <td>
                     <asp:Label ID="Label39" runat="server" Text="初期最大件数"></asp:Label>
                     <br />
                     <asp:TextBox ID="txtSrcDefaultMax" runat="server" AutoPostBack="True" Width="100px" MaxLength="6"></asp:TextBox>
                 </td>
                 <td>
                     <asp:Label ID="Label2" runat="server" Text="検索結果件数"></asp:Label>
                     <br />
                     <asp:TextBox ID="txtSrcSearchMax" runat="server" AutoPostBack="True" Width="100px" MaxLength="6"></asp:TextBox>
                 </td>
             </tr>
        </table>
    

         </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

       
   <!--メイン入力テーブル-->
    <table border="1" id="tblMain" class="center"style="margin-left:auto; margin-right:auto; width:548px; height:50px;">
        <tr style="display:none;">
            <td class="auto-style12">
            </td><td class="auto-style12">
                </td>
            <td class="auto-style12">
                </td>
              
        </tr>
        <tr style="text-align:center;">
            <td class="auto-style14" colspan="2">
            <asp:Label ID="Label36" runat="server" Text="画面ID・画面名" Width="160px"></asp:Label>
            </td>
           
            <td class="auto-style14" rowspan="4">
               <asp:Button ID="btnClear" runat="server" Text="クリア" TabIndex="81" /><br /><br />
                <asp:Button ID="btnAdd" runat="server" Text="追加" TabIndex="82" />
                <asp:Button ID="btnUpd" runat="server" Text="更新" TabIndex="83" />
            </td>
 </tr>
        <tr style="text-align:center;">
            <td class="auto-style14" colspan="2">
                <asp:DropDownList ID="ddlDispCd" runat="server" ValidationGroup="Val" Width="300px">
                </asp:DropDownList>
                <br />
                <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="ddlDispCd" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val"></asp:CustomValidator>
            </td>
          
        </tr>
        <tr style="text-align:center;">
           <td class="auto-style14">
                <asp:Label ID="Label3" runat="server" Text="初期最大件数"></asp:Label>
            </td>
             <td class="auto-style14">
                <asp:Label ID="Label4" runat="server" Text="検索結果件数"></asp:Label>
            </td>
        </tr>
        <tr style="text-align:center; ">
             <td class="auto-style14">
                <asp:TextBox ID="txtDefaultMax" runat="server" ValidationGroup="Val" Width="100px" MaxLength="6"  CssClass="IMEdisabled" AutoPostBack="True"></asp:TextBox>
                <br />
                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtDefaultMax" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val"></asp:CustomValidator>
            </td>
            <td class="auto-style14">
                <asp:TextBox ID="txtSearchMax" runat="server" ValidationGroup="Val" Width="100px" MaxLength="6" AutoPostBack="True"></asp:TextBox>
                <br />
                <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtSearchMax" CssClass="errortext" Display="Dynamic" ErrorMessage="CustomValidator" ValidateEmptyText="True" ValidationGroup="val"></asp:CustomValidator>
            </td>
        </tr>
        </table>
            

            <!-- SQL実行確認Table -->

         <br />
       



            <!-- グリッドビュー -->
    <div id="Div1" runat="server" style="position : relative; padding-top: 20px;width:580px; border: 1px solid #0099cc;background-color: #fff;margin-left:auto;margin-right:auto;">
        <div id="Div2" runat="server" style="overflow: auto; height: 250px;" tabindex="91" >
               <asp:GridView ID="GrdV" runat="server" AutoGenerateColumns="False" Font-Names="ＭＳ ゴシック" ShowHeaderWhenEmpty="True" Font-Size="10pt" TabIndex="91" HorizontalAlign="Left">
                   <Columns>
                       <asp:ButtonField ButtonType="Button" CommandName="btnVSel" Text="選択" >
                       <HeaderStyle Height="20px" Width="50px" />
                       <ItemStyle Width="50px" VerticalAlign="Middle" />
                       </asp:ButtonField>
                       <asp:TemplateField HeaderText="画面ID・画面名" SortExpression="VDispCd">
                       <ItemTemplate>
                            <asp:Label ID="lblVDispCd"  runat="server" Text='<%# Eval("CLS")%>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="300px" />
                           <ItemStyle Width="300px" HorizontalAlign="left" VerticalAlign="Middle" />
                       </asp:TemplateField>
                       <asp:BoundField DataField="M32_DEFAULT_MAX" DataFormatString="{0:N0}" HeaderText="初期最大件数" SortExpression="VDefaultMax">
                       <HeaderStyle Width="100px" />
                       <ItemStyle Width="100px" HorizontalAlign="Right" />
                       </asp:BoundField>
                       <asp:BoundField DataField="M32_SEARCH_MAX" DataFormatString="{0:N0}" HeaderText="検索結果件数" SortExpression="VSearchMax">
                       <HeaderStyle Width="100px" />
                       <ItemStyle Width="100px" HorizontalAlign="Right" />
                       </asp:BoundField>
                      
                       
                   </Columns>
                   <HeaderStyle cssclass="Freezing"/>
               </asp:GridView>
               </div>
               </div>
<style type="text/css">
        .auto-style12
        {
            height: 25px;
    
        }
        .auto-style14
        {
            height: 25px;
        }
        </style>

         </asp:Content>


    

