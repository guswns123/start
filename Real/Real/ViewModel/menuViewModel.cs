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
namespace Real.ViewModel
{
    public class menuViewModel : Notifier
    {
        EDIDModel menu = new EDIDModel();
        public menuViewModel()
        {
            if (Global.WirteFlag == "ON")
                Global.EDID = "";
            Initialization initialization = new Initialization();
            if (Global.EDID != null && Global.EDID != "") 
                 Edid = Global.EDID;
        }


        public ICommand BtnFile
        {
            get { return (this.menu.btnfile) ?? (this.menu.btnfile = new DelegateCommand(File)); }

        }
        void File()
        {
            try
            {
                FileRoad fileRoad = new FileRoad();
                sort fileSort = new sort(fileRoad.arr);
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
                menu.edid = PlusString(checkSums.SortsStr);
                OnPropertyChenaged("Edid");
                OnPathChanged();
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
        void OnPathChanged()
        {
            Initialization initialization = new Initialization();
            EdidParser edidPath = new EdidParser(Edid);
            Path = edidPath.Pather;
            Global.EDID = menu.edid;
        }
        
        public bool PlusHex
        {
            get { return menu.Plushex; }
            set
            {
                menu.Plushex = value;
                Edid = Edid;
            }
        }
        public bool PlusDot
        {
            get { return menu.Plusdot; }
            set
            {
                menu.Plusdot = value;
                Edid = Edid;
            }
        }


        string PlusString(string args)
        {
            string str = "";
            str = str.Replace(",", "");
            str = str.Replace("0X", "");
            string[] arraystr = args.Split(' ');
            foreach(string s in arraystr)
            {
                if (PlusHex == true)
                    str += "0x";
                str += s;
                if (PlusDot == true)
                    str += ",";
                str += " ";
            }
            
            return str;

        }
        

    }
}
