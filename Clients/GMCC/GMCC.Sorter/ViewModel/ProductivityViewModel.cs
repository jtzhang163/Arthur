using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public sealed class ProductivityViewModel
    {
        public string Date { get; set; }

        public int ProductivityAll { get; set; }

        public int ProductivityGood { get; set; }

        public int QualifiedRate
        {
            get
            {
                if (ProductivityAll == 0)
                    return 0;
                return ProductivityGood * 100 / ProductivityAll ;
            }
        }
    }
}
