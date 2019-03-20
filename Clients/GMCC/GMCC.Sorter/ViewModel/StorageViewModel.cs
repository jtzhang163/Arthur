using Arthur.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public class StorageViewModel : BindableObject
    {

        public StorageViewModel(int col, int floor)
        {
            this.Column = col;
            this.Floor = floor;
        }

        private string name = null;

        public int Column { get; set; }

        public int Floor { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.SetProperty(ref name, value);
            }
        }
    }
}
