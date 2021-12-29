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
                fileroad fileroad = new fileroad();
                sort filesort = new sort(fileroad.arr);
                Edid = filesort.str;
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
                sort sorts = new sort(menu.sorts);
                Global.length = sorts.strr.Length;
                CheckSums checkSums = new CheckSums(menu.sorts);
                menu.edid = checkSums.SortsStr;
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
            sort sort = new sort(Edid);
            Global.length = sort.strr.Length;
            EdidParser edidPath = new EdidParser(Edid);
            Path = edidPath.Pather;
            Global.EDID = menu.edid;
        }

    }
}
