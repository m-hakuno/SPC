'エラーメッセージ_OK
Function vb_MsgCri_O(mes, mesno)
    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno)
End Function

'警告メッセージ_OK
Function vb_MsgExc_O(mes, mesno)
    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbExclamation), mesno)
End Function

'情報メッセージ_OK
Function vb_MsgInf_O(mes, mesno)
    Call MsgBox(mesno & vbCrLf &mes, (vbOKOnly + vbInformation), mesno)
End Function

'エラーメッセージ_OKCancel
Function vb_MsgCri_OC(mes, mesno)
    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbCritical), mesno)
        Case vbOK
            vb_MsgCri_OC = True
        Case Else
            vb_MsgCri_OC = False
    End Select
End Function

'警告メッセージ_OKCancel
Function vb_MsgExc_OC(mes, mesno)
    Select Case MsgBox(mesno & vbCrLf &mes, (vbOKCancel + vbExclamation), mesno)
        Case vbOK
            vb_MsgExc_OC = True
        Case Else
            vb_MsgExc_OC = False
    End Select
End Function

'情報メッセージ_OKCancel
Function vb_MsgInf_OC(mes, mesno)
    Select Case MsgBox(mesno & vbCrLf &mes, (vbOK + vbInformation), mesno)
        Case vbOK
            vb_MsgInf_OC = True
        Case Else
            vb_MsgInf_OC = False
    End Select
End Function

'ログアウト確認メッセージ
Function vb_logout()
    Select Case MsgBox("ログアウトします。" & vbCrLf & "よろしいですか？", (vbOKCancel + vbExclamation), "ログアウト確認")
        Case vbOK
            vb_logout = True
        Case Else
            vb_logout = False
    End Select
End Function

'更新確認メッセージ
Function vb_Update()
    Select Case MsgBox("更新します。" & vbCrLf & "よろしいですか？", (vbOKCancel + vbExclamation), "更新確認")
        Case vbOK
            vb_Update = True
        Case Else
            vb_Update = False
    End Select
End Function

'登録確認メッセージ
Function vb_Add()
    Select Case MsgBox("登録します。" & vbCrLf & "よろしいですか？", (vbOKCancel + vbExclamation), "登録確認")
        Case vbOK
            vb_Add = True
        Case Else
            vb_Add = False
    End Select
End Function

'削除確認メッセージ
Function vb_Del()
    Select Case MsgBox("削除します。" & vbCrLf & "よろしいですか？", (vbOKCancel + vbExclamation), "登録確認")
        Case vbOK
            vb_Del = True
        Case Else
            vb_Del = False
    End Select
End Function
