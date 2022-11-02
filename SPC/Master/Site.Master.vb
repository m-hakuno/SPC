Imports System.Web.UI
Imports SPC.ClsCMCommon
Imports SPC.ClsCMExclusive


Public Class sitemaster05
    Inherits System.Web.UI.MasterPage

    Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Dim _antiXsrfTokenValue As String

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
    End Sub

    Private Sub CloseBtn()

    End Sub

    Private Sub Close_Cick(sender As Object, e As EventArgs)

        '画面終了処理
        ppCount = "close"
        Response.Write("<script language='javascript'> { (window.open('','_self').opener=window).close();}</script>")

    End Sub

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
            lblProgramID.Text = "≪ " & value & " ≫"
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


End Class