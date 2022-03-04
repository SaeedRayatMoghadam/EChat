////function sendPm(event) {
////    console.log("send");
////    event.preventDefault();

////    var pm = $("#pm").val();
////    connection.invoke("SendMessage", pm);
////}

$(document).ready(() => {
    if (Notification.permission !== "granted") {
        Notification.requestPermission();
    }
});
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

var currentGroupId = 0;
var userId = 0;

connection.on("welcome",
    function (id) {
        userId = id;
    });

connection.on("JoinGroup",
    (group, chats) => {
        $(".header").css("display", "block");
        $(".footer").css("display", "block");
        $(".header img").attr("src", `/images/groups/${group.imageUrl}`);
        $(".header h2").html(group.title);

        currentGroupId = group.id;

        $(".chats").html("");

        for (var i in chats) {
            var chat = chats[i];
            if (userId == chat.userId) {
                $(".chats").append(`
        <div class="chat-me">
            <div class="chat">
            <span class="pb-2">${chat.userName}</span>
                <p>${chat.body}</p>
                <span>${chat.createDate}</span>
            </div>
        </div>
        `);
            } else {
                $(".chats").append(`
        <div class="chat-you">
            <div class="chat">
                <span class="pb-2">${chat.userName}</span>
                <p>${chat.body}</p>
                <span>${chat.createDate}</span>
            </div>
        </div>
        `);
            }

        }
    });

connection.on("SendMessage", receive);

connection.on("NewGroup",
    (groupName, token, imageName) => {
        if (groupName == "Error")
            alert("an Error has occurred");
        else {

            $(".rooms #user-groups ul").append(`
            <li onclick="joinGroup('${token}')">
                ${groupName}
                <img src='/images/groups/${imageName}' />
                <span></span>
            </li>
    `);


            $("#exampleModal").modal({ show: false });
        }
    });

connection.on("SendNotification",
    (chat) => {
        if (Notification.permission === "granted") {
            if (currentGroupId !== chat.groupId) {
                var notif = new Notification(chat.groupName,
                    {
                        body: chat.body,
                    });
            }
        }
    });




function Start() {
    try {
        connection.start();
        $(".disconnect").hide();
    } catch (e) {
        $(".disconnect").show();
        setTimeout(Start, 5000);
    }
}

connection.onclose(Start);

Start();

function createChatGroup(event) {
    event.preventDefault();


    var groupName = event.target[1].value;
    var imageFile = event.target[2].files[0];


    var formData = new FormData();

    formData.append("GroupName", groupName);
    formData.append("ImageFile", imageFile);

    $.ajax({
        url: "/Home/CreateGroup",
        type: "post",
        data: formData,
        encyType: "multipart/form-data",
        processData: false,
        contentType: false
    });
}

function search() {
    var text = $("#search-input").val();

    if (text) {
        $("#search-result").show();
        $("#user-groups").hide();

        $.ajax({
            url: "/Home/Search?search=" + text,
            type: "get"
        }).done((data) => {
            $("#search-result ul").html("");
            for (var i in data) {
                if (data[i].isUser) {
                    console.log(data[i]);
                    $("#search-result ul").append(`
                            <li onclick="JoinPrivateGroup(${data[i].token})">
                                    ${data[i].title}
                                    <img src='/img/${data[i].imageUrl}' />
                                    <span></span>
                            </li>
                    `);
                } else {
                    $("#search-result ul").append(`
                             <li onclick="joinGroup('${data[i].token}')">
                                ${data[i].title}
                                 <img src='/images/groups/${data[i].imageUrl}' />
                                 <span></span>
                            </li>
                    `);
                }
            }
        });

    } else {
        $("#search-result").hide();
        $("#user-groups").show();
    }
}

function JoinPrivateGroup(receiverId) {
    connection.invoke("JoinPrivateGroup", receiverId, currentGroupId);
}

function joinGroup(token) {
    connection.invoke("JoinGroup", token, currentGroupId);
}

$("li[data-id]").click(() => {
    var token = $(this).Attr("data-id");
});


function sendPm(event) {
    event.preventDefault();

    var pm = $("#pm").val();
    if (pm)
        connection.invoke("SendMessage", pm, currentGroupId);
    else alert("Enter Your Message");
}

function receive(chat) {


    $("#pm").val("");

    if (userId == chat.userId) {
        $(".chats").append(`
        <div class="chat-me">
            <div class="chat">
                <span class="pb-2">${chat.userName}</span>
                <p>${chat.body}</p>
                <span>${chat.createDate}</span>
            </div>
        </div>
        `);
    } else {
        $(".chats").append(`
        <div class="chat-you">
            <div class="chat">
                <span class="pb-2">${chat.userName}</span>
                <p>${chat.body}</p>
                <span>${chat.createDate}</span>
            </div>
        </div>
        `);
    }
}