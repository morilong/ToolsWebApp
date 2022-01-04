function setCookie(key, value) {
    $.cookie(key, value, { expires: 30 });
}

function setNum() {
    var data = $.trim($("#txtData1").val());
    if (data.length > 0) {
        $("#txtNum1").val(data.split("\n").length);
    } else {
        $("#txtNum1").val(0);
    }
    data = $.trim($("#txtData2").val());
    if (data.length > 0) {
        $("#txtNum2").val(data.split("\n").length);
    } else {
        $("#txtNum2").val(0);
    }
    if ($("#txtData3").val() != undefined) {
        data = $.trim($("#txtData3").val());
        if (data.length > 0) {
            $("#txtNum3").val(data.split("\n").length);
        } else {
            $("#txtNum3").val(0);
        }
    }
}

$('textarea').bind('input propertychange', function () {
    setNum();
});

$('textarea').keyup(function (event) {
    //backspace、delete
    if (event.which == 8 || event.which == 46) {
        setNum();
    }
});

function downCheck(txtDataNo) {
    if ($("#txtData" + txtDataNo).val().length == 0) {
        alert("没有可保存的文本！");
        return false;
    }
    return true;
}