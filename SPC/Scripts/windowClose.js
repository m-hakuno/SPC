function window_close(mes, mesno) {

    if (vb_MsgExc_OC(mes, mesno)) {
        (window.open('', '_self').opener = window).close();
    }
    return false;
}