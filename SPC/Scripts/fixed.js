//-----------------------------
//2014/05/07 �y��@��������
//-----------------------------
//onload�C�x���g�o�^
if (window.attachEvent)
{
    window.attachEvent('onload', set_onloadscroll);
}
else if (window.addEventListener)
{
    window.addEventListener('load', function () {
        //�w�b�_�t�b�_���X�N���[��
        var header = document.getElementById('header');
        var footer = document.getElementById('footer');
        fixed(header.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, header, '0');
        fixed(footer.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, footer, '0');

        //GridView���X�N���[��
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
//2014/05/07 �y��@�����܂�
//-----------------------------

window.onresize = function () {
    //�w�b�_�t�b�_���X�N���[��
    var header = document.getElementById('header');
    var footer = document.getElementById('footer');
    fixed(header.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, header, '0');
    fixed(footer.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, footer, '0');

    //GridView���X�N���[��
    var elements = getElementsByClassNameIE(document, 'grid-in')
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-in') {
            elements[i].onscroll = gridscroll
        }
    }
}

window.onscroll = function () {
    //�w�b�_�t�b�_���X�N���[��
    var header = document.getElementById('header');
    var footer = document.getElementById('footer');
    fixed(header.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, header, '0');
    fixed(footer.scrollWidth, getScreenSize().scrollWidth, getScrollPosition().scrollLeft, footer, '0');
}

//window.onload = function () {
//}

//-----------------------------
//2014/06/06 �Ԑ��@��������
//-----------------------------
window.onfocusin = function () {
    //�w�b�_�t�b�_���X�N���[��
    var header = document.getElementById('header');
    var footer = document.getElementById('footer');
    if (document.activeElement == null) {
        return
    }
    
    var ele = document.getElementById(document.activeElement.id)
    if (ele == null) {
        return
    }
    
    //�w�b�_�t�b�^�̃{�^���𖳎�����
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

    //�X�N���[������
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
//2014/06/06 �Ԑ��@�����܂�
//-----------------------------

//-----------------------------
//2014/05/07 �y��@��������
//-----------------------------
function set_onloadscroll() {
    //-----------------------------
    //2014/05/07 �y��@�����܂�
    //-----------------------------

    //GridView�c�X�N���[��
    var elements = getElementsByClassNameIE(document, 'grid-in')
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-in') {
            elements[i].onscroll = gridscroll;
            set_scroll(elements[i])
        }
    }

    //DIV���X�N���[��
    var elements = getElementsByClassNameIE(document, 'grid-out')
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-out') {
            elements[i].onscroll = divscroll;
        }
    }
}


//GridView�X�N���[��
function divscroll() {
    var elements = getElementsByClassNameIE(document, 'grid-out');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-out') {
            elements[i].scrollLeft = 0;
        }
    }
}

//�X�N���[���ݒ�i���ݒn���Z�b�g����j
function set_scrolldata(panel) {
    var elements = getElementsByClassNameIE(panel, 'grid-data');
    for (var i = 0, il = elements.length; i < il; i++) {
        if (elements[i].className == 'grid-data') {
            var scData = elements[i];
        }
    }
    scData.value = panel.scrollTop;
}

//�X�N���[���ݒ�i�ݒ�l�܂ŃX�N���[������j
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


//GridView�X�N���[��
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

//�Œ�s�̃X�N���[��
function fixed(dataWidth, baseWidth, scrollWidth, targetElement, syokichi) {
    if (baseWidth < dataWidth) {                            // Wrapper�̃T�C�Y�w��
        var idou = syokichi - scrollWidth;                  // �����ʒu����X�N���[���ʕ�����
        targetElement.style.marginLeft = idou + 'px';       // �V�����ʒu�ݒ�
    } else {
        targetElement.style.marginLeft = syokichi + 'px';   // �E�B���h�E�T�C�Y���߂����Ƃ��̏���
    }
}

//�X�N���[���ʎ擾
function getScrollPosition() {
    var obj = new Object();
    obj.scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft;
    obj.scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    return obj;
}

//��ʃT�C�Y�擾
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