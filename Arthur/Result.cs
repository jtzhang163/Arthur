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

        public Result(string msg) : this(-1, msg)
        {

        }

        public Result(int code, string msg)
        {
            this.Code = code;
            this.Msg = msg;
        }

        public static Result OK
        {
            get
            {
                return new Result(0, string.Empty);
            }
        }

    }
}
