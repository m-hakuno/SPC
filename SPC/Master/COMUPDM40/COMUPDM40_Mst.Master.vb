Imports System.Web.UI
Imports SPC.ClsCMCommon
Imports SPC.ClsCMExclusive


Public Class COMUPDM40_Mst
    Inherits System.Web.UI.MasterPage

    Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Dim _antiXsrfTokenValue As String
    Dim clsExc As New ClsCMExclusive

    Protected Sub Page_Init(sender As Object, e As System.EventArgs)
        ' 以下のコードは、XSRF 攻撃からの保護に役立ちます
        Dim requestCookie As HttpCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If ((Not requestCookie Is Nothing) AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue)) Then
            ' Cookie の Anti-XSRF トークンを使用します
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' 新しい Anti-XSRF トークンを生成し、Cookie に保存します
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie As HttpCookie = New HttpCookie(AntiXsrfTokenKey) With {.HttpOnly = True, .Value = _antiXsrfTokenValue}
            If (FormsAuthentication.RequireSSL And Request.IsSecureConnection) Then
                responseCookie.Secure = True
            End If
            Response.Cookies.Set(responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    Private Sub master_Page_PreLoad(sender As Object, e As System.EventArgs)
        If (Not IsPostBack) Then
            ' Anti-XSRF トークンを設定します
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, String.Empty)
        Else
            ' Anti-XSRF トークンを検証します
            If (Not DirectCast(ViewState(AntiXsrfTokenKey), String) = _antiXsrfTokenValue _
                Or Not DirectCast(ViewState(AntiXsrfUserNameKey), String) = If(Context.User.Identity.Name, String.Empty)) Then
                Throw New InvalidOperationException("Anti-XSRF トークンの検証が失敗しました。")
            End If
        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lkbLogout.OnClientClick = pfGet_OCClickMes("00001", ClsComVer.E_Mタイプ.警告, ClsComVer.E_Mモード.OKCancel)
        AddHandler lkbLogout.Click, AddressOf Close_Cick
        btnDmy.Visible = False

        If Not IsPostBack Then
            '排他で使用
            ViewState(P_SESSION_GROUP_NUM) = Session(P_SESSION_GROUP_NUM)

            '削除ボタンの色を設定
            btnDelete.BackColor = Drawing.Color.FromArgb(255, 102, 102)

            'ボタン押下時のメッセージ設定
            btnInsert.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, lblTitle.Text)       '登録
            btnUpdate.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, lblTitle.Text)       '更新
            btnDelete.OnClientClick = pfGet_OCClickMes("00005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, lblTitle.Text)

            '削除

        End If
    End Sub

    Private Sub CloseBtn()

    End Sub

    Private Sub Close_Cick(sender As Object, e As EventArgs)

        '画面終了処理
        ppCount = "close"

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

        Response.Write("<script language='javascript'> { (window.open('','_self').opener=window).close();}</script>")

    End Sub

    Public Property ppMainEnabled() As Boolean
        Get
            Return pnlMain.Enabled
        End Get
        Set(ByVal value As Boolean)
            pnlMain.Enabled = value
        End Set
    End Property

    Public ReadOnly Property ppBtnSearch() As Button
        Get
            Return btnSarch
        End Get
    End Property

    Public ReadOnly Property ppBtnSrcClear() As Button
        Get
            Return btnSrcClear
        End Get
    End Property

    Public ReadOnly Property ppBtnDmy() As Button
        Get
            Return btnDmy
        End Get
    End Property

    Public ReadOnly Property ppBtnClear() As Button
        Get
            Return btnClear
        End Get
    End Property

    Public ReadOnly Property ppBtnInsert() As Button
        Get
            Return btnInsert
        End Get
    End Property

    Public ReadOnly Property ppBtnUpdate() As Button
        Get
            Return btnUpdate
        End Get
    End Property

    Public ReadOnly Property ppBtnDelete() As Button
        Get
            Return btnDelete
        End Get
    End Property

    Public ReadOnly Property ppPlaceHolderSearch() As ContentPlaceHolder
        Get
            Return SearchContent
        End Get
    End Property

    Public ReadOnly Property ppPlaceHolderMain() As ContentPlaceHolder
        Get
            Return MainContent
        End Get
    End Property

    Public Property ppCount() As String
        Get
            Return lblcount.Text
        End Get
        Set(value As String)
            lblcount.Text = value
        End Set

    End Property

    Public Property ppProgramID() As String

        Get
            Return lblProgramID.Text
        End Get
        Set(value As String)
            lblProgramID.Text = "≪" & value & "≫"
        End Set
    End Property

    Public Property ppTitle() As String

        Get
            Return lblTitle.Text
        End Get
        Set(value As String)
            lblTitle.Text = value
            Page.Header.Title = value
        End Set
    End Property

    Public Property ppBCList() As String
        Get
            Return lblBCList.Text
        End Get
        Set(value As String)
            lblBCList.Text = value
        End Set
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

    Public Sub chksDelVisible(ByVal DelVisible As Boolean)
        lblsDel.Visible = DelVisible
        chksDEL.Visible = DelVisible
    End Sub

    Public ReadOnly Property ppchksDel() As CheckBox

        Get
            Return chksDEL
        End Get

    End Property




End Class