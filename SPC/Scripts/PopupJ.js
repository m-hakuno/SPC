//エラーメッセージ_OK
function vb_MsgCri_O(mes,mesno) {
    alert(mesno + "\n" + mes);
    //    Call MsgBox(mesno & vbCrLf & mes, (vbOKOnly + vbCritical), mesno);
}

//警告メッセージ_OK
function vb_MsgExc_O(mes,mesno) {
    alert(mesno + "\n" + mes);
}

//情報メッセージ_OK
function vb_MsgInf_O(mes,mesno) {
    alert(mesno + "\n" + mes);
}


//エラーメッセージ_OKCancel
function vb_MsgCri_OC(mes, mesno) {
    return confirm(mesno + "\n" + mes)
}
//End Function

//警告メッセージ_OKCancel
function vb_MsgExc_OC(mes, mesno) {
    return confirm(mesno + "\n" + mes)
}
//End Function

//情報メッセージ_OKCancel
function vb_MsgInf_OC(mes, mesno) {
    return confirm(mesno + "\n" + mes)
}
//End Function

//ログアウト確認メッセージ
function vb_logout() {
    return confirm("ログアウトします。" + "\n" + "よろしいですか？")
}
//End Function

//更新確認メッセージ
function vb_Update() {
    return confirm("更新します。" + "\n" + "よろしいですか？")
}
//End Function

//登録確認メッセージ
function vb_Add() {
    return confirm("登録します。" + "\n" + "よろしいですか？")
}
//End Function

//削除確認メッセージ
function vb_Del() {
    return confirm("削除します。" + "\n" + "よろしいですか？")
}
//End Function
