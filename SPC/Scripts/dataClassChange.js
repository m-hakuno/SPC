function ddlDataClassChange(ddlID, pnlID1, pnlID2, pnlID3, pnlID4,
                            txtID1, txtID2, txtID3, txtID4,
                            btnID1, btnID2, btnID3, btnID4, btnID5,
                            val1,val2,val3,val4,val5) {
    //ＩＤのエレメント取得
    ddl_element = document.getElementById(ddlID);       //データ種別
    pnl1_element = document.getElementById(pnlID1);     //ＴＢＯＸ運用日
    pnl2_element = document.getElementById(pnlID2);     //ＢＢ１シリアル番号
    pnl3_element = document.getElementById(pnlID3);     //ＣＩＤ
    pnl4_element = document.getElementById(pnlID4);     //入金伝票番号

    txt1_element = document.getElementById(txtID1);      //ＴＢＯＸ運用日の日付ボックス
    txt2_element = document.getElementById(txtID2);      //ＢＢ１シリアル番号のテキストボックス
    txt3_element = document.getElementById(txtID3);      //ＣＩＤのテキストボックス
    txt4_element = document.getElementById(txtID4);      //入金伝票番号のテキストボックス
    
    btnID1_element = document.getElementById(btnID1);   //追加
    btnID2_element = document.getElementById(btnID2);   //クリア
    btnID3_element = document.getElementById(btnID3);   //照会
    btnID4_element = document.getElementById(btnID4);   //削除
    btnID5_element = document.getElementById(btnID5);   //更新

    val1_element = document.getElementById(val1);       //データ種別のValidator
    val2_element = document.getElementById(val2);       //ＴＢＯＸ運用日のValidator
    val3_element = document.getElementById(val3);       //ＢＢ１シリアル番号のValidator
    val4_element = document.getElementById(val4);       //ＣＩＤのValidator
    val5_element = document.getElementById(val5);       //入金伝票番号のValidator
                              
    //値取得
    var value = ddl_element.options[ddl_element.selectedIndex].value    //データ種別の選択値

    //照会テーブルの値初期化
    txt1_element.value = "";
    txt2_element.value = "";
    txt3_element.value = "";
    txt4_element.value = "";
    
    //照会テーブルの制御
    switch (value) {
        case "":
        case "27":
        case "29":
        case "86":
        case "ZZ":
            //全て非表示
            pnl1_element.className = "hidden";
            pnl2_element.className = "hidden";
            pnl3_element.className = "hidden";
            pnl4_element.className = "hidden";
            break;
        case "25":
            //ＣＩＤ、入金伝票番号を表示
            pnl1_element.className = "hidden";
            pnl2_element.className = "hidden";
            pnl3_element.className = "visible";
            pnl4_element.className = "visible";
            break;
        case "f1":
            //ＣＩＤを表示            
            pnl1_element.className = "hidden";
            pnl2_element.className = "hidden";
            pnl3_element.className = "visible";
            pnl4_element.className = "hidden";
            break;
        case "33":
            //ＴＢＯＸ運用日、ＢＢ１シリアル番号を表示
            pnl1_element.className = "visible";
            pnl2_element.className = "visible";
            pnl3_element.className = "hidden";
            pnl4_element.className = "hidden";
            break;
        default:
            //ＴＢＯＸ運用日を表示
            pnl1_element.className = "visible";
            pnl2_element.className = "hidden";
            pnl3_element.className = "hidden";
            pnl4_element.className = "hidden";
            break;
    }

    //ボタン制御
    if (btnID5_element.className == "hidden") {

        if (btnID1_element.isDisabled == false) {

            switch (value) {
                case "":
                    //リスト未選択
                    btnID1_element.className = "visible";
                    btnID2_element.className = "visible";
                    btnID3_element.className = "visible";
                    btnID4_element.className = "hidden";
                    btnID5_element.className = "hidden";
                    btnID1_element.disabled = true
                    btnID2_element.disabled = true
                    break;
                default:
                    //リスト選択
                    btnID1_element.className = "visible";
                    btnID2_element.className = "visible";
                    btnID3_element.className = "visible";
                    btnID4_element.className = "hidden";
                    btnID5_element.className = "hidden";
                    btnID1_element.disabled = false
                    btnID2_element.disabled = false
                    break;
            }
        } else {
            btnID1_element.disabled = false
            btnID2_element.disabled = false
        }
    }

}