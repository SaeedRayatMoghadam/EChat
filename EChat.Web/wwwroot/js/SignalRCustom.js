////function sendPm(event) {
////    console.log("send");
////    event.preventDefault();

////    var pm = $("#pm").val();
////    connection.invoke("SendMessage", pm);
////}

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
                    $("#search-result ul").append(`
                            <li data-user-id='${data[i].token}'>
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