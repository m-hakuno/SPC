'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜監視＞
'*　処理名　　：　ＴＢＯＸ結果表示
'*　ＰＧＭＩＤ：　WATOUTP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2017/08/21　：　加賀　　　
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'WATOUTP001-000     2017/XX/XX      XXXX　　　XXXX

Imports System.Data.SqlClient
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMCommon

Public Class WATOUTP001

#Region "継承定義"

    Inherits System.Web.UI.Page

#End Region

#Region "定数定義"

    'プログラムＩＤ
    Const M_DISP_ID = P_FUN_WAT & P_SCR_OUT & P_PAGE & "001"

#End Region

#Region "変数定義"

    Dim objStack As StackFrame
    Dim clsCMDBC As New ClsCMDBCom

#End Region


#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objStack = New StackFrame

        Try
            'イベント設定
            AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrint_Click         '印刷

            '初回表示
            If Not IsPostBack Then

                'Dim aryParam As NameValueCollection = Request.QueryString
                'Dim strP_KEY() As String

                'セッションから情報を取得
                ViewState(P_KEY) = Session(P_KEY)   '0：照会管理番号  1：連番  2：ＮＬ区分  3：ＩＤＩＣ区分  4：運用日付  5:要求通番  6：枝番  7:データ種別コード  8:データ種別名称  9:要求日時  10:TBOXID 11:ホール名
                'strP_KEY = Session(P_KEY)

                With Master.Master
                    '画面ヘッダ設定
                    .ppProgramID = M_DISP_ID
                    .ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)
                    .ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)    'パンくずリスト設定
                    .ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる
                    Master.ppCount_Visible = False

                    '印刷ボタン設定
                    .ppRigthButton1.Visible = True
                    .ppRigthButton1.Text = "印刷"
                    .ppRigthButton1.Enabled = True
                    .ppRigthButton1.CausesValidation = True
                    .ppRigthButton1.OnClientClick = pfGet_OCClickMes("10002", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "帳票")
                End With

                Master.ppRigthButton1.Visible = False
                Master.ppRigthButton2.Visible = False

                '「検索中」メッセージ表示javascript付与
                Master.ppRigthButton1.OnClientClick = "showMsgDiv()"

                'データ取得・表示
                setInfo(0)

            End If

        Catch ex As Exception
            'エラーメッセージ表示
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後) '画面の初期化に失敗しました。
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 印刷ボタン押下時
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not (Page.IsValid) Then
            Exit Sub
        End If

        Dim dstOrders As New DataSet
        Dim rpt As Object = Nothing
        Dim strReportName As String = lblDataClsNm.Text
        Dim strKeyList() As String = ViewState(P_KEY)

        objStack = New StackFrame

        Try

            'SQLコマンド設定
            Using cmdSQL As SqlCommand = New SqlCommand

                'パラメータ設定
                With cmdSQL.Parameters
                    .Add(pfSet_Param("ctrl_no", SqlDbType.NVarChar, strKeyList(0)))
                    .Add(pfSet_Param("seq", SqlDbType.NVarChar, strKeyList(1)))
                    .Add(pfSet_Param("nl_cls", SqlDbType.NVarChar, strKeyList(2)))
                    .Add(pfSet_Param("idic_cls", SqlDbType.NVarChar, strKeyList(3)))
                    .Add(pfSet_Param("unyo_date", SqlDbType.NVarChar, strKeyList(4)))
                    .Add(pfSet_Param("yokyu_seq", SqlDbType.NVarChar, strKeyList(5)))
                    .Add(pfSet_Param("eda_seq", SqlDbType.NVarChar, strKeyList(6)))
                End With


                Select Case strKeyList(7)
                    Case "29"   'BB機番情報

                        rpt = New TBRREP004
                        cmdSQL.CommandText = "TBRREP004_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("jbno_from", SqlDbType.Char, tftJBNo.ppFromText))
                            .Add(pfSet_Param("jbno_to", SqlDbType.Char, tftJBNo.ppToText))
                        End With

                    Case "86"   'BB機番情報2

                        rpt = New TBRREP004
                        cmdSQL.CommandText = "TBRREP004_S2"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("jbno_from", SqlDbType.Char, tftJBNo.ppFromText))
                            .Add(pfSet_Param("jbno_to", SqlDbType.Char, tftJBNo.ppToText))
                        End With

                    Case "30"   'BB装置

                        rpt = New TBRREP005
                        cmdSQL.CommandText = "TBRREP005_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("time_from", SqlDbType.Char, tmftTimeSpan.ppHourTextFrom & tmftTimeSpan.ppMinTextFrom & "00"))
                            .Add(pfSet_Param("time_to", SqlDbType.Char, tmftTimeSpan.ppHourTextTo & tmftTimeSpan.ppMinTextTo & "00"))
                        End With

                    Case "35"   'TB操作ログ

                        rpt = New TBRREP006
                        cmdSQL.CommandText = "TBRREP006_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("time_from", SqlDbType.Char, tmftTimeSpan.ppHourTextFrom & tmftTimeSpan.ppMinTextFrom & "00"))
                            .Add(pfSet_Param("time_to", SqlDbType.Char, tmftTimeSpan.ppHourTextTo & tmftTimeSpan.ppMinTextTo & "00"))
                            .Add(pfSet_Param("select_flg", SqlDbType.Char, rblSlctPrnt.SelectedItem.Value))
                        End With

                    Case "36"   'TBエラーログ

                        rpt = New TBRREP007
                        cmdSQL.CommandText = "TBRREP007_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("time_from", SqlDbType.Char, tmftTimeSpan.ppHourTextFrom & tmftTimeSpan.ppMinTextFrom & "00"))
                            .Add(pfSet_Param("time_to", SqlDbType.Char, tmftTimeSpan.ppHourTextTo & tmftTimeSpan.ppMinTextTo & "00"))
                        End With

                    Case "39"   '債権

                        rpt = New TBRREP008
                        cmdSQL.CommandText = "TBRREP008_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("time_from", SqlDbType.Char, tmftTimeSpan.ppHourTextFrom & tmftTimeSpan.ppMinTextFrom & "00"))
                            .Add(pfSet_Param("time_to", SqlDbType.Char, tmftTimeSpan.ppHourTextTo & tmftTimeSpan.ppMinTextTo & "00"))
                            .Add(pfSet_Param("jbno_from", SqlDbType.Char, tftJBNo.ppFromText))
                            .Add(pfSet_Param("jbno_to", SqlDbType.Char, tftJBNo.ppToText))
                            .Add(pfSet_Param("cid_from", SqlDbType.Char, tftCID.ppFromText))
                            .Add(pfSet_Param("cid_to", SqlDbType.Char, tftCID.ppToText))
                        End With

                    Case "40"   '債務

                        rpt = New TBRREP009
                        cmdSQL.CommandText = "TBRREP009_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("time_from", SqlDbType.Char, tmftTimeSpan.ppHourTextFrom & tmftTimeSpan.ppMinTextFrom & "00"))
                            .Add(pfSet_Param("time_to", SqlDbType.Char, tmftTimeSpan.ppHourTextTo & tmftTimeSpan.ppMinTextTo & "00"))
                            .Add(pfSet_Param("jbno_from", SqlDbType.Char, tftJBNo.ppFromText))
                            .Add(pfSet_Param("jbno_to", SqlDbType.Char, tftJBNo.ppToText))
                            .Add(pfSet_Param("cid_from", SqlDbType.Char, tftCID.ppFromText))
                            .Add(pfSet_Param("cid_to", SqlDbType.Char, tftCID.ppToText))
                        End With

                    Case "42"   '清算

                        rpt = New TBRREP010
                        cmdSQL.CommandText = "TBRREP010_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("time_from", SqlDbType.Char, tmftTimeSpan.ppHourTextFrom & tmftTimeSpan.ppMinTextFrom & "00"))
                            .Add(pfSet_Param("time_to", SqlDbType.Char, tmftTimeSpan.ppHourTextTo & tmftTimeSpan.ppMinTextTo & "00"))
                            .Add(pfSet_Param("jbno_from", SqlDbType.Char, tftJBNo.ppFromText))
                            .Add(pfSet_Param("jbno_to", SqlDbType.Char, tftJBNo.ppToText))
                            .Add(pfSet_Param("cid_from", SqlDbType.Char, tftCID.ppFromText))
                            .Add(pfSet_Param("cid_to", SqlDbType.Char, tftCID.ppToText))
                        End With

                    Case "ZZ"           '店内通信

                        rpt = New TBRREP012
                        cmdSQL.CommandText = "TBRREP012_S1"
                        With cmdSQL.Parameters
                            .Add(pfSet_Param("jbno_from", SqlDbType.Char, tftJBNo.ppFromText))
                            .Add(pfSet_Param("jbno_to", SqlDbType.Char, tftJBNo.ppToText))
                            .Add(pfSet_Param("select_flg", SqlDbType.Char, rblSlctPrnt.SelectedItem.Value))
                        End With

                        'Case "88"
                    Case Else

                        Exit Sub

                End Select

                'データ取得実行
                If pfExec_StoredProcedure(Me, strReportName, cmdSQL, dstOrders) = False Then
                    Exit Sub
                End If

            End Using

            'テーブルに該当レコードがない場合はポップアップを表示
            If dstOrders Is Nothing OrElse dstOrders.Tables(0).Rows.Count = 0 Then
                '該当するデータが存在しません。
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
            Else
                psPrintPDF(Me, rpt, dstOrders.Tables(0), strReportName)
            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "印刷")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面情報取得・表示
    ''' </summary>
    ''' <param name="intProcCode">処理コード 0:初回表示 1:検索</param>
    Private Sub setInfo(ByVal intProcCode As Integer)

        Dim strKeyList() As String = ViewState(P_KEY)

        lblTboxId.Text = strKeyList(10)
        lblHallNm.Text = strKeyList(11)
        lblReqDate.Text = strKeyList(9)
        lblDataClsNm.Text = strKeyList(8)


        tr_time.Visible = False
        tr_JBNo.Visible = False
        tr_CID.Visible = False
        tr_SlctPrnt.Visible = False

        'hdnDataCls.Value = aryParam("datacls")

        Select Case strKeyList(7)
            Case "25"

                lblDataClsNm.Text = "使用中カードDB"

            Case "26"

                lblDataClsNm.Text = "決済照会情報"

            Case "27"

                lblDataClsNm.Text = "店内装置構成表"

            Case "29"   'BB機番情報

                tr_JBNo.Visible = True

                lblDataClsNm.Text = "BB機番情報"

            Case "30"

                tr_time.Visible = True

                lblDataClsNm.Text = "BB装置情報"

            Case "35"

                lblDataClsNm.Text = "TBOX操作ログ"

                tr_time.Visible = True

                lblSlct.Text = "鍵位置"
                tr_SlctPrnt.Visible = True
                rblSlctPrnt.Items.Add(New ListItem("全て", "00000"))
                rblSlctPrnt.Items.Add(New ListItem("運用モードのみ", "00002"))
                rblSlctPrnt.Items.Add(New ListItem("保守モードのみ", "00001"))
                rblSlctPrnt.SelectedIndex = 0

            Case "36"

                lblDataClsNm.Text = "TBOXエラーログ"

                tr_time.Visible = True

            Case "39"

                lblDataClsNm.Text = "債権明細情報"

                tr_time.Visible = True
                tr_JBNo.Visible = True
                tr_CID.Visible = True

            Case "40"

                lblDataClsNm.Text = "債務明細情報"

                tr_time.Visible = True
                tr_JBNo.Visible = True
                tr_CID.Visible = True

            Case "42"

                lblDataClsNm.Text = "精算明細情報"

                tr_time.Visible = True
                tr_JBNo.Visible = True
                tr_CID.Visible = True

            Case "f1"

                lblDataClsNm.Text = "使用中カードログ２"

            Case "ZZ"           '店内通信

                lblDataClsNm.Text = "店内通信状況"

                tr_JBNo.Visible = True

                lblSlct.Text = "店内通信状況"
                tr_SlctPrnt.Visible = True
                rblSlctPrnt.Items.Add(New ListItem("全て", "0"))
                rblSlctPrnt.Items.Add(New ListItem("接続分", "1"))
                rblSlctPrnt.Items.Add(New ListItem("未接続分", "2"))
                rblSlctPrnt.SelectedIndex = 0

            Case "86"

                tr_JBNo.Visible = True

                lblDataClsNm.Text = "BB機番情報2"

            Case "88"

        End Select

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
