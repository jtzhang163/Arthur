using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Data
{
    public class Context
    {
        public static AccountContext AccountContext = new AccountContext();
        public static OptionContext OptionContext = new OptionContext();
    }
}
