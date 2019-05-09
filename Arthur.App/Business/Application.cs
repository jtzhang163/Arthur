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
            using (var db = new App.AppContext())
            {
                return db.Options.Where(o => o.Key == key).FirstOrDefault()?.Value;
            }
        }

        public static void SetOption(string key, string value)
        {
            SetOption(key, value, string.Empty);
        }

        public static Result SetOption(string key, string value, string remark)
        {
            try
            {
                using (var db = new App.AppContext())
                {
                    var option = db.Options.Where(o => o.Key == key).FirstOrDefault();
                    if (option != null)
                    {
                        db.Options.Where(o => o.Key == key).First().Value = value;
                    }
                    else
                    {
                        db.Options.Add(new Option() { Key = key, Value = value, Remark = remark });
                    }
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        LogHelper.WriteError(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                Logging.AddEvent(dbEx.Message, EventType.错误);
                return new Result(dbEx);
            }
            catch(Exception ex)
            {
                Logging.AddEvent(ex.Message, EventType.错误);
                return new Result(ex);
            }
            return Result.OK;
        }
    }
}
