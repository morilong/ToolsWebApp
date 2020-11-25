namespace Haooyou.Tool.WebApp.Models
{
    public class ApiResultBase
    {
        public ApiResultBase(ApiResultCode code = ApiResultCode.Success)
        {
            this.code = code;
        }

        public ApiResultBase(ApiResultCode code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public ApiResultBase(ApiResultCode code, string msg, string subMsg)
        {
            this.code = code;
            this.msg = msg;
            this.sub_msg = subMsg;
        }

        public ApiResultCode code { get; set; }
        public string msg { get; set; }

        private string _subMsg = string.Empty;
        public string sub_msg
        {
            get { return _subMsg; }
            set { _subMsg = value; }
        }

        /// <summary>
        /// msg="操作成功"
        /// </summary>
        /// <returns></returns>
        public static ApiResultBase Success()
        {
            return Success(ApiBaseStatus.操作成功);
        }

        public static ApiResultBase Success(string msg)
        {
            return new ApiResultBase(ApiResultCode.Success, msg);
        }

        public static ApiResultBase Success<T>(T msg)
        {
            return new ApiResultBase(ApiResultCode.Success, msg.ToString());
        }

        public static ApiResultBase Success<T>(T msg, string subMsg)
        {
            return new ApiResultBase(ApiResultCode.Success, msg.ToString(), subMsg);
        }

        public static ApiResultBase Fail(string msg)
        {
            return new ApiResultBase(ApiResultCode.Fail, msg);
        }

        public static ApiResultBase Fail<T>(T msg)
        {
            return new ApiResultBase(ApiResultCode.Fail, msg.ToString());
        }

        public static ApiResultBase Fail<T>(T msg, string subMsg)
        {
            return new ApiResultBase(ApiResultCode.Fail, msg.ToString(), subMsg);
        }

        public static ApiResultBase Fail(string msg, string subMsg)
        {
            return new ApiResultBase(ApiResultCode.Fail, msg, subMsg);
        }

        public static ApiResultBase SuccessOrFail(int rows)
        {
            return rows > 0 ? Success(ApiBaseStatus.操作成功) : Fail(ApiBaseStatus.操作失败);
        }


    }

    public enum ApiResultCode
    {
        Success = 0,
        Fail = 1
    }
}