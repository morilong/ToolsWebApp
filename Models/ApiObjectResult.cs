namespace Haooyou.Tool.WebApp.Models
{
    public class ApiObjectResult : ApiResultBase
    {
        public ApiObjectResult(ApiResultCode code = ApiResultCode.Success)
            : base(code)
        {
        }

        public ApiObjectResult(ApiResultCode code, string msg)
            : base(code, msg)
        {
        }

        public ApiObjectResult(ApiResultCode code, string msg, string subMsg)
            : base(code, msg, subMsg)
        {
        }

        public object data { get; set; }
    }
}