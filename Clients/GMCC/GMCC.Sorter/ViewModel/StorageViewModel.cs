using Arthur.App;
using GMCC.Sorter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public class StorageViewModel : BindableObject
    {

        public int Id { get; set; }

        public int Column { get; set; }

        public int Floor { get; set; }

        private string name = null;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(name != value)
                {
                    Context.Storages.FirstOrDefault(o => o.Id == this.Id).Name = value;
                    Context.AppContext.SaveChanges();
                    this.SetProperty(ref name, value);
                }
            }
        }


        private string company = null;
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                if (company != value)
                {
                    Context.Storages.FirstOrDefault(o => o.Id == this.Id).Company = value;
                    Context.AppContext.SaveChanges();
                    this.SetProperty(ref company, value);
                }
            }
        }
    }
}
