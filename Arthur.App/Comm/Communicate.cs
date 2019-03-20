using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Comm
{
    public interface ICommunicate
    {
       Result Communicate(string input);
    }

    public abstract class Communicate : ICommunicate
    {
        Result ICommunicate.Communicate(string input)
        {
            throw new NotImplementedException();
        }


        public Result Comm(EthernetCommor commor, string input)
        {
            return new Result();
        }

        public Result Comm(SerialCommor commor, string input)
        {
            return new Result();
        }
    }
}
