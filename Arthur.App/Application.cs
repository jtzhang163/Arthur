using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    public class Application
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
    }
}
