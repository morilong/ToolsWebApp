(function ($) {
    $.queryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        return r != null ? unescape(r[2]) : "";
    }

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
})(jQuery);


function setCookie(key, value) {
    $.cookie(key, value, { expires: 30 });
}

function setData1(str) {
    $("#txtData1").val(str);
}
function setData2(str) {
    $("#txtData2").val(str);
}

function getData1() {
    return $("#txtData1").val();
}
function getData2() {
    return $("#txtData2").val();
}

function checkBase64De() {
    var pattern = /^[A-Za-z0-9+/=]+$/;
    if (!pattern.test(getData1())) {
        setData2("您输入的不是有效的 Base64 字符串，无法解码。");
        return false;
    }
    return true;
}

function checkHexToStr() {
    var pattern = /^[0-9a-fA-F-]+$/;
    if (!pattern.test(getData1())) {
        setData2("您输入的不是有效的十六进制字符串，无法转换。");
        return false;
    }
    return true;
}

function checkBytesToStr() {
    var pattern = /(\d{1,3})[,]?/;
    if (!pattern.test(getData1())) {
        setData2("您输入的不是有效的字节序列，无法转换。");
        return false;
    }
    return true;
}

CodeEnDe = {
    action: "",
    data1: "",

    url_gb2312: function () {
        if (this.data1.length == 0) {
            return;
        }
        $.post("/code/UrlEncode", {
            str: this.data1,
            isDecode: (this.action == "de")
        }, function (data) {
            setData2(data);
        });
    },
    url_utf8: function () {
        if (this.data1.length == 0) {
            return;
        }
        var str;
        if (this.action == "en") {
            str = encodeURIComponent(this.data1);
        } else {
            str = decodeURIComponent(this.data1);
        }
        setData2(str);
    },

    base64_gb2312: function () {
        if (this.data1.length == 0) {
            return;
        }
        $.post("/code/Base64", {
            str: this.data1,
            charset: "GB2312",
            isDecode: (this.action == "de")
        }, function (data) {
            setData2(data);
        });
    },
    base64_utf8: function () {
        if (this.data1.length == 0) {
            return;
        }
        var str;
        try {
            if (this.action == "en") {
                str = $.base64.encode(this.data1);
            } else {
                str = $.base64.decode(this.data1);
            }
        } catch (e) {
            str = e;
        }
        setData2(str);
    },

    escape: function () {
        if (this.data1.length == 0) {
            return;
        }
        var str;
        if (this.action == "en") {
            str = escape(this.data1);
        } else {
            this.data1 = this.data1.replace(/\\u/g, "%u");
            str = unescape(this.data1);
        }
        setData2(str);
    }
};

CodeConvert = {
    data1: "",
    post: function (actionName, charset) {
        if (this.data1.length == 0) {
            return;
        }
        var postData = { str: this.data1 };
        if (charset != undefined && charset.length > 0) {
            postData = {
                str: this.data1,
                charset: charset
            };
        }
        $.post("/code/" + actionName, postData,
        function (data) {
            setData2(data);
        });
    },

    asciiToUnicode: function () {
        if (this.data1.length == 0) {
            return;
        }
        var res = "";
        for (var i = 0; i < this.data1.length; i++) {
            res += '&#' + this.data1.charCodeAt(i) + ';';
        }
        setData2(res);
    },
    unicodeToAscii: function () {
        if (this.data1.length == 0) {
            return;
        }
        var code = this.data1.match(/&#(\d+);/g);
        if (code == null) {
            setData2('没有合法的Unicode代码！');
            return;
        }
        for (var i = 0; i < code.length; i++) {
            this.data1 = this.data1.replace(code[i], String.fromCharCode(code[i].replace(/[&#;]/g, '')));
        }
        setData2(this.data1);
    }
};

CodeHash = {
    data1: "",
    post: function (actionName) {
        if (this.data1.length == 0) {
            return;
        }
        $.post("/code/" + actionName, {
            str: this.data1
        },
        function (data) {
            setData2(data);
        });
    }
};

function fnWork() {
    fnSaveData1();
    fnEnde();
    fnConvert();
    fnHash();
}

function fnExecute() {
    if (getData1().length == 0) {
        alert("请输入需要处理的内容！");
        $("#txtData1").focus();
        return;
    }
    fnWork();
}

function bindEvent() {
    $('#txtData1').keydown(function (e) {
        if (e.ctrlKey && e.which == 13) {
            fnExecute();
        }
    });

    $("#txtData1").on("postpaste", function () {
        fnExecute();
    }).pasteEvents();

    $("#btnExecute").click(function () {
        if ($.queryString("type").length == 0) {
            alert("请先选择要处理的类型！");
            return;
        }
        fnExecute();
    });

    fnWork();

    function fnSetCherset(id) {
        if (location.search.length == 0) {
            return;
        }
        var htmlCharset = $("#" + id).html();
        if (location.search.indexOf("charset=") < 0) {
            location.href = location.href + "&charset=" + htmlCharset;
        } else {
            var charset = $.queryString("charset");
            if (charset != null && charset.length > 0) {
                location.href = location.href.replace(charset, htmlCharset);
            } else {
                location.href = location.href.replace("charset=", "charset=" + htmlCharset);
            }
        }
        setCookieCodeCharset(htmlCharset);
    }
    $("#csUtf8").click(function () {
        fnSetCherset("csUtf8");
    });
    $("#csGb2312").click(function () {
        fnSetCherset("csGb2312");
    });
    $("#csUnicode").click(function () {
        fnSetCherset("csUnicode");
    });

    $("#jiaohuan").click(function () {
        var data1 = getData1();
        setData1(getData2());
        setData2(data1);
        fnSaveData1();
    });

    $("a").click(function () {
        fnSaveData1();
    });
}