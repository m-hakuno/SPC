Imports System.Data.SqlClient
Imports SPC.ClsCMCommon


Public Class ClsSQL

#Region "変数定義"
    Private ServerName As String
    Private DBName As String
    Private UserID As String
    Private Pass As String
    Private SQLDA As SqlClient.SqlDataAdapter                               '接続アダプタ
    Private SQLDS As New DataSet                                            'マスタのデータセット   
    Private Cn As SqlConnection = New SqlConnection                         'コネクション
    Public CnStr As String
    Private SQLCmd As SqlCommand = New SqlClient.SqlCommand                 'SQLコマンド実行用
    Public Overridable Property AllowSorting As Boolean
    Dim mclsDB As New ClsSQLSvrDB
    Dim objStack As StackFrame
    Dim sitemaster05 As sitemaster05
    Dim clsExc As New ClsCMExclusive


#End Region

#Region "コンストラクタ"

    Public Sub New()
        'サーバ接続情報の格納
        Dim ctsSetting As ConnectionStringSettings
        ctsSetting = ConfigurationManager.ConnectionStrings("SPCDB")
        Cn = New SqlConnection(ctsSetting.ConnectionString)
        Cn.ConnectionString = ctsSetting.ConnectionString
        SQLCmd.Connection = Cn

        CnStr = ctsSetting.ToString
    End Sub

#End Region

#Region "SQL操作"

    '--------------------------------------------
    'データセットされたテーブルを返すメソッド
    '引数はSELECT文
    '--------------------------------------------
    Public Function getDataSetTable(ByVal StrSelSQL As String, ByVal TableName As String)

        Try

            SQLDS.Clear()
            SQLDA = New SqlClient.SqlDataAdapter(StrSelSQL, Cn)
            SQLDA.Fill(SQLDS, TableName)

        Catch ex As Exception

            '特に処理なし

        End Try

        Return SQLDS.Tables(TableName).Copy

    End Function

    '--------------------------------------------
    'SQLコマンド実行メソッド
    '--------------------------------------------
    Public Function SQLRes(ByVal SQLStr As String)

        Dim ErrorCheck As Boolean = False

        Try
            Cn.ConnectionString = CnStr
            SQLCmd.Connection = Cn
            SQLCmd.CommandText = SQLStr
            Cn.Open()
            SQLCmd.ExecuteNonQuery()
            Cn.Close()
        Catch
            ErrorCheck = True
        End Try

        Return ErrorCheck
    End Function




    '--------------------------------------------
    'DB更新
    '--------------------------------------------
    ''' <summary>
    ''' DB更新
    ''' </summary>
    ''' <param name="SQL">SQL文</param>
    ''' <returns>コマンド実行で影響を受けた行数。失敗で-1が戻り値</returns>
    ''' <remarks></remarks>
    Public Function DB_Update(ByVal SQL As String)
        Try
            SQLCmd.CommandText = SQL
            Cn.Open()
            Return SQLCmd.ExecuteNonQuery
        Catch ex As Exception
            Return -1
        Finally
            Cn.Close()
        End Try
    End Function


    '--------------------------------------------
    '指定したレコードの値を取得
    '--------------------------------------------
    ''' <summary>
    ''' 指定したレコードの値を取得
    ''' </summary>
    ''' <param name="SQL">SQL SELECT文</param>
    ''' <returns>取得した値。失敗で-1が戻り値</returns>
    ''' <remarks></remarks>
    Public Function GetRecord(ByVal SQL As String)
        Try
            SQLCmd.CommandText = SQL
            Cn.Open()
            Return SQLCmd.ExecuteScalar()

        Catch ex As Exception
            Return -1
        Finally
            Cn.Close()
        End Try


    End Function


    ''' <summary>
    ''' 指定した画面IDの表示上限件数を取得
    ''' </summary>
    ''' <param name="DispID">画面ID</param>
    ''' <param name="Search">使用する上限 True：SEARCH LIMIT, False：DEFAULT LIMIT</param>
    ''' <returns>表示上限値。無い場合0、失敗で-1</returns>
    ''' <remarks></remarks>
    Public Function GetDispLimit(ByVal DispID As String, ByRef Search As Boolean)

        If Search = False Then
            SQLCmd.CommandText = "SELECT M32_DEFAULT_MAX FROM M32_LISTLIMIT WHERE M32_DISP_CD = '" & DispID & "'"
        Else
            SQLCmd.CommandText = "SELECT M32_SEARCH_MAX FROM M32_LISTLIMIT WHERE M32_DISP_CD = '" & DispID & "'"
        End If

        Try
            Cn.Open()
            Return SQLCmd.ExecuteScalar()

        Catch ex As Exception
            Return -1
        Finally
            Cn.Close()
        End Try


    End Function

    ''' <summary>
    ''' レコード件数取得
    ''' </summary>
    ''' <param name="SQL">SQL SELECT文</param>
    ''' <returns>レコード件数 0件の場合0</returns>
    ''' <remarks></remarks>
    Public Function GetRecordCount(ByVal SQL As String)

        Try
            SQL = "SELECT COUNT(*) FROM( " + SQL + " ) AS T000"
            'SQL = "SELECT COUNT(*) " + SQL.Substring(SQL.LastIndexOf("FROM"))

            Return GetRecord(SQL)
        Catch ex As Exception
            Return -1
        End Try

    End Function

    ''' <summary>
    ''' SQLデータ操作　
    ''' </summary>
    ''' <param name="ippagPage" ></param>
    ''' <param name="EditCLS">処理の種別　追加/更新/削除のいづれか</param>
    ''' <param name="strSQL" >SQL</param>
    ''' <remarks></remarks>
    Public Function Btn_EditDBData(ByVal ippagPage As Page, ByVal EditCLS As String, ByVal MasterName As String, ByVal strSQL As String) As Boolean

        Btn_EditDBData = False

        Dim strMesType As String

        Select Case EditCLS
            Case "追加"
                strMesType = "00003"
            Case "更新"
                strMesType = "00001"
            Case "削除"
                strMesType = "00002"
            Case Else
                'strMesType = ""
                Exit Function
        End Select



        Try
            'DB接続
            If mclsDB.pfDB_Connect(CnStr) = False Then
                psMesBox(ippagPage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後)
                Exit Function
            End If

            'TRAN開始
            If mclsDB.pfDB_BeginTrans() = False Then
                '失敗メッセージ表示
                psMesBox(ippagPage, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Function
            End If

            'SQL実行
            If mclsDB.pfDB_ExecuteSQL(strSQL.ToString) > 0 Then
            Else
                '失敗メッセージ表示
                psMesBox(ippagPage, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Function
            End If

            'コミット
            If mclsDB.pfDB_CommitTrans() = False Then
                '失敗メッセージ表示
                psMesBox(ippagPage, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
                Exit Function
            End If

            '完了メッセージ表示
            psMesBox(ippagPage, strMesType, ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後, MasterName, MasterName)

            Btn_EditDBData = True

        Catch ex As Exception
            '失敗メッセージ表示
            psMesBox(ippagPage, strMesType, ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画後, MasterName)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", ippagPage.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")
        Finally
            'クローズ
            mclsDB.psDB_Close()
        End Try



    End Function

    ''' <summary>
    ''' 排他制御処理
    ''' </summary>
    ''' <param name="ippagPage"></param>
    ''' <param name="DispCode">画面ID</param>
    ''' <param name="arTableName">排他対象のテーブル名</param>
    ''' <param name="arKey">排他対象のキーデータ</param>
    ''' <param name="ExclusiveDate"></param>
    ''' <param name="SESSION_GROUP_NUM"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function pfExcl(ByVal ippagPage As Page, ByVal DispCode As String, _
                   ByVal arTableName As ArrayList, ByVal arKey As ArrayList, ByVal ExclusiveDate As String, _
                   ByVal SESSION_GROUP_NUM As String)

        pfExcl = ExclusiveDate

        '★排他制御処理
        Dim strExclusiveDate As String = Nothing
        'Dim arTable_Name As New ArrayList
        'Dim arKey As New ArrayList
        'Select Case ViewState(P_SESSION_TERMS)
        '    Case  ClsComVer.E_遷移条件.更新

        '★排他情報削除
        If Not ExclusiveDate = String.Empty Then

            If clsExc.pfDel_Exclusive(ippagPage _
                               , ippagPage.Session(P_SESSION_SESSTION_ID) _
                               , ExclusiveDate) = 0 Then
                pfExcl = String.Empty
            Else
                Exit Function
            End If
        End If

        '★ロック対象テーブル名の登録
        'arTable_Name.Insert(0, TableName)

        '★排他情報確認処理(更新画面へ遷移)
        If clsExc.pfSel_Exclusive(strExclusiveDate _
                         , ippagPage _
                         , ippagPage.Session(P_SESSION_IP) _
                         , ippagPage.Session(P_SESSION_PLACE) _
                         , ippagPage.Session(P_SESSION_USERID) _
                         , ippagPage.Session(P_SESSION_SESSTION_ID) _
                         , SESSION_GROUP_NUM _
                         , DispCode _
                         , arTableName _
                         , arKey) = 0 Then

            '★登録年月日時刻(明細)
            pfExcl = strExclusiveDate

        Else

            '排他ロック中

        End If

    End Function

    ''' <summary>
    ''' 排他情報削除
    ''' </summary>
    ''' <param name="ippagPage"></param>
    ''' <param name="DispCode">画面ID</param>
    ''' <param name="ExclusiveDate"></param>
    ''' <remarks></remarks>
    Public Function pfExclDel(ByVal ippagPage As Page, ByVal DispCode As String, ByVal ExclusiveDate As String)

        pfExclDel = ExclusiveDate

        '★排他情報削除
        If Not ExclusiveDate = String.Empty Then

            If clsExc.pfDel_Exclusive(ippagPage _
                               , ippagPage.Session(P_SESSION_SESSTION_ID) _
                               , ExclusiveDate) = 0 Then
                pfExclDel = String.Empty
            Else
                Exit Function
            End If
        End If

    End Function


    ''' <summary>
    ''' データ表示上限設定
    ''' </summary>
    ''' <returns>表示上限に対応したSQL文字列　失敗で空文字""</returns>
    ''' <param name="ippagPage"></param>
    ''' <param name="DispCode">画面ID</param>
    ''' <remarks></remarks>
    Public Function pfCheckDispLimit(ByVal ippagPage As Page, ByVal DispCode As String, ByVal strSQL As String, _
                                     ByVal SearchFlg As Boolean, Optional ByVal DispMesBox As Boolean = True, _
                                     Optional ByRef intRcdCount As Integer = 0) As String

        pfCheckDispLimit = "SELECT" + strSQL

        Try
            '該当件数表示 & 表示件数上限設定
            Dim RecordCount As Integer = GetRecordCount("SELECT" + strSQL)      '該当件数
            Dim DispLimit As Integer = GetDispLimit(DispCode, SearchFlg)        '表示上限
            intRcdCount = RecordCount

            Select Case CheckLimitOver(DispCode, RecordCount, SearchFlg)
                Case -1         'エラー
                    psMesBox(ippagPage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
                    pfCheckDispLimit = ""
                    Exit Function
                Case 0          '0件
                    psMesBox(ippagPage, "00004", ClsComVer.E_Mタイプ.警告, ClsComVer.E_S実行.描画後)
                Case 1          '上限オーバー
                    pfCheckDispLimit = "SELECT TOP(" & DispLimit & ")" + strSQL
                    psMesBox(ippagPage, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                            RecordCount.ToString, DispLimit.ToString)
                Case 2          '上限以内

                Case 3          '上限０件
                    pfCheckDispLimit = ""
                    psMesBox(ippagPage, "30005", ClsComVer.E_Mタイプ.情報, ClsComVer.E_S実行.描画後,
                                            RecordCount.ToString, "0")

            End Select
        Catch ex As Exception
            pfCheckDispLimit = ""
        End Try

    End Function


#End Region

#Region "ドロップダウンリスト初期化設定"


    ''' <summary>
    ''' ドロップダウンリスト初期化 "TextField"
    ''' </summary>
    ''' <param name="ddl">初期化するDropDownList</param>
    ''' <param name="tb">データソースにセットするデータリスト(テーブル)</param>
    ''' <param name="StrClmHead">ドロップダウンリストに表示するカラム名</param>
    ''' <remarks></remarks>
    Public Sub ddlInsert(ByVal ddl As DropDownList, ByVal tb As DataTable, _
                                 ByVal StrClmHead As String)

        ddl.Items.Clear()

        Dim rowCnt As Integer = 0
        Dim hsTable As New HashSet(Of String)

        '-----------------------------------
        Dim SortedTable As DataTable = tb.Clone()


        ' ソートされたデータビューの作成
        Dim dv As DataView = New DataView(tb)
        dv.Sort = StrClmHead

        ' ソートされたレコードのコピー
        For Each drv As DataRowView In dv
            SortedTable.ImportRow(drv.Row)
        Next
        '------------------------------------

        For Each R As DataRow In SortedTable.Rows
            ' 単語を追加。未登録なら追加
            If hsTable.Add(SortedTable.Rows(rowCnt)(StrClmHead).ToString) Then
                ddl.Items.Add(SortedTable.Rows(rowCnt)(StrClmHead).ToString)
            End If
            rowCnt = rowCnt + 1
        Next

        ddl.Items.Insert("0", "")

    End Sub


    ''' <summary>
    ''' ドロップダウンリスト初期化 "ValueField：TextField
    ''' </summary>
    ''' <param name="ddl">初期化するDropDownList</param>
    ''' <param name="tb">データソースにセットするデータリスト(テーブル)</param>
    ''' <param name="StrValFldClm">ValueFieldのカラム</param>
    ''' <param name="StrTxtFldClm">TextFieldのカラム</param>
    ''' <remarks></remarks>
    Public Sub ddlInsert(ByVal ddl As DropDownList, ByVal tb As DataTable, _
                          ByVal StrValFldClm As String, ByVal StrTxtFldClm As String)

        ddl.Items.Clear()

        Dim hsTable As New HashSet(Of String)
        Dim SortedTable As DataTable = tb.Clone()
        Dim rowCnt As Integer = 0

        ' ソートされたデータビューの作成
        Dim dv As DataView = New DataView(tb)
        dv.Sort = StrValFldClm

        ' ソートされたレコードのコピー
        For Each drv As DataRowView In dv
            SortedTable.ImportRow(drv.Row)
        Next

        '表示の調整(ID：Name）

        For Each R As DataRow In SortedTable.Rows
            ' 単語を追加。未登録なら追加
            If hsTable.Add(SortedTable.Rows(rowCnt)(StrValFldClm).ToString) And SortedTable.Rows(rowCnt)(StrValFldClm).ToString <> Nothing Then
                ddl.Items.Add(SortedTable.Rows(rowCnt)(StrValFldClm).ToString & "：" & SortedTable.Rows(rowCnt)(StrTxtFldClm).ToString)
            End If
            rowCnt = rowCnt + 1
        Next



        ddl.Items.Insert("0", "")

    End Sub



    ''' <summary>
    ''' ドロップダウンリスト M29_CLASS のセット
    ''' </summary>
    ''' <param name="ddl">初期化するDropDownList</param>
    ''' <param name="StrClassCd">区分マスタの分類</param>
    ''' <remarks></remarks>
    Public Sub ddlInsertM29(ByVal ddl As DropDownList, ByVal StrClassCd As String)

        ddl.Items.Clear()

        Dim rowCnt As Integer = 0
        Dim tb As New DataTable
        tb = getDataSetTable("SELECT * FROM M29_CLASS WHERE M29_CLASS_CD ='" & StrClassCd & "'", "TableM29")

        For Each R As DataRow In tb.Rows
            If tb.Rows(rowCnt)("M29_CLASS_CD").ToString = StrClassCd Then
                ddl.Items.Add(tb.Rows(rowCnt)("M29_CODE").ToString & "：" & tb.Rows(rowCnt)("M29_NAME").ToString)
            End If
            rowCnt += 1
        Next

        ddl.Items.Insert("0", "")
    End Sub

    ''' <summary>
    ''' グリッドビュー選択ボタンでドロップダウンリスト選択用文字列を返す(キーが一意)
    ''' </summary>
    ''' <param name="strClmKey">当マスタの検索カラム</param>
    ''' <param name="strClmOmstKey">元となるマスタの検索カラム</param>
    ''' <param name="strClmOmstName">元となるマスタの名前空間カラム</param>
    ''' <param name="strSQLSelect">データベースからデータを抽出するSQL文(1行のみの抽出となるような条件で記述）</param>
    ''' <returns>当マスタのアイテムと一致する、元となるマスタの名前空間を"："でくっついた文字列が戻り値となる</returns>
    ''' <remarks></remarks>
    Public Function ddlSelect _
        (ByVal strClmKey As String, ByVal strClmOmstKey As String, _
         ByVal strClmOmstName As String, ByVal strSQLSelect As String)

        Dim rtnStr As String = ""
        Dim i As Integer = 0

        '対象のテーブルコネクト
        Dim tb As New DataTable
        tb = getDataSetTable(strSQLSelect, "TableDdl")


        '対象の名前をコード検索で取得
        'For Each drow As DataRow In tb.Rows
        '    If tb.Rows(i)(strClmOmstKey).ToString = strClmKey Then
        '        rtnStr = tb.Rows(i)(strClmOmstName).ToString
        '        Exit For
        '    End If
        '    i += 1
        'Next

        'selectedvalueと同じ文字列を返す
        If tb.Rows.Count = 0 Then
            Return rtnStr
        Else
            Return strClmKey & "：" & tb.Rows(0)(strClmOmstName)
        End If

    End Function



#End Region

#Region "その他"


    ''' <summary>
    ''' Gridの選択ボタン
    ''' </summary>
    Public Sub GridSelect(ByVal page As Page, ByVal arColumn As ArrayList, ByVal arValue As ArrayList)



    End Sub

    Public Property ppCtrlValue(ByVal objCtrl As Object) As Object
        Get
            Select Case objCtrl.GetType.Name
                Case "DropDownList"
                    Return DirectCast(objCtrl, DropDownList).SelectedValue
                Case "TextBox"
                    Return DirectCast(objCtrl, TextBox).Text
                Case "Label"
                    Return DirectCast(objCtrl, Label).Text
                Case "CheckBox"
                    Return DirectCast(objCtrl, CheckBox).Checked
                Case Else
                    Return -1
            End Select
        End Get
        Set(value As Object)
            Select Case objCtrl.GetType.Name
                Case "DropDownList"
                    DirectCast(objCtrl, DropDownList).SelectedValue = value
                Case "TextBox"
                    DirectCast(objCtrl, TextBox).Text = value
                Case "Label"
                    DirectCast(objCtrl, Label).Text = value
                Case "CheckBox"
                    DirectCast(objCtrl, CheckBox).Checked = value
                Case Else
            End Select
        End Set
    End Property


    ''' <summary>
    ''' コントロール種類を判定しの値orIDを返す
    ''' </summary>
    ''' <param name="objCtrl">対象のコントロール</param>
    ''' <param name="getID">True:IDの文字列を返す</param>
    ''' <param name="useIndex">ドロップダウンリストを指定した時にIndexを返すか否か</param>
    Public ReadOnly Property Value_or_ID(ByVal objCtrl As Object, ByVal getID As Boolean, Optional ByVal useIndex As Boolean = False)
        Get

            Select Case getID
                Case True
                    'IDの文字列を返す
                    Return objCtrl.ID.ToString
                Case Else
                    'コントロールの種別判定
                    If TypeOf objCtrl Is TextBox Or TypeOf objCtrl Is Label Then

                        Return TryCast(objCtrl, TextBox).Text

                    ElseIf TypeOf objCtrl Is DropDownList Then
                        If useIndex = True Then
                            Return TryCast(objCtrl, DropDownList).SelectedIndex
                        Else
                            Return TryCast(objCtrl, DropDownList).SelectedValue
                        End If
                    Else
                        Return ""
                    End If
            End Select

        End Get
    End Property



    ''' <summary>
    ''' SQL条件文生成補助、値がある時はSQL文を返し、値が空の時に指定した文字列を返す　デフォルト""
    ''' </summary>
    ''' <param name="strColunm" >カラム名</param>
    ''' <param name="strValue">判定する文字列</param>
    ''' <param name="strSearchCls">検索パターン　完全/前方/後方/部分</param>
    ''' <param name="strCnjnc">接続詞 デフォルト:AND</param>
    ''' <param name="strAfter" >値が空の時返す文字列　初期値""</param>
    ''' <remarks></remarks>
    Public Function GeneStrSearch(ByVal strColunm As String, ByVal strValue As String, ByVal strSearchCls As String, _
                                     Optional ByVal strCnjnc As String = "AND", Optional ByVal strAfter As String = "") As String

        GeneStrSearch = strAfter

        'カラムが空で終了
        If strColunm.Trim = "" Then
            Exit Function
        End If

        Select Case strValue.Trim
            Case ""
                GeneStrSearch = strAfter
            Case Else
                'ANDの前のSPACEを変えてはいけません
                Select Case strSearchCls
                    Case "完全"
                        GeneStrSearch = " " & strCnjnc & " " & strColunm & " = '" & strValue & "' "
                    Case "前方"
                        GeneStrSearch = " " & strCnjnc & " " & strColunm & " LIKE '" & strValue & "%' "
                    Case "後方"
                        GeneStrSearch = " " & strCnjnc & " " & strColunm & " LIKE '%" & strValue & "' "
                    Case "部分"
                        GeneStrSearch = " " & strCnjnc & " " & strColunm & " LIKE '%" & strValue & "%' "
                End Select
        End Select

    End Function


    ''' <summary>
    ''' カラム名と値のリストからSQLの生成
    ''' </summary>
    ''' <returns>生成されたSQL</returns>
    ''' <param name="strSQLType">生成するSQLの種類 INSERT/UPDATE</param>
    ''' <param name="TableName">対象のテーブル名</param>
    ''' <param name="arColumn">カラム名を格納したArrayList</param>
    ''' <param name="arValue">値を格納した配列ArrayList</param>
    ''' <param name="strWhere">条件文</param>
    ''' <remarks>カラム名と値の数が一致しなければEXIT</remarks>
    Public Function GeneSQLCommand(ByVal strSQLType As String, ByVal TableName As String, _
                                   ByVal arColumn As ArrayList, ByVal arValue As ArrayList, Optional ByVal strWhere As String = "")

        GeneSQLCommand = ""

        If arColumn.Count <> arValue.Count Then
            Exit Function
        End If

        Dim stbColumn As New StringBuilder
        Dim stbValue As New StringBuilder

        Select Case strSQLType
            Case "WHERE"

                stbColumn.Clear()

                For i As Integer = 0 To arColumn.Count - 1
                    If arValue(i).ToString.Length < 5 Then
                        '値が空の時は検索しない
                    Else
                        Select Case arValue(i).ToString.Substring(0, 5)
                            Case "[CMP]" '完全一致
                                stbColumn.Append(GeneStrSearch(arColumn(i).Replace("[NUM]", "").Replace("[MNY]", "").Replace("[EMP]", ""), _
                                                               arValue(i).Substring(5), "完全"))
                            Case "[PRT]" '部分位置
                                stbColumn.Append(GeneStrSearch(arColumn(i).Replace("[NUM]", "").Replace("[MNY]", "").Replace("[EMP]", ""), _
                                                               arValue(i).Substring(5), "部分"))

                            Case "[LFT]" '前方一致
                                stbColumn.Append(GeneStrSearch(arColumn(i).ToString.Replace("[NUM]", "").Replace("[MNY]", "").Replace("[EMP]", ""), _
                                                               arValue(i).Substring(5), "前方"))

                            Case "[RGT]" '後方一致
                                stbColumn.Append(GeneStrSearch(arColumn(i).ToString.Replace("[NUM]", "").Replace("[MNY]", "").Replace("[EMP]", ""), _
                                                               arValue(i).Substring(5), "後方"))

                            Case Else
                                '上記以外は検索しない
                        End Select
                    End If
                Next

                GeneSQLCommand = stbColumn.ToString

            Case "INSERT"
                'INSERT文生成
                stbColumn.Clear()
                stbValue.Clear()
                stbColumn.Append(" INSERT INTO [SPCDB].[dbo]." & TableName & " ( ")
                stbValue.Append("  )SELECT ")

                For i As Integer = 0 To arColumn.Count - 1
                    If arValue(i).ToString = "" Then
                        If arColumn(i).ToString.Substring(0, 5) = "[EMP]" Then
                            stbColumn.Append(" ,")
                            stbValue.Append(" ,")

                            stbColumn.Append(arColumn(i).ToString.Substring(5))
                            stbValue.Append("''")
                        End If
                    Else
                        stbColumn.Append(" ,")
                        stbValue.Append(" ,")
                        Select Case arColumn(i).ToString.Substring(0, 5)
                            Case "[NUM]", "[MNY]"
                                stbColumn.Append(arColumn(i).ToString.Substring(5))
                                stbValue.Append(arValue(i))
                            Case "[EMP]"
                                stbColumn.Append(arColumn(i).ToString.Substring(5))
                                stbValue.Append("'" & arValue(i) & "'")
                            Case Else
                                stbColumn.Append(arColumn(i))
                                stbValue.Append("'" & arValue(i) & "'")
                        End Select
                    End If
                Next

                stbColumn.Replace("(  ,", "(  ")
                stbValue.Replace("SELECT  ,", "SELECT  ")

                GeneSQLCommand = stbColumn.ToString + stbValue.ToString

            Case "UPDATE"
                'UPDATE文生成
                stbColumn.Clear()
                stbColumn.Append(" UPDATE [SPCDB].[dbo]." & TableName & " ")
                stbColumn.Append(" SET-")

                For i As Integer = 0 To arColumn.Count - 1
                    If arValue(i).ToString = "" Then
                        If arColumn(i).ToString.Substring(0, 5) = "[EMP]" Then
                            '空文字
                            stbColumn.Append(GeneUPDStr(arColumn(i).ToString.Substring(5), arValue(i), ","))
                        Else
                            'NULL
                            stbColumn.Append(GeneUPDStr(arColumn(i).ToString, arValue(i), ",", "NULL", False))
                        End If
                    Else
                        Select Case arColumn(i).ToString.Substring(0, 5)
                            Case "[NUM]", "[MNY]"
                                stbColumn.Append(GeneUPDStr(arColumn(i).ToString.Substring(5), arValue(i), ",", False))
                            Case "[EMP]"
                                stbColumn.Append(GeneUPDStr(arColumn(i).ToString.Substring(5), arValue(i), ","))
                            Case Else
                                stbColumn.Append(GeneUPDStr(arColumn(i).ToString.Substring(0), arValue(i), ","))
                        End Select
                    End If


                Next

                stbColumn.Replace("SET- ,", "SET  ")
                GeneSQLCommand = stbColumn.ToString + strWhere

        End Select


    End Function

    ''' <summary>
    ''' 描写前処理
    ''' </summary>
    ''' <returns>成功:True 失敗:False</returns>
    ''' <param name="ippagPage"></param>
    ''' <param name="DispCode">画面ID</param>
    ''' <param name="DispMode"></param>
    ''' <param name="grvList">データバインドするグリッド</param>
    ''' <param name="TableName">テーブル名</param>
    ''' <param name="strSQL">SQL文</param>
    ''' <param name="strWHERE">SQL 条件文</param>
    ''' <param name="strVS_WHERE">ビューステートに保持されている条件文</param>
    ''' <param name="strSortKey">ソートかけるカラム名</param>
    ''' <param name="intDelCls">削除区分</param>
    ''' <param name="intRcdCount">該当件数が反映される 該当件数表示で必要</param>
    ''' <remarks></remarks>
    Public Function pfGetData(ByVal ippagPage As Page, ByVal DispCode As String, ByVal DispMode As String, _
                           ByVal grvList As GridView, _
                           ByVal TableName As String, _
                           ByVal strSQL As String, _
                           ByVal strWHERE As String, _
                           ByVal strVS_WHERE As String, _
                           ByVal strSortKey As String, _
                           ByVal intDelCls As Integer, _
                           Optional ByRef intRcdCount As Integer = 0) As Boolean

        '初期化
        pfGetData = False

        Try
            If strWHERE = "" Then '条件文未指定の場合
                Select Case intDelCls
                    Case 0 '削除無し

                    Case 1 '削除フラグ
                        strWHERE = " WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 " + strWHERE
                    Case 2 'DELETE

                End Select
            End If

            'GRIDの検索結果の保持
            Dim SRCH As String = ""             'SQL文格納
            Dim SearchFlg As Boolean = False    '検索判定用フラグ
            Dim DispMesBox As Boolean = True    'メッセージ表示の有無

            Select Case DispMode
                Case "First"        '初回表示
                    If intDelCls = 1 Then
                        SRCH = strSQL + " WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 "
                    Else
                        SRCH = strSQL
                    End If
                Case "ADD", "UPD"   '追加/更新/削除 完了後
                    SRCH = strSQL + strVS_WHERE
                    DispMesBox = False
                Case "Clear"        '検索条件初期化
                    If intDelCls = 1 Then
                        SRCH = strSQL + " WHERE " & TableName.Substring(0, 4) & "DELETE_FLG = 0 "
                    Else
                        SRCH = strSQL
                    End If
                    SearchFlg = True
                Case Else           '検索時
                    SRCH = strSQL + strWHERE
                    SearchFlg = True
            End Select


            '表示上限設定
            SRCH = pfCheckDispLimit(ippagPage, DispCode, SRCH, SearchFlg, DispMesBox, intRcdCount)
            If SRCH = "" Then
                '表示上限が0件の場合
                intRcdCount = 0
                'データバインド
                grvList.DataSource = New DataTable
                grvList.DataBind()
                Exit Function
            End If
            'ソート指定
            SRCH = SRCH + strSortKey
            'データ取得
            Dim dt As DataTable = getDataSetTable(SRCH, "view")

            'データバインド
            grvList.DataSource = dt
            grvList.DataBind()

            '成功
            pfGetData = True

        Catch ex As Exception
            'データの取得に失敗しました
            psMesBox(ippagPage, "00005", ClsComVer.E_Mタイプ.エラー, ClsComVer.E_S実行.描画前)
            'ログ出力
            objStack = New StackFrame
            psLogWrite("", ippagPage.User.Identity.Name, objStack.GetMethod.DeclaringType.Name,
                            objStack.GetMethod.Name, "", ex.ToString, "Catch")

        End Try



    End Function


    ''' <summary>
    ''' 上限オーバーの判定
    ''' </summary>
    ''' <param name="DispID">画面ID</param>
    ''' <param name="RecordCount">レコード件数</param>
    ''' <param name="Search">True：SEARCH LIMIT, False：DEFAULT LIMIT</param>
    ''' <returns>-1:エラー 0:該当レコード無し 1:上限オーバー 2:上限以下</returns>
    ''' <remarks></remarks>
    Public Function CheckLimitOver(ByVal DispID As String, ByVal RecordCount As Integer, ByRef Search As Boolean)

        Try

            Dim DispLimit As Integer = GetDispLimit(DispID, Search) '表示上限


            If RecordCount = -1 Or DispLimit = -1 Then              'エラー
                Return -1
            ElseIf RecordCount = 0 Then                             '該当レコード0件
                Return 0
            ElseIf DispLimit = 0 Then                               '上限レコード無し
                Return 3
            ElseIf RecordCount > DispLimit Then                     '上限オーバー
                Return 1
            Else
                Return 2                                            '上限以下
            End If
        Catch ex As Exception
            Return -1
        End Try


    End Function


    ''' <summary>
    ''' UPDATE文生成補助、値がある時はSQL文を返し、値が空の時に指定した文字列を返す　デフォルト""
    ''' </summary>
    ''' <param name="strColunm" >カラム名</param>
    ''' <param name="strValue">判定する文字列</param>
    ''' <param name="strHead">文頭に付ける文字列</param>
    ''' <param name="strAfter " >値が空の時返す文字列　初期値""</param>
    ''' <param name="strMode">値を文字列として扱うか否か 初期値:True</param>
    ''' <remarks>NULL入れる時は strMode=False</remarks>
    Public Function GeneUPDStr(ByVal strColunm As String, ByVal strValue As String, _
                               Optional ByVal strHead As String = "", Optional ByVal strAfter As String = "", _
                               Optional ByVal strMode As Boolean = True) As String

        Select Case strValue
            Case ""
                Select Case strMode
                    Case True
                        GeneUPDStr = " " & strHead & "  " & strColunm & " = '" & strAfter & "' "
                    Case Else
                        GeneUPDStr = " " & strHead & "  " & strColunm & " = " & strAfter & " "
                End Select

            Case Else
                Select Case strMode
                    Case True
                        GeneUPDStr = " " & strHead & "  " & strColunm & " = '" & strValue & "' "
                    Case Else
                        GeneUPDStr = " " & strHead & "  " & strColunm & " = " & strValue & " "
                End Select

        End Select

    End Function


    Public Sub psSetDropDownClassData(ByVal ddl As DropDownList, ByVal ClassCode As String)

        psDropDownDataBind(ddl, " SELECT M29_CODE AS VALUE, (M29_CODE + ' : ' + M29_NAME) AS TEXT FROM M29_CLASS WHERE M29_CLASS_CD = '" + ClassCode + "'  AND M29_DELETE_FLG   = '0' ", _
                           "TEXT", "VALUE")

    End Sub

    ''' <summary>
    ''' ドロップダウンリストのバインド
    ''' </summary>
    ''' <param name="DropDown">バインドするドロップダウンリスト</param>
    ''' <param name="strSQL" >データ取得用のSQL文字列</param>
    ''' <param name="DataTextField" >DataTextFieldに設定するフィールド名</param>
    ''' <param name="DataValueField">DataValueFieldに設定するフィールド名</param>
    ''' <param name="InsertEmptyItem">先頭に空の項目を追加するか否か　初期値:True</param>
    ''' <remarks></remarks>
    Public Sub psDropDownDataBind(ByVal DropDown As DropDownList, ByVal strSQL As String, ByVal DataTextField As String, _
                                  ByVal DataValueField As String, Optional ByVal InsertEmptyItem As Boolean = True)

        Try
            Dim ddlDataTable As New DataTable
            ddlDataTable = getDataSetTable(strSQL, "DropDown")
            DropDown.DataSource = ddlDataTable
            DropDown.DataTextField = DataTextField
            DropDown.DataValueField = DataValueField
            DropDown.DataBind()
            If InsertEmptyItem = True Then
                DropDown.Items.Insert(0, "")
            End If
        Catch ex As Exception

        End Try


    End Sub


    ''' <summary>
    ''' 文字列が空文字だった場合、指定した文字列を返す　デフォルト"NULL"
    ''' </summary>
    ''' <param name="strEmpty " >判定する文字列</param>
    ''' <param name="strAfter " >変換後の文字列　初期値"NULL"</param>
    ''' <remarks></remarks>
    Public Function pfCnvEmpStr(ByVal strEmpty As String, Optional ByVal strAfter As String = "NULL") As String

        pfCnvEmpStr = ConvString(strEmpty, "", strAfter)

    End Function


    ''' <summary>
    ''' 文字列を比較し、一致した場合、指定した文字列を返す
    ''' </summary>
    ''' <param name="str" >判定する文字列</param>
    ''' <param name="strExpression" >比較する文字列</param>
    ''' <param name="strAfter" >変換後の文字列</param>
    ''' <remarks></remarks>
    Public Function ConvString(ByVal str As String, ByVal strExpression As String, ByVal strAfter As String, Optional ByVal strFlsAfter As String = Nothing) As String

        ConvString = str
        If str = strExpression Then
            ConvString = strAfter
        Else
            If Not strFlsAfter Is Nothing Then
                ConvString = strAfter
            End If
        End If

    End Function



    Public Sub psContainerEnable(ByVal Container As Control, ByVal Enable As Boolean)

    End Sub

    ''' <summary>
    ''' コンテナ内の項目クリア
    ''' </summary>
    ''' <param name="Container" >対象のコンテナ(Panel,UpdatePanel のみ対応)</param>
    ''' <param name="ClearLabel" >ラベル項目も対象にするか否か　初期値True</param>
    ''' <remarks></remarks>
    Public Sub ClearControls(ByVal Container As Control, Optional ByVal ClearLabel As Boolean = True)

        Try
            'コンテナ内を検索
            For Each Ctrl As Control In Container.Controls

                'If TypeOf Ctrl Is TextBox Then
                '    TryCast(Ctrl, TextBox).Text = ""
                'ElseIf TypeOf Ctrl Is Label AndAlso ClearLabel = True Then
                '    TryCast(Ctrl, Label).Text = ""
                'ElseIf TypeOf Ctrl Is DropDownList Then
                '    'If TryCast(Ctrl, DropDownList).Enabled = True Then
                '    TryCast(Ctrl, DropDownList).SelectedIndex = -1
                '    'End If
                'ElseIf TypeOf Ctrl Is CheckBox Then
                '    TryCast(Ctrl, CheckBox).Checked = False
                'ElseIf TypeOf Ctrl Is RadioButton Then
                '    TryCast(Ctrl, RadioButton).Checked = False
                'ElseIf TypeOf Ctrl Is HiddenField Then
                '    TryCast(Ctrl, HiddenField).Value = ""
                'End If

                'コントロールの種別判別＆値のクリア
                Select Case True
                    Case TypeOf Ctrl Is TextBox
                        TryCast(Ctrl, TextBox).Text = ""
                    Case TypeOf Ctrl Is DropDownList
                        TryCast(Ctrl, DropDownList).SelectedIndex = -1
                    Case TypeOf Ctrl Is CheckBox
                        TryCast(Ctrl, CheckBox).Checked = False
                    Case TypeOf Ctrl Is RadioButton
                        TryCast(Ctrl, RadioButton).Checked = False
                    Case TypeOf Ctrl Is HiddenField
                        TryCast(Ctrl, HiddenField).Value = ""
                    Case TypeOf Ctrl Is Label AndAlso ClearLabel = True
                        TryCast(Ctrl, Label).Text = ""
                End Select

            Next

        Catch ex As Exception

        End Try

    End Sub


    ''' <summary>
    ''' コントロールの値のクリアと活性制御
    ''' </summary>
    ''' <param name="Ctrl">コントロール</param>
    ''' <param name="CtrlEnabled">コントロールの活性 or 非活性</param>
    ''' <param name="CtrlVisible">Visible</param>
    ''' <remarks></remarks>
    Public Sub ControlClear(ByVal Ctrl As Control, ByVal CtrlEnabled As Boolean, Optional ByVal CtrlVisible As Boolean = True)

        Try
            If TypeOf Ctrl Is TextBox Then
                TryCast(Ctrl, TextBox).Text = ""
                TryCast(Ctrl, TextBox).Enabled = CtrlEnabled
                TryCast(Ctrl, TextBox).Visible = CtrlVisible
            ElseIf TypeOf Ctrl Is DropDownList Then
                If TryCast(Ctrl, DropDownList).Enabled = True Then
                    TryCast(Ctrl, DropDownList).SelectedIndex = 0
                End If
                TryCast(Ctrl, DropDownList).Enabled = CtrlEnabled
                TryCast(Ctrl, DropDownList).Visible = CtrlVisible
            ElseIf TypeOf Ctrl Is CheckBox Then
                TryCast(Ctrl, CheckBox).Checked = False
                TryCast(Ctrl, CheckBox).Enabled = CtrlEnabled
                TryCast(Ctrl, CheckBox).Visible = CtrlVisible
            End If

        Catch ex As Exception

        End Try

    End Sub


    '--------------------------------------------
    '削除チェックボックスを判定、1か0を返すメソッド
    '--------------------------------------------
    ''' <summary>
    ''' 削除チェックボックスの戻り値を判定
    ''' </summary>
    ''' <param name="ChkObj">削除チェックボックス</param>
    ''' <returns>checked = Trueで"1"、Falseで"0"が戻り値となる</returns>
    ''' <remarks></remarks>
    Public Function GetDltCheckNum(ChkObj As CheckBox)
        Dim rtnStr As String
        If ChkObj.Checked Then
            rtnStr = "1"
        Else
            rtnStr = "0"
        End If
        Return rtnStr
    End Function

    '--------------------------------------------
    '削除の値を判定
    '--------------------------------------------
    ''' <summary>
    ''' 削除チェックボックスの戻り値を判定
    ''' </summary>
    ''' <param name="DelValue">DELETE_FLG</param>
    ''' <returns>DELETE_FLG='1'で"●"をかえす</returns>
    ''' <remarks></remarks>
    Public Function GrdVDelShow(ByVal DelValue As String)
        Dim returnStr As String
        If DelValue = "1" Then
            returnStr = "●"
        Else
            returnStr = ""
        End If
        Return returnStr
    End Function

    '--------------------------------------------
    '引数のテキストボックスが空白だとnull,その他はテキストを返すメソッド
    '--------------------------------------------
    ''' <summary>
    ''' 戻り値をNullとするか文字列とするか判定する
    ''' </summary>
    ''' <param name="txtbox">対象テキストボックス</param>
    ''' <returns>入力値が空白でNullが戻り値となる。</returns>
    ''' <remarks></remarks>
    Public Function SQLNullCheck(ByVal txtbox As TextBox)
        If txtbox.Text = "" Then
            Return Nothing
            Exit Function
        End If
        Return txtbox.Text
    End Function

    ''' <summary>
    ''' 戻り値をNullとするか文字列とするか判定する
    ''' </summary>
    ''' <param name="txtbox">対象ドロップダウンリスト</param>
    ''' <returns>入力値が空白でNullが戻り値となる。</returns>
    ''' <remarks></remarks>
    Public Function SQLNullCheck(ByVal txtbox As ClsCMTextBox)
        If txtbox.ppText = "" Then
            Return Nothing
            Exit Function
        End If
        Return txtbox.ppText
    End Function

    '--------------------------------------------
    '引数のドロップダウンボックスが空白だとnull,その他はテキストを返すメソッド
    '--------------------------------------------
    ''' <summary>
    ''' 戻り値をNullとするか文字列とするか判定する
    ''' </summary>
    ''' <param name="ddl">対象ドロップダウンリスト</param>
    ''' <param name="clnCheck">：より左側を返す処理を行う場合はTrue</param>
    ''' <returns>入力値が空白でNullが戻り値となる。</returns>
    ''' <remarks></remarks>
    Public Function SQLNullCheck(ByVal ddl As DropDownList, ByVal clnCheck As Boolean)

        Dim StrCheck As String = ddl.SelectedValue

        If StrCheck = "" Then
            Return Nothing
            Exit Function
        End If

        If clnCheck Then
            StrCheck = StrCheck.Substring(0, StrCheck.IndexOf("："))
        End If

        Return StrCheck

    End Function

#End Region


End Class
