'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜保守＞
'*　処理名　　：　特別保守費用照会
'*　ＰＧＭＩＤ：　CMPINQP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.11.28　：　ＮＫＣ
'*  更　新　　：　2014.06.28　：　稲葉　　ボタン名称の変更
'*  更　新　　：　2014.09.17　：　稲葉　　NGCユーザーでの初期表示ボタン活性制御を修正
'********************************************************************************************************************************

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMExclusive
'Imports SPC.Global_asax
#End Region

Public Class CMPINQP001
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
    Const M_MY_DISP_ID = P_FUN_CMP & P_SCR_INQ & P_PAGE & "001"     '当画面（特別保守費用照会）
    Const M_MNT_DISP_ID = P_FUN_CMP & P_SCR_SEL & P_PAGE & "001"    '保守対応依頼書

    '次画面ファイルパス（保守対応依頼書）
    Const M_MNT_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "001" & "/" &
            P_FUN_CMP & P_SCR_SEL & P_PAGE & "001.aspx"
    '次画面ファイルパス（保守対応依頼書照会）
    Const M_MNTS_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_REQ & P_SCR_INQ & P_PAGE & "001" & "/" &
            P_FUN_REQ & P_SCR_INQ & P_PAGE & "001.aspx"
    '印刷画面ファイルパス（特別保守費用照会 印刷画面）
    Const M_SMPRN_DISP_PATH = "~/" & P_MAI & "/" &
            P_FUN_CMP & P_SCR_OUT & P_PAGE & "001" & "/" &
            P_FUN_CMP & P_SCR_OUT & P_PAGE & "001.aspx"

    'ビューステート
    Const M_VIEW_PLACE = "place"    '接続場所

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
    Dim clsExc As New ClsCMExclusive

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
        pfSet_GridView(Me.grvList, M_MY_DISP_ID)

    End Sub

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ボタンアクションの設定
        AddHandler Master.ppRigthButton1.Click, AddressOf btnSearch_Click
        AddHandler Master.ppRigthButton2.Click, AddressOf btnClear_Click
        AddHandler Master.Master.ppLeftButton1.Click, AddressOf btnAllSelectInsapp_Click
        AddHandler Master.Master.ppLeftButton2.Click, AddressOf btnAllCancelInsapp_Click
        AddHandler Master.Master.ppLeftButton3.Click, AddressOf btnAllSelectReqapp_Click
        AddHandler Master.Master.ppLeftButton4.Click, AddressOf btnAllCancelReqapp_Click
        AddHandler Master.Master.ppRigthButton1.Click, AddressOf btnPrint_Click
        AddHandler Master.Master.ppRigthButton2.Click, AddressOf btnApproval_Click

        If Not IsPostBack Then  '初回表示のみ

            '★排他情報用のグループ番号保管
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '接続場所の取得
            ViewState(M_VIEW_PLACE) = ConfigurationManager.AppSettings("Address").ToString

            '「検索条件クリア」、「登録」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False
            Master.ppRigthButton3.CausesValidation = False

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            'メニュー画面からパラメータ受け取り
            If Request.QueryString("param") = Nothing Then
                param.Value = "0"
            Else
                param.Value = Request.QueryString("param").ToString
            End If



            '各コマンドボタンの属性設定
            '---------------------------
            '2014/06/28 稲葉 ここから
            '---------------------------
            '--全選択（検収）
            Master.Master.ppLeftButton1.CausesValidation = False
            Master.Master.ppLeftButton1.Text = "全選択（依頼）"
            'Master.Master.ppLeftButton1.Text = "全選択（検収）"
            Master.Master.ppLeftButton1.Visible = True
            '--全解除（検収）
            Master.Master.ppLeftButton2.CausesValidation = False
            Master.Master.ppLeftButton2.Text = "全解除（依頼）"
            'Master.Master.ppLeftButton2.Text = "全解除（検収）"
            Master.Master.ppLeftButton2.Visible = True
            '--全選択（請求）
            Master.Master.ppLeftButton3.CausesValidation = False
            Master.Master.ppLeftButton3.Text = "全選択（検収）"
            'Master.Master.ppLeftButton3.Text = "全選択（請求）"
            Master.Master.ppLeftButton3.Visible = True
            '--全解除（請求）
            Master.Master.ppLeftButton4.CausesValidation = False
            Master.Master.ppLeftButton4.Text = "全解除（検収）"
            'Master.Master.ppLeftButton4.Text = "全解除（請求）"
            Master.Master.ppLeftButton4.Visible = True
            '---------------------------
            '2014/06/28 稲葉 ここまで
            '---------------------------
            '--印刷画面
            Master.Master.ppRigthButton1.CausesValidation = False
            Master.Master.ppRigthButton1.Text = "印刷画面"
            Master.Master.ppRigthButton1.Visible = True
            '--承認
            Master.Master.ppRigthButton2.Text = "承認"
            Master.Master.ppRigthButton2.Visible = True
            Master.Master.ppRigthButton2.OnClientClick =
                pfGet_OCClickMes("30006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "承認")
            Master.Master.ppRigthButton2.ValidationGroup = "Approval"

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            '画面クリア
            msClearScreen()

            'データ取得
            msGetData(False, 1)
        End If

    End Sub

    '---------------------------
    '2014/04/25 武 ここから
    '---------------------------
    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
                Master.Master.ppLeftButton3.Enabled = False
                Master.Master.ppLeftButton4.Enabled = False
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = False
                Me.lblTotalAmount.Visible = True
            Case "SPC"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
                Master.Master.ppLeftButton3.Enabled = False
                Master.Master.ppLeftButton4.Enabled = False
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Me.lblTotalAmount.Visible = False
            Case "営業所"
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
                Master.Master.ppLeftButton3.Enabled = False
                Master.Master.ppLeftButton4.Enabled = False
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Me.lblTotalAmount.Visible = False
            Case "NGC"
                Master.Master.ppLeftButton1.Enabled = True
                Master.Master.ppLeftButton2.Enabled = True
                Master.Master.ppLeftButton3.Enabled = True
                Master.Master.ppLeftButton4.Enabled = True
                Master.Master.ppRigthButton1.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True
                Me.txtChargeNm.ppEnabled = True
                Me.lblTotalAmount.Visible = True
        End Select

        '遷移元が保守メニューの場合は、権限関係なく「承認ボタン」「印刷画面ボタン」「合計金額」非活性
        If param.Value = "1" Then
            Master.Master.ppRigthButton1.Enabled = False
            Master.Master.ppRigthButton2.Enabled = False
            Me.lblTotalAmount.Visible = False
        End If

    End Sub
    Private Sub grvList_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grvList.RowDataBound

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
                e.Row.Cells(0).Enabled = False
            Case "営業所"
                e.Row.Cells(0).Enabled = False
            Case "NGC"
                e.Row.Cells(1).Enabled = False
        End Select

    End Sub
    '---------------------------
    '2014/04/25 武 ここまで
    '---------------------------

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        If (Page.IsValid) Then

            'データ取得
            msGetData()

            '検索データがあり、かつ接続場所がNGCの場合はフッター部ボタンと担当者名は活性にする
            If Master.ppCount > "0" AndAlso
               ViewState(M_VIEW_PLACE).ToString = P_ADD_NGC Then
                Master.Master.ppLeftButton1.Enabled = True
                Master.Master.ppLeftButton2.Enabled = True
                Master.Master.ppLeftButton3.Enabled = True
                Master.Master.ppLeftButton4.Enabled = True
                Master.Master.ppRigthButton2.Enabled = True
                Me.txtChargeNm.ppEnabled = True
            Else
                Master.Master.ppLeftButton1.Enabled = False
                Master.Master.ppLeftButton2.Enabled = False
                Master.Master.ppLeftButton3.Enabled = False
                Master.Master.ppLeftButton4.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Me.txtChargeNm.ppText = String.Empty
                Me.txtChargeNm.ppEnabled = False
            End If

            '遷移元が保守メニューの場合は、権限関係なく「承認ボタン」「印刷画面ボタン」「合計金額」非活性
            If param.Value = "1" Then
                Master.Master.ppRigthButton1.Enabled = False
                Master.Master.ppRigthButton2.Enabled = False
                Me.lblTotalAmount.Visible = False
            End If
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 検索条件クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnClear_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        '検索条件クリア
        msClearSearchCondition()

        Me.txtTboxId.ppTextBoxFrom.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 全選択（検収）ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllSelectInsapp_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        For Each rowData As GridViewRow In grvList.Rows

            '検収チェックボックスが非活性の場合はスキップ
            If rowData.Cells(2).Enabled = False Then
                Continue For
            End If

            '検収チェックボックスON
            CType(rowData.FindControl("検収承認"), CheckBox).Checked = True

        Next

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 全解除（検収）ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllCancelInsapp_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        For Each rowData As GridViewRow In grvList.Rows

            '検収チェックボックスが非活性の場合はスキップ
            If rowData.Cells(2).Enabled = False Then
                Continue For
            End If

            '検収チェックボックスOFF
            CType(rowData.FindControl("検収承認"), CheckBox).Checked = False

        Next

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 全選択（請求）ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllSelectReqapp_Click(sender As Object, e As EventArgs)
        Dim blnSel As Boolean = False

        '開始ログ出力
        psLogStart(Me)

        blnSel = False
        For Each rowData As GridViewRow In grvList.Rows
            '請求チェックボックスが非活性の場合はスキップ
            If rowData.Cells(3).Enabled = False Then
                Continue For
            End If

            '検収チェックボックスOFFの場合はスキップ
            If CType(rowData.FindControl("検収承認"), CheckBox).Checked = False Then
                Continue For
            End If

            '請求チェックボックスON
            CType(rowData.FindControl("請求承認"), CheckBox).Checked = True
            blnSel = True
        Next

        If blnSel = False Then
            psMesBox(Me, "10009", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "依頼承認", "チェック")
        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 全解除（請求）ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllCancelReqapp_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        For Each rowData As GridViewRow In grvList.Rows

            '請求チェックボックスが非活性の場合はスキップ
            If rowData.Cells(3).Enabled = False Then
                Continue For
            End If

            '請求チェックボックスOFF
            CType(rowData.FindControl("請求承認"), CheckBox).Checked = False

        Next

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 印刷画面ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs)

        '開始ログ出力
        psLogStart(Me)

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

        '--------------------------------
        '2014/04/16 星野　ここから
        '--------------------------------
        '■□■□結合試験時のみ使用予定□■□■
        Dim objStack As New StackFrame
        Dim strPrm As String = ""
        strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
        Dim tmp As Object() = Session(P_KEY)
        If Not tmp Is Nothing Then
            For zz = 0 To tmp.Length - 1
                If zz <> tmp.Length - 1 Then
                    strPrm &= tmp(zz).ToString & ","
                Else
                    strPrm &= tmp(zz).ToString
                End If
            Next
        End If

        psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                        objStack.GetMethod.Name, M_SMPRN_DISP_PATH, strPrm, "TRANS")
        '■□■□結合試験時のみ使用予定□■□■
        '--------------------------------
        '2014/04/16 星野　ここまで
        '--------------------------------
        '印刷画面起動
        psOpen_Window(Me, M_SMPRN_DISP_PATH)

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 承認ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnApproval_Click(sender As Object, e As EventArgs)

        Dim blnUpdflg As Boolean = True         '更新成功失敗判定フラグ
        Dim strUpdCls As String = String.Empty  '更新区分
        Dim strMntNo As String = String.Empty   '保守管理番号
        Dim strInsApp As String = String.Empty  '検収承認
        Dim strReqApp As String = String.Empty  '請求承認

        '開始ログ出力
        psLogStart(Me)

        '整合性チェック
        If (Page.IsValid AndAlso mfCheckApproval()) Then
            '更新処理
            For Each rowData As GridViewRow In grvList.Rows
                '--取消または検収済データの場合は更新処理をしない
                If CType(rowData.FindControl("取消"), TextBox).Text = "●" OrElse
                   CType(rowData.FindControl("検収区分"), TextBox).Text = "1" Then
                    Continue For
                End If

                '--更新区分設定
                '|-------------------------------------------------------------------------------------------------------------|
                '|No.|更新区分|検収承認  |請求承認  |検収 |請求 |更新|＜更新設定値＞                                           |
                '|   |        |(取得ﾃﾞｰﾀ)|(取得ﾃﾞｰﾀ)|ﾁｪｯｸ |ﾁｪｯｸ |対象|検収承認  |請求承認  |検収    |検収    |請求    |請求    |
                '|   |        |          |          |ﾎﾞｯｸｽ|ﾎﾞｯｸｽ|    |          |          |担当者名|承認日付|担当者名|承認日付|
                '|---|--------|----------|----------|-----|-----|----|----------|----------|--------|--------|--------|--------|
                '|(1)|   -    |<1:承認>  |<1:承認>  |選択 |選択 | × |-         |-         |-       |-       | -      |-       |
                '|(2)|   1    |<1:承認>  |<1:承認>  |選択 |無   | ○ |<1:承認>  |<0:未承認>|変更無し|変更無し|NULL    |NULL    |
                '|(3)|   2    |<1:承認>  |<1:承認>  |無   |無   | ○ |<0:未承認>|<0:未承認>|NULL    |NULL    |NULL    |NULL    |
                '|(4)|   3    |<1:承認>  |<0:未承認>|選択 |選択 | ○ |<1:承認>  |<1:承認>  |変更無し|変更無し|入力名  |実行日  |
                '|(5)|   -    |<1:承認>  |<0:未承認>|選択 |無   | × |-         |-         |-       |-       |-       |-       |
                '|(6)|   4    |<1:承認>  |<0:未承認>|無   |無   | ○ |<0:未承認>|<0:未承認>|NULL    |NULL    |NULL    |NULL    |
                '|(7)|   5    |<0:未承認>|<0:未承認>|選択 |選択 | ○ |<1:承認>  |<1:承認>  |入力名  |実行日  |入力名  |実行日  |
                '|(8)|   6    |<0:未承認>|<0:未承認>|選択 |無   | ○ |<1:承認>  |<0:未承認>|入力名  |実行日  |NULL    |NULL    |
                '|(9)|   -    |<0:未承認>|<0:未承認>|無   |無   | × |-         |-         |-       |-       |-       |-       |
                '--------------------------------------------------------------------------------------------------------------|
                '(1)検収承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝選択、請求ﾁｪｯｸﾎﾞｯｸｽ＝選択の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = True AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = True Then
                    Continue For
                End If

                '(2)検収承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝選択、請求ﾁｪｯｸﾎﾞｯｸｽ＝無の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = True AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = False Then
                    strUpdCls = "1"
                End If

                '(3)検収承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝無、請求ﾁｪｯｸﾎﾞｯｸｽ＝無の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = False AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = False Then
                    strUpdCls = "2"
                End If

                '(4)検収承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝選択、請求ﾁｪｯｸﾎﾞｯｸｽ＝選択の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = True AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = True Then
                    strUpdCls = "3"
                End If

                '(5)検収承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝選択、請求ﾁｪｯｸﾎﾞｯｸｽ＝無の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = True AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = False Then
                    Continue For
                End If

                '(6)検収承認フラグ(取得ﾃﾞｰﾀ)＝<1:承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝無、請求ﾁｪｯｸﾎﾞｯｸｽ＝無の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "1" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = False AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = False Then
                    strUpdCls = "4"
                End If

                '(7)検収承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝選択、請求ﾁｪｯｸﾎﾞｯｸｽ＝選択の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = True AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = True Then
                    strUpdCls = "5"
                End If

                '(8)検収承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝選択、請求ﾁｪｯｸﾎﾞｯｸｽ＝無の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = True AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = False Then
                    strUpdCls = "6"
                End If

                '(9)検収承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、請求承認フラグ(取得ﾃﾞｰﾀ)＝<0:未承認>、
                '   検収ﾁｪｯｸﾎﾞｯｸｽ＝無、請求ﾁｪｯｸﾎﾞｯｸｽ＝無の場合
                If CType(rowData.FindControl("検収承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("請求承認_取得時"), TextBox).Text = "0" AndAlso
                   CType(rowData.FindControl("検収承認"), CheckBox).Checked = False AndAlso
                   CType(rowData.FindControl("請求承認"), CheckBox).Checked = False Then
                    Continue For
                End If

                '--保守管理番号設定
                strMntNo = CType(rowData.FindControl("保守管理番号"), TextBox).Text

                '--検収承認設定
                If CType(rowData.FindControl("検収承認"), CheckBox).Checked Then
                    strInsApp = "1"
                Else
                    strInsApp = "0"
                End If

                '--請求承認設定
                If CType(rowData.FindControl("請求承認"), CheckBox).Checked Then
                    strReqApp = "1"
                Else
                    strReqApp = "0"
                End If

                'データ更新処理
                If Not mfUpdateData(strUpdCls, strMntNo, strInsApp, strReqApp,
                                    Me.txtChargeNm.ppText) Then
                    blnUpdflg = False
                    Exit For
                End If

            Next

            '更新成功時
            If blnUpdflg Then
                '承認完了メッセージ表示
                psMesBox(Me, "30007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)

                'データ取得
                msGetData(False)

                '検索データがあり、かつ接続場所がNGCの場合はフッター部ボタンと担当者名は活性にする
                If Master.ppCount > "0" AndAlso
                   ViewState(M_VIEW_PLACE).ToString = P_ADD_NGC Then
                    Master.Master.ppLeftButton1.Enabled = True
                    Master.Master.ppLeftButton2.Enabled = True
                    Master.Master.ppLeftButton3.Enabled = True
                    Master.Master.ppLeftButton4.Enabled = True
                    Master.Master.ppRigthButton2.Enabled = True
                    Me.txtChargeNm.ppEnabled = True
                Else
                    Master.Master.ppLeftButton1.Enabled = False
                    Master.Master.ppLeftButton2.Enabled = False
                    Master.Master.ppLeftButton3.Enabled = False
                    Master.Master.ppLeftButton4.Enabled = False
                    Master.Master.ppRigthButton2.Enabled = False
                    Me.txtChargeNm.ppText = String.Empty
                    Me.txtChargeNm.ppEnabled = False
                End If
            End If

        End If

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッドのボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand

        Dim rowData As GridViewRow = Nothing        'ボタン押下行
        Dim strKeyList As List(Of String) = Nothing '次画面引継ぎ用キー情報

        If e.CommandName <> "btnReference" AndAlso e.CommandName <> "btnUpdate" Then
            Exit Sub
        End If

        '開始ログ出力
        psLogStart(Me)

        'ボタン押下行の情報を取得
        rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

        '次画面引継ぎ用キー情報設定
        strKeyList = New List(Of String)
        strKeyList.Add(CType(rowData.FindControl("保守管理番号"), TextBox).Text)

        'セッション情報設定
        Session(P_SESSION_BCLIST) = Master.Master.ppBcList_Text
        Session(P_SESSION_OLDDISP) = M_MY_DISP_ID
        Session(P_KEY) = strKeyList.ToArray
        Select Case e.CommandName
            Case "btnReference" '参照
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.参照

                '★排他情報のグループ番号をセッション変数に設定
                Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                '接続場所がNGCの場合は保守対応依頼書照会(REQINQP001)を
                'NGC以外の場合は保守対応依頼書(CMPSELP001)を起動する
                If ViewState(M_VIEW_PLACE).ToString = P_ADD_NGC Then
                    '--------------------------------
                    '2014/04/16 星野　ここから
                    '--------------------------------
                    '■□■□結合試験時のみ使用予定□■□■
                    Dim objStack As New StackFrame
                    Dim strPrm As String = ""
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                    Dim tmp As Object() = Session(P_KEY)
                    If Not tmp Is Nothing Then
                        For zz = 0 To tmp.Length - 1
                            If zz <> tmp.Length - 1 Then
                                strPrm &= tmp(zz).ToString & ","
                            Else
                                strPrm &= tmp(zz).ToString
                            End If
                        Next
                    End If

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, M_MNTS_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, M_MNTS_DISP_PATH)
                Else
                    '--------------------------------
                    '2014/04/16 星野　ここから
                    '--------------------------------
                    '■□■□結合試験時のみ使用予定□■□■
                    Dim objStack As New StackFrame
                    Dim strPrm As String = ""
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                    Dim tmp As Object() = Session(P_KEY)
                    If Not tmp Is Nothing Then
                        For zz = 0 To tmp.Length - 1
                            If zz <> tmp.Length - 1 Then
                                strPrm &= tmp(zz).ToString & ","
                            Else
                                strPrm &= tmp(zz).ToString
                            End If
                        Next
                    End If

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, M_MNT_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    psOpen_Window(Me, M_MNT_DISP_PATH)
                End If

            Case "btnUpdate"    '更新
                Session(P_SESSION_TERMS) = ClsComVer.E_遷移条件.更新

                '★排他制御用の変数
                Dim dtExclusiveDate As String = String.Empty
                Dim arTable_Name As New ArrayList
                Dim arKey As New ArrayList

                '★ロック対象テーブル名の登録
                arTable_Name.Insert(0, "D75_DEAL_MAINTAIN")
                arTable_Name.Insert(1, "D76_DEALMAINTAIN_DTIL")

                '★ロックテーブルキー項目の登録(D75_DEAL_MAINTAIN,D76_DEALMAINTAIN_DTIL)
                arKey.Insert(0, CType(rowData.FindControl("保守管理番号"), TextBox).Text) 'D75_MNT_NO
                arKey.Insert(1, CType(rowData.FindControl("保守管理番号"), TextBox).Text) 'D76_MNT_NO

                '★排他情報確認処理(更新処理の実行)
                If clsExc.pfSel_Exclusive(dtExclusiveDate _
                                 , Me _
                                 , Session(P_SESSION_IP) _
                                 , Session(P_SESSION_PLACE) _
                                 , Session(P_SESSION_USERID) _
                                 , Session(P_SESSION_SESSTION_ID) _
                                 , ViewState(P_SESSION_GROUP_NUM) _
                                 , M_MNT_DISP_ID _
                                 , arTable_Name _
                                 , arKey) = 0 Then

                    '★排他情報のグループ番号をセッション変数に設定
                    Session(P_SESSION_GROUP_NUM) = ViewState(P_SESSION_GROUP_NUM)

                    '--------------------------------
                    '2014/04/16 星野　ここから
                    '--------------------------------
                    '■□■□結合試験時のみ使用予定□■□■
                    Dim objStack As New StackFrame
                    Dim strPrm As String = ""
                    strPrm &= CType(Session(P_SESSION_TERMS), String) & ","
                    Dim tmp As Object() = Session(P_KEY)
                    If Not tmp Is Nothing Then
                        For zz = 0 To tmp.Length - 1
                            If zz <> tmp.Length - 1 Then
                                strPrm &= tmp(zz).ToString & ","
                            Else
                                strPrm &= tmp(zz).ToString
                            End If
                        Next
                    End If


                    '★登録年月日時刻
                    Me.Master.Master.ppExclusiveDate = dtExclusiveDate
                    '★登録年月日時刻
                    Session(P_SESSION_EXCLUSIV_DATE) = dtExclusiveDate

                    psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                    objStack.GetMethod.Name, M_MNT_DISP_PATH, strPrm, "TRANS")
                    '■□■□結合試験時のみ使用予定□■□■
                    '--------------------------------
                    '2014/04/16 星野　ここまで
                    '--------------------------------
                    '保守対応依頼書画面起動
                    psOpen_Window(Me, M_MNT_DISP_PATH)

                Else

                    '排他ロック中

                End If

        End Select

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' データバインド時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        Dim strPlace As String = ViewState(M_VIEW_PLACE) '場所（SPC、NGC、作業拠点）
        Dim strCancelCls As String = String.Empty   'キャンセル区分
        Dim strNLCls As String = String.Empty       'ＮＬ区分
        Dim dcmReqPrice As Decimal = 0              '請求金額
        Dim strInsCls As String = String.Empty      '検収区分
        Dim dcmSumReqPrice As Decimal = 0           '請求金額合計

        For Each rowData As GridViewRow In grvList.Rows

            'グリッド内容取得
            strCancelCls = CType(rowData.FindControl("取消"), TextBox).Text
            strNLCls = CType(rowData.FindControl("ＮＬ区分"), TextBox).Text
            Decimal.TryParse(CType(rowData.FindControl("請求金額"), TextBox).Text, dcmReqPrice)
            strInsCls = CType(rowData.FindControl("検収区分"), TextBox).Text

            If strPlace = P_ADD_NGC Then
                '---------------------------------------接続場所がNGCの場合
                '更新ボタンを非活性
                rowData.Cells(1).Enabled = False

                'キャンセル区分が１：取消、または検収区分が１：検収済の場合、
                '検収と請求のチェックボックスを非活性にする
                If strCancelCls = "●" OrElse strInsCls = "1" Then
                    rowData.Cells(2).Enabled = False
                    rowData.Cells(3).Enabled = False
                End If

            Else
                '---------------------------------------接続場所がNGC以外の場合
                '参照ボタンを非活性
                rowData.Cells(0).Enabled = False

                'キャンセル区分が１：取消の場合、更新ボタンを非活性にする
                If strCancelCls = "●" Then
                    rowData.Cells(1).Enabled = False
                End If
                '検収と請求のチェックボックスを非活性にする
                rowData.Cells(2).Enabled = False
                rowData.Cells(3).Enabled = False
            End If

            'ＮＬ区分が"N"以外の場合、TOXBOXIDを青色にする
            'If strNLCls <> "N" Then
            '    CType(rowData.FindControl("ＴＢＯＸＩＤ"), TextBox).ForeColor = Drawing.Color.Blue
            'End If

            '請求金額を加算
            dcmSumReqPrice += dcmReqPrice
        Next

        '請求金額合計を表示
        Me.lblTotalAmount.Text = "合計金額： " + dcmSumReqPrice.ToString("#,##0")

    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------
        Try
            '検索条件クリア
            msClearSearchCondition()

            'グリッド初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            '担当者、コマンドボタン初期化
            Me.txtChargeNm.ppText = String.Empty
            Me.lblTotalAmount.Text = String.Empty
            Master.Master.ppLeftButton1.Enabled = False
            Master.Master.ppLeftButton2.Enabled = False
            Master.Master.ppLeftButton3.Enabled = False
            Master.Master.ppLeftButton4.Enabled = False
            Master.Master.ppRigthButton1.Enabled = True
            Master.Master.ppRigthButton2.Enabled = False
            Me.txtChargeNm.ppEnabled = False
            Me.txtTboxId.ppTextBoxFrom.Focus()

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
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
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        Me.txtTboxId.ppFromText = String.Empty
        Me.txtTboxId.ppToText = String.Empty
        Me.dtbSupportDt.ppFromText = String.Empty
        Me.dtbSupportDt.ppToText = String.Empty
        Me.tmbStartTm.ppHourTextFrom = String.Empty
        Me.tmbStartTm.ppMinTextFrom = String.Empty
        Me.tmbStartTm.ppHourTextTo = String.Empty
        Me.tmbStartTm.ppMinTextTo = String.Empty
        Me.rbtInsApp.SelectedIndex = 1
        Me.rbtReqApp.SelectedIndex = 1
        Me.cbxCancel.Checked = False

    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetData(Optional ByVal blnMsgFlg As Boolean = True, Optional ByVal intSearch_cls As Integer = 2)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strBuff As String = String.Empty    '文字列バッファ
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        'グリッド及び件数の初期化
        Me.grvList.DataSource = New DataTable
        Me.grvList.DataBind()
        Master.ppCount = "0"

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤFrom
                    If Me.txtTboxId.ppToText = String.Empty OrElse
                       Me.txtTboxId.ppToText = Me.txtTboxId.ppFromText Then
                        'ＴＢＯＸＩＤToが空白、またはFromToが一致した場合は「あいまい検索」なのでエスケープする
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar,
                                         Me.txtTboxId.ppFromText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    Else
                        '上記以外は「範囲検索」なのでエスケープしない
                        .Add(pfSet_Param("tboxid_f", SqlDbType.NVarChar, Me.txtTboxId.ppFromText))
                    End If

                    'ＴＢＯＸＩＤTo
                    .Add(pfSet_Param("tboxid_t", SqlDbType.NVarChar, Me.txtTboxId.ppToText))

                    '対応日From
                    .Add(pfSet_Param("startdt_f", SqlDbType.NVarChar, Me.dtbSupportDt.ppFromText))

                    '対応日To
                    .Add(pfSet_Param("startdt_t", SqlDbType.NVarChar, Me.dtbSupportDt.ppToText))

                    '開始From
                    If Me.tmbStartTm.ppHourTextFrom = String.Empty AndAlso
                       Me.tmbStartTm.ppMinTextFrom = String.Empty Then
                        strBuff = String.Empty
                    Else
                        strBuff = Me.tmbStartTm.ppHourTextFrom + ":" + Me.tmbStartTm.ppMinTextFrom
                    End If
                    .Add(pfSet_Param("starttm_f", SqlDbType.NVarChar, strBuff))

                    '開始To
                    If Me.tmbStartTm.ppHourTextTo = String.Empty AndAlso
                       Me.tmbStartTm.ppMinTextTo = String.Empty Then
                        strBuff = String.Empty
                    Else
                        strBuff = Me.tmbStartTm.ppHourTextTo + ":" + Me.tmbStartTm.ppMinTextTo
                    End If
                    .Add(pfSet_Param("starttm_t", SqlDbType.NVarChar, strBuff))

                    '検収承認
                    If Me.rbtInsApp.SelectedValue = "承認" Then
                        strBuff = "1"
                    Else
                        strBuff = "0"
                    End If
                    .Add(pfSet_Param("insapp", SqlDbType.NVarChar, strBuff))

                    '請求承認
                    If Me.rbtReqApp.SelectedValue = "承認" Then
                        strBuff = "1"
                    Else
                        strBuff = "0"
                    End If
                    .Add(pfSet_Param("reqapp", SqlDbType.NVarChar, strBuff))

                    '取消
                    If Me.cbxCancel.Checked Then
                        strBuff = "X"
                    Else
                        strBuff = String.Empty
                    End If
                    .Add(pfSet_Param("cancel", SqlDbType.NVarChar, strBuff))

                    .Add(pfSet_Param("search_cls", SqlDbType.NVarChar, intSearch_cls.ToString))

                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '件数を設定
                If objDs.Tables(0).Rows.Count = 0 Then
                    Master.ppCount = "0"
                    If blnMsgFlg = True Then
                        psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                Else
                    '閾値を超えた場合はメッセージを表示
                    If objDs.Tables(0).Rows(0)("データ件数") > objDs.Tables(0).Rows.Count Then
                        psMesBox(Me, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                            objDs.Tables(0).Rows(0)("データ件数").ToString, objDs.Tables(0).Rows.Count.ToString)
                    End If
                    Master.ppCount = objDs.Tables(0).Rows(0)("データ件数").ToString
                End If

                '取得したデータをグリッドに設定
                Me.grvList.DataSource = objDs.Tables(0)

                '変更を反映
                Me.grvList.DataBind()

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "特別保守費用照会")
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

    End Sub

    ''' <summary>
    ''' 承認前整合性チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCheckApproval() As Boolean

        For Each rowData As GridViewRow In grvList.Rows

            '取消または検収済データの場合はスキップ
            If CType(rowData.FindControl("取消"), TextBox).Text = "●" OrElse
               CType(rowData.FindControl("検収区分"), TextBox).Text = "1" Then
                Continue For
            End If

            '検収が未チェックで請求がチェックされている場合はエラー
            If CType(rowData.FindControl("検収承認"), CheckBox).Checked = False AndAlso
               CType(rowData.FindControl("請求承認"), CheckBox).Checked = True Then
                psMesBox(Me, "30003", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                hdnData.Value = String.Empty
                CType(rowData.FindControl("請求承認"), CheckBox).Focus()
                Return False
            End If

        Next

        Return True

    End Function

    Private Function mfUpdateData(ByVal strUpdCls As String, ByVal strMntNo As String,
                                  ByVal strInsApp As String, ByVal strReqApp As String,
                                  ByVal strChargeNm As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strBuff As String = String.Empty
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        mfUpdateData = False

        '保守対応データ更新
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_U1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    '--更新区分
                    .Add(pfSet_Param("updcls", SqlDbType.NVarChar, strUpdCls))
                    '--保守管理番号
                    .Add(pfSet_Param("mntno", SqlDbType.NVarChar, strMntNo))
                    '--検収承認
                    .Add(pfSet_Param("ins_app", SqlDbType.NVarChar, strInsApp))
                    '--請求承認
                    .Add(pfSet_Param("req_app", SqlDbType.NVarChar, strReqApp))
                    '--担当者
                    .Add(pfSet_Param("chargenm", SqlDbType.NVarChar, strChargeNm))
                    '--ユーザＩＤ
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                    '--戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))
                End With

                'データ更新
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)
                    If intRtn <> 0 Then
                        psMesBox(Me, "00007", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "特別保守費用照会", intRtn.ToString)
                        Exit Function
                    End If

                    'コミット
                    conTrn.Commit()

                End Using

                mfUpdateData = True

            Catch ex As Exception
                psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "特別保守費用照会")
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
        Else
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End If

    End Function

#End Region

#Region "終了処理プロシージャ"
#End Region


End Class
