using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Real.Model;
namespace Real.ViewModel
{
    class NameChangeViewModel : Notifier
    {
        EDIDModel NameChange = new EDIDModel();
        public string BName
        {
            get { return ChangeEdid.dess; }
        }
        public string AName
        {
            set
            {
                NameChange.aname = value;
                OnPropertyChenaged("AName");
            }
        }
        public ICommand ChangeName
        {
            get { return (this.NameChange.changename) ?? (this.NameChange.changename = new DelegateCommand(Name)); }

        }
        void Name()
        {
            try
            {
                sort sort = new sort(Global.EDID);
                char[] arr16 = sort.strc;
                if (NameChange.aname.Length > 13)
                {
                    NameChange.aname = null;
                }
                    byte[] arr_byteStr = Encoding.Default.GetBytes(NameChange.aname);
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
