'********************************************************************************************************************************
'*　システム　：　サポートセンタステム
'*　処理名　　：　マスターページ
'*　ＰＧＭＩＤ：　
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.10.16　：　土岐
'********************************************************************************************************************************

'================================================================================================================================
'=　インポート定義
'================================================================================================================================
'Imports SPC.Global_asax
Imports SPC.ClsCMCommon
Imports SPC.ClsCMExclusive
Imports System.Web.UI

Public Class SiteMaster
#Region "継承定義"
    '============================================================================================================================
    '=　継承定義
    '============================================================================================================================
    Inherits System.Web.UI.MasterPage
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
    Dim clsExc As New ClsCMExclusive
#End Region

#Region "プロパティ定義"
    '============================================================================================================================
    '=　プロパティ定義
    '============================================================================================================================
    '画面ＩＤ
    Private mstrProgramID As String
    Public Property ppProgramID() As String
        Get
            Return mstrProgramID
        End Get
        Set(ByVal value As String)
            mstrProgramID = value
            lblProgramID.Text = "≪" & value & "≫"
        End Set
    End Property

    'ログアウト_表示非表示
    Private mshtLogoutMode As ClsComVer.E_ログアウトモード = Nothing
    Public Property ppLogout_Mode() As ClsComVer.E_ログアウトモード
        Get
            Return mshtLogoutMode
        End Get
        Set(value As ClsComVer.E_ログアウトモード)
            mshtLogoutMode = value
            Select Case value
                Case ClsComVer.E_ログアウトモード.ログアウト
                    lkbLogout.Visible = True
                    lkbLogout.OnClientClick = pfGet_OCClickMes("30001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel)
                    lkbLogout.Text = "[ログアウト]"
                    AddHandler lkbLogout.Click, AddressOf lkbLogout_Click
                Case ClsComVer.E_ログアウトモード.非表示
                    lkbLogout.Visible = False
                    lnmLoginNm.Visible = False
                Case ClsComVer.E_ログアウトモード.閉じる
                    lkbLogout.Visible = True
                    lkbLogout.CausesValidation = False
                    'lkbLogout.OnClientClick = "return window_close('" & pfGet_Mes("00001", clscomver.E_Mタイプ.警告) & "'" &
                    '    ",'" & pfGet_MesType(clscomver.E_Mタイプ.警告) & "00001')"
                    lkbLogout.OnClientClick = pfGet_OCClickMes("00001", ClsComVer.E_Mタイプ.警告, ClsComVer.E_Mモード.OKCancel)
                    AddHandler lkbLogout.Click, AddressOf Close_Cick
                    lkbLogout.Text = "[閉じる]"
            End Select
        End Set
    End Property

    'タイトル
    Public Property ppTitle() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal value As String)
            lblTitle.Text = value
            Page.Header.Title = value
        End Set
    End Property

    'パンくずリスト-表示非表示
    Public Property ppBcList_Visible() As Boolean
        Get
            Return lblBreadcrumblist.Visible
        End Get
        Set(ByVal value As Boolean)
            lblBreadcrumblist.Visible = value
        End Set
    End Property

    'パンくずリスト-テキスト
    Public Property ppBcList_Text() As String
        Get
            Return lblBreadcrumblist.Text
        End Get
        Set(ByVal value As String)
            lblBreadcrumblist.Text = value
        End Set
    End Property

    '左ボタン1
    Public ReadOnly Property ppLeftButton1() As Button
        Get
            Return btnLeft1
        End Get
    End Property

    '左ボタン2
    Public ReadOnly Property ppLeftButton2() As Button
        Get
            Return btnLeft2
        End Get
    End Property

    '左ボタン3
    Public ReadOnly Property ppLeftButton3() As Button
        Get
            Return btnLeft3
        End Get
    End Property

    '左ボタン4
    Public ReadOnly Property ppLeftButton4() As Button
        Get
            Return btnLeft4
        End Get
    End Property

    '左ボタン5
    Public ReadOnly Property ppLeftButton5() As Button
        Get
            Return btnLeft5
        End Get
    End Property

    '左ボタン6
    Public ReadOnly Property ppLeftButton6() As Button
        Get
            Return btnLeft6
        End Get
    End Property

    '左ボタン7
    Public ReadOnly Property ppLeftButton7() As Button
        Get
            Return btnLeft7
        End Get
    End Property

    '左ボタン8
    Public ReadOnly Property ppLeftButton8() As Button
        Get
            Return btnLeft8
        End Get
    End Property

    '左ボタン9
    Public ReadOnly Property ppLeftButton9() As Button
        Get
            Return btnLeft9
        End Get
    End Property

    '左ボタン10
    Public ReadOnly Property ppLeftButton10() As Button
        Get
            Return btnLeft10
        End Get
    End Property

    '右ボタン1
    Public ReadOnly Property ppRigthButton1() As Button
        Get
            Return btnRigth1
        End Get
    End Property

    '右ボタン2
    Public ReadOnly Property ppRigthButton2() As Button
        Get
            Return btnRigth2
        End Get
    End Property

    '右ボタン3
    Public ReadOnly Property ppRigthButton3() As Button
        Get
            Return btnRigth3
        End Get
    End Property

    '右ボタン4
    Public ReadOnly Property ppRigthButton4() As Button
        Get
            Return btnRigth4
        End Get
    End Property

    '右ボタン5
    Public ReadOnly Property ppRigthButton5() As Button
        Get
            Return btnRigth5
        End Get
    End Property

    '右ボタン6
    Public ReadOnly Property ppRigthButton6() As Button
        Get
            Return btnRigth6
        End Get
    End Property

    '右ボタン7
    Public ReadOnly Property ppRigthButton7() As Button
        Get
            Return btnRigth7
        End Get
    End Property

    '右ボタン8
    Public ReadOnly Property ppRigthButton8() As Button
        Get
            Return btnRigth8
        End Get
    End Property

    '右ボタン9
    Public ReadOnly Property ppRigthButton9() As Button
        Get
            Return btnRigth9
        End Get
    End Property

    '右ボタン10
    Public ReadOnly Property ppRigthButton10() As Button
        Get
            Return btnRigth10
        End Get
    End Property

    '登録用年月日時刻
    Public Property ppExclusiveDate() As String
        Get
            Return hddExclusiveDate.Value
        End Get
        Set(ByVal value As String)
            hddExclusiveDate.Value = value
        End Set
    End Property

    '登録用年月日時刻(詳細)
    Public Property ppExclusiveDateDtl() As String
        Get
            Return hddExclusiveDate_dtl.Value
        End Get
        Set(ByVal value As String)
            hddExclusiveDate_dtl.Value = value
        End Set
    End Property

#End Region

#Region "イベントプロシージャ"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '--------------------------------
        '2015/04/06 加賀　ここから
        '--------------------------------
        '初期表示
        If Not IsPostBack Then
            '遷移条件保存
            hddTERMS.Value = Session(P_SESSION_TERMS)
            '参照時、排他情報削除
            If hddTERMS.Value <> "" AndAlso hddTERMS.Value = ClsComVer.E_遷移条件.参照 Then
                hddExclusiveDate.Value = ""
            End If
        End If
        '--------------------------------
        '2015/04/06 加賀　ここまで
        '--------------------------------


        '--------------------------------
        '2014/06/10 後藤　ここから
        '--------------------------------

        If ppLogout_Mode = Nothing Then
            ppLogout_Mode = ClsComVer.E_ログアウトモード.閉じる
        Else

            '警告メッセージ設定
            Dim script As StringBuilder = New StringBuilder
            script.Append("<script type='text/javascript'>")
            script.Append("function Message(){")
            script.Append("alert('排他情報の削除に失敗しました');")
            script.Append("}")
            script.Append("</script>")
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterClientScriptBlock(Me.GetType(), "Message", script.ToString, False)

        End If

        'セッションタイムアウト時、カスタムページに遷移（ただし排他削除画面は対象外とする）
        If Not String.Equals(Me.lblProgramID.Text, "≪COMLGIP001≫") And Not String.Equals(Me.lblProgramID.Text, "≪ExculsiveList≫") Then
            If String.IsNullOrEmpty(Session(P_SESSION_USERID)) Then
                If String.Equals(Me.lblProgramID.Text, "≪COMMENP001≫") Then
                    Response.Redirect("~/customSessionTimeOutMain.html")
                Else
                    Response.Redirect("~/customSessionTimeOut.html")
                End If

            End If
        End If

        '--------------------------------
        '2014/06/10 後藤　ここまで
        '--------------------------------
    End Sub

    Protected Sub lkbLogout_Click(sender As Object, e As EventArgs)

        If mshtLogoutMode = ClsComVer.E_ログアウトモード.ログアウト Then

            '--------------------------------
            '2014/06/10 後藤　ここから
            '--------------------------------
            If Not Session(P_SESSION_SESSTION_ID) = Nothing Then

                If clsExc.pfDel_Exclusive(Session(P_SESSION_SESSTION_ID)) <> 0 Then

                    'エラーの場合メッセージ出力
                    Dim csManager As ClientScriptManager = Page.ClientScript
                    csManager.RegisterClientScriptBlock(Me.GetType(), "DoMessage", "Message();", True)

                    '認証クリア
                    FormsAuthentication.SignOut()
                    Session.Abandon()

                    '画面クローズ
                    Dim script As StringBuilder = New StringBuilder
                    script.Append("<script type='text/javascript'>")
                    script.Append("window.open('', '_self').close()")
                    script.Append("</script>")
                    Dim cs As ClientScriptManager = Page.ClientScript
                    cs.RegisterClientScriptBlock(Me.GetType(), "self", script.ToString, False)
                Else
                    '認証クリア
                    FormsAuthentication.SignOut()
                    Session.Abandon()
                    FormsAuthentication.RedirectToLoginPage()
                End If
            End If

            '--------------------------------
            '2014/06/10 後藤　ここまで
            '--------------------------------
        End If
    End Sub
#End Region

#Region "そのほかのプロシージャ"
    ''' <summary>
    ''' 引数をキーとしたViewStateを返す。
    ''' </summary>
    ''' <param name="ipstrKey">キー</param>
    ''' <returns>ViewState</returns>
    ''' <remarks></remarks>
    Public Function pfGet_ViewState(ByVal ipstrKey As String)
        pfGet_ViewState = Me.ViewState(ipstrKey)
    End Function

    ''' <summary>
    ''' 引数をキーとしたViewStateに値を代入する。
    ''' </summary>
    ''' <param name="ipstrKey">キー</param>
    ''' <param name="ipobjVal">代入する値</param>
    ''' <remarks></remarks>
    Public Sub psSet_ViewState(ByVal ipstrKey As String, ByVal ipobjVal As Object)
        Me.ViewState(ipstrKey) = ipobjVal
    End Sub
#End Region

#Region "終了処理プロシージャ"

    ''' <summary>
    ''' 画面終了時のイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Close_Cick(sender As Object, e As EventArgs)

        'If hddTERMS.Value.ToString = "" OrElse hddTERMS.Value <> ClsComVer.E_遷移条件.参照 Then
        If hddTERMS.Value.ToString = "" OrElse hddTERMS.Value <> 1 Then
            '★排他情報削除
            If Not Me.hddExclusiveDate.Value = String.Empty Then

                clsExc.pfDel_Exclusive_master(Session(P_SESSION_SESSTION_ID) _
                                     , hddExclusiveDate.Value)

            End If

            '★排他情報削除(明細)
            If Not Me.hddExclusiveDate_dtl.Value = String.Empty Then

                clsExc.pfDel_Exclusive_master(Session(P_SESSION_SESSTION_ID) _
                                     , hddExclusiveDate_dtl.Value)

            End If

        End If


        '画面終了処理
        Response.Write("<script language='javascript'> { (window.open('','_self').opener=window).close();}</script>")


    End Sub

#End Region

End Class
