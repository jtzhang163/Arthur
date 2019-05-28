using Arthur.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public class SortPackViewModel : BindableObject
    {
        private string type;

        private int count;

        private int packId;

        public string Type
        {
            get => type;
            set
            {
                this.SetProperty(ref type, value);
            }
        }

        public int Count
        {
            get => count;
            set
            {
                this.SetProperty(ref count, value);
            }
        }

        public int PackId
        {
            get => packId;
            set
            {
                this.SetProperty(ref packId, value);
            }
        }
    }
}
