function ddlChange(ddlID, txtID1) {
    //ＩＤのエレメント取得
    ddl_element = document.getElementById(ddlID);
    txt1_element = document.getElementById(txtID1);

    //選択値取得
    var value = ddl_element.options[ddl_element.selectedIndex].text;
    var index = ddl_element.selectedIndex;

    switch (value) {
        case "99":
            //ホールＴＥＬの制御
            txt1_element.disabled = false;
            txt1_element.value = "";
            break;
        default:
            txt1_element.disabled = true;
            txt1_element.value = ddl_element.options[ddl_element.selectedIndex].value;
            break;
    }
}

//---2018/4/16 小野 設置環境写真用ボタン・テキストボックス追加
function rdioBtnChange(rdobtn, txtID1, txtID2, txtID3, txtID4, txtID5, txtID6, txtID7) {
   
       //ＩＤのエレメント取得
    txt1_element = document.getElementById(txtID1);
    txt2_element = document.getElementById(txtID2);
    txt3_element = document.getElementById(txtID3);
    txt4_element = document.getElementById(txtID4);
    txt5_element = document.getElementById(txtID5);
    txt6_element = document.getElementById(txtID6);
    txt7_element = document.getElementById(txtID7);

    //ラジオボタンの切り分け
    var value = rdobtn;

    //項目のクリア
    txt1_element.value = '';
    txt2_element.value = '';
    txt3_element.value = '';
    txt4_element.value = '';
    txt5_element.value = '';
    txt6_element.value = '';
    txt7_element.value = '';

    switch (value) {
        case "1":
            txt1_element.value    = 'N0010';
            txt1_element.disabled = false;
            txt2_element.disabled = false;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            txt5_element.disabled = true;
            txt6_element.disabled = true;
            txt7_element.disabled = true;
            break;
        case "2":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            txt5_element.disabled = true;
            txt6_element.disabled = true;
            txt7_element.disabled = true;
            break;
        case "3":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            txt5_element.disabled = true;
            txt6_element.disabled = true;
            txt7_element.disabled = true;
            break;
        case "4":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = false;
            txt4_element.disabled = true;
            txt5_element.disabled = true;
            txt6_element.disabled = true;
            txt7_element.disabled = true;
            break;
        case "5":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = true;
            txt4_element.disabled = false;
            txt5_element.disabled = true;
            txt6_element.disabled = true;
            txt7_element.disabled = true;
            break;
        case "6":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            txt5_element.disabled = false;
            txt6_element.disabled = true;
            txt7_element.disabled = true;
            break;
        case "7":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            txt5_element.disabled = true;
            txt6_element.disabled = false;
            txt7_element.disabled = true;
            break;
        case "8":
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            txt5_element.disabled = true;
            txt6_element.disabled = true;
            txt7_element.disabled = false;
            break;
        default:
    }
}
//---2018/4/16 小野 ここまで

//---2022/07/12
function rdioBtnChange2(rdobtn, txtID1) {

    //ＩＤのエレメント取得
    txt1_element = document.getElementById(txtID1);

    //ラジオボタンの切り分け
    var value = rdobtn;

    //項目のクリア
    txt1_element.value = '';

    switch (value) {
        case "1":
            txt1_element.disabled = false;
            break;
        case "2":
            txt1_element.disabled = true;
            break;
        case "3":
            txt1_element.disabled = true;
            break;
        case "4":
            txt1_element.disabled = true;
            break;
        case "5":
            txt1_element.disabled = true;
            break;
        case "6":
            txt1_element.disabled = true;
            break;
        case "7":
            txt1_element.disabled = true;
            break;
        case "8":
            txt1_element.disabled = true;
            break;
        default:
    }
}

function txtChange(flg, txt1, txt2, txt3, txt4) {
    //ＩＤのエレメント取得
    txt1_element = document.getElementById(txt1);
    txt2_element = document.getElementById(txt2);
    txt3_element = document.getElementById(txt3);
    txt4_element = document.getElementById(txt4);

    if (flg == "1" && txt1_element.value == "" && txt2_element.value == "")
    {
        flg = "3";
    }
    if (flg == "2" && txt3_element.value == "" && txt4_element.value == "")
    {
        flg = "3";
    }

    switch (flg) {
        case "1":
            //ホールＴＥＬの制御
            txt1_element.disabled = false;
            txt2_element.disabled = false;
            txt3_element.disabled = true;
            txt4_element.disabled = true;
            break;
        case "2":
            //ホールＴＥＬの制御
            txt1_element.disabled = true;
            txt2_element.disabled = true;
            txt3_element.disabled = false;
            txt4_element.disabled = false;
            break;
        default:
            //ホールＴＥＬの制御
            txt1_element.disabled = false;
            txt2_element.disabled = false;
            txt3_element.disabled = false;
            txt4_element.disabled = false;
            break;
    }
}