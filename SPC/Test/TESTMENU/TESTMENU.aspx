<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TESTMENU.aspx.vb" Inherits="SPC.TESTMENU" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
    <script type="text/javascript" src='<%= Me.ResolveClientUrl("~/Scripts/ctiexeNew.js") %>'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <asp:Panel ID="Panel2" runat="server">        
        <table style="text-align:right;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="キー項目"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtKey" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="遷移条件"></asp:Label>
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList ID="ddlMode" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="1">参照</asp:ListItem>
                        <asp:ListItem Value="2">更新</asp:ListItem>
                        <asp:ListItem Value="3">登録</asp:ListItem>
                        <asp:ListItem Value="4">仮登録</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="ユーザ権限"></asp:Label>
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList ID="ddlUser" runat="server" Enabled="False">
                        <asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="遷移元ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtOldDisp" runat="server"></asp:TextBox>                
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="パンくず"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblBcList" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">有</asp:ListItem>
                        <asp:ListItem Value="2">無</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="レポートクラス"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRptCls" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="データ"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtData" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="レポート名"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRptNm" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="NGCメニュー"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNgcMenu" runat="server"></asp:TextBox>                
                </td>               
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblIp" runat="server" Text="端末情報"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIp" runat="server" Enabled="True" MaxLength="15"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblPlace" runat="server" Text="場所"></asp:Label>
                </td>
                <td style="text-align:left;">
                    <asp:DropDownList ID="ddlPlace" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="1">SPC</asp:ListItem>
                        <asp:ListItem Value="2">NGC</asp:ListItem>
                        <asp:ListItem Value="3">WKB</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblLoginDt" runat="server" Text="ログイン年月日"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtLoginDt" runat="server" Enabled="True" MaxLength="19"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblSessionID" runat="server" Text="セッションＩＤ"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSessionID" runat="server" Enabled="true" MaxLength="16"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblGroup" runat="server" Text="グループ番号"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGroup" runat="server" Enabled="True" MaxLength="16"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>    
     
    <asp:Panel ID="Panel1" runat="server" Height="640px" ScrollBars="Auto">
    <table style="width:95%;" class="center">
        <tr style="font-size:large">
            <td>
                <asp:Label ID="Label21" runat="server" Text="共通"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label22" runat="server" Text="工事業務"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label23" runat="server" Text="監視業務"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label24" runat="server" Text="保守業務"></asp:Label>
            </td>
        </tr>
        <tr style="vertical-align:top; line-height:23px;">
            <td style="padding-left:15px;">
                <asp:LinkButton ID="COMMENP001" runat="server" CommandName="~/Common/COMMENP001/COMMENP001.aspx">メインメニュー</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP002" runat="server" CommandName="~/Common/COMMENP002/COMMENP002.aspx">工事管理</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP003" runat="server" CommandName="~/Common/COMMENP003/COMMENP003.aspx">保守管理</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP004" runat="server" CommandName="~/Common/COMMENP004/COMMENP004.aspx">監視業務</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP005" runat="server" CommandName="~/Common/COMMENP005/COMMENP005.aspx">進捗管理</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP006" runat="server" CommandName="~/Common/COMMENP006/COMMENP006.aspx">ホールマスタ管理</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP007" runat="server" CommandName="~/Common/COMMENP007/COMMENP007.aspx">ヘルスチェック</asp:LinkButton><br>
                <asp:LinkButton ID="COMMENP008" runat="server" CommandName="~/Common/COMMENP008/COMMENP008.aspx">検収／請求</asp:LinkButton><br>
                <asp:LinkButton ID="COMSELP001" runat="server" CommandName="~/Common/COMSELP001/COMSELP001.aspx">ホール参照</asp:LinkButton><br>
                <asp:LinkButton ID="COMSELP004" runat="server" CommandName="~/Common/COMSELP004/COMSELP004.aspx">機器参照</asp:LinkButton><br>
                <asp:LinkButton ID="COMSELP002" runat="server" CommandName="~/Common/COMSELP002/COMSELP002.aspx">業者情報</asp:LinkButton><br>
                <asp:LinkButton ID="DLCINPP001" runat="server" CommandName="~/Common/DLCINPP001/DLCINPP001.aspx">アップロード</asp:LinkButton><br>
                <asp:LinkButton ID="COMLSTP099" runat="server" CommandName="~/Common/COMLSTP099/COMLSTP099.aspx">ダウンロード</asp:LinkButton><br>          
            </td>
            <td style="padding-left:15px;">                
                 <asp:LinkButton ID="CNSLSTP001" runat="server" CommandName="~/Construction/CNSLSTP001/CNSLSTP001.aspx">工事一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSUPDP001" runat="server" CommandName="~/Construction/CNSUPDP001/CNSUPDP001.aspx">工事依頼書兼仕様書</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSLSTP002" runat="server" CommandName="~/Construction/CNSLSTP002/CNSLSTP002.aspx">物品転送依頼一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSUPDP002" runat="server" CommandName="~/Construction/CNSUPDP002/CNSUPDP002.aspx">物品転送依頼書 参照</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSLSTP004" runat="server" CommandName="~/Construction/CNSLSTP004/CNSLSTP004.aspx">進捗工事一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSUPDP003" runat="server" CommandName="~/Construction/CNSUPDP003/CNSUPDP003.aspx">工事進捗　参照／更新</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSLSTP005" runat="server" CommandName="~/Construction/CNSLSTP005/CNSLSTP005.aspx">工事連絡票一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSUPDP004" runat="server" CommandName="~/Construction/CNSUPDP004/CNSUPDP004.aspx">工事連絡票　参照／更新</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSLSTP006" runat="server" CommandName="~/Construction/CNSLSTP006/CNSLSTP006.aspx">工事料金明細一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSINQP001" runat="server" CommandName="~/Construction/CNSINQP001/CNSINQP001.aspx">工事料金明細書（確認）</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSUPDP005" runat="server" CommandName="~/Construction/CNSUPDP005/CNSUPDP005.aspx">請求資料　状況更新</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSLSTP007" runat="server" CommandName="~/Construction/CNSLSTP007/CNSLSTP007.aspx">作業予定一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CNSUPDP006" runat="server" CommandName="~/Construction/CNSUPDP006/CNSUPDP006.aspx">工事完了報告書</asp:LinkButton><br>
                 <asp:LinkButton ID="DLLSELP001" runat="server" CommandName="~/Construction/DLLSELP001/DLLSELP001.aspx">構成配信／結果参照</asp:LinkButton><br>
                 <asp:LinkButton ID="MSTLSTP001" runat="server" CommandName="~/Construction/MSTLSTP001/MSTLSTP001.aspx">即時集信一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="SERLSTP001" runat="server" CommandName="~/Construction/SERLSTP001/SERLSTP001.aspx">シリアル情報一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="SERUPDP001" runat="server" CommandName="~/Construction/SERUPDP001/SERUPDP001.aspx">シリアル登録</asp:LinkButton><br>                      
            </td>
            <td style="padding-left:15px;">                
                 <asp:LinkButton ID="WATLSTP001" runat="server" CommandName="~/Watch/WATLSTP001/WATLSTP001.aspx">監視報告書兼依頼票一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="WATUPDP001" runat="server" CommandName="~/Watch/WATUPDP001/WATUPDP001.aspx">監視報告書兼依頼票参照／更新</asp:LinkButton><br>
                 <asp:LinkButton ID="WATLSTP002" runat="server" CommandName="~/Watch/WATLSTP002/WATLSTP002.aspx">監視対象外ホール一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="WATINQP001" runat="server" CommandName="~/Watch/WATINQP001/WATINQP001.aspx">ＴＢＯＸ随時照会</asp:LinkButton><br>
                 <asp:LinkButton ID="WATOUTP002" runat="server" CommandName="~/Watch/WATOUTP002/WATOUTP002.aspx">ＴＢＯＸ結果表示　決済照会情報</asp:LinkButton><br>
                 <asp:LinkButton ID="WATOUTP026" runat="server" CommandName="~/Watch/WATOUTP026/WATOUTP026.aspx">ＴＢＯＸ結果表示　店内通信</asp:LinkButton><br>
                 <asp:LinkButton ID="HEALSTP001" runat="server" CommandName="~/Watch/HEALSTP001/HEALSTP001.aspx">ヘルスチェック一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="HEAUPDP001" runat="server" CommandName="~/Watch/HEAUPDP001/HEAUPDP001.aspx">ヘルスチェック詳細／更新</asp:LinkButton><br>  
                 <asp:LinkButton ID="OVELSTP001" runat="server" CommandName="~/Watch/OVELSTP001/OVELSTP001.aspx">時間外消費ホール一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="OVELSTP002" runat="server" CommandName="~/Watch/OVELSTP002/OVELSTP002.aspx">時間外消費ホール詳細</asp:LinkButton><br>
                 <asp:LinkButton ID="SLFLSTP001" runat="server" CommandName="~/Watch/SLFLSTP001/SLFLSTP001.aspx">券売入金機自走調査一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="SLFLSTP002" runat="server" CommandName="~/Watch/SLFLSTP002/SLFLSTP002.aspx">券売入金機自走ホール一覧</asp:LinkButton><br>   
                 <asp:LinkButton ID="ERRLSTP001" runat="server" CommandName="~/Watch/ERRLSTP001/ERRLSTP001.aspx">集信エラー調査一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="ERRSELP001" runat="server" CommandName="~/Watch/ERRSELP001/ERRSELP001.aspx">集信エラーホール一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="ICHLSTP001" runat="server" CommandName="~/Watch/ICHLSTP001/ICHLSTP001.aspx">ＩＣカード履歴調査一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="ICHLSTP002" runat="server" CommandName="~/Watch/ICHLSTP002/ICHLSTP002.aspx">ＩＣカード履歴調査一覧(詳細)</asp:LinkButton><br>
                 <asp:LinkButton ID="DLCLSTP001" runat="server" CommandName="~/Watch/DLCLSTP001/DLCLSTP001.aspx">ＤＬＬ設定変更依頼一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="DSULSTP001" runat="server" CommandName="~/Watch/DSULSTP001/DSULSTP001.aspx">ＤＳＵ交換対応依頼書一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="DSUUPDP001" runat="server" CommandName="~/Watch/DSUUPDP001/DSUUPDP001.aspx">ＤＳＵ交換対応依頼書　参照／更新</asp:LinkButton><br>        
            </td>
            <td style="padding-left:15px;" rowspan="2">                
                 <asp:LinkButton ID="REQINQP001" runat="server" CommandName="~/Maintenance/REQINQP001/REQINQP001.aspx">保守対応依頼書照会</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPINQP001" runat="server" CommandName="~/Maintenance/CMPINQP001/CMPINQP001.aspx">特別保守費用照会</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPOUTP001" runat="server" CommandName="~/Maintenance/CMPOUTP001/CMPOUTP001.aspx">特別保守費用照会印刷画面</asp:LinkButton><br>
                 <asp:LinkButton ID="CTISELP005" runat="server" CommandName="~/Maintenance/CTISELP005/CTISELP005.aspx">ＣＴＩ情報（作業者）</asp:LinkButton><br>
                 <asp:LinkButton ID="REQLSTP001" runat="server" CommandName="~/Maintenance/REQLSTP001/REQLSTP001.aspx">トラブル処理票一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="REQSELP001" runat="server" CommandName="~/Maintenance/REQSELP001/REQSELP001.aspx">トラブル処理票</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPLSTP001" runat="server" CommandName="~/Maintenance/CMPLSTP001/CMPLSTP001.aspx">保守対応依頼書一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPSELP001" runat="server" CommandName="~/Maintenance/CMPSELP001/CMPSELP001.aspx">保守対応依頼書</asp:LinkButton><br>     
                 <asp:LinkButton ID="REQLSTP002" runat="server" CommandName="~/Maintenance/REQLSTP002/REQLSTP002.aspx">持参物品一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="EQULSTP001" runat="server" CommandName="~/Maintenance/EQULSTP001/EQULSTP001.aspx">配送機器一覧表</asp:LinkButton><br>
                 <asp:LinkButton ID="EQULSTP003" runat="server" CommandName="~/Maintenance/EQULSTP003/EQULSTP003.aspx">保守予備機棚卸表</asp:LinkButton><br>
                 <asp:LinkButton ID="BPIINQP001" runat="server" CommandName="~/Maintenance/BPIINQP001/BPIINQP001.aspx">貸玉数　設定情報</asp:LinkButton><br>
                 <asp:LinkButton ID="BPIFIXP001" runat="server" CommandName="~/Maintenance/BPIFIXP001/BPIFIXP001.aspx">貸玉数　設定情報差異</asp:LinkButton><br>
                 <asp:LinkButton ID="BPILSTP001" runat="server" CommandName="~/Maintenance/BPILSTP001/BPILSTP001.aspx">玉単価設定情報一覧</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPUPDP001" runat="server" CommandName="~/Maintenance/CMPUPDP001/CMPUPDP001.aspx">保守料金明細作成</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPUPDP002" runat="server" CommandName="~/Maintenance/CMPUPDP002/CMPUPDP002.aspx">修理・有償部品費用作成</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPINQP002" runat="server" CommandName="~/Maintenance/CMPINQP002/CMPINQP002.aspx">情報機器保守検収書</asp:LinkButton><br>
                 <asp:LinkButton ID="CMPLSTP002" runat="server" CommandName="~/Maintenance/CMPLSTP002/CMPLSTP002.aspx">検収書確認リスト</asp:LinkButton><br>
                 <asp:LinkButton ID="QUAOUTP001" runat="server" CommandName="~/Maintenance/QUAOUTP001/QUAOUTP001.aspx">品質会議資料</asp:LinkButton><br>
                 <asp:LinkButton ID="QUAUPDP001" runat="server" CommandName="~/Maintenance/QUAUPDP001/QUAUPDP001.aspx">品質会議資料明細</asp:LinkButton><br>
            </td>
        </tr>
        <tr style="font-size:large">
            <td>
                <asp:Label ID="Label25" runat="server" Text="故障受付業務"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label26" runat="server" Text="修理・整備業務"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label27" runat="server" Text="サポートセンタ業務"></asp:Label>
            </td>
        </tr>
        <tr style="vertical-align:top; line-height:23px;">
            <td style="padding-left:15px;">
                <asp:LinkButton ID="BRKLSTP001" runat="server" CommandName="~/FaultReception/BRKLSTP001/BRKLSTP001.aspx">ミニ処理票一覧</asp:LinkButton><br> 
                <asp:LinkButton ID="BRKUPDP001" runat="server" CommandName="~/FaultReception/BRKUPDP001/BRKUPDP001.aspx">ミニ処理票</asp:LinkButton><br> 
                <asp:LinkButton ID="BRKINQP001" runat="server" CommandName="~/FaultReception/BRKINQP001/BRKINQP001.aspx">対応履歴照会</asp:LinkButton>
            </td>
            <td style="padding-left:15px;">
                <asp:LinkButton ID="REPLSTP001" runat="server" CommandName="~/RepairEequipment/REPLSTP001/REPLSTP001.aspx">修理進捗一覧</asp:LinkButton><br> 
                <asp:LinkButton ID="REPUPDP001" runat="server" CommandName="~/RepairEequipment/REPUPDP001/REPUPDP001.aspx">修理・有償部品費用</asp:LinkButton><br> 
                <asp:LinkButton ID="REPUPDP002" runat="server" CommandName="~/RepairEequipment/REPUPDP002/REPUPDP002.aspx">修理依頼書</asp:LinkButton><br> 
                <asp:LinkButton ID="MNTSELP001" runat="server" CommandName="~/RepairEequipment/MNTSELP001/MNTSELP001.aspx">整備進捗明細</asp:LinkButton><br> 
                <asp:LinkButton ID="MNTUPDP001" runat="server" CommandName="~/RepairEequipment/MNTUPDP001/MNTUPDP001.aspx">整備依頼書</asp:LinkButton><br> 
                <asp:LinkButton ID="REPOUTP001" runat="server" CommandName="~/RepairEequipment/MNTOUTP001/MNTOUTP001.aspx">整備検収書</asp:LinkButton>
            </td>
            <td style="padding-left:15px;">
                <asp:LinkButton ID="BBPLSTP001" runat="server" CommandName="~/SupportCenter/BBPLSTP001/BBPLSTP001.aspx">ＢＢ１調査依頼一覧</asp:LinkButton><br> 
                <asp:LinkButton ID="BBPUPDP001" runat="server" CommandName="~/SupportCenter/BBPUPDP001/BBPUPDP001.aspx">ブラックボックス調査報告書</asp:LinkButton><br> 
                <asp:LinkButton ID="CDPLSTP001" runat="server" CommandName="~/SupportCenter/CDPLSTP001/CDPLSTP001.aspx">使用中カードＤＢ吸上一覧 </asp:LinkButton><br> 
                <asp:LinkButton ID="TBPINPP001" runat="server" CommandName="~/SupportCenter/TBPINPP001/TBPINPP001.aspx">随時集信一覧状況入力</asp:LinkButton><br> 
                <asp:LinkButton ID="DOCMENP001" runat="server" CommandName="~/SupportCenter/DOCMENP001/DOCMENP001.aspx">サポートセンタ検収書</asp:LinkButton><br> 
                <asp:LinkButton ID="DOCUPDP001" runat="server" CommandName="~/SupportCenter/DOCUPDP001/DOCUPDP001.aspx">請求書作成</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnNotePad" runat="server" Text="sample.txtの起動" Width="150px" CausesValidation="false" />
            </td>
        </tr>
    </table>    
    </asp:Panel>
</asp:Content>
