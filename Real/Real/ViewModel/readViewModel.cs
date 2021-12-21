using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Real.ViewModel
{
    class readViewModel : Notifier
    {
        public readViewModel()
        {
            
        }
        List<monitor> m = monitor.Monitor();
        public List<monitor> ListMonitor
        { 
            get
            {
                return m;
            }
            set
            {
                m = value;
                OnPropertyChenaged("ListBoxItem");
            }
        }
        int index;
        public int IndexMonitor
        {
            set
            {
                index = value;
                OnPropertyChenaged("IndexMonitor");
                OnIndexChanged();
            }
        }

        void OnIndexChanged()
        {
            ReadEdid = ChangeEdid.read[index];
        }
        string edid;
        public string ReadEdid
        {
            get { return edid; }
            set
            {
                edid = value;
                OnPropertyChenaged("ReadEdid");
            }
        }

        ICommand selectedid;
        public ICommand SelectEdid
        {
            get { return (this.selectedid) ?? (this.selectedid = new DelegateCommand(selected)); }

        }
        void selected()
        {
            Global.EDID = edid;
        }


    }
}
