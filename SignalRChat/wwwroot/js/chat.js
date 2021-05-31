"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, date, reset) {
    if (reset == true) document.getElementById("messagesList").innerHTML = '';
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + "[" + date  + "]"+ " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveAllMessage", function (messages) {
    document.getElementById("messagesList").innerHTML = '';
    messages.forEach(item => {
        console.log(item)
        var msg = item.message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = item.user + "[" + item.date + "]" + " says " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


document.getElementById("loadButton").addEventListener("click", function (event) {
    connection.invoke("LoadMessage").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("loadAllButton").addEventListener("click", function (event) {
    connection.invoke("LoadAllMessage").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("resetButton").addEventListener("click", function (event) {
    connection.invoke("ResetMessage").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("intiButton").addEventListener("click", function (event) {
    connection.invoke("InitMessage").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//创建并启动连接。
//向“提交”按钮添加一个用于向中心发送消息的处理程序。
//向连接对象添加一个用于从中心接收消息并将其添加到列表的处理程序。