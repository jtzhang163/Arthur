using Arthur.App;
using Arthur.App.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public abstract class CommorViewModel : BindableObject
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                SetProperty(ref isEnabled, value);
            }
        }


        private bool isAlive;
        public bool IsAlive
        {
            get => isAlive;
            set
            {
                SetProperty(ref isAlive, value);
            }
        }


        public Commor Commor { get; private set; }

        public CommorViewModel(Commor commor)
        {
            this.Commor = commor;
        }

        public virtual void Comm()
        {
            if (this.Commor.Connected)
            {

            }
        }
    }
}
