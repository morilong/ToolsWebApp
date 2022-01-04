$(function () {
    var fgf = $.cookie("hb_fgf");
    if (fgf != null && fgf.length > 0) {
        $("#txtFgf").val(fgf);
    }
});

function mergeOrExclude(isExclude) {
    var data1 = $.trim($("#txtData1").val());
    var data2 = $.trim($("#txtData2").val());
    if (data1.length == 0) {
        alert("请填写文本1");
        $("#txtData1").val("");
        $("#txtData1").focus();
        return;
    }
    if (data2.length == 0) {
        alert("请填写文本2");
        $("#txtData2").val("");
        $("#txtData2").focus();
        return;
    }
    var s1Lines = data1.split(/\r\n|\n/);
    var s2Lines = data2.split(/\r\n|\n/);
    var count = s1Lines.length;
    var resArr = [];
    if (isExclude) {
        for (var i = 0; i < count; i++) {
            var s1 = s1Lines[i];
            if (s2Lines.indexOf(s1) == -1) {
                resArr.push(s1 + "\r\n");
            }
        }
    } else {
        if (s1Lines.length != s2Lines.length) {
            if (s1Lines.length > s2Lines.length) {
                count = s2Lines.length;
            }
        }
        var fgf = $("#txtFgf").val();
        for (var i = 0; i < count; i++) {
            resArr.push(s1Lines[i] + fgf + s2Lines[i] + "\r\n");
        }
    }

    $("#txtData3").val(resArr.join(""));
    setNum();
}

$("#btnMerge").click(function () {
    mergeOrExclude(false);
    setCookie("hb_fgf", fgf);
});

$("#btnExclude").click(function () {
    mergeOrExclude(true);
});