function setCheck(nglistid, strageid, modelid, mes, mesno) {
    var ngList = document.getElementById(nglistid)
    var strageCd = document.getElementById(strageid).options.value;
    var modelCd = document.getElementById(modelid).options.value;

    if (!ngList) {
        return;
    }
    ngList.vise
    var ngString = "";

    for (var i = 0; i < ngList.rows.length; i++) {                                          //行位置取得
        if (strageCd == ngList.rows[i].cells[0].innerText) {                                //拠点一致
            if (parseInt(ngList.rows[i].cells[5].innerText) < parseInt(ngList.rows[i].cells[6].innerText)) {    //在庫不足

                //if ((modelCd == "3" && ngList.rows[i].cells[1].innerText == "04" ) || ( modelCd == "4" && ngList.rows[i].cells[1].innerText == "05")) {
                //if ((modelCd == "3" || modelCd == "4") && (ngList.rows[i].cells[1].innerText == "04" || ngList.rows[i].cells[1].innerText == "05")) {
                if ((modelCd == ngList.rows[i].cells[7].innerText) && (ngList.rows[i].cells[1].innerText == "04" || ngList.rows[i].cells[1].innerText == "05")) {

                    // プリンタ
                    ngString = ngStringAdd(ngString, ngList.rows[i].cells[4].innerText);    //不足機器名取得

                } else if (modelCd != "3" && modelCd != "4" && ngList.rows[i].cells[1].innerText != "04" && ngList.rows[i].cells[1].innerText != "05") {

                    // プリンタ以外
                    ngString = ngStringAdd(ngString, ngList.rows[i].cells[4].innerText);    //不足機器名取得

                }
            }
        }
    }

    //不足なし
    if (ngString == "") {
        return true;
    }

    //警告表示
    return vb_MsgExc_OC(mes.replace("{0}", ngString), mesno);

}

function ngStringAdd(str, add) {
    var rtn;
    if (str == "") {
        rtn = add;
    } else {
        rtn = str + "、" + add;
    }
    return rtn;
}