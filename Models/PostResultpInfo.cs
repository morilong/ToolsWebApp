using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Haooyou.Tool.WebApp.Models
{
    public class PostResultpInfo
    {
        public PostResultpInfo()
        {
        }

        public PostResultpInfo(PostResultStatus code)
        {
            this.code = code;
            this.msg = code.ToString();
        }

        public PostResultpInfo(PostResultStatus code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public PostResultStatus code { get; set; }
        public string msg { get; set; }
    }
}