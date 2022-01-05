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
    class Namechanger
    {
        public string edidcn = "";
        int count = 0;
        public Namechanger(char[] arr16, byte[] arr_byteStr)
        {
            string[] result = new string[arr_byteStr.Length];
            char[] arr = new char[arr_byteStr.Length * 2];
            for (int i = 0; i < arr_byteStr.Length; i++)
            {
                result[i] = string.Format("{0:X2}", arr_byteStr[i]);
                arr[i * 2] = Convert.ToChar(result[i].Substring(0, 1));
                arr[(i * 2) + 1] = Convert.ToChar(result[i].Substring(1, 1));
            }
            for (int j = 0; j < ChangeEdid.des; j++)
            {
                for (int i = 10; i < 36; i++)
                {
                    try
                    {
                        arr16[ChangeEdid.de[j] + i] = arr[i - 10 + (j * 38)];
                    }
                    catch
                    {
                        count++;
                        if (count == 1)
                            arr16[ChangeEdid.de[j] + i] = 'A';
                        else if (count % 2 == 1)
                            arr16[ChangeEdid.de[j] + i] = '2';
                        else if (count % 2 == 0)
                            arr16[ChangeEdid.de[j] + i] = '0';

                    }
                }
            }
            foreach (char c in arr16)
                edidcn += c;
        }
    }
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
                    System.Windows.MessageBox.Show("Display has max length to 13 words");
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
