using Arthur.App.Data;
using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.Business
{
    public class Application
    {
        public static string GetOption(string key)
        {
            return Context.OptionContext.Options.Where(o => o.Key == key).FirstOrDefault()?.Value;
        }

        public static void SetOption(string key, string value)
        {
            SetOption(key, value, string.Empty);
        }

        public static Result SetOption(string key, string value, string remark)
        {

            Option option = Context.OptionContext.Options.Where(o => o.Key == key).FirstOrDefault();
            if (option != null)
            {
                Context.OptionContext.Options.Where(o => o.Key == key).First().Value = value;
            }
            else
            {
                Context.OptionContext.Options.Add(new Option { Key = key, Value = value, Remark = remark });
            }

            try
            {
                // 写数据库
                Context.OptionContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                return new Result(dbEx);
            }
            return Result.OK;
        }
    }
}
