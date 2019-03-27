using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur
{

    public class Result
    {

        public int Code { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }

        public bool IsOk
        {
            get
            {
                return this.Code == 0;
            }
        }

        public Result()
        {

        }

        public Result(Exception ex) : this("", ex)
        {

        }

        public Result(string title, Exception ex) : this(title + ":" + ex.Message)
        {
            //写入异常日志
            LogHelper.WriteError(ex);
        }

        public Result(string msg) : this(-1, msg)
        {

        }

        public Result(int code, string msg) : this(code, msg, null)
        {

        }

        public Result(int code, string msg, object data)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        public static Result OK
        {
            get
            {
                return new Result(0, string.Empty);
            }
        }

        public static Result OkHasData(object data)
        {
            return new Result(0, "", data);
        }

    }
}
