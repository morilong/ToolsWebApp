function setCookie(key, value) {
    $.cookie(key, value, { expires: 30 });
}

function getRegex() {
    var pattern = $("#txtPattern").val();
    if (pattern.length == 0) {
        alert("请输入正则表达式！");
        $("#txtPattern").focus();
        return null;
    }
    pattern = "/" + pattern + "/g";
    if ($("#chkIgnoreCase").val() == "on") {
        pattern += "i";
    }
    if ($("#chkMultiline").val() == "on") {
        pattern += "m";
    }
    var reg = eval(pattern);
    return reg;
}

function getSource() {
    var source = $("#txtSource").val();
    if (source.length == 0) {
        alert("请输入源文本！");
        $("#txtSource").focus();
        return null;
    }
    return source;
}

function doMatch() {
    var reg = getRegex();
    if (reg == null) return;
    var source = getSource();
    if (source == null) return;
    var m, th = null, tdArr = [], n = 0;
    while ((m = reg.exec(source)) != null) {
        var len = m.length;
        if (th == null) {
            th = "<tr><th></th>";
            for (var i = 0; i < len; i++) {
                th += "<th>" + i + "</th>";
            }
            th += "</tr>";
        }
        n++;
        var td = "<tr><td>" + n + "</td>";
        for (var i = 0; i < len; i++) {
            td += "<td>" + m[i] + "</td>";
        }
        td += "</tr>";
        tdArr.push(td);
    }
    var html = '<table class="table table-bordered table-hover" id="tbResult">' +
            th + tdArr.join("") + '</table>';
    $("#matchResult").html(html);
    saveConfig();
}

function bindEvent() {
    $('#txtPattern').keydown(function (e) {
        if (e.ctrlKey && e.which == 13) {
            doMatch();
        }
    });

    $("#btnMatch").click(function () {
        doMatch();
    });

    $("#btnReplace").click(function () {
        $(".col-replace").removeClass("col-replace");
        $(".col-pattern").removeClass("col-xs-12");
        $(".col-pattern").addClass("col-xs-6");

        var reg = getRegex();
        if (reg == null) return;
        var source = getSource();
        if (source == null) return;
        var res = source.replace(reg, $("#txtReplace").val());
        var html = '<textarea class="form-control txt-source" id="txtResult">' + res + '</textarea>';
        $("#matchResult").html(html);
        saveConfig();
    });
}