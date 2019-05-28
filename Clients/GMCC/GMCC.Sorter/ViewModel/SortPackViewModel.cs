using Arthur.App;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public class SortPackViewModel : BindableObject
    {
        private SortResult sortResult = SortResult.Unknown; 

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

        public SortResult SortResult
        {
            get => sortResult;
            set
            {
                this.SetProperty(ref sortResult, value);
            }
        }
    }
}
