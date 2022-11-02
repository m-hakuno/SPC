<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="HEAUPDP001.aspx.vb" Inherits="SPC.HEAUPDP001" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="server">
    <table style="border:0px solid #000000;width:99%;">
        <tr>
            <td style="text-align:right;">
                <label>該当件数：</label><span style="padding-left:60px;text-align:center;"><asp:Label runat="server" ID="lblKensu" /></span><label>件</label>
            </td>
        </tr>
    </table>
    <table style="border-collapse:collapse;width:99%;font-size:14px;background-color:lightblue;">
        <tr>
            <td style="border: 1px solid #000000;padding:20px 20px 20px 20px;">
                <table style="border-collapse:collapse;width:90%;">
                    <tr style="height:30px;">
                        <td style="width:20%;border: 1px solid #000000;background-color:lightgray;">
                            <label>リアル通知コード</label>
                        </td>
                        <td style="width:30%;border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblRealTutiCd" />
                        </td>
                        <td colspan="2" style="border-bottom-style:none;border-top-style:none;border-right-style:none;">
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="width:20%;border: 1px solid #000000;background-color:lightgray;">
                            <label>ＴＢＯＸＩＤ</label>
                        </td>
                        <td style="width:30%;border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblTBoxId" />
                        </td>
                        <td style="width:15%;border: 1px solid #000000;background-color:lightgray;">
                            <label>状態１</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblJotai1" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="width:20%;border: 1px solid #000000;background-color:lightgray;">
                            <label>ホール名</label>
                        </td>
                        <td style="width:30%;border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblHallNm" />
                        </td>
                        <td style="width:15%;border: 1px solid #000000;background-color:lightgray;">
                            <label>保担</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblHotan" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border: 1px solid #000000;border-left-style:hidden;vertical-align:top;padding-top:20px;padding-right:20px;text-align:right;">
                <asp:Button runat="server" ID="btnTboxRealShokai" text="ＴＢＯＸリアル照会" />
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #000000;padding:20px 20px 20px 20px;border-top-style:hidden;">
                <table style="border-collapse:collapse;width:90%;">
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>データ受信日</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblDataJusinDate" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>送信連番</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblSosinRenban" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>更新形式</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblKosinKeisiki" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>データ受信時刻</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblDataJusinTime" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>運用日付</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblUnyoDate" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>補正区分</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblHoseiKbn" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>受信連番</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblJusinRenban" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>発生日付</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblHasseiDate" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>補正状況</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblHoseiJokyo" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>元の受信連番</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblMotoJusinRenban" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>発生時刻</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblHasseiTime" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>判定</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblHantei" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>ＴＢＯＸ種別</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblTBoxSbt" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>ＴＢＯＸモード</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblTBoxMode" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>補正原因</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblHoseiGenin" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>ＴＢＯＸバージョン</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblTBoxVer" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>ＴＢＯＸ状態</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblTboxJotai" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>最新更新日</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblLatestUpdDate" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>ＴＢＯＸ運用日付</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblTBoxUnyoDate" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>ＨＣ検知コード</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblHcKenchiCd" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>最新更新時刻</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblLatestUpdTime" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:140px;background-color:lightgray;">
                            <label>データ種別</label>
                        </td>
                        <td style="border: 1px solid #000000;width:70px;background-color:white;">
                            <asp:Label runat="server" ID="lblDataSbt" />
                        </td>
                        <td style="border: 1px solid #000000;width:115px;background-color:lightgray;">
                            <label>詳細内容</label>
                        </td>
                        <td colspan="3" style="border: 1px solid #000000;background-color:white;">
                            <asp:Label runat="server" ID="lblSyosaiNaiyo" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border: 1px solid #000000;border-left-style:hidden;border-top-style:hidden;vertical-align:top;padding-top:20px;padding-right:20px;">
            </td>
        </tr>
    </table>
    <div style="width:100%;height:20px;"></div>
    <table style="border-collapse:collapse;width:99%;font-size:14px;background-color:mistyrose;">
        <tr>
            <td style="border: 1px solid #000000;padding:20px 20px 20px 20px;">
                <table style="width:90%;">
                    <tr style="height:30px;">
                        <td style="background-color:lightgray;border: 1px solid #000000;width:140px;">
                            <label>調査者</label>
                        </td>
                        <td style="background-color:white;border: 1px solid #000000;width:250px;padding-left:4px;">
                            <asp:DropDownList runat="server" ID="ddlChousasya" width="98%" TabIndex="1" />
                        </td>
                        <td style="border: 1px solid #000000;border-top-style:hidden;border-right-style:hidden;border-bottom-style:hidden;">
                        </td>
                    </tr>
                    <tr style="height:20px;">
                        <td colspan="2" style="border-left-style:hidden">
                        </td>
                        <td style="border-left-style:hidden;border-top-style:hidden;border-right-style:hidden;">
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="background-color:lightgray;border: 1px solid #000000;width:140px;">
                            <label>調査結果</label>
                        </td>
                        <td colspan="2" style="background-color:white;border: 1px solid #000000;">
                            <uc:ClsCMTextBox runat="server" ID="txtChosaKekka" ppNameVisible="false" ppWidth="913px" ppTabIndex="2" ppMaxLength="200" ppTextMode="MultiLine" ppValidationGroup="Detail" ppName="調査結果" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div class="float-left">
                    <asp:ValidationSummary ID="vasSummarySearch" runat="server" CssClass="errortext" ValidationGroup="Detail" TabIndex="8" Height="30px" />
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;height:40px;"></div>
    <label style="font-size:20px;font-weight:700;">調査履歴情報</label>
    <div class="grid-out">
        <div class="grid-in" style="width:100%;">
            <input id="hdnData" type="hidden" runat="server" class="grid-data" />
            <asp:GridView ID="grvList" runat="server" TabIndex="26" >
            </asp:GridView>
        </div>
    </div>
    <div style="width:100%;height:20px;"></div>
    <table style="border-collapse:collapse;width:99%;font-size:14px;background-color:bisque;">
        <tr>
            <td style="border: 1px solid #000000;padding:20px 20px 20px 20px;">
                <table style="width:70%;">
                    <tr>
                        <td colspan="6" style="padding-bottom:20px;">
                            <label style="font-size:16px;font-weight:700;">ＴＢＯＸ異常／センター異常</label>
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <th colspan="6" style="border: 1px solid #000000;background-color:steelblue;">
                            ＴＢＯＸ異常発生フラグ
                        </th>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＡＰ－ＨＡＬＴ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblAPHalt" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＬＡＮカード異常</label><br /><label style="color:red;">LANｶｰﾄﾞ異常検知(店内装置側)</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblLanCardErr" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>保守１操作有無</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHosyu1" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>温度異常１</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblOndoErr1" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ故障フラグ(＃１)</label><br /><label style="color:red;">ＳＳＤ１故障フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddErrFlg1" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>保守２操作有無</label><br /><label style="color:red;">ＯＳライセンス認証実施有無</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHosyu2" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＵＰＳバッテリー交換要求</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblUpsChangeReq" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ故障フラグ(＃２)</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddErrFlg2" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>前面鍵扉開放有無</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblZenmenKagiKaiho" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>電源ユニット交換</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblDengenChange" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ故障フラグ(＃３)</label><br /><label style="color:red;">ＳＳＤ２故障フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddErrFlg3" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>後面鍵扉開放有無</label><br /><label style="color:red;">Ｃｆａｓｔ抜け検知</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblKoumenKagiKaiho" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>電源ファンアラーム</label><br /><label style="color:red;">筐体ファン２アラーム</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblDengenFanAlerm" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ故障フラグ(＃４)</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddErrFlg4" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>与信限度額調査有無</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblYosinGendogaku" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＣＰＵファンアラーム</label><br /><label style="color:red;">筐体ファン１アラーム</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblCpuFanAlerm" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ抜けフラグ(＃１)</label><br /><label style="color:red;">ＳＳＤ１抜けフラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddNukeFlg1" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>違差額調査有無</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblIsagakuChosa" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>リチウムＬｏｗ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblLitiumLow" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ抜けフラグ(＃２)</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddNukeFlg2" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>精算額超過有無</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblSeisangakuChoka" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ニッカドＬｏｗ</label><br /><label style="color:red;">LANｶｰﾄﾞ異常検知(ﾌﾟﾘﾝﾀ･UPS側)</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblNikkadoLow" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ抜けフラグ(＃３)</label><br /><label style="color:red;">ＳＳＤ２抜けフラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddNukeFlg3" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>大規模店構成通信異常</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblDaikibotenKoseiTusin" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>キーボード抜け</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblKeyBordNuke" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＨＤＤ抜けフラグ(＃４)</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblHddNukeFlg4" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＴＢＯＸ開始忘れ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblTboxKaisiWasure" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＢＢＭ故障</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblBbmErr" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＴＢＯＸ改竄検出</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblTboxKaizanFound" />
                        </td>
                        <td style="border: 1px solid #000000;background-color:lightgray;width:100px;">
                            <label>ＭＣ異常</label><br /><label style="color:red;">Ｃｆａｓｔ異常</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblMcErr" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td colspan="3" style="border: 1px solid #000000;background-color:lightgray;width:215px;">
                            <label>ＣＩＤ＿ＤＢニアエンド検出フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblCidDbFlg" />
                        </td>
                        <td style="border-bottom-style:hidden;border-right-style:hidden;border: 0px solid #000000;">
                        </td>
                        <td style="border-bottom-style:hidden;border-right-style:hidden;border: 0px solid #000000;">
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td colspan="3" style="border: 1px solid #000000;background-color:lightgray;width:215px;">
                            <label>入金伝票ＤＢニアエンド＿全体</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblNkDnpDbAllFlg" />
                        </td>
                        <td style="border-bottom-style:hidden;border-right-style:hidden;border: 0px solid #000000;">
                        </td>
                        <td style="border-bottom-style:hidden;border-right-style:hidden;border: 0px solid #000000;">
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td colspan="3" style="border: 1px solid #000000;background-color:lightgray;width:215px;">
                            <label>入金伝票ＤＢニアエンド＿オフライン領域</label>
                        </td>
                        <td style="border: 1px solid #000000;background-color:white;width:15px;text-align:center;">
                            <asp:Label runat="server" ID="lblNkDnpDbOffFlg" />
                        </td>
                        <td style="border-bottom-style:hidden;border-right-style:hidden;border: 0px solid #000000;">
                        </td>
                        <td style="border-bottom-style:hidden;border-right-style:hidden;border: 0px solid #000000;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="width:100%;height:20px;"></div>
    <table style="border-collapse:collapse;width:99%;font-size:14px;background-color:thistle;">
        <tr>
            <td colspan="2" style="padding:20px 20px 0px 20px;border: 1px solid #000000;border-bottom-style:hidden;">
                <label style="font-size:16px;font-weight:700;">ＴＢＯＸ状態／ＢＢ異常</label>
            </td>
        </tr>
        <tr>
            <td style="border: 1px solid #000000;padding:20px 10px 20px 20px;width:35%;">
                <table style="width:100%;">
                    <tr style="height:30px;">
                        <th colspan="2" style="border: 1px solid #000000;background-color:steelblue;">
                            ＴＢＯＸ運用状態フラグ
                        </th>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>正常運転中／運転停止中</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblSeijoUntenStrtStp" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＡＣ電源有無</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblAcDengen" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>店内装置構成表変更フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblTennaiSotiKoseiFlg" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>拡張ＴＢＯＸシリアルＮＯ変更フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblKakuchoTboxSerialNoFlg" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>未発券消費発生フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblMihakkenShohiFlg" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＴＢＯＸソフトバージョン変更フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblTBoxSoftVerFlg" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>未受付機使用発生フラグ</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblMiuketukekiSiyoFlg" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="border: 1px solid #000000;border-left-style:hidden;vertical-align:top;padding:20px 20px 20px 10px;width:65%;">
                <table style="width:100%;">
                    <tr style="height:30px;">
                        <th colspan="4" style="border: 1px solid #000000;background-color:steelblue;">
                            ＢＢ運用状態フラグ
                        </th>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>工場出荷取込</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblKojoSyukka" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＪＢ変更有無</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblJBHenko" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>他店装置取込</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblTatenSoti" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＢＢ２シリアルＮＯ変更有無</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblBB2SerialNoHenko" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>チェーン店装置取込</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblChainTenSoti" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>仮締め範囲外</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblKarijimeHaniGai" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>代理店装置取込</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblDairitenSoti" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>他店受付ログ取込無</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblTatenUketukeLog" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＢＢシリアル管理外取込</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblBBSerialKanrigai" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>オフラインサンド検出有無</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblOfflineSandKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>鍵管理外取込</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblKagiKanrigai" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:white;">
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width:100%;border: 1px solid #000000;padding:0px 20px 20px 20px;border-top-style:hidden;">
                <table>
                    <tr style="height:30px;">
                        <th colspan="6" style="border: 1px solid #000000;background-color:steelblue;width:1150px;">
                            ＢＢ異常発生フラグ
                        </th>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>認証エラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblNinsyoErrKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>代理店装置検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblDairitenSotiKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>電文長エラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblDenbunErrKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>複合エラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblFukugoErrKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＢＢシリアル管理外検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblBBSeriaruKanrigaiKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>レスポンスコードエラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblResponseCdErrKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＢＬ対象エラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblBLtaisyoErrKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>構成表登録外ＪＢ番号検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblKoseihyoJBNoKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>鍵管理外検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblKagikanrigaiKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＢＢ重複接続エラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblBBChofukuKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>接続不可機種種別検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblSetuzokufukaSyubetu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>シーケンス異常検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblSequenceErrKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＩＰアドレスエラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblIpErrKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>接続不可機種コード検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblSetuzokufukaCd" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>ＢＢシリアルＮＯエラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblBBSerialNoErrKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>他店装置検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblTatenSotiKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>バージョンエラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblVersionErrKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>カード会社区分エラー検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblCardkaisyaErrKensyutu" />
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>チェーン店装置検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblChainSotiKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>他カード会社装置検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblTaCardkaisyaSotiKensyutu" />
                        </td>
                        <td style="border: 1px solid #000000;width:100px;background-color:lightgray;">
                            <label>電文シーケンス番号異常検出</label>
                        </td>
                        <td style="border: 1px solid #000000;width:10px;background-color:white;text-align:center;">
                            <asp:Label runat="server" ID="lblDenbunSequenceErrKensyutu" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table> 
    <table style="border-collapse:collapse;width:99%;border:0px solid #000000;">
        <tr>
            <td style="text-align:right;padding-top:20px;">
                <asp:Button runat="server" ID="btnUpdate" Text="調査登録" ValidationGroup="Detail" />
            </td>
        </tr>
    </table>
</asp:Content>
