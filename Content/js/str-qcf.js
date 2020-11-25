function unique(arr) {
    var res = [], hash = {};
    var cf = [];
    for (var i = 0, elem; (elem = arr[i]) != null; i++) {
        if (!hash[elem]) {
            res.push(elem);
            hash[elem] = true;
        } else {
            cf.push(elem);
        }
    }
    return [res, cf];
}



//删掉重复的数组元素，返回去重复后数组
function delChongfu(arr, cfArr) {
    var i = 0;
    while (true) {
        if ($.inArray(arr[i], cfArr) > -1) {
            arr.splice(i, 1);
            i--;
        }
        if (i >= arr.length - 1) {
            break;
        }
        i++;
    }
    return arr;
}

$(function () {
    $("#btnUnique").click(function () {
        var data = $.trim($("#txtData1").val());
        if (data.length == 0) {
            alert("请输入要去重复的文本行！");
            $("#txtData1").focus();
            return;
        }
        var oldArr = data.split(/\r\n|\n/);
        var newArr = unique(oldArr);
        if ($("#chkDelCfh").prop("checked") == true) {
            var arr= delChongfu(newArr[0], newArr[1]);
            $("#txtData2").val(arr.join("\r\n"));
        } else {
            $("#txtData2").val(newArr[0].join("\r\n"));
        }
        $("#txtData3").val(newArr[1].join("\r\n"));
        setNum();
    });
});