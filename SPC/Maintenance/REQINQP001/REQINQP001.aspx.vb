'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　保守対応依頼書照会
'*　ＰＧＭＩＤ：　REQINQP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.01.07　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
#End Region

Public Class REQINQP001
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    '============================================================================================================================
    '=　定数定義
    '============================================================================================================================
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_REQ & P_SCR_INQ & P_PAGE & "001"
#End Region

#Region "構造体・列挙体定義"
    '============================================================================================================================
    '=　構造体・列挙体定義
    '============================================================================================================================
#End Region

#Region "変数定義"
    '============================================================================================================================
    '=　変数定義
    '============================================================================================================================
    '--------------------------------
    '2014/04/14 星野　ここから
    '--------------------------------
    Dim objStack As StackFrame
    '--------------------------------
    '2014/04/14 星野　ここまで
    '--------------------------------
    Dim clsDataConnect As New ClsCMDataConnect
    Dim clsCMDBC As New ClsCMDBCom

#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' ページ初期処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'グリッドXML定義ファイル取得
        pfSet_GridView(Me.grvList, M_MY_DISP_ID, 70, 10)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnPrint_Click

        If Not IsPostBack Then  '初回表示のみ

            'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)

            '「印刷」ボタン設定
            Master.ppRigthButton1.Text = P_BTN_NM_PRI
            Master.ppRigthButton1.Visible = True

            'データ取得
            If mfGetData() Then
                Master.ppRigthButton1.Enabled = True
            Else
                Master.ppRigthButton1.Enabled = False
            End If

            '保守管理番号へフォーカス
            Me.lblMntNo.Focus()

        End If

    End Sub

    '---------------------------
    '2014/04/23 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/23 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 印刷ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット
        Dim rpt As Object = Nothing     'ActiveReports用クラス

        '開始ログ出力
        psLogStart(Me)

        'データ取得処理
        objDs = mfGetDataMntHistLst()

        '保守対応状況リスト印刷
        If Not objDs Is Nothing Then
            '帳票出力
            rpt = New REQREP004
            psPrintPDF(Me, rpt, objDs.Tables(0), "保守対応状況リスト")
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData() As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strMntNo As String = String.Empty   '保守管理番号（親画面からの引継キー）
        Dim dtRow As DataRow = Nothing          'DataRow
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetData = False

        'グリッドの初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '保守管理番号設定
                strMntNo = DirectCast(Session(P_KEY), String())(0)

                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得したデータを表示（テキストボックスに設定）
                If objDs.Tables(0).Rows.Count = 0 Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書照会")
                    Exit Function
                Else
                    dtRow = objDs.Tables(0).Rows(0)

                    Me.lblMntNo.Text = dtRow("管理番号").ToString
                    Me.lblSpecialMnt.Text = dtRow("特別保守").ToString
                    Me.lblNlCls.Text = dtRow("ＮＬ区分").ToString
                    Me.lblTboxid.Text = dtRow("ＴＢＯＸＩＤ").ToString
                    Me.lblTboxType.Text = dtRow("ＴＢＯＸタイプ").ToString
                    Me.lblTboxVer.Text = dtRow("ＶＥＲ").ToString
                    Me.lblHallNm.Text = dtRow("ホール名").ToString
                    Me.lblEwCls.Text = dtRow("ＥＷ区分").ToString
                    Me.lblAddr1.Text = dtRow("住所１").ToString
                    Me.lblAddr2.Text = dtRow("住所２").ToString
                    Me.lblMntNm.Text = dtRow("保担名").ToString
                    Me.lblTelno.Text = dtRow("ＴＥＬ").ToString
                    Me.lblUnfNm.Text = dtRow("統括保担名").ToString
                    Me.lblRptDt.Text = dtRow("申告日").ToString
                    Me.lblRptBase.Text = dtRow("申告元").ToString
                    Me.lblRcptDt.Text = dtRow("受付日時").ToString
                    Me.lblReqDt.Text = dtRow("依頼日時").ToString
                    Me.lblRptcdDtl.Text = dtRow("申告内容").ToString
                    Me.lblRptDtl1.Text = dtRow("申告内容１").ToString
                    Me.lblRptDtl2.Text = dtRow("申告内容２").ToString
                    Me.lblWrkDt.Text = dtRow("作業予定日時").ToString
                    Me.lblStartDt.Text = dtRow("開始日時").ToString
                    Me.lblEndDt.Text = dtRow("終了日時").ToString
                    Me.lblStatus.Text = dtRow("作業状況").ToString
                    Me.lblStNotext.Text = dtRow("作業日時").ToString
                    Me.lblAppaCd.Text = dtRow("故障機器").ToString
                    Me.lblRepairCdCntnt.Text = dtRow("回復内容").ToString
                    Me.lblRepairCntnt.Text = dtRow("回復内容２").ToString
                    Me.lblNotetext1.Text = dtRow("備考・連絡１").ToString
                    Me.lblNotetext2.Text = dtRow("備考・連絡２").ToString
                    Me.lblMntFlg.Text = dtRow("特別保守フラグ").ToString
                    Me.lblInsApp.Text = dtRow("検収承認").ToString
                    Me.lblReqApp.Text = dtRow("請求承認").ToString
                    Me.lblDeptTm.Text = dtRow("出発時間").ToString
                    Me.lblStartTm.Text = dtRow("開始時間").ToString
                    Me.lblEndTm.Text = dtRow("終了時間").ToString
                    Me.lblWrkCntnt.Text = dtRow("作業内容").ToString
                    Me.lblMntTm.Text = dtRow("特別保守作業時間").ToString
                    Me.lblGbTm.Text = dtRow("特別保守往復時間").ToString
                    Me.lblPsnNum.Text = dtRow("作業人数").ToString
                    Me.lblSubmitDt.Text = dtRow("提出日").ToString
                    Me.lblMakeRepair.Text = dtRow("メーカ修理").ToString
                    Me.lblNotetext.Text = dtRow("備考").ToString
                    Me.lblDealDtl.Text = dtRow("処置内容").ToString
                    Me.lblTransDt.Text = dtRow("輸送日").ToString
                    Me.lblTransSource.Text = dtRow("輸送元").ToString
                    Me.lblTransDest.Text = dtRow("輸送先").ToString
                    Me.lblTransItem.Text = dtRow("輸送物品").ToString
                    Me.lblTransReason.Text = dtRow("輸送理由").ToString
                    Me.lblTransComp.Text = dtRow("輸送会社").ToString
                    Me.lblTransCls.Text = dtRow("輸送区分").ToString
                End If

                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

                mfGetData = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "保守対応依頼書照会")
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                    mfGetData = False
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 保守対応状況リスト印刷用データ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetDataMntHistLst() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfGetDataMntHistLst = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, Me.lblMntNo.Text))

                End With

                'データ取得
                mfGetDataMntHistLst = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, Master.ppTitle)
                '--------------------------------
                '2014/04/14 星野　ここから
                '--------------------------------
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                '--------------------------------
                '2014/04/14 星野　ここまで
                '--------------------------------
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        For Each rowData As GridViewRow In grvList.Rows
            If (rowData.RowIndex Mod 2) = 1 Then
                CType(rowData.FindControl("対応コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 218, 238, 243)
            Else
                CType(rowData.FindControl("対応コード"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("対応日時"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
                CType(rowData.FindControl("対応内容"), TextBox).BackColor = Drawing.Color.FromArgb(255, 255, 255, 255)
            End If
        Next

    End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
