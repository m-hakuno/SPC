Imports System.IO


Public Class ClsMSTCommon

    ''' <summary>
    ''' メッセージタイプの記号を取得する。
    ''' </summary>
    ''' <param name="ipshtType">メッセージタイプ</param>
    ''' <returns>メッセージタイプの記号</returns>
    ''' <remarks></remarks>
    Public Function pfGet_MesType(ByVal ipshtType As Object) As String
        '    Public Shared Function pfGet_MesType(ByVal ipshtType As clscomver.E_Mタイプ) As String

        Select Case ipshtType
            Case ClsComVer.E_Mタイプ.エラー
                pfGet_MesType = "E"
            Case ClsComVer.E_Mタイプ.警告
                pfGet_MesType = "W"
            Case ClsComVer.E_Mタイプ.情報
                pfGet_MesType = "I"
            Case Else
                pfGet_MesType = String.Empty
        End Select
    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ)
        Dim strRtn As String = String.Empty
        pfGet_Mes = mfGet_MesXml(ipstrNo, ipshtType)
        'pfGet_Mes = strRtn.Replace(CChar(0x0D) & CChar(10), "\n").Replace(CChar(10), "\n").Replace(CChar(13), "\n")

    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ,
                                               ByVal ipstrPrm1 As String)
        Dim strRtn As String = String.Empty
        pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
                               ipstrPrm1)
        'pfGet_Mes = strRtn.Replace(CChar(13) & CChar(10), "\n").Replace(CChar(10), "\n").Replace(CChar(13), "\n")

    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ,
                                               ByVal ipstrPrm1 As String,
                                               ByVal ipstrPrm2 As String)
        Dim strRtn As String = String.Empty
        pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
                               ipstrPrm1, ipstrPrm2)
        'pfGet_Mes = strRtn.Replace(Chr(13) & Chr(10), "\n").Replace(Chr(10), "\n").Replace(Chr(13), "\n")

    End Function

    ''' <summary>
    ''' メッセージを取得する。
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <returns>取得したメッセージ</returns>
    ''' <remarks></remarks>
    Public Function pfGet_Mes(ByVal ipstrNo As String,
                                               ByVal ipshtType As ClsComVer.E_Mタイプ,
                                               ByVal ipstrPrm1 As String,
                                               ByVal ipstrPrm2 As String,
                                               ByVal ipstrPrm3 As String)

        Dim strRtn As String = String.Empty
        pfGet_Mes = String.Format(mfGet_MesXml(ipstrNo, ipshtType),
                               ipstrPrm1, ipstrPrm2, ipstrPrm3)
        'pfGet_Mes = strRtn.Replace(Chr(13) & Chr(10), "\n").Replace(Chr(10), "\n").Replace(Chr(13), "\n")

    End Function

    ''' <summary>
    ''' Xmlファイルからメッセージを取得する
    ''' </summary>
    ''' <param name="ipstrNo">取得するメッセージNo</param>
    ''' <param name="ipshtType">取得するメッセージType</param>
    ''' <returns>取得したメッセージ</returns>
    ''' <remarks></remarks>
    Private Function mfGet_MesXml(ByVal ipstrNo As String, ByVal ipshtType As ClsComVer.E_Mタイプ) As String
        Dim dtsXml As DataSet = New DataSet()
        Dim dtrSelect() As DataRow
        Dim strType As String
        '--------------------------------
        '2014/04/14 星野　ここから
        '--------------------------------
        objStackFrame = New StackFrame
        '--------------------------------
        '2014/04/14 星野　ここまで
        '--------------------------------

        strType = pfGet_MesType(ipshtType)

        Try
            dtsXml.ReadXml(HttpContext.Current.Server.MapPath("~/XML/Message.xml"))

            With dtsXml.Tables(0)
                dtrSelect = .Select("No = '" & ipstrNo & "' AND Type = '" & strType & "'")
                If dtrSelect.Length > 0 Then
                    mfGet_MesXml = dtrSelect(0).Item("Message").ToString
                Else
                    mfGet_MesXml = String.Empty
                End If

            End With
        Catch ex As Exception
            '--------------------------------
            '2014/04/14 星野　ここから
            '--------------------------------
            'ログ出力
            psLogWrite("", USER_NAME, objStackFrame.GetMethod.DeclaringType.Name,
                            objStackFrame.GetMethod.Name, "", ex.ToString, "Catch")
            '--------------------------------
            '2014/04/14 星野　ここまで
            '--------------------------------
            mfGet_MesXml = String.Empty
        End Try

    End Function

    ''' <summary>
    ''' メッセージボックスを表示するスクリプトを取得する。
    ''' </summary>
    ''' <param name="ipstrMes">表示するメッセージ</param>
    ''' <param name="ipstrTitle">表示するタイトル</param>
    ''' <param name="ipshtType">表示するタイプ</param>
    ''' <param name="ipshtMode">表示するモード</param>
    ''' <returns>メッセージボックスを表示するスクリプト</returns>
    ''' <remarks></remarks>
    Private Function mfGet_Script(ByVal ipstrMes As String,
                                         ByVal ipstrTitle As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtMode As ClsComVer.E_Mモード) As String
        Dim strScript As String

        strScript = String.Empty
        Select Case ipshtType
            Case ClsComVer.E_Mタイプ.エラー
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        strScript = "vb_MsgCri_O"
                    Case ClsComVer.E_Mモード.OKCancel
                        strScript = "vb_MsgCri_OC"
                End Select
            Case ClsComVer.E_Mタイプ.警告
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        strScript = "vb_MsgExc_O"
                    Case ClsComVer.E_Mモード.OKCancel
                        strScript = "vb_MsgExc_OC"
                End Select
            Case ClsComVer.E_Mタイプ.情報
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        strScript = "vb_MsgInf_O"
                    Case ClsComVer.E_Mモード.OKCancel
                        strScript = "vb_MsgInf_OC"
                End Select
        End Select

        If strScript = String.Empty Then
            Return String.Empty
        End If

        mfGet_Script = strScript & "('" & ipstrMes & "','" & ipstrTitle & "');"
    End Function

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行,
                                         ByVal ipstrPrm1 As String)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行,
                                         ByVal ipstrPrm1 As String,
                                         ByVal ipstrPrm2 As String)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示する。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrNo">表示するメッセージNo.</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <param name="ipstrPrm1">メッセージのパラメータ1</param>
    ''' <param name="ipstrPrm2">メッセージのパラメータ2</param>
    ''' <param name="ipstrPrm3">メッセージのパラメータ3</param>
    ''' <remarks></remarks>
    Public Sub psMesBox(ByVal ippagPage As Page,
                                         ByVal ipstrNo As String,
                                         ByVal ipshtType As ClsComVer.E_Mタイプ,
                                         ByVal ipshtRun As ClsComVer.E_S実行,
                                         ByVal ipstrPrm1 As String,
                                         ByVal ipstrPrm2 As String,
                                         ByVal ipstrPrm3 As String)
        Dim strMes As String
        Dim strMesID As String


        'メッセージID設定(メッセージタイプ & メッセージNo.)
        strMesID = pfGet_MesType(ipshtType) & ipstrNo

        strMes = pfGet_Mes(ipstrNo, ipshtType, ipstrPrm1, ipstrPrm2, ipstrPrm3)
        msSet_MesBox(ippagPage, strMes, strMesID, ipshtType, ipshtRun)

    End Sub

    ''' <summary>
    ''' メッセージを表示するScriptをHtmlに埋め込む。
    ''' </summary>
    ''' <param name="ippagPage">表示するページコントロール</param>
    ''' <param name="ipstrMes">表示するメッセージ</param>
    ''' <param name="ipstrMesID">メッセージID</param>
    ''' <param name="ipshtType">メッセージのタイプ</param>
    ''' <param name="ipshtRun">メッセージの表示タイミング</param>
    ''' <remarks></remarks>
    Public Sub msSet_MesBox(ByVal ippagPage As Page,
                                    ByVal ipstrMes As String,
                                    ByVal ipstrMesID As String,
                                    ByVal ipshtType As ClsComVer.E_Mタイプ,
                                    ByVal ipshtRun As ClsComVer.E_S実行)
        Dim strLoad As String
        Dim strScript As String
        Dim objStack As StackFrame
        Dim strVBSKey As String
        Dim strSKey As String

        If ipshtType = ClsComVer.E_Mタイプ.エラー Then
            'ログファイルへ出力
            objStack = New StackFrame(2)   '呼出元情報
            psLogWrite(ippagPage.Session.SessionID, ippagPage.User.Identity.Name,
                       objStack.GetMethod.DeclaringType.Name, objStack.GetMethod.Name,
                       "ERROR", ipstrMesID & ":" & ipstrMes)
        End If

        strScript = mfGet_Script(ipstrMes, ipstrMesID, ipshtType, ClsComVer.E_Mモード.OK)

        '使用していないスクリプトのキー設定
        strSKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "mes")
        Select Case ipshtRun
            Case ClsComVer.E_S実行.描画前
                strLoad = mfGet_MesScript(ipshtType, ClsComVer.E_Mモード.OK)
                '使用していないスクリプトのキー設定
                strVBSKey = pfGet_ScriptKey(ippagPage, ippagPage.GetType, "vbmes")
                'ページ読込前は外部ファイルが読み込まれないためVBスクリプトを発行する
                ScriptManager.RegisterClientScriptBlock(ippagPage, ippagPage.GetType, strVBSKey, strLoad, False)
                ScriptManager.RegisterClientScriptBlock(ippagPage, ippagPage.GetType, strSKey, strScript, True)
            Case ClsComVer.E_S実行.描画後
                ScriptManager.RegisterStartupScript(ippagPage, ippagPage.GetType, strSKey, strScript, True)
        End Select
    End Sub

    ''' <summary>
    ''' 使用していないスクリプトキーを取得
    ''' </summary>
    ''' <param name="ippagPage">埋め込むページ</param>
    ''' <param name="ipType">スクリプトの型</param>
    ''' <param name="ipstrKey">ベースとなるキー</param>
    ''' <returns>スクリプトキー</returns>
    ''' <remarks>ベースとなるキーに数値をつけて一意となるよう生成</remarks>
    Public Function pfGet_ScriptKey(ByVal ippagPage As Page,
                                           ByVal ipType As Type,
                                           ByVal ipstrKey As String) As String
        Dim intKeyNo = 0
        Dim strKey As String = ipstrKey

        '使用していないスクリプトのキー設定
        Do Until Not ippagPage.ClientScript.IsStartupScriptRegistered(ipType, strKey)
            strKey = ipstrKey & intKeyNo.ToString
            intKeyNo = intKeyNo + 1
        Loop

        Return strKey
    End Function

    ''' <summary>
    ''' メッセージボックスのVBScriptを取得する。
    ''' </summary>
    ''' <param name="ipshtType">表示するタイプ</param>
    ''' <param name="ipshtMode">表示するモード</param>
    ''' <returns>メッセージボックスを表示するVBスクリプト</returns>
    ''' <remarks>外部ファイル「popup.vbs」と同内容なため変更時はそちらも変更すること。</remarks>
    Private Function mfGet_MesScript(ByVal ipshtType As ClsComVer.E_Mタイプ,
                                            ByVal ipshtMode As ClsComVer.E_Mモード) As String
        Dim strScript As New StringBuilder

        'strScript.AppendLine("<script type=""text/VBScript"">")
        strScript.AppendLine("<script type=""text/javascript"">")
        Select Case ipshtType
            Case ClsComVer.E_Mタイプ.エラー
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        'strScript.AppendLine("Function vb_MsgCri_O(mes, mesno)")
                        'strScript.AppendLine("    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno)")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//エラーメッセージ_OK")
                        strScript.AppendLine("function vb_MsgCri_O(mes,mesno) {")
                        strScript.AppendLine("    alert(mesno + ""\n"" + mes);")
                        strScript.AppendLine("    //    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno);")
                        strScript.AppendLine("}")

                    Case ClsComVer.E_Mモード.OKCancel
                        'strScript.AppendLine("Function vb_MsgCri_OC(mes, mesno)")
                        'strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbCritical), mesno)")
                        'strScript.AppendLine("        Case vbOK")
                        'strScript.AppendLine("            vb_MsgCri_OC = True")
                        'strScript.AppendLine("        Case Else")
                        'strScript.AppendLine("            vb_MsgCri_OC = False")
                        'strScript.AppendLine("    End Select")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//エラーメッセージ_OKCancel")
                        strScript.AppendLine("function vb_MsgCri_OC(mes, mesno) {")
                        strScript.AppendLine("    Return confirm(mesno + ""\n"" + mes)")
                        strScript.AppendLine("}")
                        strScript.AppendLine("//End Function")

                End Select
            Case ClsComVer.E_Mタイプ.警告
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        'strScript.AppendLine("Function vb_MsgExc_O(mes, mesno)")
                        'strScript.AppendLine("    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbExclamation), mesno)")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//警告メッセージ_OK")
                        strScript.AppendLine("function vb_MsgExc_O(mes,mesno) {")
                        strScript.AppendLine("    alert(mesno + ""\n"" + mes);")
                        strScript.AppendLine("}")

                    Case ClsComVer.E_Mモード.OKCancel
                        'strScript.AppendLine("Function vb_MsgExc_OC(mes, mesno)")
                        'strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbExclamation), mesno)")
                        'strScript.AppendLine("        Case vbOK")
                        'strScript.AppendLine("            vb_MsgExc_OC = True")
                        'strScript.AppendLine("        Case Else")
                        'strScript.AppendLine("            vb_MsgExc_OC = False")
                        'strScript.AppendLine("    End Select")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//警告メッセージ_OKCancel")
                        strScript.AppendLine("function vb_MsgExc_OC(mes, mesno) {")
                        strScript.AppendLine("    Return confirm(mesno + ""\n"" + mes)")
                        strScript.AppendLine("}")
                        strScript.AppendLine("//End Function")
                End Select
            Case ClsComVer.E_Mタイプ.情報
                Select Case ipshtMode
                    Case ClsComVer.E_Mモード.OK
                        'strScript.AppendLine("Function vb_MsgInf_O(mes, mesno)")
                        'strScript.AppendLine("    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbInformation), mesno)")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//情報メッセージ_OK")
                        strScript.AppendLine("function vb_MsgInf_O(mes,mesno) {")
                        strScript.AppendLine("    alert(mesno + ""\n"" + mes);")
                        strScript.AppendLine("}")
                    Case ClsComVer.E_Mモード.OKCancel
                        'strScript.AppendLine("Function vb_MsgInf_OC(mes, mesno)")
                        'strScript.AppendLine("    Select Case MsgBox(mesno & vbCrLf &mes, (vbOK + vbInformation), mesno)")
                        'strScript.AppendLine("        Case vbOK")
                        'strScript.AppendLine("            vb_MsgInf_OC = True")
                        'strScript.AppendLine("        Case Else")
                        'strScript.AppendLine("            vb_MsgInf_OC = False")
                        'strScript.AppendLine("    End Select")
                        'strScript.AppendLine("End Function")
                        strScript.AppendLine("//情報メッセージ_OKCancel")
                        strScript.AppendLine("function vb_MsgInf_OC(mes, mesno) {")
                        strScript.AppendLine("    Return confirm(mesno + ""\n"" + mes)")
                        strScript.AppendLine("}")
                        strScript.AppendLine("//End Function")
                End Select
        End Select
        strScript.AppendLine("</script>")

        If strScript.ToString = String.Empty Then
            Return String.Empty
        End If

        mfGet_MesScript = strScript.ToString
    End Function


End Class
