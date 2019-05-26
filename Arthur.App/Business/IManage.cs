using Arthur.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Business
{
    public interface IManage<T>
    {
       Result Create(T t);
    }
}
