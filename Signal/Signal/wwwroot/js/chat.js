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

function contactSuccessfullAdd(d) {
    var contact = contactTemplate.replace("{contact-first-name}", d.firstName).replace("{contact-last-name}", d.lastName).replace("{contact-email}", d.email).replace("{u-guidl}", d.id);
    $('#contacts-container').prepend(contact);
}

$("#msg-container").on("change", "div>textarea", function () {
   // console.error('change');
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
   // addpendMessage(currentUser, message, 0);//
    connection.invoke("SendMessage", currentUser, toUser, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


(function ($) {

    $.fn.filterByData = function (prop, val) {
        var $self = this;
        if (typeof val === 'undefined') {
            return $self.filter(
                function () { return typeof $(this).data(prop) !== 'undefined'; }
            );
        }
        return $self.filter(
            function () { return $(this).data(prop) == val; }
        );
    };

})(window.jQuery);


function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}