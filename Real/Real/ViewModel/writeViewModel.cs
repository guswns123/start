using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Real.ViewModel
{
    class writeViewModel : Notifier
    {
        public string EDID
        {
            get { return Global.EDID; }
        }
        ICommand path;
        public ICommand Path
        {
            get { return (this.path) ?? (this.path = new DelegateCommand(PATH)); }

        }
        void PATH()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true; 
            dialog.ShowDialog();
            PathName = dialog.FileName;
        }
        string pathname;
        public string PathName
        {
            get { return pathname; }
            set
            {
                pathname = value;
                OnPropertyChenaged("PathName");
            }
        }
        bool? txt;
        public bool? TXT
        {
            set
            {
                txt = value;
                OnPropertyChenaged("TXT");
            }
        }
        bool? bin;
        public bool? BIN
        {
            set
            {
                bin = value;
                OnPropertyChenaged("BIN");
            }
        }
        string filename = "edid";
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
        ICommand savefile;
        public ICommand SaveFile
        {
            get { return (this.savefile) ?? (this.savefile = new DelegateCommand(savefiles)); }

        }
        void savefiles()
        {
            if (txt == true || bin == true)
            {
                string edid = Global.EDID;
                Global.EDID = "";
                string p = pathname + "\\" + filename;
                fileroad fileroad = new fileroad(txt, bin, p, edid);
            }
            else
                System.Windows.MessageBox.Show("Don't Select File Extension");
        }


    }
}
