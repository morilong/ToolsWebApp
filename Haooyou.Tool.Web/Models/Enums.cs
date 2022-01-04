namespace Haooyou.Tool.Web.Models
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

    public enum EndeGroups
    {
        ende,
        convert,
        hash
    }

}
