using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public enum PostResultStatus
    {
        失败 = 0,
        成功 = 1,
        建筑面积为必填项,
        毛坯价格为必填项,
        毛坯首付款百分比为必填项,
        毛坯按揭年数为必填项,
        毛坯利率上浮百分比为必填项,
        专项维修基金单价为必填项,
        装修部分的其他参数不能为空,
    }
}