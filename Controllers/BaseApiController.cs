using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Haooyou.Tool.WebApp.Controllers
{
    public class BaseApiController : Controller
    {
        public virtual ActionResult Index()
        {
            return Content("not found action");
        }

        protected ActionResult GetJsonResult<T>(T obj, params JsonConverter[] converters)
        {
            var defConverters = new JsonConverter[]
            {
                new JsonDateTimeFormatConvert(),
                new JsonDecimalConvert(),
            };
            converters = converters == null ? defConverters : converters.Concat(defConverters).ToArray();
            return Content(JsonConvert.SerializeObject(obj, Formatting.None, converters));
        }

        protected ActionResult GetJsonResult(string json)
        {
            return Content(json, "application/json");
        }


    }
}