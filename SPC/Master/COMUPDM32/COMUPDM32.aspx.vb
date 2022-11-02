'********************************************************************************************************************************
'*　システム　：　サポートセンタステム　＜共通＞
'*　処理名　　：　マスタ管理メニュー
'*　ＰＧＭＩＤ：　ComMstMenu
'*                                                                                                  CopyRihgt SANCOSMOS CO., LTD.
'*-------------------------------------------------------------------------------------------------------------------------------
'*  作　成　　：　2014.11.13　：　村岡
'********************************************************************************************************************************

#Region "インポート定義"
Imports System.Data.SqlClient
Imports SPC.ClsCMCommon
Imports SPC.ClsCMDataConnect
Imports SPC.ClsCMDataLink
Imports SQL_DBCLS_LIB

#End Region

Public Class COMUPDM32
    Inherits System.Web.UI.Page

#Region "定数定義"
    Const M_DISP_ID = "COMUPDM32"
    Const M_LOGIN = "~/" & P_COM & "/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "001/" & P_FUN_COM & P_SCR_LGI & P_PAGE & "032.aspx"
#End Region


#Region "変数定義"
    Private MasterTable As DataTable                    '本マスタテーブル
    Private MasterTableAdd As DataTable                 '本マスタテーブル追加用
    Private CmdBld As New System.Text.StringBuilder     'SQLコマンド格納用
    Private FirstCheck As Integer                       'ポストバック確認用
    Private ClsSQL As New ClsSQL                         'クラスSQL
    Dim clsCMDBC As New ClsCMDBCom
    Dim Updcls As Boolean = False
#End Region


#Region "ページロード～絵画直前"
    '---------------------------------------------
    'ページロード時実行
    '---------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FirstCheck = 1
        '初回のみ
        If Not IsPostBack Then


            '画面ID
            Master.ppProgramID = M_DISP_ID
            Master.ppTitle = clsCMDBC.pfGet_DispNm(M_DISP_ID)


            'パンくずリスト設定
            Master.ppBCList = pfGet_BCList(Session(P_SESSION_BCLIST), "マスター管理メニュー" & "＞" & Master.ppTitle)

            'メインテーブルボタンの初期設定
            btnAdd.Enabled = True
            btnUpd.Enabled = False



            '確認ダイアログ設定
            btnUpd.OnClientClick = pfGet_OCClickMes("00004", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "一覧閾値マスタ")
            btnAdd.OnClientClick = pfGet_OCClickMes("00006", ClsComVer.E_Mタイプ.情報, ClsComVer.E_Mモード.OKCancel, "一覧閾値マスタ")


            FirstCheck = 0
            ViewFilter()
        End If


        AddHandler Master.ppBtnSrcClear.Click, AddressOf mstrBtnSrcClear
        AddHandler Master.ppBtnSearch.Click, AddressOf mstrBtnSrc


    End Sub
    '---------------------------------------------
    'ドロップダウンリストM17_PROCESS利用(メインテーブル）
    '---------------------------------------------
    Private Sub ddlIntM17()
        Dim MT As New DataTable

        CmdBld.Clear()
        'CmdBld.Append("SELECT M17_DISP_CD,M17_SHORT_NM,M17_DISP_CD + ':' + M17_SHORT_NM CLS FROM M17_PROCESS")
        CmdBld.Append("SELECT M17_DISP_CD,M17_SHORT_NM,M17_DELETE_CD,M17_DISP_CD + ':' + M17_SHORT_NM CLS")
        CmdBld.Append(" FROM M17_PROCESS")
        CmdBld.Append(" INNER JOIN M32_LISTLIMIT ON M17_DISP_CD = M32_DISP_CD WHERE M17_DELETE_CD='0'")


        MT = ClsSQL.getDataSetTable(CmdBld.ToString, "TableM17")

        ddlDispCd.DataSource = MT
        ddlDispCd.DataTextField = "CLS"
        ddlDispCd.DataValueField = "M17_DISP_CD"
        ddlDispCd.DataBind()
        ddlDispCd.Items.Insert("0", "")

    End Sub
    '---------------------------------------------
    'ドロップダウンリストM17_PROCESS利用(検索テーブル）
    '---------------------------------------------
    Private Sub ddlInt2M17()
        Dim MT As New DataTable

        CmdBld.Clear()
        'CmdBld.Append("SELECT M17_DISP_CD,M17_SHORT_NM FROM M17_PROCESS")
        CmdBld.Append("SELECT M17_DISP_CD,M17_DELETE_CD")
        CmdBld.Append(" FROM M17_PROCESS")
        CmdBld.Append(" INNER JOIN M32_LISTLIMIT ON M17_DISP_CD = M32_DISP_CD WHERE M17_DELETE_CD='0'")

        MT = ClsSQL.getDataSetTable(CmdBld.ToString, "TableM17")

        ddlSrcDispCd.DataSource = MT
        ddlSrcDispCd.DataTextField = "M17_DISP_CD"
        ddlSrcDispCd.DataValueField = "M17_DISP_CD"
        ddlSrcDispCd.DataBind()
        ddlSrcDispCd.Items.Insert("0", "")


    End Sub

    '---------------------------------------------
    '画面表示直前処理
    '---------------------------------------------
    Private Sub COMUPDM32_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If GrdV.Rows.Count > 0 Then
            GrdV.HeaderRow.TableSection = TableRowSection.TableHeader
        End If

        '画面初期化(最初のみ)
        If FirstCheck = 0 Then

            Try

                'ドロップダウンリストの設定
                ddlIntM17() '画面ID・画面名

                ddlInt2M17() '画面ID


                'テキストボックスのカンマ編集

                Dim i As Integer = "123456789"
                txtSrcDefaultMax.Text = i.ToString("#,##0")
                txtSrcSearchMax.Text = i.ToString("#,##0")
                txtDefaultMax.Text = i.ToString("#,##0")
                txtSearchMax.Text = i.ToString("#,##0")




                '画面初期化
                SrctableClear()
                MainTableClear()

            Catch
                psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
            End Try

        End If

    End Sub
#End Region


#Region "検索テーブル"

    '--------------------------------------------
    'マスターページ　検索ボタン　押下
    '--------------------------------------------
    Private Sub mstrBtnSrc()
        ViewFilter()
    End Sub

    '---------------------------------------------
    'マスターページ　検索クリアボタン　押下
    '---------------------------------------------
    Private Sub mstrBtnSrcClear()
        SrctableClear()
    End Sub



    '--------------------------------------------
    '検索テーブル編集オブジェクトクリアメソッド
    '--------------------------------------------
    Private Sub SrctableClear()
        ddlSrcDispCd.SelectedValue = ""
        txtSrcName.Text = ""
        txtSrcDefaultMax.Text = ""
        txtSrcSearchMax.Text = ""


    End Sub
#End Region


#Region "メインテーブルボタンクリックイベント"
    '---------------------------------------------
    'メインテーブルクリアボタン
    '---------------------------------------------
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        MainTableClear()
        btnAdd.Enabled = True
        btnUpd.Enabled = False
        ddlDispCd.Enabled = True
    End Sub

    '--------------------------------------------
    'メインテーブル追加ボタン
    '--------------------------------------------
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        'エラーチェック実行
        Me.Validate("val")
        If Me.IsValid = False Then
            Exit Sub
        End If


        Try

        Catch ex As Exception
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End Try

        CmdBld.Clear()

        'レコード追加SQL文
        CmdBld.Append("INSERT INTO M32_LISTLIMIT(")
        CmdBld.Append("            M32_DISP_CD")
        CmdBld.Append("           ,M32_DEFAULT_MAX")
        CmdBld.Append("           ,M32_SEARCH_MAX")
        CmdBld.Append("           ,M32_INSERT_DT")
        CmdBld.Append("           ,M32_INSERT_USR")
        CmdBld.Append(")VALUES(")
        CmdBld.Append("            '" & ddlDispCd.SelectedValue & "'")
        CmdBld.Append("           ,'" & txtDefaultMax.Text.Replace(",", "") & "'")
        CmdBld.Append("           ,'" & txtSearchMax.Text.Replace(",", "") & "'")
        CmdBld.Append("           ,'" & DateTime.Now & "'")
        CmdBld.Append("           ,'" & User.Identity.Name & "'")
        CmdBld.Append(")")


        'SQL実行
        If ClsSQL.SQLRes(CmdBld.ToString) Then
            '失敗メッセージ表示
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧閾値マスタ")
        Else
            '完了メッセージ表示
            psMesBox(Me, "00003", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "一覧閾値マスタ", "一覧閾値マスタ")
        End If
        ddlDispCd.Enabled = True

        MainTableClear()



        'グリッドビュー更新
        ViewFilter()


    End Sub

    '--------------------------------------------
    'メインテーブル更新ボタン
    '--------------------------------------------
    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpd.Click

        Updcls = True

        'エラーチェック実行
        Me.Validate("val")
        If Me.IsValid = False Then
            Exit Sub
        End If

        CmdBld.Clear()  'SQL文初期化

        'レコード更新SQL文
        CmdBld.Append("UPDATE M32_LISTLIMIT")
        CmdBld.Append(" SET M32_DISP_CD='" & ddlDispCd.SelectedValue & "'")
        CmdBld.Append(" ,M32_DEFAULT_MAX='" & txtDefaultMax.Text.Replace(",", "") & "'")
        CmdBld.Append(" ,M32_SEARCH_MAX= '" & txtSearchMax.Text.Replace(",", "") & "'")
        CmdBld.Append(" ,M32_UPDATE_DT='" & DateTime.Now & "'")
        CmdBld.Append(" ,M32_UPDATE_USR='" & User.Identity.Name & "'")
        CmdBld.Append("WHERE M32_DISP_CD = '" & ddlDispCd.SelectedValue & "'")

        'SQL実行
        If ClsSQL.SQLRes(CmdBld.ToString) Then
            '失敗メッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "一覧閾値マスタ")
        Else
            '完了メッセージ表示
            psMesBox(Me, "00001", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, "一覧閾値マスタ", "一覧閾値マスタ")
        End If


        'If ChkDel.Checked = False Then
        '    ChkDel.Enabled = False
        'End If
        MainTableClear()
        ddlDispCd.Enabled = True



        'グリッドビュー更新
        ViewFilter()

    End Sub


    '--------------------------------------------
    'メインテーブル編集オブジェクトクリアメソッド
    '--------------------------------------------
    Private Sub MainTableClear()
        ddlDispCd.SelectedValue = ""
        txtDefaultMax.Text = ""
        txtSearchMax.Text = ""

    End Sub


#End Region


#Region "グリッドビュー操作"

    '--------------------------------------------
    '選択ボタン (グリッドビュー内)　押下
    '--------------------------------------------
    Protected Sub GridView_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GrdV.RowCommand

        'Dim strDispCd As String    '選択された社員コード格納用
        ViewFilter()


        Try
            ViewFilter()
            'Select e.CommandName
            '    '選択ボタン
            '    Case "btnVSel"
            '        '選択された行番号のみのテーブルから各要素をメインテーブルに表示
            '        strDispCd = DirectCast(GrdV.Rows(e.CommandArgument).FindControl("lblVDispCd"), Label).Text

            '        Dim MainShow As New DataView(MasterTable)
            '        MainShow.RowFilter = "M32_DISP_CD = '" & strDispCd.Substring(0, 10) & "'"
            '        ddlDispCd.SelectedValue = MainShow(0)("CLS").ToString.Substring(0, 10)
            '        txtDefaultMax.Text = MainShow(0)("M32_DEFAULT_MAX").ToString
            '        txtSearchMax.Text = MainShow(0)("M32_SEARCH_MAX").ToString

            Dim dt As New DataTable
            If e.CommandName = "btnVSel" Then
                CmdBld.Clear()
                CmdBld.Append("SELECT * FROM M32_LISTLIMIT")
                CmdBld.Append(" WHERE M32_DISP_CD = '" & DirectCast(GrdV.Rows(e.CommandArgument).FindControl("lblVDispCd"), Label).Text.Substring(0, 10) & "' ")
                dt = ClsSQL.getDataSetTable(CmdBld.ToString, "Table32")

                ddlDispCd.SelectedValue = dt(0)("M32_DISP_CD").ToString.Substring(0, 10)
                txtDefaultMax.Text = dt(0)("M32_DEFAULT_MAX").ToString
                txtSearchMax.Text = dt(0)("M32_SEARCH_MAX").ToString
            End If




            '重複不可のキーのTextBoxを編集不可にする
            ddlDispCd.Enabled = False



            '追加ボタンを使用不可にする
            btnAdd.Enabled = False
            btnUpd.Enabled = True

            'Case Else
            Exit Sub
            'End Select
        Catch
            psMesBox(Me, "10001", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, "明細選択")
            MainTableClear()
            Exit Sub
        End Try
    End Sub

    '--------------------------------------------
    'グリッドビュー表示メソッド
    '--------------------------------------------
    Private Sub ViewFilter()
        Dim FilterCheck As Integer = 1

        '検索条件設定
        CmdBld.Clear()
        If ddlSrcDispCd.SelectedValue + txtSrcName.Text + txtSrcDefaultMax.Text + txtSrcSearchMax.Text = "" Then
            CmdBld.Append("SELECT M32_DISP_CD       ,M32_DEFAULT_MAX      ,M32_SEARCH_MAX")
            CmdBld.Append("      ,M32_INSERT_DT     ,M32_INSERT_USR      ,M32_UPDATE_DT      ,M32_UPDATE_USR     ,CLS")
            CmdBld.Append("	   FROM SPCDB.dbo.M32_LISTLIMIT AS T1")
            CmdBld.Append(" INNER JOIN (SELECT M17_DISP_CD ,M17_SHORT_NM, M17_DELETE_CD ,M17_DISP_CD + ':' + M17_SHORT_NM as CLS FROM SPCDB.dbo.M17_PROCESS) AS T2")
            CmdBld.Append(" ON T1.M32_DISP_CD = T2.M17_DISP_CD")
            CmdBld.Append(" WHERE M17_DELETE_CD='0'")
        Else

            CmdBld.Append("SELECT M32_DISP_CD       ,M32_DEFAULT_MAX      ,M32_SEARCH_MAX")
            CmdBld.Append("      ,M32_INSERT_DT     ,M32_INSERT_USR      ,M32_UPDATE_DT      ,M32_UPDATE_USR      ,CLS")
            CmdBld.Append("	   FROM SPCDB.dbo.M32_LISTLIMIT AS T1")
            CmdBld.Append(" INNER JOIN (SELECT M17_DISP_CD ,M17_SHORT_NM, M17_DELETE_CD, M17_DISP_CD + ':' + M17_SHORT_NM as CLS FROM SPCDB.dbo.M17_PROCESS) AS T2")
            CmdBld.Append(" ON T1.M32_DISP_CD = T2.M17_DISP_CD  WHERE")

            If ddlSrcDispCd.SelectedValue <> "" Then
                CmdBld.Append(" AND M32_DISP_CD LIKE '%" & ddlSrcDispCd.SelectedValue & "%' ")
            End If

            If txtSrcName.Text <> "" Then
                CmdBld.Append(" AND M17_SHORT_NM LIKE '%" & txtSrcName.Text & "%' ")
            End If

            If txtSrcDefaultMax.Text <> "" Then
                CmdBld.Append(" AND M32_DEFAULT_MAX ='" & txtSrcDefaultMax.Text.Replace(",", "") & "' ")
            End If

            If txtSrcSearchMax.Text <> "" Then
                CmdBld.Append(" AND M32_SEARCH_MAX = '" & txtSrcSearchMax.Text.Replace(",", "") & "' ")
            End If
            CmdBld.Append(" AND M17_DELETE_CD='0'")

            CmdBld.Replace("WHERE AND", "WHERE")

            CmdBld.Append(" ORDER BY M32_DISP_CD")
        End If
        'グリッドビューに反映
        Try
            '検索一致のレコードのみのデータテーブル作成
            MasterTable = ClsSQL.getDataSetTable(CmdBld.ToString, "TableM32")

            'ビューステート保管
            ViewState("MasterTable") = MasterTable

            '検索結果が0件の場合のメッセージ表示
            If MasterTable.Rows.Count = 0 Then
                psMesBox(Me, "00007", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後)
            End If

            GrdV.DataSource = MasterTable
            GrdV.DataBind()
            Master.ppCount = MasterTable.Rows.Count

        Catch
            psMesBox(Me, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
        End Try


    End Sub

#End Region


#Region "エラーチェックなど"

    '---------------------------------------------------
    'エラー設定（メインテーブル）
    '---------------------------------------------------
    Protected Sub CustomValidator1_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CustomValidator1.ServerValidate
        If txtDefaultMax.Text = "" Then
            CustomValidator1.Text = "未入力エラー"
            source.errormessage = "初期最大件数に値が設定されていません。"
            args.IsValid = False
            Exit Sub
        End If

        If Regex.IsMatch(txtDefaultMax.Text, "^[0-9]{1,6}|,$") = False Then
            CustomValidator1.Text = "形式エラー"
            source.errormessage = "初期最大件数は半角数字で入力してください。"
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Protected Sub CustomValidator2_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CustomValidator2.ServerValidate
        If txtSearchMax.Text = "" Then
            CustomValidator2.Text = "未入力エラー"
            source.errormessage = "検索結果件数に値が設定されていません。"
            args.IsValid = False
            Exit Sub
        End If

        If Regex.IsMatch(txtSearchMax.Text, "^[0-9]{1,6}|,$") = False Then
            CustomValidator2.Text = "形式エラー"
            source.errormessage = "検索結果件数は半角数字で入力してください。"
            args.IsValid = False
            Exit Sub
        End If
    End Sub
    Protected Sub CustomValidator3_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles CustomValidator3.ServerValidate
        If ddlDispCd.SelectedValue = "" Then
            CustomValidator3.Text = "未入力エラー"
            source.errormessage = "画面ID・画面名に値が設定されていません。"
            args.IsValid = False
            Exit Sub
        End If

        If Updcls = False Then

            Dim ServerName As String
            Dim DBName As String
            Dim UserID As String
            Dim Pass As String
            Dim Cn As SqlConnection = New SqlConnection
            Dim Cmd As New SqlCommand
            ServerName = "192.168.100.203\test"
            DBName = "SPCDB"
            UserID = "SPC_Dvlt_User"
            Pass = "SPC_SQL_User"
            Cn.ConnectionString = _
                  "Server=" & ServerName & ";" & _
                      "Initial Catalog=" & DBName & ";" & _
                      "User ID=" & UserID & ";" & _
                      "Password=" & Pass & ";" & _
                      "Integrated Security=false"

            Cmd.Connection = Cn
            CmdBld.Clear()
            CmdBld.Append("SELECT COUNT(*) FROM spcDB.dbo.M32_LISTLIMIT WHERE M32_DISP_CD = '" & ddlDispCd.SelectedValue & "'  ")
            Cmd.CommandText = CmdBld.ToString
            Cn.Open()
            If CInt(Cmd.ExecuteScalar) <> 0 Then
                CustomValidator3.Text = "整合性エラー"
                source.errormessage = "画面ID・画面名はすでに登録されています。"
                args.IsValid = False
            End If
        End If
    End Sub
#End Region

    '------------------------------------------------
    'テキストボックスのカンマ編集
    '------------------------------------------------
    Protected Sub txtSrcDefaultMax_TextChanged(sender As Object, e As EventArgs) Handles txtSrcDefaultMax.TextChanged

        Dim inNum As Integer

        If Integer.TryParse(txtSrcDefaultMax.Text, inNum) = True Then
            txtSrcDefaultMax.Text = inNum.ToString("#,##0")
        End If

    End Sub

    Protected Sub txtSrcSearchMax_TextChanged(sender As Object, e As EventArgs) Handles txtSrcSearchMax.TextChanged

        Dim inNum As Integer
        If Integer.TryParse(txtSrcSearchMax.Text, inNum) = True Then
            txtSrcSearchMax.Text = inNum.ToString("#,##0")


        End If
    End Sub

    Protected Sub txtDefaultMax_TextChanged(sender As Object, e As EventArgs) Handles txtDefaultMax.TextChanged
        Dim inNum As Integer

        If Integer.TryParse(txtDefaultMax.Text, inNum) = True Then
            txtDefaultMax.Text = inNum.ToString("#,##0")

        End If
    End Sub

    Protected Sub txtSearchMax_TextChanged(sender As Object, e As EventArgs) Handles txtSearchMax.TextChanged
        Dim inNum As Integer
        If Integer.TryParse(txtSearchMax.Text, inNum) = True Then
            txtSearchMax.Text = inNum.ToString("#,##0")


        End If
    End Sub

End Class
