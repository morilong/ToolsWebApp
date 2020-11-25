using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public enum EndeTypes
    {
        None,
        urlencode,
        base64,
        escape,
        gb2312ToUtf8,
        utf8ToGb2312,
        asciiToUnicode,
        unicodeToAscii,
        strToHex,
        hexToStr,
        strTobytes,
        bytesToStr,
        md5,
        sha1,
        crc32
    }
}