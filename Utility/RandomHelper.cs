using System;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 生成随机数辅助类
    /// </summary>
    public static class RandomHelper
    {
        private static int _rep;
        private static Random GetNewRandom()
        {
            long num = DateTime.Now.Ticks + _rep;
            _rep++;
            return new Random(((int)(((ulong)num) & 0xffffffffL)) | ((int)(num >> _rep)));
        }

        #region int Next(*)
        /// <summary>
        /// 返回非负随机数
        /// </summary>
        /// <returns></returns>
        public static int Next()
        {
            return GetNewRandom().Next();
        }
        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数。
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Next(int maxValue)
        {
            return GetNewRandom().Next(maxValue);
        }
        /// <summary>
        /// 返回一个指定范围内的随机数。
        /// </summary>
        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）。</param>
        /// <param name="maxValue">返回的随机数的上界（随机数不能取该上界值）。</param>
        /// <returns></returns>
        public static int Next(int minValue, int maxValue)
        {
            return GetNewRandom().Next(minValue, maxValue);
        }
        #endregion

        #region 返回一个介于 0.0 和 1.0 之间的随机数。 + double NextDouble()
        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数。
        /// </summary>
        public static double NextDouble()
        {
            return GetNewRandom().NextDouble();
        }
        #endregion

        #region string NextString(int count, RandomStringOptions options)

        /// <summary>
        /// 根据 RandomStringOptions 生成指定个数的随机字符串
        /// </summary>
        /// <param name="count">生成字符个数</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string NextString(int count, RandomStringOptions options)
        {
            if (count <= 0)
                throw new ArgumentException("必须大于0", "count");

            switch (options)
            {
                case RandomStringOptions.Integer:
                    return NextIntCount(count);
                case RandomStringOptions.IntEnglish:
                    return NextIntEnglishCount("0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ", count);
                case RandomStringOptions.IntEnglishLower:
                    return NextIntEnglishCount("0123456789abcdefghigklmnopqrstuvwxyz", count);
                case RandomStringOptions.IntEnglishUpper:
                    return NextIntEnglishCount("0123456789ABCDEFGHIGKLMNOPQRSTUVWXYZ", count);
                case RandomStringOptions.English:
                    return NextIntEnglishCount("abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ", count);
                case RandomStringOptions.EnglishLower:
                    return NextIntEnglishCount("abcdefghigklmnopqrstuvwxyz", count);
                case RandomStringOptions.EnglishUpper:
                    return NextIntEnglishCount("ABCDEFGHIGKLMNOPQRSTUVWXYZ", count);
                case RandomStringOptions.Chinese:
                    return NextChineseCount(count);
                /*case RandomStringOptions.ChinesePinyin:
                    return ConvertHelper.ChineseToPinyin(NextChineseCount(count));*/
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成一个指定位数的随机整数字符串
        /// </summary>
        /// <param name="count">位数</param>
        private static string NextIntCount(int count)
        {
            StringBuilder sb = new StringBuilder(count);
            Random rd = GetNewRandom();
            for (int i = 0; i < count; i++)
            {
                sb.Append(rd.Next(0, 9));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 生成随机字符串。（大小写字母、数字混合）
        /// </summary>
        /// <param name="patternStr">如：3456789abcdefgh</param>
        /// <param name="count">需要取的字符串位数</param>
        /// <returns></returns>
        private static string NextIntEnglishCount(string patternStr, int count)
        {
            char[] patternChar = patternStr.ToCharArray();
            Random rd = GetNewRandom();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                int index = rd.Next(0, patternChar.Length);
                sb.Append(patternChar[index]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成长度为N的随机汉字
        /// </summary>
        /// <param name="count">长度</param>
        /// <returns></returns>
        private static string NextChineseCount(int count)
        {
            Encoding gb = Encoding.GetEncoding("gb2312");
            object[] bytes = CreateRegionCode(count);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[]))));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将 
        /// 四个字节数组存储在object数组中。 
        /// 参数：strlength，代表需要产生的汉字个数 
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
        private static object[] CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = GetNewRandom();

            //定义一个object数组用来 
            object[] bytes = new object[strlength];

            /*
             每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bytes数组中 
             每个汉字有四个区位码组成 
             区位码第1位和区位码第2位作为字节数组第一个元素 
             区位码第3位和区位码第4位作为字节数组第二个元素 
            */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = GetNewRandom(); // 更换随机数发生器的 种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = GetNewRandom();
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = GetNewRandom();
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                // 定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                // 将两个字节变量存储在字节数组中 
                byte[] str_r = { byte1, byte2 };

                // 将产生的一个汉字的字节数组放入object数组中 
                bytes.SetValue(str_r, i);
            }

            return bytes;
        }

        #endregion

        #region 根据指定规则生成随机字符串 + string NextStringEx(string pattern)
        /// <summary>
        /// 根据指定规则生成随机字符串。分割标记“%”，i为数字；b为小写字母、B为大写字母、bB为随机大小写；c为汉字、C为汉字的拼音；hH为数字和字母混合、h为数字和字母混合的小写、H为数字字母混合的大写，要取出多少个在后面加上数字，自定义字符在前面加“#”。
        /// </summary>
        /// <param name="pattern">转换原型（如：“#百度%c2%i3%b5”代表 自定义字符“百度”+2个随机汉字+3个随机数字+5个随机小写字母）</param>
        /// <returns></returns>
        public static string NextStringEx(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException("pattern");

            MatchCollection mc = Regex.Matches(pattern, @"%([a-zA-Z]{1,2})([1-9]{1}[\d]{0,2})");
            if (mc.Count <= 0)
                return pattern;

            string resStr = pattern;
            foreach (Match item in mc)
            {
                if (item.Groups.Count == 3)
                {
                    int count = int.Parse(item.Groups[2].Value);//%i5中的5
                    switch (item.Groups[1].Value)//%后面的字母
                    {
                        case "i"://随机数字
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.Integer));
                            break;
                        case "b"://随机小写字母
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.EnglishLower));
                            break;
                        case "B"://随机大写字母
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.EnglishUpper));
                            break;
                        case "bB"://随机大小写字母
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.English));
                            break;
                        case "c"://随机汉字
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.Chinese));
                            break;
                        case "C"://随机汉字的拼音
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.ChinesePinyin));
                            break;
                        case "h"://随机数字与字母的小写
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.IntEnglishLower));
                            break;
                        case "H"://随机数字与字母的大写
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.IntEnglishUpper));
                            break;
                        case "hH"://随机数字与字母的混合
                            resStr = resStr.Replace(item.Groups[0].Value, NextString(count, RandomStringOptions.IntEnglish));
                            break;
                    }
                }
            }
            return resStr.Replace("#", "");
        }
        /// <summary>
        /// 根据指定规则生成随机字符串。分割标记“%”，i为数字；b为小写字母、B为大写字母、bB为随机大小写；c为汉字、C为汉字的拼音；hH为数字和字母混合、h为数字和字母混合的小写、H为数字字母混合的大写，要取出多少个在后面加上数字，自定义字符在前面加“#”。
        /// </summary>
        /// <param name="pattern">转换原型（如：“#百度%c2%i3%b5”代表 自定义字符“百度”+2个随机汉字+3个随机数字+5个随机小写字母）</param>
        /// <param name="min">指定最小位数</param>
        /// <param name="max">指定最大位数</param>
        /// <returns></returns>
        public static string NextStringEx(string pattern, int min, int max)
        {
            string strTemp = NextStringEx(pattern);
            if (min <= 0 || min >= strTemp.Length || max < min)
                throw new ArgumentOutOfRangeException("min,max");

            max = max > strTemp.Length ? strTemp.Length : max;
            int len = min == max ? max : GetNewRandom().Next(min, max + 1);
            strTemp = strTemp.Substring(0, len);
            return strTemp;
        }
        #endregion

        #region char NextSurname()

        /*private const string surnameStr =
            "赵钱孙李周吴郑王冯陈褚卫蒋沈韩杨朱秦尤许何吕施张孔曹严华金魏陶姜戚谢邹喻柏水窦章云苏潘葛奚范彭郎鲁韦昌马苗凤花方俞任袁柳酆鲍史唐费廉岑薛雷贺倪汤滕殷罗毕郝邬安常乐于时傅皮卞齐康伍余元卜顾孟平黄和穆萧尹姚邵湛汪祁毛禹狄米贝明臧计伏成戴谈宋茅庞熊纪舒屈项祝董梁杜阮蓝闵席季麻强贾路娄危江童颜郭梅盛林刁钟徐邱骆高夏蔡田樊胡凌霍虞万支柯昝管卢莫柯房裘缪干解应宗丁宣贲邓郁单杭洪包诸左石崔吉钮龚程嵇邢滑裴陆荣翁荀羊于惠甄曲家封芮羿储靳汲邴糜松井段富巫乌焦巴弓牧隗山谷车侯宓蓬全郗班仰秋仲伊宫宁仇栾暴甘钭历戎祖武符刘景詹束龙叶幸司韶郜黎蓟溥印宿白怀蒲邰从鄂索咸籍赖卓蔺屠蒙池乔阳郁胥能苍双闻莘党翟谭贡劳逄姬申扶堵冉宰郦雍却璩桑桂濮牛寿通边扈燕冀浦尚农温别庄晏柴瞿阎充慕连茹习宦艾鱼容向古易慎戈廖庾终暨居衡步都耿满弘匡国文寇广禄阙东欧殳沃利蔚越夔隆师巩厍聂晁勾敖融冷訾辛阚那简饶空曾毋沙乜养鞠须丰巢关蒯相查后荆红游竺权逮盍益桓公";

        /// <summary>
        /// 取一个随机的百家姓
        /// </summary>
        /// <param name="isPinyin">是否为返回拼音</param>
        /// <returns></returns>
        public static string NextSurname(bool isPinyin = false)
        {
            char[] chr = surnameStr.ToCharArray();
            int index = Next(0, chr.Length);
            return isPinyin ? ConvertHelper.ChineseToPinyin(chr[index].ToString()) : chr[index].ToString();
        }*/
        #endregion
    }

    /// <summary>
    /// 生成随机字符串选项
    /// </summary>
    public enum RandomStringOptions
    {
        /// <summary>
        /// 生成指定个数的随机整数字符串
        /// </summary>
        Integer,
        /// <summary>
        /// 生成指定个数的随机数字和英文字母大小写混和的字符串
        /// </summary>
        IntEnglish,
        /// <summary>
        /// 生成指定个数的随机数字和英文小写字母的字符串
        /// </summary>
        IntEnglishLower,
        /// <summary>
        /// 生成指定个数的随机数字和英文大写字母的字符串
        /// </summary>
        IntEnglishUpper,
        /// <summary>
        /// 生成指定个数的随机英文字母大小写混合的字符串
        /// </summary>
        English,
        /// <summary>
        /// 生成指定个数的随机英文小写字母的字符串
        /// </summary>
        EnglishLower,
        /// <summary>
        /// 生成指定个数的随机英文大写字母的字符串
        /// </summary>
        EnglishUpper,
        /// <summary>
        /// 生成指定个数的随机中文字符串
        /// </summary>
        Chinese,
        /// <summary>
        /// 生成指定个数的随机中文拼音字符串
        /// </summary>
        ChinesePinyin
    }
}
