using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public class BuyHouseSumResult : PostResultpInfo
    {
        public BuyHouseSumResult()
        {
        }

        public BuyHouseSumResult(PostResultStatus code)
            : base(code)
        {
        }

        public BuyHouseSumResult(PostResultStatus code, string msg)
            : base(code, msg)
        {
        }

        public string Html { get; set; }
    }
}