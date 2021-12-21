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

namespace Real.menu
{
    public class menuViewModel : Notifier
    {

        public menuViewModel()
        {
            Initialization initialization = new Initialization();
            if(Global.EDID != null && Global.EDID != "") 
                 Edid = Global.EDID;
        }

        ICommand btnfile;
        public ICommand BtnFile
        {
            get { return (this.btnfile) ?? (this.btnfile = new DelegateCommand(File)); }

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

        string edid;
        string sorts;
        public string Edid
        {
            get { return edid; }
            set
            {
                sorts = value;
                sort sort = new sort(sorts);
                Global.length = sort.strr.Length;
                CheckSums checkSums = new CheckSums(sorts);
                edid = checkSums.SortsStr;
                OnPropertyChenaged("Edid");
                OnPathChanged();
            }
        }
        string path = "values";
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChenaged("Path");
            }
        }
        void OnPathChanged()
        {
            Initialization initialization = new Initialization();
            sort sort = new sort(Edid);
            Global.length = sort.strr.Length;
            EdidPath edidPath = new EdidPath(Edid);
            Path = edidPath.Pather;
            Global.EDID = edid;
        }

    }
}
