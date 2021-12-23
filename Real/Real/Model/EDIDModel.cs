using Real.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Real.Model
{
    class EDIDModel
    {
        /////////////menu Model
        public ICommand btnfile { get; set; }

        public string edid { get; set; }

        public string sorts { get; set; }

        public string path { get; set; }
        /////////////
        /////////////read Model
        public List<monitor> m
        {
            get { return monitor.Monitor(); }
        }

        public int indexmonitor { get; set; }

        public ICommand selectedid { get; set; }

        public string readedid { get; set; }
        /// /////////
        /// <summary>
        /// //////////change Mdoel
        /// </summary>
        public string hsyns { get; set; }

        public string hfront { get; set; }

        public string hactive { get; set; }

        public string hback { get; set; }

        public string vsyns { get; set; }

        public string vfront { get; set; }

        public string vactive { get; set; }

        public string vback { get; set; }

        public string pi { get; set; }

        public int index { get; set; }

        public ICommand changeedid { get; set; }
        ////////////
        ////////////NameChange Model
        public string aname { get; set; }

        public ICommand changename { get; set; }
        ////////////
        ////////////write Model
        public ICommand writepath { get; set; }

        public string pathname { get; set; }

        public bool? txt { get; set; }

        public bool? bin { get; set; }

        public ICommand savefile { get; set; }
        ////////////
    }
}
