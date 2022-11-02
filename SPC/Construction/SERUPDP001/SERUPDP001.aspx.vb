'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜工事＞
'*　処理名　　：　シリアル登録
'*　ＰＧＭＩＤ：　SERUPDP001
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.11　：　ＮＫＣ
'********************************************************************************************************************************
'-------------------------------------------------------------------------------------------------------------------------------
'  番号           |  日付        |  名前    |   備考
'-------------------------------------------------------------------------------------------------------------------------------
'SERUPDP001-001     2016/05/25      栗原  　  選択ボタン押下時処理にTry～Catch文を追加、レイアウト微調整、ドロップダウンリストの挙動修正         
'                                             登録／更新時のメッセージ追加
'SERUPDP001-002     2016/08/03      栗原      シリアルテーブルの定義変更、削除機能追加
'SERUPDP001-003     2017/04/10      加賀      検索条件を保存、シリアル登録/更新時に再検索するように変更


#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
#End Region

Public Class SERUPDP001

#Region "継承定義"
    Inherits System.Web.UI.Page
#End Region

#Region "定数定義"
    'プログラムID
    Const M_MY_DISP_ID = P_FUN_SER & P_SCR_UPD & P_PAGE & "001"

    'メッセージ（ポップアップ）
    Const M_INFO_MESSAGE = "シリアル情報"

    '業者コード
    Const M_TRD_EIGYO = "2"  '営業所
    Const M_TRD_HOSHU = "3"  '保守拠点
    Const M_TRD_DAIRI = "4"  '代理店
    Const M_TRD_MAKER = "5"  'メーカ
    Const M_TRD_SONOTA = "6" 'その他

    '機器分類コード
    Const M_APPADIV_TBOX = "01" 'ＴＢＯＸ

    '機器種別コード
    Const M_APPACLS_CTL = "01" '制御部
    Const M_APPACLS_HDD = "09" 'ＨＤＤ

    Const sCnsProgId As String = "SERUPDP001"

    'ViewState
    Const VS_SQLPRM As String = "VS_SQLPRM"

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
    'SERUPDP001-002
    Dim strMode As String '画面活性制御用変数
    'SERUPDP001-002 END
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

        'ドロップダウンリストアクションの設定
        AddHandler Me.ddlRstPlaceCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlRstPlaceCls_SelectedIndexChanged
        'AddHandler Me.ddlCndAppaCls.SelectedIndexChanged, AddressOf ddlCndAppaCls_SelectedIndexChanged
        AddHandler Me.ddlRstMoveCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlRstMoveCls_SelectedIndexChanged
        Me.ddlRstPlaceCls.ppDropDownList.AutoPostBack = True
        Me.ddlRstMoveCls.ppDropDownList.AutoPostBack = True
        ' Me.ddlCndAppaCls.AutoPostBack = True

        'SERUPDP001-002
        AddHandler Me.ddlCndPlaceCls.ppDropDownList.SelectedIndexChanged, AddressOf ddlCndPlaceCls_SelectedIndexChanged
        ddlCndPlaceCls.ppDropDownList.AutoPostBack = True

        AddHandler Me.txtCndStrageCd.ppTextBox.TextChanged, AddressOf txtCndStrageCd_TextChanged
        Me.txtCndStrageCd.ppTextBox.AutoPostBack = True

        ddlCondNm.Attributes.Add("onMouseDown", "ddlReload(" & ddlCondNm.ClientID & ");")
        ddlRstVersion.ppDropDownList.Attributes.Add("onMouseDown", "ddlReload(" & ddlRstVersion.ppDropDownList.ClientID & ");")
        'SERUPDP001-002 END

        'テキストボックスアクションの設定
        AddHandler Me.txtRstStrageCd.ppTextBox.TextChanged, AddressOf txtRstStrageCd_TextChanged
        AddHandler Me.txtRstMoveCd.ppTextBox.TextChanged, AddressOf txtRstMoveCd_TextChanged
        Me.txtRstStrageCd.ppTextBox.AutoPostBack = True
        Me.txtRstMoveCd.ppTextBox.AutoPostBack = True
        'SERUPDP001-002
        AddHandler Me.txtRstSerialNo.ppTextBox.TextChanged, AddressOf txtRstSerialNo_TextChanged
        Me.txtRstSerialNo.ppTextBox.AutoPostBack = True
        'SERUPDP001-002 END



        If Not IsPostBack Then  '初回表示のみ

            '排他情報用のグループ番号保管
            If Not Session(P_SESSION_GROUP_NUM) Is Nothing Then

                ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            End If

            '「検索条件クリア」ボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'SERUPDP001-002
            '「クリア」ボタン(明細)押下時の検証を無効
            btnDetailClear.CausesValidation = False
            'SERUPDP001-002 END

            'プログラムID、画面名設定
            Master.Master.ppProgramID = M_MY_DISP_ID
            Master.Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            Master.Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.Master.ppTitle)

            '各コマンドボタンの属性設定
            '--追加
            Me.btnDetailInsert.OnClientClick =
                pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_INFO_MESSAGE)
            '--更新
            Me.btnDetailUpdate.OnClientClick =
                pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_INFO_MESSAGE)
            '--削除
            Me.btnDetailDelete.OnClientClick =
                pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, M_INFO_MESSAGE)
            '削除ボタンの色を設定
            Me.btnDetailDelete.BackColor = Drawing.Color.FromArgb(255, 102, 102)

            'マルチビューに検索エリアを表示する
            Master.ppMultiView.ActiveViewIndex = ClsComVer.E_照会マルチビュー.一覧表示

            'ドロップダウンリスト設定
            msSetddlSystem(0)       'システム   --- 変更 2014/06/11 ----
            msSetddlAppadiv(0)      '機器分類   --- 変更 2014/06/11 ----
            Me.ddlRstAppaCls.Items.Clear()
            Me.ddlRstAppaCls.Items.Insert(0, New ListItem(Nothing, Nothing))
            msSetddlMoveReason()    '移動理由
            msSetddlHDD()           'ＨＤＤ
            'SERUPDP001-002 
            msSetddlCondNm()        '機器備考
            msSetddlVersion()       'バージョン
            'SERUPDP001-002 END

            '画面クリア
            msClearScreen()

        End If

    End Sub

    ''' <summary>
    ''' ユーザー権限
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Select Case Session(P_SESSION_AUTH)
            Case "管理者"
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

        'SERUPDP001-002
        If ddlRstAppaDiv.SelectedValue = String.Empty OrElse ddlRstAppaCls.SelectedValue = String.Empty OrElse txtRstSerialNo.ppText.Trim = String.Empty Then
            strMode = "Default"
        End If
        msSetEnable(strMode)
        'SERUPDP001-002 END

    End Sub

    ''' <summary>
    ''' 検索ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)

        Dim objDs As DataSet = Nothing  'データセット

        'データ取得
        If (Page.IsValid) Then
            '明細クリア
            'SERUPDP001-002
            'msClearDetail()
            'SERUPDP001-002 END

            'データ取得処理
            objDs = mfGetData(1)

            If Not objDs Is Nothing Then
                '画面表示
                msDispData(objDs)

                '機器分類へフォーカス
                Me.ddlRstAppaDiv.Focus()
            End If
        End If

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

        Me.txtCndSerialNo.ppTextBox.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細クリアボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailClear_Click(sender As Object, e As EventArgs) Handles btnDetailClear.Click

        '開始ログ出力
        psLogStart(Me)

        '明細クリア
        msClearDetail()

        strMode = "Default"

        '機器分類へフォーカス
        Me.ddlRstAppaDiv.Focus()

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細追加ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailInsert_Click(sender As Object, e As EventArgs) Handles btnDetailInsert.Click

        '開始ログ出力
        psLogStart(Me)

        Try

            '整合性チェック
            msCheckUpdateDetail()

            '入力内容検証
            If (Page.IsValid) Then

                '明細追加処理
                If mfUpdateDataDetail(1) Then

                    '一覧更新
                    'msAddGridData() 'SERUPDP001-003
                    msDispData(mfGetData(0))

                    '明細クリア
                    msClearDetail()

                    '機器分類へフォーカス
                    Me.ddlRstAppaDiv.Focus()

                End If
            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE & "の登録")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細更新ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailUpdate_Click(sender As Object, e As EventArgs) Handles btnDetailUpdate.Click

        '開始ログ出力
        psLogStart(Me)

        Try

            '整合性チェック
            msCheckUpdateDetail()

            '入力内容検証
            If (Page.IsValid) Then

                '明細更新処理
                If mfUpdateDataDetail(2) Then

                    '一覧更新
                    'msAddGridData() 'SERUPDP001-003
                    msDispData(mfGetData(0))

                    '明細クリア
                    msClearDetail()

                    '機器分類へフォーカス
                    Me.ddlRstAppaDiv.Focus()

                End If
            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE & "の更新")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' 明細削除ボタンクリック時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDetailDelete_Click(sender As Object, e As EventArgs) Handles btnDetailDelete.Click

        '開始ログ出力
        psLogStart(Me)

        Try

            '明細削除処理
            If mfUpdateDataDetail(3) Then

                '一覧更新
                'msDelGridData()    'SERUPDP001-003
                msDispData(mfGetData(0))

                '明細クリア
                msClearDetail()

                '機器分類へフォーカス
                Me.ddlRstAppaDiv.Focus()

            End If

        Catch ex As Exception

            'エラーメッセージ表示
            psMesBox(Me, "30015", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE & "の更新")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

        '終了ログ出力
        psLogEnd(Me)

    End Sub

    ''' <summary>
    ''' グリッド選択ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grvList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grvList.RowCommand
        'SERUPDP001-001 Try文の追加、DropDownListに選択した項目が無かった場合のエラー処理追加
        Try
            Dim rowData As GridViewRow = Nothing    'ボタン押下行

            If e.CommandName <> "btnSelect" Then
                Exit Sub
            End If

            '開始ログ出力
            psLogStart(Me)

            '明細クリア
            'msClearDetail()

            'ボタン押下行の情報を取得
            rowData = grvList.Rows(Convert.ToInt32(e.CommandArgument))

            '明細の各項目に対してデータを設定
            If ddlRstAppaDiv.Items.FindByValue(CType(rowData.FindControl("機器分類コード"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("機器分類コード"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstAppaDiv.SelectedValue = CType(rowData.FindControl("機器分類コード"), TextBox).Text
                ddlRstAppaDiv_SelectedIndexChanged(sender, e)
            End If

            If ddlRstAppaCls.Items.FindByValue(CType(rowData.FindControl("機器分類コード"), TextBox).Text & ":" & CType(rowData.FindControl("機器種別コード"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("機器分類コード"), TextBox).Text & ":" & CType(rowData.FindControl("機器種別コード"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstAppaCls.SelectedValue = CType(rowData.FindControl("機器分類コード"), TextBox).Text & ":" & CType(rowData.FindControl("機器種別コード"), TextBox).Text
            End If

            If ddlRstSystem.Items.FindByValue(CType(rowData.FindControl("システムコード"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("システムコード"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstSystem.SelectedValue = CType(rowData.FindControl("システムコード"), TextBox).Text
                Me.ddlRstSystem_SelectedIndexChanged(sender, e)
            End If

            'SERUPDP001-002
            'Me.txtRstVersion.ppText = CType(rowData.FindControl("ＶＥＲ"), TextBox).Text
            If ddlRstVersion.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("ＶＥＲ"), TextBox).Text) Is Nothing Then
                ddlRstVersion.ppDropDownList.Items.Insert(0, New ListItem(CType(rowData.FindControl("ＶＥＲ"), TextBox).Text, CType(rowData.FindControl("ＶＥＲ"), TextBox).Text)) '先頭に空白行を追加
                ddlRstVersion.ppDropDownList.SelectedIndex = 0
            Else
                Me.ddlRstVersion.ppDropDownList.SelectedValue = CType(rowData.FindControl("ＶＥＲ"), TextBox).Text
            End If
            'SERUPDP001-002 END

            If ddlRstAppaModel.Items.FindByValue(CType(rowData.FindControl("型式／機器"), TextBox).Text) Is Nothing Then
                Throw New Exception("工事機器マスタのマスタ登録データ「型式／機器」の整合性が取れません。\nシステム管理者に問い合わせてください。")
            Else
                Me.ddlRstAppaModel.SelectedValue = CType(rowData.FindControl("型式／機器"), TextBox).Text
            End If

            If ddlRstHddNo.Items.FindByValue(CType(rowData.FindControl("HDD No."), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("HDD No."), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstHddNo.SelectedValue = CType(rowData.FindControl("HDD No."), TextBox).Text
            End If

            If ddlRstHddCls.Items.FindByValue(CType(rowData.FindControl("HDD種別"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("HDD種別"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstHddCls.SelectedValue = CType(rowData.FindControl("HDD種別"), TextBox).Text
            End If

            Me.txtRstSerialNo.ppText = CType(rowData.FindControl("シリアル番号"), TextBox).Text

            If ddlRstPlaceCls.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("場所区分"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("場所区分"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstPlaceCls.ppDropDownList.SelectedValue = CType(rowData.FindControl("場所区分"), TextBox).Text
            End If
            Me.txtRstStrageCd.ppText = CType(rowData.FindControl("現設置／保管コード"), TextBox).Text
            Me.lblRstStrageNm.Text = CType(rowData.FindControl("現設置／保管場所"), TextBox).Text
            Me.dtbRstMoveDt.ppText = CType(rowData.FindControl("移動日"), TextBox).Text
            Me.dtbRstDlvPlndt.ppText = CType(rowData.FindControl("納入予定日"), TextBox).Text
            Me.dtbRstDlvDt.ppText = CType(rowData.FindControl("納入日"), TextBox).Text

            If ddlRstMoveCls.ppDropDownList.Items.FindByValue(CType(rowData.FindControl("移動先区分"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("移動先区分"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstMoveCls.ppDropDownList.SelectedValue = CType(rowData.FindControl("移動先区分"), TextBox).Text
            End If
            Me.txtRstMoveCd.ppText = CType(rowData.FindControl("移動先コード"), TextBox).Text
            Me.lblRstMoveNm.Text = CType(rowData.FindControl("移動先"), TextBox).Text

            If ddlRstMoveReason.Items.FindByValue(CType(rowData.FindControl("移動理由コード"), TextBox).Text) Is Nothing Then
                Throw New Exception(CType(rowData.FindControl("移動理由コード"), TextBox).Text & "が選択項目に存在しません")
            Else
                Me.ddlRstMoveReason.SelectedValue = CType(rowData.FindControl("移動理由コード"), TextBox).Text
            End If
            Me.txtRstCntlNo.ppText = CType(rowData.FindControl("管理番号"), TextBox).Text
            Me.txtRstArclNo.ppText = CType(rowData.FindControl("物品転送Ｎｏ．"), TextBox).Text
            Me.txtRstNotetext.ppText = CType(rowData.FindControl("備考"), TextBox).Text
            Me.txtRstNotetext.ppText = CType(rowData.FindControl("備考"), TextBox).Text
            Me.lblUpdateDt.Text = CType(rowData.FindControl("更新日時"), TextBox).Text
            'SERUPDP001-002
            If ddlCondNm.Items.FindByValue(CType(rowData.FindControl("機器備考"), TextBox).Text) Is Nothing Then

                ddlCondNm.Items.Insert(0, New ListItem(CType(rowData.FindControl("機器備考"), TextBox).Text, CType(rowData.FindControl("機器備考"), TextBox).Text)) '先頭に空白行を追加
                ddlCondNm.SelectedIndex = 0
            Else
                ''テスト
                'ddlCondNm.Items.Insert(0, New ListItem("テスト", "テスト")) '先頭に空白行を追加
                'Me.ddlCondNm.SelectedValue = "テスト"
                ''テストここまで

                Me.ddlCondNm.SelectedValue = CType(rowData.FindControl("機器備考"), TextBox).Text
            End If
            'Me.txtCondNm.ppText = CType(rowData.FindControl("機器備考"), TextBox).Text

            Me.lblSeq.Text = CType(rowData.FindControl("連番"), TextBox).Text
            'SERUPDP001-002 END

            'SERUPDP001-002
            strMode = "Select"
            ''シリアル管理キー項目の非活性
            'Me.txtRstSerialNo.ppEnabled = False
            'Me.ddlRstAppaDiv.Enabled = False
            'Me.ddlRstAppaCls.Enabled = False
            ''Me.ddlRstSystem.Enabled = False     'SERUPDP001-002 コメントアウト
            ''Me.ddlRstAppaModel.Enabled = False  'SERUPDP001-002 コメントアウト

            ''---- 追加 2014/06/10 ----S
            ''機器種別がHDD以外はHDD No、HDD種別は非活性
            'If mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) <> "09" Then
            '    Me.ddlRstHddCls.Enabled = False
            '    Me.ddlRstHddNo.Enabled = False
            '    Me.ddlRstHddCls.SelectedIndex = 0
            '    Me.ddlRstHddNo.SelectedIndex = 0
            'Else
            '    Me.ddlRstHddCls.Enabled = True
            '    Me.ddlRstHddNo.Enabled = True
            'End If
            ''---- 追加 2014/06/10 ----E

            ''ボタン活性・非活性
            'Me.btnDetailInsert.Enabled = False
            'Me.btnDetailUpdate.Enabled = True
            'Me.btnDetailDelete.Enabled = True
            'SERUPDP001-002 END

            SetFocus(ddlRstSystem.ClientID)
        Catch ex As Exception
            msClearDetail()
            Me.ddlRstAppaCls.SelectedIndex = 0 'msClearDerail()では機器種別の選択項目リセットが実施されない為。
            psMesBox(Me, "00000", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, ex.Message)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
        'SERUPDP001-001 END
    End Sub

    ''' <summary>
    ''' 機器分類マスタドロップダウンリスト（検索条件）変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlCndAppaDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCndAppaDiv.SelectedIndexChanged

        Dim tmp As String = ddlCndAppaCls.SelectedValue

        'ドロップダウンリスト設定
        msSetddlAppacls(Me.ddlCndAppaCls, Me.ddlCndAppaDiv.SelectedValue)   '機器種別

        If ddlCndAppaCls.Items.FindByValue(tmp) Is Nothing Then
        Else
            Me.ddlCndAppaCls.SelectedValue = tmp
        End If

    End Sub

    ''' <summary>
    ''' 機器種別マスタドロップダウンリスト（検索条件）変更時処理　  ---- 追加 2014/06/11 ----
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlCndAppaCls_SelectedIndexChanged(sender As Object, e As EventArgs) 'Handles ddlCndAppaCls.SelectedIndexChanged

        'ドロップダウンリスト設定
        msSetddlSystem(1)   'システム

    End Sub

    ''' <summary>
    ''' 機器分類ドロップダウンリスト（明細）変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlRstAppaDiv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRstAppaDiv.SelectedIndexChanged
        'SERUPDP001-002
        If ddlRstAppaDiv.SelectedValue <> String.Empty Then
            'ドロップダウンリスト設定
            '--機器種別
            msSetddlAppacls(Me.ddlRstAppaCls, Me.ddlRstAppaDiv.SelectedValue)
            '----- 追加 2014/06/11 -----S
            'システム
            msSetddlSystem(2)
            '型式機器
            Me.ddlRstAppaModel.Enabled = False  '非活性
            Me.ddlRstAppaModel.Items.Clear()  '初期化
            '----- 追加 2014/06/11 -----E

            '----- 削除 2014/06/11 -----S
            '--型式機器
            'If Me.ddlRstAppaDiv.SelectedValue <> String.Empty AndAlso
            '   Me.ddlRstAppaCls.SelectedValue <> String.Empty AndAlso
            '   Me.ddlRstSystem.SelectedValue <> String.Empty Then
            '   msSetddlAppamodel()
            'Else
            'Me.ddlRstAppaModel.Items.Clear()
            'Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))
            'End If
            '----- 削除 2014/06/11 -----S

            'If mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) <> "09" Then
            '    Me.ddlRstHddCls.Enabled = False
            '    Me.ddlRstHddNo.Enabled = False
            '    Me.ddlRstHddCls.SelectedIndex = 0
            '    Me.ddlRstHddNo.SelectedIndex = 0
            'Else
            '    Me.ddlRstHddCls.Enabled = True
            '    Me.ddlRstHddNo.Enabled = True
            'End If
            SetFocus(Me.ddlRstAppaCls.ClientID)
        Else

            Me.ddlRstAppaCls.Items.Clear()  '初期化
            Me.ddlRstAppaCls.Items.Insert(0, New ListItem(Nothing, Nothing))
        End If
        'SERUPDP001-002 END
    End Sub

    ''' <summary>
    ''' 機器種別ドロップダウンリスト（明細）変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlRstAppaCls_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRstAppaCls.SelectedIndexChanged
        'SERUPDP001-002
        If ddlRstAppaCls.SelectedValue <> String.Empty Then
            'ドロップダウンリスト設定
            '----- 追加 2014/06/11 -----S
            'システム
            msSetddlSystem(2)
            '----- 追加 2014/06/11 -----E
            '----- 変更 2014/06/11 -----S
            '型式機器
            'If Me.ddlRstAppaDiv.SelectedValue <> String.Empty AndAlso
            '   Me.ddlRstAppaCls.SelectedValue <> String.Empty AndAlso
            '   Me.ddlRstSystem.SelectedValue <> String.Empty Then
            'msSetddlAppamodel()
            'Else
            '    Me.ddlRstAppaModel.Items.Clear()
            '    Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))
            'End If
            '----- 変更 2014/06/11 -----E

            '----- 追加 2014/06/10 -----S
            'HDD以外を選択した場合は、HDD No、HDD種別は非活性
            'If mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) <> "09" Then
            '    Me.ddlRstHddCls.Enabled = False
            '    Me.ddlRstHddNo.Enabled = False
            '    Me.ddlRstHddCls.SelectedIndex = 0
            '    Me.ddlRstHddNo.SelectedIndex = 0
            'Else
            '    Me.ddlRstHddCls.Enabled = True
            '    Me.ddlRstHddNo.Enabled = True
            'End If
            '----- 追加 2014/06/10 -----E
            If txtRstSerialNo.ppText <> String.Empty Then
                lblSeq.Text = (mfGetMaxSeq(mfGetExistData) + 1).ToString    '連番を表示
                Select Case lblSeq.Text
                    Case "0" '連番取得時エラー
                        psMesBox(Me, "30008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)
                        lblSeq.Text = String.Empty
                        Exit Sub
                    Case "1000" '連番MAX
                        psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "入力したシリアルの登録上限に達しています。")
                        lblSeq.Text = String.Empty
                        Exit Sub
                    Case Else
                        '正常
                End Select
                strMode = "Insert"
                SetFocus(Me.ddlRstSystem.ClientID)
            Else
                SetFocus(txtRstSerialNo.ppTextBox.ClientID)
            End If
        End If
    End Sub

    ''' <summary>
    ''' システムドロップダウンリスト（明細）変更時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlRstSystem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRstSystem.SelectedIndexChanged
        'ドロップダウンリスト設定
        '--型式機器
        '----- 変更 2014/06/11 -----S
        'If Me.ddlRstAppaDiv.SelectedValue <> String.Empty AndAlso
        '   Me.ddlRstAppaCls.SelectedValue <> String.Empty AndAlso
        '   Me.ddlRstSystem.SelectedValue <> String.Empty Then
        msSetddlAppamodel()
        'Else
        'Me.ddlRstAppaModel.Items.Clear()
        'Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))
        'End If
        '----- 変更 2014/06/11 -----E

        msSetddlCondNm()
        msSetddlVersion()

        SetFocus(ddlRstVersion.ppDropDownList.ClientID)

    End Sub

    ''' <summary>
    ''' 場所区分変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlRstPlaceCls_SelectedIndexChanged()

        '現設置／保管コード、現設置／保管名称クリア
        If String.IsNullOrEmpty(txtRstStrageCd.ppText) = False Then
            txtRstStrageCd_TextChanged()
            'Me.txtRstStrageCd.ppText = String.Empty
            'Me.lblRstStrageNm.Text = String.Empty
        Else
            Me.txtRstStrageCd.ppTextBox.Focus()
        End If

    End Sub

    ''' <summary>
    ''' 移動先区分変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlRstMoveCls_SelectedIndexChanged()

        If String.IsNullOrEmpty(txtRstMoveCd.ppText) = False Then
            txtRstMoveCd_TextChanged()
            '移動先コード、移動先名クリア
            '            Me.txtRstMoveCd.ppText = String.Empty
            '            Me.lblRstMoveNm.Text = String.Empty
        Else
            Me.txtRstMoveCd.ppTextBox.Focus()
        End If

    End Sub

    ''' <summary>
    ''' 現設置／保管コード変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtRstStrageCd_TextChanged()

        Dim strbuff As String = String.Empty    '現設置／保管名称

        If Me.txtRstStrageCd.ppText <> String.Empty Then
            Select Case Me.ddlRstPlaceCls.ppSelectedValue
                Case 1  'ホール名取得
                    If mfGetHallInfo(Me.txtRstStrageCd.ppText, strbuff) Then
                        Me.lblRstStrageNm.Text = strbuff.Trim
                        Me.dtbRstMoveDt.ppDateBox.Focus()
                    Else
                        Me.lblRstStrageNm.Text = String.Empty
                        Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Me.txtRstStrageCd.ppTextBox.Focus()
                    End If
                Case 2  '営業所名、または保守拠点名取得
                    If mfGetOfficeInfo(M_TRD_EIGYO + "," + M_TRD_HOSHU, Me.txtRstStrageCd.ppText, strbuff) Then
                        Me.lblRstStrageNm.Text = strbuff.Trim
                        Me.dtbRstMoveDt.ppDateBox.Focus()
                    Else
                        Me.lblRstStrageNm.Text = String.Empty
                        Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstStrageCd.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(M_TRD_SONOTA, Me.txtRstStrageCd.ppText, strbuff) Then
                        Me.lblRstStrageNm.Text = strbuff.Trim
                        Me.dtbRstMoveDt.ppDateBox.Focus()
                    Else
                        Me.lblRstStrageNm.Text = String.Empty
                        Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstStrageCd.ppTextBox.Focus()
                    End If
                Case 4  '代理店名取得
                    If mfGetOfficeInfo(M_TRD_DAIRI, Me.txtRstStrageCd.ppText, strbuff) Then
                        Me.lblRstStrageNm.Text = strbuff.Trim
                        Me.dtbRstMoveDt.ppDateBox.Focus()
                    Else
                        Me.lblRstStrageNm.Text = String.Empty
                        Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstStrageCd.ppTextBox.Focus()
                    End If
                Case 5  'メーカ名取得
                    If mfGetOfficeInfo(M_TRD_MAKER, Me.txtRstStrageCd.ppText, strbuff) Then
                        Me.lblRstStrageNm.Text = strbuff.Trim
                        Me.dtbRstMoveDt.ppDateBox.Focus()
                    Else
                        Me.lblRstStrageNm.Text = String.Empty
                        Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstStrageCd.ppTextBox.Focus()
                    End If
                Case Else
                    lblRstStrageNm.Text = String.Empty
            End Select
        Else
            lblRstStrageNm.Text = String.Empty
        End If

    End Sub

    ''' <summary>
    ''' 移動先コード変更時処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtRstMoveCd_TextChanged()

        Dim strbuff As String = String.Empty    '現設置／保管名称

        If Me.txtRstMoveCd.ppText <> String.Empty Then
            Select Case Me.ddlRstMoveCls.ppSelectedValue
                Case 1  'ホール名取得
                    If mfGetHallInfo(Me.txtRstMoveCd.ppText, strbuff) Then
                        Me.lblRstMoveNm.Text = strbuff
                        Me.dtbRstDlvDt.ppDateBox.Focus()
                    Else
                        Me.lblRstMoveNm.Text = String.Empty
                        Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                        Me.txtRstMoveCd.ppTextBox.Focus()
                    End If
                Case 2  '営業所名取得
                    If mfGetOfficeInfo(M_TRD_EIGYO + "," + M_TRD_HOSHU, Me.txtRstMoveCd.ppText, strbuff) Then
                        Me.lblRstMoveNm.Text = strbuff
                        Me.dtbRstDlvDt.ppDateBox.Focus()
                    Else
                        Me.lblRstMoveNm.Text = String.Empty
                        Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstMoveCd.ppTextBox.Focus()
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(M_TRD_SONOTA, Me.txtRstMoveCd.ppText, strbuff) Then
                        Me.lblRstMoveNm.Text = strbuff
                        Me.dtbRstDlvDt.ppDateBox.Focus()
                    Else
                        Me.lblRstMoveNm.Text = String.Empty
                        Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstMoveCd.ppTextBox.Focus()
                    End If
                Case 4  '代理店名取得
                    If mfGetOfficeInfo(M_TRD_DAIRI, Me.txtRstMoveCd.ppText, strbuff) Then
                        Me.lblRstMoveNm.Text = strbuff
                        Me.dtbRstDlvDt.ppDateBox.Focus()
                    Else
                        Me.lblRstMoveNm.Text = String.Empty
                        Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstMoveCd.ppTextBox.Focus()
                    End If
                Case 5  'メーカ名取得
                    If mfGetOfficeInfo(M_TRD_MAKER, Me.txtRstMoveCd.ppText, strbuff) Then
                        Me.lblRstMoveNm.Text = strbuff
                        Me.dtbRstDlvDt.ppDateBox.Focus()
                    Else
                        Me.lblRstMoveNm.Text = String.Empty
                        Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                        Me.txtRstMoveCd.ppTextBox.Focus()
                    End If
                Case Else
                    lblRstMoveNm.Text = String.Empty
            End Select
        Else
            lblRstMoveNm.Text = String.Empty
        End If

    End Sub

    'SERUPDP001-002
    ''' <summary>
    ''' 移動先区分変更時(検索欄)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ddlCndPlaceCls_SelectedIndexChanged()

        If txtCndStrageCd.ppText = String.Empty Then
            txtCndStrageCd.ppTextBox.Focus()
        Else
            txtCndStrageCd_TextChanged()
            ddlCndAppaDiv.Focus()
        End If
    End Sub

    ''' <summary>
    ''' 移動先コード変更時(検索欄)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtCndStrageCd_TextChanged()

        Dim strbuff As String = String.Empty    '現設置／保管名称

        If Me.txtCndStrageCd.ppText <> String.Empty Then
            Select Case Me.ddlCndPlaceCls.ppSelectedValue
                Case 1  'ホール名取得
                    If mfGetHallInfo(Me.txtCndStrageCd.ppText, strbuff) Then
                        Me.lblPlaceNm.Text = strbuff
                    Else
                        Me.lblPlaceNm.Text = String.Empty
                    End If
                Case 2  '営業所名取得
                    If mfGetOfficeInfo(M_TRD_EIGYO + "," + M_TRD_HOSHU, Me.txtCndStrageCd.ppText, strbuff) Then
                        Me.lblPlaceNm.Text = strbuff
                    Else
                        Me.lblPlaceNm.Text = String.Empty
                    End If
                Case 3  '倉庫名取得
                    If mfGetOfficeInfo(M_TRD_SONOTA, Me.txtCndStrageCd.ppText, strbuff) Then
                        Me.lblPlaceNm.Text = strbuff
                    Else
                        Me.lblPlaceNm.Text = String.Empty
                    End If
                Case 4  '代理店名取得
                    If mfGetOfficeInfo(M_TRD_DAIRI, Me.txtCndStrageCd.ppText, strbuff) Then
                        Me.lblPlaceNm.Text = strbuff
                    Else
                        Me.lblPlaceNm.Text = String.Empty
                    End If
                Case 5  'メーカ名取得
                    If mfGetOfficeInfo(M_TRD_MAKER, Me.txtCndStrageCd.ppText, strbuff) Then
                        Me.lblPlaceNm.Text = strbuff
                    Else
                        Me.lblPlaceNm.Text = String.Empty
                    End If
                Case Else
                    Me.lblPlaceNm.Text = String.Empty
            End Select
        Else
            Me.lblPlaceNm.Text = String.Empty
        End If
        ddlCndAppaDiv.Focus()
    End Sub

    ''' <summary>
    ''' シリアル番号入力時　既存データ取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub txtRstSerialNo_TextChanged()
        If txtRstSerialNo.ppText.Trim <> String.Empty Then
            If ddlRstAppaDiv.SelectedValue <> String.Empty AndAlso ddlRstAppaCls.SelectedValue <> String.Empty Then
                lblSeq.Text = (mfGetMaxSeq(mfGetExistData) + 1).ToString    '連番を表示
                Select Case lblSeq.Text
                    Case "0" '連番取得時エラー
                        psMesBox(Me, "30008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)
                        lblSeq.Text = String.Empty  '連番の表示を初期化して終了
                        Exit Sub
                    Case "1000" '連番MAX
                        psMesBox(Me, "30023", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, "入力したシリアルの登録上限に達しています。")
                        lblSeq.Text = String.Empty  '連番の表示を初期化して終了
                        Exit Sub
                    Case Else
                        '正常
                End Select
                strMode = "Insert"
                SetFocus(Me.ddlRstSystem.ClientID)
            Else
                If ddlRstAppaDiv.SelectedValue <> String.Empty Then
                    SetFocus(Me.ddlRstAppaCls.ClientID)
                Else
                    SetFocus(Me.ddlRstAppaDiv.ClientID)
                End If
            End If
        End If
    End Sub
    'SERUPDP001-002

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' 画面クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearScreen()

        objStack = New StackFrame

        Try
            '検索条件クリア
            msClearSearchCondition()

            '明細クリア
            msClearDetail()

            'グリッド、該当件数
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            'シリアル番号へフォーカス
            Me.txtCndSerialNo.ppTextBox.Focus()

            strMode = "Default"     'SERUPDP001-002

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 検索条件クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearSearchCondition()

        Me.txtCndSerialNo.ppText = String.Empty
        Me.ddlCndPlaceCls.ppDropDownList.SelectedIndex = 0
        Me.txtCndStrageCd.ppText = String.Empty
        Me.ddlCndAppaDiv.SelectedIndex = 0
        msSetddlAppacls(Me.ddlCndAppaCls, Me.ddlCndAppaDiv.SelectedValue)
        msSetddlSystem(1)                   '---- 変更 2014/06/11 ----
        Me.dtbCndMoveDt.ppText = String.Empty
        Me.ddlCndMoveReason.SelectedIndex = 0
        Me.txtCndCntlNo.ppText = String.Empty
        Me.dtbCndDlvPlndt.ppText = String.Empty
        Me.dtbCndDlvDt.ppText = String.Empty
        'SERUPDP001-002
        Me.lblPlaceNm.Text = String.Empty
        Me.ddldel.ppSelectedValue = "0"
        'SERUPDP001-002 END
    End Sub

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <param name="intProc">0:前回の条件で再検索 1:通常検索</param>    'SERUPDP001-003 追加
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetData(ByVal intProc As Integer) As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strKey(11) As String

        objStack = New StackFrame

        mfGetData = Nothing

        '検索条件設定 'SERUPDP001-003
        Select Case intProc
            Case 0
                '前回の検索条件復元
                If ViewState(VS_SQLPRM) Is Nothing Then
                    strKey(0) = Me.txtCndSerialNo.ppText            'シリアル番号
                    strKey(1) = Me.ddlCndPlaceCls.ppSelectedValue   '場所区分   
                    strKey(2) = Me.txtCndStrageCd.ppText            '場所（現設置/保管コード）
                    strKey(3) = Me.ddlCndAppaDiv.SelectedValue      '機器分類
                    strKey(4) = Me.ddlCndAppaCls.SelectedValue      '機器種別 
                    strKey(5) = Me.ddlCndSystem.SelectedValue       'システムコード
                    strKey(6) = Me.dtbCndMoveDt.ppText              '移動日(yyyy/MM/dd)
                    strKey(7) = Me.ddlCndMoveReason.SelectedValue   '移動理由コード
                    strKey(8) = Me.txtCndCntlNo.ppText              '管理番号
                    strKey(9) = Me.dtbCndDlvPlndt.ppText            '納入予定日(yyyy/MM/dd)
                    strKey(10) = Me.dtbCndDlvDt.ppText              '納入日(yyyy/MM/dd)
                    strKey(11) = Me.ddldel.ppSelectedValue

                    '検索条件保存
                    ViewState(VS_SQLPRM) = strKey
                Else
                    strKey = ViewState(VS_SQLPRM)
                End If
            Case 1
                strKey(0) = Me.txtCndSerialNo.ppText            'シリアル番号
                strKey(1) = Me.ddlCndPlaceCls.ppSelectedValue   '場所区分   
                strKey(2) = Me.txtCndStrageCd.ppText            '場所（現設置/保管コード）
                strKey(3) = Me.ddlCndAppaDiv.SelectedValue      '機器分類
                strKey(4) = Me.ddlCndAppaCls.SelectedValue      '機器種別 
                strKey(5) = Me.ddlCndSystem.SelectedValue       'システムコード
                strKey(6) = Me.dtbCndMoveDt.ppText              '移動日(yyyy/MM/dd)
                strKey(7) = Me.ddlCndMoveReason.SelectedValue   '移動理由コード
                strKey(8) = Me.txtCndCntlNo.ppText              '管理番号
                strKey(9) = Me.dtbCndDlvPlndt.ppText            '納入予定日(yyyy/MM/dd)
                strKey(10) = Me.dtbCndDlvDt.ppText              '納入日(yyyy/MM/dd)
                strKey(11) = Me.ddldel.ppSelectedValue

                '検索条件保存
                ViewState(VS_SQLPRM) = strKey
        End Select

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'シリアル番号
                    .Add(pfSet_Param("serialno", SqlDbType.NVarChar,
                                     strKey(0).Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '場所区分   
                    .Add(pfSet_Param("placeclass", SqlDbType.NVarChar, strKey(1)))

                    '場所（現設置/保管コード）
                    .Add(pfSet_Param("stragecd", SqlDbType.NVarChar,
                                     strKey(2).Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '機器分類
                    If strKey(3) = String.Empty AndAlso strKey(4) <> String.Empty Then
                        .Add(pfSet_Param("appadivnm", SqlDbType.NVarChar, mfGETCodePart(strKey(4))))
                    Else
                        .Add(pfSet_Param("appadivnm", SqlDbType.NVarChar, strKey(3)))
                    End If

                    '機器種別
                    .Add(pfSet_Param("appaclsnm", SqlDbType.NVarChar, mfGetNamePart(strKey(4))))

                    'システムコード
                    .Add(pfSet_Param("systemcd", SqlDbType.NVarChar, strKey(5)))
                    '移動日(yyyy/MM/dd)
                    .Add(pfSet_Param("setdt", SqlDbType.NVarChar, strKey(6)))

                    '移動理由コード
                    .Add(pfSet_Param("mvrsncd", SqlDbType.NVarChar, strKey(7)))

                    '管理番号
                    .Add(pfSet_Param("serialcntlno", SqlDbType.NVarChar,
                                     strKey(8).Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))

                    '納入予定日(yyyy/MM/dd)
                    .Add(pfSet_Param("dlvplndt", SqlDbType.NVarChar, strKey(9)))

                    '納入日(yyyy/MM/dd)
                    .Add(pfSet_Param("dlvdt", SqlDbType.NVarChar, strKey(10)))

                    'SERUPDP001-002
                    .Add(pfSet_Param("del_flg", SqlDbType.NVarChar, strKey(11)))
                    'SERUPDP001-002 END

                End With

                'データ取得
                mfGetData = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' データ表示処理
    ''' </summary>
    ''' <param name="objDs"></param>
    ''' <remarks></remarks>
    Private Sub msDispData(objDs As DataSet)

        objStack = New StackFrame

        Try
            'グリッド及び件数の初期化
            Me.grvList.DataSource = New DataTable
            Me.grvList.DataBind()
            Master.ppCount = "0"

            '件数を設定
            If objDs.Tables(0).Rows.Count = 0 Then
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Master.ppCount = "0"
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
            psMesBox(Me, "30010", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後,
                     ex.ToString.Replace(Environment.NewLine, "").Replace("'", ""))

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' 明細クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClearDetail()

        objStack = New StackFrame

        Try
            Me.ddlRstAppaDiv.SelectedIndex = 0
            msSetddlAppacls(Me.ddlRstAppaCls, Me.ddlRstAppaDiv.SelectedValue)
            msSetddlSystem(3)                   '---- 変更 2014/06/11 ----
            ddlRstAppaCls.Items.Clear()
            Me.ddlRstAppaCls.Items.Insert(0, New ListItem(Nothing, Nothing))
            'SERUPDP001-002
            Me.ddlRstVersion.ppSelectedValue = String.Empty
            'Me.txtRstVersion.ppText = String.Empty
            'SERUPDP001-002 END
            Me.ddlRstAppaModel.Items.Clear()
            Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))
            Me.ddlRstAppaModel.SelectedIndex = 0
            Me.ddlRstSystem.SelectedIndex = 0
            Me.ddlRstHddNo.SelectedIndex = 0
            Me.ddlRstHddCls.SelectedIndex = 0
            Me.txtRstSerialNo.ppText = String.Empty
            Me.ddlRstPlaceCls.ppDropDownList.SelectedIndex = 0
            Me.txtRstStrageCd.ppText = String.Empty
            Me.lblRstStrageNm.Text = String.Empty
            Me.dtbRstMoveDt.ppText = String.Empty
            Me.dtbRstDlvPlndt.ppText = String.Empty
            Me.dtbRstDlvDt.ppText = String.Empty
            Me.ddlRstMoveCls.ppDropDownList.SelectedIndex = 0
            Me.txtRstMoveCd.ppText = String.Empty
            Me.lblRstMoveNm.Text = String.Empty
            Me.ddlRstMoveReason.SelectedIndex = 0
            Me.txtRstCntlNo.ppText = String.Empty
            Me.txtRstArclNo.ppText = String.Empty
            Me.txtRstNotetext.ppText = String.Empty
            Me.lblUpdateDt.Text = String.Empty
            'SERUPDP001-002
            msSetddlCondNm()
            Me.ddlCondNm.SelectedValue = String.Empty
            'Me.txtCondNm.ppText = String.Empty
            Me.lblSeq.Text = String.Empty   '連番表示ラベル
            'SERUPDP001-002 END
            '---- 追加 2014/06/10 ----S
            Me.ddlRstHddNo.Enabled = True
            Me.ddlRstHddCls.Enabled = True
            '---- 追加 2014/06/10 ----E

            'シリアル管理キー項目活性
            Me.txtRstSerialNo.ppEnabled = True
            Me.ddlRstAppaDiv.Enabled = True
            Me.ddlRstAppaCls.Enabled = True
            Me.ddlRstSystem.Enabled = True
            Me.ddlRstAppaModel.Enabled = False   '---　変更 2014/06/11 ---

            'ボタン活性
            Me.btnDetailClear.Enabled = True    'クリアボタン
            Me.btnDetailInsert.Enabled = True   '追加ボタン
            Me.btnDetailUpdate.Enabled = False  '更新ボタン
            Me.btnDetailDelete.Enabled = False  '削除ボタン

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器分類マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppadiv(ByVal ItemType As Integer)  '--- 変更 2014/06/11 ----

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL012", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '(1)検索条件
                '----- 変更 2014/06/11 -----S
                If ItemType = 0 Or ItemType = 1 Then
                    Me.ddlCndAppaDiv.Items.Clear()
                    Me.ddlCndAppaDiv.DataSource = objDs.Tables(0)
                    Me.ddlCndAppaDiv.DataTextField = "名称"
                    Me.ddlCndAppaDiv.DataValueField = "機器分類コード"
                    Me.ddlCndAppaDiv.DataBind()
                    Me.ddlCndAppaDiv.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                End If
                If ItemType = 0 Or ItemType = 2 Then
                    '(2)検索結果（明細）
                    Me.ddlRstAppaDiv.Items.Clear()
                    Me.ddlRstAppaDiv.DataSource = objDs.Tables(0)
                    Me.ddlRstAppaDiv.DataTextField = "名称"
                    Me.ddlRstAppaDiv.DataValueField = "機器分類コード"
                    Me.ddlRstAppaDiv.DataBind()
                    Me.ddlRstAppaDiv.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                End If
                '----- 変更 2014/06/11 -----E

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類マスタ一覧取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器種別マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppacls(ByRef pddlLst As DropDownList, ByVal pAppaDiv As String)

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                'objCmd = New SqlCommand("ZCMPSEL013", objCn)
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S7", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appadivcd", SqlDbType.NVarChar, pAppaDiv))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                pddlLst.Items.Clear()
                pddlLst.DataSource = objDs.Tables(0)
                pddlLst.DataTextField = "機器種別名"
                pddlLst.DataValueField = "機器種別コード"
                pddlLst.DataBind()
                pddlLst.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器種別マスタ一覧取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（機器マスタ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlAppamodel()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("SERUPDP001_S4", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '機器分類コード
                    .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    '機器種別コード
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)))
                    'システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlRstSystem.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '----- 変更 2014/06/11 -----S
                If objDs.Tables(0).Rows.Count = 0 Then
                    Me.ddlRstAppaModel.Items.Clear()    'データが無い場合は初期化
                    Me.ddlRstAppaModel.Enabled = False  'データが無い場合は非活性
                Else
                    'ドロップダウンリスト設定
                    Me.ddlRstAppaModel.Items.Clear()
                    Me.ddlRstAppaModel.DataSource = objDs.Tables(0)
                    Me.ddlRstAppaModel.DataTextField = "型式機器"
                    Me.ddlRstAppaModel.DataValueField = "型式機器"
                    Me.ddlRstAppaModel.DataBind()
                    Me.ddlRstAppaModel.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加
                    Me.ddlRstAppaModel.Enabled = True
                End If
                '----- 変更 2014/06/11 -----E

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器マスタ一覧取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（システム）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlSystem(ByVal ItemType As Integer)       '---- 変更 2014/06/11 ----

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                '----- 変更 2014/06/11 -----S
                objCmd = New SqlCommand("SERUPDP001_S5", objCn)
                '----- 変更 2014/06/11 -----E

                'パラメータ設定
                With objCmd.Parameters
                    '--パラメータ設定
                    '＜初期表示＞
                    If ItemType = 0 Or ItemType = 3 Then
                        '機器分類コード
                        .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, String.Empty))
                        '機器種別コード
                        .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, String.Empty))
                    End If
                    '＜検索条件＞
                    If ItemType = 1 Then
                        '機器分類コード
                        .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlCndAppaDiv.SelectedValue))
                        '機器種別コード
                        .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, mfGetNamePart(Me.ddlCndAppaCls.SelectedValue)))
                    End If
                    '＜検索結果（明細）＞
                    If ItemType = 2 Then
                        '機器分類コード
                        .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                        '機器種別コード
                        .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)))
                    End If
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                '----- 変更 2014/06/11 -----S
                If objDs.Tables(0).Rows.Count = 0 Then

                    If ItemType = 0 Or ItemType = 1 Then
                        '＜検索条件＞
                        Me.ddlCndSystem.Items.Clear()
                        Me.ddlCndSystem.Enabled = False
                    End If
                    If ItemType = 0 Or ItemType = 2 Then
                        '＜検索結果（明細）＞
                        Me.ddlRstSystem.Items.Clear()
                        Me.ddlRstSystem.Enabled = False
                    End If

                Else
                    'ドロップダウンリスト設定
                    If ItemType = 0 Or ItemType = 1 Then
                        '(1)検索条件
                        Me.ddlCndSystem.Items.Clear()
                        Me.ddlCndSystem.DataSource = objDs.Tables(0)
                        Me.ddlCndSystem.DataTextField = "システム"
                        Me.ddlCndSystem.DataValueField = "システムコード"
                        Me.ddlCndSystem.DataBind()
                        Me.ddlCndSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        Me.ddlCndSystem.Enabled = True
                    End If
                    If ItemType = 0 Or ItemType = 2 Or ItemType = 3 Then
                        '(2)検索結果（明細）
                        Me.ddlRstSystem.Items.Clear()
                        Me.ddlRstSystem.DataSource = objDs.Tables(0)
                        Me.ddlRstSystem.DataTextField = "システム"
                        Me.ddlRstSystem.DataValueField = "システムコード"
                        Me.ddlRstSystem.DataBind()
                        Me.ddlRstSystem.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                        Me.ddlRstSystem.Enabled = True
                    End If
                End If
                '----- 変更 2014/06/11 -----E

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸ機種マスタ一覧取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（移動理由）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlMoveReason()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL030", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ドロップダウンリスト設定
                '(1)検索条件
                Me.ddlCndMoveReason.Items.Clear()
                Me.ddlCndMoveReason.DataSource = objDs.Tables(0)
                Me.ddlCndMoveReason.DataTextField = "移動理由"
                Me.ddlCndMoveReason.DataValueField = "移動理由コード"
                Me.ddlCndMoveReason.DataBind()
                Me.ddlCndMoveReason.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '(2)検索結果（明細）
                Me.ddlRstMoveReason.Items.Clear()
                Me.ddlRstMoveReason.DataSource = objDs.Tables(0)
                Me.ddlRstMoveReason.DataTextField = "移動理由"
                Me.ddlRstMoveReason.DataValueField = "移動理由コード"
                Me.ddlRstMoveReason.DataBind()
                Me.ddlRstMoveReason.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "移動理由マスタ一覧取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（ＨＤＤ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlHDD()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL033", objCn)

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'ＨＤＤＮｏでDistinctする。（重複データをカット）
                Dim dvHDDNo As DataView = objDs.Tables(0).DefaultView
                Dim dtHDDNo As DataTable = dvHDDNo.ToTable("ＨＤＤ", True, "ＨＤＤＮｏ")

                'ＨＤＤ種別でDistinctする。（重複データをカット）
                Dim dvHDDCls As DataView = objDs.Tables(0).DefaultView
                Dim dtHDDCls As DataTable = dvHDDNo.ToTable("ＨＤＤ", True, "ＨＤＤ種別")

                'ドロップダウンリスト設定
                '(1)HDDNo
                Me.ddlRstHddNo.Items.Clear()
                Me.ddlRstHddNo.DataSource = dtHDDNo
                Me.ddlRstHddNo.DataTextField = "ＨＤＤＮｏ"
                Me.ddlRstHddNo.DataValueField = "ＨＤＤＮｏ"
                Me.ddlRstHddNo.DataBind()
                Me.ddlRstHddNo.Items.Insert(0, New ListItem(Nothing, Nothing)) '先頭に空白行を追加
                '(2)HDD種別
                Me.ddlRstHddCls.Items.Clear()
                Me.ddlRstHddCls.DataSource = dtHDDCls
                Me.ddlRstHddCls.DataTextField = "ＨＤＤ種別"
                Me.ddlRstHddCls.DataValueField = "ＨＤＤ種別"
                Me.ddlRstHddCls.DataBind()
                '空白データがない場合は先頭に空白行を追加
                If Me.ddlRstHddCls.Items.FindByValue(String.Empty) Is Nothing Then
                    Me.ddlRstHddCls.Items.Insert(0, New ListItem(Nothing, Nothing))
                End If
            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＨＤＤマスタ一覧取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Sub

    'SERUPDP001-002 
    ''' <summary>
    ''' ドロップダウンリスト設定（機器備考）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlCondNm()
        Dim objDs As DataSet = Nothing          'データセット

        Try
            objDs = mfGetCondNm()
            If Not objDs Is Nothing Then

                '空白だけのテーブルなら削除ＳＴＡＲＴ
                Dim intTmp(objDs.Tables(0).Rows.Count - 1) As Integer
                Dim blnDeleteRows As Boolean = True
                For Each dr As DataRow In objDs.Tables(0).Rows
                    If dr.Item(0).ToString.Trim <> String.Empty Then
                        blnDeleteRows = False
                        Exit For
                    End If
                Next
                If blnDeleteRows Then
                    objDs.Tables(0).Rows.Clear()
                End If
                '空白だけのテーブルなら削除ＥＮＤ

                'ドロップダウンリスト設定
                Me.ddlCondNm.Items.Clear()
                Me.ddlCondNm.DataSource = objDs.Tables(0)
                Me.ddlCondNm.DataTextField = "機器備考"
                Me.ddlCondNm.DataValueField = "機器備考"
                Me.ddlCondNm.DataBind()
                Me.ddlCondNm.Items.Insert(0, New ListItem(Nothing, Nothing))    '先頭に空白行を追加

            End If
        Catch ex As Exception
            psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器備考マスタ一覧取得")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        End Try

    End Sub

    ''' <summary>
    ''' ドロップダウンリスト設定（バージョン）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSetddlVersion()
        Dim dsTmp As DataSet

        ddlRstVersion.ppDropDownList.Items.Clear()

        If ddlRstSystem.SelectedValue = String.Empty Then
            Return
        End If

        dsTmp = mfGetVersion(ddlRstSystem.SelectedValue)
        If dsTmp Is Nothing Then
            Return
        End If

        Me.ddlRstVersion.ppDropDownList.DataSource = dsTmp.Tables(0)
        Me.ddlRstVersion.ppDropDownList.DataTextField = "正式バージョン"
        Me.ddlRstVersion.ppDropDownList.DataValueField = "正式バージョン"
        Me.ddlRstVersion.ppDropDownList.DataBind()
        Me.ddlRstVersion.ppDropDownList.Items.Insert(0, New ListItem(Nothing, Nothing))

    End Sub

    ''' <summary>
    ''' グリッド行データ削除
    ''' </summary>
    ''' <param name="drData"></param>
    ''' <remarks></remarks>
    Private Sub msDelGridRowData(ByRef drData As DataRow)

        drData.Item("機器分類") = mfGetNamePart(Me.ddlRstAppaDiv.SelectedItem.Text)
        drData.Item("機器種別") = mfGetNamePart(Me.ddlRstAppaCls.SelectedItem.Text)
        drData.Item("システム") = mfGetNamePart(Me.ddlRstSystem.SelectedItem.Text)

        'SERUPDP001-002
        drData.Item("ＶＥＲ") = Me.ddlRstVersion.ppSelectedValue
        'drData.Item("ＶＥＲ") = Me.txtRstVersion.ppText
        'SERUPDP001-002 END

        drData.Item("型式／機器") = Me.ddlRstAppaModel.SelectedItem.Text
        drData.Item("HDD No.") = Me.ddlRstHddNo.SelectedItem.Text
        drData.Item("HDD種別") = Me.ddlRstHddCls.SelectedItem.Text
        drData.Item("シリアル番号") = Me.txtRstSerialNo.ppText
        drData.Item("現設置／保管場所") = Me.lblRstStrageNm.Text
        drData.Item("移動日") = Me.dtbRstMoveDt.ppText
        drData.Item("納入予定日") = Me.dtbRstDlvPlndt.ppText
        drData.Item("納入日") = Me.dtbRstDlvDt.ppText
        drData.Item("移動先") = Me.lblRstMoveNm.Text
        drData.Item("移動理由") = mfGetNamePart(Me.ddlRstMoveReason.SelectedItem.Text)
        drData.Item("管理番号") = Me.txtRstCntlNo.ppText
        drData.Item("物品転送Ｎｏ．") = Me.txtRstArclNo.ppText
        drData.Item("備考") = Me.txtRstNotetext.ppText
        drData.Item("機器分類コード") = Me.ddlRstAppaDiv.SelectedValue
        drData.Item("機器種別コード") = mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)
        drData.Item("システムコード") = Me.ddlRstSystem.SelectedValue
        drData.Item("場所区分") = Me.ddlRstPlaceCls.ppDropDownList.SelectedValue
        drData.Item("現設置／保管コード") = Me.txtRstStrageCd.ppText
        drData.Item("移動先区分") = Me.ddlRstMoveCls.ppDropDownList.SelectedValue
        drData.Item("移動先コード") = Me.txtRstMoveCd.ppText
        drData.Item("移動理由コード") = Me.ddlRstMoveReason.SelectedValue
        'SERUPDP001-002
        drData.Item("機器備考") = Me.ddlCondNm.SelectedValue
        'drData.Item("機器備考") = Me.txtCondNm.ppText
        drData.Item("連番") = Me.lblSeq.Text
        drData.Item("削除区分") = "1"
        'SERUPDP001-002 END
        drData.Item("更新日時") = Me.lblUpdateDt.Text

    End Sub
    'SERUPDP001-002 END

    ''' <summary>
    ''' グリッド行データ設定
    ''' </summary>
    ''' <param name="drData"></param>
    ''' <remarks></remarks>
    Private Sub msSetGridRowData(ByRef drData As DataRow)

        drData.Item("機器分類") = mfGetNamePart(Me.ddlRstAppaDiv.SelectedItem.Text)
        drData.Item("機器種別") = mfGetNamePart(Me.ddlRstAppaCls.SelectedItem.Text)
        drData.Item("システム") = mfGetNamePart(Me.ddlRstSystem.SelectedItem.Text)

        'SERUPDP001-002
        drData.Item("ＶＥＲ") = Me.ddlRstVersion.ppSelectedValue
        'drData.Item("ＶＥＲ") = Me.txtRstVersion.ppText
        'SERUPDP001-002 END

        drData.Item("型式／機器") = Me.ddlRstAppaModel.SelectedItem.Text
        drData.Item("HDD No.") = Me.ddlRstHddNo.SelectedItem.Text
        drData.Item("HDD種別") = Me.ddlRstHddCls.SelectedItem.Text
        drData.Item("シリアル番号") = Me.txtRstSerialNo.ppText
        drData.Item("現設置／保管場所") = Me.lblRstStrageNm.Text
        drData.Item("移動日") = Me.dtbRstMoveDt.ppText
        drData.Item("納入予定日") = Me.dtbRstDlvPlndt.ppText
        drData.Item("納入日") = Me.dtbRstDlvDt.ppText
        drData.Item("移動先") = Me.lblRstMoveNm.Text
        drData.Item("移動理由") = mfGetNamePart(Me.ddlRstMoveReason.SelectedItem.Text)
        drData.Item("管理番号") = Me.txtRstCntlNo.ppText
        drData.Item("物品転送Ｎｏ．") = Me.txtRstArclNo.ppText
        drData.Item("備考") = Me.txtRstNotetext.ppText
        drData.Item("機器分類コード") = Me.ddlRstAppaDiv.SelectedValue
        drData.Item("機器種別コード") = mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)
        drData.Item("システムコード") = Me.ddlRstSystem.SelectedValue
        drData.Item("場所区分") = Me.ddlRstPlaceCls.ppDropDownList.SelectedValue
        drData.Item("現設置／保管コード") = Me.txtRstStrageCd.ppText
        drData.Item("移動先区分") = Me.ddlRstMoveCls.ppDropDownList.SelectedValue
        drData.Item("移動先コード") = Me.txtRstMoveCd.ppText
        drData.Item("移動理由コード") = Me.ddlRstMoveReason.SelectedValue
        'SERUPDP001-002
        drData.Item("機器備考") = Me.ddlCondNm.SelectedValue
        'drData.Item("機器備考") = Me.txtCondNm.ppText
        drData.Item("連番") = Me.lblSeq.Text
        drData.Item("削除区分") = "0"
        'SERUPDP001-002 END
        drData.Item("更新日時") = Me.lblUpdateDt.Text

    End Sub

    ''' <summary>
    ''' ホール情報取得処理
    ''' </summary>
    ''' <param name="strTboxid"></param>
    ''' <param name="strHallName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetHallInfo(ByVal strTboxid As String, ByRef strHallName As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfGetHallInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL028", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'ＴＢＯＸＩＤ
                    .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, strTboxid))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ホール名設定
                strHallName = dtRow("ホール名").ToString

                mfGetHallInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ホール情報取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 会社・営業所情報取得処理
    ''' </summary>
    ''' <param name="strTraderCd"></param>
    ''' <param name="strOfficeCd"></param>
    ''' <param name="strOfficeName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetOfficeInfo(ByVal strTraderCd As String, ByVal strOfficeCd As String, ByRef strOfficeName As String) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfGetOfficeInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL029", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    '業者コード
                    .Add(pfSet_Param("trader_cd", SqlDbType.NVarChar, strTraderCd))
                    '営業所コード
                    .Add(pfSet_Param("office_cd", SqlDbType.NVarChar, strOfficeCd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                '会社名または営業所名設定
                If dtRow("業者コード").ToString = M_TRD_EIGYO OrElse
                   dtRow("業者コード").ToString = M_TRD_HOSHU OrElse
                   dtRow("業者コード").ToString = M_TRD_DAIRI Then
                    strOfficeName = dtRow("営業所名").ToString
                Else
                    strOfficeName = dtRow("会社名").ToString
                End If

                mfGetOfficeInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "会社・営業所情報取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' ＴＢＯＸマスタ情報取得処理
    ''' </summary>
    ''' <param name="strTboxCd"></param>
    ''' <param name="strTboxVer"></param>
    ''' <param name="strTboxNm"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetTboxMasterInfo(ByVal strTboxCd As String, ByVal strTboxVer As String,
                                         ByRef strTboxNm As String) As Boolean
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfGetTboxMasterInfo = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S2", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'システムコード
                    .Add(pfSet_Param("tbox_cd", SqlDbType.NVarChar, strTboxCd))
                    'ＶＥＲ
                    .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, strTboxVer))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                dtRow = objDs.Tables(0).Rows(0)

                'ＴＢＯＸマスタ情報の設定
                strTboxNm = dtRow("ＴＢＯＸ機種名").ToString

                mfGetTboxMasterInfo = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＴＢＯＸマスタ情報取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' ＨＤＤマスタ存在チェック
    ''' </summary>
    ''' <param name="strTboxCd"></param>
    ''' <param name="strTboxVer"></param>
    ''' <param name="strHDDNo"></param>
    ''' <param name="strHDDCls"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfExistHDDData(ByVal strTboxCd As String, ByVal strTboxVer As String,
                                    ByVal strHDDNo As String, ByVal strHDDCls As String) As Boolean
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfExistHDDData = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S3", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'システムコード
                    .Add(pfSet_Param("tbox_cd", SqlDbType.NVarChar, strTboxCd))
                    'ＶＥＲ
                    .Add(pfSet_Param("tbox_ver", SqlDbType.NVarChar, strTboxVer))
                    'HDD No.
                    .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, strHDDNo))
                    'HDD 種別
                    .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, strHDDCls))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                mfExistHDDData = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "ＨＤＤマスタ情報取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 明細追加（更新）時チェック
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msCheckUpdateDetail()

        Dim strbuff As String = String.Empty    '各マスタ情報取得用
        Dim strbuff2 As String = String.Empty   '各マスタ情報取得用
        Dim drMsg As DataRow = Nothing          '検証メッセージ取得

        '機器分類必須入力チェック
        If Me.ddlRstAppaDiv.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "機器分類")
            valRstAppaDiv.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valRstAppaDiv.Text = drMsg.Item(P_VALMES_SMES)
            valRstAppaDiv.IsValid = False
        End If

        '機器種別必須入力チェック
        If Me.ddlRstAppaCls.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "機器種別")
            valRstAppaCls.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valRstAppaCls.Text = drMsg.Item(P_VALMES_SMES)
            valRstAppaCls.IsValid = False
        End If

        'システム必須入力チェック
        If Me.ddlRstSystem.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "システム")
            valRstSystem.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valRstSystem.Text = drMsg.Item(P_VALMES_SMES)
            valRstSystem.IsValid = False
        End If

        '型式／機器必須入力チェック
        If Me.ddlRstAppaModel.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "型式／機器")
            valRstAppaModel.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valRstAppaModel.Text = drMsg.Item(P_VALMES_SMES)
            valRstAppaModel.IsValid = False
        End If

        '機器分類＝ＴＢＯＸでかつ機器種別＝制御部の場合、ＴＢＯＸマスタ（Ｍ０３）の存在チェックを行う
        If Me.ddlRstAppaDiv.SelectedValue = M_APPADIV_TBOX AndAlso
           mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) = M_APPACLS_CTL Then
            'SERUPDP001-002
            'If mfGetTboxMasterInfo(Me.ddlRstSystem.SelectedValue, Me.txtRstVersion.ppText, strbuff) = False Then
            If mfGetTboxMasterInfo(Me.ddlRstSystem.SelectedValue, Me.ddlRstVersion.ppSelectedValue, strbuff) = False Then
                'SERUPDP001-002 END
                'Me.txtRstVersion.psSet_ErrorNo("2002", "入力したシステムとＶＥＲ")
                Me.ddlRstVersion.psSet_ErrorNo("2002", "入力したシステムとＶＥＲ")
                'SERUPDP001-002 END
            End If
        End If

        '機器分類＝ＴＢＯＸで、かつ機器種別＝ＨＤＤの場合は整合性チェックを行う
        If Me.ddlRstAppaDiv.SelectedValue = M_APPADIV_TBOX AndAlso
           mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) = M_APPACLS_HDD Then
            'SERUPDP001-002 END
            'If mfExistHDDData(Me.ddlRstSystem.SelectedValue, Me.txtRstVersion.ppText, Me.ddlRstHddNo.SelectedValue, Me.ddlRstHddCls.SelectedValue) = False Then
            If mfExistHDDData(Me.ddlRstSystem.SelectedValue, Me.ddlRstVersion.ppSelectedValue, Me.ddlRstHddNo.SelectedValue, Me.ddlRstHddCls.SelectedValue) = False Then
                'SERUPDP001-002 END
                drMsg = pfGet_ValMes("2002", "入力したシステムとＶＥＲに対するHDD No.とHDD種別")
                valRstHddNo.ErrorMessage = drMsg.Item(P_VALMES_MES)
                valRstHddNo.Text = drMsg.Item(P_VALMES_SMES)
                valRstHddNo.IsValid = False
            End If
        End If

        'SERUPDP001-002
        'If Me.txtCondNm.ppText <> "" Then
        '    If mfCondCheck() = False Then
        '        Me.txtCondNm.psSet_ErrorNo("2002", "指定した" & Me.txtCondNm.ppName)
        '    End If
        'End If
        'SERUPDP001-002 END


        '現設置／保管コード
        Select Case Me.ddlRstPlaceCls.ppSelectedValue
            Case 1  'ホール名取得
                If mfGetHallInfo(Me.txtRstStrageCd.ppText, strbuff) Then
                    Me.lblRstStrageNm.Text = strbuff
                Else
                    Me.lblRstStrageNm.Text = String.Empty
                    Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                End If
            Case 2  '営業所名、または保守拠点名取得
                If mfGetOfficeInfo(M_TRD_EIGYO + "," + M_TRD_HOSHU, Me.txtRstStrageCd.ppText, strbuff) Then
                    Me.lblRstStrageNm.Text = strbuff
                Else
                    Me.lblRstStrageNm.Text = String.Empty
                    Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case 3  '倉庫名取得
                If mfGetOfficeInfo(M_TRD_SONOTA, Me.txtRstStrageCd.ppText, strbuff) Then
                    Me.lblRstStrageNm.Text = strbuff
                Else
                    Me.lblRstStrageNm.Text = String.Empty
                    Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case 4  '代理店名取得
                If mfGetOfficeInfo(M_TRD_DAIRI, Me.txtRstStrageCd.ppText, strbuff) Then
                    Me.lblRstStrageNm.Text = strbuff
                Else
                    Me.lblRstStrageNm.Text = String.Empty
                    Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case 5  'メーカ名取得
                If mfGetOfficeInfo(M_TRD_MAKER, Me.txtRstStrageCd.ppText, strbuff) Then
                    Me.lblRstStrageNm.Text = strbuff
                Else
                    Me.lblRstStrageNm.Text = String.Empty
                    Me.txtRstStrageCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case Else
                If Me.txtRstStrageCd.ppText <> String.Empty Then
                    Me.lblRstStrageNm.Text = String.Empty
                    Me.ddlRstPlaceCls.psSet_ErrorNo("5003", "現設置／保管場所の場所区分")
                End If
        End Select

        '移動先コード
        Select Case Me.ddlRstMoveCls.ppSelectedValue
            Case 1  'ホール名取得
                If mfGetHallInfo(Me.txtRstMoveCd.ppText, strbuff) Then
                    Me.lblRstMoveNm.Text = strbuff
                Else
                    Me.lblRstMoveNm.Text = String.Empty
                    Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したＴＢＯＸＩＤ")
                End If
            Case 2  '営業所名、または保守拠点名取得
                If mfGetOfficeInfo(M_TRD_EIGYO + "," + M_TRD_HOSHU, Me.txtRstMoveCd.ppText, strbuff) Then
                    Me.lblRstMoveNm.Text = strbuff
                Else
                    Me.lblRstMoveNm.Text = String.Empty
                    Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case 3  '倉庫名取得
                If mfGetOfficeInfo(M_TRD_SONOTA, Me.txtRstMoveCd.ppText, strbuff) Then
                    Me.lblRstMoveNm.Text = strbuff
                Else
                    Me.lblRstMoveNm.Text = String.Empty
                    Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case 4  '代理店名取得
                If mfGetOfficeInfo(M_TRD_DAIRI, Me.txtRstMoveCd.ppText, strbuff) Then
                    Me.lblRstMoveNm.Text = strbuff
                Else
                    Me.lblRstMoveNm.Text = String.Empty
                    Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case 5  'メーカ名取得
                If mfGetOfficeInfo(M_TRD_MAKER, Me.txtRstMoveCd.ppText, strbuff) Then
                    Me.lblRstMoveNm.Text = strbuff
                Else
                    Me.lblRstMoveNm.Text = String.Empty
                    Me.txtRstMoveCd.psSet_ErrorNo("2002", "入力したコード")
                End If
            Case Else
                If Me.txtRstMoveCd.ppText <> String.Empty Then
                    Me.lblRstMoveNm.Text = String.Empty
                    Me.ddlRstMoveCls.psSet_ErrorNo("5003", "移動先の場所区分")
                End If
        End Select

        '移動理由必須入力チェック
        If Me.ddlRstMoveReason.SelectedValue = String.Empty Then
            drMsg = pfGet_ValMes("5003", "移動理由")
            valRstMoveReason.ErrorMessage = drMsg.Item(P_VALMES_MES)
            valRstMoveReason.Text = drMsg.Item(P_VALMES_SMES)
            valRstMoveReason.IsValid = False
        End If

        '---- 追加 2014/06/10 -----S
        'HDDNo、HDD種別指定可能チェック
        If Me.ddlRstHddNo.Text <> String.Empty Or
            Me.ddlRstHddCls.Text <> String.Empty Then
            '機器種別にHDD以外はエラー
            If mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) <> "09" Then
                drMsg = pfGet_ValMes("2010", "機器種別にHDD以外", "HDDNo、HDD種別")
                valRstHddNo.ErrorMessage = drMsg.Item(P_VALMES_MES)
                valRstHddNo.Text = drMsg.Item(P_VALMES_SMES)
                valRstHddNo.IsValid = False
            End If
        End If
        '---- 追加 2014/06/10 -----E

        '機器備考の存在チェック
        'SERUPDP001-002 
        If ddlCondNm.SelectedValue <> String.Empty Then
            If mfCondNmCheck() = False Then
                drMsg = pfGet_ValMes("2013", "選択された機器備考", "機器備考マスタ")
                valCondNm.ErrorMessage = drMsg.Item(P_VALMES_MES)
                valCondNm.Text = drMsg.Item(P_VALMES_SMES)
                valCondNm.IsValid = False
            End If
        End If
        'SERUPDP001-002 END

    End Sub

    ''' <summary>
    ''' ＨＤＤマスタ存在チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCondCheck() As Boolean
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim dtRow As DataRow = Nothing          'DataRow

        objStack = New StackFrame

        mfCondCheck = False

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S6", objCn)
                With objCmd.Parameters
                    '--パラメータ設定
                    'システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlRstSystem.SelectedValue))
                    'ＶＥＲ
                    'SERUPDP001-002
                    '.Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtRstVersion.ppText))
                    .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.ddlRstVersion.ppSelectedValue))
                    'SERUPDP001-002 END
                    'HDDNO
                    .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, Me.ddlRstHddNo.SelectedValue))
                    'HDDCLS
                    .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, Me.ddlRstHddCls.SelectedValue))
                    'HDD No.
                    .Add(pfSet_Param("cond_nm", SqlDbType.NVarChar, Me.ddlCondNm.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Exit Function
                End If

                mfCondCheck = True

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器備考マスタ取得")

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    'SERUPDP001-002 
    ''' <summary>
    ''' 機器備考の存在チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfCondNmCheck() As Boolean
        Dim ds As New DataSet

        mfCondNmCheck = False

        ds = mfGetCondNm()

        If ds.Tables(0).Select("機器備考 = '" & ddlCondNm.SelectedValue.ToString.Trim & "'").Count <> 0 Then
            Return True
        End If
    End Function
    'SERUPDP001-002 END

    ''' <summary>
    ''' ドロップダウンリストのコード部分を取得　※コロン（：）で区切られた文字列の１番目を返す
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGETCodePart(ByVal strText As String) As String

        Dim strArray As String() = Nothing

        objStack = New StackFrame

        Try
            If strText = String.Empty Then
                Return strText
            End If

            strArray = strText.Split(":")

            Return strArray(0)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return String.Empty

        End Try

    End Function

    ''' <summary>
    ''' ドロップダウンリストの名称部分を取得　※コロン（：）で区切られた文字列の２番目を返す
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetNamePart(ByVal strText As String) As String

        Dim strArray As String() = Nothing

        objStack = New StackFrame

        Try
            If strText = String.Empty Then
                Return strText
            End If

            strArray = strText.Split(":")

            Return strArray(1)

        Catch ex As Exception

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Return String.Empty

        End Try

    End Function

    ''' <summary>
    ''' シリアルデータ更新処理
    ''' </summary>
    ''' <param name="intMode">1:追加、2:更新、3:削除</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfUpdateDataDetail(intMode As Integer) As Boolean

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim intRtn As Integer = 0               'ストアド戻り値
        Dim strSpName As String = String.Empty  'ストアド名
        Dim strErrCode As String = String.Empty 'エラーコード
        Dim strErrCode2 As String = String.Empty 'エラーコード２
        Dim strBuff As String = String.Empty

        objStack = New StackFrame

        mfUpdateDataDetail = False

        '実行ストアド名設定
        Select Case intMode
            Case 1  '追加
                strSpName = M_MY_DISP_ID + "_U1"
                strErrCode = "00003"
                strErrCode2 = "00008"
            Case 2  '更新
                strSpName = M_MY_DISP_ID + "_U2"
                strErrCode = "00001"
                strErrCode2 = "00007"
            Case 3  '削除
                strSpName = M_MY_DISP_ID + "_D1"
                strErrCode = "00002"
                strErrCode2 = "00009"
            Case Else
                Exit Function
        End Select

        'シリアルデータ更新処理
        If clsDataConnect.pfOpen_Database(objCn) Then
            Try
                objCmd = New SqlCommand(strSpName, objCn)
                With objCmd.Parameters

                    '--シリアル管理番号
                    .Add(pfSet_Param("serial_no", SqlDbType.NVarChar, Me.txtRstSerialNo.ppText))

                    '--システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlRstSystem.SelectedValue))

                    '--機器分類
                    .Add(pfSet_Param("appadiv_nm", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))

                    '--機器種別
                    .Add(pfSet_Param("appaclass_nm", SqlDbType.NVarChar, mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)))

                    '--型式/機器
                    .Add(pfSet_Param("appa_nm", SqlDbType.NVarChar, Me.ddlRstAppaModel.SelectedValue))

                    'SERUPDP001-002
                    '--ユーザＩＤ
                    .Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))

                    .Add(pfSet_Param("seq", SqlDbType.NVarChar, lblSeq.Text.Trim))
                    'SERUPDP001-002　END

                    If intMode = 1 OrElse intMode = 2 Then

                        '--場所区分
                        .Add(pfSet_Param("place_class", SqlDbType.NVarChar, Me.ddlRstMoveCls.ppSelectedValue))

                        '--現設置/保管コード
                        .Add(pfSet_Param("strage_cd", SqlDbType.NVarChar, Me.txtRstMoveCd.ppText))

                        '--現設置/保管名称
                        .Add(pfSet_Param("strage_nm", SqlDbType.NVarChar, Me.lblRstMoveNm.Text))

                        '--HDD番号
                        .Add(pfSet_Param("hdd_no", SqlDbType.NVarChar, Me.ddlRstHddNo.SelectedValue))

                        '--HDD種別
                        .Add(pfSet_Param("hdd_cls", SqlDbType.NVarChar, Me.ddlRstHddCls.SelectedValue))

                        '--システム
                        .Add(pfSet_Param("system", SqlDbType.NVarChar, mfGetNamePart(Me.ddlRstSystem.SelectedItem.Text)))

                        '--バージョン
                        'SERUPDP001-002
                        '.Add(pfSet_Param("version", SqlDbType.NVarChar, Me.txtRstVersion.ppText))
                        .Add(pfSet_Param("version", SqlDbType.NVarChar, Me.ddlRstVersion.ppSelectedValue))
                        'SERUPDP001-002 END

                        '--移動日
                        .Add(pfSet_Param("set_dt", SqlDbType.NVarChar, Me.dtbRstMoveDt.ppText))

                        '--移動元区分
                        .Add(pfSet_Param("move_class", SqlDbType.NVarChar, Me.ddlRstPlaceCls.ppDropDownList.SelectedValue))

                        '--移動元コード
                        .Add(pfSet_Param("move_cd", SqlDbType.NVarChar, Me.txtRstStrageCd.ppText))

                        '--移動元名称
                        .Add(pfSet_Param("move_nm", SqlDbType.NVarChar, Me.lblRstStrageNm.Text))

                        '--移動理由コード
                        .Add(pfSet_Param("mvrsn_cd", SqlDbType.NVarChar, Me.ddlRstMoveReason.SelectedValue))

                        '--管理番号
                        .Add(pfSet_Param("serialcntl_no", SqlDbType.NVarChar, Me.txtRstCntlNo.ppText))

                        '--物転番号
                        .Add(pfSet_Param("move_no", SqlDbType.NVarChar, Me.txtRstArclNo.ppText))

                        '--備考   
                        .Add(pfSet_Param("notetext", SqlDbType.NVarChar, Me.txtRstNotetext.ppText))

                        '--納入予定日(yyyy/MM/dd)
                        If Me.dtbRstDlvPlndt.ppText = String.Empty Then
                            .Add(pfSet_Param("dlv_plndt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("dlv_plndt", SqlDbType.NVarChar, Me.dtbRstDlvPlndt.ppText))
                        End If

                        '--納入日
                        If Me.dtbRstDlvDt.ppText = String.Empty Then
                            .Add(pfSet_Param("dlv_dt", SqlDbType.NVarChar, DBNull.Value))
                        Else
                            .Add(pfSet_Param("dlv_dt", SqlDbType.NVarChar, Me.dtbRstDlvDt.ppText))
                        End If

                        'SERUPDP001-002
                        '--ユーザＩＤ
                        '.Add(pfSet_Param("userid", SqlDbType.NVarChar, User.Identity.Name))
                        'SERUPDP001-002　END

                        '--機器備考
                        'SERUPDP001-002
                        .Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.ddlCondNm.SelectedValue))
                        '.Add(pfSet_Param("tboxid", SqlDbType.NVarChar, Me.txtCondNm.ppText))
                        'SERUPDP001-002 END

                    End If

                    If intMode = 2 Then
                        '--データ取得時更新日時
                        .Add(pfSet_Param("acq_updatedt", SqlDbType.NVarChar, Me.lblUpdateDt.Text))

                        '--データ更新後の更新日時
                        .Add(pfSet_Param("new_updatedt", SqlDbType.NVarChar, 30, ParameterDirection.Output))

                    End If

                    '--戻り値
                    .Add(pfSet_Param("retvalue", SqlDbType.NVarChar))

                End With

                'データ追加・更新・削除
                Using conTrn = objCn.BeginTransaction
                    objCmd.Transaction = conTrn

                    'コマンドタイプ設定(ストアド)
                    objCmd.CommandType = CommandType.StoredProcedure

                    'ストアド実行
                    objCmd.ExecuteNonQuery()

                    'ストアド戻り値チェック
                    intRtn = Integer.Parse(objCmd.Parameters("retvalue").Value.ToString)

                    Select Case intRtn
                        Case 0  '正常終了

                        Case -1 '既にデータありエラー
                            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)
                            Exit Function
                        Case -2 '更新データ整合性エラー（排他）
                            psMesBox(Me, "30009", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)
                            Exit Function
                        Case Else
                            psMesBox(Me, strErrCode2, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE, intRtn.ToString)
                            Exit Function
                    End Select

                    'データ更新後の更新日時設定
                    If intMode = 2 Then
                        Me.lblUpdateDt.Text = objCmd.Parameters("new_updatedt").Value.ToString()
                    End If

                    'コミット
                    conTrn.Commit()

                End Using
                'SERUPDP001-001
                psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "シリアル一覧")
                'SERUPDP001-001 END
                mfUpdateDataDetail = True

            Catch ex As Exception
                psMesBox(Me, strErrCode, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)

                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
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

    ''' <summary>
    ''' グリッドへデータ追加
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msAddGridData()

        Dim dtData As DataTable = Nothing
        Dim drNewData As DataRow = Nothing

        objStack = New StackFrame

        Try
            'グリッドから取得
            dtData = pfParse_DataTable(Me.grvList)
            Dim dt As New DataTable
            dt = dtData.Clone

            '新規行設定
            drNewData = dt.NewRow()

            'グリッド行データ設定
            msSetGridRowData(drNewData)

            '行追加
            dt.Rows.Add(drNewData)
            'dtData.Rows.Add(drNewData)

            'データ再設定
            Me.grvList.DataSource = dt
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' グリッドデータ更新
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msUpdGridData()

        Dim dtData As DataTable = Nothing

        objStack = New StackFrame

        Try
            'グリッドから取得
            dtData = pfParse_DataTable(Me.grvList)

            'グリッド行データ設定
            'SERUPDP001-002 
            For Each dr As DataRow
                In dtData.Select("シリアル番号   = '" & Me.txtRstSerialNo.ppText.ToString.Replace("'", "''") & "' AND " &
                                 "機器分類コード = '" & Me.ddlRstAppaDiv.SelectedValue & "' AND " &
                                 "機器種別コード = '" & mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) & "' AND " &
                                 "連番           = '" & lblSeq.Text & "'")
                'In dtData.Select("シリアル番号   = '" & Me.txtRstSerialNo.ppText.ToString.Replace("'", "''") & "' AND " &
                '                 "システムコード = '" & Me.ddlRstSystem.SelectedValue & "'  AND " &
                '                 "機器分類コード = '" & Me.ddlRstAppaDiv.SelectedValue & "' AND " &
                '                 "機器種別コード = '" & mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) & "' AND " &
                '                 "型式／機器     = '" & Me.ddlRstAppaModel.SelectedValue & "'")
                'SERUPDP001-002 END
                msSetGridRowData(dr)
            Next

            '行更新
            dtData.AcceptChanges()

            'データ再設定
            Me.grvList.DataSource = dtData
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    ''' <summary>
    ''' グリッドデータ削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msDelGridData()

        Dim dtData As DataTable = Nothing
        Dim drNew As DataRow = Nothing

        objStack = New StackFrame

        Try
            'グリッドから取得
            dtData = pfParse_DataTable(Me.grvList)
            Dim dt As New DataTable
            dt = dtData.Clone
            Dim dr As DataRow
            dr = dt.NewRow
            msDelGridRowData(dr)
            dt.Rows.Add(dr)
            '行削除
            'SERUPDP001-002 
            'For Each dr As DataRow
            '    In dtData.Select("シリアル番号   = '" & Me.txtRstSerialNo.ppText.ToString.Replace("'", "''") & "' AND " &
            '                     "機器分類コード = '" & Me.ddlRstAppaDiv.SelectedValue & "' AND " &
            '                     "機器種別コード = '" & mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) & "' AND " &
            '                     "連番           = '" & lblSeq.Text & "'")
            '    'In dtData.Select("シリアル番号   = '" & Me.txtRstSerialNo.ppText.ToString.Replace("'", "''") & "' AND " &
            '    '                 "システムコード = '" & Me.ddlRstSystem.SelectedValue & "'  AND " &
            '    '                 "機器分類コード = '" & Me.ddlRstAppaDiv.SelectedValue & "' AND " &
            '    '                 "機器種別コード = '" & mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) & "' AND " &
            '    '                 "型式／機器     = '" & Me.ddlRstAppaModel.SelectedValue & "'")


            '    msDelGridRowData(dr)
            '    'dr.Delete()
            '    'SERUPDP001-002 END
            'Next

            'データ再設定
            Me.grvList.DataSource = dt
            Me.grvList.DataBind()

        Catch ex As Exception
            psMesBox(Me, "00002", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細")

            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try

    End Sub

    '------- 追加 2014/06/10 -------S
    ''' <summary>
    ''' データバインド時の編集処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub grvList_DataBound(sender As Object, e As EventArgs) Handles grvList.DataBound

        'SERUPDP001-002
        '削除シリアルの赤字表示＆選択ボタン非活性処理
        For Each rowData As GridViewRow In grvList.Rows
            If CType(rowData.FindControl("削除区分"), TextBox).Text = "1" Then
                rowData.Cells(0).Enabled = False
                For i As Integer = 1 To rowData.Cells.Count - 1
                    CType(rowData.Cells(i).Controls(0), TextBox).ForeColor = Drawing.Color.Red
                Next
            End If
        Next
        'SERUPDP001-002 END

    End Sub
    '------- 追加 2014/06/10 -------E

    'SERUPDP001-002
    ''' <summary>
    ''' 入力欄活性制御
    ''' </summary>
    ''' <param name="_mode"></param>
    ''' <remarks></remarks>
    Private Sub msSetEnable(ByVal _mode As String)
        Try
            Me.btnDetailClear.Enabled = True    'クリアボタン
            Select Case _mode
                Case "Default"
                    Me.txtRstSerialNo.ppEnabled = True
                    Me.ddlRstAppaDiv.Enabled = True
                    Me.ddlRstAppaCls.Enabled = True

                    Me.ddlRstSystem.Enabled = False
                    'SERUPDP001-002
                    'Me.txtRstVersion.ppEnabled = False
                    Me.ddlRstVersion.ppEnabled = False
                    'SERUPDP001-002 END
                    Me.ddlRstAppaModel.Enabled = False
                    Me.ddlCondNm.Enabled = False
                    Me.ddlRstHddNo.Enabled = False
                    Me.ddlRstHddCls.Enabled = False
                    Me.ddlRstPlaceCls.ppEnabled = False
                    Me.txtRstStrageCd.ppEnabled = False
                    Me.dtbRstMoveDt.ppEnabled = False
                    Me.dtbRstDlvPlndt.ppEnabled = False
                    Me.ddlRstMoveReason.Enabled = False
                    Me.ddlRstMoveCls.ppEnabled = False
                    Me.txtRstMoveCd.ppEnabled = False
                    Me.dtbRstDlvDt.ppEnabled = False
                    Me.txtRstCntlNo.ppEnabled = False
                    Me.txtRstArclNo.ppEnabled = False
                    Me.txtRstNotetext.ppEnabled = False

                    Me.btnDetailInsert.Enabled = False  '追加ボタン
                    Me.btnDetailUpdate.Enabled = False  '更新ボタン
                    Me.btnDetailDelete.Enabled = False  '削除ボタン

                Case "Insert", "Select"
                    Me.txtRstSerialNo.ppEnabled = False
                    Me.ddlRstAppaDiv.Enabled = False
                    Me.ddlRstAppaCls.Enabled = False

                    Me.ddlRstSystem.Enabled = True
                    'SERUPDP001-002
                    'Me.txtRstVersion.ppEnabled = True
                    Me.ddlRstVersion.ppEnabled = True
                    'SERUPDP001-002 END
                    Me.ddlRstAppaModel.Enabled = True
                    Me.ddlCondNm.Enabled = True
                    If ddlRstAppaDiv.SelectedValue = "01" AndAlso mfGetNamePart(Me.ddlRstAppaCls.SelectedValue) = "09" Then
                        Me.ddlRstHddNo.Enabled = True
                        Me.ddlRstHddCls.Enabled = True
                    Else
                        Me.ddlRstHddNo.Enabled = False
                        Me.ddlRstHddCls.Enabled = False
                    End If
                    Me.ddlRstPlaceCls.ppEnabled = True
                    Me.txtRstStrageCd.ppEnabled = True
                    Me.dtbRstMoveDt.ppEnabled = True
                    Me.dtbRstDlvPlndt.ppEnabled = True
                    Me.ddlRstMoveReason.Enabled = True
                    Me.ddlRstMoveCls.ppEnabled = True
                    Me.txtRstMoveCd.ppEnabled = True
                    Me.dtbRstDlvDt.ppEnabled = True
                    Me.txtRstCntlNo.ppEnabled = True
                    Me.txtRstArclNo.ppEnabled = True
                    Me.txtRstNotetext.ppEnabled = True
                    Select Case _mode
                        Case "Insert"
                            Me.btnDetailInsert.Enabled = True   '追加ボタン
                            Me.btnDetailUpdate.Enabled = False  '更新ボタン
                            Me.btnDetailDelete.Enabled = False  '削除ボタン
                        Case "Select"
                            Me.btnDetailInsert.Enabled = False  '追加ボタン
                            Me.btnDetailUpdate.Enabled = True   '更新ボタン
                            Me.btnDetailDelete.Enabled = True   '削除ボタン
                    End Select
            End Select

        Catch ex As Exception
            psMesBox(Me, "30008", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' 既存データ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetExistData() As DataSet

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット

        objStack = New StackFrame

        mfGetExistData = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S9", objCn) 'S1はシリアル検索がLIKE演算子の為使用しません。
                With objCmd.Parameters
                    '--パラメータ設定
                    'シリアル番号
                    .Add(pfSet_Param("serialno", SqlDbType.NVarChar,
                                     Me.txtRstSerialNo.ppText.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")))
                    '機器分類
                    .Add(pfSet_Param("appadivnm", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    '機器種別
                    .Add(pfSet_Param("appaclsnm", SqlDbType.NVarChar, mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)))
                End With

                'データ取得
                mfGetExistData = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, M_INFO_MESSAGE)
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If

    End Function

    ''' <summary>
    ''' 最大連番を取得
    ''' </summary>
    ''' <param name="_ds"></param>
    ''' <remarks>登録データの最大連番を返す</remarks>
    Private Function mfGetMaxSeq(ByVal _ds As DataSet) As Integer
        Dim intMaxSeq As Integer = 0
        psLogStart(Me)
        Try
            If _ds.Tables(0).Rows.Count = 0 Then
                '行数 = 0 は新規シリアルなのでそのまま0を返す
            Else
                '行数がそのまま最大連番になるはずだが念の為、登録された連番を取得し最大連番を抽出する。
                Dim intTmp As Integer = 0   '最大連番保持用
                For Each dr As DataRow In _ds.Tables(0).Rows
                    If intTmp < dr.Item("連番") Then
                        intTmp = dr.Item("連番")
                    End If
                Next
                intMaxSeq = intTmp
            End If
            mfGetMaxSeq = intMaxSeq
        Catch ex As Exception
            'ログ出力
            psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
            'エラー時は-1を返す
            mfGetMaxSeq = -1
        Finally
            '終了ログ出力
            psLogEnd(Me)
        End Try
    End Function

    ''' <summary>
    ''' システムバージョン取得
    ''' </summary>
    ''' <param name="_syscd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function mfGetVersion(ByVal _syscd As String) As DataSet
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        mfGetVersion = Nothing

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("ZCMPSEL034", objCn)
                With objCmd.Parameters
                    'システムコード
                    .Add(pfSet_Param("tbox_cd", SqlDbType.NVarChar, _syscd))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                If objDs.Tables(0).Rows.Count = 0 Then
                    Return Nothing
                Else
                    Return objDs
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "TBOXバージョンマスタ取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")

            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try
        End If
    End Function

    Private Function mfGetCondNm() As DataSet
        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        objStack = New StackFrame

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand("SERUPDP001_S8", objCn)
                With objCmd.Parameters
                    '機器分類コード
                    .Add(pfSet_Param("appaclass_cd", SqlDbType.NVarChar, Me.ddlRstAppaDiv.SelectedValue))
                    '機器種別コード
                    .Add(pfSet_Param("appa_cls", SqlDbType.NVarChar, mfGetNamePart(Me.ddlRstAppaCls.SelectedValue)))
                    'システムコード
                    .Add(pfSet_Param("system_cd", SqlDbType.NVarChar, Me.ddlRstSystem.SelectedValue))
                    'HDD
                    .Add(pfSet_Param("Hdd_no", SqlDbType.NVarChar, Me.ddlRstHddNo.SelectedValue))
                    .Add(pfSet_Param("Hdd_cls", SqlDbType.NVarChar, Me.ddlRstHddCls.SelectedValue))
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "機器分類マスタ一覧取得")
                'ログ出力
                psLogWrite("", Me.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                                objStack.GetMethod.Name, "", ex.ToString, "Catch")
                objDs = Nothing
            Finally
                'DB切断
                If Not clsDataConnect.pfClose_Database(objCn) Then
                    psMesBox(Me, "00006", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                End If
            End Try

        End If
        mfGetCondNm = objDs
    End Function

    'SERUPDP001-002 END

#End Region

End Class
