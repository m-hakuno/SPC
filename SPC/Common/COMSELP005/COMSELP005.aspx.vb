'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　業者情報詳細
'*　ＰＧＭＩＤ：　COMSELP005
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2013.12.09　：　ＮＫＣ
'********************************************************************************************************************************

#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
'Imports SPC.Global_asax
'================================================================================================================================
'=　インポート定義
'================================================================================================================================
#End Region

Public Class COMSELP005
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
    Const M_MY_DISP_ID = P_FUN_COM & P_SCR_SEL & P_PAGE & "005"
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
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then  '初回表示のみ

            '検索条件クリアボタン押下時の検証を無効
            Master.ppRigthButton2.CausesValidation = False

            'プログラムID、画面名設定
            Master.ppProgramID = M_MY_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_MY_DISP_ID)

            'パンくずリスト設定
            'Master.ppBcList_Text = pfGet_BCList(Session(P_SESSION_BCLIST), Master.ppTitle)
            Master.ppBcList_Text = DirectCast(Session(P_SESSION_BCLIST), String)

            'データ取得処理
            msGetData()

        End If

    End Sub

    '---------------------------
    '2014/04/14 武 ここから
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
            Case "SPC"
            Case "営業所"
            Case "NGC"
        End Select

    End Sub
    '---------------------------
    '2014/04/14 武 ここまで
    '---------------------------

#End Region

#Region "そのほかのプロシージャ"

    ''' <summary>
    ''' データ取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub msGetData()

        Dim objCn As SqlConnection = Nothing    'SqlConnectionクラス
        Dim objCmd As SqlCommand = Nothing      'SqlCommandクラス
        Dim objDs As DataSet = Nothing          'データセット
        Dim strKey As String = Nothing          '業者情報キー
        Dim dtRow As DataRow = Nothing          'DataRow
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStack = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        '業者情報キー設定
        strKey = DirectCast(Session(P_KEY), String())(0)

        'DB接続
        If Not clsDataConnect.pfOpen_Database(objCn) Then
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        Else
            Try
                objCmd = New SqlCommand(M_MY_DISP_ID + "_S1", objCn)
                With objCmd.Parameters
                    'パラメータ設定
                    .Add(pfSet_Param("seq", SqlDbType.Int, strKey)) '連番
                End With

                'データ取得
                objDs = clsDataConnect.pfGet_DataSet(objCmd)

                'データ表示
                If objDs Is Nothing Then
                    psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者詳細")
                Else
                    If objDs.Tables(0).Rows.Count > 0 Then
                        dtRow = objDs.Tables(0).Rows(0)

                        Me.txtTraderCd.Text = dtRow("業者コード").ToString
                        Me.txtTraderNm.ppText = dtRow("業者名").ToString
                        Me.txtCompanyCd.Text = dtRow("会社コード").ToString
                        Me.txtCompanyNm.ppText = dtRow("会社名").ToString
                        Me.txtCmpZipNo.ppText = dtRow("会社_郵便番号").ToString
                        Me.txtCmpStateCd.ppText = dtRow("会社_県コード").ToString
                        Me.txtCmpAddress.ppText = dtRow("会社_住所").ToString
                        Me.txtCmpTelNo.ppText = dtRow("会社_電話番号").ToString
                        Me.txtCmpFaxNo.ppText = dtRow("会社_FAX番号").ToString
                        Me.txtIgtCd.Text = dtRow("統括_統括コード").ToString
                        Me.txtIgtNm.ppText = dtRow("統括_営業所名").ToString
                        Me.txtIgtZipNo.ppText = dtRow("統括_郵便番号").ToString
                        Me.txtIgtStateCd.ppText = dtRow("統括_県コード").ToString
                        Me.txtIgtAddress.ppText = dtRow("統括_住所").ToString
                        Me.txtIgtTelNo.ppText = dtRow("統括_電話番号").ToString
                        Me.txtIgtFaxNo.ppText = dtRow("統括_FAX番号").ToString
                        Me.txtOfficeCd.Text = dtRow("営業_営業所コード").ToString
                        Me.txtOfficeNm.ppText = dtRow("営業_営業所名").ToString
                        Me.txtOfcZipNo.ppText = dtRow("営業_郵便番号").ToString
                        Me.txtOfcStateCd.ppText = dtRow("営業_県コード").ToString
                        Me.txtOfcAddress.ppText = dtRow("営業_住所").ToString
                        Me.txtOfcTelNo.ppText = dtRow("営業_電話番号").ToString
                        Me.txtOfcFaxNo.ppText = dtRow("営業_FAX番号").ToString
                    Else
                        psMesBox(Me, "00009", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                    End If
                End If

            Catch ex As Exception
                psMesBox(Me, "00004", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "業者詳細")
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

#End Region

#Region "終了処理プロシージャ"
#End Region

End Class
