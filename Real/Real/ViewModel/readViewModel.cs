using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Real.Model;
namespace Real.ViewModel
{
    class readViewModel : Notifier
    {
        EDIDModel read = new EDIDModel();
        public readViewModel()
        {
            
        }

        public List<monitor> ListMonitor
        { 
            get
            {
                return read.m;
            }
            set
            {
                OnPropertyChenaged("ListBoxItem");
            }
        }
        public int IndexMonitor
        {
            set
            {
                read.indexmonitor = value;
                OnPropertyChenaged("IndexMonitor");
                OnIndexChanged();
            }
        }

        void OnIndexChanged()
        {
            ReadEdid = ChangeEdid.read[read.indexmonitor];
        }
        public string ReadEdid
        {
            get { return read.readedid; }
            set
            {
                read.readedid = value;
                OnPropertyChenaged("ReadEdid");
            }
        }

        public ICommand SelectEdid
        {         
            get { return (this.read.selectedid) ?? (this.read.selectedid = new DelegateCommand(selected)); }

        }
        void selected()
        {
            Global.EDID = read.readedid;
        }


    }
}
