using Arthur.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Comm
{
    public interface IComm
    {
        Result Connect(Commor commor);
        Result EndConnect(Commor commor);
        Result Comm(Commor commor, string input);
    }
}
