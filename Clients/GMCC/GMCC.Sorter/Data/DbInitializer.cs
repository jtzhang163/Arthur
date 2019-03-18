using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Data
{
    public class DbInitializer : Arthur.App.Data.DbInitializer
    {
        public override void Initialize()
        {
          //  Database.SetInitializer(new MesInitializer());

            base.Initialize();
        }
    }
}
