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
            LogHelper.WriteInfo(string.Format("********【事件】：{0}", content));

            if (content.Length > 250)
            {
                content = content.Substring(0, 250);
            }

            var log = new EventLog()
            {
                Content = content,
                EventType = type,
                Time = DateTime.Now
            };
            try
            {
                using (var db = new App.AppContext())
                {
                    db.EventLogs.Add(log);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            return Result.OK;
        }


        public static Result AddOplog(int userId, string content, OpType type)
        {
            LogHelper.WriteInfo(string.Format("********【操作】：{0}", content));

            if (content.Length > 250)
            {
                content = content.Substring(0, 250);
            }

            var log = new Oplog()
            {
                UserId = userId,
                Content = content,
                OpType = type,
                Time = DateTime.Now
            };
            try
            {
                using (var db = new App.AppContext())
                {
                    db.Oplogs.Add(log);
                    db.SaveChanges();
                }
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
