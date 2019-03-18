using Arthur;
using Arthur.App.Data;
using Arthur.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.App;
using Arthur.App.Model;

namespace Arthur.Business
{
    public class Logging
    {
        public static Result AddEvent(string content, EventType type)
        {
            var log = new EventLog()
            {
                Content = content,
                EventType = type,
                Time = DateTime.Now
            };
            try
            {
                Context.EventLogs.Add(log);
                Context.LoggingContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            return Result.OK;
        }


        public static Result AddOplog(int userId, string content, OpType type)
        {
            var log = new Oplog()
            {
                UserId = userId,
                Content = content,
                OpType = type,
                Time = DateTime.Now
            };
            try
            {
                Context.Oplogs.Add(log);
                Context.LoggingContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            return Result.OK;
        }

        public static Result AddOplog(string content, OpType type)
        {
            return AddOplog(Current.User.Id, content, type);
        }
    }
}
