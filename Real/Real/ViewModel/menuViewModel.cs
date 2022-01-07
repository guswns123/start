using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.MobileControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Real.Model;
using EDIDParser;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace Real.ViewModel
{
    public class menuViewModel : Notifier
    {
        EDIDModel menu = new EDIDModel();
        BackgroundWorker paser = new BackgroundWorker();

        public menuViewModel()
        {
            if (Global.WirteFlag == "ON")
                Global.EDID = "";
            if (Global.EDID != null && Global.EDID != "") 
                 Edid = Global.EDID;
            paser.DoWork += new DoWorkEventHandler(Pasing);

        }


        private void Pasing(object sender, DoWorkEventArgs e)
        {
            Initialization initialization = new Initialization();
            EdidParser edidPath = new EdidParser(Edid);
            Path = edidPath.Pather;
            ButtonVisibility();
            Global.EDID = menu.edid;
        }

        public ICommand BtnFile
        {
            get { return (this.menu.btnfile) ?? (this.menu.btnfile = new DelegateCommand(FileRoads)); }

        }
        void FileRoads()
        {
            try
            {                    
                sort fileSort = new sort(FileRoad());
                Edid = fileSort.str;
            }
            catch
            {
                System.Windows.MessageBox.Show("Select File Failing");
            }
        }

        public string Edid
        {
            get { return menu.edid; }
            set
            {
                    menu.sorts = value;
                    CheckSums checkSums = new CheckSums(menu.sorts);
                    menu.edid = checkSums.SortsStr;
                    OnPropertyChenaged("Edid");
                    paser.RunWorkerAsync();
            }
        }
        public string Path
        {
            get { return menu.parser; }
            set
            {
                menu.parser = value;
                OnPropertyChenaged("Path");
            }
        }

        void ButtonVisibility()
        {
            if (ChangeEdid.dtd > 0)
                ReVisibility = true;
            else
                ReVisibility = false;
            if (ChangeEdid.dess != "")
                NameVisibility = true;
            else
                NameVisibility = false;
            if (Edid != "")
                SaveVisibility = true;
            else
                SaveVisibility = false;
        }
        
        public bool PlusHex
        {
            get { return menu.plusHex; }
            set
            {
                menu.plusHex = value;
                Edid = PlusString(Edid);
            }
        }
        public bool PlusDot
        {
            get { return menu.plusDot; }
            set
            {
                menu.plusDot = value;
                Edid = PlusString(Edid);
            }
        }


        string PlusString(string args)
        {
            int count = 0;
            string str = "";
            args = args.Replace("\r", " ");
            string[] arraystr = args.Split(' ');
            foreach(string s in arraystr)
            {
                count++;
                if (s != "")
                {
                    if (PlusHex == true)
                        str += "0x";
                    str += s;
                    if (PlusDot == true && count != arraystr.Length - 1)
                        str += ",";

                    if (count % 16 == 0 && count != 0)
                        str = str + "\r";
                    else
                        str = str + " ";
                }
            }

            
            return str;

        }

        public string FileRoad()
        {
            string value = "";
            string Extension = "";
            string fileName = "";
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Multiselect = false;
            opendlg.Filter = "Text, Binary Files(*.txt;*.bin) |*.txt;*.bin";
            if (opendlg.ShowDialog() != 0)
            {
                fileName = opendlg.FileName;
                int index = fileName.LastIndexOf("\\");
                int indexs = fileName.LastIndexOf(".");
                Extension = fileName.Substring(indexs + 1);
            }
            try
            {
                if (Extension == "txt")
                {
                    using (StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open)))
                    {
                        value = (sr.ReadToEnd()).ToUpper();
                    }
                }
                else if (Extension == "bin")
                {
                    byte[] buff = File.ReadAllBytes(fileName);
                    foreach (byte b in buff)
                    {
                        if (b > 15)
                            value += Convert.ToString(b, 16);
                        else
                            value += "0" + Convert.ToString(b, 16);
                    }
                }
            }
            catch
            {
            }
            return value;
        }

        public bool ReVisibility
        {
            get { return menu.reVisibility; }
            set
            {
                menu.reVisibility = value;
                OnPropertyChenaged("ReVisibility");
            }
        }

        public bool NameVisibility
        {
            get { return menu.nameVisibility; }
            set
            {
                menu.nameVisibility = value;
                OnPropertyChenaged("NameVisibility");
            }
        }

        public bool SaveVisibility
        {
            get { return menu.saveVisibility; }
            set
            {
                menu.saveVisibility = value;
                OnPropertyChenaged("SaveVisibility");
            }
        }
    }
}
