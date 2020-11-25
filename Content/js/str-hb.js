$(function () {
    var fgf = $.cookie("hb_fgf");
    if (fgf != null && fgf.length > 0) {
        $("#txtFgf").val(fgf);
    }
});

$("#btnMerge").click(function () {
    var data1 = $.trim($("#txtData1").val());
    var data2 = $.trim($("#txtData2").val());
    if (data1.length == 0) {
        alert("请填写待合并的账号1");
        $("#txtData1").val("");
        $("#txtData1").focus();
        return;
    }
    if (data2.length == 0) {
        alert("请填写待合并的账号2");
        $("#txtData2").val("");
        $("#txtData2").focus();
        return;
    }
    var s1Lines = data1.split(/\r\n|\n/);
    var s2Lines = data2.split(/\r\n|\n/);
    var count = s1Lines.length;
    if (s1Lines.length != s2Lines.length) {
        /*if (!confirm("账号1与账号2的行数不一样，是否合并？")) {
            return;
        }*/
        if (s1Lines.length > s2Lines.length) {
            count = s2Lines.length;
        }
    }
    var fgf = $("#txtFgf").val();
    var resArr = [];
    for (var i = 0; i < count; i++) {
        resArr.push(s1Lines[i] + fgf + s2Lines[i] + "\r\n");
    }
    $("#txtData3").val(resArr.join(""));
    setNum();

    setCookie("hb_fgf", fgf);
});