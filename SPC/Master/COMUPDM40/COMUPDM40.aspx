<%@ Page Title="" Language="VB" MasterPageFile="~/Master/COMUPDM40/COMUPDM40_Mst.Master" AutoEventWireup="false" CodeBehind="COMUPDM40.aspx.vb" Inherits="SPC.COMUPDM40" %>
<%@ MasterType VirtualPath="~/Master/COMUPDM40/COMUPDM40_Mst.Master" %>

<%--入力文字数検知とフォーカスチェンジのスクリプト--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
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

    <style type="text/css">
        .auto-style16
        {
            width: 140px;
        }
        .auto-style18
        {
            width: 230px;
        }
        .auto-style23
        {
            width: 185px;
        }
        .auto-style28
        {
            width: 466px;
        }
        .auto-style29
        {
            width: 335px;
        }
        .auto-style55
        {
            width: 290px;
        }
        .auto-style58
        {
            width: 195px;
        }
        .auto-style59
        {
            width: 220px;
        }
        .auto-style60
        {
            width: 215px;
        }
        .auto-style61
        {
            height: 23px;
        }
        </style>
    </asp:Content>

<asp:Content runat="server" ID="SearchContent" ContentPlaceHolderID="SearchContent">
    <table style="width:1020px;margin-left:auto;margin-right:auto;border:none;text-align:left">
        <tr>
            <td class="auto-style18">
            <table>
                <tr>
                 <td>
                     <asp:Label ID="Label2" runat="server" Text="ＮＬ区分" Width="90px" style="margin-bottom: 0px"></asp:Label>
                 </td>
            <td>
                 <asp:DropDownList ID="ddlSNL" runat="server" Width="110" ></asp:DropDownList>
            </td>
                    </tr>
                </table>
                </td>
             <td class="auto-style18">
                 <table>
                     <tr>
                         <td class="auto-style61">
                             <asp:Label ID="Label3" runat="server" Text="システム" Width="90px"></asp:Label>
                        </td>
                         <td class="auto-style61">
                <asp:DropDownList ID="ddlSTbox" runat="server" Width="112"></asp:DropDownList>
            </td>
                     </tr>
                     </table>
                 </td>
            
            <td colspan="2">
                <table>
                    <tr>
                     <td>
                           
                        <asp:Label ID="Label1" runat="server" Text="印刷区分" Width="90px"></asp:Label>
                    </td>
             <td class="auto-style16">
                <asp:DropDownList ID="ddlSprint" runat="server" Width="130"></asp:DropDownList>
            </td>
                          <td class="auto-style18">
                <uc:ClsCMDropDownList ID="ddldel" runat="server" ppName="削除区分" ppNameWidth="90" ppWidth="120" ppClassCD="0124" ppNotSelect="true"/>
            </td>
               </tr>
    </table>
                </td>
            </tr>
        </table>

</asp:Content>

<script runat="server">
    Sub ddl_check(source As Object, arguments As ServerValidateEventArgs)
        Dim ddl_val As String = (arguments.Value)
        arguments.IsValid = (ddl_val <> "")
    End Sub
</script>



<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div style="margin-top:8px">
        <hr>
    </div> 
    
   
              <%--  <div class="float-left">
                    <asp:ValidationSummary ID="vasSummary" runat="server" CssClass="errortext" ValidationGroup="Edit" />
                </div>--%>
       
        <table  style="border-style: none; border-color: inherit; border-width: medium; width:1000px;margin-left:50px; margin-right:auto; text-align:left;">
        <tr>
            <td class="auto-style55">
                 <asp:Label ID="Label5" runat="server" Text="ＮＬ区分" Width="97px"></asp:Label>
                     <asp:DropDownList ID="ddlNL" runat="server" Width="100"></asp:DropDownList>
                <asp:Label ID="lblJac" runat="server" Text="※JACKYを含む" Width="92px" Visible="False" ForeColor="Blue" Font-Bold="true" ></asp:Label>
             </td>
                        <td class="auto-style60">
                           <asp:Label ID="Label4" runat="server" Text="システム" Width="80px" style="margin-left: 0px"></asp:Label>
                          <asp:DropDownList ID="ddlTbox" runat="server" Width="120"></asp:DropDownList>
                       
<%--                <asp:Label ID="Label10" runat="server" Text="連番" Width="40"　visible="false"></asp:Label>--%>
                <asp:Label ID="lblTrun" runat="server" Text="" Width="0" Visible="false"></asp:Label>
                       <%--<uc:ClsCMTextBox ID="txtTrun" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="2" ppName="連番" ppNameVisible="false" ppNameWidth="100" ppWidth="30" ppCheckHan="True" ppValidationGroup="key" ppNum="true" Visible="false"/>--%>
                            
             </td>
              
              <td class="auto-style59" >
                       <%--<asp:Label ID="Label6" runat="server" Text="印刷区分" Width="75px" style="margin-left: 0px"></asp:Label> --%>
                  <uc:ClsCMDropDownList ID="ddlprint" runat="server" ppName="印刷区分"　ppnamewidth="75px" ppWidth="122px" ppValidationGroup="val" ppClassCD="" ppNotSelect="true" ppRequiredField="true" /> 
                          <%-- <asp:DropDownList ID="ddlprint" runat="server" Width="125px" ValidationGroup="val" Height="18px" ></asp:DropDownList>--%>
<%--                <div style="white-space: nowrap">
                                <asp:Panel ID="pnlSystemErr" runat="server" Width="200px">
                                    <asp:CustomValidator ID="cuvTrun" runat="server" OnServerValidate="ddl_check" ValidationGroup="val" ControlToValidate="ddlprint" CssClass="errortext" ErrorMessage="未入力エラー" EnableClientScript="True" ValidateEmptyText="True" SetFocusOnError="True" Display="Dynamic"></asp:CustomValidator>
                                </asp:Panel>
                            </div>   --%>
                       </td>
            <td class="auto-style58">
                           
             <%--<div style="float:left;margin-left:12px;" >--%>
               <asp:Button ID="btnPrint" runat="server"  Text="プレビュー"  />
              
         <%--  </div>--%>
           </td>
                </tr>
      </table>
 
      
    <hr />

    <div style="float:left;margin-left:80px;margin-top:4px;text-align:left" >
        <asp:Label ID="Label7" runat="server" Text="故障機器送付先１" Width="500"></asp:Label>
     </div>
      
   
        <table style="width:1000px;margin-left:auto;margin-right:auto;border:none;text-align:left">
      
     
       
            <tr>
            <td colspan="5">
                <uc:ClsCMTextBox ID="txtName1"  runat="server"　ppMaxLength="50" ppIMEMode="全角" ppName="気付" ppNameWidth="120" ppWidth="670" ppValidationGroup="val" />
              
            </td>
                </tr>
                <tr>
            <td colspan="1" class="auto-style28">
               
                <uc:ClsCMTextBox ID="txtZipNo1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="郵便番号" ppExpression="(^\d+-\d+$)|(^\d+$)" ppNameWidth="120" ppWidth="130" ppCheckHan="True" ppValidationGroup="val" />
              
            </td>
                      <td colspan="2">
                
                <uc:ClsCMTextBox ID="txtDetail1" runat="server" ppMaxLength="10" ppName="メモ" ppNameWidth="120" ppWidth="130" ppIMEMode="全角" ppValidationGroup="val" ppWrap="true"/>
              
            </td>
                 </tr>
        <tr>
            <td colspan="5">
            
                <uc:ClsCMTextBox ID="txtAddr1" runat="server"  ppMaxLength="50" ppName="住所" ppNameWidth="120" ppWidth="670" ppValidationGroup="val" ppIMEMode="全角"/>
              
            </td>
            </tr>
        <tr>
            <td colspan="1" class="auto-style28">
                
                <uc:ClsCMTextBox ID="txtTelNo1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="電話番号"  ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" ppNameWidth="120" ppWidth="200" ppCheckHan="True" ppValidationGroup="val" />
               
            </td>
            <td colspan="2">
               
                <uc:ClsCMTextBox ID="txtFaxNo1" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="FAX番号"  ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" ppNameWidth="120" ppWidth="200" ppCheckHan="True" ppValidationGroup="val" />
               
            </td>
       </tr>
       <tr>
            <td colspan="1" class="auto-style28">
               
                <uc:ClsCMTextBox ID="txtInfo1_1" runat="server" ppMaxLength="30" ppName="機器情報１" ppNameWidth="120" ppWidth="200" ppIMEMode="全角" ppValidationGroup="val" />
              </td>
            <td colspan="1">
               
                <uc:ClsCMTextBox ID="txtInfo1_2" runat="server"  ppMaxLength="30" ppName="機器情報２" ppNameWidth="120" ppWidth="200" ppValidationGroup="val" ppIMEMode="全角" />
              
            </td>
           <td class="auto-style23">
                <asp:Button ID="btnClear1" runat="server" Text="クリア" Enabled="true" CausesValidation="False"/>
            </td>
           
       </tr>
       
        </table>
       
    <HR>
   

     <div style="float:left;margin-left:80px;margin-top:4px;text-align:left" >
        <asp:Label ID="Label10" runat="server" Text="故障機器送付先２" Width="500"></asp:Label>

      </div>
    <table style="width:1000px;margin-left:auto;margin-right:auto;border:none;text-align:left">
                <tr>
           
            <td colspan="5">
               
                <uc:ClsCMTextBox ID="txtName2" runat="server" ppMaxLength="50" ppName="気付" ppNameWidth="120" ppWidth="670" ppValidationGroup="val" ppIMEMode="全角"/>
              
            </td>
           </tr>
        <tr>
            <td colspan="1" class="auto-style28">
             
                <uc:ClsCMTextBox ID="txtZipNo2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="郵便番号" ppExpression="(^\d+-\d+$)|(^\d+$)" ppNameWidth="120" ppWidth="130" ppCheckHan="True" ppValidationGroup="val" />
              
            </td>
             <td colspan="2">
               
                <uc:ClsCMTextBox ID="txtDetail2" runat="server" ppIMEMode="全角" ppMaxLength="10" ppName="メモ" ppNameWidth="120" ppWidth="130" ppValidationGroup="val" ppWrap="true" />
            
            </td>
      </tr>
        <tr>
            <td colspan="5">
               
                <uc:ClsCMTextBox ID="txtAddr2" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="住所" ppNameWidth="120" ppWidth="670" ppValidationGroup="val"/>
             
            </td>
            </tr>
               <tr>
             <td colspan="1" class="auto-style28">
                  
                 <uc:ClsCMTextBox ID="txtTelNo2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="電話番号"  ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" ppNameWidth="120" ppWidth="200" ppCheckHan="True" ppValidationGroup="val" />
              
            </td>
            <td colspan="2">
              
                <uc:ClsCMTextBox ID="txtFaxNo2" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="FAX番号"  ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" ppNameWidth="120" ppWidth="200" ppCheckHan="True" ppValidationGroup="val" />
              
            </td>
                </tr>
            <tr>
            <td colspan="1" class="auto-style28">
               
                <uc:ClsCMTextBox ID="txtInfo2_1" runat="server" ppIMEMode="全角"  ppMaxLength="30" ppName="機器情報１" ppNameWidth="120" ppWidth="200" ppValidationGroup="val" />
            
            </td>
            <td colspan="1" class="auto-style29">
               
                <uc:ClsCMTextBox ID="txtInfo2_2" runat="server" ppIMEMode="全角"  ppMaxLength="30" ppName="機器情報２" ppNameWidth="120" ppWidth="200" ppValidationGroup="val" />
              
            </td>
                <td>
                <asp:Button ID="btnClear2" runat="server" Text="クリア" Enabled="true" CausesValidation="False" />
            </td>
           
            </tr>
        
        </table>
    <HR>
         <div style="float:left;margin-left:80px;margin-top:4px;text-align:left" >
        <asp:Label ID="Label8" runat="server" Text="故障機器送付先３" Width="500"></asp:Label>

      </div>
    <table style="width:1000px;margin-left:auto;margin-right:auto;border:none;text-align:left">
        
           <tr>
           
            <td colspan="5">
               
                <uc:ClsCMTextBox ID="txtName3" runat="server" ppIMEMode="全角" ppMaxLength="50" ppName="気付" ppNameWidth="120" ppWidth="670" ppValidationGroup="val" />
               
            </td>
           </tr>
        <tr>
            <td colspan="1" class="auto-style28">
                
                <uc:ClsCMTextBox ID="txtZipNo3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="8" ppName="郵便番号" ppExpression="(^\d+-\d+$)|(^\d+$)" ppNameWidth="120" ppWidth="130" ppCheckHan="True" ppValidationGroup="val" />
               
            </td>
            <td colspan="2">
               
                <uc:ClsCMTextBox ID="txtDetail3" runat="server" ppIMEMode="全角" ppMaxLength="10" ppName="メモ" ppNameWidth="120" ppWidth="130" ppValidationGroup="val" ppWrap="true" />
               
            </td>
      </tr>
        <tr>
            <td colspan="5">
                
                <uc:ClsCMTextBox ID="txtAddr3" runat="server" ppIMEMode="全角"  ppMaxLength="50" ppName="住所" ppNameWidth="120" ppWidth="670" ppValidationGroup="val"/>
              
            </td>
            </tr>
        <tr>
             <td colspan="1" class="auto-style28">
                 
                 <uc:ClsCMTextBox ID="txtTelNo3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="電話番号"  ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" ppNameWidth="120" ppWidth="200" ppCheckHan="True" ppValidationGroup="val" />
               
            </td>
            <td colspan="2">
              
                <uc:ClsCMTextBox ID="txtFaxNo3" runat="server" ppIMEMode="半角_変更不可" ppMaxLength="15" ppName="FAX番号"  ppExpression="(^\d+-\d+-\d+$)|(^\d+$)|(^\d+-\d+$)" ppNameWidth="120" ppWidth="200" ppCheckHan="True" ppValidationGroup="val" />
             
            </td>
                </tr>
            <tr>
            <td colspan="1" class="auto-style28">
               
                <uc:ClsCMTextBox ID="txtInfo3_1" runat="server"  ppIMEMode="全角" ppMaxLength="30" ppName="機器情報１" ppNameWidth="120" ppWidth="200" ppValidationGroup="val" />
               
            </td>
            <td colspan="1" class="auto-style29">
                
                <uc:ClsCMTextBox ID="txtInfo3_2" runat="server"  ppIMEMode="全角" ppMaxLength="30" ppName="機器情報２" ppNameWidth="120" ppWidth="200" ppValidationGroup="val" />
              
            </td>
                <td>
                <asp:Button ID="btnClear3" runat="server" Text="クリア" Enabled="true" CausesValidation="False" />
            </td>
          
            </tr>
        
        </table>
    <HR style="margin-bottom:24px">
    <table style="width:1000px;margin-left:auto;margin-right:auto;border:none;text-align:left">
         <tr>
             <td>
                <uc:ClsCMTextBox  Visible="false" ppNameVisible="false" ID="txtRemark" runat="server"  ppIMEMode="全角" ppMaxLength="100" ppName="備考" ppNameWidth="120" ppWidth="670" ppTextMode="MultiLine" ppValidationGroup="val"  ppWrap="true"/>
             </td>
         </tr>
                    </table>

</asp:Content>



<asp:Content runat="server" ID="GridContent" ContentPlaceHolderID="GridContent">   
    
    <div id="DivOut" runat="server" class="grid-out" style="width:1200px;height:206px;">
        <div class="grid-in" style="width:1200px;height:206px;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server"></asp:GridView>
        </div>
    </div>
</asp:Content>
