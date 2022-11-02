window.addEventListener("load", function(event) {
  var button=document.createElement("button");
  button.innerText = "ボタン";
  button.id = "btnExtContents";
  document.body.appendChild(button); 
  button.addEventListener("click", function() {
    window.sendMessage('click');
  });
}, false);

window.sendMessage = function sendMessage(message) {
  chrome.runtime.sendMessage(message); 
};

chrome.runtime.onMessage.addListener(function(message) {
  console.log('onMessage...');
  console.log(message);
  alert(JSON.stringify(message));
});