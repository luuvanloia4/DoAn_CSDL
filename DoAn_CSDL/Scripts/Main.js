/* Constant error element */
const errorImagePath = "/Data/System/Images/img_error.png";
const True = true;
const False = false;
/* Validate */

var NonFunctionMessage = "Chức năng sẽ được ra mắt trong các bản cập nhật sau!";
var base64Dir = "data:image/jpeg;base64,";
function ShowPas(ele) {
    let input = $(ele).siblings('input');
    if ($(ele).hasClass("showedPas")) {
        $(ele).removeClass("showedPas");
        input.prop("type", "password");
    }
    else {
        $(ele).addClass("showedPas");
        input.prop("type", "text");
    }
}

var hasFocus;

function TryFocus(jqEle, msg) {
    jqEle.addClass("required");
    if (!hasFocus) {
        jqEle.focus();
        ShowFailMessage(msg);
        hasFocus = true;
    }
    jqEle.addClass("input-required");
}

function removeAscent(str) {
    if (str === null || str === undefined) return str;
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
}

function ValidTen(str) {
    let HoTen = removeAscent(str);
    let regex = /^[a-zA-Z]+( [a-zA-Z]+)+$/g;
    return regex.test(HoTen);
}

function ValidTaiKhoan(str) {
    let regex = /^[a-zA-Z0-9_]+$/g;
    return regex.test(str);
}

function ValidSDT(str) {
    let regex = /^[0-9]+$/g;
    return regex.test(str);
}

function AutoCorrectNumber(ele){
    console.log("Message of " + ele);
    let maxVal = parseInt($(ele).prop("max"));
    let minVal = parseInt($(ele).prop("min"));
    let curVal = parseInt($(ele).val());
    console.log(minVal + " to " + maxVal + " is " + curVal);
    if(isNaN(curVal)){
        curVal = isNaN(minVal)? 0: minVal;
        $(ele).val(curVal);
    }
    if(!isNaN(minVal)){
        if(minVal > curVal){
            $(ele).val(minVal);
        }
    }
    if(!isNaN(maxVal)){
        if(curVal > maxVal){
            $(ele).val(maxVal);
        }
    }
}

$(function () {
    $('input').change(function () {
        $(this).removeClass("input-required");
    });

    $('input[type=number]').change(function(ele){
        console.log("Message of " + this);
        let maxVal = parseInt($(this).prop("max"));
        let minVal = parseInt($(this).prop("min"));
        let curVal = parseInt($(this).val());
        console.log(minVal + " to " + maxVal + " is " + curVal);
        if(isNaN(curVal)){
            curVal = isNaN(minVal)? 0: minVal;
            $(this).val(curVal);
        }
        if(!isNaN(minVal)){
            if(minVal > curVal){
                $(this).val(minVal);
            }
        }
        if(!isNaN(maxVal)){
            if(curVal > maxVal){
                $(this).val(maxVal);
            }
        }
    });

    $('.custom-input-file input[type=file]').change(function () {
        if (this.files.length <= 1) {
            $(this).parent().find("input[type=text]").val($(this).val().split('\\').pop());
        }
        else {
            $(this).parent().find("input[type=text]").val(this.files.length + " files is choosed");
        }
    });
});

/* Check */
class MyString {
    static IsNullOrEmpty(str) {
        return (str == null || str == undefined || str.length == 0);
    }
}

class MyDateTime {
    static ParseDate(str) {
        return str.substring(0, 10);
    }
}

/* Convert */
function ParseDate(str) {
    //yyyy-MM-dd

    let year = str.substring(0, 4);
    let mounth = str.substring(5, 7);
    let day = str.substring(8, 10);

    return day + "/" + mounth + "/" + year;
}

function ParseHDate(str) {
    //yyyy-MM-dd 

    let year = str.substring(0, 4);
    let mounth = str.substring(5, 7);
    let day = str.substring(8, 10);

    let hourse = str.substring(11, 13);
    let minus = str.substring(14, 16);

    return hourse + ":" + minus + " " + day + "/" + mounth + "/" + year;

}

function ParseMSDate(str) {
    str = str.toString();
    let value = str.match(/(\d+)/)[0];
    console.log(value);
    let date = new Date(value - 0);

    return (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) + ":"
        + (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes()) + " "
        + ((date.getDate() < 10) ? "0" + date.getDate() : date.getDate()) + "/"
        + ((date.getMonth() + 1 < 10) ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) + "/"
        + date.getFullYear();
}

function ParseDateOfBirth(str){
    str = str.toString();
    let value = str.match(/(\d+)/)[0];
    let date = new Date(value - 0);

    return ((date.getDate() < 10) ? "0" + date.getDate() : date.getDate()) + "/"
        + ((date.getMonth() + 1 < 10) ? "0" + (date.getMonth() + 1) : date.getMonth() + 1) + "/"
        + date.getFullYear();
}

function PxConvert(str){
    try{
        let value = str.replace("px", "");
        return parseInt(value);
    }
    catch{
        return 0;
    }
}

function ShowImage(ele){
    let src = $(ele).prop("src");

    UIShowImage(src);
}

function ConvertFileToBase64(file, callback){
    let fileReader = new FileReader();
    //callback run when fileReader done!
    fileReader.onload = callback;

    fileReader.readAsDataURL(file);
}

//Data display
function RenderPageButton(page, maxPage) {
    let htmlButton = "";
    if (page > 1) {
        htmlButton += "<button type='button' class='btn' onclick='TransPage(1)'><i class='fas fa-angle-double-left'></i></button>";
        htmlButton += "<button type='button' class='btn' onclick='TransPage(" + (page - 1) + ")'><i class='fas fa-angle-left'></i></button>";
    }
    if (page > 3) {
        htmlButton += "<button type='button' class='btn'>...</i></button>";
    }
    for (let i = page - 2; i < page; i++) {
        if (i > 0) {
            htmlButton += "<button type='button' class='btn' onclick='TransPage(" + i + ")'>" + i + "</button>";
        }
    }
    htmlButton += "<button type='button' class='btn btn-page-active'>" + page + "</button>";
    for (let i = (page - 0) + 1; i <= (page - 0) + 2; i++) {
        if (i <= maxPage) {
            htmlButton += "<button type='button' class='btn' onclick='TransPage(" + i + ")'>" + i + "</button>";
        }
    }
    if (page < maxPage - 2) {
        htmlButton += "<button type='button' class='btn'>...</i></button>";
    }
    if (page < maxPage) {
        htmlButton += "<button type='button' class='btn' onclick='TransPage(" + ((page - 0) + 1) + ")'><i class='fas fa-angle-right'></i></button>";
        htmlButton += "<button type='button' class='btn' onclick='TransPage(" + maxPage + ")'><i class='fas fa-angle-double-right'></i></button>";
    }

    return htmlButton;
}