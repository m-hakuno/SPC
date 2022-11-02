//テーブル内のTextBoxにロストフォーカス時合計計算呼び出し設定
function setTableSum(myTblID, type) {
    var myTbl = document.getElementById(myTblID)
    if (!myTbl) {
        return;
    }
    for (var i = 0; i < myTbl.rows.length; i++) {                   //行位置取得
        for (var j = 0; j < myTbl.rows[i].cells.length; j++) {      //行内セル位置取得
            var Cells = myTbl.rows[i].cells[j];                     //i番行のj番列のセル "td"
            var input = Cells.getElementsByTagName("input");
            for (var k = 0; k < input.length; k++) {
                if (input[k].type == 'text') {                      //テキストボックス
                    input[k].onblur = function () { rowSum(myTbl, this, type); }
                    
                }
            }
        }
    }
}

//対象行取得
function rowSum(myTbl, cell, type) {
    for (var i = 0; i < myTbl.rows.length; i++) {                   //行位置取得
        for (var j = 0; j < myTbl.rows[i].cells.length; j++) {      //行内セル位置取得
            var Cells = myTbl.rows[i].cells[j];                     //i番行のj番列のセル "td"
            var input = Cells.getElementsByTagName("input");
            for (var k = 0; k < input.length; k++) {
                if (input[k].id == cell.id) {                       //対象行のセル
                    switch (type) {
                        case 1:
                            dtlSum(myTbl, i, type);
                            break;
                        case 2:
                            storeSum(myTbl, i, type);
                            break;
                        case 3:
                            fSum(myTbl, i, type);
                            break;

                    }
                }
            }
        }
    }
}

//明細合計
function dtlSum(myTbl, rowno) {
    var sum = "";
    //工事前
    var old = myTbl.rows[rowno].cells[1].getElementsByTagName("input");
    for (var k = 0; k < old.length; k++) {
        if (old[k].type == "text" && old[k].value != "") {
            sum = Number(sum) + Number(old[k].value);
        }
    }
    //新合計
    var newtot = myTbl.rows[rowno].cells[4].getElementsByTagName("input");
    for (var k = 0; k < newtot.length; k++) {
        if (newtot[k].type == "text" && newtot[k].value != "") {
            sum = Number(sum) + Number(newtot[k].value);
        }
    }

    //撤去
    var rem = myTbl.rows[rowno].cells[3].getElementsByTagName("input");
    for (var k = 0; k < rem.length; k++) {
        if (rem[k].type == "text" && rem[k].value != "") {
            sum = Number(sum) - Number(rem[k].value);
        }
    }
    
    //合計
    var afttot = myTbl.rows[rowno].cells[6].getElementsByTagName("input");
    for (var k = 0; k < afttot.length; k++) {
        if (afttot[k].type == "text") {
            if (isNaN(sum)) {
                afttot[k].value = "";
            } else {
                afttot[k].value = sum;
            }
        }
    }
}


//店内設置明細１合計
function storeSum(myTbl, rowno) {
    var sum = "";
    //工事前
    var old = myTbl.rows[rowno].cells[1].getElementsByTagName("input");
    for (var k = 0; k < old.length; k++) {
        if (old[k].type == "text" && old[k].value != "") {
            sum = Number(sum) + Number(old[k].value);
        }
    }
    //新合計
    var newtot = myTbl.rows[rowno].cells[4].getElementsByTagName("input");
    for (var k = 0; k < newtot.length; k++) {
        if (newtot[k].type == "text" && newtot[k].value != "") {
            sum = Number(sum) + Number(newtot[k].value);
        }
    }

    //撤去
    var rem = myTbl.rows[rowno].cells[3].getElementsByTagName("input");
    for (var k = 0; k < rem.length; k++) {
        if (rem[k].type == "text" && rem[k].value != "") {
            sum = Number(sum) - Number(rem[k].value);
        }
    }

    //合計
    var afttot = myTbl.rows[rowno].cells[5].getElementsByTagName("input");
    for (var k = 0; k < afttot.length; k++) {
        if (afttot[k].type == "text") {
            if (isNaN(sum)) {
                afttot[k].value = "";
            } else {
                afttot[k].value = sum;
            }
        }
    }
}

//店内設置明細新設合計
function fSum(myTbl, rowno) {
    var sum = "";
    //新設Ｆ１
    var f1 = myTbl.rows[rowno].cells[1].getElementsByTagName("input");
    for (var k = 0; k < f1.length; k++) {
        if (f1[k].type == "text" && f1[k].value != "") {
            sum = Number(sum) + Number(f1[k].value);
        }
    }
    //新設Ｆ２
    var f2 = myTbl.rows[rowno].cells[2].getElementsByTagName("input");
    for (var k = 0; k < f2.length; k++) {
        if (f2[k].type == "text" && f2[k].value != "") {
            sum = Number(sum) + Number(f2[k].value);
        }
    }

    //新設Ｆ３
    var f3 = myTbl.rows[rowno].cells[3].getElementsByTagName("input");
    for (var k = 0; k < f3.length; k++) {
        if (f3[k].type == "text" && f3[k].value != "") {
            sum = Number(sum) + Number(f3[k].value);
        }
    }

    //新設Ｆ４
    var f4 = myTbl.rows[rowno].cells[4].getElementsByTagName("input");
    for (var k = 0; k < f4.length; k++) {
        if (f4[k].type == "text" && f4[k].value != "") {
            sum = Number(sum) + Number(f4[k].value);
        }
    }

    //新設合計
    var afttot = myTbl.rows[rowno].cells[5].getElementsByTagName("input");
    for (var k = 0; k < afttot.length; k++) {
        if (afttot[k].type == "text") {
            if (isNaN(sum)) {
                afttot[k].value = "";
            } else {
                afttot[k].value = sum;
            }
        }
    }

    sum = "";
    //撤去設Ｆ１
    f1 = myTbl.rows[rowno].cells[6].getElementsByTagName("input");
    for (var k = 0; k < f1.length; k++) {
        if (f1[k].type == "text" && f1[k].value != "") {
            sum = Number(sum) + Number(f1[k].value);
        }
    }
    //撤去設Ｆ２
    f2 = myTbl.rows[rowno].cells[7].getElementsByTagName("input");
    for (var k = 0; k < f2.length; k++) {
        if (f2[k].type == "text" && f2[k].value != "") {
            sum = Number(sum) + Number(f2[k].value);
        }
    }

    //撤去設Ｆ３
    f3 = myTbl.rows[rowno].cells[8].getElementsByTagName("input");
    for (var k = 0; k < f3.length; k++) {
        if (f3[k].type == "text" && f3[k].value != "") {
            sum = Number(sum) + Number(f3[k].value);
        }
    }

    //撤去設Ｆ４
    f4 = myTbl.rows[rowno].cells[9].getElementsByTagName("input");
    for (var k = 0; k < f4.length; k++) {
        if (f4[k].type == "text" && f4[k].value != "") {
            sum = Number(sum) + Number(f4[k].value);
        }
    }

    //撤去設合計
    afttot = myTbl.rows[rowno].cells[10].getElementsByTagName("input");
    for (var k = 0; k < afttot.length; k++) {
        if (afttot[k].type == "text") {
            if (isNaN(sum)) {
                afttot[k].value = "";
            } else {
                afttot[k].value = sum;
            }
        }
    }

    sum = "";
    //工事後Ｆ１
    f1 = myTbl.rows[rowno].cells[11].getElementsByTagName("input");
    for (var k = 0; k < f1.length; k++) {
        if (f1[k].type == "text" && f1[k].value != "") {
            sum = Number(sum) + Number(f1[k].value);
        }
    }
    //工事後Ｆ２
    f2 = myTbl.rows[rowno].cells[12].getElementsByTagName("input");
    for (var k = 0; k < f2.length; k++) {
        if (f2[k].type == "text" && f2[k].value != "") {
            sum = Number(sum) + Number(f2[k].value);
        }
    }

    //工事後Ｆ３
    f3 = myTbl.rows[rowno].cells[13].getElementsByTagName("input");
    for (var k = 0; k < f3.length; k++) {
        if (f3[k].type == "text" && f3[k].value != "") {
            sum = Number(sum) + Number(f3[k].value);
        }
    }

    //工事後Ｆ４
    f4 = myTbl.rows[rowno].cells[14].getElementsByTagName("input");
    for (var k = 0; k < f4.length; k++) {
        if (f4[k].type == "text" && f4[k].value != "") {
            sum = Number(sum) + Number(f4[k].value);
        }
    }

    //工事後合計
    afttot = myTbl.rows[rowno].cells[15].getElementsByTagName("input");
    for (var k = 0; k < afttot.length; k++) {
        if (afttot[k].type == "text") {
            if (isNaN(sum)) {
                afttot[k].value = "";
            } else {
                afttot[k].value = sum;
            }
        }
    }
}
