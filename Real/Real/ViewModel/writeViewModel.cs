using Microsoft.WindowsAPICodePack.Dialogs;
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
    class writeViewModel : Notifier
    {
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
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true; 
            dialog.ShowDialog();
            PathName = dialog.FileName;
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
            get { return (this.write.saveFile) ?? (this.write.saveFile = new DelegateCommand(savefiles)); }

        }
        void savefiles()
        {
            if (write.txt == true || write.bin == true)
            {
                string p = write.parserName + "\\" + FileName;
                FileRoad fileroad = new FileRoad(write.txt, write.bin, p, Global.EDID);
                Global.WirteFlag = "ON";
            }
            else
                System.Windows.MessageBox.Show("Don't Select File Extension");
        }




    }
}
