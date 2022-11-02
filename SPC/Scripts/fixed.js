//-----------------------------
//2014/05/07 土岐　ここから
//-----------------------------
//onloadイベント登録
if (window.attachEvent)
{
    window.attachEvent('onload', set_onloadscroll);
}
else if (window.addEventListener)
{
    window.addEventListener('load', function () {
        //ヘッダフッダ横スクロール
        var header = document.getElementById('header');
        var footer = document.getElementById('footer');
        fixed(header.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, header, '0');
        fixed(footer.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, footer, '0');

        //GridView横スクロール
        var elements = getElementsByClassNameIE(document, 'grid-in')
        for (var i = 0, il = elements.length; i < il; i++)
        {
            if (elements[i].className == 'grid-in')
            {
                elements[i].onscroll = gridscroll
            }
        }
    }, false)
}
//-----------------------------
//2014/05/07 土岐　ここまで
//-----------------------------

window.onresize = function () {
    //ヘッダフッダ横スクロール
    var header = document.getElementById('header');
    var footer = document.getElementById('footer');
    fixed(header.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, header, '0');
    fixed(footer.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, footer, '0');

    //GridView横スクロール
    var elements = getElementsByClassNameIE(document, 'grid-in')
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-in') {
            elements[i].onscroll = gridscroll
        }
    }
}

window.onscroll = function () {
    //ヘッダフッダ横スクロール
    var header = document.getElementById('header');
    var footer = document.getElementById('footer');
    fixed(header.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, header, '0');
    fixed(footer.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, footer, '0');
}

//window.onload = function () {
//}

//-----------------------------
//2014/06/06 間瀬　ここから
//-----------------------------
window.onfocusin = function () {
    //ヘッダフッダ横スクロール
    var header = document.getElementById('header');
    var footer = document.getElementById('footer');
    if (document.activeElement == null) {
        return
    }
    
    var ele = document.getElementById(document.activeElement.id)
    if (ele == null) {
        return
    }
    
    //ヘッダフッタのボタンを無視する
    if (document.activeElement.id == "lkbLogout") { return }
    if (document.activeElement.id == "btnLeft1") { return }
    if (document.activeElement.id == "btnLeft2") { return }
    if (document.activeElement.id == "btnLeft3") { return }
    if (document.activeElement.id == "btnLeft4") { return }
    if (document.activeElement.id == "btnLeft5") { return }
    if (document.activeElement.id == "btnLeft6") { return }
    if (document.activeElement.id == "btnLeft7") { return }
    if (document.activeElement.id == "btnLeft8") { return }
    if (document.activeElement.id == "btnLeft9") { return }
    if (document.activeElement.id == "btnLeft10") { return }
    if (document.activeElement.id == "btnRigth1") { return }
    if (document.activeElement.id == "btnRigth2") { return }
    if (document.activeElement.id == "btnRigth3") { return }
    if (document.activeElement.id == "btnRigth4") { return }
    if (document.activeElement.id == "btnRigth5") { return }
    if (document.activeElement.id == "btnRigth6") { return }
    if (document.activeElement.id == "btnRigth7") { return }
    if (document.activeElement.id == "btnRigth8") { return }
    if (document.activeElement.id == "btnRigth9") { return }
    if (document.activeElement.id == "btnRigth10") { return }
    if (document.activeElement.id == "lkbLogout") { return }

    //スクロール処理
    var bounds = ele.getBoundingClientRect();
    if (header.offsetTop + header.offsetHeight > bounds.top - 20 ) {
        window.scrollBy(0, bounds.top - 20 - (header.offsetTop + header.offsetHeight));
    } else
        if (footer == null) {
            window.scrollBy(0, bounds.height + 20 + bounds.top - 0);
        }
        else {
            if (footer.offsetTop < bounds.height + 20 + bounds.top) {
                window.scrollBy(0, bounds.height + 20 + bounds.top - footer.offsetTop);
        }
    }
}
//-----------------------------
//2014/06/06 間瀬　ここまで
//-----------------------------

//-----------------------------
//2014/05/07 土岐　ここから
//-----------------------------
function set_onloadscroll() {
    //-----------------------------
    //2014/05/07 土岐　ここまで
    //-----------------------------

    //GridView縦スクロール
    var elements = getElementsByClassNameIE(document, 'grid-in')
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-in') {
            elements[i].onscroll = gridscroll;
            set_scroll(elements[i])
        }
    }

    //DIV横スクロール
    var elements = getElementsByClassNameIE(document, 'grid-out')
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-out') {
            elements[i].onscroll = divscroll;
        }
    }
}


//GridViewスクロール
function divscroll() {
    var elements = getElementsByClassNameIE(document, 'grid-out');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-out') {
            elements[i].scrollLeft = 0;
        }
    }
}

//スクロール設定（現在地をセットする）
function set_scrolldata(panel) {
    var elements = getElementsByClassNameIE(panel, 'grid-data');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-data') {
            var scData = elements[i];
        }
    }
    scData.value = panel.scrollTop;
}

//スクロール設定（設定値までスクロールする）
function set_scroll(panel) {
    var elements = getElementsByClassNameIE(panel, 'grid-data');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-data') {
            var scData = elements[i];
        }
    }
    if (scData.value != '') {
        panel.scrollTop = scData.value;

        elements = getElementsByClassNameIE(document, panel.className);
        for (var i = 0, il = elements.length; i < il; i++) {
            if (elements[i].className == panel.className) {
                elements[i].scrollTop = scData.value;
            }
        }
    }
}


//GridViewスクロール
function gridscroll() {
    var dataW = this;
    var baseW = this.parentNode;
    var tgW
    var elements = getElementsByClassNameIE(dataW, 'grid-head');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-head') {
            tgW = elements[i];
        }
    }
    elements = dataW.getElementsByTagName('THEAD');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].tagName == 'THEAD') {
            tgW = elements[i];
        }
    }
    fixed(dataW.scrollWidth, baseW.clientWidth, dataW.scrollLeft, tgW, '0');

    set_scrolldata(dataW)

}

//固定行のスクロール
function fixed(dataWidth, baseWidth, scrollWidth, targetElement, syokichi) {
    if (baseWidth < dataWidth) {                            // Wrapperのサイズ指定
        var idou = syokichi - scrollWidth;                  // 初期位置からスクロール量分引く
        targetElement.style.marginLeft = idou + 'px';       // 新しい位置設定
    } else {
        targetElement.style.marginLeft = syokichi + 'px';   // ウィンドウサイズが戻ったときの処理
    }
}

//スクロール量取得
function getScrollPosition() {
    var obj = new Object();
    obj.scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;
    obj.scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    return obj;
}

//画面サイズ取得
function getScreenSize() {
    var obj = new Object();
    obj.scrollWidth = document.documentElement.clientWidth || document.body.clientWidth || document.body.scrollWidth;
    obj.scrollHeight = document.documentElement.clientHeight || document.body.clientHeight || document.body.scrollHeight;
    return obj;
}

function getElementsByClassNameIE(targetObj, targetClass) {
    var foundElements = new Array();
    if (targetObj.all) {
        var allElements = targetObj.all;
    }
    else {
        var allElements = targetObj.getElementsByTagName("*");
    }
    for (i = 0, j = 0; i < allElements.length; i++) {
        if (allElements[i].className == targetClass) {
            foundElements[j] = allElements[i];
            j++;
        }
    }
    return foundElements;
}