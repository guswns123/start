﻿using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using EDIDParser;
namespace Real
{
    public class CheckSums : converter
    {
        public string SortsStr;
        public string strs;
        long checksum = 0;
        long checksum1 = 0;
        long che = 0;
        string chec = "";
        public CheckSums(string strss) : base(strss)
        {
            if (strss == null)
                return;
            sort sort = new sort(strss);
            strss = sort.strr.Trim();
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
            sort check = new sort(strs);
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
}
