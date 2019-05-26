using Arthur.Core;
using Arthur.App.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public sealed class TaskManage : IManage<TaskLog>
    {

        public Result AddTaskLog()
        {
            try
            {
                using (var db = new Data.AppContext())
                {
                    db.TaskLogs.Add(new TaskLog()
                    {
                        ProcTrayId = Current.Task.ProcTrayId,
                        EndTime = DateTime.Now,
                        StartTime = Current.Task.StartTime,
                        StorageId = Current.Task.StorageId,
                        TaskType = Current.Task.Type,
                        UserId = Current.User.Id,
                        Time = DateTime.Now
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
            return Result.OK;
        }

        public Result Create(TaskLog t)
        {
            throw new NotImplementedException();
        }
    }
}
