window.onkeydown = function (e) {
    if (e.keyCode == 116) {
        //F5 キー押下
        e.keyCode = null; // キー押下を無効に
        return false
    }
}
