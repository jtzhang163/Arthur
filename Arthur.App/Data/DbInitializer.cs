using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Data
{
    public class DbInitializer
    {
        public virtual void Initialize()
        {
            Database.SetInitializer(new AcconutInitializer());
        }
    }
}
