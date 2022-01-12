using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using EDIDParser;
using System.Windows;
namespace Real
{

    public class resoution
    {
        public char ms_ms = ' ';
        public char ms = ' ';
        public char ls_m = ' ';
        public char ls_l = ' ';
        public string sf = "";
        public int flag;
        public resoution(int value)
        {
            string str = Convert.ToString(value, 2);
            if (str.Length <= 12)
            {
                for (int i = str.Length; i < 12; i++)
                    str = "0" + str;
                string m = Convert.ToString(Convert.ToInt32(str.Substring(0, 4), 2), 16);
                string l_m = Convert.ToString(Convert.ToInt32(str.Substring(4, 4), 2), 16);
                string l_l = Convert.ToString(Convert.ToInt32(str.Substring(8, 4), 2), 16);
                ms = Convert.ToChar(m);
                ls_m = Convert.ToChar(l_m);
                ls_l = Convert.ToChar(l_l);
            }
            else
            {
                flag = 1;
            }
        }
        public resoution(int value, string b)
        {
            string str = Convert.ToString(value, 2);
            if (b == "H")
            {
                if (str.Length <= 10)
                {
                    for (int i = str.Length; i < 10; i++)
                        str = "0" + str;
                    string l_m = Convert.ToString(Convert.ToInt32(str.Substring(2, 4), 2), 16);
                    string l_l = Convert.ToString(Convert.ToInt32(str.Substring(6, 4), 2), 16);
                    ls_m = Convert.ToChar(l_m);
                    ls_l = Convert.ToChar(l_l);
                    sf = str.Substring(0, 2);
                }
                else
                {
                    flag = 1;
                }
            }
            else if (b == "V")
            {
                if (str.Length <= 6)
                {
                    for (int i = str.Length; i < 6; i++)
                        str = "0" + str;
                    string l_l = Convert.ToString(Convert.ToInt32(str.Substring(2, 4), 2), 16);
                    ls_l = Convert.ToChar(l_l);
                    sf = str.Substring(0, 2);
                }
                else
                {
                    flag = 1;
                }
            }
            else if (b == "P")
            {
                if (str.Length <= 16)
                {
                    for (int i = str.Length; i < 16; i++)
                        str = "0" + str;
                    string m_m = Convert.ToString(Convert.ToInt32(str.Substring(0, 4), 2), 16);
                    string m = Convert.ToString(Convert.ToInt32(str.Substring(4, 4), 2), 16);
                    string l_m = Convert.ToString(Convert.ToInt32(str.Substring(8, 4), 2), 16);
                    string l_l = Convert.ToString(Convert.ToInt32(str.Substring(12, 4), 2), 16);
                    ms_ms = Convert.ToChar(m_m);
                    ms = Convert.ToChar(m);
                    ls_m = Convert.ToChar(l_m);
                    ls_l = Convert.ToChar(l_l);
                }
                else
                {
                    flag = 1;
                    
                }
            }
        }
    }

    public class CheckSums : converter
    {    
        public string SortsStr;
        string strs;
        long checksum = 0;
        long checksum1 = 0;
        long che = 0;
        string chec = "";

        public CheckSums(string strss) : base(strss)
        {
            if (strss == null)
                return;
            sort sort = new sort();
            sort.Sort(strss);
            strss = sort.str2.Trim();
            int e1 = strss.Length;
            strs = strss;
            for (int j = 0; j < e1; j = j + 256)
            {
                checksum1 = 0;
                for (int i = 0; 254 > i; i = i + 2)
                {
                    checksum1 = checksum1 + imformation10[i + j] * 16 + imformation10[i + 1 + j];
                }

                che = checksum1 % 256;
                checksum = 256 - che;
                if (checksum == 256)
                    checksum = 0;
                chec = checksum.ToString("X");
                if (checksum < 16)
                    chec = "0" + chec;

                strs = strs.Remove((254 + j), 2);
                strs = strs.Insert((254 + j), chec);
            }
            sort check = new sort();
            check.Sort(strs);
            SortsStr = check.str;
        }
    }

    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChenaged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public DelegateCommand(Action exectue) : this(exectue, null)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            return this.canExecute();
        }

        public void Execute(object parameter)
        {
            this.execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }

    public class FileRoad
    {
        public string arr = "";
        public string n = "";
        string Extension = "";
        public FileRoad()
        {
            string l = "";
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Multiselect = false;
            opendlg.Filter = "Text, Binary Files(*.txt;*.bin) |*.txt;*.bin";
            if (opendlg.ShowDialog() != 0)
            {
                l = opendlg.FileName;
                int index = l.LastIndexOf("\\");
                int indexs = l.LastIndexOf(".");
                n = l.Substring(index + 1);
                Extension = l.Substring(indexs + 1);
            }   
            try
            {
                if (Extension == "txt")
                {
                    using (StreamReader sr = new StreamReader(new FileStream(l, FileMode.Open)))
                    {
                        arr = (sr.ReadToEnd()).ToUpper();
                    }
                }
                else if (Extension == "bin")
                {
                    byte[] buff = File.ReadAllBytes(l);
                    foreach (byte b in buff)
                    {
                        if (b > 15)
                            arr += Convert.ToString(b, 16);
                        else
                            arr += "0" + Convert.ToString(b, 16);
                    }
                }
            }
            catch
            {
            }
        }
    }

    public class Namechanger
    {
        public string edidcn = "";
        int count = 0;
        public char[] name = new char[36];
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
                        if (count == 1)
                            arr16[ChangeEdid.de[j] + i] = 'A';
                        else if (count % 2 == 1)
                            arr16[ChangeEdid.de[j] + i] = '2';
                        else if (count % 2 == 0)
                            arr16[ChangeEdid.de[j] + i] = '0';
                        count++;

                    }
                }
            }
            name = arr;
            foreach (char c in arr16)
                edidcn += c;
        }
    }
}
