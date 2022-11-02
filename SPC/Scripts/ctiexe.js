function exe_Cti(telno)
{

    //var cti_element = document.getElementById(txtID1);
    var obj = new ActiveXObject("WScript.Shell");
    var calltel = "C:\\ILiI\\CTIConnecTel\\Binn\\IC2CallTo.exe " + "callto:" + telno
    obj.Run(calltel);

}
