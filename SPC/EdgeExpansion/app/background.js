
var port = null;
chrome.runtime.onMessage.addListener(function(message, sender) {
  console.log(message);
  if (!port) {
    var hostName = "com.google.chrome.example.echo";
    port = chrome.runtime.connectNative(hostName);
    port.onMessage.addListener( function (rcv) {
      console.log('onNativeMessage');
      console.log(rcv);
      chrome.tabs.sendMessage(sender.tab.id ,rcv);
    });
    port.onDisconnect.addListener( function () {
      console.log('onDisconnect');
      port = null;
    });
  }
  port.postMessage({"text": message});
});

