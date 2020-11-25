$.fn.pasteEvents = function (delay) {
    if (delay == undefined) delay = 20;
    return $(this).each(function () {
        var $el = $(this);
        $el.on("paste", function () {
            $el.trigger("prepaste");
            setTimeout(function () { $el.trigger("postpaste"); }, delay);
        });
    });
};

$.fn.setCursorPosition = function (position) {
    if (this.lengh == 0) return this;
    return $(this).setSelection(position, position);
}
$.fn.setSelection = function (selectionStart, selectionEnd) {
    if (this.lengh == 0) return this;
    input = this[0];
    if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    } else if (input.setSelectionRange) {
        //input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    return this;
}
$.fn.focusEnd = function () {
    this.setCursorPosition(this.val().length);
}

function getNameMsg(name) {
    switch (name) {
        case "EvalError":
            return "非法调用 eval()";
        case "RangeError":
            return "数值越界";
        case "ReferenceError":
            return "非法或不能识别的引用数值";
        case "SyntaxError":
            return "发生语法解析错误";
        case "TypeError":
            return "操作数类型错误";
        case "URIError":
            return "URI处理函数使用不当";
        default:
            return "";
    }
}

function getErrorStr(e) {
    var msg = "错误信息：" + e.message + "\r\n" +
      "错误类型：" + e.name + "(" + getNameMsg(e.name) + ")\r\n" +
      "出错行数：" + (e.lineNumber == undefined ? "" : e.lineNumber);
    return msg;
}

function txtResultAppend(str) {
    var val = $("#txtResult").val();
    if (val.length > 0) {
        val += "------------------------------------------------------------------------\r\n";
    }
    val += str + "\r\n";
    $("#txtResult").val(val);
    $("#txtResult").focusEnd();
}

function doLoadCode() {
    var code = $("#txtCode").val();
    if (code.length == 0) {
        alert("请输入JS代码！");
        $("#txtCode").focus();
        return;
    }
    try {
        eval(code);
        txtResultAppend("代码加载成功！");
    } catch (e) {
        txtResultAppend(getErrorStr(e));
        return;
    }
    //获取设置函数列表
    var reg = /function[ ]+(([a-zA-Z_][a-zA-Z0-9_]{1,20})[ ]*\(.*?\))/g;
    var m, html = "", n = 0;
    while ((m = reg.exec(code)) != null) {
        html += '<option value="' + m[1] + '">' + m[2] + '</option>';
        n++;
    }
    $("#cmbFuncs").html(html);
    if (n == 1) {
        $("#txtFunc").val($("#cmbFuncs").val());
        $("#txtFunc").focus();
    } else {
        $("#cmbFuncs").get(0).selectedIndex = -1;
        $("#cmbFuncs").focus();
    }
}

$(function () {
    $("#txtCode").on("postpaste", function () {
        doLoadCode();
    }).pasteEvents();

    $("#btnFormat").click(function () {
        var source = $('#txtCode').val().replace(/^\s+/, '');
        if (source.length == 0)
            return;
        var fjs = js_beautify(source, 4, ' ');
        $("#txtCode").val(fjs);
    });

    $("#btnLoadCode").click(function () {
        doLoadCode();
    });

    $("#cmbFuncs").change(function () {
        $("#txtFunc").val($("#cmbFuncs").val());
    });

    $("#btnDoFunc").click(function () {
        try {
            var code = $("#txtCode").val();
            if (code.length == 0) {
                alert("请输入JS代码！");
                $("#txtCode").focus();
                return;
            }
            var funcName = $("#txtFunc").val();
            if (funcName.length == 0) {
                alert("请先选择或输入要执行的函数！");
                $("#txtFunc").focus();
                return;
            }
            var res = new Function(code + ' return ' + funcName);
            txtResultAppend("执行返回：" + res());
        } catch (e) {
            txtResultAppend(getErrorStr(e));
        }
        $("#txtFunc").focus();
    });

    $("#txtCode").focus();
});