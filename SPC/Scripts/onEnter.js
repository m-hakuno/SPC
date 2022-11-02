//-----------------------------
//2014/05/07 土岐　ここから
//-----------------------------
//onloadイベント登録
if (window.attachEvent) {
    window.attachEvent('onload', set_onloadenter);
}

function set_onloadenter() {
    //-----------------------------
    //2014/05/07 土岐　ここまで
    //-----------------------------
    //コントロール取得
    var list = document.getElementsByTagName("input");
    for (var i = 0; i < list.length; i++) {
        if (list[i].type == 'submit' || list[i].type == 'button') {
            //ボタン系
            list[i].onkeydown = convertEnterToOnclick;
        } else {
            //その他Enterキー置換
            list[i].onkeydown = convertEnterToTab;
        }
    }

    //コントロール取得（ドロップダウンリスト）
    var list = document.getElementsByTagName("select");
    for (var i = 0; i < list.length; i++) {
        list[i].onkeydown = convertEnterToTab;
    }
}

//EnterキーをTabに置換
function convertEnterToTab(event) {
    if (window.event.keyCode == 13) {
        window.event.keyCode = 9;
    }
}

//Enterキーをonclick処理に置換
function convertEnterToOnclick(event) {
    if (window.event.keyCode == 13) {
        this.onclick;
    }
}