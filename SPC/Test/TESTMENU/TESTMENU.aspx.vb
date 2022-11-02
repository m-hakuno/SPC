#Region "プログラムヘッダ"
'********************************************************************************************************************************
'*　処理名　　：　試験用メニュー画面
'*　ＰＧＭＩＤ：　TESTMENU
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.02.07　：　中川
'********************************************************************************************************************************
#End Region

#Region "インポート定義"
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
Imports System.IO
Imports SPC.ClsCMCommon
'Imports SPC.Global_asax
#End Region

#Region "クラス定義"
Public Class TESTMENU
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
#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
#End Region

#Region "イベントプロシージャ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Master.ppLogout_Mode = ClsComVer.E_ログアウトモード.ログアウト

        AddHandler Master.ppRigthButton2.Click, AddressOf btnClearSession_Click

        '共通
        AddHandler Me.COMMENP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP003.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP004.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP005.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP006.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP007.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMMENP008.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMSELP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMSELP004.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMSELP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.DLCINPP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.COMLSTP099.Click, AddressOf LinkBtn_Click

        '工事
        AddHandler Me.CNSLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSLSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSUPDP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSLSTP004.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSUPDP003.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSLSTP005.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSUPDP004.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSLSTP006.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSINQP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSUPDP005.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSLSTP007.Click, AddressOf LinkBtn_Click
        AddHandler Me.CNSUPDP006.Click, AddressOf LinkBtn_Click
        AddHandler Me.DLLSELP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.MSTLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.SERLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.SERUPDP001.Click, AddressOf LinkBtn_Click

        '監視
        AddHandler Me.WATLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.WATUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.WATLSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.WATINQP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.WATOUTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.WATOUTP026.Click, AddressOf LinkBtn_Click
        AddHandler Me.HEALSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.HEAUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.OVELSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.OVELSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.SLFLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.SLFLSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.ERRLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.ERRSELP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.ICHLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.ICHLSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.DLCLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.DSULSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.DSUUPDP001.Click, AddressOf LinkBtn_Click

        '保守
        AddHandler Me.REQINQP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPINQP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPOUTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CTISELP005.Click, AddressOf LinkBtn_Click
        AddHandler Me.REQLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.REQSELP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPSELP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.REQLSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.EQULSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.EQULSTP003.Click, AddressOf LinkBtn_Click
        AddHandler Me.BPIINQP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.BPIFIXP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.BPILSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPUPDP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPINQP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.CMPLSTP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.QUAOUTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.QUAUPDP001.Click, AddressOf LinkBtn_Click

        '故障受付
        AddHandler Me.BRKLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.BRKUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.BRKINQP001.Click, AddressOf LinkBtn_Click

        '修理・整備
        AddHandler Me.REPLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.REPUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.REPUPDP002.Click, AddressOf LinkBtn_Click
        AddHandler Me.MNTSELP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.MNTUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.REPOUTP001.Click, AddressOf LinkBtn_Click

        'サポートセンタ
        AddHandler Me.BBPLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.BBPUPDP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.CDPLSTP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.TBPINPP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.DOCMENP001.Click, AddressOf LinkBtn_Click
        AddHandler Me.DOCUPDP001.Click, AddressOf LinkBtn_Click

        Try
            If Not IsPostBack Then

                Dim strFileName As String = String.Empty
                Dim js_exe As String = String.Empty

                'ボタン設定
                Master.ppRigthButton2.Visible = True
                Master.ppRigthButton2.Text = "Sessionクリア"
                'Master.ppRigthButton3.Visible = True
                'Master.ppRigthButton3.Text = "レポート設定"

                strFileName = "sample.txt"
                js_exe = "exe_Cti(" + """" + strFileName.Replace("-", "") + """" + ");"
                'btnNotePad.OnClientClick = pfGet_OCClickMes("40001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, strFileName).Replace(";", "") + "&&" + js_exe
                btnNotePad.OnClientClick = js_exe
                btnNotePad.Enabled = True


                '画面設定
                Master.ppProgramID = "TESTMENU"
                Master.ppTitle = "試験用メニュー"

                Me.rblBcList.SelectedValue = "1"

            End If

        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' リンクボタン押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LinkBtn_Click(sender As Object, e As EventArgs)

        Try
            'セッション設定
            Call msSet_Session()
            'レポート設定
            'If Me.txtRptCls.Text <> "" Then
            '    Call msSet_Rpt()
            'End If

            '画面を表示
            psOpen_Window(Me, sender.CommandName)

        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' セッションクリア押下イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClearSession_Click(sender As Object, e As EventArgs)

        Try
            Call msClear_Session()

        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' ActiveX代替
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnActiveX_Click(sender As Object, e As EventArgs)
        Try
            Dim args As String()
            Dim filePath As String
            args = Environment.GetCommandLineArgs()
            For Each cmd As String In args
                If cmd.StartsWith("note:") Then
                    filePath = cmd.Substring("note:".Length)
                    Process.Start("notepad.exe", filePath)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' セッション情報クリア処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msClear_Session()

        Session(P_KEY) = Nothing
        Session(P_SESSION_TERMS) = Nothing
        Session(P_SESSION_BCLIST) = Nothing
        Session(P_SESSION_NGC_MEN) = Nothing
        Session(P_SESSION_OLDDISP) = Nothing
        Session(P_SESSION_PRV_REPORT) = Nothing
        Session(P_SESSION_PRV_DATA) = Nothing
        Session(P_SESSION_PRV_NAME) = Nothing
        Session(P_SESSION_IP) = Nothing
        Session(P_SESSION_PLACE) = Nothing
        Session(P_SESSION_LOGIN_DATE) = Nothing
        Session(P_SESSION_SESSTION_ID) = Nothing
        Session(P_SESSION_GROUP_NUM) = Nothing

        Me.txtKey.Text = Nothing
        Me.ddlMode.SelectedValue = ""
        Me.ddlUser.SelectedValue = ""
        Me.rblBcList.SelectedValue = "1"
        Me.txtNgcMenu.Text = Nothing
        Me.txtOldDisp.Text = Nothing
        Me.txtRptCls.Text = Nothing
        Me.txtData.Text = Nothing
        Me.txtRptNm.Text = Nothing
        Me.txtIp.Text = Nothing
        Me.ddlPlace.SelectedValue = ""
        Me.txtLoginDt.Text = Nothing
        Me.txtSessionID.Text = Nothing
        Me.txtGroup.Text = Nothing


    End Sub

    ''' <summary>
    ''' セッション設定処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msSet_Session()

        Dim strKey() As String = Nothing        'キー項目
        Dim shtMode As Short = Nothing          '遷移条件
        Dim strOldDisp As String = Nothing      '遷移元ID
        Dim strNgcMenu() As String = Nothing    'NGCメニュー

        'キー項目の設定
        If Me.txtKey.Text <> "" Then
            strKey = Me.txtKey.Text.Split(",")
            Session(P_KEY) = strKey
            'If strKey.Count = 1 Then
            '    Session(P_KEY) = strKey
            'End If
        Else
            Session(P_KEY) = Nothing
        End If

        '遷移条件の設定
        If ddlMode.SelectedValue <> "" Then
            shtMode = ddlMode.SelectedValue
            Session(P_SESSION_TERMS) = shtMode
        Else
            Session(P_SESSION_TERMS) = Nothing
        End If

        '遷移元IDの設定
        If Me.txtOldDisp.Text <> "" Then
            strOldDisp = Me.txtOldDisp.Text
            Session(P_SESSION_OLDDISP) = strOldDisp
        Else
            Session(P_SESSION_OLDDISP) = Nothing
        End If

        'パンくずの設定
        If rblBcList.SelectedValue = "1" Then
            Session(P_SESSION_BCLIST) = "試験用メニュー"
        Else
            Session(P_SESSION_BCLIST) = Nothing
        End If

        'NGCメニューの設定
        If Me.txtNgcMenu.Text <> "" Then
            strNgcMenu = Me.txtNgcMenu.Text.Split(",")
            Session(P_SESSION_NGC_MEN) = strNgcMenu
        Else
            Session(P_SESSION_NGC_MEN) = Nothing
        End If

        'ユーザ権限の設定
        If Me.ddlUser.SelectedValue <> "" Then
            'Session() = ddlUser.SelectedValue
        End If

        '端末情報
        If Me.txtIp.Text <> "" Then
            Session(P_SESSION_IP) = Me.txtIp.Text
        End If

        '場所
        If Me.ddlPlace.SelectedValue <> "" Then
            Session(P_SESSION_PLACE) = Me.ddlPlace.SelectedValue
        End If

        'ログイン年月日
        If Me.txtLoginDt.Text <> "" Then
            Session(P_SESSION_LOGIN_DATE) = Me.txtLoginDt.Text
        End If

        'セッションＩＤ
        If Me.txtSessionID.Text <> "" Then
            Session(P_SESSION_SESSTION_ID) = Me.txtSessionID.Text
        End If

        'グループ番号
        If Me.txtGroup.Text <> "" Then
            Session(P_SESSION_GROUP_NUM) = Me.txtGroup.Text
        End If

    End Sub

    ' ''' <summary>
    ' ''' レポート設定処理
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub msSet_Rpt()

    '    Dim strRpt As String = Nothing      'レポートクラス名
    '    Dim Rpt As Object = Nothing
    '    Dim objData As String = Nothing     'データ
    '    Dim strRptNm As String = Nothing    'レポート名
    '    Dim strSql As String = Nothing

    '    strRpt = Me.txtRptCls.Text
    '    strRptNm = Me.txtRptNm.Text

    '    Select Case strRpt
    '        Case "CNSREP001"
    '            'Rpt = New CNSREP001
    '        Case "CNSREP002"
    '            Rpt = New CNSREP002
    '        Case "CNSREP003"
    '            'Rpt = New CNSREP003
    '        Case "CNSREP004"
    '            'Rpt = New CNSREP004
    '        Case "CNSREP005"
    '            'Rpt = New CNSREP005
    '        Case "CNSREP006"
    '            'Rpt = New CNSREP006
    '        Case "CNSREP007"
    '            'Rpt = New CNSREP007
    '        Case "CNSREP008"
    '            'Rpt = New CNSREP008
    '        Case "CNSREP009"
    '            'Rpt = New CNSREP009
    '        Case "CNSREP010"
    '            'Rpt = New CNSREP010
    '        Case "CNSREP011"
    '            'Rpt = New CNSREP011
    '        Case "CNSREP012"
    '            'Rpt = New CNSREP012
    '        Case "CNSREP013"
    '            'Rpt = New CNSREP013
    '        Case "CNSREP014"
    '            'Rpt = New CNSREP014
    '        Case "MSTREP003"
    '            'Rpt = New MSTREP003
    '        Case "REQREP001"
    '            'Rpt = New REQREP001
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New
    '        Case ""
    '            'Rpt = New 
    '        Case "BPIREP002"
    '            Rpt = New BPIREP002
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New 
    '        Case "REQREP002"
    '            'Rpt = New REQREP002
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New 
    '        Case "CMPREP001"
    '            Rpt = New CMPREP001
    '        Case "DLCREP001"
    '            'Rpt = New DLCREP001
    '        Case "ICHREP001"
    '            'Rpt = New ICHREP001
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New 
    '        Case ""
    '            'Rpt = New  
    '        Case "TBRREP001"
    '            'Rpt = New TBRREP001
    '        Case "TBRREP002"
    '            'Rpt = New TBRREP002
    '        Case "TBRREP003"
    '            'Rpt = New TBRREP003
    '        Case "TBRREP004"
    '            'Rpt = New TBRREP004
    '        Case "TBRREP005"
    '            'Rpt = New TBRREP005
    '        Case "TBRREP006"
    '            Rpt = New TBRREP006
    '        Case "TBRREP007"
    '            'Rpt = New TBRREP007
    '        Case "TBRREP008"
    '            'Rpt = New TBRREP008
    '        Case "TBRREP009"
    '            'Rpt = New TBRREP009
    '        Case "TBRREP010"
    '            'Rpt = New TBRREP010
    '        Case "TBRREP011"
    '            'Rpt = New TBRREP011
    '        Case "TBRREP012"
    '            Rpt = New TBRREP012
    '        Case "TBRREP013"
    '            'Rpt = New TBRREP013
    '        Case "TBRREP014"
    '            'Rpt = New TBRREP014 
    '        Case "DSUREP002"
    '            'Rpt = New DSUREP002
    '        Case "MNTREP002"
    '            'Rpt = New MNTREP002
    '        Case ""
    '            'Rpt = New 
    '        Case "MNTREP003"
    '            Rpt = New MNTREP003
    '        Case Else
    '            psMesBox(Me, "30010", clscomver.E_Mタイプ.エラー, clscomver.E_S実行.描画後, strRpt & "が見つかりません。")
    '            Exit Sub
    '    End Select


    '    Dim MyArray()
    '    ReDim MyArray(5)
    '    MyArray(0) = New CNSREP002
    '    MyArray(1) = New BPIREP002
    '    MyArray(2) = New CMPREP001
    '    MyArray(3) = New TBRREP006
    '    MyArray(4) = New TBRREP006
    '    MyArray(5) = New TBRREP012



    '    'ストアドを用いたデータの指定
    '    strSql = "EXEC " & Me.txtData.Text  '「ストアド名 '引数1','引数2'......」

    '    Session(P_SESSION_PRV_REPORT) = MyArray
    '    Session(P_SESSION_PRV_DATA) = strSql
    '    Session(P_SESSION_PRV_NAME) = strRptNm

    'End Sub

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
#End Region