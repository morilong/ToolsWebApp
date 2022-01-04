function randArray(data) {
    var arrlen = data.length;
    var try1 = [];
    for (var i = 0; i < arrlen; i++) {
        try1[i] = i;
    }
    var try2 = [];
    for (var i = 0; i < arrlen; i++) {
        try2[i] = try1.splice(Math.floor(Math.random() * try1.length), 1);
    }
    var try3 = [];
    for (var i = 0; i < arrlen; i++) {
        try3[i] = data[try2[i]];
    }
    return try3;
}

$("#btnUpset").click(function () {
    var data = $.trim($("#txtData1").val());
    if (data.length == 0) {
        alert("请输入要打乱的文本行！");
        $("#txtData1").focus();
        return;
    }
    var arr = data.split(/\r\n|\n/);
    arr = randArray(arr);
    $("#txtData2").val(arr.join("\r\n"));
    setNum();
});