$(function () {
    var f1 = $.cookie("zh_format1");
    if (f1 != null && f1.length > 0) {
        $("#txtFormat1").val(f1);
    }
    var f2 = $.cookie("zh_format2");
    if (f2 != null && f2.length > 0) {
        $("#txtFormat2").val(f2);
    }
});

//传入格式字符串，返回全部分割符。
function getAllFgf(format) {
    var arr = [];
    var reg = /}(.*?){/g;
    var m;
    while ((m = reg.exec(format)) != null) {
        arr.push(m[1]);
    }
    return arr;
}

//传入格式字符串，返回全部索引。
function getAllIndex(format) {
    var arr = [];
    var reg = /{(\d+)}/g;
    var m;
    while ((m = reg.exec(format)) != null) {
        arr.push(m[1]);
    }
    return arr;
}

function format2NoFgf(dataLines, regFgf1, index2) {
    var resArr = [];
    for (var key in dataLines) {
        var columns = dataLines[key].split(regFgf1);
        resArr.push(columns[index2[0]-1]);
        resArr.push("\r\n");
    }
    return resArr;
}

$("#btnConvert").click(function () {
    var fgf1 = getAllFgf($("#txtFormat1").val());
    if (fgf1.length == 0) {
        alert("原账号格式输入错误，获取分割符失败。");
        return;
    }
    var data = $.trim($("#txtData1").val());
    if (data.length == 0) {
        alert("请填写待转换的账号！");
        $("#txtData1").focus();
        return;
    }
    var index2 = getAllIndex($("#txtFormat2").val());
    if (index2.length == 0) {
        alert("转换后格式输入错误，获取索引失败。");
        return;
    }
    var dataLines = data.split(/\r\n|\n/);
    //处理带转义字符的分隔符
    for (var i = 0; i < fgf1.length; i++) {
        fgf1[i] = fgf1[i].replace(/[|]/g, "[|]");
        fgf1[i] = fgf1[i].replace(/[$]/g, "[$]");
        fgf1[i] = fgf1[i].replace(/[*]/g, "[*]");
        fgf1[i] = fgf1[i].replace(/[?]/g, "[?]");
        fgf1[i] = fgf1[i].replace(/[+]/g, "[+]");
    }
    var reg = eval("/" + fgf1.join("|") + "/");
    var resArr = [];
    if (index2.length > 1) {
        var fgf2 = getAllFgf($("#txtFormat2").val());
        if (fgf2.length == 0) {
            alert("转换后格式输入错误，获取分割符失败。");
            return;
        }
        for (var key in dataLines) {
            var columns = dataLines[key].split(reg);
            for (var j = 0; j < index2.length; j++) {
                resArr.push(columns[index2[j] - 1]);
                if (j != index2.length - 1) {
                    resArr.push(fgf2[j]);
                }
            }
            resArr.push("\r\n");
        }
    } else {
        resArr = format2NoFgf(dataLines, reg, index2);
    }
    $("#txtData2").val(resArr.join(""));
    setNum();

    setCookie("zh_format1", $("#txtFormat1").val());
    setCookie("zh_format2", $("#txtFormat2").val());
});