"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var lastUser = "";
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    //var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msg = message;
    //var encodedMsg = user + " says " + msg;
    var encodedMsg = "";
    if (user == document.getElementById("user").value) {
        encodedMsg =
            `<div class="outgoing_msg">
                <div class="sent_msg">
                    <span class="time_date"> ` + user + `</span>
                    <p>` + msg + `</p>
                </div>
            </div>`;
    } else {
        encodedMsg =
            `<div class="received_msg">
                <div class="received_withd_msg">
                    <span class="time_date">` + user + `</span>
                    <p>` + msg + `</p>
                </div>
             </div>`;
    }
    //var encodedMsg =
    //    `<div class="row"> <div class="col-md-5"></div> <div class="col-md-7" id="myMessage">` +
    //    user + " says " + msg + `</div> </div>`;
    var li = document.createElement("p");
    li.innerHTML = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    var block = document.getElementById("scrollT");
    block.scrollTop = 9999;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    //document.getElementById("sendButton").addEventListener("keyup", function (event) {
    //var user = document.getElementById("userInput").value;
    var user = document.getElementById("user").value;
    var a = document.getElementById("user");
    //var user = UserManager.GetUserName(User);
    var message = document.getElementById("messageInput").value;
    if (message === "")
        return;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    //if (event.keyCode === 13) {
    //    event.preventDefault();
    //    document.getElementById("sendButton").click();
    //}
    event.preventDefault();
    document.getElementById("messageInput").value = "";
    document.getElementById("messageInput").focus();
});

document.getElementById("messageInput").addEventListener("keyup", function (event) {
    event.preventDefault();
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
});