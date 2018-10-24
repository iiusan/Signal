"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var currentUser = "";
var toUser = "";
var contactTemplate = "<div class=\"contact\" guid=\"{u-guid}\">"+
                    "<div class=\"row\">"+
                        "<h4>{contact-first-name} {contact-last-name}</h4>"+
                            "<h5>{contact-email}</h5>" +
                    "</div>"+
                    "<div class=\"row remove\" hidden>"+
                        "<img src=\"/images/remove.png\" style=\"width:23px;\" />"+
                        "Remove conntact"+
                    "</div>"+
                    " </div>";

var adminTable;
var adminPag = 0;

function successfullAdd(d) {
    $("[data-dismiss='modal']").click();
}

function contactSuccessfullAdd(d) {
    var contact = contactTemplate.replace("{contact-first-name}", d.firstName).replace("{contact-last-name}", d.lastName).replace("{contact-email}", d.email).replace("{u-guidl}", d.id);
    $('#contacts-container').prepend(contact);
}

$("#msg-container").on("change", "div>textarea", function () {
    var txt = $(this).val();
    connection.invoke("UpdateMessageServer", $(this).attr('guid'), txt).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("UpdateMessageClient", function (message, id) {
    $('textarea').filter("[guid='" + id + "']").text(message);
    $('p').filter("[guid='" + id + "']").text(message);
});

function searchSuccessfull(d) {
    adminTable = d;
    buildPagination();
    //var contact = contactTemplate.replace("{contact-first-name}", d.firstName).replace("{contact-last-name}", d.lastName).replace("{contact-email}", d.email).replace("{u-guidl}", d.id);
    //$('#contacts-container').prepend(contact);
}

function buildPagination(){
    $('#pag').html("")
    $('#pag').append("<li class=\"page-item disabled\">"+
                        "<a class=\"page-link\" href=\"#\" tabindex=\"-1\">1</a>"+
        "</li>");
    for (var i = 1; i < adminTable.pagination; ++i) {
        var pag1 = "<li class=\"page-item\"><a class=\"page-link\" href=\"#\">{pag}</a></li>";
        pag1 = pag1.replace('{pag}', i + 1);
        $('#pag').append(pag1);
    }
    adminPag = 0;
    buildTable();
}

function buildTable() {
    $('#atable').html("");
    var r = 0;
    for (var i = adminPag * 5; i < 5 || r < 5; ++i) {
        try {
            var row = "<tr><td>{FirstName}</td><td>{LastName}</td><td>{Email}</td><td>{IsAdmin}</td></tr>";
            row = row.replace('{FirstName}', adminTable.userList[i].firstName).replace('{LastName}', adminTable.userList[i].lastName).replace('{Email}', adminTable.userList[i].email).replace('{IsAdmin}', adminTable.userList[i].isAdmin);
            $('#atable').append(row);
        }
        catch (err) { }
        r++;
    }
    $('.page-item').click(function () {
        $('.page-item').removeClass('disabled');
        $(this).addClass('disabled');
        adminPag = $(this).text() - 1;
        buildTable();
    });
}


$("#contacts-container").on("click", ".contact, .contact-visible", function () {
    var obj = $(this);
    var cont = $("#contacts-container>div");
    cont.removeClass("contact-visible");
    cont.addClass("contact");
    cont.children('.row.remove').hide();

    obj.removeClass("contact");
    obj.addClass("contact-visible");
    obj.children('.row.remove').show();

    $("#to-user-id").text(obj.attr('guid'));
    $('#msg-container').html("");
    getAllMessages();
});


function addpendMessage(user, message, id, tb) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    msg = (user != currentUser) ? "<div class=\"msg-left\"><p guid=\"{guid}\" class=\"txt-left\">" + msg + "</p></div>" : "<div class=\"msg-right\"><p guid=\"{guid}\"  class=\"txt-right\">" + msg + "</p></div>";
    msg = msg.replace("{guid}", id);

    if (tb == true) {
        msg = msg.replace("<p", "<textarea").replace("</p>", "</textarea>");
    }
    $('#msg-container').append(msg);
    var objDiv = document.getElementById("msg-container");
    objDiv.scrollTop = objDiv.scrollHeight;
}

function getAllMessages() {
    currentUser = $('#user-id').text();
    toUser = $('#to-user-id').text();
    connection.invoke("GetAllMessages", currentUser, toUser).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}



$('.page-item').click(function () {
    $('.page-item').removeClass('disabled');
    $(this).addClass('disabled');
    adminPag = $(this).text() - 1;
    buildTable();
});

connection.connectionClosed(function () {
    setTimeout(function () {
        $.connection.hub.start();
        console.log('reconnect');
    }, 5000); 
});

connection.on("ReceiveMessage", function (user, message, id, tb) {
    addpendMessage(user, message, id, tb);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    currentUser = $('#user-id').text();
    toUser = $('#to-user-id').text();
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", currentUser, toUser, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


