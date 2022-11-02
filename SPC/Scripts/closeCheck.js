window.onbeforeunload = function () {
    // ユーザ名コントロールによりログイン状態判別
    var element = document.getElementById("lnmLoginNm")
    var close_element = document.getElementById("lkbLogout")
    if (element) {
        //---------------------------------------
        // 2014/04/12 高松　ここから
        //---------------------------------------
        if (navigator.onLine && (event.clientY < 0 || event.altKey)) {
        //if (navigator.onLine && (event.clientX > (document.body.clientWidth * 0.97)) && (event.clientY < 0 || event.altKey)) {
            var close = document.getElementById("lkbLogout")
            //if (close != null) {
            //    close.focus();
            //}
            //return "画面上の閉じる、またはログアウト後に終了してください。";
            if (close != null) {
                if (close_element.textContent == "[ログアウト]") {
                    //window.open("http://172.27.2.247/SPC/Common/COMMENP001/COMMENP001.aspx", "", "toolbar=no,location=no");
                    //window.open("http://172.27.2.247/NGC/Common/COMMENP001/COMMENP001.aspx", "", "toolbar=no,location=no");
                    //window.open("http://172.27.2.247/WKB/Common/COMMENP001/COMMENP001.aspx", "", "toolbar=no,location=no");
                    window.open("http://127.0.0.1/Common/COMMENP001/COMMENP001.aspx", "", "toolbar=no,location=no");
                }
                else {
                    alert('×ボタンにて終了しました。\n終了処理を開始します');
                    var Excul_date = document.getElementById("hddExclusiveDate")
                    var Excul_date_dtl = document.getElementById("hddExclusiveDate_dtl")
                    var flag = 0
                    if (Excul_date.value == "") {
                        //何も無し
                    }
                    else if (Excul_date_dtl.value == "") {
                        flag = 0
                        //window.open("http://172.27.2.247/SPC/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value, "", "toolbar=no,location=no,width=0,height=0");
                        //window.open("http://172.27.2.247/NGC/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value, "", "toolbar=no,location=no,width=0,height=0");
                        //window.open("http://172.27.2.247/WKB/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value, "", "toolbar=no,location=no,width=0,height=0");
                        window.open("http://127.0.0.1/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value, "", "toolbar=no,location=no,width=0,height=0");
                    }
                    else {
                        flag = 1
                        //window.open("http://172.27.2.247/SPC/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value + "&Excul_date_dtl=" + Excul_date_dtl.value, "", "toolbar=no,location=no,width=0,height=0");
                        //window.open("http://172.27.2.247/NGC/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value + "&Excul_date_dtl=" + Excul_date_dtl.value, "", "toolbar=no,location=no,width=0,height=0");
                        //window.open("http://172.27.2.247/WKB/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value + "&Excul_date_dtl=" + Excul_date_dtl.value, "", "toolbar=no,location=no,width=0,height=0");
                        window.open("http://127.0.0.1/Exculsive_del.aspx" + "?flag=" + flag + "&Excul_date=" + Excul_date.value + "&Excul_date_dtl=" + Excul_date_dtl.value, "", "toolbar=no,location=no,width=0,height=0");
                    }

                }
            }
        }
        //---------------------------------------
        // 2014/04/12 高松　ここまで
        //---------------------------------------
    }
}