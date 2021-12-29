$(function () {
    HideLoading();

    $('.UI-cancel').click(function () {
        //Ẩn tất cả UIblock
        $('.UI-show').removeClass("UI-show");
    });
});

var loadingCount = 0;

function ShowUIContent(msg) {
    $('.UI-content .message').html(msg);
    $('.UI-content').addClass("UI-show");
    $('#UI-content').addClass("UI-show");
}

function UIContentToLoading() {
    $('#UI-content').removeClass("UI-show");
    $('#UI-loading').addClass("UI-show");
    loadingCount++;
}

function ShowLoading() {
    $('.UI-loading').addClass("UI-show");
    $('#UI-loading').addClass("UI-show");
    loadingCount++;
}

function HideLoading() {
    setTimeout(function(){
        $('.UI-loading').removeClass("UI-show");
        $('#UI-loading').removeClass("UI-show");
        loadingCount = 0;
    },
    300);
}
function ShowUIBlock(){
    $('.UI-loading').removeClass("UI-show");
    $('#UI-loading').addClass("UI-show");
    $('#UI-content').removeClass("UI-show");
}

function CheckLoadingCount() {
    loadingCount--;
    if (loadingCount == 0) {
        HideLoading();
    }
}

//Message script
var msgIndex = 0;

var msgPercenArray = new Array();
var msgProgressArray = new Array();
var msgCallbackArray = new Array();
var msgIntervalArray = new Array();

function MsgIncIndex() {
    msgIndex++;
    msgIndex = (msgIndex >= 20) ? 0 : msgIndex;
}

function RemoveMessage(index) {
    let id = "#msg" + index;
    $(id).remove();
    clearInterval(msgIntervalArray[index]);
    if (msgCallbackArray[index] != undefined && msgCallbackArray[index] != null) {
        let callback = msgCallbackArray[index];
        callback();
    }
}

function CooldownMessage(index) {
    let progressBar = msgProgressArray[index].children('.progress-bar');

    if (msgPercenArray[index] >= 0) {
        msgPercenArray[index] -= 2;
        progressBar.width(msgPercenArray[index] + "%");
    }
    else {
        RemoveMessage(index);
    }
}

function ShowSuccessMessage(msg, callback = function() {}) {
    let index = msgIndex;
    MsgIncIndex();

    RemoveMessage(index);

    let htmlMsg = "<div class='UI-success' onclick='RemoveMessage(" + index + ")' onmouseover='MouseOverMessage(" + index + ")' onmouseout='MouseOutMessage(" + index + ")' id='msg" + index + "'> "
        + "<div class='message__content'>"
        + "<div class='message-icon'>"
        + "<i class='fas fa-check-circle'></i>"
        + "</div>"
        + "<div class='message-text'>"
        + "<p>" + msg + "</p>"
        + "</div>"
        + "</div>"
        + "<div class='message__progress progress'>"
        + "<div class='progress-bar' style='width: 100%'></div>"
        + "</div>"
        + "</div>";
    $('.UI-message').append(htmlMsg);
    msgCallbackArray[index] = callback;

    //Set time loop
    msgPercenArray[index] = 100;
    let id = '#msg' + index;
    msgProgressArray[index] = $(id).children('.progress');
    msgIntervalArray[index] = setInterval(function () { CooldownMessage(index); }, 100);
}

function ShowFailMessage(msg, callback = function () { }) {
    let index = msgIndex;
    MsgIncIndex();

    RemoveMessage(index);

    let htmlMsg = "<div class='UI-fail' onclick='RemoveMessage(" + index + ")' onmouseover='MouseOverMessage(" + index + ")' onmouseout='MouseOutMessage(" + index + ")' id='msg" + index + "'> "
        + "<div class='message__content'>"
        + "<div class='message-icon'>"
        + "<i class='fas fa-times-circle'></i>"
        + "</div>"
        + "<div class='message-text'>"
        + "<p>" + msg + "</p>"
        + "</div>"
        + "</div>"
        + "<div class='message__progress progress'>"
        + "<div class='progress-bar' style='width: 100%'></div>"
        + "</div>"
        + "</div>";
    $('.UI-message').append(htmlMsg);
    msgCallbackArray[index] = callback;

    //Set time loop
    msgPercenArray[index] = 100;
    let id = '#msg' + index;
    msgProgressArray[index] = $(id).children('.progress');
    msgIntervalArray[index] = setInterval(function () { CooldownMessage(index); }, 100);
}

function MouseOverMessage(index) {
    clearInterval(msgIntervalArray[index]);
}
function MouseOutMessage(index) {
    msgIntervalArray[index] = setInterval(function () { CooldownMessage(index); }, 100);
}

//Hiden content
function ShowUIHiddenContent() {
    $('.UI-hidden').addClass('d-flex');
}

$(function () {
    $('.UI-hidden').click(function (event) {
        if (event.target !== this) {
            return;
        }
        $(this).removeClass("show");
    });

    $('.add-new-item').click(function(event){
        if(event.target !== this){
            return;
        }
        $(this).removeClass("active");
    });
});

//UI image
function UIShowImage(src){
    let UI_image = $('.UI-image');
    UI_image.find("img").prop("src", src);
    // console.log(UI_image.find("img"));
    // console.log(src);
    UI_image.addClass("active");
}
function UIHideImage(){
    $('.UI-image').removeClass("active");
}