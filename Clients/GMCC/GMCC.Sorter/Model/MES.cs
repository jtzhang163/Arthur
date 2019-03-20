using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    public class MES : Server
    {
        public MES() : this(-1)
        {

        }

        public MES(int id)
        {
            Id = id;
        }
    }
}
