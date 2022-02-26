$("#register-btn").click(function() {
    $("#login").slideUp();
    $("#register").fadeIn();
});

$("#login-btn").click(function () {
    $("#register").slideUp();
    $("#login").slideDown();
});

$(document).ready(function() {
    var route = location.href.split("#")[1];
    var route2 = location.href.split("/")[4];

    console.log(route);
    console.log(route2);

    if (route) {
        if (route == "register") {
            $("#login").slideUp();
            $("#register").fadeIn();
        } else {
            $("#register").slideUp();
            $("#login").slideDown();
        }
    }
    if (route2) {
        if (route2 == "register") {
            $("#login").slideUp();
            $("#register").fadeIn();
        } else {
            $("#register").slideUp();
            $("#login").slideDown();
        }
    }
});