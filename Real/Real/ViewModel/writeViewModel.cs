using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Real.Model;
using EDIDParser;
using System.IO;

namespace Real.ViewModel
{
    class writeViewModel : Notifier
    {
        sort sort = new sort();

        public writeViewModel()
        {
            FileName = "EDID";
        }
        EDIDModel write = new EDIDModel();
        public string EDID
        {
            get { return Global.EDID; }
        }
        public ICommand Path
        {
            get { return (this.write.writeParser) ?? (this.write.writeParser = new DelegateCommand(PATH)); }

        }
        void PATH()
        {
            try
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                dialog.ShowDialog();
                PathName = dialog.FileName;
            }
            catch { }
        }
        public string PathName
        {
            get { return write.parserName; }
            set
            {
                write.parserName = value;
                OnPropertyChenaged("PathName");
            }
        }
        public bool? TXT
        {
            set
            {
                write.txt = value;
                OnPropertyChenaged("TXT");
            }
        }
        public bool? BIN
        {
            set
            {
                write.bin = value;
                OnPropertyChenaged("BIN");
            }
        }
        public string FileName
        {
            get;
            set;
        }
        public ICommand SaveFile
        {
            get { return (this.write.saveFile) ?? (this.write.saveFile = new DelegateCommand(saveFiles)); }

        }
        void saveFiles()
        {
            if (write.txt == true || write.bin == true)
            {
                string p = write.parserName + "\\" + FileName;
                FileWrite(write.txt, write.bin, p, Global.EDID);
                Global.WirteFlag = "ON";
            }
            else
                System.Windows.MessageBox.Show("Don't Select File Extension");
        }

        private void FileWrite(bool? Txt, bool? Bin, string path, string edid)
        {
            try
            {
                if (Txt == true)
                {
                    path += ".txt";
                    if (File.Exists(path) == true)
                    {
                        System.Windows.MessageBox.Show("A file with the same name exists in the specified path.");
                        path = "";
                    }
                    System.IO.File.WriteAllText(path, edid, Encoding.Default);
                }
                else if (Bin == true)
                {
                    path += ".bin";
                    if (File.Exists(path))
                    {
                        System.Windows.MessageBox.Show("A file with the same name exists in the specified path.");
                        path = "";
                    }
                    else
                    {
                        sort.Sort(edid);
                        FileStream fs = File.Open(path, FileMode.Create);
                        using (BinaryWriter wr = new BinaryWriter(fs, Encoding.UTF7))
                        {
                            for (int i = 0; i < sort.strr.Length; i = i + 2)
                            {
                                wr.Write(Convert.ToByte(sort.strr.Substring(i, 2), 16));
                            }
                        }
                    }
                }
                System.Windows.MessageBox.Show("save file success.");
            }
            catch
            {
                System.Windows.MessageBox.Show("save file failing.");
            }
        }


    }
}
