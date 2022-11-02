function isDate(obj) {
    if (obj != null) {
        // 年月日に分ける
        var str = obj.value;

        if (str.match("[0-9]{8}") == null) {
        } else {
            /* 8文字ならスラッシュを挿入 */
            if (obj.value.length == 8) {
                obj.value = str.slice(0, 4) + "/" + str.slice(4, 6) + "/" + str.slice(6, 8);
            }
        }
        //-----------------------------
        //2014/04/23 土岐　ここから
        //-----------------------------
        // 書式の確認 
        if (!obj.value.match
            (/^\d{4}\/([0]{0,1}[1-9]{1}|1[0-2]{1})\/([0]{0,1}[1-9]{1}|[1-2]{1}[0-9]{1}|3[0-1]{1})$/)) {
            return false;
        }
        var date = obj.value.split('/');
        var vYear = date[0];
        var vMonth = digitsPadding(date[1], 2);
        var vDay = digitsPadding(date[2], 2);

        obj.value = vYear + '/' + vMonth + '/' + vDay
        //-----------------------------
        //2014/04/23 土岐　ここまで
        //-----------------------------
    }
}

function isDateM(obj) {
    if (obj != null) {
        // 年月に分ける
        var str = obj.value;
        /* 6文字ならスラッシュを挿入 */
        //-----------------------------
        //2014/04/23 土岐　ここから
        //-----------------------------
        if (obj.value.match(/^\d{6}$/)) {
            //-----------------------------
            //2014/04/23 土岐　ここまで
            //-----------------------------
            obj.value = str.slice(0, 4) + "/" + str.slice(4, 6)
        }
        //-----------------------------
        //2014/04/23 土岐　ここから
        //-----------------------------
        // 書式の確認 
        if (!obj.value.match
            (/^\d{4}\/([0]{0,1}[1-9]{1}|1[0-2]{1})$/)) {
            return false;
        }
        var date = obj.value.split('/');
        var vYear = date[0];
        var vMonth = digitsPadding(date[1], 2);

        obj.value = vYear + '/' + vMonth
        //-----------------------------
        //2014/04/23 土岐　ここまで
        //-----------------------------
    }
}

function getYobi(date) {
    if (date != null) {
        // 曜日判定
        var myDay = new Array("SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT");   //配列オブジェクトを生成
        /* 10文字のみ曜日判定 */
        if (date.length == 10) {
            myY = date.slice(0, 4);  //年取得
            myM = date.slice(5, 7);  //月取得
            myD = date.slice(8, 10); //日取得

            var myDate = new Date(myY, myM - 1, myD);   //指定した時刻を表す日付オブジェクトを作成
            var myWeek = myDate.getDay();               //曜日の番号取得
            return myDay[myWeek];                       //番号から曜日の文字取得
        }
    }
    return "";
}

function setDate(obj, obj2) {
    // 年月日整形
    isDate(obj);
    if (obj2 != null && obj != null) {
        obj2.textContent = getYobi(obj.value);
    }
}

function setDateM(obj) {

    // 年月整形
    isDateM(obj);
}
//-----------------------------
//2014/04/23 土岐　ここから
//-----------------------------
function digitsPadding(num, max) {
    // 数字を0埋めmax桁にする
    num += "";

    while (num.length < max) {
        num = "0" + num;
    }
    return num;     
};

function setTime(obj) {
    if (obj.value.match(/^\d{1}$/)) {
        obj.value = digitsPadding(obj.value, 2)
    }
}
//-----------------------------
//2014/04/23 土岐　ここまで
//-----------------------------
