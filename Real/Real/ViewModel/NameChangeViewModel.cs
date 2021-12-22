using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Real.ViewModel
{
    class NameChangeViewModel : Notifier
    {
        public string BName
        {
            get { return ChangeEdid.dess; }
        }
        string aname;
        public string AName
        {
            set
            {
                aname = value;
                OnPropertyChenaged("AName");
            }
        }
        ICommand changename;
        public ICommand ChangeName
        {
            get { return (this.changename) ?? (this.changename = new DelegateCommand(Name)); }

        }
        void Name()
        {
            try
            {
                sort sort = new sort(Global.EDID);
                char[] arr16 = sort.strc;
                if (aname.Length > 13)
                {
                    aname = null;
                }
                    byte[] arr_byteStr = Encoding.Default.GetBytes(aname);
                    Namechanger name = new Namechanger(arr16, arr_byteStr);
                    CheckSums checksums = new CheckSums(name.edidcn);
                    Global.EDID = checksums.SortsStr;
                    System.Windows.MessageBox.Show("Name Change Success");
                
            }
            catch
            {
                System.Windows.MessageBox.Show("Name Change Failling");
            }
        }

    }
}
