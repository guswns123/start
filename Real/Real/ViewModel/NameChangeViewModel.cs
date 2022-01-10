using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Real.Model;
using EDIDParser;
namespace Real.ViewModel
{
    class NameChangeViewModel : Notifier
    {

        sort sort = new sort();

        EDIDModel NameChange = new EDIDModel();

        public string BName
        {
            get { return ChangeEdid.dess; }
        }

        public string AName
        {
            set
            {
                NameChange.aName = value;
                OnPropertyChenaged("AName");
            }
        }

        public ICommand ChangeName
        {
            get { return (this.NameChange.changeName) ?? (this.NameChange.changeName = new DelegateCommand(Name)); }

        }

        void Name()
        {
            try
            {
                sort.Sort(Global.EDID);
                char[] arr16 = sort.strc;
                if (NameChange.aName.Length > 13)
                {
                    NameChange.aName = null;
                    System.Windows.MessageBox.Show("Display has max length to 13 words");
                }
                    byte[] arr_byteStr = Encoding.Default.GetBytes(NameChange.aName);
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
