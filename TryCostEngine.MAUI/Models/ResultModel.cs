namespace TryCostEngine.MAUI.Models
{
    /// <summary>
    /// 接口返回结果模型
    /// </summary>
    public class ApiResultModel<T>
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 返回数据条数
        /// </summary>
        public int Rows { get; set; }


        /// <summary>
        /// 接口返回结果模型构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ApiResultModel(int code = 200, string message = "OK")
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 接口返回结果模型构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public ApiResultModel(int code, string message, T data) : this(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 接口返回结果模型构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="rows"></param>
        /// <param name="data"></param>
        public ApiResultModel(int code, string message, int rows, T data) : this(code, message, data)
        {
            Rows = rows;
        }
    }

}
